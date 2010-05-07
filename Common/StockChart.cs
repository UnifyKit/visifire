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
    /// Visifire.Charts.StockChart class
    /// </summary>
    internal class StockChart
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
        /// <returns>New Calculated Brush</returns>
        internal static Brush ReCalculateAndApplyTheNewBrush(Shape shape, Brush newBrush, Boolean isLightingEnabled)
        {
            shape.Stroke = ((Boolean)isLightingEnabled) ? Graphics.GetLightingEnabledBrush(newBrush, "Linear", null) : newBrush;

            return shape.Stroke;
        }

        private static void UpdateYValueAndXValuePosition(DataPoint dataPoint, Double canvasWidth, Double canvasHeight, Double dataPointWidth)
        {   
            Canvas dataPointVisual = dataPoint.Faces.Visual as Canvas;
            Faces faces = dataPoint.Faces;
            Line highLowLine = faces.VisualComponents[0] as Line;  // HighLowline
            Line closeLine = faces.VisualComponents[1] as Line;    // Closeline
            Line openLine = faces.VisualComponents[2] as Line;     // Openline

            Double highY = 0, lowY = 0, openY = 0, closeY = 0;
            PlotGroup plotGroup = dataPoint.Parent.PlotGroup;

            CandleStick.SetDataPointValues(dataPoint, ref highY, ref lowY, ref openY, ref closeY);

            // Calculate required pixel positions
            Double xPositionOfDataPoint = Graphics.ValueToPixelPosition(0, canvasWidth, (Double)plotGroup.AxisX.InternalAxisMinimum, (Double)plotGroup.AxisX.InternalAxisMaximum, dataPoint.InternalXValue);

            openY = Graphics.ValueToPixelPosition(canvasHeight, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, openY);
            closeY = Graphics.ValueToPixelPosition(canvasHeight, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, closeY);
            highY = Graphics.ValueToPixelPosition(canvasHeight, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, highY);
            lowY = Graphics.ValueToPixelPosition(canvasHeight, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, lowY);

            Double dataPointTop = (lowY < highY) ? lowY : highY;
            openY = openY - dataPointTop;
            closeY = closeY - dataPointTop;

            dataPointVisual.Width = dataPointWidth;
            dataPointVisual.Height = Math.Abs(lowY - highY);
            
            // Set DataPoint Visual position
            dataPointVisual.SetValue(Canvas.LeftProperty, xPositionOfDataPoint - dataPointWidth / 2);
            dataPointVisual.SetValue(Canvas.TopProperty, dataPointTop);

            // Set position for high-low line
            highLowLine.X1 = dataPointVisual.Width / 2;
            highLowLine.X2 = dataPointVisual.Width / 2;
            highLowLine.Y1 = 0;
            highLowLine.Y2 = dataPointVisual.Height;

            // Set position for open line
            openLine.X1 = 0;
            openLine.X2 = dataPointVisual.Width / 2;
            openLine.Y1 = openY;
            openLine.Y2 = openY;

            // Set position for close line
            closeLine.X1 = dataPointVisual.Width / 2;
            closeLine.X2 = dataPointVisual.Width;
            closeLine.Y1 = closeY;
            closeLine.Y2 = closeY;

            // Need to apply shadow according to position of DataPoint
            ApplyOrUpdateShadow(dataPoint, dataPointVisual, highLowLine, openLine, closeLine, dataPointWidth);

            // Place label for the DataPoint
            CandleStick.CreateAndPositionLabel(dataPoint.Parent.Faces.LabelCanvas, dataPoint);

            if (dataPoint.Parent.ToolTipElement != null)
                dataPoint.Parent.ToolTipElement.Hide();

            (dataPoint.Chart as Chart).ChartArea.DisableIndicators();

            dataPoint._visualPosition = new Point((Double)dataPointVisual.GetValue(Canvas.LeftProperty) + dataPointVisual.Width / 2, (Double)dataPointVisual.GetValue(Canvas.TopProperty));
        }

        private static void ApplyBorderProperties(DataPoint dataPoint, Line highLow, Line openLine, Line closeLine, Double dataPointWidth)
        {
            highLow.StrokeThickness = (Double)dataPoint.BorderThickness.Left;

            if (highLow.StrokeThickness > dataPointWidth / 2)
                highLow.StrokeThickness = dataPointWidth / 2;
            else if (highLow.StrokeThickness > dataPointWidth)
                highLow.StrokeThickness = dataPointWidth;

            String borderStyle = dataPoint.BorderStyle.ToString();
            highLow.StrokeDashArray = Graphics.LineStyleToStrokeDashArray(borderStyle);

            // Set style for open line
            openLine.StrokeThickness = (Double)dataPoint.BorderThickness.Left;
            openLine.StrokeDashArray = Graphics.LineStyleToStrokeDashArray(borderStyle);

            // Set style for close line
            closeLine.StrokeThickness = (Double)dataPoint.BorderThickness.Left;
            closeLine.StrokeDashArray = Graphics.LineStyleToStrokeDashArray(borderStyle);
        }

        private static void ApplyOrUpdateColorForAStockDp(DataPoint dataPoint, Line highLow, Line openLine, Line closeLine)
        {   
            // Set style for lighlow line
            ReCalculateAndApplyTheNewBrush(highLow, dataPoint.Color, (Boolean)dataPoint.LightingEnabled);
            openLine.Stroke = highLow.Stroke;
            closeLine.Stroke = highLow.Stroke;
        }

        private static void ApplyOrUpdateShadow(DataPoint dataPoint, Canvas dataPointVisual, Line highLow, Line openLine, Line closeLine, Double dataPointWidth)
        {   
            #region Apply Shadow

            //Faces dpFaces = dataPoint.Faces;
            //dataPointVisual.Background = Graphics.GetRandomColor();

            //dpFaces.ClearList(ref dpFaces.ShadowElements);

            //if (dataPoint.ShadowEnabled == true)
            //{
            //    // Create High and Low Line
            //    Line highLowShadow = new Line()
            //    {   
            //        IsHitTestVisible = false,
            //        X1 = dataPointVisual.Width / 2 + CandleStick._shadowDepth,
            //        X2 = dataPointVisual.Width / 2 + CandleStick._shadowDepth,
            //        Y1 = 0 + CandleStick._shadowDepth,
            //        Y2 = dataPointVisual.Height + CandleStick._shadowDepth,
            //        Stroke = CandleStick._shadowColor,
            //        StrokeThickness = (Double)dataPoint.BorderThickness.Left,
            //        StrokeDashArray = Graphics.LineStyleToStrokeDashArray(dataPoint.BorderStyle.ToString())
            //    };

            //    // Create Open Line
            //    Line openShadowLine = new Line()
            //    {
            //        IsHitTestVisible = false,
            //        X1 = openLine.X1 + CandleStick._shadowDepth,
            //        X2 = openLine.X2 + CandleStick._shadowDepth,
            //        Y1 = openLine.Y1 + CandleStick._shadowDepth,
            //        Y2 = openLine.Y2 + CandleStick._shadowDepth,
            //        Stroke = CandleStick._shadowColor,
            //        StrokeThickness = (Double)dataPoint.BorderThickness.Left,
            //        StrokeDashArray = Graphics.LineStyleToStrokeDashArray(dataPoint.BorderStyle.ToString())
            //    };

            //    // Create Close Line
            //    Line closeShadowLine = new Line()
            //    {   
            //        IsHitTestVisible = false,
            //        X1 = closeLine.X1 + CandleStick._shadowDepth,
            //        X2 = closeLine.X2 + CandleStick._shadowDepth,
            //        Y1 = closeLine.Y1 + CandleStick._shadowDepth,
            //        Y2 = closeLine.Y2 + CandleStick._shadowDepth,

            //        Stroke = CandleStick._shadowColor,
            //        StrokeThickness = (Double)dataPoint.BorderThickness.Left,
            //        StrokeDashArray = Graphics.LineStyleToStrokeDashArray(dataPoint.BorderStyle.ToString())
            //    };

            //    // Add shadow elements to list of shadow elements
            //    dpFaces.ShadowElements.Add(highLowShadow);
            //    dpFaces.ShadowElements.Add(openShadowLine);
            //    dpFaces.ShadowElements.Add(closeShadowLine);

            //    // Add shadows
            //    dataPointVisual.Children.Add(highLowShadow);
            //    dataPointVisual.Children.Add(openShadowLine);
            //    dataPointVisual.Children.Add(closeShadowLine);
            //}

            if ((Boolean)dataPoint.ShadowEnabled)
                dataPointVisual.Effect = ExtendedGraphics.GetShadowEffect(315, 4, 0.95);
            else
                dataPointVisual.Effect = null;

            #endregion
        }

        internal static void CreateOrUpdateAStockDataPoint(DataPoint dataPoint, Canvas stockChartCanvas, Canvas labelCanvas, Double canvasWidth, Double canvasHeight, Double dataPointWidth)
        {
            Faces dpFaces = dataPoint.Faces;

            // Remove preexisting dataPoint visual and label visual
            if (dpFaces != null && dpFaces.Visual != null && stockChartCanvas == dpFaces.Visual.Parent)
            {   
                stockChartCanvas.Children.Remove(dataPoint.Faces.Visual);
            }

            // Remove preexisting label visual
            if (dataPoint.LabelVisual != null && dataPoint.LabelVisual.Parent == labelCanvas)
            {
                labelCanvas.Children.Remove(dataPoint.LabelVisual);
            }

            dataPoint.Faces = null;

            if (dataPoint.YValues == null || dataPoint.Enabled == false)
                return;

            // Initialize DataPoint faces
            dataPoint.Faces = new Faces();
            
            // Creating ElementData for Tag
            ElementData tagElement = new ElementData() { Element = dataPoint };

            // Create DataPoint Visual
            Canvas dataPointVisual = new Canvas();          // Create DataPoint Visual
            Line highLow = new Line(){ Tag = tagElement };  // Create High and Low Line
            Line closeLine = new Line(){ Tag = tagElement };    // Create Close Line
            Line openLine = new Line() { Tag = tagElement };    // Create Close Line

            dataPoint.Faces.Visual = dataPointVisual;

            // Add VisualComponents
            dataPoint.Faces.VisualComponents.Add(highLow);
            dataPoint.Faces.VisualComponents.Add(openLine);
            dataPoint.Faces.VisualComponents.Add(closeLine);

            // Add Border elements
            dataPoint.Faces.BorderElements.Add(highLow);
            dataPoint.Faces.BorderElements.Add(openLine);
            dataPoint.Faces.BorderElements.Add(closeLine);

            dataPoint.Faces.Visual = dataPointVisual;
            stockChartCanvas.Children.Add(dataPointVisual);
            
            UpdateYValueAndXValuePosition(dataPoint, canvasWidth, canvasHeight, dataPointWidth);
            ApplyBorderProperties(dataPoint, highLow, openLine, closeLine, dataPointWidth);
            ApplyOrUpdateColorForAStockDp(dataPoint, highLow, openLine, closeLine);
                        
            // Add VisualComponents to visual
            dataPointVisual.Children.Add(highLow);
            dataPointVisual.Children.Add(openLine);
            dataPointVisual.Children.Add(closeLine);

            // Attach tooltip, events, href etc
            dataPointVisual.Opacity = dataPoint.Parent.Opacity * dataPoint.Opacity;
            Chart chart = dataPoint.Chart as Chart;
            dataPoint.SetCursor2DataPointVisualFaces();
            dataPoint.AttachEvent2DataPointVisualFaces(dataPoint);
            dataPoint.AttachEvent2DataPointVisualFaces(dataPoint.Parent);
            dataPoint._parsedToolTipText = dataPoint.TextParser(dataPoint.ToolTipText);
            if(!chart.IndicatorEnabled)
                dataPoint.AttachToolTip(chart, dataPoint, dataPoint.Faces.VisualComponents);
            dataPoint.AttachHref(chart, dataPoint.Faces.VisualComponents, dataPoint.Href, (HrefTargets)dataPoint.HrefTarget);
        }

        /// <summary>
        /// Get visual object for point chart
        /// </summary>
        /// <param name="width">Width of the charat</param>
        /// <param name="height">Height of the charat</param>
        /// <param name="plotDetails">plotDetails</param>
        /// <param name="seriesList">List of DataSeries</param>
        /// <param name="chart">Chart</param>
        /// <param name="plankDepth">Plank depth</param>
        /// <param name="animationEnabled">Whether animation is enabled</param>
        /// <returns>Point chart canvas</returns>
        internal static Canvas GetVisualObjectForStockChart(Panel preExistingPanel, Double width, Double height, PlotDetails plotDetails, List<DataSeries> seriesList, Chart chart, Double plankDepth, bool animationEnabled)
        {
            if (Double.IsNaN(width) || Double.IsNaN(height) || width <= 0 || height <= 0) return null;

            Canvas visual, labelCanvas, stockChartCanvas;

            RenderHelper.RepareCanvas4Drawing(preExistingPanel as Canvas, out visual, out labelCanvas, out stockChartCanvas, width, height);

            Double depth3d = plankDepth / (plotDetails.Layer3DCount == 0 ? 1 : plotDetails.Layer3DCount) * (chart.View3D ? 1 : 0);
            Double visualOffset = depth3d * (plotDetails.SeriesDrawingIndex[seriesList[0]] + 1 - (plotDetails.Layer3DCount == 0 ? 0 : 1));

            visual.SetValue(Canvas.TopProperty, visualOffset);
            visual.SetValue(Canvas.LeftProperty, -visualOffset);
                       
            Double animationBeginTime = 0;
            DataSeries _tempDataSeries = null;

            // Calculate width of a DataPoint 
            Double dataPointWidth = CandleStick.CalculateDataPointWidth(width, height, chart);
            
            foreach (DataSeries series in seriesList)
            {   
                if (series.Enabled == false)
                    continue;

                Faces dsFaces = new Faces() { Visual = stockChartCanvas, LabelCanvas = labelCanvas };
                series.Faces = dsFaces;

                PlotGroup plotGroup = series.PlotGroup;
                _tempDataSeries = series;

                foreach (DataPoint dataPoint in series.InternalDataPoints)
                    CreateOrUpdateAStockDataPoint(dataPoint, stockChartCanvas, labelCanvas, width, height, dataPointWidth);
            }

            // Apply animation to series
            if (animationEnabled)
            {
                if (_tempDataSeries.Storyboard == null)
                    _tempDataSeries.Storyboard = new Storyboard();

                _tempDataSeries.Storyboard = AnimationHelper.ApplyOpacityAnimation(stockChartCanvas, _tempDataSeries, _tempDataSeries.Storyboard, animationBeginTime, 1, 0, 1);
                animationBeginTime += 0.5;
            }

            // Label animation
            if (animationEnabled && _tempDataSeries != null)
                _tempDataSeries.Storyboard = AnimationHelper.ApplyOpacityAnimation(labelCanvas, _tempDataSeries, _tempDataSeries.Storyboard, animationBeginTime, 1, 0, 1);
            
            stockChartCanvas.Tag = null;

            // ColumnChart.CreateOrUpdatePlank(chart, seriesList[0].PlotGroup.AxisY, stockChartCanvas, depth3d, Orientation.Horizontal);

            // Remove old visual and add new visual in to the existing panel
            if (preExistingPanel != null)
            {
                visual.Children.RemoveAt(1);
                visual.Children.Add(stockChartCanvas);
            }
            else
            {
                labelCanvas.SetValue(Canvas.ZIndexProperty, 1);
                visual.Children.Add(labelCanvas);
                visual.Children.Add(stockChartCanvas);
            }

            RectangleGeometry clipRectangle = new RectangleGeometry();
            clipRectangle.Rect = new Rect(0, -chart.ChartArea.PLANK_DEPTH, width + chart.ChartArea.PLANK_OFFSET, height + chart.ChartArea.PLANK_DEPTH);
            visual.Clip = clipRectangle;

            return visual;
        }

        public static void Update(ObservableObject sender, VcProperties property, object newValue, Boolean isAxisChanged)
        {
            Boolean isDataPoint = sender.GetType().Equals(typeof(DataPoint));

            if (isDataPoint)
                UpdateDataPoint(sender as DataPoint, property, newValue, isAxisChanged);
            else
                UpdateDataSeries(sender as DataSeries, property, newValue, isAxisChanged);
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
                dataPointWidth = CandleStick.CalculateDataPointWidth(dsFaces.Visual.Width, dsFaces.Visual.Height, chart);

            if (property == VcProperties.Enabled)
            {
                CreateOrUpdateAStockDataPoint(dataPoint, dsFaces.Visual as Canvas, dsFaces.LabelCanvas, dsFaces.Visual.Width, dsFaces.Visual.Height, dataPointWidth);
                return;
            }

            if (dpFaces == null)
                return;

            Canvas dataPointVisual = dpFaces.Visual as Canvas;       // DataPoint visual canvas
            Line highLowLine = dpFaces.VisualComponents[0] as Line;  // HighLowline
            Line closeLine = dpFaces.VisualComponents[1] as Line;    // Closeline
            Line openLine = dpFaces.VisualComponents[2] as Line;     // Openline

            switch (property)
            {   
                case VcProperties.BorderThickness:
                case VcProperties.BorderStyle:
                    ApplyBorderProperties(dataPoint, highLowLine, openLine, closeLine, dataPointWidth);
                    break;

                case VcProperties.Color:
                    ApplyOrUpdateColorForAStockDp(dataPoint, highLowLine, openLine, closeLine);
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
                    CandleStick.CreateAndPositionLabel(dsFaces.LabelCanvas, dataPoint);
                    break;

                case VcProperties.LegendText:
                    chart.InvokeRender();
                    break;

                case VcProperties.LightingEnabled:
                    ApplyOrUpdateColorForAStockDp(dataPoint, highLowLine, openLine, closeLine);
                    break;

                //case VcProperties.MarkerBorderColor:
                //case VcProperties.MarkerBorderThickness:
                //case VcProperties.MarkerColor:
                //case VcProperties.MarkerEnabled:
                //case VcProperties.MarkerScale:
                //case VcProperties.MarkerSize:
                //case VcProperties.MarkerType:
                case VcProperties.ShadowEnabled:
                    ApplyOrUpdateShadow(dataPoint, dataPointVisual, highLowLine, openLine, closeLine, dataPointWidth);
                    break;

                case VcProperties.Opacity:
                    dpFaces.Visual.Opacity = dataSeries.Opacity * dataPoint.Opacity;
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
                    CandleStick.CreateAndPositionLabel(dsFaces.LabelCanvas, dataPoint);
                    break;

                case VcProperties.XValueType:
                    chart.InvokeRender();
                    break;

                case VcProperties.Enabled:
                    CreateOrUpdateAStockDataPoint(dataPoint, dsFaces.Visual as Canvas, dsFaces.LabelCanvas, dsFaces.Visual.Width, dsFaces.Visual.Height, dataPointWidth);
                    break;

                case VcProperties.XValue:
                case VcProperties.YValue:
                case VcProperties.YValues:
                    if (isAxisChanged)
                        UpdateDataSeries(dataSeries, property, newValue, isAxisChanged);
                    else
                    {
                        dataPoint._parsedToolTipText = dataPoint.TextParser(dataPoint.ToolTipText);
                        UpdateYValueAndXValuePosition(dataPoint, dsFaces.Visual.Width, dsFaces.Visual.Height, dpFaces.Visual.Width);

                        if ((Boolean)dataPoint.LabelEnabled)
                            CandleStick.CreateAndPositionLabel(dsFaces.LabelCanvas, dataPoint);
                    }

                    if (dataPoint.Parent.SelectionEnabled && dataPoint.Selected)
                        dataPoint.Select(true);

                    break;
            }
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

        #endregion
    }
}