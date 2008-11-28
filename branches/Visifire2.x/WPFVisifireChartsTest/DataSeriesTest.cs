using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Visifire.Charts;
using Visifire.Commons;

namespace WPFVisifireChartsTest
{
    /// <summary>
    /// Summary description for DataSeriesTest
    /// </summary>
    [TestClass]
    public class DataSeriesTest
    {
        /// <summary>
        /// Testing DataSeries Column Chart
        /// </summary>
        [TestMethod]
        public void DataSeriesColumnChecking()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;
            chart.View3D = true;

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Random rand = new Random();

            DataSeries dataSeries1 = new DataSeries();
            dataSeries1.RenderAs = RenderAs.Column;

            Int32 numberOfDataPoints = 0;
            Common.AssertAverageDuration(300, 1,
                delegate
                {
                    for (Int32 i = 0; i < 100; i++)
                    {
                        DataPoint dataPoint = new DataPoint();
                        dataPoint.XValue = i + 1;
                        dataPoint.YValue = rand.Next(-500, 500);
                        dataSeries1.DataPoints.Add(dataPoint);
                        numberOfDataPoints++;
                    }
                    chart.Series.Add(dataSeries1);
                });

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                window.Dispatcher.InvokeShutdown();
                window.Close();
            }
        }

        /// <summary>
        /// Testing DataSeries Bar Chart
        /// </summary>
        [TestMethod]
        public void DataSeriesBarChecking()
        {
            Chart chart = new Chart();
            chart.Width = 350;
            chart.Height = 450;
            chart.View3D = true;

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Random rand = new Random();

            DataSeries dataSeries1 = new DataSeries();
            dataSeries1.RenderAs = RenderAs.Bar;

            Int32 numberOfDataPoints = 0;
            Common.AssertAverageDuration(300, 1,
                delegate
                {
                    for (Int32 i = 0; i < 100; i++)
                    {
                        DataPoint dataPoint = new DataPoint();
                        dataPoint.XValue = i + 1;
                        dataPoint.YValue = rand.Next(-500, 500);
                        dataSeries1.DataPoints.Add(dataPoint);
                        numberOfDataPoints++;
                    }
                    chart.Series.Add(dataSeries1);
                });

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                window.Dispatcher.InvokeShutdown();
                window.Close();
            }
        }

        /// <summary>
        /// Testing DataSeries Event
        /// </summary>
        [TestMethod]
        public void DataSeriesEventChecking()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;
            chart.View3D = false;

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Random rand = new Random();

            DataSeries dataSeries1 = new DataSeries();
            dataSeries1.RenderAs = RenderAs.Column;

            for (Int32 i = 0; i < 20; i++)
            {
                DataPoint dataPoint = new DataPoint();
                dataPoint.XValue = i + 1;
                dataPoint.YValue = rand.Next(-500, 500);
                dataSeries1.DataPoints.Add(dataPoint);
            }

            dataSeries1.MouseLeftButtonUp += delegate(Object sender, MouseButtonEventArgs e)
            {
                MessageBox.Show("DataSeriesEventChecking:- DataSeries RenderAs: " + (sender as DataSeries).RenderAs + "MouseLeftButtonUp event fired");
            };

            dataSeries1.MouseEnter += delegate(Object sender, MouseEventArgs e)
            {
                MessageBox.Show("DataSeriesEventChecking:- DataSeries RenderAs: " + (sender as DataSeries).RenderAs + " MouseEnter event fired");
            };

            dataSeries1.MouseLeave += delegate(Object sender, MouseEventArgs e)
            {
                MessageBox.Show("DataSeriesEventChecking:- Click here to exit. MouseLeave event fired");
            };

            dataSeries1.MouseLeftButtonDown += delegate(Object sender, MouseButtonEventArgs e)
            {
                MessageBox.Show("DataSeriesEventChecking:- DataSeries RenderAs: " + (sender as DataSeries).RenderAs + " MouseLeftButtonDown event fired");
            };
            chart.Series.Add(dataSeries1);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                window.Dispatcher.InvokeShutdown();
                window.Close();
            }
        }

        /// <summary>
        /// Testing Multi DataSeries Chart
        /// </summary>
        [TestMethod]
        public void MultiSeriesChartChecking1()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;
            chart.View3D = true;

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Int32 numberOfDataPoints = 0;
            Random rand = new Random();

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
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

                DataSeries dataSeries3 = new DataSeries();
                dataSeries3.RenderAs = RenderAs.Area;
                numberOfDataPoints = 0;
                for (Int32 i = 0; i < 20; i++)
                {
                    DataPoint dataPoint = new DataPoint();
                    dataPoint.XValue = i + 1;
                    dataPoint.YValue = rand.Next(-500, 500);
                    dataSeries3.DataPoints.Add(dataPoint);
                    numberOfDataPoints++;
                }
                chart.Series.Add(dataSeries3);
                Assert.AreEqual(3, chart.Series.Count);

                DataSeries dataSeries4 = new DataSeries();
                dataSeries4.RenderAs = RenderAs.Line;
                numberOfDataPoints = 0;
                for (Int32 i = 0; i < 20; i++)
                {
                    DataPoint dataPoint = new DataPoint();
                    dataPoint.XValue = i + 1;
                    dataPoint.YValue = rand.Next(-500, 500);
                    dataSeries4.DataPoints.Add(dataPoint);
                    numberOfDataPoints++;
                }
                chart.Series.Add(dataSeries4);
                Assert.AreEqual(4, chart.Series.Count);

                DataSeries dataSeries5 = new DataSeries();
                dataSeries5.RenderAs = RenderAs.Bubble;
                numberOfDataPoints = 0;
                for (Int32 i = 0; i < 20; i++)
                {
                    DataPoint dataPoint = new DataPoint();
                    dataPoint.XValue = i + 1;
                    dataPoint.YValue = rand.Next(-500, 500);
                    dataPoint.ZValue = rand.Next(100, 500);
                    dataSeries5.DataPoints.Add(dataPoint);
                    numberOfDataPoints++;
                }
                chart.Series.Add(dataSeries5);
                Assert.AreEqual(5, chart.Series.Count);

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
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();

        }

        /// <summary>
        /// Testing Multi DataSeries Chart
        /// </summary>
        [TestMethod]
        public void MultiSeriesChartChecking2()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;
            chart.View3D = true;

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Int32 numberOfDataPoints = 0;

            Random rand = new Random();

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
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

                DataSeries dataSeries3 = new DataSeries();
                dataSeries3.RenderAs = RenderAs.StackedArea;
                numberOfDataPoints = 0;
                for (Int32 i = 0; i < 20; i++)
                {
                    DataPoint dataPoint = new DataPoint();
                    dataPoint.XValue = i + 1;
                    dataPoint.YValue = rand.Next(-500, 500);
                    dataSeries3.DataPoints.Add(dataPoint);
                    numberOfDataPoints++;
                }
                chart.Series.Add(dataSeries3);
                Assert.AreEqual(3, chart.Series.Count);

                DataSeries dataSeries4 = new DataSeries();
                dataSeries4.RenderAs = RenderAs.StackedArea;
                numberOfDataPoints = 0;
                for (Int32 i = 0; i < 20; i++)
                {
                    DataPoint dataPoint = new DataPoint();
                    dataPoint.XValue = i + 1;
                    dataPoint.YValue = rand.Next(-500, 500);
                    dataSeries4.DataPoints.Add(dataPoint);
                    numberOfDataPoints++;
                }
                chart.Series.Add(dataSeries4);
                Assert.AreEqual(4, chart.Series.Count);

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

                DataSeries dataSeries7 = new DataSeries();
                dataSeries7.RenderAs = RenderAs.Bubble;
                numberOfDataPoints = 0;
                for (Int32 i = 0; i < 20; i++)
                {
                    DataPoint dataPoint = new DataPoint();
                    dataPoint.XValue = i + 1;
                    dataPoint.YValue = rand.Next(-500, 500);
                    dataPoint.ZValue = rand.Next(100, 500);
                    dataSeries7.DataPoints.Add(dataPoint);
                    numberOfDataPoints++;
                }
                chart.Series.Add(dataSeries7);
                Assert.AreEqual(7, chart.Series.Count);

                DataSeries dataSeries8 = new DataSeries();
                dataSeries8.RenderAs = RenderAs.StackedColumn100;
                numberOfDataPoints = 0;
                for (Int32 i = 0; i < 20; i++)
                {
                    DataPoint dataPoint = new DataPoint();
                    dataPoint.XValue = i + 1;
                    dataPoint.YValue = rand.Next(-500, 500);
                    dataSeries8.DataPoints.Add(dataPoint);
                    numberOfDataPoints++;
                }
                chart.Series.Add(dataSeries8);
                Assert.AreEqual(8, chart.Series.Count);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Testing Pie/Doughnut Chart loading
        /// </summary>
        [TestMethod]
        public void PieDoughnutChartChecking()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;
            chart.View3D = true;

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Int32 numberOfDataPoints = 0;

            Random rand = new Random();

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
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
                Assert.AreEqual(2, chart.Series.Count);

                DataSeries dataSeries3 = new DataSeries();
                dataSeries3.RenderAs = RenderAs.Pie;
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
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Testing Bar MultiSeries Chart loading
        /// </summary>
        [TestMethod]
        public void MultiSeriesBarChartChecking()
        {
            Chart chart = new Chart();
            chart.Width = 350;
            chart.Height = 500;
            chart.View3D = true;

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Int32 numberOfDataPoints = 0;

            Random rand = new Random();

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                DataSeries dataSeries1 = new DataSeries();
                dataSeries1.RenderAs = RenderAs.Bar;
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

                DataSeries dataSeries2 = new DataSeries();
                dataSeries2.RenderAs = RenderAs.StackedBar;
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
                Assert.AreEqual(2, chart.Series.Count);

                DataSeries dataSeries3 = new DataSeries();
                dataSeries3.RenderAs = RenderAs.StackedBar100;
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

                DataSeries dataSeries4 = new DataSeries();
                dataSeries4.RenderAs = RenderAs.StackedBar;
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

                DataSeries dataSeries5 = new DataSeries();
                dataSeries5.RenderAs = RenderAs.Bar;
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
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Test the DataPoints collection changed event
        /// </summary>
        [TestMethod]
        public void TestDataPointsCollectionChanged()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Int32 dataPointsAdded = 0;

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

            dataSeries.DataPoints.CollectionChanged += (object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
                =>
            {
                if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
                {
                    dataPointsAdded += e.NewItems.Count;
                    Assert.AreEqual(1, e.NewItems.Count);
                }
            };

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                for (Int32 i = 0; i < 10; i++)
                {
                    DataPoint dataPoint = new DataPoint();
                    dataPoint.AxisXLabel = "Labels" + i;
                    dataPoint.XValue = i + 1;
                    dataPoint.YValue = rand.Next(-500, 500);
                    dataSeries.DataPoints.Add(dataPoint);
                    dataPointsAdded = i + 1;

                }
                chart.Series.Add(dataSeries);

                Assert.AreEqual(10, dataPointsAdded);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();

        }

        /// <summary>
        /// Check the RenderAs property value
        /// </summary>
        [TestMethod]
        public void CheckRenderAsProperty()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Random rand = new Random();

            DataSeries dataSeries = CreateDataSeries();

            chart.Series.Add(dataSeries);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                dataSeries.RenderAs = RenderAs.Column;
                Assert.AreEqual(RenderAs.Column, dataSeries.RenderAs);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();

        }

        /// <summary>
        /// Check the Color property value
        /// </summary>
        [TestMethod]
        public void CheckColorProperty()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Random rand = new Random();

            DataSeries dataSeries = CreateDataSeries();
            chart.Series.Add(dataSeries);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                dataSeries.Color = new SolidColorBrush(Colors.Cyan);
                Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Cyan), dataSeries.Color);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the LegendText property value
        /// </summary>
        [TestMethod]
        public void CheckLegendTextProperty()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Random rand = new Random();

            DataSeries dataSeries = CreateDataSeries();
            chart.Series.Add(dataSeries);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                dataSeries.LegendText = "Legend1";
                Assert.AreEqual("Legend1", dataSeries.LegendText);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the Legend property value
        /// </summary>
        [TestMethod]
        public void CheckLegendProperty()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Random rand = new Random();

            DataSeries dataSeries = CreateDataSeries();
            chart.Series.Add(dataSeries);

            Legend legend = new Legend();
            legend.SetValue(FrameworkElement.NameProperty, "Legend1");
            chart.Legends.Add(legend);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                dataSeries.Legend = "Legend1";
                Assert.AreEqual("Legend1", dataSeries.Legend);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the Bevel property value
        /// </summary>
        [TestMethod]
        public void CheckBevelProperty()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Random rand = new Random();

            DataSeries dataSeries = CreateDataSeries();
            chart.Series.Add(dataSeries);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                dataSeries.Bevel = true;
                Assert.AreEqual(true, dataSeries.Bevel);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the ColorSet property value
        /// </summary>
        [TestMethod]
        public void CheckColorSetProperty()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            DataSeries dataSeries = CreateDataSeries();
            chart.Series.Add(dataSeries);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                dataSeries.ColorSet = ColorSetNames.Visifire2;
                Assert.AreEqual("Visifire2", dataSeries.ColorSet);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the LightingEnabled property value
        /// </summary>
        [TestMethod]
        public void CheckLightingEnabledProperty()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            DataSeries dataSeries = CreateDataSeries();
            chart.Series.Add(dataSeries);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                dataSeries.LightingEnabled = true;
                Assert.AreEqual(true, dataSeries.LightingEnabled);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the LineThickness property value
        /// </summary>
        [TestMethod]
        public void CheckLineThicknessProperty()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            DataSeries dataSeries = CreateDataSeries();
            chart.Series.Add(dataSeries);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                dataSeries.LineThickness = 1;
                Assert.AreEqual(1, dataSeries.LineThickness);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the LineStyle property value
        /// </summary>
        [TestMethod]
        public void CheckLineStyleProperty()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            DataSeries dataSeries = CreateDataSeries();
            chart.Series.Add(dataSeries);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                dataSeries.LineStyle = LineStyles.Dashed;
                Assert.AreEqual(LineStyles.Dashed, dataSeries.LineStyle);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the ShadowEnabled property value
        /// </summary>
        [TestMethod]
        public void CheckShadowEnabledProperty()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            DataSeries dataSeries = CreateDataSeries();
            chart.Series.Add(dataSeries);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                dataSeries.ShadowEnabled = true;
                Assert.AreEqual(true, dataSeries.ShadowEnabled);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the ShowInLegend property value
        /// </summary>
        [TestMethod]
        public void CheckShowInLegendProperty()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            DataSeries dataSeries = CreateDataSeries();
            chart.Series.Add(dataSeries);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                dataSeries.ShowInLegend = true;
                Assert.AreEqual(true, dataSeries.ShowInLegend);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the StartAngle property value
        /// </summary>
        [TestMethod]
        public void CheckStartAngleProperty()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            DataSeries dataSeries = CreateDataSeries();
            dataSeries.RenderAs = RenderAs.Pie;
            chart.Series.Add(dataSeries);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                dataSeries.StartAngle = 90;
                Assert.AreEqual(90, dataSeries.StartAngle);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the XValueFormatString property value
        /// </summary>
        [TestMethod]
        public void CheckXValueFormatStringProperty()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            DataSeries dataSeries = CreateDataSeries();
            chart.Series.Add(dataSeries);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                dataSeries.XValueFormatString = "#,#.0#";
                Assert.AreEqual("#,#.0#", dataSeries.XValueFormatString);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the YValueFormatString property value
        /// </summary>
        [TestMethod]
        public void CheckYValueFormatStringProperty()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            DataSeries dataSeries = CreateDataSeries();
            chart.Series.Add(dataSeries);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                dataSeries.YValueFormatString = "#,#.0#'%'";
                Assert.AreEqual("#,#.0#'%'", dataSeries.YValueFormatString);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the ZValueFormatString property value
        /// </summary>
        [TestMethod]
        public void CheckZValueFormatStringProperty()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            DataSeries dataSeries = CreateDataSeries2();
            dataSeries.RenderAs = RenderAs.Bubble;
            chart.Series.Add(dataSeries);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                dataSeries.ZValueFormatString = "#,#.0#'%'";
                Assert.AreEqual("#,#.0#'%'", dataSeries.ZValueFormatString);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the ToolTipText property value
        /// </summary>
        [TestMethod]
        public void CheckToolTipTextProperty()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            DataSeries dataSeries = CreateDataSeries();
            chart.Series.Add(dataSeries);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                dataSeries.ToolTipText = "#AxisLabel";
                Assert.AreEqual("#AxisLabel", dataSeries.ToolTipText);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the AxisYType property value
        /// </summary>
        [TestMethod]
        public void CheckAxisYTypeProperty()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            DataSeries dataSeries = CreateDataSeries();
            chart.Series.Add(dataSeries);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                dataSeries.AxisYType = AxisTypes.Secondary;
                Assert.AreEqual(AxisTypes.Secondary, dataSeries.AxisYType);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the LabelEnabled property value
        /// </summary>
        [TestMethod]
        public void CheckLabelEnabledProperty()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            DataSeries dataSeries = CreateDataSeries();
            chart.Series.Add(dataSeries);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                dataSeries.LabelEnabled = true;
                Assert.AreEqual(true, dataSeries.LabelEnabled);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the LabelText property value
        /// </summary>
        [TestMethod]
        public void CheckLabelTextProperty()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            DataSeries dataSeries = CreateDataSeries();
            dataSeries.LabelEnabled = true;
            chart.Series.Add(dataSeries);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                dataSeries.LabelText = "Visifire";
                Assert.AreEqual("Visifire", dataSeries.LabelText);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the LabelFontFamily property value
        /// </summary>
        [TestMethod]
        public void CheckLabelFontFamilyProperty()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            DataSeries dataSeries = CreateDataSeries();
            dataSeries.LabelEnabled = true;
            chart.Series.Add(dataSeries);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                dataSeries.LabelFontFamily = new FontFamily("MS Trebuchet");
                Assert.AreEqual(new FontFamily("MS Trebuchet"), dataSeries.LabelFontFamily);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the LabelFontSize property value
        /// </summary>
        [TestMethod]
        public void CheckLabelFontSizeProperty()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            DataSeries dataSeries = CreateDataSeries();
            dataSeries.LabelEnabled = true;
            chart.Series.Add(dataSeries);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                dataSeries.LabelFontSize = 14;
                Assert.AreEqual(14, (Double)dataSeries.LabelFontSize, Common.HighPrecisionDelta);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the LabelFontColor property value
        /// </summary>
        [TestMethod]
        public void CheckLabelFontColorProperty()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            DataSeries dataSeries = CreateDataSeries();
            dataSeries.LabelEnabled = true;
            chart.Series.Add(dataSeries);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                dataSeries.LabelFontColor = new SolidColorBrush(Colors.DarkGray);
                Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.DarkGray), dataSeries.LabelFontColor);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the LabelFontWeight property value
        /// </summary>
        [TestMethod]
        public void CheckLabelFontWeightProperty()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            DataSeries dataSeries = CreateDataSeries();
            dataSeries.LabelEnabled = true;
            chart.Series.Add(dataSeries);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                dataSeries.LabelFontWeight = FontWeights.Bold;
                Assert.AreEqual(FontWeights.Bold, dataSeries.LabelFontWeight);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the LabelFontStyle property value
        /// </summary>
        [TestMethod]
        public void CheckLabelFontStyleProperty()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            DataSeries dataSeries = CreateDataSeries();
            dataSeries.LabelEnabled = true;
            chart.Series.Add(dataSeries);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                dataSeries.LabelFontStyle = FontStyles.Italic;
                Assert.AreEqual(FontStyles.Italic, dataSeries.LabelFontStyle);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the LabelBackground property value
        /// </summary>
        [TestMethod]
        public void CheckLabelBackgroundProperty()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            DataSeries dataSeries = CreateDataSeries();
            dataSeries.LabelEnabled = true;
            chart.Series.Add(dataSeries);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                dataSeries.LabelBackground = new SolidColorBrush(Colors.Green);
                Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Green), dataSeries.LabelBackground);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the LabelStyle property value
        /// </summary>
        [TestMethod]
        public void CheckLabelStyleProperty()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            DataSeries dataSeries = CreateDataSeries();
            dataSeries.LabelEnabled = true;
            chart.Series.Add(dataSeries);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                dataSeries.LabelStyle = LabelStyles.Inside;
                Assert.AreEqual(LabelStyles.Inside, dataSeries.LabelStyle);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the LabelLineEnabled property value
        /// </summary>
        [TestMethod]
        public void CheckLabelLineEnabledProperty()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            DataSeries dataSeries = CreateDataSeries();
            dataSeries.RenderAs = RenderAs.Pie;
            chart.Series.Add(dataSeries);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                dataSeries.LabelLineEnabled = false;
                Assert.IsFalse((Boolean)dataSeries.LabelLineEnabled);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the LabelLineColor property value
        /// </summary>
        [TestMethod]
        public void CheckLabelLineColorProperty()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            DataSeries dataSeries = CreateDataSeries();
            dataSeries.RenderAs = RenderAs.Pie;
            chart.Series.Add(dataSeries);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                dataSeries.LabelLineColor = new SolidColorBrush(Colors.Magenta);
                Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Magenta), dataSeries.LabelLineColor);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the LabelLineThickness property value
        /// </summary>
        [TestMethod]
        public void CheckLabelLineThicknessProperty()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            DataSeries dataSeries = CreateDataSeries();
            dataSeries.RenderAs = RenderAs.Pie;

            chart.Series.Add(dataSeries);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                dataSeries.LabelLineThickness = 2;
                Assert.AreEqual(2, dataSeries.LabelLineThickness);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the LabelLineStyle property value
        /// </summary>
        [TestMethod]
        public void CheckLabelLineStyleProperty()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            DataSeries dataSeries = CreateDataSeries();
            dataSeries.RenderAs = RenderAs.Pie;

            chart.Series.Add(dataSeries);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                dataSeries.LabelLineStyle = LineStyles.Dashed;
                Assert.AreEqual(LineStyles.Dashed, dataSeries.LabelLineStyle);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the MarkerEnabled property value
        /// </summary>
        [TestMethod]
        public void CheckMarkerEnabledProperty()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            DataSeries dataSeries = CreateDataSeries();
            chart.Series.Add(dataSeries);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                dataSeries.MarkerEnabled = true;
                Assert.IsTrue((Boolean)dataSeries.MarkerEnabled);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the MarkerType property value
        /// </summary>
        [TestMethod]
        public void CheckMarkerTypeProperty()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            DataSeries dataSeries = CreateDataSeries();
            dataSeries.RenderAs = RenderAs.Line;

            chart.Series.Add(dataSeries);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                dataSeries.MarkerType = MarkerTypes.Diamond;
                Assert.AreEqual(MarkerTypes.Diamond, dataSeries.MarkerType);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the MarkerBorderThickness property value
        /// </summary>
        [TestMethod]
        public void CheckMarkerBorderThicknessProperty()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            DataSeries dataSeries = CreateDataSeries();
            dataSeries.RenderAs = RenderAs.Line;
            chart.Series.Add(dataSeries);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                dataSeries.MarkerBorderThickness = new Thickness(1);
                Assert.AreEqual(new Thickness(1), dataSeries.MarkerBorderThickness);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the MarkerSize property value
        /// </summary>
        [TestMethod]
        public void CheckMarkerSizeProperty()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            DataSeries dataSeries = CreateDataSeries();
            dataSeries.RenderAs = RenderAs.Line;
            chart.Series.Add(dataSeries);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                dataSeries.MarkerSize = 10;
                Assert.AreEqual(10, (Double)dataSeries.MarkerSize);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the MarkerColor property value
        /// </summary>
        [TestMethod]
        public void CheckMarkerColorProperty()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            DataSeries dataSeries = CreateDataSeries();
            dataSeries.RenderAs = RenderAs.Line;
            chart.Series.Add(dataSeries);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                dataSeries.MarkerColor = new SolidColorBrush(Colors.Red);
                Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Red), dataSeries.MarkerColor);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the MarkerBorderColor property value
        /// </summary>
        [TestMethod]
        public void CheckMarkerBorderColorProperty()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            DataSeries dataSeries = CreateDataSeries();
            dataSeries.RenderAs = RenderAs.Line;
            chart.Series.Add(dataSeries);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                dataSeries.MarkerBorderColor = new SolidColorBrush(Colors.Purple);
                Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Purple), dataSeries.MarkerBorderColor);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the MarkerScale property value
        /// </summary>
        [TestMethod]
        public void CheckMarkerScaleProperty()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            DataSeries dataSeries = CreateDataSeries();
            dataSeries.RenderAs = RenderAs.Line;
            chart.Series.Add(dataSeries);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                dataSeries.MarkerScale = 6;
                Assert.AreEqual(6, dataSeries.MarkerScale);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the RadiusX/RadiusY property value
        /// </summary>
        [TestMethod]
        public void CheckRadiusXYRadiusPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            DataSeries dataSeries = CreateDataSeries();
            dataSeries.BorderThickness = new Thickness(1);
            chart.Series.Add(dataSeries);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                dataSeries.RadiusX = new CornerRadius(5, 5, 5, 5);
                dataSeries.RadiusY = new CornerRadius(5, 5, 5, 5);
                Assert.AreEqual(new CornerRadius(5, 5, 5, 5), dataSeries.RadiusX);
                Assert.AreEqual(new CornerRadius(5, 5, 5, 5), dataSeries.RadiusY);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the ZIndex property value
        /// </summary>
        [TestMethod]
        public void CheckZIndexProperty()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;
            chart.View3D = true;

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            DataSeries dataSeries1 = CreateDataSeries();
            dataSeries1.RenderAs = RenderAs.Area;
            dataSeries1.Color = new SolidColorBrush(Colors.Orange);

            DataSeries dataSeries2 = CreateDataSeries();
            dataSeries1.RenderAs = RenderAs.Area;
            dataSeries1.Color = new SolidColorBrush(Colors.Red);
            chart.Series.Add(dataSeries1);
            chart.Series.Add(dataSeries2);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                dataSeries1.ZIndex = 10;
                Assert.AreEqual(10, dataSeries1.ZIndex);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the AxisYType default property value
        /// </summary>
        [TestMethod]
        public void CheckAxisYTypeDefaultPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            DataSeries dataSeries = CreateDataSeries();

            chart.Series.Add(dataSeries);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                Assert.AreEqual(AxisTypes.Primary, dataSeries.AxisYType);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the Bevel default property value
        /// </summary>
        [TestMethod]
        public void CheckBevelDefaultPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            DataSeries dataSeries = CreateDataSeries();

            chart.Series.Add(dataSeries);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                Assert.IsTrue(dataSeries.Bevel);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the LightingEnabled default property value
        /// </summary>
        [TestMethod]
        public void CheckLightingEnabledDefaultPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            DataSeries dataSeries = CreateDataSeries();

            chart.Series.Add(dataSeries);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                Assert.IsTrue((Boolean)dataSeries.LightingEnabled);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the ShadowEnabled default property value
        /// </summary>
        [TestMethod]
        public void CheckShadowEnabledDefaultPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            DataSeries dataSeries = CreateDataSeries();

            chart.Series.Add(dataSeries);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                Assert.IsFalse(dataSeries.ShadowEnabled);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the ShowInLegend single series default property value
        /// </summary>
        [TestMethod]
        public void CheckShowInLegendSingleSeriesDefaultPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            DataSeries dataSeries = CreateDataSeries();

            chart.Series.Add(dataSeries);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                Assert.IsFalse((Boolean)dataSeries.ShowInLegend);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the ShowInLegend Multiple series default property value
        /// </summary>
        [TestMethod]
        public void CheckShowInLegendMultipleSeriesDefaultPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            DataSeries dataSeries1 = CreateDataSeries();
            DataSeries dataSeries2 = CreateDataSeries();

            chart.Series.Add(dataSeries1);
            chart.Series.Add(dataSeries2);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                Assert.IsTrue((Boolean)dataSeries1.ShowInLegend);
                Assert.IsTrue((Boolean)dataSeries2.ShowInLegend);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the LineThickness default property value
        /// </summary>
        [TestMethod]
        public void CheckLineThicknessDefaultPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            DataSeries dataSeries = CreateDataSeries();
            dataSeries.RenderAs = RenderAs.Line;

            chart.Series.Add(dataSeries);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                Assert.AreEqual(((chart.ActualWidth * chart.ActualHeight) + 25000) / 35000, (Double)dataSeries.LineThickness, Common.HighPrecisionDelta);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the LineStyle default property value
        /// </summary>
        [TestMethod]
        public void CheckLineStyleDefaultPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            DataSeries dataSeries = CreateDataSeries();
            dataSeries.RenderAs = RenderAs.Line;

            chart.Series.Add(dataSeries);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                Assert.AreEqual(LineStyles.Solid, dataSeries.LineStyle);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the StartAngle default property value
        /// </summary>
        [TestMethod]
        public void CheckStartAngleDefaultPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            DataSeries dataSeries = CreateDataSeries();
            dataSeries.RenderAs = RenderAs.Pie;

            chart.Series.Add(dataSeries);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                Assert.AreEqual(0, dataSeries.StartAngle);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the BorderColor default property value
        /// </summary>
        [TestMethod]
        public void CheckBorderColorDefaultPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            DataSeries dataSeries = CreateDataSeries();

            chart.Series.Add(dataSeries);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                Assert.IsNull(dataSeries.BorderColor);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the BorderThickness default property value
        /// </summary>
        [TestMethod]
        public void CheckBorderThicknessDefaultPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;
            chart.View3D = false;

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            DataSeries dataSeries = CreateDataSeries();

            chart.Series.Add(dataSeries);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                Assert.AreEqual(new Thickness(0), dataSeries.BorderThickness);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the LabelBackground default property value
        /// </summary>
        [TestMethod]
        public void CheckLabelBackgroundDefaultPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            DataSeries dataSeries = CreateDataSeries();

            chart.Series.Add(dataSeries);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                Assert.IsNull(dataSeries.LabelBackground);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the LabelEnabled default property value
        /// </summary>
        [TestMethod]
        public void CheckLabelEnabledDefaultPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            DataSeries dataSeries = CreateDataSeries();

            chart.Series.Add(dataSeries);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                Assert.IsFalse((Boolean)dataSeries.LabelEnabled);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the LabelFontColor default property value
        /// </summary>
        [TestMethod]
        public void CheckLabelFontColorDefaultPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            DataSeries dataSeries = CreateDataSeries();

            chart.Series.Add(dataSeries);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                Assert.IsNull(dataSeries.LabelFontColor);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the LabelFontFamily default property value
        /// </summary>
        [TestMethod]
        public void CheckLabelFontFamilyDefaultPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            DataSeries dataSeries = CreateDataSeries();

            chart.Series.Add(dataSeries);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                Assert.AreEqual(new FontFamily("Arial"), dataSeries.LabelFontFamily);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the LabelFontSize default property value
        /// </summary>
        [TestMethod]
        public void CheckLabelFontSizeDefaultPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            DataSeries dataSeries = CreateDataSeries();

            chart.Series.Add(dataSeries);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                Assert.AreEqual(10, dataSeries.LabelFontSize);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the LabelFontStyle default property value
        /// </summary>
        [TestMethod]
        public void CheckLabelFontStyleDefaultPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            DataSeries dataSeries = CreateDataSeries();

            chart.Series.Add(dataSeries);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                Assert.AreEqual(FontStyles.Normal, dataSeries.LabelFontStyle);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the LabelFontStyle default property value
        /// </summary>
        [TestMethod]
        public void CheckLabelFontWeightDefaultPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            DataSeries dataSeries = CreateDataSeries();

            chart.Series.Add(dataSeries);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                Assert.AreEqual(FontWeights.Normal, dataSeries.LabelFontWeight);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the LabelLineEnabled default property value
        /// </summary>
        [TestMethod]
        public void CheckLabelLineEnabledDefaultPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            DataSeries dataSeries = CreateDataSeries();
            dataSeries.RenderAs = RenderAs.Pie;

            chart.Series.Add(dataSeries);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                Assert.IsTrue((Boolean)dataSeries.LabelLineEnabled);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the LabelLineColor default property value
        /// </summary>
        [TestMethod]
        public void CheckLabelLineColorDefaultPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            DataSeries dataSeries = CreateDataSeries();
            dataSeries.RenderAs = RenderAs.Pie;

            chart.Series.Add(dataSeries);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Gray), dataSeries.LabelLineColor);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the LabelLineStyle default property value
        /// </summary>
        [TestMethod]
        public void CheckLabelLineStyleDefaultPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            DataSeries dataSeries = CreateDataSeries();
            dataSeries.RenderAs = RenderAs.Pie;

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                Assert.AreEqual(LineStyles.Solid, dataSeries.LabelLineStyle);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the LabelLineThickness default property value
        /// </summary>
        [TestMethod]
        public void CheckLabelLineThicknessDefaultPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            DataSeries dataSeries = CreateDataSeries();
            dataSeries.RenderAs = RenderAs.Pie;

            chart.Series.Add(dataSeries);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                Assert.AreEqual(0, dataSeries.LabelLineThickness);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the LabelStyle default property value
        /// </summary>
        [TestMethod]
        public void CheckLabelStyleDefaultPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            DataSeries dataSeries = CreateDataSeries();
            dataSeries.LabelEnabled = true;
            chart.Series.Add(dataSeries);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                Assert.AreEqual(LabelStyles.OutSide, dataSeries.LabelStyle);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the LabelStyle StackedColumn100 default property value
        /// </summary>
        [TestMethod]
        public void CheckLabelStyleStackedColumn100DefaultPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            DataSeries dataSeries = CreateDataSeries();
            dataSeries.RenderAs = RenderAs.StackedColumn100;
            dataSeries.LabelEnabled = true;
            chart.Series.Add(dataSeries);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                Assert.AreEqual(LabelStyles.Inside, dataSeries.LabelStyle);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the MarkerBorderColor default property value
        /// </summary>
        [TestMethod]
        public void CheckMarkerBorderColorDefaultPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            DataSeries dataSeries = CreateDataSeries();
            dataSeries.RenderAs = RenderAs.Line;

            chart.Series.Add(dataSeries);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                Assert.IsNull(dataSeries.MarkerBorderColor);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the MarkerBorderThickness default property value
        /// </summary>
        [TestMethod]
        public void CheckMarkerBorderThicknessDefaultPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            DataSeries dataSeries = CreateDataSeries();
            dataSeries.RenderAs = RenderAs.Line;

            chart.Series.Add(dataSeries);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                Assert.IsNull(dataSeries.MarkerBorderThickness);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the MarkerEnabled default property value
        /// </summary>
        [TestMethod]
        public void CheckMarkerEnabledDefaultPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            DataSeries dataSeries = CreateDataSeries();
            dataSeries.RenderAs = RenderAs.Line;

            chart.Series.Add(dataSeries);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                Assert.IsTrue((Boolean)dataSeries.MarkerEnabled);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the MarkerScale default property value
        /// </summary>
        [TestMethod]
        public void CheckMarkerScaleDefaultPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            DataSeries dataSeries = CreateDataSeries();
            dataSeries.RenderAs = RenderAs.Line;

            chart.Series.Add(dataSeries);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                Assert.AreEqual(1, dataSeries.MarkerScale);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the MarkerSize default property value
        /// </summary>
        [TestMethod]
        public void CheckMarkerSizeDefaultPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            DataSeries dataSeries = CreateDataSeries();
            dataSeries.RenderAs = RenderAs.Line;

            chart.Series.Add(dataSeries);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                Assert.AreEqual((Double)(dataSeries.LineThickness + (dataSeries.LineThickness * 80 / 100)), (Double)dataSeries.MarkerSize);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the MarkerType default property value
        /// </summary>
        [TestMethod]
        public void CheckMarkerTypeDefaultPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            DataSeries dataSeries = CreateDataSeries();
            dataSeries.RenderAs = RenderAs.Line;

            chart.Series.Add(dataSeries);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                Assert.AreEqual(MarkerTypes.Circle, dataSeries.MarkerType);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the RadiusX/RadiusY default property value
        /// </summary>
        [TestMethod]
        public void CheckRadiusXYDefaultPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            DataSeries dataSeries = CreateDataSeries();

            chart.Series.Add(dataSeries);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                Assert.AreEqual(new CornerRadius(0, 0, 0, 0), dataSeries.RadiusX);
                Assert.AreEqual(new CornerRadius(0, 0, 0, 0), dataSeries.RadiusY);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the ToolTipText default property value
        /// </summary>
        [TestMethod]
        public void CheckToolTipTextDefaultPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            DataSeries dataSeries = CreateDataSeries();

            chart.Series.Add(dataSeries);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                Assert.AreEqual("#AxisXLabel, #YValue", dataSeries.ToolTipText);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Performance and Stress
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public void DataSeriesStressTest()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Random rand = new Random();

            Int32 numberOfSeries = 0;
            Int32 numberofDataPoint = 0;
            Common.AssertAverageDuration(500, 2, delegate
            {
                DataSeries dataSeries = new DataSeries();
                dataSeries.RenderAs = RenderAs.Column;

                for (Int32 i = 0; i < 2500; i++)
                {
                    DataPoint dataPoint = new DataPoint();
                    dataPoint.AxisXLabel = "a" + i;
                    dataPoint.XValue = i + 1;
                    dataPoint.YValue = rand.Next(-100, 100);
                    dataSeries.DataPoints.Add(dataPoint);
                    numberofDataPoint++;
                }
                numberOfSeries++;

                chart.Series.Add(dataSeries);

            });
            
            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                window.Dispatcher.InvokeShutdown();
                window.Close();
            }
        }

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

        void chart_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            isLoaded = true;
        }

        #region Private Data

        bool isLoaded = false;
        const int sleepTime = 2000;

        #endregion
    }
}
