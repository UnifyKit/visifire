#if WPF

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Input;
using System.ComponentModel;
using System.Windows.Shapes;

#else

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Input;
using System.Collections.Generic;
using System.Windows.Shapes;

#endif

using Visifire.Commons;

namespace Visifire.Charts
{
    /// <summary>
    /// Legend of chart
    /// </summary>
#if SL
    [System.Windows.Browser.ScriptableType]
#endif
    public class Legend : ObservableObject
    {
        #region Public Methods

        /// <summary>
        /// Initializes a new instance of the Visifire.Charts.Legend class
        /// </summary>
        public Legend()
        {
            // Apply default style from generic
#if WPF
            if (!_defaultStyleKeyApplied)
            {
                DefaultStyleKeyProperty.OverrideMetadata(typeof(Legend), new FrameworkPropertyMetadata(typeof(Legend)));
                _defaultStyleKeyApplied = true;
            } 
#else       
            DefaultStyleKey = typeof(Legend);
#endif      

            Entries = new List<KeyValuePair<String, Marker>>();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Identifies the Visifire.Charts.Legend.HrefTarget dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Legend.HrefTarget dependency property.
        /// </returns>
        public static readonly DependencyProperty HrefTargetProperty = DependencyProperty.Register
            ("HrefTarget",
            typeof(HrefTargets),
            typeof(Legend),
            new PropertyMetadata(OnHrefTargetChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.Legend.Href dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Legend.Href dependency property.
        /// </returns>
        public static readonly DependencyProperty HrefProperty = DependencyProperty.Register
            ("Href",
            typeof(String),
            typeof(Legend),
            new PropertyMetadata(OnHrefChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.Legend.LabelMargin dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Legend.LabelMargin dependency property.
        /// </returns>
        public static readonly DependencyProperty LabelMarginProperty = DependencyProperty.Register
            ("LabelMargin",
            typeof(Double),
            typeof(Legend),
            new PropertyMetadata(OnLabelMarginPropertyChanged));

#if WPF
        /// <summary>
        /// Identifies the Visifire.Charts.Legend.Padding dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Legend.Padding dependency property.
        /// </returns>
        public new static readonly DependencyProperty PaddingProperty = DependencyProperty.Register
             ("Padding",
             typeof(Thickness),
             typeof(Legend),
             new PropertyMetadata(OnPaddingPropertyChanged));
        
        /// <summary>
        /// Identifies the Visifire.Charts.Legend.HorizontalAlignment dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Legend.HorizontalAlignment dependency property.
        /// </returns>
        public new static readonly DependencyProperty HorizontalAlignmentProperty = DependencyProperty.Register
            ("HorizontalAlignment",
            typeof(HorizontalAlignment),
            typeof(Legend),
            new PropertyMetadata(OnHorizontalAlignmentPropertyChanged));
        
        /// <summary>
        /// Identifies the Visifire.Charts.Legend.VerticalAlignment dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Legend.VerticalAlignment dependency property.
        /// </returns>
        public new static readonly DependencyProperty VerticalAlignmentProperty = DependencyProperty.Register
            ("VerticalAlignment",
            typeof(VerticalAlignment),
            typeof(Legend),
            new PropertyMetadata(OnVerticalAlignmentPropertyChanged));
#endif

        /// <summary>
        /// Identifies the Visifire.Charts.Legend.BorderColor dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Legend.BorderColor dependency property.
        /// </returns>
        public static readonly DependencyProperty BorderColorProperty = DependencyProperty.Register
            ("BorderColor",
             typeof(Brush),
             typeof(Legend),
             new PropertyMetadata(OnBorderColorPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.Legend.TitleFontColor dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Legend.TitleFontColor dependency property.
        /// </returns>
        public static readonly DependencyProperty TitleFontColorProperty = DependencyProperty.Register
            ("TitleFontColor",
            typeof(Brush),
            typeof(Legend),
            new PropertyMetadata(OnTitleFontColorPropertyChanged));

#if WPF
        
        /// <summary>
        /// Identifies the Visifire.Charts.Legend.BorderThickness dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Legend.BorderThickness dependency property.
        /// </returns>
        public new static readonly DependencyProperty BorderThicknessProperty = DependencyProperty.Register
            ("BorderThickness",
            typeof(Thickness),
            typeof(Legend),
            new PropertyMetadata(OnBorderThicknessPropertyChanged));
        
        /// <summary>
        /// Identifies the Visifire.Charts.Legend.Background dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Legend.Background dependency property.
        /// </returns>
        public new static readonly DependencyProperty BackgroundProperty = DependencyProperty.Register
            ("Background",
            typeof(Brush),
            typeof(Legend),
            new PropertyMetadata(OnBackgroundPropertyChanged));
#endif

        /// <summary>
        /// Identifies the Visifire.Charts.Legend.DockInsidePlotArea dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Legend.DockInsidePlotArea dependency property.
        /// </returns>
        public static readonly DependencyProperty DockInsidePlotAreaProperty = DependencyProperty.Register
            ("DockInsidePlotArea",
            typeof(Boolean),
            typeof(Legend),
            new PropertyMetadata(OnDockInsidePlotAreaPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.Legend.Enabled dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Legend.Enabled dependency property.
        /// </returns>
        public static readonly DependencyProperty EnabledProperty = DependencyProperty.Register
            ("Enabled",
            typeof(Nullable<Boolean>),
            typeof(Legend),
            new PropertyMetadata(OnEnabledPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.Legend.FontColor dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Legend.FontColor dependency property.
        /// </returns>
        public static readonly DependencyProperty FontColorProperty = DependencyProperty.Register
            ("FontColor",
            typeof(Brush),
            typeof(Legend),
            new PropertyMetadata(OnFontColorPropertyChanged));

#if WPF
        
        /// <summary>
        /// Identifies the Visifire.Charts.Legend.FontFamily dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Legend.FontFamily dependency property.
        /// </returns>
        public new static readonly DependencyProperty FontFamilyProperty = DependencyProperty.Register 
            ("FontFamily",
            typeof(FontFamily),
            typeof(Legend),
            new PropertyMetadata(OnFontFamilyPropertyChanged));
        
        /// <summary>
        /// Identifies the Visifire.Charts.Legend.FontSize dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Legend.FontSize dependency property.
        /// </returns>
        public new static readonly DependencyProperty FontSizeProperty = DependencyProperty.Register 
            ("FontSize",
            typeof(Double),
            typeof(Legend),
            new PropertyMetadata(OnFontSizePropertyChanged));
        
        /// <summary>
        /// Identifies the Visifire.Charts.Legend.FontStyle dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Legend.FontStyle dependency property.
        /// </returns>
        public new static readonly DependencyProperty FontStyleProperty = DependencyProperty.Register 
            ("FontStyle",
            typeof(FontStyle),
            typeof(Legend),
            new PropertyMetadata(OnFontStylePropertyChanged));
        
        /// <summary>
        /// Identifies the Visifire.Charts.Legend.FontWeight dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Legend.FontWeight dependency property.
        /// </returns>
        public new static readonly DependencyProperty FontWeightProperty = DependencyProperty.Register
            ("FontWeight",
            typeof(FontWeight),
            typeof(Legend),
            new PropertyMetadata(OnFontWeightPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.Legend.Opacity dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Legend.Opacity dependency property.
        /// </returns>
        public new static readonly DependencyProperty OpacityProperty = DependencyProperty.Register
            ("Opacity",
            typeof(Double),
            typeof(Legend),
            new PropertyMetadata(1.0, OnOpacityPropertyChanged));

#endif

        /// <summary>
        /// Identifies the Visifire.Charts.Legend.LightingEnabled dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Legend.LightingEnabled dependency property.
        /// </returns>
        public static readonly DependencyProperty LightingEnabledProperty = DependencyProperty.Register
            ("LightingEnabled",
            typeof(Boolean),
            typeof(Legend),
            new PropertyMetadata(OnLightingEnabledPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.Legend.CornerRadius dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Legend.CornerRadius dependency property.
        /// </returns>
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register
            ("CornerRadius",
            typeof(CornerRadius),
            typeof(Legend),
            new PropertyMetadata(new CornerRadius(1), OnCornerRadiusPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.Legend.Title dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Legend.Title dependency property.
        /// </returns>
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register
            ("Title",
            typeof(String),
            typeof(Legend),
            new PropertyMetadata(OnTitlePropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.Legend.TitleAlignmentX dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Legend.TitleAlignmentX dependency property.
        /// </returns>
        public static readonly DependencyProperty TitleAlignmentXProperty = DependencyProperty.Register
            ("TitleAlignmentX",
            typeof(HorizontalAlignment),
            typeof(Legend),
            new PropertyMetadata(OnTitleAlignmentXPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.Legend.TitleTextAlignment dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Legend.TitleTextAlignment dependency property.
        /// </returns>
        public static readonly DependencyProperty TitleTextAlignmentProperty = DependencyProperty.Register
            ("TitleTextAlignment",
            typeof(TextAlignment),
            typeof(Legend),
            new PropertyMetadata(OnTitleTextAlignmentPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.Legend.TitleBackground dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Legend.TitleBackground dependency property.
        /// </returns>
        public static readonly DependencyProperty TitleBackgroundProperty = DependencyProperty.Register
            ("TitleBackground",
            typeof(Brush),
            typeof(Legend),
            new PropertyMetadata(OnTitleBackgroundPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.Legend.TitleFontFamily dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Legend.TitleFontFamily dependency property.
        /// </returns>
        public static readonly DependencyProperty TitleFontFamilyProperty = DependencyProperty.Register
            ("TitleFontFamily",
            typeof(FontFamily),
            typeof(Legend),
            new PropertyMetadata(new FontFamily("Arial"), OnTitleFontFamilyPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.Legend.TitleFontSize dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Legend.TitleFontSize dependency property.
        /// </returns>
        public static readonly DependencyProperty TitleFontSizeProperty = DependencyProperty.Register
            ("TitleFontSize",
            typeof(Double),
            typeof(Legend),
            new PropertyMetadata(OnTitleFontSizePropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.Legend.TitleFontStyle dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Legend.TitleFontStyle dependency property.
        /// </returns>
        public static readonly DependencyProperty TitleFontStyleProperty = DependencyProperty.Register
            ("TitleFontStyle",
            typeof(FontStyle),
            typeof(Legend),
            new PropertyMetadata(OnTitleFontStylePropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.Legend.TitleFontWeight dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Legend.TitleFontWeight dependency property.
        /// </returns>
        public static readonly DependencyProperty TitleFontWeightProperty = DependencyProperty.Register
            ("TitleFontWeight",
            typeof(FontWeight),
            typeof(Legend),
            new PropertyMetadata(OnTitleFontWeightPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.Legend.EntryMargin dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Legend.EntryMargin dependency property.
        /// </returns>
        public static readonly DependencyProperty EntryMarginProperty = DependencyProperty.Register
           ("EntryMargin",
           typeof(Double),
           typeof(Legend),
           new PropertyMetadata(OnEntryMarginPropertyPropertyChanged));

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
        /// Get or set the Href porperty
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
        /// Get or set the label margin of the Legend
        /// </summary>
        public Double LabelMargin
        {
            get
            {
                return (Double)GetValue(LabelMarginProperty);
            }
            set
            {
                SetValue(LabelMarginProperty, value);
            }
        }

        /// <summary>
        /// Get or set the padding of the Legend
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
                FirePropertyChanged("Padding");
#endif
            }
        }

        /// <summary>
        /// Get or set the HorizontalAlignment property of the Legend
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
                    SetValue(HorizontalAlignmentProperty, value);
                    FirePropertyChanged("HorizontalAlignment");
                }
#else
                SetValue(HorizontalAlignmentProperty, value);

#endif
            }
        }

        /// <summary>
        /// Get or set the VerticalAlignment property of the Legend
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
                    SetValue(VerticalAlignmentProperty, value);
                    FirePropertyChanged("VerticalAlignment");
                }
#else
                 SetValue(VerticalAlignmentProperty, value);
#endif
            }
        }

        /// <summary>
        /// Get or set the BorderColor property of the Legend
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
        /// Get or set the BorderThickness property of the Legend
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
        /// Get or set the Background property of the Legend
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
                    FirePropertyChanged("Background");
                }
#else
                SetValue(BackgroundProperty, value);
#endif
            }
        }

        /// <summary>
        /// Get or set the DockInsidePlotArea property of the Legend
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

        /// <summary>
        /// Get or set the Enabled property of the Legend
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
        /// Get or set the FontColor property of the Legend
        /// </summary>
        public Brush FontColor
        {
            get
            {
                //return ((Brush)GetValue(FontColorProperty) == null) ? (new SolidColorBrush(Colors.Black)) : (Brush)GetValue(FontColorProperty);
                return (Brush)GetValue(FontColorProperty);
            }
            set
            {
                SetValue(FontColorProperty, value);
            }
        }

        /// <summary>
        /// Get or set the FontFamily property of the Legend
        /// </summary>
        public new FontFamily FontFamily
        {
            get
            {
                if ((FontFamily)GetValue(FontFamilyProperty) == null)
                    return new FontFamily("Verdana");
                else
                    return (FontFamily)GetValue(FontFamilyProperty);
            }
            set
            {

#if SL
                if (FontFamily != value)
                {
                    SetValue(FontFamilyProperty, value);
                    FirePropertyChanged("FontFamily");
                }
#else
                SetValue(FontFamilyProperty, value);
#endif
            }
        }

        /// <summary>
        /// Get or set the FontSize property of the Legend
        /// </summary>
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
                    SetValue(FontSizeProperty, value);
                    FirePropertyChanged("FontSize");
                }
#else
                SetValue(FontSizeProperty, value);
#endif
            }
        }

        /// <summary>
        /// Get or set the FontStyle property of the Legend
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
                if (FontStyle != value)
                {
                    SetValue(FontStyleProperty, value);
                    FirePropertyChanged("FontStyle");
                }
#else
                SetValue(FontStyleProperty, value);
#endif
            }
        }

        /// <summary>
        /// Get or set the FontWeight property of the Legend
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
#if WPF
                if (FontWeight != value)
                {
                    SetValue(FontWeightProperty, value);
                    FirePropertyChanged("FontWeight");
                }
#else
                SetValue(FontWeightProperty, value);
#endif
            }
        }

        /// <summary>
        /// Get or set the LightingEnabled property of the Legend
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

#if WPF
        [System.ComponentModel.TypeConverter(typeof(System.Windows.CornerRadiusConverter))]
#else
        [System.ComponentModel.TypeConverter(typeof(Converters.CornerRadiusConverter))]
#endif
        /// <summary>
        /// Get or set the CornerRadius property of the Legend
        /// </summary>
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
        /// Get or set the Title property of the Legend
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
        /// Get or set the TitleAlignmentX property of the Legend
        /// </summary>
        public HorizontalAlignment TitleAlignmentX
        {
            get
            {
                return (HorizontalAlignment)GetValue(TitleAlignmentXProperty);
            }
            set
            {
                SetValue(TitleAlignmentXProperty, value);
            }
        }

        /// <summary>
        /// Get or set the TitleTextAlignment property of the Legend
        /// </summary>
        public TextAlignment TitleTextAlignment
        {
            get
            {
                return (TextAlignment)GetValue(TitleTextAlignmentProperty);
            }
            set
            {
                SetValue(TitleTextAlignmentProperty, value);
            }
        }

        /// <summary>
        /// Get or set the TitleBackground property of the Legend
        /// </summary>
        public Brush TitleBackground
        {
            get
            {
                return (Brush)GetValue(TitleBackgroundProperty);
            }
            set
            {
                SetValue(TitleBackgroundProperty, value);
            }
        }

        /// <summary>
        /// Get or set the TitleFontColor property of the Legend
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
        /// Get or set the TitleFontFamily property of the Legend
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
        /// Get or set the TitleFontSize property of the Legend
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
        /// Get or set the TitleFontStyle property of the Legend
        /// </summary>
#if SL
        [System.ComponentModel.TypeConverter(typeof(Visifire.Commons.Converters.FontStyleConverter))]
#endif
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
        /// Get or set the TitleFontWeight property of the Legend
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
        /// Get or set the EntryMargin property of the Legend
        /// </summary>
        public Double EntryMargin
        {
            get
            {
                return (Double)GetValue(EntryMarginProperty);
            }
            set
            {
                SetValue(EntryMarginProperty, value);
            }
        }

        #endregion

        #region Public Events And Delegates

        #endregion

        #region Protected Methods

        #endregion

        #region Internal Properties

        /// <summary>
        /// Get or set the maximum width of the legend
        /// </summary>
        internal Double MaximumWidth
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set the maximum height of the Legend
        /// </summary>
        internal Double MaximumHeight
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set theOrientation of Legend
        /// </summary>
        internal Orientation Orientation
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set the Maximum number of rows in Legend
        /// </summary>
        internal Int32 MaxRows
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set the Maximum number of columns in Legend
        /// </summary>
        internal Int32 MaxColumns
        {
            get;
            set;
        }

        /// <summary>
        /// Legend visual
        /// </summary>
        internal Border Visual
        {
            get;
            set;
        }

        /// <summary>
        /// Layout type of Legend
        /// </summary>
        internal Layouts LegendLayout
        {
            get;
            set;
        }

        /// <summary>
        /// Label text and Marker as symbol
        /// </summary>
        internal List<KeyValuePair<String, Marker>> Entries
        {
            get;
            set;
        }

        #endregion

        #region Private Properties

        /// <summary>
        /// LegendContainer is the 1st child of the Visual
        /// </summary>
        private StackPanel LegendContainer
        {
            get;
            set;
        }

        /// <summary>
        /// Size of an entry
        /// </summary>
        private struct EntrySize
        {
            /// <summary>
            /// Size of the entry symbol
            /// </summary>
            public Size SymbolSize;

            /// <summary>
            /// Size of the textSize
            /// </summary>
            public Size TextSize;
        }

        #endregion

        #region Private Delegates

        #endregion

        #region Private Methods

        /// <summary>
        /// Event handler attached with HrefTarget property changed event of Legend element
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnHrefTargetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Legend c = d as Legend;
            c.FirePropertyChanged("HrefTarget");
        }

        /// <summary>
        /// Event handler attached with Href property changed event of Legend element
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnHrefChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Legend c = d as Legend;
            c.FirePropertyChanged("Href");
        }
        
        /// <summary>
        /// Event handler attached with LabelMargin property changed event of Legend element
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnLabelMarginPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Legend legend = d as Legend;
            legend.FirePropertyChanged("LabelMargin");
        }

#if WPF    
    
        /// <summary>
        /// Event handler attached with Padding property changed event of Legend element
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnPaddingPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Legend legend = d as Legend;
            legend.FirePropertyChanged("Padding");
        }

        /// <summary>
        /// Event handler attached with HorizontalAlignment property changed event of Legend element
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnHorizontalAlignmentPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Legend legend = d as Legend;
            legend.FirePropertyChanged("HorizontalAlignment");
        }
        
        /// <summary>
        /// Event handler attached with VerticalAlignment property changed event of Legend element
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnVerticalAlignmentPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Legend legend = d as Legend;
            legend.FirePropertyChanged("VerticalAlignment");
        }

        /// <summary>
        /// Event handler attached with Opacity property changed event of Legend element
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnOpacityPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Legend legend = d as Legend;
            legend.FirePropertyChanged("Opacity");
        }
#endif

        /// <summary>
        /// Event handler attached with BorderColor property changed event of Legend element
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnBorderColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Legend legend = d as Legend;
            legend.FirePropertyChanged("BorderColor");
        }

#if WPF
        
        /// <summary>
        /// Event handler attached with BorderThickness property changed event of Legend element
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnBorderThicknessPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Legend legend = d as Legend;
            legend.FirePropertyChanged("BorderThickness");
        }
        
        /// <summary>
        /// Event handler attached with Background property changed event of Legend element
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnBackgroundPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Legend legend = d as Legend;
            legend.FirePropertyChanged("Background");
        }
#endif

        /// <summary>
        /// Event handler attached with DockInsidePlotArea property changed event of Legend element
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnDockInsidePlotAreaPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Legend legend = d as Legend;
            legend.FirePropertyChanged("DockInsidePlotArea");
        }

        /// <summary>
        /// Event handler attached with Enabled property changed event of Legend element
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnEnabledPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Legend legend = d as Legend;
            legend.FirePropertyChanged("Enabled");
        }
        
        /// <summary>
        /// Event handler attached with FontColor property changed event of Legend element
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnFontColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Legend legend = d as Legend;
            legend.FirePropertyChanged("FontColor");
        }

#if WPF
        
        /// <summary>
        /// Event handler attached with FontFamily property changed event of Legend element
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnFontFamilyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Legend legend = d as Legend;
            legend.FirePropertyChanged("FontFamily");
        }
        
        /// <summary>
        /// Event handler attached with FontSize property changed event of Legend element
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnFontSizePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Legend legend = d as Legend;
            legend.FirePropertyChanged("FontSize");
        }
        
        /// <summary>
        /// Event handler attached with FontStyle property changed event of Legend element
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnFontStylePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Legend legend = d as Legend;
            legend.FirePropertyChanged("FontStyle");
        }
        
        /// <summary>
        /// Event handler attached with FontWeight property changed event of Legend element
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnFontWeightPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Legend legend = d as Legend;
            legend.FirePropertyChanged("FontWeight");
        }
#endif


        /// <summary>
        /// Event handler attached with LightingEnabled property changed event of Legend element
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnLightingEnabledPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Legend legend = d as Legend;
            legend.FirePropertyChanged("LightingEnabled");
        }

        /// <summary>
        /// Event handler attached with CornerRadius property changed event of Legend element
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnCornerRadiusPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Legend legend = d as Legend;
            legend.FirePropertyChanged("CornerRadius");
        }

        /// <summary>
        /// Event handler attached with Title property changed event of Legend element
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnTitlePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Legend legend = d as Legend;
            legend.FirePropertyChanged("Title");
        }

        /// <summary>
        /// Event handler attached with TitleAlignmentX property changed event of Legend element
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnTitleAlignmentXPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Legend legend = d as Legend;
            legend.FirePropertyChanged("TitleAlignmentX");
        }

        /// <summary>
        /// Event handler attached with TitleTextAlignment property changed event of Legend element
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnTitleTextAlignmentPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Legend legend = d as Legend;
            legend.FirePropertyChanged("TitleTextAlignment");
        }

        /// <summary>
        /// Event handler attached with TitleBackground property changed event of Legend element
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnTitleBackgroundPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Legend legend = d as Legend;
            legend.FirePropertyChanged("TitleBackground");
        }
        
        /// <summary>
        /// Event handler attached with TitleFontColor property changed event of Legend element
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnTitleFontColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Legend legend = d as Legend;
            legend.FirePropertyChanged("TitleFontColor");
        }

        /// <summary>
        /// Event handler attached with TitleFontFamily property changed event of Legend element
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnTitleFontFamilyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Legend legend = d as Legend;
            legend.FirePropertyChanged("TitleFontFamily");
        }

        /// <summary>
        /// Event handler attached with TitleFontStyle property changed event of Legend element
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnTitleFontStylePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Legend legend = d as Legend;
            legend.FirePropertyChanged("TitleFontStyle");
        }

        /// <summary>
        /// Event handler attached with TitleFontSize property changed event of Legend element
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnTitleFontSizePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Legend legend = d as Legend;
            legend.FirePropertyChanged("TitleFontSize");
        }

        /// <summary>
        /// Event handler attached with TitleFontWeight property changed event of Legend element
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnTitleFontWeightPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Legend legend = d as Legend;
            legend.FirePropertyChanged("TitleFontWeight");
        }

        /// <summary>
        /// Event handler attached with EntryMargin property changed event of Legend element
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnEntryMarginPropertyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Legend legend = d as Legend;
            legend.FirePropertyChanged("EntryMargin");
        }

        /// <summary>
        /// Apply font properties of a TextBlock
        /// </summary>
        /// <param name="textBlock"></param>
        private void ApplyFontProperty(TextBlock textBlock)
        {
            textBlock.FontFamily = FontFamily;
            textBlock.FontStyle = FontStyle;
            textBlock.FontWeight = FontWeight;
            textBlock.FontSize = FontSize;
            textBlock.Foreground = Charts.Chart.CalculateFontColor((Chart as Chart), FontColor, this.DockInsidePlotArea);
        }

        /// <summary>
        /// Apply font properties of a marker
        /// </summary>
        /// <param name="marker">Marker</param>
        private void ApplyFontPropertiesOfMarkerAsSymbol(Marker marker)
        {
            marker.FontFamily = FontFamily;
            marker.FontStyle = FontStyle;
            marker.FontWeight = FontWeight;
            marker.FontSize = FontSize;
            marker.FontColor = Charts.Chart.CalculateFontColor((Chart as Chart), FontColor, this.DockInsidePlotArea);
        }

        /// <summary>
        /// Apply font properties of the title of Legend
        /// </summary>
        /// <param name="title"></param>
        private void ApplyFontProperty(Title title)
        {
            if (TitleFontFamily != null)
                title.FontFamily = TitleFontFamily;

            if (TitleFontSize != 0)
                title.FontSize = TitleFontSize;

            if (TitleFontStyle != null)
                title.FontStyle = TitleFontStyle;

            if (TitleFontWeight != null)
                title.FontWeight = TitleFontWeight;

            if (!String.IsNullOrEmpty(Title))
                title.Text = GetFormattedMultilineText(Title);

            title.FontColor = Charts.Chart.CalculateFontColor((Chart as Chart), TitleFontColor, this.DockInsidePlotArea);
        }

        /// <summary>
        /// Apply visual properties
        /// </summary>
        private void ApplyVisualProperty()
        {
            if (Cursor != null)
                Visual.Cursor = Cursor;

            Visual.BorderBrush = BorderColor;

            Visual.BorderThickness = BorderThickness;

            Visual.CornerRadius = CornerRadius;
            Visual.Background = this.Background;

            Visual.HorizontalAlignment = HorizontalAlignment;
            Visual.VerticalAlignment = VerticalAlignment;
            Visual.Opacity = this.Opacity;

            ApplyLighting();
            AttachHref(Chart, Visual, Href, HrefTarget);
            AttachToolTip(Chart, this, Visual);
            AttachEvents2Visual(this, Visual);
        }

        /// <summary>
        /// Return actual size of the TextBlock
        /// </summary>
        /// <param name="textBlock">TextBlock</param>
        /// <returns>Size</returns>
        private Size TextBlockActualSize(TextBlock textBlock)
        {
#if WPF     
            textBlock.Measure(new Size(Double.MaxValue,Double.MaxValue));
            return textBlock.DesiredSize;
#else
            return new Size(textBlock.ActualWidth, textBlock.ActualHeight);
#endif
        }

        /// <summary>
        /// Plloy lighting over Legend
        /// </summary>
        private void ApplyLighting()
        {
            if (LightingEnabled)
                LegendContainer.Background = Graphics.LightingBrush(LightingEnabled);
            else
                LegendContainer.Background = new SolidColorBrush(Colors.Transparent);
        }

        /// <summary>
        /// Returns StackPanel used as a column of the Legend
        /// </summary>
        /// <returns>StackPanel</returns>
        private StackPanel StackPanelColumn()
        {
            StackPanel st = new StackPanel();
            st.HorizontalAlignment = HorizontalAlignment.Left;
            st.Orientation = Orientation.Vertical;
            return st;
        }

        /// <summary>
        /// Returns StackPanel used as a row of the Legend
        /// </summary>
        /// <returns>StackPanel</returns>
        private StackPanel StackPanelRow()
        {
            StackPanel st = new StackPanel();
            st.Orientation = Orientation.Horizontal;
            return st;
        }

        /// <summary>
        /// Returns max entry size of legend
        /// </summary>
        /// <returns>EntrySize</returns>
        private EntrySize GetMaxSymbolAndColumnWidth()
        {
            EntrySize entrySize = new EntrySize();

            foreach (KeyValuePair<String, Marker> labelAndSymbol in Entries)
            {
                TextBlock t = new TextBlock();
                t.Text = labelAndSymbol.Key;
                ApplyFontProperty(t);
                Size labelSize = TextBlockActualSize(t);
                entrySize.TextSize.Width = (labelSize.Width > entrySize.TextSize.Width) ? labelSize.Width : entrySize.TextSize.Width;
                entrySize.TextSize.Height = (labelSize.Height > entrySize.TextSize.Height) ? labelSize.Height : entrySize.TextSize.Height;
                (labelAndSymbol.Value as Marker).Margin = EntryMargin;

                (labelAndSymbol.Value as Marker).CreateVisual();
                entrySize.SymbolSize.Width = ((labelAndSymbol.Value as Marker).MarkerActualSize.Width > entrySize.SymbolSize.Width) ? (labelAndSymbol.Value as Marker).MarkerActualSize.Width : entrySize.SymbolSize.Width;
                entrySize.SymbolSize.Height = ((labelAndSymbol.Value as Marker).MarkerActualSize.Height > entrySize.SymbolSize.Height) ? (labelAndSymbol.Value as Marker).MarkerActualSize.Height : entrySize.SymbolSize.Height;
            }

            return entrySize;
        }
        
        /// <summary>
        /// Apply LineStyle to line symbol of a legend entry in Legend
        /// </summary>
        /// <param name="line"></param>
        /// <param name="lineStyle"></param>
        private DoubleCollection ApplyLineStyleForMarkerOfLegendEntry(Line line, String lineStyle)
        {
            DoubleCollection retVal = null;

            switch (lineStyle)
            {
                case "Solid":
                    line.StrokeThickness = 3;
                    retVal = null;
                    break;
                case "Dashed":
                    line.StrokeThickness = 3;
                    retVal = new DoubleCollection() { .2, .4, .2, .12 };
                    break;
                case "Dotted":
                    line.StrokeThickness = 3;
                    retVal = new DoubleCollection() { .5, .5, .5, .5 };
                    break;
            }

            return retVal;
        }

        /// <summary>
        /// Customize the marker for legend in Line chart
        /// </summary>
        /// <param name="marker"></param>
        /// <returns></returns>
        private Canvas GetNewMarkerForLineChart(Marker marker)
        {
            Canvas lineMarker = new Canvas();
            Line line = new Line();

            line.Margin = new Thickness(EntryMargin);
            line.Stroke = (marker.BorderColor);
           
            Double height = marker.TextBlockSize.Height > marker.MarkerSize.Height ? marker.TextBlockSize.Height : marker.MarkerSize.Height;
            lineMarker.Height = marker.MarkerActualSize.Height;

            line.X1 = 0;
            line.X2 = ENTRY_SYMBOL_LINE_WIDTH;
            line.Y1 = 0;
            line.Y2 = 0;
            line.Width = ENTRY_SYMBOL_LINE_WIDTH;

            lineMarker.Width = marker.MarkerActualSize.Width + ENTRY_SYMBOL_LINE_WIDTH / 2;


            line.StrokeDashArray = ApplyLineStyleForMarkerOfLegendEntry(line, marker.DataSeriesOfLegendMarker.LineStyle.ToString());

            lineMarker.Children.Add(line);
            lineMarker.Children.Add(marker.Visual);

            if (!(VerticalAlignment == VerticalAlignment.Center && (HorizontalAlignment == HorizontalAlignment.Left || HorizontalAlignment == HorizontalAlignment.Right)))
            {
                line.Margin = new Thickness(ENTRY_SYMBOL_LINE_WIDTH / 2, marker.Visual.Margin.Top, marker.Visual.Margin.Right, marker.Visual.Margin.Bottom);
                marker.Visual.Margin = new Thickness(ENTRY_SYMBOL_LINE_WIDTH / 2, marker.Visual.Margin.Top, marker.Visual.Margin.Right, marker.Visual.Margin.Bottom);
            }

#if WPF
            line.SetValue(Canvas.TopProperty, height/2 );
#else
            line.Height = 8;
            line.SetValue(Canvas.TopProperty, (height / 2) + .4876);
#endif      

            line.SetValue(Canvas.LeftProperty, (Double)(-marker.MarkerSize.Width / 2) - .4876);
            return lineMarker;
        }

        /// <summary>
        /// Draw vertical flow layout for legend
        /// </summary>
        /// <param name="legendContent">Legend content referecnce</param>
        private void DrawVerticalFlowLayout4Legend(ref Grid legendContent)
        {
            Int32 currentPanelIndex = 0;
            Double currentHeight = 0;
            StackPanel legendPanel = new StackPanel();

            (legendPanel as StackPanel).Orientation = Orientation.Horizontal;
            legendPanel.Children.Add(StackPanelColumn());
            legendPanel.Height = 0;

            foreach (KeyValuePair<String, Marker> labelAndSymbol in Entries)
            {
                Marker markerAsSymbol = labelAndSymbol.Value;
                markerAsSymbol.Margin = EntryMargin;
                markerAsSymbol.LabelMargin = LabelMargin;
                markerAsSymbol.Text = labelAndSymbol.Key;

                markerAsSymbol.TextAlignmentY = AlignmentY.Center;
                markerAsSymbol.TextAlignmentX = AlignmentX.Right;

                ApplyFontPropertiesOfMarkerAsSymbol(markerAsSymbol);

                if (markerAsSymbol.DataSeriesOfLegendMarker.RenderAs == RenderAs.Line)
                {
                    markerAsSymbol.BorderColor = markerAsSymbol.MarkerFillColor;
                    markerAsSymbol.MarkerFillColor = new SolidColorBrush(Colors.White);
                    markerAsSymbol.BorderThickness = 0.7;

                    markerAsSymbol.CreateVisual();

                    Canvas lineMarker = GetNewMarkerForLineChart(markerAsSymbol);

                    if ((currentHeight + lineMarker.Height) <= MaximumHeight)
                    {
                        (legendPanel.Children[currentPanelIndex] as StackPanel).Children.Add(lineMarker);
                        currentHeight += lineMarker.Height;
                    }
                    else
                    {
                        legendPanel.Children.Add(StackPanelColumn());
                        currentPanelIndex++;
                        (legendPanel.Children[currentPanelIndex] as StackPanel).Children.Add(lineMarker);
                        currentHeight = markerAsSymbol.MarkerActualSize.Height;
                    }
                }
                else
                {
                    markerAsSymbol.CreateVisual();

                    if ((currentHeight + markerAsSymbol.MarkerActualSize.Height) <= MaximumHeight)
                    {
                        (legendPanel.Children[currentPanelIndex] as StackPanel).Children.Add(markerAsSymbol.Visual);
                        currentHeight += markerAsSymbol.MarkerActualSize.Height;
                    }
                    else
                    {
                        legendPanel.Children.Add(StackPanelColumn());
                        currentPanelIndex++;
                        (legendPanel.Children[currentPanelIndex] as StackPanel).Children.Add(markerAsSymbol.Visual);
                        currentHeight = markerAsSymbol.MarkerActualSize.Height;
                    }
                }

                markerAsSymbol.Visual.HorizontalAlignment = HorizontalAlignment.Left;

                legendPanel.Height = (legendPanel.Height < currentHeight) ? currentHeight : legendPanel.Height;
            }

            legendPanel.HorizontalAlignment = HorizontalAlignment.Center;
            legendPanel.VerticalAlignment = VerticalAlignment.Center;

            legendContent.Children.Add(legendPanel);
        }

        /// <summary>
        /// Draw vertical grid layout for legend
        /// </summary>
        /// <param name="legendContent">Legend content referecnce</param>
        private void DrawVerticalGridlayout4Legend(ref Grid legendContent)
        {
            Int32 row, column;
            Grid legendGrid = new Grid();

            EntrySize maxEntrySize = GetMaxSymbolAndColumnWidth();

            MaxRows = (Int32)(MaximumHeight / (maxEntrySize.SymbolSize.Height + maxEntrySize.TextSize.Height + EntryMargin + LabelMargin));

            MaxColumns = (Int32)Math.Ceiling(((Double)Entries.Count / MaxRows));

            for (row = 0; row < MaxRows; row++)
                legendGrid.RowDefinitions.Add(new RowDefinition());

            row = 0;
            column = 0;

            Double maxRowHeight = 0;

            foreach (KeyValuePair<String, Marker> labelAndSymbol in Entries)
            {
                if (row == 0)
                {
                    legendGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(maxEntrySize.SymbolSize.Width + EntryMargin + LabelMargin / 2) });
                    legendGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(maxEntrySize.TextSize.Width + LabelMargin / 2) });
                }

                (labelAndSymbol.Value as Marker).Visual.Margin = new Thickness(EntryMargin, EntryMargin, LabelMargin / 2, EntryMargin);
                (labelAndSymbol.Value as Marker).Visual.SetValue(Grid.RowProperty, row);
                (labelAndSymbol.Value as Marker).Visual.SetValue(Grid.ColumnProperty, column++);

                legendGrid.Children.Add((labelAndSymbol.Value as Marker).Visual);

                TextBlock label = new TextBlock();
                label.Margin = new Thickness(LabelMargin, 0, 0, 0);

                label.Text = labelAndSymbol.Key;
                ApplyFontProperty(label);
                label.SetValue(Grid.RowProperty, row);
                label.SetValue(Grid.ColumnProperty, column++);
                label.HorizontalAlignment = HorizontalAlignment.Left;
                label.VerticalAlignment = VerticalAlignment.Center;
                legendGrid.Children.Add(label);
                label.Measure(new Size(Double.MaxValue, Double.MaxValue));

                Double maxRowHeight1 = (label.DesiredSize.Height > (labelAndSymbol.Value as Marker).Visual.DesiredSize.Height) ? label.DesiredSize.Height : (labelAndSymbol.Value as Marker).Visual.DesiredSize.Height;

                if (maxRowHeight1 > maxRowHeight)
                {
                    maxRowHeight = maxRowHeight1;
                    legendGrid.RowDefinitions[row].Height = new GridLength(maxRowHeight + 2 * EntryMargin);
                }

                if (column >= MaxColumns * 2)
                {
                    row++;
                    column = 0;
                }
            }

            legendGrid.ShowGridLines = true;
            legendGrid.HorizontalAlignment = HorizontalAlignment.Center;
            legendGrid.VerticalAlignment = VerticalAlignment.Center;

            legendContent.Children.Add(legendGrid);
        }

        /// <summary>
        /// Draw horizontal flow layout for legend
        /// </summary>
        /// <param name="legendContent">Legend content referecnce</param>
        private void DrawHorizontalFlowLayout4Legend(ref Grid legendContent)
        {
            Int32 currentPanelIndex = 0;
            Double currentWidth = 0;
            StackPanel legendPanel = new StackPanel();
            (legendPanel as StackPanel).Orientation = Orientation.Vertical;
            legendPanel.Children.Add(StackPanelRow());

            foreach (KeyValuePair<String, Marker> labelAndSymbol in Entries)
            {
                Marker marker = labelAndSymbol.Value;
                marker.Margin = EntryMargin;
                marker.LabelMargin = LabelMargin;
                marker.Text = labelAndSymbol.Key;
                ApplyFontPropertiesOfMarkerAsSymbol(marker);

                marker.TextAlignmentY = AlignmentY.Center;
                marker.TextAlignmentX = AlignmentX.Right;


                if (marker.DataSeriesOfLegendMarker.RenderAs == RenderAs.Line)
                {
                    marker.BorderColor = marker.MarkerFillColor;
                    marker.MarkerFillColor = new SolidColorBrush(Colors.White);
                    marker.BorderThickness = 0.7;

                    marker.LabelMargin += ENTRY_SYMBOL_LINE_WIDTH /2;

                    marker.CreateVisual();                   

                    Canvas lineMarker = GetNewMarkerForLineChart(marker);

                    if ((currentWidth + lineMarker.Width) <= MaximumWidth)
                    {
                        (legendPanel.Children[currentPanelIndex] as StackPanel).Children.Add(lineMarker);
                        currentWidth += lineMarker.Width;
                    }
                    else
                    {
                        legendPanel.Children.Add(StackPanelRow());
                        currentPanelIndex++;
                        (legendPanel.Children[currentPanelIndex] as StackPanel).Children.Add(lineMarker);
                        currentWidth = marker.MarkerActualSize.Width;
                    }
                }
                else
                {
                    marker.CreateVisual();

                    if ((currentWidth + marker.MarkerActualSize.Width) <= MaximumWidth)
                    {
                        (legendPanel.Children[currentPanelIndex] as StackPanel).Children.Add(marker.Visual);
                        currentWidth += marker.MarkerActualSize.Width;
                    }
                    else
                    {
                        legendPanel.Children.Add(StackPanelRow());
                        currentPanelIndex++;
                        (legendPanel.Children[currentPanelIndex] as StackPanel).Children.Add(marker.Visual);
                        currentWidth = marker.MarkerActualSize.Width;
                    }
                }

                marker.Visual.HorizontalAlignment = HorizontalAlignment.Center;
            }

            legendPanel.HorizontalAlignment = HorizontalAlignment.Center;
            legendPanel.VerticalAlignment = VerticalAlignment.Center;

            legendContent.Children.Add(legendPanel);
        }

        /// <summary>
        /// Draw horizontal grid layout for legend
        /// </summary>
        /// <param name="legendContent">Legend content referecnce</param>
        private void DrawHorizontalGridlayout4Legend(ref Grid legendContent)
        {
            Int32 row, column;
            Grid legendGrid = new Grid();

            EntrySize maxEntrySize = GetMaxSymbolAndColumnWidth();

            MaxColumns = (Int32)(MaximumWidth / (maxEntrySize.SymbolSize.Width + maxEntrySize.TextSize.Width + EntryMargin + LabelMargin));

            MaxRows = (Int32)Math.Ceiling(((Double)Entries.Count / MaxColumns));

            for (row = 0; row < MaxRows; row++)
                legendGrid.RowDefinitions.Add(new RowDefinition());

            row = 0;
            column = 0;

            Double maxRowHeight = 0;

            foreach (KeyValuePair<String, Marker> labelAndSymbol in Entries)
            {
                if (row == 0)
                {
                    legendGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(maxEntrySize.SymbolSize.Width + EntryMargin + LabelMargin / 2) });
                    legendGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(maxEntrySize.TextSize.Width + LabelMargin / 2) });
                }

                (labelAndSymbol.Value as Marker).Visual.Margin = new Thickness(EntryMargin, EntryMargin, LabelMargin / 2, EntryMargin);
                (labelAndSymbol.Value as Marker).Visual.SetValue(Grid.RowProperty, row);
                (labelAndSymbol.Value as Marker).Visual.SetValue(Grid.ColumnProperty, column++);

                legendGrid.Children.Add((labelAndSymbol.Value as Marker).Visual);

                TextBlock label = new TextBlock();
                label.Margin = new Thickness(LabelMargin, 0, 0, 0);

                label.Text = labelAndSymbol.Key;
                ApplyFontProperty(label);
                label.SetValue(Grid.RowProperty, row);
                label.SetValue(Grid.ColumnProperty, column++);
                label.HorizontalAlignment = HorizontalAlignment.Left;
                label.VerticalAlignment = VerticalAlignment.Center;
                legendGrid.Children.Add(label);
                label.Measure(new Size(Double.MaxValue, Double.MaxValue));

                Double maxRowHeight1 = (label.DesiredSize.Height > (labelAndSymbol.Value as Marker).Visual.DesiredSize.Height) ? label.DesiredSize.Height : (labelAndSymbol.Value as Marker).Visual.DesiredSize.Height;

                if (maxRowHeight1 > maxRowHeight)
                {
                    maxRowHeight = maxRowHeight1;
                    legendGrid.RowDefinitions[row].Height = new GridLength(maxRowHeight + 2 * EntryMargin);
                }

                if (column >= MaxColumns * 2)
                {
                    row++;
                    column = 0;
                }
            }

            legendGrid.ShowGridLines = true;
            legendGrid.HorizontalAlignment = HorizontalAlignment.Center;
            legendGrid.VerticalAlignment = VerticalAlignment.Center;
            
            legendContent.Children.Add(legendGrid);
        }

        /// <summary>
        /// Create the content of the Legend
        /// </summary>
        /// <returns>Grid</returns>
        private Grid CreateLegendContent()
        {
            Grid legendContent = new Grid();

            MaximumWidth -= 2 * Padding.Left;
            MaximumHeight -= 2 * Padding.Left;

            if (Orientation == Orientation.Vertical)
            {
                if (LegendLayout == Layouts.FlowLayout)
                {
                    DrawVerticalFlowLayout4Legend(ref legendContent);
                }
                else if (LegendLayout == Layouts.Gridlayout)// MaxWidth is reqired for GridLayout calculation
                {
                    DrawVerticalGridlayout4Legend(ref legendContent);
                }
            }
            else if (Orientation == Orientation.Horizontal)
            {
                if (LegendLayout == Layouts.FlowLayout)
                {
                    DrawHorizontalFlowLayout4Legend(ref legendContent);
                }
                else if (LegendLayout == Layouts.Gridlayout)// MaxWidth is reqired for GridLayout calculation
                {
                    DrawHorizontalGridlayout4Legend(ref legendContent);
                }
            }

            legendContent.Measure(new Size(Double.MaxValue, Double.MaxValue));
            legendContent.Height = legendContent.DesiredSize.Height + Padding.Left * 2;
            legendContent.Width = legendContent.DesiredSize.Width + Padding.Left * 2;

            return legendContent;
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Create visual object of the Legend
        /// </summary>
        internal void CreateVisualObject()
        {
            if (!(Boolean)Enabled)
            {
                Visual = null;
                return;
            }

            Visual = new Border();
            Grid innerGrid = new Grid();
            (Visual as Border).Child = innerGrid;

            LegendContainer = new StackPanel();

            if (!String.IsNullOrEmpty(Title))
            {
                Title legendTitle = new Title();
                ApplyFontProperty(legendTitle);

                if (TitleBackground != null)
                    legendTitle.Background = TitleBackground;

                legendTitle.HorizontalAlignment = TitleAlignmentX;
                legendTitle.VerticalAlignment = VerticalAlignment.Top;
                legendTitle.TextAlignment = TitleTextAlignment;

                legendTitle.CreateVisualObject();

                LegendContainer.Children.Add(legendTitle.Visual);
            }

            Grid legendContent = CreateLegendContent();

            LegendContainer.Children.Add(legendContent);

            ApplyVisualProperty();

            innerGrid.Children.Add(LegendContainer);

            Visual.Cursor = this.Cursor;
            Visual.Measure(new Size(Double.MaxValue, Double.MaxValue));
            Visual.Height = Visual.DesiredSize.Height;
            Visual.Width = Visual.DesiredSize.Width + Padding.Left;
        }

        #endregion

        #region Internal Events And Delegates

        #endregion

        #region Data

        private const Double ENTRY_SYMBOL_LINE_WIDTH = 18;

#if WPF

        /// <summary>
        /// Whether the default style is applied
        /// </summary>
        private static Boolean _defaultStyleKeyApplied;         
 
#endif

        #endregion
    }

}