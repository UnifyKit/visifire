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
    internal static class FunnelChart
    {
        /// <summary>
        /// Returns the visual object for funnel chart 
        /// </summary>
        /// <param name="width">PlotArea width</param>
        /// <param name="height">PlotArea height</param>
        /// <param name="plotDetails">PlotDetails</param>
        /// <param name="seriesList">List of line series</param>
        /// <param name="chart">Chart</param>
        /// <param name="animationEnabled">Whether animation is enabled</param>
        /// <param name="isStreamLine">Whether funnel chart is a Streamline funnel chart</param>
        /// <returns></returns>
        public static Grid GetVisualObjectForFunnelChart(Double width, Double height, PlotDetails plotDetails, List<DataSeries> seriesList, Chart chart, bool animationEnabled, Boolean isStreamLine)
        {
            if (seriesList.Count > 0)
            {
                DataSeries funnelSeries;            // DataSeries used for drawing funnel chart
                List<DataPoint> funnelDataPoints;   // DataPoints considered for drawing funnel chart

                // Select DataSeries for render
                List<DataSeries> selectedDataSeriesList = (from ds in seriesList where (Boolean)ds.Enabled == true select ds).ToList();

                if (selectedDataSeriesList.Count > 0)
                    funnelSeries = selectedDataSeriesList.First();
                else
                    return null;

                List<DataPoint> tempDataPoints = (from dp in funnelSeries.DataPoints where dp.Enabled == true && dp.YValue >= 0 select dp).ToList();

                if ((from dp in tempDataPoints where dp.YValue == 0 select dp).Count() == tempDataPoints.Count)
                    return null;

                // If number of DataPoints is equals to 0 then dont do any operation
                if (tempDataPoints.Count == 0 || (tempDataPoints.Count == 1 && tempDataPoints[0].YValue == 0))
                    return null;

                if (isStreamLine)
                {   
                    if (tempDataPoints.Count <= 1)
                        throw new Exception("Invalid DataSet. StreamLineFunnel chart must have more than one DataPoint in a DataSeries with YValue > 0.");

                    funnelDataPoints = (from dp in tempDataPoints orderby dp.YValue descending select dp).ToList();
                }
                else
                    funnelDataPoints = tempDataPoints.ToList();

                // Create funnel chart canvas
                Grid _funnelChartGrid = new Grid() { Height = height, Width = width };

                #region Create layout for Funnel chart and labels

                // Create canvas for label
                Canvas labelCanvas = new Canvas() { Height = height };

                // Create canvas for funnel
                Canvas funnelCanvas = new Canvas() { Height = height, HorizontalAlignment = HorizontalAlignment.Left };

                _funnelChartGrid.Children.Add(funnelCanvas);
                _funnelChartGrid.Children.Add(labelCanvas);

                #endregion

                if ((funnelSeries.Chart as Chart).AnimationEnabled)
                    funnelSeries.Storyboard = new Storyboard();

                // Creating labels for 
                CreateLabelsAndSetFunnelCanvasSize(isStreamLine, _funnelChartGrid, labelCanvas, funnelCanvas, funnelDataPoints);

                Double minPointHeight = funnelSeries.MinPointHeight;
                Double yScale = 40;
                Boolean isSameSlantAngle = true;
                Double bottomRadius = 5;
                Double gapRatio = (chart.View3D) ? 0.04 : 0.02;

                funnelCanvas = CreateFunnelChart(_funnelChartGrid, funnelSeries, funnelDataPoints, isStreamLine, funnelCanvas, minPointHeight, chart.View3D, yScale, gapRatio, isSameSlantAngle, bottomRadius, animationEnabled);

                // here
                // funnelChartCanvas.Background = new SolidColorBrush(Colors.Red);

                RectangleGeometry clipRectangle = new RectangleGeometry();
                clipRectangle.Rect = new Rect(0, 0, width, height);
                _funnelChartGrid.Clip = clipRectangle;

                return _funnelChartGrid;
            }

            return null;
        }

        /// <summary>
        /// Create labels and set width for the label canvas
        /// </summary>
        /// <param name="funnelChartCanvas">Main Funnel chart canvas</param>
        /// <param name="labelCanvas">Label canvas for funnel chart placed in side funnelChartCanvas</param>
        /// <param name="funnelSeries">DataSeries reference</param>
        private static void CreateLabelsAndSetFunnelCanvasSize(Boolean isStreamLine, Grid funnelChartCanvas, Canvas labelCanvas, Canvas funnelCanvas, List<DataPoint> funnelDataPoints)
        {
            Int32 index = 0;
            Double totalLabelsHeight = 0;
            _streamLineParentTitleSize = new Size(0, 0);

            labelCanvas.Width = 0;

            for (; index < funnelDataPoints.Count; index++)
            {
                // Create label for a funnel slice
                funnelDataPoints[index].LabelVisual = CreateLabelForDataPoint(funnelDataPoints[index], isStreamLine, index);

                // Calculate label size
                Size labelSize = Graphics.CalculateVisualSize(funnelDataPoints[index].LabelVisual);
                labelSize.Width += 2.5;

                if (isStreamLine && index == 0)
                    labelSize.Height += TITLE_FUNNEL_GAP; 
                    
                if ((Boolean)funnelDataPoints[index].LabelEnabled && funnelDataPoints[index].LabelStyle == LabelStyles.OutSide)
                    totalLabelsHeight += labelSize.Height;

                if (isStreamLine && index == 0)
                    _streamLineParentTitleSize = labelSize;
                else if (labelSize.Width > labelCanvas.Width && (Boolean)funnelDataPoints[index].LabelEnabled && funnelDataPoints[index].LabelStyle == LabelStyles.OutSide) // && !(isStreamLine && index == 0))
                    labelCanvas.Width = labelSize.Width;

                funnelDataPoints[index].LabelVisual.Height = labelSize.Height;
                funnelDataPoints[index].LabelVisual.Width = labelSize.Width;

                // labelCanvas.Children.Add(funnelDataPoints[index].LabelVisual);
            }

            labelCanvas.Width += Chart.BEVEL_DEPTH;

            if (labelCanvas.Width > .6 * funnelChartCanvas.Width)
            {
                // Do some optimization here
            }

            // if funnelcanvas height is less than total labels height reduce the funnel canvas height
            //if (funnelCanvas.Height < totalLabelsHeight)
            //    funnelCanvas.Height -= (totalLabelsHeight - funnelCanvas.Height);

            funnelCanvas.Width = funnelChartCanvas.Width - labelCanvas.Width;
            labelCanvas.SetValue(Canvas.LeftProperty, funnelCanvas.Width);

            if (isStreamLine)
            {
                funnelCanvas.Height -= _streamLineParentTitleSize.Height;
                labelCanvas.Height -= _streamLineParentTitleSize.Height;

                funnelCanvas.SetValue(Canvas.TopProperty, _streamLineParentTitleSize.Height);
                labelCanvas.SetValue(Canvas.TopProperty, _streamLineParentTitleSize.Height);

                //funnelChartCanvas.Children.Add(funnelDataPoints[0].LabelVisual);
                funnelDataPoints[0].LabelVisual.SetValue(Canvas.LeftProperty, (Double)(funnelCanvas.Width - _streamLineParentTitleSize.Width) / 2);

                funnelDataPoints[0].Faces = new Faces();
                funnelDataPoints[0].Faces.VisualComponents.Add(funnelDataPoints[0].LabelVisual);
                funnelDataPoints[0].Faces.Visual = funnelDataPoints[0].LabelVisual;

                if ((funnelDataPoints[0].Chart as Chart).AnimationEnabled)
                {
                    // funnelDataPoints[0].Parent.Storyboard = AnimationHelper.ApplyOpacityAnimation(funnelDataPoints[0].LabelVisual, funnelDataPoints[0].Parent, funnelDataPoints[0].Parent.Storyboard, 1, funnelDataPoints[0].Opacity, 1);
                }
            }
        }

        /// <summary>
        /// Create labels for DataPoint
        /// </summary>
        /// <param name="dataPoint">DataPoint</param>
        /// <returns>Border</returns>
        private static Border CreateLabelForDataPoint(DataPoint dataPoint, Boolean isStreamLine, Int32 sliceIndex)
        {
            Title title = new Title()
            {
                IsNotificationEnable = false,
                Chart = dataPoint.Chart,
                Text = dataPoint.TextParser(dataPoint.LabelText),
                InternalFontSize = (Double)dataPoint.LabelFontSize,
                InternalFontColor = (isStreamLine && sliceIndex == 0) ? Chart.CalculateDataPointLabelFontColor(dataPoint.Chart as Chart, dataPoint, null, LabelStyles.OutSide) : Chart.CalculateDataPointLabelFontColor(dataPoint.Chart as Chart, dataPoint, dataPoint.LabelFontColor, (LabelStyles)dataPoint.LabelStyle),
                InternalFontFamily = dataPoint.LabelFontFamily,
                InternalFontStyle = (FontStyle)dataPoint.LabelFontStyle,
                InternalFontWeight = (FontWeight)dataPoint.LabelFontWeight,
                InternalBackground = dataPoint.LabelBackground
            };

            // If its a StreamLine funnel then default size of the Title should be a bit bigger 
            if (isStreamLine && sliceIndex == 0 && dataPoint.GetValue(DataPoint.LabelFontSizeProperty) == null && dataPoint.Parent.GetValue(DataPoint.LabelFontSizeProperty) == null)
            {
                title.InternalFontSize = 11.5;
            }

            title.CreateVisualObject(new ElementData() { Element = dataPoint });

            if (!(Boolean)dataPoint.LabelEnabled)
                title.Visual.Visibility = Visibility.Collapsed;

            return title.Visual;
        }

        /// <summary>
        /// Creates the funnel Chart
        /// </summary>
        /// <param name="dataSeries">DataSeries reference</param>
        /// <param name="dataPoints">List of sorted reference(If required)</param>
        /// <param name="isStreamLine">Whether its a streamline funnel chart</param>
        /// <param name="funnelCanvas">Funnel Canvas reference</param>
        /// <param name="minPointHeight">Min height of a funnel slice</param>
        /// <param name="is3D">Whether chart is a 3D chart</param>
        /// <param name="yScale">YScale of the chart</param>
        /// <param name="gapRatio">Gap between two data points while a particular datapoint is Exploded</param>
        /// <param name="isSameSlantAngle">Whether the same slant angle to be used while drawing each slice</param>
        /// <param name="bottomRadius">Bottom most raduis of a funnel</param>
        /// <param name="animationEnabled">Whether animation is enabled for chart</param>
        /// <returns>Canvas with funnel</returns>
        private static Canvas CreateFunnelChart(Grid _funnelChartGrid, DataSeries dataSeries, List<DataPoint> dataPoints, Boolean isStreamLine, Canvas funnelCanvas, Double minPointHeight, Boolean is3D, Double yScale, Double gapRatio, Boolean isSameSlantAngle, Double bottomRadius, Boolean animationEnabled)
        {
            Boolean isAnimationEnabled = (dataSeries.Chart as Chart).AnimationEnabled;
            Double plotHeight = funnelCanvas.Height;
            Double plotWidth = funnelCanvas.Width;

            // Canvas funnelCanvas = new Canvas() { Height = plotHeight, Width = plotWidth }; //, Background = new SolidColorBrush(Colors.LightGray) };

            FunnelSliceParms[] funnelSlices = CalculateFunnelSliceParmsInfo(isStreamLine, dataSeries, dataPoints, plotHeight, plotWidth - Chart.BEVEL_DEPTH, minPointHeight, is3D, yScale, gapRatio, isSameSlantAngle, bottomRadius);
            dataSeries.VisualParams = funnelSlices;
            
            Double topRadius = plotWidth / 2;
            Int32 zIndex = funnelSlices.Count() + 1;

            Random rand = new Random(DateTime.Now.Millisecond);
            Brush fillColor;
            Int32 sliceCount = funnelSlices.Count();
            Double totalFunnelActualHeight = 0;

            for (Int32 index = 0; index < sliceCount; index++)
            {
                funnelSlices[index].FillType = dataSeries.FillType;
                fillColor = funnelSlices[index].DataPoint.Color;

                Double yScaleTop = yScale * (funnelSlices[index].TopRadius / topRadius);
                Double yScaleBottom = yScale * (funnelSlices[index].BottomRadius / topRadius);

                if (Double.IsNaN(yScaleTop))
                    yScaleTop = 0.0000001;

                if (Double.IsNaN(yScaleBottom))
                    yScaleBottom = 0.0000001;

                Canvas sliceCanvas = GetFunnelSliceVisual(index, topRadius, is3D, funnelSlices[index], yScaleTop, yScaleBottom, fillColor, animationEnabled);

                funnelSlices[index].Top = funnelSlices[index].TopGap + ((index == 0) ? 0 : (funnelSlices[index - 1].Top + funnelSlices[index - 1].Height + funnelSlices[index - 1].BottomGap));

                sliceCanvas.SetValue(Canvas.TopProperty, funnelSlices[index].Top);
                sliceCanvas.SetValue(Canvas.ZIndexProperty, zIndex--);

                sliceCanvas.Height = funnelSlices[index].Height;
                sliceCanvas.Width = topRadius * 2;
                totalFunnelActualHeight += (funnelSlices[index].Height + funnelSlices[index].TopGap);

                // sliceCanvas.Background = new SolidColorBrush(Color.FromArgb((byte)255, (byte)rand.Next(228), (byte)rand.Next(128), (byte)rand.Next(228)));

                // here
                //sliceCanvas.Background = new SolidColorBrush(Color.FromArgb((byte)255, (byte)rand.Next(200 * index % 226), (byte)rand.Next(200 * index % 250), (byte)rand.Next(200 * index % 223)));

                funnelCanvas.Children.Add(sliceCanvas);

                if (isStreamLine && index == 0)
                {
                    // funnelCanvas.Height -= _streamLineParentTitleSize.Height;
                    // labelCanvas.Height -= _streamLineParentTitleSize.Height;

                    // funnelCanvas.SetValue(Canvas.TopProperty, _streamLineParentTitleSize.Height);
                    // labelCanvas.SetValue(Canvas.TopProperty, _streamLineParentTitleSize.Height);

                    sliceCanvas.Children.Add(dataPoints[0].LabelVisual);
                    dataPoints[0].Faces.Visual = dataPoints[0].LabelVisual;
                    dataPoints[0].LabelVisual.SetValue(Canvas.TopProperty, (Double)(-(_streamLineParentTitleSize.Height + (is3D ? yScale / 2 : 0))));
                    funnelSlices[index].DataPoint.VisualParams = null;
                }

                funnelSlices[index].DataPoint.Faces.Visual = sliceCanvas;

                funnelSlices[index].DataPoint.VisualParams = funnelSlices[index];
            }

            CalcutateExplodedPosition(ref funnelSlices, isStreamLine, yScale, dataSeries);

            // here
            //funnelCanvas.Background = new SolidColorBrush(Colors.Yellow);

            //if (!dataSeries.Exploded)
            {
                funnelCanvas.Height = totalFunnelActualHeight - _streamLineParentTitleSize.Height;
            }

            ArrangeLabels(funnelSlices, Double.NaN, _funnelChartGrid.Height);

            return funnelCanvas;
        }

        /// <summary>
        /// Arrange labels to overcome overlaps
        /// </summary>
        private static void ArrangeLabels(FunnelSliceParms[] funnelSlices, Double width, Double height)
        {
            if (funnelSlices == null || funnelSlices.Length < 0)
                return;

            FunnelSliceParms[] selectedfunnelSlices = (from fs in funnelSlices where fs.DataPoint.LabelStyle == LabelStyles.OutSide select fs).ToArray();

            Rect baseArea = new Rect(0, 0, width, height);
            Rect[] labelInfo = new Rect[selectedfunnelSlices.Length];

            for (Int32 index = 0; index < selectedfunnelSlices.Length; index++)
            {
                Double left = (Double)selectedfunnelSlices[index].DataPoint.LabelVisual.GetValue(Canvas.LeftProperty);
                Double top = (Double)selectedfunnelSlices[index].DataPoint.LabelVisual.GetValue(Canvas.TopProperty);

                labelInfo[index] = new Rect
                    (left,
                    selectedfunnelSlices[index].Top + top,
                    selectedfunnelSlices[index].DataPoint.LabelVisual.Width,
                    selectedfunnelSlices[index].DataPoint.LabelVisual.Height);
            }

            Visifire.Commons.LabelPlacementHelper.VerticalLabelPlacement(baseArea, ref labelInfo);

            for (Int32 index = 0; index < selectedfunnelSlices.Length; index++)
            {
                Double labelTop = labelInfo[index].Top - selectedfunnelSlices[index].Top;

                selectedfunnelSlices[index].DataPoint.LabelVisual.SetValue(Canvas.LeftProperty, labelInfo[index].Left);
                selectedfunnelSlices[index].DataPoint.LabelVisual.SetValue(Canvas.TopProperty, labelTop);

                selectedfunnelSlices[index].LabelLineEndPoint = new Point(selectedfunnelSlices[index].LabelLineEndPoint.X, labelTop + selectedfunnelSlices[index].DataPoint.LabelVisual.Height / 2);
                UpdateLabelLineEndPoint(selectedfunnelSlices[index]);
            }
        }

        private static void UpdateLabelLineEndPoint(FunnelSliceParms funnelSlice)
        {
            Path labelLine = funnelSlice.DataPoint.LabelLine;

            if (labelLine != null && labelLine.Data != null)
            {
                LineSegment ls = (((labelLine.Data as PathGeometry).Figures[0] as PathFigure).Segments[0] as LineSegment);
                ls.Point = funnelSlice.LabelLineEndPoint;
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
        /// <param name="funnelSlices"></param>
        private static void CalcutateExplodedPosition(ref FunnelSliceParms[] funnelSlices, Boolean isStreamLine, Double yScale, DataSeries dataSeries)
        {
            Int32 sliceCount = funnelSlices.Count();
            Int32 index = 0;

            if (funnelSlices[0].DataPoint.Parent.Exploded)
            {
                if (!funnelSlices[0].DataPoint.Chart.IsInDesignMode)
                {
                    Int32 midIndex = sliceCount / 2;
                    Double beginTime = 0.4;

                    if ((dataSeries.Chart as Chart).ChartArea._isFirstTimeRender)
                        beginTime = 1;

                    for (index = midIndex; index >= 0; index--)
                    {
                        if (index < 0)
                            break;

                        Double yPosition = funnelSlices[index].Top - (midIndex - index) * _singleGap;// -yScale / 2;
                        dataSeries.Storyboard = CreateExplodingAnimation(dataSeries, funnelSlices[index].DataPoint, dataSeries.Storyboard, funnelSlices[index].DataPoint.Faces.Visual as Panel, yPosition, beginTime);
                    }

                    for (index = midIndex + 1; index < sliceCount; index++)
                    {
                        Double yPosition = funnelSlices[index].Top + (index - midIndex) * _singleGap;// -yScale / 2;
                        dataSeries.Storyboard = CreateExplodingAnimation(dataSeries, funnelSlices[index].DataPoint, dataSeries.Storyboard, funnelSlices[index].DataPoint.Faces.Visual as Panel, yPosition, beginTime);
                    }

                    if (dataSeries.Chart != null && !(dataSeries.Chart as Chart).ChartArea._isFirstTimeRender)
#if WPF
                        dataSeries.Storyboard.Begin(dataSeries.Chart._rootElement, true);
#else
                        dataSeries.Storyboard.Begin();
#endif

                }
            }
            else
            {
                Storyboard unExplodeStoryBoard = new Storyboard();

                for (; index < sliceCount; index++)
                {
                    funnelSlices[index].ExplodedPoints = new List<Point>();
                    funnelSlices[index].DataPoint.ExplodeAnimation = new Storyboard();
                    // For top slice
                    if (index == 0)
                    {
                        funnelSlices[index].ExplodedPoints.Add(new Point(0, funnelSlices[index].Top - _singleGap / 2));

                        funnelSlices[index].DataPoint.ExplodeAnimation = CreateExplodingAnimation(dataSeries, funnelSlices[index].DataPoint, funnelSlices[index].DataPoint.ExplodeAnimation, funnelSlices[index].DataPoint.Faces.Visual as Panel, (funnelSlices[index].Top - _singleGap / 2), 0);
                        //unExplodeStoryBoard = CreateExplodingAnimation(unExplodeStoryBoard, funnelSlices[index].DataPoint.Faces.Visual as Panel, funnelSlices[index].Top);

                        for (Int32 i = 1; i < funnelSlices.Length; i++)
                        {
                            funnelSlices[index].ExplodedPoints.Add(new Point(0, funnelSlices[i].Top + _singleGap / 2));
                            funnelSlices[index].DataPoint.ExplodeAnimation = CreateExplodingAnimation(dataSeries, funnelSlices[index].DataPoint, funnelSlices[index].DataPoint.ExplodeAnimation, funnelSlices[i].DataPoint.Faces.Visual as Panel, (funnelSlices[i].Top + _singleGap / 2), 0);
                            //unExplodeStoryBoard = CreateExplodingAnimation(unExplodeStoryBoard, funnelSlices[i].DataPoint.Faces.Visual as Panel, funnelSlices[i].Top);
                        }

                    }
                    // For bottom slice
                    else if (index == funnelSlices.Length - 1)
                    {
                        Int32 i = 0;

                        for (; i < index; i++)
                        {
                            funnelSlices[index].ExplodedPoints.Add(new Point(0, funnelSlices[i].Top - _singleGap / 2 + _singleGap / 6));
                            funnelSlices[index].DataPoint.ExplodeAnimation = CreateExplodingAnimation(dataSeries, funnelSlices[index].DataPoint, funnelSlices[index].DataPoint.ExplodeAnimation, funnelSlices[i].DataPoint.Faces.Visual as Panel, (funnelSlices[i].Top - _singleGap / 2 + _singleGap / 6), 0);
                        }

                        funnelSlices[index].ExplodedPoints.Add(new Point(0, funnelSlices[i].Top + _singleGap / 2 + _singleGap / 6));
                        funnelSlices[index].DataPoint.ExplodeAnimation = CreateExplodingAnimation(dataSeries, funnelSlices[index].DataPoint, funnelSlices[i].DataPoint.ExplodeAnimation, funnelSlices[i].DataPoint.Faces.Visual as Panel, (funnelSlices[i].Top + _singleGap / 2 + _singleGap / 6), 0);
                    }
                    // For other slice
                    else
                    {
                        Int32 i;

                        for (i = 0; i < index; i++)
                        {
                            funnelSlices[index].ExplodedPoints.Add(new Point(0, funnelSlices[i].Top - _singleGap / 2));
                            funnelSlices[index].DataPoint.ExplodeAnimation = CreateExplodingAnimation(dataSeries, funnelSlices[index].DataPoint, funnelSlices[index].DataPoint.ExplodeAnimation, funnelSlices[i].DataPoint.Faces.Visual as Panel, (funnelSlices[i].Top - _singleGap / 2), 0);
                        }

                        funnelSlices[index].ExplodedPoints.Add(new Point(0, funnelSlices[i].Top));
                        funnelSlices[index].DataPoint.ExplodeAnimation = CreateExplodingAnimation(dataSeries, funnelSlices[index].DataPoint, funnelSlices[index].DataPoint.ExplodeAnimation, funnelSlices[index].DataPoint.Faces.Visual as Panel, funnelSlices[index].Top, 0);

                        for (++i; i < funnelSlices.Length; i++)
                        {
                            funnelSlices[index].ExplodedPoints.Add(new Point(0, funnelSlices[i].Top + _singleGap / 2));
                            funnelSlices[index].DataPoint.ExplodeAnimation = CreateExplodingAnimation(dataSeries, funnelSlices[index].DataPoint, funnelSlices[index].DataPoint.ExplodeAnimation, funnelSlices[i].DataPoint.Faces.Visual as Panel, (funnelSlices[i].Top + _singleGap / 2), 0);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Calculate funnelSliceParms information
        /// </summary>
        /// <param name="isStreamLine">Whether its a streamline funnel chart</param>
        /// <param name="dataSeries">DataSeries reference</param>
        /// <param name="dataPoints">List of sorted reference(If required)</param>
        /// <param name="plotHeight">Total height of the plot canvas</param>
        /// <param name="plotWidth">Total width of the plot canvas</param>
        /// <param name="minPointHeight">Min height of a funnel slice</param>
        /// <param name="is3D">Whether chart is a 3D chart</param>
        /// <param name="yScale">YScale of the chart</param>
        /// <param name="gapRatio">Gap between two data points while a particular datapoint is Exploded</param>
        /// <param name="isSameSlantAngle">Whether the same slant angle to be used while drawing each slice</param>
        /// <param name="bottomRadius">Bottom most raduis of a funnel</param>
        /// <returns>FunnelSliceParms[]</returns>
        private static FunnelSliceParms[] CalculateFunnelSliceParmsInfo(Boolean isStreamLine, DataSeries dataSeries, List<DataPoint> dataPoints, Double plotHeight, Double plotWidth, Double minPointHeight, Boolean is3D, Double yScale, Double gapRatio, Boolean isSameSlantAngle, Double bottomRadius)
        {
            // Initialize funnel Slices parameters
            FunnelSliceParms[] funnelSlicesParms;

            // Actual funnel height
            // For 3d funnel height will be reduced to maintain yScale
            Double funnelHeight;

            if (dataSeries.Exploded)
            {
                gapRatio = 0.02;

                // Single gap height
                _singleGap = gapRatio * plotHeight;

                _totalGap = _singleGap * ((isStreamLine) ? dataPoints.Count : (dataPoints.Count + 1));
            }
            else
            {
                // Single gap height
                _singleGap = gapRatio * plotHeight;

                _totalGap = _singleGap * 2;
            }

            // Actual funnel height
            // For 3d funnel height will be reduced to maintain yScale
            funnelHeight = plotHeight - _totalGap - (is3D ? yScale / 2 : 0);

            if (!isStreamLine)
            {
                #region Sectional Funnel

                // Initialization of funnelSliceParms
                funnelSlicesParms = new FunnelSliceParms[dataPoints.Count];

                // Calculate sum of values
                Double sum = (from dp in dataPoints select dp.YValue).Sum();

                // Min YValue
                Double min = (from dp in dataPoints select dp.YValue).Min();

                // Funnel angle
                Double theta = Math.Atan((plotWidth / 2 - bottomRadius) / funnelHeight);

                // Height of the funnel drawn considering bottom radius
                funnelHeight -= Math.Tan(theta) / ((bottomRadius == 0) ? 1 : bottomRadius);

                // Creating prams for each funnel slice
                for (Int32 index = 0; index < dataPoints.Count; index++)
                {
                    funnelSlicesParms[index] = new FunnelSliceParms() { DataPoint = dataPoints[index], TopAngle = Math.PI / 2 - theta, BottomAngle = Math.PI / 2 + theta };

                    funnelSlicesParms[index].Height = funnelHeight * (dataPoints[index].YValue / sum);
                    funnelSlicesParms[index].TopRadius = index == 0 ? plotWidth / 2 : funnelSlicesParms[index - 1].BottomRadius;
                    funnelSlicesParms[index].BottomRadius = funnelSlicesParms[index].TopRadius - funnelSlicesParms[index].Height * Math.Tan(theta);
                    /*
                    // Set topgap and bottom gap
                    if (index == 0)
                        funnelSlicesParms[index].TopGap = singleGap + ((index == 0 && is3D) ? yScale / 2 : 0);
                    else if ((Boolean)funnelSlicesParms[index].DataPoint.Exploded)
                        funnelSlicesParms[index].TopGap = singleGap + ((index == 0 && is3D) ? yScale / 2 : 0);

                    if (index == dataPoints.Count - 1)
                        funnelSlicesParms[index].BottomGap = singleGap;
                    else if ((Boolean)funnelSlicesParms[index].DataPoint.Exploded)
                        funnelSlicesParms[index].BottomGap = singleGap;*/

                    //--------------
                    funnelSlicesParms[index].TopGap = ((index == 0 && is3D) ? yScale / 2 : 0);
                    //--------------

                }

                if (!Double.IsNaN(minPointHeight))
                {
                    Boolean isFixedSize = false;    // Whether to fix height of all slice
                    Double fixedSliceHeight = 0;

                    Double totalSumOfHeight = (from funnelSlice in funnelSlicesParms select funnelSlice.Height).Sum();
                    fixedSliceHeight = totalSumOfHeight / funnelSlicesParms.Length;

                    // Calculate minPointHeight in terms of pixel value
                    minPointHeight = (minPointHeight / 100) * funnelHeight;

                    // Funnel slices where height is less than the minPointHeight
                    List<FunnelSliceParms> fixedHeightFunnelSlices = (from funnelSlice in funnelSlicesParms where funnelSlice.Height < minPointHeight select funnelSlice).ToList();

                    List<FunnelSliceParms> variableHeightFunnelSlices = (from funnelSlice in funnelSlicesParms
                                                                         where !(from slice in fixedHeightFunnelSlices select slice).Contains(funnelSlice)
                                                                         select funnelSlice).ToList();

                    if (minPointHeight > fixedSliceHeight || fixedHeightFunnelSlices.Count == funnelSlicesParms.Count())
                    {
                        isFixedSize = true;
                    }

                    Double sumOfHeightOfSlice2BeFixed = (from funnelSlice in fixedHeightFunnelSlices select funnelSlice.Height).Sum();
                    Double sumOfVariableHeightSlices = (from funnelSlice in variableHeightFunnelSlices select funnelSlice.Height).Sum();

                    Double totalHeight2Reduce = minPointHeight * fixedHeightFunnelSlices.Count() - sumOfHeightOfSlice2BeFixed;

                    // Creating prams for each funnel slice
                    for (Int32 index = 0; index < dataPoints.Count; index++)
                    {
                        if (isFixedSize)
                        {
                            funnelSlicesParms[index].Height = fixedSliceHeight;
                        }
                        else
                        {
                            if (funnelSlicesParms[index].Height < minPointHeight)
                                funnelSlicesParms[index].Height = minPointHeight;
                            else
                                funnelSlicesParms[index].Height -= totalHeight2Reduce * (funnelSlicesParms[index].Height / sumOfVariableHeightSlices);
                        }

                        funnelSlicesParms[index].TopRadius = index == 0 ? plotWidth / 2 : funnelSlicesParms[index - 1].BottomRadius;
                        funnelSlicesParms[index].BottomRadius = funnelSlicesParms[index].TopRadius - funnelSlicesParms[index].Height * Math.Tan(theta);

                        // Set topgap and bottom gap
                        /*if (index == 0 || (Boolean)funnelSlicesParms[index].DataPoint.Exploded)
                            funnelSlicesParms[index].TopGap = _singleGap + ((index == 0 && is3D) ? yScale / 2 : 0);
                        
                        if(index == dataPoints.Count -1 || (Boolean) funnelSlicesParms[index].DataPoint.Exploded)
                            funnelSlicesParms[index].BottomGap = _singleGap;
                         */
                    }
                }

                #endregion
            }
            else
            {
                #region StreamLineFunnel

                funnelHeight -= _streamLineParentTitleSize.Height;

                // Initialization of iOValuePairs
                IOValuePair[] iOValuePairs = new IOValuePair[dataPoints.Count];

                for (Int32 index = 0; index < dataPoints.Count; index++)
                {
                    iOValuePairs[index] = new IOValuePair();
                    iOValuePairs[index].InputValue = ((index == 0) ? Double.NaN : dataPoints[index - 1].YValue);
                    iOValuePairs[index].OutPutValue = dataPoints[index].YValue;
                }

                // Initialization of funnelSliceParms
                funnelSlicesParms = new FunnelSliceParms[dataPoints.Count - 1];

                // Creating prams for each funnel slice
                Int32 slicesIndex;

                for (Int32 index = 1; index < iOValuePairs.Count(); index++)
                {
                    slicesIndex = index - 1;

                    funnelSlicesParms[slicesIndex] = new FunnelSliceParms() { DataPoint = dataPoints[index] };

                    funnelSlicesParms[slicesIndex].Height = (((iOValuePairs[index].InputValue - iOValuePairs[index].OutPutValue) / iOValuePairs[0].OutPutValue) * funnelHeight);
                    funnelSlicesParms[slicesIndex].TopRadius = slicesIndex == 0 ? plotWidth / 2 : funnelSlicesParms[slicesIndex - 1].BottomRadius;

                    if (!isSameSlantAngle)
                        funnelSlicesParms[slicesIndex].BottomRadius = (funnelSlicesParms[slicesIndex].TopRadius * Math.Sqrt(iOValuePairs[index].OutPutValue / iOValuePairs[index].InputValue));
                    else
                        funnelSlicesParms[slicesIndex].BottomRadius = (funnelSlicesParms[slicesIndex].TopRadius * (iOValuePairs[index].OutPutValue / iOValuePairs[index].InputValue));

                    Double theta = Math.Atan((funnelSlicesParms[slicesIndex].TopRadius - funnelSlicesParms[slicesIndex].BottomRadius) / funnelSlicesParms[slicesIndex].Height);
                    funnelSlicesParms[slicesIndex].TopAngle = Math.PI / 2 - theta;
                    funnelSlicesParms[slicesIndex].BottomAngle = Math.PI / 2 + theta;

                    /*
                    // Set top and bottom gap
                    if(index == 1 || (Boolean)funnelSlicesParms[slicesIndex].DataPoint.Exploded)
                        funnelSlicesParms[slicesIndex].TopGap = singleGap + ((slicesIndex == 0 && is3D) ? yScale / 2 : 0);

                    if (index == iOValuePairs.Count() - 1 || (Boolean)funnelSlicesParms[slicesIndex].DataPoint.Exploded)
                        funnelSlicesParms[slicesIndex].BottomGap = singleGap;
                    */

                    FixTopAndBottomRadiusForStreamLineFunnel(ref funnelSlicesParms[slicesIndex]);
                }

                // Enlarge Funnel Height-----------

                Double totalSumOfHeight = (from funnelSlice in funnelSlicesParms select funnelSlice.Height).Sum();

                if (totalSumOfHeight < funnelHeight)
                {
                    for (Int32 index = 1; index < iOValuePairs.Count(); index++)
                    {
                        slicesIndex = index - 1;

                        //funnelSlicesParms[slicesIndex] = new FunnelSliceParms() { DataPoint = dataPoints[index] };

                        funnelSlicesParms[slicesIndex].Height += (funnelHeight - totalSumOfHeight) * (funnelSlicesParms[slicesIndex].Height / totalSumOfHeight);

                        funnelSlicesParms[slicesIndex].TopRadius = slicesIndex == 0 ? plotWidth / 2 : funnelSlicesParms[slicesIndex - 1].BottomRadius;

                        if (!isSameSlantAngle)
                            funnelSlicesParms[slicesIndex].BottomRadius = Math.Round(funnelSlicesParms[slicesIndex].TopRadius * Math.Sqrt(iOValuePairs[index].OutPutValue / iOValuePairs[index].InputValue));
                        else
                            funnelSlicesParms[slicesIndex].BottomRadius = Math.Round(funnelSlicesParms[slicesIndex].TopRadius * (iOValuePairs[index].OutPutValue / iOValuePairs[index].InputValue));

                        Double theta = Math.Atan((funnelSlicesParms[slicesIndex].TopRadius - funnelSlicesParms[slicesIndex].BottomRadius) / funnelSlicesParms[slicesIndex].Height);
                        funnelSlicesParms[slicesIndex].TopAngle = Math.PI / 2 - theta;
                        funnelSlicesParms[slicesIndex].BottomAngle = Math.PI / 2 + theta;

                        /*
                        // Set top and bottom gap
                        if(index == 1 || (Boolean)funnelSlicesParms[slicesIndex].DataPoint.Exploded)
                            funnelSlicesParms[slicesIndex].TopGap = singleGap + ((slicesIndex == 0 && is3D) ? yScale / 2 : 0);

                        if (index == iOValuePairs.Count() - 1 || (Boolean)funnelSlicesParms[slicesIndex].DataPoint.Exploded)
                            funnelSlicesParms[slicesIndex].BottomGap = singleGap;
                        */

                        FixTopAndBottomRadiusForStreamLineFunnel(ref funnelSlicesParms[slicesIndex]);
                    }
                }

                // End Enlarge funnel Height-------


                if (!Double.IsNaN(minPointHeight))
                {
                    Boolean isFixedSize = false;    // Whether to fix height of all slice
                    Double fixedSliceHeight = 0;
                    Double funnelActualHeight = funnelHeight - _streamLineParentTitleSize.Height;

                    fixedSliceHeight = funnelActualHeight / funnelSlicesParms.Length;

                    // Calculate minPointHeight in terms of pixel value
                    minPointHeight = (minPointHeight / 100) * funnelHeight;

                    // Funnel slices where height is less than the minPointHeight
                    var fixedHeightFunnelSlices = (from funnelSlice in funnelSlicesParms where funnelSlice.Height < minPointHeight select funnelSlice);

                    var variableHeightFunnelSlices = (from funnelSlice in funnelSlicesParms
                                                      where !(from slice in fixedHeightFunnelSlices select slice).Contains(funnelSlice)
                                                      select funnelSlice);

                    if (minPointHeight > fixedSliceHeight || fixedHeightFunnelSlices.Count() == funnelSlicesParms.Count())
                    {
                        isFixedSize = true;
                    }

                    //Double totalSumOfHeight = (from funnelSlice in funnelSlicesParms select funnelSlice.Height).Sum();
                    Double sumOfHeightOfSlice2BeFixed = (from funnelSlice in fixedHeightFunnelSlices select funnelSlice.Height).Sum();
                    Double sumOfVariableHeightSlices = (from funnelSlice in variableHeightFunnelSlices select funnelSlice.Height).Sum();

                    Double totalHeight2Reduce = minPointHeight * fixedHeightFunnelSlices.Count() - sumOfHeightOfSlice2BeFixed;

                    for (Int32 index = 1; index < iOValuePairs.Count(); index++)
                    {
                        slicesIndex = index - 1;

                        if (isFixedSize)
                        {
                            funnelSlicesParms[slicesIndex].Height = fixedSliceHeight;
                        }
                        else
                        {
                            if (funnelSlicesParms[slicesIndex].Height < minPointHeight)
                                funnelSlicesParms[slicesIndex].Height = minPointHeight;
                            else
                                funnelSlicesParms[slicesIndex].Height -= totalHeight2Reduce * (funnelSlicesParms[slicesIndex].Height / sumOfVariableHeightSlices);
                        }

                        funnelSlicesParms[slicesIndex].TopRadius = slicesIndex == 0 ? plotWidth / 2 : funnelSlicesParms[slicesIndex - 1].BottomRadius;

                        if (!isSameSlantAngle)
                            funnelSlicesParms[slicesIndex].BottomRadius = Math.Round(funnelSlicesParms[slicesIndex].TopRadius * Math.Sqrt(iOValuePairs[index].OutPutValue / iOValuePairs[index].InputValue));
                        else
                            funnelSlicesParms[slicesIndex].BottomRadius = Math.Round(funnelSlicesParms[slicesIndex].TopRadius * (iOValuePairs[index].OutPutValue / iOValuePairs[index].InputValue));

                        // Set top and bottom gap
                        /*if(index == 1 || (Boolean)funnelSlicesParms[slicesIndex].DataPoint.Exploded)
                            funnelSlicesParms[slicesIndex].TopGap = _singleGap + ((slicesIndex == 0 && is3D) ? yScale / 2 : 0);
                        
                        if(index == iOValuePairs.Count() -1 || (Boolean)funnelSlicesParms[slicesIndex].DataPoint.Exploded)
                            funnelSlicesParms[slicesIndex].BottomGap = _singleGap;
                        */

                        // Calculate funnel angle
                        // funnelSlicesParms[slicesIndex].TopAngle = Math.PI / 2 - Math.Atan((funnelSlicesParms[slicesIndex].TopRadius - funnelSlicesParms[slicesIndex].BottomRadius) / funnelSlicesParms[slicesIndex].Height);

                        FixTopAndBottomRadiusForStreamLineFunnel(ref funnelSlicesParms[slicesIndex]);

                        Double theta = Math.Atan((funnelSlicesParms[slicesIndex].TopRadius - funnelSlicesParms[slicesIndex].BottomRadius) / funnelSlicesParms[slicesIndex].Height);
                        funnelSlicesParms[slicesIndex].TopAngle = Math.PI / 2 - theta;
                        funnelSlicesParms[slicesIndex].BottomAngle = Math.PI / 2 + theta;
                    }
                }
            }

                #endregion

            return funnelSlicesParms;
        }

        public static void FixTopAndBottomRadiusForStreamLineFunnel(ref FunnelSliceParms funnelSlice)
        {
            if (Double.IsNaN(funnelSlice.TopRadius))
            {
                funnelSlice.TopRadius = 0.00000001;
                funnelSlice.Height = 0;
            }

            if (Double.IsNaN(funnelSlice.BottomRadius))
                funnelSlice.BottomRadius = 0.0000001;

            if (Double.IsNaN(funnelSlice.Height))
                funnelSlice.Height = 0.0000001;
        }

        /// <summary>
        /// Returns the visual of a funnel slice
        /// </summary>
        /// <param name="finnelIndex">Slice index</param>
        /// <param name="topRadius">Top radius of the Funnel</param>
        /// <param name="is3D">Whether the chart is a 3D Chart</param>
        /// <param name="funnelSlice">Funnel Slice reference</param>
        /// <param name="yScaleTop">Top y-scale for 3D slice</param>
        /// <param name="yScaleBottom">Bottom y-scale for 3D slice</param>
        /// <param name="fillColor">Fill Color of the funnel slice</param>
        /// <param name="animationEnabled">Whether the animation is enabled</param>
        /// <returns>Funnel slice canvas</returns>
        private static Canvas GetFunnelSliceVisual(Int32 funnelSliceIndex, Double topRadius, Boolean is3D, FunnelSliceParms funnelSlice, Double yScaleTop, Double yScaleBottom, Brush fillColor, Boolean animationEnabled)
        {
            funnelSlice.Index = funnelSliceIndex;
            Canvas sliceCanvas = CreateFunnelSlice(false, topRadius, is3D, funnelSlice, yScaleTop, yScaleBottom, fillColor, fillColor, fillColor, animationEnabled);

            if ((Boolean)funnelSlice.DataPoint.LightingEnabled)
            {
                Brush highlightBrush4Stroke = GetLightingBrushForStroke(fillColor, funnelSlice.Index);
                Brush sideFillColor = (Boolean)funnelSlice.DataPoint.LightingEnabled ? GetSideLightingBrush(funnelSlice) : fillColor;
                Brush topFillColor = is3D ? GetTopBrush(fillColor, funnelSlice) : fillColor;
                Canvas gradientCanvas = CreateFunnelSlice(true, topRadius, is3D, funnelSlice, yScaleTop, yScaleBottom, sideFillColor, topFillColor, highlightBrush4Stroke, animationEnabled);
                sliceCanvas.Children.Add(gradientCanvas);
            }

            return sliceCanvas;
        }

        /// <summary>
        /// Get stroke brush for Top lighting Ellipse
        /// </summary>
        /// <param name="fillColor"></param>
        /// <param name="funnelSliceIndex"></param>
        /// <returns></returns>
        private static Brush GetLightingBrushForStroke(Brush fillColor, Int32 funnelSliceIndex)
        {
            SolidColorBrush fillColorGradientBrush = fillColor as SolidColorBrush;
            Brush highlightBrush4Stroke = fillColor;

            if (funnelSliceIndex > 0 && fillColorGradientBrush != null)
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
        internal static void ReCalculateAndApplyTheNewBrush(Shape shape, Brush newBrush, Boolean isLightingEnabled, Boolean is3D, FunnelSliceParms funnelSliceParms)
        {
            switch ((shape.Tag as ElementData).VisualElementName)
            {
                case "FunnelBase":
                case "FunnelTop": shape.Fill = newBrush; shape.Stroke = newBrush; break;
                case "TopBevel": shape.Fill = Graphics.GetBevelTopBrush(newBrush, 90); break;
                case "LeftBevel": shape.Fill = Graphics.GetBevelSideBrush(45, newBrush); break;
                case "RightBevel": shape.Fill = Graphics.GetBevelSideBrush(45, newBrush); break;
                case "BottomBevel": shape.Fill = Graphics.GetBevelSideBrush(180, newBrush); break;
                case "Lighting": shape.Fill = isLightingEnabled ? GetSideLightingBrush(funnelSliceParms) : newBrush; break;
                case "FunnelTopLighting":
                    shape.Fill = is3D ? GetTopBrush(newBrush, funnelSliceParms) : newBrush;
                    shape.Stroke = GetLightingBrushForStroke(newBrush, funnelSliceParms.Index);
                    break;
            }
        }

        /// <summary>
        /// Apply Bevel effect for a funnel slice
        /// </summary>
        /// <param name="parentVisual">Parent canvas of Bevel layer</param>
        /// <param name="funnelSlice">funnelSlice</param>
        /// <param name="sideFillColor">Side fill color</param>
        /// <param name="points">Funnel Points</param>
        private static void ApplyFunnelBevel(Canvas parentVisual, FunnelSliceParms funnelSlice, Brush sideFillColor, Point[] points)
        {
            if (funnelSlice.DataPoint.Parent.Bevel && funnelSlice.Height > Chart.BEVEL_DEPTH)
            {
                // Generate Inner Points
                CalculateBevelInnerPoints(funnelSlice, points);

                Path topBevelPath = ExtendedGraphics.GetPathFromPoints(Graphics.GetBevelTopBrush(sideFillColor, 90), points[0], points[4], points[5], points[1]);
                Path leftBevelPath = ExtendedGraphics.GetPathFromPoints(Graphics.GetBevelSideBrush(45, sideFillColor), points[0], points[4], points[7], points[3]);
                Path rightBevelPath = ExtendedGraphics.GetPathFromPoints(Graphics.GetBevelSideBrush(45, sideFillColor), points[1], points[5], points[6], points[2]);
                Path bottomBevelPath = ExtendedGraphics.GetPathFromPoints(Graphics.GetBevelSideBrush(180, sideFillColor), points[7], points[6], points[2], points[3]);

                topBevelPath.IsHitTestVisible = false;
                leftBevelPath.IsHitTestVisible = false;
                rightBevelPath.IsHitTestVisible = false;
                bottomBevelPath.IsHitTestVisible = false;

                parentVisual.Children.Add(topBevelPath);
                parentVisual.Children.Add(leftBevelPath);
                parentVisual.Children.Add(rightBevelPath);
                parentVisual.Children.Add(bottomBevelPath);

                topBevelPath.Tag = new ElementData() { Element = funnelSlice.DataPoint, VisualElementName = "TopBevel" };
                leftBevelPath.Tag = new ElementData() { Element = funnelSlice.DataPoint, VisualElementName = "LeftBevel" };
                rightBevelPath.Tag = new ElementData() { Element = funnelSlice.DataPoint, VisualElementName = "RightBevel" };
                bottomBevelPath.Tag = new ElementData() { Element = funnelSlice.DataPoint, VisualElementName = "BottomBevel" };

                funnelSlice.DataPoint.Faces.Parts.Add(topBevelPath);
                funnelSlice.DataPoint.Faces.Parts.Add(leftBevelPath);
                funnelSlice.DataPoint.Faces.Parts.Add(rightBevelPath);
                funnelSlice.DataPoint.Faces.Parts.Add(bottomBevelPath);

                funnelSlice.DataPoint.Faces.VisualComponents.Add(topBevelPath);
                funnelSlice.DataPoint.Faces.VisualComponents.Add(leftBevelPath);
                funnelSlice.DataPoint.Faces.VisualComponents.Add(rightBevelPath);
                funnelSlice.DataPoint.Faces.VisualComponents.Add(bottomBevelPath);

                if ((funnelSlice.DataPoint.Chart as Chart).AnimationEnabled)
                {
                    funnelSlice.DataPoint.Parent.Storyboard = AnimationHelper.ApplyOpacityAnimation(topBevelPath, funnelSlice.DataPoint.Parent, funnelSlice.DataPoint.Parent.Storyboard, 0, funnelSlice.DataPoint.InternalOpacity, 0, 1);
                    funnelSlice.DataPoint.Parent.Storyboard = AnimationHelper.ApplyOpacityAnimation(leftBevelPath, funnelSlice.DataPoint.Parent, funnelSlice.DataPoint.Parent.Storyboard, 0, funnelSlice.DataPoint.InternalOpacity, 0, 1);
                    funnelSlice.DataPoint.Parent.Storyboard = AnimationHelper.ApplyOpacityAnimation(rightBevelPath, funnelSlice.DataPoint.Parent, funnelSlice.DataPoint.Parent.Storyboard, 0, funnelSlice.DataPoint.InternalOpacity, 0, 1);
                    funnelSlice.DataPoint.Parent.Storyboard = AnimationHelper.ApplyOpacityAnimation(bottomBevelPath, funnelSlice.DataPoint.Parent, funnelSlice.DataPoint.Parent.Storyboard, 0, funnelSlice.DataPoint.InternalOpacity, 0, 1);
                }
            }
        }

        /// <summary>
        /// Create a slice of a funnel
        /// </summary>
        /// <param name="isLightingGradientLayer">Whether CreateFunnelSlice() function should create a layer for lighting</param>
        /// <param name="topRadius">Top Radius of the funnel</param>
        /// <param name="is3D">Whether the chart is a 3D Chart</param>
        /// <param name="funnelSlice">FunnelSlice canvas reference</param>
        /// <param name="yScaleTop">Top YScale for 3D view</param>
        /// <param name="yScaleBottom">Bottom YScale for 3D view</param>
        /// <param name="sideFillColor">Side surface fill color</param>
        /// <param name="topFillColor">Top surface fill color</param>
        /// <param name="topSurfaceStroke">Top surface stroke color</param>
        /// <param name="animationEnabled">Whether animation is enabled</param>
        /// <returns>Return funnel slice canvas</returns>
        private static Canvas CreateFunnelSlice(Boolean isLightingGradientLayer, Double topRadius, Boolean is3D, FunnelSliceParms funnelSlice, Double yScaleTop, Double yScaleBottom, Brush sideFillColor, Brush topFillColor, Brush topSurfaceStroke, Boolean animationEnabled)
        {
            Double SOLID_FUNNEL_EDGE_THICKNESS = 3;

            if (funnelSlice.Index == 0 && is3D && isLightingGradientLayer && (funnelSlice.FillType == FillType.Solid))
            {   
                funnelSlice.Height += SOLID_FUNNEL_EDGE_THICKNESS;
            }

            Canvas sliceCanvas = new Canvas() { Tag = new ElementData() { Element = funnelSlice.DataPoint } };
            Canvas visual = new Canvas() { Width = topRadius * 2, Height = funnelSlice.Height, Tag = new ElementData() { Element = funnelSlice.DataPoint } };  // Canvas holds a slice of a funnel chart
            Faces faces = null;


            // GeometryGroup for for a funnel slice path
            GeometryGroup geometryGroup = new GeometryGroup();

            // PathGeometry for for a funnel slice path
            PathGeometry pathGeometry = new PathGeometry();

            // pathFigure for for a funnel slice path
            PathFigure pathFigure = new PathFigure() { StartPoint = new Point(topRadius - funnelSlice.TopRadius, 0) };  // PathFigure of a funnel slice


            // Path for for a funnel slice
            Path path4Slice = new Path() { Fill = sideFillColor };

            path4Slice.Tag = new ElementData() { Element = funnelSlice.DataPoint };

            // Add PathGeometry to GeometryGroup
            geometryGroup.Children.Add(pathGeometry);

            // Set path data
            path4Slice.Data = geometryGroup;

            // Set properties for path
            path4Slice.StrokeThickness = 0;
            path4Slice.Stroke = new SolidColorBrush(Colors.Black);

            // Add path elements to its parent canvas
            pathGeometry.Figures.Add(pathFigure);
            visual.Children.Add(path4Slice);
            
            if (is3D)
            {
                
                #region 3D

                geometryGroup.FillRule = FillRule.Nonzero;

                // Create top arc left
                ArcSegment arcSegment = new ArcSegment();
                arcSegment.Point = new Point(topRadius, yScaleTop / 2);
                arcSegment.Size = new Size(funnelSlice.TopRadius, yScaleTop / 2);
                pathFigure.Segments.Add(arcSegment);

                // Create top arc right
                arcSegment = new ArcSegment();
                arcSegment.Point = new Point(topRadius + funnelSlice.TopRadius, 0);
                arcSegment.Size = new Size(funnelSlice.TopRadius, yScaleTop / 2);
                pathFigure.Segments.Add(arcSegment);

                // Create right Plain
                LineSegment lineSegment = new LineSegment() { Point = new Point(topRadius + funnelSlice.BottomRadius, funnelSlice.Height) };
                pathFigure.Segments.Add(lineSegment);

                lineSegment = new LineSegment() { Point = new Point(topRadius - funnelSlice.BottomRadius, funnelSlice.Height) };
                pathFigure.Segments.Add(lineSegment);

                // Create left Plain
                lineSegment = new LineSegment() { Point = new Point(topRadius - funnelSlice.TopRadius, 0) };
                pathFigure.Segments.Add(lineSegment);

                EllipseGeometry ellipseGeometry = new EllipseGeometry();
                ellipseGeometry.Center = new Point(topRadius, funnelSlice.Height);
                ellipseGeometry.RadiusX = funnelSlice.BottomRadius;
                ellipseGeometry.RadiusY = yScaleBottom / 2;

                geometryGroup.Children.Add(ellipseGeometry);

                // Create ellips for the funnel top
                Ellipse funnelTopEllipse = new Ellipse() { Height = yScaleTop, Width = funnelSlice.TopRadius * 2, Fill = topFillColor, Tag = new ElementData() { Element = funnelSlice.DataPoint } };

                funnelTopEllipse.SetValue(Canvas.TopProperty, -yScaleTop / 2);
                funnelTopEllipse.SetValue(Canvas.LeftProperty, topRadius - funnelSlice.TopRadius);

                //if (funnelSlice.DataPoint.Parent.Bevel)
                    funnelTopEllipse.StrokeThickness = 1.24;
                //else
                //    funnelTopEllipse.StrokeThickness = 0.24;

                funnelTopEllipse.Stroke = Graphics.GetBevelTopBrush(topSurfaceStroke, 0);

                visual.Children.Add(funnelTopEllipse);

                if (!isLightingGradientLayer)
                {
                    // Update faces for the DataPoint
                    faces = new Faces();
                    faces.VisualComponents.Add(path4Slice);
                    faces.VisualComponents.Add(funnelTopEllipse);

                    (path4Slice.Tag as ElementData).VisualElementName = "FunnelBase";
                    (funnelTopEllipse.Tag as ElementData).VisualElementName = "FunnelTop";

                    #region Creating Seperate BorderLine

                    GeometryGroup borderGeometryGroup = new GeometryGroup();

                    // PathGeometry for for a funnel slice path
                    PathGeometry leftRightBorderPathGeometry = new PathGeometry();

                    // LeftLine Border
                    PathFigure leftBorderPathFigure = new PathFigure() { StartPoint = pathFigure.StartPoint };
                    LineSegment leftBorderLineSegment = new LineSegment() { Point = new Point(topRadius - funnelSlice.BottomRadius, funnelSlice.Height) };
                    leftBorderPathFigure.Segments.Add(leftBorderLineSegment);

                    leftRightBorderPathGeometry.Figures.Add(leftBorderPathFigure);

                    // RightLine Border
                    PathGeometry rightRightBorderPathGeometry = new PathGeometry();
                    PathFigure rightBorderPathFigure = new PathFigure() { StartPoint = new Point(topRadius + funnelSlice.TopRadius, 0) };
                    LineSegment rightBorderLineSegment = new LineSegment() { Point = new Point(topRadius + funnelSlice.BottomRadius, funnelSlice.Height) };
                    rightBorderPathFigure.Segments.Add(rightBorderLineSegment);

                    rightRightBorderPathGeometry.Figures.Add(rightBorderPathFigure);

                    // Bottom _axisIndicatorBorderElement Ellipse
                    EllipseGeometry ellipseGeometryBorder = new EllipseGeometry();
                    ellipseGeometryBorder.Center = new Point(topRadius, funnelSlice.Height);
                    ellipseGeometryBorder.RadiusX = funnelSlice.BottomRadius;
                    ellipseGeometryBorder.RadiusY = yScaleBottom / 2;

                    borderGeometryGroup.Children.Add(ellipseGeometryBorder);

                    // Bottom _axisIndicatorBorderElement Ellipse
                    ellipseGeometryBorder = new EllipseGeometry();
                    ellipseGeometryBorder.Center = new Point(topRadius, 0);
                    ellipseGeometryBorder.RadiusX = funnelSlice.TopRadius;
                    ellipseGeometryBorder.RadiusY = yScaleTop / 2;

                    borderGeometryGroup.Children.Add(ellipseGeometryBorder);
                    borderGeometryGroup.Children.Add(leftRightBorderPathGeometry);
                    borderGeometryGroup.Children.Add(rightRightBorderPathGeometry);

                    Path borderPath = new Path() { Data = borderGeometryGroup, IsHitTestVisible = false };
                    borderPath.SetValue(Canvas.ZIndexProperty, (Int32)(-1));

                    visual.Children.Add(borderPath);
                    faces.BorderElements.Add(borderPath);
                    faces.BorderElements.Add(funnelTopEllipse);

                    #endregion
                    faces.Parts.Add(path4Slice);
                    faces.Parts.Add(funnelTopEllipse);
                    funnelSlice.DataPoint.Faces = faces;
                }
                else
                {
                    if (funnelSlice.FillType == FillType.Solid && funnelSlice.Index == 0)
                    {
                        path4Slice.SetValue(Canvas.ZIndexProperty, 1);
                        path4Slice.SetValue(Canvas.TopProperty, (-SOLID_FUNNEL_EDGE_THICKNESS));
                        funnelSlice.Height -= SOLID_FUNNEL_EDGE_THICKNESS;
                    }

                    path4Slice.IsHitTestVisible = false;
                    funnelTopEllipse.IsHitTestVisible = false;
                    funnelSlice.DataPoint.Faces.Parts.Add(path4Slice);
                    funnelSlice.DataPoint.Faces.Parts.Add(funnelTopEllipse);
                    funnelSlice.DataPoint.Faces.VisualComponents.Add(path4Slice);
                    funnelSlice.DataPoint.Faces.VisualComponents.Add(funnelTopEllipse);
                    (path4Slice.Tag as ElementData).VisualElementName = "Lighting";
                    (funnelTopEllipse.Tag as ElementData).VisualElementName = "FunnelTopLighting";
                }

                // Apply animation for the 3D funnel slice
                if (animationEnabled)
                {
                    funnelSlice.DataPoint.Parent.Storyboard = AnimationHelper.ApplyOpacityAnimation(funnelTopEllipse, funnelSlice.DataPoint.Parent, funnelSlice.DataPoint.Parent.Storyboard, 0, funnelSlice.DataPoint.InternalOpacity, 0, 1);
                    funnelSlice.DataPoint.Parent.Storyboard = AnimationHelper.ApplyOpacityAnimation(path4Slice, funnelSlice.DataPoint.Parent, funnelSlice.DataPoint.Parent.Storyboard, 0, funnelSlice.DataPoint.InternalOpacity, 0, 1);
                }

                #endregion
            }
            else
            {
                // Points of a 2D funnel slice
                Point[] funnelCornerPoints = new Point[8];

                // Top line
                LineSegment lineSegment = new LineSegment() { Point = new Point(topRadius - funnelSlice.BottomRadius, funnelSlice.Height) };
                pathFigure.Segments.Add(lineSegment);
                funnelCornerPoints[3] = lineSegment.Point;

                // Right line
                lineSegment = new LineSegment() { Point = new Point(topRadius + funnelSlice.BottomRadius, funnelSlice.Height) };
                pathFigure.Segments.Add(lineSegment);
                funnelCornerPoints[2] = lineSegment.Point;

                // Bottom line
                lineSegment = new LineSegment() { Point = new Point(topRadius + funnelSlice.TopRadius, 0) };
                pathFigure.Segments.Add(lineSegment);
                funnelCornerPoints[1] = lineSegment.Point;

                // Left line
                lineSegment = new LineSegment() { Point = new Point(topRadius - funnelSlice.TopRadius, 0) };
                pathFigure.Segments.Add(lineSegment);
                funnelCornerPoints[0] = lineSegment.Point;

                // Apply animation for the 2D funnel slice
                if (animationEnabled)
                    funnelSlice.DataPoint.Parent.Storyboard = AnimationHelper.ApplyOpacityAnimation(path4Slice, funnelSlice.DataPoint.Parent, funnelSlice.DataPoint.Parent.Storyboard, 0, funnelSlice.DataPoint.InternalOpacity, 0, 1);

                if (!isLightingGradientLayer)
                {
                    // Update faces for the DataPoint
                    faces = new Faces();
                    faces.VisualComponents.Add(path4Slice);

                    (path4Slice.Tag as ElementData).VisualElementName = "FunnelBase";
                    faces.Parts.Add(path4Slice);

                    faces.BorderElements.Add(path4Slice);
                    funnelSlice.DataPoint.Faces = faces;

                    // Apply bevel effect for the 2D funnel Slice
                    if (funnelSlice.DataPoint.Parent.Bevel)
                    {
                        ApplyFunnelBevel(visual, funnelSlice, sideFillColor, funnelCornerPoints);
                    }
                }
                else
                {
                    path4Slice.IsHitTestVisible = false;
                    funnelSlice.DataPoint.Faces.VisualComponents.Add(path4Slice);
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
                Canvas labelLineCanvas = CreateLabelLine(funnelSlice, topRadius, animationEnabled);

                if (labelLineCanvas != null)
                {
                    sliceCanvas.Children.Add(labelLineCanvas);
                    faces.VisualComponents.Add(labelLineCanvas);
                }

                // Add label visual to the visual
                if ((Boolean)funnelSlice.DataPoint.LabelEnabled)
                {
                    Canvas labelCanvas = new Canvas();
                    labelCanvas.SetValue(Canvas.ZIndexProperty, (Int32)10);

                    faces.VisualComponents.Add(funnelSlice.DataPoint.LabelVisual);

                    // Label placement
                    funnelSlice.DataPoint.LabelVisual.SetValue(Canvas.TopProperty, funnelSlice.LabelLineEndPoint.Y - funnelSlice.DataPoint.LabelVisual.Height / 2);

                    if (funnelSlice.DataPoint.LabelStyle == LabelStyles.OutSide)
                    {
                        funnelSlice.DataPoint.LabelVisual.SetValue(Canvas.TopProperty, funnelSlice.LabelLineEndPoint.Y - funnelSlice.DataPoint.LabelVisual.Height / 2);
                        funnelSlice.DataPoint.LabelVisual.SetValue(Canvas.LeftProperty, funnelSlice.LabelLineEndPoint.X);
                    }
                    else
                    {
                        funnelSlice.DataPoint.LabelVisual.SetValue(Canvas.TopProperty, funnelSlice.LabelLineEndPoint.Y - funnelSlice.DataPoint.LabelVisual.Height / 2 + (is3D ? yScaleTop / 2 : 0));
                        funnelSlice.DataPoint.LabelVisual.SetValue(Canvas.LeftProperty, topRadius - funnelSlice.DataPoint.LabelVisual.Width / 2);
                    }

                    if (animationEnabled)
                        funnelSlice.DataPoint.Parent.Storyboard = AnimationHelper.ApplyOpacityAnimation(funnelSlice.DataPoint.LabelVisual, funnelSlice.DataPoint.Parent, funnelSlice.DataPoint.Parent.Storyboard, 1.2, 0.5, 0, 1);

                    labelCanvas.Children.Add(funnelSlice.DataPoint.LabelVisual);
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
        /// Creates a LabelLine for Funnel Chart
        /// </summary>
        /// <param name="funnelSlice">FunnelSliceParms</param>
        /// <param name="topRadius">Top most radius of the funnel</param>
        /// <param name="animationEnabled">Whether animation is enabled</param>
        /// <returns>Canvas for labelline </returns>
        private static Canvas CreateLabelLine(FunnelSliceParms funnelSlice, Double topRadius, Boolean animationEnabled)
        {
            Canvas labelLineCanvas = null;
            Point topRightPoint = new Point(topRadius + funnelSlice.TopRadius, 0);
            Point bottomRightPoint = new Point(topRadius + funnelSlice.BottomRadius, funnelSlice.Height);
            funnelSlice.RightMidPoint = Graphics.MidPointOfALine(topRightPoint, bottomRightPoint);

            if (funnelSlice.DataPoint.Parent.RenderAs == RenderAs.StreamLineFunnel)
                funnelSlice.LabelLineEndPoint = new Point(2 * topRadius, (bottomRightPoint.Y - 1.5 * Chart.BEVEL_DEPTH) < 0 ? bottomRightPoint.Y * .9 : (bottomRightPoint.Y - 1.5 * Chart.BEVEL_DEPTH));
            else
                funnelSlice.LabelLineEndPoint = new Point(2 * topRadius, funnelSlice.RightMidPoint.Y);

            if ((Boolean)funnelSlice.DataPoint.LabelLineEnabled && funnelSlice.DataPoint.LabelStyle == LabelStyles.OutSide)
            {
                labelLineCanvas = new Canvas();

                labelLineCanvas.Width = topRadius * 2;
                labelLineCanvas.Height = funnelSlice.Height;

                funnelSlice.DataPoint.LabelLine = null;

                Path line = new Path()
                {
                    Stroke = funnelSlice.DataPoint.LabelLineColor,
                    Fill = funnelSlice.DataPoint.LabelLineColor,
                    StrokeDashArray = ExtendedGraphics.GetDashArray((LineStyles)funnelSlice.DataPoint.LabelLineStyle),
                    StrokeThickness = (Double)funnelSlice.DataPoint.LabelLineThickness,
                };

                PathGeometry linePathGeometry = new PathGeometry();

                // Set first point of the line
                PathFigure linePathFigure = new PathFigure()
                {
                    StartPoint = (funnelSlice.DataPoint.Parent.RenderAs == RenderAs.StreamLineFunnel) ? bottomRightPoint : funnelSlice.RightMidPoint
                };

                // Set second point of line
                linePathFigure.Segments.Add(new LineSegment() { Point = funnelSlice.LabelLineEndPoint });

                linePathGeometry.Figures.Add(linePathFigure);

                line.Data = linePathGeometry;

                funnelSlice.DataPoint.LabelLine = line;

                labelLineCanvas.Children.Add(line);

                if (animationEnabled)
                    funnelSlice.DataPoint.Parent.Storyboard = ApplyLabeLineAnimation(labelLineCanvas, funnelSlice.DataPoint.Parent, funnelSlice.DataPoint.Parent.Storyboard);
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
        /// <param name="funnelSlice">funnelSlice</param>
        /// <param name="points">Array of points</param>
        private static void CalculateBevelInnerPoints(FunnelSliceParms funnelSlice, Point[] points)
        {
            Double a, b, h = Chart.BEVEL_DEPTH;

            a = h * Math.Sin(funnelSlice.TopAngle / 2);
            b = h * Math.Cos(funnelSlice.TopAngle / 2);

            points[4] = new Point(points[0].X + b, a);
            points[5] = new Point(points[1].X - b, a);

            // a = h * Math.Cos(funnelSlice.BottomAngle / 2);
            b = h * Math.Sin(funnelSlice.TopAngle / 2);

            points[6] = new Point(points[2].X - a, points[2].Y - b);
            points[7] = new Point(points[3].X + a, points[3].Y - b);
        }

        /// <summary>
        /// Get side lighting brush for the funnel slice
        /// </summary>
        /// <returns></returns>
        private static Brush GetSideLightingBrush(FunnelSliceParms funnelSlice)
        {

             //LinearGradientBrush gb = new LinearGradientBrush() { EndPoint = new Point(1.016, 0.558), StartPoint = new Point(0.075, 0.708) };
             //gb.GradientStops.Add(new GradientStop() { Color = Color.FromArgb(0, 0, 0, 0), Offset = 0.491 });
             //gb.GradientStops.Add(new GradientStop() { Color = Color.FromArgb(135, 8, 8, 8), Offset = 0.938 });
             //gb.GradientStops.Add(new GradientStop() { Color = Color.FromArgb(71, 71, 71, 71), Offset = 0 });

            //LinearGradientBrush gb = new LinearGradientBrush() { EndPoint = new Point(1.016, 0.558),  };
            List<Double> stops = new List<double>();
            List<Color> colors = new List<Color>();
            
            colors.Add(Color.FromArgb(0, 0, 0, 0)); stops.Add(0.491);
            colors.Add(Color.FromArgb(135, 8, 8, 8)); stops.Add(0.938);
            colors.Add(Color.FromArgb(71, 71, 71, 71)); stops.Add(0);
            //Double angle = funnelSlice.BottomAngle * 180 / Math.PI;
            Brush gb = Graphics.CreateLinearGradientBrush(0, new Point(-0.1, 0.5), new Point(1, 0.5), colors, stops);

            return gb;
        }

        /// <summary>
        /// Get top lighting brush for funnel slice
        /// </summary>
        /// <param name="fillBrush">Current brush</param>
        /// <returns>Return new Brush</returns>
        private static Brush GetTopBrush(Brush fillBrush, FunnelSliceParms funnelSlice)
        {
            if ((fillBrush as SolidColorBrush) != null)
            {
                 SolidColorBrush solidBrush = fillBrush as SolidColorBrush;
                // LinearGradientBrush gb = new LinearGradientBrush() { EndPoint = new Point(0.5, 1), StartPoint = new Point(0.5, 0) };
                // gb.GradientStops.Add(new GradientStop() { Color = Graphics.GetLighterColor(solidBrush.Color, 0.8), Offset = 1 });
                // gb.GradientStops.Add(new GradientStop() { Color = Graphics.GetLighterColor(solidBrush.Color, 1), Offset = 0 });

                Double r = ((double)solidBrush.Color.R / (double)255) * 0.9999;
                Double g = ((double)solidBrush.Color.G / (double)255) * 0.9999;
                Double b = ((double)solidBrush.Color.B / (double)255) * 0.9999;

                /*-----------Solid---------
                 * 
                SolidColorBrush solidBrush = fillBrush as SolidColorBrush;
                // List<Color> colors = new List<Color>();
                // List<Double> stops = new List<Double>();
                */

                LinearGradientBrush gb = null;

                if (funnelSlice.FillType == FillType.Solid)
                {
                    gb = new LinearGradientBrush() { StartPoint = new Point(0, 0), EndPoint = new Point(1, 1) };
                    gb.GradientStops.Add(new GradientStop() { Color = Graphics.GetLighterColor(solidBrush.Color, 1 - r, 1 - g, 1 - b), Offset = 0 });
                    gb.GradientStops.Add(new GradientStop() { Color = solidBrush.Color, Offset = 0.9 });
                    gb.GradientStops.Add(new GradientStop() { Color = Graphics.GetDarkerColor(solidBrush.Color, 1), Offset = 0.99 });
                    gb.Opacity = 1;

                }
                else if (funnelSlice.FillType == FillType.Hollow)
                {   
                    gb = new LinearGradientBrush()
                    {
                        StartPoint = new Point(0.233, 0.297),
                        EndPoint = new Point(0.757, 0.495)
                    };

                    gb.GradientStops.Add(new GradientStop() { Offset = 0, Color = Graphics.GetDarkerColor(solidBrush.Color, 0.5) });
                    gb.GradientStops.Add(new GradientStop() { Offset = 0.70, Color = solidBrush.Color });
                   // gb.GradientStops.Add(new GradientStop() { Offset = 0.495, Color = Colors.White });
                    gb.GradientStops.Add(new GradientStop() { Offset = 1, Color = Graphics.GetDarkerColor(solidBrush.Color, 0.8)});
                    // gb.Opacity = 0.5;


                    //gb.GradientStops.Add(new GradientStop() { Offset = 0, Color = Graphics.GetDarkerColor(solidBrush.Color, 0.5) });
                    //gb.GradientStops.Add(new GradientStop() { Offset = 0.60, Color = Graphics.GetLighterColor(solidBrush.Color, 1 - r, 1 - g, 1 - b) });
                    //// gb.GradientStops.Add(new GradientStop() { Offset = 0.495, Color = Colors.White });
                    //gb.GradientStops.Add(new GradientStop() { Offset = 1, Color = Graphics.GetDarkerColor(solidBrush.Color, 0.8) });
                   
                    //gb = new LinearGradientBrush() { StartPoint = new Point(0.2, 1.6), EndPoint = new Point(0.7, 0) };
                    //gb.GradientStops.Add(new GradientStop() { Color = Graphics.GetLighterColor(solidBrush.Color, 1 - r, 1 - g, 1 - b), Offset = 0 });
                    //gb.GradientStops.Add(new GradientStop() { Color = solidBrush.Color, Offset = 0.9 });
                    //gb.GradientStops.Add(new GradientStop() { Color = Graphics.GetLighterColor(solidBrush.Color, 0.5), Offset = 0.99 });
                    //gb.Opacity = 1;
                }

                return gb;
            }
            else
                return fillBrush;
        }

        private static Double _singleGap = 0;// Single gap height

        private static Double _totalGap = 0;// Total height used for introducing gap among funnel slice
        
        /// <summary>
        /// Size of the parent title of the StreamLine funnel Chart
        /// </summary>
        private static Size _streamLineParentTitleSize;

        /// <summary>
        /// Gap between Label and top face of the StreamLineFunnel
        /// </summary>
        private const Double TITLE_FUNNEL_GAP = 10;
    }

    /// <summary>
    /// Visifire.Charts.FunnelSliceParms
    /// </summary>
    internal class FunnelSliceParms
    {
        public Int32 Index;

        /// <summary>
        /// DataPoint reference
        /// </summary>
        public DataPoint DataPoint;

        /// <summary>
        /// Height of the funnel slice
        /// </summary>
        public Double Height;

        /// <summary>
        /// Top radius of the funnel slice
        /// </summary>
        public Double TopRadius;

        /// <summary>
        /// Bottom radius of the funnel slice
        /// </summary>
        public Double BottomRadius;

        /// <summary>
        /// Top angle for the funnel slice 
        /// </summary>
        public Double TopAngle;

        /// <summary>
        /// Bottom angle for funnel slice
        /// </summary>
        public Double BottomAngle;

        /// <summary>
        /// Top position of the funnel slice canvas
        /// </summary>
        public Double Top;

        /// <summary>
        /// Top gap for the funnel slice canvas
        /// </summary>
        public Double TopGap;

        /// <summary>
        /// Bottom gap of the funnel slice canvas
        /// </summary>
        public Double BottomGap;

        /// <summary>
        /// Right mid point of the funnel slice
        /// </summary>
        public Point RightMidPoint;

        /// <summary>
        /// Left mid point of the funnel slice
        /// </summary>
        public Point LeftMidPoint;

        /// <summary>
        /// End point of the label line
        /// </summary>
        public Point LabelLineEndPoint;

        /// <summary>
        /// Fill types
        /// </summary>
        public FillType FillType;

        /// <summary>
        /// Holds the DataPoint visual position if a other DataPoints (funnel slices)
        /// </summary>
        public System.Collections.Generic.List<Point> ExplodedPoints
        {
            get;
            set;
        }
    }

    /// <summary>
    /// IOValuePair fro Streamline Funnel slice
    /// </summary>
    internal struct IOValuePair
    {
        public Double InputValue;
        public Double OutPutValue;
    }
}
