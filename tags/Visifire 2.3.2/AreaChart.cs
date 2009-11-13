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

                sides.StrokeDashArray = areaParams.BorderStyle != null ? ExtendedGraphics.CloneCollection(areaParams.BorderStyle) : areaParams.BorderStyle;
                sides.StrokeThickness = areaParams.BorderThickness;

                if(sides.StrokeThickness > 0)
                    sides.Stroke = areaParams.BorderColor;

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
            return AnimationHelper.ApplyOpacityAnimation(areaElement, currentDataSeries, storyboard, beginTime, duration, 1);
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
            areaParams.Shadow = series.ShadowEnabled;
            areaParams.Bevel = series.Bevel;
            areaParams.BorderColor = series.BorderColor;
            areaParams.BorderStyle = ExtendedGraphics.GetDashArray(series.BorderStyle);
            areaParams.BorderThickness = series.InternalBorderThickness.Left;
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
        internal static Canvas GetVisualObjectForAreaChart(Double width, Double height, PlotDetails plotDetails, List<DataSeries> seriesList, Chart chart, Double plankDepth, bool animationEnabled)
        {
            if (Double.IsNaN(width) || Double.IsNaN(height) || width <= 0 || height <= 0)
                return null;

            DataSeries currentDataSeries = null; // Current working DataSeries
            
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

            Double minimumXValue = Double.MaxValue;
            Double maximumXValue = Double.MinValue;

            foreach (DataSeries series in seriesList)
            {
                if (series.Enabled == false)
                    continue;

                if (series.Storyboard == null)
                    series.Storyboard = new Storyboard();

                currentDataSeries = series;

                PlotGroup plotGroup = series.PlotGroup;

                Brush areaBrush = series.Color;

                Double limitingYValue = 0;

                minimumXValue = Math.Min(minimumXValue, plotGroup.MinimumX);
                maximumXValue = Math.Max(maximumXValue, plotGroup.MaximumX);

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
                series.Faces.Parts = new List<FrameworkElement>();

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

                    marker = GetMarkerForDataPoint(chart, height, true, currentDataPoint, yPosition, currentDataPoint.InternalYValue > 0);
                    if (marker != null)
                    {
                        marker.AddToParent(labelCanvas, xPosition, yPosition, new Point(0.5, 0.5));

                        // Apply marker animation
                        if (animationEnabled)
                        {
                            if (currentDataPoint.Parent.Storyboard == null)
                                currentDataPoint.Parent.Storyboard = new Storyboard();

                            // Apply marker animation
                            currentDataPoint.Parent.Storyboard = AnimationHelper.ApplyOpacityAnimation(marker, currentDataSeries, currentDataPoint.Parent.Storyboard, 1, currentDataPoint.InternalOpacity * currentDataPoint.Parent.InternalOpacity);
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
                            Canvas areaVisual3D = Get3DArea(currentDataSeries, ref faces, areaParams);
                            areaVisual3D.SetValue(Canvas.ZIndexProperty, GetAreaZIndex(xPosition, yPosition, areaParams.IsPositive));
                            areaCanvas.Children.Add(areaVisual3D);
                            series.Faces.VisualComponents.Add(areaVisual3D);
                        }
                        else
                        {
                            areaCanvas.Children.Add(Get2DArea(currentDataSeries, ref faces, areaParams));
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

                marker = GetMarkerForDataPoint(chart, height, true, lastDataPoint, yPosition, lastDataPoint.InternalYValue > 0);

                if (marker != null)
                {
                    marker.AddToParent(labelCanvas, xPosition, yPosition, new Point(0.5, 0.5));
                    // Apply marker animation
                    if (animationEnabled)
                    {
                        if (lastDataPoint.Parent.Storyboard == null)
                            lastDataPoint.Parent.Storyboard = new Storyboard();

                        // Apply marker animation
                        lastDataPoint.Parent.Storyboard = AnimationHelper.ApplyOpacityAnimation(marker, currentDataSeries, lastDataPoint.Parent.Storyboard, 1, lastDataPoint.InternalOpacity * lastDataPoint.Parent.InternalOpacity);
                    }
                }

                xPosition = Graphics.ValueToPixelPosition(0, width, (Double)plotGroup.AxisX.InternalAxisMinimum, (Double)plotGroup.AxisX.InternalAxisMaximum, lastDataPoint.InternalXValue);
                yPosition = Graphics.ValueToPixelPosition(height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, limitingYValue);

                points.Add(new Point(xPosition, yPosition));

                // get the faces
                areaParams.Points = points;
                areaParams.IsPositive = (lastDataPoint.InternalYValue > 0);

                if (chart.View3D)
                {   
                    Canvas areaVisual3D = Get3DArea(currentDataSeries, ref faces, areaParams);
                    areaVisual3D.SetValue(Canvas.ZIndexProperty, GetAreaZIndex(xPosition, yPosition, areaParams.IsPositive));
                    areaCanvas.Children.Add(areaVisual3D);
                    series.Faces.VisualComponents.Add(areaVisual3D);

                }
                else
                {
                    areaCanvas.Children.Add(Get2DArea(currentDataSeries, ref faces, areaParams));
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

                    zeroPlankVisual.IsHitTestVisible = false;

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


            RectangleGeometry clipRectangle = new RectangleGeometry();
            clipRectangle.Rect = new Rect(0, -depth3d, width + depth3d, height + depth3d + chart.ChartArea.PLANK_THICKNESS);
            areaCanvas.Clip = clipRectangle;

            visual.Children.Add(areaCanvas);
            
            // Clip the label canvas
            
            clipRectangle = new RectangleGeometry();

            Double clipLeft = 0;
            Double clipTop = -depth3d;
            Double clipWidth = width + depth3d;
            Double clipHeight = height + depth3d + chart.ChartArea.PLANK_THICKNESS + 6;

            GetClipCoordinates(chart, ref clipLeft, ref clipTop, ref clipWidth, ref clipHeight, minimumXValue, maximumXValue);

            clipRectangle.Rect = new Rect(clipLeft, clipTop, clipWidth, clipHeight);
            labelCanvas.Clip = clipRectangle;

            visual.Children.Add(labelCanvas);

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
        internal static Canvas GetVisualObjectForStackedAreaChart(Double width, Double height, PlotDetails plotDetails, List<DataSeries> seriesList, Chart chart, Double plankDepth, bool animationEnabled)
        {
            if (Double.IsNaN(width) || Double.IsNaN(height) || width <= 0 || height <= 0)
                return null;

            DataSeries currentDataSeries = null; // Current working DataSeries
            
            Boolean plankDrawn = false;
            Double depth3d = plankDepth / plotDetails.Layer3DCount * (chart.View3D ? 1 : 0);
            Double visualOffset = depth3d * (plotDetails.SeriesDrawingIndex[seriesList[0]] + 1);

            if (Double.IsNaN(visualOffset) || Double.IsInfinity(visualOffset))
                return null;

            Canvas visual = new Canvas() { Width = width, Height = height };
            Canvas labelCanvas = new Canvas() { Width = width, Height = height };
            Canvas areaCanvas = new Canvas() { Width = width, Height = height };

            visual.SetValue(Canvas.TopProperty, visualOffset);
            visual.SetValue(Canvas.LeftProperty, -visualOffset);

            var plotgroups = (from series in seriesList where series.PlotGroup != null select series.PlotGroup);

            if (plotgroups.Count() == 0)
                return visual;

            PlotGroup plotGroup = plotgroups.First();

            Dictionary<Double, List<Double>> dataPointValuesInStackedOrder = plotDetails.GetDataPointValuesInStackedOrder(plotGroup);

            Dictionary<Double, List<DataPoint>> dataPointInStackedOrder = plotDetails.GetDataPointInStackOrder(plotGroup);

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
            }

            Double limitingYPosition = Graphics.ValueToPixelPosition(height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, limitingYValue);

            Marker marker;

            List<Double> curYValues;
            List<Double> nextYValues;

            List<DataPoint> curDataPoints;
            List<DataPoint> nextDataPoints;

            for (Int32 i = 0; i < xValues.Length - 1; i++)
            {
                curYValues = dataPointValuesInStackedOrder[xValues[i]];
                nextYValues = dataPointValuesInStackedOrder[xValues[i + 1]];

                Double curBase = limitingYValue;
                Double nextBase = limitingYValue;

                curDataPoints = dataPointInStackedOrder[xValues[i]];
                nextDataPoints = dataPointInStackedOrder[xValues[i + 1]];

                for (Int32 index = 0; index < curYValues.Count; index++)
                {
                    if (index >= nextYValues.Count || index >= curYValues.Count || curDataPoints[index] == null || nextDataPoints[index] == null)
                        continue;

                    Double curXPosition = Graphics.ValueToPixelPosition(0, width, (Double)plotGroup.AxisX.InternalAxisMinimum, (Double)plotGroup.AxisX.InternalAxisMaximum, xValues[i]);
                    Double nextXPosition = Graphics.ValueToPixelPosition(0, width, (Double)plotGroup.AxisX.InternalAxisMinimum, (Double)plotGroup.AxisX.InternalAxisMaximum, xValues[i + 1]);
                    Double curYPosition = Graphics.ValueToPixelPosition(height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, curBase + curYValues[index]);
                    Double nextYPosition = Graphics.ValueToPixelPosition(height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, nextBase + nextYValues[index]);
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
                            curDataPoints[index].Parent.Storyboard = AnimationHelper.ApplyOpacityAnimation(marker, currentDataSeries, curDataPoints[index].Parent.Storyboard, 1, curDataPoints[index].InternalOpacity * curDataPoints[index].Parent.InternalOpacity);
                    }
                    if (i + 1 == xValues.Length - 1)
                    {
                        if (index == nextYValues.Count - 1)
                            isTopOfStack = true;

                        marker = GetMarkerForDataPoint(chart, height, isTopOfStack, nextDataPoints[index], nextYPosition, nextDataPoints[index].InternalYValue > 0);
                        if (marker != null)
                        {
                            if (nextDataPoints[index].Parent.Storyboard == null)
                                nextDataPoints[index].Parent.Storyboard = new Storyboard();

                            currentDataSeries = nextDataPoints[index].Parent;

                            marker.AddToParent(labelCanvas, nextXPosition, nextYPosition, new Point(0.5, 0.5));
                            // Apply marker animation
                            if (animationEnabled)
                                nextDataPoints[index].Parent.Storyboard = AnimationHelper.ApplyOpacityAnimation(marker, currentDataSeries, nextDataPoints[index].Parent.Storyboard, 1, nextDataPoints[index].InternalOpacity * nextDataPoints[index].Parent.InternalOpacity);
                        }
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

                    foreach (PointCollection points in pointSet)
                    {
                        areaParams.Points = points;
                        Faces faces = curDataPoints[index].Parent.Faces;
                        if (faces.Parts == null)
                            faces.Parts = new List<FrameworkElement>();

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
                    }

                    curBase += curYValues[index];
                    nextBase += nextYValues[index];
                }

            }
            if (!plankDrawn && chart.View3D && plotGroup.AxisY.InternalAxisMinimum < 0 && plotGroup.AxisY.InternalAxisMaximum > 0)
            {
                RectangularChartShapeParams columnParams = new RectangularChartShapeParams();
                columnParams.BackgroundBrush = new SolidColorBrush(Color.FromArgb((Byte)255, (Byte)255, (Byte)255, (Byte)255));
                columnParams.Lighting = true;
                columnParams.Size = new Size(width, 1);
                columnParams.Depth = depth3d;

                Faces zeroPlank = ColumnChart.Get3DColumn(columnParams);
                Panel zeroPlankVisual = zeroPlank.Visual as Panel;

                zeroPlankVisual.IsHitTestVisible = false;

                Double top = height - Graphics.ValueToPixelPosition(0, height, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, 0);
                zeroPlankVisual.SetValue(Canvas.LeftProperty, (Double)0);
                zeroPlankVisual.SetValue(Canvas.TopProperty, top);
                zeroPlankVisual.SetValue(Canvas.ZIndexProperty, 0);
                zeroPlankVisual.Opacity = 0.7;
                visual.Children.Add(zeroPlankVisual);
            }

            RectangleGeometry clipRectangle = new RectangleGeometry();
            clipRectangle.Rect = new Rect(0, -depth3d, width + depth3d, height + depth3d + chart.ChartArea.PLANK_THICKNESS);
            areaCanvas.Clip = clipRectangle;

            visual.Children.Add(areaCanvas);

            // Clip the label canvas

            clipRectangle = new RectangleGeometry();

            Double clipLeft = 0;
            Double clipTop = -depth3d;
            Double clipWidth = width + depth3d;
            Double clipHeight = height + depth3d + chart.ChartArea.PLANK_THICKNESS + 6;

            GetClipCoordinates(chart, ref clipLeft, ref clipTop, ref clipWidth, ref clipHeight, minimumXValue, maximumXValue);

            clipRectangle.Rect = new Rect(clipLeft, clipTop, clipWidth, clipHeight);
            labelCanvas.Clip = clipRectangle;

            visual.Children.Add(labelCanvas);
            
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
        internal static Canvas GetVisualObjectForStackedArea100Chart(Double width, Double height, PlotDetails plotDetails, List<DataSeries> seriesList, Chart chart, Double plankDepth, bool animationEnabled)
        {
            if (Double.IsNaN(width) || Double.IsNaN(height) || width <= 0 || height <= 0)
                return null;

            DataSeries currentDataSeries = null; // Current working DataSeries

            Boolean plankDrawn = false;
            Double depth3d = plankDepth / plotDetails.Layer3DCount * (chart.View3D ? 1 : 0);
            Double visualOffset = depth3d * (plotDetails.SeriesDrawingIndex[seriesList[0]] + 1);

            if (Double.IsNaN(visualOffset) || Double.IsInfinity(visualOffset))
                return null;

            Canvas visual = new Canvas() { Width = width, Height = height };
            Canvas labelCanvas = new Canvas() { Width = width, Height = height };
            Canvas areaCanvas = new Canvas() { Width = width, Height = height };

            visual.SetValue(Canvas.TopProperty, visualOffset);
            visual.SetValue(Canvas.LeftProperty, -visualOffset);

            var plotgroups = (from series in seriesList where series.PlotGroup != null select series.PlotGroup);

            if (plotgroups.Count() == 0)
                return visual;

            PlotGroup plotGroup = plotgroups.First();

            Dictionary<Double, List<Double>> dataPointValuesInStackedOrder = plotDetails.GetDataPointValuesInStackedOrder(plotGroup);

            Dictionary<Double, List<DataPoint>> dataPointInStackedOrder = plotDetails.GetDataPointInStackOrder(plotGroup);

            Double[] xValues = dataPointValuesInStackedOrder.Keys.ToArray();

            Double limitingYValue = 0;

            Double minimumXValue = plotGroup.MinimumX;
            Double maximumXValue = plotGroup.MaximumX;

            if (plotGroup.AxisY.InternalAxisMinimum > 0)
                limitingYValue = (Double)plotGroup.AxisY.InternalAxisMinimum;
            if (plotGroup.AxisY.InternalAxisMaximum < 0)
                limitingYValue = (Double)plotGroup.AxisY.InternalAxisMaximum;

            foreach (DataSeries ds in seriesList)
            {
                ds.Faces = null;
            }

            Double limitingYPosition = Graphics.ValueToPixelPosition(height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, limitingYValue);

            Marker marker;

            List<Double> curYValues;
            List<Double> nextYValues;

            List<DataPoint> curDataPoints;
            List<DataPoint> nextDataPoints;

            for (Int32 i = 0; i < xValues.Length - 1; i++)
            {
                curYValues = dataPointValuesInStackedOrder[xValues[i]];
                nextYValues = dataPointValuesInStackedOrder[xValues[i + 1]];

                Double curBase = limitingYValue;
                Double nextBase = limitingYValue;
                Double curAbsoluteSum = plotGroup.XWiseStackedDataList[xValues[i]].AbsoluteYValueSum;
                Double nextAbsoluteSum = plotGroup.XWiseStackedDataList[xValues[i + 1]].AbsoluteYValueSum;

                curDataPoints = dataPointInStackedOrder[xValues[i]];

                nextDataPoints = dataPointInStackedOrder[xValues[i + 1]];

                if (Double.IsNaN(curAbsoluteSum))
                    curAbsoluteSum = 1;

                if (Double.IsNaN(nextAbsoluteSum))
                    nextAbsoluteSum = 1;

                for (Int32 index = 0; index < curYValues.Count; index++)
                {

                    if (index >= nextYValues.Count || index >= curYValues.Count || curDataPoints[index] == null || nextDataPoints[index] == null)
                        continue;

                    Double curPercentageY = curYValues[index] / curAbsoluteSum * 100;
                    Double nextPercentageY = nextYValues[index] / nextAbsoluteSum * 100;

                    if (Double.IsNaN(nextPercentageY) || Double.IsNaN(curPercentageY))
                        continue;

                    Double curXPosition = Graphics.ValueToPixelPosition(0, width, (Double)plotGroup.AxisX.InternalAxisMinimum, (Double)plotGroup.AxisX.InternalAxisMaximum, xValues[i]);
                    Double nextXPosition = Graphics.ValueToPixelPosition(0, width, (Double)plotGroup.AxisX.InternalAxisMinimum, (Double)plotGroup.AxisX.InternalAxisMaximum, xValues[i + 1]);
                    Double curYPosition = Graphics.ValueToPixelPosition(height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, curBase + curPercentageY);
                    Double nextYPosition = Graphics.ValueToPixelPosition(height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, nextBase + nextPercentageY);
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
                            curDataPoints[index].Parent.Storyboard = AnimationHelper.ApplyOpacityAnimation(marker, currentDataSeries, curDataPoints[index].Parent.Storyboard, 1, curDataPoints[index].InternalOpacity * curDataPoints[index].Parent.InternalOpacity);
                    }
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
                                nextDataPoints[index].Parent.Storyboard = AnimationHelper.ApplyOpacityAnimation(marker, currentDataSeries, nextDataPoints[index].Parent.Storyboard, 1, nextDataPoints[index].InternalOpacity * nextDataPoints[index].Parent.InternalOpacity);
                        }
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
                        faces.Parts = new List<FrameworkElement>();

                    foreach (PointCollection points in pointSet)
                    {
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
                    }
                    curBase += curPercentageY;
                    nextBase += nextPercentageY;
                }

            }
            if (!plankDrawn && chart.View3D && plotGroup.AxisY.InternalAxisMinimum < 0 && plotGroup.AxisY.InternalAxisMaximum > 0)
            {
                RectangularChartShapeParams columnParams = new RectangularChartShapeParams();
                columnParams.BackgroundBrush = new SolidColorBrush(Color.FromArgb((Byte)255, (Byte)255, (Byte)255, (Byte)255));
                columnParams.Lighting = true;
                columnParams.Size = new Size(width, 1);
                columnParams.Depth = depth3d;

                Faces zeroPlank = ColumnChart.Get3DColumn(columnParams);
                Panel zeroPlankVisual = zeroPlank.Visual as Panel;

                zeroPlankVisual.IsHitTestVisible = false;

                Double top = height - Graphics.ValueToPixelPosition(0, height, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, 0);
                zeroPlankVisual.SetValue(Canvas.LeftProperty, (Double)0);
                zeroPlankVisual.SetValue(Canvas.TopProperty, top);
                zeroPlankVisual.SetValue(Canvas.ZIndexProperty, 0);
                zeroPlankVisual.Opacity = 0.7;
                visual.Children.Add(zeroPlankVisual);
            }

            RectangleGeometry clipRectangle = new RectangleGeometry();
            clipRectangle.Rect = new Rect(0, -depth3d, width + depth3d, height + depth3d + chart.ChartArea.PLANK_THICKNESS);
            areaCanvas.Clip = clipRectangle;

            visual.Children.Add(areaCanvas);

            // Clip the label canvas

            clipRectangle = new RectangleGeometry();

            Double clipLeft = 0;
            Double clipTop = -depth3d;
            Double clipWidth = width + depth3d;
            Double clipHeight = height + depth3d + chart.ChartArea.PLANK_THICKNESS + 6;

            GetClipCoordinates(chart, ref clipLeft, ref clipTop, ref clipWidth, ref clipHeight, minimumXValue, maximumXValue);

            clipRectangle.Rect = new Rect(clipLeft, clipTop, clipWidth, clipHeight);
            labelCanvas.Clip = clipRectangle;

            visual.Children.Add(labelCanvas);

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
                faces.Parts = new List<FrameworkElement>();

            Canvas visual = new Canvas();

            visual.Width = areaParams.Size.Width;
            visual.Height = areaParams.Size.Height;

            Polygon polygon = new Polygon() { Tag = new ElementData() { Element = areaParams.TagReference, VisualElementName = "AreaBase" } };

            faces.Parts.Add(polygon);

            polygon.Fill = areaParams.Lighting ? Graphics.GetLightingEnabledBrush(areaParams.Background, "Linear", null) : areaParams.Background;

            polygon.StrokeDashArray = areaParams.BorderStyle != null ? ExtendedGraphics.CloneCollection(areaParams.BorderStyle) : areaParams.BorderStyle;
            polygon.StrokeThickness = areaParams.BorderThickness;

            if(polygon.StrokeThickness > 0)
                polygon.Stroke = areaParams.BorderColor;

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

            //faces.VisualComponents.Add(visual);

            return visual;
        }

        /// <summary>
        /// Get visual of Area 3D
        /// </summary>
        /// <param name="faces">Faces</param>
        /// <param name="areaParams">AreaParams</param>
        /// <returns>Canvas</returns>
        internal static Canvas Get3DArea(DataSeries currentDataSeries, ref Faces faces, PolygonalChartShapeParams areaParams)
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
                Polygon sides = new Polygon() { Tag = new ElementData() { Element = areaParams.TagReference }};
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

                sides.StrokeDashArray = areaParams.BorderStyle != null ? ExtendedGraphics.CloneCollection(areaParams.BorderStyle) : areaParams.BorderStyle;
                sides.StrokeThickness = areaParams.BorderThickness;

                if(sides.StrokeThickness > 0)
                    sides.Stroke = areaParams.BorderColor;

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

            polygon.StrokeDashArray = areaParams.BorderStyle != null ? ExtendedGraphics.CloneCollection(areaParams.BorderStyle) : areaParams.BorderStyle;
            polygon.StrokeThickness = areaParams.BorderThickness;

            if(polygon.StrokeThickness > 0)
                polygon.Stroke = areaParams.BorderColor;

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

        /// <summary>
        /// Get visual for StackedArea 2D
        /// </summary>
        /// <param name="faces">Faces</param>
        /// <param name="areaParams">Area parameters</param>
        /// <returns>Canvas</returns>
        internal static Canvas GetStacked2DArea(ref Faces faces, PolygonalChartShapeParams areaParams)
        {
            if (faces.Parts == null)
                faces.Parts = new List<FrameworkElement>();

            Canvas visual = new Canvas();

            visual.Width = areaParams.Size.Width;
            visual.Height = areaParams.Size.Height;

            Polygon polygon = new Polygon() { Tag = new ElementData() { Element = areaParams.TagReference, VisualElementName = "AreaBase" } };

            faces.Parts.Add(polygon);

            polygon.Fill = areaParams.Lighting ? Graphics.GetLightingEnabledBrush(areaParams.Background, "Linear", null) : areaParams.Background;

            polygon.StrokeDashArray = areaParams.BorderStyle != null ? ExtendedGraphics.CloneCollection(areaParams.BorderStyle) : areaParams.BorderStyle;
            polygon.StrokeThickness = areaParams.BorderThickness;

            if(polygon.StrokeThickness > 0)
                polygon.Stroke = areaParams.BorderColor;

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

            polygon.StrokeDashArray = areaParams.BorderStyle != null ? ExtendedGraphics.CloneCollection(areaParams.BorderStyle) : areaParams.BorderStyle;
            polygon.StrokeThickness = areaParams.BorderThickness;

            if(polygon.StrokeThickness > 0)
                polygon.Stroke = areaParams.BorderColor;

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



