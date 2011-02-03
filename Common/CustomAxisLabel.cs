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

using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Data;
using Visifire.Commons;

namespace Visifire.Charts
{
    public class CustomAxisLabel : ObservableObject
    {
        /// <summary>
        /// Initializes a new instance of the Visifire.Charts.CustomAxisLabel class
        /// </summary>
        public CustomAxisLabel()
        {
            // Apply default style from generic
#if WPF
            if (!_defaultStyleKeyApplied)
            {
                DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomAxisLabel), new FrameworkPropertyMetadata(typeof(CustomAxisLabel)));
                _defaultStyleKeyApplied = true;

            }
#else
            DefaultStyleKey = typeof(CustomAxisLabel);
#endif
        }

        #region Public Properties


        /// <summary>
        /// Identifies the Visifire.Charts.CustomAxisLabel.Text dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.CustomAxisLabel.Text dependency property.
        /// </returns>
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register
            ("Text",
            typeof(String),
            typeof(CustomAxisLabel),
            new PropertyMetadata(OnTextPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.CustomAxisLabel.To dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.CustomAxisLabel.To dependency property.
        /// </returns>
        public static readonly DependencyProperty ToProperty = DependencyProperty.Register
            ("To",
            typeof(Object),
            typeof(CustomAxisLabel),
            new PropertyMetadata(OnToPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.CustomAxisLabel.From dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.CustomAxisLabel.From dependency property.
        /// </returns>
        public static readonly DependencyProperty FromProperty = DependencyProperty.Register
            ("From",
            typeof(Object),
            typeof(CustomAxisLabel),
            new PropertyMetadata(OnFromPropertyChanged));

//#if WPF

//        /// <summary>
//        /// Identifies the Visifire.Charts.CustomAxisLabels.FontStyle dependency property.
//        /// </summary>
//        /// <returns>
//        /// The identifier for the Visifire.Charts.CustomAxisLabels.FontStyle dependency property.
//        /// </returns>
//        public new static readonly DependencyProperty FontStyleProperty = DependencyProperty.Register
//            ("FontStyle",
//            typeof(FontStyle),
//            typeof(CustomAxisLabels),
//            new PropertyMetadata(OnFontStylePropertyChanged));

//        /// <summary>
//        /// Identifies the Visifire.Charts.CustomAxisLabels.FontWeight dependency property.
//        /// </summary>
//        /// <returns>
//        /// The identifier for the Visifire.Charts.CustomAxisLabels.FontWeight dependency property.
//        /// </returns>
//        public new static readonly DependencyProperty FontWeightProperty = DependencyProperty.Register
//            ("FontWeight",
//            typeof(FontWeight),
//            typeof(CustomAxisLabels),
//            new PropertyMetadata(OnFontWeightPropertyChanged));

//        /// <summary>
//        /// Identifies the Visifire.Charts.CustomAxisLabels.FontSize dependency property.
//        /// </summary>
//        /// <returns>
//        /// The identifier for the Visifire.Charts.CustomAxisLabels.FontSize dependency property.
//        /// </returns>
//        public new static readonly DependencyProperty FontSizeProperty = DependencyProperty.Register
//            ("FontSize",
//            typeof(Double),
//            typeof(CustomAxisLabels),
//            new PropertyMetadata(OnFontSizePropertyChanged));
//#endif


        /// <summary>
        /// Identifies the Visifire.Charts.CustomAxisLabel.LineColor dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.CustomAxisLabel.LineColor dependency property.
        /// </returns>
        public static readonly DependencyProperty LineColorProperty = DependencyProperty.Register
            ("LineColor",
            typeof(Brush),
            typeof(CustomAxisLabel),
            new PropertyMetadata(OnLineColorPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.CustomAxisLabel.LineThickness dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.CustomAxisLabel.LineThickness dependency property.
        /// </returns>
        public static readonly DependencyProperty LineThicknessProperty = DependencyProperty.Register
            ("LineThickness",
            typeof(Nullable<Double>),
            typeof(CustomAxisLabel),
            new PropertyMetadata(OnLineThicknessPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.CustomAxisLabel.FontColor dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.CustomAxisLabel.FontColor dependency property.
        /// </returns>
        public static readonly DependencyProperty FontColorProperty = DependencyProperty.Register
            ("FontColor",
            typeof(Brush),
            typeof(CustomAxisLabel),
            new PropertyMetadata(OnFontColorPropertyChanged));

        /// <summary>
        /// Get or set the color for the line in custom axis label
        /// </summary>
        public Brush LineColor
        {
            get
            {
                if (GetValue(LineColorProperty) == null)
                    return Parent.LineColor;
                else
                    return (Brush)GetValue(LineColorProperty);
            }
            set
            {
                SetValue(LineColorProperty, value);
            }
        }

        /// <summary>
        /// Get or set the color for the line thickness in custom axis label
        /// </summary>
#if SL
        [System.ComponentModel.TypeConverter(typeof(Converters.NullableDoubleConverter))]
#endif
        public Nullable<Double> LineThickness
        {
            get
            {
                if (GetValue(LineThicknessProperty) == null)
                    return Parent.LineThickness;
                else
                    return (Nullable<Double>)GetValue(LineThicknessProperty);
            }
            set
            {
                SetValue(LineThicknessProperty, value);
            }
        }

//        /// <summary>
//        /// Get or set the font family of custom axis label
//        /// </summary>
//        public new FontFamily FontFamily
//        {
//            get
//            {
//                if ((FontFamily)GetValue(FontFamilyProperty) == null)
//                    return new FontFamily("Arial");
//                else
//                    return (FontFamily)GetValue(FontFamilyProperty);
//            }
//            set
//            {

//#if SL
//                if (FontFamily != value)
//                {
//                    InternalFontFamily = value;
//                    SetValue(FontFamilyProperty, value);
//                    FirePropertyChanged(VcProperties.FontFamily);
//                }
//#else           
//                SetValue(FontFamilyProperty, value);
//#endif
//            }
//        }

        /// <summary>
        /// Get or set the color for the text in custom axis label
        /// </summary>
        public Brush FontColor
        {
            get
            {
                if (GetValue(FontColorProperty) == null)
                    return Parent.FontColor;
                else
                    return (Brush)GetValue(FontColorProperty);
            }
            set
            {
                SetValue(FontColorProperty, value);
            }
        }

//        /// <summary>
//        /// Get or set the styles for the text like "Italic" or "Normal"
//        /// </summary>
//#if WPF
//         [TypeConverter(typeof(System.Windows.FontStyleConverter))]
//#endif
//        public new FontStyle FontStyle
//        {
//            get
//            {
//                return (FontStyle)(GetValue(FontStyleProperty));
//            }
//            set
//            {
//#if SL
//                if (InternalFontStyle != value)
//                {
//                    InternalFontStyle = value;
//                    SetValue(FontStyleProperty, value);
//                    UpdateVisual(VcProperties.FontStyle, value);
//                }
//#else
//                 SetValue(FontStyleProperty, value);
//#endif
//            }
//        }

//        /// <summary>
//        /// Get or set how the font appears. It takes values like "Bold", "Normal", "Black" etc
//        /// </summary>
//#if WPF
//        [System.ComponentModel.TypeConverter(typeof(System.Windows.FontWeightConverter))]
//#endif
//        public new FontWeight FontWeight
//        {
//            get
//            {
//                return (FontWeight)(GetValue(FontWeightProperty));
//            }
//            set
//            {

//#if SL
//                if (FontWeight != value)
//                {
//                    InternalFontWeight = value;
//                    SetValue(FontWeightProperty, value);
//                    UpdateVisual(VcProperties.FontWeight, value);
//                }
//#else
//                SetValue(FontWeightProperty, value);
//#endif

//            }
//        }

//        /// <summary>
//        /// Get or set the font size for custom axis label
//        /// </summary>
//        public new Double FontSize
//        {
//            get
//            {
//                return (Double)GetValue(FontSizeProperty);
//            }
//            set
//            {
//#if SL
//                if (FontSize != value)
//                {
//                    InternalFontSize = value;
//                    SetValue(FontSizeProperty, value);
//                    FirePropertyChanged(VcProperties.FontSize);
//                }
//#else
//                SetValue(FontSizeProperty, value);
//#endif
//            }
//        }

        /// <summary>
        /// Set the Text for CustomAxisLabel
        /// </summary>
        public String Text
        {
            get
            {
                return (String)GetValue(TextProperty);
            }
            set
            {
                SetValue(TextProperty, value);
            }
        }

        /// <summary>
        /// Set the Start position of CustomAxisLabel
        /// </summary>
#if SL || WP
        [System.ComponentModel.TypeConverter(typeof(Converters.ObjectConverter))]
#endif
        public Object From
        {
            get
            {
                return GetValue(FromProperty);
            }
            set
            {
                SetValue(FromProperty, value);
            }
        }

        /// <summary>
        /// Set the end position of CustomAxisLabel
        /// </summary>
#if SL || WP
        [System.ComponentModel.TypeConverter(typeof(Converters.ObjectConverter))]
#endif
        public Object To
        {
            get
            {
                return GetValue(ToProperty);
            }
            set
            {
                SetValue(ToProperty, value);
            }
        }

        #endregion

        #region Internal Properties

//        /// <summary>
//        /// Get or set the FontFamily property of title
//        /// </summary>
//        internal FontFamily InternalFontFamily
//        {
//            get
//            {
//                FontFamily retVal;

//                if (_internalFontFamily == null)
//                    retVal = (FontFamily)GetValue(FontFamilyProperty);
//                else
//                    retVal = _internalFontFamily;

//                return (retVal == null) ? new FontFamily("Verdana") : retVal;
//            }
//            set
//            {

//                _internalFontFamily = value;
//            }
//        }

//        /// <summary>
//        /// Get or set the FontSize property of title
//        /// </summary>
//        internal Double InternalFontSize
//        {
//            get
//            {
//                return (Double)(Double.IsNaN(_internalFontSize) ? GetValue(FontSizeProperty) : _internalFontSize);
//            }
//            set
//            {
//                _internalFontSize = value;
//            }
//        }

//        /// <summary>
//        /// Get or set the FontStyle property of title text
//        /// </summary>
//#if WPF
//        [TypeConverter(typeof(System.Windows.FontStyleConverter))]
//#endif
//        internal FontStyle InternalFontStyle
//        {
//            get
//            {
//                return (FontStyle)((_internalFontStyle == null) ? GetValue(FontStyleProperty) : _internalFontStyle);
//            }
//            set
//            {
//                _internalFontStyle = value;
//            }
//        }

//        /// <summary>
//        /// Get or set the FontWeight property of title text
//        /// </summary>
//#if WPF
//         [System.ComponentModel.TypeConverter(typeof(System.Windows.FontWeightConverter))]
//#endif
//        internal FontWeight InternalFontWeight
//        {
//            get
//            {
//                return (FontWeight)((_internalFontWeight == null) ? GetValue(FontWeightProperty) : _internalFontWeight);
//            }
//            set
//            {
//                _internalFontWeight = value;
//            }
//        }

        internal new CustomAxisLabels Parent
        {
            get;
            set;
        }

        internal TextBlock TextElement
        {
            get;
            set;
        }

        internal Path CustomLabelPath
        {
            get;
            set;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Event handler attached with FontColor property changed event of CustomAxisLabel element
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnFontColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CustomAxisLabel customAxisLabel = d as CustomAxisLabel;
            customAxisLabel.UpdateVisual(VcProperties.FontColor, e.NewValue);
        }
        
        /// <summary>
        /// Event handler attached with LineColor property changed event of CustomAxisLabel element
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnLineColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CustomAxisLabel customAxisLabel = d as CustomAxisLabel;
            customAxisLabel.UpdateVisual(VcProperties.LineColor, e.NewValue);
        }

        /// <summary>
        /// Event handler attached with LineThickness property changed event of CustomAxisLabel element
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnLineThicknessPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CustomAxisLabel customAxisLabel = d as CustomAxisLabel;
            customAxisLabel.UpdateVisual(VcProperties.LineThickness, e.NewValue);
        }

        /// <summary>
        /// Event handler attached with Text property changed event of CustomAxisLabel element
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CustomAxisLabel customAxisLabel = d as CustomAxisLabel;
            customAxisLabel.FirePropertyChanged(VcProperties.Text);
        }

        /// <summary>
        /// Event handler attached with To property changed event of CustomAxisLabel element
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnToPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CustomAxisLabel customAxisLabel = d as CustomAxisLabel;
            customAxisLabel.FirePropertyChanged(VcProperties.To);
        }

        /// <summary>
        /// Event handler attached with From property changed event of CustomAxisLabel element
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnFromPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CustomAxisLabel customAxisLabel = d as CustomAxisLabel;
            customAxisLabel.FirePropertyChanged(VcProperties.From);
        }

        private void ApplyCustomLabelProperties()
        {
            if (TextElement != null)
            {
                TextElement.Foreground = Charts.Chart.CalculateFontColor((Chart as Chart), Parent.ParentAxis.Background, FontColor, false);
            }

            if (CustomLabelPath != null)
            {
                CustomLabelPath.Stroke = Charts.Chart.CalculateFontColor((Chart as Chart), Parent.ParentAxis.Background, LineColor, false);
                CustomLabelPath.StrokeThickness = (Double)LineThickness;
            }
        }

        ///// <summary>
        ///// Identifies the Visifire.Charts.CustomAxisLabel.FontSize dependency property.
        ///// </summary>
        ///// <returns>
        ///// The identifier for the Visifire.Charts.CustomAxisLabel.FontSize dependency property.
        ///// </returns>
        //private static readonly DependencyProperty
        //    InternalFontSizeProperty = DependencyProperty.Register
        //    ("InternalFontSize",
        //    typeof(Double),
        //    typeof(CustomAxisLabel),
        //    new PropertyMetadata(OnFontSizePropertyChanged));

        ///// Identifies the Visifire.Charts.CustomAxisLabel.FontFamily dependency property.
        ///// </summary>
        ///// <returns>
        ///// The identifier for the Visifire.Charts.CustomAxisLabel.FontFamily dependency property.
        ///// </returns>
        //private static readonly DependencyProperty InternalFontFamilyProperty = DependencyProperty.Register
        //    ("InternalFontFamily",
        //    typeof(FontFamily),
        //    typeof(CustomAxisLabel),
        //    new PropertyMetadata(OnFontFamilyPropertyChanged));

        ///// <summary>
        ///// Identifies the Visifire.Charts.CustomAxisLabel.FontStyle dependency property.
        ///// </summary>
        ///// <returns>
        ///// The identifier for the Visifire.Charts.CustomAxisLabel.FontStyle dependency property.
        ///// </returns>
        //private static readonly DependencyProperty InternalFontStyleProperty = DependencyProperty.Register
        //    ("InternalFontStyle",
        //    typeof(FontStyle),
        //    typeof(CustomAxisLabel),
        //    new PropertyMetadata(OnFontStylePropertyChanged));

        ///// <summary>
        ///// Identifies the Visifire.Charts.CustomAxisLabel.FontWeight dependency property.
        ///// </summary>
        ///// <returns>
        ///// The identifier for the Visifire.Charts.CustomAxisLabel.FontWeight dependency property.
        ///// </returns>
        //private static readonly DependencyProperty InternalFontWeightProperty = DependencyProperty.Register
        //    ("InternalFontWeight",
        //    typeof(FontWeight),
        //    typeof(CustomAxisLabel),
        //    new PropertyMetadata(OnFontWeightPropertyChanged));

        ///// <summary>
        ///// Event handler attached with FontFamily property changed event of CustomAxisLabels element
        ///// </summary>
        ///// <param name="d">DependencyObject</param>
        ///// <param name="e">DependencyPropertyChangedEventArgs</param>
        //private static void OnFontFamilyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    CustomAxisLabel customAxisLabel = d as CustomAxisLabel;

        //    if (e.NewValue == null || e.OldValue == null)
        //    {
        //        customAxisLabel.InternalFontFamily = (FontFamily)e.NewValue;
        //        customAxisLabel.FirePropertyChanged(VcProperties.FontFamily);
        //    }
        //    else if (e.NewValue.ToString() != e.OldValue.ToString())
        //    {
        //        customAxisLabel.InternalFontFamily = (FontFamily)e.NewValue;
        //        customAxisLabel.FirePropertyChanged(VcProperties.FontFamily);
        //    }
        //}

        ///// <summary>
        ///// Event handler attached with FontStyle property changed event of CustomAxisLabels element
        ///// </summary>
        ///// <param name="d">DependencyObject</param>
        ///// <param name="e">DependencyPropertyChangedEventArgs</param>
        //private static void OnFontStylePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    CustomAxisLabel customAxisLabel = d as CustomAxisLabel;
        //    customAxisLabel.InternalFontStyle = (FontStyle)e.NewValue;
        //    customAxisLabel.UpdateVisual(VcProperties.FontStyle, e.NewValue);
        //}

        ///// <summary>
        ///// Event handler attached with FontWeight property changed event of CustomAxisLabels element
        ///// </summary>
        ///// <param name="d">DependencyObject</param>
        ///// <param name="e">DependencyPropertyChangedEventArgs</param>
        //private static void OnFontWeightPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    CustomAxisLabel customAxisLabel = d as CustomAxisLabel;
        //    customAxisLabel.InternalFontWeight = (FontWeight)e.NewValue;
        //    customAxisLabel.UpdateVisual(VcProperties.FontWeight, e.NewValue);
        //}

        ///// <summary>
        ///// Event handler attached with FontSize property changed event of CustomAxisLabels element
        ///// </summary>
        ///// <param name="d">DependencyObject</param>
        ///// <param name="e">DependencyPropertyChangedEventArgs</param>
        //private static void OnFontSizePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    CustomAxisLabel customAxisLabel = d as CustomAxisLabel;
        //    customAxisLabel.InternalFontSize = (Double)e.NewValue;
        //    customAxisLabel.FirePropertyChanged(VcProperties.FontSize);
        //}

        #endregion

        #region Internal Methods

        /// <summary>
        /// Update visual used for partial update
        /// </summary>
        /// <param name="propertyName">Name of the property</param>
        /// <param name="value">Value of the property</param>
        internal override void UpdateVisual(VcProperties propertyName, object value)
        {
            if (TextElement != null || CustomLabelPath != null)
            {
                ApplyCustomLabelProperties();
            }
            else
                FirePropertyChanged(propertyName);
        }

        #endregion

        #region Data

#if WPF
        /// <summary>
        /// Whether the default style is applied
        /// </summary>
        private static Boolean _defaultStyleKeyApplied;
#endif

       #endregion
    }
}
