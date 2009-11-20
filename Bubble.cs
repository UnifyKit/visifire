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
using Visifire.Charts;
using System.Windows.Media.Effects;

namespace Visifire.Charts
{
    public class Bubble
    {
        public Bubble()
        {
           
        }

        public Ellipse BubbleElement
        {   
            get { return _mainBubble; }
        }
        
        public Double Size
        {
            get {   return _size;   }
            set {   
                _size = value;

                if (_mainCanvasVisual != null)
                {
                    _mainCanvasVisual.Height = Size;
                    _mainCanvasVisual.Width = Size;
                    _mainBubble.Height = Size;
                    _mainBubble.Width = Size;
                    //_3dLight.Height = Size;
                    //_3dLight.Width = Size;
                   // _spotLight.Height = Size / 2;
                   // _spotLight.Width = Size / 2;
                }
            }
        }
        
        public Brush Color
        {
            get { return _color; }
            set 
            { 
                _color = value;

                if (_mainCanvasVisual != null)
                {
                    SetMainBubleColor();
                    ApplyColorFor3dLighting();
                }
            }
        }

        public Boolean ShadowEnabled
        {
            get { return _shadowEnabled; }
            set { _shadowEnabled = value; }
        }

        private void SetMainBubleColor()
        {
            if (Color.GetType().Equals(typeof(SolidColorBrush)))
            {
                Color color = (Color as SolidColorBrush).Color;
                color = Graphics.GetLighterColor(color, 1);
                _mainBubble.Fill = new SolidColorBrush(color);
            }
            else
                _mainBubble.Fill = Color;
        }

        public Panel GetVisual()
        {   
            _mainCanvasVisual = new Canvas() { Height = Size, Width = Size };
            //_mainCanvasVisual.Background = new SolidColorBrush(Colors.Yellow);
            //_mainCanvasVisual.ShowGridLines = true;

            //_mainCanvasVisual.RowDefinitions.Add(new RowDefinition());
            //_mainCanvasVisual.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(Size) });
            //_mainCanvasVisual.RowDefinitions.Add(new RowDefinition());
            //_mainCanvasVisual.ColumnDefinitions.Add(new ColumnDefinition());
            //_mainCanvasVisual.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(Size) });
            //_mainCanvasVisual.ColumnDefinitions.Add(new ColumnDefinition());
            
            _mainBubble = new Ellipse() { Height = Size, Width = Size};
            //_3dLight = new Ellipse() { Height = Size, Width = Size , IsHitTestVisible = false};
            //_spotLight = new Ellipse() { Height = Size / 2, Width = Size / 2, IsHitTestVisible = false };

            //_mainBubble.SetValue(Grid.RowProperty, (Int32)1);
            //_mainBubble.SetValue(Grid.ColumnProperty, (Int32)1);

            //_3dLight.SetValue(Grid.RowProperty, (Int32)1);
            //_3dLight.SetValue(Grid.ColumnProperty, (Int32)1);

            //_spotLight.SetValue(Grid.RowProperty, (Int32)1);
            //_spotLight.SetValue(Grid.ColumnProperty, (Int32)1);

            //_spotLight.HorizontalAlignment = HorizontalAlignment.Left;
            //_spotLight.VerticalAlignment = VerticalAlignment.Top;

            //SetMainBubleColor();
            ApplyColorFor3dLighting();

            _mainCanvasVisual.Children.Add(_mainBubble);
            //_mainCanvasVisual.Children.Add(_spotLight);
           // _mainCanvasVisual.Children.Add(_3dLight);

            ApplyShadow();

            return _mainCanvasVisual;
        }

        private void ApplyShadow()
        {
            if (ShadowEnabled)
            {
                DropShadowEffect drp = new DropShadowEffect();
                drp.Color = Colors.Gray; //Graphics.ParseSolidColor("#FF133B16");
                drp.BlurRadius = 100;
                drp.ShadowDepth = 6;
                drp.Opacity = 0.62;
                drp.Direction = 302;
                _mainCanvasVisual.Effect = drp;
            }
            else
            {
                _mainCanvasVisual.Effect = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void ApplyColorFor3dLighting()
        {   
            if(Color.GetType().Equals(typeof(SolidColorBrush)))
            {
                Color darkColor = (Color as SolidColorBrush).Color;

                RadialGradientBrush rgb = new RadialGradientBrush()
                {
                    Center = new Point(0.3, 0.3),
                    RadiusX = 0.83,
                    RadiusY = 0.85,
                    GradientOrigin = new Point(0.2, 0.2)
                };

                if (darkColor == Colors.Black)
                {
                    rgb.GradientStops.Add(new GradientStop() { Color = Colors.White, Offset = 0 });
                    rgb.GradientStops.Add(new GradientStop() { Color = Colors.Gray, Offset = 0.1 });
                    rgb.GradientStops.Add(new GradientStop() { Color = Colors.Black, Offset = 1 });

                }
                else
                {
                    Color lightColor = (Color as SolidColorBrush).Color;
                    //lightColor = Graphics.GetLighterColor(darkColor, 1);

                    darkColor = Graphics.GetDarkerColor(darkColor, 0.2);
                    rgb.GradientStops.Add(new GradientStop() { Color = Colors.White, Offset = 0 });
                    rgb.GradientStops.Add(new GradientStop() { Color = lightColor, Offset = 0.1 });
                    rgb.GradientStops.Add(new GradientStop() { Color = darkColor, Offset = 1 });
                    // CalculateColorFor3dSpotLight(false);
                }
                
                _mainBubble.Fill = rgb;
                //_3dLight.Fill = rgb;
            }
            else
                _mainBubble.Fill = Color;
                //_3dLight.Fill = rgb;
        }

        private void CalculateColorFor3dSpotLight(Boolean isBlack)
        {
            RadialGradientBrush rgb = new RadialGradientBrush()
            {
                GradientOrigin = new Point(0.5, 0.46),
                RadiusX = 1,
                RadiusY = 1
            };

            if (isBlack)
            {
                rgb.GradientStops.Add(new GradientStop() { Color = Colors.Transparent, Offset = 0.45 });
                rgb.GradientStops.Add(new GradientStop() { Color = System.Windows.Media.Color.FromArgb(50,255,255,255), Offset = 0.05558 });
                rgb.GradientStops.Add(new GradientStop() { Color = Colors.White, Offset = 0 });
 
            }
            else
            {
                rgb.GradientStops.Add(new GradientStop() { Color = Colors.Transparent, Offset = 0.1 });
                rgb.GradientStops.Add(new GradientStop() { Color = Colors.White, Offset = 0 });
            }
            _spotLight.Fill = rgb;
        }

        Double _size;
        Brush _color;
        Boolean _shadowEnabled;

        Ellipse _mainBubble;
        Ellipse _spotLight;
        Ellipse _3dLight;
        Canvas _mainCanvasVisual;
    }
}