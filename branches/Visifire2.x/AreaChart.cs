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
    internal class PolygonalChartShapeParams
    {
        public PointCollection Points { get; set; }
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
        public Storyboard Storyboard { get; set; }
        public Boolean AnimationEnabled { get; set; }
        public Size Size { get; set; }
    }

    internal class AreaChart
    {
        internal static Marker GetMarkerForDataPoint(Chart chart, DataPoint dataPoint, Double position, bool isPositive)
        {
            if((Boolean)dataPoint.MarkerEnabled || (Boolean)dataPoint.LabelEnabled)
            {   
                Size markerSize = (Boolean)dataPoint.MarkerEnabled ? new Size((Double)dataPoint.MarkerSize, (Double)dataPoint.MarkerSize) : new Size(0, 0);
                String labelText = (Boolean)dataPoint.LabelEnabled ? dataPoint.TextParser(dataPoint.LabelText) : "";
                //Boolean markerBevel = dataPoint.Parent.Bevel ? dataPoint.Parent.Bevel : false;
                Boolean markerBevel = false;
                dataPoint.Marker = new Marker((MarkerTypes)dataPoint.MarkerType, (Double)dataPoint.MarkerScale, markerSize, markerBevel, dataPoint.MarkerColor, labelText);

                dataPoint.Marker.MarkerSize = markerSize;
                dataPoint.Marker.BorderColor = dataPoint.MarkerBorderColor;
                dataPoint.Marker.BorderThickness = ((Thickness)dataPoint.MarkerBorderThickness).Left;

                dataPoint.Marker.FontColor = Graphics.ApplyLabelFontColor(chart, dataPoint, dataPoint.LabelFontColor, LabelStyles.OutSide);
                dataPoint.Marker.FontFamily = dataPoint.LabelFontFamily;
                dataPoint.Marker.FontSize = (Double)dataPoint.LabelFontSize;
                dataPoint.Marker.FontStyle = (FontStyle)dataPoint.LabelFontStyle;
                dataPoint.Marker.FontWeight = (FontWeight)dataPoint.LabelFontWeight;
                dataPoint.Marker.TextBackground = dataPoint.LabelBackground;
                dataPoint.Marker.MarkerFillColor = dataPoint.MarkerColor;

                if (true && !String.IsNullOrEmpty(labelText))
                {
                    dataPoint.Marker.CreateVisual();

                    dataPoint.Marker.TextAlignmentX = AlignmentX.Center;
                    if(isPositive)
                    {
                        if (position < dataPoint.Marker.MarkerActualSize.Height || dataPoint.LabelStyle == LabelStyles.Inside)
                            dataPoint.Marker.TextAlignmentY = AlignmentY.Bottom;
                        else
                            dataPoint.Marker.TextAlignmentY = AlignmentY.Top;
                    }
                    else
                    {
                        if (position + dataPoint.Marker.MarkerActualSize.Height > chart.PlotArea.PlotAreaBorderElement.Height || dataPoint.LabelStyle == LabelStyles.Inside)
                            dataPoint.Marker.TextAlignmentY = AlignmentY.Top;
                        else
                            dataPoint.Marker.TextAlignmentY = AlignmentY.Bottom;
                    }
                }
              
                dataPoint.Marker.CreateVisual();

                return dataPoint.Marker;
            }
            return null;
        }

        internal static Canvas GetVisualObjectForAreaChart(Double width, Double height, PlotDetails plotDetails, List<DataSeries> seriesList, Chart chart, Double plankDepth,bool animationEnabled)
        {
            if (Double.IsNaN(width) || Double.IsNaN(height) || width <= 0 || height <= 0) 
                return null;

            Boolean plankDrawn = false;

            Canvas visual = new Canvas();
            visual.Width = width;
            visual.Height = height;

            Canvas labelCanvas = new Canvas();
            labelCanvas.Width = width;
            labelCanvas.Height = height;

            Canvas areaCanvas = new Canvas();
            areaCanvas.Width = width;
            areaCanvas.Height = height;

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

                DataSeriesRef = series;

                Faces faces = new Faces();
                series.Faces = faces;
                series.Faces.Parts = new List<FrameworkElement>();

                PlotGroup plotGroup = series.PlotGroup;

                Brush areaBrush = series.Color;

                Double limitingYValue = 0;

                if (plotGroup.AxisY.InternalAxisMinimum > 0)
                    limitingYValue = (Double)plotGroup.AxisY.InternalAxisMinimum;

                if (plotGroup.AxisY.InternalAxisMaximum < 0)
                    limitingYValue = (Double) plotGroup.AxisY.InternalAxisMaximum;

                PolygonalChartShapeParams areaParams = new PolygonalChartShapeParams();
                areaParams.BackgroundBrush = areaBrush;
                areaParams.Lighting = (Boolean)series.LightingEnabled;
                areaParams.Shadow = series.ShadowEnabled;
                areaParams.ShadowOffset = 0;
                areaParams.Bevel = series.Bevel;
                areaParams.BorderBrush = series.BorderColor;
                areaParams.BorderStyle = ExtendedGraphics.GetDashArray(series.BorderStyle);
                areaParams.BorderThickness = series.BorderThickness.Left;
                areaParams.Depth = depth3d;
                areaParams.Storyboard = series.Storyboard;
                areaParams.AnimationEnabled = animationEnabled;
                areaParams.Size = new Size(width, height);
                
                PointCollection points = new PointCollection();

                List<DataPoint> enabledDataPoints = (from datapoint in series.DataPoints where datapoint.Enabled == true select datapoint).ToList();

                DataPoint currentDataPoint = enabledDataPoints[0];
                DataPoint nextDataPoint;

                Double xPosition = Graphics.ValueToPixelPosition(0, width, (Double)plotGroup.AxisX.InternalAxisMinimum, (Double)plotGroup.AxisX.InternalAxisMaximum, currentDataPoint.XValue);
                Double yPosition = Graphics.ValueToPixelPosition(height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, limitingYValue);

                points.Add(new Point(xPosition, yPosition));

                for (Int32 i = 0; i < enabledDataPoints.Count - 1; i++)
                {
                    currentDataPoint = enabledDataPoints[i];
                    nextDataPoint = enabledDataPoints[i + 1];

                    if (Double.IsNaN(currentDataPoint.YValue)) continue;

                    xPosition = Graphics.ValueToPixelPosition(0, width, (Double)plotGroup.AxisX.InternalAxisMinimum, (Double)plotGroup.AxisX.InternalAxisMaximum, currentDataPoint.XValue);
                    yPosition = Graphics.ValueToPixelPosition(height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, currentDataPoint.YValue);

                    points.Add(new Point(xPosition, yPosition));

                    marker = GetMarkerForDataPoint(chart, currentDataPoint, yPosition, currentDataPoint.YValue>0);
                    if (marker != null)
                    {
                        marker.AddToParent(labelCanvas, xPosition, yPosition, new Point(0.5, 0.5));

                        // Apply marker animation
                        if (animationEnabled)
                        {
                            if (currentDataPoint.Parent.Storyboard == null)
                                currentDataPoint.Parent.Storyboard = new Storyboard();

                            currentDataPoint.Parent.Storyboard = ApplyMarkerAnimationToAreaChart(marker, currentDataPoint.Parent.Storyboard, 1);
                        }
                    }
                    if (Math.Sign(currentDataPoint.YValue) != Math.Sign(nextDataPoint.YValue))
                    {
                        Double xNextPosition = Graphics.ValueToPixelPosition(0, width, (Double)plotGroup.AxisX.InternalAxisMinimum, (Double)plotGroup.AxisX.InternalAxisMaximum, nextDataPoint.XValue);
                        Double yNextPosition = Graphics.ValueToPixelPosition(height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, nextDataPoint.YValue);

                        Double limitingYPosition = Graphics.ValueToPixelPosition(height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, limitingYValue);
                        Double xNew = Graphics.ConvertScale(yPosition, yNextPosition, limitingYPosition, xPosition, xNextPosition);
                        Double yNew = Graphics.ValueToPixelPosition(height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, limitingYValue);

                        points.Add(new Point(xNew, yNew));

                        // get the faces
                        areaParams.Points = points;
                        areaParams.IsPositive = (currentDataPoint.YValue>0);

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

                xPosition = Graphics.ValueToPixelPosition(0, width, (Double)plotGroup.AxisX.InternalAxisMinimum, (Double)plotGroup.AxisX.InternalAxisMaximum, lastDataPoint.XValue);
                yPosition = Graphics.ValueToPixelPosition(height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, lastDataPoint.YValue);

                points.Add(new Point(xPosition, yPosition));

                marker = GetMarkerForDataPoint(chart, lastDataPoint, yPosition, lastDataPoint.YValue > 0);

                if (marker != null)
                {
                    marker.AddToParent(labelCanvas, xPosition, yPosition, new Point(0.5, 0.5));
                    // Apply marker animation
                    if (animationEnabled)
                    {
                        if (lastDataPoint.Parent.Storyboard == null)
                            lastDataPoint.Parent.Storyboard = new Storyboard();

                        lastDataPoint.Parent.Storyboard = ApplyMarkerAnimationToAreaChart(marker, lastDataPoint.Parent.Storyboard, 1);
                    }
                }

                xPosition = Graphics.ValueToPixelPosition(0, width, (Double)plotGroup.AxisX.InternalAxisMinimum, (Double)plotGroup.AxisX.InternalAxisMaximum, lastDataPoint.XValue);
                yPosition = Graphics.ValueToPixelPosition(height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, limitingYValue);

                points.Add(new Point(xPosition, yPosition));

                // get the faces
                areaParams.Points = points;
                areaParams.IsPositive = (lastDataPoint.YValue > 0);

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
                    Panel zeroPlankVisual = zeroPlank.Visual;

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

        internal static Canvas GetVisualObjectForStackedAreaChart(Double width, Double height, PlotDetails plotDetails, List<DataSeries> seriesList, Chart chart, Double plankDepth, bool animationEnabled)
        {
            if (Double.IsNaN(width) || Double.IsNaN(height) || width <= 0 || height <= 0)
                return null;

            Boolean plankDrawn = false;

            Canvas visual = new Canvas();
            visual.Width = width;
            visual.Height = height;

            Canvas labelCanvas = new Canvas();
            labelCanvas.Width = width;
            labelCanvas.Height = height;

            Canvas areaCanvas = new Canvas();
            areaCanvas.Width = width;
            areaCanvas.Height = height;

            Double depth3d = plankDepth / plotDetails.Layer3DCount * (chart.View3D ? 1 : 0);
            Double visualOffset = depth3d * (plotDetails.SeriesDrawingIndex[seriesList[0]] + 1);
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
            if (plotGroup.AxisY.InternalAxisMinimum > 0)
                limitingYValue = (Double)plotGroup.AxisY.InternalAxisMinimum;
            if (plotGroup.AxisY.InternalAxisMaximum < 0)
                limitingYValue = (Double)plotGroup.AxisY.InternalAxisMaximum;

            Double limitingYPosition = Graphics.ValueToPixelPosition(height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, limitingYValue);

            Marker marker;

            for (Int32 i=0; i < xValues.Length-1;i++)
            {
                List<Double> curYValues = dataPointValuesInStackedOrder[xValues[i]];
                List<Double> nextYValues = dataPointValuesInStackedOrder[xValues[i + 1]];

                Double curBase = limitingYValue;
                Double nextBase = limitingYValue;

                List<DataPoint> curDataPoints = dataPointInStackedOrder[xValues[i]];
                List<DataPoint> nextDataPoints = dataPointInStackedOrder[xValues[i + 1]];

                for(Int32 index = 0;index<curYValues.Count;index++)
                {
                    Double curXPosition = Graphics.ValueToPixelPosition(0, width, (Double)plotGroup.AxisX.InternalAxisMinimum, (Double)plotGroup.AxisX.InternalAxisMaximum, xValues[i]);
                    Double nextXPosition = Graphics.ValueToPixelPosition(0, width, (Double)plotGroup.AxisX.InternalAxisMinimum, (Double)plotGroup.AxisX.InternalAxisMaximum, xValues[i + 1]);
                    Double curYPosition = Graphics.ValueToPixelPosition(height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, curBase + curYValues[index]);
                    Double nextYPosition = Graphics.ValueToPixelPosition(height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, nextBase + nextYValues[index]);
                    Double curYBase = Graphics.ValueToPixelPosition(height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, curBase);
                    Double nextYBase = Graphics.ValueToPixelPosition(height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, nextBase);

                    Point intersect = GetIntersection(new Point(curXPosition, curYBase), new Point(nextXPosition, nextYBase),
                                                new Point(curXPosition, curYPosition), new Point(nextXPosition, nextYPosition));
                    
                    marker = GetMarkerForDataPoint(chart, curDataPoints[index], curYPosition, curDataPoints[index].YValue > 0);
                    if (marker != null)
                    {   
                        if (curDataPoints[index].Parent.Storyboard == null)
                            curDataPoints[index].Parent.Storyboard = new Storyboard();

                        DataSeriesRef = curDataPoints[index].Parent;

                        marker.AddToParent(labelCanvas, curXPosition, curYPosition, new Point(0.5, 0.5));
                        // Apply marker animation
                        if (animationEnabled)
                            curDataPoints[index].Parent.Storyboard = ApplyMarkerAnimationToAreaChart(marker, curDataPoints[index].Parent.Storyboard, 1);
                    }
                    if (i+1 == xValues.Length - 1)
                    {
                        marker = GetMarkerForDataPoint(chart, nextDataPoints[index], nextYPosition, nextDataPoints[index].YValue > 0);
                        if (marker != null)
                        {
                            if (nextDataPoints[index].Parent.Storyboard == null)
                                nextDataPoints[index].Parent.Storyboard = new Storyboard();

                            DataSeriesRef = nextDataPoints[index].Parent;

                            marker.AddToParent(labelCanvas, nextXPosition, nextYPosition, new Point(0.5, 0.5));
                            // Apply marker animation
                            if (animationEnabled)
                                nextDataPoints[index].Parent.Storyboard = ApplyMarkerAnimationToAreaChart(marker, nextDataPoints[index].Parent.Storyboard, 1);
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

                    DataSeriesRef = series;

                    PolygonalChartShapeParams areaParams = new PolygonalChartShapeParams();
                    areaParams.BackgroundBrush = areaBrush;
                    areaParams.Lighting = (Boolean)series.LightingEnabled;
                    areaParams.Shadow = series.ShadowEnabled;
                    areaParams.ShadowOffset = 0;
                    areaParams.Bevel = series.Bevel;
                    areaParams.BorderBrush = series.BorderColor;
                    areaParams.BorderStyle = ExtendedGraphics.GetDashArray(series.BorderStyle);
                    areaParams.BorderThickness = series.BorderThickness.Left;
                    areaParams.Depth = depth3d;

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
                                storyboard = ApplyStackedAreaAnimation(sideface, storyboard, (1.0 / seriesList.Count) * (seriesList.IndexOf(curDataPoints[index].Parent))+0.05, 1.0 / seriesList.Count);
                                storyboard = ApplyStackedAreaAnimation(frontface, storyboard, (1.0 / seriesList.Count) * (seriesList.IndexOf(curDataPoints[index].Parent)), 1.0 / seriesList.Count);
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
                                storyboard = ApplyStackedAreaAnimation(area2d, storyboard, (1.0 / seriesList.Count) * (seriesList.IndexOf(curDataPoints[index].Parent)), 1.0 / seriesList.Count);
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
                Panel zeroPlankVisual = zeroPlank.Visual;

                Double top = height - Graphics.ValueToPixelPosition(0, height, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, 0);
                zeroPlankVisual.SetValue(Canvas.LeftProperty, (Double)0);
                zeroPlankVisual.SetValue(Canvas.TopProperty, top);
                zeroPlankVisual.SetValue(Canvas.ZIndexProperty, 0);
                zeroPlankVisual.Opacity = 0.7;
                visual.Children.Add(zeroPlankVisual);
            }
            visual.Children.Add(areaCanvas);
            visual.Children.Add(labelCanvas);
            return visual;
        }

        internal static Canvas GetVisualObjectForStackedArea100Chart(Double width, Double height, PlotDetails plotDetails, List<DataSeries> seriesList, Chart chart, Double plankDepth, bool animationEnabled)
        {
            if (Double.IsNaN(width) || Double.IsNaN(height) || width <= 0 || height <= 0)
                return null;

            Boolean plankDrawn = false;

            Canvas visual = new Canvas();
            visual.Width = width;
            visual.Height = height;

            Canvas labelCanvas = new Canvas();
            labelCanvas.Width = width;
            labelCanvas.Height = height;

            Canvas areaCanvas = new Canvas();
            areaCanvas.Width = width;
            areaCanvas.Height = height;

            Double depth3d = plankDepth / plotDetails.Layer3DCount * (chart.View3D ? 1 : 0);
            Double visualOffset = depth3d * (plotDetails.SeriesDrawingIndex[seriesList[0]] + 1);
            visual.SetValue(Canvas.TopProperty, visualOffset);
            visual.SetValue(Canvas.LeftProperty, -visualOffset);

            var plotgroups = (from series in seriesList where series.PlotGroup != null select series.PlotGroup);
            if(plotgroups.Count() == 0)
                return visual;
            PlotGroup plotGroup = plotgroups.First();

            Dictionary<Double, List<Double>> dataPointValuesInStackedOrder = plotDetails.GetDataPointValuesInStackedOrder(plotGroup);

            Dictionary<Double, List<DataPoint>> dataPointInStackedOrder = plotDetails.GetDataPointInStackOrder(plotGroup);

            Double[] xValues = dataPointValuesInStackedOrder.Keys.ToArray();

            Double limitingYValue = 0;
            if (plotGroup.AxisY.InternalAxisMinimum > 0)
                limitingYValue = (Double)plotGroup.AxisY.InternalAxisMinimum;
            if (plotGroup.AxisY.InternalAxisMaximum < 0)
                limitingYValue = (Double)plotGroup.AxisY.InternalAxisMaximum;

            Double limitingYPosition = Graphics.ValueToPixelPosition(height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, limitingYValue);

            Marker marker;

            for (Int32 i = 0; i < xValues.Length - 1; i++)
            {
                List<Double> curYValues = dataPointValuesInStackedOrder[xValues[i]];
                List<Double> nextYValues = dataPointValuesInStackedOrder[xValues[i + 1]];

                Double curBase = limitingYValue;
                Double nextBase = limitingYValue;
                Double curAbsoluteSum = plotGroup.XWiseStackedDataList[xValues[i]].AbsoluteYValueSum;
                Double nextAbsoluteSum = plotGroup.XWiseStackedDataList[xValues[i+1]].AbsoluteYValueSum;

                List<DataPoint> curDataPoints = dataPointInStackedOrder[xValues[i]];
                List<DataPoint> nextDataPoints = dataPointInStackedOrder[xValues[i + 1]];

                if (Double.IsNaN(curAbsoluteSum))
                    curAbsoluteSum = 1;

                if (Double.IsNaN(nextAbsoluteSum))
                    nextAbsoluteSum = 1;

                for (Int32 index = 0; index < curYValues.Count; index++)
                {

                    Double curPercentageY = curYValues[index] / curAbsoluteSum * 100;
                    Double nextPercentageY = nextYValues[index] / nextAbsoluteSum * 100;

                    Double curXPosition = Graphics.ValueToPixelPosition(0, width, (Double)plotGroup.AxisX.InternalAxisMinimum, (Double)plotGroup.AxisX.InternalAxisMaximum, xValues[i]);
                    Double nextXPosition = Graphics.ValueToPixelPosition(0, width, (Double)plotGroup.AxisX.InternalAxisMinimum, (Double)plotGroup.AxisX.InternalAxisMaximum, xValues[i + 1]);
                    Double curYPosition = Graphics.ValueToPixelPosition(height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, curBase + curPercentageY);
                    Double nextYPosition = Graphics.ValueToPixelPosition(height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, nextBase + nextPercentageY);
                    Double curYBase = Graphics.ValueToPixelPosition(height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, curBase);
                    Double nextYBase = Graphics.ValueToPixelPosition(height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, nextBase);

                    Point intersect = GetIntersection(new Point(curXPosition, curYBase), new Point(nextXPosition, nextYBase),
                                                new Point(curXPosition, curYPosition), new Point(nextXPosition, nextYPosition));

                    marker = GetMarkerForDataPoint(chart, curDataPoints[index], curYPosition, curDataPoints[index].YValue > 0);
                    if (marker != null)
                    {
                        if (curDataPoints[index].Parent.Storyboard == null)
                            curDataPoints[index].Parent.Storyboard = new Storyboard();
                        
                        DataSeriesRef = curDataPoints[index].Parent;

                        marker.AddToParent(labelCanvas, curXPosition, curYPosition, new Point(0.5, 0.5));
                        // Apply marker animation
                        if (animationEnabled)
                            curDataPoints[index].Parent.Storyboard = ApplyMarkerAnimationToAreaChart(marker, curDataPoints[index].Parent.Storyboard, 1);
                    }
                    if (i + 1 == xValues.Length - 1)
                    {
                        marker = GetMarkerForDataPoint(chart, nextDataPoints[index], nextYPosition, nextDataPoints[index].YValue > 0);
                        if (marker != null)
                        {
                            if (curDataPoints[index].Parent.Storyboard == null)
                                curDataPoints[index].Parent.Storyboard = new Storyboard();

                            DataSeriesRef = curDataPoints[index].Parent;

                            marker.AddToParent(labelCanvas, nextXPosition, nextYPosition, new Point(0.5, 0.5));
                            // Apply marker animation
                            if (animationEnabled)
                                nextDataPoints[index].Parent.Storyboard = ApplyMarkerAnimationToAreaChart(marker, nextDataPoints[index].Parent.Storyboard, 1);
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

                    DataSeriesRef = series;

                    PolygonalChartShapeParams areaParams = new PolygonalChartShapeParams();
                    areaParams.BackgroundBrush = areaBrush;
                    areaParams.Lighting = (Boolean)series.LightingEnabled;
                    areaParams.Shadow = series.ShadowEnabled;
                    areaParams.ShadowOffset = 0;
                    areaParams.Bevel = series.Bevel;
                    areaParams.BorderBrush = series.BorderColor;
                    areaParams.BorderStyle = ExtendedGraphics.GetDashArray(series.BorderStyle);
                    areaParams.BorderThickness = series.BorderThickness.Left;
                    areaParams.Depth = depth3d;

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
                                storyboard = ApplyStackedAreaAnimation(sideface, storyboard, (1.0 / seriesList.Count) * (seriesList.IndexOf(curDataPoints[index].Parent)) + 0.05, 1.0 / seriesList.Count);
                                storyboard = ApplyStackedAreaAnimation(frontface, storyboard, (1.0 / seriesList.Count) * (seriesList.IndexOf(curDataPoints[index].Parent)), 1.0 / seriesList.Count);
                            }
                        }
                        else
                        {
                            Canvas area2d = Get2DArea(ref faces, areaParams);
                            areaCanvas.Children.Add(area2d);
                            curDataPoints[index].Parent.Faces.VisualComponents.Add(area2d);
                            if (animationEnabled)
                            {
                                if (curDataPoints[index].Parent.Storyboard == null)
                                    curDataPoints[index].Parent.Storyboard = new Storyboard();
                                Storyboard storyboard = curDataPoints[index].Parent.Storyboard;

                                // apply animation to the various faces
                                storyboard = ApplyStackedAreaAnimation(area2d, storyboard, (1.0 / seriesList.Count) * (seriesList.IndexOf(curDataPoints[index].Parent)), 1.0 / seriesList.Count);
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
                Panel zeroPlankVisual = zeroPlank.Visual;

                Double top = height - Graphics.ValueToPixelPosition(0, height, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, 0);
                zeroPlankVisual.SetValue(Canvas.LeftProperty, (Double)0);
                zeroPlankVisual.SetValue(Canvas.TopProperty, top);
                zeroPlankVisual.SetValue(Canvas.ZIndexProperty, 0);
                zeroPlankVisual.Opacity = 0.7;
                visual.Children.Add(zeroPlankVisual);
            }

            visual.Children.Add(areaCanvas);
            visual.Children.Add(labelCanvas);
            return visual;
        }

        internal static Point GetCentroid(PointCollection points)
        {
            Double sumX = 0;
            Double sumY = 0;
            foreach (Point point in points)
            {
                sumX += point.X;
                sumY += point.Y;
            }

            return new Point(sumX/points.Count,sumY/points.Count);
        }

        internal static List<PointCollection> GeneratePointsCollection(Double curX, Double curY, Double curBase, Double nextX, Double nextY, Double nextBase,Double limitingY)
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

                points.Add(new Point(curX,curBase));
                points.Add(new Point(curX, limitingY));
                points.Add(new Point(midX, limitingY));
                points.Add(new Point(nextX, nextY));
                points.Add(new Point(nextX,nextBase));
                
                pointsSet.Add(points);
            }
            else if(curY < limitingY && nextY < limitingY && curBase < limitingY && nextBase > limitingY)
            {
                points = new PointCollection();
                Double midX = Graphics.ConvertScale(curBase, nextBase, limitingY, curX, nextX);
                points.Add(new Point(midX,limitingY));
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
            else if ((curY < limitingY && nextY < limitingY && curBase > limitingY && nextBase > limitingY)||
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
                points.Add(new Point(midX1,limitingY));
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
            else if(curY > limitingY && nextY < limitingY && curBase < limitingY && nextBase < limitingY)
            {
                points = new PointCollection();
                Double midX = Graphics.ConvertScale(curY, nextY, limitingY, curX, nextX);
                points.Add(new Point(midX, limitingY));
                points.Add(new Point(curX, limitingY));
                points.Add(new Point(curX, curBase));
                points.Add(new Point(nextX, nextBase));
                points.Add(new Point(nextX,nextY));
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

        internal static Point GetCrossingPointWithSmallestX(Double curX, List<Double> curYValues, Double nextX, List<Double> nextYValues, Double curBase,Double nextBase,Int32 startIndex)
        {
            Point crossingPoint = new Point(Double.MaxValue,Double.MaxValue);

            for (Int32 index = startIndex; index < curYValues.Count; index++)
            {
                Point newPoint = GetIntersection(new Point(curX, curBase), new Point(nextX, nextBase),
                                                new Point(curX, curYValues[index]), new Point(nextX, nextYValues[index]));
                if (newPoint.X < crossingPoint.X)
                    crossingPoint = newPoint;
            }

            if(crossingPoint.X == Double.MaxValue)
                return new Point(Double.NaN, Double.NaN);

            return crossingPoint;
        }

        internal static Double GetSlope(Double x1, Double y1, Double x2, Double y2)
        {
            return (y2 - y1) / (x2 - x1);
        }

        internal static Double GetIntercept(Double x1, Double y1, Double x2, Double y2)
        {
            return y1 - x1 * GetSlope(x1, y1, x2, y2);
        }

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

        internal static Canvas Get2DArea(ref Faces faces, PolygonalChartShapeParams areaParams)
        {
            if (faces.Parts == null)
                faces.Parts = new List<FrameworkElement>();

            Canvas visual = new Canvas();

            visual.Width = areaParams.Size.Width;
            visual.Height = areaParams.Size.Height;

            Polygon polygon = new Polygon() { Tag = "AreaBase" };

            faces.Parts.Add(polygon);

            polygon.Fill = areaParams.Lighting ? Graphics.GetLightingEnabledBrush(areaParams.BackgroundBrush, "Linear", null) : areaParams.BackgroundBrush;

            polygon.Stroke = areaParams.BorderBrush;
            polygon.StrokeDashArray = areaParams.BorderStyle;
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
                areaParams.Storyboard = ApplyAreaAnimation(polygon, areaParams.Storyboard, areaParams.IsPositive, 0);
            }
            visual.Children.Add(polygon);

            if (areaParams.Bevel)
            {
                for (int i = 0; i < areaParams.Points.Count - 1; i++)
                {
                    if (areaParams.Points[i].X == areaParams.Points[i+1].X)
                        continue;

                    Double m = GetSlope(areaParams.Points[i].X, areaParams.Points[i].Y, areaParams.Points[i + 1].X, areaParams.Points[i + 1].Y);
                    Double c = GetIntercept(areaParams.Points[i].X, areaParams.Points[i].Y, areaParams.Points[i + 1].X, areaParams.Points[i + 1].Y);
                    c = c + (areaParams.IsPositive?1:-1) * 4;
                    Point newPt1 = new Point(areaParams.Points[i].X, m * areaParams.Points[i].X + c);
                    Point newPt2 = new Point(areaParams.Points[i+1].X, m * areaParams.Points[i+1].X + c);

                    PointCollection points = new PointCollection();
                    points.Add(areaParams.Points[i]);
                    points.Add(areaParams.Points[i+1]);
                    points.Add(newPt2);
                    points.Add(newPt1);

                    Polygon bevel = new Polygon();
                    bevel.Points = points;
                    bevel.Fill = Graphics.GetBevelTopBrush(areaParams.BackgroundBrush);

                    if (areaParams.AnimationEnabled)
                    {
                        areaParams.Storyboard = CreateOpacityAnimation(areaParams.Storyboard, bevel, 1, 1, 1);
                        bevel.Opacity = 0;
                    }

                    bevel.Tag = "Bevel";
                    faces.Parts.Add(bevel);

                    visual.Children.Add(bevel);
                }
            }

            return visual;
        }

        private static Storyboard CreateOpacityAnimation(Storyboard storyboard, DependencyObject target, Double beginTime, Double opacity, Double duration)
        {
            DoubleCollection values = Graphics.GenerateDoubleCollection(0, opacity);
            DoubleCollection frames = Graphics.GenerateDoubleCollection(0, duration);
            List<KeySpline> splines = Graphics.GenerateAnimationSplines(frames.Count);
            DoubleAnimationUsingKeyFrames opacityAnimation = Graphics.CreateDoubleAnimation(DataSeriesRef, target, "(UIElement.Opacity)", beginTime + 0.5, frames, values, splines);
            storyboard.Stop();
            storyboard.Children.Add(opacityAnimation);
            return storyboard;
        }

        internal static Canvas Get3DArea(ref Faces faces, PolygonalChartShapeParams areaParams)
        {
            Canvas visual = new Canvas();

            visual.Width = areaParams.Size.Width;
            visual.Height = areaParams.Size.Height;

            Point centroid;
            Brush sideBrush = areaParams.Lighting ? GetRightFaceBrush(areaParams.BackgroundBrush) : areaParams.BackgroundBrush; 
            Brush topBrush = areaParams.Lighting ? Graphics.GetTopFaceBrush(areaParams.BackgroundBrush) : areaParams.BackgroundBrush;
            Int32 pointIndexLimit = areaParams.IsPositive ? areaParams.Points.Count - 1 : areaParams.Points.Count;

            Canvas polygonSet = new Canvas();
            Rect size = GetBounds(areaParams.Points);
            polygonSet.Width = size.Width + areaParams.Depth;
            polygonSet.Height = size.Height + areaParams.Depth;
            polygonSet.SetValue(Canvas.TopProperty, size.Top - areaParams.Depth);
            polygonSet.SetValue(Canvas.LeftProperty, size.Left);
            visual.Children.Add(polygonSet);

            for (Int32 i = 0; i < pointIndexLimit; i++)
            {
                Polygon sides = new Polygon();
                PointCollection points = new PointCollection();
                Int32 index1 = i % areaParams.Points.Count;
                Int32 index2 = (i + 1) % areaParams.Points.Count;

                points.Add(areaParams.Points[index1]);
                points.Add(areaParams.Points[index2]);
                points.Add(new Point(areaParams.Points[index2].X + areaParams.Depth, areaParams.Points[index2].Y - areaParams.Depth));
                points.Add(new Point(areaParams.Points[index1].X + areaParams.Depth, areaParams.Points[index1].Y - areaParams.Depth));
                sides.Points = points;

                centroid = GetCentroid(points);
                Int32 zindex = GetAreaZIndex(centroid.X, centroid.Y, areaParams.IsPositive);
                sides.SetValue(Canvas.ZIndexProperty,zindex);

                if (i == (areaParams.Points.Count - 2))
                {
                    sides.Fill = sideBrush;
                    sides.Tag = "Side";
                }
                else
                {
                    sides.Fill = topBrush;
                    sides.Tag = "Top";
                }

                sides.Stroke = areaParams.BorderBrush;
                sides.StrokeDashArray = areaParams.BorderStyle;
                sides.StrokeThickness = areaParams.BorderThickness;
                sides.StrokeMiterLimit = 1;

                Rect sidesBounds = GetBounds(points);
                sides.Stretch = Stretch.Fill;
                sides.Width = sidesBounds.Width;
                sides.Height = sidesBounds.Height;
                sides.SetValue(Canvas.TopProperty, sidesBounds.Y - (size.Top - areaParams.Depth));
                sides.SetValue(Canvas.LeftProperty, sidesBounds.X-size.X);

                faces.Parts.Add(sides);
                polygonSet.Children.Add(sides);

            }

            Polygon polygon = new Polygon() { Tag = "AreaBase" };
            faces.Parts.Add(polygon);
            centroid = GetCentroid(areaParams.Points);

            polygon.SetValue(Canvas.ZIndexProperty, (Int32)centroid.Y+1000);
            polygon.Fill = areaParams.Lighting ? Graphics.GetFrontFaceBrush(areaParams.BackgroundBrush) : areaParams.BackgroundBrush;

            polygon.Stroke = areaParams.BorderBrush;
            polygon.StrokeDashArray = areaParams.BorderStyle;
            polygon.StrokeThickness = areaParams.BorderThickness;
            polygon.StrokeMiterLimit = 1;

            polygon.Points = areaParams.Points;

            polygon.Stretch = Stretch.Fill;
            polygon.Width = size.Width;
            polygon.Height = size.Height;
            polygon.SetValue(Canvas.TopProperty, areaParams.Depth);
            polygon.SetValue(Canvas.LeftProperty, 0.0);

            // apply area animation
            if (areaParams.AnimationEnabled)
            {
                // apply animation to the entire canvas that was used to create the area
                areaParams.Storyboard = ApplyAreaAnimation(polygonSet, areaParams.Storyboard, areaParams.IsPositive, 0);

            }
            polygonSet.Children.Add(polygon);

            return visual;
        }

        internal static Canvas GetStacked2DArea(ref Faces faces, PolygonalChartShapeParams areaParams)
        {
            if (faces.Parts == null)
                faces.Parts = new List<FrameworkElement>();

            Canvas visual = new Canvas();

            visual.Width = areaParams.Size.Width;
            visual.Height = areaParams.Size.Height;

            Polygon polygon = new Polygon() { Tag = "AreaBase" };

            faces.Parts.Add(polygon);

            polygon.Fill = areaParams.Lighting ? Graphics.GetLightingEnabledBrush(areaParams.BackgroundBrush, "Linear", null) : areaParams.BackgroundBrush;

            polygon.Stroke = areaParams.BorderBrush;
            polygon.StrokeDashArray = areaParams.BorderStyle;
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

                    Polygon bevel = new Polygon();
                    bevel.Points = points;
                    bevel.Fill = Graphics.GetBevelTopBrush(areaParams.BackgroundBrush);

                    bevel.Tag = "Bevel";
                    faces.Parts.Add(bevel);
                    visual.Children.Add(bevel);
                }

            }

            return visual;

        }

        internal static Canvas GetStacked3DAreaFrontFace(ref Faces faces, PolygonalChartShapeParams areaParams)
        {
            Polygon polygon = new Polygon() { Tag = "AreaBase" };
            faces.Parts.Add(polygon);
            Point centroid = GetCentroid(areaParams.Points);
            Rect size = GetBounds(areaParams.Points);

            polygon.SetValue(Canvas.ZIndexProperty, (Int32)centroid.Y + 1000);
            polygon.Fill = areaParams.Lighting ? Graphics.GetFrontFaceBrush(areaParams.BackgroundBrush) : areaParams.BackgroundBrush;

            polygon.Stroke = areaParams.BorderBrush;
            polygon.StrokeDashArray = areaParams.BorderStyle;
            polygon.StrokeThickness = areaParams.BorderThickness;
            polygon.StrokeMiterLimit = 1;

            polygon.Points = areaParams.Points;

            polygon.Stretch = Stretch.Fill;
            polygon.Width = size.Width;
            polygon.Height = size.Height;
            polygon.SetValue(Canvas.TopProperty, areaParams.Depth);
            polygon.SetValue(Canvas.LeftProperty, 0.0);

            Canvas polygonSet = new Canvas();
            polygonSet.Width = size.Width + areaParams.Depth;
            polygonSet.Height = size.Height + areaParams.Depth;
            polygonSet.SetValue(Canvas.TopProperty, size.Top - areaParams.Depth);
            polygonSet.SetValue(Canvas.LeftProperty, size.Left);

            polygonSet.Children.Add(polygon);
            
            return polygonSet;
        }

        private static Canvas GetStacked3DSideFaces(ref Faces faces, PolygonalChartShapeParams areaParams)
        {
            Point centroid;
            Brush sideBrush = areaParams.Lighting ? GetRightFaceBrush(areaParams.BackgroundBrush) : areaParams.BackgroundBrush;
            Brush topBrush = areaParams.Lighting ? Graphics.GetTopFaceBrush(areaParams.BackgroundBrush) : areaParams.BackgroundBrush;
            Int32 pointIndexLimit = areaParams.IsPositive ? areaParams.Points.Count - 1 : areaParams.Points.Count;

            Canvas polygonSet = new Canvas();
            Rect size = GetBounds(areaParams.Points);
            polygonSet.Width = size.Width + areaParams.Depth;
            polygonSet.Height = size.Height + areaParams.Depth;
            polygonSet.SetValue(Canvas.TopProperty, size.Top - areaParams.Depth);
            polygonSet.SetValue(Canvas.LeftProperty, size.Left);

            for (Int32 i = 0; i < pointIndexLimit; i++)
            {
                Polygon sides = new Polygon();
                PointCollection points = new PointCollection();
                Int32 index1 = i % areaParams.Points.Count;
                Int32 index2 = (i + 1) % areaParams.Points.Count;

                points.Add(areaParams.Points[index1]);
                points.Add(areaParams.Points[index2]);
                points.Add(new Point(areaParams.Points[index2].X + areaParams.Depth, areaParams.Points[index2].Y - areaParams.Depth));
                points.Add(new Point(areaParams.Points[index1].X + areaParams.Depth, areaParams.Points[index1].Y - areaParams.Depth));
                sides.Points = points;

                centroid = GetCentroid(points);
                Int32 zindex = GetAreaZIndex(centroid.X, centroid.Y, areaParams.IsPositive);
                sides.SetValue(Canvas.ZIndexProperty, zindex);

                if (i == (areaParams.Points.Count - 2))
                {
                    sides.Fill = sideBrush;
                    sides.Tag = "Side";
                }
                else
                {
                    sides.Fill = topBrush;
                    sides.Tag = "Top";
                }

                sides.Stroke = areaParams.BorderBrush;
                sides.StrokeDashArray = areaParams.BorderStyle;
                sides.StrokeThickness = areaParams.BorderThickness;
                sides.StrokeMiterLimit = 1;

                Rect sidesBounds = GetBounds(points);
                sides.Stretch = Stretch.Fill;
                sides.Width = sidesBounds.Width;
                sides.Height = sidesBounds.Height;
                sides.SetValue(Canvas.TopProperty, sidesBounds.Y - (size.Top - areaParams.Depth));
                sides.SetValue(Canvas.LeftProperty, sidesBounds.X - size.X);

                faces.Parts.Add(sides);
                polygonSet.Children.Add(sides);

            }

            return polygonSet;
        }

        private static Int32 GetAreaZIndex(Double left, Double top, Boolean isPositive)
        {
            Int32 Zi = 0;
            if (isPositive)
            {
                Zi = Zi + (Int32)(left);
            }
            else
            {
                Zi = Zi + Int32.MinValue + (Int32)(left);

            }
            return Zi;
        }

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
                zindex = Int32.MinValue + (Int32)(ioffset - topOffset);
            }
            return zindex;
        }

        internal static Brush GetRightFaceBrush(Brush brush)
        {
            if (typeof(SolidColorBrush).Equals(brush.GetType()))
            {
                SolidColorBrush solidBrush = brush as SolidColorBrush;

                List<Color> colors = new List<Color>();
                List<Double> stops = new List<Double>();

                colors.Add(Graphics.GetDarkerColor(solidBrush.Color, 0.35));
                stops.Add(0);

                colors.Add(Graphics.GetDarkerColor(solidBrush.Color, 0.75));
                stops.Add(1);


                return Graphics.CreateLinearGradientBrush(-120, new Point(0, 0.5), new Point(1, 0.5), colors, stops);
            }
            else if (brush is GradientBrush)
            {
                GradientBrush gradBrush = brush as GradientBrush;

                List<Color> colors = new List<Color>();
                List<Double> stops = new List<Double>();

                foreach (GradientStop gradStop in gradBrush.GradientStops)
                {
                    colors.Add(Graphics.GetDarkerColor(gradStop.Color, 0.75));
                    stops.Add(gradStop.Offset);
                }

                if (brush is LinearGradientBrush)
                    return Graphics.CreateLinearGradientBrush(0, new Point(0, 1), new Point(1, 0), colors, stops);
                else
                    return Graphics.CreateRadialGradientBrush(colors, stops);
            }
            else
            {
                return brush;
            }
        }

        private static PointCollection CloneCollection(PointCollection collection)
        {
            PointCollection newCollection = new PointCollection();
            foreach (Point value in collection)
                newCollection.Add(new Point(value.X, value.Y));

            return newCollection;
        }

        private static List<KeySpline> GenerateKeySplineList(params Point[] values)
        {
            List<KeySpline> splines = new List<KeySpline>();
            for (Int32 i = 0; i < values.Length; i += 2)
                splines.Add(Graphics.GetKeySpline(values[i], values[i + 1]));

            return splines;
        }

        private static DataSeries DataSeriesRef
        {
            get;
            set;
        }

        private static Storyboard ApplyAreaAnimation(UIElement areaElement, Storyboard storyboard, bool isPositive, Double beginTime)
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
            List<KeySpline> splines = GenerateKeySplineList
                (
                new Point(0, 0), new Point(1, 1),
                new Point(0, 0), new Point(0.5, 1)
                );

            DoubleAnimationUsingKeyFrames growAnimation = Graphics.CreateDoubleAnimation(DataSeriesRef, scaleTransform, "(ScaleTransform.ScaleY)", beginTime + 0.5, frameTimes, values, splines);
            storyboard.Stop();
            storyboard.Children.Add(growAnimation);

            return storyboard;
        }

        private static Storyboard ApplyMarkerAnimationToAreaChart(Marker marker, Storyboard storyboard, Double beginTime)
        {
            if (marker == null) return storyboard;

            DoubleCollection values = Graphics.GenerateDoubleCollection(0, 1);
            DoubleCollection frameTimes = Graphics.GenerateDoubleCollection(0, 0.75);
            List<KeySpline> splines = GenerateKeySplineList
                (
                new Point(0, 0), new Point(1, 1),
                new Point(0, 0), new Point(0.5, 1)
                );

            marker.Visual.Opacity = 0;

            DoubleAnimationUsingKeyFrames opacityAnimation = Graphics.CreateDoubleAnimation(DataSeriesRef, marker.Visual, "(UIElement.Opacity)", beginTime + 0.5, frameTimes, values, splines);
            storyboard.Stop();
            storyboard.Children.Add(opacityAnimation);

            return storyboard;
        }

        private static Storyboard ApplyStackedAreaAnimation(UIElement areaElement, Storyboard storyboard, Double beginTime,Double duration)
        {
            DoubleCollection values = Graphics.GenerateDoubleCollection(0, 1);
            DoubleCollection frameTimes = Graphics.GenerateDoubleCollection(0, duration);
            List<KeySpline> splines = GenerateKeySplineList
                (
                new Point(0, 0), new Point(1, 1),
                new Point(0, 0), new Point(0.5, 1)
                );

            areaElement.Opacity = 0;
            DoubleAnimationUsingKeyFrames opacityAnimation = Graphics.CreateDoubleAnimation(DataSeriesRef, areaElement, "(UIElement.Opacity)", beginTime + 0.5, frameTimes, values, splines);
            storyboard.Stop();
            storyboard.Children.Add(opacityAnimation);

            return storyboard;
        }
    }
}
