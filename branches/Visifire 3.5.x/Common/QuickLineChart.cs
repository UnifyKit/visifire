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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
#else
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
#endif
using System.Linq;

using Visifire.Commons;
using Visifire.Charts;

namespace Visifire.Charts
{
    /// <summary>
    /// Visifire.Charts.QuickLineChart class
    /// </summary>
    internal partial class QuickLineChart
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

        public static void Update(ObservableObject sender, VcProperties property, object newValue, Boolean isAxisChanged)
        {
            Boolean isDataPoint = sender.GetType().Equals(typeof(DataPoint));

            if (isDataPoint)
            {
                DataSeries ds = (sender as DataPoint).Parent;

                if (ds != null && (property == VcProperties.YValue || property == VcProperties.XValue))
                    UpdateDataSeries(ds, property, newValue);
            }
            else
                UpdateDataSeries(sender as DataSeries, property, newValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj">Object may be DataSeries or DataPoint</param>
        /// <param name="property"></param>
        /// <param name="newValue"></param>
        private static void UpdateDataSeries(ObservableObject obj, VcProperties property, object newValue)
        {
            DataPoint dataPoint = null;
            DataSeries dataSeries = obj as DataSeries;
            Boolean isDataPoint = false;

            if (dataSeries == null)
            {
                isDataPoint = true;
                dataPoint = obj as DataPoint;
                dataSeries = dataPoint.Parent;
            }

            Chart chart = dataSeries.Chart as Chart;

            PlotGroup plotGroup = dataSeries.PlotGroup;
            Canvas line2dCanvas = null;
            Canvas label2dCanvas = null;
            Path linePath = null;
            Path lineShadowPath = null;

            if (dataSeries.Faces != null)
            {
                if (dataSeries.Faces.Parts.Count > 0)
                {
                    linePath = dataSeries.Faces.Parts[0] as Path;

                    if (dataSeries.Faces.Parts.Count > 1)
                        lineShadowPath = dataSeries.Faces.Parts[1] as Path;
                }

                line2dCanvas = dataSeries.Faces.Visual as Canvas;
                label2dCanvas = dataSeries.Faces.LabelCanvas as Canvas;
            }
            else if (dataSeries.Faces == null && property == VcProperties.Enabled && (Boolean)newValue == true)
            {
                ColumnChart.Update(chart, dataSeries.RenderAs, (from ds in chart.InternalSeries where ds.RenderAs == RenderAs.QuickLine select ds).ToList());
                return;
            }
            else
                return;

            Double height = chart.ChartArea.ChartVisualCanvas.Height;
            Double width = chart.ChartArea.ChartVisualCanvas.Width;

            switch (property)
            {   
                case VcProperties.Color:
                    if (linePath != null)
                    {
                        Brush lineColorValue = (newValue != null) ? newValue as Brush : dataSeries.Color;

                        linePath.Stroke = ((Boolean)dataSeries.LightingEnabled) ? Graphics.GetLightingEnabledBrush(lineColorValue, "Linear", new Double[] { 0.65, 0.55 }) : lineColorValue; //dataPoint.Color;
                    }
                    break;
                case VcProperties.LightingEnabled:
                    if (linePath != null)
                        linePath.Stroke = ((Boolean)newValue) ? Graphics.GetLightingEnabledBrush(dataSeries.Color, "Linear", new Double[] { 0.65, 0.55 }) : dataSeries.Color;

                    break;

                case VcProperties.Opacity:
                    if (linePath != null)
                        linePath.Opacity = (Double)dataSeries.Opacity;
                    break;
                case VcProperties.LineStyle:
                case VcProperties.LineThickness:

                    if (lineShadowPath != null)
                        lineShadowPath.StrokeThickness = (Double)dataSeries.LineThickness;
                    if (linePath != null)
                        linePath.StrokeThickness = (Double)dataSeries.LineThickness;

                    if (lineShadowPath != null)
                        lineShadowPath.StrokeDashArray = ExtendedGraphics.GetDashArray(dataSeries.LineStyle);
                    if (linePath != null)
                        linePath.StrokeDashArray = ExtendedGraphics.GetDashArray(dataSeries.LineStyle);

                    break;
                case VcProperties.Enabled:

                    if (!isDataPoint && line2dCanvas != null)
                    {
                        if ((Boolean)newValue == false)
                        {
                            line2dCanvas.Visibility = Visibility.Collapsed;
                            label2dCanvas.Visibility = Visibility.Collapsed;
                        }
                        else
                        {
                            if (line2dCanvas.Parent == null)
                            {

                                ColumnChart.Update(chart, dataSeries.RenderAs, (from ds in chart.InternalSeries where ds.RenderAs == RenderAs.QuickLine select ds).ToList());
                                return;
                            }

                            line2dCanvas.Visibility = Visibility.Visible;
                            label2dCanvas.Visibility = Visibility.Visible;
                        }

                        chart._toolTip.Hide();

                        break;
                    }

                    goto RENDER_SERIES;

                case VcProperties.ShadowEnabled:
                case VcProperties.DataPoints:
                case VcProperties.YValue:
                case VcProperties.YValues:
                case VcProperties.XValue:
                case VcProperties.ViewportRangeEnabled:
                case VcProperties.DataPointUpdate:
                RENDER_SERIES:

                    LineChart.UpdateLineSeries(dataSeries, width, height, label2dCanvas);

                    break;
            }
        }


        #endregion

        #region Internal Methods

        /// <summary>
        /// Returns the visual object for line chart 
        /// </summary>
        /// <param name="width">PlotArea width</param>
        /// <param name="height">PlotArea height</param>
        /// <param name="plotDetails">PlotDetails</param>
        /// <param name="seriesList">List of line series</param>
        /// <param name="chart">Chart</param>
        /// <param name="plankDepth">PlankDepth</param>
        /// <param name="animationEnabled">Whether animation is enabled for chart</param>
        /// <returns>Canvas</returns>
        internal static Canvas GetVisualObjectForQuickLineChart(Panel preExistingPanel, Double width, Double height, PlotDetails plotDetails, List<DataSeries> seriesList, Chart chart, Double plankDepth, bool animationEnabled)
        {
            if (Double.IsNaN(width) || Double.IsNaN(height) || width <= 0 || height <= 0)
                return null;

            DataSeries currentDataSeries;

            Canvas visual, labelsCanvas, chartsCanvas;
            RenderHelper.RepareCanvas4Drawing(preExistingPanel as Canvas, out visual, out labelsCanvas, out chartsCanvas, width, height);

            Double depth3d = plankDepth / (plotDetails.Layer3DCount == 0 ? 1 : plotDetails.Layer3DCount) * (chart.View3D ? 1 : 0);
            Double visualOffset = depth3d * (plotDetails.SeriesDrawingIndex[seriesList[0]] + 1 - (plotDetails.Layer3DCount == 0 ? 0 : 1));

            // Set visual canvas position

            visual.SetValue(Canvas.TopProperty, visualOffset);
            visual.SetValue(Canvas.LeftProperty, -visualOffset);

            // visual.Background = new SolidColorBrush(Colors.Yellow);

            Boolean isMovingMarkerEnabled = false; // Whether moving marker is enabled for atleast one series

            Double minimumXValue = Double.MaxValue;
            Double maximumXValue = Double.MinValue;

            foreach (DataSeries series in seriesList)
            {   
                currentDataSeries = series;
                LineChart.CreateAlineSeries(series, width, height, labelsCanvas, chartsCanvas, animationEnabled);
                isMovingMarkerEnabled = isMovingMarkerEnabled || series.MovingMarkerEnabled;

                minimumXValue = Math.Min(minimumXValue, series.PlotGroup.MinimumX);
                maximumXValue = Math.Max(maximumXValue, series.PlotGroup.MaximumX);
            }

            // If animation is not enabled or if there are no series in the serieslist the dont apply animation
            if (animationEnabled && seriesList.Count > 0)
            {
                // Apply animation to the label canvas
                currentDataSeries = seriesList[0];

                if (currentDataSeries.Storyboard == null)
                    currentDataSeries.Storyboard = new Storyboard();

                currentDataSeries.Storyboard = LineChart.ApplyLineChartAnimation(currentDataSeries, labelsCanvas, currentDataSeries.Storyboard, false);
            }

            // Remove old visual and add new visual in to the existing panel
            if (preExistingPanel != null)
            {
                visual.Children.RemoveAt(1);
                // chartsCanvas.Background = Graphics.GetRandomColor();
                visual.Children.Add(chartsCanvas);
            }
            else
            {
                labelsCanvas.SetValue(Canvas.ZIndexProperty, 1);
                visual.Children.Add(labelsCanvas);
                visual.Children.Add(chartsCanvas);
            }

            chartsCanvas.Height = height;
            labelsCanvas.Height = height;
            chartsCanvas.Width = width;
            labelsCanvas.Width = width;

            LineChart.Clip(chart, chartsCanvas, labelsCanvas, seriesList[0].PlotGroup);

            return visual;
        }

        #endregion

        #region Internal Events And Delegates

        #endregion

        #region Data

        #endregion
    }
}