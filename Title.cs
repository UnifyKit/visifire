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
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using Visifire.Commons;
using System.ComponentModel;

#else
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Visifire.Commons;
using System.Windows.Data;
#endif

namespace Visifire.Charts
{
    /// <summary>
    /// Visifire.Charts.Title class
    /// </summary>
#if SL
    [System.Windows.Browser.ScriptableType]
#endif
    public class Title : ObservableObject
    {
        #region Public Methods

        /// <summary>
        /// Initializes a new instance of the Visifire.Charts.Title class
        /// </summary>
        public Title()
        {
            SetDefaultStyle();

            //Bind();
        }

        /// <summary>
        /// Initializes a new instance of the Visifire.Charts.Title class
        /// </summary>
        internal Title(String text)
        {
            SetDefaultStyle();
            Text = text;

            //Bind();
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

            b = new Binding("BorderThickness");
            b.Source = this;
            this.SetBinding(InternalBorderThicknessProperty, b);

            b = new Binding("Background");
            b.Source = this;
            this.SetBinding(InternalBackgroundProperty, b);

            b = new Binding("HorizontalAlignment");
            b.Source = this;
            this.SetBinding(InternalHorizontalAlignmentProperty, b);

            b = new Binding("VerticalAlignment");
            b.Source = this;
            this.SetBinding(InternalVerticalAlignmentProperty, b);

            b = new Binding("Margin");
            b.Source = this;
            this.SetBinding(InternalMarginProperty, b);

            b = new Binding("Padding");
            b.Source = this;
            this.SetBinding(InternalPaddingProperty, b);

            b = new Binding("Opacity");
            b.Source = this;
            this.SetBinding(InternalOpacityProperty, b);
#endif
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Identifies the Visifire.Charts.Title.Enabled dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Title.Enabled dependency property.
        /// </returns>
        public static readonly DependencyProperty EnabledProperty = DependencyProperty.Register
            ("Enabled",
            typeof(Nullable<Boolean>),
            typeof(Title),
            new PropertyMetadata(OnEnabledPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.Title.HrefTarget dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Title.HrefTarget dependency property.
        /// </returns>
        public static readonly DependencyProperty HrefTargetProperty = DependencyProperty.Register
            ("HrefTarget",
            typeof(HrefTargets),
            typeof(Title),
            new PropertyMetadata(OnHrefTargetChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.Title.Href dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Title.Href dependency property.
        /// </returns>
        public static readonly DependencyProperty HrefProperty = DependencyProperty.Register
            ("Href",
            typeof(String),
            typeof(Title),
            new PropertyMetadata(OnHrefChanged));

        /// <summary>
        /// FontSizeProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnFontSizePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Title title = d as Title;
            title.InternalFontSize = (Double)e.NewValue;
            title.FirePropertyChanged(VcProperties.FontSize);
        }

        /// <summary>
        /// Identifies the Visifire.Charts.Title.FontColor dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Title.FontColor dependency property.
        /// </returns>
        public static readonly DependencyProperty FontColorProperty = DependencyProperty.Register
            ("FontColor",
            typeof(Brush),
            typeof(Title),
            new PropertyMetadata(OnFontColorPropertyChanged));

#if SL
        /// <summary>
        /// Identifies the Visifire.Charts.Title.FontSize dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Title.FontSize dependency property.
        /// </returns>
        private static readonly DependencyProperty
            InternalFontSizeProperty = DependencyProperty.Register
            ("InternalFontSize",
            typeof(Double),
            typeof(Title),
            new PropertyMetadata(OnFontSizePropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.Title.FontFamily dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Title.FontFamily dependency property.
        /// </returns>
        private static readonly DependencyProperty InternalFontFamilyProperty = DependencyProperty.Register
            ("InternalFontFamily",
            typeof(FontFamily),
            typeof(Title),
            new PropertyMetadata(OnFontFamilyPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.Title.FontStyle dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Title.FontStyle dependency property.
        /// </returns>
        private static readonly DependencyProperty InternalFontStyleProperty = DependencyProperty.Register
            ("InternalFontStyle",
            typeof(FontStyle),
            typeof(Title),
            new PropertyMetadata(OnFontStylePropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.Title.FontWeight dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Title.FontWeight dependency property.
        /// </returns>
        private static readonly DependencyProperty InternalFontWeightProperty = DependencyProperty.Register
            ("InternalFontWeight",
            typeof(FontWeight),
            typeof(Title),
            new PropertyMetadata(OnFontWeightPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.Title.BorderThickness dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Title.BorderThickness dependency property.
        /// </returns>
        private static readonly DependencyProperty InternalBorderThicknessProperty = DependencyProperty.Register
            ("InternalBorderThickness",
            typeof(Thickness),
            typeof(Title),
            new PropertyMetadata(OnBorderThicknessPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.Title.Background dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Title.Background dependency property.
        /// </returns>
        private static readonly DependencyProperty InternalBackgroundProperty = DependencyProperty.Register
            ("InternalBackground",
            typeof(Brush),
            typeof(Title),
            new PropertyMetadata(OnBackgroundPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.Title.HorizontalAlignment dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Title.HorizontalAlignment dependency property.
        /// </returns>
        private static readonly DependencyProperty InternalHorizontalAlignmentProperty = DependencyProperty.Register
            ("InternalHorizontalAlignment",
            typeof(HorizontalAlignment),
            typeof(Title),
            new PropertyMetadata(OnHorizontalAlignmentPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.Title.VerticalAlignment dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Title.VerticalAlignment dependency property.
        /// </returns>
        private static readonly DependencyProperty InternalVerticalAlignmentProperty = DependencyProperty.Register
            ("InternalVerticalAlignment",
            typeof(VerticalAlignment),
            typeof(Title),
            new PropertyMetadata(OnVerticalAlignmentPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.Title.Margin dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Title.Margin dependency property.
        /// </returns>
        private static readonly DependencyProperty InternalMarginProperty = DependencyProperty.Register
            ("InternalMargin",
            typeof(Thickness),
            typeof(Title),
            new PropertyMetadata(OnMarginPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.Title.Padding dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Title.Padding dependency property.
        /// </returns>
        private static readonly DependencyProperty InternalPaddingProperty = DependencyProperty.Register
            ("InternalPadding",
            typeof(Thickness),
            typeof(Title),
            new PropertyMetadata(OnPaddingPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.Title.Opacity dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Title.Opacity dependency property.
        /// </returns>
        private static readonly DependencyProperty InternalOpacityProperty = DependencyProperty.Register
            ("InternalOpacity",
            typeof(Double),
            typeof(Title),
            new PropertyMetadata(1.0, OnOpacityPropertyChanged));
#else

        /// <summary>
        /// Identifies the Visifire.Charts.Title.FontFamily dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Title.FontFamily dependency property.
        /// </returns>
        public new static readonly DependencyProperty FontFamilyProperty = DependencyProperty.Register
            ("FontFamily",
            typeof(FontFamily),
            typeof(Title),
            new PropertyMetadata(OnFontFamilyPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.Title.FontSize dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Title.FontSize dependency property.
        /// </returns>
        public new static readonly DependencyProperty FontSizeProperty = DependencyProperty.Register
            ("FontSize",
            typeof(Double),
            typeof(Title),
            new PropertyMetadata(OnFontSizePropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.Title.FontStyle dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Title.FontStyle dependency property.
        /// </returns>
        public new static readonly DependencyProperty FontStyleProperty = DependencyProperty.Register
            ("FontStyle",
            typeof(FontStyle),
            typeof(Title),
            new PropertyMetadata(OnFontStylePropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.Title.FontWeight dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Title.FontWeight dependency property.
        /// </returns>
        public new static readonly DependencyProperty FontWeightProperty = DependencyProperty.Register
            ("FontWeight",
            typeof(FontWeight),
            typeof(Title),
            new PropertyMetadata(OnFontWeightPropertyChanged));
#endif

        /// <summary>
        /// Identifies the Visifire.Charts.Title.Text dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Title.Text dependency property.
        /// </returns>
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register
            ("Text",
            typeof(String),
            typeof(Title),
            new PropertyMetadata(OnTextPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.Title.BorderColor dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Title.BorderColor dependency property.
        /// </returns>
        public static readonly DependencyProperty BorderColorProperty = DependencyProperty.Register
            ("BorderColor",
            typeof(Brush),
            typeof(Title),
            new PropertyMetadata(OnBorderColorPropertyChanged));

#if WPF
        
        /// <summary>
        /// Identifies the Visifire.Charts.Title.BorderThickness dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Title.BorderThickness dependency property.
        /// </returns>
        public new static readonly DependencyProperty BorderThicknessProperty = DependencyProperty.Register
            ("BorderThickness",
            typeof(Thickness),
            typeof(Title),
            new PropertyMetadata(OnBorderThicknessPropertyChanged));
#endif

        /// <summary>
        /// Identifies the Visifire.Charts.Title.CornerRadius dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Title.CornerRadius dependency property.
        /// </returns>
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register
                  ("CornerRadius",
                  typeof(CornerRadius),
                  typeof(Title),
                  new PropertyMetadata(OnCornerRadiusPropertyChanged));

#if WPF
        /// <summary>
        /// Identifies the Visifire.Charts.Title.Background dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Title.Background dependency property.
        /// </returns>
        public new static readonly DependencyProperty BackgroundProperty = DependencyProperty.Register
            ("Background",
            typeof(Brush),
            typeof(Title),
            new PropertyMetadata(OnBackgroundPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.Title.HorizontalAlignment dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Title.HorizontalAlignment dependency property.
        /// </returns>
        public new static readonly DependencyProperty HorizontalAlignmentProperty = DependencyProperty.Register
            ("HorizontalAlignment",
            typeof(HorizontalAlignment),
            typeof(Title),
            new PropertyMetadata(OnHorizontalAlignmentPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.Title.VerticalAlignment dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Title.VerticalAlignment dependency property.
        /// </returns>
        public new static readonly DependencyProperty VerticalAlignmentProperty = DependencyProperty.Register
            ("VerticalAlignment",
            typeof(VerticalAlignment),
            typeof(Title),
            new PropertyMetadata(OnVerticalAlignmentPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.Title.Margin dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Title.Margin dependency property.
        /// </returns>
        public new static readonly DependencyProperty MarginProperty = DependencyProperty.Register
            ("Margin",
            typeof(Thickness),
            typeof(Title),
            new PropertyMetadata(OnMarginPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.Title.Padding dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Title.Padding dependency property.
        /// </returns>
        public new static readonly DependencyProperty PaddingProperty = DependencyProperty.Register
            ("Padding",
            typeof(Thickness),
            typeof(Title),
            new PropertyMetadata(OnPaddingPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.Title.Opacity dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Title.Opacity dependency property.
        /// </returns>
        public new static readonly DependencyProperty OpacityProperty = DependencyProperty.Register
            ("Opacity",
            typeof(Double),
            typeof(Title),
            new PropertyMetadata(1.0, OnOpacityPropertyChanged));
#endif

        /// <summary>
        /// Identifies the Visifire.Charts.Title.TextAlignment dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Title.TextAlignment dependency property.
        /// </returns>
        public static readonly DependencyProperty TextAlignmentProperty = DependencyProperty.Register
            ("TextAlignment",
            typeof(TextAlignment),
            typeof(Title),
            new PropertyMetadata(OnTextAlignmentPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.Title.DockInsidePlotArea dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Title.DockInsidePlotArea dependency property.
        /// </returns>
        public static readonly DependencyProperty DockInsidePlotAreaProperty = DependencyProperty.Register
            ("DockInsidePlotArea",
            typeof(Boolean),
            typeof(Title),
            new PropertyMetadata(OnDockInsidePlotAreaPropertyChanged));

        /// <summary>
        /// Get or set the Enabled property of title
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
        /// Get or set the HrefTarget property of title
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
        /// Get or set the Href property of title
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
                    InternalOpacity = value;
                    SetValue(OpacityProperty, value);
                    FirePropertyChanged(VcProperties.Opacity);
                }
#else
                SetValue(OpacityProperty, value);
#endif
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

        Double _internalOpacity = Double.NaN;

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

        #region Font Properties

        /// <summary>
        /// Get or set the FontFamily property of title
        /// </summary>
        public new FontFamily FontFamily
        {
            get
            {
                FontFamily retVal;
                if (_internalFontFamily == null)
                    retVal = (FontFamily)GetValue(FontFamilyProperty);
                else
                    retVal = _internalFontFamily;

                return (retVal == null) ? new FontFamily("Verdana") : (FontFamily)GetValue(FontFamilyProperty);
            }
            set
            {
#if SL
                if (InternalFontFamily != value)
                {
                    _internalFontFamily = value;
                    SetValue(FontFamilyProperty, value);
                    FirePropertyChanged(VcProperties.FontFamily);
                }
#else           
                SetValue(FontFamilyProperty, value);
#endif
            }
        }

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

        ///<summary>
        ///Get or set the FontSize property of title
        ///</summary>
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
                    FirePropertyChanged(VcProperties.FontSize);
                }
#else           
                SetValue(FontSizeProperty, value);
#endif
            }
        }

        /// <summary>
        /// Get or set the FontSize property of title
        /// </summary>
        internal Double InternalFontSize
        {
            get
            {
                if (Double.IsNaN(_internalFontSize))
                    return ((Double)GetValue(FontSizeProperty) == 0) ? 10 : (Double)GetValue(FontSizeProperty);
                else
                    return (_internalFontSize == 0) ? 10 : _internalFontSize;
            }
            set
            {
                _internalFontSize = value;
            }
        }

        private Double _internalFontSize = Double.NaN;
        private FontFamily _internalFontFamily = null;

        /// <summary>
        /// Get or set the FontColor property of title text
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

        internal Brush InternalFontColor;

        /// <summary>
        /// Get or set the FontStyle property of title text
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
                    _internalFontStyle = value;
                    SetValue(FontStyleProperty, value);
                    UpdateVisual(VcProperties.FontStyle, value);
                }
#else           
                SetValue(FontStyleProperty, value);
#endif
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

        Nullable<FontStyle> _internalFontStyle = null;


        /// <summary>
        /// Get or set the FontWeight property of title text
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
                    _internalFontWeight = value;
                    SetValue(FontWeightProperty, value);
                    UpdateVisual(VcProperties.FontWeight, value);
                }
#else
                SetValue(FontWeightProperty, value);
#endif
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

        Nullable<FontWeight> _internalFontWeight = null;

        /// <summary>
        /// Get or set the Text property of title
        /// </summary>
        public String Text
        {
            get
            {
                return (GetValue(TextProperty) == null) ? "" : (String)GetValue(TextProperty);
            }
            set
            {
                SetValue(TextProperty, value);
            }
        }

        #endregion

        #region Border Properties

        /// <summary>
        /// Get or set the BorderColor property of title
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
        /// Get or set the BorderThickness of title
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
                    _borderThickness = value;
                    SetValue(BorderThicknessProperty, value);
                    FirePropertyChanged(VcProperties.BorderThickness);
                }
#else           
                SetValue(BorderThicknessProperty, value);
#endif
            }
        }

        /// <summary>
        /// Get or set the BorderThickness of title
        /// </summary>
        internal Thickness InternalBorderThickness
        {
            get
            {
                return (Thickness)((_borderThickness == null) ? GetValue(BorderThicknessProperty) : _borderThickness);
            }
            set
            {
                _borderThickness = value;
            }
        }

        Nullable<Thickness> _borderThickness = null;

        /// <summary>
        /// Get or set the CornerRadius property of title
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

        #endregion

        /// <summary>
        /// Get or set the Background property of title
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
                    _internalBackground = value;
                    SetValue(BackgroundProperty, value);
                    UpdateVisual(VcProperties.Background, value);
                    // FirePropertyChanged("Background");
                }
#else           
                SetValue(BackgroundProperty, value);
#endif
            }
        }

        /// <summary>
        /// Get or set the Background property of title
        /// </summary>
        internal Brush InternalBackground
        {
            get
            {
                return (Brush)((_internalBackground == null) ? GetValue(BackgroundProperty) : _internalBackground);
            }
            set
            {
                _internalBackground = value;
            }
        }

        Brush _internalBackground = null;

        #region Alignment

        /// <summary>
        /// Get or set the HorizontalAlignment property of title
        /// </summary>
        public new HorizontalAlignment HorizontalAlignment
        {
            get
            {
                return (HorizontalAlignment)GetValue(HorizontalAlignmentProperty);
            }
            set
            {
#if SL
                if (HorizontalAlignment != value)
                {
                    InternalHorizontalAlignment = value;
                    SetValue(HorizontalAlignmentProperty, value);
                    FirePropertyChanged(VcProperties.HorizontalAlignment);
                }
#else           
                SetValue(HorizontalAlignmentProperty, value);
#endif
            }
        }

        /// <summary>
        /// Get or set the HorizontalAlignment property of title
        /// </summary>
        internal HorizontalAlignment InternalHorizontalAlignment
        {
            get
            {
                return (HorizontalAlignment)((_internalHorizontalAlignment == null) ? GetValue(HorizontalAlignmentProperty) : _internalHorizontalAlignment);
            }
            set
            {
                _internalHorizontalAlignment = value;
            }
        }

        Nullable<HorizontalAlignment> _internalHorizontalAlignment = null;

        /// <summary>
        /// Get or set the VerticalAlignment property of title
        /// </summary>
        public new VerticalAlignment VerticalAlignment
        {
            get
            {
                return (VerticalAlignment)GetValue(VerticalAlignmentProperty);
            }
            set
            {
#if SL
                if (VerticalAlignment != value)
                {
                    InternalVerticalAlignment = value;
                    SetValue(VerticalAlignmentProperty, value);
                    FirePropertyChanged(VcProperties.VerticalAlignment);
                }
#else
                 SetValue(VerticalAlignmentProperty, value);
#endif
            }
        }

        /// <summary>
        /// Get or set the VerticalAlignment property of title
        /// </summary>
        internal VerticalAlignment InternalVerticalAlignment
        {
            get
            {
                return (VerticalAlignment)((_internalVerticalAlignment == null) ? GetValue(VerticalAlignmentProperty) : _internalVerticalAlignment);
            }
            set
            {
                _internalVerticalAlignment = value;
            }
        }

        Nullable<VerticalAlignment> _internalVerticalAlignment = null;

        #endregion


        /// <summary>
        /// Get or set the Margin property of title
        /// </summary>
        public new Thickness Margin
        {
            get
            {
                return (Thickness)GetValue(MarginProperty);
            }
            set
            {
#if SL
                if (Margin != value)
                {
                    InternalMargin = value;
                    SetValue(MarginProperty, value);
                    FirePropertyChanged(VcProperties.Margin);
                }
#else
                SetValue(MarginProperty, value);
#endif
            }
        }

        /// <summary>
        /// Get or set the Margin property of title
        /// </summary>
        internal Thickness InternalMargin
        {
            get
            {
                return (Thickness)((_internalMargin == null) ? GetValue(MarginProperty) : _internalMargin);
            }
            set
            {
                _internalMargin = value;
            }
        }

        Nullable<Thickness> _internalMargin = null;

        /// <summary>
        /// Get or set the Padding property of title
        /// </summary>
        public new Thickness Padding
        {
            get
            {
                return (Thickness)GetValue(PaddingProperty);
            }
            set
            {
#if SL
                if (Padding != value)
                {
                    InternalPadding = value;
                    SetValue(PaddingProperty, value);
                    FirePropertyChanged(VcProperties.Padding);
                }
#else
                SetValue(PaddingProperty, value);
#endif
            }
        }

        /// <summary>
        /// Get or set the Padding property of title
        /// </summary>
        public Thickness InternalPadding
        {
            get
            {
                return (Thickness)((_internalPadding == null) ? GetValue(PaddingProperty) : _internalPadding);
            }
            set
            {
                _internalPadding = value;
            }
        }

        Nullable<Thickness> _internalPadding = null;

        /// <summary>
        /// Get or set the Property property TextAlignment
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
        /// Get or set the DockInsidePlotArea property of title
        /// (Whether the title will be docked inside PlotArea)
        /// </summary>
        public Boolean DockInsidePlotArea
        {
            get
            {
                return (Boolean)GetValue(DockInsidePlotAreaProperty);
            }
            set
            {
                SetValue(DockInsidePlotAreaProperty, value);
            }
        }


        #endregion

        #region Public Events

        #endregion

        #region Protected Methods

        #endregion

        #region Internal Properties

        /// <summary>
        /// Visual is the Framework object to be used to render the title
        /// </summary>
        internal Border Visual
        {
            get;
#if DEBUG
            set;
#else
            private set;
#endif
        }

        /// <summary>
        /// TextBlock desired size
        /// </summary>
        internal Size TextBlockDesiredSize
        {
            get;
            set;
        }

        #endregion

        #region Private Delegates

        #endregion

        #region Private Properties

        #region Hidden Properties

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        private new Brush BorderBrush
        {
            get;
            set;
        }

        private new Brush Foreground
        {
            get;
            set;
        }

        #endregion

        /// <summary>
        /// Canvas inside Title Visual
        /// </summary>
        private Canvas InnerCanvas
        {
            get;
            set;
        }

        /// <summary>
        /// Text element of the title to display text content
        /// </summary>
        internal TextBlock TextElement
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
            Title title = d as Title;
            title.FirePropertyChanged(VcProperties.Enabled);
        }

        /// <summary>
        /// HrefTargetProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnHrefTargetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Title title = d as Title;
            title.FirePropertyChanged(VcProperties.HrefTarget);
        }

        /// <summary>
        /// HrefProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnHrefChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Title title = d as Title;
            title.FirePropertyChanged(VcProperties.Href);
        }


        /// <summary>
        /// FontFamilyProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnFontFamilyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Title title = d as Title;

            if (e.NewValue == null || e.OldValue == null)
            {
                title.InternalFontFamily = (FontFamily)e.NewValue;
                title.FirePropertyChanged(VcProperties.FontFamily);
            }
            else if (e.NewValue.ToString() != e.OldValue.ToString())
            {
                title.InternalFontFamily = (FontFamily)e.NewValue;
                title.FirePropertyChanged(VcProperties.FontFamily);
            }
        }

        /// <summary>
        /// OpacityProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnOpacityPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Title title = d as Title;
            title.InternalOpacity = (Double)e.NewValue;
            title.FirePropertyChanged(VcProperties.Opacity);
        }

        /// <summary>
        /// FontColorProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnFontColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Title title = d as Title;
            title.InternalFontColor = (Brush)e.NewValue;
            title.UpdateVisual(VcProperties.FontColor, e.NewValue);
        }

        /// <summary>
        /// FontStyleProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnFontStylePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Title title = d as Title;
            title.InternalFontStyle = (FontStyle)e.NewValue;
            title.UpdateVisual(VcProperties.FontStyle, e.NewValue);
        }

        /// <summary>
        /// FontWeightProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnFontWeightPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Title title = d as Title;
            title.InternalFontWeight = (FontWeight)e.NewValue;
            title.UpdateVisual(VcProperties.FontWeight, e.NewValue);
        }

#if WPF
        

#endif

        /// <summary>
        /// TextProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Title title = d as Title;
            title.FirePropertyChanged(VcProperties.Text);
        }

        /// <summary>
        /// BorderColorProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnBorderColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Title title = d as Title;
            // title.FirePropertyChanged("BorderColor");
            title.UpdateVisual(VcProperties.BorderColor, e.NewValue);
        }

        /// <summary>
        /// BorderThicknessProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnBorderThicknessPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Title title = d as Title;
            title.InternalBorderThickness = (Thickness)e.NewValue;
            title.FirePropertyChanged(VcProperties.BorderThickness);
        }

        /// <summary>
        /// CornerRadiusProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnCornerRadiusPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Title title = d as Title;
            title.FirePropertyChanged(VcProperties.CornerRadius);
        }

        /// <summary>
        /// BackgroundProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnBackgroundPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Title title = d as Title;
            title._internalBackground = (Brush)e.NewValue;
            title.UpdateVisual(VcProperties.Background, e.NewValue);
        }

        /// <summary>
        /// HorizontalAlignmentProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnHorizontalAlignmentPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Title title = d as Title;
            title.InternalHorizontalAlignment = (HorizontalAlignment)e.NewValue;
            title.FirePropertyChanged(VcProperties.HorizontalAlignment);
        }
        /// <summary>
        /// VerticalAlignmentProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnVerticalAlignmentPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Title title = d as Title;
            title.InternalVerticalAlignment = (VerticalAlignment)e.NewValue;
            title.FirePropertyChanged(VcProperties.VerticalAlignment);
        }

        /// <summary>
        /// MarginProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnMarginPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Title title = d as Title;
            title.InternalMargin = (Thickness)e.NewValue;
            title.FirePropertyChanged(VcProperties.Margin);
        }
        /// <summary>
        /// PaddingProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnPaddingPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Title title = d as Title;
            title.InternalPadding = (Thickness)e.NewValue;
            title.FirePropertyChanged(VcProperties.Padding);
        }

        /// <summary>
        /// TextAlignmentProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnTextAlignmentPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Title title = d as Title;
            title.FirePropertyChanged(VcProperties.TextAlignment);
        }

        /// <summary>
        /// DockInsidePlotAreaProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnDockInsidePlotAreaPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Title title = d as Title;
            title.FirePropertyChanged(VcProperties.DockInsidePlotArea);
        }

        /// <summary>
        /// Set default style for title
        /// </summary>
        private void SetDefaultStyle()
        {
            // Apply default style from generic
#if WPF
            if (!_defaultStyleKeyApplied)
            {
                DefaultStyleKeyProperty.OverrideMetadata(typeof(Title), new FrameworkPropertyMetadata(typeof(Title)));
                _defaultStyleKeyApplied = true;
               
            }
#else
            DefaultStyleKey = typeof(Title);
#endif
        }



        /// <summary>
        /// Apply all style properties of the Title
        /// </summary>
        /// <param name="title">Title</param>
        /// <returns>Boolean</returns>
        private static Boolean ApplyProperties(Title title)
        {
            if (title.Visual != null)
            {
                // Set TextElement properties 
#if WPF
                title.Visual.FlowDirection = FlowDirection.LeftToRight;
#endif
                title.TextElement.FontFamily = title.InternalFontFamily;

                title.TextElement.FontSize = title.InternalFontSize;

                //Binding fbinding = new Binding("FontSize");
                //fbinding.Source = title;
                //fbinding.Mode = BindingMode.TwoWay;
                //title.TextElement.SetBinding(TextBlock.FontSizeProperty, fbinding);

                title.TextElement.FontStyle = title.InternalFontStyle;
                title.TextElement.FontWeight = title.InternalFontWeight;
                title.TextElement.Text = GetFormattedMultilineText(title.Text);
                title.TextElement.Foreground = Charts.Chart.CalculateFontColor((title.Chart as Chart), title.InternalFontColor, title.DockInsidePlotArea);
                title.TextElement.TextWrapping = TextWrapping.Wrap;

                // Set Border Properties 
                title.Visual.BorderBrush = title.BorderColor;

                //Binding binding = new Binding("Background");
                //binding.Source = title;
                //binding.Mode = BindingMode.TwoWay;
                //title.Visual.SetBinding(Border.BackgroundProperty, binding);

                title.Visual.Background = title.InternalBackground;

                title.Visual.VerticalAlignment = title.InternalVerticalAlignment;
                title.Visual.HorizontalAlignment = title.InternalHorizontalAlignment;
                title.Visual.BorderThickness = title.InternalBorderThickness;
                title.Visual.Margin = title.InternalMargin;
                title.Visual.Padding = title.InternalPadding;
                title.Visual.CornerRadius = title.CornerRadius;
                title.Visual.Cursor = (title.Cursor == null) ? Cursors.Arrow : title.Cursor;
                title.Visual.SetValue(Canvas.ZIndexProperty, title.GetValue(Canvas.ZIndexProperty));

                title.AttachToolTip(title.Chart, title, title.TextElement);
                title.AttachHref(title.Chart, title.TextElement, title.Href, title.HrefTarget);

                return true;
            }
            else
                return false;
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// UpdateVisual is used for partial rendering
        /// </summary>
        /// <param name="propertyName">Name of the property</param>
        /// <param name="value">Value of the property</param>
        internal override void UpdateVisual(VcProperties propertyName, Object value)
        {
            if (propertyName == VcProperties.Background)
            {
                if (Visual != null)
                {
                    Binding binding = new Binding("Background");
                    binding.Source = this;
                    binding.Mode = BindingMode.TwoWay;
                    Visual.SetBinding(Border.BackgroundProperty, binding);
                }
                else
                    this.FirePropertyChanged(propertyName);
            }
            else if (propertyName == VcProperties.BorderColor)
            {
                if (Visual != null)
                {
                    Visual.BorderBrush = (Brush)value;
                    // Binding binding = new Binding("Background");
                    // binding.Source = this;
                    // binding.Mode = BindingMode.TwoWay;
                    // Visual.SetBinding(Border.BackgroundProperty, binding);
                }
                else
                    this.FirePropertyChanged(propertyName);
            }
            else if (!ApplyProperties(this))
                this.FirePropertyChanged(propertyName);
        }

        /// <summary>
        /// Set TextAlignment for Titles present in Left and Right of the ChartArea
        /// </summary>
        internal void SetTextAlignment4LeftAndRight()
        {
            switch (TextAlignment)
            {
                case TextAlignment.Left: InnerCanvas.VerticalAlignment = VerticalAlignment.Bottom; break;
                case TextAlignment.Center: InnerCanvas.VerticalAlignment = VerticalAlignment.Center; break;
                case TextAlignment.Right: InnerCanvas.VerticalAlignment = VerticalAlignment.Top; break;
            }
        }

        /// <summary>
        /// Set TextAlignment for Titles present in Top and Bottom of the ChartArea
        /// </summary>
        internal void SetTextAlignment4TopAndBottom()
        {
            switch (TextAlignment)
            {
                case TextAlignment.Left: InnerCanvas.HorizontalAlignment = HorizontalAlignment.Left; break;
                case TextAlignment.Center: InnerCanvas.HorizontalAlignment = HorizontalAlignment.Center; break;
                case TextAlignment.Right: InnerCanvas.HorizontalAlignment = HorizontalAlignment.Right; break;
            }
        }

        /// <summary>
        /// Creates the Title visual object
        /// </summary>
        internal void CreateVisualObject(ElementData tag)
        {
            if (!(Boolean)Enabled)
                return;

            // Creating Title Visual Object
            Visual = new Border();
            TextElement = new TextBlock() { Tag = tag };
            InnerCanvas = new Canvas() { Tag = tag };
            InnerCanvas.Children.Add(TextElement);
            Visual.Child = InnerCanvas;
            Visual.Opacity = this.InternalOpacity;

            // Set Properties
            ApplyProperties(this);

#if WPF
            TextElement.Measure(new Size(Double.MaxValue, Double.MaxValue));
            TextBlockDesiredSize = new Size(TextElement.DesiredSize.Width, TextElement.DesiredSize.Height);
#else
            TextBlockDesiredSize = new Size(TextElement.ActualWidth, TextElement.ActualHeight);
#endif

            // Set TextElement position inside Title Visual
            if (InternalVerticalAlignment == VerticalAlignment.Center || InternalVerticalAlignment == VerticalAlignment.Stretch)
            {
                if (InternalHorizontalAlignment == HorizontalAlignment.Left || InternalHorizontalAlignment == HorizontalAlignment.Right)
                {
                    RotateTransform rt = new RotateTransform();

                    rt.Angle = 270;
                    TextElement.RenderTransformOrigin = new Point(0, 0);
                    TextElement.RenderTransform = rt;

                    InnerCanvas.Height = TextBlockDesiredSize.Width;
                    InnerCanvas.Width = TextBlockDesiredSize.Height;

                    TextElement.SetValue(Canvas.LeftProperty, (Double)0);
                    TextElement.SetValue(Canvas.TopProperty, (Double)TextBlockDesiredSize.Width);

                    SetTextAlignment4LeftAndRight();
                }
                else
                {
                    InnerCanvas.Height = TextBlockDesiredSize.Height;
                    InnerCanvas.Width = TextBlockDesiredSize.Width;
                    SetTextAlignment4TopAndBottom();
                }
            }
            else
            {
                InnerCanvas.Height = TextBlockDesiredSize.Height;
                InnerCanvas.Width = TextBlockDesiredSize.Width;
            }

            this.Height = InnerCanvas.Height;
            this.Width = InnerCanvas.Width;


            SetTextAlignment4TopAndBottom();

            // Attach event to title visual object 
            AttachEvents2Visual(this, TextElement);
        }

        #endregion

        #region Internal Events

        #endregion

        #region Data

#if WPF

        /// <summary>
        /// Whether the default style is applied
        /// </summary>
        private static Boolean _defaultStyleKeyApplied;
#endif
        #endregion Data
    }
}