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
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using Visifire.Commons;

namespace Visifire.Charts
{
    /// <summary>
    /// Contains plotting details about the data required for circular chart
    /// </summary>
    internal class CircularPlotDetails
    {
        /// <summary>
        /// Initializes a new instance of the Visifire.Charts.PlotDetails class
        /// </summary>
        public CircularPlotDetails(ChartOrientationType chartOrientation, RenderAs renderAs)
        {
            ChartOrientation = chartOrientation;
            CircularChartType = renderAs;
            ListOfPoints4CircularAxis = new List<Point>();

        }

        /// <summary>
        /// Radius of Circular chart
        /// </summary>
        public Double Radius
        {
            get;
            set;
        }

        /// <summary>
        /// Center of Circular chart
        /// </summary>
        public Point Center
        {
            get;
            set;
        }

        /// <summary>
        /// List of initial points for circular axis
        /// </summary>
        public List<Point> ListOfPoints4CircularAxis
        {
            get;
            set;
        }

        /// <summary>
        /// List of angles for all spikes (in radian)
        /// </summary>
        public List<Double> AnglesInRadian
        {
            get;
            set;
        }

        /// <summary>
        /// List of angles for all spikes (in degree)
        /// </summary>
        public List<Double> AnglesInDegree
        {
            get;
            set;
        }

        /// <summary>
        /// Minimum angle for circular chart in degree
        /// </summary>
        public Double MinAngleInDegree
        {
            get;
            set;
        }

        /// <summary>
        /// Oriantation for chart
        /// </summary>
        public ChartOrientationType ChartOrientation
        {
            get;
            set;
        }

        public RenderAs CircularChartType
        {
            get;
            set;
        }

        /// <summary>
        /// Update circular plotdetails
        /// </summary>
        /// <param name="circularLabels"></param>
        /// <param name="radius"></param>
        internal void UpdateCircularPlotDetails(List<CircularAxisLabel> circularLabels, Double radius)
        {
            ListOfPoints4CircularAxis.Clear();
            foreach (CircularAxisLabel label in circularLabels)
            {
                ListOfPoints4CircularAxis.Add(label.Position);
            }

            Radius = radius;
        }

        /// <summary>
        /// Calculate AxisX points for Circular chart
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="axisX"></param>
        /// <param name="maxDataPointsCount"></param>
        internal void CalculateAxisXLabelsPoints4Polar(Double width, Double height, Boolean isAxisLabelsEnabled, List<Double> angles, Double minValue, Double maxValue)
        {
            Double radius = Math.Min(width, height) / 2;

            Double reducedPercent = 10;
            if (isAxisLabelsEnabled)
                reducedPercent = 20;

            Radius = radius - (radius * reducedPercent / 100);

            Center = new Point(width / 2, height / 2);

            AnglesInRadian = new List<Double>();

            Double minValInRadian;

            if (minValue != 0)
                minValInRadian = AxisLabel.GetRadians(minValue - 90);
            else
                minValInRadian = 2 * Math.PI - Math.PI / 2;

            Double minAngle = Graphics.ValueToPixelPosition(minValInRadian, 2 * Math.PI + minValInRadian, AxisLabel.GetRadians(minValue), AxisLabel.GetRadians(maxValue), AxisLabel.GetRadians(minValue));
            MinAngleInDegree = minAngle * 180 / Math.PI;

            for (Int32 i = 0; i < angles.Count; i++)
            {
                Double actualAngle = Graphics.ValueToPixelPosition(minValInRadian, 2 * Math.PI + minValInRadian, AxisLabel.GetRadians(minValue), AxisLabel.GetRadians(maxValue), AxisLabel.GetRadians(angles[i]));

                Double x = Radius * Math.Cos(actualAngle) + Center.X;
                Double y = Radius * Math.Sin(actualAngle) + Center.Y;


                ListOfPoints4CircularAxis.Add(new Point(x, y));
                AnglesInRadian.Add(actualAngle);
            }
        }

        /// <summary>
        /// Calculate AxisX points for Circular chart
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="axisX"></param>
        /// <param name="maxDataPointsCount"></param>
        internal void CalculateAxisXLabelsPoints4Radar(Double width, Double height, Boolean isAxisLabelsEnabled, Int32 maxDataPointsCount)
        {
            Int32 noOfSpikes = maxDataPointsCount;
            Double startAngle = -Math.PI / 2;

            Double actualAngle = startAngle;

            Int32 nextIteration = 1;

            Double minAngle = 0;

            Double radius = Math.Min(width, height) / 2;

            Double reducedPercent = 10;
            if (isAxisLabelsEnabled)
                reducedPercent = 20;

            Radius = radius - (radius * reducedPercent / 100);

            Center = new Point(width / 2, height / 2);

            //ListOfPoints4CircularAxis = new List<Point>();
            AnglesInRadian = new List<Double>();

            MinAngleInDegree = 360.0 / noOfSpikes;

            for (Int32 i = 0; i < noOfSpikes; i++)
            {
                Double x = Radius * Math.Cos(actualAngle) + Center.X;
                Double y = Radius * Math.Sin(actualAngle) + Center.Y;

                ListOfPoints4CircularAxis.Add(new Point(x, y));
                AnglesInRadian.Add(actualAngle);

                minAngle = MinAngleInDegree * nextIteration++;
                actualAngle = AxisLabel.GetRadians(minAngle) - (Math.PI / 2);
            }
        }
    }
}
