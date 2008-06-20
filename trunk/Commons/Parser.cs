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
using System.Net;
using System.IO;

namespace Visifire.Commons
{
    public class Parser
    {
        #region Static Methods
        /// <summary>
        /// This converts a given String of angle;color,stop;.... String to a linear gradient brush
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Brush ParseLinearGradient(String str)
        {
            Double angle;
            
            String[] strSplit = str.Split(';');
            angle = Double.Parse(strSplit[0],CultureInfo.InvariantCulture);

            //rt.Angle = angle;
            //rt.CenterX = .5;
            //rt.CenterY = .5;

            //tg.Children.Add(rt);
            //brush.RelativeTransform = tg;

            GradientStopCollection gsc = new GradientStopCollection();
            foreach (String colorOffset in strSplit)
            {
                String[] colorOffsetSplit = colorOffset.Split(',');
                
                if (colorOffsetSplit.Length > 1)
                {
                    GradientStop gs = (GradientStop)XamlReader.Load(@"<GradientStop xmlns=""http://schemas.microsoft.com/client/2007"" Color=""" + colorOffsetSplit[0] + @""" Offset=""" + colorOffsetSplit[1] + @"""/>");
                    gsc.Add(gs);
                    
                }
            }

            LinearGradientBrush brush = new LinearGradientBrush();

            brush.GradientStops = gsc;
            brush.StartPoint = new Point(0, 0);
            brush.EndPoint = new Point(1, 0);

            // Rotate brush
            TransformGroup tg = new TransformGroup();
            RotateTransform rt = new RotateTransform();
            rt.Angle = angle;
            rt.CenterX = .5;
            rt.CenterY = .5;

            tg.Children.Add(rt);
            brush.RelativeTransform = tg;

            return brush;
        }

        /// <summary>
        /// This converts a given String of color,stop;.... String to a linear gradient brush
        /// </summary>
        /// <param name="str">color1,stop1;color2,stop2;.... String </param>
        /// <param name="start">gradient start point</param>
        /// <param name="end">gradient end point</param>
        /// <returns></returns>
        public static Brush ParseLinearGradient(String str, Point start, Point end)
        {
            LinearGradientBrush linearGradient = new LinearGradientBrush();

            linearGradient.StartPoint = start;
            linearGradient.EndPoint = end;

            String[] colorStopSet = str.Split(';');

            for (Int32 i = 0; i < colorStopSet.Length; i++)
            {
                String[] colorStop = colorStopSet[i].Split(',');
                if (colorStop.Length > 1)
                {
                    GradientStop stops = (GradientStop)XamlReader.Load(@"<GradientStop xmlns=""http://schemas.microsoft.com/client/2007"" Color=""" + colorStop[0] + @""" Offset=""" + colorStop[1] + @"""/>");
                    linearGradient.GradientStops.Add(stops);
                }
            }
            return linearGradient;
        }

        /// <summary>
        /// This converts a given String of color,intensity,stop;.... String to a linear gradient brush
        /// </summary>
        /// <param name="str">ShadeType1,intensity1,stop1;ShadeType2,intensity2,stop2;.... String</param>
        /// <param name="color">color to be used for generating gradient</param>
        /// <param name="angle">transform angle</param>
        /// <returns></returns>
        public static Brush ParseLinearGradient(String str, Color color,Double angle)
        {
            String linearGrad = angle.ToString(CultureInfo.InvariantCulture) + ";";

            String[] shadeValues = str.Split(';');
            
            foreach (String value in shadeValues)
            {
                String[] splitValue = value.Split(',');
                if (splitValue.Length <= 2) continue;

                Double intensity = Double.Parse(splitValue[1], CultureInfo.InvariantCulture);
                Double stops = Double.Parse(splitValue[2], CultureInfo.InvariantCulture);

                switch (splitValue[0])
                {
                    case "l":
                    case "L":
                        linearGrad += GetLighterColor(color,intensity).ToString() + "," + stops + ";";
                        break;
                    case "d":
                    case "D":
                        linearGrad += GetDarkerColor(color, intensity).ToString() + "," + stops + ";";
                        break;
                }
            }

            return ParseLinearGradient(linearGrad);
        }

        /// <summary>
        /// This converts a given String of X;Y;color,stop;.... String to a radial gradient brush
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
        /// Converts a color is String form to Solid Color Brush
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
            darkerShade.A = color.A;
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
            darkerShade.A = color.A;
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
            lighterShade.A = color.A;
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
            lighterShade.A = color.A;
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
            newColor.A = color.A;
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

        public static Color RemoveAlpha(Color color)
        {
            Color newColor = new Color();
            newColor.A = 255;
            newColor.R = color.R;
            newColor.G = color.G;
            newColor.B = color.B;
            return newColor;
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
                intensity = 1;
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

        public static Brush GetDefaultFontColor(Double intensity)
        {
            Brush brush = null;
            if (intensity < 0.5)
            {
                brush = ParseSolidColor("#EFEFEF");
            }
            else
            {
                brush = ParseSolidColor("#000000");
            }
            return brush;
        }

        public static Brush GetDefaultBorderColor(Double intensity)
        {
            Brush brush = null;
            if (intensity < 0.5)
            {
                brush = ParseSolidColor("#BBBBBB");
            }
            else
            {
                brush = ParseSolidColor("#000000");
            }
            return brush;
        }

        public static Brush GenerateBrush(Brush baseBrush,Boolean replicateBrush, GradientParams gradientParams)
        {
             Brush generatedBrush = null;

            if (baseBrush.GetType().Name == "LinearGradientBrush")
            {
                if (replicateBrush)
                    return baseBrush;

                LinearGradientBrush brush = baseBrush as LinearGradientBrush;
                generatedBrush = new LinearGradientBrush();

                (generatedBrush as LinearGradientBrush).StartPoint = new Point(gradientParams._linearGradientParams._startPoint.X, gradientParams._linearGradientParams._startPoint.Y);
                (generatedBrush as LinearGradientBrush).EndPoint = new Point(gradientParams._linearGradientParams._endPoint.X,gradientParams._linearGradientParams._endPoint.Y);

                switch(gradientParams._linearGradientParams._shadeType)
                {
                    case "l":
                    case "L":
                        Parser.GenerateLighterGradientBrush(brush, generatedBrush as LinearGradientBrush, gradientParams._linearGradientParams._intensity);
                        break;
                    case "d":
                    case "D":
                        Parser.GenerateDarkerGradientBrush(brush, generatedBrush as LinearGradientBrush, gradientParams._linearGradientParams._intensity);
                        break;

                }
                if (!Double.IsNaN(gradientParams._linearGradientParams._angle))
                {
                    RotateTransform rt = new RotateTransform();
                    rt.Angle = gradientParams._linearGradientParams._angle;
                    generatedBrush.RelativeTransform = rt;
                }

            }
            else if (baseBrush.GetType().Name == "RadialGradientBrush")
            {
                if (replicateBrush)
                    return baseBrush;

                RadialGradientBrush brush = baseBrush as RadialGradientBrush;

                generatedBrush = new RadialGradientBrush();

                (generatedBrush as RadialGradientBrush).GradientOrigin = brush.GradientOrigin;

                switch (gradientParams._radialGradientParams._shadeType)
                {
                    case "l":
                    case "L":
                        Parser.GenerateDarkerGradientBrush(brush, generatedBrush as RadialGradientBrush, gradientParams._radialGradientParams._intensity);
                        break;
                    case "d":
                    case "D":
                        Parser.GenerateLighterGradientBrush(brush, generatedBrush as RadialGradientBrush, gradientParams._radialGradientParams._intensity);
                        break;
                }
            }
            else if (baseBrush.GetType().Name == "SolidColorBrush")
            {
                SolidColorBrush brush = baseBrush as SolidColorBrush;
                if (gradientParams._solidBrushParams._isGrad)
                {
                    generatedBrush = Parser.ParseLinearGradient(gradientParams._solidBrushParams._gradString, brush.Color, gradientParams._solidBrushParams._angle);
                }
                else
                {
                    switch (gradientParams._solidBrushParams._shadeType)
                    {
                        case "l":
                        case "L":
                            generatedBrush = new SolidColorBrush(Parser.GetLighterColor(brush.Color, gradientParams._solidBrushParams._intensity));
                            break;
                        case "d":
                        case "D":
                            generatedBrush = new SolidColorBrush(Parser.GetDarkerColor(brush.Color, gradientParams._solidBrushParams._intensity));
                            break;
                        default:
                            generatedBrush = brush;
                            break;
                    }
                }
            }
            else
            {
                generatedBrush = baseBrush;
            }

            return generatedBrush;
        }

        public static DoubleCollection GetStrokeDashArray(String dashType)
        {
            if (dashType == null) return null;

            DoubleCollection dashArray = new DoubleCollection();

            switch (dashType.ToLower())
            {
                case "solid":
                    dashArray = null;
                    break;

                case "dashed":
                    dashArray.Clear();
                    dashArray.Add(4);
                    dashArray.Add(4);
                    dashArray.Add(4);
                    dashArray.Add(4);
                    break;

                case "dotted":
                    dashArray.Clear();
                    dashArray.Add(1);
                    dashArray.Add(2);
                    dashArray.Add(1);
                    dashArray.Add(2);
                    break;

            }

            return dashArray;
        }

        public static PathGeometry GetPathGeometryFromList(FillRule fillRule, Point startPoint, List<PathGeometryParams> pathGeometryParams)
        {
            PathGeometry pathGeometry = new PathGeometry();

            pathGeometry.FillRule = fillRule;
            pathGeometry.Figures = new PathFigureCollection();

            PathFigure pathFigure = new PathFigure();

            pathFigure.StartPoint = startPoint;
            pathFigure.Segments = new PathSegmentCollection();

            foreach (PathGeometryParams param in pathGeometryParams)
            {
                switch (param.GetType().Name)
                {
                    case "LineSegmentParams":
                        LineSegment lineSegment = new LineSegment();
                        lineSegment.Point = param.EndPoint;
                        pathFigure.Segments.Add(lineSegment);
                        break;

                    case "ArcSegmentParams":
                        ArcSegment arcSegment = new ArcSegment();
                        arcSegment.Point = param.EndPoint;
                        arcSegment.IsLargeArc = (param as ArcSegmentParams).IsLargeArc;
                        arcSegment.RotationAngle = (param as ArcSegmentParams).RotationAngle;
                        arcSegment.SweepDirection = (param as ArcSegmentParams).SweepDirection;
                        arcSegment.Size = (param as ArcSegmentParams).Size;
                        pathFigure.Segments.Add(arcSegment);
                        break;
                }
            }

            pathGeometry.Figures.Add(pathFigure);

            return pathGeometry;
        }

        public static GeometryGroup GetGeometryGroupFromList(FillRule fillRule, List<Object> geometryGroupParams)
        {
            GeometryGroup geometryGroup = new GeometryGroup();

            geometryGroup.Children = new GeometryCollection();
            geometryGroup.FillRule = fillRule;

            foreach (Object param in geometryGroupParams)
            {
                switch (param.GetType().Name)
                {
                    case "EllipseGeometryParams":
                        EllipseGeometryParams ellipse = param as EllipseGeometryParams;
                        geometryGroup.Children.Add(GetEllipseGeometry(ellipse.Center, ellipse.RadiusX, ellipse.RadiusY));
                        break;
                }
            }

            return geometryGroup;
        }

        public static EllipseGeometry GetEllipseGeometry(Point center, Double radiusX, Double radiusY)
        {
            EllipseGeometry ellipseGeometry = new EllipseGeometry();
            ellipseGeometry.Center = center;
            ellipseGeometry.RadiusX = radiusX;
            ellipseGeometry.RadiusY = radiusY;

            return ellipseGeometry;
        }

        /// <summary>
        /// Accepts absolute or relative Uri, builds and returns abslute path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static String BuildAbsolutePath(String path)
        {
            Uri ur = new Uri(path, UriKind.RelativeOrAbsolute);
            if (ur.IsAbsoluteUri)
            {
                return ur.AbsoluteUri;
            }
            else if (path.StartsWith("/"))
            {
                UriBuilder baseUri = new UriBuilder(Constants.Html.BaseUri);
                UriBuilder newUri = new UriBuilder(baseUri.Scheme, baseUri.Host, baseUri.Port, path);
                return newUri.ToString();
            }
            else
            {
                UriBuilder baseUri = new UriBuilder(Constants.Html.BaseUri);
                String sourcePath = baseUri.Path.Substring(0, baseUri.Path.LastIndexOf('/') + 1);
                UriBuilder newUri = new UriBuilder(baseUri.Scheme, baseUri.Host, baseUri.Port, sourcePath + path);
                return newUri.ToString();
            }
        }


        public static String GetFormattedText(String text)
        {
            if (String.IsNullOrEmpty(text)) 
                return "";

            String[] split = { "\\n" };
            String[] lines = text.Split(split, StringSplitOptions.RemoveEmptyEntries);
            String multiLineText = "";
            foreach (String line in lines)
            {
                if (line.EndsWith("\\"))
                {
                    multiLineText += line + "n";
                }
                else
                {
                    multiLineText += line + "\n";
                }
            }

            if (text.EndsWith("\\n"))
                return multiLineText;
            else 
                return multiLineText.Substring(0,multiLineText.Length-1);
        }


        public static FontFamily GetFont(String fontFamily, TextBlock element)
        {
            if (String.IsNullOrEmpty(fontFamily) || element == null)
                return null;

            if (fontFamily.Contains("#"))
            {
                String[] split = fontFamily.Split('#');

                WebClient webClient = new WebClient();
                webClient.OpenReadCompleted += delegate(object sender, OpenReadCompletedEventArgs e)
                {
                    (e.UserState as TextBlock).FontSource = new FontSource(e.Result as Stream);
                };
                webClient.OpenReadAsync(new Uri(split[0], UriKind.RelativeOrAbsolute), element);

                return new FontFamily(split[1]);
            }
            else
            {
                return new FontFamily(fontFamily);
            }

            
        }

        #endregion Static Methods

    }

}
