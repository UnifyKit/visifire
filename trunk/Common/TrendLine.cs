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
using System.Windows.Media;
using System.Windows.Input;
using System.Windows.Shapes;


#else
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Input;

#endif
using Visifire.Commons;

namespace Visifire.Charts
{

    /// <summary>
    /// TrendLine can be used to draw lines to indicate significance of certain points
    /// </summary>
    public class TrendLine : ObservableObject
    {
        #region Public Methods

        /// <summary>
        /// Initializes a new instance of the Visifire.Charts.TrendLine class
        /// </summary>
        public TrendLine()
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Identifies the Visifire.Charts.TrendLine.Enabled dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.TrendLine.Enabled dependency property.
        /// </returns>
        public static readonly DependencyProperty EnabledProperty = DependencyProperty.Register(
            "Enabled",
            typeof(Nullable<Boolean>),
            typeof(TrendLine),
            new PropertyMetadata(OnEnabledPropertyChanged));

#if WPF
        /// <summary>
        /// Identifies the Visifire.Charts.TrendLine.Opacity dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.TrendLine.Opacity dependency property.
        /// </returns>
        public new static readonly DependencyProperty OpacityProperty = DependencyProperty.Register
            ("Opacity",
            typeof(Double),
            typeof(TrendLine),
            new PropertyMetadata(1.0, OnOpacityPropertyChanged));
#endif

        /// <summary>
        /// Identifies the Visifire.Charts.TrendLine.LabelText dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.TrendLine.LabelText dependency property.
        /// </returns>
        public static readonly DependencyProperty LabelTextProperty = DependencyProperty.Register(
            "LabelText",
            typeof(String),
            typeof(TrendLine),
            new PropertyMetadata(OnLabelTextPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.TrendLine.LabelFontFamily dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.TrendLine.LabelFontFamily dependency property.
        /// </returns>
        public static readonly DependencyProperty LabelFontFamilyProperty = DependencyProperty.Register
            ("LabelFontFamily",
            typeof(FontFamily),
            typeof(TrendLine),
            new PropertyMetadata(new FontFamily("Verdana"), OnLabelFontFamilyPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.TrendLine.LabelFontSize dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.TrendLine.LabelFontSize dependency property.
        /// </returns>
        public static readonly DependencyProperty LabelFontSizeProperty = DependencyProperty.Register
             ("LabelFontSize",
             typeof(Double),
             typeof(TrendLine),
             new PropertyMetadata(11.0, OnLabelFontSizePropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.TrendLine.LabelFontColor dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.TrendLine.LabelFontColor dependency property.
        /// </returns>
        public static readonly DependencyProperty LabelFontColorProperty = DependencyProperty.Register
            ("LabelFontColor",
            typeof(Brush),
            typeof(TrendLine),
            new PropertyMetadata(new SolidColorBrush(Colors.Red), OnLabelFontColorPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.TrendLine.LabelFontWeight dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.TrendLine.LabelFontWeight dependency property.
        /// </returns>
        public static readonly DependencyProperty LabelFontWeightProperty = DependencyProperty.Register
            ("LabelFontWeight",
            typeof(FontWeight),
            typeof(TrendLine),
            new PropertyMetadata(OnLabelFontWeightPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.TrendLine.LabelFontStyle dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.TrendLine.LabelFontStyle dependency property.
        /// </returns>
        public static readonly DependencyProperty LabelFontStyleProperty = DependencyProperty.Register
            ("LabelFontStyle",
            typeof(FontStyle),
            typeof(TrendLine),
            new PropertyMetadata(OnLabelFontStylePropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.TrendLine.LineColor dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.TrendLine.LineColor dependency property.
        /// </returns>
        public static readonly DependencyProperty LineColorProperty = DependencyProperty.Register(
            "LineColor",
            typeof(Brush),
            typeof(TrendLine),
            new PropertyMetadata(OnLineColorPropertyChanged));
        
        /// <summary>
        /// Identifies the Visifire.Charts.TrendLine.LineThickness dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.TrendLine.LineThickness dependency property.
        /// </returns>
        public static readonly DependencyProperty LineThicknessProperty = DependencyProperty.Register(
            "LineThickness",
            typeof(Double),
            typeof(TrendLine),
            new PropertyMetadata(OnLineThicknessPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.TrendLine.LineStyle dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.TrendLine.LineStyle dependency property.
        /// </returns>
        public static readonly DependencyProperty LineStyleProperty = DependencyProperty.Register(
            "LineStyle",
            typeof(LineStyles),
            typeof(TrendLine),
            new PropertyMetadata(OnLineStylePropertyChanged));
        
        /// <summary>
        /// Identifies the Visifire.Charts.TrendLine.ShadowEnabled dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.TrendLine.ShadowEnabled dependency property.
        /// </returns>
        public static readonly DependencyProperty ShadowEnabledProperty = DependencyProperty.Register(
            "ShadowEnabled",
            typeof(bool),
            typeof(TrendLine),
            new PropertyMetadata(OnShadowEnabledPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.TrendLine.Value dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.TrendLine.Value dependency property.
        /// </returns>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value",
            typeof(Object),
            typeof(TrendLine),
            new PropertyMetadata(OnValuePropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.TrendLine.StartValue dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.TrendLine.StartValue dependency property.
        /// </returns>
        public static readonly DependencyProperty StartValueProperty = DependencyProperty.Register(
            "StartValue",
            typeof(Object),
            typeof(TrendLine),
            new PropertyMetadata(OnStartValuePropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.TrendLine.EndValue dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.TrendLine.EndValue dependency property.
        /// </returns>
        public static readonly DependencyProperty EndValueProperty = DependencyProperty.Register(
            "EndValue",
            typeof(Object),
            typeof(TrendLine),
            new PropertyMetadata(OnEndValuePropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.TrendLine.HrefTarget dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.TrendLine.HrefTarget dependency property.
        /// </returns>
        public static readonly DependencyProperty HrefTargetProperty = DependencyProperty.Register(
            "HrefTarget",
            typeof(HrefTargets),
            typeof(TrendLine),
            new PropertyMetadata(OnHrefTargetChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.TrendLine.Href dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.TrendLine.Href dependency property.
        /// </returns>
        public static readonly DependencyProperty HrefProperty = DependencyProperty.Register(
            "Href",
            typeof(String),
            typeof(TrendLine),
            new PropertyMetadata(OnHrefChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.TrendLine.AxisType dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.TrendLine.AxisType dependency property.
        /// </returns>
        public static readonly DependencyProperty AxisTypeProperty = DependencyProperty.Register(
            "AxisType",
            typeof(AxisTypes),
            typeof(TrendLine),
            new PropertyMetadata(OnAxisTypePropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.TrendLine.Orientation dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.TrendLine.Orientation dependency property.
        /// </returns>
        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(
            "Orientation",
            typeof(Orientation),
            typeof(TrendLine),
            new PropertyMetadata(Orientation.Horizontal, OnOrientationPropertyChanged));

        /// <summary>
        /// Enables or disables the TrendLine
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
        /// Get or set the label text of the TrendLine
        /// </summary>
        public String LabelText
        {
            get
            {
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
        public Double LabelFontSize
        {
            get
            {
                return (Double)GetValue(LabelFontSizeProperty);
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
        /// Get or set the Color of the TrendLine
        /// </summary>
        public Brush LineColor
        {
            get 
            {
                if ((Brush)GetValue(LineColorProperty) == null)
                {
                    if (StartValue != null && EndValue != null)
                        return new SolidColorBrush(Color.FromArgb((byte)255, (byte)255, (byte)228, (byte)196));
                    else
                        return new SolidColorBrush(Colors.Red);
                }
                else
                    return (Brush)GetValue(LineColorProperty);
            }
            set
            { 
                SetValue(LineColorProperty, value); 
            }
        }

        /// <summary>
        /// Get or set the Line thickness of TrendLine
        /// </summary>
        public Double LineThickness
        {
            get 
            { 
                return (Double)GetValue(LineThicknessProperty) == 0 ? 2 : (Double)GetValue(LineThicknessProperty);
            }
            set
            { 
                SetValue(LineThicknessProperty, value); 
            }
        }

        /// <summary>
        /// Get or set the Line style of TrendLine
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
        /// Whether shadow is enebled for the TrendLine
        /// </summary>
        public bool ShadowEnabled
        {
            get 
            { 
                return (bool)GetValue(ShadowEnabledProperty);
            }
            set 
            { 
                SetValue(ShadowEnabledProperty, value); 
            }
        }

        /// <summary>
        /// Get or set the Value of the TrendLine
        /// </summary>
        public Object Value
        {
            get
            {
                return (Object)GetValue(ValueProperty);
            }
            set
            {
                SetValue(ValueProperty, value);
            }
        }

        /// <summary>
        /// Get or set the start value of the TrendLine
        /// </summary>
        public Object StartValue
        {
            get
            {
                return (Object)GetValue(StartValueProperty);
            }
            set
            {
                SetValue(StartValueProperty, value);
            }
        }

        /// <summary>
        /// Get or set the end value of the TrendLine
        /// </summary>
        public Object EndValue
        {
            get
            {
                return (Object)GetValue(EndValueProperty);
            }
            set
            {
                SetValue(EndValueProperty, value);
            }
        }

        /// <summary>
        /// Get or set the AxisType of the TrendLine
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
        /// Get or set the Orientation of the TrendLine. 
        /// Whether the TrendLine should be vertically oriented or horizontally oriented
        /// </summary>
        public Orientation Orientation
        {
            get 
            { 
                return (Orientation)GetValue(OrientationProperty); 
            }
            set 
            { 
                SetValue(OrientationProperty, value); 
            }
        }

        /// <summary>
        /// Get or set the HrefTarget property of TrendLine
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
        /// Get or set the Href property of TrendLine
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

        #endregion

        #region Public Events And Delegates

        #endregion

        #region Protected Methods

        #endregion

        #region Internal Properties

        internal TextBlock LabelTextBlock
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set the Value of the TrendLine
        /// </summary>
        internal DateTime InternalDateValue
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set the start date value of the TrendLine
        /// </summary>
        internal DateTime InternalDateStartValue
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set the end date value of the TrendLine
        /// </summary>
        internal DateTime InternalDateEndValue
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set the Value of the TrendLine
        /// </summary>
        internal Double InternalNumericValue
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set the start value of the TrendLine
        /// </summary>
        internal Double InternalNumericStartValue
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set the end value of the TrendLine
        /// </summary>
        internal Double InternalNumericEndValue
        {
            get;
            set;
        }

        /// <summary>
        /// Axis reference
        /// </summary>
        internal Axis ReferingAxis
        {
            get;
            set;
        }

        /// <summary>
        /// TrendLine canvas
        /// </summary>
        internal Canvas Visual
        {
            get;
            set;
        }

        #endregion

        #region Private Properties

        /// <summary>
        /// Line visual
        /// </summary>
        private Line Line
        {
            get;
            set;
        }

        /// <summary>
        /// Start and End value visual
        /// </summary>
        private Rectangle Rectangle
        {
            get;
            set;
        }

        /// <summary>
        /// Shadow visual
        /// </summary>
        private Line Shadow
        {
            get;
            set;
        }

        /// <summary>
        /// Shadow rectangle visual
        /// </summary>
        private Rectangle ShadowRectangle
        {
            get;
            set;
        }

        #endregion

        #region Private Delegates

        #endregion

        #region Private Methods

        /// <summary>
        /// EnabledProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnEnabledPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TrendLine source = d as TrendLine;
            source.FirePropertyChanged(VcProperties.Enabled);
        }

#if WPF
        /// <summary>
        /// OpacityProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnOpacityPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TrendLine source = d as TrendLine;
            source.FirePropertyChanged(VcProperties.Opacity);
        }
#endif

        /// <summary>
        /// LabelTextProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnLabelTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TrendLine source = d as TrendLine;
            source.UpdateVisual(VcProperties.LabelText, e.NewValue);
        }

        /// <summary>
        /// LabelFontFamilyProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnLabelFontFamilyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TrendLine source = d as TrendLine;
            source.UpdateVisual(VcProperties.LabelFontFamily, e.NewValue);
        }

        /// <summary>
        /// LabelFontSizeProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnLabelFontSizePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TrendLine source = d as TrendLine;
            source.UpdateVisual(VcProperties.LabelFontSize, e.NewValue);
        }

        /// <summary>
        /// LabelFontColorProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnLabelFontColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TrendLine source = d as TrendLine;
            source.UpdateVisual(VcProperties.LabelFontColor, e.NewValue);
        }

        /// <summary>
        /// LabelFontWeightProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnLabelFontWeightPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TrendLine source = d as TrendLine;
            source.UpdateVisual(VcProperties.LabelFontWeight, e.NewValue);
        }

        /// <summary>
        /// LabelFontStyleProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnLabelFontStylePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TrendLine source = d as TrendLine;
            source.UpdateVisual(VcProperties.LabelFontStyle, e.NewValue);
        }


        /// <summary>
        /// LineColorProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnLineColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TrendLine source = d as TrendLine;
            source.UpdateVisual(VcProperties.LineColor, e.NewValue);
        }

        /// <summary>
        /// LineThicknessProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnLineThicknessPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TrendLine source = d as TrendLine;
            //source.FirePropertyChanged(VcProperties.LineThickness");
            source.UpdateVisual(VcProperties.LineThickness, e.NewValue);
        }

        /// <summary>
        /// LineStyleProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnLineStylePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TrendLine source = d as TrendLine;
            source.UpdateVisual(VcProperties.LineStyle, e.NewValue);
        }

        /// <summary>
        /// ShadowEnabledProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnShadowEnabledPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TrendLine trendLine = d as TrendLine;
            trendLine.UpdateVisual(VcProperties.ShadowEnabled, e.NewValue);
        }

        /// <summary>
        /// ValueProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TrendLine trendLine = d as TrendLine;

            // Double / Int32 value entered in Managed Code
            if (e.NewValue.GetType().Equals(typeof(Double)) || e.NewValue.GetType().Equals(typeof(Int32)))
            {
                trendLine.InternalNumericValue = Convert.ToDouble(e.NewValue);
            }
            // DateTime value entered in Managed Code
            else if ((e.NewValue.GetType().Equals(typeof(DateTime))))
            {
                trendLine.InternalDateValue = (DateTime)e.NewValue;
            }
            // Double / Int32 / DateTime entered in XAML
            else if ((e.NewValue.GetType().Equals(typeof(String))))
            {
                DateTime dateTimeresult;
                Double doubleResult;

                if (String.IsNullOrEmpty(e.NewValue.ToString()))
                {
                    trendLine.InternalNumericValue = Double.NaN;
                }
                // Double entered in XAML
                else if (Double.TryParse((string)e.NewValue, System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.InvariantCulture, out doubleResult))
                {
                    trendLine.InternalNumericValue = doubleResult;
                }
                // DateTime entered in XAML
                else if (DateTime.TryParse((string)e.NewValue, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dateTimeresult))
                {
                    trendLine.InternalDateValue = dateTimeresult;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Invalid Input for AxisMaximum");
                    throw new Exception("Invalid Input for AxisMaximum");
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Invalid Input for AxisMaximum");
                throw new Exception("Invalid Input for AxisMaximum");
            }

            trendLine.UpdateVisual(VcProperties.Value, e.NewValue);
        }

        /// <summary>
        /// StartValueProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnStartValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TrendLine trendLine = d as TrendLine;

            // Double / Int32 value entered in Managed Code
            if (e.NewValue.GetType().Equals(typeof(Double)) || e.NewValue.GetType().Equals(typeof(Int32)))
            {
                trendLine.InternalNumericStartValue = Convert.ToDouble(e.NewValue);
            }
            // DateTime value entered in Managed Code
            else if ((e.NewValue.GetType().Equals(typeof(DateTime))))
            {
                trendLine.InternalDateStartValue = (DateTime)e.NewValue;
            }
            // Double / Int32 / DateTime entered in XAML
            else if ((e.NewValue.GetType().Equals(typeof(String))))
            {
                DateTime dateTimeresult;
                Double doubleResult;

                if (String.IsNullOrEmpty(e.NewValue.ToString()))
                {
                    trendLine.InternalNumericStartValue = Double.NaN;
                }
                // Double entered in XAML
                else if (Double.TryParse((string)e.NewValue, System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.InvariantCulture, out doubleResult))
                {
                    trendLine.InternalNumericStartValue = doubleResult;
                }
                // DateTime entered in XAML
                else if (DateTime.TryParse((string)e.NewValue, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dateTimeresult))
                {
                    trendLine.InternalDateStartValue = dateTimeresult;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Invalid Input for AxisMaximum");
                    throw new Exception("Invalid Input for AxisMaximum");
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Invalid Input for AxisMaximum");
                throw new Exception("Invalid Input for AxisMaximum");
            }

            trendLine.UpdateVisual(VcProperties.StartValue, e.NewValue);
        }

        /// <summary>
        /// EndValueProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnEndValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TrendLine trendLine = d as TrendLine;

            // Double / Int32 value entered in Managed Code
            if (e.NewValue.GetType().Equals(typeof(Double)) || e.NewValue.GetType().Equals(typeof(Int32)))
            {
                trendLine.InternalNumericEndValue = Convert.ToDouble(e.NewValue);
            }
            // DateTime value entered in Managed Code
            else if ((e.NewValue.GetType().Equals(typeof(DateTime))))
            {
                trendLine.InternalDateEndValue = (DateTime)e.NewValue;
            }
            // Double / Int32 / DateTime entered in XAML
            else if ((e.NewValue.GetType().Equals(typeof(String))))
            {
                DateTime dateTimeresult;
                Double doubleResult;

                if (String.IsNullOrEmpty(e.NewValue.ToString()))
                {
                    trendLine.InternalNumericEndValue = Double.NaN;
                }
                // Double entered in XAML
                else if (Double.TryParse((string)e.NewValue, System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.InvariantCulture, out doubleResult))
                {
                    trendLine.InternalNumericEndValue = doubleResult;
                }
                // DateTime entered in XAML
                else if (DateTime.TryParse((string)e.NewValue, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dateTimeresult))
                {
                    trendLine.InternalDateEndValue = dateTimeresult;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Invalid Input for AxisMaximum");
                    throw new Exception("Invalid Input for AxisMaximum");
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Invalid Input for AxisMaximum");
                throw new Exception("Invalid Input for AxisMaximum");
            }

            trendLine.UpdateVisual(VcProperties.EndValue, e.NewValue);
        }


        /// <summary>
        /// AxisTypeProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnAxisTypePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TrendLine source = d as TrendLine;
            source.FirePropertyChanged(VcProperties.AxisType);
        }

        /// <summary>
        /// OrientationProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnOrientationPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TrendLine source = d as TrendLine;
            source.FirePropertyChanged(VcProperties.Orientation);
        }

        /// <summary>
        /// HrefTargetProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnHrefTargetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TrendLine trendLine = d as TrendLine;
            trendLine.FirePropertyChanged(VcProperties.HrefTarget);
        }

        /// <summary>
        /// HrefProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnHrefChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TrendLine trendLine = d as TrendLine;
            trendLine.FirePropertyChanged(VcProperties.Href);
        }

        internal void ApplyLabelFontProperties(TextBlock tb)
        {
            if (!String.IsNullOrEmpty(LabelText))
            {
                tb.Text = LabelText;
                tb.FontFamily = LabelFontFamily;
                tb.FontSize = LabelFontSize;
                tb.FontStyle = LabelFontStyle;
                tb.FontWeight = LabelFontWeight;
                tb.Foreground = LabelFontColor;
            }
        }

        /// <summary>
        /// Apply properties of TrendLine to line visual and line-shadow visual
        /// </summary>
        private void ApplyProperties()
        {
            if (Line != null)
            {
                Line.Stroke = LineColor;
                Line.StrokeThickness = LineThickness;
                Line.StrokeDashArray = ExtendedGraphics.GetDashArray(LineStyle);
            }

            if (Shadow != null)
            {
                Shadow.StrokeThickness = LineThickness + 2;
                Shadow.StrokeDashArray = ExtendedGraphics.GetDashArray(LineStyle);

                Shadow.Stroke = new SolidColorBrush(Colors.LightGray);
                Shadow.Opacity = 0.7;

                Shadow.StrokeDashCap = PenLineCap.Round;
                Shadow.StrokeLineJoin = PenLineJoin.Round;
                Shadow.Visibility = ShadowEnabled ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Apply properties of TrendLine to line visual and line-shadow visual
        /// </summary>
        private void ApplyRectangleProperties()
        {
            if (Rectangle != null)
                Rectangle.Fill = LineColor;

            if (ShadowRectangle != null)
            {
                ShadowRectangle.Fill = new SolidColorBrush(Colors.LightGray);
                ShadowRectangle.Opacity = 0.7;
                ShadowRectangle.Visibility = ShadowEnabled ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// UpdateVisual is used for partial rendering
        /// </summary>
        /// <param name="propertyName">Name of the property</param>
        /// <param name="value">Value of the property</param>
        internal override void UpdateVisual(VcProperties propertyName, object value)
        {
            if (Line == null || Shadow == null)
                FirePropertyChanged(propertyName);
            else if (propertyName == VcProperties.Value)
            {
                Chart chart = (Chart as Chart);
                Axis axis = chart.PlotDetails.GetAxisXFromChart(chart, AxisType);
                chart.PlotDetails.SetTrendLineValue(this, axis);
                chart.PlotDetails.SetTrendLineStartAndEndValue(this, axis);
                Canvas visualCanvas = (Chart as Chart).ChartArea.ChartVisualCanvas;
                PositionTrendLineLabel(visualCanvas.Width, visualCanvas.Height);
                PositionTheLine(visualCanvas.Width, visualCanvas.Height);
                PositionTheStartEndRectangle(visualCanvas.Width, visualCanvas.Height);
            }
            else
            {
                ApplyProperties();
                ApplyRectangleProperties();
                ApplyLabelFontProperties(LabelTextBlock);
                PositionTrendLineLabel((Chart as Chart).ChartArea.ChartVisualCanvas.Width, (Chart as Chart).ChartArea.ChartVisualCanvas.Height);
            }
        }

        internal void UpdateTrendLineLabelPosition(Double width, Double height)
        {
            PositionTrendLineLabel(width, height);
        }

        /// <summary>
        /// Set label position for TrendLine
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        private void PositionTrendLineLabel(Double width, Double height)
        {
            if (LabelTextBlock != null)
            {
                Chart chart = (Chart as Chart);

                if (Value == null)
                {
                    InternalNumericValue = (InternalNumericStartValue + InternalNumericEndValue) / 2;
                }

                Double xPos = Graphics.ValueToPixelPosition(0,
                           width,
                           (Double)ReferingAxis.InternalAxisMinimum,
                           (Double)ReferingAxis.InternalAxisMaximum,
                           InternalNumericValue);

                Double yPos = Graphics.ValueToPixelPosition(height,
                            0,
                            (Double)ReferingAxis.InternalAxisMinimum,
                            (Double)ReferingAxis.InternalAxisMaximum,
                            InternalNumericValue);

                Point positionWithRespect2ChartArea;

                Double offset = chart.ChartArea.AxisX.GetScrollBarValueFromOffset(chart.ChartArea.AxisX.CurrentScrollScrollBarOffset);
                offset = chart.ChartArea.GetScrollingOffsetOfAxis(chart.ChartArea.AxisX, offset);

                Double topOffsetGap = chart._topOffsetGrid.Height;

                Double labelPadding = 2;

#if WPF
            Size textBlockSize = Graphics.CalculateVisualSize(LabelTextBlock);
#else
                Size textBlockSize = new Size(LabelTextBlock.ActualWidth, LabelTextBlock.ActualHeight);
#endif

                if (chart.PlotDetails.ChartOrientation == ChartOrientationType.Vertical)
                {
                    if (AxisType == AxisTypes.Primary)
                    {
                        if (Orientation == Orientation.Horizontal)
                        {
                            positionWithRespect2ChartArea = chart.ChartArea.GetPositionWithRespect2ChartArea(new Point(0, yPos));
                            LabelTextBlock.SetValue(Canvas.LeftProperty, chart.ChartArea.GetPlotAreaStartPosition().X - labelPadding - textBlockSize.Width);
                            LabelTextBlock.SetValue(Canvas.TopProperty, positionWithRespect2ChartArea.Y + topOffsetGap - textBlockSize.Height / 2);
                        }
                        else
                        {
                            positionWithRespect2ChartArea = chart.ChartArea.GetPositionWithRespect2ChartArea(new Point(xPos, 0));
                            LabelTextBlock.SetValue(Canvas.LeftProperty, positionWithRespect2ChartArea.X - offset - textBlockSize.Width / 2);
                            Double newYPos = chart.ChartArea.GetAxisTop(chart.ChartArea.AxisX) + ((chart.IsScrollingActivated || chart.ZoomingEnabled) ? chart.ChartArea.AxisX.ScrollBarSize : 0);
                            LabelTextBlock.SetValue(Canvas.TopProperty, newYPos + topOffsetGap);
                        }
                    }
                    else
                    {
                        if (Orientation == Orientation.Horizontal)
                        {
                            positionWithRespect2ChartArea = chart.ChartArea.GetPositionWithRespect2ChartArea(new Point(0, yPos));
                            Double newXPos = chart.ChartArea.GetAxisLeft(chart.ChartArea.AxisY2);
                            LabelTextBlock.SetValue(Canvas.LeftProperty, newXPos + labelPadding);
                            LabelTextBlock.SetValue(Canvas.TopProperty, positionWithRespect2ChartArea.Y + chart._topOffsetGrid.ActualHeight - textBlockSize.Height / 2);
                        }
                    }
                }
                else
                {
                    if (AxisType == AxisTypes.Primary)
                    {
                        if (Orientation == Orientation.Horizontal)
                        {
                            positionWithRespect2ChartArea = chart.ChartArea.GetPositionWithRespect2ChartArea(new Point(0, yPos));
                            LabelTextBlock.SetValue(Canvas.LeftProperty, chart.ChartArea.GetPlotAreaStartPosition().X - labelPadding - ((chart.IsScrollingActivated || chart.ZoomingEnabled) ? chart.ChartArea.AxisX.ScrollBarSize : 0) - textBlockSize.Width);
                            LabelTextBlock.SetValue(Canvas.TopProperty, positionWithRespect2ChartArea.Y - offset - textBlockSize.Height / 2);
                        }
                        else
                        {
                            positionWithRespect2ChartArea = chart.ChartArea.GetPositionWithRespect2ChartArea(new Point(xPos, 0));
                            LabelTextBlock.SetValue(Canvas.LeftProperty, positionWithRespect2ChartArea.X - textBlockSize.Width / 2);
                            Double newYPos = chart.ChartArea.GetAxisTop(chart.ChartArea.AxisY);
                            LabelTextBlock.SetValue(Canvas.TopProperty, newYPos);

                        }
                    }
                    else
                    {
                        if (Orientation == Orientation.Vertical)
                        {
                            positionWithRespect2ChartArea = chart.ChartArea.GetPositionWithRespect2ChartArea(new Point(xPos, 0));
                            LabelTextBlock.SetValue(Canvas.LeftProperty, positionWithRespect2ChartArea.X - textBlockSize.Width / 2);
                            Double newYPos = chart.ChartArea.GetAxisTop(chart.ChartArea.AxisY2);
                            LabelTextBlock.SetValue(Canvas.TopProperty, newYPos);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Set the position of start and end rectange
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        private void PositionTheStartEndRectangle(Double width, Double height)
        {
            Double shadowThickness = LineThickness + 2;

            (Chart as Chart).PlotDetails.SetTrendLineStartAndEndValues(ReferingAxis);

            if (Rectangle != null)
            {
                switch (Orientation)
                {
                    case Orientation.Vertical:

                        Double X1 = Graphics.ValueToPixelPosition(0,
                        width,
                        (Double)ReferingAxis.InternalAxisMinimum,
                        (Double)ReferingAxis.InternalAxisMaximum,
                        InternalNumericStartValue);

                        Double X2 = Graphics.ValueToPixelPosition(0,
                        width,
                        (Double)ReferingAxis.InternalAxisMinimum,
                        (Double)ReferingAxis.InternalAxisMaximum,
                        InternalNumericEndValue);

                        Rectangle.SetValue(Canvas.LeftProperty, X1);
                        Rectangle.SetValue(Canvas.TopProperty, (Double)0);
                        Rectangle.Width = X2 - X1;
                        Rectangle.Height = height;
                        Visual.Height = height;
                        //Visual.Width = shadowThickness;

                        break;
                    case Orientation.Horizontal:

                        Double Y1 = Graphics.ValueToPixelPosition(height,
                        0,
                       (Double)ReferingAxis.InternalAxisMinimum,
                       (Double)ReferingAxis.InternalAxisMaximum,
                       InternalNumericEndValue);

                        Double Y2 = Graphics.ValueToPixelPosition(height,
                        0,
                        (Double)ReferingAxis.InternalAxisMinimum,
                        (Double)ReferingAxis.InternalAxisMaximum,
                        InternalNumericStartValue);

                        Rectangle.SetValue(Canvas.LeftProperty, (Double)0);
                        Rectangle.SetValue(Canvas.TopProperty, Y1);
                        Rectangle.Width = width;
                        Rectangle.Height = Y2 - Y1;
                        //Visual.Height = shadowThickness;
                        Visual.Width = width;

                        break;

                }
            }

            if (ShadowRectangle != null)
            {
                if (Orientation == Orientation.Horizontal)
                {
                    ShadowRectangle.SetValue(Canvas.LeftProperty, (Double)Rectangle.GetValue(Canvas.LeftProperty));
                    ShadowRectangle.SetValue(Canvas.TopProperty, (Double)Rectangle.GetValue(Canvas.TopProperty) + 3);
                    ShadowRectangle.Width = Rectangle.Width + 3;
                    ShadowRectangle.Height = Rectangle.Height + 3;
                }
                else
                {
                    ShadowRectangle.SetValue(Canvas.LeftProperty, (Double)Rectangle.GetValue(Canvas.LeftProperty) + 3);
                    ShadowRectangle.SetValue(Canvas.TopProperty, (Double)Rectangle.GetValue(Canvas.TopProperty) + 3);
                    ShadowRectangle.Width = Rectangle.Width + 3;
                    ShadowRectangle.Height = Rectangle.Height;
                }
            }
        }

        /// <summary>
        /// Set the position of line
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        private void PositionTheLine(Double width, Double height)
        {
            Double shadowThickness = LineThickness + 2;

            (Chart as Chart).PlotDetails.SetTrendLineValues(ReferingAxis);

            switch (Orientation)
            {
                case Orientation.Vertical:
                    Line.Y1 = 0;
                    Line.Y2 = height;
                    Line.X1 = Graphics.ValueToPixelPosition(0,
                    width,
                    (Double)ReferingAxis.InternalAxisMinimum,
                    (Double)ReferingAxis.InternalAxisMaximum,
                    InternalNumericValue);
                    Line.X2 = Line.X1;
                    Visual.Height = height;
                    Visual.Width = shadowThickness;
                    break;
                case Orientation.Horizontal:
                    Line.X1 = 0;
                    Line.X2 = width;
                    Line.Y1 = Graphics.ValueToPixelPosition(height,
                    0,
                    (Double)ReferingAxis.InternalAxisMinimum,
                    (Double)ReferingAxis.InternalAxisMaximum,
                    InternalNumericValue);
                    Line.Y2 = Line.Y1;
                    Visual.Height = shadowThickness;
                    Visual.Width = width;
                    break;

            }

            if (Orientation == Orientation.Horizontal)
            {
                Shadow.X1 = Line.X1;
                Shadow.X2 = Line.X2 + 3;
                Shadow.Y1 = Line.Y1 + 3;
                Shadow.Y2 = Line.Y2 + 3;
            }
            else
            {
                Shadow.X1 = Line.X1 + 3;
                Shadow.X2 = Line.X2 + 3;
                Shadow.Y1 = Line.Y1 + 3;
                Shadow.Y2 = Line.Y2;
            }
        }

        /// <summary>
        /// Create visual objects for TrendLine
        /// </summary>
        /// <param name="width">Width of the ChartCanvas</param>
        /// <param name="height">Height of the ChartCanvas</param>
        internal void CreateVisualObject(Double width, Double height)
        {
            if (ReferingAxis == null || !(Boolean)Enabled)
            {
                Visual = null;
                return;
            }

            if (Visual == null)
                Visual = new Canvas();
            else
                Visual.Children.Clear();

            Visual.Opacity = this.Opacity;
            Visual.Cursor = this.Cursor;
            Double shadowThickness = LineThickness + 2;

            if (StartValue != null && EndValue != null && Value != null)
                throw new Exception("Value property cannot be set with StartValue and EndValue in TrendLine");

            if (InternalNumericStartValue > InternalNumericEndValue)
                throw new Exception("StartValue should be less than or equal to EndValue in a TrendLine");

            if (Value != null)
            {
                Line = new Line() { Tag = new ElementData() { Element = this } };
                Shadow = new Line() { IsHitTestVisible = false };

                PositionTheLine(width, height);

                ApplyProperties();

                Visual.Children.Add(Shadow);
                Visual.Children.Add(Line);
            }

            if(StartValue != null && EndValue != null)
            {
                Rectangle = new Rectangle() { Tag = new ElementData() { Element = this } };
                ShadowRectangle = new Rectangle() { IsHitTestVisible = false };
                Visual.Children.Add(ShadowRectangle);
                Visual.Children.Add(Rectangle);

                PositionTheStartEndRectangle(width, height);

                ApplyRectangleProperties();
            }

            PositionTrendLineLabel(width, height);

            if (Value != null)
            {
                AttachToolTip(ReferingAxis.Chart, this, Line);
                AttachHref(ReferingAxis.Chart, Line, Href, HrefTarget);
            }
            else
            {
                AttachToolTip(ReferingAxis.Chart, this, Rectangle);
                AttachHref(ReferingAxis.Chart, Rectangle, Href, HrefTarget);
            }
        }

        #endregion

        #region Internal Events And Delegates

        #endregion

        #region Data

        #endregion
    }
}
