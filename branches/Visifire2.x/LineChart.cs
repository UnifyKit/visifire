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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
#else
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
#endif

using Visifire.Commons;

namespace Visifire.Charts
{
    /// <summary>
    /// Visifire.Charts.LineChartShapeParams class
    /// </summary>
    internal class LineChartShapeParams
    {
        internal PointCollection Points { get; set; }
        internal GeometryGroup LineGeometryGroup { get; set; }
        internal GeometryGroup LineShadowGeometryGroup { get; set; }
        internal Brush LineColor { get; set; }
        internal Double LineThickness { get; set; }
        internal Boolean Lighting { get; set; }
        internal DoubleCollection LineStyle { get; set; }
        internal Boolean ShadowEnabled { get; set; }
    }

    /// <summary>
    /// Visifire.Charts.LineChart class
    /// </summary>
    internal class LineChart
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
        /// Returns marker for DataPoint
        /// </summary>
        /// <param name="chart">Chart</param>
        /// <param name="position">Marker position</param>
        /// <param name="dataPoint">DataPoint</param>
        /// <param name="isPositive">Whether YValue is positive or negative</param>
        /// <returns>Marker</returns>
        private static Marker GetMarkerForDataPoint(Chart chart, Double position, DataPoint dataPoint, Boolean isPositive)
        {
            Size markerSize = new Size((Double)dataPoint.MarkerSize, (Double)dataPoint.MarkerSize);
            String labelText = (Boolean)dataPoint.LabelEnabled ? dataPoint.TextParser(dataPoint.LabelText) : "";
            Boolean markerBevel = false;
            dataPoint.Marker = new Marker((MarkerTypes)dataPoint.MarkerType, (Double)dataPoint.MarkerScale, markerSize, markerBevel, dataPoint.MarkerColor, labelText);

            ApplyMarkerProperties(dataPoint, markerSize);

            if (true && !String.IsNullOrEmpty(labelText))
            {
                dataPoint.Marker.CreateVisual();

                dataPoint.Marker.TextAlignmentX = AlignmentX.Center;
                if (isPositive)
                {
                    if (position < dataPoint.Marker.MarkerActualSize.Height || dataPoint.LabelStyle == LabelStyles.Inside)
                        dataPoint.Marker.TextAlignmentY = AlignmentY.Bottom;
                    else
                        dataPoint.Marker.TextAlignmentY = AlignmentY.Top;
                }
                else
                    if (position + dataPoint.Marker.MarkerActualSize.Height > chart.PlotArea.BorderElement.Height || dataPoint.LabelStyle == LabelStyles.Inside)
                        dataPoint.Marker.TextAlignmentY = AlignmentY.Top;
                    else
                        dataPoint.Marker.TextAlignmentY = AlignmentY.Bottom;
            }

            dataPoint.Marker.Control = chart;

            dataPoint.Marker.CreateVisual();

            dataPoint.Marker.Visual.Opacity = dataPoint.Opacity * dataPoint.Parent.Opacity;

            ApplyDefaultInteractivityForMarker(dataPoint);

            return dataPoint.Marker;
        }
        
        /// <summary>
        /// Apply default interactivity for Marker
        /// </summary>
        /// <param name="dataPoint">DataPoint</param>
        private static void ApplyDefaultInteractivityForMarker(DataPoint dataPoint)
        {
            if ((Boolean)dataPoint.MarkerEnabled)
            {
                dataPoint.Marker.MarkerShape.MouseEnter += delegate(object sender, MouseEventArgs e)
                {
                    Shape shape = sender as Shape;
                    shape.Stroke = new SolidColorBrush(Colors.Red);
                    shape.StrokeThickness = dataPoint.Marker.BorderThickness;
                };

                dataPoint.Marker.MarkerShape.MouseLeave += delegate(object sender, MouseEventArgs e)
                {
                    Shape shape = sender as Shape;
                    shape.Stroke = dataPoint.Marker.BorderColor;
                    shape.StrokeThickness = dataPoint.Marker.BorderThickness;
                };
            }
            else
            {
                Brush tarnsparentColor = new SolidColorBrush(Colors.Transparent);
                dataPoint.Marker.MarkerShape.Fill = tarnsparentColor;
                dataPoint.Marker.MarkerShape.Stroke = tarnsparentColor;

                if (dataPoint.Marker.MarkerShadow != null)
                {
                    dataPoint.Marker.MarkerShadow.Fill = tarnsparentColor;
                    dataPoint.Marker.MarkerShadow.Stroke = tarnsparentColor;
                }

                if (dataPoint.Marker.BevelLayer != null)
                    dataPoint.Marker.BevelLayer.Visibility = Visibility.Collapsed;
            }
        }
        
        /// <summary>
        /// Create line in 2D and place inside a canvas
        /// </summary>
        /// <param name="lineParams">Line parameters</param>
        /// <param name="line">line path reference</param>
        /// <param name="lineShadow">line shadow path reference</param>
        /// <returns>Canvas</returns>
        private static Canvas GetLine2D(LineChartShapeParams lineParams, out Path line, out Path lineShadow)
        {
            Canvas visual = new Canvas();
            line = new Path();

            line.Stroke = lineParams.Lighting ? Graphics.GetLightingEnabledBrush(lineParams.LineColor, "Linear", new Double[] { 0.65, 0.55 }) : lineParams.LineColor;
            line.StrokeThickness = lineParams.LineThickness;
            line.StrokeDashArray = lineParams.LineStyle;
            line.Data = lineParams.LineGeometryGroup;

            if (lineParams.ShadowEnabled)
            {
                lineShadow = new Path();
                lineShadow.Stroke = new SolidColorBrush(Colors.LightGray);
                lineShadow.StrokeThickness = lineParams.LineThickness;
                lineShadow.Opacity = 0.5;
                lineShadow.Data = lineParams.LineShadowGeometryGroup;
                TranslateTransform tt = new TranslateTransform() { X = 2, Y = 2 };
                lineShadow.RenderTransform = tt;

                visual.Children.Add(lineShadow);
            }
            else
                lineShadow = null;

            visual.Children.Add(line);

            return visual;
        }

        /// <summary>
        /// Apply marker properties
        /// </summary>
        /// <param name="dataPoint">DataPoint</param>
        /// <param name="markerSize">Marker size</param>
        private static void ApplyMarkerProperties(DataPoint dataPoint, Size markerSize)
        {
            dataPoint.Marker.MarkerSize = markerSize;
            dataPoint.Marker.BorderColor = dataPoint.MarkerBorderColor;
            dataPoint.Marker.BorderThickness = ((Thickness)dataPoint.MarkerBorderThickness).Left;
            dataPoint.Marker.FontColor = Chart.CalculateDataPointLabelFontColor(dataPoint.Chart as Chart, dataPoint, dataPoint.LabelFontColor, LabelStyles.OutSide);
            dataPoint.Marker.FontFamily = dataPoint.LabelFontFamily;
            dataPoint.Marker.FontSize = (Double)dataPoint.LabelFontSize;
            dataPoint.Marker.FontStyle = (FontStyle)dataPoint.LabelFontStyle;
            dataPoint.Marker.FontWeight = (FontWeight)dataPoint.LabelFontWeight;
            dataPoint.Marker.TextBackground = dataPoint.LabelBackground;
            dataPoint.Marker.MarkerFillColor = dataPoint.MarkerColor;
        }

        /// <summary>
        /// Apply animation for line chart
        /// </summary>
        /// <param name="canvas">Line chart canvas</param>
        /// <param name="storyboard">Storyboard</param>
        /// <param name="isLineCanvas">Whether canvas is line canvas</param>
        /// <returns>Storyboard</returns>
        private static Storyboard ApplyLineChartAnimation(Panel canvas, Storyboard storyboard, Boolean isLineCanvas)
        {
            LinearGradientBrush opacityMaskBrush = new LinearGradientBrush() { StartPoint = new Point(0, 0.5), EndPoint = new Point(1, 0.5) };

            // Create gradients for opacity mask animation
            GradientStop GradStop1 = new GradientStop() { Color = Colors.White, Offset = 0 };
            GradientStop GradStop2 = new GradientStop() { Color = Colors.White, Offset = 0 };
            GradientStop GradStop3 = new GradientStop() { Color = Colors.Transparent, Offset = 0.01 };
            GradientStop GradStop4 = new GradientStop() { Color = Colors.Transparent, Offset = 1 };

            // Add gradients to gradient stop list
            opacityMaskBrush.GradientStops.Add(GradStop1);
            opacityMaskBrush.GradientStops.Add(GradStop2);
            opacityMaskBrush.GradientStops.Add(GradStop3);
            opacityMaskBrush.GradientStops.Add(GradStop4);

            canvas.OpacityMask = opacityMaskBrush;

            double beginTime = (isLineCanvas) ? 0.25 + 0.5 : 0.5;

            DoubleCollection values = Graphics.GenerateDoubleCollection(0, 1);
            DoubleCollection timeFrames = Graphics.GenerateDoubleCollection(0, 1);
            List<KeySpline> splines = AnimationHelper.GenerateKeySplineList(new Point(0, 0), new Point(1, 1), new Point(0, 0), new Point(1, 1));

            storyboard.Children.Add(AnimationHelper.CreateDoubleAnimation(CurrentDataSeries, GradStop2, "(GradientStop.Offset)", beginTime, timeFrames, values, splines));

            values = Graphics.GenerateDoubleCollection(0.01, 1);
            timeFrames = Graphics.GenerateDoubleCollection(0, 1);
            splines = AnimationHelper.GenerateKeySplineList(new Point(0, 0), new Point(1, 1), new Point(0, 0), new Point(1, 1));

            storyboard.Children.Add(AnimationHelper.CreateDoubleAnimation(CurrentDataSeries, GradStop3, "(GradientStop.Offset)", beginTime, timeFrames, values, splines));

            return storyboard;
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Returns the visual object for line chart 
        /// </summary>
        /// <param name="width">PlotArea width</param>
        /// <param name="height">PlotArea height</param>
        /// <param name="plotDetails">PlotDetails</param>
        /// <param name="seriesList">List of line series</param>
        /// <param name="chart">Chart</param>
        /// <param name="plankDepth">PlankDepth</param>
        /// <param name="animationEnabled">Whether animation is enabled for chart</param>
        /// <returns>Canvas</returns>
        internal static Canvas GetVisualObjectForLineChart(Double width, Double height, PlotDetails plotDetails, List<DataSeries> seriesList, Chart chart, Double plankDepth, bool animationEnabled)
        {
            if (Double.IsNaN(width) || Double.IsNaN(height) || width <= 0 || height <= 0) return null;

            Canvas visual = new Canvas() { Width = width, Height = height };        // Canvas for line chart
            Canvas labelCanvas = new Canvas() { Width = width, Height = height };   // Canvas for placing labels

            Double depth3d = plankDepth / (plotDetails.Layer3DCount == 0 ? 1 : plotDetails.Layer3DCount) * (chart.View3D ? 1 : 0);
            Double visualOffset = depth3d * (plotDetails.SeriesDrawingIndex[seriesList[0]] + 1 - (plotDetails.Layer3DCount == 0 ? 0 : 1));

            // Set visual canvas position
            visual.SetValue(Canvas.TopProperty, visualOffset);
            visual.SetValue(Canvas.LeftProperty, -visualOffset);

            Double xPosition, yPosition;

            foreach (DataSeries series in seriesList)
            {
                if ((Boolean)series.Enabled == false)
                    continue;

                PlotGroup plotGroup = series.PlotGroup;
                LineChartShapeParams lineParams = new LineChartShapeParams();

                #region Set LineParms

                lineParams.Points = new PointCollection();
                lineParams.LineGeometryGroup = new GeometryGroup();
                lineParams.LineThickness = (Double)series.LineThickness;
                lineParams.LineColor = series.Color;
                lineParams.LineStyle = ExtendedGraphics.GetDashArray(series.LineStyle);
                lineParams.Lighting = (Boolean)series.LightingEnabled;
                lineParams.ShadowEnabled = series.ShadowEnabled;

                if (series.ShadowEnabled)
                    lineParams.LineShadowGeometryGroup = new GeometryGroup();

                #endregion

                series.VisualParams = lineParams;

                Point startPoint = new Point(), endPoint = new Point();
                Boolean IsStartPoint = true;

                foreach (DataPoint dataPoint in series.InternalDataPoints)
                {
                    if (dataPoint.Enabled == false)
                        continue;

                    if (Double.IsNaN(dataPoint.InternalYValue))
                    {
                        xPosition = Double.NaN;
                        yPosition = Double.NaN;
                        IsStartPoint = true;
                    }
                    else
                    {
                        xPosition = Graphics.ValueToPixelPosition(0, width, (Double)plotGroup.AxisX.InternalAxisMinimum, (Double)plotGroup.AxisX.InternalAxisMaximum, dataPoint.InternalXValue);
                        yPosition = Graphics.ValueToPixelPosition(height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, dataPoint.InternalYValue);

                        // Create Marker
                        Marker marker = GetMarkerForDataPoint(chart, yPosition, dataPoint, dataPoint.InternalYValue > 0);
                        marker.AddToParent(labelCanvas, xPosition, yPosition, new Point(0.5, 0.5));

                        #region Generate GeometryGroup for line and line shadow

                        if (IsStartPoint)
                            startPoint = new Point(xPosition, yPosition);
                        else
                            endPoint = new Point(xPosition, yPosition);

                        if (!IsStartPoint)
                        {
                            lineParams.LineGeometryGroup.Children.Add(new LineGeometry() { StartPoint = startPoint, EndPoint = endPoint });

                            if (lineParams.ShadowEnabled)
                                lineParams.LineShadowGeometryGroup.Children.Add(new LineGeometry() { StartPoint = startPoint, EndPoint = endPoint });

                            startPoint = endPoint;
                            IsStartPoint = false;
                        }
                        else
                            IsStartPoint = !IsStartPoint;

                        #endregion Generate GeometryGroup for line and line shadow
                    }

                    lineParams.Points.Add(new Point(xPosition, yPosition));
                }

                series.Faces = new Faces();
                series.Faces.Parts = new List<FrameworkElement>();

                Path polyline, PolylineShadow;
                Canvas line2dCanvas = GetLine2D(lineParams, out polyline, out PolylineShadow);

                series.Faces.Parts.Add(polyline);
                visual.Children.Add(line2dCanvas);

                series.Faces.Visual = line2dCanvas;
                series.Faces.LabelCanvas = labelCanvas;

                // Apply animation
                if (animationEnabled)
                {
                    if (series.Storyboard == null)
                        series.Storyboard = new Storyboard();

                    CurrentDataSeries = series;

                    // Apply animation to the lines
                    series.Storyboard = ApplyLineChartAnimation(line2dCanvas, series.Storyboard, true);
                }
            }

            visual.Children.Add(labelCanvas);

            // If animation is not enabled or if there are no series in the serieslist the dont apply animation
            if (animationEnabled && seriesList.Count > 0)
            {
                // Apply animation to the label canvas
                CurrentDataSeries = seriesList[0];

                if (CurrentDataSeries.Storyboard == null)
                    CurrentDataSeries.Storyboard = new Storyboard();

                CurrentDataSeries.Storyboard = ApplyLineChartAnimation(labelCanvas, CurrentDataSeries.Storyboard, false);
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
