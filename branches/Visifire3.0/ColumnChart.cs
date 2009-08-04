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
    public partial class ColumnChart
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
        private static void SetMarkerPosition(Size columnVisualSize, Chart chart, DataPoint dataPoint, String labelText, Size markerSize, Double canvasLeft, Double canvasTop, Point markerPosition)
        {
            Marker marker = dataPoint.Marker;
            Boolean isPositive = (dataPoint.InternalYValue >= 0);

            if ((Boolean)dataPoint.LabelEnabled && !String.IsNullOrEmpty(labelText))
            {
                marker.CreateVisual();

                if (columnVisualSize.Width < marker.TextBlockSize.Width)
                    marker.TextOrientation = Orientation.Vertical;
                else
                    marker.TextOrientation = Orientation.Horizontal;

                LabelStyles labelStyle = LabelStyles.OutSide;
                if (isPositive)
                {   
                    if (marker.TextOrientation == Orientation.Vertical)
                    {   
                        if (canvasTop - marker.MarkerActualSize.Width - marker.MarkerSize.Height < 0)
                            labelStyle = LabelStyles.Inside;
                    }
                    else
                    {   
                        if (canvasTop - marker.MarkerActualSize.Height - marker.MarkerSize.Height < 0)
                            labelStyle = LabelStyles.Inside;
                    }
                }
                else
                {
                    if (marker.TextOrientation == Orientation.Vertical)
                    {
                        if (canvasTop + markerPosition.Y + marker.MarkerActualSize.Width + marker.MarkerSize.Height > chart.PlotArea.BorderElement.Height + chart.ChartArea.PLANK_DEPTH - chart.ChartArea.PLANK_THICKNESS)
                            labelStyle = LabelStyles.Inside;
                    }
                    else
                    {
                        if (canvasTop + markerPosition.Y + marker.MarkerActualSize.Height + marker.MarkerSize.Height > chart.PlotArea.BorderElement.Height + chart.ChartArea.PLANK_DEPTH - chart.ChartArea.PLANK_THICKNESS)
                            labelStyle = LabelStyles.Inside;
                    }
                }

                marker.TextAlignmentX = AlignmentX.Center;

                if (!(Boolean)dataPoint.MarkerEnabled)
                {
                    if (chart.View3D)
                    {
                        if (labelStyle == LabelStyles.OutSide)
                        {
                            if (isPositive)
                                marker.MarkerSize = new Size(markerSize.Width / 2 + chart.ChartArea.PLANK_DEPTH + chart.ChartArea.PLANK_THICKNESS, markerSize.Height / 2 + chart.ChartArea.PLANK_DEPTH + chart.ChartArea.PLANK_THICKNESS);
                            else
                                marker.MarkerSize = new Size(markerSize.Width / 2, markerSize.Height / 2);
                        }
                        else
                            marker.MarkerSize = new Size(markerSize.Width / 2, markerSize.Height / 2);
                    }
                }
                else
                {
                    if (chart.View3D)
                    {
                        labelStyle = LabelStyles.Inside;
                    }
                }

                if (isPositive)
                    marker.TextAlignmentY = labelStyle == LabelStyles.Inside ? AlignmentY.Bottom : AlignmentY.Top;
                else
                    marker.TextAlignmentY = labelStyle == LabelStyles.Inside ? AlignmentY.Top : AlignmentY.Bottom;
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
        private static Marker GetMarker(Size columnVisualSize, Chart chart, DataPoint dataPoint, Double left, Double top)
        {
           // Canvas markerCanvas = new Canvas() { Width = columnParams.Size.Width, Height = columnParams.Size.Height };

            // markerCanvas.SetValue(Canvas.LeftProperty, left);
            // markerCanvas.SetValue(Canvas.TopProperty, top);

            if ((Boolean)dataPoint.MarkerEnabled || (Boolean)dataPoint.LabelEnabled)
            {
                Size markerSize = new Size((Double)dataPoint.MarkerSize, (Double)dataPoint.MarkerSize);
                String labelText = (Boolean)dataPoint.LabelEnabled ? dataPoint.TextParser(dataPoint.LabelText) : "";

                dataPoint.Marker = CreateNewMarker(null, dataPoint, markerSize, labelText);

                if (!(Boolean)dataPoint.MarkerEnabled)
                {
                    dataPoint.Marker.FillColor = new SolidColorBrush(Colors.Transparent);
                    dataPoint.Marker.BorderColor = new SolidColorBrush(Colors.Transparent);
                }

                Point markerPosition = new Point();

                if (dataPoint.InternalYValue >= 0)
                    if (chart.View3D)
                        markerPosition = new Point(columnVisualSize.Width / 2, 0);
                    else
                        markerPosition = new Point(columnVisualSize.Width / 2, 0);
                else
                    if (chart.View3D)
                        markerPosition = new Point(columnVisualSize.Width / 2, columnVisualSize.Height);
                    else
                        markerPosition = new Point(columnVisualSize.Width / 2, columnVisualSize.Height);

                SetMarkerPosition(columnVisualSize, chart, dataPoint, labelText, markerSize, left, top, markerPosition);

                dataPoint.Marker.FontColor = Chart.CalculateDataPointLabelFontColor(chart, dataPoint, dataPoint.LabelFontColor, (dataPoint.YValue == 0)? LabelStyles.OutSide:(LabelStyles)dataPoint.LabelStyle);

                dataPoint.Marker.Tag = new ElementData() { Element = dataPoint };
                dataPoint.Marker.CreateVisual();

                return dataPoint.Marker;
                //dataPoint.Marker.AddToParent(markerCanvas, markerPosition.X, markerPosition.Y, new Point(0.5, 0.5));
            }

            return null;
        }

        /// <summary>
        /// Calculate width of each column
        /// </summary>
        /// <param name="left">Left position</param>
        /// <param name="widthPerColumn">Width of a column</param>
        /// <param name="width">Width of chart canvas</param>
        /// <returns>Final width of DataPoint</returns>
        internal static Double CalculateWidthOfEachColumn(Chart chart, Double width, Axis axis)
        {
            PlotDetails plotDetails = chart.PlotDetails;

            Double minDiffValue = plotDetails.GetMinOfMinDifferencesForXValue(RenderAs.Column, RenderAs.StackedColumn, RenderAs.StackedColumn100);

            if (Double.IsPositiveInfinity(minDiffValue))
                minDiffValue = 0;

            Axis axisXwithMinInterval = axis;
            Double dataAxisDifference = width;
            Double maxColumnWidth = dataAxisDifference * (1 - COLUMN_GAP_RATIO);
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
                if (chart.DataPointWidth >= 0)
                    widthPerColumn = chart.DataPointWidth / 100 * chart.PlotArea.Width;
            }

            Double finalWidth = widthPerColumn;

            return (finalWidth < 2) ? 2 : finalWidth;
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
        internal static Int32 GetStackedColumnZIndex(Double left, Double top, Boolean isPositive, Int32 index)
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
        private static Storyboard ApplyColumnChartAnimation(Panel column, Storyboard storyboard, Boolean isPositive, Double beginTime, Double[] timeCollection, Double[] valueCollection)
        {
            ScaleTransform scaleTransform = new ScaleTransform() { ScaleY = valueCollection[0] };
            column.RenderTransform = scaleTransform;

            if (isPositive)
            {
                column.RenderTransformOrigin = new Point(0.5, 1);
            }
            else
            {
                column.RenderTransformOrigin = new Point(0.5, 0);
            }

            DoubleCollection values = Graphics.GenerateDoubleCollection(valueCollection);
            DoubleCollection frameTimes = Graphics.GenerateDoubleCollection(timeCollection);

            List<KeySpline> splines = null;
            
            if(valueCollection.Length == 2)
                splines = AnimationHelper.GenerateKeySplineList
                (
                new Point(0, 0), new Point(1, 1),
                new Point(0, 1), new Point(0.5, 1)
                );

            DoubleAnimationUsingKeyFrames growAnimation = AnimationHelper.CreateDoubleAnimation(CurrentDataSeries, scaleTransform, "(ScaleTransform.ScaleY)", beginTime, frameTimes, values, splines);

            //if (valueCollection.Length > 2)
            //    growAnimation.SpeedRatio = 2;

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
        internal static Marker CreateNewMarker(RectangularChartShapeParams columnParms, DataPoint dataPoint, Size markerSize, String labelText)
        {   
            Boolean markerBevel = false;

            Marker marker = new Marker((MarkerTypes)dataPoint.MarkerType, (Double) dataPoint.MarkerScale, markerSize, markerBevel, dataPoint.MarkerColor, labelText);

            marker.MarkerSize = markerSize;
            marker.BorderColor = dataPoint.MarkerBorderColor;
            marker.BorderThickness = dataPoint.MarkerBorderThickness.Value.Left;
            marker.MarkerType = (MarkerTypes)dataPoint.MarkerType;
            marker.FontColor = dataPoint.LabelFontColor;
            marker.FontFamily = dataPoint.LabelFontFamily;
            marker.FontSize = (Double)dataPoint.LabelFontSize;
            marker.FontStyle = (FontStyle)dataPoint.LabelFontStyle;
            marker.FontWeight = (FontWeight)dataPoint.LabelFontWeight;
            marker.TextBackground = dataPoint.LabelBackground;

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


        private static void CreateColumnDataPointVisual(Canvas parentCanvas, Canvas labelCanvas, PlotDetails plotDetails, DataPoint dataPoint, Boolean isPositive, Double widthOfAcolumn, Double depth3D, Boolean animationEnabled)
        {
            if (widthOfAcolumn < 0)
                return;

            Chart chart = dataPoint.Chart as Chart;
            
            Double left, bottom, top, columnHeight;
            Size columnVisualSize;

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

            if (isPositive)
            {   
                left = Graphics.ValueToPixelPosition(0, parentCanvas.Width, (Double)plotGroup.AxisX.InternalAxisMinimum, (Double)plotGroup.AxisX.InternalAxisMaximum, dataPoint.InternalXValue);
                left = left + ((Double)drawingIndex - (Double)indexSeriesList.Count() / (Double)2) * widthOfAcolumn;
                bottom = Graphics.ValueToPixelPosition(parentCanvas.Height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, limitingYValue);
                top = Graphics.ValueToPixelPosition(parentCanvas.Height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, dataPoint.InternalYValue);
                columnHeight = Math.Abs(top - bottom);
            }
            else
            {   
                left = Graphics.ValueToPixelPosition(0, parentCanvas.Width, (Double)plotGroup.AxisX.InternalAxisMinimum, (Double)plotGroup.AxisX.InternalAxisMaximum, dataPoint.InternalXValue);
                left = left + ((Double)drawingIndex - (Double)indexSeriesList.Count() / (Double)2) * widthOfAcolumn;
                bottom = Graphics.ValueToPixelPosition(parentCanvas.Height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, dataPoint.InternalYValue);
                top = Graphics.ValueToPixelPosition(parentCanvas.Height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, limitingYValue);
                columnHeight = Math.Abs(top - bottom);
            }

            columnVisualSize = new Size(widthOfAcolumn, columnHeight);

            Faces columnFaces = null;
            Panel columnVisual = null;

            if (chart.View3D)
            {
                columnFaces = Get3DColumn(dataPoint, widthOfAcolumn, columnHeight, depth3D, dataPoint.Color, null, null, null, (Boolean)dataPoint.LightingEnabled,
                    (BorderStyles)dataPoint.BorderStyle, dataPoint.BorderColor, dataPoint.BorderThickness.Left);

                columnVisual = columnFaces.Visual as Panel;
                columnVisual.SetValue(Canvas.ZIndexProperty, GetColumnZIndex(left, top, (dataPoint.InternalYValue > 0)));
            }
            else
            {   
                columnFaces = Get2DColumn(dataPoint, widthOfAcolumn, columnHeight, false, false);
                columnVisual = columnFaces.Visual as Panel;
            }

            columnFaces.IsPositive = isPositive;

            dataPoint.Faces = columnFaces;
            
            columnVisual.SetValue(Canvas.LeftProperty, left);
            columnVisual.SetValue(Canvas.TopProperty, top);

            parentCanvas.Children.Add(columnVisual);

            CreateMarker(dataPoint, labelCanvas, columnVisualSize, left, top);

            // labelCanvas.Children.Add();

            // Apply animation
            if (animationEnabled)
            {
                if (dataPoint.Parent.Storyboard == null)
                    dataPoint.Parent.Storyboard = new Storyboard();

                CurrentDataSeries = dataPoint.Parent;

                // Apply animation to the data points dataSeriesIndex.e to the rectangles that form the columns
                dataPoint.Parent.Storyboard = ApplyColumnChartAnimation(columnVisual, dataPoint.Parent.Storyboard, isPositive, 1, new Double[] { 0, 1 }, new Double[] { 0, 1 });

                // Apply animation to the marker and labels
                dataPoint.Parent.Storyboard = AnimationHelper.ApplyOpacityAnimation(dataPoint.Marker, CurrentDataSeries, dataPoint.Parent.Storyboard, 1, dataPoint.Opacity * dataPoint.Parent.Opacity);
            }

            dataPoint.AttachEvent2DataPointVisualFaces(dataPoint);

            dataPoint._parsedToolTipText = dataPoint.TextParser(dataPoint.ToolTipText);
            dataPoint.AttachToolTip(chart, dataPoint, dataPoint.Faces.VisualComponents);
            
            dataPoint.AttachHref(chart, dataPoint.Faces.VisualComponents, dataPoint.Href, (HrefTargets)dataPoint.HrefTarget);
        }

        private static void CreateMarker(DataPoint dataPoint, Canvas labelCanvas, Size columnVisualSize, Double left, Double top)
        {   
            # region Create marker
            Chart chart = dataPoint.Chart as Chart;

            Point markerPosition = new Point();

            if (dataPoint.InternalYValue >= 0)
                markerPosition = (chart.View3D) ? new Point(columnVisualSize.Width / 2, 0) 
                    : new Point(columnVisualSize.Width / 2, 0);
            else
                markerPosition =(chart.View3D) ? new Point(columnVisualSize.Width / 2, columnVisualSize.Height) 
                    : new Point(columnVisualSize.Width / 2, columnVisualSize.Height);

            Marker marker = null;

            if (dataPoint.Marker == null)
            {   
                marker = GetMarker(columnVisualSize, chart, dataPoint, left, top);

                if(marker != null)
                    marker.AddToParent(labelCanvas, left + markerPosition.X, top + markerPosition.Y, new Point(0.5, 0.5));
            }
            else
            {   
                marker = dataPoint.Marker;

                if (marker != null)
                {
                    marker.Visual.SetValue(Canvas.LeftProperty, left + markerPosition.X - marker.MarkerActualSize.Width / 2);
                    marker.Visual.SetValue(Canvas.TopProperty, top + markerPosition.Y - marker.MarkerActualSize.Width / 2);
                }
            }
            
            if (marker != null && marker.Visual != null)
                dataPoint.AttachToolTip(chart, dataPoint, marker.Visual);

            dataPoint.Marker = marker;

            #endregion
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
        internal static Canvas GetVisualObjectForColumnChart(Panel preExistingPanel, Double width, Double height, PlotDetails plotDetails, List<DataSeries> dataSeriesList4Rendering, Chart chart, Double plankDepth, bool animationEnabled)
        {   
            if (Double.IsNaN(width) || Double.IsNaN(height) || width <= 0 || height <= 0)
                return null;

            Canvas zeroPlankVisual = null;
            Canvas visual, labelCanvas;

            if (preExistingPanel != null)
            {
                visual = preExistingPanel as Canvas;
                labelCanvas = preExistingPanel.Children[0] as Canvas;
            }
            else
            {   
                visual = new Canvas();
                labelCanvas = new Canvas();
            }

            labelCanvas.Width = width;
            labelCanvas.Height = height;

            visual.Width = width;
            visual.Height = height;

            Canvas columnCanvas = new Canvas() { Width = width, Height = height };
                        
            Double depth3d = plankDepth / plotDetails.Layer3DCount * (chart.View3D ? 1 : 0);
            Double visualOffset = depth3d * (plotDetails.SeriesDrawingIndex[dataSeriesList4Rendering[0]] + 1);

            visual.SetValue(Canvas.TopProperty, visualOffset);
            visual.SetValue(Canvas.LeftProperty, -visualOffset);

            Double widthOfAcolumn = CalculateWidthOfEachColumn(chart, width, dataSeriesList4Rendering[0].PlotGroup.AxisX);

            Boolean plankDrawn = false;

            Dictionary<Double, SortDataPoints> sortedDataPoints = plotDetails.GetDataPointsGroupedByXValue(RenderAs.Column);
            Double[] xValues = sortedDataPoints.Keys.ToArray();

            foreach (Double xValue in xValues)
            {   
                List<DataPoint> positive = sortedDataPoints[xValue].Positive;
                List<DataPoint> negative = sortedDataPoints[xValue].Negative;

                foreach (DataPoint dataPoint in positive)
                {   
                    CreateColumnDataPointVisual(columnCanvas, labelCanvas, plotDetails, dataPoint, true, widthOfAcolumn, depth3d, animationEnabled);
                }

                foreach (DataPoint dataPoint in negative)
                {   
                    CreateColumnDataPointVisual(columnCanvas, labelCanvas, plotDetails, dataPoint, false, widthOfAcolumn, depth3d, animationEnabled);
                }
            }

            columnCanvas.Tag = null;
            
            ColumnChart.CreateOrUpdatePlank(chart, dataSeriesList4Rendering[0].PlotGroup.AxisY, columnCanvas, depth3d);

            if (preExistingPanel != null)
            {
                visual.Children.RemoveAt(1);
                visual.Children.Add(columnCanvas);
            }
            else
            {
                labelCanvas.SetValue(Canvas.ZIndexProperty, 1);
                visual.Children.Add(labelCanvas);
                visual.Children.Add(columnCanvas);
            }
            
            return visual;
        }

        public static void Update(ObservableObject sender, VcProperties property, object newValue, Boolean isAxisChanged)
        {
            Boolean isDataPoint = sender.GetType().Equals(typeof(DataPoint));

            if (isDataPoint)
                UpdateDataPoint(sender as DataPoint, property, newValue, isAxisChanged);
            else
                UpdateDataSeries(sender as DataSeries, property, newValue);
        }

        public static void Update(Chart chart, RenderAs currentRenderAs, List<DataSeries> selectedDataSeries4Rendering, VcProperties property, object newValue)
        {   
            Boolean is3D = chart.View3D;
            ChartArea chartArea = chart.ChartArea;
            Canvas ChartVisualCanvas = chart.ChartArea.ChartVisualCanvas;

            // Double width = chart.ChartArea.ChartVisualCanvas.Width;
            // Double height = chart.ChartArea.ChartVisualCanvas.Height;
            
            Panel preExistingPanel = null;
            Dictionary<RenderAs, Panel> RenderedCanvasList = chart.ChartArea.RenderedCanvasList;

            if (chartArea.RenderedCanvasList.ContainsKey(currentRenderAs))
            {
                preExistingPanel = RenderedCanvasList[currentRenderAs];
            }

            Panel renderedChart = chartArea.RenderSeriesFromList(preExistingPanel, selectedDataSeries4Rendering);

            if (preExistingPanel == null)
            {   
                chartArea.RenderedCanvasList.Add(currentRenderAs, renderedChart);
                ChartVisualCanvas.Children.Add(renderedChart);
            }
        }

        private static void UpdateDataSeries(DataSeries dataSeries, VcProperties property, object newValue)
        {
            Chart chart = dataSeries.Chart as Chart;
            Boolean is3D = chart.View3D;
            Canvas ChartVisualCanvas;

            switch (property)
            {
                case VcProperties.DataPoints:
                case VcProperties.Enabled:
                case VcProperties.YValue:

                    ChartVisualCanvas = chart.ChartArea.ChartVisualCanvas;

                    Double width = chart.ChartArea.ChartVisualCanvas.Width;
                    Double height = chart.ChartArea.ChartVisualCanvas.Height;

                    PlotDetails plotDetails = chart.PlotDetails;
                    PlotGroup plotGroup = dataSeries.PlotGroup;

                    Double columnWidth = CalculateWidthOfEachColumn(chart, width, dataSeries.PlotGroup.AxisX);

                    // Dictionary<Double, SortDataPoints> sortedDataPoints = plotDetails.GetDataPointsGroupedByXValue(RenderAs.Column);
                    // Contains a list of serties as per the drawing order generated in the plotdetails
                    
                    List<DataSeries> dataSeriesListInDrawingOrder = plotDetails.SeriesDrawingIndex.Keys.ToList();

                    List<DataSeries> selectedDataSeries4Rendering = new List<DataSeries>();

                    RenderAs currentRenderAs = RenderAs.Column;

                    Int32 currentDrawingIndex = plotDetails.SeriesDrawingIndex[dataSeries];
                                                           
                    for (Int32 i = 0; i < chart.InternalSeries.Count; i++)
                    {   
                        if (currentRenderAs == dataSeriesListInDrawingOrder[i].RenderAs && currentDrawingIndex == plotDetails.SeriesDrawingIndex[dataSeriesListInDrawingOrder[i]])
                            selectedDataSeries4Rendering.Add(dataSeriesListInDrawingOrder[i]);
                    }
                    
                    if (selectedDataSeries4Rendering.Count == 0)
                        return;

                    Panel oldPanel = null;
                    Dictionary<RenderAs, Panel> RenderedCanvasList = chart.ChartArea.RenderedCanvasList;

                    if (chart.ChartArea.RenderedCanvasList.ContainsKey(currentRenderAs))
                    {   
                        oldPanel = RenderedCanvasList[currentRenderAs];
                    }

                    Panel renderedChart = chart.ChartArea.RenderSeriesFromList(oldPanel, selectedDataSeries4Rendering);

                    if (oldPanel == null)
                    {
                        chart.ChartArea.RenderedCanvasList.Add(currentRenderAs, renderedChart);
                        renderedChart.SetValue(Canvas.ZIndexProperty, currentDrawingIndex);
                        ChartVisualCanvas.Children.Add(renderedChart);
                    }
                    else
                        chart.ChartArea.RenderedCanvasList[currentRenderAs] = renderedChart;
                break;
            }
        }

        private static void Update2DAnd3DColumnColor(DataPoint dataPoint, Brush newValue)
        {
            Faces faces = dataPoint.Faces;

            foreach (FrameworkElement fe in faces.Parts)
            {
                if(fe.Tag == null)
                    continue;
                
                switch((fe.Tag as ElementData).VisualElementName)
                {
                    case "ColumnBase": (fe as Rectangle).Fill = ((Boolean)dataPoint.LightingEnabled ? Graphics.GetLightingEnabledBrush(newValue, "Linear", null) : newValue);
                    break;

                    case "FrontFace": (fe as Rectangle).Fill = (Boolean)dataPoint.LightingEnabled ? Graphics.GetFrontFaceBrush((Brush)newValue) : (Brush)newValue; 
                    break;

                    case "TopFace":(fe as Rectangle).Fill = (Boolean)dataPoint.LightingEnabled ? Graphics.GetTopFaceBrush((Brush)newValue):(Brush)newValue;
                    break;

                    case "RightFace":(fe as Rectangle).Fill =(Boolean)dataPoint.LightingEnabled ? Graphics.GetRightFaceBrush((Brush)newValue) : (Brush)newValue;
                    break;
                }
            }

            foreach(FrameworkElement fe in faces.BevelElements)
            {
                switch((fe.Tag as ElementData).VisualElementName)
                {   
                    case "TopBevel":
                        (fe as Shape).Fill = Graphics.GetBevelTopBrush(newValue);
                        break;

                    case "LeftBevel":
                        (fe as Shape).Fill = Graphics.GetBevelSideBrush(((Boolean)dataPoint.LightingEnabled ? -70 : 0), newValue);
                        break;

                    case "RightBevel":
                        (fe as Shape).Fill = Graphics.GetBevelSideBrush(((Boolean)dataPoint.LightingEnabled ? -110 : 180), newValue);
                        break;

                    case "BottomBevel":
                        (fe as Shape).Fill = null;
                        break;
                }
            }

            if(dataPoint.Marker != null)
                dataPoint.Marker.BorderColor = dataPoint.Color;

            //if (!(Parent.Chart as Chart).View3D)
            //{
            //    if (Faces.Parts[0] != null) (Faces.Parts[0] as Path).Fill = (((Boolean)Parent.LightingEnabled) ? Graphics.GetLightingEnabledBrush((Brush)newValue, "Linear", new Double[] { 0.745, 0.99 }) : (Brush)newValue);
            //    if (Faces.Parts[1] != null) (Faces.Parts[1] as Polygon).Fill = Graphics.GetBevelTopBrush((Brush)newValue);
            //    if (Faces.Parts[2] != null) (Faces.Parts[2] as Polygon).Fill = Graphics.GetBevelSideBrush(((Boolean)Parent.LightingEnabled ? -70 : 0), (Brush)newValue);
            //    if (Faces.Parts[3] != null) (Faces.Parts[3] as Polygon).Fill = Graphics.GetBevelSideBrush(((Boolean)Parent.LightingEnabled ? -110 : 180), (Brush)newValue);
            //    if (Faces.Parts[4] != null) (Faces.Parts[4] as Polygon).Fill = null;
            //    if (Faces.Parts[5] != null) (Faces.Parts[5] as Shape).Fill = Graphics.GetLeftGradianceBrush(63);
            //    if (Faces.Parts[6] != null) (Faces.Parts[6] as Shape).Fill = ((Parent.Chart as Chart).PlotDetails.ChartOrientation == ChartOrientationType.Vertical) ? Graphics.GetRightGradianceBrush(63) : Graphics.GetLeftGradianceBrush(63);
            //}
            //else
        }

        private static void UpdateDataPoint(DataPoint dataPoint, VcProperties property, object newValue, Boolean isAxisChanged)
        {
            Chart chart = dataPoint.Chart as Chart;
            Marker marker = dataPoint.Marker;
            DataSeries dataSeries = dataPoint.Parent;
            PlotGroup plotGroup = dataSeries.PlotGroup;
            Canvas columnVisual = dataPoint.Faces.Visual as Canvas;
            Canvas labelCanvas = ((columnVisual.Parent as FrameworkElement).Parent as Panel).Children[0] as Canvas;

            switch (property)
            {
                case VcProperties.Bevel:
                    ApplyOrRemoveBevel(dataPoint);
                    
                    break;
                case VcProperties.Color:
                    Update2DAnd3DColumnColor(dataPoint, (Brush) newValue);
                    // marker.BorderColor = dataPoint.Color;

                    break;

                case VcProperties.Cursor:
                    dataPoint.SetCursor2DataPointVisualFaces();
                    break;

                case VcProperties.Href:
                    dataPoint.SetHref2DataPointVisualFaces();
                    break;

                case VcProperties.HrefTarget:
                    dataPoint.SetHref2DataPointVisualFaces();
                    break;

                case VcProperties.LabelBackground:
                    if (marker == null)
                    {
                        CreateMarker(dataPoint, labelCanvas, new Size(columnVisual.Width, columnVisual.Height),
                          (Double)columnVisual.GetValue(Canvas.LeftProperty), (Double)columnVisual.GetValue(Canvas.TopProperty));
                    }
                    else
                        marker.TextBackground = dataPoint.LabelBackground;
                    break;

                case VcProperties.LabelEnabled:
                    if(marker == null)
                    {   
                        CreateMarker(dataPoint, labelCanvas, new Size(columnVisual.Width, columnVisual.Height),
                          (Double)columnVisual.GetValue(Canvas.LeftProperty), (Double)columnVisual.GetValue(Canvas.TopProperty));
                    }
                    else
                        marker.LabelEnabled = (Boolean)dataPoint.LabelEnabled;
                    break;

                case VcProperties.LabelFontColor:
                    if (marker == null)
                    {
                        CreateMarker(dataPoint, labelCanvas, new Size(columnVisual.Width, columnVisual.Height),
                          (Double)columnVisual.GetValue(Canvas.LeftProperty), (Double)columnVisual.GetValue(Canvas.TopProperty));
                    }
                    else
                        marker.FontColor = dataPoint.LabelFontColor;
                    break;

                case VcProperties.LabelFontFamily:
                    if (marker == null)
                    {
                        CreateMarker(dataPoint, labelCanvas, new Size(columnVisual.Width, columnVisual.Height),
                          (Double)columnVisual.GetValue(Canvas.LeftProperty), (Double)columnVisual.GetValue(Canvas.TopProperty));
                    }
                    else
                        marker.FontFamily = dataPoint.LabelFontFamily;
                    break;

                case VcProperties.LabelFontStyle:
                    if (marker == null)
                    {
                        CreateMarker(dataPoint, labelCanvas, new Size(columnVisual.Width, columnVisual.Height),
                          (Double)columnVisual.GetValue(Canvas.LeftProperty), (Double)columnVisual.GetValue(Canvas.TopProperty));
                    }
                    else
                        marker.FontStyle = (FontStyle)dataPoint.LabelFontStyle;
                    break;

                case VcProperties.LabelFontSize:
                    if (marker == null)
                    {
                        CreateMarker(dataPoint, labelCanvas, new Size(columnVisual.Width, columnVisual.Height),
                          (Double)columnVisual.GetValue(Canvas.LeftProperty), (Double)columnVisual.GetValue(Canvas.TopProperty));
                    }
                    else
                        marker.FontSize = (Double)dataPoint.LabelFontSize;
                    break;

                case VcProperties.LabelFontWeight:
                    if (marker == null)
                    {
                        CreateMarker(dataPoint, labelCanvas, new Size(columnVisual.Width, columnVisual.Height),
                          (Double)columnVisual.GetValue(Canvas.LeftProperty), (Double)columnVisual.GetValue(Canvas.TopProperty));
                    }
                    else
                        marker.FontWeight = (FontWeight)dataPoint.LabelFontWeight;
                    break;

                case VcProperties.LabelStyle:
                    if (marker == null)
                    {
                        CreateMarker(dataPoint, labelCanvas, new Size(columnVisual.Width, columnVisual.Height),
                          (Double)columnVisual.GetValue(Canvas.LeftProperty), (Double)columnVisual.GetValue(Canvas.TopProperty));
                    }
                    break;
                case VcProperties.LabelText:
                    if (marker == null)
                    {   
                        CreateMarker(dataPoint, labelCanvas, new Size(columnVisual.Width, columnVisual.Height),
                          (Double)columnVisual.GetValue(Canvas.LeftProperty), (Double)columnVisual.GetValue(Canvas.TopProperty));
                    }
                    else
                        marker.Text = dataPoint.TextParser(dataPoint.LabelText);

                    break;

                case VcProperties.LegendText:
                    chart.InvokeRender();
                    break;

                case VcProperties.LightingEnabled:
                    ApplyRemoveLighting(dataPoint);
                    break;

                case VcProperties.MarkerBorderColor:
                    marker.BorderColor = dataPoint.MarkerBorderColor;
                    break;
                case VcProperties.MarkerBorderThickness:
                    marker.BorderThickness = dataPoint.MarkerBorderThickness.Value.Left;
                    break;

                case VcProperties.MarkerColor:
                    marker.FillColor = dataPoint.MarkerColor;
                    break;

                case VcProperties.MarkerScale:
                case VcProperties.MarkerSize:
                case VcProperties.MarkerType:
                case VcProperties.MarkerEnabled:
                    if (marker == null)
                    {
                        CreateMarker(dataPoint, labelCanvas, new Size(columnVisual.Width, columnVisual.Height),
                          (Double)columnVisual.GetValue(Canvas.LeftProperty), (Double)columnVisual.GetValue(Canvas.TopProperty));
                    }
                    else if ((Boolean)dataPoint.MarkerEnabled)
                       LineChart.ShowDataPointMarker(dataPoint);
                    else
                        LineChart.HideDataPointMarker(dataPoint);
                    break;

                case VcProperties.ShadowEnabled:
                    ApplyOrRemoveShadow(dataPoint, (dataSeries.RenderAs == RenderAs.StackedColumn || dataSeries.RenderAs == RenderAs.StackedColumn100),
                        true);
                    
                    break;

                case VcProperties.Opacity:

                    if(marker != null)
                        marker.Visual.Opacity = dataPoint.Opacity * dataSeries.Opacity;

                    if(dataPoint.Faces.Visual != null)
                        dataPoint.Faces.Visual.Opacity = dataPoint.Opacity * dataSeries.Opacity;

                    break;

                case VcProperties.ShowInLegend:
                    chart.InvokeRender();
                    break;

                case VcProperties.ToolTipText:
                    dataPoint._parsedToolTipText = dataPoint.TextParser(dataPoint.ToolTipText);
                    break;

                case VcProperties.XValueType:
                    chart.InvokeRender();
                    break;

                case VcProperties.Enabled:
                    UpdateDataSeries(dataSeries, property, newValue);
                    break;

                case VcProperties.XValue:
                    UpdateDataSeries(dataSeries, property, newValue);
                    break;

                case VcProperties.YValue:

                    if (isAxisChanged)
                        UpdateDataSeries(dataSeries, property, newValue);
                    else
                    {
                        if (dataSeries.RenderAs == RenderAs.Column)
                            UpdateVisualForYValue4ColumnChart(chart, dataPoint, isAxisChanged);
                        else
                            UpdateVisualForYValue4StackedColumnChart(chart, dataPoint, isAxisChanged);
                    }

                    // chart.Dispatcher.BeginInvoke(new Action<DataPoint>(UpdateXAndYValue), new object[]{dataPoint});
                    
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="columnCanvas"></param>
        /// <returns>Plank Canvas</returns>
        internal static Canvas CreateOrUpdatePlank(Chart chart, Axis axis, Canvas columnCanvas, Double depth3d)
        {
            Canvas plank = columnCanvas.Tag as Canvas;
            
            if (plank != null)
            {   
                columnCanvas.Children.Remove(plank);
                plank = null;
            }

            if (chart.View3D && axis.InternalAxisMinimum < 0 && axis.InternalAxisMaximum > 0)
            {   
                if(plank == null)
                {   
                    RectangularChartShapeParams columnParams = new RectangularChartShapeParams();
                    columnParams.BackgroundBrush = new SolidColorBrush(Color.FromArgb((Byte)255, (Byte)127, (Byte)127, (Byte)127));
                    columnParams.Lighting = true;
                    columnParams.Size = new Size(columnCanvas.Width, 1);
                    columnParams.Depth = depth3d;

                    Faces zeroPlank = Get3DColumn(columnParams);
                    plank = zeroPlank.Visual as Canvas;

                    Double top = columnCanvas.Height - Graphics.ValueToPixelPosition(0, columnCanvas.Height, (Double)axis.InternalAxisMinimum, (Double)axis.InternalAxisMaximum, 0);
                    plank.SetValue(Canvas.LeftProperty, (Double)0);
                    plank.SetValue(Canvas.TopProperty, top);
                    plank.SetValue(Canvas.ZIndexProperty, 0);
                    plank.Opacity = 0.7;
                    plank.IsHitTestVisible = false;
                    columnCanvas.Children.Add(plank);
                    columnCanvas.Tag = plank;
                }
                else
                {
                    Double top = columnCanvas.Height - Graphics.ValueToPixelPosition(0, columnCanvas.Height, (Double)axis.InternalAxisMinimum, (Double)axis.InternalAxisMaximum, 0);
                    plank.SetValue(Canvas.TopProperty, top);
                }
            }

            return plank;
        }

        public static void UpdateVisualForYValue4ColumnChart(Chart chart, DataPoint dataPoint, Boolean isAxisChanged)
        {   
            DataSeries dataSeries = dataPoint.Parent;             // parent of the current DataPoint
            Canvas oldVisual = dataPoint.Faces.Visual as Canvas;  // Old visual for the column
            Double widthOfAcolumn = oldVisual.Width;              // Width of the old column
            Boolean isPositive = (dataPoint.InternalYValue >= 0); // Whether YValue is positive
            Canvas columnCanvas = oldVisual.Parent as Canvas;     // Existing parent canvas of column

            Double depth3d = chart.ChartArea.PLANK_DEPTH / chart.PlotDetails.Layer3DCount * (chart.View3D ? 1 : 0);
            
            Double oldMarkerTop = Double.NaN;
            Double currentMarkerTop = Double.NaN;

            if(dataPoint.Marker != null && dataPoint.Marker.Visual != null)
                oldMarkerTop = (Double)dataPoint.Marker.Visual.GetValue(Canvas.TopProperty);

            Canvas labelCanvas = (columnCanvas.Parent as Canvas).Children[0] as Canvas;

            // Create new Column with new YValue
            CreateColumnDataPointVisual(columnCanvas, labelCanvas, chart.PlotDetails, dataPoint,
            isPositive, widthOfAcolumn, depth3d, false);

            columnCanvas.Children.Remove(oldVisual);

            if (dataPoint.Storyboard != null)
            {
                dataPoint.Storyboard.Stop();
                dataPoint.Storyboard = null;
            }

            // Update existing Plank
            CreateOrUpdatePlank(chart, dataSeries.PlotGroup.AxisY, columnCanvas, depth3d);

            Boolean animationEnabled = true;

            #region Animate Column

            if (animationEnabled)
            {
                // Calculate scale factor from the old value YValue of the DataPoint

                Double limitingYValue = 0;
                PlotGroup plotGroup = dataSeries.PlotGroup;

                if (plotGroup.AxisY.InternalAxisMinimum > 0)
                    limitingYValue = (Double)plotGroup.AxisY.InternalAxisMinimum;
                if (plotGroup.AxisY.InternalAxisMaximum < 0)
                    limitingYValue = (Double)plotGroup.AxisY.InternalAxisMaximum;

                if (dataPoint.InternalYValue > (Double)plotGroup.AxisY.InternalAxisMaximum)
                    System.Diagnostics.Debug.WriteLine("Max Value greater then axis max");

                Double oldBottom, oldTop, oldColumnHeight;

                if (dataPoint._oldYValue >= 0)
                {
                    oldBottom = Graphics.ValueToPixelPosition(columnCanvas.Height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, limitingYValue);
                    oldTop = Graphics.ValueToPixelPosition(columnCanvas.Height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, dataPoint._oldYValue);
                    oldColumnHeight = Math.Abs(oldTop - oldBottom);
                }
                else
                {
                    oldBottom = Graphics.ValueToPixelPosition(columnCanvas.Height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, dataPoint._oldYValue);
                    oldTop = Graphics.ValueToPixelPosition(columnCanvas.Height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, limitingYValue);
                    oldColumnHeight = Math.Abs(oldTop - oldBottom);
                }

                Double oldScaleFactor = oldColumnHeight / dataPoint.Faces.Visual.Height;

                if (Double.IsInfinity(oldScaleFactor))
                    oldScaleFactor = 0;

                // End Calculate scale factor from the old value YValue of the DataPoint

                dataPoint.Storyboard = new Storyboard();

                if (!Double.IsNaN(oldMarkerTop))
                    currentMarkerTop = (Double)dataPoint.Marker.Visual.GetValue(Canvas.TopProperty);
                
                if ((dataPoint._oldYValue < 0 && dataPoint.InternalYValue < 0 || dataPoint._oldYValue > 0 && dataPoint.InternalYValue > 0))
                {   
                    dataPoint.Storyboard = ApplyColumnChartAnimation(dataPoint.Faces.Visual as Panel, dataPoint.Storyboard, isPositive, 0, new Double[] { 0, 1 }, new Double[] { oldScaleFactor, 1 });
                                      
                    if (!Double.IsNaN(oldMarkerTop))
                    {   
                        dataPoint.Storyboard = AnimationHelper.ApplyPropertyAnimation(dataPoint.Marker.Visual, "(Canvas.Top)", dataPoint, dataPoint.Storyboard, 0,
                            new Double[] { 0, 1 }, new Double[] { oldMarkerTop, currentMarkerTop },
                            AnimationHelper.GenerateKeySplineList( new Point(0, 0), new Point(1, 1), new Point(0, 1), new Point(0.5, 1)));
                    }
                }
                else
                {   
                    // Top position of the DataPoint with new Value
                    Double currentTop = (Double)dataPoint.Faces.Visual.GetValue(Canvas.TopProperty);

                    // Top position of the Plank (Top position of the Zero Line)
                    Double plankTop = columnCanvas.Height - Graphics.ValueToPixelPosition(0, columnCanvas.Height, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, 0);

                    if (dataPoint._oldYValue < 0)
                    {
                        dataPoint.Storyboard = AnimationHelper.ApplyPropertyAnimation(dataPoint.Faces.Visual, "(Canvas.Top)", dataPoint, dataPoint.Storyboard, 0, new Double[] { 0, 0.5, 0.5, 1 }, new Double[] { plankTop, plankTop, plankTop, currentTop }, null);
                        dataPoint.Storyboard = ApplyColumnChartAnimation(dataPoint.Faces.Visual as Panel, dataPoint.Storyboard, false, 0, new Double[] { 0, 0.5, 0.5, 1 }, new Double[] { oldScaleFactor, 0, 0, 1 });
                    }
                    else
                    {
                        dataPoint.Storyboard = AnimationHelper.ApplyPropertyAnimation(dataPoint.Faces.Visual, "(Canvas.Top)", dataPoint, dataPoint.Storyboard, 0, new Double[] { 0, 0.5, 0.5 }, new Double[] { oldTop, plankTop, plankTop }, null);
                        dataPoint.Storyboard = ApplyColumnChartAnimation(dataPoint.Faces.Visual as Panel, dataPoint.Storyboard, false, 0, new Double[] { 0, 0.5, 0.5, 1 }, new Double[] { oldScaleFactor, 0, 0, 1 });
                    }

                    if(!Double.IsNaN(oldMarkerTop))
                        dataPoint.Storyboard = AnimationHelper.ApplyPropertyAnimation(dataPoint.Marker.Visual, "(Canvas.Top)", dataPoint, dataPoint.Storyboard, 0,
                            new Double[] { 0, 0.5, 0.5, 1 }, new Double[] { oldMarkerTop, plankTop, plankTop, currentMarkerTop },
                            null);
                }

                dataPoint.Storyboard.Begin();
            }
                       

            #endregion Apply Animation
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

                    Double finalWidth = 1;// CalculateWidthOfEachColumn(ref left, widthPerColumn, width);

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
                            column = Get2DColumn(dataPoint, finalWidth, columnHeight, false, true);
                            columnVisual = column.Visual as Panel;
                        }

                        dataPoint.Faces = column;
                        dataPoint.Faces.LabelCanvas = labelCanvas;

                        columnVisual.SetValue(Canvas.LeftProperty, left);
                        columnVisual.SetValue(Canvas.TopProperty, top);

                        columnCanvas.Children.Add(columnVisual);
                        //labelCanvas.Children.Add(GetMarker(chart, columnParams, dataPoint, left, top));

                        // Apply animation
                        if (animationEnabled)
                        {
                            if (dataPoint.Parent.Storyboard == null)
                                dataPoint.Parent.Storyboard = new Storyboard();

                            CurrentDataSeries = dataPoint.Parent;

                            // Apply animation to the data points dataSeriesIndex.e to the rectangles that form the columns
                            // recover
                            //dataPoint.Parent.Storyboard = ApplyStackedColumnChartAnimation(columnVisual, dataPoint.Parent.Storyboard, columnParams, (1.0 / seriesList.Count) * (Double)(seriesList.IndexOf(dataPoint.Parent)), 1.0 / seriesList.Count);

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
                            column = Get2DColumn(dataPoint, finalWidth, columnHeight, true, false);
                            columnVisual = column.Visual as Panel;
                        }

                        dataPoint.Faces = column;
                        dataPoint.Faces.LabelCanvas = labelCanvas;

                        columnVisual.SetValue(Canvas.LeftProperty, left);
                        columnVisual.SetValue(Canvas.TopProperty, top);

                        columnCanvas.Children.Add(columnVisual);
                        // labelCanvas.Children.Add(GetMarker(chart, columnParams, dataPoint, left, top));

                        // Apply animation
                        if (animationEnabled)
                        {
                            if (dataPoint.Parent.Storyboard == null)
                                dataPoint.Parent.Storyboard = new Storyboard();

                            CurrentDataSeries = dataPoint.Parent;

                            // Apply animation to the data points dataSeriesIndex.e to the rectangles that form the columns
                            //dataPoint.Parent.Storyboard = ApplyStackedColumnChartAnimation(columnVisual, dataPoint.Parent.Storyboard, columnParams, (1.0 / seriesList.Count) * (Double)(seriesList.IndexOf(dataPoint.Parent)), 1.0 / seriesList.Count);

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

        private static void ApplyRemoveLighting(DataPoint dataPoint)
        {
            Faces faces = dataPoint.Faces;

            Canvas columnVisual = faces.Visual as Canvas;

            // Remove visual elements used for lighting
            faces.ClearList(columnVisual, faces.LightingElements);

            // Add visual elements used for lighting
            if (!(Boolean)dataPoint.LightingEnabled && dataPoint.Parent.Bevel)
            {   
                Canvas gradienceCanvas = ExtendedGraphics.Get2DRectangleGradiance(columnVisual.Width, columnVisual.Height,
                    Graphics.GetLeftGradianceBrush(63),
                    Graphics.GetRightGradianceBrush(63),
                    Orientation.Vertical);

                columnVisual.Children.Add(gradienceCanvas);

                dataPoint.Faces.LightingElements.Add(columnVisual);
            }
             
            foreach (FrameworkElement fe in faces.Parts)
            {
                if (fe.Tag != null && (fe.Tag as ElementData).VisualElementName == "ColumnBase")
                {
                    Brush background = ((Boolean)dataPoint.LightingEnabled ? Graphics.GetLightingEnabledBrush(dataPoint.Color, "Linear", null) : dataPoint.Color);
                    (fe as Rectangle).Fill = background;
                }
            }
        }

        private static void ApplyOrRemoveBevel(DataPoint dataPoint)
        {   
            Faces faces = dataPoint.Faces;
            
            if (faces == null)
                throw new Exception("Faces of DataPoint is null. ColumnChart.ApplyBevel()");

            Canvas columnVisual = faces.Visual as Canvas;

            // Remove visual elements used for lighting
            faces.ClearList(columnVisual, faces.BevelElements);

            // Add visual elements used for lighting
            if (dataPoint.Parent.Bevel && columnVisual.Height > 7 && columnVisual.Width > 14)
            {
                Canvas bevelCanvas = ExtendedGraphics.Get2DRectangleBevel(null, columnVisual.Width - 2 * dataPoint.BorderThickness.Left, columnVisual.Height - 2 * dataPoint.BorderThickness.Left, 6, 6,
                    Graphics.GetBevelTopBrush(dataPoint.Color),
                    Graphics.GetBevelSideBrush(((Boolean)dataPoint.LightingEnabled ? -70 : 0), dataPoint.Color),
                    Graphics.GetBevelSideBrush(((Boolean)dataPoint.LightingEnabled ? -110 : 180), dataPoint.Color),
                    null);

                foreach (FrameworkElement fe in bevelCanvas.Children)
                    dataPoint.Faces.BevelElements.Add(fe);

                dataPoint.Faces.BevelElements.Add(bevelCanvas);

                bevelCanvas.SetValue(Canvas.LeftProperty, dataPoint.BorderThickness.Left);
                bevelCanvas.SetValue(Canvas.TopProperty, dataPoint.BorderThickness.Left);

                columnVisual.Children.Add(bevelCanvas);
            }
        }
        
        /// <summary>
        /// Create 2D column for a DataPoint
        /// </summary>
        /// <param name="columnParams">Column parameters</param>
        /// <returns>Faces</returns>
        internal static Faces Get2DColumn(DataPoint dataPoint, Double width, Double height, Boolean isStacked, Boolean isTopOfStack)
        {   
            Faces faces = new Faces();
            dataPoint.Faces = faces;

            Canvas columnVisual = new Canvas() ;
            faces.Visual = columnVisual;

            columnVisual.Width = width;
            columnVisual.Height = height;

            Brush background = ((Boolean)dataPoint.LightingEnabled ? Graphics.GetLightingEnabledBrush(dataPoint.Color, "Linear", null) : dataPoint.Color);

            Rectangle rectangle;

            Canvas columnBase = ExtendedGraphics.Get2DRectangle(dataPoint,out rectangle, width, height,
                dataPoint.BorderThickness.Left, ExtendedGraphics.GetDashArray((BorderStyles)dataPoint.BorderStyle), dataPoint.BorderBrush,
                background, dataPoint.RadiusX.Value, dataPoint.RadiusX.Value);
            
            (rectangle.Tag as ElementData).VisualElementName = "ColumnBase";

            faces.VisualComponents.Add(rectangle);
            faces.Parts.Add(rectangle);
            faces.BorderElements.Add(rectangle);
            columnVisual.Children.Add(columnBase);

            ApplyOrRemoveBevel(dataPoint);

            ApplyRemoveLighting(dataPoint);

            ApplyOrRemoveShadow(dataPoint, isStacked, isTopOfStack);

            return faces;
        }


        private static void ApplyOrRemoveShadow(DataPoint dataPoint, Boolean isStacked, Boolean isTopOfStack)
        {
            Faces faces = dataPoint.Faces;

            if (faces == null)
                throw new Exception("Faces of DataPoint is null. ColumnChart.ApplyOrRemoveShadow()");
            
            Canvas columnVisual = faces.Visual as Canvas;

            // Remove visual elements used for lighting
            faces.ClearList(columnVisual, faces.ShadowElements);

            if ((Boolean)dataPoint.ShadowEnabled)
            {
                Double shadowVerticalOffsetGap = 1;
                Double shadowVerticalOffset = Chart.SHADOW_DEPTH - shadowVerticalOffsetGap;
                Double shadowHeight = columnVisual.Height;
                CornerRadius xRadius = (CornerRadius)dataPoint.RadiusX;
                CornerRadius yRadius = (CornerRadius)dataPoint.RadiusY;

                if (isStacked)
                {
                    if (dataPoint.InternalXValue >= 0)
                    {
                        if (isTopOfStack)
                        {
                            shadowHeight = columnVisual.Height - shadowVerticalOffset + shadowVerticalOffsetGap;
                            shadowVerticalOffset = Chart.SHADOW_DEPTH - shadowVerticalOffsetGap - shadowVerticalOffsetGap;
                            xRadius = new CornerRadius(xRadius.TopLeft, xRadius.TopRight, xRadius.BottomRight, xRadius.BottomLeft);
                            yRadius = new CornerRadius(yRadius.TopLeft, yRadius.TopRight, 0, 0);
                        }
                        else
                        {
                            shadowHeight = columnVisual.Height + 6;
                            shadowVerticalOffset = -2;
                            xRadius = new CornerRadius(xRadius.TopLeft, xRadius.TopRight, xRadius.BottomRight, xRadius.BottomLeft);
                            yRadius = new CornerRadius(0, 0, 0, 0);
                        }
                    }
                    else
                    {
                        if (isTopOfStack)
                        {
                            shadowHeight = columnVisual.Height - shadowVerticalOffset + shadowVerticalOffsetGap;
                            xRadius = new CornerRadius(xRadius.TopLeft, xRadius.TopRight, xRadius.BottomRight, xRadius.BottomLeft);
                            yRadius = new CornerRadius(yRadius.TopLeft, yRadius.TopRight, 0, 0);
                        }
                        else
                        {
                            shadowHeight = columnVisual.Height + Chart.SHADOW_DEPTH + 2;
                            shadowVerticalOffset = -2;
                            xRadius = new CornerRadius(xRadius.TopLeft, xRadius.TopRight, xRadius.BottomRight, xRadius.BottomLeft);
                            yRadius = new CornerRadius(0, 0, 0, 0);
                        }
                    }
                }

                Grid shadowGrid = ExtendedGraphics.Get2DRectangleShadow(null, columnVisual.Width, shadowHeight, xRadius, yRadius, isStacked ? 3 : 5);
                shadowGrid.SetValue(Canvas.TopProperty, shadowVerticalOffset);
                shadowGrid.SetValue(Canvas.LeftProperty, Chart.SHADOW_DEPTH);
                shadowGrid.Opacity = 0.7;
                shadowGrid.SetValue(Canvas.ZIndexProperty, -1);
                faces.ShadowElements.Add(shadowGrid);
                columnVisual.Children.Add(shadowGrid);
            }
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

        internal static Faces Get3DColumn(FrameworkElement tagRef, Double width, Double height, Double Depth,
            Brush backgroundBrush, Brush frontBrush, Brush topBrush, Brush rightBrush, Boolean lightingEnabled, BorderStyles borderStyle, Brush borderBrush, Double borderThickness)
        {   
            DoubleCollection strokeDashArray = ExtendedGraphics.GetDashArray(borderStyle);
            Faces faces = new Faces();
            faces.Parts = new List<DependencyObject>();

            Canvas columnVisual = new Canvas();

            columnVisual.Width = width;
            columnVisual.Height = height;

            if (frontBrush == null)
                frontBrush = lightingEnabled ? Graphics.GetFrontFaceBrush(backgroundBrush) : backgroundBrush;

            if (topBrush == null)
                topBrush = lightingEnabled ? Graphics.GetTopFaceBrush(backgroundBrush) : backgroundBrush;

            if (rightBrush == null)
                rightBrush = lightingEnabled ? Graphics.GetRightFaceBrush(backgroundBrush) : backgroundBrush;

            Rectangle rectangle;

            Canvas front = ExtendedGraphics.Get2DRectangle(tagRef, out rectangle, width, height,
                borderThickness, strokeDashArray, borderBrush,
                frontBrush, new CornerRadius(0), new CornerRadius(0));

            rectangle.Tag = new ElementData() { VisualElementName = "FrontFace", Element = tagRef };

            faces.VisualComponents.Add(rectangle);
            faces.Parts.Add(rectangle);
            faces.BorderElements.Add(rectangle);

            Canvas top = ExtendedGraphics.Get2DRectangle(tagRef, out rectangle, width, Depth,
                borderThickness,strokeDashArray, borderBrush,
                topBrush, new CornerRadius(0), new CornerRadius(0));

            rectangle.Tag = new ElementData() { VisualElementName = "TopFace", Element = tagRef };

            faces.VisualComponents.Add(rectangle);
            faces.Parts.Add(rectangle);
            faces.BorderElements.Add(rectangle);

            top.RenderTransformOrigin = new Point(0, 1);
            SkewTransform skewTransTop = new SkewTransform();
            skewTransTop.AngleX = -45;
            top.RenderTransform = skewTransTop;

            Canvas right = ExtendedGraphics.Get2DRectangle(tagRef, out rectangle, Depth, height,
                borderThickness,strokeDashArray,  borderBrush,
                rightBrush, new CornerRadius(0), new CornerRadius(0));

            rectangle.Tag = new ElementData() { VisualElementName = "RightFace", Element = tagRef };

            faces.VisualComponents.Add(rectangle);
            faces.Parts.Add(rectangle);
            faces.BorderElements.Add(rectangle);

            right.RenderTransformOrigin = new Point(0, 0);
            SkewTransform skewTransRight = new SkewTransform();
            skewTransRight.AngleY = -45;
            right.RenderTransform = skewTransRight;

            columnVisual.Children.Add(front);
            columnVisual.Children.Add(top);
            columnVisual.Children.Add(right);

            top.SetValue(Canvas.TopProperty, -Depth);
            right.SetValue(Canvas.LeftProperty, width);

            faces.Visual = columnVisual;

            return faces;
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
            faces.Parts = new List<DependencyObject>();

            Canvas columnVisual = new Canvas();

            columnVisual.Width = columnParams.Size.Width;
            columnVisual.Height = columnParams.Size.Height;

            if (frontBrush == null)
                frontBrush = columnParams.Lighting ? Graphics.GetFrontFaceBrush(columnParams.BackgroundBrush) : columnParams.BackgroundBrush;

            if (topBrush == null)
                topBrush = columnParams.Lighting ? Graphics.GetTopFaceBrush(columnParams.BackgroundBrush) : columnParams.BackgroundBrush;

            if (rightBrush == null)
                rightBrush = columnParams.Lighting ? Graphics.GetRightFaceBrush(columnParams.BackgroundBrush) : columnParams.BackgroundBrush;

            Rectangle rectangle;

            Canvas front = ExtendedGraphics.Get2DRectangle(columnParams.TagReference, out rectangle, columnParams.Size.Width, columnParams.Size.Height,
                columnParams.BorderThickness, columnParams.BorderStyle, columnParams.BorderBrush,
                frontBrush, new CornerRadius(0), new CornerRadius(0));

            faces.Parts.Add(rectangle);
            faces.BorderElements.Add(rectangle);

            Canvas top = ExtendedGraphics.Get2DRectangle(columnParams.TagReference,out rectangle, columnParams.Size.Width, columnParams.Depth,
                columnParams.BorderThickness, columnParams.BorderStyle, columnParams.BorderBrush,
                topBrush, new CornerRadius(0), new CornerRadius(0));

            faces.Parts.Add(rectangle);
            faces.BorderElements.Add(rectangle);

            top.RenderTransformOrigin = new Point(0, 1);
            SkewTransform skewTransTop = new SkewTransform();
            skewTransTop.AngleX = -45;
            top.RenderTransform = skewTransTop;

            Canvas right = ExtendedGraphics.Get2DRectangle(columnParams.TagReference, out rectangle, columnParams.Depth, columnParams.Size.Height,
                columnParams.BorderThickness, columnParams.BorderStyle, columnParams.BorderBrush,
                rightBrush, new CornerRadius(0), new CornerRadius(0));

            faces.Parts.Add(rectangle);
            faces.BorderElements.Add(rectangle);

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