#if WPF

using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
#else
using System;
using System.Windows;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;


#endif

using Visifire.Commons;

namespace Visifire.Charts
{
#if SL
    [System.Windows.Browser.ScriptableType]
#endif
    public class DataPoint : ObservableObject
    {
        #region Public Methods

        public DataPoint()
        {
            XValue = Double.NaN;
            YValue = Double.NaN;
            ZValue = Double.NaN;
#if WPF
            if (!_defaultStyleKeyApplied)
            {
                DefaultStyleKeyProperty.OverrideMetadata(typeof(DataPoint), new FrameworkPropertyMetadata(typeof(DataPoint)));
                _defaultStyleKeyApplied = true;
            } 

            NameScope.SetNameScope(this, new NameScope());

#else
            DefaultStyleKey = typeof(DataPoint);
#endif

        }

        public Cursor GetCursor()
        {
            if (this.Cursor == null)
            {   
                if (_parent.Cursor == null)
                    return Cursors.Arrow;
                else
                    return _parent.Cursor;
            }
            else
            {
                return this.Cursor;
            }
        }
        
        #endregion

        #region Public Properties

#if SL
        [System.ComponentModel.TypeConverter(typeof(Converters.NullableHrefTargetsConverter))]
#endif
        public Nullable<HrefTargets> HrefTarget
        {
            get
            {
                if ((Nullable<HrefTargets>)GetValue(HrefTargetProperty) == null)
                    return _parent.HrefTarget;
                else
                    return (Nullable<HrefTargets>)GetValue(HrefTargetProperty);
            }
            set
            {
                SetValue(HrefTargetProperty, value);
            }
        }

        public static readonly DependencyProperty HrefTargetProperty = DependencyProperty.Register
            ("HrefTarget",
            typeof(Nullable<HrefTargets>), 
            typeof(DataPoint), 
            new PropertyMetadata(OnHrefTargetChanged));

        private static void OnHrefTargetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {   
            DataPoint dataPoint = d as DataPoint;
            dataPoint.FirePropertyChanged("HrefTarget");
        }

        public String Href
        {
            get
            {
                return (String)(!String.IsNullOrEmpty((String)GetValue(HrefProperty)) ? GetValue(HrefProperty) : Parent.GetValue(DataSeries.HrefProperty));
            }
            set
            {
                SetValue(HrefProperty, value);
            }
        }

        public static readonly DependencyProperty HrefProperty = DependencyProperty.Register
            ("Href", 
            typeof(String), 
            typeof(DataPoint), 
            new PropertyMetadata(OnHrefChanged));

        private static void OnHrefChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataPoint dataPoint = d as DataPoint;
            dataPoint.FirePropertyChanged("Href");
        }
        
        /// <summary>
        /// Sets the value that will appear on Y-Axis for all charts.
        /// In the case of Pie and Doughnut, the YValue will be considered for calculating the percentages.
        /// </summary>
        public Double YValue
        {
            get
            {
               return (Double)GetValue(YValueProperty);
            }
            set
            {
                SetValue(YValueProperty, value);
            }
        }

        private static readonly DependencyProperty YValueProperty = DependencyProperty.Register
            ("YValue",
            typeof(Double),
            typeof(DataPoint),
            new PropertyMetadata(OnYValuePropertyChanged));

        private static void OnYValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataPoint dataPoint = d as DataPoint;
            dataPoint.FirePropertyChanged("YValue");
        }

        /// <summary>
        /// Sets the value that will appear on Y-Axis for all charts. 
        /// In the case of Pie and Doughnut, the YValue will be considered for calculating the percentages.
        /// </summary>
        public Double XValue
        {
            get
            {
                return (Double)GetValue(XValueProperty);
            }
            set
            {
                SetValue(XValueProperty, value);
            }
        }

        private static readonly DependencyProperty XValueProperty = DependencyProperty.Register
            ("XValue",
            typeof(Double),
            typeof(DataPoint),
            new PropertyMetadata(OnXValuePropertyChanged));

        private static void OnXValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataPoint dataPoint = d as DataPoint;
            dataPoint.FirePropertyChanged("XValue");
        }

        /// <summary>
        /// Sets the value that will appear for bubble charts only
        /// </summary>
        public Double ZValue
        {
            get
            {
                return (Double)GetValue(ZValueProperty);
            }
            set
            {
                SetValue(ZValueProperty, value);
            }
        }

        private static readonly DependencyProperty ZValueProperty = DependencyProperty.Register
            ("ZValue",
            typeof(Double),
            typeof(DataPoint),
            new PropertyMetadata(OnZValuePropertyChanged));

        private static void OnZValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataPoint dataPoint = d as DataPoint;
            dataPoint.FirePropertyChanged("ZValue");
        }

        /// <summary>
        /// Label to be placed for the corresponding XValue in the AxisX
        /// </summary>
        public String AxisXLabel
        {
            get
            {
                return (String)GetValue(AxisXLabelProperty);
            }
            set
            {
                SetValue(AxisXLabelProperty, value);
            }
        }

        private static readonly DependencyProperty AxisXLabelProperty = DependencyProperty.Register
            ("AxisXLabel",
            typeof(String),
            typeof(DataPoint),
            new PropertyMetadata(OnAxisXLabelPropertyChanged));

        private static void OnAxisXLabelPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataPoint dataPoint = d as DataPoint;
            dataPoint.FirePropertyChanged("AxisXLabel");
        }

        /// <summary>
        /// Sets the color of the DataPoint
        /// </summary>
        /// <summary>
        /// Sets the DataPoint Color
        /// </summary>
        public Brush Color
        {
            get
            {
                if ((Brush)GetValue(ColorProperty) == null)
                    return (_parent.Color == null)? InternalColor : _parent.Color;
                else
                    return (Brush)GetValue(ColorProperty);
            }
            set
            {
                SetValue(ColorProperty, value);
            }
        }

        internal static readonly DependencyProperty ColorProperty = DependencyProperty.Register
            ("Color",
            typeof(Brush),
            typeof(DataPoint),
            new PropertyMetadata(OnColorPropertyChanged));

        private static void OnColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataPoint dataPoint = d as DataPoint;
            dataPoint.UpdateVisual("Color", e.NewValue);
            //dataPoint.FirePropertyChanged("Color");
        }

        /// <summary>
        /// Set the BorderStyle property
        /// </summary>
        [System.ComponentModel.TypeConverter(typeof(NullableBoolConverter))]
        public Nullable<Boolean> Enabled
        {   
            get
            {
                if ((Nullable<Boolean>)GetValue(EnabledProperty) == null)
                    return Parent.Enabled;
                else
                    return (Nullable<Boolean>)GetValue(EnabledProperty);
            }
            set
            {
                SetValue(EnabledProperty, value);
            }
        }

        private static readonly DependencyProperty EnabledProperty = DependencyProperty.Register
            ("Enabled",
            typeof(Nullable<Boolean>),
            typeof(DataPoint),
            new PropertyMetadata(OnEnabledPropertyChanged));
            
        private static void OnEnabledPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataPoint dataPoint = d as DataPoint;
            dataPoint.FirePropertyChanged("Enabled");
        }

        /// <summary>
        /// Sets the ExplodeOffset. This is used in Pie/Doughnut charts.
        /// </summary>
        public Boolean Exploded
        {
            get
            {
                return (Boolean)GetValue(ExplodedProperty);
            }
            set
            {
                SetValue(ExplodedProperty, value);
            }
        }

        private static readonly DependencyProperty ExplodedProperty = DependencyProperty.Register
            ("Exploded",
            typeof(Boolean),
            typeof(DataPoint),
            new PropertyMetadata(OnExplodedPropertyChanged));

        private static void OnExplodedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataPoint dataPoint = d as DataPoint;
            dataPoint.FirePropertyChanged("Exploded");
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
                    return _parent.LightingEnabled;
                }
                else
                {
                    return (Nullable<Boolean>)GetValue(LightingEnabledProperty);
                }
            }
            set
            {
                SetValue(LightingEnabledProperty, value);
            }
        }

        private static readonly DependencyProperty LightingEnabledProperty = DependencyProperty.Register
            ("LightingEnabled",
            typeof(Nullable<Boolean>),
            typeof(DataPoint),
            new PropertyMetadata(OnLightingEnabledPropertyChanged));
            
        private static void OnLightingEnabledPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {   
            DataPoint dataPoint = d as DataPoint;
            dataPoint.FirePropertyChanged("LightingEnabled");
        }
        
        /// <summary>
        /// Set the ShadowEnabled property
        /// </summary>
        [System.ComponentModel.TypeConverter(typeof(NullableBoolConverter))]
        public Nullable<Boolean> ShadowEnabled
        {
            get
            {

                if ((Nullable<Boolean>)GetValue(ShadowEnabledProperty) == null)
                {
                    return _parent.ShadowEnabled;
                }
                else
                {
                    return (Nullable<Boolean>)GetValue(ShadowEnabledProperty);
                }
                
            }
            set
            {
                SetValue(ShadowEnabledProperty, value);
            }
        }

        private static readonly DependencyProperty ShadowEnabledProperty = DependencyProperty.Register
            ("ShadowEnabled",
            typeof(Nullable<Boolean>),
            typeof(DataPoint),
            new PropertyMetadata(OnShadowEnabledPropertyChanged));

        private static void OnShadowEnabledPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataPoint dataPoint = d as DataPoint;
            dataPoint.FirePropertyChanged("ShadowEnabled");
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
                    return _parent.LabelEnabled;
                else
                    return (Nullable<Boolean>)GetValue(LabelEnabledProperty);
            }
            set
            {
                SetValue(LabelEnabledProperty, value);
            }
        }

        private static readonly DependencyProperty LabelEnabledProperty = DependencyProperty.Register
            ("LabelEnabled",
            typeof(Nullable<Boolean>),
            typeof(DataPoint),
            new PropertyMetadata(OnLabelEnabledPropertyChanged));

        private static void OnLabelEnabledPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataPoint dataPoint = d as DataPoint;
            dataPoint.FirePropertyChanged("LabelEnabled");
        }

        /// <summary>
        /// Sets the LabelText property
        /// </summary>
        public String LabelText
        {
            get
            {
                if (!String.IsNullOrEmpty((String)GetValue(LabelTextProperty)))
                    return (String)GetValue(LabelTextProperty);
                else
                    return _parent.LabelText;
            }
            set
            {
                SetValue(LabelTextProperty, value);
            }
        }

        private static readonly DependencyProperty LabelTextProperty = DependencyProperty.Register
            ("LabelText",
            typeof(String),
            typeof(DataPoint),
            new PropertyMetadata(OnLabelTextPropertyChanged));

        private static void OnLabelTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataPoint dataPoint = d as DataPoint;
            dataPoint.FirePropertyChanged("LabelText");
        }

        /// <summary>
        /// Sets the LabelFontFamily property
        /// </summary>
        public FontFamily LabelFontFamily
        {
            get
            {
                if (GetValue(LabelFontFamilyProperty) != null)
                    return (FontFamily)GetValue(LabelFontFamilyProperty);
                else
                    return _parent.LabelFontFamily;
            }
            set
            {
                SetValue(LabelFontFamilyProperty, value);
            }
        }

        private static readonly DependencyProperty LabelFontFamilyProperty = DependencyProperty.Register
            ("LabelFontFamily",
            typeof(FontFamily),
            typeof(DataPoint),
            new PropertyMetadata(OnLabelFontFamilyPropertyChanged));

        private static void OnLabelFontFamilyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataPoint dataPoint = d as DataPoint;
            dataPoint.FirePropertyChanged("LabelFontFamily");
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
                if ((Nullable<Double>)GetValue(LabelFontSizeProperty) != null)
                    return (Nullable<Double>)GetValue(LabelFontSizeProperty);
                else if (_parent.LabelFontSize != 0)
                    return _parent.LabelFontSize;
                else
                    return 10;
            }
            set
            {
                SetValue(LabelFontSizeProperty, value);
            }
        }

        private static readonly DependencyProperty LabelFontSizeProperty = DependencyProperty.Register
            ("LabelFontSize",
            typeof(Nullable<Double>),
            typeof(DataPoint),
            new PropertyMetadata(OnLabelFontSizePropertyChanged));

        private static void OnLabelFontSizePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataPoint dataPoint = d as DataPoint;
            dataPoint.FirePropertyChanged("LabelFontSize");
        }

        /// <summary>
        /// Sets the LabelFontColor property
        /// </summary>
        public Brush LabelFontColor
        {
            get
            {
                if (GetValue(LabelFontColorProperty) != null)
                    return (Brush)GetValue(LabelFontColorProperty);
                else
                    return _parent.LabelFontColor;
            }
            set
            {
                SetValue(LabelFontColorProperty, value);
            }
        }

        private static readonly DependencyProperty LabelFontColorProperty = DependencyProperty.Register
            ("LabelFontColor",
            typeof(Brush),
            typeof(DataPoint),
            new PropertyMetadata(OnLabelFontColorPropertyChanged));

        private static void OnLabelFontColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataPoint dataPoint = d as DataPoint;
            dataPoint.FirePropertyChanged("LabelFontColor");
        }

        /// <summary>
        /// Sets the LabelFontWeight property
        /// </summary>
#if SL
        [System.ComponentModel.TypeConverter(typeof(Visifire.Commons.Converters.FontWeightConverter))]
#endif 
        public Nullable<FontWeight> LabelFontWeight
        {
            get
            {
                if ((Nullable<FontWeight>)GetValue(LabelFontWeightProperty) != null)
                    return (FontWeight)GetValue(LabelFontWeightProperty);
                else
                    return _parent.LabelFontWeight;
            }
            set
            {
                SetValue(LabelFontWeightProperty, value);
            }
        }

        private static readonly DependencyProperty LabelFontWeightProperty = DependencyProperty.Register
            ("LabelFontWeight",
            typeof(Nullable<FontWeight>),
            typeof(DataPoint),
            new PropertyMetadata(OnLabelFontWeightPropertyChanged));

        private static void OnLabelFontWeightPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataPoint dataPoint = d as DataPoint;
            dataPoint.FirePropertyChanged("LabelFontWeight");
        }

        /// <summary>
        /// Sets the LabelFontStyle property
        /// </summary>
        public Nullable<FontStyle> LabelFontStyle
        {
            get
            {
                if ((Nullable<FontStyle>)GetValue(LabelFontStyleProperty) != null)
                    return (FontStyle)GetValue(LabelFontStyleProperty);
                else
                    return _parent.LabelFontStyle;
            }
            set
            {
                SetValue(LabelFontStyleProperty, value);
            }
        }

        private static readonly DependencyProperty LabelFontStyleProperty = DependencyProperty.Register
            ("LabelFontStyle",
            typeof(Nullable<FontStyle>),
            typeof(DataPoint),
            new PropertyMetadata(OnLabelFontStylePropertyChanged));

        private static void OnLabelFontStylePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataPoint dataPoint = d as DataPoint;
            dataPoint.FirePropertyChanged("LabelFontStyle");
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
                    return _parent.LabelBackground;
            }
            set
            {
                SetValue(LabelBackgroundProperty, value);
            }
        }

        private static readonly DependencyProperty LabelBackgroundProperty = DependencyProperty.Register
            ("LabelBackground",
            typeof(Brush),
            typeof(DataPoint),
            new PropertyMetadata(OnLabelBackgroundPropertyChanged));

        private static void OnLabelBackgroundPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataPoint dataPoint = d as DataPoint;
            dataPoint.FirePropertyChanged("LabelBackground");
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
                if ((Nullable<LabelStyles>)GetValue(LabelStyleProperty) != null)
                    return (Nullable<LabelStyles>)GetValue(LabelStyleProperty);
                else
                    return _parent.LabelStyle;
            }
            set
            {
                SetValue(LabelStyleProperty, value);
            }
        }

        private static readonly DependencyProperty LabelStyleProperty = DependencyProperty.Register
            ("LabelStyle",
            typeof(Nullable<LabelStyles>),
            typeof(DataPoint),
            new PropertyMetadata(OnLabelStylePropertyChanged));

        private static void OnLabelStylePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataPoint dataPoint = d as DataPoint;
            dataPoint.FirePropertyChanged("LabelStyle");
        }

        /// <summary>
        /// Sets the LabelLineEnabled property
        /// </summary>
        [System.ComponentModel.TypeConverter(typeof(NullableBoolConverter))]
        public Nullable<Boolean> LabelLineEnabled
        {
            get
            {   
                if ((Boolean)LabelEnabled)
                {   
                    Nullable<Boolean> retVal = null;
                    if ((Nullable<Boolean>) GetValue(LabelLineEnabledProperty) == null)
                        retVal = (_parent != null) ? _parent.LabelLineEnabled: null;
                    else
                        retVal = (Nullable<Boolean>)GetValue(LabelLineEnabledProperty);
 
                    if(retVal == null)
                        retVal = ((_parent.RenderAs == RenderAs.Pie || _parent.RenderAs == RenderAs.Doughnut) && (LabelStyle == LabelStyles.OutSide)) ? true : false;

                    return retVal;
                }

                return false;
            }
            set
            {
                SetValue(LabelLineEnabledProperty, value);
            }
        }

        private static readonly DependencyProperty LabelLineEnabledProperty = DependencyProperty.Register
            ("LabelLineEnabled",
            typeof(Nullable<Boolean>),
            typeof(DataPoint),
            new PropertyMetadata(OnLabelLineEnabledPropertyChanged));

        private static void OnLabelLineEnabledPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataPoint dataPoint = d as DataPoint;
            dataPoint.FirePropertyChanged("LabelLineEnabled");
        }

        /// <summary>
        /// Sets the LabelLineColor property
        /// </summary>
        public Brush LabelLineColor
        {
            get
            {
                if (GetValue(LabelLineColorProperty) == null)
                    return _parent.LabelLineColor;
                else
                    return (Brush)GetValue(LabelLineColorProperty);
            }
            set
            {
                SetValue(LabelLineColorProperty, value);
            }
        }

        private static readonly DependencyProperty LabelLineColorProperty = DependencyProperty.Register
            ("LabelLineColor",
            typeof(Brush),
            typeof(DataPoint),
            new PropertyMetadata(OnLabelLineColorPropertyChanged));

        private static void OnLabelLineColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataPoint dataPoint = d as DataPoint;
            dataPoint.FirePropertyChanged("LabelLineColor");
        }

        /// <summary>
        /// Sets the LabelLineThickness property
        /// </summary>
#if SL
        [System.ComponentModel.TypeConverter(typeof(Converters.NullableDoubleConverter))]
#endif
        public Nullable<Double> LabelLineThickness
        {
            get
            {
                if ((Nullable<Double>)GetValue(LabelLineThicknessProperty) != null)
                    return (Double)GetValue(LabelLineThicknessProperty);
                else if (_parent.LabelLineThickness != 0)
                    return _parent.LabelLineThickness;
                else
                    return 0.5;
            }
            set
            {
                SetValue(LabelLineThicknessProperty, value);
            }
        }

        private static readonly DependencyProperty LabelLineThicknessProperty = DependencyProperty.Register
            ("LabelLineThickness",
            typeof(Nullable<Double>),
            typeof(DataPoint),
            new PropertyMetadata(OnLabelLineThicknessPropertyChanged));

        private static void OnLabelLineThicknessPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataPoint dataPoint = d as DataPoint;
            dataPoint.FirePropertyChanged("LabelLineThickness");
        }

        /// <summary>
        /// Sets the LabelLineStyle property
        /// </summary>
#if SL
        [System.ComponentModel.TypeConverter(typeof(Converters.NullableLineStylesConverter))]
#endif
        public Nullable<LineStyles> LabelLineStyle
        {
            get
            {
                if ((Nullable<LineStyles>)GetValue(LabelLineStyleProperty) == null)
                    return _parent.LabelLineStyle;
                else
                    return (Nullable<LineStyles>)GetValue(LabelLineStyleProperty);
            }
            set
            {
                SetValue(LabelLineStyleProperty, value);
            }
        }

        private static readonly DependencyProperty LabelLineStyleProperty = DependencyProperty.Register
            ("LabelLineStyle",
            typeof(Nullable<LineStyles>),
            typeof(DataPoint),
            new PropertyMetadata(OnLabelLineStylePropertyChanged));

        private static void OnLabelLineStylePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataPoint dataPoint = d as DataPoint;
            dataPoint.FirePropertyChanged("LabelLineStyle");
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
                if ((Nullable<Boolean>)GetValue(MarkerEnabledProperty) == null)
                    return _parent.MarkerEnabled;
                else
                    return (Boolean)GetValue(MarkerEnabledProperty);
            }
            set

            
            {   
                SetValue(MarkerEnabledProperty, value);
            }
        }

        private static readonly DependencyProperty MarkerEnabledProperty = DependencyProperty.Register
            ("MarkerEnabled",
            typeof(Nullable<Boolean>),
            typeof(DataPoint),
            new PropertyMetadata(OnMarkerEnabledPropertyChanged));
            
        private static void OnMarkerEnabledPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataPoint dataPoint = d as DataPoint;
            dataPoint.FirePropertyChanged("MarkerEnabled");
        }

        /// <summary>
        /// Sets the MarkerStyle property
        /// </summary>
#if SL
        [System.ComponentModel.TypeConverter(typeof(Converters.NullableMarkerTypesConverter))]
#endif
        public Nullable<MarkerTypes> MarkerType
        {
            get
            {
                if ((Nullable<MarkerTypes>)GetValue(MarkerTypeProperty) == null)
                     return _parent.MarkerType;
                else
                   return (Nullable<MarkerTypes>)GetValue(MarkerTypeProperty);
            }
            set
            {
                SetValue(MarkerTypeProperty, value);
            }
        }

        private static readonly DependencyProperty MarkerTypeProperty = DependencyProperty.Register
            ("MarkerType",
            typeof(Nullable<MarkerTypes>),
            typeof(DataPoint),
            new PropertyMetadata(OnMarkerTypePropertyChanged));

        private static void OnMarkerTypePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataPoint dataPoint = d as DataPoint;
            dataPoint.FirePropertyChanged("MarkerType");
        }

        /// <summary>
        /// Sets the MarkerBorderThickness property
        /// </summary>
#if SL
        [System.ComponentModel.TypeConverter(typeof(Converters.NullableThicknessConverter))]
#endif
        public Nullable<Thickness> MarkerBorderThickness
        {
            get
            {
                if ((Nullable<Thickness>)GetValue(MarkerBorderThicknessProperty) == null)
                {
                    if (_parent.MarkerBorderThickness == null)
                    {
                        if (this.Parent.RenderAs == RenderAs.Point && this.MarkerType == MarkerTypes.Cross)
                            return new Thickness(1);
                        else if (this.Parent.RenderAs == RenderAs.Bubble || (this.Parent.RenderAs == RenderAs.Point && this.MarkerType != MarkerTypes.Cross))
                            return new Thickness(0);
                        else
                            return new Nullable<Thickness>(new Thickness((Double)MarkerSize / 6));
                    }
                    else
                        return _parent.MarkerBorderThickness;
                }
                else
                    return (Nullable<Thickness>)GetValue(MarkerBorderThicknessProperty);

            }
            set
            {
                SetValue(MarkerBorderThicknessProperty, value);
            }
        }

        private static readonly DependencyProperty MarkerBorderThicknessProperty = DependencyProperty.Register
            ("MarkerBorderThickness",
            typeof(Nullable<Thickness>),
            typeof(DataPoint),
            new PropertyMetadata(OnMarkerBorderThicknessPropertychanged));

        private static void OnMarkerBorderThicknessPropertychanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataPoint dataPoint = d as DataPoint;
            dataPoint.FirePropertyChanged("MarkerBorderThickness");
        }

        /// <summary>
        /// Sets the MarkerBorderColor property
        /// </summary>
        public Brush MarkerBorderColor
        {
            get
            {
                if (GetValue(MarkerBorderColorProperty) != null)
                    return (Brush)GetValue(MarkerBorderColorProperty);
                else
                    if (_parent.MarkerBorderColor != null)
                        return (_parent.MarkerBorderColor);
                    else
                        return (Color);
            }
            set
            {
                SetValue(MarkerBorderColorProperty, value);
            }
        }

        private static readonly DependencyProperty MarkerBorderColorProperty = DependencyProperty.Register
           ("MarkerBorderColor",
           typeof(Brush),
           typeof(DataPoint),
           new PropertyMetadata(OnMarkerBorderColorPropertychanged));

        private static void OnMarkerBorderColorPropertychanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataPoint dataPoint = d as DataPoint;
            dataPoint.FirePropertyChanged("MarkerBorderColor");
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
                    return _parent.MarkerSize;
                else
                    return (Nullable<Double>)GetValue(MarkerSizeProperty);
            }
            set
            {   
                SetValue(MarkerSizeProperty, value);
            }
        }

        private static readonly DependencyProperty MarkerSizeProperty = DependencyProperty.Register
            ("MarkerSize",
            typeof(Nullable<Double>),
            typeof(DataPoint),
            new PropertyMetadata(OnMarkerSizePropertychanged));

        private static void OnMarkerSizePropertychanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataPoint dataPoint = d as DataPoint;
            dataPoint.FirePropertyChanged("MarkerSize");
        }

        /// <summary>
        /// Sets the MarkerColor property
        /// </summary>
        public Brush MarkerColor
        {
            get
            {
                if (GetValue(MarkerColorProperty) == null)
                    if (_parent.MarkerColor == null)
                        //if (this.Parent.RenderAs == RenderAs.Line)
                            return new SolidColorBrush(Colors.White);
                        //else
                        //    return Color;
                    else
                        return _parent.MarkerColor;
                else
                    return (Brush)GetValue(MarkerColorProperty);
            }
            set
            {
                SetValue(MarkerColorProperty, value);
            }
        }

        private static readonly DependencyProperty MarkerColorProperty = DependencyProperty.Register
            ("MarkerColor",
            typeof(Brush),
            typeof(DataPoint),
            new PropertyMetadata(OnMarkerColorPropertychanged));

        private static void OnMarkerColorPropertychanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataPoint dataPoint = d as DataPoint;
            dataPoint.FirePropertyChanged("MarkerColor");
        }

        /// <summary>
        /// Sets the MarkerScale property
        /// </summary>
#if SL
        [System.ComponentModel.TypeConverter(typeof(Converters.NullableDoubleConverter))]
#endif
        public Nullable<Double> MarkerScale
        {
            get
            {   
                if ((Nullable<Double>)GetValue(MarkerScaleProperty) == null)
                    return _parent.MarkerScale;
                else
                    return (Nullable<Double>)GetValue(MarkerScaleProperty);
            }
            set
            {
                SetValue(MarkerScaleProperty, value);
            }
        }

        private static readonly DependencyProperty MarkerScaleProperty = DependencyProperty.Register
            ("MarkerScale",
            typeof(Nullable<Double>),
            typeof(DataPoint),
            new PropertyMetadata(OnMarkerScalePropertychanged));

        private static void OnMarkerScalePropertychanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataPoint dataPoint = d as DataPoint;
            dataPoint.FirePropertyChanged("MarkerScale");
        }

        #endregion Marker Properties

        /// <summary>
        /// Sets the ToolTipText for the DataPoint
        /// </summary>
        public override String ToolTipText
        {
            get
            {
                if (!String.IsNullOrEmpty(base.ToolTipText))
                    return base.ToolTipText;
                else if (String.IsNullOrEmpty(_toolTipText))
                    return _parent.ToolTipText;
                else
                    return _toolTipText;
            }
            set
            {
                _toolTipText = value;
                base.ToolTipText = value;
                CheckPropertyChanged<String>("ToolTipText", ref _toolTipText, ref value);
            }
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
                    return _parent.ShowInLegend;
                }
                else
                {
                    return (Nullable<Boolean>)GetValue(ShowInLegendProperty);
                }
            }
            set
            {
                SetValue(ShowInLegendProperty, value);
            }
        }

        private static readonly DependencyProperty ShowInLegendProperty = DependencyProperty.Register
            ("ShowInLegend",
            typeof(Nullable<Boolean>),
            typeof(DataPoint),
            new PropertyMetadata(OnShowInLegendPropertychanged));

        private static void OnShowInLegendPropertychanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataPoint dataPoint = d as DataPoint;
            dataPoint.FirePropertyChanged("ShowInLegend");
        }

        /// <summary>
        /// Sets the LegendText
        /// </summary>
        public String LegendText
        {
            get
            {
                if(String.IsNullOrEmpty((String)GetValue(LegendTextProperty)))
                {
                    if (String.IsNullOrEmpty(_parent.LegendText))
                        if (this.Parent.RenderAs == RenderAs.Pie || this.Parent.RenderAs == RenderAs.Doughnut)
                            return this.TextParser("#AxisXLabel");
                        else
                            return this.Name;
                    else
                        return _parent.LegendText;
                }
                else
                {
                    return (String)GetValue(LegendTextProperty);
                }
            }
            set
            {
                SetValue(LegendTextProperty, value);
            }
        }

        private static readonly DependencyProperty LegendTextProperty = DependencyProperty.Register
            ("LegendText",
            typeof(String),
            typeof(DataPoint),
            new PropertyMetadata(OnLegendTextPropertychanged));

        private static void OnLegendTextPropertychanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataPoint dataPoint = d as DataPoint;
            dataPoint.FirePropertyChanged("LegendText");
        }

        /// <summary>
        /// Set the BorderThickness property
        /// </summary>
        public new Nullable<Thickness> BorderThickness
        {
            get
            {
                if ((Nullable<Thickness>)GetValue(BorderThicknessProperty) == null || (Nullable<Thickness>)GetValue(BorderThicknessProperty) == new Thickness(0,0,0,0))
                    return _parent.BorderThickness;
                else
                    return (Nullable<Thickness>)GetValue(BorderThicknessProperty);
            }
            set
            {
                SetValue(BorderThicknessProperty, value);
                if ((Nullable<Thickness>)GetValue(BorderThicknessProperty) != null)
                    this.FirePropertyChanged("BorderThickness");
            }
        }

        //private new static readonly DependencyProperty BorderThicknessProperty = DependencyProperty.Register
        //    ("BorderThickness",
        //    typeof(Thickness),
        //    typeof(DataPoint),
        //    new PropertyMetadata(OnBorderThicknessPropertyChanged));

        //private static void OnBorderThicknessPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    DataPoint dataPoint = d as DataPoint;
        //    dataPoint.FirePropertyChanged("BorderThickness");
        //}

        /// <summary>
        /// Set the BorderColor property
        /// </summary>
        public Brush BorderColor
        {
            get
            {
                if (GetValue(BorderColorProperty) == null)
                {
                    return _parent.BorderColor;
                }
                else
                {
                    return (Brush)GetValue(BorderColorProperty);
                }
            }
            set
            {
                SetValue(BorderColorProperty, value);
            }
        }

        private static readonly DependencyProperty BorderColorProperty = DependencyProperty.Register
            ("BorderColor",
            typeof(Brush),
            typeof(DataPoint),
            new PropertyMetadata(OnBorderColorPropertyChanged));

        private static void OnBorderColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataPoint dataPoint = d as DataPoint;
            dataPoint.FirePropertyChanged("BorderColor");
        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        private new Brush BorderBrush
        {
            get;
            set;
        }
             
        public Nullable<BorderStyles> BorderStyle
        {
            get
            {
                if ((Nullable<BorderStyles>)GetValue(BorderStyleProperty) == null)
                {
                    return _parent.BorderStyle;
                }
                else
                {
                    return (Nullable<BorderStyles>)GetValue(BorderStyleProperty); 
                }
            }
            set
            {
                SetValue(BorderStyleProperty, value);
            }
        }

        private static readonly DependencyProperty BorderStyleProperty = DependencyProperty.Register
            ("BorderStyle",
            typeof(Nullable<BorderStyles>),
            typeof(DataPoint),
            new PropertyMetadata(OnBorderStylePropertyChanged));

        private static void OnBorderStylePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataPoint dataPoint = d as DataPoint;
            dataPoint.FirePropertyChanged("BorderStyle");
        }
        
        /// <summary>
        /// Set the RadiusX
        /// </summary>

#if WPF
        [System.ComponentModel.TypeConverter(typeof(System.Windows.CornerRadiusConverter))]
#else
        [System.ComponentModel.TypeConverter(typeof(Converters.CornerRadiusConverter))]
#endif
        public Nullable<CornerRadius> RadiusX
        {
            get
            {
                if ((Nullable<CornerRadius>)GetValue(RadiusXProperty) != null)
                    return (CornerRadius)GetValue(RadiusXProperty);
                else
                    return _parent.RadiusX;
            }
            set
            {
                SetValue(RadiusXProperty, value);
            }
        }

        private static readonly DependencyProperty RadiusXProperty = DependencyProperty.Register
            ("RadiusX",
            typeof(Nullable<CornerRadius>),
            typeof(DataPoint),
            new PropertyMetadata(OnRadiusXPropertyChanged));
            
        private static void OnRadiusXPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {   
            DataPoint dataPoint = d as DataPoint;
            dataPoint.FirePropertyChanged("RadiusX");
        }
        /// <summary>
        /// Set the RadiusY
        /// </summary>

#if WPF
        [System.ComponentModel.TypeConverter(typeof(System.Windows.CornerRadiusConverter))]
#else
        [System.ComponentModel.TypeConverter(typeof(Converters.CornerRadiusConverter))]
#endif
        public Nullable<CornerRadius> RadiusY
        {
            get
            {
                if ((Nullable<CornerRadius>)GetValue(RadiusYProperty) != null)
                    return (CornerRadius)GetValue(RadiusYProperty);
                else
                    return _parent.RadiusY;
            }
            set
            {
                SetValue(RadiusYProperty, value);
            }
        }

        private static readonly DependencyProperty RadiusYProperty = DependencyProperty.Register
            ("RadiusY",
            typeof(Nullable<CornerRadius>),
            typeof(DataPoint),
            new PropertyMetadata(OnRadiusYPropertyChanged));

        private static void OnRadiusYPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataPoint dataPoint = d as DataPoint;
            dataPoint.FirePropertyChanged("RadiusY");
        }

        /// <summary>
        /// Parent of DataPoints 
        /// </summary>
        public new DataSeries Parent
        {
            get
            {   
                return _parent;
            }
            internal set
            {
                System.Diagnostics.Debug.Assert(typeof(DataSeries).Equals(value.GetType()), "Unknown Parent", "DataPoint should have DataSeries as Parent"); 
                _parent = value;
            }
        }
        
        #endregion

        #region Public Events

        #endregion

        #region Protected Methods

            internal override void UpdateVisual(String PropertyName, object Value)
            {   
                switch(PropertyName)
                {
                    case "Color":

                        #region Color

                        Value = Color;

                        if (Faces != null && Faces.Parts != null)
                        {
                            switch(Parent.RenderAs)
                            {
                                case RenderAs.Column:
                                case RenderAs.Bar:
                                case RenderAs.StackedBar:
                                case RenderAs.StackedBar100:
                                case RenderAs.StackedColumn:
                                case RenderAs.StackedColumn100:

                                    if (!(Parent.Chart as Chart).View3D)
                                    {
                                        if (Faces.Parts[0] != null) (Faces.Parts[0] as Path).Fill = (((Boolean)Parent.LightingEnabled) ? Graphics.GetLightingEnabledBrush((Brush)Value, "Linear", new Double[] { 0.745, 0.99}) : (Brush)Value);
                                        if (Faces.Parts[1] != null) (Faces.Parts[1] as Polygon).Fill = Graphics.GetBevelTopBrush((Brush)Value);
                                        if (Faces.Parts[2] != null) (Faces.Parts[2] as Polygon).Fill = Graphics.GetBevelSideBrush(((Boolean)Parent.LightingEnabled ? -70 : 0), (Brush)Value);
                                        if (Faces.Parts[3] != null) (Faces.Parts[3] as Polygon).Fill = Graphics.GetBevelSideBrush(((Boolean)Parent.LightingEnabled ? -110 : 180), (Brush)Value);
                                        if (Faces.Parts[4] != null) (Faces.Parts[4] as Polygon).Fill = null;
                                        if (Faces.Parts[5] != null) (Faces.Parts[5] as Shape).Fill = Graphics.GetLeftGradianceBrush(63);
                                        if (Faces.Parts[6] != null) (Faces.Parts[6] as Shape).Fill = ((Parent.Chart as Chart).PlotDetails.ChartOrientation == ChartOrientationType.Vertical) ? Graphics.GetRightGradianceBrush(63) : Graphics.GetLeftGradianceBrush(63);
                                    }
                                    else
                                    {   
                                        if (Faces.Parts[0] != null) (Faces.Parts[0] as Shape).Fill = (Boolean)Parent.LightingEnabled ? Graphics.GetFrontFaceBrush((Brush)Value) : (Brush)Value ;   // Front brush
                                        if (Faces.Parts[1] != null) (Faces.Parts[1] as Shape).Fill = (Boolean)Parent.LightingEnabled ? Graphics.GetTopFaceBrush((Brush)Value) : (Brush)Value;      // Top brush
                                        if (Faces.Parts[2] != null) (Faces.Parts[2] as Shape).Fill = (Boolean)Parent.LightingEnabled ? Graphics.GetRightFaceBrush((Brush)Value) : (Brush)Value;    // Right
                                    }
                                    
                                break;
                                    
                                case RenderAs.Bubble:
                                    if (Faces.Parts[0] != null) (Faces.Parts[0] as Shape).Fill = ((Parent.Chart as Chart).View3D ? Graphics.GetLightingEnabledBrush3D((Brush)Value) : ((Boolean)LightingEnabled ? Graphics.GetLightingEnabledBrush((Brush)Value, "Linear", new Double[] { 0.99, 0.745 }) : (Brush)Value));
                                    
                                break;

                                case RenderAs.Pie:
                                case RenderAs.Doughnut:

                                    SectorChartShapeParams pieParams = (SectorChartShapeParams)this.VisualParams;
                                    pieParams.Background = (Brush)Value;

                                    if (!(Parent.Chart as Chart).View3D)
                                    {
                                        if (Faces.Parts[0] != null) (Faces.Parts[0] as Shape).Fill = (Boolean)Parent.LightingEnabled ? Graphics.GetLightingEnabledBrush((Brush)Value, "Radial", null) : (Brush)Value;

                                        if (Faces.Parts[1] != null)
                                            (Faces.Parts[1] as Shape).Fill = (pieParams.StartAngle > Math.PI * 0.5 && pieParams.StartAngle <= Math.PI * 1.5) ? PieChart.GetDarkerBevelBrush(pieParams.Background, pieParams.StartAngle * 180 / Math.PI + 135) : PieChart.GetLighterBevelBrush(pieParams.Background, -pieParams.StartAngle * 180 / Math.PI);

                                        if (Faces.Parts[2] != null)
                                            (Faces.Parts[2] as Shape).Fill = (pieParams.StopAngle > Math.PI * 0.5 && pieParams.StopAngle <= Math.PI * 1.5) ? PieChart.GetLighterBevelBrush(pieParams.Background, pieParams.StopAngle * 180 / Math.PI + 135) : PieChart.GetDarkerBevelBrush(pieParams.Background, -pieParams.StopAngle * 180 / Math.PI);

                                        if (Faces.Parts[3] != null)
                                            (Faces.Parts[3] as Shape).Fill = (pieParams.MeanAngle > 0 && pieParams.MeanAngle < Math.PI) ? PieChart.GetCurvedBevelBrush(pieParams.Background, pieParams.MeanAngle * 180 / Math.PI + 90, PieChart.GetDoubleCollection(-0.745, -0.85), PieChart.GetDoubleCollection(0, 1)) : (Faces.Parts[3] as Shape).Fill = PieChart.GetCurvedBevelBrush(pieParams.Background, pieParams.MeanAngle * 180 / Math.PI + 90, PieChart.GetDoubleCollection(0.745, -0.99), PieChart.GetDoubleCollection(0, 1));
                                        
                                        if (Parent.RenderAs == RenderAs.Doughnut && Faces.Parts[4] != null)
                                            (Faces.Parts[4] as Shape).Fill = (pieParams.MeanAngle > 0 && pieParams.MeanAngle < Math.PI) ? PieChart.GetCurvedBevelBrush(pieParams.Background, pieParams.MeanAngle * 180 / Math.PI + 90, PieChart.GetDoubleCollection(-0.745, -0.85), PieChart.GetDoubleCollection(0, 1)) : (Faces.Parts[4] as Shape).Fill = PieChart.GetCurvedBevelBrush(pieParams.Background, pieParams.MeanAngle * 180 / Math.PI + 90, PieChart.GetDoubleCollection(0.745, -0.99), PieChart.GetDoubleCollection(0, 1));
                                    }
                                    else
                                    {   
                                        foreach (FrameworkElement fe in Faces.Parts)
                                            if (fe != null) (fe as Path).Fill = pieParams.Lighting ? Graphics.GetLightingEnabledBrush(pieParams.Background, "Radial", null) : pieParams.Background;
                                    }

                                    break;
                            }
                        }


                        UpdateMarkerAndLegend(Value);
   #endregion

                        break;

                    default:
                        FirePropertyChanged(PropertyName);
                        break;
                }
            }
            
        #endregion


        void UpdateMarkerAndLegend(object Value)
        {
            if (Marker != null && Marker.Visual != null)
            {
                if (Parent.RenderAs == RenderAs.Line)
                {
                    Marker.BorderColor = (Brush)Value;
                    Marker.UpdateMarker();
                }
                else if (Parent.RenderAs == RenderAs.Point)
                {
                    Marker.MarkerFillColor = (Brush)Value;
                    if (Marker.MarkerType != MarkerTypes.Cross)
                    {
                        if (BorderColor != null)
                            Marker.BorderColor = BorderColor;
                    }
                    else
                        Marker.BorderColor = (Brush)Value;

                    Marker.UpdateMarker();
                }
            }

            if (LegendMarker != null && LegendMarker.Visual != null)
            {
                LegendMarker.BorderColor = (Brush)Value;
                LegendMarker.MarkerFillColor = (Brush)Value;
                LegendMarker.UpdateMarker();
            }
        }

        #region Internal Properties

            internal Marker LegendMarker
            {
                get;
                set;
            }

        internal Faces Faces
        {
            get;
            set;
        }

        internal Grid LabelVisual
        {
            get;
            set;
        }
        internal Polyline LabelLineVisual
        {
            get;
            set;
        }

        internal Marker Marker
        {
            get;
            set;
        }
        internal Path _labelLine;
        internal Storyboard ExplodeAnimation
        {
            get;
            set;
        }
        internal Storyboard UnExplodeAnimation
        {
            get;
            set;
        }

        /// <summary>
        /// Visual Parameters
        /// </summary>
        internal object VisualParams
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
            internal void AttachEvent2DataPointVisualFaces(ObservableObject Object)
            {
                //if (LegendMarker != null)
                //    ObservableObject.AttachEvents2Visual(this, this.LegendMarker.Visual);

                if (Parent.RenderAs == RenderAs.Pie || Parent.RenderAs == RenderAs.Doughnut)
                {
                    if (Faces != null)
                    {
                        if ((Parent.Chart as Chart).View3D)
                        {
                            foreach (FrameworkElement element in Faces.VisualComponents)
                            {
                                AttachEvents2Visual(Object, this, element);
                                element.MouseLeftButtonUp -= new MouseButtonEventHandler(Visual_MouseLeftButtonUp);
                                element.MouseLeftButtonUp += new MouseButtonEventHandler(Visual_MouseLeftButtonUp);
                            }
                        }
                        else
                        {
                            AttachEvents2Visual(Object, this, Faces.Visual);
                            Faces.Visual.MouseLeftButtonUp -= new MouseButtonEventHandler(Visual_MouseLeftButtonUp);
                            Faces.Visual.MouseLeftButtonUp += new MouseButtonEventHandler(Visual_MouseLeftButtonUp);
                        }

                        this.ExplodeAnimation.Completed -= new EventHandler(ExplodeAnimation_Completed);
                        this.UnExplodeAnimation.Completed -= new EventHandler(UnExplodeAnimation_Completed);
                        this.ExplodeAnimation.Completed += new EventHandler(ExplodeAnimation_Completed);
                        this.UnExplodeAnimation.Completed += new EventHandler(UnExplodeAnimation_Completed);
                    }
                }
                else if (Parent.RenderAs == RenderAs.Area || Parent.RenderAs == RenderAs.StackedArea || Parent.RenderAs == RenderAs.StackedArea100)
                {
                    //if (Parent.Faces != null)
                    //{
                    //    foreach (FrameworkElement face in Parent.Faces.VisualComponents)
                    //    {
                    //        AttachEvents2Visual(Object, this, face);
                    //    }
                    //}

                    if (Marker != null)
                        AttachEvents2Visual(Object, this, Marker.Visual);
                }
                else if (Parent.RenderAs == RenderAs.Line)
                {
                     if (Marker != null)
                         AttachEvents2Visual(Object, this, Marker.Visual);
                }
                else
                {
                    if (Faces != null)
                    {
                        foreach (FrameworkElement face in Faces.VisualComponents)
                        {
                            AttachEvents2Visual(Object, this, face);
                        }
                    }
                }
            }

            void Visual_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
            {
                InteractiveAnimation();
            }

            internal void InteractiveAnimation()
            {
                // interactivity animation is not running already and the slice is not exploded
                // then explode the slice
                if (!_interativityAnimationState)
                {
                    if (false == _interactiveExplodeState)
                    {
                        _interativityAnimationState = true;
#if WPF
                        this.ExplodeAnimation.Begin(this as FrameworkElement, true);
#else
                        this.ExplodeAnimation.Begin();
#endif
                    }

                    if (true == _interactiveExplodeState)
                    {
                        _interativityAnimationState = true;
#if WPF
                        this.UnExplodeAnimation.Begin(this as FrameworkElement, true);
#else
                        this.UnExplodeAnimation.Begin();
#endif
                    }
                }
            }
            
            void ExplodeAnimation_Completed(object sender, EventArgs e)
            {
                _interactiveExplodeState = true;
                _interativityAnimationState = false;
                Chart._rootElement.IsHitTestVisible = true;
            }

            void UnExplodeAnimation_Completed(object sender, EventArgs e)
            {
                _interactiveExplodeState = false;
                _interativityAnimationState = false;
            }

            /// <summary>
            /// Set Cursor property for DataPoint visual faces
            /// </summary>
            internal void SetHref2DataPointVisualFaces(ObservableObject Object)
            {
                System.Diagnostics.Debug.WriteLine(Href);
                if (Faces != null)
                    foreach (FrameworkElement face in Faces.VisualComponents)
                    {
                        AttachHref(Chart, face, Href, (HrefTargets)HrefTarget);
                    }
            }
            
            /// <summary>
            /// Set Cursor property for DataPoint visual faces
            /// </summary>
            internal void SetCursor2DataPointVisualFaces()
            {
                if (Faces != null)
                    foreach (FrameworkElement face in Faces.VisualComponents)
                    {
                        face.Cursor = GetCursor();
                    }
                if(this.Parent.Faces != null)
                    foreach (FrameworkElement face in this.Parent.Faces.VisualComponents)
                    {
                        face.Cursor = GetCursor();
                    }
                if (this.Marker != null)
                {
                    Marker.Visual.Cursor = GetCursor();
                }
            }
            
            private String GetAxisXLabelString()
            {
                String labelString = "";

                if (Parent.PlotGroup != null && Parent.PlotGroup.AxisX != null && Parent.PlotGroup.AxisX.AxisLabels != null)
                {
                    if (Parent.PlotGroup.AxisX.AxisLabels.AxisLabelContentDictionary != null &&
                    Parent.PlotGroup.AxisX.AxisLabels.AxisLabelContentDictionary.ContainsKey(XValue))
                    {
                        labelString = Parent.PlotGroup.AxisX.AxisLabels.AxisLabelContentDictionary[XValue];
                    }
                    else
                    {
                        labelString = Parent.PlotGroup.AxisX.GetFormattedString(XValue);
                    }
                }
                else
                    labelString = this.AxisXLabel;
                return labelString;
            }

            private Double Percentage()
            {
                Double percentage = 0;
                if (Parent.RenderAs ==  RenderAs.Pie || Parent.RenderAs == RenderAs.Doughnut)
                {
                    if ((Parent.Chart as Chart).PlotDetails != null)
                    {
                        Double sum = (Parent.Chart as Chart).PlotDetails.GetAbsoluteSumOfDataPoints(Parent.DataPoints.ToList());
                        if (sum > 0) percentage = ((YValue / sum) * 100);
                        else percentage = 0;
                    }
                }
                else if (Parent.RenderAs ==  RenderAs.StackedArea100 || Parent.RenderAs == RenderAs.StackedBar100 || Parent.RenderAs == RenderAs.StackedColumn100)
                {
                    percentage = YValue / Parent.PlotGroup.XWiseStackedDataList[XValue].AbsoluteYValueSum * 100;// _stackSum[XValue].Y Contains Absolute sum
                }
                return percentage;
            }

            public String TextParser(String unParsed)
            {
                if (string.IsNullOrEmpty(unParsed) || Enabled == false)
                    return "";

                String str = new String(unParsed.ToCharArray());
                if (str.Contains("##XValue"))
                    str = str.Replace("##XValue", "#XValue");
                else
                {
                    if (String.IsNullOrEmpty(_parent.XValueFormatString))
                    {
                        if(Parent.PlotGroup != null)
                            str = str.Replace("#XValue", Parent.PlotGroup.AxisX.GetFormattedString(XValue));
                    }
                    else
                        str = str.Replace("#XValue", XValue.ToString(Parent.XValueFormatString));
                }

                if (str.Contains("##YValue"))
                    str = str.Replace("##YValue", "#YValue");
                else
                {
                    if (String.IsNullOrEmpty(_parent.YValueFormatString))
                    {
                        if (Parent.PlotGroup != null)
                            str = str.Replace("#YValue", Parent.PlotGroup.AxisY.GetFormattedString(YValue));
                    }
                    else
                        str = str.Replace("#YValue", YValue.ToString(Parent.YValueFormatString));
                }

                if (str.Contains("##ZValue"))
                    str = str.Replace("##ZValue", "#ZValue");
                else
                {
                    str = str.Replace("#ZValue", ZValue.ToString(Parent.ZValueFormatString));
                }

                if (str.Contains("##Series"))
                    str = str.Replace("##Series", "#Series");
                else
                    str = str.Replace("#Series", Parent.Name);

                if (str.Contains("##AxisXLabel"))
                    str = str.Replace("##AxisXLabel", "#AxisXLabel");
                else
                    str = str.Replace("#AxisXLabel", String.IsNullOrEmpty(AxisXLabel) ? GetAxisXLabelString() : AxisXLabel);

                if (str.Contains("##Percentage"))
                    str = str.Replace("##Percentage", "#Percentage");
                else
                    str = str.Replace("#Percentage", Percentage().ToString("#0.##"));


                if (str.Contains("##Sum"))
                    str = str.Replace("##Sum", "#Sum");
                else
                {
                    if (Parent.PlotGroup != null && Parent.PlotGroup.XWiseStackedDataList != null && Parent.PlotGroup.XWiseStackedDataList.ContainsKey(XValue))
                    {
                        Double sum = 0;
                        sum += Parent.PlotGroup.XWiseStackedDataList[XValue].PositiveYValueSum;
                        sum += Parent.PlotGroup.XWiseStackedDataList[XValue].NegativeYValueSum;
                        str = str.Replace("#Sum", Parent.PlotGroup.AxisY.GetFormattedString(sum));  //_stackSum[XValue].X contains sum of all data points with same X value
                    }
                }
                return str;
            }
            
        #endregion

        #region Internal Events

        #endregion

        #region Data

            private DataSeries _parent;                         // Parent of DataPoints 
            private String _toolTipText;
            private Boolean _interactiveExplodeState = false;
            private Boolean _interativityAnimationState = false;
            internal Brush InternalColor;
#if WPF
        static Boolean _defaultStyleKeyApplied;            // Default Style key
#endif
        #endregion
    }
}



