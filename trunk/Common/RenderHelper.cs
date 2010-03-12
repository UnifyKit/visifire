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
        internal static Panel GetVisualObject(Panel preExistingPanel, RenderAs chartType, Double width, Double height, PlotDetails plotDetails, List<DataSeries> dataSeriesList4Rendering, Chart chart, Double plankDepth, bool animationEnabled)
        {
            Panel renderedCanvas = null;

            switch (chartType)
            {
                case RenderAs.Column:
                    renderedCanvas = ColumnChart.GetVisualObjectForColumnChart(preExistingPanel, width, height, plotDetails, dataSeriesList4Rendering, chart, plankDepth, animationEnabled);
                    break;

                case RenderAs.Bar:
                    renderedCanvas = ColumnChart.GetVisualObjectForColumnChart(preExistingPanel, width, height, plotDetails, dataSeriesList4Rendering, chart, plankDepth, animationEnabled);
                   
                    //renderedCanvas = BarChart.GetVisualObjectForBarChart(preExistingPanel, width, height, plotDetails, dataSeriesList4Rendering, chart, plankDepth, animationEnabled);
                    break;

                case RenderAs.Line:
                    renderedCanvas = LineChart.GetVisualObjectForLineChart(preExistingPanel, width, height, plotDetails, dataSeriesList4Rendering, chart, plankDepth, animationEnabled);
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

                case RenderAs.Stock:
                    renderedCanvas = StockChart.GetVisualObjectForStockChart(preExistingPanel, width, height, plotDetails, dataSeriesList4Rendering, chart, plankDepth, animationEnabled);
                    break;

                case RenderAs.CandleStick:
                    renderedCanvas = CandleStick.GetVisualObjectForCandleStick(preExistingPanel, width, height, plotDetails, dataSeriesList4Rendering, chart, plankDepth, animationEnabled);
                    break;

                case RenderAs.StepLine:
                    renderedCanvas = StepLineChart.GetVisualObjectForLineChart(preExistingPanel, width, height, plotDetails, dataSeriesList4Rendering, chart, plankDepth, animationEnabled);
                    break;

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

            drawingCanvas =  new Canvas() { Width = width, Height = height };
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
            if (Double.IsNaN(chart.ActualWidth) || Double.IsNaN(chart.ActualHeight) || chart.ActualWidth == 0 || chart.ActualHeight == 0)
                return;

            if (partialUpdate && chart._datapoint2UpdatePartially.Count <= 500)
            {   
                chart.PARTIAL_DP_RENDER_LOCK = false;
                Boolean isNeed2UpdateAllSeries = false;

                foreach(KeyValuePair<DataPoint, VcProperties>  dpInfo in chart._datapoint2UpdatePartially)
                {   
                    DataPoint dp = dpInfo.Key;

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

            chart.ChartArea.PrePartialUpdateConfiguration(chart, VcProperties.None, null, null, false, true, true, AxisRepresentations.AxisY, true);
            
            //chart.ChartArea.RenderSeries();
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
                        //ColumnChart.Update(chart, currentRenderAs, selectedDataSeries4Rendering, property, newValue);
                        break;

                     //renderedCanvas = BarChart.GetVisualObjectForBarChart(width, height, plotDetails, dataSeriesList4Rendering, chart, plankDepth, animationEnabled);

                  case RenderAs.Line:

                        if(property == VcProperties.Enabled)
                            ColumnChart.Update(chart, currentRenderAs, selectedDataSeries4Rendering);
                            //LineChart.Update(chart, currentRenderAs, selectedDataSeries4Rendering, property, newValue);
                        else
                            //ColumnChart.Update(chart, currentRenderAs, selectedDataSeries4Rendering);

                            foreach(DataSeries ds in selectedDataSeries4Rendering)
                                LineChart.Update(ds, property, newValue, false);
                        break;

                  case RenderAs.StepLine:

                        if (property == VcProperties.Enabled)
                            ColumnChart.Update(chart, currentRenderAs, selectedDataSeries4Rendering);
                        //LineChart.Update(chart, currentRenderAs, selectedDataSeries4Rendering, property, newValue);
                        else
                            //ColumnChart.Update(chart, currentRenderAs, selectedDataSeries4Rendering);

                            foreach (DataSeries ds in selectedDataSeries4Rendering)
                                StepLineChart.Update(ds, property, newValue, false);
                        break;


                    case RenderAs.Point:

                        //foreach (DataSeries ds in selectedDataSeries4Rendering)
                        //{
                        //    foreach (DataPoint dp in ds.InternalDataPoints)
                        //    {
                        //        RenderHelper.UpdateVisualObject(ds.RenderAs, dp, property, newValue, false);
                        //    }
                        //}

                        foreach (DataSeries ds in selectedDataSeries4Rendering)
                            PointChart.Update(ds, property, newValue, false);

                        // ColumnChart.Update(chart, currentRenderAs, selectedDataSeries4Rendering);

                        break;

                    case RenderAs.Bubble:

                        //foreach (DataSeries ds in selectedDataSeries4Rendering)
                        //{
                        //    foreach (DataPoint dp in ds.InternalDataPoints)
                        //    {
                        //        RenderHelper.UpdateVisualObject(ds.RenderAs, dp, property, newValue, false);
                        //    }
                        //}

                        foreach (DataSeries ds in selectedDataSeries4Rendering)
                            BubbleChart.Update(ds, property, newValue, false);

                        // ColumnChart.Update(chart, currentRenderAs, selectedDataSeries4Rendering);

                        // BubbleChart.Update(chart, currentRenderAs, selectedDataSeries4Rendering, property, newValue);

                        //renderedCanvas = BubbleChart.GetVisualObjectForBubbleChart(width, height, plotDetails, dataSeriesList4Rendering, chart, plankDepth, animationEnabled);
                        break;

                    case RenderAs.Area:
                        ColumnChart.Update(chart, currentRenderAs, selectedDataSeries4Rendering);

                       // AreaChart.Update(chart, currentRenderAs, selectedDataSeries4Rendering, property, newValue);
                        //renderedCanvas = AreaChart.GetVisualObjectForAreaChart(width, height, plotDetails, dataSeriesList4Rendering, chart, plankDepth, animationEnabled);
                        break;

                    case RenderAs.StackedColumn:
                        ColumnChart.Update(chart, currentRenderAs, selectedDataSeries4Rendering);

                        //ColumnChart.Update(chart, currentRenderAs, selectedDataSeries4Rendering, property, newValue);
                        //renderedCanvas = ColumnChart.GetVisualObjectForStackedColumnChart(width, height, plotDetails, chart, plankDepth, animationEnabled);
                        break;

                    case RenderAs.StackedColumn100:
                        ColumnChart.Update(chart, currentRenderAs, selectedDataSeries4Rendering);

                        //ColumnChart.Update(chart, currentRenderAs, selectedDataSeries4Rendering, property, newValue);

                         //renderedCanvas = ColumnChart.GetVisualObjectForStackedColumn100Chart(width, height, plotDetails, chart, plankDepth, animationEnabled);
                        break;

                    case RenderAs.StackedBar:
                        ColumnChart.Update(chart, currentRenderAs, selectedDataSeries4Rendering);

                        //ColumnChart.Update(chart, currentRenderAs, selectedDataSeries4Rendering, property, newValue);
                        //renderedCanvas = BarChart.GetVisualObjectForStackedBarChart(width, height, plotDetails, chart, plankDepth, animationEnabled);
                        break;

                    case RenderAs.StackedBar100:
                        ColumnChart.Update(chart, currentRenderAs, selectedDataSeries4Rendering);

                        //ColumnChart.Update(chart, currentRenderAs, selectedDataSeries4Rendering, property, newValue);

                        //renderedCanvas = BarChart.GetVisualObjectForStackedBar100Chart(width, height, plotDetails, chart, plankDepth, animationEnabled);
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
                        //renderedCanvas = AreaChart.GetVisualObjectForStackedAreaChart(width, height, plotDetails, dataSeriesList4Rendering, chart, plankDepth, animationEnabled);
                        break;

                    case RenderAs.StackedArea100:
                        ColumnChart.Update(chart, currentRenderAs, selectedDataSeries4Rendering);
                        chart.ChartArea.AttachEventsToolTipHref2DataSeries();
                        //renderedCanvas = AreaChart.GetVisualObjectForStackedArea100Chart(width, height, plotDetails, dataSeriesList4Rendering, chart, plankDepth, animationEnabled);
                        break;

                    case RenderAs.SectionFunnel:
                       // renderedCanvas = FunnelChart.GetVisualObjectForFunnelChart(width, height, plotDetails, dataSeriesList4Rendering, chart, animationEnabled, false);

                        break;

                    case RenderAs.StreamLineFunnel:
                        //renderedCanvas = FunnelChart.GetVisualObjectForFunnelChart(width, height, plotDetails, dataSeriesList4Rendering, chart, animationEnabled, true);
                        break;

                    case RenderAs.Stock:
                        ColumnChart.Update(chart, currentRenderAs, selectedDataSeries4Rendering);

                        //StockChart.Update(chart, currentRenderAs, selectedDataSeries4Rendering, property, newValue);
                       // renderedCanvas = StockChart.GetVisualObjectForStockChart(width, height, plotDetails, dataSeriesList4Rendering, chart, plankDepth, animationEnabled);
                        break;

                    case RenderAs.CandleStick:
                        ColumnChart.Update(chart, currentRenderAs, selectedDataSeries4Rendering);
                       // CandleStick.Update(chart, currentRenderAs, selectedDataSeries4Rendering, property, newValue);
                       // renderedCanvas = CandleStick.GetVisualObjectForCandleStick(width, height, plotDetails, dataSeriesList4Rendering, chart, plankDepth, animationEnabled);
                        break;
               
                }

                //renderedChart = RenderSeriesFromList(selectedDataSeries4Rendering);

                renderedSeriesCount += selectedDataSeries4Rendering.Count;
            }

            chart.ChartArea.AttachScrollEvents();
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
                    //ColumnChart.GetVisualObjectForColumnChart(width, height, plotDetails, dataSeriesList4Rendering, chart, plankDepth, animationEnabled);
                    break;
                    
                    // renderedCanvas = BarChart.GetVisualObjectForBarChart(width, height, plotDetails, dataSeriesList4Rendering, chart, plankDepth, animationEnabled);

                case RenderAs.Line:

                    //if (isAXisChanged && isDataPoint && chart._partialUpdateAnimation)
                    //{   foreach (DataSeries ds in chart.Series)
                    //    {   
                    //        //if (ds == dataPoint.Parent)
                    //        //    continue;
                            
                    //        foreach (DataPoint dp in ds.DataPoints)
                    //        {   
                    //            RenderHelper.UpdateVisualObject(ds.RenderAs, dp, property, newValue, false);
                    //        }
                    //    }
                    //}
                    //else
                    //    LineChart.Update(sender, property, newValue);

                    LineChart.Update(sender, property, newValue, isAXisChanged);
                    
                    break;

                case RenderAs.StepLine:

                    //if (isAXisChanged && isDataPoint && chart._partialUpdateAnimation)
                    //{   foreach (DataSeries ds in chart.Series)
                    //    {   
                    //        //if (ds == dataPoint.Parent)
                    //        //    continue;

                    //        foreach (DataPoint dp in ds.DataPoints)
                    //        {   
                    //            RenderHelper.UpdateVisualObject(ds.RenderAs, dp, property, newValue, false);
                    //        }
                    //    }
                    //}
                    //else
                    //    LineChart.Update(sender, property, newValue);

                    StepLineChart.Update(sender, property, newValue, isAXisChanged);

                    break;

                case RenderAs.Point:
                    PointChart.Update(sender, property, newValue, isAXisChanged);
                    //renderedCanvas = PointChart.GetVisualObjectForPointChart(width, height, plotDetails, dataSeriesList4Rendering, chart, plankDepth, animationEnabled);
                    break;

                case RenderAs.Bubble:
                    BubbleChart.Update(sender, property, newValue, isAXisChanged);
                    //renderedCanvas = BubbleChart.GetVisualObjectForBubbleChart(width, height, plotDetails, dataSeriesList4Rendering, chart, plankDepth, animationEnabled);
                    break;

                case RenderAs.Area:
                    AreaChart.Update(sender, property, newValue, isAXisChanged);
                    //renderedCanvas = AreaChart.GetVisualObjectForAreaChart(width, height, plotDetails, dataSeriesList4Rendering, chart, plankDepth, animationEnabled);
                    break;

                case RenderAs.StackedColumn:
                    ColumnChart.Update(sender, property, newValue, isAXisChanged);
                    //renderedCanvas = ColumnChart.GetVisualObjectForStackedColumnChart(width, height, plotDetails, chart, plankDepth, animationEnabled);
                    break;

                case RenderAs.StackedColumn100:
                    ColumnChart.Update(sender, property, newValue, isAXisChanged);
                   // renderedCanvas = ColumnChart.GetVisualObjectForStackedColumn100Chart(width, height, plotDetails, chart, plankDepth, animationEnabled);
                    break;

                case RenderAs.StackedBar:
                    ColumnChart.Update(sender, property, newValue, isAXisChanged);
                    //renderedCanvas = BarChart.GetVisualObjectForStackedBarChart(width, height, plotDetails, chart, plankDepth, animationEnabled);
                    break;

                case RenderAs.StackedBar100:
                    ColumnChart.Update(sender, property, newValue, isAXisChanged);
                    //renderedCanvas = BarChart.GetVisualObjectForStackedBar100Chart(width, height, plotDetails, chart, plankDepth, animationEnabled);
                    break;

                case RenderAs.Pie:
                    //renderedCanvas = PieChart.GetVisualObjectForPieChart(width, height, plotDetails, dataSeriesList4Rendering, chart, animationEnabled);
                    break;

                case RenderAs.Doughnut:
                    //renderedCanvas = PieChart.GetVisualObjectForDoughnutChart(width, height, plotDetails, dataSeriesList4Rendering, chart, animationEnabled);
                    break;

                case RenderAs.StackedArea:
                    //if (isDataPoint)
                    //    sender = (sender as DataPoint).Parent;

                    List<DataSeries> stackedAreaList = (from ds in chart.Series where ds.RenderAs == RenderAs.StackedArea select ds).ToList();

                    if (stackedAreaList.Count > 0)
                        ColumnChart.Update(chart, stackedAreaList[0].RenderAs, stackedAreaList);

                    chart.ChartArea.AttachEventsToolTipHref2DataSeries();

                    //ColumnChart.Update(sender, property, newValue, isAXisChanged);
                    //renderedCanvas = AreaChart.GetVisualObjectForStackedAreaChart(width, height, plotDetails, dataSeriesList4Rendering, chart, plankDepth, animationEnabled);
                    break;

                case RenderAs.StackedArea100:
                    //if (isDataPoint)
                    //    sender = (sender as DataPoint).Parent;
                    //ColumnChart.Update(sender, property, newValue, isAXisChanged);

                    List<DataSeries> stackedArea100List = (from ds in chart.Series where ds.RenderAs == RenderAs.StackedArea select ds).ToList();

                    if (stackedArea100List.Count > 0)
                        ColumnChart.Update(chart, stackedArea100List[0].RenderAs, stackedArea100List);

                    chart.ChartArea.AttachEventsToolTipHref2DataSeries();
                    //renderedCanvas = AreaChart.GetVisualObjectForStackedArea100Chart(width, height, plotDetails, dataSeriesList4Rendering, chart, plankDepth, animationEnabled);
                    break;

                case RenderAs.SectionFunnel:
                    //renderedCanvas = FunnelChart.GetVisualObjectForFunnelChart(width, height, plotDetails, dataSeriesList4Rendering, chart, animationEnabled, false);
                    break;

                case RenderAs.StreamLineFunnel:
                    //renderedCanvas = FunnelChart.GetVisualObjectForFunnelChart(width, height, plotDetails, dataSeriesList4Rendering, chart, animationEnabled, true);
                    break;

                case RenderAs.Stock:
                    StockChart.Update(sender, property, newValue, isAXisChanged);
                    //renderedCanvas = StockChart.GetVisualObjectForStockChart(width, height, plotDetails, dataSeriesList4Rendering, chart, plankDepth, animationEnabled);
                    break;

                case RenderAs.CandleStick:
                    CandleStick.Update(sender, property, newValue, isAXisChanged);
                    //renderedCanvas = CandleStick.GetVisualObjectForCandleStick(width, height, plotDetails, dataSeriesList4Rendering, chart, plankDepth, animationEnabled);
                    break;
            }

            chart.ChartArea.AttachScrollEvents();
        }

   }
}