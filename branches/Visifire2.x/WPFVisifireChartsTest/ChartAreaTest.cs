using System;
using System.Windows;
using System.Text;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Controls;
using System.Linq;
using System.ComponentModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
//using NUnit.Framework;
using System.Threading;
//using TestClass = NUnit.Framework.TestFixtureAttribute;
//using TestMethod = NUnit.Framework.TestAttribute;
using Visifire.Charts;
using Visifire.Commons;

namespace WPFVisifireChartsTest
{
    /// <summary>
    /// Summary description for ChartAreaTest
    /// </summary>
    [TestClass]
    public class ChartAreaTest
    {

        #region TestingChartAreaPropertyChanged Event
        /// <summary>
        /// Testing the ChartArea Property changed
        /// </summary>
        [TestMethod]
        //[Description("Testing the ChartArea property changed event by changing PlotArea Background")]
        //[Owner("[...]")]
        public void TestingChartAreaPropertyChanged()
        {
            //AvalonTestRunner.RunInSTA(
            //delegate
            //{
            //    //Thread.CurrentThread.SetApartmentState(ApartmentState.STA);

            //    chart = new Chart();
            //    chart.Width = 400;
            //    chart.Height = 300;
            //    chart.Loaded += new RoutedEventHandler(chart_Loaded);
            //    DataSeries dataSeries = new DataSeries();
            //    dataSeries.RenderAs = RenderAs.Column;

            //    Random rand = new Random();

            //    for (Int32 i = 0; i < 5; i++)
            //    {
            //        DataPoint datapoint = new DataPoint();
            //        datapoint.AxisXLabel = "a" + i;
            //        datapoint.YValue = rand.Next(0, 100);
            //        dataSeries.DataPoints.Add(datapoint);
            //    }
            //    chart.Series.Add(dataSeries);
            //    chart.PlotArea.Background = new SolidColorBrush(Colors.Gray);

            //});

            //Window window = new Window();
            //window.Content = chart;

            //AvalonTestRunner.RunDataBindingTests(window);

            //if (isLoaded)
            //    Common.AssertBrushesAreEqual(chart.BorderColor, new SolidColorBrush(Colors.Gray));
        }

        void chart_Loaded(object sender, RoutedEventArgs e)
        {
            isLoaded = true;
        }

        #endregion

        #region Private Data

        const int sleepTime = 1000;
        Chart chart;
        bool isLoaded = false;

        #endregion
    }
}
