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
using System.Windows.Documents;
using System.Windows.Media;
using System.ComponentModel;


#else

using System;
using System.Windows;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using System.Collections.Generic;

#endif
using System.Windows.Data;
using Visifire.Commons;

namespace Visifire.Charts
{
    /// <summary>
    /// Visifire.Charts.AxisLabels class
    /// </summary>
    public class AxisLabels : ObservableObject
    {
        #region Public Methods

        /// <summary>
        /// Initializes a new instance of the Visifire.Charts.AxisLabels class
        /// </summary>
        public AxisLabels()
        {
            // Apply default style from generic
#if WPF
            if (!_defaultStyleKeyApplied)
            {
                DefaultStyleKeyProperty.OverrideMetadata(typeof(AxisLabels), new FrameworkPropertyMetadata(typeof(AxisLabels)));
                _defaultStyleKeyApplied = true;

            }
#else
            DefaultStyleKey = typeof(AxisLabels);
#endif

            WidthOfACharacter = Double.NaN;
            InternalAngle = Double.NaN;
        }

        public override void Bind()
        {
#if SL
            Binding b = new Binding("FontSize");
            b.Source = this;
            this.SetBinding(InternalFontSizeProperty, b);

            b = new Binding("FontFamily");
            b.Source = this;
            this.SetBinding(InternalFontFamilyProperty, b);

            b = new Binding("FontStyle");
            b.Source = this;
            this.SetBinding(InternalFontStyleProperty, b);

            b = new Binding("FontWeight");
            b.Source = this;
            this.SetBinding(InternalFontWeightProperty, b);

            b = new Binding("Opacity");
            b.Source = this;
            this.SetBinding(InternalOpacityProperty, b);
#endif
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Identifies the Visifire.Charts.AxisLabels.Interval dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.AxisLabels.Interval dependency property.
        /// </returns>
        public static readonly DependencyProperty IntervalProperty = DependencyProperty.Register
            ("Interval",
            typeof(Nullable<Double>),
            typeof(AxisLabels),
            new PropertyMetadata(OnIntervalPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.AxisLabels.Angle dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.AxisLabels.Angle dependency property.
        /// </returns>
        public static readonly DependencyProperty AngleProperty = DependencyProperty.Register
            ("Angle",
            typeof(Nullable<Double>),
            typeof(AxisLabels),
            new PropertyMetadata(OnAnglePropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.AxisLabels.Enabled dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.AxisLabels.Enabled dependency property.
        /// </returns>
        public static readonly DependencyProperty EnabledProperty = DependencyProperty.Register
            ("Enabled",
            typeof(Nullable<Boolean>),
            typeof(AxisLabels),
            new PropertyMetadata(OnEnabledPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.AxisLabels.TextAlignment dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.AxisLabels.TextAlignment dependency property.
        /// </returns>
        public static readonly DependencyProperty TextAlignmentProperty = DependencyProperty.Register
            ("TextAlignment",
            typeof(TextAlignment),
            typeof(AxisLabels),
            new PropertyMetadata(TextAlignment.Left, OnTextAlignmentPropertyChanged));
        
#if WPF
        /// <summary>
        /// Identifies the Visifire.Charts.AxisLabels.FontFamily dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.AxisLabels.FontFamily dependency property.
        /// </returns>
        public new static readonly DependencyProperty FontFamilyProperty = DependencyProperty.Register
            ("FontFamily",
            typeof(FontFamily),
            typeof(AxisLabels),
            new PropertyMetadata(OnFontFamilyPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.AxisLabels.Opacity dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.AxisLabels.Opacity dependency property.
        /// </returns>
        public new static readonly DependencyProperty OpacityProperty = DependencyProperty.Register
            ("Opacity",
            typeof(Double),
            typeof(AxisLabels),
            new PropertyMetadata(1.0, OnOpacityPropertyChanged));
#endif

        /// <summary>
        /// Identifies the Visifire.Charts.AxisLabels.FontColor dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.AxisLabels.FontColor dependency property.
        /// </returns>
        public static readonly DependencyProperty FontColorProperty = DependencyProperty.Register
            ("FontColor",
            typeof(Brush),
            typeof(AxisLabels),
            new PropertyMetadata(OnFontColorPropertyChanged));
        
#if WPF

        /// <summary>
        /// Identifies the Visifire.Charts.AxisLabels.FontStyle dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.AxisLabels.FontStyle dependency property.
        /// </returns>
        public new static readonly DependencyProperty FontStyleProperty = DependencyProperty.Register
            ("FontStyle",
            typeof(FontStyle),
            typeof(AxisLabels),
            new PropertyMetadata(OnFontStylePropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.AxisLabels.FontWeight dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.AxisLabels.FontWeight dependency property.
        /// </returns>
        public new static readonly DependencyProperty FontWeightProperty = DependencyProperty.Register
            ("FontWeight",
            typeof(FontWeight),
            typeof(AxisLabels),
            new PropertyMetadata(OnFontWeightPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.AxisLabels.FontSize dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.AxisLabels.FontSize dependency property.
        /// </returns>
        public new static readonly DependencyProperty FontSizeProperty = DependencyProperty.Register
            ("FontSize",
            typeof(Double),
            typeof(AxisLabels),
            new PropertyMetadata(OnFontSizePropertyChanged));
#endif
        
        /// <summary>
        /// Identifies the Visifire.Charts.AxisLabels.TextWrap dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.AxisLabels.TextWrap dependency property.
        /// </returns>
        public static readonly DependencyProperty TextWrapProperty = DependencyProperty.Register
            ("TextWrap",
            typeof(Double),
            typeof(AxisLabels),
            new PropertyMetadata(Double.NaN, OnTextWrapPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.AxisLabels.Rows dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.AxisLabels.Rows dependency property.
        /// </returns>
        public static readonly DependencyProperty RowsProperty = DependencyProperty.Register
            ("Rows",
            typeof(Nullable<Int32>),
            typeof(AxisLabels),
            new PropertyMetadata(OnRowsPropertyChanged));

        /// <summary>
        /// ToolTipText property
        /// ( NotImplemented )
        /// </summary>
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public override String ToolTipText
        {
            get
            {
                throw new NotImplementedException("ToolTipText property for AxisLabels is not implemented");
            }
            set
            {
                throw new NotImplementedException("ToolTipText property for AxisLabels is not implemented");
            }
        }

        /// <summary>
        /// Get or set the axis labels interval
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
            }
        }

#if SL
       [System.ComponentModel.TypeConverter(typeof(Converters.NullableDoubleConverter))]
#endif
       public Nullable<Double> Angle
       {
           get
           {
               if ((Nullable<Double>)GetValue(AngleProperty) == null)
                   return InternalAngle;
               else
                   return (Nullable<Double>)GetValue(AngleProperty);
           }
           set
           {
               SetValue(AngleProperty, value);
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
                   FirePropertyChanged("Opacity");
               }
        #else
                SetValue(OpacityProperty, value);
        #endif
           }
        }

        /// <summary>
        /// Enables or disables axis labels
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
        /// Get or set the color for the text in axis labels 
        /// </summary>
        public TextAlignment TextAlignment
        {
            get
            {
                return (TextAlignment)GetValue(TextAlignmentProperty);
            }
            set
            {
                SetValue(TextAlignmentProperty, value);
            }
        }

        /// <summary>
        /// Get or set the font family of axis labels
        /// </summary>
        public new FontFamily FontFamily
        {
            get
            {
                if ((FontFamily)GetValue(FontFamilyProperty) == null)
                    return new FontFamily("Arial");
                else
                    return (FontFamily)GetValue(FontFamilyProperty);
            }
            set
            {

#if SL
                if (FontFamily != value)
                {   
                    InternalFontFamily = value;
                    SetValue(FontFamilyProperty, value);
                    FirePropertyChanged("FontFamily");
                }
#else           
                SetValue(FontFamilyProperty, value);
#endif
            }
        }

        /// <summary>
        /// Get or set the color for the text in axis labels
        /// </summary>
        public Brush FontColor
        {
            get
            {
                return (Brush)GetValue(FontColorProperty);
            }
            set
            {
                SetValue(FontColorProperty, value);
            }
        }

        private static void OnFontColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AxisLabels axisLabels = d as AxisLabels;
            axisLabels.UpdateVisual("FontColor", e.NewValue);
        }

        /// <summary>
        /// Get or set the styles for the text like "Italic" or "Normal"
        /// </summary>
#if WPF
         [TypeConverter(typeof(System.Windows.FontStyleConverter))]
#endif
        public new FontStyle FontStyle
        {   
            get
            {   
                return (FontStyle)(GetValue(FontStyleProperty));
            }
            set
            {
#if SL          
                if (InternalFontStyle != value)
                {
                    InternalFontStyle = value;
                    SetValue(FontStyleProperty, value);
                    UpdateVisual("FontStyle", value);
                }
#else
                 SetValue(FontStyleProperty, value);
#endif
            }
        }
        
        /// <summary>
        /// Get or set how the font appears. It takes values like "Bold", "Normal", "Black" etc
        /// </summary>
#if WPF
        [System.ComponentModel.TypeConverter(typeof(System.Windows.FontWeightConverter))]
#endif
        public new FontWeight FontWeight
        {
            get
            {
                return (FontWeight)(GetValue(FontWeightProperty));
            }
            set
            {

#if SL          
                if (FontWeight != value)
                {   
                    InternalFontWeight = value;
                    SetValue(FontWeightProperty, value);
                    UpdateVisual("FontWeight", value);
                }
#else
                SetValue(FontWeightProperty, value);
#endif

            }
        }

        public new Double FontSize
        {
            get
            {   
                return (Double)GetValue(FontSizeProperty);
            }
            set
            {
#if SL          
                if (FontSize != value)
                {
                    InternalFontSize = value;
                    SetValue(FontSizeProperty, value);
                    FirePropertyChanged("FontSize");
                }
#else
                SetValue(FontSizeProperty, value);
#endif
            }
        }

        /// <summary>
        /// Get or set the number of rows of the axis labels
        /// </summary>
#if SL
        [System.ComponentModel.TypeConverter(typeof(Converters.NullableInt32Converter))]
#endif
        public Nullable<Int32> Rows
        {
            get
            {
                return ((Nullable<Int32>)GetValue(RowsProperty) == null) ? 0 : ((Nullable<Int32>)GetValue(RowsProperty));
            }
            set
            {
                InternalRows = (Int32)((value == null)? 0: value);
                SetValue(RowsProperty, value);
            }
        }
             
        /// <summary>
        /// Get or set the parent as Axis
        /// </summary>
        public new Axis Parent
        {
            get
            {
                return _parent;
            }
            internal set
            {
                System.Diagnostics.Debug.Assert(typeof(Axis).Equals(value.GetType()), "Unknown Parent", "DataPoint should have DataSeries as Parent");
                _parent = value;
            }
        }


        #endregion

        #region Public Events

        #endregion

        #region Protected Methods

        #endregion

        #region Internal Properties

#if SL

        /// <summary>
        /// Identifies the Visifire.Charts.AxisLabels.FontSize dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.AxisLabels.FontSize dependency property.
        /// </returns>
        private static readonly DependencyProperty
            InternalFontSizeProperty = DependencyProperty.Register
            ("InternalFontSize",
            typeof(Double),
            typeof(AxisLabels),
            new PropertyMetadata(OnFontSizePropertyChanged));
        
        /// Identifies the Visifire.Charts.AxisLabels.FontFamily dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.AxisLabels.FontFamily dependency property.
        /// </returns>
        private static readonly DependencyProperty InternalFontFamilyProperty = DependencyProperty.Register
            ("InternalFontFamily",
            typeof(FontFamily),
            typeof(AxisLabels),
            new PropertyMetadata(OnFontFamilyPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.AxisLabels.FontStyle dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.AxisLabels.FontStyle dependency property.
        /// </returns>
        private static readonly DependencyProperty InternalFontStyleProperty = DependencyProperty.Register
            ("InternalFontStyle",
            typeof(FontStyle),
            typeof(AxisLabels),
            new PropertyMetadata(OnFontStylePropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.AxisLabels.FontWeight dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.AxisLabels.FontWeight dependency property.
        /// </returns>
        private static readonly DependencyProperty InternalFontWeightProperty = DependencyProperty.Register
            ("InternalFontWeight",
            typeof(FontWeight),
            typeof(AxisLabels),
            new PropertyMetadata(OnFontWeightPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.AxisLabels.Opacity dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.AxisLabels.Opacity dependency property.
        /// </returns>
        private static readonly DependencyProperty InternalOpacityProperty = DependencyProperty.Register
            ("InternalOpacity",
            typeof(Double),
            typeof(AxisLabels),
            new PropertyMetadata(1.0, OnOpacityPropertyChanged));

#endif
        /// <summary>
        /// Get or set the FontFamily property of title
        /// </summary>
        internal FontFamily InternalFontFamily
        {
            get
            {
                FontFamily retVal;

                if (_internalFontFamily == null)
                    retVal = (FontFamily)GetValue(FontFamilyProperty);
                else
                    retVal = _internalFontFamily;

                return (retVal == null) ? new FontFamily("Verdana") : retVal;
            }
            set
            {

                _internalFontFamily = value;
            }
        }

        /// <summary>
        /// Get or set the FontSize property of title
        /// </summary>
        internal Double InternalFontSize
        {
            get
            {
                return (Double)(Double.IsNaN(_internalFontSize) ? GetValue(FontSizeProperty) : _internalFontSize);
            }
            set
            {
                _internalFontSize = value;
            }
        }

        /// <summary>
        /// Get or set the FontStyle property of title text
        /// </summary>
#if WPF
        [TypeConverter(typeof(System.Windows.FontStyleConverter))]
#endif
        internal FontStyle InternalFontStyle
        {
            get
            {
                return (FontStyle)((_internalFontStyle == null) ? GetValue(FontStyleProperty) : _internalFontStyle);
            }
            set
            {
                _internalFontStyle = value;
            }
        }

        /// <summary>
        /// Get or set the FontWeight property of title text
        /// </summary>
#if WPF
         [System.ComponentModel.TypeConverter(typeof(System.Windows.FontWeightConverter))]
#endif
        internal FontWeight InternalFontWeight
        {
            get
            {
                return (FontWeight)((_internalFontWeight == null) ? GetValue(FontWeightProperty) : _internalFontWeight);
            }
            set
            {
                _internalFontWeight = value;
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
        /// Average width of a character after applying 
        /// </summary>
        internal Double WidthOfACharacter
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set the axis labels angle
        /// </summary>
        internal Nullable<Double> InternalAngle
        {
            get;
            set;
        }

        /// <summary>
        /// Visual element for axis labels
        /// </summary>
        internal Canvas Visual
        {
            get;
            private set;
        }

        /// <summary>
        /// Get or set the maximum width of the labels relative to chart size. Value range is 0 - 1.
        /// </summary>
        public Double TextWrap
        {
            get
            {
                return (Double)GetValue(TextWrapProperty);
            }
            set
            {   
                SetValue(TextWrapProperty, value);
            }
        }

        /// <summary>
        /// Get or set the number of rows of the axis labels
        /// </summary>
        internal Int32 InternalRows
        {
            get;
            set;
        }

        /// <summary>
        /// Actual minimum value of the axis
        /// </summary>
        internal Double Minimum
        {
            get;
            set;
        }
        
        /// <summary>
        /// Actual maximum value of the axis
        /// </summary>
        internal Double Maximum
        {
            get;
            set;
        }

        /// <summary>
        /// Visual minimum for the axis
        /// </summary>
        internal Double DataMinimum
        {
            get;
            set;
        }

        /// <summary>
        /// Visual maximum for the axis
        /// </summary>
        internal Double DataMaximum
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set the width of the axis labels canvas, will be used only with the Horizontal axis
        /// </summary>
        internal new Double Width
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set the height of the axis labels canvas, will be used ony with the vertical axis 
        /// </summary>
        internal new Double Height
        {
            get;
            set;
        }

        /// <summary>
        /// Placement decides how the labels have to be positioned 
        /// </summary>
        internal PlacementTypes Placement
        {
            get;
            set;
        }

        /// <summary>
        /// Reference to the dictionary in the PlotDetails that contains the required labels
        /// </summary>
        internal Dictionary<Double, String> AxisLabelContentDictionary
        {
            get;
            set;
        }

        /// <summary>
        /// Flag indicating whether all unique XValues have labels or not
        /// </summary>
        internal Boolean AllAxisLabels
        {
            get;
            set;
        }

        /// <summary>
        /// Reference to the axis which holds this axis labels
        /// </summary>
        internal Axis ParentAxis
        {
            get;
            set;
        }

        /// <summary>
        /// Number of pixels by which the top of the axis labels has overshot the actual canvas top
        /// </summary>
        internal Double TopOverflow
        {
            get;
            private set;
        }

        /// <summary>
        /// Number of pixels by which the bottom of the axislabels has overshot the actual canvas bottom
        /// </summary>
        internal Double BottomOverflow
        {
            get;
            private set;
        }

        /// <summary>
        /// Number of pixels by which the left of the axislabels has overshot the actual canvas left
        /// </summary>
        internal Double LeftOverflow
        {
            get;
            private set;
        }

        /// <summary>
        /// Number of pixels by which the right of the axislabels has overshot the actual canvas right
        /// </summary>
        internal Double RightOverflow
        {
            get;
            private set;
        }

        /// <summary>
        /// List of axis labels
        /// </summary>
        internal List<AxisLabel> AxisLabelList
        {
            get;
            set;
        }

        /// <summary>
        /// List of position values for the labels
        /// </summary>
        internal List<Double> LabelValues
        {
            get;
            set;
        }

        #endregion

        #region Private Delegates

        #endregion

        #region Private Methods

        /// <summary>
        /// Event handler attached with Interval property changed event of axislabels element
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnIntervalPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AxisLabels axisLabels = d as AxisLabels;
            axisLabels.FirePropertyChanged("Interval");
        }

        /// <summary>
        /// Event handler attached with Angle property changed event of axislabels element
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnAnglePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AxisLabels axisLabels = d as AxisLabels;
            axisLabels.InternalAngle = (Nullable<Double>)e.NewValue;
            axisLabels.FirePropertyChanged("Angle");
        }
        
        /// <summary>
        /// Event handler attached with Enabled property changed event of axislabels element
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnEnabledPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AxisLabels axisLabels = d as AxisLabels;
            axisLabels.FirePropertyChanged("Enabled");
        }

        /// <summary>
        /// Event handler attached with TextAlignment property changed event of axislabels element
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnTextAlignmentPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AxisLabels axisLabels = d as AxisLabels;
            axisLabels.FirePropertyChanged("TextAlignment");
        }



        /// <summary>
        /// Event handler attached with FontFamily property changed event of axislabels element
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnFontFamilyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AxisLabels axisLabels = d as AxisLabels;

            if (e.NewValue == null || e.OldValue == null)
            {
                axisLabels.InternalFontFamily = (FontFamily)e.NewValue;
                axisLabels.FirePropertyChanged("FontFamily");
            }
            else if (e.NewValue.ToString() != e.OldValue.ToString())
            {
                axisLabels.InternalFontFamily = (FontFamily)e.NewValue;
                axisLabels.FirePropertyChanged("FontFamily");
            }
        }

        /// <summary>
        /// Event handler attached with FontStyle property changed event of axislabels element
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnFontStylePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AxisLabels axisLabels = d as AxisLabels;
            axisLabels.InternalFontStyle = (FontStyle)e.NewValue;
            axisLabels.UpdateVisual("FontStyle", e.NewValue);
        }

        /// <summary>
        /// Event handler attached with FontWeight property changed event of axislabels element
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnFontWeightPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AxisLabels axisLabels = d as AxisLabels;
            axisLabels.InternalFontWeight = (FontWeight)e.NewValue;
            axisLabels.UpdateVisual("FontWeight", e.NewValue);
        }
        
        /// <summary>
        /// Event handler attached with FontSize property changed event of axislabels element
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnFontSizePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AxisLabels axisLabels = d as AxisLabels;
            axisLabels.InternalFontSize = (Double)e.NewValue;
            axisLabels.FirePropertyChanged("FontSize");
        }

        /// <summary>
        /// OpacityProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnOpacityPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AxisLabels axisLabels = d as AxisLabels;
            axisLabels.InternalOpacity = (Double)e.NewValue;
            axisLabels.FirePropertyChanged("Opacity");
        }


        /// <summary>
        /// Event handler attached with TextWrap property changed event of axislabels element
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnTextWrapPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AxisLabels axisLabels = d as AxisLabels;

            if((Double)e.NewValue < 0 || (Double)e.NewValue > 1)
                throw new Exception("Wrong property value. Range of TextWrapProperty varies from 0 to 1.");

            axisLabels.FirePropertyChanged("TextWrap");
        }

        /// <summary>
        /// Event handler attached with Rows property changed event of axislabels element
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnRowsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AxisLabels axisLabels = d as AxisLabels;
            axisLabels.FirePropertyChanged("Rows");
        }
        
        /// <summary>
        /// Create a instance of a Visifire.Charts.AxisLabel
        /// </summary>
        /// <param name="text">Text as string</param>
        /// <returns>AxisLabel</returns>
        private AxisLabel CreateLabel(String text)
        {
            AxisLabel label = new AxisLabel();
            label.Text = text;
            label.Placement = this.Placement;
            return label;
        }

        /// <summary>
        /// Calculate auto interval
        /// </summary>
        /// <param name="CurrentInterval">Current interval</param>
        /// <param name="AxisWidth">Width of the axis</param>
        /// <param name="NoOfLabels">Number of labels</param>
        /// <param name="Angle">Angle of labels</param>
        /// <param name="Rows">Number of rows</param>
        /// <returns>Double</returns>
        private Double CalculateAutoInterval(Double CurrentInterval, Double AxisWidth, Int32 NoOfLabels, Double Angle, Int32 Rows)
        {
            Double retVal = 1;
            Angle = Double.IsNaN(Angle)? 0 :Angle;
            CalculateHorizontalOverflow();

            return retVal;
        }

        /// <summary>
        /// Creates a set of labels
        /// </summary>
        private Boolean CreateLabels()
        {
            // Calculate interval
            Double interval = (Double)Interval;

            if (Double.IsNaN(interval) || interval <= 0)
                interval = ParentAxis.InternalInterval;

            // Set the begining of the axis labels same as that of the Axis Minimum.
            Decimal index = (Decimal)Minimum;

            // initialize the loop begin and end parameters
            Decimal minval = (Decimal)Minimum;
            Decimal maxVal = (Decimal)Maximum;

            // initialize the loop index increment values
            Decimal gap = (Decimal)interval;
            Int32 count = 0;

            // if the axis labels belong to axis x
            if (ParentAxis.AxisRepresentation == AxisRepresentations.AxisX)
            {   
                // if the data minimum - interval is less than the actual minimum

                if (Double.IsNaN((Double)Parent.AxisMinimumNumeric))
                {
                    if (ParentAxis.XValueType != ChartValueTypes.Numeric)
                    {
                        index = (Decimal)ParentAxis.FirstLabelPosition;
                    }
                    else
                    {
                        if ((DataMinimum - Minimum) / interval >= 1)
                            index = (Decimal)(DataMinimum - Math.Floor((DataMinimum - Minimum) / interval) * interval);
                        else
                            index = (Decimal)DataMinimum;
                    }
                }

                //if (ParentAxis.SkipOfset > 0)
                //{
                //    if ((Double)Parent.AxisMinimum < DataMinimum)
                //        index = (Decimal)DataMinimum;
                //}
                
                if (AllAxisLabels && AxisLabelContentDictionary.Count > 0)
                {
                    Dictionary<Double, String>.Enumerator enumerator = AxisLabelContentDictionary.GetEnumerator();
                    enumerator.MoveNext();

                    Int32 dictionaryIndex = 0;
                    //index = (Decimal)enumerator.Current.Key;

                    for (; dictionaryIndex < AxisLabelContentDictionary.Count - 1; dictionaryIndex++)
                    {
                        enumerator.MoveNext();
                        //index = Math.Min(index, (Decimal)enumerator.Current.Key);
                    }

                    enumerator.Dispose();
                }

                minval = index;

                if (minval != maxVal)
                {
                    if (!Double.IsNaN(TextWrap))
                        CalculatAvgWidthOfAChar();

                    for (; index <= maxVal;)
                    {
                        // if (!((AllAxisLabels) && (AxisLabelContentDictionary.Count > 0) && (index > (Decimal)DataMaximum)))
                        {   
                            String labelContent = "";

                            if (AxisLabelContentDictionary.ContainsKey((Double)index))
                            {
                                if (ParentAxis.AxisOrientation == Orientation.Vertical)
                                    labelContent = AutoFormatMultilineText(AxisLabelContentDictionary[(Double)index], false);
                                else
                                    labelContent = AutoFormatMultilineText(AxisLabelContentDictionary[(Double)index], true);
                            }
                            else
                            {
                                if (ParentAxis.XValueType == ChartValueTypes.Date)
                                {
                                    DateTime dt = ParentAxis.MinDate;
                                    Decimal tempIndex = index;

                                    if (ParentAxis._isAllXValueZero)
                                        tempIndex--;

                                    dt = DateTimeHelper.UpdateDate(Parent.FirstLabelDate, (Double)interval * count, ParentAxis.InternalIntervalType);
                                    //dt = DateTimeHelper.AlignDateTime(dt, ParentAxis.InternalInterval, ParentAxis.InternalIntervalType);

                                    //if (ParentAxis.InternalIntervalType == IntervalTypes.Years)
                                    //    dt = new DateTime(dt.Year, 1, 1, 0, 0, 0);

                                    labelContent = FormatDate(dt, ParentAxis);
                                }
                                else if (ParentAxis.XValueType == ChartValueTypes.Time)
                                {
                                    DateTime dt = ParentAxis.MinDate;
                                    Decimal tempIndex = index;

                                    System.Diagnostics.Debug.WriteLine("Index=" + index.ToString());
                                    if (ParentAxis._isAllXValueZero)
                                        tempIndex--;

                                    dt = DateTimeHelper.UpdateDate(Parent.FirstLabelDate, (Double)interval * count, ParentAxis.InternalIntervalType);
                                    labelContent = FormatDate(dt, ParentAxis);
                                }
                                else if (ParentAxis.XValueType == ChartValueTypes.DateTime)
                                {
                                    DateTime dt = ParentAxis.MinDate;
                                    Decimal tempIndex = index;

                                    if (ParentAxis._isAllXValueZero)
                                        tempIndex--;

                                    dt = DateTimeHelper.UpdateDate(Parent.FirstLabelDate, (Double)interval * count, ParentAxis.InternalIntervalType);

                                    if (ParentAxis.IntervalType == IntervalTypes.Years || ParentAxis.InternalIntervalType == IntervalTypes.Years
                                        || ParentAxis.IntervalType == IntervalTypes.Months || ParentAxis.InternalIntervalType == IntervalTypes.Months
                                        || ParentAxis.IntervalType == IntervalTypes.Weeks || ParentAxis.InternalIntervalType == IntervalTypes.Weeks
                                        || ParentAxis.IntervalType == IntervalTypes.Days || ParentAxis.InternalIntervalType == IntervalTypes.Days
                                        )
                                        dt = new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0, 0);

                                    labelContent = FormatDate(dt, ParentAxis);
                                }
                                else
                                    labelContent = GetFormattedString((Double)index);
                            }

                            AxisLabel label = CreateLabel(labelContent);

                            AxisLabelList.Add(label);
                            LabelValues.Add((Double)index);


                        }

                        if (ParentAxis.IsDateTimeAxis)
                        {
                            count++;

                            DateTime dt = DateTimeHelper.UpdateDate(Parent.FirstLabelDate, (Double)(count * gap), ParentAxis.InternalIntervalType);
                            Decimal oneUnit = (Decimal)DateTimeHelper.DateDiff(dt, Parent.FirstLabelDate, ParentAxis.MinDateRange, ParentAxis.MaxDateRange, ParentAxis.InternalIntervalType, ParentAxis.XValueType);

                            index = minval + oneUnit;
                        }
                        else
                        {   
                            index = minval + (++count) * gap;
                        }
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (minval != maxVal)
                {
                    AxisLabel label;

                    // Create and save the first label
                    label = CreateLabel(GetFormattedString(Minimum));
                    AxisLabelList.Add(label);
                    LabelValues.Add(Minimum);

                    // Shift the maximum inwards so that the last label is not created
                    // the reason for this is some times due to double approximation the last label doesnt get created
                    // thats why the last label is explicitly created and hence must not be created in the following loop
                    maxVal = (Decimal)(Maximum - interval / 2);

                    //Create and save intermediate labels
                    for (index = minval + (++count) * gap; index <= maxVal; index = minval + (++count) * gap)
                    {
                        label = CreateLabel(GetFormattedString((Double)index));
                        AxisLabelList.Add(label);
                        LabelValues.Add((Double)index);
                    }

                    //create and save the last label
                    label = CreateLabel(GetFormattedString(Maximum));
                    AxisLabelList.Add(label);
                    LabelValues.Add(Maximum);
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Calculate average width of a character
        /// </summary>
        /// <returns></returns>
        private void CalculatAvgWidthOfAChar()
        {
            AxisLabel label = new AxisLabel();
            label.Text ="ABCDabcd01";
            ApplyAxisLabelFontProperties(label);
            label.CreateVisualObject(false);
            WidthOfACharacter = label.ActualTextWidth / 10;
        }
        
        /// <summary>
        /// Set DateTime in AxisXLabel
        /// </summary>
        /// <param name="dataPoint">DataPoint</param>
        /// <param name="axis">Axis</param>
        /// <param name="label">Axis labels</param>
        private String FormatDate(DateTime dt, Axis axis)
        {
            String valueFormatString = axis.XValueType == ChartValueTypes.Date ? "M/d/yyyy" : axis.XValueType == ChartValueTypes.Time ? "h:mm:ss tt" : "M/d/yyyy h:mm:ss tt";
            valueFormatString = (String.IsNullOrEmpty((String)axis.GetValue(Axis.ValueFormatStringProperty))) ? valueFormatString : axis.ValueFormatString;

            return axis.AddPrefixAndSuffix(dt.ToString(valueFormatString, System.Globalization.CultureInfo.CurrentCulture));
        }



        /// <summary>
        /// Auto formats the AxisLabel text if bigger for Vertical charts
        /// </summary>
        /// <param name="text">Text as String</param>
        /// <returns>Formatted text as String</returns>
        private String AutoFormatMultilineText(String text, Boolean autoIncrementWrapAt)
        {
            String multiLineText = "";
            
            if (!Double.IsNaN(TextWrap))
            {   
                AxisLabel label = CreateLabel(text);
                ApplyAxisLabelFontProperties(label);
                label.CreateVisualObject(false);

                Double MaxLabelWidth = (ParentAxis.PlotDetails.ChartOrientation == ChartOrientationType.Vertical) ? Chart.ActualHeight : Chart.ActualWidth;
                MaxLabelWidth *= TextWrap;

                Int32 wrapAt = (Int32)(MaxLabelWidth / WidthOfACharacter);

                if ((ParentAxis.PlotDetails.ChartOrientation == ChartOrientationType.Vertical && label.ActualHeight > MaxLabelWidth)
                    || (label.ActualWidth > MaxLabelWidth))

                {   
                    Int32 charCount = 0;
                    foreach (Char c in text)
                    {
                        if (c != ' ')
                        {
                            charCount++;
                            multiLineText += c;
                        }
                        else if (charCount >= wrapAt)
                        {
                            multiLineText += "\n";
                            charCount = 0;

                            if (autoIncrementWrapAt)
                                wrapAt += 2;
                        }
                        else
                            multiLineText += c;
                    }
                }
                else
                    multiLineText = text;
            }
            else
                multiLineText = text;

            multiLineText = GetFormattedMultilineText(multiLineText);

            return multiLineText;
        }

        /// <summary>
        /// Sets the position of labels based on the placement type
        /// </summary>
        private void SetLabelPosition()
        {
            switch (Placement)
            {
                case PlacementTypes.Top:
                    PositionLabelsTop();
                    break;
                case PlacementTypes.Left:
                    PositionLabelsLeft();
                    break;
                case PlacementTypes.Right:
                    PositionLabelsRight();
                    break;
                case PlacementTypes.Bottom:
                    PositionLabelsBottom();
                    break;
            }
        }

        /// <summary>
        /// Labels will be positioned for the axis that will appear above the plot area
        /// </summary>
        private void PositionLabelsTop()
        {
            Double startOffset = Double.IsNaN(ParentAxis.StartOffset) ? 0 : ParentAxis.StartOffset;
            Double endOffset = Double.IsNaN(ParentAxis.EndOffset) ? 0 : ParentAxis.EndOffset;

            // Check if the width is valid or not
            if (Double.IsNaN(Width) || Width <= 0)
                return;

            // set the width of the canvas
            Visual.Width = Width;

            // Variable to calculate the height of the visual canvas
            Double height = 0;

            // Calculate Default placement values
            CalculateHorizontalDefaults();

            // Calculate the height of the labels
            for (Int32 i = 0; i < AxisLabelList.Count; i++)
            {
                AxisLabel label = AxisLabelList[i];

                //Set size affecting font properties
                ApplyAxisLabelFontProperties(label);

                // set the label position
                label.Position = new Point(0, 0);

                // create the label visual element
                label.CreateVisualObject(false);

                // get the max height of the labels
                height = Math.Max(Math.Max(height, label.ActualHeight), _maxRowHeight);

            }

            for (Int32 i = 0; i < AxisLabelList.Count; i++)
            {
                AxisLabel label = AxisLabelList[i];

                // get the position of the label
                Double position = Graphics.ValueToPixelPosition(startOffset, Width - endOffset, Minimum, Maximum, LabelValues[i]);

                // Create the visual element again
                label.CreateVisualObject(false);

                //Calculate vertical Position
                Double top = 0;
                if (GetAngle() != 0)
                {
                    top = Math.Abs((label.ActualTextHeight / 2) * Math.Sin(Math.PI / 2 - AxisLabel.GetRadians(GetAngle())));
                }

                // Set the new position
                label.Position = new Point(position, height * (Int32)InternalRows - top - ((i % (Int32)InternalRows) * _maxRowHeight) + Padding.Top);

                // Create the visual element again
                label.CreateVisualObject(true);

                // add the element to the visual canvas
                Visual.Children.Add(label.Visual);

            }

            // set the height of the visual canvas
            Visual.Height = height * (Int32)InternalRows + Padding.Top;

            // calculate the overflow due to this set of axis labels
            CalculateHorizontalOverflow();
        }

        /// <summary>
        /// Labels will be positioned for the axis that will appear to the left of the plot area
        /// </summary>
        private void PositionLabelsLeft()
        {
            Double startOffset = Double.IsNaN(ParentAxis.StartOffset) ? 0 : ParentAxis.StartOffset;
            Double endOffset = Double.IsNaN(ParentAxis.EndOffset) ? 0 : ParentAxis.EndOffset;

            // Check if the height is valid or not
            if (Double.IsNaN(Height) || Height <= 0)
                return;

            // set the height of the canvas
            Visual.Height = Height;

            // Variable to calculate the height of the visual canvas
            Double width = 0;

            //Calculate Defaults for the vertical axis
            CalculateVerticalDefaults();

            // Calculate the width of the labels
            for (Int32 i = 0; i < AxisLabelList.Count; i++)
            {
                AxisLabel label = AxisLabelList[i];

                // set size affecting pararameters
                ApplyAxisLabelFontProperties(label);

                // set the label position
                label.Position = new Point(0, 0);

                // create the label visual element
                label.CreateVisualObject(false); 

                // get the max width of the labels
                width = Math.Max(width, label.ActualWidth);
            }

            for (Int32 i = 0; i < AxisLabelList.Count; i++)
            {
                AxisLabel label = AxisLabelList[i];

                // get the position of the label
                Double position = Graphics.ValueToPixelPosition(Height - endOffset, startOffset, Minimum, Maximum, LabelValues[i]);

                // Create the visual element again
                label.CreateVisualObject(false);

                //Calculate horizontal Position
                Double left = 1;
                if (GetAngle() != 0)
                {
                    left = Math.Abs((label.ActualTextHeight / 2) * Math.Cos(Math.PI / 2 - AxisLabel.GetRadians(GetAngle())));
                }

                // Set the new position
                label.Position = new Point(width - left + Padding.Left, position);

                // Create the visual element again
                label.CreateVisualObject(true);

                // add the element to the visual canvas
                Visual.Children.Add(label.Visual);

            }

            // set the width of the visual canvas
            Visual.Width = width + Padding.Left;

            // calculate the overflow due to this set of axis labels
            CalculateVerticalOverflow();
        }

        /// <summary>
        /// Labels will be positioned for the axis that will appear to the right of the plot area
        /// </summary>
        private void PositionLabelsRight()
        {
            Double startOffset = Double.IsNaN(ParentAxis.StartOffset) ? 0 : ParentAxis.StartOffset;
            Double endOffset = Double.IsNaN(ParentAxis.EndOffset) ? 0 : ParentAxis.EndOffset;

            // Check if the height is valid or not
            if (Double.IsNaN(Height) || Height <= 0)
                return;

            // set the height of the canvas
            Visual.Height = Height;

            // Variable to calculate the height of the visual canvas
            Double width = 0;

            //Calculate Defaults for the vertical axis
            CalculateVerticalDefaults();

            // Calculate the width of the labels
            for (Int32 i = 0; i < AxisLabelList.Count; i++)
            {
                AxisLabel label = AxisLabelList[i];

                //Set size affecting font properties
                ApplyAxisLabelFontProperties(label);

                // get the position of the label
                Double position = Graphics.ValueToPixelPosition(Height - endOffset, startOffset, Minimum, Maximum, LabelValues[i]);

                // Create the visual element again
                label.CreateVisualObject(false);

                //Calculate horizontal Position
                Double left = 1;
                if (GetAngle() != 0)
                {
                    left = Math.Abs((label.ActualTextHeight / 2) * Math.Cos(Math.PI / 2 - AxisLabel.GetRadians(GetAngle())));
                }

                // Set the new position
                label.Position = new Point(left, position);

                // Create the visual element again
                label.CreateVisualObject(true);

                // add the element to the visual canvas
                Visual.Children.Add(label.Visual);

                // get the max width of the labels
                width = Math.Max(width, label.ActualWidth);
            }

            // set the width of the visual canvas
            Visual.Width = width + Padding.Right;

            // calculate the overflow due to this set of axis labels
            CalculateVerticalOverflow();
        }

        /// <summary>
        /// Apply font properties of axislabels
        /// </summary>
        /// <param name="label">AxisLabel</param>
        private void ApplyAxisLabelFontProperties(AxisLabel label)
        {
            //Set size affecting font properties
            label.FontSize = InternalFontSize;
            label.FontColor = Charts.Chart.CalculateFontColor((Chart as Chart), FontColor, false);
            label.FontFamily = InternalFontFamily;
            label.FontStyle = InternalFontStyle;
            label.FontWeight = InternalFontWeight;
            label.TextAlignment = TextAlignment;
            label.Angle = GetAngle();
        }

        /// <summary>
        /// Labels will be positioned for the axis that will appear to the bottom of the plot area
        /// </summary>
        private void PositionLabelsBottom()
        {
            Double startOffset = Double.IsNaN(ParentAxis.StartOffset) ? 0 : ParentAxis.StartOffset;
            Double endOffset = Double.IsNaN(ParentAxis.EndOffset) ? 0 : ParentAxis.EndOffset;

            // Check if the width is valid or not
            if (Double.IsNaN(Width) || Width <= 0)
                return;

            // set the width of the canvas
            Visual.Width = Width;

            // Variable to calculate the height of the visual canvas
            Double height = 0;

            // Calculate Default placement values
            CalculateHorizontalDefaults();

            // Calculate the height of the labels and position them
            for (Int32 i = 0; i < AxisLabelList.Count; i += (ParentAxis.SkipOffset + 1))
            {
                AxisLabel label = AxisLabelList[i];

                //Set size affecting font properties
                ApplyAxisLabelFontProperties(label);

                // get the position of the label
                Double position = Graphics.ValueToPixelPosition(startOffset, Width - endOffset, Minimum, Maximum, LabelValues[i]);

                //Calculate vertical Position
                Double top = 0;

                // Create the visual element again
                label.CreateVisualObject(false);

                if (GetAngle() != 0)
                {
                    top = Math.Abs((label.ActualTextHeight / 2) * Math.Sin(Math.PI / 2 - AxisLabel.GetRadians(GetAngle())));
                }

                // Set the new position
                label.Position = new Point(position, top + ((i % (Int32)InternalRows) * _maxRowHeight));

                // Create the visual element again
                label.CreateVisualObject(true);

                // add the element to the visual canvas
                Visual.Children.Add(label.Visual);

                // get the max height of the labels
                height = Math.Max(Math.Max(height, label.ActualHeight), _maxRowHeight);
            }

            // set the height of the visual canvas
            Visual.Height = height * (Int32)InternalRows + Padding.Bottom;

            // calculate the overflow due to this set of axis labels
            CalculateHorizontalOverflow();
        }

        /// <summary>
        /// This is for axis with placement setting as top or bottom
        /// </summary>
        private void CalculateHorizontalOverflow()
        {   
            // Check if the label list contains any labels or not (if not then set the overflow to 0)
            if (AxisLabelList.Count > 0)
            {
                LeftOverflow = (from axisLabel in AxisLabelList select axisLabel.ActualLeft).Min();
                RightOverflow = (from axisLabel in AxisLabelList select (axisLabel.ActualLeft + axisLabel.ActualWidth)).Max() - Width;
            }
            else
            {
                LeftOverflow = 0;
                RightOverflow = 0;
            }

            // if over flow is negative only then an actual overflow has ocured
            if ((Boolean)ParentAxis.Enabled)
                LeftOverflow = LeftOverflow > 0 ? 0 : Math.Abs(LeftOverflow);
            else
                LeftOverflow = 0;

            // if over flow is positive only then an actual overflow has ocured
            RightOverflow = RightOverflow < 0 ? 0 : RightOverflow;

            // For top or bottom these will remain zero
            TopOverflow = 0;
            BottomOverflow = 0;
        }

        /// <summary>
        /// This is for axis with placement setting as left or right
        /// </summary>
        private void CalculateVerticalOverflow()
        {
            // Check if the label list contains any labels or not (if not then set the overflow to 0)
            if (AxisLabelList.Count > 0)
            {
                TopOverflow = (from axisLabel in AxisLabelList select axisLabel.ActualTop).Min();
                BottomOverflow = ((from axisLabel in AxisLabelList select (axisLabel.ActualTop + axisLabel.ActualHeight)).Max()) - Height;
            }
            else
            {   
                TopOverflow = 0;
                BottomOverflow = 0;
            }
            
            // if over flow is negative only then an actual overflow has ocured
            TopOverflow = TopOverflow > 0 ? 0 : Math.Abs(TopOverflow);

            // if over flow is positive only then an actual overflow has ocured
            BottomOverflow = BottomOverflow < 0 ? 0 : BottomOverflow;

            // For left or right these will remain zero
            LeftOverflow = 0;
            RightOverflow = 0;
        }

        /// <summary>
        /// Returns the proper angle value for calculation
        /// </summary>
        private Double GetAngle()
        {
            return Double.IsNaN((Double)this.InternalAngle) ? 0 : (Double)this.InternalAngle;
        }

        /// <summary>
        /// Calculates default font size based on a scaling criteria
        /// </summary>
        /// <param name="area">Double</param>
        /// <returns>FontSize as Double</returns>
        private Double CalculateFontSize(Double area)
        {
            if (Double.IsNaN(InternalFontSize) || InternalFontSize <= 0)
            {
                return Graphics.DefaultFontSizes[1];
            }
            else
                return InternalFontSize;
        }
                
        /// <summary>
        /// Calculate auto font size
        /// </summary>
        /// <param name="initialFontSize">Double</param>
        /// <param name="width">Double</param>
        /// <returns>FontSize as Double</returns>
        private Double AutoAdjustFontSize(Double initialFontSize, Double width)
        {
            Double minimumFontSize = 8;
            Double fontSize = initialFontSize;
            TextBlock textBlock = new TextBlock();

            Double labelsWidth = 0;
            Size textBlockSize;
            for (; fontSize > minimumFontSize; fontSize -= 2)
            {
                textBlock.FontSize = fontSize;
                labelsWidth = 0;

                foreach (AxisLabel label in AxisLabelList)
                {
                    textBlock.Text = " " + label.Text + " ";
                    textBlockSize = Graphics.CalculateTextBlockSize(AxisLabel.GetRadians(GetAngle()), textBlock);
                    labelsWidth += textBlockSize.Width;
                }

                if (labelsWidth <= width)
                    break;
            }

            return fontSize;
        }

        private Int32 CalculateRows()
        {
            TextBlock textBlock = new TextBlock();

            textBlock = SetFontProperties(textBlock);

            //Calculate interval
            Double interval = (Double)((Double.IsNaN((Double)Interval) && Interval <= 0) ? Interval : ParentAxis.InternalInterval);

            Double pixelInterval = Graphics.ValueToPixelPosition(0, Width, Minimum, Maximum, interval + Minimum);
            List<Double> labelWidths = new List<Double>();
            Double maxRowHeight = 0;
            Size textBlockSize;

            foreach (AxisLabel label in AxisLabelList)
            {
                textBlock.Text = " " + label.Text + " ";
                textBlockSize = Graphics.CalculateTextBlockSize(AxisLabel.GetRadians(GetAngle()), textBlock);

                if (!Double.IsNaN((Double)this.InternalAngle))
                {
#if WPF
                    textBlockSize.Width = (Math.Cos(AxisLabel.GetRadians(GetAngle())) * textBlock.DesiredSize.Height) + textBlock.DesiredSize.Height / 2;
#else
                    textBlockSize.Width = (Math.Cos(AxisLabel.GetRadians(GetAngle())) * textBlock.ActualHeight) + textBlock.ActualHeight / 2;
#endif
                }
                maxRowHeight = Math.Max(maxRowHeight, textBlockSize.Height);

                labelWidths.Add(textBlockSize.Width);
            }

            _maxRowHeight = maxRowHeight;

            Boolean overlap;
            Int32 rows;
            for (rows = 1; rows <= 3; rows++)
            {
                overlap = false;
                for (Int32 i = 0; i < labelWidths.Count - rows; i++)
                {
                    Double labelFittingSize = labelWidths[i] / 2 + labelWidths[i + rows] / 2;
                    if (labelFittingSize > pixelInterval * rows)
                    {
                        overlap = true;
                        break;
                    }
                }
                if (!overlap)
                    break;
            }

            return rows;
        }

        /// <summary>
        /// Calculate number of rows for axislabels
        /// </summary>
        /// <returns></returns>
        private Int32 CalculateNumberOfRows()
        {
            if (InternalRows <= 0)
            {
                Int32 rows;
                rows = CalculateRows();
                return rows;
            }
            else
                return (Int32)InternalRows;
        }

        /// <summary>
        /// Set properties of a textblock
        /// </summary>
        /// <param name="textBlock">TextBlock</param>
        /// <returns>TextBlock</returns>
        private TextBlock SetFontProperties(TextBlock textBlock)
        {
            textBlock.FontSize = InternalFontSize;
            /* set other font properties */
            textBlock.FontFamily = InternalFontFamily;
            textBlock.FontStyle = InternalFontStyle;
            textBlock.FontWeight = InternalFontWeight;
            
            return textBlock;
        }

        /// <summary>
        /// Get max height of axislabels
        /// </summary>
        /// <returns>Double</returns>
        private Double GetMaxHeight()
        {
            TextBlock textBlock = new TextBlock();

            textBlock = SetFontProperties(textBlock);

            Double maxRowHeight = 0;
            Size textBlockSize;

            Int32 labelIndex = 0;
            for (labelIndex = 0; labelIndex < AxisLabelList.Count ; labelIndex += (ParentAxis.SkipOffset + 1))
            {
                AxisLabel label = AxisLabelList[labelIndex];
                textBlock.Text = label.Text;
                textBlockSize = Graphics.CalculateTextBlockSize(AxisLabel.GetRadians(GetAngle()), textBlock);
                maxRowHeight = Math.Max(maxRowHeight, textBlockSize.Height);
            }

            return maxRowHeight;
        }

        /// <summary>
        /// Calculate default values for angle and rows of axis
        /// </summary>
        private void CalculateHorizontalDefaults()
        {
            IsNotificationEnable = false;

            Double width = Double.IsNaN(Width) ? 0 : Width;
            Double height = Double.IsNaN(Height) ? 0 : Height;
            Double max = Math.Max(width, height);
            if (Double.IsNaN(InternalFontSize) || InternalFontSize <= 0)
            {
                Double initialFontSize = CalculateFontSize(max);
                InternalFontSize = initialFontSize;
            }

            if (InternalRows <= 0)
            {
                Int32 rows = CalculateNumberOfRows();

                if (rows > 2 && Double.IsNaN((Double)InternalAngle))
                {
                    InternalRows = 1;

                    InternalAngle = ((Chart as Chart).IsScrollingActivated && ParentAxis.XValueType != ChartValueTypes.Numeric) ? -90 : -45;

                    if ((Double.IsNaN((Double)ParentAxis.Interval) && Double.IsNaN((Double)Interval) || (ParentAxis.IntervalType == IntervalTypes.Auto && ParentAxis.IsDateTimeAxis)))
                        ParentAxis.SkipOffset = CalculateSkipOffset((int)InternalRows, (Double)InternalAngle, Width);
                    else
                    {
                        ParentAxis.SkipOffset = 0;

                        rows = CalculateRows();

                        InternalRows = rows;
                    }
                }
                else if (rows >= 2 && !Double.IsNaN((Double)InternalAngle) && (Double.IsNaN((Double)ParentAxis.Interval) && Double.IsNaN((Double)Interval) || (ParentAxis.IntervalType == IntervalTypes.Auto && ParentAxis.IsDateTimeAxis)))
                {
                    InternalRows = 1;

                    ParentAxis.SkipOffset = CalculateSkipOffset((int)InternalRows, (Double)InternalAngle, Width);
                    
                }
                else
                {
                    InternalRows = rows;
                }
            }
            else
            {   
                Int32 rows = CalculateNumberOfRows();

                if (rows > 2 && Double.IsNaN((Double)InternalAngle))
                {
                    InternalAngle = ((Chart as Chart).IsScrollingActivated && ParentAxis.XValueType != ChartValueTypes.Numeric) ? -90 : -45;
                }
            }

            _maxRowHeight = GetMaxHeight();

            IsNotificationEnable = true;
        }
        
        /// <summary>
        /// Calculate skip offset for axis labels
        /// </summary>
        /// <param name="noOfRows">Number of rows</param>
        /// <param name="angle">Rotation angle of labels</param>
        /// <param name="axisWidth">Width of the axis</param>
        /// <returns>Offset as Int32</returns>
        private Int32 CalculateSkipOffset(Int32 noOfRows, Double angle, Double axisWidth)
        {
            Int32 skipOffset = 0;             // Skip offset
            Boolean overlap = true;

            Double interval = (Double)((Double.IsNaN((Double)Interval) && Interval <= 0) ? Interval : ParentAxis.InternalInterval);
            //Double interval = (Double)((Double.IsNaN((Double)Interval) || Interval <= 0) ? ParentAxis.InternalInterval : Interval);
            Double pixelInterval;
            TextBlock textBlock = new TextBlock();
            textBlock = SetFontProperties(textBlock);
            //textBlock.Text = "ABCD";
            
            Double maxHeight = 0;
            List<Double> labelHeights = new List<Double>();
            
            foreach (AxisLabel label in AxisLabelList)
            {
                textBlock.Text = label.Text;
#if WPF 
                Size textBlockSize = Graphics.CalculateVisualSize(textBlock);
                labelHeights.Add(textBlockSize.Height);
#else
                labelHeights.Add(textBlock.ActualHeight);
#endif
            }

            for (Int32 i = 0; i < labelHeights.Count - 1; i++)
            {
                maxHeight = Math.Max(maxHeight, (labelHeights[i] + labelHeights[i + 1]) / 2 + 2);
            }

            while(overlap)
            {   
                pixelInterval = Graphics.ValueToPixelPosition(0, Width, Minimum, Maximum, interval + skipOffset + Minimum);
#if WPF
                if (pixelInterval >= maxHeight)
#else
                if (pixelInterval >= maxHeight)
#endif
                {
                    overlap = false;
                }
                else
                    skipOffset++;
            }

            return skipOffset;
        }

        /// <summary>
        /// Returns formatted string from a given value depending upon scaling set and value format string
        /// </summary>
        /// <param name="value">Double value</param>
        /// <returns>String</returns>
        private String GetFormattedString(Double value)
        {
            return (ParentAxis != null)? ParentAxis.GetFormattedString(value) : value.ToString();
        }

        /// <summary>
        ///  Calculate default values for vertical axis
        /// </summary>
        private void CalculateVerticalDefaults()
        {
            Double width = Double.IsNaN(Width) ? 0 : Width;
            Double height = Double.IsNaN(Height) ? 0 : Height;
            Double max = Math.Max(width, height);
            if (Double.IsNaN(InternalFontSize) || InternalFontSize <= 0)
            {
                InternalFontSize = CalculateFontSize(max);
            }
        }

        #endregion

        #region Private Properties
        
        /// <summary>
        /// Identifies the Visifire.Charts.AxisLabels.ToolTipText dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.AxisLabels.ToolTipText dependency property.
        /// </returns>
        private new static readonly DependencyProperty ToolTipTextProperty = DependencyProperty.Register
            ("ToolTipText",
            typeof(String),
            typeof(AxisLabels),
            null);
            
        #endregion

        #region Internal Methods

        /// <summary>
        /// Update visual used for partial update
        /// </summary>
        /// <param name="propertyName">Name of the property</param>
        /// <param name="value">Value of the property</param>
        internal override void UpdateVisual(string propertyName, object value)
        {   
            if (Visual != null)
            {
                foreach (AxisLabel axisLabel in AxisLabelList)
                {
                    ApplyAxisLabelFontProperties(axisLabel);
                    axisLabel.ApplyProperties(axisLabel);
                }
            }
            else
                FirePropertyChanged(propertyName);
        }

        /// <summary>
        /// Creates a visual object for the element
        /// </summary>
        internal void CreateVisualObject()
        {
            // Create a new 
            Visual = new Canvas();

            if (!(Boolean)Enabled)
            {
                Visual = null;
                return;
            }

            // Create new Labels list
            AxisLabelList = new List<AxisLabel>();

            // List to store the values for which the labels are created
            LabelValues = new List<Double>();

            if (InternalFontSize != _savedFontSize || InternalAngle != _savedAngle || InternalRows != _savedRows)
                _isRedraw = false;

            // check if this is a first time draw or a redraw
            if (_isRedraw)
            {
                // if redraw then restore the original values
                InternalAngle = _savedAngle;
                InternalFontSize = _savedFontSize;
                InternalRows = _savedRows;
            }
            else
            {
                // Preserve the original values for future use
                _savedAngle = (Double)InternalAngle;
                _savedFontSize = InternalFontSize;
                _savedRows = (Int32)InternalRows;
                _isRedraw = true;
            }

            // create the required labels
            CreateLabels();

            // Set the position of the labels
            SetLabelPosition();

            Visual.Opacity = InternalOpacity;
        }

        #endregion

        #region Internal Events

        #endregion

        #region Data

        /// <summary>
        /// Saved max row height
        /// </summary>
        private Double _maxRowHeight;

        /// <summary>
        /// Saved font size of axislabels
        /// </summary>
        private Double _savedFontSize;

        /// <summary>
        /// Saved number of rows
        /// </summary>
        private Int32 _savedRows;

        /// <summary>
        /// Saved old angle
        /// </summary>
        private Double _savedAngle;

        /// <summary>
        /// Whether the axis need to redraw
        /// </summary>
        private Boolean _isRedraw;

        /// <summary>
        /// Parent axis
        /// </summary>
        private Axis _parent;

        private Double _internalFontSize = Double.NaN;
        private FontFamily _internalFontFamily = null;
        internal Brush InternalFontColor;
        Nullable<FontStyle> _internalFontStyle = null;
        Nullable<FontWeight> _internalFontWeight = null;
        Double _internalOpacity = Double.NaN;

#if WPF

        /// <summary>
        /// Whether the default style is applied
        /// </summary>
        private static Boolean _defaultStyleKeyApplied;
#endif
        #endregion
    }
}