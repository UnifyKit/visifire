#if WPF

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;


#else
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;


#endif

using Visifire.Commons;


namespace Visifire.Charts
{

#if SL
    [System.Windows.Browser.ScriptableType]
#endif

    public class PlotArea : ObservableObject
    {
        #region Public Methods

        public PlotArea()
        {

#if WPF
            if (!_defaultStyleKeyApplied)
            {
                DefaultStyleKeyProperty.OverrideMetadata(typeof(PlotArea), new FrameworkPropertyMetadata(typeof(PlotArea)));
                _defaultStyleKeyApplied = true;
               
            } 

            //object dsp = this.GetValue(FrameworkElement.DefaultStyleKeyProperty);
            //Style = (Style)Application.Current.FindResource(dsp);
            
#else
            DefaultStyleKey = typeof(PlotArea);
#endif

        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Sets the color of the DataPoint
        /// </summary>
        /// <summary>
        /// Sets the DataPoint Color
        /// </summary>
        public Brush Color
        {
            get
            {
                return (Brush)GetValue(ColorProperty);
            }
            set
            {
                SetValue(ColorProperty, value);
            }
        }

        private static readonly DependencyProperty ColorProperty = DependencyProperty.Register
            ("Color",
            typeof(Brush),
            typeof(PlotArea),
            new PropertyMetadata(OnColorPropertyChanged));

        private static void OnColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PlotArea plotArea = d as PlotArea;

            if ((plotArea.Chart as Chart) != null)
                (plotArea.Chart as Chart).CallRender();
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
            typeof(PlotArea),
            new PropertyMetadata(OnHrefChanged));

        private static void OnHrefTargetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PlotArea plotArea = d as PlotArea;

            if((plotArea.Chart as Chart) != null)
                (plotArea.Chart as Chart).CallRender(); 
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
            typeof(PlotArea),
            new PropertyMetadata(OnHrefChanged));

        private static void OnHrefChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PlotArea plotArea = d as PlotArea;

            if ((plotArea.Chart as Chart) != null)
                (plotArea.Chart as Chart).CallRender(); 
        }
       
        /// <summary>
        /// Bevel effect
        /// </summary>
        public Boolean Bevel
        {
            get
            {
                return (Boolean)GetValue(BevelProperty);
            }
            set
            {
                SetValue(BevelProperty, value);
            }
        }
        
        public static readonly DependencyProperty BevelProperty = DependencyProperty.Register
            ("Bevel",
            typeof(Boolean),
            typeof(PlotArea),
            new PropertyMetadata(OnBevelPropertyChanged));
            
        private static void OnBevelPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PlotArea plotArea = d as PlotArea;
  
            if ((plotArea.Chart as Chart) != null)
                (plotArea.Chart as Chart).CallRender(); 
        }
        
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
            typeof(PlotArea),
            new PropertyMetadata(OnBorderColorPropertyChanged));

        private static void OnBorderColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PlotArea plotArea = d as PlotArea;
            
            if ((plotArea.Chart as Chart) != null)
                (plotArea.Chart as Chart).CallRender();
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
            typeof(PlotArea),
            new PropertyMetadata(OnBorderStylePropertyChanged));

        private static void OnBorderStylePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PlotArea plotArea = d as PlotArea;

            if ((plotArea.Chart as Chart) != null)
                (plotArea.Chart as Chart).CallRender(); 
        }

        public new Thickness BorderThickness
        {
            get
            {
                return (Thickness)GetValue(BorderThicknessProperty);
            }
            set
            {
                SetValue(BorderThicknessProperty, value);
                (Chart as Chart).CallRender();
            }
        }

        //public new static readonly DependencyProperty BorderThicknessProperty = DependencyProperty.Register
        //    ("BorderThickness",
        //    typeof(Thickness),
        //    typeof(PlotArea),
        //    new PropertyMetadata(OnBorderThicknessPropertyChanged));

        //private static void OnBorderThicknessPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    PlotArea plotArea = d as PlotArea;
        //    //plotArea.FirePropertyChanged("BorderThickness");

        //    if ((plotArea.Chart as Chart) != null)
        //        (plotArea.Chart as Chart).CallRender();
        //}

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
            typeof(PlotArea),
            new PropertyMetadata(OnLightingEnabledPropertyChanged));

        private static void OnLightingEnabledPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PlotArea plotArea = d as PlotArea;

            if ((plotArea.Chart as Chart) != null)
            {
                (plotArea.Chart as Chart).CallRender();
            }
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
            typeof(PlotArea),
            new PropertyMetadata(OnCornerRadiusPropertyChanged));

        private static void OnCornerRadiusPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PlotArea plotArea = d as PlotArea;

            if ((plotArea.Chart as Chart) != null)
                (plotArea.Chart as Chart).CallRender(); 
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
            typeof(PlotArea),
            new PropertyMetadata(OnShadowEnabledPropertyChanged));

        private static void OnShadowEnabledPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PlotArea plotArea = d as PlotArea;

            if ((plotArea.Chart as Chart) != null)
                (plotArea.Chart as Chart).CallRender(); 
        }

        public override String ToolTipText
        {
            get;
            set;
        }

        #endregion

        internal void CreateVisual()
        {
            if (Visual == null)
            {
                Visual = new Canvas();
                Visual.Children.Add(GetNewBorderElement());
            }

            ApplyProperties();
            ApplyLighting();
        }

        internal Border PlotAreaLightingBorder
        {   
            get;
            set;
        }

        internal Grid BevelGrid
        {
            get;
            set;
        }

        internal Grid ShadowGrid
        {
            get;
            set;
        }

        internal Border GetNewBorderElement()
        {   
            PlotAreaBorderElement = new Border();
            PlotAreaLightingBorder = new Border();

            BevelGrid = new Grid();
            BevelGrid.Children.Add(PlotAreaLightingBorder);
            PlotAreaBorderElement.Child = BevelGrid;

            ApplyProperties();
            
            return PlotAreaBorderElement;
        }

        /// <summary>
        /// Update PlotArea with new Property Values
        /// </summary>
        internal void UpdateProperties()
        {
            PlotAreaBorderElement.SetValue(Canvas.TopProperty,(Double) 0);
            PlotAreaBorderElement.SetValue(Canvas.LeftProperty, (Double)0);

            ApplyProperties();
            ApplyLighting();
        }

        internal void ApplyProperties()
        {
            Visual.Margin = new Thickness(0);
            Visual.Cursor = (Cursor == null) ? Cursors.Arrow : Cursor;

            PlotAreaBorderElement.Opacity = this.Opacity;

            if (Color != null)
                PlotAreaBorderElement.Background = Color;

            if (BorderColor != null)
                PlotAreaBorderElement.BorderBrush = BorderColor;

            if (BorderThickness != null)
            {
                PlotAreaBorderElement.BorderThickness = BorderThickness;
            }

            if (CornerRadius != null)
            {
                PlotAreaBorderElement.CornerRadius = CornerRadius;
                PlotAreaLightingBorder.CornerRadius = CornerRadius;
            }
        }

        internal void ApplyLighting()
        {
            if (PlotAreaLightingBorder != null)
            {
                if (LightingEnabled)
                {   
                    if (Bevel)
                    {
                        PlotAreaLightingBorder.Background = Graphics.GetFrontFaceBrush(Color);
                        PlotAreaLightingBorder.Background.Opacity = 0.6;
                    }
                    else
                    {
                        PlotAreaLightingBorder.Background = Graphics.LightingBrush(LightingEnabled);
                    }
                }
                else
                    PlotAreaLightingBorder.Background = new SolidColorBrush(Colors.Transparent);
            }

        }



        private void ApplyShadow(Thickness shadowDepth, Double width, Double height)
        {
            if (ShadowEnabled)
            {
                Canvas PlotAreaShadowCanvas = new Canvas();

                PlotAreaBorderElement.Margin = shadowDepth;
                Grid shadowGrid = ExtendedGraphics.Get2DRectangleShadow(width, height, new CornerRadius(10), new CornerRadius(10), 6);
                shadowGrid.SetValue(Canvas.TopProperty, shadowDepth.Bottom);
                shadowGrid.SetValue(Canvas.LeftProperty, shadowDepth.Right);

                PlotAreaShadowCanvas.Children.Add(shadowGrid);

                PlotAreaBorderElement.Child = PlotAreaShadowCanvas;
            }
        }

        internal Canvas Visual
        {
            get;
            set;
        }
                
        internal Border PlotAreaBorderElement
        {
            get;
            set;
        }

        #region Data

#if WPF
        private static Boolean _defaultStyleKeyApplied = false;
#endif
        #endregion

    }
}
