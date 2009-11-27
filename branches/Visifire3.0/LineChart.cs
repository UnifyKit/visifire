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
        internal List<DataPoint> Points { get; set; }
        internal List<DataPoint> ShadowPoints { get; set; }
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
        internal static Marker GetMarkerForDataPoint(Boolean reCreate, Chart chart, Double yPosition, DataPoint dataPoint, Boolean isPositive)
        {   
            String labelText = (Boolean)dataPoint.LabelEnabled ? dataPoint.TextParser(dataPoint.LabelText) : "";
            Boolean markerBevel = false;

            if (reCreate)
            {
                Marker marker = dataPoint.Marker;

                if(marker != null && marker.Visual != null)
                {   
                    Panel parent = marker.Visual.Parent as Panel;

                    if (parent != null)
                        parent.Children.Remove(marker.Visual);

                    marker.MarkerType = (MarkerTypes)dataPoint.MarkerType;
                    marker.ScaleFactor = (Double)dataPoint.MarkerScale;
                    marker.MarkerSize = new Size((Double)dataPoint.MarkerSize, (Double)dataPoint.MarkerSize);
                    marker.Bevel = false;
                    marker.MarkerFillColor = dataPoint.MarkerColor;
                    marker.Text = labelText;
                }
                else
                {
                    dataPoint.Marker = new Marker((MarkerTypes)dataPoint.MarkerType,
                        (Double)dataPoint.MarkerScale,
                        new Size((Double)dataPoint.MarkerSize, (Double)dataPoint.MarkerSize),
                        markerBevel,
                        dataPoint.MarkerColor,
                        labelText);
                }
            }
            else
            {   
                Marker marker = dataPoint.Marker;

                marker.MarkerType = (MarkerTypes)dataPoint.MarkerType;
                marker.ScaleFactor = (Double)dataPoint.MarkerScale;
                marker.MarkerSize = new Size((Double)dataPoint.MarkerSize, (Double)dataPoint.MarkerSize);
                marker.Bevel = false;
                marker.MarkerFillColor = dataPoint.MarkerColor;
                marker.Text = labelText;
                marker.TextAlignmentX = AlignmentX.Center;
                marker.TextAlignmentY = AlignmentY.Center;
            }

            ApplyMarkerProperties(dataPoint);

            if (!String.IsNullOrEmpty(labelText))
            {
                dataPoint.Marker.FontColor = Chart.CalculateDataPointLabelFontColor(dataPoint.Chart as Chart, dataPoint, dataPoint.LabelFontColor, LabelStyles.OutSide);
                dataPoint.Marker.FontFamily = dataPoint.LabelFontFamily;
                dataPoint.Marker.FontSize = (Double)dataPoint.LabelFontSize;
                dataPoint.Marker.FontStyle = (FontStyle)dataPoint.LabelFontStyle;
                dataPoint.Marker.FontWeight = (FontWeight)dataPoint.LabelFontWeight;
                dataPoint.Marker.TextBackground = dataPoint.LabelBackground;

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
                            //if (position < dataPoint.Marker.MarkerActualSize.Height || dataPoint.LabelStyle == LabelStyles.Inside)                            
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

            dataPoint.Marker.Control = chart;

            dataPoint.Marker.Tag = new ElementData() { Element = dataPoint };

            dataPoint.Marker.CreateVisual();

            dataPoint.Marker.Visual.Opacity = dataPoint.Opacity * dataPoint.Parent.Opacity;

            ApplyDefaultInteractivityForMarker(dataPoint);

            //dataPoint.AttachEvent2DataPointVisualFaces(dataPoint);
            ObservableObject.AttachEvents2Visual(dataPoint, dataPoint, dataPoint.Marker.Visual);
            //dataPoint.AttachEvent2DataPointVisualFaces(dataPoint.Parent);
            ObservableObject.AttachEvents2Visual(dataPoint.Parent, dataPoint, dataPoint.Marker.Visual);
            dataPoint._parsedToolTipText = dataPoint.TextParser(dataPoint.ToolTipText);
            dataPoint.AttachToolTip(chart, dataPoint, dataPoint.Marker.Visual);
            dataPoint.AttachHref(chart, dataPoint.Marker.Visual, dataPoint.Href, (HrefTargets)dataPoint.HrefTarget);
            dataPoint.SetCursor2DataPointVisualFaces();
            return dataPoint.Marker;
        }

        /// <summary>
        /// Apply default interactivity for Marker
        /// </summary>
        /// <param name="dataPoint">DataPoint</param>
        internal static void ApplyDefaultInteractivityForMarker(DataPoint dataPoint)
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
        internal static void HideDataPointMarker(DataPoint dataPoint)
        {
            Brush tarnsparentColor = new SolidColorBrush(Colors.Transparent);
            dataPoint.Marker.MarkerShape.Fill = tarnsparentColor;
            dataPoint.Marker.MarkerShape.Stroke = tarnsparentColor;

            if (dataPoint.Marker.MarkerShadow != null)
            {   
                dataPoint.Marker.MarkerShadow.Visibility = Visibility.Collapsed;
            }

            if (dataPoint.Marker.BevelLayer != null)
                dataPoint.Marker.BevelLayer.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Hides a DataPoint Marker
        /// </summary>
        /// <param name="dataPoint"></param>
        internal static void ShowDataPointMarker(DataPoint dataPoint)
        {   
            if (dataPoint.MarkerColor != null)
                dataPoint.Marker.MarkerShape.Fill = dataPoint.MarkerColor;

            if (dataPoint.MarkerBorderColor != null)
                dataPoint.Marker.MarkerShape.Stroke = dataPoint.MarkerBorderColor;
            else
                dataPoint.Marker.MarkerShape.Stroke = dataPoint.Color;

            if (dataPoint.Marker.MarkerShadow != null)
                dataPoint.Marker.MarkerShadow.Visibility = Visibility.Visible;

            if (dataPoint.Marker.BevelLayer != null)
                dataPoint.Marker.BevelLayer.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Create line in 2D and place inside a canvas
        /// </summary>
        /// <param name="lineParams">Line parameters</param>
        /// <param name="line">line path reference</param>
        /// <param name="lineShadow">line shadow path reference</param>
        /// <returns>Canvas</returns>
        private static Canvas GetLine2D(DataSeries tagReference, LineChartShapeParams lineParams, out Path line, out Path lineShadow, List<List<DataPoint>> pointCollectionList, List<List<DataPoint>> shadowPointCollectionList)
        {   
            Canvas visual = new Canvas();
            line = new Path() { Tag = new ElementData() { Element = tagReference } };
            line.StrokeLineJoin = PenLineJoin.Round;

            line.StrokeStartLineCap = PenLineCap.Round;
            line.StrokeEndLineCap = PenLineCap.Round;

            line.Stroke = lineParams.Lighting ? Graphics.GetLightingEnabledBrush(lineParams.LineColor, "Linear", new Double[] { 0.65, 0.55 }) : lineParams.LineColor;
            line.StrokeThickness = lineParams.LineThickness;
            line.StrokeDashArray = lineParams.LineStyle;

            line.Data = GetPathGeometry(null, pointCollectionList, false);

            if (lineParams.ShadowEnabled)
            {   
                lineShadow = new Path() { IsHitTestVisible = false };
                lineShadow.Stroke = Graphics.GetLightingEnabledBrush( new SolidColorBrush(Colors.LightGray), "Linear", new Double[] { 0.65, 0.55 });
                lineShadow.StrokeStartLineCap = PenLineCap.Round;
                lineShadow.StrokeEndLineCap = PenLineCap.Round;
                lineShadow.StrokeLineJoin = PenLineJoin.Round;
                lineShadow.StrokeThickness = lineParams.LineThickness;
                lineShadow.Opacity = 0.5;

                if (lineParams.ShadowEnabled)
                    lineShadow.Data = GetPathGeometry(null, shadowPointCollectionList, true);

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
        /// Get PathGeometry for Line and Shadow
        /// </summary>
        /// <param name="pointCollectionList">List of points collection</param>
        /// <returns>Geometry</returns>

        /// <summary>
        /// Get PathGeometry for Line and Shadow
        /// </summary>
        /// <param name="dataPointCollectionList">List of Segments. And EachSegments contains a list of DataPoints</param>
        /// <returns></returns>
        private static Geometry GetPathGeometry(GeometryGroup oldData, List<List<DataPoint>> dataPointCollectionList, Boolean isShadow)
        {   
            GeometryGroup gg;

            if (oldData != null)
            {
                gg = oldData;
                gg.Children.Clear();
            }
            else
            {
                gg = new GeometryGroup();
            }

            foreach (List<DataPoint> pointCollection in dataPointCollectionList)
            {
                PathGeometry geometry = new PathGeometry();

                PathFigure pathFigure = new PathFigure();

                if (pointCollection.Count > 0)
                {
                    pathFigure.StartPoint = pointCollection[0]._visualPosition;

                    Faces faces = new Faces();

                    //Add LineSegment
                    faces.Parts.Add(null);

                    // Add PathFigure
                    faces.Parts.Add(pathFigure);

                    if (isShadow)
                        pointCollection[0].ShadowFaces = faces;
                    else
                        pointCollection[0].Faces = faces;

                    /*
                     * PolyLineSegment segment = new PolyLineSegment();
                       segment.Points = GeneratePointCollection(segment, pointCollection, createFaces);
                       pathFigure.Segments.Add(segment);
                     */

                    for (int i = 1; i < pointCollection.Count; i++)
                    {
                        LineSegment segment = new LineSegment();

                        segment.Point = pointCollection[i]._visualPosition;

                        faces = new Faces();

                        //Add LineSegment
                        faces.Parts.Add(segment);

                        // Add PathFigure
                        faces.Parts.Add(pathFigure);

                        if (isShadow)
                            pointCollection[i].ShadowFaces = faces;
                        else
                            pointCollection[i].Faces = faces;

                        pathFigure.Segments.Add(segment);
                    }
                }

                geometry.Figures.Add(pathFigure);
                gg.Children.Add(geometry);
            }

            return gg;
        }

        public static void Update(ObservableObject sender, VcProperties property, object newValue, Boolean isAxisChanged)
        {   
            Boolean isDataPoint = sender.GetType().Equals(typeof(DataPoint));
            
            if (isDataPoint)
                UpdateDataPoint(sender as DataPoint, property, newValue);
            else
                UpdateDataSeries(sender as DataSeries, property, newValue);
        }

        //internal static void Update(Chart chart, RenderAs currentRenderAs, List<DataSeries> selectedDataSeries4Rendering, VcProperties property, object newValue)
        //{   
        //    foreach(
        //}

        //internal static void Update(Chart chart, RenderAs currentRenderAs, List<DataSeries> selectedDataSeries4Rendering, VcProperties property, object newValue)
        //{   
        //    Boolean is3D = chart.View3D;
        //    ChartArea chartArea = chart.ChartArea;
        //    Canvas ChartVisualCanvas = chart.ChartArea.ChartVisualCanvas;

        //    // Double width = chart.ChartArea.ChartVisualCanvas.Width;
        //    // Double height = chart.ChartArea.ChartVisualCanvas.Height;

        //    Panel preExistingPanel = null;
        //    Dictionary<RenderAs, Panel> RenderedCanvasList = chart.ChartArea.RenderedCanvasList;

        //    if (chartArea.RenderedCanvasList.ContainsKey(currentRenderAs))
        //    {   
        //        preExistingPanel = RenderedCanvasList[currentRenderAs];
        //    }

        //    Panel renderedChart = chartArea.RenderSeriesFromList(preExistingPanel, selectedDataSeries4Rendering);

        //    if (preExistingPanel == null)
        //    {
        //        chartArea.RenderedCanvasList.Add(currentRenderAs, renderedChart);
        //        ChartVisualCanvas.Children.Add(renderedChart);
        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj">Object may be DataSeries or DataPoint</param>
        /// <param name="property"></param>
        /// <param name="newValue"></param>
        private static void UpdateDataSeries(ObservableObject obj, VcProperties property, object newValue)
        {
            DataPoint dataPoint = null;
            DataSeries dataSeries = obj as DataSeries;
            Boolean isDataPoint = false;
            
            if (dataSeries == null)
            {
                isDataPoint = true;
                dataPoint = obj as DataPoint;
                dataSeries = dataPoint.Parent;
            }

            Chart chart = dataSeries.Chart as Chart;
            PlotGroup plotGroup = dataSeries.PlotGroup;
            Canvas line2dCanvas = null;
            Canvas label2dCanvas = null;
            Path linePath = null;
            Path lineShadowPath = null;

            if (dataSeries.Faces != null)
            {
                if (dataSeries.Faces.Parts.Count > 0)
                {
                    linePath = dataSeries.Faces.Parts[0] as Path;

                    if (dataSeries.Faces.Parts.Count > 1)
                        lineShadowPath = dataSeries.Faces.Parts[1] as Path;
                }

                line2dCanvas = dataSeries.Faces.Visual as Canvas;
                label2dCanvas = dataSeries.Faces.LabelCanvas as Canvas;
            }
            else
                return;

            Double height = chart.ChartArea.ChartVisualCanvas.Height;
            Double width = chart.ChartArea.ChartVisualCanvas.Width;

            switch (property)
            {   
                case VcProperties.Color:
                    if (linePath != null)
                    {
                        Brush lineColorValue = (newValue != null) ? newValue as Brush : dataSeries.Color;

                        linePath.Stroke = ((Boolean)dataSeries.LightingEnabled) ? Graphics.GetLightingEnabledBrush(lineColorValue, "Linear", new Double[] { 0.65, 0.55 }) : lineColorValue; //dataPoint.Color;
                    }
                    break;
                case VcProperties.LightingEnabled:
                    if (linePath != null)
                        linePath.Stroke = ((Boolean)newValue) ? Graphics.GetLightingEnabledBrush(dataSeries.Color, "Linear", new Double[] { 0.65, 0.55 }) : dataSeries.Color;
                        
                    break;

                case VcProperties.Opacity:
                    if (linePath != null)
                        linePath.Opacity = dataSeries.Opacity;
                    break;
                case VcProperties.LineStyle:
                case VcProperties.LineThickness:

                    if (lineShadowPath != null)
                        lineShadowPath.StrokeThickness = (Double)dataSeries.LineThickness;
                    if (linePath != null)
                        linePath.StrokeThickness = (Double)dataSeries.LineThickness;

                    if (lineShadowPath != null)
                        lineShadowPath.StrokeDashArray = ExtendedGraphics.GetDashArray(dataSeries.LineStyle);
                    if (linePath != null)
                        linePath.StrokeDashArray = ExtendedGraphics.GetDashArray(dataSeries.LineStyle);

                    break;
                case VcProperties.Enabled:
                    
                    if (!isDataPoint && line2dCanvas != null)
                    {
                        if ((Boolean)newValue == false)
                        {
                            line2dCanvas.Visibility = Visibility.Collapsed;
                            label2dCanvas.Visibility = Visibility.Collapsed;
                        }
                        else
                        {
                            line2dCanvas.Visibility = Visibility.Visible;
                            label2dCanvas.Visibility = Visibility.Visible;
                        }

                        chart._toolTip.Hide();

                        break;
                    }

                    goto NEXT;

                case VcProperties.ShadowEnabled:
                case VcProperties.DataPoints:
                case VcProperties.YValue:
                case VcProperties.YValues:
                case VcProperties.XValue:
                NEXT:
                    Axis axisX = plotGroup.AxisX;
                    Axis axisY = plotGroup.AxisY;

                    height = chart.ChartArea.ChartVisualCanvas.Height;
                    width = chart.ChartArea.ChartVisualCanvas.Width;

                    //line2dCanvas.OpacityMask = new SolidColorBrush(Colors.Transparent);
                    //label2dCanvas.OpacityMask = new SolidColorBrush(Colors.Transparent);

                    (dataSeries.Faces.Visual as Canvas).Width = width;
                    (dataSeries.Faces.Visual as Canvas).Height = height;
                    (dataSeries.Faces.LabelCanvas as Canvas).Width = width;
                    (dataSeries.Faces.LabelCanvas as Canvas).Height = height;
                    
                    List<DataPoint> pc = new List<DataPoint>();
                    List<List<DataPoint>> pointCollectionList = new List<List<DataPoint>>();

                    pointCollectionList.Add(pc);
                    // List<DataPoint> enabledDataPoints = (from dp in dataSeries.InternalDataPoints where (Boolean) dp.Enabled == true select dp).ToList();
                    foreach (DataPoint dp in dataSeries.InternalDataPoints)
                    {   
                        if (dp.Enabled == false)
                        {   
                            if (dp.Marker != null && dp.Marker.Visual != null)
                                dp.Marker.Visual.Visibility = Visibility.Collapsed;

                            chart._toolTip.Hide();
                            continue;
                        }

                        if (Double.IsNaN(dp.YValue))
                        {
                            pc = new List<DataPoint>();
                            pointCollectionList.Add(pc);
                            continue;
                        }

                        Double x = Graphics.ValueToPixelPosition(0, width, axisX.InternalAxisMinimum, axisX.InternalAxisMaximum, dp.InternalXValue);
                        Double y = Graphics.ValueToPixelPosition(height, 0, axisY.InternalAxisMinimum, axisY.InternalAxisMaximum, dp.InternalYValue);

                        Point newMarkerPosition;
                        dp._visualPosition = new Point(x, y);

                        if (dp.Marker != null && dp.Marker.Visual != null)
                        {
                            newMarkerPosition = dp.Marker.CalculateActualPosition(x, y, new Point(0.5, 0.5));

                            dp.Marker.Visual.Visibility = Visibility.Visible;
                            dp.Marker.Visual.SetValue(Canvas.TopProperty, newMarkerPosition.Y);
                            dp.Marker.Visual.SetValue(Canvas.LeftProperty, newMarkerPosition.X);
                        }
                        else
                        {
                            Marker marker = LineChart.GetMarkerForDataPoint(true, chart, y, dp, dp.InternalYValue > 0);
                            newMarkerPosition = marker.CalculateActualPosition(x, y, new Point(0.5, 0.5));

                            marker.AddToParent(dp.Parent.Faces.LabelCanvas, x, y, new Point(0.5, 0.5));
                            dp._parsedToolTipText = dp.TextParser(dp.ToolTipText);
                            dp.AttachToolTip(chart, dp, marker.Visual);
                        }

                        pc.Add(dp);
                    }

                    // gg.Children.Clear();
                    GeometryGroup gg = (dataSeries.Faces.Parts[0] as Path).Data as GeometryGroup;

                    // Apply new Data for Line
                    LineChart.GetPathGeometry(gg, pointCollectionList, false);

                    // Update GeometryGroup for shadow
                    if (dataSeries.Faces.Parts[1] != null)
                    {
                        if (dataSeries.ShadowEnabled)
                        {
                            (dataSeries.Faces.Parts[1] as Path).Visibility = Visibility.Visible;
                            
                            // gg.Children.Clear();
                            GeometryGroup ggShadow = (dataSeries.Faces.Parts[1] as Path).Data as GeometryGroup;

                            // Apply new Data for Line
                            LineChart.GetPathGeometry(ggShadow, pointCollectionList, true);
                        }
                        else
                            (dataSeries.Faces.Parts[1] as Path).Visibility = Visibility.Collapsed;
                    }

                    RectangleGeometry clipRectangle = new RectangleGeometry();

                    Double depth3d = chart.ChartArea.PLANK_DEPTH;

                    Double clipLeft = 0;
                    Double clipTop = -depth3d;
                    Double clipWidth = line2dCanvas.Width + depth3d;
                    Double clipHeight = line2dCanvas.Height + depth3d + chart.ChartArea.PLANK_THICKNESS + 6;

                    AreaChart.GetClipCoordinates(chart, ref clipLeft, ref clipTop, ref clipWidth, ref clipHeight, plotGroup.MinimumX, plotGroup.MaximumX);

                    clipRectangle.Rect = new Rect(clipLeft, clipTop, clipWidth, clipHeight);

                    (label2dCanvas.Parent as Canvas).Clip = clipRectangle;

                    clipRectangle = new RectangleGeometry();
                    clipRectangle.Rect = new Rect(0, -depth3d, line2dCanvas.Width + depth3d, line2dCanvas.Height + chart.ChartArea.PLANK_DEPTH);
                    (line2dCanvas.Parent as Canvas).Clip = clipRectangle;

                    break;
            }
        }
        
        private static void UpdateDataPoint(DataPoint dataPoint, VcProperties property, object newValue)
        {
            if (property != VcProperties.Enabled)
            {
                if (dataPoint.Parent.Enabled == false || (Boolean)dataPoint.Enabled == false)
                {
                    return;
                }
            }

            Chart chart = dataPoint.Chart as Chart;
            Marker marker = dataPoint.Marker;
            DataSeries dataSeries = dataPoint.Parent;
            PlotGroup plotGroup = dataSeries.PlotGroup;
            Double height = chart.ChartArea.ChartVisualCanvas.Height;
            Double width = chart.ChartArea.ChartVisualCanvas.Width;
            Double xPosition, yPosition;
            Canvas line2dLabelCanvas = null;

            if (dataSeries.Faces != null)
            {
                line2dLabelCanvas = dataSeries.Faces.LabelCanvas as Canvas;
                ColumnChart.UpdateParentVisualCanvasSize(chart, line2dLabelCanvas);
            }
            
            switch (property)
            {
                case VcProperties.Color:
                    if(marker != null)
                        marker.BorderColor = (dataPoint.GetValue(DataPoint.MarkerBorderColorProperty) as Brush == null) ? ((newValue != null) ? newValue as Brush : dataPoint.MarkerBorderColor) : dataPoint.MarkerBorderColor;
                    break;
                case VcProperties.Cursor:
                    dataPoint.SetCursor2DataPointVisualFaces();
                    break;

                case VcProperties.Href:
                case VcProperties.HrefTarget:
                    dataPoint.SetHref2DataPointVisualFaces();
                    break;

                case VcProperties.LabelBackground:
                    if (marker != null)
                        marker.TextBackground = dataPoint.LabelBackground;
                    break;

                case VcProperties.LabelEnabled:
                    //if (marker.LabelEnabled == false)
                        CreateMarkerAForLineDataPoint(dataPoint, width, height, ref line2dLabelCanvas, out xPosition, out yPosition);
                    //else
                    //    marker.LabelEnabled = (Boolean)dataPoint.LabelEnabled;
                    break;

                case VcProperties.LabelFontColor:
                    if (marker != null)
                        marker.FontColor = dataPoint.LabelFontColor;
                    break;

                case VcProperties.LabelFontFamily:
                    CreateMarkerAForLineDataPoint(dataPoint, width, height, ref line2dLabelCanvas, out xPosition, out yPosition);
                    // marker.FontFamily = dataPoint.LabelFontFamily;
                    break;

                case VcProperties.LabelFontStyle:
                    CreateMarkerAForLineDataPoint(dataPoint, width, height, ref line2dLabelCanvas, out xPosition, out yPosition);

                    //marker.FontStyle = (FontStyle) dataPoint.LabelFontStyle;
                    break;

                case VcProperties.LabelFontSize:
                    CreateMarkerAForLineDataPoint(dataPoint, width, height, ref line2dLabelCanvas, out xPosition, out yPosition);

                   // marker.FontSize = (Double) dataPoint.LabelFontSize;
                    break;

                case VcProperties.LabelFontWeight:
                    if (marker != null)
                        marker.FontWeight = (FontWeight) dataPoint.LabelFontWeight;
                    break;

                case VcProperties.LabelStyle:
                    CreateMarkerAForLineDataPoint(dataPoint, width, height, ref line2dLabelCanvas, out xPosition, out yPosition);
                    break;

                case VcProperties.LabelText:
                    CreateMarkerAForLineDataPoint(dataPoint, width, height, ref line2dLabelCanvas, out xPosition, out yPosition);
                    //marker.Text = dataPoint.TextParser(dataPoint.LabelText);
                    break;

                case VcProperties.LegendText:
                    chart.InvokeRender();
                    break;

                case VcProperties.LightingEnabled:
                    break;

                case VcProperties.MarkerBorderColor:
                    CreateMarkerAForLineDataPoint(dataPoint, width, height, ref line2dLabelCanvas, out xPosition, out yPosition);

                    //marker.BorderColor = dataPoint.MarkerBorderColor;
                    break;
                case VcProperties.MarkerBorderThickness:
                    CreateMarkerAForLineDataPoint(dataPoint, width, height, ref line2dLabelCanvas, out xPosition, out yPosition);
                    //marker.BorderThickness = dataPoint.MarkerBorderThickness.Value.Left;
                    break;

                case VcProperties.MarkerColor:
                    if (marker != null)
                        marker.MarkerFillColor = dataPoint.MarkerColor;
                    break;

                case VcProperties.MarkerEnabled:
                    CreateMarkerAForLineDataPoint(dataPoint, width, height, ref line2dLabelCanvas, out xPosition, out yPosition);

                    //if((Boolean)dataPoint.MarkerEnabled)
                    //    ShowDataPointMarker(dataPoint);
                    //else
                    //    HideDataPointMarker(dataPoint);
                    break;

                case VcProperties.MarkerScale:                   
                case VcProperties.MarkerSize:
                case VcProperties.MarkerType:
                case VcProperties.ShadowEnabled:
                    //Double y = Graphics.ValueToPixelPosition(plotGroup.AxisY.Height, 0, plotGroup.AxisY.InternalAxisMinimum, plotGroup.AxisY.InternalAxisMaximum, dataPoint.InternalYValue);
                    //LineChart.GetMarkerForDataPoint(true, chart, y, dataPoint, dataPoint.InternalYValue > 0);
                    CreateMarkerAForLineDataPoint(dataPoint, width, height, ref line2dLabelCanvas, out xPosition, out yPosition);

                    break;

                case VcProperties.Opacity:
                    if (marker != null)
                        marker.Visual.Opacity = dataPoint.Opacity * dataSeries.Opacity;
                    break;
                case  VcProperties.ShowInLegend:
                    chart.InvokeRender();
                    break;
                case VcProperties.ToolTipText:
                case VcProperties.XValueFormatString:
                case VcProperties.YValueFormatString:
                    dataPoint._parsedToolTipText = dataPoint.TextParser(dataPoint.ToolTipText);
                    CreateMarkerAForLineDataPoint(dataPoint, width, height, ref line2dLabelCanvas, out xPosition, out yPosition);
                    break;
                case VcProperties.XValueType:
                    chart.InvokeRender();
                    break;

                case VcProperties.Enabled:
                    if((Boolean)dataPoint.Parent.Enabled)
                        UpdateDataSeries(dataPoint, VcProperties.Enabled, newValue);
                    break;

                case VcProperties.XValue:

                    UpdateXAndYValue(dataPoint);
                    break;

                case VcProperties.YValue:
                    //if (dataPoint.Marker != null && dataPoint.Marker.Visual != null)
                    //{
                    //    Point oldMarkerPosition = new Point((Double)dataPoint.Marker.Visual.GetValue(Canvas.LeftProperty), (Double)dataPoint.Marker.Visual.GetValue(Canvas.TopProperty));
                    //    CreateMarkerAForLineDataPoint(dataPoint, width, height, ref line2dLabelCanvas, out xPosition, out yPosition);

                    //    dataPoint.Marker.Visual.SetValue(Canvas.LeftProperty, oldMarkerPosition.X);
                    //    dataPoint.Marker.Visual.SetValue(Canvas.TopProperty, oldMarkerPosition.Y);

                    //    if (dataPoint.Parent.SelectionEnabled && dataPoint.Selected)
                    //        dataPoint.Select(true);
                    //}
                    
                   //UpdateXAndYValue(dataPoint);
                   chart.Dispatcher.BeginInvoke(new Action<DataPoint>(UpdateXAndYValue), new object[]{dataPoint});
                   
                   
                   break;
            }
        }

        private static void UpdateXAndYValue(DataPoint dataPoint)
        {
            Boolean isAnimationEnabled = (dataPoint.Chart as Chart).AnimatedUpdate;

            if (!(Boolean)dataPoint.Enabled)
                return;

            Chart chart = dataPoint.Chart as Chart;
            DataSeries dataSeries = dataPoint.Parent;
            dataSeries._movingMarker.Visibility = Visibility.Collapsed;

            Axis axisX = dataSeries.PlotGroup.AxisX;
            Axis axisY = dataSeries.PlotGroup.AxisY;

            Marker dataPointMarker = dataPoint.Marker;
            Marker legendMarker = dataPoint.LegendMarker;
            
            Double x = Graphics.ValueToPixelPosition(0, chart.ChartArea.ChartVisualCanvas.Width, axisX.InternalAxisMinimum, axisX.InternalAxisMaximum, dataPoint.InternalXValue);
            Double y = Graphics.ValueToPixelPosition(chart.ChartArea.ChartVisualCanvas.Height, 0, axisY.InternalAxisMinimum, axisY.InternalAxisMaximum, dataPoint.InternalYValue);

            dataPoint._visualPosition = new Point(x, y);
            Point newMarkerPosition = new Point();
            
            if(dataPointMarker != null)
                newMarkerPosition = dataPointMarker.CalculateActualPosition(x, y, new Point(0.5, 0.5));
            
            if (!isAnimationEnabled)
                if (dataPointMarker != null && dataPointMarker.Visual != null)
                {
                    dataPointMarker.Visual.SetValue(Canvas.TopProperty, newMarkerPosition.Y);
                    dataPointMarker.Visual.SetValue(Canvas.LeftProperty, newMarkerPosition.X);
                }

            DependencyObject target, shadowTarget = null;    // Target object
            Point oldPoint = new Point();                    // Old Position

            // Collect reference of line geometry object
            LineSegment lineSeg = dataPoint.Faces.Parts[0] as LineSegment;
            PathFigure pathFigure = dataPoint.Faces.Parts[1] as PathFigure;

            LineSegment shadowLineSeg;
            PathFigure shadowPathFigure;

            // For line shadow
            if(dataPoint.Parent.ShadowEnabled)
            {   
                shadowLineSeg = dataPoint.ShadowFaces.Parts[0] as LineSegment;
                shadowPathFigure = dataPoint.ShadowFaces.Parts[1] as PathFigure;

                if (shadowLineSeg == null)
                {
                    shadowTarget = shadowPathFigure;
                    if (!isAnimationEnabled)
                        shadowPathFigure.StartPoint = new Point(x, y);
                }
                else
                {
                    shadowTarget = shadowLineSeg;
                    if (!isAnimationEnabled)
                        shadowLineSeg.Point = new Point(x, y);
                }
            }

            if (lineSeg == null)
            {   
                target = pathFigure;

                if (isAnimationEnabled)
                {
                    if (dataPoint.Storyboard != null)
                        dataPoint.Storyboard.Pause();

                    oldPoint = pathFigure.StartPoint;
                }
                else
                    pathFigure.StartPoint = new Point(x, y);
            }
            else
            {   
                target = lineSeg;
                
                if(isAnimationEnabled)
                { 
                    if (dataPoint.Storyboard != null)
                        dataPoint.Storyboard.Pause();
                    
                    oldPoint = lineSeg.Point;
                }
                else
                    lineSeg.Point = new Point(x, y);
            }

            if (isAnimationEnabled)
            {   
                #region Apply Animation to the DataPoint

                Storyboard storyBorad = new Storyboard();
                PointAnimation pointAnimation = new PointAnimation();

                pointAnimation.From = oldPoint;
                pointAnimation.To = new Point(x, y);
                pointAnimation.SpeedRatio = 2;
                pointAnimation.Duration = new Duration(new TimeSpan(0, 0, 1));

                target.SetValue(FrameworkElement.NameProperty, "Segment_" + dataPoint.Name);

                Storyboard.SetTarget(pointAnimation, target);
//#if SL
                Storyboard.SetTargetProperty(pointAnimation, (lineSeg != null) ? new PropertyPath("Point") : new PropertyPath("StartPoint"));
//#else
               // Storyboard.SetTargetProperty(pointAnimation, (lineSeg != null) ? new PropertyPath("Point") : new PropertyPath("StartPoint"));

//#endif
                Storyboard.SetTargetName(pointAnimation, (String)target.GetValue(FrameworkElement.NameProperty));

                storyBorad.Children.Add(pointAnimation);

                if (shadowTarget != null)
                {   
                    pointAnimation = new PointAnimation();

                    pointAnimation.From = oldPoint;
                    pointAnimation.To = new Point(x, y);
                    pointAnimation.SpeedRatio = 2;
                    pointAnimation.Duration = new Duration(new TimeSpan(0, 0, 1));

                    shadowTarget.SetValue(FrameworkElement.NameProperty, "ShadowSegment_" + dataPoint.Name);
                    
                    Storyboard.SetTarget(pointAnimation, shadowTarget);
                    Storyboard.SetTargetProperty(pointAnimation, (lineSeg != null) ? new PropertyPath("Point") : new PropertyPath("StartPoint"));
                    Storyboard.SetTargetName(pointAnimation, (String)shadowTarget.GetValue(FrameworkElement.NameProperty));

                    storyBorad.Children.Add(pointAnimation);

#if WPF
                    if (lineSeg != null)
                        (shadowTarget as LineSegment).BeginAnimation(LineSegment.PointProperty, pointAnimation);
                    else
                        (shadowTarget as PathFigure).BeginAnimation(PathFigure.StartPointProperty, pointAnimation);
#endif
                }

                #endregion

                #region Attach Animation with Marker

                FrameworkElement marker = dataPoint.Marker.Visual;
                
                if (marker != null)
                {
                    // Animation for (Canvas.Top) property
                    DoubleAnimation da = new DoubleAnimation()
                    {
                        From = (Double)marker.GetValue(Canvas.LeftProperty),
                        To = newMarkerPosition.X,
                        Duration = new Duration(new TimeSpan(0, 0, 1)),
                        SpeedRatio = 2
                    };

                    Storyboard.SetTarget(da, marker);
                    Storyboard.SetTargetProperty(da, new PropertyPath("(Canvas.Left)"));
                    Storyboard.SetTargetName(da, (String)marker.GetValue(FrameworkElement.NameProperty));

                    storyBorad.Children.Add(da);

                    // Animation for (Canvas.Top) property
                    da = new DoubleAnimation()
                    {
                        From = (Double)marker.GetValue(Canvas.TopProperty),
                        To = newMarkerPosition.Y,
                        Duration = new Duration(new TimeSpan(0, 0, 1)),
                        SpeedRatio = 2
                    };

                    Storyboard.SetTarget(da, marker);
                    Storyboard.SetTargetProperty(da, new PropertyPath("(Canvas.Top)"));
                    Storyboard.SetTargetName(da, (String)marker.GetValue(FrameworkElement.NameProperty));

                    storyBorad.Children.Add(da);
                }

                #endregion

                dataPoint.Storyboard = storyBorad;
#if WPF
                if (lineSeg != null)
                    (target as LineSegment).BeginAnimation(LineSegment.PointProperty, pointAnimation);
                else
                    (target as PathFigure).BeginAnimation(PathFigure.StartPointProperty, pointAnimation);
#endif
                // Start the animation
                storyBorad.Begin();
            }

            //chart.ChartArea.ChartVisualCanvas.Background = new SolidColorBrush(Colors.Blue);
            dataSeries.Faces.Visual.Width = chart.ChartArea.ChartVisualCanvas.Width;
            dataSeries.Faces.Visual.Height = chart.ChartArea.ChartVisualCanvas.Height;

            dataSeries.Faces.LabelCanvas.Width = chart.ChartArea.ChartVisualCanvas.Width;
            dataSeries.Faces.LabelCanvas.Height = chart.ChartArea.ChartVisualCanvas.Height;

            // Update ToolTip Text
            dataPoint._parsedToolTipText = dataPoint.TextParser(dataPoint.ToolTipText);
            
            if(dataSeries._movingMarker != null)
                dataSeries._movingMarker.Visibility = Visibility.Collapsed;
            
            chart._toolTip.Hide();
        }
        
        /// <summary>
        /// Apply marker properties
        /// </summary>
        /// <param name="dataPoint">DataPoint</param>
        /// <param name="markerSize">Marker size</param>
        private static void ApplyMarkerProperties(DataPoint dataPoint)
        {   
            Marker marker = dataPoint.Marker;
            marker.ScaleFactor = (Double) dataPoint.MarkerScale;
            marker.MarkerSize = new Size((Double)dataPoint.MarkerSize, (Double)dataPoint.MarkerSize);
            marker.BorderColor = dataPoint.MarkerBorderColor;
            marker.BorderThickness = ((Thickness)dataPoint.MarkerBorderThickness).Left;
            marker.ShadowEnabled = (Boolean) dataPoint.ShadowEnabled;
            marker.MarkerFillColor = dataPoint.MarkerColor;
        }

        /// <summary>
        /// Apply animation for line chart
        /// </summary>
        /// <param name="canvas">Line chart canvas</param>
        /// <param name="storyboard">Storyboard</param>
        /// <param name="isLineCanvas">Whether canvas is line canvas</param>
        /// <returns>Storyboard</returns>
        private static Storyboard ApplyLineChartAnimation(DataSeries currentDataSeries, Panel canvas, Storyboard storyboard, Boolean isLineCanvas)
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
            
            storyboard.Children.Add(AnimationHelper.CreateDoubleAnimation(currentDataSeries, GradStop2, "(GradientStop.Offset)", beginTime, timeFrames, values, splines));

            values = Graphics.GenerateDoubleCollection(0.01, 1);
            timeFrames = Graphics.GenerateDoubleCollection(0, 1);
            splines = AnimationHelper.GenerateKeySplineList(new Point(0, 0), new Point(1, 1), new Point(0, 0), new Point(1, 1));

            storyboard.Children.Add(AnimationHelper.CreateDoubleAnimation(currentDataSeries, GradStop3, "(GradientStop.Offset)", beginTime, timeFrames, values, splines));
            

            storyboard.Completed += delegate
            {
                GradStop2.Offset = 1;
                GradStop3.Offset = 1;
                GradStop1.Color = Colors.White;
                GradStop2.Color = Colors.White;
                GradStop3.Color = Colors.White;
                GradStop4.Color = Colors.White;

            };
            return storyboard;
        }

        #endregion

        #region Internal Methods

        internal static Marker CreateMarkerAForLineDataPoint(DataPoint dataPoint, Double width, Double height, ref Canvas line2dLabelCanvas, out Double xPosition, out Double yPosition)
        {
            xPosition = Double.NaN;
            yPosition = Double.NaN;
            if(Double.IsNaN(dataPoint.InternalYValue))
                return null;

            PlotGroup plotGroup = dataPoint.Parent.PlotGroup;
            Chart chart = dataPoint.Chart as Chart;

            xPosition = Graphics.ValueToPixelPosition(0, width, (Double)plotGroup.AxisX.InternalAxisMinimum, (Double)plotGroup.AxisX.InternalAxisMaximum, dataPoint.InternalXValue);
            yPosition = Graphics.ValueToPixelPosition(height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, dataPoint.InternalYValue);
                        
            // Create Marker
            Marker marker = GetMarkerForDataPoint(true, chart, yPosition, dataPoint, dataPoint.InternalYValue > 0);
            marker.AddToParent(line2dLabelCanvas, xPosition, yPosition, new Point(0.5, 0.5));

            //Graphics.DrawPointAt(new Point(xPosition, yPosition), line2dLabelCanvas, Colors.Red);

            return marker;
        }

        internal static void CreateAlineSeries(DataSeries series, Double width, Double height, Canvas labelCanvas, Canvas chartsCanvas, Boolean animationEnabled)
        {   
            Canvas line2dCanvas;
            Canvas line2dLabelCanvas;
            
            // Removing exising line chart for a series
            if(series.Faces != null)
            {   
                line2dCanvas = series.Faces.Visual as Canvas;
                line2dLabelCanvas = series.Faces.LabelCanvas;

                if (line2dCanvas != null)
                {   
                    Panel parent = line2dCanvas.Parent as Panel;
                    
                    if (parent != null)
                        parent.Children.Remove(line2dCanvas);
                }

                if (line2dLabelCanvas != null)
                {
                    Panel parent = line2dLabelCanvas.Parent as Panel;

                    if (parent != null)
                        parent.Children.Remove(line2dLabelCanvas);
                }
            }

            if ((Boolean)series.Enabled == false)
                return;

            Double xPosition, yPosition;
            Chart chart  = (series.Chart as Chart);

            line2dLabelCanvas = new Canvas() { Width = width, Height = height };   // Canvas for placing labels

            _listOfDataSeries.Add(series);

            List<List<DataPoint>> pointCollectionList = new List<List<DataPoint>>();
            List<List<DataPoint>> shadowPointCollectionList = new List<List<DataPoint>>();

            PlotGroup plotGroup = series.PlotGroup;
            LineChartShapeParams lineParams = new LineChartShapeParams();

            #region Set LineParms

            lineParams.Points = new List<DataPoint>();
            lineParams.ShadowPoints = new List<DataPoint>();
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

            // Polyline polyline, PolylineShadow;
            // Canvas line2dCanvas = new Canvas();
            // Canvas lineCanvas;

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
                    CreateMarkerAForLineDataPoint(dataPoint, width, height, ref line2dLabelCanvas, out xPosition, out yPosition);

                    #region Generate GeometryGroup for line and line shadow

                    if (IsStartPoint)
                    {
                        variableStartPoint = new Point(xPosition, yPosition);

                        IsStartPoint = !IsStartPoint;

                        if (lineParams.Points.Count > 0)
                        {
                            pointCollectionList.Add(lineParams.Points);
                            shadowPointCollectionList.Add(lineParams.ShadowPoints);
                        }

                        lineParams.Points = new List<DataPoint>();
                        lineParams.ShadowPoints = new List<DataPoint>();
                    }
                    else
                    {
                        endPoint = new Point(xPosition, yPosition);

                        variableStartPoint = endPoint;
                        IsStartPoint = false;
                    }

                    #endregion Generate GeometryGroup for line and line shadow

                    dataPoint._visualPosition = new Point(xPosition, yPosition);
                    lineParams.Points.Add(dataPoint);

                    if (lineParams.ShadowEnabled)
                        lineParams.ShadowPoints.Add(dataPoint);
                }
            }

            pointCollectionList.Add(lineParams.Points);
            shadowPointCollectionList.Add(lineParams.ShadowPoints);

            series.Faces = new Faces();

            Path polyline, PolylineShadow;
            line2dCanvas = GetLine2D(series, lineParams, out polyline, out PolylineShadow, pointCollectionList, shadowPointCollectionList);

            line2dCanvas.Width = width;
            line2dCanvas.Height = height;

            series.Faces.Parts.Add(polyline);
            series.Faces.Parts.Add(PolylineShadow);

            labelCanvas.Children.Add(line2dLabelCanvas);
            chartsCanvas.Children.Add(line2dCanvas);

            series.Faces.Visual = line2dCanvas;
            series.Faces.LabelCanvas = line2dLabelCanvas;

            // Apply animation
            if (animationEnabled)
            {
                if (series.Storyboard == null)
                    series.Storyboard = new Storyboard();
                else
                    series.Storyboard.Stop();

                // Apply animation to the lines
                series.Storyboard = ApplyLineChartAnimation(series, line2dCanvas, series.Storyboard, true);
            }

            // Create Moving Marker
            //if (series.MovingMarkerEnabled)
            {
                Double movingMarkerSize = (Double)series.MarkerSize * (Double)series.MarkerScale * MOVING_MARKER_SCALE;

                if (movingMarkerSize < 6)
                    movingMarkerSize = 6;

                Ellipse movingMarker = new Ellipse() { Visibility = Visibility.Collapsed, IsHitTestVisible = false, Height = movingMarkerSize, Width = movingMarkerSize, Fill = lineParams.LineColor };

                labelCanvas.Children.Add(movingMarker);
                series._movingMarker = movingMarker;
            }
            //else
            //series._movingMarker = null;

        }

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
        internal static Canvas GetVisualObjectForLineChart(Panel preExistingPanel, Double width, Double height, PlotDetails plotDetails, List<DataSeries> seriesList, Chart chart, Double plankDepth, bool animationEnabled)
        {
            if (Double.IsNaN(width) || Double.IsNaN(height) || width <= 0 || height <= 0) return null;
            DataSeries currentDataSeries;

            Canvas visual, labelCanvas, chartsCanvas;
            RenderHelper.RepareCanvas4Drawing(preExistingPanel as Canvas, out visual, out labelCanvas, out chartsCanvas, width, height);
           // visual.Background = Graphics.GetRandomColor();
            Double depth3d = plankDepth / (plotDetails.Layer3DCount == 0 ? 1 : plotDetails.Layer3DCount) * (chart.View3D ? 1 : 0);
            Double visualOffset = depth3d * (plotDetails.SeriesDrawingIndex[seriesList[0]] + 1 - (plotDetails.Layer3DCount == 0 ? 0 : 1));

            // Set visual canvas position
            visual.SetValue(Canvas.TopProperty, visualOffset);
            visual.SetValue(Canvas.LeftProperty, -visualOffset);
            // visual.Background = new SolidColorBrush(Colors.Yellow);

            
            _listOfDataSeries = new List<DataSeries>();

            Boolean isMovingMarkerEnabled = false; // Whether moving marker is enabled for atleast one series

            Double minimumXValue = Double.MaxValue;
            Double maximumXValue = Double.MinValue;

            foreach (DataSeries series in seriesList)
            {
                currentDataSeries = series;
                CreateAlineSeries(series, width, height, labelCanvas, chartsCanvas, animationEnabled);
                isMovingMarkerEnabled = isMovingMarkerEnabled || series.MovingMarkerEnabled;

                minimumXValue = Math.Min(minimumXValue, series.PlotGroup.MinimumX);
                maximumXValue = Math.Max(maximumXValue, series.PlotGroup.MaximumX);

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

            // If animation is not enabled or if there are no series in the serieslist the dont apply animation
            if (animationEnabled && seriesList.Count > 0)
            {   
                // Apply animation to the label canvas
                currentDataSeries = seriesList[0];

                if (currentDataSeries.Storyboard == null)
                    currentDataSeries.Storyboard = new Storyboard();

                currentDataSeries.Storyboard = ApplyLineChartAnimation(currentDataSeries, labelCanvas, currentDataSeries.Storyboard, false);
            }

            // Remove old visual and add new visual in to the existing panel
            if (preExistingPanel != null)
            {
                visual.Children.RemoveAt(1);
               // chartsCanvas.Background = Graphics.GetRandomColor();
                visual.Children.Add(chartsCanvas);
            }
            else
            {
                labelCanvas.SetValue(Canvas.ZIndexProperty, 1);
                visual.Children.Add(labelCanvas);
                visual.Children.Add(chartsCanvas);
            }

            RectangleGeometry clipRectangle = new RectangleGeometry();

            Double clipLeft = 0;
            Double clipTop = -depth3d;
            Double clipWidth = width + depth3d;
            Double clipHeight = height + depth3d + chart.ChartArea.PLANK_THICKNESS + 6;

            AreaChart.GetClipCoordinates(chart, ref clipLeft, ref clipTop, ref clipWidth, ref clipHeight, minimumXValue, maximumXValue);

            clipRectangle.Rect = new Rect(clipLeft, clipTop, clipWidth, clipHeight);

            labelCanvas.Clip = clipRectangle;

            clipRectangle = new RectangleGeometry();
            clipRectangle.Rect = new Rect(0, -depth3d, width + depth3d, height + chart.ChartArea.PLANK_DEPTH);
            chartsCanvas.Clip = clipRectangle;

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
        private static void MoveMovingMarker(object sender, MouseEventArgs e)
        {
            Double xPosition = e.GetPosition(sender as Canvas).X;

            if (!_isMouseEnteredInPlotArea)
                return;

            foreach (DataSeries ds in _listOfDataSeries)
            {
                if (!ds.MovingMarkerEnabled)
                {
                    ds._movingMarker.Visibility = Visibility.Collapsed;
                    continue;
                }

                if (ds._movingMarker != null)
                {
                    if (ds._movingMarker.Visibility == Visibility.Collapsed)
                        ds._movingMarker.Visibility = Visibility.Visible;

                    DataPoint nearestDataPoint = null;

                    foreach (DataPoint dp in ds.DataPoints)
                    {
                        if (dp.Marker != null)
                        {
                            dp._distance = Math.Abs(xPosition - dp._visualPosition.X);

                            if (nearestDataPoint == null)
                            {
                                nearestDataPoint = dp;
                                continue;
                            }

                            if (dp._distance < nearestDataPoint._distance)
                                nearestDataPoint = dp;
                        }
                    }

                    // DataPoint nearestDataPoint = (from dp in ds.DataPoints orderby Math.Abs(xPosition - dp._visualPosition.X) select dp).First();

                    Ellipse movingMarker = ds._movingMarker;

                    if (nearestDataPoint.Selected)
                    {
                        SelectMovingMarker(nearestDataPoint);
                    }
                    else
                    {
                        movingMarker.Fill = nearestDataPoint.Parent.Color;

                        Double movingMarkerSize = (Double)nearestDataPoint.Parent.MarkerSize * (Double)nearestDataPoint.Parent.MarkerScale * MOVING_MARKER_SCALE;

                        if (movingMarkerSize < 6)
                            movingMarkerSize = 6;

                        movingMarker.Height = movingMarkerSize;
                        movingMarker.Width = movingMarker.Height;
                        movingMarker.StrokeThickness = 0;

                        movingMarker.SetValue(Canvas.LeftProperty, nearestDataPoint._visualPosition.X - movingMarker.Width / 2);
                        movingMarker.SetValue(Canvas.TopProperty, nearestDataPoint._visualPosition.Y - movingMarker.Height / 2);
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
