#if WPF
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Markup;
using System.IO;
using System.Xml;
using System.Threading;
using System.Windows.Automation.Peers;
using System.Windows.Automation;
using System.Globalization;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.Collections;

#else

using System;
using System.Net;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Windows.Markup;
using System.Collections.ObjectModel;
using System.Windows.Browser;

#endif
using Visifire.Charts;

namespace Visifire.Commons
{
    public class Marker
    {
        public Marker(MarkerTypes markerType, Double scaleFactor, Size markerSize, Boolean markerBevel, Brush markerColor, String markerLabelText)
        {
            MarkerType = markerType;
            MarkerFillColor = markerColor;
            Text = markerLabelText;
            TextOrientation = Orientation.Horizontal;
            TextAlignmentX = AlignmentX.Right;
            TextAlignmentY = AlignmentY.Center;
            ScaleFactor = scaleFactor;
            MarkerSize = markerSize;
            BorderThickness = 1;
            TextBackground = new SolidColorBrush(Colors.Transparent);
            //LabelMargin = 10;
            Bevel = markerBevel;
        }

        public Marker()
        {

        }

        public Boolean Bevel
        {
            get;
            set;
        }
        
        public Double ScaleFactor
        {
            get
            {
                return _scaleFactor;
            }
            set
            {
                //System.Diagnostics.Debug.Assert(value < 1, "ScaleFactor can not be less than 1..");

                if (value > 1)
                    _scaleFactor = value; 
            }
        }

        public Double LabelPadding
        {
            get;
            set;
        }

        #region public Events

#if SL
        [ScriptableMember]
#endif
        public EventHandler<MouseEventArgs> MouseEnter
        {
            get { return _onMouseEnter; }
            set
            {
                _onMouseEnter = value;
                //Chart.Render();
            }
        }

        /// <summary>
        /// Event handler for the MouseLeave event 
        /// </summary>
#if SL
        [ScriptableMember]
#endif
        public EventHandler<MouseEventArgs> MouseLeave
        {
            get { return _onMouseLeave; }
            set
            {
                _onMouseLeave = value;
                //Chart.Render();
            }
        }

        #endregion

        public void AddToParent(Canvas parentCanvas, Double xPosition, Double yPosition, Point anchorPoint)
        {
            parentCanvas.Children.Add(Visual);
            Double visualHeight;
            Double visualWidth;
#if WPF

            Visual.Measure(new Size(Double.MaxValue,Double.MaxValue));
            visualHeight = Visual.DesiredSize.Height;
            visualWidth = Visual.DesiredSize.Width;
#else
            visualHeight = Visual.ActualHeight;
            visualWidth = Visual.ActualWidth;
#endif

            if (anchorPoint.X == 0.5)
                xPosition -= (MarkerShape.Width / 2 + ((TextAlignmentX == AlignmentX.Left)? _markerShapePosition.X: 0));
            else if (anchorPoint.X == 1)
                xPosition -= (MarkerShape.Width + ((TextAlignmentX == AlignmentX.Left) ? _markerShapePosition.X : 0));

            if (anchorPoint.Y == 0.5)
                yPosition -= (MarkerShape.Height / 2 + ((TextAlignmentY == AlignmentY.Top)? _markerShapePosition.Y : 0));
            else if (anchorPoint.Y == 1)
                yPosition -= (MarkerShape.Height + ((TextAlignmentY == AlignmentY.Top) ? _markerShapePosition.Y : 0));

            if(TextAlignmentX == AlignmentX.Center)
                if (TextBlockSize.Width > MarkerShape.Width)
                    xPosition -= (TextBlockSize.Width - MarkerShape.Width)/2;

            if(TextAlignmentY == AlignmentY.Center)
                if (TextBlockSize.Height > MarkerShape.Height)
                    yPosition -= (TextBlockSize.Height - MarkerShape.Height)/2;

            Visual.SetValue(Canvas.TopProperty, yPosition);
            Visual.SetValue(Canvas.LeftProperty, xPosition);
        }

        public void AddToParent(Panel parent)
        {
            parent.Children.Add(Visual);
        }

        /// <summary>
        /// Apply all style properties of the Marker text
        /// </summary>
        private void ApplyMarkerShapeProperties()
        {
            // Set Border Properties
            if (MarkerShape != null)
            {   
                MarkerShape.Stroke = BorderColor;
                MarkerShape.Fill = MarkerFillColor;
                MarkerShape.StrokeThickness = BorderThickness;
                MarkerShape.Height = MarkerSize.Height * ScaleFactor;
                MarkerShape.Width = MarkerSize.Width * ScaleFactor;
                MarkerShadow.Height = MarkerSize.Height * ScaleFactor;
                MarkerShadow.Width = MarkerSize.Width * ScaleFactor;
                MarkerShadow.Stroke = null;
            }

            if (BevelLayer != null)
            {
                BevelLayer.Height = MarkerSize.Height * ScaleFactor;
                BevelLayer.Width = MarkerSize.Width * ScaleFactor;

                if ((BevelLayer as Shape) != null)
                    (BevelLayer as Shape).StrokeThickness = BorderThickness;
            }
        }

        /// <summary>
        /// Apply all style properties of the Marker text
        /// </summary>
        private void ApplyTextBlockProperties()
        {
            if (TextBlock != null)
            {
                // Set TextElement properties 
                TextBlock.FontFamily = FontFamily;
                TextBlock.FontSize = FontSize;
                TextBlock.FontStyle = FontStyle;
                TextBlock.FontWeight = FontWeight;
                TextBlock.Text = Text;
                TextBlock.Foreground = FontColor;
                TextBlock.VerticalAlignment = VerticalAlignment.Top;
            }
        }

        #region Font Properties

        /// <summary>
        /// Property FontFamily
        /// </summary>
        public FontFamily FontFamily
        {
            get
            {
                if (_fontFamily == null)
                    return new FontFamily("Arial");
                else
                    return _fontFamily;
            }
            set
            {
                _fontFamily = value;
            }
        }

        /// <summary>
        /// Property FontSize
        /// </summary>
        public Double FontSize
        {
            get
            {
                if (_fontSize == 0)
                    _fontSize = 10;

                return _fontSize;
            }
            set
            {
                _fontSize = value;
            }
        }

        /// <summary>
        /// Property FontColor
        /// </summary>
        public Brush FontColor
        {
            get
            {
                return (_fontColor == null) ? (new SolidColorBrush(Colors.Black)) : _fontColor;
            }
            set
            {
                _fontColor = value;

            }
        }

        /// <summary>
        /// Property FontStyle
        /// </summary>
        public FontStyle FontStyle
        {
            get
            {
                return _fontStyle;
            }

            set
            {
                _fontStyle = value;
            }
        }

        /// <summary>
        /// Property FontWeight
        /// </summary>
        public FontWeight FontWeight
        {
            get
            {
                return _fontWeight;
            }
            set
            {
                _fontWeight = value;

            }
        }

        /// <summary>
        /// Property Text
        /// </summary>
        public String Text
        {
            get
            {
                return (_text == null) ? "" : _text;
            }
            set
            {
                _text = value;
            }
        }


        /// <summary>
        /// Property Text
        /// </summary>
        public Brush TextBackground
        {
            get
            {
                return _textBackground;
             }
            set
            {
                _textBackground = value;
            }
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
                if (_borderColor == null)
                    _borderColor = MarkerFillColor;

                return _borderColor;
            }
            set
            {
                _borderColor = value;
#if WPF
                // Make it unmodifiable. 
                _borderColor.Freeze();
#endif
            }
        }

        /// <summary>
        /// Property BorderThickness
        /// </summary>
        public Double BorderThickness
        {
            get
            {
                return _borderThickness;
            }
            set
            {
                _borderThickness = value;
            }
        }

        #endregion


        /// <summary>
        /// Marker Visual
        /// </summary>
        public Grid Visual
        {
            get;
            set;
        }

        /// <summary>
        /// Marker fill color
        /// </summary>
        internal Brush MarkerFillColor
        {
            get;
            set;
        }

        /// <summary>
        /// Marker shape reference
        /// </summary>
        internal Shape MarkerShape
        {
            get;
            set;
        }

        internal FrameworkElement BevelLayer
        {
            get;
            set;
        }

        internal Shape MarkerShadow
        {
            get;
            set;
        }

        /// <summary>
        /// Actual Size of Marker
        /// </summary>
        internal Size MarkerActualSize
        {
            get;
            private set;
        }

        /// <summary>
        /// Actual Size of Marker
        /// </summary>
        public Size MarkerSize
        {
            get;
            set;
        }

        /// <summary>
        /// This property applies only for Bubble and Point charts
        /// </summary>
        internal bool ShadowEnabled
        {
            get;
            set;
        }

        /// <summary>
        /// TextBlock for Marker
        /// </summary>
        public TextBlock TextBlock
        {
            get;
            set;
        }

        /// <summary>
        /// Text orientation of Marker label
        /// </summary>
        public Orientation TextOrientation
        {
            get;
            set;
        }

        /// <summary>
        /// HorizontalAlignment of Marker label
        /// </summary>
        public AlignmentX TextAlignmentX
        {
            get
            {
                return _textAlignmentX;
            }
            set
            {
                _textAlignmentX = value;
            }
        }

        /// <summary>
        /// VerticalAlignment of Marker label
        /// </summary>
        public AlignmentY TextAlignmentY
        {
            get
            {
                return _textAlignmentY;
            }
            set
            {
                _textAlignmentY = value;
            }
        }

        /// <summary>
        /// Shape type of a Marker
        /// </summary>
        public MarkerTypes MarkerType
        {
            get;
            set;
        }

        public Double Margin
        {
            get;
            set;
        }

        public Double LabelMargin
        {
            get;
            set;
        }

        /// <summary>
        ///Size of the TextBlock
        /// </summary>
        internal Size TextBlockSize
        {
            get;
            set;
        }

        internal Chart Chart
        {
            get;
            set;
        }

        public Cursor Cursor
        {
            get;
            set;
        }

        /// <summary>
        /// Create shape for Marker
        /// </summary>
        /// <param name="xaml"></param>
        /// <returns></returns>
        private Shape GetShape()
        {
            String xaml = null;

            switch (MarkerType)
            {
                 
                case MarkerTypes.Circle:

                    Ellipse ellipse = new Ellipse() { Height = 6, Width = 6 };

                    GradientStopCollection gsc = new GradientStopCollection();
                    gsc.Add(new GradientStop() { Offset = 0, Color = Colors.White });
                    gsc.Add(new GradientStop() { Offset = 1, Color = Colors.Gray });
                    ellipse.Fill = new LinearGradientBrush() { GradientStops = gsc, StartPoint = new Point(0.5, 1), EndPoint = new Point(0.5, 0) };

                    return ellipse;

                case MarkerTypes.Cross:
                    xaml = String.Format(@"<Path xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"" xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"" Height=""6"" Width=""6"" Fill=""#FFFFFFFF"" Stretch=""Fill"" Stroke=""#FF000000"" Data=""M126.66666,111 L156.33333,84.333336 M156.00032,111 L126.33299,84.667"" StrokeThickness=""0.5"" />");

                    break;

                case MarkerTypes.Diamond:
                    xaml = String.Format(@"<Path xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"" xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"" Height=""11"" Width=""8"" Fill=""#FFFFFFFF"" Stretch=""Fill"" Stroke=""#FF000000"" Data=""M97.374908,52.791668 L94.541656,57.041668 L97.386475,61.010586 L100.59509,57.010422 z"" StrokeThickness=""0.8""/>");

                    break;

                case MarkerTypes.Square:
                    xaml = String.Format(@"<Path xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"" xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"" Height=""8"" Width=""8"" Fill=""#FFFFFFFF"" Stretch=""Fill"" Stroke=""#FF000000"" StrokeThickness=""0.8"" Data=""M160,40.166725 L160.00034,47.611012 L168.00789,47.611374 L168.00787,40.16637 z""/>");

                    break;

                case MarkerTypes.Triangle:
                    xaml = String.Format(@"<Path xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"" xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"" Height=""8"" Margin=""0,0,0,0"" Width=""8"" Fill=""#FFFFFFFF"" Stretch=""Fill"" Stroke=""#FF000000"" StrokeThickness=""0.8"" Data=""M163.89264,40.166725 L160.00034,46.921658 L168.00789,46.922005 z""/>");

                    break;

                case MarkerTypes.Line:
                    xaml = String.Format(@"<Path xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"" xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"" Data=""M188.63132,258.42844 L188.63132,258.42847 C188.9104,278.91281 201.84741,278.91315 202.18343,258.49094 203.19153,235.62109 187.90231,234.79944 188.63132,258.42844 z M202.56129,256.09153 C202.26549,256.27575 202.32971,260.25791 202.54637,260.48211 202.68528,260.62585 208.86081,260.26716 212.77637,260.48211 214.29462,260.56545 214.56375,256.40657 212.8964,256.19363 211.16966,255.97311 202.56129,256.09153 202.56129,256.09153 z M188.24223,260.6353 L177.59218,260.65346 C176.06613,260.87177 176.17243,255.97066 177.43324,255.91033 L188.28842,255.91961"" Stretch=""Fill"" Width=""18"" Height=""8"" VerticalAlignment=""Center"" HorizontalAlignment=""Center"" Stroke=""#FF000000"" Fill=""#FFFFFFFF"" />");
                    break;
            }

#if WPF
            return (Shape)XamlReader.Load(new XmlTextReader(new StringReader(xaml)));
#else
            return System.Windows.Markup.XamlReader.Load(xaml) as Shape;
#endif
        }

        /// <summary>
        /// Create shape for Marker
        /// </summary>
        /// <param name="xaml"></param>
        /// <returns></returns>
        private FrameworkElement GetBevelLayer()
        {
            String xaml = null;
            Brush topBrush = ColumnChart.GetBevelTopBrush(MarkerFillColor);
            String color;
            
            switch (MarkerType)
            {
                case MarkerTypes.Circle:

                    Ellipse ellipse = new Ellipse() { Height = 6, Width = 6 };

                    GradientStopCollection gsc = new GradientStopCollection();
                    gsc.Add(new GradientStop() { Offset = 0, Color = Graphics.GetDarkerColor((topBrush as LinearGradientBrush).GradientStops[1].Color,.8) });
                    gsc.Add(new GradientStop() { Offset = 0.6, Color = Graphics.GetDarkerColor((topBrush as LinearGradientBrush).GradientStops[1].Color, .8) });
                    gsc.Add(new GradientStop() { Offset = 0.8, Color = (topBrush as LinearGradientBrush).GradientStops[1].Color });
                    gsc.Add(new GradientStop() { Offset = 1, Color = (topBrush as LinearGradientBrush).GradientStops[1].Color });

                    ellipse.Fill = new LinearGradientBrush() { GradientStops = gsc, StartPoint = new Point(0.5, 1), EndPoint = new Point(0.5, 0) };

                    return ellipse;

                case MarkerTypes.Cross:

                    topBrush = ColumnChart.GetBevelTopBrush(BorderColor);
                    color = (topBrush as LinearGradientBrush).GradientStops[2].Color.ToString();

                    xaml = String.Format(@"<Path xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"" xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"" Height=""6"" Width=""6"" Stretch=""Fill"" Data=""M126.66666,111 L156.33333,84.333336 M156.00032,111 L126.33299,84.667"" StrokeThickness=""0.5"" >
    		        <Path.Stroke>
    			        <RadialGradientBrush>
        			       <GradientStop Color=""{0}"" Offset=""0""/>
                         
        			        <GradientStop Color=""{1}"" Offset=""0.9889""/>
        			        <GradientStop Color=""{1}"" Offset=""1""/>
        		        </RadialGradientBrush>
    		        </Path.Stroke>
    	            </Path>", Graphics.GetDarkerColor((topBrush as LinearGradientBrush).GradientStops[2].Color, .8).ToString(), color);

                    break;

                case MarkerTypes.Diamond:
                    
                    color = (topBrush as LinearGradientBrush).GradientStops[2].Color.ToString();
                   
                    xaml = String.Format(@"<Path xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"" xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"" Height=""11"" Width=""8"" Stretch=""Fill"" Data=""M97.374908,52.791668 L94.541656,57.041668 L97.386475,61.010586 L100.59509,57.010422 z"" StrokeThickness=""0.8"">
    		        <Path.Fill>
    			       <RadialGradientBrush>
        			       <GradientStop Color=""{0}"" Offset=""0""/>
                            <GradientStop Color=""{0}"" Offset=""0.6""/>
        			        <GradientStop Color=""{1}"" Offset=""0.75""/>
        			        <GradientStop Color=""{1}"" Offset=""1""/>
        		        </RadialGradientBrush>
    		        </Path.Fill>
    	            </Path>", Graphics.GetDarkerColor((topBrush as LinearGradientBrush).GradientStops[2].Color, .8).ToString(), color);

                    break;

                case MarkerTypes.Square:

                    Double height = MarkerSize.Height * ScaleFactor;
                    Double width = MarkerSize.Width * ScaleFactor;

                    Canvas bevelCanvas = Visifire.Charts.ExtendedGraphics.Get2DRectangleBevel(width,height,
                    3, 3,
                    topBrush,
                    ColumnChart.GetBevelSideBrush(0, MarkerFillColor),
                    ColumnChart.GetBevelSideBrush(120, MarkerFillColor),
                    ColumnChart.GetBevelSideBrush(100, MarkerFillColor)
                    );
                    
                   return bevelCanvas;

                case MarkerTypes.Triangle:

                   color = (topBrush as LinearGradientBrush).GradientStops[1].Color.ToString();

                   xaml = String.Format(@"<Path xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"" xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"" Height=""8"" Margin=""0,0,0,0"" Width=""8"" Stretch=""Fill"" StrokeThickness=""0.8"" Data=""M163.89264,40.166725 L160.00034,46.921658 L168.00789,46.922005 z"">
    		        <Path.Fill>
    			       <RadialGradientBrush>
        			       <GradientStop Color=""{0}"" Offset=""0""/>
                            <GradientStop Color=""{0}"" Offset=""0.6""/>
        			        <GradientStop Color=""{1}"" Offset=""0.8""/>
        			        <GradientStop Color=""{1}"" Offset=""1""/>
        		        </RadialGradientBrush>
    		        </Path.Fill>
    	            </Path>", Graphics.GetDarkerColor((topBrush as LinearGradientBrush).GradientStops[1].Color, .8).ToString(), color);

                    break;
                case MarkerTypes.Line:

                    color = (topBrush as LinearGradientBrush).GradientStops[1].Color.ToString();

                    xaml = String.Format(@"<Path Margin=""0,0,0,0"" xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"" xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"" Data=""M188.63132,258.42844 L188.63132,258.42847 C188.9104,278.91281 201.84741,278.91315 202.18343,258.49094 203.19153,235.62109 187.90231,234.79944 188.63132,258.42844 z M202.56129,256.09153 C202.26549,256.27575 202.32971,260.25791 202.54637,260.48211 202.68528,260.62585 208.86081,260.26716 212.77637,260.48211 214.29462,260.56545 214.56375,256.40657 212.8964,256.19363 211.16966,255.97311 202.56129,256.09153 202.56129,256.09153 z M188.24223,260.6353 L177.59218,260.65346 C176.06613,260.87177 176.17243,255.97066 177.43324,255.91033 L188.28842,255.91961"" Stretch=""Fill"" StrokeThickness=""1"" Width=""19"" Height=""8"" VerticalAlignment=""Center"" HorizontalAlignment=""Center"" Stroke=""#FF000000"" >
                    <Path.Fill>
    			       <RadialGradientBrush>
        			       <GradientStop Color=""{0}"" Offset=""0""/>
                            <GradientStop Color=""{0}"" Offset=""0.6""/>
        			        <GradientStop Color=""{1}"" Offset=""0.8""/>
        			        <GradientStop Color=""{1}"" Offset=""1""/>
        		        </RadialGradientBrush>
    		        </Path.Fill>
    	            </Path>", Graphics.GetDarkerColor((topBrush as LinearGradientBrush).GradientStops[1].Color, .8).ToString(), color);

                    break;

            }

#if WPF
            return (Shape)XamlReader.Load(new XmlTextReader(new StringReader(xaml)));
#else
            return System.Windows.Markup.XamlReader.Load(xaml) as Shape;
#endif
        }

        /// <summary>
        /// Create visual for Marker
        /// </summary>
        public void CreateVisual()
        {
            // Create Visual as Grid
            Visual = new Grid();
            //Visual.Background = new SolidColorBrush(Colors.Gray);
            // Visual.ShowGridLines = true;

            // Create Shape for Marker
            MarkerShape = GetShape();
            MarkerShadow = GetShape();

            // Set shadow properties
            MarkerShadow.Fill = GetMarkerShadowColor();
            TranslateTransform tt = new TranslateTransform() { X = 1, Y = 1};
            MarkerShadow.RenderTransform = tt;
            //MarkerShadow.StrokeThickness = 0;

            // display or hide the shadow
            MarkerShadow.Visibility = (ShadowEnabled?Visibility.Visible:Visibility.Collapsed);

            // Set properties
            MarkerShape.SetValue(Grid.RowProperty, 1);
            MarkerShape.SetValue(Grid.ColumnProperty, 1);
            MarkerShadow.SetValue(Grid.RowProperty, 1);
            MarkerShadow.SetValue(Grid.ColumnProperty, 1);

            // Add Shape for Marker into Visual
            Visual.Children.Add(MarkerShadow);
            Visual.Children.Add(MarkerShape);

            if(Bevel)
            {
                BevelLayer = GetBevelLayer();
                BevelLayer.SetValue(Grid.RowProperty, 1);
                BevelLayer.SetValue(Grid.ColumnProperty, 1);
                BevelLayer.SetValue(Grid.RowProperty, 1);
                BevelLayer.SetValue(Grid.ColumnProperty, 1);
                Visual.Children.Add(BevelLayer);
            }             

            // Apply Shape property
            ApplyMarkerShapeProperties();

            if (!String.IsNullOrEmpty(Text))
            {
                // Define row and columns 
                Visual.RowDefinitions.Add(new RowDefinition());
                Visual.RowDefinitions.Add(new RowDefinition());
                Visual.RowDefinitions.Add(new RowDefinition());
                Visual.ColumnDefinitions.Add(new ColumnDefinition());
                Visual.ColumnDefinitions.Add(new ColumnDefinition());
                Visual.ColumnDefinitions.Add(new ColumnDefinition());
                
                // Create TextBlock for Label of the Marker
                TextBlock = new TextBlock();

                // Apply TextBlock Properties 
                ApplyTextBlockProperties();

                if (TextBackground != null)
                {
                    TextBackgroundCanvas = new Canvas();
                    TextBackgroundCanvas.Background = TextBackground;
                    Visual.Children.Add(TextBackgroundCanvas);
                }

                // Set Alignment
                SetAlignment4Label();
                             

                // Add TextBlock into Visual
                Visual.Children.Add(TextBlock);

                TextBlock.Margin = new Thickness(LabelMargin,0,0,0);
                
                Visual.Margin = new Thickness(Margin, Margin, Margin, Margin);
                
            }
          
#if WPF
            Visual.Measure(new Size(Double.MaxValue, Double.MaxValue));
            MarkerActualSize = Visual.DesiredSize;
#else
            Visual.Measure(new Size(Double.MaxValue, Double.MaxValue));
            MarkerActualSize = Visual.DesiredSize;
#endif
   
        }

        internal Canvas TextBackgroundCanvas
        {
            get;
            set;
        }

        /// <summary>
        /// Set alignment for the Marker label
        /// </summary>
        private void SetAlignment4Label()
        {
            if (TextBlock != null)
            {
 
#if WPF
                TextBlock.Measure(new Size(Double.MaxValue, Double.MaxValue));
                TextBlockSize = new Size(TextBlock.DesiredSize.Width, TextBlock.DesiredSize.Height);
#else
                TextBlockSize = new Size(TextBlock.ActualWidth, TextBlock.ActualHeight);
                
#endif

                if (TextOrientation == Orientation.Vertical)
                {
                    TransformGroup tg = new TransformGroup();
                    RotateTransform rt = new RotateTransform();
                    TranslateTransform tt = new TranslateTransform();

                    tg.Children.Add(rt);
                    tg.Children.Add(tt);

                    switch (TextAlignmentY)
                    {
                        case AlignmentY.Top:

                            switch (TextAlignmentX)
                            {
                                case AlignmentX.Left:

                                    TextBlock.RenderTransformOrigin = new Point(0, 1);
                                    rt.CenterX = 0;
                                    rt.CenterY = 0;
                                    rt.Angle = -90;
                                    tt.X = TextBlockSize.Width;
                                    break;

                                case AlignmentX.Center:

                                    rt.CenterX = TextBlockSize.Width / 2;
                                    rt.CenterY = TextBlockSize.Height / 2;
                                    rt.Angle = -90;
                                    tt.Y = -TextBlockSize.Width / 2 + TextBlockSize.Height / 2 - 5;
                                    break;

                                case AlignmentX.Right:

                                    rt.CenterX = 0;
                                    rt.CenterY = 0;
                                    TextBlock.RenderTransformOrigin = new Point(0, 1);
                                    rt.Angle = -90;
                                    tt.X = TextBlockSize.Height;
                                    break;
                            }
                            break;

                        case AlignmentY.Center:

                            switch (TextAlignmentX)
                            {
                                case AlignmentX.Left:
                                    rt.CenterX = TextBlockSize.Width;
                                    rt.CenterY = TextBlockSize.Height / 2;
                                    rt.Angle = 90;
                                    tt.Y = TextBlockSize.Width / 2;
                                    tt.X = -TextBlockSize.Height / 2;
                                    break;

                                case AlignmentX.Center:
                                    TextBlock.RenderTransformOrigin = new Point(0.5, 0.5);
                                    rt.Angle = 90;
                                    break;

                                case AlignmentX.Right:
                                    rt.CenterX = 0;
                                    rt.CenterY = TextBlockSize.Height / 2;
                                    rt.Angle = -90;
                                    tt.Y = TextBlockSize.Width / 2;
                                    tt.X = TextBlockSize.Height / 2;
                                    break;
                            }

                            break;

                        case AlignmentY.Bottom:

                            switch (TextAlignmentX)
                            {
                                case AlignmentX.Left:
                                    rt.CenterX = TextBlockSize.Width / 2;
                                    rt.CenterY = TextBlockSize.Height / 2;
                                    rt.Angle = -90;
                                    tt.Y = TextBlockSize.Width / 2 - TextBlockSize.Height / 2;
                                    tt.X = TextBlockSize.Width / 2 - TextBlockSize.Height / 2;
                                    break;

                                case AlignmentX.Center:
                                    rt.CenterX = TextBlockSize.Width / 2;
                                    rt.CenterY = TextBlockSize.Height / 2;
                                    rt.Angle = -90;
                                    tt.Y = TextBlockSize.Height + MarkerSize.Height * ScaleFactor;
                                    break;

                                case AlignmentX.Right:
                                    rt.CenterX = 0;
                                    rt.CenterY = 0;
                                    rt.Angle = -90;
                                    tt.Y = TextBlockSize.Width;
                                    break;
                            }
                            break;
                    }

                    TextBlock.RenderTransform = tg;

                    if (TextBackgroundCanvas != null)
                    {
                        TextBackgroundCanvas.RenderTransformOrigin = TextBlock.RenderTransformOrigin;
                        TextBackgroundCanvas.RenderTransform = tg;
                        TextBackgroundCanvas.Height = TextBlockSize.Height;
                        TextBackgroundCanvas.Width = TextBlockSize.Width;
                    }
                }

                if (TextBackgroundCanvas != null)
                {
                    TextBackgroundCanvas.Height = TextBlockSize.Height;
                    TextBackgroundCanvas.Width = TextBlockSize.Width;
                }
                
                if (TextAlignmentX == AlignmentX.Left)
                {
                    TextBlock.SetValue(Grid.ColumnProperty, 0);
                    _markerShapePosition.X = TextBlockSize.Width;

                    if (TextBackgroundCanvas != null)
                    {
                        TextBackgroundCanvas.SetValue(Grid.ColumnProperty, 0);
                    }
                    
                }
                else if (TextAlignmentX == AlignmentX.Right)
                {
                    TextBlock.SetValue(Grid.ColumnProperty, 2);

                    if (TextBackgroundCanvas != null)
                    {
                        TextBackgroundCanvas.SetValue(Grid.ColumnProperty, 2);
                    }
                }
                else
                {
                    TextBlock.SetValue(Grid.ColumnProperty, 1);
                    TextBlock.HorizontalAlignment = HorizontalAlignment.Center;

                    if (TextBackgroundCanvas != null)
                    {
                        TextBackgroundCanvas.SetValue(Grid.ColumnProperty, 1);
                        TextBackgroundCanvas.HorizontalAlignment = HorizontalAlignment.Center;
                    }
                }

                if (TextAlignmentY == AlignmentY.Top)
                {
                    TextBlock.SetValue(Grid.RowProperty, 0);
                    _markerShapePosition.Y = TextBlockSize.Height;

                    if (TextBackgroundCanvas != null)
                    {
                        TextBackgroundCanvas.SetValue(Grid.RowProperty, 0);
                    }
                }
                else if (TextAlignmentY == AlignmentY.Bottom)
                {
                    TextBlock.SetValue(Grid.RowProperty, 2);

                    if (TextBackgroundCanvas != null)
                    {
                        TextBackgroundCanvas.SetValue(Grid.RowProperty, 2);
                    }
                }
                else
                {
                    TextBlock.SetValue(Grid.RowProperty, 1);
                    TextBlock.VerticalAlignment = VerticalAlignment.Center;

                    if (TextBackgroundCanvas != null)
                    {
                        TextBackgroundCanvas.SetValue(Grid.RowProperty, 1);
                        TextBackgroundCanvas.VerticalAlignment = VerticalAlignment.Center;
                    }
                }

                if (TextAlignmentX == AlignmentX.Center)
                    _markerShapePosition.X += Math.Abs((TextBlockSize.Width - MarkerShape.Width) / 2);

                if (TextAlignmentY == AlignmentY.Center)
                    _markerShapePosition.Y += Math.Abs((TextBlockSize.Height - MarkerShape.Height) / 2);
            }


        }


        private Brush GetMarkerShadowColor()
        {

            String xaml = String.Format(@"<LinearGradientBrush xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"" xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"" EndPoint=""0.497000008821487,1.00100004673004"" StartPoint=""0.503000020980835,-0.00100000004749745"">
    				<GradientStop Color=""#B46C6C6C"" Offset=""1""/>
    				<GradientStop Color=""#00FFFFFF"" Offset=""0""/>
    			</LinearGradientBrush>");

#if WPF
            return (Brush)XamlReader.Load(new XmlTextReader(new StringReader(xaml)));
#else
            return System.Windows.Markup.XamlReader.Load(xaml) as Brush;
#endif

        }

        private Double _fontSize;                   // The identifier for property FontSize
        private FontFamily _fontFamily;             // The identifier for property FontFamily
        private FontStyle _fontStyle;               // The identifier for property FontStyle
        private FontWeight _fontWeight;             // The identifier for property FontWeight
        private Brush _fontColor;                   // The identifier for property FontColor

        private Double _borderThickness;            // The identifier for property BorderThickness
        private Brush _borderColor;                 // The identifier for property BorderColor

        private AlignmentX _textAlignmentX;         // HorizontalAlignment of Marker label
        private AlignmentY _textAlignmentY;         // VerticalAlignment of Marker label
        private String _text;                       // The identifier for property Text
        public Brush _textBackground;
        public Double _scaleFactor = 1;
        private Point _markerShapePosition;         // Actual position of the Marker shape inside Marker grid

        private event EventHandler<MouseEventArgs> _onMouseEnter;           // Handler for MouseEnter event
        private event EventHandler<MouseEventArgs> _onMouseLeave;           // Handler for MouseLeave event
    }
}
