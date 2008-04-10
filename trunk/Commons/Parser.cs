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
using System.Collections.Generic;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Markup;
using System.Globalization;

namespace Visifire.Commons
{
    public class Parser
    {
        #region Static Methods
        /// <summary>
        /// This converts a given string of angle;color,stop;.... string to a linear gradient brush
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Brush ParseLinearGradient(String str)
        {
            Double angle;
            LinearGradientBrush brush = new LinearGradientBrush();
            TransformGroup tg = new TransformGroup();
            RotateTransform rt = new RotateTransform();

            String[] strSplit = str.Split(';');
            angle = Double.Parse(strSplit[0],CultureInfo.InvariantCulture);

            brush.StartPoint = new Point(0, 0);
            brush.EndPoint = new Point(1, 0);

            rt.Angle = angle;
            rt.CenterX = .5;
            rt.CenterY = .5;

            tg.Children.Add(rt);
            brush.RelativeTransform = tg;

            foreach (String colorOffset in strSplit)
            {
                String[] colorOffsetSplit = colorOffset.Split(',');
                
                if (colorOffsetSplit.Length > 1)
                {
                    GradientStop gs = (GradientStop)XamlReader.Load(@"<GradientStop xmlns=""http://schemas.microsoft.com/client/2007"" Color=""" + colorOffsetSplit[0] + @""" Offset=""" + colorOffsetSplit[1] + @"""/>");
                    brush.GradientStops.Add(gs);
                }
            }
            return brush;
        }

        /// <summary>
        /// This converts a given string of X;Y;color,stop;.... string to a radial gradient brush
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Brush ParseRadialGradient(String str)
        {
            
            RadialGradientBrush brush = new RadialGradientBrush();
            
            String[] strSplit = str.Split(';');


            brush.GradientOrigin = new Point(Double.Parse(strSplit[0], CultureInfo.InvariantCulture), Double.Parse(strSplit[1], CultureInfo.InvariantCulture));


            foreach (String colorOffset in strSplit)
            {
                String[] colorOffsetSplit = colorOffset.Split(',');

                if (colorOffsetSplit.Length > 1)
                {
                    GradientStop gs = (GradientStop)XamlReader.Load(@"<GradientStop xmlns=""http://schemas.microsoft.com/client/2007"" Color=""" + colorOffsetSplit[0] + @""" Offset=""" + colorOffsetSplit[1] + @"""/>");
                    brush.GradientStops.Add(gs);
                }
            }

            return brush;
        }

        /// <summary>
        /// Converts a color is string form to Solid Color Brush
        /// </summary>
        /// <param name="colorCode"></param>
        /// <returns></returns>
        public static Brush ParseSolidColor(String colorCode)
        {
            return (SolidColorBrush)XamlReader.Load(String.Format(CultureInfo.InvariantCulture, @"<SolidColorBrush xmlns=""http://schemas.microsoft.com/client/2007"" Color=""{0}""></SolidColorBrush>",colorCode));
        }

        /// <summary>
        /// Returns a darker shade of the color by decreasing the brightness by the given intensity value
        /// </summary>
        /// <param name="color"></param>
        /// <param name="intensity"></param>
        /// <returns></returns>
        public static System.Windows.Media.Color GetDarkerColor(System.Windows.Media.Color color, Double intensity)
        {
            Color darkerShade = new Color();
            intensity = (intensity < 0 || intensity > 1) ? 1 : intensity;
            darkerShade.R = (Byte)(color.R * intensity);
            darkerShade.G = (Byte)(color.G * intensity);
            darkerShade.B = (Byte)(color.B * intensity);
            darkerShade.A = 255;
            return darkerShade;
        }
        public static System.Windows.Media.Color GetDarkerColor(System.Windows.Media.Color color, Double intensityR, Double intensityG, Double intensityB)
        {
            Color darkerShade = new Color();
            intensityR = (intensityR < 0 || intensityR > 1) ? 1 : intensityR;
            intensityG = (intensityG < 0 || intensityG > 1) ? 1 : intensityG;
            intensityB = (intensityB < 0 || intensityB > 1) ? 1 : intensityB;
            darkerShade.R = (Byte)(color.R * intensityR);
            darkerShade.G = (Byte)(color.G * intensityG);
            darkerShade.B = (Byte)(color.B * intensityB);
            darkerShade.A = 255;
            return darkerShade;
        }
        /// <summary>
        /// Returns a lighter shade of the color by increasing the brightness by the given intensity value
        /// </summary>
        /// <param name="color"></param>
        /// <param name="intensity"></param>
        /// <returns></returns>
        public static System.Windows.Media.Color GetLighterColor(System.Windows.Media.Color color, Double intensity)
        {
            Color lighterShade = new Color();
            intensity = (intensity < 0 || intensity > 1) ? 1 : intensity;
            lighterShade.R = (Byte)(256 - ((256 - color.R) * intensity));
            lighterShade.G = (Byte)(256 - ((256 - color.G) * intensity));
            lighterShade.B = (Byte)(256 - ((256 - color.B) * intensity));
            lighterShade.A = 255;
            return lighterShade;
        }
        public static System.Windows.Media.Color GetLighterColor(System.Windows.Media.Color color, Double intensityR, Double intensityG, Double intensityB)
        {
            Color lighterShade = new Color();
            intensityR = (intensityR < 0 || intensityR > 1) ? 1 : intensityR;
            intensityG = (intensityG < 0 || intensityG > 1) ? 1 : intensityG;
            intensityB = (intensityB < 0 || intensityB > 1) ? 1 : intensityB;
            lighterShade.R = (Byte)(256 - ((256 - color.R) * intensityR));
            lighterShade.G = (Byte)(256 - ((256 - color.G) * intensityG));
            lighterShade.B = (Byte)(256 - ((256 - color.B) * intensityB));
            lighterShade.A = 255;
            return lighterShade;
        }

        public static void GenerateDarkerGradientBrush(GradientBrush source, GradientBrush result, Double intensity)
        {
            foreach (GradientStop grad in source.GradientStops)
            {
                GradientStop gs = new GradientStop();
                gs.Color = Parser.GetDarkerColor(grad.Color, intensity);
                gs.Offset = grad.Offset;
                result.GradientStops.Add(gs);
            }
        }

        public static void GenerateLighterGradientBrush(GradientBrush source, GradientBrush result, Double intensity)
        {
            foreach (GradientStop grad in source.GradientStops)
            {
                GradientStop gs = new GradientStop();
                gs.Color = Parser.GetLighterColor(grad.Color, intensity);
                gs.Offset = grad.Offset;
                result.GradientStops.Add(gs);
            }
        }

        public static System.Windows.Media.Color InvertColor(System.Windows.Media.Color color)
        {
            Color newColor = new Color();
            newColor.A = 255;
            newColor.R = (Byte)(255 - color.R);
            newColor.G = (Byte)(255 - color.G);
            newColor.B = (Byte)(255 - color.B);
            return newColor;
        }

        public static Brush InvertBrushColors(Brush brush)
        {
            Brush newBrush;
            if (brush.GetType().Name == "SolidColorBrush")
            {
                newBrush = Parser.ParseSolidColor(InvertColor((brush as SolidColorBrush).Color).ToString());
            }
            else if (brush.GetType().Name == "LinearGradientBrush" || brush.GetType().Name == "RadialGradientBrush")
            {
                if (brush.GetType().Name == "LinearGradientBrush") newBrush = new LinearGradientBrush();
                else newBrush = new RadialGradientBrush();

                foreach (GradientStop grad in (brush as GradientBrush).GradientStops)
                {
                    GradientStop gs = new GradientStop();
                    gs.Color = InvertColor(grad.Color);
                    gs.Offset = grad.Offset;
                    (newBrush as GradientBrush).GradientStops.Add(gs);
                }
            }
            else
            {
                newBrush = new SolidColorBrush(Colors.Gray);
            }
            return newBrush;
        }

        public static Double GetBrushIntensity(Brush brush)
        {
            Color color = new Color();
            Double intensity = 0;
            if (brush == null) return 1;
            if (brush.GetType().Name == "SolidColorBrush")
            {
                color = (brush as SolidColorBrush).Color;
                intensity = (Double)(color.R + color.G + color.B) / (3 * 255);
                
            }
            else if (brush.GetType().Name == "LinearGradientBrush" || brush.GetType().Name == "RadialGradientBrush")
            {

                foreach (GradientStop grad in (brush as GradientBrush).GradientStops)
                {
                    color = grad.Color;
                    intensity += (Double)(color.R + color.G + color.B) / (3 * 255);
                }
                intensity /= (brush as GradientBrush).GradientStops.Count;
            }
            else
            {
                intensity = 0;
            }
            return intensity;
        }

        public static Brush ParseColor(String colorString)
        {
            String[] splitStr = colorString.Split(';');
            if (splitStr.Length == 1)
                return ParseSolidColor(splitStr[0]);
            else
            {
                String[] str0 = splitStr[0].Split(',');
                String[] str1 = splitStr[1].Split(',');

                if (str0.Length == 1 && str1.Length == 1)
                    return ParseRadialGradient(colorString);
                else
                    return ParseLinearGradient(colorString);
            }
        }
        #endregion Static Methods

    }


    public class PointMath
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
            return Math.Sqrt(Math.Pow(point1.X-point2.X,2) + Math.Pow(point1.Y-point2.Y,2));
        }

        /// <summary>
        /// Calculate Centroid from given point collection
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public static Point Centroid(List<Point> points)
        {
            Double Area = 0;
            Double X = 0, Y = 0;
            int i = 0;

            //Calculate area of the polygon
            for (i = 0; i < points.Count - 1; i++)
            {
                Area += (points[i].X*points[i+1].Y -points[i].Y*points[i+1].X);
            }
            Area /= 2;

            //Calculate centroid X
            for (i = 0; i < points.Count - 1; i++)
            {
                X += ((points[i].X + points[i + 1].X) * (points[i].X * points[i + 1].Y - points[i].Y * points[i + 1].X));
            }
            X /= (6 * Area);
            //Calculate Centroid Y
            for (i = 0; i < points.Count - 1; i++)
            {
                Y += ((points[i].Y + points[i + 1].Y) * (points[i].X * points[i + 1].Y - points[i].Y * points[i + 1].X));
            }
            Y /= (6 * Area);
            return new Point(X, Y);
            
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
        public static Point PointFromLength(Double reduceAmt, Point point1, Point point2)
        {
            Double length = DistanceFormulae(point1, point2);
            Double l = Math.Abs(length - reduceAmt);

            Double a, b, c;
            Point temp=SlopeIntercept(point1,point2); //to store slope and intercept

            a = 1 + Math.Pow(temp.X,2);

            b= 2*(temp.X*temp.Y - point1.X - point1.Y*temp.X);

            c = Math.Pow(point1.X, 2) + Math.Pow(point1.Y, 2) + Math.Pow(temp.Y, 2) - 2 * point1.Y * temp.Y - l * l;

            Double x1, x2,d;
            d = b*b-4*a*c;
            if (d < 0)
            {
                return point1;
            }
            else
            {
                x1 = (-b + Math.Sqrt(d))/(2*a);
                x2 = (-b - Math.Sqrt(d)) / (2 * a);
            }

            Double X,Y;
            if (Math.Abs(point2.X - x1) < Math.Abs(point2.X - x2))
                X = x1;
            else
                X = x2;

            Y = temp.X * X + temp.Y;

            return new Point(X, Y);
        }

        public static Boolean DoubleLT(Double a,Double b)
        {
            return a < b;
        }
        public static Boolean DoubleGT(Double a, Double b)
        {
            return a > b;
        }
        public static Boolean DoubleEQ(Double a, Double b)
        {
            return Math.Abs(a - b) < Double.Epsilon;
        }
        public static Boolean DoubleGE(Double a, Double b)
        {
            return DoubleGT(a, b) || DoubleEQ(a, b);
        }
        public static Boolean DoubleLE(Double a, Double b)
        {
            return DoubleLT(a, b) || DoubleEQ(a, b);
        }
        #endregion Static Methods
    }
}
