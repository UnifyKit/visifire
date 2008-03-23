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
using System.Net;
using System.Windows.Browser;
using System.Collections.Generic;
using Visifire.Commons;
using Visifire.Charts;
using System.Windows.Markup;

namespace Visifire.Charts
{

    public class DataPoint : VisualObject, IComparable
    {
        #region Public Methods

        public DataPoint()
        {
            
        }

        public int CompareTo(Object o)
        {
            DataPoint dataPoint = (DataPoint)o;
            return this.XValue.CompareTo(dataPoint.XValue);
        }

        public override void Init()
        {
            // Validate Parent element
            // Parent element should be a DataSeries element. Else throw an exception.
            if (this.Parent.GetType().Name == "DataSeries")
                _parent = this.Parent as DataSeries;
            else
                throw new Exception(this + "Parent should be a DataSeries");

            SetName();


            if (base.Background != null)
            {
                this.Background = Cloner.CloneBrush(base.Background);
                base.Background = null;
            }
            else if (this.Background == null)
            {
                this.Background = Cloner.CloneBrush(_parent.ColorSetReference.GetColor(Index));
            }


            // When the render as type is Pie or Doughnut then display datapoints in legend by default if ShowInLegend is not defined
            // this is done in init of dataPoint
            if (_showInLegend == "Undefined" && _parent._showInLegend == "Undefined")
            {
                if (_parent.RenderAs.ToLower() == "pie" || _parent.RenderAs.ToLower() == "doughnut")
                {
                    _showInLegend = true.ToString();
                }
                else
                {
                    _showInLegend = false.ToString();
                }
            }
            else if (_showInLegend == "Undefined" && _parent._showInLegend.ToLower() == "true")
            {
                if (_parent.RenderAs.ToLower() == "pie" || _parent.RenderAs.ToLower() == "doughnut")
                {
                    _showInLegend = true.ToString();
                }
            }
        }

        public Marker PlaceMarker()
        {
            // This part of the code cause marker to be displayed by default
            if (_parent.RenderAs.ToLower() != "line" && _markerEnabled == "Undefined" && _parent._markerEnabled == "Undefined")
            {
                if (MarkerEnabled.ToLower() != "true") return null;
            }
            else if (_markerEnabled.ToLower() == "false" || _parent._markerEnabled.ToLower() == "false")
            {
                return null;
            }

            Marker marker = new Marker();

            _markerRef = marker;
            Double top;
            Double left;
            marker.ImageScale = MarkerScale;
            marker.Color = MarkerBackground;
            marker.BorderThickness = MarkerBorderThickness;
            marker.BorderColor = MarkerBorderColor;
            if (MarkerImage == null)
            {
                if (_parent.RenderAs == "Bubble")
                    marker.Shadow = _parent.ShadowEnabled;
                marker.Style = MarkerStyle;
                marker.Size = MarkerSize;


                if (_parent._parent.PlotDetails.AxisOrientation == AxisOrientation.Bar)
                {
                    top = (Double)GetValue(TopProperty) + ((Double)GetValue(HeightProperty) - MarkerSize * MarkerScale) / 2;
                    if (this.YValue >= 0)
                    {
                        left = (Double)GetValue(LeftProperty) + (Double)GetValue(WidthProperty) - MarkerSize * MarkerScale / 2;
                    }
                    else
                    {
                        left = (Double)GetValue(LeftProperty) - MarkerSize * MarkerScale / 2;
                    }

                }
                else
                {
                    if (this.YValue >= 0)
                    {
                        top = (Double)GetValue(TopProperty) - (marker.Height / 2 * MarkerScale);
                    }
                    else
                    {
                        top = (Double)GetValue(TopProperty) + this.Height - (marker.Height / 2 * MarkerScale);
                    }

                    left = (Double)GetValue(LeftProperty) + this.Width / 2 - (marker.Width / 2 * MarkerScale);
                }


            }
            else
            {

                marker.ImagePath = MarkerImage;
                marker.Style = MarkerStyle;


                marker.Size = MarkerSize;

                if (_parent._parent.PlotDetails.AxisOrientation == AxisOrientation.Bar)
                {
                    top = (Double)GetValue(TopProperty) + ((Double)GetValue(HeightProperty) - marker.Height * MarkerScale) / 2;
                    if (this.YValue >= 0)
                    {
                        left = (Double)GetValue(LeftProperty) + (Double)GetValue(WidthProperty) - marker.Width * MarkerScale / 2;
                    }
                    else
                    {
                        left = (Double)GetValue(LeftProperty) - marker.Width / 2;
                    }

                }
                else
                {
                    if (this.YValue >= 0)
                    {
                        top = (Double)GetValue(TopProperty) - marker.Height * MarkerScale / 2;
                    }
                    else
                    {
                        top = (Double)GetValue(TopProperty) + (Double)GetValue(HeightProperty) - (marker.Height * MarkerScale / 2);
                    }
                    left = (Double)GetValue(LeftProperty) + (Double)GetValue(WidthProperty) / 2 - (marker.Width * MarkerScale / 2);
                }

            }
            if (_parent._parent.View3D && _parent.RenderAs != "Point" && _parent.RenderAs != "Bubble")
            {
                marker.SetValue(TopProperty, top + (Double)_parent._parent.PlotArea.GetValue(TopProperty));
                marker.SetValue(LeftProperty, left + (Double)_parent._parent.PlotArea.GetValue(LeftProperty));

                int chType = -1;

                if (_parent.RenderAs.ToUpper() == "COLUMN")
                    chType = (int)Surface3DCharts.Column;
                else if (_parent.RenderAs.ToUpper() == "STACKEDCOLUMN")
                    chType = (int)Surface3DCharts.StackedColumn;
                else if (_parent.RenderAs.ToUpper() == "BAR")
                    chType = (int)Surface3DCharts.Bar;
                else if (_parent.RenderAs.ToUpper() == "STACKEDBAR")
                    chType = (int)Surface3DCharts.StackedBar;
                else if (_parent.RenderAs.ToUpper() == "STACKEDBAR100")
                    chType = (int)Surface3DCharts.StackedBar100;
                else if (_parent.RenderAs.ToUpper() == "STACKEDCOLUMN100")
                    chType = (int)Surface3DCharts.StackedColumn100;
                else if (_parent.RenderAs.ToUpper() == "AREA")
                    chType = (int)Surface3DCharts.Area;
                else if (_parent.RenderAs.ToUpper() == "STACKEDAREA")
                    chType = (int)Surface3DCharts.StackedArea;
                else if (_parent.RenderAs.ToUpper() == "STACKEDAREA100")
                    chType = (int)Surface3DCharts.StackedArea100;
                else if (_parent.RenderAs.ToUpper() == "LINE")
                    chType = (int)Surface3DCharts.Line;
                else
                    _parent._parent.Children.Add(marker);

                if (chType >= 0) _parent._drawingCanvas.Children.Add(marker);

            }
            else
            {

                marker.SetValue(TopProperty, top);
                marker.SetValue(LeftProperty, left);
                _parent.Children.Add(marker);


            }



            marker.MouseEnter += delegate(object sender, MouseEventArgs e)
            {
                ((Marker)sender).BorderColor = new SolidColorBrush(Colors.Red);
                ((Marker)sender).BorderThickness = ((Marker)sender).BorderThickness * 1.5;
                if (!String.IsNullOrEmpty(Href))
                    _parent.Cursor = Cursors.Hand;


            };

            marker.MouseLeave += delegate(object sender, MouseEventArgs e)
            {
                ((Marker)sender).BorderColor = Cloner.CloneBrush(MarkerBorderColor);
                ((Marker)sender).BorderThickness = MarkerBorderThickness;
                if (!String.IsNullOrEmpty(Href))
                    _parent.Cursor = Cursors.Arrow;


                _parent.ToolTip.Visibility = Visibility.Collapsed;
            };
            if (!String.IsNullOrEmpty(Href))
            {
                marker.MouseLeftButtonUp += delegate(object sender, MouseButtonEventArgs e)
                {
                    if (!Href.ToLower().StartsWith("http://"))
                        Href = Href.Insert(0, "http://");
                    HtmlPage.Window.Navigate(new Uri(Href));
                };
            }

            return marker;
        }

        public override System.Collections.Generic.List<Point> GetBoundingPoints()
        {
            return points;
        }

        public void AttachLabel(FrameworkElement element, Double depth)
        {
            if (element == null) return;
            _label = new Label();

            _label.Background = Cloner.CloneBrush(LabelBackground);

            _label.SetTextWrap(_parent._parent.Width * _parent._parent.Label.TextWrap);

            ApplyLabelFontColor(_label, false);

            _label.FontFamily = LabelFontFamily;
            _label.FontWeight = LabelFontWeight;
            _label.FontStyle = LabelFontStyle;
            if (Double.IsNaN(LabelFontSize))
                _label.FontSize = CalculateFontSize();
            else
                _label.FontSize = LabelFontSize;

            _label.BorderThickness = 0;

            Double top = 0;
            Double left = 0;
            Double LeftCorrection = 0;
            Double TopCorrection = 0;
            Double factor = 1;
            Double offset = 0;

            if (_parent._parent.View3D)
            {

                _parent._drawingCanvas.Children.Add(_label);
                LeftCorrection = (Double)_parent._parent.PlotArea.GetValue(LeftProperty);
                TopCorrection = (Double)_parent._parent.PlotArea.GetValue(TopProperty);
                factor = 2;
                offset = depth;
            }
            else
            {
                switch (_parent.RenderAs.ToLower())
                {
                    case "area":
                    case "stackedarea":
                    case "stackedarea100":
                        if (_parent._parent._areaLabelMarker == null)
                        {
                            _parent._parent._areaLabelMarker = new Canvas();
                            _parent._parent.Children.Add(_parent._parent._areaLabelMarker);
                            _parent._parent._areaLabelMarker.SetValue(ZIndexProperty, (int)_parent.GetValue(ZIndexProperty) + 1000);
                            _parent._parent._areaLabelMarker.SetValue(LeftProperty, _parent.GetValue(LeftProperty));
                            _parent._parent._areaLabelMarker.SetValue(TopProperty, _parent.GetValue(TopProperty));
                        }
                        _parent._parent._areaLabelMarker.Children.Add(_label);
                        break;
                    default:
                        this._parent.Children.Add(_label);
                        break;
                }


            }
            Label.Text = TextParser(LabelText);

            Label.Visibility = Visibility.Visible;
            if (LabelStyle == "Outside")
            {
                if (_parent._parent.PlotDetails.AxisOrientation == AxisOrientation.Bar)
                {
                    factor = _parent._parent.View3D ? offset + 5 : 5;

                    top = (Double)GetValue(TopProperty) + ((Double)GetValue(HeightProperty) - (Double)Label.GetValue(HeightProperty)) / 2;
                    if (this.YValue >= 0)
                    {
                        left = (Double)GetValue(LeftProperty) + (Double)GetValue(WidthProperty) + factor;

                        // if label is going outside plotarea put it back inside plot area (right edge)
                        if (left + Label.Width > _parent._parent.PlotArea.Width)
                        {
                            ApplyLabelFontColor(Label, true);
                            left = (Double)GetValue(LeftProperty) + (Double)GetValue(WidthProperty) - Label.Width - factor;
                        }

                    }
                    else
                    {
                        left = (Double)GetValue(LeftProperty) - Label.Width - factor / 2;

                        //if label is moving out of plot area then bring it back in (left edge)
                        if (left < 0)
                        {
                            ApplyLabelFontColor(Label, true);
                            left = (Double)GetValue(LeftProperty) + factor / 2;
                        }
                    }

                }
                else
                {
                    factor = _parent._parent.View3D ? offset + 5 : 5;

                    if ((Double)GetValue(WidthProperty) < Label.Width && (_parent.RenderAs.ToLower() == "column" || _parent.RenderAs.ToLower() == "stackedcolumn" || _parent.RenderAs.ToLower() == "stackedcolumn100"))
                    {
                        Label.RotateLabelVerticle();
                    }
                    if (_parent.RenderAs.ToLower() == "point" || _parent.RenderAs.ToLower() == "line" || _parent.RenderAs.ToLower() == "bubble")
                    {
                        left = (Double)GetValue(LeftProperty) + (Double)GetValue(WidthProperty) / 2 - (Double)Label.GetValue(WidthProperty) / 2;
                    }
                    else
                    {
                        left = (Double)GetValue(LeftProperty) + (Double)GetValue(WidthProperty) / 2 - (Double)Label.GetValue(WidthProperty) / 2 + offset;
                    }
                    if (this.YValue >= 0)
                    {
                        top = (Double)GetValue(TopProperty) - (Double)Label.GetValue(HeightProperty) - factor;

                        //if label is moving out of plot area then bring it back in (top edge)
                        if (top < 0)
                        {
                            if (_parent.RenderAs.ToLower() != "point" && _parent.RenderAs.ToLower() != "line" && _parent.RenderAs.ToLower() != "bubble")
                                ApplyLabelFontColor(Label, true);
                            top = (Double)GetValue(TopProperty) + factor / 2;
                            left = (Double)GetValue(LeftProperty) + (Double)GetValue(WidthProperty) / 2 - (Double)Label.GetValue(WidthProperty) / 2;
                        }
                    }
                    else
                    {
                        factor = 5;
                        top = (Double)GetValue(TopProperty) + (Double)GetValue(HeightProperty) + factor;
                        left = (Double)GetValue(LeftProperty) + (Double)GetValue(WidthProperty) / 2 - (Double)Label.GetValue(WidthProperty) / 2;

                        if (top + Label.Height > _parent._parent.PlotArea.Height + offset)
                        {
                            if (_parent.RenderAs.ToLower() != "point" && _parent.RenderAs.ToLower() != "line" && _parent.RenderAs.ToLower() != "bubble")
                                ApplyLabelFontColor(Label, true);
                            top = (Double)GetValue(TopProperty) + (Double)GetValue(HeightProperty) - (Double)Label.GetValue(HeightProperty) - factor / 2;
                            left = (Double)GetValue(LeftProperty) + (Double)GetValue(WidthProperty) / 2 - (Double)Label.GetValue(WidthProperty) / 2;
                        }
                    }

                }
            }
            else if (LabelStyle == "Inside")
            {

                if (_parent._parent.PlotDetails.AxisOrientation == AxisOrientation.Bar)
                {
                    ApplyLabelFontColor(Label, true);
                    factor = _parent._parent.View3D ? 5 : 5;

                    top = (Double)GetValue(TopProperty) + ((Double)GetValue(HeightProperty) - (Double)Label.GetValue(HeightProperty)) / 2;
                    if (this.YValue >= 0)
                    {
                        left = (Double)GetValue(LeftProperty) + (Double)GetValue(WidthProperty) - Label.Width - factor - offset;
                        if (left < (Double)GetValue(LeftProperty))
                            left = (Double)GetValue(LeftProperty) - offset;
                    }
                    else
                    {
                        left = (Double)GetValue(LeftProperty);
                    }

                }
                else
                {
                    factor = _parent._parent.View3D ? offset + 5 : 5;

                    if ((this.Width < this.Height) && (this.Width < Label.Width) && (_parent.RenderAs.ToLower() == "column" || _parent.RenderAs.ToLower() == "stackedcolumn" || _parent.RenderAs.ToLower() == "stackedcolumn100"))
                    {
                        Label.RotateLabelVerticle();
                    }

                    if (_parent.RenderAs.ToLower() == "area" || _parent.RenderAs.ToLower() == "stackedarea" || _parent.RenderAs.ToLower() == "stackedarea100")
                    {

                        if (this.YValue >= 0)
                        {
                            top = (Double)GetValue(TopProperty) + factor / 2;
                            left = (Double)GetValue(LeftProperty) + this.Width / 2 - (Double)Label.GetValue(WidthProperty) / 2;
                        }
                        else
                        {
                            top = (Double)GetValue(TopProperty) + (Double)GetValue(HeightProperty) - (Double)Label.GetValue(HeightProperty) - factor / 2;
                            left = (Double)GetValue(LeftProperty) + this.Width / 2 - (Double)Label.GetValue(WidthProperty) / 2;
                        }
                    }
                    else
                    {
                        ApplyLabelFontColor(Label, true);
                        if (this.YValue >= 0)
                        {
                            top = (Double)GetValue(TopProperty) + factor / 2;
                            if (top + Label.Height > (Double)GetValue(TopProperty) + Height)
                                top = (Double)GetValue(TopProperty) + Height - Label.Height;
                            left = (Double)GetValue(LeftProperty) + (Double)GetValue(WidthProperty) / 2 - (Double)Label.GetValue(WidthProperty) / 2;
                        }
                        else
                        {
                            top = (Double)GetValue(TopProperty) + (Double)GetValue(HeightProperty) - (Double)Label.GetValue(HeightProperty) - factor / 2;
                            if (top < (Double)GetValue(TopProperty))
                                top = (Double)GetValue(TopProperty);
                            left = (Double)GetValue(LeftProperty) + (Double)GetValue(WidthProperty) / 2 - (Double)Label.GetValue(WidthProperty) / 2;
                        }
                    }

                }
            }
            Label.SetValue(TopProperty, top + TopCorrection);
            Label.SetValue(LeftProperty, left + LeftCorrection);
            Label.SetValue(ZIndexProperty, (int)element.GetValue(ZIndexProperty) + 1000);
        }

        public void AttachLabel(Double top, Double left, Double radius, Double angle, Point center, Double yScalingFactor)
        {
            _label = new Label();

            _label.Background = Cloner.CloneBrush(LabelBackground);

            _label.SetTextWrap(_parent._parent.Width * _parent._parent.Label.TextWrap);
            _label.IsHitTestVisible = false;
            ApplyLabelFontColor(_label, false);

            _label.FontFamily = LabelFontFamily;
            _label.FontWeight = LabelFontWeight;
            _label.FontStyle = LabelFontStyle;


            _label.FontSize = LabelFontSize;

            _label.BorderThickness = 0;

            this._parent.Children.Add(_label);
            Label.Text = TextParser(LabelText);


            Double newLeft = left;
            Double newTop = top;

            if (angle > Math.PI / 2 && angle < Math.PI * 3 / 2)
                newLeft = left - _label.Width - 10;
            else
                newLeft = left + 10;

            // this condition places the label such that they do not go outside of the plot area in horizontal direction
            if (angle > (Math.PI / 2) && angle < (Math.PI * 3 / 2))
            {

                if (newLeft < 0)
                    newLeft = 0;
            }
            else
            {

                if ((newLeft + _label.Width) > (_parent._parent.PlotArea.Width))
                    newLeft = (_parent._parent.PlotArea.Width - _label.Width);
            }

            // this condition places the label such that they do not go outside of the plot area in horizontal direction
            if (angle > Math.PI && angle < Math.PI * 2)
            {
                if (newTop < 0)
                    newTop = 0;
            }
            else
            {
                if ((newTop + _label.Height) > (_parent._parent.PlotArea.Height))
                    newTop = _parent._parent.PlotArea.Height - _label.Height;

            }

            if (LabelLineEnabled.ToLower() == "true" && LabelStyle.ToLower() == "outside")
            {
                Point[] points = new Point[3];


                if (angle > (Math.PI / 2) && angle < (Math.PI * 3 / 2))
                {
                    points[0].X = newLeft + Label.Width;
                    points[1].X = newLeft + Label.Width + 10;
                }
                else
                {
                    points[0].X = newLeft;
                    points[1].X = points[0].X - 10;
                }


                points[0].Y = newTop + Label.Height / 2;
                points[1].Y = points[0].Y;

                points[2].X = center.X + radius * Math.Cos(angle);
                points[2].Y = center.Y + radius * Math.Sin(angle) * yScalingFactor;
                String lineXAML = "<Polyline xmlns=\"http://schemas.microsoft.com/client/2007\" Points=\"" + points[0] + " " + points[1] + " " + points[2] + "\"/>";
                _labelLine = (Polyline)XamlReader.Load(lineXAML);
                if (LabelLineColor != null)
                    _labelLine.Stroke = Cloner.CloneBrush(LabelLineColor);
                else if (_labelFontColor != null)
                    _labelLine.Stroke = Cloner.CloneBrush(LabelFontColor);
                else
                    _labelLine.Stroke = Cloner.CloneBrush(_label.FontColor);
                _labelLine.StrokeThickness = LabelLineThickness;
                switch (LabelLineStyle)
                {
                    case "Solid":
                        break;

                    case "Dashed":
                        _labelLine.StrokeDashArray = Converter.ArrayToCollection(new Double[] { 4, 4, 4, 4 });
                        break;

                    case "Dotted":
                        _labelLine.StrokeDashArray = Converter.ArrayToCollection(new Double[] { 1, 2, 1, 2 });
                        break;
                }
                _labelLine.Points = Converter.ArrayToCollection(points);
                _labelLine.Opacity = 1;
                _labelLine.Visibility = Visibility.Visible;

                _labelLine.SetValue(ZIndexProperty, 100);


                this._parent.Children.Add(_labelLine);
            }



            Label.Visibility = Visibility.Visible;


            Label.SetValue(TopProperty, newTop);
            Label.SetValue(LeftProperty, newLeft);
            if (LabelStyle.ToLower() == "outside")
                Label.SetValue(ZIndexProperty, 400);
            else
                Label.SetValue(ZIndexProperty, 400);
        }

        public override String TextParser(String unParsed)
        {

            String str = new String(unParsed.ToCharArray());
            if (str.Contains("##XValue"))
                str = str.Replace("##XValue", "#XValue");
            else
            {
                if (String.IsNullOrEmpty(_parent.XValueFormatString))
                    str = str.Replace("#XValue", _parent._parent.AxisX.GetFormattedText(XValue));
                else
                    str = str.Replace("#XValue", XValue.ToString(_parent.XValueFormatString));
            }

            if (str.Contains("##YValue"))
                str = str.Replace("##YValue", "#YValue");
            else
            {
                if (String.IsNullOrEmpty(_parent.YValueFormatString))
                    str = str.Replace("#YValue", _parent._parent.AxisY.GetFormattedText(YValue));
                else
                    str = str.Replace("#YValue", YValue.ToString(_parent.YValueFormatString));
            }

            if (str.Contains("##ZValue"))
                str = str.Replace("##ZValue", "#ZValue");
            else
            {
                str = str.Replace("#ZValue", ZValue.ToString(_parent.ZValueFormatString));
            }

            if (str.Contains("##Series"))
                str = str.Replace("##Series", "#Series");
            else
                str = str.Replace("#Series", _parent.Name);

            if (str.Contains("##AxisLabel"))
                str = str.Replace("##AxisLabel", "#AxisLabel");
            else
                str = str.Replace("#AxisLabel", String.IsNullOrEmpty(AxisLabel) ? GetAxisLabelString() : AxisLabel);

            if (str.Contains("##Percentage"))
                str = str.Replace("##Percentage", "#Percentage");
            else
                str = str.Replace("#Percentage", Percentage().ToString("#0.##"));


            if (str.Contains("##Sum"))
                str = str.Replace("##Sum", "#Sum");
            else
            {
                str = str.Replace("#Sum", _parent._parent.AxisY.GetFormattedText(_parent._parent._stackSum[XValue].X));//_stackSum[XValue].X contains sum of all data points with same X value
            }
            return str;
        }

        public override void SetWidth() { }

        public override void SetHeight() { }

        public override void SetLeft() { }

        public override void SetTop() { }

        /// <summary>
        /// Draws a border for the chart. Border itseld will become clipping region for the chart.
        /// </summary>
        public override void ApplyBorder()
        {
            _borderRectangle.Width = this.Width;
            _borderRectangle.Height = this.Height;

            _borderRectangle.Stroke = BorderColor;
            _borderRectangle.StrokeThickness = BorderThickness;
            _borderRectangle.RadiusX = RadiusX;
            _borderRectangle.RadiusY = RadiusY;


            switch (this._borderStyle)
            {
                case "Solid":
                    break;

                case "Dashed":
                    _borderRectangle.StrokeDashArray = Converter.ArrayToCollection(new Double[] { 4, 4, 4, 4 });
                    break;

                case "Dotted":
                    _borderRectangle.StrokeDashArray = Converter.ArrayToCollection(new Double[] { 1, 2, 1, 2 });
                    break;
            }

            RectangleGeometry rg = new RectangleGeometry();
            rg.Rect = new Rect(0, 0, _borderRectangle.Width + 6, _borderRectangle.Height);
            rg.RadiusX = RadiusX;
            rg.RadiusY = RadiusY;

            this.Clip = rg;
        }

        #endregion  Public Methods

        #region Private Methods
        protected override void SetDefaults()
        {
            base.SetDefaults();

            _shadowRectangle = new Rectangle();
            this.Children.Add(_shadowRectangle);

            _borderRectangle = new Rectangle();
            this.Children.Add(_borderRectangle);

            _lineShadow = new Line();
            this.Children.Add(_lineShadow);

            _line = new Line();
            this.Children.Add(_line);

            _polygon = new Polygon();
            this.Children.Add(_polygon);

            XValue = Double.NaN;
            YValue = Double.NaN;

            RadiusX = Double.NaN;
            RadiusY = Double.NaN;

            _borderThickness = Double.NaN;
            BorderColor = null;
            Enabled = true;
            _markerBorderThickness = Double.NaN;
            _markerSize = Double.NaN;
            _markerScale = Double.NaN;
            _labelText = "";
            _labelEnabled = "Undefined";
            _markerEnabled = "Undefined";
            _labelLineEnabled = "Undefined";
            _labelFontColor = null;
            _labelFontSize = Double.NaN;
            _explodeOffset = 0;
            _showInLegend = "Undefined";
            this.SetValue(ZIndexProperty, 0);
        }

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

        private int CalculateFontSize()
        {
            int[] fontSizes = { 6, 8, 10, 12, 14, 16, 18, 20, 24, 28, 32, 36, 40 };
            Double _parentSize = _parent._parent.PlotArea.Width * _parent._parent.PlotArea.Height;
            return fontSizes[(int)(Math.Ceiling(((_parentSize + 161027.5) / 163840)) % fontSizes.Length)];
        }

        private void ApplyLabelFontColor(Label label, Boolean onDataPoint)
        {
            System.Windows.Media.Color color = new System.Windows.Media.Color();
            Double intensity;
            if (onDataPoint && _parent.RenderAs.ToLower() != "line")
            {
                if (LabelFontColor == null)
                {
                    intensity = Parser.GetBrushIntensity(Background);
                    if (intensity <= 0.5)
                    {
                        label.FontColor = new SolidColorBrush(Colors.White);

                    }
                    else
                    {
                        label.FontColor = new SolidColorBrush(Colors.Black);
                    }
                }
            }
            else
            {
                if (LabelFontColor == null)
                {

                    if (_parent._parent.PlotArea.Background == null)
                    {
                        if (_parent._parent.Background == null)
                        {
                            label.FontColor = new SolidColorBrush(Colors.Black);
                        }
                        else
                        {


                            intensity = Parser.GetBrushIntensity(_parent._parent.Background);
                            if (intensity <= 0.5)
                            {

                                label.FontColor = Parser.ParseSolidColor("#BBBBBB");
                            }
                            else
                            {
                                label.FontColor = new SolidColorBrush(Colors.Black);
                            }

                        }
                    }
                    else
                    {

                        intensity = Parser.GetBrushIntensity(_parent._parent.PlotArea.Background);
                        if (intensity <= 0.5)
                        {

                            label.FontColor = Parser.ParseSolidColor("#BBBBBB");
                        }
                        else
                        {
                            label.FontColor = new SolidColorBrush(Colors.Black);
                        }

                    }
                }
                else
                {
                    label.FontColor = Cloner.CloneBrush(LabelFontColor);
                }
            }
        }
        private void PositionLabelInside(Label _label, int recurCount)
        {
            Double top;
            Double left;
            Double offset = recurCount >= 2 ? 0 : 5;
            Double plotLeft = (Double)_parent._parent.PlotArea.GetValue(LeftProperty);
            Double plotRight = plotLeft + _parent._parent.PlotArea.Width;
            Double plotTop = (Double)_parent._parent.PlotArea.GetValue(TopProperty);
            Double plotBottom = plotTop + _parent._parent.PlotArea.Height + (_parent._parent.View3D ? _parent._parent.AxisX.MajorTicks.TickLength : 0);
            Double Depth = _parent._parent.AxisX.MajorTicks.TickLength;

            if (_parent._parent.View3D)
            {
                if (_parent._parent.PlotDetails.AxisOrientation == AxisOrientation.Bar)
                {
                    top = (Double)GetValue(TopProperty) + Height / 2 - _label.Height / 2;
                    if (YValue >= 0)
                    {
                        left = (Double)GetValue(LeftProperty) + Width - _label.Width - offset;
                    }
                    else
                    {
                        left = (Double)GetValue(LeftProperty) + offset;
                    }
                    if ((left < (Double)GetValue(LeftProperty) || (left + _label.Width) > (Double)GetValue(LeftProperty) + this.Width) && recurCount < 2)
                    {
                        _label.RotateLabelVerticle();
                        PositionLabelInside(_label, recurCount++);
                    }
                    else
                    {
                        _label.SetValue(LeftProperty, left);
                        _label.SetValue(TopProperty, top);
                    }


                }
                else
                {
                    left = (Double)GetValue(LeftProperty) + this.Width / 2 - _label.Width / 2;
                    if (YValue >= 0)
                    {
                        top = (Double)GetValue(TopProperty) + offset;
                    }
                    else
                    {
                        top = (Double)GetValue(TopProperty) + this.Height - _label.Height - offset;
                    }
                    if ((top < (Double)GetValue(TopProperty) || (top + _label.Height) > (Double)GetValue(TopProperty) + this.Height) && recurCount < 2)
                    {
                        _label.RotateLabelHorizontal();
                        PositionLabelInside(_label, recurCount++);
                    }
                    else
                    {
                        _label.SetValue(LeftProperty, left);
                        _label.SetValue(TopProperty, top);
                    }

                }
            }
            else
            {
                if (_parent._parent.PlotDetails.AxisOrientation == AxisOrientation.Bar)
                {
                    top = (Double)GetValue(TopProperty) + this.Height / 2 - _label.Height / 2;
                    if (YValue >= 0)
                    {
                        left = (Double)GetValue(LeftProperty) + this.Width - _label.Width - offset;
                    }
                    else
                    {
                        left = (Double)GetValue(LeftProperty) + offset;
                    }
                    if ((left < (Double)GetValue(LeftProperty) || (left + _label.Width) > (Double)GetValue(LeftProperty) + this.Width) && recurCount < 2)
                    {
                        _label.RotateLabelVerticle();
                        PositionLabelInside(_label, recurCount++);
                    }
                    else
                    {
                        _label.SetValue(LeftProperty, left);
                        _label.SetValue(TopProperty, top);
                    }
                }
                else
                {
                    left = (Double)GetValue(LeftProperty) + this.Width / 2 - _label.Width / 2;
                    if (YValue >= 0)
                    {
                        top = (Double)GetValue(TopProperty) + offset;
                    }
                    else
                    {
                        top = (Double)GetValue(TopProperty) + this.Height - _label.Height - offset;
                    }
                    if ((top < (Double)GetValue(TopProperty) || (top + _label.Height) > (Double)GetValue(TopProperty) + this.Height) && recurCount < 2)
                    {
                        _label.RotateLabelHorizontal();
                        PositionLabelInside(_label, recurCount++);
                    }
                    else
                    {
                        _label.SetValue(LeftProperty, left);
                        _label.SetValue(TopProperty, top);
                    }
                }
            }
        }
        private void PositionLabelOutside(Label _label, int recurCount)
        {
            Double top;
            Double left;
            Double offset = 5;
            Double plotLeft = 0;
            Double plotRight = plotLeft + _parent._parent.PlotArea.Width;
            Double plotTop = 0;
            Double plotBottom = plotTop + _parent._parent.PlotArea.Height + (_parent._parent.View3D ? _parent._parent.AxisX.MajorTicks.TickLength : 0);
            Double Depth = _parent._parent.AxisX.MajorTicks.TickLength;

            if (_parent._parent.View3D)
            {
                if (_parent._parent.PlotDetails.AxisOrientation == AxisOrientation.Bar)
                {
                    top = ((Double)this.GetValue(TopProperty) * 2 + this.Height) / 2 - _label.Height / 2;
                    if (YValue >= 0)
                    {
                        left = (Double)this.GetValue(LeftProperty) + this.Width + Depth + offset;
                    }
                    else
                    {
                        left = (Double)this.GetValue(LeftProperty) - _label.Width - offset;
                    }

                    if ((left < plotLeft || (left + _label.Width) > plotRight) && recurCount < 2)
                    {
                        PositionLabelInside(_label, recurCount++);
                    }
                    else
                    {
                        _label.SetValue(LeftProperty, left);
                        _label.SetValue(TopProperty, top);
                    }
                }
                else
                {
                    if (this.Width < _label.Width && (_parent.RenderAs.ToLower().Contains("column")))
                    {
                        _label.RotateLabelVerticle();
                    }

                    left = ((Double)this.GetValue(LeftProperty) * 2 + Depth + this.Width) / 2 - _label.Width / 2;
                    if (YValue >= 0)
                    {
                        top = (Double)GetValue(TopProperty) - _label.Height - Depth - offset;
                    }
                    else
                    {
                        top = (Double)GetValue(TopProperty) + Height + offset;
                    }
                    if ((top < plotTop || (top + _label.Height) > plotBottom) && recurCount < 2)
                    {
                        PositionLabelInside(_label, recurCount++);
                    }
                    else
                    {
                        _label.SetValue(LeftProperty, left);
                        _label.SetValue(TopProperty, top);
                    }
                }
            }
            else
            {
                if (_parent._parent.PlotDetails.AxisOrientation == AxisOrientation.Bar)
                {
                    top = (Double)GetValue(TopProperty) + this.Height / 2 - _label.Height / 2;
                    if (YValue >= 0)
                    {
                        left = (Double)GetValue(LeftProperty) + this.Width + offset;
                    }
                    else
                    {
                        left = (Double)GetValue(LeftProperty) - _label.Width - offset;
                    }
                    if ((left < plotLeft || (left + _label.Width) > plotRight) && recurCount < 2)
                    {
                        PositionLabelInside(_label, recurCount++);
                    }
                    else
                    {
                        _label.SetValue(LeftProperty, left);
                        _label.SetValue(TopProperty, top);
                    }
                }
                else
                {
                    left = (Double)GetValue(LeftProperty) + this.Width / 2 - _label.Width / 2;
                    if (YValue >= 0)
                    {
                        top = (Double)GetValue(TopProperty) - _label.Height - offset;
                    }
                    else
                    {
                        top = (Double)GetValue(TopProperty) + this.Height + offset;
                    }
                    if ((top < plotTop || (top + _label.Height) > plotBottom) && recurCount < 2)
                    {
                        PositionLabelInside(_label, recurCount++);
                    }
                    else
                    {
                        _label.SetValue(LeftProperty, left);
                        _label.SetValue(TopProperty, top);
                    }
                }
            }
        }



        private Double Percentage()
        {
            Double percentage = 0;
            if (_parent.RenderAs.ToLower() == "pie" || _parent.RenderAs.ToLower() == "doughnut")
            {
                if (_parent._sum > 0) percentage = (YValue / _parent._sum * 100);
                else percentage = 0;
            }
            else if (_parent.RenderAs.ToLower() == "stackedcolumn100" || _parent.RenderAs.ToLower() == "stackedbar100" || _parent.RenderAs.ToLower() == "stackedarea100")
            {

                percentage = YValue / _parent._parent._stackSum[XValue].Y * 100;// _stackSum[XValue].Y Contains Absolute sum
            }
            return percentage;
        }



        private String GetAxisLabelString()
        {
            String labelString;
            if (_parent._parent.PlotDetails.AxisLabels.ContainsKey(XValue))
            {
                labelString = _parent._parent.PlotDetails.AxisLabels[XValue];
            }
            else
            {
                labelString = _parent._parent.AxisX.GetFormattedText(XValue);
            }
            return labelString;
        }
        #endregion Private Methods

        #region Public Properties
        /// <summary>
        /// The value that corresponds to Y-axis in a chart
        /// </summary>
        public Double YValue
        {
            get
            {
                switch (_parent.RenderAs.ToLower())
                {
                    case "stackedarea100":
                    case "stackedcolumn100":
                    case "stackedbar100":
                        // in case yvalue is not given this code returns 0
                        return (Double.IsNaN(_yValue)?0:_yValue);
                    case "pie":
                    case "doughnut":
                        // To return positive value always in case of pie and doughnut
                        return (Double.IsNaN(_yValue)?0:Math.Abs(_yValue));
                    default:
                        return _yValue;
                }
                
            }
            set
            {
                _yValue = value;
            }
        }

        /// <summary>
        /// This value is used only by Bubble chart. it controls the size of the bubble
        /// </summary>
        public Double ZValue
        {
            set;
            get;
        }

        public Double XValue
        {
            get
            {
                if (!Double.IsNaN(_xValue))
                    return _xValue;
                else
                    return Index + 1;
            }
            set
            {
                _xValue = value;
            }
        }

        public override bool Enabled
        {
            get;
            set;
        }

        public Double ExplodeOffset
        {
            get
            {
                return _explodeOffset / 2;
            }
            set
            {
                _explodeOffset = value;
            }
        }

        #region Marker Properties
        public String MarkerEnabled
        {
            get
            {
                if (_markerEnabled.ToString().ToLower() == "undefined")
                    return _parent.MarkerEnabled;
                else
                    return _markerEnabled;
            }
            set
            {
                _markerEnabled = value;
            }
        }
        public String MarkerStyle
        {
            get
            {
                if (_markerStyle != null)
                    return _markerStyle;
                else
                    return _parent.MarkerStyle;
            }
            set
            {
                _markerStyle = value;
            }
        }

        public Double MarkerBorderThickness
        {
            get
            {
                if (!Double.IsNaN(_markerBorderThickness))
                    return _markerBorderThickness;
                else
                    if (!Double.IsNaN(_parent.MarkerBorderThickness))
                    {
                        return _parent.MarkerBorderThickness;
                    }
                    else
                    {
                        return MarkerSize / 4;
                    }
            }
            set
            {
                _markerBorderThickness = value;
            }
        }

        public Double MarkerSize
        {
            get
            {
                if (!Double.IsNaN(_markerSize))
                    return _markerSize;
                else
                    if (!Double.IsNaN(_parent.MarkerSize))
                    {
                        return _parent.MarkerSize;
                    }
                    else
                    {
                        if (_parent.RenderAs.ToUpper() == "POINT")
                            return (_parent._parent.Width * _parent._parent.Height + 18000)/28000;
                        if (_parent.RenderAs.ToUpper() == "LINE")
                            return ((_parent._parent.Width * _parent._parent.Height + 12000) / 22000);
                        else
                            return 10;
                    }
            }
            set
            {
                _markerSize = value;
            }
        }

        public Double MarkerScale
        {
            get
            {
                if (!Double.IsNaN(_markerScale))
                    return _markerScale;
                else
                    if (!Double.IsNaN(_parent.MarkerScale))
                    {
                        return _parent.MarkerScale;
                    }
                    else
                    {
                        return 1;
                    }
            }
            set
            {
                _markerScale = value;
            }
        }

        internal Brush MarkerBackground
        {
            get
            {
                if (_markerBackground != null)
                    return _markerBackground;
                else
                    if (_parent.MarkerBackground != null)
                        return Cloner.CloneBrush(_parent.MarkerBackground);
                    else
                        return new SolidColorBrush(Colors.White);
            }
            set
            {
                _markerBackground = value;
            }

        }

        public String MarkerColor
        {
            set
            {
                MarkerBackground = Parser.ParseColor(value);
            }
        }
        public Brush MarkerBorderColor
        {
            get
            {
                if (_markerBorderColor != null)
                    return _markerBorderColor;
                else
                    if (_parent.MarkerBorderColor != null)
                        return Cloner.CloneBrush(_parent.MarkerBorderColor);
                    else
                        return Cloner.CloneBrush(Background);
            }
            set
            {
                _markerBorderColor = value;
            }
        }

        public String MarkerImage
        {
            get
            {
                if (_markerImage != null)
                    return _markerImage;
                else
                    return _parent.MarkerImage;
            }
            set
            {
                _markerImage = value;
            }
        }

        

        #endregion Marker Properties

        #region Label Properties

        public String LabelEnabled
        {
            get
            {
                if (_labelEnabled.ToString().ToLower() == "undefined")
                    return _parent.LabelEnabled;
                else
                    return _labelEnabled;
            }
            set
            {
                _labelEnabled = value;
                LabelLineEnabled = value;
            }
        }

        public String LabelText
        {
            get
            {
                if (!String.IsNullOrEmpty(_labelText))
                    return _labelText;
                else
                    return _parent.LabelText;
            }
            set
            {
                _labelText = value; ;
            }
        }

        public String LabelFontFamily
        {
            get
            {
                if (_labelFontFamily != null)
                    return _labelFontFamily;
                else
                    return _parent.LabelFontFamily;
            }
            set
            {
                _labelFontFamily = value;
            }
        }

        public Double LabelFontSize
        {
            get
            {
                if (!Double.IsNaN(_labelFontSize))
                    return _labelFontSize;
                else if (!Double.IsNaN(_parent.LabelFontSize))
                    return _parent.LabelFontSize;
                else
                    return CalculateFontSize();
            }
            set
            {
                _labelFontSize = value;
            }
        }

        public Brush LabelFontColor
        {
            get
            {
                if (_labelFontColor != null)
                    return _labelFontColor;
                else
                    return _parent.LabelFontColor;
            }
            set
            {
                _labelFontColor = value;
            }
        }

        public String LabelFontWeight
        {
            get
            {
                if (_labelFontWeight != null)
                    return _labelFontWeight.ToString();
                else
                    return _parent.LabelFontWeight;
            }
            set
            {
                _labelFontWeight = Converter.StringToFontWeight(value);
            }
        }

        public String LabelFontStyle
        {
            get
            {
                if (_labelFontStyle != null)
                    return _labelFontStyle;
                else
                    return _parent.LabelFontStyle;
            }
            set
            {
                _labelFontStyle = value;
            }
        }

        public Brush LabelBackground
        {
            get
            {
                if (_labelBackground != null)
                    return _labelBackground;
                else
                    return _parent.LabelBackground;
            }
            set
            {
                _labelBackground = value;
            }
        }

        internal Label Label
        {
            get
            {
                return _label;
            }
        }
        internal Marker Marker
        {
            get
            {
                return _markerRef;
            }
        }
        internal Polyline LabelLine
        {
            get
            {
                return _labelLine;
            }
        }

        public String LabelStyle
        {
            get
            {
                if (!String.IsNullOrEmpty(_labelStyle))
                    return _labelStyle;
                else
                    return _parent.LabelStyle;
            }
            set
            {
                _labelStyle = value;
            }
        }

        public String LabelLineEnabled
        {
            get
            {
                if (LabelEnabled.ToLower() == "true")
                {
                    if (_labelLineEnabled.ToLower() == "undefined")
                        return _parent.LabelLineEnabled;
                    else
                        return _labelLineEnabled;
                }
                return "false";
            }
            set
            {
                _labelLineEnabled = value;
            }
        }

        public Brush LabelLineColor
        {
            get
            {
                if (_labelLineColor == null)
                    return _parent.LabelLineColor;
                else
                    return _labelLineColor;
            }
            set
            {
                _labelLineColor = value;
            }
        }

        public Double LabelLineThickness
        {
            get
            {
                if (Double.IsNaN(_labelLineThickness) || _labelLineThickness == 0)
                    return _parent.LabelLineThickness;
                else
                    return _labelLineThickness;
            }
            set
            {
                _labelLineThickness = value;
            }

        }

        public String LabelLineStyle
        {
            get
            {
                if (!String.IsNullOrEmpty(_labelLineStyle))
                    return _labelLineStyle;
                else
                    return _parent.LabelLineStyle;
            }
            set
            {
                _labelLineStyle = value;
            }
        }

        #endregion Label Properties

        public override String ToolTipText
        {
            get
            {
                if (!String.IsNullOrEmpty(_toolTipText))
                    return _toolTipText;
                else
                    return _parent.ToolTipText;
            }
            set
            {
                _toolTipText = value;
            }

        }

        public override String Href
        {
            get
            {
                if (!String.IsNullOrEmpty(_href))
                    return _href;
                else
                    return _parent.Href;
            }
            set
            {
                _href = value;
            }
        }

        public override Brush Background
        {
            get
            {
                if (_background != null)
                    return _background;
                else if (_parent.Background != null)
                    return _parent.Background;
                else
                    return (Brush)GetFromTheme("Background");
            }
            set
            {
                this._background = value;
            }
        }

        public Boolean ShowInLegend
        {
            get
            {
                if (_showInLegend != "Undefined" && !String.IsNullOrEmpty(_showInLegend))
                    return Boolean.Parse(_showInLegend);
                else
                    return false;
            }
            set
            {
                _showInLegend = value.ToString();
            }
        }

        public String LegendText
        {
            get;
            set;
        }

        public String AxisLabel
        {
            get
            {
                return _axisLabel;
            }
            set
            {
                _axisLabel = value;
            }
        }

        #endregion Public Properties

        #region Internal Properties

        internal Int32 Index
        {
            get
            {
                return _index;
            }
            set
            {
                _index = value;
            }
        }

        #endregion Internal Properties

        #region Internal Methods
        internal void ApplyEffects(int ZIndex)
        {
            
            if (_parent.Bevel)
            {
                String renderType = _parent.RenderAs.ToLower();
                List<Point> bevelPoints = new List<Point>();
                switch (renderType)
                {
                    case "column":
                    case "stackedcolumn100":
                    case "stackedcolumn":

                        if (this.Height > 10)
                        {
                            bevelPoints.Add(new Point(0, 0));
                            bevelPoints.Add(new Point(Width, 0));
                            bevelPoints.Add(new Point(Width - 6, 6));
                            bevelPoints.Add(new Point(6, 6));
                            CreateAndAddBevelPath(bevelPoints, "Bright", 90, ZIndex);
                            bevelPoints.Clear();

                            bevelPoints.Add(new Point(0, 0));
                            bevelPoints.Add(new Point(5, 5));
                            bevelPoints.Add(new Point(5, Height - 5));
                            bevelPoints.Add(new Point(0, Height));
                            CreateAndAddBevelPath(bevelPoints, "Medium", -80, ZIndex);
                            bevelPoints.Clear();

                            bevelPoints.Add(new Point(Width, 0));
                            bevelPoints.Add(new Point(Width, Height));
                            bevelPoints.Add(new Point(Width - 5, Height - 5));
                            bevelPoints.Add(new Point(Width - 5, 5));
                            CreateAndAddBevelPath(bevelPoints, "Medium", -100, ZIndex);
                            bevelPoints.Clear();
                        }
                        break;
                    case "bar":
                    case "stackedbar100":
                    case "stackedbar":
                        
                        if (this.Width > 10)
                        {
                            bevelPoints.Add(new Point(0, 0));
                            bevelPoints.Add(new Point(Width, 0));
                            bevelPoints.Add(new Point(Width - 4, 4));
                            bevelPoints.Add(new Point(4, 4));
                            CreateAndAddBevelPath(bevelPoints, "Bright", 90, ZIndex);
                            bevelPoints.Clear();


                            bevelPoints.Add(new Point(0, 0));
                            bevelPoints.Add(new Point(5, 5));
                            bevelPoints.Add(new Point(5, Height - 5));
                            bevelPoints.Add(new Point(0, Height));
                            CreateAndAddBevelPath(bevelPoints, "Medium", -80, ZIndex);
                            bevelPoints.Clear();

                            bevelPoints.Add(new Point(Width, 0));
                            bevelPoints.Add(new Point(Width, Height));
                            bevelPoints.Add(new Point(Width - 5, Height - 5));
                            bevelPoints.Add(new Point(Width - 5, 5));
                            CreateAndAddBevelPath(bevelPoints, "Medium", -100, ZIndex);
                            bevelPoints.Clear();
                        }
                        break;
                    case "area":
                    case "stackedarea":
                    case "stackedarea100":
                        String[] type2 = { "Bright", "Medium", "Dark", "Medium" };
                        Double[] length2 = { 6, 6, 0, 6 };
                        Double[] Angle2 = { 90, 180, -90, 0 };
                        ApplyBevel(type2,length2,Angle2,ZIndex);
                        break;
                }
                
            }
        }
        #endregion Internal Methods

        #region Border Properties
        public override Double BorderThickness
        {
            get
            {
                if (!Double.IsNaN(_borderThickness))
                    return _borderThickness;
                else
                    return _parent.BorderThickness;
            }
            set
            {
                _borderThickness = value;
            }

        }

        public override Brush BorderColor
        {
            get
            {
                if (_borderColor == null)
                {
                    return _parent.BorderColor;
                }
                else
                {
                    return _borderColor;
                }
            }
            set
            {
                _borderColor = value;
            }
        }

        public override String BorderStyle
        {
            get
            {
                return _borderStyle;
            }

            set
            {
                _borderStyle = value;
            }
        }

        public override Double RadiusX
        {
            get
            {
                if (Double.IsNaN(_radiusX))
                {
                    return _parent.RadiusX;
                }
                else
                    return _radiusX;
            }
            set
            {
                _radiusX = value;
            }
        }

        public override Double RadiusY
        {
            get
            {
                if (Double.IsNaN(_radiusY))
                {
                    return _parent.RadiusY;
                }
                else
                    return _radiusY;
            }
            set
            {
                _radiusY = value;
            }
        }


        
        public override String Color
        {
            set
            {
                Background = Parser.ParseColor(value);
                _color = value;
            }
            get
            {
                return _color;
            }
        }
        public override String Image
        {
            get
            {
                return _image;
            }
            set
            {
                _image = value;
                ImageBrush imgBrush = new ImageBrush();
                Uri ur = new Uri(_image, UriKind.RelativeOrAbsolute);
                if (ur.IsAbsoluteUri)
                {
                    _image = ur.AbsoluteUri;
                }
                else
                {
                    UriBuilder ub = new UriBuilder(Application.Current.Host.Source);
                    String sourcePath = ub.Path.Substring(0, ub.Path.LastIndexOf('/') + 1);
                    UriBuilder ub2 = new UriBuilder(ub.Scheme, ub.Host, ub.Port, sourcePath + value);
                    _image = ub2.ToString();
                }
                String XAMLimage = "<ImageBrush xmlns=\"http://schemas.microsoft.com/client/2007\" ImageSource=\"" + _image + "\"/>";

                imgBrush = (ImageBrush)XamlReader.Load(XAMLimage);
                imgBrush.ImageFailed += new ExceptionRoutedEventHandler(imgBrush_ImageFailed);
                _background = imgBrush;
            }
        }
        public virtual Stretch ImageStretch
        {
            get
            {
                return _imageStretch;
            }
            set
            {
                _imageStretch = value;
                if (_background != null)
                {
                    if (_background.GetType().Name == "ImageBrush")
                    {
                        (_background as ImageBrush).Stretch = value;
                    }
                }
            }
        }
        #endregion

        #region Data
        private Double _xValue;
        private Double _yValue;
        private String _axisLabel;
        private Int32 _index;
        
        private String _color;
        private DataSeries _parent;

        public Line _line;
        public Line _lineShadow;
        public Polygon _polygon;

        private Double _borderThickness;
        public Rectangle _borderRectangle;
        public Rectangle _shadowRectangle;

        private Brush _borderColor;
        private String _borderStyle;

        private Double _explodeOffset;

        private Marker _markerRef;
        private Double _markerBorderThickness;
        private Double _markerSize;
        private Brush _markerBackground;
        private Brush _markerBorderColor;
        private String _markerImage;
        private String _markerStyle;
        private Double _markerScale;
        private String _markerEnabled;


        private String _labelEnabled;
        private String _labelText;
        private Double _labelFontSize;
        private Brush _labelFontColor;
        private Brush _labelBackground;
        private FontWeight _labelFontWeight;
        private String _labelFontFamily;
        private String _labelStyle;
        private Brush _labelLineColor;
        private Double _labelLineThickness;
        private String _labelLineStyle;
        private String _labelLineEnabled;
        private String _labelFontStyle;

        private Label _label;
        private Polyline _labelLine;

        private String _toolTipText;


        private Brush _background;
        private String _href;

        internal System.Collections.Generic.List<Point> points = new System.Collections.Generic.List<Point>();
        private Double _radiusX;
        private Double _radiusY;

        private String _showInLegend;
        private String _image;
        private Stretch _imageStretch;
        #endregion Data
    }
}