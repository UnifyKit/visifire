using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Collections.Generic;
using System.Collections;
using System.Windows.Browser;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Visifire.Charts;
using Visifire.Commons;

namespace SLVisifireChartsTest
{
    /// <summary>
    /// This class runs the unit tests Visifire.Charts.DataPoint class 
    /// </summary>
    [TestClass]
    public class DataPointTest : SilverlightControlTest
    {

        #region TestDataPointPropertyChange
        /// <summary>
        /// Test DataPoint property changed event.
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void TestDataPointPropertyChanged()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;


            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Random rand = new Random();

            TestPanel.Children.Add(chart);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueSleep(_sleepTime);

            DataSeries dataSeries = new DataSeries();
            DataPoint dataPoint = null;

            for (Int32 i = 0; i < 10; i++)
            {
                dataPoint = new DataPoint();
                dataPoint.XValue = i + 1;
                dataPoint.YValue = rand.Next(10, 100);

                dataPoint.PropertyChanged += delegate(Object sender, System.ComponentModel.PropertyChangedEventArgs e)
                {
                    Assert.IsNotNull(e.PropertyName);
                    if (e.PropertyName == "XValue")
                        Assert.AreEqual("XValue", e.PropertyName);
                    else if (e.PropertyName == "YValue")
                        Assert.AreEqual("YValue", e.PropertyName);
                    else
                        Assert.IsNotNull(e.PropertyName);
                };

                dataSeries.DataPoints.Add(dataPoint);
            }

            chart.Series.Add(dataSeries);

            EnqueueCallback(() =>
            {
                dataPoint.XValue = 10;
                dataPoint.YValue = rand.Next(-100, 100);
            });

            EnqueueSleep(_sleepTime);
            EnqueueTestComplete();

        }
        #endregion

        #region DataPointDecimalXValueChecking
        /// <summary>
        /// Test DataPoints with Decimal values in the second series
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void DataPointDecimalXValueChecking()
        {
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "400px");

            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;


            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Random rand = new Random();

            List<Double> xList = new List<Double>();
            List<Double> yList = new List<Double>();
            Double y = 0;

            TestPanel.Children.Add(chart);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueSleep(_sleepTime);

            EnqueueCallback(() =>
            {
                DataSeries dataSeries1 = new DataSeries();
                dataSeries1.RenderAs = RenderAs.Column;

                for (Double i = 0; i < 10; i++)
                {
                    DataPoint dataPoint = new DataPoint();
                    dataPoint.XValue = i + 1;
                    xList.Add(i);
                    dataPoint.YValue = (y = rand.Next(-100, 100));
                    yList.Add(y);
                    dataSeries1.DataPoints.Add(dataPoint);
                }
                chart.Series.Add(dataSeries1);
            });

            EnqueueSleep(_sleepTime);

            EnqueueCallback(() =>
            {

                DataSeries dataSeries2 = new DataSeries();
                dataSeries2.RenderAs = RenderAs.Column;

                Double j = 0.5;
                for (Int32 i = 0; i < 10; i++)
                {
                    DataPoint dataPoint = new DataPoint();
                    dataPoint.XValue = j;
                    dataPoint.YValue = yList[i];
                    dataSeries2.DataPoints.Add(dataPoint);
                    j++;
                }
                chart.Series.Add(dataSeries2);
                _htmlElement1.AttachEvent("onclick", new EventHandler<HtmlEventArgs>(this.HtmlElement_OnClick));
            });

            _htmlElement1 = Common.GetDisplayMessageButton(_htmlElement1);
            _htmlElement1.SetProperty("value", "Testing DataSeries Behaviour :- First DataSeries with all positive XValues and Second DataSeries with all Decimal XValues. Click here to exit.");
            System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement1);
        }
        #endregion

        #region DataPointRepeatedXValueChecking
        /// <summary>
        /// Test DataPoints with repeated XValues in the series
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void DataPointRepeatedXValueChecking()
        {

            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "400px");

            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;


            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Axis axisX = new Axis();
            chart.AxesX.Add(axisX);

            Random rand = new Random();

            DataSeries dataSeries1 = new DataSeries();

            for (Double i = 0; i < 10; i++)
            {
                DataPoint dataPoint = new DataPoint();
                if (i >= 5)
                    dataPoint.XValue = i - 4;
                else
                    dataPoint.XValue = i + 1;
                dataPoint.YValue = rand.Next(-100, 100);
                dataSeries1.DataPoints.Add(dataPoint);
            }

            chart.Series.Add(dataSeries1);

            TestPanel.Children.Add(chart);

            EnqueueConditional(() => { return _isLoaded; });

            EnqueueCallback(() =>
            {
                _htmlElement1.AttachEvent("onclick", new EventHandler<HtmlEventArgs>(this.HtmlElement_OnClick));
            });

            _htmlElement1 = Common.GetDisplayMessageButton(_htmlElement1);
            _htmlElement1.SetProperty("value", "Testing DataSeries Behaviour with repeated XValues. Click here to exit.");
            System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement1);
        }
        #endregion

        #region DataPointStressXValueChecking
        /// <summary>
        /// Stress testing DataPoints with hundreds of XValues
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void DataPointStressXValueChecking()
        {

            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "400px");

            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;
            chart.ScrollingEnabled = false;


            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Axis axisX = new Axis();
            chart.AxesX.Add(axisX);

            Random rand = new Random();

            DataSeries dataSeries1 = new DataSeries();

            Int32 numberOfDataPoints = 0;

            TestPanel.Children.Add(chart);

            EnqueueConditional(() => { return _isLoaded; });

            String msg = Common.AssertAverageDuration(100, 2, delegate
            {
                for (Int32 i = 0; i < 500; i++)
                {
                    DataPoint dataPoint = new DataPoint();
                    dataPoint.XValue = i + 1;
                    dataPoint.YValue = rand.Next(-500, 500);
                    dataSeries1.DataPoints.Add(dataPoint);
                    numberOfDataPoints++;
                }
            });

            chart.Series.Add(dataSeries1);

            EnqueueSleep(_sleepTime);

            EnqueueCallback(() =>
            {
                _htmlElement1 = Common.GetDisplayMessageButton(_htmlElement1);
                _htmlElement1.SetStyleAttribute("width", "900px");
                _htmlElement1.SetProperty("value", numberOfDataPoints + " XValues added. " + msg + " Click here to exit.");
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement1);
            });

            EnqueueCallback(() =>
            {
                _htmlElement1.AttachEvent("onclick", new EventHandler<HtmlEventArgs>(this.HtmlElement_OnClick));
            });
        }
        #endregion

        #region DataPointDecimalXValueChecking2
        /// <summary>
        /// Performance testing DataPoints with decimal XValues
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void DataPointDecimalXValueChecking2()
        {

            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "400px");

            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;


            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Axis axisX = new Axis();
            chart.AxesX.Add(axisX);

            Random rand = new Random();

            DataSeries dataSeries1 = new DataSeries();

            DataPoint dataPoint = new DataPoint();
            dataPoint.XValue = 0.001;
            dataPoint.YValue = rand.Next(0, 100);
            dataSeries1.DataPoints.Add(dataPoint);
            dataPoint = new DataPoint();
            dataPoint.XValue = 1.265;
            dataPoint.YValue = rand.Next(0, 100);
            dataSeries1.DataPoints.Add(dataPoint);
            dataPoint = new DataPoint();
            dataPoint.XValue = 2;
            dataPoint.YValue = rand.Next(0, 100);
            dataSeries1.DataPoints.Add(dataPoint);
            dataPoint = new DataPoint();
            dataPoint.XValue = 4.454;
            dataPoint.YValue = rand.Next(0, 100);
            dataSeries1.DataPoints.Add(dataPoint);
            dataPoint = new DataPoint();
            dataPoint.XValue = 5.998;
            dataPoint.YValue = rand.Next(0, 100);
            dataSeries1.DataPoints.Add(dataPoint);
            dataPoint = new DataPoint();
            dataPoint.XValue = 3.2;
            dataPoint.YValue = rand.Next(0, 100);
            dataSeries1.DataPoints.Add(dataPoint);

            chart.Series.Add(dataSeries1);

            TestPanel.Children.Add(chart);

            EnqueueConditional(() => { return _isLoaded; });

            EnqueueCallback(() =>
            {
                _htmlElement1.AttachEvent("onclick", new EventHandler<HtmlEventArgs>(this.HtmlElement_OnClick));
            });

            _htmlElement1 = Common.GetDisplayMessageButton(_htmlElement1);
            _htmlElement1.SetStyleAttribute("width", "900px");
            _htmlElement1.SetProperty("value", " DataSeries with decimal XValues. Click here to exit.");
            System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement1);
        }
        #endregion

        #region DataPointAxisXLabelChecking
        /// <summary>
        /// Performance testing DataPoints with AxisXLabels
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void DataPointAxisXLabelChecking()
        {

            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "400px");

            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;


            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Axis axisX = new Axis();
            chart.AxesX.Add(axisX);

            Random rand = new Random();

            DataSeries dataSeries1 = new DataSeries();

            DataPoint dataPoint = new DataPoint();
            dataPoint.AxisXLabel = "a1";
            dataPoint.XValue = 0.001;
            dataPoint.YValue = rand.Next(0, 100);
            dataSeries1.DataPoints.Add(dataPoint);
            dataPoint = new DataPoint();
            dataPoint.AxisXLabel = "a1";
            dataPoint.XValue = 1.265;
            dataPoint.YValue = rand.Next(0, 100);
            dataSeries1.DataPoints.Add(dataPoint);
            dataPoint = new DataPoint();
            dataPoint.AxisXLabel = "a1";
            dataPoint.XValue = 2;
            dataPoint.YValue = rand.Next(0, 100);
            dataSeries1.DataPoints.Add(dataPoint);
            dataPoint = new DataPoint();
            dataPoint.AxisXLabel = "a1";
            dataPoint.XValue = 4.454;
            dataPoint.YValue = rand.Next(0, 100);
            dataSeries1.DataPoints.Add(dataPoint);
            dataPoint = new DataPoint();
            dataPoint.AxisXLabel = "a1";
            dataPoint.XValue = 5.998;
            dataPoint.YValue = rand.Next(0, 100);
            dataSeries1.DataPoints.Add(dataPoint);
            dataPoint = new DataPoint();
            dataPoint.AxisXLabel = "a1";
            dataPoint.XValue = 3.2;
            dataPoint.YValue = rand.Next(0, 100);
            dataSeries1.DataPoints.Add(dataPoint);

            chart.Series.Add(dataSeries1);

            TestPanel.Children.Add(chart);

            EnqueueConditional(() => { return _isLoaded; });

            EnqueueCallback(() =>
            {
                _htmlElement1.AttachEvent("onclick", new EventHandler<HtmlEventArgs>(this.HtmlElement_OnClick));
            });

            _htmlElement1 = Common.GetDisplayMessageButton(_htmlElement1);
            _htmlElement1.SetStyleAttribute("width", "900px");
            _htmlElement1.SetProperty("value", " DataSeries with decimal XValues. Click here to exit.");
            System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement1);
        }
        #endregion

        #region DataPointRandomXValuesChecking
        /// <summary>
        /// Performance testing DataPoints with random XValues
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void DataPointRandomXValuesChecking()
        {

            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "400px");

            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;


            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Axis axisX = new Axis();
            axisX.Interval = 1;
            chart.AxesX.Add(axisX);

            Random rand = new Random();

            DataSeries dataSeries1 = new DataSeries();

            for (Int32 i = 0; i < 5; i++)
            {
                DataPoint dataPoint = new DataPoint();
                dataPoint.XValue = rand.Next(1, 10);
                dataPoint.YValue = rand.Next(-500, 500);
                dataSeries1.DataPoints.Add(dataPoint);
            }
            for (Int32 i = 0; i < 5; i++)
            {
                DataPoint dataPoint = new DataPoint();
                dataPoint.XValue = rand.Next(1, 10);
                dataPoint.YValue = rand.Next(-500, 500);
                dataSeries1.DataPoints.Add(dataPoint);
            }

            chart.Series.Add(dataSeries1);

            TestPanel.Children.Add(chart);

            EnqueueConditional(() => { return _isLoaded; });

            EnqueueCallback(() =>
            {
                _htmlElement1.AttachEvent("onclick", new EventHandler<HtmlEventArgs>(this.HtmlElement_OnClick));
            });

            _htmlElement1 = Common.GetDisplayMessageButton(_htmlElement1);
            _htmlElement1.SetStyleAttribute("width", "900px");
            _htmlElement1.SetProperty("value", " DataSeries with random XValues. Click here to exit.");
            System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement1);
        }
        #endregion

        #region ZeroPieChecking
        /// <summary>
        /// Checking Pie with single dataPoint with zero value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void ZeroPieChecking()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;


            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            DataSeries dataSeries = new DataSeries();
            dataSeries.RenderAs = RenderAs.Pie;
            DataPoint dataPoint = new DataPoint();
            dataPoint.YValue = 0;
            dataSeries.DataPoints.Add(dataPoint);
            chart.Series.Add(dataSeries);

            CreateAsyncTask(chart,
                () => Assert.IsTrue(_isLoaded, "There is some problem in " + dataSeries.RenderAs + "chart rendering."));

            EnqueueTestComplete();

        }
        #endregion

        #region SinglePieChecking
        /// <summary>
        /// Checking Pie with single dataPoint
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void SinglePieChecking()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            DataSeries dataSeries = new DataSeries();
            dataSeries.RenderAs = RenderAs.Pie;
            DataPoint dataPoint = new DataPoint();
            dataPoint.YValue = 20;
            dataSeries.DataPoints.Add(dataPoint);
            chart.Series.Add(dataSeries);

            CreateAsyncTask(chart,
                () => Assert.IsTrue(_isLoaded, "There is some problem in " + dataSeries.RenderAs + "chart rendering."));

            EnqueueTestComplete();

        }
        #endregion

        #region SingleDoughnutChecking
        /// <summary>
        /// Checking Doughnut with single dataPoint
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void SingleDoughnutChecking()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            DataSeries dataSeries = new DataSeries();
            dataSeries.RenderAs = RenderAs.Doughnut;
            DataPoint dataPoint = new DataPoint();
            dataPoint.YValue = 20;
            dataSeries.DataPoints.Add(dataPoint);
            chart.Series.Add(dataSeries);

            CreateAsyncTask(chart,
                () => Assert.IsTrue(_isLoaded, "There is some problem in " + dataSeries.RenderAs + "chart rendering."));

            EnqueueTestComplete();

        }
        #endregion

        #region PieCheckingWithAllZeroValues
        /// <summary>
        /// Checking Pie with all zero values
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void PieCheckingWithAllZeroValues()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;


            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            DataSeries dataSeries = new DataSeries();
            dataSeries.RenderAs = RenderAs.Pie;
            for (Int32 i = 0; i < 5; i++)
            {
                DataPoint dataPoint = new DataPoint();
                dataPoint.XValue = i + 1;
                dataPoint.YValue = 0;
                dataSeries.DataPoints.Add(dataPoint);
            }
            chart.Series.Add(dataSeries);

            CreateAsyncTask(chart,
               () => Assert.IsTrue(_isLoaded, "There is some problem in " + dataSeries.RenderAs + "chart rendering."));

            EnqueueTestComplete();

        }
        #endregion

        #region DataPointStressTesting
        /// <summary>
        /// Stress testing DataPoints with thousand values under different RenderAs.
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void DataPointStressTesting()
        {
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "400px");

            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;
            chart.View3D = false;
            chart.ScrollingEnabled = false;
            //

            bool isPropertyChanged = false;
            bool isRenderOver = false;

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            TestPanel.Children.Add(chart);

            Axis axisX = new Axis();
            //axisX.Interval = 1;
            chart.AxesX.Add(axisX);

            Random rand = new Random();

            DataSeries dataSeries = new DataSeries();
            dataSeries.RenderAs = RenderAs.Column;

            Int32 numberOfDataPoints = 0;
            Double totalDuration = 0;
            DateTime start = DateTime.UtcNow;
            String msg = Common.AssertAverageDuration(200, 1, delegate
            {
                for (Int32 i = 0; i < 1000; i++)
                {
                    DataPoint dataPoint = new DataPoint();
                    dataPoint.XValue = i + 1;
                    dataPoint.YValue = Math.Sin((Math.PI * 2) * i / 500) * 10;
                    dataSeries.DataPoints.Add(dataPoint);
                    numberOfDataPoints++;
                }
            });
            chart.Series.Add(dataSeries);

            EnqueueConditional(() => { return _isLoaded; });

            EnqueueCallback(() =>
            {
                DateTime end = DateTime.UtcNow;

                totalDuration = (end - start).TotalSeconds;
            });

            EnqueueCallback(() =>
            {
                _htmlElement1 = Common.GetDisplayMessageButton(_htmlElement1);
                _htmlElement1.SetStyleAttribute("width", "900px");
                _htmlElement1.SetProperty("value", dataSeries.RenderAs + " with " + numberOfDataPoints + " DataPoints. ");
                _htmlElement2 = Common.GetDisplayMessageButton(_htmlElement2);
                _htmlElement2.SetStyleAttribute("top", "540px");
                _htmlElement2.SetProperty("value", msg + " Total chart loading time : " + totalDuration + "s");
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement1);
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement2);
            });

            isPropertyChanged = false;

            EnqueueCallback(() =>
            {
                totalDuration = 0;
                start = DateTime.UtcNow;
                dataSeries.RenderAs = RenderAs.Line;
                isPropertyChanged = true;
            });

            EnqueueConditional(() => { return isPropertyChanged; });

            EnqueueCallback(() =>
            {
                DateTime end = DateTime.UtcNow;

                totalDuration = (end - start).TotalSeconds;
            });

            EnqueueCallback(() =>
            {
                _htmlElement1.SetProperty("value", dataSeries.RenderAs + " with " + numberOfDataPoints + " DataPoints. Total load time : " + totalDuration + "s");
                _htmlElement2.RemoveAttribute("value");
                isRenderOver = true;
            });

            EnqueueConditional(() => { return isRenderOver; });
            isPropertyChanged = false;

            EnqueueCallback(() =>
            {
                totalDuration = 0;
                start = DateTime.UtcNow;
                dataSeries.RenderAs = RenderAs.Area;
                isPropertyChanged = true;
            });

            EnqueueConditional(() => { return isPropertyChanged; });

            EnqueueCallback(() =>
            {
                DateTime end = DateTime.UtcNow;

                totalDuration = (end - start).TotalSeconds;
            });

            EnqueueCallback(() =>
            {
                _htmlElement1.SetProperty("value", dataSeries.RenderAs + " with " + numberOfDataPoints + " DataPoints. Total load time : " + totalDuration + "s");
                _htmlElement2.RemoveAttribute("value");
                isRenderOver = true;
            });

            EnqueueConditional(() => { return isRenderOver; });
            isPropertyChanged = false;

            EnqueueCallback(() =>
            {
                totalDuration = 0;
                start = DateTime.UtcNow;
                dataSeries.ShowInLegend = false;
                dataSeries.RenderAs = RenderAs.Pie;
                isPropertyChanged = true;
            });

            EnqueueConditional(() => { return isPropertyChanged; });

            EnqueueCallback(() =>
            {
                DateTime end = DateTime.UtcNow;

                totalDuration = (end - start).TotalSeconds;
            });

            EnqueueCallback(() =>
            {
                _htmlElement1.SetProperty("value", dataSeries.RenderAs + " with " + numberOfDataPoints + " DataPoints. Total load time : " + totalDuration + "s");
                _htmlElement2.RemoveAttribute("value");
                isRenderOver = true;
            });


            EnqueueConditional(() => { return isRenderOver; });
            isPropertyChanged = false;

            EnqueueCallback(() =>
            {
                totalDuration = 0;
                start = DateTime.UtcNow;
                dataSeries.ShowInLegend = false;
                dataSeries.RenderAs = RenderAs.Doughnut;
                isPropertyChanged = true;
            });

            EnqueueConditional(() => { return isPropertyChanged; });

            EnqueueCallback(() =>
            {
                DateTime end = DateTime.UtcNow;

                totalDuration = (end - start).TotalSeconds;
            });

            EnqueueCallback(() =>
            {
                _htmlElement1.SetProperty("value", dataSeries.RenderAs + " with " + numberOfDataPoints + " DataPoints. Total load time : " + totalDuration + "s");
                _htmlElement2.RemoveAttribute("value");
                isRenderOver = true;
            });


            EnqueueConditional(() => { return isRenderOver; });
            isPropertyChanged = false;

            EnqueueCallback(() =>
            {
                totalDuration = 0;
                start = DateTime.UtcNow;
                dataSeries.RenderAs = RenderAs.Bar;
                isPropertyChanged = true;
            });

            EnqueueConditional(() => { return isPropertyChanged; });

            EnqueueCallback(() =>
            {
                DateTime end = DateTime.UtcNow;

                totalDuration = (end - start).TotalSeconds;
            });

            EnqueueCallback(() =>
            {
                _htmlElement1.SetProperty("value", dataSeries.RenderAs + " with " + numberOfDataPoints + " DataPoints. Total load time : " + totalDuration + "s");
                _htmlElement2.RemoveAttribute("value");
                isRenderOver = true;
            });


            EnqueueConditional(() => { return isRenderOver; });
            isPropertyChanged = false;

            EnqueueCallback(() =>
            {
                totalDuration = 0;
                start = DateTime.UtcNow;
                dataSeries.RenderAs = RenderAs.Bubble;
                isPropertyChanged = true;
            });

            EnqueueConditional(() => { return isPropertyChanged; });

            EnqueueCallback(() =>
            {
                DateTime end = DateTime.UtcNow;

                totalDuration = (end - start).TotalSeconds;
            });

            EnqueueCallback(() =>
            {
                _htmlElement1.SetProperty("value", dataSeries.RenderAs + " with " + numberOfDataPoints + " DataPoints. Total load time : " + totalDuration + "s");
                _htmlElement2.RemoveAttribute("value");
                isRenderOver = true;
            });


            EnqueueConditional(() => { return isRenderOver; });
            isPropertyChanged = false;

            EnqueueCallback(() =>
            {
                totalDuration = 0;
                start = DateTime.UtcNow;
                dataSeries.RenderAs = RenderAs.Point;
                isPropertyChanged = true;
            });

            EnqueueCallback(() =>
            {
                DateTime end = DateTime.UtcNow;

                totalDuration = (end - start).TotalSeconds;
            });

            EnqueueCallback(() =>
            {
                _htmlElement1.SetProperty("value", dataSeries.RenderAs + " with " + numberOfDataPoints + " DataPoints. Total load time : " + totalDuration + "s. Click here to exit.");
                _htmlElement2.RemoveAttribute("value");
                isRenderOver = true;
            });

            EnqueueCallback(() =>
            {
                _htmlElement1.AttachEvent("onclick", new EventHandler<HtmlEventArgs>(this.HtmlElement_OnClick));
            });
        }
        #endregion

        #region DataPointDecimalYValuesChecking
        /// <summary>
        /// Testing DataPoints with YValues
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void DataPointDecimalYValuesChecking()
        {
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "400px");

            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;


            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Axis axisX = new Axis();
            axisX.Interval = 1;
            axisX.AxisOrientation = Orientation.Horizontal;
            axisX.AxisType = AxisTypes.Primary;
            chart.AxesX.Add(axisX);

            Random rand = new Random();

            DataSeries dataSeries1 = new DataSeries();
            dataSeries1.RenderAs = RenderAs.Column;

            DataPoint dataPoint = new DataPoint();
            dataPoint.XValue = 1;
            dataPoint.YValue = 1234366799.999999;
            dataSeries1.DataPoints.Add(dataPoint);
            dataPoint = new DataPoint();
            dataPoint.XValue = 2;
            dataPoint.YValue = 2314377565.027241;
            dataSeries1.DataPoints.Add(dataPoint);
            dataPoint = new DataPoint();
            dataPoint.XValue = 3;
            dataPoint.YValue = 8925999900.00000123;
            dataSeries1.DataPoints.Add(dataPoint);
            dataPoint = new DataPoint();
            dataPoint.XValue = 4;
            dataPoint.YValue = 7623122735.00000978;
            dataSeries1.DataPoints.Add(dataPoint);
            dataPoint = new DataPoint();
            dataPoint.XValue = 5;
            dataPoint.YValue = 6354139864.99999999;
            dataSeries1.DataPoints.Add(dataPoint);
            dataPoint = new DataPoint();
            dataPoint.XValue = 6;
            dataPoint.YValue = 4276247835.029725353;
            dataSeries1.DataPoints.Add(dataPoint);

            chart.Series.Add(dataSeries1);

            TestPanel.Children.Add(chart);

            EnqueueConditional(() => { return _isLoaded; });

            EnqueueCallback(() =>
            {
                _htmlElement1.AttachEvent("onclick", new EventHandler<HtmlEventArgs>(this.HtmlElement_OnClick));
            });

            _htmlElement1 = Common.GetDisplayMessageButton(_htmlElement1);
            _htmlElement1.SetStyleAttribute("width", "900px");
            _htmlElement1.SetProperty("value", "DataPoints with larger YValues. Click here to exit.");
            System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement1);
        }
        #endregion

        #region DataPointEventChecking
        /// <summary>
        /// Testing DataPoint Event checking
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void DataPointEventChecking()
        {
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "400px");

            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            TestPanel.Children.Add(chart);

            Random rand = new Random();

            DataSeries dataSeries = new DataSeries();
            dataSeries.RenderAs = RenderAs.Column;

            for (Double i = 0; i < 10; i++)
            {
                DataPoint dataPoint = new DataPoint();
                dataPoint.XValue = rand.Next(0, 10);
                dataPoint.YValue = rand.Next(-100, 100);
                dataPoint.MouseLeftButtonUp += delegate(Object sender, MouseButtonEventArgs e)
                {
                    _htmlElement1.SetProperty("value", "DataPoint YValue: " + (sender as DataPoint).InternalYValue + " MouseLeftButtonUp event fired");
                };

                dataPoint.MouseEnter += delegate(Object sender, MouseEventArgs e)
                {
                    _htmlElement1.SetProperty("value", "DataPoint YValue: " + (sender as DataPoint).InternalYValue + " MouseEnter event fired");
                };

                dataPoint.MouseLeave += delegate(Object sender, MouseEventArgs e)
                {
                    _htmlElement1.SetProperty("value", "Click here to exit. MouseLeave event fired");
                };

                dataPoint.MouseLeftButtonDown += delegate(Object sender, MouseButtonEventArgs e)
                {
                    _htmlElement1.SetProperty("value", "DataPoint YValue: " + (sender as DataPoint).InternalYValue + " MouseLeftButtonDown event fired");
                };

                dataPoint.MouseMove += delegate(Object sender, MouseEventArgs e)
                {
                    _htmlElement1.SetProperty("value", "DataPoint YValue: " + (sender as DataPoint).InternalYValue + " MouseMove event fired");
                };

                dataSeries.DataPoints.Add(dataPoint);
            }
            chart.Series.Add(dataSeries);

            EnqueueConditional(() => { return _isLoaded; });

            EnqueueCallback(() =>
            {
                _htmlElement1.AttachEvent("onclick", new EventHandler<HtmlEventArgs>(this.HtmlElement_OnClick));
            });

            _htmlElement1 = Common.GetDisplayMessageButton(_htmlElement1);
            _htmlElement1.SetStyleAttribute("width", "900px");
            _htmlElement1.SetProperty("value", "Check Mouse events for DataPoints");
            System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement1);
        }
        #endregion

        #region CheckNewPropertyValue
        /// <summary>
        /// Check the Color property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckColorPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;


            DataSeries dataSeries = CreateDataSeries();

            chart.Series.Add(dataSeries);

            Random rand = new Random();
            Double r, g, b;
            EnqueueSleep(3000);

            CreateAsyncTask(chart,
                delegate
                {
                    for (Int32 i = 0; i < 10; i++)
                    {
                        dataSeries.DataPoints[i].Color = new SolidColorBrush(Color.FromArgb((Byte)255, (Byte)(r = rand.Next(0, 100)), (Byte)(g = rand.Next(0, 100)), (Byte)(b = rand.Next(0, 100))));
                        Common.AssertBrushesAreEqual(new SolidColorBrush(Color.FromArgb((Byte)255, (Byte)r, (Byte)g, (Byte)b)), dataSeries.DataPoints[i].Color);
                    }
                });

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the Exploded property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckExplodedPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;


            DataSeries dataSeries = CreateDataSeries();
            dataSeries.RenderAs = RenderAs.Pie;

            chart.Series.Add(dataSeries);

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => dataSeries.DataPoints[5].Exploded = true,
                () => Assert.IsTrue((Boolean)dataSeries.DataPoints[5].Exploded));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the LabelEnabled property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckLabelEnabledPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;


            DataSeries dataSeries = CreateDataSeries();

            chart.Series.Add(dataSeries);

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                delegate
                {
                    for (Int32 i = 0; i < 10; i++)
                    {
                        dataSeries.DataPoints[i].LabelEnabled = true;
                        Assert.IsTrue((Boolean)dataSeries.DataPoints[i].LabelEnabled);
                    }
                });
            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the LabelText property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckLabelTextPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;


            DataSeries dataSeries = CreateDataSeries();
            dataSeries.LabelEnabled = true;

            chart.Series.Add(dataSeries);

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                delegate
                {
                    for (Int32 i = 0; i < 10; i++)
                    {
                        dataSeries.DataPoints[i].LabelText = "NewLabel";
                        Assert.AreEqual("NewLabel", dataSeries.DataPoints[i].LabelText);
                    }
                });
            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the LabelFontFamily property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckLabelFontFamilyPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;


            DataSeries dataSeries = CreateDataSeries();
            dataSeries.LabelEnabled = true;

            chart.Series.Add(dataSeries);

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                delegate
                {
                    for (Int32 i = 0; i < 10; i++)
                    {
                        dataSeries.DataPoints[i].LabelFontFamily = new FontFamily("MS Trebuchet");
                        Assert.AreEqual(new FontFamily("MS Trebuchet"), dataSeries.DataPoints[i].LabelFontFamily);
                    }
                });
            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the LabelFontSize property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckLabelFontSizePropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;


            DataSeries dataSeries = CreateDataSeries();
            dataSeries.LabelEnabled = true;

            chart.Series.Add(dataSeries);

            EnqueueSleep(_sleepTime);
            CreateAsyncTest(chart,
                delegate
                {
                    for (Int32 i = 0; i < 10; i++)
                    {
                        dataSeries.DataPoints[i].LabelFontSize = 14;
                        Assert.AreEqual(14, dataSeries.DataPoints[i].LabelFontSize);
                    }
                });
            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the LabelFontColor property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckLabelFontColorPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;


            DataSeries dataSeries = CreateDataSeries();
            dataSeries.LabelEnabled = true;

            chart.Series.Add(dataSeries);

            EnqueueSleep(_sleepTime);
            CreateAsyncTest(chart,
                delegate
                {
                    for (Int32 i = 0; i < 10; i++)
                    {
                        dataSeries.DataPoints[i].LabelFontColor = new SolidColorBrush(Colors.Blue);
                        Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Blue), dataSeries.DataPoints[i].LabelFontColor);
                    }
                });
            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the LabelFontWeight property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckLabelFontWeightPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;


            DataSeries dataSeries = CreateDataSeries();
            dataSeries.LabelEnabled = true;

            chart.Series.Add(dataSeries);

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                delegate
                {
                    for (Int32 i = 0; i < 10; i++)
                    {
                        dataSeries.DataPoints[i].LabelFontWeight = FontWeights.ExtraBold;
                        Assert.AreEqual(FontWeights.ExtraBold, dataSeries.DataPoints[i].LabelFontWeight);
                    }
                });
            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the LabelFontStyle property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckLabelFontStylePropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;


            DataSeries dataSeries = CreateDataSeries();
            dataSeries.LabelEnabled = true;

            chart.Series.Add(dataSeries);

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                delegate
                {
                    for (Int32 i = 0; i < 10; i++)
                    {
                        dataSeries.DataPoints[i].LabelFontStyle = FontStyles.Italic;
                        Assert.AreEqual(FontStyles.Italic, dataSeries.DataPoints[i].LabelFontStyle);
                    }
                });
            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the LabelBackground property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckLabelBackgroundPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;


            DataSeries dataSeries = CreateDataSeries();
            dataSeries.LabelEnabled = true;

            chart.Series.Add(dataSeries);

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                delegate
                {
                    for (Int32 i = 0; i < 10; i++)
                    {
                        dataSeries.DataPoints[i].LabelBackground = new SolidColorBrush(Colors.DarkGray);
                        Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.DarkGray), dataSeries.DataPoints[i].LabelBackground);
                    }
                });
            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the LabelStyle property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckLabelStylePropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;


            DataSeries dataSeries = CreateDataSeries();
            dataSeries.LabelEnabled = true;

            chart.Series.Add(dataSeries);

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                delegate
                {
                    for (Int32 i = 0; i < 10; i++)
                    {
                        dataSeries.DataPoints[i].LabelStyle = LabelStyles.Inside;
                        Assert.AreEqual(LabelStyles.Inside, dataSeries.DataPoints[i].LabelStyle);
                    }
                });
            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the LabelLineEnabled property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckLabelLineEnabledPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;


            DataSeries dataSeries = CreateDataSeries();
            dataSeries.RenderAs = RenderAs.Pie;
            chart.Series.Add(dataSeries);

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                delegate
                {
                    for (Int32 i = 0; i < 10; i++)
                    {
                        dataSeries.DataPoints[i].LabelLineEnabled = false;
                        Assert.IsFalse((Boolean)dataSeries.DataPoints[i].LabelLineEnabled);
                    }
                });

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the LabelLineColor property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckLabelLineColorPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;


            DataSeries dataSeries = CreateDataSeries();
            dataSeries.RenderAs = RenderAs.Pie;

            chart.Series.Add(dataSeries);

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                delegate
                {
                    for (Int32 i = 0; i < 10; i++)
                    {
                        dataSeries.DataPoints[i].LabelLineColor = new SolidColorBrush(Colors.Red);
                        Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Red), dataSeries.DataPoints[i].LabelLineColor);
                    }
                });

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the LabelLineThickness property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckLabelLineThicknessPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;


            DataSeries dataSeries = CreateDataSeries();
            dataSeries.RenderAs = RenderAs.Pie;

            chart.Series.Add(dataSeries);

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                delegate
                {
                    for (Int32 i = 0; i < 10; i++)
                    {
                        dataSeries.DataPoints[i].LabelLineThickness = 2;
                        Assert.AreEqual(2, dataSeries.DataPoints[i].LabelLineThickness);
                    }
                });
            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the LabelLineStyle property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckLabelLineStylePropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;


            DataSeries dataSeries = CreateDataSeries();
            dataSeries.RenderAs = RenderAs.Pie;

            chart.Series.Add(dataSeries);

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                delegate
                {
                    for (Int32 i = 0; i < 10; i++)
                    {
                        dataSeries.DataPoints[i].LabelLineStyle = LineStyles.Dashed;
                        Assert.AreEqual(LineStyles.Dashed, dataSeries.DataPoints[i].LabelLineStyle);
                    }
                });

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the MarkerEnabled property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckMarkerEnabledPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;


            DataSeries dataSeries = CreateDataSeries();

            chart.Series.Add(dataSeries);

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                delegate
                {
                    for (Int32 i = 0; i < 10; i++)
                    {
                        dataSeries.DataPoints[i].MarkerEnabled = true;
                        Assert.IsTrue((Boolean)dataSeries.DataPoints[i].MarkerEnabled);
                    }
                });
            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the MarkerType property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckMarkerTypePropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;


            DataSeries dataSeries = CreateDataSeries();
            dataSeries.RenderAs = RenderAs.Line;

            chart.Series.Add(dataSeries);

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => dataSeries.DataPoints[1].MarkerType = MarkerTypes.Cross,
                () => dataSeries.DataPoints[2].MarkerType = MarkerTypes.Diamond,
                () => dataSeries.DataPoints[3].MarkerType = MarkerTypes.Square,
                () => dataSeries.DataPoints[4].MarkerType = MarkerTypes.Triangle,
                () => Assert.AreEqual(MarkerTypes.Cross, dataSeries.DataPoints[1].MarkerType),
                () => Assert.AreEqual(MarkerTypes.Diamond, dataSeries.DataPoints[2].MarkerType),
                () => Assert.AreEqual(MarkerTypes.Square, dataSeries.DataPoints[3].MarkerType),
                () => Assert.AreEqual(MarkerTypes.Triangle, dataSeries.DataPoints[4].MarkerType));
            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the MarkerBorderThickness property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckMarkerBorderThicknessPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;


            DataSeries dataSeries = CreateDataSeries();
            dataSeries.RenderAs = RenderAs.Line;

            chart.Series.Add(dataSeries);

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                delegate
                {
                    for (Int32 i = 0; i < 10; i++)
                    {
                        dataSeries.DataPoints[i].MarkerBorderThickness = new Thickness(2);
                        Assert.AreEqual(new Thickness(2), (Thickness)dataSeries.DataPoints[i].MarkerBorderThickness);
                    }
                });

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the MarkerBorderColor property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckMarkerBorderColorPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;


            DataSeries dataSeries = CreateDataSeries();
            dataSeries.RenderAs = RenderAs.Line;

            chart.Series.Add(dataSeries);

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                delegate
                {
                    for (Int32 i = 0; i < 10; i++)
                    {
                        dataSeries.DataPoints[i].MarkerBorderColor = new SolidColorBrush(Colors.Green);
                        Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Green), dataSeries.DataPoints[i].MarkerBorderColor);
                    }
                });

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the MarkerSize property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckMarkerSizePropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;


            DataSeries dataSeries = CreateDataSeries();
            dataSeries.RenderAs = RenderAs.Line;

            chart.Series.Add(dataSeries);

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                delegate
                {
                    for (Int32 i = 0; i < 10; i++)
                    {
                        dataSeries.DataPoints[i].MarkerSize = 15;
                        Assert.AreEqual(15, dataSeries.DataPoints[i].MarkerSize);
                    }
                });
            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the MarkerColor property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckMarkerColorPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;


            DataSeries dataSeries = CreateDataSeries();
            dataSeries.RenderAs = RenderAs.Line;

            chart.Series.Add(dataSeries);

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                delegate
                {
                    for (Int32 i = 0; i < 10; i++)
                    {
                        dataSeries.DataPoints[i].MarkerColor = new SolidColorBrush(Colors.Cyan);
                        Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Cyan), dataSeries.DataPoints[i].MarkerColor);
                    }
                });

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the MarkerScale property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckMarkerScalePropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;


            DataSeries dataSeries = CreateDataSeries();
            dataSeries.RenderAs = RenderAs.Line;

            chart.Series.Add(dataSeries);

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                delegate
                {
                    for (Int32 i = 0; i < 10; i++)
                    {
                        dataSeries.DataPoints[i].MarkerScale = 5;
                        Assert.AreEqual(5, dataSeries.DataPoints[i].MarkerScale);
                    }
                });

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the ToolTipText property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckToolTipTextPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;


            DataSeries dataSeries = CreateDataSeries();

            chart.Series.Add(dataSeries);

            EnqueueSleep(4000);
            CreateAsyncTask(chart,
                () => dataSeries.DataPoints[1].ToolTipText = "#AxisLabel",
                () => Assert.AreEqual("#AxisLabel", dataSeries.DataPoints[1].ToolTipText));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the ShowInLegend property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckShowInLegendPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;


            DataSeries dataSeries = CreateDataSeries();
            chart.Series.Add(dataSeries);

            EnqueueSleep(_sleepTime);
            CreateAsyncTest(chart,
                delegate
                {
                    for (Int32 i = 0; i < 10; i++)
                    {
                        dataSeries.DataPoints[i].ShowInLegend = true;
                        Assert.IsTrue((Boolean)dataSeries.DataPoints[i].ShowInLegend);
                    }
                });

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the LegendText property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckLegendTextPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;


            DataSeries dataSeries = CreateDataSeries();
            chart.Series.Add(dataSeries);

            EnqueueSleep(_sleepTime);
            CreateAsyncTest(chart,
                delegate
                {
                    for (Int32 i = 0; i < 10; i++)
                    {
                        dataSeries.DataPoints[i].ShowInLegend = true;
                        dataSeries.DataPoints[i].LegendText = "DataPoint" + i;
                        Assert.AreEqual("DataPoint" + i, dataSeries.DataPoints[i].LegendText);
                    }
                });

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the BorderThickness` property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckBorderThicknessPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;


            DataSeries dataSeries = CreateDataSeries();
            dataSeries.DataPoints[0].BorderThickness = new Thickness(2);
            chart.Series.Add(dataSeries);

            EnqueueSleep(_sleepTime);
            CreateAsyncTest(chart,
                delegate
                {
                    for (Int32 i = 0; i < 10; i++)
                    {
                        dataSeries.DataPoints[i].BorderThickness = new Thickness(2);
                        Assert.AreEqual(new Thickness(2), dataSeries.DataPoints[i].BorderThickness);
                    }
                });

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the BorderColor property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckBorderColorPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;


            DataSeries dataSeries = CreateDataSeries();
            chart.Series.Add(dataSeries);

            EnqueueSleep(_sleepTime);
            CreateAsyncTest(chart,
                delegate
                {
                    for (Int32 i = 0; i < 10; i++)
                    {
                        dataSeries.DataPoints[i].BorderThickness = new Thickness(2);
                        dataSeries.DataPoints[i].BorderColor = new SolidColorBrush(Colors.Magenta);
                        Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Magenta), dataSeries.DataPoints[i].BorderColor);
                    }
                });

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the BorderStyle property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckBorderStylePropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;


            DataSeries dataSeries = CreateDataSeries();

            chart.Series.Add(dataSeries);

            EnqueueSleep(_sleepTime);
            CreateAsyncTest(chart,
                delegate
                {
                    for (Int32 i = 0; i < 10; i++)
                    {
                        dataSeries.DataPoints[i].BorderThickness = new Thickness(2);
                        dataSeries.DataPoints[i].BorderColor = new SolidColorBrush(Colors.Magenta);
                        dataSeries.DataPoints[i].BorderStyle = BorderStyles.Dashed;
                        Assert.AreEqual(BorderStyles.Dashed, dataSeries.DataPoints[i].BorderStyle);
                    }
                });

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the RadiusX/RadiusY property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckRadiusXYPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;


            DataSeries dataSeries = CreateDataSeries();
            dataSeries.BorderThickness = new Thickness(1);
            chart.Series.Add(dataSeries);

            EnqueueSleep(_sleepTime);
            CreateAsyncTest(chart,
                delegate
                {
                    for (Int32 i = 0; i < 10; i++)
                    {
                        dataSeries.DataPoints[i].RadiusX = new CornerRadius(5, 5, 5, 5);
                        dataSeries.DataPoints[i].RadiusY = new CornerRadius(5, 5, 5, 5);
                        Assert.AreEqual(new CornerRadius(5, 5, 5, 5), dataSeries.DataPoints[i].RadiusX);
                        Assert.AreEqual(new CornerRadius(5, 5, 5, 5), dataSeries.DataPoints[i].RadiusY);
                    }
                });

            EnqueueSleep(_sleepTime);
        }
        #endregion CheckNewPropertyValue

        #region CheckDefaultPropertyValue
        /// <summary>
        /// Check the ShowInLegend default property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckShowInLegendDefaultPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            DataSeries dataSeries = CreateDataSeries();

            chart.Series.Add(dataSeries);

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                delegate
                {
                    for (Int32 i = 0; i < 10; i++)
                        Assert.IsFalse((Boolean)dataSeries.DataPoints[i].ShowInLegend);
                });

            EnqueueTestComplete();
        }

        /// Check the BorderColor default property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckBorderColorDefaultPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            DataSeries dataSeries = CreateDataSeries();

            chart.Series.Add(dataSeries);

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                delegate
                {
                    for (Int32 i = 0; i < 10; i++)
                        Assert.IsNull(dataSeries.DataPoints[i].BorderColor);
                });

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the BorderThickness default property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckBorderThicknessDefaultPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;
            chart.View3D = false;

            DataSeries dataSeries = CreateDataSeries();

            chart.Series.Add(dataSeries);

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                delegate
                {
                    for (Int32 i = 0; i < 10; i++)
                    {
                        Assert.AreEqual(new Thickness(0), dataSeries.DataPoints[i].BorderThickness);
                    }
                });

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the LabelBackground default property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckLabelBackgroundDefaultPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            DataSeries dataSeries = CreateDataSeries();
            dataSeries.LabelEnabled = true;

            chart.Series.Add(dataSeries);

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                delegate
                {
                    for (Int32 i = 0; i < 10; i++)
                    {
                        Assert.IsNull(dataSeries.DataPoints[i].LabelBackground);
                    }
                });

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the LabelEnabled default property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckLabelEnabledDefaultPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            DataSeries dataSeries = CreateDataSeries();

            chart.Series.Add(dataSeries);

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                delegate
                {
                    for (Int32 i = 0; i < 10; i++)
                    {
                        Assert.IsFalse((Boolean)dataSeries.DataPoints[i].LabelEnabled);
                    }
                });

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the LabelEnabled default property value for Pie
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckLabelEnabledDefaultPropertyValue4Pie()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            DataSeries dataSeries = CreateDataSeries();
            dataSeries.RenderAs = RenderAs.Pie;

            chart.Series.Add(dataSeries);

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                delegate
                {
                    for (Int32 i = 0; i < 10; i++)
                    {
                        Assert.IsTrue((Boolean)dataSeries.DataPoints[i].LabelEnabled);
                    }
                });

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the LabelFontColor default property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckLabelFontColorDefaultPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            DataSeries dataSeries = CreateDataSeries();
            dataSeries.LabelEnabled = true;

            chart.Series.Add(dataSeries);

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                delegate
                {
                    for (Int32 i = 0; i < 10; i++)
                    {
                        Assert.IsNull(dataSeries.DataPoints[i].LabelFontColor);
                    }
                });

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the LabelFontFamily default property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckLabelFontFamilyDefaultPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            DataSeries dataSeries = CreateDataSeries();
            dataSeries.LabelEnabled = true;

            chart.Series.Add(dataSeries);

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                delegate
                {
                    for (Int32 i = 0; i < 10; i++)
                    {
                        Assert.AreEqual(new FontFamily("Arial"), dataSeries.DataPoints[i].LabelFontFamily);
                    }
                });

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the LabelFontSize default property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckLabelFontSizeDefaultPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            DataSeries dataSeries = CreateDataSeries();
            dataSeries.LabelEnabled = true;

            chart.Series.Add(dataSeries);

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                delegate
                {
                    for (Int32 i = 0; i < 10; i++)
                    {
                        Assert.AreEqual(10, (Double)dataSeries.DataPoints[i].LabelFontSize, Common.HighPrecisionDelta);
                    }
                });

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the LabelFontStyle default property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckLabelFontStyleDefaultPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            DataSeries dataSeries = CreateDataSeries();
            dataSeries.LabelEnabled = true;

            chart.Series.Add(dataSeries);

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                delegate
                {
                    for (Int32 i = 0; i < 10; i++)
                    {
                        Assert.AreEqual(FontStyles.Normal, dataSeries.DataPoints[i].LabelFontStyle);
                    }
                });

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the LabelFontStyle default property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckLabelFontWeightDefaultPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            DataSeries dataSeries = CreateDataSeries();
            dataSeries.LabelEnabled = true;

            chart.Series.Add(dataSeries);

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                delegate
                {
                    for (Int32 i = 0; i < 10; i++)
                    {
                        Assert.AreEqual(FontWeights.Normal, dataSeries.DataPoints[i].LabelFontWeight);
                    }
                });

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the LabelLineEnabled default property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckLabelLineEnabledDefaultPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            DataSeries dataSeries = CreateDataSeries();
            dataSeries.RenderAs = RenderAs.Pie;

            chart.Series.Add(dataSeries);

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                delegate
                {
                    for (Int32 i = 0; i < 10; i++)
                    {
                        Assert.IsTrue((Boolean)dataSeries.DataPoints[i].LabelLineEnabled);
                    }
                });

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the LabelLineColor default property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckLabelLineColorDefaultPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            DataSeries dataSeries = CreateDataSeries();
            dataSeries.RenderAs = RenderAs.Pie;

            chart.Series.Add(dataSeries);

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                delegate
                {
                    for (Int32 i = 0; i < 10; i++)
                    {
                        Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Gray), dataSeries.DataPoints[i].LabelLineColor);
                    }
                });

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the LabelLineStyle default property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckLabelLineStyleDefaultPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            DataSeries dataSeries = CreateDataSeries();
            dataSeries.RenderAs = RenderAs.Pie;

            chart.Series.Add(dataSeries);

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                delegate
                {
                    for (Int32 i = 0; i < 10; i++)
                    {
                        Assert.AreEqual(LineStyles.Solid, dataSeries.DataPoints[i].LabelLineStyle);
                    }
                });

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the LabelLineThickness default property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckLabelLineThicnessDefaultPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            DataSeries dataSeries = CreateDataSeries();
            dataSeries.RenderAs = RenderAs.Pie;

            chart.Series.Add(dataSeries);

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                delegate
                {
                    for (Int32 i = 0; i < 10; i++)
                    {
                        Assert.AreEqual(0.5, dataSeries.DataPoints[i].LabelLineThickness);
                    }
                });

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the LabelStyle default property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckLabelStyleDefaultPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            DataSeries dataSeries = CreateDataSeries();

            chart.Series.Add(dataSeries);

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                delegate
                {
                    for (Int32 i = 0; i < 10; i++)
                    {
                        Assert.AreEqual(LabelStyles.OutSide, dataSeries.DataPoints[i].LabelStyle);
                    }
                });

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the LabelStyle StackedColumn100 default property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckLabelStyleStackedColumn100DefaultPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            DataSeries dataSeries = CreateDataSeries();
            dataSeries.RenderAs = RenderAs.StackedColumn100;

            chart.Series.Add(dataSeries);

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                delegate
                {
                    for (Int32 i = 0; i < 10; i++)
                    {
                        Assert.AreEqual(LabelStyles.Inside, dataSeries.DataPoints[i].LabelStyle);
                    }
                });

            EnqueueTestComplete();

        }

        /// <summary>
        /// Check the MarkerBorderColor default property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckMarkerBorderColorDefaultPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            DataSeries dataSeries = CreateDataSeries();
            dataSeries.RenderAs = RenderAs.Line;

            chart.Series.Add(dataSeries);

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                delegate
                {
                    for (Int32 i = 0; i < 10; i++)
                    {
                        Assert.IsNotNull(dataSeries.DataPoints[i].MarkerBorderColor);
                    }
                });

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the MarkerBorderThickness default property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckMarkerBorderThicknessDefaultPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            DataSeries dataSeries = CreateDataSeries();
            dataSeries.RenderAs = RenderAs.Line;

            chart.Series.Add(dataSeries);

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                delegate
                {
                    for (Int32 i = 0; i < 10; i++)
                    {
                        Assert.AreEqual(new Thickness((Double)dataSeries.DataPoints[i].MarkerSize / 6), dataSeries.DataPoints[i].MarkerBorderThickness);
                    }
                });

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the MarkerEnabled default property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckMarkerEnabledDefaultPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            DataSeries dataSeries = CreateDataSeries();
            dataSeries.RenderAs = RenderAs.Line;

            chart.Series.Add(dataSeries);

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                delegate
                {
                    for (Int32 i = 0; i < 10; i++)
                    {
                        Assert.IsTrue((Boolean)dataSeries.DataPoints[i].MarkerEnabled);
                    }
                });

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the MarkerScale default property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckMarkerScaleDefaultPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            DataSeries dataSeries = CreateDataSeries();
            dataSeries.RenderAs = RenderAs.Line;

            chart.Series.Add(dataSeries);

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                delegate
                {
                    for (Int32 i = 0; i < 10; i++)
                    {
                        Assert.AreEqual(1, dataSeries.DataPoints[i].MarkerScale);
                    }
                });

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the MarkerSize default property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckMarkerSizeDefaultPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            DataSeries dataSeries = CreateDataSeries();
            dataSeries.RenderAs = RenderAs.Line;

            chart.Series.Add(dataSeries);

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                delegate
                {
                    for (Int32 i = 0; i < 10; i++)
                    {
                        Assert.AreEqual((dataSeries.LineThickness + (dataSeries.LineThickness * 80 / 100)), dataSeries.DataPoints[i].MarkerSize);
                    }
                });

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the MarkerType default property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckMarkerTypeDefaultPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            DataSeries dataSeries = CreateDataSeries();
            dataSeries.RenderAs = RenderAs.Line;

            chart.Series.Add(dataSeries);

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                delegate
                {
                    for (Int32 i = 0; i < 10; i++)
                    {
                        Assert.AreEqual(MarkerTypes.Circle, dataSeries.DataPoints[i].MarkerType);
                    }
                });

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the RadiusX/RadiusY default property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckRadiusXYDefaultPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            DataSeries dataSeries = CreateDataSeries();

            chart.Series.Add(dataSeries);

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                delegate
                {
                    for (Int32 i = 0; i < 10; i++)
                    {
                        Assert.AreEqual(new CornerRadius(0, 0, 0, 0), dataSeries.DataPoints[i].RadiusX);
                        Assert.AreEqual(new CornerRadius(0, 0, 0, 0), dataSeries.DataPoints[i].RadiusY);
                    }
                });

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the ToolTipText default property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckToolTipTextDefaultPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            DataSeries dataSeries = CreateDataSeries();

            chart.Series.Add(dataSeries);

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                delegate
                {
                    for (Int32 i = 0; i < 10; i++)
                    {
                        Assert.AreEqual("#AxisXLabel, #YValue", dataSeries.DataPoints[i].ToolTipText);
                    }
                });

            EnqueueTestComplete();
        }
        #endregion

        /// <summary>
        /// Create a DataSeries
        /// </summary>
        /// <returns></returns>
        private DataSeries CreateDataSeries()
        {
            Random rand = new Random();

            DataSeries dataSeries = new DataSeries();

            for (Int32 i = 0; i < 10; i++)
            {
                DataPoint dataPoint = new DataPoint();
                dataPoint.AxisXLabel = "Label" + i;
                dataPoint.XValue = i + 1;
                dataPoint.YValue = rand.Next(-100, 100);
                dataSeries.DataPoints.Add(dataPoint);
            }
            return dataSeries;
        }

        /// <summary>
        /// Event handler for click event of the Html element
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HtmlElement_OnClick(object sender, HtmlEventArgs e)
        {
            EnqueueTestComplete();
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "100%");
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
        /// Html element reference
        /// </summary>
        private System.Windows.Browser.HtmlElement _htmlElement1;

        /// <summary>
        /// Html element reference
        /// </summary>
        private System.Windows.Browser.HtmlElement _htmlElement2;

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
