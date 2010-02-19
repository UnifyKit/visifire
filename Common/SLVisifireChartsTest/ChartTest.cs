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
    /// <summary>
    /// This class runs the unit tests Visifire.Charts.Chart class 
    /// </summary>
    [TestClass]
    public class ChartTest : SilverlightControlTest
    {
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
            _chart = new Chart();
            _chart.Width = 400;
            _chart.Height = 300;

            _isLoaded = false;
            _chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Common.CreateAndAddDefaultDataSeries(_chart);

            TestPanel.Children.Add(_chart);

            bool isPropertyChanged = false;
            int numberOfPropertiesAdded = 0;

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                _chart.Background = new SolidColorBrush(Colors.Red);
                isPropertyChanged = true;
                numberOfPropertiesAdded++;
            });

            EnqueueConditional(() => { return isPropertyChanged; });
            isPropertyChanged = false;
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                _chart.LightingEnabled = true;
                isPropertyChanged = true;
                numberOfPropertiesAdded++;
            });

            EnqueueConditional(() => { return isPropertyChanged; });
            isPropertyChanged = false;
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                _chart.ShadowEnabled = true;
                isPropertyChanged = true;
                numberOfPropertiesAdded++;
            });

            EnqueueConditional(() => { return isPropertyChanged; });
            isPropertyChanged = false;
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                _chart.BorderBrush = new SolidColorBrush(Colors.Green);
                isPropertyChanged = true;
                numberOfPropertiesAdded++;
            });

            EnqueueConditional(() => { return isPropertyChanged; });
            isPropertyChanged = false;
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                _chart.BorderThickness = new Thickness(2);
                isPropertyChanged = true;
                numberOfPropertiesAdded++;
            });

            EnqueueConditional(() => { return isPropertyChanged; });
            isPropertyChanged = false;
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                _chart.CornerRadius = new CornerRadius(5);
                isPropertyChanged = true;
                numberOfPropertiesAdded++;
            });

            EnqueueConditional(() => { return isPropertyChanged; });
            isPropertyChanged = false;
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                _chart.Bevel = true;
                isPropertyChanged = true;
                numberOfPropertiesAdded++;
            });

            EnqueueConditional(() => { return isPropertyChanged; });
            isPropertyChanged = false;
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                _chart.ColorSet = "Visifire2";
                isPropertyChanged = true;
                numberOfPropertiesAdded++;
            });

            EnqueueConditional(() => { return isPropertyChanged; });
            isPropertyChanged = false;
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                _chart.Margin = new Thickness(10);
                isPropertyChanged = true;
                numberOfPropertiesAdded++;
            });

            EnqueueConditional(() => { return isPropertyChanged; });
            isPropertyChanged = false;
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                _chart.Padding = new Thickness(8);
                isPropertyChanged = true;
                numberOfPropertiesAdded++;
            });

            EnqueueConditional(() => { return isPropertyChanged; });
            isPropertyChanged = false;
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                _chart.Theme = "Theme2";
                isPropertyChanged = true;
                numberOfPropertiesAdded++;
            });

            EnqueueConditional(() => { return isPropertyChanged; });
            isPropertyChanged = false;
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                _chart.View3D = true;
                isPropertyChanged = true;
                numberOfPropertiesAdded++;
            });

            EnqueueConditional(() => { return isPropertyChanged; });
            isPropertyChanged = false;
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
                {
                    Assert.AreEqual(12, numberOfPropertiesAdded, "One or more property has not been applied.");
                });

            EnqueueDelay(_sleepTime);
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

            EnqueueDelay(3000);

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

            EnqueueDelay(_sleepTime);

            CreateAsyncTask(chart,
                () => Assert.Equals(chart.Height, TestPanel.ActualHeight),
                () => Assert.Equals(chart.Width, TestPanel.ActualWidth));

            EnqueueTestComplete();
        }
        #endregion

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

            EnqueueDelay(_sleepTime);

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

        #region EmptyTemplate
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
            EnqueueDelay(_sleepTime);

            CreateAsyncTask(chart,
                () => Assert.IsFalse(chart.View3D));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the UniqueColors default property value.
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckUniqueColorsDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);
            EnqueueDelay(_sleepTime);

            CreateAsyncTask(chart,
                () => Assert.IsTrue(chart.UniqueColors));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the HrefAndHrefTArget default property value.
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckHrefAndHrefTArgetDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            Common.CreateAndAddDefaultDataSeries(chart);
            EnqueueDelay(_sleepTime);

            CreateAsyncTask(chart,
                () => Assert.AreEqual(null, chart.Href),
                () => Assert.AreEqual(HrefTargets._self, chart.HrefTarget));

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

            EnqueueDelay(_sleepTime);
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
            EnqueueDelay(_sleepTime);

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
            EnqueueDelay(_sleepTime);

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

            EnqueueDelay(_sleepTime);
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

            EnqueueDelay(_sleepTime);
            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the Padding default property value.
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckPaddingDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            Common.CreateAndAddDefaultDataSeries(chart);

            CreateAsyncTask(chart,
               () => Assert.AreEqual(new Thickness(5), chart.Padding));

            EnqueueDelay(_sleepTime);
            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the Background default property value.
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckBackgroundDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            Common.CreateAndAddDefaultDataSeries(chart);
            EnqueueDelay(_sleepTime);

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
            EnqueueDelay(_sleepTime);

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
            EnqueueDelay(_sleepTime);

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
            EnqueueDelay(_sleepTime);

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
            EnqueueDelay(_sleepTime);

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
            EnqueueDelay(_sleepTime);

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
            EnqueueDelay(_sleepTime);

            CreateAsyncTask(chart,
               () => Assert.IsFalse(chart.ShadowEnabled));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the ToolTipEnabled default property value.
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckToolTipEnabledDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            Common.CreateAndAddDefaultDataSeries(chart);
            EnqueueDelay(_sleepTime);

            CreateAsyncTask(chart,
               () => Assert.IsTrue(chart.ToolTipEnabled));

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
            EnqueueDelay(_sleepTime);

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
            EnqueueDelay(_sleepTime);

            CreateAsyncTask(chart,
               () => Assert.IsNotNull(chart.ToolTipText));

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
            EnqueueDelay(_sleepTime);

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

            EnqueueDelay(_sleepTime);
            EnqueueTestComplete();
        }

        /// <summary>
        /// Check UniqueColors new property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckUniqueColorsNewPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            CreateAsyncTask(chart,
                () => chart.UniqueColors = false,
                () => Assert.IsFalse(chart.UniqueColors));

            EnqueueDelay(_sleepTime);
            EnqueueTestComplete();
        }

        /// <summary>
        /// Check HrefAndHrefTarget new property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckHrefAndHrefTargetNewPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            Common.CreateAndAddDefaultDataSeries(chart);

            CreateAsyncTask(chart,
                () => chart.Href = "http://www.visifire.com",
                () => chart.HrefTarget = HrefTargets._blank,
                () => Assert.AreEqual("http://www.visifire.com", chart.Href),
                () => Assert.AreEqual(HrefTargets._blank, chart.HrefTarget));

            EnqueueDelay(_sleepTime);
            EnqueueTestComplete();
        }

        /// <summary>
        /// Check Opacity new property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckOpacityNewPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            Common.CreateAndAddDefaultDataSeries(chart);

            CreateAsyncTask(chart,
                () => chart.Opacity = 0.5,
                () => Assert.AreEqual(0.5, chart.Opacity, Common.HighPrecisionDelta));

            EnqueueDelay(_sleepTime);
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
            EnqueueDelay(_sleepTime);

            CreateAsyncTask(chart,
                () => chart.AnimationEnabled = false,
                () => Assert.IsFalse(chart.AnimationEnabled));

            EnqueueDelay(_sleepTime);
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

            EnqueueDelay(_sleepTime);
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

            EnqueueDelay(_sleepTime);
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
                () => chart.BorderBrush = new SolidColorBrush(Colors.Black),
                () => chart.BorderThickness = new Thickness(2),
                () => Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Black), chart.BorderBrush),
                () => Assert.AreEqual(new Thickness(2), chart.BorderThickness));

            EnqueueDelay(_sleepTime);
            EnqueueTestComplete();
        }

        /// <summary>
        /// Check Background new property value
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

            EnqueueDelay(_sleepTime);
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
                () => EnqueueDelay(_sleepTime),
                () => chart.ColorSet = "VisiGray",
                () => Assert.AreEqual("VisiGray", chart.ColorSet));

            EnqueueDelay(_sleepTime);
            EnqueueTestComplete();
        }

        /// <summary>
        /// Check Padding new property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckPaddingPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            Common.CreateAndAddDefaultDataSeries(chart);

            CreateAsyncTask(chart,
                () => chart.Padding = new Thickness(12),
                () => Assert.AreEqual(new Thickness(12), chart.Padding));

            EnqueueDelay(_sleepTime);
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

            EnqueueDelay(_sleepTime);
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

            EnqueueDelay(_sleepTime);
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

            EnqueueDelay(_sleepTime);
            EnqueueTestComplete();
        }

        /// <summary>
        /// Check ToolTipEnabled new property value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckToolTipEnabledPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            Common.CreateAndAddDefaultDataSeries(chart);

            CreateAsyncTask(chart,
                () => chart.ToolTipEnabled = false,
                () => Assert.IsFalse(chart.ToolTipEnabled));

            EnqueueDelay(_sleepTime);
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

            EnqueueDelay(_sleepTime);
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

            EnqueueDelay(_sleepTime);
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

            EnqueueDelay(_sleepTime);
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

            EnqueueDelay(_sleepTime);
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

            _chart = result as Chart;
            _chart.Loaded += new RoutedEventHandler(chart_Loaded);

            TestPanel.Children.Add(_chart);

            Int32 iterations = 20;
            String msg = Common.AssertAverageDuration(50, iterations, delegate
            {
                _chart.Series.Add((DataSeries)XamlReader.Load(Resource.DataSeries_BigXaml));
            });

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
                {
                    _htmlElement2 = Common.GetDisplayMessageButton(_htmlElement2);
                    _htmlElement2.SetStyleAttribute("top", "540px");
                    _htmlElement2.SetProperty("value", msg + " Click here to exit.");
                    System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement2);
                });

            EnqueueCallback(() =>
                {
                    _htmlElement2.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.HtmlElement_OnClick));
                });

            _htmlElement1 = Common.GetDisplayMessageButton(_htmlElement1);
            _htmlElement1.SetProperty("value", "Testing DataSeries behaviour via Xaml:- " + iterations + " DataSeries with " + _chart.Series[0].DataPoints.Count + " DataPoints each. ");
            System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement1);
        }

        #endregion

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
            _chart = new Chart();
            _isLoaded = false;

            _chart.Width = 400;
            _chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(_chart);

            _chart.Loaded += new RoutedEventHandler(chart_Loaded);

            TestPanel.Children.Add(_chart);

            EnqueueConditional(() => { return _isLoaded; });

            EnqueueDelay(_sleepTime);

            EnqueueCallback(delegate
            {
                _chart.Loaded -= new RoutedEventHandler(chart_Loaded);
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

            _isLoaded = false;
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

            EnqueueConditional(() => { return _isLoaded; });
            isTitleAdded = false;
            EnqueueDelay(_sleepTime);

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
            EnqueueDelay(_sleepTime);

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
            EnqueueDelay(_sleepTime);

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
            EnqueueDelay(_sleepTime);

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
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                Assert.AreEqual(4, titlesAdded);
            });

            EnqueueTestComplete();
        }

        #endregion

        #region ChartEventTesting
        /// <summary>
        /// Testing events in Chart
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void ChartEventChecking()
        {
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "400px");

            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            TestPanel.Children.Add(chart);

            Common.CreateAndAddDefaultDataSeries(chart);

            chart.MouseEnter += delegate(Object sender, MouseEventArgs e)
            {
                _htmlElement1.SetProperty("value", "Chart MouseEnter event fired");
            };

            chart.MouseLeave += delegate(Object sender, MouseEventArgs e)
            {
                _htmlElement1.SetProperty("value", "Chart MouseLeave event fired");
            };

            chart.MouseLeftButtonUp += delegate(Object sender, MouseButtonEventArgs e)
            {
                _htmlElement1.SetProperty("value", "Chart MouseLeftButtonUp event fired");
            };

            chart.MouseLeftButtonDown += delegate(Object sender, MouseButtonEventArgs e)
            {
                _htmlElement1.SetProperty("value", "Chart MouseLeftButtonDown event fired");
            };

            EnqueueCallback(() =>
            {
                _htmlElement1.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.HtmlElement_OnClick));
            });

            _htmlElement1 = Common.GetDisplayMessageButton(_htmlElement1);
            _htmlElement1.SetStyleAttribute("width", "900px");
            _htmlElement1.SetProperty("value", "Click here to exit.");
            System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement1);

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
            //Assert.AreSame(typeof(StackPanel), templateParts["LeftInnerPanel"]);
            Assert.AreSame(typeof(StackPanel), templateParts["LeftInnerTitlePanel"]);
            Assert.AreSame(typeof(StackPanel), templateParts["LeftInnerLegendPanel"]);
            Assert.AreSame(typeof(StackPanel), templateParts["RightInnerPanel"]);
            Assert.AreSame(typeof(StackPanel), templateParts["RightInnerTitlePanel"]);
            Assert.AreSame(typeof(StackPanel), templateParts["RightInnerLegendPanel"]);
            Assert.AreSame(typeof(Canvas), templateParts["BevelCanvas"]);
            Assert.AreSame(typeof(StackPanel), templateParts["CenterDockInsidePlotAreaPanel"]);
            Assert.AreSame(typeof(StackPanel), templateParts["CenterDockOutsidePlotAreaPanel"]);
            Assert.AreSame(typeof(Canvas), templateParts["ToolTipCanvas"]);
            //Assert.AreSame(typeof(Border), templateParts["ToolTip"]);
        }
        #endregion CheckControlTemplate

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
                EnqueueDelay(_sleepTime);
            }
            EnqueueTestComplete();
        }

        #endregion CheckChartTitles

        #region TestChartWithoutWidthHeight
        /// <summary>
        /// Test Chart without setting Width and Height
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void TestChartWithoutWidthHeight()
        {
            Chart chart = new Chart();

            EnqueueDelay(_sleepTime);

            CreateAsyncTask(chart,
                () => Assert.AreEqual(Double.NaN, chart.Height),
                () => Assert.AreEqual(Double.NaN, chart.Width));

            EnqueueTestComplete();
        }
        #endregion

        #region TestChartWithoutHeight
        /// <summary>
        /// Test Chart without setting Height
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void TestChartWithoutHeight()
        {
            Chart chart = new Chart();
            chart.Width = 500;

            EnqueueDelay(_sleepTime);

            CreateAsyncTask(chart,
                () => Assert.AreEqual(Double.NaN, chart.Height),
                () => Assert.AreEqual(500, chart.Width));

            EnqueueTestComplete();
        }
        #endregion

        #region TestChartWithoutWidth
        /// <summary>
        /// Test Chart without setting Width
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void TestChartWithoutWidth()
        {
            Chart chart = new Chart();
            chart.Height = 300;

            EnqueueDelay(_sleepTime);

            CreateAsyncTask(chart,
                () => Assert.AreEqual(300, chart.Height),
                () => Assert.AreEqual(Double.NaN, chart.Width));

            EnqueueTestComplete();
        }
        #endregion

        #region TestUniqueColors
        /// <summary>
        /// Test Unique Colors
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void TestUniqueColors()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;
            chart.UniqueColors = false;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.Column;

            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                delegate
                {
                    for (Int32 i = 0; i < 5; i++)
                    {
                        if (i < 4)
                            Common.AssertBrushesAreEqual(chart.Series[0].DataPoints[i].Color, chart.Series[0].DataPoints[i + 1].Color);
                    }
                });

            EnqueueTestComplete();
        }
        #endregion

        /// <summary>
        /// Gets a chart
        /// </summary>
        /// <param name="chart"></param>
        public void DefineABigChart(Chart chart)
        {
            chart.Height = TestPanel.ActualHeight + 300;
            chart.Width = TestPanel.ActualWidth + 300;

            chart.Background = new SolidColorBrush(Colors.LightGray);

            Common.CreateAndAddDefaultDataSeries(chart);
        }

        /// <summary>
        /// Gets a default instance of Control (or a derived type) to test.
        /// </summary>
        public Control DefaultControlToTest
        {
            get { return new Chart(); }
        }

        /// <summary>
        /// Gets a default instances of Chart to test.
        /// </summary>
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

        /// <summary>
        /// Event handler for loaded event of the chart
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chart_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            _isLoaded = true;
        }

        /// <summary>
        /// Event handler for click event of the Html element
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void HtmlElement_OnClick(object sender, System.Windows.Browser.HtmlEventArgs e)
        {
            EnqueueTestComplete();
            try
            {
                System.Windows.Browser.HtmlPage.Document.Body.RemoveChild(_htmlElement1);
                System.Windows.Browser.HtmlPage.Document.Body.RemoveChild(_htmlElement2);
                System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "100%");
            }
            catch
            {}
        }

        #region Private Data
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
