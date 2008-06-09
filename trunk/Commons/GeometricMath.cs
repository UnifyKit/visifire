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
using System.Collections.Generic;
using System.Globalization;

namespace Visifire.Commons
{
    public class GeometricMath
    {
        #region Static Methods
        /// <summary>
        /// calculates distance between two points
        /// </summary>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        /// <returns></returns>
        public static Double DistanceFormulae(Point point1, Point point2)
        {
            return Math.Sqrt(Math.Pow(point1.X - point2.X, 2) + Math.Pow(point1.Y - point2.Y, 2));
        }

        /// <summary>
        /// Calculate Centroid from given point collection
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public static Point Centroid(List<Point> points)
        {
            Double area = 0;
            Double x = 0, y = 0;
            Int32 i = 0;

            //Calculate area of the polygon
            for (i = 0; i < points.Count - 1; i++)
            {
                area += (points[i].X * points[i + 1].Y - points[i].Y * points[i + 1].X);
            }
            area /= 2;

            //Calculate centroid X
            for (i = 0; i < points.Count - 1; i++)
            {
                x += ((points[i].X + points[i + 1].X) * (points[i].X * points[i + 1].Y - points[i].Y * points[i + 1].X));
            }
            x /= (6 * area);
            //Calculate Centroid Y
            for (i = 0; i < points.Count - 1; i++)
            {
                y += ((points[i].Y + points[i + 1].Y) * (points[i].X * points[i + 1].Y - points[i].Y * points[i + 1].X));
            }
            y /= (6 * area);
            return new Point(x, y);

        }

        /// <summary>
        /// Calculates the slope of the line joining the two points
        /// </summary>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        /// <returns></returns>
        public static Double LineSlope(Point point1, Point point2)
        {
            return (point2.Y - point1.Y) / (point2.X - point1.X);
        }

        /// <summary>
        /// Calculates intercept of a line joining the two points
        /// </summary>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        /// <returns></returns>
        public static Double LineIntercept(Point point1, Point point2)
        {
            return point1.Y - LineSlope(point1, point2) * point1.X;
        }

        /// <summary>
        /// Calculates the slope and intercept and returns X=slope and Y=intercept
        /// </summary>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        /// <returns></returns>
        public static Point SlopeIntercept(Point point1, Point point2)
        {
            Double slope = LineSlope(point1, point2);
            Double intercept = LineIntercept(point1, point2);
            return new Point(slope, intercept);
        }

        /// <summary>
        /// Gets a point which is reduced by a given value in distance from point1 and closest to point2
        /// </summary>
        /// <param name="reduceAmt"></param>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        /// <returns></returns>
        public static Point PointFromLength(Double reduceValue, Point point1, Point point2)
        {
            Double length = DistanceFormulae(point1, point2);
            Double l = Math.Abs(length - reduceValue);

            Double a, b, c;
            Point temp = SlopeIntercept(point1, point2); //to store slope and intercept

            a = 1 + Math.Pow(temp.X, 2);

            b = 2 * (temp.X * temp.Y - point1.X - point1.Y * temp.X);

            c = Math.Pow(point1.X, 2) + Math.Pow(point1.Y, 2) + Math.Pow(temp.Y, 2) - 2 * point1.Y * temp.Y - l * l;

            Double x1, x2, d;
            d = b * b - 4 * a * c;
            if (d < 0)
            {
                return point1;
            }
            else
            {
                x1 = (-b + Math.Sqrt(d)) / (2 * a);
                x2 = (-b - Math.Sqrt(d)) / (2 * a);
            }

            Double X, Y;
            if (Math.Abs(point2.X - x1) < Math.Abs(point2.X - x2))
                X = x1;
            else
                X = x2;

            Y = temp.X * X + temp.Y;

            return new Point(X, Y);
        }

        public static Double Approximate(Double value)
        {
            Double n = value;
            Double s = (n < 0) ? -1 : 1;
            Double k = Math.Abs(n);
            Double r = Math.Round(k * 100);
            Double b = Math.Floor(r / 5);
            Double t = Double.Parse((r - b * 5).ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture);
            Double m = (t > 2) ? b * 5 + 5 : b * 5;
            return s * (m / 100);
        }

        #endregion Static Methods
    }
}
