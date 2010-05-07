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

using System.Linq;
using System.Windows.Shapes;

using Visifire.Commons;
namespace Visifire.Charts
{
    public partial class ColumnChart
    {   
        /// <summary>
        /// Creates visual object for a StackedColumn
        /// </summary>
        /// <param name="isPositive">Whether the DataPoint YValue is greater than or equals to 0.</param>
        /// <param name="columnCanvas"></param>
        /// <param name="labelCanvas"></param>
        /// <param name="dataPoint"></param>
        /// <param name="isTopOFStack"></param>
        /// <param name="left"></param>
        /// <param name="top"></param>
        /// <param name="bottom"></param>
        /// <param name="columnWidth"></param>
        /// <param name="depth3d"></param>
        /// <param name="prevSum"></param>
        /// <param name="positiveOrNegativeIndex"></param>
        /// <param name="animationEnabled"></param>
        private static void CreateStackedColumnVisual(Boolean isPositive, Canvas columnCanvas, Canvas labelCanvas, 
            DataPoint dataPoint, Boolean isTopOFStack, Double left, ref Double top, ref Double bottom, Double columnWidth, 
            Double columnHeight, Double depth3d, ref Int32 positiveOrNegativeIndex, Boolean animationEnabled, 
            Double animationBeginTime)
        {   
            PlotGroup plotGroup = dataPoint.Parent.PlotGroup;
            Chart chart = dataPoint.Chart as Chart;         

            Faces column;
            Panel columnVisual = null;

            dataPoint.Parent.Faces = new Faces { Visual = columnCanvas, LabelCanvas = labelCanvas };
            
            if (chart.View3D)
            {   
                column = ColumnChart.Get3DColumn(dataPoint, columnWidth, columnHeight, depth3d, dataPoint.Color, null, null, null, (Boolean)dataPoint.LightingEnabled,
                (BorderStyles)dataPoint.BorderStyle, dataPoint.BorderColor, dataPoint.BorderThickness.Left);
                columnVisual = column.Visual as Panel;
                columnVisual.SetValue(Canvas.ZIndexProperty, ColumnChart.GetStackedColumnZIndex(left, top, (dataPoint.InternalYValue > 0), positiveOrNegativeIndex));
                dataPoint.Faces = column;
            }
            else
            {   
                column = ColumnChart.Get2DColumn(dataPoint, columnWidth, columnHeight, true, false);
                columnVisual = column.Visual as Panel;
                dataPoint.Faces = column;
            }

            ColumnChart.ApplyOrRemoveShadow(chart, dataPoint);

            dataPoint.Faces.LabelCanvas = labelCanvas;

            columnVisual.SetValue(Canvas.LeftProperty, left);
            columnVisual.SetValue(Canvas.TopProperty, top);

            columnCanvas.Children.Add(columnVisual);

            dataPoint.IsTopOfStack = isTopOFStack;

            CreateOrUpdateMarker4VerticalChart(dataPoint, labelCanvas, new Size(columnVisual.Width, columnVisual.Height),
                          left, top);
            
            // labelCanvas.Children.Add(GetMarker(chart, columnParams, dataPoint, left, top));

            DataSeries currentDataSeries;

            // Apply animation
            if (animationEnabled)
            {
                currentDataSeries = dataPoint.Parent;

                if (currentDataSeries.Storyboard == null)
                    currentDataSeries.Storyboard = new Storyboard();

                // Apply animation to the data points dataSeriesIndex.e to the rectangles that form the columns
                currentDataSeries.Storyboard = ApplyStackedColumnChartAnimation(currentDataSeries, columnVisual, dataPoint.Parent.Storyboard, animationBeginTime, 0.5);
            }

            if (isPositive)
                bottom = top;
            else
                top = bottom;

            if (isPositive)
                dataPoint._visualPosition = new Point(left + columnWidth / 2, top);
            else
                dataPoint._visualPosition = new Point(left + columnWidth / 2, bottom);

            dataPoint.Faces.Visual.Opacity = dataPoint.Opacity * dataPoint.Parent.Opacity;
            dataPoint.AttachEvent2DataPointVisualFaces(dataPoint);
            dataPoint.AttachEvent2DataPointVisualFaces(dataPoint.Parent);
            dataPoint._parsedToolTipText = dataPoint.TextParser(dataPoint.ToolTipText);
            //dataPoint.AttachToolTip(chart, dataPoint, dataPoint.Faces.VisualComponents);
            //dataPoint.AttachHref(chart, dataPoint.Faces.VisualComponents, dataPoint.Href, (HrefTargets)dataPoint.HrefTarget);
            if(!chart.IndicatorEnabled)
                dataPoint.AttachToolTip(chart, dataPoint, dataPoint.Faces.Visual);
            dataPoint.AttachHref(chart, dataPoint.Faces.Visual, dataPoint.Href, (HrefTargets)dataPoint.HrefTarget);
            
            dataPoint.SetCursor2DataPointVisualFaces();
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
        internal static Canvas GetVisualObjectForStackedColumnChart(RenderAs chartType, Panel preExistingPanel, Double width, Double height, PlotDetails plotDetails, Chart chart, Double plankDepth, bool animationEnabled)
        {   
            if (Double.IsNaN(width) || Double.IsNaN(height) || width <= 0 || height <= 0) return null;

            Canvas visual, labelCanvas, columnCanvas;
            RenderHelper.RepareCanvas4Drawing(preExistingPanel as Canvas, out visual, out labelCanvas, out columnCanvas, width, height);

            List<PlotGroup> plotGroupList = (from plots in plotDetails.PlotGroups where plots.RenderAs == chartType select plots).ToList();

            Double depth3d = plankDepth / plotDetails.Layer3DCount * (chart.View3D ? 1 : 0);
            Double visualOffset = depth3d * (plotDetails.SeriesDrawingIndex[plotGroupList[0].DataSeriesList[0]] + 1);
            
            visual.SetValue(Canvas.TopProperty, visualOffset);
            visual.SetValue(Canvas.LeftProperty, -visualOffset);

            List<DataSeries> seriesList = plotDetails.GetSeriesListByRenderAs(chartType);

            Dictionary<Axis, Dictionary<Axis, Int32>> seriesIndex = ColumnChart.GetSeriesIndex(seriesList);

            Double minDiff, widthPerColumn, maxColumnWidth;
            DataSeries currentDataSeries = null;

            foreach (PlotGroup plotGroup in plotGroupList)
            {
                if (!seriesIndex.ContainsKey(plotGroup.AxisY))
                    continue;
                
                currentDataSeries = plotGroup.DataSeriesList[0];

                List<Double> xValuesList = plotGroup.XWiseStackedDataList.Keys.ToList();
                plotGroup.DrawingIndex = seriesIndex[plotGroup.AxisY][plotGroup.AxisX];

                widthPerColumn = CalculateWidthOfEachStackedColumn(chart, plotGroup, width, out minDiff, out  maxColumnWidth);

                Double limitingYValue = 0;

                if (plotGroup.AxisY.InternalAxisMinimum > 0)
                    limitingYValue = (Double)plotGroup.AxisY.InternalAxisMinimum;
                if (plotGroup.AxisY.InternalAxisMaximum < 0)
                    limitingYValue = (Double)plotGroup.AxisY.InternalAxisMaximum;

                foreach (Double xValue in xValuesList)
                {
                    DrawStackedColumnsAtXValue(chartType, xValue, plotGroup, columnCanvas, labelCanvas,
                        plotGroup.DrawingIndex, widthPerColumn, maxColumnWidth, limitingYValue, depth3d, animationEnabled);
                }
            }

            // Apply animation
            if (animationEnabled && currentDataSeries != null)
            {
               if (currentDataSeries.Storyboard == null)
                    currentDataSeries.Storyboard = new Storyboard();

               // Apply animation to the marker and labels
               currentDataSeries.Storyboard = AnimationHelper.ApplyOpacityAnimation(labelCanvas, currentDataSeries, currentDataSeries.Storyboard, 1, 1, 0, 1);
            }

            if (plotGroupList.Count > 0 && plotGroupList[0].XWiseStackedDataList.Keys.Count > 0)
            {
                ColumnChart.CreateOrUpdatePlank(chart, plotGroupList[0].AxisY, columnCanvas, depth3d, Orientation.Horizontal);
            }

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

            RectangleGeometry clipRectangle = new RectangleGeometry();
            clipRectangle.Rect = new Rect(0, -chart.ChartArea.PLANK_DEPTH - (chart.View3D ? 0 : 5), width + chart.ChartArea.PLANK_DEPTH, height + chart.ChartArea.PLANK_DEPTH + chart.ChartArea.PLANK_THICKNESS + (chart.View3D ? 0 : 10));
            visual.Clip = clipRectangle;

            return visual;
        }

        /// <summary>
        /// Draws a All Stacked column under a XValue with a particular dtrawingIndex
        /// </summary>
        /// <param name="xValue">XValue</param>
        /// <param name="plotGroup">PlotGroup reference</param>
        /// <param name="columnCanvas">Parent canvas of StackedColumn visuals</param>
        /// <param name="labelCanvas">Parent canvas of StackedColumn labels</param>
        /// <param name="drawingIndex">Drawing index of the DataSeries</param>
        /// <param name="widthPerColumn">Width par each column</param>
        /// <param name="maxColumnWidth">Max width of a Column</param>
        /// <param name="limitingYValue">Limiting value</param>
        /// <param name="depth3d">3d depth for 3d charts</param>
        /// <param name="animationEnabled">Whether animation for chart is enabled</param>
        private static void DrawStackedColumnsAtXValue(RenderAs chartType, Double xValue, PlotGroup plotGroup, Canvas columnCanvas, Canvas labelCanvas, Double drawingIndex, Double widthPerColumn, Double maxColumnWidth, Double limitingYValue, Double depth3d, Boolean animationEnabled)
        {
            Double top;
            Double prevSum = 0;
            Int32 positiveIndex = 1, negativeIndex = 1;
            Boolean isTopOFStack;
            DataPoint dataPointAtTopOfStack = null;
            Double columnHeight = 0;
            Double absoluteSum = 0;
            Double left = Graphics.ValueToPixelPosition(0, columnCanvas.Width, (Double)plotGroup.AxisX.InternalAxisMinimum, (Double)plotGroup.AxisX.InternalAxisMaximum, xValue) + drawingIndex * widthPerColumn - (maxColumnWidth / 2);
            Double bottom = Graphics.ValueToPixelPosition(columnCanvas.Height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, limitingYValue);

            if (plotGroup.XWiseStackedDataList[xValue].Positive.Count > 0)
                dataPointAtTopOfStack = plotGroup.XWiseStackedDataList[xValue].Positive.Last();
            
            if(chartType == RenderAs.StackedColumn100)
                absoluteSum = plotGroup.XWiseStackedDataList[xValue].AbsoluteYValueSum;
            
            Double animationBeginTime = 0.4;
            Double animationTime = 1.0 / plotGroup.XWiseStackedDataList[xValue].Positive.Count;

            // Plot positive values
            foreach (DataPoint dataPoint in plotGroup.XWiseStackedDataList[xValue].Positive)
            {
                dataPoint.Parent.Faces = new Faces() { Visual = columnCanvas, LabelCanvas = labelCanvas };
                
                if (!(Boolean)dataPoint.Enabled || Double.IsNaN(dataPoint.InternalYValue))
                   continue;

                isTopOFStack = (dataPoint == dataPointAtTopOfStack);

                if (chartType == RenderAs.StackedColumn)
                {
                    top = Graphics.ValueToPixelPosition(columnCanvas.Height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, dataPoint.InternalYValue + prevSum);
                    prevSum += dataPoint.InternalYValue;
                    columnHeight = Math.Abs(top - bottom);
                }
                else // if (chartType == RenderAs.StackedColumn100)
                {
                    Double percentYValue = 0;

                    if(absoluteSum != 0)
                        percentYValue = (dataPoint.InternalYValue / absoluteSum * 100);

                    top = Graphics.ValueToPixelPosition(columnCanvas.Height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, percentYValue + prevSum);
                    columnHeight = Math.Abs(top - bottom);
                    prevSum += percentYValue;
                }

                if (chartType == RenderAs.StackedColumn || chartType == RenderAs.StackedColumn100)
                    CreateStackedColumnVisual(true, columnCanvas, labelCanvas, dataPoint,
                    isTopOFStack, left, ref top, ref bottom, widthPerColumn, columnHeight, depth3d,
                    ref positiveIndex, animationEnabled, animationBeginTime);
                else if (chartType == RenderAs.StackedColumn || chartType == RenderAs.StackedColumn100)
                    BarChart.CreateBarDataPointVisual(dataPoint, labelCanvas, columnCanvas, dataPoint.InternalYValue >= 0, widthPerColumn, depth3d, false);
                
                animationBeginTime += animationTime;
                positiveIndex++;
            }

            prevSum = 0;

            top = Graphics.ValueToPixelPosition(columnCanvas.Height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, limitingYValue);

            dataPointAtTopOfStack = null;

            if (plotGroup.XWiseStackedDataList[xValue].Negative.Count > 0)
            {
                dataPointAtTopOfStack = plotGroup.XWiseStackedDataList[xValue].Negative.Last();
                animationTime = 1.0 / plotGroup.XWiseStackedDataList[xValue].Negative.Count;
                animationBeginTime = 0.4;
            }

            // Plot negative values
            foreach (DataPoint dataPoint in plotGroup.XWiseStackedDataList[xValue].Negative)
            {
                dataPoint.Parent.Faces = new Faces() { Visual = columnCanvas, LabelCanvas = labelCanvas };

                if (!(Boolean)dataPoint.Enabled || Double.IsNaN(dataPoint.InternalYValue))
                    continue;

                isTopOFStack = (dataPoint == dataPointAtTopOfStack);

                if (chartType == RenderAs.StackedColumn)
                {   
                    bottom = Graphics.ValueToPixelPosition(columnCanvas.Height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, dataPoint.InternalYValue + prevSum);
                    prevSum += dataPoint.InternalYValue;
                    columnHeight = Math.Abs(top - bottom);
                }
                else // if (chartType == RenderAs.StackedColumn100)
                {
                    Double percentYValue = (dataPoint.InternalYValue / absoluteSum * 100);
                    bottom = Graphics.ValueToPixelPosition(columnCanvas.Height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, percentYValue + prevSum);
                    columnHeight = Math.Abs(top - bottom);
                    prevSum += percentYValue;
                }

                CreateStackedColumnVisual(false, columnCanvas, labelCanvas, dataPoint,
                    isTopOFStack, left, ref top, ref bottom, widthPerColumn, columnHeight, depth3d,
                    ref negativeIndex, animationEnabled, animationBeginTime);

                animationBeginTime += animationTime;
                negativeIndex--;
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
        private static Storyboard ApplyStackedColumnChartAnimation( DataSeries dataSeries, Panel column, Storyboard storyboard, Double begin, Double duration)
        {
            ScaleTransform scaleTransform = new ScaleTransform() { ScaleY = 0 };
            column.RenderTransform = scaleTransform;

            column.RenderTransformOrigin = new Point(0.5, 0.5);

            DoubleCollection values = Graphics.GenerateDoubleCollection(0, 1.5, 0.75, 1.125, 0.9325, 1);
            DoubleCollection frameTimes = Graphics.GenerateDoubleCollection(0, 0.25 * duration, 0.5 * duration, 0.75 * duration, 1.0 * duration, 1.25 * duration);
            List<KeySpline> splines = AnimationHelper.GenerateKeySplineList
                (
                new Point(0, 0), new Point(1, 0.5), new Point(0, 0), new Point(0.5, 1),
                new Point(0, 0), new Point(1, 0.5), new Point(0, 0), new Point(0.5, 1),
                new Point(0, 0), new Point(1, 0.5), new Point(0, 0), new Point(0.5, 1)
                );
                
            DoubleAnimationUsingKeyFrames growAnimation = AnimationHelper.CreateDoubleAnimation(dataSeries, scaleTransform, "(ScaleTransform.ScaleY)", begin + 0.5, frameTimes, values, splines);
            
            storyboard.Children.Add(growAnimation);

            return storyboard;
        }

        /// <summary>
        /// Calculate width of each stacked column
        /// </summary>
        /// <param name="chart">Chart reference</param>
        /// <param name="plotGroup">PlotGroup</param>
        /// <param name="width">Width of the PlotArea</param>
        /// <param name="height">Width of the PlotArea</param>
        /// <param name="minDiff">Minimum difference between two DataPoints</param>
        /// <param name="maxColumnWidth">Maximum width of a StackedColumn</param>
        /// <returns>Width of a stacked column</returns>
        internal static Double CalculateWidthOfEachStackedColumn(Chart chart, PlotGroup plotGroup, Double heightOrWidth, out Double minDiff, out Double maxColumnWidth)
        {   
            Double widthPerColumn;
            PlotDetails plotDetails = chart.PlotDetails;
            Double widthDivisionFactor = plotDetails.DrawingDivisionFactor;
            
            if(chart.PlotDetails.ChartOrientation == ChartOrientationType.Vertical)
                minDiff = plotDetails.GetMinOfMinDifferencesForXValue(RenderAs.Column, RenderAs.StackedColumn, RenderAs.StackedColumn100);
            else
                minDiff = plotDetails.GetMinOfMinDifferencesForXValue(RenderAs.Bar, RenderAs.StackedBar, RenderAs.StackedBar100);

            if (Double.IsPositiveInfinity(minDiff))
                minDiff = 0;

            maxColumnWidth = Graphics.ValueToPixelPosition(0, heightOrWidth, (Double)plotGroup.AxisX.InternalAxisMinimum, (Double)plotGroup.AxisX.InternalAxisMaximum, minDiff + (Double)plotGroup.AxisX.InternalAxisMinimum) * (1 - ColumnChart.COLUMN_GAP_RATIO);
            widthPerColumn = maxColumnWidth / widthDivisionFactor;

            if (minDiff == 0)
            {
                widthPerColumn = heightOrWidth * .5 / widthDivisionFactor;
                maxColumnWidth = widthPerColumn * widthDivisionFactor;
            }
            else
            {
                widthPerColumn = Graphics.ValueToPixelPosition(0, heightOrWidth, plotGroup.AxisX.InternalAxisMinimum, plotGroup.AxisX.InternalAxisMaximum, minDiff + plotGroup.AxisX.InternalAxisMinimum);
                widthPerColumn *= (1 - ((chart.PlotDetails.ChartOrientation == ChartOrientationType.Vertical) ? ColumnChart.COLUMN_GAP_RATIO : BarChart.BAR_GAP_RATIO));

                maxColumnWidth = widthPerColumn;
                widthPerColumn /= widthDivisionFactor;
            }

            if (!Double.IsNaN(chart.DataPointWidth))
            {
                if (chart.DataPointWidth >= 0)
                {
                    widthPerColumn = maxColumnWidth = chart.DataPointWidth / 100 * ((chart.PlotDetails.ChartOrientation == ChartOrientationType.Vertical) ? chart.PlotArea.Width: chart.PlotArea.Height);

                    maxColumnWidth *= widthDivisionFactor;
                }
            }

            if (maxColumnWidth < 2)
                maxColumnWidth = 2;

            return widthPerColumn;
        }

        public static void UpdateVisualForYValue4StackedBarChart(RenderAs chartType, Chart chart, DataPoint dataPoint, Boolean isAxisChanged)
        {
            //if (dataPoint.Faces == null)
            //    return;

            Boolean animationEnabled = (Boolean)chart.AnimatedUpdate;                                            // Whether the animation for the DataPoint is enabled   
            DataSeries dataSeries = dataPoint.Parent;                                   // parent of the current DataPoint
            Canvas columnCanvas, labelCanvas;

            PlotGroup plotGroup = dataSeries.PlotGroup;                                 // PlotGroup reference

            // Calculate 3d depth for the DataPoints
            Double depth3d = chart.ChartArea.PLANK_DEPTH / chart.PlotDetails.Layer3DCount * (chart.View3D ? 1 : 0);

            // Calculate required parameters for Creating new Stacked Columns
            Double minDiff, heightPerBar, maxBarHeight;

            // Calculate limiting value
            Double limitingYValue = (plotGroup.AxisY.InternalAxisMinimum > 0) ? plotGroup.AxisY.InternalAxisMinimum : (plotGroup.AxisY.InternalAxisMaximum < 0) ? plotGroup.AxisY.InternalAxisMaximum : 0;

            if (dataPoint.Faces == null)
            {
                if (dataSeries != null && dataSeries.Faces != null)
                {
                    labelCanvas = dataSeries.Faces.LabelCanvas as Canvas;
                    columnCanvas = dataSeries.Faces.Visual as Canvas;

                    heightPerBar = CalculateWidthOfEachStackedColumn(chart, plotGroup, columnCanvas.Height, out minDiff, out  maxBarHeight);

                    if (dataPoint.Parent.RenderAs == RenderAs.StackedBar || dataPoint.Parent.RenderAs == RenderAs.StackedBar100)
                    {
                        // Create new Column with new YValue
                        BarChart.DrawStackedBarsAtXValue(chartType, dataPoint.InternalXValue, plotGroup, columnCanvas, labelCanvas, plotGroup.DrawingIndex, heightPerBar, maxBarHeight, limitingYValue, depth3d, false);
                    }
                }
                else
                {
                    UpdateDataSeries(dataSeries, VcProperties.YValue, null);
                    return;
                }
            }

            Canvas dataPointVisual = dataPoint.Faces.Visual as Canvas;                  // Old visual for the column
            labelCanvas = dataPoint.Faces.LabelCanvas;  // Parent canvas of Datapoint label
            columnCanvas = (labelCanvas.Parent as Canvas).Children[1] as Canvas;//dataPointVisual.Parent as Canvas;                     // Existing parent canvas of column

            heightPerBar = CalculateWidthOfEachStackedColumn(chart, plotGroup, columnCanvas.Height, out minDiff, out  maxBarHeight);

            UpdateParentVisualCanvasSize(chart, columnCanvas);
            UpdateParentVisualCanvasSize(chart, labelCanvas);

            // List of effected DataPoints for the current update of YValue property of the DataPoint
            XWiseStackedData effectedDataPoints = plotGroup.XWiseStackedDataList[dataPoint.InternalXValue];

            // Marging all DataPoints in to single a single list of DataPoint
            List<DataPoint> positiveList = effectedDataPoints.Positive.ToList();
            List<DataPoint> negativeList = effectedDataPoints.Negative.ToList();
            List<DataPoint> listOfDataPoint = new List<DataPoint>();
            listOfDataPoint.AddRange(positiveList);
            listOfDataPoint.AddRange(negativeList);

            // Storing reference of old Visual 
            foreach (DataPoint dp in listOfDataPoint)
            {
                if (dp.Marker != null && dp.Marker.Visual != null)
                    dp._oldMarkerPosition = new Point((Double)dp.Marker.Visual.GetValue(Canvas.LeftProperty), (Double)dp.Marker.Visual.GetValue(Canvas.TopProperty));
                    
                if (dp.LabelVisual != null)
                    dp._oldLabelPosition = new Point((Double)dp.LabelVisual.GetValue(Canvas.LeftProperty), (Double)dp.LabelVisual.GetValue(Canvas.TopProperty));
                    
                if (dp.Faces != null)
                {   
                    dp._oldVisual = dp.Faces.Visual;
                    columnCanvas.Children.Remove(dp._oldVisual);
                }
            }

            // Create new Column with new YValue
            BarChart.DrawStackedBarsAtXValue(chartType, dataPoint.InternalXValue, plotGroup, columnCanvas, labelCanvas, plotGroup.DrawingIndex, dataPointVisual.Height, maxBarHeight, limitingYValue, depth3d, false);

            if (dataSeries.ToolTipElement != null)
                dataSeries.ToolTipElement.Hide();

            CreateOrUpdatePlank(chart, dataSeries.PlotGroup.AxisY, columnCanvas, depth3d,
                (dataPoint.Parent.RenderAs == RenderAs.StackedColumn || dataPoint.Parent.RenderAs == RenderAs.StackedColumn100) ? Orientation.Horizontal : Orientation.Vertical);

            Boolean isPositive;

            if (animationEnabled)
            {   
                // Create new Storyboard for animation
                if (dataPoint.Storyboard != null)
                {
                    dataPoint.Storyboard.Stop();
                    //dataPoint.Storyboard = null;
                    dataPoint.Storyboard.Children.Clear();
                }

                if (dataPoint.Storyboard == null)
                    dataPoint.Storyboard = new Storyboard();

                // Whether to animate the top of DataPoint Visual
                Boolean isAnimateLeft = false;

                // Loop through all Datapoints under the PlotGroup of the current DataPoint and apply animation
                foreach (DataPoint dp in listOfDataPoint)
                {   
                    if (dp.Faces == null || dp._oldVisual == null)
                        continue;

                    FrameworkElement newVisual = dp.Faces.Visual;                          // New StackedColumn visual reference of DataPoint
                    Double oldLeft = (Double)dp._oldVisual.GetValue(Canvas.LeftProperty);  // Left of the old visual of the DataPoint
                    Double newLeft = (Double)newVisual.GetValue(Canvas.LeftProperty);      // Left of the new visual of the DataPoint
                    Double oldWidth = dp._oldVisual.Width;                                 // Width of the old visual of the DataPoint
                    Double newWidth = newVisual.Width;                                     // Width of the new visual of the DataPoint
                    Double oldScale = oldWidth / newWidth;                                 // Scale value for the old DataPoint                       

                    if (dp == dataPoint)
                    {   
                        isPositive = (dataPoint._oldYValue < 0 && dataPoint.InternalYValue > 0) ? true :
                            (dataPoint._oldYValue > 0 && dataPoint.InternalYValue < 0) ? false :
                            dp._oldYValue >= 0 ? true : false;
                    }
                    else
                        isPositive = dp.InternalYValue >= 0 ? true : false;
                        
                    if (isPositive)
                        newVisual.RenderTransformOrigin = new Point(0, 0.5);
                    else
                        newVisual.RenderTransformOrigin = new Point(1, 0.5);

                    // Apply new RenderTransform to the DataPoint Visual
                    newVisual.RenderTransform = new ScaleTransform();

                    if (Double.IsInfinity(oldScale) || Double.IsNaN(oldScale))
                        oldScale = 0;
                        
                    if (oldScale != 1)
                        dataPoint.Storyboard = AnimationHelper.ApplyPropertyAnimation(newVisual, "(UIElement.RenderTransform).(ScaleTransform.ScaleX)", dataPoint, dataPoint.Storyboard, 0,
                            new Double[] { 0, 1 }, new Double[] { oldScale, 1 }, null);
                        
                    if ((isAnimateLeft && oldLeft != newLeft) || dp._oldYValue == dp.InternalYValue && oldLeft != newLeft)
                    {   
                        newVisual.SetValue(Canvas.LeftProperty, oldLeft);

                        dataPoint.Storyboard = AnimationHelper.ApplyPropertyAnimation(newVisual, "(Canvas.Left)", dataPoint, dataPoint.Storyboard, 0,
                               new Double[] { 0, 1 }, new Double[] { oldLeft, newLeft }, null);
                    }

                    if (dp == dataPoint)
                        isAnimateLeft = true;

                    // Apply animation to markers if marker exists
                    if (dp.Marker != null && dp.Marker.Visual != null)
                    {   
                        Double markerNewLeft = (Double)dp.Marker.Visual.GetValue(Canvas.LeftProperty);

                        dp.Marker.Visual.SetValue(Canvas.LeftProperty, dp._oldMarkerPosition.X);

                        dataPoint.Storyboard = AnimationHelper.ApplyPropertyAnimation(dp.Marker.Visual, "(Canvas.Left)", dataPoint, dataPoint.Storyboard, 0,
                            new Double[] { 0, 1 }, new Double[] { dp._oldMarkerPosition.X, markerNewLeft }, null);
                    }

                    if (dp.LabelVisual != null)
                    {
                        Double labelNewLeft = (Double)dp.LabelVisual.GetValue(Canvas.LeftProperty);
                        System.Diagnostics.Debug.WriteLine("oldPos=" + dp._oldLabelPosition.X.ToString() + " NewPos=" + labelNewLeft.ToString());
                        dp.LabelVisual.SetValue(Canvas.LeftProperty, dp._oldLabelPosition.X);

                        dataPoint.Storyboard = AnimationHelper.ApplyPropertyAnimation(dp.LabelVisual, "(Canvas.Left)", dataPoint, dataPoint.Storyboard, 0,
                            new Double[] { 0, 1 }, new Double[] { dp._oldLabelPosition.X, labelNewLeft }, null);

                    }

                    dataPoint.Storyboard.SpeedRatio = 2;

                    // Remove old visual of the DataPoint from the columncanvas
                    dp._oldVisual = null;
                }

                dataPoint.Storyboard.SpeedRatio = 2;

                // Begin storyboard animation

#if WPF
                dataPoint.Storyboard.Begin(dataPoint.Chart._rootElement, true);
#else
                dataPoint.Storyboard.Begin();
#endif
            }

            if (columnCanvas.Parent != null)
            {
                Double width = chart.ChartArea.ChartVisualCanvas.Width;
                Double height = chart.ChartArea.ChartVisualCanvas.Height;

                RectangleGeometry clipRectangle = new RectangleGeometry();
              
                clipRectangle.Rect = new Rect(-(chart.View3D ? 0 : 5) - chart.ChartArea.PLANK_THICKNESS, -chart.ChartArea.PLANK_DEPTH, width + chart.ChartArea.PLANK_DEPTH + chart.ChartArea.PLANK_THICKNESS + (chart.View3D ? 0 : 10)
                    , height + chart.ChartArea.PLANK_DEPTH);
                (columnCanvas.Parent as Canvas).Clip = clipRectangle;
            }

            if (dataPoint.Parent.SelectionEnabled && dataPoint.Selected)
                dataPoint.Select(true);
        }
        
        public static void UpdateVisualForYValue4StackedColumnChart(RenderAs chartType, Chart chart, DataPoint dataPoint, Boolean isAxisChanged)
        {
            System.Diagnostics.Debug.WriteLine("Animate--YValue" + dataPoint.YValue.ToString() + " IsAxisChange=" + isAxisChanged.ToString());

            Boolean animationEnabled = (Boolean)chart.AnimatedUpdate;                                            // Whether the animation for the DataPoint is enabled   
            DataSeries dataSeries = dataPoint.Parent;                                   // parent of the current DataPoint
            //dataSeries.Faces.Visual as Canvas;// dataPointVisual.Parent as Canvas;                     // Existing parent canvas of column

            Canvas columnCanvas, labelCanvas;

            // Calculate 3d depth for the DataPoints
            Double depth3d = chart.ChartArea.PLANK_DEPTH / chart.PlotDetails.Layer3DCount * (chart.View3D ? 1 : 0);

            PlotGroup plotGroup = dataSeries.PlotGroup;                                 // PlotGroup reference

            Double width, height;

            // Calculate required parameters for Creating new Stacked Columns
            Double minDiff, widthPerColumn, maxColumnWidth;

            // Calculate limiting value
            Double limitingYValue = (plotGroup.AxisY.InternalAxisMinimum > 0) ? plotGroup.AxisY.InternalAxisMinimum : (plotGroup.AxisY.InternalAxisMaximum < 0) ? plotGroup.AxisY.InternalAxisMaximum : 0;

            if (dataPoint.Faces == null)
            {
                if (dataSeries != null && dataSeries.Faces != null)
                {
                    labelCanvas = dataSeries.Faces.LabelCanvas as Canvas;
                    columnCanvas = dataSeries.Faces.Visual as Canvas;

                    height = labelCanvas.Height;
                    width = labelCanvas.Width;

                    widthPerColumn = CalculateWidthOfEachStackedColumn(chart, plotGroup, width, out minDiff, out  maxColumnWidth);

                    if (dataPoint.Parent.RenderAs == RenderAs.StackedColumn || dataPoint.Parent.RenderAs == RenderAs.StackedColumn100)
                    {
                        // Create new Column with new YValue
                        DrawStackedColumnsAtXValue(chartType, dataPoint.InternalXValue, plotGroup, columnCanvas, labelCanvas, plotGroup.DrawingIndex, widthPerColumn, maxColumnWidth, limitingYValue, depth3d, false);
                    }
                }
                else
                {
                    UpdateDataSeries(dataSeries, VcProperties.YValue, null);
                    return;
                }
            }

            labelCanvas = dataPoint.Faces.LabelCanvas;// (columnCanvas.Parent as Canvas).Children[0] as Canvas; // Parent canvas of Datapoint label
            columnCanvas = (labelCanvas.Parent as Canvas).Children[1] as Canvas;

            height = labelCanvas.Height;
            width = labelCanvas.Width;

            widthPerColumn = CalculateWidthOfEachStackedColumn(chart, plotGroup, width, out minDiff, out  maxColumnWidth);

            Canvas dataPointVisual = dataPoint.Faces.Visual as Canvas;                  // Old visual for the column

            UpdateParentVisualCanvasSize(chart, columnCanvas);
            UpdateParentVisualCanvasSize(chart, labelCanvas);

            // List of effected DataPoints for the current update of YValue property of the DataPoint
            XWiseStackedData effectedDataPoints = plotGroup.XWiseStackedDataList[dataPoint.InternalXValue];

            // Marging all DataPoints in to single a single list of DataPoint
            List<DataPoint> positiveList = effectedDataPoints.Positive.ToList();
            List<DataPoint> negativeList = effectedDataPoints.Negative.ToList();
            List<DataPoint> listOfDataPoint = new List<DataPoint>();
            listOfDataPoint.AddRange(positiveList);
            listOfDataPoint.AddRange(negativeList);

            // Storing reference of old Visual 
            foreach(DataPoint dp in listOfDataPoint)
            {   
                if (dp.Marker != null && dp.Marker.Visual != null)
                    dp._oldMarkerPosition = new Point((Double)dp.Marker.Visual.GetValue(Canvas.LeftProperty), (Double)dp.Marker.Visual.GetValue(Canvas.TopProperty));

                if (dp.LabelVisual != null)
                    dp._oldLabelPosition = new Point((Double)dp.LabelVisual.GetValue(Canvas.LeftProperty), (Double)dp.LabelVisual.GetValue(Canvas.TopProperty));

                if (dp.Faces != null)
                {
                    dp._oldVisual = dp.Faces.Visual;
                    columnCanvas.Children.Remove(dp._oldVisual);
                }
            }

            // Create new Column with new YValue
            DrawStackedColumnsAtXValue(chartType, dataPoint.InternalXValue, plotGroup, columnCanvas, labelCanvas, plotGroup.DrawingIndex, dataPointVisual.Width, maxColumnWidth, limitingYValue, depth3d, false);

            if (dataSeries.ToolTipElement != null)
                dataSeries.ToolTipElement.Hide();

            chart.ChartArea.DisableIndicators();

            CreateOrUpdatePlank(chart, dataSeries.PlotGroup.AxisY, columnCanvas, depth3d,
                (dataPoint.Parent.RenderAs == RenderAs.StackedColumn || dataPoint.Parent.RenderAs == RenderAs.StackedColumn100) ? Orientation.Horizontal : Orientation.Vertical);

            Boolean isPositive;

            if (animationEnabled)
            {
                // Create new Storyboard for animation
                if (dataPoint.Storyboard != null)
                {
                    dataPoint.Storyboard.Stop();
                    //dataPoint.Storyboard = null;
                    dataPoint.Storyboard.Children.Clear();
                }

                if (dataPoint.Storyboard == null)
                    dataPoint.Storyboard = new Storyboard();

                // Whether to animate the top of DataPoint Visual
                Boolean isAnimateTop = false;

                // Loop through all Datapoints under the PlotGroup of the current DataPoint and apply animation
                foreach (DataPoint dp in listOfDataPoint)
                {
                    if (dp.Faces == null || dp._oldVisual == null)
                        continue;

                    FrameworkElement newVisual = dp.Faces.Visual;                       // New StackedColumn visual reference of DataPoint
                    Double oldTop = (Double)dp._oldVisual.GetValue(Canvas.TopProperty); // Top of the old visual of the DataPoint
                    Double newTop = (Double)newVisual.GetValue(Canvas.TopProperty);     // Top of the new visual of the DataPoint
                    Double oldHeight = dp._oldVisual.Height;                            // Height of the old visual of the DataPoint
                    Double newHeight = newVisual.Height;                                // Height of the new visual of the DataPoint
                    Double oldScale = oldHeight / newHeight;                            // Scale value for the old DataPoint                       

                    System.Diagnostics.Debug.WriteLine("DataPoint--oldTop =" + oldTop.ToString() + " newTop=" + newTop.ToString() + "oldYValue=" + dp._oldYValue.ToString() + " newYValue=" + dp.InternalYValue.ToString());

                    if (dp == dataPoint)
                    {   
                        isPositive = (dataPoint._oldYValue < 0 && dataPoint.InternalYValue > 0) ? true : 
                            (dataPoint._oldYValue > 0 && dataPoint.InternalYValue < 0) ? false : 
                            dp._oldYValue >= 0 ? true : false;
                    }
                    else
                        isPositive = dp.InternalYValue >= 0 ? true : false;

                    if (isPositive)
                        newVisual.RenderTransformOrigin = new Point(0.5, 1);
                    else
                        newVisual.RenderTransformOrigin = new Point(0.5, 0);

                    // Apply new RenderTransform to the DataPoint Visual
                    newVisual.RenderTransform = new ScaleTransform();


                    if(Double.IsInfinity(oldScale) || Double.IsNaN(oldScale))
                        oldScale = 0;

                    // if (oldScale > 1)
                    //    oldScale = 1.1;

                    if (oldScale != 1)
                        dataPoint.Storyboard = AnimationHelper.ApplyPropertyAnimation(newVisual, "(UIElement.RenderTransform).(ScaleTransform.ScaleY)", dataPoint, dataPoint.Storyboard, 0,
                            new Double[] { 0, 1 }, new Double[] { oldScale, 1 }, null);

                    if ((isAnimateTop && oldTop != newTop) || (dp._oldYValue == dp.InternalYValue && oldTop != newTop))
                    {   
                        System.Diagnostics.Debug.WriteLine("Animate Top ----");
                        newVisual.SetValue(Canvas.TopProperty, oldTop);
                        dataPoint.Storyboard = AnimationHelper.ApplyPropertyAnimation(newVisual, "(Canvas.Top)", dataPoint, dataPoint.Storyboard, 0,
                              new Double[] { 0, 1 }, new Double[] { oldTop, newTop }, null);
                    }

                    if (dp == dataPoint)
                        isAnimateTop = true;

                    // Apply animation to markers if marker exists
                    if (dp.Marker != null && dp.Marker.Visual != null)
                    {
                        Double markerNewTop = (Double)dp.Marker.Visual.GetValue(Canvas.TopProperty);

                        dataPoint.Storyboard = AnimationHelper.ApplyPropertyAnimation(dp.Marker.Visual, "(Canvas.Top)", dataPoint, dataPoint.Storyboard, 0,
                            new Double[] { 0, 1 }, new Double[] { dp._oldMarkerPosition.Y, markerNewTop }, null);

                        dp.Marker.Visual.SetValue(Canvas.TopProperty, dp._oldMarkerPosition.Y);
                    }

                    if (dp.LabelVisual != null)
                    {   
                        Double labelNewTop = (Double)dp.LabelVisual.GetValue(Canvas.TopProperty);

                        dataPoint.Storyboard = AnimationHelper.ApplyPropertyAnimation(dp.LabelVisual, "(Canvas.Top)", dataPoint, dataPoint.Storyboard, 0,
                            new Double[] { 0, 1 }, new Double[] { dp._oldLabelPosition.Y, labelNewTop }, null);

                        dp.LabelVisual.SetValue(Canvas.TopProperty, dp._oldLabelPosition.Y);
                    }

                    dataPoint.Storyboard.SpeedRatio = 2;

                    // Remove old visual of the DataPoint from the columncanvas
                    dp._oldVisual = null;
                }

                dataPoint.Storyboard.SpeedRatio = 2;

                // Begin storyboard animation

#if WPF
                dataPoint.Storyboard.Begin(dataPoint.Chart._rootElement, true);
#else
                dataPoint.Storyboard.Begin();
#endif
            }

            if (columnCanvas.Parent != null)
            {
                width = chart.ChartArea.ChartVisualCanvas.Width;
                height = chart.ChartArea.ChartVisualCanvas.Height;

                RectangleGeometry clipRectangle = new RectangleGeometry();
                clipRectangle.Rect = new Rect(0, -chart.ChartArea.PLANK_DEPTH - (chart.View3D ? 0 : 5), width + chart.ChartArea.PLANK_DEPTH, height + chart.ChartArea.PLANK_DEPTH + chart.ChartArea.PLANK_THICKNESS + (chart.View3D ? 0 : 10));
                (columnCanvas.Parent as Canvas).Clip = clipRectangle;
            }

            if (dataPoint.Parent.SelectionEnabled && dataPoint.Selected)
                dataPoint.Select(true);
        }
    }
}
