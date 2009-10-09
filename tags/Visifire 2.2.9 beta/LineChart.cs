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
using System.Linq;

using Visifire.Commons;

namespace Visifire.Charts
{   
    /// <summary>
    /// Visifire.Charts.LineChartShapeParams class
    /// </summary>
    internal class LineChartShapeParams
    {
        internal PointCollection Points { get; set; }
        internal PointCollection ShadowPoints { get; set; }
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

            //dataPoint.Marker.ShadowEnabled =(Boolean) dataPoint.ShadowEnabled;

            ApplyMarkerProperties(dataPoint, markerSize);

            if (true && !String.IsNullOrEmpty(labelText))
            {
                if (!Double.IsNaN(dataPoint.LabelAngle) && dataPoint.LabelAngle != 0)
                {
                    dataPoint.Marker.LabelAngle = dataPoint.LabelAngle;
                    dataPoint.Marker.TextOrientation = Orientation.Vertical;

                    if (isPositive)
                    {
                        dataPoint.Marker.TextAlignmentX = AlignmentX.Center;
                        dataPoint.Marker.TextAlignmentY = AlignmentY.Top;
                    }
                    else
                    {
                        dataPoint.Marker.TextAlignmentX = AlignmentX.Center;
                        dataPoint.Marker.TextAlignmentY = AlignmentY.Bottom;
                    }

                    dataPoint.Marker.LabelStyle = (LabelStyles)dataPoint.LabelStyle;
                }

                dataPoint.Marker.CreateVisual();

                if (Double.IsNaN(dataPoint.LabelAngle) || dataPoint.LabelAngle == 0)
                {
                    dataPoint.Marker.TextAlignmentX = AlignmentX.Center;
                    
                    if (isPositive)
                    {
                        if (dataPoint.LabelStyle == LabelStyles.OutSide && !dataPoint.IsLabelStyleSet && !dataPoint.Parent.IsLabelStyleSet)
                        {
                            if (position < dataPoint.Marker.MarkerActualSize.Height || dataPoint.LabelStyle == LabelStyles.Inside)
                                dataPoint.Marker.TextAlignmentY = AlignmentY.Bottom;
                            else
                                dataPoint.Marker.TextAlignmentY = AlignmentY.Top;
                        }
                        else if(dataPoint.LabelStyle == LabelStyles.OutSide)
                            dataPoint.Marker.TextAlignmentY = AlignmentY.Top;
                        else
                            dataPoint.Marker.TextAlignmentY = AlignmentY.Bottom;
                    }
                    else
                    {
                        if (dataPoint.LabelStyle == LabelStyles.OutSide && !dataPoint.IsLabelStyleSet && !dataPoint.Parent.IsLabelStyleSet)
                        {
                            if (position + dataPoint.Marker.MarkerActualSize.Height > chart.PlotArea.BorderElement.Height || dataPoint.LabelStyle == LabelStyles.Inside)
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

            dataPoint.Marker.Control = chart;

            dataPoint.Marker.Tag = new ElementData() { Element = dataPoint };

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
                if (!dataPoint.Parent.MovingMarkerEnabled)
                {
                    dataPoint.Marker.MarkerShape.MouseEnter += delegate(object sender, MouseEventArgs e)
                    {
                        if (!dataPoint.Selected)
                        {
                            Shape shape = sender as Shape;
                            shape.Stroke = new SolidColorBrush(Colors.Red);
                            shape.StrokeThickness = dataPoint.Marker.BorderThickness;
                        }
                    };

                    dataPoint.Marker.MarkerShape.MouseLeave += delegate(object sender, MouseEventArgs e)
                    {
                        if (!dataPoint.Selected)
                        {
                            Shape shape = sender as Shape;
                            shape.Stroke = dataPoint.Marker.BorderColor;
                            shape.StrokeThickness = dataPoint.Marker.BorderThickness;
                        }
                    };
                }
            }
            else
            {
                HideDataPointMarker(dataPoint);
            }
        }

        /// <summary>
        /// Hides a DataPoint Marker
        /// </summary>
        /// <param name="dataPoint"></param>
        private static void HideDataPointMarker(DataPoint dataPoint)
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

        /// <summary>
        /// Create line in 2D and place inside a canvas
        /// </summary>
        /// <param name="lineParams">Line parameters</param>
        /// <param name="line">line path reference</param>
        /// <param name="lineShadow">line shadow path reference</param>
        /// <returns>Canvas</returns>
        private static Canvas GetLine2D(DataSeries tagReference, LineChartShapeParams lineParams, out Path line, out Path lineShadow, List<PointCollection> pointCollectionList, List<PointCollection> shadowPointCollectionList)
        {
            Canvas visual = new Canvas();
            line = new Path() { Tag = new ElementData() { Element = tagReference } };
            line.StrokeLineJoin = PenLineJoin.Round;

            line.StrokeStartLineCap = PenLineCap.Round;
            line.StrokeEndLineCap = PenLineCap.Round;

            line.Stroke = lineParams.Lighting ? Graphics.GetLightingEnabledBrush(lineParams.LineColor, "Linear", new Double[] { 0.65, 0.55 }) : lineParams.LineColor;
            line.StrokeThickness = lineParams.LineThickness;
            line.StrokeDashArray = lineParams.LineStyle;

            line.Data = GetPathGeometry(pointCollectionList);

            if (lineParams.ShadowEnabled)
            {
                lineShadow = new Path() { Tag = new ElementData() { Element = tagReference } };
                lineShadow.Stroke = new SolidColorBrush(Colors.Gray);
                lineShadow.StrokeThickness = lineParams.LineThickness +0.24;
                lineShadow.Opacity = 0.5;
                lineShadow.StrokeLineJoin = PenLineJoin.Round;
                lineShadow.StrokeStartLineCap = PenLineCap.Round;
                lineShadow.StrokeEndLineCap = PenLineCap.Round;
                lineShadow.Data = GetPathGeometry(shadowPointCollectionList);
                TranslateTransform tt = new TranslateTransform() { X = 1.26, Y = 1.26 };
                lineShadow.RenderTransform = tt;

                visual.Children.Add(lineShadow);

                //System.Windows.Media.Effects.PixelShader e1 = new System.Windows.Media.Effects.PixelShader();

                //e1.UriSource = System.Windows.Media.Effects.ShaderEffect.ImplicitInput;
                ////e1.BlurRadius = lineParams.LineThickness + 1;
                //////e1.Direction = 145;
                ////e1.ShadowDepth = 2;
                //line.Effect = System.Windows.Media.Effects.ShaderEffect.ImplicitInput;
            }
            else
                lineShadow = null;

            visual.Children.Add(line);

            return visual;
        }

        /// <summary>
        /// Get PathGeometry for Line and Shadow
        /// </summary>
        /// <param name="pointCollectionList">List of points collection</param>
        /// <returns>Geometry</returns>
        private static Geometry GetPathGeometry(List<PointCollection> pointCollectionList)
        {
            GeometryGroup gg = new GeometryGroup();

            foreach (PointCollection pointCollection in pointCollectionList)
            {
                PathGeometry geometry = new PathGeometry();
                
                PathFigure pathFigure = new PathFigure();

                if (pointCollection.Count > 0)
                    pathFigure.StartPoint = new Point(pointCollection[0].X, pointCollection[0].Y);
                
                PolyLineSegment segment = new PolyLineSegment();
                
                segment.Points = pointCollection;
                pathFigure.Segments.Add(segment);

                geometry.Figures.Add(pathFigure);
                gg.Children.Add(geometry);
            }
            
            return gg;
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

            _listOfDataSeries = new List<DataSeries>();

            Boolean isMovingMarkerEnabled = false; // Whether moving marker is enabled for atleast one series

            foreach (DataSeries series in seriesList)
            {
                if ((Boolean)series.Enabled == false)
                    continue;
                
                _listOfDataSeries.Add(series);
                
                List<PointCollection> pointCollectionList = new List<PointCollection>();
                List<PointCollection> shadowPointCollectionList = new List<PointCollection>();

                PlotGroup plotGroup = series.PlotGroup;
                LineChartShapeParams lineParams = new LineChartShapeParams();

                series.Faces = new Faces();
                series.Faces.Parts = new List<FrameworkElement>();

                #region Set LineParms

                lineParams.Points = new PointCollection();
                lineParams.ShadowPoints = new PointCollection();
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

                Point variableStartPoint = new Point(), endPoint = new Point();
                Boolean IsStartPoint = true;

                //Polyline polyline, PolylineShadow;
                //Canvas line2dCanvas = new Canvas();
                //Canvas lineCanvas;

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
                        {
                            variableStartPoint = new Point(xPosition, yPosition);
                        }
                        else
                            endPoint = new Point(xPosition, yPosition);

                        if (!IsStartPoint)
                        {
                            //lineParams.LineGeometryGroup.Children.Add(new LineGeometry() { StartPoint = startPoint, EndPoint = endPoint });

                            //if (lineParams.ShadowEnabled)
                            //    lineParams.LineShadowGeometryGroup.Children.Add(new LineGeometry() { StartPoint = startPoint, EndPoint = endPoint });

                            variableStartPoint = endPoint;

                            IsStartPoint = false;
                        }
                        else
                        {
                            IsStartPoint = !IsStartPoint;

                            if (lineParams.Points.Count > 0)
                            {
                                pointCollectionList.Add(lineParams.Points);
                                shadowPointCollectionList.Add(lineParams.ShadowPoints);
                            }

                            lineParams.Points = new PointCollection();
                            lineParams.ShadowPoints = new PointCollection();
                        }

                        #endregion Generate GeometryGroup for line and line shadow

                        lineParams.Points.Add(new Point(xPosition, yPosition));

                        if (lineParams.ShadowEnabled)
                            lineParams.ShadowPoints.Add(new Point(xPosition, yPosition));
                    }
                }

                pointCollectionList.Add(lineParams.Points);
                shadowPointCollectionList.Add(lineParams.ShadowPoints);
                
                series.Faces = new Faces();
                series.Faces.Parts = new List<FrameworkElement>();

                Path polyline, PolylineShadow;
                Canvas line2dCanvas = GetLine2D(series, lineParams, out polyline, out PolylineShadow, pointCollectionList, shadowPointCollectionList);

                line2dCanvas.Width = width;
                line2dCanvas.Height = height;

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
                
                // Create Moving Marker
                if (series.MovingMarkerEnabled)
                {
                    Double movingMarkerSize = (Double)series.MarkerSize * (Double)series.MarkerScale * MOVING_MARKER_SCALE;
                    
                    if (movingMarkerSize < 6)
                        movingMarkerSize = 6;

                    Ellipse movingMarker = new Ellipse() { Visibility= Visibility.Collapsed, IsHitTestVisible = false, Height = movingMarkerSize, Width = movingMarkerSize, Fill = lineParams.LineColor };
                    
                    labelCanvas.Children.Add(movingMarker);
                    series._movingMarker = movingMarker;
                }
                else
                    series._movingMarker = null;

                isMovingMarkerEnabled = isMovingMarkerEnabled || series.MovingMarkerEnabled;
            }

            // Detach attached events
            chart.ChartArea.PlotAreaCanvas.MouseMove -= PlotAreaCanvas_MouseMove;
            chart.ChartArea.PlotAreaCanvas.MouseLeave -= PlotAreaCanvas_MouseLeave;
            chart.ChartArea.PlotAreaCanvas.MouseEnter -= PlotAreaCanvas_MouseEnter;

            if (isMovingMarkerEnabled)
            {   
                chart.ChartArea.PlotAreaCanvas.MouseMove += new MouseEventHandler(PlotAreaCanvas_MouseMove);
                chart.ChartArea.PlotAreaCanvas.MouseLeave += new MouseEventHandler(PlotAreaCanvas_MouseLeave);
                chart.ChartArea.PlotAreaCanvas.MouseEnter += new MouseEventHandler(PlotAreaCanvas_MouseEnter);
            }

            RectangleGeometry clipRectangle = new RectangleGeometry();
            clipRectangle.Rect = new Rect(-8, -chart.ChartArea.PLANK_DEPTH, width + 8 + chart.ChartArea.PLANK_OFFSET, height + chart.ChartArea.PLANK_DEPTH + 6);
            labelCanvas.Clip = clipRectangle;

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

        /// <summary>
        /// MouseEnter event handler for MouseEnter event over PlotAreaCanvas
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">MouseEventArgs</param>
        static void PlotAreaCanvas_MouseEnter(object sender, MouseEventArgs e)
        {
            _isMouseEnteredInPlotArea = true;
        }

        /// <summary>
        /// MouseLeave event handler for MouseLeave event over PlotAreaCanvas
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">MouseEventArgs</param>
        static void PlotAreaCanvas_MouseLeave(object sender, MouseEventArgs e)
        {   
            _isMouseEnteredInPlotArea = false;

            // Disable Moving marker for PlotArea Canvas
            foreach (DataSeries ds in _listOfDataSeries)
            {
                if (ds._movingMarker != null)
                {
                    ds._movingMarker.Visibility = Visibility.Collapsed;
                }
            }
        }

        /// <summary>
        /// MouseMove event handler for MouseMove event over PlotAreaCanvas
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">MouseEventArgs</param>
        static void PlotAreaCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            (sender as FrameworkElement).Dispatcher.BeginInvoke(new Action<object, MouseEventArgs>(MoveMovingMarker), sender, e);
        }

        /// <summary>
        /// Move the moving marker
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">MouseEventArgs</param>
        internal static void MoveMovingMarker(object sender, MouseEventArgs e)
        {   
            Double xPosition = e.GetPosition(sender as Canvas).X;

            if (!_isMouseEnteredInPlotArea)
                return;

            foreach (DataSeries ds in _listOfDataSeries)
            {
                if (!ds.MovingMarkerEnabled)
                    continue;

                if (ds._movingMarker != null)
                {
                    if (ds._movingMarker.Visibility == Visibility.Collapsed)
                        ds._movingMarker.Visibility = Visibility.Visible;

                    DataPoint nearestDataPoint = null;

                    foreach (DataPoint dp in ds.DataPoints)
                    {
                        if (dp.Marker != null)
                        {
                            dp._distance = Math.Abs(xPosition - dp.Marker.Position.X);

                            if (nearestDataPoint == null)
                            {
                                nearestDataPoint = dp;
                                continue;
                            }

                            if (dp._distance < nearestDataPoint._distance)
                                nearestDataPoint = dp;
                        }
                    }

                    // DataPoint nearestDataPoint = (from dp in ds.DataPoints orderby Math.Abs(xPosition - dp.Marker.Position.X) select dp).First();
                    
                    Ellipse movingMarker = ds._movingMarker;

                    if (nearestDataPoint.Selected)
                    {
                        SelectMovingMarker(nearestDataPoint);
                    }
                    else
                    {
                        movingMarker.Fill = nearestDataPoint.Parent.Color;
                        
                        Double movingMarkerSize =(Double)nearestDataPoint.Parent.MarkerSize * (Double)nearestDataPoint.Parent.MarkerScale * MOVING_MARKER_SCALE;
                        
                        if(movingMarkerSize < 6)
                            movingMarkerSize = 6;

                        movingMarker.Height = movingMarkerSize;
                        movingMarker.Width = movingMarker.Height;
                        movingMarker.StrokeThickness = 0;

                        movingMarker.SetValue(Canvas.LeftProperty, nearestDataPoint.Marker.Position.X - movingMarker.Width / 2);
                        movingMarker.SetValue(Canvas.TopProperty, nearestDataPoint.Marker.Position.Y - movingMarker.Height / 2);
                     }
                }
            }
        }

        /// <summary>
        /// Apply Selected effect on Moving Markers
        /// </summary>
        /// <param name="dataPoint"></param>
        internal static void SelectMovingMarker(DataPoint dataPoint)
        {   
            Ellipse movingMarker = dataPoint.Parent._movingMarker;

            if (movingMarker != null)
            {   
                movingMarker.Stroke = dataPoint.Marker.MarkerShape.Stroke;
                movingMarker.StrokeThickness = 2;
                //_movingMarker.Fill = nearestDataPoint.Marker.MarkerShape.Fill;

                movingMarker.Width = dataPoint.Marker.MarkerShape.Width + 2;
                movingMarker.Height = movingMarker.Width;

                movingMarker.SetValue(Canvas.LeftProperty, dataPoint.Marker.Position.X - movingMarker.Width / 2);
                movingMarker.SetValue(Canvas.TopProperty, dataPoint.Marker.Position.Y - movingMarker.Height / 2);
 
            }
        }

        #endregion

        #region Internal Events And Delegates

        #endregion

        #region Data

        private static Boolean _isMouseEnteredInPlotArea = false;

        private static List<DataSeries> _listOfDataSeries;

        private static Double MOVING_MARKER_SCALE = 1.1;

        #endregion
    }
}
