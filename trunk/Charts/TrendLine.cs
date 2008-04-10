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
    public class TrendLine : Canvas
    {
        #region Public Methods

        public TrendLine()
        {
            _line = new Line();
            _shadow = new Line();
            Children.Add(_line);
            Children.Add(_shadow);

            SetDefaults();
        }

        public String TextParser(String unParsed)
        {
            String str = new String(unParsed.ToCharArray());
            if (str.Contains("##XValue"))
                str = str.Replace("##XValue", "#XValue");
            else
            {
                if (!Double.IsNaN(XValue))
                    str = str.Replace("#XValue", _parent.AxisX.GetFormattedText(XValue));
            }
            if (str.Contains("##YValue"))
                str = str.Replace("##YValue", "#YValue");
            else
            {
                if (!Double.IsNaN(YValue))
                    str = str.Replace("#YValue", _parent.AxisY.GetFormattedText(YValue));
            }
            return str;
        }

        #endregion Public Methods

        #region Public Properties

        public bool ToolTipEnabled
        {
            get;
            set;
        }

        public String ToolTipText
        {
            get;
            set;
        }

        public Double XValue
        {
            get
            {
                return _xValue;
            }
            set
            {
                _xValue = value;
            }
        }

        public Double YValue
        {
            get
            {
                return _yValue;
            }
            set
            {
                _yValue = value;
            }
        }

        public bool Enabled
        {
            get;
            set;
        }

        public bool ShadowEnabled
        {
            get;
            set;
        }

        #region Line Properties

        internal Brush Color
        {
            get
            {
                if(_lineColor == null)
                    return new SolidColorBrush(Colors.Orange);
                return _lineColor;
            }

            set
            {
                _lineColor = value;
            }
        }
        public string LineColor
        {
            set
            {
                _lineColor = Parser.ParseColor(value);
            }
        }
        public Double LineThickness
        {
            get
            {
                return _lineThickness;
            }
            set
            {
                _lineThickness = value;
            }
        }

        public String LineStyle
        {
            get
            {
                return _lineStyle;
            }

            set
            {
                _lineStyle = value;
            }
        }
        #endregion Line Properties
        #endregion Public Properties

        #region Internal Methods
        internal void AttachToolTip()
        {
            String str;

            if (String.IsNullOrEmpty(ToolTipText)) return;


            str = TextParser(ToolTipText);

            this.MouseEnter += delegate(object sender, MouseEventArgs e)
            {
                _parent.ToolTip.Text = str;

                _parent.ToolTip.Visibility = Visibility.Visible;

                _parent.ToolTip.SetTop(e.GetPosition(_parent.ToolTip.Parent as UIElement).Y - (Double)_parent.ToolTip.GetValue(HeightProperty) * 1.5);
                _parent.ToolTip.SetLeft(e.GetPosition(_parent.ToolTip.Parent as UIElement).X - (Double)_parent.ToolTip.GetValue(WidthProperty) / 2);
            };

            this.MouseLeave += delegate(object sender, MouseEventArgs e)
            {
                _parent.ToolTip.Visibility = Visibility.Collapsed;

            };
        }
        

        internal void Init()
        {
            ValidateParent();

            

            SetName();
        }

        internal void SetDimensions()
        {
            _line.StrokeThickness = LineThickness;
            _line.Stroke = Cloner.CloneBrush(Color);
            _shadow.StrokeThickness = LineThickness;
            _shadow.Stroke = Parser.ParseSolidColor("#7f7f7f");

            switch (LineStyle)
            {
                case "Solid":
                    break;

                case "Dashed":
                    
                    _line.StrokeDashArray.Add(4);
                    _line.StrokeDashArray.Add(4);
                    _line.StrokeDashArray.Add(4);
                    _line.StrokeDashArray.Add(4);
                    break;

                case "Dotted":
                    
                    _line.StrokeDashArray.Add(1);
                    _line.StrokeDashArray.Add(2);
                    _line.StrokeDashArray.Add(1);
                    _line.StrokeDashArray.Add(2);
                    break;
            }
            if (!Double.IsNaN(_yValue))
            {
                if (_yValue >= _parent.AxisY.AxisMaximum || _yValue <= _parent.AxisY.AxisMinimum)
                {
                    this.SetValue(WidthProperty, 0);
                    this.SetValue(HeightProperty, 0);

                }
                else if (_parent.PlotDetails.AxisOrientation == AxisOrientation.Column)
                {
                    this.SetValue(WidthProperty, _parent.PlotArea.GetValue(WidthProperty));
                    this.SetValue(HeightProperty, LineThickness + 3);


                    this.SetValue(TopProperty, (Double)_parent.AxisY.DoubleToPixel(_yValue) + (Double)_parent.PlotArea.GetValue(TopProperty) - Height / 2);
                    
                    this.SetValue(LeftProperty, _parent.PlotArea.GetValue(LeftProperty));
                    _line.X1 = 0;
                    _line.Y1 = Height / 2;
                    _line.X2 = Width;
                    _line.Y2 = Height / 2;

                    _shadow.X1 = _line.X1;
                    _shadow.X2 = _line.X2;
                    _shadow.Y1 = _line.Y1 + LineThickness / 2;
                    _shadow.Y2 = _line.Y2 + LineThickness / 2;

                }
                else if (_parent.PlotDetails.AxisOrientation == AxisOrientation.Bar)
                {
                    this.SetValue(HeightProperty, _parent.PlotArea.GetValue(HeightProperty));
                    this.SetValue(WidthProperty, LineThickness + 3);

                    this.SetValue(LeftProperty, (Double)_parent.AxisY.DoubleToPixel(_yValue) + (Double)_parent.PlotArea.GetValue(LeftProperty) - Width / 2);
                    
                    this.SetValue(TopProperty, _parent.PlotArea.GetValue(TopProperty));
                    _line.Y1 = 0;
                    _line.X1 = Width / 2;
                    _line.Y2 = Height;
                    _line.X2 = Width / 2;

                    _shadow.X1 = _line.X1 + LineThickness / 2;
                    _shadow.X2 = _line.X2 + LineThickness / 2;
                    _shadow.Y1 = _line.Y1;
                    _shadow.Y2 = _line.Y2;

                }
                else
                {
                    this.SetValue(WidthProperty, 0);
                    this.SetValue(HeightProperty, 0);
                }
            }
            else if (!Double.IsNaN(_xValue))
            {
                if (_xValue >= _parent.AxisX.AxisMaximum || _xValue <= _parent.AxisX.AxisMinimum)
                {
                    this.SetValue(WidthProperty, 0);
                    this.SetValue(HeightProperty, 0);

                }
                else if (_parent.PlotDetails.AxisOrientation == AxisOrientation.Bar)
                {
                    this.SetValue(WidthProperty, _parent.PlotArea.GetValue(WidthProperty));
                    this.SetValue(HeightProperty, LineThickness + 3);
                    this.SetValue(LeftProperty, (Double)_parent.AxisY.DoubleToPixel(_parent.AxisY.AxisMinimum) + (Double)_parent.PlotArea.GetValue(LeftProperty));
                    
                    this.SetValue(TopProperty, (Double)_parent.AxisX.DoubleToPixel(_xValue) + (Double)_parent.PlotArea.GetValue(TopProperty) - Height / 2);
                    _line.X1 = 0;
                    _line.Y1 = Height / 2;
                    _line.X2 = Width;
                    _line.Y2 = Height / 2;

                    _shadow.X1 = _line.X1;
                    _shadow.X2 = _line.X2;
                    _shadow.Y1 = _line.Y1 + LineThickness / 2;
                    _shadow.Y2 = _line.Y2 + LineThickness / 2;

                }
                else if (_parent.PlotDetails.AxisOrientation == AxisOrientation.Column)
                {
                    this.SetValue(HeightProperty, _parent.PlotArea.GetValue(HeightProperty));
                    this.SetValue(WidthProperty, LineThickness + 3);

                    this.SetValue(TopProperty, (Double)_parent.PlotArea.GetValue(TopProperty));
                    
                    this.SetValue(LeftProperty, (Double)_parent.AxisX.DoubleToPixel(_xValue) + (Double)_parent.PlotArea.GetValue(LeftProperty) - Width / 2);
                    _line.Y1 = 0;
                    _line.X1 = Width / 2;
                    _line.Y2 = Height;
                    _line.X2 = Width / 2;

                    _shadow.X1 = _line.X1 + LineThickness / 2;
                    _shadow.X2 = _line.X2 + LineThickness / 2;
                    _shadow.Y1 = _line.Y1;
                    _shadow.Y2 = _line.Y2;

                }
                else
                {
                    this.SetValue(WidthProperty, 0);
                    this.SetValue(HeightProperty, 0);
                }
            }
            else
            {
                this.SetValue(WidthProperty, 0);
                this.SetValue(HeightProperty, 0);
            }


            if (!ShadowEnabled)
                _shadow.Opacity = 0;
        }

        internal void SetLeft()
        {
        }
        internal void SetTop()
        {
        }
        #endregion Internal Methods

        #region Private Methods

        /// <summary>
        /// Set a default Name. This is usefull if user has not specified this object in data XML and it has been 
        /// created by default.
        /// </summary>
        private void SetName()
        {
            if (this.Name.Length == 0)
            {
                int i = 0;

                String type = this.GetType().Name;
                String name = type;

                // Check for an available name
                while (FindName(name + i.ToString()) != null)
                {
                    i++;
                }

                name += i.ToString();

                this.SetValue(NameProperty, name);
            }
        }

        /// <summary>
        /// Validate Parent element and assign it to _parent.
        /// Parent element should be a Canvas element. Else throw an exception.
        /// </summary>
        private void ValidateParent()
        {
            if (this.Parent is Chart)
                _parent = this.Parent as Chart;
            else
                throw new Exception(this + "Parent should be an Chart");
        }

        private void SetDefaults()
        {
            Color = new SolidColorBrush(Colors.Red);
            LineThickness = 2;
            LineStyle = "Solid";

            _xValue = Double.NaN;
            _yValue = Double.NaN;
            Enabled = true;
            this.SetValue(ZIndexProperty, 3);
            _line.SetValue(ZIndexProperty, 10);
            _shadow.SetValue(ZIndexProperty, 1);
            this.Opacity = .5;
        }

        #endregion Private Methods

        #region Data

        Double _xValue;
        Double _yValue;

        Chart _parent;
        Line _line;
        Line _shadow;

        Double _lineThickness;
        String _lineStyle;
        Brush _lineColor;



        #endregion Data
    }
}
