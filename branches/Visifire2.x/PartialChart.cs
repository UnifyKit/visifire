#if WPF
using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Markup;
using System.IO;
using System.Xml;
using System.Threading;
using System.Collections.ObjectModel;


#else

using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Collections.ObjectModel;

#endif

using Visifire.Commons;
using System.Windows.Controls.Primitives;

namespace Visifire.Charts
{
    /// <summary>
    /// Partially extended chart control class
    /// </summary>
    public partial class Chart : VisifireControl
    {   
        #region Public Methods

        /// <summary>
        /// On Template applied
        /// </summary>
        public override void OnApplyTemplate()
        {   
            base.OnApplyTemplate();

            #region "Loading TemplateChild"

            _rootElement = GetTemplateChild(RootElementName) as Grid;

            _shadowGrid = GetTemplateChild(ShadowGridName) as Grid;
            _chartBorder = GetTemplateChild(ChartBorderName) as Border;
            _chartLightingBorder = GetTemplateChild(ChartLightingBorderName) as Rectangle;

            _bevelCanvas = GetTemplateChild(BevelCanvasName) as Canvas;

            _chartAreaGrid = GetTemplateChild(ChartAreaGridName) as Grid;

            _topOuterPanel = GetTemplateChild(TopOuterPanelName) as StackPanel;
            _topOuterTitlePanel = GetTemplateChild(TopOuterTitlePanelName) as StackPanel;
            _topOuterLegendPanel = GetTemplateChild(TopOuterLegendPanelName) as StackPanel;

            _bottomOuterPanel = GetTemplateChild(BottomOuterPanelName) as StackPanel;
            _bottomOuterLegendPanel = GetTemplateChild(BottomOuterLegendPanelName) as StackPanel;
            _bottomOuterTitlePanel = GetTemplateChild(BottomOuterTitlePanelName) as StackPanel;

            _leftOuterPanel = GetTemplateChild(LeftOuterPanelName) as StackPanel;
            _leftOuterTitlePanel = GetTemplateChild(LeftOuterTitlePanelName) as StackPanel;
            _leftOuterLegendPanel = GetTemplateChild(LeftOuterLegendPanelName) as StackPanel;

            _rightOuterPanel = GetTemplateChild(RightOuterPanelName) as StackPanel;
            _rightOuterLegendPanel = GetTemplateChild(RightOuterLegendPanelName) as StackPanel;
            _rightOuterTitlePanel = GetTemplateChild(RightOuterTitlePanelName) as StackPanel;

            _centerGrid = GetTemplateChild(CenterGridName) as Grid;
            _centerOuterGrid = GetTemplateChild(CenterOuterGridName) as Grid;

            _topOffsetGrid = GetTemplateChild(TopOffsetGridName) as Grid;
            _bottomOffsetGrid = GetTemplateChild(BottomOffsetGridName) as Grid;
            _leftOffsetGrid = GetTemplateChild(LeftOffsetGridName) as Grid;
            _rightOffsetGrid = GetTemplateChild(RightOffsetGridName) as Grid;

            _topAxisGrid = GetTemplateChild(TopAxisGridName) as Grid;
            _topAxisContainer = GetTemplateChild(TopAxisContainerName) as StackPanel;
            _topAxisPanel = GetTemplateChild(TopAxisPanelName) as StackPanel;
            _topAxisScrollBar = GetTemplateChild(TopAxisScrollBarName) as ScrollBar;

            _leftAxisGrid = GetTemplateChild(LeftAxisGridName) as Grid;
            _leftAxisContainer = GetTemplateChild(LeftAxisContainerName) as StackPanel;
            _leftAxisPanel = GetTemplateChild(LeftAxisPanelName) as StackPanel;
            _leftAxisScrollBar = GetTemplateChild(LeftAxisScrollBarName) as ScrollBar;

            _rightAxisGrid = GetTemplateChild(RightAxisGridName) as Grid;
            _rightAxisContainer = GetTemplateChild(RightAxisContainerName) as StackPanel;
            _rightAxisScrollBar = GetTemplateChild(RightAxisScrollBarName) as ScrollBar;
            _rightAxisPanel = GetTemplateChild(RightAxisPanelName) as StackPanel;

            _bottomAxisGrid = GetTemplateChild(BottomAxisGridName) as Grid;
            _bottomAxisContainer = GetTemplateChild(BottomAxisContainerName) as StackPanel;
            _bottomAxisScrollBar = GetTemplateChild(BottomAxisScrollBarName) as ScrollBar;
            _bottomAxisPanel = GetTemplateChild(BottomAxisPanelName) as StackPanel;

            _centerInnerGrid = GetTemplateChild(CenterInnerGridName) as Grid;

            _innerGrid = GetTemplateChild(InnerGridName) as Grid;

            _topInnerPanel = GetTemplateChild(TopInnerPanelName) as StackPanel;
            _topInnerTitlePanel = GetTemplateChild(TopInnerTitlePanelName) as StackPanel;
            _topInnerLegendPanel = GetTemplateChild(TopInnerLegendPanelName) as StackPanel;

            _bottomInnerPanel = GetTemplateChild(BottomInnerPanelName) as StackPanel;
            _bottomInnerLegendPanel = GetTemplateChild(BottomInnerLegendPanelName) as StackPanel;
            _bottomInnerTitlePanel = GetTemplateChild(BottomInnerTitlePanelName) as StackPanel;

            _leftInnerPanel = GetTemplateChild(LeftInnerPanelName) as StackPanel;
            _leftInnerTitlePanel = GetTemplateChild(LeftInnerTitlePanelName) as StackPanel;
            _leftInnerLegendPanel = GetTemplateChild(LeftInnerLegendPanelName) as StackPanel;

            _rightInnerPanel = GetTemplateChild(RightInnerPanelName) as StackPanel;
            _rightInnerLegendPanel = GetTemplateChild(RightInnerLegendPanelName) as StackPanel;
            _rightInnerTitlePanel = GetTemplateChild(RightInnerTitlePanelName) as StackPanel;

            _plotAreaGrid = GetTemplateChild(PlotAreaGridName) as Grid;
            _plotAreaScrollViewer = GetTemplateChild(PlotAreaScrollViewerName) as ScrollViewer;
            _plotCanvas = GetTemplateChild(PlotGridName) as Canvas;
            _plotAreaShadowCanvas = GetTemplateChild(PlotAreaShadowCanvasName) as Canvas;
            _drawingCanvas = GetTemplateChild(DrawingCanvasName) as Canvas;

            _centerDockInsidePlotAreaPanel = GetTemplateChild(CenterDockInsidePlotAreaPanelName) as StackPanel;
            _centerDockOutsidePlotAreaPanel = GetTemplateChild(CenterDockOutsidePlotAreaPanelName) as StackPanel;

            _toolTipCanvas = GetTemplateChild(ToolTipCanvasName) as Canvas;

            
            if (ToolTips.Count == 0)
            {
                ToolTip toolTip = new ToolTip() { Chart = this, Visibility = Visibility.Collapsed };
                ToolTips.Add(toolTip);
            }
            

            _toolTip = ToolTips[0];
            _toolTip.Chart = this;

            //_toolTip.OnSizeChanged += delegate(object sender, MouseEventArgs e)
            //{
            //     //UpdateToolTipPosition(this, e);
            //};

            _toolTipCanvas.Children.Add(_toolTip);

            #endregion       

            if (ShadowEnabled)
            {
                _chartBorder.Margin = new Thickness(0, 0, SHADOW_DEPTH, SHADOW_DEPTH);
                _bevelCanvas.Margin = new Thickness(0, 0, SHADOW_DEPTH, SHADOW_DEPTH);
            }
            
            _chartAreaOriginalMargin = new Thickness(_chartAreaGrid.Margin.Left, _chartAreaGrid.Margin.Top, _chartAreaGrid.Margin.Right, _chartAreaGrid.Margin.Bottom);

            if (Bevel)
            {   
                _chartAreaGrid.Margin = new Thickness(
                    _chartAreaOriginalMargin.Left + BEVEL_DEPTH,
                    _chartAreaOriginalMargin.Top + BEVEL_DEPTH,
                    _chartAreaOriginalMargin.Right + BEVEL_DEPTH,
                    _chartAreaOriginalMargin.Bottom + BEVEL_DEPTH);
            }
            
            ApplyLighting();
            
            SetEventsToToolTipObject();

            LoadWatermark();

            if(StyleDictionary == null)
                LoadTheme("Theme1");

            LoadColorSets();
            
            IsTemplateApplied = true;

            if (!String.IsNullOrEmpty(ToolTipText))
                AttachToolTip(this,this, this);
                        
            AttachEvents2Visual(this, this, this._rootElement);

            AttachEvents2Visual4MouseDownEvent(this, this, this._plotCanvas);

           
/*          
            this.EventChanged += delegate
            {
                 AttachEvents2Visual(this, this, this._rootElement);
#if WPF
                 AttachEvents2Visual(this, this, this._plotCanvas);
#endif
            };
*/
            _internalAnimationEnabled = AnimationEnabled;

            if (_internalAnimationEnabled)
                _rootElement.IsHitTestVisible = false;

        }

        internal override void OnToolTipTextPropertyChanged(string NewValue)
        {   
            base.OnToolTipTextPropertyChanged(NewValue);
            DetachToolTip(this._toolTipCanvas);

            if (!String.IsNullOrEmpty(NewValue))
                AttachToolTip(this, this, this);

        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Bevel canvas as Bevel Visual
        /// </summary>
        internal Canvas BevelVisual
        {
            get;
            set;
        }

        /// <summary>
        /// MinimumGap between two DataPoint in Plotarea
        /// </summary>
        internal Int32 FixedDataPoints
        {   
            get
            {
                return (Int32)GetValue(FixedDataPointsProperty);
            }
            set
            {
                SetValue(FixedDataPointsProperty, value);
            }
        }

        private static readonly DependencyProperty FixedDataPointsProperty = DependencyProperty.Register
            ("FixedDataPoints",
            typeof(Int32),
            typeof(Chart),
            new PropertyMetadata(OnFixedDataPointsChanged));

        private static void OnFixedDataPointsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {   
            Chart c = d as Chart;
            c.CallRender();
        }

        /// <summary>
        /// MinimumGap between two DataPoint in Plotarea
        /// </summary>
#if SL
        [System.ComponentModel.TypeConverter(typeof(Converters.NullableDoubleConverter))]
#endif
        public Nullable<Double> MinimumGap
        {   
            get
            {   
                if ((Nullable<Double>)GetValue(MinimumGapProperty) == null)
                    return 30;
                else
                    return (Nullable<Double>)GetValue(MinimumGapProperty);
            }
            set
            {   
                SetValue(MinimumGapProperty, value);
            }
        }

        public static readonly DependencyProperty MinimumGapProperty = DependencyProperty.Register
            ("MinimumGap",
            typeof(Nullable<Double>),
            typeof(Chart),
            new PropertyMetadata(OnMinimumGapPropertyChanged));

        private static void OnMinimumGapPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Chart c = d as Chart;
            c.CallRender();
        }
        
        /// <summary>
        /// Enable or disable 3D effect
        /// </summary>
#if SL
        [System.ComponentModel.TypeConverter(typeof(NullableBoolConverter))]
#endif
        public Nullable<Boolean> ScrollingEnabled
        {
            set
            {
                SetValue(ScrollingEnabledProperty, value);
            }
            get
            {
                if ((Nullable<Boolean>)GetValue(ScrollingEnabledProperty) == null)
                {
                    return true;
                }
                else
                    return (Nullable<Boolean>)GetValue(ScrollingEnabledProperty);
            }
        }

        public static readonly DependencyProperty ScrollingEnabledProperty = DependencyProperty.Register
            ("ScrollingEnabled",
            typeof(Nullable<Boolean>),
            typeof(Chart),
            new PropertyMetadata(OnScrollingEnabledPropertyChanged));

        /// <summary>
        /// Event handler view3DProperty changed event
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnScrollingEnabledPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Chart c = d as Chart;
            c.CallRender();  
        }

        /// <summary>
        /// Enable or disable 3D effect
        /// </summary>
        public Boolean View3D
        {
            set
            {
                SetValue(View3DProperty, value);
            }
            get
            {
                return (Boolean)GetValue(View3DProperty);
            }
        }

        public static readonly DependencyProperty View3DProperty = DependencyProperty.Register
            ("View3D",
            typeof(Boolean),
            typeof(Chart),
            new PropertyMetadata(OnView3DPropertyChanged));

        /// <summary>
        /// Event handler view3DProperty changed event
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnView3DPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Chart c = d as Chart;
            c.CallRender();
        }

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

        public static readonly DependencyProperty HrefTargetProperty = DependencyProperty.Register
            ("HrefTarget",
            typeof(HrefTargets),
            typeof(Chart),
            new PropertyMetadata(OnHrefTargetChanged));

        private static void OnHrefTargetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Chart c = d as Chart;
            ObservableObject.AttachHref(c, c, c.Href, (HrefTargets)e.NewValue);
        }

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

        public static readonly DependencyProperty HrefProperty = DependencyProperty.Register
            ("Href",
            typeof(String),
            typeof(Chart),
            new PropertyMetadata(OnHrefChanged));

        private static void OnHrefChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Chart c = d as Chart;
            ObservableObject.AttachHref(c, c, e.NewValue.ToString(), c.HrefTarget);
        }

        public String Theme
        {
            get
            {
                return (String)GetValue(ThemeProperty);
            }
            set
            {
                SetValue(ThemeProperty, value);
            }
        }

        public static readonly DependencyProperty ThemeProperty = DependencyProperty.Register
            ("Theme",
            typeof(String),
            typeof(Chart),
            new PropertyMetadata(OnThemePropertyChanged));
            
        private static void OnThemePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Chart c = d as Chart;
            c.LoadTheme((String)e.NewValue);
            c.CallRender();
        }
        
        internal void LoadTheme(String themeName)
        {
            if (String.IsNullOrEmpty(themeName))
                return;
            
            StyleDictionary = null;

            string fooResourceName = "Visifire.Charts." + themeName + ".xaml";

            using (System.IO.Stream s = typeof(Chart).Assembly.GetManifestResourceStream(fooResourceName))
            {   
                if (s == null)
                {
#if WPF             
                    ResourceDictionary resource = (ResourceDictionary)Application.LoadComponent(new Uri(@"" + themeName + ".xaml", UriKind.RelativeOrAbsolute));
                    StyleDictionary = resource;
#else
                    fooResourceName = themeName + ".xaml";
                    System.Windows.Resources.StreamResourceInfo srif = Application.GetResourceStream(new Uri(fooResourceName, UriKind.RelativeOrAbsolute));

                    if (srif == null)
                        throw new Exception(fooResourceName + " Theme file not found as application resources.");
                    
                    using (System.IO.Stream s1 = srif.Stream)
                    {
                        if (s1 != null)
                        {
                            System.IO.StreamReader reader = new System.IO.StreamReader(s1);
                            String xaml = reader.ReadToEnd();
                            StyleDictionary = System.Windows.Markup.XamlReader.Load(xaml) as ResourceDictionary;
                            reader.Close();
                            s1.Close();
                        }
                    }
#endif
                }
                else
                {
                    System.IO.StreamReader reader = new System.IO.StreamReader(s);
                    String xaml = reader.ReadToEnd();
#if WPF
                    StyleDictionary  = (ResourceDictionary)XamlReader.Load(new XmlTextReader(new StringReader(xaml)));
#else
                    StyleDictionary = System.Windows.Markup.XamlReader.Load(xaml) as ResourceDictionary;
#endif
                    reader.Close();
                    s.Close();
                }
            }

            if (StyleDictionary != null)
            {
                if (Style == null)
                {
                    Style myStyle = StyleDictionary["Chart"] as Style;

                    if (myStyle != null)
                        Style = myStyle;
                }
            }
            else
            {
                throw new Exception("Theme file " + themeName + ".xaml not found..");
            }
        }

        #region AnimationProperty

        /// <summary>
        /// Enabled or disables animation
        /// </summary>
        public Boolean AnimationEnabled
        {
            get
            {   
                if (!IsInDesignMode)
                {   
                    if (String.IsNullOrEmpty(AnimationEnabledProperty.ToString()))
                    {
                        return false;
                    }
                    else
                        return (Boolean)GetValue(AnimationEnabledProperty);
                }
                else
                    return false;
            }
            set
            {
                SetValue(AnimationEnabledProperty, value);
            }
        }

        public static readonly DependencyProperty AnimationEnabledProperty = DependencyProperty.Register
            ("AnimationEnabled",
            typeof(Boolean),
            typeof(Chart),
            null);

        /// <summary>
        /// Set the type of animation
        /// </summary>
        internal AnimationType AnimationType
        {
            get
            {
                if (GetValue(AnimationTypeProperty) == null)
                {
                    return AnimationType.Type1;
                }
                else
                    return (AnimationType)GetValue(AnimationTypeProperty);
            }
            set
            {
                SetValue(AnimationTypeProperty, value);
            }
        }

        public static readonly DependencyProperty AnimationTypeProperty = DependencyProperty.Register
            ("AnimationType",
            typeof(AnimationType),
            typeof(Chart),
            null);

        /// <summary>
        /// Sets the duration for animation
        /// </summary>
        internal Double AnimationDuration
        {
            get
            {

                return ((Double)GetValue(AnimationDurationProperty) == 0) ? 1 : Math.Abs((Double)GetValue(AnimationDurationProperty));
            }
            set
            {
                SetValue(AnimationDurationProperty, value);
            }
        }

        public static readonly DependencyProperty AnimationDurationProperty = DependencyProperty.Register
            ("AnimationDuration",
            typeof(Double),
            typeof(Chart),
            null);

        #endregion

        #region BorderProperties

        public new Thickness BorderThickness
        {
            get
            {
                return (Thickness)GetValue(BorderThicknessProperty);
            }
            set
            {
                SetValue(BorderThicknessProperty, value);
                SetValue(InternalBorderThicknessProperty, value);
            }
        }

        public static readonly DependencyProperty InternalBorderThicknessProperty = DependencyProperty.Register
           ("InternalBorderThickness",
           typeof(Thickness),
           typeof(Chart),
           new PropertyMetadata(OnInternalBorderThicknessChanged));

        private static void OnInternalBorderThicknessChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Chart c = d as Chart;

            c.RemoveChartBevel();
            c.CallRender();

            //if (c.ApplyChartBevel())
            //{
            //    if ((Boolean)c.Bevel == true)
            //    {
            //        c.CallRender();
            //    }
            //    else
            //    {
            //        c.RemoveChartBevel();
            //    }
            //}
        }

        /// <summary>
        /// Sets the border line style
        /// </summary>
        public BorderStyles BorderStyle
        {
            get
            {

                return (BorderStyles)GetValue(BorderStyleProperty);
            }
            set
            {
                SetValue(BorderStyleProperty, value);
            }
        }

        public static readonly DependencyProperty BorderStyleProperty = DependencyProperty.Register
            ("BorderStyle",
            typeof(BorderStyles),
            typeof(Chart),
            null);

        #endregion

        public new Brush Background
        {   
            get
            {   
                return (Brush)GetValue(BackgroundProperty);
            }
            set
            {
                SetValue(BackgroundProperty, value);
                SetValue(InternalBackgroundProperty, value);
            }
        }

        public static readonly DependencyProperty InternalBackgroundProperty = DependencyProperty.Register
          ("InternalBackground",
          typeof(Brush),
          typeof(Chart),
          new PropertyMetadata(OnInternalBackgroundPropertyChanged));


        private static void OnInternalBackgroundPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {   
            Chart c = d as Chart;
            //c._chartBorder.Background = (Brush) e.NewValue;
            c.RemoveChartBevel();
            c.CallRender();

            if (c.ApplyChartBevel())
            {
                if ((Boolean)c.Bevel == true)
                {
                    c.CallRender();
                }
                else
                {
                    c.RemoveChartBevel();
                }
            }

            //c.CallRender();
        }

        private static void OnInternalPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Chart c = d as Chart;
            //c.RemoveChartBevel();
            //c.CallRender();

            if (c.ApplyChartBevel())
            {
                if ((Boolean)c.Bevel == true)
                {
                    c.CallRender();
                }
                else
                {
                    c.RemoveChartBevel();
                }
            }
        }

        /// <summary>
        /// Enabled or disables the bevel effect
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

        public static readonly DependencyProperty BevelProperty = DependencyProperty.Register
            ("Bevel",
            typeof(Boolean),
            typeof(Chart),
            new PropertyMetadata(OnBevelPropertyChanged));

        private static void OnBevelPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Chart c = d as Chart;
            c.RemoveChartBevel();
            c.CallRender();

            //c.ApplyChartBevel();

            //if ((Boolean)e.NewValue == true)
            //{
            //    c.CallRender();
            //}
            //else
            //{
                
            //}
        }

        /// <summary>
        /// Set of colors that will be used for the DataPoints
        /// </summary>
        public String ColorSet
        {
            get
            {
                return (String)GetValue(ColorSetProperty);
            }
            set
            {
                SetValue(ColorSetProperty, value);
            }
        }

        public static readonly DependencyProperty ColorSetProperty = DependencyProperty.Register
            ("ColorSet",
            typeof(String),
            typeof(Chart),
            new PropertyMetadata(OnColorSetPropertyChanged));

        private static void OnColorSetPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Chart c = d as Chart;
            c.CallRender();
        }

        /// <summary>
        /// Set of colors that will be used for the DataPoints
        /// </summary>
        public ColorSets ColorSets
        {
            get
            {
                return (ColorSets)GetValue(ColorSetsProperty);
            }
            set
            {
                SetValue(ColorSetsProperty, value);
            }
        }

        public static readonly DependencyProperty ColorSetsProperty = DependencyProperty.Register
            ("ColorSets",
            typeof(ColorSets),
            typeof(Chart),
            null);

        /// <summary>
        /// Set of colors that will be used for the DataPoints
        /// </summary>
        internal ColorSets InternalColorSets
        {
            get;
            set;
        }

        /// <summary>
        /// Enabled or disabled automatic color shading
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

        public static readonly DependencyProperty LightingEnabledProperty = DependencyProperty.Register
            ("LightingEnabled",
            typeof(Boolean),
            typeof(Chart),
            new PropertyMetadata(OnLightingEnabledPropertyChanged));

        private static void OnLightingEnabledPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Chart c = d as Chart;
            c.ApplyLighting();
        }

        /// <summary>
        /// Sets the parameter used to create rounded or elliptical corners of the element
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

        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(Chart), null);

        /// <summary>
        /// Enables or disables the shadow for the element
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

        public static readonly DependencyProperty ShadowEnabledProperty = DependencyProperty.Register("ShadowEnabled", typeof(Boolean), typeof(Chart), new PropertyMetadata(OnShadowEnabledPropertyChanged));

        private static void OnShadowEnabledPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Chart c = d as Chart;

            if ((Boolean)e.NewValue == true)
            {
                c.ApplyChartShadow(c.ActualHeight, c.ActualWidth);
            }
            else
            {
                c.RemoveShadow();
            }

            c.ApplyChartBevel();
        }
   
        /// <summary>
        /// Enables or disables the "Powered by Visifire" watermark
        /// </summary>
        public Boolean Watermark
        {
            get
            {
                return (Boolean)GetValue(WatermarkProperty);
            }
            set
            {
                SetValue(WatermarkProperty, value);
            }
        }

        public static readonly DependencyProperty WatermarkProperty = DependencyProperty.Register("Watermark", typeof(Boolean), typeof(Chart), new PropertyMetadata(OnWatermarkPropertyChanged));

        private static void OnWatermarkPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Chart c = d as Chart;

            if (c.WaterMarkElement != null)
                c.WaterMarkElement.Visibility = ((Boolean)e.NewValue) ? Visibility.Visible : Visibility.Collapsed;
        }

        #region Titles Property

        /// <summary>
        /// AxesX as Observable collection of type Axis
        /// </summary>
        public AxisCollection AxesX
        {
            get;
            set;
        }

        /// <summary>
        /// AxesX as Observable collection of type Axis
        /// </summary>
        internal IList<Axis> InternalAxesX
        {
            get;
            set;
        }

        /// <summary>
        /// AxesY as Observable collection of type Axis
        /// </summary>
        public AxisCollection AxesY
        {
            get;
            set;
        }

        /// <summary>
        /// AxesY as Observable collection of type Axis
        /// </summary>
        internal IList<Axis> InternalAxesY
        {
            get;
            set;
        }
        
        /// <summary>
        /// Titles as Observable collection of type Title
        /// </summary>
        public TitleCollection Titles
        {
            get;
            set;
        }

        public LegendCollection Legends
        {
            get;
            set;
        }

        public ToolTipCollection ToolTips
        {
            get;
            set;
        }

        public TrendLineCollection TrendLines
        {
            get;
            set;
        }
        #endregion



        public PlotArea PlotArea
        {
            get
            {
                return (PlotArea)GetValue(PlotAreaProperty);
            }
            set
            {   
                SetValue(PlotAreaProperty, value);
                PlotArea.Chart = this;
                PlotArea.PropertyChanged -= PlotArea_PropertyChanged;
                PlotArea.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(PlotArea_PropertyChanged);
            }
        }

        public static readonly DependencyProperty PlotAreaProperty = DependencyProperty.Register
        ("PlotArea",
        typeof(PlotArea),
        typeof(Chart),
        new PropertyMetadata(OnPlotAreaPropertyChanged));

        private static void OnPlotAreaPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Chart chart = d as Chart;
            PlotArea plotArea = (PlotArea) e.NewValue;
            plotArea.Chart = chart;
            plotArea.PropertyChanged -= chart.PlotArea_PropertyChanged;
            plotArea.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(chart.PlotArea_PropertyChanged);
     
        }
        
        void PlotArea_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            CallRender();
        }

        #region Series Property

        /// <summary>
        /// Collection of DataSeries
        /// </summary>
#if SL
        [System.Windows.Browser.ScriptableMember]
#endif
        internal List<DataSeries> InternalSeries
        {
            get;
            set;
        }

        /// <summary>
        /// Collection of DataSeries
        /// </summary>
        public ObservableCollection<DataSeries> Series
        {
            get;
            set;
        }

        #endregion

        ///// <summary>
        ///// ToolTip text property for the Chart
        ///// </summary>
        //public String ToolTipText
        //{
        //    get
        //    {
        //        return (String)GetValue(ToolTipTextProperty);
        //    }
        //    set
        //    {
        //        SetValue(ToolTipTextProperty, value);
        //    }
        //}

        //public static readonly DependencyProperty ToolTipTextProperty = DependencyProperty.Register
        //    ("ToolTipText",
        //    typeof(String),
        //    typeof(Chart),
        //    null);

        #endregion

        #region Public Events

        #endregion

        #region Protected Methods

        #endregion

        #region Internal Properties

        internal PlotDetails PlotDetails
        {
            get;
            set;
        }

        /// <summary>
        /// Currently visible ChartArea
        /// </summary>
        internal ChartArea ChartArea
        {
            get;
            set;
        }

        #endregion

        #region Private Delegates

        #endregion

        #region Private Methods

        /// <summary>
        /// Removes Chart shadow
        /// </summary>
        private void RemoveShadow()
        {
            if (IsTemplateApplied && !ShadowEnabled)
            {
                _chartBorder.Margin = new Thickness(0, 0, 0, 0);
                _bevelCanvas.Margin = new Thickness(0, 0, 0, 0);
                IsShadowApplied = false;
            }
        }

        private void LoadWatermark()
        {
            if (Watermark)
            {
                WaterMarkElement = new TextBlock();
                WaterMarkElement.HorizontalAlignment = HorizontalAlignment.Right;
                WaterMarkElement.VerticalAlignment = VerticalAlignment.Top;
                WaterMarkElement.Margin = new Thickness(0, 3, 5, 0);
                WaterMarkElement.SetValue(Canvas.ZIndexProperty, 90000);
                WaterMarkElement.Text = "www.visifire.com";
                WaterMarkElement.TextDecorations = TextDecorations.Underline;
                WaterMarkElement.Foreground = new SolidColorBrush(Colors.LightGray);
                WaterMarkElement.FontSize = 9;
                ObservableObject.AttachHref(this, WaterMarkElement, "http://www.visifire.com", HrefTargets._blank);
                _rootElement.Children.Add(WaterMarkElement);
            }
        }

        private void LoadColorSets()
        {
            string fooResourceName = "Visifire.Charts.ColorSets.xaml";
            ColorSets embeddedColorSets;

            using (System.IO.Stream s = typeof(Chart).Assembly.GetManifestResourceStream(fooResourceName))
            {   
                if (s != null)
                {   
                    System.IO.StreamReader reader = new System.IO.StreamReader(s);

                    String xaml = reader.ReadToEnd();
#if WPF
                    embeddedColorSets = XamlReader.Load(new XmlTextReader(new StringReader(xaml))) as ColorSets;
#else
                    embeddedColorSets = System.Windows.Markup.XamlReader.Load(xaml) as ColorSets;
#endif

                    if (embeddedColorSets == null)
                        System.Diagnostics.Debug.WriteLine("Unable to load embedded ColorSets. Reload project and try again.");

                    if (InternalColorSets == null)
                        InternalColorSets = new ColorSets();
                    
                    if (embeddedColorSets != null)
                        InternalColorSets.AddRange(embeddedColorSets);

                    if(ColorSets != null)
                        foreach (ColorSet colorSet in ColorSets)
                            InternalColorSets.Add(colorSet);
                        
                    reader.Close();
                    s.Close();
                }
            }
        }

        private void SetEventsToToolTipObject()
        {
            this._rootElement.MouseLeave += new MouseEventHandler(Chart_MouseLeave);
        }


        void Chart_MouseLeave(object sender, MouseEventArgs e)
        {
            if (ToolTipEnabled)
            {
                if (_toolTip.Visibility == Visibility.Visible)
                    _toolTip.Hide();
            }
            else
            {
                _toolTip.Hide();
            }
            _toolTip.Text = "";
        }

        Point MousePosition
        {
            get;
            set;
        }

        internal void UpdateToolTipPosition(object sender, MouseEventArgs e)
        {
            
            //if (e == null && _mouseEventArgs == null)
            //    return;
            //Chart_MouseMoveSetToolTipPosition(sender, (e == null) ? _mouseEventArgs : e);

            Chart_MouseMoveSetToolTipPosition(sender, e);
        }

        /// <summary>
        /// OnMouseMoveOver chart set ToolTip position
        /// ToolTip at current mouse position over the chart control        
        /// </summary>
        /// <param name="sender">Chart as object</param>
        /// <param name="e">MouseEventArgs</param>
        private void Chart_MouseMoveSetToolTipPosition(object sender, MouseEventArgs e)
        {
            // _mouseEventArgs = e;

            if (ToolTipEnabled && (Boolean)_toolTip.Enabled)
            {   
                Double x = e.GetPosition(this).X;
                Double y = e.GetPosition(this).Y;

                #region Set position of ToolTip
                _toolTip.Measure(new Size(Double.MaxValue, Double.MaxValue));
                _toolTip.UpdateLayout();

                Size size = Visifire.Commons.Graphics.CalculateVisualSize(_toolTip._borderElement);

                //System.Diagnostics.Debug.WriteLine("Size :" + size.ToString());

                Double toolTipWidth = size.Width;
                Double toolTipHeight = size.Height;

                y = y - (toolTipHeight + 5);

                x = x - toolTipWidth / 2;

                    if (x <= 0)
                    {
                        x = e.GetPosition(this).X + 10;
                        y = e.GetPosition(this).Y + 20;

                        if ((y + toolTipHeight) >= this.ActualHeight)
                            y = this.ActualHeight - toolTipHeight;
                    }
                    
                    if ((x + toolTipWidth) >= this.ActualWidth)
                    {
                        x = e.GetPosition(this).X - toolTipWidth;
                        y = e.GetPosition(this).Y - toolTipHeight;
                    }

                    if (y < 0)
                        y = e.GetPosition(this).Y + 20;

                    if (x + toolTipWidth > this.ActualWidth)
                        x = x + toolTipWidth - this.ActualWidth;
                    
                    
                    if (toolTipWidth == _toolTip.MaxWidth)
                        x = 0;
                    
                    if (x < 0)
                    {
                        x = 0;
                    }

               

                _toolTip.SetValue(Canvas.LeftProperty, x);

                _toolTip.SetValue(Canvas.TopProperty, y);

                Double left = (Double)_toolTip.GetValue(Canvas.LeftProperty);
                //System.Diagnostics.Debug.WriteLine("LEft =" + left.ToString());


                #endregion
            }
            else
            {
                _toolTip.Hide();
            }
        }

        /// <summary>
        /// Event handler manages the addition and removal of title from chart
        /// </summary>
        private void Titles_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                ObservableCollection<Title> titles = sender as ObservableCollection<Title>;

                if (e.NewItems != null)
                {
                    foreach (Title title in e.NewItems)
                    {
                        title.Chart = this;
                        title.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(Title_PropertyChanged);
                    }
                }
            }

            CallRender();

            //else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            //{
            //    CallRender();
            //}
        }

        void Title_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            CallRender();
        }

        void Legends_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (Legend legend in e.NewItems)
                {
                    if (Legends.Count > 0)
                    {   
                        if (String.IsNullOrEmpty((String)legend.GetValue(NameProperty)))
                            legend.SetValue(NameProperty, "Legend" + Legends.IndexOf(legend));
                    }

                    legend.Chart = this;
                    legend.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(legend_PropertyChanged);
                }

            }

            if (IsRenderCallAllowed)
                CallRender();

            //else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            //{
            //    if(IsRenderCallAllowed)
            //        CallRender();
            //}
        }

        void legend_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            CallRender();
        }

        void TrendLines_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                if (e.NewItems != null)
                {   
                    foreach (TrendLine trendLine in e.NewItems)
                    {
                        trendLine.Chart = this;

                        if (!String.IsNullOrEmpty(this.Theme))
                        {   
                            trendLine.ApplyStyleFromTheme(this, "TrendLine");
                        }

                        trendLine.PropertyChanged -= trendLine_PropertyChanged;
                        trendLine.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(trendLine_PropertyChanged);
                    }
                }

            }

            CallRender();

            //else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            //{
            //    CallRender();
            //}
        }

        void trendLine_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            CallRender();
        }

        /// <summary>
        /// Event handler manages the addition and removal of DataSeries from Series list of chart
        /// </summary>
        void Series_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                if (e.NewItems != null)
                    foreach (DataSeries ds in e.NewItems)
                    {
                        ds.Chart = this;

                        foreach (DataPoint dp in ds.DataPoints)
                        {
                            dp.Chart = this;
                        }

                        if (!String.IsNullOrEmpty(this.Theme))
                            ds.ApplyStyleFromTheme(this, "DataSeries");

                        ds.SetValue(NameProperty, ds.GetType().Name + this.Series.IndexOf(ds));
                        ds.PropertyChanged -= Series_PropertyChanged;
                        ds.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(Series_PropertyChanged);
                    }
                 
            }

            CallRender();

            //}
            //else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            //{
            //    CallRender();
            //}
            //else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Reset)
            //{   
            //    //if (ChartArea != null)
            //    //    ChartArea.ClearPlotAreaChildren();
            //    CallRender();
            //}
        }

        void Series_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            CallRender();
        }

        /// <summary>
        /// Event handler manages the addition and removal of Axis from AxesY list of chart
        /// </summary>
        void AxesY_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Boolean isAutoAxis = false;

            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                if (e.NewItems != null)
                {   
                    foreach (Axis axis in e.NewItems)
                    {
                        axis.Chart = this;

                        if (axis.Tag != null && axis.Tag.ToString() == "AutoAxis")
                            isAutoAxis = true;

                        axis.AxisRepresentation = AxisRepresentations.AxisY;

                        if (axis.AxisLabels != null)
                            axis.AxisLabels.Chart = this;

                        foreach (ChartGrid cg in axis.Grids)
                            cg.Chart = this;

                        foreach (Ticks ticks in axis.Ticks)
                            ticks.Chart = this;
                        
                        axis.IsNotificationEnable = false;

                        if (!String.IsNullOrEmpty(this.Theme))
                        {
                            axis.ApplyStyleFromTheme(this, "AxisY");
                        }

                        if(axis.StartFromZero == null) 
                            axis.StartFromZero = true;

                        axis.IsNotificationEnable = true;

                        axis.PropertyChanged -= AxesY_PropertyChanged;
                        axis.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(AxesY_PropertyChanged);
                    }
                }
            }

            if (!isAutoAxis)
            {
                CallRender();
            }

            //else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            //{
            //    if (!isAutoAxis)
            //    {
            //        CallRender();
            //    }
            //}
        }

        void AxesY_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            CallRender();
        }

        /// <summary>
        /// Event handler manages the addition and removal of Axis from AxesX list of chart
        /// </summary>
        void AxesX_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Boolean isAutoAxis = false;
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                if (e.NewItems != null)
                {   
                    foreach (Axis axis in e.NewItems)
                    {
                        axis.Chart = this;

                        if (axis.Tag != null && axis.Tag.ToString() == "AutoAxis")
                            isAutoAxis = true;

                        if (axis.AxisLabels != null)
                            axis.AxisLabels.Chart = this;

                        foreach (ChartGrid cg in axis.Grids)
                            cg.Chart = this;

                        foreach (Ticks ticks in axis.Ticks)
                            ticks.Chart = this;

                        axis.IsNotificationEnable = false;

                        if (!String.IsNullOrEmpty(this.Theme))
                        {
                            axis.ApplyStyleFromTheme(this, "AxisX");                           
                        }
                        
                        if (axis.StartFromZero == null)
                            axis.StartFromZero = false;

                        axis.IsNotificationEnable = true;

                        axis.PropertyChanged -= AxesX_PropertyChanged;
                        axis.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(AxesX_PropertyChanged);
                    }
                }
            }

            if (!isAutoAxis)
            {
                CallRender();
            }

            //else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            //{
            //    if (!isAutoAxis)
            //    {
            //        CallRender();
            //    }
            //}
        }

        void AxesX_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            CallRender();
        }

        void PropertyBindingBeforeFirstTimeRender()
        {
            System.Windows.Data.Binding binding = new System.Windows.Data.Binding("Background");
            binding.Source = this;
            binding.Mode = System.Windows.Data.BindingMode.OneWay;
            base.SetBinding(InternalBackgroundProperty, binding);

            System.Windows.Data.Binding binding1 = new System.Windows.Data.Binding("BorderThickness");
            binding1.Source = this;
            binding1.Mode = System.Windows.Data.BindingMode.OneWay;
            base.SetBinding(InternalBorderThicknessProperty, binding1);
        }

        /// <summary>
        /// Create visual tree before rendering
        /// </summary>
        /// <returns></returns>
        private ChartArea CreateVisualTree()
        {

            // Create new ChartArea
            ChartArea chartArea = new ChartArea(this as Chart);
            
            chartArea.PropertyChanged += new EventHandler(ChartArea_PropertyChanged);

            PropertyBindingBeforeFirstTimeRender();

            return chartArea;
        }
        
        void ChartArea_PropertyChanged(object sender, EventArgs e)
        {
            CallRender();
        }

        /// <summary>
        /// Render() replace the existing Chart with a new Chart
        /// </summary>
        internal void Render()
        {   
            if (IsTemplateApplied)
            {   
                if ((!RENDER_LOCK ) && _rootElement != null)    
                {
                    System.Diagnostics.Debug.WriteLine("Render______");

                    if (Double.IsNaN(this.ActualWidth) || Double.IsNaN(this.ActualHeight) || this.ActualWidth == 0 || this.ActualHeight == 0)
                        return;                        

                    RENDER_LOCK = true;

                    try
                    {
                        if(ChartArea == null)
                            ChartArea = CreateVisualTree();

                        ApplyChartBevel();
                        
                        ApplyChartShadow(this.ActualHeight, this.ActualWidth);

                        _leftAxisPanel.Children.Clear();
                        _bottomAxisPanel.Children.Clear();
                        _topAxisPanel.Children.Clear();
                        _rightAxisPanel.Children.Clear();

                        _renderLapsedCounter = 0;
                        ChartArea.Draw(this);

                        // System.Diagnostics.Debug.WriteLine("Debug End");

                    }
                    catch (Exception e)
                    {    
                         RENDER_LOCK = false;
                         throw new Exception(e.Message, e);
                    }
                }
            }
        }

        #endregion

        #region Private Properties

        private TextBlock WaterMarkElement
        {
            get;
            set;
        }

        // Hides the Foreground property in control
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        private new Brush Foreground
        {
            get;
            set;
        }

        #endregion

        #region Internal Property

        internal Boolean IsShadowApplied
        {
            get;
            set;
        }

        internal ResourceDictionary StyleDictionary
        {
            get;
            set;
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Apply Bevel effect to Chart
        /// </summary>
        /// <returns>If success return true else false.</returns>
        internal Boolean ApplyChartBevel()
        {
            if (Bevel && _rootElement != null)
            {
                if (_chartBorder.ActualWidth == 0 && _chartBorder.ActualHeight == 0)
                {
                    return false;
                }

                _chartAreaGrid.Margin = new Thickness(
                    _chartAreaOriginalMargin.Left + BEVEL_DEPTH,
                    _chartAreaOriginalMargin.Top + BEVEL_DEPTH,
                    _chartAreaOriginalMargin.Right,
                    _chartAreaOriginalMargin.Bottom);
                _chartAreaGrid.UpdateLayout();

                _bevelCanvas.Children.Clear();

                Brush topBrush = Graphics.GetBevelTopBrush(this.Background);
                Brush leftBrush = Graphics.GetBevelSideBrush(0, this.Background);
                Brush rightBrush = Graphics.GetBevelSideBrush(170, this.Background);
                Brush bottomBrush = Graphics.GetBevelSideBrush(180, this.Background);

                BevelVisual = ExtendedGraphics.Get2DRectangleBevel(
                    _chartBorder.ActualWidth - _chartBorder.BorderThickness.Left - _chartBorder.BorderThickness.Right - _chartAreaOriginalMargin.Right - _chartAreaOriginalMargin.Left,
                    _chartBorder.ActualHeight - _chartBorder.BorderThickness.Top - _chartBorder.BorderThickness.Bottom - _chartAreaOriginalMargin.Top - _chartAreaOriginalMargin.Bottom,
                BEVEL_DEPTH, BEVEL_DEPTH, topBrush, leftBrush, rightBrush, bottomBrush);

                if (LightingEnabled)
                {   
                    _chartLightingBorder.Opacity = 0.4;
                }

                BevelVisual.Margin = new Thickness(0, 0, 0, 0);
                BevelVisual.IsHitTestVisible = false;
                BevelVisual.SetValue(Canvas.TopProperty, _chartAreaOriginalMargin.Top + _chartBorder.BorderThickness.Top);
                BevelVisual.SetValue(Canvas.LeftProperty, _chartAreaOriginalMargin.Left + _chartBorder.BorderThickness.Left);
                _bevelCanvas.Children.Add(BevelVisual);
            }
            // else
            //    _chartAreaGrid.Margin = new Thickness(0);

            return true;
        }

        internal void RemoveChartBevel()
        {
            if (IsTemplateApplied)
            {
                if (BevelVisual != null && _bevelCanvas != null)
                {
                    _bevelCanvas.Children.Clear();
                    _chartAreaGrid.Margin = new Thickness(0);
                }
            }
        }

        internal Grid ChartShadowGrid
        {
            get;
            set;
        }

        internal void ApplyChartShadow(Double Height, Double Width)
        {
            if (!IsShadowApplied && ShadowEnabled && !Double.IsNaN(Height) && Height != 0 && !Double.IsNaN(Width) && Width != 0)
            {
                _shadowGrid.Children.Clear();

                if (_rootElement != null)
                {
                    // Shadow grid contains multiple rectangles that give a blurred effect at the edges 
                    ChartShadowGrid = ExtendedGraphics.Get2DRectangleShadow(Width - Chart.SHADOW_DEPTH, Height - Chart.SHADOW_DEPTH, new CornerRadius(6), new CornerRadius(6), 6);
                    ChartShadowGrid.Width = Width - Chart.SHADOW_DEPTH;
                    ChartShadowGrid.Height = Height - Chart.SHADOW_DEPTH;
                    ChartShadowGrid.IsHitTestVisible = false;
                    ChartShadowGrid.SetValue(Canvas.ZIndexProperty, 0);

                    _shadowGrid.Children.Add(ChartShadowGrid);

                    ChartShadowGrid.Margin = new Thickness(Chart.SHADOW_DEPTH, Chart.SHADOW_DEPTH, 0, 0);

                    if (this._chartBorder != null)
                        this._chartBorder.Margin = new Thickness(0, 0, SHADOW_DEPTH, SHADOW_DEPTH);

                    IsShadowApplied = true;
                }
            }

            if (ShadowEnabled && ChartShadowGrid != null)
                ChartShadowGrid.Visibility = Visibility.Visible;
            //else
            //    ChartShadowGrid.Visibility = Visibility.Collapsed;
        }
//#if WPF
//        [STAThread]
//#endif    
        internal void CallRender()
        {
            if (IsTemplateApplied)
            {
#if WPF

                if (RENDER_LOCK)
                    _renderLapsedCounter++;
                else if (Application.Current != null && Application.Current != null && Application.Current.Dispatcher.Thread != Thread.CurrentThread)
                    Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, new RenderDelegate(Render));
                else
                    Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new RenderDelegate(Render));
#else       
            if (RENDER_LOCK)
                _renderLapsedCounter++;
            else
            {
                if (IsInDesignMode)
                    Render();
                else
                    this.Dispatcher.BeginInvoke(Render);
            }
#endif
            }
        }

        /// <summary>
        /// Initializes the various properties of the class
        /// </summary>
        internal void Init()
        {

#if WPF
            NameScope.SetNameScope(this, new NameScope());
#endif
            Watermark = true;

            //ColorSets = new ColorSets();
            ToolTips = new ToolTipCollection();

            // Initialize title list
            Titles = new TitleCollection();

            // Initialize legend list
            Legends = new LegendCollection();

            TrendLines = new TrendLineCollection();

            // Initialize AxesX list
            AxesX = new AxisCollection();

            // Initialize AxesY list
            AxesY = new AxisCollection();

            // Initialize Series list
            Series = new DataSeriesCollection();

            PlotArea = new PlotArea();
            PlotArea.Chart = this;

            ToolTips.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(ToolTips_CollectionChanged);

            // Attach event handler for the Title collection changed event
            Titles.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(Titles_CollectionChanged);

            // Attach event handler for the Legend collection changed event
            Legends.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(Legends_CollectionChanged);

            // Attach event handler for the TrendLine collection changed event
            TrendLines.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(TrendLines_CollectionChanged);

            // Attach event handler for the Series collection changed event
            Series.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(Series_CollectionChanged);

            // Attach event handler for the AxesX collection changed event
            (AxesX as AxisCollection).CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(AxesX_CollectionChanged);

            // Attach event handler for the AxisY collection changed event
            (AxesY as AxisCollection).CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(AxesY_CollectionChanged);

            InternalColorSets = new ColorSets();
        }

        void ToolTips_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                ObservableCollection<ToolTip> titles = sender as ObservableCollection<ToolTip>;

                if (e.NewItems != null)
                {
                    foreach (ToolTip toolTip in e.NewItems)
                    {
                        if (toolTip.Style == null && StyleDictionary != null)
                        {
                            Style myStyle = StyleDictionary["ToolTip"] as Style;

                            if (myStyle != null)
                                toolTip.Style = myStyle;
                        }

                        toolTip.Chart = this;
                        
                    }
                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (ToolTip toolTip in e.OldItems)
                    (toolTip.Chart as Chart)._toolTipCanvas.Children.Remove(toolTip);
            }
        }

        /// <summary>
        /// Apply lighting effect for chart
        /// </summary>
        internal void ApplyLighting()
        {
            if (_chartLightingBorder != null)
            {
                if (LightingEnabled)
                {
                    _chartLightingBorder.Visibility = Visibility.Visible;
                    _chartLightingBorder.Opacity = 0.4;
                }
                else
                {
                    _chartLightingBorder.Visibility = Visibility.Collapsed;
                }
            }
        }

        /// <summary>
        /// Get collection of titles which are docked inside PlotArea
        /// Using LINQ
        /// </summary>
        /// <returns>List of Titles docked inside PlotArea </returns>
        internal List<Title> GetTitlesDockedInsidePlotArea()
        {   
            if (Titles != null)
            {
                var titlesDockedInsidePlotArea =
                    from title in Titles
                    where (title.DockInsidePlotArea == true)
                    select title;

                return titlesDockedInsidePlotArea.ToList<Title>();
            }

            return null;
        }

        /// <summary>
        /// Get collection of titles which are docked outside PlotArea
        /// Using LINQ
        /// </summary>
        /// <returns>List of Titles docked inside PlotArea </returns>
        internal List<Title> GetTitlesDockedOutSidePlotArea()
        {
            if (Titles != null)
            {   
                var titlesDockedOutSidePlotArea =
                    from title in Titles
                    where (title.DockInsidePlotArea == false)
                    select title;

                return titlesDockedOutSidePlotArea.ToList<Title>();
            }
            else
                return null;
        }

        #endregion

        #region Internal Events
#if WPF
        internal delegate void RenderDelegate();        // Delegate used for attaching Render() function as target while Invoking Render() using Dispatcher.
#endif
        #endregion

        #region Data

        // internal MouseEventArgs _mouseEventArgs;
        internal bool RENDER_LOCK = false;                                  // Render process Lock to recover from 
        internal static Double SHADOW_DEPTH = 4;                            // Shadow Depth for chart
        internal static Double BEVEL_DEPTH = 5;                             // Bevel Depth for chart
        internal Int32 _renderLapsedCounter;                                // Noumber of time UI render is not done called due to RENDER_LOCK
        private Thickness _chartAreaOriginalMargin;
        internal Boolean IsRenderCallAllowed = true;
        internal Boolean _internalAnimationEnabled = false;
        
        #endregion
    }
}
