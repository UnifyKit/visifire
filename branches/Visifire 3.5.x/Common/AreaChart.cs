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
using System.Windows.Shapes;
using System.Windows.Media.Animation;
#else

using System;
using System.Windows;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;


#endif

using Visifire.Commons;

namespace Visifire.Charts
{   
    /// <summary>
    /// Visifire.Charts.PolygonalChartShapeParams class
    /// </summary>
    internal class PolygonalChartShapeParams
    {
        /// <summary>
        /// Collection of points
        /// </summary>
        public PointCollection Points 
        { 
            get; 
            set; 
        }

        /// <summary>
        /// Background color of the polygon
        /// </summary>
        public Brush Background
        {
            get;
            set;
        }

        /// <summary>
        /// Border color of the polygon
        /// </summary>
        public Brush BorderColor
        {
            get;
            set;
        }

        /// <summary>
        /// Weather bevel effect is applied
        /// </summary>
        public Boolean Bevel
        {
            get;
            set;
        }

        /// <summary>
        /// Weather lighting effect is applied
        /// </summary>
        public Boolean Lighting
        {
            get;
            set;
        }

        /// <summary>
        /// Weather shadow effect is applied
        /// </summary>
        public Boolean Shadow
        {
            get;
            set;
        }

        /// <summary>
        /// Border style property
        /// </summary>
        public DoubleCollection BorderStyle
        {
            get;
            set;
        }

        /// <summary>
        /// Border thickness property
        /// </summary>
        public Double BorderThickness
        {
            get;
            set;
        }

        /// <summary>
        /// Whether used for positive DataPoint
        /// </summary>
        public Boolean IsPositive
        {
            get;
            set;
        }

        /// <summary>
        /// 3D depth of ploygon area
        /// </summary>
        public Double Depth3D
        {
            get;
            set;
        }

        /// <summary>
        /// Storyboard used for animation
        /// </summary>
        public Storyboard Storyboard
        {
            get;
            set;
        }

        /// <summary>
        /// Whether animation is enabled for control
        /// </summary>
        public Boolean AnimationEnabled
        {
            get;
            set;
        }

        /// <summary>
        /// Ploygon visual size
        /// </summary>
        public Size Size
        {
            get;
            set;
        }

        /// <summary>
        /// Tag reference for visual object
        /// </summary>
        public FrameworkElement TagReference
        {
            get;
            set;
        }

    }

    /// <summary>
    /// Visifire.Charts.AreaChart class
    /// </summary>
    internal class AreaChart
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
        /// Get Stacked3D side faces
        /// </summary>
        /// <param name="faces">Faces</param>
        /// <param name="areaParams">Area parameters</param>
        /// <returns>Canvas</returns>
        private static Canvas GetStacked3DSideFaces(ref Faces faces, PolygonalChartShapeParams areaParams)
        {
            Point centroid;
            Brush sideBrush = areaParams.Lighting ? Graphics.GetRightFaceBrush(areaParams.Background) : areaParams.Background;
            Brush topBrush = areaParams.Lighting ? Graphics.GetTopFaceBrush(areaParams.Background) : areaParams.Background;
            Int32 pointIndexLimit = areaParams.IsPositive ? areaParams.Points.Count - 1 : areaParams.Points.Count;

            Canvas polygonSet = new Canvas();
            Rect size = GetBounds(areaParams.Points);
            polygonSet.Width = size.Width + areaParams.Depth3D;
            polygonSet.Height = size.Height + areaParams.Depth3D;
            polygonSet.SetValue(Canvas.TopProperty, size.Top - areaParams.Depth3D);
            polygonSet.SetValue(Canvas.LeftProperty, size.Left);

            for (Int32 i = 0; i < pointIndexLimit; i++)
            {
                Polygon sides = new Polygon() { Tag = new ElementData() { Element = areaParams.TagReference } };
                PointCollection points = new PointCollection();
                Int32 index1 = i % areaParams.Points.Count;
                Int32 index2 = (i + 1) % areaParams.Points.Count;

                points.Add(areaParams.Points[index1]);
                points.Add(areaParams.Points[index2]);
                points.Add(new Point(areaParams.Points[index2].X + areaParams.Depth3D, areaParams.Points[index2].Y - areaParams.Depth3D));
                points.Add(new Point(areaParams.Points[index1].X + areaParams.Depth3D, areaParams.Points[index1].Y - areaParams.Depth3D));
                sides.Points = points;

                centroid = GetCentroid(points);
                Int32 zindex = GetAreaZIndex(centroid.X, centroid.Y, areaParams.IsPositive);
                sides.SetValue(Canvas.ZIndexProperty, zindex);

                if (i == (areaParams.Points.Count - 2))
                {
                    sides.Fill = sideBrush;
                    (sides.Tag as ElementData).VisualElementName = "Side";
                }
                else
                {
                    sides.Fill = topBrush;
                    (sides.Tag as ElementData).VisualElementName = "Top";
                }

                sides.Stroke = areaParams.BorderColor;
                sides.StrokeDashArray = areaParams.BorderStyle != null ? ExtendedGraphics.CloneCollection(areaParams.BorderStyle) : areaParams.BorderStyle;
                sides.StrokeThickness = areaParams.BorderThickness;
                sides.StrokeMiterLimit = 1;

                Rect sidesBounds = GetBounds(points);
                sides.Stretch = Stretch.Fill;
                sides.Width = sidesBounds.Width;
                sides.Height = sidesBounds.Height;
                sides.SetValue(Canvas.TopProperty, sidesBounds.Y - (size.Top - areaParams.Depth3D));
                sides.SetValue(Canvas.LeftProperty, sidesBounds.X - size.X);

                faces.Parts.Add(sides);
                polygonSet.Children.Add(sides);

            }

            return polygonSet;
        }

        /// <summary>
        /// Get area Z-Index
        /// </summary>
        /// <param name="left">Left position</param>
        /// <param name="top">Top position</param>
        /// <param name="isPositive">Whether DataPoint Value is positive or negative</param>
        /// <returns>Zindex as Int32</returns>
        private static Int32 GetAreaZIndex(Double left, Double top, Boolean isPositive)
        {
            Int32 Zi = 0;
            Int32 ioffset = (Int32)left;

            if (ioffset == 0)
                ioffset++;

            Zi = (isPositive) ? Zi + (Int32)(ioffset) : Zi + Int32.MinValue + (Int32)(ioffset);

            return Zi;
        }

        /// <summary>
        /// Get Z-Index for StackedArea visual
        /// </summary>
        /// <param name="left">Left position</param>
        /// <param name="top">Top position</param>
        /// <param name="isPositive">Whether DataPoint value is negative or positive</param>
        /// <param name="index">Index</param>
        /// <returns>Zindex as Int32</returns>
        private static Int32 GetStackedAreaZIndex(Double left, Double top, Boolean isPositive, Int32 index)
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
        ///  Apply area chart animation
        /// </summary>
        /// <param name="areaElement">Area visual reference</param>
        /// <param name="storyboard">Storyboard</param>
        /// <param name="isPositive">Whether DataPoint is positive</param>
        /// <param name="beginTime">Animation begin time</param>
        /// <returns>Storyboard</returns>
        private static Storyboard ApplyAreaAnimation(DataSeries currentDataSeries, UIElement areaElement, Storyboard storyboard, bool isPositive, Double beginTime)
        {
            ScaleTransform scaleTransform = new ScaleTransform() { ScaleY = 0 };
            areaElement.RenderTransform = scaleTransform;

            if (isPositive)
            {
                areaElement.RenderTransformOrigin = new Point(0.5, 1);
            }
            else
            {
                areaElement.RenderTransformOrigin = new Point(0.5, 0);
            }

            DoubleCollection values = Graphics.GenerateDoubleCollection(0, 1);
            DoubleCollection frameTimes = Graphics.GenerateDoubleCollection(0, 0.75);
            List<KeySpline> splines = AnimationHelper.GenerateKeySplineList
                (
                new Point(0, 0), new Point(1, 1),
                new Point(0, 0), new Point(0.5, 1)
                );

            DoubleAnimationUsingKeyFrames growAnimation = AnimationHelper.CreateDoubleAnimation(currentDataSeries, scaleTransform, "(ScaleTransform.ScaleY)", beginTime + 0.5, frameTimes, values, splines);
            storyboard.Children.Add(growAnimation);

            return storyboard;
        }

        /// <summary>
        /// Apply animation for StackedArea chart
        /// </summary>
        /// <param name="areaElement">Area visual reference</param>
        /// <param name="storyboard">Storyboard</param>
        /// <param name="beginTime">Animation begin time</param>
        /// <param name="duration">Animation duration</param>
        /// <returns>Storyboard</returns>
        private static Storyboard ApplyStackedAreaAnimation(DataSeries currentDataSeries, FrameworkElement areaElement, Storyboard storyboard, Double beginTime, Double duration)
        {
            return AnimationHelper.ApplyOpacityAnimation(areaElement, currentDataSeries, storyboard, beginTime, duration,0, 1);
        }

        /// <summary>
        /// Creating opacity animation for bevel layer
        /// </summary>
        /// <param name="storyboard">Storyboard</param>
        /// <param name="target">Target bevel layer</param>
        /// <param name="beginTime">Animation begin time</param>
        /// <param name="opacity">Target opacity</param>
        /// <param name="duration">Animation duration</param>
        /// <returns>Storyboard</returns>
        private static Storyboard CreateOpacityAnimation(DataSeries currentDataSeries, Storyboard storyboard, DependencyObject target, Double beginTime, Double opacity, Double duration)
        {
            DoubleCollection values = Graphics.GenerateDoubleCollection(0, opacity);
            DoubleCollection frames = Graphics.GenerateDoubleCollection(0, duration);
            List<KeySpline> splines = AnimationHelper.GenerateKeySplineList(frames.Count);

            DoubleAnimationUsingKeyFrames opacityAnimation = AnimationHelper.CreateDoubleAnimation(currentDataSeries, target, "(UIElement.Opacity)", beginTime + 0.5, frames, values, splines);
            storyboard.Children.Add(opacityAnimation);

            return storyboard;
        }

        /// <summary>
        /// Get area parameters
        /// </summary>
        /// <param name="series">DataSeries</param>
        /// <param name="colorBrush">color</param>
        /// <param name="depth3d">3D depth</param>
        /// <returns>PolygonalChartShapeParams</returns>
        private static PolygonalChartShapeParams GetAreaParms(DataSeries series, Brush colorBrush, Double depth3d)
        {
            PolygonalChartShapeParams areaParams = new PolygonalChartShapeParams();
            areaParams.Background = colorBrush;
            areaParams.Lighting = (Boolean)series.LightingEnabled;
            areaParams.Shadow = (Boolean)series.ShadowEnabled;
            areaParams.Bevel = series.Bevel;
            areaParams.BorderColor = series.BorderColor;
            areaParams.BorderStyle = ExtendedGraphics.GetDashArray(series.BorderStyle);
            areaParams.BorderThickness = series.BorderThickness.Left;
            areaParams.Depth3D = depth3d;
            areaParams.TagReference = series;
            return areaParams;
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Create new Marker
        /// </summary>
        /// <param name="chart">Chart</param>
        /// <param name="dataPoint">DataPoint</param>
        /// <param name="markerSize">Marker Size</param>
        /// <param name="labelText">Label text</param>
        /// <returns>Marker</returns>
        internal static Marker CreateNewMarker(Chart chart, DataPoint dataPoint, Size markerSize, String labelText)
        {
            Boolean markerBevel = false;
            Marker marker = new Marker((MarkerTypes)dataPoint.MarkerType, (Double)dataPoint.MarkerScale, markerSize, markerBevel, dataPoint.MarkerColor, labelText);

            marker.MarkerSize = markerSize;
            marker.BorderColor = dataPoint.MarkerBorderColor;
            marker.BorderThickness = ((Thickness)dataPoint.MarkerBorderThickness).Left;
            marker.FontColor = Chart.CalculateDataPointLabelFontColor(chart, dataPoint, dataPoint.LabelFontColor, LabelStyles.OutSide);
            marker.FontFamily = dataPoint.LabelFontFamily;
            marker.FontSize = (Double)dataPoint.LabelFontSize;
            marker.FontStyle = (FontStyle)dataPoint.LabelFontStyle;
            marker.FontWeight = (FontWeight)dataPoint.LabelFontWeight;
            marker.TextBackground = dataPoint.LabelBackground;
            marker.MarkerFillColor = dataPoint.MarkerColor;
            marker.Tag = new ElementData() { Element = dataPoint };
            return marker;
        }

        /// <summary>
        /// Get Marker for DataPoint
        /// </summary>
        /// <param name="chart">Chart</param>
        /// <param name="dataPoint">DataPoint</param>
        /// <param name="position">position of the marker</param>
        /// <param name="isPositive">Whether DataPoint Value is positive or negative</param>
        /// <returns>Marker</returns>
        internal static Marker GetMarkerForDataPoint(Chart chart, Double height, Boolean isTopOfStack, DataPoint dataPoint, Double position, bool isPositive)
        {
            if ((Boolean)dataPoint.MarkerEnabled || (Boolean)dataPoint.LabelEnabled)
            {
                Size markerSize = (Boolean)dataPoint.MarkerEnabled ? new Size((Double)dataPoint.MarkerSize, (Double)dataPoint.MarkerSize) : new Size(0, 0);
                String labelText = (Boolean)dataPoint.LabelEnabled ? dataPoint.TextParser(dataPoint.LabelText) : "";

                dataPoint.Marker = CreateNewMarker(chart, dataPoint, markerSize, labelText);

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
                                if (!isTopOfStack)
                                {
                                    if (position + dataPoint.Marker.TextBlockSize.Height > height)
                                        dataPoint.Marker.TextAlignmentY = AlignmentY.Top;
                                    else
                                        dataPoint.Marker.TextAlignmentY = AlignmentY.Bottom;
                                }
                                else
                                {
                                    //if (position < dataPoint.Marker.MarkerActualSize.Height || dataPoint.LabelStyle == LabelStyles.Inside)
                                    if (position - dataPoint.Marker.MarkerActualSize.Height - dataPoint.Marker.MarkerSize.Height / 2 < 0 || dataPoint.LabelStyle == LabelStyles.Inside)
                                        dataPoint.Marker.TextAlignmentY = AlignmentY.Bottom;
                                    else
                                        dataPoint.Marker.TextAlignmentY = AlignmentY.Top;
                                }
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
                                if (!isTopOfStack)
                                    dataPoint.Marker.TextAlignmentY = AlignmentY.Top;
                                else
                                {
                                    if (position + dataPoint.Marker.MarkerActualSize.Height + dataPoint.Marker.MarkerSize.Height / 2 > chart.PlotArea.BorderElement.Height || dataPoint.LabelStyle == LabelStyles.Inside)
                                        dataPoint.Marker.TextAlignmentY = AlignmentY.Top;
                                    else
                                        dataPoint.Marker.TextAlignmentY = AlignmentY.Bottom;
                                }
                            }
                            else if (dataPoint.LabelStyle == LabelStyles.OutSide)
                                dataPoint.Marker.TextAlignmentY = AlignmentY.Bottom;
                            else
                                dataPoint.Marker.TextAlignmentY = AlignmentY.Top;
                        }

                    }
                }

                dataPoint.Marker.CreateVisual();

                return dataPoint.Marker;
            }

            return null;
        }

        public static List<List<DataPoint>> BrokenAreaDataPointsGroup(Double width, Double height, DataSeries dataSeries)
        {
            Double xPosition;
            Double yPosition;
            Chart chart = dataSeries.Chart as Chart;

            PlotGroup plotGroup = dataSeries.PlotGroup;

            List<List<DataPoint>> brokenAreaDataPointsCollection = new List<List<DataPoint>>();

            List<DataPoint> pointsList4EachBrokenAreaGroup = new List<DataPoint>();

            Point endPoint = new Point();
            Boolean IsStartPoint = true;

            List<DataPoint> dataPointsInViewPort = RenderHelper.GetDataPointsUnderViewPort(dataSeries, false);
            List<DataPoint> sortedDataPoints = (from dp in dataPointsInViewPort orderby dp.InternalXValue select dp).ToList();

            foreach (DataPoint dataPoint in sortedDataPoints)
            {
                dataPoint.Faces = null;
                if (dataPoint.Enabled == false)
                {
                    continue;
                }

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
                    
                    if (IsStartPoint)
                    {   
                        IsStartPoint = !IsStartPoint;

                        if (pointsList4EachBrokenAreaGroup.Count > 0)
                        {   
                            brokenAreaDataPointsCollection.Add(pointsList4EachBrokenAreaGroup);
                        }

                        pointsList4EachBrokenAreaGroup = new List<DataPoint>();
                    }
                    else
                    {   
                        endPoint = new Point(xPosition, yPosition);
                        IsStartPoint = false;
                    }

                    dataPoint._visualPosition = new Point(xPosition, yPosition);
                    pointsList4EachBrokenAreaGroup.Add(dataPoint);
                }
            }

            brokenAreaDataPointsCollection.Add(pointsList4EachBrokenAreaGroup);

            return brokenAreaDataPointsCollection;
        }

        private static void CreateBevelFor2DArea(Canvas areaCanvas, DataPoint currentDataPoint, DataPoint previusDataPoint, Boolean clipAtStart, Boolean clipAtEnd)
        {
            Line line4Bevel42DArea = new Line(){
                StrokeThickness = 2.4,
                Stroke = Graphics.GetBevelTopBrush(currentDataPoint.Parent.Color),
                StrokeEndLineCap = PenLineCap.Round,
                StrokeStartLineCap = PenLineCap.Triangle,
                IsHitTestVisible = false
            };

            line4Bevel42DArea.SetValue(Canvas.ZIndexProperty, (Int32) 2);
            line4Bevel42DArea.X1 = previusDataPoint.Faces.AreaFrontFaceLineSegment.Point.X ;
            line4Bevel42DArea.Y1 = previusDataPoint.Faces.AreaFrontFaceLineSegment.Point.Y;
            line4Bevel42DArea.X2 = currentDataPoint._visualPosition.X;
            line4Bevel42DArea.Y2 = currentDataPoint._visualPosition.Y;
            currentDataPoint.Faces.BevelLine = line4Bevel42DArea;
            previusDataPoint.Faces.BevelLine = line4Bevel42DArea;
            areaCanvas.Children.Add(line4Bevel42DArea);
        }

        /// <summary>
        /// Get visual object for area chart
        /// </summary>
        /// <param name="width">Width of the PlotArea</param>
        /// <param name="height">Height of the PlotArea</param>
        /// <param name="plotDetails">PlotDetails</param>
        /// <param name="seriesList">List of DataSeries with render as area chart</param>
        /// <param name="chart">Chart</param>
        /// <param name="plankDepth">PlankDepth</param>
        /// <param name="animationEnabled">Whether animation is enabled for chart</param>
        /// <returns>Area chart canvas</returns>
        internal static Canvas GetVisualObjectForAreaChart(Panel preExistingPanel, Double width, Double height, PlotDetails plotDetails, List<DataSeries> seriesList, Chart chart, Double plankDepth, bool animationEnabled)
        {   
            if (Double.IsNaN(width) || Double.IsNaN(height) || width <= 0 || height <= 0)
                return null;

            DataSeries series = seriesList[0] as DataSeries;

            if (animationEnabled)
            {
                if (series.Storyboard == null)
                    series.Storyboard = new Storyboard();
            }

            Canvas visual, labelCanvas, areaCanvas, areaFaceCanvas;
            RenderHelper.RepareCanvas4Drawing(preExistingPanel as Canvas, out visual, out labelCanvas, out areaCanvas, width, height);
            areaFaceCanvas = new Canvas() { Height = height, Width = width };

            Double depth3d = plankDepth / plotDetails.Layer3DCount * (chart.View3D ? 1 : 0);
            Double visualOffset = depth3d * (plotDetails.SeriesDrawingIndex[series] + 1);
            
            visual.SetValue(Canvas.TopProperty, visualOffset);
            visual.SetValue(Canvas.LeftProperty, -visualOffset);

            labelCanvas.SetValue(Canvas.TopProperty, (Double)0);
            labelCanvas.SetValue(Canvas.LeftProperty, (Double)0);

            areaFaceCanvas.SetValue(Canvas.TopProperty, (Double)0);
            areaFaceCanvas.SetValue(Canvas.LeftProperty, (Double)0);
            //areaFaceCanvas.SetValue(Canvas.ZIndexProperty, (Int32)1);
            DataSeries currentDataSeries;

            Double minimumXValue = Double.MaxValue;
            Double maximumXValue = Double.MinValue;

            if ((Boolean)series.Enabled)
            {   
                if (series.Storyboard == null)
                    series.Storyboard = new Storyboard();

                currentDataSeries = series;

                PlotGroup plotGroup = series.PlotGroup;

                Double limitingYValue = plotGroup.GetLimitingYValue();

                minimumXValue = Math.Min(minimumXValue, plotGroup.MinimumX);
                maximumXValue = Math.Max(maximumXValue, plotGroup.MaximumX);

                //List<DataPoint> enabledDataPoints = (from datapoint in series.InternalDataPoints where datapoint.Enabled == true select datapoint).ToList();

                Faces dataSeriesFaces = new Faces();
                dataSeriesFaces.FrontFacePaths = new List<Path>();
                dataSeriesFaces.Visual = areaFaceCanvas;
                dataSeriesFaces.LabelCanvas = labelCanvas;
                series.Faces = dataSeriesFaces;

                List<List<DataPoint>> brokenAreaDataPointsCollection = BrokenAreaDataPointsGroup(width, height, series);
                
                DataPoint currentDataPoint;
                DataPoint nextDataPoint;
                DataPoint previusDataPoint;

                Double plankYPos = Graphics.ValueToPixelPosition(height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, limitingYValue);

                foreach (List<DataPoint> dataPointList in brokenAreaDataPointsCollection)
                {   
                    if (dataPointList.Count <= 0)
                        continue;
                    
                    currentDataPoint = dataPointList[0];
                    previusDataPoint = currentDataPoint;
                    PointCollection points = new PointCollection();

                    List<DataPoint> dataPoints = new List<DataPoint>();
                    
                    Path frontFacePath = null;
                    PathGeometry frontFacePathGeometry;
                    PathFigure frontFacePathFigure = null;

                    Int32 maxZIndex = 0;

                    for (Int32 i = 0; i < dataPointList.Count - 1; i++)
                    {
                        Path areaBase = new Path();
                        Faces dataPointFaces;
                        Faces nextDataPointFaces = new Faces();

                        currentDataPoint = dataPointList[i];
                        currentDataPoint._parsedToolTipText = currentDataPoint.TextParser(currentDataPoint.ToolTipText);
                        nextDataPoint = dataPointList[i + 1];

                        if (currentDataPoint.Faces == null)
                        {   
                            dataPointFaces = new Faces();
                            currentDataPoint.Faces = dataPointFaces;
                        }
                        else
                            dataPointFaces = currentDataPoint.Faces;

                        nextDataPoint.Faces = nextDataPointFaces;

                        dataPointFaces.PreviousDataPoint = previusDataPoint;
                        dataPointFaces.NextDataPoint = nextDataPoint;

                        if (i == 0)
                        {   
                            // For the first DataPoint left and top face are drawn.
                            Double xPosDataPoint = Graphics.ValueToPixelPosition(0, width, (Double)plotGroup.AxisX.InternalAxisMinimum, (Double)plotGroup.AxisX.InternalAxisMaximum, currentDataPoint.InternalXValue);

                            if (chart.View3D)
                            {   
                                // Set points for left face
                                Area3DDataPointFace leftFace = new Area3DDataPointFace(depth3d);
                                leftFace.FrontFacePoints.Add(new Point(xPosDataPoint, plankYPos)); // Bottom Point
                                leftFace.FrontFacePoints.Add(currentDataPoint._visualPosition);    // Top Point
                                currentDataPoint.Faces.Area3DLeftFace = leftFace;

                                // Set points for top left face
                                Area3DDataPointFace topFace = new Area3DDataPointFace(depth3d);
                                topFace.FrontFacePoints.Add(currentDataPoint._visualPosition);   // Front Left Point
                                topFace.FrontFacePoints.Add(nextDataPoint._visualPosition);      // Front Right Point     
                                currentDataPoint.Faces.Area3DRightTopFace = topFace;
                                nextDataPointFaces.Area3DLeftTopFace = topFace;
                            }

                            // Start creating front face of 3D area
                            frontFacePath = new Path() { Opacity = currentDataPoint.Parent.Opacity };
                            //frontFacePath = new Path() { Opacity = 0.5 };
                            ApplyBorderProperties(frontFacePath, currentDataPoint.Parent);
                            dataSeriesFaces.FrontFacePaths.Add(frontFacePath);
                            frontFacePathGeometry = new PathGeometry();
                            frontFacePathFigure = new PathFigure() { StartPoint = new Point(xPosDataPoint, plankYPos), IsClosed = true };
                            frontFacePathGeometry.Figures.Add(frontFacePathFigure);
                            frontFacePath.Data = frontFacePathGeometry;
                            
                            // Area front face Line path from end point to first
                            LineSegment ls = new LineSegment() { Point = currentDataPoint._visualPosition };
                            frontFacePathFigure.Segments.Add(ls);
                            dataPointFaces.AreaFrontFaceLineSegment = ls; 
                        }
                        else
                        {
                            if (chart.View3D)
                            {   
                                // DataPoint which has two different top faces at the left and right side of it position.
                                Area3DDataPointFace topFace = new Area3DDataPointFace(depth3d);
                                topFace.FrontFacePoints.Add(currentDataPoint._visualPosition);  // Front Left Point
                                topFace.FrontFacePoints.Add(nextDataPoint._visualPosition);     // Front Right Point     

                                currentDataPoint.Faces.Area3DRightTopFace = topFace;
                                nextDataPointFaces.Area3DLeftTopFace = topFace;
                            }
                            else
                            {   
                                if(currentDataPoint.Parent.Bevel)
                                    CreateBevelFor2DArea(areaFaceCanvas, currentDataPoint, previusDataPoint,false, false); 
                            }

                            // Area front face Line path
                            LineSegment ls = new LineSegment() { Point = currentDataPoint._visualPosition };
                            frontFacePathFigure.Segments.Add(ls);
                            dataPointFaces.AreaFrontFaceLineSegment = ls; 
                        }

                        #region Create Marker

                        if (currentDataPoint.MarkerEnabled == true || currentDataPoint.LabelEnabled == true)
                        {
                            Double xPos, yPos;
                            // Create Marker
                            Marker marker = LineChart.CreateMarkerAForLineDataPoint(currentDataPoint, width, height, ref labelCanvas, out xPos, out yPos);

                            //if (marker != null)
                            //{
                            //    //LineChart.ApplyDefaultInteractivityForMarker(dataPoint);

                            //    //marker.AddToParent(labelCanvas, currentDataPoint._visualPosition.X, currentDataPoint._visualPosition.Y, new Point(0.5, 0.5));

                            //    // Apply marker animation
                            //    if (animationEnabled)
                            //    {
                            //        if (currentDataPoint.Parent.Storyboard == null)
                            //            currentDataPoint.Parent.Storyboard = new Storyboard();

                            //        // Apply marker animation
                            //        currentDataPoint.Parent.Storyboard = AnimationHelper.ApplyOpacityAnimation(marker, CurrentDataSeries, currentDataPoint.Parent.Storyboard, 1, currentDataPoint.Opacity * currentDataPoint.Parent.Opacity);
                            //    }
                            //}
                        }

                        #endregion

                        if (chart.View3D)
                        {
                            Int32 zindex = Draw3DArea(areaFaceCanvas, previusDataPoint, currentDataPoint, nextDataPoint, ref dataSeriesFaces, ref dataPointFaces, currentDataPoint.Parent, plankYPos);
                            maxZIndex = Math.Max(maxZIndex, zindex);
                        }
                        else
                        {   
                            //areaCanvas.Children.Add(Get2DArea(ref faces, areaParams));
                        }

                        if (i == dataPointList.Count - 2) // If next DataPoint is the last DataPoint
                        {   
                            if (chart.View3D)
                            {
                                DataPoint lastDataPoint = nextDataPoint;

                                Area3DDataPointFace rightFace = new Area3DDataPointFace(depth3d);
                                rightFace.FrontFacePoints.Add(nextDataPoint._visualPosition); // Front top point
                                rightFace.FrontFacePoints.Add(new Point(nextDataPoint._visualPosition.X, plankYPos));
                                nextDataPoint.Faces.Area3DRightFace = rightFace;

                                // Draw base of the 3d area
                                areaBase = new Path();
                                areaBase.Fill = (Boolean)dataPointList[0].Parent.LightingEnabled ? Graphics.GetTopFaceBrush(dataPointList[0].Parent.Color) : dataPointList[0].Parent.Color;
                                PathGeometry pg = new PathGeometry();
                                PathFigure pf = new PathFigure() { StartPoint = new Point(dataPointList[0]._visualPosition.X, plankYPos) };
                                pg.Figures.Add(pf);
                                pf.Segments.Add(new LineSegment() { Point = new Point(dataPointList[0]._visualPosition.X + depth3d, plankYPos - depth3d) });
                                pf.Segments.Add(new LineSegment() { Point = new Point(lastDataPoint._visualPosition.X + depth3d, plankYPos - depth3d) });
                                pf.Segments.Add(new LineSegment() { Point = new Point(lastDataPoint._visualPosition.X, plankYPos) });
                                areaBase.Data = pg;
                                areaBase.SetValue(Canvas.ZIndexProperty, (Int32) 1);
                                areaBase.Opacity = lastDataPoint.Parent.Opacity;
                                areaCanvas.Children.Add(areaBase);
                                dataSeriesFaces.FrontFacePaths.Add(areaBase);
                                series.Faces.VisualComponents.Add(areaBase);

                                // Animating AreaBase opacity
                                if (animationEnabled)
                                    series.Storyboard = AnimationHelper.ApplyOpacityAnimation(areaBase, series, series.Storyboard, 0.25, 1, 0, 1);
                            }
                            else
                            {   
                                if (nextDataPoint.Parent.Bevel)
                                    CreateBevelFor2DArea(areaFaceCanvas, nextDataPoint, currentDataPoint, false, false); 
                            }

                            // Front face for 3D and 2D both
                            LineSegment ls = new LineSegment() { Point = nextDataPoint._visualPosition };
                            frontFacePathFigure.Segments.Add(ls);
                            nextDataPointFaces.AreaFrontFaceLineSegment = ls;
                            ls = new LineSegment() { Point = new Point(nextDataPoint._visualPosition.X, plankYPos) };
                            frontFacePathFigure.Segments.Add(ls);
                            nextDataPointFaces.AreaFrontFaceBaseLineSegment = ls;

                            nextDataPointFaces.NextDataPoint = nextDataPoint;
                            
                            // Graphics.DrawPointAt(rightFace.FrontFacePoints[0], areaCanvas, Colors.Yellow);
                            if (chart.View3D)
                            {   
                                Int32 zindex = Draw3DArea(areaFaceCanvas, previusDataPoint, nextDataPoint, nextDataPoint, ref dataSeriesFaces, ref nextDataPointFaces, nextDataPoint.Parent, plankYPos);
                                maxZIndex = Math.Max(maxZIndex, zindex);
                            }
                            else
                            {   
                                // areaCanvas.Children.Add(Get2DArea(ref faces, areaParams));
                            }

                            if (nextDataPoint.MarkerEnabled == true || nextDataPoint.LabelEnabled == true)
                            {
                                Double xPos, yPos;
                                Marker marker = LineChart.CreateMarkerAForLineDataPoint(nextDataPoint, width, height, ref labelCanvas, out xPos, out yPos);
                            }

                            nextDataPoint._parsedToolTipText = nextDataPoint.TextParser(nextDataPoint.ToolTipText);

                        }

                        previusDataPoint = currentDataPoint;
                    }

                    if (frontFacePath != null)
                    {
                        if (chart.View3D)
                            frontFacePath.Fill = (Boolean)dataPointList[0].Parent.LightingEnabled ? Graphics.GetFrontFaceBrush(dataPointList[0].Parent.Color) : dataPointList[0].Parent.Color;
                        else
                            frontFacePath.Fill = (Boolean)dataPointList[0].Parent.LightingEnabled ? Graphics.GetLightingEnabledBrush(dataPointList[0].Parent.Color, "Linear", null) : dataPointList[0].Parent.Color;

                        series.Faces.VisualComponents.Add(frontFacePath);

                        frontFacePath.SetValue(Canvas.ZIndexProperty, maxZIndex);
                        areaFaceCanvas.Children.Add(frontFacePath);
                    }

                    foreach (FrameworkElement face in series.Faces.VisualComponents)
                        VisifireElement.AttachEvents2AreaVisual(currentDataPoint, currentDataPoint, face);

                }

                foreach (FrameworkElement face in series.Faces.VisualComponents)
                    VisifireElement.AttachEvents2AreaVisual(series, series, face);

                if (!VisifireControl.IsXbapApp)
                {
                    if ((Boolean)series.ShadowEnabled)
                    {
                        if (series.Faces != null && series.Faces.Visual != null)
                        {
                            series.Faces.Visual.Effect = ExtendedGraphics.GetShadowEffect(135, 2, 1);
                        }
                    }
                }

                if(!chart.IndicatorEnabled)
                    series.AttachAreaToolTip(chart, dataSeriesFaces.VisualComponents);

                areaFaceCanvas.Tag = null;

                if (brokenAreaDataPointsCollection.Count > 0)
                {
                    Canvas plank = ColumnChart.CreateOrUpdatePlank(chart, series.PlotGroup.AxisY, areaCanvas, depth3d, Orientation.Horizontal);
                }

                // apply area animation
                if (animationEnabled)
                {   
                    // if (series.Storyboard == null)
                    //  series.Storyboard = new Storyboard();

                    ScaleTransform scaleTransform = new ScaleTransform() { ScaleY = 0 };
                    areaFaceCanvas.RenderTransformOrigin = new Point(0.5, plankYPos / height);
                    areaFaceCanvas.RenderTransform = scaleTransform;

                    List<KeySpline> splines = AnimationHelper.GenerateKeySplineList
                        (
                        new Point(0, 0), new Point(1, 1),
                        new Point(0, 1), new Point(0.5, 1)
                        );

                    // Apply animation to the entire canvas that was used to create the area
                    series.Storyboard = AnimationHelper.ApplyPropertyAnimation(scaleTransform, "(ScaleTransform.ScaleY)", series, series.Storyboard, 1,
                        new Double[] { 0, 1 }, new Double[] { 0, 1 }, splines);

                    // Animating plank opacity
                    //series.Storyboard = AnimationHelper.ApplyOpacityAnimation(areaBase, series, series.Storyboard, 1.25, 1, 0, 1);

                    // Apply animation for label canvas
                    series.Storyboard = AnimationHelper.ApplyOpacityAnimation(labelCanvas, series, series.Storyboard, 1.25, 1, 0, 1);
                }
            }

            areaFaceCanvas.SetValue(Canvas.ZIndexProperty, (Int32)2);
            areaCanvas.Children.Add(areaFaceCanvas);

            // Remove old visual and add new visual in to the existing panel
            if (preExistingPanel != null)
            {
                visual.Children.RemoveAt(1);
                visual.Children.Add(areaCanvas);
            }
            else
            {
                labelCanvas.SetValue(Canvas.ZIndexProperty, 1);
                visual.Children.Add(labelCanvas);
                visual.Children.Add(areaCanvas);
            }

            PlotArea plotArea = chart.PlotArea;

            RectangleGeometry clipRectangle = new RectangleGeometry();
            clipRectangle.Rect = new Rect(0, plotArea.BorderThickness.Top - depth3d, width + depth3d, height + depth3d + chart.ChartArea.PLANK_THICKNESS - plotArea.BorderThickness.Bottom - plotArea.BorderThickness.Top);
            areaCanvas.Clip = clipRectangle;

            // Clip the label canvas

            clipRectangle = new RectangleGeometry();

            Double clipLeft = 0;
            Double clipTop = -depth3d - 4;
            Double clipWidth = width + depth3d;
            Double clipHeight = height + depth3d + chart.ChartArea.PLANK_THICKNESS + 10;

            GetClipCoordinates(chart, ref clipLeft, ref clipTop, ref clipWidth, ref clipHeight, minimumXValue, maximumXValue);

            clipRectangle.Rect = new Rect(clipLeft, clipTop, clipWidth, clipHeight);

            labelCanvas.Clip = clipRectangle;

            return visual;
        }

        internal static void GetClipCoordinates(Chart chart, ref Double clipLeft, ref Double clipTop, ref Double clipWidth, ref Double clipHeight, Double minimumXValue, Double maximumXValue)
        {
            Double tickLengthOfAxisX = 0, tickLengthOfPrimaryAxisY = 0, tickLengthOfSecondaryAxisY = 0;
            Axis.CalculateTotalTickLength(chart, ref tickLengthOfAxisX, ref tickLengthOfPrimaryAxisY, ref tickLengthOfSecondaryAxisY);

            if (minimumXValue >= chart.ChartArea.AxisX.InternalAxisMinimum)
            {
                clipLeft -= tickLengthOfPrimaryAxisY;
                clipWidth += tickLengthOfPrimaryAxisY;
            }

            if (maximumXValue <= chart.ChartArea.AxisX.InternalAxisMaximum)
            {
                clipWidth += tickLengthOfSecondaryAxisY;
            }
        }

        /// <summary>
        /// Get visual of Area 3D
        /// </summary>
        /// <param name="faces">Faces</param>
        /// <param name="areaParams">AreaParams</param>
        /// <returns>Canvas</returns>
        internal static Canvas Get3DArea(DataSeries currentDataSeries,  ref Faces faces, PolygonalChartShapeParams areaParams)
        {
            Canvas visual = new Canvas();

            visual.Width = areaParams.Size.Width;
            visual.Height = areaParams.Size.Height;

            Point centroid;
            Brush sideBrush = areaParams.Lighting ? Graphics.GetRightFaceBrush(areaParams.Background) : areaParams.Background;
            Brush topBrush = areaParams.Lighting ? Graphics.GetTopFaceBrush(areaParams.Background) : areaParams.Background;
            Int32 pointIndexLimit = areaParams.IsPositive ? areaParams.Points.Count - 1 : areaParams.Points.Count;

            Canvas polygonSet = new Canvas();
            Rect size = GetBounds(areaParams.Points);
            polygonSet.Width = size.Width + areaParams.Depth3D;
            polygonSet.Height = size.Height + areaParams.Depth3D;
            polygonSet.SetValue(Canvas.TopProperty, size.Top - areaParams.Depth3D);
            polygonSet.SetValue(Canvas.LeftProperty, size.Left);
            visual.Children.Add(polygonSet);

            for (Int32 i = 0; i < pointIndexLimit; i++)
            {
                Polygon sides = new Polygon() { Tag = new ElementData() { Element = areaParams.TagReference } };
                PointCollection points = new PointCollection();
                Int32 index1 = i % areaParams.Points.Count;
                Int32 index2 = (i + 1) % areaParams.Points.Count;

                points.Add(areaParams.Points[index1]);
                points.Add(areaParams.Points[index2]);
                points.Add(new Point(areaParams.Points[index2].X + areaParams.Depth3D, areaParams.Points[index2].Y - areaParams.Depth3D));
                points.Add(new Point(areaParams.Points[index1].X + areaParams.Depth3D, areaParams.Points[index1].Y - areaParams.Depth3D));
                sides.Points = points;

                centroid = GetCentroid(points);
                Int32 zindex = GetAreaZIndex(centroid.X, centroid.Y, areaParams.IsPositive);
                sides.SetValue(Canvas.ZIndexProperty, zindex);

                if (i == (areaParams.Points.Count - 2))
                {
                    sides.Fill = sideBrush;
                    (sides.Tag as ElementData).VisualElementName = "Side";
                }
                else
                {
                    sides.Fill = topBrush;
                    (sides.Tag as ElementData).VisualElementName = "Top";
                }

                sides.Stroke = areaParams.BorderColor;
                sides.StrokeDashArray = areaParams.BorderStyle != null ? ExtendedGraphics.CloneCollection(areaParams.BorderStyle) : areaParams.BorderStyle;
                sides.StrokeThickness = areaParams.BorderThickness;
                sides.StrokeMiterLimit = 1;

                Rect sidesBounds = GetBounds(points);
                sides.Stretch = Stretch.Fill;
                sides.Width = sidesBounds.Width;
                sides.Height = sidesBounds.Height;
                sides.SetValue(Canvas.TopProperty, sidesBounds.Y - (size.Top - areaParams.Depth3D));
                sides.SetValue(Canvas.LeftProperty, sidesBounds.X - size.X);

                faces.Parts.Add(sides);
                polygonSet.Children.Add(sides);

            }

            Polygon polygon = new Polygon() { Tag = new ElementData() { Element = areaParams.TagReference, VisualElementName = "AreaBase" } };

            faces.Parts.Add(polygon);
            centroid = GetCentroid(areaParams.Points);

            polygon.SetValue(Canvas.ZIndexProperty, (Int32)centroid.Y + 1000);
            polygon.Fill = areaParams.Lighting ? Graphics.GetFrontFaceBrush(areaParams.Background) : areaParams.Background;

            polygon.Stroke = areaParams.BorderColor;
            polygon.StrokeDashArray = areaParams.BorderStyle != null ? ExtendedGraphics.CloneCollection(areaParams.BorderStyle) : areaParams.BorderStyle;
            polygon.StrokeThickness = areaParams.BorderThickness;
            polygon.StrokeMiterLimit = 1;

            polygon.Points = areaParams.Points;

            polygon.Stretch = Stretch.Fill;
            polygon.Width = size.Width;
            polygon.Height = size.Height;
            polygon.SetValue(Canvas.TopProperty, areaParams.Depth3D);
            polygon.SetValue(Canvas.LeftProperty, 0.0);

            // apply area animation
            if (areaParams.AnimationEnabled)
            {
                // apply animation to the entire canvas that was used to create the area
                areaParams.Storyboard = ApplyAreaAnimation(currentDataSeries, polygonSet, areaParams.Storyboard, areaParams.IsPositive, 0);

            }

            polygonSet.Children.Add(polygon);

            return visual;
        }

        ///// <summary>
        ///// Get visual object for area chart
        ///// </summary>
        ///// <param name="width">Width of the PlotArea</param>
        ///// <param name="height">Height of the PlotArea</param>
        ///// <param name="plotDetails">PlotDetails</param>
        ///// <param name="seriesList">List of DataSeries with render as area chart</param>
        ///// <param name="chart">Chart</param>
        ///// <param name="plankDepth">PlankDepth</param>
        ///// <param name="animationEnabled">Whether animation is enabled for chart</param>
        ///// <returns>Area chart canvas</returns>
        /*  internal static Canvas GetVisualObjectForAreaChart(Double width, Double height, PlotDetails plotDetails, List<DataSeries> seriesList, Chart chart, Double plankDepth, bool animationEnabled)
        {
            if (Double.IsNaN(width) || Double.IsNaN(height) || width <= 0 || height <= 0)
                return null;

            Boolean plankDrawn = false;

            Canvas visual = new Canvas() { Width = width, Height = height };
            Canvas labelCanvas = new Canvas() { Width = width, Height = height };
            Canvas areaCanvas = new Canvas() { Width = width, Height = height };

            Double depth3d = plankDepth / plotDetails.Layer3DCount * (chart.View3D ? 1 : 0);
            Double visualOffset = depth3d * (plotDetails.SeriesDrawingIndex[seriesList[0]] + 1);
            areaCanvas.SetValue(Canvas.TopProperty, visualOffset);
            areaCanvas.SetValue(Canvas.LeftProperty, -visualOffset);
            labelCanvas.SetValue(Canvas.TopProperty, visualOffset);
            labelCanvas.SetValue(Canvas.LeftProperty, -visualOffset);

            Marker marker;

            foreach (DataSeries series in seriesList)
            {
                if (series.Enabled == false)
                    continue;

                if (series.Storyboard == null)
                    series.Storyboard = new Storyboard();
                
                CurrentDataSeries = series;
                
                PlotGroup plotGroup = series.PlotGroup;
                
                Brush areaBrush = series.Color;
                
                Double limitingYValue = 0;
                
                if (plotGroup.AxisY.InternalAxisMinimum > 0)
                    limitingYValue = (Double)plotGroup.AxisY.InternalAxisMinimum;
                
                if (plotGroup.AxisY.InternalAxisMaximum < 0)
                    limitingYValue = (Double)plotGroup.AxisY.InternalAxisMaximum;
                
                PolygonalChartShapeParams areaParams = GetAreaParms(series, areaBrush, depth3d);
                
                areaParams.Storyboard = series.Storyboard;
                areaParams.AnimationEnabled = animationEnabled;
                areaParams.Size = new Size(width, height);
                
                PointCollection points = new PointCollection();
                
                List<DataPoint> enabledDataPoints = (from datapoint in series.InternalDataPoints where datapoint.Enabled == true select datapoint).ToList();
                
                if (enabledDataPoints.Count <= 0)
                    continue;
                
                Faces faces = new Faces();
                series.Faces = faces;
                series.Faces.Parts = new List<DependencyObject>();

                DataPoint currentDataPoint = enabledDataPoints[0];
                DataPoint nextDataPoint;

                Double xPosition = Graphics.ValueToPixelPosition(0, width, (Double)plotGroup.AxisX.InternalAxisMinimum, (Double)plotGroup.AxisX.InternalAxisMaximum, currentDataPoint.InternalXValue);
                Double yPosition = Graphics.ValueToPixelPosition(height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, limitingYValue);

                points.Add(new Point(xPosition, yPosition));

                for (Int32 i = 0; i < enabledDataPoints.Count - 1; i++)
                {
                    currentDataPoint = enabledDataPoints[i];
                    nextDataPoint = enabledDataPoints[i + 1];

                    if (Double.IsNaN(currentDataPoint.InternalYValue)) continue;

                    xPosition = Graphics.ValueToPixelPosition(0, width, (Double)plotGroup.AxisX.InternalAxisMinimum, (Double)plotGroup.AxisX.InternalAxisMaximum, currentDataPoint.InternalXValue);
                    yPosition = Graphics.ValueToPixelPosition(height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, currentDataPoint.InternalYValue);

                    points.Add(new Point(xPosition, yPosition));

                    marker = GetMarkerForDataPoint(chart, currentDataPoint, yPosition, currentDataPoint.InternalYValue > 0);

                    if (marker != null)
                    {
                        marker.AddToParent(labelCanvas, xPosition, yPosition, new Point(0.5, 0.5));

                        // Apply marker animation
                        if (animationEnabled)
                        {
                            if (currentDataPoint.Parent.Storyboard == null)
                                currentDataPoint.Parent.Storyboard = new Storyboard();

                            // Apply marker animation
                            currentDataPoint.Parent.Storyboard = AnimationHelper.ApplyOpacityAnimation(marker, CurrentDataSeries, currentDataPoint.Parent.Storyboard, 1, currentDataPoint.Opacity * currentDataPoint.Parent.Opacity);
                        }
                    }

                    if (Math.Sign(currentDataPoint.InternalYValue) != Math.Sign(nextDataPoint.InternalYValue))
                    {
                        Double xNextPosition = Graphics.ValueToPixelPosition(0, width, (Double)plotGroup.AxisX.InternalAxisMinimum, (Double)plotGroup.AxisX.InternalAxisMaximum, nextDataPoint.InternalXValue);
                        Double yNextPosition = Graphics.ValueToPixelPosition(height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, nextDataPoint.InternalYValue);

                        Double limitingYPosition = Graphics.ValueToPixelPosition(height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, limitingYValue);
                        Double xNew = Graphics.ConvertScale(yPosition, yNextPosition, limitingYPosition, xPosition, xNextPosition);
                        Double yNew = Graphics.ValueToPixelPosition(height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, limitingYValue);

                        points.Add(new Point(xNew, yNew));

                        // get the faces
                        areaParams.Points = points;
                        areaParams.IsPositive = (currentDataPoint.InternalYValue > 0);

                        if (chart.View3D)
                        {
                            Canvas areaVisual3D = Get3DArea(ref faces, areaParams);
                            areaVisual3D.SetValue(Canvas.ZIndexProperty, GetAreaZIndex(xPosition, yPosition, areaParams.IsPositive));
                            areaCanvas.Children.Add(areaVisual3D);
                            series.Faces.VisualComponents.Add(areaVisual3D);
                        }
                        else
                        {
                            areaCanvas.Children.Add(Get2DArea(ref faces, areaParams));
                            series.Faces.VisualComponents.Add(areaCanvas);
                        }

                        points = new PointCollection();
                        points.Add(new Point(xNew, yNew));
                    }
                }

                DataPoint lastDataPoint = enabledDataPoints[enabledDataPoints.Count - 1];

                xPosition = Graphics.ValueToPixelPosition(0, width, (Double)plotGroup.AxisX.InternalAxisMinimum, (Double)plotGroup.AxisX.InternalAxisMaximum, lastDataPoint.InternalXValue);
                yPosition = Graphics.ValueToPixelPosition(height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, lastDataPoint.InternalYValue);

                points.Add(new Point(xPosition, yPosition));

                marker = GetMarkerForDataPoint(chart, lastDataPoint, yPosition, lastDataPoint.InternalYValue > 0);

                if (marker != null)
                {
                    marker.AddToParent(labelCanvas, xPosition, yPosition, new Point(0.5, 0.5));
                    // Apply marker animation
                    if (animationEnabled)
                    {
                        if (lastDataPoint.Parent.Storyboard == null)
                            lastDataPoint.Parent.Storyboard = new Storyboard();

                        // Apply marker animation
                        lastDataPoint.Parent.Storyboard = AnimationHelper.ApplyOpacityAnimation(marker, CurrentDataSeries, lastDataPoint.Parent.Storyboard, 1, lastDataPoint.Opacity * lastDataPoint.Parent.Opacity);
                    }
                }

                xPosition = Graphics.ValueToPixelPosition(0, width, (Double)plotGroup.AxisX.InternalAxisMinimum, (Double)plotGroup.AxisX.InternalAxisMaximum, lastDataPoint.InternalXValue);
                yPosition = Graphics.ValueToPixelPosition(height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, limitingYValue);

                points.Add(new Point(xPosition, yPosition));

                // Get the faces
                areaParams.Points = points;
                areaParams.IsPositive = (lastDataPoint.InternalYValue > 0);

                if (chart.View3D)
                {
                    Canvas areaVisual3D = Get3DArea(ref faces, areaParams);
                    areaVisual3D.SetValue(Canvas.ZIndexProperty, GetAreaZIndex(xPosition, yPosition, areaParams.IsPositive));
                    areaCanvas.Children.Add(areaVisual3D);
                    series.Faces.VisualComponents.Add(areaVisual3D);
                }
                else
                {
                    areaCanvas.Children.Add(Get2DArea(ref faces, areaParams));
                    series.Faces.VisualComponents.Add(areaCanvas);
                }

                if (!plankDrawn && chart.View3D && plotGroup.AxisY.InternalAxisMinimum < 0 && plotGroup.AxisY.InternalAxisMaximum > 0)
                {
                    RectangularChartShapeParams columnParams = new RectangularChartShapeParams();
                    columnParams.BackgroundBrush = new SolidColorBrush(Color.FromArgb((Byte)255, (Byte)127, (Byte)127, (Byte)127));
                    columnParams.Lighting = true;
                    columnParams.Size = new Size(width, 1);
                    columnParams.Depth = depth3d;

                    Faces zeroPlank = ColumnChart.Get3DColumn(columnParams);
                    Panel zeroPlankVisual = zeroPlank.Visual as Panel;

                    Double top = height - Graphics.ValueToPixelPosition(0, height, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, 0);
                    zeroPlankVisual.SetValue(Canvas.LeftProperty, (Double)0);
                    zeroPlankVisual.SetValue(Canvas.TopProperty, top);
                    zeroPlankVisual.SetValue(Canvas.ZIndexProperty, 0);
                    zeroPlankVisual.Opacity = 0.7;
                    areaCanvas.Children.Add(zeroPlankVisual);
                }

                series.Faces.Visual = visual;
                series.Faces.LabelCanvas = labelCanvas;
            }

            visual.Children.Add(areaCanvas);
            visual.Children.Add(labelCanvas);

            return visual;
        }
        */
        
        /// <summary>
        /// Get visual object for stacked area chart
        /// </summary>
        /// <param name="width">Width of the PlotArea</param>
        /// <param name="height">Height of the PlotArea</param>
        /// <param name="plotDetails">PlotDetails</param>
        /// <param name="seriesList">List of DataSeries with render as StackedArea chart</param>
        /// <param name="chart">Chart</param>
        /// <param name="plankDepth">PlankDepth</param>
        /// <param name="animationEnabled">Whether animation is enabled for chart</param>
        /// <returns>StackedArea chart canvas</returns>
        internal static Canvas GetVisualObjectForStackedAreaChart(Panel preExistingPanel, Double width, Double height, PlotDetails plotDetails, List<DataSeries> seriesList, Chart chart, Double plankDepth, bool animationEnabled)
        {
            if (Double.IsNaN(width) || Double.IsNaN(height) || width <= 0 || height <= 0)
                return null;

            DataSeries currentDataSeries;

            Boolean plankDrawn = false;
            Double depth3d = plankDepth / plotDetails.Layer3DCount * (chart.View3D ? 1 : 0);
            Double visualOffset = depth3d * (plotDetails.SeriesDrawingIndex[seriesList[0]] + 1);

            if (Double.IsNaN(visualOffset) || Double.IsInfinity(visualOffset))
                return null;

            Canvas visual, labelCanvas, areaCanvas;
            RenderHelper.RepareCanvas4Drawing(preExistingPanel as Canvas, out visual, out labelCanvas, out areaCanvas, width, height);
            //labelCanvas.Background = Graphics.GetRandomColor();
            //Canvas visual = new Canvas() { Width = width, Height = height };
            //Canvas labelCanvas = new Canvas() { Width = width, Height = height };
            //Canvas areaCanvas = new Canvas() { Width = width, Height = height };

            visual.SetValue(Canvas.TopProperty, visualOffset);
            visual.SetValue(Canvas.LeftProperty, -visualOffset);

            //labelCanvas.SetValue(Canvas.TopProperty, (Double) 0);
            //labelCanvas.SetValue(Canvas.LeftProperty,(Double)  0);
            //labelCanvas.SetValue(Canvas.ZIndexProperty, (Int32)1);

            var plotgroups = (from series in seriesList where series.PlotGroup != null select series.PlotGroup);

            if (plotgroups.Count() == 0)
                return visual;

            PlotGroup plotGroup = plotgroups.First();

            Dictionary<Double, List<Double>> dataPointValuesInStackedOrder = plotDetails.GetDataPointValuesInStackedOrder(plotGroup);

            Dictionary<Double, List<DataPoint>> dataPointInStackedOrder = plotDetails.GetDataPointInStackOrder(plotGroup);
            
            // Double[] xValues = dataPointValuesInStackedOrder.Keys.ToArray();
            Double[] xValues = RenderHelper.GetXValuesUnderViewPort(dataPointValuesInStackedOrder.Keys.ToList(), plotGroup.AxisX, plotGroup.AxisY, false);

            Double minimumXValue = plotGroup.MinimumX;
            Double maximumXValue = plotGroup.MaximumX;

            Double limitingYValue = 0;
            if (plotGroup.AxisY.InternalAxisMinimum > 0)
                limitingYValue = (Double)plotGroup.AxisY.InternalAxisMinimum;
            if (plotGroup.AxisY.InternalAxisMaximum < 0)
                limitingYValue = (Double)plotGroup.AxisY.InternalAxisMaximum;

            foreach (DataSeries ds in seriesList)
            {
                ds.Faces = null;

                if (ds.ToolTipElement != null)
                    ds.ToolTipElement.Hide();
            }

            Double limitingYPosition = Graphics.ValueToPixelPosition(height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, limitingYValue);

            Marker marker;

            for (Int32 i = 0; i < xValues.Length - 1; i++)
            {
                List<Double> curYValues = dataPointValuesInStackedOrder[xValues[i]];
                List<Double> nextYValues = dataPointValuesInStackedOrder[xValues[i + 1]];

                Double curBase = limitingYValue;
                Double nextBase = limitingYValue;

                List<DataPoint> curDataPoints = dataPointInStackedOrder[xValues[i]];
                List<DataPoint> nextDataPoints = dataPointInStackedOrder[xValues[i + 1]];

                for (Int32 index = 0; index < curYValues.Count; index++)
                {
                    if (index >= nextYValues.Count || index >= curYValues.Count || curDataPoints[index] == null || nextDataPoints[index] == null)
                        continue;

                    Double curXPosition = Graphics.ValueToPixelPosition(0, width, (Double)plotGroup.AxisX.InternalAxisMinimum, (Double)plotGroup.AxisX.InternalAxisMaximum, xValues[i]);
                    Double nextXPosition = Graphics.ValueToPixelPosition(0, width, (Double)plotGroup.AxisX.InternalAxisMinimum, (Double)plotGroup.AxisX.InternalAxisMaximum, xValues[i + 1]);

                    Double totalOfCurrSucessiveDpValues = 0;
                    Double totalOfNextSucessiveDpValues = 0;

                    if (plotGroup.AxisY.Logarithmic)
                    {
                        totalOfCurrSucessiveDpValues = Math.Pow(plotGroup.AxisY.LogarithmBase, curBase) + Math.Pow(plotGroup.AxisY.LogarithmBase, curYValues[index]);
                        totalOfCurrSucessiveDpValues = Math.Log(totalOfCurrSucessiveDpValues, plotGroup.AxisY.LogarithmBase);

                        totalOfNextSucessiveDpValues = Math.Pow(plotGroup.AxisY.LogarithmBase, nextBase) + Math.Pow(plotGroup.AxisY.LogarithmBase, nextYValues[index]);
                        totalOfNextSucessiveDpValues = Math.Log(totalOfNextSucessiveDpValues, plotGroup.AxisY.LogarithmBase);
                    }
                    else
                    {
                        totalOfCurrSucessiveDpValues = curBase + curYValues[index];
                        totalOfNextSucessiveDpValues = nextBase + nextYValues[index];
                    }

                    Double curYPosition = Graphics.ValueToPixelPosition(height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, totalOfCurrSucessiveDpValues);
                    Double nextYPosition = Graphics.ValueToPixelPosition(height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, totalOfNextSucessiveDpValues);
                    Double curYBase = Graphics.ValueToPixelPosition(height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, curBase);
                    Double nextYBase = Graphics.ValueToPixelPosition(height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, nextBase);

                    Point intersect = GetIntersection(new Point(curXPosition, curYBase), new Point(nextXPosition, nextYBase),
                                                new Point(curXPosition, curYPosition), new Point(nextXPosition, nextYPosition));

                    Boolean isTopOfStack = false;
                    if (index == curYValues.Count - 1)
                        isTopOfStack = true;

                    marker = GetMarkerForDataPoint(chart, height, isTopOfStack, curDataPoints[index], curYPosition, curDataPoints[index].InternalYValue > 0);
                    if (marker != null)
                    {
                        if (curDataPoints[index].Parent.Storyboard == null)
                            curDataPoints[index].Parent.Storyboard = new Storyboard();

                        currentDataSeries = curDataPoints[index].Parent;

                        marker.AddToParent(labelCanvas, curXPosition, curYPosition, new Point(0.5, 0.5));
                        // Apply marker animation
                        if (animationEnabled)
                            curDataPoints[index].Parent.Storyboard = AnimationHelper.ApplyOpacityAnimation(marker, currentDataSeries, curDataPoints[index].Parent.Storyboard, 1, curDataPoints[index].Opacity * curDataPoints[index].Parent.Opacity);
                    }

                    curDataPoints[index]._visualPosition = new Point(curXPosition, curYPosition);

                    if (i + 1 == xValues.Length - 1)
                    {
                        marker = GetMarkerForDataPoint(chart, height, isTopOfStack, nextDataPoints[index], nextYPosition, nextDataPoints[index].InternalYValue > 0);
                        if (marker != null)
                        {
                            if (nextDataPoints[index].Parent.Storyboard == null)
                                nextDataPoints[index].Parent.Storyboard = new Storyboard();

                            currentDataSeries = nextDataPoints[index].Parent;

                            marker.AddToParent(labelCanvas, nextXPosition, nextYPosition, new Point(0.5, 0.5));
                            // Apply marker animation
                            if (animationEnabled)
                                nextDataPoints[index].Parent.Storyboard = AnimationHelper.ApplyOpacityAnimation(marker, currentDataSeries, nextDataPoints[index].Parent.Storyboard, 1, nextDataPoints[index].Opacity * nextDataPoints[index].Parent.Opacity);
                        }

                        nextDataPoints[index]._visualPosition = new Point(nextXPosition, nextYPosition);
                    }

                    if (curDataPoints[index].Parent.Faces == null)
                    {
                        curDataPoints[index].Parent.Faces = new Faces();
                    }

                    List<PointCollection> pointSet = null;
                    if ((!Double.IsNaN(intersect.X) && !Double.IsInfinity(intersect.X)) && (intersect.X >= curXPosition && intersect.X <= nextXPosition))
                    {
                        List<PointCollection> set1 = GeneratePointsCollection(curXPosition, curYPosition, curYBase, intersect.X, intersect.Y, intersect.Y, limitingYPosition);
                        List<PointCollection> set2 = GeneratePointsCollection(intersect.X, intersect.Y, intersect.Y, nextXPosition, nextYPosition, nextYBase, limitingYPosition);

                        pointSet = set1;
                        pointSet.InsertRange(pointSet.Count, set2);
                    }
                    else
                    {
                        pointSet = GeneratePointsCollection(curXPosition, curYPosition, curYBase, nextXPosition, nextYPosition, nextYBase, limitingYPosition);
                    }

                    DataSeries series = curDataPoints[index].Parent;
                    Brush areaBrush = series.Color;

                    currentDataSeries = series;

                    PolygonalChartShapeParams areaParams = GetAreaParms(series, areaBrush, depth3d);

                    // This is used to set some pixel padding for the last two points of a PointCollection
                    Double pixelPadding = 0.8;

                    foreach (PointCollection points in pointSet)
                    {
                        points[points.Count - 2] = new Point(points[points.Count - 2].X + pixelPadding, points[points.Count - 2].Y);
                        points[points.Count - 1] = new Point(points[points.Count - 1].X + pixelPadding, points[points.Count - 1].Y);

                        areaParams.Points = points;
                        
                        Faces faces = curDataPoints[index].Parent.Faces;
                        if (faces.Parts == null)
                            faces.Parts = new List<DependencyObject>();

                        if (chart.View3D)
                        {   
                            Point centroid = GetCentroid(points);
                            areaParams.IsPositive = centroid.Y < limitingYPosition;
                            Canvas frontface = GetStacked3DAreaFrontFace(ref faces, areaParams);
                            Canvas sideface = GetStacked3DSideFaces(ref faces, areaParams);
                            sideface.SetValue(Canvas.ZIndexProperty, GetStackedAreaZIndex(centroid.X, centroid.Y, areaParams.IsPositive, index));
                            frontface.SetValue(Canvas.ZIndexProperty, 50000);
                            areaCanvas.Children.Add(sideface);
                            areaCanvas.Children.Add(frontface);
                            curDataPoints[index].Parent.Faces.VisualComponents.Add(sideface);
                            curDataPoints[index].Parent.Faces.VisualComponents.Add(frontface);

                            // Apply Animation
                            if (animationEnabled)
                            {
                                if (curDataPoints[index].Parent.Storyboard == null)
                                    curDataPoints[index].Parent.Storyboard = new Storyboard();

                                Storyboard storyboard = curDataPoints[index].Parent.Storyboard;

                                // apply animation to the various faces
                                storyboard = ApplyStackedAreaAnimation(currentDataSeries, sideface, storyboard, (1.0 / seriesList.Count) * (seriesList.IndexOf(curDataPoints[index].Parent)) + 0.05, 1.0 / seriesList.Count);
                                storyboard = ApplyStackedAreaAnimation(currentDataSeries, frontface, storyboard, (1.0 / seriesList.Count) * (seriesList.IndexOf(curDataPoints[index].Parent)), 1.0 / seriesList.Count);
                            }
                        }
                        else
                        {
                            Canvas area2d = GetStacked2DArea(ref faces, areaParams);
                            areaCanvas.Children.Add(area2d);
                            curDataPoints[index].Parent.Faces.VisualComponents.Add(area2d);
                            if (animationEnabled)
                            {
                                if (curDataPoints[index].Parent.Storyboard == null)
                                    curDataPoints[index].Parent.Storyboard = new Storyboard();
                                Storyboard storyboard = curDataPoints[index].Parent.Storyboard;

                                // apply animation to the various faces
                                storyboard = ApplyStackedAreaAnimation(currentDataSeries, area2d, storyboard, (1.0 / seriesList.Count) * (seriesList.IndexOf(curDataPoints[index].Parent)), 1.0 / seriesList.Count);
                            }
                        }

                        curDataPoints[index].Parent.Faces.Visual = visual;

                        if (!VisifireControl.IsXbapApp)
                        {
                            if ((Boolean)series.ShadowEnabled)
                            {
                                if (series.Faces != null && series.Faces.Visual != null)
                                {
                                    series.Faces.Visual.Effect = ExtendedGraphics.GetShadowEffect(135, 2, 1);
                                }
                            }
                        }
                    }

                    curBase += curYValues[index];
                    nextBase += nextYValues[index];
                }
            }

            if (xValues.Count() > 0)
            {
                if (!plankDrawn && chart.View3D && plotGroup.AxisY.InternalAxisMinimum < 0 && plotGroup.AxisY.InternalAxisMaximum > 0)
                {
                    //RectangularChartShapeParams columnParams = new RectangularChartShapeParams();
                    //Brush brush = new SolidColorBrush(Color.FromArgb((Byte)255, (Byte)255, (Byte)255, (Byte)255));
                    //columnParams.Lighting = true;
                    //columnParams.Size = new Size(width, 1);
                    //columnParams.Depth = depth3d;

                    Brush frontBrush, topBrush, rightBrush;
                    ExtendedGraphics.GetBrushesForPlank(chart, out frontBrush, out topBrush, out rightBrush, true);

                    Faces zeroPlank = ColumnChart.Get3DPlank(width, 1, depth3d, frontBrush, topBrush, rightBrush);
                    Panel zeroPlankVisual = zeroPlank.Visual as Panel;

                    Double top = height - Graphics.ValueToPixelPosition(0, height, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, 0);
                    zeroPlankVisual.SetValue(Canvas.LeftProperty, (Double)0);
                    zeroPlankVisual.SetValue(Canvas.TopProperty, top);
                    zeroPlankVisual.SetValue(Canvas.ZIndexProperty, 0);
                    zeroPlankVisual.Opacity = 0.7;
                    visual.Children.Add(zeroPlankVisual);
                }
            }

            //visual.Children.Add(areaCanvas);
            //visual.Children.Add(labelCanvas);

            chart.ChartArea.DisableIndicators();

            // Remove old visual and add new visual in to the existing panel
            if (preExistingPanel != null)
            {
                visual.Children.RemoveAt(1);
                visual.Children.Add(areaCanvas);
            }
            else
            {
                labelCanvas.SetValue(Canvas.ZIndexProperty, 1);
                visual.Children.Add(labelCanvas);
                visual.Children.Add(areaCanvas);
            }

            PlotArea plotArea = chart.PlotArea;

            RectangleGeometry clipRectangle = new RectangleGeometry();
            clipRectangle.Rect = new Rect(0, plotArea.BorderThickness.Top - depth3d, width + depth3d, height + depth3d + chart.ChartArea.PLANK_THICKNESS - plotArea.BorderThickness.Bottom - plotArea.BorderThickness.Top);
            areaCanvas.Clip = clipRectangle;

            // Clip the label canvas

            clipRectangle = new RectangleGeometry();

            Double clipLeft = 0;
            Double clipTop = -depth3d - 4;
            Double clipWidth = width + depth3d;
            Double clipHeight = height + depth3d + chart.ChartArea.PLANK_THICKNESS + 10;

            GetClipCoordinates(chart, ref clipLeft, ref clipTop, ref clipWidth, ref clipHeight, minimumXValue, maximumXValue);

            clipRectangle.Rect = new Rect(clipLeft, clipTop, clipWidth, clipHeight);
            labelCanvas.Clip = clipRectangle;

            return visual;
        }

        /// <summary>
        /// Get visual object for stacked area100 chart
        /// </summary>
        /// <param name="width">Width of the PlotArea</param>
        /// <param name="height">Height of the PlotArea</param>
        /// <param name="plotDetails">PlotDetails</param>
        /// <param name="seriesList">List of DataSeries with render as StackedArea100 chart</param>
        /// <param name="chart">Chart</param>
        /// <param name="plankDepth">PlankDepth</param>
        /// <param name="animationEnabled">Whether animation is enabled for chart</param>
        /// <returns>StackedArea100 chart canvas</returns>
        internal static Canvas GetVisualObjectForStackedArea100Chart(Panel preExistingPanel,Double width, Double height, PlotDetails plotDetails, List<DataSeries> seriesList, Chart chart, Double plankDepth, bool animationEnabled)
        {
            if (Double.IsNaN(width) || Double.IsNaN(height) || width <= 0 || height <= 0)
                return null;

            DataSeries currentDataSeries;
            Boolean plankDrawn = false;
            Double depth3d = plankDepth / plotDetails.Layer3DCount * (chart.View3D ? 1 : 0);
            Double visualOffset = depth3d * (plotDetails.SeriesDrawingIndex[seriesList[0]] + 1);

            if (Double.IsNaN(visualOffset) || Double.IsInfinity(visualOffset))
                return null;

            Canvas visual, labelCanvas, areaCanvas;
            RenderHelper.RepareCanvas4Drawing(preExistingPanel as Canvas, out visual, out labelCanvas, out areaCanvas, width, height);
           
            //Canvas visual = new Canvas() { Width = width, Height = height };
            //Canvas labelCanvas = new Canvas() { Width = width, Height = height };
            //Canvas areaCanvas = new Canvas() { Width = width, Height = height };

            visual.SetValue(Canvas.TopProperty, visualOffset);
            visual.SetValue(Canvas.LeftProperty, -visualOffset);

            labelCanvas.SetValue(Canvas.TopProperty, (Double)0);
            labelCanvas.SetValue(Canvas.LeftProperty, (Double)0);
            labelCanvas.SetValue(Canvas.ZIndexProperty, (Int32)1);

            var plotgroups = (from series in seriesList where series.PlotGroup != null select series.PlotGroup);

            if (plotgroups.Count() == 0)
                return visual;

            PlotGroup plotGroup = plotgroups.First();

            Dictionary<Double, List<Double>> dataPointValuesInStackedOrder = plotDetails.GetDataPointValuesInStackedOrder(plotGroup);

            Dictionary<Double, List<DataPoint>> dataPointInStackedOrder = plotDetails.GetDataPointInStackOrder(plotGroup);

            //Double[] xValues = RenderHelper.GetXValuesUnderViewPort(dataPointValuesInStackedOrder.Keys.ToList(), plotGroup.AxisX, plotGroup.AxisY, true);
            Double[] xValues = dataPointValuesInStackedOrder.Keys.ToArray();

            Double minimumXValue = plotGroup.MinimumX;
            Double maximumXValue = plotGroup.MaximumX;

            Double limitingYValue = 0;

            if (plotGroup.AxisY.InternalAxisMinimum > 0)
                limitingYValue = (Double)plotGroup.AxisY.InternalAxisMinimum;
            if (plotGroup.AxisY.InternalAxisMaximum < 0)
                limitingYValue = (Double)plotGroup.AxisY.InternalAxisMaximum;

            foreach (DataSeries ds in seriesList)
            {
                ds.Faces = null;

                if (ds.ToolTipElement != null)
                    ds.ToolTipElement.Hide();
            }

            Double limitingYPosition = Graphics.ValueToPixelPosition(height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, limitingYValue);

            Marker marker;

            for (Int32 i = 0; i < xValues.Length - 1; i++)
            {
                List<Double> curYValues = dataPointValuesInStackedOrder[xValues[i]];
                List<Double> nextYValues = dataPointValuesInStackedOrder[xValues[i + 1]];

                Double curBase = limitingYValue;
                Double nextBase = limitingYValue;
                Double curAbsoluteSum = plotGroup.XWiseStackedDataList[xValues[i]].AbsoluteYValueSum;
                Double nextAbsoluteSum = plotGroup.XWiseStackedDataList[xValues[i + 1]].AbsoluteYValueSum;

                List<DataPoint> curDataPoints = dataPointInStackedOrder[xValues[i]];

                List<DataPoint> nextDataPoints = dataPointInStackedOrder[xValues[i + 1]];

                if (Double.IsNaN(curAbsoluteSum))
                    curAbsoluteSum = 1;

                if (Double.IsNaN(nextAbsoluteSum))
                    nextAbsoluteSum = 1;

                for (Int32 index = 0; index < curYValues.Count; index++)
                {
                    if (index >= nextYValues.Count || index >= curYValues.Count || curDataPoints[index] == null || nextDataPoints[index] == null)
                        continue;

                    Double curPercentageY = 0;
                    Double nextPercentageY = 0;

                    if (Double.IsNaN(nextPercentageY) || Double.IsNaN(curPercentageY))
                        continue;

                    Double percentOfCurrSucessiveDpValues = 0;
                    Double percentOfNextSucessiveDpValues = 0;

                    if (plotGroup.AxisY.Logarithmic)
                    {
                        curPercentageY = Math.Pow(plotGroup.AxisY.LogarithmBase, curYValues[index]) / curAbsoluteSum * 100;

                        nextPercentageY = Math.Pow(plotGroup.AxisY.LogarithmBase, nextYValues[index]) / nextAbsoluteSum * 100;

                        percentOfCurrSucessiveDpValues = Math.Pow(plotGroup.AxisY.LogarithmBase, curBase) + curPercentageY;
                        percentOfCurrSucessiveDpValues = Math.Log(percentOfCurrSucessiveDpValues, plotGroup.AxisY.LogarithmBase);

                        percentOfNextSucessiveDpValues = Math.Pow(plotGroup.AxisY.LogarithmBase, nextBase) + nextPercentageY;
                        percentOfNextSucessiveDpValues = Math.Log(percentOfNextSucessiveDpValues, plotGroup.AxisY.LogarithmBase);

                        curPercentageY = Math.Log(curPercentageY, plotGroup.AxisY.LogarithmBase);
                        nextPercentageY = Math.Log(nextPercentageY, plotGroup.AxisY.LogarithmBase);


                    }
                    else
                    {
                        curPercentageY = curYValues[index] / curAbsoluteSum * 100;
                        nextPercentageY = nextYValues[index] / nextAbsoluteSum * 100;

                        percentOfCurrSucessiveDpValues = curBase + curPercentageY;
                        percentOfNextSucessiveDpValues = nextBase + nextPercentageY;
                    }

                    Double curXPosition = Graphics.ValueToPixelPosition(0, width, (Double)plotGroup.AxisX.InternalAxisMinimum, (Double)plotGroup.AxisX.InternalAxisMaximum, xValues[i]);
                    Double nextXPosition = Graphics.ValueToPixelPosition(0, width, (Double)plotGroup.AxisX.InternalAxisMinimum, (Double)plotGroup.AxisX.InternalAxisMaximum, xValues[i + 1]);
                    Double curYPosition = Graphics.ValueToPixelPosition(height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, percentOfCurrSucessiveDpValues);
                    Double nextYPosition = Graphics.ValueToPixelPosition(height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, percentOfNextSucessiveDpValues);
                    Double curYBase = Graphics.ValueToPixelPosition(height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, curBase);
                    Double nextYBase = Graphics.ValueToPixelPosition(height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, nextBase);

                    Point intersect = GetIntersection(new Point(curXPosition, curYBase), new Point(nextXPosition, nextYBase),
                                                new Point(curXPosition, curYPosition), new Point(nextXPosition, nextYPosition));

                    marker = GetMarkerForDataPoint(chart, height, false, curDataPoints[index], curYPosition, curDataPoints[index].InternalYValue > 0);
                    if (marker != null)
                    {
                        if (curDataPoints[index].Parent.Storyboard == null)
                            curDataPoints[index].Parent.Storyboard = new Storyboard();

                        currentDataSeries = curDataPoints[index].Parent;

                        marker.AddToParent(labelCanvas, curXPosition, curYPosition, new Point(0.5, 0.5));
                        // Apply marker animation
                        if (animationEnabled)
                            curDataPoints[index].Parent.Storyboard = AnimationHelper.ApplyOpacityAnimation(marker, currentDataSeries, curDataPoints[index].Parent.Storyboard, 1, curDataPoints[index].Opacity * curDataPoints[index].Parent.Opacity);
                    }

                    curDataPoints[index]._visualPosition = new Point(curXPosition, curYPosition);

                    if (i + 1 == xValues.Length - 1)
                    {
                        marker = GetMarkerForDataPoint(chart, height, false, nextDataPoints[index], nextYPosition, nextDataPoints[index].InternalYValue > 0);
                        if (marker != null)
                        {
                            if (curDataPoints[index].Parent.Storyboard == null)
                                curDataPoints[index].Parent.Storyboard = new Storyboard();

                            currentDataSeries = curDataPoints[index].Parent;

                            marker.AddToParent(labelCanvas, nextXPosition, nextYPosition, new Point(0.5, 0.5));
                            // Apply marker animation
                            if (animationEnabled)
                                nextDataPoints[index].Parent.Storyboard = AnimationHelper.ApplyOpacityAnimation(marker, currentDataSeries, nextDataPoints[index].Parent.Storyboard, 1, nextDataPoints[index].Opacity * nextDataPoints[index].Parent.Opacity);
                        }

                        nextDataPoints[index]._visualPosition = new Point(nextXPosition, nextYPosition);
                    }

                    if (curDataPoints[index].Parent.Faces == null)
                    {
                        curDataPoints[index].Parent.Faces = new Faces();
                    }

                    List<PointCollection> pointSet = null;
                    if ((!Double.IsNaN(intersect.X) && !Double.IsInfinity(intersect.X)) && (intersect.X >= curXPosition && intersect.X <= nextXPosition))
                    {
                        List<PointCollection> set1 = GeneratePointsCollection(curXPosition, curYPosition, curYBase, intersect.X, intersect.Y, intersect.Y, limitingYPosition);

                        List<PointCollection> set2 = GeneratePointsCollection(intersect.X, intersect.Y, intersect.Y, nextXPosition, nextYPosition, nextYBase, limitingYPosition);

                        pointSet = set1;
                        pointSet.InsertRange(pointSet.Count, set2);
                    }
                    else
                    {
                        pointSet = GeneratePointsCollection(curXPosition, curYPosition, curYBase, nextXPosition, nextYPosition, nextYBase, limitingYPosition);
                    }

                    DataSeries series = curDataPoints[index].Parent;
                    Brush areaBrush = series.Color;

                    currentDataSeries = series;

                    PolygonalChartShapeParams areaParams = GetAreaParms(series, areaBrush, depth3d);

                    Faces faces = curDataPoints[index].Parent.Faces;
                    if (faces.Parts == null)
                        faces.Parts = new List<DependencyObject>();

                    // This is used to set some pixel padding for the last two points of a PointCollection
                    Double pixelPadding = 0.8;

                    foreach (PointCollection points in pointSet)
                    {
                        points[points.Count - 2] = new Point(points[points.Count - 2].X + pixelPadding, points[points.Count - 2].Y);
                        points[points.Count - 1] = new Point(points[points.Count - 1].X + pixelPadding, points[points.Count - 1].Y);

                        areaParams.Points = points;

                        if (chart.View3D)
                        {
                            Point centroid = GetCentroid(points);
                            areaParams.IsPositive = centroid.Y < limitingYPosition;
                            Canvas frontface = GetStacked3DAreaFrontFace(ref faces, areaParams);
                            Canvas sideface = GetStacked3DSideFaces(ref faces, areaParams);
                            sideface.SetValue(Canvas.ZIndexProperty, GetStackedAreaZIndex(centroid.X, centroid.Y, areaParams.IsPositive, index));
                            frontface.SetValue(Canvas.ZIndexProperty, 50000);
                            areaCanvas.Children.Add(sideface);
                            areaCanvas.Children.Add(frontface);
                            curDataPoints[index].Parent.Faces.VisualComponents.Add(sideface);
                            curDataPoints[index].Parent.Faces.VisualComponents.Add(frontface);

                            // Apply Animation
                            if (animationEnabled)
                            {
                                if (curDataPoints[index].Parent.Storyboard == null)
                                    curDataPoints[index].Parent.Storyboard = new Storyboard();
                                Storyboard storyboard = curDataPoints[index].Parent.Storyboard;

                                // apply animation to the various faces
                                storyboard = ApplyStackedAreaAnimation(currentDataSeries, sideface, storyboard, (1.0 / seriesList.Count) * (seriesList.IndexOf(curDataPoints[index].Parent)) + 0.05, 1.0 / seriesList.Count);
                                storyboard = ApplyStackedAreaAnimation(currentDataSeries, frontface, storyboard, (1.0 / seriesList.Count) * (seriesList.IndexOf(curDataPoints[index].Parent)), 1.0 / seriesList.Count);
                            }
                        }
                        else
                        {
                            Canvas area2d = Get2DArea(currentDataSeries, ref faces, areaParams);
                            areaCanvas.Children.Add(area2d);
                            curDataPoints[index].Parent.Faces.VisualComponents.Add(area2d);
                            if (animationEnabled)
                            {
                                if (curDataPoints[index].Parent.Storyboard == null)
                                    curDataPoints[index].Parent.Storyboard = new Storyboard();
                                Storyboard storyboard = curDataPoints[index].Parent.Storyboard;

                                // apply animation to the various faces
                                storyboard = ApplyStackedAreaAnimation(currentDataSeries, area2d, storyboard, (1.0 / seriesList.Count) * (seriesList.IndexOf(curDataPoints[index].Parent)), 1.0 / seriesList.Count);
                            }
                        }
                        curDataPoints[index].Parent.Faces.Visual = visual;

                        if (!VisifireControl.IsXbapApp)
                        {
                            if ((Boolean)series.ShadowEnabled)
                            {
                                if (series.Faces != null && series.Faces.Visual != null)
                                {
                                    series.Faces.Visual.Effect = ExtendedGraphics.GetShadowEffect(135, 2, 1);
                                }
                            }
                        }
                    }
                    curBase += curPercentageY;
                    nextBase += nextPercentageY;
                }

            }

            if (xValues.Count() > 0)
            {
                if (!plankDrawn && chart.View3D && plotGroup.AxisY.InternalAxisMinimum < 0 && plotGroup.AxisY.InternalAxisMaximum > 0)
                {
                    //RectangularChartShapeParams columnParams = new RectangularChartShapeParams();
                    //columnParams.BackgroundBrush = new SolidColorBrush(Color.FromArgb((Byte)255, (Byte)255, (Byte)255, (Byte)255));
                    //columnParams.Lighting = true;
                    //columnParams.Size = new Size(width, 1);
                    //columnParams.Depth = depth3d;

                    Brush frontBrush, topBrush, rightBrush;
                    ExtendedGraphics.GetBrushesForPlank(chart, out frontBrush, out topBrush, out rightBrush, true);

                    Faces zeroPlank = ColumnChart.Get3DPlank(width, 1, depth3d, frontBrush, topBrush, rightBrush);
                    Panel zeroPlankVisual = zeroPlank.Visual as Panel;

                    Double top = height - Graphics.ValueToPixelPosition(0, height, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, 0);
                    zeroPlankVisual.SetValue(Canvas.LeftProperty, (Double)0);
                    zeroPlankVisual.SetValue(Canvas.TopProperty, top);
                    zeroPlankVisual.SetValue(Canvas.ZIndexProperty, 0);
                    zeroPlankVisual.Opacity = 0.7;
                    visual.Children.Add(zeroPlankVisual);
                }
            }

            chart.ChartArea.DisableIndicators();

            //visual.Children.Add(areaCanvas);
            //visual.Children.Add(labelCanvas);

            // Remove old visual and add new visual in to the existing panel
            if (preExistingPanel != null)
            {
                visual.Children.RemoveAt(1);
                visual.Children.Add(areaCanvas);
            }
            else
            {
                labelCanvas.SetValue(Canvas.ZIndexProperty, 1);
                visual.Children.Add(labelCanvas);
                visual.Children.Add(areaCanvas);
            }

            PlotArea plotArea = chart.PlotArea;

            RectangleGeometry clipRectangle = new RectangleGeometry();
            clipRectangle.Rect = new Rect(0, plotArea.BorderThickness.Top - depth3d, width + depth3d, height + depth3d + chart.ChartArea.PLANK_THICKNESS - plotArea.BorderThickness.Bottom - plotArea.BorderThickness.Top);
            areaCanvas.Clip = clipRectangle;

            // Clip the label canvas

            clipRectangle = new RectangleGeometry();

            Double clipLeft = 0;
            Double clipTop = -depth3d - 4;
            Double clipWidth = width + depth3d;
            Double clipHeight = height + depth3d + chart.ChartArea.PLANK_THICKNESS + 10;

            GetClipCoordinates(chart, ref clipLeft, ref clipTop, ref clipWidth, ref clipHeight, minimumXValue, maximumXValue);

            clipRectangle.Rect = new Rect(clipLeft, clipTop, clipWidth, clipHeight);
            labelCanvas.Clip = clipRectangle;

            return visual;
        }

        /// <summary>
        /// Get centroid of some points
        /// </summary>
        /// <param name="points">Point collection</param>
        /// <returns>Point</returns>
        internal static Point GetCentroid(PointCollection points)
        {
            Double sumX = 0;
            Double sumY = 0;
            foreach (Point point in points)
            {
                sumX += point.X;
                sumY += point.Y;
            }

            return new Point(sumX / points.Count, sumY / points.Count);
        }

        /// <summary>
        /// Generate points collection
        /// </summary>
        /// <param name="curX">Current X position</param>
        /// <param name="curY">Current Y position</param>
        /// <param name="curBase">Current base position</param>
        /// <param name="nextX">Next X position</param>
        /// <param name="nextY">Next Y position</param>
        /// <param name="nextBase">Next base position</param>
        /// <param name="limitingY">Limiting Y position</param>
        /// <returns>List of point collection</returns>
        internal static List<PointCollection> GeneratePointsCollection(Double curX, Double curY, Double curBase, Double nextX, Double nextY, Double nextBase, Double limitingY)
        {
            List<PointCollection> pointsSet = new List<PointCollection>();

            PointCollection points;

            if (curY < limitingY && nextY < limitingY && curBase > limitingY && nextBase < limitingY)
            {
                points = new PointCollection();
                points.Add(new Point(curX, limitingY));
                points.Add(new Point(curX, curY));
                points.Add(new Point(nextX, nextY));
                points.Add(new Point(nextX, nextBase));
                Double midX = Graphics.ConvertScale(curBase, nextBase, limitingY, curX, nextX);
                points.Add(new Point(midX, limitingY));

                pointsSet.Add(points);

                points = new PointCollection();

                points.Add(new Point(curX, limitingY));
                points.Add(new Point(midX, limitingY));
                points.Add(new Point(curX, curBase));

                pointsSet.Add(points);
            }
            else if (curY < limitingY && nextY > limitingY && curBase > limitingY && nextBase > limitingY)
            {
                points = new PointCollection();
                points.Add(new Point(curX, limitingY));
                points.Add(new Point(curX, curY));
                Double midX = Graphics.ConvertScale(curY, nextY, limitingY, curX, nextX);
                points.Add(new Point(midX, limitingY));

                pointsSet.Add(points);

                points = new PointCollection();

                points.Add(new Point(curX, curBase));
                points.Add(new Point(curX, limitingY));
                points.Add(new Point(midX, limitingY));
                points.Add(new Point(nextX, nextY));
                points.Add(new Point(nextX, nextBase));

                pointsSet.Add(points);
            }
            else if (curY < limitingY && nextY < limitingY && curBase < limitingY && nextBase > limitingY)
            {
                points = new PointCollection();
                Double midX = Graphics.ConvertScale(curBase, nextBase, limitingY, curX, nextX);
                points.Add(new Point(midX, limitingY));
                points.Add(new Point(curX, curBase));
                points.Add(new Point(curX, curY));
                points.Add(new Point(nextX, nextY));
                points.Add(new Point(nextX, limitingY));
                pointsSet.Add(points);

                points = new PointCollection();
                points.Add(new Point(midX, limitingY));
                points.Add(new Point(nextX, limitingY));
                points.Add(new Point(nextX, nextBase));

                pointsSet.Add(points);

            }
            else if (curY > limitingY && nextY < limitingY && curBase > limitingY && nextBase > limitingY)
            {
                points = new PointCollection();

                Double midX = Graphics.ConvertScale(curY, nextY, limitingY, curX, nextX);
                points.Add(new Point(midX, limitingY));
                points.Add(new Point(nextX, nextY));
                points.Add(new Point(nextX, limitingY));

                pointsSet.Add(points);

                points = new PointCollection();
                points.Add(new Point(curX, curBase));
                points.Add(new Point(curX, curY));
                points.Add(new Point(midX, limitingY));
                points.Add(new Point(nextX, limitingY));
                points.Add(new Point(nextX, nextY));

                pointsSet.Add(points);

            }
            else if ((curY < limitingY && nextY < limitingY && curBase > limitingY && nextBase > limitingY) ||
                    (curY > limitingY && nextY > limitingY && curBase < limitingY && nextBase < limitingY))
            {
                points = new PointCollection();
                points.Add(new Point(curX, limitingY));
                points.Add(new Point(curX, curY));
                points.Add(new Point(nextX, nextY));
                points.Add(new Point(nextX, limitingY));
                pointsSet.Add(points);

                points = new PointCollection();
                points.Add(new Point(curX, curBase));
                points.Add(new Point(curX, limitingY));
                points.Add(new Point(nextX, limitingY));
                points.Add(new Point(nextX, nextBase));
                pointsSet.Add(points);

            }
            else if (curY < limitingY && nextY > limitingY && curBase < limitingY && nextBase > limitingY)
            {
                points = new PointCollection();
                points.Add(new Point(curX, curBase));
                points.Add(new Point(curX, curY));
                Double midX1 = Graphics.ConvertScale(curY, nextY, limitingY, curX, nextX);
                points.Add(new Point(midX1, limitingY));
                Double midX2 = Graphics.ConvertScale(curBase, nextBase, limitingY, curX, nextX);
                points.Add(new Point(midX2, limitingY));
                pointsSet.Add(points);

                points = new PointCollection();
                points.Add(new Point(midX2, limitingY));
                points.Add(new Point(midX1, limitingY));
                points.Add(new Point(nextX, nextY));
                points.Add(new Point(nextX, nextBase));
                pointsSet.Add(points);

            }
            else if (curY > limitingY && nextY < limitingY && curBase > limitingY && nextBase < limitingY)
            {
                points = new PointCollection();
                points.Add(new Point(curX, curBase));
                points.Add(new Point(curX, curY));
                Double midX1 = Graphics.ConvertScale(curY, nextY, limitingY, curX, nextX);
                points.Add(new Point(midX1, limitingY));
                Double midX2 = Graphics.ConvertScale(curBase, nextBase, limitingY, curX, nextX);
                points.Add(new Point(midX2, limitingY));
                pointsSet.Add(points);

                points = new PointCollection();
                points.Add(new Point(midX2, limitingY));
                points.Add(new Point(midX1, limitingY));
                points.Add(new Point(nextX, nextY));
                points.Add(new Point(nextX, nextBase));
                pointsSet.Add(points);
            }
            else if (curY > limitingY && nextY < limitingY && curBase < limitingY && nextBase < limitingY)
            {
                points = new PointCollection();
                Double midX = Graphics.ConvertScale(curY, nextY, limitingY, curX, nextX);
                points.Add(new Point(midX, limitingY));
                points.Add(new Point(curX, limitingY));
                points.Add(new Point(curX, curBase));
                points.Add(new Point(nextX, nextBase));
                points.Add(new Point(nextX, nextY));
                pointsSet.Add(points);

                points = new PointCollection();
                points.Add(new Point(curX, curY));
                points.Add(new Point(curX, limitingY));
                points.Add(new Point(midX, limitingY));
                pointsSet.Add(points);
            }
            else if (curY > limitingY && nextY > limitingY && curBase < limitingY && nextBase > limitingY)
            {
                points = new PointCollection();
                points.Add(new Point(curX, limitingY));
                points.Add(new Point(curX, curBase));
                Double midX = Graphics.ConvertScale(curBase, nextBase, limitingY, curX, nextX);
                points.Add(new Point(midX, limitingY));
                pointsSet.Add(points);

                points = new PointCollection();
                points.Add(new Point(curX, curY));
                points.Add(new Point(curX, limitingY));
                points.Add(new Point(midX, limitingY));
                points.Add(new Point(nextX, nextBase));
                points.Add(new Point(nextX, nextY));
                pointsSet.Add(points);

            }
            else if (curY < limitingY && nextY > limitingY && curBase < limitingY && nextBase < limitingY)
            {
                points = new PointCollection();

                Double midX = Graphics.ConvertScale(curY, nextY, limitingY, curX, nextX);
                points.Add(new Point(midX, limitingY));
                points.Add(new Point(curX, curY));
                points.Add(new Point(curX, curBase));
                points.Add(new Point(nextX, nextBase));
                points.Add(new Point(nextX, limitingY));
                pointsSet.Add(points);

                points = new PointCollection();
                points.Add(new Point(midX, limitingY));
                points.Add(new Point(nextX, limitingY));
                points.Add(new Point(nextX, nextY));
                pointsSet.Add(points);

            }
            else if (curY > limitingY && nextY > limitingY && curBase > limitingY && nextBase < limitingY)
            {
                points = new PointCollection();

                Double midX = Graphics.ConvertScale(curBase, nextBase, limitingY, curX, nextX);
                points.Add(new Point(curX, curY));
                points.Add(new Point(curX, curBase));
                points.Add(new Point(midX, limitingY));
                points.Add(new Point(nextX, limitingY));
                points.Add(new Point(nextX, nextBase));
                pointsSet.Add(points);

                points = new PointCollection();
                points.Add(new Point(midX, limitingY));
                points.Add(new Point(nextX, nextY));
                points.Add(new Point(nextX, limitingY));
                pointsSet.Add(points);

            }
            else if (curY > curBase && nextY > nextBase)
            {
                points = new PointCollection();
                points.Add(new Point(curX, curY));
                points.Add(new Point(curX, curBase));
                points.Add(new Point(nextX, nextBase));
                points.Add(new Point(nextX, nextY));
                pointsSet.Add(points);
            }
            else
            {
                points = new PointCollection();
                points.Add(new Point(curX, curBase));
                points.Add(new Point(curX, curY));
                points.Add(new Point(nextX, nextY));
                points.Add(new Point(nextX, nextBase));
                pointsSet.Add(points);
            }

            return pointsSet;

        }
 
        /// <summary>
        /// Get crossing point with smallestX value
        /// </summary>
        /// <param name="curX">Current X position</param>
        /// <param name="curYValues">List of current YValues</param>
        /// <param name="nextX">Next X position</param>
        /// <param name="nextYValues">List of next YValues</param>
        /// <param name="curBase">Current base position</param>
        /// <param name="nextBase">Next base position</param>
        /// <param name="startIndex">Start index</param>
        /// <returns>Point</returns>
        internal static Point GetCrossingPointWithSmallestX(Double curX, List<Double> curYValues, Double nextX, List<Double> nextYValues, Double curBase, Double nextBase, Int32 startIndex)
        {
            Point crossingPoint = new Point(Double.MaxValue, Double.MaxValue);

            for (Int32 index = startIndex; index < curYValues.Count; index++)
            {
                Point newPoint = GetIntersection(new Point(curX, curBase), new Point(nextX, nextBase),
                                                new Point(curX, curYValues[index]), new Point(nextX, nextYValues[index]));
                if (newPoint.X < crossingPoint.X)
                    crossingPoint = newPoint;
            }

            if (crossingPoint.X == Double.MaxValue)
                return new Point(Double.NaN, Double.NaN);

            return crossingPoint;
        }

        /// <summary>
        /// Returns slope value
        /// </summary>
        /// <param name="x1">X1</param>
        /// <param name="y1">Y1</param>
        /// <param name="x2">X2</param>
        /// <param name="y2">Y2</param>
        /// <returns>Double</returns>
        internal static Double GetSlope(Double x1, Double y1, Double x2, Double y2)
        {
            return (y2 - y1) / (x2 - x1);
        }

       /// <summary>
        /// Returns intercept value
       /// </summary>
       /// <param name="x1">X1</param>
       /// <param name="y1">Y1</param>
       /// <param name="x2">X2</param>
       /// <param name="y2">Y2</param>
       /// <returns>Double</returns>
        internal static Double GetIntercept(Double x1, Double y1, Double x2, Double y2)
        {
            return y1 - x1 * GetSlope(x1, y1, x2, y2);
        }

        /// <summary>
        /// Get intersection point between two lines
        /// </summary>
        /// <param name="Line1Start">Line 1 dateTime position</param>
        /// <param name="Line1End">Line 1 end position</param>
        /// <param name="Line2Start">Line 2 stsrt position</param>
        /// <param name="Line2End">Line 2 end position</param>
        /// <returns>Point</returns>
        internal static Point GetIntersection(Point Line1Start, Point Line1End, Point Line2Start, Point Line2End)
        {
            Double line1Slope = GetSlope(Line1Start.X, Line1Start.Y, Line1End.X, Line1End.Y);
            Double line2Slope = GetSlope(Line2Start.X, Line2Start.Y, Line2End.X, Line2End.Y);
            Double line1Intercept = GetIntercept(Line1Start.X, Line1Start.Y, Line1End.X, Line1End.Y);
            Double line2Intercept = GetIntercept(Line2Start.X, Line2Start.Y, Line2End.X, Line2End.Y);

            Double Y = (line1Slope * line2Intercept - line2Slope * line1Intercept) / (line1Slope - line2Slope);
            Double X = (line2Intercept - line1Intercept) / (line1Slope - line2Slope);

            return new Point(X, Y);
        }

        /// <summary>
        /// Get bounds from point collection
        /// </summary>
        /// <param name="points">Collection of points</param>
        /// <returns>Rect</returns>
        internal static Rect GetBounds(params Point[] points)
        {
            Double minX = Double.MaxValue, minY = Double.MaxValue, maxX = Double.MinValue, maxY = Double.MinValue;
            foreach (Point point in points)
            {
                minX = Math.Min(minX, point.X);
                minY = Math.Min(minY, point.Y);

                maxX = Math.Max(maxX, point.X);
                maxY = Math.Max(maxY, point.Y);
            }
            return new Rect(minX, minY, Math.Abs(maxX - minX), Math.Abs(maxY - minY));
        }

        /// <summary>
        /// Get bounds from point collection
        /// </summary>
        /// <param name="points">Collection of points</param>
        /// <returns>Rect</returns>
        internal static Rect GetBounds(PointCollection points)
        {
            Double minX = Double.MaxValue, minY = Double.MaxValue, maxX = Double.MinValue, maxY = Double.MinValue;
            foreach (Point point in points)
            {
                minX = Math.Min(minX, point.X);
                minY = Math.Min(minY, point.Y);

                maxX = Math.Max(maxX, point.X);
                maxY = Math.Max(maxY, point.Y);
            }
            return new Rect(minX, minY, Math.Abs(maxX - minX), Math.Abs(maxY - minY));
        }
        /// <summary>
        /// Returns visual for 2DArea
        /// </summary>
        /// <param name="faces">Faces</param>
        /// <param name="areaParams">Area parameters</param>
        /// <returns>Canvas</returns>
        internal static Canvas Get2DArea(DataSeries currentDataSeries, ref Faces faces, PolygonalChartShapeParams areaParams)
        {
            if (faces.Parts == null)
                faces.Parts = new List<DependencyObject>();

            Canvas visual = new Canvas();

            visual.Width = areaParams.Size.Width;
            visual.Height = areaParams.Size.Height;

            Polygon polygon = new Polygon() { Tag = new ElementData() { Element = areaParams.TagReference, VisualElementName = "AreaBase" } };

            faces.Parts.Add(polygon);

            polygon.Fill = areaParams.Lighting ? Graphics.GetLightingEnabledBrush(areaParams.Background, "Linear", null) : areaParams.Background;

            polygon.Stroke = areaParams.BorderColor;
            polygon.StrokeDashArray = areaParams.BorderStyle != null ? ExtendedGraphics.CloneCollection(areaParams.BorderStyle) : areaParams.BorderStyle;
            polygon.StrokeThickness = areaParams.BorderThickness;
            polygon.StrokeMiterLimit = 1;
            polygon.Points = areaParams.Points;

            Rect polygonBounds = GetBounds(areaParams.Points);
            polygon.Stretch = Stretch.Fill;
            polygon.Width = polygonBounds.Width;
            polygon.Height = polygonBounds.Height;
            polygon.SetValue(Canvas.TopProperty, polygonBounds.Y);
            polygon.SetValue(Canvas.LeftProperty, polygonBounds.X);

            // apply area animation
            if (areaParams.AnimationEnabled)
            {
                // apply animation to the polygon that was used to create the area
                areaParams.Storyboard = ApplyAreaAnimation(currentDataSeries, polygon, areaParams.Storyboard, areaParams.IsPositive, 0);
            }

            visual.Children.Add(polygon);

            if (areaParams.Bevel)
            {   
                for (int i = 0; i < areaParams.Points.Count - 1; i++)
                {
                    if (areaParams.Points[i].X == areaParams.Points[i + 1].X)
                        continue;

                    Double m = GetSlope(areaParams.Points[i].X, areaParams.Points[i].Y, areaParams.Points[i + 1].X, areaParams.Points[i + 1].Y);
                    Double c = GetIntercept(areaParams.Points[i].X, areaParams.Points[i].Y, areaParams.Points[i + 1].X, areaParams.Points[i + 1].Y);
                    c = c + (areaParams.IsPositive ? 1 : -1) * 4;
                    Point newPt1 = new Point(areaParams.Points[i].X, m * areaParams.Points[i].X + c);
                    Point newPt2 = new Point(areaParams.Points[i + 1].X, m * areaParams.Points[i + 1].X + c);

                    PointCollection points = new PointCollection();
                    points.Add(areaParams.Points[i]);
                    points.Add(areaParams.Points[i + 1]);
                    points.Add(newPt2);
                    points.Add(newPt1);

                    Polygon bevel = new Polygon() { Tag = new ElementData() { Element = areaParams.TagReference, VisualElementName = "Bevel" } };
                    bevel.Points = points;
                    bevel.Fill = Graphics.GetBevelTopBrush(areaParams.Background);

                    if (areaParams.AnimationEnabled)
                    {
                        areaParams.Storyboard = CreateOpacityAnimation(currentDataSeries, areaParams.Storyboard, bevel, 1, 1, 1);
                        bevel.Opacity = 0;
                    }

                    faces.Parts.Add(bevel);

                    visual.Children.Add(bevel);
                }
            }

            return visual;
        }

        /// <summary>
        /// Get visual of Area 3D
        /// </summary>
        /// <param name="faces">Faces</param>
        /// <param name="areaParams">AreaParams</param>
        /// <returns>ZIndex</returns>
        internal static Int32 Draw3DArea(Canvas parentVisual, DataPoint previusDataPoint, DataPoint dataPoint, DataPoint nextDataPoint, ref Faces dataSeriesFaces, ref Faces dataPointFaces, DataSeries dataSeries, Double plankYPos)
        {   
            Brush sideBrush = (Boolean) dataSeries.LightingEnabled ? Graphics.GetRightFaceBrush(dataSeries.Color) : dataSeries.Color;
            Brush topBrush = (Boolean)dataSeries.LightingEnabled ? Graphics.GetTopFaceBrush(dataSeries.Color) : dataSeries.Color;
            
            // Int32 pointIndexLimit = dataSeries.IsPositive ? areaParams.Points.Count - 1 : areaParams.Points.Count;

            Random rand = new Random(DateTime.Now.Millisecond);

            // parentVisual.Background = new SolidColorBrush(Colors.Green);

            Boolean isPositive2Negative = false;    // DataPoint at -ve, previous DataPoint is positive
            //Boolean isNegative2Positive = false;    // DataPoint at -ve, next DataPoint is positive

            //if (dataPoint.InternalYValue < 0 && previusDataPoint.InternalYValue > 0)
            //    isPositive2Negative = true;

            //if (dataPoint.InternalYValue > 0 && nextDataPoint.InternalYValue < 0)
            //    isNegative2Positive = true;

            Int32 zIndex = GetAreaZIndex(dataPoint._visualPosition.X, dataPoint._visualPosition.Y, dataPoint.InternalYValue > 0 || isPositive2Negative);            
            //dataPointFaces.da
            if (dataPointFaces.Area3DLeftFace != null)
            {   
                Area3DDataPointFace leftFace = dataPointFaces.Area3DLeftFace;
                leftFace.CalculateBackFacePoints();

                Path sides = new Path() { Tag = new ElementData() { Element = dataSeries } };
                sides.SetValue(Canvas.ZIndexProperty, zIndex);

                PathGeometry pg = new PathGeometry();
                PathFigure pf = new PathFigure() { IsClosed = true };
                pg.Figures.Add(pf);

                PointCollection facePoints = leftFace.GetFacePoints();
                pf.StartPoint = leftFace.FrontFacePoints[0];

                // Graphics.DrawPointAt(dataPointFaces.Area3DLeftFace.FrontPointLeft, parentVisual, Colors.Yellow);

                foreach (Point point in facePoints)
                {
                    LineSegment ls = new LineSegment() { Point = point };
                    pf.Segments.Add(ls);
                }

                sides.Data = pg;

                sides.Fill = sideBrush;
                sides.Opacity = dataPoint.Parent.Opacity;
                ApplyBorderProperties(sides, dataPoint.Parent);
                // sides.Fill = new SolidColorBrush(Color.FromArgb(255, (byte)rand.Next(155), (byte)rand.Next(200), (byte)rand.Next(126)));
                
                parentVisual.Children.Add(sides);

                leftFace.LeftFace = sides;

                //dataPointFaces.VisualComponents.Add(sides);
                //dataPointFaces.Parts.Add(sides);

                dataSeriesFaces.VisualComponents.Add(sides);

                //dataPointFaces.BorderElements.Add(sides);
            }

            if (dataPointFaces.Area3DLeftTopFace != null)
            {         
                Area3DDataPointFace topFace = dataPointFaces.Area3DLeftTopFace;

                if (isPositive2Negative)
                {
                    //Graphics.DrawPointAt(new Point(previusDataPoint._visualPosition.X, plankYPos), parentVisual, Colors.Red);
                    //Graphics.DrawPointAt(new Point(dataPoint._visualPosition.X, plankYPos), parentVisual, Colors.Red);

                    //Graphics.DrawPointAt(previusDataPoint._visualPosition, parentVisual, Colors.Red);
                    //Graphics.DrawPointAt(dataPoint._visualPosition, parentVisual, Colors.Red);

                    Point midPoint = new Point();

                    if (dataPointFaces.Area3DRightFace == null)
                    {
                        if (Graphics.IntersectionOfTwoStraightLines(new Point(previusDataPoint._visualPosition.X, plankYPos),
                            new Point(dataPoint._visualPosition.X, plankYPos),
                            previusDataPoint._visualPosition, dataPoint._visualPosition, ref midPoint))
                        {

                            topFace.FrontFacePoints[1] = midPoint;

                            //Graphics.DrawPointAt(midPoint, parentVisual, Colors.Green);
                        }
                    }


                    //Graphics.DrawPointAt(midPoint, parentVisual, Colors.Green);
                }

                //if (isNegative2Positive)
                //{   
                //    Graphics.DrawPointAt(new Point(previusDataPoint._visualPosition.X, plankYPos), parentVisual, Colors.Red);
                //    Graphics.DrawPointAt(new Point(dataPoint._visualPosition.X, plankYPos), parentVisual, Colors.Red);

                //    Graphics.DrawPointAt(previusDataPoint._visualPosition, parentVisual, Colors.Red);
                //    Graphics.DrawPointAt(dataPoint._visualPosition, parentVisual, Colors.Red);

                //    Point midPoint = new Point();

                //    if (Graphics.IntersectionOfTwoStraightLines(new Point(nextDataPoint._visualPosition.X, plankYPos),
                //        new Point(dataPoint._visualPosition.X, plankYPos),
                //        nextDataPoint._visualPosition, dataPoint._visualPosition, ref midPoint))
                //    {   
                //        topFace.FrontFacePoints[1] = midPoint;
                //    }

                //    Graphics.DrawPointAt(midPoint, parentVisual, Colors.Green);
                //}

                topFace.CalculateBackFacePoints();

                Path sides = new Path() { Tag = new ElementData() { Element = dataSeries } };
                sides.SetValue(Canvas.ZIndexProperty, zIndex);

                PathGeometry pg = new PathGeometry();
                PathFigure pf = new PathFigure() { IsClosed = true };
                pg.Figures.Add(pf);

                // Graphics.DrawPointAt(dataPointFaces.Area3DTopFace.FrontPointLeft, parentVisual, Colors.Yellow);

                pf.StartPoint = topFace.FrontFacePoints[0];

                PointCollection facePoints = topFace.GetFacePoints();

                foreach (Point point in facePoints)
                {   
                    LineSegment ls = new LineSegment() { Point = point };
                    pf.Segments.Add(ls);
                }
                
                sides.Data = pg;

                sides.Fill = topBrush;
                sides.Opacity = dataPoint.Parent.Opacity;
                ApplyBorderProperties(sides, dataPoint.Parent);
                // sides.Fill = new SolidColorBrush(Color.FromArgb(255, (byte)rand.Next(155), (byte)rand.Next(200), (byte)rand.Next(126))); //sideBrush;

                parentVisual.Children.Add(sides);

                topFace.TopFace = sides;
                // sides.Fill = new SolidColorBrush(Colors.Red);
                //dataPointFaces.VisualComponents.Add(sides);
                //dataPointFaces.Parts.Add(sides);
                dataSeriesFaces.VisualComponents.Add(sides);
                //dataPointFaces.BorderElements.Add(sides);

            }

            if (dataPointFaces.Area3DRightFace != null)
            {
                Area3DDataPointFace rightFace = dataPointFaces.Area3DRightFace;
                rightFace.CalculateBackFacePoints();

                Path sides = new Path() { Tag = new ElementData() { Element = dataSeries } };
                sides.SetValue(Canvas.ZIndexProperty, zIndex);

                PathGeometry pg = new PathGeometry();
                PathFigure pf = new PathFigure() { IsClosed = true };
                pg.Figures.Add(pf);

                PointCollection facePoints = rightFace.GetFacePoints();
                pf.StartPoint = rightFace.FrontFacePoints[0];

                // Graphics.DrawPointAt(dataPointFaces.Area3DLeftFace.FrontPointLeft, parentVisual, Colors.Yellow);
                // Graphics.DrawPointAt(dataPointFaces.Area3DLeftFace.FrontPointRight, parentVisual, Colors.Yellow);
                // Graphics.DrawPointAt(dataPointFaces.Area3DLeftFace.BackPointRight, parentVisual, Colors.Yellow);
                // Graphics.DrawPointAt(dataPointFaces.Area3DLeftFace.BackPointLeft, parentVisual, Colors.Yellow);

                foreach (Point point in facePoints)
                {
                    LineSegment ls = new LineSegment() { Point = point };
                    pf.Segments.Add(ls);
                }

                sides.Data = pg;

                sides.Fill = sideBrush;
                sides.Opacity = dataPoint.Parent.Opacity;
                ApplyBorderProperties(sides, dataPoint.Parent);
                // sides.Fill = new SolidColorBrush(Color.FromArgb(255, (byte)rand.Next(155), (byte)rand.Next(200), (byte)rand.Next(126)));

                parentVisual.Children.Add(sides);

                rightFace.RightFace = sides;
                
                ApplyBorderProperties(sides, dataSeries);

                //dataPointFaces.VisualComponents.Add(sides);
                //dataPointFaces.Parts.Add(sides);

                dataSeriesFaces.VisualComponents.Add(sides);

                //dataPointFaces.BorderElements.Add(sides);
            }

            return zIndex;
        }

        internal static void ApplyBorderProperties(Path path, DataSeries dataSeries)
        {
            path.Stroke = dataSeries.BorderColor;
            path.StrokeDashArray = ExtendedGraphics.GetDashArray(dataSeries.BorderStyle);
            path.StrokeThickness = dataSeries.BorderThickness.Left;
            path.StrokeMiterLimit = 1;
        }

        internal static void Update(ObservableObject sender, VcProperties property, object newValue, Boolean isAxisChanged)
        {
            Boolean isDataPoint;

            if (property == VcProperties.Bevel)
            {
                sender = (sender as DataPoint).Parent;
                isDataPoint = false;
            }
            else
                isDataPoint = sender.GetType().Equals(typeof(DataPoint));

            if (isDataPoint)
                UpdateDataPoint(sender as DataPoint, property, newValue, isAxisChanged);
            else
                UpdateDataSeries(sender as DataSeries, property, newValue);
        }

        internal static void Update(Chart chart, RenderAs currentRenderAs, List<DataSeries> selectedDataSeries4Rendering, VcProperties property, object newValue)
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


        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataPoint"></param>
        /// <param name="property"></param>
        /// <param name="newValue"></param>
        /// <param name="isAxisChanged"></param>
        private static void UpdateDataPoint(DataPoint dataPoint, VcProperties property, object newValue, Boolean isAxisChanged)
        {
            Chart chart = dataPoint.Chart as Chart;
            PlotDetails plotDetails = chart.PlotDetails;

            Marker marker = dataPoint.Marker;
            DataSeries dataSeries = dataPoint.Parent;
            PlotGroup plotGroup = dataSeries.PlotGroup;
            Canvas areaVisual = dataSeries.Faces.Visual as Canvas;
            Canvas labelCanvas = ((areaVisual as FrameworkElement).Parent as Panel).Children[0] as Canvas;
            Double height = chart.ChartArea.ChartVisualCanvas.Height;
            Double width = chart.ChartArea.ChartVisualCanvas.Width;
            Double xPosition, yPosition;
            Faces faces = dataPoint.Faces;

            if (faces == null && property != VcProperties.Enabled && property != VcProperties.YValue)
                return;

            switch (property)
            {    
                 case VcProperties.Bevel:
                     UpdateDataSeries(dataSeries, property, newValue);
                     break;

                case VcProperties.Color:
                case VcProperties.LightingEnabled:
                     if (property != VcProperties.LightingEnabled && marker != null && (Boolean)dataPoint.MarkerEnabled)
                         marker.BorderColor = (dataPoint.GetValue(DataPoint.MarkerBorderColorProperty) as Brush == null) ? ((newValue != null) ? newValue as Brush : dataPoint.MarkerBorderColor) : dataPoint.MarkerBorderColor;

                    if (faces != null)
                    {
                        if(dataPoint.Faces.BevelLine != null)
                            dataPoint.Faces.BevelLine.Stroke = Graphics.GetBevelTopBrush(dataPoint.Parent.Color);

                        Brush sideBrush = (Boolean)dataSeries.LightingEnabled ? Graphics.GetRightFaceBrush(dataSeries.Color) : dataSeries.Color;
                        Brush topBrush = (Boolean)dataSeries.LightingEnabled ? Graphics.GetTopFaceBrush(dataSeries.Color) : dataSeries.Color;

                        if (dataPoint.Faces.Area3DLeftFace != null)
                            (dataPoint.Faces.Area3DLeftFace.LeftFace as Path).Fill = sideBrush;

                        if (dataPoint.Faces.Area3DRightTopFace != null)
                            (dataPoint.Faces.Area3DRightTopFace.TopFace as Path).Fill = topBrush;

                        if (dataPoint.Faces.Area3DLeftTopFace != null)
                            (dataPoint.Faces.Area3DLeftTopFace.TopFace as Path).Fill = topBrush;

                        if (dataPoint.Faces.Area3DRightFace != null)
                            (dataPoint.Faces.Area3DRightFace.RightFace as Path).Fill = sideBrush;
                    }

                    break;

                case VcProperties.Opacity:

                    Double opacity = dataPoint.Opacity * dataSeries.Opacity;
                    if (marker != null)
                        marker.Visual.Opacity = opacity;

                    if (faces != null)
                    {   
                        if (dataPoint.Faces.BevelLine != null)
                            dataPoint.Faces.BevelLine.Opacity = opacity;

                        if (dataPoint.Faces.Area3DLeftFace != null)
                            (dataPoint.Faces.Area3DLeftFace.LeftFace as Path).Opacity = opacity;

                        if (dataPoint.Faces.Area3DRightTopFace != null)
                            (dataPoint.Faces.Area3DRightTopFace.TopFace as Path).Opacity = opacity;

                        if (dataPoint.Faces.Area3DLeftTopFace != null)
                            (dataPoint.Faces.Area3DLeftTopFace.TopFace as Path).Opacity = opacity;

                        if (dataPoint.Faces.Area3DRightFace != null)
                            (dataPoint.Faces.Area3DRightFace.RightFace as Path).Opacity = opacity;
                    }

                    break;
                case VcProperties.BorderColor:
                case VcProperties.BorderStyle:
                case VcProperties.BorderThickness:
                    if (faces != null)
                    {
                        if (dataPoint.Faces.Area3DLeftFace != null)
                            ApplyBorderProperties(dataPoint.Faces.Area3DLeftFace.LeftFace as Path, dataSeries);

                        if (dataPoint.Faces.Area3DRightTopFace != null)
                            ApplyBorderProperties(dataPoint.Faces.Area3DRightTopFace.TopFace as Path, dataSeries);

                        if (dataPoint.Faces.Area3DLeftTopFace != null)
                            ApplyBorderProperties(dataPoint.Faces.Area3DLeftTopFace.TopFace as Path, dataSeries);

                        if (dataPoint.Faces.Area3DRightFace != null)
                            ApplyBorderProperties(dataPoint.Faces.Area3DRightFace.RightFace as Path, dataSeries);
                    }

                    break;
                case VcProperties.Cursor:
                    dataPoint.SetCursor2DataPointVisualFaces();
                    break;

                case VcProperties.Href:
                case VcProperties.HrefTarget:
                    dataPoint.SetHref2DataPointVisualFaces();
                    break;

                case VcProperties.LabelBackground:
                    LineChart.CreateMarkerAForLineDataPoint(dataPoint, width, height, ref labelCanvas, out xPosition, out yPosition);
                    marker.TextBackground = dataPoint.LabelBackground;
                    break;

                case VcProperties.LabelEnabled:
                    LineChart.CreateMarkerAForLineDataPoint(dataPoint, width, height, ref labelCanvas, out xPosition, out yPosition);
                    break;

                case VcProperties.LabelFontColor:
                    marker.FontColor = dataPoint.LabelFontColor;
                    break;

                case VcProperties.LabelFontFamily:
                    LineChart.CreateMarkerAForLineDataPoint(dataPoint, width, height, ref labelCanvas, out xPosition, out yPosition);
                    // marker.FontFamily = dataPoint.LabelFontFamily;
                    break;

                case VcProperties.LabelFontStyle:
                    LineChart.CreateMarkerAForLineDataPoint(dataPoint, width, height, ref labelCanvas, out xPosition, out yPosition);

                    //marker.FontStyle = (FontStyle) dataPoint.LabelFontStyle;
                    break;

                case VcProperties.LabelFontSize:
                    LineChart.CreateMarkerAForLineDataPoint(dataPoint, width, height, ref labelCanvas, out xPosition, out yPosition);

                    // marker.FontSize = (Double) dataPoint.LabelFontSize;
                    break;

                case VcProperties.LabelFontWeight:
                    marker.FontWeight = (FontWeight)dataPoint.LabelFontWeight;
                    break;

                case VcProperties.LabelStyle:
                    LineChart.CreateMarkerAForLineDataPoint(dataPoint, width, height, ref labelCanvas, out xPosition, out yPosition);
                    break;

                case VcProperties.LabelText:
                    LineChart.CreateMarkerAForLineDataPoint(dataPoint, width, height, ref labelCanvas, out xPosition, out yPosition);
                    //marker.Text = dataPoint.TextParser(dataPoint.LabelText);
                    break;

                case VcProperties.LabelAngle:
                    LineChart.CreateMarkerAForLineDataPoint(dataPoint, width, height, ref labelCanvas, out xPosition, out yPosition);
                    //marker.Text = dataPoint.TextParser(dataPoint.LabelText);
                    break;

                case VcProperties.LegendText:
                    chart.InvokeRender();
                    break;

                case VcProperties.MarkerBorderColor:
                    LineChart.CreateMarkerAForLineDataPoint(dataPoint, width, height, ref labelCanvas, out xPosition, out yPosition);

                    //marker.BorderColor = dataPoint.MarkerBorderColor;
                    break;
                case VcProperties.MarkerBorderThickness:
                    LineChart.CreateMarkerAForLineDataPoint(dataPoint, width, height, ref labelCanvas, out xPosition, out yPosition);
                    //marker.BorderThickness = dataPoint.MarkerBorderThickness.Value.Left;
                    break;

                case VcProperties.MarkerColor:
                    if(marker != null && (Boolean) dataPoint.MarkerEnabled)
                        marker.MarkerFillColor = dataPoint.MarkerColor;
                    break;

                case VcProperties.MarkerEnabled:
                    LineChart.CreateMarkerAForLineDataPoint(dataPoint, width, height, ref labelCanvas, out xPosition, out yPosition);

                    // if((Boolean)dataPoint.MarkerEnabled)
                    //    ShowDataPointMarker(dataPoint);
                    // else
                    //    HideDataPointMarker(dataPoint);
                    break;

                case VcProperties.MarkerScale:
                case VcProperties.MarkerSize:
                case VcProperties.MarkerType:
                case VcProperties.ShadowEnabled:
                    // Double y = Graphics.ValueToPixelPosition(plotGroup.AxisY.Height, 0, plotGroup.AxisY.InternalAxisMinimum, plotGroup.AxisY.InternalAxisMaximum, dataPoint.InternalYValue);
                    // LineChart.GetMarkerForDataPoint(true, chart, y, dataPoint, dataPoint.InternalYValue > 0);
                    LineChart.CreateMarkerAForLineDataPoint(dataPoint, width, height, ref labelCanvas, out xPosition, out yPosition);
                    break;

                case VcProperties.ShowInLegend:
                    chart.InvokeRender();
                    break;

                case VcProperties.ToolTipText:
                case VcProperties.XValueFormatString:
                case VcProperties.YValueFormatString:
                    dataPoint._parsedToolTipText = dataPoint.TextParser(dataPoint.ToolTipText);
                    LineChart.CreateMarkerAForLineDataPoint(dataPoint, width, height, ref labelCanvas, out xPosition, out yPosition);
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
                case VcProperties.YValues:

                    if (isAxisChanged || dataPoint._oldYValue >= 0 && dataPoint.InternalYValue < 0 || dataPoint._oldYValue <= 0 && dataPoint.InternalYValue > 0)
                        UpdateDataSeries(dataSeries, property, newValue);
                    else
                    {
                        dataPoint._parsedToolTipText = dataPoint.TextParser(dataPoint.ToolTipText);
                        UpdateVisualForYValue4AreaChart(chart, dataPoint, isAxisChanged);
                        //chart.Dispatcher.BeginInvoke(new Action<Chart, DataPoint, Boolean>(UpdateVisualForYValue4AreaChart), new object[] { chart, dataPoint, isAxisChanged});

                    }

                    chart._toolTip.Hide();
                    // chart.Dispatcher.BeginInvoke(new Action<DataPoint>(UpdateXAndYValue), new object[]{dataPoint});

                    break;
            }
        }

        public static void UpdateVisualForYValue4AreaChart(Chart chart, DataPoint dataPoint, Boolean isAxisChanged)
        {
            if (dataPoint.Faces == null)
                return;

            DataSeries dataSeries = dataPoint.Parent;               // parent of the current DataPoint
            Canvas areaCanvas = dataSeries.Faces.Visual as Canvas;  // Existing parent area of column
            Boolean isPositive = (dataPoint.InternalYValue >= 0);   // Whether YValue is positive
            Double depth3d = chart.ChartArea.PLANK_DEPTH / chart.PlotDetails.Layer3DCount * (chart.View3D ? 1 : 0);
            PlotGroup  plotGroup = dataPoint.Parent.PlotGroup;
            Double oldMarkerTop = Double.NaN;
            Double oldMarkerLeft = Double.NaN;
            Point newPosition, oldPosition;
            Storyboard storyBoardDp = null;
            Boolean animationEnabled = (Boolean)chart.AnimatedUpdate;
            Point oldVisualPositionOfDataPoint;
            Canvas labelCanvas = dataSeries.Faces.LabelCanvas;//(areaCanvas.Parent as Canvas).Children[0] as Canvas;
            Double height = chart.ChartArea.ChartVisualCanvas.Height;
            Double width = chart.ChartArea.ChartVisualCanvas.Width;

            ColumnChart.UpdateParentVisualCanvasSize(chart, labelCanvas);
            ColumnChart.UpdateParentVisualCanvasSize(chart, areaCanvas);
            ColumnChart.UpdateParentVisualCanvasSize(chart, areaCanvas.Children[0] as Canvas);

            // Create new Column with new YValue
            if (dataPoint.Storyboard != null)
            {
                dataPoint.Storyboard.Stop();

                dataPoint.Storyboard.Children.Clear();
            }

            if (animationEnabled)
            {
                if (dataPoint.Storyboard != null)
                    storyBoardDp = dataPoint.Storyboard;
                else
                    storyBoardDp = new Storyboard();
            }
            
            // Calculate pixel position for DataPoint
            Double xPosition = Graphics.ValueToPixelPosition(0, width, (Double)plotGroup.AxisX.InternalAxisMinimum, (Double)plotGroup.AxisX.InternalAxisMaximum, dataPoint.InternalXValue);
            Double yPosition = Graphics.ValueToPixelPosition(height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, dataPoint.InternalYValue);
            oldVisualPositionOfDataPoint = dataPoint._visualPosition;
            
            
            if (dataPoint.Marker != null && dataPoint.Marker.Visual != null)
            {
                Double xMpos, yMpos;

                if (storyBoardDp != null)
                    storyBoardDp.Pause();

                oldMarkerTop = (Double)dataPoint.Marker.Visual.GetValue(Canvas.TopProperty);
                oldMarkerLeft = (Double)dataPoint.Marker.Visual.GetValue(Canvas.LeftProperty);
                LineChart.CreateMarkerAForLineDataPoint(dataPoint, width, height, ref labelCanvas, out xMpos, out yMpos);

                Point actualPosOfVisual = dataPoint.Marker.CalculateActualPosition(xPosition, yPosition, new Point(0.5, 0.5));

                //Double markerNEwTop = dataPoint.Marker.MarkerActualPosition.Y;//yPosition - Math.Abs(oldVisualPositionOfDataPoint.Y - oldMarkerTop);
                //Double markerNewLeft = dataPoint.Marker.MarkerActualPosition.X;
                // dataPoint.Marker.Visual.SetValue(Canvas.TopProperty, markerNEwTop);

                if ((Boolean)dataPoint.MarkerEnabled || (Boolean)dataPoint.LabelEnabled)
                {
                    if (animationEnabled)
                    {
                        AnimationHelper.ApplyPropertyAnimation(dataPoint.Marker.Visual, "(Canvas.Top)", dataPoint, storyBoardDp, 0,
                             new Double[] { 0, 1 }, new Double[] { oldMarkerTop, actualPosOfVisual.Y }, null);

                        AnimationHelper.ApplyPropertyAnimation(dataPoint.Marker.Visual, "(Canvas.Left)", dataPoint, storyBoardDp, 0,
                            new Double[] { 0, 1 }, new Double[] { oldMarkerLeft, actualPosOfVisual.X }, null);

                    }
                    else
                    {
                        dataPoint.Marker.Visual.SetValue(Canvas.TopProperty, actualPosOfVisual.Y);
                        dataPoint.Marker.Visual.SetValue(Canvas.LeftProperty, actualPosOfVisual.X);
                    }
                }
            }

            

            dataPoint._visualPosition.X = xPosition;
            dataPoint._visualPosition.Y = yPosition;

            DataPoint nextDataPoint = dataPoint.Faces.NextDataPoint;

            Double plankYPos = Graphics.ValueToPixelPosition(areaCanvas.Height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, plotGroup.GetLimitingYValue());

            // Update left face of the area
            if (dataPoint.Faces.Area3DLeftFace != null)
            {   
                Area3DDataPointFace leftFace = dataPoint.Faces.Area3DLeftFace;
                Path path = leftFace.LeftFace as Path;

                ((path.Data as PathGeometry).Figures[0] as PathFigure).StartPoint = new Point(xPosition, plankYPos);
                //path.Fill = new SolidColorBrush(Colors.Orange);
                
                // Update front top line
                LineSegment ls = Area3DDataPointFace.GetLineSegment(path, 0);
                //--newPosition = new Point(ls.Point.X, yPosition);
                newPosition = new Point(xPosition, yPosition);
                oldPosition = ls.Point;
                ls.Point = newPosition;

                if (animationEnabled)
                     AnimationHelper.ApplyPointAnimation(ls, "Point", dataPoint, storyBoardDp, 0,
                        new Double[]{ 0, 1}, new Point[]{oldPosition, newPosition}, null, Double.NaN);

                // Update back top line
                ls = Area3DDataPointFace.GetLineSegment(path, 1);
                //--newPosition = new Point(ls.Point.X, yPosition - leftFace.Depth3d);
                newPosition = new Point(xPosition + leftFace.Depth3d, yPosition - leftFace.Depth3d);
                oldPosition = ls.Point;
                ls.Point = newPosition;

                if (animationEnabled)
                    AnimationHelper.ApplyPointAnimation(ls, "Point", dataPoint, storyBoardDp, 0,
                       new Double[] { 0, 1 }, new Point[] { oldPosition, newPosition }, null, Double.NaN);

                // Update back top line
                ls = Area3DDataPointFace.GetLineSegment(path, 2);
                //--newPosition = new Point(ls.Point.X, yPosition - leftFace.Depth3d);
                newPosition = new Point(xPosition + leftFace.Depth3d, plankYPos - leftFace.Depth3d);
                oldPosition = ls.Point;
                ls.Point = newPosition;

                //if (animationEnabled)
                //    AnimationHelper.ApplyPointAnimation(ls, "Point", dataPoint, storyBoardDp, 0,
                //       new Double[] { 0, 1 }, new Point[] { oldPosition, newPosition }, null, Double.NaN);
            }

            // Update right face of the area
            if (dataPoint.Faces.Area3DRightTopFace != null)
            {
                Area3DDataPointFace rightTopFace = dataPoint.Faces.Area3DRightTopFace;
                Path rightTopFacePath = rightTopFace.TopFace as Path;
                // rightTopFacePath.Fill = new SolidColorBrush(Colors.Red);

                // Update front left point
                PathFigure pf = Area3DDataPointFace.GetPathFigure(rightTopFacePath);
                //--newPosition = new Point(pf.StartPoint.X, yPosition);
                newPosition = new Point(xPosition, yPosition);
                oldPosition = pf.StartPoint;
                pf.StartPoint = newPosition;
                //rightTopFacePath.Fill = new SolidColorBrush(Colors.Red);

                if (animationEnabled)
                    //AnimationHelper.ApplyPointAnimation(storyBoardDp, pf, "rightTopFacePath_" + dataPoint.Name, "StartPoint",
                    //    oldPosition, newPosition, 1, 0);

                    AnimationHelper.ApplyPointAnimation(pf, "StartPoint", dataPoint, storyBoardDp, 0,
                        new Double[]{ 0, 1}, new Point[]{oldPosition, newPosition}, null, Double.NaN);

                else
                    pf.StartPoint = newPosition;

                // Update back left point
                LineSegment ls = Area3DDataPointFace.GetLineSegment(rightTopFacePath, 2);
                //--newPosition = new Point(ls.Point.X, yPosition - rightTopFace.Depth3d);
                newPosition = new Point(xPosition + rightTopFace.Depth3d, yPosition - rightTopFace.Depth3d);
                oldPosition = ls.Point;
                ls.Point = newPosition;

                if (animationEnabled)
                    AnimationHelper.ApplyPointAnimation(ls, "Point", dataPoint, storyBoardDp, 0,
                        new Double[]{ 0, 1}, new Point[]{oldPosition, newPosition}, null, Double.NaN);
                else
                    ls.Point = newPosition;
            }

            // Update left-top face of the area
            if (dataPoint.Faces.Area3DLeftTopFace != null)
            {   
                DataPoint previusDataPoint = dataPoint.Faces.PreviousDataPoint;
                
                Boolean isPositive2Negative = false;    // DataPoint at -ve, previous DataPoint!is +ve
                Boolean isNegative2Positive = false;    // DataPoint at -ve, next DataPoint is +ve
                Boolean isNegative2Negative = false;    // DataPoint at -ve, next DataPoint is -ve

                if (dataPoint.InternalYValue < 0 && previusDataPoint != null && previusDataPoint.InternalYValue > 0)
                    //|| (nextDataPoint != null && nextDataPoint.InternalYValue > 0)))
                    isPositive2Negative = true;

                if (dataPoint.InternalYValue > 0 && nextDataPoint != null && nextDataPoint.InternalYValue < 0)
                    isNegative2Positive = true;

                if (dataPoint.InternalYValue < 0 && previusDataPoint != null && nextDataPoint.InternalYValue < 0)
                    isNegative2Negative = true;
                
                Area3DDataPointFace leftTopFace = dataPoint.Faces.Area3DLeftTopFace;
                Path leftTopFacePath = leftTopFace.TopFace as Path;
                // leftTopFacePath.Fill = new SolidColorBrush(Colors.Green);

                // DataPoint was positive but after YValue update it’s became negative
                if (isPositive2Negative)
                {   
                    Point midPoint = new Point();

                    Graphics.IntersectionOfTwoStraightLines(new Point(previusDataPoint._visualPosition.X, plankYPos),
                        new Point(dataPoint._visualPosition.X, plankYPos),
                        previusDataPoint._visualPosition, dataPoint._visualPosition, ref midPoint);

                    // leftTopFacePath.Fill = new SolidColorBrush(Colors.Green);

                    //Graphics.DrawPointAt(new Point(previusDataPoint._visualPosition.X, plankYPos), areaCanvas, Colors.Red);
                    //Graphics.DrawPointAt(new Point(dataPoint._visualPosition.X, plankYPos), areaCanvas, Colors.Red);
                    //Graphics.DrawPointAt(previusDataPoint._visualPosition, areaCanvas, Colors.Red);
                    
                    // Update right front point
                    LineSegment ls = Area3DDataPointFace.GetLineSegment(leftTopFacePath, 0);
                    newPosition = new Point(midPoint.X, midPoint.Y);
                    oldPosition = ls.Point;
                    ls.Point = newPosition;

                    // Update right back point
                    LineSegment ls1 = Area3DDataPointFace.GetLineSegment(leftTopFacePath, 1);
                    Point newPosition1 = new Point(midPoint.X + leftTopFace.Depth3d, midPoint.Y - leftTopFace.Depth3d);
                    Point oldPosition1 = ls1.Point;
                    ls1.Point = newPosition1;

                    //Double D1 = Math.Abs(dataPoint.Faces.AreaFrontFaceLineSegment.Point.Y - plankYPos); // Y distance
                    //Double D2 = Math.Abs(oldPosition.X - dataPoint._visualPosition.X);  // X distance
                    //Double deltaDistance = D1 - D2;
                    //Double t = deltaDistance * T1 / D1;
                    //Double timetoTravleTillPlank = (Double)(T1 + t);

                    
                    Double D1 = Math.Abs(dataPoint.Faces.AreaFrontFaceLineSegment.Point.Y - plankYPos); // Y distance from top to plank
                    Double T1 = D1 / Math.Abs(dataPoint.Faces.AreaFrontFaceLineSegment.Point.Y - dataPoint._visualPosition.Y);// Time take to traval distance D1

                    Double D2 = Math.Abs(newPosition.X - dataPoint._visualPosition.X);  // Y distance from plank to bottom
                    Double T2 = D2 / Math.Abs(dataPoint.Faces.AreaFrontFaceLineSegment.Point.Y - dataPoint._visualPosition.Y); // Time take to traval distance D1
                    
                    //Double deltaDistance = D1 - D2;
                    //Double t = deltaDistance * 0.5 / D1;
                    // Double timetoTravleTillPlank = (Double)(T1);

                    if (animationEnabled)
                    {
                        if (dataPoint._oldYValue > 0 && dataPoint.InternalYValue < 0)
                        {
                            AnimationHelper.ApplyPointAnimation(ls, "Point", dataPoint, storyBoardDp, 0,
                                   new Double[] { 0, T1, T1 + T2 }, new Point[] { oldPosition, 
                                new Point(oldPosition.X, plankYPos), newPosition }, null, Double.NaN);

                            AnimationHelper.ApplyPointAnimation(ls1, "Point", dataPoint, storyBoardDp, 0,
                                new Double[] { 0, T1, T1 + T2 },
                                new Point[] { oldPosition1,new Point(oldPosition1.X, plankYPos - leftTopFace.Depth3d),                             
                            newPosition1 }, null, Double.NaN);
                        }
                        else
                        {
                            AnimationHelper.ApplyPointAnimation(ls, "Point", dataPoint, storyBoardDp, 0,
                                new Double[] { 0, 1 }, 
                                new Point[] { oldPosition, newPosition }, null, Double.NaN);

                            AnimationHelper.ApplyPointAnimation(ls1, "Point", dataPoint, storyBoardDp, 0,
                                new Double[] { 0, 1 },
                                new Point[] { oldPosition1, newPosition1 }, null, Double.NaN);
                        }
                    }
                }
                else if (isNegative2Negative)
                {   
                    // Right top face of this DataPoint = left top face of next DataPoint
                    Area3DDataPointFace rightTopFace = nextDataPoint.Faces.Area3DLeftTopFace;
                    Path rightTopFacePath = rightTopFace.TopFace as Path;
                   // rightTopFacePath.Fill = new SolidColorBrush(Colors.Blue);

                    Int32 zIndex = GetAreaZIndex(dataPoint._visualPosition.X, dataPoint._visualPosition.Y, dataPoint.InternalYValue > 0 || isNegative2Negative);
                    //rightTopFacePath.SetValue(Canvas.ZIndexProperty, zIndex);

                    LineSegment ls = Area3DDataPointFace.GetLineSegment(rightTopFacePath, 0);
                    newPosition = new Point(nextDataPoint._visualPosition.X, nextDataPoint._visualPosition.Y);
                    oldPosition = ls.Point;
                    ls.Point = newPosition;

                    if (animationEnabled)
                        AnimationHelper.ApplyPointAnimation(ls, "Point", dataPoint, storyBoardDp, 0,
                            new Double[] { 0, 1 }, new Point[] { oldPosition, newPosition }, null, Double.NaN);
                    else
                        ls.Point = newPosition;

                    // Update right back point of top right
                    ls = Area3DDataPointFace.GetLineSegment(rightTopFacePath, 1);
                    newPosition = new Point(nextDataPoint._visualPosition.X + rightTopFace.Depth3d, nextDataPoint._visualPosition.Y - rightTopFace.Depth3d);
                    oldPosition = ls.Point;
                    ls.Point = newPosition;

                    if (animationEnabled)
                        AnimationHelper.ApplyPointAnimation(ls, "Point", dataPoint, storyBoardDp, 0,
                            new Double[] { 0, 1 }, new Point[] { oldPosition, newPosition }, null, Double.NaN);
                    else
                        ls.Point = newPosition;
                    
                    // Update right front point for 1st DataPoint
                    // Or Update left front point
                    ls = Area3DDataPointFace.GetLineSegment(leftTopFacePath, 0);
                    newPosition = new Point(dataPoint._visualPosition.X, yPosition);
                    oldPosition = ls.Point;
                    ls.Point = newPosition;

                    if (animationEnabled)
                        AnimationHelper.ApplyPointAnimation(ls, "Point", dataPoint, storyBoardDp, 0,
                            new Double[] { 0, 1 }, new Point[] { oldPosition, newPosition }, null, Double.NaN);
                    else
                        ls.Point = newPosition;

                    // Update right back point for 1st DataPoint
                    // Or Update left back point
                    ls = Area3DDataPointFace.GetLineSegment(leftTopFacePath, 1);
                    newPosition = new Point(dataPoint._visualPosition.X + leftTopFace.Depth3d, yPosition - leftTopFace.Depth3d);
                    oldPosition = ls.Point;
                    ls.Point = newPosition;

                    if (animationEnabled)
                        AnimationHelper.ApplyPointAnimation(ls, "Point", dataPoint, storyBoardDp, 0,
                            new Double[] { 0, 1 }, new Point[] { oldPosition, newPosition }, null, Double.NaN);
                    else
                        ls.Point = newPosition;
                }
                else if (isNegative2Positive)
                {   
                    
                    Point midPoint = new Point();

                    Graphics.IntersectionOfTwoStraightLines(new Point(nextDataPoint._visualPosition.X, plankYPos),
                        new Point(dataPoint._visualPosition.X, plankYPos),
                        nextDataPoint._visualPosition, dataPoint._visualPosition, ref midPoint);

                    // Right top face of this DataPoint = left top face of next DataPoint
                    Area3DDataPointFace rightTopFace = nextDataPoint.Faces.Area3DLeftTopFace;
                    Path rightTopFacePath = rightTopFace.TopFace as Path;

                    Int32 zIndex = GetAreaZIndex(dataPoint._visualPosition.X, dataPoint._visualPosition.Y, dataPoint.InternalYValue > 0 || isNegative2Positive);
                    rightTopFacePath.SetValue(Canvas.ZIndexProperty, zIndex);

                    // rightTopFacePath.Fill = new SolidColorBrush(Colors.Red);

                    // Update right front point of top right
                    LineSegment ls = Area3DDataPointFace.GetLineSegment(rightTopFacePath, 0);
                    newPosition = new Point(midPoint.X, midPoint.Y);
                    oldPosition = ls.Point;
                    ls.Point = newPosition;

                    if (animationEnabled)
                        AnimationHelper.ApplyPointAnimation(ls, "Point", dataPoint, storyBoardDp, 0,
                            new Double[] { 0, 1 }, new Point[] { oldPosition, newPosition }, null, Double.NaN);
                    else
                        ls.Point = newPosition;

                    // Update right back point of top right
                    ls = Area3DDataPointFace.GetLineSegment(rightTopFacePath, 1);
                    newPosition = new Point(midPoint.X + leftTopFace.Depth3d, midPoint.Y - leftTopFace.Depth3d);
                    oldPosition = ls.Point;
                    ls.Point = newPosition;

                    if (animationEnabled)
                        AnimationHelper.ApplyPointAnimation(ls, "Point", dataPoint, storyBoardDp, 0,
                            new Double[] { 0, 1 }, new Point[] { oldPosition, newPosition }, null, Double.NaN);
                    else
                        ls.Point = newPosition;

                    // Update right front point of top left face
                    ls = Area3DDataPointFace.GetLineSegment(leftTopFacePath, 0);
                    newPosition = new Point(dataPoint._visualPosition.X, yPosition);
                    oldPosition = ls.Point;
                    ls.Point = newPosition;

                    // Update right back point of top left face
                    LineSegment ls1 = Area3DDataPointFace.GetLineSegment(leftTopFacePath, 1);
                    Point newPosition1 = new Point(dataPoint._visualPosition.X + leftTopFace.Depth3d, yPosition - leftTopFace.Depth3d);
                    Point oldPosition1 = ls1.Point;
                    ls1.Point = newPosition1;
                                        
                    if (animationEnabled)
                    {
                        if (dataPoint._oldYValue < 0 && dataPoint.InternalYValue >= 0)
                        {
                            Double D1 = Math.Abs(dataPoint.Faces.AreaFrontFaceLineSegment.Point.Y - plankYPos); // Y distance from top to plank
                            Double T1 = D1 / Math.Abs(dataPoint.Faces.AreaFrontFaceLineSegment.Point.Y - dataPoint._visualPosition.Y);// Time take to traval distance D1

                            Double D2 = Math.Abs(oldPosition.X - dataPoint._visualPosition.X);  // Y distance from plank to bottom
                            Double T2 = D2 / Math.Abs(dataPoint.Faces.AreaFrontFaceLineSegment.Point.Y - dataPoint._visualPosition.Y); // Time take to traval distance D1

                            AnimationHelper.ApplyPointAnimation(ls, "Point", dataPoint, storyBoardDp, 0,
                               new Double[] { 0, T1, 1 },
                               new Point[] { oldPosition, new Point(dataPoint._visualPosition.X, plankYPos)
                                   , newPosition }
                               , null, Double.NaN);

                            AnimationHelper.ApplyPointAnimation(ls1, "Point", dataPoint, storyBoardDp, 0,
                              new Double[] { 0, T1, 1 },
                              new Point[] { oldPosition1, 
                                   new Point(dataPoint._visualPosition.X + leftTopFace.Depth3d, plankYPos - leftTopFace.Depth3d),
                                   newPosition1 }
                              , null, Double.NaN);
                        }
                        else
                        {
                            AnimationHelper.ApplyPointAnimation(ls, "Point", dataPoint, storyBoardDp, 0,
                                new Double[] { 0, 1 }, new Point[] { oldPosition, newPosition }, null, Double.NaN);
                            AnimationHelper.ApplyPointAnimation(ls1, "Point", dataPoint, storyBoardDp, 0,
                                new Double[] { 0, 1 }, new Point[] { oldPosition, newPosition }, null, Double.NaN);
                        }
                    }

                }
                else
                {   
                    // Update right front point for 1st DataPoint
                    // Or Update left front point
                    LineSegment ls = Area3DDataPointFace.GetLineSegment(leftTopFacePath, 0);
                    newPosition = new Point(dataPoint._visualPosition.X, yPosition);
                    oldPosition = ls.Point;
                    ls.Point = newPosition;

                    //leftTopFacePath.Fill = new SolidColorBrush(Colors.Purple);

                    // Update right back point for 1st DataPoint
                    // Or Update left back point
                    LineSegment ls1 = Area3DDataPointFace.GetLineSegment(leftTopFacePath, 1);
                    Point newPosition1 = new Point(dataPoint._visualPosition.X + leftTopFace.Depth3d, yPosition - leftTopFace.Depth3d);
                    Point oldPosition1 = ls1.Point;
                    ls1.Point = newPosition1;

                    if (dataPoint._oldYValue < 0 && dataPoint.InternalYValue >= 0)
                    {
                        if (animationEnabled)
                        {
                           
                            Double D1 = Math.Abs(dataPoint.Faces.AreaFrontFaceLineSegment.Point.Y - plankYPos); // Y distance from top to plank
                            Double T1 = D1 / Math.Abs(dataPoint.Faces.AreaFrontFaceLineSegment.Point.Y - dataPoint._visualPosition.Y);// Time take to traval distance D1

                            Double D2 = Math.Abs(oldPosition.X - dataPoint._visualPosition.X);  // Y distance from plank to bottom
                            Double T2 = D2 / Math.Abs(dataPoint.Faces.AreaFrontFaceLineSegment.Point.Y - dataPoint._visualPosition.Y); // Time take to traval distance D1

                            AnimationHelper.ApplyPointAnimation(ls, "Point", dataPoint, storyBoardDp, 0,
                               new Double[] { 0, T1, 1 },
                               new Point[] { oldPosition, new Point(dataPoint._visualPosition.X, plankYPos)
                                   , newPosition }
                               , null, Double.NaN);

                            AnimationHelper.ApplyPointAnimation(ls1, "Point", dataPoint, storyBoardDp, 0,
                              new Double[] { 0, T1, 1 },
                              new Point[] { oldPosition1, 
                                   new Point(dataPoint._visualPosition.X + leftTopFace.Depth3d, plankYPos - leftTopFace.Depth3d),
                                   newPosition1 }
                              , null, Double.NaN);
                        }
                    }
                    else
                    {
                        if (animationEnabled)
                        {
                            AnimationHelper.ApplyPointAnimation(ls, "Point", dataPoint, storyBoardDp, 0,
                              new Double[] { 0, 1 },
                              new Point[] { oldPosition, newPosition }
                              , null, Double.NaN);
                            
                            AnimationHelper.ApplyPointAnimation(ls1, "Point", dataPoint, storyBoardDp, 0,
                             new Double[] { 0, 1 },
                             new Point[] { oldPosition1, newPosition1 }
                             , null, Double.NaN);
                        }

                    }


                }
            }

            if (dataPoint.Faces.Area3DRightFace != null)
            {   
                Area3DDataPointFace rightFace = dataPoint.Faces.Area3DRightFace;
                Path rightFacePath = rightFace.RightFace as Path;
                // rightFacePath.Fill = new SolidColorBrush(Colors.Green);

                // Update front point
                PathFigure pf = Area3DDataPointFace.GetPathFigure(rightFacePath);
                //--newPosition = new Point(pf.StartPoint.X, yPosition);
                newPosition = new Point(xPosition, yPosition);
                oldPosition = pf.StartPoint;
                pf.StartPoint = newPosition;

                if (animationEnabled)
                    AnimationHelper.ApplyPointAnimation(pf, "StartPoint", dataPoint, storyBoardDp, 0,
                        new Double[] { 0, 1 }, new Point[] { oldPosition, newPosition }, null, Double.NaN);
                else
                    pf.StartPoint = newPosition;

                // Update top back point
                LineSegment ls = Area3DDataPointFace.GetLineSegment(rightFacePath, 2);
                //--newPosition = new Point(ls.Point.X, yPosition - rightFace.Depth3d);
                newPosition = new Point(xPosition + rightFace.Depth3d, yPosition - rightFace.Depth3d);
                oldPosition = ls.Point;
                ls.Point = newPosition;

                if (animationEnabled)
                    AnimationHelper.ApplyPointAnimation(ls, "Point", dataPoint, storyBoardDp, 0,
                        new Double[] { 0, 1 }, new Point[] { oldPosition, newPosition }, null, Double.NaN);
                else
                    ls.Point = newPosition;


                // Update zero base line point front point
                ls = Area3DDataPointFace.GetLineSegment(rightFacePath, 0);
                //--newPosition = new Point(ls.Point.X, yPosition - rightFace.Depth3d);
                newPosition = new Point(xPosition, plankYPos);
                oldPosition = ls.Point;
                ls.Point = newPosition;

                //if (animationEnabled)
                //    AnimationHelper.ApplyPointAnimation(ls, "Point", dataPoint, storyBoardDp, 0,
                //        new Double[] { 0, 1 }, new Point[] { oldPosition, newPosition }, null, Double.NaN);
                //else
                //    ls.Point = newPosition;

                // Update zero base line point back point
                ls = Area3DDataPointFace.GetLineSegment(rightFacePath, 1);
                newPosition = new Point(xPosition + rightFace.Depth3d, plankYPos - rightFace.Depth3d);
                oldPosition = ls.Point;
                ls.Point = newPosition;

                //if (animationEnabled)
                //    AnimationHelper.ApplyPointAnimation(ls, "Point", dataPoint, storyBoardDp, 0,
                //        new Double[] { 0, 1 }, new Point[] { oldPosition, newPosition }, null, Double.NaN);
                //else
                //    ls.Point = newPosition;
            }

            if (animationEnabled)
            {
                oldPosition = dataPoint.Faces.AreaFrontFaceLineSegment.Point;
                dataPoint.Faces.AreaFrontFaceLineSegment.Point = dataPoint._visualPosition;

                // If is the last DataPoint to update
                if (dataPoint == nextDataPoint)
                {
                    if (chart.View3D)
                    {
                        if (dataSeries.Faces != null && dataSeries.Faces.FrontFacePaths.Count > 0)
                        {
                            //LineSegment ls = Area3DDataPointFace.GetLineSegment(dataSeries.Faces.FrontFacePaths[dataSeries.Faces.FrontFacePaths.Count - 1], 0);
                            //ls.Point = new Point(dataSeries.DataPoints[0]._visualPosition.X + depth3d, plankYPos - depth3d);

                            ((dataSeries.Faces.FrontFacePaths[dataSeries.Faces.FrontFacePaths.Count - 1].Data as PathGeometry).Figures[0] as PathFigure).StartPoint = new Point(dataSeries.DataPoints[0]._visualPosition.X + depth3d, plankYPos);

                            LineSegment ls = Area3DDataPointFace.GetLineSegment(dataSeries.Faces.FrontFacePaths[dataSeries.Faces.FrontFacePaths.Count - 1], 0);
                            ls.Point = new Point(dataSeries.DataPoints[0]._visualPosition.X + depth3d, plankYPos - depth3d);

                            ls = Area3DDataPointFace.GetLineSegment(dataSeries.Faces.FrontFacePaths[dataSeries.Faces.FrontFacePaths.Count - 1], 1);
                            ls.Point = new Point(dataPoint._visualPosition.X + depth3d, plankYPos - depth3d);

                            ls = Area3DDataPointFace.GetLineSegment(dataSeries.Faces.FrontFacePaths[dataSeries.Faces.FrontFacePaths.Count - 1], 2);
                            ls.Point = new Point(dataPoint._visualPosition.X, plankYPos);
                        }
                    }

                    if (dataSeries.Faces != null && dataSeries.Faces.FrontFacePaths.Count > 0)
                    {
                        ((dataSeries.Faces.FrontFacePaths[0].Data as PathGeometry).Figures[0] as PathFigure).StartPoint = new Point(((dataSeries.Faces.FrontFacePaths[0].Data as PathGeometry).Figures[0] as PathFigure).StartPoint.X, plankYPos);
                    }

                    dataPoint.Faces.AreaFrontFaceBaseLineSegment.Point = new Point(dataPoint._visualPosition.X, plankYPos);

                }

                //if (dataPoint._oldYValue < 0 && dataPoint.InternalYValue > 0)
                //{

                //    Double D1 = Math.Abs(dataPoint.Faces.AreaFrontFaceLineSegment.Point.Y - plankYPos); // Y distance
                //    Double timeTaken2TouchPlank = D1 / Math.Abs(dataPoint.Faces.AreaFrontFaceLineSegment.Point.Y - dataPoint._visualPosition.Y);//Time take to traval distance D1

                //    AnimationHelper.ApplyPointAnimation(dataPoint.Faces.AreaFrontFaceLineSegment, "Point", dataPoint, storyBoardDp, 0,
                //        new Double[] { 0, timeTaken2TouchPlank, 1 },
                //        new Point[] { oldPosition, new Point(oldPosition.X, plankYPos),
                //        dataPoint._visualPosition
                //        }
                //        , null, Double.NaN);

                //    // AnimationHelper.ApplyPointAnimation(storyBoardDp, dataPoint.Faces.AreaFrontFaceLineSegment, "frontface_" + dataPoint.Name, "Point",
                //    //    oldPosition, new Point(dataPoint._visualPosition.X, plankYPos), 0.5, 0);

                //    // AnimationHelper.ApplyPointAnimation(storyBoardDp, dataPoint.Faces.AreaFrontFaceLineSegment, "frontface_" + dataPoint.Name, "Point",
                //    //    oldPosition, new Point(dataPoint._visualPosition.X, plankYPos), 0.5, 0.5);
                //}
                //else
                //{
                AnimationHelper.ApplyPointAnimation(dataPoint.Faces.AreaFrontFaceLineSegment, "Point", dataPoint, storyBoardDp, 0,
                    new Double[] { 0, 1 }, new Point[] { oldPosition, dataPoint._visualPosition }, null, Double.NaN);
                //}

                dataPoint.Storyboard = storyBoardDp;
                oldVisualPositionOfDataPoint = oldPosition;
                if (dataSeries.Bevel && !chart.View3D)
                    AnimateBevelLayer(dataPoint, oldVisualPositionOfDataPoint, animationEnabled);

                
#if WPF
                storyBoardDp.Begin(chart._rootElement, true);
#else
                storyBoardDp.Begin();
#endif
            }
            else
            {
                dataPoint.Faces.AreaFrontFaceLineSegment.Point = dataPoint._visualPosition;

                // If is the last DataPoint to update
                if (dataPoint == nextDataPoint)
                {
                    if (chart.View3D)
                    {
                        if (dataSeries.Faces != null && dataSeries.Faces.FrontFacePaths.Count > 0)
                        {
                            ((dataSeries.Faces.FrontFacePaths[dataSeries.Faces.FrontFacePaths.Count - 1].Data as PathGeometry).Figures[0] as PathFigure).StartPoint = new Point(dataSeries.DataPoints[0]._visualPosition.X, plankYPos);

                            LineSegment ls = Area3DDataPointFace.GetLineSegment(dataSeries.Faces.FrontFacePaths[dataSeries.Faces.FrontFacePaths.Count - 1], 0);
                            ls.Point = new Point(dataSeries.DataPoints[0]._visualPosition.X, plankYPos - depth3d);

                            ls = Area3DDataPointFace.GetLineSegment(dataSeries.Faces.FrontFacePaths[dataSeries.Faces.FrontFacePaths.Count - 1], 1);
                            ls.Point = new Point(dataPoint._visualPosition.X + depth3d, plankYPos - depth3d);

                            ls = Area3DDataPointFace.GetLineSegment(dataSeries.Faces.FrontFacePaths[dataSeries.Faces.FrontFacePaths.Count - 1], 2);
                            ls.Point = new Point(dataPoint._visualPosition.X, plankYPos);
                        }
                    }

                    if(dataSeries.Faces != null && dataSeries.Faces.FrontFacePaths.Count > 0)
                        ((dataSeries.Faces.FrontFacePaths[0].Data as PathGeometry).Figures[0] as PathFigure).StartPoint = new Point(((dataSeries.Faces.FrontFacePaths[0].Data as PathGeometry).Figures[0] as PathFigure).StartPoint.X, plankYPos);

                    dataPoint.Faces.AreaFrontFaceBaseLineSegment.Point = new Point(dataPoint._visualPosition.X, plankYPos);
                }

                if (dataSeries.Bevel && !chart.View3D)
                    AnimateBevelLayer(dataPoint, oldVisualPositionOfDataPoint, animationEnabled);
            }

            if (dataSeries.ToolTipElement != null)
                dataSeries.ToolTipElement.Hide();

            chart.ChartArea.DisableIndicators();

            // Update existing Plank
            ColumnChart.CreateOrUpdatePlank(chart, dataSeries.PlotGroup.AxisY, areaCanvas.Parent as Canvas, depth3d, Orientation.Horizontal);

            PlotArea plotArea = chart.PlotArea;

            RectangleGeometry clipRectangle = new RectangleGeometry();
            clipRectangle.Rect = new Rect(0, plotArea.BorderThickness.Top - depth3d - 4, width + depth3d, height + depth3d + chart.ChartArea.PLANK_THICKNESS + 10 - plotArea.BorderThickness.Bottom - plotArea.BorderThickness.Top);

            if (areaCanvas.Parent != null)
                (areaCanvas.Parent as Canvas).Clip = clipRectangle;

            clipRectangle = new RectangleGeometry();

            Double clipLeft = 0;
            Double clipTop = -depth3d - 4;
            Double clipWidth = width + depth3d;
            Double clipHeight = height + depth3d + chart.ChartArea.PLANK_THICKNESS + 10;

            GetClipCoordinates(chart, ref clipLeft, ref clipTop, ref clipWidth, ref clipHeight, dataSeries.PlotGroup.MinimumX, dataSeries.PlotGroup.MaximumX);

            clipRectangle.Rect = new Rect(clipLeft, clipTop, clipWidth, clipHeight);

            if (labelCanvas != null)
                (labelCanvas as Canvas).Clip = clipRectangle;
        }

        /// <summary>
        /// Animate the Bevel layer
        /// </summary>
        /// <param name="dataPoint"></param>
        private static void AnimateBevelLayer(DataPoint dataPoint, Point oldVisualPositionOfDataPoint, Boolean animationEnabled)
        {
            Line bevelLine = dataPoint.Faces.BevelLine;
            DataPoint previousDataPoint = dataPoint.Faces.PreviousDataPoint;
            DataPoint nextDataPoint = dataPoint.Faces.NextDataPoint;
            Storyboard storyBoardDp = dataPoint.Storyboard;

            // If dataPoint is the first DataPoint of the area
            if (dataPoint.Faces.PreviousDataPoint == dataPoint)
            {
                if (animationEnabled)
                {
                    AnimationHelper.ApplyPropertyAnimation(bevelLine, "Y1", dataPoint, storyBoardDp, 0,
                            new Double[] { 0, 1 }, new Double[] { oldVisualPositionOfDataPoint.Y, dataPoint._visualPosition.Y }, null);

                    AnimationHelper.ApplyPropertyAnimation(bevelLine, "X1", dataPoint, storyBoardDp, 0,
                           new Double[] { 0, 1 }, new Double[] { oldVisualPositionOfDataPoint.X, dataPoint._visualPosition.X }, null);
                }
                else
                {
                    bevelLine.Y1 = dataPoint._visualPosition.Y;
                    bevelLine.X1 = dataPoint._visualPosition.X;
                }
            }
            // dataPoint is the last DataPoint of the area
            else if (dataPoint == nextDataPoint)
            {
                if (animationEnabled)
                {
                    AnimationHelper.ApplyPropertyAnimation(bevelLine, "Y2", dataPoint, storyBoardDp, 0,
                            new Double[] { 0, 1 }, new Double[] { oldVisualPositionOfDataPoint.Y, dataPoint._visualPosition.Y }, null);
                    
                    AnimationHelper.ApplyPropertyAnimation(bevelLine, "X2", dataPoint, storyBoardDp, 0,
                            new Double[] { 0, 1 }, new Double[] { oldVisualPositionOfDataPoint.X, dataPoint._visualPosition.X }, null);
                }
                else
                {
                    bevelLine.Y2 = dataPoint._visualPosition.Y;
                    bevelLine.X2 = dataPoint._visualPosition.X;
                }
            }
            else
            {
                if (animationEnabled)
                {
                    AnimationHelper.ApplyPropertyAnimation(bevelLine, "Y1", dataPoint, storyBoardDp, 0,
                              new Double[] { 0, 1 }, new Double[] { oldVisualPositionOfDataPoint.Y, dataPoint._visualPosition.Y }, null);

                    AnimationHelper.ApplyPropertyAnimation(bevelLine, "X1", dataPoint, storyBoardDp, 0,
                             new Double[] { 0, 1 }, new Double[] { oldVisualPositionOfDataPoint.X, dataPoint._visualPosition.X }, null);
                }
                else
                {
                    bevelLine.Y1 = dataPoint._visualPosition.Y;
                    bevelLine.X1 = dataPoint._visualPosition.X;
                }

                bevelLine = dataPoint.Faces.PreviousDataPoint.Faces.BevelLine;

                if (animationEnabled)
                {
                    AnimationHelper.ApplyPropertyAnimation(bevelLine, "Y2", dataPoint, storyBoardDp, 0,
                         new Double[] { 0, 1 }, new Double[] { oldVisualPositionOfDataPoint.Y, dataPoint._visualPosition.Y }, null);

                    AnimationHelper.ApplyPropertyAnimation(bevelLine, "X2", dataPoint, storyBoardDp, 0,
                        new Double[] { 0, 1 }, new Double[] { oldVisualPositionOfDataPoint.X, dataPoint._visualPosition.X }, null);
                }
                else
                {
                    bevelLine.Y2 = dataPoint._visualPosition.Y;
                    bevelLine.X2 = dataPoint._visualPosition.X;
                }
            }
        }
        
        private static void UpdateDataSeries(DataSeries dataSeries, VcProperties property, object newValue)
        {
            Chart chart = dataSeries.Chart as Chart;
            Boolean is3D = chart.View3D;

            switch (property)
            {   
                case VcProperties.Color:
                case VcProperties.LightingEnabled:
                    if(dataSeries.Faces != null)
                    {
                        foreach (Path path in dataSeries.Faces.FrontFacePaths)
                        {
                            if (chart.View3D)
                                path.Fill = (Boolean)dataSeries.LightingEnabled ? Graphics.GetFrontFaceBrush(dataSeries.Color) : dataSeries.Color;
                            else
                                path.Fill = (Boolean)dataSeries.LightingEnabled ? Graphics.GetLightingEnabledBrush(dataSeries.Color, "Linear", null) : dataSeries.Color;
                        }
                    }

                    break;
                case VcProperties.Opacity:
                    if (dataSeries.Faces != null)
                    {
                        foreach (Path path in dataSeries.Faces.FrontFacePaths)
                            path.Opacity = (Double) newValue;
                    }
                    break;

                case VcProperties.BorderColor:
                case VcProperties.BorderStyle:
                case VcProperties.BorderThickness:
                    if (dataSeries.Faces != null)
                    {
                        foreach (Path path in dataSeries.Faces.FrontFacePaths)
                            ApplyBorderProperties(path, dataSeries);
                    }

                    break;

                case VcProperties.ShadowEnabled:
                    if (!VisifireControl.IsXbapApp)
                    {
                        if (dataSeries.Faces != null && dataSeries.Faces.Visual != null)
                        {
                            if ((Boolean)dataSeries.ShadowEnabled)
                            {
                                dataSeries.Faces.Visual.Effect = ExtendedGraphics.GetShadowEffect(135, 2, 1);
                            }
                            else
                                dataSeries.Faces.Visual.Effect = null;
                        }
                    }
                    break;

                case VcProperties.DataPoints:
                case VcProperties.Enabled:
                case VcProperties.Bevel:
                case VcProperties.YValue:
                case VcProperties.YValues:
                case VcProperties.XValue:

                    chart.ChartArea.RenderSeries();
                    //ChartVisualCanvas = chart.ChartArea.ChartVisualCanvas;

                    //Double width = chart.ChartArea.ChartVisualCanvas.Width;
                    //Double height = chart.ChartArea.ChartVisualCanvas.Height;

                    //PlotDetails plotDetails = chart.PlotDetails;
                    //PlotGroup plotGroup = dataSeries.PlotGroup;

                    ////Double columnWidth = CalculateWidthOfEachColumn(chart, width, dataSeries.PlotGroup.AxisX,RenderAs.Column, Orientation.Horizontal);

                    //// Dictionary<Double, SortDataPoints> sortedDataPoints = plotDetails.GetDataPointsGroupedByXValue(RenderAs.Column);
                    //// Contains a list of serties as per the drawing order generated in the plotdetails

                    //List<DataSeries> dataSeriesListInDrawingOrder = plotDetails.SeriesDrawingIndex.Keys.ToList();

                    //List<DataSeries> selectedDataSeries4Rendering = new List<DataSeries>();

                    //RenderAs currentRenderAs = dataSeries.RenderAs;

                    //Int32 currentDrawingIndex = plotDetails.SeriesDrawingIndex[dataSeries];

                    //for (Int32 i = 0; i < chart.InternalSeries.Count; i++)
                    //{
                    //    if (currentRenderAs == dataSeriesListInDrawingOrder[i].RenderAs && currentDrawingIndex == plotDetails.SeriesDrawingIndex[dataSeriesListInDrawingOrder[i]])
                    //        selectedDataSeries4Rendering.Add(dataSeriesListInDrawingOrder[i]);
                    //}

                    //if (selectedDataSeries4Rendering.Count == 0)
                    //    return;

                    //Panel oldPanel = null;
                    //Dictionary<RenderAs, Panel> RenderedCanvasList = chart.ChartArea.RenderedCanvasList;

                    //if (chart.ChartArea.RenderedCanvasList.ContainsKey(currentRenderAs))
                    //{
                    //    oldPanel = RenderedCanvasList[currentRenderAs];
                    //}

                    //Panel renderedChart = chart.ChartArea.RenderSeriesFromList(oldPanel, selectedDataSeries4Rendering);

                    //if (oldPanel == null)
                    //{
                    //    chart.ChartArea.RenderedCanvasList.Add(currentRenderAs, renderedChart);
                    //    renderedChart.SetValue(Canvas.ZIndexProperty, currentDrawingIndex);
                    //    ChartVisualCanvas.Children.Add(renderedChart);
                    //}
                    //else
                    //    chart.ChartArea.RenderedCanvasList[currentRenderAs] = renderedChart;
                    break;
            }
        }
        
        /// <summary>
        /// Get visual for StackedArea 2D
        /// </summary>
        /// <param name="faces">Faces</param>
        /// <param name="areaParams">Area parameters</param>
        /// <returns>Canvas</returns>
        internal static Canvas GetStacked2DArea(ref Faces faces, PolygonalChartShapeParams areaParams)
        {
            if (faces.Parts == null)
                faces.Parts = new List<DependencyObject>();

            Canvas visual = new Canvas();
            visual.Width = areaParams.Size.Width;
            visual.Height = areaParams.Size.Height;

            Polygon polygon = new Polygon() { Tag = new ElementData() { Element = areaParams.TagReference, VisualElementName = "AreaBase" } };

            faces.Parts.Add(polygon);

            polygon.Fill = areaParams.Lighting ? Graphics.GetLightingEnabledBrush(areaParams.Background, "Linear", null) : areaParams.Background;
            polygon.Stroke = areaParams.BorderColor;
            polygon.StrokeDashArray = areaParams.BorderStyle != null ? ExtendedGraphics.CloneCollection(areaParams.BorderStyle) : areaParams.BorderStyle;
            polygon.StrokeThickness = areaParams.BorderThickness;
            polygon.StrokeMiterLimit = 1;

            polygon.Points = areaParams.Points;

            Rect polygonBounds = GetBounds(areaParams.Points);
            polygon.Stretch = Stretch.Fill;
            polygon.Width = polygonBounds.Width;
            polygon.Height = polygonBounds.Height;
            polygon.SetValue(Canvas.TopProperty, polygonBounds.Y);
            polygon.SetValue(Canvas.LeftProperty, polygonBounds.X);

            visual.Children.Add(polygon);

            if (areaParams.Bevel)
            {
                for (int i = 0; i < areaParams.Points.Count - 1; i++)
                {
                    if (areaParams.Points[i].X == areaParams.Points[i + 1].X)
                        continue;

                    Double m = GetSlope(areaParams.Points[i].X, areaParams.Points[i].Y, areaParams.Points[i + 1].X, areaParams.Points[i + 1].Y);
                    Double c = GetIntercept(areaParams.Points[i].X, areaParams.Points[i].Y, areaParams.Points[i + 1].X, areaParams.Points[i + 1].Y);
                    c = c + (areaParams.IsPositive ? 1 : -1) * 4;
                    Point newPt1 = new Point(areaParams.Points[i].X, m * areaParams.Points[i].X + c);
                    Point newPt2 = new Point(areaParams.Points[i + 1].X, m * areaParams.Points[i + 1].X + c);

                    PointCollection points = new PointCollection();
                    points.Add(areaParams.Points[i]);
                    points.Add(areaParams.Points[i + 1]);
                    points.Add(newPt2);
                    points.Add(newPt1);

                    Polygon bevel = new Polygon() { Tag = new ElementData() { Element = areaParams.TagReference, VisualElementName = "Bevel" } };
                    bevel.Points = points;
                    bevel.Fill = Graphics.GetBevelTopBrush(areaParams.Background);

                    faces.Parts.Add(bevel);
                    visual.Children.Add(bevel);
                }
            }

            return visual;
        }

        /// <summary>
        /// Get Stacked3DArea front face
        /// </summary>
        /// <param name="faces">Faces</param>
        /// <param name="areaParams">Area parameters</param>
        /// <returns>Canvas</returns>
        internal static Canvas GetStacked3DAreaFrontFace(ref Faces faces, PolygonalChartShapeParams areaParams)
        {
            Polygon polygon = new Polygon() { Tag = new ElementData() { Element = areaParams.TagReference, VisualElementName = "AreaBase" } };

            faces.Parts.Add(polygon);
            Point centroid = GetCentroid(areaParams.Points);
            Rect size = GetBounds(areaParams.Points);

            polygon.SetValue(Canvas.ZIndexProperty, (Int32)centroid.Y + 1000);
            polygon.Fill = areaParams.Lighting ? Graphics.GetFrontFaceBrush(areaParams.Background) : areaParams.Background;

            polygon.Stroke = areaParams.BorderColor;
            polygon.StrokeDashArray = areaParams.BorderStyle != null ? ExtendedGraphics.CloneCollection(areaParams.BorderStyle) : areaParams.BorderStyle;
            polygon.StrokeThickness = areaParams.BorderThickness;
            polygon.StrokeMiterLimit = 1;

            polygon.Points = areaParams.Points;

            polygon.Stretch = Stretch.Fill;
            polygon.Width = size.Width;
            polygon.Height = size.Height;
            polygon.SetValue(Canvas.TopProperty, areaParams.Depth3D);
            polygon.SetValue(Canvas.LeftProperty, 0.0);

            Canvas polygonSet = new Canvas() { Tag = new ElementData() { Element = areaParams.TagReference } };
            polygonSet.Width = size.Width + areaParams.Depth3D;
            polygonSet.Height = size.Height + areaParams.Depth3D;
            polygonSet.SetValue(Canvas.TopProperty, size.Top - areaParams.Depth3D);
            polygonSet.SetValue(Canvas.LeftProperty, size.Left);

            polygonSet.Children.Add(polygon);

            return polygonSet;
        }

        #endregion

        #region Internal Events And Delegates

        #endregion

        #region Data

        #endregion


    }
}



