/*
      Copyright (C) 2008 Webyog Softworks Private Limited

     This file is part of VisifireCharts.
 
     VisifireCharts is a free software: you can redistribute it and/or modify
     it under the terms of the GNU General Public License as published by
     the Free Software Foundation, either version 3 of the License, or
     (at your option) any later version.
 
     VisifireCharts is distributed in the hope that it will be useful,
     but WITHOUT ANY WARRANTY; without even the implied warranty of
     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
     GNU General Public License for more details.
 
     You should have received a copy of the GNU General Public License
     along with VisifireCharts.  If not, see <http://www.gnu.org/licenses/>.
 
*/

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Markup;

namespace Visifire.Commons
{
    /// <summary>
    /// Legend base
    /// </summary>
    public abstract class LegendBase : Visifire.Commons.VisualObject
    {
        #region Public Methods

        public LegendBase()
        {
            lineStrokeXaml = String.Format(@"<Path.Stroke>
			    <LinearGradientBrush xmlns=""http://schemas.microsoft.com/client/2007"" EndPoint=""0.174,0.273"" StartPoint=""0.826,0.727"">
				    <GradientStop Color=""#FFD4D4D4"" Offset=""0""/>
				    <GradientStop Color=""#FEE8E5E5"" Offset=""1""/>
				    <GradientStop Color=""#FF000000"" Offset=""0.518""/>
			    </LinearGradientBrush>
		    </Path.Stroke>");
            lineStrokeLeftXaml = String.Format(@"<Path.Stroke xmlns=""http://schemas.microsoft.com/client/2007"">
			<LinearGradientBrush EndPoint=""0.008,0.833"" StartPoint=""0.998,0.833"">
				<GradientStop Color=""#FF000000"" Offset=""1""/>
				<GradientStop Color=""#FFFFFFFF"" Offset=""0""/>
			</LinearGradientBrush>
		</Path.Stroke>");
            lineStrokeRightXaml = String.Format(@"<Path.Stroke xmlns=""http://schemas.microsoft.com/client/2007"">
			<LinearGradientBrush EndPoint=""0.998,0.833"" StartPoint=""0.008,0.833"">
				<GradientStop Color=""#FF000000"" Offset=""1""/>
				<GradientStop Color=""#FFFFFFFF"" Offset=""0""/>
			</LinearGradientBrush>
		</Path.Stroke>");
            SetDefaults();

        }

        public override void SetHeight()
        {
            throw new NotImplementedException();
        }
        public override void SetWidth()
        {
            throw new NotImplementedException();
        }
        public override void SetLeft()
        {
            throw new NotImplementedException();
        }
        public override void SetTop()
        {
            throw new NotImplementedException();
        }

        public override void Init()
        {
            _currentTop = Padding;
            _currentLeft = Padding;
            _currentHeight = Padding;
            _currentWidth = Padding;
            if (!_alignmentXChanged && DockInsidePlotArea) _alignmentX = AlignmentX.Right;
            if (!_alignmentYChanged && DockInsidePlotArea) _alignmentY = AlignmentY.Top;
        }

        /// <summary>
        /// Set a default Name. This is usefull if user has not specified this object in data XML and it has been 
        /// created by default.
        /// </summary>
        public virtual void SetName()
        {
            this.SetValue(NameProperty, this.GetType().Name + Index.ToString());

            _index++;
        }

        /// <summary>
        /// Set the position of the title
        /// </summary>
        /// <Tips> Call this function after rendering the legend and before setting the border of legend. </Tips>
        public void SetTitleOfLegendPosition()
        {
            if (!String.IsNullOrEmpty(Title))
            {
                if (this.Width < _title.ActualWidth)
                {
                    Double titleHeight = _title.ActualHeight;
                    _title.Width = this.Width;
                    _title.TextWrapping = TextWrapping.Wrap;

                    _entryCanvas.SetValue(TopProperty, (Double)_entryCanvas.GetValue(TopProperty) + _title.ActualHeight - titleHeight);
                    this.Height += _title.ActualHeight - titleHeight;

                    _titleUnderLine.Y1 += _title.ActualHeight - titleHeight;
                    _titleUnderLine.Y2 += _title.ActualHeight - titleHeight;
                }

                // Set title left position. Note: top of title is already set while adding title
                if (TitleAlignmentX == AlignmentX.Center)
                    _title.SetValue(LeftProperty, (this.Width - _title.ActualWidth) / 2);
                else if (TitleAlignmentX == AlignmentX.Left)
                    _title.SetValue(LeftProperty, Padding);
                else
                    _title.SetValue(LeftProperty, (this.Width - _title.ActualWidth - Padding));

                // Set the under line width
                _titleUnderLine.X2 = this.Width;

                // Set title background position
                if (_titleBackground != null)
                {
                    _titleBackground.Fill = TitleBackground;
                    _titleBackground.SetValue(TopProperty, (Double)_title.GetValue(TopProperty) - 0.05);
                    _titleBackground.Height = _title.ActualHeight + 0.09;
                    _titleBackground.Width = this.Width; 
                    _titleBackground.SetValue(LeftProperty, 0);
                }
            }
            else
            {
                _entryCanvas.SetValue(TopProperty, (Double)_entryCanvas.GetValue(TopProperty) - Spacing + BorderThickness);
                this.Height +=  Padding - Spacing;
            }

            this.Height += BorderThickness; 


            if (this.DockInsidePlotArea == false)
                this.Width += Padding - Spacing;

        }

        /// <summary>
        /// Set the max height and width of the legend
        /// </summary>
        public virtual void SetMaxWidthHeight(Rect titleBounds, Double margin)
        {
            if (Enabled == true)
            {
                if (Double.IsNaN(_maxWidth))
                {
                    _maxWidth = titleBounds.Width - 2 * margin;
                }
                if (Double.IsNaN(_maxHeight))
                {
                    _maxHeight = titleBounds.Height - 2 * margin;
                }
            }
            else
            {
                _maxHeight = 0;
                _maxWidth = 0;

                this.SetValue(WidthProperty, 0);
                this.SetValue(HeightProperty, 0);

                this.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Add an entry for Background
        /// </summary>
        /// <returns>Unable to add a entry</returns>
        public Boolean DrawEntryInGridLayout(FrameworkElement symbol, String legendText)
        {
            if (_title == null && !String.IsNullOrEmpty(Title))
                DrawTitleEntry();

            if (_entryCanvas == null)
            {
                _entryCanvas = new Canvas();
                this.Children.Add(_entryCanvas);
            }

            // Create new legend entry
            Canvas entry = CreateEntry(symbol, legendText);

            // unable to add the current entry ?
            if (_currentLeft + entry.Width > _maxWidth)
            {
                this.Width = _maxWidth;

                // Place title at center
                if (_title == null && !String.IsNullOrEmpty(Title))
                    _title.SetValue(LeftProperty, (this.Width - _title.ActualHeight) / 2);

                return false;
            }

            Rect tempPosition = new Rect(_currentLeft, _currentTop, entry.Width + Padding, entry.Height + Padding);

            if (tempPosition.Top + tempPosition.Height > _maxHeight)
            {
                // try to put at new column
                _currentLeft += _currentWidth + Spacing;

                // Going for a new column
                _currentWidth = 0;
                _currentTop = Padding + ((_title != null) ? _title.ActualHeight : 0);

            }
            else
            {   
                // Fix current width of the legend for current column
                _currentWidth = Math.Max(_currentWidth, entry.Width);
            }

            _currentHeight = ((_currentTop + entry.Height + Padding) > _maxHeight) ? _currentHeight : _currentTop + entry.Height + Spacing;

            // Set entry position 
            entry.SetValue(LeftProperty, _currentLeft);
            entry.SetValue(TopProperty, _currentTop);

            _currentTop += entry.Height + Spacing;
            this.Width = _currentLeft + ((_currentWidth == 0) ? entry.Width : _currentWidth) + Padding;
            _currentWidth = Math.Max(_currentWidth, entry.Width);
            this.Height = (this.Height > _currentHeight) ? this.Height : _currentHeight;
            
            // Add entry
            _entryCanvas.Children.Add(entry);
            _entryCanvas.SetValue(ZIndexProperty, 5);
            return true;
        }

        /// <summary>
        /// Add an entry for Background
        /// </summary>
        /// <param name="nextTop"></param>
        /// <param name="bin"></param>
        /// <returns>Unable to add a entry</returns>
        public Boolean DrawEntryInFlowLayout(FrameworkElement symbol, String legendText)
        {
            Boolean flagHeightFixed = false;      // If height cannot be changed farther but the current 
            // row is enough capable of plotting entries then flag is true

            if (_title == null && !String.IsNullOrEmpty(Title))
                DrawTitleEntry();

            if (_entryCanvas == null)
            {
                _entryCanvas = new Canvas();
                this.Children.Add(_entryCanvas);
            }

            Canvas entry = CreateEntry(symbol, legendText);

            // unable to add the current entry ?
            if (_currentTop + entry.Height > _maxHeight)
            {
                this.Height = _maxHeight;

                // Place title at center
                if (_title == null && !String.IsNullOrEmpty(Title))
                    _title.SetValue(LeftProperty, (this.Width - _title.ActualWidth) / 2);

                if (_currentTop < this.Height)
                    // legend height is fixed Farther changes in legend height is not allowed
                    flagHeightFixed = true;
                else
                {
                    SetTitleOfLegendPosition();
                    return false;
                }
            }

            Rect tempPosition = new Rect(_currentLeft, _currentTop, entry.Width + Padding, entry.Height + Padding);

            if (tempPosition.Left + tempPosition.Width > _maxWidth)
            {
                // try to put at new column
                _currentTop += _currentHeight + Spacing;

                // Going for a new column
                _currentHeight = 0;
                _currentLeft = Padding; ;

            }
            else
            {
                // Fix current width of the legend for current column
                _currentHeight = Math.Max(_currentHeight, entry.Height);
            }

            _currentWidth = ((_currentLeft + entry.Width + Padding) > _maxWidth) ? _currentWidth : _currentLeft + entry.Width + Spacing;

            entry.SetValue(TopProperty, _currentTop);
            entry.SetValue(LeftProperty, _currentLeft);

            _currentLeft += entry.Width + Spacing;

            if (!flagHeightFixed)
                this.Height = _currentTop + ((_currentHeight == 0) ? entry.Height : _currentHeight) + Padding;

            _currentHeight = Math.Max(_currentHeight, entry.Height);
            this.Width = (this.Width > _currentWidth) ? this.Width : _currentWidth;

            _entryCanvas.Children.Add(entry);
            _entryCanvas.SetValue(ZIndexProperty, 5);
            return true;
        }

        #endregion Public Methods

        #region Public properties

        
        /// <summary>
        /// Max width of the legend
        /// </summary>
        public Double MaxWidth
        {
            get
            {
                return _maxWidth;
            }
            set
            {
                _maxWidth = value;
            }
        }

        /// <summary>
        /// Max height of the legend
        /// </summary>
        public Double MaxHeight
        {
            get
            {
                return _maxHeight;
            }
            set
            {
                _maxHeight = value;
            }
        }

        /// <summary>
        /// Whether the legend is dock inside plot area
        /// </summary>
        public Boolean DockInsidePlotArea
        {
            get;
            set;
        }

        /// <summary>
        /// Horizontal alignment of legend
        /// </summary>
        public AlignmentX AlignmentX
        {
            get
            {
                if (_alignmentXChanged)
                    return _alignmentX;
                else if (GetFromTheme("AlignmentX") != null)
                {
                    switch (Convert.ToString(GetFromTheme("AlignmentX")))
                    {
                        case "Center":
                            return AlignmentX.Center;
                            
                        case "Left":
                            return AlignmentX.Left;
                            
                        case "Right":
                            return AlignmentX.Right;
                            
                        default:
                            return AlignmentX.Center;
                    }
                }
                else
                    return AlignmentX.Center;

            }
            set
            {
                _alignmentX = value;
                _alignmentXChanged = true;
            }
        }

        /// <summary>
        /// Vertical alignment of legend
        /// </summary>
        public AlignmentY AlignmentY
        {
            get
            {
                if (_alignmentYChanged)
                    return _alignmentY;
                else if (GetFromTheme("AlignmentY") != null)
                {
                    switch (Convert.ToString(GetFromTheme("AlignmentY")))
                    {
                        case "Center":
                            return AlignmentY.Center;
                            
                        case "Top":
                            return AlignmentY.Top;
                            
                        case "Bottom":
                            return AlignmentY.Bottom;
                            
                        default:
                            return AlignmentY.Top;
                    }
                }
                else
                    return AlignmentY.Top;

            }
            set
            {
                _alignmentY = value;
                _alignmentYChanged = true;
            }
        }

        /// <summary>
        /// Inner padding of legend
        /// </summary>
        public Double Padding
        {
            get
            {   
                if (Double.IsNaN(_padding) || _padding < 0)
                {
                    return 3;
                }
                else
                    return _padding;
            }
            set
            {
                _padding = value;
                _currentLeft = _padding;
                _currentTop = _padding;
            }
        }

        /// <summary>
        /// Spacing among legend entries.
        /// </summary>
        protected Double Spacing
        {
            get
            {
                if (Double.IsNaN(_spacing))
                {
                    _spacing = Padding * 25 / 100 + 1;
                }
                else if (_spacing < 0)
                {
                    
                    _spacing = 1;
                }

                return _spacing;
            }
            set
            {
                _spacing = value;
            }
        }

        /// <summary>
        /// Background of legend
        /// </summary>
        public new Brush Background
        {
            get
            {
                return base.Background;
            }
            set
            {
                base.Background = value;
            }
        }

        /// <summary>
        /// Index of legend
        /// </summary>
        public static Int32 Index
        {
            get
            {
                return _index;
            }
            set
            {
                _index = value;
            }
        }

        #region Title Properties

        /// <summary>
        /// Text of the title
        /// </summary>
        public String Title
        {
            get;
            set;
        }

        /// <summary>
        /// Background of title
        /// </summary>
        public Brush TitleBackground
        {
            get;
            set;
        }

        /// <summary>
        /// Horizontal alignment of title
        /// </summary>
        public AlignmentX TitleAlignmentX
        {
            get;
            set;
        }

        /// <summary>
        /// Family of font
        /// </summary>
        public String TitleFontFamily
        {
            get
            {
                if (String.IsNullOrEmpty(_titleFontFamily))
                    return "Verdana"; // Default value
                else
                    return _titleFontFamily;
            }
            set
            {
                _titleFontFamily = value;
            }
        }

        /// <summary>
        /// Size of the font
        /// </summary>
        public Double TitleFontSize
        {
            get
            {
                if (Double.IsNaN(_titleFontSize))
                    return 10; // Default value                                
                else
                    return _titleFontSize;
            }
            set
            {
                _titleFontSize = value;
            }
        }

        /// <summary>
        /// Color of the font
        /// </summary>
        public Brush TitleFontColor
        {
            get
            {
                if (_titleFontColor == null)
                    return new SolidColorBrush(Colors.Black);  // Default value
                else
                    return _titleFontColor;
            }
            set
            {
                _titleFontColor = value;
            }
        }

        /// <summary>
        /// Style of the font
        /// </summary>
        public String TitleFontStyle
        {
            get
            {
                return _titleFontStyle.ToString();
            }
            set
            {
                _titleFontStyle = Converter.StringToFontStyle(value);
            }
        }

        /// <summary>
        /// Title Font weights
        /// </summary>
        public String TitleFontWeight
        {
            get
            {
                if (_titleFontWeight == null)
                    return "Normal";
                else
                    return _titleFontWeight.ToString();
            }
            set
            {
                _titleFontWeight = Converter.StringToFontWeight(value);
            }
        }



        #endregion

        #region Label Font Properties

        /// <summary>
        /// Family of font
        /// </summary>
        public String FontFamily
        {
            get
            {
                if (String.IsNullOrEmpty(_fontFamily))
                    return "Verdana"; // Default value
                else
                    return _fontFamily;
            }
            set
            {
                _fontFamily = value;
            }
        }

        /// <summary>
        /// Size of the font
        /// </summary>
        public Double FontSize
        {
            get
            {
                if (Double.IsNaN(_fontSize))
                    return 10; // Default value                                
                else
                    return _fontSize;
            }
            set
            {
                _fontSize = value;
            }
        }

        /// <summary>
        /// Color of the font
        /// </summary>
        public Brush FontColor
        {
            get
            {
                return _fontColor;
            }
            set
            {
                _fontColor = value;
            }
        }

        /// <summary>
        /// Style of the font
        /// </summary>
        public String FontStyle
        {
            get
            {
                return _fontStyle.ToString();
            }
            set
            {
                _fontStyle = Converter.StringToFontStyle(value);
            }
        }

        /// <summary>
        ///  Font weights
        /// </summary>
        public String FontWeight
        {
            get
            {
                if (_fontWeight == null)
                    return "Normal";
                else
                    return _fontWeight.ToString();
            }
            set
            {
                _fontWeight = Converter.StringToFontWeight(value);
            }
        }


        #endregion

        #endregion

        #region Internal Properties
        
        #endregion Internal properties

        #region Private Methods

        /// <summary>
        /// Calculates font sizes from chart size for the Title
        /// </summary>
        /// <returns></returns>
        private int CalculateTitleFontSize()
        {
            int[] fontSizes = { 6, 8, 10, 12, 14, 16, 18, 20, 24, 28, 32, 36, 40 };
            Double _parentSize = (Parent as FrameworkElement).Width * (Parent as FrameworkElement).Height;
            int i = (int)(Math.Ceiling(((_parentSize + 10000) / 115000)));
            i = (i >= fontSizes.Length ? fontSizes.Length - 1 : i);
            return fontSizes[i];
        }

        /// <summary>
        /// Returns a suitable font color for the Title
        /// </summary>
        /// <returns></returns>
        private Brush GetTitleFontColor()
        {
            if (TitleBackground == null)
            {
                
                Double intensity;
                if ((this.Parent as VisualObject).Background == null)
                {
                    return new SolidColorBrush(Colors.Black);
                }
                else
                {
                    
                        
                    intensity = Parser.GetBrushIntensity((this.Parent as VisualObject).Background);
                    if (intensity <= 0.5)
                    {
                        
                        return Parser.ParseSolidColor("#BBBBBB");
                    }
                    else
                    {
                        return new SolidColorBrush(Colors.Black);
                    }
                    
                }
            }
            else
                return TitleBackground;
        }

        /// <summary>
        /// Returns a suitable font color for the Legend text
        /// </summary>
        /// <returns></returns>
        private Brush GetFontColor()
        {
            if (FontColor == null)
            {
                
                Double intensity;
                if ((this.Parent as VisualObject).Background == null)
                {
                    return new SolidColorBrush(Colors.Black);
                }
                else
                {
                    
                    intensity = Parser.GetBrushIntensity((this.Parent as VisualObject).Background);
                    if (intensity <= 0.5)
                    {
                        
                        return Parser.ParseSolidColor("#BBBBBB");
                    }
                    else
                    {
                        return new SolidColorBrush(Colors.Black);
                    }
                    
                }
            }
            else
                return FontColor;
        }

        /// <summary>
        /// Calculates font sizes from chart size for the Legend fonts
        /// </summary>
        /// <returns></returns>
        private int CalculateFontSize()
        {
            int[] fontSizes = { 6, 8, 10, 12, 14, 16, 18, 20, 24, 28, 32, 36, 40 };
            Double _parentSize = (Parent as FrameworkElement).Width * (Parent as FrameworkElement).Height;
            int i = (int)(Math.Ceiling(((_parentSize + 10000) / 115000)));
            i = (i >= fontSizes.Length ? fontSizes.Length - 1 : i);
            return fontSizes[i];
        }

        /// <summary>
        /// Apply Font style properties
        /// </summary>
        /// <param name="line"></param>
        private void ApplyTitleFontStyle()
        {
            _title.FontFamily = new FontFamily(TitleFontFamily);
            _title.Foreground = Cloner.CloneBrush(GetTitleFontColor());
            _title.FontStyle = Converter.StringToFontStyle(TitleFontStyle);
            _title.FontWeight = Converter.StringToFontWeight(TitleFontWeight);
            if (Double.IsNaN(_titleFontSize))
            {
                _title.FontSize = CalculateTitleFontSize();
            }
            else
                _title.FontSize = TitleFontSize;
            _title.SetValue(ZIndexProperty, 1);
        }

        /// <summary>
        /// Draws entry for title
        /// </summary>
        private void DrawTitleEntry()
        {
            _title = new TextBlock();
            _title.Text = Title;
            ApplyTitleFontStyle();

            this.Children.Add(_title);

            
            _currentTop = BorderThickness;
            

            _currentLeft = Padding;
            _title.SetValue(LeftProperty, _currentLeft);
            _title.SetValue(TopProperty, _currentTop);

            _titleUnderLine = new Line();
            _titleUnderLine.StrokeThickness = 0.4;

            switch (TitleAlignmentX)
            {
                case AlignmentX.Left:
                    _titleUnderLine.Stroke = (Brush)XamlReader.Load(lineStrokeLeftXaml);
                    break;
                case AlignmentX.Right:
                    _titleUnderLine.Stroke = (Brush)XamlReader.Load(lineStrokeRightXaml);
                    break;
                case AlignmentX.Center:
                    _titleUnderLine.Stroke = (Brush)XamlReader.Load(lineStrokeXaml);
                    break;
            }

            _titleUnderLine.X1 = 0;
            _titleUnderLine.Y1 = (Double)_title.GetValue(TopProperty) + _title.ActualHeight;
            _titleUnderLine.X2 = this.Width;
            _titleUnderLine.Y2 = (Double)_title.GetValue(TopProperty) + _title.ActualHeight;
            this.Children.Add(_titleUnderLine);

            // Add title background
            if (TitleBackground != null)
            {
                _titleBackground = new Rectangle();
                this.Children.Add(_titleBackground);
            }

            _currentHeight = _currentTop + _title.ActualHeight + _titleUnderLine.StrokeThickness;
            _currentTop += _title.ActualHeight + _titleUnderLine.StrokeThickness + Spacing;

        }

        /// <summary>
        ///  Apply Font style properties
        /// </summary>
        /// <param name="line"></param>
        private void ApplyFontStyle(TextBlock textBlock)
        {
            textBlock.FontFamily = new FontFamily(FontFamily);
            textBlock.Foreground = Cloner.CloneBrush(GetFontColor());
            textBlock.FontStyle = Converter.StringToFontStyle(FontStyle);
            textBlock.FontWeight = Converter.StringToFontWeight(FontWeight);
            if (Double.IsNaN(_fontSize))
                textBlock.FontSize = CalculateFontSize();
            else
                textBlock.FontSize = FontSize;
        }


        /// <summary>
        /// Entry of a legend
        /// </summary>
        /// <param name="symbol"> entry symbol</param>
        /// <param name="legendText">text to display</param>
        /// <returns></returns>
        private Canvas CreateEntry(FrameworkElement symbol, String legendText)
        {
            Canvas entry = new Canvas();    // A entry in legend

            // Create label for legend entry
            TextBlock txt = new TextBlock();
            txt.Text = legendText;
            ApplyFontStyle(txt);

            // Set left property
            // for symbol -- already set
            txt.SetValue(LeftProperty, symbol.Width + Spacing);

            // Set top property
            
            symbol.SetValue(TopProperty, (symbol.Height < txt.ActualHeight) ? (txt.ActualHeight - symbol.Height) / 2 : 0);

            txt.SetValue(TopProperty, (txt.ActualHeight < symbol.Height) ? (symbol.Height - txt.ActualHeight) / 2 : 0);

            
            // Set size
            entry.Height = Math.Max(txt.ActualHeight, symbol.Height);
            entry.Width = symbol.Width + Spacing + txt.ActualWidth;

            // Add children
            entry.Children.Add(symbol);
            entry.Children.Add(txt);

            
            
            return entry;
        }

        #endregion Private Methods

        #region protected Methods

        /// <summary>
        /// Sets the default properties
        /// </summary>
        protected override void SetDefaults()
        {
            BorderThickness = Double.NaN;
            Spacing = Padding * 50 / 100 + 2;
        }

        #endregion

        #region Data

        private Double _spacing = Double.NaN;                       // Spacing among legend entries
        private Double _padding = Double.NaN;                                // Inner padding of legend      
        private Double _currentLeft = 0;                            // Left of a new legend entry
        private Double _currentTop = 0;                             // Top of a new legend entry
        private Double _currentHeight = 0;                          // Current height of row or column of legend entries.
        private Double _currentWidth = 0;                           // Current width of row or column of legend entries.

        private Double _maxHeight = Double.NaN;                     // Max height of the legend             
        private Double _maxWidth = Double.NaN;                      // Max width of the legend

        private AlignmentX _alignmentX;                             // Horizontal Alignment
        private AlignmentY _alignmentY;                             // Vertical Alignment

        // Label properties
        private String _fontFamily;                                 // FontFamily
        private Double _fontSize = Double.NaN;                      // FontSize
        private Brush _fontColor;                                   // FontColor
        private FontStyle _fontStyle;                              // FontStyle
        private FontWeight _fontWeight;                            // FontWeight

        // Title properties
        private String _titleFontFamily;                            // Title FontFamily
        private Double _titleFontSize = Double.NaN;                 // Title FontSize
        private Brush _titleFontColor;                              // Title FontColor
        private FontStyle _titleFontStyle;                         // Title FontStyle
        private FontWeight _titleFontWeight;                       // Title FontWeight

        private Canvas _entryCanvas;
        public TextBlock _title = null;                             // Title object 
        private Line _titleUnderLine;                               // Line object
        private Rectangle _titleBackground;                         // Background of the title
        private static Int32 _index = 0;                            // Index of the legend
        private Boolean _alignmentYChanged = false;
        private Boolean _alignmentXChanged = false;
        // Default stroke for the line bellow the title
        private String lineStrokeXaml;

        private String lineStrokeLeftXaml ;

        private String lineStrokeRightXaml ;


        #endregion
    }
}
