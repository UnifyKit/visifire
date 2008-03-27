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
using Visifire.Commons;

namespace Visifire.Charts
{
    public class MajorTicks : Canvas
    {
        #region Public Methods

        public MajorTicks()
        {
            SetDefaults();
        }

        #endregion Public Methods

        #region Public Properties

        #region Line Properties

        internal Brush LineBackground
        {
            get
            {
                return _lineBackground;
            }
            set
            {
                _lineBackground = value;
            }
        }
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

        public bool Enabled
        {
            get;
            set;
        }

        public Double Interval
        {
            get
            {
                if (Double.IsNaN(_interval))
                    return _parent.Interval;
                else
                    return _interval;
            }
            set
            {
                _interval = value;
            }
        }

        #endregion Public Properties

        #region Internal Properties

        internal Double TickLength
        {
            get
            {
                if (Double.IsNaN(_tickLength) || _tickLength == 0)
                    _tickLength = 10;
                return _tickLength;
            }
            set
            {
                _tickLength = value;
            }
        }

        #endregion Internal properties

        #region Internal Methods
        internal void Init()
        {
            ValidateParent();

            SetName();
        }

        internal void DrawTicks()
        {

            if ((_parent.Parent as Chart).View3D && _parent.GetType().Name == "AxisX" && _parent.AxisOrientation == AxisOrientation.Column)
            {
                this.Children.Clear();
                Rectangle rectBase = new Rectangle();
                Rectangle rectFront = new Rectangle();
                Rectangle rectSide = new Rectangle();

                Chart chart = _parent.Parent as Chart;

                rectBase.Width = chart.AxisX.DoubleToPixel(chart.AxisX.AxisMaximum) - (Double)rectBase.GetValue(LeftProperty);
                rectBase.Height = TickLength;


                rectFront.SetValue(TopProperty, TickLength);
                rectFront.SetValue(LeftProperty, -TickLength);


                rectFront.Width = rectBase.Width;
                rectFront.Height = _parent.PlankThickness;

                rectSide.SetValue(LeftProperty, rectFront.Width - TickLength);
                rectSide.SetValue(TopProperty, rectBase.Height);

                rectSide.Width = TickLength;
                rectSide.Height = rectFront.Height;

                #region Color Gradient
                Brush brush2 = null;
                Brush brushShade = null;
                Brush brushFront = null;
                Brush tempBrush;
                if ((_parent.Parent as Chart).PlotArea.Background != null)
                    tempBrush = (_parent.Parent as Chart).PlotArea.Background;
                else if ((_parent.Parent as Chart).Background != null)
                    tempBrush = (_parent.Parent as Chart).Background;
                else
                    tempBrush = Parser.ParseLinearGradient("0;#afafaf,0;#efefef,1");



                if (tempBrush.GetType().Name == "LinearGradientBrush")
                {
                    LinearGradientBrush brush = tempBrush as LinearGradientBrush;
                    brush2 = Cloner.CloneBrush(tempBrush);
                    String linBrush1 = "-90;", linBrush2 = "-90;";

                    foreach (GradientStop grad in brush.GradientStops)
                    {

                        linBrush1 += Parser.GetDarkerColor(grad.Color, 0.9) + ",";
                        linBrush1 += grad.Offset.ToString() + ";";

                        linBrush2 += Parser.GetDarkerColor(grad.Color, 0.85) + ",";
                        linBrush2 += grad.Offset.ToString() + ";";
                    }
                    brushFront = Parser.ParseLinearGradient(linBrush1);
                    brushShade = Parser.ParseLinearGradient(linBrush2);

                }
                else if (tempBrush.GetType().Name == "RadialGradientBrush")
                {
                    RadialGradientBrush brush = tempBrush as RadialGradientBrush;
                    brush2 = Cloner.CloneBrush(tempBrush);
                    String linBrush1 = "0.5;0.5;", linBrush2 = "0.5;0.5;";

                    foreach (GradientStop grad in brush.GradientStops)
                    {

                        linBrush1 += Parser.GetDarkerColor(grad.Color, 0.75) + ",";
                        linBrush1 += grad.Offset.ToString() + ";";

                        linBrush2 += Parser.GetDarkerColor(grad.Color, 0.85) + ",";
                        linBrush2 += grad.Offset.ToString() + ";";
                    }
                    brushFront = Parser.ParseRadialGradient(linBrush1);
                    brushShade = Parser.ParseRadialGradient(linBrush2);

                }
                else if (tempBrush.GetType().Name == "SolidColorBrush")
                {
                    SolidColorBrush brush = tempBrush as SolidColorBrush;

                    //In case of video or image brush will have null
                    String linbrush;
                    linbrush = "-90;";
                    linbrush += Parser.GetDarkerColor(brush.Color, 0.85);
                    linbrush += ",0;";
                    linbrush += Parser.GetLighterColor(brush.Color, 0.35);
                    linbrush += ",1";

                    brush2 = Parser.ParseLinearGradient(linbrush);

                    linbrush = "-90;";
                    linbrush += Parser.GetDarkerColor(brush.Color, 0.65);
                    linbrush += ",0;";
                    linbrush += Parser.GetLighterColor(brush.Color, 0.55);
                    linbrush += ",1";


                    brushFront = Parser.ParseLinearGradient(linbrush);

                    linbrush = "-120;";
                    linbrush += Parser.GetDarkerColor(brush.Color, 0.35);
                    linbrush += ",0;";
                    linbrush += Parser.GetDarkerColor(brush.Color, 0.75);
                    linbrush += ",1";

                    brushShade = Parser.ParseLinearGradient(linbrush);

                }
                #endregion Color Gradient

                rectBase.Fill = Cloner.CloneBrush(brush2);
                rectSide.Fill = Cloner.CloneBrush(brushShade);
                rectFront.Fill = Cloner.CloneBrush(brushFront);

                rectFront.StrokeThickness = 1;
                rectBase.StrokeThickness = 1;
                rectSide.StrokeThickness = 1;

                rectFront.Opacity = 1;
                rectBase.Opacity = 1;
                rectSide.Opacity = 1;

                SkewTransform st = new SkewTransform();
                st.AngleX = -45;

                rectBase.RenderTransform = st;


                st = new SkewTransform();
                st.AngleY = -45;

                rectSide.RenderTransform = st;

                this.Height = rectBase.Height + rectFront.Height;
                this.Children.Add(rectBase);
                this.Children.Add(rectFront);
                this.Children.Add(rectSide);


            }
            else if ((_parent.Parent as Chart).View3D && _parent.GetType().Name == "AxisX" && _parent.AxisOrientation == AxisOrientation.Bar)
            {
                this.Children.Clear();
                Rectangle rectBase = new Rectangle();
                Rectangle rectFront = new Rectangle();
                Rectangle rectSide = new Rectangle();

                Chart chart = _parent.Parent as Chart;

                rectBase.Width = _parent.PlankThickness;
                rectBase.Height = TickLength;
                rectBase.SetValue(TopProperty, 0);
                rectBase.SetValue(LeftProperty, TickLength);

                rectFront.SetValue(TopProperty, TickLength);
                rectFront.SetValue(LeftProperty, 0);
                rectFront.Height = chart.AxisX.DoubleToPixel(chart.AxisX.AxisMinimum) - chart.AxisX.DoubleToPixel(chart.AxisX.AxisMaximum);
                rectFront.Width = _parent.PlankThickness;

                rectSide.Height = rectFront.Height;
                rectSide.Width = TickLength;
                rectSide.SetValue(TopProperty, TickLength);
                rectSide.SetValue(LeftProperty, _parent.PlankThickness);




                #region Color Gradient
                Brush brush2 = null;
                Brush brushShade = null;
                Brush brushFront = null;
                Brush tempBrush;
                if ((_parent.Parent as Chart).PlotArea.Background != null)
                    tempBrush = (_parent.Parent as Chart).PlotArea.Background;
                else if ((_parent.Parent as Chart).Background != null)
                    tempBrush = (_parent.Parent as Chart).Background;
                else
                    tempBrush = Parser.ParseLinearGradient("0;#afafaf,0;#efefef,1");

                if (tempBrush.GetType().Name == "LinearGradientBrush")
                {
                    LinearGradientBrush brush = tempBrush as LinearGradientBrush;
                    brush2 = Cloner.CloneBrush(tempBrush);
                    String linBrush1 = "0;", linBrush2 = "45;";

                    foreach (GradientStop grad in brush.GradientStops)
                    {

                        linBrush1 += Parser.GetDarkerColor(grad.Color, 0.75) + ",";
                        linBrush1 += grad.Offset.ToString() + ";";

                        linBrush2 += Parser.GetDarkerColor(grad.Color, 0.85) + ",";
                        linBrush2 += grad.Offset.ToString() + ";";
                    }
                    brushFront = Parser.ParseLinearGradient(linBrush1);
                    brushShade = Parser.ParseLinearGradient(linBrush2);

                }
                else if (tempBrush.GetType().Name == "RadialGradientBrush")
                {
                    RadialGradientBrush brush = tempBrush as RadialGradientBrush;
                    brush2 = Cloner.CloneBrush(tempBrush);
                    String linBrush1 = "0.5;0.5;", linBrush2 = "0.5;0.5;";

                    foreach (GradientStop grad in brush.GradientStops)
                    {

                        linBrush1 += Parser.GetDarkerColor(grad.Color, 0.65) + ",";
                        linBrush1 += grad.Offset.ToString() + ";";

                        linBrush2 += Parser.GetDarkerColor(grad.Color, 0.75) + ",";
                        linBrush2 += grad.Offset.ToString() + ";";
                    }
                    brushFront = Parser.ParseRadialGradient(linBrush1);
                    brushShade = Parser.ParseRadialGradient(linBrush2);

                }
                else if (tempBrush.GetType().Name == "SolidColorBrush")
                {
                    SolidColorBrush brush = tempBrush as SolidColorBrush;

                    //In case of video or image brush will have null
                    String linbrush;
                    linbrush = "135;";
                    linbrush += Parser.GetDarkerColor(brush.Color, 0.75);
                    linbrush += ",0;";
                    if(Parser.GetBrushIntensity(brush)>0.5)
                        linbrush += Parser.GetDarkerColor(brush.Color, 0.85);
                    else
                        linbrush += Parser.GetLighterColor(brush.Color, 0.35);
                    linbrush += ",1";

                    brush2 = Parser.ParseLinearGradient(linbrush);

                    linbrush = "0;";
                    linbrush += Parser.GetDarkerColor(brush.Color, 0.65);
                    linbrush += ",0;";
                    if (Parser.GetBrushIntensity(brush) > 0.5)
                        linbrush += Parser.GetLighterColor(brush.Color, 0.90);
                    else
                        linbrush += Parser.GetLighterColor(brush.Color, 0.55);
                    linbrush += ",1";


                    brushFront = Parser.ParseLinearGradient(linbrush);

                    linbrush = "90;";
                    linbrush += Parser.GetDarkerColor(brush.Color, 0.80);
                    linbrush += ",0;";
                    linbrush += Parser.GetDarkerColor(brush.Color, 0.99);
                    linbrush += ",1";

                    brushShade = Parser.ParseLinearGradient(linbrush);

                }
                #endregion Color Gradient

                rectBase.Fill = Cloner.CloneBrush(brush2);
                rectSide.Fill = Cloner.CloneBrush(brushShade);
                rectFront.Fill = Cloner.CloneBrush(brushFront);

                rectFront.StrokeThickness = 1;
                rectBase.StrokeThickness = 1;
                rectSide.StrokeThickness = 1;

                rectFront.Opacity = 1;
                rectBase.Opacity = 1;
                rectSide.Opacity = 1;

                SkewTransform st = new SkewTransform();
                st.AngleY = -45;
                rectSide.RenderTransform = st;

                st = new SkewTransform();
                st.AngleX = -45;
                rectBase.RenderTransform = st;



                this.Width = rectBase.Width + rectFront.Width;
                this.Children.Add(rectBase);
                this.Children.Add(rectFront);
                this.Children.Add(rectSide);
            }


            else
            {
                Int32 noOfIntervals;
                Double interval;
                Line ln = new Line();

                if (!Enabled || !_parent.Enabled) return;

                if (Interval > 0)
                {
                    interval = Interval;
                    noOfIntervals = (int)Math.Ceiling((_parent.AxisMaximum - _parent.AxisMinimum) / Interval);
                }
                else
                {
                    noOfIntervals = _parent.NumberOfIntervals;
                    interval = _parent.Interval;
                }
                this.Children.Clear();

                PlotDetails plotDetails = (_parent.Parent as Chart).PlotDetails;

                Double i = _parent.AxisMinimum;

                if (_parent.GetType().Name == "AxisX")
                    if (plotDetails.AllAxisLabels)
                    {
                        System.Collections.Generic.Dictionary<Double, String>.Enumerator enumerator = plotDetails.AxisLabels.GetEnumerator();

                        enumerator.MoveNext();
                        int j = 0;
                        for (i = enumerator.Current.Key; j < plotDetails.AxisLabels.Count - 1; j++)
                        {
                            enumerator.MoveNext();
                            if (i > enumerator.Current.Key)
                                i = enumerator.Current.Key;
                        }
                        enumerator.Dispose();
                    }
                Double vals;


                Double minValue;
                int countInterval = 0;
                if (_parent.AxisOrientation == AxisOrientation.Bar)
                {
                    if ((Double)_parent.AxisManager.GetMinimumDataValue() - interval < _parent.AxisMinimum && _parent.GetType().Name == "AxisX")
                        vals = (Double)_parent.AxisManager.GetMinimumDataValue();
                    else
                        vals = _parent.AxisMinimum;

                    minValue = vals;

                    while (vals <= _parent.AxisMaximum)
                    {
                        if (_parent.GetType().Name == "AxisX")
                            if (plotDetails.AllAxisLabels && i > plotDetails.MaxAxisXValue)
                                break;

                        ln = new Line();

                        if ((_parent.Parent as Chart).View3D)
                            ln.X1 = (Double)this.GetValue(WidthProperty) * 1.5;
                        else
                            ln.X1 = 0;
                        ln.X2 = ln.X1 + (Double)this.GetValue(WidthProperty);


                        ln.Y1 = _parent.DoubleToPixel(vals);


                        vals = minValue + interval * (++countInterval);

                        ln.Y2 = ln.Y1;

                        if (ln.Y1 < -0.5 || ln.Y1 > this.Height + 0.5) continue;

                        ln.Stroke = Visifire.Commons.Cloner.CloneBrush(LineBackground);
                        ln.StrokeThickness = LineThickness;

                        this.Children.Add(ln);
                    }
                }
                else if (_parent.AxisOrientation == AxisOrientation.Column)
                {
                    if ((Double)_parent.AxisManager.GetMinimumDataValue() - interval < _parent.AxisMinimum && _parent.GetType().Name == "AxisX")
                        vals = (Double)_parent.AxisManager.GetMinimumDataValue();
                    else
                        vals = _parent.AxisMinimum;

                    minValue = vals;

                    while (vals <= _parent.AxisMaximum)
                    {
                        if (_parent.GetType().Name == "AxisX")
                            if (plotDetails.AllAxisLabels && i > plotDetails.MaxAxisXValue)
                                break;

                        ln = new Line();


                        ln.X1 = _parent.DoubleToPixel(vals);


                        vals = minValue + interval * (++countInterval);

                        ln.X2 = ln.X1;
                        ln.Y1 = 0;
                        ln.Y2 = (Double)this.GetValue(HeightProperty);
                        if (ln.X1 < -0.5 || ln.X1 >= this.Width + 0.5) continue;

                        ln.Stroke = Visifire.Commons.Cloner.CloneBrush(LineBackground);
                        ln.StrokeThickness = LineThickness;

                        this.Children.Add(ln);
                    }
                }
            }
        }

        internal void SetWidth()
        {

            if (_parent.AxisOrientation == AxisOrientation.Bar)
            {
                if ((_parent.Parent as Chart).View3D)
                    this.Width = TickLength + _parent.PlankThickness;
                else
                    this.Width = TickLength;
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
                this.SetValue(HeightProperty, TickLength);

            }
        }

        internal void SetLeft()
        {


            if (_parent.AxisOrientation == AxisOrientation.Bar)
            {
                if ((_parent.Parent as Chart).View3D)
                {
                    if (_parent.GetType().Name == "AxisX")
                        this.SetValue(LeftProperty, (Double)_parent.AxisLabels.GetValue(LeftProperty) + _parent.AxisLabels.Width + (_parent.Parent as Chart).Padding);
                    else
                        this.SetValue(LeftProperty, (Double)_parent.GetValue(WidthProperty) - TickLength - (_parent.Parent as Chart).Padding);
                }
                else
                {
                    //This will bring the ticks to the left of the plot area for the vertcal axis
                    this.SetValue(LeftProperty, (Double)_parent.GetValue(WidthProperty) - TickLength);
                }
            }
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
                if ((_parent.Parent as Chart).View3D)
                {

                    this.SetValue(TopProperty, 0);
                }
                else
                {
                    this.SetValue(TopProperty, 0);
                }
            }
        }

        internal void Render()
        {
        }

        #endregion Internal Methods

        #region Private Methods

        private void SetDefaults()
        {
            LineBackground = new SolidColorBrush(Colors.LightGray);
            LineThickness = 1.0;
            LineStyle = "Solid";
            TickLength = 5;
            Interval = Double.NaN;
            Enabled = true;
            Width = 0;
            Height = 0;
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

        Axes _parent;

        
        private Double _interval;
        
        private Double _lineThickness;
        private Brush _lineBackground;
        private String _lineStyle;
        private Double _tickLength;

        #endregion Data
    }
}
