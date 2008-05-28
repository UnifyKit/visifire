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

        public Boolean Enabled
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
                    _tickLength = 5;
                return _tickLength;
            }
            set
            {
                _tickLength = value;
            }
        }

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
                DrawPlankColumnOrientation();
            }
            else if ((_parent.Parent as Chart).View3D && _parent.GetType().Name == "AxisX" && _parent.AxisOrientation == AxisOrientation.Bar)
            {
                DrawPlankBarOrientation();
            }
            else
            {
                DrawMajorTicks();
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
            LineThickness = 1;
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

        private void DrawPlankBarOrientation()
        {
            this.Children.Clear();

            if (!_parent.Enabled) return;

            Rectangle rectBase = new Rectangle();
            Rectangle rectFront = new Rectangle();
            Rectangle rectSide = new Rectangle();

            rectBase.Tag = _parent.Name;
            rectFront.Tag = _parent.Name;
            rectSide.Tag = _parent.Name;

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
            Brush topBrush = null;
            Brush sideBrush = null;
            Brush frontBrush = null;
            Brush tempBrush;
            if ((_parent.Parent as Chart).PlotArea.Background != null)
                tempBrush = Cloner.CloneBrush((_parent.Parent as Chart).PlotArea.Background);
            else if ((_parent.Parent as Chart).Background != null)
                tempBrush = Cloner.CloneBrush((_parent.Parent as Chart).Background);
            else
                tempBrush = Parser.ParseLinearGradient("0;#ffafafaf,0;#ffefefef,1");

            if (tempBrush.GetType().Name == "LinearGradientBrush")
            {
                LinearGradientBrush brush = tempBrush as LinearGradientBrush;
                topBrush = Cloner.CloneBrush(tempBrush);
                String linBrush1 = "0;", linBrush2 = "45;";

                foreach (GradientStop grad in brush.GradientStops)
                {

                    linBrush1 += Parser.GetDarkerColor(grad.Color, 0.75) + ",";
                    linBrush1 += grad.Offset.ToString() + ";";

                    linBrush2 += Parser.GetDarkerColor(grad.Color, 0.85) + ",";
                    linBrush2 += grad.Offset.ToString() + ";";
                }
                frontBrush = Parser.ParseLinearGradient(linBrush1);
                sideBrush = Parser.ParseLinearGradient(linBrush2);

            }
            else if (tempBrush.GetType().Name == "RadialGradientBrush")
            {
                RadialGradientBrush brush = tempBrush as RadialGradientBrush;
                topBrush = Cloner.CloneBrush(tempBrush);
                String linBrush1 = "0.5;0.5;", linBrush2 = "0.5;0.5;";

                foreach (GradientStop grad in brush.GradientStops)
                {

                    linBrush1 += Parser.GetDarkerColor(grad.Color, 0.65) + ",";
                    linBrush1 += grad.Offset.ToString() + ";";

                    linBrush2 += Parser.GetDarkerColor(grad.Color, 0.75) + ",";
                    linBrush2 += grad.Offset.ToString() + ";";
                }
                frontBrush = Parser.ParseRadialGradient(linBrush1);
                sideBrush = Parser.ParseRadialGradient(linBrush2);

            }
            else if (tempBrush.GetType().Name == "SolidColorBrush")
            {
                SolidColorBrush brush = tempBrush as SolidColorBrush;

                brush.Color = Parser.RemoveAlpha(brush.Color);
                //In case of video or image brush will have null
                String linbrush;
                linbrush = "135;";
                linbrush += Parser.GetDarkerColor(brush.Color, 0.75);
                linbrush += ",0;";
                if (Parser.GetBrushIntensity(brush) > 0.5)
                    linbrush += Parser.GetDarkerColor(brush.Color, 0.85);
                else
                    linbrush += Parser.GetLighterColor(brush.Color, 0.35);
                linbrush += ",1";

                topBrush = Parser.ParseLinearGradient(linbrush);

                linbrush = "0;";
                linbrush += Parser.GetDarkerColor(brush.Color, 0.65);
                linbrush += ",0;";
                if (Parser.GetBrushIntensity(brush) > 0.5)
                    linbrush += Parser.GetLighterColor(brush.Color, 0.90);
                else
                    linbrush += Parser.GetLighterColor(brush.Color, 0.55);
                linbrush += ",1";


                frontBrush = Parser.ParseLinearGradient(linbrush);

                linbrush = "90;";
                linbrush += Parser.GetDarkerColor(brush.Color, 0.80);
                linbrush += ",0;";
                linbrush += Parser.GetDarkerColor(brush.Color, 0.99);
                linbrush += ",1";

                sideBrush = Parser.ParseLinearGradient(linbrush);

            }
            #endregion Color Gradient

            rectBase.Fill = Cloner.CloneBrush(topBrush);
            rectSide.Fill = Cloner.CloneBrush(sideBrush);
            rectFront.Fill = Cloner.CloneBrush(frontBrush);

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

        private void DrawPlankColumnOrientation()
        {
            this.Children.Clear();

            if (!_parent.Enabled) return;

            Rectangle rectBase = new Rectangle();
            Rectangle rectFront = new Rectangle();
            Rectangle rectSide = new Rectangle();

            rectBase.Tag = _parent.Name;
            rectFront.Tag = _parent.Name;
            rectSide.Tag = _parent.Name;

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
                tempBrush = Cloner.CloneBrush((_parent.Parent as Chart).PlotArea.Background);
            else if ((_parent.Parent as Chart).Background != null)
                tempBrush = Cloner.CloneBrush((_parent.Parent as Chart).Background);
            else
                tempBrush = Parser.ParseLinearGradient("0;#ffafafaf,0;#ffefefef,1");



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

                brush.Color = Parser.RemoveAlpha(brush.Color);
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

        private Line CreateLine(Point start, Point end)
        {
            Line line = new Line();

            line.Tag = _parent.Name;

            line.X1 = start.X;
            line.Y1 = start.Y;

            line.X2 = end.X;
            line.Y2 = end.Y;

            line.Stroke = Cloner.CloneBrush(LineBackground);
            line.StrokeThickness = LineThickness;
            line.StrokeDashArray = Parser.GetStrokeDashArray(LineStyle);

            return line;
        }

        private void DrawMajorTicks()
        {
            if (!Enabled || !_parent.Enabled) return;

            Decimal interval = (Decimal)((Interval > 0) ? Interval : _parent.Interval);

            this.Children.Clear();

            Decimal position;

            if ((Double)(_parent.AxisManager.GetMinimumDataValue() - interval) < _parent.AxisMinimum && _parent.GetType().Name == "AxisX")
                position = _parent.AxisManager.GetMinimumDataValue();
            else
                position = (Decimal)_parent.AxisMinimum;

            Decimal minimumValue = position;
            Int32 intervalCount = 0;

            Point start = new Point();
            Point end = new Point();

            while (position <= (Decimal)_parent.AxisMaximum)
            {
                switch (_parent.AxisOrientation)
                {
                    case AxisOrientation.Column:
                        start.X = _parent.DoubleToPixel((Double)position);
                        start.Y = 0;

                        end.X = start.X;
                        end.Y = this.Height;

                        if (start.X < -0.5 || start.X >= this.Width + 0.5) continue;

                        this.Children.Add(CreateLine(start, end));
                        break;
                    case AxisOrientation.Bar:
                        if (_parent._parent.View3D)
                            start.X = (Double)_parent._parent.PlotArea.GetValue(LeftProperty) - (Double)this.GetValue(LeftProperty) - this.Width - TickLength / 2;
                        else
                            start.X = this.Width - TickLength;
                        start.Y = _parent.DoubleToPixel((Double)position);

                        end.X = start.X + TickLength;
                        end.Y = start.Y;

                        if (start.Y < -0.5 || start.Y > this.Height + 0.5) continue;

                        this.Children.Add(CreateLine(start, end));
                        break;
                    default:
                        break;
                }
                position = minimumValue + interval * (++intervalCount);
            }

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
