using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
#if WPF
public class SilverlightTest { } 
#else
using Microsoft.Silverlight.Testing;
#endif
using System.Reflection;
using System.Linq;
using System.Collections.ObjectModel;
using Visifire.Charts;
using Visifire.Commons;
#if WPF
namespace WPF
#else
namespace SLVisifireChartsTest
#endif
{
    //[TestClass]
    public class ChartAreaTest : SilverlightTest
    {

        void chart_Loaded(object sender, RoutedEventArgs e)
        {
            isLoaded = true;
        }

        #region TestingPlotAreaPropertyChanged Event
        /// <summary>
        /// Testing the PlotArea Property changed
        /// </summary>
        [TestMethod]
        [Description("Testing the PlotArea property changed event")]
        [Owner("[...]")]
        [Asynchronous]
        public void TestingPlotAreaPropertyChanged()
        {
            chart = new Chart();

            isLoaded = false;
            chart.Width = 400;
            chart.Height = 300;
            chart.AnimationEnabled = false;

            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Common.CreateAndAddDefaultDataSeries(chart);

            TestPanel.Children.Add(chart);

            EnqueueSleep(sleepTime);

            chart.PlotArea = new PlotArea();

            bool isPropertyChanged = false;

            EnqueueConditional(() => { return isLoaded; });

            EnqueueCallback(() =>
            {
                chart.PlotArea.Background = new SolidColorBrush(Colors.Red);
                isPropertyChanged = true;
            });

            EnqueueConditional(() => { return isPropertyChanged; });
            isPropertyChanged = false;
            EnqueueSleep(sleepTime);

            EnqueueCallback(() =>
                {
                    chart.PlotArea.Bevel = true;
                    isPropertyChanged = true;
                });

            EnqueueConditional(() => { return isPropertyChanged; });
            EnqueueSleep(sleepTime);

            EnqueueCallback(() =>
            {
                chart.PlotArea.BorderColor = new SolidColorBrush(Colors.Green);
                isPropertyChanged = true;
            });

            EnqueueConditional(() => { return isPropertyChanged; });
            EnqueueSleep(sleepTime);

            EnqueueCallback(() =>
            {
                chart.PlotArea.BorderThickness = new Thickness(2);
                isPropertyChanged = true;
            });

            EnqueueConditional(() => { return isPropertyChanged; });
            EnqueueSleep(sleepTime);

            EnqueueCallback(() =>
            {
                chart.PlotArea.BorderStyle = BorderStyles.Dashed;
                isPropertyChanged = true;
            });

            EnqueueConditional(() => { return isPropertyChanged; });
            EnqueueSleep(sleepTime);

            EnqueueCallback(() =>
            {
                chart.PlotArea.CornerRadius = new CornerRadius(5);
                isPropertyChanged = true;
            });

            EnqueueConditional(() => { return isPropertyChanged; });
            EnqueueSleep(sleepTime);

            EnqueueCallback(() =>
            {
                chart.PlotArea.LightingEnabled = true;
                isPropertyChanged = true;
            });

            EnqueueConditional(() => { return isPropertyChanged; });
            EnqueueSleep(sleepTime);

            EnqueueCallback(() =>
            {
                chart.PlotArea.ShadowEnabled = true;
                isPropertyChanged = true;
            });

            EnqueueConditional(() => { return isPropertyChanged; });
            EnqueueSleep(5000);

            //EnqueueCallback(() =>
            //    {
            //        Assert.IsTrue(backgroundFired, "The PropertyChanged event for Name did not fire");
            //        Assert.IsTrue(bevelFired, "The PropertyChanged event for Bio did not fire");
            //        Assert.IsTrue(cornerRadiusFired, "The PropertyChanged event for Tweet did not fire");
            //        Assert.IsTrue(borderColorFired, "The PropertyChanged event for DateAndLocation did not fire");
            //        Assert.IsTrue(borderThicknessFired, "The PropertyChanged event for ProfilePictureUrl did not fire");
            //        Assert.IsTrue(borderStyleFired, "The PropertyChanged event for TwitterUsername did not fire");
            //        Assert.IsTrue(lightingEnabledFired, "The PropertyChanged event for FlikrFeedUrl did not fire");
            //        Assert.IsTrue(shadowEnabledFired, "The PropertyChanged event for FlikrFeedUrl did not fire");
            //    });

            //EnqueueSleep(5000);
            EnqueueTestComplete();
        }
        #endregion

        #region Private Data

        const int sleepTime = 1000;
        PropertyInfo[] properties;
        int numberOfProperties;
        Chart chart;
        bool isLoaded = false;

        #endregion
    }
}
