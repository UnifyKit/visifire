/*   
    Copyright (C) 2008 Webyog Softworks Private Limited

    This file is a part of Visifire Charts.
 
    Visifire is a free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.
      
    You should have received a copy of the GNU General Public License
    along with Visifire Charts.  If not, see <http://www.gnu.org/licenses/>.
  
    If GPL is not suitable for your products or company, Webyog provides Visifire 
    under a flexible commercial license designed to meet your specific usage and 
    distribution requirements. If you have already obtained a commercial license 
    from Webyog, you can use this file under those license terms.
    
*/

#if WPF

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Globalization;
using System.Collections.ObjectModel;

#else
using System;
using System.Windows;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
#if !WP
using System.Windows.Browser;
#endif
#endif
using Visifire.Commons;
using System.Windows.Data;
using Visifire.Commons.Controls;

namespace Visifire.Charts
{
    /// <summary>
    /// Visifire.Charts.Axis class
    /// </summary>
#if SL &&!WP
    [System.Windows.Browser.ScriptableType]
#endif
    public partial class Axis : ObservableObject
    {
        #region Public Methods

        /// <summary>
        /// Initializes a new instance of the Visifire.Charts.Axis class
        /// </summary>
        public Axis()
        {
            // Apply default style from generic
#if WPF
            if (!_defaultStyleKeyApplied)
            {
                DefaultStyleKeyProperty.OverrideMetadata(typeof(Axis), new FrameworkPropertyMetadata(typeof(Axis)));
                _defaultStyleKeyApplied = true;
            }
#else
            DefaultStyleKey = typeof(Axis);
#endif

            // Initialize list of ChartGrid list
            Grids = new ChartGridCollection();

            // Initialize list of Ticks list 
            Ticks = new TicksCollection();

            // Initialize CustomLabel list
            CustomAxisLabels = new CustomAxisLabelsCollection();

            // Initialize AxisLabels element
            //AxisLabels = new AxisLabels();

            // Attach event handler on collection changed event with chart grid collection
            Grids.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(Grids_CollectionChanged);

            // Attach event handler on collection changed event with ticks collection
            Ticks.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(Ticks_CollectionChanged);

            CustomAxisLabels.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(CustomLabels_CollectionChanged);
            
            InternalAxisMinimum = Double.NaN;
            InternalAxisMaximum = Double.NaN;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _rootElement = GetTemplateChild(RootElementName) as Canvas;

            if (AxisLabels != null)
            {

#if WPF
                if (IsInDesignMode)
                    ObservableObject.RemoveElementFromElementTree(AxisLabels);
#endif

                if (!_rootElement.Children.Contains(AxisLabels))
                    _rootElement.Children.Add(AxisLabels);
            }

            foreach (CustomAxisLabels labels in CustomAxisLabels)
            {

#if WPF
                if (IsInDesignMode)
                    ObservableObject.RemoveElementFromElementTree(labels);
#endif

                if (!_rootElement.Children.Contains(labels))
                    _rootElement.Children.Add(labels);
            }

            foreach (ChartGrid grid in Grids)
            {

#if WPF
                if (IsInDesignMode)
                    ObservableObject.RemoveElementFromElementTree(grid);
#endif

                if (!_rootElement.Children.Contains(grid))
                    _rootElement.Children.Add(grid);
            }

            foreach (Ticks ticks in Ticks)
            {

#if WPF
                if (IsInDesignMode)
                    ObservableObject.RemoveElementFromElementTree(ticks);
#endif

                if (!_rootElement.Children.Contains(ticks))
                    _rootElement.Children.Add(ticks);
            }
        }

        public override void Bind()
        {
#if SL      
            Binding b = new Binding("Background");
            b.Source = this;
            this.SetBinding(InternalBackgroundProperty, b);

            b = new Binding("Padding");
            b.Source = this;
            this.SetBinding(InternalPaddingProperty, b);

            b = new Binding("Opacity");
            b.Source = this;
            this.SetBinding(InternalOpacityProperty, b);

            b = new Binding("MaxWidth");
            b.Source = this;
            this.SetBinding(InternalMaxWidthProperty, b);

            b = new Binding("MaxHeight");
            b.Source = this;
            this.SetBinding(InternalMaxHeightProperty, b);

            b = new Binding("MinWidth");
            b.Source = this;
            this.SetBinding(InternalMinWidthProperty, b);

            b = new Binding("MinHeight");
            b.Source = this;
            this.SetBinding(InternalMinHeightProperty, b);
#endif


        }

        /// <summary>
        /// Returns ScrollBarOffset corrosponding to a XValue
        /// </summary>
        /// <param name="axis">Axis</param>
        /// <param name="xValue">XValue</param>
        /// <returns></returns>
        public Double XValueToScrollBarOffset(Double xValue)
        {   
            if (AxisRepresentation == AxisRepresentations.AxisX)
            {   
                Double minPixelValue;
                Double maxPixelValue;
                Double scrollBarOffset;
                if (AxisOrientation == AxisOrientation.Horizontal)
                {
                    minPixelValue = Graphics.ValueToPixelPosition(0, ScrollableSize, InternalAxisMinimum, InternalAxisMaximum, xValue);
                    scrollBarOffset = minPixelValue / (ScrollableSize - Width);
                }
                else
                {
                    maxPixelValue = Graphics.ValueToPixelPosition(ScrollableSize, 0, InternalAxisMinimum, InternalAxisMaximum, xValue);
                    scrollBarOffset = maxPixelValue / (ScrollableSize - Height);
                    scrollBarOffset = 1 - scrollBarOffset;
                }

                // ScrollBarOffset value varies from 0 to 1
                return scrollBarOffset > 1 ? 1 : (scrollBarOffset < 0 ? 0 : scrollBarOffset);
            }
            else
                return Double.NaN;
        }

        /// <summary>
        /// Convert XValue to InternalXValue
        /// </summary>
        /// <param name="xValue">XValue as Object</param>
        /// <returns>InternalXValue as Double</returns>
        private Double XValueToInternalXValue(Object xValue)
        {
            if (IsDateTimeAxis)
                return DateTimeHelper.DateDiff(Convert.ToDateTime(xValue), MinDate, MinDateRange, MaxDateRange, InternalIntervalType, XValueType);
            else
                return Convert.ToDouble(xValue);
        }

        /// <summary>
        /// Calculate ZoomingScale from XValue range
        /// </summary>
        /// <param name="minXValueNumeric">Minimum XValue</param>
        /// <param name="maxXValueNumeric">Maximum XValue</param>
        /// <returns>ZoomingScale as Double</returns>
        private Double CalculateZoomingScaleFromXValueRange(Object minXValue, Object maxXValue)
        {   
            Chart chart = Chart as Chart;

            Double newMinXValue = XValueToInternalXValue(minXValue);
            Double newMaxXValue = XValueToInternalXValue(maxXValue);
            
            chart.ChartArea.AxisX._numericViewMinimum = Math.Min(newMaxXValue, newMinXValue);
            chart.ChartArea.AxisX._numericViewMaximum = Math.Max(newMaxXValue, newMinXValue);
            
            Double minPixelValue;
            Double maxPixelValue;

            if (chart.PlotDetails.ChartOrientation == ChartOrientationType.Vertical)
            {   
                minPixelValue = Graphics.ValueToPixelPosition(0, ScrollableSize, InternalAxisMinimum, InternalAxisMaximum, newMinXValue);
                maxPixelValue = Graphics.ValueToPixelPosition(0, ScrollableSize, InternalAxisMinimum, InternalAxisMaximum, newMaxXValue);
            }
            else
            {   
                minPixelValue = Graphics.ValueToPixelPosition(ScrollableSize, 0, InternalAxisMinimum, InternalAxisMaximum, newMinXValue);
                maxPixelValue = Graphics.ValueToPixelPosition(ScrollableSize, 0, InternalAxisMinimum, InternalAxisMaximum, newMaxXValue);
            }
            
            Double viewPortPixDiff = ScrollableSize;    // Pixel diff between ViewMin and ViewMax
            Double xValuePixDiff = Math.Abs(maxPixelValue - minPixelValue);

            if (viewPortPixDiff < xValuePixDiff)
                viewPortPixDiff = xValuePixDiff;

            Double viewportSize;
            if (AxisOrientation == AxisOrientation.Horizontal)
                viewportSize = Width;
            else
                viewportSize = Height;

            Double zoomingScale = (xValuePixDiff / viewPortPixDiff);
            zoomingScale = ValidateAndUpdateMaxZoomingScale(zoomingScale, viewportSize);

            return zoomingScale;
        }

        /// <summary>
        /// Zooming Scale should not contain such value so that PlotArea size exceeds the Max_PLOTAREA_SIZE
        /// </summary>
        /// <returns></returns>
        private Double ValidateAndUpdateMaxZoomingScale(Double zoomingScale, Double viewPortSize)
        {
            if (viewPortSize / zoomingScale > ChartArea.MAX_PLOTAREA_SIZE)
            {
                zoomingScale = viewPortSize / ChartArea.MAX_PLOTAREA_SIZE;
            }

            return zoomingScale;
        }

        internal void ZoomIn(Object minXValue, Object maxXValue)
        {
            Chart chart = Chart as Chart;

            if (chart == null)
                return;

            ScrollBarElement.Scale = _internalZoomingScale = CalculateZoomingScaleFromXValueRange(minXValue, maxXValue);

            chart.ChartArea.AxisX.ScrollBarElement._isZoomedUsingZoomRect = true;

            chart.ChartArea.OnScrollBarScaleChanged(chart);

            chart.Dispatcher.BeginInvoke(new Action(delegate()
            {
                Double minValue = XValueToInternalXValue(minXValue);
                Double maxValue = XValueToInternalXValue(maxXValue);
                
                // Swap min and max values if min value is greater than max value
                if (minValue > maxValue)
                {   
                    Double temp = minValue;
                    minValue = maxValue;
                    maxValue = temp;
                }

                chart.ChartArea.AxisX.ScrollBarElement.UpdateScale(_internalZoomingScale);

                Double xValue = (AxisOrientation == AxisOrientation.Horizontal) ? minValue : maxValue;
                Double scrollBarOffset = XValueToScrollBarOffset(xValue);
                chart.ChartArea.AxisX.ScrollBarOffset = scrollBarOffset;

                if (Math.Round(ScrollBarElement.Scale, 4) >= 1)
                    ResetZoomState(Chart as Chart, true);
            }));
        }

        /// <summary>
        /// Zoom the chart by supplying minimum XValue and maximum XValue
        /// </summary>
        /// <param name="minXValueNumeric">Min XValue</param>
        /// <param name="maxXValueNumeric">Max XValue</param>
        public void Zoom(Object minXValue, Object maxXValue)
        {
            if (Chart != null && (Chart as Chart).PlotDetails != null && (Chart as Chart).PlotDetails.ChartOrientation == ChartOrientationType.Circular)
                return;

            System.Diagnostics.Debug.WriteLine("minXValue : " + minXValue + ", MaxXValue : " + maxXValue);

            Chart chart = (Chart as Chart);

            if (chart.ZoomingEnabled)
            {
                if(_oldZoomState.MinXValue != null && _oldZoomState.MaxXValue != null)
                    _zoomStateStack.Push(new ZoomState(_oldZoomState.MinXValue, _oldZoomState.MaxXValue));
                
                ZoomIn(minXValue, maxXValue);

                _oldZoomState.MinXValue = minXValue;
                _oldZoomState.MaxXValue = maxXValue;
                
                chart._zoomOutTextBlock.Visibility = Visibility.Visible;
                chart._zoomIconSeparater.Visibility = Visibility.Visible;
                chart._showAllTextBlock.Visibility = Visibility.Visible;
            }
        }

        internal void ZoomOutIconImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {   
            if (_zoomStateStack.Count > 0)
            {
                _zoomState = _zoomStateStack.Pop();
                ZoomIn(_zoomState.MinXValue, _zoomState.MaxXValue);

                Chart chart = (Chart as Chart);

                if (_zoomStateStack.Count == 0)
                {
                    chart._zoomOutTextBlock.Visibility = Visibility.Collapsed;
                    chart._showAllTextBlock.Visibility = Visibility.Collapsed;
                    chart._zoomIconSeparater.Visibility = Visibility.Collapsed;
                }

                FireZoomEvent(_zoomState, e);
            }
        }

        internal void ShowAllIconImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ZoomIn(_initialState.MinXValue, _initialState.MaxXValue);

            _showAllState = true;

            Chart chart = (Chart as Chart);

            chart._zoomOutTextBlock.Visibility = Visibility.Collapsed;
            chart._showAllTextBlock.Visibility = Visibility.Collapsed;
            chart._zoomIconSeparater.Visibility = Visibility.Collapsed;

            FireZoomEvent(_initialState, e);
        }

        /// <summary>
        /// Get top of Axis
        /// </summary>
        /// <param name="axis"></param>
        /// <returns></returns>
        internal static Double GetAxisTop(Axis axis)
        {
            Double top = 0;
            Chart chart = axis.Chart as Chart;
            if (axis.AxisRepresentation == AxisRepresentations.AxisY)
            {
                if (axis.AxisOrientation == AxisOrientation.Vertical)
                {
                    if (axis.AxisType == AxisTypes.Primary || axis.AxisType == AxisTypes.Secondary)
                    {
                        top = chart.Padding.Top + chart._topOuterPanel.ActualHeight
                            + chart._topAxisGrid.ActualHeight + chart._topOffsetGrid.ActualHeight
                        + chart.BorderThickness.Top;
                    }
                }
                else
                {
                    if (axis.AxisType == AxisTypes.Primary)
                    {
                        top = chart.Padding.Top + chart._topOuterPanel.ActualHeight
                            + chart._topAxisGrid.ActualHeight
                            + chart.ChartArea.PlotAreaCanvas.Height
                            + chart._topOffsetGrid.ActualHeight
                            + chart.BorderThickness.Top;
                    }
                    else
                    {
                        top = chart.Padding.Top + chart._topOuterPanel.ActualHeight
                            + chart._topOffsetGrid.ActualHeight + chart.BorderThickness.Top;
                    }
                }
            }
            else
            {
                if (axis.AxisOrientation == AxisOrientation.Vertical)
                {
                    if (axis.AxisType == AxisTypes.Primary)
                    {
                        top = chart.Padding.Top + chart._topOuterPanel.ActualHeight 
                            + chart._topAxisGrid.ActualHeight
                            + chart._topOffsetGrid.ActualHeight + chart.BorderThickness.Top;
                    }
                }
                else
                {
                    if (axis.AxisType == AxisTypes.Primary)
                    {
                        top = chart.Padding.Top + chart._topOuterPanel.ActualHeight 
                            + chart._topAxisGrid.ActualHeight + chart.ChartArea.PlotAreaCanvas.Height
                            + chart._topOffsetGrid.ActualHeight + chart.BorderThickness.Top;
                    }
                    else
                    {
                        top = chart.Padding.Top + chart._topOuterPanel.ActualHeight
                            + chart._topOffsetGrid.ActualHeight + chart.BorderThickness.Top;
                    }
                }
            }

            return top;
        }

        internal static Double GetAxisLeft(Axis axis)
        {
            Double left = 0;
            Chart chart = axis.Chart as Chart;
            if (axis.AxisRepresentation == AxisRepresentations.AxisY)
            {
                if (axis.AxisOrientation == AxisOrientation.Vertical)
                {
                    if (axis.AxisType == AxisTypes.Primary)
                    {
                        left = chart.Padding.Left + chart._leftOuterPanel.ActualWidth
                            + chart._leftOffsetGrid.ActualWidth + chart.BorderThickness.Left;
                    }
                    else
                    {
                        left = chart.Padding.Left + chart._leftOuterPanel.ActualWidth
                            + chart._leftAxisGrid.ActualWidth + chart.ChartArea.PlotAreaCanvas.Width 
                            + chart._leftOffsetGrid.ActualWidth + chart.BorderThickness.Left;
                    }
                }
                else
                {
                    if (axis.AxisType == AxisTypes.Primary || axis.AxisType == AxisTypes.Secondary)
                    {
                        left = chart.Padding.Left + chart._leftOuterPanel.ActualWidth
                            + chart._leftAxisGrid.ActualWidth 
                            + chart._leftOffsetGrid.ActualWidth + chart.BorderThickness.Left;
                    }
                }
            }
            else
            {
                if (axis.AxisOrientation == AxisOrientation.Vertical)
                {
                    if (axis.AxisType == AxisTypes.Primary)
                    {
                        left = chart.Padding.Left + chart._leftOuterPanel.ActualWidth
                            + chart._leftOffsetGrid.ActualWidth + chart.BorderThickness.Left;
                    }
                }
                else
                {
                    if (axis.AxisType == AxisTypes.Primary || axis.AxisType == AxisTypes.Secondary)
                    {
                        left = chart.Padding.Left + chart._leftOuterPanel.ActualWidth
                            + chart._leftAxisGrid.ActualWidth 
                            + chart._leftOffsetGrid.ActualWidth + chart.BorderThickness.Left;
                    }
                }
            }

            return left;
        }


        #endregion

        #region Public Properties


        /// <summary>
        /// Identifies the Visifire.Charts.Axis.ViewportRangeEnabled dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Axis.ViewportRangeEnabled dependency property.
        /// </returns>
        public static DependencyProperty ViewportRangeEnabledProperty = DependencyProperty.Register
            ("ViewportRangeEnabled",
            typeof(Boolean),
            typeof(Axis),
            new PropertyMetadata(false, OnViewportRangeEnabledPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.Axis.AxisLabels dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Axis.AxisLabels dependency property.
        /// </returns>
        public static DependencyProperty AxisLabelsProperty = DependencyProperty.Register
            ("AxisLabels",
            typeof(AxisLabels),
            typeof(Axis),
            new PropertyMetadata(OnAxisLabelsPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.Axis.HrefTarget dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Axis.HrefTarget dependency property.
        /// </returns>
        public static readonly DependencyProperty HrefTargetProperty = DependencyProperty.Register
            ("HrefTarget",
            typeof(HrefTargets),
            typeof(Axis),
            new PropertyMetadata(OnHrefTargetChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.Axis.Href dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Axis.Href dependency property.
        /// </returns>
        public static readonly DependencyProperty HrefProperty = DependencyProperty.Register
            ("Href",
            typeof(String),
            typeof(Axis),
            new PropertyMetadata(OnHrefChanged));

#if WPF

        /// <summary>
        /// Identifies the Visifire.Charts.Axis.MaxHeight dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Axis.MaxHeight dependency property.
        /// </returns>
        public new static readonly DependencyProperty MaxHeightProperty = DependencyProperty.Register
            ("MaxHeight",
            typeof(Double),
            typeof(Axis),
            new PropertyMetadata(Double.PositiveInfinity, OnMaxHeightPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.Axis.MinHeight dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Axis.MinHeight dependency property.
        /// </returns>
        public new static readonly DependencyProperty MinHeightProperty = DependencyProperty.Register
            ("MinHeight",
            typeof(Double),
            typeof(Axis),
            new PropertyMetadata(OnMinHeightPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.Axis.MinWidth dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Axis.MinWidth dependency property.
        /// </returns>
        public new static readonly DependencyProperty MinWidthProperty = DependencyProperty.Register
            ("MinWidth",
            typeof(Double),
            typeof(Axis),
            new PropertyMetadata(OnMinWidthPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.Axis.MaxWidth dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Axis.MaxWidth dependency property.
        /// </returns>
        public new static readonly DependencyProperty MaxWidthProperty = DependencyProperty.Register
            ("MaxWidth",
            typeof(Double),
            typeof(Axis),
            new PropertyMetadata(Double.PositiveInfinity, OnMaxWidthPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.Axis.Padding dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Axis.Padding dependency property.
        /// </returns>
        public new static readonly DependencyProperty PaddingProperty = DependencyProperty.Register
             ("Padding",
             typeof(Thickness),
             typeof(Axis),
             new PropertyMetadata(OnPaddingPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.Axis.Background dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Axis.Background dependency property.
        /// </returns>
        public new static readonly DependencyProperty BackgroundProperty = DependencyProperty.Register
            ("Background",
            typeof(Brush),
            typeof(Axis),
            new PropertyMetadata(OnBackgroundPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.Axis.Opacity dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Axis.Opacity dependency property.
        /// </returns>
        public new static readonly DependencyProperty OpacityProperty = DependencyProperty.Register
            ("Opacity",
            typeof(Double),
            typeof(Axis),
            new PropertyMetadata(1.0, OnOpacityPropertyChanged));
#endif

        /// <summary>
        /// Identifies the Visifire.Charts.Axis.Interval dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Axis.Interval dependency property.
        /// </returns>
        public static readonly DependencyProperty IntervalProperty = DependencyProperty.Register
            ("Interval",
            typeof(Nullable<Double>),
            typeof(Axis),
            new PropertyMetadata(OnIntervalPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.Axis.LineColor dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Axis.LineColor dependency property.
        /// </returns>
        public static readonly DependencyProperty LineColorProperty = DependencyProperty.Register
            ("LineColor",
            typeof(Brush),
            typeof(Axis),
            new PropertyMetadata(OnLineColorPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.Axis.LineThickness dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Axis.LineThickness dependency property.
        /// </returns>
        public static readonly DependencyProperty LineThicknessProperty = DependencyProperty.Register
            ("LineThickness",
            typeof(Nullable<Double>),
            typeof(Axis),
            new PropertyMetadata(OnLineThicknessPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.Axis.LineStyle dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Axis.LineStyle dependency property.
        /// </returns>
        public static readonly DependencyProperty LineStyleProperty = DependencyProperty.Register
            ("LineStyle",
            typeof(LineStyles),
            typeof(Axis),
            new PropertyMetadata(OnLineStylePropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.Axis.Title dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Axis.Title dependency property.
        /// </returns>
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register
            ("Title",
            typeof(String),
            typeof(Axis),
            new PropertyMetadata(OnTitlePropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.Axis.TitleFontColor dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Axis.TitleFontColor dependency property.
        /// </returns>
        public static readonly DependencyProperty TitleFontColorProperty = DependencyProperty.Register
            ("TitleFontColor",
            typeof(Brush),
            typeof(Axis),
            new PropertyMetadata(OnTitleFontColorPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.Axis.TitleFontFamily dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Axis.TitleFontFamily dependency property.
        /// </returns>
        public static readonly DependencyProperty TitleFontFamilyProperty = DependencyProperty.Register
            ("TitleFontFamily",
            typeof(FontFamily),
            typeof(Axis),
            new PropertyMetadata(OnTitleFontFamilyPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.Axis.TitleFontSize dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Axis.TitleFontSize dependency property.
        /// </returns>
        public static readonly DependencyProperty TitleFontSizeProperty = DependencyProperty.Register
            ("TitleFontSize",
            typeof(Double),
            typeof(Axis),
            new PropertyMetadata(OnTitleFontSizePropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.Axis.TitleFontStyle dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Axis.TitleFontStyle dependency property.
        /// </returns>
        public static readonly DependencyProperty TitleFontStyleProperty = DependencyProperty.Register
            ("TitleFontStyle",
            typeof(FontStyle),
            typeof(Axis),
            new PropertyMetadata(OnTitleFontStylePropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.Axis.TitleFontWeight dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Axis.TitleFontWeight dependency property.
        /// </returns>
        public static readonly DependencyProperty TitleFontWeightProperty = DependencyProperty.Register
            ("TitleFontWeight",
            typeof(FontWeight),
            typeof(Axis),
            new PropertyMetadata(OnTitleFontWeightPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.Axis.AxisType dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Axis.AxisType dependency property.
        /// </returns>
        public static readonly DependencyProperty AxisTypeProperty = DependencyProperty.Register
            ("AxisType",
            typeof(AxisTypes),
            typeof(Axis),
            new PropertyMetadata(OnAxisTypePropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.Axis.AxisMaximum dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Axis.AxisMaximum dependency property.
        /// </returns>
        public static readonly DependencyProperty AxisMaximumProperty = DependencyProperty.Register
            ("AxisMaximum",
            typeof(Object),
            typeof(Axis),
            new PropertyMetadata(null, OnAxisMaximumPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.Axis.AxisMinimum dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Axis.AxisMinimum dependency property.
        /// </returns>
        public static readonly DependencyProperty AxisMinimumProperty = DependencyProperty.Register
            ("AxisMinimum",
            typeof(Object),
            typeof(Axis),
            new PropertyMetadata(null, OnAxisMinimumPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.Axis.IncludeZero dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Axis.IncludeZero dependency property.
        /// </returns>
        public static readonly DependencyProperty IncludeZeroProperty = DependencyProperty.Register
            ("IncludeZero",
            typeof(Boolean),
            typeof(Axis),
            new PropertyMetadata(OnIncludeZeroPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.Axis.StartFromZero dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Axis.StartFromZero dependency property.
        /// </returns>
        public static readonly DependencyProperty StartFromZeroProperty = DependencyProperty.Register
            ("StartFromZero",
            typeof(Nullable<Boolean>),
            typeof(Axis),
            new PropertyMetadata(OnStartFromZeroPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.Axis.Prefix dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Axis.Prefix dependency property.
        /// </returns>
        public static readonly DependencyProperty PrefixProperty = DependencyProperty.Register
            ("Prefix",
            typeof(String),
            typeof(Axis),
            new PropertyMetadata(OnPrefixPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.Axis.Suffix dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Axis.Suffix dependency property.
        /// </returns>
        public static readonly DependencyProperty SuffixProperty = DependencyProperty.Register
            ("Suffix",
            typeof(String),
            typeof(Axis),
            new PropertyMetadata(OnSuffixPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.Axis.ScalingSet dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Axis.ScalingSet dependency property.
        /// </returns>
        public static readonly DependencyProperty ScalingSetProperty = DependencyProperty.Register
            ("ScalingSet",
            typeof(String),
            typeof(Axis),
            new PropertyMetadata(OnScalingSetPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.Axis.ValueFormatString dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Axis.ValueFormatString dependency property.
        /// </returns>
        public static readonly DependencyProperty ValueFormatStringProperty = DependencyProperty.Register
            ("ValueFormatString",
            typeof(String),
            typeof(Axis),
            new PropertyMetadata(OnValueFormatStringPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.Axis.ScrollBarOffset dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Axis.ScrollBarOffset dependency property.
        /// </returns>
        public static readonly DependencyProperty ScrollBarOffsetProperty = DependencyProperty.Register
           ("ScrollBarOffset",
           typeof(Double),
           typeof(Axis),
           new PropertyMetadata(Double.NaN, OnScrollBarOffsetChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.Axis.ScrollBarScale dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Axis.ScrollBarScale dependency property.
        /// </returns>
        public static readonly DependencyProperty ScrollBarScaleProperty =
            DependencyProperty.Register("ScrollBarScale",
            typeof(double),
            typeof(Axis),
            new PropertyMetadata(Double.NaN, OnScrollBarScalePropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.Axis.ClosestPlotDistance dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Axis.ClosestPlotDistance dependency property.
        /// </returns>
        public static readonly DependencyProperty ClosestPlotDistanceProperty =
            DependencyProperty.Register("ClosestPlotDistance",
            typeof(double),
            typeof(Axis),
            new PropertyMetadata(Double.NaN, OnClosestPlotDistancePropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.Axis.ScrollBarSize dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Axis.ScrollBarSize dependency property.
        /// </returns>
        public static readonly DependencyProperty ScrollBarSizeProperty = DependencyProperty.Register
           ("ScrollBarSize",
           typeof(Double),
           typeof(Axis),
           new PropertyMetadata(Double.NaN, OnScrollBarSizeChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.Axis.Enabled dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Axis.Enabled dependency property.
        /// </returns>
        public static readonly DependencyProperty EnabledProperty = DependencyProperty.Register
            ("Enabled",
            typeof(Nullable<Boolean>),
            typeof(Axis),
            new PropertyMetadata(OnEnabledPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.Axis.InternalIntervalType dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Axis.InternalIntervalType dependency property.
        /// </returns>
        public static readonly DependencyProperty IntervalTypeProperty = DependencyProperty.Register
            ("IntervalType",
            typeof(IntervalTypes),
            typeof(Axis),
            new PropertyMetadata(OnIntervalTypePropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.Axis.Logarithmic dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Axis.Logarithmic dependency property.
        /// </returns>
        public static readonly DependencyProperty LogarithmicProperty = DependencyProperty.Register
            ("Logarithmic",
            typeof(Boolean),
            typeof(Axis),
            new PropertyMetadata(false, OnLogarithmicPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.Axis.LogarithmBase dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Axis.LogarithmBase dependency property.
        /// </returns>
        public static readonly DependencyProperty LogarithmBaseProperty = DependencyProperty.Register
            ("LogarithmBase",
            typeof(Double),
            typeof(Axis),
            new PropertyMetadata(10.0, OnLogarithmBasePropertyChanged));

        /// <summary>
        /// Gets axis data view maximum XValue position.
        /// </summary>
        /// <Returns>
        /// Axis data view maximum XValue position.
        /// </Returns>
        public Object ViewMaximum
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets axis data view minimum XValue position.
        /// </summary>
        /// <Returns>
        /// Axis data view minimum XValue position.
        /// </Returns>
        public Object ViewMinimum
        {
            get;
            internal set;
        }

        /// <summary>
        /// Height of the Axis
        /// </summary>
#if SL &&!WP
        [ScriptableMember]
#endif
        public new Double Height
        {
            get;
            internal set;
        }

        /// <summary>
        /// Width of the Axis
        /// </summary>
#if SL &&!WP
        [ScriptableMember]
#endif
        public new Double Width
        {
            get;
            internal set;
        }

        /// <summary>
        /// MaxHeight of the Axis
        /// </summary>
        public new Double MaxHeight
        {
            get
            {
                return (Double)GetValue(MaxHeightProperty);
            }
            set
            {
#if SL
                if (MaxHeight != value)
                {
                    InternalMaxHeight = value;
                    SetValue(MaxHeightProperty, value);
                    FirePropertyChanged(VcProperties.MaxHeight);
                }
#else
                SetValue(MaxHeightProperty, value);
#endif
            }
        }

        /// <summary>
        /// MaxWidth of the Axis
        /// </summary>
        public new Double MaxWidth
        {
            get
            {
                return (Double)GetValue(MaxWidthProperty);
            }
            set
            {
#if SL
                if (MaxWidth != value)
                {
                    InternalMaxWidth = value;
                    SetValue(MaxWidthProperty, value);
                    FirePropertyChanged(VcProperties.MaxWidth);
                }
#else
                SetValue(MaxWidthProperty, value);
#endif
            }
        }

        /// <summary>
        /// MinHeight of the Axis
        /// </summary>
        public new Double MinHeight
        {
            get
            {
                return (Double)GetValue(MinHeightProperty);
            }
            set
            {
#if SL
                if (MinHeight != value)
                {
                    InternalMinHeight = value;
                    SetValue(MinHeightProperty, value);
                    FirePropertyChanged(VcProperties.MinHeight);
                }
#else
                SetValue(MinHeightProperty, value);
#endif
            }
        }

        /// <summary>
        /// MinWidth of the Axis
        /// </summary>
        public new Double MinWidth
        {
            get
            {
                return (Double)GetValue(MinWidthProperty);
            }
            set
            {
#if SL
                if (MinWidth != value)
                {
                    InternalMinWidth = value;
                    SetValue(MinWidthProperty, value);
                    FirePropertyChanged(VcProperties.MinWidth);
                }
#else
                SetValue(MinWidthProperty, value);
#endif
            }
        }

        /// <summary>
        /// Sets whether the Axis scale is Logarithmic. The default value is False.
        /// </summary>
        public Boolean Logarithmic
        {
            get
            {
                return (Boolean)GetValue(LogarithmicProperty);
            }
            set
            {

                SetValue(LogarithmicProperty, value);
            }
        }

        /// <summary>
        /// Sets the base of the Logarithm for Logarithmic Axis. The default value is 10.
        /// </summary>
        public Double LogarithmBase
        {
            get
            {
                return (Double)GetValue(LogarithmBaseProperty);
            }
            set
            {

                SetValue(LogarithmBaseProperty, value);
            }
        }

        /// <summary>
        /// Get or set the "AxisLabels element" property of the axis
        /// </summary>
        public IntervalTypes IntervalType
        {
            get
            {
                return (IntervalTypes)GetValue(IntervalTypeProperty);
            }
            set
            {
                SetValue(IntervalTypeProperty, value);
            }
        }

        /// <summary>
        /// Get or set the ViewportRangeEnabled property of the axis
        /// </summary>
        public Boolean ViewportRangeEnabled
        {
            get
            {
                return (Boolean)GetValue(ViewportRangeEnabledProperty);
            }
            set
            {
                SetValue(ViewportRangeEnabledProperty, value);
            }
        }

        /// <summary>
        /// Get or set the "AxisLabels element" property of the axis
        /// </summary>
        public AxisLabels AxisLabels
        {
            get
            {
                return (AxisLabels)GetValue(AxisLabelsProperty);
            }
            set
            {
                SetValue(AxisLabelsProperty, value);
            }
        }

        /// <summary>
        /// Get or set the Opacity property
        /// </summary>
        public new Double Opacity
        {
            get
            {
                return (Double)GetValue(OpacityProperty);
            }
            set
            {
#if SL
                if (Opacity != value)
                {
                    InternalOpacity = value;
                    SetValue(OpacityProperty, value);
                    FirePropertyChanged(VcProperties.Opacity);
                }
#else
                SetValue(OpacityProperty, value);
#endif
            }
        }

        /// <summary>
        /// Get or set the Cursor property
        /// </summary>
        public new Cursor Cursor
        {
            get
            {
                return base.Cursor;
            }
            set
            {
                if (base.Cursor != value)
                {
                    base.Cursor = value;
                    FirePropertyChanged(VcProperties.Cursor);
                }
            }
        }

        /// <summary>
        /// Get or set the href target property of the axis
        /// </summary>
        public HrefTargets HrefTarget
        {
            get
            {
                return (HrefTargets)GetValue(HrefTargetProperty);
            }
            set
            {
                SetValue(HrefTargetProperty, value);
            }
        }

        /// <summary>
        /// Get or set the href property of the axis
        /// </summary>
        public String Href
        {
            get
            {
                return (String)GetValue(HrefProperty);
            }
            set
            {
                SetValue(HrefProperty, value);
            }
        }

        /// <summary>
        /// Get or set the background property of the axis
        /// </summary>
        public new Brush Background
        {
            get
            {
                return (Brush)GetValue(BackgroundProperty);
            }
            set
            {
#if SL
                if (Background != value)
                {
                    InternalBackground = value;
                    SetValue(BackgroundProperty, value);
                    FirePropertyChanged(VcProperties.Background);
                }
#else
                SetValue(BackgroundProperty, value);
#endif
            }
        }

        /// <summary>
        /// Get or set the interval for all the axis elements
        /// </summary>
#if SL
        [System.ComponentModel.TypeConverter(typeof(Converters.NullableDoubleConverter))]
#endif
        public Nullable<Double> Interval
        {
            get
            {
                if ((Nullable<Double>)GetValue(IntervalProperty) == null)
                    return Double.NaN;
                else
                    return (Nullable<Double>)GetValue(IntervalProperty);
            }
            set
            {
                SetValue(IntervalProperty, value);
                _axisIntervalOverride = (Double)value;
            }
        }

        /// <summary>
        /// Get the actual value of Interval property which is calculated internally or set by user. 
        /// It is a read-only property.
        /// </summary>
        public Double ActualInterval
        {
            get
            {
                return InternalInterval;
            }
        }

        /// <summary>
        /// Get the actual value of IntervalType property which is calculated internally or set by user. 
        /// It is a read-only property.
        /// </summary>
        public IntervalTypes ActualIntervalType
        {
            get
            {
                return InternalIntervalType;
            }
        }

        /// <summary>
        /// Get or set the Color of the axis line
        /// </summary>
        public Brush LineColor
        {
            get
            {
                return (Brush)GetValue(LineColorProperty);
            }
            set
            {
                SetValue(LineColorProperty, value);
            }
        }

        /// <summary>
        /// Get or set the thickness of the axis line
        /// </summary>
#if SL
        [System.ComponentModel.TypeConverter(typeof(Converters.NullableDoubleConverter))]
#endif
        public Nullable<Double> LineThickness
        {
            get
            {
                if (GetValue(LineThicknessProperty) == null)
                {
                    if (Chart != null && (Chart as Chart).PlotDetails != null && (Chart as Chart).PlotDetails.ChartOrientation == ChartOrientationType.Circular)
                        return 1;
                    else
                        return 0.5;
                }
                else
                    return (Double)GetValue(LineThicknessProperty);
            }
            set
            {
                SetValue(LineThicknessProperty, value);
            }
        }

        /// <summary>
        /// Get or set the style of the axis line. It takes values like "Dashed", "Dotted" etc
        /// </summary>
        public LineStyles LineStyle
        {
            get
            {
                return (LineStyles)GetValue(LineStyleProperty);
            }
            set
            {
                SetValue(LineStyleProperty, value);
            }
        }

        /// <summary>
        /// Get or set the padding of the axis
        /// </summary>
        public new Thickness Padding
        {
            get
            {
                return (Thickness)GetValue(PaddingProperty);
            }
            set
            {
#if WPF
                SetValue(PaddingProperty, value);
#else
                InternalPadding = value;
                SetValue(PaddingProperty, value);
                FirePropertyChanged(VcProperties.Padding);
#endif
            }
        }

        /// <summary>
        /// Get or set the title for the axis
        /// </summary>
        public String Title
        {
            get
            {
                return (String)GetValue(TitleProperty);
            }
            set
            {
                SetValue(TitleProperty, value);
            }
        }

        /// <summary>
        /// Get or set the font color for axis title
        /// </summary>
        public Brush TitleFontColor
        {
            get
            {
                return (Brush)GetValue(TitleFontColorProperty);
            }
            set
            {
                SetValue(TitleFontColorProperty, value);
            }
        }

        /// <summary>
        /// Get or set the font family for axis title
        /// </summary>
        public FontFamily TitleFontFamily
        {
            get
            {
                return (FontFamily)GetValue(TitleFontFamilyProperty);
            }
            set
            {
                SetValue(TitleFontFamilyProperty, value);
            }
        }

        /// <summary>
        /// Get or set the font size for axis title
        /// </summary>
        public Double TitleFontSize
        {
            get
            {
                return (Double)GetValue(TitleFontSizeProperty);
            }
            set
            {
                SetValue(TitleFontSizeProperty, value);
            }
        }

        /// <summary>
        /// Get or set Sets the font style for axis title
        /// </summary>
        public FontStyle TitleFontStyle
        {
            get
            {
                return (FontStyle)GetValue(TitleFontStyleProperty);
            }
            set
            {
                SetValue(TitleFontStyleProperty, value);
            }
        }

        /// <summary>
        /// Get or set the font weight for axis title
        /// </summary>
        public FontWeight TitleFontWeight
        {
            get
            {
                return (FontWeight)GetValue(TitleFontWeightProperty);
            }
            set
            {
                SetValue(TitleFontWeightProperty, value);
            }
        }

        /// <summary>
        /// Get or set the axis type (Primary or Secondary)
        /// </summary>
        public AxisTypes AxisType
        {
            get
            {
                return (AxisTypes)GetValue(AxisTypeProperty);
            }
            set
            {
                SetValue(AxisTypeProperty, value);
            }
        }

        /// <summary>
        /// Get or set the maximum value for the axis
        /// </summary>
        //#if SL
        //        [System.ComponentModel.TypeConverter(typeof(Converters.NullableDoubleConverter))]
        //#endif
        public Object AxisMaximum
        {
            get
            {
                return (Object)GetValue(AxisMaximumProperty);
            }
            set
            {
                SetValue(AxisMaximumProperty, value);
            }
        }

        /// <summary>
        /// Get or set the minimum value for the axis
        /// </summary>
        //#if SL
        // [System.ComponentModel.TypeConverter(typeof(Converters.NullableDoubleConverter))]
        //#endif
        public Object AxisMinimum
        {
            get
            {
                return (Object)GetValue(AxisMinimumProperty);
            }
            set
            {
                SetValue(AxisMinimumProperty, value);
            }
        }

        /// <summary>
        /// Include zero within the axis range
        /// </summary>
        public Boolean IncludeZero
        {
            get
            {
                return (Boolean)GetValue(IncludeZeroProperty);
            }
            set
            {
                SetValue(IncludeZeroProperty, value);
            }
        }

        /// <summary>
        /// Get or set dateTime from zero property of the axis. 
        /// Forces the axis to include zero or atleast have either AxisMinimum or AxisMaximum as zero
        /// </summary>
        [System.ComponentModel.TypeConverter(typeof(NullableBoolConverter))]
        public Nullable<Boolean> StartFromZero
        {
            get
            {
                if ((Nullable<Boolean>)GetValue(StartFromZeroProperty) == null)
                {
                    if (AxisRepresentation == AxisRepresentations.AxisY)
                    {
                        if (ViewportRangeEnabled)
                            return false;
                        else
                            return true;
                    }
                    else
                    {
                        if (AxisOrientation == AxisOrientation.Circular)
                            return true;
                        else
                            return false;
                    }
                }
                else
                    return (Nullable<Boolean>)GetValue(StartFromZeroProperty);
            }
            set
            {
                SetValue(StartFromZeroProperty, value);
            }
        }

        /// <summary>
        /// Get or set the prefix for the axis labels used in the axis
        /// </summary>
        public String Prefix
        {
            get
            {
                return (String)GetValue(PrefixProperty);
            }
            set
            {
                SetValue(PrefixProperty, value);
            }
        }

        /// <summary>
        /// Get or set the suffix for the axis labels used in the axis
        /// </summary>
        public String Suffix
        {
            get
            {
                return (String)GetValue(SuffixProperty);
            }
            set
            {
                SetValue(SuffixProperty, value);
            }
        }

        /// <summary>
        /// Get or set the scaling values for the axis
        /// </summary>
        public String ScalingSet
        {
            get
            {
                return (String)GetValue(ScalingSetProperty);
            }
            set
            {
                SetValue(ScalingSetProperty, value);
            }
        }

        /// <summary>
        /// Get or set the format string that can be used with the axis labels
        /// </summary>
        public String ValueFormatString
        {
            get
            {
                return String.IsNullOrEmpty((String)GetValue(ValueFormatStringProperty)) ? "###,##0.##" : (String)GetValue(ValueFormatStringProperty);
            }
            set
            {
                SetValue(ValueFormatStringProperty, value);
            }
        }

        /// <summary>
        /// Get or set scrollbar offset value property of the axis. 
        /// ScrollBarOffset value can be accessed after the chart is rendered. Value range from 0 to 1
        /// </summary>
        public Double ScrollBarOffset
        {
            get
            {
                return (Double)GetValue(ScrollBarOffsetProperty);
            }
            set
            {
                if (value < 0 || value > 1)
                    throw new Exception("Value does not fall under the expected range. ScrollBarOffset always varies from 0 to 1.");
                SetValue(ScrollBarOffsetProperty, value);
            }
        }



        /// <summary>
        /// ScrollBarScale sets the size of ScrollBar thumb.  
        /// Example, if ScrollBarScale is set to 0.5, width of ScrollBar thumb will be
        /// half of the ScrollBar width which in turn increase the PlotArea width to
        /// double the actual width of PlotArea.
        /// </summary>
        public Double ScrollBarScale
        {
            get { return (Double)GetValue(ScrollBarScaleProperty); }
            set
            {
                if (value <= 0 || value > 1)
                    throw new Exception("Value does not fall under the expected range. ScrollBarScale always varies from 0 to 1.");

                SetValue(ScrollBarScaleProperty, value);
            }
        }

        /// <summary>
        /// Distance between two nearest plots
        /// </summary>
        public Double ClosestPlotDistance
        {
            get { return (Double)GetValue(ClosestPlotDistanceProperty); }
            set
            {
                SetValue(ClosestPlotDistanceProperty, value);
            }
        }

        /// <summary>
        /// ScrollBarSize sets the size of ScrollBar.
        /// For AxisX, setting the ScrollBarSize will set the Height of ScrollBar.
        /// For AxisY, setting the ScrollBarSize will set the Width of ScrollBar.
        /// </summary>
        public Double ScrollBarSize
        {
            get
            {
                return (Double)GetValue(ScrollBarSizeProperty);
            }
            set
            {
                SetValue(ScrollBarSizeProperty, value);
            }
        }

        /// <summary>
        /// Get or set enabled property of the axis
        /// </summary>
        [System.ComponentModel.TypeConverter(typeof(NullableBoolConverter))]
        public Nullable<Boolean> Enabled
        {
            get
            {
                if ((Nullable<Boolean>)GetValue(EnabledProperty) == null)
                    return true;
                else
                    return (Nullable<Boolean>)GetValue(EnabledProperty);
            }
            set
            {
                SetValue(EnabledProperty, value);
            }
        }

        /// <summary>
        /// Collection of CustomLabel
        /// </summary>
        public CustomAxisLabelsCollection CustomAxisLabels
        {
            get;
            set;
        }

        /// <summary>
        /// Collection of grids for an axis
        /// </summary>
        public ChartGridCollection Grids
        {
            get;
            set;
        }

        /// <summary>
        /// Collection of ticks for an axis
        /// </summary>
        public TicksCollection Ticks
        {
            get;
            set;
        }

        #endregion

        #region Public Events And Delegates

        #endregion

        #region Protected Methods

        #endregion

        #region Internal Properties
#if SL
        /// <summary>
        /// Identifies the Visifire.Charts.Axis.Padding dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Axis.Padding dependency property.
        /// </returns>
        private static readonly DependencyProperty InternalPaddingProperty = DependencyProperty.Register
            ("InternalPadding",
            typeof(Thickness),
            typeof(Axis),
            new PropertyMetadata(OnPaddingPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.Axis.Opacity dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Axis.Opacity dependency property.
        /// </returns>
        private static readonly DependencyProperty InternalOpacityProperty = DependencyProperty.Register
            ("InternalOpacity",
            typeof(Double),
            typeof(Axis),
            new PropertyMetadata(1.0, OnOpacityPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.Axis.Background dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Axis.Background dependency property.
        /// </returns>
        private static readonly DependencyProperty InternalBackgroundProperty = DependencyProperty.Register
            ("InternalBackground",
            typeof(Brush),
            typeof(Axis),
            new PropertyMetadata(OnBackgroundPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.Axis.MaxHeight dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Axis.MaxHeight dependency property.
        /// </returns>
        private static readonly DependencyProperty InternalMaxHeightProperty = DependencyProperty.Register
           ("InternalMaxHeight",
           typeof(Double),
           typeof(Axis),
           new PropertyMetadata(Double.PositiveInfinity, OnMaxHeightPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.Axis.MaxWidth dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Axis.MaxWidth dependency property.
        /// </returns>
        private static readonly DependencyProperty InternalMaxWidthProperty = DependencyProperty.Register
           ("InternalMaxWidth",
           typeof(Double),
           typeof(Axis),
           new PropertyMetadata(Double.PositiveInfinity, OnMaxWidthPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.Axis.MinHeight dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Axis.MinHeight dependency property.
        /// </returns>
        private static readonly DependencyProperty InternalMinHeightProperty = DependencyProperty.Register
           ("InternalMinHeight",
           typeof(Double),
           typeof(Axis),
           new PropertyMetadata(OnMinHeightPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.Axis.MinWidth dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Axis.MinWidth dependency property.
        /// </returns>
        private static readonly DependencyProperty InternalMinWidthProperty = DependencyProperty.Register
           ("InternalMinWidth",
           typeof(Double),
           typeof(Axis),
           new PropertyMetadata(OnMinWidthPropertyChanged));
#endif

        /// <summary>
        /// MaxHeight of the Axis
        /// </summary>
        internal Double InternalMaxHeight
        {
            get
            {
                return (Double)(Double.IsNaN(_internalMaxHeight) ? GetValue(MaxHeightProperty) : _internalMaxHeight);
            }
            set
            {
                _internalMaxHeight = value;
            }
        }

        /// <summary>
        /// MaxWidth of the Axis
        /// </summary>
        internal Double InternalMaxWidth
        {
            get
            {
                return (Double)(Double.IsNaN(_internalMaxWidth) ? GetValue(MaxWidthProperty) : _internalMaxWidth);
            }
            set
            {
                _internalMaxWidth = value;
            }
        }

        /// <summary>
        /// MinHeight of the Axis
        /// </summary>
        internal Double InternalMinHeight
        {
            get
            {
                return (Double)(Double.IsNaN(_internalMinHeight) ? GetValue(MinHeightProperty) : _internalMinHeight);
            }
            set
            {
                _internalMinHeight = value;
            }
        }

        /// <summary>
        /// MinWidth of the Axis
        /// </summary>
        internal Double InternalMinWidth
        {
            get
            {
                return (Double)(Double.IsNaN(_internalMinWidth) ? GetValue(MinWidthProperty) : _internalMinWidth);
            }
            set
            {
                _internalMinWidth = value;
            }
        }

        /// <summary>
        /// Get or set the Background property of title
        /// </summary>
        internal Brush InternalBackground
        {
            get
            {
                return (Brush)((_internalBackground == null) ? GetValue(BackgroundProperty) : _internalBackground);
            }
            set
            {
                _internalBackground = value;
            }
        }

        /// <summary>
        /// Get or set the Padding property of title
        /// </summary>
        internal Thickness InternalPadding
        {
            get
            {
                return (Thickness)((_internalPadding == null) ? GetValue(PaddingProperty) : _internalPadding);
            }
            set
            {
                _internalPadding = value;
            }
        }

        /// <summary>
        /// Get or set the Opacity property
        /// </summary>
        internal Double InternalOpacity
        {
            get
            {
                return (Double)(Double.IsNaN(_internalOpacity) ? GetValue(OpacityProperty) : _internalOpacity);
            }
            set
            {
                _internalOpacity = value;
            }
        }

        /// <summary>
        /// AxisMinimum Numeric value
        /// </summary>
        internal Double AxisMinimumNumeric = Double.NaN;

        /// <summary>
        /// AxisMinimum DateTime value
        /// </summary>
        internal DateTime AxisMinimumDateTime;

        /// <summary>
        /// AxisMaximum numeric value
        /// </summary>
        internal Double AxisMaximumNumeric = Double.NaN;

        /// <summary>
        /// AxisMaximum DateTime value
        /// </summary>
        internal DateTime AxisMaximumDateTime;

        /// <summary>
        /// Internal interval type used to handle auto interval type 
        /// </summary>
        internal IntervalTypes InternalIntervalType
        {
            get;
            set;
        }

        /// <summary>
        /// Axis is a DateTime axis
        /// </summary>
        internal Boolean IsDateTimeAxis
        {
            get;
            set;
        }

        /// <summary>
        /// Axis XValue Types
        /// </summary>
        internal ChartValueTypes XValueType
        {
            get;
            set;
        }

        /// <summary>
        /// Axis Minimum Date
        /// </summary>
        internal DateTime MinDate
        {
            get;
            set;
        }

        /// <summary>
        /// Axis Maximum Date
        /// </summary>
        internal DateTime MaxDate
        {
            get;
            set;
        }

        internal TimeSpan MinDateRange
        {
            get;
            set;
        }

        internal TimeSpan MaxDateRange
        {
            get;
            set;
        }

        /// <summary>
        /// Returns the visual element for the Axis
        /// </summary>
        internal StackPanel Visual
        {
            get;
            set;
        }

        /// <summary>
        /// Returns the visual element for the Axis
        /// </summary>
        internal StackPanel AxisElementsContainer
        {
            get;
            set;
        }

        /// <summary>
        /// Get the actual value of AxisMinimum property which is calculated internally or set by user. 
        /// It is a read-only property.
        /// </summary>
        public Object ActualAxisMinimum
        {   
            get
            {   
                if (IsDateTimeAxis)
                {   
                    if (Chart != null && (Chart as Chart).PlotDetails.ListOfAllDataPoints.Count != 0)
                    {   
                        return DateTimeHelper.XValueToDateTime(MinDate,
                            InternalAxisMinimum, InternalIntervalType);
                    }
                    else
                        return MinDate;
                }
                else
                {
                    if (AxisRepresentation == AxisRepresentations.AxisY && Logarithmic)
                        return DataPoint.ConvertLogarithmicValue2ActualValue(Chart as Chart, InternalAxisMinimum, AxisType);
                    else
                        return InternalAxisMinimum;
                }
            }
        }

        /// <summary>
        /// Get the actual value of AxisMaximum property which is calculated internally or set by user. 
        /// It is a read-only property.
        /// </summary>
        public Object ActualAxisMaximum
        {   
            get
            {   
                if (IsDateTimeAxis)
                {   
                    if (Chart != null && (Chart as Chart).PlotDetails.ListOfAllDataPoints.Count != 0)
                    {
                        return DateTimeHelper.XValueToDateTime(MinDate,
                            InternalAxisMaximum, InternalIntervalType);
                    }
                    else
                        return MaxDate;
                }
                else
                {
                    if (AxisRepresentation == AxisRepresentations.AxisY && Logarithmic)
                        return DataPoint.ConvertLogarithmicValue2ActualValue(Chart as Chart, InternalAxisMaximum, AxisType);
                    else
                        return InternalAxisMaximum;
                }
            }
        }

        /// <summary>
        /// Internal axis minimum is used for internal calculation purpose
        /// </summary>
        internal Double InternalAxisMinimum
        {   
            get;
            private set;
        }

        /// <summary>
        /// Internal axis maximum is used for internal calculation purpose
        /// </summary>
        internal Double InternalAxisMaximum
        {
            get;
            private set;
        }

        internal Double _oldInternalAxisMinimum;
        internal Double _oldInternalAxisMaximum;
        
        /// <summary>
        /// Internal interval is used for internal calculation purpose
        /// </summary>
        internal Double InternalInterval
        {
            get;
            set;
        }

        /// <summary>
        /// Keep tracks about current offsetvalue of the axis scrollviewer
        /// </summary>
        internal Double CurrentScrollScrollBarOffset
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set the data maximum for the axis
        /// </summary>
        internal Double Maximum
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set the data minimum for the axis
        /// </summary>
        internal Double Minimum
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set majorgrid element
        /// </summary>
        internal ChartGrid MajorGridsElement
        {
            get;
            set;
        }

        // Change it
        /// <summary>
        /// Get or set the scroll bar
        /// </summary>
        public ZoomBar ScrollBarElement
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set axis scrollviewer element
        /// </summary>
        internal Canvas ScrollViewerElement
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set the major ticks
        /// </summary>
        internal Ticks MajorTicksElement
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set the axis title element
        /// </summary>
        internal Title AxisTitleElement
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set the axis manager of the axis
        /// </summary>
        internal AxisManager AxisManager
        {
            get;
            set;
        }

        /// <summary>
        /// Scrollable size of PlotArea
        /// </summary>
        internal Double ScrollableSize
        {
            get;
            set;
        }

        /// <summary>
        /// Line for axis
        /// </summary>
        internal Line AxisLine
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set the axis orientation. 
        /// Vertical = AxisY for all types except bar, 
        /// Horizontal = AxesX for types except bar
        /// </summary>
        internal AxisOrientation AxisOrientation
        {
            get
            {
                return _orientation;
            }
            set
            {
                _orientation = value;
            }
        }

        /// <summary>
        /// Get or set the axis representation
        /// </summary>
        internal AxisRepresentations AxisRepresentation
        {
            get;
            set;
        }

        /// <summary>
        /// Details about plot groups
        /// </summary>
        internal PlotDetails PlotDetails
        {
            get;
            set;
        }

        /// <summary>
        /// Scale values of axis
        /// </summary>
        internal List<Double> ScaleValues
        {
            get
            {
                return _scaleValues;
            }
        }

        /// <summary>
        /// Scale units of axis
        /// </summary>
        internal List<String> ScaleUnits
        {
            get
            {
                return _scaleUnits;
            }
        }

        /// <summary>
        /// Get or set the axis dateTime offset
        /// </summary>
        internal Double StartOffset
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set the axis end offset
        /// </summary>
        internal Double EndOffset
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set the axis skip offset
        /// </summary>
        internal Int32 SkipOffset
        {
            get;
            set;
        }


        #endregion

        #region Private Properties

        /// <summary>
        /// StackPanel used for internal purpose
        /// </summary>
        private Canvas InternalStackPanel
        {
            get;
            set;
        }

        #region Hidden Control Properties

        /// <summary>
        /// Get or set
        /// </summary>
        private new FontFamily FontFamily
        {
            get;
            set;
        }

        private new static DependencyProperty FontFamilyProperty = DependencyProperty.Register
            ("FontFamily",
            typeof(FontFamily),
            typeof(Axis),
            null);

        private new Double FontSize
        {
            get;
            set;
        }

        private new static DependencyProperty FontSizeProperty = DependencyProperty.Register
            ("FontSize",
            typeof(Double),
            typeof(Axis),
            null);

        private new FontStretch FontStretch
        {
            get;
            set;
        }

        private new static DependencyProperty FontStretchProperty = DependencyProperty.Register
            ("FontStretch",
            typeof(FontStretch),
            typeof(Axis),
            null);

        private new FontStyle FontStyle
        {
            get;
            set;
        }

        private new static DependencyProperty FontStyleProperty = DependencyProperty.Register
            ("FontStyle",
            typeof(FontStyle),
            typeof(Axis),
            null);

        private new FontWeight FontWeight
        {
            get;
            set;
        }

        private new static DependencyProperty FontWeightProperty = DependencyProperty.Register
            ("FontWeight",
            typeof(FontWeight),
            typeof(Axis),
            null);

        private new Brush Foreground
        {
            get;
            set;
        }

        private new static DependencyProperty ForegroundProperty = DependencyProperty.Register
            ("Foreground",
            typeof(Brush),
            typeof(Axis),
            null);

        private new Thickness BorderThickness
        {
            get;
            set;
        }

        private new static DependencyProperty BorderThicknessProperty = DependencyProperty.Register
            ("BorderThickness",
            typeof(Thickness),
            typeof(Axis),
            null);

        private new Brush BorderBrush
        {
            get;
            set;
        }

        private new static DependencyProperty BorderBrushProperty = DependencyProperty.Register
            ("BorderBrush",
            typeof(Brush),
            typeof(Axis),
            null);

        #endregion

        #endregion

        #region Private Delegates

        #endregion

        #region Private Methods

        // <summary>
        /// Event handler attached with Padding property changed event of AxisLabels elements
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnPaddingPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Axis axis = d as Axis;
            axis.InternalPadding = (Thickness)e.NewValue;
            axis.FirePropertyChanged(VcProperties.Padding);
        }

        /// <summary>
        /// Event handler manages background property change of axis
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnBackgroundPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Axis axis = d as Axis;
            axis.InternalBackground = (Brush)e.NewValue;
            axis.FirePropertyChanged(VcProperties.Background);
        }

        /// <summary>
        /// OpacityProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnOpacityPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Axis axis = d as Axis;
            axis.InternalOpacity = (Double)e.NewValue;
            axis.FirePropertyChanged(VcProperties.Opacity);
        }

        /// <summary>
        /// ViewportRangeEnabledProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnViewportRangeEnabledPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Axis axis = d as Axis;
            axis.FirePropertyChanged(VcProperties.ViewportRangeEnabled);
        }

        /// <summary>
        /// Event handler manages axislabels property change event of axis
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnAxisLabelsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Axis axis = d as Axis;

            if (axis.Chart != null)
                axis.AxisLabels.Chart = axis.Chart;

            axis.AxisLabels.Parent = axis;
            axis.AxisLabels.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(AxisLabels_PropertyChanged);
            
            if(!axis.AxisLabels.IsDefault)
                axis.FirePropertyChanged(VcProperties.AxisLabels);
        }

        /// <summary>
        /// Event handler manages property change event of axislabels element
        /// </summary>
        /// <param name="sender">ObservableObject</param>
        /// <param name="e">PropertyChangedEventArgs</param>
        private static void AxisLabels_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            (sender as AxisLabels).Parent.FirePropertyChanged((VcProperties)Enum.Parse(typeof(VcProperties), e.PropertyName, true));
        }

        /// <summary>
        /// Event handler manages href target property change event of axis
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnHrefTargetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Axis axis = d as Axis;
            axis.FirePropertyChanged(VcProperties.HrefTarget);
        }

        /// <summary>
        /// Event handler manages href property change event of axis
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnHrefChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Axis axis = d as Axis;
            axis.FirePropertyChanged(VcProperties.Href);
        }

        /// <summary>
        /// Event handler manages interval property change event of axis
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnIntervalPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Axis axis = d as Axis;

            if (e.NewValue == null)
                axis.ResetAxisInterval();

            axis.FirePropertyChanged(VcProperties.Interval);
        }

        /// <summary>
        /// Event handler manages linecolor property change event of axis
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnLineColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Axis axis = d as Axis;
            axis.FirePropertyChanged(VcProperties.LineColor);
        }

        /// <summary>
        /// Event handler manages linethickness property change event of axis
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnLineThicknessPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Axis axis = d as Axis;
            axis.FirePropertyChanged(VcProperties.LineThickness);
        }

        /// <summary>
        /// Event handler manages linestyle property change event of axis
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnLineStylePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Axis axis = d as Axis;
            axis.FirePropertyChanged(VcProperties.LineStyle);
        }

        /// <summary>
        /// Event handler manages title property change event of axis
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnTitlePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Axis axis = d as Axis;
            axis.FirePropertyChanged(VcProperties.Title);
        }

        /// <summary>
        /// Event handler manages titlefontcolor property change event of axis
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnTitleFontColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Axis axis = d as Axis;
            axis.FirePropertyChanged(VcProperties.TitleFontColor);
        }

        /// <summary>
        /// Event handler manages titlefontfamily property change event of axis
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnTitleFontFamilyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Axis axis = d as Axis;
            axis.FirePropertyChanged(VcProperties.TitleFontFamily);
        }

        /// <summary>
        /// Event handler manages titlefontsize property change event of axis
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnTitleFontSizePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Axis axis = d as Axis;
            axis.FirePropertyChanged(VcProperties.TitleFontSize);
        }

        /// <summary>
        /// Event handler manages titlefontstyle property change event of axis
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnTitleFontStylePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Axis axis = d as Axis;
            axis.FirePropertyChanged(VcProperties.TitleFontStyle);
        }

        /// <summary>
        /// Event handler manages title fontweight property change event of axis
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnTitleFontWeightPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Axis axis = d as Axis;
            axis.FirePropertyChanged(VcProperties.TitleFontWeight);
        }

        /// <summary>
        /// Event handler manages axistype property change event of axis
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnAxisTypePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Axis axis = d as Axis;
            axis.FirePropertyChanged(VcProperties.AxisType);
        }

        /// <summary>
        /// Event handler manages MaxHeight property change event of axis
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnMaxHeightPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Axis axis = d as Axis;
            axis.InternalMaxHeight = (Double)e.NewValue;
            axis.FirePropertyChanged(VcProperties.MaxHeight);
        }

        /// <summary>
        /// Event handler manages MinHeight property change event of axis
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnMinHeightPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Axis axis = d as Axis;
            axis.InternalMinHeight = (Double)e.NewValue;
            axis.FirePropertyChanged(VcProperties.MinHeight);
        }

        /// <summary>
        /// Event handler manages MaxWidth property change event of axis
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnMaxWidthPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Axis axis = d as Axis;
            axis.InternalMaxWidth = (Double)e.NewValue;
            axis.FirePropertyChanged(VcProperties.MaxWidth);
        }

        /// <summary>
        /// Event handler manages MinWidth property change event of axis
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnMinWidthPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Axis axis = d as Axis;
            axis.InternalMinWidth = (Double)e.NewValue;
            axis.FirePropertyChanged(VcProperties.MinWidth);
        }

        /// <summary>
        /// Event handler manages axis maximum property change event of axis
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnAxisMaximumPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Axis axis = d as Axis;

            if (e.NewValue == null)
            {   
                axis.ResetAxisMaximum();
                axis.FirePropertyChanged(VcProperties.AxisMaximum);
            }
            else
            {   
                Double numericVal = axis.AxisMaximumNumeric;
                DateTime dateTimeValue = axis.AxisMaximumDateTime;
                Axis.ConvertValueToDateTimeOrNumeric("AxisMaximum", e.NewValue, ref numericVal, ref dateTimeValue, out axis._axisMaximumValueType);
                axis.AxisMaximumNumeric = numericVal;
                axis.AxisMaximumDateTime = dateTimeValue;

                if (axis.AxisRepresentation == AxisRepresentations.AxisY)
                {
                    if (axis.Chart != null && (axis.Chart as Chart).Series.Count > 0)
                        (axis.Chart as Chart).Dispatcher.BeginInvoke(new Action<VcProperties, object>((axis.Chart as Chart).Series[0].UpdateVisual), new object[] { VcProperties.AxisMaximum, null });
                }
                else
                    axis.FirePropertyChanged(VcProperties.AxisMaximum);
            }
        }

        /// <summary>
        /// Event handler manages axis minimum property change event of axis
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnAxisMinimumPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {   
            Axis axis = d as Axis;
            
            if (e.NewValue == null)
            {      
                axis.ResetAxisMinimum();
                axis.FirePropertyChanged(VcProperties.AxisMinimum);
            }
            else
            {   
                Double numericVal = axis.AxisMinimumNumeric;
                DateTime dateTimeValue = axis.AxisMinimumDateTime;
                Axis.ConvertValueToDateTimeOrNumeric("AxisMinimum", e.NewValue, ref numericVal, ref dateTimeValue, out axis._axisMinimumValueType);
                axis.AxisMinimumNumeric = numericVal;
                axis.AxisMinimumDateTime = dateTimeValue;

                if (axis.AxisRepresentation == AxisRepresentations.AxisY)
                {   
                    if (axis.Chart != null && (axis.Chart as Chart).Series.Count > 0)
                        (axis.Chart as Chart).Dispatcher.BeginInvoke(new Action<VcProperties, object>((axis.Chart as Chart).Series[0].UpdateVisual), new object[] { VcProperties.AxisMinimum, null });
                }
                else
                    axis.FirePropertyChanged(VcProperties.AxisMinimum);
            }
        }

        private void ResetAxisMinimum()
        {
            AxisMinimumNumeric = Double.NaN;
            AxisMinimumDateTime = new DateTime();
            InternalAxisMinimum = Double.NaN;
            _numericViewMinimum = 0;
            _oldInternalAxisMinimum = 0;
        }

        private void ResetAxisMaximum()
        {
            AxisMaximumNumeric = Double.NaN;
            AxisMaximumDateTime = new DateTime();
            InternalAxisMaximum = Double.NaN;
            _numericViewMaximum = 0;
            _oldInternalAxisMaximum = 0;
        }

        private void ResetAxisInterval()
        {
            InternalInterval = Double.NaN;
        }

        private static void ConvertValueToDateTimeOrNumeric(String propertyName, Object newValue, ref Double numericVal, ref DateTime dateTimeValue, out ChartValueTypes valueType)
        {
            // Double / Int32 value entered in Managed Code
            if (newValue.GetType().Equals(typeof(Double)) || newValue.GetType().Equals(typeof(Int32)))
            {
                numericVal = Convert.ToDouble(newValue);
                valueType = ChartValueTypes.Numeric;
            }
            // DateTime value entered in Managed Code
            else if ((newValue.GetType().Equals(typeof(DateTime))))
            {
                dateTimeValue = (DateTime)newValue;
                valueType = ChartValueTypes.DateTime;
            }
            // Double / Int32 / DateTime entered in XAML
            else if ((newValue.GetType().Equals(typeof(String))))
            {
                DateTime dateTimeresult;
                Double doubleResult;

                if (String.IsNullOrEmpty(newValue.ToString()))
                {
                    numericVal = Double.NaN;
                    valueType = ChartValueTypes.Numeric;
                }
                // Double entered in XAML
                else if (Double.TryParse((string)newValue, System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.InvariantCulture, out doubleResult))
                {
                    numericVal = doubleResult;
                    valueType = ChartValueTypes.Numeric;
                }
                // DateTime entered in XAML
                else if (DateTime.TryParse((string)newValue, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dateTimeresult))
                {
                    dateTimeValue = dateTimeresult;
                    valueType = ChartValueTypes.DateTime;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Invalid Input for " + propertyName);
                    throw new Exception("Invalid Input for " + propertyName);
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Invalid Input for " + propertyName);
                throw new Exception("Invalid Input for " + propertyName);
            }
        }


        /// <summary>
        /// Event handler manages include zero property change event of axis
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnIncludeZeroPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Axis axis = d as Axis;
            axis.FirePropertyChanged(VcProperties.IncludeZero);
        }

        /// <summary>
        /// Event handler manages dateTime from zero property change event of axis
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnStartFromZeroPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Axis axis = d as Axis;
            axis.FirePropertyChanged(VcProperties.StartFromZero);
        }

        /// <summary>
        /// Event handler manages prefix property change event of axis
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnPrefixPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Axis axis = d as Axis;
            axis.FirePropertyChanged(VcProperties.Prefix);
        }

        /// <summary>
        /// Event handler manages suffix property change event of axis
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnSuffixPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Axis axis = d as Axis;
            axis.FirePropertyChanged(VcProperties.Suffix);
        }

        /// <summary>
        /// Event handler manages scaling set property change event of axis
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnScalingSetPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Axis axis = d as Axis;
            axis.ParseScalingSets((String)e.NewValue);
            axis.FirePropertyChanged(VcProperties.ScalingSet);
        }

        /// <summary>
        /// Event handler manages value format string property change event of axis
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnValueFormatStringPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Axis axis = d as Axis;
            axis.FirePropertyChanged(VcProperties.ValueFormatString);
        }

        /// <summary>
        /// Event handler manages scrollbar offset property change event of axis
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnScrollBarOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Axis axis = d as Axis;

            if (axis._isScrollToOffsetEnabled)
                axis.SetScrollBarValueFromOffset((Double)e.NewValue, true);
        }

        /// <summary>
        /// Event handler manages scrollbar size property change event of axis
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnScrollBarSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {   
            Axis axis = d as Axis;
            axis.FirePropertyChanged(VcProperties.ScrollBarSize);
        }

        private static void OnScrollBarScalePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Axis axis = d as Axis;

           axis.FirePropertyChanged(VcProperties.ScrollBarScale);

            if (axis.Chart != null && (axis.Chart as Chart).ChartArea != null)
            {   
                if (axis.IsNotificationEnable)
                {
                    (axis.Chart as Chart).ChartArea.IsAutoCalculatedScrollBarScale = false;
                }
                else
                {
                    (axis.Chart as Chart).ChartArea.IsAutoCalculatedScrollBarScale = true;
                }
            }
        }

        private static void OnClosestPlotDistancePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Axis axis = d as Axis;                     
            axis.FirePropertyChanged(VcProperties.ClosestPlotDistance);
        }

        /// <summary>
        /// Event handler manages enabled property change event of axis
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnEnabledPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Axis axis = d as Axis;
            axis.FirePropertyChanged(VcProperties.Enabled);
        }

        /// <summary>
        /// Event handler manages interval type property change event of axis
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnIntervalTypePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Axis axis = d as Axis;
            axis.InternalIntervalType = (IntervalTypes)e.NewValue;
            axis.FirePropertyChanged(VcProperties.IntervalType);
        }

        /// <summary>
        /// Event handler manages Logarithmic property change event of axis
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnLogarithmicPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Axis axis = d as Axis;
            axis.FirePropertyChanged(VcProperties.Logarithmic);
        }

        /// <summary>
        /// Event handler manages LogarithmBase property change event of axis
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnLogarithmBasePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Axis axis = d as Axis;
            axis.FirePropertyChanged(VcProperties.LogarithmBase);
        }

        /// <summary>
        /// Set up axis manager and calculate axis values
        /// </summary>
        private void SetUpAxisManager()
        {
            // Get the minimum and maximum value dependeing on the axis representation value
            Minimum = AxisRepresentation == AxisRepresentations.AxisX ? PlotDetails.GetAxisXMinimumDataValue(this) : PlotDetails.GetAxisYMinimumDataValue(this);
            Maximum = AxisRepresentation == AxisRepresentations.AxisX ? PlotDetails.GetAxisXMaximumDataValue(this) : PlotDetails.GetAxisYMaximumDataValue(this);

            if(Logarithmic)
            {
                Double minimum = PlotDetails.GetAxisYMinimumDataValueFromAllDataSeries(this);
                if (!PlotDetails.CheckIfAnyDataPointVisualExistsInChart())
                {
                    minimum = 1;
                    Maximum = Math.Pow(LogarithmBase, 1);
                }

                if (minimum <= 0 || Maximum <= 0)
                    throw new Exception("Negative or zero values cannot be plotted correctly on logarithmic charts.");
            }
                
            Boolean overflowValidity = (AxisRepresentation == AxisRepresentations.AxisX);
            Boolean stackingOverride = PlotDetails.GetStacked100OverrideState();


            // Check if ViewportRangeEnabled property is set in Logarithmic Axis.
            // If set then the minimum value of Log scale should be generated depending upon the new AxisMinimum.
            Boolean startFromMinimumValue4LogScale = false;
            if (AxisRepresentation == AxisRepresentations.AxisY && ViewportRangeEnabled && Logarithmic)
                startFromMinimumValue4LogScale = true;

            Boolean isCircularAxis = (AxisOrientation == AxisOrientation.Circular) ? true : false;

            // Create and initialize the AxisManagers
            AxisManager = new AxisManager(Maximum, Minimum, (Boolean)StartFromZero, overflowValidity, stackingOverride, isCircularAxis, AxisRepresentation, Logarithmic, LogarithmBase, startFromMinimumValue4LogScale);

            // Set the include zero state
            AxisManager.IncludeZero = IncludeZero;

            // settings specific to axis X
            if (AxisRepresentation == AxisRepresentations.AxisX && AxisOrientation != AxisOrientation.Circular)
            {
                Double interval = GenerateDefaultInterval();

                if (IsDateTimeAxis)
                {
                    if (interval > 0 || !Double.IsNaN(interval))
                    {
                        AxisManager.Interval = interval;
                        InternalInterval = interval;
                    }
                    else
                    {
                        // if interval is greater than zero then set the interval of the axis manager
                        if (Interval > 0 || !Double.IsNaN((Double)Interval))
                        {
                            AxisManager.Interval = (Double)Interval;
                            InternalInterval = (Double)Interval;
                        }
                    }
                }
                else if (interval > 0 || !Double.IsNaN(interval))
                {
                    AxisManager.Interval = interval;
                    InternalInterval = interval;
                }
            }
            else
            {
                // if interval is greater than zero then set the interval of the axis manager
                if (Interval > 0 || !Double.IsNaN((Double)Interval))
                {
                    AxisManager.Interval = (Double)Interval;
                    InternalInterval = (Double)Interval;
                }
            }

            // set the axis maximum value if user has provided it
            if (!Double.IsNaN((Double)AxisMaximumNumeric))
            {
                AxisManager.AxisMaximumValue = (Double)AxisMaximumNumeric;
                InternalAxisMaximum = (Double)AxisMaximumNumeric;
            }
            else
            {
                if (AxisOrientation == AxisOrientation.Circular)
                {
                    if (IsDateTimeAxis && AxisMaximum != null)
                    {
                        Double axisMaxNumeric;
                        if (XValueType != ChartValueTypes.Time)
                        {
                            axisMaxNumeric = DateTimeHelper.DateDiff(AxisMaximumDateTime, MinDate, MinDateRange, MaxDateRange, InternalIntervalType, XValueType);
                        }
                        else
                        {
                            axisMaxNumeric = DateTimeHelper.DateDiff(Convert.ToDateTime(AxisMaximum), MinDate, MinDateRange, MaxDateRange, InternalIntervalType, XValueType);
                        }

                        AxisManager.AxisMaximumValue = axisMaxNumeric;
                        InternalAxisMaximum = axisMaxNumeric;
                    }
                }
            }

            // set the axis minimum value if the user has provided it
            if (!Double.IsNaN((Double)AxisMinimumNumeric))
            {
                AxisManager.AxisMinimumValue = (Double)AxisMinimumNumeric;
                InternalAxisMinimum = (Double)AxisMinimumNumeric;
            }
            else
            {
                if (AxisOrientation == AxisOrientation.Circular)
                {
                    if (IsDateTimeAxis && AxisMinimum != null)
                    {
                        Double axisMinNumeric;
                        if (XValueType != ChartValueTypes.Time)
                        {
                            axisMinNumeric = DateTimeHelper.DateDiff(AxisMinimumDateTime, MinDate, MinDateRange, MaxDateRange, InternalIntervalType, XValueType);
                        }
                        else
                        {
                            axisMinNumeric = DateTimeHelper.DateDiff(Convert.ToDateTime(AxisMinimum), MinDate, MinDateRange, MaxDateRange, InternalIntervalType, XValueType);
                        }

                        AxisManager.AxisMinimumValue = axisMinNumeric;
                        InternalAxisMinimum = axisMinNumeric;
                    }
                }
            }

            // Calculate the various parameters for creating the axis
            AxisManager.Calculate();

            // Set axis specific limits based on axis limits.
            // if (AxisRepresentation == AxisRepresentations.AxisX && !(Boolean)StartFromZero)
            //      if (!SetAxesXLimits())
            //          return;

            if (AxisRepresentation == AxisRepresentations.AxisX && AxisOrientation != AxisOrientation.Circular)
                if (!SetAxesXLimits())
                    return;

            // Settings specific to axis y
            if (this.AxisRepresentation == AxisRepresentations.AxisY && this.AxisType == AxisTypes.Primary)
            {
                // Set the internal axis limits the one obtained from axis manager
                InternalAxisMaximum = AxisManager.AxisMaximumValue;
                InternalAxisMinimum = AxisManager.AxisMinimumValue;
            }
            else if (this.AxisRepresentation == AxisRepresentations.AxisY && this.AxisType == AxisTypes.Secondary)
            {
                var axisYPrimary = (from axis in (Chart as Chart).InternalAxesY where axis.AxisRepresentation == AxisRepresentations.AxisY && axis.AxisType == AxisTypes.Primary select axis);

                Axis primaryAxisY = null;
                if (axisYPrimary.Count() > 0)
                    primaryAxisY = axisYPrimary.First();

                if (Double.IsNaN((double)Interval) && primaryAxisY != null)
                {
                    // number of interval in the primary axis
                    Double primaryAxisIntervalCount = ((double)primaryAxisY.InternalAxisMaximum - (double)primaryAxisY.InternalAxisMinimum) / (double)primaryAxisY.InternalInterval;

                    if (!Double.IsNaN(primaryAxisIntervalCount))
                    {   
                        // This will set the internal overriding flag
                        AxisManager.AxisMinimumValue = AxisManager.AxisMinimumValue;
                        if ((Boolean)StartFromZero)
                        {
                            if (AxisManager.AxisMinimumValue == 0)
                            {
                                if (Logarithmic)
                                    AxisManager.AxisMinimumValue = Math.Pow(LogarithmBase, 0);
                            }
                        }

                        if (Logarithmic)
                        {
                            AxisManager.AxisMaximumValue = Math.Pow(LogarithmBase, AxisManager.AxisMaximumValue);

                            // This interval will reflect the interval in primary axis it is not same as that of primary axis
                            AxisManager.Interval = (Math.Log(AxisManager.AxisMaximumValue, LogarithmBase) - Math.Log(AxisManager.AxisMinimumValue, LogarithmBase)) / primaryAxisIntervalCount;

                        }
                        else
                        {
                            AxisManager.AxisMaximumValue = AxisManager.AxisMaximumValue;

                            // This interval will reflect the interval in primary axis it is not same as that of primary axis
                            AxisManager.Interval = (AxisManager.AxisMaximumValue - AxisManager.AxisMinimumValue) / primaryAxisIntervalCount;

                        }

                        // This interval will reflect the interval in primary axis it is not same as that of primary axis
                        //AxisManager.Interval = (AxisManager.AxisMaximumValue - AxisManager.AxisMinimumValue) / primaryAxisIntervalCount;

                        AxisManager.Calculate();
                    }
                }

                InternalAxisMaximum = AxisManager.AxisMaximumValue;
                InternalAxisMinimum = AxisManager.AxisMinimumValue;
            }
            else
            {
                InternalAxisMaximum = AxisManager.AxisMaximumValue;
                InternalAxisMinimum = AxisManager.AxisMinimumValue;
            }

            InternalInterval = AxisManager.Interval;
        }

        /// <summary>
        /// Applies setting for vertical type axis
        /// </summary>
        private void ApplyVerticalAxisSettings()
        {
            // Apply  settings based on the axis type
            switch (AxisType)
            {
                case AxisTypes.Primary:
                    ApplyVerticalPrimaryAxisSettings();
                    break;
                case AxisTypes.Secondary:
                    ApplyVerticalSecondaryAxisSettings();
                    break;
            }
        }

        /// <summary>
        /// Apply axis title properties
        /// </summary>
        private void ApplyTitleProperties()
        {
            #region Apply AxisTitle Properties

            if (AxisTitleElement != null)
            {
                AxisTitleElement.IsNotificationEnable = false;

                if (this.TitleFontFamily != null)
                    AxisTitleElement.InternalFontFamily = this.TitleFontFamily;

                if (this.TitleFontSize != 0)
                    AxisTitleElement.InternalFontSize = this.TitleFontSize;

                if (this.TitleFontStyle != null)
                    AxisTitleElement.InternalFontStyle = this.TitleFontStyle;

                if (this.TitleFontWeight != null)
                    AxisTitleElement.InternalFontWeight = this.TitleFontWeight;

                // if (!String.IsNullOrEmpty(this.Title) && String.IsNullOrEmpty(AxisTitleElement.Text))
                AxisTitleElement.Text = GetFormattedMultilineText(this.Title);

                AxisTitleElement.InternalFontColor = Visifire.Charts.Chart.CalculateFontColor((Chart as Chart), Background, this.TitleFontColor, false);

                AxisTitleElement.IsNotificationEnable = true;
            }

            #endregion
        }

        /// <summary>
        /// Create line for axis
        /// </summary>
        /// <param name="y1">Y1</param>
        /// <param name="y2">Y2</param>
        /// <param name="x1">X1</param>
        /// <param name="x2">X2</param>
        /// <param name="width">Axis width</param>
        /// <param name="height">Axis height</param>
        private void CreateAxisLine(Double y1, Double y2, Double x1, Double x2, Double width, Double height)
        {
            AxisLine = new Line() { Y1 = y1, Y2 = y2, X1 = x1, X2 = x2, Width = width, Height = height };
            AxisLine.StrokeThickness = (Double)LineThickness;
            AxisLine.Stroke = LineColor;
            AxisLine.StrokeDashArray = ExtendedGraphics.GetDashArray(LineStyle);
        }

        /// <summary>
        /// Clip vertical axis
        /// </summary>
        /// <param name="ticksWidth">Ticks width</param>
        private void ClipVerticalAxis(Double ticksWidth)
        {
            // Clip at top or bottom of the scrallable axis in order to avoid axislabel clip 
            if (Height != ScrollableSize)
            {
                // clip addition value at top or bottom of the scrallable axis in order to avoid axislabel clip 
                Double clipAdditionValue = 4;

                PathGeometry pathGeometry = new PathGeometry();

                pathGeometry.Figures = new PathFigureCollection();

                PathFigure pathFigure = new PathFigure();

                pathFigure.StartPoint = new Point(0, -(clipAdditionValue - 1));
                pathFigure.Segments = new PathSegmentCollection();

                // Do not change the order of the lines below
                // Segmens required to create the rectangle
                pathFigure.Segments.Add(Graphics.GetLineSegment(new Point(ScrollViewerElement.Width - ticksWidth, -(clipAdditionValue - 1))));
                pathFigure.Segments.Add(Graphics.GetLineSegment(new Point(ScrollViewerElement.Width - ticksWidth, 0)));
                pathFigure.Segments.Add(Graphics.GetLineSegment(new Point(ScrollViewerElement.Width, 0)));
                pathFigure.Segments.Add(Graphics.GetLineSegment(new Point(ScrollViewerElement.Width, Height)));
                pathFigure.Segments.Add(Graphics.GetLineSegment(new Point(ScrollViewerElement.Width - ticksWidth, Height)));
                pathFigure.Segments.Add(Graphics.GetLineSegment(new Point(ScrollViewerElement.Width - ticksWidth, Height + clipAdditionValue)));
                pathFigure.Segments.Add(Graphics.GetLineSegment(new Point(0, Height + clipAdditionValue)));
                pathGeometry.Figures.Add(pathFigure);
                ScrollViewerElement.Clip = pathGeometry;
            }
        }

        private Double GetSizeFromTrendLineLabels(Axis axis)
        {
            Double size = 0;

            List<TrendLine> trendLinesReferingToPrimaryAxesX;
            List<TrendLine> trendLinesReferingToPrimaryAxesY;
            List<TrendLine> trendLinesReferingToSecondaryAxesX;
            List<TrendLine> trendLinesReferingToSecondaryAxesY;

            Chart chart = Chart as Chart;
            if (PlotDetails.ChartOrientation == ChartOrientationType.Vertical)
            {
                trendLinesReferingToPrimaryAxesX = (from trendline in chart.TrendLines
                                                    where (trendline.Orientation == Orientation.Vertical) && (trendline.AxisType == AxisTypes.Primary)
                                                    select trendline).ToList();
                trendLinesReferingToPrimaryAxesY = (from trendline in chart.TrendLines
                                                    where (trendline.Orientation == Orientation.Horizontal) && (trendline.AxisType == AxisTypes.Primary)
                                                    select trendline).ToList();
                trendLinesReferingToSecondaryAxesX = (from trendline in chart.TrendLines
                                                      where (trendline.Orientation == Orientation.Vertical) && (trendline.AxisType == AxisTypes.Secondary)
                                                      select trendline).ToList();
                trendLinesReferingToSecondaryAxesY = (from trendline in chart.TrendLines
                                                      where (trendline.Orientation == Orientation.Horizontal) && (trendline.AxisType == AxisTypes.Secondary)
                                                      select trendline).ToList();
            }
            else
            {
                trendLinesReferingToPrimaryAxesX = (from trendline in chart.TrendLines
                                                    where (trendline.Orientation == Orientation.Horizontal) && (trendline.AxisType == AxisTypes.Primary)
                                                    select trendline).ToList();
                trendLinesReferingToPrimaryAxesY = (from trendline in chart.TrendLines
                                                    where (trendline.Orientation == Orientation.Vertical) && (trendline.AxisType == AxisTypes.Primary)
                                                    select trendline).ToList();
                trendLinesReferingToSecondaryAxesX = (from trendline in chart.TrendLines
                                                      where (trendline.Orientation == Orientation.Horizontal) && (trendline.AxisType == AxisTypes.Secondary)
                                                      select trendline).ToList();
                trendLinesReferingToSecondaryAxesY = (from trendline in chart.TrendLines
                                                      where (trendline.Orientation == Orientation.Vertical) && (trendline.AxisType == AxisTypes.Secondary)
                                                      select trendline).ToList();
            }

            if (axis.AxisRepresentation == AxisRepresentations.AxisX)
            {
                if (axis.AxisType == AxisTypes.Primary)
                    size = GetTrendLineLabelSize(trendLinesReferingToPrimaryAxesX, axis);
                else
                    size = GetTrendLineLabelSize(trendLinesReferingToSecondaryAxesX, axis);
            }
            else
            {
                if (axis.AxisType == AxisTypes.Primary)
                    size = GetTrendLineLabelSize(trendLinesReferingToPrimaryAxesY, axis);
                else
                    size = GetTrendLineLabelSize(trendLinesReferingToSecondaryAxesY, axis);
            }

            return size;
        }

        private Double GetTrendLineLabelSize(List<TrendLine> trendLines, Axis axis)
        {
            Double size = Double.MinValue;

            foreach (TrendLine trendLine in trendLines)
            {
                if ((Boolean)trendLine.Enabled)
                {
                    if (trendLine.LabelTextBlock != null)
                    {
#if WPF
                        Size textBlockSize = Graphics.CalculateVisualSize(trendLine.LabelTextBlock);
#else
                        Size textBlockSize = new Size(trendLine.LabelTextBlock.ActualWidth, trendLine.LabelTextBlock.ActualHeight);
#endif

                        if (PlotDetails.ChartOrientation == ChartOrientationType.Vertical)
                        {
                            if (axis.AxisOrientation == AxisOrientation.Horizontal)
                            {
                                if (axis.AxisType == AxisTypes.Primary)
                                    size = Math.Max(size, textBlockSize.Height);
                            }
                            else
                            {
                                if (axis.AxisType == AxisTypes.Primary)
                                    size = Math.Max(size, textBlockSize.Width);
                                else
                                    size = Math.Max(size, textBlockSize.Width);
                            }
                        }
                        else
                        {
                            if (axis.AxisOrientation == AxisOrientation.Vertical)
                            {
                                if (axis.AxisType == AxisTypes.Primary)
                                    size = Math.Max(size, textBlockSize.Width);
                            }
                            else
                            {
                                if (axis.AxisType == AxisTypes.Primary)
                                    size = Math.Max(size, textBlockSize.Height);
                                else
                                    size = Math.Max(size, textBlockSize.Height);
                            }
                        }
                    }
                }
            }

            return size;
        }

        /// <summary>
        /// Applies setting for primary vertical axis (Primary axis Y or Primary axis X in Bar)
        /// </summary>
        private void ApplyVerticalPrimaryAxisSettings()
        {   
            // Set the parameters fo the Axis Stack panel
            Visual.Children.Add(new Border() { Width = this.InternalPadding.Left });

            Visual.HorizontalAlignment = HorizontalAlignment.Left;
            Visual.VerticalAlignment = VerticalAlignment.Stretch;

            AxisElementsContainer.HorizontalAlignment = HorizontalAlignment.Right;
            AxisElementsContainer.VerticalAlignment = VerticalAlignment.Stretch;
            AxisElementsContainer.Orientation = Orientation.Horizontal;

            InternalStackPanel.Width = 0;
            InternalStackPanel.HorizontalAlignment = HorizontalAlignment.Left;
            InternalStackPanel.VerticalAlignment = VerticalAlignment.Stretch;
            ScrollViewerElement.VerticalAlignment = VerticalAlignment.Stretch;

            // Set the parameters for the scroll bar
            ScrollBarElement.Orientation = Orientation.Vertical;
            //ScrollBarElement.Width = 10;

            if (Double.IsNaN(ScrollBarSize))
            {
                IsNotificationEnable = false;
                ScrollBarSize = ScrollBarElement.Width;
                IsNotificationEnable = true;
            }
            else
            {
                ScrollBarElement.Width = ScrollBarSize;
            }

            // Set the parameters for the axis labels
            AxisLabels.Placement = PlacementTypes.Left;
            AxisLabels.Height = ScrollableSize;
            
            CreateAxisLine(StartOffset, Height - EndOffset, (Double)LineThickness / 2, (Double)LineThickness / 2, (Double)LineThickness, this.Height);

            // Set parameters for the Major Grids
            foreach (ChartGrid grid in Grids)
                grid.Placement = PlacementTypes.Left;

            // Generate the visual object for the required elements
            AxisLabels.CreateVisualObject();

            foreach (CustomAxisLabels customLabels in CustomAxisLabels)
            {
                customLabels.Placement = PlacementTypes.Left;
                customLabels.Height = ScrollableSize;
                customLabels.CreateVisualObject();
            }

            // Set the alignement for the axis Title
            if (AxisTitleElement != null)
            {   
                AxisTitleElement.InternalHorizontalAlignment = HorizontalAlignment.Left;
                AxisTitleElement.InternalVerticalAlignment = VerticalAlignment.Center;
            }

            CreateAxisTitleVisual(new Thickness(INNER_MARGIN, 0, INNER_MARGIN, 0));

            // Place the visual elements in the axis stack panel
            if (AxisTitleElement != null)
            {
                AxisElementsContainer.Children.Add(AxisTitleElement.Visual);
            }

            #region Set AxisLabels Width

            Double topOverflow = 0;
            Double bottomOverflow = 0;
            Double newTopOverflow = 0;
            Double newBottomOverflow = 0;

            if (AxisLabels.Visual != null)
            {
                Double totalAxisLabelsWidth = 0;

                if (AxisLabels.InternalMinWidth != 0 && AxisLabels.InternalMinWidth > AxisLabels.Visual.Width)
                    totalAxisLabelsWidth = AxisLabels.InternalMinWidth;
                else
                    totalAxisLabelsWidth = AxisLabels.Visual.Width;

                if (!Double.IsPositiveInfinity(AxisLabels.InternalMaxWidth) && AxisLabels.InternalMaxWidth < totalAxisLabelsWidth)
                {
                    AxisLabels.Visual.Width = AxisLabels.InternalMaxWidth;
                }
                else
                    AxisLabels.Visual.Width = totalAxisLabelsWidth;

                topOverflow = Math.Max(topOverflow, AxisLabels.TopOverflow);
                bottomOverflow = Math.Max(bottomOverflow, AxisLabels.BottomOverflow);

                if (!Double.IsPositiveInfinity(AxisLabels.InternalMaxWidth) && AxisLabels.InternalMaxWidth <= totalAxisLabelsWidth)
                {
                    GetNewOverflow4LeftLabels(topOverflow, bottomOverflow, ref newTopOverflow, ref newBottomOverflow, AxisLabels.Visual.Width);
                }
                else
                {
                    newBottomOverflow = bottomOverflow;
                    newTopOverflow = topOverflow;
                }

                RectangleGeometry clipRectangle = new RectangleGeometry();
                clipRectangle.Rect = new Rect(-1, -4 - newTopOverflow, AxisLabels.Visual.Width + 2, AxisLabels.Visual.Height + newTopOverflow + newBottomOverflow + 8);
                AxisLabels.Visual.Clip = clipRectangle;
            }

            
            #endregion

            #region Set CustomAxisLabels Width

            Double newTopOverflow4CustomLabels = 0;
            Double newBottomOverflow4CustomLabels = 0;
            Double topOverflow4CustomLabels = 0;
            Double bottomOverflow4CustomLabels = 0;

            List<CustomAxisLabels> reversedCustomAxisLabels = null;
            if (CustomAxisLabels.Count > 0)
            {
                reversedCustomAxisLabels = CustomAxisLabels.ToList();
                reversedCustomAxisLabels.Reverse();
                {
                    foreach (CustomAxisLabels customLabels in reversedCustomAxisLabels)
                    {
                        if (customLabels.Visual != null)
                        {
                            Double totalAxisLabelsWidth = 0;

                            if (customLabels.InternalMinHeight != 0 && customLabels.InternalMinWidth > customLabels.Visual.Width)
                                totalAxisLabelsWidth = customLabels.InternalMinWidth;
                            else
                                totalAxisLabelsWidth = customLabels.Visual.Width;

                            if (!Double.IsPositiveInfinity(customLabels.InternalMaxWidth) && customLabels.InternalMaxWidth < totalAxisLabelsWidth)
                            {
                                customLabels.Visual.Width = customLabels.InternalMaxWidth;
                            }
                            else
                                customLabels.Visual.Width = totalAxisLabelsWidth;

                            topOverflow4CustomLabels = Math.Max(topOverflow4CustomLabels, customLabels.TopOverflow);
                            bottomOverflow4CustomLabels = Math.Max(bottomOverflow4CustomLabels, customLabels.BottomOverflow);

                            if (!Double.IsPositiveInfinity(customLabels.InternalMaxWidth) && customLabels.InternalMaxWidth <= totalAxisLabelsWidth)
                            {
                                GetNewOverflow4LeftCustomLabels(customLabels, topOverflow4CustomLabels, bottomOverflow4CustomLabels, ref newTopOverflow4CustomLabels, ref newBottomOverflow4CustomLabels, customLabels.Visual.Width);
                            }
                            else
                            {
                                newTopOverflow4CustomLabels = topOverflow4CustomLabels;
                                newBottomOverflow4CustomLabels = bottomOverflow4CustomLabels;
                            }

                            RectangleGeometry clipRectangle = new RectangleGeometry();
                            clipRectangle.Rect = new Rect(-1, -4 - newTopOverflow4CustomLabels, customLabels.Visual.Width + 2, customLabels.Visual.Height + newTopOverflow4CustomLabels + newBottomOverflow4CustomLabels + 8);
                            customLabels.Visual.Clip = clipRectangle;

                        }
                    }
                }
            }

            if (topOverflow < topOverflow4CustomLabels)
                topOverflow = topOverflow4CustomLabels;
            if (bottomOverflow < bottomOverflow4CustomLabels)
                bottomOverflow = bottomOverflow4CustomLabels;

            #endregion

            if (reversedCustomAxisLabels != null)
            {
                foreach (CustomAxisLabels customLabels in reversedCustomAxisLabels)
                {
                    if (customLabels.Visual != null)
                    {
                        InternalStackPanel.Width = customLabels.Visual.Width;

                        if (Height == ScrollableSize)
                            AxisElementsContainer.Children.Add(customLabels.Visual);
                        else
                            InternalStackPanel.Children.Add(customLabels.Visual);
                    }
                }
            }

            if (AxisLabels.Visual != null)
            {
                InternalStackPanel.Width += AxisLabels.Visual.Width;

                if (Height == ScrollableSize)
                    AxisElementsContainer.Children.Add(AxisLabels.Visual);
                else
                {
                    InternalStackPanel.Children.Add(AxisLabels.Visual);
                    AxisLabels.Visual.SetValue(Canvas.LeftProperty, InternalStackPanel.Width - AxisLabels.Visual.Width);
                }
            }

            Double ticksWidth = 0;

            List<Ticks> ticks = Ticks.Reverse().ToList();

            foreach (Ticks tick in ticks)
            {
                tick.SetParms(PlacementTypes.Left, Double.NaN, ScrollableSize);

                tick.CreateVisualObject();

                if (tick.Visual != null)
                {
                    if (Height == ScrollableSize)
                        AxisElementsContainer.Children.Add(tick.Visual);
                    else
                    {
                        InternalStackPanel.Children.Add(tick.Visual);
                        tick.Visual.SetValue(Canvas.LeftProperty, InternalStackPanel.Width + ticksWidth);
                        ticksWidth += tick.Visual.Width;
                    }
                }
            }

            InternalStackPanel.Width += ticksWidth;

            if (Height != ScrollableSize)
            {
                ScrollViewerElement.Children.Add(InternalStackPanel);
                AxisElementsContainer.Children.Add(ScrollViewerElement);
            }

            AxisElementsContainer.Children.Add(AxisLine);

            InternalStackPanel.Width += AxisLine.Width;

            ScrollViewerElement.Width = InternalStackPanel.Width;

            ClipVerticalAxis(ticksWidth);

            #region Set Axis Width

            Double totalVisualWidth = 0;

            Size size = Graphics.CalculateVisualSize(AxisElementsContainer);

            if (InternalMinWidth != 0 && InternalMinWidth > size.Width)
                totalVisualWidth = InternalMinWidth;
            else
                totalVisualWidth = size.Width;

            if (!Double.IsPositiveInfinity(InternalMaxWidth) && InternalMaxWidth < totalVisualWidth)
            {
                totalVisualWidth = InternalMaxWidth;
            }
            //else
            //    AxisElementsContainer.Width = totalVisualWidth;

            Canvas axisContainer = new Canvas();
            axisContainer.HorizontalAlignment = HorizontalAlignment.Right;
            axisContainer.VerticalAlignment = VerticalAlignment.Stretch;
            axisContainer.Width = totalVisualWidth;
            axisContainer.Height = Height;

            axisContainer.Children.Add(AxisElementsContainer);
            AxisElementsContainer.SetValue(Canvas.LeftProperty, (Double)totalVisualWidth - size.Width);

            if (!Double.IsPositiveInfinity(InternalMaxWidth) && InternalMaxWidth <= totalVisualWidth)
            {
                if (AxisLabels.Visual != null)
                    GetNewOverflow4LeftLabels(topOverflow, bottomOverflow, ref newTopOverflow, ref newBottomOverflow, axisContainer.Width);

                if (reversedCustomAxisLabels != null)
                {
                    foreach (CustomAxisLabels customLabels in reversedCustomAxisLabels)
                    {
                        if (customLabels.Visual != null)
                        {
                            GetNewOverflow4LeftCustomLabels(customLabels, topOverflow4CustomLabels, bottomOverflow4CustomLabels, ref newTopOverflow4CustomLabels, ref newBottomOverflow4CustomLabels, axisContainer.Width);
                        }
                    }
                }

                if (newTopOverflow < newTopOverflow4CustomLabels)
                    newTopOverflow = newTopOverflow4CustomLabels;
                if (newBottomOverflow < newBottomOverflow4CustomLabels)
                    newBottomOverflow = newBottomOverflow4CustomLabels;
            }

            RectangleGeometry clipVisual = new RectangleGeometry();
            clipVisual.Rect = new Rect(-1, -4 - newTopOverflow, axisContainer.Width + 2, size.Height + newTopOverflow + newBottomOverflow + 8);
            axisContainer.Clip = clipVisual;

            #endregion

            Size visualSize = Graphics.CalculateVisualSize(Visual);

            Visual.Children.Add(axisContainer);

            visualSize.Width += axisContainer.Width;

            Double trendLineLabelsSize = GetSizeFromTrendLineLabels(this);

            if (visualSize.Width < trendLineLabelsSize)
            {
                (Visual.Children[0] as Border).Width = trendLineLabelsSize - visualSize.Width;

                visualSize.Width += (Visual.Children[0] as Border).Width - this.InternalPadding.Left;
            }

            Visual.Width = visualSize.Width;
            //Visual.Children.Add(AxisElementsContainer);
        }

        private void GetNewOverflow4LeftLabels(Double topOverflow, Double bottomOverflow, ref Double newTopOverflow, ref Double newBottomOverflow, Double visualWidth)
        {
            if ((Double)AxisLabels.InternalAngle > 0)
            {
                Double maxYPos = AxisLabels.AxisLabelList[AxisLabels.AxisLabelList.Count - 1].Position.Y;
                Point lastLabelPosition = new Point(AxisLabels.AxisLabelList[AxisLabels.AxisLabelList.Count - 1].Position.X, AxisLabels.AxisLabelList[AxisLabels.AxisLabelList.Count - 1].Position.Y);
                Point actualTopPosition = new Point(AxisLabels.AxisLabelList[AxisLabels.AxisLabelList.Count - 1].ActualLeft - AxisLabels.AxisLabelList[AxisLabels.AxisLabelList.Count - 1].ActualWidth, AxisLabels.AxisLabelList[AxisLabels.AxisLabelList.Count - 1].ActualTop);
                Point topOffsetPosition = new Point(lastLabelPosition.X, actualTopPosition.Y);

                Double width = Graphics.DistanceBetweenTwoPoints(topOffsetPosition, actualTopPosition);
                Double labelBase = Graphics.DistanceBetweenTwoPoints(topOffsetPosition, lastLabelPosition);
                Double theta = Math.Atan(width / labelBase);

                if (width > visualWidth)
                    width = visualWidth;

                topOverflow = (width / Math.Tan(theta));
                topOverflow = topOverflow - maxYPos;

                newTopOverflow = topOverflow;

                AxisLabels.TopOverflow = newTopOverflow;
            }
            else if ((Double)AxisLabels.InternalAngle < 0)
            {
                Double maxYPos = AxisLabels.AxisLabelList[0].Position.Y;
                Point firstLabelPosition = new Point(AxisLabels.AxisLabelList[0].Position.X, AxisLabels.AxisLabelList[0].Position.Y);
                Point actualBottomPosition = new Point(AxisLabels.AxisLabelList[0].ActualLeft, AxisLabels.AxisLabelList[0].ActualTop + AxisLabels.AxisLabelList[0].ActualHeight);
                Point bottomOffsetPosition = new Point(firstLabelPosition.X, actualBottomPosition.Y);

                Double width = Graphics.DistanceBetweenTwoPoints(bottomOffsetPosition, actualBottomPosition);
                Double labelBase = Graphics.DistanceBetweenTwoPoints(bottomOffsetPosition, firstLabelPosition);
                Double theta = Math.Atan(width / labelBase);

                if (width > visualWidth)
                    width = visualWidth;

                bottomOverflow = (width / Math.Tan(theta));
                bottomOverflow = bottomOverflow + maxYPos;

                newBottomOverflow = bottomOverflow - Height;

                AxisLabels.BottomOverflow = newBottomOverflow;
            }
        }

        private void GetNewOverflow4LeftCustomLabels(CustomAxisLabels customLabels, Double topOverflow, Double bottomOverflow, ref Double newTopOverflow, ref Double newBottomOverflow, Double visualWidth)
        {
            if ((Double)customLabels.InternalAngle > 0)
            {
                Double maxYPos = customLabels.AxisLabelList[customLabels.AxisLabelList.Count - 1].Position.Y;
                Point lastLabelPosition = new Point(customLabels.AxisLabelList[customLabels.AxisLabelList.Count - 1].Position.X, customLabels.AxisLabelList[customLabels.AxisLabelList.Count - 1].Position.Y);
                Point actualTopPosition = new Point(customLabels.AxisLabelList[customLabels.AxisLabelList.Count - 1].ActualLeft - customLabels.AxisLabelList[customLabels.AxisLabelList.Count - 1].ActualWidth, customLabels.AxisLabelList[customLabels.AxisLabelList.Count - 1].ActualTop);
                Point topOffsetPosition = new Point(lastLabelPosition.X, actualTopPosition.Y);

                Double width = Graphics.DistanceBetweenTwoPoints(topOffsetPosition, actualTopPosition);
                Double labelBase = Graphics.DistanceBetweenTwoPoints(topOffsetPosition, lastLabelPosition);
                Double theta = Math.Atan(width / labelBase);

                if (width > visualWidth)
                    width = visualWidth;

                topOverflow = (width / Math.Tan(theta));
                topOverflow = topOverflow - maxYPos;

                newTopOverflow = topOverflow;

                customLabels.TopOverflow = newTopOverflow;
            }
            else if ((Double)customLabels.InternalAngle < 0)
            {
                Double maxYPos = customLabels.AxisLabelList[0].Position.Y;
                Point firstLabelPosition = new Point(customLabels.AxisLabelList[0].Position.X, customLabels.AxisLabelList[0].Position.Y);
                Point actualBottomPosition = new Point(customLabels.AxisLabelList[0].ActualLeft, customLabels.AxisLabelList[0].ActualTop + customLabels.AxisLabelList[0].ActualHeight);
                Point bottomOffsetPosition = new Point(firstLabelPosition.X, actualBottomPosition.Y);

                Double width = Graphics.DistanceBetweenTwoPoints(bottomOffsetPosition, actualBottomPosition);
                Double labelBase = Graphics.DistanceBetweenTwoPoints(bottomOffsetPosition, firstLabelPosition);
                Double theta = Math.Atan(width / labelBase);

                if (width > visualWidth)
                    width = visualWidth;

                bottomOverflow = (width / Math.Tan(theta));
                bottomOverflow = bottomOverflow + maxYPos;

                newBottomOverflow = bottomOverflow - Height;

                customLabels.BottomOverflow = newBottomOverflow;
            }
        }

        /// <summary>
        /// Applies setting for secondary vertical axis (Secondary axis Y or Secondary axis X in Bar)
        /// </summary>
        private void ApplyVerticalSecondaryAxisSettings()
        {
            // Set the parameters fo the Axis Stack panel
            AxisElementsContainer.HorizontalAlignment = HorizontalAlignment.Left;
            AxisElementsContainer.VerticalAlignment = VerticalAlignment.Stretch;
            AxisElementsContainer.Orientation = Orientation.Horizontal;

            InternalStackPanel.HorizontalAlignment = HorizontalAlignment.Right;
            InternalStackPanel.VerticalAlignment = VerticalAlignment.Stretch;

            InternalStackPanel.SizeChanged += delegate(object sender, SizeChangedEventArgs e)
            {
                ScrollViewerElement.Width = e.NewSize.Width;
            };

            ScrollViewerElement.VerticalAlignment = VerticalAlignment.Stretch;

            // Set the parameters for the scroll bar
            ScrollBarElement.Orientation = Orientation.Vertical;
            //ScrollBarElement.Width = 10;

            if (Double.IsNaN(ScrollBarSize))
            {
                IsNotificationEnable = false;
                ScrollBarSize = ScrollBarElement.Width;
                IsNotificationEnable = true;
            }
            else
            {
                ScrollBarElement.Width = ScrollBarSize;
            }

            // Set the parameters for the axis labels
            AxisLabels.Placement = PlacementTypes.Right;
            AxisLabels.Height = ScrollableSize;

            CreateAxisLine(StartOffset, Height - EndOffset, (Double)LineThickness / 2, (Double)LineThickness / 2, (Double)LineThickness, this.Height);

            // Set parameters for the Major Grids
            foreach (ChartGrid grid in Grids)
                grid.Placement = PlacementTypes.Right;

            // Set the alignement for the axis Title
            if (AxisTitleElement != null)
            {
                AxisTitleElement.InternalHorizontalAlignment = HorizontalAlignment.Right;
                AxisTitleElement.InternalVerticalAlignment = VerticalAlignment.Center;
            }

            // Generate the visual object for the required elements
            AxisLabels.CreateVisualObject();

            foreach (CustomAxisLabels customLabels in CustomAxisLabels)
            {
                customLabels.Placement = PlacementTypes.Right;
                customLabels.Height = ScrollableSize;
                customLabels.CreateVisualObject();
            }

            // Place the visual elements in the axis stack panel
            AxisElementsContainer.Children.Add(AxisLine);

            foreach (Ticks tick in Ticks)
            {
                tick.SetParms(PlacementTypes.Right, Double.NaN, ScrollableSize);

                tick.CreateVisualObject();
                if (tick.Visual != null)
                {
                    if (Height == ScrollableSize)
                        AxisElementsContainer.Children.Add(tick.Visual);
                    else
                        InternalStackPanel.Children.Add(tick.Visual);

                }
            }

            #region Set AxisLabels Width

            Double topOverflow = 0;
            Double bottomOverflow = 0;
            Double newTopOverflow = 0;
            Double newBottomOverflow = 0;

            if (AxisLabels.Visual != null)
            {
                Double totalAxisLabelsWidth = 0;

                if (AxisLabels.InternalMinHeight != 0 && AxisLabels.InternalMinWidth > AxisLabels.Visual.Width)
                    totalAxisLabelsWidth = AxisLabels.InternalMinWidth;
                else
                    totalAxisLabelsWidth = AxisLabels.Visual.Width;

                if (!Double.IsPositiveInfinity(AxisLabels.InternalMaxWidth) && AxisLabels.InternalMaxWidth < totalAxisLabelsWidth)
                {
                    AxisLabels.Visual.Width = AxisLabels.InternalMaxWidth;
                }
                else
                    AxisLabels.Visual.Width = totalAxisLabelsWidth;

                topOverflow = Math.Max(topOverflow, AxisLabels.TopOverflow);
                bottomOverflow = Math.Max(bottomOverflow, AxisLabels.BottomOverflow);

                if (!Double.IsPositiveInfinity(AxisLabels.InternalMaxWidth) && AxisLabels.InternalMaxWidth <= totalAxisLabelsWidth)
                {
                    GetNewOverflow4RightLabels(topOverflow, bottomOverflow, ref newTopOverflow, ref newBottomOverflow, AxisLabels.Visual.Width);
                }
                else
                {
                    newBottomOverflow = bottomOverflow;
                    newTopOverflow = topOverflow;
                }

                RectangleGeometry clipRectangle = new RectangleGeometry();
                clipRectangle.Rect = new Rect(-1, -4 - newTopOverflow, AxisLabels.Visual.Width + 2, AxisLabels.Visual.Height + newTopOverflow + newBottomOverflow + 8);
                AxisLabels.Visual.Clip = clipRectangle;
            }

            #endregion

            #region Set CustomAxisLabels Width

            Double topOverflow4CustomLabels = 0;
            Double bottomOverflow4CustomLabels = 0;
            Double newTopOverflow4CustomLabels = 0;
            Double newBottomOverflow4CustomLabels = 0;

            foreach (CustomAxisLabels customLabels in CustomAxisLabels)
            {
                if (customLabels.Visual != null)
                {
                    Double totalAxisLabelsWidth = 0;

                    if (customLabels.InternalMinHeight != 0 && customLabels.InternalMinWidth > customLabels.Visual.Width)
                        totalAxisLabelsWidth = customLabels.InternalMinWidth;
                    else
                        totalAxisLabelsWidth = customLabels.Visual.Width;

                    if (!Double.IsPositiveInfinity(customLabels.InternalMaxWidth) && customLabels.InternalMaxWidth < totalAxisLabelsWidth)
                    {
                        customLabels.Visual.Width = customLabels.InternalMaxWidth;
                    }
                    else
                        customLabels.Visual.Width = totalAxisLabelsWidth;

                    topOverflow4CustomLabels = Math.Max(topOverflow4CustomLabels, customLabels.TopOverflow);
                    bottomOverflow4CustomLabels = Math.Max(bottomOverflow4CustomLabels, customLabels.BottomOverflow);

                    if (!Double.IsPositiveInfinity(customLabels.InternalMaxWidth) && customLabels.InternalMaxWidth <= totalAxisLabelsWidth)
                    {
                        GetNewOverflow4RightCustomLabels(customLabels, topOverflow4CustomLabels, bottomOverflow4CustomLabels, ref newTopOverflow4CustomLabels, ref newBottomOverflow4CustomLabels, customLabels.Visual.Width);
                    }
                    else
                    {
                        newBottomOverflow4CustomLabels = bottomOverflow4CustomLabels;
                        newTopOverflow4CustomLabels = topOverflow4CustomLabels;
                    }

                    RectangleGeometry clipRectangle = new RectangleGeometry();
                    clipRectangle.Rect = new Rect(-1, -4 - newTopOverflow4CustomLabels, customLabels.Visual.Width + 2, customLabels.Visual.Height + newTopOverflow4CustomLabels + newBottomOverflow4CustomLabels + 8);
                    customLabels.Visual.Clip = clipRectangle;
                }
            }

            if (topOverflow < topOverflow4CustomLabels)
                topOverflow = topOverflow4CustomLabels;
            if (bottomOverflow < bottomOverflow4CustomLabels)
                bottomOverflow = bottomOverflow4CustomLabels;

            #endregion

            if (Height == ScrollableSize)
            {   
                if (AxisLabels.Visual != null)
                    AxisElementsContainer.Children.Add(AxisLabels.Visual);

                foreach (CustomAxisLabels customLabels in CustomAxisLabels)
                {
                    if (customLabels.Visual != null)
                        AxisElementsContainer.Children.Add(customLabels.Visual);
                }
            }
            else
            {   
                InternalStackPanel.Children.Add(AxisLabels.Visual);

                foreach (CustomAxisLabels customLabels in CustomAxisLabels)
                {
                    if (customLabels.Visual != null)
                        InternalStackPanel.Children.Add(customLabels.Visual);
                }

                ScrollViewerElement.Children.Add(InternalStackPanel);

                AxisElementsContainer.Children.Add(ScrollViewerElement);
            }

            CreateAxisTitleVisual(new Thickness(INNER_MARGIN, 0, INNER_MARGIN, 0));

            if (AxisTitleElement != null)
            {
                AxisElementsContainer.Children.Add(AxisTitleElement.Visual);
            }

            #region Set Axis Width

            Double totalVisualWidth = 0;

            Size size = Graphics.CalculateVisualSize(AxisElementsContainer);

            if (InternalMinWidth != 0 && InternalMinWidth > size.Width)
                totalVisualWidth = InternalMinWidth;
            else
                totalVisualWidth = size.Width;

            if (!Double.IsPositiveInfinity(InternalMaxWidth) && InternalMaxWidth < totalVisualWidth)
            {
                totalVisualWidth = InternalMaxWidth;
            }
            //else
            //    AxisElementsContainer.Width = totalVisualWidth;

            #endregion

            Canvas axisContainer = new Canvas();
            axisContainer.HorizontalAlignment = HorizontalAlignment.Left;
            axisContainer.VerticalAlignment = VerticalAlignment.Stretch;
            axisContainer.Width = totalVisualWidth;
            axisContainer.Height = Height;

            axisContainer.Children.Add(AxisElementsContainer);
            //AxisElementsContainer.SetValue(Canvas.LeftProperty, (Double)totalVisualWidth - size.Width);

            if (!Double.IsPositiveInfinity(InternalMaxWidth) && InternalMaxWidth <= totalVisualWidth)
            {
                if (AxisLabels.Visual != null)
                    GetNewOverflow4RightLabels(topOverflow, bottomOverflow, ref newTopOverflow, ref newBottomOverflow, axisContainer.Width);

                foreach (CustomAxisLabels customLabels in CustomAxisLabels)
                {
                    if (customLabels.Visual != null)
                    {
                        GetNewOverflow4LeftCustomLabels(customLabels, topOverflow4CustomLabels, bottomOverflow4CustomLabels, ref newTopOverflow4CustomLabels, ref newBottomOverflow4CustomLabels, axisContainer.Width);
                    }
                }

                if (newTopOverflow < newTopOverflow4CustomLabels)
                    newTopOverflow = newTopOverflow4CustomLabels;
                if (newBottomOverflow < newBottomOverflow4CustomLabels)
                    newBottomOverflow = newBottomOverflow4CustomLabels;
            }

            RectangleGeometry clipVisual = new RectangleGeometry();
            clipVisual.Rect = new Rect(-1, -4 - newTopOverflow, axisContainer.Width + 2, size.Height + newTopOverflow + newBottomOverflow + 8);
            axisContainer.Clip = clipVisual;

            Visual.Children.Add(axisContainer);

            Size visualSize = Graphics.CalculateVisualSize(Visual);

            Visual.Children.Add(new Border() { Width = this.InternalPadding.Right });

            visualSize.Width += this.InternalPadding.Right;

            Double trendLineLabelsSize = GetSizeFromTrendLineLabels(this);

            if (visualSize.Width < trendLineLabelsSize)
            {
                (Visual.Children[Visual.Children.Count - 1] as Border).Width = trendLineLabelsSize - visualSize.Width;

                visualSize.Width += (Visual.Children[Visual.Children.Count - 1] as Border).Width - this.InternalPadding.Right;
            }

            Visual.Width = visualSize.Width;
        }

        private void GetNewOverflow4RightLabels(Double topOverflow, Double bottomOverflow, ref Double newTopOverflow, ref Double newBottomOverflow, Double visualWidth)
        {
            if ((Double)AxisLabels.InternalAngle < 0)
            {
                Double maxYPos = AxisLabels.AxisLabelList[AxisLabels.AxisLabelList.Count - 1].Position.Y;
                Point lastLabelPosition = new Point(AxisLabels.AxisLabelList[AxisLabels.AxisLabelList.Count - 1].Position.X, AxisLabels.AxisLabelList[AxisLabels.AxisLabelList.Count - 1].Position.Y);
                Point actualTopPosition = new Point(AxisLabels.AxisLabelList[AxisLabels.AxisLabelList.Count - 1].ActualLeft - AxisLabels.AxisLabelList[AxisLabels.AxisLabelList.Count - 1].ActualWidth, AxisLabels.AxisLabelList[AxisLabels.AxisLabelList.Count - 1].ActualTop);
                Point topOffsetPosition = new Point(lastLabelPosition.X, actualTopPosition.Y);

                Double width = Graphics.DistanceBetweenTwoPoints(topOffsetPosition, actualTopPosition);
                Double labelBase = Graphics.DistanceBetweenTwoPoints(topOffsetPosition, lastLabelPosition);
                Double theta = Math.Atan(width / labelBase);

                if (width > visualWidth)
                    width = visualWidth;

                topOverflow = (width / Math.Tan(theta));
                topOverflow = topOverflow - maxYPos;

                newTopOverflow = topOverflow;

                AxisLabels.TopOverflow = newTopOverflow;
            }
            else if ((Double)AxisLabels.InternalAngle > 0)
            {
                Double maxYPos = AxisLabels.AxisLabelList[0].Position.Y;
                Point firstLabelPosition = new Point(AxisLabels.AxisLabelList[0].Position.X, AxisLabels.AxisLabelList[0].Position.Y);
                Point actualBottomPosition = new Point(AxisLabels.AxisLabelList[0].ActualLeft, AxisLabels.AxisLabelList[0].ActualTop + AxisLabels.AxisLabelList[0].ActualHeight);
                Point bottomOffsetPosition = new Point(firstLabelPosition.X, actualBottomPosition.Y);

                Double width = Graphics.DistanceBetweenTwoPoints(bottomOffsetPosition, actualBottomPosition);
                Double labelBase = Graphics.DistanceBetweenTwoPoints(bottomOffsetPosition, firstLabelPosition);
                Double theta = Math.Atan(width / labelBase);

                if (width > visualWidth)
                    width = visualWidth;

                bottomOverflow = (width / Math.Tan(theta));
                bottomOverflow = bottomOverflow + maxYPos;

                newBottomOverflow = bottomOverflow - Height;

                AxisLabels.BottomOverflow = newBottomOverflow;
            }
        }

        private void GetNewOverflow4RightCustomLabels(CustomAxisLabels customLabels, Double topOverflow, Double bottomOverflow, ref Double newTopOverflow, ref Double newBottomOverflow, Double visualWidth)
        {
            if ((Double)customLabels.InternalAngle < 0)
            {
                Double maxYPos = customLabels.AxisLabelList[customLabels.AxisLabelList.Count - 1].Position.Y;
                Point lastLabelPosition = new Point(customLabels.AxisLabelList[customLabels.AxisLabelList.Count - 1].Position.X, customLabels.AxisLabelList[customLabels.AxisLabelList.Count - 1].Position.Y);
                Point actualTopPosition = new Point(customLabels.AxisLabelList[customLabels.AxisLabelList.Count - 1].ActualLeft - customLabels.AxisLabelList[customLabels.AxisLabelList.Count - 1].ActualWidth, customLabels.AxisLabelList[customLabels.AxisLabelList.Count - 1].ActualTop);
                Point topOffsetPosition = new Point(lastLabelPosition.X, actualTopPosition.Y);

                Double width = Graphics.DistanceBetweenTwoPoints(topOffsetPosition, actualTopPosition);
                Double labelBase = Graphics.DistanceBetweenTwoPoints(topOffsetPosition, lastLabelPosition);
                Double theta = Math.Atan(width / labelBase);

                if (width > visualWidth)
                    width = visualWidth;

                topOverflow = (width / Math.Tan(theta));
                topOverflow = topOverflow - maxYPos;

                newTopOverflow = topOverflow;

                customLabels.TopOverflow = newTopOverflow;
            }
            else if ((Double)customLabels.InternalAngle > 0)
            {
                Double maxYPos = customLabels.AxisLabelList[0].Position.Y;
                Point firstLabelPosition = new Point(customLabels.AxisLabelList[0].Position.X, customLabels.AxisLabelList[0].Position.Y);
                Point actualBottomPosition = new Point(customLabels.AxisLabelList[0].ActualLeft, customLabels.AxisLabelList[0].ActualTop + customLabels.AxisLabelList[0].ActualHeight);
                Point bottomOffsetPosition = new Point(firstLabelPosition.X, actualBottomPosition.Y);

                Double width = Graphics.DistanceBetweenTwoPoints(bottomOffsetPosition, actualBottomPosition);
                Double labelBase = Graphics.DistanceBetweenTwoPoints(bottomOffsetPosition, firstLabelPosition);
                Double theta = Math.Atan(width / labelBase);

                if (width > visualWidth)
                    width = visualWidth;

                bottomOverflow = (width / Math.Tan(theta));
                bottomOverflow = bottomOverflow + maxYPos;

                newBottomOverflow = bottomOverflow - Height;

                customLabels.BottomOverflow = newBottomOverflow;
            }
        }

        /// <summary>
        /// Applies setting for primary horizontal axis (Primary axis X or Primary axis Y in Bar)
        /// </summary>
        private void ApplyHorizontalPrimaryAxisSettings()
        {
            // Set the parameters for the Axis Stack panel
            AxisElementsContainer.HorizontalAlignment = HorizontalAlignment.Stretch;
            AxisElementsContainer.VerticalAlignment = VerticalAlignment.Bottom;
            AxisElementsContainer.Orientation = Orientation.Vertical;
            InternalStackPanel.Height = 0;
            InternalStackPanel.HorizontalAlignment = HorizontalAlignment.Stretch;
            InternalStackPanel.VerticalAlignment = VerticalAlignment.Bottom;

            ScrollViewerElement.HorizontalAlignment = HorizontalAlignment.Stretch;

            // Set the parameters for the scroll bar
            ScrollBarElement.Orientation = Orientation.Horizontal;
            //ScrollBarElement.Height = 10;

            if (Double.IsNaN(ScrollBarSize))
            {
                IsNotificationEnable = false;
                ScrollBarSize = ScrollBarElement.Height;
                IsNotificationEnable = true;
            }
            else
            {
                ScrollBarElement.Height = ScrollBarSize;
            }

            // Set the parameters for the axis labels
            AxisLabels.Placement = PlacementTypes.Bottom;
            AxisLabels.Width = ScrollableSize;

            CreateAxisLine((Double)LineThickness / 2, (Double)LineThickness / 2, StartOffset, Width - EndOffset, this.Width, (Double)LineThickness);

            // Set parameters for the Major Grids
            foreach (ChartGrid grid in Grids)
                grid.Placement = PlacementTypes.Bottom;

            // Set the alignement for the axis Title
            if (AxisTitleElement != null)
            {
                AxisTitleElement.InternalHorizontalAlignment = HorizontalAlignment.Center;
                AxisTitleElement.InternalVerticalAlignment = VerticalAlignment.Bottom;
            }

            // Generate the visual object for the required elements
            AxisLabels.CreateVisualObject();

            foreach (CustomAxisLabels customLabels in CustomAxisLabels)
            {
                customLabels.Placement = PlacementTypes.Bottom;
                customLabels.Width = ScrollableSize;
                customLabels.CreateVisualObject();
            }

            // Place the visual elements in the axis stack panel
            AxisElementsContainer.Children.Add(AxisLine);

            Double ticksHeight = 0;
            //AxisLabels.Visual.Background = new SolidColorBrush(Colors.Orange);
            foreach (Ticks tick in Ticks)
            {
                tick.SetParms(PlacementTypes.Bottom, ScrollableSize, Double.NaN);

                tick.CreateVisualObject();
                if (tick.Visual != null)
                {
                    if (Width == ScrollableSize)
                        AxisElementsContainer.Children.Add(tick.Visual);
                    else
                    {
                        InternalStackPanel.Children.Add(tick.Visual);
                        tick.Visual.SetValue(Canvas.TopProperty, ticksHeight);
                        ticksHeight += tick.Visual.Height;
                    }
                }
            }

            InternalStackPanel.Height += ticksHeight;

            #region Set AxisLabels Height

            Double newLeftOverflow = 0;
            Double newRightOverflow = 0;
            Double leftOverflow = 0;
            Double rightOverflow = 0;

            if (AxisLabels.Visual != null)
            {
                Double totalAxisLabelsHeight = 0;

                if (AxisLabels.InternalMinHeight != 0 && AxisLabels.InternalMinHeight > AxisLabels.Visual.Height)
                    totalAxisLabelsHeight = AxisLabels.InternalMinHeight;
                else
                    totalAxisLabelsHeight = AxisLabels.Visual.Height;

                if (!Double.IsPositiveInfinity(AxisLabels.InternalMaxHeight) && AxisLabels.InternalMaxHeight < totalAxisLabelsHeight)
                {
                    AxisLabels.Visual.Height = AxisLabels.InternalMaxHeight;
                }
                else
                    AxisLabels.Visual.Height = totalAxisLabelsHeight;

                leftOverflow = Math.Max(leftOverflow, AxisLabels.LeftOverflow);
                rightOverflow = Math.Max(rightOverflow, AxisLabels.RightOverflow);

                if (!Double.IsPositiveInfinity(AxisLabels.InternalMaxHeight) && AxisLabels.InternalMaxHeight <= totalAxisLabelsHeight)
                {
                    GetNewOverflow4BottomLabels(leftOverflow, rightOverflow, ref newLeftOverflow, ref newRightOverflow, AxisLabels.Visual.Height);
                }
                else
                {
                    newLeftOverflow = leftOverflow;
                    newRightOverflow = rightOverflow;
                }

                RectangleGeometry clipRectangle = new RectangleGeometry();
                clipRectangle.Rect = new Rect(-4 - newLeftOverflow, 0, AxisLabels.Visual.Width + newLeftOverflow + newRightOverflow + 8, AxisLabels.Visual.Height);
                AxisLabels.Visual.Clip = clipRectangle;

            }

            #endregion

            #region Set CustomAxisLabels Height

            Double newLeftOverflow4CustomLabels = 0;
            Double newRightOverflow4CustomLabels = 0;
            Double leftOverflow4CustomLabels = 0;
            Double rightOverflow4CustomLabels = 0;

            foreach (CustomAxisLabels customLabels in CustomAxisLabels)
            {
                if (customLabels.Visual != null)
                {
                    Double totalAxisLabelsHeight = 0;

                    if (customLabels.InternalMinHeight != 0 && customLabels.InternalMinHeight > customLabels.Visual.Height)
                        totalAxisLabelsHeight = customLabels.InternalMinHeight;
                    else
                        totalAxisLabelsHeight = customLabels.Visual.Height;

                    if (!Double.IsPositiveInfinity(customLabels.InternalMaxHeight) && customLabels.InternalMaxHeight < totalAxisLabelsHeight)
                    {
                        customLabels.Visual.Height = customLabels.InternalMaxHeight;
                    }
                    else
                        customLabels.Visual.Height = totalAxisLabelsHeight;

                    leftOverflow4CustomLabels = Math.Max(leftOverflow4CustomLabels, customLabels.LeftOverflow);
                    rightOverflow4CustomLabels = Math.Max(rightOverflow4CustomLabels, customLabels.RightOverflow);

                    if (!Double.IsPositiveInfinity(customLabels.InternalMaxHeight) && customLabels.InternalMaxHeight <= totalAxisLabelsHeight)
                    {
                        GetNewOverflow4BottomCustomLabels(customLabels, leftOverflow4CustomLabels, rightOverflow4CustomLabels, ref newLeftOverflow4CustomLabels, ref newRightOverflow4CustomLabels, customLabels.Visual.Height);
                    }
                    else
                    {
                        newLeftOverflow4CustomLabels = leftOverflow4CustomLabels;
                        newRightOverflow4CustomLabels = rightOverflow4CustomLabels;
                    }

                    RectangleGeometry clipRectangle = new RectangleGeometry();
                    clipRectangle.Rect = new Rect(-4 - newLeftOverflow4CustomLabels, 0, customLabels.Visual.Width + newLeftOverflow4CustomLabels + newRightOverflow4CustomLabels + 8, customLabels.Visual.Height);
                    customLabels.Visual.Clip = clipRectangle;

                }
            }

            if (leftOverflow < leftOverflow4CustomLabels)
                leftOverflow = leftOverflow4CustomLabels;
            if (rightOverflow < rightOverflow4CustomLabels)
                rightOverflow = rightOverflow4CustomLabels;

            #endregion

            if (Width == ScrollableSize)
            {
                if (AxisLabels.Visual != null)
                    AxisElementsContainer.Children.Add(AxisLabels.Visual);

                foreach (CustomAxisLabels customLabels in CustomAxisLabels)
                {
                    if (customLabels.Visual != null)
                        AxisElementsContainer.Children.Add(customLabels.Visual);
                }
            }
            else
            {
                if (AxisLabels.Visual != null)
                {
                    InternalStackPanel.Width = AxisLabels.Visual.Width;
                    AxisLabels.Visual.SetValue(Canvas.TopProperty, InternalStackPanel.Height);
                    InternalStackPanel.Children.Add(AxisLabels.Visual);
                    InternalStackPanel.Height += AxisLabels.Visual.Height;
                }

                foreach (CustomAxisLabels customLabels in CustomAxisLabels)
                {
                    if (customLabels.Visual != null)
                    {
                        InternalStackPanel.Width = customLabels.Visual.Width;
                        customLabels.Visual.SetValue(Canvas.TopProperty, InternalStackPanel.Height);
                        InternalStackPanel.Children.Add(customLabels.Visual);
                        InternalStackPanel.Height += customLabels.Visual.Height;
                    }
                }

                ScrollViewerElement.Children.Add(InternalStackPanel);
                AxisElementsContainer.Children.Add(ScrollViewerElement);
            }

            ScrollViewerElement.Height = InternalStackPanel.Height;

            ClipHorizontalAxis(ticksHeight);

            CreateAxisTitleVisual(new Thickness(0, INNER_MARGIN, 0, INNER_MARGIN));

            if (AxisTitleElement != null)
            {   
                AxisElementsContainer.Children.Add(AxisTitleElement.Visual);
            }

            #region Set Axis Height

            Double totalVisualHeight = 0;

            Size size = Graphics.CalculateVisualSize(AxisElementsContainer);

            if (InternalMinHeight != 0 && InternalMinHeight > size.Height)
                totalVisualHeight = InternalMinHeight;
            else
                totalVisualHeight = size.Height;

            if (!Double.IsPositiveInfinity(InternalMaxHeight) && InternalMaxHeight < totalVisualHeight)
            {
                totalVisualHeight = InternalMaxHeight;
            }

            Canvas axisContainer = new Canvas();
            axisContainer.HorizontalAlignment = HorizontalAlignment.Stretch;
            axisContainer.VerticalAlignment = VerticalAlignment.Bottom;
            axisContainer.Height = totalVisualHeight;
            axisContainer.Width = Width;

            axisContainer.Children.Add(AxisElementsContainer);

            if (!Double.IsPositiveInfinity(InternalMaxHeight) && InternalMaxHeight <= totalVisualHeight)
            {
                if (AxisLabels.Visual != null)
                    GetNewOverflow4BottomLabels(leftOverflow, rightOverflow, ref newLeftOverflow, ref newRightOverflow, axisContainer.Height);

                foreach (CustomAxisLabels customLabels in CustomAxisLabels)
                {
                    if (customLabels.Visual != null)
                    {
                        GetNewOverflow4BottomCustomLabels(customLabels, leftOverflow4CustomLabels, rightOverflow4CustomLabels, ref newLeftOverflow4CustomLabels, ref newRightOverflow4CustomLabels, axisContainer.Height);
                    }
                }


                if (newLeftOverflow < newLeftOverflow4CustomLabels)
                    newLeftOverflow = newLeftOverflow4CustomLabels;
                if (newRightOverflow < newRightOverflow4CustomLabels)
                    newRightOverflow = newRightOverflow4CustomLabels;
            }
            else
            {
                newLeftOverflow = leftOverflow;
                newRightOverflow = rightOverflow;
            }

            RectangleGeometry clipVisual = new RectangleGeometry();
            clipVisual.Rect = new Rect(-4 - newLeftOverflow, 0, axisContainer.Width + newLeftOverflow + newRightOverflow + 8, axisContainer.Height);
            axisContainer.Clip = clipVisual;

            #endregion

            Visual.Children.Add(axisContainer);

            Visual.Children.Add(new Border() { Height = this.InternalPadding.Bottom });

            Size visualSize = Graphics.CalculateVisualSize(Visual);

            Double trendLineLabelsSize = GetSizeFromTrendLineLabels(this);

            if (visualSize.Height < trendLineLabelsSize)
            {
                (Visual.Children[Visual.Children.Count - 1] as Border).Height = trendLineLabelsSize - visualSize.Height;
                visualSize.Height += (Visual.Children[Visual.Children.Count - 1] as Border).Height - this.InternalPadding.Bottom;
            }

            Visual.Height = visualSize.Height;
        }

        private void GetNewOverflow4BottomLabels(Double leftOverflow, Double rightOverflow, ref Double newLeftOverflow, ref Double newRightOverflow, Double visualHeight)
        {
            if ((Double)AxisLabels.InternalAngle < 0)
            {
                Double maxXPos = AxisLabels.AxisLabelList[0].Position.X;
                Point firstLabelPosition = new Point(AxisLabels.AxisLabelList[0].Position.X, AxisLabels.AxisLabelList[0].Position.Y);
                Point actualLeftPosition = new Point(AxisLabels.AxisLabelList[0].ActualLeft, AxisLabels.AxisLabelList[0].ActualHeight);
                Point leftOffsetPosition = new Point(actualLeftPosition.X, firstLabelPosition.Y);

                Double height = Graphics.DistanceBetweenTwoPoints(leftOffsetPosition, actualLeftPosition);
                Double labelBase = Graphics.DistanceBetweenTwoPoints(leftOffsetPosition, firstLabelPosition);
                Double theta = Math.Atan(height / labelBase);

                if (height > visualHeight)
                    height = visualHeight;

                leftOverflow = (height / Math.Tan(theta));
                leftOverflow = leftOverflow - maxXPos;

                newLeftOverflow = leftOverflow;

                AxisLabels.LeftOverflow = newLeftOverflow;
            }
            else if ((Double)AxisLabels.InternalAngle > 0)
            {
                Double maxXPos = AxisLabels.AxisLabelList[AxisLabels.AxisLabelList.Count - 1].Position.X;
                Point lastLabelPosition = new Point(AxisLabels.AxisLabelList[AxisLabels.AxisLabelList.Count - 1].Position.X, AxisLabels.AxisLabelList[AxisLabels.AxisLabelList.Count - 1].Position.Y);
                Point actualRightPosition = new Point(AxisLabels.AxisLabelList[AxisLabels.AxisLabelList.Count - 1].ActualLeft + AxisLabels.AxisLabelList[AxisLabels.AxisLabelList.Count - 1].ActualWidth, AxisLabels.AxisLabelList[AxisLabels.AxisLabelList.Count - 1].ActualHeight);
                Point rightOffsetPosition = new Point(actualRightPosition.X, lastLabelPosition.Y);

                Double height = Graphics.DistanceBetweenTwoPoints(rightOffsetPosition, actualRightPosition);
                Double labelBase = Graphics.DistanceBetweenTwoPoints(rightOffsetPosition, lastLabelPosition);
                Double theta = Math.Atan(height / labelBase);

                if (height > visualHeight)
                    height = visualHeight;

                rightOverflow = (height / Math.Tan(theta));
                rightOverflow = rightOverflow + maxXPos;

                rightOverflow = rightOverflow - Width;

                newRightOverflow = rightOverflow;

                AxisLabels.RightOverflow = newRightOverflow;
            }
        }

        private void GetNewOverflow4BottomCustomLabels(CustomAxisLabels customLabels, Double leftOverflow, Double rightOverflow, ref Double newLeftOverflow, ref Double newRightOverflow, Double visualHeight)
        {
            if ((Double)customLabels.InternalAngle < 0)
            {
                Double maxXPos = customLabels.AxisLabelList[0].Position.X;
                Point firstLabelPosition = new Point(customLabels.AxisLabelList[0].Position.X, customLabels.AxisLabelList[0].Position.Y);
                Point actualLeftPosition = new Point(customLabels.AxisLabelList[0].ActualLeft, customLabels.AxisLabelList[0].ActualHeight);
                Point leftOffsetPosition = new Point(actualLeftPosition.X, firstLabelPosition.Y);

                Double height = Graphics.DistanceBetweenTwoPoints(leftOffsetPosition, actualLeftPosition);
                Double labelBase = Graphics.DistanceBetweenTwoPoints(leftOffsetPosition, firstLabelPosition);
                Double theta = Math.Atan(height / labelBase);

                if (height > visualHeight)
                    height = visualHeight;

                leftOverflow = (height / Math.Tan(theta));
                leftOverflow = leftOverflow - maxXPos;

                newLeftOverflow = leftOverflow;

                customLabels.LeftOverflow = newLeftOverflow;
            }
            else if ((Double)customLabels.InternalAngle > 0)
            {
                Double maxXPos = customLabels.AxisLabelList[customLabels.AxisLabelList.Count - 1].Position.X;
                Point lastLabelPosition = new Point(customLabels.AxisLabelList[customLabels.AxisLabelList.Count - 1].Position.X, customLabels.AxisLabelList[customLabels.AxisLabelList.Count - 1].Position.Y);
                Point actualRightPosition = new Point(customLabels.AxisLabelList[customLabels.AxisLabelList.Count - 1].ActualLeft + customLabels.AxisLabelList[customLabels.AxisLabelList.Count - 1].ActualWidth, customLabels.AxisLabelList[customLabels.AxisLabelList.Count - 1].ActualHeight);
                Point rightOffsetPosition = new Point(actualRightPosition.X, lastLabelPosition.Y);

                Double height = Graphics.DistanceBetweenTwoPoints(rightOffsetPosition, actualRightPosition);
                Double labelBase = Graphics.DistanceBetweenTwoPoints(rightOffsetPosition, lastLabelPosition);
                Double theta = Math.Atan(height / labelBase);

                if (height > visualHeight)
                    height = visualHeight;

                rightOverflow = (height / Math.Tan(theta));
                rightOverflow = rightOverflow + maxXPos;

                rightOverflow = rightOverflow - Width;

                newRightOverflow = rightOverflow;

                customLabels.RightOverflow = newRightOverflow;
            }
        }

        /// <summary>
        /// Clip horizontal axis
        /// </summary>
        /// <param name="ticksHeight">Ticks height</param>
        private void ClipHorizontalAxis(Double ticksHeight)
        {
            // Clip at left or right the scrallable axis in order to avoid axislabel clip 
            if (Width != ScrollableSize)
            {
                // clip addition value at right or left of the scrallable axis in order to avoid axislabel clip 
                Double clipAdditionValue = 4;

                PathGeometry pathGeometry = new PathGeometry();

                pathGeometry.Figures = new PathFigureCollection();

                PathFigure pathFigure = new PathFigure();

                pathFigure.StartPoint = new Point(0, 0);
                pathFigure.Segments = new PathSegmentCollection();

                // Do not change the order of the lines below
                pathFigure.Segments.Add(Graphics.GetLineSegment(new Point(Width, 0)));
                pathFigure.Segments.Add(Graphics.GetLineSegment(new Point(Width, ticksHeight)));
                pathFigure.Segments.Add(Graphics.GetLineSegment(new Point(Width + clipAdditionValue, ticksHeight)));
                pathFigure.Segments.Add(Graphics.GetLineSegment(new Point(Width + clipAdditionValue, ScrollViewerElement.Height)));
                pathFigure.Segments.Add(Graphics.GetLineSegment(new Point(-clipAdditionValue, ScrollViewerElement.Height)));
                pathFigure.Segments.Add(Graphics.GetLineSegment(new Point(-clipAdditionValue, ticksHeight)));
                pathFigure.Segments.Add(Graphics.GetLineSegment(new Point(0, ticksHeight)));

                pathGeometry.Figures.Add(pathFigure);
                ScrollViewerElement.Clip = pathGeometry;
            }
        }

        /// <summary>
        /// Applies setting for secondary horizontal axis (Secondary axis X or Secondary axis Y in Bar)
        /// </summary>
        private void ApplyHorizontalSecondaryAxisSettings()
        {
            // Set the parameters fo the Axis Stack panel
            Visual.Children.Add(new Border() { Height = this.InternalPadding.Top });
            AxisElementsContainer.HorizontalAlignment = HorizontalAlignment.Stretch;
            AxisElementsContainer.VerticalAlignment = VerticalAlignment.Top;
            AxisElementsContainer.Orientation = Orientation.Vertical;

            InternalStackPanel.HorizontalAlignment = HorizontalAlignment.Stretch;
            InternalStackPanel.VerticalAlignment = VerticalAlignment.Top;

            InternalStackPanel.SizeChanged += delegate(object sender, SizeChangedEventArgs e)
            {
                ScrollViewerElement.Height = e.NewSize.Height;
            };

            ScrollViewerElement.HorizontalAlignment = HorizontalAlignment.Stretch;

            // Set the parameters for the scroll bar
            ScrollBarElement.Orientation = Orientation.Horizontal;
            //ScrollBarElement.Height = 10;

            if (Double.IsNaN(ScrollBarSize))
            {
                IsNotificationEnable = false;
                ScrollBarSize = ScrollBarElement.Height;
                IsNotificationEnable = true;
            }
            else
            {
                ScrollBarElement.Height = ScrollBarSize;
            }

            // Set the parameters for the axis labels
            AxisLabels.Placement = PlacementTypes.Top;
            AxisLabels.Width = ScrollableSize;

            CreateAxisLine((Double)LineThickness / 2, (Double)LineThickness / 2, StartOffset, Width - EndOffset, this.Width, (Double)LineThickness);

            // Set parameters for the Major Grids
            foreach (ChartGrid grid in Grids)
                grid.Placement = PlacementTypes.Top;

            // Set the alignement for the axis Title
            if (AxisTitleElement != null)
            {
                AxisTitleElement.InternalHorizontalAlignment = HorizontalAlignment.Center;
                AxisTitleElement.InternalVerticalAlignment = VerticalAlignment.Top;
            }

            AxisLabels.CreateVisualObject();

            foreach (CustomAxisLabels customLabels in CustomAxisLabels)
            {
                customLabels.Placement = PlacementTypes.Top;
                customLabels.Width = ScrollableSize;
                customLabels.CreateVisualObject();
            }

            CreateAxisTitleVisual(new Thickness(0, INNER_MARGIN, 0, INNER_MARGIN));

            // Place the visual elements in the axis stack panel
            if (AxisTitleElement != null)
            {
                AxisElementsContainer.Children.Add(AxisTitleElement.Visual);
            }

            #region Set AxisLabels Height

            Double leftOverflow = 0;
            Double rightOverflow = 0;
            Double newLeftOverflow = 0;
            Double newRightOverflow = 0;

            if (AxisLabels.Visual != null)
            {
                Double totalAxisLabelsHeight = 0;

                if (AxisLabels.InternalMinHeight != 0 && AxisLabels.InternalMinHeight > AxisLabels.Visual.Height)
                    totalAxisLabelsHeight = AxisLabels.InternalMinHeight;
                else
                    totalAxisLabelsHeight = AxisLabels.Visual.Height;

                if (!Double.IsPositiveInfinity(AxisLabels.InternalMaxHeight) && AxisLabels.InternalMaxHeight < totalAxisLabelsHeight)
                {
                    AxisLabels.Visual.Height = AxisLabels.InternalMaxHeight;
                }
                else
                    AxisLabels.Visual.Height = totalAxisLabelsHeight;

                leftOverflow = Math.Max(leftOverflow, AxisLabels.LeftOverflow);
                rightOverflow = Math.Max(rightOverflow, AxisLabels.RightOverflow);

                if (!Double.IsPositiveInfinity(AxisLabels.InternalMaxHeight) && AxisLabels.InternalMaxHeight <= totalAxisLabelsHeight)
                {
                    GetNewOverflow4TopLabels(leftOverflow, rightOverflow, ref newLeftOverflow, ref newRightOverflow, AxisLabels.Visual.Height);
                }
                else
                {
                    newLeftOverflow = leftOverflow;
                    newRightOverflow = rightOverflow;
                }

                RectangleGeometry clipRectangle = new RectangleGeometry();
                clipRectangle.Rect = new Rect(-4 - newLeftOverflow, 0, AxisLabels.Visual.Width + newLeftOverflow + newRightOverflow + 8, AxisLabels.Visual.Height);
                AxisLabels.Visual.Clip = clipRectangle;
            }

            #endregion

            #region Set CustomAxisLabels Height

            Double leftOverflow4CustomLabels = 0;
            Double rightOverflow4CustomLabels = 0;
            Double newLeftOverflow4CustomLabels = 0;
            Double newRightOverflow4CustomLabels = 0;

            List<CustomAxisLabels> reversedCustomAxisLabels = null;
            if (CustomAxisLabels.Count > 0)
            {
                reversedCustomAxisLabels = CustomAxisLabels.ToList();
                reversedCustomAxisLabels.Reverse();
                {
                    foreach (CustomAxisLabels customLabels in reversedCustomAxisLabels)
                    {
                        if (customLabels.Visual != null)
                        {
                            Double totalAxisLabelsHeight = 0;

                            if (customLabels.InternalMinHeight != 0 && customLabels.InternalMinHeight > customLabels.Visual.Height)
                                totalAxisLabelsHeight = customLabels.InternalMinHeight;
                            else
                                totalAxisLabelsHeight = customLabels.Visual.Height;

                            if (!Double.IsPositiveInfinity(customLabels.InternalMaxHeight) && customLabels.InternalMaxHeight < totalAxisLabelsHeight)
                            {
                                customLabels.Visual.Height = customLabels.InternalMaxHeight;
                            }
                            else
                                customLabels.Visual.Height = totalAxisLabelsHeight;

                            leftOverflow4CustomLabels = Math.Max(leftOverflow4CustomLabels, customLabels.LeftOverflow);
                            rightOverflow4CustomLabels = Math.Max(rightOverflow4CustomLabels, customLabels.RightOverflow);

                            if (!Double.IsPositiveInfinity(customLabels.InternalMaxHeight) && customLabels.InternalMaxHeight <= totalAxisLabelsHeight)
                            {
                                GetNewOverflow4TopCustomLabels(customLabels, leftOverflow4CustomLabels, rightOverflow4CustomLabels, ref newLeftOverflow4CustomLabels, ref newRightOverflow4CustomLabels, customLabels.Visual.Height);
                            }
                            else
                            {
                                newLeftOverflow4CustomLabels = leftOverflow4CustomLabels;
                                newRightOverflow4CustomLabels = rightOverflow4CustomLabels;
                            }

                            RectangleGeometry clipRectangle = new RectangleGeometry();
                            clipRectangle.Rect = new Rect(-4 - newLeftOverflow4CustomLabels, -1, customLabels.Visual.Width + newLeftOverflow4CustomLabels + newRightOverflow4CustomLabels + 8, customLabels.Visual.Height + 2);
                            customLabels.Visual.Clip = clipRectangle;
                        }
                    }
                }
            }

            if (leftOverflow < leftOverflow4CustomLabels)
                leftOverflow = leftOverflow4CustomLabels;
            if (rightOverflow < rightOverflow4CustomLabels)
                rightOverflow = rightOverflow4CustomLabels;

            #endregion

            if (reversedCustomAxisLabels != null)
            {
                foreach (CustomAxisLabels customLabels in reversedCustomAxisLabels)
                {
                    if (customLabels.Visual != null)
                    {
                        if (Width == ScrollableSize)
                            AxisElementsContainer.Children.Add(customLabels.Visual);
                        else
                            InternalStackPanel.Children.Add(customLabels.Visual);
                    }
                }
            }

            if (AxisLabels.Visual != null)
            {
                if (Width == ScrollableSize)
                    AxisElementsContainer.Children.Add(AxisLabels.Visual);
                else
                    InternalStackPanel.Children.Add(AxisLabels.Visual);
            }

            List<Ticks> ticks = Ticks.Reverse().ToList();

            foreach (Ticks tick in ticks)
            {
                tick.SetParms(PlacementTypes.Top, ScrollableSize, Double.NaN);

                tick.CreateVisualObject();
                if (tick.Visual != null)
                {
                    if (Width == ScrollableSize)
                        AxisElementsContainer.Children.Add(tick.Visual);
                    else
                        InternalStackPanel.Children.Add(tick.Visual);
                }
            }

            if (Width != ScrollableSize)
            {
                ScrollViewerElement.Children.Add(InternalStackPanel);

                AxisElementsContainer.Children.Add(ScrollViewerElement);
            }

            AxisElementsContainer.Children.Add(AxisLine);

            #region Set Axis Height

            Double totalVisualHeight = 0;

            Size size = Graphics.CalculateVisualSize(AxisElementsContainer);

            if (InternalMinHeight != 0 && InternalMinHeight > size.Height)
                totalVisualHeight = InternalMinHeight;
            else
                totalVisualHeight = size.Height;

            if (!Double.IsPositiveInfinity(InternalMaxHeight) && InternalMaxHeight < totalVisualHeight)
            {
                totalVisualHeight = InternalMaxHeight;
            }
            //else
            //    AxisElementsContainer.Height = totalVisualHeight;

            Canvas axisContainer = new Canvas();
            axisContainer.Height = totalVisualHeight;
            axisContainer.Width = Width;

            axisContainer.Children.Add(AxisElementsContainer);
            AxisElementsContainer.SetValue(Canvas.TopProperty, (Double)totalVisualHeight - size.Height);

            if (!Double.IsPositiveInfinity(InternalMaxHeight) && InternalMaxHeight <= totalVisualHeight)
            {
                if (AxisLabels.Visual != null)
                    GetNewOverflow4TopLabels(leftOverflow, rightOverflow, ref newLeftOverflow, ref newRightOverflow, axisContainer.Height);

                if (reversedCustomAxisLabels != null)
                {
                    foreach (CustomAxisLabels customLabels in reversedCustomAxisLabels)
                    {
                        if (customLabels.Visual != null)
                        {
                            GetNewOverflow4TopCustomLabels(customLabels, leftOverflow4CustomLabels, rightOverflow4CustomLabels, ref newLeftOverflow4CustomLabels, ref newRightOverflow4CustomLabels, axisContainer.Height);
                        }
                    }
                }

                if (newLeftOverflow < newLeftOverflow4CustomLabels)
                    newLeftOverflow = newLeftOverflow4CustomLabels;
                if (newRightOverflow < newRightOverflow4CustomLabels)
                    newRightOverflow = newRightOverflow4CustomLabels;
            }
            else
            {
                newLeftOverflow = leftOverflow;
                newRightOverflow = rightOverflow;
            }

            RectangleGeometry clipVisual = new RectangleGeometry();
            clipVisual.Rect = new Rect(-4 - newLeftOverflow, 0, axisContainer.Width + newLeftOverflow + newRightOverflow + 8, size.Height);
            axisContainer.Clip = clipVisual;

            #endregion

            Visual.Children.Add(axisContainer);

            Size visualSize = Graphics.CalculateVisualSize(Visual);

            Double trendLineLabelsSize = GetSizeFromTrendLineLabels(this);

            if (visualSize.Height < trendLineLabelsSize)
                (Visual.Children[0] as Border).Height = trendLineLabelsSize - visualSize.Height;

            visualSize = Graphics.CalculateVisualSize(Visual);

            Visual.Height = visualSize.Height;

            //Visual.Children.Add(AxisElementsContainer);
        }

        private void GetNewOverflow4TopLabels(Double leftOverflow, Double rightOverflow, ref Double newLeftOverflow, ref Double newRightOverflow, Double visualHeight)
        {
            if ((Double)AxisLabels.InternalAngle > 0)
            {
                Double maxXPos = AxisLabels.AxisLabelList[0].Position.X;
                Point firstLabelPosition = new Point(AxisLabels.AxisLabelList[0].Position.X, AxisLabels.AxisLabelList[0].Position.Y);
                Point actualLeftPosition = new Point(AxisLabels.AxisLabelList[0].ActualLeft - AxisLabels.AxisLabelList[0].ActualWidth, AxisLabels.AxisLabelList[0].ActualTop);
                Point leftOffsetPosition = new Point(actualLeftPosition.X, firstLabelPosition.Y);

                Double height = Graphics.DistanceBetweenTwoPoints(leftOffsetPosition, actualLeftPosition);
                Double labelBase = Graphics.DistanceBetweenTwoPoints(leftOffsetPosition, firstLabelPosition);
                Double theta = Math.Atan(height / labelBase);

                if (height > visualHeight)
                    height = visualHeight;

                leftOverflow = (height / Math.Tan(theta));
                leftOverflow = leftOverflow - maxXPos;

                newLeftOverflow = leftOverflow;

                AxisLabels.LeftOverflow = newLeftOverflow;
            }
            else if ((Double)AxisLabels.InternalAngle < 0)
            {
                Double maxXPos = AxisLabels.AxisLabelList[AxisLabels.AxisLabelList.Count - 1].Position.X;
                Point lastLabelPosition = new Point(AxisLabels.AxisLabelList[AxisLabels.AxisLabelList.Count - 1].Position.X, AxisLabels.AxisLabelList[AxisLabels.AxisLabelList.Count - 1].Position.Y);
                Point actualRightPosition = new Point(AxisLabels.AxisLabelList[AxisLabels.AxisLabelList.Count - 1].ActualLeft + AxisLabels.AxisLabelList[AxisLabels.AxisLabelList.Count - 1].ActualWidth, AxisLabels.AxisLabelList[AxisLabels.AxisLabelList.Count - 1].ActualHeight);
                Point rightOffsetPosition = new Point(actualRightPosition.X, lastLabelPosition.Y);

                Double height = Graphics.DistanceBetweenTwoPoints(rightOffsetPosition, actualRightPosition);
                Double labelBase = Graphics.DistanceBetweenTwoPoints(rightOffsetPosition, lastLabelPosition);
                Double theta = Math.Atan(height / labelBase);

                if (height > visualHeight)
                    height = visualHeight;

                rightOverflow = (height / Math.Tan(theta));
                rightOverflow = rightOverflow + maxXPos;

                rightOverflow = rightOverflow - Width;

                newRightOverflow = rightOverflow;

                AxisLabels.RightOverflow = newRightOverflow;
            }
        }

        private void GetNewOverflow4TopCustomLabels(CustomAxisLabels customLabels, Double leftOverflow, Double rightOverflow, ref Double newLeftOverflow, ref Double newRightOverflow, Double visualHeight)
        {
            if ((Double)customLabels.InternalAngle > 0)
            {
                Double maxXPos = customLabels.AxisLabelList[0].Position.X;
                Point firstLabelPosition = new Point(customLabels.AxisLabelList[0].Position.X, customLabels.AxisLabelList[0].Position.Y);
                Point actualLeftPosition = new Point(customLabels.AxisLabelList[0].ActualLeft - customLabels.AxisLabelList[0].ActualWidth, customLabels.AxisLabelList[0].ActualTop);
                Point leftOffsetPosition = new Point(actualLeftPosition.X, firstLabelPosition.Y);

                Double height = Graphics.DistanceBetweenTwoPoints(leftOffsetPosition, actualLeftPosition);
                Double labelBase = Graphics.DistanceBetweenTwoPoints(leftOffsetPosition, firstLabelPosition);
                Double theta = Math.Atan(height / labelBase);

                if (height > visualHeight)
                    height = visualHeight;

                leftOverflow = (height / Math.Tan(theta));
                leftOverflow = leftOverflow - maxXPos;

                newLeftOverflow = leftOverflow;

                customLabels.LeftOverflow = newLeftOverflow;
            }
            else if ((Double)AxisLabels.InternalAngle < 0)
            {
                Double maxXPos = customLabels.AxisLabelList[customLabels.AxisLabelList.Count - 1].Position.X;
                Point lastLabelPosition = new Point(customLabels.AxisLabelList[customLabels.AxisLabelList.Count - 1].Position.X, customLabels.AxisLabelList[customLabels.AxisLabelList.Count - 1].Position.Y);
                Point actualRightPosition = new Point(customLabels.AxisLabelList[customLabels.AxisLabelList.Count - 1].ActualLeft + customLabels.AxisLabelList[customLabels.AxisLabelList.Count - 1].ActualWidth, customLabels.AxisLabelList[customLabels.AxisLabelList.Count - 1].ActualHeight);
                Point rightOffsetPosition = new Point(actualRightPosition.X, lastLabelPosition.Y);

                Double height = Graphics.DistanceBetweenTwoPoints(rightOffsetPosition, actualRightPosition);
                Double labelBase = Graphics.DistanceBetweenTwoPoints(rightOffsetPosition, lastLabelPosition);
                Double theta = Math.Atan(height / labelBase);

                if (height > visualHeight)
                    height = visualHeight;

                rightOverflow = (height / Math.Tan(theta));
                rightOverflow = rightOverflow + maxXPos;

                rightOverflow = rightOverflow - Width;

                newRightOverflow = rightOverflow;

                customLabels.RightOverflow = newRightOverflow;
            }
        }

        /// <summary>
        /// Create axis title visual
        /// </summary>
        /// <param name="margin">Margin between axis title and axis scale</param>
        private void CreateAxisTitleVisual(Thickness margin)
        {
            try
            {
                Double fontSize;

                if (AxisTitleElement != null)
                {
                    AxisTitleElement.InternalMargin = margin;

                    AxisTitleElement.IsNotificationEnable = false;

                RECAL:

                    AxisTitleElement.CreateVisualObject(new ElementData() { Element = this });
#if WPF
                AxisTitleElement.Visual.FlowDirection = FlowDirection.LeftToRight;
#endif
                    Size size = Graphics.CalculateVisualSize(AxisTitleElement.Visual);

                    if (AxisOrientation == AxisOrientation.Horizontal)
                    {
                        if (size.Width > Width && Width != 0)
                        {
                            if (AxisTitleElement.InternalFontSize == 0.2)
                                goto RETURN;

                            fontSize = AxisTitleElement.InternalFontSize;
                            fontSize -= 0.2;
                            AxisTitleElement.InternalFontSize = Math.Round(fontSize, 2);

                            goto RECAL;
                        }
                    }
                    else
                    {
                        if (size.Height > Height && Height != 0)
                        {   
                            if (AxisTitleElement.InternalFontSize == 0.2)
                                goto RETURN;
                                
                            fontSize = AxisTitleElement.InternalFontSize;
                            fontSize -= 0.2;
                            AxisTitleElement.InternalFontSize = Math.Round(fontSize, 2);
                            
                            goto RECAL;
                        }
                    }

                RETURN:

                    AxisTitleElement.IsNotificationEnable = true;
                }

            }
            catch
            {
                throw new ArgumentException("Internal Size Error");
            }
        }

        /// <summary>
        /// Set the axis limits considering the width of the columns that will be drawn in the chart
        /// </summary>
        private bool SetAxesXLimits()
        {
            //if (PlotDetails.DrawingDivisionFactor > 0)
            {
                if (!SetAxisLimitForMinimumGap())
                    return false;
            }

            MatchLeftAndRightGaps();
            return true;
        }

        /// <summary>
        /// Set the limits such that the gap between plot area and the Columns will be minimum
        /// </summary>
        private bool SetAxisLimitForMinimumGap()
        {
            Double minimumDifference = PlotDetails.GetMaxOfMinDifferencesForXValue();
            Double minValue = minimumDifference;

            if (Double.IsInfinity(minValue))
            {
                minValue = (AxisManager.AxisMaximumValue - AxisManager.AxisMinimumValue) * .8;
            }

            if (AxisMinimum != null && IsDateTimeAxis)
            {
                AxisMinimumNumeric = DateTimeHelper.DateDiff(AxisMinimumDateTime, MinDate, MinDateRange, MaxDateRange, InternalIntervalType, XValueType);
                AxisManager.AxisMinimumValue = AxisMinimumNumeric;
                AxisManager.Calculate();
            }

            if (AxisMaximum != null && IsDateTimeAxis)
            {
                AxisMaximumNumeric = DateTimeHelper.DateDiff(AxisMaximumDateTime, MinDate, MinDateRange, MaxDateRange, InternalIntervalType, XValueType);
                AxisManager.AxisMaximumValue = AxisMaximumNumeric;
                AxisManager.Calculate();
            }

            if (Double.IsNaN((Double)AxisMinimumNumeric) && !(Boolean)StartFromZero)
            {
                if (PlotDetails.DrawingDivisionFactor != 0)
                {
                    AxisManager.AxisMinimumValue = AxisManager.MinimumValue - (minValue / 2 * 1.1);
                }
                else
                {
                    AxisManager.AxisMinimumValue = AxisManager.MinimumValue - (minValue) / 2 * .4;
                }

                if (XValueType != ChartValueTypes.Numeric)
                {
                    DateTime startDate;
                    Double start;

                    startDate = DateTimeHelper.AlignDateTime(MinDate, 1, InternalIntervalType);

                    start = DateTimeHelper.DateDiff(startDate, MinDate, MinDateRange, MaxDateRange, InternalIntervalType, XValueType);

                    if (AxisManager.AxisMinimumValue > start)
                    {
                        AxisManager.AxisMinimumValue = start;
                    }
                    else
                    {
                        Double temp = Math.Floor((start - AxisManager.AxisMinimumValue) / InternalInterval);

                        if (!Double.IsInfinity(temp) && temp >= 1)
                            start = (start - Math.Floor(temp) * InternalInterval);
                    }

                    DateTime tempFirstLabelDate = DateTimeHelper.XValueToDateTime(MinDate, start, InternalIntervalType);
                    FirstLabelDate = DateTimeHelper.AlignDateTime(tempFirstLabelDate, InternalInterval < 1 ? InternalInterval : 1, InternalIntervalType);
                    FirstLabelPosition = DateTimeHelper.DateDiff(FirstLabelDate, MinDate, MinDateRange, MaxDateRange, InternalIntervalType, XValueType);

                    if (AxisManager.AxisMinimumValue > FirstLabelPosition)
                    {
                        FirstLabelDate = tempFirstLabelDate;
                        FirstLabelPosition = DateTimeHelper.DateDiff(FirstLabelDate, MinDate, MinDateRange, MaxDateRange, InternalIntervalType, XValueType);
                    }
                }
            }
            else
            {
                if (!Double.IsNaN(AxisMinimumNumeric))
                    AxisManager.AxisMinimumValue = (Double)AxisMinimumNumeric;

                FirstLabelPosition = AxisManager.AxisMinimumValue;

                if (XValueType != ChartValueTypes.Numeric)
                    FirstLabelDate = DateTimeHelper.XValueToDateTime(MinDate, AxisManager.AxisMinimumValue, InternalIntervalType);
            }


            if (Double.IsNaN((Double)AxisMaximumNumeric))
            {
                if (PlotDetails.DrawingDivisionFactor != 0 && Double.IsNaN((Double)AxisMaximumNumeric))
                {
                    AxisManager.AxisMaximumValue = AxisManager.MaximumValue + (minValue) / 2 * 1.1;
                }
                else
                {
                    AxisManager.AxisMaximumValue = AxisManager.MaximumValue + (minValue) / 2 * .4;
                }
            }
            else
            {
                AxisManager.AxisMaximumValue = (Double)AxisMaximumNumeric;
            }

            return true;
        }

        internal Double FirstLabelPosition
        {
            get;
            set;
        }

        internal DateTime FirstLabelDate
        {
            get;
            set;
        }

        /// <summary>
        /// Calculate and set axis manager maximum and minimum value in order to match equal gaps at the right and left side of the chart
        /// </summary>
        private void MatchLeftAndRightGaps()
        {
            Double minimumDifference = PlotDetails.GetMaxOfMinDifferencesForXValue();

            if (Double.IsNaN((Double)AxisMinimumNumeric))
            {
                if (Double.IsNaN((Double)AxisMaximumNumeric))
                {
                    if ((AxisManager.AxisMaximumValue - Maximum) <= (Minimum - AxisManager.AxisMinimumValue))
                    {
                        // This part makes the gaps equal
                        AxisManager.AxisMaximumValue = Maximum + Minimum - AxisManager.AxisMinimumValue;
                    }
                }
                else
                {
                    AxisManager.AxisMaximumValue = (Double)AxisMaximumNumeric;
                }
            }
        }

        /// <summary>
        /// Convert scaling sets from string to unit and value array
        /// </summary>
        /// <param name="scalingSets">ScalingSets as string</param>
        private void ParseScalingSets(String scalingSets)
        {
            if (String.IsNullOrEmpty(scalingSets)) return;

            // scaling sets are available in the form of value,unit;value,unit;value,unit;
            String[] pairs = scalingSets.Split(';');

            // Since scale has to be successively multiplied initialize it to 1
            Double scale = 1;

            // variable to store the parsed double value
            Double parsedValue;

            _scaleUnits = new List<String>();
            _scaleValues = new List<Double>();

            for (Int32 i = 0; i < pairs.Length; i++)
            {
                // split the individual pairs available in "value,unit" form
                String[] sets = pairs[i].Split(',');

                // if either of value or unit is missing then throw this exception
                if (sets.Length != 2)
                    throw new Exception("Invalid scaling set parameters. should be of the form value,unit;value,unit;...");

                // parse the value part of the string as double
                parsedValue = Double.Parse(sets[0], CultureInfo.InvariantCulture);

                // multiply the scale with the parsed value and store it
                scale *= parsedValue;
                _scaleValues.Add(scale);

                // store the unit in the units list
                _scaleUnits.Add(sets[1]);
            }
        }

        /// <summary>
        /// Calculate DateTime from numeric XValue
        /// </summary>
        internal Object CalculateDateTimeFromNumericXValue(Double xValue)
        {
            if (IsDateTimeAxis)
                return DateTimeHelper.XValueToDateTime(MinDate, xValue, InternalIntervalType);
            else
                return xValue;
        }

        /// <summary>
        /// Apply axis visual properties
        /// </summary>
        private void ApplyVisualProperty()
        {
            Visual.Cursor = (Cursor == null) ? Cursors.Arrow : Cursor;
            AttachHref(Chart, Visual, Href, HrefTarget);
            AttachToolTip(Chart, this, Visual);
            AttachEvents2Visual(this, this.Visual);
            Visual.Opacity = this.InternalOpacity;
        }

        /// <summary>
        /// Calculate and return default interval for axis
        /// </summary>
        /// <returns>Interval as Double</returns>
        private Double GenerateDefaultInterval()
        {
            if (_isDateTimeAutoInterval ||
                (XValueType != ChartValueTypes.Numeric && IntervalType != IntervalTypes.Years && Double.IsNaN((Double)Interval) && Double.IsNaN((Double)AxisLabels.Interval) && !Double.IsNaN(InternalInterval) && InternalInterval >= 1))
                return InternalInterval;

            if (AxisType == AxisTypes.Primary)
            {
                if (Double.IsNaN((Double)Interval) && Double.IsNaN((Double)AxisLabels.Interval) && PlotDetails.IsAllPrimaryAxisXLabelsPresent)
                {
                    List<Double> uniqueXValues = (from entry in PlotDetails.AxisXPrimaryLabels orderby entry.Key select entry.Key).ToList();

                    if (uniqueXValues.Count > 0)
                    {
                        Double minDiff = Double.MaxValue;

                        for (Int32 i = 0; i < uniqueXValues.Count - 1; i++)
                        {
                            minDiff = Math.Min(minDiff, Math.Abs(uniqueXValues[i] - uniqueXValues[i + 1]));
                        }

                        if (minDiff != Double.MaxValue)
                            return minDiff;
                    }
                }
            }
            else
            {
                if (Double.IsNaN((Double)Interval) && Double.IsNaN((Double)AxisLabels.Interval) && PlotDetails.IsAllSecondaryAxisXLabelsPresent)
                {
                    List<Double> uniqueXValues = (from entry in PlotDetails.AxisXSecondaryLabels orderby entry.Key select entry.Key).ToList();

                    if (uniqueXValues.Count > 0)
                    {
                        Double minDiff = Double.MaxValue;

                        for (Int32 i = 0; i < uniqueXValues.Count - 1; i++)
                        {
                            minDiff = Math.Min(minDiff, Math.Abs(uniqueXValues[i] - uniqueXValues[i + 1]));
                        }

                        if (minDiff != Double.MaxValue)
                            return minDiff;
                    }
                }
            }

            return (Double)Interval;
        }

        private void CustomLabels_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                if (e.NewItems != null)
                {
                    foreach (CustomAxisLabels customLabels in e.NewItems)
                    {
                        if (Chart != null)
                            customLabels.Chart = Chart;

                        customLabels.Parent = this;

                        customLabels.PropertyChanged -= customLabels_PropertyChanged;
                        customLabels.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(customLabels_PropertyChanged);
                    }
                }
            }

            this.FirePropertyChanged(VcProperties.CustomAxisLabels);
        }

        /// <summary>
        /// Event handler manages the addition and removal of ticks from axis
        /// </summary>
        private void Ticks_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Boolean isAutoTick = false;

            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                if (e.NewItems != null)
                {
                    foreach (Ticks tick in e.NewItems)
                    {
                        if (tick._isAutoGenerated)
                            isAutoTick = true;

                        if (Chart != null)
                            tick.Chart = Chart;

                        tick.PropertyChanged -= tick_PropertyChanged;
                        tick.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(tick_PropertyChanged);
                    }
                }
            }

            if (!isAutoTick)
                this.FirePropertyChanged(VcProperties.Ticks);
        }

        /// <summary>
        /// Event handler manages the addition and removal of chartgrid from axis
        /// </summary>
        private void Grids_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Boolean isAutoGrids = false;

            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                if (e.NewItems != null)
                {
                    foreach (ChartGrid grid in e.NewItems)
                    {
                        if (grid._isAutoGenerated)
                            isAutoGrids = true;

                        if (Chart != null)
                            grid.Chart = Chart;

                        grid.PropertyChanged -= grid_PropertyChanged;
                        grid.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(grid_PropertyChanged);
                    }
                }
            }

            if (!isAutoGrids)
                this.FirePropertyChanged(VcProperties.Grids);
        }

        /// <summary>
        /// Event handler attached with PropertyChanged event of chartgrids
        /// </summary>
        /// <param name="sender">ObservableObject</param>
        /// <param name="e">PropertyChangedEventArgs</param>
        private void grid_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            this.FirePropertyChanged((VcProperties)Enum.Parse(typeof(VcProperties), e.PropertyName, true));
        }

        /// <summary>
        ///  Event handler attached with PropertyChanged event of ticks
        /// </summary>
        /// <param name="sender">ObservableObject</param>
        /// <param name="e">PropertyChangedEventArgs</param>
        private void tick_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            this.FirePropertyChanged((VcProperties)Enum.Parse(typeof(VcProperties), e.PropertyName, true));
        }

        /// <summary>
        ///  Event handler attached with PropertyChanged event of ticks
        /// </summary>
        /// <param name="sender">ObservableObject</param>
        /// <param name="e">PropertyChangedEventArgs</param>
        private void customLabels_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            this.FirePropertyChanged((VcProperties)Enum.Parse(typeof(VcProperties), e.PropertyName, true));
        }

        /// <summary>
        /// Applies axis settings for horizontal type axis
        /// </summary>
        private void ApplyHorizontalAxisSettings()
        {
            // Apply  settings based on the axis type
            switch (AxisType)
            {
                case AxisTypes.Primary:
                    ApplyHorizontalPrimaryAxisSettings();
                    break;
                case AxisTypes.Secondary:
                    ApplyHorizontalSecondaryAxisSettings();
                    break;
            }
        }

        /// <summary>
        /// Set up ticks for axis
        /// </summary>
        private void SetUpTicks()
        {
            if (Ticks.Count == 0)
                Ticks.Add(new Ticks() { _isAutoGenerated = true });

            foreach (Ticks tick in Ticks)
            {
                tick.IsNotificationEnable = false;

                if (AxisRepresentation.ToString() == "AxisX")
                    tick.ApplyStyleFromTheme(Chart, "AxisXTicks");
                else if (AxisRepresentation.ToString() == "AxisY")
                    tick.ApplyStyleFromTheme(Chart, "AxisYTicks");

                tick.Maximum = AxisManager.AxisMaximumValue;
                tick.Minimum = AxisManager.AxisMinimumValue;
                tick.DataMaximum = Maximum;
                tick.DataMinimum = Minimum;
                tick.ParentAxis = this;

                if (PlotDetails.ChartOrientation == ChartOrientationType.Circular)
                {
                    if(tick.GetValue(Visifire.Charts.Ticks.TickLengthProperty) == null)
                        tick.TickLength = 2.3;
                }

                tick.IsNotificationEnable = true;
            }
        }

        /// <summary>
        /// Set up grids for axis
        /// </summary>
        private void SetUpGrids()
        {
            if (Grids.Count == 0 && AxisRepresentation.ToString() != "AxisX")
                Grids.Add(new ChartGrid() { _isAutoGenerated = true });

            foreach (ChartGrid grid in Grids)
            {
                grid.IsNotificationEnable = false;

                grid.Maximum = AxisManager.AxisMaximumValue;
                grid.Minimum = AxisManager.AxisMinimumValue;
                grid.DataMaximum = Maximum;
                grid.DataMinimum = Minimum;
                grid.ParentAxis = this;

                if (PlotDetails.ChartOrientation == ChartOrientationType.Circular && grid.GetValue(ChartGrid.LineThicknessProperty) == null)
                    grid.LineThickness = 0.50;

                grid.IsNotificationEnable = true;
            }
        }

        /// <summary>
        /// Set up axis labels for axis
        /// </summary>
        private void SetUpCustomAxisLabels()
        {
            foreach (CustomAxisLabels labels in CustomAxisLabels)
            {
                // set the params to create custom AxisLabels
                labels.Maximum = AxisManager.AxisMaximumValue;
                labels.Minimum = AxisManager.AxisMinimumValue;
                labels.ParentAxis = this;
            }
        }

        /// <summary>
        /// Set up axis labels for axis
        /// </summary>
        private void SetUpAxisLabels()
        {
            // set the params to create Axis Labels
            AxisLabels.Maximum = AxisManager.AxisMaximumValue;
            AxisLabels.Minimum = AxisManager.AxisMinimumValue;
            AxisLabels.DataMaximum = Maximum;
            AxisLabels.DataMinimum = Minimum;
            AxisLabels.ParentAxis = this;
            AxisLabels.InternalRows = (Int32)AxisLabels.Rows;

            //AxisLabels.Padding = this.Padding;

            if (AxisRepresentation == AxisRepresentations.AxisX)
            {
                if (AxisType == AxisTypes.Primary)
                {
                    AxisLabels.AxisLabelContentDictionary = PlotDetails.AxisXPrimaryLabels;
                    AxisLabels.AllAxisLabels = PlotDetails.IsAllPrimaryAxisXLabelsPresent;
                }
                else
                {
                    AxisLabels.AxisLabelContentDictionary = PlotDetails.AxisXSecondaryLabels;
                    AxisLabels.AllAxisLabels = PlotDetails.IsAllSecondaryAxisXLabelsPresent;
                }
            }

            foreach (Ticks tick in Ticks)
            {
                tick.IsNotificationEnable = false;

                tick.AllAxisLabels = AxisLabels.AllAxisLabels;
                tick.AxisLabelsDictionary = AxisLabels.AxisLabelContentDictionary;

                tick.IsNotificationEnable = true;
            }
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Calculate total tick length from an axis
        /// </summary>
        /// <param name="chart"></param>
        /// <param name="tickLengthOfAxisX"></param>
        /// <param name="tickLengthOfPrimaryAxisY"></param>
        /// <param name="tickLengthOfSecondaryAxisY"></param>
        internal static void CalculateTotalTickLength(Chart chart, ref Double tickLengthOfAxisX, ref Double tickLengthOfPrimaryAxisY, ref Double tickLengthOfSecondaryAxisY)
        {
            tickLengthOfAxisX = (from tick in chart.AxesX[0].Ticks
                                 where (Boolean)chart.AxesX[0].Enabled && (Boolean)tick.Enabled
                                 select (Double)tick.TickLength).Sum();

            if (tickLengthOfAxisX == 0)
                tickLengthOfAxisX = 5;

            tickLengthOfPrimaryAxisY = (from axis in chart.AxesY
                                        where axis.AxisType == AxisTypes.Primary
                                        from tick in axis.Ticks
                                        where (Boolean)axis.Enabled && (Boolean)tick.Enabled
                                        select (Double)tick.TickLength).Sum();

            if (tickLengthOfPrimaryAxisY == 0)
                tickLengthOfPrimaryAxisY = 8;

            tickLengthOfSecondaryAxisY = (from axis in chart.AxesY
                                          where axis.AxisType == AxisTypes.Secondary
                                          from tick in axis.Ticks
                                          where (Boolean)axis.Enabled && (Boolean)tick.Enabled
                                          select (Double)tick.TickLength).Sum();

            if (tickLengthOfSecondaryAxisY == 0)
                tickLengthOfSecondaryAxisY = 8;
        }

        /// <summary>
        /// Converts pixel position to value
        /// </summary>
        /// <param name="maxWidth">Pixel width of the scale</param>
        /// <param name="position">Pixel position</param>
        /// <returns>Double</returns>
        internal Double PixelPositionToXValue(Double maxWidth, Double pixelPosition)
        {
            return Graphics.PixelPositionToValue(0, maxWidth, InternalAxisMinimum, InternalAxisMaximum, pixelPosition);
        }

        /// <summary>
        /// Converts pixel position to value
        /// </summary>
        /// <param name="maxWidth">Pixel width of the scale</param>
        /// <param name="position">Pixel position</param>
        /// <returns>Double</returns>
        internal Double PixelPositionToYValue(Double maxHeight, Double pixelPosition)
        {
            return Graphics.PixelPositionToValue(maxHeight, 0, InternalAxisMinimum, InternalAxisMaximum, pixelPosition);
        }

        /// <summary>
        /// Add prefix and suffix
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        internal string AddPrefixAndSuffix(String str)
        {
            return Prefix + str + Suffix;
        }

        /// <summary>
        /// Return formatted string from a given value depending upon scaling set and value format string
        /// </summary>
        /// <param name="value">Double value</param>
        /// <returns>String</returns>
        internal String GetFormattedString(Double value)
        {
            String str = value.ToString();
            if (ScaleValues != null && ScaleUnits != null)
            {
                String sUnit = ScaleUnits[0];
                Double sValue = ScaleValues[0];
                for (Int32 i = 0; i < ScaleValues.Count; i++)
                {
                    if ((Math.Abs(value) / ScaleValues[i]) < 1)
                    {
                        break;
                    }
                    sValue = ScaleValues[i];
                    sUnit = ScaleUnits[i];
                }

                if (IsDateTimeAxis)
                    str = (value / sValue).ToString() + sUnit;
                else
                    str = (value / sValue).ToString(ValueFormatString) + sUnit;
            }
            else
            {
                if (IsDateTimeAxis)
                    str = value.ToString();
                else
                    str = value.ToString(ValueFormatString);
            }

            str = AddPrefixAndSuffix(str);

            return str;
        }

        /// <summary>
        /// Associate a scrollbar with itself
        /// </summary>
        internal void SetScrollBar()
        {
            if (this.AxisOrientation == AxisOrientation.Vertical)
                ScrollBarElement = (AxisType == AxisTypes.Primary) ? Chart._leftAxisScrollBar : Chart._rightAxisScrollBar;
            else
                ScrollBarElement = (AxisType == AxisTypes.Primary) ? Chart._bottomAxisScrollBar : Chart._topAxisScrollBar;
        }

        /// <summary>
        /// Set axis scroll value to scrollbar associated with this axis
        /// </summary>
        /// <param name="offset">Scrollbar offset</param>
        internal void SetScrollBarValueFromOffset(Double offset, Boolean isScrollBarOffsetPropertyChanged)
        {
            if (ScrollBarElement != null && !ScrollBarElement.IsStretching) // &&  !(Chart as Chart).ZoomingEnabled)//
            {
                Double value = GetScrollBarValueFromOffset(offset);
                ScrollBarElement.SetValue(ScrollBar.ValueProperty, value);

                if (ScrollBarOffsetChanged != null)
                    ScrollBarOffsetChanged(ScrollBarElement, new ScrollEventArgs(isScrollBarOffsetPropertyChanged ? ScrollEventType.ThumbTrack : ScrollEventType.First, value));
            }
        }

        /// <summary>
        /// Get axis scroll value in pixel from scrollbar offset
        /// </summary>
        /// <param name="offset">Scrollbar offset</param>
        internal Double GetScrollBarValueFromOffset(Double offset)
        {
            if (Double.IsNaN(offset))
            {
               offset = 0;
            }

            offset = (ScrollBarElement.Maximum - ScrollBarElement.Minimum) * offset + ScrollBarElement.Minimum;

            if (PlotDetails != null)
            {
                if (PlotDetails.ChartOrientation == ChartOrientationType.Horizontal)
                {
                    offset = ScrollBarElement.Maximum - offset;
                }
            }

            return offset;
        }

        internal void CalculateViewMinimumAndMaximum(Chart chart, Double offsetInPixel, Double plotAreaWidth, Double plotAreaHeight)
        {
            Double offsetInPixel4MinValue;
            Double offsetInPixel4MaxValue;

            AxisOrientation axisOrientation = chart.ChartArea.AxisX.AxisOrientation;
            Double lengthInPixel;

            if (axisOrientation == AxisOrientation.Horizontal)
            {
                offsetInPixel = chart.ChartArea.GetScrollingOffsetOfAxis(chart.ChartArea.AxisX, offsetInPixel);

                lengthInPixel = plotAreaWidth;// chart.ChartArea.ChartVisualCanvas.Width;
                offsetInPixel4MinValue = offsetInPixel;
                offsetInPixel4MaxValue = offsetInPixel4MinValue + Width;
            }
            else
            {
                offsetInPixel = chart.ChartArea.GetScrollingOffsetOfAxis(chart.ChartArea.AxisX, offsetInPixel);

                lengthInPixel = plotAreaHeight;// chart.ChartArea.ChartVisualCanvas.Height;
                offsetInPixel4MaxValue = lengthInPixel - offsetInPixel;
                offsetInPixel4MinValue = offsetInPixel4MaxValue - Height;
            }

            // Calculate View Minimum
            Double xValue = PixelPositionToXValue(lengthInPixel, offsetInPixel4MinValue);
            _numericViewMinimum = xValue;

            if (chart.ChartArea.AxisX.IsDateTimeAxis)
            {   
                if (chart.PlotDetails.ListOfAllDataPoints.Count != 0)
                {   
                    ViewMinimum = DateTimeHelper.XValueToDateTime(chart.ChartArea.AxisX.MinDate,
                        xValue, chart.ChartArea.AxisX.InternalIntervalType);
                }
                else
                    ViewMinimum = chart.ChartArea.AxisX.MinDate;
            }   
            else
                ViewMinimum = xValue;

            // Calculate View Maximum
            xValue = PixelPositionToXValue(lengthInPixel, offsetInPixel4MaxValue);
            _numericViewMaximum = xValue;

            if (chart.ChartArea.AxisX.IsDateTimeAxis)
                ViewMaximum = DateTimeHelper.XValueToDateTime(chart.ChartArea.AxisX.MinDate,
                    xValue, chart.ChartArea.AxisX.InternalIntervalType);
            else
                ViewMaximum = xValue;
        }

        internal void FireScrollEvent(ScrollEventArgs e, Double offsetInPixel)
        {   
            if (Chart != null && (Chart as Chart).Series.Count > 0)
            {   
                if (PlotDetails.ListOfAllDataPoints.Count != 0)
                {
                    if (_oldScrollBarOffsetInPixel == offsetInPixel && !(Chart as Chart).ChartArea._isDragging)
                    {
                        if ((Chart as Chart)._clearAndResetZoomState)
                            ResetZoomState(Chart as Chart, true);

                        if (_initialState.MinXValue == null && _initialState.MaxXValue == null)
                        {
                            _initialState.MinXValue = ViewMinimum;
                            _initialState.MaxXValue = ViewMaximum;
                        }

                        return;
                    }

                    _oldScrollBarOffsetInPixel = offsetInPixel;

                    if (AxisRepresentation == AxisRepresentations.AxisX)
                    {   
                        Chart chart = Chart as Chart;

                        CalculateViewMinimumAndMaximum(chart, offsetInPixel, chart.ChartArea.ChartVisualCanvas.Width, chart.ChartArea.ChartVisualCanvas.Height);

                        if (chart.ZoomingEnabled)
                        {
                            if (_zoomStateStack.Count == 0)
                            {
                                _zoomStateStack.Push(new ZoomState(ViewMinimum, ViewMaximum));
                                _oldZoomState.MinXValue = null;
                                _oldZoomState.MaxXValue = null;
                            }

                            if (_showAllState)
                            {
                                _zoomStateStack.Clear();
                                _zoomStateStack.Push(new ZoomState(ViewMinimum, ViewMaximum));
                                _oldZoomState.MinXValue = null;
                                _oldZoomState.MaxXValue = null;
                                _showAllState = false;
                            }

                            if (chart.ChartArea._isFirstTimeRender || _isScrollEventFiredFirstTime
                                || (_initialState.MinXValue == null && _initialState.MaxXValue == null))
                            {
                                _initialState.MinXValue = ViewMinimum;
                                _initialState.MaxXValue = ViewMaximum;
                            }
                        }

                        if (_onScroll != null && !chart.ChartArea._isFirstTimeRender)
                        {   
                            _onScroll(this, new AxisScrollEventArgs(e));
                        }

                        _isScrollEventFiredFirstTime = false;
                    }
                }
            }
        }

        /// <summary>
        /// Creates the visual element for the Axis
        /// </summary>
        /// <param name="Chart">Chart</param>
        internal void CreateVisualObject(Chart Chart)
        {   
            if(AxisLabels == null)
                AxisLabels = new AxisLabels() {  IsDefault = true };

            IsNotificationEnable = false;
            AxisLabels.IsNotificationEnable = false;
            AxisLabels.Chart = Chart;

            if (!Double.IsNaN(ClosestPlotDistance) && !Chart.ChartArea.IsAutoCalculatedScrollBarScale && Chart.IsScrollingActivated)
                throw new ArgumentException(" ScrollBarScale property and ClosestPlotDistance property in Axis cannot be set together.");

            if (AxisRepresentation == AxisRepresentations.AxisX)
                AxisLabels.ApplyStyleFromTheme(Chart, "AxisXLabels");
            else if (AxisRepresentation == AxisRepresentations.AxisY)
                AxisLabels.ApplyStyleFromTheme(Chart, "AxisYLabels");

            foreach (CustomAxisLabels customLabels in CustomAxisLabels)
            {
                customLabels.Chart = Chart;

                if (AxisRepresentation == AxisRepresentations.AxisX)
                    customLabels.ApplyStyleFromTheme(Chart, "CustomAxisXLabels");
                else if (AxisRepresentation == AxisRepresentations.AxisY)
                    customLabels.ApplyStyleFromTheme(Chart, "CustomAxisYLabels");
            }

            if (Chart.PlotDetails.ChartOrientation != ChartOrientationType.Circular)
            {   
                // Create visual elements
                Visual = new StackPanel() { Background = InternalBackground };

                AxisElementsContainer = new StackPanel();
                InternalStackPanel = new Canvas();
                ScrollViewerElement = new Canvas();

                if (!String.IsNullOrEmpty(this.Title))
                {
                    AxisTitleElement = new Title();
                    ApplyTitleProperties();
                }
                else
                    AxisTitleElement = null;

                ApplyVisualProperty();

                SetUpAxisManager();
                SetUpTicks();
                SetUpGrids();
                SetUpAxisLabels();
                SetUpCustomAxisLabels();

                // set the placement order based on the axis orientation
                switch (AxisOrientation)
                {
                    case AxisOrientation.Horizontal:
                        ApplyHorizontalAxisSettings();
                        _zeroBaseLinePixPosition = Graphics.ValueToPixelPosition(0, Width, InternalAxisMinimum, InternalAxisMaximum, 0);

                        break;

                    case AxisOrientation.Vertical:
                        ApplyVerticalAxisSettings();
                        _zeroBaseLinePixPosition = Height - Graphics.ValueToPixelPosition(0, Height, InternalAxisMinimum, InternalAxisMaximum, 0);

                        break;
                }

                IsNotificationEnable = true;
                AxisLabels.IsNotificationEnable = true;

                if (!(Boolean)this.Enabled)
                {
                    Visual.Visibility = Visibility.Collapsed;
                }

                AttachEventForZoomBar();
            }
            else
            {
                CreateAxesForCircularChart();   
            }
        }

        private void AttachEventForZoomBar()
        {
            ScrollBarElement.ScaleChanged -= OnZoomingScaleChanged;
            ScrollBarElement.DragCompleted -= ScrollBarElement_DragCompleted;

            Chart chart = Chart as Chart;

            Boolean isAllLineCharts = (((from ds in chart.Series
                                         where ( RenderHelper.IsLineCType(ds) || ds.RenderAs == RenderAs.Area) 
                                        select ds).Count() == chart.Series.Count)
                                        && chart.PlotDetails.ListOfAllDataPoints.Count <= 500);

            if (isAllLineCharts || chart.PlotDetails.ListOfAllDataPoints.Count < 500)
            {
                ScrollBarElement.ScaleChanged += new EventHandler(OnZoomingScaleChanged);

                ScrollBarElement.DragCompleted += new EventHandler(ScrollBarElement_DragCompleted);

                _renderAfterDrag = false;
            }
            else
            {
                ScrollBarElement.DragStarted += new EventHandler(ScrollBarElement_DragStarted);
                ScrollBarElement.DragCompleted += new EventHandler(ScrollBarElement_DragCompleted);
                _renderAfterDrag = true;
            }
        }

        Boolean _dragCompletedLock = false;
        Boolean _renderAfterDrag = false;

        void ScrollBarElement_DragCompleted(object sender, EventArgs e)
        {
            if (!_renderAfterDrag)
                SetZoomState();
            else
            {
                OnZoomingScaleChanged(sender, e);
                (Chart as Chart).Dispatcher.BeginInvoke(new Action(SetZoomState));
            }
        }

        private void SetZoomState()
        {
            Chart chart = (Chart as Chart);

            if (chart == null)
                return;

            if (!_dragCompletedLock && ScrollBarElement.Scale < 1)
            {
                _dragCompletedLock = true;

                if (_oldZoomState.MinXValue != null && _oldZoomState.MaxXValue != null)
                    _zoomStateStack.Push(new ZoomState(_oldZoomState.MinXValue, _oldZoomState.MaxXValue));

                _oldZoomState.MinXValue = ViewMinimum;
                _oldZoomState.MaxXValue = ViewMaximum;

                chart._zoomOutTextBlock.Visibility = Visibility.Visible;
                chart._zoomIconSeparater.Visibility = Visibility.Visible;
                chart._showAllTextBlock.Visibility = Visibility.Visible;
            }

            if(ScrollBarElement.Scale == 1)
                ResetZoomState(chart, true);
        }

        internal void ResetZoomState(Chart chart, Boolean initializeStack)
        {
            chart._showAllTextBlock.Visibility = Visibility.Collapsed;
            chart._zoomIconSeparater.Visibility = Visibility.Collapsed;
            chart._zoomOutTextBlock.Visibility = Visibility.Collapsed;

            _zoomStateStack.Clear();

            if (initializeStack)
            {
                if (ViewMinimum != null && ViewMaximum != null)
                    _zoomStateStack.Push(new ZoomState(ViewMinimum, ViewMaximum));
            }
            else
            {
                _initialState.MinXValue = null;
                _initialState.MaxXValue = null;
            }

            _oldZoomState.MinXValue = null;
            _oldZoomState.MaxXValue = null;
        }

        void ScrollBarElement_DragStarted(object sender, EventArgs e)
        {   
            ZoomBar zoomBar = sender as ZoomBar;
            zoomBar.ScrollEventFireEnabled = false;
        }
        
        void OnZoomingScaleChanged(object sender, EventArgs e)
        {
            ZoomBar zoomBar = sender as ZoomBar;
            zoomBar.ScrollEventFireEnabled = true;

            if (ZoomingScaleChanged != null)
                ZoomingScaleChanged(this, e);

            _dragCompletedLock = false;
        }

        internal void FireZoomEvent(ZoomState zoomState, EventArgs e)
        {
            if (_onZoom != null)
                _onZoom(this, CreateAxisZoomEventArgs(zoomState, e));
        }

        internal AxisZoomEventArgs CreateAxisZoomEventArgs(ZoomState zoomState, EventArgs e)
        {
            AxisZoomEventArgs eventArgs = new AxisZoomEventArgs(e);

            eventArgs.MinValue = zoomState.MinXValue;
            eventArgs.MaxValue = zoomState.MaxXValue;

            return eventArgs;
        }

        internal static void SaveOldValueOfAxisRange(Axis axis)
        {
            if (axis != null)
            {
                axis._oldInternalAxisMaximum = axis.InternalAxisMaximum;
                axis._oldInternalAxisMinimum = axis.InternalAxisMinimum;
            }
        }

        internal override void ClearInstanceRefs()
        {
            foreach (ChartGrid grid in Grids)
            {
                if (grid.Storyboard != null)
                {
                    grid.Storyboard.FillBehavior = System.Windows.Media.Animation.FillBehavior.Stop;
                    grid.Storyboard.Stop();
                    grid.Storyboard = null;
                }
            }

            if (Storyboard != null)
            {
                Storyboard.FillBehavior = System.Windows.Media.Animation.FillBehavior.Stop;
                Storyboard.Stop();
                Storyboard = null;
            }
        }

        #endregion

        #region Internal Events And Delegates

        internal event EventHandler ZoomingScaleChanged;

        /// <summary>
        /// Scroll event of axis
        /// </summary>
        internal event ScrollEventHandler ScrollBarOffsetChanged;

        /// <summary>
        /// Event handler for the Scroll event
        /// </summary>
#if SL &&!WP
        [ScriptableMember]
#endif
        public event EventHandler<AxisScrollEventArgs> Scroll
        {
            remove
            {
                _onScroll -= value;
            }
            add
            {
                _onScroll += value;
            }
        }

        /// <summary>
        /// Event handler for the OnZoom event
        /// </summary>
#if SL &&!WP
        [ScriptableMember]
#endif
        public event EventHandler<AxisZoomEventArgs> OnZoom
        {
            remove
            {
                _onZoom -= value;
            }
            add
            {
                _onZoom += value;
            }
        }
        
        #endregion

        #region Data

        internal const string RootElementName = "RootElement";
        internal Canvas _rootElement;

        internal ZoomState _oldZoomState = new ZoomState(null, null);
        internal ZoomState _zoomState = new ZoomState(null, null);
        internal ZoomState _initialState = new ZoomState(null, null);

        // Pixel position of zero line of a axis
        internal Double _zeroBaseLinePixPosition;

        /// <summary>
        /// Value type of the AxisMinimum Property
        /// </summary>
        internal ChartValueTypes _axisMinimumValueType;

        /// <summary>
        /// Value type of the AxisMinimum Property
        /// </summary>
        internal ChartValueTypes _axisMaximumValueType;

        /// <summary>
        /// Whether ScrollBar scrolling is enabled due to change of ScrollBarOffset property
        /// </summary>
        internal Boolean _isScrollToOffsetEnabled = true;

        /// <summary>
        /// Whether Interval is auto-calculated for DateTime Axis
        /// </summary>
        internal Boolean _isDateTimeAutoInterval = false;

        /// <summary>
        /// Whether all XValues are equals to zero
        /// </summary>
        internal Boolean _isAllXValueZero = true;

        /// <summary>
        /// Stores the  orientation (vertical or horizontal) of the axis 
        /// </summary>
        private AxisOrientation _orientation;

        /// <summary>
        /// Stores the units extracted from the scaling sets
        /// </summary>
        private List<String> _scaleUnits;

        /// <summary>
        /// Stores the values extracted from the scaling sets
        /// </summary>
        private List<Double> _scaleValues;

        /// <summary>
        /// Whether user has set the axis interval or not 
        /// </summary>
        private Double _axisIntervalOverride;

        /// <summary>
        /// Old ScrollBar offset value in pixel
        /// </summary>
        internal Double _oldScrollBarOffsetInPixel = Double.NaN;

        /// <summary>
        /// Handler for Scroll event
        /// </summary>
        private event EventHandler<AxisScrollEventArgs> _onScroll;

        /// <summary>
        /// Handler for OnZoom event
        /// </summary>
        internal event EventHandler<AxisZoomEventArgs> _onZoom;

        internal Stack<ZoomState> _zoomStateStack = new Stack<ZoomState>();
        
        /// <summary>
        /// Numeric XValue corresponding to View Minimum (Applicable for Numeric axis as well as DateTime axis)
        /// </summary>
        internal Double _numericViewMinimum;

        /// <summary>
        /// Numeric XValue corresponding to View Maximum (Applicable for Numeric axis as well as DateTime axis)
        /// </summary>
        internal Double _numericViewMaximum;

        /// <summary>
        /// Check whether Scroll event fired first time
        /// </summary>
        private Boolean _isScrollEventFiredFirstTime = true;

        /// <summary>
        /// Margin between axis title and axis scale
        /// </summary>
        private const Double INNER_MARGIN = 4;

        Double _internalOpacity = Double.NaN;
        Brush _internalBackground = null;
        Nullable<Thickness> _internalPadding = null;

        Double _internalMaxHeight = Double.NaN;
        Double _internalMaxWidth = Double.NaN;
        Double _internalMinHeight = Double.NaN;
        Double _internalMinWidth = Double.NaN;

        /// <summary>
        /// CurrentZooming Scale
        /// </summary>
        internal Double _internalZoomingScale;

        /// <summary>
        /// Old value of ZoomBar. Its used only while zooming to remember the old state of ZoomBar.
        /// </summary>
        internal Double _internalOldZoomBarValue;

        /// <summary>
        /// Old Zooming Scale. Its used only while zooming to remember the old state of ZoomBar.
        /// </summary>
        internal Double _internalOldZoomingScale = Double.NaN;

        /// <summary>
        /// MinimumZooming Scale
        /// </summary>
        internal static readonly Double INTERNAL_MINIMUM_ZOOMING_SCALE = 0.0000001;

        Boolean _showAllState = false;

#if WPF
        /// <summary>
        /// Whether the default style is applied
        /// </summary>
        private static Boolean _defaultStyleKeyApplied;
#endif

        internal class ZoomState
        {
            public ZoomState(Object minValue, Object maxValue)
            {
                MinXValue = minValue;
                MaxXValue = maxValue;
            }

            public Object MinXValue;

            public Object MaxXValue;
        }

        #endregion
    }
}
