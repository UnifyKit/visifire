﻿/*   
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

using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using Visifire.Commons;

namespace Visifire.Charts
{
    internal class PolarChart
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

        #endregion

        #region Private Delegates

        #endregion

        #region Private Methods

        /// <summary>
        /// Create Polar series
        /// </summary>
        /// <param name="chart"></param>
        /// <param name="series"></param>
        /// <param name="polarCanvas"></param>
        /// <param name="labelCanvas"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="plotGroup"></param>
        /// <param name="circularPlotDetails"></param>
        private static void CreatePolarSeries(Chart chart, DataSeries series, Canvas polarCanvas, Canvas labelCanvas, Double width, Double height, PlotGroup plotGroup, CircularPlotDetails circularPlotDetails)
        {
            List<List<DataPoint>> brokenLineDataPointsGroup = GetBrokenLineDataPointsGroup(series, circularPlotDetails, plotGroup);
            foreach (List<DataPoint> dataPointList in brokenLineDataPointsGroup)
            {
                foreach (DataPoint dataPoint in dataPointList)
                    DrawMarker(dataPoint, labelCanvas, width, height, circularPlotDetails.Center);

                DrawDataSeriesPath(series, dataPointList, polarCanvas);
            }
        }

        /// <summary>
        /// Get broken list of polar DataPoints collection
        /// </summary>
        /// <param name="dataSeries"></param>
        /// <param name="circularPlotDetails"></param>
        /// <param name="plotGroup"></param>
        /// <returns></returns>
        private static List<List<DataPoint>> GetBrokenLineDataPointsGroup(DataSeries dataSeries, CircularPlotDetails circularPlotDetails, PlotGroup plotGroup)
        {
            Boolean IsStartPoint = true;
            Double xPosition;
            Double yPosition;
            Point endPoint = new Point();
            List<List<DataPoint>> brokenLineDataPointsCollection = new List<List<DataPoint>>();
            List<DataPoint> pointsList4EachBrokenLineGroup = new List<DataPoint>();

            foreach (DataPoint dataPoint in dataSeries.InternalDataPoints)
            {
                if (dataPoint.Enabled == false)
                    continue;

                if (Double.IsNaN(dataPoint.InternalYValue) || ((dataPoint.InternalYValue > plotGroup.AxisY.InternalAxisMaximum)
                    || (dataPoint.InternalYValue < plotGroup.AxisY.InternalAxisMinimum)))
                {
                    xPosition = Double.NaN;
                    yPosition = Double.NaN;
                    IsStartPoint = true;
                }
                else
                {
                    Point point = GetPolarPoint(circularPlotDetails, plotGroup, dataPoint);
                    xPosition = point.X;
                    yPosition = point.Y;

                    #region Generate GeometryGroup for line

                    if (IsStartPoint)
                    {
                        IsStartPoint = !IsStartPoint;

                        if (pointsList4EachBrokenLineGroup.Count > 0)
                        {
                            brokenLineDataPointsCollection.Add(pointsList4EachBrokenLineGroup);
                        }

                        pointsList4EachBrokenLineGroup = new List<DataPoint>();
                    }
                    else
                    {
                        endPoint = new Point(xPosition, yPosition);
                        IsStartPoint = false;
                    }

                    #endregion Generate GeometryGroup for line and line shadow

                    dataPoint._visualPosition = new Point(xPosition, yPosition);
                    pointsList4EachBrokenLineGroup.Add(dataPoint);

                }
            }

            brokenLineDataPointsCollection.Add(pointsList4EachBrokenLineGroup);

            return brokenLineDataPointsCollection;
        }

        /// <summary>
        /// Draw marker for polar DataPoint
        /// </summary>
        /// <param name="dp"></param>
        /// <param name="labelCanvas"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="center"></param>
        private static void DrawMarker(DataPoint dp, Canvas labelCanvas, Double width, Double height, Point center)
        {
            Marker marker = null;

            Chart chart = (dp.Chart as Chart);

            Double radianAngle = Visifire.Commons.CircularLabel.ResetMeanAngle(CalculateAngleByCoordinate(dp._visualPosition, center));

            if (radianAngle > 0 && radianAngle < Math.PI)
                marker = GetMarkerForDataPoint(chart, width, height, dp._visualPosition.Y, dp, false);
            else
                marker = GetMarkerForDataPoint(chart, width, height, dp._visualPosition.Y, dp, true);

            if (marker != null)
            {
                marker.AddToParent(labelCanvas, dp._visualPosition.X, dp._visualPosition.Y, new Point(0.5, 0.5));
            }
        }

        /// <summary>
        /// Get marker for DataPoint
        /// </summary>
        /// <param name="chart"></param>
        /// <param name="plotWidth"></param>
        /// <param name="plotHeight"></param>
        /// <param name="yPosition"></param>
        /// <param name="dataPoint"></param>
        /// <param name="isPositionTop"></param>
        /// <returns></returns>
        private static Marker GetMarkerForDataPoint(Chart chart, Double plotWidth, Double plotHeight, Double yPosition, DataPoint dataPoint, Boolean isPositionTop)
        {
            String labelText = (Boolean)dataPoint.LabelEnabled ? dataPoint.TextParser(dataPoint.LabelText) : "";

            Boolean markerBevel = false;

            Marker marker = dataPoint.Marker;

            if (marker != null && marker.Visual != null)
            {
                Panel parent = marker.Visual.Parent as Panel;

                if (parent != null)
                    parent.Children.Remove(marker.Visual);
            }

            dataPoint.Marker = new Marker((MarkerTypes)dataPoint.MarkerType,
                (Double)dataPoint.MarkerScale,
                new Size((Double)dataPoint.MarkerSize, (Double)dataPoint.MarkerSize),
                markerBevel,
                dataPoint.MarkerColor,
                labelText);

            LineChart.ApplyMarkerProperties(dataPoint);

            if ((Boolean)dataPoint.LabelEnabled && !String.IsNullOrEmpty(labelText))
            {
                LineChart.ApplyLabelProperties(dataPoint);

                if (!Double.IsNaN(dataPoint.LabelAngle) && dataPoint.LabelAngle != 0)
                {
                    dataPoint.Marker.LabelAngle = dataPoint.LabelAngle;
                    dataPoint.Marker.TextOrientation = Orientation.Vertical;

                    SetPositionForDataPointLabel(chart, isPositionTop, dataPoint, yPosition);

                    dataPoint.Marker.LabelStyle = (LabelStyles)dataPoint.LabelStyle;
                }

                dataPoint.Marker.CreateVisual();

                if (Double.IsNaN(dataPoint.LabelAngle) || dataPoint.LabelAngle == 0)
                {
                    dataPoint.Marker.TextAlignmentX = AlignmentX.Center;

                    SetPositionForDataPointLabel(chart, isPositionTop, dataPoint, yPosition);
                }
            }

            dataPoint.Marker.Control = chart;

            dataPoint.Marker.Tag = new ElementData() { Element = dataPoint };

            dataPoint.Marker.CreateVisual();

            dataPoint.Marker.Visual.Opacity = (Double)dataPoint.Opacity * (Double)dataPoint.Parent.Opacity;

            LineChart.ApplyDefaultInteractivityForMarker(dataPoint);

            ObservableObject.AttachEvents2Visual(dataPoint, dataPoint, dataPoint.Marker.Visual);
            ObservableObject.AttachEvents2Visual(dataPoint.Parent, dataPoint, dataPoint.Marker.Visual);
            dataPoint._parsedToolTipText = dataPoint.TextParser(dataPoint.ToolTipText);
            dataPoint.AttachToolTip(chart, dataPoint, dataPoint.Marker.Visual);
            dataPoint.AttachHref(chart, dataPoint.Marker.Visual, dataPoint.Href, (HrefTargets)dataPoint.HrefTarget);
            dataPoint.SetCursor2DataPointVisualFaces();
            return dataPoint.Marker;
        }

        /// <summary>
        /// Set position for DataPoint label
        /// </summary>
        /// <param name="chart"></param>
        /// <param name="isPositionTop"></param>
        /// <param name="dataPoint"></param>
        /// <param name="yPosition"></param>
        private static void SetPositionForDataPointLabel(Chart chart, Boolean isPositionTop, DataPoint dataPoint, Double yPosition)
        {
            if (!Double.IsNaN(dataPoint.LabelAngle) && dataPoint.LabelAngle != 0)
            {
                if (isPositionTop)
                {
                    dataPoint.Marker.TextAlignmentX = AlignmentX.Center;
                    dataPoint.Marker.TextAlignmentY = AlignmentY.Top;
                }
                else
                {
                    dataPoint.Marker.TextAlignmentX = AlignmentX.Center;
                    dataPoint.Marker.TextAlignmentY = AlignmentY.Bottom;
                }
            }
            else
            {
                if (isPositionTop)
                {
                    if (dataPoint.LabelStyle == LabelStyles.OutSide && !dataPoint.IsLabelStyleSet && !dataPoint.Parent.IsLabelStyleSet)
                    {
                        if (yPosition - dataPoint.Marker.MarkerActualSize.Height - dataPoint.Marker.MarkerSize.Height / 2 < 0 || dataPoint.LabelStyle == LabelStyles.Inside)
                            dataPoint.Marker.TextAlignmentY = AlignmentY.Bottom;
                        else
                            dataPoint.Marker.TextAlignmentY = AlignmentY.Top;
                    }
                    else if (dataPoint.LabelStyle == LabelStyles.OutSide)
                        dataPoint.Marker.TextAlignmentY = AlignmentY.Top;
                    else
                        dataPoint.Marker.TextAlignmentY = AlignmentY.Bottom;
                }
                else
                {
                    if (dataPoint.LabelStyle == LabelStyles.OutSide && !dataPoint.IsLabelStyleSet && !dataPoint.Parent.IsLabelStyleSet)
                    {
                        if (yPosition + dataPoint.Marker.MarkerActualSize.Height + dataPoint.Marker.MarkerSize.Height / 2 > chart.PlotArea.BorderElement.Height || dataPoint.LabelStyle == LabelStyles.Inside)
                            dataPoint.Marker.TextAlignmentY = AlignmentY.Top;
                        else
                            dataPoint.Marker.TextAlignmentY = AlignmentY.Bottom;
                    }
                    else if (dataPoint.LabelStyle == LabelStyles.OutSide)
                        dataPoint.Marker.TextAlignmentY = AlignmentY.Bottom;
                    else
                        dataPoint.Marker.TextAlignmentY = AlignmentY.Top;
                }
            }
        }

        /// <summary>
        /// Draw DataSeries visual (Path) for Polar
        /// </summary>
        /// <param name="series"></param>
        /// <param name="pointCollection"></param>
        /// <param name="polarCanvas"></param>
        private static void DrawDataSeriesPath(DataSeries series, List<DataPoint> pointCollection, Canvas polarCanvas)
        {
            if (series.InternalDataPoints.Count > 0)
            {
                Path path = new Path() { Tag = new ElementData() { Element = series, VisualElementName = "PolarVisual" } };

                path.Stroke = (Boolean)series.LightingEnabled ? Graphics.GetLightingEnabledBrush(series.Color, "Linear", null) : series.Color;
                path.StrokeDashArray = ExtendedGraphics.GetDashArray(series.LineStyle);
                path.StrokeThickness = (Double)series.LineThickness;
                path.StrokeMiterLimit = 1;
                path.Opacity = series.Opacity;

                path.Data = GetPathGeometry(pointCollection);

                series.Faces.Parts.Add(path);

                polarCanvas.Children.Add(path);
            }
        }

        /// <summary>
        /// Get path geometry for Polar points
        /// </summary>
        /// <param name="pointCollection"></param>
        /// <returns></returns>
        private static Geometry GetPathGeometry(List<DataPoint> pointCollection)
        {
            PathGeometry geometry = new PathGeometry();

            PathFigure pathFigure = new PathFigure();

            if (pointCollection.Count > 0)
            {
                pathFigure.StartPoint = pointCollection[0]._visualPosition;

                for (int i = 1; i < pointCollection.Count; i++)
                {
                    LineSegment segment = new LineSegment();

                    segment.Point = pointCollection[i]._visualPosition;

                    Faces faces = new Faces();

                    pathFigure.Segments.Add(segment);
                }
            }

            geometry.Figures.Add(pathFigure);

            return geometry;
        }

        /// <summary>
        /// Get visual for DataSeries
        /// </summary>
        /// <param name="chart">Chart</param>
        /// <param name="width">Width</param>
        /// <param name="height">Height</param>
        /// <param name="series">DataSeries</param>
        /// <param name="circularPlotDetails">CircularPlotDetails</param>
        /// <returns>Canvas</returns>
        private static Canvas GetDataSeriesVisual(Chart chart, Double width, Double height, DataSeries series, CircularPlotDetails circularPlotDetails)
        {
            Canvas visual = new Canvas();

            Canvas polarCanvas = new Canvas();
            Canvas labelCanvas = new Canvas();

            if ((Boolean)series.Enabled)
            {
                PlotGroup plotGroup = series.PlotGroup;

                if (circularPlotDetails.ListOfPoints4CircularAxis.Count > 0)
                {
                    series.Faces = new Faces();
                    CreatePolarSeries(chart, series, polarCanvas, labelCanvas, width, height, plotGroup, circularPlotDetails);
                    
                }
            }

            // Apply animation for DataSeries
            if (chart._internalAnimationEnabled)
            {
                if (series.Storyboard == null)
                    series.Storyboard = new Storyboard();

                // Apply animation to radar visual
                series.Storyboard = AnimationHelper.ApplyOpacityAnimation(polarCanvas, series, series.Storyboard, 1, 1, 0, 1);

                // Apply animation to the marker and labels
                series.Storyboard = AnimationHelper.ApplyOpacityAnimation(labelCanvas, series, series.Storyboard, 1.5, 1, 0, 1);
            }

            visual.Children.Add(polarCanvas);
            visual.Children.Add(labelCanvas);

            return visual;
        }

        /// <summary>
        /// Get point for Polar
        /// </summary>
        /// <param name="circularPlotDetails"></param>
        /// <param name="plotGroup"></param>
        /// <param name="dp"></param>
        /// <returns></returns>
        private static Point GetPolarPoint(CircularPlotDetails circularPlotDetails, PlotGroup plotGroup, DataPoint dp)
        {
            Double yValue = dp.InternalYValue;

            Double yPosition = Graphics.ValueToPixelPosition(circularPlotDetails.Radius, 0, plotGroup.AxisY.InternalAxisMinimum, plotGroup.AxisY.InternalAxisMaximum, yValue);

            Double radius = circularPlotDetails.Radius - yPosition;

            Axis axisX = plotGroup.AxisX;

            Double minValInRadian;

            if (axisX.InternalAxisMinimum != 0)
                minValInRadian = AxisLabel.GetRadians(axisX.InternalAxisMinimum - 90);
            else
                minValInRadian = 2 * Math.PI - Math.PI / 2;

            Double actualAngle;

            if(dp.Parent.XValueType == ChartValueTypes.Time)
                actualAngle = Graphics.ValueToPixelPosition(minValInRadian, 2 * Math.PI + minValInRadian, AxisLabel.GetRadians(0), AxisLabel.GetRadians(360), AxisLabel.GetRadians(dp.InternalXValue));
            else
                actualAngle = Graphics.ValueToPixelPosition(minValInRadian, 2 * Math.PI + minValInRadian, AxisLabel.GetRadians(axisX.InternalAxisMinimum), AxisLabel.GetRadians(axisX.InternalAxisMaximum), AxisLabel.GetRadians(axisX.InternalAxisMinimum + dp.InternalXValue));

            Double x = radius * Math.Cos(actualAngle) + circularPlotDetails.Center.X;
            Double y = radius * Math.Sin(actualAngle) + circularPlotDetails.Center.Y;

            return new Point(x, y);
        }

        /// <summary>
        /// Calculate angle by Coordinate
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        private static Double CalculateAngleByCoordinate(Point position, Point center)
        {
            return Math.Atan2((position.Y - center.Y), (position.X - center.X));
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Get visual object for Polar chart
        /// </summary>
        /// <param name="width">Width</param>
        /// <param name="height">Height</param>
        /// <param name="plotDetails">PlotDetails</param>
        /// <param name="seriesList">List of series</param>
        /// <param name="chart">Chart</param>
        /// <param name="isAnimationEnabled">Whether animation is enabled for chart</param>
        /// <returns>Canvas</returns>
        internal static Canvas GetVisualObjectForPolarChart(Double width, Double height, PlotDetails plotDetails, List<DataSeries> seriesList, Chart chart, bool isAnimationEnabled)
        {
            if (Double.IsNaN(width) || Double.IsNaN(height) || width <= 0 || height <= 0) return null;

            Canvas visual = new Canvas() { Width = width, Height = height };

            Axis axisX = seriesList[0].PlotGroup.AxisX;

            Size radarSize = new Size(width, height);

            foreach (DataSeries ds in seriesList)
            {
                if (ds.InternalDataPoints.Count == 0)
                    continue;

                Canvas polarCanvas = GetDataSeriesVisual(chart, radarSize.Width, radarSize.Height, ds, axisX.CircularPlotDetails);

                visual.Children.Add(polarCanvas);
            }

            return visual;
        }

        #endregion

        #region Internal Events And Delegates

        #endregion

        #region Data

        #endregion
    }
}