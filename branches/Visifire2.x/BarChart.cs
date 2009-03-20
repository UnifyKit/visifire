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
    /// Visifire.Charts.BarChart class
    /// </summary>
    internal class BarChart
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
        /// Set position of the marker
        /// </summary>
        /// <param name="barParams">Bar parameters</param>
        /// <param name="chart">Chart</param>
        /// <param name="dataPoint">DataPoint</param>
        /// <param name="labelText">label text</param>
        /// <param name="markerSize">Size of the marker</param>
        /// <param name="canvasLeft">Left position of marker canvas</param>
        /// <param name="canvasTop">Top position of marker canvas</param>
        /// <param name="markerPosition">Position of the Marker</param>
        private static void SetMarkerPosition(RectangularChartShapeParams barParams, Chart chart, DataPoint dataPoint, String labelText, Size markerSize, Double canvasLeft, Double canvasTop, Point markerPosition)
        {
            if (barParams.IsLabelEnabled && !String.IsNullOrEmpty(labelText))
            {
                dataPoint.Marker.CreateVisual();

                if (barParams.IsPositive)
                {
                    if (canvasLeft + markerPosition.X + dataPoint.Marker.MarkerActualSize.Width > chart.PlotArea.BorderElement.Width)
                        barParams.LabelStyle = LabelStyles.Inside;
                }
                else
                {
                    if (canvasLeft < dataPoint.Marker.MarkerActualSize.Width)
                        barParams.LabelStyle = LabelStyles.Inside;
                }

                dataPoint.Marker.TextAlignmentY = AlignmentY.Center;

                if (!barParams.IsMarkerEnabled)
                {
                    if (chart.View3D)
                    {
                        if (barParams.LabelStyle == LabelStyles.OutSide)
                            dataPoint.Marker.MarkerSize = new Size(markerSize.Width + chart.ChartArea.PLANK_DEPTH, markerSize.Height + chart.ChartArea.PLANK_DEPTH);
                        else
                            dataPoint.Marker.MarkerSize = new Size(markerSize.Width, markerSize.Height);
                    }
                }
                else
                {
                    if (chart.View3D)
                    {
                        barParams.LabelStyle = LabelStyles.Inside;
                    }
                }

                if (barParams.IsPositive)
                    dataPoint.Marker.TextAlignmentX = barParams.LabelStyle == LabelStyles.Inside ? AlignmentX.Left : AlignmentX.Right;
                else
                    dataPoint.Marker.TextAlignmentX = barParams.LabelStyle == LabelStyles.Inside ? AlignmentX.Right : AlignmentX.Left;

            }

        }
      
        /// <summary>
        /// Calculate height of each bar
        /// </summary>
        /// <param name="top">Top position</param>
        /// <param name="heightPerBar">Height of a bar</param>
        /// <param name="height">Height of chart canvas</param>
        /// <returns>Final height of each bar</returns>
        private static Double CalculateHeightOfEachColumn(ref Double top, Double heightPerBar, Double height)
        {
            Double finalHeight = heightPerBar;
            Double minPosValue = 0;
            Double maxPosValue = height;

            if (top < minPosValue)
            {
                finalHeight = top + heightPerBar - minPosValue;
                top = minPosValue;
            }
            else if (top + heightPerBar > maxPosValue)
            {
                finalHeight = maxPosValue - top;
            }

            return (finalHeight < 2.5) ? 2.5 : finalHeight;
        }

        /// <summary>
        /// Get Bar Z-Index
        /// </summary>
        /// <param name="left">Left position</param>
        /// <param name="top">Top position</param>
        /// <param name="top">Actual height of chart canvas</param>
        /// <param name="isPositive">Whether DataPoint Value is positive or negative</param>
        /// <returns>Zindex as Int32</returns>
        private static Int32 GetBarZIndex(Double left, Double top, Double height, Boolean isPositive)
        {
            Int32 yOffset = (Int32)(height - top);
            Int32 zindex = (Int32)(Math.Sqrt(Math.Pow(left / 2, 2) + Math.Pow(yOffset, 2)));
            if (isPositive)
                return zindex;
            else
                return Int32.MinValue + zindex;
        }

        /// <summary>
        /// Get ZIndex for StackedBar visual
        /// </summary>
        /// <param name="left">Left position</param>
        /// <param name="top">Top position</param>
        /// <param name="top">Actual Width of chart canvas</param>
        /// <param name="top">Actual Height of chart canvas</param>
        /// <param name="isPositive">Whether DataPoint value is negative or positive</param>
        /// <returns>Zindex as Int32</returns>
        private static Int32 GetStackedBarZIndex(Double left, Double top, Double width, Double height, Boolean isPositive)
        {
            Double zOffset = Math.Pow(10, (Int32)(Math.Log10(width) - 1));
            Int32 iOffset = (Int32)(left / (zOffset < 1 ? 1 : zOffset));
            Int32 zindex = (Int32)((top) * zOffset) + iOffset;
            if (isPositive)
                return zindex;
            else
                return Int32.MinValue + zindex;
        }

        /// <summary>
        /// Apply bar chart animation
        /// </summary>
        /// <param name="bar">Bar visual reference</param>
        /// <param name="storyboard">Storyboard</param>
        /// <param name="barParams">Bar parameters</param>
        /// <returns>Storyboard</returns>
        private static Storyboard ApplyBarChartAnimation(Panel bar, Storyboard storyboard, RectangularChartShapeParams barParams)
        {
            ScaleTransform scaleTransform = new ScaleTransform() { ScaleX = 0 };
            bar.RenderTransform = scaleTransform;

            if (barParams.IsPositive)
            {
                bar.RenderTransformOrigin = new Point(0, 0.5);
            }
            else
            {
                bar.RenderTransformOrigin = new Point(1, 0.5);
            }
            DoubleCollection values = Graphics.GenerateDoubleCollection(0, 1);
            DoubleCollection frameTimes = Graphics.GenerateDoubleCollection(0, 0.75);
            List<KeySpline> splines = AnimationHelper.GenerateKeySplineList
                (
                new Point(0, 0), new Point(1, 1),
                new Point(0, 0), new Point(0.5, 1)
                );

            DoubleAnimationUsingKeyFrames growAnimation = AnimationHelper.CreateDoubleAnimation(CurrentDataSeries, scaleTransform, "(ScaleTransform.ScaleX)", 0.5, frameTimes, values, splines);

            storyboard.Children.Add(growAnimation);

            return storyboard;
        }

        /// <summary>
        /// Apply animation for StackedBar chart
        /// </summary>
        /// <param name="bar">Bar visual reference</param>
        /// <param name="storyboard">Storyboard</param>
        /// <param name="barParams">Bar params</param>
        /// <param name="begin">Animation begin time</param>
        /// <param name="duration">Animation duration</param>
        /// <returns>Storyboard</returns>
        private static Storyboard ApplyStackedBarChartAnimation(Panel bar, Storyboard storyboard, RectangularChartShapeParams barParams, Double begin, Double duration)
        {
            ScaleTransform scaleTransform = new ScaleTransform() { ScaleX = 0 };
            bar.RenderTransform = scaleTransform;

            bar.RenderTransformOrigin = new Point(0.5, 0.5);

            DoubleCollection values = Graphics.GenerateDoubleCollection(0, 1.5, 0.75, 1.125, 0.9325, 1);
            DoubleCollection frameTimes = Graphics.GenerateDoubleCollection(0, 0.25 * duration, 0.5 * duration, 0.75 * duration, 1.0 * duration, 1.25 * duration);
            List<KeySpline> splines = AnimationHelper.GenerateKeySplineList
                (
                new Point(0, 0), new Point(1, 0.5),
                new Point(0, 0), new Point(0.5, 1),
                new Point(0, 0), new Point(1, 0.5),
                new Point(0, 0), new Point(0.5, 1),
                new Point(0, 0), new Point(1, 0.5),
                new Point(0, 0), new Point(0.5, 1)
                );

            DoubleAnimationUsingKeyFrames growAnimation = AnimationHelper.CreateDoubleAnimation(CurrentDataSeries, scaleTransform, "(ScaleTransform.ScaleX)", begin + 0.5, frameTimes, values, splines);
            storyboard.Children.Add(growAnimation);
            return storyboard;
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Returns marker for DataPoint
        /// </summary>
        /// <param name="chart">Chart</param>
        /// <param name="barParams">Bar parameters</param>
        /// <param name="dataPoint">DataPoint</param>
        /// <param name="left">Left position of MarkerCanvas</param>
        /// <param name="top">Top position</param>
        /// <returns>Marker canvas</returns>
        internal static Canvas GetMarker(Chart chart, RectangularChartShapeParams barParams, DataPoint dataPoint, Double left, Double top)
        {
            Canvas markerCanvas = new Canvas();
            markerCanvas.Width = barParams.Size.Width;
            markerCanvas.Height = barParams.Size.Height;
            markerCanvas.SetValue(Canvas.LeftProperty, left);
            markerCanvas.SetValue(Canvas.TopProperty, top);

            if (barParams.IsMarkerEnabled || barParams.IsLabelEnabled)
            {
                Size markerSize = new Size(barParams.MarkerSize, barParams.MarkerSize);
                String labelText = barParams.IsLabelEnabled ? barParams.LabelText : "";

                if (!barParams.IsMarkerEnabled)
                {
                    barParams.MarkerColor = new SolidColorBrush(Colors.Transparent);
                    barParams.MarkerBorderColor = new SolidColorBrush(Colors.Transparent);
                }

                dataPoint.Marker = ColumnChart.CreateNewMarker(barParams, dataPoint, markerSize, labelText);

                Point markerPosition = new Point();

                if (barParams.IsPositive)
                    if (chart.View3D)
                        markerPosition = new Point(barParams.Size.Width + barParams.Depth, barParams.Size.Height / 2 - barParams.Depth);
                    else
                        markerPosition = new Point(barParams.Size.Width, barParams.Size.Height / 2);
                else
                    if (chart.View3D)
                        markerPosition = new Point(barParams.Depth, barParams.Size.Height / 2 - barParams.Depth);
                    else
                        markerPosition = new Point(0, barParams.Size.Height / 2);

                SetMarkerPosition(barParams, chart, dataPoint, labelText, markerSize, left, top, markerPosition);

                barParams.LabelFontColor = Chart.CalculateDataPointLabelFontColor(chart, dataPoint, dataPoint.LabelFontColor,(dataPoint.YValue == 0)? LabelStyles.OutSide : (LabelStyles)barParams.LabelStyle);
                dataPoint.Marker.FontColor = barParams.LabelFontColor;

                dataPoint.Marker.CreateVisual();

                dataPoint.Marker.AddToParent(markerCanvas, markerPosition.X, markerPosition.Y, new Point(0.5, 0.5));
            }

            return markerCanvas;
        }

        /// <summary>
        /// Get visual object for bar chart
        /// </summary>
        /// <param name="width">Width of the PlotArea</param>
        /// <param name="height">Height of the PlotArea</param>
        /// <param name="plotDetails">PlotDetails</param>
        /// <param name="dataSeriesList4Rendering">DataSeries list</param>
        /// <param name="chart">Chart</param>
        /// <param name="plankDepth">PlankDepth</param>
        /// <param name="animationEnabled">Whether animation is enabled</param>
        /// <returns>Bar chart canvas</returns>
        internal static Canvas GetVisualObjectForBarChart(Double width, Double height, PlotDetails plotDetails, List<DataSeries> dataSeriesList4Rendering, Chart chart, Double plankDepth, bool animationEnabled)
        {
            if (Double.IsNaN(width) || Double.IsNaN(height) || width <= 0 || height <= 0) return null;

            Dictionary<Double, SortDataPoints> sortedDataPoints = null;

            sortedDataPoints = plotDetails.GetDataPointsGroupedByXValue(RenderAs.Bar);

            List<Double> xValues = sortedDataPoints.Keys.ToList();

            Canvas visual = new Canvas() { Width = width, Height = height };
            Canvas labelCanvas = new Canvas() { Width = width, Height = height };
            Canvas columnCanvas = new Canvas() { Width = width, Height = height };

            List<PlotGroup> plotGroupList = (from plots in plotDetails.PlotGroups where plots.RenderAs == RenderAs.Bar select plots).ToList();

            Double depth3d = plankDepth / plotDetails.Layer3DCount * (chart.View3D ? 1 : 0);
            Double visualOffset = depth3d * (plotDetails.SeriesDrawingIndex[plotGroupList[0].DataSeriesList[0]] + 1);
            visual.SetValue(Canvas.TopProperty, visualOffset);
            visual.SetValue(Canvas.LeftProperty, -visualOffset);
            labelCanvas.SetValue(Canvas.TopProperty, visualOffset);
            labelCanvas.SetValue(Canvas.LeftProperty, -visualOffset);

            Double minDiffValue = plotDetails.GetMinOfMinDifferencesForXValue(RenderAs.Bar, RenderAs.StackedBar, RenderAs.StackedBar100);

            Axis AxesXwithMinInterval = dataSeriesList4Rendering[0].PlotGroup.AxisX;

            minDiffValue = (minDiffValue < (Double)AxesXwithMinInterval.InternalInterval) ? minDiffValue : (Double)AxesXwithMinInterval.InternalInterval;

            Double dataAxisDifference = Math.Abs((Double)AxesXwithMinInterval.InternalAxisMinimum - (Double)AxesXwithMinInterval.Minimum) * 2;
            Double dataMinimumGap = Graphics.ValueToPixelPosition(0, height, (Double)AxesXwithMinInterval.InternalAxisMinimum, (Double)AxesXwithMinInterval.InternalAxisMaximum, dataAxisDifference + (Double)AxesXwithMinInterval.InternalAxisMinimum);
            Double minDiffGap = Graphics.ValueToPixelPosition(0, height, (Double)AxesXwithMinInterval.InternalAxisMinimum, (Double)AxesXwithMinInterval.InternalAxisMaximum, minDiffValue + (Double)AxesXwithMinInterval.InternalAxisMinimum);

            if (dataMinimumGap > 0 && minDiffGap > 0)
                minDiffGap = Math.Min(minDiffGap, dataMinimumGap);
            else
                minDiffGap = Math.Max(minDiffGap, dataMinimumGap);

            Double maxColumnHeight = minDiffGap * (1 - BAR_GAP_RATIO);
            Double numberOfDivisions = plotDetails.GetMaxDivision(sortedDataPoints);
            Double heightPerBar = maxColumnHeight / numberOfDivisions;
            Boolean plankDrawn = false;

            foreach (Double xValue in xValues)
            {
                RectangularChartShapeParams barParams = new RectangularChartShapeParams();
                barParams.ShadowOffset = 5;
                barParams.Depth = depth3d;

                foreach (DataPoint dataPoint in sortedDataPoints[xValue].Positive)
                {
                    ColumnChart.SetColumnParms(ref barParams, ref chart, dataPoint, true);
                    barParams.XRadius = new CornerRadius(0, dataPoint.RadiusX.Value.TopRight, dataPoint.RadiusX.Value.BottomRight, 0);
                    barParams.YRadius = new CornerRadius(0, dataPoint.RadiusY.Value.TopRight, dataPoint.RadiusY.Value.BottomRight, 0);
                    PlotGroup plotGroup = dataPoint.Parent.PlotGroup;
                    Double limitingYValue = 0;

                    if (plotGroup.AxisY.InternalAxisMinimum > 0)
                        limitingYValue = (Double)plotGroup.AxisY.InternalAxisMinimum;
                    if (plotGroup.AxisY.InternalAxisMaximum < 0)
                        limitingYValue = (Double)plotGroup.AxisY.InternalAxisMaximum;

                    List<DataSeries> indexSeriesList = plotDetails.GetSeriesFromDataPoint(dataPoint);
                    Int32 drawingIndex = indexSeriesList.IndexOf(dataPoint.Parent);

                    Double top = Graphics.ValueToPixelPosition(height, 0, (Double)plotGroup.AxisX.InternalAxisMinimum, (Double)plotGroup.AxisX.InternalAxisMaximum, xValue);
                    top = top + ((Double)drawingIndex - (Double)indexSeriesList.Count() / (Double)2) * heightPerBar;
                    Double left = Graphics.ValueToPixelPosition(0, width, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, limitingYValue);
                    Double right = Graphics.ValueToPixelPosition(0, width, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, dataPoint.InternalYValue);
                    Double columnWidth = Math.Abs(left - right);

                    Double finalHeight = CalculateHeightOfEachColumn(ref top, heightPerBar, height);

                    if (finalHeight < 0)
                        continue;

                    barParams.Size = new Size(columnWidth, finalHeight);

                    Faces column;
                    Panel columnVisual = null;

                    if (chart.View3D)
                    {
                        column = Get3DBar(barParams);
                        columnVisual = column.Visual;
                        columnVisual.SetValue(Canvas.ZIndexProperty, GetBarZIndex(left, top, height, dataPoint.InternalYValue > 0));
                    }
                    else
                    {
                        column = Get2DBar(barParams);
                        columnVisual = column.Visual;
                    }

                    dataPoint.Faces = column;
                    dataPoint.Faces.LabelCanvas = labelCanvas;

                    columnVisual.SetValue(Canvas.LeftProperty, left);
                    columnVisual.SetValue(Canvas.TopProperty, top);

                    columnCanvas.Children.Add(columnVisual);

                    labelCanvas.Children.Add(GetMarker(chart, barParams, dataPoint, left, top));
                    dataPoint.Faces.LabelCanvas = labelCanvas;

                    // Apply animation
                    if (animationEnabled)
                    {
                        if (dataPoint.Parent.Storyboard == null)
                            dataPoint.Parent.Storyboard = new Storyboard();

                        CurrentDataSeries = dataPoint.Parent;

                        // Apply animation to the bars 
                        dataPoint.Parent.Storyboard = ApplyBarChartAnimation(columnVisual, dataPoint.Parent.Storyboard, barParams);

                        // Apply animation to the marker and labels
                        dataPoint.Parent.Storyboard = AnimationHelper.ApplyOpacityAnimation(dataPoint.Marker, CurrentDataSeries, dataPoint.Parent.Storyboard, 1, dataPoint.Opacity * dataPoint.Parent.Opacity);
                    }
                }

                foreach (DataPoint dataPoint in sortedDataPoints[xValue].Negative)
                {
                    ColumnChart.SetColumnParms(ref barParams, ref chart, dataPoint, false);

                    barParams.XRadius = new CornerRadius(dataPoint.RadiusX.Value.TopLeft, 0, 0, dataPoint.RadiusX.Value.BottomLeft);
                    barParams.YRadius = new CornerRadius(dataPoint.RadiusY.Value.TopLeft, 0, 0, dataPoint.RadiusY.Value.BottomLeft);

                    PlotGroup plotGroup = dataPoint.Parent.PlotGroup;

                    Double limitingYValue = 0;
                    if (plotGroup.AxisY.InternalAxisMinimum > 0)
                        limitingYValue = (Double)plotGroup.AxisY.InternalAxisMinimum;
                    if (plotGroup.AxisY.InternalAxisMaximum < 0)
                        limitingYValue = (Double)plotGroup.AxisY.InternalAxisMaximum;


                    List<DataSeries> indexSeriesList = plotDetails.GetSeriesFromDataPoint(dataPoint);
                    Int32 drawingIndex = indexSeriesList.IndexOf(dataPoint.Parent);

                    Double top = Graphics.ValueToPixelPosition(height, 0, (Double)plotGroup.AxisX.InternalAxisMinimum, (Double)plotGroup.AxisX.InternalAxisMaximum, xValue);
                    top = top + ((Double)drawingIndex - (Double)indexSeriesList.Count() / (Double)2) * heightPerBar;
                    Double right = Graphics.ValueToPixelPosition(0, width, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, limitingYValue);
                    Double left = Graphics.ValueToPixelPosition(0, width, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, dataPoint.InternalYValue);
                    Double columnWidth = Math.Abs(right - left);

                    Double finalHeight = CalculateHeightOfEachColumn(ref top, heightPerBar, height);

                    if (finalHeight < 0)
                        continue;

                    barParams.Size = new Size(columnWidth, finalHeight);

                    Faces column;
                    Panel columnVisual = null;

                    if (chart.View3D)
                    {
                        column = Get3DBar(barParams);
                        columnVisual = column.Visual;
                        columnVisual.SetValue(Canvas.ZIndexProperty, GetBarZIndex(left, top, height, dataPoint.InternalYValue > 0));
                    }
                    else
                    {
                        column = Get2DBar(barParams);
                        columnVisual = column.Visual;
                    }

                    dataPoint.Faces = column;
                    dataPoint.Faces.LabelCanvas = labelCanvas;

                    columnVisual.SetValue(Canvas.LeftProperty, left);
                    columnVisual.SetValue(Canvas.TopProperty, top);

                    columnCanvas.Children.Add(columnVisual);

                    labelCanvas.Children.Add(GetMarker(chart, barParams, dataPoint, left, top));
                    dataPoint.Faces.LabelCanvas = labelCanvas;

                    // Apply animation
                    if (animationEnabled)
                    {
                        if (dataPoint.Parent.Storyboard == null)
                            dataPoint.Parent.Storyboard = new Storyboard();

                        CurrentDataSeries = dataPoint.Parent;

                        // Apply animation to the bars 
                        dataPoint.Parent.Storyboard = ApplyBarChartAnimation(columnVisual, dataPoint.Parent.Storyboard, barParams);

                        // Apply animation to the marker and labels
                        dataPoint.Parent.Storyboard = AnimationHelper.ApplyOpacityAnimation(dataPoint.Marker, CurrentDataSeries, dataPoint.Parent.Storyboard, 1, dataPoint.Opacity * dataPoint.Parent.Opacity);
                    }
                }
            }
            if (!plankDrawn && chart.View3D && dataSeriesList4Rendering[0].PlotGroup.AxisY.InternalAxisMinimum < 0 && dataSeriesList4Rendering[0].PlotGroup.AxisY.InternalAxisMaximum > 0)
            {
                RectangularChartShapeParams barParams = new RectangularChartShapeParams();
                barParams.BackgroundBrush = new SolidColorBrush(Color.FromArgb((Byte)255, (Byte)127, (Byte)127, (Byte)127));
                barParams.Lighting = true;
                barParams.Size = new Size(1, height);
                barParams.Depth = depth3d;

                Faces zeroPlank = ColumnChart.Get3DColumn(barParams);
                Panel zeroPlankVisual = zeroPlank.Visual;

                Double left = Graphics.ValueToPixelPosition(0, width, (Double)dataSeriesList4Rendering[0].PlotGroup.AxisY.InternalAxisMinimum, (Double)dataSeriesList4Rendering[0].PlotGroup.AxisY.InternalAxisMaximum, 0);
                zeroPlankVisual.SetValue(Canvas.LeftProperty, left);
                zeroPlankVisual.SetValue(Canvas.TopProperty, (Double)0);
                zeroPlankVisual.SetValue(Canvas.ZIndexProperty, 0);
                zeroPlankVisual.Opacity = 0.7;
                columnCanvas.Children.Add(zeroPlankVisual);
            }

            visual.Children.Add(columnCanvas);
            visual.Children.Add(labelCanvas);

            return visual;
        }

        /// <summary>
        /// Get visual object for StackedBar chart
        /// </summary>
        /// <param name="width">Width of the PlotArea</param>
        /// <param name="height">Height of the PlotArea</param>
        /// <param name="plotDetails">PlotDetails</param>
        /// <param name="dataSeriesList4Rendering">DataSeries list</param>
        /// <param name="chart">Chart</param>
        /// <param name="plankDepth">PlankDepth</param>
        /// <param name="animationEnabled">Whether animation is enabled</param>
        /// <returns>StackedBar chart canvas</returns>
        internal static Canvas GetVisualObjectForStackedBarChart(Double width, Double height, PlotDetails plotDetails, Chart chart, Double plankDepth, bool animationEnabled)
        {
            if (Double.IsNaN(width) || Double.IsNaN(height) || width <= 0 || height <= 0) return null;

            List<PlotGroup> plotGroupList = (from plots in plotDetails.PlotGroups where plots.RenderAs == RenderAs.StackedBar select plots).ToList();

            Double numberOfDivisions = plotDetails.DrawingDivisionFactor;
            Boolean plankDrawn = false;

            Canvas visual = new Canvas() { Width = width, Height = height };
            Canvas labelCanvas = new Canvas() { Width = width, Height = height };
            Canvas columnCanvas = new Canvas() { Width = width, Height = height };

            Double depth3d = plankDepth / plotDetails.Layer3DCount * (chart.View3D ? 1 : 0);
            Double visualOffset = depth3d * (plotDetails.SeriesDrawingIndex[plotGroupList[0].DataSeriesList[0]] + 1);
            visual.SetValue(Canvas.TopProperty, visualOffset);
            visual.SetValue(Canvas.LeftProperty, -visualOffset);

            List<DataSeries> seriesList = plotDetails.GetSeriesListByRenderAs(RenderAs.StackedBar);
            Dictionary<Axis, Dictionary<Axis, Int32>> seriesIndex = ColumnChart.GetSeriesIndex(seriesList);
            Int32 index = 1;

            foreach (PlotGroup plotGroup in plotGroupList)
            {
                if (!seriesIndex.ContainsKey(plotGroup.AxisY))
                    continue;

                Int32 drawingIndex = seriesIndex[plotGroup.AxisY][plotGroup.AxisX];
                Double minDiff = plotDetails.GetMinOfMinDifferencesForXValue(RenderAs.Bar, RenderAs.StackedBar, RenderAs.StackedBar100);

                minDiff = (minDiff < (Double)plotGroup.AxisX.InternalInterval) ? minDiff : (Double)plotGroup.AxisX.InternalInterval;

                Double maxBarHeight = Graphics.ValueToPixelPosition(0, height, (Double)plotGroup.AxisX.InternalAxisMinimum, (Double)plotGroup.AxisX.InternalAxisMaximum, minDiff + (Double)plotGroup.AxisX.InternalAxisMinimum) * (1 - BAR_GAP_RATIO);
                Double heightPerBar = maxBarHeight / numberOfDivisions;

                List<Double> xValuesList = plotGroup.XWiseStackedDataList.Keys.ToList();

                Double limitingYValue = 0;
                if (plotGroup.AxisY.InternalAxisMinimum > 0)
                    limitingYValue = (Double)plotGroup.AxisY.InternalAxisMinimum;
                if (plotGroup.AxisY.InternalAxisMaximum < 0)
                    limitingYValue = (Double)plotGroup.AxisY.InternalAxisMaximum;

                index++;

                foreach (Double xValue in xValuesList)
                {
                    RectangularChartShapeParams barParams = new RectangularChartShapeParams();
                    barParams.ShadowOffset = 5;
                    barParams.Depth = depth3d;
                    barParams.IsStacked = true;

                    Double top = Graphics.ValueToPixelPosition(height, 0, (Double)plotGroup.AxisX.InternalAxisMinimum, (Double)plotGroup.AxisX.InternalAxisMaximum, xValue) + drawingIndex * heightPerBar - (maxBarHeight / 2);
                    Double left = Graphics.ValueToPixelPosition(0, width, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, limitingYValue);

                    Double finalHeight = CalculateHeightOfEachColumn(ref top, heightPerBar, height);

                    if (finalHeight < 0)
                        continue;

                    Double right;
                    Double barWidth;
                    Double prevSum = 0;

                    // Plot positive values
                    foreach (DataPoint dataPoint in plotGroup.XWiseStackedDataList[xValue].Positive)
                    {
                        if (!(Boolean)dataPoint.Enabled || Double.IsNaN(dataPoint.InternalYValue))
                            continue;

                        ColumnChart.SetColumnParms(ref barParams, ref chart, dataPoint, true);

                        barParams.IsTopOfStack = (dataPoint == plotGroup.XWiseStackedDataList[xValue].Positive.Last());
                        if (barParams.IsTopOfStack)
                        {
                            barParams.XRadius = new CornerRadius(0, dataPoint.RadiusX.Value.TopRight, dataPoint.RadiusX.Value.BottomRight, 0);
                            barParams.YRadius = new CornerRadius(0, dataPoint.RadiusY.Value.TopRight, dataPoint.RadiusY.Value.BottomRight, 0);
                        }

                        right = Graphics.ValueToPixelPosition(0, width, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, dataPoint.InternalYValue + prevSum);
                        barWidth = Math.Abs(right - left);

                        prevSum += dataPoint.InternalYValue;

                        barParams.Size = new Size(barWidth, finalHeight);

                        Faces bar;
                        Panel barVisual = null;

                        if (chart.View3D)
                        {
                            bar = Get3DBar(barParams);
                            barVisual = bar.Visual;
                            barVisual.SetValue(Canvas.ZIndexProperty, GetStackedBarZIndex(left, top, width, height, (dataPoint.InternalYValue > 0)));
                        }
                        else
                        {
                            bar = Get2DBar(barParams);
                            barVisual = bar.Visual;
                        }

                        dataPoint.Faces = bar;
                        dataPoint.Faces.LabelCanvas = labelCanvas;

                        barVisual.SetValue(Canvas.LeftProperty, left);
                        barVisual.SetValue(Canvas.TopProperty, top);

                        columnCanvas.Children.Add(barVisual);
                        labelCanvas.Children.Add(GetMarker(chart, barParams, dataPoint, left, top));

                        // Apply animation
                        if (animationEnabled)
                        {
                            if (dataPoint.Parent.Storyboard == null)
                                dataPoint.Parent.Storyboard = new Storyboard();

                            CurrentDataSeries = dataPoint.Parent;

                            // Apply animation to the data points dataSeriesIndex.e to the rectangles that form the columns
                            dataPoint.Parent.Storyboard = ApplyStackedBarChartAnimation(barVisual, dataPoint.Parent.Storyboard, barParams, (1.0 / seriesList.Count) * (Double)(seriesList.IndexOf(dataPoint.Parent)), 1.0 / seriesList.Count);

                            // Apply animation to the marker and labels
                            dataPoint.Parent.Storyboard = AnimationHelper.ApplyOpacityAnimation(dataPoint.Marker, CurrentDataSeries, dataPoint.Parent.Storyboard, 1, dataPoint.Opacity * dataPoint.Parent.Opacity);
                        }

                        left = right;
                    }

                    prevSum = 0;
                    right = Graphics.ValueToPixelPosition(0, width, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, limitingYValue);

                    // Plot negative values
                    foreach (DataPoint dataPoint in plotGroup.XWiseStackedDataList[xValue].Negative)
                    {
                        if (!(Boolean)dataPoint.Enabled || Double.IsNaN(dataPoint.InternalYValue))
                            continue;

                        ColumnChart.SetColumnParms(ref barParams, ref chart, dataPoint, false);

                        barParams.IsTopOfStack = (dataPoint == plotGroup.XWiseStackedDataList[xValue].Negative.Last());
                        if (barParams.IsTopOfStack)
                        {
                            barParams.XRadius = new CornerRadius(dataPoint.RadiusX.Value.TopLeft, 0, 0, dataPoint.RadiusX.Value.BottomLeft);
                            barParams.YRadius = new CornerRadius(dataPoint.RadiusY.Value.TopLeft, 0, 0, dataPoint.RadiusY.Value.BottomLeft);
                        }

                        left = Graphics.ValueToPixelPosition(0, width, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, dataPoint.InternalYValue + prevSum);
                        barWidth = Math.Abs(right - left);

                        prevSum += dataPoint.InternalYValue;
                        barParams.Size = new Size(barWidth, finalHeight);

                        Faces bar;
                        Panel barVisual = null;

                        if (chart.View3D)
                        {
                            bar = Get3DBar(barParams);
                            barVisual = bar.Visual;
                            barVisual.SetValue(Canvas.ZIndexProperty, GetStackedBarZIndex(left, top, width, height, (dataPoint.InternalYValue > 0)));
                        }
                        else
                        {
                            bar = Get2DBar(barParams);
                            barVisual = bar.Visual;
                        }

                        dataPoint.Faces = bar;
                        dataPoint.Faces.LabelCanvas = labelCanvas;

                        barVisual.SetValue(Canvas.LeftProperty, left);
                        barVisual.SetValue(Canvas.TopProperty, top);

                        columnCanvas.Children.Add(barVisual);
                        labelCanvas.Children.Add(GetMarker(chart, barParams, dataPoint, left, top));

                        // Apply animation
                        if (animationEnabled)
                        {
                            if (dataPoint.Parent.Storyboard == null)
                                dataPoint.Parent.Storyboard = new Storyboard();

                            CurrentDataSeries = dataPoint.Parent;

                            // Apply animation to the data points dataSeriesIndex.e to the rectangles that form the columns
                            dataPoint.Parent.Storyboard = ApplyStackedBarChartAnimation(barVisual, dataPoint.Parent.Storyboard, barParams, (1.0 / seriesList.Count) * (Double)(seriesList.IndexOf(dataPoint.Parent)), 1.0 / seriesList.Count);

                            // Apply animation to the marker and labels
                            dataPoint.Parent.Storyboard = AnimationHelper.ApplyOpacityAnimation(dataPoint.Marker, CurrentDataSeries, dataPoint.Parent.Storyboard, 1, dataPoint.Opacity * dataPoint.Parent.Opacity);
                        }

                        right = left;
                    }
                }
            }
            if (!plankDrawn && chart.View3D && plotGroupList[0].AxisY.InternalAxisMinimum < 0 && plotGroupList[0].AxisY.InternalAxisMaximum > 0)
            {
                RectangularChartShapeParams barParams = new RectangularChartShapeParams();
                barParams.BackgroundBrush = new SolidColorBrush(Color.FromArgb((Byte)255, (Byte)127, (Byte)127, (Byte)127));
                barParams.Lighting = true;
                barParams.Size = new Size(1, height);
                barParams.Depth = depth3d;

                Faces zeroPlank = ColumnChart.Get3DColumn(barParams);
                Panel zeroPlankVisual = zeroPlank.Visual;

                Double left = Graphics.ValueToPixelPosition(0, width, (Double)plotGroupList[0].AxisY.InternalAxisMinimum, (Double)plotGroupList[0].AxisY.InternalAxisMaximum, 0);
                zeroPlankVisual.SetValue(Canvas.LeftProperty, left);
                zeroPlankVisual.SetValue(Canvas.TopProperty, (Double)0);
                zeroPlankVisual.SetValue(Canvas.ZIndexProperty, 0);
                zeroPlankVisual.Opacity = 0.7;
                columnCanvas.Children.Add(zeroPlankVisual);
            }
            visual.Children.Add(columnCanvas);
            visual.Children.Add(labelCanvas);
            return visual;
        }

        /// <summary>
        /// Get visual object for StackedBar100 chart
        /// </summary>
        /// <param name="width">Width of the PlotArea</param>
        /// <param name="height">Height of the PlotArea</param>
        /// <param name="plotDetails">PlotDetails</param>
        /// <param name="chart">Chart</param>
        /// <param name="plankDepth">PlankDepth</param>
        /// <param name="animationEnabled">Whether animation is enabled for chart</param>
        /// <returns>StackedBar100 chart Canvas</returns>
        internal static Canvas GetVisualObjectForStackedBar100Chart(Double width, Double height, PlotDetails plotDetails, Chart chart, Double plankDepth, Boolean animationEnabled)
        {
            if (Double.IsNaN(width) || Double.IsNaN(height) || width <= 0 || height <= 0) return null;

            List<PlotGroup> plotGroupList = (from plots in plotDetails.PlotGroups where plots.RenderAs == RenderAs.StackedBar100 select plots).ToList();

            Double numberOfDivisions = plotDetails.DrawingDivisionFactor;

            Boolean plankDrawn = false;
            Canvas visual = new Canvas() { Width = width, Height = height };
            Canvas labelCanvas = new Canvas() { Width = width, Height = height };
            Canvas columnCanvas = new Canvas() { Width = width, Height = height };

            Double depth3d = plankDepth / plotDetails.Layer3DCount * (chart.View3D ? 1 : 0);
            Double visualOffset = depth3d * (plotDetails.SeriesDrawingIndex[plotGroupList[0].DataSeriesList[0]] + 1);
            visual.SetValue(Canvas.TopProperty, visualOffset);
            visual.SetValue(Canvas.LeftProperty, -visualOffset);

            List<DataSeries> seriesList = plotDetails.GetSeriesListByRenderAs(RenderAs.StackedBar100);
            Dictionary<Axis, Dictionary<Axis, Int32>> seriesIndex = ColumnChart.GetSeriesIndex(seriesList);

            foreach (PlotGroup plotGroup in plotGroupList)
            {
                if (!seriesIndex.ContainsKey(plotGroup.AxisY))
                    continue;

                Int32 drawingIndex = seriesIndex[plotGroup.AxisY][plotGroup.AxisX];

                Double minDiff = plotDetails.GetMinOfMinDifferencesForXValue(RenderAs.Bar, RenderAs.StackedBar, RenderAs.StackedBar100);

                minDiff = (minDiff < (Double)plotGroup.AxisX.InternalInterval) ? minDiff : (Double)plotGroup.AxisX.InternalInterval;

                Double maxBarHeight = Graphics.ValueToPixelPosition(0, height, (Double)plotGroup.AxisX.InternalAxisMinimum, (Double)plotGroup.AxisX.InternalAxisMaximum, minDiff + (Double)plotGroup.AxisX.InternalAxisMinimum) * (1 - BAR_GAP_RATIO);
                Double heightPerBar = maxBarHeight / numberOfDivisions;

                List<Double> xValuesList = plotGroup.XWiseStackedDataList.Keys.ToList();

                Double limitingYValue = 0;
                if (plotGroup.AxisY.InternalAxisMinimum > 0)
                    limitingYValue = (Double)plotGroup.AxisY.InternalAxisMinimum;
                if (plotGroup.AxisY.InternalAxisMaximum < 0)
                    limitingYValue = (Double)plotGroup.AxisY.InternalAxisMaximum;

                foreach (Double xValue in xValuesList)
                {
                    RectangularChartShapeParams barParams = new RectangularChartShapeParams();
                    barParams.ShadowOffset = 5;
                    barParams.Depth = depth3d;
                    barParams.IsStacked = true;

                    Double absoluteSum = plotGroup.XWiseStackedDataList[xValue].AbsoluteYValueSum;

                    if (Double.IsNaN(absoluteSum) || absoluteSum <= 0)
                        absoluteSum = 1;

                    Double top = Graphics.ValueToPixelPosition(height, 0, (Double)plotGroup.AxisX.InternalAxisMinimum, (Double)plotGroup.AxisX.InternalAxisMaximum, xValue) + drawingIndex * heightPerBar - (maxBarHeight / 2);
                    Double left = Graphics.ValueToPixelPosition(0, width, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, limitingYValue);

                    Double finalHeight = CalculateHeightOfEachColumn(ref top, heightPerBar, height);

                    if (finalHeight < 0)
                        continue;

                    Double right;
                    Double barWidth;
                    Double prevSum = 0;
                    Double percentYValue;

                    // Plot positive values
                    foreach (DataPoint dataPoint in plotGroup.XWiseStackedDataList[xValue].Positive)
                    {
                        if (!(Boolean)dataPoint.Enabled || Double.IsNaN(dataPoint.InternalYValue))
                            continue;

                        ColumnChart.SetColumnParms(ref barParams, ref chart, dataPoint, true);

                        barParams.IsTopOfStack = (dataPoint == plotGroup.XWiseStackedDataList[xValue].Positive.Last());
                        if (barParams.IsTopOfStack)
                        {
                            barParams.XRadius = new CornerRadius(0, dataPoint.RadiusX.Value.TopRight, dataPoint.RadiusX.Value.BottomRight, 0);
                            barParams.YRadius = new CornerRadius(0, dataPoint.RadiusY.Value.TopRight, dataPoint.RadiusY.Value.BottomRight, 0);
                        }

                        percentYValue = (dataPoint.InternalYValue / absoluteSum * 100);
                        right = Graphics.ValueToPixelPosition(0, width, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, percentYValue + prevSum);
                        barWidth = Math.Abs(right - left);

                        prevSum += percentYValue;

                        barParams.Size = new Size(barWidth, finalHeight);

                        Faces bar;
                        Panel barVisual = null;

                        if (chart.View3D)
                        {
                            bar = Get3DBar(barParams);
                            barVisual = bar.Visual;
                            barVisual.SetValue(Canvas.ZIndexProperty, GetStackedBarZIndex(left, top, width, height, (dataPoint.InternalYValue > 0)));
                        }
                        else
                        {
                            bar = Get2DBar(barParams);
                            barVisual = bar.Visual;
                        }

                        dataPoint.Faces = bar;
                        dataPoint.Faces.LabelCanvas = labelCanvas;

                        barVisual.SetValue(Canvas.LeftProperty, left);
                        barVisual.SetValue(Canvas.TopProperty, top);

                        columnCanvas.Children.Add(barVisual);
                        labelCanvas.Children.Add(GetMarker(chart, barParams, dataPoint, left, top));

                        // Apply animation
                        if (animationEnabled)
                        {
                            if (dataPoint.Parent.Storyboard == null)
                                dataPoint.Parent.Storyboard = new Storyboard();

                            CurrentDataSeries = dataPoint.Parent;

                            // Apply animation to the data points dataSeriesIndex.e to the rectangles that form the columns
                            dataPoint.Parent.Storyboard = ApplyStackedBarChartAnimation(barVisual, dataPoint.Parent.Storyboard, barParams, (1.0 / seriesList.Count) * (Double)(seriesList.IndexOf(dataPoint.Parent)), 1.0 / seriesList.Count);

                            // Apply animation to the marker and labels
                            dataPoint.Parent.Storyboard = AnimationHelper.ApplyOpacityAnimation(dataPoint.Marker, CurrentDataSeries, dataPoint.Parent.Storyboard, 1, dataPoint.Opacity * dataPoint.Parent.Opacity);
                        }

                        left = right;
                    }

                    prevSum = 0;
                    right = Graphics.ValueToPixelPosition(0, width, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, limitingYValue);

                    // Plot negative values
                    foreach (DataPoint dataPoint in plotGroup.XWiseStackedDataList[xValue].Negative)
                    {
                        if (!(Boolean)dataPoint.Enabled || Double.IsNaN(dataPoint.InternalYValue))
                            continue;

                        ColumnChart.SetColumnParms(ref barParams, ref chart, dataPoint, false);

                        barParams.IsTopOfStack = (dataPoint == plotGroup.XWiseStackedDataList[xValue].Negative.Last());
                        if (barParams.IsTopOfStack)
                        {
                            barParams.XRadius = new CornerRadius(dataPoint.RadiusX.Value.TopLeft, 0, 0, dataPoint.RadiusX.Value.BottomLeft);
                            barParams.YRadius = new CornerRadius(dataPoint.RadiusY.Value.TopRight, 0, 0, dataPoint.RadiusY.Value.BottomLeft);
                        }

                        percentYValue = (dataPoint.InternalYValue / absoluteSum * 100);

                        left = Graphics.ValueToPixelPosition(0, width, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, percentYValue + prevSum);
                        barWidth = Math.Abs(right - left);

                        prevSum += percentYValue;
                        barParams.Size = new Size(barWidth, finalHeight);

                        Faces bar;
                        Panel barVisual = null;

                        if (chart.View3D)
                        {
                            bar = Get3DBar(barParams);
                            barVisual = bar.Visual;
                            barVisual.SetValue(Canvas.ZIndexProperty, GetStackedBarZIndex(left, top, width, height, (dataPoint.InternalYValue > 0)));
                        }
                        else
                        {
                            bar = Get2DBar(barParams);
                            barVisual = bar.Visual;
                        }

                        dataPoint.Faces = bar;
                        dataPoint.Faces.LabelCanvas = labelCanvas;

                        barVisual.SetValue(Canvas.LeftProperty, left);
                        barVisual.SetValue(Canvas.TopProperty, top);

                        columnCanvas.Children.Add(barVisual);
                        labelCanvas.Children.Add(GetMarker(chart, barParams, dataPoint, left, top));

                        // Apply animation
                        if (animationEnabled)
                        {
                            if (dataPoint.Parent.Storyboard == null)
                                dataPoint.Parent.Storyboard = new Storyboard();

                            CurrentDataSeries = dataPoint.Parent;

                            // Apply animation to the data points dataSeriesIndex.e to the rectangles that form the columns
                            dataPoint.Parent.Storyboard = ApplyStackedBarChartAnimation(barVisual, dataPoint.Parent.Storyboard, barParams, (1.0 / seriesList.Count) * (Double)(seriesList.IndexOf(dataPoint.Parent)), 1.0 / seriesList.Count);

                            // Apply animation to the marker and labels
                            dataPoint.Parent.Storyboard = AnimationHelper.ApplyOpacityAnimation(dataPoint.Marker, CurrentDataSeries, dataPoint.Parent.Storyboard, 1, dataPoint.Opacity * dataPoint.Parent.Opacity);
                        }

                        right = left;
                    }

                }

            }

            if (!plankDrawn && chart.View3D && plotGroupList[0].AxisY.InternalAxisMinimum < 0 && plotGroupList[0].AxisY.InternalAxisMaximum > 0)
            {
                RectangularChartShapeParams barParams = new RectangularChartShapeParams();
                barParams.BackgroundBrush = new SolidColorBrush(Color.FromArgb((Byte)255, (Byte)127, (Byte)127, (Byte)127));
                barParams.Lighting = true;
                barParams.Size = new Size(1, height);
                barParams.Depth = depth3d;

                Faces zeroPlank = ColumnChart.Get3DColumn(barParams);
                Panel zeroPlankVisual = zeroPlank.Visual;

                Double left = Graphics.ValueToPixelPosition(0, width, (Double)plotGroupList[0].AxisY.InternalAxisMinimum, (Double)plotGroupList[0].AxisY.InternalAxisMaximum, 0);
                zeroPlankVisual.SetValue(Canvas.LeftProperty, left);
                zeroPlankVisual.SetValue(Canvas.TopProperty, (Double)0);
                zeroPlankVisual.SetValue(Canvas.ZIndexProperty, 0);
                zeroPlankVisual.Opacity = 0.7;
                columnCanvas.Children.Add(zeroPlankVisual);
            }
            visual.Children.Add(columnCanvas);
            visual.Children.Add(labelCanvas);
            return visual;
        }

        /// <summary>
        /// Create 2D bar for a DataPoint
        /// </summary>
        /// <param name="barParams">Bar parameters</param>
        /// <returns>Faces for bar</returns>
        internal static Faces Get2DBar(RectangularChartShapeParams barParams)
        {
            Faces faces = new Faces();
            faces.Parts = new List<FrameworkElement>();

            Grid barVisual = new Grid();

            barVisual.Width = barParams.Size.Width;
            barVisual.Height = barParams.Size.Height;

            Brush background = (barParams.Lighting ? Graphics.GetLightingEnabledBrush(barParams.BackgroundBrush, "Linear", null) : barParams.BackgroundBrush);

            Canvas barBase = ExtendedGraphics.Get2DRectangle(barParams.Size.Width, barParams.Size.Height,
                barParams.BorderThickness, barParams.BorderStyle, barParams.BorderBrush,
                background, barParams.XRadius, barParams.YRadius);

            (barBase.Children[0] as FrameworkElement).Tag = "ColumnBase";
            faces.Parts.Add(barBase.Children[0] as FrameworkElement);

            barVisual.Children.Add(barBase);

            if (barParams.Size.Height > 7 && barParams.Size.Width > 14 && barParams.Bevel)
            {
                Canvas bevelCanvas = ExtendedGraphics.Get2DRectangleBevel(barParams.Size.Width - barParams.BorderThickness - barParams.BorderThickness, barParams.Size.Height - barParams.BorderThickness - barParams.BorderThickness, 6, 6,
                    Graphics.GetBevelTopBrush(barParams.BackgroundBrush),
                    Graphics.GetBevelSideBrush((barParams.Lighting ? -70 : 0), barParams.BackgroundBrush),
                    Graphics.GetBevelSideBrush((barParams.Lighting ? -110 : 180), barParams.BackgroundBrush),
                    null);

                foreach (FrameworkElement fe in bevelCanvas.Children)
                    faces.Parts.Add(fe);

                bevelCanvas.SetValue(Canvas.LeftProperty, barParams.BorderThickness);
                bevelCanvas.SetValue(Canvas.TopProperty, barParams.BorderThickness);
                barVisual.Children.Add(bevelCanvas);
            }
            else
            {
                faces.Parts.Add(null);
                faces.Parts.Add(null);
                faces.Parts.Add(null);
                faces.Parts.Add(null);
            }

            if (barParams.Lighting && barParams.Bevel)
            {
                Canvas gradienceCanvas = ExtendedGraphics.Get2DRectangleGradiance(barParams.Size.Width, barParams.Size.Height,
                    Graphics.GetLeftGradianceBrush(63),
                    Graphics.GetLeftGradianceBrush(63),
                    Orientation.Horizontal);

                foreach (FrameworkElement fe in gradienceCanvas.Children)
                    faces.Parts.Add(fe);

                barVisual.Children.Add(gradienceCanvas);
            }
            else
            {
                faces.Parts.Add(null);
                faces.Parts.Add(null);
            }

            if (barParams.Shadow)
            {
                Double shadowVerticalOffsetGap = 1;
                Double shadowVerticalOffset = barParams.ShadowOffset - shadowVerticalOffsetGap;
                Double shadowHeight = barParams.Size.Height;
                CornerRadius xRadius = barParams.XRadius;
                CornerRadius yRadius = barParams.YRadius;
                if (barParams.IsStacked)
                {
                    if (barParams.IsPositive)
                    {
                        if (barParams.IsTopOfStack)
                        {
                            shadowHeight = barParams.Size.Height - barParams.ShadowOffset;
                            shadowVerticalOffset = barParams.ShadowOffset - shadowVerticalOffsetGap - shadowVerticalOffsetGap;
                            xRadius = new CornerRadius(xRadius.TopLeft, xRadius.TopRight, xRadius.BottomRight, xRadius.BottomLeft);
                            yRadius = new CornerRadius(yRadius.TopLeft, yRadius.TopRight, 0, 0);
                        }
                        else
                        {
                            shadowHeight = barParams.Size.Height + 6;
                            shadowVerticalOffset = -2;
                            xRadius = new CornerRadius(xRadius.TopLeft, xRadius.TopRight, xRadius.BottomRight, xRadius.BottomLeft);
                            yRadius = new CornerRadius(0, 0, 0, 0);
                        }
                    }
                    else
                    {
                        if (barParams.IsTopOfStack)
                        {
                            shadowHeight = barParams.Size.Height - barParams.ShadowOffset;
                            xRadius = new CornerRadius(xRadius.TopLeft, xRadius.TopRight, xRadius.BottomRight, xRadius.BottomLeft);
                            yRadius = new CornerRadius(yRadius.TopLeft, yRadius.TopRight, 0, 0);
                        }
                        else
                        {
                            shadowHeight = barParams.Size.Height + barParams.ShadowOffset + 2;
                            shadowVerticalOffset = -2;
                            xRadius = new CornerRadius(xRadius.TopLeft, xRadius.TopRight, xRadius.BottomRight, xRadius.BottomLeft);
                            yRadius = new CornerRadius(0, 0, 0, 0);
                        }
                    }
                }
                Grid shadowGrid = ExtendedGraphics.Get2DRectangleShadow(barParams.Size.Width, shadowHeight, xRadius, yRadius, barParams.IsStacked ? 3 : 5);
                TranslateTransform tt = new TranslateTransform() { X = barParams.ShadowOffset, Y = shadowVerticalOffset };
                shadowGrid.Opacity = 0.7;
                shadowGrid.SetValue(Canvas.ZIndexProperty, -1);
                shadowGrid.RenderTransform = tt;
                barVisual.Children.Add(shadowGrid);
            }

            faces.VisualComponents.Add(barVisual);

            faces.Visual = barVisual;

            return faces;
        }

        /// <summary>
        /// Create 3D bar for a DataPoint
        /// </summary>
        /// <param name="barParams">Bar parameters</param>
        /// <returns>Faces for bar</returns>
        internal static Faces Get3DBar(RectangularChartShapeParams barParams)
        {
            Faces faces = new Faces();
            faces.Parts = new List<FrameworkElement>();
            Canvas barVisual = new Canvas();

            barVisual.Width = barParams.Size.Width;
            barVisual.Height = barParams.Size.Height;

            Brush frontBrush = barParams.Lighting ? Graphics.GetFrontFaceBrush(barParams.BackgroundBrush) : barParams.BackgroundBrush;
            Brush topBrush = barParams.Lighting ? Graphics.GetTopFaceBrush(barParams.BackgroundBrush) : barParams.BackgroundBrush;
            Brush rightBrush = barParams.Lighting ? Graphics.GetRightFaceBrush(barParams.BackgroundBrush) : barParams.BackgroundBrush;

            Canvas front = ExtendedGraphics.Get2DRectangle(barParams.Size.Width, barParams.Size.Height,
                barParams.BorderThickness, barParams.BorderStyle, barParams.BorderBrush,
                frontBrush, new CornerRadius(0), new CornerRadius(0));

            faces.Parts.Add(front.Children[0] as FrameworkElement);

            Canvas top = ExtendedGraphics.Get2DRectangle(barParams.Size.Width, barParams.Depth,
                barParams.BorderThickness, barParams.BorderStyle, barParams.BorderBrush,
                topBrush, new CornerRadius(0), new CornerRadius(0));

            faces.Parts.Add(top.Children[0] as FrameworkElement);

            top.RenderTransformOrigin = new Point(0, 1);
            SkewTransform skewTransTop = new SkewTransform();
            skewTransTop.AngleX = -45;
            top.RenderTransform = skewTransTop;

            Canvas right = ExtendedGraphics.Get2DRectangle(barParams.Depth, barParams.Size.Height,
                barParams.BorderThickness, barParams.BorderStyle, barParams.BorderBrush,
                rightBrush, new CornerRadius(0), new CornerRadius(0));

            faces.Parts.Add(right.Children[0] as FrameworkElement);

            right.RenderTransformOrigin = new Point(0, 0);
            SkewTransform skewTransRight = new SkewTransform();
            skewTransRight.AngleY = -45;
            right.RenderTransform = skewTransRight;

            barVisual.Children.Add(front);
            barVisual.Children.Add(top);
            barVisual.Children.Add(right);

            top.SetValue(Canvas.TopProperty, -barParams.Depth);
            right.SetValue(Canvas.LeftProperty, barParams.Size.Width);

            faces.Visual = barVisual;

            faces.VisualComponents.Add(front);
            faces.VisualComponents.Add(top);
            faces.VisualComponents.Add(right);

            return faces;
        }

        #endregion

        #region Internal Events And Delegates

        #endregion

        #region Data

        /// <summary>
        /// Gap ratio between two column
        /// </summary>
        internal static Double BAR_GAP_RATIO = 0.2;

        #endregion

    }
}

