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
    internal class RadarChart
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

        private static void CalculateRadarPoints(DataSeries series, ref List<Point> listOfRadarPoints, PlotGroup plotGroup, CircularPlotDetails circularPlotDetails)
        {
            DataPoint currDataPoint;
            DataPoint nextDataPoint;
            DataPoint preDataPoint = null;

            DataPoint firstDataPoint = series.InternalDataPoints[0];
            DataPoint lastDataPoint = series.InternalDataPoints[series.InternalDataPoints.Count - 1];

            for (Int32 i = 0; i < series.InternalDataPoints.Count - 1; i++)
            {
                currDataPoint = series.InternalDataPoints[i];
                Double dataPointIndex = series.InternalDataPoints.IndexOf(currDataPoint);

                if (!(Boolean)currDataPoint.Enabled)
                    continue;

                nextDataPoint = series.InternalDataPoints[i + 1];

                Point dataPointPosition = GetRadarPoint(circularPlotDetails, plotGroup, currDataPoint, dataPointIndex);

                Double radianAngle = Visifire.Commons.CircularLabel.ResetMeanAngle(CalculateAngleByCoordinate(dataPointPosition, circularPlotDetails.Center));

                if (!Double.IsNaN(currDataPoint.InternalYValue) && (currDataPoint.InternalYValue > plotGroup.AxisY.InternalAxisMaximum
                    || currDataPoint.InternalYValue < plotGroup.AxisY.InternalAxisMinimum))
                {
                    currDataPoint._visualPosition = new Point(Double.NaN, Double.NaN);
                    listOfRadarPoints.Add(new Point(circularPlotDetails.Center.X, circularPlotDetails.Center.Y));
                }
                else if (currDataPoint == firstDataPoint && (nextDataPoint != null && Double.IsNaN(nextDataPoint.InternalYValue))
                     && (lastDataPoint != null && !Double.IsNaN(lastDataPoint.InternalYValue)))
                {
                    listOfRadarPoints.Add(dataPointPosition);
                    currDataPoint._visualPosition = dataPointPosition;
                }
                else if (!Double.IsNaN(currDataPoint.InternalYValue)
                    && ((preDataPoint != null && !Double.IsNaN(preDataPoint.InternalYValue))
                    || (nextDataPoint != null && !Double.IsNaN(nextDataPoint.InternalYValue))))
                {
                    listOfRadarPoints.Add(dataPointPosition);
                    currDataPoint._visualPosition = dataPointPosition;
                }
                else
                {
                    currDataPoint._visualPosition = new Point(Double.NaN, Double.NaN);
                    listOfRadarPoints.Add(new Point(circularPlotDetails.Center.X, circularPlotDetails.Center.Y));
                }

                preDataPoint = series.InternalDataPoints[i];

                if (i == series.InternalDataPoints.Count - 2) // If next DataPoint is the last DataPoint
                {
                    dataPointIndex = series.InternalDataPoints.IndexOf(nextDataPoint);
                    dataPointPosition = GetRadarPoint(circularPlotDetails, plotGroup, nextDataPoint, dataPointIndex);

                    radianAngle = Visifire.Commons.CircularLabel.ResetMeanAngle(CalculateAngleByCoordinate(dataPointPosition, circularPlotDetails.Center));

                    if (!Double.IsNaN(nextDataPoint.InternalYValue) && (nextDataPoint.InternalYValue > plotGroup.AxisY.InternalAxisMaximum
                    || nextDataPoint.InternalYValue < plotGroup.AxisY.InternalAxisMinimum))
                    {
                        nextDataPoint._visualPosition = new Point(Double.NaN, Double.NaN);
                        listOfRadarPoints.Add(new Point(circularPlotDetails.Center.X, circularPlotDetails.Center.Y));
                    }
                    else if (!Double.IsNaN(nextDataPoint.InternalYValue)
                    && (preDataPoint != null && !Double.IsNaN(preDataPoint.InternalYValue))
                        || (lastDataPoint != null && !Double.IsNaN(lastDataPoint.InternalYValue)))
                    {
                        listOfRadarPoints.Add(dataPointPosition);
                        nextDataPoint._visualPosition = dataPointPosition;
                    }
                    else
                    {
                        nextDataPoint._visualPosition = new Point(Double.NaN, Double.NaN);
                        listOfRadarPoints.Add(new Point(circularPlotDetails.Center.X, circularPlotDetails.Center.Y));
                    }

                    if (series.InternalDataPoints.Count < circularPlotDetails.ListOfPoints4CircularAxis.Count)
                        listOfRadarPoints.Add(new Point(circularPlotDetails.Center.X, circularPlotDetails.Center.Y));
                }
            }
        }

        private static void DrawMarkers(DataSeries series, Canvas labelCanvas, Chart chart, Double width, Double height, Point center)
        {
            foreach(DataPoint dp in series.InternalDataPoints)
            {
                if (Double.IsNaN(dp._visualPosition.X) || Double.IsNaN(dp._visualPosition.Y))
                    continue;

                Marker marker = null;

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
        }

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

                    SetPositionForDataPointsLabel(chart, isPositionTop, dataPoint, yPosition);

                    dataPoint.Marker.LabelStyle = (LabelStyles)dataPoint.LabelStyle;
                }

                dataPoint.Marker.CreateVisual();

                if (Double.IsNaN(dataPoint.LabelAngle) || dataPoint.LabelAngle == 0)
                {
                    dataPoint.Marker.TextAlignmentX = AlignmentX.Center;

                    SetPositionForDataPointsLabel(chart, isPositionTop, dataPoint, yPosition);
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

        private static void SetPositionForDataPointsLabel(Chart chart, Boolean isPositionTop, DataPoint dataPoint, Double yPosition)
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

        private static void DrawDataSeriesPolygon(List<Point> listOfRadarPoints, DataSeries series, Canvas radarCanvas)
        {
            if (listOfRadarPoints.Count > 0)
            {
                Polygon polygon = new Polygon() { Tag = new ElementData() { Element = series, VisualElementName = "RadarVisual" } };

                polygon.Fill = (Boolean)series.LightingEnabled ? Graphics.GetLightingEnabledBrush(series.Color, "Linear", null) : series.Color;

                polygon.Stroke = (series.BorderColor == null) ? GetDarkerColor(series.Color) : series.BorderColor;
                polygon.StrokeDashArray = ExtendedGraphics.GetDashArray(series.BorderStyle);
                polygon.StrokeThickness = (series.BorderThickness.Left == 0) ? 0.5 : series.BorderThickness.Left;
                polygon.StrokeMiterLimit = 1;
                polygon.Opacity = series.Opacity;

                PointCollection pointCollection = new PointCollection();

                foreach (Point point in listOfRadarPoints)
                    pointCollection.Add(point);

                Rect polygonBounds = AreaChart.GetBounds(pointCollection);

                polygon.Width = polygonBounds.Width;
                polygon.Height = polygonBounds.Height;
                polygon.SetValue(Canvas.TopProperty, polygonBounds.Y);
                polygon.SetValue(Canvas.LeftProperty, polygonBounds.X);

                polygon.Points = pointCollection;
                polygon.Stretch = Stretch.Fill;

                series.Faces.Visual = polygon;

                radarCanvas.Children.Add(polygon);
            }
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

            Canvas radarCanvas = new Canvas();
            Canvas labelCanvas = new Canvas();

            if ((Boolean)series.Enabled)
            {
                PlotGroup plotGroup = series.PlotGroup;

                List<Point> listOfRadarPoints = new List<Point>();

                series.Faces = new Faces();

                CalculateRadarPoints(series, ref listOfRadarPoints, plotGroup, circularPlotDetails);
                DrawMarkers(series, labelCanvas, chart, width, height, circularPlotDetails.Center);
                DrawDataSeriesPolygon(listOfRadarPoints, series, radarCanvas);
            }

            // Apply animation for DataSeries
            if (chart._internalAnimationEnabled)
            {
                if (series.Storyboard == null)
                    series.Storyboard = new Storyboard();

                // Apply animation to radar visual
                series.Storyboard = AnimationHelper.ApplyOpacityAnimation(radarCanvas, series, series.Storyboard, 1, 1, 0, 1);

                // Apply animation to the marker and labels
                series.Storyboard = AnimationHelper.ApplyOpacityAnimation(labelCanvas, series, series.Storyboard, 1.5, 1, 0, 1);
            }

            visual.Children.Add(radarCanvas);
            visual.Children.Add(labelCanvas);

            return visual;
        }

        private static Brush GetDarkerColor(Brush seriesColor)
        {
            if (seriesColor != null && seriesColor.GetType().Equals(typeof(SolidColorBrush)))
            {
                Double intensity = Graphics.GetBrushIntensity(seriesColor);
                Color color = Graphics.GetDarkerColor(Color.FromArgb((Byte)255, (seriesColor as SolidColorBrush).Color.R, (seriesColor as SolidColorBrush).Color.G, (seriesColor as SolidColorBrush).Color.B), intensity);

                Brush newBrush = Graphics.CreateSolidColorBrush(color);
                return newBrush;
            }
            else
                return seriesColor;
        }

        /// <summary>
        /// Get point for Radar
        /// </summary>
        /// <param name="circularPlotDetails"></param>
        /// <param name="plotGroup"></param>
        /// <param name="dp"></param>
        /// <param name="dataPointIndex"></param>
        /// <returns></returns>
        private static Point GetRadarPoint(CircularPlotDetails circularPlotDetails, PlotGroup plotGroup, DataPoint dp, Double dataPointIndex)
        {
            Double yValue;
            if (Double.IsNaN(dp.InternalYValue))
                yValue = 0;
            else
                yValue = dp.InternalYValue;

            Double yPosition = Graphics.ValueToPixelPosition(circularPlotDetails.Radius, 0, plotGroup.AxisY.InternalAxisMinimum, plotGroup.AxisY.InternalAxisMaximum, yValue);

            Double radius = circularPlotDetails.Radius - yPosition;

            Double minAngle = circularPlotDetails.MinAngleInDegree * dataPointIndex;
            Double actualAngle = AxisLabel.GetRadians(minAngle) - (Math.PI / 2);

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

        /// <summary>
        /// Get Opacity for Radar
        /// </summary>
        /// <param name="series"></param>
        /// <returns></returns>
        private static Double GetOpacity4Radar(DataSeries series)
        {
            if (series.GetValue(DataSeries.OpacityProperty) == null)
                return 0.65;
            else
                return (Double)series.Opacity;
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Get visual object for Radar chart
        /// </summary>
        /// <param name="width">Width</param>
        /// <param name="height">Height</param>
        /// <param name="plotDetails">PlotDetails</param>
        /// <param name="seriesList">List of series</param>
        /// <param name="chart">Chart</param>
        /// <param name="isAnimationEnabled">Whether animation is enabled for chart</param>
        /// <returns>Canvas</returns>
        internal static Canvas GetVisualObjectForRadarChart(Double width, Double height, PlotDetails plotDetails, List<DataSeries> seriesList, Chart chart, bool isAnimationEnabled)
        {
            if (Double.IsNaN(width) || Double.IsNaN(height) || width <= 0 || height <= 0) return null;

            Canvas visual = new Canvas() { Width = width, Height = height };

            Axis axisX = seriesList[0].PlotGroup.AxisX;

            Size radarSize = new Size(width, height);

            foreach(DataSeries ds in seriesList)
            {
                if (ds.InternalDataPoints.Count == 0)
                    continue;

                Canvas radarCanvas = GetDataSeriesVisual(chart, radarSize.Width, radarSize.Height, ds, axisX.CircularPlotDetails);

                visual.Children.Add(radarCanvas);
            }

            return visual;
        }

        #endregion

        #region Internal Events And Delegates

        #endregion

        #region Data

        #endregion
    }

    /// <summary>
    /// Contains plotting details about the data required for circular chart
    /// </summary>
    internal class CircularPlotDetails
    {
        /// <summary>
        /// Initializes a new instance of the Visifire.Charts.PlotDetails class
        /// </summary>
        public CircularPlotDetails(ChartOrientationType chartOrientation)
        {
            ChartOrientation = chartOrientation;
        }

        /// <summary>
        /// Radius of Circular chart
        /// </summary>
        public Double Radius
        {
            get;
            set;
        }

        /// <summary>
        /// Center of Circular chart
        /// </summary>
        public Point Center
        {
            get;
            set;
        }

        /// <summary>
        /// List of initial points for circular axis
        /// </summary>
        public List<Point> ListOfPoints4CircularAxis
        {
            get;
            set;
        }

        /// <summary>
        /// List of angles for all spikes (in radian)
        /// </summary>
        public List<Double> AnglesInRadian
        {
            get;
            set;
        }

        /// <summary>
        /// Minimum angle for circular chart in degree
        /// </summary>
        public Double MinAngleInDegree
        {
            get;
            set;
        }

        /// <summary>
        /// Oriantation for chart
        /// </summary>
        public ChartOrientationType ChartOrientation
        {
            get;
            set;
        }

        /// <summary>
        /// Update circular plotdetails
        /// </summary>
        /// <param name="circularLabels"></param>
        /// <param name="radius"></param>
        internal void UpdateCircularPlotDetails(List<CircularAxisLabel> circularLabels, Double radius)
        {
            ListOfPoints4CircularAxis.Clear();
            foreach (CircularAxisLabel label in circularLabels)
            {
                ListOfPoints4CircularAxis.Add(label.Position);
            }

            Radius = radius;
        }

        /// <summary>
        /// Calculate AxisX points for Circular chart
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="axisX"></param>
        /// <param name="maxDataPointsCount"></param>
        internal void CalculateAxisXPoints4Radar(Double width, Double height, Axis axisX, Int32 maxDataPointsCount)
        {
            Int32 noOfSpikes = maxDataPointsCount;
            Double startAngle = -Math.PI / 2;

            Double actualAngle = startAngle;

            Int32 nextIteration = 1;

            Double minAngle = 0;

            Double radius = Math.Min(width, height) / 2;

            Double reducedPercent = 10;
            if (axisX != null && axisX.AxisLabels != null && (Boolean)axisX.AxisLabels.Enabled)
                reducedPercent = 20;

            Radius = radius - (radius * reducedPercent / 100);

            Center = new Point(width / 2, height / 2);

            ListOfPoints4CircularAxis = new List<Point>();
            AnglesInRadian = new List<Double>();

            MinAngleInDegree = 360.0 / noOfSpikes;

            for (Int32 i = 0; i < noOfSpikes; i++)
            {
                Double x = Radius * Math.Cos(actualAngle) + Center.X;
                Double y = Radius * Math.Sin(actualAngle) + Center.Y;

                ListOfPoints4CircularAxis.Add(new Point(x, y));
                AnglesInRadian.Add(actualAngle);

                minAngle = MinAngleInDegree * nextIteration++;
                actualAngle = AxisLabel.GetRadians(minAngle) - (Math.PI / 2);
            }
        }
    }
}
