using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Windows.Input;
using System.Collections.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Visifire.Charts;
using Visifire.Commons;

namespace WPFVisifireChartsTest
{
    /// <summary>
    /// This class runs the unit tests Visifire.Charts.Title class 
    /// </summary>
    [TestClass]
    public class TitleTest
    {
        #region CheckTitleBackgroundPropertyChanged Event
        /// <summary>
        /// Testing Title Background property changed.
        /// </summary>
        [TestMethod]
        [Description("Testing Title Background property changed.")]
        [Owner("[...]")]
        public void TestingTitleBackgroundPropertyChanged()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            chart.Titles = new TitleCollection();

            Title title = new Title();
            title.Text = "Title1";
            title.FontSize = 15;
            title.VerticalAlignment = VerticalAlignment.Top;
            title.HorizontalAlignment = HorizontalAlignment.Center;
            title.Background = new SolidColorBrush(Colors.Magenta);
            chart.Titles.Add(title);
      
            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                title.PropertyChanged += (object sender, System.ComponentModel.PropertyChangedEventArgs e)
                    =>
                {
                    Assert.AreEqual("Background", e.PropertyName);
                };

                title.Background = new SolidColorBrush(Colors.Yellow);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        #endregion

        #region CheckDefaultPropertyValue

        [TestMethod]
        [Description("Check the default value of Background of Title")]
        [Owner("[....]")]
        //[Ignore, Bug("Visifire")]
        public void BackgroundDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            Title title = new Title();
            chart.Titles.Add(title);
                 
            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Transparent), chart.Titles[0].Background);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        [TestMethod]
        [Description("Check the default value of FontFamily of Title")]
        [Owner("[....]")]
        //[Ignore, Bug("Visifire")]
        public void FontFamilyDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            Title title = new Title();
            chart.Titles.Add(title);
                 
            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.AreEqual(new FontFamily("Verdana"), chart.Titles[0].FontFamily);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary> 
        /// Check the default value of FontSize. 
        /// </summary>
        [TestMethod]
        [Description("Check the default value of FontSize.")]
        [Owner("[....]")]
        public void FontSizeDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            Title title = new Title();
            chart.Titles.Add(title);
                 
            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.AreEqual(12, (Double)chart.Titles[0].FontSize, Common.HighPrecisionDelta);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the default value of FontStretch. 
        /// </summary>
        [TestMethod]
        [Description("Check the default value of FontStretch.")]
        [Owner("[....]")]
        public void FontColorDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            Title title = new Title();
            chart.Titles.Add(title);
                 
            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.IsNull(chart.Titles[0].FontColor);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary> 
        /// Check the default value of FontStyle.
        /// </summary> 
        [TestMethod]
        [Description("Check the default value of FontStyle.")]
        [Owner("[....]")]
        public void FontStyleDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            Title title = new Title();
            chart.Titles.Add(title);
                 
            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.AreEqual(FontStyles.Normal, chart.Titles[0].FontStyle);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary> 
        /// Check the default value of FontWeight.
        /// </summary>
        [TestMethod]
        [Description("Check the default value of FontWeight.")]
        [Owner("[....]")]
        // [Ignore, Bug("Jolt #15464")] 
        public void FontWeightDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            Title title = new Title();
            chart.Titles.Add(title);
                 
            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.AreEqual(FontWeights.Normal, chart.Titles[0].FontWeight);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the default value of HorizontalTitleAlignment. 
        /// </summary> 
        [TestMethod]
        [Description("Check the default value of HorizontalAlignment.")]
        [Owner("[....]")]
        public void HorizontalAlignmentDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            Title title = new Title();
            chart.Titles.Add(title);
                 
            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.AreEqual(HorizontalAlignment.Center, chart.Titles[0].HorizontalAlignment);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the default value of VerticalTitleAlignment. 
        /// </summary> 
        [TestMethod]
        [Description("Check the default value of VerticalAlignment.")]
        [Owner("[....]")]
        public void VerticalAlignmentDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            Title title = new Title();
            chart.Titles.Add(title);
                 
            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.AreEqual(VerticalAlignment.Top, chart.Titles[0].VerticalAlignment);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the default value of Padding. 
        /// </summary> 
        [TestMethod]
        [Description("Check the default value of Padding.")]
        [Owner("[....]")]
        public void PaddingDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            Title title = new Title();
            chart.Titles.Add(title);
                 
            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.AreEqual(new Thickness(0), chart.Titles[0].Padding);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the default value of Padding. 
        /// </summary> 
        [TestMethod]
        [Description("Check the default value of Margin.")]
        [Owner("[....]")]
        public void MarginDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            Title title = new Title();
            chart.Titles.Add(title);
                 
            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.AreEqual(new Thickness(0), chart.Titles[0].Margin);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the default value of DockInsidePlotArea. 
        /// </summary> 
        [TestMethod]
        [Description("Check the default value of DockInsidePlotArea.")]
        [Owner("[....]")]
        public void DockInsidePlotAreaDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            Title title = new Title();
            chart.Titles.Add(title);

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.AreEqual(false, chart.Titles[0].DockInsidePlotArea);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the default value of Text. 
        /// </summary> 
        [TestMethod]
        [Description("Check the default value of Text.")]
        [Owner("[....]")]
        public void TextDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            Title title = new Title();
            chart.Titles.Add(title);
                 
            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.AreEqual("", chart.Titles[0].Text);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the default value of CornerRadius. 
        /// </summary> 
        [TestMethod]
        [Description("Check the default value of CornerRadius.")]
        [Owner("[....]")]
        public void CornerRadiusDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            Title title = new Title();
            chart.Titles.Add(title);
                 
            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.AreEqual(new CornerRadius(0, 0, 0, 0), chart.Titles[0].CornerRadius);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the default value of TextAlignment. 
        /// </summary> 
        [TestMethod]
        [Description("Check the default value of TextAlignment.")]
        [Owner("[....]")]
        public void TextAlignmentDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            Title title = new Title();
            chart.Titles.Add(title);
                 
            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.AreEqual(TextAlignment.Left, chart.Titles[0].TextAlignment);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        #endregion CheckDefaultPropertyValue

        #region CheckNewPropertyValue
        /// <summary> 
        /// Check Titles new property values
        /// </summary> 
        [TestMethod]
        [Description("Check all the values of Titles")]
        [Owner("[....]")]
        public void TitleNewValuePropertiesCheck()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            foreach (Title title in TitlesToTest)
                chart.Titles.Add(title);

            foreach (Title title in chart.Titles)
            {
                title.DockInsidePlotArea = false;
                title.Background = new SolidColorBrush(Colors.Red);
                title.BorderThickness = new Thickness(1);
                title.BorderColor = new SolidColorBrush(Colors.Yellow);
                title.Text = "Title";
                title.TextAlignment = TextAlignment.Center;
                title.FontFamily = new FontFamily("Trebuchet MS");
                title.FontSize = 11;
                title.FontStyle = FontStyles.Normal;
                title.FontWeight = FontWeights.Bold;
                title.FontColor = new SolidColorBrush(Colors.White);
                title.HorizontalAlignment = HorizontalAlignment.Center;
                title.VerticalAlignment = VerticalAlignment.Top;
                title.Padding = new Thickness(5);
                title.Margin = new Thickness(2);
                title.CornerRadius = new CornerRadius(1, 1, 1, 1);
            }
                 
            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                foreach (Title title in chart.Titles) Assert.AreEqual(false, title.DockInsidePlotArea);
                foreach (Title title in chart.Titles) Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Red), title.Background);
                foreach (Title title in chart.Titles) Assert.AreEqual(new Thickness(1), title.BorderThickness);
                foreach (Title title in chart.Titles) Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Yellow), title.BorderColor);
                foreach (Title title in chart.Titles) Assert.AreEqual("Title", title.Text);
                foreach (Title title in chart.Titles) Assert.AreEqual(TextAlignment.Center, title.TextAlignment);
                foreach (Title title in chart.Titles) Assert.AreEqual(new FontFamily("Trebuchet MS"), title.FontFamily);
                foreach (Title title in chart.Titles) Assert.AreEqual(11, (Double)title.FontSize, Common.HighPrecisionDelta);
                foreach (Title title in chart.Titles) Assert.AreEqual(FontStyles.Normal, title.FontStyle);
                foreach (Title title in chart.Titles) Assert.AreEqual(FontWeights.Bold, title.FontWeight);
                foreach (Title title in chart.Titles) Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.White), title.FontColor);
                foreach (Title title in chart.Titles) Assert.AreEqual(HorizontalAlignment.Center, title.HorizontalAlignment);
                foreach (Title title in chart.Titles) Assert.AreEqual(VerticalAlignment.Top, title.VerticalAlignment);
                foreach (Title title in chart.Titles) Assert.AreEqual(new Thickness(5), title.Padding);
                foreach (Title title in chart.Titles) Assert.AreEqual(new Thickness(2), title.Margin);
                foreach (Title title in chart.Titles) Assert.AreEqual(new CornerRadius(1, 1, 1, 1), title.CornerRadius);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        #endregion CheckNewPropertyValue

        #region CheckTextAlignmentProperty
        /// <summary> 
        /// Ensure TextAlignment values are consistent across gets and sets.
        /// </summary> 
        [TestMethod]
        [Description("Ensure TextAlignment values are consistent across gets and sets.")]
        [Owner("[....]")]
        public void TitleTextAlignment()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            foreach (Title title in TitlesToTest)
                chart.Titles.Add(title);

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                foreach (Title title in chart.Titles)
                {
                    TextAlignment value = TextAlignment.Right;
                    title.TextAlignment = value;
                    Assert.AreEqual(value, title.TextAlignment);

                    value = TextAlignment.Center;
                    title.TextAlignment = value;
                    Assert.AreEqual(value, title.TextAlignment);

                    value = TextAlignment.Left;
                    title.TextAlignment = value;
                    Assert.AreEqual(value, title.TextAlignment);
                }
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        #endregion CheckTextAlignmentProperty

        #region CheckVerticalAlignment

        /// <summary> 
        /// Ensure VerticalAlignment values are consistent across gets and sets.
        /// </summary> 
        [TestMethod]
        [Description("Ensure VerticalAlignment values are consistent across gets and sets.")]
        [Owner("[....]")]
        public void TitleVerticalAlignment()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            foreach (Title title in TitlesToTest)
                chart.Titles.Add(title);
                 
            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                foreach (Title title in chart.Titles)
                {
                    VerticalAlignment value = VerticalAlignment.Top;
                    title.VerticalAlignment = value;
                    Assert.AreEqual(value, title.VerticalAlignment);

                    value = VerticalAlignment.Bottom;
                    title.VerticalAlignment = value;
                    Assert.AreEqual(value, title.VerticalAlignment);

                    value = VerticalAlignment.Center;
                    title.VerticalAlignment = value;
                    Assert.AreEqual(value, title.VerticalAlignment);

                    value = VerticalAlignment.Stretch;
                    title.VerticalAlignment = value;
                    Assert.AreEqual(value, title.VerticalAlignment);
                }
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        #endregion CheckVerticalAlignment

        #region CheckHorizontalAlignment

        /// <summary> 
        /// Ensure HorizontalAlignment values are consistent across gets and sets.
        /// </summary> 
        [TestMethod]
        [Description("Ensure HorizontalAlignment values are consistent across gets and sets.")]
        [Owner("[....]")]
        public void TitleHorizontalAlignment()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            foreach (Title title in TitlesToTest)
                chart.Titles.Add(title);
                 
            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                foreach (Title title in chart.Titles)
                {
                    HorizontalAlignment value = HorizontalAlignment.Center;
                    title.HorizontalAlignment = value;
                    Assert.AreEqual(value, title.HorizontalAlignment);

                    value = HorizontalAlignment.Left;
                    title.HorizontalAlignment = value;
                    Assert.AreEqual(value, title.HorizontalAlignment);

                    value = HorizontalAlignment.Right;
                    title.HorizontalAlignment = value;
                    Assert.AreEqual(value, title.HorizontalAlignment);

                    value = HorizontalAlignment.Stretch;
                    title.HorizontalAlignment = value;
                    Assert.AreEqual(value, title.HorizontalAlignment);
                }
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        #endregion CheckHorizontalAlignment

        #region Performance and Stress
        /// <summary> 
        /// Stress test Title creation.
        /// </summary>
        [TestMethod]
        [Description("Stress test Title creation.")]
        [Owner("[....]")]
        //[Ignore] //Test fails on slower machines and needs to be rewritten to consider that.
        public void TitleStressCreate()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            Title title = new Title();

            Int32 i = 0;

            string msg = Common.AssertAverageDuration(500, 1, delegate
            {
                for (i = 0; i < 20; i++)
                {

                    title.Text = "Title" + i;
                    chart.Titles.Add(title);
                }
            });
        }
        #endregion Performance and Stress

        /// <summary>
        /// Instances that should be used across all Title tests.
        /// </summary>
        public IEnumerable<Title> TitlesToTest
        {
            get
            {
                // Simple standard Title 
                yield return new Title();

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

        #region Private Data
        /// <summary>
        /// Whether the chart is loaded
        /// </summary>
        private bool _isLoaded = false;

        #endregion
    }
}
