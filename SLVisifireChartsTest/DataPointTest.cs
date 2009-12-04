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
            EnqueueDelay(_sleepTime);

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

            EnqueueDelay(_sleepTime);
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
            EnqueueDelay(_sleepTime);

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

            EnqueueDelay(_sleepTime);

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

            EnqueueDelay(_sleepTime);

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

        #region ZeroDoughnutChecking
        /// <summary>
        /// Checking Doughnut with single dataPoint with zero value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void ZeroDoughnutChecking()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;


            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            DataSeries dataSeries = new DataSeries();
            dataSeries.RenderAs = RenderAs.Doughnut;
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

        #region SingleColumnChecking
        /// <summary>
        /// Checking Column with single dataPoint
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void SingleColumnChecking()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            DataSeries dataSeries = new DataSeries();
            dataSeries.RenderAs = RenderAs.Column;
            DataPoint dataPoint = new DataPoint();
            dataPoint.YValue = 18;
            dataSeries.DataPoints.Add(dataPoint);
            chart.Series.Add(dataSeries);

            CreateAsyncTask(chart,
                () => Assert.IsTrue(_isLoaded, "There is some problem in " + dataSeries.RenderAs + "chart rendering."));

            EnqueueTestComplete();

        }
        #endregion

        #region SingleBarChecking
        /// <summary>
        /// Checking Bar with single dataPoint
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void SingleBarChecking()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            DataSeries dataSeries = new DataSeries();
            dataSeries.RenderAs = RenderAs.Bar;
            DataPoint dataPoint = new DataPoint();
            dataPoint.YValue = 18;
            dataSeries.DataPoints.Add(dataPoint);
            chart.Series.Add(dataSeries);

            CreateAsyncTask(chart,
                () => Assert.IsTrue(_isLoaded, "There is some problem in " + dataSeries.RenderAs + "chart rendering."));

            EnqueueTestComplete();

        }
        #endregion

        #region SingleAreaChecking
        /// <summary>
        /// Checking Area with single dataPoint
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void SingleAreaChecking()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            DataSeries dataSeries = new DataSeries();
            dataSeries.RenderAs = RenderAs.Area;
            DataPoint dataPoint = new DataPoint();
            dataPoint.YValue = 18;
            dataSeries.DataPoints.Add(dataPoint);
            chart.Series.Add(dataSeries);

            CreateAsyncTask(chart,
                () => Assert.IsTrue(_isLoaded, "There is some problem in " + dataSeries.RenderAs + "chart rendering."));

            EnqueueTestComplete();

        }
        #endregion

        #region SingleLineChecking
        /// <summary>
        /// Checking Line with single dataPoint
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void SingleLineChecking()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            DataSeries dataSeries = new DataSeries();
            dataSeries.RenderAs = RenderAs.Line;
            DataPoint dataPoint = new DataPoint();
            dataPoint.YValue = 18;
            dataSeries.DataPoints.Add(dataPoint);
            chart.Series.Add(dataSeries);

            CreateAsyncTask(chart,
                () => Assert.IsTrue(_isLoaded, "There is some problem in " + dataSeries.RenderAs + "chart rendering."));

            EnqueueTestComplete();

        }
        #endregion

        #region SinglePointChecking
        /// <summary>
        /// Checking Point with single dataPoint
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void SinglePointChecking()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            DataSeries dataSeries = new DataSeries();
            dataSeries.RenderAs = RenderAs.Point;
            DataPoint dataPoint = new DataPoint();
            dataPoint.YValue = 18;
            dataSeries.DataPoints.Add(dataPoint);
            chart.Series.Add(dataSeries);

            CreateAsyncTask(chart,
                () => Assert.IsTrue(_isLoaded, "There is some problem in " + dataSeries.RenderAs + "chart rendering."));

            EnqueueTestComplete();

        }
        #endregion

        #region SingleBubbleChecking
        /// <summary>
        /// Checking Bubble with single dataPoint
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void SingleBubbleChecking()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            DataSeries dataSeries = new DataSeries();
            dataSeries.RenderAs = RenderAs.Bubble;
            DataPoint dataPoint = new DataPoint();
            dataPoint.YValue = 18;
            dataSeries.DataPoints.Add(dataPoint);
            chart.Series.Add(dataSeries);

            CreateAsyncTask(chart,
                () => Assert.IsTrue(_isLoaded, "There is some problem in " + dataSeries.RenderAs + "chart rendering."));

            EnqueueTestComplete();

        }
        #endregion

        #region SingleStackedColumnChecking
        /// <summary>
        /// Checking StackedColumn with single dataPoint
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void SingleStackedColumnChecking()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            DataSeries dataSeries = new DataSeries();
            dataSeries.RenderAs = RenderAs.StackedColumn;
            DataPoint dataPoint = new DataPoint();
            dataPoint.YValue = 18;
            dataSeries.DataPoints.Add(dataPoint);
            chart.Series.Add(dataSeries);

            CreateAsyncTask(chart,
                () => Assert.IsTrue(_isLoaded, "There is some problem in " + dataSeries.RenderAs + "chart rendering."));

            EnqueueTestComplete();

        }
        #endregion

        #region SingleStackedColumn100Checking
        /// <summary>
        /// Checking StackedColumn100 with single dataPoint
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void SingleStackedColumn100Checking()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            DataSeries dataSeries = new DataSeries();
            dataSeries.RenderAs = RenderAs.StackedColumn100;
            DataPoint dataPoint = new DataPoint();
            dataPoint.YValue = 18;
            dataSeries.DataPoints.Add(dataPoint);
            chart.Series.Add(dataSeries);

            CreateAsyncTask(chart,
                () => Assert.IsTrue(_isLoaded, "There is some problem in " + dataSeries.RenderAs + "chart rendering."));

            EnqueueTestComplete();

        }
        #endregion

        #region SingleStackedBarChecking
        /// <summary>
        /// Checking StackedBar with single dataPoint
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void SingleStackedBarChecking()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            DataSeries dataSeries = new DataSeries();
            dataSeries.RenderAs = RenderAs.StackedBar;
            DataPoint dataPoint = new DataPoint();
            dataPoint.YValue = 18;
            dataSeries.DataPoints.Add(dataPoint);
            chart.Series.Add(dataSeries);

            CreateAsyncTask(chart,
                () => Assert.IsTrue(_isLoaded, "There is some problem in " + dataSeries.RenderAs + "chart rendering."));

            EnqueueTestComplete();

        }
        #endregion

        #region SingleStackedBar100Checking
        /// <summary>
        /// Checking StackedBar100 with single dataPoint
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void SingleStackedBar100Checking()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            DataSeries dataSeries = new DataSeries();
            dataSeries.RenderAs = RenderAs.StackedBar100;
            DataPoint dataPoint = new DataPoint();
            dataPoint.YValue = 18;
            dataSeries.DataPoints.Add(dataPoint);
            chart.Series.Add(dataSeries);

            CreateAsyncTask(chart,
                () => Assert.IsTrue(_isLoaded, "There is some problem in " + dataSeries.RenderAs + "chart rendering."));

            EnqueueTestComplete();

        }
        #endregion

        #region SingleStackedAreaChecking
        /// <summary>
        /// Checking StackedArea with single dataPoint
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void SingleStackedAreaChecking()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            DataSeries dataSeries = new DataSeries();
            dataSeries.RenderAs = RenderAs.StackedArea;
            DataPoint dataPoint = new DataPoint();
            dataPoint.YValue = 18;
            dataSeries.DataPoints.Add(dataPoint);
            chart.Series.Add(dataSeries);

            CreateAsyncTask(chart,
                () => Assert.IsTrue(_isLoaded, "There is some problem in " + dataSeries.RenderAs + "chart rendering."));

            EnqueueTestComplete();

        }
        #endregion

        #region SingleStackedArea100Checking
        /// <summary>
        /// Checking StackedArea100 with single dataPoint
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void SingleStackedArea100Checking()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            DataSeries dataSeries = new DataSeries();
            dataSeries.RenderAs = RenderAs.StackedArea100;
            DataPoint dataPoint = new DataPoint();
            dataPoint.YValue = 18;
            dataSeries.DataPoints.Add(dataPoint);
            chart.Series.Add(dataSeries);

            CreateAsyncTask(chart,
                () => Assert.IsTrue(_isLoaded, "There is some problem in " + dataSeries.RenderAs + "chart rendering."));

            EnqueueTestComplete();

        }
        #endregion

        #region SinglePieNullValueChecking
        /// <summary>
        /// Checking Pie with NullValue dataPoint
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void SinglePieNullValueChecking()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            DataSeries dataSeries = new DataSeries();
            dataSeries.RenderAs = RenderAs.Pie;
            DataPoint dataPoint = new DataPoint();
            dataPoint.YValue = Double.NaN;
            dataSeries.DataPoints.Add(dataPoint);
            chart.Series.Add(dataSeries);

            CreateAsyncTask(chart,
                () => Assert.IsTrue(_isLoaded, "There is some problem in " + dataSeries.RenderAs + "chart rendering."));

            EnqueueTestComplete();

        }
        #endregion

        #region SingleDoughnutNullValueChecking
        /// <summary>
        /// Checking Doughnut with NullValue dataPoint
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void SingleDoughnutNullValueChecking()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            DataSeries dataSeries = new DataSeries();
            dataSeries.RenderAs = RenderAs.Doughnut;
            DataPoint dataPoint = new DataPoint();
            dataPoint.YValue = Double.NaN;
            dataSeries.DataPoints.Add(dataPoint);
            chart.Series.Add(dataSeries);

            CreateAsyncTask(chart,
                () => Assert.IsTrue(_isLoaded, "There is some problem in " + dataSeries.RenderAs + "chart rendering."));

            EnqueueTestComplete();

        }
        #endregion

        #region SingleColumnNullValueChecking
        /// <summary>
        /// Checking Column with NullValue dataPoint
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void SingleColumnNullValueChecking()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            DataSeries dataSeries = new DataSeries();
            dataSeries.RenderAs = RenderAs.Column;
            DataPoint dataPoint = new DataPoint();
            dataPoint.YValue = Double.NaN;
            dataSeries.DataPoints.Add(dataPoint);
            chart.Series.Add(dataSeries);

            CreateAsyncTask(chart,
                () => Assert.IsTrue(_isLoaded, "There is some problem in " + dataSeries.RenderAs + "chart rendering."));

            EnqueueTestComplete();

        }
        #endregion

        #region SingleBarNullValueChecking
        /// <summary>
        /// Checking Bar with NullValue dataPoint
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void SingleBarNullValueChecking()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            DataSeries dataSeries = new DataSeries();
            dataSeries.RenderAs = RenderAs.Bar;
            DataPoint dataPoint = new DataPoint();
            dataPoint.YValue = Double.NaN;
            dataSeries.DataPoints.Add(dataPoint);
            chart.Series.Add(dataSeries);

            CreateAsyncTask(chart,
                () => Assert.IsTrue(_isLoaded, "There is some problem in " + dataSeries.RenderAs + "chart rendering."));

            EnqueueTestComplete();

        }
        #endregion

        #region SingleAreaNullValueChecking
        /// <summary>
        /// Checking Area with NullValue dataPoint
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void SingleAreaNullValueChecking()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            DataSeries dataSeries = new DataSeries();
            dataSeries.RenderAs = RenderAs.Area;
            DataPoint dataPoint = new DataPoint();
            dataPoint.YValue = Double.NaN;
            dataSeries.DataPoints.Add(dataPoint);
            chart.Series.Add(dataSeries);

            CreateAsyncTask(chart,
                () => Assert.IsTrue(_isLoaded, "There is some problem in " + dataSeries.RenderAs + "chart rendering."));

            EnqueueTestComplete();

        }
        #endregion

        #region SingleLineNullValueChecking
        /// <summary>
        /// Checking Line with NullValue dataPoint
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void SingleLineNullValueChecking()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            DataSeries dataSeries = new DataSeries();
            dataSeries.RenderAs = RenderAs.Line;
            DataPoint dataPoint = new DataPoint();
            dataPoint.YValue = Double.NaN;
            dataSeries.DataPoints.Add(dataPoint);
            chart.Series.Add(dataSeries);

            CreateAsyncTask(chart,
                () => Assert.IsTrue(_isLoaded, "There is some problem in " + dataSeries.RenderAs + "chart rendering."));

            EnqueueTestComplete();

        }
        #endregion

        #region SinglePointNullValueChecking
        /// <summary>
        /// Checking Point with NullValue dataPoint
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void SinglePointNullValueChecking()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            DataSeries dataSeries = new DataSeries();
            dataSeries.RenderAs = RenderAs.Point;
            DataPoint dataPoint = new DataPoint();
            dataPoint.YValue = Double.NaN;
            dataSeries.DataPoints.Add(dataPoint);
            chart.Series.Add(dataSeries);

            CreateAsyncTask(chart,
                () => Assert.IsTrue(_isLoaded, "There is some problem in " + dataSeries.RenderAs + "chart rendering."));

            EnqueueTestComplete();

        }
        #endregion

        #region SingleBubbleNullValueChecking
        /// <summary>
        /// Checking Bubble with NullValue dataPoint
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void SingleBubbleNullValueChecking()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            DataSeries dataSeries = new DataSeries();
            dataSeries.RenderAs = RenderAs.Bubble;
            DataPoint dataPoint = new DataPoint();
            dataPoint.YValue = Double.NaN;
            dataSeries.DataPoints.Add(dataPoint);
            chart.Series.Add(dataSeries);

            CreateAsyncTask(chart,
                () => Assert.IsTrue(_isLoaded, "There is some problem in " + dataSeries.RenderAs + "chart rendering."));

            EnqueueTestComplete();

        }
        #endregion

        #region SingleStackedColumnNullValueChecking
        /// <summary>
        /// Checking StackedColumn with NullValue dataPoint
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void SingleStackedColumnNullValueChecking()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            DataSeries dataSeries = new DataSeries();
            dataSeries.RenderAs = RenderAs.StackedColumn;
            DataPoint dataPoint = new DataPoint();
            dataPoint.YValue = Double.NaN;
            dataSeries.DataPoints.Add(dataPoint);
            chart.Series.Add(dataSeries);

            CreateAsyncTask(chart,
                () => Assert.IsTrue(_isLoaded, "There is some problem in " + dataSeries.RenderAs + "chart rendering."));

            EnqueueTestComplete();

        }
        #endregion

        #region SingleStackedColumn100NullValueChecking
        /// <summary>
        /// Checking StackedColumn100 with NullValue dataPoint
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void SingleStackedColumn100NullValueChecking()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            DataSeries dataSeries = new DataSeries();
            dataSeries.RenderAs = RenderAs.StackedColumn100;
            DataPoint dataPoint = new DataPoint();
            dataPoint.YValue = Double.NaN;
            dataSeries.DataPoints.Add(dataPoint);
            chart.Series.Add(dataSeries);

            CreateAsyncTask(chart,
                () => Assert.IsTrue(_isLoaded, "There is some problem in " + dataSeries.RenderAs + "chart rendering."));

            EnqueueTestComplete();

        }
        #endregion

        #region SingleStackedBarNullValueChecking
        /// <summary>
        /// Checking StackedBar with NullValue dataPoint
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void SingleStackedBarNullValueChecking()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            DataSeries dataSeries = new DataSeries();
            dataSeries.RenderAs = RenderAs.StackedBar;
            DataPoint dataPoint = new DataPoint();
            dataPoint.YValue = Double.NaN;
            dataSeries.DataPoints.Add(dataPoint);
            chart.Series.Add(dataSeries);

            CreateAsyncTask(chart,
                () => Assert.IsTrue(_isLoaded, "There is some problem in " + dataSeries.RenderAs + "chart rendering."));

            EnqueueTestComplete();

        }
        #endregion

        #region SingleStackedBar100NullValueChecking
        /// <summary>
        /// Checking StackedBar100 with NullValue dataPoint
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void SingleStackedBar100NullValueChecking()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            DataSeries dataSeries = new DataSeries();
            dataSeries.RenderAs = RenderAs.StackedBar100;
            DataPoint dataPoint = new DataPoint();
            dataPoint.YValue = Double.NaN;
            dataSeries.DataPoints.Add(dataPoint);
            chart.Series.Add(dataSeries);

            CreateAsyncTask(chart,
                () => Assert.IsTrue(_isLoaded, "There is some problem in " + dataSeries.RenderAs + "chart rendering."));

            EnqueueTestComplete();

        }
        #endregion

        #region SingleStackedAreaNullValueChecking
        /// <summary>
        /// Checking StackedArea with NullValue dataPoint
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void SingleStackedAreaNullValueChecking()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            DataSeries dataSeries = new DataSeries();
            dataSeries.RenderAs = RenderAs.StackedArea;
            DataPoint dataPoint = new DataPoint();
            dataPoint.YValue = Double.NaN;
            dataSeries.DataPoints.Add(dataPoint);
            chart.Series.Add(dataSeries);

            CreateAsyncTask(chart,
                () => Assert.IsTrue(_isLoaded, "There is some problem in " + dataSeries.RenderAs + "chart rendering."));

            EnqueueTestComplete();

        }
        #endregion

        #region SingleStackedArea100NullValueChecking
        /// <summary>
        /// Checking StackedArea100 with NullValue dataPoint
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void SingleStackedArea100NullValueChecking()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            DataSeries dataSeries = new DataSeries();
            dataSeries.RenderAs = RenderAs.StackedArea100;
            DataPoint dataPoint = new DataPoint();
            dataPoint.YValue = Double.NaN;
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
            chart.View3D = true;

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

        #region DoughnutCheckingWithAllZeroValues
        /// <summary>
        /// Checking Doughnut with all zero values
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void DoughnutCheckingWithAllZeroValues()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;
            chart.View3D = true;

            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            DataSeries dataSeries = new DataSeries();
            dataSeries.RenderAs = RenderAs.Doughnut;
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

            bool isPropertyChanged = false;
            bool isRenderOver = false;

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            TestPanel.Children.Add(chart);

            Axis axisX = new Axis();
            chart.AxesX.Add(axisX);

            Random rand = new Random();

            DataSeries dataSeries = new DataSeries();
            dataSeries.RenderAs = RenderAs.Column;
            dataSeries.LabelEnabled = false;

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
        /// Check the Enabled property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckEnabledPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            DataSeries dataSeries = CreateDataSeries();

            chart.Series.Add(dataSeries);

            Random rand = new Random();
            EnqueueDelay(2000);

            CreateAsyncTask(chart,
                delegate
                {
                    for (Int32 i = 0; i < 10; i++)
                    {
                        dataSeries.DataPoints[i].Enabled = false;
                        Assert.IsFalse((Boolean)dataSeries.DataPoints[i].Enabled);
                    }
                });

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the ZValue property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckZValuePropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            DataSeries dataSeries = CreateDataSeries();
            dataSeries.RenderAs = RenderAs.Bubble;

            List<Double> zValue = new List<Double>();
            Double z = 0;

            chart.Series.Add(dataSeries);

            Random rand = new Random();
            EnqueueDelay(2000);

            CreateAsyncTask(chart,
                delegate
                {
                    for (Int32 i = 0; i < 10; i++)
                    {
                        dataSeries.DataPoints[i].ZValue = (z = rand.Next(100, 500));
                        zValue.Add(z);
                        Assert.AreEqual(zValue[i], dataSeries.DataPoints[i].ZValue);
                    }
                });

            EnqueueTestComplete();
        }

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
            EnqueueDelay(3000);

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

            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                () => dataSeries.DataPoints[5].Exploded = true,
                () => Assert.IsTrue((Boolean)dataSeries.DataPoints[5].Exploded));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the HrefAndHrefTarget property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckHrefAndHrefTargetPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            DataSeries dataSeries = CreateDataSeries();
            dataSeries.RenderAs = RenderAs.Pie;

            chart.Series.Add(dataSeries);

            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                delegate
                {
                    for (Int32 i = 0; i < 10; i++)
                    {
                        dataSeries.DataPoints[i].Href = "http://www.visifire.com";
                        dataSeries.DataPoints[i].HrefTarget = HrefTargets._blank;
                        Assert.AreEqual("http://www.visifire.com", dataSeries.DataPoints[i].Href);
                        Assert.AreEqual(HrefTargets._blank, dataSeries.DataPoints[i].HrefTarget);
                    }
                });
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

            EnqueueDelay(_sleepTime);
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

            EnqueueDelay(_sleepTime);
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

            EnqueueDelay(_sleepTime);
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

            EnqueueDelay(_sleepTime);
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

            EnqueueDelay(_sleepTime);
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

            EnqueueDelay(_sleepTime);
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

            EnqueueDelay(_sleepTime);
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

            EnqueueDelay(_sleepTime);
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

            EnqueueDelay(_sleepTime);
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

            EnqueueDelay(_sleepTime);
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

            EnqueueDelay(_sleepTime);
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

            EnqueueDelay(_sleepTime);
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

            EnqueueDelay(_sleepTime);
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

            EnqueueDelay(_sleepTime);
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

            EnqueueDelay(_sleepTime);
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

            EnqueueDelay(_sleepTime);
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

            EnqueueDelay(_sleepTime);
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

            EnqueueDelay(_sleepTime);
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

            EnqueueDelay(_sleepTime);
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

            EnqueueDelay(_sleepTime);
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

            EnqueueDelay(4000);
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

            EnqueueDelay(_sleepTime);
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

            EnqueueDelay(_sleepTime);
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
            dataSeries.DataPoints[0].InternalBorderThickness = new Thickness(2);
            chart.Series.Add(dataSeries);

            EnqueueDelay(_sleepTime);
            CreateAsyncTest(chart,
                delegate
                {
                    for (Int32 i = 0; i < 10; i++)
                    {
                        dataSeries.DataPoints[i].InternalBorderThickness = new Thickness(2);
                        Assert.AreEqual(new Thickness(2), dataSeries.DataPoints[i].InternalBorderThickness);
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

            EnqueueDelay(_sleepTime);
            CreateAsyncTest(chart,
                delegate
                {
                    for (Int32 i = 0; i < 10; i++)
                    {
                        dataSeries.DataPoints[i].InternalBorderThickness = new Thickness(2);
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

            EnqueueDelay(_sleepTime);
            CreateAsyncTest(chart,
                delegate
                {
                    for (Int32 i = 0; i < 10; i++)
                    {
                        dataSeries.DataPoints[i].InternalBorderThickness = new Thickness(2);
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
            dataSeries.InternalBorderThickness = new Thickness(1);
            chart.Series.Add(dataSeries);

            EnqueueDelay(_sleepTime);
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

            EnqueueDelay(_sleepTime);
        }

        /// <summary>
        /// Check the Opacity property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckOpacityPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;


            DataSeries dataSeries = CreateDataSeries();
            chart.Series.Add(dataSeries);

            EnqueueDelay(_sleepTime);
            CreateAsyncTest(chart,
                delegate
                {
                    for (Int32 i = 0; i < 10; i++)
                    {
                        dataSeries.DataPoints[i].InternalOpacity = 0.5;
                        Assert.AreEqual(0.5, dataSeries.DataPoints[i].InternalOpacity, Common.HighPrecisionDelta);
                    }
                });

            EnqueueDelay(_sleepTime);
        }

        /// <summary>
        /// Check the Cursor property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckCursorPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;


            DataSeries dataSeries = CreateDataSeries();
            chart.Series.Add(dataSeries);

            EnqueueDelay(_sleepTime);
            CreateAsyncTest(chart,
                delegate
                {
                    for (Int32 i = 0; i < 10; i++)
                    {
                        dataSeries.DataPoints[i].Cursor = Cursors.Hand;
                        Assert.AreEqual(Cursors.Hand, dataSeries.DataPoints[i].Cursor);
                    }
                });

            EnqueueDelay(_sleepTime);
        }

        #endregion CheckNewPropertyValue

        #region CheckDefaultPropertyValue

        /// <summary>
        /// Check the Enabled default property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckEnabledDefaultPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            DataSeries dataSeries = CreateDataSeries();

            chart.Series.Add(dataSeries);

            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                delegate
                {
                    for (Int32 i = 0; i < 10; i++)
                        Assert.IsTrue((Boolean)dataSeries.DataPoints[i].Enabled);
                });

            EnqueueTestComplete();
        }

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

            EnqueueDelay(_sleepTime);
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

            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                delegate
                {
                    for (Int32 i = 0; i < 10; i++)
                        Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Black), dataSeries.DataPoints[i].BorderColor);
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

            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                delegate
                {
                    for (Int32 i = 0; i < 10; i++)
                    {
                        Assert.AreEqual(new Thickness(0), dataSeries.DataPoints[i].InternalBorderThickness);
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

            EnqueueDelay(_sleepTime);
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

            EnqueueDelay(_sleepTime);
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

            EnqueueDelay(_sleepTime);
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

            EnqueueDelay(_sleepTime);
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

            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                delegate
                {
                    for (Int32 i = 0; i < 10; i++)
                    {
                        Assert.AreEqual(new FontFamily("Verdana"), dataSeries.DataPoints[i].LabelFontFamily);
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

            EnqueueDelay(_sleepTime);
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

            EnqueueDelay(_sleepTime);
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

            EnqueueDelay(_sleepTime);
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

            EnqueueDelay(_sleepTime);
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

            EnqueueDelay(_sleepTime);
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

            EnqueueDelay(_sleepTime);
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

            EnqueueDelay(_sleepTime);
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

            EnqueueDelay(_sleepTime);
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

            EnqueueDelay(_sleepTime);
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

            EnqueueDelay(_sleepTime);
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

            EnqueueDelay(_sleepTime);
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

            EnqueueDelay(_sleepTime);
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

            EnqueueDelay(_sleepTime);
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

            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                delegate
                {
                    for (Int32 i = 0; i < 10; i++)
                    {
                        Assert.AreEqual((dataSeries.LineThickness * 2), dataSeries.DataPoints[i].MarkerSize);
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

            EnqueueDelay(_sleepTime);
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

            EnqueueDelay(_sleepTime);
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

            EnqueueDelay(_sleepTime);
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

        #region TestSingleDataPointWidth
        [TestMethod]
        [Asynchronous]
        public void TestSingleDataPointWidth()
        {
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "400px");

            _chart = new Chart();
            _chart.Width = 400;
            _chart.Height = 300;

            Random rand = new Random();

            _isLoaded = false;
            _chart.Loaded += new RoutedEventHandler(chart_Loaded);

            TestPanel.Children.Add(_chart);

            DataSeries dataSeries = new DataSeries();
            dataSeries.RenderAs = RenderAs.Column;
            dataSeries.DataPoints.Add(new DataPoint() { YValue = 200 });
            _chart.Series.Add(dataSeries);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                _chart.DataPointWidth = 10;
            });

            EnqueueCallback(() =>
            {
                _htmlElement1 = Common.GetDisplayMessageButton(_htmlElement1);
                _htmlElement1.SetStyleAttribute("width", "900px");
                _htmlElement1.SetProperty("value", "Click here to check DataPoint Width at realtime.");
                _htmlElement2 = Common.GetDisplayMessageButton(_htmlElement2);
                _htmlElement2.SetStyleAttribute("top", "540px");
                _htmlElement2.SetProperty("value", " Click here to exit.");
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement1);
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement2);
            });

            EnqueueDelay(_sleepTime);
            EnqueueCallback(() =>
            {
                _htmlElement1.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.HtmlElement1_OnClick));
                _htmlElement2.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.HtmlElement2_OnClick));
            });
        }
        #endregion

        #region TestSingleDataPointWidthIn3D
        [TestMethod]
        [Asynchronous]
        public void TestSingleDataPointWidthIn3D()
        {
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "400px");

            _chart = new Chart();
            _chart.Width = 400;
            _chart.Height = 300;
            _chart.View3D = true;

            Random rand = new Random();

            _isLoaded = false;
            _chart.Loaded += new RoutedEventHandler(chart_Loaded);

            TestPanel.Children.Add(_chart);

            DataSeries dataSeries = new DataSeries();
            dataSeries.RenderAs = RenderAs.Column;
            dataSeries.DataPoints.Add(new DataPoint() { YValue = 200 });
            _chart.Series.Add(dataSeries);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                _chart.DataPointWidth = 10;
            });

            EnqueueCallback(() =>
            {
                _htmlElement1 = Common.GetDisplayMessageButton(_htmlElement1);
                _htmlElement1.SetStyleAttribute("width", "900px");
                _htmlElement1.SetProperty("value", "Click here to check DataPoint Width at realtime.");
                _htmlElement2 = Common.GetDisplayMessageButton(_htmlElement2);
                _htmlElement2.SetStyleAttribute("top", "540px");
                _htmlElement2.SetProperty("value", " Click here to exit.");
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement1);
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement2);
            });

            EnqueueDelay(_sleepTime);
            EnqueueCallback(() =>
            {
                _htmlElement1.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.HtmlElement1_OnClick));
                _htmlElement2.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.HtmlElement2_OnClick));
            });
        }
        #endregion

        #region TestDataPointWidth4DecimalXValue
        /// <summary>
        /// Test DataPoint width with Decimal XValues
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void TestDataPointWidth4DecimalXValue()
        {
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "400px");

            _chart = new Chart();
            _chart.Width = 500;
            _chart.Height = 300;

            _isLoaded = false;
            _chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Random rand = new Random();

            TestPanel.Children.Add(_chart);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                DataSeries dataSeries1 = new DataSeries();
                dataSeries1.RenderAs = RenderAs.Column;

                for (Double i = 0; i < 10; i++)
                {
                    DataPoint dataPoint = new DataPoint();
                    dataPoint.XValue = rand.NextDouble() + i;
                    dataPoint.YValue = rand.Next(0, 100);
                    dataSeries1.DataPoints.Add(dataPoint);
                }
                _chart.Series.Add(dataSeries1);
            });

            EnqueueCallback(() =>
            {
                _htmlElement1 = Common.GetDisplayMessageButton(_htmlElement1);
                _htmlElement1.SetStyleAttribute("width", "900px");
                _htmlElement1.SetProperty("value", "Click here to check DataPoint Width at realtime.");
                _htmlElement2 = Common.GetDisplayMessageButton(_htmlElement2);
                _htmlElement2.SetStyleAttribute("top", "540px");
                _htmlElement2.SetProperty("value", " Click here to exit.");
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement1);
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement2);
            });

            EnqueueDelay(_sleepTime);
            EnqueueCallback(() =>
            {
                _htmlElement1.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.HtmlElement1_OnClick));
                _htmlElement2.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.HtmlElement2_OnClick));
            });
        }
        #endregion

        #region TestMultiSeriesDataPointWidth4DecimalXValue
        /// <summary>
        /// Test multiseries DataPoint width with Decimal XValues
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void TestMultiSeriesDataPointWidth4DecimalXValue()
        {
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "400px");

            _chart = new Chart();
            _chart.Width = 500;
            _chart.Height = 300;

            _isLoaded = false;
            _chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Random rand = new Random();

            List<Double> xList = new List<Double>();
            List<Double> yList = new List<Double>();
            Double y = 0;

            TestPanel.Children.Add(_chart);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueDelay(_sleepTime);

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
                _chart.Series.Add(dataSeries1);
            });

            EnqueueDelay(_sleepTime);

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
                _chart.Series.Add(dataSeries2);
            });

            EnqueueCallback(() =>
            {
                _htmlElement1 = Common.GetDisplayMessageButton(_htmlElement1);
                _htmlElement1.SetStyleAttribute("width", "900px");
                _htmlElement1.SetProperty("value", "Click here to check DataPoint Width at realtime.");
                _htmlElement2 = Common.GetDisplayMessageButton(_htmlElement2);
                _htmlElement2.SetStyleAttribute("top", "540px");
                _htmlElement2.SetProperty("value", " Click here to exit.");
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement1);
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement2);
            });

            EnqueueDelay(_sleepTime);
            EnqueueCallback(() =>
            {
                _htmlElement1.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.HtmlElement1_OnClick));
                _htmlElement2.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.HtmlElement2_OnClick));
            });
        }
        #endregion

        #region TestMultiSeriesDataPointWidth
        /// <summary>
        /// Test DataPoint width in multiseries Column chart
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void TestMultiSeriesDataPointWidth()
        {
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "400px");

            _chart = new Chart();
            _chart.Width = 500;
            _chart.Height = 300;

            _isLoaded = false;
            _chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Random rand = new Random();

            TestPanel.Children.Add(_chart);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                for (Int32 i = 0; i < 2; i++)
                {
                    DataSeries dataSeries = new DataSeries();
                    dataSeries.RenderAs = RenderAs.Column;

                    for (Double j = 0; j < 10; j++)
                    {
                        DataPoint dataPoint = new DataPoint();
                        dataPoint.XValue = j + 1;
                        dataPoint.YValue = rand.Next(0, 100);
                        dataSeries.DataPoints.Add(dataPoint);
                    }
                    _chart.Series.Add(dataSeries);
                }
            });

            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                _htmlElement1 = Common.GetDisplayMessageButton(_htmlElement1);
                _htmlElement1.SetStyleAttribute("width", "900px");
                _htmlElement1.SetProperty("value", "Click here to check DataPoint Width at realtime.");
                _htmlElement2 = Common.GetDisplayMessageButton(_htmlElement2);
                _htmlElement2.SetStyleAttribute("top", "540px");
                _htmlElement2.SetProperty("value", " Click here to exit.");
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement1);
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement2);
            });

            EnqueueDelay(_sleepTime);
            EnqueueCallback(() =>
            {
                _htmlElement1.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.HtmlElement1_OnClick));
                _htmlElement2.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.HtmlElement2_OnClick));
            });
        }
        #endregion

        #region TestMultiSeriesDataPointWidthIn3D
        /// <summary>
        /// Test DataPoint width in multiseries 3D Column chart
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void TestMultiSeriesDataPointWidthIn3D()
        {
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "400px");

            _chart = new Chart();
            _chart.Width = 500;
            _chart.Height = 300;
            _chart.View3D = true;

            _isLoaded = false;
            _chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Random rand = new Random();

            TestPanel.Children.Add(_chart);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                for (Int32 i = 0; i < 2; i++)
                {
                    DataSeries dataSeries = new DataSeries();
                    dataSeries.RenderAs = RenderAs.Column;

                    for (Double j = 0; j < 10; j++)
                    {
                        DataPoint dataPoint = new DataPoint();
                        dataPoint.XValue = j + 1;
                        dataPoint.YValue = rand.Next(0, 100);
                        dataSeries.DataPoints.Add(dataPoint);
                    }
                    _chart.Series.Add(dataSeries);
                }
            });

            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                _htmlElement1 = Common.GetDisplayMessageButton(_htmlElement1);
                _htmlElement1.SetStyleAttribute("width", "900px");
                _htmlElement1.SetProperty("value", "Click here to check DataPoint Width at realtime.");
                _htmlElement2 = Common.GetDisplayMessageButton(_htmlElement2);
                _htmlElement2.SetStyleAttribute("top", "540px");
                _htmlElement2.SetProperty("value", " Click here to exit.");
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement1);
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement2);
            });

            EnqueueDelay(_sleepTime);
            EnqueueCallback(() =>
            {
                _htmlElement1.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.HtmlElement1_OnClick));
                _htmlElement2.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.HtmlElement2_OnClick));
            });
        }
        #endregion

        #region TestDataPointWidthWithScroll
        /// <summary>
        /// Test DataPoint width in 3D Column chart with Scroll
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void TestDataPointWidthWithScroll()
        {
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "400px");

            _chart = new Chart();
            _chart.Width = 500;
            _chart.Height = 300;
            _chart.View3D = true;

            _isLoaded = false;
            _chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Random rand = new Random();

            TestPanel.Children.Add(_chart);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                for (Int32 i = 0; i < 1; i++)
                {
                    DataSeries dataSeries = new DataSeries();
                    dataSeries.RenderAs = RenderAs.Column;

                    for (Double j = 0; j < 60; j++)
                    {
                        DataPoint dataPoint = new DataPoint();
                        dataPoint.AxisXLabel = "abc" + j;
                        dataPoint.YValue = rand.Next(0, 100);
                        dataSeries.DataPoints.Add(dataPoint);
                    }
                    _chart.Series.Add(dataSeries);
                }
            });

            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                _htmlElement1 = Common.GetDisplayMessageButton(_htmlElement1);
                _htmlElement1.SetStyleAttribute("width", "900px");
                _htmlElement1.SetProperty("value", "Click here to check DataPoint Width at realtime.");
                _htmlElement2 = Common.GetDisplayMessageButton(_htmlElement2);
                _htmlElement2.SetStyleAttribute("top", "540px");
                _htmlElement2.SetProperty("value", "Switch 2D/3D.");
                _htmlElement3 = Common.GetDisplayMessageButton(_htmlElement3);
                _htmlElement3.SetStyleAttribute("top", "560px");
                _htmlElement3.SetProperty("value", " Click here to exit.");
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement1);
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement2);
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement3);
            });

            EnqueueDelay(_sleepTime);
            EnqueueCallback(() =>
            {
                _htmlElement1.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.HtmlElement1_OnClick));
                _htmlElement2.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.View3D_OnClick));
                _htmlElement3.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.HtmlElement2_OnClick));
            });
        }
        #endregion

        #region TestNegativeDataPointWidth
        [TestMethod]
        [Asynchronous]
        public void TestNegativeDataPointWidth()
        {
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "400px");

            _chart = new Chart();
            _chart.Width = 400;
            _chart.Height = 300;

            Random rand = new Random();

            _isLoaded = false;
            _chart.Loaded += new RoutedEventHandler(chart_Loaded);

            TestPanel.Children.Add(_chart);

            DataSeries dataSeries = new DataSeries();
            dataSeries.RenderAs = RenderAs.Column;
            dataSeries.DataPoints.Add(new DataPoint() { YValue = 200 });
            dataSeries.DataPoints.Add(new DataPoint() { YValue = 100 });
            dataSeries.DataPoints.Add(new DataPoint() { YValue = 300 });
            _chart.Series.Add(dataSeries);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                _chart.DataPointWidth = -10;
            });
            EnqueueCallback(() =>
            {
                EnqueueTestComplete();
            });
        }
        #endregion

        #region TestSingleDataPointWidthInBar
        /// <summary>
        /// Test single DataPoint width in Bar
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void TestSingleDataPointWidthInBar()
        {
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "400px");

            _chart = new Chart();
            _chart.Width = 300;
            _chart.Height = 400;

            Random rand = new Random();

            _isLoaded = false;
            _chart.Loaded += new RoutedEventHandler(chart_Loaded);

            TestPanel.Children.Add(_chart);

            DataSeries dataSeries = new DataSeries();
            dataSeries.RenderAs = RenderAs.Bar;
            dataSeries.DataPoints.Add(new DataPoint() { YValue = 200 });
            _chart.Series.Add(dataSeries);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                _chart.DataPointWidth = 10;
            });

            EnqueueCallback(() =>
            {
                _htmlElement1 = Common.GetDisplayMessageButton(_htmlElement1);
                _htmlElement1.SetStyleAttribute("width", "900px");
                _htmlElement1.SetProperty("value", "Click here to check DataPoint Width at realtime.");
                _htmlElement2 = Common.GetDisplayMessageButton(_htmlElement2);
                _htmlElement2.SetStyleAttribute("top", "540px");
                _htmlElement2.SetProperty("value", " Click here to exit.");
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement1);
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement2);
            });

            EnqueueDelay(_sleepTime);
            EnqueueCallback(() =>
            {
                _htmlElement1.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.HtmlElement1_OnClick));
                _htmlElement2.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.HtmlElement2_OnClick));
            });
        }
        #endregion

        #region TestSingleDataPointWidthInBar3D
        /// <summary>
        /// Test single DataPoint width in 3D Bar
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void TestSingleDataPointWidthInBar3D()
        {
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "400px");

            _chart = new Chart();
            _chart.Width = 300;
            _chart.Height = 400;
            _chart.View3D = true;

            Random rand = new Random();

            _isLoaded = false;
            _chart.Loaded += new RoutedEventHandler(chart_Loaded);

            TestPanel.Children.Add(_chart);

            DataSeries dataSeries = new DataSeries();
            dataSeries.RenderAs = RenderAs.Bar;
            dataSeries.DataPoints.Add(new DataPoint() { YValue = 200 });
            _chart.Series.Add(dataSeries);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                _chart.DataPointWidth = 10;
            });

            EnqueueCallback(() =>
            {
                _htmlElement1 = Common.GetDisplayMessageButton(_htmlElement1);
                _htmlElement1.SetStyleAttribute("width", "900px");
                _htmlElement1.SetProperty("value", "Click here to check DataPoint Width at realtime.");
                _htmlElement2 = Common.GetDisplayMessageButton(_htmlElement2);
                _htmlElement2.SetStyleAttribute("top", "540px");
                _htmlElement2.SetProperty("value", " Click here to exit.");
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement1);
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement2);
            });

            EnqueueDelay(_sleepTime);
            EnqueueCallback(() =>
            {
                _htmlElement1.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.HtmlElement1_OnClick));
                _htmlElement2.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.HtmlElement2_OnClick));
            });
        }
        #endregion

        #region TestDataPointWidth4DecimalXValueInBar
        /// <summary>
        /// Test DataPoint width with Decimal XValues in Bar
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void TestDataPointWidth4DecimalXValueInBar()
        {
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "400px");

            _chart = new Chart();
            _chart.Width = 300;
            _chart.Height = 400;

            _isLoaded = false;
            _chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Random rand = new Random();

            TestPanel.Children.Add(_chart);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                DataSeries dataSeries1 = new DataSeries();
                dataSeries1.RenderAs = RenderAs.Bar;

                for (Double i = 0; i < 10; i++)
                {
                    DataPoint dataPoint = new DataPoint();
                    dataPoint.XValue = rand.NextDouble() + i;
                    dataPoint.YValue = rand.Next(0, 100);
                    dataSeries1.DataPoints.Add(dataPoint);
                }
                _chart.Series.Add(dataSeries1);
            });

            EnqueueCallback(() =>
            {
                _htmlElement1 = Common.GetDisplayMessageButton(_htmlElement1);
                _htmlElement1.SetStyleAttribute("width", "900px");
                _htmlElement1.SetProperty("value", "Click here to check DataPoint Width at realtime.");
                _htmlElement2 = Common.GetDisplayMessageButton(_htmlElement2);
                _htmlElement2.SetStyleAttribute("top", "540px");
                _htmlElement2.SetProperty("value", " Click here to exit.");
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement1);
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement2);
            });

            EnqueueDelay(_sleepTime);
            EnqueueCallback(() =>
            {
                _htmlElement1.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.HtmlElement1_OnClick));
                _htmlElement2.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.HtmlElement2_OnClick));
            });
        }
        #endregion

        #region TestMultiSeriesDataPointWidth4DecimalXValueInBar
        /// <summary>
        /// Test multiseries DataPoint width with Decimal XValues in Bar
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void TestMultiSeriesDataPointWidth4DecimalXValueInBar()
        {
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "400px");

            _chart = new Chart();
            _chart.Width = 300;
            _chart.Height = 400;

            _isLoaded = false;
            _chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Random rand = new Random();

            List<Double> xList = new List<Double>();
            List<Double> yList = new List<Double>();
            Double y = 0;

            TestPanel.Children.Add(_chart);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                DataSeries dataSeries1 = new DataSeries();
                dataSeries1.RenderAs = RenderAs.Bar;

                for (Double i = 0; i < 10; i++)
                {
                    DataPoint dataPoint = new DataPoint();
                    dataPoint.XValue = i + 1;
                    xList.Add(i);
                    dataPoint.YValue = (y = rand.Next(-100, 100));
                    yList.Add(y);
                    dataSeries1.DataPoints.Add(dataPoint);
                }
                _chart.Series.Add(dataSeries1);
            });

            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {

                DataSeries dataSeries2 = new DataSeries();
                dataSeries2.RenderAs = RenderAs.Bar;

                Double j = 0.5;
                for (Int32 i = 0; i < 10; i++)
                {
                    DataPoint dataPoint = new DataPoint();
                    dataPoint.XValue = j;
                    dataPoint.YValue = yList[i];
                    dataSeries2.DataPoints.Add(dataPoint);
                    j++;
                }
                _chart.Series.Add(dataSeries2);
            });

            EnqueueCallback(() =>
            {
                _htmlElement1 = Common.GetDisplayMessageButton(_htmlElement1);
                _htmlElement1.SetStyleAttribute("width", "900px");
                _htmlElement1.SetProperty("value", "Click here to check DataPoint Width at realtime.");
                _htmlElement2 = Common.GetDisplayMessageButton(_htmlElement2);
                _htmlElement2.SetStyleAttribute("top", "540px");
                _htmlElement2.SetProperty("value", " Click here to exit.");
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement1);
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement2);
            });

            EnqueueDelay(_sleepTime);
            EnqueueCallback(() =>
            {
                _htmlElement1.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.HtmlElement1_OnClick));
                _htmlElement2.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.HtmlElement2_OnClick));
            });
        }
        #endregion

        #region TestMultiSeriesDataPointWidthInBar
        /// <summary>
        /// Test DataPoint width in multiseries Bar chart
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void TestMultiSeriesDataPointWidthInBar()
        {
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "400px");

            _chart = new Chart();
            _chart.Width = 300;
            _chart.Height = 400;

            _isLoaded = false;
            _chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Random rand = new Random();

            TestPanel.Children.Add(_chart);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                for (Int32 i = 0; i < 2; i++)
                {
                    DataSeries dataSeries = new DataSeries();
                    dataSeries.RenderAs = RenderAs.Bar;

                    for (Double j = 0; j < 10; j++)
                    {
                        DataPoint dataPoint = new DataPoint();
                        dataPoint.XValue = j + 1;
                        dataPoint.YValue = rand.Next(0, 100);
                        dataSeries.DataPoints.Add(dataPoint);
                    }
                    _chart.Series.Add(dataSeries);
                }
            });

            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                _htmlElement1 = Common.GetDisplayMessageButton(_htmlElement1);
                _htmlElement1.SetStyleAttribute("width", "900px");
                _htmlElement1.SetProperty("value", "Click here to check DataPoint Width at realtime.");
                _htmlElement2 = Common.GetDisplayMessageButton(_htmlElement2);
                _htmlElement2.SetStyleAttribute("top", "540px");
                _htmlElement2.SetProperty("value", " Click here to exit.");
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement1);
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement2);
            });

            EnqueueDelay(_sleepTime);
            EnqueueCallback(() =>
            {
                _htmlElement1.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.HtmlElement1_OnClick));
                _htmlElement2.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.HtmlElement2_OnClick));
            });
        }
        #endregion

        #region TestMultiSeriesDataPointWidthInBar3D
        /// <summary>
        /// Test DataPoint width in multiseries 3D Bar chart
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void TestMultiSeriesDataPointWidthInBar3D()
        {
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "400px");

            _chart = new Chart();
            _chart.Width = 300;
            _chart.Height = 400;
            _chart.View3D = true;

            _isLoaded = false;
            _chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Random rand = new Random();

            TestPanel.Children.Add(_chart);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                for (Int32 i = 0; i < 2; i++)
                {
                    DataSeries dataSeries = new DataSeries();
                    dataSeries.RenderAs = RenderAs.Bar;

                    for (Double j = 0; j < 10; j++)
                    {
                        DataPoint dataPoint = new DataPoint();
                        dataPoint.XValue = j + 1;
                        dataPoint.YValue = rand.Next(0, 100);
                        dataSeries.DataPoints.Add(dataPoint);
                    }
                    _chart.Series.Add(dataSeries);
                }
            });

            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                _htmlElement1 = Common.GetDisplayMessageButton(_htmlElement1);
                _htmlElement1.SetStyleAttribute("width", "900px");
                _htmlElement1.SetProperty("value", "Click here to check DataPoint Width at realtime.");
                _htmlElement2 = Common.GetDisplayMessageButton(_htmlElement2);
                _htmlElement2.SetStyleAttribute("top", "540px");
                _htmlElement2.SetProperty("value", " Click here to exit.");
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement1);
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement2);
            });

            EnqueueDelay(_sleepTime);
            EnqueueCallback(() =>
            {
                _htmlElement1.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.HtmlElement1_OnClick));
                _htmlElement2.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.HtmlElement2_OnClick));
            });
        }
        #endregion

        #region TestDataPointWidthWithScrollInBar
        /// <summary>
        /// Test DataPoint width in 3D Column chart with Scroll in Bar chart
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void TestDataPointWidthWithScrollInBar()
        {
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "400px");

            _chart = new Chart();
            _chart.Width = 300;
            _chart.Height = 400;
            _chart.View3D = true;

            _isLoaded = false;
            _chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Random rand = new Random();

            TestPanel.Children.Add(_chart);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                for (Int32 i = 0; i < 1; i++)
                {
                    DataSeries dataSeries = new DataSeries();
                    dataSeries.RenderAs = RenderAs.Bar;

                    for (Double j = 0; j < 60; j++)
                    {
                        DataPoint dataPoint = new DataPoint();
                        dataPoint.AxisXLabel = "abc" + j;
                        dataPoint.YValue = rand.Next(0, 100);
                        dataSeries.DataPoints.Add(dataPoint);
                    }
                    _chart.Series.Add(dataSeries);
                }
            });

            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                _htmlElement1 = Common.GetDisplayMessageButton(_htmlElement1);
                _htmlElement1.SetStyleAttribute("width", "900px");
                _htmlElement1.SetProperty("value", "Click here to check DataPoint Width at realtime.");
                _htmlElement2 = Common.GetDisplayMessageButton(_htmlElement2);
                _htmlElement2.SetStyleAttribute("top", "540px");
                _htmlElement2.SetProperty("value", "Switch 2D/3D.");
                _htmlElement3 = Common.GetDisplayMessageButton(_htmlElement3);
                _htmlElement3.SetStyleAttribute("top", "560px");
                _htmlElement3.SetProperty("value", " Click here to exit.");
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement1);
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement2);
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement3);
            });

            EnqueueDelay(_sleepTime);
            EnqueueCallback(() =>
            {
                _htmlElement1.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.HtmlElement1_OnClick));
                _htmlElement2.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.View3D_OnClick));
                _htmlElement3.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.HtmlElement2_OnClick));
            });
        }
        #endregion

        #region TestNegativeDataPointWidthInBar
        [TestMethod]
        [Asynchronous]
        public void TestNegativeDataPointWidthInBar()
        {
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "400px");

            _chart = new Chart();
            _chart.Width = 300;
            _chart.Height = 400;

            Random rand = new Random();

            _isLoaded = false;
            _chart.Loaded += new RoutedEventHandler(chart_Loaded);

            TestPanel.Children.Add(_chart);

            DataSeries dataSeries = new DataSeries();
            dataSeries.RenderAs = RenderAs.Bar;
            dataSeries.DataPoints.Add(new DataPoint() { YValue = 200 });
            dataSeries.DataPoints.Add(new DataPoint() { YValue = 100 });
            dataSeries.DataPoints.Add(new DataPoint() { YValue = 300 });
            _chart.Series.Add(dataSeries);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                _chart.DataPointWidth = -10;
            });
            EnqueueCallback(() =>
            {
                EnqueueTestComplete();
            });
        }
        #endregion

        #region TestSingleDataPointWidthInStackedColumn
        /// <summary>
        /// Test single DataPoint width in StackedColumn
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void TestSingleDataPointWidthInStackedColumn()
        {
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "400px");

            _chart = new Chart();
            _chart.Width = 400;
            _chart.Height = 300;

            Random rand = new Random();

            _isLoaded = false;
            _chart.Loaded += new RoutedEventHandler(chart_Loaded);

            TestPanel.Children.Add(_chart);

            DataSeries dataSeries = new DataSeries();
            dataSeries.RenderAs = RenderAs.StackedColumn;
            dataSeries.DataPoints.Add(new DataPoint() { YValue = 200 });
            _chart.Series.Add(dataSeries);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                _chart.DataPointWidth = 10;
            });

            EnqueueCallback(() =>
            {
                _htmlElement1 = Common.GetDisplayMessageButton(_htmlElement1);
                _htmlElement1.SetStyleAttribute("width", "900px");
                _htmlElement1.SetProperty("value", "Click here to check DataPoint Width at realtime.");
                _htmlElement2 = Common.GetDisplayMessageButton(_htmlElement2);
                _htmlElement2.SetStyleAttribute("top", "540px");
                _htmlElement2.SetProperty("value", " Click here to exit.");
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement1);
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement2);
            });

            EnqueueDelay(_sleepTime);
            EnqueueCallback(() =>
            {
                _htmlElement1.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.HtmlElement1_OnClick));
                _htmlElement2.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.HtmlElement2_OnClick));
            });
        }
        #endregion

        #region TestSingleDataPointWidthInStackedColumn3D
        /// <summary>
        /// Test single DataPoint width in 3D StackedColumn
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void TestSingleDataPointWidthInStackedColumn3D()
        {
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "400px");

            _chart = new Chart();
            _chart.Width = 400;
            _chart.Height = 300;
            _chart.View3D = true;

            Random rand = new Random();

            _isLoaded = false;
            _chart.Loaded += new RoutedEventHandler(chart_Loaded);

            TestPanel.Children.Add(_chart);

            DataSeries dataSeries = new DataSeries();
            dataSeries.RenderAs = RenderAs.StackedColumn;
            dataSeries.DataPoints.Add(new DataPoint() { YValue = 200 });
            _chart.Series.Add(dataSeries);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                _chart.DataPointWidth = 10;
            });

            EnqueueCallback(() =>
            {
                _htmlElement1 = Common.GetDisplayMessageButton(_htmlElement1);
                _htmlElement1.SetStyleAttribute("width", "900px");
                _htmlElement1.SetProperty("value", "Click here to check DataPoint Width at realtime.");
                _htmlElement2 = Common.GetDisplayMessageButton(_htmlElement2);
                _htmlElement2.SetStyleAttribute("top", "540px");
                _htmlElement2.SetProperty("value", " Click here to exit.");
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement1);
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement2);
            });

            EnqueueDelay(_sleepTime);
            EnqueueCallback(() =>
            {
                _htmlElement1.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.HtmlElement1_OnClick));
                _htmlElement2.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.HtmlElement2_OnClick));
            });
        }
        #endregion

        #region TestDataPointWidth4DecimalXValueInStackedColumn
        /// <summary>
        /// Test DataPoint width with Decimal XValues in StackedColumn
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void TestDataPointWidth4DecimalXValueInStackedColumn()
        {
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "400px");

            _chart = new Chart();
            _chart.Width = 500;
            _chart.Height = 300;

            _isLoaded = false;
            _chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Random rand = new Random();

            TestPanel.Children.Add(_chart);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                DataSeries dataSeries1 = new DataSeries();
                dataSeries1.RenderAs = RenderAs.StackedColumn;

                for (Double i = 0; i < 10; i++)
                {
                    DataPoint dataPoint = new DataPoint();
                    dataPoint.XValue = rand.NextDouble() + i;
                    dataPoint.YValue = rand.Next(0, 100);
                    dataSeries1.DataPoints.Add(dataPoint);
                }
                _chart.Series.Add(dataSeries1);
            });

            EnqueueCallback(() =>
            {
                _htmlElement1 = Common.GetDisplayMessageButton(_htmlElement1);
                _htmlElement1.SetStyleAttribute("width", "900px");
                _htmlElement1.SetProperty("value", "Click here to check DataPoint Width at realtime.");
                _htmlElement2 = Common.GetDisplayMessageButton(_htmlElement2);
                _htmlElement2.SetStyleAttribute("top", "540px");
                _htmlElement2.SetProperty("value", " Click here to exit.");
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement1);
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement2);
            });

            EnqueueDelay(_sleepTime);
            EnqueueCallback(() =>
            {
                _htmlElement1.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.HtmlElement1_OnClick));
                _htmlElement2.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.HtmlElement2_OnClick));
            });
        }
        #endregion

        #region TestMultiSeriesDataPointWidth4DecimalXValueInStackedColumn
        /// <summary>
        /// Test multiseries DataPoint width with Decimal XValues in StackedColumn
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void TestMultiSeriesDataPointWidth4DecimalXValueInStackedColumn()
        {
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "400px");

            _chart = new Chart();
            _chart.Width = 500;
            _chart.Height = 300;

            _isLoaded = false;
            _chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Random rand = new Random();

            List<Double> xList = new List<Double>();
            List<Double> yList = new List<Double>();
            Double y = 0;

            TestPanel.Children.Add(_chart);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                DataSeries dataSeries1 = new DataSeries();
                dataSeries1.RenderAs = RenderAs.StackedColumn;

                for (Double i = 0; i < 10; i++)
                {
                    DataPoint dataPoint = new DataPoint();
                    dataPoint.XValue = i + 1;
                    xList.Add(i);
                    dataPoint.YValue = (y = rand.Next(-100, 100));
                    yList.Add(y);
                    dataSeries1.DataPoints.Add(dataPoint);
                }
                _chart.Series.Add(dataSeries1);
            });

            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {

                DataSeries dataSeries2 = new DataSeries();
                dataSeries2.RenderAs = RenderAs.StackedColumn;

                Double j = 0.5;
                for (Int32 i = 0; i < 10; i++)
                {
                    DataPoint dataPoint = new DataPoint();
                    dataPoint.XValue = j;
                    dataPoint.YValue = yList[i];
                    dataSeries2.DataPoints.Add(dataPoint);
                    j++;
                }
                _chart.Series.Add(dataSeries2);
            });

            EnqueueCallback(() =>
            {
                _htmlElement1 = Common.GetDisplayMessageButton(_htmlElement1);
                _htmlElement1.SetStyleAttribute("width", "900px");
                _htmlElement1.SetProperty("value", "Click here to check DataPoint Width at realtime.");
                _htmlElement2 = Common.GetDisplayMessageButton(_htmlElement2);
                _htmlElement2.SetStyleAttribute("top", "540px");
                _htmlElement2.SetProperty("value", " Click here to exit.");
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement1);
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement2);
            });

            EnqueueDelay(_sleepTime);
            EnqueueCallback(() =>
            {
                _htmlElement1.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.HtmlElement1_OnClick));
                _htmlElement2.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.HtmlElement2_OnClick));
            });
        }
        #endregion

        #region TestMultiSeriesDataPointWidthInStackedColumn
        /// <summary>
        /// Test DataPoint width in multiseries StackedColumn chart
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void TestMultiSeriesDataPointWidthInStackedColumn()
        {
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "400px");

            _chart = new Chart();
            _chart.Width = 500;
            _chart.Height = 300;

            _isLoaded = false;
            _chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Random rand = new Random();

            TestPanel.Children.Add(_chart);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                for (Int32 i = 0; i < 2; i++)
                {
                    DataSeries dataSeries = new DataSeries();
                    dataSeries.RenderAs = RenderAs.StackedColumn;

                    for (Double j = 0; j < 10; j++)
                    {
                        DataPoint dataPoint = new DataPoint();
                        dataPoint.XValue = j + 1;
                        dataPoint.YValue = rand.Next(0, 100);
                        dataSeries.DataPoints.Add(dataPoint);
                    }
                    _chart.Series.Add(dataSeries);
                }
            });

            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                _htmlElement1 = Common.GetDisplayMessageButton(_htmlElement1);
                _htmlElement1.SetStyleAttribute("width", "900px");
                _htmlElement1.SetProperty("value", "Click here to check DataPoint Width at realtime.");
                _htmlElement2 = Common.GetDisplayMessageButton(_htmlElement2);
                _htmlElement2.SetStyleAttribute("top", "540px");
                _htmlElement2.SetProperty("value", " Click here to exit.");
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement1);
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement2);
            });

            EnqueueDelay(_sleepTime);
            EnqueueCallback(() =>
            {
                _htmlElement1.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.HtmlElement1_OnClick));
                _htmlElement2.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.HtmlElement2_OnClick));
            });
        }
        #endregion

        #region TestMultiSeriesDataPointWidthInStackedColumn3D
        /// <summary>
        /// Test DataPoint width in multiseries 3D StackedColumn chart
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void TestMultiSeriesDataPointWidthInStackedColumn3D()
        {
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "400px");

            _chart = new Chart();
            _chart.Width = 500;
            _chart.Height = 300;
            _chart.View3D = true;

            _isLoaded = false;
            _chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Random rand = new Random();

            TestPanel.Children.Add(_chart);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                for (Int32 i = 0; i < 2; i++)
                {
                    DataSeries dataSeries = new DataSeries();
                    dataSeries.RenderAs = RenderAs.StackedColumn;

                    for (Double j = 0; j < 10; j++)
                    {
                        DataPoint dataPoint = new DataPoint();
                        dataPoint.XValue = j + 1;
                        dataPoint.YValue = rand.Next(0, 100);
                        dataSeries.DataPoints.Add(dataPoint);
                    }
                    _chart.Series.Add(dataSeries);
                }
            });

            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                _htmlElement1 = Common.GetDisplayMessageButton(_htmlElement1);
                _htmlElement1.SetStyleAttribute("width", "900px");
                _htmlElement1.SetProperty("value", "Click here to check DataPoint Width at realtime.");
                _htmlElement2 = Common.GetDisplayMessageButton(_htmlElement2);
                _htmlElement2.SetStyleAttribute("top", "540px");
                _htmlElement2.SetProperty("value", " Click here to exit.");
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement1);
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement2);
            });

            EnqueueDelay(_sleepTime);
            EnqueueCallback(() =>
            {
                _htmlElement1.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.HtmlElement1_OnClick));
                _htmlElement2.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.HtmlElement2_OnClick));
            });
        }
        #endregion

        #region TestDataPointWidthWithScrollInStackedColumn
        /// <summary>
        /// Test DataPoint width in 3D Column chart with Scroll in StackedColumn chart
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void TestDataPointWidthWithScrollInStackedColumn()
        {
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "400px");

            _chart = new Chart();
            _chart.Width = 500;
            _chart.Height = 300;
            _chart.View3D = true;

            _isLoaded = false;
            _chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Random rand = new Random();

            TestPanel.Children.Add(_chart);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                for (Int32 i = 0; i < 1; i++)
                {
                    DataSeries dataSeries = new DataSeries();
                    dataSeries.RenderAs = RenderAs.StackedColumn;

                    for (Double j = 0; j < 60; j++)
                    {
                        DataPoint dataPoint = new DataPoint();
                        dataPoint.AxisXLabel = "abc" + j;
                        dataPoint.YValue = rand.Next(0, 100);
                        dataSeries.DataPoints.Add(dataPoint);
                    }
                    _chart.Series.Add(dataSeries);
                }
            });

            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                _htmlElement1 = Common.GetDisplayMessageButton(_htmlElement1);
                _htmlElement1.SetStyleAttribute("width", "900px");
                _htmlElement1.SetProperty("value", "Click here to check DataPoint Width at realtime.");
                _htmlElement2 = Common.GetDisplayMessageButton(_htmlElement2);
                _htmlElement2.SetStyleAttribute("top", "540px");
                _htmlElement2.SetProperty("value", "Switch 2D/3D.");
                _htmlElement3 = Common.GetDisplayMessageButton(_htmlElement3);
                _htmlElement3.SetStyleAttribute("top", "560px");
                _htmlElement3.SetProperty("value", " Click here to exit.");
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement1);
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement2);
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement3);
            });

            EnqueueDelay(_sleepTime);
            EnqueueCallback(() =>
            {
                _htmlElement1.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.HtmlElement1_OnClick));
                _htmlElement2.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.View3D_OnClick));
                _htmlElement3.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.HtmlElement2_OnClick));
            });
        }
        #endregion

        #region TestNegativeDataPointWidthInStackedColumn
        [TestMethod]
        [Asynchronous]
        public void TestNegativeDataPointWidthInStackedColumn()
        {
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "400px");

            _chart = new Chart();
            _chart.Width = 400;
            _chart.Height = 300;

            Random rand = new Random();

            _isLoaded = false;
            _chart.Loaded += new RoutedEventHandler(chart_Loaded);

            TestPanel.Children.Add(_chart);

            DataSeries dataSeries = new DataSeries();
            dataSeries.RenderAs = RenderAs.StackedColumn;
            dataSeries.DataPoints.Add(new DataPoint() { YValue = 200 });
            dataSeries.DataPoints.Add(new DataPoint() { YValue = 100 });
            dataSeries.DataPoints.Add(new DataPoint() { YValue = 300 });
            _chart.Series.Add(dataSeries);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                _chart.DataPointWidth = -10;
            });
            EnqueueCallback(() =>
            {
                EnqueueTestComplete();
            });
        }
        #endregion

        #region TestSingleDataPointWidthInStackedColumn100
        /// <summary>
        /// Test single DataPoint width in StackedColumn100
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void TestSingleDataPointWidthInStackedColumn100()
        {
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "400px");

            _chart = new Chart();
            _chart.Width = 400;
            _chart.Height = 300;

            Random rand = new Random();

            _isLoaded = false;
            _chart.Loaded += new RoutedEventHandler(chart_Loaded);

            TestPanel.Children.Add(_chart);

            DataSeries dataSeries = new DataSeries();
            dataSeries.RenderAs = RenderAs.StackedColumn100;
            dataSeries.DataPoints.Add(new DataPoint() { YValue = 200 });
            _chart.Series.Add(dataSeries);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                _chart.DataPointWidth = 10;
            });

            EnqueueCallback(() =>
            {
                _htmlElement1 = Common.GetDisplayMessageButton(_htmlElement1);
                _htmlElement1.SetStyleAttribute("width", "900px");
                _htmlElement1.SetProperty("value", "Click here to check DataPoint Width at realtime.");
                _htmlElement2 = Common.GetDisplayMessageButton(_htmlElement2);
                _htmlElement2.SetStyleAttribute("top", "540px");
                _htmlElement2.SetProperty("value", " Click here to exit.");
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement1);
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement2);
            });

            EnqueueDelay(_sleepTime);
            EnqueueCallback(() =>
            {
                _htmlElement1.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.HtmlElement1_OnClick));
                _htmlElement2.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.HtmlElement2_OnClick));
            });
        }
        #endregion

        #region TestSingleDataPointWidthInStackedColumn1003D
        /// <summary>
        /// Test single DataPoint width in 3D StackedColumn100
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void TestSingleDataPointWidthInStackedColumn1003D()
        {
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "400px");

            _chart = new Chart();
            _chart.Width = 400;
            _chart.Height = 300;
            _chart.View3D = true;

            Random rand = new Random();

            _isLoaded = false;
            _chart.Loaded += new RoutedEventHandler(chart_Loaded);

            TestPanel.Children.Add(_chart);

            DataSeries dataSeries = new DataSeries();
            dataSeries.RenderAs = RenderAs.StackedColumn100;
            dataSeries.DataPoints.Add(new DataPoint() { YValue = 200 });
            _chart.Series.Add(dataSeries);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                _chart.DataPointWidth = 10;
            });

            EnqueueCallback(() =>
            {
                _htmlElement1 = Common.GetDisplayMessageButton(_htmlElement1);
                _htmlElement1.SetStyleAttribute("width", "900px");
                _htmlElement1.SetProperty("value", "Click here to check DataPoint Width at realtime.");
                _htmlElement2 = Common.GetDisplayMessageButton(_htmlElement2);
                _htmlElement2.SetStyleAttribute("top", "540px");
                _htmlElement2.SetProperty("value", " Click here to exit.");
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement1);
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement2);
            });

            EnqueueDelay(_sleepTime);
            EnqueueCallback(() =>
            {
                _htmlElement1.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.HtmlElement1_OnClick));
                _htmlElement2.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.HtmlElement2_OnClick));
            });
        }
        #endregion

        #region TestDataPointWidth4DecimalXValueInStackedColumn100
        /// <summary>
        /// Test DataPoint width with Decimal XValues in StackedColumn100
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void TestDataPointWidth4DecimalXValueInStackedColumn100()
        {
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "400px");

            _chart = new Chart();
            _chart.Width = 500;
            _chart.Height = 300;

            _isLoaded = false;
            _chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Random rand = new Random();

            TestPanel.Children.Add(_chart);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                DataSeries dataSeries1 = new DataSeries();
                dataSeries1.RenderAs = RenderAs.StackedColumn100;

                for (Double i = 0; i < 10; i++)
                {
                    DataPoint dataPoint = new DataPoint();
                    dataPoint.XValue = rand.NextDouble() + i;
                    dataPoint.YValue = rand.Next(0, 100);
                    dataSeries1.DataPoints.Add(dataPoint);
                }
                _chart.Series.Add(dataSeries1);
            });

            EnqueueCallback(() =>
            {
                _htmlElement1 = Common.GetDisplayMessageButton(_htmlElement1);
                _htmlElement1.SetStyleAttribute("width", "900px");
                _htmlElement1.SetProperty("value", "Click here to check DataPoint Width at realtime.");
                _htmlElement2 = Common.GetDisplayMessageButton(_htmlElement2);
                _htmlElement2.SetStyleAttribute("top", "540px");
                _htmlElement2.SetProperty("value", " Click here to exit.");
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement1);
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement2);
            });

            EnqueueDelay(_sleepTime);
            EnqueueCallback(() =>
            {
                _htmlElement1.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.HtmlElement1_OnClick));
                _htmlElement2.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.HtmlElement2_OnClick));
            });
        }
        #endregion

        #region TestMultiSeriesDataPointWidth4DecimalXValueInStackedColumn100
        /// <summary>
        /// Test multiseries DataPoint width with Decimal XValues in StackedColumn100
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void TestMultiSeriesDataPointWidth4DecimalXValueInStackedColumn100()
        {
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "400px");

            _chart = new Chart();
            _chart.Width = 500;
            _chart.Height = 300;

            _isLoaded = false;
            _chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Random rand = new Random();

            List<Double> xList = new List<Double>();
            List<Double> yList = new List<Double>();
            Double y = 0;

            TestPanel.Children.Add(_chart);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                DataSeries dataSeries1 = new DataSeries();
                dataSeries1.RenderAs = RenderAs.StackedColumn100;

                for (Double i = 0; i < 10; i++)
                {
                    DataPoint dataPoint = new DataPoint();
                    dataPoint.XValue = i + 1;
                    xList.Add(i);
                    dataPoint.YValue = (y = rand.Next(-100, 100));
                    yList.Add(y);
                    dataSeries1.DataPoints.Add(dataPoint);
                }
                _chart.Series.Add(dataSeries1);
            });

            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {

                DataSeries dataSeries2 = new DataSeries();
                dataSeries2.RenderAs = RenderAs.StackedColumn100;

                Double j = 0.5;
                for (Int32 i = 0; i < 10; i++)
                {
                    DataPoint dataPoint = new DataPoint();
                    dataPoint.XValue = j;
                    dataPoint.YValue = yList[i];
                    dataSeries2.DataPoints.Add(dataPoint);
                    j++;
                }
                _chart.Series.Add(dataSeries2);
            });

            EnqueueCallback(() =>
            {
                _htmlElement1 = Common.GetDisplayMessageButton(_htmlElement1);
                _htmlElement1.SetStyleAttribute("width", "900px");
                _htmlElement1.SetProperty("value", "Click here to check DataPoint Width at realtime.");
                _htmlElement2 = Common.GetDisplayMessageButton(_htmlElement2);
                _htmlElement2.SetStyleAttribute("top", "540px");
                _htmlElement2.SetProperty("value", " Click here to exit.");
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement1);
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement2);
            });

            EnqueueDelay(_sleepTime);
            EnqueueCallback(() =>
            {
                _htmlElement1.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.HtmlElement1_OnClick));
                _htmlElement2.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.HtmlElement2_OnClick));
            });
        }
        #endregion

        #region TestMultiSeriesDataPointWidthInStackedColumn100
        /// <summary>
        /// Test DataPoint width in multiseries StackedColumn100 chart
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void TestMultiSeriesDataPointWidthInStackedColumn100()
        {
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "400px");

            _chart = new Chart();
            _chart.Width = 500;
            _chart.Height = 300;

            _isLoaded = false;
            _chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Random rand = new Random();

            TestPanel.Children.Add(_chart);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                for (Int32 i = 0; i < 2; i++)
                {
                    DataSeries dataSeries = new DataSeries();
                    dataSeries.RenderAs = RenderAs.StackedColumn100;

                    for (Double j = 0; j < 10; j++)
                    {
                        DataPoint dataPoint = new DataPoint();
                        dataPoint.XValue = j + 1;
                        dataPoint.YValue = rand.Next(0, 100);
                        dataSeries.DataPoints.Add(dataPoint);
                    }
                    _chart.Series.Add(dataSeries);
                }
            });

            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                _htmlElement1 = Common.GetDisplayMessageButton(_htmlElement1);
                _htmlElement1.SetStyleAttribute("width", "900px");
                _htmlElement1.SetProperty("value", "Click here to check DataPoint Width at realtime.");
                _htmlElement2 = Common.GetDisplayMessageButton(_htmlElement2);
                _htmlElement2.SetStyleAttribute("top", "540px");
                _htmlElement2.SetProperty("value", " Click here to exit.");
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement1);
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement2);
            });

            EnqueueDelay(_sleepTime);
            EnqueueCallback(() =>
            {
                _htmlElement1.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.HtmlElement1_OnClick));
                _htmlElement2.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.HtmlElement2_OnClick));
            });
        }
        #endregion

        #region TestMultiSeriesDataPointWidthInStackedColumn1003D
        /// <summary>
        /// Test DataPoint width in multiseries 3D StackedColumn100 chart
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void TestMultiSeriesDataPointWidthInStackedColumn1003D()
        {
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "400px");

            _chart = new Chart();
            _chart.Width = 500;
            _chart.Height = 300;
            _chart.View3D = true;

            _isLoaded = false;
            _chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Random rand = new Random();

            TestPanel.Children.Add(_chart);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                for (Int32 i = 0; i < 2; i++)
                {
                    DataSeries dataSeries = new DataSeries();
                    dataSeries.RenderAs = RenderAs.StackedColumn100;

                    for (Double j = 0; j < 10; j++)
                    {
                        DataPoint dataPoint = new DataPoint();
                        dataPoint.XValue = j + 1;
                        dataPoint.YValue = rand.Next(0, 100);
                        dataSeries.DataPoints.Add(dataPoint);
                    }
                    _chart.Series.Add(dataSeries);
                }
            });

            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                _htmlElement1 = Common.GetDisplayMessageButton(_htmlElement1);
                _htmlElement1.SetStyleAttribute("width", "900px");
                _htmlElement1.SetProperty("value", "Click here to check DataPoint Width at realtime.");
                _htmlElement2 = Common.GetDisplayMessageButton(_htmlElement2);
                _htmlElement2.SetStyleAttribute("top", "540px");
                _htmlElement2.SetProperty("value", " Click here to exit.");
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement1);
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement2);
            });

            EnqueueDelay(_sleepTime);
            EnqueueCallback(() =>
            {
                _htmlElement1.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.HtmlElement1_OnClick));
                _htmlElement2.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.HtmlElement2_OnClick));
            });
        }
        #endregion

        #region TestDataPointWidthWithScrollInStackedColumn100
        /// <summary>
        /// Test DataPoint width in 3D Column chart with Scroll in StackedColumn100 chart
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void TestDataPointWidthWithScrollInStackedColumn100()
        {
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "400px");

            _chart = new Chart();
            _chart.Width = 500;
            _chart.Height = 300;
            _chart.View3D = true;

            _isLoaded = false;
            _chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Random rand = new Random();

            TestPanel.Children.Add(_chart);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                for (Int32 i = 0; i < 1; i++)
                {
                    DataSeries dataSeries = new DataSeries();
                    dataSeries.RenderAs = RenderAs.StackedColumn100;

                    for (Double j = 0; j < 60; j++)
                    {
                        DataPoint dataPoint = new DataPoint();
                        dataPoint.AxisXLabel = "abc" + j;
                        dataPoint.YValue = rand.Next(0, 100);
                        dataSeries.DataPoints.Add(dataPoint);
                    }
                    _chart.Series.Add(dataSeries);
                }
            });

            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                _htmlElement1 = Common.GetDisplayMessageButton(_htmlElement1);
                _htmlElement1.SetStyleAttribute("width", "900px");
                _htmlElement1.SetProperty("value", "Click here to check DataPoint Width at realtime.");
                _htmlElement2 = Common.GetDisplayMessageButton(_htmlElement2);
                _htmlElement2.SetStyleAttribute("top", "540px");
                _htmlElement2.SetProperty("value", "Switch 2D/3D.");
                _htmlElement3 = Common.GetDisplayMessageButton(_htmlElement3);
                _htmlElement3.SetStyleAttribute("top", "560px");
                _htmlElement3.SetProperty("value", " Click here to exit.");
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement1);
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement2);
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement3);
            });

            EnqueueDelay(_sleepTime);
            EnqueueCallback(() =>
            {
                _htmlElement1.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.HtmlElement1_OnClick));
                _htmlElement2.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.View3D_OnClick));
                _htmlElement3.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.HtmlElement2_OnClick));
            });
        }
        #endregion

        #region TestNegativeDataPointWidthInStackedColumn100
        [TestMethod]
        [Asynchronous]
        public void TestNegativeDataPointWidthInStackedColumn100()
        {
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "400px");

            _chart = new Chart();
            _chart.Width = 400;
            _chart.Height = 300;

            Random rand = new Random();

            _isLoaded = false;
            _chart.Loaded += new RoutedEventHandler(chart_Loaded);

            TestPanel.Children.Add(_chart);

            DataSeries dataSeries = new DataSeries();
            dataSeries.RenderAs = RenderAs.StackedColumn100;
            dataSeries.DataPoints.Add(new DataPoint() { YValue = 200 });
            dataSeries.DataPoints.Add(new DataPoint() { YValue = 100 });
            dataSeries.DataPoints.Add(new DataPoint() { YValue = 300 });
            _chart.Series.Add(dataSeries);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                _chart.DataPointWidth = -10;
            });
            EnqueueCallback(() =>
            {
                EnqueueTestComplete();
            });
        }
        #endregion

        #region TestSingleDataPointWidthInStackedBar
        /// <summary>
        /// Test single DataPoint width in StackedBar
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void TestSingleDataPointWidthInStackedBar()
        {
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "400px");

            _chart = new Chart();
            _chart.Width = 300;
            _chart.Height = 400;

            Random rand = new Random();

            _isLoaded = false;
            _chart.Loaded += new RoutedEventHandler(chart_Loaded);

            TestPanel.Children.Add(_chart);

            DataSeries dataSeries = new DataSeries();
            dataSeries.RenderAs = RenderAs.StackedBar;
            dataSeries.DataPoints.Add(new DataPoint() { YValue = 200 });
            _chart.Series.Add(dataSeries);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                _chart.DataPointWidth = 10;
            });

            EnqueueCallback(() =>
            {
                _htmlElement1 = Common.GetDisplayMessageButton(_htmlElement1);
                _htmlElement1.SetStyleAttribute("width", "900px");
                _htmlElement1.SetProperty("value", "Click here to check DataPoint Width at realtime.");
                _htmlElement2 = Common.GetDisplayMessageButton(_htmlElement2);
                _htmlElement2.SetStyleAttribute("top", "540px");
                _htmlElement2.SetProperty("value", " Click here to exit.");
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement1);
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement2);
            });

            EnqueueDelay(_sleepTime);
            EnqueueCallback(() =>
            {
                _htmlElement1.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.HtmlElement1_OnClick));
                _htmlElement2.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.HtmlElement2_OnClick));
            });
        }
        #endregion

        #region TestSingleDataPointWidthInStackedBar3D
        /// <summary>
        /// Test single DataPoint width in 3D StackedBar
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void TestSingleDataPointWidthInStackedBar3D()
        {
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "400px");

            _chart = new Chart();
            _chart.Width = 300;
            _chart.Height = 400;
            _chart.View3D = true;

            Random rand = new Random();

            _isLoaded = false;
            _chart.Loaded += new RoutedEventHandler(chart_Loaded);

            TestPanel.Children.Add(_chart);

            DataSeries dataSeries = new DataSeries();
            dataSeries.RenderAs = RenderAs.StackedBar;
            dataSeries.DataPoints.Add(new DataPoint() { YValue = 200 });
            _chart.Series.Add(dataSeries);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                _chart.DataPointWidth = 10;
            });

            EnqueueCallback(() =>
            {
                _htmlElement1 = Common.GetDisplayMessageButton(_htmlElement1);
                _htmlElement1.SetStyleAttribute("width", "900px");
                _htmlElement1.SetProperty("value", "Click here to check DataPoint Width at realtime.");
                _htmlElement2 = Common.GetDisplayMessageButton(_htmlElement2);
                _htmlElement2.SetStyleAttribute("top", "540px");
                _htmlElement2.SetProperty("value", " Click here to exit.");
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement1);
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement2);
            });

            EnqueueDelay(_sleepTime);
            EnqueueCallback(() =>
            {
                _htmlElement1.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.HtmlElement1_OnClick));
                _htmlElement2.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.HtmlElement2_OnClick));
            });
        }
        #endregion

        #region TestDataPointWidth4DecimalXValueInStackedBar
        /// <summary>
        /// Test DataPoint width with Decimal XValues in StackedBar
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void TestDataPointWidth4DecimalXValueInStackedBar()
        {
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "400px");

            _chart = new Chart();
            _chart.Width = 300;
            _chart.Height = 400;

            _isLoaded = false;
            _chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Random rand = new Random();

            TestPanel.Children.Add(_chart);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                DataSeries dataSeries1 = new DataSeries();
                dataSeries1.RenderAs = RenderAs.StackedBar;

                for (Double i = 0; i < 10; i++)
                {
                    DataPoint dataPoint = new DataPoint();
                    dataPoint.XValue = rand.NextDouble() + i;
                    dataPoint.YValue = rand.Next(0, 100);
                    dataSeries1.DataPoints.Add(dataPoint);
                }
                _chart.Series.Add(dataSeries1);
            });

            EnqueueCallback(() =>
            {
                _htmlElement1 = Common.GetDisplayMessageButton(_htmlElement1);
                _htmlElement1.SetStyleAttribute("width", "900px");
                _htmlElement1.SetProperty("value", "Click here to check DataPoint Width at realtime.");
                _htmlElement2 = Common.GetDisplayMessageButton(_htmlElement2);
                _htmlElement2.SetStyleAttribute("top", "540px");
                _htmlElement2.SetProperty("value", " Click here to exit.");
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement1);
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement2);
            });

            EnqueueDelay(_sleepTime);
            EnqueueCallback(() =>
            {
                _htmlElement1.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.HtmlElement1_OnClick));
                _htmlElement2.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.HtmlElement2_OnClick));
            });
        }
        #endregion

        #region TestMultiSeriesDataPointWidth4DecimalXValueInStackedBar
        /// <summary>
        /// Test multiseries DataPoint width with Decimal XValues in StackedBar
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void TestMultiSeriesDataPointWidth4DecimalXValueInStackedBar()
        {
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "400px");

            _chart = new Chart();
            _chart.Width = 300;
            _chart.Height = 400;

            _isLoaded = false;
            _chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Random rand = new Random();

            List<Double> xList = new List<Double>();
            List<Double> yList = new List<Double>();
            Double y = 0;

            TestPanel.Children.Add(_chart);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                DataSeries dataSeries1 = new DataSeries();
                dataSeries1.RenderAs = RenderAs.StackedBar;

                for (Double i = 0; i < 10; i++)
                {
                    DataPoint dataPoint = new DataPoint();
                    dataPoint.XValue = i + 1;
                    xList.Add(i);
                    dataPoint.YValue = (y = rand.Next(-100, 100));
                    yList.Add(y);
                    dataSeries1.DataPoints.Add(dataPoint);
                }
                _chart.Series.Add(dataSeries1);
            });

            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {

                DataSeries dataSeries2 = new DataSeries();
                dataSeries2.RenderAs = RenderAs.StackedBar;

                Double j = 0.5;
                for (Int32 i = 0; i < 10; i++)
                {
                    DataPoint dataPoint = new DataPoint();
                    dataPoint.XValue = j;
                    dataPoint.YValue = yList[i];
                    dataSeries2.DataPoints.Add(dataPoint);
                    j++;
                }
                _chart.Series.Add(dataSeries2);
            });

            EnqueueCallback(() =>
            {
                _htmlElement1 = Common.GetDisplayMessageButton(_htmlElement1);
                _htmlElement1.SetStyleAttribute("width", "900px");
                _htmlElement1.SetProperty("value", "Click here to check DataPoint Width at realtime.");
                _htmlElement2 = Common.GetDisplayMessageButton(_htmlElement2);
                _htmlElement2.SetStyleAttribute("top", "540px");
                _htmlElement2.SetProperty("value", " Click here to exit.");
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement1);
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement2);
            });

            EnqueueDelay(_sleepTime);
            EnqueueCallback(() =>
            {
                _htmlElement1.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.HtmlElement1_OnClick));
                _htmlElement2.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.HtmlElement2_OnClick));
            });
        }
        #endregion

        #region TestMultiSeriesDataPointWidthInStackedBar
        /// <summary>
        /// Test DataPoint width in multiseries StackedBar chart
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void TestMultiSeriesDataPointWidthInStackedBar()
        {
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "400px");

            _chart = new Chart();
            _chart.Width = 300;
            _chart.Height = 400;

            _isLoaded = false;
            _chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Random rand = new Random();

            TestPanel.Children.Add(_chart);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                for (Int32 i = 0; i < 2; i++)
                {
                    DataSeries dataSeries = new DataSeries();
                    dataSeries.RenderAs = RenderAs.StackedBar;

                    for (Double j = 0; j < 10; j++)
                    {
                        DataPoint dataPoint = new DataPoint();
                        dataPoint.XValue = j + 1;
                        dataPoint.YValue = rand.Next(0, 100);
                        dataSeries.DataPoints.Add(dataPoint);
                    }
                    _chart.Series.Add(dataSeries);
                }
            });

            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                _htmlElement1 = Common.GetDisplayMessageButton(_htmlElement1);
                _htmlElement1.SetStyleAttribute("width", "900px");
                _htmlElement1.SetProperty("value", "Click here to check DataPoint Width at realtime.");
                _htmlElement2 = Common.GetDisplayMessageButton(_htmlElement2);
                _htmlElement2.SetStyleAttribute("top", "540px");
                _htmlElement2.SetProperty("value", " Click here to exit.");
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement1);
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement2);
            });

            EnqueueDelay(_sleepTime);
            EnqueueCallback(() =>
            {
                _htmlElement1.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.HtmlElement1_OnClick));
                _htmlElement2.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.HtmlElement2_OnClick));
            });
        }
        #endregion

        #region TestMultiSeriesDataPointWidthInStackedBar3D
        /// <summary>
        /// Test DataPoint width in multiseries 3D StackedBar chart
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void TestMultiSeriesDataPointWidthInStackedBar3D()
        {
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "400px");

            _chart = new Chart();
            _chart.Width = 300;
            _chart.Height = 400;
            _chart.View3D = true;

            _isLoaded = false;
            _chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Random rand = new Random();

            TestPanel.Children.Add(_chart);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                for (Int32 i = 0; i < 2; i++)
                {
                    DataSeries dataSeries = new DataSeries();
                    dataSeries.RenderAs = RenderAs.StackedBar;

                    for (Double j = 0; j < 10; j++)
                    {
                        DataPoint dataPoint = new DataPoint();
                        dataPoint.XValue = j + 1;
                        dataPoint.YValue = rand.Next(0, 100);
                        dataSeries.DataPoints.Add(dataPoint);
                    }
                    _chart.Series.Add(dataSeries);
                }
            });

            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                _htmlElement1 = Common.GetDisplayMessageButton(_htmlElement1);
                _htmlElement1.SetStyleAttribute("width", "900px");
                _htmlElement1.SetProperty("value", "Click here to check DataPoint Width at realtime.");
                _htmlElement2 = Common.GetDisplayMessageButton(_htmlElement2);
                _htmlElement2.SetStyleAttribute("top", "540px");
                _htmlElement2.SetProperty("value", " Click here to exit.");
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement1);
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement2);
            });

            EnqueueDelay(_sleepTime);
            EnqueueCallback(() =>
            {
                _htmlElement1.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.HtmlElement1_OnClick));
                _htmlElement2.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.HtmlElement2_OnClick));
            });
        }
        #endregion

        #region TestDataPointWidthWithScrollInStackedBar
        /// <summary>
        /// Test DataPoint width in 3D Column chart with Scroll in StackedBar chart
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void TestDataPointWidthWithScrollInStackedBar()
        {
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "400px");

            _chart = new Chart();
            _chart.Width = 300;
            _chart.Height = 400;
            _chart.View3D = true;

            _isLoaded = false;
            _chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Random rand = new Random();

            TestPanel.Children.Add(_chart);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                for (Int32 i = 0; i < 1; i++)
                {
                    DataSeries dataSeries = new DataSeries();
                    dataSeries.RenderAs = RenderAs.StackedBar;

                    for (Double j = 0; j < 60; j++)
                    {
                        DataPoint dataPoint = new DataPoint();
                        dataPoint.AxisXLabel = "abc" + j;
                        dataPoint.YValue = rand.Next(0, 100);
                        dataSeries.DataPoints.Add(dataPoint);
                    }
                    _chart.Series.Add(dataSeries);
                }
            });

            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                _htmlElement1 = Common.GetDisplayMessageButton(_htmlElement1);
                _htmlElement1.SetStyleAttribute("width", "900px");
                _htmlElement1.SetProperty("value", "Click here to check DataPoint Width at realtime.");
                _htmlElement2 = Common.GetDisplayMessageButton(_htmlElement2);
                _htmlElement2.SetStyleAttribute("top", "540px");
                _htmlElement2.SetProperty("value", "Switch 2D/3D.");
                _htmlElement3 = Common.GetDisplayMessageButton(_htmlElement3);
                _htmlElement3.SetStyleAttribute("top", "560px");
                _htmlElement3.SetProperty("value", " Click here to exit.");
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement1);
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement2);
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement3);
            });

            EnqueueDelay(_sleepTime);
            EnqueueCallback(() =>
            {
                _htmlElement1.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.HtmlElement1_OnClick));
                _htmlElement2.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.View3D_OnClick));
                _htmlElement3.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.HtmlElement2_OnClick));
            });
        }
        #endregion

        #region TestNegativeDataPointWidthInStackedBar
        [TestMethod]
        [Asynchronous]
        public void TestNegativeDataPointWidthInStackedBar()
        {
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "400px");

            _chart = new Chart();
            _chart.Width = 300;
            _chart.Height = 400;

            Random rand = new Random();

            _isLoaded = false;
            _chart.Loaded += new RoutedEventHandler(chart_Loaded);

            TestPanel.Children.Add(_chart);

            DataSeries dataSeries = new DataSeries();
            dataSeries.RenderAs = RenderAs.StackedBar;
            dataSeries.DataPoints.Add(new DataPoint() { YValue = 200 });
            dataSeries.DataPoints.Add(new DataPoint() { YValue = 100 });
            dataSeries.DataPoints.Add(new DataPoint() { YValue = 300 });
            _chart.Series.Add(dataSeries);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                _chart.DataPointWidth = -10;
            });
            EnqueueCallback(() =>
            {
                EnqueueTestComplete();
            });
        }
        #endregion

        #region TestSingleDataPointWidthInStackedBar100
        /// <summary>
        /// Test single DataPoint width in StackedBar100
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void TestSingleDataPointWidthInStackedBar100()
        {
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "400px");

            _chart = new Chart();
            _chart.Width = 300;
            _chart.Height = 400;

            Random rand = new Random();

            _isLoaded = false;
            _chart.Loaded += new RoutedEventHandler(chart_Loaded);

            TestPanel.Children.Add(_chart);

            DataSeries dataSeries = new DataSeries();
            dataSeries.RenderAs = RenderAs.StackedBar100;
            dataSeries.DataPoints.Add(new DataPoint() { YValue = 200 });
            _chart.Series.Add(dataSeries);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                _chart.DataPointWidth = 10;
            });

            EnqueueCallback(() =>
            {
                _htmlElement1 = Common.GetDisplayMessageButton(_htmlElement1);
                _htmlElement1.SetStyleAttribute("width", "900px");
                _htmlElement1.SetProperty("value", "Click here to check DataPoint Width at realtime.");
                _htmlElement2 = Common.GetDisplayMessageButton(_htmlElement2);
                _htmlElement2.SetStyleAttribute("top", "540px");
                _htmlElement2.SetProperty("value", " Click here to exit.");
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement1);
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement2);
            });

            EnqueueDelay(_sleepTime);
            EnqueueCallback(() =>
            {
                _htmlElement1.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.HtmlElement1_OnClick));
                _htmlElement2.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.HtmlElement2_OnClick));
            });
        }
        #endregion

        #region TestSingleDataPointWidthInStackedBar1003D
        /// <summary>
        /// Test single DataPoint width in 3D StackedBar100
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void TestSingleDataPointWidthInStackedBar1003D()
        {
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "400px");

            _chart = new Chart();
            _chart.Width = 300;
            _chart.Height = 400;
            _chart.View3D = true;

            Random rand = new Random();

            _isLoaded = false;
            _chart.Loaded += new RoutedEventHandler(chart_Loaded);

            TestPanel.Children.Add(_chart);

            DataSeries dataSeries = new DataSeries();
            dataSeries.RenderAs = RenderAs.StackedBar100;
            dataSeries.DataPoints.Add(new DataPoint() { YValue = 200 });
            _chart.Series.Add(dataSeries);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                _chart.DataPointWidth = 10;
            });

            EnqueueCallback(() =>
            {
                _htmlElement1 = Common.GetDisplayMessageButton(_htmlElement1);
                _htmlElement1.SetStyleAttribute("width", "900px");
                _htmlElement1.SetProperty("value", "Click here to check DataPoint Width at realtime.");
                _htmlElement2 = Common.GetDisplayMessageButton(_htmlElement2);
                _htmlElement2.SetStyleAttribute("top", "540px");
                _htmlElement2.SetProperty("value", " Click here to exit.");
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement1);
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement2);
            });

            EnqueueDelay(_sleepTime);
            EnqueueCallback(() =>
            {
                _htmlElement1.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.HtmlElement1_OnClick));
                _htmlElement2.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.HtmlElement2_OnClick));
            });
        }
        #endregion

        #region TestDataPointWidth4DecimalXValueInStackedBar100
        /// <summary>
        /// Test DataPoint width with Decimal XValues in StackedBar100
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void TestDataPointWidth4DecimalXValueInStackedBar100()
        {
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "400px");

            _chart = new Chart();
            _chart.Width = 300;
            _chart.Height = 400;

            _isLoaded = false;
            _chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Random rand = new Random();

            TestPanel.Children.Add(_chart);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                DataSeries dataSeries1 = new DataSeries();
                dataSeries1.RenderAs = RenderAs.StackedBar100;

                for (Double i = 0; i < 10; i++)
                {
                    DataPoint dataPoint = new DataPoint();
                    dataPoint.XValue = rand.NextDouble() + i;
                    dataPoint.YValue = rand.Next(0, 100);
                    dataSeries1.DataPoints.Add(dataPoint);
                }
                _chart.Series.Add(dataSeries1);
            });

            EnqueueCallback(() =>
            {
                _htmlElement1 = Common.GetDisplayMessageButton(_htmlElement1);
                _htmlElement1.SetStyleAttribute("width", "900px");
                _htmlElement1.SetProperty("value", "Click here to check DataPoint Width at realtime.");
                _htmlElement2 = Common.GetDisplayMessageButton(_htmlElement2);
                _htmlElement2.SetStyleAttribute("top", "540px");
                _htmlElement2.SetProperty("value", " Click here to exit.");
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement1);
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement2);
            });

            EnqueueDelay(_sleepTime);
            EnqueueCallback(() =>
            {
                _htmlElement1.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.HtmlElement1_OnClick));
                _htmlElement2.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.HtmlElement2_OnClick));
            });
        }
        #endregion

        #region TestMultiSeriesDataPointWidth4DecimalXValueInStackedBar100
        /// <summary>
        /// Test multiseries DataPoint width with Decimal XValues in StackedBar100
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void TestMultiSeriesDataPointWidth4DecimalXValueInStackedBar100()
        {
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "400px");

            _chart = new Chart();
            _chart.Width = 300;
            _chart.Height = 400;

            _isLoaded = false;
            _chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Random rand = new Random();

            List<Double> xList = new List<Double>();
            List<Double> yList = new List<Double>();
            Double y = 0;

            TestPanel.Children.Add(_chart);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                DataSeries dataSeries1 = new DataSeries();
                dataSeries1.RenderAs = RenderAs.StackedBar100;

                for (Double i = 0; i < 10; i++)
                {
                    DataPoint dataPoint = new DataPoint();
                    dataPoint.XValue = i + 1;
                    xList.Add(i);
                    dataPoint.YValue = (y = rand.Next(-100, 100));
                    yList.Add(y);
                    dataSeries1.DataPoints.Add(dataPoint);
                }
                _chart.Series.Add(dataSeries1);
            });

            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {

                DataSeries dataSeries2 = new DataSeries();
                dataSeries2.RenderAs = RenderAs.StackedBar100;

                Double j = 0.5;
                for (Int32 i = 0; i < 10; i++)
                {
                    DataPoint dataPoint = new DataPoint();
                    dataPoint.XValue = j;
                    dataPoint.YValue = yList[i];
                    dataSeries2.DataPoints.Add(dataPoint);
                    j++;
                }
                _chart.Series.Add(dataSeries2);
            });

            EnqueueCallback(() =>
            {
                _htmlElement1 = Common.GetDisplayMessageButton(_htmlElement1);
                _htmlElement1.SetStyleAttribute("width", "900px");
                _htmlElement1.SetProperty("value", "Click here to check DataPoint Width at realtime.");
                _htmlElement2 = Common.GetDisplayMessageButton(_htmlElement2);
                _htmlElement2.SetStyleAttribute("top", "540px");
                _htmlElement2.SetProperty("value", " Click here to exit.");
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement1);
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement2);
            });

            EnqueueDelay(_sleepTime);
            EnqueueCallback(() =>
            {
                _htmlElement1.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.HtmlElement1_OnClick));
                _htmlElement2.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.HtmlElement2_OnClick));
            });
        }
        #endregion

        #region TestMultiSeriesDataPointWidthInStackedBar100
        /// <summary>
        /// Test DataPoint width in multiseries StackedBar100 chart
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void TestMultiSeriesDataPointWidthInStackedBar100()
        {
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "400px");

            _chart = new Chart();
            _chart.Width = 300;
            _chart.Height = 400;

            _isLoaded = false;
            _chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Random rand = new Random();

            TestPanel.Children.Add(_chart);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                for (Int32 i = 0; i < 2; i++)
                {
                    DataSeries dataSeries = new DataSeries();
                    dataSeries.RenderAs = RenderAs.StackedBar100;

                    for (Double j = 0; j < 10; j++)
                    {
                        DataPoint dataPoint = new DataPoint();
                        dataPoint.XValue = j + 1;
                        dataPoint.YValue = rand.Next(0, 100);
                        dataSeries.DataPoints.Add(dataPoint);
                    }
                    _chart.Series.Add(dataSeries);
                }
            });

            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                _htmlElement1 = Common.GetDisplayMessageButton(_htmlElement1);
                _htmlElement1.SetStyleAttribute("width", "900px");
                _htmlElement1.SetProperty("value", "Click here to check DataPoint Width at realtime.");
                _htmlElement2 = Common.GetDisplayMessageButton(_htmlElement2);
                _htmlElement2.SetStyleAttribute("top", "540px");
                _htmlElement2.SetProperty("value", " Click here to exit.");
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement1);
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement2);
            });

            EnqueueDelay(_sleepTime);
            EnqueueCallback(() =>
            {
                _htmlElement1.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.HtmlElement1_OnClick));
                _htmlElement2.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.HtmlElement2_OnClick));
            });
        }
        #endregion

        #region TestMultiSeriesDataPointWidthInStackedBar1003D
        /// <summary>
        /// Test DataPoint width in multiseries 3D StackedBar100 chart
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void TestMultiSeriesDataPointWidthInStackedBar1003D()
        {
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "400px");

            _chart = new Chart();
            _chart.Width = 300;
            _chart.Height = 400;
            _chart.View3D = true;

            _isLoaded = false;
            _chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Random rand = new Random();

            TestPanel.Children.Add(_chart);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                for (Int32 i = 0; i < 2; i++)
                {
                    DataSeries dataSeries = new DataSeries();
                    dataSeries.RenderAs = RenderAs.StackedBar100;

                    for (Double j = 0; j < 10; j++)
                    {
                        DataPoint dataPoint = new DataPoint();
                        dataPoint.XValue = j + 1;
                        dataPoint.YValue = rand.Next(0, 100);
                        dataSeries.DataPoints.Add(dataPoint);
                    }
                    _chart.Series.Add(dataSeries);
                }
            });

            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                _htmlElement1 = Common.GetDisplayMessageButton(_htmlElement1);
                _htmlElement1.SetStyleAttribute("width", "900px");
                _htmlElement1.SetProperty("value", "Click here to check DataPoint Width at realtime.");
                _htmlElement2 = Common.GetDisplayMessageButton(_htmlElement2);
                _htmlElement2.SetStyleAttribute("top", "540px");
                _htmlElement2.SetProperty("value", " Click here to exit.");
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement1);
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement2);
            });

            EnqueueDelay(_sleepTime);
            EnqueueCallback(() =>
            {
                _htmlElement1.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.HtmlElement1_OnClick));
                _htmlElement2.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.HtmlElement2_OnClick));
            });
        }
        #endregion

        #region TestDataPointWidthWithScrollInStackedBar100
        /// <summary>
        /// Test DataPoint width in 3D Column chart with Scroll in StackedBar100 chart
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void TestDataPointWidthWithScrollInStackedBar100()
        {
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "400px");

            _chart = new Chart();
            _chart.Width = 300;
            _chart.Height = 400;
            _chart.View3D = true;

            _isLoaded = false;
            _chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Random rand = new Random();

            TestPanel.Children.Add(_chart);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                for (Int32 i = 0; i < 1; i++)
                {
                    DataSeries dataSeries = new DataSeries();
                    dataSeries.RenderAs = RenderAs.StackedBar100;

                    for (Double j = 0; j < 60; j++)
                    {
                        DataPoint dataPoint = new DataPoint();
                        dataPoint.AxisXLabel = "abc" + j;
                        dataPoint.YValue = rand.Next(0, 100);
                        dataSeries.DataPoints.Add(dataPoint);
                    }
                    _chart.Series.Add(dataSeries);
                }
            });

            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                _htmlElement1 = Common.GetDisplayMessageButton(_htmlElement1);
                _htmlElement1.SetStyleAttribute("width", "900px");
                _htmlElement1.SetProperty("value", "Click here to check DataPoint Width at realtime.");
                _htmlElement2 = Common.GetDisplayMessageButton(_htmlElement2);
                _htmlElement2.SetStyleAttribute("top", "540px");
                _htmlElement2.SetProperty("value", "Switch 2D/3D.");
                _htmlElement3 = Common.GetDisplayMessageButton(_htmlElement3);
                _htmlElement3.SetStyleAttribute("top", "560px");
                _htmlElement3.SetProperty("value", " Click here to exit.");
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement1);
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement2);
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement3);
            });

            EnqueueDelay(_sleepTime);
            EnqueueCallback(() =>
            {
                _htmlElement1.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.HtmlElement1_OnClick));
                _htmlElement2.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.View3D_OnClick));
                _htmlElement3.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.HtmlElement2_OnClick));
            });
        }
        #endregion

        #region TestNegativeDataPointWidthInStackedBar100
        [TestMethod]
        [Asynchronous]
        public void TestNegativeDataPointWidthInStackedBar100()
        {
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "400px");

            _chart = new Chart();
            _chart.Width = 300;
            _chart.Height = 400;

            Random rand = new Random();

            _isLoaded = false;
            _chart.Loaded += new RoutedEventHandler(chart_Loaded);

            TestPanel.Children.Add(_chart);

            DataSeries dataSeries = new DataSeries();
            dataSeries.RenderAs = RenderAs.StackedBar100;
            dataSeries.DataPoints.Add(new DataPoint() { YValue = 200 });
            dataSeries.DataPoints.Add(new DataPoint() { YValue = 100 });
            dataSeries.DataPoints.Add(new DataPoint() { YValue = 300 });
            _chart.Series.Add(dataSeries);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                _chart.DataPointWidth = -10;
            });
            EnqueueCallback(() =>
            {
                EnqueueTestComplete();
            });
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
        /// Event handler for click event of the Html element
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HtmlElement2_OnClick(object sender, HtmlEventArgs e)
        {
            if (timer.IsEnabled)
                timer.Stop();

            EnqueueTestComplete();

            if (_htmlElement1 != null)
                System.Windows.Browser.HtmlPage.Document.Body.RemoveChild(_htmlElement1);

            if (_htmlElement2 != null)
                System.Windows.Browser.HtmlPage.Document.Body.RemoveChild(_htmlElement2);

            if(_htmlElement3 != null)
                System.Windows.Browser.HtmlPage.Document.Body.RemoveChild(_htmlElement3);

            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "100%");
        }

        /// <summary>
        /// Event handler for click event of the Html element
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HtmlElement1_OnClick(object sender, HtmlEventArgs e)
        {
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 1500);
            timer.Start();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            _chart.DataPointWidth = rand.Next(0, 200);
        }

        /// <summary>
        /// Event handler for click event of the Html element
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void View3D_OnClick(object sender, System.Windows.Browser.HtmlEventArgs e)
        {
            if (_chart.View3D)
                _chart.View3D = false;
            else
                _chart.View3D = true;

        }

        /// <summary>
        /// Event handler for click event of the Html element
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Exit_OnClick(object sender, System.Windows.Browser.HtmlEventArgs e)
        {
            EnqueueTestComplete();
            try
            {
                System.Windows.Browser.HtmlPage.Document.Body.RemoveChild(_htmlElement1);
                System.Windows.Browser.HtmlPage.Document.Body.RemoveChild(_htmlElement2);
                System.Windows.Browser.HtmlPage.Document.Body.RemoveChild(_htmlElement3);
                System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "100%");
            }
            catch { }
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
        /// Html element reference
        /// </summary>
        private System.Windows.Browser.HtmlElement _htmlElement3;

        /// <summary>
        /// Number of milliseconds to wait between actions in CreateAsyncTasks or Enqueue callbacks. 
        /// </summary>
        private const int _sleepTime = 1000;

        /// <summary>
        /// Chart 
        /// </summary>
        private Chart _chart;

        // Create a new instance of Random class
        private Random rand = new Random();

        /// <summary>
        /// Whether the chart is loaded
        /// </summary>
        private bool _isLoaded = false;

        /// <summary>
        /// Dispatch Timer
        /// </summary>
        private System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();

        #endregion
    }
}
