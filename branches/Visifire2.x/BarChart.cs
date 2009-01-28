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

using Visifire.Commons;

namespace Visifire.Charts
{
    internal class BarChart
    {
        internal static Canvas GetMarker(Chart chart, RectangularChartShapeParams columnParams, DataPoint dataPoint, Double left, Double top)
        {   
            Canvas markerCanvas = new Canvas();
            markerCanvas.Width = columnParams.Size.Width;
            markerCanvas.Height = columnParams.Size.Height;
            markerCanvas.SetValue(Canvas.LeftProperty, left);
            markerCanvas.SetValue(Canvas.TopProperty, top);

            if (columnParams.IsMarkerEnabled || columnParams.IsLabelEnabled)
            {
                Size markerSize = new Size(columnParams.MarkerSize, columnParams.MarkerSize);
                String labelText = columnParams.IsLabelEnabled ? columnParams.LabelText : "";
                Boolean markerBevel = false;

                if (!columnParams.IsMarkerEnabled)
                {
                    columnParams.MarkerColor = new SolidColorBrush(Colors.Transparent);
                    columnParams.MarkerBorderColor = new SolidColorBrush(Colors.Transparent);
                }

                dataPoint.Marker = new Marker(columnParams.MarkerType, columnParams.MarkerScale, markerSize, markerBevel, columnParams.MarkerColor, labelText);

                dataPoint.Marker.MarkerSize = markerSize;
                dataPoint.Marker.BorderColor = columnParams.MarkerBorderColor;
                dataPoint.Marker.BorderThickness = columnParams.MarkerBorderThickness.Left;
                dataPoint.Marker.MarkerType = columnParams.MarkerType;
                dataPoint.Marker.FontColor = columnParams.LabelFontColor;
                dataPoint.Marker.FontFamily = columnParams.LabelFontFamily;
                dataPoint.Marker.FontSize = columnParams.LabelFontSize;
                dataPoint.Marker.FontStyle = columnParams.LabelFontStyle;
                dataPoint.Marker.FontWeight = columnParams.LabelFontWeight;
                dataPoint.Marker.TextBackground = columnParams.LabelBackground;

                Point positionXY = new Point();

                if (columnParams.IsPositive)
                    if (chart.View3D)
                        positionXY = new Point(columnParams.Size.Width + columnParams.Depth, columnParams.Size.Height / 2 - columnParams.Depth);
                    else
                        positionXY = new Point(columnParams.Size.Width, columnParams.Size.Height / 2);
                else
                    if (chart.View3D)
                        positionXY = new Point(columnParams.Depth, columnParams.Size.Height / 2 - columnParams.Depth);
                    else
                        positionXY = new Point(0, columnParams.Size.Height / 2);
                    
                if (columnParams.IsLabelEnabled && !String.IsNullOrEmpty(labelText))
                {
                    dataPoint.Marker.CreateVisual();

                    if (columnParams.IsPositive)
                    {
                        if (left + positionXY.X + dataPoint.Marker.MarkerActualSize.Width > chart.PlotArea.PlotAreaBorderElement.Width)
                            columnParams.LabelStyle = LabelStyles.Inside;
                    }
                    else
                    {
                        if (left < dataPoint.Marker.MarkerActualSize.Width)
                            columnParams.LabelStyle = LabelStyles.Inside;
                    }

                    dataPoint.Marker.TextAlignmentY = AlignmentY.Center;

                    if (!columnParams.IsMarkerEnabled)
                    {
                        if (chart.View3D)
                        {
                            if (columnParams.LabelStyle == LabelStyles.OutSide)
                                dataPoint.Marker.MarkerSize = new Size(markerSize.Width + chart.ChartArea.PlankDepth, markerSize.Height + chart.ChartArea.PlankDepth);
                            else
                                dataPoint.Marker.MarkerSize = new Size(markerSize.Width, markerSize.Height);
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
                        dataPoint.Marker.TextAlignmentX = columnParams.LabelStyle == LabelStyles.Inside ? AlignmentX.Left : AlignmentX.Right;
                    else
                        dataPoint.Marker.TextAlignmentX = columnParams.LabelStyle == LabelStyles.Inside ? AlignmentX.Right : AlignmentX.Left;

                    if (columnParams.Size.Height < dataPoint.Marker.TextBlockSize.Height)
                        dataPoint.Marker.TextOrientation = Orientation.Vertical;
                    else
                        dataPoint.Marker.TextOrientation = Orientation.Horizontal;

                }

                dataPoint.Marker.CreateVisual();

                dataPoint.Marker.AddToParent(markerCanvas, positionXY.X, positionXY.Y, new Point(0.5, 0.5));
                
            }

            return markerCanvas;
        }

        internal static Canvas GetVisualObjectForBarChart(Double width, Double height, PlotDetails plotDetails, List<DataSeries> dataSeriesList4Rendering, Chart chart, Double plankDepth, bool animationEnabled)
        {
            if (Double.IsNaN(width) || Double.IsNaN(height) || width <= 0 || height <= 0) return null;

            Double columnGapRatio = 0.2;

            Dictionary<Double, SortedDataPoints> sortedDataPoints = null;
            //if (chart.View3D)
            //    sortedDataPoints = plotDetails.GetDataPointsGroupedByXValue(RenderAs.Bar, dataSeriesList4Rendering[0].PlotGroup.AxesX, dataSeriesList4Rendering[0].PlotGroup.AxisY);
            //else
            sortedDataPoints = plotDetails.GetDataPointsGroupedByXValue(RenderAs.Bar);

            List<Double> xValues = sortedDataPoints.Keys.ToList();

            Canvas visual = new Canvas();
            visual.Width = width;
            visual.Height = height;

            Canvas labelCanvas = new Canvas();
            labelCanvas.Width = width;
            labelCanvas.Height = height;

            Canvas columnCanvas = new Canvas();
            columnCanvas.Width = width;
            columnCanvas.Height = height;

            List<PlotGroup> plotGroupList = (from plots in plotDetails.PlotGroups where plots.RenderAs == RenderAs.Bar select plots).ToList();

            Double depth3d = plankDepth / plotDetails.Layer3DCount * (chart.View3D ? 1 : 0);
            Double visualOffset = depth3d * (plotDetails.SeriesDrawingIndex[plotGroupList[0].DataSeriesList[0]] + 1);
            visual.SetValue(Canvas.TopProperty, visualOffset);
            visual.SetValue(Canvas.LeftProperty, -visualOffset);
            labelCanvas.SetValue(Canvas.TopProperty, visualOffset);
            labelCanvas.SetValue(Canvas.LeftProperty, -visualOffset);

            Double minDiffValue = plotDetails.GetMinOfMinDifferencesForXValue(RenderAs.Bar, RenderAs.StackedBar, RenderAs.StackedBar100);

            Axis AxesXwithMinInterval = dataSeriesList4Rendering[0].PlotGroup.AxisX;

            minDiffValue = (minDiffValue < (Double)AxesXwithMinInterval.InternalInterval) ? minDiffValue : (Double)AxesXwithMinInterval.InternalInterval;

            Double dataAxisDifference = Math.Abs((Double)AxesXwithMinInterval.InternalAxisMinimum - (Double)AxesXwithMinInterval.Minimum) * 2;

            Double dataMinimumGap = Graphics.ValueToPixelPosition(0, height, (Double)AxesXwithMinInterval.InternalAxisMinimum, (Double)AxesXwithMinInterval.InternalAxisMaximum, dataAxisDifference + (Double)AxesXwithMinInterval.InternalAxisMinimum);
            Double minDiffGap = Graphics.ValueToPixelPosition(0, height, (Double)AxesXwithMinInterval.InternalAxisMinimum, (Double)AxesXwithMinInterval.InternalAxisMaximum, minDiffValue + (Double)AxesXwithMinInterval.InternalAxisMinimum);

            if (dataMinimumGap > 0 && minDiffGap > 0)
                minDiffGap = Math.Min(minDiffGap, dataMinimumGap);
            else
                minDiffGap = Math.Max(minDiffGap, dataMinimumGap);

            Double maxColumnHeight = minDiffGap * (1 - columnGapRatio);

            Double numberOfDivisions = plotDetails.GetMaxDivision(sortedDataPoints);

            Double heightPerBar = maxColumnHeight / numberOfDivisions;

            Boolean plankDrawn = false;

            foreach (Double xValue in xValues)
            {
                RectangularChartShapeParams barParams = new RectangularChartShapeParams();
                barParams.ShadowOffset = 5;
                barParams.Depth = depth3d;

                foreach (DataPoint dataPoint in sortedDataPoints[xValue].Positive)
                {
                    barParams.Bevel = dataPoint.Parent.Bevel;
                    barParams.Lighting = (Boolean)dataPoint.LightingEnabled;
                    barParams.Shadow = (Boolean)dataPoint.ShadowEnabled;
                    barParams.BorderBrush = dataPoint.BorderColor;
                    barParams.BorderThickness = ((Thickness)dataPoint.BorderThickness).Left;
                    barParams.BorderStyle = Graphics.BorderStyleToStrokeDashArray((BorderStyles)dataPoint.BorderStyle);
                    barParams.XRadius = new CornerRadius(0,dataPoint.RadiusX.Value.TopRight, dataPoint.RadiusX.Value.BottomRight, 0);
                    barParams.YRadius = new CornerRadius(0,dataPoint.RadiusY.Value.TopRight, dataPoint.RadiusY.Value.BottomRight, 0);
                    barParams.IsPositive = true;
                    barParams.BackgroundBrush = dataPoint.Color;

                    barParams.IsMarkerEnabled = (Boolean)dataPoint.MarkerEnabled;
                    barParams.MarkerType = (MarkerTypes)dataPoint.MarkerType;
                    barParams.MarkerColor = dataPoint.MarkerColor;
                    barParams.MarkerBorderColor = dataPoint.MarkerBorderColor;
                    barParams.MarkerBorderThickness = (Thickness)dataPoint.MarkerBorderThickness;
                    barParams.MarkerScale = (Double)dataPoint.MarkerScale;
                    barParams.MarkerSize = (Double)dataPoint.MarkerSize;

                    barParams.IsLabelEnabled = (Boolean)dataPoint.LabelEnabled;
                    barParams.LabelStyle = (LabelStyles)dataPoint.LabelStyle;
                    barParams.LabelText = dataPoint.TextParser(dataPoint.LabelText);
                    barParams.LabelBackground = dataPoint.LabelBackground;
                    barParams.LabelFontColor = Graphics.ApplyLabelFontColor(chart, dataPoint, dataPoint.LabelFontColor, (LabelStyles)barParams.LabelStyle);
                    barParams.LabelFontSize = (Double)dataPoint.LabelFontSize;
                    barParams.LabelFontFamily = dataPoint.LabelFontFamily;
                    barParams.LabelFontStyle = (FontStyle)dataPoint.LabelFontStyle;
                    barParams.LabelFontWeight = (FontWeight)dataPoint.LabelFontWeight;

                    PlotGroup plotGroup = dataPoint.Parent.PlotGroup;

                    Double limitingYValue = 0;
                    if (plotGroup.AxisY.InternalAxisMinimum > 0)
                        limitingYValue = (Double)plotGroup.AxisY.InternalAxisMinimum;
                    if (plotGroup.AxisY.InternalAxisMaximum < 0)
                        limitingYValue = (Double)plotGroup.AxisY.InternalAxisMaximum;

                    //List<DataSeries> indexSeriesList = plotDetails.GetSeriesFromSortedPoints(sortedDataPoints[xValue]);

                    List<DataSeries> indexSeriesList = plotDetails.GetSeriesFromDataPoint(dataPoint);
                    Int32 drawingIndex = indexSeriesList.IndexOf(dataPoint.Parent);
                    
                    Double top = Graphics.ValueToPixelPosition(height, 0, (Double)plotGroup.AxisX.InternalAxisMinimum, (Double)plotGroup.AxisX.InternalAxisMaximum, xValue);
                    //left = drawingIndex * widthPerColumn - (maxColumnWidth / 2);
                    top = top + ((Double)drawingIndex - (Double)indexSeriesList.Count() / (Double)2) * heightPerBar;
                    Double left = Graphics.ValueToPixelPosition(0, width, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, limitingYValue);
                    Double right = Graphics.ValueToPixelPosition(0, width, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, dataPoint.InternalYValue);
                    Double columnWidth = Math.Abs(left - right);

                    Double finalHeight = heightPerBar;
                    Double minPosValue = 0;
                    Double maxPosValue = height;

                    if (top < minPosValue)
                    {
                        finalHeight = top + heightPerBar - minPosValue;
                        top = minPosValue;
                    }
                    else if (top + heightPerBar > maxPosValue)
                    {
                        finalHeight = maxPosValue - top;
                    }
                    if (finalHeight < 0)
                        continue;
                    
                    barParams.Size = new Size(columnWidth, finalHeight);

                    Faces column;
                    Panel columnVisual = null;

                    if (chart.View3D)
                    {
                        column = Get3DBar(barParams);
                        columnVisual = column.Visual;
                        columnVisual.SetValue(Canvas.ZIndexProperty, GetBarZIndex(left, top, height, dataPoint.InternalYValue > 0));
                    }
                    else
                    {
                        column = Get2DBar(barParams);
                        columnVisual = column.Visual;
                    }

                    dataPoint.Faces = column;
                    dataPoint.Faces.LabelCanvas = labelCanvas;

                    columnVisual.SetValue(Canvas.LeftProperty, left);
                    columnVisual.SetValue(Canvas.TopProperty, top);

                    columnCanvas.Children.Add(columnVisual);

                    labelCanvas.Children.Add(GetMarker(chart, barParams, dataPoint, left, top));
                    dataPoint.Faces.LabelCanvas = labelCanvas;

                    // Apply animation
                    if (animationEnabled)
                    {
                        if (dataPoint.Parent.Storyboard == null)
                            dataPoint.Parent.Storyboard = new Storyboard();

                        DataSeriesRef = dataPoint.Parent;

                        // Apply animation to the bars 
                        dataPoint.Parent.Storyboard = ApplyBarChartAnimation(columnVisual, dataPoint.Parent.Storyboard, barParams);

                        // Apply animation to the marker and labels
                        dataPoint.Parent.Storyboard = ApplyMarkerAnimationToBarChart(dataPoint.Marker, dataPoint.Parent.Storyboard, 1);
                    }
                }

                foreach (DataPoint dataPoint in sortedDataPoints[xValue].Negative)
                {
                    barParams.Bevel = dataPoint.Parent.Bevel;
                    barParams.Lighting = (Boolean)dataPoint.LightingEnabled;
                    barParams.Shadow = (Boolean)dataPoint.ShadowEnabled;
                    barParams.BorderBrush = dataPoint.BorderColor;
                    barParams.BorderThickness = ((Thickness)dataPoint.BorderThickness).Left;
                    barParams.BorderStyle = Graphics.BorderStyleToStrokeDashArray((BorderStyles)dataPoint.BorderStyle);
                    barParams.XRadius = new CornerRadius(dataPoint.RadiusX.Value.TopLeft,0,0,dataPoint.RadiusX.Value.BottomLeft);
                    barParams.YRadius = new CornerRadius(dataPoint.RadiusY.Value.TopLeft,0,0,dataPoint.RadiusY.Value.BottomLeft);
                    barParams.IsPositive = false;
                    barParams.BackgroundBrush = dataPoint.Color;

                    barParams.IsMarkerEnabled = (Boolean)dataPoint.MarkerEnabled;
                    barParams.MarkerType = (MarkerTypes)dataPoint.MarkerType;
                    barParams.MarkerColor = dataPoint.MarkerColor;
                    barParams.MarkerBorderColor = dataPoint.MarkerBorderColor;
                    barParams.MarkerBorderThickness = (Thickness)dataPoint.MarkerBorderThickness;
                    barParams.MarkerScale = (Double)dataPoint.MarkerScale;
                    barParams.MarkerSize = (Double)dataPoint.MarkerSize;

                    barParams.IsLabelEnabled = (Boolean)dataPoint.LabelEnabled;
                    barParams.LabelStyle = (LabelStyles)dataPoint.LabelStyle;
                    barParams.LabelText = dataPoint.TextParser(dataPoint.LabelText);
                    barParams.LabelBackground = dataPoint.LabelBackground;
                    barParams.LabelFontColor = Graphics.ApplyLabelFontColor(chart, dataPoint, dataPoint.LabelFontColor, (LabelStyles)barParams.LabelStyle);
                    barParams.LabelFontSize = (Double)dataPoint.LabelFontSize;
                    barParams.LabelFontFamily = dataPoint.LabelFontFamily;
                    barParams.LabelFontStyle = (FontStyle)dataPoint.LabelFontStyle;
                    barParams.LabelFontWeight = (FontWeight)dataPoint.LabelFontWeight;

                    PlotGroup plotGroup = dataPoint.Parent.PlotGroup;

                    Double limitingYValue = 0;
                    if (plotGroup.AxisY.InternalAxisMinimum > 0)
                        limitingYValue = (Double)plotGroup.AxisY.InternalAxisMinimum;
                    if (plotGroup.AxisY.InternalAxisMaximum < 0)
                        limitingYValue = (Double)plotGroup.AxisY.InternalAxisMaximum;

                    //List<DataSeries> indexSeriesList = plotDetails.GetSeriesFromSortedPoints(sortedDataPoints[xValue]);

                    List<DataSeries> indexSeriesList = plotDetails.GetSeriesFromDataPoint(dataPoint);
                    Int32 drawingIndex = indexSeriesList.IndexOf(dataPoint.Parent);
                                        
                    Double top = Graphics.ValueToPixelPosition(height, 0, (Double)plotGroup.AxisX.InternalAxisMinimum, (Double)plotGroup.AxisX.InternalAxisMaximum, xValue);
                    //left = drawingIndex * widthPerColumn - (maxColumnWidth / 2);
                    top = top + ((Double)drawingIndex - (Double)indexSeriesList.Count() / (Double)2) * heightPerBar;
                    Double right = Graphics.ValueToPixelPosition(0, width, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, limitingYValue);
                    Double left = Graphics.ValueToPixelPosition(0, width, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, dataPoint.InternalYValue);
                    Double columnWidth = Math.Abs(right - left);

                    Double finalHeight = heightPerBar;
                    Double minPosValue = 0;
                    Double maxPosValue = height;

                    if (top < minPosValue)
                    {
                        finalHeight = top + heightPerBar - minPosValue;
                        top = minPosValue;
                    }
                    else if (top + heightPerBar > maxPosValue)
                    {
                        finalHeight = maxPosValue - top;
                    }
                    if (finalHeight < 0)
                        continue;

                    barParams.Size = new Size(columnWidth, finalHeight);

                    Faces column;
                    Panel columnVisual = null;

                    if (chart.View3D)
                    {
                        column = Get3DBar(barParams);
                        columnVisual = column.Visual;
                        columnVisual.SetValue(Canvas.ZIndexProperty, GetBarZIndex(left, top, height, dataPoint.InternalYValue > 0));
                    }
                    else
                    {
                        column = Get2DBar(barParams);
                        columnVisual = column.Visual;
                    }

                    dataPoint.Faces = column;
                    dataPoint.Faces.LabelCanvas = labelCanvas;

                    columnVisual.SetValue(Canvas.LeftProperty, left);
                    columnVisual.SetValue(Canvas.TopProperty, top);

                    columnCanvas.Children.Add(columnVisual);

                    labelCanvas.Children.Add(GetMarker(chart, barParams, dataPoint, left, top));
                    dataPoint.Faces.LabelCanvas = labelCanvas;

                    // Apply animation
                    if (animationEnabled)
                    {
                        if (dataPoint.Parent.Storyboard == null)
                            dataPoint.Parent.Storyboard = new Storyboard();

                        DataSeriesRef = dataPoint.Parent;

                        // Apply animation to the bars 
                        dataPoint.Parent.Storyboard = ApplyBarChartAnimation(columnVisual, dataPoint.Parent.Storyboard, barParams);

                        // Apply animation to the marker and labels
                        dataPoint.Parent.Storyboard = ApplyMarkerAnimationToBarChart(dataPoint.Marker, dataPoint.Parent.Storyboard, 1);
                    }
                }
            }
            if (!plankDrawn && chart.View3D && dataSeriesList4Rendering[0].PlotGroup.AxisY.InternalAxisMinimum < 0 && dataSeriesList4Rendering[0].PlotGroup.AxisY.InternalAxisMaximum > 0)
            {
                RectangularChartShapeParams columnParams = new RectangularChartShapeParams();
                columnParams.BackgroundBrush = new SolidColorBrush(Color.FromArgb((Byte)255, (Byte)127, (Byte)127, (Byte)127));
                columnParams.Lighting = true;
                columnParams.Size = new Size(1, height);
                columnParams.Depth = depth3d;

                Faces zeroPlank = ColumnChart.Get3DColumn(columnParams);
                Panel zeroPlankVisual = zeroPlank.Visual;

                Double left = Graphics.ValueToPixelPosition(0, width, (Double)dataSeriesList4Rendering[0].PlotGroup.AxisY.InternalAxisMinimum, (Double)dataSeriesList4Rendering[0].PlotGroup.AxisY.InternalAxisMaximum, 0);
                zeroPlankVisual.SetValue(Canvas.LeftProperty, left);
                zeroPlankVisual.SetValue(Canvas.TopProperty, (Double)0);
                zeroPlankVisual.SetValue(Canvas.ZIndexProperty, 0);
                zeroPlankVisual.Opacity = 0.7;
                columnCanvas.Children.Add(zeroPlankVisual);
            }

            visual.Children.Add(columnCanvas);
            visual.Children.Add(labelCanvas);

            return visual;
        }

        internal static Canvas GetVisualObjectForStackedBarChart(Double width, Double height, PlotDetails plotDetails, Chart chart, Double plankDepth,bool animationEnabled)
        {
            if (Double.IsNaN(width) || Double.IsNaN(height) || width <= 0 || height <= 0) return null;

            List<PlotGroup> plotGroupList = (from plots in plotDetails.PlotGroups where plots.RenderAs == RenderAs.StackedBar select plots).ToList();

            Double numberOfDivisions = plotDetails.DrawingDivisionFactor;
            Double barGapRatio = 0.2;

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

            Random rand = new Random();

            List<DataSeries> seriesList = plotDetails.GetSeriesListByRenderAs(RenderAs.StackedBar);

            Dictionary<Axis, Dictionary<Axis, Int32>> seriesIndex = GetSeriesIndex(seriesList);

            Int32 index = 1;

            foreach (PlotGroup plotGroup in plotGroupList)
            {

                if (!seriesIndex.ContainsKey(plotGroup.AxisY))
                    continue;

                Int32 drawingIndex = seriesIndex[plotGroup.AxisY][plotGroup.AxisX];

                Double minDiff = plotDetails.GetMinOfMinDifferencesForXValue(RenderAs.Bar, RenderAs.StackedBar, RenderAs.StackedBar100);

                minDiff = (minDiff < (Double)plotGroup.AxisX.InternalInterval) ? minDiff : (Double)plotGroup.AxisX.InternalInterval;

                Double maxBarHeight = Graphics.ValueToPixelPosition(0, height, (Double)plotGroup.AxisX.InternalAxisMinimum, (Double)plotGroup.AxisX.InternalAxisMaximum, minDiff + (Double)plotGroup.AxisX.InternalAxisMinimum) * (1 - barGapRatio);
                Double heightPerBar = maxBarHeight / numberOfDivisions;

                List<Double> xValuesList = plotGroup.XWiseStackedDataList.Keys.ToList();

                Double limitingYValue = 0;
                if (plotGroup.AxisY.InternalAxisMinimum > 0)
                    limitingYValue = (Double)plotGroup.AxisY.InternalAxisMinimum;
                if (plotGroup.AxisY.InternalAxisMaximum < 0)
                    limitingYValue = (Double)plotGroup.AxisY.InternalAxisMaximum;

                index++;

                foreach (Double xValue in xValuesList)
                {
                    RectangularChartShapeParams barParams = new RectangularChartShapeParams();
                    barParams.ShadowOffset = 5;
                    barParams.Depth = depth3d;
                    barParams.IsStacked = true;

                    Double top = Graphics.ValueToPixelPosition(height, 0, (Double)plotGroup.AxisX.InternalAxisMinimum, (Double)plotGroup.AxisX.InternalAxisMaximum, xValue) + drawingIndex * heightPerBar - (maxBarHeight / 2);
                    Double left = Graphics.ValueToPixelPosition(0, width, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, limitingYValue);

                    //-----------------------------------------------------

                    Double finalHeight = heightPerBar;
                    Double minPosValue = 0;
                    Double maxPosValue = height;

                    if (top < minPosValue)
                    {
                        finalHeight = top + heightPerBar - minPosValue;
                        top = minPosValue;
                    }
                    else if (top + heightPerBar > maxPosValue)
                    {
                        finalHeight = maxPosValue - top;
                    }
                    if (finalHeight < 0)
                        continue;

                    //-----------------------------------------------------

                    Double right;
                    Double barWidth;
                    Double prevSum = 0;
                    // Plot positive values
                    foreach (DataPoint dataPoint in plotGroup.XWiseStackedDataList[xValue].Positive)
                    {
                        if (!(Boolean)dataPoint.Enabled || Double.IsNaN(dataPoint.InternalYValue))
                            continue;

                        barParams.Bevel = dataPoint.Parent.Bevel;
                        barParams.Lighting = (Boolean)dataPoint.LightingEnabled;
                        barParams.Shadow = (Boolean)dataPoint.ShadowEnabled;
                        barParams.BorderBrush = dataPoint.BorderColor;
                        barParams.BorderThickness = ((Thickness) dataPoint.BorderThickness).Left;
                        barParams.BorderStyle = Graphics.BorderStyleToStrokeDashArray((BorderStyles)dataPoint.BorderStyle);
                        barParams.IsTopOfStack = (dataPoint == plotGroup.XWiseStackedDataList[xValue].Positive.Last());
                        if (barParams.IsTopOfStack)
                        {
                            barParams.XRadius = new CornerRadius(0,dataPoint.RadiusX.Value.TopRight, dataPoint.RadiusX.Value.BottomRight, 0);
                            barParams.YRadius = new CornerRadius(0,dataPoint.RadiusY.Value.TopRight, dataPoint.RadiusY.Value.BottomRight, 0);
                        }
                        barParams.IsPositive = true;

                        barParams.IsMarkerEnabled = (Boolean)dataPoint.MarkerEnabled;
                        barParams.MarkerType = (MarkerTypes)dataPoint.MarkerType;
                        barParams.MarkerColor = dataPoint.MarkerColor;
                        barParams.MarkerBorderColor = dataPoint.MarkerBorderColor;
                        barParams.MarkerBorderThickness = (Thickness)dataPoint.MarkerBorderThickness;
                        barParams.MarkerScale = (Double)dataPoint.MarkerScale;
                        barParams.MarkerSize = (Double)dataPoint.MarkerSize;

                        barParams.IsLabelEnabled = (Boolean)dataPoint.LabelEnabled;
                        barParams.LabelStyle = (LabelStyles)dataPoint.LabelStyle;
                        barParams.LabelText = dataPoint.TextParser(dataPoint.LabelText);
                        barParams.LabelBackground = dataPoint.LabelBackground;
                        barParams.LabelFontColor = Graphics.ApplyLabelFontColor(chart, dataPoint, dataPoint.LabelFontColor, (LabelStyles)barParams.LabelStyle);
                        barParams.LabelFontSize = (Double)dataPoint.LabelFontSize;
                        barParams.LabelFontFamily = dataPoint.LabelFontFamily;
                        barParams.LabelFontStyle = (FontStyle)dataPoint.LabelFontStyle;
                        barParams.LabelFontWeight = (FontWeight)dataPoint.LabelFontWeight;

                        right = Graphics.ValueToPixelPosition(0, width, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, dataPoint.InternalYValue + prevSum);
                        barWidth = Math.Abs(right - left);

                        prevSum += dataPoint.InternalYValue;

                        barParams.BackgroundBrush = dataPoint.Color;

                        barParams.Size = new Size(barWidth, finalHeight);
                        //barParams.Size = new Size(barWidth, heightPerBar);

                        Faces bar;
                        Panel barVisual = null;

                        if (chart.View3D)
                        {
                            bar = Get3DBar(barParams);
                            barVisual = bar.Visual;
                            barVisual.SetValue(Canvas.ZIndexProperty, GetStackedBarZIndex(left, top, width, height, (dataPoint.InternalYValue > 0)));
                        }
                        else
                        {
                            bar = Get2DBar(barParams);
                            barVisual = bar.Visual;
                        }

                        dataPoint.Faces = bar;
                        dataPoint.Faces.LabelCanvas = labelCanvas;

                        barVisual.SetValue(Canvas.LeftProperty, left);
                        barVisual.SetValue(Canvas.TopProperty, top);

                        columnCanvas.Children.Add(barVisual);
                        labelCanvas.Children.Add(GetMarker(chart, barParams, dataPoint, left, top));

                        // Apply animation
                        if (animationEnabled)
                        {
                            if (dataPoint.Parent.Storyboard == null)
                                dataPoint.Parent.Storyboard = new Storyboard();

                            DataSeriesRef = dataPoint.Parent;

                            // Apply animation to the data points i.e to the rectangles that form the columns
                            dataPoint.Parent.Storyboard = ApplyStackedBarChartAnimation(barVisual, dataPoint.Parent.Storyboard, barParams, (1.0 / seriesList.Count) * (Double)(seriesList.IndexOf(dataPoint.Parent)), 1.0 / seriesList.Count);

                            // Apply animation to the marker and labels
                            dataPoint.Parent.Storyboard = ApplyMarkerAnimationToBarChart(dataPoint.Marker, dataPoint.Parent.Storyboard, 1);
                        }

                        left = right;
                        
                    }

                    prevSum = 0;
                    right = Graphics.ValueToPixelPosition(0, width, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, limitingYValue);
                    // Plot negative values
                    foreach (DataPoint dataPoint in plotGroup.XWiseStackedDataList[xValue].Negative)
                    {
                        if (!(Boolean)dataPoint.Enabled || Double.IsNaN(dataPoint.InternalYValue))
                            continue;
                        
                        barParams.Bevel = dataPoint.Parent.Bevel;
                        barParams.Lighting = (Boolean)dataPoint.LightingEnabled;
                        barParams.Shadow = (Boolean)dataPoint.ShadowEnabled;
                        barParams.BorderBrush = dataPoint.BorderColor;
                        barParams.BorderThickness = ((Thickness) dataPoint.BorderThickness).Left;
                        barParams.BorderStyle = Graphics.BorderStyleToStrokeDashArray((BorderStyles)dataPoint.BorderStyle);
                        barParams.IsTopOfStack = (dataPoint == plotGroup.XWiseStackedDataList[xValue].Negative.Last());
                        if (barParams.IsTopOfStack)
                        {
                            barParams.XRadius = new CornerRadius(dataPoint.RadiusX.Value.TopLeft,0,0,dataPoint.RadiusX.Value.BottomLeft);
                            barParams.YRadius = new CornerRadius(dataPoint.RadiusY.Value.TopLeft,0,0,dataPoint.RadiusY.Value.BottomLeft);
                        }
                        barParams.IsPositive = false;

                        barParams.IsMarkerEnabled = (Boolean)dataPoint.MarkerEnabled;
                        barParams.MarkerType = (MarkerTypes)dataPoint.MarkerType;
                        barParams.MarkerColor = dataPoint.MarkerColor;
                        barParams.MarkerBorderColor = dataPoint.MarkerBorderColor;
                        barParams.MarkerBorderThickness = (Thickness)dataPoint.MarkerBorderThickness;
                        barParams.MarkerScale = (Double)dataPoint.MarkerScale;
                        barParams.MarkerSize = (Double)dataPoint.MarkerSize;

                        barParams.IsLabelEnabled = (Boolean)dataPoint.LabelEnabled;
                        barParams.LabelStyle = (LabelStyles)dataPoint.LabelStyle;
                        barParams.LabelText = dataPoint.TextParser(dataPoint.LabelText);
                        barParams.LabelBackground = dataPoint.LabelBackground;
                        barParams.LabelFontColor = Graphics.ApplyLabelFontColor(chart, dataPoint, dataPoint.LabelFontColor, (LabelStyles)barParams.LabelStyle);
                        barParams.LabelFontSize = (Double)dataPoint.LabelFontSize;
                        barParams.LabelFontFamily = dataPoint.LabelFontFamily;
                        barParams.LabelFontStyle = (FontStyle)dataPoint.LabelFontStyle;
                        barParams.LabelFontWeight = (FontWeight)dataPoint.LabelFontWeight;

                        left = Graphics.ValueToPixelPosition(0, width, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, dataPoint.InternalYValue + prevSum);
                        barWidth = Math.Abs(right - left);

                        prevSum += dataPoint.InternalYValue;

                        barParams.BackgroundBrush = dataPoint.Color;

                        barParams.Size = new Size(barWidth, finalHeight);
                        //barParams.Size = new Size(barWidth, heightPerBar);

                        Faces bar;
                        Panel barVisual = null;

                        if (chart.View3D)
                        {
                            bar = Get3DBar(barParams);
                            barVisual = bar.Visual;
                            barVisual.SetValue(Canvas.ZIndexProperty, GetStackedBarZIndex(left, top, width, height, (dataPoint.InternalYValue > 0)));
                        }
                        else
                        {
                            bar = Get2DBar(barParams);
                            barVisual = bar.Visual;
                        }

                        dataPoint.Faces = bar;
                        dataPoint.Faces.LabelCanvas = labelCanvas;

                        barVisual.SetValue(Canvas.LeftProperty, left);
                        barVisual.SetValue(Canvas.TopProperty, top);

                        columnCanvas.Children.Add(barVisual);
                        labelCanvas.Children.Add(GetMarker(chart, barParams, dataPoint, left, top));

                        // Apply animation
                        if (animationEnabled)
                        {
                            if (dataPoint.Parent.Storyboard == null)
                                dataPoint.Parent.Storyboard = new Storyboard();

                            DataSeriesRef = dataPoint.Parent;

                            // Apply animation to the data points i.e to the rectangles that form the columns
                            dataPoint.Parent.Storyboard = ApplyStackedBarChartAnimation(barVisual, dataPoint.Parent.Storyboard, barParams, (1.0 / seriesList.Count) * (Double)(seriesList.IndexOf(dataPoint.Parent)), 1.0 / seriesList.Count);

                            // Apply animation to the marker and labels
                            dataPoint.Parent.Storyboard = ApplyMarkerAnimationToBarChart(dataPoint.Marker, dataPoint.Parent.Storyboard, 1);
                        }

                        right = left;
                    }

                }

            }
            if (!plankDrawn && chart.View3D && plotGroupList[0].AxisY.InternalAxisMinimum < 0 && plotGroupList[0].AxisY.InternalAxisMaximum > 0)
            {
                RectangularChartShapeParams columnParams = new RectangularChartShapeParams();
                columnParams.BackgroundBrush = new SolidColorBrush(Color.FromArgb((Byte)255, (Byte)127, (Byte)127, (Byte)127));
                columnParams.Lighting = true;
                columnParams.Size = new Size(1, height);
                columnParams.Depth = depth3d;

                Faces zeroPlank = ColumnChart.Get3DColumn(columnParams);
                Panel zeroPlankVisual = zeroPlank.Visual;

                Double left = Graphics.ValueToPixelPosition(0, width, (Double)plotGroupList[0].AxisY.InternalAxisMinimum, (Double)plotGroupList[0].AxisY.InternalAxisMaximum, 0);
                zeroPlankVisual.SetValue(Canvas.LeftProperty, left);
                zeroPlankVisual.SetValue(Canvas.TopProperty, (Double)0);
                zeroPlankVisual.SetValue(Canvas.ZIndexProperty, 0);
                zeroPlankVisual.Opacity = 0.7;
                columnCanvas.Children.Add(zeroPlankVisual);
            }
            visual.Children.Add(columnCanvas);
            visual.Children.Add(labelCanvas);
            return visual;
        }

        internal static Canvas GetVisualObjectForStackedBar100Chart(Double width, Double height, PlotDetails plotDetails, Chart chart, Double plankDepth,bool animationEnabled)
        {
            if (Double.IsNaN(width) || Double.IsNaN(height) || width <= 0 || height <= 0) return null;

            List<PlotGroup> plotGroupList = (from plots in plotDetails.PlotGroups where plots.RenderAs == RenderAs.StackedBar100 select plots).ToList();

            Double numberOfDivisions = plotDetails.DrawingDivisionFactor;
            Double barGapRatio = 0.2;

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

            Random rand = new Random();

            List<DataSeries> seriesList = plotDetails.GetSeriesListByRenderAs(RenderAs.StackedBar100);

            Dictionary<Axis, Dictionary<Axis, Int32>> seriesIndex = GetSeriesIndex(seriesList);

            foreach (PlotGroup plotGroup in plotGroupList)
            {
                if (!seriesIndex.ContainsKey(plotGroup.AxisY))
                    continue;
                
                Int32 drawingIndex = seriesIndex[plotGroup.AxisY][plotGroup.AxisX];

                Double minDiff = plotDetails.GetMinOfMinDifferencesForXValue(RenderAs.Bar, RenderAs.StackedBar, RenderAs.StackedBar100);

                minDiff = (minDiff < (Double)plotGroup.AxisX.InternalInterval) ? minDiff : (Double)plotGroup.AxisX.InternalInterval;

                Double maxBarHeight = Graphics.ValueToPixelPosition(0, height, (Double)plotGroup.AxisX.InternalAxisMinimum, (Double)plotGroup.AxisX.InternalAxisMaximum, minDiff + (Double)plotGroup.AxisX.InternalAxisMinimum) * (1 - barGapRatio);
                Double heightPerBar = maxBarHeight / numberOfDivisions;

                List<Double> xValuesList = plotGroup.XWiseStackedDataList.Keys.ToList();

                Double limitingYValue = 0;
                if (plotGroup.AxisY.InternalAxisMinimum > 0)
                    limitingYValue = (Double)plotGroup.AxisY.InternalAxisMinimum;
                if (plotGroup.AxisY.InternalAxisMaximum < 0)
                    limitingYValue = (Double)plotGroup.AxisY.InternalAxisMaximum;

                foreach (Double xValue in xValuesList)
                {
                    RectangularChartShapeParams barParams = new RectangularChartShapeParams();
                    barParams.ShadowOffset = 5;
                    barParams.Depth = depth3d;
                    barParams.IsStacked = true;

                    Double absoluteSum = plotGroup.XWiseStackedDataList[xValue].AbsoluteYValueSum;

                    if (Double.IsNaN(absoluteSum) || absoluteSum <= 0)
                        absoluteSum = 1;

                    Double top = Graphics.ValueToPixelPosition(height, 0, (Double)plotGroup.AxisX.InternalAxisMinimum, (Double)plotGroup.AxisX.InternalAxisMaximum, xValue) + drawingIndex * heightPerBar - (maxBarHeight / 2);
                    Double left = Graphics.ValueToPixelPosition(0, width, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, limitingYValue);

                    //-----------------------------------------------------

                    Double finalHeight = heightPerBar;
                    Double minPosValue = 0;
                    Double maxPosValue = height;

                    if (top < minPosValue)
                    {
                        finalHeight = top + heightPerBar - minPosValue;
                        top = minPosValue;
                    }
                    else if (top + heightPerBar > maxPosValue)
                    {
                        finalHeight = maxPosValue - top;
                    }
                    if (finalHeight < 0)
                        continue;

                    //-----------------------------------------------------

                    Double right;
                    Double barWidth;
                    Double prevSum = 0;
                    Double percentYValue;
                    // Plot positive values
                    foreach (DataPoint dataPoint in plotGroup.XWiseStackedDataList[xValue].Positive)
                    {
                        if (!(Boolean)dataPoint.Enabled || Double.IsNaN(dataPoint.InternalYValue))
                            continue;

                        barParams.Bevel = dataPoint.Parent.Bevel;
                        barParams.Lighting = (Boolean)dataPoint.LightingEnabled;
                        barParams.Shadow = (Boolean)dataPoint.ShadowEnabled;
                        barParams.BorderBrush = dataPoint.BorderColor;
                        barParams.BorderThickness = ((Thickness) dataPoint.BorderThickness).Left;
                        barParams.BorderStyle = Graphics.BorderStyleToStrokeDashArray((BorderStyles)dataPoint.BorderStyle);
                        barParams.IsTopOfStack = (dataPoint == plotGroup.XWiseStackedDataList[xValue].Positive.Last());
                        if (barParams.IsTopOfStack)
                        {
                            barParams.XRadius = new CornerRadius(0,dataPoint.RadiusX.Value.TopRight, dataPoint.RadiusX.Value.BottomRight,0);
                            barParams.YRadius = new CornerRadius(0,dataPoint.RadiusY.Value.TopRight, dataPoint.RadiusY.Value.BottomRight,0);
                        }
                        barParams.IsPositive = true;

                        barParams.IsMarkerEnabled = (Boolean)dataPoint.MarkerEnabled;
                        barParams.MarkerType = (MarkerTypes)dataPoint.MarkerType;
                        barParams.MarkerColor = dataPoint.MarkerColor;
                        barParams.MarkerBorderColor = dataPoint.MarkerBorderColor;
                        barParams.MarkerBorderThickness = (Thickness) dataPoint.MarkerBorderThickness;
                        barParams.MarkerScale = (Double)dataPoint.MarkerScale;
                        barParams.MarkerSize = (Double)dataPoint.MarkerSize;

                        barParams.IsLabelEnabled = (Boolean)dataPoint.LabelEnabled;
                        barParams.LabelStyle = (LabelStyles)dataPoint.LabelStyle;
                        barParams.LabelText = dataPoint.TextParser(dataPoint.LabelText);
                        barParams.LabelBackground = dataPoint.LabelBackground;
                        barParams.LabelFontColor = Graphics.ApplyLabelFontColor(chart, dataPoint, dataPoint.LabelFontColor, (LabelStyles)barParams.LabelStyle);
                        barParams.LabelFontSize = (Double)dataPoint.LabelFontSize;
                        barParams.LabelFontFamily = dataPoint.LabelFontFamily;
                        barParams.LabelFontStyle = (FontStyle)dataPoint.LabelFontStyle;
                        barParams.LabelFontWeight = (FontWeight)dataPoint.LabelFontWeight;

                        percentYValue = (dataPoint.InternalYValue/absoluteSum * 100);
                        right = Graphics.ValueToPixelPosition(0, width, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, percentYValue + prevSum);
                        barWidth = Math.Abs(right - left);


                        prevSum += percentYValue;

                        barParams.BackgroundBrush = dataPoint.Color;
                        
                        barParams.Size = new Size(barWidth, finalHeight);
                        //barParams.Size = new Size(barWidth, heightPerBar);

                        Faces bar;
                        Panel barVisual = null;

                        if (chart.View3D)
                        {
                            bar = Get3DBar(barParams);
                            barVisual = bar.Visual;
                            barVisual.SetValue(Canvas.ZIndexProperty, GetStackedBarZIndex(left, top, width, height, (dataPoint.InternalYValue > 0)));
                        }
                        else
                        {
                            bar = Get2DBar(barParams);
                            barVisual = bar.Visual;
                        }

                        dataPoint.Faces = bar;
                        dataPoint.Faces.LabelCanvas = labelCanvas;

                        barVisual.SetValue(Canvas.LeftProperty, left);
                        barVisual.SetValue(Canvas.TopProperty, top);

                        columnCanvas.Children.Add(barVisual);
                        labelCanvas.Children.Add(GetMarker(chart, barParams, dataPoint, left, top));

                        // Apply animation
                        if (animationEnabled)
                        {
                            if (dataPoint.Parent.Storyboard == null)
                                dataPoint.Parent.Storyboard = new Storyboard();

                            DataSeriesRef = dataPoint.Parent;

                            // Apply animation to the data points i.e to the rectangles that form the columns
                            dataPoint.Parent.Storyboard = ApplyStackedBarChartAnimation(barVisual, dataPoint.Parent.Storyboard, barParams, (1.0 / seriesList.Count) * (Double)(seriesList.IndexOf(dataPoint.Parent)), 1.0 / seriesList.Count);

                            // Apply animation to the marker and labels
                            dataPoint.Parent.Storyboard = ApplyMarkerAnimationToBarChart(dataPoint.Marker, dataPoint.Parent.Storyboard, 1);
                        }

                        left = right;
                    }

                    prevSum = 0;
                    right = Graphics.ValueToPixelPosition(0, width, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, limitingYValue);
                    // Plot negative values
                    foreach (DataPoint dataPoint in plotGroup.XWiseStackedDataList[xValue].Negative)
                    {
                        if (!(Boolean)dataPoint.Enabled || Double.IsNaN(dataPoint.InternalYValue))
                            continue;

                        barParams.Bevel = dataPoint.Parent.Bevel;
                        barParams.Lighting = (Boolean)dataPoint.LightingEnabled;
                        barParams.Shadow = (Boolean)dataPoint.ShadowEnabled;
                        barParams.BorderBrush = dataPoint.BorderColor;
                        barParams.BorderThickness = ((Thickness) dataPoint.BorderThickness).Left;
                        barParams.BorderStyle = Graphics.BorderStyleToStrokeDashArray((BorderStyles)dataPoint.BorderStyle);
                        barParams.IsTopOfStack = (dataPoint == plotGroup.XWiseStackedDataList[xValue].Negative.Last());
                        if (barParams.IsTopOfStack)
                        {
                            barParams.XRadius = new CornerRadius(dataPoint.RadiusX.Value.TopLeft,0,0,dataPoint.RadiusX.Value.BottomLeft);
                            barParams.YRadius = new CornerRadius(dataPoint.RadiusY.Value.TopRight,0,0,dataPoint.RadiusY.Value.BottomLeft);
                        }
                        barParams.IsPositive = false;

                        barParams.IsMarkerEnabled = (Boolean)dataPoint.MarkerEnabled;
                        barParams.MarkerType = (MarkerTypes)dataPoint.MarkerType;
                        barParams.MarkerColor = dataPoint.MarkerColor;
                        barParams.MarkerBorderColor = dataPoint.MarkerBorderColor;
                        barParams.MarkerBorderThickness = (Thickness)dataPoint.MarkerBorderThickness;
                        barParams.MarkerScale = (Double)dataPoint.MarkerScale;
                        barParams.MarkerSize = (Double)dataPoint.MarkerSize;

                        barParams.IsLabelEnabled = (Boolean)dataPoint.LabelEnabled;
                        barParams.LabelStyle = (LabelStyles)dataPoint.LabelStyle;
                        barParams.LabelText = dataPoint.TextParser(dataPoint.LabelText);
                        barParams.LabelBackground = dataPoint.LabelBackground;
                        barParams.LabelFontColor = Graphics.ApplyLabelFontColor(chart, dataPoint, dataPoint.LabelFontColor, (LabelStyles)barParams.LabelStyle);
                        barParams.LabelFontSize = (Double)dataPoint.LabelFontSize;
                        barParams.LabelFontFamily = dataPoint.LabelFontFamily;
                        barParams.LabelFontStyle = (FontStyle)dataPoint.LabelFontStyle;
                        barParams.LabelFontWeight = (FontWeight)dataPoint.LabelFontWeight;

                        percentYValue = (dataPoint.InternalYValue / absoluteSum * 100);

                        left = Graphics.ValueToPixelPosition(0, width, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, percentYValue + prevSum);
                        barWidth = Math.Abs(right - left);

                        prevSum += percentYValue;

                        barParams.BackgroundBrush = dataPoint.Color;

                        barParams.Size = new Size(barWidth, finalHeight);
                        //barParams.Size = new Size(barWidth, heightPerBar);

                        Faces bar;
                        Panel barVisual = null;

                        if (chart.View3D)
                        {
                            bar = Get3DBar(barParams);
                            barVisual = bar.Visual;
                            barVisual.SetValue(Canvas.ZIndexProperty, GetStackedBarZIndex(left, top, width, height, (dataPoint.InternalYValue > 0)));
                        }
                        else
                        {
                            bar = Get2DBar(barParams);
                            barVisual = bar.Visual;
                        }

                        dataPoint.Faces = bar;
                        dataPoint.Faces.LabelCanvas = labelCanvas;

                        barVisual.SetValue(Canvas.LeftProperty, left);
                        barVisual.SetValue(Canvas.TopProperty, top);

                        columnCanvas.Children.Add(barVisual);
                        labelCanvas.Children.Add(GetMarker(chart, barParams, dataPoint, left, top));

                        // Apply animation
                        if (animationEnabled)
                        {
                            if (dataPoint.Parent.Storyboard == null)
                                dataPoint.Parent.Storyboard = new Storyboard();

                            DataSeriesRef = dataPoint.Parent;

                            // Apply animation to the data points i.e to the rectangles that form the columns
                            dataPoint.Parent.Storyboard = ApplyStackedBarChartAnimation(barVisual, dataPoint.Parent.Storyboard, barParams, (1.0 / seriesList.Count) * (Double)(seriesList.IndexOf(dataPoint.Parent)), 1.0 / seriesList.Count);

                            // Apply animation to the marker and labels
                            dataPoint.Parent.Storyboard = ApplyMarkerAnimationToBarChart(dataPoint.Marker, dataPoint.Parent.Storyboard, 1);
                        }

                        right = left;
                    }

                }

            }
            if (!plankDrawn && chart.View3D && plotGroupList[0].AxisY.InternalAxisMinimum < 0 && plotGroupList[0].AxisY.InternalAxisMaximum > 0)
            {
                RectangularChartShapeParams columnParams = new RectangularChartShapeParams();
                columnParams.BackgroundBrush = new SolidColorBrush(Color.FromArgb((Byte)255, (Byte)127, (Byte)127, (Byte)127));
                columnParams.Lighting = true;
                columnParams.Size = new Size(1, height);
                columnParams.Depth = depth3d;

                Faces zeroPlank = ColumnChart.Get3DColumn(columnParams);
                Panel zeroPlankVisual = zeroPlank.Visual;

                Double left = Graphics.ValueToPixelPosition(0, width, (Double)plotGroupList[0].AxisY.InternalAxisMinimum, (Double)plotGroupList[0].AxisY.InternalAxisMaximum, 0);
                zeroPlankVisual.SetValue(Canvas.LeftProperty, left);
                zeroPlankVisual.SetValue(Canvas.TopProperty, (Double)0);
                zeroPlankVisual.SetValue(Canvas.ZIndexProperty, 0);
                zeroPlankVisual.Opacity = 0.7;
                columnCanvas.Children.Add(zeroPlankVisual);
            }
            visual.Children.Add(columnCanvas);
            visual.Children.Add(labelCanvas);
            return visual;
        }

        internal static Faces Get2DBar(RectangularChartShapeParams columnParams)
        {
            Faces faces = new Faces();
            faces.Parts = new List<FrameworkElement>();

            Grid columnVisual = new Grid();

            columnVisual.Width = columnParams.Size.Width;
            columnVisual.Height = columnParams.Size.Height;

            Brush background = (columnParams.Lighting ? Graphics.GetLightingEnabledBrush(columnParams.BackgroundBrush, "Linear",null) : columnParams.BackgroundBrush);

            Canvas columnBase = ExtendedGraphics.Get2DRectangle(columnParams.Size.Width, columnParams.Size.Height,
                columnParams.BorderThickness, columnParams.BorderStyle, columnParams.BorderBrush,
                background, columnParams.XRadius, columnParams.YRadius);

            (columnBase.Children[0] as FrameworkElement).Tag = "ColumnBase";
            faces.Parts.Add(columnBase.Children[0] as FrameworkElement);

            columnVisual.Children.Add(columnBase);

            if (columnParams.Size.Height > 7 && columnParams.Size.Width > 14 && columnParams.Bevel)
            {
                Canvas bevelCanvas = ExtendedGraphics.Get2DRectangleBevel(columnParams.Size.Width - columnParams.BorderThickness - columnParams.BorderThickness, columnParams.Size.Height - columnParams.BorderThickness - columnParams.BorderThickness, 6, 6,
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

            if (columnParams.Lighting && columnParams.Bevel)
            {
                Canvas gradienceCanvas = ExtendedGraphics.Get2DRectangleGradiance(columnParams.Size.Width, columnParams.Size.Height,
                    Graphics.GetLeftGradianceBrush(63),
                    Graphics.GetLeftGradianceBrush(63),
                    Orientation.Horizontal);

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
                            shadowHeight = columnParams.Size.Height - columnParams.ShadowOffset;
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
                            shadowHeight = columnParams.Size.Height - columnParams.ShadowOffset;
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
                Grid shadowGrid = ExtendedGraphics.Get2DRectangleShadow(columnParams.Size.Width, shadowHeight, xRadius, yRadius, columnParams.IsStacked ? 3 : 5);
                TranslateTransform tt = new TranslateTransform() { X = columnParams.ShadowOffset, Y = shadowVerticalOffset };
                shadowGrid.Opacity = 0.7;
                shadowGrid.SetValue(Canvas.ZIndexProperty, -1);
                shadowGrid.RenderTransform = tt;
                columnVisual.Children.Add(shadowGrid);
            }
            
            faces.VisualComponents.Add(columnVisual);

            faces.Visual = columnVisual;

            return faces;
        }

        internal static Faces Get3DBar(RectangularChartShapeParams columnParams)
        {
            Faces faces = new Faces();
            faces.Parts = new List<FrameworkElement>();
            Canvas columnVisual = new Canvas();

            columnVisual.Width = columnParams.Size.Width;
            columnVisual.Height = columnParams.Size.Height;

            Brush frontBrush = columnParams.Lighting ? Graphics.GetFrontFaceBrush(columnParams.BackgroundBrush) : columnParams.BackgroundBrush;
            Brush topBrush = columnParams.Lighting ? Graphics.GetTopFaceBrush(columnParams.BackgroundBrush) : columnParams.BackgroundBrush;
            Brush rightBrush = columnParams.Lighting ? Graphics.GetRightFaceBrush(columnParams.BackgroundBrush) : columnParams.BackgroundBrush;

            Canvas front = ExtendedGraphics.Get2DRectangle(columnParams.Size.Width, columnParams.Size.Height,
                columnParams.BorderThickness, columnParams.BorderStyle, columnParams.BorderBrush,
                frontBrush, new CornerRadius(0), new CornerRadius(0));

            faces.Parts.Add(front.Children[0] as FrameworkElement);

            Canvas top = ExtendedGraphics.Get2DRectangle(columnParams.Size.Width, columnParams.Depth,
                columnParams.BorderThickness, columnParams.BorderStyle, columnParams.BorderBrush,
                topBrush, new CornerRadius(0), new CornerRadius(0));

            faces.Parts.Add(top.Children[0] as FrameworkElement);

            top.RenderTransformOrigin = new Point(0, 1);
            SkewTransform skewTransTop = new SkewTransform();
            skewTransTop.AngleX = -45;
            top.RenderTransform = skewTransTop;

            Canvas right = ExtendedGraphics.Get2DRectangle(columnParams.Depth, columnParams.Size.Height,
                columnParams.BorderThickness, columnParams.BorderStyle, columnParams.BorderBrush,
                rightBrush, new CornerRadius(0), new CornerRadius(0));

            faces.Parts.Add(right.Children[0] as FrameworkElement);

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

        private static Dictionary<Axis, Dictionary<Axis, Int32>> GetSeriesIndex(List<DataSeries> seriesList)
        {
            Dictionary<Axis, Dictionary<Axis, Int32>> seriesIndex = new Dictionary<Axis, Dictionary<Axis, Int32>>();

            var seriesByAxis = (from series in seriesList
                                group series
                                    by new
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

        private static Int32 GetBarZIndex(Double left, Double top, Double height, Boolean isPositive)
        {
            Int32 yOffset = (Int32)(height - top);
            Int32 zindex = (Int32)(Math.Sqrt(Math.Pow(left / 2, 2) + Math.Pow(yOffset, 2)));
            if (isPositive)
                return zindex;
            else
                return Int32.MinValue + zindex;
        }

        //private static Int32 GetStackedBarZIndex(Double left, Double top, Double width, Boolean isPositive)
        //{
        //    Double zOffset = Math.Pow(10, (Int32)(Math.Log10(width) - 1));
        //    Int32 iOffset = (Int32)(left / (zOffset < 1 ? 1 : zOffset));
        //    Int32 zindex = (Int32)((top) * zOffset) + iOffset;
        //    if (isPositive)
        //        return zindex;
        //    else
        //        return Int32.MinValue + zindex;
        //}

        private static Int32 GetStackedBarZIndex(Double left, Double top, Double width, Double height, Boolean isPositive)
        {
            Int32 yOffset = (Int32)(height - top);
            Double zOffset = Math.Pow(10, (Int32)(Math.Log10(width) - 1));
            Int32 iOffset = (Int32)(left / (zOffset < 1 ? 1 : zOffset) + Math.Pow(yOffset, 2));
            Int32 zindex = (Int32)((top) * zOffset) + iOffset;
            if (isPositive)
                return zindex;
            else
                return Int32.MinValue + zindex;
        }

        private static List<KeySpline> GenerateKeySplineList(params Point[] values)
        {
            List<KeySpline> splines = new List<KeySpline>();
            for (Int32 i = 0; i < values.Length; i += 2)
                splines.Add(GetKeySpline(values[i], values[i + 1]));

            return splines;
        }

        private static KeySpline GetKeySpline(Point controlPoint1, Point controlPoint2)
        {
            return new KeySpline() { ControlPoint1 = controlPoint1, ControlPoint2 = controlPoint2 };
        }

        private static Storyboard ApplyBarChartAnimation(Panel bar, Storyboard storyboard, RectangularChartShapeParams columnParams)
        {

            ScaleTransform scaleTransform = new ScaleTransform() { ScaleX= 0 };
            bar.RenderTransform = scaleTransform;

            if (columnParams.IsPositive)
            {
                bar.RenderTransformOrigin = new Point(0, 0.5);
            }
            else
            {
                bar.RenderTransformOrigin = new Point(1, 0.5);
            }
            DoubleCollection values = Graphics.GenerateDoubleCollection(0, 1);
            DoubleCollection frameTimes = Graphics.GenerateDoubleCollection(0, 0.75);
            List<KeySpline> splines = GenerateKeySplineList
                (
                new Point(0, 0), new Point(1, 1),
                new Point(0, 0), new Point(0.5, 1)
                );

            DoubleAnimationUsingKeyFrames growAnimation = Graphics.CreateDoubleAnimation(DataSeriesRef, scaleTransform, "(ScaleTransform.ScaleX)", 0.5, frameTimes, values, splines);
            storyboard.Stop();
            storyboard.Children.Add(growAnimation);
            
            return storyboard;
        }
        private static Storyboard ApplyStackedBarChartAnimation(Panel column, Storyboard storyboard, RectangularChartShapeParams columnParams, Double begin, Double duration)
        {
            ScaleTransform scaleTransform = new ScaleTransform() { ScaleX = 0 };
            column.RenderTransform = scaleTransform;

            column.RenderTransformOrigin = new Point(0.5, 0.5);

            DoubleCollection values = Graphics.GenerateDoubleCollection(0, 1.5, 0.75, 1.125, 0.9325, 1);
            DoubleCollection frameTimes = Graphics.GenerateDoubleCollection(0, 0.25 * duration, 0.5 * duration, 0.75 * duration, 1.0 * duration, 1.25 * duration);
            List<KeySpline> splines = GenerateKeySplineList
                (
                new Point(0, 0), new Point(1, 0.5),
                new Point(0, 0), new Point(0.5, 1),
                new Point(0, 0), new Point(1, 0.5),
                new Point(0, 0), new Point(0.5, 1),
                new Point(0, 0), new Point(1, 0.5),
                new Point(0, 0), new Point(0.5, 1)
                );

            DoubleAnimationUsingKeyFrames growAnimation = Graphics.CreateDoubleAnimation(DataSeriesRef, scaleTransform, "(ScaleTransform.ScaleX)", begin + 0.5, frameTimes, values, splines);
            storyboard.Stop();
            storyboard.Children.Add(growAnimation);
            return storyboard;
        }

        private static Storyboard ApplyMarkerAnimationToBarChart(Marker marker, Storyboard storyboard, Double beginTime)
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

        private static DataSeries DataSeriesRef
        {
            get;
            set;
        }

    }
}