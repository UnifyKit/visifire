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
using System.Windows.Shapes;

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
        private static void SetMarkerPosition(Chart chart, DataPoint dataPoint, Boolean isPositive, String labelText, Size markerSize, Double canvasLeft, Double canvasTop, Point markerPosition)
        {
            if ((Boolean) dataPoint.LabelEnabled && !String.IsNullOrEmpty(labelText))
            {
                LabelStyles labelStyle = (LabelStyles)dataPoint.LabelStyle;

                dataPoint.Marker.CreateVisual();

                if (isPositive)
                {   
                    if (canvasLeft + markerPosition.X + dataPoint.Marker.MarkerActualSize.Width > chart.PlotArea.BorderElement.Width)
                        labelStyle = LabelStyles.Inside;
                }
                else
                {   
                    if (canvasLeft < dataPoint.Marker.MarkerActualSize.Width)
                        labelStyle = LabelStyles.Inside;
                }

                dataPoint.Marker.TextAlignmentY = AlignmentY.Center;

                if (!(Boolean)dataPoint.MarkerEnabled)
                {   
                    if (chart.View3D)
                    {   
                        if (labelStyle == LabelStyles.OutSide)
                            dataPoint.Marker.MarkerSize = new Size(markerSize.Width + chart.ChartArea.PLANK_DEPTH, markerSize.Height + chart.ChartArea.PLANK_DEPTH);
                        else
                            dataPoint.Marker.MarkerSize = new Size(markerSize.Width, markerSize.Height);
                    }
                }
                else
                {   
                    if (chart.View3D)
                    {
                        labelStyle = LabelStyles.Inside;
                    }
                }

                if (isPositive)
                    dataPoint.Marker.TextAlignmentX = labelStyle == LabelStyles.Inside ? AlignmentX.Left : AlignmentX.Right;
                else
                    dataPoint.Marker.TextAlignmentX = labelStyle == LabelStyles.Inside ? AlignmentX.Right : AlignmentX.Left;
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

            //Double minPosValue = 0;
            //Double maxPosValue = height;

            //if (top < minPosValue)
            //{
            //    finalHeight = top + heightPerBar - minPosValue;
            //    top = minPosValue;
            //}
            //else if (top + heightPerBar > maxPosValue)
            //{
            //    finalHeight = maxPosValue - top;
            //}

            return (finalHeight < 2) ? 2 : finalHeight;
            //return finalHeight;
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
        private static Storyboard ApplyBarChartAnimation(Panel bar, Storyboard storyboard, Boolean isPositive)
        {
            ScaleTransform scaleTransform = new ScaleTransform() { ScaleX = 0 };
            bar.RenderTransform = scaleTransform;

            if (isPositive)
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
        private static Storyboard ApplyStackedBarChartAnimation(Panel bar, Storyboard storyboard, Double begin, Double duration)
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
        internal static void CreateOrUpdateMarker4HorizontalChart(Chart chart, Canvas labelCanvas, DataPoint dataPoint, Double left, Double top, Boolean isPositive, Double depth3d)
        {
            if (dataPoint.Faces == null)
                return;

            Canvas barVisual = dataPoint.Faces.Visual as Canvas;

            if ((Boolean)dataPoint.MarkerEnabled || (Boolean)dataPoint.LabelEnabled)
            {   
                Size markerSize = new Size(dataPoint.MarkerSize.Value, dataPoint.MarkerSize.Value);
                String labelText = (Boolean) dataPoint.LabelEnabled ? dataPoint.TextParser(dataPoint.LabelText) : "";

                if (dataPoint.Marker != null)
                    labelCanvas.Children.Remove(dataPoint.Marker.Visual);

                Marker marker = ColumnChart.CreateNewMarker(dataPoint, markerSize, labelText);
                dataPoint.Marker = marker;

                if (!(Boolean)dataPoint.MarkerEnabled)
                {
                    marker.FillColor = new SolidColorBrush(Colors.Transparent);
                    marker.BorderColor = new SolidColorBrush(Colors.Transparent);
                }

                Point markerPosition = new Point();

                if (isPositive)
                    if (chart.View3D)
                        markerPosition = new Point(barVisual.Width + depth3d, barVisual.Height / 2 - depth3d);
                    else
                        markerPosition = new Point(barVisual.Width, barVisual.Height / 2);
                else
                    if (chart.View3D)
                        markerPosition = new Point(depth3d, barVisual.Height / 2 - depth3d);
                    else
                        markerPosition = new Point(0, barVisual.Height / 2);

                SetMarkerPosition(chart, dataPoint, isPositive, labelText, markerSize, left, top, markerPosition);

                marker.FontColor = Chart.CalculateDataPointLabelFontColor(chart, dataPoint, dataPoint.LabelFontColor, (dataPoint.YValue == 0) ? LabelStyles.OutSide : (LabelStyles) dataPoint.LabelStyle);

                marker.Tag = new ElementData() { Element = dataPoint };

                marker.CreateVisual();

                marker.AddToParent(labelCanvas, left + markerPosition.X, top + markerPosition.Y, new Point(0.5, 0.5));
            }
        }

        internal static void CreateColumnDataPointVisual(DataPoint dataPoint, Canvas labelCanvas, Canvas columnCanvas , Boolean isPositive, Double heightPerBar, Double depth3d, Boolean animationEnabled)
        {
            Double xValue = dataPoint.InternalXValue;
            Double width = columnCanvas.Width, height = columnCanvas.Height;
            Chart chart = dataPoint.Chart as Chart;
            PlotDetails plotDetails = chart.PlotDetails;

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

            Double columnHeight = CalculateHeightOfEachColumn(ref top, heightPerBar, height);

            if (columnHeight < 0)
                return;
            
            Faces column;
            Panel columnVisual = null;

            if (chart.View3D)
            {   
                // column = Get3DBar(barParams);
                column = ColumnChart.Get3DColumn(dataPoint, columnWidth, columnHeight, depth3d, dataPoint.Color, 
                    null, null, null, (Boolean)dataPoint.LightingEnabled, (BorderStyles)dataPoint.BorderStyle, 
                    dataPoint.BorderColor, dataPoint.BorderThickness.Left);
                    
                columnVisual = column.Visual as Panel;
                columnVisual.SetValue(Canvas.ZIndexProperty, GetBarZIndex(isPositive ? left : right, top, height, dataPoint.InternalYValue > 0));
            }
            else
            {   
                // column = Get2DBar(barParams);
                column = ColumnChart.Get2DColumn(dataPoint, columnWidth, columnHeight, false, false);
                columnVisual = column.Visual as Panel;
            }

            dataPoint.Faces = column;
            dataPoint.Faces.LabelCanvas = labelCanvas;
            dataPoint.Parent.Faces = new Faces() { Visual = columnCanvas, LabelCanvas = labelCanvas };

            columnVisual.SetValue(Canvas.LeftProperty, isPositive ? left : right);
            columnVisual.SetValue(Canvas.TopProperty, top);

            columnCanvas.Children.Add(columnVisual);

            CreateOrUpdateMarker4HorizontalChart(chart, labelCanvas, dataPoint, isPositive ? left : right, top, isPositive, depth3d);

            dataPoint.Faces.LabelCanvas = labelCanvas;
            
            // Apply animation
            if (animationEnabled)
            {   
                if (dataPoint.Parent.Storyboard == null)
                    dataPoint.Parent.Storyboard = new Storyboard();

                CurrentDataSeries = dataPoint.Parent;

                // Apply animation to the bars 
                dataPoint.Parent.Storyboard = ApplyBarChartAnimation(columnVisual, dataPoint.Parent.Storyboard, isPositive);

                // Apply animation to the marker and labels
                dataPoint.Parent.Storyboard = AnimationHelper.ApplyOpacityAnimation(dataPoint.Marker, CurrentDataSeries, dataPoint.Parent.Storyboard, 1, dataPoint.Opacity * dataPoint.Parent.Opacity);
            }

            dataPoint.Faces.Visual.Opacity = dataPoint.Opacity * dataPoint.Parent.Opacity;
            dataPoint.AttachEvent2DataPointVisualFaces(dataPoint);
            dataPoint.AttachEvent2DataPointVisualFaces(dataPoint.Parent);
            dataPoint._parsedToolTipText = dataPoint.TextParser(dataPoint.ToolTipText);
            dataPoint.AttachToolTip(chart, dataPoint, dataPoint.Faces.VisualComponents);
            dataPoint.AttachHref(chart, dataPoint.Faces.VisualComponents, dataPoint.Href, (HrefTargets)dataPoint.HrefTarget);
            dataPoint.SetCursor2DataPointVisualFaces();
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
        internal static Canvas GetVisualObjectForBarChart(Panel preExistingPanel, Double width, Double height, PlotDetails plotDetails, List<DataSeries> dataSeriesList4Rendering, Chart chart, Double plankDepth, bool animationEnabled)
        {   
            if (Double.IsNaN(width) || Double.IsNaN(height) || width <= 0 || height <= 0)
                return null;

            Canvas visual, labelCanvas, columnCanvas;

            RenderHelper.RepareCanvas4Drawing(preExistingPanel as Canvas, out visual, out labelCanvas, out columnCanvas, width, height);
            
            List<PlotGroup> plotGroupList = (from plots in plotDetails.PlotGroups where plots.RenderAs == RenderAs.Bar select plots).ToList();

            Double depth3d = plankDepth / plotDetails.Layer3DCount * (chart.View3D ? 1 : 0);
            Double visualOffset = depth3d * (plotDetails.SeriesDrawingIndex[plotGroupList[0].DataSeriesList[0]] + 1);

            visual.SetValue(Canvas.TopProperty, visualOffset);
            visual.SetValue(Canvas.LeftProperty, -visualOffset);

            labelCanvas.SetValue(Canvas.TopProperty, visualOffset);
            labelCanvas.SetValue(Canvas.LeftProperty, -visualOffset);
            
            Dictionary<Double, SortDataPoints> sortedDataPoints = plotDetails.GetDataPointsGroupedByXValue(RenderAs.Bar);
            List<Double> xValues = sortedDataPoints.Keys.ToList();

            Double heightPerBar = ColumnChart.CalculateWidthOfEachColumn(chart, height, plotGroupList[0].AxisX, RenderAs.Bar, Orientation.Vertical);

            foreach (Double xValue in xValues)
            {   
                foreach (DataPoint dataPoint in sortedDataPoints[xValue].Positive)
                {   
                    CreateColumnDataPointVisual(dataPoint, labelCanvas, columnCanvas, true,heightPerBar, depth3d,  animationEnabled);
                }

                foreach (DataPoint dataPoint in sortedDataPoints[xValue].Negative)
                {
                    CreateColumnDataPointVisual(dataPoint, labelCanvas, columnCanvas,false, heightPerBar, depth3d,   animationEnabled);
                }
            }

            ColumnChart.CreateOrUpdatePlank(chart, plotGroupList[0].AxisY, columnCanvas, depth3d, Orientation.Vertical);

            // Remove old visual and add new visual in to the existing panel
            if (preExistingPanel != null)
            {
                visual.Children.RemoveAt(1);
                visual.Children.Add(columnCanvas);
            }
            else
            {
                labelCanvas.SetValue(Canvas.ZIndexProperty, 1);
                visual.Children.Add(labelCanvas);
                visual.Children.Add(columnCanvas);
            }

            return visual;
        }

        internal static void DrawStackedBarsAtXValue(RenderAs chartType, Double xValue, PlotGroup plotGroup, Canvas columnCanvas, Canvas labelCanvas, Double drawingIndex, Double heightPerBar, Double maxBarHeight, Double limitingYValue, Double depth3d, Boolean animationEnabled)
        {   
            RectangularChartShapeParams barParams = new RectangularChartShapeParams();
            barParams.ShadowOffset = 5;
            barParams.Depth = depth3d;
            barParams.IsStacked = true;

            Double top = Graphics.ValueToPixelPosition(columnCanvas.Height, 0, (Double)plotGroup.AxisX.InternalAxisMinimum, (Double)plotGroup.AxisX.InternalAxisMaximum, xValue) + drawingIndex * heightPerBar - (maxBarHeight / 2);
            Double left = Graphics.ValueToPixelPosition(0, columnCanvas.Width, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, limitingYValue);

            Double columnHeight = CalculateHeightOfEachColumn(ref top, heightPerBar, columnCanvas.Height);

            Double right=0;
            Double prevSum = 0;

            Double animationBeginTime = 0.4;
            Double animationTime = 1.0 / plotGroup.XWiseStackedDataList[xValue].Positive.Count;

            Double absoluteSum = Double.NaN;
            
            if (chartType == RenderAs.StackedBar100)
                absoluteSum = plotGroup.XWiseStackedDataList[xValue].AbsoluteYValueSum;

            // Plot positive values
            foreach (DataPoint dataPoint in plotGroup.XWiseStackedDataList[xValue].Positive)
            {
                dataPoint.Parent.Faces = new Faces { Visual = columnCanvas, LabelCanvas = labelCanvas };

                if (!(Boolean)dataPoint.Enabled || Double.IsNaN(dataPoint.InternalYValue))
                    continue;

                 CreateStackedBarVisual(dataPoint.Parent.RenderAs, dataPoint.InternalYValue >= 0, columnCanvas, labelCanvas, dataPoint,
                     top, ref left, ref right, columnHeight, ref prevSum, absoluteSum, depth3d, animationEnabled,
                     animationBeginTime);

                 animationBeginTime += animationTime;
            }

            prevSum = 0;
            right = Graphics.ValueToPixelPosition(0, columnCanvas.Width, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, limitingYValue);
            
            animationBeginTime = 0.4;
            animationTime = 1.0 / plotGroup.XWiseStackedDataList[xValue].Negative.Count;
            
            // Plot negative values
            foreach (DataPoint dataPoint in plotGroup.XWiseStackedDataList[xValue].Negative)
            {
                dataPoint.Parent.Faces = new Faces { Visual = columnCanvas, LabelCanvas = labelCanvas };

                if (!(Boolean)dataPoint.Enabled || Double.IsNaN(dataPoint.InternalYValue))
                    continue;

                CreateStackedBarVisual(dataPoint.Parent.RenderAs, dataPoint.InternalYValue >= 0, columnCanvas, labelCanvas, dataPoint, 
                    top, ref left, ref right, columnHeight, ref prevSum, absoluteSum, depth3d, animationEnabled, 
                    animationBeginTime);

                animationBeginTime += animationTime;
            }
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
        internal static Canvas GetVisualObjectForStackedBarChart(RenderAs chartType, Panel preExistingPanel, Double width, Double height, PlotDetails plotDetails, Chart chart, Double plankDepth, bool animationEnabled)
        {   
            if (Double.IsNaN(width) || Double.IsNaN(height) || width <= 0 || height <= 0) return null;

            Canvas visual, labelCanvas, columnCanvas;
            RenderHelper.RepareCanvas4Drawing(preExistingPanel as Canvas, out visual, out labelCanvas, out columnCanvas, width, height);

            List<PlotGroup> plotGroupList = (from plots in plotDetails.PlotGroups where plots.RenderAs == chartType select plots).ToList();

            Double numberOfDivisions = plotDetails.DrawingDivisionFactor;

            Double depth3d = plankDepth / plotDetails.Layer3DCount * (chart.View3D ? 1 : 0);
            Double visualOffset = depth3d * (plotDetails.SeriesDrawingIndex[plotGroupList[0].DataSeriesList[0]] + 1);
            
            visual.SetValue(Canvas.TopProperty, visualOffset);
            visual.SetValue(Canvas.LeftProperty, -visualOffset);

            List<DataSeries> seriesList = plotDetails.GetSeriesListByRenderAs(chartType);
            Dictionary<Axis, Dictionary<Axis, Int32>> seriesIndex = ColumnChart.GetSeriesIndex(seriesList);
            Int32 index = 1;

            foreach (PlotGroup plotGroup in plotGroupList)
            {   
                if (!seriesIndex.ContainsKey(plotGroup.AxisY))
                    continue;

                Int32 drawingIndex = seriesIndex[plotGroup.AxisY][plotGroup.AxisX];
                //Double minDiff = plotDetails.GetMinOfMinDifferencesForXValue(RenderAs.Bar, RenderAs.StackedBar, RenderAs.StackedBar100);

                //if (Double.IsPositiveInfinity(minDiff))
                //    minDiff = 0;
                              
                //Double maxBarHeight = Graphics.ValueToPixelPosition(0, height, (Double)plotGroup.AxisX.InternalAxisMinimum, (Double)plotGroup.AxisX.InternalAxisMaximum, minDiff + (Double)plotGroup.AxisX.InternalAxisMinimum) * (1 - BAR_GAP_RATIO);
                //Double heightPerBar = maxBarHeight / numberOfDivisions;

                //if (minDiff == 0)
                //{
                //    heightPerBar = height * .5;
                //    maxBarHeight = heightPerBar;
                //    heightPerBar /= numberOfDivisions;
                //}
                //else
                //{
                //    heightPerBar = Graphics.ValueToPixelPosition(0, height, (Double)plotGroup.AxisX.InternalAxisMinimum, (Double)plotGroup.AxisX.InternalAxisMaximum, minDiff + (Double)plotGroup.AxisX.InternalAxisMinimum);
                //    heightPerBar *= (1 - BAR_GAP_RATIO);
                //    maxBarHeight = heightPerBar;
                //    heightPerBar /= numberOfDivisions;
                //}

                //if (!Double.IsNaN(chart.DataPointWidth))
                //{
                //    if (chart.DataPointWidth >= 0)
                //    {
                //        heightPerBar = maxBarHeight = chart.DataPointWidth / 100 * chart.PlotArea.Height;
                //        maxBarHeight *= numberOfDivisions;
                //    }
                //}

                Double minDiff, heightPerBar, maxBarHeight;
                heightPerBar = ColumnChart.CalculateWidthOfEachStackedColumn(chart, plotGroup, height, out minDiff, out maxBarHeight);
                
                List<Double> xValuesList = plotGroup.XWiseStackedDataList.Keys.ToList();

                Double limitingYValue = 0;
                if (plotGroup.AxisY.InternalAxisMinimum > 0)
                    limitingYValue = (Double)plotGroup.AxisY.InternalAxisMinimum;
                if (plotGroup.AxisY.InternalAxisMaximum < 0)
                    limitingYValue = (Double)plotGroup.AxisY.InternalAxisMaximum;

                index++;

                foreach (Double xValue in xValuesList)
                {
                    DrawStackedBarsAtXValue(chartType, xValue, plotGroup, columnCanvas, labelCanvas, drawingIndex, heightPerBar, maxBarHeight, limitingYValue, depth3d, animationEnabled);
                }
            }

            ColumnChart.CreateOrUpdatePlank(chart, plotGroupList[0].AxisY, columnCanvas, depth3d, Orientation.Vertical);

            // Remove old visual and add new visual in to the existing panel
            if (preExistingPanel != null)
            {
                visual.Children.RemoveAt(1);
                visual.Children.Add(columnCanvas);
            }
            else
            {
                labelCanvas.SetValue(Canvas.ZIndexProperty, 1);
                visual.Children.Add(labelCanvas);
                visual.Children.Add(columnCanvas);
            }

            return visual;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="chartType"></param>
        /// <param name="isPositive"></param>
        /// <param name="columnCanvas"></param>
        /// <param name="labelCanvas"></param>
        /// <param name="dataPoint"></param>
        /// <param name="top"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="finalHeight"></param>
        /// <param name="prevSum"></param>
        /// <param name="absoluteSum">Absolute sum of DataPoints if applicable</param>
        /// <param name="depth3d"></param>
        /// <param name="animationEnabled"></param>
        /// <param name="animationBeginTime"></param>
        private static void CreateStackedBarVisual(RenderAs chartType, Boolean isPositive, Canvas columnCanvas, Canvas labelCanvas,
            DataPoint dataPoint, Double top, ref Double left, ref Double right, Double finalHeight,
            ref Double prevSum, Double absoluteSum, Double depth3d, Boolean animationEnabled,
            Double animationBeginTime)
        {
            PlotGroup plotGroup = dataPoint.Parent.PlotGroup;
            Chart chart = dataPoint.Chart as Chart;

            Double percentYValue;

            if (chartType == RenderAs.StackedBar100)
                percentYValue = (dataPoint.InternalYValue / absoluteSum * 100);
            else
                percentYValue = dataPoint.InternalYValue;

            if (isPositive)
                right = Graphics.ValueToPixelPosition(0, columnCanvas.Width, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, percentYValue + prevSum);
            else
                left = Graphics.ValueToPixelPosition(0, columnCanvas.Width, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, percentYValue + prevSum);

            Double barWidth = Math.Abs(right - left);

            prevSum += percentYValue;
            // barParams.Size = new Size(barWidth, finalHeight);

            Faces bar;
            Panel barVisual = null;

            if ((dataPoint.Chart as Chart).View3D)
            {
                bar = ColumnChart.Get3DColumn(dataPoint, barWidth, finalHeight, depth3d, dataPoint.Color, null, null, null, (Boolean)dataPoint.LightingEnabled,
                    (BorderStyles)dataPoint.BorderStyle, dataPoint.BorderColor, dataPoint.BorderThickness.Left);

                barVisual = bar.Visual as Panel;
                barVisual.SetValue(Canvas.ZIndexProperty, GetStackedBarZIndex(left, top, columnCanvas.Width, columnCanvas.Height, (dataPoint.InternalYValue > 0)));
            }
            else
            {
                bar = ColumnChart.Get2DColumn(dataPoint, barWidth, finalHeight, true, false);
                barVisual = bar.Visual as Panel;
            }

            dataPoint.Faces = bar;
            dataPoint.Faces.LabelCanvas = labelCanvas;

            barVisual.SetValue(Canvas.LeftProperty, left);
            barVisual.SetValue(Canvas.TopProperty, top);

            columnCanvas.Children.Add(barVisual);
            CreateOrUpdateMarker4HorizontalChart(dataPoint.Chart as Chart, labelCanvas, dataPoint, left, top, isPositive, depth3d);

            //labelCanvas.Children.Add(CreateMarker(chart, barParams, dataPoint, left, top));

            // Apply animation
            if (animationEnabled)
            {
                if (dataPoint.Parent.Storyboard == null)
                    dataPoint.Parent.Storyboard = new Storyboard();

                CurrentDataSeries = dataPoint.Parent;

                // Apply animation to the data points dataSeriesIndex.e to the rectangles that form the columns
                dataPoint.Parent.Storyboard = ApplyStackedBarChartAnimation(barVisual, dataPoint.Parent.Storyboard, animationBeginTime, 0.5);

                // Apply animation to the marker and labels
                dataPoint.Parent.Storyboard = AnimationHelper.ApplyOpacityAnimation(dataPoint.Marker, CurrentDataSeries, dataPoint.Parent.Storyboard, 1, dataPoint.Opacity * dataPoint.Parent.Opacity);
            }

            if (isPositive)
                left = right;
            else
                right = left;

            dataPoint.Faces.Visual.Opacity = dataPoint.Opacity * dataPoint.Parent.Opacity;
            dataPoint.AttachEvent2DataPointVisualFaces(dataPoint);
            dataPoint.AttachEvent2DataPointVisualFaces(dataPoint.Parent);
            dataPoint._parsedToolTipText = dataPoint.TextParser(dataPoint.ToolTipText);
            dataPoint.AttachToolTip(chart, dataPoint, dataPoint.Faces.VisualComponents);
            dataPoint.AttachHref(chart, dataPoint.Faces.VisualComponents, dataPoint.Href, (HrefTargets)dataPoint.HrefTarget);
            dataPoint.SetCursor2DataPointVisualFaces();
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

                if (Double.IsPositiveInfinity(minDiff))
                    minDiff = 0;

                //minDiff = (minDiff < (Double)plotGroup.AxisX.InternalInterval) ? minDiff : (Double)plotGroup.AxisX.InternalInterval;

                Double maxBarHeight = Graphics.ValueToPixelPosition(0, height, (Double)plotGroup.AxisX.InternalAxisMinimum, (Double)plotGroup.AxisX.InternalAxisMaximum, minDiff + (Double)plotGroup.AxisX.InternalAxisMinimum) * (1 - BAR_GAP_RATIO);
                Double heightPerBar = maxBarHeight / numberOfDivisions;

                if (minDiff == 0)
                {
                    heightPerBar = height * .5;
                    maxBarHeight = heightPerBar;
                    heightPerBar /= numberOfDivisions;
                }
                else
                {
                    heightPerBar = Graphics.ValueToPixelPosition(0, height, (Double)plotGroup.AxisX.InternalAxisMinimum, (Double)plotGroup.AxisX.InternalAxisMaximum, minDiff + (Double)plotGroup.AxisX.InternalAxisMinimum);
                    heightPerBar *= (1 - BAR_GAP_RATIO);
                    maxBarHeight = heightPerBar;
                    heightPerBar /= numberOfDivisions;
                }

                if (!Double.IsNaN(chart.DataPointWidth))
                {
                    if (chart.DataPointWidth >= 0)
                    {
                        heightPerBar = maxBarHeight = chart.DataPointWidth / 100 * chart.PlotArea.Height;
                        maxBarHeight *= numberOfDivisions;
                    }
                }

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
                        dataPoint.Parent.Faces = new Faces { Visual = columnCanvas, LabelCanvas = labelCanvas };

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
                            barVisual = bar.Visual as Panel;
                            barVisual.SetValue(Canvas.ZIndexProperty, GetStackedBarZIndex(left, top, width, height, (dataPoint.InternalYValue > 0)));
                        }
                        else
                        {
                            bar = Get2DBar(barParams);
                            barVisual = bar.Visual as Panel;
                        }

                        dataPoint.Faces = bar;
                        dataPoint.Faces.LabelCanvas = labelCanvas;

                        barVisual.SetValue(Canvas.LeftProperty, left);
                        barVisual.SetValue(Canvas.TopProperty, top);

                        columnCanvas.Children.Add(barVisual);
                        //labelCanvas.Children.Add(GetMarker(chart, barParams, dataPoint, left, top));

                        // Apply animation
                        if (animationEnabled)
                        {
                            if (dataPoint.Parent.Storyboard == null)
                                dataPoint.Parent.Storyboard = new Storyboard();

                            CurrentDataSeries = dataPoint.Parent;

                            // Apply animation to the data points dataSeriesIndex.e to the rectangles that form the columns
                            dataPoint.Parent.Storyboard = ApplyStackedBarChartAnimation(barVisual, dataPoint.Parent.Storyboard, (1.0 / seriesList.Count) * (Double)(seriesList.IndexOf(dataPoint.Parent)), 1.0 / seriesList.Count);

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
                        dataPoint.Parent.Faces = new Faces { Visual = columnCanvas, LabelCanvas = labelCanvas };

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
                            barVisual = bar.Visual as Panel;
                            barVisual.SetValue(Canvas.ZIndexProperty, GetStackedBarZIndex(left, top, width, height, (dataPoint.InternalYValue > 0)));
                        }
                        else
                        {
                            bar = Get2DBar(barParams);
                            barVisual = bar.Visual as Panel;
                        }

                        dataPoint.Faces = bar;
                        dataPoint.Faces.LabelCanvas = labelCanvas;

                        barVisual.SetValue(Canvas.LeftProperty, left);
                        barVisual.SetValue(Canvas.TopProperty, top);

                        columnCanvas.Children.Add(barVisual);
                        
                        // labelCanvas.Children.Add(GetMarker(chart, labelCanvas, dataPoint, left, top));

                        // Apply animation
                        if (animationEnabled)
                        {   
                            if (dataPoint.Parent.Storyboard == null)
                                dataPoint.Parent.Storyboard = new Storyboard();

                            CurrentDataSeries = dataPoint.Parent;

                            // Apply animation to the data points dataSeriesIndex.e to the rectangles that form the columns
                            dataPoint.Parent.Storyboard = ApplyStackedBarChartAnimation(barVisual, dataPoint.Parent.Storyboard, (1.0 / seriesList.Count) * (Double)(seriesList.IndexOf(dataPoint.Parent)), 1.0 / seriesList.Count);

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
                Panel zeroPlankVisual = zeroPlank.Visual as Panel;

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
        /// Create 3D bar for a DataPoint
        /// </summary>
        /// <param name="barParams">Bar parameters</param>
        /// <returns>Faces for bar</returns>
        internal static Faces Get3DBar(RectangularChartShapeParams barParams)
        {
            Faces faces = new Faces();
            faces.Parts = new List<DependencyObject>();
            Canvas barVisual = new Canvas();

            barVisual.Width = barParams.Size.Width;
            barVisual.Height = barParams.Size.Height;

            Brush frontBrush = barParams.Lighting ? Graphics.GetFrontFaceBrush(barParams.BackgroundBrush) : barParams.BackgroundBrush;
            Brush topBrush = barParams.Lighting ? Graphics.GetTopFaceBrush(barParams.BackgroundBrush) : barParams.BackgroundBrush;
            Brush rightBrush = barParams.Lighting ? Graphics.GetRightFaceBrush(barParams.BackgroundBrush) : barParams.BackgroundBrush;

            Rectangle rectangle;
            Rectangle front = ExtendedGraphics.Get2DRectangle(barParams.TagReference, barParams.Size.Width, barParams.Size.Height,
                barParams.BorderThickness, barParams.BorderStyle, barParams.BorderBrush,
                frontBrush, new CornerRadius(0), new CornerRadius(0));

            faces.Parts.Add(front);
            faces.BorderElements.Add(front);

            Rectangle top = ExtendedGraphics.Get2DRectangle(barParams.TagReference, barParams.Size.Width, barParams.Depth,
                barParams.BorderThickness, barParams.BorderStyle, barParams.BorderBrush,
                topBrush, new CornerRadius(0), new CornerRadius(0));

            faces.Parts.Add(top);
            faces.BorderElements.Add(top);

            top.RenderTransformOrigin = new Point(0, 1);
            SkewTransform skewTransTop = new SkewTransform();
            skewTransTop.AngleX = -45;
            top.RenderTransform = skewTransTop;

            Rectangle right = ExtendedGraphics.Get2DRectangle(barParams.TagReference, barParams.Depth, barParams.Size.Height,
                barParams.BorderThickness, barParams.BorderStyle, barParams.BorderBrush,
                rightBrush, new CornerRadius(0), new CornerRadius(0));

            faces.Parts.Add(right);
            faces.BorderElements.Add(right);

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


        //--------------del

        /// <summary>
        /// Create 2D bar for a DataPoint
        /// </summary>
        /// <param name="barParams">Bar parameters</param>
        /// <returns>Faces for bar</returns>
        internal static Faces Get2DBar(RectangularChartShapeParams barParams)
        {
            Faces faces = new Faces();
            faces.Parts = new List<DependencyObject>();

            Grid barVisual = new Grid();

            barVisual.Width = barParams.Size.Width;
            barVisual.Height = barParams.Size.Height;

            Brush background = (barParams.Lighting ? Graphics.GetLightingEnabledBrush(barParams.BackgroundBrush, "Linear", null) : barParams.BackgroundBrush);

            Rectangle rectangle;
            Rectangle barBase = ExtendedGraphics.Get2DRectangle(barParams.TagReference, barParams.Size.Width, barParams.Size.Height,
                barParams.BorderThickness, barParams.BorderStyle, barParams.BorderBrush,
                background, barParams.XRadius, barParams.YRadius);

            (barBase.Tag as ElementData).VisualElementName = "ColumnBase";

            //faces.Parts.Add(barBase.Children[0] as FrameworkElement);
            //faces.BorderElements.Add(barBase.Children[0] as Path);

            faces.Parts.Add(barBase);
            faces.BorderElements.Add(barBase);

            barVisual.Children.Add(barBase);

            if (barParams.Size.Height > 7 && barParams.Size.Width > 14 && barParams.Bevel)
            {
                Canvas bevelCanvas = ExtendedGraphics.Get2DRectangleBevel(barParams.TagReference, barParams.Size.Width - barParams.BorderThickness - barParams.BorderThickness, barParams.Size.Height - barParams.BorderThickness - barParams.BorderThickness, 6, 6,
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

                Grid shadowGrid = ExtendedGraphics.Get2DRectangleShadow(barParams.TagReference, barParams.Size.Width, shadowHeight, xRadius, yRadius, barParams.IsStacked ? 3 : 5);
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

    }
}

