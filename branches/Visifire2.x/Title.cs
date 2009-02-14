﻿/*   
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
        }

        /// <summary>
        /// Initializes a new instance of the Visifire.Charts.Title class
        /// </summary>
        internal Title(String text)
        {
            SetDefaultStyle();
            Text = text;
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

#if WPF

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
#endif

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

#if WPF
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
        private new static readonly DependencyProperty FontWeightProperty = DependencyProperty.Register
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
        private new static readonly DependencyProperty BorderThicknessProperty = DependencyProperty.Register
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
        private new static readonly DependencyProperty BackgroundProperty = DependencyProperty.Register
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
        private new static readonly DependencyProperty MarginProperty = DependencyProperty.Register
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
        private new static readonly DependencyProperty PaddingProperty = DependencyProperty.Register
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
        /// Set or get Enabled property of title
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
        /// Set or get HrefTarget property of title
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
        /// Set or get Href property of title
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
                
        #region Font Properties

        /// <summary>
        /// Set or get FontFamily property of title
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
        /// Set or get FontSize property of title
        /// </summary>
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

        /// <summary>
        /// Set or get FontColor property of title text
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

        /// <summary>
        /// Set or get FontStyle property of title text
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
                    UpdateVisual("FontStyle", value);
                }
#else
                SetValue(FontStyleProperty, value);
#endif
            }
        }

        /// <summary>
        /// Set or get FontWeight property of title text
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
                    SetValue(FontWeightProperty, value);
                    UpdateVisual("FontWeight", value);
                }
#else
                SetValue(FontWeightProperty, value);
#endif
            }
        }

        /// <summary>
        /// Set or get Text property of title
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
        
        #endregion

        #region Border Properties

        /// <summary>
        /// Set or get BorderColor property of title
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
        /// Set or get BorderThickness of title
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
        /// Set or get CornerRadius property of title
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
        /// Set or get Background property of title
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

        #region Alignment

        /// <summary>
        /// Set or get HorizontalAlignment property of title
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
        /// Set or get VerticalAlignment property of title
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

        #endregion

        /// <summary>
        /// Set or get Margin property of title
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
                    SetValue(MarginProperty, value);
                    FirePropertyChanged("Margin");
                }
#else
                SetValue(MarginProperty, value);
#endif
            }
        }

        /// <summary>
        /// Set or get Padding property of title
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

        /// <summary>
        /// Set or get Property property TextAlignment
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
        /// Set or get DockInsidePlotArea property of title
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
        /// Text element of the title to display text content
        /// </summary>
        private TextBlock TextElement
        {
            get;
            set;
        }

        /// <summary>
        /// Canvas inside Title Visual
        /// </summary>
        private Canvas InnerCanvas
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
            title.FirePropertyChanged("Enabled");
        }

        /// <summary>
        /// HrefTargetProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnHrefTargetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Title title = d as Title;
            title.FirePropertyChanged("HrefTarget");
        }

        /// <summary>
        /// HrefProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnHrefChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Title title = d as Title;
            title.FirePropertyChanged("Href");
        }

#if WPF
        /// <summary>
        /// FontFamilyProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnFontFamilyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Title title = d as Title;
            title.FirePropertyChanged("FontFamily");
        }

        /// <summary>
        /// FontSizeProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnFontSizePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Title title = d as Title;
            title.FirePropertyChanged("FontSize");
        }

        /// <summary>
        /// OpacityProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnOpacityPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Title title = d as Title;
            title.FirePropertyChanged("Opacity");
        }
#endif

        /// <summary>
        /// FontColorProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnFontColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Title title = d as Title;
            title.UpdateVisual("FontColor", e.NewValue);
        }

#if WPF
        /// <summary>
        /// FontStyleProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnFontStylePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Title title = d as Title;
            title.UpdateVisual("FontStyle",  e.NewValue);
        }

        /// <summary>
        /// FontWeightProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnFontWeightPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Title title = d as Title;
            title.UpdateVisual("FontWeight", e.NewValue);
        }
#endif

        /// <summary>
        /// TextProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {   
            Title title = d as Title;
            title.FirePropertyChanged("Text");
        }
        
        /// <summary>
        /// BorderColorProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnBorderColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {   
            Title title = d as Title;
            title.FirePropertyChanged("BorderColor");
        }

#if WPF
        /// <summary>
        /// BorderThicknessProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnBorderThicknessPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Title title = d as Title;
            title.FirePropertyChanged("BorderThickness");
        }
#endif

        /// <summary>
        /// CornerRadiusProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnCornerRadiusPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Title title = d as Title;
            title.FirePropertyChanged("CornerRadius");
        }

#if WPF
        /// <summary>
        /// BackgroundProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnBackgroundPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Title title = d as Title;
            title.FirePropertyChanged("Background");
        }

        /// <summary>
        /// HorizontalAlignmentProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnHorizontalAlignmentPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Title title = d as Title;
            title.FirePropertyChanged("HorizontalAlignment");
        }

        /// <summary>
        /// VerticalAlignmentProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnVerticalAlignmentPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Title title = d as Title;
            title.FirePropertyChanged("VerticalAlignment");
        }

        /// <summary>
        /// MarginProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnMarginPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Title title = d as Title;
            title.FirePropertyChanged("Margin");
        }

        /// <summary>
        /// PaddingProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnPaddingPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Title title = d as Title;
            title.FirePropertyChanged("Padding");
        }
#endif
        
        /// <summary>
        /// TextAlignmentProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnTextAlignmentPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Title title = d as Title;
            title.FirePropertyChanged("TextAlignment");
        }
        
        /// <summary>
        /// DockInsidePlotAreaProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnDockInsidePlotAreaPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Title title = d as Title;
            title.FirePropertyChanged("DockInsidePlotArea");
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
        private static Boolean ApplyProperties(Title title)
        {   
            if (title.Visual != null)
            {   
                // Set TextElement properties 
                title.TextElement.FontFamily = title.FontFamily;
                title.TextElement.FontSize = title.FontSize;
                title.TextElement.FontStyle = title.FontStyle;
                title.TextElement.FontWeight = title.FontWeight;
                title.TextElement.Text = GetFormattedMultilineText(title.Text);
                title.TextElement.Foreground = Charts.Chart.CalculateFontColor((title.Chart as Chart), title.FontColor, title.DockInsidePlotArea);
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
        /// <param name="PropertyName">Name of the property</param>
        /// <param name="Value">Value of the property</param>
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
                if (HorizontalAlignment == HorizontalAlignment.Left || HorizontalAlignment == HorizontalAlignment.Right)
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