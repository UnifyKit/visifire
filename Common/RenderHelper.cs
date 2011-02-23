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
using System.Collections.Generic;
using Visifire.Commons;
using System.Linq;
using System.Reflection;

namespace Visifire.Charts
{
    public static class RenderHelper
    {

        internal static void ResetMarkersForSeries(List<DataSeries> dataSeriesList4Rendering)
        {
            foreach (DataSeries ds in dataSeriesList4Rendering)
            {
                foreach (DataPoint dp in ds.DataPoints)
                {
                    dp.Marker = null;
                }
            }
        }

        internal static Panel GetVisualObject(Panel preExistingPanel, RenderAs chartType, Double width, Double height, PlotDetails plotDetails, List<DataSeries> dataSeriesList4Rendering, Chart chart, Double plankDepth, bool animationEnabled)
        {
            Panel renderedCanvas = null;

            ResetMarkersForSeries(dataSeriesList4Rendering);

            switch (chartType)
            {
                case RenderAs.Column:
                    renderedCanvas = ColumnChart.GetVisualObjectForColumnChart(preExistingPanel, width, height, plotDetails, dataSeriesList4Rendering, chart, plankDepth, animationEnabled);
                    break;

                case RenderAs.Bar:
                    renderedCanvas = ColumnChart.GetVisualObjectForColumnChart(preExistingPanel, width, height, plotDetails, dataSeriesList4Rendering, chart, plankDepth, animationEnabled);
                    // renderedCanvas = BarChart.GetVisualObjectForBarChart(preExistingPanel, width, height, plotDetails, dataSeriesList4Rendering, chart, plankDepth, animationEnabled);
                    break;

                case RenderAs.Line:
                case RenderAs.Spline:
                    renderedCanvas = LineChart.GetVisualObjectForLineChart(preExistingPanel, width, height, plotDetails, dataSeriesList4Rendering, chart, plankDepth, animationEnabled);
                    break;

                case RenderAs.QuickLine:
                    renderedCanvas = QuickLineChart.GetVisualObjectForQuickLineChart(preExistingPanel, width, height, plotDetails, dataSeriesList4Rendering, chart, plankDepth, animationEnabled);
                    break;

                case RenderAs.Point:
                    renderedCanvas = PointChart.GetVisualObjectForPointChart(preExistingPanel, width, height, plotDetails, dataSeriesList4Rendering, chart, plankDepth, animationEnabled);
                    break;

                case RenderAs.Bubble:
                    renderedCanvas = BubbleChart.GetVisualObjectForBubbleChart(preExistingPanel, width, height, plotDetails, dataSeriesList4Rendering, chart, plankDepth, animationEnabled);
                    break;

                case RenderAs.Area:
                    renderedCanvas = AreaChart.GetVisualObjectForAreaChart(preExistingPanel, width, height, plotDetails, dataSeriesList4Rendering, chart, plankDepth, animationEnabled);
                    break;

                case RenderAs.StackedColumn:
                    renderedCanvas = ColumnChart.GetVisualObjectForStackedColumnChart(chartType, preExistingPanel, width, height, plotDetails, chart, plankDepth, animationEnabled);
                    break;

                case RenderAs.StackedColumn100:
                    renderedCanvas = ColumnChart.GetVisualObjectForStackedColumnChart(chartType, preExistingPanel, width, height, plotDetails, chart, plankDepth, animationEnabled);

                    //renderedCanvas = ColumnChart.GetVisualObjectForStackedColumn100Chart(width, height, plotDetails, chart, plankDepth, animationEnabled);
                    break;

                case RenderAs.StackedBar:
                    renderedCanvas = BarChart.GetVisualObjectForStackedBarChart(chartType, preExistingPanel, width, height, plotDetails, chart, plankDepth, animationEnabled);
                    break;

                case RenderAs.StackedBar100:
                    renderedCanvas = BarChart.GetVisualObjectForStackedBarChart(chartType, preExistingPanel, width, height, plotDetails, chart, plankDepth, animationEnabled);

                    // renderedCanvas = BarChart.GetVisualObjectForStackedBar100Chart(width, height, plotDetails, chart, plankDepth, animationEnabled);
                    break;

                case RenderAs.Pie:
                    renderedCanvas = PieChart.GetVisualObjectForPieChart(width, height, plotDetails, dataSeriesList4Rendering, chart, animationEnabled);
                    break;

                case RenderAs.Doughnut:
                    renderedCanvas = PieChart.GetVisualObjectForDoughnutChart(width, height, plotDetails, dataSeriesList4Rendering, chart, animationEnabled);
                    break;

                case RenderAs.Radar:
                    renderedCanvas = RadarChart.GetVisualObjectForRadarChart(width, height, plotDetails, dataSeriesList4Rendering, chart, animationEnabled);
                    break;

                case RenderAs.Polar:
                    renderedCanvas = PolarChart.GetVisualObjectForPolarChart(width, height, plotDetails, dataSeriesList4Rendering, chart, animationEnabled);
                    break;

                case RenderAs.StackedArea:
                    renderedCanvas = AreaChart.GetVisualObjectForStackedAreaChart(preExistingPanel, width, height, plotDetails, dataSeriesList4Rendering, chart, plankDepth, animationEnabled);
                    break;

                case RenderAs.StackedArea100:
                    renderedCanvas = AreaChart.GetVisualObjectForStackedArea100Chart(preExistingPanel, width, height, plotDetails, dataSeriesList4Rendering, chart, plankDepth, animationEnabled);
                    break;

                case RenderAs.SectionFunnel:
                    renderedCanvas = FunnelChart.GetVisualObjectForFunnelChart(width, height, plotDetails, dataSeriesList4Rendering, chart, animationEnabled, false);

                    break;

                case RenderAs.StreamLineFunnel:
                    renderedCanvas = FunnelChart.GetVisualObjectForFunnelChart(width, height, plotDetails, dataSeriesList4Rendering, chart, animationEnabled, true);
                    break;

                case RenderAs.Pyramid:
                    renderedCanvas = PyramidChart.GetVisualObjectForPyramidChart(width, height, plotDetails, dataSeriesList4Rendering, chart, animationEnabled);
                    break;

                case RenderAs.Stock:
                    renderedCanvas = StockChart.GetVisualObjectForStockChart(preExistingPanel, width, height, plotDetails, dataSeriesList4Rendering, chart, plankDepth, animationEnabled);
                    break;

                case RenderAs.CandleStick:
                    renderedCanvas = CandleStick.GetVisualObjectForCandleStick(preExistingPanel, width, height, plotDetails, dataSeriesList4Rendering, chart, plankDepth, animationEnabled);
                    break;

                case RenderAs.StepLine:
                    renderedCanvas = StepLineChart.GetVisualObjectForLineChart(preExistingPanel, width, height, plotDetails, dataSeriesList4Rendering, chart, plankDepth, animationEnabled);
                    break;
                #region Custom Chart Nortek Added
                case RenderAs.HeatMap:
                    renderedCanvas = HeatMap.GetVisualObjectForHeatChart(preExistingPanel, width, height, dataSeriesList4Rendering[0], chart);
                    break;

                case RenderAs.RadarHeatMap:
                    renderedCanvas = HeatMap.GetVisualObjectForRadarHeatMap(preExistingPanel, width, height, dataSeriesList4Rendering[0], chart);
                    break;
                #endregion

            }

            return renderedCanvas;
        }

        internal static void RepareCanvas4Drawing(Canvas preExistingPanel, out Canvas visual, out Canvas labelCanvas, out Canvas drawingCanvas, Double width, Double height)
        {
            if (preExistingPanel != null)
            {
                visual = preExistingPanel as Canvas;
                labelCanvas = preExistingPanel.Children[0] as Canvas;
                labelCanvas.Children.Clear();
            }
            else
            {
                visual = new Canvas();
                labelCanvas = new Canvas();
            }

            labelCanvas.Width = width;
            labelCanvas.Height = height;

            visual.Width = width;
            visual.Height = height;

            drawingCanvas = new Canvas() { Width = width, Height = height };
        }

        internal static void RepareCanvas4Drawing(Canvas preExistingPanel, out Canvas visual, out Canvas drawingCanvas, Double width, Double height)
        {
            if (preExistingPanel != null)
            {
                visual = preExistingPanel as Canvas;
                visual.Children.Clear();
            }
            else
            {
                visual = new Canvas();
            }

            visual.Width = width;
            visual.Height = height;

            drawingCanvas = new Canvas() { Width = width, Height = height };
            visual.Children.Add(drawingCanvas);
        }

        internal static void UpdateVisualObject(Chart chart, VcProperties property, object newValue, Boolean partialUpdate)
        {
            if (!chart._internalPartialUpdateEnabled || Double.IsNaN(chart.ActualWidth) || Double.IsNaN(chart.ActualHeight) || chart.ActualWidth == 0 || chart.ActualHeight == 0)
                return;

            if (partialUpdate && chart._datapoint2UpdatePartially.Count <= 500)
            {
                chart.PARTIAL_DP_RENDER_LOCK = false;
                Boolean isNeed2UpdateAllSeries = false;

                List<DataSeries> _dataSeries2UpdateAtSeriesWise = (from value in chart._datapoint2UpdatePartially
                                                                   where value.Key.Parent.RenderAs == RenderAs.Spline || value.Key.Parent.RenderAs == RenderAs.QuickLine
                                                                   select value.Key.Parent).Distinct().ToList();

                foreach (DataSeries splineDs in _dataSeries2UpdateAtSeriesWise)
                    splineDs.UpdateVisual(VcProperties.DataPointUpdate, null);

                List<KeyValuePair<DataPoint, VcProperties>> remainingDpInfo = (from dpInfo in chart._datapoint2UpdatePartially where !_dataSeries2UpdateAtSeriesWise.Contains(dpInfo.Key.Parent) select dpInfo).ToList();

                // If there is nothing to render anymore
                if (remainingDpInfo.Count == 0)
                    return;

                foreach (KeyValuePair<DataPoint, VcProperties> dpInfo in remainingDpInfo)
                {
                    DataPoint dp = dpInfo.Key;

                    if (dp.Parent.RenderAs == RenderAs.Spline || dp.Parent.RenderAs == RenderAs.QuickLine)
                    {
                        isNeed2UpdateAllSeries = false;
                        continue;
                    }

                    if (dpInfo.Value == VcProperties.XValue)
                    {
                        isNeed2UpdateAllSeries = true;
                        break;
                    }

                    PropertyInfo pInfo = dp.GetType().GetProperty(dpInfo.Value.ToString());
                    newValue = pInfo.GetValue(dp, null);

                    isNeed2UpdateAllSeries = dpInfo.Key.UpdateVisual(dpInfo.Value, newValue, true);

                    if (isNeed2UpdateAllSeries)
                        break;
                }

                if (!isNeed2UpdateAllSeries)
                    return;
            }

            chart.ChartArea.PrePartialUpdateConfiguration(chart, Visifire.Charts.ElementTypes.Chart, VcProperties.None, null, null, false, true, true, AxisRepresentations.AxisY, true);

            Int32 renderedSeriesCount = 0;      // Contain count of series that have been already rendered

            // Contains a list of serties as per the drawing order generated in the plotdetails
            List<DataSeries> dataSeriesListInDrawingOrder = chart.PlotDetails.SeriesDrawingIndex.Keys.ToList();

            List<DataSeries> selectedDataSeries4Rendering;          // Contains a list of serries to be rendered in a rendering cycle
            Int32 currentDrawingIndex;                              // Drawing index of the selected series 
            RenderAs currentRenderAs;                               // Rendereas type of the selected series

            // This loop will select series for rendering and it will repeat until all series have been rendered
            while (renderedSeriesCount < chart.InternalSeries.Count)
            {
                selectedDataSeries4Rendering = new List<DataSeries>();

                currentRenderAs = dataSeriesListInDrawingOrder[renderedSeriesCount].RenderAs;

                currentDrawingIndex = chart.PlotDetails.SeriesDrawingIndex[dataSeriesListInDrawingOrder[renderedSeriesCount]];

                for (Int32 i = renderedSeriesCount; i < chart.InternalSeries.Count; i++)
                {
                    DataSeries ds = dataSeriesListInDrawingOrder[i];
                    if (currentRenderAs == ds.RenderAs && currentDrawingIndex == chart.PlotDetails.SeriesDrawingIndex[ds])
                        selectedDataSeries4Rendering.Add(ds);
                }

                if (selectedDataSeries4Rendering.Count == 0)
                    break;

                chart._toolTip.Hide();

                if (selectedDataSeries4Rendering.Count > 0)
                {
                    if (selectedDataSeries4Rendering[0].ToolTipElement != null)
                        selectedDataSeries4Rendering[0].ToolTipElement.Hide();
                }

                chart.ChartArea.DisableIndicators();

                switch (currentRenderAs)
                {
                    case RenderAs.Column:
                    case RenderAs.Bar:
                        ColumnChart.Update(chart, currentRenderAs, selectedDataSeries4Rendering);
                        break;

                    case RenderAs.Spline:
                    case RenderAs.QuickLine:
                        if (property == VcProperties.Enabled)
                            ColumnChart.Update(chart, currentRenderAs, selectedDataSeries4Rendering);
                        else
                            foreach (DataSeries ds in selectedDataSeries4Rendering)
                                UpdateVisualObject(ds.RenderAs, ds, property, newValue, false);

                        break;

                    case RenderAs.Line:
                        if (property == VcProperties.Enabled)
                            ColumnChart.Update(chart, currentRenderAs, selectedDataSeries4Rendering);
                        else
                            foreach (DataSeries ds in selectedDataSeries4Rendering)
                                LineChart.Update(ds, property, newValue, false);
                        break;

                    case RenderAs.StepLine:

                        if (property == VcProperties.Enabled)
                            ColumnChart.Update(chart, currentRenderAs, selectedDataSeries4Rendering);
                        else
                            foreach (DataSeries ds in selectedDataSeries4Rendering)
                                StepLineChart.Update(ds, property, newValue, false);
                        break;


                    case RenderAs.Point:
                        if (property == VcProperties.ViewportRangeEnabled)
                            ColumnChart.Update(chart, currentRenderAs, selectedDataSeries4Rendering);
                        else
                        {
                            foreach (DataSeries ds in selectedDataSeries4Rendering)
                                PointChart.Update(ds, property, newValue, false);
                        }
                        break;

                    case RenderAs.Bubble:
                        if (property == VcProperties.ViewportRangeEnabled)
                            ColumnChart.Update(chart, currentRenderAs, selectedDataSeries4Rendering);
                        else
                        {
                            foreach (DataSeries ds in selectedDataSeries4Rendering)
                                BubbleChart.Update(ds, property, newValue, false);
                        }
                        break;

                    case RenderAs.Area:
                        ColumnChart.Update(chart, currentRenderAs, selectedDataSeries4Rendering);
                        break;

                    case RenderAs.StackedColumn:
                        ColumnChart.Update(chart, currentRenderAs, selectedDataSeries4Rendering);
                        break;

                    case RenderAs.StackedColumn100:
                        ColumnChart.Update(chart, currentRenderAs, selectedDataSeries4Rendering);
                        break;

                    case RenderAs.StackedBar:
                        ColumnChart.Update(chart, currentRenderAs, selectedDataSeries4Rendering);
                        break;

                    case RenderAs.StackedBar100:
                        ColumnChart.Update(chart, currentRenderAs, selectedDataSeries4Rendering);
                        break;

                    case RenderAs.Pie:
                        //renderedCanvas = PieChart.GetVisualObjectForPieChart(width, height, plotDetails, dataSeriesList4Rendering, chart, animationEnabled);
                        break;

                    case RenderAs.Doughnut:
                        //renderedCanvas = PieChart.GetVisualObjectForDoughnutChart(width, height, plotDetails, dataSeriesList4Rendering, chart, animationEnabled);
                        break;

                    case RenderAs.StackedArea:
                        ColumnChart.Update(chart, currentRenderAs, selectedDataSeries4Rendering);
                        chart.ChartArea.AttachEventsToolTipHref2DataSeries();
                        break;

                    case RenderAs.StackedArea100:
                        ColumnChart.Update(chart, currentRenderAs, selectedDataSeries4Rendering);
                        chart.ChartArea.AttachEventsToolTipHref2DataSeries();
                        break;

                    case RenderAs.SectionFunnel:
                    case RenderAs.StreamLineFunnel:
                    case RenderAs.Pyramid:
                        //renderedCanvas = FunnelChart.GetVisualObjectForFunnelChart(width, height, plotDetails, dataSeriesList4Rendering, chart, animationEnabled, true);
                        break;

                    case RenderAs.Stock:
                        ColumnChart.Update(chart, currentRenderAs, selectedDataSeries4Rendering);
                        break;

                    case RenderAs.CandleStick:
                        ColumnChart.Update(chart, currentRenderAs, selectedDataSeries4Rendering);
                        break;
                }

                renderedSeriesCount += selectedDataSeries4Rendering.Count;
            }

            chart.ChartArea.AttachScrollBarOffsetChangedEventWithAxes();

            chart.ChartArea.AttachOrDetachIntaractivity(chart.InternalSeries);
            Visifire.Charts.Chart.SelectDataPoints(chart);
            //AttachEventsToolTipHref2DataSeries();
        }

        internal static void UpdateVisualObject(RenderAs chartType, Visifire.Commons.ObservableObject sender, VcProperties property, object newValue, Boolean isAXisChanged)
        {
            Boolean isDataPoint = sender.GetType().Equals(typeof(DataPoint));
            DataPoint dataPoint = sender as DataPoint;
            Chart chart = (sender as ObservableObject).Chart as Chart;
            switch (chartType)
            {
                case RenderAs.Column:
                case RenderAs.Bar:
                    ColumnChart.Update(sender, property, newValue, isAXisChanged);
                    break;

                case RenderAs.Line:
                case RenderAs.Spline:
                    LineChart.Update(sender, property, newValue, isAXisChanged);

                    break;
                case RenderAs.QuickLine:
                    QuickLineChart.Update(sender, property, newValue, isAXisChanged);

                    break;
                case RenderAs.StepLine:
                    StepLineChart.Update(sender, property, newValue, isAXisChanged);

                    break;

                case RenderAs.Point:
                    PointChart.Update(sender, property, newValue, isAXisChanged);
                    break;

                case RenderAs.Bubble:
                    BubbleChart.Update(sender, property, newValue, isAXisChanged);
                    break;

                case RenderAs.Area:
                    AreaChart.Update(sender, property, newValue, isAXisChanged);
                    break;

                case RenderAs.StackedColumn:
                    ColumnChart.Update(sender, property, newValue, isAXisChanged);
                    break;

                case RenderAs.StackedColumn100:
                    ColumnChart.Update(sender, property, newValue, isAXisChanged);
                    break;

                case RenderAs.StackedBar:
                    ColumnChart.Update(sender, property, newValue, isAXisChanged);
                    break;

                case RenderAs.StackedBar100:
                    ColumnChart.Update(sender, property, newValue, isAXisChanged);
                    break;

                case RenderAs.Pie:
                    //renderedCanvas = PieChart.GetVisualObjectForPieChart(width, height, plotDetails, dataSeriesList4Rendering, chart, animationEnabled);
                    break;

                case RenderAs.Doughnut:
                    //renderedCanvas = PieChart.GetVisualObjectForDoughnutChart(width, height, plotDetails, dataSeriesList4Rendering, chart, animationEnabled);
                    break;

                case RenderAs.StackedArea:
                    List<DataSeries> stackedAreaList = (from ds in chart.Series where ds.RenderAs == RenderAs.StackedArea select ds).ToList();

                    if (stackedAreaList.Count > 0)
                        ColumnChart.Update(chart, stackedAreaList[0].RenderAs, stackedAreaList);

                    chart.ChartArea.AttachEventsToolTipHref2DataSeries();

                    break;

                case RenderAs.StackedArea100:
                    List<DataSeries> stackedArea100List = (from ds in chart.Series where ds.RenderAs == RenderAs.StackedArea select ds).ToList();

                    if (stackedArea100List.Count > 0)
                        ColumnChart.Update(chart, stackedArea100List[0].RenderAs, stackedArea100List);

                    chart.ChartArea.AttachEventsToolTipHref2DataSeries();
                    break;

                case RenderAs.SectionFunnel:
                case RenderAs.StreamLineFunnel:
                case RenderAs.Pyramid:
                    //renderedCanvas = FunnelChart.GetVisualObjectForFunnelChart(width, height, plotDetails, dataSeriesList4Rendering, chart, animationEnabled, true);
                    break;

                case RenderAs.Stock:
                    StockChart.Update(sender, property, newValue, isAXisChanged);
                    break;

                case RenderAs.CandleStick:
                    CandleStick.Update(sender, property, newValue, isAXisChanged);
                    break;
            }

            //chart.ChartArea.AttachScrollEvents();
        }

        internal static List<DataPoint> GetDataPointsUnderViewPort(List<DataPoint> dataPoints, Boolean isUsedForAxisRange, Int32 leftRightPointPadding)
        {
            if (dataPoints.Count > 0)
            {
                DataSeries dataSeries = dataPoints[0].Parent;
                if (dataSeries.PlotGroup.AxisY.ViewportRangeEnabled)
                {
                    PlotGroup plotGroup = dataSeries.PlotGroup;
                    Axis axisX = plotGroup.AxisX;
                    List<DataPoint> viewPortDataPoints;

                    Double minXValueRangeOfViewPort = axisX._numericViewMinimum;
                    Double maxXValueRangeOfViewPort = axisX._numericViewMaximum;

                    Double offset = Math.Abs(minXValueRangeOfViewPort - maxXValueRangeOfViewPort) * .4;
                    //minXValueRangeOfViewPort -= offset;
                    //maxXValueRangeOfViewPort += offset;

                    if (!isUsedForAxisRange)
                    {
                        var leftDataPoints = (from dp in dataPoints where (dp.InternalXValue < minXValueRangeOfViewPort) select dp.InternalXValue);
                        var rightDataPoints = (from dp in dataPoints where (dp.InternalXValue > maxXValueRangeOfViewPort) select dp.InternalXValue);

                        if (leftDataPoints.Count() > 0)
                            minXValueRangeOfViewPort = leftDataPoints.Max();
                        if (rightDataPoints.Count() > 0)
                            maxXValueRangeOfViewPort = rightDataPoints.Min();
                    }

                    viewPortDataPoints = dataPoints
                               .Where(d => d.InternalXValue >= minXValueRangeOfViewPort
                                   && d.InternalXValue <= maxXValueRangeOfViewPort).ToList();

                    System.Diagnostics.Debug.WriteLine("viewPortDataPoints=" + viewPortDataPoints.Count.ToString());

                    //
                    if (viewPortDataPoints.Count <= leftRightPointPadding)
                    {
                        var leftDataPoints = (from dp in dataPoints where (dp.InternalXValue < minXValueRangeOfViewPort) orderby dp.InternalXValue select dp);
                        List<DataPoint> rightDataPoints = (from dp in dataPoints where (dp.InternalXValue > maxXValueRangeOfViewPort) orderby dp.InternalXValue select dp).ToList();

                        if (leftDataPoints.Count() > 0)
                            viewPortDataPoints.Insert(0, leftDataPoints.Last());

                        if (rightDataPoints.Count > 0)
                            viewPortDataPoints.Add(rightDataPoints[0]);

                        if (rightDataPoints.Count > 1)
                            viewPortDataPoints.Add(rightDataPoints[1]);

                        if (rightDataPoints.Count > 2)
                            viewPortDataPoints.Add(rightDataPoints[2]);
                    }

                    return viewPortDataPoints;
                }
                else
                    return dataPoints;
            }
            else
                return dataPoints;
        }

        internal static List<DataPoint> GetDataPointsUnderViewPort(DataSeries dataSeries, Boolean isUsedForAxisRange)
        {
            if (dataSeries.PlotGroup.AxisY.ViewportRangeEnabled)
            {
                PlotGroup plotGroup = dataSeries.PlotGroup;
                Axis axisX = plotGroup.AxisX;
                List<DataPoint> viewPortDataPoints;

                Double minXValueRangeOfViewPort = axisX._numericViewMinimum;
                Double maxXValueRangeOfViewPort = axisX._numericViewMaximum;
                //Double offset = Math.Abs(minXValueRangeOfViewPort - maxXValueRangeOfViewPort) * .4;
                //minXValueRangeOfViewPort -= offset;
                //maxXValueRangeOfViewPort += offset;

                if (!isUsedForAxisRange)
                {
                    var leftDataPoints = (from dp in dataSeries.InternalDataPoints where (dp.InternalXValue < minXValueRangeOfViewPort) select dp.InternalXValue);
                    var rightDataPoints = (from dp in dataSeries.InternalDataPoints where (dp.InternalXValue > maxXValueRangeOfViewPort) select dp.InternalXValue);

                    if (leftDataPoints.Count() > 0)
                        minXValueRangeOfViewPort = leftDataPoints.Max();
                    if (rightDataPoints.Count() > 0)
                        maxXValueRangeOfViewPort = rightDataPoints.Min();
                }

                viewPortDataPoints = dataSeries.InternalDataPoints
                           .Where(d => d.InternalXValue >= minXValueRangeOfViewPort
                               && d.InternalXValue <= maxXValueRangeOfViewPort).ToList();

                if (viewPortDataPoints.Count <= 3)
                {
                    var leftDataPoints = (from dp in dataSeries.InternalDataPoints where (dp.InternalXValue < minXValueRangeOfViewPort) orderby dp.InternalXValue select dp);
                    List<DataPoint> rightDataPoints = (from dp in dataSeries.InternalDataPoints where (dp.InternalXValue > maxXValueRangeOfViewPort) orderby dp.InternalXValue select dp).ToList();

                    if (leftDataPoints.Count() > 0)
                        viewPortDataPoints.Insert(0, leftDataPoints.Last());

                    if (rightDataPoints.Count > 0)
                        viewPortDataPoints.Add(rightDataPoints[0]);

                    if (rightDataPoints.Count > 1)
                        viewPortDataPoints.Add(rightDataPoints[1]);

                    if (rightDataPoints.Count > 2)
                        viewPortDataPoints.Add(rightDataPoints[2]);
                }

                return viewPortDataPoints;
            }
            else
                return dataSeries.InternalDataPoints;
        }

        internal static Double[] GetXValuesUnderViewPort(List<Double> xValues, Axis axisX, Axis axisY, Boolean isUsedForAxisRange)
        {
            if (axisY.ViewportRangeEnabled)
            {
                List<Double> viewPortXValues;

                Double minXValueRangeOfViewPort = axisX._numericViewMinimum;
                Double maxXValueRangeOfViewPort = axisX._numericViewMaximum;
                //Double offset = Math.Abs(minXValueRangeOfViewPort - maxXValueRangeOfViewPort) * .4;
                //minXValueRangeOfViewPort -= offset;
                //maxXValueRangeOfViewPort += offset;

                if (!isUsedForAxisRange)
                {
                    var leftXValues = (from xValue in xValues where (xValue < minXValueRangeOfViewPort) select xValue);
                    var rightXValues = (from xValue in xValues where (xValue > maxXValueRangeOfViewPort) select xValue);

                    if (leftXValues.Count() > 0)
                        minXValueRangeOfViewPort = leftXValues.Max();
                    if (rightXValues.Count() > 0)
                        maxXValueRangeOfViewPort = rightXValues.Min();
                }

                viewPortXValues = xValues.Where(d =>
                    d >= minXValueRangeOfViewPort && d <= maxXValueRangeOfViewPort).ToList();

                if (viewPortXValues.Count <= 3)
                {
                    var leftDataPoints = (from xValue in xValues where (xValue < minXValueRangeOfViewPort) select xValue).Distinct();
                    List<Double> rightDataPoints = (from xValue in xValues where (xValue > maxXValueRangeOfViewPort) select xValue).Distinct().ToList();

                    if (leftDataPoints.Count() > 0)
                        viewPortXValues.Insert(0, leftDataPoints.Last());

                    if (rightDataPoints.Count > 0)
                        viewPortXValues.Add(rightDataPoints[0]);

                    if (rightDataPoints.Count > 1)
                        viewPortXValues.Add(rightDataPoints[1]);

                    if (rightDataPoints.Count > 2)
                        viewPortXValues.Add(rightDataPoints[2]);
                }

                return viewPortXValues.ToArray();
            }
            else
                return xValues.ToArray();
        }

        #region Helper function for Pixel to Value and Value to Pixel conversion purpose
        #region Nortek Add

        internal static double CalculatePixelPosFromInternalXValue(Chart chart, Axis xAxis, double xValue)
        {
            AxisOrientation axisOrientation = xAxis.AxisOrientation;
            Double lengthInPixel = ((axisOrientation == AxisOrientation.Horizontal) ? chart.ChartArea.ChartVisualCanvas.Width : chart.ChartArea.ChartVisualCanvas.Height);
            return xAxis.XValueToPixelPosition(lengthInPixel, (axisOrientation == AxisOrientation.Horizontal) ? xValue : xAxis.InternalAxisMaximum - xValue);
        }

        internal static double CalculatePixelPosFromInternalYValue(Chart chart, Axis yAxis, double yValue)
        {
            AxisOrientation axisOrientation = yAxis.AxisOrientation;
            Double lengthInPixel = ((axisOrientation == AxisOrientation.Vertical) ? chart.ChartArea.ChartVisualCanvas.Height : chart.ChartArea.ChartVisualCanvas.Width);

            return yAxis.YValueToPixelPosition(lengthInPixel, (axisOrientation == AxisOrientation.Vertical) ? yValue : yAxis.InternalAxisMaximum - yValue);
        }

        #endregion

        /// <summary>
        /// Calculate internalYValue from mouse pointer position
        /// </summary>
        /// <param name="chart">Chart</param>
        /// <param name="yAxis">y-axis reference</param>
        /// <param name="e">MouseEventArgs</param>
        /// <returns>Double internalXValue</returns>
        internal static Double CalculateInternalXValueFromPixelPos(Chart chart, Axis xAxis, MouseEventArgs e)
        {
            AxisOrientation axisOrientation = xAxis.AxisOrientation;
            Double pixelPosition = (axisOrientation == AxisOrientation.Horizontal) ? e.GetPosition(chart.ChartArea.PlottingCanvas).X : e.GetPosition(chart.ChartArea.PlottingCanvas).Y;
            Double lengthInPixel = ((axisOrientation == AxisOrientation.Horizontal) ? chart.ChartArea.ChartVisualCanvas.Width : chart.ChartArea.ChartVisualCanvas.Height);

            return xAxis.PixelPositionToXValue(lengthInPixel, (axisOrientation == AxisOrientation.Horizontal) ? pixelPosition : lengthInPixel - pixelPosition);
        }

        /// <summary>
        /// Calculate internalYValue from mouse pointer position
        /// </summary>
        /// <param name="chart">Chart</param>
        /// <param name="yAxis">y-axis reference</param>
        /// <param name="e">MouseEventArgs</param>
        /// <returns>Double internalYValue</returns>
        internal static Double CalculateInternalYValueFromPixelPos(Chart chart, Axis yAxis, MouseEventArgs e)
        {
            AxisOrientation axisOrientation = yAxis.AxisOrientation;
            Double pixelPosition = (axisOrientation == AxisOrientation.Vertical) ? e.GetPosition(chart.ChartArea.PlottingCanvas).Y : e.GetPosition(chart.ChartArea.PlottingCanvas).X;
            Double lengthInPixel = ((axisOrientation == AxisOrientation.Vertical) ? chart.ChartArea.ChartVisualCanvas.Height : chart.ChartArea.ChartVisualCanvas.Width);

            return yAxis.PixelPositionToYValue(lengthInPixel, (axisOrientation == AxisOrientation.Vertical) ? pixelPosition : lengthInPixel - pixelPosition);
        }

        /// <summary>
        /// Get nearest DataPoint along XAxis
        /// </summary>
        /// <param name="dataPoints"></param>
        /// <param name="internalXValue"></param>
        /// <returns></returns>
        internal static DataPoint GetNearestDataPointAlongXAxis(List<DataPoint> dataPoints, Axis xAxis, Double internalXValue)
        {
            List<DataPoint> dataPointsAlongX = (from dp in dataPoints
                                                where
                                                !(RenderHelper.IsFinancialCType(dp.Parent) && dp.InternalYValues == null)
                                                || !(!RenderHelper.IsFinancialCType(dp.Parent) && Double.IsNaN(dp.YValue))
                                                orderby Math.Abs(dp.InternalXValue - internalXValue)
                                                select dp).ToList();

            if (dataPointsAlongX.Count > 0)
                return dataPointsAlongX.First();
            else
                return null;
        }

        internal static void UserValueToInternalValues(Axis xAxis, Axis yAxis, Object userXValue, Double userYValue, out Double internalXValue, out Double internalYValue)
        {
            internalYValue = userYValue;
            internalXValue = Double.NaN;

            if (xAxis != null)
            {
                if (userXValue != null)
                {
                    if (xAxis.IsDateTimeAxis)
                    {
                        try
                        {
                            DateTime dateTime = Convert.ToDateTime(userXValue);
                            internalXValue = DateTimeHelper.DateDiff(dateTime, xAxis.FirstLabelDate, xAxis.MinDateRange, xAxis.MaxDateRange, xAxis.InternalIntervalType, xAxis.XValueType);
                        }
                        catch
                        {
                            throw new ArgumentException("Incorrect DateTime value of XValue.");
                        }
                    }
                    else
                        internalXValue = Convert.ToDouble(userXValue);
                }

            }

            if (yAxis != null && yAxis.Logarithmic)
                internalYValue = DataPoint.ConvertYValue2LogarithmicValue(xAxis.Chart as Chart, userYValue, yAxis.AxisType);
            else
                internalYValue = userYValue;
        }

        /// <summary>
        /// Find the nearest DataPoint from each dataSeries from a xValue and yValue
        /// </summary>
        /// <param name="dataSeries"></param>
        /// <param name="internalXValue">internalXValue</param>
        /// <param name="internalYValue">internalYValue</param>
        /// <returns></returns>
        internal static DataPoint GetNearestDataPoint(DataSeries dataSeries, Double xValueAtMousePos, Double yValueAtMousePos)
        {
            DataPoint nearestDataPoint = null;

            // Get all DataPoints order by the XValue distance from the internalXValue.
            List<DataPoint> dataPointsAlongX = (from dp in dataSeries.InternalDataPoints
                                                where
                                                !(RenderHelper.IsFinancialCType(dp.Parent) && dp.InternalYValues == null)
                                                || !(!RenderHelper.IsFinancialCType(dp.Parent) && Double.IsNaN(dp.YValue))
                                                orderby Math.Abs(dp.InternalXValue - xValueAtMousePos)
                                                select dp).ToList();

            if (dataPointsAlongX.Count > 0)
            {
                // Get the internalXValue of the first DataPoint of the ordered list
                Double xValue = dataPointsAlongX[0].InternalXValue;

                // DataPoints along y pixel direction which have same XValue
                List<DataPoint> dataPointsAlongYAxisHavingSameXValue = new List<DataPoint>();

                foreach (DataPoint dp in dataPointsAlongX)
                {
                    if (dp.InternalXValue == xValue)
                        dataPointsAlongYAxisHavingSameXValue.Add(dp);
                }

                if (!Double.IsNaN(yValueAtMousePos))
                {
                    // Sort according to YValue or YValues
                    if (RenderHelper.IsFinancialCType(dataSeries))
                        dataPointsAlongYAxisHavingSameXValue = (from dp in dataPointsAlongYAxisHavingSameXValue
                                                                where (dp.InternalYValues != null)
                                                                orderby Math.Abs(dp.InternalYValues.Max() - yValueAtMousePos)
                                                                select dp).ToList();
                    else
                        dataPointsAlongYAxisHavingSameXValue = (from dp in dataPointsAlongYAxisHavingSameXValue
                                                                where !Double.IsNaN(dp.InternalYValue)
                                                                orderby Math.Abs(dp.InternalYValue - yValueAtMousePos)
                                                                select dp).ToList();
                }

                if (dataPointsAlongYAxisHavingSameXValue.Count > 0)
                    nearestDataPoint = dataPointsAlongYAxisHavingSameXValue.First();
            }

            return nearestDataPoint;
        }

        #endregion

        /// <summary>
        /// (dataSeries.RenderAs == RenderAs.CandleStick || dataSeries.RenderAs == RenderAs.Stock);
        /// </summary>
        /// <param name="dataSeries"></param>
        /// <returns></returns>
        public static Boolean IsFinancialCType(DataSeries dataSeries)
        {
            return (dataSeries.RenderAs == RenderAs.CandleStick || dataSeries.RenderAs == RenderAs.Stock);
        }

        /// <summary>
        /// (dataSeries.RenderAs == RenderAs.Radar || dataSeries.RenderAs == RenderAs.Polar);
        /// </summary>
        /// <param name="dataSeries"></param>
        /// <returns></returns>
        public static Boolean IsCircularCType(DataSeries dataSeries)
        {
            return (dataSeries.RenderAs == RenderAs.Radar || dataSeries.RenderAs == RenderAs.Polar);
        }

        /// <summary>
        /// (dataSeries.RenderAs == RenderAs.Line 
        /// || dataSeries.RenderAs == RenderAs.StepLine 
        /// || dataSeries.RenderAs == RenderAs.StepLine)
        /// </summary>
        /// <param name="dataSeries"></param>
        /// <returns></returns>
        public static Boolean IsLineCType(DataSeries dataSeries)
        {
            return (dataSeries.RenderAs == RenderAs.Line
                || dataSeries.RenderAs == RenderAs.Spline
                || dataSeries.RenderAs == RenderAs.QuickLine
                || dataSeries.RenderAs == RenderAs.StepLine);
        }

        /// <summary>
        /// dataSeries.RenderAs == RenderAs.Area
        /// || dataSeries.RenderAs == RenderAs.StackedArea
        /// || dataSeries.RenderAs == RenderAs.StackedArea100
        /// </summary>
        /// <param name="dataSeries"></param>
        /// <returns></returns>
        public static Boolean IsAreaCType(DataSeries dataSeries)
        {
            return (dataSeries.RenderAs == RenderAs.Area
                || dataSeries.RenderAs == RenderAs.StackedArea
                || dataSeries.RenderAs == RenderAs.StackedArea100);
        }

        /// <summary>
        /// Chart types which are not dependent upon other DataSeries.
        /// (dataSeries.RenderAs == RenderAs.Area 
        /// || dataSeries.RenderAs == RenderAs.Column
        /// || dataSeries.RenderAs == RenderAs.Bar 
        /// || dataSeries.RenderAs == RenderAs.Line
        /// || dataSeries.RenderAs == RenderAs.StepLine)
        /// </summary>
        /// <param name="dataSeries"></param>
        /// <returns></returns>
        public static Boolean IsIndependentCType(DataSeries dataSeries)
        {
            return (dataSeries.RenderAs == RenderAs.Area
                || dataSeries.RenderAs == RenderAs.Bar
                || dataSeries.RenderAs == RenderAs.Bubble
                || dataSeries.RenderAs == RenderAs.Column
                || dataSeries.RenderAs == RenderAs.Spline
                || dataSeries.RenderAs == RenderAs.StepLine
                || dataSeries.RenderAs == RenderAs.Line
                || dataSeries.RenderAs == RenderAs.Point
                || dataSeries.RenderAs == RenderAs.CandleStick
                || dataSeries.RenderAs == RenderAs.Stock
                );
        }

        /// <summary>
        /// Chart types which works without axis
        /// (dataSeries.RenderAs == RenderAs.Pie 
        /// || dataSeries.RenderAs == RenderAs.Doughnut
        /// || dataSeries.RenderAs == RenderAs.SectionFunnel 
        /// || dataSeries.RenderAs == RenderAs.StreamLineFunnel)
        /// </summary>
        /// <param name="dataSeries"></param>
        /// <returns></returns>
        public static Boolean IsAxisIndependentCType(DataSeries dataSeries)
        {
            return (dataSeries.RenderAs == RenderAs.Pie
                || dataSeries.RenderAs == RenderAs.Doughnut
                || dataSeries.RenderAs == RenderAs.SectionFunnel
                || dataSeries.RenderAs == RenderAs.StreamLineFunnel
                || dataSeries.RenderAs == RenderAs.Pyramid);
        }

    }
}