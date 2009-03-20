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
    /// <summary>
    /// Visifire.Charts.DataSeries class
    /// </summary>
#if SL
    [System.Windows.Browser.ScriptableType]
#endif
    public class DataSeries : ObservableObject
    {   
        #region Public Methods

        /// <summary>
        /// Initializes a new instance of the Visifire.Charts.DataSeries class
        /// </summary>
        public DataSeries()
        {
            ToolTipText = "";

            // Apply default style from generic
#if WPF
            if (!_defaultStyleKeyApplied)
            {
                DefaultStyleKeyProperty.OverrideMetadata(typeof(DataSeries), new FrameworkPropertyMetadata(typeof(DataSeries)));
                _defaultStyleKeyApplied = true;
            }
#else
            DefaultStyleKey = typeof(DataSeries);
#endif

            // Initialize DataPoints list
            DataPoints = new ObservableCollection<DataPoint>();

            // Initialize InternalDataPoints list
            InternalDataPoints = new List<DataPoint>();

            // Attach event handler for the Title collection changed event
            DataPoints.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(DataPoints_CollectionChanged);
        }
        
        #endregion

        #region Public Properties
        
        /// <summary>
        /// Identifies the Visifire.Charts.DataSeries.Enabled dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.DataSeries.Enabled dependency property.
        /// </returns>
        public static readonly DependencyProperty EnabledProperty = DependencyProperty.Register
            ("Enabled",
            typeof(Nullable<Boolean>),
            typeof(DataSeries),
            new PropertyMetadata(OnEnabledPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.DataSeries.RenderAs dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.DataSeries.RenderAs dependency property.
        /// </returns>
        public static readonly DependencyProperty RenderAsProperty = DependencyProperty.Register
            ("RenderAs",
            typeof(RenderAs),
            typeof(DataSeries),
            new PropertyMetadata(OnRenderAsPropertyChanged));
        
        /// <summary>
        /// Identifies the Visifire.Charts.DataSeries.HrefTarget dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.DataSeries.HrefTarget dependency property.
        /// </returns>
        public static readonly DependencyProperty HrefTargetProperty = DependencyProperty.Register
            ("HrefTarget",
            typeof(HrefTargets),
            typeof(DataSeries),
            new PropertyMetadata(OnHrefTargetChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.DataSeries.Href dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.DataSeries.Href dependency property.
        /// </returns>
        public static readonly DependencyProperty HrefProperty = DependencyProperty.Register
            ("Href",
            typeof(String),
            typeof(DataSeries),
            new PropertyMetadata(OnHrefChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.DataSeries.Color dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.DataSeries.Color dependency property.
        /// </returns>
        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register
            ("Color",
            typeof(Brush),
            typeof(DataSeries),
            new PropertyMetadata(OnColorPropertyChanged));
        
        /// <summary>
        /// Identifies the Visifire.Charts.DataSeries.LightingEnabled dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.DataSeries.LightingEnabled dependency property.
        /// </returns>
        public static readonly DependencyProperty LightingEnabledProperty = DependencyProperty.Register
            ("LightingEnabled",
            typeof(Nullable<Boolean>),
            typeof(DataSeries),
            new PropertyMetadata(OnLightingEnabledPropertyChanged));
        
        /// <summary>
        /// Identifies the Visifire.Charts.DataSeries.ShadowEnabled dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.DataSeries.ShadowEnabled dependency property.
        /// </returns>
        public static readonly DependencyProperty ShadowEnabledProperty = DependencyProperty.Register
            ("ShadowEnabled",
            typeof(Boolean),
            typeof(DataSeries),
            new PropertyMetadata(OnShadowEnabledPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.DataSeries.LegendText dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.DataSeries.LegendText dependency property.
        /// </returns>
        public static readonly DependencyProperty LegendTextProperty = DependencyProperty.Register
            ("LegendText",
            typeof(String),
            typeof(DataSeries),
            new PropertyMetadata(OnLegendTextPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.DataSeries.Legend dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.DataSeries.Legend dependency property.
        /// </returns>
        public static readonly DependencyProperty LegendProperty = DependencyProperty.Register
            ("Legend",
            typeof(String),
            typeof(DataSeries),
            new PropertyMetadata(OnLegendPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.DataSeries.Bevel dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.DataSeries.Bevel dependency property.
        /// </returns>
        public static readonly DependencyProperty BevelProperty = DependencyProperty.Register
            ("Bevel",
            typeof(Boolean),
            typeof(DataSeries),
            new PropertyMetadata(OnBevelPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.DataSeries.ColorSet dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.DataSeries.ColorSet dependency property.
        /// </returns>
        public static readonly DependencyProperty ColorSetProperty = DependencyProperty.Register
            ("ColorSet",
            typeof(String),
            typeof(DataSeries),
            new PropertyMetadata(OnColorSetPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.DataSeries.RadiusX dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.DataSeries.RadiusX dependency property.
        /// </returns>
        public static readonly DependencyProperty RadiusXProperty = DependencyProperty.Register
             ("RadiusX",
             typeof(CornerRadius),
             typeof(DataSeries),
             new PropertyMetadata(OnRadiusXPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.DataSeries.RadiusY dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.DataSeries.RadiusY dependency property.
        /// </returns>
        public static readonly DependencyProperty RadiusYProperty = DependencyProperty.Register
            ("RadiusY",
            typeof(CornerRadius),
            typeof(DataSeries),
            new PropertyMetadata(OnRadiusYPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.DataSeries.LineThickness dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.DataSeries.LineThickness dependency property.
        /// </returns>
        public static readonly DependencyProperty LineThicknessProperty = DependencyProperty.Register
             ("LineThickness",
             typeof(Nullable<Double>),
             typeof(DataSeries),
             new PropertyMetadata(OnLineThicknessPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.DataSeries.LineStyle dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.DataSeries.LineStyle dependency property.
        /// </returns>
        public static readonly DependencyProperty LineStyleProperty = DependencyProperty.Register
            ("LineStyle",
            typeof(LineStyles),
            typeof(DataSeries),
            new PropertyMetadata(OnLineStylePropertyChanged));

#if WPF
        /// <summary>
        /// Identifies the Visifire.Charts.DataSeries.Opacity dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.DataSeries.Opacity dependency property.
        /// </returns>
        public new static readonly DependencyProperty OpacityProperty = DependencyProperty.Register
            ("Opacity",
            typeof(Double),
            typeof(DataSeries),
            new PropertyMetadata(1.0, OnOpacityPropertyChanged));
#endif

        /// <summary>
        /// Identifies the Visifire.Charts.DataSeries.ShowInLegend dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.DataSeries.ShowInLegend dependency property.
        /// </returns>
        public static readonly DependencyProperty ShowInLegendProperty = DependencyProperty.Register
            ("ShowInLegend",
            typeof(Nullable<Boolean>),
            typeof(DataSeries),
            new PropertyMetadata(OnShowInLegendPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.DataSeries.LabelEnabled dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.DataSeries.LabelEnabled dependency property.
        /// </returns>
        public static readonly DependencyProperty LabelEnabledProperty = DependencyProperty.Register
            ("LabelEnabled",
            typeof(Nullable<Boolean>),
            typeof(DataSeries),
            new PropertyMetadata(OnLabelEnabledPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.DataSeries.LabelText dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.DataSeries.LabelText dependency property.
        /// </returns>
        public static readonly DependencyProperty LabelTextProperty = DependencyProperty.Register
            ("LabelText",
            typeof(String),
            typeof(DataSeries),
            new PropertyMetadata(OnLabelTextPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.DataSeries.LabelFontFamily dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.DataSeries.LabelFontFamily dependency property.
        /// </returns>
        public static readonly DependencyProperty LabelFontFamilyProperty = DependencyProperty.Register
            ("LabelFontFamily",
            typeof(FontFamily),
            typeof(DataSeries),
            new PropertyMetadata(OnLabelFontFamilyPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.DataSeries.LabelFontSize dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.DataSeries.LabelFontSize dependency property.
        /// </returns>
        public static readonly DependencyProperty LabelFontSizeProperty = DependencyProperty.Register
            ("LabelFontSize",
            typeof(Nullable<Double>),
            typeof(DataSeries),
            new PropertyMetadata(OnLabelFontSizePropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.DataSeries.LabelFontColor dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.DataSeries.LabelFontColordependency property.
        /// </returns>
        public static readonly DependencyProperty LabelFontColorProperty = DependencyProperty.Register
            ("LabelFontColor",
            typeof(Brush),
            typeof(DataSeries),
            new PropertyMetadata(OnLabelFontColorPropertyChanged));
        
        /// <summary> 
        /// Identifies the Visifire.Charts.DataSeries.LabelFontWeight dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.DataSeries.LabelFontWeight dependency property.
        /// </returns>
        public static readonly DependencyProperty LabelFontWeightProperty = DependencyProperty.Register
            ("LabelFontWeight",
            typeof(FontWeight),
            typeof(DataSeries),
            new PropertyMetadata(OnLabelFontWeightPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.DataSeries.LabelFontStyle dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.DataSeries.LabelFontStyle dependency property.
        /// </returns>
        public static readonly DependencyProperty LabelFontStyleProperty = DependencyProperty.Register
            ("LabelFontStyle",
            typeof(FontStyle),
            typeof(DataSeries),
            new PropertyMetadata(OnLabelFontStylePropertyChanged));
        
        /// <summary>
        /// Identifies the Visifire.Charts.DataSeries.LabelBackground dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.DataSeries.LabelBackground dependency property.
        /// </returns>
        public static readonly DependencyProperty LabelBackgroundProperty = DependencyProperty.Register
               ("LabelBackground",
               typeof(Brush),
               typeof(DataSeries),
               new PropertyMetadata(OnLabelBackgroundPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.DataSeries.LabelStyle dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.DataSeries.LabelStyle dependency property.
        /// </returns>
        public static readonly DependencyProperty LabelStyleProperty = DependencyProperty.Register
             ("LabelStyle",
             typeof(Nullable<LabelStyles>),
             typeof(DataSeries),
             new PropertyMetadata(OnLabelStylePropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.DataSeries.LabelLineEnabled dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.DataSeries.LabelLineEnabled dependency property.
        /// </returns>
        public static readonly DependencyProperty LabelLineEnabledProperty = DependencyProperty.Register
            ("LabelLineEnabled",
            typeof(Nullable<Boolean>),
            typeof(DataSeries),
            new PropertyMetadata(OnLabelLineEnabledPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.DataSeries.LabelLineColor dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.DataSeries.LabelLineColor dependency property.
        /// </returns>
        public static readonly DependencyProperty LabelLineColorProperty = DependencyProperty.Register
            ("LabelLineColor",
            typeof(Brush),
            typeof(DataSeries),
            new PropertyMetadata(OnLabelLineColorPropertyChanged));


        /// <summary>
        /// Identifies the Visifire.Charts.DataSeries.LabelLineThickness dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.DataSeries.LabelLineThickness dependency property.
        /// </returns>
        public static readonly DependencyProperty LabelLineThicknessProperty = DependencyProperty.Register
            ("LabelLineThickness",
            typeof(Double),
            typeof(DataSeries),
            new PropertyMetadata(OnLabelLineThicknessPropertyChanged));
        
        /// <summary>
        /// Identifies the Visifire.Charts.DataSeries.LabelLineStyle dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.DataSeries.LabelLineStyle dependency property.
        /// </returns>
        public static readonly DependencyProperty LabelLineStyleProperty = DependencyProperty.Register
            ("LabelLineStyle",
            typeof(LineStyles),
            typeof(DataSeries),
            new PropertyMetadata(OnLabelLineStylePropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.DataSeries.MarkerEnabled dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.DataSeries.MarkerEnabled dependency property.
        /// </returns>
        public static readonly DependencyProperty MarkerEnabledProperty = DependencyProperty.Register
            ("MarkerEnabled",
            typeof(Nullable<Boolean>),
            typeof(DataSeries),
            new PropertyMetadata(OnMarkerEnabledPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.DataSeries.MarkerType dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.DataSeries.MarkerType dependency property.
        /// </returns>
        public static readonly DependencyProperty MarkerTypeProperty = DependencyProperty.Register
            ("MarkerType",
            typeof(MarkerTypes),
            typeof(DataSeries),
            new PropertyMetadata(OnMarkerTypePropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.DataSeries.MarkerBorderThickness dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.DataSeries.MarkerBorderThickness dependency property.
        /// </returns>
        public static readonly DependencyProperty MarkerBorderThicknessProperty = DependencyProperty.Register
            ("MarkerBorderThickness",
            typeof(Nullable<Thickness>),
            typeof(DataSeries),
            new PropertyMetadata(OnMarkerBorderThicknessPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.DataSeries.MarkerBorderColor dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.DataSeries.MarkerBorderColor dependency property.
        /// </returns>
        public static readonly DependencyProperty MarkerBorderColorProperty = DependencyProperty.Register
            ("MarkerBorderColor",
            typeof(Brush),
            typeof(DataSeries),
            new PropertyMetadata(OnMarkerBorderColorPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.DataSeries.MarkerSize dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.DataSeries.MarkerSize dependency property.
        /// </returns>
        public static readonly DependencyProperty MarkerSizeProperty = DependencyProperty.Register
            ("MarkerSize",
            typeof(Nullable<Double>),
            typeof(DataSeries),
            new PropertyMetadata(OnMarkerSizePropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.DataSeries.MarkerColor dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.DataSeries.MarkerColor dependency property.
        /// </returns>
        public static readonly DependencyProperty MarkerColorProperty = DependencyProperty.Register
            ("MarkerColor",
            typeof(Brush),
            typeof(DataSeries),
            new PropertyMetadata(OnMarkerColorPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.DataSeries.MarkerScale dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.DataSeries.MarkerScale dependency property.
        /// </returns>
        public static readonly DependencyProperty MarkerScaleProperty = DependencyProperty.Register
             ("MarkerScale",
             typeof(Nullable<Double>),
             typeof(DataSeries),
             new PropertyMetadata(OnMarkerScalePropertyChanged));
        
        /// <summary>
        /// Identifies the Visifire.Charts.DataSeries.StartAngle dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.DataSeries.StartAngle dependency property.
        /// </returns>
        public static readonly DependencyProperty StartAngleProperty = DependencyProperty.Register
            ("StartAngle",
            typeof(Double),
            typeof(DataSeries),
            new PropertyMetadata(OnStartAnglePropertyChanged));

#if WPF
        /// <summary>
        /// Identifies the Visifire.Charts.DataSeries.BorderThickness dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.DataSeries.BorderThickness dependency property.
        /// </returns>
        public new static readonly DependencyProperty BorderThicknessProperty = DependencyProperty.Register
        ("BorderThickness",
        typeof(Thickness),
        typeof(DataSeries),
        new PropertyMetadata(OnBorderThicknessPropertyChanged));
#endif

        /// <summary>
        /// Identifies the Visifire.Charts.DataSeries.BorderColor dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.DataSeries.BorderColor dependency property.
        /// </returns>
        public static readonly DependencyProperty BorderColorProperty = DependencyProperty.Register
            ("BorderColor",
            typeof(Brush),
            typeof(DataSeries),
            new PropertyMetadata(OnBorderColorPropertychanged));
        
        /// <summary>
        /// Identifies the Visifire.Charts.DataSeries.BorderStyle dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.DataSeries.BorderStyle dependency property.
        /// </returns>
        public static readonly DependencyProperty BorderStyleProperty = DependencyProperty.Register
            ("BorderStyle",
            typeof(BorderStyles),
            typeof(DataSeries),
            new PropertyMetadata(OnBorderStylePropertychanged));

        /// <summary>
        /// Identifies the Visifire.Charts.DataSeries.XValueFormatString dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.DataSeries.XValueFormatString dependency property.
        /// </returns>
        public static readonly DependencyProperty XValueFormatStringProperty = DependencyProperty.Register
            ("XValueFormatString",
            typeof(String),
            typeof(DataSeries),
            new PropertyMetadata(OnXValueFormatStringPropertyChanged));


        /// <summary>
        /// Identifies the Visifire.Charts.DataSeries.YValueFormatString dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.DataSeries.YValueFormatString dependency property.
        /// </returns>
        public static readonly DependencyProperty YValueFormatStringProperty = DependencyProperty.Register
            ("YValueFormatString",
            typeof(String),
            typeof(DataSeries),
            new PropertyMetadata(OnYValueFormatStringPropertyChanged));


        /// <summary>
        /// Identifies the Visifire.Charts.DataSeries.ZValueFormatString dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.DataSeries.ZValueFormatString dependency property.
        /// </returns>
        public static readonly DependencyProperty ZValueFormatStringProperty = DependencyProperty.Register
            ("ZValueFormatString",
            typeof(String),
            typeof(DataSeries),
            new PropertyMetadata(OnZValueFormatStringPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.DataSeries.AxisXType dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.DataSeries.AxisXType dependency property.
        /// </returns>
        public static readonly DependencyProperty AxisXTypeProperty = DependencyProperty.Register
            ("AxisXType",
            typeof(AxisTypes),
            typeof(DataSeries),
            new PropertyMetadata(OnAxisXTypePropertyChanged));
        
        /// <summary>
        /// Identifies the Visifire.Charts.DataSeries.AxisYType dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.DataSeries.AxisYType dependency property.
        /// </returns>
        public static readonly DependencyProperty AxisYTypeProperty = DependencyProperty.Register
            ("AxisYType",
            typeof(AxisTypes),
            typeof(DataSeries),
            new PropertyMetadata(OnAxisYTypePropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.DataSeries.XValueType dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.DataSeries.XValueType dependency property.
        /// </returns>
        public static readonly DependencyProperty XValueTypeProperty = DependencyProperty.Register
            ("XValueType",
            typeof(ChartValueTypes),
            typeof(DataSeries),
            new PropertyMetadata(OnXValueTypePropertyChanged));
        
        /// <summary>
        /// Identifies the Visifire.Charts.DataSeries.ZIndex dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.DataSeries.ZIndex dependency property.
        /// </returns>
        public static readonly DependencyProperty ZIndexProperty = DependencyProperty.Register
            ("ZIndex",
            typeof(Int32),
            typeof(DataSeries),
            new PropertyMetadata(OnZIndexPropertyChanged));
        
        /// <summary>
        /// Get or set ZIndex property
        /// (Will be used to decide which series comes in front and which one goes back)
        /// </summary>
        internal Int32 InternalZIndex
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set ZIndex property
        /// (Will be used to decide which series comes in front and which one goes back)
        /// </summary>
        public Int32 ZIndex
        {
            get
            {
                return (Int32)GetValue(ZIndexProperty);
            }
            set
            {
                InternalZIndex = value;
                SetValue(ZIndexProperty, value);
            }
        }

        /// <summary>
        /// Enables or disables DataSeries
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
                    FirePropertyChanged("Opacity");
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
                    FirePropertyChanged("Cursor");
                }
            }
        }

        /// <summary>
        /// Get or set the RenderAs property (Chart type)
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

        /// <summary>
        /// Get or set the HrefTarget property
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
        ///Get or set the Href property
        /// </summary>
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
        
        /// <summary>
        /// Get or set the Color property
        /// </summary>
        public Brush Color
        {
            get
            {
                if(((Brush)GetValue(DataSeries.ColorProperty) == null))
                    return _internalColor;
                else
                    return (Brush)GetValue(ColorProperty);
            }
            set
            {
                SetValue(ColorProperty, value);
            }
        }

        /// <summary>
        /// Get or set the LightingEnabled property
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
        
        /// <summary>
        /// Get or set the ShadowEnabled property
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
        /// Get or set the LegendText property
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
        
        /// <summary>
        /// Get or set the Legend (Legend Name) for the DataSeries
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
        
        /// <summary>
        /// Get or set the Bevel property for bevel effect
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
        /// Get or set the ColorSet property
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
        /// Get or set the RadiusX property
        /// </summary>
#if WPF
        [System.ComponentModel.TypeConverter(typeof(System.Windows.CornerRadiusConverter))]
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
        
        /// <summary>
        /// Get or set the RadiusY property
        /// </summary>
#if WPF
        [System.ComponentModel.TypeConverter(typeof(System.Windows.CornerRadiusConverter))]
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

        /// <summary>
        /// Get or set the LineThickness property
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

        /// <summary>
        /// Get or set the LineStyle property
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
        /// Get or set the ShowInLegend property
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

        #region Label Properties

        /// <summary>
        /// Get or set the LabelEnabled property
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

        /// <summary>
        /// Get or set the LabelText property
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

        /// <summary>
        /// Get or set the LabelFontFamily property
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

        /// <summary>
        /// Get or set the LabelFontSize property
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

        /// <summary>
        /// Get or set the LabelFontColor property
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

        /// <summary>
        /// Get or set the LabelFontWeight property
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
        
        /// <summary>
        /// Get or set the LabelFontStyle property
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


        /// <summary>
        /// Get or set the LabelBackground property
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
           
        /// <summary>
        /// Get or set the LabelStyle property
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
        
        /// <summary>
        /// Get or set the LabelLineEnabled property
        /// </summary>
        [System.ComponentModel.TypeConverter(typeof(NullableBoolConverter))]
        public Nullable<Boolean>LabelLineEnabled
        {
            get
            {
                if ((Boolean)LabelEnabled)
                    return (Nullable<Boolean>)GetValue(LabelLineEnabledProperty);

                return false;
            }
            set
            {
                SetValue(LabelLineEnabledProperty, value);
            }
        }


        /// <summary>
        /// Get or set the LabelLineColor property
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



        /// <summary>
        /// Get or set the LabelLineThickness property
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


        /// <summary>
        /// Get or set the LabelLineStyle property
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


        #endregion

        #region Marker Properties

        /// <summary>
        /// Get or set the MarkerEnabled property
        /// </summary>
        [System.ComponentModel.TypeConverter(typeof(NullableBoolConverter))]
        public Nullable<Boolean> MarkerEnabled
        {
            get
            {
                if (this.RenderAs == RenderAs.Line || this.RenderAs == RenderAs.Point)
                    return ((Nullable<Boolean>)GetValue(MarkerEnabledProperty) == null) ? true : (Nullable<Boolean>)GetValue(MarkerEnabledProperty);
                else
                    return ((Nullable<Boolean>)GetValue(MarkerEnabledProperty) == null) ? false : (Nullable<Boolean>)GetValue(MarkerEnabledProperty);
            }
            set
            {   
                SetValue(MarkerEnabledProperty, value);
            }
        }
        
        /// <summary>
        /// Get or set the MarkerStyle property
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
        
        /// <summary>
        /// Get or set the MarkerBorderColor property
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
        
        /// <summary>
        /// Get or set the MarkerSize property
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
        
        /// <summary>
        /// Get or set the MarkerColor property
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

        /// <summary>
        /// Get or set MarkerScale property
        /// </summary>
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



        #endregion Marker Properties

        /// <summary>
        /// Get or set the Internal Start angle property
        /// </summary>
         internal Double InternalStartAngle
         {
             get
             {
                 return (StartAngle % 360) * (Math.PI / 180);
             }
         }
         
        /// <summary>
        /// Get or set the StartAngle property. 
        /// This property is generally used in Pie/Doughnut.
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
                
                SetValue(StartAngleProperty, value);
            }
        }

        #region BorderProperties
        
        /// <summary>
        /// Get or set the BorderThickness property
        /// </summary>
        public new Thickness BorderThickness
        {
            get
            {
                return (Thickness)GetValue(BorderThicknessProperty);
            }
            set
            {
#if SL
                if (BorderThickness != value)
                {
                    SetValue(BorderThicknessProperty, value);
                    FirePropertyChanged("BorderThickness");
                }
#else
                SetValue(BorderThicknessProperty, value);
#endif
            }
        }

        /// <summary>
        /// Get or set the BorderColor property
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
        /// Get or set the BorderStyle property
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
        
        #endregion

        /// <summary>
        /// Get or set the XValueFormatString property
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

        /// <summary>
        /// Get or set the YValueFormatString property
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
        
        /// <summary>
        /// Get or set the ZValueFormatString property
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
        
        /// <summary>
        /// Get or set the ToolTipText property for the DataSeries 
        /// </summary>
        public override String ToolTipText
        {
            get
            {
                if ((Chart != null && !String.IsNullOrEmpty((Chart as Chart).ToolTipText)))
                    return null;

                if (String.IsNullOrEmpty((String)GetValue(ToolTipTextProperty)))
                {   
                    if (GetValue(ToolTipTextProperty) == null)
                        return null;
                    
                    Chart chart = Chart as Chart;

                    switch (RenderAs)
                    {
                        case RenderAs.StackedColumn100:
                        case RenderAs.StackedBar100:
                        case RenderAs.StackedArea100:
                            if (chart.ChartArea.AxisX != null && chart.ChartArea.AxisX.XValueType != ChartValueTypes.Numeric)
                                return "#XValue, #YValue(#Sum)";
                            else
                                return "#AxisXLabel, #YValue(#Sum)";

                        case RenderAs.Pie:
                        case RenderAs.Doughnut:
                            if (chart.ChartArea.AxisX != null && chart.ChartArea.AxisX.XValueType != ChartValueTypes.Numeric)
                                return "#XValue, #YValue(#Percentage%)";
                            else
                                return "#AxisXLabel, #YValue(#Percentage%)";

                        default:
                            if (chart.ChartArea.AxisX != null && chart.ChartArea.AxisX.XValueType != ChartValueTypes.Numeric)
                                return "#XValue, #YValue";
                            else
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
        /// Get or set the AxisXType property
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
        
        /// <summary>
        /// Get or set the AxisYType property
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

        /// <summary>
        /// Type of scale used in axis
        /// </summary>
        public ChartValueTypes XValueType
        {
            get
            {
                return (ChartValueTypes)GetValue(XValueTypeProperty);
            }
            set
            {
                SetValue(XValueTypeProperty, value);
            }
        }


        #endregion

        #region Public Events

        #endregion

        #region Protected Methods

        /// <summary>
        /// UpdateVisual is used for partial rendering
        /// </summary>
        /// <param name="propertyName">Name of the property</param>
        /// <param name="value">Value of the property</param>
        internal override void UpdateVisual(String propertyName, object value)
        {
            if (propertyName == "Color")
            {
                if (RenderAs == RenderAs.Area || RenderAs == RenderAs.StackedArea || RenderAs == RenderAs.StackedArea100)
                {
                    if (Faces != null && Faces.Parts != null)
                    {

                        if ((Chart as Chart).View3D)
                        {
                            Brush sideBrush = (Boolean)LightingEnabled ? Graphics.GetRightFaceBrush((Brush)value) : (Brush)value;
                            Brush topBrush = (Boolean)LightingEnabled ? Graphics.GetTopFaceBrush((Brush)value) : (Brush)value;

                            foreach (FrameworkElement fe in Faces.Parts)
                            {
                                if (fe.Tag.ToString() == "AreaBase")
                                    (fe as Shape).Fill = (Boolean)LightingEnabled ? Graphics.GetFrontFaceBrush((Brush)value) : (Brush)value;
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
                                    (fe as Shape).Fill = (Boolean)LightingEnabled ? Graphics.GetLightingEnabledBrush((Brush)value, "Linear", null) : (Brush)value;
                                }
                                else if (fe.Tag.ToString() == "Bevel")
                                {
                                    (fe as Shape).Fill = Graphics.GetBevelTopBrush((Brush)value);
                                }
                            }
                        }

                        foreach (DataPoint dp in InternalDataPoints)
                            dp.UpdateVisual("Color", null);
                    }
                }
                else if (RenderAs == RenderAs.Line)
                {
                    if (VisualParams != null)
                    {
                        LineChartShapeParams lineParams = VisualParams as LineChartShapeParams;
                        (Faces.Parts[0] as Path).Stroke = lineParams.Lighting ? Graphics.GetLightingEnabledBrush((Brush)value, "Linear", new Double[] { 0.65, 0.55 }) : (Brush)value;
                        foreach (DataPoint dp in InternalDataPoints)
                            dp.UpdateVisual("Color", null);
                    }
                }
                else
                    foreach (DataPoint dp in InternalDataPoints)
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

        /// <summary>
        /// Collection of InternalDataPoints used for calculation
        /// </summary>
        internal List<DataPoint> InternalDataPoints
        {
            get;
            set;
        }

        /// <summary>
        /// Internal XValue Type 
        /// </summary>
        internal ChartValueTypes InternalXValueType
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

        /// <summary>
        /// Marker associated with DataSeries and shown in legend
        /// </summary>
        internal Marker LegendMarker
        {
            get;
            set;
        }

        /// <summary>
        /// This storyboard is used for animating the DataSeries
        /// </summary>
        internal Storyboard Storyboard
        {
            get;
            set;
        }

        /// <summary>
        /// Total Count of DataSeries of the group to which this series belongs. 
        /// This count is helpful while rendering, the space allocated for the columns at a particular InternalXValue 
        /// must be divided between indivisual datapoints of different series with same InternalXValue
        /// </summary>
        internal Int32 SeriesCountOfSameRenderAs
        {
            get;
            set;
        }

        /// <summary>
        /// Faces holds the visual object references associated with this DataSeries
        /// </summary>
        internal Faces Faces
        {
            get;
            set;
        }
        
        /// <summary>
        /// PlotGroup associated with the DataSeries
        /// </summary>
        internal PlotGroup PlotGroup
        {
            get;
            set;
        }


        /// <summary>
        /// InternalLegendName is used for automatic linking a Legend with a DataSeries
        /// </summary>
        internal String InternalLegendName
        {
            get;
            set;
        }

        #endregion

        #region Private Delegates

        #endregion

        #region Private Property

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        private new Brush Background
        {
            get;
            set;
        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        private new Brush BorderBrush
        {
            get;
            set;
        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        private new Brush Foreground
        {
            get;
            set;
        }

        #endregion
        
        #region Private Methods

        /// <summary>
        /// EnabledProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnEnabledPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataSeries dataSeries = d as DataSeries;
            dataSeries.FirePropertyChanged("Enabled");
        }

#if WPF
        /// <summary>
        /// OpacityProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnOpacityPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataSeries dataSeries = d as DataSeries;
            dataSeries.FirePropertyChanged("Opacity");
        }
#endif

        /// <summary>
        /// RenderAsProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnRenderAsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataSeries dataSeries = d as DataSeries;
            dataSeries._internalColor = null;
            dataSeries.FirePropertyChanged("RenderAs");
        }

        /// <summary>
        /// HrefTargetProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnHrefTargetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataSeries dataSeries = d as DataSeries;
            dataSeries.FirePropertyChanged("HrefTarget");
        }

        /// <summary>
        /// HrefProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnHrefChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataSeries dataSeries = d as DataSeries;
            dataSeries.FirePropertyChanged("Href");
        }

        /// <summary>
        /// ColorProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataSeries dataSeries = d as DataSeries;
            dataSeries.UpdateVisual("Color", e.NewValue);
        }

        /// <summary>
        /// LightingEnabledProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnLightingEnabledPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataSeries dataSeries = d as DataSeries;
            dataSeries.FirePropertyChanged("LightingEnabled");
        }

        /// <summary>
        /// ShadowEnabledProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnShadowEnabledPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataSeries dataSeries = d as DataSeries;
            dataSeries.FirePropertyChanged("ShadowEnabled");
        }

        /// <summary>
        /// LegendTextProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnLegendTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataSeries dataSeries = d as DataSeries;
            dataSeries.FirePropertyChanged("LegendText");
            dataSeries.InternalLegendName = (String)e.NewValue;
        }

        /// <summary>
        /// LegendProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnLegendPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataSeries dataSeries = d as DataSeries;
            dataSeries.FirePropertyChanged("Legend");
        }

        /// <summary>
        /// BevelProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnBevelPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataSeries dataSeries = d as DataSeries;
            dataSeries.FirePropertyChanged("Bevel");
        }

        /// <summary>
        /// ColorSetProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnColorSetPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataSeries dataSeries = d as DataSeries;
            dataSeries.FirePropertyChanged("ColorSet");
        }
        
        /// <summary>
        /// RadiusXProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnRadiusXPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataSeries dataSeries = d as DataSeries;
            dataSeries.FirePropertyChanged("RadiusX");
        }

        /// <summary>
        /// RadiusYProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnRadiusYPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataSeries dataSeries = d as DataSeries;
            dataSeries.FirePropertyChanged("RadiusY");
        }

        /// <summary>
        /// LineThicknessProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnLineThicknessPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataSeries dataSeries = d as DataSeries;
            dataSeries.FirePropertyChanged("LineThickness");
        }

        /// <summary>
        /// LineStyleProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnLineStylePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataSeries dataSeries = d as DataSeries;
            dataSeries.FirePropertyChanged("LineStyle");
        }

        /// <summary>
        /// ShowInLegendProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnShowInLegendPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataSeries dataSeries = d as DataSeries;
            dataSeries.FirePropertyChanged("ShowInLegend");
        }

        /// <summary>
        /// LabelEnabledProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnLabelEnabledPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataSeries dataSeries = d as DataSeries;
            dataSeries.FirePropertyChanged("LabelEnabled");
        }

        /// <summary>
        /// LabelTextProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnLabelTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataSeries dataSeries = d as DataSeries;
            dataSeries.FirePropertyChanged("LabelText");
        }

        /// <summary>
        /// LabelFontFamilyProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnLabelFontFamilyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataSeries dataSeries = d as DataSeries;
            dataSeries.FirePropertyChanged("LabelFontFamily");
        }

        /// <summary>
        /// LabelFontSizeProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnLabelFontSizePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataSeries dataSeries = d as DataSeries;
            dataSeries.FirePropertyChanged("LabelFontSize");
        }

        /// <summary>
        /// LabelFontColorProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnLabelFontColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataSeries dataSeries = d as DataSeries;
            dataSeries.FirePropertyChanged("LabelFontColor");
        }

        /// <summary>
        /// LabelFontWeightProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnLabelFontWeightPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataSeries dataSeries = d as DataSeries;
            dataSeries.FirePropertyChanged("LabelFontWeight");
        }

        /// <summary>
        /// LabelFontStyleProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnLabelFontStylePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataSeries dataSeries = d as DataSeries;
            dataSeries.FirePropertyChanged("LabelFontStyle");
        }

        /// <summary>
        /// LabelBackgroundProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnLabelBackgroundPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataSeries dataSeries = d as DataSeries;
            dataSeries.FirePropertyChanged("LabelBackground");
        }

        /// <summary>
        /// LabelStyleProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnLabelStylePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataSeries dataSeries = d as DataSeries;
            dataSeries.FirePropertyChanged("LabelStyle");
        }

        /// <summary>
        /// LabelLineEnabledProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnLabelLineEnabledPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataSeries dataSeries = d as DataSeries;
            dataSeries.FirePropertyChanged("LabelLineEnabled");
        }
        
        /// <summary>
        /// LabelLineColorProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnLabelLineColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataSeries dataSeries = d as DataSeries;
            dataSeries.FirePropertyChanged("LabelLineColor");
        }

        /// <summary>
        /// LabelLineThicknessProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnLabelLineThicknessPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataSeries dataSeries = d as DataSeries;
            dataSeries.FirePropertyChanged("LabelLineThickness");
        }

        /// <summary>
        /// LabelLineStyleProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnLabelLineStylePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataSeries dataSeries = d as DataSeries;
            dataSeries.FirePropertyChanged("LabelLineStyle");
        }

        /// <summary>
        /// MarkerEnabledProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnMarkerEnabledPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataSeries dataSeries = d as DataSeries;
            dataSeries.FirePropertyChanged("MarkerEnabled");
        }
        
        /// <summary>
        /// MarkerTypeProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnMarkerTypePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataSeries dataSeries = d as DataSeries;
            dataSeries.FirePropertyChanged("MarkerType");
        }

        /// <summary>
        /// MarkerBorderThicknessProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnMarkerBorderThicknessPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataSeries dataSeries = d as DataSeries;
            dataSeries.UpdateMarkers();
        }

        /// <summary>
        /// MarkerBorderColorProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnMarkerBorderColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataSeries dataSeries = d as DataSeries;
            dataSeries.UpdateMarkers();
        }

        /// <summary>
        /// MarkerSizeProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnMarkerSizePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataSeries dataSeries = d as DataSeries;
            dataSeries.FirePropertyChanged("MarkerSize");
        }

        /// <summary>
        /// MarkerColorProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnMarkerColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataSeries dataSeries = d as DataSeries;
            dataSeries.UpdateMarkers();
        }

        /// <summary>
        /// MarkerScaleProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnMarkerScalePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataSeries dataSeries = d as DataSeries;
            dataSeries.FirePropertyChanged("MarkerScale");
        }

        /// <summary>
        /// StartAngleProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnStartAnglePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataSeries dataSeries = d as DataSeries;
            dataSeries.FirePropertyChanged("StartAngle");
        }
        
#if WPF
        /// <summary>
        /// BorderThicknessProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnBorderThicknessPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataSeries dataSeries = d as DataSeries;
            dataSeries.FirePropertyChanged("BorderThickness");
        }
#endif

        /// <summary>
        /// BorderColorProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnBorderColorPropertychanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataSeries dataSeries = d as DataSeries;
            dataSeries.FirePropertyChanged("BorderColor");
        }

        /// <summary>
        /// BorderStyleProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnBorderStylePropertychanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataSeries dataSeries = d as DataSeries;
            dataSeries.FirePropertyChanged("BorderStyle");
        }

        /// <summary>
        /// XValueFormatStringProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnXValueFormatStringPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataSeries dataSeries = d as DataSeries;
            dataSeries.FirePropertyChanged("XValueFormatString");
        }
        
        /// <summary>
        /// YValueFormatStringProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnYValueFormatStringPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataSeries dataSeries = d as DataSeries;
            dataSeries.FirePropertyChanged("YValueFormatString");
        }

        /// <summary>
        /// ZValueFormatStringProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnZValueFormatStringPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataSeries dataSeries = d as DataSeries;
            dataSeries.FirePropertyChanged("ZValueFormatString");
        }

        /// <summary>
        /// AxisXTypeProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnAxisXTypePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataSeries dataSeries = d as DataSeries;
            dataSeries.FirePropertyChanged("AxisXType");
        }

        /// <summary>
        /// AxisYTypeProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnAxisYTypePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataSeries dataSeries = d as DataSeries;
            dataSeries.FirePropertyChanged("AxisYType");
        }

        /// <summary>
        /// XValueTypeProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnXValueTypePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataSeries dataSeries = d as DataSeries;
            dataSeries.InternalXValueType = (ChartValueTypes) e.NewValue;
            dataSeries.FirePropertyChanged("XValueType");
        }
        
        /// <summary>
        /// ZIndexProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnZIndexPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataSeries dataSeries = d as DataSeries;
            dataSeries.InternalZIndex = (Int32) e.NewValue;
            dataSeries.FirePropertyChanged("ZIndex");
        }

        /// <summary>
        /// Update marker partially
        /// </summary>
        private void UpdateMarkers()
        {
            foreach (DataPoint dataPoint in InternalDataPoints)
                dataPoint.UpdateMarker();
        }
        
        /// <summary>
        /// InternalDataPoints collection changed event handler
        /// </summary>
        /// <param name="sender">InternalDataPoints</param>
        /// <param name="e">NotifyCollectionChangedEventArgs</param>
        private void DataPoints_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
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

                        if (Double.IsNaN(dataPoint.InternalXValue))
                            dataPoint.InternalXValue = this.DataPoints.Count;
                        dataPoint.SetValue(NameProperty, dataPoint.GetType().Name + this.DataPoints.IndexOf(dataPoint));

                        dataPoint.PropertyChanged -= DataPoint_PropertyChanged;
                        dataPoint.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(DataPoint_PropertyChanged);
                    }
                }
            }

            this.FirePropertyChanged("DataPoints");
        }

        /// <summary>
        /// Find nearest DataPoint by InternalXValue
        /// </summary>
        /// <param name="xValue">Double InternalXValue</param>
        /// <returns>DataPoint</returns>
        private DataPoint GetNearestDataPoint(Double xValue)
        {
            DataPoint dp = this.InternalDataPoints[0];
            Double diff = Math.Abs(dp.InternalXValue - xValue);

            for (Int32 i = 1; i < this.InternalDataPoints.Count; i++)
            {
                if (Math.Abs(this.InternalDataPoints[i].InternalXValue - xValue) < diff)
                {
                    diff = Math.Abs(this.InternalDataPoints[i].InternalXValue - xValue);
                    dp = this.InternalDataPoints[i];
                }
            }

            return dp;
        }

        /// <summary>
        /// DataPoint property changed event handler
        /// </summary>
        /// <param name="sender">DataSeries</param>
        /// <param name="e">PropertyChangedEventArgs</param>
        private void DataPoint_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            this.FirePropertyChanged(e.PropertyName);
        }
    
        #endregion

        #region Internal Methods

        /// <summary>
        /// Attach events to each and every visual face in Faces
        /// </summary>
        internal void AttachEvent2DataSeriesVisualFaces()
        {
            foreach (DataPoint dp in InternalDataPoints)
            {
                dp.AttachEvent2DataPointVisualFaces(this);
            }
        }

        /// <summary>
        /// Attach tooltip with a framework element
        /// </summary>
        /// <param name="control">Control reference</param>
        /// <param name="elements">FrameworkElement list</param>
        internal void AttachAreaToolTip(VisifireControl control, List<FrameworkElement> elements)
        {
            // Show ToolTip on mouse move over the chart element
            foreach (FrameworkElement element in elements)
            {
                element.MouseMove += delegate(object sender, MouseEventArgs e)
                {
                    Point position = e.GetPosition(this.Faces.Visual);
                    Double xValue = Graphics.PixelPositionToValue(0, this.Faces.Visual.Width, (Double)(control as Chart).ChartArea.AxisX.AxisManager.AxisMinimumValue, (Double)(control as Chart).ChartArea.AxisX.AxisManager.AxisMaximumValue, position.X);
                    DataPoint dataPoint = GetNearestDataPoint(xValue);

                    if (dataPoint.ToolTipText == null)
                    {
                        control._toolTip.Text = "";
                        control._toolTip.Hide();
                        return;
                    }
                    else
                    {
                        control._toolTip.Text = dataPoint.TextParser(dataPoint.ToolTipText);

                        if (control.ToolTipEnabled)
                            control._toolTip.Show();

                        (control as Chart).UpdateToolTipPosition(sender, e);
                    }
                };

                // Hide ToolTip on mouse out from the chart element
                element.MouseLeave += delegate(object sender, MouseEventArgs e)
                {
                    control._toolTip.Hide();
                };
            }
        }

        #endregion

        #region Internal Events

        #endregion

        #region Data

        /// <summary>
        /// Internal color holds color from theme
        /// </summary>
        internal Brush _internalColor;   

#if WPF
        /// <summary>
        /// Whether the default style is applied
        /// </summary>
        private static Boolean _defaultStyleKeyApplied;
#endif
        #endregion

    }

}
