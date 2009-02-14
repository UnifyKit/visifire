using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Silverlight.Testing;
using Visifire.Charts;
using Visifire.Commons;

namespace SLVisifireChartsTest
{
    /// <summary>
    /// This class runs the unit tests Visifire.Charts.Title class 
    /// </summary>
    [TestClass]
    public class TitleTest : SilverlightControlTest
    {

        #region CheckTitleBackgroundPropertyChanged Event
        /// <summary>
        /// Testing Title Background property changed.
        /// </summary>
        [TestMethod]
        [Description("Testing Title Background property changed.")]
        [Owner("[...]")]
        [Asynchronous]
        public void TestingTitleBackgroundPropertyChanged()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            Title title = new Title();
            title.Text = "Title1";
            title.FontSize = 15;
            title.VerticalAlignment = VerticalAlignment.Top;
            title.HorizontalAlignment = HorizontalAlignment.Center;
            title.Background = new SolidColorBrush(Colors.Magenta);
            chart.Titles.Add(title);

            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            TestPanel.Children.Add(chart);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueSleep(_sleepTime);

            title.PropertyChanged += (object sender, System.ComponentModel.PropertyChangedEventArgs e)
                =>
            {
                Assert.AreEqual("Background", e.PropertyName);
            };

            EnqueueCallback(() => title.Background = new SolidColorBrush(Colors.Yellow));

            EnqueueTestComplete();
        }

        #endregion

        #region CheckDefaultPropertyValue

        [TestMethod]
        [Description("Check the default value of Background of Title")]
        [Owner("[....]")]
        [Asynchronous]
        public void BackgroundDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;
            Common.CreateAndAddDefaultDataSeries(chart);

            Title title = TitleToTest;
            chart.Titles.Add(title);

            CreateAsyncTask(chart,
                () => Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Transparent), chart.Titles[0].Background));

            EnqueueTestComplete();
        }

        [TestMethod]
        [Description("Check the default value of FontFamily of Title")]
        [Owner("[....]")]
        [Asynchronous]
        //[Ignore, Bug("Visifire")]
        public void FontFamilyDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            Title title = TitleToTest;
            chart.Titles.Add(title);

            EnqueueSleep(_sleepTime);

            CreateAsyncTask(chart,
                () => Assert.AreEqual(new FontFamily("Verdana"), chart.Titles[0].FontFamily));

            EnqueueTestComplete();
        }

        /// <summary> 
        /// Check the default value of FontSize. 
        /// </summary>
        [TestMethod]
        [Description("Check the default value of FontSize.")]
        [Owner("[....]")]
        [Asynchronous]
        public void FontSizeDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            Title title = TitleToTest;
            chart.Titles.Add(title);

            EnqueueSleep(_sleepTime);

            CreateAsyncTask(chart,
                () => Assert.AreEqual(12, (Double)chart.Titles[0].FontSize, Common.HighPrecisionDelta));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the default value of FontStretch. 
        /// </summary>
        [TestMethod]
        [Description("Check the default value of FontStretch.")]
        [Owner("[....]")]
        [Asynchronous]
        public void FontColorDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            Title title = TitleToTest;
            chart.Titles.Add(title);

            EnqueueSleep(_sleepTime);

            CreateAsyncTask(chart,
                () => Assert.IsNull(chart.Titles[0].FontColor));

            EnqueueTestComplete();
        }

        /// <summary> 
        /// Check the default value of FontStyle.
        /// </summary> 
        [TestMethod]
        [Description("Check the default value of FontStyle.")]
        [Owner("[....]")]
        [Asynchronous]
        public void FontStyleDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            Title title = TitleToTest;
            chart.Titles.Add(title);

            EnqueueSleep(_sleepTime);

            CreateAsyncTask(chart,
                () => Assert.AreEqual(FontStyles.Normal, chart.Titles[0].FontStyle));

            EnqueueTestComplete();
        }

        /// <summary> 
        /// Check the default value of FontWeight.
        /// </summary>
        [TestMethod]
        [Description("Check the default value of FontWeight.")]
        [Owner("[....]")]
        [Asynchronous]
        public void FontWeightDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            Title title = TitleToTest;
            chart.Titles.Add(title);

            EnqueueSleep(_sleepTime);

            CreateAsyncTask(chart,
                () => Assert.AreEqual(FontWeights.Normal, chart.Titles[0].FontWeight));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the default value of HorizontalTitleAlignment. 
        /// </summary> 
        [TestMethod]
        [Description("Check the default value of HorizontalAlignment.")]
        [Owner("[....]")]
        [Asynchronous]
        public void HorizontalAlignmentDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            Title title = TitleToTest;
            chart.Titles.Add(title);

            EnqueueSleep(_sleepTime);

            CreateAsyncTask(chart,
                () => Assert.AreEqual(HorizontalAlignment.Center, chart.Titles[0].HorizontalAlignment));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the default value of VerticalTitleAlignment. 
        /// </summary> 
        [TestMethod]
        [Description("Check the default value of VerticalAlignment.")]
        [Owner("[....]")]
        [Asynchronous]
        public void VerticalAlignmentDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            Title title = TitleToTest;
            chart.Titles.Add(title);

            EnqueueSleep(_sleepTime);

            CreateAsyncTask(chart,
                () => Assert.AreEqual(VerticalAlignment.Top, chart.Titles[0].VerticalAlignment));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the default value of Padding. 
        /// </summary> 
        [TestMethod]
        [Description("Check the default value of Padding.")]
        [Owner("[....]")]
        [Asynchronous]
        public void PaddingDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            Title title = TitleToTest;
            chart.Titles.Add(title);

            EnqueueSleep(_sleepTime);

            CreateAsyncTask(chart,
                () => Assert.AreEqual(new Thickness(0), chart.Titles[0].Padding));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the default value of Padding. 
        /// </summary> 
        [TestMethod]
        [Description("Check the default value of Margin.")]
        [Owner("[....]")]
        [Asynchronous]
        public void MarginDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            Title title = TitleToTest;
            chart.Titles.Add(title);

            EnqueueSleep(_sleepTime);

            CreateAsyncTask(chart,
                () => Assert.AreEqual(new Thickness(0), chart.Titles[0].Margin));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the default value of DockInsidePlotArea. 
        /// </summary> 
        [TestMethod]
        [Description("Check the default value of DockInsidePlotArea.")]
        [Owner("[....]")]
        [Asynchronous]
        public void DockInsidePlotAreaDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            Title title = TitleToTest;
            chart.Titles.Add(title);

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => Assert.AreEqual(false, chart.Titles[0].DockInsidePlotArea));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the default value of Text. 
        /// </summary> 
        [TestMethod]
        [Description("Check the default value of Text.")]
        [Owner("[....]")]
        [Asynchronous]
        public void TextDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            Title title = new Title();
            chart.Titles.Add(title);

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => Assert.AreEqual("", chart.Titles[0].Text));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the default value of CornerRadius. 
        /// </summary> 
        [TestMethod]
        [Description("Check the default value of CornerRadius.")]
        [Owner("[....]")]
        [Asynchronous]
        public void CornerRadiusDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            Title title = TitleToTest;
            chart.Titles.Add(title);

            EnqueueSleep(_sleepTime);

            CreateAsyncTask(chart,
                () => Assert.AreEqual(new CornerRadius(0, 0, 0, 0), chart.Titles[0].CornerRadius));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the default value of TextAlignment. 
        /// </summary> 
        [TestMethod]
        [Description("Check the default value of TextAlignment.")]
        [Owner("[....]")]
        [Asynchronous]
        public void TextAlignmentDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            Title title = TitleToTest;
            title.HorizontalAlignment = HorizontalAlignment.Stretch;
            title.Background = new SolidColorBrush(Colors.LightGray);
            chart.Titles.Add(title);

            EnqueueSleep(_sleepTime);

            CreateAsyncTask(chart,
                () => Assert.AreEqual(TextAlignment.Center, chart.Titles[0].TextAlignment));

            EnqueueTestComplete();
        }

        #endregion CheckDefaultPropertyValue

        #region CheckNewPropertyValue
        /// <summary> 
        /// Check Titles new property values
        /// </summary> 
        [TestMethod]
        [Description("Check all the values of Titles")]
        [Owner("[....]")]
        [Asynchronous]
        public void TitleNewPropertiesValueCheck()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Common.CreateAndAddDefaultDataSeries(chart);

            foreach (Title title in TitlesToTest)
                chart.Titles.Add(title);
            EnqueueSleep(_sleepTime);

            CreateAsyncTask(chart,
                delegate
                {
                    foreach (Title title in chart.Titles)
                    {
                        title.DockInsidePlotArea = true;
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
                        title.ToolTipText = "Title";
                    }
                });

            EnqueueConditional(() => { return _isLoaded; });

            EnqueueCallback(() =>
                {
                    foreach (Title title in chart.Titles)
                    {
                        Assert.IsTrue(title.DockInsidePlotArea);
                        Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Red), title.Background);
                        Assert.AreEqual(new Thickness(1), title.BorderThickness);
                        Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Yellow), title.BorderColor);
                        Assert.AreEqual("Title", title.Text);
                        Assert.AreEqual(TextAlignment.Center, title.TextAlignment);
                        Assert.AreEqual(new FontFamily("Trebuchet MS"), title.FontFamily);
                        Assert.AreEqual(11, (Double)title.FontSize, Common.HighPrecisionDelta);
                        Assert.AreEqual(FontStyles.Normal, title.FontStyle);
                        Assert.AreEqual(FontWeights.Bold, title.FontWeight);
                        Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.White), title.FontColor);
                        Assert.AreEqual(HorizontalAlignment.Center, title.HorizontalAlignment);
                        Assert.AreEqual(VerticalAlignment.Top, title.VerticalAlignment);
                        Assert.AreEqual(new Thickness(5), title.Padding);
                        Assert.AreEqual(new Thickness(2), title.Margin);
                        Assert.AreEqual(new CornerRadius(1, 1, 1, 1), title.CornerRadius);
                        Assert.AreEqual("Title", title.ToolTipText);
                    }
                });

            EnqueueTestComplete();
        }

        #endregion CheckNewPropertyValue

        #region CheckTextAlignmentProperty
        /// <summary> 
        /// Ensure TextAlignment values are consistent across gets and sets.
        /// </summary> 
        [TestMethod]
        [Description("Ensure TextAlignment values are consistent across gets and sets.")]
        [Owner("[....]")]
        [Asynchronous]
        public void TitleTextAlignment()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            foreach (Title title in TitlesToTest)
                chart.Titles.Add(title);
            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                delegate
                {
                    foreach (Title title in chart.Titles)
                    {
                        TextAlignment value = TextAlignment.Right;
                        title.TextAlignment = TextAlignment.Right;
                        Assert.AreEqual(value, title.TextAlignment);

                        value = TextAlignment.Center;
                        title.TextAlignment = TextAlignment.Center;
                        Assert.AreEqual(value, title.TextAlignment);

                        value = TextAlignment.Left;
                        title.TextAlignment = TextAlignment.Left;
                        Assert.AreEqual(value, title.TextAlignment);

                    }
                }
            );
            EnqueueTestComplete();
        }

        #endregion CheckTextAlignmentProperty

        #region CheckVerticalAlignment

        /// <summary> 
        /// Ensure VerticalAlignment values are consistent across gets and sets.
        /// </summary> 
        [TestMethod]
        [Description("Ensure VerticalAlignment values are consistent across gets and sets.")]
        [Owner("[....]")]
        [Asynchronous]
        public void TitleVerticalAlignment()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            foreach (Title title in TitlesToTest)
                chart.Titles.Add(title);
            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                delegate
                {
                    foreach (Title title in chart.Titles)
                    {
                        VerticalAlignment value = VerticalAlignment.Top;
                        title.VerticalAlignment = VerticalAlignment.Top;
                        Assert.AreEqual(value, title.VerticalAlignment);

                        value = VerticalAlignment.Bottom;
                        title.VerticalAlignment = VerticalAlignment.Bottom;
                        Assert.AreEqual(value, title.VerticalAlignment);

                        value = VerticalAlignment.Center;
                        title.VerticalAlignment = VerticalAlignment.Center;
                        Assert.AreEqual(value, title.VerticalAlignment);

                        value = VerticalAlignment.Stretch;
                        title.VerticalAlignment = VerticalAlignment.Stretch;
                        Assert.AreEqual(value, title.VerticalAlignment);

                        EnqueueSleep(_sleepTime);
                    }
                }
            );
            EnqueueTestComplete();
        }

        #endregion CheckVerticalAlignment

        #region CheckHorizontalAlignment

        /// <summary> 
        /// Ensure HorizontalAlignment values are consistent across gets and sets.
        /// </summary> 
        [TestMethod]
        [Description("Ensure HorizontalAlignment values are consistent across gets and sets.")]
        [Owner("[....]")]
        [Asynchronous]
        public void TitleHorizontalAlignment()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            foreach (Title title in TitlesToTest)
                chart.Titles.Add(title);
            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                delegate
                {
                    foreach (Title title in chart.Titles)
                    {
                        HorizontalAlignment value = HorizontalAlignment.Center;
                        title.HorizontalAlignment = HorizontalAlignment.Center;
                        Assert.AreEqual(value, title.HorizontalAlignment);

                        value = HorizontalAlignment.Left;
                        title.HorizontalAlignment = HorizontalAlignment.Left;
                        Assert.AreEqual(value, title.HorizontalAlignment);

                        value = HorizontalAlignment.Right;
                        title.HorizontalAlignment = HorizontalAlignment.Right;
                        Assert.AreEqual(value, title.HorizontalAlignment);

                        value = HorizontalAlignment.Stretch;
                        title.HorizontalAlignment = HorizontalAlignment.Stretch;
                        Assert.AreEqual(value, title.HorizontalAlignment);
                    }
                }
            );
            EnqueueTestComplete();
        }

        #endregion CheckHorizontalAlignment

        #region TitleEventTesting
        /// <summary>
        /// Testing Evnets in Title
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void TitleEventChecking()
        {
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "400px");

            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            TestPanel.Children.Add(chart);

            Title title = TitleToTest;
            title.FontSize = 16;
            title.FontColor = new SolidColorBrush(Colors.LightGray);
            title.HorizontalAlignment = HorizontalAlignment.Center;
            chart.Titles.Add(title);

            Common.CreateAndAddDefaultDataSeries(chart);

            title.MouseEnter += delegate(Object sender, MouseEventArgs e)
             {
                 _htmlElement1.SetProperty("value", "Title MouseEnter event fired");
             };

            title.MouseLeave += delegate(Object sender, MouseEventArgs e)
            {
                _htmlElement1.SetProperty("value", "Title MouseLeave event fired");
            };

            title.MouseLeftButtonUp += delegate(Object sender, MouseButtonEventArgs e)
            {
                _htmlElement1.SetProperty("value", "Title MouseLeftButtonUp event fired");
            };

            title.MouseLeftButtonDown += delegate(Object sender, MouseButtonEventArgs e)
            {
                _htmlElement1.SetProperty("value", "Title MouseLeftButtonDown event fired");
            };

            EnqueueCallback(() =>
            {
                _htmlElement1.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.TitleTest_OnClick));
            });

            _htmlElement1 = Common.GetDisplayMessageButton(_htmlElement1);
            _htmlElement1.SetStyleAttribute("width", "900px");
            _htmlElement1.SetProperty("value", "Click here to exit.");
            System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement1);

        }
        #endregion

        #region Performance and Stress
        /// <summary> 
        /// Stress test Title creation.
        /// </summary>
        [TestMethod]
        [Description("Stress test Title creation.")]
        [Owner("[....]")]
        [Asynchronous]
        public void StressCreate()
        {
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "400px");

            Double totalDuration = 0;
            DateTime start = DateTime.UtcNow;

            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            this.TestPanel.Children.Add(chart);

            Int32 i = 0;

            Random rand = new Random();
            Int32 numberOfTitles = 0;
       
            Canvas c = new Canvas();
            string msg = Common.AssertAverageDuration(100, 1, delegate
            {
                for (i = 0; i < 20; i++)
                {
                    Title title = new Title();
                    title.Background = new SolidColorBrush(Color.FromArgb((Byte)255, (Byte)rand.Next(0, 255), (Byte)rand.Next(0, 255), (Byte)rand.Next(0, 255)));
                    title.Text = "Title" + (i + 1);
                    title.VerticalAlignment = (VerticalAlignment)rand.Next(0, 3);
                    title.HorizontalAlignment = (HorizontalAlignment)rand.Next(0, 3);
                    chart.Titles.Add(title);
                    numberOfTitles++;
                }
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
                _htmlElement1.SetProperty("value", numberOfTitles + " Titles are added. Click here to exit.");
                _htmlElement2 = Common.GetDisplayMessageButton(_htmlElement2);
                _htmlElement2.SetStyleAttribute("top", "540px");
                _htmlElement2.SetProperty("value", msg + " Total Chart Loading Time: " + totalDuration + "s");
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement1);
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement2);
            });

            EnqueueCallback(() =>
            {
                _htmlElement1.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.TitleTest_OnClick));
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
                yield return new Title() { Text="Visifire"};

            }
        }

        /// <summary>
        /// New Title instance
        /// </summary>
        public Title TitleToTest
        {
            get
            {
                // Simple standard Title 
                return new Title() { Text = "Visifire" };

            }
        }

        /// <summary>
        /// Event handler for click event of html element
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TitleTest_OnClick(object sender, System.Windows.Browser.HtmlEventArgs e)
        {
            EnqueueTestComplete();
            System.Windows.Browser.HtmlPage.Document.Body.RemoveChild(_htmlElement1);
            System.Windows.Browser.HtmlPage.Document.Body.RemoveChild(_htmlElement2);
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "100%");
        }

        /// <summary>
        /// Event handler for loaded event of the chart
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chart_Loaded(object sender, RoutedEventArgs e)
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
    