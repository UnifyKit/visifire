using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Visifire.Charts;
using Visifire.Commons;

namespace WPFVisifireChartsTest
{
    /// <summary>
    /// Summary description for PlotDetailsTest
    /// </summary>
    //[TestClass]
    public class PlotDetailsTest : WPFControlTest
    {
        //#region TestPlotDetailsData

        ///// <summary>
        ///// Test PlotDetails containing PlotGroups and other parameters.
        ///// </summary>
        //[TestMethod]
        //[Description("Test PlotDetails containing PlotGroups and other parameters.")]
        //public void TestPlotDetails()
        //{
        //    chart = new Chart();

        //    chart.Width = 400;
        //    chart.Height = 300;

        //    chart.Loaded += new RoutedEventHandler(chart_Loaded);

        //    Axis axisX = new Axis();
        //    axisX.AxisOrientation = Orientation.Horizontal;
        //    axisX.Maximum = 10;
        //    axisX.Minimum = 0;
        //    axisX.Width = chart.Width;
        //    axisX.Interval = 1;

        //    Axis axisY = new Axis();
        //    axisY.AxisOrientation = Orientation.Vertical;
        //    axisY.Maximum = 100;
        //    axisY.Minimum = 0;
        //    axisY.Width = chart.Width;
        //    axisY.Interval = 1;

        //    chart.AxisX = new ObservableCollection<Axis>();
        //    chart.AxisX.Add(axisX);
        //    chart.AxisY = new ObservableCollection<Axis>();
        //    chart.AxisY.Add(axisY);

        //    Int32 plotDetailsIndex = 0;
        //    foreach (DataSeries dataSeries in DataSeriesToTest)
        //    {
        //        dataSeries.RenderAs = RenderAs.Column;

        //        Random rand = new Random();

        //        dataSeries.DataPoints = new System.Collections.ObjectModel.ObservableCollection<DataPoint>();
        //        for (Int32 i = 0; i < 5; i++)
        //        {
        //            DataPoint datapoint = new DataPoint();
        //            datapoint.XValue = i + 1;
        //            datapoint.YValue = rand.Next(0, 100);
        //            dataSeries.DataPoints.Add(datapoint);
        //        }

        //        chart.Series = new System.Collections.ObjectModel.ObservableCollection<DataSeries>();
        //        chart.Series.Add(dataSeries);

        //        PlotDetails plotDetails = new PlotDetails(chart);

        //        plotDetails.PlotGroups = new List<PlotGroup>();

        //        PlotGroup pg = new PlotGroup(RenderAs.Column, axisX, axisY);

        //        plotDetails.PlotGroups.Add(pg);
        //        plotDetails.PlotGroups[plotDetailsIndex].DataSeriesList = new List<DataSeries>();

        //        Int32 dataSeriesIndex = 0;

        //        plotDetails.PlotGroups[plotDetailsIndex].DataSeriesList.Add(dataSeries);

        //        Assert.AreEqual(5, dataSeries.DataPoints.Count, String.Format("Should be {0} items in DataPoints", dataSeries.DataPoints.Count));
        //        dataSeriesIndex++;

        //        Assert.AreEqual(dataSeriesIndex, plotDetails.PlotGroups[plotDetailsIndex].DataSeriesList.Count, String.Format("Should be {0} items in DataSeries", plotDetails.PlotGroups[plotDetailsIndex].DataSeriesList.Count));

        //        plotDetails.PlotGroups[plotDetailsIndex].Update();

        //        plotDetails.GetAxisXMaximumDataValue(axisX);
        //        plotDetails.GetAxisXMinimumDataValue(axisX);
        //        plotDetails.GetAxisYMaximumDataValue(axisY);
        //        plotDetails.GetAxisYMinimumDataValue(axisY);

        //        var getAxisXMaximumDataValue = (from plotData in plotDetails.PlotGroups
        //                                        where (!Double.IsNaN(plotData.MaximumX) && plotData.AxisX == axisX)
        //                                        select plotData.MaximumX).Max();

        //        var getAxisYMaximumDataValue = (from plotData in plotDetails.PlotGroups
        //                                        where (!Double.IsNaN(plotData.MaximumY) && plotData.AxisY == axisY)
        //                                        select plotData.MaximumY).Max();

        //        var getAxisXMinimumDataValue = (from plotData in plotDetails.PlotGroups
        //                                        where (!Double.IsNaN(plotData.MinimumX) && plotData.AxisX == axisX)
        //                                        select plotData.MinimumX).Min();

        //        var getAxisYMinimumDataValue = (from plotData in plotDetails.PlotGroups
        //                                        where (!Double.IsNaN(plotData.MinimumY) && plotData.AxisY == axisY)
        //                                        select plotData.MinimumY).Min();

        //        var getMaximumZValue = (from plotData in plotDetails.PlotGroups
        //                                where !Double.IsNaN(plotData.MinimumZ)
        //                                select plotData.MaximumZ).Max();

        //        var getMinimumZValue = (from plotData in plotDetails.PlotGroups
        //                                where !Double.IsNaN(plotData.MinimumZ)
        //                                select plotData.MinimumZ).Min();

        //        var getMaxOfMinDifferencesForXValue = (from plotData in plotDetails.PlotGroups
        //                                               where !Double.IsNaN(plotData.MinDifferenceX)
        //                                               select plotData.MinDifferenceX).Max();

        //        var getMinOfMinDifferencesForXValue = (from plotData in plotDetails.PlotGroups
        //                                               where !Double.IsNaN(plotData.MinDifferenceX)
        //                                               select plotData.MinDifferenceX).Min();

        //        Assert.AreEqual(getAxisXMaximumDataValue, plotDetails.GetAxisXMaximumDataValue(axisX));
        //        Assert.AreEqual(getAxisYMaximumDataValue, plotDetails.GetAxisYMaximumDataValue(axisY));
        //        Assert.AreEqual(getAxisXMinimumDataValue, plotDetails.GetAxisXMinimumDataValue(axisX));
        //        Assert.AreEqual(getAxisYMinimumDataValue, plotDetails.GetAxisYMinimumDataValue(axisY));
        //        Assert.AreEqual(getMaximumZValue, plotDetails.GetMaximumZValue());
        //        Assert.AreEqual(getMinimumZValue, plotDetails.GetMinimumZValue());
        //        Assert.AreEqual(getMaxOfMinDifferencesForXValue, plotDetails.GetMaxOfMinDifferencesForXValue());
        //        Assert.AreEqual(getMinOfMinDifferencesForXValue, plotDetails.GetMinOfMinDifferencesForXValue());

        //        Assert.AreEqual(axisX, plotDetails.PlotGroups[plotDetailsIndex].AxisX);
        //        Assert.AreEqual(axisY, plotDetails.PlotGroups[plotDetailsIndex].AxisY);

        //        plotDetailsIndex++;

        //    }
        //}

        //#endregion

        //#region TestMinimumDifferenceBetweenXValues

        ///// <summary>
        ///// Test the minimum difference calculation for XValues for its correctness.
        ///// </summary>
        //[TestMethod]
        //[Description("Test the minimum difference calculation for XValues for its correctness.")]
        //public void TestMinimumDifference()
        //{
        //    chart = new Chart();
        //    chart.Width = 400;
        //    chart.Height = 300;

        //    foreach (DataSeries dataSeries in DataSeriesToTest)
        //    {
        //        dataSeries.RenderAs = RenderAs.Column;

        //        Random rand = new Random();

        //        dataSeries.DataPoints = new System.Collections.ObjectModel.ObservableCollection<DataPoint>();
        //        for (Int32 i = 0; i < 5; i++)
        //        {
        //            DataPoint datapoint = new DataPoint();
        //            datapoint.XValue = i + 1;
        //            datapoint.YValue = rand.Next(0, 100);
        //            dataSeries.DataPoints.Add(datapoint);
        //        }

        //        var xValues = (from dataPoint in dataSeries.DataPoints where !Double.IsNaN(dataPoint.XValue) select dataPoint.XValue).Distinct();

        //        chart.Series = new System.Collections.ObjectModel.ObservableCollection<DataSeries>();
        //        chart.Series.Add(dataSeries);

        //        Axis axisX = new Axis();
        //        axisX.AxisOrientation = Orientation.Horizontal;
        //        axisX.Maximum = 10;
        //        axisX.Minimum = 0;
        //        axisX.Width = chart.Width;
        //        axisX.Interval = 1;

        //        Axis axisY = new Axis();
        //        axisY.AxisOrientation = Orientation.Vertical;
        //        axisY.Maximum = 100;
        //        axisY.Minimum = 0;
        //        axisY.Width = chart.Width;
        //        axisY.Interval = 1;

        //        chart.AxisX = new ObservableCollection<Axis>();
        //        chart.AxisX.Add(axisX);
        //        chart.AxisY = new ObservableCollection<Axis>();
        //        chart.AxisY.Add(axisY);

        //        PlotDetails plotDetails = new PlotDetails(chart);

        //        plotDetails.PlotGroups = new List<PlotGroup>();

        //        PlotGroup pg = new PlotGroup(RenderAs.Column, axisX, axisY);

        //        plotDetails.PlotGroups.Add(pg);

        //        plotDetails.PlotGroups[0].MinDifferenceX = plotDetails.PlotGroups[0].GetMinDifference(xValues.ToArray());

        //        Assert.AreEqual(this.CalculateMinimumDifference(xValues.ToArray()), plotDetails.PlotGroups[0].MinDifferenceX, "Minimum difference calculation is wrong for XValues");

        //    }
        //}

        //#endregion

        //#region CalculateMinimumDifference

        //public Double CalculateMinimumDifference(Double[] values)
        //{
        //    Double minDiff = Double.MaxValue;

        //    Array.Sort(values.Distinct().ToArray());

        //    for (Int32 i = 0; i < values.Length - 1; i++)
        //    {
        //        minDiff = Math.Min(minDiff, Math.Abs(values[i] - values[i + 1]));
        //    }

        //    return minDiff;
        //}

        //#endregion

        //#region TestXWiseStackedData

        ///// <summary>
        ///// Test the XWiseStackedData
        ///// </summary>
        //[TestMethod]
        //[Description("Test the XWiseStackedData collection changed event")]
        //public void TestXWiseStackedDataCollectionChanged()
        //{
        //    chart = new Chart();
        //    chart.Width = 400;
        //    chart.Height = 300;

        //    isLoaded = false;

        //    chart.Loaded += new RoutedEventHandler(chart_Loaded);

        //    DataSeries dataSeries = new DataSeries();
        //    dataSeries.RenderAs = RenderAs.Column;

        //    Random rand = new Random();

        //    dataSeries.DataPoints = new System.Collections.ObjectModel.ObservableCollection<DataPoint>();

        //    Int32 i;

        //    for (i = 0; i < 5; i++)
        //    {
        //        DataPoint datapoint = new DataPoint();
        //        datapoint.XValue = i + 1;
        //        datapoint.YValue = rand.Next(-100, 100);
        //        dataSeries.DataPoints.Add(datapoint);
        //    }

        //    chart.Series = new System.Collections.ObjectModel.ObservableCollection<DataSeries>();
        //    chart.Series.Add(dataSeries);

        //    Axis axisX = new Axis();
        //    axisX.AxisOrientation = Orientation.Horizontal;

        //    Axis axisY = new Axis();
        //    axisY.AxisOrientation = Orientation.Vertical;

        //    chart.AxisX = new ObservableCollection<Axis>();
        //    chart.AxisX.Add(axisX);
        //    chart.AxisY = new ObservableCollection<Axis>();
        //    chart.AxisY.Add(axisY);

        //    PlotDetails plotDetails = new PlotDetails(chart);

        //    plotDetails.PlotGroups = new List<PlotGroup>();

        //    PlotGroup pg = new PlotGroup(RenderAs.Column, axisX, axisY);

        //    plotDetails.PlotGroups.Add(pg);

        //    plotDetails.PlotGroups[0].XWiseStackedDataList = new Dictionary<Double, XWiseStackedData>();

        //    XWiseStackedData xWiseData = new XWiseStackedData();

        //    Int32 positiveDataPointIndex = 0;
        //    Int32 negativeDataPointIndex = 0;
        //    foreach (DataPoint dataPoint in dataSeries.DataPoints)
        //    {
        //        plotDetails.PlotGroups[0].XWiseStackedDataList.Add(dataPoint.XValue, xWiseData);

        //        plotDetails.PlotGroups[0].AddXWiseStackedDataEntry(ref xWiseData, dataPoint);

        //        if (dataPoint.YValue >= 0)
        //        {
        //            Assert.AreEqual(dataPoint, xWiseData.Positive.ElementAt(positiveDataPointIndex++));

        //        }
        //        else
        //        {
        //            Assert.AreEqual(dataPoint, xWiseData.Negative.ElementAt(negativeDataPointIndex++));

        //        }
        //    }

        //    Int32 positiveValuesAdded = 0;
        //    xWiseData.Positive.CollectionChanged += (object sender, NotifyCollectionChangedEventArgs e)
        //        =>
        //    {
        //        if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
        //        {
        //            positiveValuesAdded += e.NewItems.Count;
        //            Assert.AreEqual(1, e.NewItems.Count);
        //        }
        //    };

        //    Int32 negativeValuesAdded = 0;
        //    xWiseData.Negative.CollectionChanged += (object sender, NotifyCollectionChangedEventArgs e)
        //       =>
        //    {
        //        if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
        //        {
        //            negativeValuesAdded += e.NewItems.Count;
        //            Assert.AreEqual(1, e.NewItems.Count);
        //        }
        //    };

        //    Int32 positiveDataPointValue = 0;
        //    Int32 negativeDataPointValue = 0;

        //    for (i = 5; i < 10; i++)
        //    {
        //        DataPoint datapoint = new DataPoint();
        //        datapoint.XValue = i + 1;
        //        datapoint.YValue = rand.Next(-100, 100);
        //        dataSeries.DataPoints.Add(datapoint);
        //    }

        //    foreach (DataPoint dataPoint in dataSeries.DataPoints.Skip(5))
        //    {
        //        plotDetails.PlotGroups[0].XWiseStackedDataList.Add(dataPoint.XValue, xWiseData);

        //        plotDetails.PlotGroups[0].AddXWiseStackedDataEntry(ref xWiseData, dataPoint);

        //        if (dataPoint.YValue >= 0)
        //        {
        //            Assert.AreEqual(dataPoint, xWiseData.Positive.ElementAt(positiveDataPointIndex++));
        //            positiveDataPointValue++;
        //        }
        //        else
        //        {
        //            Assert.AreEqual(dataPoint, xWiseData.Negative.ElementAt(negativeDataPointIndex++));
        //            negativeDataPointValue++;
        //        }

        //        Assert.AreEqual(positiveDataPointValue, positiveValuesAdded);
        //        Assert.AreEqual(negativeDataPointValue, negativeDataPointValue);
        //    }
        //}

        //#endregion

        public IEnumerable<DataSeries> DataSeriesToTest
        {
            get
            {
                yield return new DataSeries();
            }
        }

        #region Private Data

        const int sleepTime = 2000;

        #endregion
    }
}
