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
using System.Windows.Shapes;

namespace Visifire.Charts
{
    /// <summary>
    /// Visifire.Charts.CandleStick class
    /// </summary>
    internal class CandleStick
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

        #endregion

        #region Internal Methods

        /// <summary>
        /// Calculate DataPoint width
        /// </summary>
        /// <param name="width">PlotCanvas width</param>
        /// <param name="height">PlotCanvas height</param>
        /// <param name="chart">Chart reference</param>
        /// <returns>DataPointWidth as Double</returns>
       internal static Double CalculateDataPointWidth(Double width, Double height, Chart chart)
        {
            Double dataPointWidth;

            Double minDiffValue = chart.PlotDetails.GetMinOfMinDifferencesForXValue(RenderAs.Column, RenderAs.StackedColumn, RenderAs.StackedColumn100, RenderAs.Stock, RenderAs.CandleStick);
            
            if (double.IsPositiveInfinity(minDiffValue))
                minDiffValue = 0;

            if (Double.IsNaN(chart.DataPointWidth) || chart.DataPointWidth < 0)
            {
                if (minDiffValue != 0)
                {   
                    dataPointWidth = Graphics.ValueToPixelPosition(0, width, (Double)chart.AxesX[0].InternalAxisMinimum, (Double)chart.AxesX[0].InternalAxisMaximum, minDiffValue + (Double)chart.AxesX[0].InternalAxisMinimum);
                    dataPointWidth *= .9;

                    if (dataPointWidth < 5)
                        dataPointWidth = 5;
                }
                else
                {   
                    dataPointWidth = width * .3;
                }
            }
            else
            {
                dataPointWidth = chart.PlotArea.Width / 100 * chart.DataPointWidth;
            }

            if (dataPointWidth < 2)
                dataPointWidth = 2;

            return dataPointWidth;
        }

        /// <summary>
       /// Get StrokeThickness
        /// </summary>
        /// <param name="dataPoint"></param>
        /// <returns></returns>
        internal static Double GetStrokeThickness(DataPoint dataPoint)
        {
            return dataPoint.InternalBorderThickness.Left == 0 ? ((Double)dataPoint.Parent.LineThickness >= _dataPointWidth / 2 ? _dataPointWidth / 4 : (Double)dataPoint.Parent.LineThickness) : dataPoint.InternalBorderThickness.Left;
        }

        /// <summary>
        /// Set DataPoint Values
        /// </summary>
        /// <param name="dataPoint"></param>
        /// <param name="highY"></param>
        /// <param name="lowY"></param>
        /// <param name="openY"></param>
        /// <param name="closeY"></param>
        internal static void SetDataPointValues(DataPoint dataPoint, ref Double highY, ref Double lowY, ref Double openY, ref Double closeY)
        {
            highY = lowY = openY = closeY = 0;

            if (dataPoint.YValues.Length >= 4)
            {
                openY = dataPoint.YValues[0];
                closeY = dataPoint.YValues[1];
                highY = dataPoint.YValues[2];
                lowY = dataPoint.YValues[3];
            }
            else if (dataPoint.YValues.Length >= 3)
            {
                openY = dataPoint.YValues[0];
                closeY = dataPoint.YValues[1];
                highY = dataPoint.YValues[2];
            }
            else if (dataPoint.YValues.Length >= 2)
            {
                openY = dataPoint.YValues[0];
                closeY = dataPoint.YValues[1];
            }
            else if (dataPoint.YValues.Length >= 1)
            {
                openY = dataPoint.YValues[0];
            }
        }

        private static Brush GetOpenCloseRectangleBorderbrush(DataPoint dataPoint, Brush dataPointColor)
        {
            return (dataPoint.BorderColor == null) ? ((dataPointColor == null) ? dataPoint.Parent.PriceUpColor : dataPointColor) : dataPoint.BorderColor;
        }

        private static Brush GetOpenCloseRectangleFillbrush(DataPoint dataPoint, Brush dataPointColor)
        {
            if (dataPoint._isPriceUp)
                return (dataPointColor == null) ? dataPoint.Parent.PriceUpColor : dataPointColor;
            else
                return dataPoint.Parent.PriceDownColor;
        }
        /// <summary>
        /// Get visual object for CandleStick chart
        /// </summary>
        /// <param name="width">Width of the chart</param>
        /// <param name="height">Height of the chart</param>
        /// <param name="plotDetails">plotDetails</param>
        /// <param name="seriesList">List of DataSeries</param>
        /// <param name="chart">Chart</param>
        /// <param name="plankDepth">Plank depth</param>
        /// <param name="animationEnabled">Whether animation is enabled</param>
        /// <returns>CandleStick chart canvas</returns>
        internal static Canvas GetVisualObjectForCandleStick(Double width, Double height, PlotDetails plotDetails, List<DataSeries> seriesList, Chart chart, Double plankDepth, bool animationEnabled)
        {
            if (Double.IsNaN(width) || Double.IsNaN(height) || width <= 0 || height <= 0) return null;

            Canvas visual = new Canvas() { Width = width, Height = height };
            Canvas labelCanvas = new Canvas() { Width = width, Height = height };

            Double depth3d = plankDepth / (plotDetails.Layer3DCount == 0 ? 1 : plotDetails.Layer3DCount) * (chart.View3D ? 1 : 0);
            Double visualOffset = depth3d * (plotDetails.SeriesDrawingIndex[seriesList[0]] + 1 - (plotDetails.Layer3DCount == 0 ? 0 : 1));
            
            visual.SetValue(Canvas.TopProperty, visualOffset);
            visual.SetValue(Canvas.LeftProperty, -visualOffset);

            Double xPositionOfDataPoint, highY = 0, lowY = 0, openY = 0, closeY = 0;
            Double animationBeginTime = 0;
            DataSeries _tempDataSeries = null;

            // Calculate width of a DataPoint 
            _dataPointWidth = CalculateDataPointWidth(width, height, chart);
            
            foreach (DataSeries series in seriesList)
            {
                if (series.Enabled == false)
                    continue;

                Canvas seriesCanvas = new Canvas();

                PlotGroup plotGroup = series.PlotGroup;
                _tempDataSeries = series;

                foreach (DataPoint dataPoint in series.InternalDataPoints)
                {
                    if (dataPoint.YValues == null || dataPoint.Enabled == false)
                        continue;

                    Brush dataPointColor = (Brush)dataPoint.GetValue(DataPoint.ColorProperty);

                    // Initialize DataPoint faces
                    dataPoint.Faces = new Faces();
                    dataPoint.Faces.Parts = new List<FrameworkElement>();
                    
                    SetDataPointValues(dataPoint, ref highY, ref lowY, ref openY, ref closeY);
                    
                    // Calculate required pixel positions
                    xPositionOfDataPoint = Graphics.ValueToPixelPosition(0, width, (Double)plotGroup.AxisX.InternalAxisMinimum, (Double)plotGroup.AxisX.InternalAxisMaximum, dataPoint.InternalXValue);
                    openY = Graphics.ValueToPixelPosition(height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, openY);
                    closeY = Graphics.ValueToPixelPosition(height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, closeY);
                    highY = Graphics.ValueToPixelPosition(height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, highY);
                    lowY = Graphics.ValueToPixelPosition(height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, lowY);

                    dataPoint._isPriceUp = (openY > closeY) ? true : false;

                    // Create DataPoint Visual
                    Canvas dataPointVisual = new Canvas() 
                    { 
                        //Opacity = 0.5, 
                        //Background = new SolidColorBrush(Colors.Yellow),
                        Width = _dataPointWidth,
                        Height = Math.Abs(lowY - highY)
                    };
                    
                    dataPointVisual.SetValue(Canvas.TopProperty, (highY < lowY) ? highY : lowY);
                    dataPointVisual.SetValue(Canvas.LeftProperty, xPositionOfDataPoint - _dataPointWidth / 2);

                    // Create High and Low Line
                    Line highLowLine = new Line() 
                    { 
                        X1 = dataPointVisual.Width / 2,
                        X2 = dataPointVisual.Width / 2,
                        Y1 = 0,
                        Y2 = dataPointVisual.Height,
                        Tag = new ElementData() { Element = dataPoint, VisualElementName = "HlLine" },
                        StrokeThickness = GetStrokeThickness(dataPoint),
                        StrokeDashArray = Graphics.LineStyleToStrokeDashArray(dataPoint.BorderStyle.ToString())
                    };

                    if ((Boolean)dataPoint.ShadowEnabled)
                    {
                        Line highLowShadowLine = new Line()
                        {
                            IsHitTestVisible = false,
                            X1 = dataPointVisual.Width / 2 + _shadowDepth,
                            X2 = dataPointVisual.Width / 2 + _shadowDepth,
                            Y1 = 0,
                            Y2 = Math.Max(dataPointVisual.Height -_shadowDepth, 1),
                            Stroke = _shadowColor,
                            StrokeThickness = GetStrokeThickness(dataPoint),
                            StrokeDashArray = Graphics.LineStyleToStrokeDashArray(dataPoint.BorderStyle.ToString())
                        };

                        highLowShadowLine.SetValue(Canvas.TopProperty, _shadowDepth);
                        dataPointVisual.Children.Add(highLowShadowLine);
                    }

                    dataPoint.Faces.Parts.Add(highLowLine);
                    dataPoint.Faces.VisualComponents.Add(highLowLine);
                    dataPointVisual.Children.Add(highLowLine);
                    
                    /* Create Open-Close Rectangle
                     * Math.Max is used to make sure that the rectangle is visible 
                     * even when the difference between high and low is 0 */
                    Rectangle openCloseRect = new Rectangle()
                    {   
                        Width = _dataPointWidth, 
                        Height = Math.Max(Math.Abs(openY - closeY), 1),
                        Stroke = GetOpenCloseRectangleBorderbrush(dataPoint, dataPointColor),
                        StrokeThickness = dataPoint.InternalBorderThickness.Left,
                        StrokeDashArray = Graphics.LineStyleToStrokeDashArray(dataPoint.BorderStyle.ToString()),
                        Tag = new ElementData() { Element = dataPoint, VisualElementName = "OcRect" }
                    };

                    openCloseRect.SetValue(Canvas.TopProperty, ((closeY > openY) ? openY : closeY) - ((Double)dataPointVisual.GetValue(Canvas.TopProperty)));
                    openCloseRect.SetValue(Canvas.LeftProperty, (Double)0);

                    // If Closing Value is higher than Opening value, then fill
                    //openCloseRect.Fill = (openY > closeY) ? 
                    //    (dataPointColor ?? dataPoint.Parent.PriceUpColor) : 
                    //    (dataPointColor ?? dataPoint.Parent.PriceDownColor);

                    openCloseRect.Fill = GetOpenCloseRectangleFillbrush(dataPoint, dataPointColor);

                    if ((Boolean)dataPoint.ShadowEnabled)
                    {
                        Rectangle openCloseShadowRect = new Rectangle()
                        {
                            IsHitTestVisible = false,
                            Fill = _shadowColor,
                            Width = _dataPointWidth,
                            Height = Math.Max(Math.Abs(openY - closeY), 1),
                            Stroke = _shadowColor,
                            StrokeThickness = dataPoint.InternalBorderThickness.Left,
                            StrokeDashArray = Graphics.LineStyleToStrokeDashArray(dataPoint.BorderStyle.ToString())
                        };
                        
                        openCloseShadowRect.SetValue(Canvas.TopProperty, (Double)openCloseRect.GetValue(Canvas.TopProperty) + _shadowDepth);
                        openCloseShadowRect.SetValue(Canvas.LeftProperty, (Double)openCloseRect.GetValue(Canvas.LeftProperty) + _shadowDepth);
                        openCloseShadowRect.SetValue(Canvas.ZIndexProperty, -4);
                        dataPointVisual.Children.Add(openCloseShadowRect);
                    }

                    dataPoint.Faces.Parts.Add(openCloseRect);
                    dataPoint.Faces.VisualComponents.Add(openCloseRect);
                    dataPoint.Faces.BorderElements.Add(openCloseRect);
                    dataPointVisual.Children.Add(openCloseRect);
                    
                    // Create Bevel
                    if (dataPoint.Parent.Bevel && _dataPointWidth > 8 && openCloseRect.Height > 6)
                    {
                        Double reduceSize = openCloseRect.StrokeThickness;

                        if (dataPoint.Parent.SelectionEnabled && dataPoint.InternalBorderThickness.Left == 0)
                            reduceSize = 1.5 + reduceSize;

                        if (openCloseRect.Width - 2 * reduceSize >= 0 && openCloseRect.Height - 2 * reduceSize >= 0)
                        {
                            Canvas bevelCanvas = ExtendedGraphics.Get2DRectangleBevel(dataPoint, openCloseRect.Width - 2 * reduceSize, openCloseRect.Height - 2 * reduceSize, 5, 5,
                               Graphics.GetBevelTopBrush(openCloseRect.Fill),
                               Graphics.GetBevelSideBrush(((Boolean)dataPoint.LightingEnabled ? -70 : 0), openCloseRect.Fill),
                               Graphics.GetBevelSideBrush(((Boolean)dataPoint.LightingEnabled ? -110 : 180), openCloseRect.Fill),
                               Graphics.GetBevelSideBrush(90, openCloseRect.Fill));

                            bevelCanvas.IsHitTestVisible = false;

                            bevelCanvas.SetValue(Canvas.TopProperty, (Double)openCloseRect.GetValue(Canvas.TopProperty) + reduceSize);
                            bevelCanvas.SetValue(Canvas.LeftProperty, reduceSize);

                            // Adding parts of bevel
                            foreach(FrameworkElement fe in bevelCanvas.Children)
                                dataPoint.Faces.Parts.Add(fe);
                            
                            dataPoint.Faces.VisualComponents.Add(bevelCanvas);
                            dataPointVisual.Children.Add(bevelCanvas);
                        }
                    }
                                        
                    Brush highLowLineColor;

                    if (dataPointColor == null)
                        highLowLineColor = dataPoint.Parent.PriceUpColor;
                    else
                        highLowLineColor = dataPointColor;

                    if ((Boolean)dataPoint.LightingEnabled)
                    {
                        openCloseRect.Fill = Graphics.GetLightingEnabledBrush(openCloseRect.Fill, "Linear", null);
                        highLowLine.Stroke = Graphics.GetLightingEnabledBrush(highLowLineColor, "Linear", null);
                    }
                    else
                        highLowLine.Stroke = highLowLineColor;

                    seriesCanvas.Children.Add(dataPointVisual);
                    dataPoint.Faces.Visual = dataPointVisual;

                    PlaceLabel(visual, labelCanvas, dataPoint);

                } // DataPoint loop End

                // Apply animation to series
                if (animationEnabled)
                {
                    if (_tempDataSeries.Storyboard == null)
                        _tempDataSeries.Storyboard = new Storyboard();

                    _tempDataSeries.Storyboard = AnimationHelper.ApplyOpacityAnimation(seriesCanvas, _tempDataSeries, _tempDataSeries.Storyboard, animationBeginTime, 1, 1);
                    animationBeginTime += 0.5;
                }

                visual.Children.Add(seriesCanvas);
            }

            // Label animation
            if (animationEnabled && _tempDataSeries != null)
                _tempDataSeries.Storyboard = AnimationHelper.ApplyOpacityAnimation(labelCanvas, _tempDataSeries, _tempDataSeries.Storyboard, animationBeginTime, 1, 1);

            visual.Children.Add(labelCanvas);

            RectangleGeometry clipRectangle = new RectangleGeometry();
            clipRectangle.Rect = new Rect(0, -chart.ChartArea.PLANK_DEPTH, width + chart.ChartArea.PLANK_OFFSET, height + chart.ChartArea.PLANK_DEPTH);
            visual.Clip = clipRectangle;

            return visual;
        }

        /// <summary>
        /// Recalculate and apply new brush
        /// </summary>
        /// <param name="shape">Shape reference</param>
        /// <param name="newBrush">New Brush</param>
        /// <param name="isLightingEnabled">Whether lighting is enabled</param>
        /// <param name="is3D">Whether 3d effevt is enabled</param>
        internal static void ReCalculateAndApplyTheNewBrush(DataPoint dataPoint, Shape shape, Brush newBrush, Boolean isLightingEnabled, Boolean is3D)
        {
            Brush oCRectfillColor = GetOpenCloseRectangleFillbrush(dataPoint, newBrush);

            switch ((shape.Tag as ElementData).VisualElementName)
            {
                case "HlLine": 
                    shape.Stroke = isLightingEnabled ? Graphics.GetLightingEnabledBrush(newBrush, "Linear", null) : Graphics.GetBevelTopBrush(newBrush); 
                    break;
                case "OcRect":
                    shape.Fill = isLightingEnabled ? Graphics.GetLightingEnabledBrush(oCRectfillColor, "Linear", null) : oCRectfillColor;
                    shape.Stroke = GetOpenCloseRectangleBorderbrush(dataPoint, newBrush);
                    break;
                case "TopBevel":
                    shape.Fill = Graphics.GetBevelTopBrush(oCRectfillColor); 
                    break;
                case "LeftBevel":
                    shape.Fill = Graphics.GetBevelSideBrush((isLightingEnabled ? -70 : 0), oCRectfillColor); 
                    break;
                case "RightBevel":
                    shape.Fill = Graphics.GetBevelSideBrush((isLightingEnabled ? -110 : 180), oCRectfillColor); 
                    break;
                case "BottomBevel":
                    shape.Fill = Graphics.GetBevelSideBrush(90, oCRectfillColor); 
                    break;
            }
        }

        /// <summary>
        /// Place label for DataPoint
        /// </summary>
        /// <param name="visual">Visual</param>
        /// <param name="labelCanvas">Canvas for label</param>
        /// <param name="dataPoint">DataPoint</param>
        internal static void PlaceLabel(Canvas visual, Canvas labelCanvas, DataPoint dataPoint)
        {
            if ((Boolean)dataPoint.LabelEnabled && !String.IsNullOrEmpty(dataPoint.LabelText))
            {
                Canvas dataPointVisual = dataPoint.Faces.Visual as Canvas;

                Title tb = new Title()
                {   
                    Text = dataPoint.TextParser(dataPoint.LabelText),
                    InternalFontFamily = dataPoint.LabelFontFamily,
                    InternalFontSize = dataPoint.LabelFontSize.Value,
                    InternalFontWeight = (FontWeight)dataPoint.LabelFontWeight,
                    InternalFontStyle = (FontStyle)dataPoint.LabelFontStyle,
                    InternalBackground = dataPoint.LabelBackground,
                    InternalFontColor = Chart.CalculateDataPointLabelFontColor(dataPoint.Chart as Chart, dataPoint, dataPoint.LabelFontColor, LabelStyles.OutSide)
                };

                tb.CreateVisualObject();

                tb.TextElement.SetValue(Canvas.TopProperty, -(Double)1);

                Double labelTop;
                Double labelLeft;

                if (Double.IsNaN(dataPoint.LabelAngle) || dataPoint.LabelAngle == 0)
                {
                    labelTop = (Double)dataPointVisual.GetValue(Canvas.TopProperty) - tb.Height;
                    labelLeft = (Double)dataPointVisual.GetValue(Canvas.LeftProperty) + (dataPointVisual.Width - tb.Width) / 2;

                    if (labelTop < 0) labelTop = (Double)dataPointVisual.GetValue(Canvas.TopProperty);
                    if (labelLeft < 0) labelLeft = 1;
                    if (labelLeft + tb.ActualWidth > labelCanvas.Width)
                        labelLeft = labelCanvas.Width - tb.ActualWidth - 2;

                    tb.Visual.SetValue(Canvas.LeftProperty, labelLeft);
                    tb.Visual.SetValue(Canvas.TopProperty, labelTop - 2);
                }
                else
                {
                    Point centerOfRotation = new Point((Double)dataPointVisual.GetValue(Canvas.LeftProperty) + dataPointVisual.Width / 2,
                        (Double)dataPointVisual.GetValue(Canvas.TopProperty));

                    Double radius = 4;
                    Double angle = 0;
                    Double angleInRadian = 0;

                    if (dataPoint.LabelAngle > 0 && dataPoint.LabelAngle <= 90)
                    {
                        angle = dataPoint.LabelAngle - 180;
                        angleInRadian = (Math.PI / 180) * angle;
                        radius += tb.Width;
                        angle = (angleInRadian - Math.PI) * (180 / Math.PI);
                    }
                    else if (dataPoint.LabelAngle >= -90 && dataPoint.LabelAngle < 0)
                    {
                        angle = dataPoint.LabelAngle;
                        angleInRadian = (Math.PI / 180) * angle;
                    }
                    //else
                    //{
                    //    if (LabelAngle >= -90 && LabelAngle < 0)
                    //    {
                    //        angle = 180 + LabelAngle;
                    //        angleInRadian = (Math.PI / 180) * angle;
                    //        radius += TextBlockSize.Width;
                    //        angle = (angleInRadian - Math.PI) * (180 / Math.PI);
                    //        SetRotation(radius, angle, angleInRadian, centerOfRotation);
                    //    }
                    //    else if (LabelAngle > 0 && LabelAngle <= 90)
                    //    {
                    //        angle = LabelAngle;
                    //        angleInRadian = (Math.PI / 180) * angle;
                    //        SetRotation(radius, angle, angleInRadian, centerOfRotation);
                    //    }
                    //}

                    labelLeft = centerOfRotation.X + radius * Math.Cos(angleInRadian);
                    labelTop = centerOfRotation.Y + radius * Math.Sin(angleInRadian);

                    labelTop -= tb.Height / 2;

                    tb.Visual.SetValue(Canvas.LeftProperty, labelLeft);
                    tb.Visual.SetValue(Canvas.TopProperty, labelTop);

                    tb.Visual.RenderTransformOrigin = new Point(0, 0.5);
                    tb.Visual.RenderTransform = new RotateTransform()
                    {
                        CenterX = 0,
                        CenterY = 0,
                        Angle = angle
                    };
                }

                labelCanvas.Children.Add(tb.Visual);
                dataPoint.LabelVisual = tb.Visual;
            }
        }

        #endregion

        #region Internal Events And Delegates

        #endregion

        #region Data

        /// <summary>
        /// Width of a DataPoint
        /// </summary>#cbcbcb
        private static Double _dataPointWidth;
        internal static Brush _shadowColor = new SolidColorBrush(Color.FromArgb((Byte)0xff, (Byte)0xb0, (Byte)0xb0, (Byte)0xb0));
        internal static Double _shadowDepth = 1.5;

        #endregion
    }
}