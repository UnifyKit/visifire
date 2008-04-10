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


using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;


namespace Visifire.Commons
{
    public abstract class TitleBase : VisualObject
    {
        #region Public Methods

        public TitleBase()
        {

        }

        public override void Init()
        {
            base.Init();
            _textBlock.SetValue(TopProperty, 0);
            _textBlock.SetValue(LeftProperty, 0);
            
            _textBlock.Margin = new Thickness(Padding);
            FixTitleSize();

            RotateTransform rt = new RotateTransform();

            rt.CenterX = 0;
            rt.CenterY = 0;

            // Title should be rotated by 90 Degree if it is placed to the right or left of the PlotArea
            if (!DockInsidePlotArea && AlignmentX == AlignmentX.Left && AlignmentY == AlignmentY.Center)
            {
                rt.Angle = -90;
                _textBlock.RenderTransform = rt;
                _textBlock.SetValue(TopProperty, _textBlock.ActualWidth);
                _textBlock.SetValue(LeftProperty, 0);
            }
            else if (!DockInsidePlotArea && AlignmentX == AlignmentX.Right && AlignmentY == AlignmentY.Center)
            {
                rt.Angle = +90;
                _textBlock.RenderTransform = rt;
                _textBlock.SetValue(LeftProperty, _textBlock.ActualHeight);
                _textBlock.SetValue(TopProperty, 0);
                
            }

            
        }
        /// <summary>
        /// This functions sets the warp size for a title and applies the hyperlink
        /// </summary>
        
        public override void Render()
        {
            base.Render();
            this.Children.Add(_textBlock);
            _textBlock.SetValue(ZIndexProperty, 70);

        }
        

        #endregion Public Methods

        #region Public Properties
        public Double TextWrap
        {
            get;
            set;
        }
        public Double Padding
        {
            get
            {
                if (Double.IsNaN(_padding))
                    return 2;
                else
                    return _padding;
            }
            set
            {
                _padding = value;
            }
        }

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

        public Boolean DockInsidePlotArea
        {
            get;
            set;
        }

        #region Font Properties

        public String FontFamily
        {
            get
            {
                return _textBlock.FontFamily.ToString();
            }

            set
            {
                _textBlock.FontFamily = new FontFamily(value);
            }
        }

        public Double FontSize
        {
            get
            {
                return _textBlock.FontSize;
            }
            set
            {
                _textBlock.FontSize = value;
                _fontSize = value;
            }
        }

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

        public String FontStyle
        {
            get
            {
                return _textBlock.FontStyle.ToString();
            }

            set
            {
                _textBlock.FontStyle = Converter.StringToFontStyle(value);
                
            }

        }

        public String FontWeight
        {
            get
            {
                return _textBlock.FontWeight.ToString();
            }
            set
            {
                _textBlock.FontWeight = Converter.StringToFontWeight(value);
            }
        }

        public Boolean FullSize
        {
            get;
            set;
        }
        #endregion

        public String Text
        {
            get
            {
                return _textBlock.Text;
            }
            set
            {
                _textBlock.Text = value;
                
            }
        }

        #endregion Public Properties

        #region Protected Methods

        protected void FixTitleSize()
        {

            if (TextWrap < 0 || TextWrap > 1)
            {
                System.Diagnostics.Debug.WriteLine("Text wrap value is invalid, must be between (0 - 1)");
                throw (new Exception("Invalid TextWrap value"));
            }
            if (!DockInsidePlotArea)
            {
                if ((AlignmentX == AlignmentX.Left || AlignmentX == AlignmentX.Right) && (AlignmentY == AlignmentY.Center))
                {
                    _textBlock.Width = WrapSize * TextWrap;
                    _textBlock.TextWrapping = TextWrapping.Wrap;

                }
                else
                {
                    _textBlock.Width = WrapSize * TextWrap;
                    _textBlock.TextWrapping = TextWrapping.Wrap;
                }
            }
            else
            {
                _textBlock.Width = WrapSize * TextWrap;
                _textBlock.TextWrapping = TextWrapping.Wrap;

            }


        }

        protected int CalculateFontSize()
        {
            int[] fontSizes = { 8, 10, 12, 14, 16, 18, 20, 24, 28, 32, 36, 40 };
            Double _parentSize = (Parent as FrameworkElement).Width * (Parent as FrameworkElement).Height;
            int i = (int)(Math.Ceiling(((_parentSize + 10000) / 115000)));
            i = (i >= fontSizes.Length ? fontSizes.Length - 1 : i);
            return fontSizes[i];
        }

        protected override void SetDefaults()
        {
            base.SetDefaults();
            _textBlock = new TextBlock();
            _alignmentX = AlignmentX.Center;
            _alignmentY = AlignmentY.Top;
            _padding = Double.NaN;

            _textBlock.FontFamily = new FontFamily("verdana");
            _textBlock.FontSize = 12;
            _textBlock.FontStyle = FontStyles.Normal;
            _textBlock.FontWeight = FontWeights.Normal;
            _textBlock.FontStretch = FontStretches.Normal;
            _fontSize = Double.NaN;
            _fontColor = null;
            DockInsidePlotArea = false;
            this.Opacity = 1.0;
            this.BorderThickness = 0;
            this.BorderColor = null;
            this.BorderStyle = "Solid";
            this.SetValue(ZIndexProperty, 2);
            Enabled = true;
            TextWrap = 0.9;
        }
        #endregion Protected Methods

        #region Protected Properties
        protected Double WrapSize
        {
            get;
            set;
        }
        #endregion Protected Properties

        #region Data
        protected TextBlock _textBlock;
        private AlignmentX _alignmentX;                         // Horizontal Alignment.
        private AlignmentY _alignmentY;                         // Vertical Alignment
        private Double _padding;
        protected Double _fontSize;
        protected Brush _fontColor;
        protected Boolean _alignmentXChanged = false;
        protected Boolean _alignmentYChanged = false;
        #endregion Data

    }
}
