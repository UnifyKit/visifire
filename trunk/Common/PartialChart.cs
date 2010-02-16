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
using System.ComponentModel;
using Visifire.Commons;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace Visifire.Charts
{
    /// <summary>
    /// Partially extended chart control class
    /// </summary>
    public partial class Chart : VisifireControl
    {   
        #region Public Methods

        /// <summary>
        /// Load controls from template
        /// </summary>
        private void LoadControlsFromTemplate()
        {
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
        }

        /// <summary>
        /// Set Margin for various elements to maintain effects like Shadow and Bevel
        /// </summary>
        public void SetMarginOfElements()
        {
            if (ShadowEnabled)
            {
                _chartBorder.Margin = new Thickness(0, 0, SHADOW_DEPTH, SHADOW_DEPTH);
                _bevelCanvas.Margin = new Thickness(0, 0, SHADOW_DEPTH, SHADOW_DEPTH);
            }
            
            _chartAreaMargin = new Thickness(_chartAreaGrid.Margin.Left, _chartAreaGrid.Margin.Top, _chartAreaGrid.Margin.Right, _chartAreaGrid.Margin.Bottom);

            if (Bevel)
            {   
                _chartAreaGrid.Margin = new Thickness(
                    _chartAreaMargin.Left + BEVEL_DEPTH,
                    _chartAreaMargin.Top + BEVEL_DEPTH,
                    _chartAreaMargin.Right + BEVEL_DEPTH,
                    _chartAreaMargin.Bottom + BEVEL_DEPTH);
            }
        }

        /// <summary>
        /// OnApplyTemplate() function is invoked whenever application code
        /// or internal processes such as a rebuilding layout pass. 
        /// </summary>
        public override void OnApplyTemplate()
        {   
            base.OnApplyTemplate();
            
            LoadControlsFromTemplate();

            LoadToolBar();

            if (StyleDictionary == null)
                LoadTheme("Theme1", false);

            LoadOrUpdateColorSets();

            LoadToolTips();

            SetMarginOfElements();

            ApplyLighting();

            AttachToolTipAndEvents();

            BindProperties();

            _internalAnimationEnabled = AnimationEnabled;

            if (_internalAnimationEnabled)
                _rootElement.IsHitTestVisible = false;

            _isTemplateApplied = true;

#if WPF
            NameScope.SetNameScope(this._rootElement, new NameScope());
#endif

            foreach (DataSeries ds in Series)
            {
                _rootElement.Children.Add(ds);
            }
        }


       
        /// <summary>
        /// Export Visifire chart 
        /// </summary>
        /// <param name="Chart">Visifire.Charts.Chart</param>
#if SL
        [System.Windows.Browser.ScriptableMember()]
#endif
        public void Export()
        {   
            // User will be able to select the image format type while saving
            base.Save(null, ExportType.Jpg , true);
        }

#if WPF
        /// <summary>
        /// Export Visifire chart 
        /// </summary>
        /// <param name="Chart">Visifire.Charts.Chart</param>
        public void Export(String path, ExportType exportType)
        {   
            base.Save(path, exportType, false);
        }
#endif

        #endregion

        #region Public Properties

        /// <summary>
        /// Identifies the Visifire.Charts.Chart.IndicatorEnabled dependency property.  
        /// </summary>
        public static readonly DependencyProperty IndicatorEnabledProperty =
            DependencyProperty.Register("IndicatorEnabled",
            typeof(Boolean),
            typeof(Chart),
            new PropertyMetadata(OnIndicatorEnabledPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.Chart.SmartLabelEnabled dependency property.  
        /// </summary>
        public static readonly DependencyProperty SmartLabelEnabledProperty =
            DependencyProperty.Register("SmartLabelEnabled",
            typeof(Boolean),
            typeof(Chart),
            new PropertyMetadata(OnSmartLabelEnabledPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.Chart.DataPointWidth dependency property.  
        /// </summary>
        public static readonly DependencyProperty DataPointWidthProperty =
            DependencyProperty.Register("DataPointWidth",
            typeof(Double),
            typeof(Chart),
            new PropertyMetadata(Double.NaN,OnDataPointWidthPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.Chart.UniqueColors dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Chart.UniqueColors dependency property.
        /// </returns>
        public static readonly DependencyProperty UniqueColorsProperty = DependencyProperty.Register
            ("UniqueColors",
            typeof(Boolean),
            typeof(Chart),
            new PropertyMetadata(true, OnUniqueColorsPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.Chart.ScrollingEnabled dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Chart.ScrollingEnabled dependency property.
        /// </returns>
        public static readonly DependencyProperty ScrollingEnabledProperty = DependencyProperty.Register
            ("ScrollingEnabled",
            typeof(Nullable<Boolean>),
            typeof(Chart),
            new PropertyMetadata(OnScrollingEnabledPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.Chart.View3D dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Chart.View3D dependency property.
        /// </returns>
        public static readonly DependencyProperty View3DProperty = DependencyProperty.Register
            ("View3D",
            typeof(Boolean),
            typeof(Chart),
            new PropertyMetadata(OnView3DPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.Chart.HrefTarget dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Chart.HrefTarget dependency property.
        /// </returns>
        public static readonly DependencyProperty HrefTargetProperty = DependencyProperty.Register
            ("HrefTarget",
            typeof(HrefTargets),
            typeof(Chart),
            new PropertyMetadata(OnHrefTargetChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.Chart.Href dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Chart.Href dependency property.
        /// </returns>
        public static readonly DependencyProperty HrefProperty = DependencyProperty.Register
            ("Href",
            typeof(String),
            typeof(Chart),
            new PropertyMetadata(OnHrefChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.Chart.Theme dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Chart.Theme dependency property.
        /// </returns>
        public static readonly DependencyProperty ThemeProperty = DependencyProperty.Register
            ("Theme",
            typeof(String),
            typeof(Chart),
            new PropertyMetadata(OnThemePropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.Chart.AnimationEnabled dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Chart.AnimationEnabled dependency property.
        /// </returns>
        public static readonly DependencyProperty AnimationEnabledProperty = DependencyProperty.Register
            ("AnimationEnabled",
            typeof(Boolean),
            typeof(Chart),
            null);

        /// <summary>
        /// Identifies the Visifire.Charts.Chart.AnimationEnabled dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Chart.AnimationEnabled dependency property.
        /// </returns>
        public static readonly DependencyProperty AnimatedUpdateProperty = DependencyProperty.Register
            ("AnimatedUpdate",
            typeof(Nullable<Boolean>),
            typeof(Chart), null);

        /// <summary>
        /// Identifies the Visifire.Charts.Chart.InternalBorderThickness dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Chart.InternalBorderThickness dependency property.
        /// </returns>
        private static readonly DependencyProperty InternalBorderThicknessProperty = DependencyProperty.Register
           ("InternalBorderThickness",
           typeof(Thickness),
           typeof(Chart),
           new PropertyMetadata(OnInternalBorderThicknessChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.Chart.InternalBackground dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Chart.InternalBackground dependency property.
        /// </returns>
        private static readonly DependencyProperty InternalBackgroundProperty = DependencyProperty.Register
          ("InternalBackground",
          typeof(Brush),
          typeof(Chart),
          new PropertyMetadata(OnInternalBackgroundPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.Chart.Bevel dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Chart.Bevel dependency property.
        /// </returns>
        public static readonly DependencyProperty BevelProperty = DependencyProperty.Register
            ("Bevel",
            typeof(Boolean),
            typeof(Chart),
            new PropertyMetadata(OnBevelPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.Chart.ColorSet dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Chart.ColorSet dependency property.
        /// </returns>
        public static readonly DependencyProperty ColorSetProperty = DependencyProperty.Register
            ("ColorSet",
            typeof(String),
            typeof(Chart),
            new PropertyMetadata(OnColorSetPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.Chart.ColorSets dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Chart.ColorSets dependency property.
        /// </returns>
        public static readonly DependencyProperty ColorSetsProperty = DependencyProperty.Register
            ("ColorSets",
            typeof(ColorSets),
            typeof(Chart),
            new PropertyMetadata(new ColorSets(), null));

        /// <summary>
        /// Identifies the Visifire.Charts.Chart.LightingEnabled dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Chart.LightingEnabled dependency property.
        /// </returns>
        public static readonly DependencyProperty LightingEnabledProperty = DependencyProperty.Register
            ("LightingEnabled",
            typeof(Boolean),
            typeof(Chart),
            new PropertyMetadata(OnLightingEnabledPropertyChanged));
        
        /// <summary>
        /// Identifies the Visifire.Charts.Chart.CornerRadius dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Chart.CornerRadius dependency property.
        /// </returns>
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register
            ("CornerRadius",
            typeof(CornerRadius),
            typeof(Chart),
            null);
        
        /// <summary>
        /// Identifies the Visifire.Charts.Chart.ShadowEnabled dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Chart.ShadowEnabled dependency property.
        /// </returns>
        public static readonly DependencyProperty ShadowEnabledProperty = DependencyProperty.Register
            ("ShadowEnabled",
            typeof(Boolean),
            typeof(Chart),
            new PropertyMetadata(OnShadowEnabledPropertyChanged));
                
        /// <summary>
        /// Identifies the Visifire.Charts.Chart.PlotArea dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Chart.PlotArea dependency property.
        /// </returns>
        public static readonly DependencyProperty PlotAreaProperty = DependencyProperty.Register
        ("PlotArea",
        typeof(PlotArea),
        typeof(Chart),
        new PropertyMetadata(OnPlotAreaPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.Chart.MinScrollingGap dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Chart.MinScrollingGap dependency property.
        /// </returns>
        [Obsolete]
        public static readonly DependencyProperty MinimumGapProperty = DependencyProperty.Register
            ("MinimumGap",
            typeof(Nullable<Double>),
            typeof(Chart),
            new PropertyMetadata(OnMinimumGapPropertyChanged));

        /// <summary>
        /// Decides how the color will be applied to the DataPoints.
        /// <example>If UniqueColors = True and if only one DataSeries is present in Chart then each DataPoint in that DataSeries takes one color from the ColorSet given to the chart.
        /// If UniqueColors = True and if more than one DataSeries are present in Chart then each series takes one color from the ColorSet provided at the chart.
        /// If UniqueColors = False then each DataSeries takes one color
        /// </example>
        /// </summary>
        public Boolean UniqueColors
        {
            get
            {
                return (Boolean)GetValue(UniqueColorsProperty);
            }
            set
            {
                SetValue(UniqueColorsProperty, value);
            }
        }

        /// <summary>
        /// Enables indicator. Currently this property is applicable for Line DataSeries only.
        /// </summary>
        public Boolean IndicatorEnabled
        {
            get { return (Boolean)GetValue(IndicatorEnabledProperty); }
            set { SetValue(IndicatorEnabledProperty, value); }
        }

        /// <summary>
        /// Whether skipping of labels are allowed
        /// Note: Currently implemented for Pie and Doughnut charts only
        /// </summary>
        public Boolean SmartLabelEnabled
        {
            get { return (Boolean)GetValue(SmartLabelEnabledProperty); }
            set { SetValue(SmartLabelEnabledProperty, value); }
        }

        /// <summary>
        /// Width of a DataPoint
        /// </summary>
        public Double DataPointWidth
        {
            get { return (Double)GetValue(DataPointWidthProperty); }
            set { SetValue(DataPointWidthProperty, value); }
        }

        /// <summary>
        /// Minimum gap between two DataPoint of same series in PlotArea
        /// </summary>
#if SL
        [System.ComponentModel.TypeConverter(typeof(Converters.NullableDoubleConverter))]
#endif
        [Obsolete]
        public Nullable<Double> MinimumGap
        {
            get
            {
                return (Nullable<Double>)GetValue(MinimumGapProperty);
            }
            set
            {
                SetValue(MinimumGapProperty, value);
            }
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
        
        /// <summary>
        /// Target window Property for hyperlink 
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
        /// Hyperlink Property
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
        /// Name of the theme to be applied
        /// </summary>
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

        /// <summary>
        /// Enables or disables animation
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

        /// <summary>
        /// Enables or disables animation
        /// </summary>
        [System.ComponentModel.TypeConverter(typeof(NullableBoolConverter))]
        public Nullable<Boolean> AnimatedUpdate
        {
            get
            {
                if ((Nullable<Boolean>)GetValue(AnimatedUpdateProperty) == null)
                {
                    if (Series.Count == 1 && (Series[0].RenderAs == RenderAs.Pie || Series[0].RenderAs == RenderAs.Doughnut))
                        return true;
                    else
                        return false;
                }
                else
                    return (Nullable<Boolean>)GetValue(AnimatedUpdateProperty);
            }
            set
            {
                SetValue(AnimatedUpdateProperty, value);
            }
        }

        #region BorderProperties

        /// <summary>
        /// BorderThickness property
        /// </summary>
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
        
        #endregion

        /// <summary>
        /// Background color property
        /// </summary>
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
        
        /// <summary>
        /// Set of colors that will be used for the InternalDataPoints
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
        
        /// <summary>
        /// Set of colors that will be used for the InternalDataPoints
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
                       
        /// <summary>
        /// Enables or disables automatic color shading
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
                       
        /// <summary>
        /// PlotArea of the chart
        /// </summary>
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
                PlotArea.PropertyChanged -= Element_PropertyChanged;
                PlotArea.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(Element_PropertyChanged);
            }
        }

        /// <summary>
        /// AxesX as AxisCollection of type Axis
        /// </summary>
        public AxisCollection AxesX
        {
            get;
            set;
        }

        /// <summary>
        /// AxesY as AxisCollection of type Axis
        /// </summary>
        public AxisCollection AxesY
        {
            get;
            set;
        }
        
        /// <summary>
        /// Titles as TitleCollection of type Title
        /// </summary>
        public TitleCollection Titles
        {
            get;
            set;
        }

        /// <summary>
        /// Legends as LegendCollection of type Legend
        /// </summary>
        public LegendCollection Legends
        {
            get;
            set;
        }

        /// <summary>
        /// ToolTips as ToolTipCollection of type ToolTip
        /// </summary>
        public ToolTipCollection ToolTips
        {
            get;
            set;
        }

        /// <summary>
        /// TrendLines as TrendLineCollection of type TrendLine
        /// </summary>
        public TrendLineCollection TrendLines
        {
            get;
            set;
        }

        /// <summary>
        /// Collection of DataSeries
        /// </summary>
        public DataSeriesCollection Series
        {
            get;
            set;
        }

        #endregion

        #region Public Events

        /// <summary>
        /// Event handler for the Rendered event 
        /// </summary>
        public event EventHandler Rendered
        {
            remove
            {
                _rendered -= value;
            }
            add
            {
                _rendered += value;
            }
        }

        #endregion

        #region Protected Methods

        #endregion

        #region Private Delegates

        #endregion

        #region Private Methods

        /// <summary>
        /// Initializes the various properties of the class
        /// </summary>
        private void Init()
        {
            // Initialize colorSets
            //ColorSets = new ColorSets();

            // Initialize tooltip
            ToolTips = new ToolTipCollection();

            // Initialize title list
            Titles = new TitleCollection();

            // Initialize legend list
            Legends = new LegendCollection();

            // Initialize trendLine list
            TrendLines = new TrendLineCollection();

            // Initialize AxesX list
            AxesX = new AxisCollection();

            // Initialize AxesY list
            AxesY = new AxisCollection();

            // Initialize Series list
            Series = new DataSeriesCollection();

            // Initialize PlotArea
            PlotArea = new PlotArea() { Chart = this };

            // Attach event handler on collection changed event with ToolTip collection
            ToolTips.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(ToolTips_CollectionChanged);

            // Attach event handler on collection changed event with Title collection
            Titles.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(Titles_CollectionChanged);

            // Attach event handler on collection changed event with Legend collection
            Legends.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(Legends_CollectionChanged);

            // Attach event handler on collection changed event with TrendLines collection
            TrendLines.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(TrendLines_CollectionChanged);

            // Attach event handler on collection changed event with DataSeries collection
            Series.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(Series_CollectionChanged);

            // Attach event handler on collection changed event with axisX collection
            (AxesX as AxisCollection).CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(AxesX_CollectionChanged);

            // Attach event handler on collection changed event with AxisY collection
            (AxesY as AxisCollection).CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(AxesY_CollectionChanged);
        }


        /// <summary>
        /// OnMouseMoveLeave chart set ToolTip position. 
        /// ToolTip at current mouse position over the chart control        
        /// </summary>
        /// <param name="sender">Chart as object</param>
        /// <param name="e">MouseEventArgs</param>
        private void Chart_MouseLeave(object sender, MouseEventArgs e)
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

        /// <summary>
        /// OnMouseMoveOver chart set ToolTip position. 
        /// ToolTip at current mouse position over the chart control        
        /// </summary>
        /// <param name="sender">Chart as object</param>
        /// <param name="e">MouseEventArgs</param>
        private void Chart_MouseMove(object sender, MouseEventArgs e)
        {
            UpdateToolTipPosition(sender, e);
        }

        /// <summary>
        /// Event handler manages the addition and removal of tooltip from tooltip list of chart
        /// </summary>
        /// <param name="sender">ToolTips</param>
        /// <param name="e">NotifyCollectionChangedEventArgs</param>
        private void ToolTips_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
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
        /// Event handler manages the addition and removal of Axis from AxesX list of chart
        /// </summary>
        /// <param name="sender">AxesX</param>
        /// <param name="e">NotifyCollectionChangedEventArgs</param>
        private void AxesX_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Boolean isAutoAxis = false;

            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                if (e.NewItems != null)
                {
                    foreach (Axis axis in e.NewItems)
                    {
                        axis.Chart = this;

                        if (axis._isAutoGenerated)
                            isAutoAxis = true;

                        if (axis.AxisLabels != null)
                            axis.AxisLabels.Chart = this;

                        foreach (ChartGrid cg in axis.Grids)
                            cg.Chart = this;

                        foreach (Ticks ticks in axis.Ticks)
                            ticks.Chart = this;

                        axis.IsNotificationEnable = false;

                        if (axis.StartFromZero == null)
                            axis.StartFromZero = false;

                        axis.IsNotificationEnable = true;

                        axis.PropertyChanged -= Element_PropertyChanged;
                        axis.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(Element_PropertyChanged);
                    }
                }
            }

            _forcedRedraw = true;

            if (!isAutoAxis)
            {
                InvokeRender();
            }
        }

        /// <summary>
        /// Event handler manages the addition and removal of Axis from AxesY list of chart
        /// </summary>
        /// <param name="sender">AxesY</param>
        /// <param name="e">NotifyCollectionChangedEventArgs</param>
        private void AxesY_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Boolean isAutoAxis = false;

            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                if (e.NewItems != null)
                {
                    foreach (Axis axis in e.NewItems)
                    {   
                        axis.Chart = this;

                        if (axis._isAutoGenerated)
                            isAutoAxis = true;

                        axis.AxisRepresentation = AxisRepresentations.AxisY;

                        if (axis.AxisLabels != null)
                            axis.AxisLabels.Chart = this;

                        foreach (ChartGrid cg in axis.Grids)
                            cg.Chart = this;

                        foreach (Ticks ticks in axis.Ticks)
                            ticks.Chart = this;

                        axis.IsNotificationEnable = false;

                        if (axis.StartFromZero == null)
                            axis.StartFromZero = true;

                        axis.IsNotificationEnable = true;

                        axis.PropertyChanged -= Element_PropertyChanged;
                        axis.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(Element_PropertyChanged);
                    }
                }
            }

            _forcedRedraw = true;

            if (!isAutoAxis)
            {
                InvokeRender();
            }
        }

        /// <summary>
        /// Event handler manages the addition and removal of title from chart
        /// </summary>
        /// <param name="sender">Titles</param>
        /// <param name="e">NotifyCollectionChangedEventArgs</param>
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
                        title.PropertyChanged -= Element_PropertyChanged;
                        title.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(Element_PropertyChanged);
                    }
                }
            }

            InvokeRender();
        }
        
        /// <summary>
        /// Event handler manages the addition and removal of legend from chart
        /// </summary>
        /// <param name="sender">Legend</param>
        /// <param name="e">NotifyCollectionChangedEventArgs</param>
        private void Legends_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Boolean isAutoLegend = false;

            if (e.NewItems != null)
            {
                foreach (Legend legend in e.NewItems)
                {
                    if (legend._isAutoGenerated)
                        isAutoLegend = true;

                    if (Legends.Count > 0)
                    {   
                        if (String.IsNullOrEmpty((String)legend.GetValue(NameProperty)))
                            legend.SetValue(NameProperty, "Legend" + Legends.IndexOf(legend));
                    }

                    legend.Chart = this;
                    legend.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(Element_PropertyChanged);
                }

            }

            if (!isAutoLegend)
            {
                InvokeRender();
            }
        }
        
        /// <summary>
        /// Event handler manages the addition and removal of trendLine from chart
        /// </summary>
        /// <param name="sender">TrendLine</param>
        /// <param name="e">NotifyCollectionChangedEventArgs</param>
        private void TrendLines_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
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

                        trendLine.PropertyChanged -= Element_PropertyChanged;
                        trendLine.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(Element_PropertyChanged);
                    }
                }

            }

            _forcedRedraw = true;
            InvokeRender();
        }

        /// <summary>
        /// Event handler manages the addition and removal of DataSeries from Series list of chart
        /// </summary>
        /// <param name="sender">Series</param>
        /// <param name="e">NotifyCollectionChangedEventArgs</param>
        private void Series_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                if (e.NewItems != null)
                    foreach (DataSeries ds in e.NewItems)
                    {   
                        ds.Chart = this;

                        //if (ds.DataContext == null)
                          //  ds.SetBinding(DataContextProperty, new Binding());

                        foreach (DataPoint dp in ds.DataPoints)
                        {   
                            dp.Chart = this;
                        }

                        if (!String.IsNullOrEmpty(this.Theme))
                            ds.ApplyStyleFromTheme(this, "DataSeries");

                        if (String.IsNullOrEmpty((String)ds.GetValue(NameProperty)))
                        {
                            ds.Name = "DataSeries" + (this.Series.Count - 1).ToString() + "_" + Guid.NewGuid().ToString().Replace('-', '_');

                            // ds.SetValue(NameProperty, ds.GetType().Name + this.Series.IndexOf(ds).ToString() + "_" + Guid.NewGuid().ToString().Replace('-', '_'));
                            ds._isAutoName = true;
                        }
                        else
                            ds._isAutoName = false;

                        ds.PropertyChanged -= Element_PropertyChanged;
                        ds.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(Element_PropertyChanged);
                    }
                 
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                if (e.OldItems != null)
                {

                    List<DataSeries> dataSeriesListExceptOldItems = (from x in Series where !e.OldItems.Contains(x) select x).ToList();
                    List<Panel> preExistingCanvases = (from can in dataSeriesListExceptOldItems select can.Visual).ToList();

                    foreach (DataSeries ds in e.OldItems)
                    {   
                        ds.RemoveToolTip();

                        if (ds.Visual != null)
                        {   
                            if(preExistingCanvases.Contains(ds.Visual))
                                continue;
                            
                            if ((ds.PlotGroup != null && ds.PlotGroup.DataSeriesList.Count == 1) || (ds.RenderAs == RenderAs.Area))
                            {
                                Panel seriesVisual = ds.Visual;

                                // remove pre existing parent panel for the series visual 
                                if (seriesVisual != null && seriesVisual.Parent != null)
                                {
                                    Panel parent = seriesVisual.Parent as Panel;
                                    parent.Children.Remove(seriesVisual);
                                }

                                ds.Visual = null;
                            }
                        }
                    }
                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Reset)
            {   
                if (this.InternalSeries != null)
                {   
                    foreach (DataSeries ds in InternalSeries)
                    {   
                        Panel seriesVisual = ds.Visual;

                        // remove pre existing parent panel for the series visual 
                        if (seriesVisual != null && seriesVisual.Parent != null)
                        {
                            Panel parent = seriesVisual.Parent as Panel;
                            parent.Children.Remove(seriesVisual);
                        }

                        ds.RemoveToolTip();
                    }

                    InternalSeries.Clear();
                }
            }

            _datapoint2UpdatePartially = null;
            InvokeRender();
        }
              
        /// <summary>
        /// Event handler manages property change of visifire elements, like Title, Legends, DataSeries etc.
        /// </summary>
        /// <param name="sender">ObservableObject</param>
        /// <param name="e">PropertyChangedEventArgs</param>
        private void Element_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            InvokeRender();
        }

        /// <summary>
        /// Load or Update color sets from chart resource
        /// </summary>
        private void LoadOrUpdateColorSets()
        {
            if (EmbeddedColorSets == null)
            {
                string resourceName = "Visifire.Charts.ColorSets.xaml"; // Resource location for embedded color sets

                using (System.IO.Stream s = typeof(Chart).Assembly.GetManifestResourceStream(resourceName))
                {
                    if (s != null)
                    {
                        System.IO.StreamReader reader = new System.IO.StreamReader(s);

                        String xaml = reader.ReadToEnd();
#if WPF
                        EmbeddedColorSets = XamlReader.Load(new XmlTextReader(new StringReader(xaml))) as ColorSets;
#else
                        EmbeddedColorSets = System.Windows.Markup.XamlReader.Load(xaml) as ColorSets;
#endif
                        //if (EmbeddedColorSets == null)
                        //    System.Diagnostics.Debug.WriteLine("Unable to load embedded ColorSets. Reload project and try again.");

                        //if (InternalColorSets == null)
                        //    InternalColorSets = new ColorSets();

                        //if (EmbeddedColorSets != null)
                        //    InternalColorSets.AddRange(EmbeddedColorSets);

                        //if (ColorSets != null)
                        //    InternalColorSets.AddRange(ColorSets);

                        reader.Close();
                        s.Close();
                    }
                }
            }
            //else
            //{
            //    InternalColorSets.Clear();

            //    if (EmbeddedColorSets != null)
            //        InternalColorSets.AddRange(EmbeddedColorSets);

            //    if (ColorSets != null)
            //        InternalColorSets.AddRange(ColorSets);
            //}
        }

        /// <summary>
        /// Some property needs to bind with the existing property in control before first time render. 
        /// And binding should be done only once. 
        /// </summary>
        private void BindProperties()
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
        /// Prepare chart area for drawing
        /// </summary>
        private void PrepareChartAreaForDrawing()
        {
            // Create new ChartArea
            if (ChartArea == null)
            {
                ChartArea = new ChartArea(this as Chart);
            }

#if WPF
            NameScope.SetNameScope(this._rootElement, new NameScope());
#endif

            ApplyChartBevel();

            ApplyChartShadow(this.ActualWidth, this.ActualHeight);

            _renderLapsedCounter = 0;
        }

        /// <summary>
        /// Apply bevel effect to Chart
        /// </summary>
        /// <returns>If success return true else false.</returns>
        private Boolean ApplyChartBevel()
        {
            if (Bevel && _rootElement != null)
            {
                if (_chartBorder.ActualWidth == 0 && _chartBorder.ActualHeight == 0)
                {
                    return false;
                }

                _chartAreaGrid.Margin = new Thickness(
                    _chartAreaMargin.Left + BEVEL_DEPTH,
                    _chartAreaMargin.Top + BEVEL_DEPTH,
                    _chartAreaMargin.Right,
                    _chartAreaMargin.Bottom);

                _chartAreaGrid.UpdateLayout();

                _bevelCanvas.Children.Clear();

                Brush topBrush = Graphics.GetBevelTopBrush(this.Background);
                Brush leftBrush = Graphics.GetBevelSideBrush(0, this.Background);
                Brush rightBrush = Graphics.GetBevelSideBrush(170, this.Background);
                Brush bottomBrush = Graphics.GetBevelSideBrush(180, this.Background);

                BevelVisual = ExtendedGraphics.Get2DRectangleBevel(null,
                    _chartBorder.ActualWidth - _chartBorder.BorderThickness.Left - _chartBorder.BorderThickness.Right - _chartAreaMargin.Right - _chartAreaMargin.Left,
                    _chartBorder.ActualHeight - _chartBorder.BorderThickness.Top - _chartBorder.BorderThickness.Bottom - _chartAreaMargin.Top - _chartAreaMargin.Bottom,
                BEVEL_DEPTH, BEVEL_DEPTH, topBrush, leftBrush, rightBrush, bottomBrush);

                if (LightingEnabled)
                {
                    _chartLightingBorder.Opacity = 0.4;
                }

                BevelVisual.Margin = new Thickness(0, 0, 0, 0);
                BevelVisual.IsHitTestVisible = false;
                BevelVisual.SetValue(Canvas.TopProperty, _chartAreaMargin.Top + _chartBorder.BorderThickness.Top);
                BevelVisual.SetValue(Canvas.LeftProperty, _chartAreaMargin.Left + _chartBorder.BorderThickness.Left);

                _bevelCanvas.Children.Add(BevelVisual);
            }

            return true;
        }

        /// <summary>
        /// Remove bevel effect from chart
        /// </summary>
        private void RemoveChartBevel()
        {
            if (_isTemplateApplied)
            {
                if (BevelVisual != null && _bevelCanvas != null)
                {
                    _bevelCanvas.Children.Clear();
                    _chartAreaGrid.Margin = new Thickness(0);
                }
            }
        }
              
        /// <summary>
        /// Apply lighting effect for chart
        /// </summary>
        private void ApplyLighting()
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
        /// Apply shadow effect on chart
        /// (Creates a shadow layer behind the chart)
        /// </summary>
        /// <param name="Height">Chart width</param>
        /// <param name="Width">Chart height</param>
        private void ApplyChartShadow(Double width, Double height)
        {   
            if (!_isShadowApplied && ShadowEnabled && !Double.IsNaN(height) && height != 0 && !Double.IsNaN(width) && width != 0)
            {   
                _shadowGrid.Children.Clear();

                if (_rootElement != null)
                {   
                    // Shadow grid contains multiple rectangles that give a blurred effect at the edges 
                    ChartShadowLayer = ExtendedGraphics.Get2DRectangleShadow(null, width - Chart.SHADOW_DEPTH, height - Chart.SHADOW_DEPTH, new CornerRadius(6), new CornerRadius(6), 6);
                    ChartShadowLayer.Width = width - Chart.SHADOW_DEPTH;
                    ChartShadowLayer.Height = height - Chart.SHADOW_DEPTH;
                    ChartShadowLayer.IsHitTestVisible = false;
                    ChartShadowLayer.SetValue(Canvas.ZIndexProperty, 0);

                    _shadowGrid.Children.Add(ChartShadowLayer);

                    ChartShadowLayer.Margin = new Thickness(Chart.SHADOW_DEPTH, Chart.SHADOW_DEPTH, 0, 0);

                    if (this._chartBorder != null)
                        this._chartBorder.Margin = new Thickness(0, 0, SHADOW_DEPTH, SHADOW_DEPTH);

                    _isShadowApplied = true;
                }
            }

            if (ShadowEnabled && ChartShadowLayer != null)
                ChartShadowLayer.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Removes Chart shadow
        /// </summary>
        private void RemoveShadow()
        {
            if (_isTemplateApplied && !ShadowEnabled)
            {
                _chartBorder.Margin = new Thickness(0, 0, 0, 0);
                _bevelCanvas.Margin = new Thickness(0, 0, 0, 0);
                _isShadowApplied = false;

                if (ChartShadowLayer != null)
                    ChartShadowLayer.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Load theme from resource file and select specific theme using theme name
        /// </summary>
        /// <param name="themeName">String themeName</param>
        private void LoadTheme(String themeName, Boolean isThemePropertyChanged)
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
//#if SL
//                if (Style == null)
//                {
//                    Style myStyle = StyleDictionary["Chart"] as Style;

//                    if (myStyle != null)
//                        Style = myStyle;
//                }
//#else
                Style myStyle = StyleDictionary["Chart"] as Style;

                _isThemeChanged = isThemePropertyChanged;

                if (myStyle != null)
                {
                    if (isThemePropertyChanged)
                        Style = myStyle;
                    else if(Style == null)
                        Style = myStyle;
                }

//#endif
            }
            else
            {
                throw new Exception("Theme file " + themeName + ".xaml not found..");
            }
        }

        /// <summary>
        /// Loads default ToolTips if required. 
        /// Currently only one tooltip is supported
        /// </summary>
        private void LoadToolTips()
        {
            if (ToolTips.Count == 0)
            {
                ToolTip toolTip = new ToolTip() { Chart = this, Visibility = Visibility.Collapsed };
                ToolTips.Add(toolTip);
            }

            _toolTip = ToolTips[0];

            _toolTip.Chart = this;

            _toolTipCanvas.Children.Add(_toolTip);

            // Attach events to the root element of the chart to track mouse movement over chart.
            this._rootElement.MouseLeave += new MouseEventHandler(Chart_MouseLeave);
        }
        
        /// <summary>
        /// Attach events and tooltip to chart
        /// </summary>
        private void AttachToolTipAndEvents()
        {   
            if (!String.IsNullOrEmpty(ToolTipText))
                AttachToolTip(this, this, this);
           
            AttachEvents2Visual(this, this, this._rootElement);

            AttachEvents2Visual4MouseDownEvent(this, this, this._plotCanvas);
        }

        /// <summary>
        /// DataPointWidthProperty changed call back function
        /// </summary>
        /// <param name="d">Chart</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnSmartLabelEnabledPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Chart c = d as Chart;
            c.InvokeRender();
        }

        /// <summary>
        /// IndicatorEnabledProperty changed call back function
        /// </summary>
        /// <param name="d">Chart</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnIndicatorEnabledPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Chart c = d as Chart;
            c.InvokeRender();
        }

        /// <summary>
        /// DataPointWidthProperty changed call back function
        /// </summary>
        /// <param name="d">Chart</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnDataPointWidthPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Chart c = d as Chart;
            c.InvokeRender();
        }

        /// <summary>
        /// ScrollingEnabledProperty changed call back function
        /// </summary>
        /// <param name="d">Chart</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnScrollingEnabledPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Chart c = d as Chart;
            c.InvokeRender();
        }

        /// <summary>
        /// UniqueColorsProperty changed call back function
        /// </summary>
        /// <param name="d">Chart</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnUniqueColorsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Chart c = d as Chart;
            c.InvokeRender();
        }

        /// <summary>
        /// View3DProperty changed call back function
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnView3DPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Chart c = d as Chart;
            c._forcedRedraw = true;
            c.InvokeRender();
        }

        /// <summary>
        /// HrefTargetProperty changed call back function
        /// </summary>
        /// <param name="d">Chart</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnHrefTargetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Chart c = d as Chart;
            c.AttachHref(c, c, c.Href, (HrefTargets)e.NewValue);
        }

        /// <summary>
        /// HrefProperty changed call back function
        /// </summary>
        /// <param name="d">Chart</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnHrefChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Chart c = d as Chart;
            c.AttachHref(c, c, e.NewValue.ToString(), c.HrefTarget);
        }

        /// <summary>
        /// ThemeProperty changed call back function
        /// </summary>
        /// <param name="d">Chart</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnThemePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Chart c = d as Chart;
            c.LoadTheme((String)e.NewValue, (e.OldValue == null)?false:true);
            c.InvokeRender();
        }

        /// <summary>
        /// InternalBorderThicknessProperty changed call back function
        /// </summary>
        /// <param name="d">Chart</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnInternalBorderThicknessChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Chart c = d as Chart;

            c.RemoveChartBevel();
            c.InvokeRender();
        }

        /// <summary>
        /// InternalBackgroundProperty changed call back function
        /// </summary>
        /// <param name="d">Chart</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnInternalBackgroundPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Chart c = d as Chart;
            c.RemoveChartBevel();
            c.ApplyChartBevel();
            c.InvokeRender();
        }

        /// <summary>
        /// InternalPropertyProperty changed call back function
        /// </summary>
        /// <param name="d">Chart</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnInternalPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Chart c = d as Chart;

            if (c.ApplyChartBevel())
            {
                if ((Boolean)c.Bevel == true)
                {
                    c.InvokeRender();
                }
                else
                {
                    c.RemoveChartBevel();
                }
            }
        }

        /// <summary>
        /// Property changed call back function
        /// </summary>
        /// <param name="d">Chart</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnBevelPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Chart c = d as Chart;
            c.RemoveChartBevel();
            c.InvokeRender();
        }

        /// <summary>
        /// Property changed call back function
        /// </summary>
        /// <param name="d">Chart</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnColorSetPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Chart c = d as Chart;
            c.InvokeRender();
        }

        /// <summary>
        /// Property changed call back function
        /// </summary>
        /// <param name="d">Chart</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnLightingEnabledPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Chart c = d as Chart;
            c.ApplyLighting();
        }

        /// <summary>
        /// Property changed call back function
        /// </summary>
        /// <param name="d">Chart</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnShadowEnabledPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Chart c = d as Chart;

            if ((Boolean)e.NewValue == true)
            {
                c.ApplyChartShadow(c.ActualWidth, c.ActualHeight);
            }
            else
            {
                c.RemoveShadow();
            }

            c.ApplyChartBevel();
        }

        /// <summary>
        /// PlotAreaProperty changed call back function
        /// </summary>
        /// <param name="d">Chart</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnPlotAreaPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Chart chart = d as Chart;
            PlotArea plotArea = (PlotArea)e.NewValue;
            plotArea.Chart = chart;
            plotArea.PropertyChanged -= chart.Element_PropertyChanged;
            plotArea.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(chart.Element_PropertyChanged);

            chart.InvokeRender();
        }

        /// <summary>
        /// MinScrollingGapProperty changed call back function
        /// </summary>
        /// <param name="d">Chart</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnMinimumGapPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Chart c = d as Chart;
            c.InvokeRender();
        }

        #endregion

        #region Private Properties


        /// <summary>
        /// Embedded color sets
        /// </summary>
        private ColorSets EmbeddedColorSets
        {
            get;
            set;
        }

        /// <summary>
        /// Chart shadow layer is created and added to ShadowGrid, present in template
        /// </summary>
        private Grid ChartShadowLayer
        {
            get;
            set;
        }

        /// <summary>
        /// Foreground is a default property of a Control and it is not applicable in chart. 
        /// Hides the Foreground property in control
        /// </summary>
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        private new Brush Foreground
        {
            get;
            set;
        }
        
        #endregion
                
        #region Internal Properties

        /// <summary>
        /// If scroll is activated by any cause then IsScrollingActivated should be set to true. 
        /// </summary>
        internal Boolean IsScrollingActivated
        {   
            get;
            set;
        }

        /// <summary>
        /// Collection of DataSeries
        /// </summary>
        internal List<DataSeries> InternalSeries
        {
            get;
            set;
        }

        /// <summary>
        /// Bevel canvas as Bevel Visual
        /// </summary>
        internal Canvas BevelVisual
        {
            get;
            set;
        }

        /// <summary>
        /// PlotDetails holds plotting information. 
        /// As example, grouping of series depending upon axis types and RenderAs type of Series.
        /// </summary>
        internal PlotDetails PlotDetails
        {
            get;
            set;
        }

        /// <summary>
        /// Chart area is the entire area defined by the chart. 
        /// Chart area manages jobs related to plotting the graph.
        /// </summary>
        internal ChartArea ChartArea
        {
            get;
            set;
        }
        
        /// <summary>
        /// StyleDictionary of themes
        /// </summary>
        internal ResourceDictionary StyleDictionary
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
        internal IList<Axis> InternalAxesY
        {
            get;
            set;
        }

        #endregion
        
        #region Internal Methods

        /// <summary>
        /// Visually select the DataPoints which are selected
        /// </summary>
        /// <param name="chart"></param>
        internal static void SelectDataPoints(Chart chart)
        {
            if (chart == null || chart.InternalSeries == null)
                return;

            foreach (DataSeries ds in chart.InternalSeries)
            {
                if (ds.SelectionEnabled)
                {   
                    foreach (DataPoint dp in ds.DataPoints)
                    {
                        if (dp.Selected)
                        {
                            dp.Select(true);

                            if (ds.SelectionMode == SelectionModes.Single)
                                dp.DeSelectOthers();
                        }
                        else
                            dp.DeSelect(dp, true, true);
                    }
                }
            }
        }
        
        /// <summary>
        /// Get ColorSet by ColorSet name
        /// </summary>
        /// <param name="id">Name of the ColorSet</param>
        /// <returns>ColorSet</returns>
        internal ColorSet GetColorSetByName(String id)
        {
            ColorSet colorSet = null;

            if (ColorSets != null && ColorSets.Count > 0)
                colorSet = ColorSets.GetColorSetByName(id);
            
            if(colorSet == null)
                colorSet = EmbeddedColorSets.GetColorSetByName(id);
            
            return colorSet;
        }

        /// <summary>
        /// Fire Rendered event
        /// </summary>
        internal void FireRenderedEvent()
        {
            if (_rendered != null)
                _rendered(this, null);
        }

        /// <summary>
        /// Check if exception occurred due to negative size error
        /// </summary>
        /// <param name="e">ArgumentException</param>
        /// <returns>Boolean</returns>
        internal Boolean CheckSizeError(ArgumentException e)
        {
            if (e == null)
                return false;

            String stackTrace = e.StackTrace.TrimStart();

            if (stackTrace.Contains("set_Height(Double value)")
            || stackTrace.Contains("set_Width(Double value)")
            || e.Message == "Height must be non-negative." || e.Message == "Width must be non-negative.")
            {
#if DEBUG
                System.Diagnostics.Debug.WriteLine("------------------------------------------");
                System.Diagnostics.Debug.WriteLine("The following SizeChanged Error is handled");
                System.Diagnostics.Debug.WriteLine("------------------------------------------");
                System.Diagnostics.Debug.WriteLine(e.Message);
                if (e.StackTrace != null)
                    System.Diagnostics.Debug.WriteLine(e.StackTrace);

                System.Diagnostics.Debug.WriteLine("------------------------------------------");
#endif
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// On ToolTipText PropertyChanged event, this function is invoked
        /// </summary>
        /// <param name="NewValue">New text value</param>
        internal override void OnToolTipTextPropertyChanged(string newValue)
        {
            base.OnToolTipTextPropertyChanged(newValue);
            DetachToolTip(this._toolTipCanvas);

            if (!String.IsNullOrEmpty(newValue))
                AttachToolTip(this, this, this);
        }
        
        /// <summary>
        /// Get collection of titles which are docked inside PlotArea
        /// using LINQ
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
        /// using LINQ
        /// </summary>
        /// <returns>List of Titles docked out side PlotArea</returns>
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
        
        /// <summary>
        /// Render is a delegate to a method that takes no arguments and does not return a value, 
        /// which is pushed onto the System.Windows.Threading.Dispatcher event queue.
        /// </summary>
        internal void InvokeRender()
        {

            if (_isTemplateApplied)
            {
                if (_renderLock)
                    _renderLapsedCounter++;
                else
                {
#if WPF             
                    if (Application.Current != null && Application.Current.Dispatcher.Thread != Thread.CurrentThread)
                        Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, new RenderDelegate(Render));
                    else
                        Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new RenderDelegate(Render));
#else
                    if (IsInDesignMode)
                        Render();
                    else
                        this.Dispatcher.BeginInvoke(Render);
#endif
                }
            }
        }
        
        /// <summary>
        /// Render redraws the chart
        /// </summary>
        internal void Render()
        {
            if (_isTemplateApplied && !_renderLock && _rootElement != null)
            {
                if (Double.IsNaN(this.ActualWidth) || Double.IsNaN(this.ActualHeight) || this.ActualWidth == 0 || this.ActualHeight == 0)
                    return;

                _renderLock = true;

                try
                {   
                    PrepareChartAreaForDrawing();

                    ChartArea.Draw(this);
                }
                catch (Exception e)
                {
                    _renderLock = false;
                    if (CheckSizeError(e as ArgumentException))
                        return;
                    else
                        throw new Exception(e.Message, e);
                }
            }
        }
        
        internal void UnlockRender()
        {   
            _renderLock = false;           
        }

        /// <summary>
        /// Calculate font color of DataPoint labels depending upon chart background
        /// </summary>
        /// <param name="chart">Chart</param>
        /// <param name="dataPoint">DataPoint</param>
        /// <param name="labelFontColor">labelFontColor</param>
        /// <param name="labelStyle">labelStyle</param>
        /// <returns>Brush</returns>
        internal static Brush CalculateDataPointLabelFontColor(Chart chart, DataPoint dataPoint, Brush labelFontColor, LabelStyles labelStyle)
        {
            Brush returnBrush = dataPoint.LabelFontColor;

            if (labelFontColor == null)
            {
                Double intensity;

                if (labelStyle == LabelStyles.Inside && dataPoint.Parent.RenderAs != RenderAs.Line)
                {
                    intensity = Graphics.GetBrushIntensity(dataPoint.Color);
                    returnBrush = Graphics.GetDefaultFontColor(intensity);
                }
                else
                {
                    if (chart.PlotArea.InternalBackground == null)
                    {
                        if (chart.Background == null)
                        {
                            // returnBrush = Graphics.BLACK_BRUSH;
                            returnBrush = Graphics.AUTO_BLACK_FONT_BRUSH;
                        }
                        else
                        {
                            intensity = Graphics.GetBrushIntensity(chart.Background);
                            returnBrush = Graphics.GetDefaultFontColor(intensity);
                        }
                    }
                    else
                    {
                        intensity = Graphics.GetBrushIntensity(chart.PlotArea.InternalBackground);
                        returnBrush = Graphics.GetDefaultFontColor(intensity);
                    }
                }
            }

            return returnBrush;
        }

        /// <summary>
        /// Calculate FontColor for text elements over chart depending upon chart background and dockInsidePlotArea
        /// </summary>
        /// <param name="chart">Chart</param>
        /// <param name="color">FontColor</param>
        /// <param name="dockInsidePlotArea">DockInsidePlotArea</param>
        /// <returns>Brush</returns>
        internal static Brush CalculateFontColor(Chart chart, Brush color, Boolean dockInsidePlotArea)
        {
            Brush brush = color;
            Double intensity;
            if (color == null)
            {
                if (!dockInsidePlotArea)
                {
                    if (chart != null)
                    {
                        if (Graphics.AreBrushesEqual(chart.Background, Graphics.TRANSPARENT_BRUSH) || chart.Background == null)
                        {
                            brush = Graphics.BLACK_BRUSH;
                        }
                        else
                        {
                            intensity = Graphics.GetBrushIntensity(chart.Background);
                            brush = Graphics.GetDefaultFontColor(intensity);
                        }
                    }
                }
                else
                {
                    if (chart.PlotArea != null)
                    {
                        if (Graphics.AreBrushesEqual(chart.PlotArea.InternalBackground, Graphics.TRANSPARENT_BRUSH) || chart.PlotArea.InternalBackground == null)
                        {
                            if (Graphics.AreBrushesEqual(chart.Background, Graphics.TRANSPARENT_BRUSH) || chart.Background == null)
                            {
                                brush = Graphics.BLACK_BRUSH;
                            }
                            else
                            {
                                intensity = Graphics.GetBrushIntensity(chart.Background);
                                brush = Graphics.GetDefaultFontColor(intensity);
                            }
                        }
                        else
                        {
                            intensity = Graphics.GetBrushIntensity(chart.PlotArea.InternalBackground);
                            brush = Graphics.GetDefaultFontColor(intensity);
                        }
                    }
                }
            }
            return brush;
        }

        #endregion

        #region Internal Events And Delegates

#if WPF 
        /// <summary>
        /// RenderDelegate is used for attaching Render() function as target while Invoking Render() using Dispatcher.
        /// </summary>
        internal delegate void RenderDelegate();
#endif

        #endregion
        
        #region Data

        internal Boolean PARTIAL_DP_RENDER_LOCK = false;
        internal Double PARTIAL_RENDER_BLOCKD_COUNT = 0;
        internal Dictionary<DataPoint, VcProperties> _datapoint2UpdatePartially;

        internal Boolean PARTIAL_DS_RENDER_LOCK = false;
        // internal Double PARTIAL_RENDER_BLOCKD_COUNT = 0;
        // internal Dictionary<DataPoint, VcProperties> _datapoint2UpdatePartially;

        /// <summary>
        /// Set to true before calling forced rerender redraw
        /// </summary>
        internal Boolean _forcedRedraw;
        
        private EventHandler _rendered;

        /// <summary>
        /// Whether shadow effect of chart is applied or not
        /// </summary>
        private Boolean _isShadowApplied;

        /// <summary>
        /// Chart area margin
        /// </summary>
        private Thickness _chartAreaMargin;
        
        /// <summary>
        /// Shadow Depth for chart
        /// </summary>
        internal static Double SHADOW_DEPTH = 4;

        /// <summary>
        /// Bevel Depth for chart
        /// </summary>
        internal static Double BEVEL_DEPTH = 5;   

        /// <summary>
        /// Number of time render call is lapsed or failed due to render lock
        /// </summary>
        internal Int32 _renderLapsedCounter;

        /// <summary>
        /// Render lock is used to protect chart from multiple render
        /// </summary>
        internal bool _renderLock = false;
        
        /// <summary>
        /// Is used to handle inactive animation after first time render
        /// </summary>
        internal Boolean _internalAnimationEnabled = false;


        /// <summary>
        /// Whether Theme is changed by the user
        /// </summary>
        internal Boolean _isThemeChanged = false;
        
        #endregion
    }
}
