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
    public class PlotArea : PlotAreaBase
    {
        #region Public Methods

        public PlotArea()
        {
            
        }

        public override System.Collections.Generic.List<Point> GetBoundingPoints()
        {
            System.Collections.Generic.List<Point> points = new System.Collections.Generic.List<Point>();
            points.Add(new Point(0, 0));
            points.Add(new Point(this.Width, 0));
            points.Add(new Point(this.Width, this.Height));
            points.Add(new Point(0, this.Height));
            return points;
        }

        public override void Init()
        {
            base.Init();
            ValidateParent();

            SetName();

            //This step applies theme background
            if(GetFromTheme("Background") != null)
                Background = GetFromTheme("Background") as Brush;

            AttachToolTip();
            AttachHref();

            if (BorderColor == null)
            {

                if (_parent.Background == null && this.Background == null)
                {
                    BorderColor = new SolidColorBrush(Colors.Black);
                }
                else if (Background == null)
                {
                    if (Parser.GetBrushIntensity(_parent.Background) > 0.5)
                    {
                        BorderColor = new SolidColorBrush(Colors.Black);
                    }
                    else
                    {
                        BorderColor = new SolidColorBrush(Colors.LightGray);
                    }
                }
                else
                {
                    if (Parser.GetBrushIntensity(Background) > 0.5)
                    {
                        BorderColor = new SolidColorBrush(Colors.Black);
                    }
                    else
                    {
                        BorderColor = new SolidColorBrush(Colors.LightGray);
                    }
                }
            }
        }

        /// <summary>
        /// Set the width of the PlotArea to the chart width.
        /// Fire an event whenever the width changes so that other dependent objects can adjust their positoin.
        /// </summary>
        public override void SetWidth()
        {

            if (_parent.PlotDetails.AxisOrientation == AxisOrientation.Bar)
            {

                //Use the width calculated from the inner bounds
                Double tempWidth;
                tempWidth = _parent._innerBounds.Right - (Double)GetValue(LeftProperty);
                Double spaceToRight = 0;

                spaceToRight = _parent.Width - _parent._innerBounds.Right;

                if (_parent.LabelPaddingRight > spaceToRight)
                    tempWidth = tempWidth - (_parent.LabelPaddingRight - spaceToRight);

                Width = tempWidth - _parent.Padding;
            }
            else if (_parent.PlotDetails.AxisOrientation == AxisOrientation.Column)
            {
                Double spaceToRight = 0; // Space remaining to the right of the PlotArea


                Width = _parent._innerBounds.Right - (Double)GetValue(LeftProperty) - _parent.Padding;

                spaceToRight = (Double)_parent.GetValue(WidthProperty) -
                                    ((Double)this.GetValue(LeftProperty) +
                                    (Double)this.GetValue(WidthProperty) + _parent.Padding);

                if (_parent.LabelPaddingRight > spaceToRight)
                {
                    this.SetValue(WidthProperty, _parent.Width - _parent.Padding - (Double)this.GetValue(LeftProperty) - _parent.LabelPaddingRight);

                }


            }
            else if (_parent.PlotDetails.AxisOrientation == AxisOrientation.Pie)
            {

                Width = _parent._innerBounds.Width;
            }


        }

        /// <summary>
        /// Sets the height of the plot area
        /// </summary>
        public override void SetHeight()
        {
            if (_parent.PlotDetails.AxisOrientation == AxisOrientation.Bar)
            {
                SetValue(HeightProperty, (Double)_parent.AxisY.GetValue(TopProperty) - (Double)this.GetValue(TopProperty));
            }
            else if (_parent.PlotDetails.AxisOrientation == AxisOrientation.Column)
            {
                SetValue(HeightProperty, (Double)_parent.AxisX.GetValue(TopProperty) - (Double)this.GetValue(TopProperty));
            }


            Double bottomLimit = _parent._innerBounds.Bottom;

            if (_parent.PlotDetails.AxisOrientation == AxisOrientation.Pie)
            {

                SetValue(HeightProperty, bottomLimit - (Double)GetValue(TopProperty) - _parent.Padding);
            }
            else if (_parent.LabelPaddingBottom > (bottomLimit - (this.Height + (Double)this.GetValue(TopProperty))))
                SetValue(HeightProperty, bottomLimit - _parent.LabelPaddingBottom - (Double)GetValue(TopProperty));
        }

        public override void SetLeft()
        {
            if (_parent.PlotDetails.AxisOrientation == AxisOrientation.Bar)
            {
                SetValue(LeftProperty, (Double)_parent.AxisX.GetValue(LeftProperty) + (Double)_parent.AxisX.GetValue(WidthProperty));
            }
            else if (_parent.PlotDetails.AxisOrientation == AxisOrientation.Column)
            {
                SetValue(LeftProperty, (Double)_parent.AxisY.GetValue(LeftProperty) + (Double)_parent.AxisY.GetValue(WidthProperty));


            }
            else if (_parent.PlotDetails.AxisOrientation == AxisOrientation.Pie)
            {

                this.SetValue(LeftProperty, _parent._innerBounds.Left);

            }
        }

        public override void SetTop()
        {

            SetValue(TopProperty, _parent._innerBounds.Top + _parent.Padding);
            if (_parent.LabelPaddingTop > (Double)this.GetValue(TopProperty))
                this.SetValue(TopProperty, _parent.LabelPaddingTop + _parent.Padding);
        }

        public override void Render()
        {
            if (_parent.View3D && _parent.PlotDetails.AxisOrientation != AxisOrientation.Pie)
            {
                if (_parent.PlotDetails.AxisOrientation == AxisOrientation.Column)
                {
                    System.Collections.Generic.List<Point> effect = new System.Collections.Generic.List<Point>();
                    effect.Add(new Point(Width, 0));
                    effect.Add(new Point(Width + 3, 3));
                    effect.Add(new Point(Width + 3, Height - 3));
                    effect.Add(new Point(Width, Height));
                    Polygon plot3d = new Polygon();
                    if (Background != null)
                    {
                        if (Background.GetType().Name == "SolidColorBrush")
                        {
                            String fillString = "0;";
                            fillString += Parser.GetDarkerColor((Background as SolidColorBrush).Color, 0.61) + ",0;";
                            fillString += Parser.GetDarkerColor((Background as SolidColorBrush).Color, 0.95) + ",1";
                            plot3d.Fill = Parser.ParseLinearGradient(fillString);
                        }
                        else if (Background.GetType().Name == "LinearGradientBrush" || Background.GetType().Name == "RadialGradientBrush")
                        {
                            plot3d.Fill = Cloner.CloneBrush(Background);
                            (plot3d.Fill as GradientBrush).GradientStops.Clear();
                            Parser.GenerateDarkerGradientBrush(Background as GradientBrush, plot3d.Fill as GradientBrush, 0.75);
                        }
                    }
                    else
                    {
                        plot3d.Fill = Parser.ParseColor("0;#7f000000,0;#33000000,1");
                    }


                    plot3d.Points = Converter.ArrayToCollection(effect.ToArray());

                    plot3d.Opacity = 1;
                    plot3d.SetValue(ZIndexProperty, 60);
                    plot3d.SetValue(LeftProperty, GetValue(LeftProperty));
                    plot3d.SetValue(TopProperty, GetValue(TopProperty));
                    _parent.Children.Add(plot3d);
                    ApplyClipRegion = false;
                    RectangleGeometry rg = new RectangleGeometry();
                    rg.Rect = new Rect(0, 0, Width + 3, Height);
                    rg.RadiusX = RadiusX + BorderThickness / 2;
                    rg.RadiusY = RadiusY + BorderThickness / 2;
                    this.Clip = rg;
                }
                else
                {
                    System.Collections.Generic.List<Point> effect = new System.Collections.Generic.List<Point>();
                    effect.Add(new Point(0, 0));
                    effect.Add(new Point(Width , 0));
                    effect.Add(new Point(Width - 3, - 3));
                    effect.Add(new Point(3, -3));
                    Polygon plot3d = new Polygon();
                    if (Background != null)
                    {
                        if (Background.GetType().Name == "SolidColorBrush")
                        {
                            String fillString = "180;";
                            fillString += Parser.GetDarkerColor((Background as SolidColorBrush).Color, 0.61) + ",0;";
                            fillString += Parser.GetDarkerColor((Background as SolidColorBrush).Color, 0.95) + ",1";
                            plot3d.Fill = Parser.ParseLinearGradient(fillString);
                        }
                        else if (Background.GetType().Name == "LinearGradientBrush" || Background.GetType().Name == "RadialGradientBrush")
                        {
                            plot3d.Fill = Cloner.CloneBrush(Background);
                            (plot3d.Fill as GradientBrush).GradientStops.Clear();
                            Parser.GenerateDarkerGradientBrush(Background as GradientBrush, plot3d.Fill as GradientBrush, 0.75);
                        }
                    }
                    else
                    {
                        plot3d.Fill = Parser.ParseColor("-45;#7f000000,0;#33000000,1");
                    }


                    plot3d.Points = Converter.ArrayToCollection(effect.ToArray());

                    plot3d.Opacity = 1;
                    plot3d.SetValue(ZIndexProperty, 60);
                    plot3d.SetValue(LeftProperty, GetValue(LeftProperty));
                    plot3d.SetValue(TopProperty, GetValue(TopProperty));
                    _parent.Children.Add(plot3d);
                    ApplyClipRegion = false;
                    RectangleGeometry rg = new RectangleGeometry();
                    rg.Rect = new Rect(0, -3, Width, Height+3);
                    rg.RadiusX = RadiusX + BorderThickness / 2;
                    rg.RadiusY = RadiusY + BorderThickness / 2;
                    this.Clip = rg;
                }
            }

            ApplyBorder();


        }
        #endregion Public Methods

        #region Internal Methods

        internal void ApplyEffects()
        {


            if (ShadowEnabled)
            {
                if(Background != null)
                    ApplyShadow();
            }
            if (LightingEnabled)
            {
                if(Background != null)
                    ApplyLighting();
            }
            if (Bevel)
            {

                String[] type = { "Bright", "Medium", "Dark", "Medium" };
                Double[] length = { 13, 10, 13, 10 };
                Double[] Angle = { 90, 180, -90, 0 };
                ApplyBevel(type, length, Angle);
            }


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
        /// Parent element should be a Chart element. Else throw an exception.
        /// </summary>
        private void ValidateParent()
        {
            
            if (this.Parent.GetType().Name == "Chart")
                _parent = this.Parent as Chart;
            else
                throw new Exception(this + "Parent should be a Chart");
        }

        

        #endregion Private Methods

        #region Data
        private Chart _parent;

        #endregion Data

    }
}
