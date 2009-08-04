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
                    renderedCanvas = BarChart.GetVisualObjectForBarChart(width, height, plotDetails, dataSeriesList4Rendering, chart, plankDepth, animationEnabled);
                    break;

                case RenderAs.Line:
                    renderedCanvas = LineChart.GetVisualObjectForLineChart(width, height, plotDetails, dataSeriesList4Rendering, chart, plankDepth, animationEnabled);
                    break;

                case RenderAs.Point:
                    renderedCanvas = PointChart.GetVisualObjectForPointChart(width, height, plotDetails, dataSeriesList4Rendering, chart, plankDepth, animationEnabled);
                    break;

                case RenderAs.Bubble:
                    renderedCanvas = BubbleChart.GetVisualObjectForBubbleChart(width, height, plotDetails, dataSeriesList4Rendering, chart, plankDepth, animationEnabled);
                    break;

                case RenderAs.Area:
                    renderedCanvas = AreaChart.GetVisualObjectForAreaChart(width, height, plotDetails, dataSeriesList4Rendering, chart, plankDepth, animationEnabled);
                    break;

                case RenderAs.StackedColumn:
                    renderedCanvas = ColumnChart.GetVisualObjectForStackedColumnChart(preExistingPanel, width, height, plotDetails, chart, plankDepth, animationEnabled);
                    break;

                case RenderAs.StackedColumn100:
                    renderedCanvas = ColumnChart.GetVisualObjectForStackedColumn100Chart(width, height, plotDetails, chart, plankDepth, animationEnabled);
                    break;

                case RenderAs.StackedBar:
                    renderedCanvas = BarChart.GetVisualObjectForStackedBarChart(width, height, plotDetails, chart, plankDepth, animationEnabled);
                    break;

                case RenderAs.StackedBar100:
                    renderedCanvas = BarChart.GetVisualObjectForStackedBar100Chart(width, height, plotDetails, chart, plankDepth, animationEnabled);
                    break;

                case RenderAs.Pie:
                    renderedCanvas = PieChart.GetVisualObjectForPieChart(width, height, plotDetails, dataSeriesList4Rendering, chart, animationEnabled);
                    break;

                case RenderAs.Doughnut:
                    renderedCanvas = PieChart.GetVisualObjectForDoughnutChart(width, height, plotDetails, dataSeriesList4Rendering, chart, animationEnabled);
                    break;

                case RenderAs.StackedArea:
                    renderedCanvas = AreaChart.GetVisualObjectForStackedAreaChart(width, height, plotDetails, dataSeriesList4Rendering, chart, plankDepth, animationEnabled);
                    break;

                case RenderAs.StackedArea100:
                    renderedCanvas = AreaChart.GetVisualObjectForStackedArea100Chart(width, height, plotDetails, dataSeriesList4Rendering, chart, plankDepth, animationEnabled);
                    break;

                case RenderAs.SectionFunnel:
                    renderedCanvas = FunnelChart.GetVisualObjectForFunnelChart(width, height, plotDetails, dataSeriesList4Rendering, chart, animationEnabled, false);

                    break;

                case RenderAs.StreamLineFunnel:
                    renderedCanvas = FunnelChart.GetVisualObjectForFunnelChart(width, height, plotDetails, dataSeriesList4Rendering, chart, animationEnabled, true);
                    break;

                case RenderAs.Stock:
                    renderedCanvas = StockChart.GetVisualObjectForStockChart(width, height, plotDetails, dataSeriesList4Rendering, chart, plankDepth, animationEnabled);
                    break;

                case RenderAs.CandleStick:
                    renderedCanvas = CandleStick.GetVisualObjectForCandleStick(width, height, plotDetails, dataSeriesList4Rendering, chart, plankDepth, animationEnabled);
                    break;
            }

            return renderedCanvas;
        }

        internal static void UpdateVisualObject(Chart chart, VcProperties property, object newValue)
        {
            Int32 renderedSeriesCount = 0;      // Contain count of series that have been already rendered

            // Contains a list of serties as per the drawing order generated in the plotdetails
            List<DataSeries> dataSeriesListInDrawingOrder = chart.PlotDetails.SeriesDrawingIndex.Keys.ToList();

            List<DataSeries> selectedDataSeries4Rendering;          // Contains a list of serries to be rendered in a rendering cycle
            Int32 currentDrawingIndex;                              // Drawing index of the selected series 
            RenderAs currentRenderAs;                               // Rendereas type of the selected series
            Panel renderedChart;                                   // A canvas that contains the chart rendered using the selected series
            PlotDetails plotDetails = chart.PlotDetails;
            
            // This loop will select series for rendering and it will repeat until all series have been rendered
            while (renderedSeriesCount < chart.InternalSeries.Count)
            {
                selectedDataSeries4Rendering = new List<DataSeries>();

                currentRenderAs = dataSeriesListInDrawingOrder[renderedSeriesCount].RenderAs;

                currentDrawingIndex = chart.PlotDetails.SeriesDrawingIndex[dataSeriesListInDrawingOrder[renderedSeriesCount]];

                for (Int32 i = renderedSeriesCount; i < chart.InternalSeries.Count; i++)
                {
                    DataSeries ds = dataSeriesListInDrawingOrder[i];
                    if (currentRenderAs == ds.RenderAs && currentDrawingIndex == plotDetails.SeriesDrawingIndex[ds])
                        selectedDataSeries4Rendering.Add(ds);
                }

                if (selectedDataSeries4Rendering.Count == 0)
                    break;
                
                switch (currentRenderAs)
                {
                    case RenderAs.Column:
                        ColumnChart.Update(chart, currentRenderAs, selectedDataSeries4Rendering, property, newValue);
                        break;

                    case RenderAs.Bar:
                        // renderedCanvas = BarChart.GetVisualObjectForBarChart(width, height, plotDetails, dataSeriesList4Rendering, chart, plankDepth, animationEnabled);
                        break;

                    case RenderAs.Line:
                        foreach(DataSeries ds in selectedDataSeries4Rendering)
                            LineChart.Update(ds, property, newValue);
                        break;

                    case RenderAs.Point:
                        //renderedCanvas = PointChart.GetVisualObjectForPointChart(width, height, plotDetails, dataSeriesList4Rendering, chart, plankDepth, animationEnabled);
                        break;

                    case RenderAs.Bubble:
                        //renderedCanvas = BubbleChart.GetVisualObjectForBubbleChart(width, height, plotDetails, dataSeriesList4Rendering, chart, plankDepth, animationEnabled);
                        break;

                    case RenderAs.Area:
                        //renderedCanvas = AreaChart.GetVisualObjectForAreaChart(width, height, plotDetails, dataSeriesList4Rendering, chart, plankDepth, animationEnabled);
                        break;

                    case RenderAs.StackedColumn:
                        //renderedCanvas = ColumnChart.GetVisualObjectForStackedColumnChart(width, height, plotDetails, chart, plankDepth, animationEnabled);
                        break;

                    case RenderAs.StackedColumn100:
                        // renderedCanvas = ColumnChart.GetVisualObjectForStackedColumn100Chart(width, height, plotDetails, chart, plankDepth, animationEnabled);
                        break;

                    case RenderAs.StackedBar:
                        //renderedCanvas = BarChart.GetVisualObjectForStackedBarChart(width, height, plotDetails, chart, plankDepth, animationEnabled);
                        break;

                    case RenderAs.StackedBar100:
                        //renderedCanvas = BarChart.GetVisualObjectForStackedBar100Chart(width, height, plotDetails, chart, plankDepth, animationEnabled);
                        break;

                    case RenderAs.Pie:
                        //renderedCanvas = PieChart.GetVisualObjectForPieChart(width, height, plotDetails, dataSeriesList4Rendering, chart, animationEnabled);
                        break;

                    case RenderAs.Doughnut:
                        //renderedCanvas = PieChart.GetVisualObjectForDoughnutChart(width, height, plotDetails, dataSeriesList4Rendering, chart, animationEnabled);
                        break;

                    case RenderAs.StackedArea:
                        //renderedCanvas = AreaChart.GetVisualObjectForStackedAreaChart(width, height, plotDetails, dataSeriesList4Rendering, chart, plankDepth, animationEnabled);
                        break;

                    case RenderAs.StackedArea100:
                        //renderedCanvas = AreaChart.GetVisualObjectForStackedArea100Chart(width, height, plotDetails, dataSeriesList4Rendering, chart, plankDepth, animationEnabled);
                        break;

                    case RenderAs.SectionFunnel:
                        //renderedCanvas = FunnelChart.GetVisualObjectForFunnelChart(width, height, plotDetails, dataSeriesList4Rendering, chart, animationEnabled, false);

                        break;

                    case RenderAs.StreamLineFunnel:
                        //renderedCanvas = FunnelChart.GetVisualObjectForFunnelChart(width, height, plotDetails, dataSeriesList4Rendering, chart, animationEnabled, true);
                        break;

                    case RenderAs.Stock:
                        //renderedCanvas = StockChart.GetVisualObjectForStockChart(width, height, plotDetails, dataSeriesList4Rendering, chart, plankDepth, animationEnabled);
                        break;

                    case RenderAs.CandleStick:
                        //renderedCanvas = CandleStick.GetVisualObjectForCandleStick(width, height, plotDetails, dataSeriesList4Rendering, chart, plankDepth, animationEnabled);
                        break;
                }

                // renderedChart = RenderSeriesFromList(selectedDataSeries4Rendering);
                
                renderedSeriesCount += selectedDataSeries4Rendering.Count;
            }

            //AttachEventsToolTipHref2DataSeries();
        }

        internal static void UpdateVisualObject(RenderAs chartType, Visifire.Commons.ObservableObject sender, VcProperties property, object newValue, Boolean isAXisChanged)
        {
            Boolean isDataPoint = sender.GetType().Equals(typeof(DataPoint));

            switch (chartType)
            {   
                case RenderAs.Column:
                    ColumnChart.Update(sender, property, newValue, isAXisChanged);
                    //ColumnChart.GetVisualObjectForColumnChart(width, height, plotDetails, dataSeriesList4Rendering, chart, plankDepth, animationEnabled);
                    break;

                case RenderAs.Bar:
                    //renderedCanvas = BarChart.GetVisualObjectForBarChart(width, height, plotDetails, dataSeriesList4Rendering, chart, plankDepth, animationEnabled);
                    break;

                case RenderAs.Line:
                    LineChart.Update(sender, property, newValue);
                    break;

                case RenderAs.Point:
                    //renderedCanvas = PointChart.GetVisualObjectForPointChart(width, height, plotDetails, dataSeriesList4Rendering, chart, plankDepth, animationEnabled);
                    break;

                case RenderAs.Bubble:
                    //renderedCanvas = BubbleChart.GetVisualObjectForBubbleChart(width, height, plotDetails, dataSeriesList4Rendering, chart, plankDepth, animationEnabled);
                    break;

                case RenderAs.Area:
                    //renderedCanvas = AreaChart.GetVisualObjectForAreaChart(width, height, plotDetails, dataSeriesList4Rendering, chart, plankDepth, animationEnabled);
                    break;

                case RenderAs.StackedColumn:
                    ColumnChart.Update(sender, property, newValue, isAXisChanged);
                    //renderedCanvas = ColumnChart.GetVisualObjectForStackedColumnChart(width, height, plotDetails, chart, plankDepth, animationEnabled);
                    break;

                case RenderAs.StackedColumn100:
                   // renderedCanvas = ColumnChart.GetVisualObjectForStackedColumn100Chart(width, height, plotDetails, chart, plankDepth, animationEnabled);
                    break;

                case RenderAs.StackedBar:
                    //renderedCanvas = BarChart.GetVisualObjectForStackedBarChart(width, height, plotDetails, chart, plankDepth, animationEnabled);
                    break;

                case RenderAs.StackedBar100:
                    //renderedCanvas = BarChart.GetVisualObjectForStackedBar100Chart(width, height, plotDetails, chart, plankDepth, animationEnabled);
                    break;

                case RenderAs.Pie:
                    //renderedCanvas = PieChart.GetVisualObjectForPieChart(width, height, plotDetails, dataSeriesList4Rendering, chart, animationEnabled);
                    break;

                case RenderAs.Doughnut:
                    //renderedCanvas = PieChart.GetVisualObjectForDoughnutChart(width, height, plotDetails, dataSeriesList4Rendering, chart, animationEnabled);
                    break;

                case RenderAs.StackedArea:
                    //renderedCanvas = AreaChart.GetVisualObjectForStackedAreaChart(width, height, plotDetails, dataSeriesList4Rendering, chart, plankDepth, animationEnabled);
                    break;

                case RenderAs.StackedArea100:
                    //renderedCanvas = AreaChart.GetVisualObjectForStackedArea100Chart(width, height, plotDetails, dataSeriesList4Rendering, chart, plankDepth, animationEnabled);
                    break;

                case RenderAs.SectionFunnel:
                    //renderedCanvas = FunnelChart.GetVisualObjectForFunnelChart(width, height, plotDetails, dataSeriesList4Rendering, chart, animationEnabled, false);

                    break;

                case RenderAs.StreamLineFunnel:
                    //renderedCanvas = FunnelChart.GetVisualObjectForFunnelChart(width, height, plotDetails, dataSeriesList4Rendering, chart, animationEnabled, true);
                    break;

                case RenderAs.Stock:
                    //renderedCanvas = StockChart.GetVisualObjectForStockChart(width, height, plotDetails, dataSeriesList4Rendering, chart, plankDepth, animationEnabled);
                    break;

                case RenderAs.CandleStick:
                    //renderedCanvas = CandleStick.GetVisualObjectForCandleStick(width, height, plotDetails, dataSeriesList4Rendering, chart, plankDepth, animationEnabled);
                    break;
            }


        }

   }
}