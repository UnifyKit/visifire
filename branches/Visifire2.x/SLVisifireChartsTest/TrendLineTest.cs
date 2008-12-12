using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Silverlight.Testing;
using Visifire.Charts;
using Visifire.Commons;

namespace SLVisifireChartsTest
{
    //[TestClass]
    public class TrendLineTest:SilverlightControlTest
    {

        /// <summary>
        /// Check the default value of Enabled
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void EnabledDefaultValue()
        {
            Chart chart = new Chart();
            chart.AnimationEnabled = false;
            chart.Width = 500;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            TrendLine trendLine = TrendLineToTest;
            chart.TrendLines.Add(trendLine);

            EnqueueSleep(sleepTime);
            CreateAsyncTask(chart,
                () => Assert.IsTrue((Boolean)trendLine.Enabled));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the default value of Orientation
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void OrientationDefaultValue()
        {
            Chart chart = new Chart();
            chart.AnimationEnabled = false;
            chart.Width = 500;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            TrendLine trendLine = new TrendLine();
            chart.TrendLines.Add(trendLine);

            EnqueueSleep(sleepTime);
            CreateAsyncTask(chart,
                () => Assert.AreEqual(Orientation.Vertical, trendLine.Orientation));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the default value of AxisType
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void AxisTypeDefaultValue()
        {
            Chart chart = new Chart();
            chart.AnimationEnabled = false;
            chart.Width = 500;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            TrendLine trendLine = TrendLineToTest;
            chart.TrendLines.Add(trendLine);

            EnqueueSleep(sleepTime);
            CreateAsyncTask(chart,
                () => Assert.AreEqual(AxisTypes.Primary, trendLine.AxisType));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the default value of LineColor
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void LineColorDefaultValue()
        {
            Chart chart = new Chart();
            chart.AnimationEnabled = false;
            chart.Width = 500;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            TrendLine trendLine = TrendLineToTest;
            chart.TrendLines.Add(trendLine);

            EnqueueSleep(sleepTime);
            CreateAsyncTask(chart,
                () => Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Red), trendLine.LineColor));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the default value of LineThickness
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void LineThicknessDefaultValue()
        {
            Chart chart = new Chart();
            chart.AnimationEnabled = false;
            chart.Width = 500;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            TrendLine trendLine = TrendLineToTest;
            chart.TrendLines.Add(trendLine);

            EnqueueSleep(sleepTime);
            CreateAsyncTask(chart,
                () => Assert.AreEqual(2, trendLine.LineThickness));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the default value of LineStyle
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void LineStyleDefaultValue()
        {
            Chart chart = new Chart();
            chart.AnimationEnabled = false;
            chart.Width = 500;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            TrendLine trendLine = TrendLineToTest;
            chart.TrendLines.Add(trendLine);

            EnqueueSleep(sleepTime);
            CreateAsyncTask(chart,
                () => Assert.AreEqual(LineStyles.Solid, trendLine.LineStyle));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the default value of ShadowEnabled
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void ShadowEnabledDefaultValue()
        {
            Chart chart = new Chart();
            chart.AnimationEnabled = false;
            chart.Width = 500;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            TrendLine trendLine = TrendLineToTest;
            chart.TrendLines.Add(trendLine);

            EnqueueSleep(sleepTime);
            CreateAsyncTask(chart,
                () => Assert.IsFalse(trendLine.ShadowEnabled));

            EnqueueTestComplete();
        }

        #region CheckTrendLineNewPropertyValue
        /// <summary>
        /// Check the new value of Enabled. 
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of Enabled.")]
        [Owner("[....]")]
        [Asynchronous]
        public void EnabledNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            chart.AnimationEnabled = false;

            Common.CreateAndAddDefaultDataSeries(chart);

            TrendLine trendLine = TrendLineToTest;
            chart.TrendLines.Add(trendLine);

            EnqueueSleep(sleepTime);
            CreateAsyncTask(chart,
                () => trendLine.Enabled = true,
                () => Assert.IsTrue((Boolean)trendLine.Enabled));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of Orientation. 
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of Orientation.")]
        [Owner("[....]")]
        [Asynchronous]
        public void OrientationNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            chart.AnimationEnabled = false;

            Common.CreateAndAddDefaultDataSeries(chart);

            TrendLine trendLine = TrendLineToTest;
            chart.TrendLines.Add(trendLine);

            EnqueueSleep(sleepTime);
            CreateAsyncTask(chart,
                () => trendLine.Value = 3,
                () => trendLine.Orientation = Orientation.Vertical,
                () => Assert.AreEqual(Orientation.Vertical, trendLine.Orientation));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of AxisType. 
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of AxisType.")]
        [Owner("[....]")]
        [Asynchronous]
        public void AxisTypeNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            chart.AnimationEnabled = false;

            Common.CreateAndAddDefaultDataSeries(chart);

            TrendLine trendLine = TrendLineToTest;
            chart.TrendLines.Add(trendLine);

            EnqueueSleep(sleepTime);
            CreateAsyncTask(chart,
                () => chart.Series[0].AxisYType = AxisTypes.Secondary,
                () => trendLine.AxisType = AxisTypes.Secondary,
                () => Assert.AreEqual(AxisTypes.Secondary, trendLine.AxisType));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of LineColor. 
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of LineColor.")]
        [Owner("[....]")]
        [Asynchronous]
        public void LineColorNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            chart.AnimationEnabled = false;

            Common.CreateAndAddDefaultDataSeries(chart);

            TrendLine trendLine = TrendLineToTest;
            chart.TrendLines.Add(trendLine);

            EnqueueSleep(sleepTime);
            CreateAsyncTask(chart,
                () => trendLine.LineColor = new SolidColorBrush(Colors.Cyan),
                () => Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Cyan), trendLine.LineColor));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of LineThickness. 
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of LineThickness.")]
        [Owner("[....]")]
        [Asynchronous]
        public void LineThicknessNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            chart.AnimationEnabled = false;

            Common.CreateAndAddDefaultDataSeries(chart);

            TrendLine trendLine = TrendLineToTest;
            chart.TrendLines.Add(trendLine);

            EnqueueSleep(sleepTime);
            CreateAsyncTask(chart,
                () => trendLine.LineThickness = 5,
                () => Assert.AreEqual(5, trendLine.LineThickness));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of LineStyle. 
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of LineStyle.")]
        [Owner("[....]")]
        [Asynchronous]
        public void LineStyleNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            chart.AnimationEnabled = false;

            Common.CreateAndAddDefaultDataSeries(chart);

            TrendLine trendLine = TrendLineToTest;
            chart.TrendLines.Add(trendLine);

            EnqueueSleep(sleepTime);
            CreateAsyncTask(chart,
                () => trendLine.LineStyle = LineStyles.Dashed,
                () => Assert.AreEqual(LineStyles.Dashed, trendLine.LineStyle));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of ShadowEnabled. 
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of ShadowEnabled.")]
        [Owner("[....]")]
        [Asynchronous]
        public void ShadowEnabledNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            chart.AnimationEnabled = false;

            Common.CreateAndAddDefaultDataSeries(chart);

            TrendLine trendLine = TrendLineToTest;
            chart.TrendLines.Add(trendLine);

            EnqueueSleep(sleepTime);
            CreateAsyncTask(chart,
                () => trendLine.ShadowEnabled = true,
                () => Assert.IsTrue(trendLine.ShadowEnabled));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of Value. 
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of Value.")]
        [Owner("[....]")]
        [Asynchronous]
        public void ValueNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            chart.AnimationEnabled = false;

            Common.CreateAndAddDefaultDataSeries(chart);

            TrendLine trendLine = TrendLineToTest;
            chart.TrendLines.Add(trendLine);

            EnqueueSleep(sleepTime);
            CreateAsyncTask(chart,
                () => trendLine.Orientation = Orientation.Vertical,
                () => trendLine.Value = 2,
                () => Assert.AreEqual(2, trendLine.Value));

            EnqueueTestComplete();
        }
        #endregion

        /// <summary>
        /// Check the TrendLines collection changed. 
        /// </summary> 
        [TestMethod]
        [Description("Check the TrendLines collection changed.")]
        [Owner("[....]")]
        [Asynchronous]
        public void TestTrendLinesCollectionChanged()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            chart.AnimationEnabled = false;

            Int32 trendLinesAdded = 0;

            isLoaded = false;

            Common.CreateAndAddDefaultDataSeries(chart);

            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            TestPanel.Children.Add(chart);

            EnqueueConditional(() => { return isLoaded; });
            EnqueueSleep(1000);

            chart.TrendLines.CollectionChanged += (object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
            =>
            {
                if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
                {
                    trendLinesAdded += e.NewItems.Count;
                    Assert.AreEqual(1, e.NewItems.Count);
                }
            };

            EnqueueCallback(() =>
            {
                TrendLine trendLine = new TrendLine();
                trendLine.Orientation = Orientation.Horizontal;
                trendLine.Value = 30;
                chart.TrendLines.Add(trendLine);
            });

            EnqueueConditional(() => { return isLoaded; });
            EnqueueSleep(1000);

            EnqueueCallback(() =>
            {
                TrendLine trendLine = new TrendLine();
                trendLine.Orientation = Orientation.Vertical;
                trendLine.Value = 3;
                chart.TrendLines.Add(trendLine);
            });

            EnqueueConditional(() => { return isLoaded; });
            EnqueueSleep(1000);

            EnqueueCallback(() =>
            {
                TrendLine trendLine = new TrendLine();
                trendLine.Orientation = Orientation.Vertical;
                trendLine.Value = 4;
                chart.TrendLines.Add(trendLine);
            });

            EnqueueConditional(() => { return isLoaded; });
            EnqueueSleep(1000);

            EnqueueCallback(() =>
            {
                TrendLine trendLine = new TrendLine();
                trendLine.Orientation = Orientation.Horizontal;
                trendLine.Value = 60;
                chart.TrendLines.Add(trendLine);
            });

            EnqueueConditional(() => { return isLoaded; });
            EnqueueSleep(1000);

            EnqueueCallback(() =>
            {
                TrendLine trendLine = new TrendLine();
                trendLine.Orientation = Orientation.Horizontal;
                trendLine.Value = 80;
                chart.TrendLines.Add(trendLine);
            });

            EnqueueConditional(() => { return isLoaded; });
            EnqueueSleep(1000);

            EnqueueCallback(() =>
            {
                TrendLine trendLine = new TrendLine();
                trendLine.Orientation = Orientation.Vertical;
                trendLine.Value = 1;
                chart.TrendLines.Add(trendLine);
            });

            EnqueueSleep(1000);
            EnqueueTestComplete();
        }

        void chart_Loaded(object sender, RoutedEventArgs e)
        {
            isLoaded = true;
        }

        private TrendLine TrendLineToTest
        {
            get { return new TrendLine() { Value = 60, Orientation = Orientation.Horizontal }; }
        }

        #region Private Data

        private const int sleepTime = 1000;
        private bool isLoaded = false;

        #endregion
    }
}
