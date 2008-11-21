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
    /// Summary description for ChartTest
    /// </summary>
    [TestClass]
    public class ChartTest
    {
        void chart_Loaded(object sender, RoutedEventArgs e)
        {
            isLoaded = true;
        }

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

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
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

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
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

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
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

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
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
            chart = new Chart();
            isLoaded = false;

            chart.Width = 400;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            Assert.IsTrue(isLoaded);

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

            isLoaded = false;

            Common.CreateAndAddDefaultDataSeries(chart);

            chart.Titles = new ObservableCollection<Title>();

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
            if (isLoaded)
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

            isLoaded = false;
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
                if (isLoaded)
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

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
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

            isLoaded = true;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
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

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
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

            isLoaded = false;

            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
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

            isLoaded = false;

            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
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

            isLoaded = false;

            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
                Assert.AreEqual(new Thickness(1), chart.BorderThickness);

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the BorderStyle default property value.
        /// </summary>
        [TestMethod]
        public void CheckBorderStyleDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            isLoaded = false;

            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
                Assert.AreEqual(BorderStyles.Solid, chart.BorderStyle);

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the Color default property value.
        /// </summary>
        [TestMethod]
        public void CheckBackgroundDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            isLoaded = false;

            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
                Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Transparent), chart.Background);

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

            isLoaded = false;

            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
                Assert.AreEqual(Visifire.Commons.ColorSetNames.Visifire1, chart.ColorSet);

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

            isLoaded = false;

            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
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

            isLoaded = false;

            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
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

            isLoaded = false;

            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
                Assert.IsFalse(chart.ShadowEnabled);

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

            isLoaded = false;

            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
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

            isLoaded = false;

            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
                Assert.IsNull(chart.ToolTipText);

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

            isLoaded = false;

            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
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

            isLoaded = false;

            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
                Assert.IsTrue(chart.View3D);

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

            isLoaded = false;

            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
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

            isLoaded = false;

            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
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
            chart.BorderStyle = BorderStyles.Dashed;

            isLoaded = false;

            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                Assert.AreEqual(new Thickness(1), chart.BorderThickness);
                Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Black), chart.BorderBrush);
                Assert.AreEqual(BorderStyles.Dashed, chart.BorderStyle);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check Color new property value
        /// </summary>
        [TestMethod]
        public void CheckColorNewPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            chart.Background = new SolidColorBrush(Colors.Magenta);

            isLoaded = false;

            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
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

            chart.ColorSet = Visifire.Commons.ColorSetNames.Visifire2;

            isLoaded = false;

            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
                Assert.AreEqual(Visifire.Commons.ColorSetNames.Visifire2, chart.ColorSet);

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

            isLoaded = false;

            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                Assert.AreEqual(600, chart.Width);
                Assert.AreEqual(500, chart.Height);
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

            isLoaded = false;

            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
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

            isLoaded = false;

            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
                Assert.IsTrue(chart.ShadowEnabled);

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

            isLoaded = false;

            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
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

            isLoaded = false;

            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
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

            isLoaded = false;

            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
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

            isLoaded = false;

            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
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
            Assert.AreEqual(60, templateParts.Count);
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

        #region Private Data

        const int sleepTime = 1000;
        Chart chart;
        bool isLoaded = false;

        #endregion
    }
}
