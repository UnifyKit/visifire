#if WPF
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Markup;
using System.Xml;
using System.Globalization;


#else

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Windows.Markup;
using System.Globalization;
#endif
using System.ComponentModel;
using Visifire.Charts;
using Visifire.Commons;


namespace Visifire.Charts
{
    /// <summary>
    /// Visifire.Charts.Faces class
    /// </summary>
    public class Faces
    {
        /// <summary>
        /// Initializes a new instance of Visifire.Charts.Faces class
        /// </summary>
        public Faces()
        {
            VisualComponents = new List<FrameworkElement>();
        }

        /// <summary>
        /// Contains references to individual components of the elements in the visual
        /// </summary>
        public List<FrameworkElement> VisualComponents
        {
            get;
            set;
        }

        /// <summary>
        /// Different parts of Visuals. Parts are used while doing partial update
        /// </summary>
        public List<FrameworkElement> Parts
        {
            get;
            set;
        }

        /// <summary>
        /// Visual of faces
        /// </summary>
        public Panel Visual
        {
            get;
            set;
        }

        /// <summary>
        /// Label canvas reference for faces
        /// </summary>
        public Canvas LabelCanvas
        {
            get;
            set;
        }

    }

    /// <summary>
    /// Visifire.Charts.ExtendedGraphics class
    /// </summary>
    internal class ExtendedGraphics
    {
        /// <summary>
        /// Initializes a new instance of Visifire.Charts.ExtendedGraphics class
        /// </summary>
        public ExtendedGraphics()
        {
        }

        #region Static Methods

        /// <summary>
        /// Returns dash array for border
        /// </summary>
        /// <param name="borderStyle">BorderStyle as BorderStyles</param>
        /// <returns>DashArray as DoubleCollection</returns>
        internal static DoubleCollection GetDashArray(BorderStyles borderStyle)
        {
            return Graphics.LineStyleToStrokeDashArray(borderStyle.ToString());
        }
        
        /// <summary>
        /// Returns dash array for line
        /// </summary>
        /// <param name="lineStyle">LineStyle as LineStyles</param>
        /// <returns>DashArray as DoubleCollection</returns>
        internal static DoubleCollection GetDashArray(LineStyles lineStyle)
        {
            return Graphics.LineStyleToStrokeDashArray(lineStyle.ToString());
        }

        /// <summary>
        /// Generates a rectangle. The shape of each of the corners can be controlled and is useful for creating single sided 
        /// curved rectangles.
        /// </summary>
        private static PathGeometry GetRectanglePathGeometry(Double width, Double height, CornerRadius xRadius, CornerRadius yRadius)
        {
            // Create a path geometry object
            PathGeometry pathGeometry = new PathGeometry();

            pathGeometry.Figures = new PathFigureCollection();

            PathFigure pathFigure = new PathFigure();

            pathFigure.StartPoint = new Point(xRadius.TopLeft, 0);
            pathFigure.Segments = new PathSegmentCollection();

            // Do not change the order of the lines below
            // Segmens required to create the rectangle
            pathFigure.Segments.Add(Graphics.GetLineSegment(new Point(width - xRadius.TopRight, 0)));
            pathFigure.Segments.Add(Graphics.GetArcSegment(new Point(width, yRadius.TopRight), new Size(xRadius.TopRight, yRadius.TopRight), 0, SweepDirection.Clockwise));
            pathFigure.Segments.Add(Graphics.GetLineSegment(new Point(width, height - yRadius.BottomRight)));
            pathFigure.Segments.Add(Graphics.GetArcSegment(new Point(width - xRadius.BottomRight, height), new Size(xRadius.BottomRight, yRadius.BottomRight), 0, SweepDirection.Clockwise));
            pathFigure.Segments.Add(Graphics.GetLineSegment(new Point(xRadius.BottomLeft, height)));
            pathFigure.Segments.Add(Graphics.GetArcSegment(new Point(0, height - yRadius.BottomLeft), new Size(xRadius.BottomLeft, yRadius.BottomLeft), 0, SweepDirection.Clockwise));
            pathFigure.Segments.Add(Graphics.GetLineSegment(new Point(0, yRadius.TopLeft)));
            pathFigure.Segments.Add(Graphics.GetArcSegment(new Point(xRadius.TopLeft, 0), new Size(xRadius.TopLeft, yRadius.TopLeft), 0, SweepDirection.Clockwise));

            pathGeometry.Figures.Add(pathFigure);

            return pathGeometry;
        }

        /// <summary>
        /// Get the corrected corner radius
        /// </summary>
        /// <param name="radius">Radius as CornerRadius</param>
        /// <param name="limit">Limit as Double</param>
        /// <returns>CornerRadius</returns>
        private static CornerRadius GetCorrectedRadius(CornerRadius radius,Double limit)
        {
           return new CornerRadius(
                ((radius.TopLeft > limit) ? limit : radius.TopLeft),
                ((radius.TopRight > limit) ? limit : radius.TopRight),
                ((radius.BottomRight > limit) ? limit : radius.BottomRight),
                ((radius.BottomLeft > limit) ? limit : radius.BottomLeft)
                );
        }

        /// <summary>
        /// Get corner shadow gradient brush for a rectangle
        /// </summary>
        /// <param name="corner">Corner as Corners</param>
        /// <returns>Brush</returns>
        private static Brush GetCornerShadowGradientBrush(Corners corner)
        {
            RadialGradientBrush gradBrush = new RadialGradientBrush();
            gradBrush.GradientStops = new GradientStopCollection();
            gradBrush.GradientStops.Add(Graphics.GetGradientStop(Color.FromArgb(191, 0, 0, 0), 0));
            gradBrush.GradientStops.Add(Graphics.GetGradientStop(Color.FromArgb(0, 0, 0, 0), 1));
            TransformGroup tg = new TransformGroup();
            ScaleTransform st = new ScaleTransform() { ScaleX = 2, ScaleY = 2, CenterX = 0.5, CenterY = 0.5 };
            TranslateTransform tt = null;
            switch (corner)
            {
                case Corners.TopLeft:
                    tt = new TranslateTransform() { X = 0.5, Y = 0.5 };
                    break;
                case Corners.TopRight:
                    tt = new TranslateTransform() { X = -0.5, Y = 0.5 };
                    break;
                case Corners.BottomLeft:
                    tt = new TranslateTransform() { X = 0.5, Y = -0.5 };
                    break;
                case Corners.BottomRight:
                    tt = new TranslateTransform() { X = -0.5, Y = -0.5 };
                    break;
            }
            tg.Children.Add(st);
            tg.Children.Add(tt);
            gradBrush.RelativeTransform = tg;
            return gradBrush;
        }

        /// <summary>
        /// Get side shadow gradient brush for a rectangle
        /// </summary>
        /// <param name="direction">Direction as Directions</param>
        /// <returns>Brush</returns>
        private static Brush GetSideShadowGradientBrush(Directions direction)
        {
            LinearGradientBrush gradBrush = new LinearGradientBrush();
            gradBrush.GradientStops = new GradientStopCollection();
            gradBrush.GradientStops.Add(Graphics.GetGradientStop(Color.FromArgb(191, 0, 0, 0), 0));
            gradBrush.GradientStops.Add(Graphics.GetGradientStop(Color.FromArgb(0, 0, 0, 0), 1));
            switch (direction)
            {   
                case Directions.Top:
                    gradBrush.StartPoint = new Point(0.5, 1);
                    gradBrush.EndPoint = new Point(0.5, 0);
                    break;
                case Directions.Right:
                    gradBrush.StartPoint = new Point(0, 0.5);
                    gradBrush.EndPoint = new Point(1, 0.5);
                    break;
                case Directions.Left:
                    gradBrush.StartPoint = new Point(1, 0.5);
                    gradBrush.EndPoint = new Point(0, 0.5);
                    break;
                case Directions.Bottom:
                    gradBrush.StartPoint = new Point(0.5, 0);
                    gradBrush.EndPoint = new Point(0.5, 1);
                    break;
            }
            return gradBrush;
        }

        /// <summary>
        /// Clone a DoubleCollection
        /// </summary>
        /// <param name="collection">Collection as DoubleCollection</param>
        /// <returns>DoubleCollection</returns>
        public static DoubleCollection CloneCollection(DoubleCollection collection)
        {
            DoubleCollection newCollection = new DoubleCollection();
            foreach (Double value in collection)
                newCollection.Add(value);

            return newCollection;
        }

        /// <summary>
        /// Creates and returns a rectangle based on the given params
        /// </summary>
        /// <param name="width">Visual width</param>
        /// <param name="height">Visual height</param>
        /// <param name="strokeThickness">StrokeThickness</param>
        /// <param name="strokeDashArray">StrokeDashArray</param>
        /// <param name="stroke">Stroke</param>
        /// <param name="fill">Fill color</param>
        /// <param name="xRadius">XRadius as CornerRadius</param>
        /// <param name="yRadius">YRadius as CornerRadius</param>
        /// <returns>Canvas</returns>
        public static Canvas Get2DRectangle(Double width, Double height, Double strokeThickness, DoubleCollection strokeDashArray, Brush stroke, Brush fill, CornerRadius xRadius, CornerRadius yRadius)
        {
            Canvas canvas = new Canvas();

            Path rectangle = new Path();

            canvas.Width = width;
            canvas.Height = height;

            rectangle.StrokeThickness = strokeThickness;
            rectangle.StrokeDashArray = strokeDashArray != null ? CloneCollection(strokeDashArray) : strokeDashArray;
            rectangle.StrokeDashCap = PenLineCap.Flat;
            rectangle.StrokeEndLineCap = PenLineCap.Flat;
            rectangle.StrokeMiterLimit = 1;
            rectangle.StrokeStartLineCap = PenLineCap.Flat;
            rectangle.StrokeLineJoin = PenLineJoin.Bevel;
            rectangle.Stroke = stroke;

            rectangle.Fill = fill;

            rectangle.Data = GetRectanglePathGeometry(
                width,
                height,
                GetCorrectedRadius(xRadius, width),
                GetCorrectedRadius(yRadius, height)
                );

            rectangle.SetValue(Canvas.TopProperty, (Double)0);
            rectangle.SetValue(Canvas.LeftProperty, (Double)0);

            canvas.Children.Add(rectangle);

            return canvas;
        }

        /// <summary>
        /// Creates and returns a rectangle bevel layer based on the given params
        /// </summary>
        /// <param name="width">Visual width</param>
        /// <param name="height">Visual height</param>
        /// <param name="bevelX">BevelX as Double</param>
        /// <param name="bevelY">BevelY as Double</param>
        /// <param name="topBrush">TopBrush</param>
        /// <param name="leftBrush">LeftBrush</param>
        /// <param name="rightBrush">RightBrush</param>
        /// <param name="bottomBrush">BottomBrush</param>
        /// <returns>Canvas</returns>
        public static Canvas Get2DRectangleBevel(Double width, Double height, Double bevelX,Double bevelY, Brush topBrush, Brush leftBrush, Brush rightBrush, Brush bottomBrush)
        {
            Canvas canvas = new Canvas();

            canvas.Width = width;
            canvas.Height = height;

            Polygon topBevel = new Polygon();
            topBevel.Points = new PointCollection();
            topBevel.Points.Add(new Point(0, 0));
            topBevel.Points.Add(new Point(width, 0));
            topBevel.Points.Add(new Point(width - bevelX , bevelY));
            topBevel.Points.Add(new Point(bevelX, bevelY));
            topBevel.Fill = topBrush;
            topBevel.Tag = "TopBevel";
            canvas.Children.Add(topBevel);

            Polygon leftBevel = new Polygon();
            leftBevel.Points = new PointCollection();
            leftBevel.Points.Add(new Point(0, 0));
            leftBevel.Points.Add(new Point(bevelX, bevelY));
            leftBevel.Points.Add(new Point(bevelX, height - bevelY));
            leftBevel.Points.Add(new Point(0, height));
            leftBevel.Fill = leftBrush;
            leftBevel.Tag = "LeftBevel";
            canvas.Children.Add(leftBevel);

            Polygon rightBevel = new Polygon();
            rightBevel.Points = new PointCollection();
            rightBevel.Points.Add(new Point(width, 0));
            rightBevel.Points.Add(new Point(width, height));
            rightBevel.Points.Add(new Point(width - bevelX, height - bevelY));
            rightBevel.Points.Add(new Point(width - bevelX, bevelY));
            rightBevel.Fill = rightBrush;
            rightBevel.Tag = "RightBevel";
            canvas.Children.Add(rightBevel);

            Polygon bottomBevel = new Polygon();
            bottomBevel.Points = new PointCollection();
            bottomBevel.Points.Add(new Point(0, height));
            bottomBevel.Points.Add(new Point(bevelX,height - bevelY));
            bottomBevel.Points.Add(new Point(width - bevelX, height - bevelY));
            bottomBevel.Points.Add(new Point(width, height));
            bottomBevel.Fill = bottomBrush;
            bottomBevel.Tag = "BottomBevel";
            canvas.Children.Add(bottomBevel);

            return canvas;
        }

        /// <summary>
        /// Creates and returns a rectangle gradient layer based on the given params
        /// </summary>
        /// <param name="width">Visual width</param>
        /// <param name="height">visual height</param>
        /// <param name="brush1">Brush1</param>
        /// <param name="brush2">Brush2</param>
        /// <param name="orientation">Orientation</param>
        /// <returns>Canvas</returns>
        public static Canvas Get2DRectangleGradiance(Double width, Double height, Brush brush1, Brush brush2, Orientation orientation)
        {
            Canvas canvas = new Canvas();

            canvas.Width = width;
            canvas.Height = height;

            if (orientation == Orientation.Vertical)
            {
                Rectangle rectLeft = new Rectangle();
                rectLeft.Width = width / 2 ;
                rectLeft.Height = height;
                rectLeft.SetValue(Canvas.TopProperty, (Double)0);
                rectLeft.SetValue(Canvas.LeftProperty, (Double)0);
                rectLeft.Fill = brush1;
                rectLeft.Tag = "GradianceLeft";
                canvas.Children.Add(rectLeft);

                Rectangle rectRight = new Rectangle();
                rectRight.Width = width / 2;
                rectRight.Height = height;
                rectRight.SetValue(Canvas.TopProperty, (Double)0);
                rectRight.SetValue(Canvas.LeftProperty, (Double)width / 2);
                rectRight.Fill = brush2;
                rectLeft.Tag = "GradianceRight";
                canvas.Children.Add(rectRight);
            }
            else
            {   
                Rectangle rectTop = new Rectangle();
                rectTop.Width = width;
                rectTop.Height = height / 2;
                rectTop.SetValue(Canvas.TopProperty, (Double)0);
                rectTop.SetValue(Canvas.LeftProperty, (Double)0);
                rectTop.Fill = brush1;
                rectTop.Tag = "GradianceTop";
                canvas.Children.Add(rectTop);

                Rectangle rectBottom = new Rectangle();
                rectBottom.Width = width;
                rectBottom.Height = height / 2;
                rectBottom.SetValue(Canvas.TopProperty, (Double)height / 2);
                rectBottom.SetValue(Canvas.LeftProperty, (Double)0);
                rectBottom.Fill = brush2;
                rectBottom.Tag = "GradianceBottom";
                canvas.Children.Add(rectBottom);
            }

            return canvas;
        }

        /// <summary>
        /// Creates and returns a rectangle shadow based on the given params
        /// </summary>
        /// <param name="width">Visual width</param>
        /// <param name="height">Visual height</param>
        /// <param name="xRadius">XRadius as CornerRadius</param>
        /// <param name="yRadius">YRadius as CornerRadius</param>
        /// <param name="minCurvature">MinCurvature as Double</param>
        /// <returns>Grid</returns>
        public static Grid Get2DRectangleShadow(Double width, Double height, CornerRadius xRadius, CornerRadius yRadius, Double minCurvature)
        {
            CornerRadius tempXRadius = new CornerRadius(Math.Max(xRadius.TopLeft, minCurvature), Math.Max(xRadius.TopRight, minCurvature), Math.Max(xRadius.BottomRight, minCurvature), Math.Max(xRadius.BottomLeft, minCurvature));
            CornerRadius tempYRadius = new CornerRadius(Math.Max(yRadius.TopLeft, minCurvature), Math.Max(yRadius.TopRight, minCurvature), Math.Max(yRadius.BottomRight, minCurvature), Math.Max(yRadius.BottomLeft, minCurvature));

            CornerRadius radiusX = GetCorrectedRadius(tempXRadius, width/2);
            CornerRadius radiusY = GetCorrectedRadius(tempYRadius, height/2);

            Grid visual = new Grid();
            visual.Height = height;
            visual.Width = width;

            for (Int32 index = 0; index < 3; index++)
            {
                visual.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(0, GridUnitType.Auto) });
                visual.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(0, GridUnitType.Auto) });
            }

            Rectangle topLeft = new Rectangle() { Width = radiusX.TopLeft, Height = radiusY.TopLeft,Fill = GetCornerShadowGradientBrush(Corners.TopLeft) };
            Rectangle topRight = new Rectangle() { Width = radiusX.TopRight, Height = radiusY.TopRight, Fill = GetCornerShadowGradientBrush(Corners.TopRight) };
            Rectangle bottomLeft = new Rectangle() { Width = radiusX.BottomLeft, Height = radiusY.BottomLeft, Fill = GetCornerShadowGradientBrush(Corners.BottomLeft) };
            Rectangle bottomRight = new Rectangle() { Width = radiusX.BottomRight, Height = radiusY.BottomRight, Fill = GetCornerShadowGradientBrush(Corners.BottomRight) };
            Rectangle center = new Rectangle() { Width = width - radiusX.TopLeft - radiusX.TopRight, Height = height - radiusY.TopLeft - radiusY.BottomLeft, Fill = new SolidColorBrush(Color.FromArgb((Byte)191, (Byte)0, (Byte)0, (Byte)0)) };
            Rectangle top = new Rectangle() { Width = width - radiusX.TopLeft - radiusX.TopRight, Height = Math.Max(radiusY.TopLeft, radiusY.TopRight), Fill = GetSideShadowGradientBrush(Directions.Top) };
            Rectangle right = new Rectangle() { Width = Math.Max(radiusX.TopRight, radiusX.BottomRight), Height = height - radiusY.TopRight - radiusY.BottomRight, Fill = GetSideShadowGradientBrush(Directions.Right) };
            Rectangle left = new Rectangle() { Width = Math.Max(radiusX.TopLeft, radiusX.BottomLeft), Height = height - radiusY.TopLeft - radiusY.BottomLeft, Fill = GetSideShadowGradientBrush(Directions.Left) };
            Rectangle bottom = new Rectangle() { Width = width - radiusX.BottomLeft - radiusX.BottomRight, Height = Math.Max(radiusY.BottomLeft, radiusY.BottomRight), Fill = GetSideShadowGradientBrush(Directions.Bottom) };

            topLeft.SetValue(Grid.RowProperty, (Int32)0); topLeft.SetValue(Grid.ColumnProperty, (Int32)0);
            top.SetValue(Grid.RowProperty, (Int32)0); top.SetValue(Grid.ColumnProperty, (Int32)1);
            topRight.SetValue(Grid.RowProperty, (Int32)0); topRight.SetValue(Grid.ColumnProperty, (Int32)2);
            left.SetValue(Grid.RowProperty, (Int32)1); topLeft.SetValue(Grid.ColumnProperty, (Int32)0);
            center.SetValue(Grid.RowProperty, (Int32)1); center.SetValue(Grid.ColumnProperty, (Int32)1);
            right.SetValue(Grid.RowProperty, (Int32)1); right.SetValue(Grid.ColumnProperty, (Int32)2);
            bottomLeft.SetValue(Grid.RowProperty, (Int32)2); bottomLeft.SetValue(Grid.ColumnProperty, (Int32)0);
            bottom.SetValue(Grid.RowProperty, (Int32)2); bottom.SetValue(Grid.ColumnProperty, (Int32)1);
            bottomRight.SetValue(Grid.RowProperty, (Int32)2); bottomRight.SetValue(Grid.ColumnProperty, (Int32)2);

            visual.Children.Add(topLeft);
            visual.Children.Add(top);
            visual.Children.Add(topRight);
            visual.Children.Add(left);
            visual.Children.Add(center);
            visual.Children.Add(right);
            visual.Children.Add(bottomLeft);
            visual.Children.Add(bottom);
            visual.Children.Add(bottomRight);

            return visual;
        }

        private enum Corners { TopLeft, TopRight, BottomLeft, BottomRight };
        private enum Directions { Top, Left, Right, Bottom };
        #endregion
    }
}

namespace Visifire.Commons
{
    /// <summary>
    /// Visifire.Commons.Graphics class
    /// </summary>
    public class Graphics
    {
        /// <summary>
        /// Initializes a new instance of Visifire.Commons.Graphics class
        /// </summary>
        public Graphics()
        {
        }

        #region Static Methods

        /// <summary>
        /// Calculates visual size
        /// </summary>
        /// <param name="visual">Visual as FrameworkElement</param>
        /// <returns>Visual size</returns>
        internal static Size CalculateVisualSize(FrameworkElement visual)
        {
            Size retVal = new Size(0,0);;

            if (visual != null)
            {
                visual.Measure(new Size(Double.MaxValue, Double.MaxValue));
                retVal = visual.DesiredSize;
            }

            return retVal;
        }

        /// <summary>
        /// Calculates textblock size
        /// </summary>
        /// <param name="radianAngle">RadianAngle as Double</param>
        /// <param name="textBlock">TextBlock</param>
        /// <returns>TextBlock size</returns>
        internal static Size CalculateTextBlockSize(Double radianAngle, TextBlock textBlock)
        {
            Double actualHeight;
            Double actualWidth;

#if WPF
            textBlock.Measure(new Size(Double.MaxValue, Double.MaxValue));
            actualHeight = textBlock.DesiredSize.Height;
            actualWidth = textBlock.DesiredSize.Width;
#else
            actualHeight = textBlock.ActualHeight;
            actualWidth = textBlock.ActualWidth;
#endif
            if (radianAngle != 0)
            {
                // length of the diagonal from top left to bottom right
                Double length = Math.Sqrt(Math.Pow(actualHeight, 2) + Math.Pow(actualWidth, 2));

                // angle made by the diagonal with respect to the horizontal
                Double beta = Math.Atan(actualHeight / actualWidth);

                // calculate the two possible height and width values using the diagonal length and angle
                Double height1 = length * Math.Sin(radianAngle + beta);
                Double height2 = length * Math.Sin(radianAngle - beta);
                Double width1 = length * Math.Cos(radianAngle + beta);
                Double width2 = length * Math.Cos(radianAngle - beta);

                // Actual height will be the maximum of the two calculated heights
                actualHeight = Math.Max(Math.Abs(height1), Math.Abs(height2));

                // Actual width will be the maximum of the two calculated widths
                actualWidth = Math.Max(Math.Abs(width1), Math.Abs(width2));
            }

            return new Size(actualWidth, actualHeight);
        }

        /// <summary>
        /// Generate double collection
        /// </summary>
        /// <param name="values">Array of double values</param>
        /// <returns>DoubleCollection</returns>
        internal static DoubleCollection GenerateDoubleCollection(params Double[] values)
        {
            DoubleCollection collection = new DoubleCollection();

            foreach (Double value in values)
                collection.Add(value);

            return collection;
        }

        /// <summary>
        /// Creates and returns a right gradient brush
        /// </summary>
        /// <param name="alpha">Alpha as Int32</param>
        /// <returns>Brush</returns>
        internal static Brush GetRightGradianceBrush(Int32 alpha)
        {
            LinearGradientBrush gradBrush = new LinearGradientBrush();

            gradBrush.GradientStops = new GradientStopCollection();

            gradBrush.StartPoint = new Point(1, 1);
            gradBrush.EndPoint = new Point(0, 0);

            gradBrush.GradientStops.Add(Graphics.GetGradientStop(Color.FromArgb((byte)alpha, 0, 0, 0), 0));
            gradBrush.GradientStops.Add(Graphics.GetGradientStop(Color.FromArgb(0, 0, 0, 0), 0));

            return gradBrush;
        }

        /// <summary>
        /// Creates and returns a top face brush
        /// </summary>
        /// <param name="brush">Brush</param>
        /// <returns>Brush</returns>
        internal static Brush GetTopFaceBrush(Brush brush)
        {
            if (typeof(SolidColorBrush).Equals(brush.GetType()))
            {
                SolidColorBrush solidBrush = brush as SolidColorBrush;

                List<Color> colors = new List<Color>();
                List<Double> stops = new List<Double>();

                colors.Add(Graphics.GetDarkerColor(solidBrush.Color, 0.85));
                stops.Add(0);

                colors.Add(Graphics.GetLighterColor(solidBrush.Color, 0.35));
                stops.Add(1);

                return Graphics.CreateLinearGradientBrush(-45, new Point(0, 0.5), new Point(1, 0.5), colors, stops);
            }
            else if (brush is GradientBrush)
            {
                GradientBrush gradBrush = brush as GradientBrush;

                List<Color> colors = new List<Color>();
                List<Double> stops = new List<Double>();

                foreach (GradientStop gradStop in gradBrush.GradientStops)
                {
                    colors.Add(Graphics.GetLighterColor(gradStop.Color, 0.85));
                    stops.Add(gradStop.Offset);
                }

                if (brush is LinearGradientBrush)
                    return Graphics.CreateLinearGradientBrush(-45, new Point(-0.5, 1.5), new Point(0.5, 0), colors, stops);
                else
                    return Graphics.CreateRadialGradientBrush(colors, stops);
            }
            else
            {
                return brush;
            }
        }

        /// <summary>
        /// Creates and returns a right face brush
        /// </summary>
        /// <param name="brush">Brush</param>
        /// <returns>Brush</returns>
        internal static Brush GetRightFaceBrush(Brush brush)
        {
            if (brush != null)
            {
                if (typeof(SolidColorBrush).Equals(brush.GetType()))
                {
                    SolidColorBrush solidBrush = brush as SolidColorBrush;

                    List<Color> colors = new List<Color>();
                    List<Double> stops = new List<Double>();

                    colors.Add(Graphics.GetDarkerColor(solidBrush.Color, 0.35));
                    stops.Add(0);

                    colors.Add(Graphics.GetDarkerColor(solidBrush.Color, 0.75));
                    stops.Add(1);


                    return Graphics.CreateLinearGradientBrush(-120, new Point(0, 0.5), new Point(1, 0.5), colors, stops);
                }
                else if (brush is GradientBrush)
                {
                    GradientBrush gradBrush = brush as GradientBrush;

                    List<Color> colors = new List<Color>();
                    List<Double> stops = new List<Double>();

                    foreach (GradientStop gradStop in gradBrush.GradientStops)
                    {
                        colors.Add(Graphics.GetDarkerColor(gradStop.Color, 0.75));
                        stops.Add(gradStop.Offset);
                    }

                    if (brush is LinearGradientBrush)
                        return Graphics.CreateLinearGradientBrush(0, new Point(0, 1), new Point(1, 0), colors, stops);
                    else
                        return Graphics.CreateRadialGradientBrush(colors, stops);
                }
                else
                {
                    return brush;
                }
            }
            else
                return null;
        }

        /// <summary>
        /// Creates and returns a front face brush
        /// </summary>
        /// <param name="brush">Brush</param>
        /// <returns>Brush</returns>
        internal static Brush GetFrontFaceBrush(Brush brush)
        {
            if (typeof(SolidColorBrush).Equals(brush.GetType()))
            {
                SolidColorBrush solidBrush = brush as SolidColorBrush;

                List<Color> colors = new List<Color>();
                List<Double> stops = new List<Double>();

                colors.Add(Graphics.GetDarkerColor(solidBrush.Color, 0.65));
                stops.Add(0);

                colors.Add(Graphics.GetLighterColor(solidBrush.Color, 0.55));
                stops.Add(1);

                return Graphics.CreateLinearGradientBrush(-90, new Point(0, 0.5), new Point(1, 0.5), colors, stops);
            }
            else
            {
                return brush;
            }
        }

        /// <summary>
        /// Creates and returns a left gradient brush
        /// </summary>
        /// <param name="alpha">Alpha as Int32</param>
        /// <returns>Brush</returns>
        internal static Brush GetLeftGradianceBrush(Int32 alpha)
        {
            LinearGradientBrush gradBrush = new LinearGradientBrush();

            gradBrush.GradientStops = new GradientStopCollection();

            gradBrush.StartPoint = new Point(0, 1);
            gradBrush.EndPoint = new Point(1, 0);

            gradBrush.GradientStops.Add(Graphics.GetGradientStop(Color.FromArgb((byte)alpha, 0, 0, 0), 0));
            gradBrush.GradientStops.Add(Graphics.GetGradientStop(Color.FromArgb(0, 0, 0, 0), 0));

            return gradBrush;
        }

        /// <summary>
        /// Creates and returns a line segment
        /// </summary>
        /// <param name="point">Point</param>
        /// <returns>LineSegment</returns>
        public static LineSegment GetLineSegment(Point point)
        {
            LineSegment lineSegment = new LineSegment();
            lineSegment.Point = point;
            return lineSegment;
        }

        /// <summary>
        /// Creates and returns an arc segment
        /// </summary>
        /// <param name="point">Point</param>
        /// <param name="size">Size</param>
        /// <param name="rotation">Rotation as Double</param>
        /// <param name="sweep">Sweep as SweepDirection</param>
        /// <returns>ArcSegment</returns>
        public static ArcSegment GetArcSegment(Point point, Size size, Double rotation, SweepDirection sweep)
        {
            ArcSegment arcSegment = new ArcSegment();
            arcSegment.Point = point;
            arcSegment.Size = size;
            arcSegment.RotationAngle = 0;
            arcSegment.SweepDirection = SweepDirection.Clockwise;
            return arcSegment;
        }

        /// <summary>
        /// Convert scale based on the parameters
        /// </summary>
        /// <param name="fromScaleMin">FromScaleMin</param>
        /// <param name="fromScaleMax">FromScaleMax</param>
        /// <param name="fromValue">FromValue</param>
        /// <param name="toScaleMin">ToScaleMin</param>
        /// <param name="toScaleMax">ToScaleMax</param>
        /// <returns>Double</returns>
        public static Double ConvertScale(Double fromScaleMin, Double fromScaleMax, Double fromValue, Double toScaleMin, Double toScaleMax)
        {
            return ((fromValue - fromScaleMin) * (toScaleMax - toScaleMin) / (fromScaleMax - fromScaleMin)) + toScaleMin;
        }
        
       /// <summary>
        /// Converts value to pixel position
       /// </summary>
       /// <param name="positionMin">Min position</param>
       /// <param name="positionMax">Max position</param>
       /// <param name="valueMin">Min value</param>
       /// <param name="valueMax">Max value</param>
       /// <param name="value">Value</param>
       /// <returns>Double</returns>
        public static Double ValueToPixelPosition(Double positionMin, Double positionMax, Double valueMin, Double valueMax, Double value)
        {
            return ((value - valueMin) / (valueMax - valueMin)) * (positionMax - positionMin) + positionMin;
        }

        /// <summary>
        /// Converts pixel position to value
        /// </summary>
        /// <param name="positionMin">Min position</param>
        /// <param name="positionMax">Max position</param>
        /// <param name="valueMin">Min value</param>
        /// <param name="valueMax">Max value</param>
        /// <param name="position">position</param>
        /// <returns>Double</returns>
        public static Double PixelPositionToValue(Double positionMin, Double positionMax, Double valueMin, Double valueMax, Double position)
        {
            return ((position - positionMin) / (positionMax - positionMin) * (valueMax - valueMin)) + valueMin;
        }

        /// <summary>
        /// Creates and returns gradient stop
        /// </summary>
        /// <param name="color">Color as Color</param>
        /// <param name="stop">Stop as Double</param>
        /// <returns>GradientStop</returns>
        public static GradientStop GetGradientStop(Color color, Double stop)
        {
            GradientStop gradStop = new GradientStop();
            gradStop.Color = color;
            gradStop.Offset = stop;
            return gradStop;
        }

        /// <summary>
        /// Creates and returns a linear gradient brush
        /// </summary>
        /// <param name="angle">Angle</param>
        /// <param name="start">Start point</param>
        /// <param name="end">End point</param>
        /// <param name="colors">List of color</param>
        /// <param name="stops">List of Double</param>
        /// <returns>Brush</returns>
        public static Brush CreateLinearGradientBrush(Double angle, Point start, Point end, List<Color> colors, List<Double> stops)
        { 
            LinearGradientBrush brush = new LinearGradientBrush();
            if (colors.Count != stops.Count)
                throw new Exception("Colors and Stops arrays don't match");

            brush.StartPoint = start;
            brush.EndPoint = end;
            brush.GradientStops = new GradientStopCollection();

            for (Int32 i = 0; i < colors.Count; i++)
            {
                brush.GradientStops.Add(GetGradientStop(colors[i],stops[i]));
            }

            RotateTransform rt = new RotateTransform();
            rt.Angle = angle;
            rt.CenterX = 0.5;
            rt.CenterY = 0.5;
            brush.RelativeTransform = rt;

            return brush;
        }

        /// <summary>
        /// Creates and returns bevel top brush
        /// </summary>
        /// <param name="brush">Brush</param>
        /// <param name="angle">Angle</param>
        /// <returns>Brush</returns>
        internal static Brush GetBevelTopBrush(Brush brush, Double angle)
        {
            if (brush != null)
            {
                if (typeof(SolidColorBrush).Equals(brush.GetType()))
                {
                    SolidColorBrush solidBrush = brush as SolidColorBrush;
                    Double r, g, b;
                    List<Color> colors = new List<Color>();
                    List<Double> stops = new List<Double>();

                    r = ((double)solidBrush.Color.R / (double)255) * 0.9999;
                    g = ((double)solidBrush.Color.G / (double)255) * 0.9999;
                    b = ((double)solidBrush.Color.B / (double)255) * 0.9999;

                    colors.Add(Graphics.GetLighterColor(solidBrush.Color, 0.99));
                    stops.Add(0);

                    colors.Add(Graphics.GetLighterColor(solidBrush.Color, 1 - r, 1 - g, 1 - b));
                    stops.Add(0.2);

                    colors.Add(Graphics.GetLighterColor(solidBrush.Color, 1 - r, 1 - g, 1 - b));
                    stops.Add(0.6);

                    colors.Add(Graphics.GetLighterColor(solidBrush.Color, 0.99));
                    stops.Add(1);

                    return Graphics.CreateLinearGradientBrush(angle, new Point(0, 0.5), new Point(1, 0.5), colors, stops);
                }
                else
                {
                    return brush;
                }
            }
            else
                return null;
        }

        /// <summary>
        /// Creates and returns bevel side brush
        /// </summary>
        /// <param name="angle">Angle</param>
        /// <param name="brush">Brush</param>
        /// <returns>Brush</returns>
        internal static Brush GetBevelSideBrush(Double angle, Brush brush)
        {
            return Graphics.GetLightingEnabledBrush(brush, angle, "Linear", new Double[] { 0.75, 0.97});
        }

        /// <summary>
        /// Creates and returns a bevel top brush
        /// </summary>
        /// <param name="brush">Brush</param>
        /// <returns>Brush</returns>
        internal static Brush GetBevelTopBrush(Brush brush)
        {
            return GetBevelTopBrush(brush, 90);
        }

        /// <summary>
        /// Creates and returns a lighting brush
        /// </summary>
        /// <param name="brush">Brush</param>
        /// <returns>Brush</returns>
        internal static Brush GetLightingEnabledBrush3D(Brush brush)
        {
            return GetLightingEnabledBrush(brush, "Linear", new Double[] { 0.65, 0.55 });
        }

        /// <summary>
        /// Creates and returns a lighting brush
        /// </summary>
        /// <param name="brush">Brush</param>
        /// <param name="angle">Angle</param>
        /// <param name="type">Type as String</param>
        /// <param name="colorIntensies">Array of Double</param>
        /// <returns>Brush</returns>
        internal static Brush GetLightingEnabledBrush(Brush brush, Double angle, String type, Double[] colorIntensies)
        {
            if (brush != null)
            {
                if (typeof(SolidColorBrush).Equals(brush.GetType()))
                {
                    if (colorIntensies == null)
                        colorIntensies = new Double[] { 0.745, 0.99 };

                    SolidColorBrush solidBrush = brush as SolidColorBrush;

                    List<Color> colors = new List<Color>();
                    List<Double> stops = new List<Double>();

                    colors.Add(Graphics.GetDarkerColor(solidBrush.Color, colorIntensies[0]));
                    stops.Add(0);

                    colors.Add(Graphics.GetDarkerColor(solidBrush.Color, colorIntensies[1]));
                    stops.Add(1);

                    if (type == "Radial")
                        return Graphics.CreateRadialGradientBrush(colors, stops);
                    else
                        return Graphics.CreateLinearGradientBrush(angle, new Point(0, 0.5), new Point(1, 0.5), colors, stops);
                }
                else
                {   
                    return brush;
                }
            }
            else
            {
                return brush;
            }

        }
 
        /// <summary>
        /// Creates and returns a lighting brush
        /// </summary>
        /// <param name="brush">Brush</param>
        /// <param name="type">Type as String</param>
        /// <param name="colorIntensies">Array of Double</param>
        /// <returns>Brush</returns>
        internal static Brush GetLightingEnabledBrush(Brush brush, String type, Double[] colorIntensies)
        {
             return GetLightingEnabledBrush(brush, -90, type, colorIntensies);
        }
        
        /// <summary>
        /// Creates and returns a radial gradient brush
        /// </summary>
        /// <param name="colors">List of color</param>
        /// <param name="stops">List of Double</param>
        /// <returns>Brush</returns>
        public static Brush CreateRadialGradientBrush(List<Color> colors, List<Double> stops)
        {
            RadialGradientBrush brush = new RadialGradientBrush();
            if (colors.Count != stops.Count)
                throw new Exception("Colors and Stops arrays don't match");

            brush.GradientStops = new GradientStopCollection();

            for (Int32 i = 0; i < colors.Count; i++)
            {
                brush.GradientStops.Add(GetGradientStop(colors[i], stops[i]));
            }

            return brush;
        }

        /// <summary>
        /// Creates and returns a brush intensity
        /// </summary>
        /// <param name="brush">Brush</param>
        /// <returns>Double</returns>
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

        /// <summary>
        /// Creates and returns a default font color based on intensity
        /// </summary>
        /// <param name="intensity">Intensity as Double</param>
        /// <returns>Brush</returns>
        public static Brush GetDefaultFontColor(Double intensity)
        {
            Brush brush = null;
            if (intensity < 0.6)
            {
                brush = ParseSolidColor("#EFEFEF");
            }
            else
            {
                brush = ParseSolidColor("#000000");
            }
            return brush;
        }

        /// <summary>
        /// Compare brushes
        /// </summary>
        /// <param name="first">First brush</param>
        /// <param name="second">Second brush</param>
        /// <returns></returns>
        internal static bool AreBrushesEqual(Brush first, Brush second)
        {
            // If the default comparison is true, that's good enough.
            if (object.Equals(first, second))
            {
                return true;
            }

            // Do a field by field comparison if they're not the same reference
            SolidColorBrush firstSolidColorBrush = first as SolidColorBrush;
            if (firstSolidColorBrush != null)
            {
                SolidColorBrush secondSolidColorBrush = second as SolidColorBrush;
                if (secondSolidColorBrush != null)
                {
                    return object.Equals(firstSolidColorBrush.Color, secondSolidColorBrush.Color);
                }
            }

            return false;
        }

        /// <summary>
        /// Converts a color in String form to Solid Color Brush
        /// </summary>
        /// <param name="colorCode">ColorCode as String</param>
        /// <returns>Brush</returns>
        public static Brush ParseSolidColor(String colorCode)
        {
#if WPF
            return (Brush)XamlReader.Load(new XmlTextReader(new System.IO.StringReader(String.Format(System.Globalization.CultureInfo.InvariantCulture, @"<SolidColorBrush xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"" Color=""{0}""></SolidColorBrush>", colorCode))));
#else
            return (Brush)XamlReader.Load(String.Format(System.Globalization.CultureInfo.InvariantCulture, @"<SolidColorBrush xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"" xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"" Color=""{0}""></SolidColorBrush>", colorCode));
#endif
        }

        /// <summary>
        /// Returns a darker shade of the color by decreasing the brightness by the given intensity value
        /// </summary>
        /// <param name="color">Color as Color</param>
        /// <param name="intensity">Intensity as Double</param>
        /// <returns></returns>
        public static Color GetDarkerColor(Color color, Double intensity)
        {
            Color darkerShade = new Color();
            intensity = (intensity < 0 || intensity > 1) ? 1 : intensity;
            darkerShade.R = (Byte)(color.R * intensity);
            darkerShade.G = (Byte)(color.G * intensity);
            darkerShade.B = (Byte)(color.B * intensity);
            darkerShade.A = color.A;
            return darkerShade;
        }



        /// <summary>
        /// Returns a lighter shade of the color by increasing the brightness by the given intensity value
        /// </summary>
        /// <param name="color">Color as Color</param>
        /// <param name="intensity">Intensity as Double</param>
        /// <returns></returns>
        public static Color GetLighterColor(Color color, Double intensity)
        {
            Color lighterShade = new Color();
            intensity = (intensity < 0 || intensity > 1) ? 1 : intensity;
            lighterShade.R = (Byte)(256 - ((256 - color.R) * intensity));
            lighterShade.G = (Byte)(256 - ((256 - color.G) * intensity));
            lighterShade.B = (Byte)(256 - ((256 - color.B) * intensity));
            lighterShade.A = color.A;
            return lighterShade;
        }

        /// <summary>
        /// Creates and returns a lighter color based on the parameters
        /// </summary>
        /// <param name="color">Color as Color</param>
        /// <param name="intensityR">IntensityR as Double</param>
        /// <param name="intensityG">IntensityG as Double</param>
        /// <param name="intensityB">IntensityB as Double</param>
        /// <returns>Color</returns>
        public static Color GetLighterColor(Color color, Double intensityR, Double intensityG, Double intensityB)
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

        /// <summary>
        /// Converts LineStyle to StrokeDashArray
        /// </summary>
        /// <param name="lineStyle">lineStyle as String</param>
        /// <returns>DashArray as DoubleCollection</returns>
        public static DoubleCollection LineStyleToStrokeDashArray(String lineStyle)
        {
            DoubleCollection retVal = null;

            switch (lineStyle)
            {
                case "Solid":
                    retVal = null;
                    break;
                case "Dashed":
                    retVal = new DoubleCollection() { 4, 4, 4, 4 };
                    break;
                case "Dotted":
                    retVal = new DoubleCollection() { 1, 2, 1, 2 };
                    break;
            }

            return retVal;
        }

        /// <summary>
        /// Creates and returns a lighting brush
        /// </summary>
        /// <param name="lightingEnabled">Whether lighting is enabled</param>
        /// <returns>Brush</returns>
        public static Brush LightingBrush(Boolean lightingEnabled)
        {
            Brush brush;

            if (lightingEnabled)
            {   
                String xaml = String.Format(@"<LinearGradientBrush EndPoint=""0.5,1"" StartPoint=""0.5,0"" xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"">
                                                <GradientStop Color=""#A0FFFFFF"" Offset=""0""/>
                                                <GradientStop Color=""#00FFFFFF"" Offset=""1""/>
                                          </LinearGradientBrush>");

#if WPF
                brush = (Brush)XamlReader.Load(new XmlTextReader(new System.IO.StringReader(xaml)));
#else
                brush = System.Windows.Markup.XamlReader.Load(xaml) as Brush;
#endif
            }
            else
                brush = new SolidColorBrush(Colors.Transparent);

            return brush;
        }

        #endregion

        #region Constants

        /// <summary>
        /// Array of font sizes
        /// </summary>
        public static Double[] DefaultFontSizes = { 8, 10, 12, 14, 16, 18, 20, 24, 28, 32, 36, 40 };

        #endregion
    }
}



