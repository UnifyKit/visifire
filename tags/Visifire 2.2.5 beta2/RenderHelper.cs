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

namespace Visifire.Charts
{
    public static class RenderHelper
    {
        internal static Panel GetVisualObject(RenderAs chartType, Double width, Double height, PlotDetails plotDetails, List<DataSeries> dataSeriesList4Rendering, Chart chart, Double plankDepth, bool animationEnabled)
        {
            Panel renderedCanvas = null;

            switch (chartType)
            {
                case RenderAs.Column:
                    renderedCanvas = ColumnChart.GetVisualObjectForColumnChart(width, height, plotDetails, dataSeriesList4Rendering, chart, plankDepth, animationEnabled);
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
                    renderedCanvas = ColumnChart.GetVisualObjectForStackedColumnChart(width, height, plotDetails, chart, plankDepth, animationEnabled);
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

    }
}