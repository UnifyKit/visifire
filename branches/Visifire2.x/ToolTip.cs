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

namespace Visifire.Charts
{

    /// <summary>
    /// Class ToolTip is used for Displaying ToolTip
    /// </summary>
#if SL
    [System.Windows.Browser.ScriptableType]
#endif
    public class ToolTip : Visifire.Commons.VisifireElement
    {
        #region Public Methods

        public ToolTip()
        {   
#if WPF     
            if (!_defaultStyleKeyApplied)
            {
                DefaultStyleKeyProperty.OverrideMetadata(typeof(ToolTip), new FrameworkPropertyMetadata(typeof(ToolTip)));
                _defaultStyleKeyApplied = true;
            }
#else       
            DefaultStyleKey = typeof(ToolTip);
#endif

        }

        // Summary:
        //     When overridden in a derived class, is invoked whenever application code
        //     or internal processes (such as a rebuilding layout pass) call System.Windows.Controls.Control.ApplyTemplate().
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            
            _borderElement = GetTemplateChild("ToolTipBorder") as Border;
            _toolTipTextBlock = GetTemplateChild("ToolTipTextBlock") as TextBlock;
            this.Visibility = Visibility.Collapsed;
            this.SizeChanged += new SizeChangedEventHandler(ToolTip_SizeChanged);
        }

        #endregion

        #region Private Methods

        void ToolTip_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (Chart != null)
            {
                if (e.NewSize.Width + Chart.BorderThickness.Left + Chart.BorderThickness.Right == Chart.ActualWidth)
                    SetValue(Canvas.LeftProperty, (Double)Chart.Padding.Left);
            }
        }

        #endregion 

        #region Properties

#if WPF
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
#endif
        public Visifire.Commons.VisifireControl Chart
        {
            get;
            set;
        }

        /// <summary>
        /// Enabled property of ToolTip
        /// </summary>
        [System.ComponentModel.TypeConverter(typeof(NullableBoolConverter))]
        public Nullable<Boolean> Enabled
        {
            get
            {
                if ((Nullable<Boolean>)GetValue(EnabledProperty) == null)
                {
                    if (Chart != null)
                        return Chart.ToolTipEnabled;
                    else
                        return true;
                }
                else
                {
                    return (Nullable<Boolean>)GetValue(EnabledProperty);
                }
            }
            set
            {
                SetValue(EnabledProperty, value);
            }
        }

        public static readonly DependencyProperty EnabledProperty = DependencyProperty.Register
            ("Enabled",
            typeof(Nullable<Boolean>),
            typeof(ToolTip),
            new PropertyMetadata(OnEnabledPropertyChanged));

        private static void OnEnabledPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ToolTip t = d as ToolTip;

            if (t._borderElement != null)
            {
                if ((Boolean)e.NewValue)
                    t._borderElement.Visibility = t.Visibility;
                else
                    t._borderElement.Visibility = Visibility.Collapsed;
            }
        }


        /// <summary>
        /// Property Text
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

        /// <summary>
        /// ToolTip text dependency property
        /// </summary>
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register
            ("Text",
            typeof(String),
            typeof(ToolTip),
            new PropertyMetadata(OnTextPropertyChanged));

        private static void OnTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ToolTip tootlTip = d as ToolTip;

            if(tootlTip._toolTipTextBlock != null)
                tootlTip._toolTipTextBlock.Text = (String)e.NewValue;

            if (tootlTip.Chart != null)
            {   
                tootlTip.MaxWidth = tootlTip.Chart.ActualWidth;
                if (tootlTip._toolTipTextBlock != null)
                    tootlTip._toolTipTextBlock.MaxWidth = tootlTip.Chart.ActualWidth - 4;
            }
        }

        /// <summary>
        /// Property Text
        /// </summary>
        public CornerRadius CornerRadius
        {
            get
            {
                return (CornerRadius) GetValue(CornerRadiusProperty);
            }
            set
            {
                SetValue(CornerRadiusProperty, value);
            }
        }

        /// <summary>
        /// CornerRadius dependency property
        /// </summary>
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register
            ("CornerRadius",
            typeof(CornerRadius),
            typeof(ToolTip),
            null);

        /// <summary>
        /// Property Text
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
        /// CornerRadius dependency property
        /// </summary>
        public static readonly DependencyProperty FontColorProperty = DependencyProperty.Register
            ("FontColor",
            typeof(Brush),
            typeof(ToolTip),
            null);

        #endregion

        internal void Show()
        {
            if ((Boolean)Enabled)
            {
                this.Visibility = Visibility.Visible;
            }
            else
                Hide();
        }

        internal void Hide()
        {
            this.Visibility = Visibility.Collapsed;
        }


        #region Data

        private Border _borderElement;
        private TextBlock _toolTipTextBlock;

#if WPF
        static Boolean _defaultStyleKeyApplied = false;            // Default Style key
#endif
        #endregion
    }
}