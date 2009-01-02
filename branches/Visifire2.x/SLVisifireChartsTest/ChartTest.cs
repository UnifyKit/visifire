using System;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Silverlight.Testing;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Markup;
using System.Collections.ObjectModel;
using System.Windows.Media.Animation;
using System.Collections;
using Visifire.Charts;
using Visifire.Commons;

namespace SLVisifireChartsTest
{
    [TestClass]
    public class ChartTest : SilverlightControlTest
    {

        void chart_Loaded(object sender, RoutedEventArgs e)
        {
            isLoaded = true;
        }

        #region Chart asynchronous test
        /// <summary>
        /// Testing the Chart Properties asynchronously
        /// </summary>
        [TestMethod]
        [Description("Testing the Chart properties asynchronously")]
        [Owner("[...]")]
        [Asynchronous]
        public void TestingChartPropertiesAsync()
        {
            chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Common.CreateAndAddDefaultDataSeries(chart);

            TestPanel.Children.Add(chart);

            bool isPropertyChanged = false;
            int numberOfPropertiesAdded = 0;

            EnqueueConditional(() => { return isLoaded; });
            EnqueueSleep(sleepTime);

            EnqueueCallback(() =>
            {
                chart.Background = new SolidColorBrush(Colors.Red);
                isPropertyChanged = true;
                numberOfPropertiesAdded++;
            });

            EnqueueConditional(() => { return isPropertyChanged; });
            isPropertyChanged = false;
            EnqueueSleep(sleepTime);

            EnqueueCallback(() =>
            {
                chart.LightingEnabled = true;
                isPropertyChanged = true;
                numberOfPropertiesAdded++;
            });

            EnqueueConditional(() => { return isPropertyChanged; });
            isPropertyChanged = false;
            EnqueueSleep(sleepTime);

            EnqueueCallback(() =>
            {
                chart.ShadowEnabled = true;
                isPropertyChanged = true;
                numberOfPropertiesAdded++;
            });

            EnqueueConditional(() => { return isPropertyChanged; });
            isPropertyChanged = false;
            EnqueueSleep(sleepTime);

            EnqueueCallback(() =>
            {
                chart.BorderBrush = new SolidColorBrush(Colors.Green);
                isPropertyChanged = true;
                numberOfPropertiesAdded++;
            });

            EnqueueConditional(() => { return isPropertyChanged; });
            isPropertyChanged = false;
            EnqueueSleep(sleepTime);

            EnqueueCallback(() =>
            {
                chart.BorderThickness = new Thickness(2);
                isPropertyChanged = true;
                numberOfPropertiesAdded++;
            });

            EnqueueConditional(() => { return isPropertyChanged; });
            isPropertyChanged = false;
            EnqueueSleep(sleepTime);

            EnqueueCallback(() =>
            {
                chart.BorderStyle = BorderStyles.Dashed;
                isPropertyChanged = true;
                numberOfPropertiesAdded++;
            });

            EnqueueConditional(() => { return isPropertyChanged; });
            isPropertyChanged = false;
            EnqueueSleep(sleepTime);

            EnqueueCallback(() =>
            {
                chart.CornerRadius = new CornerRadius(5);
                isPropertyChanged = true;
                numberOfPropertiesAdded++;
            });

            EnqueueConditional(() => { return isPropertyChanged; });
            isPropertyChanged = false;
            EnqueueSleep(sleepTime);

            EnqueueCallback(() =>
            {
                chart.Bevel = true;
                isPropertyChanged = true;
                numberOfPropertiesAdded++;
            });

            EnqueueConditional(() => { return isPropertyChanged; });
            isPropertyChanged = false;
            EnqueueSleep(sleepTime);

            EnqueueCallback(() =>
            {
                chart.ColorSet = "Visifire2";
                isPropertyChanged = true;
                numberOfPropertiesAdded++;
            });

            EnqueueConditional(() => { return isPropertyChanged; });
            isPropertyChanged = false;
            EnqueueSleep(sleepTime);

            EnqueueCallback(() =>
            {   
                chart.Margin = new Thickness(10);
                isPropertyChanged = true;
                numberOfPropertiesAdded++;
            });

            EnqueueConditional(() => { return isPropertyChanged; });
            isPropertyChanged = false;
            EnqueueSleep(sleepTime);

            EnqueueCallback(() =>
            {
                chart.Padding = new Thickness(8);
                isPropertyChanged = true;
                numberOfPropertiesAdded++;
            });

            EnqueueConditional(() => { return isPropertyChanged; });
            isPropertyChanged = false;
            EnqueueSleep(sleepTime);

            EnqueueCallback(() =>
            {
                chart.Theme = "Theme2";
                isPropertyChanged = true;
                numberOfPropertiesAdded++;
            });

            EnqueueConditional(() => { return isPropertyChanged; });
            isPropertyChanged = false;
            EnqueueSleep(sleepTime);

            EnqueueCallback(() =>
            {
                chart.View3D = true;
                isPropertyChanged = true;
                numberOfPropertiesAdded++;
            });

            EnqueueConditional(() => { return isPropertyChanged; });
            isPropertyChanged = false;
            EnqueueSleep(sleepTime);

            EnqueueCallback(() =>
                {
                    Assert.AreEqual(13, numberOfPropertiesAdded, "One or more property has not been applied.");
                });

            EnqueueSleep(sleepTime);
            EnqueueTestComplete();
        }
        #endregion

        #region XAML Parsing Tests
        /// <summary>
        /// Assign Title content to a Chart in XAML markup via a property. 
        /// </summary> 
        [TestMethod]
        [Description("Assign Title content to a Chart in XAML markup via a property.")]
        [Owner("[....]")]
        [Asynchronous]
        public void LoadChartTitleContentPropertyXaml()
        {
            Object result = XamlReader.Load(Resource.Chart_TitleContentPropertyXaml);
            Assert.IsInstanceOfType(result, typeof(Chart));

            Chart chart = result as Chart;
            Assert.IsInstanceOfType(chart.Titles[0], typeof(Title));

            TestPanel.Children.Add(chart);

            Title title = chart.Titles[0] as Title;
            Assert.AreEqual(Resource.Title_ShortText, title.Text);

            EnqueueSleep(3000);

            EnqueueTestComplete();
        }
        #endregion

        #region ResizeTest
        /// <summary>
        /// Resize a Chart to plug-in size if it's bigger than the plug-in.
        /// </summary>
        [TestMethod]
        [Description("[Resize a Chart to plug-in size if it's bigger than the plug-in.]")]
        [Asynchronous]
        public void ChartResize()
        {
            Chart chart = new Chart();
            DefineABigChart(chart);

            EnqueueSleep(sleepTime);

            CreateAsyncTask(chart,
                () => Assert.Equals(chart.Height, TestPanel.ActualHeight),
                () => Assert.Equals(chart.Width, TestPanel.ActualWidth));

            EnqueueTestComplete();
        }
        #endregion

        public void DefineABigChart(Chart chart)
        {
            chart.Height = TestPanel.ActualHeight + 300;
            chart.Width = TestPanel.ActualWidth + 300;

            chart.Background = new SolidColorBrush(Colors.LightGray);

            Common.CreateAndAddDefaultDataSeries(chart);
        }

        #region CreateInXaml
        /// <summary>
        /// Create a Chart in XAML markup.
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CreateInXaml()
        {
            Object result = XamlReader.Load(Resource.Chart_DefaultXaml);
            Assert.IsInstanceOfType(result, typeof(Chart));

            Chart chart = result as Chart;

            EnqueueSleep(sleepTime);

            CreateAsyncTest(chart,
                () => Assert.IsNotNull(chart._rootElement));

        }
        #endregion

        #region CheckRootElement
        /// <summary>
        /// Ensure the Content property starts null when created in XAML markup. 
        /// </summary>
        [TestMethod]
        [Description("Ensure the Chart RootElement starts null when created in XAML markup.")]
        [Owner("[....]")]
        public void ChartDefaultValueInXaml()
        {
            Object result = XamlReader.Load(Resource.Chart_DefaultXaml);
            Assert.IsInstanceOfType(result, typeof(Chart));

            Chart chart = result as Chart;

            using (chart.CreateLiveReference(this))
            {
                Assert.IsNull(chart._rootElement);
            }
        }
        #endregion

        #region Empty Template
        [TestMethod]
        public void EmptyTemplate()
        {
            ControlTemplate template = (ControlTemplate)XamlReader.Load(Resource.Chart_EmptyTemplate);

            Chart chart = new Chart();

            chart.Template = template;

        }
        #endregion

        #region CheckDefaultPropertyValue
        /// <summary>
        /// Check the View3D default property value.
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckView3DDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            Common.CreateAndAddDefaultDataSeries(chart);
            EnqueueSleep(sleepTime);

            CreateAsyncTask(chart,
                () => Assert.IsFalse(chart.View3D));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the ScrollingEnabled default property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckScrollingEnabledDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            Random rand = new Random();
            DataSeries dataSeries = new DataSeries();
            for (Int32 i = 0; i < 100; i++)
            {
                DataPoint dataPoint = new DataPoint();
                dataPoint.XValue = i + 1;
                dataPoint.YValue = rand.Next(0, 100);
                dataSeries.DataPoints.Add(dataPoint);
            }
            chart.Series.Add(dataSeries);

            EnqueueSleep(sleepTime);
            CreateAsyncTask(chart,
                () => Assert.IsTrue((Boolean)chart.ScrollingEnabled));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the AnimationEnabled default property value.
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckAnimationEnabledDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            Common.CreateAndAddDefaultDataSeries(chart);
            EnqueueSleep(sleepTime);

            CreateAsyncTask(chart,
                () => Assert.IsTrue(chart.AnimationEnabled));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the Bevel default property value.
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckBevelDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            Common.CreateAndAddDefaultDataSeries(chart);
            EnqueueSleep(sleepTime);

            CreateAsyncTask(chart,
              () => Assert.IsFalse(chart.Bevel));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the BorderColor default property value.
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckBorderBrushDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            Common.CreateAndAddDefaultDataSeries(chart);

            CreateAsyncTask(chart,
               () => Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Gray), chart.BorderBrush));

            EnqueueSleep(sleepTime);
            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the BorderThickness default property value.
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckBorderThicknessDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            Common.CreateAndAddDefaultDataSeries(chart);

            CreateAsyncTask(chart,
               () => Assert.AreEqual(new Thickness(0.5), chart.BorderThickness));

            EnqueueSleep(sleepTime);
            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the BorderStyle default property value.
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckBorderStyleDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            Common.CreateAndAddDefaultDataSeries(chart);
            EnqueueSleep(sleepTime);

            CreateAsyncTask(chart,
               () => Assert.AreEqual(BorderStyles.Solid, chart.BorderStyle));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the Color default property value.
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckBackgroundDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            Common.CreateAndAddDefaultDataSeries(chart);
            EnqueueSleep(sleepTime);

            CreateAsyncTask(chart,
               () => Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.White), chart.Background));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the ColorSet default property value.
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckColorSetDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            Common.CreateAndAddDefaultDataSeries(chart);
            EnqueueSleep(sleepTime);

            CreateAsyncTask(chart,
               () => Assert.AreEqual("Visifire1", chart.ColorSet));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the Height default property value.
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckHeightSetDefaultValue()
        {
            Chart chart = new Chart();
            Common.CreateAndAddDefaultDataSeries(chart);
            EnqueueSleep(sleepTime);

            CreateAsyncTask(chart,
               () => Assert.IsNotNull(chart.Height),
               () => Assert.AreEqual(Double.NaN, chart.Height));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the Width default property value.
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckWidthSetDefaultValue()
        {
            Chart chart = new Chart();
            Common.CreateAndAddDefaultDataSeries(chart);
            EnqueueSleep(sleepTime);

            CreateAsyncTask(chart,
               () => Assert.IsNotNull(chart.Width),
               () => Assert.AreEqual(Double.NaN, chart.Width));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the LightingEnabled default property value.
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckLightingEnabledDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            Common.CreateAndAddDefaultDataSeries(chart);
            EnqueueSleep(sleepTime);

            CreateAsyncTask(chart,
               () => Assert.IsTrue(chart.LightingEnabled));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the CornerRadius default property value.
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckCornerRadiusDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            Common.CreateAndAddDefaultDataSeries(chart);
            EnqueueSleep(sleepTime);

            CreateAsyncTask(chart,
               () => Assert.AreEqual(new CornerRadius(0, 0, 0, 0), chart.CornerRadius));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the ShadowEnabled default property value.
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckShadowEnabledDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            Common.CreateAndAddDefaultDataSeries(chart);
            EnqueueSleep(sleepTime);

            CreateAsyncTask(chart,
               () => Assert.IsFalse(chart.ShadowEnabled));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the Theme default property value.
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckThemeDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            Common.CreateAndAddDefaultDataSeries(chart);
            EnqueueSleep(sleepTime);

            CreateAsyncTask(chart,
               () => Assert.AreEqual("Theme1", chart.Theme));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the ToolTipText default property value.
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckToolTipTextDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            Common.CreateAndAddDefaultDataSeries(chart);
            EnqueueSleep(sleepTime);

            CreateAsyncTask(chart,
               () => Assert.IsNull(chart.ToolTipText));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the Watermark default property value.
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckWatermarkDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            Common.CreateAndAddDefaultDataSeries(chart);
            EnqueueSleep(sleepTime);

            CreateAsyncTask(chart,
               () => Assert.IsTrue(chart.Watermark));

            EnqueueTestComplete();
        }

        #endregion

        #region CheckNewPropertyValue

        /// <summary>
        /// Check View3D new property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckView3DNewPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            Common.CreateAndAddDefaultDataSeries(chart);

            CreateAsyncTask(chart,
                () => chart.View3D = true,
                () => Assert.IsTrue(chart.View3D));

            EnqueueSleep(sleepTime);
            EnqueueTestComplete();
        }

        /// <summary>
        /// Check AnimationEnabled new property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckAnimationEnabledNewPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            Common.CreateAndAddDefaultDataSeries(chart);
            EnqueueSleep(sleepTime);

            CreateAsyncTask(chart,
                () => chart.AnimationEnabled = false,
                () => Assert.IsFalse(chart.AnimationEnabled));

            EnqueueSleep(sleepTime);
            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the ScrollingEnabled new property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckScrollingEnabledNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            Random rand = new Random();
            DataSeries dataSeries = new DataSeries();
            for (Int32 i = 0; i < 100; i++)
            {
                DataPoint dataPoint = new DataPoint();
                dataPoint.XValue = i + 1;
                dataPoint.YValue = rand.Next(0, 100);
                dataSeries.DataPoints.Add(dataPoint);
            }
            chart.Series.Add(dataSeries);

            CreateAsyncTask(chart,
                () => chart.ScrollingEnabled = false,
                () => Assert.IsFalse((Boolean)chart.ScrollingEnabled));

            EnqueueSleep(sleepTime);
            EnqueueTestComplete();
        }

        /// <summary>
        /// Check Bevel new property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckBevelNewPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            Common.CreateAndAddDefaultDataSeries(chart);

            CreateAsyncTask(chart,
                () => chart.Background = new SolidColorBrush(Colors.Red),
                () => chart.Bevel = true,
                () => Assert.IsTrue(chart.Bevel));

            EnqueueSleep(sleepTime);
            EnqueueTestComplete();
        }

        /// <summary>
        /// Check Border Properties new value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckBorderPropertiesNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            Common.CreateAndAddDefaultDataSeries(chart);

            CreateAsyncTask(chart,
                () => chart.BorderStyle = BorderStyles.Dotted,
                () => chart.BorderBrush = new SolidColorBrush(Colors.Black),
                () => chart.BorderThickness = new Thickness(2),
                () => Assert.AreEqual(BorderStyles.Dotted, chart.BorderStyle),
                () => Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Black), chart.BorderBrush),
                () => Assert.AreEqual(new Thickness(2), chart.BorderThickness));

            EnqueueSleep(sleepTime);
            EnqueueTestComplete();
        }

        /// <summary>
        /// Check Color new property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckBackgroundNewPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            Common.CreateAndAddDefaultDataSeries(chart);

            CreateAsyncTask(chart,
                () => chart.Background = new SolidColorBrush(Colors.Magenta),
                () => Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Magenta), chart.Background));

            EnqueueSleep(sleepTime);
            EnqueueTestComplete();
        }

        /// <summary>
        /// Check ColorSet new property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckColorSetNewPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            Common.CreateAndAddDefaultDataSeries(chart);

            CreateAsyncTask(chart,
                () => EnqueueSleep(sleepTime),
                () => chart.ColorSet = "VisiGray",
                () => Assert.AreEqual("VisiGray", chart.ColorSet));

            EnqueueSleep(sleepTime);
            EnqueueTestComplete();
        }

        /// <summary>
        /// Check ChartSize new property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckChartSizePropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            Common.CreateAndAddDefaultDataSeries(chart);

            CreateAsyncTask(chart,
                () => chart.Width = 600,
                () => chart.Height = 500,
                () => Assert.AreEqual(600, chart.Width),
                () => Assert.AreEqual(500, chart.Height));

            EnqueueSleep(sleepTime);
            EnqueueTestComplete();
        }

        /// <summary>
        /// Check LightingEnabled new property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckLightingEnabledPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            Common.CreateAndAddDefaultDataSeries(chart);

            CreateAsyncTask(chart,
                () => chart.Background = new SolidColorBrush(Colors.Red),
                () => chart.LightingEnabled = true,
                () => Assert.IsTrue(chart.LightingEnabled));

            EnqueueSleep(sleepTime);
            EnqueueTestComplete();
        }

        /// <summary>
        /// Check ShadowEnabled new property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckShadowEnabledPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            Common.CreateAndAddDefaultDataSeries(chart);

            CreateAsyncTask(chart,
                () => chart.Background = new SolidColorBrush(Colors.Red),
                () => chart.ShadowEnabled = true,
                () => Assert.IsTrue(chart.ShadowEnabled));

            EnqueueSleep(sleepTime);
            EnqueueTestComplete();
        }

        /// <summary>
        /// Check CornerRadius new property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckCornerRadiusPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            Common.CreateAndAddDefaultDataSeries(chart);

            CreateAsyncTask(chart,
                () => chart.CornerRadius = new CornerRadius(5, 5, 5, 5),
                () => Assert.AreEqual(new CornerRadius(5, 5, 5, 5), chart.CornerRadius));

            EnqueueSleep(sleepTime);
            EnqueueTestComplete();
        }

        /// <summary>
        /// Check Theme new property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckThemePropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            Common.CreateAndAddDefaultDataSeries(chart);

            CreateAsyncTask(chart,
                () => chart.Theme = "Theme3",
                () => Assert.AreEqual("Theme3", chart.Theme));

            EnqueueSleep(sleepTime);
            EnqueueTestComplete();
        }

        /// <summary>
        /// Check ToolTipText new property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckToolTipTextPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            Common.CreateAndAddDefaultDataSeries(chart);

            CreateAsyncTask(chart,
                () => chart.ToolTipText = "This is a Chart",
                () => Assert.AreEqual("This is a Chart", chart.ToolTipText));

            EnqueueSleep(sleepTime);
            EnqueueTestComplete();
        }

        /// <summary>
        /// Check Watermark new property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckWatermarkPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            Common.CreateAndAddDefaultDataSeries(chart);

            CreateAsyncTask(chart,
                () => chart.Watermark = false,
                () => Assert.IsFalse(chart.Watermark));

            EnqueueSleep(sleepTime);
            EnqueueTestComplete();
        }

        #endregion

        #region CheckChartStressViaXaml
        /// <summary>
        /// Stress testing Chart loading via XAML
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckChartStressViaXaml()
        {
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "400px");

            Object result = XamlReader.Load(Resource.Chart_Xaml);
            Assert.IsInstanceOfType(result, typeof(Chart));

            chart = result as Chart;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            TestPanel.Children.Add(chart);

            Int32 iterations = 20;
            String msg = Common.AssertAverageDuration(50, iterations, delegate
            {
                chart.Series.Add((DataSeries)XamlReader.Load(Resource.DataSeries_BigXaml));
            });

            EnqueueConditional(() => { return isLoaded; });
            EnqueueSleep(sleepTime);

            EnqueueCallback(() =>
                {
                    htmlElement2 = Common.GetDisplayMessageButton(htmlElement2);
                    htmlElement2.SetStyleAttribute("top", "540px");
                    htmlElement2.SetProperty("value", msg + " Click here to exit.");
                    System.Windows.Browser.HtmlPage.Document.Body.AppendChild(htmlElement2);
                });

            EnqueueCallback(() =>
                {
                    htmlElement2.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.Chart_OnClick));
                });

            htmlElement1 = Common.GetDisplayMessageButton(htmlElement1);
            htmlElement1.SetProperty("value", "Testing DataSeries behaviour via Xaml:- " + iterations + " DataSeries with " + chart.Series[0].DataPoints.Count + " DataPoints each. ");
            System.Windows.Browser.HtmlPage.Document.Body.AppendChild(htmlElement1);
        }

        #endregion

        void Chart_OnClick(object sender, System.Windows.Browser.HtmlEventArgs e)
        {
            EnqueueTestComplete();
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "100%");
        }

        #region TestChartLoaded

        /// <summary>
        /// Testing the Chart loading
        /// </summary>
        [TestMethod]
        [Description("Testing the Chart loading")]
        [Owner("[....]")]
        [Asynchronous]
        public void ChartLoaded()
        {
            chart = new Chart();
            isLoaded = false;

            chart.Width = 400;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            TestPanel.Children.Add(chart);

            EnqueueConditional(() => { return isLoaded; });

            EnqueueSleep(sleepTime);

            EnqueueCallback(delegate
            {
                chart.Loaded -= new RoutedEventHandler(chart_Loaded);
            });

            EnqueueTestComplete();
        }

        #endregion

        #region CreateAndVerifyChartRoot
        /// <summary>
        /// Create a Chart and verify its Content root.
        /// </summary> 
        [TestMethod]
        [Description("Create a Chart and verify its Content root.")]
        [Owner("[...]")]
        [Asynchronous]
        public void CreateAndVerifyRoot()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            CreateAsyncTest(chart,
                () => Assert.IsNotNull(chart._rootElement),
                () => Assert.IsInstanceOfType(chart._rootElement, typeof(Grid)));
        }
        #endregion

        #region TestTitlesCollectionChanged Event
        /// <summary>
        /// Testing the Titles collection changed event
        /// </summary>
        [TestMethod]
        [Description("Testing the Titles collection changed event")]
        [Owner("[...]")]
        [Asynchronous]
        public void TestingTitlesCollectionChanged()
        {
            Chart chart = new Chart();

            Int32 titlesAdded = 0;

            chart.Background = new SolidColorBrush(Colors.LightGray);

            chart.Width = 400;
            chart.Height = 300;
            chart.AnimationEnabled = false;

            isLoaded = false;
            bool isTitleAdded = false;
            Common.CreateAndAddDefaultDataSeries(chart);

            Title title = new Title();
            title.Text = "Title1";
            title.VerticalAlignment = VerticalAlignment.Top;
            title.HorizontalAlignment = HorizontalAlignment.Center;
            chart.Titles.Add(title);

            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            TestPanel.Children.Add(chart);

            chart.Titles.CollectionChanged += (object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
            =>
            {
                if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
                {
                    titlesAdded += e.NewItems.Count;
                    Assert.AreEqual(1, e.NewItems.Count);
                }
            };

            EnqueueConditional(() => { return isLoaded; });
            isTitleAdded = false;
            EnqueueSleep(sleepTime);

            EnqueueCallback(() =>
            {
                title = new Title();
                title.Text = "Title2";
                title.VerticalAlignment = VerticalAlignment.Center;
                title.HorizontalAlignment = HorizontalAlignment.Right;
                chart.Titles.Add(title);
                isTitleAdded = true;
            });

            EnqueueConditional(() => { return isTitleAdded; });
            isTitleAdded = false;
            EnqueueSleep(sleepTime);

            EnqueueCallback(() =>
            {
                title = new Title();
                title.Text = "Title3";
                title.VerticalAlignment = VerticalAlignment.Bottom;
                title.HorizontalAlignment = HorizontalAlignment.Center;
                chart.Titles.Add(title);
                isTitleAdded = true;
            });

            EnqueueConditional(() => { return isTitleAdded; });
            isTitleAdded = false;
            EnqueueSleep(sleepTime);

            EnqueueCallback(() =>
            {
                title = new Title();
                title.Text = "Title4";
                title.VerticalAlignment = VerticalAlignment.Center;
                title.HorizontalAlignment = HorizontalAlignment.Left;
                chart.Titles.Add(title);
                isTitleAdded = true;
            });

            EnqueueConditional(() => { return isTitleAdded; });
            isTitleAdded = false;
            EnqueueSleep(sleepTime);

            EnqueueCallback(() =>
            {
                title = new Title();
                title.Text = "Title5";
                title.VerticalAlignment = VerticalAlignment.Top;
                title.HorizontalAlignment = HorizontalAlignment.Stretch;
                title.Background = new SolidColorBrush(Colors.Gray);
                chart.Titles.Add(title);
                isTitleAdded = true;
            });

            EnqueueConditional(() => { return isTitleAdded; });
            isTitleAdded = false;
            EnqueueSleep(sleepTime);

            EnqueueCallback(() =>
            {
                Assert.AreEqual(4, titlesAdded);
            });

            EnqueueTestComplete();
        }

        #endregion

        #region CheckObservableTitlesCollection
        /// <summary>
        /// Testing Observable title colection using a instanciated title.
        /// </summary>
        [TestMethod]
        [Description("Check the observable Title collection.")]
        [Owner("[....]")]
        public void ObservableCollection()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            TitleTest titleTest = new TitleTest();
            bool changedObserved = false;
            foreach (Title title in titleTest.TitlesToTest)
            {
                using (chart.CreateLiveReference(this))
                {
                    chart.Titles.CollectionChanged += delegate(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
                    {
                        changedObserved = true;
                        Assert.AreEqual(System.Collections.Specialized.NotifyCollectionChangedAction.Add, e.Action);
                        Assert.AreEqual(1, e.NewItems.Count);
                        Assert.AreEqual(new FontFamily("Trebuchet MS"), title.FontFamily);
                    };

                    title.FontFamily = new FontFamily("Trebuchet MS");
                    chart.Titles.Add(title);

                }

                Assert.IsTrue(changedObserved, "The event handler did not fire");
            }
        }

        #endregion CheckObservableTitlesCollection

        #region CheckControlTemplate
        /// <summary>
        /// Verifies the Control's TemplateParts.
        /// </summary>
        [TestMethod]
        [Description("Verifies the Control's TemplateParts.")]
        public void TemplatePartsAreDefined()
        {
            IDictionary<string, Type> templateParts = DefaultControlToTest.GetType().GetTemplateParts();
            Assert.AreEqual(57, templateParts.Count);
            Assert.AreSame(typeof(Grid), templateParts["RootElement"]);
            Assert.AreSame(typeof(Grid), templateParts["ShadowGrid"]);
            Assert.AreSame(typeof(Border), templateParts["ChartBorder"]);
            Assert.AreSame(typeof(Grid), templateParts["ChartAreaGrid"]);
            Assert.AreSame(typeof(Rectangle), templateParts["ChartLightingBorder"]);
            Assert.AreSame(typeof(StackPanel), templateParts["TopOuterPanel"]);
            Assert.AreSame(typeof(StackPanel), templateParts["TopOuterTitlePanel"]);
            Assert.AreSame(typeof(StackPanel), templateParts["TopOuterLegendPanel"]);
            Assert.AreSame(typeof(StackPanel), templateParts["BottomOuterPanel"]);
            Assert.AreSame(typeof(StackPanel), templateParts["BottomOuterTitlePanel"]);
            Assert.AreSame(typeof(StackPanel), templateParts["BottomOuterLegendPanel"]);
            Assert.AreSame(typeof(StackPanel), templateParts["LeftOuterPanel"]);
            Assert.AreSame(typeof(StackPanel), templateParts["LeftOuterTitlePanel"]);
            Assert.AreSame(typeof(StackPanel), templateParts["LeftOuterLegendPanel"]);
            Assert.AreSame(typeof(StackPanel), templateParts["RightOuterPanel"]);
            Assert.AreSame(typeof(StackPanel), templateParts["RightOuterTitlePanel"]);
            Assert.AreSame(typeof(StackPanel), templateParts["RightOuterLegendPanel"]);
            Assert.AreSame(typeof(Grid), templateParts["CenterOuterGrid"]);
            Assert.AreSame(typeof(Grid), templateParts["CenterGrid"]);
            Assert.AreSame(typeof(Grid), templateParts["TopAxisGrid"]);
            Assert.AreSame(typeof(StackPanel), templateParts["TopAxisContainer"]);
            Assert.AreSame(typeof(StackPanel), templateParts["TopAxisPanel"]);
            Assert.AreSame(typeof(Grid), templateParts["LeftAxisGrid"]);
            Assert.AreSame(typeof(StackPanel), templateParts["LeftAxisContainer"]);
            Assert.AreSame(typeof(StackPanel), templateParts["LeftAxisPanel"]);
            Assert.AreSame(typeof(Grid), templateParts["RightAxisGrid"]);
            Assert.AreSame(typeof(StackPanel), templateParts["RightAxisContainer"]);
            Assert.AreSame(typeof(StackPanel), templateParts["RightAxisPanel"]);
            Assert.AreSame(typeof(Grid), templateParts["BottomAxisGrid"]);
            Assert.AreSame(typeof(StackPanel), templateParts["BottomAxisContainer"]);
            Assert.AreSame(typeof(StackPanel), templateParts["BottomAxisPanel"]);
            Assert.AreSame(typeof(Grid), templateParts["CenterInnerGrid"]);
            Assert.AreSame(typeof(Grid), templateParts["PlotAreaGrid"]);
            Assert.AreSame(typeof(ScrollViewer), templateParts["PlotAreaScrollViewer"]);
            Assert.AreSame(typeof(Canvas), templateParts["PlotCanvas"]);
            Assert.AreSame(typeof(Canvas), templateParts["PlotAreaShadowCanvas"]);
            Assert.AreSame(typeof(Canvas), templateParts["DrawingCanvas"]);
            Assert.AreSame(typeof(Grid), templateParts["InnerGrid"]);
            Assert.AreSame(typeof(StackPanel), templateParts["TopInnerPanel"]);
            Assert.AreSame(typeof(StackPanel), templateParts["TopInnerTitlePanel"]);
            Assert.AreSame(typeof(StackPanel), templateParts["TopInnerLegendPanel"]);
            Assert.AreSame(typeof(StackPanel), templateParts["BottomInnerPanel"]);
            Assert.AreSame(typeof(StackPanel), templateParts["BottomInnerTitlePanel"]);
            Assert.AreSame(typeof(StackPanel), templateParts["BottomInnerLegendPanel"]);
            Assert.AreSame(typeof(StackPanel), templateParts["LeftInnerPanel"]);
            Assert.AreSame(typeof(StackPanel), templateParts["LeftInnerTitlePanel"]);
            Assert.AreSame(typeof(StackPanel), templateParts["LeftInnerLegendPanel"]);
            Assert.AreSame(typeof(StackPanel), templateParts["RightInnerPanel"]);
            Assert.AreSame(typeof(StackPanel), templateParts["RightInnerTitlePanel"]);
            Assert.AreSame(typeof(StackPanel), templateParts["RightInnerLegendPanel"]);
            Assert.AreSame(typeof(Canvas), templateParts["BevelCanvas"]);
            Assert.AreSame(typeof(StackPanel), templateParts["CenterDockInsidePlotAreaPanel"]);
            Assert.AreSame(typeof(StackPanel), templateParts["CenterDockOutsidePlotAreaPanel"]);
            Assert.AreSame(typeof(Canvas), templateParts["ToolTipCanvas"]);
            Assert.AreSame(typeof(Border), templateParts["ToolTip"]);
        }
        #endregion CheckControlTemplate

        /// <summary>
        /// Gets a default instance of Control (or a derived type) to test.
        /// </summary>
        public Control DefaultControlToTest
        {
            get { return new Chart(); }
        }

        #region CheckChartTitles
        /// <summary>
        /// Assign Title contents to different Charts.
        /// </summary>
        [TestMethod]
        [Description("Assign Title contents to different Charts.")]
        [Owner("[....]")]
        [Asynchronous]
        public void ChartTitleObjects()
        {
            foreach (Chart chart in ChartToTest)
            {
                chart.Width = 500;
                chart.Height = 300;

                TestPanel.Children.Add(chart);

                Title title = new Title()
                {
                    Text = "Visifire" + GetHashCode(),
                    FontSize = 14,
                };
                chart.Titles.Add(title);
                chart.Titles.Add(chart.Titles[0]);
                EnqueueSleep(sleepTime);
            }
            EnqueueTestComplete();
        }

        #endregion CheckChartTitles

        public IEnumerable<Chart> ChartToTest
        {
            get
            {
                // Standard chart
                yield return new Chart();

                // Chart with an empty template
                Chart empty = new Chart();
                empty.Template = new ControlTemplate();
                yield return empty;
            }
        }

        #region Private Data

        const int sleepTime = 1000;
        Chart chart;
        bool isLoaded = false;
        System.Windows.Browser.HtmlElement htmlElement1, htmlElement2;

        #endregion
    }
}
