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
        private static DataSeries currentDataSeries
        {
            get;
            set;
        }

        #endregion

        #region Private Delegates

        #endregion

        #region Private Methods

        /// <summary>
        /// Calculate auto placement for DataPoint label
        /// </summary>
        /// <param name="isView3D"></param>
        /// <param name="dataPoint"></param>
        /// <param name="barParams"></param>
        /// <param name="labelStyle"></param>
        /// <param name="labelLeft"></param>
        /// <param name="labelTop"></param>
        /// <param name="angle"></param>
        /// <param name="canvasLeft"></param>
        /// <param name="canvasTop"></param>
        /// <param name="canvasRight"></param>
        /// <param name="isVertical"></param>
        /// <param name="insideGap"></param>
        /// <param name="outsideGap"></param>
        /// <param name="tb"></param>
        private static void CalculateAutoPlacement(Boolean isView3D, DataPoint dataPoint, Size barVisualSize, Boolean isPositive,
            LabelStyles labelStyle, ref Double labelLeft, ref Double labelTop, ref Double angle, Double canvasLeft,
            Double canvasTop, Double canvasRight, Boolean isVertical, Double insideGap, Double outsideGap, Title tb)
        {
            if (isPositive)
            {
                if (labelStyle == LabelStyles.Inside)
                {
                    if (barVisualSize.Width - insideGap - (dataPoint.MarkerSize / 2 * dataPoint.MarkerScale) >= tb.TextBlockDesiredSize.Width)
                    {
                        labelLeft = canvasRight - tb.TextBlockDesiredSize.Width - (Double)(dataPoint.MarkerSize / 2 * dataPoint.MarkerScale) - insideGap;
                        labelTop = canvasTop + (barVisualSize.Height - tb.TextBlockDesiredSize.Height) / 2 + 6;
                    }
                    else
                    {
                        labelLeft = canvasLeft + insideGap;
                        labelTop = canvasTop + (barVisualSize.Height - tb.TextBlockDesiredSize.Height) / 2 + 6;
                    }
                }
                else
                {
                    labelLeft = canvasRight + (Double)(dataPoint.MarkerSize / 2 * dataPoint.MarkerScale) + outsideGap;
                    labelTop = canvasTop + (barVisualSize.Height - tb.TextBlockDesiredSize.Height) / 2 + 6;
                }
            }
            else
            {
                if (labelStyle == LabelStyles.Inside)
                {
                    if (barVisualSize.Width - insideGap - (dataPoint.MarkerSize / 2 * dataPoint.MarkerScale) >= tb.TextBlockDesiredSize.Width)
                    {
                        labelLeft = canvasLeft + (Double)(dataPoint.MarkerSize / 2 * dataPoint.MarkerScale + insideGap);
                        labelTop = canvasTop + (barVisualSize.Height - tb.TextBlockDesiredSize.Height) / 2 + 6;
                    }
                    else
                    {
                        labelLeft = canvasRight - tb.TextBlockDesiredSize.Width - insideGap;
                        labelTop = canvasTop + (barVisualSize.Height - tb.TextBlockDesiredSize.Height) / 2 + 6;
                    }
                }
                else
                {
                    labelLeft = canvasLeft - tb.TextBlockDesiredSize.Width - (Double)(dataPoint.MarkerSize / 2 * dataPoint.MarkerScale + outsideGap);
                    labelTop = canvasTop + (barVisualSize.Height - tb.TextBlockDesiredSize.Height) / 2 + 6;
                }
            }
        }

        /// <summary>
        /// Returns label for DataPoint
        /// </summary>
        /// <param name="chart"></param>
        /// <param name="barParams"></param>
        /// <param name="dataPoint"></param>
        /// <param name="canvasLeft"></param>
        /// <param name="canvasTop"></param>
        /// <param name="canvasRight"></param>
        /// <returns></returns>
        private static void CreateLabel(Chart chart, Size barVisualSize, Boolean isPositive, Boolean isTopOfStack, DataPoint dataPoint,
            Double canvasLeft, Double canvasTop, Double canvasRight, Canvas labelCanvas)
        {
            if (dataPoint.Faces == null)
                return;

            LabelStyles autoLabelStyle = (LabelStyles)dataPoint.LabelStyle;

            if (isPositive || dataPoint.YValue == 0)
                isPositive = true;

            // Calculate proper position for Canvas top
            canvasTop -= 7;

            Double angle = 0;

            if ((Boolean)dataPoint.LabelEnabled && !String.IsNullOrEmpty(dataPoint.LabelText))
            {
                Title tb = new Title()
                {
                    Text = dataPoint.TextParser(dataPoint.LabelText),
                    InternalFontFamily = dataPoint.LabelFontFamily,
                    InternalFontSize = dataPoint.LabelFontSize.Value,
                    InternalFontWeight = (FontWeight)dataPoint.LabelFontWeight,
                    InternalFontStyle = (FontStyle)dataPoint.LabelFontStyle,
                    InternalBackground = dataPoint.LabelBackground,
                    InternalFontColor = Chart.CalculateDataPointLabelFontColor(dataPoint.Chart as Chart, dataPoint, dataPoint.LabelFontColor, autoLabelStyle),
                    Tag = new ElementData() { Element = dataPoint }

                };

                tb.CreateVisualObject(new ElementData() { Element = dataPoint });

                Double labelTop = 0;
                Double labelLeft = 0;

                Double outsideGap = (chart.View3D ? 5 : 3);
                Double insideGap = (chart.View3D ? 4 : 3);

                if (Double.IsNaN(dataPoint.LabelAngle) || dataPoint.LabelAngle == 0)
                {
                    Boolean isVertical = false;

                    if (!dataPoint.IsLabelStyleSet && !dataPoint.Parent.IsLabelStyleSet && !isTopOfStack && dataPoint.Parent.RenderAs != RenderAs.Bar)
                    {
                        autoLabelStyle = LabelStyles.Inside;
                    }

                    CalculateAutoPlacement(chart.View3D, dataPoint, barVisualSize, isPositive, autoLabelStyle, ref labelLeft, ref labelTop, ref angle,
                        canvasLeft, canvasTop, canvasRight, isVertical, insideGap, outsideGap, tb);

                    tb.Visual.SetValue(Canvas.LeftProperty, labelLeft);
                    tb.Visual.SetValue(Canvas.TopProperty, labelTop);

                    tb.Visual.RenderTransformOrigin = new Point(0, 0.5);
                    tb.Visual.RenderTransform = new RotateTransform()
                    {
                        CenterX = 0,
                        CenterY = 0,
                        Angle = angle
                    };

                    if (!dataPoint.IsLabelStyleSet && !dataPoint.Parent.IsLabelStyleSet)
                    {
                        if (isPositive)
                        {
                            if (labelLeft + tb.TextBlockDesiredSize.Width > chart.PlotArea.BorderElement.Width)
                                autoLabelStyle = LabelStyles.Inside;
                        }
                        else
                        {
                            if (labelLeft < 0)
                                autoLabelStyle = LabelStyles.Inside;
                        }
                    }

                    if (autoLabelStyle != dataPoint.LabelStyle)
                    {
                        CalculateAutoPlacement(chart.View3D, dataPoint, barVisualSize, isPositive, autoLabelStyle, ref labelLeft, ref labelTop, ref angle,
                        canvasLeft, canvasTop, canvasRight, isVertical, insideGap, outsideGap, tb);

                        tb.Visual.SetValue(Canvas.LeftProperty, labelLeft);
                        tb.Visual.SetValue(Canvas.TopProperty, labelTop);
                    }
                }
                else
                {
                    if (isPositive)
                    {
                        Point centerOfRotation = new Point(canvasRight + (((Double)dataPoint.MarkerSize / 2) * (Double)dataPoint.MarkerScale),
                            canvasTop + barVisualSize.Height / 2 + 6);

                        Double radius = 0;
                        angle = 0;
                        Double angleInRadian = 0;

                        if (autoLabelStyle == LabelStyles.OutSide)
                        {
                            if (dataPoint.LabelAngle <= 90 && dataPoint.LabelAngle >= -90)
                            {
                                angle = dataPoint.LabelAngle;
                                radius += 4;
                                angleInRadian = (Math.PI / 180) * angle;
                                SetRotation(radius, angle, angleInRadian, centerOfRotation, labelLeft, labelTop, tb);
                            }
                        }
                        else
                        {
                            centerOfRotation = new Point(canvasRight - (((Double)dataPoint.MarkerSize / 2) * (Double)dataPoint.MarkerScale),
                            canvasTop + barVisualSize.Height / 2 + 6);

                            if (dataPoint.LabelAngle > 0 && dataPoint.LabelAngle <= 90)
                            {
                                angle = dataPoint.LabelAngle - 180;
                                angleInRadian = (Math.PI / 180) * angle;
                                radius += tb.TextBlockDesiredSize.Width + 3;
                                angle = (angleInRadian - Math.PI) * (180 / Math.PI);
                                SetRotation(radius, angle, angleInRadian, centerOfRotation, labelLeft, labelTop, tb);
                            }
                            else if (dataPoint.LabelAngle >= -90 && dataPoint.LabelAngle < 0)
                            {
                                angle = dataPoint.LabelAngle - 180;
                                angleInRadian = (Math.PI / 180) * angle;
                                radius += tb.TextBlockDesiredSize.Width + 4;
                                angle = (angleInRadian - Math.PI) * (180 / Math.PI);
                                SetRotation(radius, angle, angleInRadian, centerOfRotation, labelLeft, labelTop, tb);
                            }
                        }
                    }
                    else
                    {
                        Point centerOfRotation = new Point(canvasLeft - (((Double)dataPoint.MarkerSize / 2) * (Double)dataPoint.MarkerScale),
                            canvasTop + barVisualSize.Height / 2 + 6);

                        Double radius = 0;
                        angle = 0;
                        Double angleInRadian = 0;

                        if (autoLabelStyle == LabelStyles.OutSide)
                        {
                            if (dataPoint.LabelAngle > 0 && dataPoint.LabelAngle <= 90)
                            {
                                angle = dataPoint.LabelAngle - 180;
                                angleInRadian = (Math.PI / 180) * angle;
                                radius += tb.TextBlockDesiredSize.Width + 3;
                                angle = (angleInRadian - Math.PI) * (180 / Math.PI);
                                SetRotation(radius, angle, angleInRadian, centerOfRotation, labelLeft, labelTop, tb);
                            }
                            else if (dataPoint.LabelAngle >= -90 && dataPoint.LabelAngle < 0)
                            {
                                angle = dataPoint.LabelAngle - 180;
                                angleInRadian = (Math.PI / 180) * angle;
                                radius += tb.TextBlockDesiredSize.Width + 3;
                                angle = (angleInRadian - Math.PI) * (180 / Math.PI);
                                SetRotation(radius, angle, angleInRadian, centerOfRotation, labelLeft, labelTop, tb);
                            }
                        }
                        else
                        {
                            centerOfRotation = new Point(canvasLeft + (((Double)dataPoint.MarkerSize / 2) * (Double)dataPoint.MarkerScale),
                            canvasTop + barVisualSize.Height / 2 + 6);

                            if (dataPoint.LabelAngle <= 90 && dataPoint.LabelAngle >= -90)
                            {
                                angle = dataPoint.LabelAngle;
                                radius += 3;
                                angleInRadian = (Math.PI / 180) * angle;
                                SetRotation(radius, angle, angleInRadian, centerOfRotation, labelLeft, labelTop, tb);
                            }
                        }
                    }
                }

                if (autoLabelStyle != dataPoint.LabelStyle)
                {
                    tb.TextElement.Foreground = Chart.CalculateDataPointLabelFontColor(dataPoint.Chart as Chart, dataPoint, dataPoint.LabelFontColor, (dataPoint.YValue == 0 ? LabelStyles.OutSide : autoLabelStyle));
                }

                dataPoint.LabelVisual = tb.Visual;
                labelCanvas.Children.Add(tb.Visual);
            }
        }

        /// <summary>
        /// Set rotation angle for DataPoint label if LabelAngle property is set
        /// </summary>
        /// <param name="radius"></param>
        /// <param name="angle"></param>
        /// <param name="angleInRadian"></param>
        /// <param name="centerOfRotation"></param>
        /// <param name="labelLeft"></param>
        /// <param name="labelTop"></param>
        /// <param name="textBlock"></param>
        private static void SetRotation(Double radius, Double angle, Double angleInRadian, Point centerOfRotation,
            Double labelLeft, Double labelTop, Title textBlock)
        {
            labelLeft = centerOfRotation.X + radius * Math.Cos(angleInRadian);
            labelTop = centerOfRotation.Y + radius * Math.Sin(angleInRadian);

            labelTop -= textBlock.TextBlockDesiredSize.Height / 2;

            textBlock.Visual.SetValue(Canvas.LeftProperty, labelLeft);
            textBlock.Visual.SetValue(Canvas.TopProperty, labelTop);

            textBlock.Visual.RenderTransformOrigin = new Point(0, 0.5);
            textBlock.Visual.RenderTransform = new RotateTransform()
            {
                CenterX = 0,
                CenterY = 0,
                Angle = angle
            };
        }

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

            return finalHeight;
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
        private static Int32 GetStackedBarZIndex(Double plotAreaCanvasHeight, Double left, Double top, Double width, Double height, Boolean isPositive, Int32 index)
        {
            //Double zOffset = Math.Pow(10, (Int32)(Math.Log10(width) - 1));
            //Int32 iOffset = (Int32)(left / (zOffset < 1 ? 1 : zOffset));
            //Int32 zindex = (Int32)((top) * zOffset) + iOffset;
            //if (isPositive)
            //    return zindex;
            //else
            //    return Int32.MinValue + zindex;

            Double zOffset = Math.Pow(10, (Int32)(Math.Log10(width) - 1));
            Int32 iOffset = (Int32)(left / (zOffset < 1 ? 1 : zOffset));

            Int32 zindex = 0;

            if (top < plotAreaCanvasHeight)
                zindex = (Int32)((plotAreaCanvasHeight - top) * zOffset) + iOffset;
            else
                zindex = (Int32)((height - top) * zOffset) + iOffset;

            zindex = zindex / 2;

            if (isPositive)
            {
                return zindex + index;
            }
            else
                return Int32.MinValue + zindex + index;
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

            DoubleAnimationUsingKeyFrames growAnimation = AnimationHelper.CreateDoubleAnimation(currentDataSeries, scaleTransform, "(ScaleTransform.ScaleX)", 0.5, frameTimes, values, splines);

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

            DoubleAnimationUsingKeyFrames growAnimation = AnimationHelper.CreateDoubleAnimation(currentDataSeries, scaleTransform, "(ScaleTransform.ScaleX)", begin + 0.5, frameTimes, values, splines);
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

            ColumnChart.CleanUpMarkerAndLabel(dataPoint, labelCanvas);

            if ((Boolean)dataPoint.MarkerEnabled)
            {   
                Size markerSize = new Size(dataPoint.MarkerSize.Value, dataPoint.MarkerSize.Value);
                String labelText = "";// (Boolean)dataPoint.LabelEnabled ? dataPoint.TextParser(dataPoint.LabelText) : "";

                Marker marker = ColumnChart.CreateNewMarker(dataPoint, markerSize, labelText);
                dataPoint.Marker = marker;

                if (!(Boolean)dataPoint.MarkerEnabled)
                {
                    marker.MarkerFillColor = Graphics.TRANSPARENT_BRUSH;
                    marker.BorderColor = Graphics.TRANSPARENT_BRUSH;
                }

                Point markerPosition = new Point();

                if (isPositive)
                    if (chart.View3D)
                        markerPosition = new Point(barVisual.Width, barVisual.Height / 2);
                    else
                        markerPosition = new Point(barVisual.Width, barVisual.Height / 2);
                else
                    if (chart.View3D)
                        markerPosition = new Point(0, barVisual.Height / 2);
                    else
                        markerPosition = new Point(0, barVisual.Height / 2);

                //SetMarkerPosition(chart, dataPoint, isPositive, labelText, markerSize, left, top, markerPosition);

                //marker.FontColor = Chart.CalculateDataPointLabelFontColor(chart, dataPoint, dataPoint.LabelFontColor, (dataPoint.YValue == 0) ? LabelStyles.OutSide : (LabelStyles) dataPoint.LabelStyle);

                marker.Tag = new ElementData() { Element = dataPoint };

                marker.CreateVisual();

                marker.AddToParent(labelCanvas, left + markerPosition.X, top + markerPosition.Y, new Point(0.5, 0.5));

                if (marker != null && marker.Visual != null && !chart.IndicatorEnabled)
                    dataPoint.AttachToolTip(chart, dataPoint, marker.Visual);
            }

            if ((Boolean)dataPoint.LabelEnabled)
            {
                Double right = left + barVisual.Width;
                CreateLabel(chart, new Size(barVisual.Width, barVisual.Height), isPositive, dataPoint.IsTopOfStack, dataPoint, left, top, right, labelCanvas);

                if (dataPoint.LabelVisual != null && !chart.IndicatorEnabled)
                    dataPoint.AttachToolTip(chart, dataPoint, dataPoint.LabelVisual);
            }
        }

        internal static void CreateBarDataPointVisual(DataPoint dataPoint, Canvas labelCanvas, Canvas columnCanvas , Boolean isPositive, Double heightPerBar, Double depth3d, Boolean animationEnabled)
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

            Double left, right;

            if (isPositive)
            {
                left = Graphics.ValueToPixelPosition(0, width, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, limitingYValue);
                right = Graphics.ValueToPixelPosition(0, width, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, Double.IsNaN(dataPoint.InternalYValue) ? 0 : dataPoint.InternalYValue);
               
                if (right < left)
                    right = left;
            }
            else
            {
                left = Graphics.ValueToPixelPosition(0, width, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, Double.IsNaN(dataPoint.InternalYValue) ? 0 : dataPoint.InternalYValue);
                right = Graphics.ValueToPixelPosition(0, width, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, limitingYValue);
            }
                        
            Double columnWidth = Math.Abs(left - right);

            if (columnWidth < dataPoint.Parent.MinPointHeight)
            {
                if (dataPoint.InternalYValue == 0)
                {
                    if (plotGroup.AxisY.InternalAxisMaximum <= 0)
                        left -= (dataPoint.Parent.MinPointHeight - columnWidth);
                    else
                       right += (dataPoint.Parent.MinPointHeight - columnWidth);
                }
                else if (isPositive)
                    right += (dataPoint.Parent.MinPointHeight - columnWidth);
                else
                    left -= (dataPoint.Parent.MinPointHeight - columnWidth);

                columnWidth = dataPoint.Parent.MinPointHeight;
            }

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
                columnVisual.SetValue(Canvas.ZIndexProperty, GetBarZIndex(left, top, height, dataPoint.InternalYValue > 0));

                dataPoint.Faces = column;

                if (!VisifireControl.IsMediaEffectsEnabled)
                    ColumnChart.ApplyOrRemoveShadow4XBAP(dataPoint, false, false);

            }
            else
            {   
                // column = Get2DBar(barParams);
                column = ColumnChart.Get2DColumn(dataPoint, columnWidth, columnHeight, false, false);
                columnVisual = column.Visual as Panel;

                dataPoint.Faces = column;
            }

            if (VisifireControl.IsMediaEffectsEnabled)
                ApplyOrRemoveShadow(chart, dataPoint);

            dataPoint.Faces.LabelCanvas = labelCanvas;
            dataPoint.Parent.Faces = new Faces() { Visual = columnCanvas, LabelCanvas = labelCanvas };

            columnVisual.SetValue(Canvas.LeftProperty, left);
            columnVisual.SetValue(Canvas.TopProperty, top);

            columnCanvas.Children.Add(columnVisual);

            dataPoint.IsTopOfStack = true;

            CreateOrUpdateMarker4HorizontalChart(chart, labelCanvas, dataPoint, left, top, isPositive, depth3d);

            if (isPositive)
                dataPoint._visualPosition = new Point(right, top + columnHeight / 2);
            else
                dataPoint._visualPosition = new Point(left, top + columnHeight / 2);

            dataPoint.Faces.LabelCanvas = labelCanvas;

            // Apply animation
            if (animationEnabled)
            {
                if (dataPoint.Parent.Storyboard == null)
                    dataPoint.Parent.Storyboard = new Storyboard();

                currentDataSeries = dataPoint.Parent;

                // Apply animation to the bars 
                dataPoint.Parent.Storyboard = ApplyBarChartAnimation(columnVisual, dataPoint.Parent.Storyboard, isPositive);
            }

            dataPoint.Faces.Visual.Opacity = (Double)dataPoint.Opacity * (Double)dataPoint.Parent.Opacity;
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

        internal static void ApplyOrRemoveShadow(Chart chart, DataPoint dataPoint)
        {
            Faces faces = dataPoint.Faces;

            if (faces == null)
                throw new Exception("Faces of DataPoint is null. ApplyOrRemoveShadow()");
            
            Canvas barVisual = faces.Visual as Canvas;

#if !WP
            if ((Boolean)dataPoint.ShadowEnabled)
            {
                barVisual.Effect = ExtendedGraphics.GetShadowEffect(325, 3.3, 0.95);
            }
            else
                barVisual.Effect = null;
#endif
        }

        internal static void DrawStackedBarsAtXValue(RenderAs chartType, Double xValue, PlotGroup plotGroup, Canvas columnCanvas, Canvas labelCanvas, Double drawingIndex, Double heightPerBar, Double maxBarHeight, Double limitingYValue, Double depth3d, Boolean animationEnabled)
        {   
            RectangularChartShapeParams barParams = new RectangularChartShapeParams();
            barParams.ShadowOffset = 5;
            barParams.Depth = depth3d;
            barParams.IsStacked = true;
            Boolean isTopOFStack;
            DataPoint dataPointAtTopOfStack = null;
            Int32 positiveIndex = 1, negativeIndex = 1;

            Double top = Graphics.ValueToPixelPosition(columnCanvas.Height, 0, (Double)plotGroup.AxisX.InternalAxisMinimum, (Double)plotGroup.AxisX.InternalAxisMaximum, xValue) + drawingIndex * heightPerBar - (maxBarHeight / 2);
            Double left = Graphics.ValueToPixelPosition(0, columnCanvas.Width, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, limitingYValue);

            Double columnHeight = CalculateHeightOfEachColumn(ref top, heightPerBar, columnCanvas.Height);

            Double right=0;
            Double prevSum = 0;

            Double animationBeginTime = 0.4;
            Double animationTime = 1.0 / plotGroup.XWiseStackedDataList[xValue].Positive.Count;

            if (plotGroup.XWiseStackedDataList[xValue].Positive.Count > 0)
                dataPointAtTopOfStack = plotGroup.XWiseStackedDataList[xValue].Positive.Last();

            Double absoluteSum = Double.NaN;
            
            if (chartType == RenderAs.StackedBar100)
                absoluteSum = plotGroup.XWiseStackedDataList[xValue].AbsoluteYValueSum;

            // Plot positive values
            foreach (DataPoint dataPoint in plotGroup.XWiseStackedDataList[xValue].Positive)
            {
                dataPoint.Parent.Faces = new Faces { Visual = columnCanvas, LabelCanvas = labelCanvas };

                if (!(Boolean)dataPoint.Enabled || Double.IsNaN(dataPoint.InternalYValue))
                {
                    ColumnChart.CleanUpMarkerAndLabel(dataPoint, labelCanvas);
                    continue;
                }

                isTopOFStack = (dataPoint == dataPointAtTopOfStack);

                 CreateStackedBarVisual(dataPoint.Parent.RenderAs, dataPoint.InternalYValue >= 0, columnCanvas, labelCanvas, dataPoint,
                     top, ref left, ref right, columnHeight, ref prevSum, absoluteSum, depth3d, animationEnabled,
                     animationBeginTime, isTopOFStack, positiveIndex, plotGroup.XWiseStackedDataList[xValue].Positive.ToList());

                 animationBeginTime += animationTime;
                 positiveIndex++;
            }

            prevSum = 0;
            right = Graphics.ValueToPixelPosition(0, columnCanvas.Width, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, limitingYValue);

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
                dataPoint.Parent.Faces = new Faces { Visual = columnCanvas, LabelCanvas = labelCanvas };

                if (!(Boolean)dataPoint.Enabled || Double.IsNaN(dataPoint.InternalYValue))
                    continue;

                isTopOFStack = (dataPoint == dataPointAtTopOfStack);

                CreateStackedBarVisual(dataPoint.Parent.RenderAs, dataPoint.InternalYValue >= 0, columnCanvas, labelCanvas, dataPoint, 
                    top, ref left, ref right, columnHeight, ref prevSum, absoluteSum, depth3d, animationEnabled, 
                    animationBeginTime, isTopOFStack, negativeIndex, plotGroup.XWiseStackedDataList[xValue].Negative.ToList());

                animationBeginTime += animationTime;
                negativeIndex--;
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

            DataSeries currentDataSeries = null;

            foreach (PlotGroup plotGroup in plotGroupList)
            {
                if (!seriesIndex.ContainsKey(plotGroup.AxisY))
                    continue;

                currentDataSeries = plotGroup.DataSeriesList[0];

                Int32 drawingIndex = seriesIndex[plotGroup.AxisY][plotGroup.AxisX];
                
                Double minDiff, heightPerBar, maxBarHeight;
                heightPerBar = ColumnChart.CalculateWidthOfEachStackedColumn(chart, plotGroup, height, out minDiff, out maxBarHeight);
                
                //List<Double> xValuesList = plotGroup.XWiseStackedDataList.Keys.ToList();
                Double[] xValuesList = RenderHelper.GetXValuesUnderViewPort(plotGroup.XWiseStackedDataList.Keys.ToList(), plotGroup.AxisX, plotGroup.AxisY, false);

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

            if (plotGroupList.Count > 0 && plotGroupList[0].XWiseStackedDataList.Keys.Count > 0)
            {
                ColumnChart.CreateOrUpdatePlank(chart, plotGroupList[0].AxisY, columnCanvas, depth3d, Orientation.Vertical);
            }

            // Apply animation
            if (animationEnabled && currentDataSeries != null)
            {
                if (currentDataSeries.Storyboard == null)
                    currentDataSeries.Storyboard = new Storyboard();

                // Apply animation to the marker and labels
                currentDataSeries.Storyboard = AnimationHelper.ApplyOpacityAnimation(labelCanvas, currentDataSeries, currentDataSeries.Storyboard, 1, 1, 0, 1);
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
            clipRectangle.Rect = new Rect(-(chart.View3D ? 0 : 5) - chart.ChartArea.PLANK_THICKNESS, -chart.ChartArea.PLANK_DEPTH, width + chart.ChartArea.PLANK_DEPTH + chart.ChartArea.PLANK_THICKNESS + (chart.View3D ? 0 : 10), height + chart.ChartArea.PLANK_DEPTH);
            visual.Clip = clipRectangle;

            // Clip Column Canvas
            PlotArea plotArea = chart.PlotArea;
            RectangleGeometry clipRetGeo = new RectangleGeometry();

            clipRetGeo.Rect = new Rect(plotArea.BorderThickness.Left - chart.ChartArea.PLANK_THICKNESS,
                    -chart.ChartArea.PLANK_DEPTH,
                    width + chart.ChartArea.PLANK_DEPTH + chart.ChartArea.PLANK_THICKNESS
                        - plotArea.BorderThickness.Left - plotArea.BorderThickness.Right
                    , height + chart.ChartArea.PLANK_DEPTH);

            columnCanvas.Clip = clipRetGeo;

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
            Double animationBeginTime, Boolean isTopOFStack, Int32 PositiveOrNegativeZIndex, List<DataPoint> listOfDataPointsInXValue)
        {   
            PlotGroup plotGroup = dataPoint.Parent.PlotGroup;
            Chart chart = dataPoint.Chart as Chart;

            Double percentYValue = 0;

            if (chartType == RenderAs.StackedBar100)
            {
                if (absoluteSum != 0)
                {
                    if (plotGroup.AxisY.Logarithmic)
                    {
                        percentYValue = Math.Log((dataPoint.InternalYValue / absoluteSum * 100), plotGroup.AxisY.LogarithmBase);
                    }
                    else
                        percentYValue = (dataPoint.InternalYValue / absoluteSum * 100);
                }
            }
            else
                percentYValue = dataPoint.InternalYValue;


            if (isPositive)
            {
                if(plotGroup.AxisY.Logarithmic)
                    right = ColumnChart.CalculatePositionOfDataPointForLogAxis(dataPoint, columnCanvas.Width, plotGroup, listOfDataPointsInXValue, absoluteSum);
                else
                    right = Graphics.ValueToPixelPosition(0, columnCanvas.Width, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, percentYValue + prevSum);
            }
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
                barVisual.SetValue(Canvas.ZIndexProperty, GetStackedBarZIndex(chart.ChartArea.PlotAreaCanvas.Height, left, top, columnCanvas.Width, columnCanvas.Height, (dataPoint.InternalYValue > 0), PositiveOrNegativeZIndex));

                dataPoint.Faces = bar;
                if (!VisifireControl.IsMediaEffectsEnabled)
                    ColumnChart.ApplyOrRemoveShadow4XBAP(dataPoint, true, false);
            }
            else
            {
                bar = ColumnChart.Get2DColumn(dataPoint, barWidth, finalHeight, true, isTopOFStack);
                barVisual = bar.Visual as Panel;
                dataPoint.Faces = bar;
            }

            if (VisifireControl.IsMediaEffectsEnabled)
                ApplyOrRemoveShadow(chart, dataPoint);

            dataPoint.Faces.LabelCanvas = labelCanvas;

            barVisual.SetValue(Canvas.LeftProperty, left);
            barVisual.SetValue(Canvas.TopProperty, top);

            columnCanvas.Children.Add(barVisual);

            dataPoint.IsTopOfStack = isTopOFStack;

            CreateOrUpdateMarker4HorizontalChart(dataPoint.Chart as Chart, labelCanvas, dataPoint, left, top, isPositive, depth3d);

            //labelCanvas.Children.Add(CreateMarker(chart, barParams, dataPoint, left, top));

            // Apply animation
            if (animationEnabled)
            {
                if (dataPoint.Parent.Storyboard == null)
                    dataPoint.Parent.Storyboard = new Storyboard();

                currentDataSeries = dataPoint.Parent;

                // Apply animation to the data points dataSeriesIndex.e to the rectangles that form the columns
                dataPoint.Parent.Storyboard = ApplyStackedBarChartAnimation(barVisual, dataPoint.Parent.Storyboard, animationBeginTime, 0.5);
            }

            if (isPositive)
                left = right;
            else
                right = left;

            if (isPositive)
                dataPoint._visualPosition = new Point(right, top + finalHeight / 2);
            else
                dataPoint._visualPosition = new Point(left, top + finalHeight / 2);

            dataPoint.Faces.Visual.Opacity = (Double)dataPoint.Opacity * (Double)dataPoint.Parent.Opacity;
            dataPoint.AttachEvent2DataPointVisualFaces(dataPoint);
            dataPoint.AttachEvent2DataPointVisualFaces(dataPoint.Parent);
            dataPoint._parsedToolTipText = dataPoint.TextParser(dataPoint.ToolTipText);
            
            if(!chart.IndicatorEnabled)
                dataPoint.AttachToolTip(chart, dataPoint, dataPoint.Faces.Visual);
            dataPoint.AttachHref(chart, dataPoint.Faces.Visual, dataPoint.Href, (HrefTargets)dataPoint.HrefTarget);
            
            dataPoint.SetCursor2DataPointVisualFaces();
        }
        
        /// <summary>
        /// Create 3D bar for a DataPoint
        /// </summary>
        /// <param name="barParams">Bar parameters</param>
        /// <returns>Faces for bar</returns>
        internal static Faces Get3DBar(RectangularChartShapeParams barParams)
        {
            Faces faces = new Faces();

            Canvas barVisual = new Canvas();

            barVisual.Width = barParams.Size.Width;
            barVisual.Height = barParams.Size.Height;

            Brush frontBrush = barParams.Lighting ? Graphics.GetFrontFaceBrush(barParams.BackgroundBrush) : barParams.BackgroundBrush;
            Brush topBrush = barParams.Lighting ? Graphics.GetTopFaceBrush(barParams.BackgroundBrush) : barParams.BackgroundBrush;
            Brush rightBrush = barParams.Lighting ? Graphics.GetRightFaceBrush(barParams.BackgroundBrush) : barParams.BackgroundBrush;

            Path front = ExtendedGraphics.Get2DRectangle(barParams.TagReference, barParams.Size.Width, barParams.Size.Height,
                barParams.BorderThickness, barParams.BorderStyle, barParams.BorderBrush,
                frontBrush, new CornerRadius(0), new CornerRadius(0), false);

            faces.Parts.Add(front);
            faces.BorderElements.Add(front);

            Path top = ExtendedGraphics.Get2DRectangle(barParams.TagReference, barParams.Size.Width, barParams.Depth,
                barParams.BorderThickness, barParams.BorderStyle, barParams.BorderBrush,
                topBrush, new CornerRadius(0), new CornerRadius(0), false);

            faces.Parts.Add(top);
            faces.BorderElements.Add(top);

            top.RenderTransformOrigin = new Point(0, 1);
            SkewTransform skewTransTop = new SkewTransform();
            skewTransTop.AngleX = -45;
            top.RenderTransform = skewTransTop;

            Path right = ExtendedGraphics.Get2DRectangle(barParams.TagReference, barParams.Depth, barParams.Size.Height,
                barParams.BorderThickness, barParams.BorderStyle, barParams.BorderBrush,
                rightBrush, new CornerRadius(0), new CornerRadius(0), false);

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

            Grid barVisual = new Grid();

            barVisual.Width = barParams.Size.Width;
            barVisual.Height = barParams.Size.Height;

            Brush background = (barParams.Lighting ? Graphics.GetLightingEnabledBrush(barParams.BackgroundBrush, "Linear", null) : barParams.BackgroundBrush);

            Path barBase = ExtendedGraphics.Get2DRectangle(barParams.TagReference, barParams.Size.Width, barParams.Size.Height,
                barParams.BorderThickness, barParams.BorderStyle, barParams.BorderBrush,
                background, barParams.XRadius, barParams.YRadius, true);

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

