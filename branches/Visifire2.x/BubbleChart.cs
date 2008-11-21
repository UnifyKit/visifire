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
    internal class BubbleChart
    {   
        internal static Canvas GetVisualObjectForBubbleChart(Double width, Double height, PlotDetails plotDetails, List<DataSeries> seriesList, Chart chart, Double plankDepth, bool animationEnabled)
        {
            if (Double.IsNaN(width) || Double.IsNaN(height) || width <= 0 || height <= 0) return null;
            
            Canvas visual = new Canvas();
            visual.Width = width;
            visual.Height = height;

            Double depth3d = plankDepth / (plotDetails.Layer3DCount == 0 ? 1 : plotDetails.Layer3DCount) * (chart.View3D ? 1 : 0);
            Double visualOffset = depth3d * (plotDetails.SeriesDrawingIndex[seriesList[0]] + 1 - (plotDetails.Layer3DCount == 0 ? 0 : 1));
            visual.SetValue(Canvas.TopProperty, visualOffset);
            visual.SetValue(Canvas.LeftProperty, -visualOffset);

            foreach (DataSeries series in seriesList)
            {
                if (series.Enabled == false)
                    continue;

                PlotGroup plotGroup = series.PlotGroup;

                var dataPointsList = (from dataPoint in series.DataPoints where !Double.IsNaN(dataPoint.ZValue) && dataPoint.Enabled==true select dataPoint.ZValue);

                Double minValue = 0;
                Double maxValue = 1;

                if (dataPointsList.Count() > 0)
                {
                    minValue = dataPointsList.Min();
                    maxValue = dataPointsList.Max();
                }

                foreach (DataPoint dataPoint in series.DataPoints)
                {
                    if (Double.IsNaN(dataPoint.YValue)||(dataPoint.Enabled == false))
                    {
                        continue;
                    }

                    Double xPosition = Graphics.ValueToPixelPosition(0, width, (Double)plotGroup.AxisX.InternalAxisMinimum, (Double)plotGroup.AxisX.InternalAxisMaximum, dataPoint.XValue);
                    Double yPosition = Graphics.ValueToPixelPosition(height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, dataPoint.YValue);
                    
                    Brush markerColor = dataPoint.Color;
                    markerColor = (chart.View3D ? GetLightingEnabledBrush3D(markerColor) : ((Boolean)dataPoint.LightingEnabled ? GetLightingEnabledBrush(markerColor) : markerColor));

                    Double value = !Double.IsNaN(dataPoint.ZValue) ? dataPoint.ZValue : (minValue + maxValue) / 2;
                    
                    Double markerScale = Graphics.ConvertScale(minValue, maxValue, value, 1, (Double)dataPoint.MarkerScale);

                    //((chart as Chart).ActualWidth > (chart as Chart).ActualHeight ? (chart as Chart).ActualHeight 
                    //    : (chart as Chart).ActualWidth) * 0.05, ((chart as Chart).ActualWidth > (chart as Chart).ActualHeight
                    //    ? (chart as Chart).ActualWidth : (chart as Chart).ActualHeight) * 1 / 5
                    Size markerSize = new Size((Double)dataPoint.MarkerSize, (Double)dataPoint.MarkerSize);

                    String labelText = (Boolean)dataPoint.LabelEnabled ? dataPoint.TextParser(dataPoint.LabelText) : "";
                    Boolean markerBevel = false;
                    Marker marker = new Marker((MarkerTypes)dataPoint.MarkerType, markerScale * (Double)dataPoint.MarkerScale, markerSize, markerBevel, markerColor, labelText);

                    marker.ShadowEnabled = dataPoint.Parent.ShadowEnabled;
                    marker.MarkerSize = new Size((Double)dataPoint.MarkerSize, (Double)dataPoint.MarkerSize);
                    if(dataPoint.BorderColor != null)
                        marker.BorderColor = dataPoint.BorderColor;
                    marker.BorderThickness = ((Thickness)dataPoint.MarkerBorderThickness).Left;

                    marker.FontColor = Graphics.ApplyLabelFontColor(chart, dataPoint, dataPoint.LabelFontColor, LabelStyles.OutSide);
                    marker.FontSize = (Double)dataPoint.LabelFontSize;
                    marker.FontWeight = (FontWeight)dataPoint.LabelFontWeight;
                    marker.FontFamily = dataPoint.LabelFontFamily;
                    marker.FontStyle = (FontStyle)dataPoint.LabelFontStyle;

                    marker.TextAlignmentX = AlignmentX.Center;
                    marker.TextAlignmentY = AlignmentY.Center;

                    marker.CreateVisual();

                    Double gap = (markerScale * (Double)dataPoint.MarkerScale * (Double)dataPoint.MarkerSize) / 2;
                    
                    if (yPosition - gap < 0 && (yPosition - marker.TextBlockSize.Height/2) < 0)
                        marker.TextAlignmentY = AlignmentY.Bottom;
                    else if (yPosition + gap > height && (yPosition + marker.TextBlockSize.Height / 2) > height)
                        marker.TextAlignmentY = AlignmentY.Top;

                    if (xPosition - gap < 0 && (xPosition - marker.TextBlockSize.Width/2)<0)
                        marker.TextAlignmentX = AlignmentX.Right;
                    else if (xPosition + gap > width && (xPosition + marker.TextBlockSize.Width / 2) >width)
                        marker.TextAlignmentX = AlignmentX.Left;

                    marker.CreateVisual();
                    marker.AddToParent(visual, xPosition, yPosition, new Point(0.5, 0.5));

                    // Apply animation
                    if (animationEnabled)
                    {
                        if (dataPoint.Parent.Storyboard == null)
                            dataPoint.Parent.Storyboard = new Storyboard();

                        DataSeriesRef = dataPoint.Parent;

                        // Apply animation to the bubbles
                        dataPoint.Parent.Storyboard = ApplyBubbleChartAnimation(marker.Visual, dataPoint.Parent.Storyboard, width, height);
                    }

                    Faces bubble = new Faces();
                    bubble.VisualComponents.Add(marker.Visual);
                    bubble.Visual = marker.Visual;
                    dataPoint.Faces = bubble;
                }
            }

            RectangleGeometry clipRectangle = new RectangleGeometry();
            clipRectangle.Rect = new Rect(0, 0, width, height);
            visual.Clip = clipRectangle;

            return visual;
        }   

        private static Brush GetLightingEnabledBrush(Brush brush)
        {
            if (typeof(SolidColorBrush).Equals(brush.GetType()))
            {
                SolidColorBrush solidBrush = brush as SolidColorBrush;

                List<Color> colors = new List<Color>();
                List<Double> stops = new List<Double>();

                colors.Add(Graphics.GetDarkerColor(solidBrush.Color, 0.745));
                stops.Add(0);

                colors.Add(Graphics.GetDarkerColor(solidBrush.Color, 0.99));
                stops.Add(1);


                return Graphics.CreateLinearGradientBrush(-90, new Point(0, 0.5), new Point(1, 0.5), colors, stops);
            }
            else
            {
                return brush;
            }
        }
        private static Brush GetLightingEnabledBrush3D(Brush brush)
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

        private static Storyboard ApplyBubbleChartAnimation(Panel pointGrid, Storyboard storyboard, Double width, Double height)
        {
            TranslateTransform translateTransform = new TranslateTransform() { X = 0, Y = -height };
            pointGrid.RenderTransform = translateTransform;

            Random rand = new Random((Int32)DateTime.Now.Ticks);
            double begin = rand.NextDouble();

            Double hPitchSize = width / 5;

            DoubleCollection times1 = GenerateDoubleCollection(0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9, 1.0);
            DoubleCollection times2 = GenerateDoubleCollection(0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9, 1.0);
            DoubleCollection translateXValues;
            DoubleCollection pitchFactors = GenerateDoubleCollection(1, 15.0 / 23.0, 11.0 / 23.0, 7.0 / 23.0, 5.0 / 23.0, 3.0 / 23.0, 2.0 / 23.0, 1.0 / 23.0, 0.5 / 23.0, 0);
            translateXValues = new DoubleCollection();
            Double sign = 1;

            if (rand.NextDouble() > 0.5)
            {
                if ((Double)pointGrid.GetValue(Canvas.LeftProperty) > width / 2) sign = -1;
                else sign = 1;
            }
            else
            {
                if ((Double)pointGrid.GetValue(Canvas.LeftProperty) > width / 2) sign = 1;
                else sign = -1;
            }

            // Generate pitch values
            foreach (Double factor in pitchFactors)
                translateXValues.Add(sign * hPitchSize * factor);

            DoubleCollection translateYValues = GenerateDoubleCollection(-height, 0, -height * 0.5, 0, -height * 0.25, 0,-height*0.125,0,-height*0.0625,0);
            List<KeySpline> splines3 = GenerateKeySplineList(
                new Point(0, 0), new Point(1, 1),
                new Point(0, 0), new Point(1, 1),
                new Point(0, 0), new Point(1, 1),
                new Point(0, 0), new Point(1, 1),
                new Point(0, 0), new Point(1, 1),
                new Point(0, 0), new Point(1, 1),
                new Point(0, 0), new Point(1, 1),
                new Point(0, 0), new Point(1, 1),
                new Point(0, 0), new Point(1, 1),
                new Point(0, 0), new Point(1, 1));

            List<KeySpline> splines4 = GenerateKeySplineList(
                new Point(0, 0), new Point(1, 1),
                new Point(0.5, 0), new Point(1, 1),
                new Point(0, 0), new Point(0.5, 1),
                new Point(0.5, 0), new Point(1, 1),
                new Point(0, 0), new Point(0.5, 1),
                new Point(0.5, 0), new Point(1, 1),
                new Point(0, 0), new Point(0.5, 1),
                new Point(0.5, 0), new Point(1, 1),
                new Point(0, 0), new Point(0.5, 1),
                new Point(0.5, 0), new Point(1, 1));

            DoubleAnimationUsingKeyFrames xTranslateAnimation = CreateDoubleAnimation(translateTransform, "(TranslateTransform.X)", begin*0.5+0.5, times1, translateXValues, splines3);
            DoubleAnimationUsingKeyFrames yTranslateAnimation = CreateDoubleAnimation(translateTransform, "(TranslateTransform.Y)", begin*0.5+0.5, times2, translateYValues, splines4);
            storyboard.Stop();
            storyboard.Children.Add(xTranslateAnimation);
            storyboard.Children.Add(yTranslateAnimation);

            return storyboard;
        }

        private static DataSeries DataSeriesRef
        {
            get;
            set;
        }
    }
}
