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

using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Linq;
using System.Collections.Generic;
using Visifire.Commons;

namespace Visifire.Charts
{
    /// <summary>
    /// Visifire.Charts.PyramidChart class
    /// </summary>
    internal static class PyramidChart
    {
        /// <summary>
        /// Returns the visual object for pyramid chart 
        /// </summary>
        /// <param name="width">PlotArea width</param>
        /// <param name="height">PlotArea height</param>
        /// <param name="plotDetails">PlotDetails</param>
        /// <param name="seriesList">List of line series</param>
        /// <param name="chart">Chart</param>
        /// <param name="animationEnabled">Whether animation is enabled</param>
        /// <param name="isStreamLine">Whether pyramid chart is a Streamline pyramid chart</param>
        /// <returns></returns>
        public static Grid GetVisualObjectForPyramidChart(Double width, Double height, PlotDetails plotDetails, List<DataSeries> seriesList, Chart chart, bool animationEnabled)
        {
            if (seriesList.Count > 0)
            {
                DataSeries pyramidSeries;            // DataSeries used for drawing pyramid chart
                List<DataPoint> pyramidDataPoints;   // DataPoints considered for drawing pyramid chart

                // Select DataSeries for render
                List<DataSeries> selectedDataSeriesList = (from ds in seriesList where (Boolean)ds.Enabled == true select ds).ToList();

                if (selectedDataSeriesList.Count > 0)
                {
                    pyramidSeries = selectedDataSeriesList.First();
                    pyramidSeries.Faces = null;
                }
                else
                    return null;

                List<DataPoint> tempDataPoints = (from dp in pyramidSeries.DataPoints where dp.Enabled == true && dp.YValue >= 0 select dp).ToList();

                if ((from dp in tempDataPoints where dp.YValue == 0 select dp).Count() == tempDataPoints.Count)
                    return null;

                // If number of DataPoints is equals to 0 then dont do any operation
                if (tempDataPoints.Count == 0 || (tempDataPoints.Count == 1 && tempDataPoints[0].YValue == 0))
                    return null;

                pyramidDataPoints = tempDataPoints.ToList();

                // Create pyramid chart canvas
                Grid _pyramidChartGrid = new Grid() { Height = height, Width = width };

                #region Create layout for Pyramid chart and labels

                // Create canvas for label
                Canvas labelCanvas = new Canvas() { Height = height };

                // Create canvas for pyramid
                Canvas pyramidCanvas = new Canvas() { Height = height, HorizontalAlignment = HorizontalAlignment.Left };

                _pyramidChartGrid.Children.Add(pyramidCanvas);
                _pyramidChartGrid.Children.Add(labelCanvas);

                #endregion

                pyramidSeries.Storyboard = null;

                if ((pyramidSeries.Chart as Chart).AnimationEnabled)
                    pyramidSeries.Storyboard = new Storyboard();

                // Creating labels for 
                CreateLabelsAndSetPyramidCanvasSize(_pyramidChartGrid, labelCanvas, pyramidCanvas, pyramidDataPoints);

                Double minPointHeight = pyramidSeries.MinPointHeight;
                Double yScale = 40;
                Boolean isSameSlantAngle = true;
                Double bottomRadius = 5;
                Double gapRatio = (chart.View3D) ? 0.06 : 0.02;
                
                pyramidCanvas = CreatePyramidChart(_pyramidChartGrid, pyramidSeries, pyramidDataPoints, pyramidCanvas, minPointHeight, chart.View3D, yScale, gapRatio, isSameSlantAngle, bottomRadius, animationEnabled);

                // Here
                // pyramidChartCanvas.Background = new SolidColorBrush(Colors.Red);

                if(chart.View3D)
                    pyramidCanvas.Margin = new Thickness(0, - yScale / 2, 0, 0);

                RectangleGeometry clipRectangle = new RectangleGeometry();
                clipRectangle.Rect = new Rect(0, 0, width, height);
                _pyramidChartGrid.Clip = clipRectangle;

                return _pyramidChartGrid;
            }

            return null;
        }

        /// <summary>
        /// Create labels and set width for the label canvas
        /// </summary>
        /// <param name="pyramidChartCanvas">Main Pyramid chart canvas</param>
        /// <param name="labelCanvas">Label canvas for pyramid chart placed in side pyramidChartCanvas</param>
        /// <param name="pyramidSeries">DataSeries reference</param>
        private static void CreateLabelsAndSetPyramidCanvasSize(Grid pyramidChartCanvas, Canvas labelCanvas, Canvas pyramidCanvas, List<DataPoint> pyramidDataPoints)
        {
            Int32 index = 0;
            Double totalLabelsHeight = 0;
            _streamLineParentTitleSize = new Size(0, 0);

            labelCanvas.Width = 0;

            for (; index < pyramidDataPoints.Count; index++)
            {
                // Create label for a pyramid slice
                pyramidDataPoints[index].LabelVisual = CreateLabelForDataPoint(pyramidDataPoints[index], index);

                // Calculate label size
                Size labelSize = Graphics.CalculateVisualSize(pyramidDataPoints[index].LabelVisual);
                labelSize.Width += 2.5;
                
                if ((Boolean)pyramidDataPoints[index].LabelEnabled && pyramidDataPoints[index].LabelStyle == LabelStyles.OutSide)
                    totalLabelsHeight += labelSize.Height;

                if (labelSize.Width > labelCanvas.Width && (Boolean)pyramidDataPoints[index].LabelEnabled && pyramidDataPoints[index].LabelStyle == LabelStyles.OutSide) // && !(isStreamLine && index == 0))
                    labelCanvas.Width = labelSize.Width;

                pyramidDataPoints[index].LabelVisual.Height = labelSize.Height;
                pyramidDataPoints[index].LabelVisual.Width = labelSize.Width;
            }

            labelCanvas.Width += Chart.BEVEL_DEPTH;

            pyramidCanvas.Width = pyramidChartCanvas.Width - labelCanvas.Width;
            labelCanvas.SetValue(Canvas.LeftProperty, pyramidCanvas.Width);
        }

        /// <summary>
        /// Create labels for DataPoint
        /// </summary>
        /// <param name="dataPoint">DataPoint</param>
        /// <returns>Border</returns>
        private static Border CreateLabelForDataPoint(DataPoint dataPoint, Int32 sliceIndex)
        {
            Title title = new Title()
            {
                IsNotificationEnable = false,
                Chart = dataPoint.Chart,
                Text = dataPoint.TextParser(dataPoint.LabelText),
                InternalFontSize = (Double)dataPoint.LabelFontSize,
                InternalFontColor = Chart.CalculateDataPointLabelFontColor(dataPoint.Chart as Chart, dataPoint, dataPoint.LabelFontColor, (LabelStyles)dataPoint.LabelStyle),
                InternalFontFamily = dataPoint.LabelFontFamily,
                InternalFontStyle = (FontStyle)dataPoint.LabelFontStyle,
                InternalFontWeight = (FontWeight)dataPoint.LabelFontWeight,
                InternalBackground = dataPoint.LabelBackground
            };

            title.CreateVisualObject(new ElementData() { Element = dataPoint });

            if (!(Boolean)dataPoint.LabelEnabled)
                title.Visual.Visibility = Visibility.Collapsed;

            return title.Visual;
        }

        /// <summary>
        /// Creates the pyramid Chart
        /// </summary>
        /// <param name="dataSeries">DataSeries reference</param>
        /// <param name="dataPoints">List of sorted reference(If required)</param>
        /// <param name="isStreamLine">Whether its a streamline pyramid chart</param>
        /// <param name="pyramidCanvas">Pyramid Canvas reference</param>
        /// <param name="minPointHeight">Min height of a pyramid slice</param>
        /// <param name="is3D">Whether chart is a 3D chart</param>
        /// <param name="yScale">YScale of the chart</param>
        /// <param name="gapRatio">Gap between two data points while a particular datapoint is Exploded</param>
        /// <param name="isSameSlantAngle">Whether the same slant angle to be used while drawing each slice</param>
        /// <param name="bottomRadius">Bottom most raduis of a pyramid</param>
        /// <param name="animationEnabled">Whether animation is enabled for chart</param>
        /// <returns>Canvas with pyramid</returns>
        private static Canvas CreatePyramidChart(Grid _pyramidChartGrid, DataSeries dataSeries, List<DataPoint> dataPoints, Canvas pyramidCanvas, Double minPointHeight, Boolean is3D, Double yScale, Double gapRatio, Boolean isSameSlantAngle, Double bottomRadius, Boolean animationEnabled)
        {
            Boolean isAnimationEnabled = (dataSeries.Chart as Chart).AnimationEnabled;
            Double plotHeight = pyramidCanvas.Height;
            Double plotWidth = pyramidCanvas.Width;

            // Canvas pyramidCanvas = new Canvas() { Height = plotHeight, Width = plotWidth }; //, Background = new SolidColorBrush(Colors.LightGray) };

            TriangularChartSliceParms[] pyramidSlices = CalculatePyramidSliceParmsInfo(dataSeries, dataPoints, plotHeight, plotWidth - Chart.BEVEL_DEPTH, minPointHeight, is3D, yScale, gapRatio, isSameSlantAngle, bottomRadius);
            dataSeries.VisualParams = pyramidSlices;
            
            Double topRadius = plotWidth / 2;
            Int32 zIndex = pyramidSlices.Count() + 1;

            Random rand = new Random(DateTime.Now.Millisecond);
            Brush fillColor;
            Int32 sliceCount = pyramidSlices.Count();
            Double totalPyramidActualHeight = 0;
            
            for (Int32 index = 0; index < sliceCount; index++)
            {
                pyramidSlices[index].FillType = dataSeries.FillType;
                fillColor = pyramidSlices[index].DataPoint.Color;

                Double yScaleTop = yScale * (pyramidSlices[index].TopRadius / topRadius);
                Double yScaleBottom = yScale * (pyramidSlices[index].BottomRadius / topRadius);

                if (Double.IsNaN(yScaleTop))
                    yScaleTop = 0.0000001;

                if (Double.IsNaN(yScaleBottom))
                    yScaleBottom = 0.0000001;

                Canvas sliceCanvas = GetPyramidSliceVisual(index, topRadius, is3D, pyramidSlices[index], yScaleTop, yScaleBottom, fillColor, animationEnabled);

                pyramidSlices[index].Top = pyramidSlices[index].TopGap + ((index == 0) ? 0 : (pyramidSlices[index - 1].Top + pyramidSlices[index - 1].Height + pyramidSlices[index - 1].BottomGap));
                
                sliceCanvas.SetValue(Canvas.TopProperty, pyramidSlices[index].Top);
                sliceCanvas.SetValue(Canvas.ZIndexProperty, zIndex--);

                sliceCanvas.Height = pyramidSlices[index].Height;
                sliceCanvas.Width = topRadius * 2;
                totalPyramidActualHeight += (pyramidSlices[index].Height + pyramidSlices[index].TopGap);
                
                pyramidCanvas.Children.Add(sliceCanvas);

                pyramidSlices[index].DataPoint.Faces.Visual = sliceCanvas;

                pyramidSlices[index].DataPoint.VisualParams = pyramidSlices[index];
            }

            CalcutateExplodedPosition(ref pyramidSlices, yScale, dataSeries);

            pyramidCanvas.Height = totalPyramidActualHeight - _streamLineParentTitleSize.Height;

            ArrangeLabels(pyramidSlices, Double.NaN, _pyramidChartGrid.Height);

            return pyramidCanvas;
        }

        /// <summary>
        /// Arrange labels to overcome overlaps
        /// </summary>
        private static void ArrangeLabels(TriangularChartSliceParms[] pyramidSlices, Double width, Double height)
        {
            if (pyramidSlices == null || pyramidSlices.Length < 0)
                return;

            TriangularChartSliceParms[] selectedPyramidSlices = (from fs in pyramidSlices where fs.DataPoint.LabelStyle == LabelStyles.OutSide select fs).ToArray();

            Rect baseArea = new Rect(0, 0, width, height);
            Rect[] labelInfo = new Rect[selectedPyramidSlices.Length];

            for (Int32 index = 0; index < selectedPyramidSlices.Length; index++)
            {
                Double left = (Double)selectedPyramidSlices[index].DataPoint.LabelVisual.GetValue(Canvas.LeftProperty);
                Double top = (Double)selectedPyramidSlices[index].DataPoint.LabelVisual.GetValue(Canvas.TopProperty);

                labelInfo[index] = new Rect
                    (left,
                    selectedPyramidSlices[index].Top + top,
                    selectedPyramidSlices[index].DataPoint.LabelVisual.Width,
                    selectedPyramidSlices[index].DataPoint.LabelVisual.Height);
            }

            Visifire.Commons.LabelPlacementHelper.VerticalLabelPlacement(baseArea, ref labelInfo);

            for (Int32 index = 0; index < selectedPyramidSlices.Length; index++)
            {
                Double labelTop = labelInfo[index].Top - selectedPyramidSlices[index].Top;

                selectedPyramidSlices[index].DataPoint.LabelVisual.SetValue(Canvas.LeftProperty, labelInfo[index].Left);
                selectedPyramidSlices[index].DataPoint.LabelVisual.SetValue(Canvas.TopProperty, labelTop);

                selectedPyramidSlices[index].LabelLineEndPoint = new Point(selectedPyramidSlices[index].LabelLineEndPoint.X, labelTop + selectedPyramidSlices[index].DataPoint.LabelVisual.Height / 2);
                UpdateLabelLineEndPoint(selectedPyramidSlices[index]);
            }
        }

        private static void UpdateLabelLineEndPoint(TriangularChartSliceParms pyramidSlice)
        {
            Path labelLine = pyramidSlice.DataPoint.LabelLine;

            if (labelLine != null && labelLine.Data != null)
            {
                LineSegment ls = (((labelLine.Data as PathGeometry).Figures[0] as PathFigure).Segments[0] as LineSegment);
                ls.Point = pyramidSlice.LabelLineEndPoint;
            }
        }

        /// <summary>
        /// Create exploding in animation 
        /// </summary>
        /// <param name="dataPoint">DataPoint</param>
        /// <param name="storyboard">Stroyboard used for animation</param>
        /// <param name="pathElements">Path elements reference</param>
        /// <param name="label">Label reference</param>
        /// <param name="labelLine">Label line reference</param>
        /// <param name="unExplodedPoints">Unexploded points</param>
        /// <param name="explodedPoints">Exploded points</param>
        /// <param name="xOffset">X offset</param>
        /// <param name="yOffset">Y offset</param>
        /// <returns>Storyboard</returns>
        internal static Storyboard CreateExplodingAnimation(DataSeries dataSeries, DataPoint dataPoint, Storyboard storyboard, Panel visual, Double targetValue, Double beginTime)
        {
            if (storyboard == null)
                storyboard = new Storyboard();

#if WPF
            if (storyboard != null && storyboard.GetValue(System.Windows.Media.Animation.Storyboard.TargetProperty) != null)
                storyboard.Stop();
#else
            if (storyboard != null)
                storyboard.Stop();
#endif

            //Double fromValue = (Double) visual.GetValue(Canvas.TopProperty);
            DoubleCollection values = Graphics.GenerateDoubleCollection(targetValue);
            DoubleCollection frames = Graphics.GenerateDoubleCollection(.2);
            List<KeySpline> splines = AnimationHelper.GenerateKeySplineList(frames.Count);
            DoubleAnimationUsingKeyFrames topAnimation = PieChart.CreateDoubleAnimation(dataSeries, dataPoint, visual, "(Canvas.Top)", beginTime, frames, values, splines);
            storyboard.Children.Add(topAnimation);

            return storyboard;
        }

        internal static Storyboard CreateUnExplodingAnimation(DataSeries dataSeries, DataPoint dataPoint, Storyboard storyboard, Panel visual, Double targetValue)
        {
#if WPF
            if (storyboard != null && storyboard.GetValue(System.Windows.Media.Animation.Storyboard.TargetProperty) != null)
                storyboard.Stop();
#else
            if (storyboard != null)
                storyboard.Stop();
#endif

            Double fromValue = (Double)visual.GetValue(Canvas.TopProperty);
            DoubleCollection values = Graphics.GenerateDoubleCollection(fromValue, targetValue);
            DoubleCollection frames = Graphics.GenerateDoubleCollection(0, .2);
            List<KeySpline> splines = AnimationHelper.GenerateKeySplineList(frames.Count);
            DoubleAnimationUsingKeyFrames topAnimation = PieChart.CreateDoubleAnimation(dataSeries, dataPoint, visual, "(Canvas.Top)", 0, frames, values, splines);
            storyboard.Children.Add(topAnimation);

            return storyboard;
        }

        /// <summary>
        /// Calculates Exploded DataPoint Positions
        /// </summary>
        /// <param name="pyramidSlices"></param>
        private static void CalcutateExplodedPosition(ref TriangularChartSliceParms[] pyramidSlices, Double yScale, DataSeries dataSeries)
        {
            Int32 sliceCount = pyramidSlices.Count();
            Int32 index = 0;

            if (pyramidSlices[0].DataPoint.Parent.Exploded)
            {
                if (!pyramidSlices[0].DataPoint.Chart.IsInDesignMode)
                {
                    Int32 midIndex = sliceCount / 2;
                    Double beginTime = 0.4;

                    if ((dataSeries.Chart as Chart).ChartArea._isFirstTimeRender)
                        beginTime = 1;

                    for (index = midIndex; index >= 0; index--)
                    {
                        if (index < 0)
                            break;

                        Double yPosition = pyramidSlices[index].Top - (midIndex - index) * _singleGap;// -yScale / 2;
                        dataSeries.Storyboard = CreateExplodingAnimation(dataSeries, pyramidSlices[index].DataPoint, dataSeries.Storyboard, pyramidSlices[index].DataPoint.Faces.Visual as Panel, yPosition, beginTime);
                    }

                    for (index = midIndex + 1; index < sliceCount; index++)
                    {
                        Double yPosition = pyramidSlices[index].Top + (index - midIndex) * _singleGap;// -yScale / 2;
                        dataSeries.Storyboard = CreateExplodingAnimation(dataSeries, pyramidSlices[index].DataPoint, dataSeries.Storyboard, pyramidSlices[index].DataPoint.Faces.Visual as Panel, yPosition, beginTime);
                    }

                    if (dataSeries.Chart != null && !(dataSeries.Chart as Chart).ChartArea._isFirstTimeRender)
#if WPF
                        dataSeries.Storyboard.Begin(dataSeries.Chart._rootElement, true);
#else
                        dataSeries.Storyboard.Stop();
                        dataSeries.Storyboard.Begin();
#endif

                }
            }
            else
            {
                Storyboard unExplodeStoryBoard = new Storyboard();

                for (; index < sliceCount; index++)
                {
                    pyramidSlices[index].ExplodedPoints = new List<Point>();
                    pyramidSlices[index].DataPoint.ExplodeAnimation = new Storyboard();
                    // For top slice
                    if (index == 0)
                    {
                        pyramidSlices[index].ExplodedPoints.Add(new Point(0, pyramidSlices[index].Top - _singleGap / 2));

                        pyramidSlices[index].DataPoint.ExplodeAnimation = CreateExplodingAnimation(dataSeries, pyramidSlices[index].DataPoint, pyramidSlices[index].DataPoint.ExplodeAnimation, pyramidSlices[index].DataPoint.Faces.Visual as Panel, (pyramidSlices[index].Top - _singleGap / 2), 0);
                        //unExplodeStoryBoard = CreateExplodingAnimation(unExplodeStoryBoard, pyramidSlices[index].DataPoint.Faces.Visual as Panel, pyramidSlices[index].Top);

                        for (Int32 i = 1; i < pyramidSlices.Length; i++)
                        {
                            pyramidSlices[index].ExplodedPoints.Add(new Point(0, pyramidSlices[i].Top + _singleGap / 2));
                            pyramidSlices[index].DataPoint.ExplodeAnimation = CreateExplodingAnimation(dataSeries, pyramidSlices[index].DataPoint, pyramidSlices[index].DataPoint.ExplodeAnimation, pyramidSlices[i].DataPoint.Faces.Visual as Panel, (pyramidSlices[i].Top + _singleGap / 2), 0);
                            //unExplodeStoryBoard = CreateExplodingAnimation(unExplodeStoryBoard, pyramidSlices[i].DataPoint.Faces.Visual as Panel, pyramidSlices[i].Top);
                        }

                    }
                    // For bottom slice
                    else if (index == pyramidSlices.Length - 1)
                    {
                        Int32 i = 0;

                        for (; i < index; i++)
                        {
                            pyramidSlices[index].ExplodedPoints.Add(new Point(0, pyramidSlices[i].Top - _singleGap / 2 + _singleGap / 6));
                            pyramidSlices[index].DataPoint.ExplodeAnimation = CreateExplodingAnimation(dataSeries, pyramidSlices[index].DataPoint, pyramidSlices[index].DataPoint.ExplodeAnimation, pyramidSlices[i].DataPoint.Faces.Visual as Panel, (pyramidSlices[i].Top - _singleGap / 2 + _singleGap / 6), 0);
                        }

                        pyramidSlices[index].ExplodedPoints.Add(new Point(0, pyramidSlices[i].Top + _singleGap / 2 + _singleGap / 6));
                        pyramidSlices[index].DataPoint.ExplodeAnimation = CreateExplodingAnimation(dataSeries, pyramidSlices[index].DataPoint, pyramidSlices[i].DataPoint.ExplodeAnimation, pyramidSlices[i].DataPoint.Faces.Visual as Panel, (pyramidSlices[i].Top + _singleGap / 2 + _singleGap / 6), 0);
                    }
                    // For other slice
                    else
                    {
                        Int32 i;

                        for (i = 0; i < index; i++)
                        {
                            pyramidSlices[index].ExplodedPoints.Add(new Point(0, pyramidSlices[i].Top - _singleGap / 2));
                            pyramidSlices[index].DataPoint.ExplodeAnimation = CreateExplodingAnimation(dataSeries, pyramidSlices[index].DataPoint, pyramidSlices[index].DataPoint.ExplodeAnimation, pyramidSlices[i].DataPoint.Faces.Visual as Panel, (pyramidSlices[i].Top - _singleGap / 2), 0);
                        }

                        pyramidSlices[index].ExplodedPoints.Add(new Point(0, pyramidSlices[i].Top));
                        pyramidSlices[index].DataPoint.ExplodeAnimation = CreateExplodingAnimation(dataSeries, pyramidSlices[index].DataPoint, pyramidSlices[index].DataPoint.ExplodeAnimation, pyramidSlices[index].DataPoint.Faces.Visual as Panel, pyramidSlices[index].Top, 0);

                        for (++i; i < pyramidSlices.Length; i++)
                        {
                            pyramidSlices[index].ExplodedPoints.Add(new Point(0, pyramidSlices[i].Top + _singleGap / 2));
                            pyramidSlices[index].DataPoint.ExplodeAnimation = CreateExplodingAnimation(dataSeries, pyramidSlices[index].DataPoint, pyramidSlices[index].DataPoint.ExplodeAnimation, pyramidSlices[i].DataPoint.Faces.Visual as Panel, (pyramidSlices[i].Top + _singleGap / 2), 0);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Calculate pyramidSliceParms information
        /// </summary>
        /// <param name="dataSeries">DataSeries reference</param>
        /// <param name="dataPoints">List of sorted reference(If required)</param>
        /// <param name="plotHeight">Total height of the plot canvas</param>
        /// <param name="plotWidth">Total width of the plot canvas</param>
        /// <param name="minPointHeight">Min height of a pyramid slice</param>
        /// <param name="is3D">Whether chart is a 3D chart</param>
        /// <param name="yScale">YScale of the chart</param>
        /// <param name="gapRatio">Gap between two data points while a particular datapoint is Exploded</param>
        /// <param name="isSameSlantAngle">Whether the same slant angle to be used while drawing each slice</param>
        /// <param name="bottomRadius">Bottom most raduis of a pyramid</param>
        /// <returns>TriangularChartSliceParms[]</returns>
        private static TriangularChartSliceParms[] CalculatePyramidSliceParmsInfo(DataSeries dataSeries, List<DataPoint> dataPoints, Double plotHeight, Double plotWidth, Double minPointHeight, Boolean is3D, Double yScale, Double gapRatio, Boolean isSameSlantAngle, Double bottomRadius)
        {
            // Initialize pyramid Slices parameters
            TriangularChartSliceParms[] pyramidSlicesParms;

            // Actual pyramid height
            // For 3d pyramid height will be reduced to maintain yScale
            Double pyramidHeight;

            if (dataSeries.Exploded)
            {
                gapRatio = 0.02;

                // Single gap height
                _singleGap = gapRatio * plotHeight;

                _totalGap = _singleGap * (dataPoints.Count + 1);
            }
            else
            {
                // Single gap height
                _singleGap = gapRatio * plotHeight;

                _totalGap = _singleGap * 2;
            }

            // Actual pyramid height
            // For 3d pyramid height will be reduced to maintain yScale
            pyramidHeight = plotHeight - _totalGap -(is3D ? yScale / 3 : 0) - (is3D ? plotHeight * 0.05:0);

            #region Sectional Pyramid

            // Initialization of pyramidSliceParms
            pyramidSlicesParms = new TriangularChartSliceParms[dataPoints.Count];

            // Calculate sum of values
            Double sum = (from dp in dataPoints select dp.YValue).Sum();

            // Min YValue
            Double min = (from dp in dataPoints select dp.YValue).Min();

            // Pyramid angle
            Double theta = Math.Atan((plotWidth / 2 - bottomRadius) / pyramidHeight);

            // Height of the pyramid drawn considering bottom radius
            pyramidHeight -= Math.Tan(theta) / ((bottomRadius == 0) ? 1 : bottomRadius);

            // Creating prams for each pyramid slice
            for (Int32 index = 0; index < dataPoints.Count; index++)
            {
                pyramidSlicesParms[index] = new TriangularChartSliceParms() { DataPoint = dataPoints[index], TopAngle = Math.PI / 2 - theta, BottomAngle = Math.PI / 2 + theta };

                pyramidSlicesParms[index].Height = pyramidHeight * (dataPoints[index].YValue / sum);
                pyramidSlicesParms[index].TopRadius = index == 0 ? 0 : pyramidSlicesParms[index - 1].BottomRadius;
                pyramidSlicesParms[index].BottomRadius = pyramidSlicesParms[index].TopRadius - pyramidSlicesParms[index].Height * Math.Tan(theta);


                pyramidSlicesParms[index].TopGap = 0; // ((index == 0 && is3D) ? yScale / 2 : 0);

            }

            if (!Double.IsNaN(minPointHeight))
            {
                Boolean isFixedSize = false;    // Whether to fix height of all slice
                Double fixedSliceHeight = 0;

                Double totalSumOfHeight = (from pyramidSlice in pyramidSlicesParms select pyramidSlice.Height).Sum();
                fixedSliceHeight = totalSumOfHeight / pyramidSlicesParms.Length;

                // Calculate minPointHeight in terms of pixel value
                minPointHeight = (minPointHeight / 100) * pyramidHeight;

                // Pyramid slices where height is less than the minPointHeight
                List<TriangularChartSliceParms> fixedHeightPyramidSlices = (from pyramidSlice in pyramidSlicesParms where pyramidSlice.Height < minPointHeight select pyramidSlice).ToList();

                List<TriangularChartSliceParms> variableHeightPyramidSlices = (from pyramidSlice in pyramidSlicesParms
                                                                     where !(from slice in fixedHeightPyramidSlices select slice).Contains(pyramidSlice)
                                                                     select pyramidSlice).ToList();

                if (minPointHeight > fixedSliceHeight || fixedHeightPyramidSlices.Count == pyramidSlicesParms.Count())
                {
                    isFixedSize = true;
                }

                Double sumOfHeightOfSlice2BeFixed = (from pyramidSlice in fixedHeightPyramidSlices select pyramidSlice.Height).Sum();
                Double sumOfVariableHeightSlices = (from pyramidSlice in variableHeightPyramidSlices select pyramidSlice.Height).Sum();

                Double totalHeight2Reduce = minPointHeight * fixedHeightPyramidSlices.Count() - sumOfHeightOfSlice2BeFixed;

                // Creating prams for each pyramid slice
                for (Int32 index = 0; index < dataPoints.Count; index++)
                {
                    if (isFixedSize)
                    {
                        pyramidSlicesParms[index].Height = fixedSliceHeight;
                    }
                    else
                    {
                        if (pyramidSlicesParms[index].Height < minPointHeight)
                            pyramidSlicesParms[index].Height = minPointHeight;
                        else
                            pyramidSlicesParms[index].Height -= totalHeight2Reduce * (pyramidSlicesParms[index].Height / sumOfVariableHeightSlices);
                    }

                    pyramidSlicesParms[index].TopRadius = index == 0 ? 0 : pyramidSlicesParms[index - 1].BottomRadius;
                    pyramidSlicesParms[index].BottomRadius = pyramidSlicesParms[index].TopRadius - pyramidSlicesParms[index].Height * Math.Tan(theta);

                    // Set topgap and bottom gap
                    /*if (index == 0 || (Boolean)pyramidSlicesParms[index].DataPoint.Exploded)
                        pyramidSlicesParms[index].TopGap = _singleGap + ((index == 0 && is3D) ? yScale / 2 : 0);
                        
                    if(index == dataPoints.Count -1 || (Boolean) pyramidSlicesParms[index].DataPoint.Exploded)
                        pyramidSlicesParms[index].BottomGap = _singleGap;
                     */
                }
            }

            #endregion

            return pyramidSlicesParms;
        }

        /// <summary>
        /// Returns the visual of a pyramid slice
        /// </summary>
        /// <param name="finnelIndex">Slice index</param>
        /// <param name="topRadius">Top radius of the Pyramid</param>
        /// <param name="is3D">Whether the chart is a 3D Chart</param>
        /// <param name="pyramidSlice">Pyramid Slice reference</param>
        /// <param name="yScaleTop">Top y-scale for 3D slice</param>
        /// <param name="yScaleBottom">Bottom y-scale for 3D slice</param>
        /// <param name="fillColor">Fill Color of the pyramid slice</param>
        /// <param name="animationEnabled">Whether the animation is enabled</param>
        /// <returns>Pyramid slice canvas</returns>
        private static Canvas GetPyramidSliceVisual(Int32 pyramidSliceIndex, Double topRadius, Boolean is3D, TriangularChartSliceParms pyramidSlice, Double yScaleTop, Double yScaleBottom, Brush fillColor, Boolean animationEnabled)
        {
            pyramidSlice.Index = pyramidSliceIndex;
            Canvas sliceCanvas = CreatePyramidSlice(false, topRadius, is3D, pyramidSlice, yScaleTop, yScaleBottom, fillColor, fillColor, fillColor, animationEnabled);

            if ((Boolean)pyramidSlice.DataPoint.LightingEnabled)
            {
                Brush highlightBrush4Stroke = GetLightingBrushForStroke(fillColor, pyramidSlice.Index);
                Brush sideFillColor = (Boolean)pyramidSlice.DataPoint.LightingEnabled ? GetSideLightingBrush(pyramidSlice) : fillColor;
                Brush topFillColor = is3D ? GetTopBrush(fillColor, pyramidSlice) : fillColor;
                Canvas gradientCanvas = CreatePyramidSlice(true, topRadius, is3D, pyramidSlice, yScaleTop, yScaleBottom, sideFillColor, topFillColor, highlightBrush4Stroke, animationEnabled);
                sliceCanvas.Children.Add(gradientCanvas);
            }

            return sliceCanvas;
        }

        /// <summary>
        /// Get stroke brush for Top lighting Ellipse
        /// </summary>
        /// <param name="fillColor"></param>
        /// <param name="pyramidSliceIndex"></param>
        /// <returns></returns>
        private static Brush GetLightingBrushForStroke(Brush fillColor, Int32 pyramidSliceIndex)
        {
            SolidColorBrush fillColorGradientBrush = fillColor as SolidColorBrush;
            Brush highlightBrush4Stroke = fillColor;

            if (pyramidSliceIndex > 0 && fillColorGradientBrush != null)
                highlightBrush4Stroke = new SolidColorBrush(Graphics.GetLighterColor(fillColorGradientBrush.Color, 100));
            else
                highlightBrush4Stroke = Graphics.GetBevelTopBrush(new SolidColorBrush(Graphics.GetLighterColor(fillColorGradientBrush.Color, 100)), 0.12);

            return highlightBrush4Stroke;
        }

        /// <summary>
        /// Get new Brush for Bevel
        /// </summary>
        /// <param name="bevelSideName"></param>
        /// <param name="fullBrush"></param>
        /// <returns></returns>
        /// 
        internal static void ReCalculateAndApplyTheNewBrush(Shape shape, Brush newBrush, Boolean isLightingEnabled, Boolean is3D, TriangularChartSliceParms pyramidSliceParms)
        {
            if (is3D)
            {
                Path path = shape as Path; 
                Pyramid3DSlice.ApplyColor(ref path, (Pyramid3dLayer)Enum.Parse(typeof(Pyramid3dLayer), (shape.Tag as ElementData).VisualElementName, true)
                    , newBrush
                    , isLightingEnabled
                    , pyramidSliceParms.Index == 0);
            }
            else
            {   
                switch ((shape.Tag as ElementData).VisualElementName)
                {
                case "PyramidBase":
                case "FunnelTop": shape.Fill = newBrush; shape.Stroke = newBrush; break;
                case "TopBevel": shape.Fill = Graphics.GetBevelTopBrush(newBrush, 90); break;
                case "LeftBevel": shape.Fill = Graphics.GetBevelSideBrush(45, newBrush); break;
                case "RightBevel": shape.Fill = Graphics.GetBevelSideBrush(45, newBrush); break;
                case "BottomBevel": shape.Fill = Graphics.GetBevelSideBrush(180, newBrush); break;
                case "Lighting": shape.Fill = isLightingEnabled ? GetSideLightingBrush(pyramidSliceParms) : newBrush; break;
                case "FunnelTopLighting":
                    shape.Fill = is3D ? GetTopBrush(newBrush, pyramidSliceParms) : newBrush;
                    shape.Stroke = GetLightingBrushForStroke(newBrush, pyramidSliceParms.Index);
                    break;
                }
            }
        }

        /// <summary>
        /// Apply Bevel effect for a pyramid slice
        /// </summary>
        /// <param name="parentVisual">Parent canvas of Bevel layer</param>
        /// <param name="pyramidSlice">pyramidSlice</param>
        /// <param name="sideFillColor">Side fill color</param>
        /// <param name="points">Pyramid Points</param>
        private static void ApplyPyramidBevel(Canvas parentVisual, TriangularChartSliceParms pyramidSlice, Brush sideFillColor, Point[] points)
        {
            if (pyramidSlice.DataPoint.Parent.Bevel && pyramidSlice.Height > Chart.BEVEL_DEPTH)
            {
                // Generate Inner Points
                CalculateBevelInnerPoints(pyramidSlice, points, (pyramidSlice.Index == 0));
                
                Path topBevelPath = ExtendedGraphics.GetPathFromPoints(Graphics.GetBevelTopBrush(sideFillColor, 90), points[0], points[4], points[5], points[1]);
                Path leftBevelPath = ExtendedGraphics.GetPathFromPoints(Graphics.GetDarkerBrush(sideFillColor, 0.9), points[1], points[5], points[6], points[2]);
                Path rightBevelPath = ExtendedGraphics.GetPathFromPoints(Graphics.GetBevelSideBrush(45, sideFillColor), points[0], points[4], points[7], points[3]);
                Path bottomBevelPath = ExtendedGraphics.GetPathFromPoints(Graphics.GetBevelSideBrush(180, sideFillColor), points[7], points[6], points[2], points[3]);

                topBevelPath.IsHitTestVisible = false;
                rightBevelPath.IsHitTestVisible = false;
                leftBevelPath.IsHitTestVisible = false;
                bottomBevelPath.IsHitTestVisible = false;

                parentVisual.Children.Add(topBevelPath);
                parentVisual.Children.Add(rightBevelPath);
                parentVisual.Children.Add(leftBevelPath);
                parentVisual.Children.Add(bottomBevelPath);

                topBevelPath.Tag = new ElementData() { Element = pyramidSlice.DataPoint, VisualElementName = "TopBevel" };
                rightBevelPath.Tag = new ElementData() { Element = pyramidSlice.DataPoint, VisualElementName = "LeftBevel" };
                leftBevelPath.Tag = new ElementData() { Element = pyramidSlice.DataPoint, VisualElementName = "RightBevel" };
                bottomBevelPath.Tag = new ElementData() { Element = pyramidSlice.DataPoint, VisualElementName = "BottomBevel" };

                pyramidSlice.DataPoint.Faces.Parts.Add(topBevelPath);
                pyramidSlice.DataPoint.Faces.Parts.Add(rightBevelPath);
                pyramidSlice.DataPoint.Faces.Parts.Add(leftBevelPath);
                pyramidSlice.DataPoint.Faces.Parts.Add(bottomBevelPath);

                pyramidSlice.DataPoint.Faces.VisualComponents.Add(topBevelPath);
                pyramidSlice.DataPoint.Faces.VisualComponents.Add(rightBevelPath);
                pyramidSlice.DataPoint.Faces.VisualComponents.Add(leftBevelPath);
                pyramidSlice.DataPoint.Faces.VisualComponents.Add(bottomBevelPath);

                if ((pyramidSlice.DataPoint.Chart as Chart).AnimationEnabled)
                {
                    pyramidSlice.DataPoint.Parent.Storyboard = AnimationHelper.ApplyOpacityAnimation(topBevelPath, pyramidSlice.DataPoint.Parent, pyramidSlice.DataPoint.Parent.Storyboard, 0, pyramidSlice.DataPoint.InternalOpacity, 0, 1);
                    pyramidSlice.DataPoint.Parent.Storyboard = AnimationHelper.ApplyOpacityAnimation(rightBevelPath, pyramidSlice.DataPoint.Parent, pyramidSlice.DataPoint.Parent.Storyboard, 0, pyramidSlice.DataPoint.InternalOpacity, 0, 1);
                    pyramidSlice.DataPoint.Parent.Storyboard = AnimationHelper.ApplyOpacityAnimation(leftBevelPath, pyramidSlice.DataPoint.Parent, pyramidSlice.DataPoint.Parent.Storyboard, 0, pyramidSlice.DataPoint.InternalOpacity, 0, 1);
                    pyramidSlice.DataPoint.Parent.Storyboard = AnimationHelper.ApplyOpacityAnimation(bottomBevelPath, pyramidSlice.DataPoint.Parent, pyramidSlice.DataPoint.Parent.Storyboard, 0, pyramidSlice.DataPoint.InternalOpacity, 0, 1);
                }
            }
        }

        private enum Pyramid3dLayer
        {   
            BackLayerLeft,
            BackLayerRight,
            FrontLayerLeft,
            FrontLayerRight,
            TopLayer,
            BottomLayer,
            LightingLayerFront,
            LightingLayerLeft,
            LightingLayerRight,
        }
        
        /// <summary>
        /// Pyramid 2d Slice
        /// </summary>
        private class Pyramid3DLayer
        {   
            /// <summary>
            /// Left point
            /// </summary>
            public Point L;

            /// <summary>
            /// Right point
            /// </summary>
            public Point R;

            /// <summary>
            /// Center Top point
            /// </summary>
            public Point CT;

            /// <summary>
            /// Center Bottom point
            /// </summary>
            public Point CB;

            public PointCollection GetAllPoints()
            {   
                PointCollection retVal = new PointCollection();
                retVal.Add(L);
                retVal.Add(CT);
                retVal.Add(R);
                retVal.Add(CB);

                return retVal;
            }
        }

        /// <summary>
        /// Pyramid 2d Slice
        /// </summary>
        private class Pyramid2DSlice
        {   
            /// <summary>
            /// LeftTop point
            /// </summary>
            public Point LT;

            /// <summary>
            /// Right Top point
            /// </summary>
            public Point RT;

            /// <summary>
            /// Left Bottom point
            /// </summary>
            public Point LB;

            /// <summary>
            /// Right Bottom point
            /// </summary>
            public Point RB;

            /// <summary>
            /// Center Top Point
            /// </summary>
            public Point CT
            {
                get
                {
                    return new Point(LT.X + Math.Abs(RT.X - LT.X) / 2, LT.Y);
                }
            }

            /// <summary>
            /// Center Top Point
            /// </summary>
            public Point CB
            {
                get
                {
                    return new Point(LB.X + Math.Abs(RB.X - LB.X) / 2, LB.Y);
                }
            }
            
            public PointCollection GetAllPoints()
            {
                PointCollection retVal = new PointCollection();
                retVal.Add(LT);
                retVal.Add(RT);
                retVal.Add(RB);
                retVal.Add(LB);

                return retVal;
            }
        }

        private class Pyramid3DSlice
        {
            public Pyramid3DSlice(Pyramid2DSlice pyramid2DSlice, Double topOffset, Double bottomOffset, Brush color, Boolean isTopOfPyramid, Boolean lightingEnabled, Double opacity)
            {
                _points2d = pyramid2DSlice;
                _topOffset = topOffset;
                _bottomOffset = bottomOffset;
                _color = color;
                _isTopOfPyramid = isTopOfPyramid;
                _lightingEnabled = lightingEnabled;
                _opacity = opacity;
                Calculate3DLayerPoints();
            }

            private void Calculate3DLayerPoints()
            {
                _top3DLayer = new Pyramid3DLayer()
                {
                    L = CalculatePoint(_points2d.LT, _isTopOfPyramid ? 0 : - _topOffset),
                    R = CalculatePoint(_points2d.RT, _isTopOfPyramid ? 0 : -_topOffset),
                    CT = CalculatePoint(_points2d.CT, _isTopOfPyramid ? 0 : (-_topOffset - _bottomOffset)),
                    CB = CalculatePoint(_points2d.CT, _isTopOfPyramid ? 0 : _topOffset)
                };

                _bottom3DLayer = new Pyramid3DLayer()
                {   
                    L = CalculatePoint(_points2d.LB, -_bottomOffset),
                    R = CalculatePoint(_points2d.RB, -_bottomOffset),
                    CT = CalculatePoint(_points2d.CB, -2 * _bottomOffset ),
                    CB = CalculatePoint(_points2d.CB, _bottomOffset)
                };
            }

            public Path GetFace(Pyramid3dLayer pyramid3dFaceType, DataPoint dataPoint, Boolean attachTagInfo)
            {
                PointCollection points = Get3DLayerPoints(pyramid3dFaceType);
                Path path = GetPath(points);
                path.Opacity = _opacity;
                ApplyColor(ref path, pyramid3dFaceType, _color, _lightingEnabled, _isTopOfPyramid);
                SetZIndexOf3DLayers(ref path, pyramid3dFaceType);
                if(attachTagInfo)
                    path.Tag =  new ElementData(){Element = dataPoint, VisualElementName = pyramid3dFaceType.ToString()};
                
                //path.Opacity = 0.8;
                return path;
            }

            private void SetZIndexOf3DLayers(ref Path path, Pyramid3dLayer pyramid3dFaceType)
            {
                switch (pyramid3dFaceType)
                {   
                    case Pyramid3dLayer.BackLayerLeft:
                        path.SetValue(Canvas.ZIndexProperty, -1);
                        break;

                    case Pyramid3dLayer.BackLayerRight:
                        path.SetValue(Canvas.ZIndexProperty, -1);
                        break;

                    case Pyramid3dLayer.BottomLayer:
                        path.SetValue(Canvas.ZIndexProperty, -2);
                        break;

                    case Pyramid3dLayer.FrontLayerLeft:
                        path.SetValue(Canvas.ZIndexProperty, 1);
                        break;

                    case Pyramid3dLayer.FrontLayerRight:
                        path.SetValue(Canvas.ZIndexProperty, 1);
                        break;

                    case Pyramid3dLayer.TopLayer:
                        path.SetValue(Canvas.ZIndexProperty, 2);
                        break;

                    case Pyramid3dLayer.LightingLayerFront:
                        path.SetValue(Canvas.ZIndexProperty, 2);

                        break;

                    case Pyramid3dLayer.LightingLayerLeft:
                        path.SetValue(Canvas.ZIndexProperty, 2);
                        break;

                    case Pyramid3dLayer.LightingLayerRight:
                        path.SetValue(Canvas.ZIndexProperty, 2);
                        break;
                }
            }

            public static void ApplyColor(ref Path path, Pyramid3dLayer pyramid3dFaceType, Brush color, Boolean lightingEnabled, Boolean isTopOfPyramid)
            {   
                path.StrokeLineJoin = PenLineJoin.Round;
                Color lightingColor;
                Color whiteColor = Colors.White;
                SolidColorBrush solidColorBrush = color as SolidColorBrush;

                switch (pyramid3dFaceType)
                {   
                    case Pyramid3dLayer.LightingLayerFront:

                        if (solidColorBrush != null && lightingEnabled)
                        {   
                            lightingColor = Graphics.GetLighterColor(solidColorBrush.Color, 1);
                            if(isTopOfPyramid)
                                path.StrokeThickness = 2;
                            else
                                path.StrokeThickness = 3.5;

                            path.Stroke = Graphics.GetBevelSideBrush(180, solidColorBrush);

                            path.StrokeStartLineCap = PenLineCap.Flat;
                            path.StrokeEndLineCap = PenLineCap.Flat;
                        }
                        
                        break;

                    case Pyramid3dLayer.LightingLayerLeft:
                        if (solidColorBrush != null && lightingEnabled)
                        {
                            path.StrokeThickness = 2;

                            lightingColor = Graphics.GetLighterColor(solidColorBrush.Color, .88);

                            path.Stroke = new SolidColorBrush(lightingColor);
                                                       

                            path.StrokeStartLineCap = PenLineCap.Round;
                            path.StrokeEndLineCap = PenLineCap.Round;
                            //path.Opacity = 0.5;
                        }
                        break;

                    case Pyramid3dLayer.LightingLayerRight:
                        if (solidColorBrush != null && lightingEnabled)
                        {
                            path.StrokeThickness = 2;

                            lightingColor = solidColorBrush.Color;

                            lightingColor.A = (byte) 127;
                            path.Stroke = Graphics.CreateLinearGradientBrush(0, new Point(0, 1), new Point(1, .2),
                               new List<Color>() { lightingColor, Graphics.GetDarkerColor(solidColorBrush.Color, 0.6) },
                               new List<double>() { 0, .6 });

                            path.StrokeStartLineCap = PenLineCap.Round;
                            path.StrokeEndLineCap = PenLineCap.Round;

                        }
                        break;

                    case Pyramid3dLayer.BackLayerLeft:
                        if (lightingEnabled)
                            path.Fill = Graphics.GetDarkerBrush(color, .4);
                        else
                            path.Fill = color;

                        break;

                    case Pyramid3dLayer.BackLayerRight:
                        if (lightingEnabled)
                            path.Fill = Graphics.GetDarkerBrush(color, .4);
                        else
                            path.Fill = color;
                        break;

                    case Pyramid3dLayer.TopLayer:

                        if(solidColorBrush == null || !lightingEnabled)
                            path.Fill = color;
                        else
                        {
                            //EndPoint="" StartPoint=""

                            path.Fill = Graphics.CreateLinearGradientBrush(-35, new Point(0,0), new Point(1,1),
                              new List<Color>() { Graphics.GetLighterColor(solidColorBrush.Color, 0.99), Graphics.GetDarkerColor(solidColorBrush.Color, 0.6) },
                              new List<double>() { 0, 0.7 });
                        }
                        
                        break;

                    case Pyramid3dLayer.BottomLayer:
                        if (lightingEnabled)
                            path.Fill = Graphics.GetDarkerBrush(color, 0.4);
                        else
                            path.Fill = color;
                        break;

                    case Pyramid3dLayer.FrontLayerLeft:
                        if (lightingEnabled)
                            path.Fill = Graphics.CreateLinearGradientBrush(-35, new Point(0, 1), new Point(1, 0),
                             new List<Color>() { Graphics.GetLighterColor(solidColorBrush.Color, 0.8678), Graphics.GetLighterColor(solidColorBrush.Color, 0.99), solidColorBrush.Color },
                             new List<double>() { 0, 0.7, 1 });
                        else
                            path.Fill = color;

                        break;

                    case Pyramid3dLayer.FrontLayerRight:
                        if (lightingEnabled)
                            path.Fill = Graphics.GetDarkerBrush(color, 0.6);
                        else
                            path.Fill = color;
                        break;
                }

                path.StrokeLineJoin = PenLineJoin.Round;

                //
            }

            private Path GetPath(PointCollection points)
            {
                if (points.Count < 1)
                    return null;

                Path path = new Path();
                
                PathGeometry pathGeo = new PathGeometry();
                PathFigure pathFig = new PathFigure() { StartPoint = points[0] , IsClosed=true};

                PolyLineSegment polyLineSeg = new PolyLineSegment();
                
                for (int i = 1; i < points.Count; i++)
                    polyLineSeg.Points.Add(points[i]);

                pathFig.Segments.Add(polyLineSeg);

                pathGeo.Figures.Add(pathFig);

                path.Data = pathGeo;

                return path;
            }

            private PointCollection Get3DLayerPoints(Pyramid3dLayer pyramid3dFaceType)
            {   
                PointCollection retVal = new PointCollection();

                switch (pyramid3dFaceType)
                {
                    case Pyramid3dLayer.LightingLayerFront:
                        if(_isTopOfPyramid)
                            retVal.Add(CalculatePoint(_top3DLayer.CB, 2));
                        else
                            retVal.Add(CalculatePoint(_top3DLayer.CB, 1));
                        retVal.Add(CalculatePoint(_bottom3DLayer.CB, -3));
                        break;

                    case Pyramid3dLayer.LightingLayerLeft:
                        retVal.Add(CalculatePoint(_top3DLayer.CB, 0));
                        retVal.Add(CalculatePoint(_top3DLayer.L, 2,0));
                        break;

                    case Pyramid3dLayer.LightingLayerRight:
                        retVal.Add(CalculatePoint(_top3DLayer.CB, 0));
                        retVal.Add(CalculatePoint(_top3DLayer.R, -2,1));
                        break;

                    case Pyramid3dLayer.BackLayerLeft:
                        retVal.Add(_top3DLayer.L);
                        retVal.Add(_top3DLayer.CT);
                        retVal.Add(_bottom3DLayer.CT);
                        retVal.Add(_bottom3DLayer.L);
                        break;

                    case Pyramid3dLayer.BackLayerRight:
                        retVal.Add(_top3DLayer.CT);
                        retVal.Add(_top3DLayer.R);
                        retVal.Add(_bottom3DLayer.R);
                        retVal.Add(_bottom3DLayer.CT);
                        break;

                    case Pyramid3dLayer.TopLayer:
                        retVal = _top3DLayer.GetAllPoints();
                        break;

                    case Pyramid3dLayer.BottomLayer:
                        retVal = _bottom3DLayer.GetAllPoints();
                        break;

                    case Pyramid3dLayer.FrontLayerLeft:
                        retVal.Add(_top3DLayer.L);
                        retVal.Add(_top3DLayer.CB);
                        retVal.Add(_bottom3DLayer.CB);
                        retVal.Add(_bottom3DLayer.L);
                        break;

                    case Pyramid3dLayer.FrontLayerRight:
                        retVal.Add(_top3DLayer.CB);
                        retVal.Add(_top3DLayer.R);
                        retVal.Add(_bottom3DLayer.R);
                        retVal.Add(_bottom3DLayer.CB);
                        break;
                }

                return retVal;
            }

            /// <summary>
            /// Calculates new point with offset
            /// </summary>
            private Point CalculatePoint(Point point, Double yOffSet)
            {
                return new Point(point.X, point.Y + yOffSet);
            }

            private Point CalculatePoint(Point point, Double xOffSet, Double yOffSet)
            {
                return new Point(point.X + xOffSet, point.Y + yOffSet);
            }

            public Pyramid3DLayer Top3DLayer
            {
                get { return _top3DLayer;}
            }

            public Pyramid3DLayer Bottom3DLayer
            {
                get { return _bottom3DLayer; }
            }

            Double _topOffset;
            Double _bottomOffset;
            Pyramid2DSlice _points2d;
            Pyramid3DLayer _top3DLayer;
            Pyramid3DLayer _bottom3DLayer;
            Brush _color;
            Boolean _isTopOfPyramid;
            Boolean _lightingEnabled;
            Double _opacity;
        }

        /// <summary>
        /// Create a slice of a pyramid
        /// </summary>
        /// <param name="isLightingGradientLayer">Whether CreatePyramidSlice() function should create a layer for lighting</param>
        /// <param name="topRadius">Top Radius of the pyramid</param>
        /// <param name="is3D">Whether the chart is a 3D Chart</param>
        /// <param name="pyramidSlice">PyramidSlice canvas reference</param>
        /// <param name="yScaleTop">Top YScale for 3D view</param>
        /// <param name="yScaleBottom">Bottom YScale for 3D view</param>
        /// <param name="sideFillColor">Side surface fill color</param>
        /// <param name="topFillColor">Top surface fill color</param>
        /// <param name="topSurfaceStroke">Top surface stroke color</param>
        /// <param name="animationEnabled">Whether animation is enabled</param>
        /// <returns>Return pyramid slice canvas</returns>
        private static Canvas CreatePyramidSlice(Boolean isLightingGradientLayer, Double topRadius, Boolean is3D, TriangularChartSliceParms pyramidSlice, Double yScaleTop, Double yScaleBottom, Brush sideFillColor, Brush topFillColor, Brush topSurfaceStroke, Boolean animationEnabled)
        {
            Canvas sliceCanvas = new Canvas() { Tag = new ElementData() { Element = pyramidSlice.DataPoint } };
            Canvas visual = new Canvas() { Width = topRadius * 2, Height = pyramidSlice.Height, Tag = new ElementData() { Element = pyramidSlice.DataPoint } };  // Canvas holds a slice of a pyramid chart
            Faces faces = null;
            //visual.Background = Graphics.GetRandomColor();

            Pyramid2DSlice pyramid2DSlice = new Pyramid2DSlice()
            {
                LT = new Point(topRadius - Math.Abs(pyramidSlice.TopRadius), 0),
                RT = new Point(topRadius + Math.Abs(pyramidSlice.TopRadius), 0),
                LB = new Point(topRadius - Math.Abs(pyramidSlice.BottomRadius), pyramidSlice.Height),
                RB = new Point(topRadius + Math.Abs(pyramidSlice.BottomRadius), pyramidSlice.Height)
            };

            Pyramid3DSlice pyramid3DSlice = null;

            if (is3D)
            {
                Double opacity = pyramidSlice.DataPoint.Opacity * pyramidSlice.DataPoint.Parent.Opacity;

                pyramid3DSlice = new Pyramid3DSlice(pyramid2DSlice, Math.Abs(yScaleTop / 2), Math.Abs(yScaleBottom / 2)
                    , isLightingGradientLayer ? new SolidColorBrush(Colors.Transparent) : pyramidSlice.DataPoint.Color
                    , (pyramidSlice.Index == 0)
                    , (Boolean)pyramidSlice.DataPoint.LightingEnabled, opacity);

                List<Path> paths = new List<Path>();

                Path lightingLayerLeft = null, lightingLayerFront = null, lightingLayerRight = null;
                Path backLayerLeft = pyramid3DSlice.GetFace(Pyramid3dLayer.BackLayerLeft, pyramidSlice.DataPoint, !isLightingGradientLayer);
                Path backLayerRight = pyramid3DSlice.GetFace(Pyramid3dLayer.BackLayerRight, pyramidSlice.DataPoint, !isLightingGradientLayer);
                Path bottomLayer = pyramid3DSlice.GetFace(Pyramid3dLayer.BottomLayer, pyramidSlice.DataPoint, !isLightingGradientLayer);
                Path frontLayerLeft = pyramid3DSlice.GetFace(Pyramid3dLayer.FrontLayerLeft, pyramidSlice.DataPoint, !isLightingGradientLayer);
                Path frontLayerRight = pyramid3DSlice.GetFace(Pyramid3dLayer.FrontLayerRight, pyramidSlice.DataPoint, !isLightingGradientLayer);
                Path topLayer = pyramid3DSlice.GetFace(Pyramid3dLayer.TopLayer, pyramidSlice.DataPoint, !isLightingGradientLayer);
                
                paths.Add(backLayerLeft);
                paths.Add(backLayerRight);
                paths.Add(bottomLayer);

                paths.Add(frontLayerLeft);
                paths.Add(frontLayerRight);
                paths.Add(topLayer);

                if ((Boolean)pyramidSlice.DataPoint.LightingEnabled && !isLightingGradientLayer)
                {   
                    if (pyramidSlice.Index != 0)
                        paths.Add(lightingLayerLeft = pyramid3DSlice.GetFace(Pyramid3dLayer.LightingLayerLeft, pyramidSlice.DataPoint, !isLightingGradientLayer));

                    paths.Add(lightingLayerFront = pyramid3DSlice.GetFace(Pyramid3dLayer.LightingLayerFront, pyramidSlice.DataPoint, !isLightingGradientLayer));

                    if (pyramidSlice.Index != 0)
                        paths.Add(lightingLayerRight = pyramid3DSlice.GetFace(Pyramid3dLayer.LightingLayerRight, pyramidSlice.DataPoint, !isLightingGradientLayer));
                }

                foreach (Path path in paths)
                    visual.Children.Add(path);

                if (!isLightingGradientLayer)
                {   
                    // Update faces for the DataPoint
                    faces = new Faces();

                    foreach (Path path in paths)
                        faces.VisualComponents.Add(path);

                    foreach (Path path in paths)
                        faces.Parts.Add(path);

                    // If lighting is disabled then the visual components are the border elements
                    if (!(Boolean)pyramidSlice.DataPoint.LightingEnabled)
                    {
                        foreach (Path path in paths)
                        {   
                            if (path != bottomLayer && path != backLayerLeft && path != backLayerRight)
                                faces.BorderElements.Add(path);
                        }

                        if(opacity < 1)
                            faces.BorderElements.Add(bottomLayer);
                    }

                    pyramidSlice.DataPoint.Faces = faces;
                }
                else
                {
                    faces = pyramidSlice.DataPoint.Faces;

                    if (faces != null)
                    {
                        foreach (Path path in paths)
                        {
                            if (path != bottomLayer && path != backLayerLeft && path != backLayerRight)
                                faces.BorderElements.Add(path);
                        }

                        if (opacity < 1)
                            faces.BorderElements.Add(bottomLayer);
                    }
                }


                if (faces.BorderElements != null)
                {
                    foreach (Path path in faces.BorderElements)
                    {
                        path.StrokeThickness = pyramidSlice.DataPoint.BorderThickness.Left;
                        path.Stroke = pyramidSlice.DataPoint.BorderColor;
                        path.StrokeDashArray = ExtendedGraphics.GetDashArray((BorderStyles)pyramidSlice.DataPoint.BorderStyle);
                    }
                }

                // Apply animation for the 3D funnel slice
                if (animationEnabled)
                {
                    foreach (Path path in paths)
                    {
                        //Double animationBeginTime = (pyramidSlice.DataPoint.Parent.InternalDataPoints.Count - pyramidSlice.Index) / pyramidSlice.DataPoint.Parent.InternalDataPoints.Count;
                        pyramidSlice.DataPoint.Parent.Storyboard = AnimationHelper.ApplyOpacityAnimation(path, pyramidSlice.DataPoint.Parent, pyramidSlice.DataPoint.Parent.Storyboard, 0, pyramidSlice.DataPoint.InternalOpacity, 0, 1);
                    }
                }
            }
            else
            {   
                // PathGeometry for for a pyramid slice path
                PathGeometry pathGeometry = new PathGeometry();

                // pathFigure for for a pyramid slice path
                PathFigure pathFigure = new PathFigure() { StartPoint = pyramid2DSlice.LT, IsClosed= true };  // PathFigure of a pyramid slice

                // Path for for a pyramid slice
                Path path4Slice = new Path() { Fill = sideFillColor };

                path4Slice.Tag = new ElementData() { Element = pyramidSlice.DataPoint };

                // Set path data
                path4Slice.Data = pathGeometry;

                // Set properties for path
                path4Slice.StrokeThickness = 0;
                path4Slice.Stroke = new SolidColorBrush(Colors.Black);

                // Add path elements to its parent canvas
                pathGeometry.Figures.Add(pathFigure);
                visual.Children.Add(path4Slice);
                
                PolyLineSegment polySeg = new PolyLineSegment();
                polySeg.Points = pyramid2DSlice.GetAllPoints();
                pathFigure.Segments.Add(polySeg);

                // Points of a 2D pyramid slice
                Point[] pyramidCornerPoints = new Point[8];
                pyramidCornerPoints[0] = new Point(topRadius - pyramidSlice.TopRadius, 0);
                pyramidCornerPoints[1] = new Point(topRadius + pyramidSlice.TopRadius, 0);
                pyramidCornerPoints[2] = new Point(topRadius + pyramidSlice.BottomRadius, pyramidSlice.Height);
                pyramidCornerPoints[3] = new Point(topRadius - pyramidSlice.BottomRadius, pyramidSlice.Height);

                // Apply animation for the 2D pyramid slice
                if (animationEnabled)
                    pyramidSlice.DataPoint.Parent.Storyboard = AnimationHelper.ApplyOpacityAnimation(path4Slice, pyramidSlice.DataPoint.Parent, pyramidSlice.DataPoint.Parent.Storyboard, 0, pyramidSlice.DataPoint.InternalOpacity, 0, 1);

                if (!isLightingGradientLayer)
                {
                    // Update faces for the DataPoint
                    faces = new Faces();
                    faces.VisualComponents.Add(path4Slice);

                    (path4Slice.Tag as ElementData).VisualElementName = "PyramidBase";
                    faces.Parts.Add(path4Slice);

                    faces.BorderElements.Add(path4Slice);

                    path4Slice.StrokeThickness = pyramidSlice.DataPoint.BorderThickness.Left;
                    path4Slice.Stroke = pyramidSlice.DataPoint.BorderColor;
                    path4Slice.StrokeDashArray = ExtendedGraphics.GetDashArray((BorderStyles)pyramidSlice.DataPoint.BorderStyle);

                    pyramidSlice.DataPoint.Faces = faces;

                    // Apply bevel effect for the 2D pyramid Slice
                    if (pyramidSlice.DataPoint.Parent.Bevel)
                    {
                        ApplyPyramidBevel(visual, pyramidSlice, sideFillColor, pyramidCornerPoints);
                    }
                }
                else
                {
                    path4Slice.IsHitTestVisible = false;
                    pyramidSlice.DataPoint.Faces.VisualComponents.Add(path4Slice);
                    (path4Slice.Tag as ElementData).VisualElementName = "Lighting";
                }
            }

            if (isLightingGradientLayer)
            {
                visual.IsHitTestVisible = false;
            }
            else
            {
                // Drawing LabelLine
                Canvas labelLineCanvas = CreateLabelLine(pyramidSlice, pyramid3DSlice, topRadius, animationEnabled);

                if (labelLineCanvas != null)
                {
                    sliceCanvas.Children.Add(labelLineCanvas);
                    faces.VisualComponents.Add(labelLineCanvas);
                }

                // Add label visual to the visual
                if ((Boolean)pyramidSlice.DataPoint.LabelEnabled)
                {
                    Canvas labelCanvas = new Canvas();
                    labelCanvas.SetValue(Canvas.ZIndexProperty, (Int32)10);

                    faces.VisualComponents.Add(pyramidSlice.DataPoint.LabelVisual);

                    // Label placement

                    if (pyramidSlice.DataPoint.LabelStyle == LabelStyles.OutSide)
                    {
                        pyramidSlice.DataPoint.LabelVisual.SetValue(Canvas.TopProperty, pyramidSlice.LabelLineEndPoint.Y - pyramidSlice.DataPoint.LabelVisual.Height / 2);
                        pyramidSlice.DataPoint.LabelVisual.SetValue(Canvas.LeftProperty, pyramidSlice.LabelLineEndPoint.X + 2);
                    }
                    else
                    {
                        if(is3D)
                        {
                            Point centerPoint = Graphics.MidPointOfALine(pyramid3DSlice.Top3DLayer.CB, pyramid3DSlice.Bottom3DLayer.CB);
                           pyramidSlice.DataPoint.LabelVisual.SetValue(Canvas.TopProperty, (centerPoint.Y - pyramidSlice.DataPoint.LabelVisual.Height / 2));
                        }
                        else
                            pyramidSlice.DataPoint.LabelVisual.SetValue(Canvas.TopProperty, pyramidSlice.LabelLineEndPoint.Y - pyramidSlice.DataPoint.LabelVisual.Height / 2);
                        pyramidSlice.DataPoint.LabelVisual.SetValue(Canvas.LeftProperty, topRadius - pyramidSlice.DataPoint.LabelVisual.Width / 2);
                    }

                    if (animationEnabled)
                        pyramidSlice.DataPoint.Parent.Storyboard = AnimationHelper.ApplyOpacityAnimation(pyramidSlice.DataPoint.LabelVisual, pyramidSlice.DataPoint.Parent, pyramidSlice.DataPoint.Parent.Storyboard, 1.2, 0.5, 0, 1);

                    labelCanvas.Children.Add(pyramidSlice.DataPoint.LabelVisual);
                    sliceCanvas.Children.Add(labelCanvas);
                }
            }

            // if (!isLightingGradientLayer)
            //    faces.Visual = visual;

            sliceCanvas.Children.Add(visual);

            // sliceCanvas.Background = new SolidColorBrush(Color.FromArgb((byte)rand.Next(0,200),(byte)rand.Next(0,200),(byte)rand.Next(0,200),(byte)rand.Next(0,200)));

            return sliceCanvas;
        }

        private static Random rand = new Random();

        /// <summary>
        /// Creates a LabelLine for Pyramid Chart
        /// </summary>
        /// <param name="pyramidSlice">TriangularChartSliceParms</param>
        /// <param name="topRadius">Top most radius of the pyramid</param>
        /// <param name="animationEnabled">Whether animation is enabled</param>
        /// <returns>Canvas for labelline </returns>
        private static Canvas CreateLabelLine(TriangularChartSliceParms pyramidSlice, Pyramid3DSlice pyramid3DSlice, Double topRadius, Boolean animationEnabled)
        {
            Canvas labelLineCanvas = null;

            Point topRightPoint; 
            Point bottomRightPoint;

            if (pyramid3DSlice == null) // If 2d
            {
                topRightPoint = new Point(topRadius + Math.Abs(pyramidSlice.TopRadius), 0);
                bottomRightPoint = new Point(topRadius + Math.Abs(pyramidSlice.BottomRadius), pyramidSlice.Height);
            }
            else
            {
                topRightPoint = pyramid3DSlice.Top3DLayer.R;
                bottomRightPoint = pyramid3DSlice.Bottom3DLayer.R;
            }

            pyramidSlice.RightMidPoint = Graphics.MidPointOfALine(topRightPoint, bottomRightPoint);

            pyramidSlice.LabelLineEndPoint = new Point(2 * topRadius, pyramidSlice.RightMidPoint.Y);

            if ((Boolean)pyramidSlice.DataPoint.LabelLineEnabled && pyramidSlice.DataPoint.LabelStyle == LabelStyles.OutSide)
            {
                labelLineCanvas = new Canvas();

                labelLineCanvas.Width = topRadius * 2;
                labelLineCanvas.Height = pyramidSlice.Height;

                pyramidSlice.DataPoint.LabelLine = null;

                Path line = new Path()
                {
                    Stroke = pyramidSlice.DataPoint.LabelLineColor,
                    Fill = pyramidSlice.DataPoint.LabelLineColor,
                    StrokeDashArray = ExtendedGraphics.GetDashArray((LineStyles)pyramidSlice.DataPoint.LabelLineStyle),
                    StrokeThickness = (Double)pyramidSlice.DataPoint.LabelLineThickness,
                };

                PathGeometry linePathGeometry = new PathGeometry();

                // Set first point of the line
                PathFigure linePathFigure = new PathFigure()
                {
                    StartPoint = pyramidSlice.RightMidPoint
                };

                // Set second point of line
                linePathFigure.Segments.Add(new LineSegment() { Point = pyramidSlice.LabelLineEndPoint });

                linePathGeometry.Figures.Add(linePathFigure);

                line.Data = linePathGeometry;

                pyramidSlice.DataPoint.LabelLine = line;

                labelLineCanvas.Children.Add(line);

                if (animationEnabled)
                    pyramidSlice.DataPoint.Parent.Storyboard = ApplyLabeLineAnimation(labelLineCanvas, pyramidSlice.DataPoint.Parent, pyramidSlice.DataPoint.Parent.Storyboard);
            }

            return labelLineCanvas;
        }

        /// <summary>
        /// Apply animation for line chart
        /// </summary>
        /// <param name="canvas">Line chart canvas</param>
        /// <param name="DataSeries">DataSeries</param>
        /// <param name="storyboard">Storyboard</param>
        /// <param name="isLineCanvas">Whether canvas is line canvas</param>
        /// <returns>Storyboard</returns>
        private static Storyboard ApplyLabeLineAnimation(Panel canvas, DataSeries dataSeries, Storyboard storyboard)
        {
            LinearGradientBrush opacityMaskBrush = new LinearGradientBrush() { StartPoint = new Point(0, 0.5), EndPoint = new Point(1, 0.5) };

            // Create gradients for opacity mask animation
            GradientStop GradStop1 = new GradientStop() { Color = Colors.White, Offset = 0 };
            GradientStop GradStop2 = new GradientStop() { Color = Colors.White, Offset = 0 };
            GradientStop GradStop3 = new GradientStop() { Color = Colors.Transparent, Offset = 0.01 };
            GradientStop GradStop4 = new GradientStop() { Color = Colors.Transparent, Offset = 1 };

            // Add gradients to gradient stop list
            opacityMaskBrush.GradientStops.Add(GradStop1);
            opacityMaskBrush.GradientStops.Add(GradStop2);
            opacityMaskBrush.GradientStops.Add(GradStop3);
            opacityMaskBrush.GradientStops.Add(GradStop4);

            canvas.OpacityMask = opacityMaskBrush;

            double beginTime = 1;

            DoubleCollection values = Graphics.GenerateDoubleCollection(0, 1);
            DoubleCollection timeFrames = Graphics.GenerateDoubleCollection(0, 0.5);
            List<KeySpline> splines = AnimationHelper.GenerateKeySplineList(new Point(0, 0), new Point(1, 1), new Point(0, 0), new Point(1, 1));

            storyboard.Children.Add(AnimationHelper.CreateDoubleAnimation(dataSeries, GradStop2, "(GradientStop.Offset)", beginTime, timeFrames, values, splines));

            values = Graphics.GenerateDoubleCollection(0.01, 1);
            timeFrames = Graphics.GenerateDoubleCollection(0, 0.5);
            splines = AnimationHelper.GenerateKeySplineList(new Point(0, 0), new Point(1, 1), new Point(0, 0), new Point(1, 1));

            storyboard.Children.Add(AnimationHelper.CreateDoubleAnimation(dataSeries, GradStop3, "(GradientStop.Offset)", beginTime, timeFrames, values, splines));

            return storyboard;
        }

        /// <summary>
        /// Calculate Bevel innter points
        /// </summary>
        /// <param name="pyramidSlice">pyramidSlice</param>
        /// <param name="points">Array of points</param>
        private static void CalculateBevelInnerPoints(TriangularChartSliceParms pyramidSlice, Point[] points, Boolean isTopOfPyramid)
        {
            Double a, b, bevelHeight = 3;

            a = bevelHeight * Math.Sin(pyramidSlice.TopAngle / 2);
            b = bevelHeight * Math.Cos(pyramidSlice.TopAngle / 2);

            if (isTopOfPyramid)
            {
                points[4] = new Point(points[0].X, points[0].Y + bevelHeight);
                points[5] = new Point(points[1].X, points[1].Y + bevelHeight);
            }
            else
            {
                points[4] = new Point(points[0].X - a, bevelHeight);
                points[5] = new Point(points[1].X + a, bevelHeight);
            }

             //b = h * Math.Cos(Math.PI /2 - pyramidSlice.BottomAngle / 2);
            b = bevelHeight * Math.Tan(pyramidSlice.BottomAngle / 2);

            bevelHeight = 2;

            points[6] = new Point(points[2].X + b, points[2].Y - bevelHeight);
            points[7] = new Point(points[3].X - b, points[3].Y - bevelHeight);
        }

        /// <summary>
        /// Get side lighting brush for the pyramid slice
        /// </summary>
        /// <returns></returns>
        private static Brush GetSideLightingBrush(TriangularChartSliceParms pyramidSlice)
        {
            //LinearGradientBrush gb = new LinearGradientBrush() { EndPoint = new Point(1.016, 0.558),  };
            List<Double> stops = new List<double>();
            List<Color> colors = new List<Color>();
            
            colors.Add(Color.FromArgb(0, 0, 0, 0)); stops.Add(0.491);
            colors.Add(Color.FromArgb(135, 8, 8, 8)); stops.Add(0.938);
            colors.Add(Color.FromArgb(71, 71, 71, 71)); stops.Add(0);
            //Double angle = pyramidSlice.BottomAngle * 180 / Math.PI;
            Brush gb = Graphics.CreateLinearGradientBrush(0, new Point(-0.1, 0.5), new Point(1, 0.5), colors, stops);

            return gb;
        }

        /// <summary>
        /// Get top lighting brush for pyramid slice
        /// </summary>
        /// <param name="fillBrush">Current brush</param>
        /// <returns>Return new Brush</returns>
        private static Brush GetTopBrush(Brush fillBrush, TriangularChartSliceParms pyramidSlice)
        {
            if ((fillBrush as SolidColorBrush) != null)
            {
                SolidColorBrush solidBrush = fillBrush as SolidColorBrush;
                
                Double r = ((double)solidBrush.Color.R / (double)255) * 0.9999;
                Double g = ((double)solidBrush.Color.G / (double)255) * 0.9999;
                Double b = ((double)solidBrush.Color.B / (double)255) * 0.9999;

                LinearGradientBrush gb = null;

                if (pyramidSlice.FillType == FillType.Solid)
                {
                    gb = new LinearGradientBrush() { StartPoint = new Point(0, 0), EndPoint = new Point(1, 1) };
                    gb.GradientStops.Add(new GradientStop() { Color = Graphics.GetLighterColor(solidBrush.Color, 1 - r, 1 - g, 1 - b), Offset = 0 });
                    gb.GradientStops.Add(new GradientStop() { Color = solidBrush.Color, Offset = 0.9 });
                    gb.GradientStops.Add(new GradientStop() { Color = Graphics.GetDarkerColor(solidBrush.Color, 1), Offset = 0.99 });
                    gb.Opacity = 1;

                }
                else if (pyramidSlice.FillType == FillType.Hollow)
                {   
                    gb = new LinearGradientBrush()
                    {
                        StartPoint = new Point(0.233, 0.297),
                        EndPoint = new Point(0.757, 0.495)
                    };

                    gb.GradientStops.Add(new GradientStop() { Offset = 0, Color = Graphics.GetDarkerColor(solidBrush.Color, 0.5) });
                    gb.GradientStops.Add(new GradientStop() { Offset = 0.70, Color = solidBrush.Color });
                    gb.GradientStops.Add(new GradientStop() { Offset = 1, Color = Graphics.GetDarkerColor(solidBrush.Color, 0.8)});
                }

                return gb;
            }
            else
                return fillBrush;
        }

        private static Double _singleGap = 0;// Single gap height

        private static Double _totalGap = 0;// Total height used for introducing gap among pyramid slice
        
        /// <summary>
        /// Size of the parent title of the StreamLine pyramid Chart
        /// </summary>
        private static Size _streamLineParentTitleSize;
    }
}
