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
using System.Linq;

namespace Visifire.Charts
{   
    /// <summary>
    /// Visifire.Charts.PointChart class
    /// </summary>
    internal class PointChart
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

        /// <summary>
        /// Apply animation for point chart
        /// </summary>
        /// <param name="pointGrid">Point chart grid</param>
        /// <param name="storyboard">Stroyboard</param>
        /// <param name="width">Width of the chart canvas</param>
        /// <param name="height">Height of the chart canvas</param>
        /// <returns>Storyboard</returns>
        private static Storyboard ApplyPointChartAnimation(DataSeries currentDataSeries, Panel pointGrid, Storyboard storyboard, Double width, Double height)
        {   
#if WPF
            if (storyboard != null && storyboard.GetValue(System.Windows.Media.Animation.Storyboard.TargetProperty) != null)
                storyboard.Stop();
#else
            if (storyboard != null)
                storyboard.Stop();
#endif

            TransformGroup group = new TransformGroup();
            ScaleTransform scaleTransform = new ScaleTransform() { ScaleX = 0, ScaleY = 0, CenterX = 0.5, CenterY = 0.5 };
            TranslateTransform translateTransform = new TranslateTransform() { X = 0, Y = 0 };
            group.Children.Add(scaleTransform);
            group.Children.Add(translateTransform);

            pointGrid.RenderTransform = group;

            Random rand = new Random((Int32)DateTime.Now.Ticks);
            double begin = rand.NextDouble();

            pointGrid.Measure(new Size(Double.MaxValue, Double.MaxValue));

            DoubleCollection times = Graphics.GenerateDoubleCollection(0, 0.5, 0.75, 1);
            DoubleCollection scaleValues = Graphics.GenerateDoubleCollection(0, 1, 0.5, 1);
            DoubleCollection translateXValues = Graphics.GenerateDoubleCollection(pointGrid.DesiredSize.Width / 2, 0, pointGrid.DesiredSize.Width / 4, 0);
            DoubleCollection translateYValues = Graphics.GenerateDoubleCollection(pointGrid.DesiredSize.Height / 2, 0, pointGrid.DesiredSize.Height / 4, 0);
            List<KeySpline> splines1 = AnimationHelper.GenerateKeySplineList(new Point(0, 0.5), new Point(0.5, 1), new Point(0, 0.5), new Point(0.5, 1), new Point(0, 0.5), new Point(0.5, 1), new Point(0, 0.5), new Point(0.5, 1));
            List<KeySpline> splines2 = AnimationHelper.GenerateKeySplineList(new Point(0, 0.5), new Point(0.5, 1), new Point(0, 0.5), new Point(0.5, 1), new Point(0, 0.5), new Point(0.5, 1), new Point(0, 0.5), new Point(0.5, 1));
            List<KeySpline> splines3 = AnimationHelper.GenerateKeySplineList(new Point(0, 0.5), new Point(0.5, 1), new Point(0, 0.5), new Point(0.5, 1), new Point(0, 0.5), new Point(0.5, 1), new Point(0, 0.5), new Point(0.5, 1));
            List<KeySpline> splines4 = AnimationHelper.GenerateKeySplineList(new Point(0, 0.5), new Point(0.5, 1), new Point(0, 0.5), new Point(0.5, 1), new Point(0, 0.5), new Point(0.5, 1), new Point(0, 0.5), new Point(0.5, 1));

            DoubleAnimationUsingKeyFrames xScaleAnimation = AnimationHelper.CreateDoubleAnimation(currentDataSeries, scaleTransform, "(ScaleTransform.ScaleX)", begin + 0.5, times, scaleValues, splines1);
            DoubleAnimationUsingKeyFrames yScaleAnimation = AnimationHelper.CreateDoubleAnimation(currentDataSeries, scaleTransform, "(ScaleTransform.ScaleY)", begin + 0.5, times, scaleValues, splines2);
            DoubleAnimationUsingKeyFrames xTranslateAnimation = AnimationHelper.CreateDoubleAnimation(currentDataSeries, translateTransform, "(TranslateTransform.X)", begin + 0.5, times, translateXValues, splines3);
            DoubleAnimationUsingKeyFrames yTranslateAnimation = AnimationHelper.CreateDoubleAnimation(currentDataSeries, translateTransform, "(TranslateTransform.Y)", begin + 0.5, times, translateYValues, splines4);

            storyboard.Children.Add(xScaleAnimation);
            storyboard.Children.Add(yScaleAnimation);
            storyboard.Children.Add(xTranslateAnimation);
            storyboard.Children.Add(yTranslateAnimation);

            return storyboard;
        }

        #endregion

        #region Internal Methods

        // Canvas bubleChartCanvas, DataPoint dataPoint, Double minimumZVal, Double maximumZVal, Double plotWidth, Double plotHeight
        private static void CreateOrUpdateAPointDataPoint(Canvas pointChartCanvas, DataPoint dataPoint, Double plotAreaWidth, Double plotAreaHeight)
        {   
            Faces dpFaces = dataPoint.Faces;

            // Remove preexisting dataPoint visual and label visual
            if (dpFaces != null && dpFaces.Visual != null && pointChartCanvas == dpFaces.Visual.Parent)
            {
                pointChartCanvas.Children.Remove(dataPoint.Faces.Visual);
                //dpFaces = null;
            }

            dataPoint.Faces = null;
            dpFaces = new Faces();

            if (Double.IsNaN(dataPoint.InternalYValue) || (dataPoint.Enabled == false))
                return;
            
            Chart chart = dataPoint.Chart as Chart;
            PlotGroup plotGroup = dataPoint.Parent.PlotGroup;

            Double xPosition = Graphics.ValueToPixelPosition(0, plotAreaWidth, (Double)plotGroup.AxisX.InternalAxisMinimum, (Double)plotGroup.AxisX.InternalAxisMaximum, dataPoint.InternalXValue);
            Double yPosition = Graphics.ValueToPixelPosition(plotAreaHeight, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, dataPoint.InternalYValue);

            Brush markerColor = dataPoint.Color;
            //markerColor = (chart.View3D ? Graphics.GetLightingEnabledBrush3D(markerColor) :
            //    ((Boolean)dataPoint.LightingEnabled ? Graphics.GetLightingEnabledBrush(markerColor, "Linear", null) : markerColor));
            
            markerColor = (chart.View3D ? Graphics.Get3DBrushLighting(dataPoint.Color, (Boolean)dataPoint.LightingEnabled) :
                ((Boolean)dataPoint.LightingEnabled ? Graphics.GetLightingEnabledBrush(markerColor, "Linear", null) : markerColor));

            Size markerSize = new Size((Double)dataPoint.MarkerSize, (Double)dataPoint.MarkerSize);
            Boolean markerBevel = false;
            String labelText = (Boolean)dataPoint.LabelEnabled ? dataPoint.TextParser(dataPoint.LabelText) : "";
            Marker marker = new Marker((MarkerTypes)dataPoint.MarkerType, (Double)dataPoint.MarkerScale, markerSize, markerBevel, markerColor, labelText);

            marker.Tag = new ElementData() { Element = dataPoint };

            marker.ShadowEnabled = (Boolean)dataPoint.ShadowEnabled;

            if (VisifireControl.IsXbapApp)
                marker.PixelLavelShadow = false;
            else
                marker.PixelLavelShadow = true;

            marker.MarkerSize = new Size((Double)dataPoint.MarkerSize, (Double)dataPoint.MarkerSize);

            if (marker.MarkerType != MarkerTypes.Cross)
            {
                if (dataPoint.BorderColor != null)
                    marker.BorderColor = dataPoint.BorderColor;
            }
            else
                marker.BorderColor = markerColor;
            marker.BorderThickness = ((Thickness)dataPoint.MarkerBorderThickness).Left;

            if (!String.IsNullOrEmpty(labelText))
            {
                marker.FontColor = Chart.CalculateDataPointLabelFontColor(chart, dataPoint, dataPoint.LabelFontColor, LabelStyles.OutSide);
                marker.FontSize = (Double)dataPoint.LabelFontSize;
                marker.FontWeight = (FontWeight)dataPoint.LabelFontWeight;
                marker.FontFamily = dataPoint.LabelFontFamily;
                marker.FontStyle = (FontStyle)dataPoint.LabelFontStyle;
                marker.TextBackground = dataPoint.LabelBackground;

                marker.TextAlignmentX = AlignmentX.Center;
                marker.TextAlignmentY = AlignmentY.Center;

                if (!Double.IsNaN(dataPoint.LabelAngle) && dataPoint.LabelAngle != 0)
                {
                    marker.LabelAngle = dataPoint.LabelAngle;
                    marker.TextOrientation = Orientation.Vertical;

                    marker.TextAlignmentX = AlignmentX.Center;
                    marker.TextAlignmentY = AlignmentY.Center;

                    marker.LabelStyle = (LabelStyles)dataPoint.LabelStyle;
                }

                marker.CreateVisual();

                if (Double.IsNaN(dataPoint.LabelAngle) || dataPoint.LabelAngle == 0)
                {
                    if ((yPosition - marker.TextBlockSize.Height / 2) < 0)
                        marker.TextAlignmentY = AlignmentY.Bottom;
                    else if ((yPosition + marker.TextBlockSize.Height / 2) > plotAreaHeight)
                        marker.TextAlignmentY = AlignmentY.Top;

                    if ((xPosition - marker.TextBlockSize.Width / 2) < 0)
                        marker.TextAlignmentX = AlignmentX.Right;
                    else if ((xPosition + marker.TextBlockSize.Width / 2) > plotAreaWidth)
                        marker.TextAlignmentX = AlignmentX.Left;
                }
            }

            //marker.LabelEnabled =(Boolean) dataPoint.LabelEnabled;
            //marker.TextBackground = dataPoint.LabelBackground;
            //marker.FontColor = Chart.CalculateDataPointLabelFontColor(chart, dataPoint, dataPoint.LabelFontColor, LabelStyles.OutSide);
            //marker.FontSize = (Double)dataPoint.LabelFontSize;
            //marker.FontWeight = (FontWeight)dataPoint.LabelFontWeight;
            //marker.FontFamily = dataPoint.LabelFontFamily;
            //marker.FontStyle = (FontStyle)dataPoint.LabelFontStyle;

            //marker.TextAlignmentX = AlignmentX.Center;
            //marker.TextAlignmentY = AlignmentY.Center;

            marker.CreateVisual();

            marker.Visual.Opacity = dataPoint.Opacity * dataPoint.Parent.Opacity;

            marker.AddToParent(pointChartCanvas, xPosition, yPosition, new Point(0.5, 0.5));

            dataPoint._visualPosition = new Point(xPosition, yPosition);

            dpFaces.VisualComponents.Add(marker.Visual);
            dpFaces.Visual = marker.Visual;

            dpFaces.BorderElements.Add(marker.MarkerShape);

            dataPoint.Marker = marker;
            dataPoint.Faces = dpFaces;
            
            dataPoint.Faces.Visual.Opacity = dataPoint.Opacity * dataPoint.Parent.Opacity;
            dataPoint.AttachEvent2DataPointVisualFaces(dataPoint);
            dataPoint.AttachEvent2DataPointVisualFaces(dataPoint.Parent);
            dataPoint._parsedToolTipText = dataPoint.TextParser(dataPoint.ToolTipText);
            if(!chart.IndicatorEnabled)
                dataPoint.AttachToolTip(chart, dataPoint, dataPoint.Faces.VisualComponents);
            dataPoint.AttachHref(chart, dataPoint.Faces.VisualComponents, dataPoint.Href, (HrefTargets)dataPoint.HrefTarget);
            dataPoint.SetCursor2DataPointVisualFaces();

            if (dataPoint.Parent.SelectionEnabled && dataPoint.Selected)
                dataPoint.Select(true);
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
        internal static Canvas GetVisualObjectForPointChart(Panel preExistingPanel, Double plotAreaWidth, Double plotAreaHeight, PlotDetails plotDetails, List<DataSeries> seriesList, Chart chart, Double plankDepth, bool animationEnabled)
        {
            if (Double.IsNaN(plotAreaWidth) || Double.IsNaN(plotAreaHeight) || plotAreaWidth <= 0 || plotAreaHeight <= 0) return null;

            Canvas visual, pointChartCanvas; // pointChartCanvas holds all points for all series

            RenderHelper.RepareCanvas4Drawing(preExistingPanel as Canvas, out visual, out pointChartCanvas, plotAreaWidth, plotAreaHeight);
            
            Double depth3d = plankDepth / (plotDetails.Layer3DCount == 0 ? 1 : plotDetails.Layer3DCount) * (chart.View3D ? 1 : 0);
            Double visualOffset = depth3d * (plotDetails.SeriesDrawingIndex[seriesList[0]] + 1 - (plotDetails.Layer3DCount == 0 ? 0 : 1));
            visual.SetValue(Canvas.TopProperty, visualOffset);
            visual.SetValue(Canvas.LeftProperty, -visualOffset);

            foreach (DataSeries series in seriesList)
            {
                series.Faces = new Faces() { Visual = pointChartCanvas };

                if (series.Enabled == false)
                    continue;

                List<DataPoint> dataPointsInViewPort = RenderHelper.GetDataPointsUnderViewPort(series, false);

                foreach (DataPoint dataPoint in dataPointsInViewPort)
                {
                    CreateOrUpdateAPointDataPoint(pointChartCanvas, dataPoint, plotAreaWidth, plotAreaHeight);

                    // Apply initial animation
                    if (animationEnabled && dataPoint.Marker != null)
                    {   
                        if (dataPoint.Parent.Storyboard == null)
                            dataPoint.Parent.Storyboard = new Storyboard();

                        // Apply animation to the points
                        dataPoint.Parent.Storyboard = ApplyPointChartAnimation(dataPoint.Parent, dataPoint.Marker.Visual, dataPoint.Parent.Storyboard, plotAreaWidth, plotAreaHeight);
                    }
                }
            }

            Double tickLengthOfAxisX = (from tick in chart.AxesX[0].Ticks
                                        where (Boolean)chart.AxesX[0].Enabled && (Boolean)tick.Enabled
                                        select tick.TickLength).Sum();

            if (tickLengthOfAxisX == 0)
                tickLengthOfAxisX = 5;

            Double tickLengthOfPrimaryAxisY = (from axis in chart.AxesY
                                               where axis.AxisType == AxisTypes.Primary
                                               from tick in axis.Ticks
                                               where (Boolean)axis.Enabled && (Boolean)tick.Enabled
                                               select tick.TickLength).Sum();

            if (tickLengthOfPrimaryAxisY == 0)
                tickLengthOfPrimaryAxisY = 8;

            Double tickLengthOfSecondaryAxisY = (from axis in chart.AxesY
                                                 where axis.AxisType == AxisTypes.Secondary
                                                 from tick in axis.Ticks
                                                 where (Boolean)axis.Enabled && (Boolean)tick.Enabled
                                                 select tick.TickLength).Sum();

            if (tickLengthOfSecondaryAxisY == 0)
                tickLengthOfSecondaryAxisY = 8;

            Double plotGroupCount = (from c in chart.PlotDetails.PlotGroups
                                     where c.AxisY.AxisType == AxisTypes.Secondary
                                     select c).Count();

            RectangleGeometry clipRectangle = new RectangleGeometry();
            clipRectangle.Rect = new Rect(-tickLengthOfPrimaryAxisY, -chart.ChartArea.PLANK_DEPTH - 4, plotAreaWidth + tickLengthOfSecondaryAxisY + (plotGroupCount > 0 ? tickLengthOfPrimaryAxisY : 8) + chart.ChartArea.PLANK_OFFSET, plotAreaHeight + chart.ChartArea.PLANK_DEPTH + chart.ChartArea.PLANK_THICKNESS + tickLengthOfAxisX + 4);
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

        private static void UpdateDataSeries(DataSeries dataSeries, VcProperties property, object newValue, Boolean isAxisChanged)
        {
            Chart chart = dataSeries.Chart as Chart;
            switch (property)
            {
                case VcProperties.DataPoints:
                    //case VcProperties.YValues:
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataPoint"></param>
        /// <param name="property"></param>
        /// <param name="newValue"></param>
        /// <param name="isAxisChanged"></param>
        private static void UpdateDataPoint(DataPoint dataPoint, VcProperties property, object newValue, Boolean isAxisChanged)
        {
            Chart chart = dataPoint.Chart as Chart;
            PlotDetails plotDetails = chart.PlotDetails;
            Marker marker = dataPoint.Marker;
            DataSeries dataSeries = dataPoint.Parent;

            Canvas pointChartCanvas = dataSeries.Faces.Visual as Canvas;

            Double plotHeight = chart.ChartArea.ChartVisualCanvas.Height;
            Double plotWidth = chart.ChartArea.ChartVisualCanvas.Width;
            ColumnChart.UpdateParentVisualCanvasSize(chart, pointChartCanvas);

            if (property == VcProperties.Enabled || (dataPoint.Faces == null && (property == VcProperties.XValue || property == VcProperties.YValue)))
            {
                CreateOrUpdateAPointDataPoint(pointChartCanvas, dataPoint, plotWidth, plotHeight);
                return;
            }

            if (dataPoint.Faces == null)
                return;

            //Grid bubbleVisual = dataPoint.Faces.Visual as Grid;
            
            switch (property)
            {
                case VcProperties.Bevel:
                    break;

                case VcProperties.Cursor:
                    break;

                case VcProperties.Href:
                    dataPoint.SetHref2DataPointVisualFaces();
                    break;

                case VcProperties.HrefTarget:
                    dataPoint.SetHref2DataPointVisualFaces();
                    break;

                case VcProperties.LabelBackground:
                    if (marker != null)
                        marker.TextBackground = dataPoint.LabelBackground;
                    break;

                case VcProperties.LabelEnabled:
                    CreateOrUpdateAPointDataPoint(pointChartCanvas, dataPoint, plotWidth, plotHeight);
                    //if (marker != null)
                    //    marker.LabelEnabled = (Boolean)dataPoint.LabelEnabled;
                    break;

                case VcProperties.LabelFontColor:
                    if (marker != null)
                        marker.FontColor = dataPoint.LabelFontColor;

                    break;

                case VcProperties.LabelFontFamily:
                    if (marker != null)
                        marker.FontFamily = dataPoint.LabelFontFamily;
                    break;

                case VcProperties.LabelFontStyle:
                    if (marker != null)
                        marker.FontStyle = (FontStyle)dataPoint.LabelFontStyle;
                    break;

                //case VcProperties.LabelFontSize:
                //    if (marker != null)
                //        marker.FontSize = (Double)dataPoint.LabelFontSize;
                //    break;

                case VcProperties.LabelFontWeight:
                    if (marker != null)
                        marker.FontWeight = (FontWeight)dataPoint.LabelFontWeight;
                    break;

                case VcProperties.LabelAngle:
                    if (marker != null)
                        marker.FontWeight = (FontWeight)dataPoint.LabelFontWeight;
                    break;

                case VcProperties.LegendText:
                    chart.InvokeRender();
                    break;

                case VcProperties.Color:
                case VcProperties.LightingEnabled:
                    if (marker != null)
                        marker.MarkerShape.Fill = (chart.View3D ? Graphics.Get3DBrushLighting(dataPoint.Color, (Boolean)dataPoint.LightingEnabled) : ((Boolean)dataPoint.LightingEnabled ? Graphics.GetLightingEnabledBrush(dataPoint.Color, "Linear", null) : dataPoint.Color));
                    break;

                case VcProperties.MarkerBorderColor:
                    if (marker != null)
                        marker.BorderColor = dataPoint.MarkerBorderColor;
                    break;

                case VcProperties.MarkerBorderThickness:
                    if (marker != null)
                        marker.BorderThickness = dataPoint.MarkerBorderThickness.Value.Left;
                    break;

                case VcProperties.MarkerColor:
                    if (marker != null)
                        marker.MarkerFillColor = dataPoint.MarkerColor;
                    break;

                case VcProperties.LabelFontSize:
                case VcProperties.LabelStyle:
                case VcProperties.LabelText:
                case VcProperties.MarkerScale:
                case VcProperties.MarkerSize:
                case VcProperties.MarkerType:
                    CreateOrUpdateAPointDataPoint(pointChartCanvas, dataPoint, plotWidth, plotHeight);
                    break;

                case VcProperties.ShadowEnabled:
                    if (marker != null)
                    {
                        marker.ShadowEnabled = (Boolean)dataPoint.ShadowEnabled;
                        marker.ApplyRemoveShadow();
                    }

                    break;

                case VcProperties.Opacity:
                    if (marker != null)
                        marker.Visual.Opacity = dataPoint.Opacity * dataSeries.Opacity;
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
                    CreateOrUpdateAPointDataPoint(pointChartCanvas, dataPoint, plotWidth, plotHeight);
                    break;

                case VcProperties.XValueType:
                    chart.InvokeRender();
                    break;
                    
                case VcProperties.XValue:
                case VcProperties.YValue:
                case VcProperties.YValues:
                case VcProperties.DataPoints:
                    if (isAxisChanged)
                        UpdateDataSeries(dataSeries, property, newValue, false);
                    else
                        if (marker != null)
                        {   
                            dataPoint._parsedToolTipText = dataPoint.TextParser(dataPoint.ToolTipText);
                            
                            if((Boolean)dataPoint.LabelEnabled)
                                marker.Text = dataPoint.TextParser(dataPoint.LabelText);

                            BubbleChart.UpdateBubblePositionAccording2XandYValue(dataPoint, plotWidth, plotHeight, (Boolean)chart.AnimatedUpdate, marker.MarkerShape.Width, marker.MarkerShape.Width);
                        }

                    break;
            }

            if (pointChartCanvas.Parent != null)
            {
                Double tickLengthOfAxisX = (from tick in chart.AxesX[0].Ticks
                                            where (Boolean)chart.AxesX[0].Enabled && (Boolean)tick.Enabled
                                            select tick.TickLength).Sum();

                if (tickLengthOfAxisX == 0)
                    tickLengthOfAxisX = 5;

                Double tickLengthOfPrimaryAxisY = (from axis in chart.AxesY
                                                   where axis.AxisType == AxisTypes.Primary
                                                   from tick in axis.Ticks
                                                   where (Boolean)axis.Enabled && (Boolean)tick.Enabled
                                                   select tick.TickLength).Sum();

                if (tickLengthOfPrimaryAxisY == 0)
                    tickLengthOfPrimaryAxisY = 8;

                Double tickLengthOfSecondaryAxisY = (from axis in chart.AxesY
                                                     where axis.AxisType == AxisTypes.Secondary
                                                     from tick in axis.Ticks
                                                     where (Boolean)axis.Enabled && (Boolean)tick.Enabled
                                                     select tick.TickLength).Sum();

                if (tickLengthOfSecondaryAxisY == 0)
                    tickLengthOfSecondaryAxisY = 8;

                Double plotGroupCount = (from c in chart.PlotDetails.PlotGroups
                                         where c.AxisY.AxisType == AxisTypes.Secondary
                                         select c).Count();

                RectangleGeometry clipRectangle = new RectangleGeometry();
                clipRectangle.Rect = new Rect(-tickLengthOfPrimaryAxisY, -chart.ChartArea.PLANK_DEPTH - 4, plotWidth + tickLengthOfSecondaryAxisY + (plotGroupCount > 0 ? tickLengthOfPrimaryAxisY : 8) + chart.ChartArea.PLANK_OFFSET, plotHeight + chart.ChartArea.PLANK_DEPTH + chart.ChartArea.PLANK_THICKNESS + tickLengthOfAxisX + 4);
                (pointChartCanvas.Parent as Canvas).Clip = clipRectangle;
            }
        }

        #endregion

        #region Internal Events And Delegates

        #endregion

        #region Data

        #endregion
    }
}