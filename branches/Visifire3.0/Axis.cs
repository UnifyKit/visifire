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

#endif

using Visifire.Commons;
using Visifire.Charts;

namespace Visifire.Charts
{
    /// <summary>
    /// Visifire.Charts.Axis class
    /// </summary>
#if SL
    [System.Windows.Browser.ScriptableType]
#endif
    public class Axis : ObservableObject
    {
        #region Public Methods

        /// <summary>
        /// Initializes a new instance of the Visifire.Charts.Axis class
        /// </summary>
        public Axis()
        {
            // Initialize list of ChartGrid list
            Grids = new ChartGridCollection();

            // Initialize list of Ticks list 
            Ticks = new TicksCollection();

            // Initialize AxisLabels element
            AxisLabels = new AxisLabels();

            // Attach event handler on collection changed event with chart grid collection
            Grids.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(Grids_CollectionChanged);

            // Attach event handler on collection changed event with ticks collection
            Ticks.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(Ticks_CollectionChanged);

            InternalAxisMinimum = Double.NaN;
            InternalAxisMaximum = Double.NaN;

        }

        #endregion

        #region Public Properties


        // Using a DependencyProperty as the backing store for ScrollBarScale.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ScrollBarScaleProperty =
            DependencyProperty.Register("ScrollBarScale", typeof(double), typeof(Axis), new PropertyMetadata(Double.NaN, OnScrollBarScalePropertyChanged));

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
        private new static readonly DependencyProperty BackgroundProperty = DependencyProperty.Register
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
            typeof(Double),
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
        public Double LineThickness
        {
            get
            {
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
                        return true;
                    else
                        return false;
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
                ParseScalingSets(value);
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
        /// Internal axis minimum is used for internal calculation purpose
        /// </summary>
        internal Double InternalAxisMinimum
        {
            get;
            set;
        }

        /// <summary>
        /// Internal axis maximum is used for internal calculation purpose
        /// </summary>
        internal Double InternalAxisMaximum
        {
            get;
            set;
        }

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

        /// <summary>
        /// Get or set the scroll bar
        /// </summary>
        internal ScrollBar ScrollBarElement
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
        internal Orientation AxisOrientation
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
        /// Get or set the width of the axis
        /// </summary>
        internal new Double Width
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set the height of the axis
        /// </summary>
        internal new Double Height
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
        private static void OnScrollBarScalePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Axis axis = d as Axis;

            //if (axis._isScrollToOffsetEnabled)
            //    axis.SetScrollBarValueFromOffset((Double)e.NewValue);
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

#if WPF 

        // <summary>
        /// Event handler attached with Padding property changed event of AxisLabels elements
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnPaddingPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Axis axis = d as Axis;
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
            axis.FirePropertyChanged(VcProperties.Opacity);
        }
#endif

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
        /// Event handler manages axis maximum property change event of axis
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnAxisMaximumPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Axis axis = d as Axis;

            Double numericVal = axis.AxisMaximumNumeric;
            DateTime dateTimeValue = axis.AxisMaximumDateTime;
            Axis.ConvertValueToDateTimeOrNumeric("AxisMaximum", e.NewValue, ref numericVal, ref dateTimeValue, out axis._axisMaximumValueType);
            axis.AxisMaximumNumeric = numericVal;
            axis.AxisMaximumDateTime = dateTimeValue;
            
            axis.FirePropertyChanged(VcProperties.AxisMaximum);
        }

        /// <summary>
        /// Event handler manages axis minimum property change event of axis
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnAxisMinimumPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Axis axis = d as Axis;


            Double numericVal = axis.AxisMinimumNumeric;
            DateTime dateTimeValue = axis.AxisMinimumDateTime;
            Axis.ConvertValueToDateTimeOrNumeric("AxisMinimum", e.NewValue, ref numericVal, ref dateTimeValue, out axis._axisMinimumValueType);
            axis.AxisMinimumNumeric = numericVal;
            axis.AxisMinimumDateTime = dateTimeValue;

            axis.FirePropertyChanged(VcProperties.AxisMinimum);
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
                axis.SetScrollBarValueFromOffset((Double)e.NewValue);
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
        /// Set up axis manager and calculate axis values
        /// </summary>
        private void SetUpAxisManager()
        {
            // Get the minimum and maximum value dependeing on the axis representation value
            Minimum = AxisRepresentation == AxisRepresentations.AxisX ? PlotDetails.GetAxisXMinimumDataValue(this) : PlotDetails.GetAxisYMinimumDataValue(this);
            Maximum = AxisRepresentation == AxisRepresentations.AxisX ? PlotDetails.GetAxisXMaximumDataValue(this) : PlotDetails.GetAxisYMaximumDataValue(this);

            Boolean overflowValidity = (AxisRepresentation == AxisRepresentations.AxisX);
            Boolean stackingOverride = PlotDetails.GetStacked100OverrideState();

            // Create and initialize the AxisManagers
            AxisManager = new AxisManager(Maximum, Minimum, (Boolean)StartFromZero, overflowValidity, stackingOverride, AxisRepresentation);

            // Set the include zero state
            AxisManager.IncludeZero = IncludeZero;

            // settings specific to axis X
            if (AxisRepresentation == AxisRepresentations.AxisX)
            {
                Double interval = GenerateDefaultInterval();

                if (IsDateTimeAxis)
                {
                    if (interval > 0 || !Double.IsNaN(interval) || IntervalType == IntervalTypes.Auto)
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

            // set the axis minimum value if the user has provided it
            if (!Double.IsNaN((Double)AxisMinimumNumeric))
            {
                AxisManager.AxisMinimumValue = (Double)AxisMinimumNumeric;
                InternalAxisMinimum = (Double)AxisMinimumNumeric;
            }

            // Calculate the various parameters for creating the axis
            AxisManager.Calculate();

            // Set axis specific limits based on axis limits.
            // if (AxisRepresentation == AxisRepresentations.AxisX && !(Boolean)StartFromZero)
            //      if (!SetAxesXLimits())
            //          return;

            if (AxisRepresentation == AxisRepresentations.AxisX)
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

                    // This will set the internal overriding flag
                    AxisManager.AxisMinimumValue = AxisManager.AxisMinimumValue;
                    AxisManager.AxisMaximumValue = AxisManager.AxisMaximumValue;

                    // This interval will reflect the interval in primary axis it is not same as that of primary axis
                    AxisManager.Interval = (AxisManager.AxisMaximumValue - AxisManager.AxisMinimumValue) / primaryAxisIntervalCount;

                    AxisManager.Calculate();
                }

                InternalAxisMaximum = AxisManager.AxisMaximumValue;
                InternalAxisMinimum = AxisManager.AxisMinimumValue;
            }
            else
            {
                // Set the internal axis limits the one obtained from axis manager
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

            if (this.TitleFontFamily != null)
                AxisTitleElement.FontFamily = this.TitleFontFamily;

            if (this.TitleFontSize != 0)
                AxisTitleElement.FontSize = this.TitleFontSize;

            if (this.TitleFontStyle != null)
                AxisTitleElement.FontStyle = this.TitleFontStyle;

            if (this.TitleFontWeight != null)
                AxisTitleElement.FontWeight = this.TitleFontWeight;

            if (!String.IsNullOrEmpty(this.Title) && String.IsNullOrEmpty(AxisTitleElement.Text))
                AxisTitleElement.Text = GetFormattedMultilineText(this.Title);

            AxisTitleElement.FontColor = Visifire.Charts.Chart.CalculateFontColor((Chart as Chart), this.TitleFontColor, false);

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
            AxisLine.StrokeThickness = LineThickness;
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

        /// <summary>
        /// Applies setting for primary vertical axis (Primary axis Y or Primary axis X in Bar)
        /// </summary>
        private void ApplyVerticalPrimaryAxisSettings()
        {
            // Set the parameters fo the Axis Stack panel
            Visual.Children.Add(new Border() { Width = this.Padding.Left });
            Visual.HorizontalAlignment = HorizontalAlignment.Left;
            Visual.VerticalAlignment = VerticalAlignment.Stretch;
            Visual.Orientation = Orientation.Horizontal;

            InternalStackPanel.Width = 0;
            InternalStackPanel.HorizontalAlignment = HorizontalAlignment.Left;
            InternalStackPanel.VerticalAlignment = VerticalAlignment.Stretch;
            ScrollViewerElement.VerticalAlignment = VerticalAlignment.Stretch;

            // Set the parameters for the scroll bar
            ScrollBarElement.Orientation = Orientation.Vertical;
            ScrollBarElement.Width = 10;

            // Set the parameters for the axis labels
            AxisLabels.Placement = PlacementTypes.Left;
            AxisLabels.Height = ScrollableSize;

            CreateAxisLine(StartOffset, Height - EndOffset, LineThickness / 2, LineThickness / 2, LineThickness, this.Height);

            // Set parameters for the Major Grids
            foreach (ChartGrid grid in Grids)
                grid.Placement = PlacementTypes.Left;

            // Generate the visual object for the required elements
            AxisLabels.CreateVisualObject();

            // Set the alignement for the axis Title
            AxisTitleElement.HorizontalAlignment = HorizontalAlignment.Left;
            AxisTitleElement.VerticalAlignment = VerticalAlignment.Center;

            CreateAxisTitleVisual(new Thickness(INNER_MARGIN, 0, INNER_MARGIN, 0));

            // Place the visual elements in the axis stack panel
            if (!String.IsNullOrEmpty(Title))
            {
                Visual.Children.Add(AxisTitleElement.Visual);
            }

            if (AxisLabels.Visual != null)
            {
                InternalStackPanel.Width += AxisLabels.Visual.Width;

                if (Height == ScrollableSize)
                {
                    if (AxisLabels.Visual != null)
                        Visual.Children.Add(AxisLabels.Visual);
                }
                else
                {
                    InternalStackPanel.Children.Add(AxisLabels.Visual);
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
                        Visual.Children.Add(tick.Visual);
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
                Visual.Children.Add(ScrollViewerElement);
            }

            Visual.Children.Add(AxisLine);

            InternalStackPanel.Width += AxisLine.Width;

            ScrollViewerElement.Width = InternalStackPanel.Width;

            ClipVerticalAxis(ticksWidth);
        }

        /// <summary>
        /// Applies setting for secondary vertical axis (Secondary axis Y or Secondary axis X in Bar)
        /// </summary>
        private void ApplyVerticalSecondaryAxisSettings()
        {
            // Set the parameters fo the Axis Stack panel
            Visual.HorizontalAlignment = HorizontalAlignment.Right;
            Visual.VerticalAlignment = VerticalAlignment.Stretch;
            Visual.Orientation = Orientation.Horizontal;

            InternalStackPanel.HorizontalAlignment = HorizontalAlignment.Right;
            InternalStackPanel.VerticalAlignment = VerticalAlignment.Stretch;

            InternalStackPanel.SizeChanged += delegate(object sender, SizeChangedEventArgs e)
            {
                ScrollViewerElement.Width = e.NewSize.Width;
            };

            ScrollViewerElement.VerticalAlignment = VerticalAlignment.Stretch;

            // Set the parameters for the scroll bar
            ScrollBarElement.Orientation = Orientation.Vertical;
            ScrollBarElement.Width = 10;

            // Set the parameters for the axis labels
            AxisLabels.Placement = PlacementTypes.Right;
            AxisLabels.Height = ScrollableSize;

            CreateAxisLine(StartOffset, Height - EndOffset, LineThickness / 2, LineThickness / 2, LineThickness, this.Height);

            // Set parameters for the Major Grids
            foreach (ChartGrid grid in Grids)
                grid.Placement = PlacementTypes.Right;

            // Set the alignement for the axis Title
            AxisTitleElement.HorizontalAlignment = HorizontalAlignment.Right;
            AxisTitleElement.VerticalAlignment = VerticalAlignment.Center;

            // Generate the visual object for the required elements
            AxisLabels.CreateVisualObject();

            // Place the visual elements in the axis stack panel
            Visual.Children.Add(AxisLine);

            foreach (Ticks tick in Ticks)
            {
                tick.SetParms(PlacementTypes.Right, Double.NaN, ScrollableSize);

                tick.CreateVisualObject();
                if (tick.Visual != null)
                {
                    if (Height == ScrollableSize)
                        Visual.Children.Add(tick.Visual);
                    else
                        InternalStackPanel.Children.Add(tick.Visual);

                }
            }

            if (Height == ScrollableSize)
            {
                if (AxisLabels.Visual != null)
                    Visual.Children.Add(AxisLabels.Visual);
            }
            else
            {
                InternalStackPanel.Children.Add(AxisLabels.Visual);

                ScrollViewerElement.Children.Add(InternalStackPanel);

                Visual.Children.Add(ScrollViewerElement);
            }

            CreateAxisTitleVisual(new Thickness(INNER_MARGIN, 0, INNER_MARGIN, 0));
            if (!String.IsNullOrEmpty(Title))
            {
                Visual.Children.Add(AxisTitleElement.Visual);
            }

            Visual.Children.Add(new Border() { Width = this.Padding.Right });
        }

        /// <summary>
        /// Applies setting for primary horizontal axis (Primary axis X or Primary axis Y in Bar)
        /// </summary>
        private void ApplyHorizontalPrimaryAxisSettings()
        {
            // Set the parameters fo the Axis Stack panel
            Visual.HorizontalAlignment = HorizontalAlignment.Stretch;
            Visual.VerticalAlignment = VerticalAlignment.Bottom;
            Visual.Orientation = Orientation.Vertical;
            InternalStackPanel.Height = 0;
            InternalStackPanel.HorizontalAlignment = HorizontalAlignment.Stretch;
            InternalStackPanel.VerticalAlignment = VerticalAlignment.Bottom;

            ScrollViewerElement.HorizontalAlignment = HorizontalAlignment.Stretch;

            // Set the parameters for the scroll bar
            ScrollBarElement.Orientation = Orientation.Horizontal;
            ScrollBarElement.Height = 10;

            // Set the parameters for the axis labels
            AxisLabels.Placement = PlacementTypes.Bottom;
            AxisLabels.Width = ScrollableSize;

            CreateAxisLine(LineThickness / 2, LineThickness / 2, StartOffset, Width - EndOffset, this.Width, LineThickness);

            // Set parameters for the Major Grids
            foreach (ChartGrid grid in Grids)
                grid.Placement = PlacementTypes.Bottom;

            // Set the alignement for the axis Title
            AxisTitleElement.HorizontalAlignment = HorizontalAlignment.Center;
            AxisTitleElement.VerticalAlignment = VerticalAlignment.Bottom;

            // Generate the visual object for the required elements
            AxisLabels.CreateVisualObject();

            // Place the visual elements in the axis stack panel
            Visual.Children.Add(AxisLine);

            Double ticksHeight = 0;
            //AxisLabels.Visual.Background = new SolidColorBrush(Colors.Orange);
            foreach (Ticks tick in Ticks)
            {
                tick.SetParms(PlacementTypes.Bottom, ScrollableSize, Double.NaN);

                tick.CreateVisualObject();
                if (tick.Visual != null)
                {
                    if (Width == ScrollableSize)
                        Visual.Children.Add(tick.Visual);
                    else
                    {
                        InternalStackPanel.Children.Add(tick.Visual);
                        tick.Visual.SetValue(Canvas.TopProperty, ticksHeight);
                        ticksHeight += tick.Visual.Height;
                    }
                }
            }

            InternalStackPanel.Height += ticksHeight;

            if (Width == ScrollableSize)
            {
                if (AxisLabels.Visual != null)
                    Visual.Children.Add(AxisLabels.Visual);
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

                ScrollViewerElement.Children.Add(InternalStackPanel);
                Visual.Children.Add(ScrollViewerElement);
            }

            ScrollViewerElement.Height = InternalStackPanel.Height;

            ClipHorizontalAxis(ticksHeight);

            CreateAxisTitleVisual(new Thickness(0, INNER_MARGIN, 0, INNER_MARGIN));

            if (!String.IsNullOrEmpty(Title))
            {
                Visual.Children.Add(AxisTitleElement.Visual);
            }

            Visual.Children.Add(new Border() { Height = this.Padding.Bottom });
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
            Visual.Children.Add(new Border() { Height = this.Padding.Top });
            Visual.HorizontalAlignment = HorizontalAlignment.Stretch;
            Visual.VerticalAlignment = VerticalAlignment.Top;
            Visual.Orientation = Orientation.Vertical;

            InternalStackPanel.HorizontalAlignment = HorizontalAlignment.Stretch;
            InternalStackPanel.VerticalAlignment = VerticalAlignment.Top;

            InternalStackPanel.SizeChanged += delegate(object sender, SizeChangedEventArgs e)
            {
                ScrollViewerElement.Height = e.NewSize.Height;
            };

            ScrollViewerElement.HorizontalAlignment = HorizontalAlignment.Stretch;

            // Set the parameters for the scroll bar
            ScrollBarElement.Orientation = Orientation.Horizontal;
            ScrollBarElement.Height = 10;

            // Set the parameters for the axis labels
            AxisLabels.Placement = PlacementTypes.Top;
            AxisLabels.Width = ScrollableSize;

            CreateAxisLine(LineThickness / 2, LineThickness / 2, StartOffset, Width - EndOffset, this.Width, LineThickness);

            // Set parameters for the Major Grids
            foreach (ChartGrid grid in Grids)
                grid.Placement = PlacementTypes.Top;

            // Set the alignement for the axis Title
            AxisTitleElement.HorizontalAlignment = HorizontalAlignment.Center;
            AxisTitleElement.VerticalAlignment = VerticalAlignment.Top;

            AxisLabels.CreateVisualObject();

            CreateAxisTitleVisual(new Thickness(0, INNER_MARGIN, 0, INNER_MARGIN));

            // Place the visual elements in the axis stack panel
            if (!String.IsNullOrEmpty(Title))
            {
                Visual.Children.Add(AxisTitleElement.Visual);
            }

            if (AxisLabels.Visual != null)
            {
                if (Width == ScrollableSize)
                    Visual.Children.Add(AxisLabels.Visual);
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
                        Visual.Children.Add(tick.Visual);
                    else
                        InternalStackPanel.Children.Add(tick.Visual);
                }
            }

            if (Width != ScrollableSize)
            {
                ScrollViewerElement.Children.Add(InternalStackPanel);

                Visual.Children.Add(ScrollViewerElement);
            }

            Visual.Children.Add(AxisLine);
        }

        /// <summary>
        /// Create axis title visual
        /// </summary>
        /// <param name="margin">Margin between axis title and axis scale</param>
        private void CreateAxisTitleVisual(Thickness margin)
        {
            AxisTitleElement.Margin = margin;

            AxisTitleElement.IsNotificationEnable = false;

        RECAL:

            AxisTitleElement.CreateVisualObject();
#if WPF
            AxisTitleElement.Visual.FlowDirection = FlowDirection.LeftToRight;
#endif
            Size size = Graphics.CalculateVisualSize(AxisTitleElement.Visual);

            if (AxisOrientation == Orientation.Horizontal)
            {
                if (size.Width > Width && Width != 0)
                {
                    if (AxisTitleElement.FontSize == 0.2)
                        goto RETURN;

                    AxisTitleElement.FontSize -= 0.2;

                    goto RECAL;
                }
            }
            else
            {
                if (size.Height > Height && Height != 0)
                {
                    if (AxisTitleElement.FontSize == 0.2)
                        goto RETURN;

                    AxisTitleElement.FontSize -= 0.2;
                    goto RECAL;
                }
            }

        RETURN:

            AxisTitleElement.IsNotificationEnable = true;
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
                        
                        if (temp >= 1)
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
        /// Apply axis visual properties
        /// </summary>
        private void ApplyVisualProperty()
        {
            Visual.Cursor = (Cursor == null) ? Cursors.Arrow : Cursor;
            AttachHref(Chart, Visual, Href, HrefTarget);
            AttachToolTip(Chart, this, Visual);
            AttachEvents2Visual(this, this.Visual);
            Visual.Opacity = this.Opacity;
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

        #endregion

        #region Internal Methods

        /// <summary>
        /// Converts pixel position to value
        /// </summary>
        /// <param name="maxWidth">Pixel width of the scale</param>
        /// <param name="position">Pixel position</param>
        /// <returns>Double</returns>
        internal Double PixelPositionToValue(Double maxWidth, Double pixelPosition)
        {
            return Graphics.PixelPositionToValue(0, maxWidth, InternalAxisMinimum, InternalAxisMaximum, pixelPosition);
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
                str = (value / sValue).ToString(ValueFormatString) + sUnit;
            }
            else
            {
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
            if (this.AxisOrientation == Orientation.Vertical)
                ScrollBarElement = (AxisType == AxisTypes.Primary) ? Chart._leftAxisScrollBar : Chart._rightAxisScrollBar;
            else
                ScrollBarElement = (AxisType == AxisTypes.Primary) ? Chart._bottomAxisScrollBar : Chart._topAxisScrollBar;
        }

        /// <summary>
        /// Set axis scroll value to scrollbar associated with this axis
        /// </summary>
        /// <param name="offset">Scrollbar offset</param>
        internal void SetScrollBarValueFromOffset(Double offset)
        {
            if (ScrollBarElement != null)
            {
                Double value = GetScrollBarValueFromOffset(offset);
                ScrollBarElement.SetValue(ScrollBar.ValueProperty, value);

                if (Scroll != null)
                    Scroll(ScrollBarElement, new ScrollEventArgs(ScrollEventType.First, value));
            }
        }

        /// <summary>
        /// Get axis scroll value in pixel from scrollbar offset
        /// </summary>
        /// <param name="offset">Scrollbar offset</param>
        internal Double GetScrollBarValueFromOffset(Double offset)
        {
            if (Double.IsNaN(offset))
                offset = 0;

            offset = (ScrollBarElement.Maximum - ScrollBarElement.Minimum) * offset + ScrollBarElement.Minimum;

            if (PlotDetails.ChartOrientation == ChartOrientationType.Horizontal)
                offset = ScrollBarElement.Maximum - offset;

            return offset;
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
                tick.TickLength = 5;
                tick.ParentAxis = this;

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

                grid.IsNotificationEnable = true;
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

        /// <summary>
        /// Creates the visual element for the Axis
        /// </summary>
        /// <param name="Chart">Chart</param>
        internal void CreateVisualObject(Chart Chart)
        {
            IsNotificationEnable = false;
            AxisLabels.IsNotificationEnable = false;
            AxisLabels.Chart = Chart;

            if (AxisRepresentation == AxisRepresentations.AxisX)
                AxisLabels.ApplyStyleFromTheme(Chart, "AxisXLabels");
            else if (AxisRepresentation == AxisRepresentations.AxisY)
                AxisLabels.ApplyStyleFromTheme(Chart, "AxisYLabels");

            // Create visual elements
            Visual = new StackPanel() { Background = Background };
            InternalStackPanel = new Canvas();
            ScrollViewerElement = new Canvas();
            AxisTitleElement = new Title();

            ApplyVisualProperty();
            ApplyTitleProperties();

            SetUpAxisManager();
            SetUpTicks();
            SetUpGrids();
            SetUpAxisLabels();

            // set the placement order based on the axis orientation
            switch (AxisOrientation)
            {
                case Orientation.Horizontal:
                    ApplyHorizontalAxisSettings();
                    break;

                case Orientation.Vertical:
                    ApplyVerticalAxisSettings();
                    break;
            }

            IsNotificationEnable = true;
            AxisLabels.IsNotificationEnable = true;

            if (!(Boolean)this.Enabled)
            {
                Visual.Visibility = Visibility.Collapsed;
            }
        }

        #endregion

        #region Internal Events And Delegates

        /// <summary>
        /// Scroll event of axis
        /// </summary>
        internal event ScrollEventHandler Scroll;

        #endregion

        #region Data

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
        private Orientation _orientation;

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
        /// Margin between axis title and axis scale
        /// </summary>
        private const Double INNER_MARGIN = 4;

        #endregion
    }
}
