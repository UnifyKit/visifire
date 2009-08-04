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

            // Attach event handler with EventChanged event of VisfiireElement
            EventChanged += delegate
            {
                FirePropertyChanged(VcProperties.MouseEvent);
            };
        }

        #endregion

        #region Public Properties

#if WPF
        
        /// <summary>
        /// Identifies the Visifire.Charts.PlotArea.Background dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.PlotArea.Background dependency property.
        /// </returns>
        private new static readonly DependencyProperty BackgroundProperty = DependencyProperty.Register
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
            typeof(Boolean),
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
                    
                    if(Visual != null)
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
                SetValue(BorderThicknessProperty, value);
                FirePropertyChanged(VcProperties.BorderThickness);
#endif
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
        public Boolean ShadowEnabled
        {
            get
            {
                return (Boolean)GetValue(ShadowEnabledProperty);
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
                    SetValue(BackgroundProperty, value);
                    FirePropertyChanged(VcProperties.Background);
                }
#else
                SetValue(BackgroundProperty, value);
#endif
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
        internal Grid ShadowGrid
        {
            get;
            set;
        }

        /// <summary>
        /// Shadow inside ScrollViewer
        /// </summary>
        internal Grid InnerShadowGrid
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

#if WPF
        
        /// <summary>
        /// BackgroundProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnBackgroundPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as PlotArea).FirePropertyChanged(VcProperties.Background);
        }
#endif

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
        
#if WPF
        
        /// <summary>
        /// BorderThicknessProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnBorderThicknessPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as PlotArea).FirePropertyChanged(VcProperties.BorderThickness);
        }

        /// <summary>
        /// OpacityProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnOpacityPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as PlotArea).FirePropertyChanged(VcProperties.Opacity);
        }
#endif
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

        /// <summary>
        /// Returns PathGeometry for clipping Shadow of PlotArea
        /// </summary>
        /// <param name="clipSize">Size clipSize</param>
        /// <returns>PathGeometry</returns>
        private PathGeometry GetShadowClip(Size clipSize)
        {
            PathGeometry pathGeometry = new PathGeometry();
            pathGeometry.FillRule = FillRule.EvenOdd;
            pathGeometry.Figures = new PathFigureCollection();

            PathFigure pathFigure = new PathFigure();

            pathFigure.StartPoint = new Point(0, clipSize.Height - Charts.Chart.SHADOW_DEPTH);
            pathFigure.Segments = new PathSegmentCollection();

            // Do not change the order of the lines below
            pathFigure.Segments.Add(Graphics.GetLineSegment(new Point(clipSize.Width - Charts.Chart.SHADOW_DEPTH, clipSize.Height - Charts.Chart.SHADOW_DEPTH)));
            pathFigure.Segments.Add(Graphics.GetLineSegment(new Point(clipSize.Width - Charts.Chart.SHADOW_DEPTH, 0)));
            pathFigure.Segments.Add(Graphics.GetLineSegment(new Point(clipSize.Width, 0)));
            pathFigure.Segments.Add(Graphics.GetLineSegment(new Point(clipSize.Width, clipSize.Height)));
            pathFigure.Segments.Add(Graphics.GetLineSegment(new Point(0, clipSize.Height)));
            pathFigure.Segments.Add(Graphics.GetLineSegment(new Point(0, clipSize.Height - Charts.Chart.SHADOW_DEPTH)));

            pathGeometry.Figures.Add(pathFigure);

            return pathGeometry;
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
        /// Creates new border element for PlotArea
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
        /// Apply properties to visual and parts of visual
        /// </summary>
        internal void ApplyProperties()
        {
            Visual.Margin = new Thickness(0);
            Visual.Cursor = (Cursor == null) ? Cursors.Arrow : Cursor;

            BorderElement.Opacity = this.Opacity;

            if (Background != null)
                BorderElement.Background = Background;

            if (BorderColor != null)
                BorderElement.BorderBrush = BorderColor;

            if (BorderThickness != null)
            {
                BorderElement.BorderThickness = BorderThickness;
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
                BevelGrid.Children.Remove(_bevelCanvas);
            
            if (Bevel)
            {
                Chart chart = Chart as Chart;

                _bevelCanvas = ExtendedGraphics.Get2DRectangleBevel(this, BorderElement.Width - BorderThickness.Left - BorderThickness.Right// - plankDepth
                , BorderElement.Height - BorderThickness.Top - BorderThickness.Bottom //+ ((chart.PlotDetails.ChartOrientation == ChartOrientationType.Horizontal || chart.PlotDetails.ChartOrientation == ChartOrientationType.NoAxis) ? 0 : (-plankDepth - plankThickness))
                , Charts.Chart.BEVEL_DEPTH, Charts.Chart.BEVEL_DEPTH
                , Graphics.GetBevelTopBrush(BorderElement.Background)
                , Graphics.GetBevelSideBrush(0, BorderElement.Background)
                , Graphics.GetBevelSideBrush(180, BorderElement.Background)
                , Graphics.GetBevelSideBrush(90, BorderElement.Background));

                _bevelCanvas.SetValue(Canvas.LeftProperty, BorderThickness.Left);
                _bevelCanvas.SetValue(Canvas.TopProperty, BorderThickness.Top);

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
                        LightingBorder.Background = Graphics.GetFrontFaceBrush(Background);
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
            Chart._drawingCanvas.Children.Remove(InnerShadowGrid);
            Chart chart = Chart as Chart;
            Size clipSize;

            if (chart.PlotArea.ShadowEnabled)
            {
                if (chart.PlotDetails.ChartOrientation == ChartOrientationType.NoAxis)
                {
                    ShadowGrid = ExtendedGraphics.Get2DRectangleShadow(this, BorderElement.Width + Visifire.Charts.Chart.SHADOW_DEPTH
                    , BorderElement.Height + Visifire.Charts.Chart.SHADOW_DEPTH
                    , CornerRadius
                    , CornerRadius, 6);

                    clipSize = new Size(ShadowGrid.Width, ShadowGrid.Height);
                }
                else
                {
                    if (chart.View3D)
                    {
                        if (chart.PlotDetails.ChartOrientation == ChartOrientationType.Horizontal)
                        {
                            ShadowGrid = ExtendedGraphics.Get2DRectangleShadow(this, plotAreaViewPortSize.Width + Visifire.Charts.Chart.SHADOW_DEPTH - plankThickness - plankDepth - ChartArea.SCROLLVIEWER_OFFSET4HORIZONTAL_CHART
                            , plotAreaViewPortSize.Height - plankDepth + Visifire.Charts.Chart.SHADOW_DEPTH
                            , CornerRadius
                            , CornerRadius, 6);
                            clipSize = new Size(ShadowGrid.Width, ShadowGrid.Height + Visifire.Charts.Chart.SHADOW_DEPTH);

                            ShadowGrid.SetValue(Canvas.LeftProperty, plankOffset);

                            InnerShadowGrid = ExtendedGraphics.Get2DRectangleShadow(this, plotAreaViewPortSize.Width + Visifire.Charts.Chart.SHADOW_DEPTH - plankThickness - plankDepth - ChartArea.SCROLLVIEWER_OFFSET4HORIZONTAL_CHART
                           , BorderElement.Height + Visifire.Charts.Chart.SHADOW_DEPTH
                           , CornerRadius
                           , CornerRadius, 6);
                            InnerShadowGrid.Clip = GetShadowClip(new Size(InnerShadowGrid.Width + Visifire.Charts.Chart.SHADOW_DEPTH, InnerShadowGrid.Height));
                            InnerShadowGrid.SetValue(Canvas.LeftProperty, plankOffset);
                            Chart._drawingCanvas.Children.Add(InnerShadowGrid);
                        }
                        else
                        {
                            ShadowGrid = ExtendedGraphics.Get2DRectangleShadow(this, plotAreaViewPortSize.Width + Visifire.Charts.Chart.SHADOW_DEPTH - plankThickness - plankDepth
                                                   , plotAreaViewPortSize.Height + Visifire.Charts.Chart.SHADOW_DEPTH - plankOffset
                                                   , CornerRadius
                                                   , CornerRadius, 6);
                            clipSize = new Size(ShadowGrid.Width, ShadowGrid.Height);
                            ShadowGrid.SetValue(Canvas.LeftProperty, plankOffset);
                        }
                    }
                    else
                    {
                        ShadowGrid = ExtendedGraphics.Get2DRectangleShadow(this, plotAreaViewPortSize.Width + Charts.Chart.SHADOW_DEPTH
                            , plotAreaViewPortSize.Height - plankOffset + Visifire.Charts.Chart.SHADOW_DEPTH, CornerRadius
                            , CornerRadius
                            , 6);

                        clipSize = new Size(ShadowGrid.Width, ShadowGrid.Height);
                    }
                }
                
                ShadowGrid.Clip = GetShadowClip(clipSize);

                ShadowGrid.IsHitTestVisible = false;
                Chart._plotAreaShadowCanvas.Children.Add(ShadowGrid);
                ShadowGrid.UpdateLayout();
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
                Orientation axisOrientation = chart.ChartArea.AxisX.AxisOrientation;
                Double pixelPosition = (axisOrientation == Orientation.Horizontal) ? e.GetPosition(chart.ChartArea.PlottingCanvas).X : e.GetPosition(chart.ChartArea.PlottingCanvas).Y;
                Double lenthInPixel =((axisOrientation == Orientation.Horizontal) ? chart.ChartArea.ChartVisualCanvas.Width : chart.ChartArea.ChartVisualCanvas.Height);

                xValue = chart.ChartArea.AxisX.PixelPositionToValue(lenthInPixel, (axisOrientation == Orientation.Horizontal) ? pixelPosition : lenthInPixel- pixelPosition);

                if (chart.ChartArea.AxisX.IsDateTimeAxis)
                    eventArgs.XValue = DateTimeHelper.XValueToDateTime(chart.ChartArea.AxisX.MinDate, xValue, chart.ChartArea.AxisX.InternalIntervalType);
                else
                    eventArgs.XValue = xValue;
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
                Orientation axisOrientation = chart.ChartArea.AxisX.AxisOrientation;
                Double pixelPosition = (axisOrientation == Orientation.Horizontal) ? e.GetPosition(chart.ChartArea.PlottingCanvas).X : e.GetPosition(chart.ChartArea.PlottingCanvas).Y;
                Double lenthInPixel = ((axisOrientation == Orientation.Horizontal) ? chart.ChartArea.ChartVisualCanvas.Width : chart.ChartArea.ChartVisualCanvas.Height);

                xValue = chart.ChartArea.AxisX.PixelPositionToValue(lenthInPixel, (axisOrientation == Orientation.Horizontal) ? pixelPosition : lenthInPixel - pixelPosition);

                if (chart.ChartArea.AxisX.IsDateTimeAxis)
                    eventArgs.XValue = DateTimeHelper.XValueToDateTime(chart.ChartArea.AxisX.MinDate, xValue, chart.ChartArea.AxisX.InternalIntervalType);
                else
                    eventArgs.XValue = xValue;
            }

            return eventArgs;
        }


        /// <summary>
        /// Fire MouseLeftButtonDown event
        /// </summary>
        /// <param name="e">MouseButtonEventArgs</param>
        internal void FireMouseLeftButtonDownEvent(MouseButtonEventArgs e)
        {   
            if(_onMouseLeftButtonDown != null)
                _onMouseLeftButtonDown(this, CreatePlotAreaMouseButtonEventArgs(e));
        }

        /// <summary>
        /// Fire MouseLeftButtonDown event
        /// </summary>
        /// <param name="e">MouseButtonEventArgs</param>
        internal void FireMouseLeftButtonUpEvent(MouseButtonEventArgs e)
        {
            if (_onMouseLeftButtonUp != null)
                _onMouseLeftButtonUp(this, CreatePlotAreaMouseButtonEventArgs(e));
        }

        /// <summary>
        /// Fire MouseLeftButtonDown event
        /// </summary>
        /// <param name="e">MouseButtonEventArgs</param>
        internal void FireMouseMoveEvent(MouseEventArgs e)
        {
            if (_onMouseMove != null)
                _onMouseMove(this, CreatePlotAreaMouseEventArgs(e));
        }

        /// <summary>
        /// Get MouseLeftButtonDown EventHandler
        /// </summary>
        /// <returns></returns>
        internal EventHandler<PlotAreaMouseButtonEventArgs> GetMouseLeftButtonDownEventHandler()
        {
            return _onMouseLeftButtonDown;
        }

        /// <summary>
        /// Get MouseLeftButtonUp EventHandler
        /// </summary>
        /// <returns></returns>
        internal EventHandler<PlotAreaMouseButtonEventArgs> GetMouseLeftButtonUpEventHandler()
        {
            return _onMouseLeftButtonUp;
        }

        /// <summary>
        /// Get MouseLeftButtonUp EventHandler
        /// </summary>
        /// <returns></returns>
        internal EventHandler<PlotAreaMouseEventArgs> GetMouseMoveEventHandler()
        {
            return _onMouseMove;
        }

        #endregion

        #region Internal Events And Delegates

        /// <summary>
        /// EventChanged event is fired if any event is attached
        /// </summary>
        internal new event EventHandler EventChanged;

        #endregion

        #region Data

        /// <summary>
        /// Handler for MouseLeftButtonDown event
        /// </summary>
        private event EventHandler<PlotAreaMouseButtonEventArgs> _onMouseLeftButtonDown;

        /// <summary>
        /// Handler for MouseLeftButtonUp event
        /// </summary>
        private event EventHandler<PlotAreaMouseButtonEventArgs> _onMouseLeftButtonUp;

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
