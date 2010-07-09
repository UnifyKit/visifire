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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

#else

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Browser;
#endif

using Visifire.Commons;
using System.ComponentModel;
using System.Windows.Data;

namespace Visifire.Charts
{
    /// <summary>
    /// Visifire.Charts.PlotArea class
    /// </summary>
#if SL
    [System.Windows.Browser.ScriptableType]
#endif
    public class PlotArea : ObservableObject
    {
        #region Public Methods

        /// <summary>
        /// Initializes a new instance of the Visifire.Charts.PlotArea class
        /// </summary>
        public PlotArea()
        {

            // Apply default style from generic
#if WPF

            if (!_defaultStyleKeyApplied)
            {
                DefaultStyleKeyProperty.OverrideMetadata(typeof(PlotArea), new FrameworkPropertyMetadata(typeof(PlotArea)));
                _defaultStyleKeyApplied = true;
            }            
#else
            DefaultStyleKey = typeof(PlotArea);
#endif

            // Attach event handler with EventChanged event of VisifireElement
            EventChanged += delegate
            {
                FirePropertyChanged(VcProperties.MouseEvent);
            };
        }

        public override void Bind()
        {
#if SL
            Binding b = new Binding("Background");
            b.Source = this;
            this.SetBinding(InternalBackgroundProperty, b);

            b = new Binding("BorderThickness");
            b.Source = this;
            this.SetBinding(InternalBorderThicknessProperty, b);

            b = new Binding("Opacity");
            b.Source = this;
            this.SetBinding(InternalOpacityProperty, b);
#endif
        }

        #endregion

        #region Public Properties

#if SL

        /// <summary>
        /// Identifies the Visifire.Charts.PlotArea.Background dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.PlotArea.Background dependency property.
        /// </returns>
        private static readonly DependencyProperty InternalBackgroundProperty = DependencyProperty.Register
            ("InternalBackground",
            typeof(Brush),
            typeof(PlotArea),
            new PropertyMetadata(OnBackgroundPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.PlotArea.Opacity dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.PlotArea.Opacity dependency property.
        /// </returns>
        private static readonly DependencyProperty InternalOpacityProperty = DependencyProperty.Register
            ("InternalOpacity",
            typeof(Double),
            typeof(PlotArea),
            new PropertyMetadata(1.0, OnOpacityPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.PlotArea.BorderThickness dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.PlotArea.BorderThickness dependency property.
        /// </returns>
        private static readonly DependencyProperty InternalBorderThicknessProperty = DependencyProperty.Register
            ("InternalBorderThickness",
            typeof(Thickness),
            typeof(PlotArea),
            new PropertyMetadata(OnBorderThicknessPropertyChanged));
#else
        
        /// <summary>
        /// Identifies the Visifire.Charts.PlotArea.Background dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.PlotArea.Background dependency property.
        /// </returns>
        public new static readonly DependencyProperty BackgroundProperty = DependencyProperty.Register
            ("Background",
            typeof(Brush),
            typeof(PlotArea),
            new PropertyMetadata(OnBackgroundPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.PlotArea.Opacity dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.PlotArea.Opacity dependency property.
        /// </returns>
        public new static readonly DependencyProperty OpacityProperty = DependencyProperty.Register
            ("Opacity",
            typeof(Double),
            typeof(PlotArea),
            new PropertyMetadata(1.0, OnOpacityPropertyChanged));

                /// <summary>
        /// Identifies the Visifire.Charts.PlotArea.BorderThickness dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.PlotArea.BorderThickness dependency property.
        /// </returns>
        public new static readonly DependencyProperty BorderThicknessProperty = DependencyProperty.Register
            ("BorderThickness",
            typeof(Thickness),
            typeof(PlotArea),
            new PropertyMetadata(OnBorderThicknessPropertyChanged));
#endif

        /// <summary>
        /// Identifies the Visifire.Charts.PlotArea.HrefTarget dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.PlotArea.HrefTarget dependency property.
        /// </returns>
        public static readonly DependencyProperty HrefTargetProperty = DependencyProperty.Register
            ("HrefTarget",
            typeof(HrefTargets),
            typeof(PlotArea),
            new PropertyMetadata(OnHrefTargetChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.PlotArea.Href dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.PlotArea.Href dependency property.
        /// </returns>
        public static readonly DependencyProperty HrefProperty = DependencyProperty.Register
            ("Href",
            typeof(String),
            typeof(PlotArea),
            new PropertyMetadata(OnHrefChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.PlotArea.Bevel dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.PlotArea.Bevel dependency property.
        /// </returns>
        public static readonly DependencyProperty BevelProperty = DependencyProperty.Register
            ("Bevel",
            typeof(Boolean),
            typeof(PlotArea),
            new PropertyMetadata(OnBevelPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.PlotArea.BorderColor dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.PlotArea.BorderColor dependency property.
        /// </returns>
        public static readonly DependencyProperty BorderColorProperty = DependencyProperty.Register
            ("BorderColor",
            typeof(Brush),
            typeof(PlotArea),
            new PropertyMetadata(OnBorderColorPropertyChanged));

#if WPF
        

#endif

        /// <summary>
        /// Identifies the Visifire.Charts.PlotArea.LightingEnabled dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.PlotArea.LightingEnabled dependency property.
        /// </returns>
        public static readonly DependencyProperty LightingEnabledProperty = DependencyProperty.Register
            ("LightingEnabled",
            typeof(Boolean),
            typeof(PlotArea),
            new PropertyMetadata(OnLightingEnabledPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.PlotArea.CornerRadius dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.PlotArea.CornerRadius dependency property.
        /// </returns>
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register
            ("CornerRadius",
            typeof(CornerRadius),
            typeof(PlotArea),
            new PropertyMetadata(OnCornerRadiusPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.PlotArea.ShadowEnabled dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.PlotArea.ShadowEnabled dependency property.
        /// </returns>
        public static readonly DependencyProperty ShadowEnabledProperty = DependencyProperty.Register
            ("ShadowEnabled",
            typeof(Nullable<Boolean>),
            typeof(PlotArea),
            new PropertyMetadata(OnShadowEnabledPropertyChanged));

        /// <summary>
        /// Get or set HrefTarget property of PlotArea
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
        /// Get or set Href property of PlotArea
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

                    if (Visual != null)
                        Visual.Cursor = (Cursor == null) ? Cursors.Arrow : Cursor;
                    else
                        FirePropertyChanged(VcProperties.Cursor);
                }
            }
        }

        /// <summary>
        /// Get or set Bevel property of PlotArea for Bevel effect
        /// </summary>
        public Boolean Bevel
        {
            get
            {
                return (Boolean)GetValue(BevelProperty);
            }
            set
            {
                SetValue(BevelProperty, value);
            }
        }

        /// <summary>
        /// Get or set BorderColor property of PlotArea
        /// </summary>
        public Brush BorderColor
        {
            get
            {
                return (Brush)GetValue(BorderColorProperty);
            }
            set
            {
                SetValue(BorderColorProperty, value);
            }
        }
        
        /// <summary>
        /// Get or set ToolTipText property for the Chart
        /// </summary>
        public override String ToolTipText
        {
            get
            {
                if (Chart != null && !String.IsNullOrEmpty(Chart.ToolTipText))
                    return null;
                else
                    return (String)GetValue(ToolTipTextProperty);
            }
            set
            {
                SetValue(ToolTipTextProperty, value);
            }
        }

        /// <summary>
        /// Get or set BorderThickness property of PlotArea
        /// </summary>
        public new Thickness BorderThickness
        {
            get
            {
                return (Thickness)GetValue(BorderThicknessProperty);
            }
            set
            {
#if WPF
                SetValue(BorderThicknessProperty, value);
#else
                InternalBorderThickness = value;
                SetValue(BorderThicknessProperty, value);
                FirePropertyChanged(VcProperties.BorderThickness);
#endif
            }
        }

        /// <summary>
        /// Get or set the BorderThickness of title
        /// </summary>
        internal Thickness InternalBorderThickness
        {
            get
            {
                return (Thickness)((_borderThickness == null) ? GetValue(BorderThicknessProperty) : _borderThickness);
            }
            set
            {
                _borderThickness = value;
            }
        }

        /// <summary>
        /// Get or set LightingEnabled property of PlotArea
        /// </summary>
        public Boolean LightingEnabled
        {
            get
            {
                return (Boolean)GetValue(LightingEnabledProperty);
            }
            set
            {
                SetValue(LightingEnabledProperty, value);
            }
        }


        /// <summary>
        /// Get or set CornerRadius property of PlotArea
        /// </summary>
#if WPF
        [System.ComponentModel.TypeConverter(typeof(System.Windows.CornerRadiusConverter))]
#else
        [System.ComponentModel.TypeConverter(typeof(Converters.CornerRadiusConverter))]
#endif
        public CornerRadius CornerRadius
        {
            get
            {
                return (CornerRadius)GetValue(CornerRadiusProperty);
            }
            set
            {
                SetValue(CornerRadiusProperty, value);
            }
        }

        /// <summary>
        /// Get or set ShadowEnabled property of PlotArea
        /// </summary>
        [System.ComponentModel.TypeConverter(typeof(NullableBoolConverter))]
        public Nullable<Boolean> ShadowEnabled
        {
            get
            {
                if ((Nullable<Boolean>)GetValue(ShadowEnabledProperty) == null)
                {
                    if (Chart != null)
                    {
                        Chart chart = Chart as Chart;
                        if (!chart.View3D && chart.ChartArea != null && chart.ChartArea.AxisY2 == null
                            && chart.PlotDetails != null && chart.PlotDetails.ChartOrientation == ChartOrientationType.Vertical)
                            return true;
                        else
                            return false;
                    }
                    else
                        return false;
                }
                else
                    return (Nullable<Boolean>)GetValue(ShadowEnabledProperty);
            }
            set
            {
                SetValue(ShadowEnabledProperty, value);
            }
        }

        /// <summary>
        /// Get or set Background property of PlotArea
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
        #endregion

        #region Public Events And Delegates

        /// <summary>
        /// Event handler for the MouseLeftButtonDown event 
        /// </summary>
#if SL
        [ScriptableMember]
#endif
        public new event EventHandler<PlotAreaMouseButtonEventArgs> MouseLeftButtonDown
        {
            remove
            {
                _onMouseLeftButtonDown -= value;

                if (EventChanged != null)
                    EventChanged(this, null);
            }
            add
            {   
                _onMouseLeftButtonDown += value;

                if (EventChanged != null)
                    EventChanged(this, null);
            }
        }

        /// <summary>
        /// Event handler for the MouseLeftButtonUp event 
        /// </summary>
#if SL
        [ScriptableMember]
#endif
        public new event EventHandler<PlotAreaMouseButtonEventArgs> MouseLeftButtonUp
        {
            remove
            {
                _onMouseLeftButtonUp -= value;

                if (EventChanged != null)
                    EventChanged(this, null);
            }
            add
            {
                _onMouseLeftButtonUp += value;

                if (EventChanged != null)
                    EventChanged(this, null);
            }
        }

#if WPF
        /// <summary>
        /// Event handler for the MouseLeftButtonDown event 
        /// </summary>
        public new event EventHandler<PlotAreaMouseButtonEventArgs> MouseRightButtonDown
        {
            remove
            {
                _onMouseRightButtonDown -= value;

                if (EventChanged != null)
                    EventChanged(this, null);
            }
            add
            {
                _onMouseRightButtonDown += value;

                if (EventChanged != null)
                    EventChanged(this, null);
            }
        }

        /// <summary>
        /// Event handler for the MouseLeftButtonUp event 
        /// </summary>
        public new event EventHandler<PlotAreaMouseButtonEventArgs> MouseRightButtonUp
        {
            remove
            {
                _onMouseRightButtonUp -= value;

                if (EventChanged != null)
                    EventChanged(this, null);
            }
            add
            {
                _onMouseRightButtonUp += value;

                if (EventChanged != null)
                    EventChanged(this, null);
            }
        }
#endif

        /// <summary>
        /// Event handler for the MouseMove event 
        /// </summary>
#if SL
        [ScriptableMember]
#endif
        public new event EventHandler<PlotAreaMouseEventArgs> MouseMove
        {
            remove
            {
                _onMouseMove -= value;

                if (EventChanged != null)
                    EventChanged(this, null);
            }
            add
            {
                _onMouseMove += value;

                if (EventChanged != null)
                    EventChanged(this, null);
            }
        }

        #endregion

        #region Protected Methods

        #endregion

        #region Internal Properties

        /// <summary>
        /// Border used for applying lighting effect over PlotArea
        /// </summary>
        internal Border LightingBorder
        {
            get;
            set;
        }

        /// <summary>
        /// Bevel grid of PlotArea
        /// </summary>
        internal Grid BevelGrid
        {
            get;
            set;
        }

        /// <summary>
        /// Shadow grid of PlotArea
        /// </summary>
        internal FrameworkElement ShadowElement
        {
            get;
            set;
        }

        /// <summary>
        /// Shadow inside ScrollViewer
        /// </summary>
        internal FrameworkElement InnerShadowElement
        {
            get;
            set;
        }

        /// <summary>
        /// PlotArea visual 
        /// </summary>
        internal Canvas Visual
        {
            get;
            set;
        }

        /// <summary>
        /// Border element of PlotArea
        /// </summary>
        internal Border BorderElement
        {
            get;
            set;
        }

        #endregion

        #region Private Properties

        #endregion

        #region Private Delegates

        #endregion

        #region Private Methods

        /// <summary>
        /// BackgroundProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnBackgroundPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PlotArea plotArea = d as PlotArea;
            plotArea.InternalBackground = (Brush)e.NewValue;
            plotArea.FirePropertyChanged(VcProperties.Background);
        }

        /// <summary>
        /// OpacityProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnOpacityPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PlotArea plotArea = d as PlotArea;
            plotArea.InternalOpacity = (Double)e.NewValue;
            plotArea.FirePropertyChanged(VcProperties.Opacity);
        }

        /// <summary>
        /// BorderThicknessProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnBorderThicknessPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PlotArea plotArea = d as PlotArea;
            plotArea.InternalBorderThickness = (Thickness)e.NewValue;
            plotArea.FirePropertyChanged(VcProperties.BorderThickness);
        }

        /// <summary>
        /// HrefTargetProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnHrefTargetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as PlotArea).FirePropertyChanged(VcProperties.HrefTarget);
        }

        /// <summary>
        /// HrefProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnHrefChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as PlotArea).FirePropertyChanged(VcProperties.Href);
        }

        /// <summary>
        /// BevelProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnBevelPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as PlotArea).FirePropertyChanged(VcProperties.Bevel);
        }

        /// <summary>
        /// BorderColorProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnBorderColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as PlotArea).FirePropertyChanged(VcProperties.BorderColor);
        }

        /// <summary>
        /// LightingEnabledProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnLightingEnabledPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as PlotArea).FirePropertyChanged(VcProperties.LightingEnabled);
        }

        /// <summary>
        /// CornerRadiusProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>        
        private static void OnCornerRadiusPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as PlotArea).FirePropertyChanged(VcProperties.CornerRadius);
        }

        /// <summary>
        /// ShadowEnabledProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnShadowEnabledPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as PlotArea).FirePropertyChanged(VcProperties.ShadowEnabled);
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Creates visual of PlotArea
        /// </summary>
        internal void CreateVisualObject()
        {
            if (Visual == null)
            {
                Visual = new Canvas();
                Visual.Children.Add(GetNewBorderElement());
            }

            ApplyProperties();
            ApplyLighting();
        }

        /// <summary>
        /// Creates new _axisIndicatorBorderElement element for PlotArea
        /// </summary>
        /// <returns>Border</returns>
        internal Border GetNewBorderElement()
        {
            BorderElement = new Border() { Tag = new ElementData() { Element = this } };
            LightingBorder = new Border() { Tag = new ElementData() { Element = this } };

            BevelGrid = new Grid();
            BevelGrid.Children.Add(LightingBorder);
            BorderElement.Child = BevelGrid;

            ApplyProperties();

            return BorderElement;
        }

        /// <summary>
        /// Update PlotArea with new property values
        /// </summary>
        internal void UpdateProperties()
        {
            BorderElement.SetValue(Canvas.TopProperty, (Double)0);
            BorderElement.SetValue(Canvas.LeftProperty, (Double)0);

            ApplyProperties();
            ApplyLighting();
        }

        /// <summary>
        /// Calculates the top and left position of PlotArea
        /// </summary>
        /// <param name="chart"></param>
        internal Point GetPlotAreaStartPosition()
        {
            Double left, top;
            Chart chart = Chart as Chart;
            Axis xAxis = chart.ChartArea.AxisX;
            Axis yAxis = chart.ChartArea.AxisY;

            if (chart.PlotDetails.ChartOrientation == ChartOrientationType.Vertical)
            {   
                if (yAxis != null)
                {
                    left = Axis.GetAxisLeft(yAxis) + yAxis.Width;

                    top = Axis.GetAxisTop(yAxis);
                }
                else
                {
                    left = Chart.Padding.Left + Chart._leftOuterPanel.ActualWidth +
                        chart._leftOffsetGrid.ActualWidth + Chart.BorderThickness.Left;

                    top = Chart.Padding.Top + chart._topOffsetGrid.ActualHeight 
                        + Chart._topOuterPanel.ActualHeight + Chart._topAxisGrid.ActualHeight 
                        + Chart.BorderThickness.Top;
                }
            }
            else
            {
                if (xAxis != null)
                {
                    left = Axis.GetAxisLeft(xAxis) + xAxis.Width;

                    top = Axis.GetAxisTop(xAxis);
                }
                else
                {
                    left = Chart.Padding.Left + Chart._leftOuterPanel.ActualWidth
                        + chart._leftOffsetGrid.ActualWidth + Chart.BorderThickness.Left;

                    top = Chart.Padding.Top + chart._topOffsetGrid.ActualHeight 
                        + Chart._topOuterPanel.ActualHeight + Chart._topAxisGrid.ActualHeight 
                        + Chart.BorderThickness.Top;
                }
            }
            
            this.SetValue(Canvas.LeftProperty, left);
            this.SetValue(Canvas.TopProperty, top);

            return new Point(left, top);
        }

        /// <summary>
        /// Apply properties to visual and parts of visual
        /// </summary>
        internal void ApplyProperties()
        {
            Visual.Margin = new Thickness(0);
            Visual.Cursor = (Cursor == null) ? Cursors.Arrow : Cursor;

            BorderElement.Opacity = this.InternalOpacity;

            BorderElement.Background = InternalBackground;

            if (BorderColor != null)
                BorderElement.BorderBrush = BorderColor;

            if (InternalBorderThickness != null)
            {
                BorderElement.BorderThickness = InternalBorderThickness;
            }

            if (CornerRadius != null)
            {
                BorderElement.CornerRadius = CornerRadius;
                LightingBorder.CornerRadius = CornerRadius;
                
            }
        }

        /// <summary>
        /// Apply PlotAreaBevel
        /// </summary>
        internal void ApplyBevel(Double plankDepth, Double plankThickness)
        {
            if (_bevelCanvas != null)
            {
                BevelGrid.Children.Remove(_bevelCanvas);
                _bevelCanvas = null;
            }

            if (Bevel)
            {
                Chart chart = Chart as Chart;

                _bevelCanvas = ExtendedGraphics.Get2DRectangleBevel(this, BorderElement.Width - InternalBorderThickness.Left - InternalBorderThickness.Right// - plankDepth
                , BorderElement.Height - InternalBorderThickness.Top - InternalBorderThickness.Bottom //+ ((chart.PlotDetails.ChartOrientation == ChartOrientationType.Horizontal || chart.PlotDetails.ChartOrientation == ChartOrientationType.NoAxis) ? 0 : (-plankDepth - plankThickness))
                , Charts.Chart.BEVEL_DEPTH, Charts.Chart.BEVEL_DEPTH
                , Graphics.GetBevelTopBrush(BorderElement.Background)
                , Graphics.GetBevelSideBrush(0, BorderElement.Background)
                , Graphics.GetBevelSideBrush(180, BorderElement.Background)
                , Graphics.GetBevelSideBrush(90, BorderElement.Background));

                _bevelCanvas.SetValue(Canvas.LeftProperty, InternalBorderThickness.Left);
                _bevelCanvas.SetValue(Canvas.TopProperty, InternalBorderThickness.Top);

                _bevelCanvas.IsHitTestVisible = false;

                BevelGrid.Children.Add(_bevelCanvas);
            }
        }

        /// <summary>
        /// Apply lighting effect to PlotArea
        /// </summary>
        internal void ApplyLighting()
        {
            if (LightingBorder != null)
            {
                if (LightingEnabled)
                {
                    if (Bevel)
                    {
                        LightingBorder.Background = Graphics.GetFrontFaceBrush(InternalBackground);
                        LightingBorder.Background.Opacity = 0.6;
                    }
                    else
                    {
                        LightingBorder.Background = Graphics.LightingBrush(LightingEnabled);
                    }
                }
                else
                    LightingBorder.Background = new SolidColorBrush(Colors.Transparent);
            }

        }

        /// <summary>
        /// Apply shadow for PlotArea
        /// </summary>
        internal void ApplyShadow(Size plotAreaViewPortSize, Double plankOffset, Double plankDepth, Double plankThickness)
        {
            Chart._plotAreaShadowCanvas.Children.Clear();
            Chart._drawingCanvas.Children.Remove(InnerShadowElement);

            if (ShadowElement != null)
                ShadowElement = null;

            Chart chart = Chart as Chart;

            if(VisifireControl.IsXbapApp)
                GetShadow4XBAP(chart, plotAreaViewPortSize, plankOffset, plankDepth, plankThickness);
            else
                GetShadow(chart, plotAreaViewPortSize, plankOffset, plankDepth, plankThickness);
        }

        private void GetShadow4XBAP(Chart chart, Size plotAreaViewPortSize, Double plankOffset, Double plankDepth, Double plankThickness)
        {
            if ((Boolean)chart.PlotArea.ShadowEnabled || chart.View3D)
            {
                Size clipSize;

                if (chart.PlotDetails.ChartOrientation == ChartOrientationType.NoAxis)
                {
                    if ((Boolean)chart.PlotArea.ShadowEnabled)
                    {
                        ShadowElement = ExtendedGraphics.Get2DRectangleShadow(this, BorderElement.Width + Visifire.Charts.Chart.SHADOW_DEPTH
                        , BorderElement.Height + Visifire.Charts.Chart.SHADOW_DEPTH
                        , CornerRadius
                        , CornerRadius, 6);

                        clipSize = new Size(ShadowElement.Width, ShadowElement.Height);

                        ShadowElement.Clip = ExtendedGraphics.GetShadowClip(clipSize);
                    }
                }
                else
                {
                    if (chart.View3D)
                    {
                        if (chart.PlotDetails.ChartOrientation == ChartOrientationType.Horizontal)
                        {
                            if ((Boolean)chart.PlotArea.ShadowEnabled)
                            {
                                ShadowElement = ExtendedGraphics.Get2DRectangleShadow(this, plotAreaViewPortSize.Width + Visifire.Charts.Chart.SHADOW_DEPTH - plankThickness - plankDepth - ChartArea.SCROLLVIEWER_OFFSET4HORIZONTAL_CHART
                                , plotAreaViewPortSize.Height - plankDepth + Visifire.Charts.Chart.SHADOW_DEPTH
                                , CornerRadius
                                , CornerRadius, 6);
                                clipSize = new Size(ShadowElement.Width, ShadowElement.Height + Visifire.Charts.Chart.SHADOW_DEPTH);

                                ShadowElement.SetValue(Canvas.LeftProperty, plankOffset);
                                ShadowElement.Clip = ExtendedGraphics.GetShadowClip(clipSize);

                                InnerShadowElement = ExtendedGraphics.Get2DRectangleShadow(this, plotAreaViewPortSize.Width + Visifire.Charts.Chart.SHADOW_DEPTH - plankThickness - plankDepth - ChartArea.SCROLLVIEWER_OFFSET4HORIZONTAL_CHART
                               , BorderElement.Height + Visifire.Charts.Chart.SHADOW_DEPTH
                               , CornerRadius
                               , CornerRadius, 6);
                                InnerShadowElement.Clip = ExtendedGraphics.GetShadowClip(new Size(InnerShadowElement.Width + Visifire.Charts.Chart.SHADOW_DEPTH, InnerShadowElement.Height));
                                InnerShadowElement.SetValue(Canvas.LeftProperty, plankOffset);
                                Chart._drawingCanvas.Children.Add(InnerShadowElement);
                            }
                        }
                        else
                        {
                            if ((Boolean)chart.PlotArea.ShadowEnabled)
                            {
                                ShadowElement = ExtendedGraphics.Get2DRectangleShadow(this, plotAreaViewPortSize.Width + Visifire.Charts.Chart.SHADOW_DEPTH - plankThickness - plankDepth
                                                       , plotAreaViewPortSize.Height + Visifire.Charts.Chart.SHADOW_DEPTH - plankOffset
                                                       , CornerRadius
                                                       , CornerRadius, 6);
                                clipSize = new Size(ShadowElement.Width, ShadowElement.Height);
                                ShadowElement.SetValue(Canvas.LeftProperty, plankOffset);
                                ShadowElement.Clip = ExtendedGraphics.GetShadowClip(clipSize);

                            }
                            else
                            {
                                Double offset = 3.5;
                                if (this.Background != null && !Graphics.AreBrushesEqual(this.Background, new SolidColorBrush(Colors.Transparent)))
                                {
                                    ShadowElement = ExtendedGraphics.GetRectangle4PlotAreaEdge(this, plotAreaViewPortSize.Width + offset - plankThickness - plankDepth
                                                           , plotAreaViewPortSize.Height - plankOffset
                                                           , CornerRadius
                                                           , CornerRadius, 6, this.InternalBackground);

                                    clipSize = new Size(ShadowElement.Width, ShadowElement.Height + 5);
                                    ShadowElement.SetValue(Canvas.LeftProperty, plankOffset);
                                    ShadowElement.Clip = ExtendedGraphics.GetShadowClip(clipSize);
                                }
                            }
                        }
                    }
                    else
                    {
                        if ((Boolean)chart.PlotArea.ShadowEnabled)
                        {
                            ShadowElement = ExtendedGraphics.Get2DRectangleShadow(this, plotAreaViewPortSize.Width + Charts.Chart.SHADOW_DEPTH
                                , plotAreaViewPortSize.Height - plankOffset + Visifire.Charts.Chart.SHADOW_DEPTH, CornerRadius
                                , CornerRadius
                                , 6);

                            clipSize = new Size(ShadowElement.Width, ShadowElement.Height);
                            ShadowElement.Clip = ExtendedGraphics.GetShadowClip(clipSize);
                        }
                    }
                }


                if (ShadowElement != null)
                {
                    ShadowElement.IsHitTestVisible = false;
                    Chart._plotAreaShadowCanvas.Children.Add(ShadowElement);
                    ShadowElement.UpdateLayout();
                }
            }
        }

        private void GetShadow(Chart chart, Size plotAreaViewPortSize, Double plankOffset, Double plankDepth, Double plankThickness)
        {
            if ((Boolean)chart.PlotArea.ShadowEnabled || chart.View3D)
            {
                Size clipSize;

                if (chart.PlotDetails.ChartOrientation == ChartOrientationType.NoAxis)
                {
                    if ((Boolean)chart.PlotArea.ShadowEnabled)
                    {
                        ShadowElement = new Border() { CornerRadius = CornerRadius };
                        ShadowElement.Width = BorderElement.Width;
                        ShadowElement.Height = BorderElement.Height;

                        ShadowElement.Effect = ExtendedGraphics.GetShadowEffect(315, 4, 0.95);
                        //clipSize = new Size(ShadowGrid.Width, ShadowGrid.Height);

                        if (this.Background != null && !Graphics.AreBrushesEqual(this.Background, new SolidColorBrush(Colors.Transparent)))
                            (ShadowElement as Border).Background = this.Background;
                        else
                        {
                            (ShadowElement as Border).Background = new SolidColorBrush(Colors.LightGray);
                            clipSize = new Size(ShadowElement.Width + 5, ShadowElement.Height + 5);
                            ShadowElement.Clip = ExtendedGraphics.GetShadowClip(clipSize);
                        }
                    }
                }
                else
                {
                    if (chart.View3D)
                    {
                        if (chart.PlotDetails.ChartOrientation == ChartOrientationType.Horizontal)
                        {
                            if ((Boolean)chart.PlotArea.ShadowEnabled)
                            {
                                ShadowElement = new Border() { CornerRadius = CornerRadius };
                                ShadowElement.Width = plotAreaViewPortSize.Width - plankThickness - plankDepth - ChartArea.SCROLLVIEWER_OFFSET4HORIZONTAL_CHART;
                                ShadowElement.Height = plotAreaViewPortSize.Height - plankDepth;

                                ShadowElement.Effect = ExtendedGraphics.GetShadowEffect(315, 4, 0.95);

                                //clipSize = new Size(ShadowGrid.Width, ShadowGrid.Height - 4);

                                if (this.Background != null && !Graphics.AreBrushesEqual(this.Background, new SolidColorBrush(Colors.Transparent)))
                                {
                                    (ShadowElement as Border).Background = this.Background;
                                    //clipSize = new Size(ShadowGrid.Width + 5, ShadowGrid.Height + 5);
                                    //ShadowGrid.Clip = GetShadowClip(clipSize);
                                }
                                else
                                {
                                    (ShadowElement as Border).Background = new SolidColorBrush(Colors.LightGray);
                                    clipSize = new Size(ShadowElement.Width + 5, ShadowElement.Height + 5);
                                    ShadowElement.Clip = ExtendedGraphics.GetShadowClip(clipSize);
                                }

                                ShadowElement.SetValue(Canvas.LeftProperty, plankOffset);

                                InnerShadowElement = new Border() { CornerRadius = CornerRadius };
                                InnerShadowElement.Width = plotAreaViewPortSize.Width - plankThickness - plankDepth - ChartArea.SCROLLVIEWER_OFFSET4HORIZONTAL_CHART;
                                InnerShadowElement.Height = BorderElement.Height;

                                InnerShadowElement.Effect = ExtendedGraphics.GetShadowEffect(315, 4, 0.95);
                                InnerShadowElement.SetValue(Canvas.LeftProperty, plankOffset);
                                if (this.Background != null && !Graphics.AreBrushesEqual(this.Background, new SolidColorBrush(Colors.Transparent)))
                                {
                                    InnerShadowElement.Clip = ExtendedGraphics.GetShadowClip(new Size(InnerShadowElement.Width, InnerShadowElement.Height + 6));
                                    (InnerShadowElement as Border).Background = this.Background;
                                }
                                else
                                {
                                    (InnerShadowElement as Border).Background = new SolidColorBrush(Colors.LightGray);
                                    clipSize = new Size(InnerShadowElement.Width + 5, InnerShadowElement.Height + 6);
                                    InnerShadowElement.Clip = ExtendedGraphics.GetShadowClip(clipSize);
                                }
                                Chart._drawingCanvas.Children.Add(InnerShadowElement);
                            }
                        }
                        else
                        {
                            if ((Boolean)chart.PlotArea.ShadowEnabled)
                            {
                                ShadowElement = new Border() { CornerRadius = CornerRadius };
                                ShadowElement.Width = plotAreaViewPortSize.Width - plankThickness - plankDepth;
                                ShadowElement.Height = plotAreaViewPortSize.Height - plankOffset;

                                ShadowElement.SetValue(Canvas.LeftProperty, plankOffset);

                                ShadowElement.Effect = ExtendedGraphics.GetShadowEffect(315, 4, 0.95);

                                if (this.Background != null && !Graphics.AreBrushesEqual(this.Background, new SolidColorBrush(Colors.Transparent)))
                                    (ShadowElement as Border).Background = this.Background;
                                else
                                {
                                    (ShadowElement as Border).Background = new SolidColorBrush(Colors.LightGray);
                                    clipSize = new Size(ShadowElement.Width + 5, ShadowElement.Height + 5);
                                    ShadowElement.Clip = ExtendedGraphics.GetShadowClip(clipSize);
                                }
                            }
                            else
                            {
                                Double offset = 3.5;
                                if (this.Background != null && !Graphics.AreBrushesEqual(this.Background, new SolidColorBrush(Colors.Transparent)))
                                {
                                    ShadowElement = ExtendedGraphics.GetRectangle4PlotAreaEdge(this, plotAreaViewPortSize.Width + offset - plankThickness - plankDepth
                                                           , plotAreaViewPortSize.Height - plankOffset
                                                           , CornerRadius
                                                           , CornerRadius, 6, this.InternalBackground);

                                    clipSize = new Size(ShadowElement.Width, ShadowElement.Height + 5);
                                    ShadowElement.SetValue(Canvas.LeftProperty, plankOffset);
                                    ShadowElement.Clip = ExtendedGraphics.GetShadowClip(clipSize);
                                }
                            }
                        }
                    }
                    else
                    {
                        if ((Boolean)chart.PlotArea.ShadowEnabled)
                        {
                            ShadowElement = new Border() { CornerRadius = CornerRadius };
                            ShadowElement.Width = plotAreaViewPortSize.Width;
                            ShadowElement.Height = plotAreaViewPortSize.Height - plankOffset;

                            ShadowElement.Effect = ExtendedGraphics.GetShadowEffect(315, 4, 0.95);
                            //clipSize = new Size(ShadowGrid.Width, ShadowGrid.Height);
                            if (this.Background != null && !Graphics.AreBrushesEqual(this.Background, new SolidColorBrush(Colors.Transparent)))
                                (ShadowElement as Border).Background = this.Background;
                            else
                            {   
                                if ((this.Background == null || Graphics.AreBrushesEqual(this.Background, new SolidColorBrush(Colors.Transparent)))

                                    && (Chart.Background != null && !Graphics.AreBrushesEqual(Chart.Background, new SolidColorBrush(Colors.Transparent))))
                                    (ShadowElement as Border).Background = Chart.Background;
                                else
                                    (ShadowElement as Border).Background = new SolidColorBrush(Color.FromArgb((byte)255, (byte)254, (byte)254, (byte)254));
                                
                                // clipSize = new Size(ShadowElement.Width + 5, ShadowElement.Height + 5);
                                // ShadowElement.Clip = ExtendedGraphics.GetShadowClip(clipSize);
                            }
                        }
                    }
                }

                if (ShadowElement != null)
                {
                    ShadowElement.IsHitTestVisible = false;
                    Chart._plotAreaShadowCanvas.Children.Add(ShadowElement);
                    ShadowElement.UpdateLayout();
                }
            }
        }

        /// <summary>
        /// Creates PlotAreaMouseButtonEventArgs
        /// </summary>
        /// <param name="e">MouseButtonEventArgs</param>
        /// <returns>PlotAreaMouseButtonEventArgs</returns>
        internal PlotAreaMouseButtonEventArgs CreatePlotAreaMouseButtonEventArgs(MouseButtonEventArgs e)
        {
            Chart chart = Chart as Chart;
            PlotAreaMouseButtonEventArgs eventArgs = new PlotAreaMouseButtonEventArgs(e);

            if (chart.ChartArea.AxisX != null)
            {
                Double xValue;
                AxisOrientation axisOrientation = chart.ChartArea.AxisX.AxisOrientation;
                Double pixelPosition = (axisOrientation == AxisOrientation.Horizontal) ? e.GetPosition(chart.ChartArea.PlottingCanvas).X : e.GetPosition(chart.ChartArea.PlottingCanvas).Y;
                Double lenthInPixel = ((axisOrientation == AxisOrientation.Horizontal) ? chart.ChartArea.ChartVisualCanvas.Width : chart.ChartArea.ChartVisualCanvas.Height);

                xValue = chart.ChartArea.AxisX.PixelPositionToXValue(lenthInPixel, (axisOrientation == AxisOrientation.Horizontal) ? pixelPosition : lenthInPixel - pixelPosition);

                if (chart.ChartArea.AxisX.IsDateTimeAxis)
                    eventArgs.XValue = DateTimeHelper.XValueToDateTime(chart.ChartArea.AxisX.MinDate, xValue, chart.ChartArea.AxisX.InternalIntervalType);
                else
                    eventArgs.XValue = xValue;

            }

            if (chart.ChartArea.AxisY != null)
            {
                Double yValue;
                AxisOrientation axisOrientation = chart.ChartArea.AxisY.AxisOrientation;
                Double pixelPosition = (axisOrientation == AxisOrientation.Vertical) ? e.GetPosition(chart.ChartArea.PlottingCanvas).Y : e.GetPosition(chart.ChartArea.PlottingCanvas).X;
                Double lenthInPixel = ((axisOrientation == AxisOrientation.Vertical) ? chart.ChartArea.ChartVisualCanvas.Height : chart.ChartArea.ChartVisualCanvas.Width);

                yValue = chart.ChartArea.AxisY.PixelPositionToYValue(lenthInPixel, (axisOrientation == AxisOrientation.Vertical) ? pixelPosition : lenthInPixel - pixelPosition);

                if (chart.ChartArea.AxisY.Logarithmic)
                    eventArgs.YValue = DataPoint.ConvertLogarithmicValue2ActualValue(chart, yValue, chart.ChartArea.AxisY.AxisType);
                else
                    eventArgs.YValue = yValue;
            }

            return eventArgs;
        }

        /// <summary>
        /// Creates PlotAreaMouseEventArgs
        /// </summary>
        /// <param name="e">MouseButtonEventArgs</param>
        /// <returns>PlotAreaMouseButtonEventArgs</returns>
        internal PlotAreaMouseEventArgs CreatePlotAreaMouseEventArgs(MouseEventArgs e)
        {   
            Chart chart = Chart as Chart;
            PlotAreaMouseEventArgs eventArgs = new PlotAreaMouseEventArgs(e);

            if (chart.ChartArea.AxisX != null)
            {   
                Double xValue;
                AxisOrientation axisOrientation = chart.ChartArea.AxisX.AxisOrientation;
                Double pixelPosition = (axisOrientation == AxisOrientation.Horizontal) ? e.GetPosition(chart.ChartArea.PlottingCanvas).X : e.GetPosition(chart.ChartArea.PlottingCanvas).Y;
                Double lengthInPixel = ((axisOrientation == AxisOrientation.Horizontal) ? chart.ChartArea.ChartVisualCanvas.Width : chart.ChartArea.ChartVisualCanvas.Height);

                xValue = chart.ChartArea.AxisX.PixelPositionToXValue(lengthInPixel, (axisOrientation == AxisOrientation.Horizontal) ? pixelPosition : lengthInPixel - pixelPosition);

                if (chart.ChartArea.AxisX.IsDateTimeAxis)
                    eventArgs.XValue = DateTimeHelper.XValueToDateTime(chart.ChartArea.AxisX.MinDate, xValue, chart.ChartArea.AxisX.InternalIntervalType);
                else
                    eventArgs.XValue = xValue;
            }

            if (chart.ChartArea.AxisY != null)
            {   
                Double yValue;
                AxisOrientation axisOrientation = chart.ChartArea.AxisY.AxisOrientation;
                Double pixelPosition = (axisOrientation == AxisOrientation.Vertical) ? e.GetPosition(chart.ChartArea.PlottingCanvas).Y : e.GetPosition(chart.ChartArea.PlottingCanvas).X;
                Double lengthInPixel = ((axisOrientation == AxisOrientation.Vertical) ? chart.ChartArea.ChartVisualCanvas.Height : chart.ChartArea.ChartVisualCanvas.Width);

                yValue = chart.ChartArea.AxisY.PixelPositionToYValue(lengthInPixel, (axisOrientation == AxisOrientation.Vertical) ? pixelPosition : lengthInPixel - pixelPosition);

                if(chart.ChartArea.AxisY.Logarithmic)
                    eventArgs.YValue = DataPoint.ConvertLogarithmicValue2ActualValue(chart, yValue, chart.ChartArea.AxisY.AxisType);
                else
                    eventArgs.YValue = yValue;
            }

            if (chart.ChartArea.AxisY2 != null)
            {
                Double yValue;
                AxisOrientation axisOrientation = chart.ChartArea.AxisY2.AxisOrientation;
                Double pixelPosition = (axisOrientation == AxisOrientation.Vertical) ? e.GetPosition(chart.ChartArea.PlottingCanvas).Y : e.GetPosition(chart.ChartArea.PlottingCanvas).X;
                Double lengthInPixel = ((axisOrientation == AxisOrientation.Vertical) ? chart.ChartArea.ChartVisualCanvas.Height : chart.ChartArea.ChartVisualCanvas.Width);

                yValue = chart.ChartArea.AxisY2.PixelPositionToYValue(lengthInPixel, (axisOrientation == AxisOrientation.Vertical) ? pixelPosition : lengthInPixel - pixelPosition);

                if (chart.ChartArea.AxisY2.Logarithmic)
                    eventArgs.YValue = DataPoint.ConvertLogarithmicValue2ActualValue(chart, yValue, chart.ChartArea.AxisY2.AxisType);
                else
                    eventArgs.YValue = yValue;
            }

            return eventArgs;
        }

        /// <summary>
        /// Fire MouseLeftButtonDown event
        /// </summary>
        /// <param name="e">MouseButtonEventArgs</param>
        internal override void FireMouseLeftButtonDownEvent(Object sender, Object e)
        {   
            if (_onMouseLeftButtonDown != null)
                _onMouseLeftButtonDown(sender, CreatePlotAreaMouseButtonEventArgs(e as MouseButtonEventArgs));
        }

        /// <summary>
        /// Fire MouseLeftButtonDown event
        /// </summary>
        /// <param name="e">MouseButtonEventArgs</param>
        internal override void FireMouseLeftButtonUpEvent(Object sender, Object e)
        {
            if (_onMouseLeftButtonUp != null)
                _onMouseLeftButtonUp(this, CreatePlotAreaMouseButtonEventArgs(e as MouseButtonEventArgs));
        }

        /// <summary>
        /// Fire MouseLeftButtonDown event
        /// </summary>
        /// <param name="e">MouseButtonEventArgs</param>
        internal override void FireMouseMoveEvent(Object sender, Object e)
        {
            if (_onMouseMove != null)
                _onMouseMove(this, CreatePlotAreaMouseEventArgs(e as MouseEventArgs));
        }

#if WPF
        /// <summary>
        /// Fire MouseRightButtonDown event
        /// </summary>
        /// <param name="e">MouseButtonEventArgs</param>
        internal override void FireMouseRightButtonDownEvent(Object sender, Object e)
        {   
            if (_onMouseRightButtonDown != null)
                _onMouseRightButtonDown(sender, CreatePlotAreaMouseButtonEventArgs(e as MouseButtonEventArgs));

        }
        
        /// <summary>
        /// Fire MouseRightButtonDown event
        /// </summary>
        /// <param name="e">MouseButtonEventArgs</param>
        internal override void FireMouseRightButtonUpEvent(Object sender, Object e)
        {
            if (_onMouseRightButtonUp != null)
                _onMouseRightButtonUp(this, CreatePlotAreaMouseButtonEventArgs(e as MouseButtonEventArgs));
        }

#endif

        /// <summary>
        /// Get MouseLeftButtonDown EventHandler
        /// </summary>
        /// <returns></returns>
        internal override object GetMouseLeftButtonDownEventHandler()
        {
            return _onMouseLeftButtonDown;
        }

        /// <summary>
        /// Get MouseLeftButtonUp EventHandler
        /// </summary>
        /// <returns></returns>
        internal override object GetMouseLeftButtonUpEventHandler()
        {
            return _onMouseLeftButtonUp;
        }

        /// <summary>
        /// Get MouseLeftButtonUp EventHandler
        /// </summary>
        /// <returns></returns>
        internal override object GetMouseMoveEventHandler()
        {
            return _onMouseMove;
        }

#if WPF
        /// <summary>
        /// Get MouseRightButtonDown EventHandler
        /// </summary>
        /// <returns></returns>
        internal override object GetMouseRightButtonDownEventHandler()
        {   
            return _onMouseRightButtonDown;
        }

        /// <summary>
        /// Get MouseRightButtonUp EventHandler
        /// </summary>
        /// <returns></returns>
        internal override object GetMouseRightButtonUpEventHandler()
        {
            return _onMouseRightButtonUp;
        }
#endif

        #endregion

        #region Internal Events And Delegates

        /// <summary>
        /// EventChanged event is fired if any event is attached
        /// </summary>
        internal new event EventHandler EventChanged;

        #endregion

        #region Data

        Brush _internalBackground = null;
        Nullable<Thickness> _borderThickness = null;
        Double _internalOpacity = Double.NaN;

        /// <summary>
        /// Handler for MouseLeftButtonDown event
        /// </summary>
        private event EventHandler<PlotAreaMouseButtonEventArgs> _onMouseLeftButtonDown;

        /// <summary>
        /// Handler for MouseLeftButtonUp event
        /// </summary>
        private event EventHandler<PlotAreaMouseButtonEventArgs> _onMouseLeftButtonUp;

#if WPF
        /// <summary>
        /// Handler for MouseRightButtonDown event
        /// </summary>
        private event EventHandler<PlotAreaMouseButtonEventArgs> _onMouseRightButtonDown;

        /// <summary>
        /// Handler for MouseRightButtonUp event
        /// </summary>
        private event EventHandler<PlotAreaMouseButtonEventArgs> _onMouseRightButtonUp;
#endif

        /// <summary>
        /// Handler for MouseMove event
        /// </summary>
        private event EventHandler<PlotAreaMouseEventArgs> _onMouseMove;

        /// <summary>
        /// Canvas for bevel
        /// </summary>
        Canvas _bevelCanvas;

#if WPF
        /// <summary>
        /// Whether the default style is applied
        /// </summary>
        private static Boolean _defaultStyleKeyApplied;
#endif
        #endregion


    }
}
