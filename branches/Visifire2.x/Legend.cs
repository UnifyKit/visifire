#if WPF
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.ComponentModel;

#else

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Collections.Generic;


#endif
using Visifire.Commons;

namespace Visifire.Charts
{
#if SL
    [System.Windows.Browser.ScriptableType]
#endif
    public class Legend : ObservableObject
    {
        public Legend()
        {
#if WPF
            if (!_defaultStyleKeyApplied)
            {
                DefaultStyleKeyProperty.OverrideMetadata(typeof(Legend), new FrameworkPropertyMetadata(typeof(Legend)));
                _defaultStyleKeyApplied = true;
            } 

            //object dsp = this.GetValue(FrameworkElement.DefaultStyleKeyProperty);
            //Style = (Style)Application.Current.FindResource(dsp);
#else
            DefaultStyleKey = typeof(Legend);
#endif      

            Entries = new List<KeyValuePair<String, Marker>>();

            SetDefaults();
        }
        
        private void SetDefaults()
        {

        }

        internal Border Visual
        {
            get;
            set;
        }
 
        internal LegendLayouts LegendLayout
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
            typeof(Legend),
            new PropertyMetadata(OnHrefChanged));

        private static void OnHrefTargetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Legend c = d as Legend;
            c.FirePropertyChanged("HrefTarget");
        }

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

        public static readonly DependencyProperty HrefProperty = DependencyProperty.Register
            ("Href",
            typeof(String),
            typeof(Legend),
            new PropertyMetadata(OnHrefChanged));

        private static void OnHrefChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Legend c = d as Legend;
            c.FirePropertyChanged("Href");
        }

        /// <summary>
        /// Set the maximum height of the legend.
        /// </summary>
        public Double MaximumWidth
        {
            get
            {
                return (Double)GetValue(MaxWidthProperty);
            }
            set
            {
                SetValue(MaxWidthProperty, value);
            }
        }

        public static readonly DependencyProperty MaximumWidthProperty = DependencyProperty.Register
        ("MaximumWidth",
        typeof(Double),
        typeof(Legend),
        new PropertyMetadata(OnMaxWidthPropertyChanged));

        private static void OnMaxWidthPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Legend legend = d as Legend;
            legend.FirePropertyChanged("MaxWidth");
        }

        public Double MaximumHeight
        {
            get
            {
                return (Double)GetValue(MaxHeightProperty);
            }
            set
            {
                SetValue(MaxHeightProperty, value);
            }
        }

        public static readonly DependencyProperty MaximumHeightProperty = DependencyProperty.Register
        ("MaximumHeight",
        typeof(Double),
        typeof(Legend),
        new PropertyMetadata(OnMaxHeightPropertyChanged));

        private static void OnMaxHeightPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Legend legend = d as Legend;
            legend.FirePropertyChanged("MaximumHeight");
        }

        internal Int32 MaxRows
        {
            get
            {
                return (Int32)GetValue(MaxRowsProperty);
            }
            set
            {
                SetValue(MaxRowsProperty, value);
            }
        }

        public static readonly DependencyProperty MaxRowsProperty = DependencyProperty.Register
        ("MaxRows",
        typeof(Int32),
        typeof(Legend),
        new PropertyMetadata(OnMaxRowsPropertyChanged));

        private static void OnMaxRowsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Legend legend = d as Legend;
            legend.FirePropertyChanged("MaxRows");
        }

        internal Int32 MaxColumns
        {
            get
            {
                return (Int32)GetValue(MaxColumnsProperty);
            }
            set
            {
                SetValue(MaxColumnsProperty, value);
            }
        }

        public static readonly DependencyProperty MaxColumnsProperty = DependencyProperty.Register
        ("MaxColumns",
        typeof(Int32),
        typeof(Legend),
        new PropertyMetadata(OnMaxColumnsPropertyChanged));

        private static void OnMaxColumnsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Legend legend = d as Legend;
            legend.FirePropertyChanged("MaxColumns");
        }

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

        public static readonly DependencyProperty LabelMarginProperty = DependencyProperty.Register
        ("LabelMargin",
        typeof(Double),
        typeof(Legend),
        new PropertyMetadata(OnLabelMarginPropertyChanged));

        private static void OnLabelMarginPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Legend legend = d as Legend;
            legend.FirePropertyChanged("LabelMargin");
        }

        public new Thickness Padding
        {
            get
            {
                return (Thickness)GetValue(PaddingProperty);
            }
            set
            {
                SetValue(PaddingProperty, value);
            }
        }

        public new static readonly DependencyProperty PaddingProperty = DependencyProperty.Register
        ("Padding",
        typeof(Thickness),
        typeof(Legend),
        new PropertyMetadata(OnPaddingPropertyChanged));

        private static void OnPaddingPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Legend legend = d as Legend;
            legend.FirePropertyChanged("Padding");
        }

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

#if WPF

        public new static readonly DependencyProperty HorizontalAlignmentProperty = DependencyProperty.Register
        ("HorizontalAlignment",
        typeof(HorizontalAlignment),
        typeof(Legend),
        new PropertyMetadata(OnHorizontalAlignmentPropertyChanged));

        private static void OnHorizontalAlignmentPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Legend legend = d as Legend;
            legend.FirePropertyChanged("HorizontalAlignment");
        }

#endif

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

#if WPF
        public new static readonly DependencyProperty VerticalAlignmentProperty = DependencyProperty.Register
        ("VerticalAlignment",
        typeof(VerticalAlignment),
        typeof(Legend),
        new PropertyMetadata(OnVerticalAlignmentPropertyChanged));

        private static void OnVerticalAlignmentPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Legend legend = d as Legend;
            legend.FirePropertyChanged("VerticalAlignment");
        }

#endif

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
        typeof(Legend),
        new PropertyMetadata(OnBorderColorPropertyChanged));

        private static void OnBorderColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {   
            Legend legend = d as Legend;
            legend.FirePropertyChanged("BorderColor");
        }   

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
        typeof(Legend),
        new PropertyMetadata(OnBorderStylePropertyChanged));

        private static void OnBorderStylePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Legend legend = d as Legend;
            legend.FirePropertyChanged("BorderStyle");
        }

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
#if WPF
        public new static readonly DependencyProperty BorderThicknessProperty = DependencyProperty.Register
        ("BorderThickness",
        typeof(Thickness),
        typeof(Legend),
        new PropertyMetadata(OnBorderThicknessPropertyChanged));

        private static void OnBorderThicknessPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Legend legend = d as Legend;
            legend.FirePropertyChanged("BorderThickness");
        }
#endif

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

#if WPF
        private new static readonly DependencyProperty BackgroundProperty = DependencyProperty.Register
            ("Background",
            typeof(Brush),
            typeof(Legend),
            new PropertyMetadata(OnBackgroundPropertyChanged));

        private static void OnBackgroundPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Legend legend = d as Legend;
            legend.FirePropertyChanged("Background");
        }
#endif

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

        public static readonly DependencyProperty DockInsidePlotAreaProperty = DependencyProperty.Register
        ("DockInsidePlotArea",
        typeof(Boolean),
        typeof(Legend),
        new PropertyMetadata(OnDockInsidePlotAreaPropertyChanged));

        private static void OnDockInsidePlotAreaPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Legend legend = d as Legend;
            legend.FirePropertyChanged("DockInsidePlotArea");
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
            typeof(Legend),
            new PropertyMetadata(OnEnabledPropertyChanged));

        private static void OnEnabledPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Legend legend = d as Legend;
            legend.FirePropertyChanged("Enabled");
        }
        /// <summary>
        /// Property FontColor
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

        public static readonly DependencyProperty FontColorProperty = DependencyProperty.Register
            ("FontColor",
            typeof(Brush),
            typeof(Legend),
            new PropertyMetadata(OnFontColorPropertyChanged));

        private static void OnFontColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Legend legend = d as Legend;
            legend.FirePropertyChanged("FontColor");
        }

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
#if WPF
        private new static readonly DependencyProperty FontFamilyProperty = DependencyProperty.Register
            ("FontFamily",
            typeof(FontFamily),
            typeof(Legend),
            new PropertyMetadata(OnFontFamilyPropertyChanged));

        private static void OnFontFamilyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Legend legend = d as Legend;
            legend.FirePropertyChanged("FontFamily");
        }
#endif
        

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

#if WPF
        internal new static readonly DependencyProperty FontSizeProperty = DependencyProperty.Register
            ("FontSize",
            typeof(Double),
            typeof(Legend),
            new PropertyMetadata(OnFontSizePropertyChanged));
            
        private static void OnFontSizePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Legend legend = d as Legend;
            legend.FirePropertyChanged("FontSize");
        }
#endif


#if SL
       
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
#endif

#if WPF

        [TypeConverter(typeof(System.Windows.FontStyleConverter))]
        internal FontStyle InternalFontStyle
        {
            get
            {
                return (FontStyle)(GetValue(InternalFontStyleProperty));
            }
            set
            {
                SetValue(InternalFontStyleProperty, value);
            }
        }

        private static readonly DependencyProperty InternalFontStyleProperty = DependencyProperty.Register
            ("InternalFontStyle",
            typeof(FontStyle),
            typeof(Legend),
            new PropertyMetadata(OnFontStylePropertyChanged));

        private static void OnFontStylePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Legend legend = d as Legend;
            legend.FirePropertyChanged("FontStyle");
        }
#endif




#if SL

       
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
                    SetValue(FontWeightProperty, value);
                    FirePropertyChanged("FontWeight");
                }
#else
                SetValue(FontWeightProperty, value);
#endif
            }
        }

#else
        [System.ComponentModel.TypeConverter(typeof(System.Windows.FontWeightConverter))]
        internal FontWeight InternalFontWeight
        {   
            get
            {
                return (FontWeight)(GetValue(InternalFontWeightProperty));
            }
            set
            {
                SetValue(InternalFontWeightProperty, value);
            }
        }

        private static readonly DependencyProperty InternalFontWeightProperty = DependencyProperty.Register
            ("InternalFontWeight",
            typeof(FontWeight),
            typeof(Legend),
            new PropertyMetadata(OnFontWeightPropertyChanged));

        private static void OnFontWeightPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Legend legend = d as Legend;
            legend.FirePropertyChanged("FontWeight");
        }
#endif


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

        public static readonly DependencyProperty LightingEnabledProperty = DependencyProperty.Register
        ("LightingEnabled",
        typeof(Boolean),
        typeof(Legend),
        new PropertyMetadata(OnLightingEnabledPropertyChanged));

        private static void OnLightingEnabledPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Legend legend = d as Legend;
            legend.FirePropertyChanged("LightingEnabled");
        }

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

        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register
        ("CornerRadius",
        typeof(CornerRadius),
        typeof(Legend),
        new PropertyMetadata(OnCornerRadiusPropertyChanged));

        private static void OnCornerRadiusPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Legend legend = d as Legend;
            legend.FirePropertyChanged("CornerRadius");
        }

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
        typeof(Legend),
        new PropertyMetadata(OnShadowEnabledPropertyChanged));

        private static void OnShadowEnabledPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {   
            Legend legend = d as Legend;
            legend.FirePropertyChanged("ShadowEnabled");
        }

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

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register
        ("Title",
        typeof(String),
        typeof(Legend),
        new PropertyMetadata(OnTitlePropertyChanged));

        private static void OnTitlePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Legend legend = d as Legend;
            legend.FirePropertyChanged("Title");
        }

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

        public static readonly DependencyProperty TitleAlignmentXProperty = DependencyProperty.Register
        ("TitleAlignmentX",
        typeof(HorizontalAlignment),
        typeof(Legend),
        new PropertyMetadata(OnTitleAlignmentXPropertyChanged));

        private static void OnTitleAlignmentXPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Legend legend = d as Legend;
            legend.FirePropertyChanged("TitleAlignmentX");
        }

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

        public static readonly DependencyProperty TitleTextAlignmentProperty = DependencyProperty.Register
            ("TitleTextAlignment",
            typeof(TextAlignment),
            typeof(Legend),
            new PropertyMetadata(OnTitleTextAlignmentPropertyChanged));

        private static void OnTitleTextAlignmentPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Legend legend = d as Legend;
            legend.FirePropertyChanged("TitleTextAlignment");
        }

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

        public static readonly DependencyProperty TitleBackgroundProperty = DependencyProperty.Register
        ("TitleBackground",
        typeof(Brush),
        typeof(Legend),
        new PropertyMetadata(OnTitleBackgroundPropertyChanged));

        private static void OnTitleBackgroundPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Legend legend = d as Legend;
            legend.FirePropertyChanged("TitleBackground");
        }

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

        public static readonly DependencyProperty TitleFontColorProperty = DependencyProperty.Register
        ("TitleFontColor",
        typeof(Brush),
        typeof(Legend),
        new PropertyMetadata(OnTitleFontColorPropertyChanged));

        private static void OnTitleFontColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Legend legend = d as Legend;
            legend.FirePropertyChanged("TitleFontColor");
        }

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

        public static readonly DependencyProperty TitleFontFamilyProperty = DependencyProperty.Register
        ("TitleFontFamily",
        typeof(FontFamily),
        typeof(Legend),
        new PropertyMetadata(OnTitleFontFamilyPropertyChanged));

        private static void OnTitleFontFamilyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Legend legend = d as Legend;
            legend.FirePropertyChanged("TitleFontFamily");
        }

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

        public static readonly DependencyProperty TitleFontSizeProperty = DependencyProperty.Register
        ("TitleFontSize",
        typeof(Double),
        typeof(Legend),
        new PropertyMetadata(OnTitleFontSizePropertyChanged));

        private static void OnTitleFontSizePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Legend legend = d as Legend;
            legend.FirePropertyChanged("TitleFontSize");
        }
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

        public static readonly DependencyProperty TitleFontStyleProperty = DependencyProperty.Register
        ("TitleFontStyle",
        typeof(FontStyle),
        typeof(Legend),
        new PropertyMetadata(OnTitleFontStylePropertyChanged));

        private static void OnTitleFontStylePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Legend legend = d as Legend;
            legend.FirePropertyChanged("TitleFontStyle");
        }

        //[System.ComponentModel.TypeConverter(typeof(System.Windows.FontWeightConverter))]
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

        public static readonly DependencyProperty TitleFontWeightProperty = DependencyProperty.Register
        ("TitleFontWeight",
        typeof(FontWeight),
        typeof(Legend),
        new PropertyMetadata(OnTitleFontWeightPropertyChanged));

        private static void OnTitleFontWeightPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Legend legend = d as Legend;
            legend.FirePropertyChanged("TitleFontWeight");
        }

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

        public static readonly DependencyProperty EntryMarginProperty = DependencyProperty.Register
           ("EntryMargin",
           typeof(Double),
           typeof(Legend),
           new PropertyMetadata(OnEntryMarginPropertyPropertyChanged));

        private static void OnEntryMarginPropertyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Legend legend = d as Legend;
            legend.FirePropertyChanged("EntryMargin");
        }
        
        internal Orientation Orientation
        {
            get;
            set;
        }

        /// <summary>
        /// LegendContainer is the 1st child of the Visual
        /// </summary>
        private StackPanel LegendContainer
        {
            get;
            set;
        }

        private TextBlock TitleElement
        {
            get;
            set;
        }

        internal void CreateVisual()
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

        internal Grid CreateLegendContent()
        {
            Grid legendContent = new Grid();

            Int32 row, column;
            MaximumWidth -= 2 * Padding.Left;
            MaximumHeight -= 2 * Padding.Left;

            if (Orientation == Orientation.Vertical)
            {   
                if (LegendLayout == LegendLayouts.FlowLayout)
                {   
                    Int32 currentPanelIndex = 0;
                    Double currentHeight = 0;
                    StackPanel legendPanel = new StackPanel();
                    (legendPanel as StackPanel).Orientation = Orientation.Horizontal;
                    legendPanel.Children.Add(StackPanelColumn());
                    legendPanel.Height = 0;

                    foreach (KeyValuePair<String, Marker> labelAndSymbol in Entries)
                    {   
                        Marker marker = labelAndSymbol.Value;
                        marker.Margin = EntryMargin;
                        marker.LabelMargin = LabelMargin;
                        marker.Text = labelAndSymbol.Key;
                        
                        marker.TextAlignmentY = AlignmentY.Center;
                        marker.TextAlignmentX = AlignmentX.Right;

                        ApplyFontProperty(marker);

                        marker.CreateVisual();
                       

                        marker.Visual.HorizontalAlignment = HorizontalAlignment.Left;

                        if ((currentHeight + marker.MarkerActualSize.Height) <= MaximumHeight)
                        {
                            (legendPanel.Children[currentPanelIndex] as StackPanel).Children.Add(marker.Visual);
                            currentHeight += marker.MarkerActualSize.Height;
                        }
                        else
                        {   
                            legendPanel.Children.Add(StackPanelColumn());
                            currentPanelIndex++;
                            (legendPanel.Children[currentPanelIndex] as StackPanel).Children.Add(marker.Visual);
                            currentHeight = marker.MarkerActualSize.Height;
                        }

                        legendPanel.Height = (legendPanel.Height < currentHeight) ? currentHeight : legendPanel.Height;
                    }

                    legendPanel.HorizontalAlignment = HorizontalAlignment.Center;
                    legendPanel.VerticalAlignment = VerticalAlignment.Center;

                    legendContent.Children.Add(legendPanel);
                }
                else if (LegendLayout == LegendLayouts.Gridlayout)// MaxWidth is reqired for GridLayout calculation
                {
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
            }
            else if (Orientation == Orientation.Horizontal)
            {   
                if (LegendLayout == LegendLayouts.FlowLayout)
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
                        ApplyFontProperty(marker);

                        marker.TextAlignmentY = AlignmentY.Center;
                        marker.TextAlignmentX = AlignmentX.Right;

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

                    legendPanel.HorizontalAlignment = HorizontalAlignment.Center;
                    legendPanel.VerticalAlignment = VerticalAlignment.Center;

                    legendContent.Children.Add(legendPanel);
                }
                else if (LegendLayout == LegendLayouts.Gridlayout)// MaxWidth is reqired for GridLayout calculation
                {
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
                            legendGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(maxEntrySize.TextSize.Width + LabelMargin/2) });
                        }

                        (labelAndSymbol.Value as Marker).Visual.Margin = new Thickness(EntryMargin, EntryMargin, LabelMargin / 2, EntryMargin);
                        (labelAndSymbol.Value as Marker).Visual.SetValue(Grid.RowProperty, row);
                        (labelAndSymbol.Value as Marker).Visual.SetValue(Grid.ColumnProperty, column++);
                        
                        legendGrid.Children.Add((labelAndSymbol.Value as Marker).Visual);

                        TextBlock label = new TextBlock();
                        label.Margin = new Thickness(LabelMargin, 0,0, 0);

                        label.Text = labelAndSymbol.Key;
                        ApplyFontProperty(label);
                        label.SetValue(Grid.RowProperty, row);
                        label.SetValue(Grid.ColumnProperty, column++);
                        label.HorizontalAlignment = HorizontalAlignment.Left;
                        label.VerticalAlignment = VerticalAlignment.Center;
                        legendGrid.Children.Add(label);
                        label.Measure( new Size(Double.MaxValue,Double.MaxValue));

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
            }

            legendContent.Measure(new Size(Double.MaxValue, Double.MaxValue));
            legendContent.Height = legendContent.DesiredSize.Height + Padding.Left * 2;
            legendContent.Width = legendContent.DesiredSize.Width + Padding.Left * 2;

            return legendContent;
        }
            
        /// <summary>
        /// Returns max entry size of legend
        /// </summary>
        /// <returns></returns>
        private EntrySize GetMaxSymbolAndColumnWidth()
        {
            EntrySize entrySize = new EntrySize();
            
            foreach(KeyValuePair<String,Marker> labelAndSymbol in Entries)
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

        private struct EntrySize
        {
            public Size SymbolSize;
            public Size TextSize;
        }

        private new Double MaxWidth
        {
            get
            {
               return (Double) GetValue(MaxColumnsProperty);
            }
            set
            {
                SetValue(MaxWidthProperty, value);
            }
        }

        private new Double MaxHeight
        {
            get
            {
                return (Double) GetValue(MaxColumnsProperty);
            }
            set
            {
                SetValue(MaxWidthProperty, value);
            }
        }
        private void ApplyFontProperty(TextBlock textBlock)
        {
            textBlock.FontFamily = FontFamily;
            textBlock.FontStyle = FontStyle;
            textBlock.FontWeight = FontWeight;
            textBlock.FontSize = FontSize;
            textBlock.Foreground = Graphics.ApplyAutoFontColor((Chart as Chart), FontColor, this.DockInsidePlotArea);
        }

        private void ApplyFontProperty(Marker marker)
        {
            marker.FontFamily = FontFamily;
            marker.FontStyle = FontStyle;
            marker.FontWeight = FontWeight;
            marker.FontSize = FontSize;
            marker.FontColor = Graphics.ApplyAutoFontColor((Chart as Chart), FontColor, this.DockInsidePlotArea);
        }

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
                title.Text = Title;

            //if (TitleFontColor != null)
            title.FontColor = Graphics.ApplyAutoFontColor((Chart as Chart), TitleFontColor, this.DockInsidePlotArea);
        }

        private void ApplyVisualProperty()
        {
            if (Cursor != null)
                Visual.Cursor = Cursor;

            Visual.BorderBrush = BorderColor;
                        
            Visual.BorderThickness = BorderThickness;

            //Binding binding = new Binding("BorderThickness");
            //binding.Source = this;
            //binding.Mode = BindingMode.TwoWay;
            //Visual.SetBinding(Border.BorderThicknessProperty, binding);

            Visual.CornerRadius = CornerRadius;
            Visual.Background = this.Background;
            
            Visual.HorizontalAlignment = HorizontalAlignment;
            Visual.VerticalAlignment = VerticalAlignment;
            Visual.Opacity = this.Opacity;

            ApplyLighting();
            AttachHref(Chart, Visual, Href, HrefTarget);
            AttachToolTip(Chart, Visual, ToolTipText);
            AttachEvents2Visual(this, Visual);
        }

        private Size TextBlockActualSize(TextBlock textBlock)
        {
#if WPF     
            textBlock.Measure(new Size(Double.MaxValue,Double.MaxValue));
            return textBlock.DesiredSize;
#else
            return new Size(textBlock.ActualWidth, textBlock.ActualHeight);
#endif
        }

        public void ApplyLighting()
        {
            if (LightingEnabled)
                LegendContainer.Background = Graphics.LightingBrush(LightingEnabled);
            else
                LegendContainer.Background = new SolidColorBrush(Colors.Transparent);
        }
        
        private StackPanel StackPanelColumn()
        {
            StackPanel st = new StackPanel();
            st.HorizontalAlignment = HorizontalAlignment.Left;
            st.Orientation = Orientation.Vertical;
            return st;
        }

        private StackPanel StackPanelRow()
        {
            StackPanel st = new StackPanel();
            st.Orientation = Orientation.Horizontal;
            return st;
        }

#if WPF
        static Boolean _defaultStyleKeyApplied;            // Default Style key
#endif

    }

}


  
