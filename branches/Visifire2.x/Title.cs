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
    /// Title class is a observable object.
    /// Title supports the Visifire Control as parent which implements IVisifireControl interface
    /// </summary>
#if SL
    [System.Windows.Browser.ScriptableType]
#endif
    public class Title : ObservableObject
    {
        #region Public Methods

        #region Constructors

        public Title()
        {
#if SL
            //Binding binding = new Binding("FontFamily");
            //binding.Source = this;
            //binding.Mode = BindingMode.TwoWay;
            //base.SetBinding(Control.FontFamilyProperty, binding);
#else       
            //Binding binding = new Binding("InternalFontStyle");
            //binding.Source = this;
            //binding.Mode = BindingMode.TwoWay;
            //SetBinding(FontStyleProperty, binding);

            //binding = new Binding("InternalFontWeight");
            //binding.Source = this;
            //binding.Mode = BindingMode.TwoWay;
            //SetBinding(FontWeightProperty, binding);
#endif

            SetDefaults();
        }

        internal Title(String text)
        {
            SetDefaults();
            Text = text;
        }

        private void SetDefaults()
        {
#if WPF
            if (!_defaultStyleKeyApplied)
            {
                DefaultStyleKeyProperty.OverrideMetadata(typeof(Title), new FrameworkPropertyMetadata(typeof(Title)));
                _defaultStyleKeyApplied = true;
               
            }

            //object dsp = this.GetValue(FrameworkElement.DefaultStyleKeyProperty);
            //Style = (Style)Application.Current.FindResource(dsp);
#else

            DefaultStyleKey = typeof(Title);
#endif
        }
        
        #endregion Constructors

        #endregion

        #region Public Properties

        /// <summary>
        /// Enabled property
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
            typeof(Title),
            new PropertyMetadata(OnEnabledPropertyChanged));

        private static void OnEnabledPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Title title = d as Title;
            title.FirePropertyChanged("Enabled");
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
            typeof(Title),
            new PropertyMetadata(OnHrefTargetChanged));

        private static void OnHrefTargetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Title title = d as Title;
            title.FirePropertyChanged("HrefTarget");
        }

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

        public static readonly DependencyProperty HrefProperty = DependencyProperty.Register
            ("Href",
            typeof(String),
            typeof(Title),
            new PropertyMetadata(OnHrefChanged));

        private static void OnHrefChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Title title = d as Title;
            title.FirePropertyChanged("Href");
        }
        
        #region Font Properties

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
            typeof(Title),
            new PropertyMetadata(OnFontFamilyPropertyChanged));

        private static void OnFontFamilyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Title title = d as Title;
            title.FirePropertyChanged("FontFamily");
        }

#endif

        public new Double FontSize
        {
            get
            {
                return ((Double)GetValue(FontSizeProperty) == 0) ? 10 : (Double)GetValue(FontSizeProperty);
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
            typeof(Title),
            new PropertyMetadata(OnFontSizePropertyChanged));
            
        private static void OnFontSizePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Title title = d as Title;
            title.FirePropertyChanged("FontSize");
            //title.UpdateVisual("FontColor", e.NewValue);
        }
#endif

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
            typeof(Title),
            new PropertyMetadata(OnFontColorPropertyChanged));
        
        private static void OnFontColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {   
            Title title = d as Title;
            title.UpdateVisual("FontColor", e.NewValue);
        }

        private new Brush Foreground
        {
            get;
            set;
        }

#if SL

        public new FontStyle FontStyle
        {
            get
            {
                return (FontStyle)(GetValue(FontStyleProperty));
            }
            set
            {
                if (FontStyle != value)
                {
                    SetValue(FontStyleProperty, value);
                    UpdateVisual("FontStyle", value);
                }
            }
        }
#else

        [TypeConverter(typeof(System.Windows.FontStyleConverter))]
        public new FontStyle FontStyle
        {
            get
            {
                return (FontStyle)(GetValue(FontStyleProperty));
            }
            set
            {
                SetValue(FontStyleProperty, value);
            }
        }

        public new static readonly DependencyProperty FontStyleProperty = DependencyProperty.Register
            ("FontStyle",
            typeof(FontStyle),
            typeof(Title),
            new PropertyMetadata(OnFontStylePropertyChanged));

        private static void OnFontStylePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Title title = d as Title;
            title.UpdateVisual("FontStyle",  e.NewValue);
        }

#endif

        //#if SL
//        [System.ComponentModel.TypeConverter(typeof(Visifire.Commons.Converters.FontStyleConverter))]
//        public new FontStyle FontStyle
//        {
//            get
//            {
//                return (FontStyle)(GetValue(FontStyleProperty));
//            }
//            set
//            {
//#if SL
//                if (FontStyle != value)
//                {
//                    SetValue(FontStyleProperty, value);
//                    //FirePropertyChanged("FontStyle"); 
//                    UpdateVisual("FontStyle", value);
//                }
//#else
//                SetValue(FontStyleProperty, value);
//#endif
//            }
//        }
//#endif

//#if WPF

//        [TypeConverter(typeof(System.Windows.FontStyleConverter))]
//        internal FontStyle InternalFontStyle
//        {
//            get
//            {   
//                return (FontStyle)(GetValue(InternalFontStyleProperty));
//            }
//            set
//            {
//                SetValue(InternalFontStyleProperty, value);
//            }
//        }

//        private static readonly DependencyProperty InternalFontStyleProperty = DependencyProperty.Register
//            ("InternalFontStyle",
//            typeof(FontStyle),
//            typeof(Title),
//            new PropertyMetadata(OnFontStylePropertyChanged));

//        private static void OnFontStylePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
//        {
//            Title title = d as Title;
//            //title.FirePropertyChanged("FontStyle");
//            title.UpdateVisual("FontStyle", e.NewValue);
//        }
//#endif

#if SL
        public new FontWeight FontWeight
        {
            get
            {
                return (FontWeight)(GetValue(FontWeightProperty));
            }
            set
            {
                if (FontWeight != value)
                {
                    SetValue(FontWeightProperty, value);
                    UpdateVisual("FontWeight", value);
                }
            }
        }

#else
        [System.ComponentModel.TypeConverter(typeof(System.Windows.FontWeightConverter))]
        public new FontWeight FontWeight
        {
            get
            {
                return (FontWeight)(GetValue(FontWeightProperty));
            }
            set
            {
                SetValue(FontWeightProperty, value);
            }
        }

        private new static readonly DependencyProperty FontWeightProperty = DependencyProperty.Register
            ("FontWeight",
            typeof(FontWeight),
            typeof(Title),
            new PropertyMetadata(OnFontWeightPropertyChanged));

        private static void OnFontWeightPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Title title = d as Title;
            title.UpdateVisual("FontWeight", e.NewValue);
        }
#endif

        //#if SL

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
//                    SetValue(FontWeightProperty, value);
//                    // FirePropertyChanged("FontWeight");
//                    UpdateVisual("FontWeight", value);
//                }
//#else
//                SetValue(FontWeightProperty, value);
//#endif
//            }
//        }

//#else
//        [System.ComponentModel.TypeConverter(typeof(System.Windows.FontWeightConverter))]
//        internal FontWeight InternalFontWeight
//        {   
//            get
//            {
//                return (FontWeight)(GetValue(InternalFontWeightProperty));
//            }
//            set
//            {
//                SetValue(InternalFontWeightProperty, value);
//            }
//        }

//        private static readonly DependencyProperty InternalFontWeightProperty = DependencyProperty.Register
//            ("InternalFontWeight",
//            typeof(FontWeight),
//            typeof(Title),
//            new PropertyMetadata(OnFontWeightPropertyChanged));

//        private static void OnFontWeightPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
//        {
//            Title title = d as Title;
//            //title.FirePropertyChanged("FontWeight");
//            title.UpdateVisual("FontWeight", e.NewValue);
//        }
//#endif
        /// <summary>
        /// Property Text
        /// </summary>
        public String Text
        {
            get
            {
                return (GetValue(TextProperty) == null) ? "": (String)GetValue(TextProperty);
            }
            set
            {
                SetValue(TextProperty, value);
            }
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register
            ("Text",
            typeof(String),
            typeof(Title),
            new PropertyMetadata(OnTextPropertyChanged));

        private static void OnTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Title title = d as Title;
            title.FirePropertyChanged("Text");
        }

        #endregion

        #region Border Properties

        /// <summary>
        /// Property BorderColor
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

        public static readonly DependencyProperty BorderColorProperty = DependencyProperty.Register
            ("BorderColor", 
            typeof(Brush), 
            typeof(Title), 
            new PropertyMetadata(OnBorderColorPropertyChanged));

        private static void OnBorderColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Title title = d as Title;
            title.FirePropertyChanged("BorderColor");
        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        private new Brush BorderBrush
        {
            get;
            set;
        }

        private new static readonly DependencyProperty BorderThicknessProperty = DependencyProperty.Register
            ("BorderThickness",
            typeof(Thickness),
            typeof(Title),
            new PropertyMetadata(OnBorderThicknessPropertyChanged));

        private static void OnBorderThicknessPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Title title = d as Title;
            title.FirePropertyChanged("BorderThickness");
        }

        /// <summary>
        /// Property CornerRadius
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

        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register
            ("CornerRadius",
            typeof(CornerRadius),
            typeof(Title),
            new PropertyMetadata(OnCornerRadiusPropertyChanged));

        private static void OnCornerRadiusPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Title title = d as Title;
            title.FirePropertyChanged("CornerRadius");
        }

        #endregion

        #region Background

        public new Brush Background
        {
            get
            {
                return (Brush)GetValue(BackgroundProperty);
            }
            set
            {
#if SL
                if(Background != value)
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
            typeof(Title),
            new PropertyMetadata(OnBackgroundPropertyChanged));

        private static void OnBackgroundPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Title title = d as Title;
            title.FirePropertyChanged("Background");
        }
#endif

        #endregion

        #region Alignment

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
        typeof(Title),
        new PropertyMetadata(OnHorizontalAlignmentPropertyChanged));

        private static void OnHorizontalAlignmentPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Title title = d as Title;
            title.FirePropertyChanged("HorizontalAlignment");
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
        typeof(Title),
        new PropertyMetadata(OnVerticalAlignmentPropertyChanged));

        private static void OnVerticalAlignmentPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Title title = d as Title;
            title.FirePropertyChanged("VerticalAlignment");
        }

#endif

        #endregion


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
                    SetValue(MarginProperty, value);
                    FirePropertyChanged("Margin");
                }
#else
                SetValue(MarginProperty, value);
#endif
            }
        }

#if WPF
        private new static readonly DependencyProperty MarginProperty = DependencyProperty.Register
            ("Margin",
            typeof(Thickness),
            typeof(Title),
            new PropertyMetadata(OnMarginPropertyChanged));

        private static void OnMarginPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Title title = d as Title;
            title.FirePropertyChanged("Margin");
        }
#endif

        public new Thickness Padding
        {
            get
            {
                return (Thickness)GetValue(PaddingProperty);
            }
            set
            {
#if SL
                if (Margin != value)
                {
                    SetValue(PaddingProperty, value);
                    FirePropertyChanged("Padding");
                }
#else
                SetValue(PaddingProperty, value);
#endif
            }
        }

#if WPF
        private new static readonly DependencyProperty PaddingProperty = DependencyProperty.Register
            ("Padding",
            typeof(Thickness),
            typeof(Title),
            new PropertyMetadata(OnPaddingPropertyChanged));

        private static void OnPaddingPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Title title = d as Title;
            title.FirePropertyChanged("Padding");
        }
#endif

        /// <summary>
        /// Property TextAlignment
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

        public static readonly DependencyProperty TextAlignmentProperty = DependencyProperty.Register
            ("TextAlignment",
            typeof(TextAlignment),
            typeof(Title),
            new PropertyMetadata(OnTextAlignmentPropertyChanged));

        private static void OnTextAlignmentPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Title title = d as Title;
            title.FirePropertyChanged("TextAlignment");
        }

        /// <summary>
        /// Whether the title will be docked inside PlotArea 
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

        public static readonly DependencyProperty DockInsidePlotAreaProperty = DependencyProperty.Register
            ("DockInsidePlotArea",
            typeof(Boolean),
            typeof(Title),
            new PropertyMetadata(OnDockInsidePlotAreaChanged));

        private static void OnDockInsidePlotAreaChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Title title = d as Title;
            title.FirePropertyChanged("DockInsidePlotArea");
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


        #endregion

        #region Private Delegates

        #endregion

        #region Private Properties

        //private new HorizontalAlignment HorizontalAlignment
        //{
        //    get
        //    {
        //        return HorizontalAlignment.Center;
        //    }

        //}

        //private new VerticalAlignment VerticalAlignment
        //{
        //    get
        //    {
        //        return VerticalAlignment.Center;
        //    }
        //}
        
        /// <summary>
        /// Text element of the title to display text content
        /// </summary>
#if DEBUG
        internal TextBlock TextElement
#else
        private TextBlock TextElement
#endif
        {
            get;
            set;
        }

        /// <summary>
        /// Canvas inside Title Visual
        /// </summary>
#if DEBUG
        internal Canvas InnerCanvas
#else
        private Canvas InnerCanvas
#endif
        {
            get;
            set;
        }

        internal Size TextBlockDesiredSize
        {
            get;
            set;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Apply all style properties of the Title
        /// </summary>
        /// <param name="title"></param>
        private static Boolean ApplyProperties(Title title)
        {   
            if (title.Visual != null)
            {   
                // Set TextElement properties 
                title.TextElement.FontFamily = title.FontFamily;
                title.TextElement.FontSize = (Double) title.FontSize;
                title.TextElement.FontStyle = title.FontStyle;
                title.TextElement.FontWeight = title.FontWeight;
                title.TextElement.Text = GetFormattedMultilineText(title.Text);
                title.TextElement.Foreground = Graphics.ApplyAutoFontColor((title.Chart as Chart), title.FontColor, title.DockInsidePlotArea);
                title.TextElement.TextWrapping = TextWrapping.Wrap;

                // Set Border Properties 
                title.Visual.BorderBrush = title.BorderColor;
                
                Binding binding = new Binding("Background");
                binding.Source = title;
                binding.Mode = BindingMode.TwoWay;
                title.Visual.SetBinding(Border.BackgroundProperty, binding);
                
                title.Visual.VerticalAlignment = title.VerticalAlignment;
                title.Visual.HorizontalAlignment = title.HorizontalAlignment;
                title.Visual.BorderThickness = title.BorderThickness;
                title.Visual.Margin = title.Margin;
                title.Visual.Padding = title.Padding;
                title.Visual.CornerRadius = title.CornerRadius;
                title.Visual.Cursor = (title.Cursor == null)? Cursors.Arrow : title.Cursor;
                title.Visual.SetValue(Canvas.ZIndexProperty, title.GetValue(Canvas.ZIndexProperty));
                // AttachToolTip
                ObservableObject.AttachToolTip(title.Chart, title.TextElement, title.ToolTipText);
                ObservableObject.AttachHref(title.Chart, title.TextElement, title.Href, title.HrefTarget);

                return true;
            }
            else
                return false;
        }

        #endregion

        #region Internal Methods

        internal override void UpdateVisual(String propertyName, Object Value)
        {
            if (!ApplyProperties(this))
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
        internal void CreateVisualObject()
        {
            if (!(Boolean)Enabled)
                return;

            // Creating Title Visual Object
            Visual = new Border();
            TextElement = new TextBlock();
            InnerCanvas = new Canvas();
            InnerCanvas.Children.Add(TextElement);
            Visual.Child = InnerCanvas;
            Visual.Opacity = this.Opacity;
            // Set Properties
            ApplyProperties(this);

#if WPF
            TextElement.Measure(new Size(Double.MaxValue, Double.MaxValue));
            TextBlockDesiredSize = new Size(TextElement.DesiredSize.Width, TextElement.DesiredSize.Height);
#else
            TextBlockDesiredSize = new Size(TextElement.ActualWidth, TextElement.ActualHeight);
#endif

            // Set TextElement position inside Title Visual
            if (VerticalAlignment == VerticalAlignment.Center || VerticalAlignment == VerticalAlignment.Stretch)
            {
                if (HorizontalAlignment == HorizontalAlignment.Left)
                {   
                    RotateTransform rt = new RotateTransform();

                    rt.Angle = 270;
                    TextElement.RenderTransformOrigin = new Point(0, 0);
                    TextElement.RenderTransform = rt;
                    
                    InnerCanvas.Height = TextBlockDesiredSize.Width;
                    InnerCanvas.Width = TextBlockDesiredSize.Height;

                    Visual.Width = TextBlockDesiredSize.Height;

                    TextElement.SetValue(Canvas.LeftProperty, (Double)0);
                    TextElement.SetValue(Canvas.TopProperty, (Double)TextBlockDesiredSize.Width);

                    SetTextAlignment4LeftAndRight();

                }
                else if (HorizontalAlignment == HorizontalAlignment.Right)
                {
                    RotateTransform rt = new RotateTransform();
                    rt.Angle = 90;
                    TextElement.RenderTransformOrigin = new Point(0, 0);
                    TextElement.RenderTransform = rt;

                    InnerCanvas.Height = TextBlockDesiredSize.Width;
                    InnerCanvas.Width = TextBlockDesiredSize.Height;

                    Visual.Width = TextBlockDesiredSize.Height;

                    TextElement.SetValue(Canvas.LeftProperty, (Double)TextBlockDesiredSize.Height);
                    TextElement.SetValue(Canvas.TopProperty, (Double)0);

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
        static Boolean _defaultStyleKeyApplied = false;            // Default Style key
#endif 
        #endregion Data
    }
}
