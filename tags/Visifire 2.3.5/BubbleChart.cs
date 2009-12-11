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
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Animation;
#else
using System;
using System.Windows;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Collections.Generic;
#endif

using Visifire.Commons;

namespace Visifire.Charts
{
    /// <summary>
    /// Visifire.Charts.BubbleChart class
    /// </summary>
    internal class BubbleChart
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
       
        #endregion

        #region Private Delegates

        #endregion

        #region Private Methods

        /// <summary>
        /// Apply animation for bubble chart
        /// </summary>
        /// <param name="pointGrid">Bubble chart grid</param>
        /// <param name="storyboard">Stroyboard</param>
        /// <param name="width">Width of the chart canvas</param>
        /// <param name="height">Height of the chart canvas</param>
        /// <returns>Storyboard</returns>
        private static Storyboard ApplyBubbleChartAnimation(DataSeries currentDataSeries, Panel bubbleGrid, Storyboard storyboard, Double width, Double height)
        {
            TranslateTransform translateTransform = new TranslateTransform() { X = 0, Y = -height };
            bubbleGrid.RenderTransform = translateTransform;

            Random rand = new Random((Int32)DateTime.Now.Ticks);
            Double begin = rand.NextDouble();

            Double hPitchSize = width / 5;

            DoubleCollection times1 = Graphics.GenerateDoubleCollection(0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9, 1.0);
            DoubleCollection times2 = Graphics.GenerateDoubleCollection(0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9, 1.0);
            DoubleCollection translateXValues;
            DoubleCollection pitchFactors = Graphics.GenerateDoubleCollection(1, 15.0 / 23.0, 11.0 / 23.0, 7.0 / 23.0, 5.0 / 23.0, 3.0 / 23.0, 2.0 / 23.0, 1.0 / 23.0, 0.5 / 23.0, 0);
            translateXValues = new DoubleCollection();
            Double sign = 1;

            if (rand.NextDouble() > 0.5)
            {
                if ((Double)bubbleGrid.GetValue(Canvas.LeftProperty) > width / 2) sign = -1;
                else sign = 1;
            }
            else
            {
                if ((Double)bubbleGrid.GetValue(Canvas.LeftProperty) > width / 2) sign = 1;
                else sign = -1;
            }

            // Generate pitch values
            foreach (Double factor in pitchFactors)
                translateXValues.Add(sign * hPitchSize * factor);

            DoubleCollection translateYValues = Graphics.GenerateDoubleCollection(-height, 0, -height * 0.5, 0, -height * 0.25, 0, -height * 0.125, 0, -height * 0.0625, 0);
            List<KeySpline> splines3 = AnimationHelper.GenerateKeySplineList(
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

            List<KeySpline> splines4 = AnimationHelper.GenerateKeySplineList(
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

            DoubleAnimationUsingKeyFrames xTranslateAnimation = AnimationHelper.CreateDoubleAnimation(currentDataSeries, translateTransform, "(TranslateTransform.X)", begin * 0.5 + 0.5, times1, translateXValues, splines3);
            DoubleAnimationUsingKeyFrames yTranslateAnimation = AnimationHelper.CreateDoubleAnimation(currentDataSeries, translateTransform, "(TranslateTransform.Y)", begin * 0.5 + 0.5, times2, translateYValues, splines4);
            storyboard.Children.Add(xTranslateAnimation);
            storyboard.Children.Add(yTranslateAnimation);

            return storyboard;
        }
        
        #endregion

        #region Internal Methods

        /// <summary>
        /// Get visual object for bubble chart
        /// </summary>
        /// <param name="width">Width of the chart</param>
        /// <param name="height">Height of the chart</param>
        /// <param name="plotDetails">plotDetails</param>
        /// <param name="seriesList">List of DataSeries</param>
        /// <param name="chart">Chart</param>
        /// <param name="plankDepth">Plank depth</param>
        /// <param name="animationEnabled">Whether animation is enabled</param>
        /// <returns>Bubble chart canvas</returns>
        internal static Canvas GetVisualObjectForBubbleChart(Double width, Double height, PlotDetails plotDetails, List<DataSeries> seriesList, Chart chart, Double plankDepth, bool animationEnabled)
        {
            if (Double.IsNaN(width) || Double.IsNaN(height) || width <= 0 || height <= 0) return null;
            DataSeries currentDataSeries = null;

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

                var dataPointsList = (from dataPoint in series.InternalDataPoints where !Double.IsNaN(dataPoint.ZValue) && dataPoint.Enabled == true select dataPoint.ZValue);

                Double minValue = 0;
                Double maxValue = 1;

                if (dataPointsList.Count() > 0)
                {
                    minValue = dataPointsList.Min();
                    maxValue = dataPointsList.Max();
                }

                foreach (DataPoint dataPoint in series.InternalDataPoints)
                {
                    if (Double.IsNaN(dataPoint.InternalYValue) || (dataPoint.Enabled == false))
                    {
                        continue;
                    }
                    
                    Faces bubbleFaces = new Faces();
                    bubbleFaces.Parts = new List<FrameworkElement>();

                    Double xPosition = Graphics.ValueToPixelPosition(0, width, (Double)plotGroup.AxisX.InternalAxisMinimum, (Double)plotGroup.AxisX.InternalAxisMaximum, dataPoint.InternalXValue);
                    Double yPosition = Graphics.ValueToPixelPosition(height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, dataPoint.InternalYValue);

                    Brush markerColor = dataPoint.Color;
                    markerColor = (chart.View3D ? Graphics.GetLightingEnabledBrush3D(markerColor) : ((Boolean)dataPoint.LightingEnabled ? Graphics.GetLightingEnabledBrush(markerColor, "Linear", null) : markerColor));

                    Double value = !Double.IsNaN(dataPoint.ZValue) ? dataPoint.ZValue : (minValue + maxValue) / 2;

                    Double markerScale = Graphics.ConvertScale(minValue, maxValue, value, 1, (Double)dataPoint.MarkerScale);
                    Size markerSize = new Size((Double)dataPoint.MarkerSize, (Double)dataPoint.MarkerSize);

                    String labelText = (Boolean)dataPoint.LabelEnabled ? dataPoint.TextParser(dataPoint.LabelText) : "";

                    Boolean markerBevel = false;
                    Marker marker = new Marker((MarkerTypes)dataPoint.MarkerType, markerScale * (Double)dataPoint.MarkerScale, markerSize, markerBevel, markerColor, labelText);

                    marker.ShadowEnabled = dataPoint.Parent.ShadowEnabled;
                    marker.MarkerSize = new Size((Double)dataPoint.MarkerSize, (Double)dataPoint.MarkerSize);
                    if (dataPoint.BorderColor != null)
                        marker.BorderColor = dataPoint.BorderColor;
                    
                    marker.BorderThickness = ((Thickness)dataPoint.MarkerBorderThickness).Left;

                    marker.Tag = new ElementData() { Element = dataPoint };

                    Double gap = (markerScale * (Double)dataPoint.MarkerScale * (Double)dataPoint.MarkerSize) / 2;

                    if (!String.IsNullOrEmpty(labelText))
                    {
                        marker.FontColor = Chart.CalculateDataPointLabelFontColor(chart, dataPoint, dataPoint.LabelFontColor, LabelStyles.OutSide);
                        marker.FontSize = (Double)dataPoint.LabelFontSize;
                        marker.FontWeight = (FontWeight)dataPoint.LabelFontWeight;
                        marker.FontFamily = dataPoint.LabelFontFamily;
                        marker.FontStyle = (FontStyle)dataPoint.LabelFontStyle;
                        marker.TextBackground = dataPoint.LabelBackground;

                        marker.TextAlignmentX = AlignmentX.Center;
                        marker.TextAlignmentY = AlignmentY.Center;

                        if (!Double.IsNaN(dataPoint.LabelAngle) && dataPoint.LabelAngle != 0)
                        {
                            marker.LabelAngle = dataPoint.LabelAngle;
                            marker.TextOrientation = Orientation.Vertical;

                            marker.TextAlignmentX = AlignmentX.Center;
                            marker.TextAlignmentY = AlignmentY.Center;

                            marker.LabelStyle = (LabelStyles)dataPoint.LabelStyle;
                        }

                        marker.CreateVisual();

                        if (Double.IsNaN(dataPoint.LabelAngle) || dataPoint.LabelAngle == 0)
                        {
                            if (yPosition - gap < 0 && (yPosition - marker.TextBlockSize.Height / 2) < 0)
                                marker.TextAlignmentY = AlignmentY.Bottom;
                            else if (yPosition + gap > height && (yPosition + marker.TextBlockSize.Height / 2) > height)
                                marker.TextAlignmentY = AlignmentY.Top;

                            if (xPosition - gap < 0 && (xPosition - marker.TextBlockSize.Width / 2) < 0)
                                marker.TextAlignmentX = AlignmentX.Right;
                            else if (xPosition + gap > width && (xPosition + marker.TextBlockSize.Width / 2) > width)
                                marker.TextAlignmentX = AlignmentX.Left;
                        }
                    }

                    marker.CreateVisual();

                    marker.Visual.Opacity = dataPoint.InternalOpacity * dataPoint.Parent.InternalOpacity;

                    marker.AddToParent(visual, xPosition, yPosition, new Point(0.5, 0.5));

                    // Apply animation
                    if (animationEnabled)
                    {
                        if (dataPoint.Parent.Storyboard == null)
                            dataPoint.Parent.Storyboard = new Storyboard();

                        currentDataSeries = dataPoint.Parent;

                        // Apply animation to the bubbles
                        dataPoint.Parent.Storyboard = ApplyBubbleChartAnimation(currentDataSeries, marker.Visual, dataPoint.Parent.Storyboard, width, height);
                    }

                    bubbleFaces.Parts.Add(marker.MarkerShape);
                    bubbleFaces.VisualComponents.Add(marker.Visual);
                    bubbleFaces.BorderElements.Add(marker.MarkerShape);

                    bubbleFaces.Visual = marker.Visual;
                    dataPoint.Faces = bubbleFaces;
                }
            }

            RectangleGeometry clipRectangle = new RectangleGeometry();
            clipRectangle.Rect = new Rect(0, -chart.ChartArea.PLANK_DEPTH, width + chart.ChartArea.PLANK_OFFSET, height + chart.ChartArea.PLANK_DEPTH);
            visual.Clip = clipRectangle;

            return visual;
        }

        #endregion

        #region Internal Events And Delegates

        #endregion

        #region Data

        #endregion

    }
}
