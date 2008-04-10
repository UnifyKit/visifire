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
            Canvas _parent = (Parent as Canvas);

            Double limitTop, limitBottom;
            Double toolTipTop = top;

            limitTop = (Double)_parent.GetValue(TopProperty);
            limitBottom = limitTop + _parent.Height - this.Height;

            if (limitTop <= top && top <= limitBottom) toolTipTop = top;
            else if (limitTop > top) toolTipTop = limitTop + 30;
            else if (limitBottom < top) toolTipTop = limitBottom;

            this.SetValue(TopProperty, toolTipTop);
            _textBlock.SetValue(TopProperty, Padding);
        }

        public void SetLeft(Double left)
        {
            Canvas _parent = (Parent as Canvas);
            Double limitLeft, limitRight;
            Double toolTipLeft = left;

            limitRight = (Double)_parent.GetValue(LeftProperty) + _parent.Width - this.Width;
            limitLeft = (Double)_parent.GetValue(LeftProperty);

            if (limitLeft <= left && left <= limitRight) toolTipLeft = left;
            else if (limitLeft > left) toolTipLeft = limitLeft;
            else if (limitRight < left) toolTipLeft = limitRight;

            this.SetValue(LeftProperty, toolTipLeft);
            _textBlock.SetValue(LeftProperty, Padding);
        }

        public override void SetHeight()
        {
            this.SetValue(HeightProperty, _textBlock.ActualHeight + Padding * 2);

            ApplyBorder();
        }

        public override void SetWidth()
        {
            this.SetValue(WidthProperty, _textBlock.ActualWidth + Padding * 2);
            ApplyBorder();
        }

        public void RotateLabelVerticle()
        {
            RotateTransform rt = new RotateTransform();

            rt.CenterX = 0;
            rt.CenterY = 0;
            rt.Angle = -90;
            _textBlock.RenderTransform = rt;
            this.Width = _textBlock.ActualHeight + Padding * 2;
            this.Height = _textBlock.ActualWidth + Padding * 2;
            _textBlock.SetValue(TopProperty, _textBlock.ActualWidth);
            _textBlock.SetValue(LeftProperty, Padding);
            ApplyBorder();

        }

        public void RotateLabelHorizontal()
        {
            _textBlock.RenderTransform = null;
            this.Width = _textBlock.ActualWidth + Padding * 2;
            this.Height = _textBlock.ActualHeight + Padding * 2;
            _textBlock.SetValue(TopProperty, Padding);
            _textBlock.SetValue(LeftProperty, Padding);
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
                _textBlock.Text = value;

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
                _textBlock.FontFamily = new FontFamily(value);
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
                _textBlock.Foreground = Cloner.CloneBrush(_fontColor);
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

        #region Private Methods

        protected override void SetDefaults()
        {
            base.SetDefaults();
            base.Background = null;
            _textBlock = new TextBlock();
            _textBlock.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255,118, 118, 118));
            _textBlock.FontFamily = new FontFamily("Verdana");

            _fontSize = Double.NaN;
            _fontColor = null;

            Enabled = true;
            TextWrap = 0.9;
        }

        #endregion Private Methods

        #region Data
        protected TextBlock _textBlock;

        private Double Padding = 1;
        private Double _textWrap;
        protected Double _fontSize;
        protected Brush _fontColor;
        #endregion Data
    }
}
