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

#if WPF
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Animation;
#else
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Collections.Generic;
#endif

using Visifire.Commons;

namespace Visifire.Charts
{
    /// <summary>
    /// Visifire.Charts.PointChart class
    /// </summary>
    internal class PointChart
    {
        #region Public Methods

        #endregion

        #region Public Properties

        #endregion

        #region Public Events And Delegates

        #endregion

        #region Protected Methods

        #endregion

        #region Internal Properties

        #endregion

        #region Private Properties

        /// <summary>
        /// Current working DataSeries
        /// </summary>
        private static DataSeries CurrentDataSeries
        {
            get;
            set;
        }

        #endregion

        #region Private Delegates

        #endregion

        #region Private Methods

        /// <summary>
        /// Apply animation for point chart
        /// </summary>
        /// <param name="pointGrid">Point chart grid</param>
        /// <param name="storyboard">Stroyboard</param>
        /// <param name="width">Width of the chart canvas</param>
        /// <param name="height">Height of the chart canvas</param>
        /// <returns>Storyboard</returns>
        private static Storyboard ApplyPointChartAnimation(Panel pointGrid, Storyboard storyboard, Double width, Double height)
        {
#if WPF
            if (storyboard != null && storyboard.GetValue(System.Windows.Media.Animation.Storyboard.TargetProperty) != null)
                storyboard.Stop();
#else
            if (storyboard != null)
                storyboard.Stop();
#endif

            TransformGroup group = new TransformGroup();
            ScaleTransform scaleTransform = new ScaleTransform() { ScaleX = 0, ScaleY = 0, CenterX = 0.5, CenterY = 0.5 };
            TranslateTransform translateTransform = new TranslateTransform() { X = 0, Y = 0 };
            group.Children.Add(scaleTransform);
            group.Children.Add(translateTransform);

            pointGrid.RenderTransform = group;

            Random rand = new Random((Int32)DateTime.Now.Ticks);
            double begin = rand.NextDouble();

            pointGrid.Measure(new Size(Double.MaxValue, Double.MaxValue));

            DoubleCollection times = Graphics.GenerateDoubleCollection(0, 0.5, 0.75, 1);
            DoubleCollection scaleValues = Graphics.GenerateDoubleCollection(0, 1, 0.5, 1);
            DoubleCollection translateXValues = Graphics.GenerateDoubleCollection(pointGrid.DesiredSize.Width / 2, 0, pointGrid.DesiredSize.Width / 4, 0);
            DoubleCollection translateYValues = Graphics.GenerateDoubleCollection(pointGrid.DesiredSize.Height / 2, 0, pointGrid.DesiredSize.Height / 4, 0);
            List<KeySpline> splines1 = AnimationHelper.GenerateKeySplineList(new Point(0, 0.5), new Point(0.5, 1), new Point(0, 0.5), new Point(0.5, 1), new Point(0, 0.5), new Point(0.5, 1), new Point(0, 0.5), new Point(0.5, 1));
            List<KeySpline> splines2 = AnimationHelper.GenerateKeySplineList(new Point(0, 0.5), new Point(0.5, 1), new Point(0, 0.5), new Point(0.5, 1), new Point(0, 0.5), new Point(0.5, 1), new Point(0, 0.5), new Point(0.5, 1));
            List<KeySpline> splines3 = AnimationHelper.GenerateKeySplineList(new Point(0, 0.5), new Point(0.5, 1), new Point(0, 0.5), new Point(0.5, 1), new Point(0, 0.5), new Point(0.5, 1), new Point(0, 0.5), new Point(0.5, 1));
            List<KeySpline> splines4 = AnimationHelper.GenerateKeySplineList(new Point(0, 0.5), new Point(0.5, 1), new Point(0, 0.5), new Point(0.5, 1), new Point(0, 0.5), new Point(0.5, 1), new Point(0, 0.5), new Point(0.5, 1));

            DoubleAnimationUsingKeyFrames xScaleAnimation = AnimationHelper.CreateDoubleAnimation(CurrentDataSeries, scaleTransform, "(ScaleTransform.ScaleX)", begin + 0.5, times, scaleValues, splines1);
            DoubleAnimationUsingKeyFrames yScaleAnimation = AnimationHelper.CreateDoubleAnimation(CurrentDataSeries, scaleTransform, "(ScaleTransform.ScaleY)", begin + 0.5, times, scaleValues, splines2);
            DoubleAnimationUsingKeyFrames xTranslateAnimation = AnimationHelper.CreateDoubleAnimation(CurrentDataSeries, translateTransform, "(TranslateTransform.X)", begin + 0.5, times, translateXValues, splines3);
            DoubleAnimationUsingKeyFrames yTranslateAnimation = AnimationHelper.CreateDoubleAnimation(CurrentDataSeries, translateTransform, "(TranslateTransform.Y)", begin + 0.5, times, translateYValues, splines4);

            storyboard.Children.Add(xScaleAnimation);
            storyboard.Children.Add(yScaleAnimation);
            storyboard.Children.Add(xTranslateAnimation);
            storyboard.Children.Add(yTranslateAnimation);

            return storyboard;
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Get visual object for point chart
        /// </summary>
        /// <param name="width">Width of the charat</param>
        /// <param name="height">Height of the charat</param>
        /// <param name="plotDetails">plotDetails</param>
        /// <param name="seriesList">List of DataSeries</param>
        /// <param name="chart">Chart</param>
        /// <param name="plankDepth">Plank depth</param>
        /// <param name="animationEnabled">Whether animation is enabled</param>
        /// <returns>Point chart canvas</returns>
        internal static Canvas GetVisualObjectForPointChart(Double width, Double height, PlotDetails plotDetails, List<DataSeries> seriesList, Chart chart, Double plankDepth, bool animationEnabled)
        {
            if (Double.IsNaN(width) || Double.IsNaN(height) || width <= 0 || height <= 0) return null;

            Canvas visual = new Canvas() { Width = width, Height = height };

            Double depth3d = plankDepth / (plotDetails.Layer3DCount == 0 ? 1 : plotDetails.Layer3DCount) * (chart.View3D ? 1 : 0);
            Double visualOffset = depth3d * (plotDetails.SeriesDrawingIndex[seriesList[0]] + 1 - (plotDetails.Layer3DCount == 0 ? 0 : 1));
            visual.SetValue(Canvas.TopProperty, visualOffset);
            visual.SetValue(Canvas.LeftProperty, -visualOffset);

            foreach (DataSeries series in seriesList)
            {
                if (series.Enabled == false)
                    continue;

                PlotGroup plotGroup = series.PlotGroup;

                foreach (DataPoint dataPoint in series.InternalDataPoints)
                {
                    if (Double.IsNaN(dataPoint.InternalYValue) || (dataPoint.Enabled == false))
                    {
                        continue;
                    }

                    Double xPosition = Graphics.ValueToPixelPosition(0, width, (Double)plotGroup.AxisX.InternalAxisMinimum, (Double)plotGroup.AxisX.InternalAxisMaximum, dataPoint.InternalXValue);
                    Double yPosition = Graphics.ValueToPixelPosition(height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, dataPoint.InternalYValue);


                    Brush markerColor = dataPoint.Color;
                    markerColor = (chart.View3D ? Graphics.GetLightingEnabledBrush3D(markerColor) :
                        ((Boolean)dataPoint.LightingEnabled ? Graphics.GetLightingEnabledBrush(markerColor, "Linear", null) : markerColor));

                    Size markerSize = new Size((Double)dataPoint.MarkerSize, (Double)dataPoint.MarkerSize);
                    Boolean markerBevel = false;
                    String labelText = (Boolean)dataPoint.LabelEnabled ? dataPoint.TextParser(dataPoint.LabelText) : "";
                    Marker marker = new Marker((MarkerTypes)dataPoint.MarkerType, (Double)dataPoint.MarkerScale, markerSize, markerBevel, markerColor, labelText);

                    marker.Tag = new ElementData() { Element = dataPoint };
                    
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

                    marker.FontColor = Chart.CalculateDataPointLabelFontColor(chart, dataPoint, dataPoint.LabelFontColor, LabelStyles.OutSide);
                    marker.FontSize = (Double)dataPoint.LabelFontSize;
                    marker.FontWeight = (FontWeight)dataPoint.LabelFontWeight;
                    marker.FontFamily = dataPoint.LabelFontFamily;
                    marker.FontStyle = (FontStyle)dataPoint.LabelFontStyle;

                    marker.TextAlignmentX = AlignmentX.Center;
                    marker.TextAlignmentY = AlignmentY.Center;

                    marker.CreateVisual();

                    marker.Visual.Opacity = dataPoint.Opacity * dataPoint.Parent.Opacity;

                    marker.AddToParent(visual, xPosition, yPosition, new Point(0.5, 0.5));

                    // Apply animation
                    if (animationEnabled)
                    {
                        if (dataPoint.Parent.Storyboard == null)
                            dataPoint.Parent.Storyboard = new Storyboard();

                        CurrentDataSeries = dataPoint.Parent;

                        // Apply animation to the points
                        dataPoint.Parent.Storyboard = ApplyPointChartAnimation(marker.Visual, dataPoint.Parent.Storyboard, width, height);
                    }

                    Faces point = new Faces();
                    point.VisualComponents.Add(marker.Visual);
                    point.Visual = marker.Visual;

                    point.BorderElements.Add(marker.MarkerShape);

                    dataPoint.Marker = marker;
                    dataPoint.Faces = point;
                }
            }

            return visual;
        }

        #endregion

        #region Internal Events And Delegates

        #endregion

        #region Data

        #endregion
    }
}