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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Markup;
using System.IO;
using System.Xml;


#else

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Browser;

#endif
using Visifire.Charts;

namespace Visifire.Commons
{
    /// <summary>
    /// Visifire.Commons.Marker class
    /// </summary>
    public class Marker : IComparable
    {
        #region Public Methods

        /// <summary>
        /// Initializes a new instance of the Visifire.Commons.Marker class
        /// </summary>
        public Marker(MarkerTypes markerType, Double scaleFactor, Size markerSize, Boolean markerBevel, Brush markerColor, String markerLabelText)
        {
            MarkerType = markerType;
            FillColor = markerColor;
            Text = markerLabelText;
            TextOrientation = Orientation.Horizontal;
            TextAlignmentX = AlignmentX.Right;
            TextAlignmentY = AlignmentY.Center;
            ScaleFactor = scaleFactor;
            MarkerSize = markerSize;
            BorderThickness = 1;
            TextBackground = new SolidColorBrush(Colors.Transparent);
            Bevel = markerBevel;

            Opacity = 1;
        }

        /// <summary>
        /// Initializes a new instance of the Visifire.Commons.Marker class
        /// </summary>
        public Marker()
        {

        }

        /// <summary>
        /// Compare two Markers
        /// </summary>
        /// <param name="o">Object</param>
        /// <returns>int</returns>
        public int CompareTo(Object o)
        {
            Marker dataPoint = (Marker)o;
            return this.Position.X.CompareTo(dataPoint.Position.X);
        }

        public void HideLabel()
        {
            TextBackgroundCanvas.Background = Graphics.TRANSPARENT_BRUSH;
            TextBlock.Foreground = Graphics.TRANSPARENT_BRUSH;
        }

        public void ShowLabel()
        {
            TextBackgroundCanvas.Background = TextBackground;
            TextBlock.Foreground = FontColor;
        }

        /// <summary>
        /// Add elements to parent panel
        /// </summary>
        /// <param name="parentCanvas">ParentCanvas</param>
        /// <param name="xPosition">x position value</param>
        /// <param name="yPosition">y position value</param>
        /// <param name="anchorPoint">AnchorPoint</param>
        public void AddToParent(Canvas parentCanvas, Double xPosition, Double yPosition, Point anchorPoint)
        {
            Position = new Point(xPosition, yPosition);
            parentCanvas.Children.Add(Visual);
            Double visualHeight;
            Double visualWidth;
#if WPF

            Visual.Measure(new Size(Double.MaxValue, Double.MaxValue));
            visualHeight = Visual.DesiredSize.Height;
            visualWidth = Visual.DesiredSize.Width;
#else
            visualHeight = Visual.ActualHeight;
            visualWidth = Visual.ActualWidth;
#endif

            if (anchorPoint.X == 0.5)
                xPosition -= (MarkerShape.Width / 2 + ((TextAlignmentX == AlignmentX.Left) ? _markerShapePosition.X : 0));
            else if (anchorPoint.X == 1)
                xPosition -= (MarkerShape.Width + ((TextAlignmentX == AlignmentX.Left) ? _markerShapePosition.X : 0));

            if (anchorPoint.Y == 0.5)
                yPosition -= (MarkerShape.Height / 2 + ((TextAlignmentY == AlignmentY.Top) ? _markerShapePosition.Y : 0));
            else if (anchorPoint.Y == 1)
                yPosition -= (MarkerShape.Height + ((TextAlignmentY == AlignmentY.Top) ? _markerShapePosition.Y : 0));

            if (TextAlignmentX == AlignmentX.Center)
                if (TextBlockSize.Width > MarkerShape.Width)
                    xPosition -= (TextBlockSize.Width - MarkerShape.Width) / 2;

            if (TextAlignmentY == AlignmentY.Center)
                if (TextBlockSize.Height > MarkerShape.Height)
                    yPosition -= (TextBlockSize.Height - MarkerShape.Height) / 2;

            Visual.SetValue(Canvas.TopProperty, yPosition);
            Visual.SetValue(Canvas.LeftProperty, xPosition);

        }

        /// <summary>
        /// Add elements to parent panel
        /// </summary>
        /// <param name="parent">Panel</param>
        public void AddToParent(Panel parent)
        {
            parent.Children.Add(Visual);
        }

        /// <summary>
        /// Create visual for Marker
        /// </summary>
        public void CreateVisual()
        {
            // Create Visual as Grid
            Visual = new Grid();

            // Create Shape for Marker
            MarkerShape = GetShape();
            MarkerShadow = GetShape();

            if (MarkerShape != null)
                MarkerShape.Tag = Tag;

            if (MarkerShadow != null)
                MarkerShadow.Tag = Tag;

            // Set shadow properties
            MarkerShadow.Fill = GetMarkerShadowColor();
            TranslateTransform tt = new TranslateTransform() { X = 1, Y = 1 };
            MarkerShadow.RenderTransform = tt;

            // display or hide the shadow
            MarkerShadow.Visibility = (ShadowEnabled ? Visibility.Visible : Visibility.Collapsed);

            // Set properties
            MarkerShape.SetValue(Grid.RowProperty, 1);
            MarkerShape.SetValue(Grid.ColumnProperty, 1);
            MarkerShadow.SetValue(Grid.RowProperty, 1);
            MarkerShadow.SetValue(Grid.ColumnProperty, 1);

            // Add Shape for Marker into Visual
            Visual.Children.Add(MarkerShadow);
            Visual.Children.Add(MarkerShape);

            UpdateMarker();

            //if (!String.IsNullOrEmpty(Text))
            {   
                // Define row and columns 
                Visual.RowDefinitions.Add(new RowDefinition());
                Visual.RowDefinitions.Add(new RowDefinition());
                Visual.RowDefinitions.Add(new RowDefinition());
                Visual.ColumnDefinitions.Add(new ColumnDefinition());
                Visual.ColumnDefinitions.Add(new ColumnDefinition());
                Visual.ColumnDefinitions.Add(new ColumnDefinition());

                // Create TextBlock for Label of the Marker
                TextBlock = new TextBlock() { Tag = this.Tag };

                // Apply TextBlock Properties 
                ApplyTextBlockProperties();

                // if (TextBackground != null)
                {
                    TextBackgroundCanvas = new Canvas();
                    TextBackgroundCanvas.Background = TextBackground;
                    Visual.Children.Add(TextBackgroundCanvas);
                }

                // Set Alignment
                SetAlignment4Label();


                // Add TextBlock into Visual
                Visual.Children.Add(TextBlock);

                TextBlock.Margin = new Thickness(LabelMargin, 0, 0, 0);

                Visual.Margin = new Thickness(Margin, Margin, Margin, Margin);
            }

            MarkerShape.Opacity = Opacity;

            MarkerActualSize = Graphics.CalculateVisualSize(Visual);
        }

        /// <summary>
        /// Update Marker with new properties
        /// </summary>
        public void UpdateMarker()
        {
            if (BevelLayer != null)
                Visual.Children.Remove(BevelLayer);

            if (Bevel)
            {
                BevelLayer = GetBevelLayer();
                BevelLayer.SetValue(Grid.RowProperty, 1);
                BevelLayer.SetValue(Grid.ColumnProperty, 1);
                BevelLayer.SetValue(Grid.RowProperty, 1);
                BevelLayer.SetValue(Grid.ColumnProperty, 1);
                Visual.Children.Add(BevelLayer);
            }

            ApplyMarkerShapeProperties();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Tag property
        /// </summary>
        public Object Tag
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set the Marker Visual
        /// </summary>
        public Grid Visual
        {
            get;
            set;
        }

        /// <summary>
        /// Whether the Bevel effect is applied on Marker
        /// </summary>
        public Boolean Bevel
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set scale factor of Marker symbol
        /// </summary>
        public Double ScaleFactor
        {
            get
            {
                return _scaleFactor;
            }
            set
            {
                // System.Diagnostics.Debug.Assert(value < 1, "ScaleFactor can not be less than 1..");
                if (value > 1)
                    _scaleFactor = value;
            }
        }

        /// <summary>
        /// Get or set padding for label of a Marker
        /// </summary>
        public Double LabelPadding
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set Position of the Marker
        /// </summary>
        public Point Position
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set the Actual Size of Marker
        /// </summary>
        public Size MarkerSize
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set the Text orientation of Marker label
        /// </summary>
        public Orientation TextOrientation
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set the HorizontalAlignment of Marker label
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
        /// Get or set the VerticalAlignment of Marker label
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
        /// Get or set the Shape type of a Marker
        /// </summary>
        public MarkerTypes MarkerType
        {
            get;
            set;
        }

        public Double Opacity
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set the Margin of Marker
        /// </summary>
        public Double Margin
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set the LabelMargin of the label of Marker
        /// </summary>
        public Double LabelMargin
        {
            get;
            set;
        }

        #region Font Properties

        public Boolean LabelEnabled
        {
            get
            {
                return _labelEnabled;
            }
            set
            {
                _labelEnabled = value;

                if (Visual != null)
                    if (value)
                        ShowLabel();
                    else
                        HideLabel();
            }
        }

        /// <summary>
        /// Get or set the marker label FontFamily
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
        /// Get or set the marker label FontSize
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
        /// Get or set the marker label FontColor
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

                if (TextBlock != null)
                    TextBlock.Foreground = _fontColor;
            }
        }

        /// <summary>
        /// Get or set the marker label FontStyle
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

                if (TextBlock != null)
                    TextBlock.FontStyle = _fontStyle;
            }
        }

        /// <summary>
        /// Get or set the marker label FontWeight
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

                if (TextBlock != null)
                    TextBlock.FontWeight = _fontWeight;
            }
        }

        /// <summary>
        /// Get or set the marker label Text
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

                if (TextBlock != null)
                {
                    TextBlock.Text = _text;
                    Size size = Graphics.CalculateVisualSize(TextBlock);
                    TextBackgroundCanvas.Height = size.Height;
                    TextBackgroundCanvas.Width = size.Width;
                }
            }
        }


        /// <summary>
        /// Get or set the background of marker label text 
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

                if (TextBlock != null)
                    TextBlock.Text = _text;
            }
        }

        #endregion

        #region Border Properties

        /// <summary>
        /// Get or set the BorderColor
        /// </summary>
        public Brush BorderColor
        {
            get
            {   
                if (_borderColor == null)
                    _borderColor = FillColor;

                return _borderColor;
            }
            set
            {
                _borderColor = value;

                if (MarkerShape != null)
                    MarkerShape.Stroke = value;
            }
        }

        /// <summary>
        /// Get or set the BorderThickness
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

                if (MarkerShape != null)
                    MarkerShape.StrokeThickness = value;
            }
        }

        /// <summary>
        /// Get or set the Marker fill color
        /// </summary>
        public Brush FillColor
        {
            get
            {   
                return _markerFillColor;
            }
            set
            {
                _markerFillColor = value;

                if (MarkerShape != null)
                    MarkerShape.Fill = value;
            }
        }

        #endregion

        #endregion

        #region Public Events And Delegates

        #endregion

        #region Protected Methods

        #endregion

        #region Internal Properties

        /// <summary>
        /// Marker shape reference
        /// </summary>
        internal Shape MarkerShape
        {
            get;
            set;
        }

        /// <summary>
        /// BevelLayer of the Marker shape
        /// </summary>
        internal FrameworkElement BevelLayer
        {
            get;
            set;
        }

        /// <summary>
        /// MarkerShadow of the Marker shape
        /// </summary>
        internal Shape MarkerShadow
        {
            get;
            set;
        }

        /// <summary>
        /// Control reference
        /// </summary>
        internal VisifireControl Control
        {
            get;
            set;
        }

        /// <summary>
        /// TextBlock for Marker
        /// </summary>
        internal TextBlock TextBlock
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
        /// Size of the TextBlock
        /// </summary>
        internal Size TextBlockSize
        {
            get;
            set;
        }

        /// <summary>
        /// Background of the label of the Marker
        /// </summary>
        internal Canvas TextBackgroundCanvas
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
        /// DataSeries used in Legend for line chart
        /// </summary>
        internal DataSeries DataSeriesOfLegendMarker
        {
            get;
            set;
        }

        #endregion

        #region Private Properties

        #endregion

        #region Private Delegates

        #endregion

        #region Private Methods

        /// <summary>
        /// Apply all style properties of the Marker text
        /// </summary>
        private void ApplyMarkerShapeProperties()
        {
            // Set Border Properties
            if (MarkerShape != null)
            {
                MarkerShape.Stroke = BorderColor;
                MarkerShape.Fill = FillColor;
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
#if WPF
                TextBlock.FlowDirection = FlowDirection.LeftToRight;
#endif
                TextBlock.FontFamily = FontFamily;
                TextBlock.FontSize = FontSize;
                TextBlock.FontStyle = FontStyle;
                TextBlock.FontWeight = FontWeight;
                TextBlock.Text = Text;
                TextBlock.Foreground = FontColor;
                TextBlock.VerticalAlignment = VerticalAlignment.Top;
            }
        }

        /// <summary>
        /// Create shape for Marker
        /// </summary>
        /// <returns>Shape</returns>
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
        /// <returns>FrameworkElement</returns>
        private FrameworkElement GetBevelLayer()
        {
            String xaml = null;
            Brush topBrush = Graphics.GetBevelTopBrush(FillColor);
            String color;

            switch (MarkerType)
            {
                case MarkerTypes.Circle:

                    Ellipse ellipse = new Ellipse() { Height = 6, Width = 6 };

                    GradientStopCollection gsc = new GradientStopCollection();
                    gsc.Add(new GradientStop() { Offset = 0, Color = Graphics.GetDarkerColor((topBrush as LinearGradientBrush).GradientStops[1].Color, .8) });
                    gsc.Add(new GradientStop() { Offset = 0.6, Color = Graphics.GetDarkerColor((topBrush as LinearGradientBrush).GradientStops[1].Color, .8) });
                    gsc.Add(new GradientStop() { Offset = 0.8, Color = (topBrush as LinearGradientBrush).GradientStops[1].Color });
                    gsc.Add(new GradientStop() { Offset = 1, Color = (topBrush as LinearGradientBrush).GradientStops[1].Color });

                    ellipse.Fill = new LinearGradientBrush() { GradientStops = gsc, StartPoint = new Point(0.5, 1), EndPoint = new Point(0.5, 0) };

                    return ellipse;

                case MarkerTypes.Cross:

                    topBrush = Graphics.GetBevelTopBrush(BorderColor);
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

                    Canvas bevelCanvas = Visifire.Charts.ExtendedGraphics.Get2DRectangleBevel(this.Tag as FrameworkElement, width, height,
                    3, 3,
                    topBrush,
                    Graphics.GetBevelSideBrush(0, FillColor),
                    Graphics.GetBevelSideBrush(120, FillColor),
                    Graphics.GetBevelSideBrush(100, FillColor)
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
        /// Sets values for rotate transform 
        /// </summary>
        /// <param name="rotateTransform">Ref RotateTransform</param>
        /// <param name="centerX">CenterX</param>
        /// <param name="centerY">CenterY</param>
        /// <param name="angle">Angle</param>
        private void SetRotateTransformValues(ref RotateTransform rotateTransform, Double centerX, Double centerY, Double angle)
        {
            rotateTransform.CenterX = centerX;
            rotateTransform.CenterY = centerY;
            rotateTransform.Angle = angle;
        }

        /// <summary>
        /// Set alignment for the Marker label
        /// </summary>
        private void SetAlignment4Label()
        {
            if (TextBlock != null)
            {

#if WPF
                TextBlockSize = Graphics.CalculateVisualSize(TextBlock);
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
                                    SetRotateTransformValues(ref rt, 0, 0, -90);
                                    tt.X = TextBlockSize.Width;
                                    break;

                                case AlignmentX.Center:

                                    SetRotateTransformValues(ref rt, TextBlockSize.Width / 2, TextBlockSize.Height / 2, -90);
                                    tt.Y = -TextBlockSize.Width / 2 + TextBlockSize.Height / 2 - 5;
                                    break;

                                case AlignmentX.Right:
                                    TextBlock.RenderTransformOrigin = new Point(0, 1);
                                    SetRotateTransformValues(ref rt, 0, 0, -90);
                                    tt.X = TextBlockSize.Height;
                                    break;
                            }
                            break;

                        case AlignmentY.Center:

                            switch (TextAlignmentX)
                            {
                                case AlignmentX.Left:
                                    SetRotateTransformValues(ref rt, TextBlockSize.Width, TextBlockSize.Height / 2, 90);
                                    tt.Y = TextBlockSize.Width / 2;
                                    tt.X = -TextBlockSize.Height / 2;
                                    break;

                                case AlignmentX.Center:
                                    TextBlock.RenderTransformOrigin = new Point(0.5, 0.5);
                                    rt.Angle = 90;
                                    break;

                                case AlignmentX.Right:
                                    SetRotateTransformValues(ref rt, 0, TextBlockSize.Height / 2, -90);
                                    tt.Y = TextBlockSize.Width / 2;
                                    tt.X = TextBlockSize.Height / 2;
                                    break;
                            }

                            break;

                        case AlignmentY.Bottom:

                            switch (TextAlignmentX)
                            {
                                case AlignmentX.Left:
                                    SetRotateTransformValues(ref rt, TextBlockSize.Width / 2, TextBlockSize.Height / 2, -90);

                                    tt.Y = TextBlockSize.Width / 2 - TextBlockSize.Height / 2;
                                    tt.X = TextBlockSize.Width / 2 - TextBlockSize.Height / 2;
                                    break;

                                case AlignmentX.Center:

                                    SetRotateTransformValues(ref rt, TextBlockSize.Width / 2, TextBlockSize.Height / 2, -90);
                                    tt.Y = TextBlockSize.Height + MarkerSize.Height * ScaleFactor;
                                    break;

                                case AlignmentX.Right:

                                    SetRotateTransformValues(ref rt, 0, 0, -90);
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

                SetLabelBackgroundAndSymbolPosition();
            }
        }

        /// <summary>
        /// Set background of label and position of symbol
        /// </summary>
        private void SetLabelBackgroundAndSymbolPosition()
        {

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

        /// <summary>
        /// Get shadow color of the Marker shape
        /// </summary>
        /// <returns></returns>
        private Brush GetMarkerShadowColor()
        {

//            String xaml = String.Format(@"<LinearGradientBrush xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"" xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"" EndPoint=""0.497000008821487,1.00100004673004"" StartPoint=""0.503000020980835,-0.00100000004749745"">
//    				<GradientStop Color=""#B46C6C6C"" Offset=""1""/>
//    				<GradientStop Color=""#00FFFFFF"" Offset=""0""/>
//    			</LinearGradientBrush>");

            LinearGradientBrush brush = new LinearGradientBrush();
            brush.StartPoint = new Point(0.497000008821487, 1.00100004673004);
            brush.EndPoint = new Point(0.503000020980835, -0.00100000004749745);

            brush.GradientStops.Add(new GradientStop() { Color = Color.FromArgb(180, 108, 108, 108), Offset = 1 });
            brush.GradientStops.Add(new GradientStop() { Color = Color.FromArgb(00, 255, 255, 255), Offset = 1 });

            return brush;

#if WPF
            //return (Brush)XamlReader.Load(new XmlTextReader(new StringReader(xaml)));
#else
            //return System.Windows.Markup.XamlReader.Load(xaml) as Brush;
#endif
        }

        #endregion

        #region Internal Methods

        #endregion

        #region Internal Events And Delegates

        #endregion

        #region Data

        private Boolean _labelEnabled;

        private Brush _textBackground;

        /// <summary>
        /// The identifier for property FontSize
        /// </summary>
        private Double _fontSize;

        /// <summary>
        /// The identifier for property FontFamily
        /// </summary>
        private FontFamily _fontFamily;

        /// <summary>
        /// The identifier for property FontStyle
        /// </summary>
        private FontStyle _fontStyle;

        /// <summary>
        /// The identifier for property FontWeight
        /// </summary>
        private FontWeight _fontWeight;

        /// <summary>
        /// The identifier for property FontColor
        /// </summary>
        private Brush _fontColor;

        /// <summary>
        /// The identifier for property BorderThickness
        /// </summary>
        private Double _borderThickness;

        private Brush _markerFillColor;

        /// <summary>
        /// The identifier for property BorderColor
        /// </summary>
        private Brush _borderColor;

        /// <summary>
        /// HorizontalAlignment of Marker label
        /// </summary>
        private AlignmentX _textAlignmentX;

        /// <summary>
        /// VerticalAlignment of Marker label
        /// </summary>
        private AlignmentY _textAlignmentY;

        /// <summary>
        /// The identifier for property Text
        /// </summary>
        private String _text;

        /// <summary>
        /// The identifier for ScaleFactor property 
        /// </summary>
        private Double _scaleFactor = 1;

        /// <summary>
        /// Actual position of the Marker shape inside Marker grid
        /// </summary>
        private Point _markerShapePosition;

        #endregion
    }
}
