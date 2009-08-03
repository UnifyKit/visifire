using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Silverlight.Testing;
using Visifire.Charts;
using Visifire.Commons;
namespace SLVisifireChartsTest
{
    /// <summary>
    /// This class runs the unit tests Visifire.Charts.ChartGrid class 
    /// </summary>
    [TestClass]
    public class ChartGridTest:SilverlightControlTest
    {
        #region CheckChartGridDefaultPropertyValue
        
        /// <summary>
        /// Check the default value of Enabled
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void EnabledDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                delegate
                {
                    if(chart.AxesX[0].Grids.Count > 0)
                        Assert.IsFalse((Boolean)chart.AxesX[0].Grids[0].Enabled);
                    Assert.IsTrue((Boolean)chart.AxesY[0].Grids[0].Enabled);
                });

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the default value of LineThickness
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void LineThicknessDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => Assert.AreEqual(0.25, chart.AxesY[0].Grids[0].LineThickness));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the default value of LineColor
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void LineColorDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Gray), chart.AxesY[0].Grids[0].LineColor));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the default value of LineStyle
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void LineStyleDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => Assert.AreEqual(LineStyles.Solid, chart.AxesY[0].Grids[0].LineStyle));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the default value of InterlacedColor
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void InterlacedColorDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => Common.AssertBrushesAreEqual(null, chart.AxesY[0].Grids[0].InterlacedColor));

            EnqueueTestComplete();
        }
        #endregion

        #region CheckChartGridNewPropertyValue
        /// <summary>
        /// Check the new value of Enabled. 
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of Enabled.")]
        [Owner("[....]")]
        [Asynchronous]
        public void EnabledNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            Axis axis = new Axis();
            ChartGrid grid = new ChartGrid();
            axis.Grids.Add(grid);
            chart.AxesX.Add(axis);

            CreateAsyncTask(chart,
                () => grid.Enabled = true,
                () => Assert.IsTrue((Boolean)grid.Enabled),
                () => chart.AxesY[0].Grids[0].Enabled = false,
                () => Assert.IsFalse((Boolean)chart.AxesY[0].Grids[0].Enabled));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of Opacity. 
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of Opacity.")]
        [Owner("[....]")]
        [Asynchronous]
        public void OpacityNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            Axis axis = new Axis();
            ChartGrid grid = new ChartGrid();
            axis.Grids.Add(grid);
            chart.AxesX.Add(axis);

            CreateAsyncTask(chart,
                () => grid.Opacity = 0.5,
                () => Assert.AreEqual(0.5, grid.Opacity, Common.HighPrecisionDelta),
                () => chart.AxesY[0].Grids[0].Opacity = 0.5,
                () => Assert.AreEqual(0.5, chart.AxesY[0].Grids[0].Opacity, Common.HighPrecisionDelta));
            

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of Interval. 
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of Interval.")]
        [Owner("[....]")]
        [Asynchronous]
        public void IntervalNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            Axis axis = new Axis();
            ChartGrid grid = new ChartGrid();
            axis.Grids.Add(grid);
            chart.AxesY.Add(axis);

            CreateAsyncTask(chart,
                () => grid.Interval = 20,
                () => Assert.AreEqual(20, grid.Interval));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of LineColor. 
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of LineColor.")]
        [Owner("[....]")]
        [Asynchronous]
        public void LineColorNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            Axis axis = new Axis();
            ChartGrid grid = new ChartGrid();
            axis.Grids.Add(grid);
            chart.AxesY.Add(axis);

            CreateAsyncTask(chart,
                () => grid.LineColor = new SolidColorBrush(Colors.Red),
                () => Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Red), grid.LineColor));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of LineThickness. 
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of LineThickness.")]
        [Owner("[....]")]
        [Asynchronous]
        public void LineThicknessNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            Axis axis = new Axis();
            ChartGrid grid = new ChartGrid();
            axis.Grids.Add(grid);
            chart.AxesY.Add(axis);

            CreateAsyncTask(chart,
                () => grid.LineThickness = 1,
                () => Assert.AreEqual(1, grid.LineThickness));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of LineStyle. 
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of LineStyle.")]
        [Owner("[....]")]
        [Asynchronous]
        public void LineStyleNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            Axis axis = new Axis();
            ChartGrid grid = new ChartGrid();
            axis.Grids.Add(grid);
            chart.AxesY.Add(axis);

            CreateAsyncTask(chart,
                () => grid.LineStyle = LineStyles.Dashed,
                () => Assert.AreEqual(LineStyles.Dashed, grid.LineStyle));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of InterlacedColor. 
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of InterlacedColor.")]
        [Owner("[....]")]
        [Asynchronous]
        public void InterlacedColorNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            Axis axis = new Axis();
            ChartGrid grid = new ChartGrid();
            axis.Grids.Add(grid);
            chart.AxesY.Add(axis);

            CreateAsyncTask(chart,
                () => grid.InterlacedColor = new SolidColorBrush(Colors.LightGray),
                () => Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.LightGray), grid.InterlacedColor));

            EnqueueTestComplete();
        }
        #endregion

        #region Private Data

        /// <summary>
        /// Number of milliseconds to wait between actions in CreateAsyncTasks or Enqueue callbacks. 
        /// </summary>
        private const int _sleepTime = 1000;

        #endregion
    }
}
