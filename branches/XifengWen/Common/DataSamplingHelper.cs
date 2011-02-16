using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using Visifire.Charts;

namespace Visifire.Commons
{
    /// <summary>
    /// DataSamplingHelper class keeps track of XValue and YValue or YValues(as in CandleStick and Stock charts) which is held in an array.
    /// </summary>
    public static class DataSamplingHelper
    {
        public class Point
        {
            public Point(Double xValue, Double[] yValues)
            {
                XValue = xValue;
                YValues = yValues;
            }

            public Double[] YValues;
            public Double XValue;
        }

        /// <summary>
        /// This function will take Actual list of points as input and returns the sampled points based on the sampling function i.e average
        /// </summary>
        /// <param name="actualList">Actual list of points</param>
        /// <param name="minXPosition">Minimum position of X-Cordinate i.e Axis Minimum</param>
        /// <param name="plotXValueDistance">XValue range</param>
        /// <param name="DataPointsLimit">SamplingThreshold</param>
        /// <param name="samplingFunction">Sampling function</param>
        /// <returns></returns>
        public static List<DataSamplingHelper.Point> Filter(List<DataSamplingHelper.Point> actualList, Double minXPosition, Double plotXValueDistance, Int32 DataPointsLimit, SamplingFunction samplingFunction)
        {   
            if (actualList.Count <= DataPointsLimit || DataPointsLimit==0)
                return actualList;

            List<Point> retPointList = actualList.ToList();

            if (samplingFunction == SamplingFunction.Average)
                retPointList = GroupAverageFilter(retPointList, minXPosition, plotXValueDistance, DataPointsLimit);
            
            return retPointList;
        }

        /// <summary>
        /// Creates Point groups based on the groupSize value.MinxPosition,maxXPosition and groupSize is calculated for every iteration
        /// All the points which fall within the groupSize are grouped.Threshold area will be the distance from  minXPosition to maxXPosition
        /// and the process is continued till minXPosition not greater than Plotarea width.
        /// </summary>
        /// <param name="actualList">List containing actual number of points</param>
        /// <param name="plotXValueDistance">Width of the plot Area i.e XValue range </param>
        /// <param name="groupSize">An area within which all the points falling are grouped</param>
        /// <returns></returns>
        private static List<PointGroup> CreatePointGroups(List<Point> actualList, Double minXPosition, Double plotXValueDistance, Double threshold)
        {
            List<PointGroup> PointGroups = new List<PointGroup>();

            Double maxXPosition = 0;
            Double position = minXPosition;

            for (; position <= (minXPosition + plotXValueDistance); position = maxXPosition)
            {
                maxXPosition = position + threshold;

                PointGroup pointGroup = new PointGroup()
                {
                    dataPoints = (from point in actualList where point.XValue >= position && point.XValue <= maxXPosition select point).ToList()
                };

                PointGroups.Add(pointGroup);
            }

            return PointGroups;
        }




        /// <summary>
        /// This function will take actual list of points and creates a new list containing sampled points  based on the threshold(groupSize) value calculated.
        /// </summary>
        /// <param name="actualList">Actual list of points</param>
        /// <param name="minXPosition">Minimum X Positon axis minimum</param>
        /// <param name="plotXValueDistance">XValue range</param>
        /// <param name="DataPointsLimit">SamplingThreshold</param>
        /// <returns></returns>

        private static List<Point> GroupAverageFilter(List<Point> actualList, Double minXPosition, Double plotXValueDistance, Int32 DataPointsLimit)
        {   
            Double groupSize = Math.Abs(plotXValueDistance / DataPointsLimit);

            List<PointGroup> groups;
            List<Point> newLisList;
                        
            groups = CreatePointGroups(actualList, minXPosition, plotXValueDistance, groupSize);

            newLisList = new List<Point>();
                
            foreach (PointGroup pointGroup in groups)
            {   
                if(pointGroup.dataPoints.Count > 0)
                {
                    Point sampledPoint = (pointGroup.GetAvgPoint());

                    // If PointGroup does not contain any Points then sampledPoint will be null
                    if(sampledPoint != null)
                        newLisList.Add(sampledPoint);
                }
            }
            
            actualList = newLisList;
                
            return actualList;
        }
        
        /// <summary>
        /// The class PointGroup has methods which are used to find out the average of all points in a group.A point group must contain atleast one DataPoint.
        /// In each group average Y Co-ordinate is calculated.YValue may contain single value or elements like High,Low,Open,Close values as in CandleStick and Stock charts.
        /// The new point's Y Co-ordinate will be the average of Y Co-ordinates or average of YValue components in case of CandleStick and Stock charts.
        /// and new point's X Co-ordinate will be the mid point of the group.
        /// </summary>
        private class PointGroup
        {
            public List<Point> dataPoints = new List<Point>();

            public Point GetAvgPoint()
            {
                if (dataPoints.Count > 0)
                {   
                    List<Double> yValues = new List<double>();
                    Int32 yValuesCount = dataPoints[0].YValues.Length;

                    for (Int32 index = 0; index < yValuesCount; index++)
                        yValues.Add(GetAvgYValue(index));

                    return new Point(GetAvgXValue(), yValues.ToArray());
                }
                else
                    return null;
            }

            private Double GetAvgYValue(Int32 index)
            {
                return (from dataPoint in dataPoints where !Double.IsNaN(dataPoint.YValues[index]) select dataPoint.YValues[index]).Sum() / dataPoints.Count;
            }

            private Double GetAvgXValue()
            {   
                Double min = (from dataPoint in dataPoints select dataPoint.XValue).Max();
                Double max = (from dataPoint in dataPoints select dataPoint.XValue).Min();

                return (max + min) / 2;
            }
        }
    }
}