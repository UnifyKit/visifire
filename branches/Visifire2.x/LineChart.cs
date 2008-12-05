#if WPF

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Markup;
using System.IO;
using System.Xml;
using System.Threading;
using System.Windows.Automation.Peers;
using System.Windows.Automation;
using System.Globalization;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.Windows.Media.Animation;
#else
using System;
using System.Windows;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Windows.Markup;
using System.Collections.ObjectModel;

#endif

using Visifire.Commons;

namespace Visifire.Charts
{
    internal class PolylineChartShapeParams
    {
        internal PointCollection Points { get; set; }
        internal Brush LineColor { get; set; }
        internal Double LineThickness { get; set; }
        internal Boolean Lighting { get; set; }
        internal DoubleCollection LineStyle {get;set;}
        internal Boolean ShadowEnabled { get; set; }
    }

    internal class LineChart
    {
        internal static Marker GetMarkerForDataPoint(Chart chart, Double position, DataPoint dataPoint, Boolean isPositive)
        {
            if ((Boolean)dataPoint.MarkerEnabled || (Boolean)dataPoint.LabelEnabled)
            {
                Size markerSize = (Boolean)dataPoint.MarkerEnabled ? new Size((Double)dataPoint.MarkerSize, (Double)dataPoint.MarkerSize) : new Size(0, 0);
                String labelText = (Boolean)dataPoint.LabelEnabled ? dataPoint.TextParser(dataPoint.LabelText) : "";
                Boolean markerBevel = false;
                dataPoint.Marker = new Marker((MarkerTypes)dataPoint.MarkerType, (Double)dataPoint.MarkerScale, markerSize, markerBevel, dataPoint.MarkerColor, labelText);

                dataPoint.Marker.MarkerSize = markerSize;
                dataPoint.Marker.BorderColor = dataPoint.MarkerBorderColor;
                dataPoint.Marker.BorderThickness = ((Thickness)dataPoint.MarkerBorderThickness).Left;
                dataPoint.Marker.FontColor = Graphics.ApplyLabelFontColor(chart, dataPoint, dataPoint.LabelFontColor, LabelStyles.OutSide);
                dataPoint.Marker.FontFamily = dataPoint.LabelFontFamily;
                dataPoint.Marker.FontSize = (Double)dataPoint.LabelFontSize;
                dataPoint.Marker.FontStyle = (FontStyle)dataPoint.LabelFontStyle;
                dataPoint.Marker.FontWeight = (FontWeight)dataPoint.LabelFontWeight;
                dataPoint.Marker.TextBackground = dataPoint.LabelBackground;
                dataPoint.Marker.MarkerFillColor = dataPoint.MarkerColor;

                if (true && !String.IsNullOrEmpty(labelText))
                {
                    dataPoint.Marker.CreateVisual();

                    dataPoint.Marker.TextAlignmentX = AlignmentX.Center;
                    if (isPositive)
                    {   
                        if (position < dataPoint.Marker.MarkerActualSize.Height)
                            dataPoint.Marker.TextAlignmentY = AlignmentY.Bottom;
                        else
                            dataPoint.Marker.TextAlignmentY = AlignmentY.Top;
                    }
                    else
                        if (position + dataPoint.Marker.MarkerActualSize.Height > chart.PlotArea.PlotAreaBorderElement.Height)
                            dataPoint.Marker.TextAlignmentY = AlignmentY.Top;
                        else
                            dataPoint.Marker.TextAlignmentY = AlignmentY.Bottom;
                }

                dataPoint.Marker.Chart = chart;

                dataPoint.Marker.CreateVisual();

                dataPoint.Marker.MarkerShape.MouseEnter += delegate(object sender, MouseEventArgs e)
                {
                    Shape shape = sender as Shape;
                    shape.Stroke = new SolidColorBrush(Colors.Red);
                    shape.StrokeThickness = dataPoint.Marker.BorderThickness;
                };

                dataPoint.Marker.MarkerShape.MouseLeave += delegate(object sender, MouseEventArgs e)
                {
                    Shape shape = sender as Shape;
                    shape.Stroke = dataPoint.Marker.BorderColor;
                    shape.StrokeThickness = dataPoint.Marker.BorderThickness;
                };

                // Visifire.Commons.ObservableObject.AttachEvents2Visual(dataPoint, dataPoint.Marker.Visual);

                return dataPoint.Marker;
            }
            return null;
        }

        internal static Canvas GetVisualObjectForLineChart(Double width, Double height, PlotDetails plotDetails, List<DataSeries> seriesList, Chart chart, Double plankDepth, bool animationEnabled)
        {
            if (Double.IsNaN(width) || Double.IsNaN(height) || width <= 0 || height <= 0) return null;

            Canvas visual = new Canvas();
            visual.Width = width;
            visual.Height = height;

            Canvas labelCanvas = new Canvas();
            labelCanvas.Width = width;
            labelCanvas.Height = height;

            Double depth3d = plankDepth / (plotDetails.Layer3DCount == 0 ? 1 : plotDetails.Layer3DCount) * (chart.View3D ? 1 : 0);
            Double visualOffset = depth3d * (plotDetails.SeriesDrawingIndex[seriesList[0]] + 1 - (plotDetails.Layer3DCount == 0 ? 0 : 1));
            visual.SetValue(Canvas.TopProperty, visualOffset);
            visual.SetValue(Canvas.LeftProperty, -visualOffset);

            foreach (DataSeries series in seriesList)
            {
                if (series.Enabled == false)
                    continue;

                Brush stroke = series.Color;

                PlotGroup plotGroup = series.PlotGroup;

                PointCollection pointsCollection = new PointCollection();
                foreach (DataPoint dataPoint in series.DataPoints)
                {
                    if (Double.IsNaN(dataPoint.YValue)|| (dataPoint.Enabled == false))
                    {
                        continue;
                    }

                    Double xPosition = Graphics.ValueToPixelPosition(0, width, (Double)plotGroup.AxisX.InternalAxisMinimum, (Double)plotGroup.AxisX.InternalAxisMaximum, dataPoint.XValue);
                    Double yPosition = Graphics.ValueToPixelPosition(height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, dataPoint.YValue);

                    pointsCollection.Add(new Point(xPosition, yPosition));

                    Marker marker = GetMarkerForDataPoint(chart, yPosition, dataPoint, dataPoint.YValue > 0);
                    if (marker != null)
                    {
                        marker.AddToParent(labelCanvas, xPosition, yPosition, new Point(0.5, 0.5));
                    }
                }

                series.Faces = new Faces();
                PolylineChartShapeParams lineParams = new PolylineChartShapeParams();

                lineParams.LineThickness = (Double)series.LineThickness;
                lineParams.LineColor = stroke;

                lineParams.LineStyle = series.GetDashArray(series.LineStyle);
                lineParams.Lighting = (Boolean)series.LightingEnabled;
                lineParams.ShadowEnabled = series.ShadowEnabled;

                lineParams.Points = pointsCollection;

                Canvas line2dCanvas = GetLine2D(lineParams);
                visual.Children.Add(line2dCanvas);

                series.Faces.Visual = line2dCanvas;
                series.Faces.LabelCanvas = labelCanvas;

                // Apply animation
                if (animationEnabled)
                {
                    if (series.Storyboard == null)
                        series.Storyboard = new Storyboard();

                    DataSeriesRef = series;

                    // Apply animation to the lines
                    series.Storyboard = ApplyLineChartAnimation(line2dCanvas, series.Storyboard, true);
                }
            }
            
            visual.Children.Add(labelCanvas);
            
            // If animation is not enabled or if there are no series in the serieslist the dont apply animation
            if (animationEnabled && seriesList.Count > 0)
            {
                //if (seriesList[0].Storyboard == null)
                //    seriesList[0].Storyboard = new Storyboard();
                // Apply animation to the label canvas
                DataSeriesRef = seriesList[0];

                seriesList[0].Storyboard = ApplyLineChartAnimation(labelCanvas, seriesList[0].Storyboard, false);
            }

            return visual;
        }

        private static Canvas GetLine2D(PolylineChartShapeParams lineParams)
        {
            Canvas visual = new Canvas();

            Polyline polyline = new Polyline();
            polyline.Points = lineParams.Points;
            polyline.Stroke = lineParams.Lighting ? GetLightingEnabledBrush(lineParams.LineColor) : lineParams.LineColor;
            polyline.StrokeThickness = lineParams.LineThickness;
            polyline.StrokeDashArray = lineParams.LineStyle;

            if (lineParams.ShadowEnabled)
            {
                Polyline polylineShadow = new Polyline();
                polylineShadow.Points = CloneCollection(lineParams.Points);
                polylineShadow.Stroke = GetShadowBrush();
                polylineShadow.StrokeThickness = lineParams.LineThickness;
                TranslateTransform tt = new TranslateTransform() { X = 2, Y = 2 };
                polylineShadow.RenderTransform = tt;
                polylineShadow.Opacity = 0.5;
                visual.Children.Add(polylineShadow);
            }

            visual.Children.Add(polyline);

            return visual;
        }

        private static Brush GetLightingEnabledBrush(Brush brush)
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

        
        private static Brush GetShadowBrush()
        {
            return new SolidColorBrush(Colors.LightGray);
        }

        private static PointCollection CloneCollection(PointCollection collection)
        {
            PointCollection newCollection = new PointCollection();
            foreach(Point value in collection)
                newCollection.Add(new Point(value.X,value.Y));

            return newCollection;
        }

        private static DoubleCollection GenerateDoubleCollection(params Double[] values)
        {
            DoubleCollection collection = new DoubleCollection();
            foreach (Double value in values)
                collection.Add(value);
            return collection;
        }

        private static List<KeySpline> GenerateKeySplineList(params Point[] values)
        {
            List<KeySpline> splines = new List<KeySpline>();
            for (Int32 i = 0; i < values.Length; i += 2)
                splines.Add(GetKeySpline(values[i], values[i + 1]));

            return splines;
        }

        private static KeySpline GetKeySpline(Point controlPoint1, Point controlPoint2)
        {
            return new KeySpline() { ControlPoint1 = controlPoint1, ControlPoint2 = controlPoint2 };
        }

        private static DoubleAnimationUsingKeyFrames CreateDoubleAnimation(DependencyObject target, String property, Double beginTime, DoubleCollection frameTime, DoubleCollection values, List<KeySpline> splines)
        {
            DoubleAnimationUsingKeyFrames da = new DoubleAnimationUsingKeyFrames();
#if WPF
            target.SetValue(FrameworkElement.NameProperty, target.GetType().Name + target.GetHashCode().ToString());
            Storyboard.SetTargetName(da, target.GetValue(FrameworkElement.NameProperty).ToString());

            DataSeriesRef.RegisterName((string)target.GetValue(FrameworkElement.NameProperty), target);
#else
            Storyboard.SetTarget(da, target);
#endif
            Storyboard.SetTargetProperty(da, new PropertyPath(property));

            da.BeginTime = TimeSpan.FromSeconds(beginTime);

            for (Int32 index = 0; index < splines.Count; index++)
            {
                SplineDoubleKeyFrame keyFrame = new SplineDoubleKeyFrame();
                keyFrame.KeySpline = splines[index];
                keyFrame.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(frameTime[index]));
                keyFrame.Value = values[index];
                da.KeyFrames.Add(keyFrame);
            }

            return da;
        }

        private static Storyboard ApplyLineChartAnimation(Panel canvas, Storyboard storyboard,Boolean isLineCanvas)
        {

            LinearGradientBrush opacityMaskBrush = new LinearGradientBrush(){StartPoint=new Point(0,0.5), EndPoint=new Point(1,0.5)};
            GradientStop GradStop1 = new GradientStop() { Color = Colors.White, Offset = 0 };
            GradientStop GradStop2 = new GradientStop() { Color = Colors.White, Offset = 0 };
            GradientStop GradStop3 = new GradientStop() { Color = Colors.Transparent, Offset = 0.01 };
            GradientStop GradStop4 = new GradientStop() { Color = Colors.Transparent, Offset = 1 };
            opacityMaskBrush.GradientStops.Add(GradStop1);
            opacityMaskBrush.GradientStops.Add(GradStop2);
            opacityMaskBrush.GradientStops.Add(GradStop3);
            opacityMaskBrush.GradientStops.Add(GradStop4);
            storyboard.Stop();
            canvas.OpacityMask = opacityMaskBrush;

            DoubleCollection values;
            DoubleCollection timeFrames;
            List<KeySpline> splines;

            if (isLineCanvas)
            {
                values = GenerateDoubleCollection(0, 1);
                timeFrames = GenerateDoubleCollection(0, 1);
                splines = GenerateKeySplineList(new Point(0, 0), new Point(1, 1), new Point(0, 0), new Point(1, 1));

                storyboard.Children.Add(CreateDoubleAnimation(GradStop2, "(GradientStop.Offset)", 0.25+0.5, timeFrames, values, splines));

                values = GenerateDoubleCollection(0.01, 1);
                timeFrames = GenerateDoubleCollection(0, 1);
                splines = GenerateKeySplineList(new Point(0, 0), new Point(1, 1), new Point(0, 0), new Point(1, 1));

                storyboard.Children.Add(CreateDoubleAnimation(GradStop3, "(GradientStop.Offset)", 0.25+0.5, timeFrames, values, splines));
            }
            else
            {
                values = GenerateDoubleCollection(0, 1);
                timeFrames = GenerateDoubleCollection(0, 1);
                splines = GenerateKeySplineList(new Point(0, 0), new Point(1, 1), new Point(0, 0), new Point(1, 1));

                storyboard.Children.Add(CreateDoubleAnimation(GradStop2, "(GradientStop.Offset)", 0.5, timeFrames, values, splines));

                values = GenerateDoubleCollection(0.01, 1);
                timeFrames = GenerateDoubleCollection(0, 1);
                splines = GenerateKeySplineList(new Point(0, 0), new Point(1, 1), new Point(0, 0), new Point(1, 1));

                storyboard.Children.Add(CreateDoubleAnimation(GradStop3, "(GradientStop.Offset)", 0.5, timeFrames, values, splines));
            }

            return storyboard;
        }

        private static DataSeries DataSeriesRef
        {
            get;
            set;
        }
    }
}
