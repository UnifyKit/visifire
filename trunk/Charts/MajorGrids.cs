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

        #region Protected Methods
        #endregion Protected Methods

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
            Int32 noOfIntervals;
            Double interval;
            Line ln = new Line();
            Rectangle interlacedRectangle = new Rectangle();


            interval = Interval;

            noOfIntervals = (int)Math.Ceiling((_parent.AxisMaximum - _parent.AxisMinimum) / interval);

            Double vals;
            Double vals2;
            this.Children.Clear();

            if (Enabled)
            {

                Double minValue;
                int countIntervals = 0;
                if (_parent.AxisOrientation == AxisOrientation.Bar)
                {

                    if ((Double)_parent.AxisManager.GetMinimumDataValue() - interval < _parent.AxisMinimum && _parent.GetType().Name == "AxisX")
                        vals = (Double)_parent.AxisManager.GetMinimumDataValue();
                    else
                        vals = _parent.AxisMinimum;

                    int i = 0;
                    minValue = vals;
                    while (vals <= _parent.AxisMaximum)
                    {
                        ln = new Line();

                        ln.SetValue(ZIndexProperty, 4);
                        ln.X1 = 0;
                        ln.X2 = (Double)(this._parent.Parent as Chart).PlotArea.GetValue(WidthProperty);


                        ln.Y1 = _parent.DoubleToPixel(vals);

                        vals2 = vals + interval;
                        ln.Y2 = ln.Y1;

                        vals = minValue + interval * (++countIntervals);
                        if (ln.Y1 < -0.5 || ln.Y1 > this.Height + 0.5) continue;


                        if (i % 2 == 0)
                        {
                            interlacedRectangle = new Rectangle();


                            (this._parent.Parent as Chart).PlotArea.Children.Add(interlacedRectangle);

                            interlacedRectangle.Fill = Cloner.CloneBrush(_parent.InterlacedBackground);

                            interlacedRectangle.SetValue(TopProperty, _parent.DoubleToPixel(vals2) + LineThickness / 2);
                            interlacedRectangle.SetValue(LeftProperty, ln.X1);

                            interlacedRectangle.SetValue(HeightProperty, ln.Y2 - (Double)interlacedRectangle.GetValue(TopProperty) - LineThickness / 2);


                            interlacedRectangle.SetValue(WidthProperty, ln.X2 - ln.X1);

                            interlacedRectangle.SetValue(ZIndexProperty, 0);


                        }
                        ln.Stroke = Visifire.Commons.Cloner.CloneBrush(LineBackground);
                        ln.StrokeThickness = LineThickness;


                        switch (this._lineStyle)
                        {
                            case "Solid":
                                break;

                            case "Dashed":
                                ln.StrokeDashArray = Converter.ArrayToCollection(new Double[] { 12, 12, 12, 12 });
                                break;

                            case "Dotted":
                                ln.StrokeDashArray = Converter.ArrayToCollection(new Double[] { 1, 2, 1, 2 });
                                break;
                        }

                        this.Children.Add(ln);



                        i++;
                    }

                }
                else if (_parent.AxisOrientation == AxisOrientation.Column)
                {
                    if ((Double)_parent.AxisManager.GetMinimumDataValue() - interval < _parent.AxisMinimum && _parent.GetType().Name == "AxisX")
                        vals = (Double)_parent.AxisManager.GetMinimumDataValue();
                    else
                        vals = _parent.AxisMinimum;

                    int i = 0;
                    minValue = vals;
                    while (vals <= _parent.AxisMaximum)
                    {
                        ln = new Line();

                        ln.SetValue(ZIndexProperty, 4);

                        ln.X1 = _parent.DoubleToPixel(vals);


                        ln.X2 = ln.X1;
                        ln.Y1 = 0;
                        ln.Y2 = -(Double)(this._parent.Parent as Chart).PlotArea.GetValue(HeightProperty);
                        vals2 = vals;
                        vals = minValue + interval * (++countIntervals);
                        if (ln.X1 < -0.5 || ln.X1 > this.Width + 0.5) continue;


                        if (i % 2 == 0)
                        {
                            interlacedRectangle = new Rectangle();

                            (this._parent.Parent as Chart).PlotArea.Children.Add(interlacedRectangle);
                            interlacedRectangle.Fill = Cloner.CloneBrush(_parent.InterlacedBackground);

                            interlacedRectangle.SetValue(LeftProperty, _parent.DoubleToPixel(vals2) + LineThickness / 2);
                            interlacedRectangle.SetValue(TopProperty, 0);


                            interlacedRectangle.Width = Math.Abs(_parent.DoubleToPixel(vals) - _parent.DoubleToPixel(vals2) - LineThickness / 2);


                            interlacedRectangle.SetValue(HeightProperty, Math.Abs(ln.Y1 - ln.Y2));
                            interlacedRectangle.SetValue(ZIndexProperty, 0);

                        }
                        ln.Stroke = Visifire.Commons.Cloner.CloneBrush(LineBackground);
                        ln.StrokeThickness = LineThickness;


                        switch (this._lineStyle)
                        {
                            case "Solid":
                                break;

                            case "Dashed":
                                ln.StrokeDashArray = Converter.ArrayToCollection(new Double[] { 12, 12, 12, 12 });
                                break;

                            case "Dotted":
                                ln.StrokeDashArray = Converter.ArrayToCollection(new Double[] { 1, 2, 1, 2 });
                                break;
                        }

                        this.Children.Add(ln);


                        i++;
                    }

                }
            }
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
                this.SetValue(LeftProperty, (Double)_parent.GetValue(WidthProperty));
            else if (_parent.AxisOrientation == AxisOrientation.Column)
            {
                this.SetValue(LeftProperty, 0);

            }
        }
        internal void SetTop()
        {


            if (_parent.AxisOrientation == AxisOrientation.Bar)
            {
                this.SetValue(TopProperty, 0);
            }
            else if (_parent.AxisOrientation == AxisOrientation.Column)
            {
                this.SetValue(TopProperty, 0);
            }
        }

        internal void Render()
        {

        }

        #endregion Internal Methods

        #region Private Methods

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
            if (this.Parent is Axes)
                _parent = this.Parent as Axes;
            else
                throw new Exception(this + "Parent should be an Axis");
        }

        
        #endregion Private Methods

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
