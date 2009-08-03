/*   
    Copyright (C) 2008 Webyog Softworks Private Limited

    This file is a part of Visifire Charts.
 
    Visifire is a free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.
      
    You should have received a copy of the GNU General Public License
    along with Visifire Charts.  If not, see <http://www.gnu.org/licenses/>.
  
    If GPL is not suitable for your products or company, Webyog provides Visifire 
    under a flexible commercial license designed to meet your specific usage and 
    distribution requirements. If you have already obtained a commercial license 
    from Webyog, you can use this file under those license terms.
    
*/

#if WPF

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Animation;
#else
using System;
using System.Windows;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Collections.Generic;
#endif

using System.Windows.Shapes;

using Visifire.Commons;

namespace Visifire.Charts
{
    /// <summary>
    /// Visifire.Charts.RectangularChartShapeParams class
    /// (Used for column and bar charts)
    /// </summary>
    internal class RectangularChartShapeParams
    {
        public Size Size { get; set; }
        public CornerRadius XRadius { get; set; }
        public CornerRadius YRadius { get; set; }
        public Brush BackgroundBrush { get; set; }
        public Brush BorderBrush { get; set; }
        public Boolean Bevel { get; set; }
        public Boolean Lighting { get; set; }
        public Boolean Shadow { get; set; }
        public Double ShadowOffset { get; set; }
        public DoubleCollection BorderStyle { get; set; }
        public Double BorderThickness { get; set; }
        public Boolean IsPositive { get; set; }
        public Double Depth { get; set; }
        public Boolean IsTopOfStack { get; set; }
        public Boolean IsStacked { get; set; }
        public Boolean IsMarkerEnabled { get; set; }
        public Brush MarkerColor { get; set; }
        public Brush MarkerBorderColor { get; set; }
        public double MarkerSize { get; set; }
        public Thickness MarkerBorderThickness { get; set; }
        public double MarkerScale { get; set; }
        public MarkerTypes MarkerType { get; set; }
        public Boolean IsLabelEnabled { get; set; }
        public Brush LabelBackground { get; set; }
        public Brush LabelFontColor { get; set; }
        public FontFamily LabelFontFamily { get; set; }
        public Double LabelFontSize { get; set; }
        public FontStyle LabelFontStyle { get; set; }
        public FontWeight LabelFontWeight { get; set; }
        public Nullable<LabelStyles> LabelStyle { get; set; }
        public String LabelText { get; set; }
        public FrameworkElement TagReference { get; set; }
    }

    /// <summary>
    ///  Visifire.Charts.SortedDataPoints class. 
    ///  SortedDataPoints used to store InternalDataPoints with positive and negative values 
    /// </summary>
    public class SortDataPoints
    {
        /// <summary>
        /// Initializes a new instance of the Visifire.Charts.SortedDataPoints class
        /// </summary>
        public SortDataPoints()
        {
        }

        /// <summary>
        /// Sort InternalDataPoints
        /// </summary>
        /// <param name="positive">Positive InternalDataPoints</param>
        /// <param name="">Negative InternalDataPoints</param>
        public SortDataPoints(List<DataPoint> positive, List<DataPoint> negative)
        {
            Positive = positive;
            Negative = negative;
        }

        /// <summary>
        /// List of InternalDataPoints with Positive values
        /// </summary>
        public List<DataPoint> Positive { get; private set; }

        /// <summary>
        /// List of InternalDataPoints with Negative values
        /// </summary>
        public List<DataPoint> Negative { get; private set; }
    }

    /// <summary>
    /// Visifire.Charts.ColumnChart class
    /// </summary>
    public class ColumnChart
    {
        #region Public Methods

        #endregion

        #region Public Properties

        #endregion

        #region Public Events And Delegates

        #endregion

        #region Protected Methods

        #endregion

        #region Internal Properties

        #endregion

        #region Private Properties

        /// <summary>
        /// Current working DataSeries
        /// </summary>
        private static DataSeries CurrentDataSeries
        {
            get;
            set;
        }

        #endregion

        #region Private Delegates

        #endregion

        #region Private Methods

        /// <summary>
        /// Set position of the marker
        /// </summary>
        /// <param name="columnParams">Column parameters</param>
        /// <param name="chart">Chart</param>
        /// <param name="dataPoint">DataPoint</param>
        /// <param name="labelText">label text</param>
        /// <param name="markerSize">Size of the marker</param>
        /// <param name="canvasLeft">Left position of marker canvas</param>
        /// <param name="canvasTop">Top position of marker canvas</param>
        /// <param name="markerPosition">Position of the Marker</param>
        private static void SetMarkerPosition(RectangularChartShapeParams columnParams, Chart chart, DataPoint dataPoint, String labelText, Size markerSize, Double canvasLeft, Double canvasTop, Point markerPosition)
        {
            if (columnParams.IsLabelEnabled && !String.IsNullOrEmpty(labelText))
            {
                dataPoint.Marker.CreateVisual();

                if (columnParams.Size.Width < dataPoint.Marker.TextBlockSize.Width)
                    dataPoint.Marker.TextOrientation = Orientation.Vertical;
                else
                    dataPoint.Marker.TextOrientation = Orientation.Horizontal;

                if (columnParams.IsPositive)
                {
                    if (dataPoint.Marker.TextOrientation == Orientation.Vertical)
                    {
                        if (canvasTop - dataPoint.Marker.MarkerActualSize.Width - dataPoint.Marker.MarkerSize.Height < 0)
                            columnParams.LabelStyle = LabelStyles.Inside;
                    }
                    else
                    {
                        if (canvasTop - dataPoint.Marker.MarkerActualSize.Height - dataPoint.Marker.MarkerSize.Height < 0)
                            columnParams.LabelStyle = LabelStyles.Inside;
                    }
                }
                else
                {
                    if (dataPoint.Marker.TextOrientation == Orientation.Vertical)
                    {
                        if (canvasTop + markerPosition.Y + dataPoint.Marker.MarkerActualSize.Width + dataPoint.Marker.MarkerSize.Height > chart.PlotArea.BorderElement.Height + chart.ChartArea.PLANK_DEPTH - chart.ChartArea.PLANK_THICKNESS)
                            columnParams.LabelStyle = LabelStyles.Inside;
                    }
                    else
                    {
                        if (canvasTop + markerPosition.Y + dataPoint.Marker.MarkerActualSize.Height + dataPoint.Marker.MarkerSize.Height > chart.PlotArea.BorderElement.Height + chart.ChartArea.PLANK_DEPTH - chart.ChartArea.PLANK_THICKNESS)
                            columnParams.LabelStyle = LabelStyles.Inside;
                    }
                }

                dataPoint.Marker.TextAlignmentX = AlignmentX.Center;

                if (!columnParams.IsMarkerEnabled)
                {
                    if (chart.View3D)
                    {
                        if (columnParams.LabelStyle == LabelStyles.OutSide)
                        {
                            if (columnParams.IsPositive)
                                dataPoint.Marker.MarkerSize = new Size(markerSize.Width / 2 + chart.ChartArea.PLANK_DEPTH + chart.ChartArea.PLANK_THICKNESS, markerSize.Height / 2 + chart.ChartArea.PLANK_DEPTH + chart.ChartArea.PLANK_THICKNESS);
                            else
                                dataPoint.Marker.MarkerSize = new Size(markerSize.Width / 2, markerSize.Height / 2);
                        }
                        else
                            dataPoint.Marker.MarkerSize = new Size(markerSize.Width / 2, markerSize.Height / 2);
                    }
                }
                else
                {
                    if (chart.View3D)
                    {
                        columnParams.LabelStyle = LabelStyles.Inside;
                    }
                }

                if (columnParams.IsPositive)
                    dataPoint.Marker.TextAlignmentY = columnParams.LabelStyle == LabelStyles.Inside ? AlignmentY.Bottom : AlignmentY.Top;
                else
                    dataPoint.Marker.TextAlignmentY = columnParams.LabelStyle == LabelStyles.Inside ? AlignmentY.Top : AlignmentY.Bottom;
            }
        }

        /// <summary>
        /// Returns marker for DataPoint
        /// </summary>
        /// <param name="chart">Chart</param>
        /// <param name="columnParams">Column parameters</param>
        /// <param name="dataPoint">DataPoint</param>
        /// <param name="left">Left position of MarkerCanvas</param>
        /// <param name="top">Top position</param>
        /// <returns>Marker canvas</returns>
        private static Canvas GetMarker(Chart chart, RectangularChartShapeParams columnParams, DataPoint dataPoint, Double left, Double top)
        {
            Canvas markerCanvas = new Canvas() { Width = columnParams.Size.Width, Height = columnParams.Size.Height };

            markerCanvas.SetValue(Canvas.LeftProperty, left);
            markerCanvas.SetValue(Canvas.TopProperty, top);

            if (columnParams.IsMarkerEnabled || columnParams.IsLabelEnabled)
            {
                Size markerSize = new Size(columnParams.MarkerSize, columnParams.MarkerSize);
                String labelText = columnParams.IsLabelEnabled ? columnParams.LabelText : "";

                if (!columnParams.IsMarkerEnabled)
                {
                    columnParams.MarkerColor = new SolidColorBrush(Colors.Transparent);
                    columnParams.MarkerBorderColor = new SolidColorBrush(Colors.Transparent);
                }

                dataPoint.Marker = CreateNewMarker(columnParams, dataPoint, markerSize, labelText);

                Point markerPosition = new Point();

                if (columnParams.IsPositive)
                    if (chart.View3D)
                        markerPosition = new Point(columnParams.Size.Width / 2, 0);
                    else
                        markerPosition = new Point(columnParams.Size.Width / 2, 0);
                else
                    if (chart.View3D)
                        markerPosition = new Point(columnParams.Size.Width / 2, columnParams.Size.Height);
                    else
                        markerPosition = new Point(columnParams.Size.Width / 2, columnParams.Size.Height);

                SetMarkerPosition(columnParams, chart, dataPoint, labelText, markerSize, left, top, markerPosition);

                columnParams.LabelFontColor = Chart.CalculateDataPointLabelFontColor(chart, dataPoint, dataPoint.LabelFontColor, (dataPoint.YValue == 0)? LabelStyles.OutSide:(LabelStyles)columnParams.LabelStyle);
                dataPoint.Marker.FontColor = columnParams.LabelFontColor;

                dataPoint.Marker.Tag = new ElementData() { Element = dataPoint };
                dataPoint.Marker.CreateVisual();

                dataPoint.Marker.AddToParent(markerCanvas, markerPosition.X, markerPosition.Y, new Point(0.5, 0.5));
            }

            return markerCanvas;
        }

        /// <summary>
        /// Calculate width of each column
        /// </summary>
        /// <param name="left">Left position</param>
        /// <param name="widthPerColumn">Width of a column</param>
        /// <param name="width">Width of chart canvas</param>
        /// <returns>Final width of DataPoint</returns>
        private static Double CalculateWidthOfEachColumn(ref Double left, Double widthPerColumn, Double width)
        {   
            Double finalWidth = widthPerColumn;
            //Double minPosValue = 0;
            //Double maxPosValue = width;
            //if (left < minPosValue)
            //{
            //    finalWidth = left + widthPerColumn - minPosValue;
            //    left = minPosValue;
            //}
            //else if (left + widthPerColumn > maxPosValue)
            //{
            //    finalWidth = maxPosValue - left;
            //}

            return (finalWidth < 2) ? 2 : finalWidth;
            //return finalWidth;
        }

        /// <summary>
        /// Get columns Z-Index
        /// </summary>
        /// <param name="left">Left position</param>
        /// <param name="top">Top position</param>
        /// <param name="isPositive">Whether DataPoint value is positive or negative</param>
        /// <returns>Zindex as Int32</returns>
        private static Int32 GetColumnZIndex(Double left, Double top, Boolean isPositive)
        {
            Int32 Zi = 0;
            Int32 ioffset = (Int32)left;

            if (ioffset == 0)
                ioffset++;

            Zi = (isPositive) ? Zi + (Int32)(ioffset) : Zi + Int32.MinValue + (Int32)(ioffset);

            return Zi;
        }

        /// <summary>
        /// Get ZIndex for StackedColumn visual
        /// </summary>
        /// <param name="left">Left position</param>
        /// <param name="top">Top position</param>
        /// <param name="isPositive">Whether column value is positive or negative</param>
        /// <param name="index">Index</param>
        /// <returns>Zindex as Int32</returns>
        private static Int32 GetStackedColumnZIndex(Double left, Double top, Boolean isPositive, Int32 index)
        {
            Int32 ioffset = (Int32)(left);
            Int32 topOffset = (Int32)(index);

            Int32 zindex = 0;
            if (isPositive)
            {
                zindex = (Int32)(ioffset + topOffset);
            }
            else
            {
                if (ioffset == 0)
                    ioffset = 1;
                zindex = Int32.MinValue + (Int32)(ioffset + topOffset);
            }

            return zindex;
        }

        /// <summary>
        /// Apply column chart animation
        /// </summary>
        /// <param name="column">Column visual reference</param>
        /// <param name="storyboard">Storyboard</param>
        /// <param name="columnParams">Column parameters</param>
        /// <returns>Storyboard</returns>
        private static Storyboard ApplyColumnChartAnimation(Panel column, Storyboard storyboard, RectangularChartShapeParams columnParams)
        {
            ScaleTransform scaleTransform = new ScaleTransform() { ScaleY = 0 };
            column.RenderTransform = scaleTransform;

            if (columnParams.IsPositive)
            {
                column.RenderTransformOrigin = new Point(0.5, 1);
            }
            else
            {
                column.RenderTransformOrigin = new Point(0.5, 0);
            }

            DoubleCollection values = Graphics.GenerateDoubleCollection(0, 1);
            DoubleCollection frameTimes = Graphics.GenerateDoubleCollection(0, 1);
            List<KeySpline> splines = AnimationHelper.GenerateKeySplineList
                (
                new Point(0, 0), new Point(1, 1),
                new Point(0, 1), new Point(0.5, 1)
                );

            DoubleAnimationUsingKeyFrames growAnimation = AnimationHelper.CreateDoubleAnimation(CurrentDataSeries, scaleTransform, "(ScaleTransform.ScaleY)", 1, frameTimes, values, splines);

            storyboard.Children.Add(growAnimation);

            return storyboard;
        }

        /// <summary>
        /// Apply animation for StackedColumn chart
        /// </summary>
        /// <param name="column">Column visual reference</param>
        /// <param name="storyboard">Storyboard</param>
        /// <param name="columnParams">Column params</param>
        /// <param name="begin">Animation begin time</param>
        /// <param name="duration">Animation duration</param>
        /// <returns>Storyboard</returns>
        private static Storyboard ApplyStackedColumnChartAnimation(Panel column, Storyboard storyboard, RectangularChartShapeParams columnParams, Double begin, Double duration)
        {
            ScaleTransform scaleTransform = new ScaleTransform() { ScaleY = 0 };
            column.RenderTransform = scaleTransform;

            column.RenderTransformOrigin = new Point(0.5, 0.5);

            DoubleCollection values = Graphics.GenerateDoubleCollection(0, 1.5, 0.75, 1.125, 0.9325, 1);
            DoubleCollection frameTimes = Graphics.GenerateDoubleCollection(0, 0.25 * duration, 0.5 * duration, 0.75 * duration, 1.0 * duration, 1.25 * duration);
            List<KeySpline> splines = AnimationHelper.GenerateKeySplineList
                (
                new Point(0, 0), new Point(1, 0.5),
                new Point(0, 0), new Point(0.5, 1),
                new Point(0, 0), new Point(1, 0.5),
                new Point(0, 0), new Point(0.5, 1),
                new Point(0, 0), new Point(1, 0.5),
                new Point(0, 0), new Point(0.5, 1)
                );

            DoubleAnimationUsingKeyFrames growAnimation = AnimationHelper.CreateDoubleAnimation(CurrentDataSeries, scaleTransform, "(ScaleTransform.ScaleY)", begin + 0.5, frameTimes, values, splines);

            storyboard.Children.Add(growAnimation);

            return storyboard;
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Set column parameters
        /// </summary>
        /// <param name="columnParams">Column parameters</param>
        /// <param name="chart">Chart reference</param>
        /// <param name="dataPoint">DataPoint</param>
        /// <param name="IsPositive">Whether the DataPoint YValue is positive or negative</param>
        internal static void SetColumnParms(ref RectangularChartShapeParams columnParams, ref Chart chart, DataPoint dataPoint, Boolean isPositive)
        {
            columnParams.Bevel = dataPoint.Parent.Bevel;
            columnParams.Lighting = (Boolean)dataPoint.LightingEnabled;
            columnParams.Shadow = (Boolean)dataPoint.ShadowEnabled;
            columnParams.BorderBrush = dataPoint.BorderColor;
            columnParams.BorderThickness = ((Thickness)dataPoint.BorderThickness).Left;
            columnParams.BorderStyle = ExtendedGraphics.GetDashArray((BorderStyles)dataPoint.BorderStyle);
            columnParams.IsPositive = isPositive;
            columnParams.BackgroundBrush = dataPoint.Color;

            columnParams.IsMarkerEnabled = (Boolean)dataPoint.MarkerEnabled;
            columnParams.MarkerType = (MarkerTypes)dataPoint.MarkerType;
            columnParams.MarkerColor = dataPoint.MarkerColor;
            columnParams.MarkerBorderColor = dataPoint.MarkerBorderColor;
            columnParams.MarkerBorderThickness = (Thickness)dataPoint.MarkerBorderThickness;
            columnParams.MarkerScale = (Double)dataPoint.MarkerScale;
            columnParams.MarkerSize = (Double)dataPoint.MarkerSize;

            columnParams.IsLabelEnabled = (Boolean)dataPoint.LabelEnabled;
            columnParams.LabelStyle = (LabelStyles)dataPoint.LabelStyle;
            columnParams.LabelText = dataPoint.TextParser(dataPoint.LabelText);
            columnParams.LabelBackground = dataPoint.LabelBackground;
            columnParams.LabelFontColor = dataPoint.LabelFontColor;
            columnParams.LabelFontSize = (Double)dataPoint.LabelFontSize;
            columnParams.LabelFontFamily = dataPoint.LabelFontFamily;
            columnParams.LabelFontStyle = (FontStyle)dataPoint.LabelFontStyle;
            columnParams.LabelFontWeight = (FontWeight)dataPoint.LabelFontWeight;

            columnParams.TagReference = dataPoint;
        }
        
        /// <summary>
        /// Create new Marker
        /// </summary>
        /// <param name="columnParams">Column parameters</param>
        /// <param name="dataPoint">DataPoint</param>
        /// <param name="markerSize">Marker size</param>
        /// <param name="labelText">Label text</param>
        /// <returns>Marker</returns>
        internal static Marker CreateNewMarker(RectangularChartShapeParams columnParams, DataPoint dataPoint, Size markerSize, String labelText)
        {
            Boolean markerBevel = false;

            Marker marker = new Marker(columnParams.MarkerType, columnParams.MarkerScale, markerSize, markerBevel, columnParams.MarkerColor, labelText);

            marker.MarkerSize = markerSize;
            marker.BorderColor = columnParams.MarkerBorderColor;
            marker.BorderThickness = columnParams.MarkerBorderThickness.Left;
            marker.MarkerType = columnParams.MarkerType;
            marker.FontColor = columnParams.LabelFontColor;
            marker.FontFamily = columnParams.LabelFontFamily;
            marker.FontSize = columnParams.LabelFontSize;
            marker.FontStyle = columnParams.LabelFontStyle;
            marker.FontWeight = columnParams.LabelFontWeight;
            marker.TextBackground = columnParams.LabelBackground;

            return marker;
        }

        /// <summary>
        /// Get a dictionary of related DataSeries list with a particular axis, where axis works as key.
        /// </summary>
        /// <param name="seriesList">DataSeries List</param>
        /// <returns>Dictionary[Axis, Dictionary[Axis, Int32]]</returns>
        internal static Dictionary<Axis, Dictionary<Axis, Int32>> GetSeriesIndex(List<DataSeries> seriesList)
        {
            Dictionary<Axis, Dictionary<Axis, Int32>> seriesIndex = new Dictionary<Axis, Dictionary<Axis, Int32>>();

            var seriesByAxis = (from series in seriesList
                                where series.Enabled == true
                                group series by new
                                {
                                    series.PlotGroup.AxisX,
                                    series.PlotGroup.AxisY
                                });

            Int32 index = 0;

            foreach (var entry in seriesByAxis)
            {
                if (seriesIndex.ContainsKey(entry.Key.AxisY))
                {
                    if (!seriesIndex[entry.Key.AxisY].ContainsKey(entry.Key.AxisX))
                    {
                        seriesIndex[entry.Key.AxisY].Add(entry.Key.AxisX, index++);
                    }
                }
                else
                {
                    seriesIndex.Add(entry.Key.AxisY, new Dictionary<Axis, Int32>());
                    seriesIndex[entry.Key.AxisY].Add(entry.Key.AxisX, index++);
                }
            }

            return seriesIndex;
        }
        
        /// <summary>
        /// Get visual object for column chart
        /// </summary>
        /// <param name="width">Width of the PlotArea</param>
        /// <param name="height">Height of the PlotArea</param>
        /// <param name="plotDetails">PlotDetails</param>
        /// <param name="dataSeriesList4Rendering">DataSeriesList with render as Column chart</param>
        /// <param name="chart">Chart</param>
        /// <param name="plankDepth">PlankDepth</param>
        /// <param name="animationEnabled">Whether animation is enabled for chart</param>
        /// <returns>Column chart canvas</returns>
        internal static Canvas GetVisualObjectForColumnChart(Double width, Double height, PlotDetails plotDetails, List<DataSeries> dataSeriesList4Rendering, Chart chart, Double plankDepth, bool animationEnabled)
        {
            if (Double.IsNaN(width) || Double.IsNaN(height) || width <= 0 || height <= 0)
                return null;

            Dictionary<Double, SortDataPoints> sortedDataPoints = plotDetails.GetDataPointsGroupedByXValue(RenderAs.Column);

            List<Double> xValues = sortedDataPoints.Keys.ToList();

            Canvas visual = new Canvas() { Width = width, Height = height };
            Canvas labelCanvas = new Canvas() { Width = width, Height = height };
            Canvas columnCanvas = new Canvas() { Width = width, Height = height };

            List<PlotGroup> plotGroupList = (from plots in plotDetails.PlotGroups where plots.RenderAs == RenderAs.Column select plots).ToList();

            Double depth3d = plankDepth / plotDetails.Layer3DCount * (chart.View3D ? 1 : 0);
            Double visualOffset = depth3d * (plotDetails.SeriesDrawingIndex[plotGroupList[0].DataSeriesList[0]] + 1);

            visual.SetValue(Canvas.TopProperty, visualOffset);
            visual.SetValue(Canvas.LeftProperty, -visualOffset);

            Double minDiffValue = plotDetails.GetMinOfMinDifferencesForXValue(RenderAs.Column, RenderAs.StackedColumn, RenderAs.StackedColumn100);

            if (Double.IsPositiveInfinity(minDiffValue))
                minDiffValue = 0;

            Axis axisXwithMinInterval = dataSeriesList4Rendering[0].PlotGroup.AxisX;

            //minDiffValue = (minDiffValue < (Double)axisXwithMinInterval.InternalInterval) ? minDiffValue : (Double)axisXwithMinInterval.InternalInterval;

            Double dataAxisDifference = width;

            //Double dataMinimumGap = Graphics.ValueToPixelPosition(0, width, (Double)axisXwithMinInterval.InternalAxisMinimum, (Double)axisXwithMinInterval.InternalAxisMaximum, dataAxisDifference + (Double)axisXwithMinInterval.InternalAxisMinimum);
            //Double minDiffGap = Graphics.ValueToPixelPosition(0, width, (Double)axisXwithMinInterval.InternalAxisMinimum, (Double)axisXwithMinInterval.InternalAxisMaximum, minDiffValue + (Double)axisXwithMinInterval.InternalAxisMinimum);

            //minDiffGap = (dataMinimumGap > 0 && minDiffGap > 0) ? Math.Min(minDiffGap, dataMinimumGap) : Math.Max(minDiffGap, dataMinimumGap);

            Double maxColumnWidth = dataAxisDifference * (1 - COLUMN_GAP_RATIO);

            //Double numberOfDivisions = plotDetails.GetMaxDivision(sortedDataPoints);
            Double numberOfDivisions = plotDetails.DrawingDivisionFactor;

            Double widthPerColumn;
            
            if (minDiffValue == 0)
            {
                widthPerColumn = width * .5 / numberOfDivisions;
            }
            else
            {
                widthPerColumn = Graphics.ValueToPixelPosition(0, width, (Double)axisXwithMinInterval.InternalAxisMinimum, (Double)axisXwithMinInterval.InternalAxisMaximum, minDiffValue + (Double)axisXwithMinInterval.InternalAxisMinimum);
                widthPerColumn *= (1 - COLUMN_GAP_RATIO);
                widthPerColumn /= numberOfDivisions;
            }

            if (!Double.IsNaN(chart.DataPointWidth))
            {
                if(chart.DataPointWidth >= 0)
                    widthPerColumn = chart.DataPointWidth / 100 * chart.PlotArea.Width;
            }
            
            Boolean plankDrawn = false;

            foreach (Double xValue in xValues)
            {
                RectangularChartShapeParams columnParams = new RectangularChartShapeParams();
                columnParams.ShadowOffset = 5;
                columnParams.Depth = depth3d;

                foreach (DataPoint dataPoint in sortedDataPoints[xValue].Positive)
                {
                    SetColumnParms(ref columnParams, ref chart, dataPoint, true);

                    columnParams.XRadius = new CornerRadius(dataPoint.RadiusX.Value.TopLeft, dataPoint.RadiusX.Value.TopRight, 0, 0);
                    columnParams.YRadius = new CornerRadius(dataPoint.RadiusY.Value.TopLeft, dataPoint.RadiusY.Value.TopRight, 0, 0);

                    PlotGroup plotGroup = dataPoint.Parent.PlotGroup;

                    Double limitingYValue = 0;
                    if (plotGroup.AxisY.InternalAxisMinimum > 0)
                        limitingYValue = (Double)plotGroup.AxisY.InternalAxisMinimum;
                    if (plotGroup.AxisY.InternalAxisMaximum < 0)
                        limitingYValue = (Double)plotGroup.AxisY.InternalAxisMaximum;

                    List<DataSeries> indexSeriesList = plotDetails.GetSeriesFromDataPoint(dataPoint);
                    Int32 drawingIndex = indexSeriesList.IndexOf(dataPoint.Parent);

                    if (dataPoint.InternalYValue > (Double)plotGroup.AxisY.InternalAxisMaximum)
                        System.Diagnostics.Debug.WriteLine("Max Value greater then axis max");

                    Double left = Graphics.ValueToPixelPosition(0, width, (Double)plotGroup.AxisX.InternalAxisMinimum, (Double)plotGroup.AxisX.InternalAxisMaximum, xValue);
                    left = left + ((Double)drawingIndex - (Double)indexSeriesList.Count() / (Double)2) * widthPerColumn;
                    Double bottom = Graphics.ValueToPixelPosition(height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, limitingYValue);
                    Double top = Graphics.ValueToPixelPosition(height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, dataPoint.InternalYValue);
                    Double columnHeight = Math.Abs(top - bottom);

                    Double finalWidth = CalculateWidthOfEachColumn(ref left, widthPerColumn, width);
                    //Double finalWidth = widthPerColumn;

                    if (finalWidth < 0)
                        continue;

                    columnParams.Size = new Size(finalWidth, columnHeight);

                    Faces columnFaces;
                    Panel columnVisual = null;

                    if (chart.View3D)
                    {
                        columnFaces = Get3DColumn(columnParams);
                        columnVisual = columnFaces.Visual as Panel;
                        columnVisual.SetValue(Canvas.ZIndexProperty, GetColumnZIndex(left, top, (dataPoint.InternalYValue > 0)));
                    }
                    else
                    {
                        columnFaces = Get2DColumn(columnParams);
                        columnVisual = columnFaces.Visual as Panel;
                    }

                    dataPoint.Faces = columnFaces;
                    dataPoint.Faces.LabelCanvas = labelCanvas;

                    columnVisual.SetValue(Canvas.LeftProperty, left);
                    columnVisual.SetValue(Canvas.TopProperty, top);

                    columnCanvas.Children.Add(columnVisual);

                    labelCanvas.Children.Add(GetMarker(chart, columnParams, dataPoint, left, top));

                    // Apply animation
                    if (animationEnabled)
                    {
                        if (dataPoint.Parent.Storyboard == null)
                            dataPoint.Parent.Storyboard = new Storyboard();

                        CurrentDataSeries = dataPoint.Parent;

                        // Apply animation to the data points dataSeriesIndex.e to the rectangles that form the columns
                        dataPoint.Parent.Storyboard = ApplyColumnChartAnimation(columnVisual, dataPoint.Parent.Storyboard, columnParams);

                        // Apply animation to the marker and labels
                        dataPoint.Parent.Storyboard = AnimationHelper.ApplyOpacityAnimation(dataPoint.Marker, CurrentDataSeries, dataPoint.Parent.Storyboard, 1, dataPoint.Opacity * dataPoint.Parent.Opacity);
                    }
                }

                foreach (DataPoint dataPoint in sortedDataPoints[xValue].Negative)
                {
                    SetColumnParms(ref columnParams, ref chart, dataPoint, false);

                    columnParams.XRadius = new CornerRadius(0, 0, dataPoint.RadiusX.Value.BottomRight, dataPoint.RadiusX.Value.BottomLeft);
                    columnParams.YRadius = new CornerRadius(0, 0, dataPoint.RadiusY.Value.BottomRight, dataPoint.RadiusY.Value.BottomLeft);

                    PlotGroup plotGroup = dataPoint.Parent.PlotGroup;

                    Double limitingYValue = 0;
                    if (plotGroup.AxisY.InternalAxisMinimum > 0)
                        limitingYValue = (Double)plotGroup.AxisY.InternalAxisMinimum;
                    if (plotGroup.AxisY.InternalAxisMaximum < 0)
                        limitingYValue = (Double)plotGroup.AxisY.InternalAxisMaximum;

                    List<DataSeries> indexSeriesList = plotDetails.GetSeriesFromDataPoint(dataPoint);
                    Int32 drawingIndex = indexSeriesList.IndexOf(dataPoint.Parent);

                    Double left = Graphics.ValueToPixelPosition(0, width, (Double)plotGroup.AxisX.InternalAxisMinimum, (Double)plotGroup.AxisX.InternalAxisMaximum, xValue);
                    left = left + ((Double)drawingIndex - (Double)indexSeriesList.Count() / (Double)2) * widthPerColumn;
                    Double bottom = Graphics.ValueToPixelPosition(height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, dataPoint.InternalYValue);
                    Double top = Graphics.ValueToPixelPosition(height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, limitingYValue);
                    Double columnHeight = Math.Abs(top - bottom);

                    Double finalWidth = CalculateWidthOfEachColumn(ref left, widthPerColumn, width);

                    if (finalWidth < 0)
                        continue;

                    columnParams.Size = new Size(finalWidth, columnHeight);

                    Faces column;
                    Panel columnVisual = null;

                    if (chart.View3D)
                    {
                        column = Get3DColumn(columnParams);
                        columnVisual = column.Visual as Panel;
                        columnVisual.SetValue(Canvas.ZIndexProperty, GetColumnZIndex(left, top, (dataPoint.InternalYValue > 0)));
                    }
                    else
                    {
                        column = Get2DColumn(columnParams);
                        columnVisual = column.Visual as Panel;
                    }

                    dataPoint.Faces = column;
                    dataPoint.Faces.LabelCanvas = labelCanvas;

                    columnVisual.SetValue(Canvas.LeftProperty, left);
                    columnVisual.SetValue(Canvas.TopProperty, top);

                    columnCanvas.Children.Add(columnVisual);

                    labelCanvas.Children.Add(GetMarker(chart, columnParams, dataPoint, left, top));

                    // Apply animation
                    if (animationEnabled)
                    {
                        if (dataPoint.Parent.Storyboard == null)
                            dataPoint.Parent.Storyboard = new Storyboard();

                        CurrentDataSeries = dataPoint.Parent;

                        // Apply animation to the data points dataSeriesIndex.e to the rectangles that form the columns
                        dataPoint.Parent.Storyboard = ApplyColumnChartAnimation(columnVisual, dataPoint.Parent.Storyboard, columnParams);

                        // Apply animation to the marker and labels
                        dataPoint.Parent.Storyboard = AnimationHelper.ApplyOpacityAnimation(dataPoint.Marker, CurrentDataSeries, dataPoint.Parent.Storyboard, 1, dataPoint.Opacity * dataPoint.Parent.Opacity);
                    }

                }
            }

            if (!plankDrawn && chart.View3D && dataSeriesList4Rendering[0].PlotGroup.AxisY.InternalAxisMinimum < 0 && dataSeriesList4Rendering[0].PlotGroup.AxisY.InternalAxisMaximum > 0)
            {
                RectangularChartShapeParams columnParams = new RectangularChartShapeParams();
                columnParams.BackgroundBrush = new SolidColorBrush(Color.FromArgb((Byte)255, (Byte)127, (Byte)127, (Byte)127));
                columnParams.Lighting = true;
                columnParams.Size = new Size(width, 1);
                columnParams.Depth = depth3d;

                Faces zeroPlank = Get3DColumn(columnParams);
                Panel zeroPlankVisual = zeroPlank.Visual as Panel;

                zeroPlankVisual.IsHitTestVisible = false;

                Double top = height - Graphics.ValueToPixelPosition(0, height, (Double)dataSeriesList4Rendering[0].PlotGroup.AxisY.InternalAxisMinimum, (Double)dataSeriesList4Rendering[0].PlotGroup.AxisY.InternalAxisMaximum, 0);
                zeroPlankVisual.SetValue(Canvas.LeftProperty, (Double)0);
                zeroPlankVisual.SetValue(Canvas.TopProperty, top);
                zeroPlankVisual.SetValue(Canvas.ZIndexProperty, 0);
                zeroPlankVisual.Opacity = 0.7;
                columnCanvas.Children.Add(zeroPlankVisual);
            }

            visual.Children.Add(columnCanvas);
            visual.Children.Add(labelCanvas);

            return visual;
        }

        /// <summary>
        /// Get visual object for stacked column chart
        /// </summary>
        /// <param name="width">Width of the PlotArea</param>
        /// <param name="height">Height of the PlotArea</param>
        /// <param name="plotDetails">PlotDetails</param>
        /// <param name="chart">Chart</param>
        /// <param name="plankDepth">PlankDepth</param>
        /// <param name="animationEnabled">Whether animation is enabled for chart</param>
        /// <returns>StackedColumn chart canvas</returns>
        internal static Canvas GetVisualObjectForStackedColumnChart(Double width, Double height, PlotDetails plotDetails, Chart chart, Double plankDepth, bool animationEnabled)
        {
            if (Double.IsNaN(width) || Double.IsNaN(height) || width <= 0 || height <= 0) return null;

            List<PlotGroup> plotGroupList = (from plots in plotDetails.PlotGroups where plots.RenderAs == RenderAs.StackedColumn select plots).ToList();

            Double widthDivisionFactor = plotDetails.DrawingDivisionFactor;

            Boolean plankDrawn = false;

            Canvas visual = new Canvas() { Width = width, Height = height };
            Canvas labelCanvas = new Canvas() { Width = width, Height = height };
            Canvas columnCanvas = new Canvas() { Width = width, Height = height };

            Double depth3d = plankDepth / plotDetails.Layer3DCount * (chart.View3D ? 1 : 0);
            Double visualOffset = depth3d * (plotDetails.SeriesDrawingIndex[plotGroupList[0].DataSeriesList[0]] + 1);
            visual.SetValue(Canvas.TopProperty, visualOffset);
            visual.SetValue(Canvas.LeftProperty, -visualOffset);

            List<DataSeries> seriesList = plotDetails.GetSeriesListByRenderAs(RenderAs.StackedColumn);

            Dictionary<Axis, Dictionary<Axis, Int32>> seriesIndex = GetSeriesIndex(seriesList);

            foreach (PlotGroup plotGroup in plotGroupList)
            {
                if (!seriesIndex.ContainsKey(plotGroup.AxisY))
                    continue;

                Int32 drawingIndex = seriesIndex[plotGroup.AxisY][plotGroup.AxisX];

                Double minDiff = plotDetails.GetMinOfMinDifferencesForXValue(RenderAs.Column, RenderAs.StackedColumn, RenderAs.StackedColumn100);

                if (Double.IsPositiveInfinity(minDiff))
                    minDiff = 0;

                //minDiff = (minDiff < (Double)plotGroup.AxisX.InternalInterval) ? minDiff : (Double)plotGroup.AxisX.InternalInterval;

                Double maxColumnWidth = Graphics.ValueToPixelPosition(0, width, (Double)plotGroup.AxisX.InternalAxisMinimum, (Double)plotGroup.AxisX.InternalAxisMaximum, minDiff + (Double)plotGroup.AxisX.InternalAxisMinimum) * (1 - COLUMN_GAP_RATIO);

                Double widthPerColumn;

                widthPerColumn = maxColumnWidth / widthDivisionFactor;

                if (minDiff == 0)
                {
                    widthPerColumn = width * .5 / widthDivisionFactor;
                    maxColumnWidth = widthPerColumn * widthDivisionFactor;
                }
                else
                {
                    widthPerColumn = Graphics.ValueToPixelPosition(0, width, plotGroup.AxisX.InternalAxisMinimum, plotGroup.AxisX.InternalAxisMaximum, minDiff + plotGroup.AxisX.InternalAxisMinimum);
                    widthPerColumn *= (1 - COLUMN_GAP_RATIO);
                    maxColumnWidth = widthPerColumn;
                    widthPerColumn /= widthDivisionFactor;
                }

                if (!Double.IsNaN(chart.DataPointWidth))
                {
                    if (chart.DataPointWidth >= 0)
                    {
                        widthPerColumn = maxColumnWidth = chart.DataPointWidth / 100 * chart.PlotArea.Width;

                        maxColumnWidth *= widthDivisionFactor;
                    }
                }

                List<Double> xValuesList = plotGroup.XWiseStackedDataList.Keys.ToList();

                Double limitingYValue = 0;
                if (plotGroup.AxisY.InternalAxisMinimum > 0)
                    limitingYValue = (Double)plotGroup.AxisY.InternalAxisMinimum;
                if (plotGroup.AxisY.InternalAxisMaximum < 0)
                    limitingYValue = (Double)plotGroup.AxisY.InternalAxisMaximum;

                foreach (Double xValue in xValuesList)
                {
                    RectangularChartShapeParams columnParams = new RectangularChartShapeParams();
                    columnParams.ShadowOffset = 5;
                    columnParams.Depth = depth3d;
                    columnParams.IsStacked = true;

                    Double left = Graphics.ValueToPixelPosition(0, width, (Double)plotGroup.AxisX.InternalAxisMinimum, (Double)plotGroup.AxisX.InternalAxisMaximum, xValue) + drawingIndex * widthPerColumn - (maxColumnWidth / 2);
                    Double bottom = Graphics.ValueToPixelPosition(height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, limitingYValue);

                    Double finalWidth = CalculateWidthOfEachColumn(ref left, widthPerColumn, width);

                    if (finalWidth < 0)
                        continue;

                    Double top;
                    Double columnHeight;
                    Double prevSum = 0;
                    Int32 positiveIndex = 1;
                    Int32 negativeIndex = 1;

                    // Plot positive values
                    foreach (DataPoint dataPoint in plotGroup.XWiseStackedDataList[xValue].Positive)
                    {
                        if (!(Boolean)dataPoint.Enabled || Double.IsNaN(dataPoint.InternalYValue))
                            continue;

                        SetColumnParms(ref columnParams, ref chart, dataPoint, true);

                        columnParams.IsTopOfStack = (dataPoint == plotGroup.XWiseStackedDataList[xValue].Positive.Last());
                        if (columnParams.IsTopOfStack)
                        {
                            columnParams.XRadius = new CornerRadius(dataPoint.RadiusX.Value.TopLeft, dataPoint.RadiusX.Value.TopRight, 0, 0);
                            columnParams.YRadius = new CornerRadius(dataPoint.RadiusY.Value.TopLeft, dataPoint.RadiusY.Value.TopRight, 0, 0);
                        }

                        top = Graphics.ValueToPixelPosition(height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, dataPoint.InternalYValue + prevSum);
                        columnHeight = Math.Abs(top - bottom);

                        prevSum += dataPoint.InternalYValue;

                        columnParams.Size = new Size(finalWidth, columnHeight);

                        Faces column;
                        Panel columnVisual = null;

                        if (chart.View3D)
                        {
                            column = Get3DColumn(columnParams);
                            columnVisual = column.Visual as Panel;
                            columnVisual.SetValue(Canvas.ZIndexProperty, GetStackedColumnZIndex(left, top, (dataPoint.InternalYValue > 0), positiveIndex++));
                        }
                        else
                        {
                            column = Get2DColumn(columnParams);
                            columnVisual = column.Visual as Panel;
                        }

                        dataPoint.Faces = column;
                        dataPoint.Faces.LabelCanvas = labelCanvas;

                        columnVisual.SetValue(Canvas.LeftProperty, left);
                        columnVisual.SetValue(Canvas.TopProperty, top);

                        columnCanvas.Children.Add(columnVisual);
                        labelCanvas.Children.Add(GetMarker(chart, columnParams, dataPoint, left, top));

                        // Apply animation
                        if (animationEnabled)
                        {
                            if (dataPoint.Parent.Storyboard == null)
                                dataPoint.Parent.Storyboard = new Storyboard();

                            CurrentDataSeries = dataPoint.Parent;

                            // Apply animation to the data points dataSeriesIndex.e to the rectangles that form the columns
                            dataPoint.Parent.Storyboard = ApplyStackedColumnChartAnimation(columnVisual, dataPoint.Parent.Storyboard, columnParams, (1.0 / seriesList.Count) * (Double)(seriesList.IndexOf(dataPoint.Parent)), 1.0 / seriesList.Count);

                            // Apply animation to the marker and labels
                            dataPoint.Parent.Storyboard = AnimationHelper.ApplyOpacityAnimation(dataPoint.Marker, CurrentDataSeries, dataPoint.Parent.Storyboard, 1, dataPoint.Opacity * dataPoint.Parent.Opacity);
                        }

                        bottom = top;
                    }

                    prevSum = 0;

                    top = Graphics.ValueToPixelPosition(height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, limitingYValue);

                    // Plot negative values
                    foreach (DataPoint dataPoint in plotGroup.XWiseStackedDataList[xValue].Negative)
                    {
                        if (!(Boolean)dataPoint.Enabled || Double.IsNaN(dataPoint.InternalYValue))
                            continue;

                        SetColumnParms(ref columnParams, ref chart, dataPoint, false);

                        columnParams.IsTopOfStack = (dataPoint == plotGroup.XWiseStackedDataList[xValue].Negative.Last());
                        if (columnParams.IsTopOfStack)
                        {
                            columnParams.XRadius = new CornerRadius(0, 0, dataPoint.RadiusX.Value.BottomRight, dataPoint.RadiusX.Value.BottomLeft);
                            columnParams.YRadius = new CornerRadius(0, 0, dataPoint.RadiusY.Value.BottomRight, dataPoint.RadiusY.Value.BottomLeft);
                        }

                        bottom = Graphics.ValueToPixelPosition(height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, dataPoint.InternalYValue + prevSum);
                        columnHeight = Math.Abs(top - bottom);

                        prevSum += dataPoint.InternalYValue;

                        columnParams.Size = new Size(finalWidth, columnHeight);

                        Faces column;
                        Panel columnVisual = null;

                        if (chart.View3D)
                        {
                            column = Get3DColumn(columnParams);
                            columnVisual = column.Visual as Panel;
                            columnVisual.SetValue(Canvas.ZIndexProperty, GetStackedColumnZIndex(left, top, (dataPoint.InternalYValue > 0), negativeIndex--));
                        }
                        else
                        {
                            column = Get2DColumn(columnParams);
                            columnVisual = column.Visual as Panel;
                        }

                        dataPoint.Faces = column;
                        dataPoint.Faces.LabelCanvas = labelCanvas;

                        columnVisual.SetValue(Canvas.LeftProperty, left);
                        columnVisual.SetValue(Canvas.TopProperty, top);

                        columnCanvas.Children.Add(columnVisual);
                        labelCanvas.Children.Add(GetMarker(chart, columnParams, dataPoint, left, top));

                        // Apply animation
                        if (animationEnabled)
                        {
                            if (dataPoint.Parent.Storyboard == null)
                                dataPoint.Parent.Storyboard = new Storyboard();

                            CurrentDataSeries = dataPoint.Parent;

                            // Apply animation to the data points dataSeriesIndex.e to the rectangles that form the columns
                            dataPoint.Parent.Storyboard = ApplyStackedColumnChartAnimation(columnVisual, dataPoint.Parent.Storyboard, columnParams, (1.0 / seriesList.Count) * (Double)(seriesList.IndexOf(dataPoint.Parent)), 1.0 / seriesList.Count);

                            // Apply animation to the marker and labels
                            dataPoint.Parent.Storyboard = AnimationHelper.ApplyOpacityAnimation(dataPoint.Marker, CurrentDataSeries, dataPoint.Parent.Storyboard, 1, dataPoint.Opacity * dataPoint.Parent.Opacity);
                        }

                        top = bottom;
                    }

                }

            }
            if (!plankDrawn && chart.View3D && plotGroupList[0].AxisY.InternalAxisMinimum < 0 && plotGroupList[0].AxisY.InternalAxisMaximum > 0)
            {
                RectangularChartShapeParams columnParams = new RectangularChartShapeParams();
                columnParams.BackgroundBrush = new SolidColorBrush(Color.FromArgb((Byte)255, (Byte)127, (Byte)127, (Byte)127));
                columnParams.Lighting = true;
                columnParams.Size = new Size(width, 1);
                columnParams.Depth = depth3d;

                Faces zeroPlank = Get3DColumn(columnParams);
                Panel zeroPlankVisual = zeroPlank.Visual as Panel;

                zeroPlankVisual.IsHitTestVisible = false;

                Double top = height - Graphics.ValueToPixelPosition(0, height, (Double)plotGroupList[0].AxisY.InternalAxisMinimum, (Double)plotGroupList[0].AxisY.InternalAxisMaximum, 0);
                zeroPlankVisual.SetValue(Canvas.LeftProperty, (Double)0);
                zeroPlankVisual.SetValue(Canvas.TopProperty, top);
                zeroPlankVisual.SetValue(Canvas.ZIndexProperty, 0);
                zeroPlankVisual.Opacity = 0.7;
                columnCanvas.Children.Add(zeroPlankVisual);
            }
            visual.Children.Add(columnCanvas);
            visual.Children.Add(labelCanvas);
            return visual;
        }

        /// <summary>
        /// Get visual object for stacked column100 chart
        /// </summary>
        /// <param name="width">Width of the PlotArea</param>
        /// <param name="height">Height of the PlotArea</param>
        /// <param name="plotDetails">PlotDetails</param>
        /// <param name="chart">Chart</param>
        /// <param name="plankDepth">PlankDepth</param>
        /// <param name="animationEnabled">Whether animation is enabled for chart</param>
        /// <returns>StackedColumn100 chart Canvas</returns>
        internal static Canvas GetVisualObjectForStackedColumn100Chart(Double width, Double height, PlotDetails plotDetails, Chart chart, Double plankDepth, bool animationEnabled)
        {
            if (Double.IsNaN(width) || Double.IsNaN(height) || width <= 0 || height <= 0) return null;

            List<PlotGroup> plotGroupList = (from plots in plotDetails.PlotGroups where plots.RenderAs == RenderAs.StackedColumn100 select plots).ToList();

            Double widthDivisionFactor = plotDetails.DrawingDivisionFactor;

            Boolean plankDrawn = false;

            Canvas visual = new Canvas();
            visual.Width = width;
            visual.Height = height;

            Canvas labelCanvas = new Canvas();
            labelCanvas.Width = width;
            labelCanvas.Height = height;

            Canvas columnCanvas = new Canvas();
            columnCanvas.Width = width;
            columnCanvas.Height = height;

            Double depth3d = plankDepth / plotDetails.Layer3DCount * (chart.View3D ? 1 : 0);
            Double visualOffset = depth3d * (plotDetails.SeriesDrawingIndex[plotGroupList[0].DataSeriesList[0]] + 1);
            visual.SetValue(Canvas.TopProperty, visualOffset);
            visual.SetValue(Canvas.LeftProperty, -visualOffset);

            List<DataSeries> seriesList = plotDetails.GetSeriesListByRenderAs(RenderAs.StackedColumn100);

            Dictionary<Axis, Dictionary<Axis, Int32>> seriesIndex = GetSeriesIndex(seriesList);

            foreach (PlotGroup plotGroup in plotGroupList)
            {
                if (!seriesIndex.ContainsKey(plotGroup.AxisY))
                    continue;

                Int32 drawingIndex = seriesIndex[plotGroup.AxisY][plotGroup.AxisX];

                Double minDiff = plotDetails.GetMinOfMinDifferencesForXValue(RenderAs.Column, RenderAs.StackedColumn, RenderAs.StackedColumn100);

                if (Double.IsPositiveInfinity(minDiff))
                    minDiff = 0;

                //minDiff = (minDiff < (Double)plotGroup.AxisX.InternalInterval) ? minDiff : (Double)plotGroup.AxisX.InternalInterval;

                Double maxColumnWidth = Graphics.ValueToPixelPosition(0, width, (Double)plotGroup.AxisX.InternalAxisMinimum, (Double)plotGroup.AxisX.InternalAxisMaximum, minDiff + (Double)plotGroup.AxisX.InternalAxisMinimum) * (1 - COLUMN_GAP_RATIO);
                Double widthPerColumn = maxColumnWidth / widthDivisionFactor;

                if (minDiff == 0)
                {
                    widthPerColumn = width * .5 / widthDivisionFactor;
                    maxColumnWidth = widthPerColumn * widthDivisionFactor;
                }
                else
                {
                    widthPerColumn = Graphics.ValueToPixelPosition(0, width, plotGroup.AxisX.InternalAxisMinimum, plotGroup.AxisX.InternalAxisMaximum, minDiff + plotGroup.AxisX.InternalAxisMinimum);
                    widthPerColumn *= (1 - COLUMN_GAP_RATIO);
                    maxColumnWidth = widthPerColumn;
                    widthPerColumn /= widthDivisionFactor;
                }

                if (!Double.IsNaN(chart.DataPointWidth))
                {
                    if (chart.DataPointWidth >= 0)
                    {
                        widthPerColumn = maxColumnWidth = chart.DataPointWidth / 100 * chart.PlotArea.Width;
                        maxColumnWidth *= widthDivisionFactor;
                    }
                }

                List<Double> xValuesList = plotGroup.XWiseStackedDataList.Keys.ToList();

                Double limitingYValue = 0;
                if (plotGroup.AxisY.InternalAxisMinimum > 0)
                    limitingYValue = (Double)plotGroup.AxisY.InternalAxisMinimum;
                if (plotGroup.AxisY.InternalAxisMaximum < 0)
                    limitingYValue = (Double)plotGroup.AxisY.InternalAxisMaximum;

                foreach (Double xValue in xValuesList)
                {
                    RectangularChartShapeParams columnParams = new RectangularChartShapeParams();
                    columnParams.ShadowOffset = 5;
                    columnParams.Depth = depth3d;

                    Double left = Graphics.ValueToPixelPosition(0, width, (Double)plotGroup.AxisX.InternalAxisMinimum, (Double)plotGroup.AxisX.InternalAxisMaximum, xValue) + drawingIndex * widthPerColumn - (maxColumnWidth / 2);
                    Double bottom = Graphics.ValueToPixelPosition(height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, limitingYValue);

                    Double finalWidth = CalculateWidthOfEachColumn(ref left, widthPerColumn, width);

                    if (finalWidth < 0)
                        continue;

                    Double absoluteSum = plotGroup.XWiseStackedDataList[xValue].AbsoluteYValueSum;

                    if (Double.IsNaN(absoluteSum) || absoluteSum <= 0)
                        absoluteSum = 1;

                    Double top;
                    Double columnHeight;
                    Double prevSum = 0;
                    Double percentYValue;
                    //Int32 index = 1;

                    Int32 positiveIndex = 1;
                    Int32 negativeIndex = 1;
                    // Plot positive values
                    foreach (DataPoint dataPoint in plotGroup.XWiseStackedDataList[xValue].Positive)
                    {
                        if (!(Boolean)dataPoint.Enabled || Double.IsNaN(dataPoint.InternalYValue))
                            continue;
                        SetColumnParms(ref columnParams, ref chart, dataPoint, true);

                        columnParams.IsTopOfStack = (dataPoint == plotGroup.XWiseStackedDataList[xValue].Positive.Last());
                        if (columnParams.IsTopOfStack)
                        {
                            columnParams.XRadius = new CornerRadius(dataPoint.RadiusX.Value.TopLeft, dataPoint.RadiusX.Value.TopRight, 0, 0);
                            columnParams.YRadius = new CornerRadius(dataPoint.RadiusY.Value.TopLeft, dataPoint.RadiusY.Value.TopRight, 0, 0);
                        }

                        percentYValue = (dataPoint.InternalYValue / absoluteSum * 100);
                        top = Graphics.ValueToPixelPosition(height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, percentYValue + prevSum);
                        columnHeight = Math.Abs(top - bottom);

                        prevSum += percentYValue;
                        columnParams.Size = new Size(finalWidth, columnHeight);

                        Faces column;
                        Panel columnVisual = null;

                        if (chart.View3D)
                        {
                            column = Get3DColumn(columnParams);
                            columnVisual = column.Visual as Panel;
                            columnVisual.SetValue(Canvas.ZIndexProperty, GetStackedColumnZIndex(left, top, (dataPoint.InternalYValue > 0), positiveIndex++));
                        }
                        else
                        {
                            column = Get2DColumn(columnParams);
                            columnVisual = column.Visual as Panel;
                        }

                        dataPoint.Faces = column;
                        dataPoint.Faces.LabelCanvas = labelCanvas;

                        columnVisual.SetValue(Canvas.LeftProperty, left);
                        columnVisual.SetValue(Canvas.TopProperty, top);

                        columnCanvas.Children.Add(columnVisual);
                        labelCanvas.Children.Add(GetMarker(chart, columnParams, dataPoint, left, top));

                        // Apply animation
                        if (animationEnabled)
                        {
                            if (dataPoint.Parent.Storyboard == null)
                                dataPoint.Parent.Storyboard = new Storyboard();

                            CurrentDataSeries = dataPoint.Parent;

                            // Apply animation to the data points dataSeriesIndex.e to the rectangles that form the columns
                            dataPoint.Parent.Storyboard = ApplyStackedColumnChartAnimation(columnVisual, dataPoint.Parent.Storyboard, columnParams, (1.0 / seriesList.Count) * (Double)(seriesList.IndexOf(dataPoint.Parent)), 1.0 / seriesList.Count);

                            // Apply animation to the marker and labels
                            dataPoint.Parent.Storyboard = AnimationHelper.ApplyOpacityAnimation(dataPoint.Marker, CurrentDataSeries, dataPoint.Parent.Storyboard, 1, dataPoint.Opacity * dataPoint.Parent.Opacity);
                        }
                        bottom = top;
                    }

                    prevSum = 0;
                    top = Graphics.ValueToPixelPosition(height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, limitingYValue);

                    // Plot negative values
                    foreach (DataPoint dataPoint in plotGroup.XWiseStackedDataList[xValue].Negative)
                    {
                        if (!(Boolean)dataPoint.Enabled || Double.IsNaN(dataPoint.InternalYValue))
                            continue;
                        SetColumnParms(ref columnParams, ref chart, dataPoint, false);


                        columnParams.IsTopOfStack = (dataPoint == plotGroup.XWiseStackedDataList[xValue].Negative.Last());
                        if (columnParams.IsTopOfStack)
                        {
                            columnParams.XRadius = new CornerRadius(0, 0, dataPoint.RadiusX.Value.BottomRight, dataPoint.RadiusX.Value.BottomLeft);
                            columnParams.YRadius = new CornerRadius(0, 0, dataPoint.RadiusY.Value.BottomRight, dataPoint.RadiusY.Value.BottomLeft);
                        }

                        percentYValue = (dataPoint.InternalYValue / absoluteSum * 100);

                        bottom = Graphics.ValueToPixelPosition(height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, percentYValue + prevSum);

                        columnHeight = Math.Abs(top - bottom);

                        prevSum += percentYValue;

                        columnParams.Size = new Size(finalWidth, columnHeight);

                        Faces column;
                        Panel columnVisual = null;

                        if (chart.View3D)
                        {
                            column = Get3DColumn(columnParams);
                            columnVisual = column.Visual as Panel;
                            columnVisual.SetValue(Canvas.ZIndexProperty, GetStackedColumnZIndex(left, top, (dataPoint.InternalYValue > 0), negativeIndex--));
                        }
                        else
                        {
                            column = Get2DColumn(columnParams);
                            columnVisual = column.Visual as Panel;
                        }

                        dataPoint.Faces = column;
                        dataPoint.Faces.LabelCanvas = labelCanvas;

                        columnVisual.SetValue(Canvas.LeftProperty, left);
                        columnVisual.SetValue(Canvas.TopProperty, top);

                        columnCanvas.Children.Add(columnVisual);
                        labelCanvas.Children.Add(GetMarker(chart, columnParams, dataPoint, left, top));

                        // Apply animation
                        if (animationEnabled)
                        {
                            if (dataPoint.Parent.Storyboard == null)
                                dataPoint.Parent.Storyboard = new Storyboard();

                            CurrentDataSeries = dataPoint.Parent;

                            // Apply animation to the data points dataSeriesIndex.e to the rectangles that form the columns
                            dataPoint.Parent.Storyboard = ApplyStackedColumnChartAnimation(columnVisual, dataPoint.Parent.Storyboard, columnParams, (1.0 / seriesList.Count) * (Double)(seriesList.IndexOf(dataPoint.Parent)), 1.0 / seriesList.Count);

                            // Apply animation to the marker and labels
                            dataPoint.Parent.Storyboard = AnimationHelper.ApplyOpacityAnimation(dataPoint.Marker, CurrentDataSeries, dataPoint.Parent.Storyboard, 1, dataPoint.Opacity * dataPoint.Parent.Opacity);
                        }

                        top = bottom;
                    }

                }
            }

            if (!plankDrawn && chart.View3D && plotGroupList[0].AxisY.InternalAxisMinimum < 0 && plotGroupList[0].AxisY.InternalAxisMaximum > 0)
            {
                RectangularChartShapeParams columnParams = new RectangularChartShapeParams();
                columnParams.BackgroundBrush = new SolidColorBrush(Color.FromArgb((Byte)255, (Byte)127, (Byte)127, (Byte)127));
                columnParams.Lighting = true;
                columnParams.Size = new Size(width, 1);
                columnParams.Depth = depth3d;

                Faces zeroPlank = Get3DColumn(columnParams);
                Panel zeroPlankVisual = zeroPlank.Visual as Panel;

                zeroPlankVisual.IsHitTestVisible = false;

                Double top = height - Graphics.ValueToPixelPosition(0, height, (Double)plotGroupList[0].AxisY.InternalAxisMinimum, (Double)plotGroupList[0].AxisY.InternalAxisMaximum, 0);
                zeroPlankVisual.SetValue(Canvas.LeftProperty, (Double)0);
                zeroPlankVisual.SetValue(Canvas.TopProperty, top);
                zeroPlankVisual.SetValue(Canvas.ZIndexProperty, 0);
                zeroPlankVisual.Opacity = 0.7;
                columnCanvas.Children.Add(zeroPlankVisual);
            }

            visual.Children.Add(columnCanvas);
            visual.Children.Add(labelCanvas);

            return visual;
        }

        /// <summary>
        /// Create 2D column for a DataPoint
        /// </summary>
        /// <param name="columnParams">Column parameters</param>
        /// <returns>Faces</returns>
        internal static Faces Get2DColumn(RectangularChartShapeParams columnParams)
        {
            Faces faces = new Faces();
            faces.Parts = new List<FrameworkElement>();
            Canvas columnVisual = new Canvas();

            columnVisual.Width = columnParams.Size.Width;
            columnVisual.Height = columnParams.Size.Height;

            Brush background = (columnParams.Lighting ? Graphics.GetLightingEnabledBrush(columnParams.BackgroundBrush, "Linear", null) : columnParams.BackgroundBrush);

            Canvas columnBase = ExtendedGraphics.Get2DRectangle(columnParams.TagReference, columnParams.Size.Width, columnParams.Size.Height,
                columnParams.BorderThickness, columnParams.BorderStyle, columnParams.BorderBrush,
                background, columnParams.XRadius, columnParams.YRadius);

            ((columnBase.Children[0] as FrameworkElement).Tag as ElementData).VisualElementName = "ColumnBase";
            
            faces.Parts.Add(columnBase.Children[0] as FrameworkElement);
            faces.BorderElements.Add(columnBase.Children[0] as Path);
            
            columnVisual.Children.Add(columnBase);
            
            if (columnParams.Size.Height > 7 && columnParams.Size.Width > 14 && columnParams.Bevel)
            {   
                Canvas bevelCanvas = ExtendedGraphics.Get2DRectangleBevel(columnParams.TagReference, columnParams.Size.Width - columnParams.BorderThickness - columnParams.BorderThickness, columnParams.Size.Height - columnParams.BorderThickness - columnParams.BorderThickness, 6, 6,
                    Graphics.GetBevelTopBrush(columnParams.BackgroundBrush),
                    Graphics.GetBevelSideBrush((columnParams.Lighting ? -70 : 0), columnParams.BackgroundBrush),
                    Graphics.GetBevelSideBrush((columnParams.Lighting ? -110 : 180), columnParams.BackgroundBrush),
                    null);

                foreach (FrameworkElement fe in bevelCanvas.Children)
                    faces.Parts.Add(fe);

                bevelCanvas.SetValue(Canvas.LeftProperty, columnParams.BorderThickness);
                bevelCanvas.SetValue(Canvas.TopProperty, columnParams.BorderThickness);
                columnVisual.Children.Add(bevelCanvas);
            }
            else
            {
                faces.Parts.Add(null);
                faces.Parts.Add(null);
                faces.Parts.Add(null);
                faces.Parts.Add(null);
            }

            if (!columnParams.Lighting && columnParams.Bevel)
            {
                Canvas gradienceCanvas = ExtendedGraphics.Get2DRectangleGradiance(columnParams.TagReference, columnParams.Size.Width, columnParams.Size.Height,
                    Graphics.GetLeftGradianceBrush(63),
                    Graphics.GetRightGradianceBrush(63),
                    Orientation.Vertical);

                foreach (FrameworkElement fe in gradienceCanvas.Children)
                    faces.Parts.Add(fe);

                columnVisual.Children.Add(gradienceCanvas);
            }
            else
            {
                faces.Parts.Add(null);
                faces.Parts.Add(null);
            }

            if (columnParams.Shadow)
            {
                Double shadowVerticalOffsetGap = 1;
                Double shadowVerticalOffset = columnParams.ShadowOffset - shadowVerticalOffsetGap;
                Double shadowHeight = columnParams.Size.Height;
                CornerRadius xRadius = columnParams.XRadius;
                CornerRadius yRadius = columnParams.YRadius;

                if (columnParams.IsStacked)
                {
                    if (columnParams.IsPositive)
                    {
                        if (columnParams.IsTopOfStack)
                        {
                            shadowHeight = columnParams.Size.Height - shadowVerticalOffset + shadowVerticalOffsetGap;
                            shadowVerticalOffset = columnParams.ShadowOffset - shadowVerticalOffsetGap - shadowVerticalOffsetGap;
                            xRadius = new CornerRadius(xRadius.TopLeft, xRadius.TopRight, xRadius.BottomRight, xRadius.BottomLeft);
                            yRadius = new CornerRadius(yRadius.TopLeft, yRadius.TopRight, 0, 0);
                        }
                        else
                        {
                            shadowHeight = columnParams.Size.Height + 6;
                            shadowVerticalOffset = -2;
                            xRadius = new CornerRadius(xRadius.TopLeft, xRadius.TopRight, xRadius.BottomRight, xRadius.BottomLeft);
                            yRadius = new CornerRadius(0, 0, 0, 0);
                        }
                    }
                    else
                    {
                        if (columnParams.IsTopOfStack)
                        {
                            shadowHeight = columnParams.Size.Height - shadowVerticalOffset + shadowVerticalOffsetGap;
                            xRadius = new CornerRadius(xRadius.TopLeft, xRadius.TopRight, xRadius.BottomRight, xRadius.BottomLeft);
                            yRadius = new CornerRadius(yRadius.TopLeft, yRadius.TopRight, 0, 0);
                        }
                        else
                        {
                            shadowHeight = columnParams.Size.Height + columnParams.ShadowOffset + 2;
                            shadowVerticalOffset = -2;
                            xRadius = new CornerRadius(xRadius.TopLeft, xRadius.TopRight, xRadius.BottomRight, xRadius.BottomLeft);
                            yRadius = new CornerRadius(0, 0, 0, 0);
                        }
                    }
                }

                Grid shadowGrid = ExtendedGraphics.Get2DRectangleShadow(columnParams.TagReference ,columnParams.Size.Width, shadowHeight, xRadius, yRadius, columnParams.IsStacked ? 3 : 5);
                shadowGrid.SetValue(Canvas.TopProperty, shadowVerticalOffset);
                shadowGrid.SetValue(Canvas.LeftProperty, columnParams.ShadowOffset);
                shadowGrid.Opacity = 0.7;
                shadowGrid.SetValue(Canvas.ZIndexProperty, -1);
                columnVisual.Children.Add(shadowGrid);
            }

            faces.Visual = columnVisual;

            return faces;
        }

        /// <summary>
        /// Returns faces for 3D column
        /// </summary>
        /// <param name="columnParams">Column parameters</param>
        /// <returns>Faces</returns>
        internal static Faces Get3DColumn(RectangularChartShapeParams columnParams)
        {
            return Get3DColumn(columnParams, null, null, null);
        }

        /// <summary>
        /// Returns faces for 3D column
        /// </summary>
        /// <param name="columnParams">Column parameters</param>
        /// <param name="frontBrush">Brush for front face</param>
        /// <param name="topBrush">Brush for top face</param>
        /// <param name="rightBrush">Brush for right face</param>
        /// <returns>Faces</returns>
        internal static Faces Get3DColumn(RectangularChartShapeParams columnParams, Brush frontBrush, Brush topBrush, Brush rightBrush)
        {
            Faces faces = new Faces();
            faces.Parts = new List<FrameworkElement>();

            Canvas columnVisual = new Canvas();

            columnVisual.Width = columnParams.Size.Width;
            columnVisual.Height = columnParams.Size.Height;

            if (frontBrush == null)
                frontBrush = columnParams.Lighting ? Graphics.GetFrontFaceBrush(columnParams.BackgroundBrush) : columnParams.BackgroundBrush;

            if (topBrush == null)
                topBrush = columnParams.Lighting ? Graphics.GetTopFaceBrush(columnParams.BackgroundBrush) : columnParams.BackgroundBrush;

            if (rightBrush == null)
                rightBrush = columnParams.Lighting ? Graphics.GetRightFaceBrush(columnParams.BackgroundBrush) : columnParams.BackgroundBrush;

            Canvas front = ExtendedGraphics.Get2DRectangle(columnParams.TagReference, columnParams.Size.Width, columnParams.Size.Height,
                columnParams.BorderThickness, columnParams.BorderStyle, columnParams.BorderBrush,
                frontBrush, new CornerRadius(0), new CornerRadius(0));

            faces.Parts.Add(front.Children[0] as FrameworkElement);
            faces.BorderElements.Add(front.Children[0] as Path);

            Canvas top = ExtendedGraphics.Get2DRectangle(columnParams.TagReference, columnParams.Size.Width, columnParams.Depth,
                columnParams.BorderThickness, columnParams.BorderStyle, columnParams.BorderBrush,
                topBrush, new CornerRadius(0), new CornerRadius(0));

            faces.Parts.Add(top.Children[0] as FrameworkElement);
            faces.BorderElements.Add(top.Children[0] as Path);

            top.RenderTransformOrigin = new Point(0, 1);
            SkewTransform skewTransTop = new SkewTransform();
            skewTransTop.AngleX = -45;
            top.RenderTransform = skewTransTop;

            Canvas right = ExtendedGraphics.Get2DRectangle(columnParams.TagReference, columnParams.Depth, columnParams.Size.Height,
                columnParams.BorderThickness, columnParams.BorderStyle, columnParams.BorderBrush,
                rightBrush, new CornerRadius(0), new CornerRadius(0));

            faces.Parts.Add(right.Children[0] as FrameworkElement);
            faces.BorderElements.Add(right.Children[0] as Path);

            right.RenderTransformOrigin = new Point(0, 0);
            SkewTransform skewTransRight = new SkewTransform();
            skewTransRight.AngleY = -45;
            right.RenderTransform = skewTransRight;

            columnVisual.Children.Add(front);
            columnVisual.Children.Add(top);
            columnVisual.Children.Add(right);

            top.SetValue(Canvas.TopProperty, -columnParams.Depth);
            right.SetValue(Canvas.LeftProperty, columnParams.Size.Width);

            faces.Visual = columnVisual;

            faces.VisualComponents.Add(front);
            faces.VisualComponents.Add(top);
            faces.VisualComponents.Add(right);

            return faces;
        }
        
        #endregion

        #region Internal Events And Delegates

        #endregion

        #region Data
        
        /// <summary>
        /// Gap ratio between two column
        /// </summary>
        internal static Double COLUMN_GAP_RATIO = 0.1;

        #endregion
    }
}