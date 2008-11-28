﻿#if WPF

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

    internal class PointChart
    {
        internal static Canvas GetVisualObjectForPointChart(Double width, Double height, PlotDetails plotDetails, List<DataSeries> seriesList, Chart chart, Double plankDepth,bool animationEnabled)
        {
            if (Double.IsNaN(width) || Double.IsNaN(height) || width <= 0 || height <= 0) return null;

            Canvas visual = new Canvas();
            visual.Width = width;
            visual.Height = height;

            Double depth3d = plankDepth / (plotDetails.Layer3DCount == 0 ? 1 : plotDetails.Layer3DCount) * (chart.View3D ? 1 : 0);
            Double visualOffset = depth3d * (plotDetails.SeriesDrawingIndex[seriesList[0]] + 1 - (plotDetails.Layer3DCount == 0 ? 0 : 1));
            visual.SetValue(Canvas.TopProperty, visualOffset);
            visual.SetValue(Canvas.LeftProperty, -visualOffset);

            Random rand = new Random();

            foreach (DataSeries series in seriesList)
            {
                if (series.Enabled == false)
                    continue;

                PlotGroup plotGroup = series.PlotGroup;

                foreach (DataPoint dataPoint in series.DataPoints)
                {
                    if (Double.IsNaN(dataPoint.YValue)||(dataPoint.Enabled == false))
                    {
                        continue;
                    }

                    Double xPosition = Graphics.ValueToPixelPosition(0, width, (Double)plotGroup.AxisX.InternalAxisMinimum, (Double)plotGroup.AxisX.InternalAxisMaximum, dataPoint.XValue);
                    Double yPosition = Graphics.ValueToPixelPosition(height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, dataPoint.YValue);


                    Brush markerColor = dataPoint.Color;
                    markerColor = (chart.View3D ? GetLightingEnabledBrush3D(markerColor) :
                        ((Boolean)dataPoint.LightingEnabled ? GetLightingEnabledBrush(markerColor) : markerColor));

                    Size markerSize = new Size((Double)dataPoint.MarkerSize, (Double)dataPoint.MarkerSize);
                    Boolean markerBevel = false;
                    String labelText = (Boolean)dataPoint.LabelEnabled ? dataPoint.TextParser(dataPoint.LabelText) : "";
                    Marker marker = new Marker((MarkerTypes)dataPoint.MarkerType, (Double)dataPoint.MarkerScale, markerSize, markerBevel, markerColor, labelText);

                    marker.ShadowEnabled = dataPoint.Parent.ShadowEnabled;
                    marker.MarkerSize = new Size((Double)dataPoint.MarkerSize, (Double)dataPoint.MarkerSize);
                    if (marker.MarkerType != MarkerTypes.Cross)
                    {
                        if (dataPoint.BorderColor != null)
                            marker.BorderColor = dataPoint.BorderColor;
                    }
                    else
                        marker.BorderColor = markerColor;
                    marker.BorderThickness = ((Thickness)dataPoint.MarkerBorderThickness).Left;

                    marker.FontColor = Graphics.ApplyLabelFontColor(chart, dataPoint, dataPoint.LabelFontColor, LabelStyles.OutSide);
                    marker.FontSize = (Double)dataPoint.LabelFontSize;
                    marker.FontWeight = (FontWeight)dataPoint.LabelFontWeight;
                    marker.FontFamily = dataPoint.LabelFontFamily;
                    marker.FontStyle = (FontStyle)dataPoint.LabelFontStyle;

                    marker.TextAlignmentX = AlignmentX.Center;
                    marker.TextAlignmentY = AlignmentY.Center;

                    marker.CreateVisual();
                    marker.AddToParent(visual, xPosition, yPosition, new Point(0.5, 0.5));

                    // Apply animation
                    if (animationEnabled)
                    {
                        if (dataPoint.Parent.Storyboard == null)
                            dataPoint.Parent.Storyboard = new Storyboard();

                        DataSeriesRef = dataPoint.Parent;

                        // Apply animation to the points
                        dataPoint.Parent.Storyboard = ApplyPointChartAnimation(marker.Visual, dataPoint.Parent.Storyboard, width, height);
                    }

                    Faces point = new Faces();
                    point.VisualComponents.Add(marker.Visual);
                    point.Visual = marker.Visual;

                    dataPoint.Faces = point;
                }
            }

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
        private static Storyboard ApplyPointChartAnimation(Panel pointGrid, Storyboard storyboard,Double width,Double height)
        {
            if (storyboard != null)
                storyboard.Stop();

            TransformGroup group = new TransformGroup();
            ScaleTransform scaleTransform = new ScaleTransform() { ScaleX=0,ScaleY=0,CenterX=0.5,CenterY=0.5};
            TranslateTransform translateTransform = new TranslateTransform() { X = 0, Y = 0 };
            group.Children.Add(scaleTransform);
            group.Children.Add(translateTransform);

            pointGrid.RenderTransform = group;

            Random rand = new Random((Int32)DateTime.Now.Ticks);
            double begin = rand.NextDouble();

            pointGrid.Measure(new Size(Double.MaxValue, Double.MaxValue));

            DoubleCollection times = GenerateDoubleCollection(0, 0.5, 0.75, 1);
            DoubleCollection scaleValues = GenerateDoubleCollection(0, 1, 0.5, 1);
            DoubleCollection translateXValues = GenerateDoubleCollection(pointGrid.DesiredSize.Width / 2, 0, pointGrid.DesiredSize.Width/4, 0);
            DoubleCollection translateYValues = GenerateDoubleCollection(pointGrid.DesiredSize.Height / 2, 0, pointGrid.DesiredSize.Height / 4, 0);
            List<KeySpline> splines1 = GenerateKeySplineList(new Point(0, 0.5), new Point(0.5, 1), new Point(0, 0.5), new Point(0.5, 1), new Point(0, 0.5), new Point(0.5, 1), new Point(0, 0.5), new Point(0.5, 1));
            List<KeySpline> splines2 = GenerateKeySplineList(new Point(0, 0.5), new Point(0.5, 1), new Point(0, 0.5), new Point(0.5, 1), new Point(0, 0.5), new Point(0.5, 1), new Point(0, 0.5), new Point(0.5, 1));
            List<KeySpline> splines3 = GenerateKeySplineList(new Point(0, 0.5), new Point(0.5, 1), new Point(0, 0.5), new Point(0.5, 1), new Point(0, 0.5), new Point(0.5, 1), new Point(0, 0.5), new Point(0.5, 1));
            List<KeySpline> splines4 = GenerateKeySplineList(new Point(0, 0.5), new Point(0.5, 1), new Point(0, 0.5), new Point(0.5, 1), new Point(0, 0.5), new Point(0.5, 1), new Point(0, 0.5), new Point(0.5, 1));

            DoubleAnimationUsingKeyFrames xScaleAnimation = CreateDoubleAnimation(scaleTransform, "(ScaleTransform.ScaleX)", begin+0.5, times, scaleValues, splines1);
            DoubleAnimationUsingKeyFrames yScaleAnimation = CreateDoubleAnimation(scaleTransform, "(ScaleTransform.ScaleY)", begin+0.5, times, scaleValues, splines2);
            DoubleAnimationUsingKeyFrames xTranslateAnimation = CreateDoubleAnimation(translateTransform, "(TranslateTransform.X)", begin+0.5, times, translateXValues, splines3);
            DoubleAnimationUsingKeyFrames yTranslateAnimation = CreateDoubleAnimation(translateTransform, "(TranslateTransform.Y)", begin+0.5, times, translateYValues, splines4);

            storyboard.Children.Add(xScaleAnimation);
            storyboard.Children.Add(yScaleAnimation);
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