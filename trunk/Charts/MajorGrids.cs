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
    public class MajorGrids : Canvas
    {
        #region Public Methods

        public MajorGrids()
        {
            SetDefaults();
        }

        #endregion Public Methods

        #region Public Properties

        #region Line Properties

        public String LineColor
        {
            set
            {
                LineBackground = Parser.ParseColor(value);
            }
        }

        public Double LineThickness
        {
            get
            {
                if (!Double.IsNaN(_lineThickness))
                    return _lineThickness;
                else if (GetFromTheme("LineThickness") != null)
                    return Convert.ToDouble(GetFromTheme("LineThickness"));
                else
                    return 0.25;
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

        public Double Interval
        {
            get
            {
                if (_interval <= 0)
                    return _parent.Interval;
                else
                    return _interval;
            }
            set
            {
                _interval = value;
            }
        }

        public Boolean Enabled
        {
            get
            {
                return _enabled;
            }
            set
            {
                _enabled = value;
            }
        }

        #endregion Public Properties

        #region Internal Properties
        internal Brush LineBackground
        {
            get
            {
                if (_lineBackground != null)
                    return _lineBackground;
                else if (GetFromTheme("LineColor") != null)
                    return GetFromTheme("LineColor") as Brush;
                else
                    return Parser.ParseColor("Gray");
            }
            set
            {
                _lineBackground = value;
            }
        }
        #endregion Internal Properties

        #region Internal Methods

        internal void DrawGrids()
        {
            if (!Enabled || !_parent.Enabled) return;

            Decimal interval;

            interval = (Decimal)Interval;

            this.Children.Clear();

            GenerateMajorGrids((_parent.GetType().Name == "AxisX"), interval);
        }

        internal void Init()
        {
            ValidateParent();

            SetName();
        }

        internal void SetWidth()
        {

            if (_parent.AxisOrientation == AxisOrientation.Bar)
            {
                this.SetValue(WidthProperty, 0);
            }
            else if (_parent.AxisOrientation == AxisOrientation.Column)
            {
                this.SetValue(WidthProperty, _parent.GetValue(WidthProperty));
            }

        }

        internal void SetHeight()
        {

            if (_parent.AxisOrientation == AxisOrientation.Bar)
            {
                this.SetValue(HeightProperty, _parent.GetValue(HeightProperty));
            }
            else if (_parent.AxisOrientation == AxisOrientation.Column)
            {
                this.SetValue(HeightProperty, 0);

            }
        }

        internal void SetLeft()
        {

            if (_parent.AxisOrientation == AxisOrientation.Bar)
            {
                this.SetValue(LeftProperty, (Double) _parent.GetValue(WidthProperty));
            }
            else if (_parent.AxisOrientation == AxisOrientation.Column)
            {
                this.SetValue(LeftProperty, (Double) 0);

            }
        }

        internal void SetTop()
        {

            if (_parent.AxisOrientation == AxisOrientation.Bar)
            {
                this.SetValue(TopProperty, (Double) 0);
            }
            else if (_parent.AxisOrientation == AxisOrientation.Column)
            {
                this.SetValue(TopProperty, (Double) 0);
            }
        }

        #endregion Internal Methods

        #region Private Methods

        private Object GetFromTheme(String propertyName)
        {
            Object obj = null;



            if (Root.AppliedTheme.ContainsKey(this.GetType().Name))
            {
                System.Collections.Generic.Dictionary<String, Object> properties = Root.AppliedTheme[this.GetType().Name];

                if (properties.ContainsKey(propertyName))
                    obj = (properties[propertyName]);
            }

            return obj;
        }

        private Rectangle CreateInterlacedRectangle(Rect rect, Int32 zindex, Brush color)
        {
            Rectangle interlacedRectangle = new Rectangle();

            interlacedRectangle.Fill = Cloner.CloneBrush(color);

            interlacedRectangle.SetValue(TopProperty, (Double)rect.Y);

            interlacedRectangle.SetValue(LeftProperty, (Double) rect.X);

            interlacedRectangle.Height = rect.Height;

            interlacedRectangle.Width = rect.Width;

            interlacedRectangle.SetValue(ZIndexProperty, 0);

            return interlacedRectangle;
        }

        private Line CreateLine(Point start, Point end, Int32 zindex)
        {
            Line line = new Line();

            line.X1 = start.X;
            line.Y1 = start.Y;

            line.X2 = end.X;
            line.Y2 = end.Y;

            line.Stroke = Visifire.Commons.Cloner.CloneBrush(LineBackground);
            line.StrokeThickness = LineThickness;
            line.StrokeDashArray = Parser.GetStrokeDashArray(this._lineStyle);

            return line;
        }

        private void GenerateMajorGrids(Boolean isAxisX, Decimal interval)
        {
            Decimal position;
            Decimal rectPosition;

            if ((Double)(_parent.AxisManager.GetMinimumDataValue() - interval) < _parent.AxisMinimum && isAxisX)
            {
                position = _parent.AxisManager.GetMinimumDataValue();
            }
            else
            {
                position = (Decimal)_parent.AxisMinimum;
            }

            Decimal minmumValue = position;

            Point start = new Point();
            Point end = new Point();
            Rect rect = new Rect();

            for (Int32 i = 0, intervalCount = 0; position <= (Decimal)_parent.AxisMaximum; i++)
            {
                if (_parent.AxisOrientation == AxisOrientation.Bar)
                {
                    if (_parent.AxisType == AxisType.Primary)
                    {
                        start.X = 0;
                        end.X = (this._parent.Parent as Chart).PlotArea.Width;
                    }
                    else
                    {
                        start.X = -_parent.Width - (this._parent.Parent as Chart).PlotArea.Width;
                        end.X = -_parent.Width;
                    }
                    
                    start.Y = _parent.DoubleToPixel((Double)position);
                    end.Y = start.Y;

                    rectPosition = position + interval;
                    position = minmumValue + interval * (++intervalCount);

                    if (start.Y < -0.5 || start.Y > this.Height + 0.5) continue;

                    this.Children.Add(CreateLine(start, end, 4));

                    if (i % 2 == 0)
                    {
                        rect.X = start.X;
                        rect.Y = _parent.DoubleToPixel((Double)rectPosition) + LineThickness / 2;

                        rect.Height = end.Y - rect.Y - LineThickness / 2;
                        rect.Width = end.X - start.X;

                        (this._parent.Parent as Chart).PlotArea.Children.Add(CreateInterlacedRectangle(rect, 0, _parent.InterlacedBackground));
                    }
                }
                else if (_parent.AxisOrientation == AxisOrientation.Column)
                {
                    if (_parent.AxisType == AxisType.Primary)
                    {
                        start.Y = 0;
                        end.Y = -_parent._parent.PlotArea.Height;
                    }
                    else
                    {
                        start.Y = _parent.Height;
                        end.Y = start.Y + _parent._parent.PlotArea.Height;
                    }
                    start.X = _parent.DoubleToPixel((Double)position);
                    

                    end.X = start.X;
                    

                    rectPosition = position;
                    position = minmumValue + interval * (++intervalCount);

                    if (start.X < -0.5 || start.X > this.Width + 0.5) continue;

                    this.Children.Add(CreateLine(start, end, 4));

                    if (i % 2 == 0)
                    {
                        rect.X = _parent.DoubleToPixel((Double)rectPosition) + LineThickness / 2;
                        rect.Y = 0;

                        rect.Height = Math.Abs(start.Y - end.Y);
                        rect.Width = Math.Abs(_parent.DoubleToPixel((Double)position) - _parent.DoubleToPixel((Double)rectPosition) - LineThickness / 2);

                        (this._parent.Parent as Chart).PlotArea.Children.Add(CreateInterlacedRectangle(rect, 0, _parent.InterlacedBackground));
                    }
                }
                else
                {
                    // not a valid axis orientation
                    break;
                }
            }
        }

        private void SetDefaults()
        {
            _lineBackground = null;
            _lineThickness = Double.NaN;
            LineStyle = "Solid";
            _enabled = true;

            Interval = -1;

            this.SetValue(ZIndexProperty, 2);
        }

        /// <summary>
        /// Set a default Name. This is usefull if user has not specified this object in data XML and it has been 
        /// created by default.
        /// </summary>
        private void SetName()
        {
            Generic.SetNameAndTag(this);
        }


        /// <summary>
        /// Validate Parent element and assign it to _parent.
        /// Parent element should be a Canvas element. Else throw an exception.
        /// </summary>
        private void ValidateParent()
        {
            if (this.Parent is Axes)
                _parent = this.Parent as Axes;
            else
                throw new Exception(this + "Parent should be an Axis");
        }

        
        #endregion Private Methods

        #region Private Properties
        private Container Root
        {
            get
            {
                FrameworkElement parent;

                if (_root == null)
                {
                    parent = this;

                    while (!(parent is Container))
                        parent = (FrameworkElement)parent.Parent;

                    _root = (Container)parent;
                }
                return _root;
            }
        }
        #endregion Private Properties

        #region Data

        private Boolean _enabled;

        private Axes _parent;

        
        private Double _interval;
        
        private Double _lineThickness;
        private Brush _lineBackground;
        private String _lineStyle;
        private Container _root = null;
        #endregion Data
    }
}
