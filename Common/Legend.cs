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
using System.Windows.Browser;

#endif

using System.Windows.Data;

using Visifire.Commons;
using System.Linq;
using System.Windows.Media.Effects;

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

            Entries = new List<LegendEntry>();

            // Attach event handler with EventChanged event of VisifireElement
            EventChanged += delegate
            {
                FirePropertyChanged(VcProperties.MouseEvent);
            };
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

            b = new Binding("Padding");
            b.Source = this;
            this.SetBinding(InternalPaddingProperty, b);

            b = new Binding("Opacity");
            b.Source = this;
            this.SetBinding(InternalOpacityProperty, b);
            
            b = new Binding("MaxWidth");
            b.Source = this;
            this.SetBinding(InternalMaxWidthProperty, b);

            b = new Binding("MaxHeight");
            b.Source = this;
            this.SetBinding(InternalMaxHeightProperty, b);
#endif
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
        /// Identifies the Visifire.Charts.Legend.Reversed dependency property.
        /// Set to true if you want to reverse the sequence of Legend entries.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Legend.Reversed dependency property.
        /// </returns>
        public static readonly DependencyProperty ReversedProperty = DependencyProperty.Register
            ("Reversed",
            typeof(Boolean),
            typeof(Legend),
            new PropertyMetadata(OnReversedChanged));

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
        /// Identifies the Visifire.Charts.Legend.LegendFontColor dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Legend.TitleFontColor dependency property.
        /// </returns>
        public static readonly DependencyProperty TitleFontColorProperty = DependencyProperty.Register
            ("TitleFontColor",
            typeof(Brush),
            typeof(Legend),
            new PropertyMetadata(OnTitleFontColorPropertyChanged));

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
        /// Identifies the Visifire.Charts.Legend.ShadowEnabled dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Legend.ShadowEnabled dependency property.
        /// </returns>
        public static readonly DependencyProperty ShadowEnabledProperty = DependencyProperty.Register
            ("ShadowEnabled",
            typeof(Boolean),
            typeof(Legend),
            new PropertyMetadata(OnShadowEnabledPropertyChanged));

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
           new PropertyMetadata(OnEntryMarginPropertyChanged));

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
            typeof(Legend), new PropertyMetadata(OnFontSizePropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.Title.FontFamily dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Title.FontFamily dependency property.
        /// </returns>
        private static readonly DependencyProperty InternalFontFamilyProperty = DependencyProperty.Register
            ("InternalFontFamily",
            typeof(FontFamily),
            typeof(Legend),
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
            typeof(Legend),
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
            typeof(Legend),
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
            typeof(Legend),
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
            typeof(Legend),
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
            typeof(Legend),
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
            typeof(Legend),
            new PropertyMetadata(OnVerticalAlignmentPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.Title.Padding dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Title.Padding dependency property.
        /// </returns>
        private static readonly DependencyProperty InternalPaddingProperty = DependencyProperty.Register
            ("InternalPadding",
            typeof(Thickness),
            typeof(Legend),
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
            typeof(Legend),
            new PropertyMetadata(1.0, OnOpacityPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.Legend.MaxHeight dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Legend.MaxHeight dependency property.
        /// </returns>
        private static readonly DependencyProperty InternalMaxHeightProperty = DependencyProperty.Register
           ("InternalMaxHeight",
           typeof(Double),
           typeof(Legend),
           new PropertyMetadata(Double.PositiveInfinity, OnMaxHeightPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.Legend.MaxWidth dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Legend.MaxWidth dependency property.
        /// </returns>
        private static readonly DependencyProperty InternalMaxWidthProperty = DependencyProperty.Register
           ("InternalMaxWidth",
           typeof(Double),
           typeof(Legend),
           new PropertyMetadata(Double.PositiveInfinity, OnMaxWidthPropertyChanged));
#else

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

        /// <summary>
        /// Identifies the Visifire.Charts.Legend.MaxHeight dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Legend.MaxHeight dependency property.
        /// </returns>
        public new static readonly DependencyProperty MaxHeightProperty = DependencyProperty.Register
           ("MaxHeight",
           typeof(Double),
           typeof(Legend),
           new PropertyMetadata(Double.PositiveInfinity, OnMaxHeightPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.Legend.MaxWidth dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Legend.MaxWidth dependency property.
        /// </returns>
        public new static readonly DependencyProperty MaxWidthProperty = DependencyProperty.Register
           ("MaxWidth",
           typeof(Double),
           typeof(Legend),
           new PropertyMetadata(Double.PositiveInfinity, OnMaxWidthPropertyChanged));
#endif

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
        /// Get or set the Reversed porperty.
        /// Set to true if you want to reverse the sequence of Legend entries.
        /// </summary>
        public Boolean Reversed
        {
            get
            {
                return (Boolean)GetValue(ReversedProperty);
            }
            set
            {
                SetValue(ReversedProperty, value);
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

                InternalPadding = value;
                SetValue(PaddingProperty, value);
                FirePropertyChanged(VcProperties.Padding);
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
                    InternalHorizontalAlignment = value;
                    SetValue(HorizontalAlignmentProperty, value);
                    FirePropertyChanged(VcProperties.HorizontalAlignment);
                }
                else
                    SetValue(HorizontalAlignmentProperty, value);
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
                    InternalVerticalAlignment = value;
                    SetValue(VerticalAlignmentProperty, value);
                    FirePropertyChanged(VcProperties.VerticalAlignment);
                }
                else
                    SetValue(VerticalAlignmentProperty, value);
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
                    InternalBorderThickness = value;
                    SetValue(BorderThicknessProperty, value);
                    FirePropertyChanged(VcProperties.BorderThickness);
                }
                else
                    SetValue(BorderThicknessProperty, value);
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
                    InternalBackground = value;
                    SetValue(BackgroundProperty, value);
                    FirePropertyChanged(VcProperties.Background);
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
                    InternalFontFamily = value;
                    SetValue(FontFamilyProperty, value);
                    FirePropertyChanged(VcProperties.FontFamily);
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
                    InternalFontSize = value;
                    SetValue(FontSizeProperty, value);
                    FirePropertyChanged(VcProperties.FontSize);
                }
                else
                    SetValue(FontSizeProperty, value);
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
                    InternalFontStyle = value;
                    SetValue(FontStyleProperty, value);
                    FirePropertyChanged(VcProperties.FontStyle);
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
#if SL
                if (InternalFontWeight != value)
                {
                    InternalFontWeight = value;
                    SetValue(FontWeightProperty, value);
                    FirePropertyChanged(VcProperties.FontWeight);
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

        /// <summary>
        /// Get or set the ShadowEnabled property of the Legend
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

        /// <summary>
        /// Get or set the maximum height of the Legend
        /// </summary>
        public new Double MaxHeight
        {
            get
            {
                return (Double)GetValue(MaxHeightProperty);
            }
            set
            {
#if SL          
                if (MaxHeight != value)
                {
                    SetValue(MaxHeightProperty, value);
                    FirePropertyChanged(VcProperties.MaxHeight);
                }
#else
                SetValue(MaxHeightProperty, value);
#endif
            }
        }

        /// <summary>
        /// Get or set the maximum height of the Legend
        /// </summary>
        public new Double MaxWidth
        {
            get
            {
                return (Double)GetValue(MaxWidthProperty);
            }
            set
            {
#if SL
                if (MaxWidth != value)
                {
                    InternalMaxWidth = value;
                    SetValue(MaxWidthProperty, value);
                    FirePropertyChanged(VcProperties.MaxWidth);
                }
#else
                SetValue(MaxWidthProperty, value);
#endif
            }
        }



        /// <summary>
        /// Get or set the FontFamily property of Legend
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
        /// Get or set the FontSize property of Legend
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
        /// Get or set the FontStyle property of Legend text
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
        /// Get or set the FontWeight property of Legend text
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
        /// Get or set the BorderThickness of Legend
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

        /// <summary>
        /// Get or set the Background property of Legend
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

        /// <summary>
        /// Get or set the HorizontalAlignment property of Legend
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

        /// <summary>
        /// Get or set the VerticalAlignment property of Legend
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

        /// <summary>
        /// Get or set the Padding property of Legend
        /// </summary>
        internal Thickness InternalPadding
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
        /// Get or set the maximum height of the Legend
        /// </summary>
        internal Double InternalMaxHeight
        {
            get
            {
                return (Double)(Double.IsNaN(_internalMaxheight) ? GetValue(MaxHeightProperty) : _internalMaxheight);
            }
            set
            {
                _internalMaxheight = value;
            }
        }

        /// <summary>
        /// Get or set the maximum height of the Legend
        /// </summary>
        internal Double InternalMaxWidth
        {
            get
            {
                return (Double)(Double.IsNaN(_internalMaxWidth) ? GetValue(MaxWidthProperty) : _internalMaxWidth);
            }
            set
            {
                _internalMaxWidth = value;
            }
        }

        #endregion

        #region Public Events And Delegates

        /// <summary>
        /// Event handler for the MouseLeftButtonDown event 
        /// </summary>
#if SL
        [ScriptableMember]
#endif
        public new event EventHandler<LegendMouseButtonEventArgs> MouseLeftButtonDown
        {
            remove
            {
                _onMouseLeftButtonDown -= value;

                if (EventChanged != null)
                    EventChanged(this, null);
            }
            add
            {
                _onMouseLeftButtonDown += value;

                if (EventChanged != null)
                    EventChanged(this, null);
            }
        }

        /// <summary>
        /// Event handler for the MouseLeftButtonUp event 
        /// </summary>
#if SL
        [ScriptableMember]
#endif
        public new event EventHandler<LegendMouseButtonEventArgs> MouseLeftButtonUp
        {
            remove
            {
                _onMouseLeftButtonUp -= value;

                if (EventChanged != null)
                    EventChanged(this, null);
            }
            add
            {
                _onMouseLeftButtonUp += value;

                if (EventChanged != null)
                    EventChanged(this, null);
            }
        }

#if WPF
        /// <summary>
        /// Event handler for the MouseLeftButtonDown event 
        /// </summary>
        public new event EventHandler<LegendMouseButtonEventArgs> MouseRightButtonDown
        {
            remove
            {
                _onMouseRightButtonDown -= value;

                if (EventChanged != null)
                    EventChanged(this, null);
            }
            add
            {
                _onMouseRightButtonDown += value;

                if (EventChanged != null)
                    EventChanged(this, null);
            }
        }

        /// <summary>
        /// Event handler for the MouseLeftButtonUp event 
        /// </summary>
        public new event EventHandler<LegendMouseButtonEventArgs> MouseRightButtonUp
        {
            remove
            {
                _onMouseRightButtonUp -= value;

                if (EventChanged != null)
                    EventChanged(this, null);
            }
            add
            {
                _onMouseRightButtonUp += value;

                if (EventChanged != null)
                    EventChanged(this, null);
            }
        }
#endif

        /// <summary>
        /// Event handler for the MouseMove event 
        /// </summary>
#if SL
        [ScriptableMember]
#endif
        public new event EventHandler<LegendMouseEventArgs> MouseMove
        {
            remove
            {
                _onMouseMove -= value;

                if (EventChanged != null)
                    EventChanged(this, null);
            }
            add
            {
                _onMouseMove += value;

                if (EventChanged != null)
                    EventChanged(this, null);
            }
        }

        #endregion

        #region Protected Methods

        #endregion

        #region Internal Properties

        /// <summary>
        /// Get or set the maximum height of the Legend
        /// </summary>
        internal Double InternalMaximumHeight
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set the maximum height of the Legend
        /// </summary>
        internal Double InternalMaximumWidth
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

        internal Border ShadowBorder
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
        internal Layouts Layout
        {
            get
            {
                Layouts layOut = (Layouts) GetValue(LayoutProperty);

                if(layOut == Layouts.Auto)
                    return Layouts.FlowLayout;
                else
                    return (Layouts) GetValue(LayoutProperty);
            }
            set
            {
                SetValue(LayoutProperty, value);
            }
        }

        /// <summary>
        /// Identifies the Visifire.Charts.Legend.HrefTarget dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Legend.HrefTarget dependency property.
        /// </returns>
        internal static readonly DependencyProperty LayoutProperty = DependencyProperty.Register
            ("Layout",
            typeof(Layouts),
            typeof(Legend),null);

        /// <summary>
        /// Label text and Marker as symbol
        /// </summary>
        internal List<LegendEntry> Entries
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
            c.FirePropertyChanged(VcProperties.HrefTarget);
        }

        /// <summary>
        /// Event handler attached with Href property changed event of Legend element
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnHrefChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Legend c = d as Legend;
            c.FirePropertyChanged(VcProperties.Href);
        }

        /// <summary>
        /// Event handler attached with Reversed property changed event of Legend element
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnReversedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Legend c = d as Legend;
            c.FirePropertyChanged(VcProperties.Reversed);
        }

        /// <summary>
        /// Event handler attached with LabelMargin property changed event of Legend element
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnLabelMarginPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Legend legend = d as Legend;
            legend.FirePropertyChanged(VcProperties.LabelMargin);
        }

        /// <summary>
        /// Event handler attached with BorderColor property changed event of Legend element
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnBorderColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Legend legend = d as Legend;
            legend.FirePropertyChanged(VcProperties.BorderColor);
        }

        /// <summary>
        /// Event handler attached with DockInsidePlotArea property changed event of Legend element
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnDockInsidePlotAreaPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Legend legend = d as Legend;
            legend.FirePropertyChanged(VcProperties.DockInsidePlotArea);
        }

        /// <summary>
        /// Event handler attached with Enabled property changed event of Legend element
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnEnabledPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Legend legend = d as Legend;
            legend.FirePropertyChanged(VcProperties.Enabled);
        }

        /// <summary>
        /// Event handler attached with FontColor property changed event of Legend element
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnFontColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Legend legend = d as Legend;
            legend.FirePropertyChanged(VcProperties.FontColor);
        }

        //------------------------------------------------------------

        /// <summary>
        /// Event handler attached with FontFamily property changed event of Legend element
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnFontFamilyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Legend legend = d as Legend;

            if (e.NewValue == null || e.OldValue == null)
            {
                legend.InternalFontFamily = (FontFamily)e.NewValue;
                legend.FirePropertyChanged(VcProperties.FontFamily);
            }
            else if (e.NewValue.ToString() != e.OldValue.ToString())
            {
                legend.InternalFontFamily = (FontFamily)e.NewValue;
                legend.FirePropertyChanged(VcProperties.FontFamily);
            }
            //legend.FirePropertyChanged("FontFamily");
        }

        /// <summary>
        /// Event handler attached with FontSize property changed event of Legend element
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnFontSizePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Legend legend = d as Legend;
            legend.InternalFontSize = (Double)e.NewValue;
            legend.FirePropertyChanged(VcProperties.FontSize);
        }

        /// <summary>
        /// Event handler attached with FontStyle property changed event of Legend element
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnFontStylePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Legend legend = d as Legend;
            legend.InternalFontStyle = (FontStyle)e.NewValue;
            legend.FirePropertyChanged(VcProperties.FontStyle);
        }

        /// <summary>
        /// Event handler attached with FontWeight property changed event of Legend element
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnFontWeightPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Legend legend = d as Legend;
            legend.InternalFontWeight = (FontWeight)e.NewValue;
            legend.FirePropertyChanged(VcProperties.FontWeight);
        }

        /// <summary>
        /// Event handler attached with BorderThickness property changed event of Legend element
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnBorderThicknessPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Legend legend = d as Legend;
            legend.InternalBorderThickness = (Thickness)e.NewValue;
            legend.FirePropertyChanged(VcProperties.BorderThickness);
        }

        /// <summary>
        /// Event handler attached with Background property changed event of Legend element
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnBackgroundPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Legend legend = d as Legend;
            legend.InternalBackground = (Brush)e.NewValue;
            legend.FirePropertyChanged(VcProperties.Background);
        }

        /// <summary>
        /// Event handler attached with Padding property changed event of Legend element
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnPaddingPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Legend legend = d as Legend;
            legend.InternalPadding = (Thickness)e.NewValue;
            legend.FirePropertyChanged(VcProperties.Padding);
        }

        /// <summary>
        /// Event handler attached with HorizontalAlignment property changed event of Legend element
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnHorizontalAlignmentPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Legend legend = d as Legend;
            legend.InternalHorizontalAlignment = (HorizontalAlignment)e.NewValue;
            legend.FirePropertyChanged(VcProperties.HorizontalAlignment);
        }

        /// <summary>
        /// Event handler attached with VerticalAlignment property changed event of Legend element
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnVerticalAlignmentPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Legend legend = d as Legend;
            legend.InternalVerticalAlignment = (VerticalAlignment)e.NewValue;
            legend.FirePropertyChanged(VcProperties.VerticalAlignment);
        }

        /// <summary>
        /// Event handler attached with Opacity property changed event of Legend element
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnOpacityPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Legend legend = d as Legend;
            legend.InternalOpacity = (Double)e.NewValue;
            legend.FirePropertyChanged(VcProperties.Opacity);
        }

        /// <summary>
        /// Event handler attached with MaxHeight property changed event of Legend element
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnMaxHeightPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Legend legend = d as Legend;
            legend.InternalMaxHeight = (Double)e.NewValue;
            legend.FirePropertyChanged(VcProperties.MaxHeight);
        }

        /// <summary>
        /// Event handler attached with MaxWidth property changed event of Legend element
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnMaxWidthPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Legend legend = d as Legend;
            legend.InternalMaxWidth = (Double)e.NewValue;
            legend.FirePropertyChanged(VcProperties.MaxWidth);
        }
        //----------------------------------------------

        /// <summary>
        /// Event handler attached with LightingEnabled property changed event of Legend element
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnLightingEnabledPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Legend legend = d as Legend;
            legend.FirePropertyChanged(VcProperties.LightingEnabled);
        }

        /// <summary>
        /// Event handler attached with ShadowEnabled property changed event of Legend element
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnShadowEnabledPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Legend legend = d as Legend;
            legend.FirePropertyChanged(VcProperties.ShadowEnabled);
        }

        /// <summary>
        /// Event handler attached with CornerRadius property changed event of Legend element
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnCornerRadiusPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Legend legend = d as Legend;
            legend.FirePropertyChanged(VcProperties.CornerRadius);
        }

        /// <summary>
        /// Event handler attached with Title property changed event of Legend element
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnTitlePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Legend legend = d as Legend;
            legend.FirePropertyChanged(VcProperties.Title);
        }

        /// <summary>
        /// Event handler attached with TitleAlignmentX property changed event of Legend element
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnTitleAlignmentXPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Legend legend = d as Legend;
            legend.FirePropertyChanged(VcProperties.TitleAlignmentX);
        }

        /// <summary>
        /// Event handler attached with TitleTextAlignment property changed event of Legend element
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnTitleTextAlignmentPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Legend legend = d as Legend;
            legend.FirePropertyChanged(VcProperties.TitleTextAlignment);
        }

        /// <summary>
        /// Event handler attached with TitleBackground property changed event of Legend element
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnTitleBackgroundPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Legend legend = d as Legend;
            legend.FirePropertyChanged(VcProperties.TitleBackground);
        }

        /// <summary>
        /// Event handler attached with TitleFontColor property changed event of Legend element
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnTitleFontColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Legend legend = d as Legend;
            legend.FirePropertyChanged(VcProperties.TitleFontColor);
        }

        /// <summary>
        /// Event handler attached with TitleFontFamily property changed event of Legend element
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnTitleFontFamilyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Legend legend = d as Legend;
            legend.FirePropertyChanged(VcProperties.TitleFontFamily);
        }

        /// <summary>
        /// Event handler attached with TitleFontStyle property changed event of Legend element
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnTitleFontStylePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Legend legend = d as Legend;
            legend.FirePropertyChanged(VcProperties.TitleFontStyle);
        }

        /// <summary>
        /// Event handler attached with TitleFontSize property changed event of Legend element
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnTitleFontSizePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Legend legend = d as Legend;
            legend.FirePropertyChanged(VcProperties.TitleFontSize);
        }

        /// <summary>
        /// Event handler attached with TitleFontWeight property changed event of Legend element
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnTitleFontWeightPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Legend legend = d as Legend;
            legend.FirePropertyChanged(VcProperties.TitleFontWeight);
        }

        /// <summary>
        /// Event handler attached with EntryMargin property changed event of Legend element
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnEntryMarginPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Legend legend = d as Legend;
            legend.FirePropertyChanged(VcProperties.EntryMargin);
        }


        /// <summary>
        /// Apply font properties of a TextBlock
        /// </summary>
        /// <param name="textBlock"></param>
        private void ApplyFontProperty(TextBlock textBlock)
        {
            textBlock.FontFamily = InternalFontFamily;
            textBlock.FontStyle = InternalFontStyle;
            textBlock.FontWeight = InternalFontWeight;
            textBlock.FontSize = InternalFontSize;
            textBlock.Foreground = Charts.Chart.CalculateFontColor((Chart as Chart), Background, FontColor, this.DockInsidePlotArea);
        }

        /// <summary>
        /// Apply font properties of a marker
        /// </summary>
        /// <param name="marker">Marker</param>
        private void ApplyFontPropertiesOfMarkerAsSymbol(Marker marker)
        {
            marker.FontFamily = InternalFontFamily;
            marker.FontStyle = InternalFontStyle;
            marker.FontWeight = InternalFontWeight;
            marker.FontSize = InternalFontSize;
            marker.FontColor = Charts.Chart.CalculateFontColor((Chart as Chart), Background, FontColor, this.DockInsidePlotArea);
        }

        /// <summary>
        /// Apply font properties of the title of Legend
        /// </summary>
        /// <param name="title"></param>
        private void ApplyFontProperty(Title title)
        {
            if (TitleFontFamily != null)
                title.InternalFontFamily = TitleFontFamily;

            if (TitleFontSize != 0)
                title.InternalFontSize = TitleFontSize;

            if (TitleFontStyle != null)
                title.InternalFontStyle = TitleFontStyle;

            if (TitleFontWeight != null)
                title.InternalFontWeight = TitleFontWeight;

            if (!String.IsNullOrEmpty(Title))
                title.Text = GetFormattedMultilineText(Title);

            title.InternalFontColor = Charts.Chart.CalculateFontColor((Chart as Chart), Background, TitleFontColor, this.DockInsidePlotArea);
        }

        /// <summary>
        /// Apply visual properties
        /// </summary>
        private void ApplyVisualProperty()
        {
            if (Cursor != null)
                Visual.Cursor = Cursor;

            Visual.BorderBrush = BorderColor;
            Visual.BorderThickness = InternalBorderThickness;

            Visual.CornerRadius = CornerRadius;
            Visual.Background = this.InternalBackground;

            Visual.HorizontalAlignment = InternalHorizontalAlignment;
            Visual.VerticalAlignment = InternalVerticalAlignment;
            Visual.Opacity = this.InternalOpacity;

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
        /// Apply lighting over Legend
        /// </summary>
        private void ApplyLighting()
        {
            if (LightingEnabled)
                LegendContainer.Background = Graphics.LightingBrush(LightingEnabled);
            else
                LegendContainer.Background = new SolidColorBrush(Colors.Transparent);
        }

        /// <summary>
        /// Apply shadow over Legend
        /// </summary>
        private void ApplyShadow(Grid innerGrid)
        {   
            if (ShadowEnabled)
            {   
                if (VisifireControl.IsXbapApp)
                {   
                    Grid shadowGrid = ExtendedGraphics.Get2DRectangleShadow(this, Visual.Width, Visual.Height,
                                                        CornerRadius, CornerRadius, 6);

                    shadowGrid.Clip = ExtendedGraphics.GetShadowClip(new Size(shadowGrid.Width , shadowGrid.Height), CornerRadius);
                    
                    shadowGrid.SetValue(Canvas.ZIndexProperty, -12);
                    shadowGrid.HorizontalAlignment = HorizontalAlignment.Stretch;
                    shadowGrid.VerticalAlignment = VerticalAlignment.Stretch;
                    // shadowGrid.Margin = new Thickness(0, 0, -(Charts.Chart.SHADOW_DEPTH + BorderThickness.Right + BorderThickness.Left), -(Charts.Chart.SHADOW_DEPTH + BorderThickness.Bottom + BorderThickness.Top));
                    shadowGrid.Margin = new Thickness(0, 0, -(Charts.Chart.SHADOW_DEPTH + EntryMargin), -(Charts.Chart.SHADOW_DEPTH + EntryMargin));

                    innerGrid.Children.Insert(0,shadowGrid);
                }
                else
                {   
                    DropShadowEffect shadow = new DropShadowEffect()
                    {
                        BlurRadius = 5,
                        Direction = 315,
                        ShadowDepth = 4,
                        Opacity = 0.95,
#if WPF
                        Color = Color.FromArgb((Byte)255, (Byte)185, (Byte)185, (Byte)185)
#else
                    Color = Color.FromArgb((Byte)255, (Byte)135, (Byte)135, (Byte)135)
#endif
                    };

                    Visual.Effect = shadow;
                }
            }
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

            foreach (LegendEntry labelAndSymbol in Entries)
            {
                TextBlock t = new TextBlock();
                t.Text = labelAndSymbol.Labels[0];
                ApplyFontProperty(t);
                Size labelSize = TextBlockActualSize(t);
                entrySize.TextSize.Width = (labelSize.Width > entrySize.TextSize.Width) ? labelSize.Width : entrySize.TextSize.Width;
                entrySize.TextSize.Height = (labelSize.Height > entrySize.TextSize.Height) ? labelSize.Height : entrySize.TextSize.Height;
                (labelAndSymbol.Marker as Marker).Margin = EntryMargin;

                (labelAndSymbol.Marker as Marker).CreateVisual();
                entrySize.SymbolSize.Width = ((labelAndSymbol.Marker as Marker).MarkerActualSize.Width > entrySize.SymbolSize.Width) ? (labelAndSymbol.Marker as Marker).MarkerActualSize.Width : entrySize.SymbolSize.Width;
                entrySize.SymbolSize.Height = ((labelAndSymbol.Marker as Marker).MarkerActualSize.Height > entrySize.SymbolSize.Height) ? (labelAndSymbol.Marker as Marker).MarkerActualSize.Height : entrySize.SymbolSize.Height;
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
            Line line = new Line() { Tag = marker.Tag };

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

            if (!(InternalVerticalAlignment == VerticalAlignment.Center && (InternalHorizontalAlignment == HorizontalAlignment.Left || InternalHorizontalAlignment == HorizontalAlignment.Right)))
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

            foreach (LegendEntry labelAndSymbol in Entries)
            {
                Marker markerAsSymbol = labelAndSymbol.Marker;
                markerAsSymbol.Margin = EntryMargin;
                markerAsSymbol.LabelMargin = LabelMargin;
                markerAsSymbol.Text = labelAndSymbol.Labels[0];

                markerAsSymbol.TextAlignmentY = AlignmentY.Center;
                markerAsSymbol.TextAlignmentX = AlignmentX.Right;

                ApplyFontPropertiesOfMarkerAsSymbol(markerAsSymbol);

                if (markerAsSymbol.DataSeriesOfLegendMarker.RenderAs == RenderAs.Line
                    || markerAsSymbol.DataSeriesOfLegendMarker.RenderAs == RenderAs.StepLine
                    || markerAsSymbol.DataSeriesOfLegendMarker.RenderAs == RenderAs.Stock
                    || markerAsSymbol.DataSeriesOfLegendMarker.RenderAs == RenderAs.CandleStick
                    )
                {
                    markerAsSymbol.BorderColor = markerAsSymbol.MarkerFillColor;
                    markerAsSymbol.MarkerFillColor = new SolidColorBrush(Colors.White);
                    markerAsSymbol.BorderThickness = 0.7;

                    markerAsSymbol.CreateVisual();

                    Canvas lineMarker = GetNewMarkerForLineChart(markerAsSymbol);

                    lineMarker.HorizontalAlignment = HorizontalAlignment.Left;
                    lineMarker.VerticalAlignment = VerticalAlignment.Center;

                    if ((currentHeight + lineMarker.Height) <= InternalMaximumHeight)
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

                    if ((currentHeight + markerAsSymbol.MarkerActualSize.Height) <= InternalMaximumHeight)
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
        private void DrawVerticalGridlayout4Legend(ref Grid legendContent, out ScrollViewer scrollViewer)
        {
            DrawHorizontalGridlayout4Legend(ref legendContent, out scrollViewer);
        }

        //private void DrawVerticalGridlayout4Legend(ref Grid legendContent)
        //{
        //    Int32 row, column;
        //    Grid legendGrid = new Grid();

        //    EntrySize maxEntrySize = GetMaxSymbolAndColumnWidth();

        //    MaxRows = (Int32)(InternalMaximumHeight / (maxEntrySize.SymbolSize.Height + maxEntrySize.TextSize.Height + EntryMargin + LabelMargin));

        //    MaxColumns = (Int32)Math.Ceiling(((Double)Entries.Count / MaxRows));

        //    for (row = 0; row < MaxRows; row++)
        //        legendGrid.RowDefinitions.Add(new RowDefinition());

        //    row = 0;
        //    column = 0;

        //    Double maxRowHeight = 0;

        //    foreach (LegendEntry labelAndSymbol in Entries)
        //    {
        //        if (row == 0)
        //        {
        //            legendGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(maxEntrySize.SymbolSize.Width + EntryMargin + LabelMargin / 2) });
        //            legendGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(maxEntrySize.TextSize.Width + LabelMargin / 2) });
        //        }

        //        (labelAndSymbol.Marker as Marker).Visual.Margin = new Thickness(EntryMargin, EntryMargin, LabelMargin / 2, EntryMargin);
        //        (labelAndSymbol.Marker as Marker).Visual.SetValue(Grid.RowProperty, row);
        //        (labelAndSymbol.Marker as Marker).Visual.SetValue(Grid.ColumnProperty, column++);

        //        legendGrid.Children.Add((labelAndSymbol.Marker as Marker).Visual);

        //        TextBlock label = new TextBlock();
        //        label.Margin = new Thickness(LabelMargin, 0, 0, 0);

        //        label.Text = labelAndSymbol.Labels[0];
        //        ApplyFontProperty(label);
        //        label.SetValue(Grid.RowProperty, row);
        //        label.SetValue(Grid.ColumnProperty, column++);
        //        label.HorizontalAlignment = HorizontalAlignment.Left;
        //        label.VerticalAlignment = VerticalAlignment.Center;
        //        legendGrid.Children.Add(label);
        //        label.Measure(new Size(Double.MaxValue, Double.MaxValue));

        //        Double maxRowHeight1 = (label.DesiredSize.Height > (labelAndSymbol.Marker as Marker).Visual.DesiredSize.Height) ? label.DesiredSize.Height : (labelAndSymbol.Marker as Marker).Visual.DesiredSize.Height;

        //        if (maxRowHeight1 > maxRowHeight)
        //        {
        //            maxRowHeight = maxRowHeight1;
        //            legendGrid.RowDefinitions[row].Height = new GridLength(maxRowHeight + 2 * EntryMargin);
        //        }

        //        if (column >= MaxColumns * 2)
        //        {
        //            row++;
        //            column = 0;
        //        }
        //    }

        //    legendGrid.ShowGridLines = true;
        //    legendGrid.HorizontalAlignment = HorizontalAlignment.Center;
        //    legendGrid.VerticalAlignment = VerticalAlignment.Center;

        //    legendContent.Children.Add(legendGrid);
        //}

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

            foreach (LegendEntry labelAndSymbol in Entries)
            {
                Marker marker = labelAndSymbol.Marker;
                marker.Margin = EntryMargin;
                marker.LabelMargin = LabelMargin;
                marker.Text = labelAndSymbol.Labels[0];
                ApplyFontPropertiesOfMarkerAsSymbol(marker);

                marker.TextAlignmentY = AlignmentY.Center;
                marker.TextAlignmentX = AlignmentX.Right;


                if (marker.DataSeriesOfLegendMarker.RenderAs == RenderAs.Line
                    || marker.DataSeriesOfLegendMarker.RenderAs == RenderAs.StepLine
                    || marker.DataSeriesOfLegendMarker.RenderAs == RenderAs.Stock
                    || marker.DataSeriesOfLegendMarker.RenderAs == RenderAs.CandleStick
                    )
                {
                    marker.BorderColor = marker.MarkerFillColor;
                    marker.MarkerFillColor = new SolidColorBrush(Colors.White);
                    marker.BorderThickness = 0.7;

                    marker.LabelMargin += ENTRY_SYMBOL_LINE_WIDTH / 2;

                    marker.CreateVisual();

                    Canvas lineMarker = GetNewMarkerForLineChart(marker);

                    //if (marker.DataSeriesOfLegendMarker.MarkerEnabled == false)
                    //    marker.MarkerShape.Opacity = 0;

                    if ((currentWidth + lineMarker.Width) <= InternalMaximumWidth)
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

                    if ((currentWidth + marker.MarkerActualSize.Width) <= InternalMaximumWidth)
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
        private void DrawHorizontalGridlayout4Legend(ref Grid legendContent, out ScrollViewer scrollViewer)
        {   
            Grid legendGrid = new Grid();
            scrollViewer = new ScrollViewer();
            scrollViewer.BorderThickness = new Thickness(0);

            if (Entries.Count > 0)
            {   
                // Check number of columns possible
                Int32 totalNumberOfColumn = (from le in Entries where le.Labels != null select le.Labels.Count()).Max() + 1; // 1 for Legend

                // Create required numbers of column
                for (Int32 i = 0; i < totalNumberOfColumn; i++)
                    legendGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });

                //Create required numbers of row
                for (Int32 i = 1; i <= Entries.Count; i++)
                    legendGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

                Int32 rowIndex = 0;

                List<Rectangle> linesAsRect = new List<Rectangle>();
                
                foreach (LegendEntry legendEntry in Entries)
                {   
                    Int32 columnIndex = 0;

                    if (legendEntry.IsCompleteLine)
                    {   
                        Rectangle rectAsLine = new Rectangle()
                        {   
                            Height = 1,
                            HorizontalAlignment = HorizontalAlignment.Stretch,
                            Fill = new SolidColorBrush(Colors.Black)
                        };
                        
                        linesAsRect.Add(rectAsLine);

                        rectAsLine.SetValue(Grid.RowProperty, rowIndex);
                        legendGrid.Children.Add(rectAsLine);
                    }
                    else
                    {   
                        Marker marker = legendEntry.Marker;

                        // Create the first column for Marker
                        if (marker != null)
                        {   
                            marker.CreateVisual();
                            marker.Visual.Margin = new Thickness(EntryMargin, EntryMargin, LabelMargin / 2, EntryMargin);
                            marker.Visual.SetValue(Grid.RowProperty, rowIndex);
                            marker.Visual.SetValue(Grid.ColumnProperty, columnIndex);
                            marker.Visual.VerticalAlignment = VerticalAlignment.Center;
                            legendGrid.Children.Add(marker.Visual);
                        }

                        VerticalAlignment verticalAlignment = VerticalAlignment.Center;
                        List<TextBlock> labels = new List<TextBlock>();
                        Int32 labelIndex = 0;

                        // Create one or more column for labels
                        foreach (String labelText in legendEntry.Labels)
                        {
                            TextBlock label = new TextBlock();
                            labels.Add(label);

                            columnIndex++;

                            if (legendEntry.XAlignments[labelIndex] != HorizontalAlignment.Left)
                                label.Margin = new Thickness(LabelMargin, 0, 0, 0);

                            label.Text = labelText;
                            ApplyFontProperty(label);
                            label.SetValue(Grid.RowProperty, rowIndex);
                            label.SetValue(Grid.ColumnProperty, columnIndex);
                            label.HorizontalAlignment = legendEntry.XAlignments[labelIndex];
                            label.VerticalAlignment = VerticalAlignment.Center;
                            legendGrid.Children.Add(label);

                            // Align the Marker at top if any labels included NewLine '\n'
                            if (labelText.Contains('\n'))
                                verticalAlignment = VerticalAlignment.Top;

                            labelIndex++;
                        }

                        // Update verticalAlignment for all column items
                        if (marker != null)
                            marker.Visual.VerticalAlignment = verticalAlignment;

                        foreach (TextBlock label in labels)
                            label.VerticalAlignment = verticalAlignment;
                    }
                    
                    // Next row
                    rowIndex++;
                }

                foreach(Rectangle rectAsLine in linesAsRect)
                   rectAsLine.SetValue(Grid.ColumnSpanProperty, totalNumberOfColumn);

            }

            //legendGrid.ShowGridLines = true;
            legendGrid.HorizontalAlignment = HorizontalAlignment.Center;
            legendGrid.VerticalAlignment = VerticalAlignment.Center;
            scrollViewer.Content = legendGrid;
            scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;
            scrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled;
            legendContent.Children.Add(scrollViewer);
        }

        //private void DrawHorizontalGridlayout4Legend(ref Grid legendContent)
        //{
        //    Int32 row, column;
        //    Grid legendGrid = new Grid();

        //    EntrySize maxEntrySize = GetMaxSymbolAndColumnWidth();

        //    MaxColumns = (Int32)(InternalMaximumWidth / (maxEntrySize.SymbolSize.Width + maxEntrySize.TextSize.Width + EntryMargin + LabelMargin));

        //    MaxRows = (Int32)Math.Ceiling(((Double)Entries.Count / MaxColumns));

        //    for (row = 0; row < MaxRows; row++)
        //        legendGrid.RowDefinitions.Add(new RowDefinition());

        //    row = 0;
        //    column = 0;

        //    Double maxRowHeight = 0;

        //    foreach (LegendEntry labelAndSymbol in Entries)
        //    {   
        //        if (row == 0)
        //        {
        //            legendGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(maxEntrySize.SymbolSize.Width + EntryMargin + LabelMargin / 2) });
        //            legendGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(maxEntrySize.TextSize.Width + LabelMargin / 2) });
        //        }

        //        (labelAndSymbol.Marker as Marker).Visual.Margin = new Thickness(EntryMargin, EntryMargin, LabelMargin / 2, EntryMargin);
        //        (labelAndSymbol.Marker as Marker).Visual.SetValue(Grid.RowProperty, row);
        //        (labelAndSymbol.Marker as Marker).Visual.SetValue(Grid.ColumnProperty, column++);

        //        legendGrid.Children.Add((labelAndSymbol.Marker as Marker).Visual);

        //        TextBlock label = new TextBlock();
        //        label.Margin = new Thickness(LabelMargin, 0, 0, 0);

        //        label.Text = labelAndSymbol.Labels[0];
        //        ApplyFontProperty(label);
        //        label.SetValue(Grid.RowProperty, row);
        //        label.SetValue(Grid.ColumnProperty, column++);
        //        label.HorizontalAlignment = HorizontalAlignment.Left;
        //        label.VerticalAlignment = VerticalAlignment.Center;
        //        legendGrid.Children.Add(label);
        //        label.Measure(new Size(Double.MaxValue, Double.MaxValue));

        //        Double maxRowHeight1 = (label.DesiredSize.Height > (labelAndSymbol.Marker as Marker).Visual.DesiredSize.Height) ? label.DesiredSize.Height : (labelAndSymbol.Marker as Marker).Visual.DesiredSize.Height;

        //        if (maxRowHeight1 > maxRowHeight)
        //        {
        //            maxRowHeight = maxRowHeight1;
        //            legendGrid.RowDefinitions[row].Height = new GridLength(maxRowHeight + 2 * EntryMargin);
        //        }

        //        if (column >= MaxColumns * 2)
        //        {
        //            row++;
        //            column = 0;
        //        }
        //    }

        //    legendGrid.ShowGridLines = true;
        //    legendGrid.HorizontalAlignment = HorizontalAlignment.Center;
        //    legendGrid.VerticalAlignment = VerticalAlignment.Center;

        //    legendContent.Children.Add(legendGrid);
        //}

        /// <summary>
        /// Create the content of the Legend
        /// </summary>
        /// <returns>Grid</returns>
        private Grid CreateLegendContent()
        {   
            Grid legendContent = new Grid();
            ScrollViewer scrollViewer = null;

            InternalMaximumWidth -= 2 * InternalPadding.Left;
            InternalMaximumHeight -= 2 * InternalPadding.Left;

            if (Orientation == Orientation.Vertical)
            {   
                if (Layout == Layouts.FlowLayout)
                {
                    DrawVerticalFlowLayout4Legend(ref legendContent);
                }
                else if (Layout == Layouts.GridLayout) // MaxWidth is reqired for GridLayout calculation
                {
                    DrawVerticalGridlayout4Legend(ref legendContent, out scrollViewer);
                }
            }
            else if (Orientation == Orientation.Horizontal)
            {
                if (Layout == Layouts.FlowLayout)
                {   
                    DrawHorizontalFlowLayout4Legend(ref legendContent);
                }
                else if (Layout == Layouts.GridLayout) // MaxWidth is reqired for GridLayout calculation
                {
                    DrawHorizontalGridlayout4Legend(ref legendContent, out scrollViewer);
                }
            }

            legendContent.Measure(new Size(Double.MaxValue, Double.MaxValue));
            legendContent.Height = legendContent.DesiredSize.Height + InternalPadding.Left * 2;
            legendContent.Width = legendContent.DesiredSize.Width + InternalPadding.Left * 2;

            if (Layout == Layouts.GridLayout)
            {   
                Double maxHeight, maxWidth;

                if (Double.IsInfinity(InternalMaximumWidth))
                    maxWidth = (Chart as Chart).ActualWidth * 0.5;
                else
                    maxWidth = InternalMaximumWidth;

                                if (Double.IsInfinity(InternalMaximumHeight))
                    maxHeight = (Chart as Chart).ActualHeight * 0.5;
                else
                    maxHeight = InternalMaximumHeight;

                if (legendContent.Width > maxWidth)
                {
                    scrollViewer.Width = maxWidth - InternalPadding.Left * 2;
                    legendContent.Width = maxWidth;
                    scrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Visible;
                }
 
                if (legendContent.Height > maxHeight)
                {   
                    scrollViewer.Height = maxHeight - InternalPadding.Left * 2;
                    scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
                    legendContent.Height = maxHeight;
                }

                if (scrollViewer.VerticalScrollBarVisibility == ScrollBarVisibility.Visible)
                {
                    if (legendContent.Width + SCROLLBAR_SIZE_OF_SCROLLVIEWER < maxWidth)
                    {
                        legendContent.Width += SCROLLBAR_SIZE_OF_SCROLLVIEWER;

                        if(!Double.IsNaN(scrollViewer.Width))
                            scrollViewer.Width += SCROLLBAR_SIZE_OF_SCROLLVIEWER;
                    }
                }

                if (scrollViewer.HorizontalScrollBarVisibility == ScrollBarVisibility.Visible)
                {
                    if (legendContent.Height + SCROLLBAR_SIZE_OF_SCROLLVIEWER < maxHeight)
                    {
                        legendContent.Height += SCROLLBAR_SIZE_OF_SCROLLVIEWER;

                        if (!Double.IsNaN(scrollViewer.Height))
                            scrollViewer.Height += SCROLLBAR_SIZE_OF_SCROLLVIEWER;
                    }
                }

            }

            return legendContent;
        }

        #endregion

        #region Internal Methods

#if WPF
        internal override void FireMouseRightButtonDownEvent(object sender, object e)
        {
            if (_onMouseRightButtonDown != null)
                _onMouseRightButtonDown(sender, new LegendMouseButtonEventArgs(e as MouseButtonEventArgs));
        }

        internal override void FireMouseRightButtonUpEvent(object sender, object e)
        {
            if (_onMouseRightButtonUp != null)
                _onMouseRightButtonUp(sender, new LegendMouseButtonEventArgs(e as MouseButtonEventArgs));
        }
#endif

        internal override void FireMouseLeftButtonDownEvent(object sender, object e)
        {
            if (_onMouseLeftButtonDown != null)
                _onMouseLeftButtonDown(sender, new LegendMouseButtonEventArgs(e as MouseButtonEventArgs));
        }

        internal override void FireMouseLeftButtonUpEvent(object sender, object e)
        {
            if (_onMouseLeftButtonUp != null)
                _onMouseLeftButtonUp(sender, new LegendMouseButtonEventArgs(e as MouseButtonEventArgs));
        }

        internal override void FireMouseMoveEvent(object sender, object e)
        {
            if (_onMouseMove != null)
                _onMouseMove(sender, new LegendMouseEventArgs(e as MouseEventArgs));
        }
        
        internal override object GetMouseLeftButtonDownEventHandler()
        {
            return _onMouseLeftButtonDown;
        }

        internal override object GetMouseLeftButtonUpEventHandler()
        {
            return _onMouseLeftButtonUp;
        }

        internal override object GetMouseMoveEventHandler()
        {
            return _onMouseMove;
        }

#if WPF
        internal override object GetMouseRightButtonDownEventHandler()
        {
            return _onMouseRightButtonDown;
        }

        internal override object GetMouseRightButtonUpEventHandler()
        {
            return _onMouseRightButtonUp;
        }
#endif



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

            ElementData tag = new ElementData { Element = this, VisualElementName = "Legend" };

            Visual = new Border() { Tag = tag };
            Grid innerGrid = new Grid() { Tag = tag };

            (Visual as Border).Child = innerGrid;

            LegendContainer = new StackPanel() { Tag = tag };

            if (!String.IsNullOrEmpty(Title))
            {
                Title legendTitle = new Title();
                ApplyFontProperty(legendTitle);

                if (TitleBackground != null)
                    legendTitle.InternalBackground = TitleBackground;

                legendTitle.InternalHorizontalAlignment = TitleAlignmentX;
                legendTitle.InternalVerticalAlignment = VerticalAlignment.Top;
                legendTitle.TextAlignment = TitleTextAlignment;

                legendTitle.CreateVisualObject(tag);

                legendTitle.Measure(new Size(Double.MaxValue, Double.MaxValue));

                if (legendTitle.DesiredSize.Width > InternalMaxWidth)
                    legendTitle.Visual.Width = InternalMaxWidth;

                LegendContainer.Children.Add(legendTitle.Visual);
            }

            Grid legendContent = CreateLegendContent();
            legendContent.Tag = tag;

            LegendContainer.Children.Add(legendContent);

            LegendContainer.VerticalAlignment = VerticalAlignment.Center;
            LegendContainer.HorizontalAlignment = HorizontalAlignment.Stretch;

            ApplyVisualProperty();

            innerGrid.Children.Add(LegendContainer);

            Visual.Cursor = this.Cursor;
            Visual.Measure(new Size(Double.MaxValue, Double.MaxValue));

            if (!Double.IsPositiveInfinity(InternalMaxHeight) && InternalMaxHeight < Visual.DesiredSize.Height)
                Visual.Height = InternalMaxHeight;
            else 
                Visual.Height = Visual.DesiredSize.Height;

            if (!Double.IsPositiveInfinity(InternalMaxWidth) && InternalMaxWidth < Visual.DesiredSize.Width + InternalPadding.Left)
                Visual.Width = InternalMaxWidth;
            else if (Layout == Layouts.GridLayout)
                Visual.Width = Visual.DesiredSize.Width + InternalPadding.Left;
            else
                Visual.Width = Visual.DesiredSize.Width;

            PlotArea plotArea = (Chart as Chart).PlotArea;

            RectangleGeometry rectGeo = new RectangleGeometry();
            rectGeo.Rect = new Rect(InternalBorderThickness.Left, InternalBorderThickness.Top, Visual.Width - InternalBorderThickness.Left - InternalBorderThickness.Right, Visual.Height - InternalBorderThickness.Top - InternalBorderThickness.Bottom);
            rectGeo.RadiusX = CornerRadius.TopLeft;
            rectGeo.RadiusY = CornerRadius.TopRight;
            LegendContainer.Clip = rectGeo;

            ApplyShadow(innerGrid);

            if (VerticalAlignment == System.Windows.VerticalAlignment.Bottom && (HorizontalAlignment == System.Windows.HorizontalAlignment.Center
                || HorizontalAlignment == System.Windows.HorizontalAlignment.Left || HorizontalAlignment == System.Windows.HorizontalAlignment.Right
                || HorizontalAlignment == System.Windows.HorizontalAlignment.Stretch))
                Visual.Margin = new Thickness(0, DEFAULT_MARGIN, 0, 0);
            else if (VerticalAlignment == System.Windows.VerticalAlignment.Top && (HorizontalAlignment == System.Windows.HorizontalAlignment.Center
                || HorizontalAlignment == System.Windows.HorizontalAlignment.Left || HorizontalAlignment == System.Windows.HorizontalAlignment.Right
                || HorizontalAlignment == System.Windows.HorizontalAlignment.Stretch))
                Visual.Margin = new Thickness(0, 0, 0, DEFAULT_MARGIN);
            else if (HorizontalAlignment == System.Windows.HorizontalAlignment.Left && (VerticalAlignment == System.Windows.VerticalAlignment.Center
                || VerticalAlignment == System.Windows.VerticalAlignment.Stretch))
                Visual.Margin = new Thickness(0, 0, DEFAULT_MARGIN, 0);
            else if (HorizontalAlignment == System.Windows.HorizontalAlignment.Right && (VerticalAlignment == System.Windows.VerticalAlignment.Center
                || VerticalAlignment == System.Windows.VerticalAlignment.Stretch))
                Visual.Margin = new Thickness(DEFAULT_MARGIN, 0, 0, 0);
        }

        #endregion

        #region Internal Events And Delegates

        /// <summary>
        /// EventChanged event is fired if any event is attached
        /// </summary>
        internal new event EventHandler EventChanged;

        #endregion

        #region Data

        private const Double DEFAULT_MARGIN = 3.5;

        /// <summary>
        /// Handler for MouseLeftButtonDown event
        /// </summary>
        private event EventHandler<LegendMouseButtonEventArgs> _onMouseLeftButtonDown;

        /// <summary>
        /// Handler for MouseLeftButtonUp event
        /// </summary>
        private event EventHandler<LegendMouseButtonEventArgs> _onMouseLeftButtonUp;
        
        /// <summary>
        /// Handler for MouseMove event
        /// </summary>
        private event EventHandler<LegendMouseEventArgs> _onMouseMove;

#if WPF
        /// <summary>
        /// Handler for MouseRightButtonDown event
        /// </summary>
        private event EventHandler<LegendMouseButtonEventArgs> _onMouseRightButtonDown;

        /// <summary>
        /// Handler for MouseRightButtonUp event
        /// </summary>
        private event EventHandler<LegendMouseButtonEventArgs> _onMouseRightButtonUp;

#endif

        private const Double SCROLLBAR_SIZE_OF_SCROLLVIEWER = 18;
        private const Double ENTRY_SYMBOL_LINE_WIDTH = 18;
        private Double _internalFontSize = Double.NaN;
        private FontFamily _internalFontFamily = null;
        private Nullable<FontStyle> _internalFontStyle = null;
        private Nullable<FontWeight> _internalFontWeight = null;
        private Nullable<Thickness> _borderThickness = null;
        private Brush _internalBackground = null;
        private Nullable<HorizontalAlignment> _internalHorizontalAlignment = null;
        
        private Nullable<VerticalAlignment> _internalVerticalAlignment = null;
        private Nullable<Thickness> _internalPadding = null;
        private Double _internalOpacity = Double.NaN;
        private Double _internalMaxheight = Double.NaN;
        private Double _internalMaxWidth = Double.NaN;
        internal Brush InternalFontColor;

#if WPF

        /// <summary>
        /// Whether the default style is applied
        /// </summary>
        private static Boolean _defaultStyleKeyApplied;         
 
#endif


        #endregion
    }

    internal class LegendEntry
    {
        public LegendEntry(Marker marker, List<String> labels, List<HorizontalAlignment> xAlignments)
        {   
            Marker = marker;
            Labels = labels;
            XAlignments = xAlignments;
        }

        public LegendEntry() { }
            
        public Boolean IsCompleteLine;
        public Marker Marker;
        public List<String> Labels;
        public List<HorizontalAlignment> XAlignments;
    }
}