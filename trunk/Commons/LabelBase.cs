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
using Visifire.Commons;


namespace Visifire.Commons
{
    public abstract class LabelBase : VisualObject
    {
        #region Public Methods

        public LabelBase()
        {
            this.Children.Add(_textBlock);
        }

        public override void Render()
        {
            base.Render();
        }

        public void SetTop(Double top)
        {
            Canvas parent = (Parent as Canvas);

            Double limitTop, limitBottom;
            Double toolTipTop = top;

            limitTop = 0;
            limitBottom = parent.Height - this.Height;

            if (limitTop <= top && top <= limitBottom) toolTipTop = top;
            else if (limitTop > top) toolTipTop = limitTop + 30;
            else if (limitBottom < top) toolTipTop = limitBottom;

            this.SetValue(TopProperty, (Double) toolTipTop);
            _textBlock.SetValue(TopProperty, (Double) _padding);

        }

        public void SetLeft(Double left)
        {
            Canvas parent = (Parent as Canvas);
            Double limitLeft, limitRight;
            Double toolTipLeft = left;

            limitRight = parent.Width - this.Width;
            limitLeft = 0;

            if (limitLeft <= left && left <= limitRight) toolTipLeft = left;
            else if (limitLeft > left) toolTipLeft = limitLeft;
            else if (limitRight < left) toolTipLeft = limitRight;

            this.SetValue(LeftProperty, (Double) toolTipLeft);
            _textBlock.SetValue(LeftProperty, (Double) _padding);
        }

        public override void SetHeight()
        {
            this.SetValue(HeightProperty, _textBlock.ActualHeight + _padding * 2);

            ApplyBorder();
        }

        public override void SetWidth()
        {
            this.SetValue(WidthProperty, _textBlock.ActualWidth + _padding * 2);
            ApplyBorder();
        }

        public void RotateLabelVerticle()
        {
            RotateTransform rt = new RotateTransform();

            rt.CenterX = 0;
            rt.CenterY = 0;
            rt.Angle = -90;
            _textBlock.RenderTransform = rt;
            this.Width = _textBlock.ActualHeight + _padding * 2;
            this.Height = _textBlock.ActualWidth + _padding * 2;
            _textBlock.SetValue(TopProperty, (Double) _textBlock.ActualWidth);
            _textBlock.SetValue(LeftProperty, (Double) _padding);
            ApplyBorder();

        }

        public void RotateLabelHorizontal()
        {
            _textBlock.RenderTransform = null;
            this.Width = _textBlock.ActualWidth + _padding * 2;
            this.Height = _textBlock.ActualHeight + _padding * 2;
            _textBlock.SetValue(TopProperty, (Double)  _padding);
            _textBlock.SetValue(LeftProperty, (Double) _padding);
            ApplyBorder();

        }

        #endregion Public Methods

        #region Public Properties

        public Double TextWrap
        {
            get
            {
                return _textWrap;
            }
            set
            {
                _textWrap = value;

            }
        }

        public String Text
        {
            get
            {
                return _textBlock.Text;
            }
            set
            {
                _textBlock.Text = Parser.GetFormattedText(value);

                SetWidth();
                SetHeight();

            }
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
                _fontString = value;
                _textBlock.FontFamily = Parser.GetFont(value,_textBlock);
            }
        }

        public Double FontSize
        {
            get
            {
                return _fontSize;
            }
            set
            {
                
                _fontSize = value;
                _textBlock.FontSize = value;
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
                _textBlock.Foreground = _fontColor;
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

        #endregion Font Properties

        #endregion Public Properties

        #region Protected Property
        protected String FontString
        {
            get
            {
                return _fontString;
            }
        }
        #endregion

        #region Private Methods

        protected override void SetDefaults()
        {
            base.SetDefaults();
            base.Background = null;

            _fontString = "Verdana";

            _textBlock = new TextBlock();
            _textBlock.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255,118, 118, 118));
            _textBlock.FontFamily = new FontFamily(FontString);
            
            _fontSize = Double.NaN;
            _fontColor = null;

            Enabled = true;
            TextWrap = 0.9;
        }

        #endregion Private Methods

        #region Data
        protected TextBlock _textBlock;

        private Double _padding = 1;
        private Double _textWrap;
        protected Double _fontSize;
        protected Brush _fontColor;
        private String _fontString;
        #endregion Data
    }
}
