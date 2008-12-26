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
    }

    public class SortedDataPoints
    {
        public SortedDataPoints()
        {
        }
        public SortedDataPoints(List<DataPoint> positive, List<DataPoint> negative)
        {
            Positive = positive;
            Negative = negative;
        }

        public List<DataPoint> Positive { get; private set; }
        public List<DataPoint> Negative { get; private set; }
    }

    public class ColumnChart
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
                        positionXY = new Point(columnParams.Size.Width / 2, 0);
                    else
                        positionXY = new Point(columnParams.Size.Width / 2, 0);
                else
                    if (chart.View3D)
                        positionXY = new Point(columnParams.Size.Width / 2, columnParams.Size.Height);
                    else
                        positionXY = new Point(columnParams.Size.Width / 2, columnParams.Size.Height);

                if (columnParams.IsLabelEnabled && !String.IsNullOrEmpty(labelText))
                {
                    dataPoint.Marker.CreateVisual();

                    if (columnParams.Size.Width < dataPoint.Marker.TextBlockSize.Width)
                        dataPoint.Marker.TextOrientation = Orientation.Vertical;
                    else
                        dataPoint.Marker.TextOrientation = Orientation.Horizontal;

                    if (columnParams.IsPositive)
                    {
                        if (dataPoint.Marker.TextOrientation == Orientation.Vertical)
                        {
                            if (top + dataPoint.Marker.MarkerActualSize.Height + chart.ChartArea.PlankDepth + chart.ChartArea.PlankThickness < chart.Height - chart.PlotArea.PlotAreaBorderElement.Height)
                                columnParams.LabelStyle = LabelStyles.Inside;
                        }
                        else
                        {
                            if (top + dataPoint.Marker.MarkerActualSize.Height < (chart.Height - chart.PlotArea.PlotAreaBorderElement.Height) / 2)
                                columnParams.LabelStyle = LabelStyles.Inside;
                        }
                    }
                    else
                    {
                        if (dataPoint.Marker.TextOrientation == Orientation.Vertical)
                        {
                            if ((chart.Height - chart.PlotArea.PlotAreaBorderElement.Height) + top + positionXY.Y + dataPoint.Marker.MarkerActualSize.Height > chart.PlotArea.PlotAreaBorderElement.Height + chart.ChartArea.PlankDepth + chart.ChartArea.PlankThickness + dataPoint.Marker.MarkerSize.Height)
                                columnParams.LabelStyle = LabelStyles.Inside;
                        }
                        else
                        {
                            if ((chart.Height - chart.PlotArea.PlotAreaBorderElement.Height) / 2 + top + positionXY.Y + dataPoint.Marker.MarkerActualSize.Height > chart.PlotArea.PlotAreaBorderElement.Height + chart.ChartArea.PlankDepth + chart.ChartArea.PlankThickness)
                                columnParams.LabelStyle = LabelStyles.Inside;
                        }
                    }

                    dataPoint.Marker.TextAlignmentX = AlignmentX.Center;

                    if (!columnParams.IsMarkerEnabled)
                    {
                        if (chart.View3D)
                        {
                            if (columnParams.LabelStyle == LabelStyles.OutSide)
                            {
                                if (columnParams.IsPositive)
                                    dataPoint.Marker.MarkerSize = new Size(markerSize.Width + chart.ChartArea.PlankDepth + chart.ChartArea.PlankThickness, markerSize.Height + chart.ChartArea.PlankDepth + chart.ChartArea.PlankThickness);
                                else
                                    dataPoint.Marker.MarkerSize = new Size(markerSize.Width, markerSize.Height);
                            }
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
                        dataPoint.Marker.TextAlignmentY = columnParams.LabelStyle == LabelStyles.Inside ? AlignmentY.Bottom : AlignmentY.Top;
                    else
                        dataPoint.Marker.TextAlignmentY = columnParams.LabelStyle == LabelStyles.Inside ? AlignmentY.Top : AlignmentY.Bottom;

                }


                dataPoint.Marker.CreateVisual();

                dataPoint.Marker.AddToParent(markerCanvas, positionXY.X, positionXY.Y, new Point(0.5, 0.5));

            }

            return markerCanvas;
        }

        internal static Canvas GetVisualObjectForColumnChart(Double width, Double height, PlotDetails plotDetails, List<DataSeries> dataSeriesList4Rendering, Chart chart, Double plankDepth, bool animationEnabled)
        {
            if (Double.IsNaN(width) || Double.IsNaN(height) || width <= 0 || height <= 0)
                return null;

            Double columnGapRatio = 0.1;

            Dictionary<Double, SortedDataPoints> sortedDataPoints = null;

            sortedDataPoints = plotDetails.GetDataPointsGroupedByXValue(RenderAs.Column);

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

            List<PlotGroup> plotGroupList = (from plots in plotDetails.PlotGroups where plots.RenderAs == RenderAs.Column select plots).ToList();

            Double depth3d = plankDepth / plotDetails.Layer3DCount * (chart.View3D ? 1 : 0);
            Double visualOffset = depth3d * (plotDetails.SeriesDrawingIndex[plotGroupList[0].DataSeriesList[0]] + 1);
            visual.SetValue(Canvas.TopProperty, visualOffset);
            visual.SetValue(Canvas.LeftProperty, -visualOffset);

            Double minDiffValue = plotDetails.GetMinOfMinDifferencesForXValue(RenderAs.Column, RenderAs.StackedColumn, RenderAs.StackedColumn100);

            Axis axisXwithMinInterval = dataSeriesList4Rendering[0].PlotGroup.AxisX;

            minDiffValue = (minDiffValue < (Double)axisXwithMinInterval.InternalInterval) ? minDiffValue : (Double)axisXwithMinInterval.InternalInterval;

            Double dataAxisDifference = Math.Abs((Double)axisXwithMinInterval.InternalAxisMinimum - (Double)axisXwithMinInterval.Minimum) * 2;

            Double dataMinimumGap = Graphics.ValueToPixelPosition(0, width, (Double)axisXwithMinInterval.InternalAxisMinimum, (Double)axisXwithMinInterval.InternalAxisMaximum, dataAxisDifference + (Double)axisXwithMinInterval.InternalAxisMinimum);
            Double minDiffGap = Graphics.ValueToPixelPosition(0, width, (Double)axisXwithMinInterval.InternalAxisMinimum, (Double)axisXwithMinInterval.InternalAxisMaximum, minDiffValue + (Double)axisXwithMinInterval.InternalAxisMinimum);

            if (dataMinimumGap > 0 && minDiffGap > 0)
                minDiffGap = Math.Min(minDiffGap, dataMinimumGap);
            else
                minDiffGap = Math.Max(minDiffGap, dataMinimumGap);

            Double maxColumnWidth = minDiffGap * (1 - columnGapRatio);

            Double numberOfDivisions = plotDetails.GetMaxDivision(sortedDataPoints);

            Double widthPerColumn = maxColumnWidth / numberOfDivisions;

            Boolean plankDrawn = false;

            foreach (Double xValue in xValues)
            {
                RectangularChartShapeParams columnParams = new RectangularChartShapeParams();
                columnParams.ShadowOffset = 5;
                columnParams.Depth = depth3d;

                foreach (DataPoint dataPoint in sortedDataPoints[xValue].Positive)
                {
                    columnParams.Bevel = dataPoint.Parent.Bevel;
                    columnParams.Lighting = (Boolean)dataPoint.Parent.LightingEnabled;
                    columnParams.Shadow = dataPoint.Parent.ShadowEnabled;
                    columnParams.BorderBrush = dataPoint.BorderColor;
                    columnParams.BorderThickness = ((Thickness)dataPoint.InternalBorderThickness).Left;
                    columnParams.BorderStyle = Graphics.BorderStyleToStrokeDashArray((BorderStyles)dataPoint.BorderStyle);
                    columnParams.XRadius = new CornerRadius(dataPoint.RadiusX.Value.TopLeft, dataPoint.RadiusX.Value.TopRight, 0, 0);
                    columnParams.YRadius = new CornerRadius(dataPoint.RadiusY.Value.TopLeft, dataPoint.RadiusY.Value.TopRight, 0, 0);
                    columnParams.IsPositive = true;
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
                    columnParams.LabelFontColor = Graphics.ApplyLabelFontColor(chart, dataPoint, dataPoint.LabelFontColor, (LabelStyles)columnParams.LabelStyle);
                    columnParams.LabelFontSize = (Double)dataPoint.LabelFontSize;
                    columnParams.LabelFontFamily = dataPoint.LabelFontFamily;
                    columnParams.LabelFontStyle = (FontStyle)dataPoint.LabelFontStyle;
                    columnParams.LabelFontWeight = (FontWeight)dataPoint.LabelFontWeight;

                    PlotGroup plotGroup = dataPoint.Parent.PlotGroup;

                    Double limitingYValue = 0;
                    if (plotGroup.AxisY.InternalAxisMinimum > 0)
                        limitingYValue = (Double)plotGroup.AxisY.InternalAxisMinimum;
                    if (plotGroup.AxisY.InternalAxisMaximum < 0)
                        limitingYValue = (Double)plotGroup.AxisY.InternalAxisMaximum;

                    List<DataSeries> indexSeriesList = plotDetails.GetSeriesFromSortedPoints(sortedDataPoints[xValue]);

                    Int32 drawingIndex = indexSeriesList.IndexOf(dataPoint.Parent);

                    //Int32 drawingIndex = plotGroup.DataSeriesList.IndexOf(dataPoint.Parent);

                    if (dataPoint.YValue > (Double)plotGroup.AxisY.InternalAxisMaximum)
                        System.Diagnostics.Debug.WriteLine("Max Value greater then axis max");

                    Double left = Graphics.ValueToPixelPosition(0, width, (Double)plotGroup.AxisX.InternalAxisMinimum, (Double)plotGroup.AxisX.InternalAxisMaximum, xValue);
                    left = left + ((Double)drawingIndex - (Double)indexSeriesList.Count() / (Double)2) * widthPerColumn;
                    Double bottom = Graphics.ValueToPixelPosition(height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, limitingYValue);
                    Double top = Graphics.ValueToPixelPosition(height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, dataPoint.YValue);
                    Double columnHeight = Math.Abs(top - bottom);

                    Double finalWidth = widthPerColumn;
                    Double minPosValue = 0;
                    Double maxPosValue = width;
                    if (left < minPosValue)
                    {
                        finalWidth = left + widthPerColumn - minPosValue;
                        left = minPosValue;
                    }
                    else if (left + widthPerColumn > maxPosValue)
                    {
                        finalWidth = maxPosValue - left;
                    }

                    if (finalWidth < 0)
                        return null;

                    columnParams.Size = new Size(finalWidth, columnHeight);

                    Faces columnFaces;
                    Panel columnVisual = null;

                    if (chart.View3D)
                    {
                        columnFaces = Get3DColumn(columnParams);
                        columnVisual = columnFaces.Visual;
                        columnVisual.SetValue(Canvas.ZIndexProperty, GetColumnZIndex(left, top, (dataPoint.YValue > 0)));
                    }
                    else
                    {
                        columnFaces = Get2DColumn(columnParams);
                        columnVisual = columnFaces.Visual;
                    }

                    dataPoint.Faces = columnFaces;
                    dataPoint.Faces.LabelCanvas = labelCanvas;

                    columnVisual.SetValue(Canvas.LeftProperty, left);
                    columnVisual.SetValue(Canvas.TopProperty, top);

                    columnCanvas.Children.Add(columnVisual);

                    labelCanvas.Children.Add(GetMarker(chart, columnParams, dataPoint, left, top));

                    // Apply animation
                    if (animationEnabled)
                    {
                        if (dataPoint.Parent.Storyboard == null)
                            dataPoint.Parent.Storyboard = new Storyboard();

                        DataSeriesRef = dataPoint.Parent;

                        // Apply animation to the data points i.e to the rectangles that form the columns
                        dataPoint.Parent.Storyboard = ApplyColumnChartAnimation(columnVisual, dataPoint.Parent.Storyboard, columnParams);

                        // Apply animation to the marker and labels
                        dataPoint.Parent.Storyboard = ApplyMarkerAnimationToColumnChart(dataPoint.Marker, dataPoint.Parent.Storyboard, 1);
                    }


                }

                foreach (DataPoint dataPoint in sortedDataPoints[xValue].Negative)
                {
                    columnParams.Bevel = dataPoint.Parent.Bevel;
                    columnParams.Lighting = (Boolean)dataPoint.Parent.LightingEnabled;
                    columnParams.Shadow = dataPoint.Parent.ShadowEnabled;
                    columnParams.BorderBrush = dataPoint.BorderColor;
                    columnParams.BorderThickness = ((Thickness)dataPoint.InternalBorderThickness).Left;
                    columnParams.BorderStyle = Graphics.BorderStyleToStrokeDashArray((BorderStyles)dataPoint.BorderStyle);
                    columnParams.XRadius = new CornerRadius(0, 0, dataPoint.RadiusX.Value.BottomRight, dataPoint.RadiusX.Value.BottomLeft);
                    columnParams.YRadius = new CornerRadius(0, 0, dataPoint.RadiusY.Value.BottomRight, dataPoint.RadiusY.Value.BottomLeft);
                    columnParams.IsPositive = false;
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
                    columnParams.LabelFontColor = Graphics.ApplyLabelFontColor(chart, dataPoint, dataPoint.LabelFontColor, (LabelStyles)columnParams.LabelStyle);
                    columnParams.LabelFontSize = (Double)dataPoint.LabelFontSize;
                    columnParams.LabelFontFamily = dataPoint.LabelFontFamily;
                    columnParams.LabelFontStyle = (FontStyle)dataPoint.LabelFontStyle;
                    columnParams.LabelFontWeight = (FontWeight)dataPoint.LabelFontWeight;

                    PlotGroup plotGroup = dataPoint.Parent.PlotGroup;

                    Double limitingYValue = 0;
                    if (plotGroup.AxisY.InternalAxisMinimum > 0)
                        limitingYValue = (Double)plotGroup.AxisY.InternalAxisMinimum;
                    if (plotGroup.AxisY.InternalAxisMaximum < 0)
                        limitingYValue = (Double)plotGroup.AxisY.InternalAxisMaximum;

                    List<DataSeries> indexSeriesList = plotDetails.GetSeriesFromSortedPoints(sortedDataPoints[xValue]);

                    Int32 drawingIndex = indexSeriesList.IndexOf(dataPoint.Parent);
                    //Int32 drawingIndex = plotGroup.DataSeriesList.IndexOf(dataPoint.Parent);

                    Double left = Graphics.ValueToPixelPosition(0, width, (Double)plotGroup.AxisX.InternalAxisMinimum, (Double)plotGroup.AxisX.InternalAxisMaximum, xValue);
                    //left = drawingIndex * widthPerColumn - (maxColumnWidth / 2);
                    left = left + ((Double)drawingIndex - (Double)indexSeriesList.Count() / (Double)2) * widthPerColumn;
                    Double bottom = Graphics.ValueToPixelPosition(height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, dataPoint.YValue);
                    Double top = Graphics.ValueToPixelPosition(height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, limitingYValue);
                    Double columnHeight = Math.Abs(top - bottom);


                    Double finalWidth = widthPerColumn;
                    Double minPosValue = 0;
                    Double maxPosValue = width;
                    if (left < minPosValue)
                    {
                        finalWidth = left + widthPerColumn - minPosValue;
                        left = minPosValue;
                    }
                    else if (left + widthPerColumn > maxPosValue)
                    {
                        finalWidth = maxPosValue - left;
                    }

                    columnParams.Size = new Size(finalWidth, columnHeight);

                    Faces column;
                    Panel columnVisual = null;

                    if (chart.View3D)
                    {
                        column = Get3DColumn(columnParams);
                        columnVisual = column.Visual;
                        columnVisual.SetValue(Canvas.ZIndexProperty, GetColumnZIndex(left, top, (dataPoint.YValue > 0)));
                    }
                    else
                    {
                        column = Get2DColumn(columnParams);
                        columnVisual = column.Visual;
                    }

                    dataPoint.Faces = column;
                    dataPoint.Faces.LabelCanvas = labelCanvas;

                    columnVisual.SetValue(Canvas.LeftProperty, left);
                    columnVisual.SetValue(Canvas.TopProperty, top);

                    columnCanvas.Children.Add(columnVisual);

                    labelCanvas.Children.Add(GetMarker(chart, columnParams, dataPoint, left, top));

                    // Apply animation
                    if (animationEnabled)
                    {
                        if (dataPoint.Parent.Storyboard == null)
                            dataPoint.Parent.Storyboard = new Storyboard();

                        DataSeriesRef = dataPoint.Parent;

                        // Apply animation to the data points i.e to the rectangles that form the columns
                        dataPoint.Parent.Storyboard = ApplyColumnChartAnimation(columnVisual, dataPoint.Parent.Storyboard, columnParams);

                        // Apply animation to the marker and labels
                        dataPoint.Parent.Storyboard = ApplyMarkerAnimationToColumnChart(dataPoint.Marker, dataPoint.Parent.Storyboard, 1);
                    }

                }
            }
            if (!plankDrawn && chart.View3D && dataSeriesList4Rendering[0].PlotGroup.AxisY.InternalAxisMinimum < 0 && dataSeriesList4Rendering[0].PlotGroup.AxisY.InternalAxisMaximum > 0)
            {
                RectangularChartShapeParams columnParams = new RectangularChartShapeParams();
                columnParams.BackgroundBrush = new SolidColorBrush(Color.FromArgb((Byte)255, (Byte)127, (Byte)127, (Byte)127));
                columnParams.Lighting = true;
                columnParams.Size = new Size(width, 1);
                columnParams.Depth = depth3d;

                Faces zeroPlank = Get3DColumn(columnParams);
                Panel zeroPlankVisual = zeroPlank.Visual;

                Double top = height - Graphics.ValueToPixelPosition(0, height, (Double)dataSeriesList4Rendering[0].PlotGroup.AxisY.InternalAxisMinimum, (Double)dataSeriesList4Rendering[0].PlotGroup.AxisY.InternalAxisMaximum, 0);
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

        internal static Canvas GetVisualObjectForStackedColumnChart(Double width, Double height, PlotDetails plotDetails, Chart chart, Double plankDepth, bool animationEnabled)
        {
            if (Double.IsNaN(width) || Double.IsNaN(height) || width <= 0 || height <= 0) return null;

            List<PlotGroup> plotGroupList = (from plots in plotDetails.PlotGroups where plots.RenderAs == RenderAs.StackedColumn select plots).ToList();

            Double widthDivisionFactor = plotDetails.DrawingDivisionFactor;
            Double columnGapRatio = 0.2;

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

            List<DataSeries> seriesList = plotDetails.GetSeriesListByRenderAs(RenderAs.StackedColumn);

            Dictionary<Axis, Dictionary<Axis, Int32>> seriesIndex = GetSeriesIndex(seriesList);

            foreach (PlotGroup plotGroup in plotGroupList)
            {
                Int32 drawingIndex = seriesIndex[plotGroup.AxisY][plotGroup.AxisX];

                Double minDiff = plotDetails.GetMinOfMinDifferencesForXValue(RenderAs.Column, RenderAs.StackedColumn, RenderAs.StackedColumn100);

                minDiff = (minDiff < (Double)plotGroup.AxisX.InternalInterval) ? minDiff : (Double)plotGroup.AxisX.InternalInterval;

                Double maxColumnWidth = Graphics.ValueToPixelPosition(0, width, (Double)plotGroup.AxisX.InternalAxisMinimum, (Double)plotGroup.AxisX.InternalAxisMaximum, minDiff + (Double)plotGroup.AxisX.InternalAxisMinimum) * (1 - columnGapRatio);
                Double widthPerColumn = maxColumnWidth / widthDivisionFactor;

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
                    columnParams.IsStacked = true;

                    Double left = Graphics.ValueToPixelPosition(0, width, (Double)plotGroup.AxisX.InternalAxisMinimum, (Double)plotGroup.AxisX.InternalAxisMaximum, xValue) + drawingIndex * widthPerColumn - (maxColumnWidth / 2);
                    Double bottom = Graphics.ValueToPixelPosition(height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, limitingYValue);

                    Double top;
                    Double columnHeight;
                    Double prevSum = 0;
                    Int32 index = 1;
                    // Plot positive values
                    foreach (DataPoint dataPoint in plotGroup.XWiseStackedDataList[xValue].Positive)
                    {
                        columnParams.Bevel = dataPoint.Parent.Bevel;
                        columnParams.Lighting = (Boolean)dataPoint.Parent.LightingEnabled;
                        columnParams.Shadow = dataPoint.Parent.ShadowEnabled;
                        columnParams.BorderBrush = dataPoint.BorderColor;
                        columnParams.BorderThickness = ((Thickness)dataPoint.InternalBorderThickness).Left;
                        columnParams.BorderStyle = Graphics.BorderStyleToStrokeDashArray((BorderStyles)dataPoint.BorderStyle);
                        columnParams.IsTopOfStack = (dataPoint == plotGroup.XWiseStackedDataList[xValue].Positive.Last());
                        if (columnParams.IsTopOfStack)
                        {
                            columnParams.XRadius = new CornerRadius(dataPoint.RadiusX.Value.TopLeft, dataPoint.RadiusX.Value.TopRight, 0, 0);
                            columnParams.YRadius = new CornerRadius(dataPoint.RadiusY.Value.TopLeft, dataPoint.RadiusY.Value.TopRight, 0, 0);
                        }
                        columnParams.IsPositive = true;

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
                        columnParams.LabelFontColor = Graphics.ApplyLabelFontColor(chart, dataPoint, dataPoint.LabelFontColor, (LabelStyles)columnParams.LabelStyle);
                        columnParams.LabelFontSize = (Double)dataPoint.LabelFontSize;
                        columnParams.LabelFontFamily = dataPoint.LabelFontFamily;
                        columnParams.LabelFontStyle = (FontStyle)dataPoint.LabelFontStyle;
                        columnParams.LabelFontWeight = (FontWeight)dataPoint.LabelFontWeight;

                        top = Graphics.ValueToPixelPosition(height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, dataPoint.YValue + prevSum);
                        columnHeight = Math.Abs(top - bottom);

                        prevSum += dataPoint.YValue;

                        columnParams.BackgroundBrush = dataPoint.Color;
                        columnParams.Size = new Size(widthPerColumn, columnHeight);

                        Faces column;
                        Panel columnVisual = null;

                        if (chart.View3D)
                        {
                            column = Get3DColumn(columnParams);
                            columnVisual = column.Visual;
                            columnVisual.SetValue(Canvas.ZIndexProperty, GetStackedColumnZIndex(left, top, (dataPoint.YValue > 0), index++));
                        }
                        else
                        {
                            column = Get2DColumn(columnParams);
                            columnVisual = column.Visual;
                        }

                        dataPoint.Faces = column;
                        dataPoint.Faces.LabelCanvas = labelCanvas;

                        columnVisual.SetValue(Canvas.LeftProperty, left);
                        columnVisual.SetValue(Canvas.TopProperty, top);

                        columnCanvas.Children.Add(columnVisual);
                        labelCanvas.Children.Add(GetMarker(chart, columnParams, dataPoint, left, top));

                        // Apply animation
                        if (animationEnabled)
                        {
                            if (dataPoint.Parent.Storyboard == null)
                                dataPoint.Parent.Storyboard = new Storyboard();

                            DataSeriesRef = dataPoint.Parent;

                            // Apply animation to the data points i.e to the rectangles that form the columns
                            dataPoint.Parent.Storyboard = ApplyStackedColumnChartAnimation(columnVisual, dataPoint.Parent.Storyboard, columnParams, (1.0 / seriesList.Count) * (Double)(seriesList.IndexOf(dataPoint.Parent)), 1.0 / seriesList.Count);

                            // Apply animation to the marker and labels
                            dataPoint.Parent.Storyboard = ApplyMarkerAnimationToColumnChart(dataPoint.Marker, dataPoint.Parent.Storyboard, 1);
                        }

                        bottom = top;
                    }

                    prevSum = 0;
                    //index = 1;
                    top = Graphics.ValueToPixelPosition(height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, limitingYValue);
                    // Plot negative values
                    foreach (DataPoint dataPoint in plotGroup.XWiseStackedDataList[xValue].Negative)
                    {
                        columnParams.Bevel = dataPoint.Parent.Bevel;
                        columnParams.Lighting = (Boolean)dataPoint.Parent.LightingEnabled;
                        columnParams.Shadow = dataPoint.Parent.ShadowEnabled;
                        columnParams.BorderBrush = dataPoint.BorderColor;
                        columnParams.BorderThickness = ((Thickness)dataPoint.InternalBorderThickness).Left;
                        columnParams.BorderStyle = Graphics.BorderStyleToStrokeDashArray((BorderStyles)dataPoint.BorderStyle);
                        columnParams.IsTopOfStack = (dataPoint == plotGroup.XWiseStackedDataList[xValue].Negative.Last());
                        if (columnParams.IsTopOfStack)
                        {
                            columnParams.XRadius = new CornerRadius(0, 0, dataPoint.RadiusX.Value.BottomRight, dataPoint.RadiusX.Value.BottomLeft);
                            columnParams.YRadius = new CornerRadius(0, 0, dataPoint.RadiusY.Value.BottomRight, dataPoint.RadiusY.Value.BottomLeft);
                        }
                        columnParams.IsPositive = false;

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
                        columnParams.LabelFontColor = Graphics.ApplyLabelFontColor(chart, dataPoint, dataPoint.LabelFontColor, (LabelStyles)columnParams.LabelStyle);
                        columnParams.LabelFontSize = (Double)dataPoint.LabelFontSize;
                        columnParams.LabelFontFamily = dataPoint.LabelFontFamily;
                        columnParams.LabelFontStyle = (FontStyle)dataPoint.LabelFontStyle;
                        columnParams.LabelFontWeight = (FontWeight)dataPoint.LabelFontWeight;

                        bottom = Graphics.ValueToPixelPosition(height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, dataPoint.YValue + prevSum);
                        columnHeight = Math.Abs(top - bottom);

                        prevSum += dataPoint.YValue;

                        columnParams.BackgroundBrush = dataPoint.Color;
                        columnParams.Size = new Size(widthPerColumn, columnHeight);

                        Faces column;
                        Panel columnVisual = null;

                        if (chart.View3D)
                        {
                            column = Get3DColumn(columnParams);
                            columnVisual = column.Visual;
                            columnVisual.SetValue(Canvas.ZIndexProperty, GetStackedColumnZIndex(left, top, (dataPoint.YValue > 0), index++));
                        }
                        else
                        {
                            column = Get2DColumn(columnParams);
                            columnVisual = column.Visual;
                        }

                        dataPoint.Faces = column;
                        dataPoint.Faces.LabelCanvas = labelCanvas;

                        columnVisual.SetValue(Canvas.LeftProperty, left);
                        columnVisual.SetValue(Canvas.TopProperty, top);

                        columnCanvas.Children.Add(columnVisual);
                        labelCanvas.Children.Add(GetMarker(chart, columnParams, dataPoint, left, top));

                        // Apply animation
                        if (animationEnabled)
                        {
                            if (dataPoint.Parent.Storyboard == null)
                                dataPoint.Parent.Storyboard = new Storyboard();

                            DataSeriesRef = dataPoint.Parent;

                            // Apply animation to the data points i.e to the rectangles that form the columns
                            dataPoint.Parent.Storyboard = ApplyStackedColumnChartAnimation(columnVisual, dataPoint.Parent.Storyboard, columnParams, (1.0 / seriesList.Count) * (Double)(seriesList.IndexOf(dataPoint.Parent)), 1.0 / seriesList.Count);

                            // Apply animation to the marker and labels
                            dataPoint.Parent.Storyboard = ApplyMarkerAnimationToColumnChart(dataPoint.Marker, dataPoint.Parent.Storyboard, 1);
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
                Panel zeroPlankVisual = zeroPlank.Visual;

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

        internal static Canvas GetVisualObjectForStackedColumn100Chart(Double width, Double height, PlotDetails plotDetails, Chart chart, Double plankDepth, bool animationEnabled)
        {
            if (Double.IsNaN(width) || Double.IsNaN(height) || width <= 0 || height <= 0) return null;

            List<PlotGroup> plotGroupList = (from plots in plotDetails.PlotGroups where plots.RenderAs == RenderAs.StackedColumn100 select plots).ToList();

            Double widthDivisionFactor = plotDetails.DrawingDivisionFactor;
            Double columnGapRatio = 0.2;

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
                Int32 drawingIndex = seriesIndex[plotGroup.AxisY][plotGroup.AxisX];

                Double minDiff = plotDetails.GetMinOfMinDifferencesForXValue(RenderAs.Column, RenderAs.StackedColumn, RenderAs.StackedColumn100);

                minDiff = (minDiff < (Double)plotGroup.AxisX.InternalInterval) ? minDiff : (Double)plotGroup.AxisX.InternalInterval;

                Double maxColumnWidth = Graphics.ValueToPixelPosition(0, width, (Double)plotGroup.AxisX.InternalAxisMinimum, (Double)plotGroup.AxisX.InternalAxisMaximum, minDiff + (Double)plotGroup.AxisX.InternalAxisMinimum) * (1 - columnGapRatio);
                Double widthPerColumn = maxColumnWidth / widthDivisionFactor;

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

                    Double absoluteSum = plotGroup.XWiseStackedDataList[xValue].AbsoluteYValueSum;

                    if (Double.IsNaN(absoluteSum) || absoluteSum <= 0)
                        absoluteSum = 1;

                    Double top;
                    Double columnHeight;
                    Double prevSum = 0;
                    Double percentYValue;
                    Int32 index = 1;
                    // Plot positive values
                    foreach (DataPoint dataPoint in plotGroup.XWiseStackedDataList[xValue].Positive)
                    {
                        columnParams.Bevel = dataPoint.Parent.Bevel;
                        columnParams.Lighting = (Boolean)dataPoint.Parent.LightingEnabled;
                        columnParams.Shadow = dataPoint.Parent.ShadowEnabled;
                        columnParams.BorderBrush = dataPoint.BorderColor;
                        columnParams.BorderThickness = ((Thickness)dataPoint.InternalBorderThickness).Left;
                        columnParams.BorderStyle = Graphics.BorderStyleToStrokeDashArray((BorderStyles)dataPoint.BorderStyle);
                        columnParams.IsTopOfStack = (dataPoint == plotGroup.XWiseStackedDataList[xValue].Positive.Last());
                        if (columnParams.IsTopOfStack)
                        {
                            columnParams.XRadius = new CornerRadius(dataPoint.RadiusX.Value.TopLeft, dataPoint.RadiusX.Value.TopRight, 0, 0);
                            columnParams.YRadius = new CornerRadius(dataPoint.RadiusY.Value.TopLeft, dataPoint.RadiusY.Value.TopRight, 0, 0);
                        }

                        columnParams.IsPositive = true;

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
                        columnParams.LabelFontColor = Graphics.ApplyLabelFontColor(chart, dataPoint, dataPoint.LabelFontColor, (LabelStyles)columnParams.LabelStyle);
                        columnParams.LabelFontSize = (Double)dataPoint.LabelFontSize;
                        columnParams.LabelFontFamily = dataPoint.LabelFontFamily;
                        columnParams.LabelFontStyle = (FontStyle)dataPoint.LabelFontStyle;
                        columnParams.LabelFontWeight = (FontWeight)dataPoint.LabelFontWeight;

                        percentYValue = (dataPoint.YValue / absoluteSum * 100);
                        top = Graphics.ValueToPixelPosition(height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, percentYValue + prevSum);
                        columnHeight = Math.Abs(top - bottom);

                        prevSum += percentYValue;

                        columnParams.BackgroundBrush = dataPoint.Color;
                        columnParams.Size = new Size(widthPerColumn, columnHeight);

                        Faces column;
                        Panel columnVisual = null;

                        if (chart.View3D)
                        {
                            column = Get3DColumn(columnParams);
                            columnVisual = column.Visual;
                            columnVisual.SetValue(Canvas.ZIndexProperty, GetStackedColumnZIndex(left, top, (dataPoint.YValue > 0), index++));
                        }
                        else
                        {

                            column = Get2DColumn(columnParams);
                            columnVisual = column.Visual;
                        }

                        dataPoint.Faces = column;
                        dataPoint.Faces.LabelCanvas = labelCanvas;

                        columnVisual.SetValue(Canvas.LeftProperty, left);
                        columnVisual.SetValue(Canvas.TopProperty, top);

                        columnCanvas.Children.Add(columnVisual);
                        labelCanvas.Children.Add(GetMarker(chart, columnParams, dataPoint, left, top));

                        // Apply animation
                        if (animationEnabled)
                        {
                            if (dataPoint.Parent.Storyboard == null)
                                dataPoint.Parent.Storyboard = new Storyboard();

                            DataSeriesRef = dataPoint.Parent;

                            // Apply animation to the data points i.e to the rectangles that form the columns
                            dataPoint.Parent.Storyboard = ApplyStackedColumnChartAnimation(columnVisual, dataPoint.Parent.Storyboard, columnParams, (1.0 / seriesList.Count) * (Double)(seriesList.IndexOf(dataPoint.Parent)), 1.0 / seriesList.Count);

                            // Apply animation to the marker and labels
                            dataPoint.Parent.Storyboard = ApplyMarkerAnimationToColumnChart(dataPoint.Marker, dataPoint.Parent.Storyboard, 1);
                        }
                        bottom = top;
                    }

                    prevSum = 0;
                    index = 1;
                    top = Graphics.ValueToPixelPosition(height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, limitingYValue);
                    // Plot negative values
                    foreach (DataPoint dataPoint in plotGroup.XWiseStackedDataList[xValue].Negative)
                    {
                        columnParams.Bevel = dataPoint.Parent.Bevel;
                        columnParams.Lighting = (Boolean)dataPoint.Parent.LightingEnabled;
                        columnParams.Shadow = dataPoint.Parent.ShadowEnabled;
                        columnParams.BorderBrush = dataPoint.BorderColor;
                        columnParams.BorderThickness = ((Thickness)dataPoint.InternalBorderThickness).Left;
                        columnParams.BorderStyle = Graphics.BorderStyleToStrokeDashArray((BorderStyles)dataPoint.BorderStyle);
                        columnParams.IsTopOfStack = (dataPoint == plotGroup.XWiseStackedDataList[xValue].Negative.Last());
                        if (columnParams.IsTopOfStack)
                        {
                            columnParams.XRadius = new CornerRadius(0, 0, dataPoint.RadiusX.Value.BottomRight, dataPoint.RadiusX.Value.BottomLeft);
                            columnParams.YRadius = new CornerRadius(0, 0, dataPoint.RadiusY.Value.BottomRight, dataPoint.RadiusY.Value.BottomLeft);
                        }
                        columnParams.IsPositive = false;

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
                        columnParams.LabelFontColor = Graphics.ApplyLabelFontColor(chart, dataPoint, dataPoint.LabelFontColor, (LabelStyles)columnParams.LabelStyle);
                        columnParams.LabelFontSize = (Double)dataPoint.LabelFontSize;
                        columnParams.LabelFontFamily = dataPoint.LabelFontFamily;
                        columnParams.LabelFontStyle = (FontStyle)dataPoint.LabelFontStyle;
                        columnParams.LabelFontWeight = (FontWeight)dataPoint.LabelFontWeight;

                        percentYValue = (dataPoint.YValue / absoluteSum * 100);

                        bottom = Graphics.ValueToPixelPosition(height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, percentYValue + prevSum);

                        columnHeight = Math.Abs(top - bottom);

                        prevSum += percentYValue;

                        columnParams.BackgroundBrush = dataPoint.Color;
                        columnParams.Size = new Size(widthPerColumn, columnHeight);

                        Faces column;
                        Panel columnVisual = null;

                        if (chart.View3D)
                        {
                            column = Get3DColumn(columnParams);
                            columnVisual = column.Visual;
                            columnVisual.SetValue(Canvas.ZIndexProperty, GetStackedColumnZIndex(left, top, (dataPoint.YValue > 0), index++));
                        }
                        else
                        {
                            column = Get2DColumn(columnParams);
                            columnVisual = column.Visual;
                        }

                        dataPoint.Faces = column;
                        dataPoint.Faces.LabelCanvas = labelCanvas;

                        columnVisual.SetValue(Canvas.LeftProperty, left);
                        columnVisual.SetValue(Canvas.TopProperty, top);

                        columnCanvas.Children.Add(columnVisual);
                        labelCanvas.Children.Add(GetMarker(chart, columnParams, dataPoint, left, top));

                        // Apply animation
                        if (animationEnabled)
                        {
                            if (dataPoint.Parent.Storyboard == null)
                                dataPoint.Parent.Storyboard = new Storyboard();

                            DataSeriesRef = dataPoint.Parent;

                            // Apply animation to the data points i.e to the rectangles that form the columns
                            dataPoint.Parent.Storyboard = ApplyStackedColumnChartAnimation(columnVisual, dataPoint.Parent.Storyboard, columnParams, (1.0 / seriesList.Count) * (Double)(seriesList.IndexOf(dataPoint.Parent)), 1.0 / seriesList.Count);

                            // Apply animation to the marker and labels
                            dataPoint.Parent.Storyboard = ApplyMarkerAnimationToColumnChart(dataPoint.Marker, dataPoint.Parent.Storyboard, 1);
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
                Panel zeroPlankVisual = zeroPlank.Visual;

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

        private static Dictionary<Axis, Dictionary<Axis, Int32>> GetSeriesIndex(List<DataSeries> seriesList)
        {
            Dictionary<Axis, Dictionary<Axis, Int32>> seriesIndex = new Dictionary<Axis, Dictionary<Axis, Int32>>();

            var seriesByAxis = (from series in seriesList
                                where series.Enabled == true
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

        internal static Faces Get2DColumn(RectangularChartShapeParams columnParams)
        {
            Faces faces = new Faces();
            faces.Parts = new List<FrameworkElement>();
            Canvas columnVisual = new Canvas();

            columnVisual.Width = columnParams.Size.Width;
            columnVisual.Height = columnParams.Size.Height;

            Brush background = (columnParams.Lighting ? Graphics.GetLightingEnabledBrush(columnParams.BackgroundBrush, "Linear", null) : columnParams.BackgroundBrush);

            Canvas columnBase = ExtendedGraphics.Get2DRectangle(columnParams.Size.Width, columnParams.Size.Height,
                columnParams.BorderThickness, columnParams.BorderStyle, columnParams.BorderBrush,
                background, columnParams.XRadius, columnParams.YRadius);

            (columnBase.Children[0] as FrameworkElement).Tag = "ColumnBase";
            faces.Parts.Add(columnBase.Children[0] as FrameworkElement);

            columnVisual.Children.Add(columnBase);

            // if (((!columnParams.IsStacked) || (columnParams.IsStacked && columnParams.IsTopOfStack))
            //    && columnParams.Size.Height > 7 && columnParams.Size.Width > 14 && columnParams.Bevel)

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

            if (!columnParams.Lighting && columnParams.Bevel)
            {
                Canvas gradienceCanvas = ExtendedGraphics.Get2DRectangleGradiance(columnParams.Size.Width, columnParams.Size.Height,
                    Graphics.GetLeftGradianceBrush(63),
                    Graphics.GetRightGradianceBrush(63),
                    Orientation.Vertical);

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
                            shadowHeight = columnParams.Size.Height - shadowVerticalOffset + shadowVerticalOffsetGap;
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
                            shadowHeight = columnParams.Size.Height - shadowVerticalOffset + shadowVerticalOffsetGap;
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
                //TranslateTransform tt = new TranslateTransform() { X = columnParams.ShadowOffset, Y = shadowVerticalOffset };
                shadowGrid.SetValue(Canvas.TopProperty, shadowVerticalOffset);
                shadowGrid.SetValue(Canvas.LeftProperty, columnParams.ShadowOffset);
                shadowGrid.Opacity = 0.7;
                shadowGrid.SetValue(Canvas.ZIndexProperty, -1);
                //shadowGrid.RenderTransform = tt;
                columnVisual.Children.Add(shadowGrid);
            }


            faces.VisualComponents.Add(columnVisual);

            faces.Visual = columnVisual;

            return faces;
        }

        internal static Faces Get3DColumn(RectangularChartShapeParams columnParams)
        {
            return Get3DColumn(columnParams, null, null, null);
        }

        internal static Faces Get3DColumn(RectangularChartShapeParams columnParams, Brush frontBrush, Brush topBrush, Brush rightBrush)
        {
            Faces faces = new Faces();
            faces.Parts = new List<FrameworkElement>();

            Canvas columnVisual = new Canvas();

            columnVisual.Width = columnParams.Size.Width;
            columnVisual.Height = columnParams.Size.Height;

            if (frontBrush == null)
                frontBrush = columnParams.Lighting ? Graphics.GetFrontFaceBrush(columnParams.BackgroundBrush) : columnParams.BackgroundBrush;

            if (topBrush == null)
                topBrush = columnParams.Lighting ? Graphics.GetTopFaceBrush(columnParams.BackgroundBrush) : columnParams.BackgroundBrush;

            if (rightBrush == null)
                rightBrush = columnParams.Lighting ? Graphics.GetRightFaceBrush(columnParams.BackgroundBrush) : columnParams.BackgroundBrush;

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

        private static Int32 GetColumnZIndex(Double left, Double top, Boolean isPositive)
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

        private static Int32 GetStackedColumnZIndex(Double left, Double top, Boolean isPositive, Int32 index)
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


        private static Storyboard ApplyColumnChartAnimation(Panel column, Storyboard storyboard, RectangularChartShapeParams columnParams)
        {

            ScaleTransform scaleTransform = new ScaleTransform() { ScaleY = 0 };
            column.RenderTransform = scaleTransform;

            if (columnParams.IsPositive)
            {
                column.RenderTransformOrigin = new Point(0.5, 1);
            }
            else
            {
                column.RenderTransformOrigin = new Point(0.5, 0);
            }

            DoubleCollection values = Graphics.GenerateDoubleCollection(0, 1);
            DoubleCollection frameTimes = Graphics.GenerateDoubleCollection(0, 1);
            List<KeySpline> splines = GenerateKeySplineList
                (
                new Point(0, 0), new Point(1, 1),
                new Point(0, 1), new Point(0.5, 1)
                );

            DoubleAnimationUsingKeyFrames growAnimation = Graphics.CreateDoubleAnimation(DataSeriesRef, scaleTransform, "(ScaleTransform.ScaleY)", 1, frameTimes, values, splines);

            storyboard.Children.Add(growAnimation);

            return storyboard;
        }

        private static Storyboard ApplyStackedColumnChartAnimation(Panel column, Storyboard storyboard, RectangularChartShapeParams columnParams, Double begin, Double duration)
        {
            ScaleTransform scaleTransform = new ScaleTransform() { ScaleY = 0 };
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

            DoubleAnimationUsingKeyFrames growAnimation = Graphics.CreateDoubleAnimation(DataSeriesRef, scaleTransform, "(ScaleTransform.ScaleY)", begin + 0.5, frameTimes, values, splines);
            storyboard.Stop();
            storyboard.Children.Add(growAnimation);

            return storyboard;
        }

        private static Storyboard ApplyMarkerAnimationToColumnChart(Marker marker, Storyboard storyboard, Double beginTime)
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
            //storyboard.Stop();
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
