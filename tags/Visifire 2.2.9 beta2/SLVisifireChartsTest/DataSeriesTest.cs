﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Input;
using System.Collections.Specialized;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Globalization;
using Visifire.Charts;
using Visifire.Commons;

namespace SLVisifireChartsTest
{
    /// <summary>
    /// This class runs the unit tests Visifire.Charts.DataSeries class 
    /// </summary>
    [TestClass]
    public class DataSeriesTest : SilverlightControlTest
    {
        #region PieStressChecking
        /// <summary>
        /// Stress testing Pie Chart
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void PieStressChecking()
        {
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "400px");

            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;
            chart.View3D = true;
            chart.SmartLabelEnabled = true;

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            TestPanel.Children.Add(chart);

            Random rand = new Random();

            DataSeries dataSeries = new DataSeries();
            dataSeries.RenderAs = RenderAs.Pie;
            dataSeries.ShowInLegend = false;

            Int32 numberOfDataPoints = 0;
            Double totalDuration = 0;
            DateTime start = DateTime.UtcNow;
            String msg = Common.AssertAverageDuration(200, 1,
                delegate
                {
                    for (Int32 i = 0; i < 500; i++)
                    {
                        DataPoint dataPoint = new DataPoint();
                        dataPoint.XValue = i + 1;
                        dataPoint.YValue = rand.Next(-500, 500);
                        dataPoint.Exploded = true;
                        dataSeries.DataPoints.Add(dataPoint);
                        numberOfDataPoints++;
                    }
                    chart.Series.Add(dataSeries);
                });

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
                _htmlElement1.SetProperty("value", dataSeries.RenderAs + " chart with " + numberOfDataPoints + " DataPoints. Total Chart Loading Time: " + totalDuration + "s. Number of Render Count: " + chart.ChartArea._renderCount);
                _htmlElement2 = Common.GetDisplayMessageButton(_htmlElement2);
                _htmlElement2.SetStyleAttribute("top", "540px");
                _htmlElement2.SetProperty("value", "Total Series Calculation: " + msg + " Click here to exit");
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement1);
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement2);
            });

            EnqueueCallback(() =>
            {
                _htmlElement2.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.HtmlElement_OnClick));
            });
        }
        #endregion

        #region ExplodedPieChecking
        /// <summary>
        /// Testing Pie Chart exploded
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void ExplodedPieChecking()
        {
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "400px");

            _chart = new Chart();
            _chart.Width = 500;
            _chart.Height = 300;

            _isLoaded = false;
            _chart.Loaded += new RoutedEventHandler(chart_Loaded);

            TestPanel.Children.Add(_chart);

            Random rand = new Random();

            DataSeries dataSeries = new DataSeries();
            dataSeries.RenderAs = RenderAs.Pie;

            for (Int32 i = 0; i < 20; i++)
            {
                DataPoint dataPoint = new DataPoint();
                dataPoint.YValue = rand.Next(100, 500);
                dataSeries.DataPoints.Add(dataPoint);

            }
            _chart.Series.Add(dataSeries);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                _htmlElement1 = Common.GetDisplayMessageButton(_htmlElement1);
                _htmlElement1.SetStyleAttribute("width", "900px");
                _htmlElement1.SetProperty("value", "Click here to View3D(true/false).");
                _htmlElement2 = Common.GetDisplayMessageButton(_htmlElement2);
                _htmlElement2.SetStyleAttribute("top", "540px");
                _htmlElement2.SetProperty("value", "Switch Pie/Doughnut.");
                _htmlElement3 = Common.GetDisplayMessageButton(_htmlElement3);
                _htmlElement3.SetStyleAttribute("top", "560px");
                _htmlElement3.SetProperty("value", "Number of Render Count: " + _chart.ChartArea._renderCount + ". Click here to exit.");
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement1);
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement2);
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement3);
            });

            EnqueueCallback(() =>
            {
                _htmlElement1.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.View3D_OnClick));
            });

            EnqueueCallback(() =>
            {
                _htmlElement2.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.SwitchDataSeries_OnClick));
            });

            EnqueueCallback(() =>
            {
                _htmlElement3.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.Exit_OnClick));
            });

        }
        #endregion

        #region TestingMovingMarkerInLine
        /// <summary>
        /// Testing Moving marker in Line chart
        [TestMethod]
        [Asynchronous]
        public void TestingMovingMarkerInLine()
        {
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "400px");

            _chart = new Chart();
            _chart.Width = 500;
            _chart.Height = 300;

            _isLoaded = false;
            _chart.Loaded += new RoutedEventHandler(chart_Loaded);

            TestPanel.Children.Add(_chart);

            Random rand = new Random();

            DataSeries dataSeries = new DataSeries();
            dataSeries.RenderAs = RenderAs.Line;
            dataSeries.MovingMarkerEnabled = true;

            for (Int32 i = 0; i < 6; i++)
            {
                DataPoint dataPoint = new DataPoint();
                dataPoint.YValue = rand.Next(100, 500);
                dataSeries.DataPoints.Add(dataPoint);

            }
            _chart.Series.Add(dataSeries);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                _htmlElement1 = Common.GetDisplayMessageButton(_htmlElement1);
                _htmlElement1.SetStyleAttribute("width", "900px");
                _htmlElement1.SetProperty("value", "Move mouse on PlotArea.");
                _htmlElement2 = Common.GetDisplayMessageButton(_htmlElement2);
                _htmlElement2.SetStyleAttribute("top", "540px");
                _htmlElement2.SetProperty("value", "Click here to exit.");
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement1);
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement2);
            });

            EnqueueCallback(() =>
            {
                _htmlElement2.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.Exit_OnClick));
            });

        }
        #endregion

        #region TestingMovingMarkerInLineOnSomeDataseries
        /// <summary>
        /// Testing Moving marker on few DataSeries in Line chart
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void TestingMovingMarkerInLineOnSomeDataseries()
        {
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "400px");

            _chart = new Chart();
            _chart.Width = 500;
            _chart.Height = 300;

            _isLoaded = false;
            _chart.Loaded += new RoutedEventHandler(chart_Loaded);

            TestPanel.Children.Add(_chart);

            Random rand = new Random();

            for (Int32 j = 0; j < 5; j++)
            {
                DataSeries dataSeries = new DataSeries();
                dataSeries.RenderAs = RenderAs.Line;
                if (j < 4)
                    dataSeries.MovingMarkerEnabled = true;

                for (Int32 i = 0; i < 6; i++)
                {
                    DataPoint dataPoint = new DataPoint();
                    dataPoint.YValue = rand.Next(100, 500);
                    dataSeries.DataPoints.Add(dataPoint);

                }
                _chart.Series.Add(dataSeries);
            }

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                _htmlElement1 = Common.GetDisplayMessageButton(_htmlElement1);
                _htmlElement1.SetStyleAttribute("width", "900px");
                _htmlElement1.SetProperty("value", "Move mouse on PlotArea.");
                _htmlElement2 = Common.GetDisplayMessageButton(_htmlElement2);
                _htmlElement2.SetStyleAttribute("top", "540px");
                _htmlElement2.SetProperty("value", "Click here to exit.");
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement1);
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement2);
            });

            EnqueueCallback(() =>
            {
                _htmlElement2.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.Exit_OnClick));
            });

        }
        #endregion

        #region TestingSelectionInPie
        /// <summary>
        /// Testing Selection in Pie Chart
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void TestingSelectionInPie()
        {
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "400px");

            _chart = new Chart();
            _chart.Width = 500;
            _chart.Height = 300;

            _isLoaded = false;
            _chart.Loaded += new RoutedEventHandler(chart_Loaded);

            TestPanel.Children.Add(_chart);

            Random rand = new Random();

            DataSeries dataSeries = new DataSeries();
            dataSeries.RenderAs = RenderAs.Pie;
            dataSeries.SelectionEnabled = true;

            for (Int32 i = 0; i < 6; i++)
            {
                DataPoint dataPoint = new DataPoint();
                dataPoint.YValue = rand.Next(100, 500);
                if (i == 0)
                    dataPoint.Selected = true;
                dataSeries.DataPoints.Add(dataPoint);

            }
            _chart.Series.Add(dataSeries);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                _htmlElement1 = Common.GetDisplayMessageButton(_htmlElement1);
                _htmlElement1.SetStyleAttribute("width", "900px");
                _htmlElement1.SetProperty("value", "Click on a DataPoint to select.");
                _htmlElement2 = Common.GetDisplayMessageButton(_htmlElement2);
                _htmlElement2.SetStyleAttribute("top", "540px");
                _htmlElement2.SetProperty("value", "Switch 2D/3D.");
                _htmlElement3 = Common.GetDisplayMessageButton(_htmlElement3);
                _htmlElement3.SetStyleAttribute("top", "560px");
                _htmlElement3.SetProperty("value", "Click here to exit.");
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement1);
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement2);
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement3);
            });

            EnqueueCallback(() =>
            {
                _htmlElement2.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.View3D_OnClick));
            });

            EnqueueCallback(() =>
            {
                _htmlElement3.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.Exit_OnClick));
            });

        }
        #endregion

        #region TestingMultipleSelectionInPie
        /// <summary>
        /// Testing multiple Selection in Pie Chart
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void TestingMultipleSelectionInPie()
        {
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "400px");

            _chart = new Chart();
            _chart.Width = 500;
            _chart.Height = 300;

            _isLoaded = false;
            _chart.Loaded += new RoutedEventHandler(chart_Loaded);

            TestPanel.Children.Add(_chart);

            Random rand = new Random();

            DataSeries dataSeries = new DataSeries();
            dataSeries.RenderAs = RenderAs.Pie;
            dataSeries.SelectionEnabled = true;
            dataSeries.SelectionMode = SelectionModes.Multiple;

            for (Int32 i = 0; i < 6; i++)
            {
                DataPoint dataPoint = new DataPoint();
                dataPoint.YValue = rand.Next(100, 500);
                if (i == 0)
                    dataPoint.Selected = true;
                dataSeries.DataPoints.Add(dataPoint);

            }
            _chart.Series.Add(dataSeries);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                _htmlElement1 = Common.GetDisplayMessageButton(_htmlElement1);
                _htmlElement1.SetStyleAttribute("width", "900px");
                _htmlElement1.SetProperty("value", "Click on DataPoints to select.");
                _htmlElement2 = Common.GetDisplayMessageButton(_htmlElement2);
                _htmlElement2.SetStyleAttribute("top", "540px");
                _htmlElement2.SetProperty("value", "Switch 2D/3D.");
                _htmlElement3 = Common.GetDisplayMessageButton(_htmlElement3);
                _htmlElement3.SetStyleAttribute("top", "560px");
                _htmlElement3.SetProperty("value", "Click here to exit.");
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement1);
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement2);
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement3);
            });

            EnqueueCallback(() =>
            {
                _htmlElement2.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.View3D_OnClick));
            });

            EnqueueCallback(() =>
            {
                _htmlElement3.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.Exit_OnClick));
            });


        }
        #endregion

        #region TestingSelectionInColumn
        /// <summary>
        /// Testing Selection in Column Chart
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void TestingSelectionInColumn()
        {
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "400px");

            _chart = new Chart();
            _chart.Width = 500;
            _chart.Height = 300;
            _chart.View3D = true;

            _isLoaded = false;
            _chart.Loaded += new RoutedEventHandler(chart_Loaded);

            TestPanel.Children.Add(_chart);

            Random rand = new Random();

            for (Int32 j = 0; j < 2; j++)
            {
                DataSeries dataSeries = new DataSeries();
                dataSeries.SelectionEnabled = true;

                for (Int32 i = 0; i < 6; i++)
                {
                    DataPoint dataPoint = new DataPoint();
                    dataPoint.YValue = rand.Next(100, 500);
                    if (i == 0)
                        dataPoint.Selected = true;
                    dataSeries.DataPoints.Add(dataPoint);

                }
                _chart.Series.Add(dataSeries);
            }

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                _htmlElement1 = Common.GetDisplayMessageButton(_htmlElement1);
                _htmlElement1.SetStyleAttribute("width", "900px");
                _htmlElement1.SetProperty("value", "Click on a DataPoint to select.");
                _htmlElement2 = Common.GetDisplayMessageButton(_htmlElement2);
                _htmlElement2.SetStyleAttribute("top", "540px");
                _htmlElement2.SetProperty("value", "Switch 2D/3D.");
                _htmlElement3 = Common.GetDisplayMessageButton(_htmlElement3);
                _htmlElement3.SetStyleAttribute("top", "560px");
                _htmlElement3.SetProperty("value", "Click here to exit.");
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement1);
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement2);
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement3);
            });

            EnqueueCallback(() =>
            {
                _htmlElement2.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.View3D_OnClick));
            });

            EnqueueCallback(() =>
            {
                _htmlElement3.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.Exit_OnClick));
            });

        }
        #endregion

        #region TestingMultipleSelectionInColumn
        /// <summary>
        /// Testing multiple Selection in Column Chart
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void TestingMultipleSelectionInColumn()
        {
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "400px");

            _chart = new Chart();
            _chart.Width = 500;
            _chart.Height = 300;
            _chart.View3D = true;

            _isLoaded = false;
            _chart.Loaded += new RoutedEventHandler(chart_Loaded);

            TestPanel.Children.Add(_chart);

            Random rand = new Random();

            for (Int32 j = 0; j < 2; j++)
            {
                DataSeries dataSeries = new DataSeries();
                dataSeries.SelectionEnabled = true;
                dataSeries.SelectionMode = SelectionModes.Multiple;

                for (Int32 i = 0; i < 6; i++)
                {
                    DataPoint dataPoint = new DataPoint();
                    dataPoint.YValue = rand.Next(100, 500);
                    if (i == 0)
                        dataPoint.Selected = true;
                    dataSeries.DataPoints.Add(dataPoint);

                }
                _chart.Series.Add(dataSeries);
            }

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                _htmlElement1 = Common.GetDisplayMessageButton(_htmlElement1);
                _htmlElement1.SetStyleAttribute("width", "900px");
                _htmlElement1.SetProperty("value", "Click on DataPoints to select.");
                _htmlElement2 = Common.GetDisplayMessageButton(_htmlElement2);
                _htmlElement2.SetStyleAttribute("top", "540px");
                _htmlElement2.SetProperty("value", "Switch 2D/3D.");
                _htmlElement3 = Common.GetDisplayMessageButton(_htmlElement3);
                _htmlElement3.SetStyleAttribute("top", "560px");
                _htmlElement3.SetProperty("value", "Click here to exit.");
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement1);
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement2);
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement3);
            });

            EnqueueCallback(() =>
            {
                _htmlElement2.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.View3D_OnClick));
            });

            EnqueueCallback(() =>
            {
                _htmlElement3.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.Exit_OnClick));
            });

        }
        #endregion

        #region TestingSelectionInBar
        /// <summary>
        /// Testing Selection in Bar Chart
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void TestingSelectionInBar()
        {
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "400px");

            _chart = new Chart();
            _chart.Width = 500;
            _chart.Height = 300;
            _chart.View3D = true;

            _isLoaded = false;
            _chart.Loaded += new RoutedEventHandler(chart_Loaded);

            TestPanel.Children.Add(_chart);

            Random rand = new Random();

            for (Int32 j = 0; j < 2; j++)
            {
                DataSeries dataSeries = new DataSeries();
                dataSeries.RenderAs = RenderAs.Bar;
                dataSeries.SelectionEnabled = true;

                for (Int32 i = 0; i < 6; i++)
                {
                    DataPoint dataPoint = new DataPoint();
                    dataPoint.YValue = rand.Next(100, 500);
                    if (i == 0)
                        dataPoint.Selected = true;
                    dataSeries.DataPoints.Add(dataPoint);

                }
                _chart.Series.Add(dataSeries);
            }

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                _htmlElement1 = Common.GetDisplayMessageButton(_htmlElement1);
                _htmlElement1.SetStyleAttribute("width", "900px");
                _htmlElement1.SetProperty("value", "Click on a DataPoint to select.");
                _htmlElement2 = Common.GetDisplayMessageButton(_htmlElement2);
                _htmlElement2.SetStyleAttribute("top", "540px");
                _htmlElement2.SetProperty("value", "Switch 2D/3D.");
                _htmlElement3 = Common.GetDisplayMessageButton(_htmlElement3);
                _htmlElement3.SetStyleAttribute("top", "560px");
                _htmlElement3.SetProperty("value", "Click here to exit.");
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement1);
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement2);
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement3);
            });

            EnqueueCallback(() =>
            {
                _htmlElement2.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.View3D_OnClick));
            });

            EnqueueCallback(() =>
            {
                _htmlElement3.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.Exit_OnClick));
            });

        }
        #endregion

        #region TestingMultipleSelectionInBar
        /// <summary>
        /// Testing multiple Selection in Bar Chart
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void TestingMultipleSelectionInBar()
        {
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "400px");

            _chart = new Chart();
            _chart.Width = 500;
            _chart.Height = 300;
            _chart.View3D = true;

            _isLoaded = false;
            _chart.Loaded += new RoutedEventHandler(chart_Loaded);

            TestPanel.Children.Add(_chart);

            Random rand = new Random();

            for (Int32 j = 0; j < 2; j++)
            {
                DataSeries dataSeries = new DataSeries();
                dataSeries.RenderAs = RenderAs.Bar;
                dataSeries.SelectionEnabled = true;
                dataSeries.SelectionMode = SelectionModes.Multiple;

                for (Int32 i = 0; i < 6; i++)
                {
                    DataPoint dataPoint = new DataPoint();
                    dataPoint.YValue = rand.Next(100, 500);
                    if (i == 0)
                        dataPoint.Selected = true;
                    dataSeries.DataPoints.Add(dataPoint);

                }
                _chart.Series.Add(dataSeries);
            }

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                _htmlElement1 = Common.GetDisplayMessageButton(_htmlElement1);
                _htmlElement1.SetStyleAttribute("width", "900px");
                _htmlElement1.SetProperty("value", "Click on DataPoints to select.");
                _htmlElement2 = Common.GetDisplayMessageButton(_htmlElement2);
                _htmlElement2.SetStyleAttribute("top", "540px");
                _htmlElement2.SetProperty("value", "Switch 2D/3D.");
                _htmlElement3 = Common.GetDisplayMessageButton(_htmlElement3);
                _htmlElement3.SetStyleAttribute("top", "560px");
                _htmlElement3.SetProperty("value", "Click here to exit.");
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement1);
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement2);
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement3);
            });

            EnqueueCallback(() =>
            {
                _htmlElement2.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.View3D_OnClick));
            });

            EnqueueCallback(() =>
            {
                _htmlElement3.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.Exit_OnClick));
            });

        }
        #endregion

        #region TestingSelectionInLine
        /// <summary>
        /// Testing Selection in Line Chart
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void TestingSelectionInLine()
        {
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "400px");

            _chart = new Chart();
            _chart.Width = 500;
            _chart.Height = 300;
            _chart.View3D = true;

            _isLoaded = false;
            _chart.Loaded += new RoutedEventHandler(chart_Loaded);

            TestPanel.Children.Add(_chart);

            Random rand = new Random();

            for (Int32 j = 0; j < 2; j++)
            {
                DataSeries dataSeries = new DataSeries();
                dataSeries.RenderAs = RenderAs.Line;
                dataSeries.SelectionEnabled = true;

                for (Int32 i = 0; i < 6; i++)
                {
                    DataPoint dataPoint = new DataPoint();
                    dataPoint.YValue = rand.Next(100, 500);
                    if (i == 0)
                        dataPoint.Selected = true;
                    dataSeries.DataPoints.Add(dataPoint);

                }
                _chart.Series.Add(dataSeries);
            }

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                _htmlElement1 = Common.GetDisplayMessageButton(_htmlElement1);
                _htmlElement1.SetStyleAttribute("width", "900px");
                _htmlElement1.SetProperty("value", "Click on a DataPoint to select.");
                _htmlElement2 = Common.GetDisplayMessageButton(_htmlElement2);
                _htmlElement2.SetStyleAttribute("top", "540px");
                _htmlElement2.SetProperty("value", "Switch 2D/3D.");
                _htmlElement3 = Common.GetDisplayMessageButton(_htmlElement3);
                _htmlElement3.SetStyleAttribute("top", "560px");
                _htmlElement3.SetProperty("value", "Click here to exit.");
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement1);
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement2);
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement3);
            });

            EnqueueCallback(() =>
            {
                _htmlElement2.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.View3D_OnClick));
            });

            EnqueueCallback(() =>
            {
                _htmlElement3.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.Exit_OnClick));
            });

        }
        #endregion

        #region TestingMultipleSelectionInLine
        /// <summary>
        /// Testing multiple Selection in Line Chart
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void TestingMultipleSelectionInLine()
        {
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "400px");

            _chart = new Chart();
            _chart.Width = 500;
            _chart.Height = 300;
            _chart.View3D = true;

            _isLoaded = false;
            _chart.Loaded += new RoutedEventHandler(chart_Loaded);

            TestPanel.Children.Add(_chart);

            Random rand = new Random();

            for (Int32 j = 0; j < 2; j++)
            {
                DataSeries dataSeries = new DataSeries();
                dataSeries.RenderAs = RenderAs.Line;
                dataSeries.SelectionEnabled = true;
                dataSeries.SelectionMode = SelectionModes.Multiple;

                for (Int32 i = 0; i < 6; i++)
                {
                    DataPoint dataPoint = new DataPoint();
                    dataPoint.YValue = rand.Next(100, 500);
                    if (i == 0)
                        dataPoint.Selected = true;
                    dataSeries.DataPoints.Add(dataPoint);

                }
                _chart.Series.Add(dataSeries);
            }

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                _htmlElement1 = Common.GetDisplayMessageButton(_htmlElement1);
                _htmlElement1.SetStyleAttribute("width", "900px");
                _htmlElement1.SetProperty("value", "Click on DataPoints to select.");
                _htmlElement2 = Common.GetDisplayMessageButton(_htmlElement2);
                _htmlElement2.SetStyleAttribute("top", "540px");
                _htmlElement2.SetProperty("value", "Switch 2D/3D.");
                _htmlElement3 = Common.GetDisplayMessageButton(_htmlElement3);
                _htmlElement3.SetStyleAttribute("top", "560px");
                _htmlElement3.SetProperty("value", "Click here to exit.");
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement1);
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement2);
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement3);
            });

            EnqueueCallback(() =>
            {
                _htmlElement2.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.View3D_OnClick));
            });

            EnqueueCallback(() =>
            {
                _htmlElement3.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.Exit_OnClick));
            });

        }
        #endregion

        #region TestingSelectionInArea
        /// <summary>
        /// Testing Selection in Area Chart
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void TestingSelectionInArea()
        {
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "400px");

            _chart = new Chart();
            _chart.Width = 500;
            _chart.Height = 300;
            _chart.View3D = true;

            _isLoaded = false;
            _chart.Loaded += new RoutedEventHandler(chart_Loaded);

            TestPanel.Children.Add(_chart);

            Random rand = new Random();

            for (Int32 j = 0; j < 2; j++)
            {
                DataSeries dataSeries = new DataSeries();
                dataSeries.RenderAs = RenderAs.Area;
                dataSeries.MarkerEnabled = true;
                dataSeries.SelectionEnabled = true;

                for (Int32 i = 0; i < 6; i++)
                {
                    DataPoint dataPoint = new DataPoint();
                    dataPoint.YValue = rand.Next(100, 500);
                    if (i == 0)
                        dataPoint.Selected = true;
                    dataSeries.DataPoints.Add(dataPoint);

                }
                _chart.Series.Add(dataSeries);
            }

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                _htmlElement1 = Common.GetDisplayMessageButton(_htmlElement1);
                _htmlElement1.SetStyleAttribute("width", "900px");
                _htmlElement1.SetProperty("value", "Click on a DataPoint to select.");
                _htmlElement2 = Common.GetDisplayMessageButton(_htmlElement2);
                _htmlElement2.SetStyleAttribute("top", "540px");
                _htmlElement2.SetProperty("value", "Switch 2D/3D.");
                _htmlElement3 = Common.GetDisplayMessageButton(_htmlElement3);
                _htmlElement3.SetStyleAttribute("top", "560px");
                _htmlElement3.SetProperty("value", "Click here to exit.");
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement1);
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement2);
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement3);
            });

            EnqueueCallback(() =>
            {
                _htmlElement2.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.View3D_OnClick));
            });

            EnqueueCallback(() =>
            {
                _htmlElement3.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.Exit_OnClick));
            });

        }
        #endregion

        #region TestingMultipleSelectionInArea
        /// <summary>
        /// Testing multiple Selection in Line Chart
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void TestingMultipleSelectionInArea()
        {
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "400px");

            _chart = new Chart();
            _chart.Width = 500;
            _chart.Height = 300;
            _chart.View3D = true;

            _isLoaded = false;
            _chart.Loaded += new RoutedEventHandler(chart_Loaded);

            TestPanel.Children.Add(_chart);

            Random rand = new Random();

            for (Int32 j = 0; j < 2; j++)
            {
                DataSeries dataSeries = new DataSeries();
                dataSeries.RenderAs = RenderAs.Area;
                dataSeries.MarkerEnabled = true;
                dataSeries.SelectionEnabled = true;
                dataSeries.SelectionMode = SelectionModes.Multiple;

                for (Int32 i = 0; i < 6; i++)
                {
                    DataPoint dataPoint = new DataPoint();
                    dataPoint.YValue = rand.Next(100, 500);
                    if (i == 0)
                        dataPoint.Selected = true;
                    dataSeries.DataPoints.Add(dataPoint);

                }
                _chart.Series.Add(dataSeries);
            }

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                _htmlElement1 = Common.GetDisplayMessageButton(_htmlElement1);
                _htmlElement1.SetStyleAttribute("width", "900px");
                _htmlElement1.SetProperty("value", "Click on DataPoints to select.");
                _htmlElement2 = Common.GetDisplayMessageButton(_htmlElement2);
                _htmlElement2.SetStyleAttribute("top", "540px");
                _htmlElement2.SetProperty("value", "Switch 2D/3D.");
                _htmlElement3 = Common.GetDisplayMessageButton(_htmlElement3);
                _htmlElement3.SetStyleAttribute("top", "560px");
                _htmlElement3.SetProperty("value", "Click here to exit.");
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement1);
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement2);
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement3);
            });

            EnqueueCallback(() =>
            {
                _htmlElement2.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.View3D_OnClick));
            });

            EnqueueCallback(() =>
            {
                _htmlElement3.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.Exit_OnClick));
            });

        }
        #endregion

        #region BrokenLineChart
        /// <summary>
        /// Testing broken line chart
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void BrokenLineChartChecking()
        {
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "400px");

            _chart = new Chart();
            _chart.Width = 500;
            _chart.Height = 300;

            _isLoaded = false;
            _chart.Loaded += new RoutedEventHandler(chart_Loaded);

            TestPanel.Children.Add(_chart);

            Random rand = new Random();

            DataSeries dataSeries = new DataSeries();
            dataSeries.RenderAs = RenderAs.Line;

            for (Int32 i = 0; i < 12; i++)
            {
                DataPoint dataPoint = new DataPoint();
                if (i != 2 && i != 3)
                    dataPoint.YValue = rand.Next(10, 100);
                dataSeries.DataPoints.Add(dataPoint);

            }
            _chart.Series.Add(dataSeries);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                _htmlElement1 = Common.GetDisplayMessageButton(_htmlElement1);
                _htmlElement1.SetStyleAttribute("width", "900px");
                _htmlElement1.SetProperty("value", "Number of Render Count: " + _chart.ChartArea._renderCount + ". Click here to exit.");
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement1);
            });

            EnqueueCallback(() =>
            {
                _htmlElement1.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.Exit_OnClick));
            });
        }
        #endregion

        #region CheckConnectedLineChart4DisabledDataPoints
        /// <summary>
        /// Testing connected line chart if few DataPoints are disabled
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckConnectedLineChart4DisabledDataPoints()
        {
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "400px");

            _chart = new Chart();
            _chart.Width = 500;
            _chart.Height = 300;

            _isLoaded = false;
            _chart.Loaded += new RoutedEventHandler(chart_Loaded);

            TestPanel.Children.Add(_chart);

            Random rand = new Random();

            DataSeries dataSeries = new DataSeries();
            dataSeries.RenderAs = RenderAs.Line;

            for (Int32 i = 0; i < 12; i++)
            {
                DataPoint dataPoint = new DataPoint();
                if (i == 2 || i == 3)
                    dataPoint.Enabled = false;
                dataPoint.YValue = rand.Next(10, 100);
                dataSeries.DataPoints.Add(dataPoint);

            }
            _chart.Series.Add(dataSeries);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                try
                {
                    _htmlElement1 = Common.GetDisplayMessageButton(_htmlElement1);
                    _htmlElement1.SetStyleAttribute("width", "900px");
                    _htmlElement1.SetProperty("value", "Number of Render Count: " + _chart.ChartArea._renderCount + ". Click here to exit.");
                    System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement1);
                }
                catch { }
            });

            EnqueueCallback(() =>
            {
                _htmlElement1.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.Exit_OnClick));
            });
        }
        #endregion

        #region CheckPieChartLegend4DisabledDataSeriesAndEnabledDataPoint
        /// <summary>
        /// Testing pie chart legend if DataSeries is disabled and few DataPoints are enabled
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckPieChartLegend4DisabledDataSeriesAndEnabledDataPoint()
        {
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "400px");

            _chart = new Chart();
            _chart.Width = 500;
            _chart.Height = 300;

            _isLoaded = false;
            _chart.Loaded += new RoutedEventHandler(chart_Loaded);

            TestPanel.Children.Add(_chart);

            Random rand = new Random();

            DataSeries dataSeries = new DataSeries();
            dataSeries.RenderAs = RenderAs.Pie;
            dataSeries.Enabled = false;

            for (Int32 i = 0; i < 12; i++)
            {
                DataPoint dataPoint = new DataPoint();
                if (i == 1 || i == 2)
                    dataPoint.Enabled = true;
                dataPoint.YValue = rand.Next(10, 100);
                dataSeries.DataPoints.Add(dataPoint);

            }
            _chart.Series.Add(dataSeries);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                _htmlElement1 = Common.GetDisplayMessageButton(_htmlElement1);
                _htmlElement1.SetStyleAttribute("width", "900px");
                _htmlElement1.SetProperty("value", "Number of Render Count: " + _chart.ChartArea._renderCount + ". Click here to exit.");
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement1);
            });

            EnqueueCallback(() =>
            {
                _htmlElement1.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.Exit_OnClick));
            });
        }
        #endregion

        #region DoughnutStressChecking
        /// <summary>
        /// Stress testing Doughnut Chart
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void DoughnutStressChecking()
        {
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "400px");

            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;
            chart.View3D = true;
            chart.SmartLabelEnabled = true;

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            TestPanel.Children.Add(chart);

            Random rand = new Random();

            DataSeries dataSeries = new DataSeries();
            dataSeries.RenderAs = RenderAs.Doughnut;
            dataSeries.ShowInLegend = false;

            Int32 numberOfDataPoints = 0;
            Double totalDuration = 0;
            DateTime start = DateTime.UtcNow;
            String msg = Common.AssertAverageDuration(200, 1,
                delegate
                {
                    for (Int32 i = 0; i < 500; i++)
                    {
                        DataPoint dataPoint = new DataPoint();
                        dataPoint.XValue = i + 1;
                        dataPoint.YValue = rand.Next(-500, 500);
                        dataSeries.DataPoints.Add(dataPoint);
                        numberOfDataPoints++;
                    }
                    chart.Series.Add(dataSeries);
                });

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
                _htmlElement1.SetProperty("value", dataSeries.RenderAs + " chart with " + numberOfDataPoints + " DataPoints. Total Chart Loading Time: " + totalDuration + "s. Number of Render Count: " + chart.ChartArea._renderCount);
                _htmlElement2 = Common.GetDisplayMessageButton(_htmlElement2);
                _htmlElement2.SetStyleAttribute("top", "540px");
                _htmlElement2.SetProperty("value", "Total Series Calculation " + msg + " Click here to exit");
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement1);
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement2);
            });

            EnqueueCallback(() =>
            {
                _htmlElement2.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.HtmlElement_OnClick));
            });
        }
        #endregion

        #region DataSeriesBarChecking
        /// <summary>
        /// Testing DataSeries Bar Chart
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void DataSeriesBarChecking()
        {
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "450px");

            Chart chart = new Chart();
            chart.Width = 350;
            chart.Height = 450;
            chart.View3D = true;

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            TestPanel.Children.Add(chart);

            Axis axisX = new Axis();
            axisX.Interval = 1;
            chart.AxesX.Add(axisX);

            Random rand = new Random();

            DataSeries dataSeries = new DataSeries();
            dataSeries.RenderAs = RenderAs.Bar;

            Int32 numberOfDataPoints = 0;
            Double totalDuration = 0;
            DateTime start = DateTime.UtcNow;
            String msg = Common.AssertAverageDuration(200, 1,
                delegate
                {
                    for (Int32 i = 0; i < 500; i++)
                    {
                        DataPoint dataPoint = new DataPoint();
                        dataPoint.XValue = i + 1;
                        dataPoint.YValue = rand.Next(-500, 500);
                        dataSeries.DataPoints.Add(dataPoint);
                        numberOfDataPoints++;
                    }
                    chart.Series.Add(dataSeries);
                });

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
                _htmlElement1.SetProperty("value", dataSeries.RenderAs + " chart with " + numberOfDataPoints + " DataPoints. Total Chart Loading Time: " + totalDuration + "s. Number of Render Count: " + chart.ChartArea._renderCount);
                _htmlElement2 = Common.GetDisplayMessageButton(_htmlElement2);
                _htmlElement2.SetStyleAttribute("top", "540px");
                _htmlElement2.SetProperty("value", "Total Series Calculation " + msg + " Click here to exit");
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement1);
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement2);
            });

            EnqueueCallback(() =>
            {
                _htmlElement2.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.HtmlElement_OnClick));
            });
        }
        #endregion

        #region MultiSeriesWithNoDataPoints
        /// <summary>
        /// Testing second series with no DataPoints
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void MultiSeriesWithNoDataPointsChecking()
        {
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "450px");

            Chart chart = new Chart();
            chart.Width = 350;
            chart.Height = 450;

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            TestPanel.Children.Add(chart);

            Random rand = new Random();

            EnqueueConditional(() => { return _isLoaded; });

            EnqueueCallback(() =>
            {
                DataSeries dataSeries;
                for (Int32 ds = 0; ds < 2; ds++)
                {
                    dataSeries = new DataSeries();
                    if (ds == 0)
                    {
                        for (Int32 dp = 0; dp < 5; dp++)
                        {
                            DataPoint dataPoint = new DataPoint();
                            dataPoint.XValue = dp + 1;
                            dataPoint.YValue = rand.Next(0, 100);
                            dataSeries.DataPoints.Add(dataPoint);
                        }
                    }

                    chart.Series.Add(dataSeries);
                }
            });

            EnqueueDelay(_sleepTime);
            EnqueueTestComplete();
        }
        #endregion

        #region LineSeriesWithNoDataPoints
        /// <summary>
        /// Testing second series (Line) with no DataPoints
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void LineSeriesWithNoDataPoints()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            TestPanel.Children.Add(chart);

            Random rand = new Random();

            EnqueueConditional(() => { return _isLoaded; });

            EnqueueCallback(() =>
            {
                DataSeries dataSeries;
                for (Int32 ds = 0; ds < 2; ds++)
                {
                    dataSeries = new DataSeries();
                    if (ds == 0)
                    {
                        for (Int32 dp = 0; dp < 5; dp++)
                        {
                            DataPoint dataPoint = new DataPoint();
                            dataPoint.XValue = dp + 1;
                            dataPoint.YValue = rand.Next(0, 100);
                            dataSeries.DataPoints.Add(dataPoint);
                        }
                    }
                    else
                        dataSeries.RenderAs = RenderAs.Line;

                    chart.Series.Add(dataSeries);
                }
            });

            EnqueueDelay(_sleepTime);
            EnqueueTestComplete();
        }
        #endregion

        #region DataSeriesEventChecking
        /// <summary>
        /// Testing DataSeries Event
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void DataSeriesEventChecking()
        {
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "400px");

            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;
            chart.View3D = false;

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Random rand = new Random();

            DataSeries dataSeries = new DataSeries();
            dataSeries.RenderAs = RenderAs.Column;

            for (Int32 i = 0; i < 20; i++)
            {
                DataPoint dataPoint = new DataPoint();
                dataPoint.XValue = i + 1;
                dataPoint.YValue = rand.Next(-500, 500);
                dataSeries.DataPoints.Add(dataPoint);

            }
            chart.Series.Add(dataSeries);

            TestPanel.Children.Add(chart);

            EnqueueConditional(() => { return _isLoaded; });

            dataSeries.MouseLeftButtonUp += delegate(Object sender, MouseButtonEventArgs e)
            {
                _htmlElement1.SetProperty("value", "DataSeries RenderAs: " + (sender as DataPoint).Parent.RenderAs + "MouseLeftButtonUp event fired");
            };

            dataSeries.MouseEnter += delegate(Object sender, MouseEventArgs e)
            {
                _htmlElement1.SetProperty("value", "DataSeries RenderAs: " + (sender as DataPoint).Parent.RenderAs + " MouseEnter event fired");
            };

            dataSeries.MouseLeave += delegate(Object sender, MouseEventArgs e)
            {
                _htmlElement1.SetProperty("value", "Click here to exit. MouseLeave event fired");
            };

            dataSeries.MouseLeftButtonDown += delegate(Object sender, MouseButtonEventArgs e)
            {
                _htmlElement1.SetProperty("value", "DataSeries RenderAs: " + (sender as DataPoint).Parent.RenderAs + " MouseLeftButtonDown event fired");
            };

            EnqueueCallback(() =>
            {
                _htmlElement1.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.HtmlElement_OnClick));
            });

            _htmlElement1 = Common.GetDisplayMessageButton(_htmlElement1);
            _htmlElement1.SetStyleAttribute("width", "900px");
            _htmlElement1.SetProperty("value", "Mouse over the DataPoints");
            System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement1);
        }
        #endregion

        #region MultiSeriesChartChecking1
        /// <summary>
        /// Testing Multi DataSeries Chart
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void MultiSeriesChartChecking1()
        {
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "400px");

            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;
            chart.View3D = true;


            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            TestPanel.Children.Add(chart);

            Int32 numberOfDataPoints = 0;

            Random rand = new Random();

            _htmlElement1 = Common.GetDisplayMessageButton(_htmlElement1);
            _htmlElement1.SetStyleAttribute("width", "900px");
            System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement1);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                DataSeries dataSeries1 = new DataSeries();
                dataSeries1.RenderAs = RenderAs.Column;
                numberOfDataPoints = 0;
                for (Int32 i = 0; i < 20; i++)
                {
                    DataPoint dataPoint = new DataPoint();
                    dataPoint.XValue = i + 1;
                    dataPoint.YValue = rand.Next(-500, 500);
                    dataSeries1.DataPoints.Add(dataPoint);
                    numberOfDataPoints++;
                }
                chart.Series.Add(dataSeries1);
                Assert.AreEqual(1, chart.Series.Count);
                isDataSeriesAdded = true;
                _htmlElement1.SetProperty("value", dataSeries1.RenderAs + " with " + numberOfDataPoints + " DataPoints. Number of Render Count: " + chart.ChartArea._renderCount);
            });

            EnqueueConditional(() => { return isDataSeriesAdded; });
            EnqueueDelay(_sleepTime);
            isDataSeriesAdded = false;

            EnqueueCallback(() =>
            {
                DataSeries dataSeries2 = new DataSeries();
                dataSeries2.RenderAs = RenderAs.Column;
                numberOfDataPoints = 0;
                for (Int32 i = 0; i < 20; i++)
                {
                    DataPoint dataPoint = new DataPoint();
                    dataPoint.XValue = i + 1;
                    dataPoint.YValue = rand.Next(50, 500);
                    dataSeries2.DataPoints.Add(dataPoint);
                    numberOfDataPoints++;
                }
                chart.Series.Add(dataSeries2);
                Assert.AreEqual(2, chart.Series.Count);
                isDataSeriesAdded = true;
                _htmlElement1.SetProperty("value", dataSeries2.RenderAs + " with " + numberOfDataPoints + " DataPoints. Number of Render Count: " + chart.ChartArea._renderCount);
            });

            EnqueueConditional(() => { return isDataSeriesAdded; });
            EnqueueDelay(_sleepTime);
            isDataSeriesAdded = false;

            EnqueueCallback(() =>
            {
                DataSeries dataSeries3 = new DataSeries();
                dataSeries3.RenderAs = RenderAs.Line;
                numberOfDataPoints = 0;
                for (Int32 i = 0; i < 20; i++)
                {
                    DataPoint dataPoint = new DataPoint();
                    dataPoint.XValue = i + 1;
                    dataPoint.YValue = rand.Next(0, 500);
                    dataSeries3.DataPoints.Add(dataPoint);
                    numberOfDataPoints++;
                }
                chart.Series.Add(dataSeries3);
                Assert.AreEqual(3, chart.Series.Count);
                isDataSeriesAdded = true;
                _htmlElement1.SetProperty("value", dataSeries3.RenderAs + " with " + numberOfDataPoints + " DataPoints. Number of Render Count: " + chart.ChartArea._renderCount);
            });

            EnqueueConditional(() => { return isDataSeriesAdded; });
            EnqueueDelay(_sleepTime);
            isDataSeriesAdded = false;

            EnqueueCallback(() =>
            {
                DataSeries dataSeries4 = new DataSeries();
                dataSeries4.RenderAs = RenderAs.Area;
                numberOfDataPoints = 0;
                for (Int32 i = 0; i < 20; i++)
                {
                    DataPoint dataPoint = new DataPoint();
                    dataPoint.XValue = i + 1;
                    dataPoint.YValue = rand.Next(100, 500);
                    dataSeries4.DataPoints.Add(dataPoint);
                    numberOfDataPoints++;
                }
                chart.Series.Add(dataSeries4);
                Assert.AreEqual(4, chart.Series.Count);
                isDataSeriesAdded = true;
                _htmlElement1.SetProperty("value", dataSeries4.RenderAs + " with " + numberOfDataPoints + " DataPoints. Number of Render Count: " + chart.ChartArea._renderCount);
            });

            EnqueueConditional(() => { return isDataSeriesAdded; });
            EnqueueDelay(_sleepTime);
            isDataSeriesAdded = false;

            EnqueueCallback(() =>
            {
                DataSeries dataSeries5 = new DataSeries();
                dataSeries5.RenderAs = RenderAs.Bubble;
                numberOfDataPoints = 0;
                for (Int32 i = 0; i < 20; i++)
                {
                    DataPoint dataPoint = new DataPoint();
                    dataPoint.XValue = i + 1;
                    dataPoint.YValue = rand.Next(-100, 400);
                    dataPoint.ZValue = rand.Next(100, 500);
                    dataSeries5.DataPoints.Add(dataPoint);
                    numberOfDataPoints++;
                }
                chart.Series.Add(dataSeries5);
                Assert.AreEqual(5, chart.Series.Count);
                isDataSeriesAdded = true;
                _htmlElement1.SetProperty("value", dataSeries5.RenderAs + " with " + numberOfDataPoints + " DataPoints. Number of Render Count: " + chart.ChartArea._renderCount);
            });

            EnqueueConditional(() => { return isDataSeriesAdded; });
            EnqueueDelay(_sleepTime);
            isDataSeriesAdded = false;

            EnqueueCallback(() =>
            {
                DataSeries dataSeries6 = new DataSeries();
                dataSeries6.RenderAs = RenderAs.Area;
                numberOfDataPoints = 0;
                for (Int32 i = 0; i < 20; i++)
                {
                    DataPoint dataPoint = new DataPoint();
                    dataPoint.XValue = i + 1;
                    dataPoint.YValue = rand.Next(-200, 500);
                    dataPoint.ZValue = rand.Next(100, 500);
                    dataSeries6.DataPoints.Add(dataPoint);
                    numberOfDataPoints++;
                }
                chart.Series.Add(dataSeries6);
                Assert.AreEqual(6, chart.Series.Count);
                isDataSeriesAdded = true;
                _htmlElement1.SetProperty("value", dataSeries6.RenderAs + " with " + numberOfDataPoints + " DataPoints. Click here to exit. Number of Render Count: " + chart.ChartArea._renderCount);
            });

            EnqueueConditional(() => { return isDataSeriesAdded; });
            isDataSeriesAdded = false;

            EnqueueCallback(() =>
            {
                EnqueueTestComplete();
            });
        }
        #endregion

        #region MultiSeriesChartChecking2
        /// <summary>
        /// Testing Multi DataSeries Chart
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void MultiSeriesChartChecking2()
        {
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "400px");

            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;
            chart.View3D = false;

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            TestPanel.Children.Add(chart);

            Int32 numberOfDataPoints = 0;

            Random rand = new Random();

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                DataSeries dataSeries1 = new DataSeries();
                dataSeries1.RenderAs = RenderAs.Area;
                numberOfDataPoints = 0;
                for (Int32 i = 0; i < 20; i++)
                {
                    DataPoint dataPoint = new DataPoint();
                    dataPoint.XValue = i + 1;
                    dataPoint.YValue = rand.Next(-500, 500);
                    dataSeries1.DataPoints.Add(dataPoint);
                    numberOfDataPoints++;
                }
                chart.Series.Add(dataSeries1);
                Assert.AreEqual(1, chart.Series.Count);
                isDataSeriesAdded = true;
                _htmlElement1.SetProperty("value", dataSeries1.RenderAs + " with " + numberOfDataPoints + " DataPoints. Number of Render Count: " + chart.ChartArea._renderCount);
            });

            EnqueueConditional(() => { return isDataSeriesAdded; });
            EnqueueDelay(_sleepTime);
            isDataSeriesAdded = false;

            EnqueueCallback(() =>
            {
                DataSeries dataSeries2 = new DataSeries();
                dataSeries2.RenderAs = RenderAs.Line;
                numberOfDataPoints = 0;
                for (Int32 i = 0; i < 20; i++)
                {
                    DataPoint dataPoint = new DataPoint();
                    dataPoint.XValue = i + 1;
                    dataPoint.YValue = rand.Next(-500, 500);
                    dataSeries2.DataPoints.Add(dataPoint);
                    numberOfDataPoints++;
                }
                chart.Series.Add(dataSeries2);
                Assert.AreEqual(2, chart.Series.Count);
                isDataSeriesAdded = true;
                _htmlElement1.SetProperty("value", dataSeries2.RenderAs + " with " + numberOfDataPoints + " DataPoints. Number of Render Count: " + chart.ChartArea._renderCount);
            });

            EnqueueConditional(() => { return isDataSeriesAdded; });
            EnqueueDelay(_sleepTime);
            isDataSeriesAdded = false;

            EnqueueCallback(() =>
            {
                DataSeries dataSeries3 = new DataSeries();
                dataSeries3.RenderAs = RenderAs.StackedArea;
                numberOfDataPoints = 0;
                for (Int32 i = 0; i < 20; i++)
                {
                    DataPoint dataPoint = new DataPoint();
                    dataPoint.XValue = i + 1;
                    dataPoint.YValue = rand.Next(0, 500);
                    dataSeries3.DataPoints.Add(dataPoint);
                    numberOfDataPoints++;
                }
                chart.Series.Add(dataSeries3);
                Assert.AreEqual(3, chart.Series.Count);
                isDataSeriesAdded = true;
                _htmlElement1.SetProperty("value", dataSeries3.RenderAs + " with " + numberOfDataPoints + " DataPoints. Number of Render Count: " + chart.ChartArea._renderCount);
            });

            EnqueueConditional(() => { return isDataSeriesAdded; });
            EnqueueDelay(_sleepTime);
            isDataSeriesAdded = false;

            EnqueueCallback(() =>
            {
                DataSeries dataSeries4 = new DataSeries();
                dataSeries4.RenderAs = RenderAs.StackedArea;
                numberOfDataPoints = 0;
                for (Int32 i = 0; i < 20; i++)
                {
                    DataPoint dataPoint = new DataPoint();
                    dataPoint.XValue = i + 1;
                    dataPoint.YValue = rand.Next(0, 500);
                    dataSeries4.DataPoints.Add(dataPoint);
                    numberOfDataPoints++;
                }
                chart.Series.Add(dataSeries4);
                Assert.AreEqual(4, chart.Series.Count);
                isDataSeriesAdded = true;
                _htmlElement1.SetProperty("value", dataSeries4.RenderAs + " with " + numberOfDataPoints + " DataPoints. Number of Render Count: " + chart.ChartArea._renderCount);
            });

            EnqueueConditional(() => { return isDataSeriesAdded; });
            EnqueueDelay(_sleepTime);
            isDataSeriesAdded = false;

            EnqueueCallback(() =>
            {
                DataSeries dataSeries5 = new DataSeries();
                dataSeries5.RenderAs = RenderAs.StackedColumn;
                numberOfDataPoints = 0;
                for (Int32 i = 0; i < 20; i++)
                {
                    DataPoint dataPoint = new DataPoint();
                    dataPoint.XValue = i + 1;
                    dataPoint.YValue = rand.Next(100, 500);
                    dataSeries5.DataPoints.Add(dataPoint);
                    numberOfDataPoints++;
                }
                chart.Series.Add(dataSeries5);
                Assert.AreEqual(5, chart.Series.Count);
                isDataSeriesAdded = true;
                _htmlElement1.SetProperty("value", dataSeries5.RenderAs + " with " + numberOfDataPoints + " DataPoints. Number of Render Count: " + chart.ChartArea._renderCount);
            });

            EnqueueConditional(() => { return isDataSeriesAdded; });
            EnqueueDelay(_sleepTime);
            isDataSeriesAdded = false;

            EnqueueCallback(() =>
            {
                DataSeries dataSeries6 = new DataSeries();
                dataSeries6.RenderAs = RenderAs.Area;
                numberOfDataPoints = 0;
                for (Int32 i = 0; i < 20; i++)
                {
                    DataPoint dataPoint = new DataPoint();
                    dataPoint.XValue = i + 1;
                    dataPoint.YValue = rand.Next(-500, 500);
                    dataPoint.ZValue = rand.Next(100, 500);
                    dataSeries6.DataPoints.Add(dataPoint);
                    numberOfDataPoints++;
                }
                chart.Series.Add(dataSeries6);
                Assert.AreEqual(6, chart.Series.Count);
                isDataSeriesAdded = true;
                _htmlElement1.SetProperty("value", dataSeries6.RenderAs + " with " + numberOfDataPoints + " DataPoints. Number of Render Count: " + chart.ChartArea._renderCount);
            });

            EnqueueConditional(() => { return isDataSeriesAdded; });
            EnqueueDelay(_sleepTime);
            isDataSeriesAdded = false;

            EnqueueCallback(() =>
            {
                DataSeries dataSeries7 = new DataSeries();
                dataSeries7.RenderAs = RenderAs.Line;
                numberOfDataPoints = 0;
                for (Int32 i = 0; i < 20; i++)
                {
                    DataPoint dataPoint = new DataPoint();
                    dataPoint.XValue = i + 1;
                    dataPoint.YValue = rand.Next(-500, 500);
                    dataSeries7.DataPoints.Add(dataPoint);
                    numberOfDataPoints++;
                }
                chart.Series.Add(dataSeries7);
                Assert.AreEqual(7, chart.Series.Count);
                isDataSeriesAdded = true;
                _htmlElement1.SetProperty("value", dataSeries7.RenderAs + " with " + numberOfDataPoints + " DataPoints. Number of Render Count: " + chart.ChartArea._renderCount);
            });

            EnqueueConditional(() => { return isDataSeriesAdded; });
            EnqueueDelay(_sleepTime);
            isDataSeriesAdded = false;

            EnqueueCallback(() =>
            {
                DataSeries dataSeries8 = new DataSeries();
                dataSeries8.RenderAs = RenderAs.StackedColumn100;
                numberOfDataPoints = 0;
                for (Int32 i = 0; i < 20; i++)
                {
                    DataPoint dataPoint = new DataPoint();
                    dataPoint.XValue = i + 1;
                    dataPoint.YValue = rand.Next(0, 500);
                    dataSeries8.DataPoints.Add(dataPoint);
                    numberOfDataPoints++;
                }
                chart.Series.Add(dataSeries8);
                Assert.AreEqual(8, chart.Series.Count);
                isDataSeriesAdded = true;
                _htmlElement1.SetProperty("value", dataSeries8.RenderAs + " with " + numberOfDataPoints + " DataPoints. Number of Render Count: " + chart.ChartArea._renderCount);
            });

            EnqueueConditional(() => { return isDataSeriesAdded; });
            isDataSeriesAdded = false;

            EnqueueCallback(() =>
            {
                EnqueueTestComplete();
            });

            _htmlElement1 = Common.GetDisplayMessageButton(_htmlElement1);
            _htmlElement1.SetStyleAttribute("width", "900px");
            System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement1);
        }
        #endregion

        #region MultiSeriesChartCheckingWithSecondaryAxis
        /// <summary>
        /// Testing Multi DataSeries Chart with Secondary axis
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void MultiSeriesChartCheckingWithSecondaryAxis()
        {
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "400px");

            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;
            chart.View3D = true;

            chart.ScrollingEnabled = false;

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            TestPanel.Children.Add(chart);

            Int32 numberOfDataPoints = 0;

            Random rand = new Random();

            Axis axis = new Axis();
            axis.Interval = 1;
            axis.Background = new SolidColorBrush(Colors.LightGray);
            chart.AxesX.Add(axis);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueDelay(1000);

            EnqueueCallback(() =>
            {
                DataSeries dataSeries1 = new DataSeries();
                dataSeries1.RenderAs = RenderAs.Area;
                numberOfDataPoints = 0;
                for (Int32 i = 0; i < 20; i++)
                {
                    DataPoint dataPoint = new DataPoint();
                    dataPoint.XValue = i + 1;
                    dataPoint.YValue = rand.Next(-500, 500);
                    dataSeries1.DataPoints.Add(dataPoint);
                    numberOfDataPoints++;
                }
                chart.Series.Add(dataSeries1);
                Assert.AreEqual(1, chart.Series.Count);
                _htmlElement1.SetProperty("value", dataSeries1.RenderAs + " with " + numberOfDataPoints + " DataPoints. Number of Render Count: " + chart.ChartArea._renderCount);
            });

            EnqueueCallback(() =>
            {
                DataSeries dataSeries2 = new DataSeries();
                dataSeries2.RenderAs = RenderAs.Line;
                numberOfDataPoints = 0;
                for (Int32 i = 0; i < 20; i++)
                {
                    DataPoint dataPoint = new DataPoint();
                    dataPoint.XValue = i + 1;
                    dataPoint.YValue = rand.Next(-500, 500);
                    dataSeries2.DataPoints.Add(dataPoint);
                    numberOfDataPoints++;
                }
                chart.Series.Add(dataSeries2);
                Assert.AreEqual(2, chart.Series.Count);
                isDataSeriesAdded = true;
                _htmlElement1.SetProperty("value", dataSeries2.RenderAs + " with " + numberOfDataPoints + " DataPoints. Number of Render Count: " + chart.ChartArea._renderCount);
            });

            EnqueueConditional(() => { return isDataSeriesAdded; });
            EnqueueDelay(1000);
            isDataSeriesAdded = false;

            EnqueueCallback(() =>
            {
                DataSeries dataSeries3 = new DataSeries();
                dataSeries3.RenderAs = RenderAs.StackedArea;
                dataSeries3.AxisYType = AxisTypes.Secondary;
                numberOfDataPoints = 0;
                for (Int32 i = 0; i < 20; i++)
                {
                    DataPoint dataPoint = new DataPoint();
                    dataPoint.XValue = i + 1;
                    dataPoint.YValue = rand.Next(100, 500);
                    dataSeries3.DataPoints.Add(dataPoint);
                    numberOfDataPoints++;
                }
                chart.Series.Add(dataSeries3);
                Assert.AreEqual(3, chart.Series.Count);
                isDataSeriesAdded = true;
                _htmlElement1.SetProperty("value", dataSeries3.RenderAs + " with " + numberOfDataPoints + " DataPoints. Number of Render Count: " + chart.ChartArea._renderCount);
            });

            EnqueueConditional(() => { return isDataSeriesAdded; });
            EnqueueDelay(1000);
            isDataSeriesAdded = false;

            EnqueueCallback(() =>
            {
                DataSeries dataSeries4 = new DataSeries();
                dataSeries4.RenderAs = RenderAs.StackedArea;
                dataSeries4.AxisYType = AxisTypes.Secondary;
                numberOfDataPoints = 0;
                for (Int32 i = 0; i < 20; i++)
                {
                    DataPoint dataPoint = new DataPoint();
                    dataPoint.XValue = i + 1;
                    dataPoint.YValue = rand.Next(-100, 400);
                    dataSeries4.DataPoints.Add(dataPoint);
                    numberOfDataPoints++;
                }
                chart.Series.Add(dataSeries4);
                Assert.AreEqual(4, chart.Series.Count);
                isDataSeriesAdded = true;
                _htmlElement1.SetProperty("value", dataSeries4.RenderAs + " with " + numberOfDataPoints + " DataPoints. Number of Render Count: " + chart.ChartArea._renderCount);
            });

            EnqueueConditional(() => { return isDataSeriesAdded; });
            EnqueueDelay(1000);
            isDataSeriesAdded = false;

            EnqueueCallback(() =>
            {
                DataSeries dataSeries5 = new DataSeries();
                dataSeries5.RenderAs = RenderAs.StackedColumn;
                numberOfDataPoints = 0;
                for (Int32 i = 0; i < 20; i++)
                {
                    DataPoint dataPoint = new DataPoint();
                    dataPoint.XValue = i + 1;
                    dataPoint.YValue = rand.Next(-200, 400);
                    dataSeries5.DataPoints.Add(dataPoint);
                    numberOfDataPoints++;
                }
                chart.Series.Add(dataSeries5);
                Assert.AreEqual(5, chart.Series.Count);
                isDataSeriesAdded = true;
                _htmlElement1.SetProperty("value", dataSeries5.RenderAs + " with " + numberOfDataPoints + " DataPoints. Number of Render Count: " + chart.ChartArea._renderCount);
            });

            EnqueueConditional(() => { return isDataSeriesAdded; });
            EnqueueDelay(1000);
            isDataSeriesAdded = false;

            EnqueueCallback(() =>
            {
                DataSeries dataSeries6 = new DataSeries();
                dataSeries6.RenderAs = RenderAs.Area;
                dataSeries6.AxisYType = AxisTypes.Secondary;
                numberOfDataPoints = 0;
                for (Int32 i = 0; i < 20; i++)
                {
                    DataPoint dataPoint = new DataPoint();
                    dataPoint.XValue = i + 1;
                    dataPoint.YValue = rand.Next(-100, 500);
                    dataPoint.ZValue = rand.Next(100, 500);
                    dataSeries6.DataPoints.Add(dataPoint);
                    numberOfDataPoints++;
                }
                chart.Series.Add(dataSeries6);
                Assert.AreEqual(6, chart.Series.Count);
                isDataSeriesAdded = true;
                _htmlElement1.SetProperty("value", dataSeries6.RenderAs + " with " + numberOfDataPoints + " DataPoints. Number of Render Count: " + chart.ChartArea._renderCount);
            });

            EnqueueConditional(() => { return isDataSeriesAdded; });
            EnqueueDelay(1000);
            isDataSeriesAdded = false;

            EnqueueCallback(() =>
            {
                DataSeries dataSeries7 = new DataSeries();
                dataSeries7.RenderAs = RenderAs.Line;
                numberOfDataPoints = 0;
                for (Int32 i = 0; i < 20; i++)
                {
                    DataPoint dataPoint = new DataPoint();
                    dataPoint.XValue = i + 1;
                    dataPoint.YValue = rand.Next(-400, 500);
                    dataSeries7.DataPoints.Add(dataPoint);
                    numberOfDataPoints++;
                }
                chart.Series.Add(dataSeries7);
                Assert.AreEqual(7, chart.Series.Count);
                isDataSeriesAdded = true;
                _htmlElement1.SetProperty("value", dataSeries7.RenderAs + " with " + numberOfDataPoints + " DataPoints. Number of Render Count: " + chart.ChartArea._renderCount);
            });

            EnqueueConditional(() => { return isDataSeriesAdded; });
            EnqueueDelay(1000);
            isDataSeriesAdded = false;

            EnqueueCallback(() =>
            {
                DataSeries dataSeries8 = new DataSeries();
                dataSeries8.RenderAs = RenderAs.StackedColumn100;
                dataSeries8.AxisYType = AxisTypes.Secondary;
                numberOfDataPoints = 0;
                for (Int32 i = 0; i < 20; i++)
                {
                    DataPoint dataPoint = new DataPoint();
                    dataPoint.XValue = i + 1;
                    dataPoint.YValue = rand.Next(0, 500);
                    dataSeries8.DataPoints.Add(dataPoint);
                    numberOfDataPoints++;
                }
                chart.Series.Add(dataSeries8);
                Assert.AreEqual(8, chart.Series.Count);
                isDataSeriesAdded = true;
                _htmlElement1.SetProperty("value", dataSeries8.RenderAs + " with " + numberOfDataPoints + " DataPoints. Click here to exit. Number of Render Count: " + chart.ChartArea._renderCount);
            });

            EnqueueConditional(() => { return isDataSeriesAdded; });
            isDataSeriesAdded = false;

            EnqueueCallback(() =>
            {
                EnqueueTestComplete();
            });

            _htmlElement1 = Common.GetDisplayMessageButton(_htmlElement1);
            _htmlElement1.SetStyleAttribute("width", "900px");
            System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement1);
        }
        #endregion

        #region PieDoughnutChartChecking
        /// <summary>
        /// Testing Pie/Doughnut Chart loading
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void PieDoughnutChartChecking()
        {
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "400px");

            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;
            chart.View3D = true;


            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            TestPanel.Children.Add(chart);

            Int32 numberOfDataPoints = 0;

            Random rand = new Random();

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                DataSeries dataSeries1 = new DataSeries();
                dataSeries1.RenderAs = RenderAs.Pie;
                numberOfDataPoints = 0;
                for (Int32 i = 0; i < 20; i++)
                {
                    DataPoint dataPoint = new DataPoint();
                    dataPoint.XValue = i + 1;
                    dataPoint.YValue = rand.Next(100, 500);
                    dataSeries1.DataPoints.Add(dataPoint);
                    numberOfDataPoints++;
                }
                chart.Series.Add(dataSeries1);
                Assert.AreEqual(1, chart.Series.Count);
                _htmlElement1.SetProperty("value", dataSeries1.RenderAs + " with " + numberOfDataPoints + " DataPoints. Number of Render Count: " + chart.ChartArea._renderCount);
            });

            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                chart.Series.Clear();
            });

            EnqueueCallback(() =>
            {
                DataSeries dataSeries2 = new DataSeries();
                dataSeries2.RenderAs = RenderAs.Doughnut;
                numberOfDataPoints = 0;
                for (Int32 i = 0; i < 20; i++)
                {
                    DataPoint dataPoint = new DataPoint();
                    dataPoint.XValue = i + 1;
                    dataPoint.YValue = rand.Next(100, 500);
                    dataSeries2.DataPoints.Add(dataPoint);
                    numberOfDataPoints++;
                }
                chart.Series.Add(dataSeries2);
                Assert.AreEqual(1, chart.Series.Count);
                _htmlElement1.SetProperty("value", dataSeries2.RenderAs + " with " + numberOfDataPoints + " DataPoints. Number of Render Count: " + chart.ChartArea._renderCount);
            });

            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                chart.Series.Clear();
            });

            EnqueueCallback(() =>
            {
                DataSeries dataSeries3 = new DataSeries();
                dataSeries3.RenderAs = RenderAs.Pie;
                numberOfDataPoints = 0;
                for (Int32 i = 0; i < 30; i++)
                {
                    DataPoint dataPoint = new DataPoint();
                    dataPoint.XValue = i + 1;
                    dataPoint.YValue = rand.Next(100, 500);
                    dataSeries3.DataPoints.Add(dataPoint);
                    numberOfDataPoints++;
                }
                chart.Series.Add(dataSeries3);
                Assert.AreEqual(1, chart.Series.Count);
                _htmlElement1.SetProperty("value", dataSeries3.RenderAs + " with " + numberOfDataPoints + " DataPoints. Number of Render Count: " + chart.ChartArea._renderCount);
            });

            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                chart.Series.Clear();
            });

            EnqueueCallback(() =>
            {
                DataSeries dataSeries4 = new DataSeries();
                dataSeries4.RenderAs = RenderAs.Doughnut;
                numberOfDataPoints = 0;
                for (Int32 i = 0; i < 30; i++)
                {
                    DataPoint dataPoint = new DataPoint();
                    dataPoint.XValue = i + 1;
                    dataPoint.YValue = rand.Next(100, 500);
                    dataSeries4.DataPoints.Add(dataPoint);
                    numberOfDataPoints++;
                }
                chart.Series.Add(dataSeries4);
                Assert.AreEqual(1, chart.Series.Count);
                _htmlElement1.SetProperty("value", dataSeries4.RenderAs + " with " + numberOfDataPoints + " DataPoints. Number of Render Count: " + chart.ChartArea._renderCount);
            });

            EnqueueCallback(() =>
            {
                EnqueueTestComplete();
            });

            _htmlElement1 = Common.GetDisplayMessageButton(_htmlElement1);
            _htmlElement1.SetStyleAttribute("width", "900px");
            System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement1);
        }
        #endregion

        #region MultiSeriesBarChartCheckingWithSecondaryAxis
        /// <summary>
        /// Testing Bar MultiSeries Chart loading
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void MultiSeriesBarChartCheckingWithSecondaryAxis()
        {
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "500px");
            Chart chart = new Chart();
            chart.Width = 350;
            chart.Height = 500;
            chart.View3D = true;

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Int32 numberOfDataPoints = 0;

            Random rand = new Random();

            TestPanel.Children.Add(chart);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                isDataSeriesAdded = false;
                DataSeries dataSeries1 = new DataSeries();
                dataSeries1.RenderAs = RenderAs.Bar;
                numberOfDataPoints = 0;
                for (Int32 i = 0; i < 20; i++)
                {
                    DataPoint dataPoint = new DataPoint();
                    dataPoint.XValue = i + 1;
                    dataPoint.YValue = rand.Next(100, 400);
                    dataSeries1.DataPoints.Add(dataPoint);
                    numberOfDataPoints++;
                }
                chart.Series.Add(dataSeries1);
                Assert.AreEqual(1, chart.Series.Count);
                isDataSeriesAdded = true;
                _htmlElement1.SetProperty("value", dataSeries1.RenderAs + " with " + numberOfDataPoints + " DataPoints. Number of Render Count: " + chart.ChartArea._renderCount);
            });

            EnqueueConditional(() => isDataSeriesAdded);
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                isDataSeriesAdded = false;
                DataSeries dataSeries2 = new DataSeries();
                dataSeries2.RenderAs = RenderAs.StackedBar;
                numberOfDataPoints = 0;
                for (Int32 i = 0; i < 20; i++)
                {
                    DataPoint dataPoint = new DataPoint();
                    dataPoint.XValue = i + 1;
                    dataPoint.YValue = rand.Next(100, 300);
                    dataSeries2.DataPoints.Add(dataPoint);
                    numberOfDataPoints++;
                }
                chart.Series.Add(dataSeries2);
                Assert.AreEqual(2, chart.Series.Count);
                isDataSeriesAdded = true;
                _htmlElement1.SetProperty("value", dataSeries2.RenderAs + " with " + numberOfDataPoints + " DataPoints. Number of Render Count: " + chart.ChartArea._renderCount);
            });

            EnqueueConditional(() => isDataSeriesAdded);
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                isDataSeriesAdded = false;
                DataSeries dataSeries3 = new DataSeries();
                dataSeries3.RenderAs = RenderAs.StackedBar100;
                dataSeries3.AxisYType = AxisTypes.Secondary;
                numberOfDataPoints = 0;
                for (Int32 i = 0; i < 20; i++)
                {
                    DataPoint dataPoint = new DataPoint();
                    dataPoint.XValue = i + 1;
                    dataPoint.YValue = rand.Next(0, 500);
                    dataSeries3.DataPoints.Add(dataPoint);
                    numberOfDataPoints++;
                }
                chart.Series.Add(dataSeries3);
                Assert.AreEqual(3, chart.Series.Count);
                isDataSeriesAdded = true;
                _htmlElement1.SetProperty("value", dataSeries3.RenderAs + " with " + numberOfDataPoints + " DataPoints. Number of Render Count: " + chart.ChartArea._renderCount);
            });

            EnqueueConditional(() => isDataSeriesAdded);
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                isDataSeriesAdded = false;
                DataSeries dataSeries4 = new DataSeries();
                dataSeries4.RenderAs = RenderAs.StackedBar;
                dataSeries4.AxisYType = AxisTypes.Secondary;
                numberOfDataPoints = 0;
                for (Int32 i = 0; i < 20; i++)
                {
                    DataPoint dataPoint = new DataPoint();
                    dataPoint.XValue = i + 1;
                    dataPoint.YValue = rand.Next(50, 350);
                    dataSeries4.DataPoints.Add(dataPoint);
                    numberOfDataPoints++;
                }
                chart.Series.Add(dataSeries4);
                Assert.AreEqual(4, chart.Series.Count);
                isDataSeriesAdded = true;
                _htmlElement1.SetProperty("value", dataSeries4.RenderAs + " with " + numberOfDataPoints + " DataPoints. Number of Render Count: " + chart.ChartArea._renderCount);
            });

            EnqueueConditional(() => isDataSeriesAdded);
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                isDataSeriesAdded = false;
                DataSeries dataSeries5 = new DataSeries();
                dataSeries5.RenderAs = RenderAs.StackedBar;
                dataSeries5.AxisYType = AxisTypes.Secondary;
                numberOfDataPoints = 0;
                for (Int32 i = 0; i < 20; i++)
                {
                    DataPoint dataPoint = new DataPoint();
                    dataPoint.XValue = i + 1;
                    dataPoint.YValue = rand.Next(50, 350);
                    dataSeries5.DataPoints.Add(dataPoint);
                    numberOfDataPoints++;
                }
                chart.Series.Add(dataSeries5);
                Assert.AreEqual(5, chart.Series.Count);
                isDataSeriesAdded = true;
                _htmlElement1.SetProperty("value", dataSeries5.RenderAs + " with " + numberOfDataPoints + " DataPoints. Number of Render Count: " + chart.ChartArea._renderCount);
            });

            EnqueueConditional(() => isDataSeriesAdded);
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                isDataSeriesAdded = false;
                DataSeries dataSeries6 = new DataSeries();
                dataSeries6.RenderAs = RenderAs.StackedBar;
                numberOfDataPoints = 0;
                for (Int32 i = 0; i < 20; i++)
                {
                    DataPoint dataPoint = new DataPoint();
                    dataPoint.XValue = i + 1;
                    dataPoint.YValue = rand.Next(50, 350);
                    dataSeries6.DataPoints.Add(dataPoint);
                    numberOfDataPoints++;
                }
                chart.Series.Add(dataSeries6);
                Assert.AreEqual(6, chart.Series.Count);
                isDataSeriesAdded = true;
                _htmlElement1.SetProperty("value", dataSeries6.RenderAs + " with " + numberOfDataPoints + " DataPoints. Number of Render Count: " + chart.ChartArea._renderCount);
            });

            EnqueueConditional(() => isDataSeriesAdded);
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                isDataSeriesAdded = false;
                DataSeries dataSeries7 = new DataSeries();
                dataSeries7.RenderAs = RenderAs.Bar;
                dataSeries7.AxisYType = AxisTypes.Secondary;
                numberOfDataPoints = 0;
                for (Int32 i = 0; i < 20; i++)
                {
                    DataPoint dataPoint = new DataPoint();
                    dataPoint.XValue = i + 1;
                    dataPoint.YValue = rand.Next(0, 250);
                    dataSeries7.DataPoints.Add(dataPoint);
                    numberOfDataPoints++;
                }
                chart.Series.Add(dataSeries7);
                Assert.AreEqual(7, chart.Series.Count);
                isDataSeriesAdded = true;
                _htmlElement1.SetProperty("value", dataSeries7.RenderAs + " with " + numberOfDataPoints + " DataPoints. Number of Render Count: " + chart.ChartArea._renderCount);
            });

            EnqueueConditional(() => isDataSeriesAdded);
            isDataSeriesAdded = false;
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                EnqueueTestComplete();
            });

            _htmlElement1 = Common.GetDisplayMessageButton(_htmlElement1);
            _htmlElement1.SetStyleAttribute("width", "900px");
            System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement1);
        }
        #endregion

        #region TestDataPointsCollectionChanged
        /// <summary>
        /// Test the DataPoints collection changed event
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void TestDataPointsCollectionChanged()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;


            Int32 dataPointsAdded = 0;

            chart.Loaded += new System.Windows.RoutedEventHandler(chart_Loaded);

            TestPanel.Children.Add(chart);

            DataSeries dataSeries = new DataSeries();

            Random rand = new Random();

            for (Int32 i = 0; i < 10; i++)
            {
                DataPoint dataPoint = new DataPoint();
                dataPoint.AxisXLabel = "Label" + i;
                dataPoint.XValue = i + 1;
                dataPoint.YValue = rand.Next(-100, 100);
                dataSeries.DataPoints.Add(dataPoint);
            }

            chart.Series.Add(dataSeries);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueDelay(_sleepTime);

            dataSeries.DataPoints.CollectionChanged += (object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
                =>
            {
                if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
                {
                    dataPointsAdded += e.NewItems.Count;
                    Assert.AreEqual(1, e.NewItems.Count);
                }
            };

            EnqueueCallback(() =>
            {
                DataPoint dataPoint = new DataPoint();
                dataPoint.AxisXLabel = "Label";
                dataPoint.YValue = rand.Next(-100, 100);
                dataSeries.DataPoints.Add(dataPoint);
            }
            );

            EnqueueCallback(() =>
            {
                Assert.AreEqual(1, dataPointsAdded);
            });

            EnqueueDelay(_sleepTime);
            EnqueueTestComplete();
        }
        #endregion

        #region CheckDataseriesNewPropertyValue

        #region CheckEnabledProperty4AllchartTypes

        /// <summary>
        /// Check the Column Enabled property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckColumnEnabledProperty()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            Random rand = new Random();

            DataSeries dataSeries = CreateDataSeries();

            chart.Series.Add(dataSeries);

            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                () => dataSeries.Enabled = false,
                () => Assert.IsFalse((Boolean)dataSeries.Enabled));

            EnqueueTestComplete();

        }

        /// <summary>
        /// Check the Bar Enabled property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckBarEnabledProperty()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            Random rand = new Random();

            DataSeries dataSeries = CreateDataSeries();
            dataSeries.RenderAs = RenderAs.Bar;

            chart.Series.Add(dataSeries);

            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                () => dataSeries.Enabled = false,
                () => Assert.IsFalse((Boolean)dataSeries.Enabled));

            EnqueueTestComplete();

        }

        /// <summary>
        /// Check the Area Enabled property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckAreaEnabledProperty()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            Random rand = new Random();

            DataSeries dataSeries = CreateDataSeries();
            dataSeries.RenderAs = RenderAs.Area;

            chart.Series.Add(dataSeries);

            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                () => dataSeries.Enabled = false,
                () => Assert.IsFalse((Boolean)dataSeries.Enabled));

            EnqueueTestComplete();

        }

        /// <summary>
        /// Check the Bubble Enabled property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckBubbleEnabledProperty()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            Random rand = new Random();

            DataSeries dataSeries = CreateDataSeries();
            dataSeries.RenderAs = RenderAs.Bubble;

            chart.Series.Add(dataSeries);

            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                () => dataSeries.Enabled = false,
                () => Assert.IsFalse((Boolean)dataSeries.Enabled));

            EnqueueTestComplete();

        }

        /// <summary>
        /// Check the Point Enabled property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckPointEnabledProperty()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            Random rand = new Random();

            DataSeries dataSeries = CreateDataSeries();
            dataSeries.RenderAs = RenderAs.Point;

            chart.Series.Add(dataSeries);

            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                () => dataSeries.Enabled = false,
                () => Assert.IsFalse((Boolean)dataSeries.Enabled));

            EnqueueTestComplete();

        }

        /// <summary>
        /// Check the Pie Enabled property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckPieEnabledProperty()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            Random rand = new Random();

            DataSeries dataSeries = CreateDataSeries();
            dataSeries.RenderAs = RenderAs.Pie;

            chart.Series.Add(dataSeries);

            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                () => dataSeries.Enabled = false,
                () => Assert.IsFalse((Boolean)dataSeries.Enabled));

            EnqueueTestComplete();

        }

        /// <summary>
        /// Check the Doughnut Enabled property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckDoughnutEnabledProperty()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            Random rand = new Random();

            DataSeries dataSeries = CreateDataSeries();
            dataSeries.RenderAs = RenderAs.Doughnut;

            chart.Series.Add(dataSeries);

            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                () => dataSeries.Enabled = false,
                () => Assert.IsFalse((Boolean)dataSeries.Enabled));

            EnqueueTestComplete();

        }

        /// <summary>
        /// Check the Line Enabled property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckLineEnabledProperty()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            Random rand = new Random();

            DataSeries dataSeries = CreateDataSeries();
            dataSeries.RenderAs = RenderAs.Line;

            chart.Series.Add(dataSeries);

            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                () => dataSeries.Enabled = false,
                () => Assert.IsFalse((Boolean)dataSeries.Enabled));

            EnqueueTestComplete();

        }

        /// <summary>
        /// Check the StackedColumn Enabled property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckStackedColumnEnabledProperty()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            Random rand = new Random();

            DataSeries dataSeries = CreateDataSeries();
            dataSeries.RenderAs = RenderAs.StackedColumn;

            chart.Series.Add(dataSeries);

            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                () => dataSeries.Enabled = false,
                () => Assert.IsFalse((Boolean)dataSeries.Enabled));

            EnqueueTestComplete();

        }

        /// <summary>
        /// Check the StackedColumn100 Enabled property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckStackedColumn100EnabledProperty()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            Random rand = new Random();

            DataSeries dataSeries = CreateDataSeries();
            dataSeries.RenderAs = RenderAs.StackedColumn100;

            chart.Series.Add(dataSeries);

            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                () => dataSeries.Enabled = false,
                () => Assert.IsFalse((Boolean)dataSeries.Enabled));

            EnqueueTestComplete();

        }

        /// <summary>
        /// Check the StackedBar Enabled property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckStackedBarEnabledProperty()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            Random rand = new Random();

            DataSeries dataSeries = CreateDataSeries();
            dataSeries.RenderAs = RenderAs.StackedBar;

            chart.Series.Add(dataSeries);

            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                () => dataSeries.Enabled = false,
                () => Assert.IsFalse((Boolean)dataSeries.Enabled));

            EnqueueTestComplete();

        }

        /// <summary>
        /// Check the StackedBar100 Enabled property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckStackedBar100EnabledProperty()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            Random rand = new Random();

            DataSeries dataSeries = CreateDataSeries();
            dataSeries.RenderAs = RenderAs.StackedBar100;

            chart.Series.Add(dataSeries);

            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                () => dataSeries.Enabled = false,
                () => Assert.IsFalse((Boolean)dataSeries.Enabled));

            EnqueueTestComplete();

        }

        /// <summary>
        /// Check the StackedArea Enabled property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckStackedAreaEnabledProperty()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            Random rand = new Random();

            DataSeries dataSeries = CreateDataSeries();
            dataSeries.RenderAs = RenderAs.StackedArea;

            chart.Series.Add(dataSeries);

            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                () => dataSeries.Enabled = false,
                () => Assert.IsFalse((Boolean)dataSeries.Enabled));

            EnqueueTestComplete();

        }

        /// <summary>
        /// Check the StackedArea100 Enabled property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckStackedArea100EnabledProperty()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            Random rand = new Random();

            DataSeries dataSeries = CreateDataSeries();
            dataSeries.RenderAs = RenderAs.StackedArea100;

            chart.Series.Add(dataSeries);

            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                () => dataSeries.Enabled = false,
                () => Assert.IsFalse((Boolean)dataSeries.Enabled));

            EnqueueTestComplete();

        }

        #endregion

        /// <summary>
        /// Check the RenderAs property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckRenderAsProperty()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            Random rand = new Random();

            DataSeries dataSeries = CreateDataSeries();
            dataSeries.RenderAs = RenderAs.Area;

            chart.Series.Add(dataSeries);

            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                () => dataSeries.RenderAs = RenderAs.Bubble,
                () => Assert.AreEqual(RenderAs.Bubble, dataSeries.RenderAs));

            EnqueueTestComplete();

        }

        /// <summary>
        /// Check the Color property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckColorProperty()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;


            Random rand = new Random();

            DataSeries dataSeries = CreateDataSeries();

            chart.Series.Add(dataSeries);

            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                () => dataSeries.Color = new SolidColorBrush(Colors.Cyan),
                () => Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Cyan), dataSeries.Color));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the LegendText property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckLegendTextProperty()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;


            Random rand = new Random();

            DataSeries dataSeries = CreateDataSeries();
            dataSeries.ShowInLegend = true;

            chart.Series.Add(dataSeries);

            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                () => dataSeries.LegendText = "Legend1",
                () => Assert.AreEqual("Legend1", dataSeries.LegendText));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the Legend property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckLegendProperty()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            Random rand = new Random();

            DataSeries dataSeries = CreateDataSeries();

            Legend legend = new Legend();
            legend.SetValue(FrameworkElement.NameProperty, "Legend0");
            chart.Legends.Add(legend);

            dataSeries.ShowInLegend = true;
            chart.Series.Add(dataSeries);

            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                () => dataSeries.Legend = "Legend0",
                () => Assert.AreEqual("Legend0", dataSeries.Legend));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the Bevel property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckBevelProperty()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;


            Random rand = new Random();

            DataSeries dataSeries = CreateDataSeries();
            dataSeries.RenderAs = RenderAs.Pie;
            chart.Series.Add(dataSeries);

            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                () => dataSeries.Bevel = true,
                () => Assert.AreEqual(true, dataSeries.Bevel));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the LightingEnabled property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckLightingEnabledProperty()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;


            DataSeries dataSeries = CreateDataSeries();
            dataSeries.Color = new SolidColorBrush(Colors.Red);

            chart.Series.Add(dataSeries);

            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                () => dataSeries.LightingEnabled = true,
                () => Assert.AreEqual(true, dataSeries.LightingEnabled));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the LineThickness property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckLineThicknessProperty()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;


            DataSeries dataSeries = CreateDataSeries();
            dataSeries.RenderAs = RenderAs.Line;
            chart.Series.Add(dataSeries);

            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                () => dataSeries.LineThickness = 1,
                () => Assert.AreEqual(1, dataSeries.LineThickness));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the LineStyle property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckLineStyleProperty()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;


            DataSeries dataSeries = CreateDataSeries();
            dataSeries.RenderAs = RenderAs.Line;
            chart.Series.Add(dataSeries);

            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                () => dataSeries.LineStyle = LineStyles.Dashed,
                () => Assert.AreEqual(LineStyles.Dashed, dataSeries.LineStyle));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the ShadowEnabled property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckShadowEnabledProperty()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;


            DataSeries dataSeries = CreateDataSeries();

            chart.Series.Add(dataSeries);

            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                () => dataSeries.ShadowEnabled = true,
                () => Assert.AreEqual(true, dataSeries.ShadowEnabled));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the ShowInLegend property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckShowInLegendProperty()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;


            DataSeries dataSeries = CreateDataSeries();

            chart.Series.Add(dataSeries);

            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                () => dataSeries.ShowInLegend = true,
                () => Assert.AreEqual(true, dataSeries.ShowInLegend));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the StartAngle property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckStartAngleProperty()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            DataSeries dataSeries = CreateDataSeries();
            dataSeries.RenderAs = RenderAs.Pie;

            chart.Series.Add(dataSeries);

            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                () => dataSeries.StartAngle = 90,
                () => Assert.AreEqual(90, dataSeries.StartAngle));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the ZIndex property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckZIndexProperty()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;
            chart.View3D = true;

            DataSeries dataSeries1 = CreateDataSeries();
            dataSeries1.RenderAs = RenderAs.Column;
            dataSeries1.Color = new SolidColorBrush(Colors.Orange);
            chart.Series.Add(dataSeries1);

            DataSeries dataSeries2 = CreateDataSeries();
            dataSeries2.RenderAs = RenderAs.Area;
            dataSeries2.Color = new SolidColorBrush(Colors.Red);
            chart.Series.Add(dataSeries2);

            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                () => dataSeries1.ZIndex = 100,
                () => Assert.AreEqual(100, dataSeries1.ZIndex));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the XValueFormatString property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckXValueFormatStringProperty()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;


            DataSeries dataSeries = CreateDataSeries();

            chart.Series.Add(dataSeries);

            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                () => dataSeries.XValueFormatString = "#,#.0#'%'",
                () => Assert.AreEqual("#,#.0#'%'", dataSeries.XValueFormatString));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the YValueFormatString property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckYValueFormatStringProperty()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;


            DataSeries dataSeries = CreateDataSeries();

            chart.Series.Add(dataSeries);

            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                () => dataSeries.YValueFormatString = "#,#.0#",
                () => Assert.AreEqual("#,#.0#", dataSeries.YValueFormatString));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the ZValueFormatString property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckZValueFormatStringProperty()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            DataSeries dataSeries = CreateDataSeries2();
            dataSeries.RenderAs = RenderAs.Bubble;

            chart.Series.Add(dataSeries);

            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                () => dataSeries.ZValueFormatString = "#,#.0#",
                () => Assert.AreEqual("#,#.0#", dataSeries.ZValueFormatString));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the ToolTipText property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckToolTipTextProperty()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            DataSeries dataSeries = CreateDataSeries();

            chart.Series.Add(dataSeries);

            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                () => dataSeries.ToolTipText = "#AxisLabel",
                () => Assert.AreEqual("#AxisLabel", dataSeries.ToolTipText));

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
                () => dataSeries.Href = "http://www.visifire.com",
                () => dataSeries.HrefTarget = HrefTargets._blank,
                () => Assert.AreEqual("http://www.visifire.com", dataSeries.Href),
                () => Assert.AreEqual(HrefTargets._blank, dataSeries.HrefTarget));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the AxisYType property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckAxisYTypeProperty()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            DataSeries dataSeries = CreateDataSeries();

            chart.Series.Add(dataSeries);

            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                () => dataSeries.AxisYType = AxisTypes.Secondary,
                () => Assert.AreEqual(AxisTypes.Secondary, dataSeries.AxisYType));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the LabelEnabled property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckLabelEnabledProperty()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;


            DataSeries dataSeries = CreateDataSeries();

            chart.Series.Add(dataSeries);

            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                () => dataSeries.LabelEnabled = true,
                () => Assert.AreEqual(true, dataSeries.LabelEnabled));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the LabelText property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckLabelTextProperty()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;


            DataSeries dataSeries = CreateDataSeries();
            dataSeries.LabelEnabled = true;

            chart.Series.Add(dataSeries);

            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                () => dataSeries.LabelText = "Visifire",
                () => Assert.AreEqual("Visifire", dataSeries.LabelText));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the LabelFontFamily property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckLabelFontFamilyProperty()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;


            DataSeries dataSeries = CreateDataSeries();
            dataSeries.LabelEnabled = true;

            chart.Series.Add(dataSeries);

            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                () => dataSeries.LabelFontFamily = new FontFamily("MS Trebuchet"),
                () => Assert.AreEqual(new FontFamily("MS Trebuchet"), dataSeries.LabelFontFamily));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the LabelFontSize property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckLabelFontSizeProperty()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;


            DataSeries dataSeries = CreateDataSeries();
            dataSeries.LabelEnabled = true;

            chart.Series.Add(dataSeries);

            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                () => dataSeries.LabelFontSize = 14,
                () => Assert.AreEqual(14, dataSeries.LabelFontSize));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the LabelFontColor property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckLabelFontColorProperty()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;


            DataSeries dataSeries = CreateDataSeries();
            dataSeries.LabelEnabled = true;

            chart.Series.Add(dataSeries);

            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                () => dataSeries.LabelFontColor = new SolidColorBrush(Colors.DarkGray),
                () => Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.DarkGray), dataSeries.LabelFontColor));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the LabelFontWeight property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckLabelFontWeightProperty()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;


            DataSeries dataSeries = CreateDataSeries();
            dataSeries.LabelEnabled = true;

            chart.Series.Add(dataSeries);

            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                () => dataSeries.LabelFontWeight = FontWeights.Bold,
                () => Assert.AreEqual(FontWeights.Bold, dataSeries.LabelFontWeight));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the LabelFontStyle property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckLabelFontStyleProperty()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;


            DataSeries dataSeries = CreateDataSeries();
            dataSeries.LabelEnabled = true;

            chart.Series.Add(dataSeries);

            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                () => dataSeries.LabelFontStyle = FontStyles.Italic,
                () => Assert.AreEqual(FontStyles.Italic, dataSeries.LabelFontStyle));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the LabelBackground property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckLabelBackgroundProperty()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;


            DataSeries dataSeries = CreateDataSeries();
            dataSeries.LabelEnabled = true;

            chart.Series.Add(dataSeries);

            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                () => dataSeries.LabelBackground = new SolidColorBrush(Colors.Green),
                () => Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Green), dataSeries.LabelBackground));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the LabelStyle property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckLabelStyleProperty()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            DataSeries dataSeries = CreateDataSeries();
            dataSeries.LabelEnabled = true;

            chart.Series.Add(dataSeries);

            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                () => dataSeries.LabelStyle = LabelStyles.Inside,
                () => Assert.AreEqual(LabelStyles.Inside, dataSeries.LabelStyle));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the LabelLineEnabled property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckLabelLineEnabledProperty()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            DataSeries dataSeries = CreateDataSeries();
            dataSeries.RenderAs = RenderAs.Pie;

            chart.Series.Add(dataSeries);

            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                () => dataSeries.LabelLineEnabled = false,
                () => Assert.IsFalse((Boolean)dataSeries.LabelLineEnabled));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the LabelLineColor property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckLabelLineColorProperty()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            DataSeries dataSeries = CreateDataSeries();
            dataSeries.RenderAs = RenderAs.Pie;

            chart.Series.Add(dataSeries);

            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                () => dataSeries.LabelLineColor = new SolidColorBrush(Colors.Magenta),
                () => Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Magenta), dataSeries.LabelLineColor));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the LabelLineThickness property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckLabelLineThicknessProperty()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            DataSeries dataSeries = CreateDataSeries();
            dataSeries.RenderAs = RenderAs.Pie;

            chart.Series.Add(dataSeries);

            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                () => dataSeries.LabelLineThickness = 2,
                () => Assert.AreEqual(2, dataSeries.LabelLineThickness));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the LabelLineStyle property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckLabelLineStyleProperty()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            DataSeries dataSeries = CreateDataSeries();
            dataSeries.RenderAs = RenderAs.Pie;

            chart.Series.Add(dataSeries);

            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                () => dataSeries.LabelLineStyle = LineStyles.Dashed,
                () => Assert.AreEqual(LineStyles.Dashed, dataSeries.LabelLineStyle));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the MarkerEnabled property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckMarkerEnabledProperty()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            DataSeries dataSeries = CreateDataSeries();
            dataSeries.RenderAs = RenderAs.Column;
            chart.Series.Add(dataSeries);

            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                () => dataSeries.MarkerEnabled = true,
                () => Assert.AreEqual(true, dataSeries.MarkerEnabled));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the MarkerType property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckMarkerTypeProperty()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            DataSeries dataSeries = CreateDataSeries();
            dataSeries.RenderAs = RenderAs.Line;

            chart.Series.Add(dataSeries);

            EnqueueDelay(_sleepTime);
            CreateAsyncTest(chart,
                () => dataSeries.MarkerType = MarkerTypes.Diamond,
                () => Assert.AreEqual(MarkerTypes.Diamond, dataSeries.MarkerType));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the MarkerBorderThickness property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckMarkerBorderThicknessProperty()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            DataSeries dataSeries = CreateDataSeries();
            dataSeries.RenderAs = RenderAs.Line;

            chart.Series.Add(dataSeries);

            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                () => dataSeries.MarkerBorderThickness = new Thickness(1),
                () => Assert.AreEqual(new Thickness(1), dataSeries.MarkerBorderThickness));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the MarkerSize property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckMarkerSizeProperty()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            DataSeries dataSeries = CreateDataSeries();
            dataSeries.RenderAs = RenderAs.Line;

            chart.Series.Add(dataSeries);

            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                () => dataSeries.MarkerSize = 15,
                () => Assert.AreEqual(15, dataSeries.MarkerSize));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the MarkerColor property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckMarkerColorProperty()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            DataSeries dataSeries = CreateDataSeries();
            dataSeries.RenderAs = RenderAs.Line;

            chart.Series.Add(dataSeries);

            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                () => dataSeries.MarkerColor = new SolidColorBrush(Colors.Red),
                () => Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Red), dataSeries.MarkerColor));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the MarkerBorderColor property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckMarkerBorderColorProperty()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;


            DataSeries dataSeries = CreateDataSeries();
            dataSeries.RenderAs = RenderAs.Line;
            chart.Series.Add(dataSeries);

            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                () => dataSeries.MarkerBorderColor = new SolidColorBrush(Colors.Purple),
                () => Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Purple), dataSeries.MarkerBorderColor));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the MarkerScale property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckMarkerScaleProperty()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            DataSeries dataSeries = CreateDataSeries();
            dataSeries.RenderAs = RenderAs.Line;
            chart.Series.Add(dataSeries);

            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                () => dataSeries.MarkerScale = 10,
                () => Assert.AreEqual(10, dataSeries.MarkerScale));

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
            dataSeries.Bevel = false;

            chart.Series.Add(dataSeries);

            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                () => dataSeries.RadiusX = new CornerRadius(5, 5, 5, 5),
                () => dataSeries.RadiusY = new CornerRadius(5, 5, 5, 5),
                () => Assert.AreEqual(new CornerRadius(5, 5, 5, 5), dataSeries.RadiusX),
                () => Assert.AreEqual(new CornerRadius(5, 5, 5, 5), dataSeries.RadiusY));

            EnqueueTestComplete();
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
            CreateAsyncTask(chart,
                () => dataSeries.InternalOpacity = 0.5,
                () => Assert.AreEqual(0.5, dataSeries.InternalOpacity, Common.HighPrecisionDelta));

            EnqueueTestComplete();
        }

        #endregion

        #region CheckDataSeriesDefaultPropertyValue

        /// <summary>
        /// Check the Enabled property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckEnabledDefaultProperty()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            Random rand = new Random();

            DataSeries dataSeries = CreateDataSeries();

            chart.Series.Add(dataSeries);

            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                () => Assert.IsTrue((Boolean)dataSeries.Enabled));

            EnqueueTestComplete();

        }

        /// <summary>
        /// Check the RenderAs property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckRenderAsDefaultProperty()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;


            Random rand = new Random();

            DataSeries dataSeries = CreateDataSeries();


            chart.Series.Add(dataSeries);

            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                () => Assert.AreEqual(RenderAs.Column, dataSeries.RenderAs));

            EnqueueTestComplete();

        }

        /// <summary>
        /// Check the Color property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckColorDefaultProperty()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;


            Random rand = new Random();

            DataSeries dataSeries = CreateDataSeries();

            chart.Series.Add(dataSeries);

            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                () => Assert.IsNull(dataSeries.Color));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the LegendText property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckLegendTextDefaultProperty()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;


            Random rand = new Random();

            DataSeries dataSeries1 = CreateDataSeries();
            DataSeries dataSeries2 = CreateDataSeries();
            chart.Series.Add(dataSeries1);
            chart.Series.Add(dataSeries2);

            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                delegate
                {
                    for (Int32 i = 0; i < chart.Series.Count; i++)
                        Assert.IsNull(chart.Series[i].LegendText);
                });

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the Legend property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckLegendDefaultProperty()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;


            Random rand = new Random();

            DataSeries dataSeries = CreateDataSeries();

            dataSeries.ShowInLegend = true;
            chart.Series.Add(dataSeries);

            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                () => Assert.AreEqual("Legend0", dataSeries.Legend));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the Bevel property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckBevelDefaultProperty()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;


            Random rand = new Random();

            DataSeries dataSeries = CreateDataSeries();

            chart.Series.Add(dataSeries);

            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                () => Assert.IsTrue(dataSeries.Bevel));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the LightingEnabled property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckLightingEnabledDefaultProperty()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;


            DataSeries dataSeries = CreateDataSeries();

            chart.Series.Add(dataSeries);

            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                () => Assert.IsTrue((Boolean)dataSeries.LightingEnabled));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the AxisYType default property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckAxisYTypeDefaultPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            DataSeries dataSeries = CreateDataSeries();

            chart.Series.Add(dataSeries);

            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                () => Assert.AreEqual(AxisTypes.Primary, dataSeries.AxisYType));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the Bevel default property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckBevelDefaultPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            DataSeries dataSeries = CreateDataSeries();

            chart.Series.Add(dataSeries);

            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                () => Assert.IsTrue(dataSeries.Bevel));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the ShadowEnabled default property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckShadowEnabledDefaultPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            DataSeries dataSeries = CreateDataSeries();

            chart.Series.Add(dataSeries);

            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                () => Assert.IsFalse(dataSeries.ShadowEnabled));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the ShowInLegend single series default property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckShowInLegendSingleSeriesDefaultPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            DataSeries dataSeries = CreateDataSeries();

            chart.Series.Add(dataSeries);

            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                () => Assert.IsFalse((Boolean)dataSeries.ShowInLegend));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the ShowInLegend Multiple series default property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckShowInLegendMultipleSeriesDefaultPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            DataSeries dataSeries1 = CreateDataSeries();
            DataSeries dataSeries2 = CreateDataSeries();

            chart.Series.Add(dataSeries1);
            chart.Series.Add(dataSeries2);

            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                () => Assert.IsTrue((Boolean)dataSeries1.ShowInLegend),
                () => Assert.IsTrue((Boolean)dataSeries2.ShowInLegend));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the LineThickness default property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckLineThicknessDefaultPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            DataSeries dataSeries = CreateDataSeries();
            dataSeries.RenderAs = RenderAs.Line;

            chart.Series.Add(dataSeries);

            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                () => Assert.AreEqual((((chart.ActualWidth * chart.ActualHeight) + 25000) / 35000) > 4 ? 4 : ((chart.ActualWidth * chart.ActualHeight) + 25000) / 35000, dataSeries.LineThickness));

            EnqueueTestComplete();
        }

        ///// <summary>
        /// Check the LineStyle default property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckLineStyleDefaultPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            DataSeries dataSeries = CreateDataSeries();
            dataSeries.RenderAs = RenderAs.Line;

            chart.Series.Add(dataSeries);

            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                () => Assert.AreEqual(LineStyles.Solid, dataSeries.LineStyle));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the StartAngle default property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckStartAngleDefaultPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            DataSeries dataSeries = CreateDataSeries();
            dataSeries.RenderAs = RenderAs.Pie;

            chart.Series.Add(dataSeries);

            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                () => Assert.AreEqual(0, dataSeries.StartAngle));

            EnqueueTestComplete();
        }

        /// <summary>
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
                () => Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Black), dataSeries.BorderColor));

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
                () => Assert.AreEqual(new Thickness(0), dataSeries.BorderThickness));

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
                () => Assert.IsNull(dataSeries.LabelBackground));

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
                () => Assert.IsFalse((Boolean)dataSeries.LabelEnabled));

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
                () => Assert.IsTrue((Boolean)dataSeries.LabelEnabled));

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
                () => Assert.IsNull(dataSeries.LabelFontColor));

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
                () => Assert.AreEqual(new FontFamily("Verdana"), dataSeries.LabelFontFamily));

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
                () => Assert.AreEqual(10
                    , dataSeries.LabelFontSize));

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
                () => Assert.AreEqual(FontStyles.Normal, dataSeries.LabelFontStyle));

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
                () => Assert.AreEqual(FontWeights.Normal, dataSeries.LabelFontWeight));

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
                () => Assert.IsNull(dataSeries.LabelLineEnabled));

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
                () => Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Gray), dataSeries.LabelLineColor));

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
                () => Assert.AreEqual(LineStyles.Solid, dataSeries.LabelLineStyle));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the LabelLineThickness default property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckLabelLineThicknessDefaultPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            DataSeries dataSeries = CreateDataSeries();
            dataSeries.RenderAs = RenderAs.Pie;

            chart.Series.Add(dataSeries);

            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                () => Assert.AreEqual(0, dataSeries.LabelLineThickness));

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
            dataSeries.LabelEnabled = true;

            chart.Series.Add(dataSeries);

            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                () => Assert.AreEqual(LabelStyles.OutSide, dataSeries.LabelStyle));

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
            dataSeries.LabelEnabled = true;

            chart.Series.Add(dataSeries);

            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                () => Assert.AreEqual(LabelStyles.Inside, dataSeries.LabelStyle));

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
                () => Assert.IsNull(dataSeries.MarkerBorderColor));

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
                () => Assert.IsNull(dataSeries.MarkerBorderThickness));

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
                () => Assert.IsTrue((Boolean)dataSeries.MarkerEnabled));

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
                () => Assert.AreEqual(1, dataSeries.MarkerScale));

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
                () => Assert.AreEqual((Double)(dataSeries.LineThickness * 2), (Double)dataSeries.MarkerSize, Common.HighPrecisionDelta));

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
                () => Assert.AreEqual(MarkerTypes.Circle, dataSeries.MarkerType));

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
                () => Assert.AreEqual(new CornerRadius(0, 0, 0, 0), dataSeries.RadiusX),
                () => Assert.AreEqual(new CornerRadius(0, 0, 0, 0), dataSeries.RadiusY));

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
                () => Assert.AreEqual("#AxisXLabel, #YValue", dataSeries.ToolTipText));

            EnqueueTestComplete();
        }
        #endregion

        #region Performance Tests
        /// <summary>
        /// Performance and Stress
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        [Asynchronous]
        public void ColumnDataSeriesStressTest()
        {
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "400px");

            Int32 numberOfSeries = 0;
            DataSeries dataSeries = null;

            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;
            chart.View3D = true;

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            TestPanel.Children.Add(chart);

            Axis axisX = new Axis();
            axisX.Interval = 1;
            chart.AxesX.Add(axisX);

            Random rand = new Random();

            Int32 numberofDataPoint = 0;

            Double totalDuration = 0;
            DateTime start = DateTime.UtcNow;
            String msg = Common.AssertAverageDuration(200, 1, delegate
            {
                dataSeries = new DataSeries();
                dataSeries.RenderAs = RenderAs.Column;

                for (Int32 i = 0; i < 1000; i++)
                {
                    DataPoint dataPoint = new DataPoint();
                    dataPoint.AxisXLabel = "a" + i;
                    dataPoint.YValue = rand.Next(-100, 100);
                    dataSeries.DataPoints.Add(dataPoint);
                    numberofDataPoint++;
                }
                numberOfSeries++;
                chart.Series.Add(dataSeries);
            });

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
                _htmlElement1.SetProperty("value", dataSeries.RenderAs + " chart with " + numberOfSeries + " DataSeries having " + numberofDataPoint + " DataPoints. Total Chart Loading Time: " + totalDuration + "s. Number of Render Count: " + chart.ChartArea._renderCount);
                _htmlElement2 = Common.GetDisplayMessageButton(_htmlElement2);
                _htmlElement2.SetStyleAttribute("top", "540px");
                _htmlElement2.SetProperty("value", "Total Calculation: " + msg + " Click here to exit.");
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement1);
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement2);
            });

            EnqueueDelay(_sleepTime);
            EnqueueCallback(() =>
            {
                _htmlElement2.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.HtmlElement_OnClick));
            });
        }

        /// <summary>
        /// Performance and Stress
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        [Asynchronous]
        public void AreaDataSeriesStressTest()
        {
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "400px");

            Double totalDuration = 0;
            DateTime start = DateTime.UtcNow;

            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;
            chart.View3D = false;

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            TestPanel.Children.Add(chart);

            Axis axisX = new Axis();
            axisX.Interval = 1;
            chart.AxesX.Add(axisX);

            Random rand = new Random();

            Int32 numberOfSeries = 0;
            DataSeries dataSeries = null;
            Int32 numberofDataPoint = 0;

            String msg = Common.AssertAverageDuration(200, 2, delegate
            {
                dataSeries = new DataSeries();
                dataSeries.RenderAs = RenderAs.Area;

                for (Int32 i = 0; i < 500; i++)
                {
                    DataPoint dataPoint = new DataPoint();
                    dataPoint.AxisXLabel = "a" + i;
                    dataPoint.YValue = rand.Next(-100, 100);
                    dataSeries.DataPoints.Add(dataPoint);
                    numberofDataPoint++;
                }
                numberOfSeries++;
                chart.Series.Add(dataSeries);
            });

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
                _htmlElement1.SetProperty("value", dataSeries.RenderAs + " chart with " + numberOfSeries + " DataSeries having " + numberofDataPoint + " DataPoints. Total Chart Loading Time: " + totalDuration + "s. Number of Render Count: " + chart.ChartArea._renderCount);
                _htmlElement2 = Common.GetDisplayMessageButton(_htmlElement2);
                _htmlElement2.SetStyleAttribute("top", "540px");
                _htmlElement2.SetProperty("value", "Total Calculation: " + msg + " Click here to exit.");
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement1);
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement2);
            });

            EnqueueDelay(_sleepTime);
            EnqueueCallback(() =>
            {
                _htmlElement2.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.HtmlElement_OnClick));
            });
        }
        #endregion

        /// <summary>
        /// Create DataSeries
        /// </summary>
        /// <returns></returns>
        private DataSeries CreateDataSeries()
        {
            Random rand = new Random();

            DataSeries dataSeries = new DataSeries();
            dataSeries.RenderAs = RenderAs.Column;

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
        /// Create DataSeries with ZValue
        /// </summary>
        /// <returns></returns>
        private DataSeries CreateDataSeries2()
        {
            Random rand = new Random();

            DataSeries dataSeries = new DataSeries();

            for (Int32 i = 0; i < 10; i++)
            {
                DataPoint dataPoint = new DataPoint();
                dataPoint.AxisXLabel = "Label" + i;
                dataPoint.YValue = rand.Next(-100, 100);
                dataPoint.ZValue = rand.Next(100, 200);
                dataSeries.DataPoints.Add(dataPoint);
            }
            return dataSeries;
        }

        /// <summary>
        /// Event handler for click event of the Html element
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HtmlElement_OnClick(object sender, System.Windows.Browser.HtmlEventArgs e)
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

            _htmlElement3.SetProperty("value", "Number of Render Count: " + _chart.ChartArea._renderCount + ". Click here to exit.");
        }

        /// <summary>
        /// Event handler for click event of the Html element
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SwitchDataSeries_OnClick(object sender, System.Windows.Browser.HtmlEventArgs e)
        {
            if (_chart.Series[0].RenderAs == RenderAs.Pie)
                _chart.Series[0].RenderAs = RenderAs.Doughnut;
            else
                _chart.Series[0].RenderAs = RenderAs.Pie;

            _htmlElement3.SetProperty("value", "Number of Render Count: " + _chart.ChartArea._renderCount + ". Click here to exit.");
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
        void chart_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            _isLoaded = true;
        }

        #region Private Data

        /// <summary>
        /// Whether DataSeries is added inside the chart
        /// </summary>
        private Boolean isDataSeriesAdded = false;

        /// <summary>
        /// Reference for Chart
        /// </summary>
        private Chart _chart;

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
        private const int _sleepTime = 2000;

        /// <summary>
        /// Whether the chart is loaded
        /// </summary>
        private bool _isLoaded = false;

        #endregion
    }
}

