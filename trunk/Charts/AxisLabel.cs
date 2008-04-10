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

namespace Visifire.Charts
{
    internal class AxisLabel : VisualObject
    {
        #region Public Methods

        public AxisLabel()
        {
            this.Loaded += new RoutedEventHandler(OnLoaded);
        }

        void OnLoaded(object sender, EventArgs e)
        {
        }

        public override void Init()
        {

            base.Init();
            _textBlock.FontFamily = new FontFamily((Parent as AxisLabels).FontFamily);
            _textBlock.FontSize = (Parent as AxisLabels).FontSize;
            _textBlock.Width = (Parent as AxisLabels).MaxLabelWidth;

            if ((Parent as AxisLabels)._fontColor == null)
            {

                Double intensity;
                if (((Parent as AxisLabels).Parent as Axes).Background == null)
                {
                    if ((((Parent as AxisLabels).Parent as Axes).Parent as Chart).Background == null)
                    {
                        _textBlock.Foreground = new SolidColorBrush(Colors.Black);
                    }
                    else
                    {
                        intensity = Parser.GetBrushIntensity((((Parent as AxisLabels).Parent as Axes).Parent as Chart).Background);

                        if (intensity <= 0.5)
                        {

                            _textBlock.Foreground = Parser.ParseSolidColor("#BBBBBB");
                        }
                        else
                        {
                            _textBlock.Foreground = new SolidColorBrush(Colors.Black);
                        }

                    }
                }
                else
                {

                    intensity = Parser.GetBrushIntensity(((Parent as AxisLabels).Parent as Axes).Background);
                    if (intensity <= 0.5)
                    {

                        _textBlock.Foreground = Parser.ParseSolidColor("#BBBBBB");
                    }
                    else
                    {
                        _textBlock.Foreground = new SolidColorBrush(Colors.Black);
                    }


                }
            }
            else
            {
                _textBlock.Foreground = Cloner.CloneBrush((Parent as AxisLabels).FontColor);
            }
            this.SetWidth();
            this.SetHeight();
        }

        public override void SetWidth()
        {

            this.SetValue(WidthProperty, _textBlock.ActualWidth);

        }

        public override void SetHeight()
        {

            this.SetValue(HeightProperty, _textBlock.ActualHeight);
        }

        public override void SetLeft()
        {
            this.SetValue(LeftProperty, 0);
        }

        public override void SetTop()
        {
            this.SetValue(TopProperty, 0);
        }

        #endregion Public Methods

        #region Internal Properties

        internal Double ActualWidth
        {
            get
            {
                
                Double width1 = Math.Abs(DiagonalLength * Math.Cos(DiagonalAngle * Math.PI / 180));
                Double width2 = Math.Abs(DiagonalLength * Math.Cos((360 - DiagonalAngle + Angle * 2) * Math.PI / 180));
                
                if (width1 > width2)
                    return width1;
                else
                    return width2;
                
            }
        }

        internal Double ActualTop
        {
            get
            {
                Double top = 0;
                top = ((Parent as AxisLabels).Parent as Axes).DoubleToPixel(this.Position) - ((Double)this.GetValue(WidthProperty) * Math.Sin(Angle * Math.PI / 180)) - ((Double)this.GetValue(HeightProperty) / 2 * Math.Cos(Angle * Math.PI / 180));

                if (Angle >= -90 && Angle < 0)
                {
                    if (Angle == -90)
                        top = ((Parent as AxisLabels).Parent as Axes).DoubleToPixel(this.Position) - this.ActualHeight / 2;
                    else
                        top -= (this.ActualHeight - ((Double)this.GetValue(HeightProperty) * Math.Cos(Angle * Math.PI / 180)));
                }

                return top;
            }
        }

        internal Double ActualLeft
        {
            get
            {
                Double left = 0;

                left = ((Parent as AxisLabels).Parent as Axes).DoubleToPixel(this.Position) - ((Double)this.GetValue(HeightProperty) / 2 * Math.Sin(Angle * Math.PI / 180));

                if (Angle >= -90 && Angle < 0)
                    left -= this.ActualWidth;
                else if (Angle == 0)
                    left -= this.ActualWidth / 2;

                return left;
            }
        }

        // Represents whether its a AxisLabel or not.
        internal Boolean IsAxisLabel
        {
            get;
            set;
        }

        internal Double ActualHeight
        {
            get
            {
                Double height1 = Math.Abs(DiagonalLength * Math.Sin((DiagonalAngle) * Math.PI / 180));
                Double height2 = Math.Abs(DiagonalLength * Math.Sin((360 - DiagonalAngle + Angle * 2) * Math.PI / 180));

                if (height1 > height2)
                    return height1;
                else
                    return height2;
            }
        }

        internal Double DiagonalAngle
        {
            get
            {
                return 180 - (Math.Atan((Double)this.GetValue(HeightProperty) / (Double)this.GetValue(WidthProperty)) * 180 / Math.PI) + Angle;
            }
        }

        internal Double DiagonalLength
        {
            get
            {
                return Math.Sqrt(Math.Pow((Double)this.GetValue(WidthProperty), 2) + Math.Pow((Double)this.GetValue(HeightProperty), 2));
            }
        }

        internal String FontStyle
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

        internal String FontWeight
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

        internal String Text
        {
            get
            {
                return _textBlock.Text;
            }
            set
            {
                _textBlock.Text = value;

                this.SetWidth();
                this.SetHeight();

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
                value %= 360;
                value += 360;
                value %= 360;

                if (value > 90 && value < 270)
                {
                    _angle = -180 + value;
                }
                else if (value >= 270 && value <= 360)
                {
                    _angle = value - 360;
                }
                else
                    _angle = value;

                _angle %= 360;

                _rt = new RotateTransform();

                _rt.Angle = _angle;

                this.RenderTransform = _rt;


            }
        }

        internal Double Left
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

        internal Double Top
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

        internal Double Position
        {
            get
            {
                return _position;
            }
            set
            {
                _position = value;
            }
        }

        internal Double FontSize
        {
            get
            {
                return _textBlock.FontSize;
            }
            set
            {
                _textBlock.FontSize = value;
                SetHeight();
                SetWidth();
            }
        }

                
        #endregion Internal Properties

        #region Private Methods

        protected override void SetDefaults()
        {
            base.SetDefaults();

            _textBlock = new TextBlock();
            


            this.Children.Add(_textBlock);

            toolTip = new TextBlock();
            this.Children.Add(toolTip);

            Angle = 0;
            
            _textBlock.TextWrapping = TextWrapping.Wrap;
            
        }

        #endregion Private Methods

        #region Data

        private Double _position;
        private Double _angle;
        private Double _left;
        private Double _top;
        
        private RotateTransform _rt;

        internal TextBlock _textBlock;

        private TextBlock toolTip;

        #endregion Data
    }
}
