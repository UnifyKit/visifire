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
using System.Windows.Documents;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Visifire.Commons;

namespace Visifire.Charts
{
    internal class AxisLabel : Canvas
    {
        #region Public Methods

        public AxisLabel()
        {
            this.Children.Add(_textBlock);

            _transformGroup.Children.Add(_rotateTransform);

            _textBlock.RenderTransform = _transformGroup;

            this.RenderTransformOrigin = new Point(0.5, 0.5);
        }

        private void SetFontProperties(AxisLabels parent)
        {
            _textBlock.FontFamily = Parser.GetFont(parent.FontFamily, _textBlock);

            _textBlock.FontSize = parent.FontSize;

            _textBlock.FontStyle = Converter.StringToFontStyle(parent.FontStyle);

            _textBlock.FontWeight = Converter.StringToFontWeight(parent.FontWeight);

            if (parent._fontColor == null)
                _textBlock.Foreground = GetDefaultFontcolor();
            else
                _textBlock.Foreground = (parent.FontColor);

        }

        public void Init()
        {

            SetTags();

            SetFontProperties(Parent as AxisLabels);

            TextBlock.Width = (Parent as AxisLabels).MaxLabelWidth;
            TextBlock.TextWrapping = TextWrapping.Wrap;


        }

        #endregion Public Methods

        #region Internal Properties

        internal Double ActualLeft
        {
            get
            {
                return _left;
            }
            set
            {
                _left = value;
            }
        }

        internal Double ActualTop
        {
            get
            {
                return _top;
            }
            set
            {
                _top = value;
            }
        }

        internal Double Angle
        {
            get
            {
                return _angle;
            }
            set
            {
                _angle = GetAngle(value);
            }
        }

        internal String Text
        {
            get
            {
                return _textBlock.Text;
            }
            set
            {
                _textBlock.Text = value;
                _textBlock.Width = _textBlock.ActualWidth;
                _textBlock.Height = _textBlock.ActualHeight;
            }
        }

        internal new Double ActualHeight
        {
            get
            {
                return _height;
            }
        }

        internal new Double ActualWidth
        {
            get
            {
                return _width;
            }
        }

        internal TextBlock TextBlock
        {
            get
            {
                return _textBlock;
            }
        }

        internal Double Left
        {
            get
            {
                return (Double)this.GetValue(LeftProperty);
            }
            set
            {
                SetValue(LeftProperty, value);
            }
        }

        internal Double Top
        {
            get
            {
                return (Double)this.GetValue(TopProperty);
            }
            set
            {
                SetValue(TopProperty, value);
            }
        }

        internal Double Position
        {
            get;
            set;
        }

        #endregion Internal Properties

        #region Internal Methods
        internal void UpdatePositionRight(Point newPos)
        {
            Double left;
            Double top;
            Double relativeAngle = Math.Atan(_textBlock.ActualHeight / (2 * _textBlock.ActualWidth));
            Double length = Math.Sqrt(Math.Pow(_textBlock.ActualHeight / 2, 2) + Math.Pow(_textBlock.ActualWidth, 2));

            _rotateTransform.Angle = Angle;

            _rotateTransform.CenterX = 0;
            _rotateTransform.CenterY = 0.5;

            top = (_textBlock.Height / 2) * Math.Cos(GetRadians(Angle));

            left = -(_textBlock.Height / 2) * Math.Sin(GetRadians(Angle));

            this.SetValue(LeftProperty, (Double) ( newPos.X - left));
            this.SetValue(TopProperty, (Double) ( newPos.Y - top));

            CalculateSize(GetRadians(Angle));

            if (Angle > 90)
            {
                _top = (Double)this.GetValue(TopProperty) - top + length * Math.Sin(GetRadians(Angle) + relativeAngle);
                _left = (Double)this.GetValue(LeftProperty);
            }
            else
            {
                _top = (Double)this.GetValue(TopProperty);
                _left = (Double)this.GetValue(LeftProperty) + left - (_textBlock.Height / 2) * Math.Cos(GetRadians(90 - Angle));
            }
        }

        internal void UpdatePositionLeft(Point newPos)
        {

            _rotateTransform.Angle = Angle;

            _rotateTransform.CenterX = 0;
            _rotateTransform.CenterY = 0.5;

            Double left;
            Double top;
            Double radians = GetRadians(Angle);
            Double relativeAngle = Math.Atan(_textBlock.ActualHeight / (2 * _textBlock.ActualWidth));
            Double length = Math.Sqrt(Math.Pow(_textBlock.ActualHeight / 2, 2) + Math.Pow(_textBlock.ActualWidth, 2));

            left = length * Math.Cos(radians + relativeAngle);
            top = length * Math.Sin(radians + relativeAngle);

            this.SetValue(LeftProperty, (Double) ( newPos.X - left));
            this.SetValue(TopProperty, (Double) ( newPos.Y - top));

            CalculateSize(GetRadians(Angle));

            if (Angle > 90)
            {
                _top = (Double)this.GetValue(TopProperty) + top - ((_textBlock.Height / 2) * Math.Sin(GetRadians(90 - Angle)));
                _left = (Double)this.GetValue(LeftProperty);
            }
            else
            {
                _top = (Double)this.GetValue(TopProperty);
                _left = (Double)this.GetValue(LeftProperty) + left - ((_textBlock.Height / 2) * Math.Cos(GetRadians(90 - Angle))); ;
            }
        }

        internal void UpdatePositionTop(Point newPos)
        {
            Double left;
            Double top;

            if (Angle == 0)
            {
                left = _textBlock.ActualWidth / 2;
                top = _textBlock.ActualHeight;

                _rotateTransform.Angle = 0;

                this.SetValue(LeftProperty, (Double) ( newPos.X - left));
                this.SetValue(TopProperty, (Double) ( newPos.Y - top));

                _top = (Double)this.GetValue(TopProperty);
                _left = (Double)this.GetValue(LeftProperty);

                CalculateSize(GetRadians(Angle));
            }
            else if (Angle > 90)
            {
                UpdatePositionRight(newPos);

            }
            else
            {
                UpdatePositionLeft(newPos);
            }

        }


        internal void UpdatePositionBottom(Point newPos)
        {

            Double left;
            Double top;

            if (Angle == 0)
            {
                left = _textBlock.ActualWidth / 2;
                top = 0;

                _rotateTransform.Angle = 0;

                this.SetValue(LeftProperty, (Double) ( newPos.X - left));
                this.SetValue(TopProperty, (Double) ( newPos.Y + top));

                _top = (Double)this.GetValue(TopProperty);
                _left = (Double)this.GetValue(LeftProperty);

                CalculateSize(GetRadians(Angle));
            }
            else if (Angle > 90)
            {
                UpdatePositionLeft(newPos);
            }
            else
            {
                UpdatePositionRight(newPos);
            }

            
        }

        internal void UpdateSize()
        {
            CalculateSize(GetRadians(Angle));
        }

        private void CalculateSize(Double radianAngle)
        {
            Double length = Math.Sqrt(Math.Pow(_textBlock.ActualHeight, 2) + Math.Pow(_textBlock.ActualWidth, 2));
            Double beta = Math.Atan(_textBlock.ActualHeight / _textBlock.ActualWidth);
            Double height1 = Math.Abs(length * Math.Sin(radianAngle + beta));
            Double height2 = Math.Abs(length * Math.Sin(radianAngle - beta));
            Double width1 = Math.Abs(length * Math.Cos(radianAngle + beta));
            Double width2 = Math.Abs(length * Math.Cos(radianAngle - beta));

            _height = Math.Max(height1, height2);

            _width = Math.Max(width1, width2);

        }

        private Double GetRadians(Double angle)
        {
            return angle * Math.PI / 180;
        }

        private Double GetAngle(Double angle)
        {
            Double newAngle = angle;

            while (newAngle < 0) { newAngle += 360; }

            return newAngle;
        }

        #endregion Internal Methods

        #region Protected Methods

        private void SetDefaults()
        {

            _textBlock = new TextBlock();

            Angle = 0;
            
            _textBlock.TextWrapping = TextWrapping.Wrap;

        }

        #endregion Protected Methods

        #region Private Methods

        private Brush GetDefaultFontcolor()
        {
            Brush fontColorBrush = null;
            Double intensity;

            if (((Parent as AxisLabels).Parent as Axes).Background == null)
            {
                if ((((Parent as AxisLabels).Parent as Axes).Parent as Chart).Background == null)
                {
                    fontColorBrush = new SolidColorBrush(Colors.Black);
                }
                else
                {
                    intensity = Parser.GetBrushIntensity((((Parent as AxisLabels).Parent as Axes).Parent as Chart).Background);
                    fontColorBrush = Parser.GetDefaultFontColor(intensity);
                }
            }
            else
            {
                intensity = Parser.GetBrushIntensity(((Parent as AxisLabels).Parent as Axes).Background);
                fontColorBrush = Parser.GetDefaultFontColor(intensity);
            }

            return fontColorBrush;
        }

        private void SetTags()
        {
            // The tag for axislabel is the name of the axis
            _textBlock.Tag = ((Parent as Canvas).Parent as Canvas).Name;
        }

        #endregion Private Methods

        #region Data

        private Double _left;
        private Double _top;
        private Double _angle;
        private Double _height;
        private Double _width;
        private TextBlock _textBlock = new TextBlock();
        private TransformGroup _transformGroup = new TransformGroup();
        private RotateTransform _rotateTransform = new RotateTransform();

        #endregion Data
    }
}
