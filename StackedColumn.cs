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
    public partial class ColumnChart
    {   
        internal static void CreateStackedColumnVisual(Boolean isPositive, Canvas columnCanvas, Canvas labelCanvas, DataPoint dataPoint, Boolean isTopOFStack, Double left, ref Double top, ref Double bottom, Double columnWidth, Double depth3d, ref Double prevSum, Int32 positiveOrNegativeIndex, Boolean animationEnabled)
        {   
            PlotGroup plotGroup = dataPoint.Parent.PlotGroup;
            Chart chart = dataPoint.Chart as Chart;

            // if (isTopOfStack)
            // {
            //     columnParams.XRadius = new CornerRadius(dataPoint.RadiusX.Value.TopLeft, dataPoint.RadiusX.Value.TopRight, 0, 0);
            //     columnParams.YRadius = new CornerRadius(dataPoint.RadiusY.Value.TopLeft, dataPoint.RadiusY.Value.TopRight, 0, 0);
            // }

            Double columnHeight = Math.Abs(top - bottom);

            prevSum += dataPoint.InternalYValue;

            // columnParams.Size = new Size(finalWidth, columnHeight);

            Faces column;
            Panel columnVisual = null;

            if (chart.View3D)
            {   
                column = ColumnChart.Get3DColumn(dataPoint, columnWidth, columnHeight, depth3d, dataPoint.Color, null, null, null, (Boolean)dataPoint.LightingEnabled,
                (BorderStyles)dataPoint.BorderStyle, dataPoint.BorderColor, dataPoint.BorderThickness.Left);

                columnVisual = column.Visual as Panel;
                columnVisual.SetValue(Canvas.ZIndexProperty, ColumnChart.GetStackedColumnZIndex(left, top, (dataPoint.InternalYValue > 0), positiveOrNegativeIndex++));
            }
            else
            {   
                column = ColumnChart.Get2DColumn(dataPoint, columnWidth, columnHeight, true, false);
                columnVisual = column.Visual as Panel;
            }

            dataPoint.Faces = column;
            dataPoint.Faces.LabelCanvas = labelCanvas;

            columnVisual.SetValue(Canvas.LeftProperty, left);
            columnVisual.SetValue(Canvas.TopProperty, top);

            columnCanvas.Children.Add(columnVisual);
            // labelCanvas.Children.Add(GetMarker(chart, columnParams, dataPoint, left, top));
            DataSeries CurrentDataSeries;

            // Apply animation
            if (animationEnabled)
            {
                if (dataPoint.Parent.Storyboard == null)
                    dataPoint.Parent.Storyboard = new Storyboard();

                CurrentDataSeries = dataPoint.Parent;

                // Apply animation to the data points dataSeriesIndex.e to the rectangles that form the columns
                // dataPoint.Parent.Storyboard = ApplyStackedColumnChartAnimation(CurrentDataSeries, columnVisual, dataPoint.Parent.Storyboard, columnParams, (1.0 / seriesList.Count) * (Double)(seriesList.IndexOf(dataPoint.Parent)), 1.0 / seriesList.Count);

                // Apply animation to the marker and labels
                // dataPoint.Parent.Storyboard = AnimationHelper.ApplyOpacityAnimation(dataPoint.Marker, CurrentDataSeries, dataPoint.Parent.Storyboard, 1, dataPoint.Opacity * dataPoint.Parent.Opacity);
            }

            if (isPositive)
                bottom = top;
            else
                top = bottom;
        }
        
        /// <summary>
        /// Get visual object for stacked column chart
        /// </summary>
        /// <param name="width">Width of the PlotArea</param>
        /// <param name="height">Height of the PlotArea</param>
        /// <param name="plotDetails">PlotDetails</param>
        /// <param name="chart">Chart</param>
        /// <param name="plankDepth">PlankDepth</param>
        /// <param name="animationEnabled">Whether animation is enabled for chart</param>
        /// <returns>StackedColumn chart canvas</returns>
        internal static Canvas GetVisualObjectForStackedColumnChart(Panel preExistingPanel, Double width, Double height, PlotDetails plotDetails, Chart chart, Double plankDepth, bool animationEnabled)
        {
            if (Double.IsNaN(width) || Double.IsNaN(height) || width <= 0 || height <= 0) return null;

            Canvas visual, labelCanvas;
           
            if (preExistingPanel == null)
            {
                visual = new Canvas();
                labelCanvas = new Canvas();
            }
            else
            {
                visual = preExistingPanel as Canvas;
                labelCanvas = preExistingPanel.Children[0] as Canvas;
            }

            labelCanvas.Width = width;
            labelCanvas.Height = height;

            visual.Width = width;
            visual.Height = height;

            Canvas columnCanvas = new Canvas() { Width = width, Height = height };

            List<PlotGroup> plotGroupList = (from plots in plotDetails.PlotGroups where plots.RenderAs == RenderAs.StackedColumn select plots).ToList();

            Double depth3d = plankDepth / plotDetails.Layer3DCount * (chart.View3D ? 1 : 0);
            Double visualOffset = depth3d * (plotDetails.SeriesDrawingIndex[plotGroupList[0].DataSeriesList[0]] + 1);
            
            visual.SetValue(Canvas.TopProperty, visualOffset);
            visual.SetValue(Canvas.LeftProperty, -visualOffset);

            List<DataSeries> seriesList = plotDetails.GetSeriesListByRenderAs(RenderAs.StackedColumn);

            Dictionary<Axis, Dictionary<Axis, Int32>> seriesIndex = ColumnChart.GetSeriesIndex(seriesList);

            Double minDiff, widthPerColumn, maxColumnWidth;

            foreach (PlotGroup plotGroup in plotGroupList)
            {
                if (!seriesIndex.ContainsKey(plotGroup.AxisY))
                    continue;

                List<Double> xValuesList = plotGroup.XWiseStackedDataList.Keys.ToList();
                Int32 drawingIndex = seriesIndex[plotGroup.AxisY][plotGroup.AxisX];

                widthPerColumn = CalculateWidthOfEachColumn(chart, plotGroup, width, height, out minDiff, out  maxColumnWidth);

                Double limitingYValue = 0;

                if (plotGroup.AxisY.InternalAxisMinimum > 0)
                    limitingYValue = (Double)plotGroup.AxisY.InternalAxisMinimum;
                if (plotGroup.AxisY.InternalAxisMaximum < 0)
                    limitingYValue = (Double)plotGroup.AxisY.InternalAxisMaximum;

                foreach (Double xValue in xValuesList)
                {   
                    DrawStackedColumnAtXValue(xValue, plotGroup, columnCanvas, labelCanvas,
                        drawingIndex, widthPerColumn, maxColumnWidth, limitingYValue, depth3d, animationEnabled);
                }
            }

            ColumnChart.CreateOrUpdatePlank(chart, plotGroupList[0].AxisY, columnCanvas, depth3d);

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
        /// Draws a All Stacked column under a XValue with a particular dtrawingIndex
        /// </summary>
        /// <param name="xValue"></param>
        /// <param name="plotGroup"></param>
        /// <param name="columnCanvas"></param>
        /// <param name="labelCanvas"></param>
        /// <param name="drawingIndex"></param>
        /// <param name="widthPerColumn"></param>
        /// <param name="maxColumnWidth"></param>
        /// <param name="limitingYValue"></param>
        /// <param name="depth3d"></param>
        /// <param name="animationEnabled"></param>
        private static void DrawStackedColumnAtXValue(Double xValue, PlotGroup plotGroup, Canvas columnCanvas, Canvas labelCanvas, Double drawingIndex, Double widthPerColumn, Double maxColumnWidth, Double limitingYValue, Double depth3d, Boolean animationEnabled)
        {
            Double top;
            Double prevSum = 0;
            Int32 positiveIndex = 1, negativeIndex = 1;
            Boolean isTopOFStack;
            DataPoint dataPointAtTopOfStack = null;

            Double left = Graphics.ValueToPixelPosition(0, columnCanvas.Width, (Double)plotGroup.AxisX.InternalAxisMinimum, (Double)plotGroup.AxisX.InternalAxisMaximum, xValue) + drawingIndex * widthPerColumn - (maxColumnWidth / 2);
            Double bottom = Graphics.ValueToPixelPosition(columnCanvas.Height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, limitingYValue);

            if (plotGroup.XWiseStackedDataList[xValue].Positive.Count > 0)
                dataPointAtTopOfStack = plotGroup.XWiseStackedDataList[xValue].Positive.Last();

            // Plot positive values
            foreach (DataPoint dataPoint in plotGroup.XWiseStackedDataList[xValue].Positive)
            {
                if (!(Boolean)dataPoint.Enabled || Double.IsNaN(dataPoint.InternalYValue))
                    continue;

                isTopOFStack = (dataPoint == dataPointAtTopOfStack);
                top = Graphics.ValueToPixelPosition(columnCanvas.Height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, dataPoint.InternalYValue + prevSum);

                CreateStackedColumnVisual(true, columnCanvas, labelCanvas, dataPoint,
                    isTopOFStack, left, ref top, ref bottom, widthPerColumn, depth3d,
                    ref prevSum, positiveIndex, animationEnabled);
            }

            prevSum = 0;

            top = Graphics.ValueToPixelPosition(columnCanvas.Height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, limitingYValue);

            dataPointAtTopOfStack = null;
            
            if (plotGroup.XWiseStackedDataList[xValue].Negative.Count > 0)
                dataPointAtTopOfStack = plotGroup.XWiseStackedDataList[xValue].Negative.Last();

            // Plot negative values
            foreach (DataPoint dataPoint in plotGroup.XWiseStackedDataList[xValue].Negative)
            {
                if (!(Boolean)dataPoint.Enabled || Double.IsNaN(dataPoint.InternalYValue))
                    continue;

                isTopOFStack = (dataPoint == dataPointAtTopOfStack);

                bottom = Graphics.ValueToPixelPosition(columnCanvas.Height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, dataPoint.InternalYValue + prevSum);

                CreateStackedColumnVisual(false, columnCanvas, labelCanvas, dataPoint,
                    isTopOFStack, left, ref top, ref bottom, widthPerColumn, depth3d,
                    ref prevSum, negativeIndex, animationEnabled);
            }
        }

        /// <summary>
        /// Apply animation for StackedColumn chart
        /// </summary>
        /// <param name="column">Column visual reference</param>
        /// <param name="storyboard">Storyboard</param>
        /// <param name="columnParams">Column params</param>
        /// <param name="begin">Animation begin time</param>
        /// <param name="duration">Animation duration</param>
        /// <returns>Storyboard</returns>
        private static Storyboard ApplyStackedColumnChartAnimation(DataSeries dataSeries, Panel column, Storyboard storyboard, RectangularChartShapeParams columnParams, Double begin, Double duration)
        {
            ScaleTransform scaleTransform = new ScaleTransform() { ScaleY = 0 };
            column.RenderTransform = scaleTransform;

            column.RenderTransformOrigin = new Point(0.5, 0.5);

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

            DoubleAnimationUsingKeyFrames growAnimation = AnimationHelper.CreateDoubleAnimation(dataSeries, scaleTransform, "(ScaleTransform.ScaleY)", begin + 0.5, frameTimes, values, splines);

            storyboard.Children.Add(growAnimation);

            return storyboard;
        }

        private static Double CalculateWidthOfEachColumn(Chart chart, PlotGroup plotGroup, Double width, Double height, out Double minDiff, out Double maxColumnWidth)
        {
            Double widthPerColumn;
            PlotDetails plotDetails = chart.PlotDetails;

            Double widthDivisionFactor = plotDetails.DrawingDivisionFactor;
            minDiff = plotDetails.GetMinOfMinDifferencesForXValue(RenderAs.Column, RenderAs.StackedColumn, RenderAs.StackedColumn100);

            if (Double.IsPositiveInfinity(minDiff))
                minDiff = 0;

            maxColumnWidth = Graphics.ValueToPixelPosition(0, width, (Double)plotGroup.AxisX.InternalAxisMinimum, (Double)plotGroup.AxisX.InternalAxisMaximum, minDiff + (Double)plotGroup.AxisX.InternalAxisMinimum) * (1 - ColumnChart.COLUMN_GAP_RATIO);

            widthPerColumn = maxColumnWidth / widthDivisionFactor;

            if (minDiff == 0)
            {
                widthPerColumn = width * .5 / widthDivisionFactor;
                maxColumnWidth = widthPerColumn * widthDivisionFactor;
            }
            else
            {
                widthPerColumn = Graphics.ValueToPixelPosition(0, width, plotGroup.AxisX.InternalAxisMinimum, plotGroup.AxisX.InternalAxisMaximum, minDiff + plotGroup.AxisX.InternalAxisMinimum);
                widthPerColumn *= (1 - ColumnChart.COLUMN_GAP_RATIO);
                maxColumnWidth = widthPerColumn;
                widthPerColumn /= widthDivisionFactor;
            }

            if (!Double.IsNaN(chart.DataPointWidth))
            {
                if (chart.DataPointWidth >= 0)
                {
                    widthPerColumn = maxColumnWidth = chart.DataPointWidth / 100 * chart.PlotArea.Width;

                    maxColumnWidth *= widthDivisionFactor;
                }
            }

            if (maxColumnWidth < 2)
                maxColumnWidth = 2;

            return widthPerColumn;
        }

        public static void UpdateVisualForYValue4StackedColumnChart(Chart chart, DataPoint dataPoint, Boolean isAxisChanged)
        {   
            DataSeries dataSeries = dataPoint.Parent;             // parent of the current DataPoint
            Canvas oldVisual = dataPoint.Faces.Visual as Canvas;  // Old visual for the column
            Double widthOfAcolumn = oldVisual.Width;              // Width of the old column
            Boolean isPositive = (dataPoint.InternalYValue >= 0); // Whether YValue is positive
            Canvas columnCanvas = oldVisual.Parent as Canvas;     // Existing parent canvas of column

            Double depth3d = chart.ChartArea.PLANK_DEPTH / chart.PlotDetails.Layer3DCount * (chart.View3D ? 1 : 0);

            Double oldMarkerTop = Double.NaN;
            Double currentMarkerTop = Double.NaN;

            if (dataPoint.Marker != null && dataPoint.Marker.Visual != null)
                oldMarkerTop = (Double)dataPoint.Marker.Visual.GetValue(Canvas.TopProperty);

            Canvas labelCanvas = (columnCanvas.Parent as Canvas).Children[0] as Canvas;

            // Create new Column with new YValue
            CreateColumnDataPointVisual(columnCanvas, labelCanvas, chart.PlotDetails, dataPoint,
            isPositive, widthOfAcolumn, depth3d, false);

            columnCanvas.Children.Remove(oldVisual);

            if (dataPoint.Storyboard != null)
            {
                dataPoint.Storyboard.Stop();
                dataPoint.Storyboard = null;
            }

            // Update existing Plank
            CreateOrUpdatePlank(chart, dataSeries.PlotGroup.AxisY, columnCanvas, depth3d);

            Boolean animationEnabled = true;

            #region Animate Column

            if (animationEnabled)
            {
                // Calculate scale factor from the old value YValue of the DataPoint

                Double limitingYValue = 0;
                PlotGroup plotGroup = dataSeries.PlotGroup;

                if (plotGroup.AxisY.InternalAxisMinimum > 0)
                    limitingYValue = (Double)plotGroup.AxisY.InternalAxisMinimum;
                if (plotGroup.AxisY.InternalAxisMaximum < 0)
                    limitingYValue = (Double)plotGroup.AxisY.InternalAxisMaximum;

                if (dataPoint.InternalYValue > (Double)plotGroup.AxisY.InternalAxisMaximum)
                    System.Diagnostics.Debug.WriteLine("Max Value greater then axis max");

                Double oldBottom, oldTop, oldColumnHeight;

                if (dataPoint._oldYValue >= 0)
                {
                    oldBottom = Graphics.ValueToPixelPosition(columnCanvas.Height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, limitingYValue);
                    oldTop = Graphics.ValueToPixelPosition(columnCanvas.Height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, dataPoint._oldYValue);
                    oldColumnHeight = Math.Abs(oldTop - oldBottom);
                }
                else
                {
                    oldBottom = Graphics.ValueToPixelPosition(columnCanvas.Height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, dataPoint._oldYValue);
                    oldTop = Graphics.ValueToPixelPosition(columnCanvas.Height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, limitingYValue);
                    oldColumnHeight = Math.Abs(oldTop - oldBottom);
                }

                Double oldScaleFactor = oldColumnHeight / dataPoint.Faces.Visual.Height;

                if (Double.IsInfinity(oldScaleFactor))
                    oldScaleFactor = 0;

                // End Calculate scale factor from the old value YValue of the DataPoint

                dataPoint.Storyboard = new Storyboard();

                if (!Double.IsNaN(oldMarkerTop))
                    currentMarkerTop = (Double)dataPoint.Marker.Visual.GetValue(Canvas.TopProperty);

                if ((dataPoint._oldYValue < 0 && dataPoint.InternalYValue < 0 || dataPoint._oldYValue > 0 && dataPoint.InternalYValue > 0))
                {
                    dataPoint.Storyboard = ApplyColumnChartAnimation(dataPoint.Faces.Visual as Panel, dataPoint.Storyboard, isPositive, 0, new Double[] { 0, 1 }, new Double[] { oldScaleFactor, 1 });

                    if (!Double.IsNaN(oldMarkerTop))
                    {
                        dataPoint.Storyboard = AnimationHelper.ApplyPropertyAnimation(dataPoint.Marker.Visual, "(Canvas.Top)", dataPoint, dataPoint.Storyboard, 0,
                            new Double[] { 0, 1 }, new Double[] { oldMarkerTop, currentMarkerTop },
                            AnimationHelper.GenerateKeySplineList(new Point(0, 0), new Point(1, 1), new Point(0, 1), new Point(0.5, 1)));
                    }
                }
                else
                {   
                    // Top position of the DataPoint with new Value
                    Double currentTop = (Double)dataPoint.Faces.Visual.GetValue(Canvas.TopProperty);

                    // Top position of the Plank (Top position of the Zero Line)
                    Double plankTop = columnCanvas.Height - Graphics.ValueToPixelPosition(0, columnCanvas.Height, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, 0);

                    if (dataPoint._oldYValue < 0)
                    {
                        dataPoint.Storyboard = AnimationHelper.ApplyPropertyAnimation(dataPoint.Faces.Visual, "(Canvas.Top)", dataPoint, dataPoint.Storyboard, 0, new Double[] { 0, 0.5, 0.5, 1 }, new Double[] { plankTop, plankTop, plankTop, currentTop }, null);
                        dataPoint.Storyboard = ApplyColumnChartAnimation(dataPoint.Faces.Visual as Panel, dataPoint.Storyboard, false, 0, new Double[] { 0, 0.5, 0.5, 1 }, new Double[] { oldScaleFactor, 0, 0, 1 });
                    }
                    else
                    {
                        dataPoint.Storyboard = AnimationHelper.ApplyPropertyAnimation(dataPoint.Faces.Visual, "(Canvas.Top)", dataPoint, dataPoint.Storyboard, 0, new Double[] { 0, 0.5, 0.5 }, new Double[] { oldTop, plankTop, plankTop }, null);
                        dataPoint.Storyboard = ApplyColumnChartAnimation(dataPoint.Faces.Visual as Panel, dataPoint.Storyboard, false, 0, new Double[] { 0, 0.5, 0.5, 1 }, new Double[] { oldScaleFactor, 0, 0, 1 });
                    }

                    if (!Double.IsNaN(oldMarkerTop))
                        dataPoint.Storyboard = AnimationHelper.ApplyPropertyAnimation(dataPoint.Marker.Visual, "(Canvas.Top)", dataPoint, dataPoint.Storyboard, 0,
                            new Double[] { 0, 0.5, 0.5, 1 }, new Double[] { oldMarkerTop, plankTop, plankTop, currentMarkerTop },
                            null);
                }

                dataPoint.Storyboard.Begin();
            }


            #endregion Apply Animation
        }
    }
}
