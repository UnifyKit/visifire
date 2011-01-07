using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Silverlight.Testing;
using Visifire.Charts;
using Visifire.Commons;
using System.Windows.Browser;

namespace SLVisifireChartsTest
{
    /// <summary>
    /// This class runs the unit tests Visifire.Charts.TrendLine class 
    /// </summary>
    [TestClass]
    public class QuickLineTest : SilverlightControlTest
    {
        #region AddDataPointAtRealtime

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void QuickLineAddDataPointsAtRealtime()
        {
            Common.SetSLPluginHeight(500);

            Chart chart = CreatAQuickLineChartWithSingleSeries();

            EnqueueDelay(_sleepTime);

            Common.AddMessageButton(this, "Adding DataPoints At RealTime. Click Me to Stop and Continue.");

            Common.EnableAutoTimerCallBack(this, new TimeSpan(0, 0, 0, 0, 500), new TimeSpan(0, 0, 15),
               () =>
               {
                   chart.Series[0].DataPoints.Add(new DataPoint() { YValue = Graphics.RAND.Next(0, 100) });
               }
            );

            TestPanel.Children.Add(chart);
        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void QuickLineUpdateYValueAtRealtime()
        {
            Common.SetSLPluginHeight(500);

            Chart chart = CreatAQuickLineChartWithSingleSeries();

            EnqueueDelay(_sleepTime);

            Common.AddMessageButton(this, "Updating YValue property of DataPoints at RealTime. Click Me to Stop and Continue.");

            Common.EnableAutoTimerCallBack(this, new TimeSpan(0, 0, 0, 0, 500), new TimeSpan(0, 0, 5),
               () =>
               {
                   chart.Series[0].DataPoints[Graphics.RAND.Next(0, 3)].YValue = Graphics.RAND.Next(0, 100);
                   chart.Series[0].DataPoints[Graphics.RAND.Next(0, 3)].Color = new SolidColorBrush(Colors.Red);
               }
            );

            TestPanel.Children.Add(chart);
        }
        
        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void QuickLineSamplingThresholdInputValue()
        {   
            Common.SetSLPluginHeight(500);

            Chart chart = CreatAQuickLineChartWithSingleSeries();

            EnqueueDelay(_sleepTime);

            Common.AddMessageButton(this, "Updating YValue property of DataPoints at RealTime. Click Me to Stop and Continue.");

            Common.EnableAutoTimerCallBack(this, new TimeSpan(0, 0, 0, 0, 500), new TimeSpan(0, 0, 5),
               () =>
               {
                   try
                   {
                       chart.SamplingThreshold = 0;
                   }
                   catch
                   {
                        Assert.Fail("Setting SamplingThreshold = 0, fails");
                   }
               },
               () =>
               {
                   try
                   {
                       chart.SamplingThreshold = -1;
                       Assert.Fail("Setting SamplingThreshold = -1, failed");
                   }
                   catch
                   {
                   }
               }

            );

            TestPanel.Children.Add(chart);
        }
        
        public Chart CreatAQuickLineChartWithSingleSeries()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;
            chart.ScrollingEnabled = false;

            Common.CreateAndAddDefaultDataSeriesWithLargeNoOfDps(chart);

            chart.Series[0].RenderAs = RenderAs.QuickLine;

            return chart;
        }

        
        #endregion

        /// <summary>
        /// Event handler for loaded event of the chart
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chart_Loaded(object sender, RoutedEventArgs e)
        {
            _isLoaded = true;
        }
        
        #region Private Data
        /// <summary>
        /// Number of milliseconds to wait between actions in CreateAsyncTasks or Enqueue callbacks. 
        /// </summary>
        private const int _sleepTime = 1000;

        /// <summary>
        /// Whether the chart is loaded
        /// </summary>
        private bool _isLoaded = false;

        private System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();

        #endregion
    }
}
