#if WPF

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Collections.ObjectModel;
using System.Windows.Media.Animation;
#else
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Collections.Generic;
using System.Windows.Markup;
using System.Collections.ObjectModel;

#endif
using System.Windows.Shapes;

using Visifire.Commons;
using System.Windows.Data;

namespace Visifire.Charts
{
#if SL
    [System.Windows.Browser.ScriptableType]
#endif
    public class DataSeries : ObservableObject
    {   
        public DataSeries()
        {
            Binding myBinding = new Binding("BorderThickness");
            myBinding.Source = this;
            myBinding.Mode = BindingMode.TwoWay;
            this.SetBinding(InternalBorderThicknessProperty, myBinding);
            
#if WPF
            if (!_defaultStyleKeyApplied)
            {
                DefaultStyleKeyProperty.OverrideMetadata(typeof(DataSeries), new FrameworkPropertyMetadata(typeof(DataSeries)));
                _defaultStyleKeyApplied = true;
            }
            NameScope.SetNameScope(this, new NameScope());
            //object dsp = this.GetValue(FrameworkElement.DefaultStyleKeyProperty);
            //Style = (Style)Application.Current.FindResource(dsp);
            
#else
            DefaultStyleKey = typeof(DataSeries);
#endif

            // Initialize DataPoints list
            DataPoints = new ObservableCollection<DataPoint>();

            // Attach event handler for the Title collection changed event
            DataPoints.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(DataPoints_CollectionChanged);
        }

        /// <summary>
        /// DataPoints collection changed event handler
        /// </summary>
        /// <param name="sender">DataPoints</param>
        /// <param name="e">NotifyCollectionChangedEventArgs</param>
        void DataPoints_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                if (e.NewItems != null)
                {
                    foreach (DataPoint dataPoint in e.NewItems)
                    {
                        dataPoint.Parent = this;

                        Type str = dataPoint.Parent.GetType();
                        
                        if (Chart != null)
                            dataPoint.Chart = Chart;

                        if (Double.IsNaN(dataPoint.XValue))
                            dataPoint.XValue = this.DataPoints.Count;
                        dataPoint.SetValue(NameProperty, dataPoint.GetType().Name + this.DataPoints.IndexOf(dataPoint));

                        dataPoint.PropertyChanged -= DataPoint_PropertyChanged;
                        dataPoint.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(DataPoint_PropertyChanged);
                    }
                }
            }
            
            this.FirePropertyChanged("DataPoints");
        }
        
#if SL
        public void AddDataPoint(String DataPointXml)
        {
            DataPointXml = String.Format(@"<vc:DataPoint xmlns:vc=""clr-namespace:Visifire.Charts;assembly=SLVisifire.Charts"" xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
             xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""  XValue=""7"" YValue=""10"" />");
            DataPoints.Add((DataPoint)XamlReader.Load(DataPointXml));
        }
#endif

        /// <summary>
        /// Attach tooltip with a framework element
        /// </summary>
        /// <param name="control">Control reference</param>
        /// <param name="elements">FrameworkElements list</param>
        /// <param name="toolTipText">Tooltip text</param>
        public void AttachAreaToolTip(VisifireControl Control, List<FrameworkElement> Elements)
        {
            if (!String.IsNullOrEmpty(ToolTipText))
            {
                // Show ToolTip on mouse move over the chart element
                foreach (FrameworkElement element in Elements)
                {   
                    element.MouseMove += delegate(object sender, MouseEventArgs e)
                    { 
                        Point position = e.GetPosition(this.Faces.Visual);
                        Double xValue = Graphics.PixelPositionToValue(0, this.Faces.Visual.Width, (Double)(Control as Chart).ChartArea.AxisX.AxisManager.AxisMinimumValue, (Double)(Control as Chart).ChartArea.AxisX.AxisManager.AxisMaximumValue, position.X);
                        DataPoint dataPoint = GetNearestDataPoint(xValue);
                        
                        Control._toolTip.Text = dataPoint.TextParser(dataPoint.ToolTipText);
                        (Control as Chart).UpdateToolTipPosition(sender, e);

                        if (Control.ToolTipEnabled)
                            Control._toolTip.Show();
                    };

                    // Hide ToolTip on mouse out from the chart element
                    element.MouseLeave += delegate(object sender, MouseEventArgs e)
                    {
                        //if (Control.ToolTipElement.Visibility == Visibility.Visible)
                        Control._toolTip.Hide();
                    };
                }
            }
        }

        private DataPoint GetNearestDataPoint(Double xValue)
        {
            DataPoint dp = this.DataPoints[0];
            Double diff = Math.Abs(dp.XValue - xValue);

            for (Int32 i = 1; i < this.DataPoints.Count; i++)
            {
                if (Math.Abs(this.DataPoints[i].XValue - xValue) < diff)
                {
                    diff = Math.Abs(this.DataPoints[i].XValue - xValue);
                    dp = this.DataPoints[i];
                }
            }

            return dp;
        }

        /// <summary>
        /// DataPoint property changed event handler
        /// </summary>
        /// <param name="sender">DataSeries</param>
        /// <param name="e">PropertyChangedEventArgs</param>
        void DataPoint_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            this.FirePropertyChanged(e.PropertyName);
        }
        
        #region Public Methods

        #endregion

        #region Public Properties

        /// <summary>
        /// Set the BorderStyle property
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

        public static readonly DependencyProperty EnabledProperty = DependencyProperty.Register
            ("Enabled",
            typeof(Nullable<Boolean>),
            typeof(DataSeries),
            new PropertyMetadata(OnEnabledPropertyChanged));

        private static void OnEnabledPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataSeries dataSeries = d as DataSeries;
            dataSeries.FirePropertyChanged("Enabled");
        }

        /// <summary>
        /// Sets the Chart type
        /// </summary>
        public RenderAs RenderAs
        {
            get
            {
                return (RenderAs)GetValue(RenderAsProperty);
            }
            set
            {   
                SetValue(RenderAsProperty, value);

            }
        }

        public static readonly DependencyProperty RenderAsProperty = DependencyProperty.Register
            ("RenderAs",
            typeof(RenderAs),
            typeof(DataSeries),
            new PropertyMetadata(OnRenderAsPropertyChanged));

        private static void OnRenderAsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataSeries dataSeries = d as DataSeries;
            dataSeries.InternalColor = null;
            dataSeries.FirePropertyChanged("RenderAs");
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
            typeof(DataSeries),
            new PropertyMetadata(OnHrefTargetChanged));

        private static void OnHrefTargetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataSeries dataSeries = d as DataSeries;
            dataSeries.FirePropertyChanged("HrefTarget");
        }

        public String Href
        {
            get
            {
                return (String) GetValue(HrefProperty);
            }
            set
            {
                SetValue(HrefProperty, value);
            }
        }

        public static readonly DependencyProperty HrefProperty = DependencyProperty.Register
            ("Href",
            typeof(String),
            typeof(DataSeries),
            new PropertyMetadata(OnHrefChanged));

        private static void OnHrefChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataSeries dataSeries = d as DataSeries;
            dataSeries.FirePropertyChanged("Href");
        }
        
        /// <summary>
        /// Sets the DataSeries Color
        /// </summary>
        public Brush Color
        {
            get
            {
                if(((Brush)GetValue(DataSeries.ColorProperty) == null))
                    return InternalColor;
                else
                    return (Brush)GetValue(ColorProperty);
            }
            set
            {
                SetValue(ColorProperty, value);
            }
        }

        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register
            ("Color", 
            typeof(Brush), 
            typeof(DataSeries), 
            new PropertyMetadata(OnColorPropertyChanged));
        
        private static void OnColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataSeries dataSeries = d as DataSeries;
            //dataSeries.FirePropertyChanged("Color");
            dataSeries.UpdateVisual("Color", e.NewValue);
        }

        private new Brush Background
        {
            get;
            set;
        }

        /// <summary>
        /// Set the LightingEnabled property
        /// </summary>
        [System.ComponentModel.TypeConverter(typeof(NullableBoolConverter))]
        public Nullable<Boolean> LightingEnabled
        {
            get
            {
                if ((Nullable<Boolean>)GetValue(LightingEnabledProperty) == null)
                {
                    if (this.RenderAs == RenderAs.Bubble)
                        return true;
                    else
                        return false;
                }
                else
                    return (Nullable<Boolean>)GetValue(LightingEnabledProperty);
            }
            set
            {
                SetValue(LightingEnabledProperty, value);
            }
        }

        public static readonly DependencyProperty LightingEnabledProperty = DependencyProperty.Register
            ("LightingEnabled",
            typeof(Nullable<Boolean>),
            typeof(DataSeries),
            new PropertyMetadata(OnLightingEnabledPropertyChanged));

        private static void OnLightingEnabledPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataSeries dataSeries = d as DataSeries;
            dataSeries.FirePropertyChanged("LightingEnabled");
        }

        /// <summary>
        /// Set the ShadowEnabled property
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

        public static readonly DependencyProperty ShadowEnabledProperty = DependencyProperty.Register
            ("ShadowEnabled",
            typeof(Boolean),
            typeof(DataSeries),
            new PropertyMetadata(OnShadowEnabledPropertyChanged));

        private static void OnShadowEnabledPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataSeries dataSeries = d as DataSeries;
            dataSeries.FirePropertyChanged("ShadowEnabled");
        }


        internal String InternalLegendName
        {
            get;
            set;
        }
        
        /// <summary>
        /// Sets the LegendText
        /// </summary>
        public String LegendText
        {
            get
            {
                return (String)GetValue(LegendTextProperty);
            }
            set
            {
                SetValue(LegendTextProperty, value);
            }
        }

        public static readonly DependencyProperty LegendTextProperty = DependencyProperty.Register
            ("LegendText",
            typeof(String),
            typeof(DataSeries),
            new PropertyMetadata(OnLegendTextPropertyChanged));

        private static void OnLegendTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataSeries dataSeries = d as DataSeries;
            dataSeries.FirePropertyChanged("LegendText");
            dataSeries.InternalLegendName = (String) e.NewValue;

        }

        /// <summary>
        /// Sets the Legend for the DataSeries
        /// </summary>
        public String Legend
        {
            get
            {
                return (String.IsNullOrEmpty((String)GetValue(LegendProperty))? "Legend0": (String)GetValue(LegendProperty));
            }
            set
            {
                SetValue(LegendProperty, value);
            }
        }

        public static readonly DependencyProperty LegendProperty = DependencyProperty.Register
            ("Legend",
            typeof(String),
            typeof(DataSeries),
            new PropertyMetadata(OnLegendPropertyChanged));

        private static void OnLegendPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataSeries dataSeries = d as DataSeries;
            dataSeries.FirePropertyChanged("Legend");
        }

        /// <summary>
        /// Bevel effect
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
            typeof(DataSeries),
            new PropertyMetadata(OnBevelPropertyChanged));

        private static void OnBevelPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataSeries dataSeries = d as DataSeries;
            dataSeries.FirePropertyChanged("Bevel");
        }

        /// <summary>
        /// Sets the ColorSet 
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
            typeof(DataSeries),
            new PropertyMetadata(OnColorSetPropertyChanged));

        private static void OnColorSetPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataSeries dataSeries = d as DataSeries;
            dataSeries.FirePropertyChanged("ColorSet");
        }

        /// <summary>
        /// Sets the RadiusX
        /// </summary>

#if WPF
        [System.ComponentModel.TypeConverter(typeof(System.Windows.CornerRadiusConverter))]
#else
       [System.ComponentModel.TypeConverter(typeof(Converters.CornerRadiusConverter))]
#endif
        public CornerRadius RadiusX
        {
            get
            {
                return (CornerRadius)GetValue(RadiusXProperty);
            }
            set
            {
                SetValue(RadiusXProperty, value);
            }
        }

       public static readonly DependencyProperty RadiusXProperty = DependencyProperty.Register
            ("RadiusX",
            typeof(CornerRadius),
            typeof(DataSeries),
            new PropertyMetadata(OnRadiusXPropertyChanged));

        private static void OnRadiusXPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataSeries dataSeries = d as DataSeries;
            dataSeries.FirePropertyChanged("RadiusX");
        }
        /// <summary>
        /// Sets the RadiusY
        /// </summary>

#if WPF
        [System.ComponentModel.TypeConverter(typeof(System.Windows.CornerRadiusConverter))]
#else
        //[System.ComponentModel.TypeConverter(typeof(Converters.CornerRadiusConverter))]
#endif
        public CornerRadius RadiusY
        {
            get
            {
                return (CornerRadius)GetValue(RadiusYProperty);
            }
            set
            {
                SetValue(RadiusYProperty, value);
            }
        }

        public static readonly DependencyProperty RadiusYProperty = DependencyProperty.Register
            ("RadiusY",
            typeof(CornerRadius),
            typeof(DataSeries),
            new PropertyMetadata(OnRadiusYPropertyChanged));

        private static void OnRadiusYPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataSeries dataSeries = d as DataSeries;
            dataSeries.FirePropertyChanged("RadiusY");
        }

        /// <summary>
        /// Sets the LineThickness property
        /// </summary>
#if SL
       [System.ComponentModel.TypeConverter(typeof(Converters.NullableDoubleConverter))]
#endif
        public Nullable<Double> LineThickness
        {
            get
            {
                if ((Nullable<Double>)GetValue(LineThicknessProperty) == null)
                    return (Double)(((Chart as Chart).ActualWidth * (Chart as Chart).ActualHeight) + 25000) / 35000;
                else
                    return (Nullable<Double>)GetValue(LineThicknessProperty);
            }
            set
            {
                SetValue(LineThicknessProperty, value);
            }
        }

       public static readonly DependencyProperty LineThicknessProperty = DependencyProperty.Register
            ("LineThickness",
            typeof(Nullable<Double>),
            typeof(DataSeries),
            new PropertyMetadata(OnLineThicknessPropertyChanged));

        private static void OnLineThicknessPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataSeries dataSeries = d as DataSeries;
            dataSeries.FirePropertyChanged("LineThickness");
        }

        /// <summary>
        /// Sets the LineStyle property
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

        public static readonly DependencyProperty LineStyleProperty = DependencyProperty.Register
            ("LineStyle",
            typeof(LineStyles),
            typeof(DataSeries),
            new PropertyMetadata(OnLineStylePropertyChanged));

        private static void OnLineStylePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataSeries dataSeries = d as DataSeries;
            dataSeries.FirePropertyChanged("LineStyle");
        }

        /// <summary>
        /// Sets the ShowInLegend property
        /// </summary>
        [System.ComponentModel.TypeConverter(typeof(NullableBoolConverter))]
        public Nullable<Boolean> ShowInLegend
        {
            get
            {
                if ((Nullable<Boolean>)GetValue(ShowInLegendProperty) == null)
                {
                    if ((Chart as Chart).Series.Count > 1)
                        return true;
                    else if ((Chart as Chart).Series.Count == 1 && (this.RenderAs == RenderAs.Pie || this.RenderAs == RenderAs.Doughnut))
                        return true;
                    else
                        return false;
                        
                }
                else
                    return (Nullable<Boolean>)GetValue(ShowInLegendProperty);
            }
            set
            {
                SetValue(ShowInLegendProperty, value);
            }
        }

        public static readonly DependencyProperty ShowInLegendProperty = DependencyProperty.Register
            ("ShowInLegend",
            typeof(Nullable<Boolean>),
            typeof(DataSeries),
            new PropertyMetadata(OnShowInLegendPropertyChanged));

        private static void OnShowInLegendPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataSeries dataSeries = d as DataSeries;
            dataSeries.FirePropertyChanged("ShowInLegend");
        }

        #region Label Properties

        /// <summary>
        /// Sets the LabelEnabled property
        /// </summary>
        [System.ComponentModel.TypeConverter(typeof(NullableBoolConverter))]
        public Nullable<Boolean> LabelEnabled
        {
            get
            {
                if (GetValue(LabelEnabledProperty) == null)
                {
                    if (this.RenderAs == RenderAs.Pie || this.RenderAs == RenderAs.Doughnut)
                    {
                        return true;
                    }
                    else
                        return false;
                }
                else
                    return (Nullable<Boolean>)GetValue(LabelEnabledProperty);
            }
            set
            {
                SetValue(LabelEnabledProperty, value);
            }
        }

        public static readonly DependencyProperty LabelEnabledProperty = DependencyProperty.Register
            ("LabelEnabled",
            typeof(Nullable<Boolean>),
            typeof(DataSeries),
            new PropertyMetadata(OnLabelEnabledPropertyChanged));

        private static void OnLabelEnabledPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataSeries dataSeries = d as DataSeries;
            dataSeries.FirePropertyChanged("LabelEnabled");
        }

        /// <summary>
        /// Sets the LabelText property
        /// </summary>
        public String LabelText
        {
            get
            {
                if (String.IsNullOrEmpty((String)GetValue(LabelTextProperty)))
                {
                    if (RenderAs == RenderAs.StackedArea100 || RenderAs == RenderAs.StackedArea100 || RenderAs == RenderAs.StackedArea100)
                    {
                        return "#Sum";
                    }
                    else if (RenderAs == RenderAs.Doughnut || RenderAs == RenderAs.Pie)
                    {
                        return "#Percentage";
                    }
                    else
                    {
                        return "#YValue";
                    }
                }
                else
                    return (String)GetValue(LabelTextProperty);
            }
            set
            {
                SetValue(LabelTextProperty, value);
            }
        }

        public static readonly DependencyProperty LabelTextProperty = DependencyProperty.Register
            ("LabelText",
            typeof(String),
            typeof(DataSeries),
            new PropertyMetadata(OnLabelTextPropertyChanged));

        private static void OnLabelTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataSeries dataSeries = d as DataSeries;
            dataSeries.FirePropertyChanged("LabelText");
        }

        /// <summary>
        /// Sets the LabelFontFamily property
        /// </summary>
        public FontFamily LabelFontFamily
        {
            get
            {
                if (GetValue(LabelFontFamilyProperty) == null)
                    return new FontFamily("Arial");
                else
                    return (FontFamily)GetValue(LabelFontFamilyProperty);
            }
            set
            {
                SetValue(LabelFontFamilyProperty, value);
            }
        }

        public static readonly DependencyProperty LabelFontFamilyProperty = DependencyProperty.Register
            ("LabelFontFamily",
            typeof(FontFamily),
            typeof(DataSeries),
            new PropertyMetadata(OnLabelFontFamilyPropertyChanged));

        private static void OnLabelFontFamilyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataSeries dataSeries = d as DataSeries;
            dataSeries.FirePropertyChanged("LabelFontFamily");
        }

        /// <summary>
        /// Sets the LabelFontSize property
        /// </summary>
#if SL
        [System.ComponentModel.TypeConverter(typeof(Converters.NullableDoubleConverter))]
#endif
        public Nullable<Double> LabelFontSize
        {
            get
            {
                if ((Nullable<Double>)GetValue(LabelFontSizeProperty) == null)
                    return 10;
                else
                    return (Nullable<Double>)GetValue(LabelFontSizeProperty);
            }
            set
            {
                SetValue(LabelFontSizeProperty, value);
            }
        }

        public static readonly DependencyProperty LabelFontSizeProperty = DependencyProperty.Register
            ("LabelFontSize",
            typeof(Nullable<Double>),
            typeof(DataSeries),
            new PropertyMetadata(OnLabelFontSizePropertyChanged));

        private static void OnLabelFontSizePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataSeries dataSeries = d as DataSeries;
            dataSeries.FirePropertyChanged("LabelFontSize");
        }

        /// <summary>
        /// Sets the LabelFontColor property
        /// </summary>
        public Brush LabelFontColor
        {
            get
            {
                return (Brush)GetValue(LabelFontColorProperty);
            }
            set
            {
                SetValue(LabelFontColorProperty, value);
            }
        }

        public static readonly DependencyProperty LabelFontColorProperty = DependencyProperty.Register
            ("LabelFontColor",
            typeof(Brush),
            typeof(DataSeries),
            new PropertyMetadata(OnLabelFontColorPropertyChanged));

        private static void OnLabelFontColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataSeries dataSeries = d as DataSeries;
            dataSeries.FirePropertyChanged("LabelFontColor");
        }

        /// <summary>
        /// Sets the LabelFontWeight property
        /// </summary>
#if SL
        [System.ComponentModel.TypeConverter(typeof(Visifire.Commons.Converters.FontWeightConverter))]
#endif
        public FontWeight LabelFontWeight
        {
            get
            {
                return (FontWeight)GetValue(LabelFontWeightProperty);
            }
            set
            {
                SetValue(LabelFontWeightProperty, value);
            }
        }

        public static readonly DependencyProperty LabelFontWeightProperty = DependencyProperty.Register
            ("LabelFontWeight",
            typeof(FontWeight),
            typeof(DataSeries),
            new PropertyMetadata(OnLabelFontWeightPropertyChanged));

        private static void OnLabelFontWeightPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataSeries dataSeries = d as DataSeries;
            dataSeries.FirePropertyChanged("LabelFontWeight");
        }

        /// <summary>
        /// Sets the LabelFontStyle property
        /// </summary>
#if SL
        [System.ComponentModel.TypeConverter(typeof(Visifire.Commons.Converters.FontStyleConverter))]
#endif
        public FontStyle LabelFontStyle
        {
            get
            {
                return (FontStyle)GetValue(LabelFontStyleProperty);
            }
            set
            {
                SetValue(LabelFontStyleProperty, value);
            }
        }

        public static readonly DependencyProperty LabelFontStyleProperty = DependencyProperty.Register
            ("LabelFontStyle",
            typeof(FontStyle),
            typeof(DataSeries),
            new PropertyMetadata(OnLabelFontStylePropertyChanged));

        private static void OnLabelFontStylePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataSeries dataSeries = d as DataSeries;
            dataSeries.FirePropertyChanged("LabelFontStyle");
        }

        /// <summary>
        /// Sets the LabelBackground property
        /// </summary>
        public Brush LabelBackground
        {
            get
            {
                if (GetValue(LabelBackgroundProperty) != null)
                    return (Brush)GetValue(LabelBackgroundProperty);
                else
                    return null;
            }
            set
            {
                SetValue(LabelBackgroundProperty, value);
            }
        }

        public static readonly DependencyProperty LabelBackgroundProperty = DependencyProperty.Register
            ("LabelBackground",
            typeof(Brush),
            typeof(DataSeries),
            new PropertyMetadata(OnLabelBackgroundPropertyChanged));

        private static void OnLabelBackgroundPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataSeries dataSeries = d as DataSeries;
            dataSeries.FirePropertyChanged("LabelBackground");
        }

        /// <summary>
        /// Sets the LabelStyle property
        /// </summary>
#if SL
       [System.ComponentModel.TypeConverter(typeof(Converters.NullableLabelStylesConverter))]
#endif
        public Nullable<LabelStyles> LabelStyle
        {
            get
            {   
                if ((Nullable<LabelStyles>)GetValue(LabelStyleProperty) == null)
                {
                   switch (RenderAs)
                    {
                        case RenderAs.StackedColumn:
                        case RenderAs.StackedBar:
                        case RenderAs.StackedArea:
                        case RenderAs.StackedColumn100:
                        case RenderAs.StackedBar100:
                        case RenderAs.StackedArea100:
                            return LabelStyles.Inside;
                        default:
                            return LabelStyles.OutSide;
                    }
                }
                else
                    return (Nullable<LabelStyles>)GetValue(LabelStyleProperty);
            }
            set
            {
                SetValue(LabelStyleProperty, value);
            }
        }

       public static readonly DependencyProperty LabelStyleProperty = DependencyProperty.Register
            ("LabelStyle",
            typeof(Nullable<LabelStyles>),
            typeof(DataSeries),
            new PropertyMetadata(OnLabelStylePropertyChanged));

        private static void OnLabelStylePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataSeries dataSeries = d as DataSeries;
            dataSeries.FirePropertyChanged("LabelStyle");
        }

        /// <summary>
        /// Sets the LabelLineEnabled property
        /// </summary>
        [System.ComponentModel.TypeConverter(typeof(NullableBoolConverter))]
        public Nullable<Boolean>LabelLineEnabled
        {
            get
            {
                if ((Boolean)LabelEnabled)
                {
                    //if (GetValue(LabelLineEnabledProperty) == null)
                    //{
                    //    //if (this.RenderAs == RenderAs.Pie || this.RenderAs == RenderAs.Doughnut)
                    //    //    return true;
                    //}
                    //else 
                        return (Nullable<Boolean>)GetValue(LabelLineEnabledProperty);
                }
                return false;
            }
            set
            {
                SetValue(LabelLineEnabledProperty, value);
            }
        }

        public static readonly DependencyProperty LabelLineEnabledProperty = DependencyProperty.Register
            ("LabelLineEnabled",
            typeof(Nullable<Boolean>),
            typeof(DataSeries),
            new PropertyMetadata(OnLabelLineEnabledPropertyChanged));

        private static void OnLabelLineEnabledPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataSeries dataSeries = d as DataSeries;
            dataSeries.FirePropertyChanged("LabelLineEnabled");
        }

        /// <summary>
        /// Sets the LabelLineColor property
        /// </summary>
        public Brush LabelLineColor
        {
            get
            {
                if (GetValue(LabelLineColorProperty) != null)
                    return (Brush)GetValue(LabelLineColorProperty);
                else
                    return new SolidColorBrush(Colors.Gray);
            }
            set
            {
                SetValue(LabelLineColorProperty, value);
            }
        }

        public static readonly DependencyProperty LabelLineColorProperty = DependencyProperty.Register
            ("LabelLineColor",
            typeof(Brush),
            typeof(DataSeries),
            new PropertyMetadata(OnLabelLineColorPropertyChanged));

        private static void OnLabelLineColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataSeries dataSeries = d as DataSeries;
            dataSeries.FirePropertyChanged("LabelLineColor");
        }

        /// <summary>
        /// Sets the LabelLineThickness property
        /// </summary>
        public Double LabelLineThickness
        {
            get
            {
                return (Double)GetValue(LabelLineThicknessProperty);
            }
            set
            {
                SetValue(LabelLineThicknessProperty, value);
            }

        }

        public static readonly DependencyProperty LabelLineThicknessProperty = DependencyProperty.Register
            ("LabelLineThickness",
            typeof(Double),
            typeof(DataSeries),
            new PropertyMetadata(OnLabelLineThicknessPropertyChanged));

        private static void OnLabelLineThicknessPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataSeries dataSeries = d as DataSeries;
            dataSeries.FirePropertyChanged("LabelLineThickness");
        }

        /// <summary>
        /// Sets the LabelLineStyle property
        /// </summary>
        public LineStyles LabelLineStyle
        {
            get
            {   
                return (LineStyles)GetValue(LabelLineStyleProperty);
            }
            set
            {
                SetValue(LabelLineStyleProperty, value);
            }
        }

        public static readonly DependencyProperty LabelLineStyleProperty = DependencyProperty.Register
            ("LabelLineStyle",
            typeof(LineStyles),
            typeof(DataSeries),
            new PropertyMetadata(OnLabelLineStylePropertyChanged));

        private static void OnLabelLineStylePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataSeries dataSeries = d as DataSeries;
            dataSeries.FirePropertyChanged("LabelLineStyle");
        }

        #endregion

        #region Marker Properties

        /// <summary>
        /// Sets the MarkerEnabled property
        /// </summary>
        [System.ComponentModel.TypeConverter(typeof(NullableBoolConverter))]
        public Nullable<Boolean> MarkerEnabled
        {
            get
            {   
                if (this.RenderAs == RenderAs.Line)
                    return ((Nullable<Boolean>)GetValue(MarkerEnabledProperty) == null) ? true : (Nullable<Boolean>)GetValue(MarkerEnabledProperty);
                else
                    return ((Nullable<Boolean>)GetValue(MarkerEnabledProperty) == null) ? false : (Nullable<Boolean>)GetValue(MarkerEnabledProperty);
            }
            set
            {   
                SetValue(MarkerEnabledProperty, value);
            }
        }

        public static readonly DependencyProperty MarkerEnabledProperty = DependencyProperty.Register
            ("MarkerEnabled",
            typeof(Nullable<Boolean>),
            typeof(DataSeries),
            new PropertyMetadata(OnMarkerEnabledPropertyChanged));

        private static void OnMarkerEnabledPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataSeries dataSeries = d as DataSeries;
            dataSeries.FirePropertyChanged("MarkerEnabled");
        }

        /// <summary>
        /// Sets the MarkerStyle property
        /// </summary>
        public MarkerTypes MarkerType
        {
            get
            {
                return (MarkerTypes)GetValue(MarkerTypeProperty);
            }
            set
            {
                SetValue(MarkerTypeProperty, value);
            }
        }

        public static readonly DependencyProperty MarkerTypeProperty = DependencyProperty.Register
            ("MarkerType",
            typeof(MarkerTypes),
            typeof(DataSeries),
            new PropertyMetadata(OnMarkerTypePropertyChanged));

        private static void OnMarkerTypePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataSeries dataSeries = d as DataSeries;
            dataSeries.FirePropertyChanged("MarkerType");
        }

        /// <summary>
        /// Sets the MarkerBorderThickness property
        /// </summary>
#if SL
        [System.ComponentModel.TypeConverter(typeof(Converters.NullableThicknessConverter))]
#endif
        public Nullable<Thickness>MarkerBorderThickness
        {
            get
            {
                return (Nullable<Thickness>)GetValue(MarkerBorderThicknessProperty);
            }
            set
            {
                SetValue(MarkerBorderThicknessProperty, value);
            }
        }

        public static readonly DependencyProperty MarkerBorderThicknessProperty = DependencyProperty.Register
            ("MarkerBorderThickness",
            typeof(Nullable<Thickness>),
            typeof(DataSeries),
            new PropertyMetadata(OnMarkerBorderThicknessPropertyChanged));

        private static void OnMarkerBorderThicknessPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataSeries dataSeries = d as DataSeries;
            dataSeries.FirePropertyChanged("MarkerBorderThickness");
        }

        /// <summary>
        /// Sets the MarkerBorderColor property
        /// </summary>
        public Brush MarkerBorderColor
        {
            get
            {
                return (Brush)GetValue(MarkerBorderColorProperty);
            }
            set
            {
                SetValue(MarkerBorderColorProperty, value);
            }
        }

        public static readonly DependencyProperty MarkerBorderColorProperty = DependencyProperty.Register
            ("MarkerBorderColor",
            typeof(Brush),
            typeof(DataSeries),
            new PropertyMetadata(OnMarkerBorderColorPropertyChanged));

        private static void OnMarkerBorderColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataSeries dataSeries = d as DataSeries;
            dataSeries.FirePropertyChanged("MarkerBorderColor");
        }

        /// <summary>
        /// Sets the MarkerSize property
        /// </summary>
#if SL
        [System.ComponentModel.TypeConverter(typeof(Converters.NullableDoubleConverter))]
#endif
        public Nullable<Double> MarkerSize
        {
            get
            {
                if ((Nullable<Double>)GetValue(MarkerSizeProperty) == null)
                    if (this.RenderAs == RenderAs.Line)
                        return (Nullable<Double>)(this.LineThickness + (this.LineThickness * 80 / 100));

                    else
                        return 8;
                else
                    return (Nullable<Double>)GetValue(MarkerSizeProperty);
            }
            set
            {
                SetValue(MarkerSizeProperty, value);
            }
        }

        public static readonly DependencyProperty MarkerSizeProperty = DependencyProperty.Register
            ("MarkerSize",
            typeof(Nullable<Double>),
            typeof(DataSeries),
            new PropertyMetadata(OnMarkerSizePropertyChanged));

        private static void OnMarkerSizePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataSeries dataSeries = d as DataSeries;
            dataSeries.FirePropertyChanged("MarkerSize");
        }

        /// <summary>
        /// Sets the MarkerColor property
        /// </summary>
        public Brush MarkerColor
        {
            get
            {
                return (Brush)GetValue(MarkerColorProperty);
            }
            set
            {
                SetValue(MarkerColorProperty, value);
            }
        }

        public static readonly DependencyProperty MarkerColorProperty = DependencyProperty.Register
            ("MarkerColor",
            typeof(Brush),
            typeof(DataSeries),
            new PropertyMetadata(OnMarkerColorPropertyChanged));

        private static void OnMarkerColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataSeries dataSeries = d as DataSeries;
            dataSeries.FirePropertyChanged("MarkerColor");
        }

#if SL
       [System.ComponentModel.TypeConverter(typeof(Converters.NullableDoubleConverter))]
#endif
        public Nullable<Double> MarkerScale
        {
            get
            {
                if ((Nullable<Double>)GetValue(MarkerScaleProperty) == null)
                {
                    if (RenderAs == RenderAs.Bubble)
                    {
                        Double x = Math.Abs(Chart.ActualHeight - Chart.ActualWidth);
                        x = x / 100;
                        return 2 + Math.Sqrt(x);
                    }
                    else
                        return 1;
                }
                else
                    return (Nullable<Double>)GetValue(MarkerScaleProperty);
            }
            set
            {
                SetValue(MarkerScaleProperty, value);
            }
        }

       public static readonly DependencyProperty MarkerScaleProperty = DependencyProperty.Register
            ("MarkerScale",
            typeof(Nullable<Double>),
            typeof(DataSeries),
            new PropertyMetadata(OnMarkerScalePropertyChanged));

        private static void OnMarkerScalePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataSeries dataSeries = d as DataSeries;
            dataSeries.FirePropertyChanged("MarkerScale");
        }

        #endregion Marker Properties

        /// <summary>
        /// Sets the StartAngle property. This property is generally used in Pie/Doughnut.
        /// </summary>
        public Double StartAngle
        {
            get
            {
                return (Double)GetValue(StartAngleProperty);
            }
            set
            {
                if(value < 0 || value > 360)
                    throw (new Exception("Invalid property value:: StartAngle should be greater than 0 and less than 360."));
               // value = (value % 360) * (Math.PI / 180);
                SetValue(StartAngleProperty, value);
            }
        }

        public static readonly DependencyProperty StartAngleProperty = DependencyProperty.Register
            ("StartAngle",
            typeof(Double),
            typeof(DataSeries),
            new PropertyMetadata(OnStartAnglePropertyChanged));

        private static void OnStartAnglePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataSeries dataSeries = d as DataSeries;
            dataSeries.FirePropertyChanged("StartAngle");
        }

        #region BorderProperties

        /// <summary>
        /// Set the BorderThickness property
        /// </summary>
        internal Thickness InternalBorderThickness
        {
            get
            {
                return (Thickness)GetValue(InternalBorderThicknessProperty);
            }
            set
            {   
                SetValue(InternalBorderThicknessProperty, value);
            }
        }

        public static readonly DependencyProperty InternalBorderThicknessProperty = DependencyProperty.Register
            ("InternalBorderThickness",
            typeof(Thickness),
            typeof(DataSeries),
            new PropertyMetadata(OnBorderThicknessPropertychanged));

        private static void OnBorderThicknessPropertychanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataSeries dataSeries = d as DataSeries;
            dataSeries.FirePropertyChanged("BorderThickness");
        }

        /// <summary>
        /// Set the BorderColor property
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

        public static readonly DependencyProperty BorderColorProperty = DependencyProperty.Register
            ("BorderColor",
            typeof(Brush),
            typeof(DataSeries),
            new PropertyMetadata(OnBorderColorPropertychanged));

        private static void OnBorderColorPropertychanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataSeries dataSeries = d as DataSeries;
            dataSeries.FirePropertyChanged("BorderColor");
        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        private new Brush BorderBrush
        {
            get;
            set;
        }

        /// <summary>
        /// Set the BorderStyle property
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
            typeof(DataSeries),
            new PropertyMetadata(OnBorderStylePropertychanged));

        private static void OnBorderStylePropertychanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataSeries dataSeries = d as DataSeries;
            dataSeries.FirePropertyChanged("BorderStyle");
        }

        #endregion

        /// <summary>
        /// Sets the XValueFormatString
        /// </summary>
        public String XValueFormatString
        {
            get
            {
                return (String)GetValue(XValueFormatStringProperty);
            }
            set
            {
                SetValue(XValueFormatStringProperty, value);
            }
        }

        public static readonly DependencyProperty XValueFormatStringProperty = DependencyProperty.Register
            ("XValueFormatString",
            typeof(String),
            typeof(DataSeries),
            new PropertyMetadata(OnXValueFormatStringPropertyChanged));

        private static void OnXValueFormatStringPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataSeries dataSeries = d as DataSeries;
            dataSeries.FirePropertyChanged("XValueFormatString");
        }

        /// <summary>
        /// Sets the YValueFormatString
        /// </summary>
        public String YValueFormatString
        {
            get
            {
                return (String)GetValue(YValueFormatStringProperty);
            }
            set
            {
                SetValue(YValueFormatStringProperty, value);
            }
        }

        public static readonly DependencyProperty YValueFormatStringProperty = DependencyProperty.Register
            ("YValueFormatString",
            typeof(String),
            typeof(DataSeries),
            new PropertyMetadata(OnYValueFormatStringPropertyChanged));

        private static void OnYValueFormatStringPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataSeries dataSeries = d as DataSeries;
            dataSeries.FirePropertyChanged("YValueFormatString");
        }

        /// <summary>
        /// Sets the ZValueFormatString
        /// </summary>
        public String ZValueFormatString
        {
            get
            {
                if (String.IsNullOrEmpty((String)GetValue(ZValueFormatStringProperty)))
                    return "###,##0.##";
                else
                    return (String)GetValue(ZValueFormatStringProperty);

            }
            set
            {
                SetValue(ZValueFormatStringProperty, value);
            }
        }

        public static readonly DependencyProperty ZValueFormatStringProperty = DependencyProperty.Register
            ("ZValueFormatString",
            typeof(String),
            typeof(DataSeries),
            new PropertyMetadata(OnZValueFormatStringPropertyChanged));

        private static void OnZValueFormatStringPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataSeries dataSeries = d as DataSeries;
            dataSeries.FirePropertyChanged("ZValueFormatString");
        }

        /// <summary>
        /// Sets the ToolTipText for the DataSeries
        /// </summary>
        public override String ToolTipText
        {
            get
            {
                if (String.IsNullOrEmpty((String)GetValue(ToolTipTextProperty)))
                {
                    switch (RenderAs)
                    {
                        case RenderAs.StackedColumn100:
                        case RenderAs.StackedBar100:
                        case RenderAs.StackedArea100:
                            return "#AxisXLabel, #YValue(#Sum)";

                        case RenderAs.Pie:
                        case RenderAs.Doughnut:
                            return "#AxisXLabel, #YValue(#Percentage%)";

                        default:
                            return "#AxisXLabel, #YValue";
                    }
                }
                else
                    return (String) GetValue(ToolTipTextProperty);
            }
            set
            {
                SetValue(ToolTipTextProperty, value);
            }
        }



        /// <summary>
        /// Collection of DataPoints
        /// </summary>
        public ObservableCollection<DataPoint> DataPoints
        {
            get;
            set;
        }

        /// <summary>
        /// Sets the AxisX Type
        /// </summary>
        public AxisTypes AxisXType
        {
            get
            {
                return (AxisTypes)GetValue(AxisXTypeProperty);
            }
            set
            {
                SetValue(AxisXTypeProperty, value);
            }
        }

        public static readonly DependencyProperty AxisXTypeProperty = DependencyProperty.Register
            ("AxisXType",
            typeof(AxisTypes),
            typeof(DataSeries),
            new PropertyMetadata(OnAxisXTypePropertyChanged));

        private static void OnAxisXTypePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataSeries dataSeries = d as DataSeries;
            dataSeries.FirePropertyChanged("AxisXType");
        }

        /// <summary>
        /// Sets the AxisY Type
        /// </summary>
        public AxisTypes AxisYType
        {
            get
            {
                return (AxisTypes)GetValue(AxisYTypeProperty);
            }
            set
            {
                SetValue(AxisYTypeProperty, value);
            }
        }

        public static readonly DependencyProperty AxisYTypeProperty = DependencyProperty.Register
            ("AxisYType",
            typeof(AxisTypes),
            typeof(DataSeries),
            new PropertyMetadata(OnAxisYTypePropertyChanged));

        private static void OnAxisYTypePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataSeries dataSeries = d as DataSeries;
            dataSeries.FirePropertyChanged("AxisYType");
        }

        #endregion

        #region Public Events

        #endregion

        #region Protected Methods

        internal override void UpdateVisual(String PropertyName, object Value)
        {
            if (PropertyName == "Color")
            {
                if (RenderAs == RenderAs.Area || RenderAs == RenderAs.StackedArea || RenderAs == RenderAs.StackedArea100)
                {
                    if (Faces != null && Faces.Parts != null)
                    {

                        if ((Chart as Chart).View3D)
                        {
                            Brush sideBrush = (Boolean)LightingEnabled ? AreaChart.GetRightFaceBrush((Brush)Value) : (Brush)Value;
                            Brush topBrush = (Boolean)LightingEnabled ? Graphics.GetTopFaceBrush((Brush)Value) : (Brush)Value;

                            foreach (FrameworkElement fe in Faces.Parts)
                            {
                                if (fe.Tag.ToString() == "AreaBase")
                                    (fe as Shape).Fill = (Boolean)LightingEnabled ? Graphics.GetFrontFaceBrush((Brush)Value) : (Brush)Value;
                                else if (fe.Tag.ToString() == "Side")
                                    (fe as Shape).Fill = sideBrush;
                                else if (fe.Tag.ToString() == "Top")
                                    (fe as Shape).Fill = topBrush;
                            }
                        }
                        else
                        {   
                            foreach (FrameworkElement fe in Faces.Parts)
                            {
                                if (fe.Tag.ToString() == "AreaBase")
                                {
                                    (fe as Shape).Fill = (Boolean)LightingEnabled ? Graphics.GetLightingEnabledBrush((Brush)Value, "Linear", null) : (Brush)Value;
                                }
                                else if (fe.Tag.ToString() == "Bevel")
                                {
                                    (fe as Shape).Fill = Graphics.GetBevelTopBrush((Brush)Value);
                                }
                            }
                        }
                    }
                }
                else
                    foreach (DataPoint dp in DataPoints)
                        dp.UpdateVisual("Color", null);

                if (LegendMarker != null && LegendMarker.Visual != null)
                {
                    LegendMarker.BorderColor = (Brush)Color;
                    LegendMarker.MarkerFillColor = (Brush)Color;
                    LegendMarker.UpdateMarker();
                }
            }
            else
                FirePropertyChanged("Color");
        }

        #endregion

        #region Internal Properties

        internal Marker LegendMarker
        {
            get;
            set;
        }

        /// <summary>
        /// This storyboard has to be used for animating the DataSeries
        /// </summary>
        internal Storyboard Storyboard
        {
            get
            {
                return _storyboard;
            }
            set
            {
                _storyboard = value;
            }
        }
        /// <summary>
        /// Total Count of DataSeries of the group to which this series belongs
        /// This count is helpfun while rendering, the space allocated for the columns at a particular XValue 
        /// must be divided between indivisual datapoints of different series with same XValue
        /// </summary>
        internal Int32 SeriesCountOfSameRenderAs
        {
            get;
            set;
        }

        /// <summary>
        /// Will be used to decide which series comes in fornt which one goes back
        /// </summary>
        public Int32 ZIndex
        {
            get;
            set;
        }

        internal Faces Faces
        {
            get;
            set;
        }


        internal PlotGroup PlotGroup
        {
            get;
            set;
        }
        #endregion

        #region Private Delegates

        #endregion

        #region Private Methods

        #endregion

        #region Internal Methods

        /// <summary>
        /// Attach events to each and every individual face of Faces
        /// </summary>
        /// <param name="faces"></param>
        internal void AttachEvent2DataSeriesVisualFaces()
        {
            foreach (DataPoint dp in DataPoints)
            {
                dp.AttachEvent2DataPointVisualFaces(this);
            }
        }
        
        #endregion

        #region Internal Events

        #endregion

        #region Data

        internal Brush InternalColor;   

        private Storyboard _storyboard;

#if WPF
        private static Boolean _defaultStyleKeyApplied = false;            // Default Style key
#endif        
        #endregion

    }

}
