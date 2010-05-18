using System;
using System.Windows;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Browser;
using Visifire.Charts;
using Visifire.Commons;

namespace SLVisifireChartsTest
{
    /// <summary>
    /// This class runs the unit tests for DateTime values
    /// </summary>
    [TestClass]
    public class DateTimeAxisTest : SilverlightControlTest
    {
        #region TestDateTimeWithSingleDataPoint
        /// <summary>
        /// Test Single DataPoint with DateTime value.
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void TestDateTimeWithSingleDataPoint()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Random rand = new Random();

            TestPanel.Children.Add(chart);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                DataSeries dataSeries = new DataSeries();
                DataPoint dataPoint = new DataPoint();
                dataPoint.XValue = new DateTime(2009, 1, 1);
                dataPoint.YValue = rand.Next(10, 100);
                dataSeries.DataPoints.Add(dataPoint);

                chart.Series.Add(dataSeries);
            });

            EnqueueCallback(() =>
            {
                Assert.AreEqual(1, chart.Series[0].DataPoints.Count);
            });

            EnqueueDelay(_sleepTime);
            EnqueueTestComplete();
        }
        #endregion

        #region TestDateTimeWithTwoDataPoints
        /// <summary>
        /// Test Two DataPoints with DateTime values.
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void TestDateTimeWithTwoDataPoints()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Random rand = new Random();

            TestPanel.Children.Add(chart);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                DataSeries dataSeries = new DataSeries();

                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2009, 1, 1), YValue = rand.Next(10, 100) });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2009, 1, 2), YValue = rand.Next(10, 100) });

                chart.Series.Add(dataSeries);
            });

            EnqueueCallback(() =>
            {
                Assert.AreEqual(2, chart.Series[0].DataPoints.Count);
            });

            EnqueueDelay(_sleepTime);
            EnqueueTestComplete();
        }
        #endregion

        #region TestDateTimeWithAxisMinimum
        /// <summary>
        /// Test AxisMinimum in DateTime values.
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void TestDateTimeWithAxisMinimum()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Random rand = new Random();

            TestPanel.Children.Add(chart);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueDelay(_sleepTime);

            Axis axis = new Axis();
            axis.AxisMinimum = new DateTime(2008, 10, 1);
            axis.Interval = 1;
            axis.IntervalType = IntervalTypes.Months;
            chart.AxesX.Add(axis);

            EnqueueCallback(() =>
            {
                DataSeries dataSeries = new DataSeries();

                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2009, 1, 1), YValue = rand.Next(10, 100) });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2009, 1, 5), YValue = rand.Next(10, 100) });

                chart.Series.Add(dataSeries);
            });

            EnqueueCallback(() =>
            {
                Assert.AreEqual(new DateTime(2008, 10, 1), chart.AxesX[0].AxisMinimum);
            });

            EnqueueDelay(_sleepTime);
            EnqueueTestComplete();
        }
        #endregion

        #region TestDateTimeWithAxisMaximum
        /// <summary>
        /// Test AxisMaximum in DateTime values.
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void TestDateTimeWithAxisMaximum()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Random rand = new Random();

            TestPanel.Children.Add(chart);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueDelay(_sleepTime);

            Axis axis = new Axis();
            axis.AxisMaximum = new DateTime(2009, 10, 15);
            axis.Interval = 1;
            axis.IntervalType = IntervalTypes.Months;
            chart.AxesX.Add(axis);

            EnqueueCallback(() =>
            {
                DataSeries dataSeries = new DataSeries();

                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2009, 1, 1), YValue = rand.Next(10, 100) });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2009, 1, 5), YValue = rand.Next(10, 100) });

                chart.Series.Add(dataSeries);
            });

            EnqueueCallback(() =>
            {
                Assert.AreEqual(new DateTime(2009, 10, 15), chart.AxesX[0].AxisMaximum);
            });

            EnqueueDelay(_sleepTime);
            EnqueueTestComplete();
        }
        #endregion

        #region TestDateTimeWithoutIntervalType
        /// <summary>
        /// Test DateTime without setting IntervalType and Interval
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void TestDateTimeWithoutIntervalType()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Random rand = new Random();

            TestPanel.Children.Add(chart);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                DataSeries dataSeries = new DataSeries();

                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2009, 1, 1), YValue = rand.Next(10, 100) });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2009, 2, 5), YValue = rand.Next(10, 100) });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2009, 3, 12), YValue = rand.Next(10, 100) });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2009, 4, 24), YValue = rand.Next(10, 100) });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2009, 5, 2), YValue = rand.Next(10, 100) });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2009, 6, 4), YValue = rand.Next(10, 100) });

                chart.Series.Add(dataSeries);
            });

            EnqueueCallback(() =>
            {
                Assert.AreEqual(IntervalTypes.Auto, chart.AxesX[0].IntervalType);
            });

            EnqueueDelay(_sleepTime);
            EnqueueTestComplete();
        }
        #endregion

        //#region TestDateTimeWithDifferentXValueType
        ///// <summary>
        ///// Test DateTime with different XValueType
        ///// </summary>
        //[TestMethod]
        //[Asynchronous]
        //public void TestDateTimeWithDifferentXValueType()
        //{
        //    Chart chart = new Chart();
        //    chart.Width = 500;
        //    chart.Height = 300;

        //    Random rand = new Random();

        //    TestPanel.Children.Add(chart);

        //    DataSeries dataSeries = new DataSeries();

        //    dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2009, 1, 1), YValue = rand.Next(10, 100) });
        //    dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2009, 2, 5), YValue = rand.Next(10, 100) });
        //    dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2009, 3, 12), YValue = rand.Next(10, 100) });

        //    chart.Series.Add(dataSeries);

        //    dataSeries = new DataSeries();
        //    dataSeries.XValueType = ChartValueTypes.DateTime;

        //    dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2009, 1, 1), YValue = rand.Next(10, 100) });
        //    dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2009, 2, 5), YValue = rand.Next(10, 100) });
        //    dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2009, 3, 12), YValue = rand.Next(10, 100) });

        //    chart.Series.Add(dataSeries);

        //    EnqueueTestComplete();
        //}
        //#endregion

        #region TestDateTimeWithSameXValues
        /// <summary>
        /// Test DateTime with same XValues
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void TestDateTimeWithSameXValues()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            Random rand = new Random();

            TestPanel.Children.Add(chart);

            DataSeries dataSeries = new DataSeries();

            dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2009, 1, 1), YValue = rand.Next(10, 100) });
            dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2009, 1, 1), YValue = rand.Next(10, 100) });
            dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2009, 1, 1), YValue = rand.Next(10, 100) });

            chart.Series.Add(dataSeries);

            EnqueueTestComplete();
        }
        #endregion

        #region TestTrendLineWithDateTime
        /// <summary>
        /// Test TrendLine with DateTime
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void TestTrendLineWithDateTime()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Random rand = new Random();

            TestPanel.Children.Add(chart);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueDelay(_sleepTime);

            Axis axis = new Axis();
            axis.Interval = 1;
            axis.IntervalType = IntervalTypes.Months;
            chart.AxesX.Add(axis);

            EnqueueCallback(() =>
            {
                TrendLine trendLine = new TrendLine();
                trendLine.Value = new DateTime(2001, 3, 3);
                trendLine.Orientation = Orientation.Vertical;
                chart.TrendLines.Add(trendLine);

                DataSeries dataSeries = new DataSeries();

                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2001, 1, 1), YValue = rand.Next(10, 100) });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2001, 2, 2), YValue = rand.Next(10, 100) });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2001, 3, 3), YValue = rand.Next(10, 100) });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2001, 4, 4), YValue = rand.Next(10, 100) });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2001, 5, 5), YValue = rand.Next(10, 100) });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2001, 6, 6), YValue = rand.Next(10, 100) });

                chart.Series.Add(dataSeries);
            });

            EnqueueCallback(() =>
            {
                Assert.AreEqual(new DateTime(2001, 3, 3), chart.TrendLines[0].Value);
            });

            EnqueueDelay(_sleepTime);
            EnqueueTestComplete();
        }
        #endregion

        #region TestSingleDataPointWithAxisMinimum
        /// <summary>
        /// Test Single DataPoint with AxisMinimum
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void TestSingleDataPointWithAxisMinimum()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Random rand = new Random();

            TestPanel.Children.Add(chart);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueDelay(_sleepTime);

            Axis axis = new Axis();
            axis.AxisMinimum = new DateTime(2001, 1, 1);
            chart.AxesX.Add(axis);

            EnqueueCallback(() =>
            {
                DataSeries dataSeries = new DataSeries();

                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2001, 2, 1), YValue = rand.Next(10, 100) });

                chart.Series.Add(dataSeries);
            });

            EnqueueCallback(() =>
            {
                Assert.AreEqual(new DateTime(2001, 1, 1), chart.AxesX[0].AxisMinimum);
            });

            EnqueueDelay(_sleepTime);
            EnqueueTestComplete();
        }
        #endregion

        #region TestIntervalWithDecimalValue
        /// <summary>
        /// Test Interval with Decimal value if IntervalType is Months
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void TestIntervalWithDecimalValue()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Random rand = new Random();

            TestPanel.Children.Add(chart);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueDelay(_sleepTime);

            Axis axis = new Axis();
            axis.IntervalType = IntervalTypes.Months;
            axis.Interval = 0.1;
            chart.AxesX.Add(axis);

            EnqueueCallback(() =>
            {
                DataSeries dataSeries = new DataSeries();

                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2009, 3, 1), YValue = rand.Next(10, 100) });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2009, 4, 1), YValue = rand.Next(10, 100) });
                chart.Series.Add(dataSeries);
            });

            EnqueueCallback(() =>
            {
                Assert.AreEqual(chart.AxesX[0].Interval, chart.AxesX[0].InternalInterval);
            });

            EnqueueDelay(_sleepTime);
            EnqueueTestComplete();
        }
        #endregion

        #region TestDateTimeWithStartFromZero
        /// <summary>
        /// Test DateTime with StartFromZero
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void TestDateTimeWithStartFromZero()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Random rand = new Random();

            TestPanel.Children.Add(chart);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueDelay(_sleepTime);

            Axis axis = new Axis();
            axis.StartFromZero = true;
            axis.Interval = 1;
            axis.IntervalType = IntervalTypes.Weeks;
            chart.AxesX.Add(axis);

            EnqueueCallback(() =>
            {
                DataSeries dataSeries = new DataSeries();

                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2009, 1, 1), YValue = rand.Next(10, 100) });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2009, 2, 1), YValue = rand.Next(10, 100) });

                chart.Series.Add(dataSeries);
            });

            EnqueueCallback(() =>
            {
                Assert.IsTrue((Boolean)chart.AxesX[0].StartFromZero);
            });

            EnqueueDelay(_sleepTime);
            EnqueueTestComplete();
        }
        #endregion

        #region TestScrolling
        /// <summary>
        /// Test DateTime with Scrolling
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void TestScrolling()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Random rand = new Random();

            TestPanel.Children.Add(chart);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueDelay(_sleepTime);

            Axis axis = new Axis();
            axis.Interval = 1;
            axis.IntervalType = IntervalTypes.Weeks;
            chart.AxesX.Add(axis);

            EnqueueCallback(() =>
            {
                DataSeries dataSeries = new DataSeries();

                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2009, 2, 1), YValue = rand.Next(10, 100) });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2009, 3, 8), YValue = rand.Next(10, 100) });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2009, 3, 7), YValue = rand.Next(10, 100) });

                chart.Series.Add(dataSeries);
            });

            EnqueueDelay(_sleepTime);
            EnqueueTestComplete();
        }
        #endregion

        #region TestGridAlignment
        /// <summary>
        /// Test Grid alignment
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void TestGridAlignment()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;
            chart.Padding = new Thickness(6);

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Random rand = new Random();

            TestPanel.Children.Add(chart);

            Axis axis = new Axis();
            axis.ValueFormatString = "dd MMM yyyy";
            ChartGrid grid = new ChartGrid();
            grid.Enabled = true;
            axis.Grids.Add(grid);
            chart.AxesX.Add(axis);

            DataSeries dataSeries = new DataSeries();

            dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2009, 1, 1), YValue = rand.Next(10, 100) });
            dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2009, 2, 1), YValue = rand.Next(10, 100) });
            dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2009, 3, 1), YValue = rand.Next(10, 100) });
            dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2009, 11, 1), YValue = rand.Next(10, 100) });
            dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2009, 12, 1), YValue = rand.Next(10, 100) });
            dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2010, 10, 1), YValue = rand.Next(10, 100) });

            chart.Series.Add(dataSeries);

            EnqueueDelay(2000);
            EnqueueTestComplete();
        }
        #endregion

        #region TestIntervalType
        /// <summary>
        /// Test IntervalType as Minutes if XValueType is Date
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void TestIntervalTypeAsMinutes4XValueTypeAsDate()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Random rand = new Random();

            TestPanel.Children.Add(chart);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueDelay(_sleepTime);

            Axis axis = new Axis();
            axis.IntervalType = IntervalTypes.Minutes;
            axis.Interval = 10;
            chart.AxesX.Add(axis);

            EnqueueCallback(() =>
            {
                DataSeries dataSeries = new DataSeries();

                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2009, 2, 1), YValue = rand.Next(10, 100) });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2009, 3, 8), YValue = rand.Next(10, 100) });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2009, 8, 6), YValue = rand.Next(10, 100) });

                chart.Series.Add(dataSeries);
            });

            EnqueueDelay(_sleepTime);
            EnqueueTestComplete();
        }
        #endregion

        #region TestDateTimeAsXValueType
        /// <summary>
        /// Test DateTime as XValueType
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void TestDateTimeAsXValueType()
        {
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "100%");

            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Random rand = new Random();

            TestPanel.Children.Add(chart);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueDelay(_sleepTime);

            Axis axis = new Axis();
            axis.IntervalType = IntervalTypes.Hours;
            axis.Interval = 200;
            axis.AxisLabels = new AxisLabels();
            axis.AxisLabels.Angle = -45;
            chart.AxesX.Add(axis);

            EnqueueCallback(() =>
            {
                DataSeries dataSeries = new DataSeries();
                dataSeries.XValueType = ChartValueTypes.DateTime;

                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2009, 2, 1, 1, 3, 4), YValue = rand.Next(10, 100) });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2009, 3, 8, 10, 12, 28), YValue = rand.Next(10, 100) });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2009, 8, 6, 6, 2, 1), YValue = rand.Next(10, 100) });

                chart.Series.Add(dataSeries);
            });

            EnqueueDelay(_sleepTime);
            EnqueueTestComplete();
        }
        #endregion

        #region TestTimeAsXValueType
        /// <summary>
        /// Test Time as XValueType
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void TestTimeAsXValueType()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Random rand = new Random();

            TestPanel.Children.Add(chart);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueDelay(_sleepTime);

            Axis axis = new Axis();
            axis.IntervalType = IntervalTypes.Minutes;
            axis.Interval = 20;
            axis.AxisLabels = new AxisLabels();
            axis.AxisLabels.Angle = -45;
            ChartGrid grid = new ChartGrid();
            grid.Enabled = true;
            axis.Grids.Add(grid);
            chart.AxesX.Add(axis);

            EnqueueCallback(() =>
            {
                DataSeries dataSeries = new DataSeries();
                dataSeries.XValueType = ChartValueTypes.Time;

                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2009, 2, 1, 1, 3, 4), YValue = rand.Next(10, 100) });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2009, 3, 8, 10, 12, 28), YValue = rand.Next(10, 100) });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2009, 8, 6, 6, 2, 1), YValue = rand.Next(10, 100) });

                chart.Series.Add(dataSeries);
            });

            EnqueueDelay(_sleepTime);
            EnqueueTestComplete();
        }
        #endregion

        #region TestTimeWithDefaultDate
        /// <summary>
        /// Test Time with default Date in Axis
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void TestTimeWithDefaultDate()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Random rand = new Random();

            TestPanel.Children.Add(chart);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueDelay(_sleepTime);

            Axis axis = new Axis();
            axis.ValueFormatString = "dd/MM/yyyy hh:mm:ss tt";
            axis.AxisLabels = new AxisLabels();
            axis.AxisLabels.Angle = -45;
            ChartGrid grid = new ChartGrid();
            grid.Enabled = true;
            axis.Grids.Add(grid);
            chart.AxesX.Add(axis);

            EnqueueCallback(() =>
            {
                DataSeries dataSeries = new DataSeries();
                dataSeries.XValueType = ChartValueTypes.Time;

                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2009, 2, 1, 1, 3, 4), YValue = rand.Next(10, 100) });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2009, 3, 8, 10, 12, 28), YValue = rand.Next(10, 100) });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2009, 8, 6, 6, 2, 1), YValue = rand.Next(10, 100) });

                chart.Series.Add(dataSeries);
            });

            EnqueueDelay(_sleepTime);
            EnqueueTestComplete();
        }
        #endregion

        #region TestDateTimeWithAxisMinimumAndMaximum
        /// <summary>
        /// Test DateTime with AxisMinimum and AxisMaximum
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void TestDateTimeWithAxisMinimumAndMaximum()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Random rand = new Random();

            TestPanel.Children.Add(chart);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueDelay(_sleepTime);

            Axis axis = new Axis();
            axis.AxisMinimum = new DateTime(2008, 12, 1, 1, 2, 4);
            axis.AxisMaximum = new DateTime(2009, 6, 1, 12, 4, 5);
            axis.AxisLabels = new AxisLabels();
            axis.AxisLabels.Angle = -45;
            ChartGrid grid = new ChartGrid();
            grid.Enabled = true;
            axis.Grids.Add(grid);
            chart.AxesX.Add(axis);

            EnqueueCallback(() =>
            {
                DataSeries dataSeries = new DataSeries();
                dataSeries.XValueType = ChartValueTypes.DateTime;

                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2009, 1, 1, 1, 3, 4), YValue = rand.Next(10, 100) });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2009, 2, 1, 10, 12, 28), YValue = rand.Next(10, 100) });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2009, 5, 1, 6, 2, 1), YValue = rand.Next(10, 100) });

                chart.Series.Add(dataSeries);
            });

            EnqueueDelay(_sleepTime);
            EnqueueTestComplete();
        }
        #endregion

        #region TestDateTimeWithCombinationCharts
        /// <summary>
        /// Test DateTime with different charttypes
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void TestDateTimeWithCombinationCharts()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Random rand = new Random();

            TestPanel.Children.Add(chart);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueDelay(_sleepTime);

            Axis axis = new Axis();
            axis.AxisLabels = new AxisLabels();
            axis.AxisLabels.Angle = -45;
            ChartGrid grid = new ChartGrid();
            grid.Enabled = true;
            axis.Grids.Add(grid);
            chart.AxesX.Add(axis);

            Int32 ds = 0;
            EnqueueCallback(() =>
            {
                DataSeries dataSeries;

                for (ds = 0; ds < 5; ds++)
                {
                    dataSeries = new DataSeries();
                    if (ds == 1 || ds == 3)
                        dataSeries.RenderAs = RenderAs.Line;
                    else if (ds == 4)
                        dataSeries.RenderAs = RenderAs.Area;

                    dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2009, 1, 1), YValue = rand.Next(10, 100) });
                    dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2009, 2, 1), YValue = rand.Next(10, 100) });
                    dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2009, 3, 1), YValue = rand.Next(10, 100) });
                    dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2009, 4, 1), YValue = rand.Next(10, 100) });
                    dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2009, 5, 1), YValue = rand.Next(10, 100) });
                    dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2009, 8, 1), YValue = rand.Next(10, 100) });

                    chart.Series.Add(dataSeries);
                }
            });

            EnqueueCallback(() =>
                {
                    Assert.AreEqual(ds, chart.Series.Count);
                });

            EnqueueDelay(_sleepTime);
            EnqueueTestComplete();
        }
        #endregion

        #region TestDateTimeValueInPie
        /// <summary>
        /// Test DateTime values in Pie
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void TestDateTimeValueInPie()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Random rand = new Random();

            TestPanel.Children.Add(chart);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                DataSeries dataSeries = new DataSeries();
                dataSeries.RenderAs = RenderAs.Pie;

                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2009, 1, 1), YValue = rand.Next(10, 100) });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2009, 2, 1), YValue = rand.Next(10, 100) });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2009, 3, 1), YValue = rand.Next(10, 100) });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2009, 4, 1), YValue = rand.Next(10, 100) });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2009, 5, 1), YValue = rand.Next(10, 100) });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2009, 8, 1), YValue = rand.Next(10, 100) });

                chart.Series.Add(dataSeries);
            });

            EnqueueCallback(() =>
            {
                Assert.AreEqual(RenderAs.Pie, chart.Series[0].RenderAs);
            });

            EnqueueDelay(_sleepTime);
            EnqueueTestComplete();
        }
        #endregion

        #region TestDateTimeValueInDoughnut
        /// <summary>
        /// Test DateTime values in Doughnut
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void TestDateTimeValueInDoughnut()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Random rand = new Random();

            TestPanel.Children.Add(chart);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                DataSeries dataSeries = new DataSeries();
                dataSeries.RenderAs = RenderAs.Doughnut;

                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2009, 1, 1), YValue = rand.Next(10, 100) });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2009, 2, 1), YValue = rand.Next(10, 100) });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2009, 3, 1), YValue = rand.Next(10, 100) });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2009, 4, 1), YValue = rand.Next(10, 100) });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2009, 5, 1), YValue = rand.Next(10, 100) });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2009, 8, 1), YValue = rand.Next(10, 100) });

                chart.Series.Add(dataSeries);
            });

            EnqueueCallback(() =>
            {
                Assert.AreEqual(RenderAs.Doughnut, chart.Series[0].RenderAs);
            });

            EnqueueDelay(_sleepTime);
            EnqueueTestComplete();
        }
        #endregion

        #region TestDateTimeValueInBar
        /// <summary>
        /// Test DateTime values in Bar
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void TestDateTimeValueInBar()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Random rand = new Random();

            TestPanel.Children.Add(chart);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                DataSeries dataSeries = new DataSeries();
                dataSeries.RenderAs = RenderAs.Bar;

                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2009, 1, 1), YValue = rand.Next(10, 100) });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2009, 2, 1), YValue = rand.Next(10, 100) });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2009, 3, 1), YValue = rand.Next(10, 100) });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2009, 4, 1), YValue = rand.Next(10, 100) });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2009, 5, 1), YValue = rand.Next(10, 100) });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2009, 8, 1), YValue = rand.Next(10, 100) });

                chart.Series.Add(dataSeries);
            });

            EnqueueCallback(() =>
            {
                Assert.AreEqual(RenderAs.Bar, chart.Series[0].RenderAs);
            });

            EnqueueDelay(_sleepTime);
            EnqueueTestComplete();
        }
        #endregion

        #region TestValueFormatString
        /// <summary>
        /// Test ValueFormatString
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void TestValueFormatString()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Random rand = new Random();

            TestPanel.Children.Add(chart);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueDelay(_sleepTime);

            Axis axis = new Axis();
            axis.ValueFormatString = "dd MMM yyyy";
            axis.AxisLabels = new AxisLabels();
            axis.AxisLabels.Angle = -45;
            chart.AxesX.Add(axis);

            EnqueueCallback(() =>
            {
                DataSeries dataSeries = new DataSeries();

                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2009, 1, 1, 1, 2, 3), YValue = rand.Next(10, 100) });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2009, 2, 1, 4, 5, 6), YValue = rand.Next(10, 100) });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2009, 3, 1, 12, 4, 8), YValue = rand.Next(10, 100) });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2009, 4, 1, 22, 2, 4), YValue = rand.Next(10, 100) });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2009, 5, 1, 20, 5, 4), YValue = rand.Next(10, 100) });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2009, 12, 1, 2, 1, 8), YValue = rand.Next(10, 100) });

                chart.Series.Add(dataSeries);
            });

            EnqueueDelay(_sleepTime);
            EnqueueTestComplete();
        }
        #endregion

        #region TestIntervalTypeWithoutInterval
        /// <summary>
        /// Test IntervalType without Interval
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void TestIntervalTypeWithoutInterval()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Random rand = new Random();

            TestPanel.Children.Add(chart);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueDelay(_sleepTime);

            Axis axis = new Axis();
            axis.AxisLabels = new AxisLabels();
            axis.AxisLabels.Angle = -45;
            axis.IntervalType = IntervalTypes.Months;
            chart.AxesX.Add(axis);

            EnqueueCallback(() =>
            {
                DataSeries dataSeries = new DataSeries();

                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2009, 1, 1, 1, 2, 3), YValue = rand.Next(10, 100) });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2009, 2, 1, 4, 5, 6), YValue = rand.Next(10, 100) });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2009, 3, 1, 12, 4, 8), YValue = rand.Next(10, 100) });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2009, 4, 1, 22, 2, 4), YValue = rand.Next(10, 100) });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2009, 5, 1, 20, 5, 4), YValue = rand.Next(10, 100) });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2009, 8, 1, 2, 1, 8), YValue = rand.Next(10, 100) });

                chart.Series.Add(dataSeries);
            });

            EnqueueDelay(_sleepTime);
            EnqueueTestComplete();
        }
        #endregion

        #region TestDateTimeViaXaml
        /// <summary>
        /// Test DateTime via XAML
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void TestDateTimeViaXaml()
        {
            Object result = System.Windows.Markup.XamlReader.Load(Resource.Chart_Xaml);
            Assert.IsInstanceOfType(result, typeof(Chart));

            Chart chart = result as Chart;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            TestPanel.Children.Add(chart);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                chart.AxesX[0].AxisLabels.Angle = -45;
                chart.Series.Add((DataSeries)System.Windows.Markup.XamlReader.Load(Resource.DateTimeXaml));
            });

            EnqueueCallback(() =>
            {
                Assert.AreEqual(ChartValueTypes.DateTime, chart.Series[0].XValueType);
            });

            EnqueueDelay(_sleepTime);
            EnqueueTestComplete();
        }
        #endregion

        #region TestTrendLineInBar
        /// <summary>
        /// Test TrendLine in Bar chart
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void TestTrendLineInBar()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Random rand = new Random();

            TestPanel.Children.Add(chart);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                TrendLine trendLine = new TrendLine();
                trendLine.Value = new DateTime(2009, 2, 1, 1, 2, 3);
                chart.TrendLines.Add(trendLine);

                DataSeries dataSeries = new DataSeries();
                dataSeries.RenderAs = RenderAs.Bar;
                dataSeries.XValueType = ChartValueTypes.DateTime;

                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2009, 1, 1, 1, 2, 3), YValue = rand.Next(10, 100) });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2009, 2, 1, 4, 5, 6), YValue = rand.Next(10, 100) });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2009, 3, 1, 12, 4, 8), YValue = rand.Next(10, 100) });

                chart.Series.Add(dataSeries);
            });

            EnqueueDelay(_sleepTime);
            EnqueueTestComplete();
        }
        #endregion

        #region TestDefaultTime
        /// <summary>
        /// Test default time if XValueType is Date
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void TestDefaultTime4XValueTypeAsDate()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Random rand = new Random();

            TestPanel.Children.Add(chart);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                chart.AxesX[0].ValueFormatString = "MM/dd/yyyy hh:mm:ss tt";
                chart.AxesX[0].AxisLabels.Angle = -45;

                DataSeries dataSeries = new DataSeries();

                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2009, 1, 1), YValue = rand.Next(10, 100) });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2009, 2, 1), YValue = rand.Next(10, 100) });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2009, 3, 1), YValue = rand.Next(10, 100) });

                chart.Series.Add(dataSeries);
            });

            EnqueueCallback(() =>
                {
                    Assert.AreEqual("MM/dd/yyyy hh:mm:ss tt", chart.AxesX[0].ValueFormatString);
                });

            EnqueueDelay(_sleepTime);
            EnqueueTestComplete();
        }
        #endregion

        #region TestSingleDataPointWithBar
        /// <summary>
        /// Test Single DataPoint with Bar chart
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void TestSingleDataPointWithBar()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Random rand = new Random();

            TestPanel.Children.Add(chart);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                DataSeries dataSeries = new DataSeries();
                dataSeries.RenderAs = RenderAs.Bar;
                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2009, 1, 1), YValue = rand.Next(10, 100) });

                chart.Series.Add(dataSeries);
            });

            EnqueueCallback(() =>
            {
                Assert.AreEqual(1, chart.Series[0].DataPoints.Count);
            });

            EnqueueDelay(_sleepTime);
            EnqueueTestComplete();
        }
        #endregion

        #region TestIntervalInDecimalAndIntervalTypeAsYears
        /// <summary>
        /// Test Interval in decimals and IntervalType as Years
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void TestIntervalInDecimalAndIntervalTypeAsYears()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Random rand = new Random();

            TestPanel.Children.Add(chart);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                chart.AxesX[0].AxisLabels.Angle = -45;
                chart.AxesX[0].IntervalType = IntervalTypes.Years;
                chart.AxesX[0].Interval = 0.005;

                DataSeries dataSeries = new DataSeries();
                dataSeries.RenderAs = RenderAs.Column;
                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2001, 1, 1), YValue = rand.Next(10, 100) });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2001, 1, 20), YValue = rand.Next(10, 100) });

                chart.Series.Add(dataSeries);
            });

            EnqueueCallback(() =>
            {
                Assert.AreEqual(0.005, chart.AxesX[0].Interval);
            });

            EnqueueDelay(_sleepTime);
            EnqueueTestComplete();
        }
        #endregion

        #region TestValueFormatStringDefaultValue4Date
        /// <summary>
        /// Test default value for ValueFormatString for XValueType as Date
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void TestValueFormatStringNewValue4Date()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;
                
            DataSeries dataSeries = CreateDataSeries();
            dataSeries.XValueType = ChartValueTypes.Date;

            chart.Series.Add(dataSeries);

            EnqueueDelay(_sleepTime);

            CreateAsyncTest(chart,
                () => chart.AxesX[0].ValueFormatString = "MMM dd yyyy",
                () => Assert.AreEqual("MMM dd yyyy", chart.AxesX[0].ValueFormatString));
        }
        #endregion

        #region TestValueFormatStringDefaultValue4Time
        /// <summary>
        /// Test default value for ValueFormatString for XValueType as Time
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void TestValueFormatStringNewValue4Time()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            DataSeries dataSeries = CreateDataSeries();
            dataSeries.XValueType = ChartValueTypes.Time;

            chart.Series.Add(dataSeries);

            EnqueueDelay(_sleepTime);

            CreateAsyncTest(chart,
                () => chart.AxesX[0].ValueFormatString = "hh:mm:ss",
                () => Assert.AreEqual("hh:mm:ss", chart.AxesX[0].ValueFormatString));
        }
        #endregion

        #region TestValueFormatStringDefaultValue4DateTime
        /// <summary>
        /// Test default value for ValueFormatString for XValueType as DateTime
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void TestValueFormatStringNewValue4DateTime()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            DataSeries dataSeries = CreateDataSeries();
            dataSeries.XValueType = ChartValueTypes.DateTime;

            chart.Series.Add(dataSeries);

            EnqueueDelay(_sleepTime);

            CreateAsyncTest(chart,
                () => chart.AxesX[0].ValueFormatString = "MMM/dd/yyyy hh:mm:ss tt",
                () => Assert.AreEqual("MMM/dd/yyyy hh:mm:ss tt", chart.AxesX[0].ValueFormatString));
        }
        #endregion

        #region TestXValueTypeDefaultValue
        /// <summary>
        /// Test default value for XValueType
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void TestXValueTypeDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            DataSeries dataSeries = CreateDataSeries();
            chart.Series.Add(dataSeries);

            EnqueueDelay(_sleepTime);

            CreateAsyncTest(chart,
                () => Assert.AreEqual(ChartValueTypes.Auto, chart.Series[0].XValueType));
        }
        #endregion

        #region TestIntervalTypeDefaultValue
        /// <summary>
        /// Test default value for IntervalType
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void TestIntervalTypeDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            DataSeries dataSeries = CreateDataSeries();
            chart.Series.Add(dataSeries);

            EnqueueDelay(_sleepTime);

            CreateAsyncTest(chart,
                () => Assert.AreEqual(IntervalTypes.Auto, chart.AxesX[0].IntervalType));
        }
        #endregion

        /// <summary>
        /// Create DataSeries
        /// </summary>
        /// <returns>DataSeries</returns>
        private DataSeries CreateDataSeries()
        {
            Random rand = new Random();

            DataSeries dataSeries = new DataSeries();
            dataSeries.RenderAs = RenderAs.Column;

            for (Int32 i = 0; i < 5; i++)
            {
                DataPoint dataPoint = new DataPoint();
                dataPoint.XValue = new DateTime(2001, rand.Next(1,5), rand.Next(1,30), rand.Next(1,5), rand.Next(1,20), rand.Next(1,30));
                dataPoint.YValue = rand.Next(0, 100);
                dataSeries.DataPoints.Add(dataPoint);
            }
            return dataSeries;
        }

        /// <summary>
        /// Event handler for loaded event of the chart
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chart_Loaded(object sender, System.Windows.RoutedEventArgs e)
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

        #endregion
    }
}