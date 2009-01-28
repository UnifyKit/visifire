using System;
using System.Windows;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Visifire.Charts;
using Visifire.Commons;

namespace WPFVisifireChartsTest
{
    /// <summary>
    /// Summary description for DataPointTest
    /// </summary>
    [TestClass]
    public class DataPointTest
    {
        /// <summary>
        /// Test DataPoint property changed event.
        /// </summary>
        [TestMethod]
        public void TestDataPointPropertyChanged()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Random rand = new Random();

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
                    else
                        Assert.AreEqual("YValue", e.PropertyName);
                        
                };

                dataSeries.DataPoints.Add(dataPoint);
            }
            chart.Series.Add(dataSeries);
            
            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                dataPoint.XValue = 10;
                dataPoint.YValue = rand.Next(-100, 100);
            }
       
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Test DataPoints with Decimal values in the second series
        /// </summary>
        [TestMethod]
        public void DataPointDecimalXValueChecking()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            Axis axisX = new Axis();
            axisX.Interval = 1;
            chart.AxesX.Add(axisX);
                   
            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Random rand = new Random();

            DataSeries dataSeries1 = new DataSeries();

            List<Double> xList = new List<Double>();
            List<Double> yList = new List<Double>();
            Double y = 0;
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
        /// Test DataPoints with repeated XValues in the series
        /// </summary>
        [TestMethod]
        public void DataPointRepeatedXValueChecking()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            Axis axisX = new Axis();
            axisX.Interval = 1;
            chart.AxesX.Add(axisX);

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

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
        /// Stress testing DataPoints with hundreds of XValues
        /// </summary>
        [TestMethod]
        public void DataPointStressXValueChecking()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            Axis axisX = new Axis();
            axisX.Interval = 1;
            chart.AxesX.Add(axisX);

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Random rand = new Random();

            DataSeries dataSeries1 = new DataSeries();

            Int32 numberOfDataPoints = 0;

            Common.AssertAverageDuration(200, 2, delegate
            {
                for (Int32 i = 0; i < 250; i++)
                {
                    DataPoint dataPoint = new DataPoint();
                    dataPoint.XValue = i + 1;
                    dataPoint.YValue = rand.Next(-500, 500);
                    dataSeries1.DataPoints.Add(dataPoint);
                    numberOfDataPoints++;
                }
            });

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
        /// Performance testing DataPoints with decimal XValues
        /// </summary>
        [TestMethod]
        public void DataPointDecimalXValueChecking2()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            Axis axisX = new Axis();
            axisX.Interval = 1;
            chart.AxesX.Add(axisX);

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

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
        /// Performance testing DataPoints with AxisXLabels
        /// </summary>
        [TestMethod]
        public void DataPointAxisXLabelChecking()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            Axis axisX = new Axis();
            axisX.Interval = 1;
            chart.AxesX.Add(axisX);

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

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
        /// Performance testing DataPoints with random XValues
        /// </summary>
        [TestMethod]
        public void DataPointRandomXValuesChecking()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            isLoaded = false;
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
        /// Stress testing DataPoints with thousands of XValues
        /// </summary>
        [TestMethod]
        public void DataPointStressXValuesChecking2()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;
            chart.View3D = true;

            Axis axisX = new Axis();
            axisX.Interval = 1;
            chart.AxesX.Add(axisX);

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Random rand = new Random();

            DataSeries dataSeries1 = new DataSeries();
            dataSeries1.RenderAs = RenderAs.Area;

            Int32 numberOfDataPoints = 0;

            String msg = Common.AssertAverageDuration(300, 2, delegate
            {
                for (Int32 i = 0; i < 2500; i++)
                {
                    DataPoint dataPoint = new DataPoint();
                    dataPoint.XValue = i + 1;
                    dataPoint.YValue = Math.Sin((Math.PI * 2) * i / 2500) * 10;
                    dataSeries1.DataPoints.Add(dataPoint);
                    numberOfDataPoints++;
                }
            });

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
        /// Testing DataPoints with YValues
        /// </summary>
        [TestMethod]
        public void DataPointStressYValuesChecking()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Random rand = new Random();

            DataSeries dataSeries1 = new DataSeries();

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
        /// Testing DataPoint Event checking
        /// </summary>
        [TestMethod]
        public void DataPointEventChecking()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

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
                    MessageBox.Show("DataPointEventChecking:- DataPoint YValue: " + (sender as DataPoint).YValue + " MouseLeftButtonUp event fired");
                };

                dataPoint.MouseEnter += delegate(Object sender, MouseEventArgs e)
                {
                    MessageBox.Show("DataPointEventChecking:- DataPoint YValue: " + (sender as DataPoint).YValue + " MouseEnter event fired");
                };

                dataPoint.MouseLeave += delegate(Object sender, MouseEventArgs e)
                {
                    MessageBox.Show("DataPointEventChecking:- Click here to exit. MouseLeave event fired");
                };

                dataPoint.MouseLeftButtonDown += delegate(Object sender, MouseButtonEventArgs e)
                {
                    MessageBox.Show("DataPointEventChecking:- DataPoint YValue: " + (sender as DataPoint).YValue + " MouseLeftButtonDown event fired");
                };

                dataSeries.DataPoints.Add(dataPoint);
            }

            chart.Series.Add(dataSeries);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                System.Threading.Thread.Sleep(1000);
                window.Dispatcher.InvokeShutdown();
                window.Close();
            }
        }

        #region CheckNewPropertyValue
        /// <summary>
        /// Check the Color property value
        /// </summary>
        [TestMethod]
        public void CheckColorPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            DataSeries dataSeries = CreateDataSeries2();
            Random rand = new Random();
            Double r, g, b;
                   
            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                for (Int32 i = 0; i < 10; i++)
                {
                    dataSeries.DataPoints[i].Color = new SolidColorBrush(Color.FromArgb((Byte)255, (Byte)(r = rand.Next(0, 100)), (Byte)(g = rand.Next(0, 100)), (Byte)(b = rand.Next(0, 100))));
                    Common.AssertBrushesAreEqual(new SolidColorBrush(Color.FromArgb((Byte)255, (Byte)r, (Byte)g, (Byte)b)), dataSeries.DataPoints[i].Color);
                }
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the Exploded property value
        /// </summary>
        [TestMethod]
        public void CheckExplodedPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            DataSeries dataSeries = CreateDataSeries2();
            dataSeries.RenderAs = RenderAs.Pie;

            chart.Series.Add(dataSeries);
                   
            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                for (Int32 i = 0; i < 10; i++)
                {
                    dataSeries.DataPoints[i].Exploded = true;
                    Assert.IsTrue((Boolean)dataSeries.DataPoints[i].Exploded);
                }
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the LabelEnabled property value
        /// </summary>
        [TestMethod]
        public void CheckLabelEnabledPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            DataSeries dataSeries = CreateDataSeries2();

            chart.Series.Add(dataSeries);
                   
            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                for (Int32 i = 0; i < 10; i++)
                {
                    dataSeries.DataPoints[i].LabelEnabled = false;
                    Assert.IsFalse((Boolean)dataSeries.DataPoints[i].LabelEnabled);
                }
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the LabelText property value
        /// </summary>
        [TestMethod]
        public void CheckLabelTextPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            DataSeries dataSeries = CreateDataSeries2();
            dataSeries.LabelEnabled = true;
            chart.Series.Add(dataSeries);
                   
            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                for (Int32 i = 0; i < 10; i++)
                {
                    dataSeries.DataPoints[i].LabelText = "Label"+i;
                    Assert.AreEqual("Label"+i, dataSeries.DataPoints[i].LabelText);
                }
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the LabelFontFamily property value
        /// </summary>
        [TestMethod]
        public void CheckLabelFontFamilyPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            DataSeries dataSeries = CreateDataSeries2();
            dataSeries.LabelEnabled = true;
            chart.Series.Add(dataSeries);
                   
            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                for (Int32 i = 0; i < 10; i++)
                {
                    dataSeries.DataPoints[i].LabelFontFamily = new FontFamily("MS Trebuchet");
                    Assert.AreEqual(new FontFamily("MS Trebuchet"), dataSeries.DataPoints[i].LabelFontFamily);
                }
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the LabelFontSize property value
        /// </summary>
        [TestMethod]
        public void CheckLabelFontSizePropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            DataSeries dataSeries = CreateDataSeries2();
            dataSeries.LabelEnabled = true;
            chart.Series.Add(dataSeries);
                   
            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                for (Int32 i = 0; i < 10; i++)
                {
                    dataSeries.DataPoints[i].LabelFontSize = 12;
                    Assert.AreEqual(12, dataSeries.DataPoints[i].LabelFontSize);
                }
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the LabelFontColor property value
        /// </summary>
        [TestMethod]
        public void CheckLabelFontColorPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            DataSeries dataSeries = CreateDataSeries2();
            dataSeries.LabelEnabled = true;
            chart.Series.Add(dataSeries);
                   
            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                for (Int32 i = 0; i < 10; i++)
                {
                    dataSeries.DataPoints[i].LabelFontColor = new SolidColorBrush(Colors.Blue);
                    Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Blue), dataSeries.DataPoints[i].LabelFontColor);
                }
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the LabelFontWeight property value
        /// </summary>
        [TestMethod]
        public void CheckLabelFontWeightPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            DataSeries dataSeries = CreateDataSeries2();
            dataSeries.LabelEnabled = true;
            chart.Series.Add(dataSeries);
                   
            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                for (Int32 i = 0; i < 10; i++)
                {
                    dataSeries.DataPoints[i].LabelFontWeight = FontWeights.ExtraBold;
                    Assert.AreEqual(FontWeights.ExtraBold, dataSeries.DataPoints[i].LabelFontWeight);
                }
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the LabelFontStyle property value
        /// </summary>
        [TestMethod]
        public void CheckLabelFontStylePropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            DataSeries dataSeries = CreateDataSeries2();
            dataSeries.LabelEnabled = true;
            chart.Series.Add(dataSeries);
                   
            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                for (Int32 i = 0; i < 10; i++)
                {
                    dataSeries.DataPoints[i].LabelFontStyle = FontStyles.Italic;
                    Assert.AreEqual(FontStyles.Italic, dataSeries.DataPoints[i].LabelFontStyle);
                }
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the LabelBackground property value
        /// </summary>
        [TestMethod]
        public void CheckLabelBackgroundPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            DataSeries dataSeries = CreateDataSeries2();
            dataSeries.LabelEnabled = true;
            chart.Series.Add(dataSeries);
                   
            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                for (Int32 i = 0; i < 10; i++)
                {
                    dataSeries.DataPoints[i].LabelBackground = new SolidColorBrush(Colors.DarkGray);
                    Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.DarkGray), dataSeries.DataPoints[i].LabelBackground);
                }
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the LabelStyle property value
        /// </summary>
        [TestMethod]
        public void CheckLabelStylePropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            DataSeries dataSeries = CreateDataSeries2();
            dataSeries.LabelEnabled = true;
            chart.Series.Add(dataSeries);
                   
            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                for (Int32 i = 0; i < 10; i++)
                {
                    dataSeries.DataPoints[i].LabelStyle = LabelStyles.Inside;
                    Assert.AreEqual(LabelStyles.Inside, dataSeries.DataPoints[i].LabelStyle);
                }
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the LabelLineEnabled property value
        /// </summary>
        [TestMethod]
        public void CheckLabelLineEnabledPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            DataSeries dataSeries = CreateDataSeries2();
            dataSeries.RenderAs = RenderAs.Pie;

            chart.Series.Add(dataSeries);
                   
            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                for (Int32 i = 0; i < 10; i++)
                {
                    dataSeries.DataPoints[i].LabelLineEnabled = false;
                    Assert.IsFalse((Boolean)dataSeries.DataPoints[i].LabelLineEnabled);
                }
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the LabelLineColor property value
        /// </summary>
        [TestMethod]
        public void CheckLabelLineColorPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            DataSeries dataSeries = CreateDataSeries2();
            dataSeries.RenderAs = RenderAs.Pie;

            chart.Series.Add(dataSeries);
                   
            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                for (Int32 i = 0; i < 10; i++)
                {
                    dataSeries.DataPoints[i].LabelLineColor = new SolidColorBrush(Colors.Cyan);
                    Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Cyan), dataSeries.DataPoints[i].LabelLineColor);
                }
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the LabelLineThickness property value
        /// </summary>
        [TestMethod]
        public void CheckLabelLineThicknessPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            DataSeries dataSeries = CreateDataSeries2();
            dataSeries.RenderAs = RenderAs.Pie;

            chart.Series.Add(dataSeries);
                   
            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                for (Int32 i = 0; i < 10; i++)
                {
                    dataSeries.DataPoints[i].LabelLineThickness = 2;
                    Assert.AreEqual(2, dataSeries.DataPoints[i].LabelLineThickness);
                }
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the LabelLineStyle property value
        /// </summary>
        [TestMethod]
        public void CheckLabelLineStylePropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            DataSeries dataSeries = CreateDataSeries2();
            dataSeries.RenderAs = RenderAs.Pie;

            chart.Series.Add(dataSeries);
                   
            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                for (Int32 i = 0; i < 10; i++)
                {
                    dataSeries.DataPoints[i].LabelLineStyle = LineStyles.Dashed;
                    Assert.AreEqual(LineStyles.Dashed, dataSeries.DataPoints[i].LabelLineStyle);
                }
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the MarkerEnabled property value
        /// </summary>
        [TestMethod]
        public void CheckMarkerEnabledPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            DataSeries dataSeries = CreateDataSeries2();

            chart.Series.Add(dataSeries);
                   
            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                for (Int32 i = 0; i < 10; i++)
                {
                    dataSeries.DataPoints[i].MarkerEnabled = true;
                    Assert.IsTrue((Boolean)dataSeries.DataPoints[i].MarkerEnabled);
                }
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the MarkerType property value
        /// </summary>
        [TestMethod]
        public void CheckMarkerTypePropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            DataSeries dataSeries = CreateDataSeries2();
            dataSeries.RenderAs = RenderAs.Line;
            chart.Series.Add(dataSeries);
                   
            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                for (Int32 i = 0; i < 10; i++)
                {
                    dataSeries.DataPoints[i].MarkerType = MarkerTypes.Diamond;
                    Assert.AreEqual(MarkerTypes.Diamond, dataSeries.DataPoints[i].MarkerType);
                }
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the MarkerBorderThickness property value
        /// </summary>
        [TestMethod]
        public void CheckMarkerBorderThicknessPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            DataSeries dataSeries = CreateDataSeries2();
            dataSeries.RenderAs = RenderAs.Line;
            chart.Series.Add(dataSeries);
                   
            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                for (Int32 i = 0; i < 10; i++)
                {
                    dataSeries.DataPoints[i].MarkerBorderColor = new SolidColorBrush(Colors.Green);
                    dataSeries.DataPoints[i].MarkerBorderThickness = new Thickness(2);
                    Assert.AreEqual(new Thickness(2), dataSeries.DataPoints[i].MarkerBorderThickness);
                }
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the MarkerBorderColor property value
        /// </summary>
        [TestMethod]
        public void CheckMarkerBorderColorPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            DataSeries dataSeries = CreateDataSeries2();
            dataSeries.RenderAs = RenderAs.Line;
            chart.Series.Add(dataSeries);
                   
            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                for (Int32 i = 0; i < 10; i++)
                {
                    dataSeries.DataPoints[i].MarkerBorderThickness = new Thickness(1);
                    dataSeries.DataPoints[i].MarkerBorderColor = new SolidColorBrush(Colors.Green);
                    Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Green), dataSeries.DataPoints[i].MarkerBorderColor);
                }
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the MarkerSize property value
        /// </summary>
        [TestMethod]
        public void CheckMarkerSizePropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            DataSeries dataSeries = CreateDataSeries2();
            dataSeries.RenderAs = RenderAs.Line;
            chart.Series.Add(dataSeries);
                   
            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                for (Int32 i = 0; i < 10; i++)
                {
                    dataSeries.DataPoints[i].MarkerSize = 10;
                    Assert.AreEqual(10, dataSeries.DataPoints[i].MarkerSize);
                }
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the MarkerColor property value
        /// </summary>
        [TestMethod]
        public void CheckMarkerColorPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            DataSeries dataSeries = CreateDataSeries2();
            dataSeries.RenderAs = RenderAs.Line;
            chart.Series.Add(dataSeries);
                   
            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                for (Int32 i = 0; i < 10; i++)
                {
                    dataSeries.DataPoints[i].MarkerColor = new SolidColorBrush(Colors.Cyan);
                    Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Cyan), dataSeries.DataPoints[i].MarkerColor);
                }
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the MarkerScale property value
        /// </summary>
        [TestMethod]
        public void CheckMarkerScalePropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            DataSeries dataSeries = CreateDataSeries2();
            dataSeries.RenderAs = RenderAs.Line;
            chart.Series.Add(dataSeries);
                   
            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                for (Int32 i = 0; i < 10; i++)
                {
                    dataSeries.DataPoints[i].MarkerScale = 5;
                    Assert.AreEqual(5, dataSeries.DataPoints[i].MarkerScale);
                }
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the ToolTipText property value
        /// </summary>
        [TestMethod]
        public void CheckToolTipTextPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            DataSeries dataSeries = CreateDataSeries2();

            chart.Series.Add(dataSeries);
                   
            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                for (Int32 i = 0; i < 10; i++)
                {
                    dataSeries.DataPoints[i].ToolTipText = "#AxisLabel";
                    Assert.AreEqual("#AxisLabel", dataSeries.DataPoints[i].ToolTipText);
                }
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the ShowInLegend property value
        /// </summary>
        [TestMethod]
        public void CheckShowInLegendPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            DataSeries dataSeries = CreateDataSeries2();

            chart.Series.Add(dataSeries);
                   
            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                for (Int32 i = 0; i < 10; i++)
                {
                    dataSeries.DataPoints[i].ShowInLegend = true;
                    Assert.IsTrue((Boolean)dataSeries.DataPoints[i].ShowInLegend);
                }
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the LegendText property value
        /// </summary>
        [TestMethod]
        public void CheckLegendTextPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            DataSeries dataSeries = CreateDataSeries2();

            chart.Series.Add(dataSeries);
                   
            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                for (Int32 i = 0; i < 10; i++)
                {
                    dataSeries.DataPoints[i].ShowInLegend = true;
                    dataSeries.DataPoints[i].LegendText = "DataPoint" + i;
                    Assert.AreEqual("DataPoint" + i, dataSeries.DataPoints[i].LegendText);
                }
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the BorderThickness property value
        /// </summary>
        [TestMethod]
        public void CheckBorderThicknessPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            DataSeries dataSeries = CreateDataSeries2();

            chart.Series.Add(dataSeries);
                   
            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                for (Int32 i = 0; i < 10; i++)
                {
                    dataSeries.DataPoints[i].BorderThickness = new Thickness(2);
                    Assert.AreEqual(new Thickness(2), dataSeries.DataPoints[i].BorderThickness);
                }
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the BorderColor property value
        /// </summary>
        [TestMethod]
        public void CheckBorderColorPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            DataSeries dataSeries = CreateDataSeries2();
            chart.Series.Add(dataSeries);
                   
            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                for (Int32 i = 0; i < 10; i++)
                {
                    dataSeries.DataPoints[i].BorderThickness = new Thickness(2);
                    dataSeries.DataPoints[i].BorderColor = new SolidColorBrush(Colors.Magenta);
                    Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Magenta), dataSeries.DataPoints[i].BorderColor);
                }
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the BorderStyle property value
        /// </summary>
        [TestMethod]
        public void CheckBorderStylePropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            DataSeries dataSeries = CreateDataSeries2();

            chart.Series.Add(dataSeries);
                   
            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                for (Int32 i = 0; i < 10; i++)
                {
                    dataSeries.DataPoints[i].BorderThickness = new Thickness(1);
                    dataSeries.DataPoints[i].BorderColor = new SolidColorBrush(Colors.Magenta);
                    dataSeries.DataPoints[i].BorderStyle = BorderStyles.Dashed;
                    Assert.AreEqual(BorderStyles.Dashed, dataSeries.DataPoints[i].BorderStyle);
                }
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the RadiusX/RadiusY property value
        /// </summary>
        [TestMethod]
        public void CheckRadiusXYPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;
            chart.AnimationEnabled = false;

            DataSeries dataSeries = CreateDataSeries2();

            chart.Series.Add(dataSeries);
                   
            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                for (Int32 i = 0; i < 10; i++)
                {
                    dataSeries.DataPoints[i].RadiusX = new CornerRadius(5, 5, 5, 5);
                    dataSeries.DataPoints[i].RadiusY = new CornerRadius(5, 5, 5, 5);
                    Assert.AreEqual(new CornerRadius(5, 5, 5, 5), dataSeries.DataPoints[i].RadiusX);
                    Assert.AreEqual(new CornerRadius(5, 5, 5, 5), dataSeries.DataPoints[i].RadiusY);
                }
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }
        #endregion

        #region CheckDefaultPropertyValue
        /// <summary>
        /// Check the ShowInLegend default property value
        /// </summary>
        [TestMethod]
        public void CheckShowInLegendDefaultPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            DataSeries dataSeries = CreateDataSeries1();

            chart.Series.Add(dataSeries);
                   
            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                for (Int32 i = 0; i < 10; i++)
                    Assert.IsFalse((Boolean)dataSeries.DataPoints[i].ShowInLegend);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }
  
        /// Check the BorderColor default property value
        /// </summary>
        [TestMethod]
        public void CheckBorderColorDefaultPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            DataSeries dataSeries = CreateDataSeries1();

            chart.Series.Add(dataSeries);
                   
            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                for (Int32 i = 0; i < 10; i++)
                    Assert.IsNull(dataSeries.DataPoints[i].BorderColor);
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

            DataSeries dataSeries = CreateDataSeries1();

            chart.Series.Add(dataSeries);
                   
            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                for (Int32 i = 0; i < 10; i++)
                {
                    Assert.AreEqual(new Thickness(0), (Thickness)dataSeries.DataPoints[i].BorderThickness);
                }
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the BorderStyle property value
        /// </summary>
        [TestMethod]
        public void CheckBorderStyleDefaultPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;
            chart.AnimationEnabled = false;

            DataSeries dataSeries = CreateDataSeries1();

            chart.Series.Add(dataSeries);
                  
            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                for (Int32 i = 0; i < 10; i++)
                {
                    Assert.AreEqual(BorderStyles.Solid, dataSeries.DataPoints[i].BorderStyle);
                }
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

            DataSeries dataSeries = CreateDataSeries1();
            dataSeries.LabelEnabled = true;

            chart.Series.Add(dataSeries);
                   
            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                for (Int32 i = 0; i < 10; i++)
                {
                    Assert.IsNull(dataSeries.DataPoints[i].LabelBackground);
                }
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

            DataSeries dataSeries = CreateDataSeries1();

            chart.Series.Add(dataSeries);
                   
            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                for (Int32 i = 0; i < 10; i++)
                {
                    Assert.IsFalse((Boolean)dataSeries.DataPoints[i].LabelEnabled);
                }
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

            DataSeries dataSeries = CreateDataSeries1();
            dataSeries.LabelEnabled = true;

            chart.Series.Add(dataSeries);
                   
            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                for (Int32 i = 0; i < 10; i++)
                {
                    Assert.IsNull(dataSeries.DataPoints[i].LabelFontColor);
                }
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

            DataSeries dataSeries = CreateDataSeries1();
            dataSeries.LabelEnabled = true;

            chart.Series.Add(dataSeries);
                   
            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                for (Int32 i = 0; i < 10; i++)
                {
                    Assert.AreEqual(new FontFamily("Arial"), dataSeries.DataPoints[i].LabelFontFamily);
                }
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

            DataSeries dataSeries = CreateDataSeries1();
            dataSeries.LabelEnabled = true;

            chart.Series.Add(dataSeries);
                   
            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                for (Int32 i = 0; i < 10; i++)
                {
                    Assert.AreEqual(10, dataSeries.DataPoints[i].LabelFontSize);
                }
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

            DataSeries dataSeries = CreateDataSeries1();
            dataSeries.LabelEnabled = true;

            chart.Series.Add(dataSeries);
                   
            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                for (Int32 i = 0; i < 10; i++)
                {
                    Assert.AreEqual(FontStyles.Normal, (FontStyle)dataSeries.DataPoints[i].LabelFontStyle);
                }
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

            DataSeries dataSeries = CreateDataSeries1();
            dataSeries.LabelEnabled = true;

            chart.Series.Add(dataSeries);
                   
            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                for (Int32 i = 0; i < 10; i++)
                {
                    Assert.AreEqual(FontWeights.Normal, (FontWeight)dataSeries.DataPoints[i].LabelFontWeight);
                }
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

            DataSeries dataSeries = CreateDataSeries1();
            dataSeries.RenderAs = RenderAs.Pie;

            chart.Series.Add(dataSeries);
                   
            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                for (Int32 i = 0; i < 10; i++)
                {
                    Assert.IsTrue((Boolean)dataSeries.DataPoints[i].LabelLineEnabled);
                }
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

            DataSeries dataSeries = CreateDataSeries1();
            dataSeries.RenderAs = RenderAs.Pie;

            chart.Series.Add(dataSeries);
                   
            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                for (Int32 i = 0; i < 10; i++)
                {
                    Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Gray), dataSeries.DataPoints[i].LabelLineColor);
                }
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

            DataSeries dataSeries = CreateDataSeries1();
            dataSeries.RenderAs = RenderAs.Pie;

            chart.Series.Add(dataSeries);
                   
            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                for (Int32 i = 0; i < 10; i++)
                {
                    Assert.AreEqual(LineStyles.Solid, dataSeries.DataPoints[i].LabelLineStyle);
                }
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the LabelLineThickness default property value
        /// </summary>
        [TestMethod]
        public void CheckLabelLineThicnessDefaultPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            DataSeries dataSeries = CreateDataSeries1();
            dataSeries.RenderAs = RenderAs.Pie;

            chart.Series.Add(dataSeries);
                   
            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                for (Int32 i = 0; i < 10; i++)
                {
                    Assert.AreEqual(0.5, dataSeries.DataPoints[i].LabelLineThickness);
                }
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

            DataSeries dataSeries = CreateDataSeries1();

            chart.Series.Add(dataSeries);
                   
            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                for (Int32 i = 0; i < 10; i++)
                {
                    Assert.AreEqual(LabelStyles.OutSide, dataSeries.DataPoints[i].LabelStyle);
                }
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

            DataSeries dataSeries = CreateDataSeries1();
            dataSeries.RenderAs = RenderAs.StackedColumn100;

            chart.Series.Add(dataSeries);
                   
            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                for (Int32 i = 0; i < 10; i++)
                {
                    Assert.AreEqual(LabelStyles.Inside, dataSeries.DataPoints[i].LabelStyle);
                }
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

            DataSeries dataSeries = CreateDataSeries1();
            dataSeries.RenderAs = RenderAs.Line;

            chart.Series.Add(dataSeries);
                   
            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                for (Int32 i = 0; i < 10; i++)
                {
                    Assert.IsNotNull(dataSeries.DataPoints[i].MarkerBorderColor);
                }
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

            DataSeries dataSeries = CreateDataSeries1();
            dataSeries.RenderAs = RenderAs.Line;

            chart.Series.Add(dataSeries);
                   
            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                for (Int32 i = 0; i < 10; i++)
                {
                    Assert.AreEqual(new Thickness((Double)dataSeries.DataPoints[i].MarkerSize / 6), dataSeries.DataPoints[i].MarkerBorderThickness);
                }
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

            DataSeries dataSeries = CreateDataSeries1();
            dataSeries.RenderAs = RenderAs.Line;

            chart.Series.Add(dataSeries);
                   
            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                for (Int32 i = 0; i < 10; i++)
                {
                    Assert.IsTrue((Boolean)dataSeries.DataPoints[i].MarkerEnabled);
                }
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

            DataSeries dataSeries = CreateDataSeries1();
            dataSeries.RenderAs = RenderAs.Line;

            chart.Series.Add(dataSeries);
                   
            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                for (Int32 i = 0; i < 10; i++)
                {
                    Assert.AreEqual(1, dataSeries.DataPoints[i].MarkerScale);
                }
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

            DataSeries dataSeries = CreateDataSeries1();
            dataSeries.RenderAs = RenderAs.Line;

            chart.Series.Add(dataSeries);
                   
            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                for (Int32 i = 0; i < 10; i++)
                {
                    Assert.AreEqual((dataSeries.LineThickness + (dataSeries.LineThickness * 80 / 100)), (Double)dataSeries.DataPoints[i].MarkerSize);
                }
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

            DataSeries dataSeries = CreateDataSeries1();
            dataSeries.RenderAs = RenderAs.Line;

            chart.Series.Add(dataSeries);
                   
            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                for (Int32 i = 0; i < 10; i++)
                {
                    Assert.AreEqual(MarkerTypes.Circle, dataSeries.DataPoints[i].MarkerType);
                }
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the RadiusX/RadiusY property value
        /// </summary>
        [TestMethod]
        public void CheckRadiusXYDefaultPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;
            chart.AnimationEnabled = false;

            DataSeries dataSeries = CreateDataSeries1();

            chart.Series.Add(dataSeries);
                   
            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                for (Int32 i = 0; i < 10; i++)
                {
                    Assert.AreEqual(new CornerRadius(0), dataSeries.DataPoints[i].RadiusX);
                    Assert.AreEqual(new CornerRadius(0), dataSeries.DataPoints[i].RadiusY);
                }
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

            DataSeries dataSeries = CreateDataSeries1();

            chart.Series.Add(dataSeries);
                   
            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                for (Int32 i = 0; i < 10; i++)
                {
                    Assert.AreEqual("#AxisXLabel, #YValue", dataSeries.DataPoints[i].ToolTipText);
                }
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }
        #endregion

        private DataSeries CreateDataSeries1()
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
