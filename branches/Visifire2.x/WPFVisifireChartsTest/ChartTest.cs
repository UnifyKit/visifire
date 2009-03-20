using System;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Documents;
using System.IO;
using System.Text;
using System.Xml;
using System.Collections.Generic;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Markup;
using System.Collections.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Visifire.Charts;
using Visifire.Commons;

namespace WPFVisifireChartsTest
{
    /// <summary>
    /// This class runs the unit tests Visifire.Charts.Chart class 
    /// </summary>
    [TestClass]
    public class ChartTest
    {
        #region XAML Parsing Tests
        /// <summary>
        /// Assign Title content to a Chart in XAML markup via a property. 
        /// </summary> 
        [TestMethod]
        [Description("Assign Title content to a Chart in XAML markup via a property.")]
        [Owner("[....]")]
        public void LoadChartTitleContentPropertyXaml()
        {
            Object result = XamlReader.Load(new XmlTextReader(new StringReader(Resource.Chart_TitleContentPropertyXaml)));
            Assert.IsInstanceOfType(result, typeof(Chart));

            Chart chart = result as Chart;
            Assert.IsInstanceOfType(chart.Titles[0], typeof(Title));

            Title title = chart.Titles[0] as Title;

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
                Assert.AreEqual(Resource.Title_ShortText, title.Text);

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }
        #endregion

        #region CreateInXaml
        /// <summary>
        /// Create a Chart in XAML markup.
        /// </summary>
        [TestMethod]
        public void CreateInXaml()
        {
            Object result = XamlReader.Load(new XmlTextReader(new StringReader(Resource.Chart_DefaultXaml)));
            Assert.IsInstanceOfType(result, typeof(Chart));

            Chart chart = result as Chart;

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
                Assert.IsNotNull(chart._rootElement);

            window.Dispatcher.InvokeShutdown();
            window.Close();
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
            Object result = XamlReader.Load(new XmlTextReader(new StringReader(Resource.Chart_DefaultXaml)));
            Assert.IsInstanceOfType(result, typeof(Chart));

            Chart chart = result as Chart;

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
                Assert.IsNotNull(chart._rootElement);

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }
        #endregion

        #region Empty Template
        [TestMethod]
        public void EmptyTemplate()
        {
            ControlTemplate template = (ControlTemplate)XamlReader.Load(new XmlTextReader(new StringReader(Resource.Chart_EmptyTemplate)));

            Chart chart = new Chart();

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
                chart.Template = template;

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        #endregion

        #region TestChartLoaded

        /// <summary>
        /// Testing the Chart loading
        /// </summary>
        [TestMethod]
        [Description("Testing the Chart loading")]
        [Owner("[....]")]
        public void ChartLoaded()
        {
            _chart = new Chart();
            _isLoaded = false;

            _chart.Width = 400;
            _chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(_chart);

            _chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = _chart;
            window.Show();
            Assert.IsTrue(_isLoaded);

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        #endregion

        #region TestTitlesCollectionChanged Event
        [TestMethod]
        [Description("Testing the Titles collection changed event")]
        [Owner("[...]")]
        public void TestingTitlesCollectionChanged()
        {
            Chart chart = new Chart();

            Int32 titlesAdded = 0;

            chart.Background = new SolidColorBrush(Colors.LightGray);

            chart.Width = 400;
            chart.Height = 300;

            _isLoaded = false;

            Common.CreateAndAddDefaultDataSeries(chart);

            chart.Titles = new TitleCollection();

            Title title = new Title();
            title.Text = "Title1";
            title.VerticalAlignment = VerticalAlignment.Top;
            title.HorizontalAlignment = HorizontalAlignment.Center;
            chart.Titles.Add(title);

            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            chart.Titles.CollectionChanged += (object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
                =>
            {
                if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
                {
                    titlesAdded += e.NewItems.Count;
                    Assert.AreEqual(1, e.NewItems.Count);
                }
            };

            title = new Title();
            title.Text = "Title2";
            title.VerticalAlignment = VerticalAlignment.Center;
            title.HorizontalAlignment = HorizontalAlignment.Right;
            chart.Titles.Add(title);

            title = new Title();
            title.Text = "Title3";
            title.VerticalAlignment = VerticalAlignment.Bottom;
            title.HorizontalAlignment = HorizontalAlignment.Center;
            chart.Titles.Add(title);

            title = new Title();
            title.Text = "Title4";
            title.VerticalAlignment = VerticalAlignment.Center;
            title.HorizontalAlignment = HorizontalAlignment.Left;
            chart.Titles.Add(title);

            title = new Title();
            title.Text = "Title5";
            title.VerticalAlignment = VerticalAlignment.Top;
            title.HorizontalAlignment = HorizontalAlignment.Stretch;
            title.Background = new SolidColorBrush(Colors.Gray);
            chart.Titles.Add(title);

            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
                Assert.AreEqual(4, titlesAdded);

            window.Dispatcher.InvokeShutdown();
            window.Close();

        }

        #endregion

        #region CheckObservableTitlesCollection

        [TestMethod]
        [Description("Check the observable Title collection.")]
        [Owner("[....]")]
        public void ObservableCollection()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            _isLoaded = false;
            Common.CreateAndAddDefaultDataSeries(chart);

            TitleTest titleTest = new TitleTest();
            bool changedObserved = false;
            foreach (Title title in titleTest.TitlesToTest)
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

                chart.Loaded += new RoutedEventHandler(chart_Loaded);

                Window window = new Window();
                window.Content = chart;
                window.Show();
                if (_isLoaded)
                    Assert.IsTrue(changedObserved, "The event handler did not fire");

                window.Dispatcher.InvokeShutdown();
                window.Close();
            }
        }

        #endregion CheckObservableTitlesCollection

        #region CheckDefaultPropertyValue

        /// <summary>
        /// Check the View3D default property value.
        /// </summary>
        [TestMethod]
        public void CheckView3DDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
                Assert.IsFalse(chart.View3D);

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the AnimationEnabled default property value.
        /// </summary>
        [TestMethod]
        public void CheckAnimationEnabledDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            _isLoaded = true;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
                Assert.IsTrue(chart.AnimationEnabled);

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the ScrolligEnabled default property value.
        /// </summary>
        [TestMethod]
        public void CheckScrollingEnabledDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
                Assert.IsTrue((Boolean)chart.ScrollingEnabled);

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the Bevel default property value.
        /// </summary>
        [TestMethod]
        public void CheckBevelDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            _isLoaded = false;

            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
                Assert.IsFalse(chart.Bevel);

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the BorderColor default property value.
        /// </summary>
        [TestMethod]
        public void CheckBorderColorDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            _isLoaded = false;

            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
                Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Gray), chart.BorderBrush);

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the BorderThickness default property value.
        /// </summary>
        [TestMethod]
        public void CheckBorderThicknessDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            _isLoaded = false;

            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
                Assert.AreEqual(new Thickness(1), chart.BorderThickness);

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the Background default property value.
        /// </summary>
        [TestMethod]
        public void CheckBackgroundDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            _isLoaded = false;

            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
                Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.White), chart.Background);

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the Padding default property value.
        /// </summary>
        [TestMethod]
        public void CheckPaddingDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            _isLoaded = false;

            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
                Assert.AreEqual(new Thickness(5), chart.Padding);

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the ColorSet default property value.
        /// </summary>
        [TestMethod]
        public void CheckColorSetDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            _isLoaded = false;

            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
                Assert.AreEqual("Visifire1", chart.ColorSet);

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the LightingEnabled default property value.
        /// </summary>
        [TestMethod]
        public void CheckLightingEnabledDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            _isLoaded = false;

            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
                Assert.IsTrue(chart.LightingEnabled);

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the CornerRadius default property value.
        /// </summary>
        [TestMethod]
        public void CheckCornerRadiusDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            _isLoaded = false;

            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
                Assert.AreEqual(new CornerRadius(0, 0, 0, 0), chart.CornerRadius);

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the ShadowEnabled default property value.
        /// </summary>
        [TestMethod]
        public void CheckShadowEnabledDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            _isLoaded = false;

            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
                Assert.IsFalse(chart.ShadowEnabled);

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the ToolTipEnabled default property value.
        /// </summary>
        [TestMethod]
        public void CheckToolTipEnabledDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            _isLoaded = false;

            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
                Assert.IsTrue(chart.ToolTipEnabled);

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the Theme default property value.
        /// </summary>
        [TestMethod]
        public void CheckThemeDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            _isLoaded = false;

            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
                Assert.AreEqual("Theme1", chart.Theme);

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the ToolTipText default property value.
        /// </summary>
        [TestMethod]
        public void CheckToolTipTextDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            _isLoaded = false;

            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
                Assert.IsNotNull(chart.ToolTipText);

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the Watermark default property value.
        /// </summary>
        [TestMethod]
        public void CheckWatermarkDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            _isLoaded = false;

            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
                Assert.IsTrue(chart.Watermark);

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        #endregion

        #region CheckNewPropertyValue

        /// <summary>
        /// Check View3D new property value
        /// </summary>
        [TestMethod]
        public void CheckView3DNewPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            chart.View3D = true;

            _isLoaded = false;

            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
                Assert.IsTrue(chart.View3D);

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check Opacity new value
        /// </summary>
        [TestMethod]
        public void CheckOpacityNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            chart.Opacity = 0.5;
            _isLoaded = false;

            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.AreEqual(0.5, chart.Opacity, Common.HighPrecisionDelta);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check Cursor new value
        /// </summary>
        [TestMethod]
        public void CheckCursorNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            chart.Cursor = Cursors.Hand;
            _isLoaded = false;

            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.AreEqual(Cursors.Hand, chart.Cursor);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check HrefAndHrefTarget new value
        /// </summary>
        [TestMethod]
        public void CheckHrefAndHrefTargetNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            chart.Href = "http://www.visifire.com";
            chart.HrefTarget = HrefTargets._blank;
            _isLoaded = false;

            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.AreEqual("http://www.visifire.com", chart.Href);
                Assert.AreEqual(HrefTargets._blank, chart.HrefTarget);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check AnimationEnabled new property value
        /// </summary>
        [TestMethod]
        public void CheckAnimationEnabledNewPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            chart.AnimationEnabled = false;
            _isLoaded = false;

            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
                Assert.IsFalse(chart.AnimationEnabled);

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check Bevel new property value
        /// </summary>
        [TestMethod]
        public void CheckBevelNewPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            chart.Bevel = true;

            _isLoaded = false;

            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
                Assert.IsTrue(chart.Bevel);

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check Border Properties new value
        /// </summary>
        [TestMethod]
        public void CheckBorderPropertiesNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            chart.BorderThickness = new Thickness(1);
            chart.BorderBrush = new SolidColorBrush(Colors.Black);

            _isLoaded = false;

            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.AreEqual(new Thickness(1), chart.BorderThickness);
                Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Black), chart.BorderBrush);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check Background new property value
        /// </summary>
        [TestMethod]
        public void CheckBackgroundNewPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            chart.Background = new SolidColorBrush(Colors.Magenta);

            _isLoaded = false;

            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
                Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Magenta), chart.Background);

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check ColorSet new property value
        /// </summary>
        [TestMethod]
        public void CheckColorSetNewPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            chart.ColorSet = "Visifire2";

            _isLoaded = false;

            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
                Assert.AreEqual("Visifire2", chart.ColorSet);

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check ChartSize new property value
        /// </summary>
        [TestMethod]
        public void CheckChartSizePropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            chart.Width = 600;
            chart.Height = 500;

            _isLoaded = false;

            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.AreEqual(600, chart.Width);
                Assert.AreEqual(500, chart.Height);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check Padding new property value
        /// </summary>
        [TestMethod]
        public void CheckPaddingPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            chart.Padding = new Thickness(12);
            _isLoaded = false;

            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.AreEqual(new Thickness(12), chart.Padding);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check LightingEnabled new property value
        /// </summary>
        [TestMethod]
        public void CheckLightingEnabledPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            chart.Background = new SolidColorBrush(Colors.Red);

            chart.LightingEnabled = true;

            _isLoaded = false;

            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
                Assert.IsTrue(chart.LightingEnabled);

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check ShadowEnabled new property value
        /// </summary>
        [TestMethod]
        public void CheckShadowEnabledPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            chart.ShadowEnabled = true;

            _isLoaded = false;

            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
                Assert.IsTrue(chart.ShadowEnabled);

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check ToolTipEnabled new property value
        /// </summary>
        [TestMethod]
        public void CheckToolTipEnabledPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            chart.ToolTipEnabled = false;

            _isLoaded = false;

            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
                Assert.IsFalse(chart.ToolTipEnabled);

            window.Dispatcher.InvokeShutdown();
            window.Close();

        }
        /// <summary>
        /// Check CornerRadius new property value
        /// </summary>
        [TestMethod]
        public void CheckCornerRadiusPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            chart.CornerRadius = new CornerRadius(2, 2, 2, 2);

            _isLoaded = false;

            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
                Assert.AreEqual(new CornerRadius(2, 2, 2, 2), chart.CornerRadius);

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check Theme new property value
        /// </summary>
        [TestMethod]
        public void CheckThemePropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            chart.Theme = "Theme3";

            _isLoaded = false;

            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
                Assert.AreEqual("Theme3", chart.Theme);

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check ToolTipText new property value
        /// </summary>
        [TestMethod]
        public void CheckToolTipTextPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            chart.ToolTipText = "This is a Chart";

            _isLoaded = false;

            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
                Assert.AreEqual("This is a Chart", chart.ToolTipText);

            window.Dispatcher.InvokeShutdown();
            window.Close();

        }

        /// <summary>
        /// Check Watermark new property value
        /// </summary>
        [TestMethod]
        public void CheckWatermarkPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            chart.Watermark = false;

            _isLoaded = false;

            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
                Assert.IsFalse(chart.Watermark);

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        #endregion

        #region CheckControlTemplate
        /// <summary>
        /// Verifies the Control's TemplateParts.
        /// </summary>
        [TestMethod]
        [Description("Verifies the Control's TemplateParts.")]
        public void TemplatePartsAreDefined()
        {
            IDictionary<string, Type> templateParts = DefaultControlToTest.GetType().GetTemplateParts();
            Assert.AreEqual(58, templateParts.Count);
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
            //Assert.AreSame(typeof(Border), templateParts["ToolTip"]);
        }
        #endregion CheckControlTemplate

        #region CheckChartStressViaXaml

        /// <summary>
        /// Stress testing Chart loading via XAML
        /// </summary>
        [TestMethod]
        public void CheckChartStressViaXaml()
        {
            Object result = XamlReader.Load(new XmlTextReader(new StringReader(Resource.Chart_Xaml)));
            Assert.IsInstanceOfType(result, typeof(Chart));

            Chart chart = result as Chart;

            Int32 iterations = 5;
            Common.AssertAverageDuration(50, iterations, delegate
            {
                chart.Series.Add((DataSeries)XamlReader.Load(new XmlTextReader(new StringReader(Resource.DataSeries_BigXaml))));

            });
        }
        #endregion

        /// <summary>
        /// Gets a default instance of Control (or a derived type) to test.
        /// </summary>
        public Control DefaultControlToTest
        {
            get { return new Chart(); }
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
        /// Reference for Chart
        /// </summary>
        private Chart _chart;

        /// <summary>
        /// Whether the chart is loaded
        /// </summary>
        private bool _isLoaded = false;

        #endregion
    }
}
