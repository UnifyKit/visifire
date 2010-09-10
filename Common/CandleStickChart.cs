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
       internal static Double GetStrokeThickness(DataPoint dataPoint, Double dataPointWidth)
        {
            return dataPoint.BorderThickness.Left == 0 ? ((Double)dataPoint.Parent.LineThickness >= dataPointWidth / 2 ? dataPointWidth / 4 : (Double)dataPoint.Parent.LineThickness) : dataPoint.BorderThickness.Left;
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

            if (dataPoint.InternalYValues.Length >= 4)
            {
                openY = dataPoint.InternalYValues[0];
                closeY = dataPoint.InternalYValues[1];
                highY = dataPoint.InternalYValues[2];
                lowY = dataPoint.InternalYValues[3];
            }
            else if (dataPoint.InternalYValues.Length >= 3)
            {
                openY = dataPoint.InternalYValues[0];
                closeY = dataPoint.InternalYValues[1];
                highY = dataPoint.InternalYValues[2];
            }
            else if (dataPoint.InternalYValues.Length >= 2)
            {
                openY = dataPoint.InternalYValues[0];
                closeY = dataPoint.InternalYValues[1];
            }
            else if (dataPoint.InternalYValues.Length >= 1)
            {
                openY = dataPoint.InternalYValues[0];
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
                return (dataPointColor == null) ? dataPoint.Parent.PriceDownColor : dataPointColor;
        }

        internal static void ApplyOrRemoveBevel(DataPoint dataPoint, Double dataPointWidth)
        {   
            // Removing parts of bevel
            Canvas bevelCanvas = null;
            Faces faces = dataPoint.Faces;
            Rectangle openCloseRect = faces.VisualComponents[1] as Rectangle;

            faces.ClearList(ref faces.BevelElements);

            // Create Bevel
            if (dataPoint.Parent.Bevel && dataPointWidth > 8 && openCloseRect.Height > 6)
            {
                Double reduceSize = openCloseRect.StrokeThickness;

                if (dataPoint.Parent.SelectionEnabled && dataPoint.BorderThickness.Left == 0)
                    reduceSize = 1.5 + reduceSize;

                if (openCloseRect.Width - 2 * reduceSize >= 0 && openCloseRect.Height - 2 * reduceSize >= 0)
                {
                    Brush color = GetOpenCloseRectangleFillbrush(dataPoint, (Brush) dataPoint.GetValue(DataPoint.ColorProperty));
                    bevelCanvas = ExtendedGraphics.Get2DRectangleBevel(dataPoint, openCloseRect.Width - 2 * reduceSize, openCloseRect.Height - 2 * reduceSize, 5, 5,
                       Graphics.GetBevelTopBrush(color),
                       Graphics.GetBevelSideBrush(((Boolean)dataPoint.LightingEnabled ? -70 : 0), color),
                       Graphics.GetBevelSideBrush(((Boolean)dataPoint.LightingEnabled ? -110 : 180), color),
                       Graphics.GetBevelSideBrush(90, color));

                    bevelCanvas.IsHitTestVisible = false;

                    bevelCanvas.SetValue(Canvas.TopProperty, (Double)openCloseRect.GetValue(Canvas.TopProperty) + reduceSize);
                    bevelCanvas.SetValue(Canvas.LeftProperty, reduceSize);

                    // Adding parts of bevel
                    foreach (FrameworkElement fe in bevelCanvas.Children)
                        dataPoint.Faces.BevelElements.Add(fe);

                    dataPoint.Faces.BevelElements.Add(bevelCanvas);

                    (dataPoint.Faces.Visual as Canvas).Children.Add(bevelCanvas);
                }
            }
        }

        /// <summary>
        /// ApplyOrRemoveShadow
        /// </summary>
        /// <param name="dataPoint">DataPoint</param>
        internal static void ApplyOrRemoveShadow(DataPoint dataPoint, Double dataPointWidth)
        {
            Canvas dataPointVisual = dataPoint.Faces.Visual as Canvas;

            if (!VisifireControl.IsMediaEffectsEnabled)
            {
                Faces faces = dataPoint.Faces;
                Rectangle openCloseRect = faces.VisualComponents[1] as Rectangle;

                dataPoint.Faces.ClearList(ref faces.ShadowElements);

                if ((Boolean)dataPoint.ShadowEnabled)
                {
                    // Shadow for line
                    Line highLowShadowLine = new Line()
                    {
                        IsHitTestVisible = false,
                        X1 = dataPointVisual.Width / 2 + _shadowDepth,
                        X2 = dataPointVisual.Width / 2 + _shadowDepth,
                        Y1 = 0,
                        Y2 = Math.Max(dataPointVisual.Height - _shadowDepth, 1),
                        Stroke = _shadowColor,
                        StrokeThickness = GetStrokeThickness(dataPoint, dataPointWidth),
                        StrokeDashArray = Graphics.LineStyleToStrokeDashArray(dataPoint.BorderStyle.ToString())
                    };

                    highLowShadowLine.SetValue(Canvas.ZIndexProperty, -4);
                    highLowShadowLine.SetValue(Canvas.TopProperty, _shadowDepth);

                    // Shadow for Rectangle
                    Rectangle openCloseShadowRect = new Rectangle()
                    {
                        IsHitTestVisible = false,
                        Fill = _shadowColor,
                        Width = dataPointWidth,
                        Height = openCloseRect.Height,
                        Stroke = _shadowColor,
                        StrokeThickness = dataPoint.BorderThickness.Left,
                        StrokeDashArray = Graphics.LineStyleToStrokeDashArray(dataPoint.BorderStyle.ToString())
                    };

                    openCloseShadowRect.SetValue(Canvas.TopProperty, (Double)openCloseRect.GetValue(Canvas.TopProperty) + _shadowDepth);
                    openCloseShadowRect.SetValue(Canvas.LeftProperty, (Double)openCloseRect.GetValue(Canvas.LeftProperty) + _shadowDepth);
                    openCloseShadowRect.SetValue(Canvas.ZIndexProperty, -4);

                    // Add elements into the list of Shadow Elements
                    faces.ShadowElements.Add(highLowShadowLine);
                    faces.ShadowElements.Add(openCloseShadowRect);

                    // Added to DataPoint visual Canvas
                    dataPointVisual.Children.Add(openCloseShadowRect);
                    dataPointVisual.Children.Add(highLowShadowLine);
                }
            }
            else
            {
#if !WP
                if ((Boolean)dataPoint.ShadowEnabled)
                    dataPointVisual.Effect = ExtendedGraphics.GetShadowEffect(315, 5, 0.95);
                else
                    dataPointVisual.Effect = null;
#endif
            }
        }

        /// <summary>
        /// Update position of the DataPoint according to YValue and XValue 
        /// </summary>
        /// <param name="dataPoint">DataPoint</param>
        /// <param name="canvasWidth">Width of the Canvas</param>
        /// <param name="canvasHeight">height of the Canvas</param>
        /// <param name="dataPointWidth">Width of the DataPoint</param>
        private static void UpdateYValueAndXValuePosition(DataPoint dataPoint, Double canvasWidth, Double canvasHeight, Double dataPointWidth)
        {   
            Faces dpFaces = dataPoint.Faces;
            Double xPositionOfDataPoint, highY = 0, lowY = 0, openY = 0, closeY = 0;
            PlotGroup plotGroup = dataPoint.Parent.PlotGroup;

            SetDataPointValues(dataPoint, ref highY, ref lowY, ref openY, ref closeY);

            // Calculate required pixel positions
            xPositionOfDataPoint = Graphics.ValueToPixelPosition(0, canvasWidth, (Double)plotGroup.AxisX.InternalAxisMinimum, (Double)plotGroup.AxisX.InternalAxisMaximum, dataPoint.InternalXValue);
            openY = Graphics.ValueToPixelPosition(canvasHeight, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, openY);
            closeY = Graphics.ValueToPixelPosition(canvasHeight, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, closeY);
            highY = Graphics.ValueToPixelPosition(canvasHeight, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, highY);
            lowY = Graphics.ValueToPixelPosition(canvasHeight, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, lowY);

            dataPoint._isPriceUp = (openY > closeY) ? true : false;

            // Update size and position of visual of the DatPoint
            Canvas dataPointVisual = dataPoint.Faces.Visual as Canvas;
            dataPointVisual.Width = dataPointWidth;
            dataPointVisual.Height = Math.Abs(lowY - highY);
            dataPointVisual.SetValue(Canvas.TopProperty, (highY < lowY) ? highY : lowY);
            dataPointVisual.SetValue(Canvas.LeftProperty, xPositionOfDataPoint - dataPointWidth / 2);

            // Update size and position of High-Low line of the DatPoint
            Line highLowLine = dataPoint.Faces.VisualComponents[0] as Line;
            highLowLine.X1 = dataPointVisual.Width / 2;
            highLowLine.X2 = dataPointVisual.Width / 2;
            highLowLine.Y1 = 0;
            highLowLine.Y2 = dataPointVisual.Height;
            highLowLine.Tag = new ElementData() { Element = dataPoint, VisualElementName = "HlLine" };

            // Update size and position of Open-Close rectangle of the DatPoint
            Rectangle openCloseRect = dpFaces.VisualComponents[1] as Rectangle;
            openCloseRect.Width = dataPointWidth;
            openCloseRect.Height = Math.Max(Math.Abs(openY - closeY), 1);
            openCloseRect.Tag = new ElementData() { Element = dataPoint, VisualElementName = "OcRect" };
            openCloseRect.SetValue(Canvas.TopProperty, ((closeY > openY) ? openY : closeY) - ((Double)dataPointVisual.GetValue(Canvas.TopProperty)));
            openCloseRect.SetValue(Canvas.LeftProperty, (Double)0);

            // Apply color for a CandleStick
            ApplyOrUpdateColorForACandleStick(dataPoint);

            // Need to re position the shadow also
            ApplyOrRemoveShadow(dataPoint, dataPointWidth);

            // Apply or remove _axisIndicatorBorderElement
            ApplyOrUpdateBorder(dataPoint, dataPointWidth);

            // Apply or remove bevel
            ApplyOrRemoveBevel(dataPoint, dataPointWidth);
            
            if(dataPoint.LabelVisual != null)
                SetLabelPosition(dataPoint, canvasWidth, canvasHeight);

            if (dataPoint.Parent.ToolTipElement != null)
                dataPoint.Parent.ToolTipElement.Hide();

            (dataPoint.Chart as Chart).ChartArea.DisableIndicators();

            dataPoint._visualPosition= new Point((Double)dataPointVisual.GetValue(Canvas.LeftProperty) + dataPointVisual.Width / 2, (Double)dataPointVisual.GetValue(Canvas.TopProperty));

        }

        /// <summary>
        /// Apply or Update _axisIndicatorBorderElement for the DataPoint
        /// </summary>
        /// <param name="dataPoint"></param>
        /// <param name="dataPointWidth"></param>
        private static void ApplyOrUpdateBorder(DataPoint dataPoint, Double dataPointWidth)
        {   
            Faces dpFaces = dataPoint.Faces;
            Line highLowLine = dataPoint.Faces.VisualComponents[0] as Line;
            Rectangle openCloseRect = dpFaces.VisualComponents[1] as Rectangle;

            highLowLine.StrokeThickness = GetStrokeThickness(dataPoint, dataPointWidth);
            highLowLine.StrokeDashArray = Graphics.LineStyleToStrokeDashArray(dataPoint.BorderStyle.ToString());

            openCloseRect.StrokeThickness = dataPoint.BorderThickness.Left;
            openCloseRect.StrokeDashArray = Graphics.LineStyleToStrokeDashArray(dataPoint.BorderStyle.ToString());
            openCloseRect.Stroke = GetOpenCloseRectangleBorderbrush(dataPoint, (Brush)dataPoint.GetValue(DataPoint.ColorProperty));
        }

        /// <summary>
        /// Create or update a CandleStick
        /// </summary>
        /// <param name="dataPoint"></param>
        /// <param name="seriesCanvas"></param>
        /// <param name="labelCanvas"></param>
        /// <param name="canvasWidth"></param>
        /// <param name="canvasHeight"></param>
        /// <param name="dataPointWidth"></param>
        internal static void CreateOrUpdateACandleStick(DataPoint dataPoint, Canvas candleStickCanvas, Canvas labelCanvas, Double canvasWidth, Double canvasHeight, Double dataPointWidth)
        {
            Faces dpFaces = dataPoint.Faces;

            // Remove preexisting dataPoint visual and label visual
            if (dpFaces != null && dpFaces.Visual != null && candleStickCanvas == dpFaces.Visual.Parent)
            {
                candleStickCanvas.Children.Remove(dataPoint.Faces.Visual);
                //dpFaces = null;
            }

            // Remove preexisting label visual
            if(dataPoint.LabelVisual != null && dataPoint.LabelVisual.Parent == labelCanvas)
            {
                labelCanvas.Children.Remove(dataPoint.LabelVisual);
                //dataPoint.LabelVisual = null;
            }

            dataPoint.Faces = null;

            if (dataPoint.InternalYValues == null || dataPoint.Enabled == false)
                return;

            // Creating ElementData for Tag
            ElementData tagElement = new ElementData() { Element = dataPoint };

            // Initialize DataPoint faces
            dataPoint.Faces = new Faces();
            
            // Create DataPoint Visual
            Canvas dataPointVisual = new Canvas();
            dataPoint.Faces.Visual = dataPointVisual;

            // Create High and Low Line
            Line highLowLine = new Line() { Tag = tagElement };
            
            dataPoint.Faces.Parts.Add(highLowLine);
            dataPoint.Faces.VisualComponents.Add(highLowLine);
            dataPointVisual.Children.Add(highLowLine);

            /* Create Open-Close Rectangle
             * Math.Max is used to make sure that the rectangle is visible 
             * even when the difference between high and low is 0 */
            Rectangle openCloseRect = new Rectangle() { Tag = tagElement };
            
            dataPoint.Faces.VisualComponents.Add(openCloseRect);
            dataPoint.Faces.BorderElements.Add(openCloseRect);
            dataPointVisual.Children.Add(openCloseRect);

            UpdateYValueAndXValuePosition(dataPoint, canvasWidth, canvasHeight, dataPointWidth);

            // Add dataPointVisual to seriesCanvas
            candleStickCanvas.Children.Add(dataPointVisual);

            CreateAndPositionLabel(labelCanvas, dataPoint);

            dataPointVisual.Opacity = (Double)dataPoint.Parent.Opacity * (Double)dataPoint.Opacity;

            Chart chart = dataPoint.Chart as Chart;
            dataPoint.SetCursor2DataPointVisualFaces();
            dataPoint.AttachEvent2DataPointVisualFaces(dataPoint);
            dataPoint.AttachEvent2DataPointVisualFaces(dataPoint.Parent);
            dataPoint._parsedToolTipText = dataPoint.TextParser(dataPoint.ToolTipText);
            if (!chart.IndicatorEnabled)
            {
                dataPoint.AttachToolTip(chart, dataPoint, dataPoint.Faces.VisualComponents);

                if(dataPoint.LabelVisual != null)
                    dataPoint.AttachToolTip(chart, dataPoint, dataPoint.LabelVisual);
            }
            dataPoint.AttachHref(chart, dataPoint.Faces.VisualComponents, dataPoint.Href, (HrefTargets)dataPoint.HrefTarget);
        }

        /// <summary>
        /// Apply or update Color for a CandleStick
        /// </summary>
        /// <param name="dataPoint">DataPoint</param>
        internal static void ApplyOrUpdateColorForACandleStick(DataPoint dataPoint)
        {   
            Faces dpFaces = dataPoint.Faces;
            Brush dataPointColor = (Brush)dataPoint.GetValue(DataPoint.ColorProperty);

            #region  Update Color for High-Low line and Open-Close rectangle

            Line highLowLine = dataPoint.Faces.VisualComponents[0] as Line;
            Rectangle openCloseRect = dataPoint.Faces.VisualComponents[1] as Rectangle;
            Brush highLowLineColor;
            Brush openCloseRectColor;
            openCloseRectColor = GetOpenCloseRectangleFillbrush(dataPoint, dataPointColor);

            if (dataPoint.StickColor != null)
                highLowLineColor = dataPoint.StickColor;
            else if (dataPointColor == null)
                highLowLineColor = dataPoint.Parent.PriceUpColor;
            else
                highLowLineColor = dataPointColor;

            if ((Boolean)dataPoint.LightingEnabled)
            {
                openCloseRect.Fill = Graphics.GetLightingEnabledBrush(openCloseRectColor, "Linear", null);
                highLowLine.Stroke = Graphics.GetLightingEnabledBrush(highLowLineColor, "Linear", null);
            }
            else
            {
                openCloseRect.Fill = openCloseRectColor;
                highLowLine.Stroke = highLowLineColor;
            }
          
            #endregion

            #region Update Color for Bevel layer

            UpdateBevelLayerColor(dataPoint, openCloseRectColor);
            
            #endregion
        }


        internal static void UpdateBevelLayerColor(DataPoint dataPoint, Brush brush)
        {
            Boolean isLightingEnabled = (Boolean)dataPoint.LightingEnabled;
            Faces dpFaces = dataPoint.Faces;

            // Update the color for bevel Layer
            foreach (FrameworkElement fe in dpFaces.BevelElements)
            {
                Shape shape = fe as Shape;

                if (shape == null) continue;

                switch ((shape.Tag as ElementData).VisualElementName)
                {   
                    case "TopBevel":
                        shape.Fill = Graphics.GetBevelTopBrush(brush);
                        break;
                    case "LeftBevel":
                        shape.Fill = Graphics.GetBevelSideBrush((isLightingEnabled ? -70 : 0), brush);
                        break;
                    case "RightBevel":
                        shape.Fill = Graphics.GetBevelSideBrush((isLightingEnabled ? -110 : 180), brush);
                        break;
                    case "BottomBevel":
                        shape.Fill = Graphics.GetBevelSideBrush(90, brush);
                        break;
                }
            }
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
        internal static Canvas GetVisualObjectForCandleStick(Panel preExistingPanel, Double width, Double height, PlotDetails plotDetails, List<DataSeries> seriesList, Chart chart, Double plankDepth, bool animationEnabled)
        {
           // return new Canvas() { Background = Graphics.GetRandonColor() , Width = width, Height = height};

            if (Double.IsNaN(width) || Double.IsNaN(height) || width <= 0 || height <= 0) return null;

            Canvas visual, labelCanvas, candleStickCanvas;

            RenderHelper.RepareCanvas4Drawing(preExistingPanel as Canvas, out visual, out labelCanvas, out candleStickCanvas, width, height);
            
            Double depth3d = plankDepth / (plotDetails.Layer3DCount == 0 ? 1 : plotDetails.Layer3DCount) * (chart.View3D ? 1 : 0);
            Double visualOffset = depth3d * (plotDetails.SeriesDrawingIndex[seriesList[0]] + 1 - (plotDetails.Layer3DCount == 0 ? 0 : 1));
            
            visual.SetValue(Canvas.TopProperty, visualOffset);
            visual.SetValue(Canvas.LeftProperty, -visualOffset);

            Double animationBeginTime = 0;
            DataSeries _tempDataSeries = null;

            // Calculate width of a DataPoint
            Double dataPointWidth = CalculateDataPointWidth(width, height, chart);

            foreach (DataSeries series in seriesList)
            {
                if (series.Enabled == false)
                    continue;

                Faces dsFaces = new Faces() { Visual = candleStickCanvas, LabelCanvas = labelCanvas };
                series.Faces = dsFaces;

                PlotGroup plotGroup = series.PlotGroup;
                _tempDataSeries = series;

                List<DataPoint> viewPortDataPoints = RenderHelper.GetDataPointsUnderViewPort(series, false);

                foreach (DataPoint dataPoint in viewPortDataPoints)
                    CreateOrUpdateACandleStick(dataPoint, candleStickCanvas, labelCanvas, width, height, dataPointWidth);

                // Apply animation to series
                if (animationEnabled)
                {   
                    if (_tempDataSeries.Storyboard == null)
                        _tempDataSeries.Storyboard = new Storyboard();

                    _tempDataSeries.Storyboard = AnimationHelper.ApplyOpacityAnimation(candleStickCanvas, _tempDataSeries, _tempDataSeries.Storyboard, animationBeginTime, 1, 0, 1);
                    animationBeginTime += 0.5;
                }

                
            }

            // Label animation
            if (animationEnabled && _tempDataSeries != null)
                _tempDataSeries.Storyboard = AnimationHelper.ApplyOpacityAnimation(labelCanvas, _tempDataSeries, _tempDataSeries.Storyboard, animationBeginTime, 1, 0, 1);
            
            candleStickCanvas.Tag = null;

            // ColumnChart.CreateOrUpdatePlank(chart, seriesList[0].PlotGroup.AxisY, candleStickCanvas, depth3d, Orientation.Horizontal);

            // Remove old visual and add new visual in to the existing panel
            if (preExistingPanel != null)
            {
                visual.Children.RemoveAt(1);
                visual.Children.Add(candleStickCanvas);
            }
            else
            {   
                labelCanvas.SetValue(Canvas.ZIndexProperty, 1);
                visual.Children.Add(labelCanvas);
                visual.Children.Add(candleStickCanvas);
            }

            RectangleGeometry clipRectangle = new RectangleGeometry();
            clipRectangle.Rect = new Rect(0, -chart.ChartArea.PLANK_DEPTH, width + chart.ChartArea.PLANK_OFFSET, height + chart.ChartArea.PLANK_DEPTH);
            visual.Clip = clipRectangle;

            return visual;

            // visual.Children.Add(candleStickCanvas);
            // visual.Children.Add(labelCanvas);

            // return visual;
        }
        
        /// <summary>
        /// Place label for DataPoint
        /// </summary>
        /// <param name="visual">Visual</param>
        /// <param name="labelCanvas">Canvas for label</param>
        /// <param name="dataPoint">DataPoint</param>
        internal static void CreateAndPositionLabel(Canvas labelCanvas, DataPoint dataPoint)
        {   
            if (dataPoint.LabelVisual != null)
            {
                Panel parent = dataPoint.LabelVisual.Parent as Panel;

                if(parent != null)
                    parent.Children.Remove(dataPoint.LabelVisual);
            }
            
            if ((Boolean)dataPoint.LabelEnabled && !String.IsNullOrEmpty(dataPoint.LabelText))
            {   
                Canvas dataPointVisual = dataPoint.Faces.Visual as Canvas;

                Title tb = new Title()
                {   
                    Text = dataPoint.TextParser(dataPoint.LabelText),
                    FontFamily = dataPoint.LabelFontFamily,
                    FontSize = dataPoint.LabelFontSize.Value,
                    FontWeight = (FontWeight)dataPoint.LabelFontWeight,
                    FontStyle = (FontStyle)dataPoint.LabelFontStyle,
                    Background = dataPoint.LabelBackground,
                    FontColor = Chart.CalculateDataPointLabelFontColor(dataPoint.Chart as Chart, dataPoint, dataPoint.LabelFontColor, LabelStyles.OutSide)
                };

                tb.CreateVisualObject(new ElementData() { Element = dataPoint });
                tb.Visual.Height = tb.Height;
                tb.Visual.Width = tb.Width;
                dataPoint.LabelVisual = tb.Visual;

                // Double labelTop = (Double)dataPointVisual.GetValue(Canvas.TopProperty) - tb.Height;
                // Double labelLeft = (Double)dataPointVisual.GetValue(Canvas.LeftProperty) + (dataPointVisual.Width - tb.Width) / 2;

                // if (labelTop < 0) labelTop = (Double)dataPointVisual.GetValue(Canvas.TopProperty);
                // if (labelLeft < 0) labelLeft = 1;
                // if (labelLeft + tb.ActualWidth > labelCanvas.Width)
                //    labelLeft = labelCanvas.Width - tb.ActualWidth - 2;

                // tb.Visual.SetValue(Canvas.LeftProperty, labelLeft);
                // tb.Visual.SetValue(Canvas.TopProperty, labelTop);

                SetLabelPosition(dataPoint, labelCanvas.Width, labelCanvas.Height);

                labelCanvas.Children.Add(tb.Visual);
            }
        }

        private static void SetLabelPosition(DataPoint dataPoint, Double canvasWidth, Double canvasHeight)
        {
            Canvas dataPointVisual = dataPoint.Faces.Visual as Canvas;

            Double labelTop;
            Double labelLeft;

            if (Double.IsNaN(dataPoint.LabelAngle) || dataPoint.LabelAngle == 0)
            {
                labelTop = (Double)dataPointVisual.GetValue(Canvas.TopProperty) - dataPoint.LabelVisual.Height;
                labelLeft = (Double)dataPointVisual.GetValue(Canvas.LeftProperty) + (dataPointVisual.Width - dataPoint.LabelVisual.Width) / 2;

                if (labelTop < 0) labelTop = (Double)dataPointVisual.GetValue(Canvas.TopProperty);
                if (labelLeft < 0) labelLeft = 1;
                if (labelLeft + dataPoint.LabelVisual.Width > canvasWidth)
                    labelLeft = canvasWidth - dataPoint.LabelVisual.Width - 2;

                dataPoint.LabelVisual.SetValue(Canvas.LeftProperty, labelLeft);
                dataPoint.LabelVisual.SetValue(Canvas.TopProperty, labelTop);
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
                    radius += dataPoint.LabelVisual.Width;
                    angle = (angleInRadian - Math.PI) * (180 / Math.PI);
                }
                else if (dataPoint.LabelAngle >= -90 && dataPoint.LabelAngle < 0)
                {
                    angle = dataPoint.LabelAngle;
                    angleInRadian = (Math.PI / 180) * angle;
                }

                labelLeft = centerOfRotation.X + radius * Math.Cos(angleInRadian);
                labelTop = centerOfRotation.Y + radius * Math.Sin(angleInRadian);

                labelTop -= dataPoint.LabelVisual.Height / 2;

                dataPoint.LabelVisual.SetValue(Canvas.LeftProperty, labelLeft);
                dataPoint.LabelVisual.SetValue(Canvas.TopProperty, labelTop);

                dataPoint.LabelVisual.RenderTransformOrigin = new Point(0, 0.5);
                dataPoint.LabelVisual.RenderTransform = new RotateTransform()
                {
                    CenterX = 0,
                    CenterY = 0,
                    Angle = angle
                };
            }
        }

        public static void Update(ObservableObject sender, VcProperties property, object newValue, Boolean isAxisChanged)
        {
            Boolean isDataPoint = sender.GetType().Equals(typeof(DataPoint));

            if (isDataPoint)
                UpdateDataPoint(sender as DataPoint, property, newValue, isAxisChanged);
            else
                UpdateDataSeries(sender as DataSeries, property, newValue, isAxisChanged);
        }

        private static void UpdateDataSeries(DataSeries dataSeries, VcProperties property, object newValue, Boolean isAxisChanged)
        {
            Chart chart = dataSeries.Chart as Chart;
            switch (property)
            {
                case VcProperties.DataPoints:
                case VcProperties.YValues:
                case VcProperties.XValue:
                    chart.ChartArea.RenderSeries();
                    //Canvas ChartVisualCanvas = chart.ChartArea.ChartVisualCanvas;

                    //Double width = chart.ChartArea.ChartVisualCanvas.Width;
                    //Double height = chart.ChartArea.ChartVisualCanvas.Height;

                    //PlotDetails plotDetails = chart.PlotDetails;
                    //PlotGroup plotGroup = dataSeries.PlotGroup;

                    //List<DataSeries> dataSeriesListInDrawingOrder = plotDetails.SeriesDrawingIndex.Keys.ToList();

                    //List<DataSeries> selectedDataSeries4Rendering = new List<DataSeries>();

                    //RenderAs currentRenderAs = dataSeries.RenderAs;

                    //Int32 currentDrawingIndex = plotDetails.SeriesDrawingIndex[dataSeries];

                    //for (Int32 i = 0; i < chart.InternalSeries.Count; i++)
                    //{
                    //    if (currentRenderAs == dataSeriesListInDrawingOrder[i].RenderAs && currentDrawingIndex == plotDetails.SeriesDrawingIndex[dataSeriesListInDrawingOrder[i]])
                    //        selectedDataSeries4Rendering.Add(dataSeriesListInDrawingOrder[i]);
                    //}

                    //if (selectedDataSeries4Rendering.Count == 0)
                    //    return;

                    //Panel oldPanel = null;
                    //Dictionary<RenderAs, Panel> RenderedCanvasList = chart.ChartArea.RenderedCanvasList;

                    //if (chart.ChartArea.RenderedCanvasList.ContainsKey(currentRenderAs))
                    //{
                    //    oldPanel = RenderedCanvasList[currentRenderAs];
                    //}

                    //Panel renderedChart = chart.ChartArea.RenderSeriesFromList(oldPanel, selectedDataSeries4Rendering);

                    //if (oldPanel == null)
                    //{
                    //    chart.ChartArea.RenderedCanvasList.Add(currentRenderAs, renderedChart);
                    //    renderedChart.SetValue(Canvas.ZIndexProperty, currentDrawingIndex);
                    //    ChartVisualCanvas.Children.Add(renderedChart);
                    //}
                    //else
                    //    chart.ChartArea.RenderedCanvasList[currentRenderAs] = renderedChart;

                    //Visifire.Charts.Chart.SelectDataPoints(chart);
                    break;

                default:
                    // case VcProperties.Enabled:
                    foreach (DataPoint dataPoint in dataSeries.InternalDataPoints)
                        UpdateDataPoint(dataPoint, property, newValue, isAxisChanged);
                   break;
            }
        }

        private static void UpdateDataPoint(DataPoint dataPoint, VcProperties property, object newValue, Boolean isAxisChanged)
        {
            Chart chart = dataPoint.Chart as Chart;
            DataSeries dataSeries = dataPoint.Parent;
            PlotGroup plotGroup = dataSeries.PlotGroup;
            Faces dsFaces = dataSeries.Faces;
            Faces dpFaces = dataPoint.Faces;
            Double dataPointWidth;

            if (dsFaces != null)
                ColumnChart.UpdateParentVisualCanvasSize(chart, dsFaces.Visual as Canvas);

            if (dpFaces != null && dpFaces.Visual != null)
                dataPointWidth = dpFaces.Visual.Width;
            else if (dsFaces == null)
                return;
            else
                dataPointWidth = CalculateDataPointWidth(dsFaces.Visual.Width, dsFaces.Visual.Height, chart);

            if (property == VcProperties.Enabled || (dpFaces == null && (property == VcProperties.XValue || property == VcProperties.YValues)))
            {
                CreateOrUpdateACandleStick(dataPoint, dsFaces.Visual as Canvas, dsFaces.LabelCanvas, dsFaces.Visual.Width, dsFaces.Visual.Height, dataPointWidth);
                return;
            }

            if (dpFaces == null)
                return;

            switch (property)
            {   
                case VcProperties.BorderThickness:
                      ApplyOrUpdateBorder(dataPoint, dataPointWidth);
                      ApplyOrRemoveBevel(dataPoint, dataPointWidth);
                    break;

                case VcProperties.BorderStyle:
                    ApplyOrUpdateBorder(dataPoint, dataPointWidth);
                    break;

                case VcProperties.BorderColor:
                    ApplyOrUpdateBorder(dataPoint, dataPointWidth);
                    break;

                case VcProperties.Bevel:
                    ApplyOrRemoveBevel(dataPoint, dataPointWidth);
                    break;

                case VcProperties.Color:
                case VcProperties.PriceUpColor:
                case VcProperties.PriceDownColor:
                case VcProperties.StickColor:
                    ApplyOrUpdateColorForACandleStick(dataPoint);
                    break;
                    
                case VcProperties.Cursor:
                    dataPoint.SetCursor2DataPointVisualFaces();
                    break;

                case VcProperties.Href:
                    dataPoint.SetHref2DataPointVisualFaces();
                    break;

                case VcProperties.HrefTarget:
                    dataPoint.SetHref2DataPointVisualFaces();
                    break;

                case VcProperties.LabelBackground:
                case VcProperties.LabelEnabled:
                case VcProperties.LabelFontColor:
                case VcProperties.LabelFontFamily:
                case VcProperties.LabelFontStyle:
                case VcProperties.LabelFontSize:
                case VcProperties.LabelFontWeight:
                case VcProperties.LabelStyle:
                case VcProperties.LabelText:
                    CreateAndPositionLabel(dsFaces.LabelCanvas, dataPoint);
                    break;


                case VcProperties.LegendText:
                    chart.InvokeRender();
                    break;

                case VcProperties.LightingEnabled:
                    ApplyOrUpdateColorForACandleStick(dataPoint);
                    break;

                //case VcProperties.MarkerBorderColor:
                //case VcProperties.MarkerBorderThickness:
                //case VcProperties.MarkerColor:
                //case VcProperties.MarkerEnabled:
                //case VcProperties.MarkerScale:
                //case VcProperties.MarkerSize:
                //case VcProperties.MarkerType:
                case VcProperties.ShadowEnabled:
                    ApplyOrRemoveShadow(dataPoint, dataPointWidth);
                    break;

                case VcProperties.Opacity:
                    dpFaces.Visual.Opacity = (Double)dataSeries.Opacity * (Double)dataPoint.Opacity;
                    break;

                case VcProperties.ShowInLegend:
                    chart.InvokeRender();
                    break;
                case VcProperties.ToolTipText:
                    dataPoint._parsedToolTipText = dataPoint.TextParser(dataPoint.ToolTipText);
                    break;

                case VcProperties.XValueFormatString:
                case VcProperties.YValueFormatString:
                    dataPoint._parsedToolTipText = dataPoint.TextParser(dataPoint.ToolTipText);
                    CreateAndPositionLabel(dsFaces.LabelCanvas, dataPoint);
                    break;

                case VcProperties.XValueType:
                    chart.InvokeRender();
                    break;

                case VcProperties.Enabled:
                    CreateOrUpdateACandleStick(dataPoint, dsFaces.Visual as Canvas, dsFaces.LabelCanvas, dsFaces.Visual.Width, dsFaces.Visual.Height, dataPointWidth);
                    break;

                case VcProperties.XValue:
                case VcProperties.YValue:
                case VcProperties.YValues:
                    if (isAxisChanged || dataPoint.InternalYValues == null)
                        UpdateDataSeries(dataSeries, property, newValue, isAxisChanged);
                    else
                    {
                        dataPoint._parsedToolTipText = dataPoint.TextParser(dataPoint.ToolTipText);
                        UpdateYValueAndXValuePosition(dataPoint, dsFaces.Visual.Width, dsFaces.Visual.Height, dpFaces.Visual.Width);
                        
                        if ((Boolean)dataPoint.LabelEnabled)
                            CreateAndPositionLabel(dsFaces.LabelCanvas, dataPoint);
                    }

                    if (dataPoint.Parent.SelectionEnabled && dataPoint.Selected)
                        dataPoint.Select(true);
                    
                        break;
            }
        }

        //internal static void Update(Chart chart, RenderAs currentRenderAs, List<DataSeries> selectedDataSeries4Rendering, VcProperties property, object newValue)
        //{   
        //    Boolean is3D = chart.View3D;
        //    ChartArea chartArea = chart.ChartArea;
        //    Canvas ChartVisualCanvas = chart.ChartArea.ChartVisualCanvas;

        //    // Double width = chart.ChartArea.ChartVisualCanvas.Width;
        //    // Double height = chart.ChartArea.ChartVisualCanvas.Height;

        //    Panel preExistingPanel = null;
        //    Dictionary<RenderAs, Panel> RenderedCanvasList = chart.ChartArea.RenderedCanvasList;

        //    if (chartArea.RenderedCanvasList.ContainsKey(currentRenderAs))
        //    {
        //        preExistingPanel = RenderedCanvasList[currentRenderAs];
        //    }

        //    Panel renderedChart = chartArea.RenderSeriesFromList(preExistingPanel, selectedDataSeries4Rendering);

        //    if (preExistingPanel == null)
        //    {
        //        chartArea.RenderedCanvasList.Add(currentRenderAs, renderedChart);
        //        ChartVisualCanvas.Children.Add(renderedChart);
        //    }
        //}

        #endregion

        #region Internal Events And Delegates

        #endregion

        #region Data

        /// <summary>
        /// Width of a DataPoint
        /// </summary>#cbcbcb
        internal static Brush _shadowColor = new SolidColorBrush(Color.FromArgb((Byte)0xff, (Byte)0xb0, (Byte)0xb0, (Byte)0xb0));
        internal static Double _shadowDepth = 1.5;

        #endregion
    }
}