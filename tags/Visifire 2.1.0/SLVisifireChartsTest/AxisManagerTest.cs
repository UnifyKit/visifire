//using System;
//using System.Windows;
//using System.Collections.Generic;
//using Microsoft.Silverlight.Testing;
//using System.Windows.Controls;
//using System.Windows.Media;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Visifire.Charts;
//using Visifire.Commons;

//namespace SLVisifireChartsTest
//{
//    //[TestClass]
//    public class AxisManagerTest : AxisTest
//    {

//        void chart_Loaded(object sender, System.Windows.RoutedEventArgs e)
//        {
//            isLoaded = true;
//        }

//        #region AxisManagerTest

//        /// <summary>
//        /// Test AxisManager Class
//        /// </summary>

//        [TestMethod]
//        [Description("Test Axis Manager")]
//        [Asynchronous]
//        public void TestAxisManager()
//        {
//            Chart chart = new Chart();
//            chart.Width = 400;
//            chart.Height = 300;

//            isLoaded = false;

//            chart.Loaded += new RoutedEventHandler(chart_Loaded);

//            chart.ChartArea = new ChartArea(chart);

//            Common.CreateAndAddDefaultDataSeries(chart);

//            chart.ChartArea.AxisX = new Axis();
//            chart.ChartArea.AxisX.AxisOrientation = Orientation.Horizontal;
//            chart.ChartArea.AxisX.AxisType = AxisTypes.Primary;
//            chart.ChartArea.AxisX.Maximum = 10;
//            chart.ChartArea.AxisX.Minimum = 1;
//            chart.ChartArea.AxisX.Interval = 1;

//            chart.ChartArea.AxisY = new Axis();
//            chart.ChartArea.AxisY.AxisOrientation = Orientation.Vertical;
//            chart.ChartArea.AxisY.AxisType = AxisTypes.Secondary;
//            chart.ChartArea.AxisY.Maximum = 100;
//            chart.ChartArea.AxisY.Minimum = 10;
//            chart.ChartArea.AxisX.Interval = 10;

//            chart.ChartArea.AxisX.AxisLabelsElement = new AxisLabels();
//            chart.ChartArea.AxisX.MajorTicksElement = new Ticks();

//            chart.ChartArea.AxisY.AxisLabelsElement = new AxisLabels();
//            chart.ChartArea.AxisY.MajorTicksElement = new Ticks();

//            chart.AxisX = new System.Collections.ObjectModel.ObservableCollection<Axis>();
//            chart.AxisX.Add(chart.ChartArea.AxisX);
//            chart.AxisY = new System.Collections.ObjectModel.ObservableCollection<Axis>();
//            chart.AxisY.Add(chart.ChartArea.AxisY);

//            Silverlight.TestSurface.Children.Add(chart);

//            Double axisXLabelMaximumValue = chart.ChartArea.AxisX.AxisLabelsElement.Maximum;
//            Double axisXLabelMinimumValue = chart.ChartArea.AxisX.AxisLabelsElement.Minimum;
//            Double axisXLabelIntervalValue = chart.ChartArea.AxisX.AxisLabelsElement.Interval;
//            Double axisXTickMaximumValue = chart.ChartArea.AxisX.MajorTicksElement.Maximum;
//            Double axisXTickMinimumValue = chart.ChartArea.AxisX.MajorTicksElement.Minimum;
//            Double axisXTickIntervalValue = chart.ChartArea.AxisX.MajorTicksElement.Interval;
//            Double axisYLabelMaximumValue = chart.ChartArea.AxisY.AxisLabelsElement.Maximum;
//            Double axisYLabelMinimumValue = chart.ChartArea.AxisY.AxisLabelsElement.Minimum;
//            Double axisYLabelIntervalValue = chart.ChartArea.AxisY.AxisLabelsElement.Interval;
//            Double axisYTickMaximumValue = chart.ChartArea.AxisY.MajorTicksElement.Maximum;
//            Double axisYTickMinimumValue = chart.ChartArea.AxisY.MajorTicksElement.Minimum;
//            Double axisYTickIntervalValue = chart.ChartArea.AxisY.MajorTicksElement.Interval;


//            EnqueueCallback(() =>
//            {
//                chart.ChartArea.AxisX.AxisManager = new AxisManager(chart.ChartArea.AxisX.Maximum, chart.ChartArea.AxisX.Minimum, true, false,false);
//                chart.ChartArea.AxisX.AxisManager.Calculate();

//                chart.ChartArea.AxisX.AxisLabelsElement.Maximum = chart.ChartArea.AxisX.AxisManager.AxisMaximumValue;
//                chart.ChartArea.AxisX.AxisLabelsElement.Minimum = chart.ChartArea.AxisX.AxisManager.AxisMinimumValue;
//                chart.ChartArea.AxisX.AxisLabelsElement.Interval = chart.ChartArea.AxisX.AxisManager.Interval;

//                chart.ChartArea.AxisX.MajorTicksElement.Maximum = chart.ChartArea.AxisX.AxisManager.AxisMaximumValue;
//                chart.ChartArea.AxisX.MajorTicksElement.Minimum = chart.ChartArea.AxisX.AxisManager.AxisMinimumValue;
//                chart.ChartArea.AxisX.MajorTicksElement.Interval = chart.ChartArea.AxisX.AxisManager.Interval;

//                chart.ChartArea.AxisY.AxisManager = new AxisManager(chart.ChartArea.AxisY.Maximum, chart.ChartArea.AxisY.Minimum, true, false,false);
//                chart.ChartArea.AxisY.AxisManager.Calculate();

//                chart.ChartArea.AxisY.AxisLabelsElement.Maximum = chart.ChartArea.AxisY.AxisManager.AxisMaximumValue;
//                chart.ChartArea.AxisY.AxisLabelsElement.Minimum = chart.ChartArea.AxisY.AxisManager.AxisMinimumValue;
//                chart.ChartArea.AxisY.AxisLabelsElement.Interval = chart.ChartArea.AxisY.AxisManager.Interval;

//                chart.ChartArea.AxisY.MajorTicksElement.Maximum = chart.ChartArea.AxisY.AxisManager.AxisMaximumValue;
//                chart.ChartArea.AxisY.MajorTicksElement.Minimum = chart.ChartArea.AxisY.AxisManager.AxisMinimumValue;
//                chart.ChartArea.AxisY.MajorTicksElement.Interval = chart.ChartArea.AxisY.AxisManager.Interval;
//            }
//            );

//            EnqueueConditional(() => { return isLoaded; });
//            EnqueueSleep(sleepTime);

//            EnqueueCallback(() =>
//            {
//                Assert.AreNotEqual(axisXLabelMaximumValue, chart.ChartArea.AxisX.AxisLabelsElement.Maximum);
//                Assert.AreNotEqual(axisXLabelMinimumValue, chart.ChartArea.AxisX.AxisLabelsElement.Minimum);
//                Assert.AreNotEqual(axisXLabelIntervalValue, chart.ChartArea.AxisX.AxisLabelsElement.Interval);
//                Assert.AreNotEqual(axisXTickMaximumValue, chart.ChartArea.AxisX.MajorTicksElement.Maximum);
//                Assert.AreNotEqual(axisXTickMinimumValue, chart.ChartArea.AxisX.MajorTicksElement.Minimum);
//                Assert.AreNotEqual(axisXTickIntervalValue, chart.ChartArea.AxisX.MajorTicksElement.Interval);

//                Assert.AreNotEqual(axisYLabelMaximumValue, chart.ChartArea.AxisY.AxisLabelsElement.Maximum);
//                Assert.AreNotEqual(axisYLabelMinimumValue, chart.ChartArea.AxisY.AxisLabelsElement.Minimum);
//                Assert.AreNotEqual(axisYLabelIntervalValue, chart.ChartArea.AxisY.AxisLabelsElement.Interval);
//                Assert.AreNotEqual(axisYTickMaximumValue, chart.ChartArea.AxisY.MajorTicksElement.Maximum);
//                Assert.AreNotEqual(axisYTickMinimumValue, chart.ChartArea.AxisY.MajorTicksElement.Minimum);
//                Assert.AreNotEqual(axisYTickIntervalValue, chart.ChartArea.AxisY.MajorTicksElement.Interval);
//            }
//            );

//            EnqueueTestComplete();
//        }

//        #endregion

//        #region Private Data

//        const int sleepTime = 2000;
//        bool isLoaded = false;

//        #endregion
//    }
//}
