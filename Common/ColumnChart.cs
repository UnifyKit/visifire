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

using System.Windows.Shapes;

using Visifire.Commons;

namespace Visifire.Charts
{   
    /// <summary>
    /// Visifire.Charts.RectangularChartShapeParams class
    /// (Used for column and bar charts)
    /// </summary>
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
        public FrameworkElement TagReference { get; set; }
    }

    /// <summary>
    ///  Visifire.Charts.SortedDataPoints class. 
    ///  SortedDataPoints used to store InternalDataPoints with positive and negative values 
    /// </summary>
    public class SortDataPoints
    {   
        /// <summary>
        /// Initializes a new instance of the Visifire.Charts.SortedDataPoints class
        /// </summary>
        public SortDataPoints()
        {
            
        }

        /// <summary>
        /// Sort InternalDataPoints
        /// </summary>
        /// <param name="positive">Positive InternalDataPoints</param>
        /// <param name="">Negative InternalDataPoints</param>
        public SortDataPoints(List<DataPoint> positive, List<DataPoint> negative)
        {
            Positive = positive;
            Negative = negative;
        }

        /// <summary>
        /// List of InternalDataPoints with Positive values
        /// </summary>
        public List<DataPoint> Positive { get; private set; }

        /// <summary>
        /// List of InternalDataPoints with Negative values
        /// </summary>
        public List<DataPoint> Negative { get; private set; }
    }

    /// <summary>
    /// Visifire.Charts.ColumnChart class
    /// </summary>
    public partial class ColumnChart
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
        /// Calculate auto placement for DataPoint label
        /// </summary>
        /// <param name="isView3D"></param>
        /// <param name="dataPoint"></param>
        /// <param name="columnParams"></param>
        /// <param name="labelStyle"></param>
        /// <param name="labelLeft"></param>
        /// <param name="labelTop"></param>
        /// <param name="angle"></param>
        /// <param name="canvasLeft"></param>
        /// <param name="canvasTop"></param>
        /// <param name="isVertical"></param>
        /// <param name="insideGap"></param>
        /// <param name="outsideGap"></param>
        /// <param name="tb"></param>
        private static void CalculateAutoPlacement(Boolean isView3D, DataPoint dataPoint, Size columnVisualSize, Boolean isPositive, LabelStyles labelStyle, ref Double labelLeft, ref Double labelTop, ref Double angle, Double canvasLeft, Double canvasTop, Boolean isVertical, Double insideGap, Double outsideGap, Title tb, Boolean isTopOfStack)
        {
            Double radius = 0;
            Double angleInRadian = 0;
            Point centerOfRotation;

            if (isPositive)
            {
                if (labelStyle == LabelStyles.Inside)
                {
                    if (isVertical)
                    {
                        if (columnVisualSize.Height - insideGap - (dataPoint.MarkerSize / 2 * dataPoint.MarkerScale) < tb.TextBlockDesiredSize.Width)
                        {
                            labelLeft = canvasLeft + columnVisualSize.Width / 2;
                            labelTop = canvasTop - tb.TextBlockDesiredSize.Height + columnVisualSize.Height + insideGap;
                            angle = -90;
                        }
                        else
                        {
                            centerOfRotation = new Point(canvasLeft + columnVisualSize.Width / 2, canvasTop + (((Double)dataPoint.MarkerSize / 2) * (Double)dataPoint.MarkerScale + insideGap));
                            angle = -90 + 180;
                            angleInRadian = (Math.PI / 180) * angle;
                            radius += tb.Width;
                            angle = (angleInRadian - Math.PI) * (180 / Math.PI);

                            labelLeft = centerOfRotation.X + radius * Math.Cos(angleInRadian);
                            labelTop = centerOfRotation.Y + radius * Math.Sin(angleInRadian);
                        }
                    }
                    else
                    {
                        if (columnVisualSize.Height - insideGap - (dataPoint.MarkerSize / 2 * dataPoint.MarkerScale) < tb.TextBlockDesiredSize.Height)
                        {
                            labelLeft = canvasLeft + columnVisualSize.Width / 2 - tb.TextBlockDesiredSize.Width / 2;
                            labelTop = canvasTop - tb.TextBlockDesiredSize.Height + columnVisualSize.Height + insideGap;

                            if (labelTop < 0)
                                labelTop = 0;
                        }
                        else
                        {
                            labelLeft = canvasLeft + columnVisualSize.Width / 2 - tb.TextBlockDesiredSize.Width / 2;
                            labelTop = canvasTop + (((Double)dataPoint.MarkerSize / 2) * (Double)dataPoint.MarkerScale) + insideGap;
                        }
                    }
                }
                else
                {
                    if (isVertical)
                    {
                        labelTop = canvasTop - tb.TextBlockDesiredSize.Height - (((Double)dataPoint.MarkerSize / 2) * (Double)dataPoint.MarkerScale - (isView3D ? outsideGap : outsideGap + 3));
                        labelLeft = canvasLeft + columnVisualSize.Width / 2;
                        angle = -90;
                    }
                    else
                    {
                        if (dataPoint.Parent.RenderAs == RenderAs.StackedColumn100 && isView3D
                        && isTopOfStack)
                        {
                            if (!dataPoint.IsLabelStyleSet && !dataPoint.Parent.IsLabelStyleSet && !isVertical && tb.TextBlockDesiredSize.Height >= columnVisualSize.Height)
                            {
                                labelLeft = canvasLeft + columnVisualSize.Width / 2 - tb.TextBlockDesiredSize.Width / 2;
                                labelTop = canvasTop - outsideGap;// -tb.TextBlockDesiredSize.Height - (((Double)dataPoint.MarkerSize / 2) * (Double)dataPoint.MarkerScale - (isView3D ? -outsideGap : outsideGap));
                            }
                            else
                            {
                                labelLeft = canvasLeft + columnVisualSize.Width / 2 - tb.TextBlockDesiredSize.Width / 2;
                                labelTop = canvasTop - tb.TextBlockDesiredSize.Height - (((Double)dataPoint.MarkerSize / 2) * (Double)dataPoint.MarkerScale - (isView3D ? -outsideGap : outsideGap));
                            }
                        }
                        else
                        {
                            labelLeft = canvasLeft + columnVisualSize.Width / 2 - tb.TextBlockDesiredSize.Width / 2;
                            labelTop = canvasTop - tb.TextBlockDesiredSize.Height - (((Double)dataPoint.MarkerSize / 2) * (Double)dataPoint.MarkerScale - (isView3D ? -outsideGap : outsideGap));
                        }
                    }
                }
            }
            else
            {
                if (labelStyle == LabelStyles.Inside)
                {
                    if (isVertical)
                    {
                        if (columnVisualSize.Height - insideGap - (dataPoint.MarkerSize / 2 * dataPoint.MarkerScale) < tb.TextBlockDesiredSize.Width)
                        {
                            centerOfRotation = new Point(canvasLeft + columnVisualSize.Width / 2, canvasTop - columnVisualSize.Height + insideGap);
                            angle = -90 - 180;
                            angleInRadian = (Math.PI / 180) * angle;
                            radius += tb.Width;
                            angle = (angleInRadian - Math.PI) * (180 / Math.PI);

                            labelLeft = centerOfRotation.X + radius * Math.Cos(angleInRadian);
                            labelTop = centerOfRotation.Y + radius * Math.Sin(angleInRadian);
                        }
                        else
                        {
                            labelLeft = canvasLeft + columnVisualSize.Width / 2;
                            labelTop = canvasTop - (((Double)dataPoint.MarkerSize / 2) * (Double)dataPoint.MarkerScale + insideGap);
                            angle = -90;
                        }
                    }
                    else
                    {
                        if (columnVisualSize.Height + insideGap - (dataPoint.MarkerSize / 2 * dataPoint.MarkerScale) < tb.TextBlockDesiredSize.Height)
                        {
                            labelLeft = canvasLeft + columnVisualSize.Width / 2 - tb.TextBlockDesiredSize.Width / 2;
                            labelTop = canvasTop - columnVisualSize.Height + insideGap;

                            if (labelTop + tb.TextBlockDesiredSize.Height > (dataPoint.Chart as Chart).ChartArea.ChartVisualCanvas.Height - (dataPoint.Chart as Chart).ChartArea.PLANK_THICKNESS)
                                labelTop = (dataPoint.Chart as Chart).ChartArea.ChartVisualCanvas.Height - (dataPoint.Chart as Chart).ChartArea.PLANK_THICKNESS - tb.TextBlockDesiredSize.Height;

                            angle = 0;
                        }
                        else
                        {
                            labelLeft = canvasLeft + columnVisualSize.Width / 2 - tb.TextBlockDesiredSize.Width / 2;
                            labelTop = canvasTop - tb.TextBlockDesiredSize.Height - (((Double)dataPoint.MarkerSize / 2) * (Double)dataPoint.MarkerScale - insideGap);
                            angle = 0;
                        }
                    }
                }
                else
                {
                    if (isVertical)
                    {
                        centerOfRotation = new Point(canvasLeft + columnVisualSize.Width / 2, canvasTop + (((Double)dataPoint.MarkerSize / 2) * (Double)dataPoint.MarkerScale + outsideGap + 2));
                        angle = -90 + 180;
                        angleInRadian = (Math.PI / 180) * angle;
                        radius += tb.Width;
                        angle = (angleInRadian - Math.PI) * (180 / Math.PI);

                        labelLeft = centerOfRotation.X + radius * Math.Cos(angleInRadian);
                        labelTop = centerOfRotation.Y + radius * Math.Sin(angleInRadian);
                    }
                    else
                    {
                        if (dataPoint.Parent.RenderAs == RenderAs.StackedColumn100 && isView3D
                        && isTopOfStack)
                        {
                            if (!dataPoint.IsLabelStyleSet && !dataPoint.Parent.IsLabelStyleSet && !isVertical && tb.TextBlockDesiredSize.Height >= columnVisualSize.Height)
                            {
                                labelLeft = canvasLeft + columnVisualSize.Width / 2 - tb.TextBlockDesiredSize.Width / 2;
                                labelTop = canvasTop - outsideGap;
                            }
                            else
                            {
                                labelLeft = canvasLeft + columnVisualSize.Width / 2 - tb.TextBlockDesiredSize.Width / 2;
                                labelTop = canvasTop + (((Double)dataPoint.MarkerSize / 2) * (Double)dataPoint.MarkerScale + outsideGap + 3);
                            }
                        }
                        else
                        {
                            labelLeft = canvasLeft + columnVisualSize.Width / 2 - tb.TextBlockDesiredSize.Width / 2;
                            labelTop = canvasTop + (((Double)dataPoint.MarkerSize / 2) * (Double)dataPoint.MarkerScale + outsideGap + 3);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Returns label for DataPoint
        /// </summary>
        /// <param name="chart"></param>
        /// <param name="columnParams"></param>
        /// <param name="dataPoint"></param>
        /// <param name="canvasLeft"></param>
        /// <param name="canvasTop"></param>
        /// <returns></returns>
        private static void CreateLabel(Chart chart, Size columnVisualSize, Boolean isPositive, Boolean isTopOfStack, DataPoint dataPoint, Double canvasLeft, Double canvasTop, ref Canvas labelCanvas)
        {
            if (dataPoint.Faces == null)
                return;          

            if ((Boolean)dataPoint.LabelEnabled && !String.IsNullOrEmpty(dataPoint.LabelText))
            {
                LabelStyles autoLabelStyle = (LabelStyles)dataPoint.LabelStyle;

                if (isPositive || dataPoint.YValue == 0)
                    isPositive = true;

                // Calculate proper position for Canvas top
                if (isPositive)
                    canvasTop -= 6;
                else
                    canvasTop -= 8;

                Double angle = 0;

                Title tb = new Title()
                {
                    Text = dataPoint.TextParser(dataPoint.LabelText),
                    InternalFontFamily = dataPoint.LabelFontFamily,
                    InternalFontSize = dataPoint.LabelFontSize.Value,
                    InternalFontWeight = (FontWeight)dataPoint.LabelFontWeight,
                    InternalFontStyle = (FontStyle)dataPoint.LabelFontStyle,
                    InternalBackground = dataPoint.LabelBackground,
                    InternalFontColor = Chart.CalculateDataPointLabelFontColor(dataPoint.Chart as Chart, dataPoint, dataPoint.LabelFontColor, autoLabelStyle),
                    Padding = new Thickness(0.1, 0.1, 0.1, 0.1),
                    Tag = new ElementData() { Element = dataPoint }
                };

                tb.CreateVisualObject(new ElementData() { Element = dataPoint });

                Double labelLeft = 0;
                Double labelTop = 0;
                Double outsideGap = 4;
                Double insideGap = 6;

                if (Double.IsNaN(dataPoint.LabelAngle) || dataPoint.LabelAngle == 0)
                {
                    Boolean isVertical = false;
                    if (columnVisualSize.Width < tb.TextBlockDesiredSize.Width)
                    {
                        tb.Visual.RenderTransformOrigin = new Point(0, 0.5);
                        tb.Visual.RenderTransform = new RotateTransform()
                        {
                            CenterX = 0,
                            CenterY = 0,
                            Angle = -90
                        };

                        isVertical = true;
                    }
                    else
                        isVertical = false;

                    if (!dataPoint.IsLabelStyleSet && !dataPoint.Parent.IsLabelStyleSet && !isTopOfStack && dataPoint.Parent.RenderAs != RenderAs.Column)
                    {
                        autoLabelStyle = LabelStyles.Inside;
                    }

                    if (dataPoint.Parent.RenderAs == RenderAs.StackedColumn100 && chart.View3D
                        && isTopOfStack)
                    {
                        if (!dataPoint.IsLabelStyleSet && !dataPoint.Parent.IsLabelStyleSet && !isVertical && tb.TextBlockDesiredSize.Height >= columnVisualSize.Height)
                            autoLabelStyle = LabelStyles.OutSide;
                    }

                    CalculateAutoPlacement(chart.View3D, dataPoint, columnVisualSize, isPositive, autoLabelStyle, ref labelLeft, ref labelTop, ref angle,
                        canvasLeft, canvasTop, isVertical, insideGap, outsideGap, tb, isTopOfStack);

                    tb.Visual.SetValue(Canvas.LeftProperty, labelLeft);
                    tb.Visual.SetValue(Canvas.TopProperty, labelTop);

                    tb.Visual.RenderTransformOrigin = new Point(0, 0.5);
                    tb.Visual.RenderTransform = new RotateTransform()
                    {
                        CenterX = 0,
                        CenterY = 0,
                        Angle = angle
                    };
                    
                    Double depth3D = chart.ChartArea.PLANK_DEPTH / chart.PlotDetails.Layer3DCount * (chart.View3D ? 1 : 0);

                    if (!dataPoint.IsLabelStyleSet && !dataPoint.Parent.IsLabelStyleSet)
                    {
                        if (isPositive)
                        {   
                            if (isVertical)
                            {
                                if (labelTop + outsideGap - tb.TextBlockDesiredSize.Width < -depth3D)
                                    autoLabelStyle = LabelStyles.Inside;
                            }
                            else
                            {
                                if (labelTop < -depth3D)
                                    autoLabelStyle = LabelStyles.Inside;
                            }
                        }
                        else
                        {
                            if (isVertical)
                            {
                                if (labelTop + outsideGap + 2 > chart.PlotArea.BorderElement.Height - depth3D + chart.ChartArea.PLANK_THICKNESS)
                                    autoLabelStyle = LabelStyles.Inside;
                            }
                            else
                            {
                                if (labelTop + tb.TextBlockDesiredSize.Height > chart.PlotArea.BorderElement.Height - depth3D + chart.ChartArea.PLANK_THICKNESS)
                                    autoLabelStyle = LabelStyles.Inside;
                            }
                        }
                    }

                    if (autoLabelStyle != dataPoint.LabelStyle)
                    {
                        CalculateAutoPlacement(chart.View3D, dataPoint, columnVisualSize, isPositive, autoLabelStyle, ref labelLeft, ref labelTop, ref angle,
                        canvasLeft, canvasTop, isVertical, insideGap, outsideGap, tb, isTopOfStack);

                        tb.Visual.SetValue(Canvas.LeftProperty, labelLeft);
                        tb.Visual.SetValue(Canvas.TopProperty, labelTop);

                        tb.Visual.RenderTransformOrigin = new Point(0, 0.5);
                        tb.Visual.RenderTransform = new RotateTransform()
                        {
                            CenterX = 0,
                            CenterY = 0,
                            Angle = angle
                        };
                    }
                }
                else
                {
                    if (isPositive)
                    {
                        Point centerOfRotation = new Point(canvasLeft + columnVisualSize.Width / 2,
                            canvasTop - (((Double)dataPoint.MarkerSize / 2) * (Double)dataPoint.MarkerScale));

                        Double radius = 0;
                        angle = 0;
                        Double angleInRadian = 0;

                        if (autoLabelStyle == LabelStyles.OutSide)
                        {
                            if (dataPoint.LabelAngle > 0 && dataPoint.LabelAngle <= 90)
                            {
                                angle = dataPoint.LabelAngle - 180;
                                angleInRadian = (Math.PI / 180) * angle;
                                radius += tb.TextBlockDesiredSize.Width;
                                angle = (angleInRadian - Math.PI) * (180 / Math.PI);
                                SetRotation(radius, angle, angleInRadian, centerOfRotation, labelLeft, labelTop, tb);
                            }
                            else if (dataPoint.LabelAngle >= -90 && dataPoint.LabelAngle < 0)
                            {
                                angle = dataPoint.LabelAngle;
                                angleInRadian = (Math.PI / 180) * angle;
                                SetRotation(radius, angle, angleInRadian, centerOfRotation, labelLeft, labelTop, tb);
                            }
                        }
                        else
                        {
                            centerOfRotation = new Point(canvasLeft + columnVisualSize.Width / 2,
                            canvasTop + (((Double)dataPoint.MarkerSize / 2) * (Double)dataPoint.MarkerScale));
                            radius = 4;
                            if (dataPoint.LabelAngle >= -90 && dataPoint.LabelAngle < 0)
                            {
                                angle = 180 + dataPoint.LabelAngle;
                                angleInRadian = (Math.PI / 180) * angle;
                                radius += tb.TextBlockDesiredSize.Width + 5;
                                angle = (angleInRadian - Math.PI) * (180 / Math.PI);
                                SetRotation(radius, angle, angleInRadian, centerOfRotation, labelLeft, labelTop, tb);
                            }
                            else if (dataPoint.LabelAngle > 0 && dataPoint.LabelAngle <= 90)
                            {
                                radius += 5;
                                angle = dataPoint.LabelAngle;
                                angleInRadian = (Math.PI / 180) * angle;
                                SetRotation(radius, angle, angleInRadian, centerOfRotation, labelLeft, labelTop, tb);
                            }
                        }
                    }
                    else
                    {
                        Point centerOfRotation = new Point();

                        Double radius = 0;
                        angle = 0;
                        Double angleInRadian = 0;

                        if (autoLabelStyle == LabelStyles.OutSide)
                        {
                            centerOfRotation = new Point(canvasLeft + columnVisualSize.Width / 2,
                            canvasTop + (((Double)dataPoint.MarkerSize / 2) * (Double)dataPoint.MarkerScale) + 10);

                            radius = 4;

                            if (dataPoint.LabelAngle >= -90 && dataPoint.LabelAngle < 0)
                            {
                                angle = 180 + dataPoint.LabelAngle;
                                angleInRadian = (Math.PI / 180) * angle;
                                radius += tb.TextBlockDesiredSize.Width;
                                angle = (angleInRadian - Math.PI) * (180 / Math.PI);
                                SetRotation(radius, angle, angleInRadian, centerOfRotation, labelLeft, labelTop, tb);
                            }
                            else if (dataPoint.LabelAngle > 0 && dataPoint.LabelAngle <= 90)
                            {
                                angle = dataPoint.LabelAngle;
                                angleInRadian = (Math.PI / 180) * angle;
                                SetRotation(radius, angle, angleInRadian, centerOfRotation, labelLeft, labelTop, tb);
                            }
                        }
                        else
                        {
                            centerOfRotation = new Point(canvasLeft + columnVisualSize.Width / 2,
                            canvasTop - (((Double)dataPoint.MarkerSize / 2) * (Double)dataPoint.MarkerScale));

                            if (dataPoint.LabelAngle > 0 && dataPoint.LabelAngle <= 90)
                            {
                                angle = dataPoint.LabelAngle - 180;
                                angleInRadian = (Math.PI / 180) * angle;
                                radius += tb.TextBlockDesiredSize.Width;
                                angle = (angleInRadian - Math.PI) * (180 / Math.PI);
                                SetRotation(radius, angle, angleInRadian, centerOfRotation, labelLeft, labelTop, tb);
                            }
                            else if (dataPoint.LabelAngle >= -90 && dataPoint.LabelAngle < 0)
                            {
                                //radius += 3;  
                                angle = dataPoint.LabelAngle;
                                angleInRadian = (Math.PI / 180) * angle;
                                SetRotation(radius, angle, angleInRadian, centerOfRotation, labelLeft, labelTop, tb);
                            }
                        }
                    }
                }

                if (autoLabelStyle != dataPoint.LabelStyle)
                {
                    tb.TextElement.Foreground = Chart.CalculateDataPointLabelFontColor(dataPoint.Chart as Chart, dataPoint, dataPoint.LabelFontColor, (dataPoint.YValue == 0 ? LabelStyles.OutSide : autoLabelStyle));
                }

                dataPoint.LabelVisual = tb.Visual;
                labelCanvas.Children.Add(tb.Visual);

            }
        }

        /// <summary>
        /// Set rotation angle for DataPoint label if LabelAngle property is set
        /// </summary>
        /// <param name="radius"></param>
        /// <param name="angle"></param>
        /// <param name="angleInRadian"></param>
        /// <param name="centerOfRotation"></param>
        /// <param name="labelLeft"></param>
        /// <param name="labelTop"></param>
        /// <param name="textBlock"></param>
        internal static void SetRotation(Double radius, Double angle, Double angleInRadian, Point centerOfRotation,
            Double labelLeft, Double labelTop, Title textBlock)
        {
            labelLeft = centerOfRotation.X + radius * Math.Cos(angleInRadian);
            labelTop = centerOfRotation.Y + radius * Math.Sin(angleInRadian);

            labelTop -= textBlock.TextBlockDesiredSize.Height / 2;

            textBlock.Visual.SetValue(Canvas.LeftProperty, labelLeft);
            textBlock.Visual.SetValue(Canvas.TopProperty, labelTop);

            textBlock.Visual.RenderTransformOrigin = new Point(0, 0.5);
            textBlock.Visual.RenderTransform = new RotateTransform()
            {
                CenterX = 0,
                CenterY = 0,
                Angle = angle
            };
        }

        /// <summary>
        /// Set position of the marker
        /// </summary>
        /// <param name="columnParams">Column parameters</param>
        /// <param name="chart">Chart</param>
        /// <param name="dataPoint">DataPoint</param>
        /// <param name="labelText">label text</param>
        /// <param name="markerSize">Size of the marker</param>
        /// <param name="canvasLeft">Left position of marker canvas</param>
        /// <param name="canvasTop">Top position of marker canvas</param>
        /// <param name="markerPosition">Position of the Marker</param>
        private static void SetMarkerPosition(Size columnVisualSize, Chart chart, DataPoint dataPoint, String labelText, Size markerSize, Double canvasLeft, Double canvasTop, Point markerPosition)
        {
            Marker marker = dataPoint.Marker;
            Boolean isPositive = (dataPoint.InternalYValue >= 0);

            if ((Boolean)dataPoint.LabelEnabled && !String.IsNullOrEmpty(labelText))
            {
                marker.CreateVisual();

                if (columnVisualSize.Width < marker.TextBlockSize.Width)
                    marker.TextOrientation = Orientation.Vertical;
                else
                    marker.TextOrientation = Orientation.Horizontal;

                LabelStyles labelStyle = LabelStyles.OutSide;

                if (isPositive)
                {   
                    if (marker.TextOrientation == Orientation.Vertical)
                    {   
                        if (canvasTop - marker.MarkerActualSize.Width - marker.MarkerSize.Height < 0)
                            labelStyle = LabelStyles.Inside;
                    }
                    else
                    {   
                        if (canvasTop - marker.MarkerActualSize.Height - marker.MarkerSize.Height < 0)
                            labelStyle = LabelStyles.Inside;
                    }
                }
                else
                {   
                    if (marker.TextOrientation == Orientation.Vertical)
                    {   
                        if (canvasTop + markerPosition.Y + marker.MarkerActualSize.Width + marker.MarkerSize.Height > chart.PlotArea.BorderElement.Height + chart.ChartArea.PLANK_DEPTH - chart.ChartArea.PLANK_THICKNESS)
                            labelStyle = LabelStyles.Inside;
                    }
                    else
                    {   
                        if (canvasTop + markerPosition.Y + marker.MarkerActualSize.Height + marker.MarkerSize.Height > chart.PlotArea.BorderElement.Height + chart.ChartArea.PLANK_DEPTH - chart.ChartArea.PLANK_THICKNESS)
                            labelStyle = LabelStyles.Inside;
                    }
                }

                marker.TextAlignmentX = AlignmentX.Center;

                if (!(Boolean)dataPoint.MarkerEnabled)
                {
                    if (chart.View3D)
                    {
                        if (labelStyle == LabelStyles.OutSide)
                        {   
                            if (isPositive)
                                marker.MarkerSize = new Size(markerSize.Width / 2 + chart.ChartArea.PLANK_DEPTH + chart.ChartArea.PLANK_THICKNESS, markerSize.Height / 2 + chart.ChartArea.PLANK_DEPTH + chart.ChartArea.PLANK_THICKNESS);
                            else
                                marker.MarkerSize = new Size(markerSize.Width / 2, markerSize.Height / 2);
                        }
                        else
                            marker.MarkerSize = new Size(markerSize.Width / 2, markerSize.Height / 2);
                    }
                }
                else
                {
                    if (chart.View3D)
                    {
                        labelStyle = LabelStyles.Inside;
                    }
                }

                if (isPositive)
                    marker.TextAlignmentY = labelStyle == LabelStyles.Inside ? AlignmentY.Bottom : AlignmentY.Top;
                else
                    marker.TextAlignmentY = labelStyle == LabelStyles.Inside ? AlignmentY.Top : AlignmentY.Bottom;
            }
        }

        /// <summary>
        /// Returns marker for DataPoint
        /// </summary>
        /// <param name="chart">Chart</param>
        /// <param name="columnParams">Column parameters</param>
        /// <param name="dataPoint">DataPoint</param>
        /// <param name="left">Left position of MarkerCanvas</param>
        /// <param name="top">Top position</param>
        /// <returns>Marker canvas</returns>
        private static Marker GetMarker(Size columnVisualSize, Chart chart, DataPoint dataPoint, Double left, Double top)
        {
            if ((Boolean)dataPoint.MarkerEnabled)
            {
                Size markerSize = new Size((Double)dataPoint.MarkerSize, (Double)dataPoint.MarkerSize);
                String labelText = "";// (Boolean)dataPoint.LabelEnabled ? dataPoint.TextParser(dataPoint.LabelText) : "";

                dataPoint.Marker = CreateNewMarker(dataPoint, markerSize, labelText);

                if (!(Boolean)dataPoint.MarkerEnabled)
                {
                    dataPoint.Marker.MarkerFillColor = Graphics.TRANSPARENT_BRUSH;
                    dataPoint.Marker.BorderColor = Graphics.TRANSPARENT_BRUSH;
                }

                Point markerPosition = new Point();

                if (dataPoint.InternalYValue >= 0)
                    if (chart.View3D)
                        markerPosition = new Point(columnVisualSize.Width / 2, 0);
                    else
                        markerPosition = new Point(columnVisualSize.Width / 2, 0);
                else
                    if (chart.View3D)
                        markerPosition = new Point(columnVisualSize.Width / 2, columnVisualSize.Height);
                    else
                        markerPosition = new Point(columnVisualSize.Width / 2, columnVisualSize.Height);

                //SetMarkerPosition(columnVisualSize, chart, dataPoint, labelText, markerSize, left, top, markerPosition);

                //dataPoint.Marker.FontColor = Chart.CalculateDataPointLabelFontColor(chart, dataPoint, dataPoint.LabelFontColor, (dataPoint.YValue == 0)? LabelStyles.OutSide:(LabelStyles)dataPoint.LabelStyle);

                dataPoint.Marker.Tag = new ElementData() { Element = dataPoint };
                dataPoint.Marker.CreateVisual();

                return dataPoint.Marker;
            }

            return null;
        }

        /// <summary>
        /// Calculate width of each column
        /// </summary>
        /// <param name="left">Left position</param>
        /// <param name="widthPerColumn">Width of a column</param>
        /// <param name="width">Width of chart canvas</param>
        /// <returns>Final width of DataPoint</returns>
        internal static Double CalculateWidthOfEachColumn(Chart chart, Double width, Axis axisX, RenderAs chartType, Orientation direction)
        {
            PlotDetails plotDetails = chart.PlotDetails;

            Double minDiffValue = plotDetails.GetMinOfMinDifferencesForXValue(chartType, RenderAs.StackedColumn, RenderAs.StackedColumn100);

            if (Double.IsPositiveInfinity(minDiffValue))
                minDiffValue = 0;

            Axis axisXwithMinInterval = axisX;
            Double dataAxisDifference = width;
            Double maxColumnWidth = dataAxisDifference * (1 - COLUMN_GAP_RATIO);
            Double numberOfDivisions = plotDetails.DrawingDivisionFactor;
            Double heightOrwidthPerColumn;

            if (minDiffValue == 0)
            {
                heightOrwidthPerColumn = width * .5 / numberOfDivisions;
            }
            else
            {
                heightOrwidthPerColumn = Graphics.ValueToPixelPosition(0, width, (Double)axisXwithMinInterval.InternalAxisMinimum, (Double)axisXwithMinInterval.InternalAxisMaximum, minDiffValue + (Double)axisXwithMinInterval.InternalAxisMinimum);
                heightOrwidthPerColumn *= (1 - ((direction == Orientation.Horizontal) ? COLUMN_GAP_RATIO : BarChart.BAR_GAP_RATIO));
                heightOrwidthPerColumn /= numberOfDivisions;
            }

            if (!Double.IsNaN(chart.DataPointWidth))
            {
                if (chart.DataPointWidth >= 0)
                    heightOrwidthPerColumn = chart.DataPointWidth / 100 * ((direction == Orientation.Horizontal) ? chart.PlotArea.Width : chart.PlotArea.Height);
            }

            Double finalWidth = heightOrwidthPerColumn;

            return (finalWidth < 2) ? 2 : finalWidth;
        }

        /// <summary>
        /// Get columns Z-Index
        /// </summary>
        /// <param name="left">Left position</param>
        /// <param name="top">Top position</param>
        /// <param name="isPositive">Whether DataPoint value is positive or negative</param>
        /// <returns>Zindex as Int32</returns>
        private static Int32 GetColumnZIndex(Double left, Double top, Boolean isPositive)
        {
            Int32 Zi = 0;
            Int32 ioffset = (Int32)left;

            if (ioffset == 0)
                ioffset++;

            Zi = (isPositive) ? Zi + (Int32)(ioffset) : Zi + Int32.MinValue + (Int32)(ioffset);

            return Zi;
        }

        /// <summary>
        /// Get ZIndex for StackedColumn visual
        /// </summary>
        /// <param name="left">Left position</param>
        /// <param name="top">Top position</param>
        /// <param name="isPositive">Whether column value is positive or negative</param>
        /// <param name="index">Index</param>
        /// <returns>Zindex as Int32</returns>
        internal static Int32 GetStackedColumnZIndex(Double left, Double top, Boolean isPositive, Int32 index)
        {   
            Int32 ioffset = (Int32) left;
            Int32 topOffset = (Int32) index;

            Int32 zindex = 0;

            if (isPositive)
                zindex = (Int32)(ioffset + topOffset);
            else
            {   
                if (ioffset == 0)
                    ioffset = 1;

                zindex = Int32.MinValue + (Int32)(ioffset + topOffset);
            }

            return zindex;
        }

        /// <summary>
        /// Apply column chart animation
        /// </summary>
        /// <param name="column">Column visual reference</param>
        /// <param name="storyboard">Storyboard</param>
        /// <param name="columnParams">Column parameters</param>
        /// <returns>Storyboard</returns>
        private static Storyboard ApplyColumnChartAnimation( DataSeries currentDataSeries, Panel column, Storyboard storyboard, Boolean isPositive, Double beginTime, Double[] timeCollection, Double[] valueCollection, RenderAs renderAs)
        {   
            ScaleTransform scaleTransform;
            
            String property2Animate;

            // storyboard.Pause();

            if(renderAs == RenderAs.Column)
            {
                property2Animate ="(ScaleTransform.ScaleY)";
                scaleTransform = new ScaleTransform() { ScaleY = valueCollection[0] };
                column.RenderTransformOrigin = (isPositive) ? new Point(0.5, 1) : new Point(0.5, 0);
            }
            else
            {   
                scaleTransform = new ScaleTransform() { ScaleX = valueCollection[0] };
                property2Animate=  "(ScaleTransform.ScaleX)";
                column.RenderTransformOrigin = (isPositive) ? new Point(0, 0.5) : new Point(1, 0.5);
            }

            column.RenderTransform = scaleTransform;

            DoubleCollection values = Graphics.GenerateDoubleCollection(valueCollection);
            DoubleCollection frameTimes = Graphics.GenerateDoubleCollection(timeCollection);

            List<KeySpline> splines = null;
            
            if(valueCollection.Length == 2)
                splines = AnimationHelper.GenerateKeySplineList
                (
                new Point(0, 0), new Point(1, 1),
                new Point(0, 1), new Point(0.5, 1)
                );

            DoubleAnimationUsingKeyFrames growAnimation = AnimationHelper.CreateDoubleAnimation(currentDataSeries, scaleTransform, property2Animate, beginTime, frameTimes, values, splines);

            storyboard.Children.Add(growAnimation);

            return storyboard;
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Set column parameters
        /// </summary>
        /// <param name="columnParams">Column parameters</param>
        /// <param name="chart">Chart reference</param>
        /// <param name="dataPoint">DataPoint</param>
        /// <param name="IsPositive">Whether the DataPoint YValue is positive or negative</param>
        internal static void SetColumnParms(ref RectangularChartShapeParams columnParams, ref Chart chart, DataPoint dataPoint, Boolean isPositive)
        {
            columnParams.Bevel = dataPoint.Parent.Bevel;
            columnParams.Lighting = (Boolean)dataPoint.LightingEnabled;
            columnParams.Shadow = (Boolean)dataPoint.ShadowEnabled;
            columnParams.BorderBrush = dataPoint.BorderColor;
            columnParams.BorderThickness = ((Thickness)dataPoint.BorderThickness).Left;
            columnParams.BorderStyle = ExtendedGraphics.GetDashArray((BorderStyles)dataPoint.BorderStyle);
            columnParams.IsPositive = isPositive;
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
            columnParams.LabelFontColor = dataPoint.LabelFontColor;
            columnParams.LabelFontSize = (Double)dataPoint.LabelFontSize;
            columnParams.LabelFontFamily = dataPoint.LabelFontFamily;
            columnParams.LabelFontStyle = (FontStyle)dataPoint.LabelFontStyle;
            columnParams.LabelFontWeight = (FontWeight)dataPoint.LabelFontWeight;

            columnParams.TagReference = dataPoint;
        }
        
        /// <summary>
        /// Create new Marker
        /// </summary>
        /// <param name="columnParams">Column parameters</param>
        /// <param name="dataPoint">DataPoint</param>
        /// <param name="markerSize">Marker size</param>
        /// <param name="labelText">Label text</param>
        /// <returns>Marker</returns>
        internal static Marker CreateNewMarker(DataPoint dataPoint, Size markerSize, String labelText)
        {   
            Boolean markerBevel = false;

            Marker marker = new Marker((MarkerTypes)dataPoint.MarkerType, (Double) dataPoint.MarkerScale, markerSize, markerBevel, dataPoint.MarkerColor, labelText);

            marker.MarkerSize = markerSize;
            marker.BorderColor = dataPoint.MarkerBorderColor;
            marker.BorderThickness = dataPoint.MarkerBorderThickness.Value.Left;
            marker.MarkerType = (MarkerTypes)dataPoint.MarkerType;
            marker.FontColor = dataPoint.LabelFontColor;
            marker.FontFamily = dataPoint.LabelFontFamily;
            marker.FontSize = (Double)dataPoint.LabelFontSize;
            marker.FontStyle = (FontStyle)dataPoint.LabelFontStyle;
            marker.FontWeight = (FontWeight)dataPoint.LabelFontWeight;
            marker.TextBackground = dataPoint.LabelBackground;

            return marker;
        }

        /// <summary>
        /// Get a dictionary of related DataSeries list with a particular axis, where axis works as key.
        /// </summary>
        /// <param name="seriesList">DataSeries List</param>
        /// <returns>Dictionary[Axis, Dictionary[Axis, Int32]]</returns>
        internal static Dictionary<Axis, Dictionary<Axis, Int32>> GetSeriesIndex(List<DataSeries> seriesList)
        {   
            Dictionary<Axis, Dictionary<Axis, Int32>> seriesIndex = new Dictionary<Axis, Dictionary<Axis, Int32>>();

            var seriesByAxis = (from series in seriesList where series.Enabled == true
                                group series by new
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

        private static void CreateColumnDataPointVisual( Canvas parentCanvas, Canvas labelCanvas, PlotDetails plotDetails, DataPoint dataPoint, Boolean isPositive, Double widthOfAcolumn, Double depth3D, Boolean animationEnabled)
        {   
            if (widthOfAcolumn < 0)
                return;
            DataSeries currentDataSeries = dataPoint.Parent;

            Chart chart = dataPoint.Chart as Chart;

            dataPoint.Parent.Faces = new Faces { Visual = parentCanvas, LabelCanvas = labelCanvas };
            
            Double left, bottom, top, columnHeight;
            Size columnVisualSize;

            PlotGroup plotGroup = dataPoint.Parent.PlotGroup;

            Double limitingYValue = 0;

            if (plotGroup.AxisY.InternalAxisMinimum > 0)
                limitingYValue = (Double)plotGroup.AxisY.InternalAxisMinimum;
            if (plotGroup.AxisY.InternalAxisMaximum < 0)
                limitingYValue = (Double)plotGroup.AxisY.InternalAxisMaximum;

            List<DataSeries> indexSeriesList = plotDetails.GetSeriesFromDataPoint(dataPoint);
            Int32 drawingIndex = indexSeriesList.IndexOf(dataPoint.Parent);

            // if (dataPoint.InternalYValue > (Double)plotGroup.AxisY.InternalAxisMaximum)
            //     System.Diagnostics.Debug.WriteLine("Max Value greater then axis max");

            left = Graphics.ValueToPixelPosition(0, parentCanvas.Width, (Double)plotGroup.AxisX.InternalAxisMinimum, (Double)plotGroup.AxisX.InternalAxisMaximum, dataPoint.InternalXValue);
            left = left + ((Double)drawingIndex - (Double)indexSeriesList.Count() / (Double)2) * widthOfAcolumn;

//            Double midPosition = Graphics.ValueToPixelPosition(0, parentCanvas.Width, (Double)plotGroup.AxisX.InternalAxisMinimum, (Double)plotGroup.AxisX.InternalAxisMaximum, dataPoint.InternalXValue);

            if (isPositive)
            {   
                bottom = Graphics.ValueToPixelPosition(parentCanvas.Height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, limitingYValue);
                top = Graphics.ValueToPixelPosition(parentCanvas.Height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, Double.IsNaN(dataPoint.InternalYValue) ? 0 : dataPoint.InternalYValue);
            }
            else
            {
                bottom = Graphics.ValueToPixelPosition(parentCanvas.Height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, Double.IsNaN(dataPoint.InternalYValue) ? 0 : dataPoint.InternalYValue);
                top = Graphics.ValueToPixelPosition(parentCanvas.Height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, limitingYValue);
            }

            columnHeight = Math.Abs(top - bottom);

            columnVisualSize = new Size(widthOfAcolumn, columnHeight);

            Faces columnFaces = null;
            Panel columnVisual = null;

            if (chart.View3D)
            {
                columnFaces = Get3DColumn(dataPoint, widthOfAcolumn, columnHeight, depth3D, dataPoint.Color, null, null, null, (Boolean)dataPoint.LightingEnabled,
                    (BorderStyles)dataPoint.BorderStyle, dataPoint.BorderColor, dataPoint.BorderThickness.Left);
                columnVisual = columnFaces.Visual as Panel;
                columnVisual.SetValue(Canvas.ZIndexProperty, GetColumnZIndex(left, top, (dataPoint.InternalYValue > 0)));
                
                dataPoint.Faces = columnFaces;
                ColumnChart.ApplyOrRemoveShadow(dataPoint, false, false);
            }   
            else
            {   
                columnFaces = Get2DColumn(dataPoint, widthOfAcolumn, columnHeight, false, false);
                columnVisual = columnFaces.Visual as Panel;
            }

            dataPoint.Faces = columnFaces;
            
            columnVisual.SetValue(Canvas.LeftProperty, left);
            columnVisual.SetValue(Canvas.TopProperty, top);

            parentCanvas.Children.Add(columnVisual);

            dataPoint.IsTopOfStack = true;

            CreateOrUpdateMarker4VerticalChart(dataPoint, labelCanvas, columnVisualSize, left, top);

            if(isPositive)
                dataPoint._visualPosition = new Point(left + columnVisualSize.Width / 2, top);
            else
                dataPoint._visualPosition = new Point(left + columnVisualSize.Width / 2, bottom);

            // Apply animation
            if (animationEnabled)
            {
                if (dataPoint.Parent.Storyboard == null)
                    dataPoint.Parent.Storyboard = new Storyboard();

                currentDataSeries = dataPoint.Parent;
                //dataPoint.Parent.Storyboard.Stop();

                // Apply animation to the data points dataSeriesIndex.e to the rectangles that form the columns
                dataPoint.Parent.Storyboard = ApplyColumnChartAnimation(currentDataSeries, columnVisual, dataPoint.Parent.Storyboard, isPositive, 1, new Double[] { 0, 1 }, new Double[] { 0, 1 }, dataPoint.Parent.RenderAs);

            }

            dataPoint.Faces.Visual.Opacity = dataPoint.Opacity * dataPoint.Parent.Opacity;
            dataPoint.AttachEvent2DataPointVisualFaces(dataPoint);
            dataPoint.AttachEvent2DataPointVisualFaces(dataPoint.Parent);
            dataPoint._parsedToolTipText = dataPoint.TextParser(dataPoint.ToolTipText);
            if(!chart.IndicatorEnabled)
                dataPoint.AttachToolTip(chart, dataPoint, dataPoint.Faces.Visual);
            //dataPoint.AttachToolTip(chart, dataPoint, dataPoint.Faces.VisualComponents);
            dataPoint.AttachHref(chart, dataPoint.Faces.Visual, dataPoint.Href, (HrefTargets)dataPoint.HrefTarget);
            //dataPoint.AttachHref(chart, dataPoint.Faces.VisualComponents, dataPoint.Href, (HrefTargets)dataPoint.HrefTarget);
            dataPoint.SetCursor2DataPointVisualFaces();
            chart._toolTip.Hide();
        }

        internal static void CleanUpMarkerAndLabel(DataPoint dataPoint, Canvas labelCanvas)
        {
            if (dataPoint.Marker != null && dataPoint.Marker.Visual != null)
            {   
                labelCanvas.Children.Remove(dataPoint.Marker.Visual);
                dataPoint.Marker.Visual = null;
            }

            if (dataPoint.LabelVisual != null)
            {
                labelCanvas.Children.Remove(dataPoint.LabelVisual);
                dataPoint.LabelVisual = null;
            }
        }

        private static void CreateOrUpdateMarker4VerticalChart(DataPoint dataPoint, Canvas labelCanvas, Size columnVisualSize, Double left, Double top)
        {   
            # region Create marker
            if (dataPoint.Faces == null)
                return;

            CleanUpMarkerAndLabel(dataPoint, labelCanvas);

            Chart chart = dataPoint.Chart as Chart;

            if ((Boolean)dataPoint.MarkerEnabled)
            {   
                Point markerPosition = new Point();

                if (dataPoint.InternalYValue >= 0)
                    markerPosition = (chart.View3D) ? new Point(columnVisualSize.Width / 2, 0)
                        : new Point(columnVisualSize.Width / 2, 0);
                else
                    markerPosition = (chart.View3D) ? new Point(columnVisualSize.Width / 2, columnVisualSize.Height)
                        : new Point(columnVisualSize.Width / 2, columnVisualSize.Height);

                Marker marker = GetMarker(columnVisualSize, chart, dataPoint, left, top);

                if (marker != null)
                {
                    dataPoint.Marker.Visual.Opacity = dataPoint.InternalOpacity * dataPoint.Parent.InternalOpacity;

                    marker.AddToParent(labelCanvas, left + markerPosition.X, top + markerPosition.Y, new Point(0.5, 0.5));
                }

                if (marker != null && marker.Visual != null)
                    dataPoint.AttachToolTip(chart, dataPoint, marker.Visual);

                dataPoint.Marker = marker;
            }

            if ((Boolean)dataPoint.LabelEnabled)
            {
                Boolean isPositive = false;
                if (!Double.IsNaN(dataPoint.YValue) && dataPoint.YValue > 0)
                    isPositive = true;

                Double bottom = top + columnVisualSize.Height;

                if (isPositive)
                    CreateLabel(chart, columnVisualSize, isPositive, dataPoint.IsTopOfStack, dataPoint, left, top, ref labelCanvas);
                else
                    CreateLabel(chart, columnVisualSize, isPositive, dataPoint.IsTopOfStack, dataPoint, left, bottom, ref labelCanvas);

                if (dataPoint.LabelVisual != null)
                    dataPoint.AttachToolTip(chart, dataPoint, dataPoint.LabelVisual);
            }
            
            #endregion
        }
        
        /// <summary>
        /// Get visual object for column chart
        /// </summary>
        /// <param name="width">Width of the PlotArea</param>
        /// <param name="height">Height of the PlotArea</param>
        /// <param name="plotDetails">PlotDetails</param>
        /// <param name="dataSeriesList4Rendering">DataSeriesList with render as Column chart</param>
        /// <param name="chart">Chart</param>
        /// <param name="plankDepth">PlankDepth</param>
        /// <param name="animationEnabled">Whether animation is enabled for chart</param>
        /// <returns>Column chart canvas</returns>
        internal static Canvas GetVisualObjectForColumnChart(Panel preExistingPanel, Double width, Double height, PlotDetails plotDetails, List<DataSeries> dataSeriesList4Rendering, Chart chart, Double plankDepth, bool animationEnabled)
        {   
            if (Double.IsNaN(width) || Double.IsNaN(height) || width <= 0 || height <= 0)
                return null;

            DataSeries currentDataSeries = null;

            Canvas visual, labelCanvas, columnCanvas;
            
            RenderHelper.RepareCanvas4Drawing(preExistingPanel as Canvas, out visual, out labelCanvas, out columnCanvas, width, height);
            
            Double depth3d = plankDepth / plotDetails.Layer3DCount * (chart.View3D ? 1 : 0);
            Double visualOffset = depth3d * (plotDetails.SeriesDrawingIndex[dataSeriesList4Rendering[0]] + 1);

            visual.SetValue(Canvas.TopProperty, visualOffset);
            visual.SetValue(Canvas.LeftProperty, -visualOffset);

            Double widthOfAcolumn;

            if(plotDetails.ChartOrientation == ChartOrientationType.Vertical)
                widthOfAcolumn = CalculateWidthOfEachColumn(chart, width, dataSeriesList4Rendering[0].PlotGroup.AxisX, RenderAs.Column, Orientation.Horizontal);
            else
                widthOfAcolumn = CalculateWidthOfEachColumn(chart, height, dataSeriesList4Rendering[0].PlotGroup.AxisX, RenderAs.Bar, Orientation.Vertical);



            Dictionary<Double, SortDataPoints> sortedDataPoints = plotDetails.GetDataPointsGroupedByXValue(plotDetails.ChartOrientation == ChartOrientationType.Vertical ? RenderAs.Column : RenderAs.Bar);
            Double[] xValues = sortedDataPoints.Keys.ToArray();

            foreach (Double xValue in xValues)
            {   
                List<DataPoint> positive = sortedDataPoints[xValue].Positive;
                List<DataPoint> negative = sortedDataPoints[xValue].Negative;

                foreach (DataPoint dataPoint in positive)
                {
                    dataPoint.Parent.Faces = new Faces() { Visual = columnCanvas, LabelCanvas = labelCanvas };
                    dataPoint.Faces = null;

                    if (!(Boolean)dataPoint.Enabled)
                    {
                        continue;
                    }

                    currentDataSeries = dataPoint.Parent;

                    if(plotDetails.ChartOrientation == ChartOrientationType.Vertical)
                        CreateColumnDataPointVisual(columnCanvas, labelCanvas, plotDetails, dataPoint, true, widthOfAcolumn, depth3d, animationEnabled);
                    else
                        BarChart.CreateBarDataPointVisual(dataPoint, labelCanvas, columnCanvas, true, widthOfAcolumn, depth3d, animationEnabled);
                }

                foreach (DataPoint dataPoint in negative)
                {
                    dataPoint.Parent.Faces = new Faces() { Visual = columnCanvas, LabelCanvas = labelCanvas };
                    dataPoint.Faces = null;

                    if (!(Boolean)dataPoint.Enabled)
                    {
                        continue;
                    }

                    currentDataSeries = dataPoint.Parent;

                    if (plotDetails.ChartOrientation == ChartOrientationType.Vertical)
                        CreateColumnDataPointVisual(columnCanvas, labelCanvas, plotDetails, dataPoint, false, widthOfAcolumn, depth3d, animationEnabled);
                    else
                        BarChart.CreateBarDataPointVisual(dataPoint, labelCanvas, columnCanvas, false, widthOfAcolumn, depth3d, animationEnabled);
                }
            }

            // Apply animation
            if (animationEnabled && currentDataSeries != null)
            {
                if (currentDataSeries.Storyboard == null)
                    currentDataSeries.Storyboard = new Storyboard();

                // Apply animation to the marker and labels
                currentDataSeries.Storyboard = AnimationHelper.ApplyOpacityAnimation(labelCanvas, currentDataSeries, currentDataSeries.Storyboard, 1, 1, 0 ,1);
            }

            columnCanvas.Tag = null;

            if (plotDetails.ChartOrientation == ChartOrientationType.Vertical)
                ColumnChart.CreateOrUpdatePlank(chart, dataSeriesList4Rendering[0].PlotGroup.AxisY, columnCanvas, depth3d, Orientation.Horizontal);
            else
                ColumnChart.CreateOrUpdatePlank(chart, dataSeriesList4Rendering[0].PlotGroup.AxisY, columnCanvas, depth3d, Orientation.Vertical);

            // Remove old visual and add new visual in to the existing panel
            if (preExistingPanel != null)
            {   
                visual.Children.RemoveAt(1);
                visual.Children.Add(columnCanvas);
            }   
            else
            {   
                labelCanvas.SetValue(Canvas.ZIndexProperty, 1);
                visual.Children.Add(labelCanvas);
                visual.Children.Add(columnCanvas);
            }

            RectangleGeometry clipRectangle = new RectangleGeometry();
            if (plotDetails.ChartOrientation == ChartOrientationType.Vertical)
            {
                clipRectangle.Rect = new Rect(0, -chart.ChartArea.PLANK_DEPTH - (chart.View3D ? 0 : 5), width + chart.ChartArea.PLANK_DEPTH, height + chart.ChartArea.PLANK_DEPTH + chart.ChartArea.PLANK_THICKNESS + (chart.View3D ? 0 : 10));
                visual.Clip = clipRectangle;
            }
            else
            {
                clipRectangle.Rect = new Rect(-(chart.View3D ? 0 : 5) - chart.ChartArea.PLANK_THICKNESS, -chart.ChartArea.PLANK_DEPTH, width + chart.ChartArea.PLANK_DEPTH + chart.ChartArea.PLANK_THICKNESS + (chart.View3D ? 0 : 10)
                    , height + chart.ChartArea.PLANK_DEPTH);
                visual.Clip = clipRectangle;
            }
            return visual;
        }

        internal static void Update(ObservableObject sender, VcProperties property, object newValue, Boolean isAxisChanged)
        {
            Boolean isDataPoint = sender.GetType().Equals(typeof(DataPoint));

            if (isDataPoint)
                UpdateDataPoint(sender as DataPoint, property, newValue, isAxisChanged);
            else
                UpdateDataSeries(sender as DataSeries, property, newValue);
        }

        internal static void Update(Chart chart, RenderAs currentRenderAs, List<DataSeries> selectedDataSeries4Rendering)
        {
            
            Boolean is3D = chart.View3D;
            ChartArea chartArea = chart.ChartArea;
            Canvas ChartVisualCanvas = chart.ChartArea.ChartVisualCanvas;

            // Double width = chart.ChartArea.ChartVisualCanvas.Width;
            // Double height = chart.ChartArea.ChartVisualCanvas.Height;

            Panel renderedChart = selectedDataSeries4Rendering[0].Visual as Panel;

            renderedChart.Width = chart.ChartArea.ChartVisualCanvas.Width;
            renderedChart.Height = chart.ChartArea.ChartVisualCanvas.Height;

            renderedChart = chartArea.RenderSeriesFromList(renderedChart, selectedDataSeries4Rendering);

            foreach (DataSeries ds in selectedDataSeries4Rendering)
                ds.Visual = renderedChart;
        }

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

        private static void UpdateDataSeries(DataSeries dataSeries, VcProperties property, object newValue)
        {
            Chart chart = dataSeries.Chart as Chart;
            Boolean is3D = chart.View3D;

            switch (property)
            {
                case VcProperties.DataPoints:
                case VcProperties.Enabled:
                case VcProperties.YValue:
                    chart.ChartArea.RenderSeries();
                break;
                case VcProperties.XValue:
                    chart.ChartArea.RenderSeries();

                break;
            }
        }

        private static void Update2DAnd3DColumnBorderColor(DataPoint dataPoint, Boolean view3d)
        {
             Faces faces = dataPoint.Faces;
             foreach (Shape fe in faces.BorderElements)
             {
                 Rectangle rectangle = fe as Rectangle;
                 if (rectangle == null)
                     continue;

                 ExtendedGraphics.UpdateBorderOf2DRectangle(ref rectangle, dataPoint.BorderThickness.Left, ExtendedGraphics.GetDashArray((BorderStyles)dataPoint.BorderStyle)
                     , dataPoint.BorderColor, view3d ? new CornerRadius(0) : (CornerRadius)dataPoint.RadiusX, view3d ? new CornerRadius(0) : (CornerRadius)dataPoint.RadiusY);
             }
        }

        private static void Update2DAnd3DColumnColor(DataPoint dataPoint, Brush newValue)
        {
            Brush colorNewValue = (newValue != null) ? newValue : dataPoint.Color;

            Faces faces = dataPoint.Faces;

            if (faces == null)
                return;

            foreach (FrameworkElement fe in faces.Parts)
            {   
                if(fe.Tag == null)
                    continue;
                
                switch((fe.Tag as ElementData).VisualElementName)
                {
                    case "ColumnBase": (fe as Rectangle).Fill = ((Boolean)dataPoint.LightingEnabled ? Graphics.GetLightingEnabledBrush(colorNewValue, "Linear", null) : colorNewValue);
                    break;

                    case "FrontFace": (fe as Rectangle).Fill = (Boolean)dataPoint.LightingEnabled ? Graphics.GetFrontFaceBrush((Brush)colorNewValue) : (Brush)colorNewValue; 
                    break;

                    case "TopFace": (fe as Rectangle).Fill = (Boolean)dataPoint.LightingEnabled ? Graphics.GetTopFaceBrush((Brush)colorNewValue) : (Brush)colorNewValue;
                    break;

                    case "RightFace": (fe as Rectangle).Fill = (Boolean)dataPoint.LightingEnabled ? Graphics.GetRightFaceBrush((Brush)colorNewValue) : (Brush)colorNewValue;
                    break;
                }
            }

            foreach(FrameworkElement fe in faces.BevelElements)
            {   
                switch((fe.Tag as ElementData).VisualElementName)
                {   
                    case "TopBevel":
                        (fe as Shape).Fill = Graphics.GetBevelTopBrush(colorNewValue);
                        break;

                    case "LeftBevel":
                        (fe as Shape).Fill = Graphics.GetBevelSideBrush(((Boolean)dataPoint.LightingEnabled ? -70 : 0), colorNewValue);
                        break;

                    case "RightBevel":
                        (fe as Shape).Fill = Graphics.GetBevelSideBrush(((Boolean)dataPoint.LightingEnabled ? -110 : 180), colorNewValue);
                        break;

                    case "BottomBevel":
                        (fe as Shape).Fill = null;
                        break;
                }
            }

            if (dataPoint.Marker != null && (Boolean)dataPoint.MarkerEnabled)
                dataPoint.Marker.BorderColor = (dataPoint.GetValue(DataPoint.MarkerBorderColorProperty) as Brush == null) ? ((newValue != null) ? newValue as Brush : dataPoint.MarkerBorderColor) : dataPoint.MarkerBorderColor;
        }

        private static void CreateOrUpdateMarker(Chart chart, DataPoint dataPoint, Canvas labelCanvas, Canvas columnVisual)
        {
            
            if (chart.PlotDetails.ChartOrientation == ChartOrientationType.Vertical)
            {
                CreateOrUpdateMarker4VerticalChart(dataPoint, labelCanvas, new Size(columnVisual.Width, columnVisual.Height),
                (Double)columnVisual.GetValue(Canvas.LeftProperty), (Double)columnVisual.GetValue(Canvas.TopProperty));
            }
            else
            {
                Double depth3d = chart.ChartArea.PLANK_DEPTH / chart.PlotDetails.Layer3DCount * (chart.View3D ? 1 : 0);
                BarChart.CreateOrUpdateMarker4HorizontalChart(chart, labelCanvas, dataPoint, (Double)columnVisual.GetValue(Canvas.LeftProperty), (Double)columnVisual.GetValue(Canvas.TopProperty), dataPoint.InternalYValue >= 0, depth3d);
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
            if (property != VcProperties.Enabled)
            {
                if (dataPoint.Parent.Enabled == false || (Boolean)dataPoint.Enabled == false)
                {
                    return;
                }
            }

            Chart chart = dataPoint.Chart as Chart;
            PlotDetails plotDetails = chart.PlotDetails;

            Marker marker = dataPoint.Marker;
            DataSeries dataSeries = dataPoint.Parent;
            PlotGroup plotGroup = dataSeries.PlotGroup;
            Canvas labelCanvas = null, columnVisual = null;
            
            if (dataSeries.Faces != null)
            {   
                labelCanvas = dataSeries.Faces.LabelCanvas as Canvas;
                columnVisual = dataSeries.Faces.Visual as Canvas;
            }

            if(dataPoint.Faces != null)
                columnVisual = dataPoint.Faces.Visual as Canvas;

            //if (labelCanvas == null)
            //    return;
            
            switch (property)
            {
                case VcProperties.Bevel:
                    ApplyOrRemoveBevel(dataPoint);
                    
                    break;
                case VcProperties.Color:
                    Update2DAnd3DColumnColor(dataPoint, (Brush) newValue);
                    break;

                case VcProperties.BorderColor:
                case VcProperties.BorderThickness:
                case VcProperties.RadiusX:
                case VcProperties.RadiusY:
                case VcProperties.BorderStyle:
                    Update2DAnd3DColumnBorderColor(dataPoint, chart.View3D);
                    break;

                case VcProperties.Cursor:
                    dataPoint.SetCursor2DataPointVisualFaces();
                    break;

                case VcProperties.Href:
                    dataPoint.SetHref2DataPointVisualFaces();
                    break;

                case VcProperties.HrefTarget:
                    dataPoint.SetHref2DataPointVisualFaces();
                    break;

                case VcProperties.LabelBackground:
                    if (marker == null)
                        CreateOrUpdateMarker(chart, dataPoint, labelCanvas, columnVisual);
                    else if ((Boolean)dataPoint.LabelEnabled)
                        marker.TextBackground = dataPoint.LabelBackground;
                    else
                        marker.TextBackground = Graphics.TRANSPARENT_BRUSH;
                    break;

                case VcProperties.LabelEnabled:              
                    //if(marker == null)
                        CreateOrUpdateMarker(chart, dataPoint, labelCanvas, columnVisual);
                    //else
                    //    marker.LabelEnabled = (Boolean)dataPoint.LabelEnabled;

                    break;

                case VcProperties.LabelFontColor:
                    if (marker == null)
                        CreateOrUpdateMarker(chart, dataPoint, labelCanvas, columnVisual);
                    else
                        marker.FontColor = dataPoint.LabelFontColor;

                    break;

                case VcProperties.LabelFontFamily:
                    //if (marker == null)
                        CreateOrUpdateMarker(chart, dataPoint, labelCanvas, columnVisual);
                    //else
                    //    marker.FontFamily = dataPoint.LabelFontFamily;
                    break;

                case VcProperties.LabelFontStyle:
                    if (marker == null)
                        CreateOrUpdateMarker(chart, dataPoint, labelCanvas, columnVisual);
                    else
                        marker.FontStyle = (FontStyle)dataPoint.LabelFontStyle;
                    break;

                case VcProperties.LabelFontSize:
                    //if (marker == null)
                        CreateOrUpdateMarker(chart, dataPoint, labelCanvas, columnVisual);
                    //else
                    //    marker.FontSize = (Double)dataPoint.LabelFontSize;
                    break;

                case VcProperties.LabelFontWeight:
                    if (marker == null)
                        CreateOrUpdateMarker(chart, dataPoint, labelCanvas, columnVisual);
                    else
                        marker.FontWeight = (FontWeight)dataPoint.LabelFontWeight;
                    break;

                case VcProperties.LabelStyle:
                    if (marker == null)
                        CreateOrUpdateMarker(chart, dataPoint, labelCanvas, columnVisual);
                    break;

                case VcProperties.LabelAngle:
                    if (marker == null)
                        CreateOrUpdateMarker(chart, dataPoint, labelCanvas, columnVisual);
                    break;

                case VcProperties.LabelText:
                    CreateOrUpdateMarker(chart, dataPoint, labelCanvas, columnVisual);
                    break;

                case VcProperties.LegendText:
                    chart.InvokeRender();
                    break;

                case VcProperties.LightingEnabled:
                    ApplyRemoveLighting(dataPoint);
                    break;

                case VcProperties.MarkerBorderColor:
                    if (marker == null)
                        CreateOrUpdateMarker(chart, dataPoint, labelCanvas, columnVisual);
                    else if ((Boolean)dataPoint.MarkerEnabled)
                        marker.BorderColor = dataPoint.MarkerBorderColor;
                    break;
                    
                case VcProperties.MarkerBorderThickness:
                    if (marker == null)
                        CreateOrUpdateMarker(chart, dataPoint, labelCanvas, columnVisual);
                    else if ((Boolean)dataPoint.MarkerEnabled)
                        marker.BorderThickness = dataPoint.MarkerBorderThickness.Value.Left;
                    break;

                case VcProperties.MarkerColor:
                    if (marker == null)
                        CreateOrUpdateMarker(chart, dataPoint, labelCanvas, columnVisual);
                    else if ((Boolean)dataPoint.MarkerEnabled)
                        marker.MarkerFillColor = dataPoint.MarkerColor;
                    break;

                case VcProperties.MarkerScale:
                case VcProperties.MarkerSize:
                case VcProperties.MarkerType:
                    CreateOrUpdateMarker(chart, dataPoint, labelCanvas, columnVisual); 
                    break;

                case VcProperties.ToolTipText:
                    dataPoint._parsedToolTipText = dataPoint.TextParser(dataPoint.ToolTipText);
                    break;

                case VcProperties.XValueFormatString:
                case VcProperties.YValueFormatString:
                    dataPoint._parsedToolTipText = dataPoint.TextParser(dataPoint.ToolTipText);
                    CreateOrUpdateMarker(chart, dataPoint, labelCanvas, columnVisual); 
                    break;
                    
                case VcProperties.MarkerEnabled:
                    if (marker == null)
                        CreateOrUpdateMarker(chart, dataPoint, labelCanvas, columnVisual);
                    else if ((Boolean)dataPoint.MarkerEnabled)
                        LineChart.ShowDataPointMarker(dataPoint);
                    else
                        LineChart.HideDataPointMarker(dataPoint);

                    break;

                case VcProperties.ShadowEnabled:
                    ApplyOrRemoveShadow(dataPoint,
                        (dataSeries.RenderAs == RenderAs.StackedColumn || dataSeries.RenderAs == RenderAs.StackedColumn100
                        || dataSeries.RenderAs == RenderAs.StackedBar || dataSeries.RenderAs == RenderAs.StackedBar100
                        ), false);
                    
                    break;

                case VcProperties.Opacity:

                    if(marker != null)
                        marker.Visual.Opacity = dataPoint.Opacity * dataSeries.Opacity;

                    if(dataPoint.Faces.Visual != null)
                        dataPoint.Faces.Visual.Opacity = dataPoint.Opacity * dataSeries.Opacity;

                    break;

                case VcProperties.ShowInLegend:
                    chart.InvokeRender();
                    break;

                // case VcProperties.ToolTipText:
                //    dataPoint._parsedToolTipText = dataPoint.TextParser(dataPoint.ToolTipText);
                //    break;

                case VcProperties.XValueType:
                    chart.InvokeRender();
                    break;

                case VcProperties.XValue:
                    UpdateDataSeries(dataSeries, property, newValue);

                    //chart.Dispatcher.BeginInvoke(new Action<DataSeries, VcProperties, Object>(UpdateDataSeries), new object[] { dataSeries, property, newValue});

                    break;

                case VcProperties.Enabled:
                    if (dataPoint.Faces == null)
                    {   
                        UpdateDataSeries(dataSeries, property, newValue);
                        break;
                    }
                    else if (dataPoint.Parent.RenderAs == RenderAs.Column || dataSeries.RenderAs == RenderAs.Bar)
                    {   
                        if (dataPoint.Faces.Visual != null)
                        {
                            dataPoint.Faces.Visual.Visibility = ((Boolean)newValue) ? Visibility.Visible : Visibility.Collapsed;
                            if (marker != null && marker.Visual != null)
                                marker.Visual.Visibility = ((Boolean)newValue) ? Visibility.Visible : Visibility.Collapsed;

                            break;
                        }
                    }

                    if (marker != null && marker.Visual != null)
                        marker.Visual.Visibility = ((Boolean)newValue) ? Visibility.Visible : Visibility.Collapsed;

                    // For other stacked charts we need to redraw the charts, because stacked chart need reposition like stack
                    goto YVALUE_UPDATE;

                case VcProperties.YValue:
                YVALUE_UPDATE:
                    if (isAxisChanged)
                        UpdateDataSeries(dataSeries, property, newValue);
                    else
                    {
                        if (dataSeries.RenderAs == RenderAs.Column || dataSeries.RenderAs == RenderAs.Bar)
                        {
                            //if (chart.AnimatedUpdate)
                                chart.Dispatcher.BeginInvoke(new Action<Chart, DataPoint, Boolean>(UpdateVisualForYValue4ColumnChart), new object[] { chart, dataPoint, isAxisChanged });
                            //else
                            //    UpdateVisualForYValue4ColumnChart(chart, dataPoint, isAxisChanged);
                        }
                        else if (plotDetails.ChartOrientation == ChartOrientationType.Vertical)
                        {
                            if ((Boolean)chart.AnimatedUpdate)
                                chart.Dispatcher.BeginInvoke(new Action<RenderAs, Chart, DataPoint, Boolean>(UpdateVisualForYValue4StackedColumnChart), new object[] { dataSeries.RenderAs, chart, dataPoint, isAxisChanged });
                            else
                                UpdateVisualForYValue4StackedColumnChart(dataSeries.RenderAs, chart, dataPoint, isAxisChanged);
                        }
                        else
                        {
                            if ((Boolean)chart.AnimatedUpdate)
                                chart.Dispatcher.BeginInvoke(new Action<RenderAs, Chart, DataPoint, Boolean>(UpdateVisualForYValue4StackedBarChart), new object[] { dataSeries.RenderAs, chart, dataPoint, isAxisChanged });
                            else
                                UpdateVisualForYValue4StackedBarChart(dataSeries.RenderAs, chart, dataPoint, isAxisChanged);
                        }
                    }

                    //if (dataPoint.Parent.SelectionEnabled && dataPoint.Selected)
                    //    dataPoint.Select(true);
                    // chart.Dispatcher.BeginInvoke(new Action<DataPoint>(UpdateXAndYValue), new object[]{dataPoint});

                    chart._toolTip.Hide();

                    break;
            }
        }

        /// <summary>
        /// Create or update plank
        /// </summary>
        /// <param name="chart"></param>
        /// <param name="axis"></param>
        /// <param name="columnCanvas"></param>
        /// <param name="depth3d"></param>
        /// <param name="orientation"></param>
        /// <returns></returns>
        internal static Canvas CreateOrUpdatePlank(Chart chart, Axis axis, Canvas columnCanvas, Double depth3d, Orientation orientation)
        {
            Canvas plank = columnCanvas.Tag as Canvas;
            Double top=0, left=0;
            
            if (chart.View3D && axis.InternalAxisMinimum < 0 && axis.InternalAxisMaximum > 0)
            {
                if (orientation == Orientation.Horizontal)
                {
                    top = columnCanvas.Height - Graphics.ValueToPixelPosition(0, columnCanvas.Height, (Double)axis.InternalAxisMinimum, (Double)axis.InternalAxisMaximum, 0);
                    if (plank != null && (Double)plank.GetValue(Canvas.TopProperty) == top)
                        return plank;
                }
                else
                {
                    left = Graphics.ValueToPixelPosition(0, columnCanvas.Width, (Double)axis.InternalAxisMinimum, (Double)axis.InternalAxisMaximum, 0);
                    if (plank != null && (Double)plank.GetValue(Canvas.LeftProperty) == left)
                        return plank;
                }

                if (plank != null)
                { 
                    // Remove existing plank if plank is not required
                    columnCanvas.Children.Remove(plank);
                    columnCanvas.Tag = null;
                }
                //else
                {
                    // Set parameters for zero plank
                    //RectangularChartShapeParams plankParms = new RectangularChartShapeParams();
                    //plankParms.BackgroundBrush = new SolidColorBrush(Color.FromArgb((Byte)255, (Byte)127, (Byte)127, (Byte)127));
                    //plankParms.Lighting = true;
                    //plankParms.Depth = depth3d;

                    Brush frontBrush, topBrush, rightBrush;
                    ExtendedGraphics.GetBrushesForPlank(chart, out frontBrush, out topBrush, out rightBrush, true);

                    // Draw horizontal plank
                    if (orientation == Orientation.Horizontal)
                    {
                        //plankParms.Size = new Size(columnCanvas.Width, 1);

                        Faces zeroPlank = Get3DPlank(columnCanvas.Width, 1.4, depth3d, frontBrush, topBrush, rightBrush); 
                        
                        plank = zeroPlank.Visual as Canvas;

                        plank.SetValue(Canvas.ZIndexProperty, 0);
                        plank.Opacity = 0.7;
                        plank.IsHitTestVisible = false;
                        columnCanvas.Children.Add(plank);
                        columnCanvas.Tag = plank;

                    } // Draw vertical plank
                    else if (orientation == Orientation.Vertical)
                    {
                        //plankParms.Size = new Size(1, );

                        Faces zeroPlank = Get3DPlank(1, columnCanvas.Height, depth3d, frontBrush, topBrush, rightBrush);
                        plank = zeroPlank.Visual as Canvas;

                        plank.SetValue(Canvas.ZIndexProperty, 0);
                        plank.Opacity = 0.7;
                        plank.IsHitTestVisible = false;
                        columnCanvas.Children.Add(plank);
                        columnCanvas.Tag = plank;
                    }
                }

                // Set the position of Plank
                plank.SetValue(Canvas.LeftProperty, left);
                plank.SetValue(Canvas.TopProperty, (Double)top);
            }
            else if (plank != null)
            {
                // Remove existing plank if plank is not required
                columnCanvas.Children.Remove(plank);
                plank = null;
            }

            return plank;
        }

        public static void UpdateParentVisualCanvasSize(Chart chart, Canvas canvas)
        {
            if (canvas != null)
            {
                canvas.Width = chart.ChartArea.ChartVisualCanvas.Width;
                canvas.Height = chart.ChartArea.ChartVisualCanvas.Height;
            }
        }

        public static void UpdateVisualForYValue4ColumnChart(Chart chart, DataPoint dataPoint, Boolean isAxisChanged)
        {   
            DataSeries currentDataSeries;
            DataSeries dataSeries = dataPoint.Parent;
            Canvas columnChartCanvas, labelCanvas;
            Double depth3d = chart.ChartArea.PLANK_DEPTH / chart.PlotDetails.Layer3DCount * (chart.View3D ? 1 : 0);

            if (dataPoint.Faces == null)
            {
                // When datapoint faces is null and dataSeries faces is null we need to create atleast once DataPoint 
                // inorder to generate columnChartCanvas and labelCanvas
                if (dataSeries != null && dataSeries.Faces != null)
                {
                    labelCanvas = dataSeries.Faces.LabelCanvas as Canvas;
                    columnChartCanvas = dataSeries.Faces.Visual as Canvas;

                    Double sizeOfColumnOrBar;

                    if (dataPoint.Parent.RenderAs == RenderAs.Column)
                    {
                        sizeOfColumnOrBar = CalculateWidthOfEachColumn(chart, chart.ChartArea.ChartVisualCanvas.Width, dataSeries.PlotGroup.AxisX, RenderAs.Column, Orientation.Horizontal);
                        CreateColumnDataPointVisual(columnChartCanvas, labelCanvas, chart.PlotDetails, dataPoint,
                        true, sizeOfColumnOrBar, depth3d, false);
                    }
                    else
                    {
                        sizeOfColumnOrBar = CalculateWidthOfEachColumn(chart, chart.ChartArea.ChartVisualCanvas.Height, dataSeries.PlotGroup.AxisX, RenderAs.Bar, Orientation.Vertical);
                        BarChart.CreateBarDataPointVisual(dataPoint, labelCanvas, columnChartCanvas, true, sizeOfColumnOrBar, depth3d, false);
                    }
                }
                else
                {
                    UpdateDataSeries(dataSeries, VcProperties.YValue, null);
                    return;
                }
            }
            
            // Parent of the current DataPoint
            Canvas oldVisual = dataPoint.Faces.Visual as Canvas;  // Old visual for the column
            columnChartCanvas = oldVisual.Parent as Canvas;     // Existing parent canvas of column

            Boolean isPositive = (dataPoint.InternalYValue >= 0); // Whether YValue is positive
            
            Double oldMarkerTop = Double.NaN;
            Double currentMarkerTop = Double.NaN;
            Double oldLabelTop = Double.NaN;
            Double currentLabelTop = Double.NaN;
            Double plankYPos, oldTop, oldColumnHeight;
            RenderAs chartType = dataPoint.Parent.RenderAs;

            if (dataPoint.Storyboard != null)
            {
                dataPoint.Storyboard.Pause();
            }

            if (dataPoint.Marker != null && dataPoint.Marker.Visual != null)
            {
                if (chartType == RenderAs.Column)
                    oldMarkerTop = (Double)dataPoint.Marker.Visual.GetValue(Canvas.TopProperty);
                else
                    oldMarkerTop = (Double)dataPoint.Marker.Visual.GetValue(Canvas.LeftProperty);
            }

            if (dataPoint.LabelVisual != null)
            {
                if (chartType == RenderAs.Column)
                    oldLabelTop = (Double)dataPoint.LabelVisual.GetValue(Canvas.TopProperty);
                else
                    oldLabelTop = (Double)dataPoint.LabelVisual.GetValue(Canvas.LeftProperty);
            }

            if (dataPoint.Parent.RenderAs == RenderAs.Column)
            {
                oldTop = (Double)oldVisual.GetValue(Canvas.TopProperty);
                oldColumnHeight = oldVisual.Height;
            }
            else
            {
                oldTop = (Double)oldVisual.GetValue(Canvas.LeftProperty);
                oldColumnHeight = oldVisual.Width;
            }

            if (columnChartCanvas.Parent == null && Double.IsNaN(dataPoint.InternalYValue))
                return;

            labelCanvas = (columnChartCanvas.Parent as Canvas).Children[0] as Canvas;

            UpdateParentVisualCanvasSize(chart, columnChartCanvas);
            UpdateParentVisualCanvasSize(chart, labelCanvas);

            // Create new Column with new YValue
            if (chartType == RenderAs.Column)
                CreateColumnDataPointVisual(columnChartCanvas, labelCanvas, chart.PlotDetails, dataPoint,
                isPositive, oldVisual.Width, depth3d, false);
            else
                BarChart.CreateBarDataPointVisual(dataPoint, labelCanvas, columnChartCanvas, isPositive, oldVisual.Height, depth3d, false);

            // Visifire.Profiler.Profiler.Start("Remove");
            columnChartCanvas.Children.Remove(oldVisual);
            //Visifire.Profiler.Profiler.Report("Remove", true, true);

            // Update existing Plank
            CreateOrUpdatePlank(chart, dataSeries.PlotGroup.AxisY, columnChartCanvas, depth3d,
                dataPoint.Parent.RenderAs == RenderAs.Column ? Orientation.Horizontal : Orientation.Vertical);

            Boolean animationEnabled = (Boolean)chart.AnimatedUpdate;

            #region Animate Column

            // animationEnabled = false;
            if (animationEnabled)
            {
                //if (dataPoint._oldYValue == dataPoint.InternalYValue)
                //    return;

                if (dataPoint.Storyboard != null)
                {
                    dataPoint.Storyboard.Stop();

                    dataPoint.Storyboard.Children.Clear();
                }

                Storyboard storyBoard;

                // Calculate scale factor from the old value YValue of the DataPoint
                Double axisSize = (chartType == RenderAs.Column) ? columnChartCanvas.Height : columnChartCanvas.Width;
                Double limitingYValue = 0;
                PlotGroup plotGroup = dataSeries.PlotGroup;

                if (plotGroup.AxisY.InternalAxisMinimum > 0)
                    limitingYValue = (Double)plotGroup.AxisY.InternalAxisMinimum;
                if (plotGroup.AxisY.InternalAxisMaximum < 0)
                    limitingYValue = (Double)plotGroup.AxisY.InternalAxisMaximum;

                if (dataPoint.InternalYValue > (Double)plotGroup.AxisY.InternalAxisMaximum)
                    System.Diagnostics.Debug.WriteLine("Max Value greater then axis max");


                Double currentTop; /* Top position of the DataPoint with new Value */

                //Double axisYMin = plotGroup.AxisY.InternalAxisMinimum != plotGroup.AxisY._oldInternalAxisMinimum ? plotGroup.AxisY.InternalAxisMinimum : plotGroup.AxisY._oldInternalAxisMinimum;
                //Double axisYMax = plotGroup.AxisY.InternalAxisMaximum != plotGroup.AxisY._oldInternalAxisMaximum ? plotGroup.AxisY.InternalAxisMaximum : plotGroup.AxisY._oldInternalAxisMaximum;
                                
                Double axisYMin = (isAxisChanged || Double.IsNaN(plotGroup.AxisY._oldInternalAxisMinimum)) ? plotGroup.AxisY.InternalAxisMinimum : plotGroup.AxisY._oldInternalAxisMinimum;
                Double axisYMax = (isAxisChanged || Double.IsNaN(plotGroup.AxisY._oldInternalAxisMaximum)) ? plotGroup.AxisY.InternalAxisMaximum : plotGroup.AxisY._oldInternalAxisMaximum;
                
                System.Diagnostics.Debug.WriteLine("OldAxisMaximum : " + plotGroup.AxisY._oldInternalAxisMaximum + " NewAxisMaximum : " + plotGroup.AxisY.InternalAxisMaximum);
                
                plankYPos = Graphics.ValueToPixelPosition(axisSize, 0, axisYMin, axisYMax, limitingYValue);
                
                // Double axisYMin = isAxisChanged ? plotGroup.AxisY.InternalAxisMinimum : plotGroup.AxisY.InternalAxisMinimum;
                // Double axisYMax = isAxisChanged ? plotGroup.AxisY.InternalAxisMaximum : plotGroup.AxisY.InternalAxisMaximum;

                System.Diagnostics.Debug.WriteLine("AxisChanged=" + isAxisChanged.ToString());

                if (dataPoint._oldYValue >= 0)
                {
                    if (chartType == RenderAs.Column)
                    {
                        oldTop = Graphics.ValueToPixelPosition(axisSize, 0, axisYMin, axisYMax, dataPoint._oldYValue);
                        plankYPos = Graphics.ValueToPixelPosition(axisSize, 0, axisYMin, axisYMax, limitingYValue);
                    }
                    else
                    {
                        oldTop = Graphics.ValueToPixelPosition(0, axisSize, axisYMin, axisYMax, dataPoint._oldYValue);
                        plankYPos = Graphics.ValueToPixelPosition(0, axisSize, axisYMin, axisYMax, limitingYValue);
                    }

                    oldColumnHeight = Math.Abs(oldTop - plankYPos);
                }
                else
                {
                    if (chartType == RenderAs.Column)
                    {
                        plankYPos = Graphics.ValueToPixelPosition(axisSize, 0, axisYMin, axisYMax, dataPoint._oldYValue);
                        oldTop = Graphics.ValueToPixelPosition(axisSize, 0, axisYMin, axisYMax, limitingYValue);
                    }
                    else
                    {   
                        oldTop = Graphics.ValueToPixelPosition(0, axisSize, axisYMin, axisYMax, dataPoint._oldYValue);
                        plankYPos = Graphics.ValueToPixelPosition(0, axisSize, axisYMin, axisYMax, limitingYValue);
                    }

                    oldColumnHeight = Math.Abs(oldTop - plankYPos);
                }

                Double oldScaleFactor = oldColumnHeight / ((dataPoint.Parent.RenderAs == RenderAs.Column) ? dataPoint.Faces.Visual.Height : dataPoint.Faces.Visual.Width);

                if (Double.IsInfinity(oldScaleFactor))
                {
                    oldScaleFactor = 0;

                    if(dataPoint.Marker != null && dataPoint.Marker.Visual != null)
                        oldMarkerTop = plankYPos;
                }

                if (Double.IsNaN(oldScaleFactor))
                {
                    oldScaleFactor = 1;

                }

                //else if (oldScaleFactor > 1)
                //    oldScaleFactor = oldColumnHeight / ((dataPoint.Parent.RenderAs == RenderAs.Column) ? columnChartCanvas.Height : columnChartCanvas.Width);

                // End Calculate scale factor from the old value YValue of the DataPoint

                if (dataPoint.Storyboard != null)
                    storyBoard = dataPoint.Storyboard;
                else
                    storyBoard = new Storyboard();

                if (!Double.IsNaN(oldMarkerTop))
                {
                    if (dataPoint.Parent.RenderAs == RenderAs.Column)
                        currentMarkerTop = (Double)dataPoint.Marker.Visual.GetValue(Canvas.TopProperty);
                    else
                        currentMarkerTop = (Double)dataPoint.Marker.Visual.GetValue(Canvas.LeftProperty);
                }

                if (!Double.IsNaN(oldLabelTop))
                {
                    if (dataPoint.Parent.RenderAs == RenderAs.Column)

                        currentLabelTop = (Double)dataPoint.LabelVisual.GetValue(Canvas.TopProperty);
                    else
                        currentLabelTop = (Double)dataPoint.LabelVisual.GetValue(Canvas.LeftProperty);
                }

                if (dataPoint.Parent.RenderAs == RenderAs.Column)
                    currentTop = (Double)dataPoint.Faces.Visual.GetValue(Canvas.TopProperty);
                else
                    currentTop = (Double)dataPoint.Faces.Visual.GetValue(Canvas.LeftProperty);

                String property2Animate1 = (dataPoint.Parent.RenderAs == RenderAs.Column) ? "(Canvas.Top)" : "(Canvas.Left)";
                String property2Animate2 = (dataPoint.Parent.RenderAs == RenderAs.Column) ? "Height" : "Width";

                if (chart.View3D)
                {
                    Rectangle frontFace, topFace, rightFace;
                    
                    if (chartType == RenderAs.Column)
                    {
                        frontFace = dataPoint.Faces.VisualComponents[0] as Rectangle;
                        topFace = dataPoint.Faces.VisualComponents[1] as Rectangle;
                        rightFace = dataPoint.Faces.VisualComponents[2] as Rectangle;
                    }
                    else
                    {
                        // right face = topface;
                        // topfase = rightface;
                        frontFace = dataPoint.Faces.VisualComponents[0] as Rectangle;
                        topFace = dataPoint.Faces.VisualComponents[2] as Rectangle;
                        rightFace = dataPoint.Faces.VisualComponents[1] as Rectangle;
                    }

                    if (dataPoint._oldYValue > 0 && dataPoint.InternalYValue > 0 || dataPoint._oldYValue < 0 && dataPoint.InternalYValue < 0)
                    {
                        if (chartType == RenderAs.Column)
                        {
                             storyBoard = AnimationHelper.ApplyPropertyAnimation(dataPoint.Faces.Visual, property2Animate1, dataPoint, storyBoard, 0,
                            new Double[] { 0, 1.5 }, new Double[] { oldTop, currentTop }, AnimationHelper.GenerateKeySplineList(new Point(0, 0), new Point(1, 1), new Point(0, 1), new Point(0.5, 1)));
                        }
                        else
                        {   
                            if (dataPoint._oldYValue < 0 && dataPoint.InternalYValue < 0)
                            {
                                storyBoard = AnimationHelper.ApplyPropertyAnimation(dataPoint.Faces.Visual, property2Animate1, dataPoint, storyBoard, 0,
                                new Double[] { 0, 1.5 }, new Double[] { oldTop, currentTop }, AnimationHelper.GenerateKeySplineList(new Point(0, 0), new Point(1, 1), new Point(0, 1), new Point(0.5, 1)));
                            }

                            storyBoard = AnimationHelper.ApplyPropertyAnimation(topFace, property2Animate1, dataPoint, storyBoard, 0,
                            new Double[] { 0, 1.5 }, new Double[] { oldColumnHeight, frontFace.Width }, AnimationHelper.GenerateKeySplineList(new Point(0, 0), new Point(1, 1), new Point(0, 1), new Point(0.5, 1)));
                        }

                        storyBoard = AnimationHelper.ApplyPropertyAnimation(frontFace, property2Animate2, dataPoint, storyBoard, 0,
                            new Double[] { 0, 1.5 }, new Double[] { oldColumnHeight, chartType == RenderAs.Column ? dataPoint.Faces.Visual.Height : dataPoint.Faces.Visual.Width }, AnimationHelper.GenerateKeySplineList(new Point(0, 0), new Point(1, 1), new Point(0, 1), new Point(0.5, 1)));

                        storyBoard = AnimationHelper.ApplyPropertyAnimation(rightFace, property2Animate2, dataPoint, storyBoard, 0,
                            new Double[] { 0, 1.5 }, new Double[] { oldColumnHeight, chartType == RenderAs.Column ? rightFace.Height : rightFace.Width }, AnimationHelper.GenerateKeySplineList(new Point(0, 0), new Point(1, 1), new Point(0, 1), new Point(0.5, 1)));

                        if ((Boolean)dataPoint.MarkerEnabled && !Double.IsNaN(oldMarkerTop))
                        {
                            storyBoard = AnimationHelper.ApplyPropertyAnimation(dataPoint.Marker.Visual, property2Animate1, dataPoint, storyBoard, 0,
                                new Double[] { 0, 1.5 }, new Double[] { oldMarkerTop, currentMarkerTop },
                                AnimationHelper.GenerateKeySplineList(new Point(0, 0), new Point(1, 1), new Point(0, 1), new Point(0.5, 1)));

                            if (chartType == RenderAs.Column)
                                dataPoint.Marker.Visual.SetValue(Canvas.TopProperty, oldMarkerTop);
                            else
                                dataPoint.Marker.Visual.SetValue(Canvas.LeftProperty, oldMarkerTop);
                        }

                        if ((Boolean)dataPoint.LabelEnabled && !Double.IsNaN(oldLabelTop))
                        {   
                            storyBoard = AnimationHelper.ApplyPropertyAnimation(dataPoint.LabelVisual, property2Animate1, dataPoint, storyBoard, 0,
                                new Double[] { 0, 1.5 }, new Double[] { oldLabelTop, currentLabelTop },
                                AnimationHelper.GenerateKeySplineList(new Point(0, 0), new Point(1, 1), new Point(0, 1), new Point(0.5, 1)));
                            
                            if (chartType == RenderAs.Column)
                                dataPoint.LabelVisual.SetValue(Canvas.TopProperty, oldLabelTop);
                            else
                                dataPoint.LabelVisual.SetValue(Canvas.LeftProperty, oldLabelTop);
                        }
                    }
                    else
                    {
                        if (dataPoint._oldYValue >= 0 && dataPoint.InternalYValue < 0)
                        {
                            if (chartType == RenderAs.Column)
                            {
                                storyBoard = AnimationHelper.ApplyPropertyAnimation(dataPoint.Faces.Visual, property2Animate1, dataPoint, storyBoard, 0,
                                new Double[] { 0, 0.75, 1.5 }, new Double[] { oldTop, plankYPos, currentTop }, AnimationHelper.GenerateKeySplineList(new Point(0, 0), new Point(1, 1), new Point(0, 0.5), new Point(0.5, 0.5), new Point(0, 1), new Point(0.5, 1)));
                            }
                            else
                            {
                                storyBoard = AnimationHelper.ApplyPropertyAnimation(dataPoint.Faces.Visual, property2Animate1, dataPoint, storyBoard, 0,
                                new Double[] { 0, 0.75, 1.5 }, new Double[] { plankYPos, plankYPos, currentTop }, AnimationHelper.GenerateKeySplineList(new Point(0, 0), new Point(1, 1), new Point(0, 0.5), new Point(0.5, 0.5), new Point(0, 1), new Point(0.5, 1)));

                                storyBoard = AnimationHelper.ApplyPropertyAnimation(topFace, property2Animate1, dataPoint, storyBoard, 0,
                                    new Double[] { 0, 0.75, 1.5 }, new Double[] { oldColumnHeight, 0, dataPoint.Faces.Visual.Width }, AnimationHelper.GenerateKeySplineList(new Point(0, 0), new Point(1, 1), new Point(0, 0.5), new Point(0.5, 0.5), new Point(0, 1), new Point(0.5, 1)));
                            }
                            
                            storyBoard = AnimationHelper.ApplyPropertyAnimation(frontFace, property2Animate2, dataPoint, storyBoard, 0,
                                new Double[] { 0, 0.75, 1.5 }, new Double[] { oldColumnHeight, 0, chartType == RenderAs.Column ? dataPoint.Faces.Visual.Height : dataPoint.Faces.Visual.Width }, AnimationHelper.GenerateKeySplineList(new Point(0, 0), new Point(1, 1), new Point(0, 0.5), new Point(0.5, 0.5), new Point(0, 1), new Point(0.5, 1)));

                            storyBoard = AnimationHelper.ApplyPropertyAnimation(rightFace, property2Animate2, dataPoint, storyBoard, 0,
                                new Double[] { 0, 0.75, 1.5 }, new Double[] { oldColumnHeight, 0, chartType == RenderAs.Column ? rightFace.Height : rightFace.Width }, AnimationHelper.GenerateKeySplineList(new Point(0, 0), new Point(1, 1), new Point(0, 0.5), new Point(0.5, 0.5), new Point(0, 1), new Point(0.5, 1)));
                        }
                        else if (dataPoint._oldYValue <= 0 && dataPoint.InternalYValue >= 0)
                        {
                            if (chartType == RenderAs.Column)
                            {   
                                storyBoard = AnimationHelper.ApplyPropertyAnimation(dataPoint.Faces.Visual, property2Animate1, dataPoint, storyBoard, 0,
                                new Double[] { 0, 0.75, 1.5 }, new Double[] { oldTop, oldTop, currentTop }, AnimationHelper.GenerateKeySplineList(new Point(0, 0), new Point(1, 1), new Point(0, 0.5), new Point(0.5, 0.5), new Point(0, 1), new Point(0.5, 1)));
                            }
                            else
                            {
                                storyBoard = AnimationHelper.ApplyPropertyAnimation(dataPoint.Faces.Visual, property2Animate1, dataPoint, storyBoard, 0,
                                new Double[] { 0, 0.75, 1.5 }, new Double[] { oldTop, plankYPos, currentTop }, AnimationHelper.GenerateKeySplineList(new Point(0, 0), new Point(1, 1), new Point(0, 0.5), new Point(0.5, 0.5), new Point(0, 1), new Point(0.5, 1)));
                                
                                storyBoard = AnimationHelper.ApplyPropertyAnimation(topFace, property2Animate1, dataPoint, storyBoard, 0,
                                new Double[] { 0, 0.75, 1.5 }, new Double[] { oldColumnHeight, 0, dataPoint.Faces.Visual.Width }, AnimationHelper.GenerateKeySplineList(new Point(0, 0), new Point(1, 1), new Point(0, 0.5), new Point(0.5, 0.5), new Point(0, 1), new Point(0.5, 1)));
                            }

                            // plankYPos, plankYPos, currentTop
                            storyBoard = AnimationHelper.ApplyPropertyAnimation(frontFace, property2Animate2, dataPoint, storyBoard, 0,
                                new Double[] { 0, 0.75, 1.5 }, new Double[] { oldColumnHeight, 0, chartType == RenderAs.Column ? dataPoint.Faces.Visual.Height : dataPoint.Faces.Visual.Width }, AnimationHelper.GenerateKeySplineList(new Point(0, 0), new Point(1, 1), new Point(0, 0.5), new Point(0.5, 0.5), new Point(0, 1), new Point(0.5, 1)));

                            storyBoard = AnimationHelper.ApplyPropertyAnimation(rightFace, property2Animate2, dataPoint, storyBoard, 0,
                                new Double[] { 0, 0.75, 1.5 }, new Double[] { oldColumnHeight, 0, chartType == RenderAs.Column ? rightFace.Height : rightFace.Width }, AnimationHelper.GenerateKeySplineList(new Point(0, 0), new Point(1, 1), new Point(0, 0.5), new Point(0.5, 0.5), new Point(0, 1), new Point(0.5, 1)));
                        }

                        if (chartType == RenderAs.Column)
                            plankYPos = Math.Abs(axisSize - Graphics.ValueToPixelPosition(0, axisSize, plotGroup.AxisY.InternalAxisMinimum, plotGroup.AxisY.InternalAxisMaximum, limitingYValue));
                        else
                            plankYPos = Graphics.ValueToPixelPosition(0, axisSize, plotGroup.AxisY.InternalAxisMinimum, plotGroup.AxisY.InternalAxisMaximum, limitingYValue);
                            
                        if ((Boolean)dataPoint.MarkerEnabled && !Double.IsNaN(oldMarkerTop))
                        {
                            storyBoard = AnimationHelper.ApplyPropertyAnimation(dataPoint.Marker.Visual, property2Animate1, dataPoint, storyBoard, 0,
                                new Double[] { 0, 0.75, 1.5 }, new Double[] { oldMarkerTop, plankYPos, currentMarkerTop },
                                AnimationHelper.GenerateKeySplineList(new Point(0, 0), new Point(1, 1), new Point(0, 0.5), new Point(0.5, 0.5), new Point(0, 1), new Point(0.5, 1)));

                            if (chartType == RenderAs.Column)
                                dataPoint.Marker.Visual.SetValue(Canvas.TopProperty, oldMarkerTop);
                            else
                                dataPoint.Marker.Visual.SetValue(Canvas.LeftProperty, oldMarkerTop);
                        }

                        if ((Boolean)dataPoint.LabelEnabled && !Double.IsNaN(oldLabelTop))
                        {
                            storyBoard = AnimationHelper.ApplyPropertyAnimation(dataPoint.LabelVisual, property2Animate1, dataPoint, storyBoard, 0,
                                new Double[] { 0, 0.75, 1.5 }, new Double[] { oldLabelTop, plankYPos, currentLabelTop },
                                AnimationHelper.GenerateKeySplineList(new Point(0, 0), new Point(1, 1), new Point(0, 0.5), new Point(0.5, 0.5), new Point(0, 1), new Point(0.5, 1)));

                            if (chartType == RenderAs.Column)
                                dataPoint.LabelVisual.SetValue(Canvas.TopProperty, oldLabelTop);
                            else
                                dataPoint.LabelVisual.SetValue(Canvas.LeftProperty, oldLabelTop);
                        }
                    }
                    

                }
                else  // For 2D Charts
                {
                    if ((dataPoint._oldYValue < 0 && dataPoint.InternalYValue < 0 || dataPoint._oldYValue > 0 && dataPoint.InternalYValue > 0))
                    {
                        currentDataSeries = dataPoint.Parent;
                        storyBoard = ApplyColumnChartAnimation(currentDataSeries, dataPoint.Faces.Visual as Panel, storyBoard, isPositive, 0, new Double[] { 0, 1 }, new Double[] { oldScaleFactor, 1 }, dataPoint.Parent.RenderAs);
                        
                        if ((Boolean)dataPoint.MarkerEnabled && !Double.IsNaN(oldMarkerTop))
                        {
                            storyBoard = AnimationHelper.ApplyPropertyAnimation(dataPoint.Marker.Visual, property2Animate1, dataPoint, storyBoard, 0,
                                new Double[] { 0, 1 }, new Double[] { oldMarkerTop, currentMarkerTop },
                                AnimationHelper.GenerateKeySplineList(new Point(0, 0), new Point(1, 1), new Point(0, 1), new Point(0.5, 1)));

                            storyBoard = AnimationHelper.ApplyOpacityAnimation(dataPoint.Marker.Visual, dataPoint, storyBoard, 0,0.2, 0.98, 1);
                            
                            if (chartType == RenderAs.Column)
                                dataPoint.Marker.Visual.SetValue(Canvas.TopProperty, oldMarkerTop);
                            else
                                dataPoint.Marker.Visual.SetValue(Canvas.LeftProperty, oldMarkerTop);
                        }

                        if ((Boolean)dataPoint.LabelEnabled && !Double.IsNaN(oldLabelTop))
                        {
                            storyBoard = AnimationHelper.ApplyPropertyAnimation(dataPoint.LabelVisual, property2Animate1, dataPoint, storyBoard, 0,
                                new Double[] { 0, 1 }, new Double[] { oldLabelTop, currentLabelTop },
                                AnimationHelper.GenerateKeySplineList(new Point(0, 0), new Point(1, 1), new Point(0, 1), new Point(0.5, 1)));

                            if (chartType == RenderAs.Column)
                                dataPoint.LabelVisual.SetValue(Canvas.TopProperty, oldLabelTop);
                            else
                                dataPoint.LabelVisual.SetValue(Canvas.LeftProperty, oldLabelTop);
                        }
                    }
                    else
                    {
                        Double plankTop;    // Top position of the Plank (Top position of the Zero Line)

                        if (dataPoint.Parent.RenderAs == RenderAs.Column)
                        {
                            currentTop = (Double)dataPoint.Faces.Visual.GetValue(Canvas.TopProperty);
                            plankTop = axisSize - Graphics.ValueToPixelPosition(0, axisSize, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, 0);

                            if (dataPoint._oldYValue <= 0)
                            {
                                storyBoard = AnimationHelper.ApplyPropertyAnimation(dataPoint.Faces.Visual, property2Animate1, dataPoint, storyBoard, 0, new Double[] { 0, 0.5, 0.5, 1 }, new Double[] { plankTop, plankTop, plankTop, currentTop }, null);
                                storyBoard = ApplyColumnChartAnimation(dataPoint.Parent, dataPoint.Faces.Visual as Panel, storyBoard, false, 0, new Double[] { 0, 0.5, 0.5, 1 }, new Double[] { oldScaleFactor, 0, 0, 1 }, dataPoint.Parent.RenderAs);
                            }
                            else
                            {
                                storyBoard = AnimationHelper.ApplyPropertyAnimation(dataPoint.Faces.Visual, property2Animate1, dataPoint, storyBoard, 0, new Double[] { 0, 0.5, 0.5 }, new Double[] { oldTop, plankTop, plankTop }, null);
                                storyBoard = ApplyColumnChartAnimation(dataPoint.Parent, dataPoint.Faces.Visual as Panel, storyBoard, false, 0, new Double[] { 0, 0.5, 0.5, 1 }, new Double[] { oldScaleFactor, 0, 0, 1 }, dataPoint.Parent.RenderAs);
                            }
                        }
                        else
                        {   
                            currentTop = (Double)dataPoint.Faces.Visual.GetValue(Canvas.LeftProperty);
                            plankTop = Graphics.ValueToPixelPosition(0, axisSize, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, 0);

                            if (dataPoint._oldYValue > 0)
                            {
                                storyBoard = AnimationHelper.ApplyPropertyAnimation(dataPoint.Faces.Visual, property2Animate1, dataPoint, storyBoard, 0, new Double[] { 0, 0.5, 0.5, 1 }, new Double[] { plankTop, plankTop, plankTop, currentTop }, null);
                                storyBoard = ApplyColumnChartAnimation(dataPoint.Parent, dataPoint.Faces.Visual as Panel, storyBoard, true, 0, new Double[] { 0, 0.5, 0.5, 1 }, new Double[] { oldScaleFactor, 0, 0, 1 }, dataPoint.Parent.RenderAs);
                            }
                            else
                            {
                                storyBoard = AnimationHelper.ApplyPropertyAnimation(dataPoint.Faces.Visual, property2Animate1, dataPoint, storyBoard, 0, new Double[] { 0, 0.5, 0.5, 1 }, new Double[] { oldTop, plankTop, plankTop }, null);
                                storyBoard = ApplyColumnChartAnimation(dataPoint.Parent, dataPoint.Faces.Visual as Panel, storyBoard, true, 0, new Double[] { 0, 0.5, 0.5, 1 }, new Double[] { oldScaleFactor, 0, 0, 1 }, dataPoint.Parent.RenderAs);
                            }
                        }

                        if ((Boolean)dataPoint.MarkerEnabled && !Double.IsNaN(oldMarkerTop))
                        {
                            storyBoard = AnimationHelper.ApplyPropertyAnimation(dataPoint.Marker.Visual, property2Animate1, dataPoint, storyBoard, 0,
                                new Double[] { 0, 0.5, 0.5, 1 }, new Double[] { oldMarkerTop, plankTop, plankTop, currentMarkerTop },
                                null);
                        }

                        if ((Boolean)dataPoint.LabelEnabled && !Double.IsNaN(oldLabelTop))
                        {
                            storyBoard = AnimationHelper.ApplyPropertyAnimation(dataPoint.LabelVisual, property2Animate1, dataPoint, storyBoard, 0,
                               new Double[] { 0, 0.5, 0.5, 1 }, new Double[] { oldLabelTop, plankTop, plankTop, currentLabelTop },
                               null);
                        }
                    }
                }


                dataPoint.Storyboard = storyBoard;

#if WPF
                storyBoard.Begin(dataPoint.Chart._rootElement, true);
#else
                storyBoard.Begin();
#endif

            }
            
            #endregion Apply Animation

            if (columnChartCanvas.Parent != null)
            {
                Double width = chart.ChartArea.ChartVisualCanvas.Width;
                Double height = chart.ChartArea.ChartVisualCanvas.Height;

                RectangleGeometry clipRectangle = new RectangleGeometry();
                if (chart.PlotDetails.ChartOrientation == ChartOrientationType.Vertical)
                {
                    clipRectangle.Rect = new Rect(0, -chart.ChartArea.PLANK_DEPTH - (chart.View3D ? 0 : 5), width + chart.ChartArea.PLANK_DEPTH, height + chart.ChartArea.PLANK_DEPTH + chart.ChartArea.PLANK_THICKNESS + (chart.View3D ? 0 : 10));
                    (columnChartCanvas.Parent as Canvas).Clip = clipRectangle;
                }
                else
                {
                    clipRectangle.Rect = new Rect(-(chart.View3D ? 0 : 5) - chart.ChartArea.PLANK_THICKNESS, -chart.ChartArea.PLANK_DEPTH, width + chart.ChartArea.PLANK_DEPTH + chart.ChartArea.PLANK_THICKNESS + (chart.View3D ? 0 : 10)
                        , height + chart.ChartArea.PLANK_DEPTH);
                    (columnChartCanvas.Parent as Canvas).Clip = clipRectangle;
                }
            }

            if (dataPoint.Parent.SelectionEnabled && dataPoint.Selected)
                dataPoint.Select(true);
        }

        public static void UpdateVisualForYValue4ColumnChart1(Chart chart, DataPoint dataPoint, Boolean isAxisChanged)
        {   
            DataSeries currentDataSeries;

            DataSeries dataSeries = dataPoint.Parent;             // parent of the current DataPoint
            Canvas oldVisual = dataPoint.Faces.Visual as Canvas;  // Old visual for the column
            Canvas columnChartCanvas = oldVisual.Parent as Canvas;     // Existing parent canvas of column

            Boolean isPositive = (dataPoint.InternalYValue >= 0); // Whether YValue is positive
            Double depth3d = chart.ChartArea.PLANK_DEPTH / chart.PlotDetails.Layer3DCount * (chart.View3D ? 1 : 0);
            
            Double oldMarkerTop = Double.NaN;
            Double currentMarkerTop = Double.NaN;
            Double oldLabelTop = Double.NaN;
            Double currentLabelTop = Double.NaN;

            if (dataPoint.Marker != null && dataPoint.Marker.Visual != null)
            {   
                if(dataPoint.Parent.RenderAs == RenderAs.Column)
                    oldMarkerTop = (Double)dataPoint.Marker.Visual.GetValue(Canvas.TopProperty);
                else
                    oldMarkerTop = (Double)dataPoint.Marker.Visual.GetValue(Canvas.LeftProperty);
            }

            if (dataPoint.LabelVisual != null)
            {
                    if (dataPoint.Parent.RenderAs == RenderAs.Column)
                        oldLabelTop = (Double)dataPoint.LabelVisual.GetValue(Canvas.TopProperty);
                    else
                        oldLabelTop = (Double)dataPoint.LabelVisual.GetValue(Canvas.LeftProperty);
            }

            Canvas labelCanvas = (columnChartCanvas.Parent as Canvas).Children[0] as Canvas;

            UpdateParentVisualCanvasSize(chart, columnChartCanvas);
            UpdateParentVisualCanvasSize(chart, labelCanvas);

            // Create new Column with new YValue
            if (dataPoint.Parent.RenderAs == RenderAs.Column)
                CreateColumnDataPointVisual(columnChartCanvas, labelCanvas, chart.PlotDetails, dataPoint,
                isPositive, oldVisual.Width, depth3d, false);
            else
                BarChart.CreateBarDataPointVisual(dataPoint, labelCanvas, columnChartCanvas, isPositive, oldVisual.Height, depth3d, false);

           // Visifire.Profiler.Profiler.Start("Remove");
            columnChartCanvas.Children.Remove(oldVisual);
            //Visifire.Profiler.Profiler.Report("Remove", true, true);

            // Update existing Plank
            CreateOrUpdatePlank(chart, dataSeries.PlotGroup.AxisY, columnChartCanvas, depth3d, 
                dataPoint.Parent.RenderAs == RenderAs.Column ? Orientation.Horizontal : Orientation.Vertical);

            Boolean animationEnabled = (Boolean)chart.AnimatedUpdate;

            if (animationEnabled && dataPoint.Storyboard != null)
            {
                dataPoint.Storyboard.Stop();
            }

            #region Animate Column

           // animationEnabled = false;
            if (animationEnabled)
            {   
                Storyboard storyBoard;

                // Calculate scale factor from the old value YValue of the DataPoint
                Double axisSize = (dataPoint.Parent.RenderAs == RenderAs.Column) ? columnChartCanvas.Height : columnChartCanvas.Width;
                Double limitingYValue = 0;
                PlotGroup plotGroup = dataSeries.PlotGroup;

                if (plotGroup.AxisY.InternalAxisMinimum > 0)
                    limitingYValue = (Double)plotGroup.AxisY.InternalAxisMinimum;
                if (plotGroup.AxisY.InternalAxisMaximum < 0)
                    limitingYValue = (Double)plotGroup.AxisY.InternalAxisMaximum;

                if (dataPoint.InternalYValue > (Double)plotGroup.AxisY.InternalAxisMaximum)
                    System.Diagnostics.Debug.WriteLine("Max Value greater then axis max");

                Double oldBottom, oldTop, oldColumnHeight;

                Double axisYMin = plotGroup.AxisY.InternalAxisMinimum !=  plotGroup.AxisY._oldInternalAxisMinimum ? plotGroup.AxisY.InternalAxisMinimum : plotGroup.AxisY._oldInternalAxisMinimum;
                Double axisYMax = plotGroup.AxisY.InternalAxisMaximum !=  plotGroup.AxisY._oldInternalAxisMaximum ? plotGroup.AxisY.InternalAxisMaximum : plotGroup.AxisY._oldInternalAxisMaximum;

                // Double axisYMin = isAxisChanged ? plotGroup.AxisY.InternalAxisMinimum : plotGroup.AxisY._oldInternalAxisMinimum;
                // Double axisYMax = isAxisChanged ? plotGroup.AxisY.InternalAxisMaximum : plotGroup.AxisY._oldInternalAxisMaximum;

                // Double axisYMin = isAxisChanged ? plotGroup.AxisY.InternalAxisMinimum : plotGroup.AxisY.InternalAxisMinimum;
                // Double axisYMax = isAxisChanged ? plotGroup.AxisY.InternalAxisMaximum : plotGroup.AxisY.InternalAxisMaximum;
                
                System.Diagnostics.Debug.WriteLine("AxisChanged=" + isAxisChanged.ToString());
                if (dataPoint._oldYValue >= 0)
                {   
                    if (dataPoint.Parent.RenderAs == RenderAs.Column)
                    {
                        oldBottom = Graphics.ValueToPixelPosition(axisSize, 0, axisYMin, axisYMax, limitingYValue);
                        oldTop = Graphics.ValueToPixelPosition(axisSize, 0, axisYMin, axisYMax, dataPoint._oldYValue);
                    }
                    else
                    {
                        oldBottom = Graphics.ValueToPixelPosition(0, axisSize, axisYMin, axisYMax, limitingYValue);
                        oldTop = Graphics.ValueToPixelPosition(0, axisSize, axisYMin, axisYMax, dataPoint._oldYValue);
                    }

                    oldColumnHeight = Math.Abs(oldTop - oldBottom);
                }
                else
                {
                    if (dataPoint.Parent.RenderAs == RenderAs.Column)
                    {
                        oldBottom = Graphics.ValueToPixelPosition(axisSize, 0, axisYMin, axisYMax, dataPoint._oldYValue);
                        oldTop = Graphics.ValueToPixelPosition(axisSize, 0, axisYMin, axisYMax, limitingYValue);
                    }
                    else
                    {
                        oldTop = Graphics.ValueToPixelPosition(0, axisSize, axisYMin, axisYMax, dataPoint._oldYValue);
                        oldBottom = Graphics.ValueToPixelPosition(0, axisSize, axisYMin, axisYMax, limitingYValue);
                    }
                    
                    oldColumnHeight = Math.Abs(oldTop - oldBottom);
                }

                Double oldScaleFactor = oldColumnHeight / ((dataPoint.Parent.RenderAs == RenderAs.Column) ? dataPoint.Faces.Visual.Height : dataPoint.Faces.Visual.Width);

                if (Double.IsInfinity(oldScaleFactor))
                    oldScaleFactor = 0;

                if (Double.IsNaN(oldScaleFactor))
                    oldScaleFactor = 1;
                // else if (oldScaleFactor > 1)
                //     oldScaleFactor = oldColumnHeight / ((dataPoint.Parent.RenderAs == RenderAs.Column) ? columnChartCanvas.Height : columnChartCanvas.Width);

                // End Calculate scale factor from the old value YValue of the DataPoint

                storyBoard = new Storyboard();

                if (!Double.IsNaN(oldMarkerTop))
                {
                    if(dataPoint.Parent.RenderAs == RenderAs.Column)
                        currentMarkerTop = (Double)dataPoint.Marker.Visual.GetValue(Canvas.TopProperty);
                    else
                        currentMarkerTop = (Double)dataPoint.Marker.Visual.GetValue(Canvas.LeftProperty);
                }

                if (!Double.IsNaN(oldLabelTop))
                {
                    if (dataPoint.Parent.RenderAs == RenderAs.Column)
                        currentLabelTop = (Double)dataPoint.LabelVisual.GetValue(Canvas.TopProperty);
                    else
                        currentLabelTop = (Double)dataPoint.LabelVisual.GetValue(Canvas.LeftProperty);
                }

                String property2Animate = (dataPoint.Parent.RenderAs == RenderAs.Column) ? "(Canvas.Top)" : "(Canvas.Left)";

                if ((dataPoint._oldYValue < 0 && dataPoint.InternalYValue < 0 || dataPoint._oldYValue > 0 && dataPoint.InternalYValue > 0))
                {
                    currentDataSeries = dataPoint.Parent;
                    storyBoard = ApplyColumnChartAnimation(currentDataSeries, dataPoint.Faces.Visual as Panel, storyBoard, isPositive, 0, new Double[] { 0, 1 }, new Double[] { oldScaleFactor, 1 }, dataPoint.Parent.RenderAs);

                    if ((Boolean)dataPoint.MarkerEnabled && !Double.IsNaN(oldMarkerTop))
                    {   
                        storyBoard = AnimationHelper.ApplyPropertyAnimation(dataPoint.Marker.Visual, property2Animate, dataPoint, storyBoard, 0,
                            new Double[] { 0, 1 }, new Double[] { oldMarkerTop, currentMarkerTop },
                            AnimationHelper.GenerateKeySplineList( new Point(0, 0), new Point(1, 1), new Point(0, 1), new Point(0.5, 1)));
                    }

                    if ((Boolean)dataPoint.LabelEnabled && !Double.IsNaN(oldLabelTop))
                    {
                        storyBoard = AnimationHelper.ApplyPropertyAnimation(dataPoint.LabelVisual, property2Animate, dataPoint, storyBoard, 0,
                            new Double[] { 0, 1 }, new Double[] { oldLabelTop, currentLabelTop },
                            AnimationHelper.GenerateKeySplineList(new Point(0, 0), new Point(1, 1), new Point(0, 1), new Point(0.5, 1)));
                    }
                }
                else
                {   
                    Double currentTop;  // Top position of the DataPoint with new Value
                    Double plankTop;    // Top position of the Plank (Top position of the Zero Line)

                    if (dataPoint.Parent.RenderAs == RenderAs.Column)
                    {
                        currentTop = (Double)dataPoint.Faces.Visual.GetValue(Canvas.TopProperty);
                        plankTop = axisSize - Graphics.ValueToPixelPosition(0, axisSize, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, 0);

                        if (dataPoint._oldYValue <= 0)
                        {
                            storyBoard = AnimationHelper.ApplyPropertyAnimation(dataPoint.Faces.Visual, property2Animate, dataPoint, storyBoard, 0, new Double[] { 0, 0.5, 0.5, 1 }, new Double[] { plankTop, plankTop, plankTop, currentTop }, null);
                            storyBoard = ApplyColumnChartAnimation(dataPoint.Parent, dataPoint.Faces.Visual as Panel, storyBoard, false, 0, new Double[] { 0, 0.5, 0.5, 1 }, new Double[] { oldScaleFactor, 0, 0, 1 }, dataPoint.Parent.RenderAs);
                        }
                        else
                        {
                            storyBoard = AnimationHelper.ApplyPropertyAnimation(dataPoint.Faces.Visual, property2Animate, dataPoint, storyBoard, 0, new Double[] { 0, 0.5, 0.5 }, new Double[] { oldTop, plankTop, plankTop }, null);
                            storyBoard = ApplyColumnChartAnimation(dataPoint.Parent, dataPoint.Faces.Visual as Panel, storyBoard, false, 0, new Double[] { 0, 0.5, 0.5, 1 }, new Double[] { oldScaleFactor, 0, 0, 1 }, dataPoint.Parent.RenderAs);
                        }
                    }
                    else
                    {   
                        currentTop =(Double)dataPoint.Faces.Visual.GetValue(Canvas.LeftProperty);
                        plankTop = Graphics.ValueToPixelPosition(0, axisSize, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, 0);

                        if (dataPoint._oldYValue > 0)
                        {
                            storyBoard = AnimationHelper.ApplyPropertyAnimation(dataPoint.Faces.Visual, property2Animate, dataPoint, storyBoard, 0, new Double[] { 0, 0.5, 0.5, 1 }, new Double[] { plankTop, plankTop, plankTop, currentTop }, null);
                            storyBoard = ApplyColumnChartAnimation(dataPoint.Parent, dataPoint.Faces.Visual as Panel, storyBoard, true, 0, new Double[] { 0, 0.5, 0.5, 1 }, new Double[] { oldScaleFactor, 0, 0, 1 }, dataPoint.Parent.RenderAs);
                        }
                        else
                        {
                            storyBoard = AnimationHelper.ApplyPropertyAnimation(dataPoint.Faces.Visual, property2Animate, dataPoint, storyBoard, 0, new Double[] { 0, 0.5, 0.5, 1 }, new Double[] { oldTop, plankTop, plankTop}, null);
                            storyBoard = ApplyColumnChartAnimation(dataPoint.Parent, dataPoint.Faces.Visual as Panel, storyBoard, true, 0, new Double[] { 0, 0.5, 0.5, 1 }, new Double[] { oldScaleFactor, 0, 0, 1 }, dataPoint.Parent.RenderAs);
                        }
                    }

                    if ((Boolean)dataPoint.MarkerEnabled && !Double.IsNaN(oldMarkerTop))
                    {
                        storyBoard = AnimationHelper.ApplyPropertyAnimation(dataPoint.Marker.Visual, property2Animate, dataPoint, storyBoard, 0,
                            new Double[] { 0, 0.5, 0.5, 1 }, new Double[] { oldMarkerTop, plankTop, plankTop, currentMarkerTop },
                            null);
                    }

                    if ((Boolean)dataPoint.LabelEnabled && !Double.IsNaN(oldLabelTop))
                    {
                        storyBoard = AnimationHelper.ApplyPropertyAnimation(dataPoint.LabelVisual, property2Animate, dataPoint, storyBoard, 0,
                           new Double[] { 0, 0.5, 0.5, 1 }, new Double[] { oldLabelTop, plankTop, plankTop, currentLabelTop },
                           null);
                    }
                }

                dataPoint.Storyboard = storyBoard;

#if WPF
                storyBoard.Begin(dataPoint.Chart._rootElement, true);
#else           
                storyBoard.Begin();
#endif          
            }
            
            #endregion Apply Animation
        }
        
        private static void ApplyRemoveLighting(DataPoint dataPoint)
        {   
            Faces faces = dataPoint.Faces;

            if (faces == null)
                return;

            if ((dataPoint.Chart as Chart).View3D)
            {
                Update2DAnd3DColumnColor(dataPoint, dataPoint.Color);
                return;
            }

            Canvas columnVisual = faces.Visual as Canvas;

            // Remove visual elements used for lighting
            faces.ClearList(ref faces.LightingElements);

            // Add visual elements used for lighting
            if (!(Boolean)dataPoint.LightingEnabled && dataPoint.Parent.Bevel)
            {   
                Canvas gradienceCanvas = ExtendedGraphics.Get2DRectangleGradiance(columnVisual.Width, columnVisual.Height,
                    Graphics.GetLeftGradianceBrush(63),
                    Graphics.GetRightGradianceBrush(63),
                    Orientation.Vertical);

                columnVisual.Children.Add(gradienceCanvas);

                dataPoint.Faces.LightingElements.Add(columnVisual);
            }
             
            foreach (FrameworkElement fe in faces.Parts)
            {
                if (fe.Tag != null && (fe.Tag as ElementData).VisualElementName == "ColumnBase")
                {
                    Brush background = ((Boolean)dataPoint.LightingEnabled ? Graphics.GetLightingEnabledBrush(dataPoint.Color, "Linear", null) : dataPoint.Color);
                    (fe as Rectangle).Fill = background;
                }
            }
        }

        private static void ApplyOrRemoveBevel(DataPoint dataPoint)
        {   
            Faces faces = dataPoint.Faces;
            
            if (faces == null)
                throw new Exception("Faces of DataPoint is null. ColumnChart.ApplyBevel()");

            Canvas columnVisual = faces.Visual as Canvas;

            // Remove visual elements used for lighting
            faces.ClearList(ref faces.BevelElements);

            if ((dataPoint.Chart as Chart).View3D)
                return;

            // Add visual elements used for lighting
            if (dataPoint.Parent.Bevel && columnVisual.Height > 7 && columnVisual.Width > 14)
            {
                Canvas bevelCanvas = ExtendedGraphics.Get2DRectangleBevel(null, columnVisual.Width - 2 * dataPoint.BorderThickness.Left, columnVisual.Height - 2 * dataPoint.BorderThickness.Left, 6, 6,
                    Graphics.GetBevelTopBrush(dataPoint.Color),
                    Graphics.GetBevelSideBrush(((Boolean)dataPoint.LightingEnabled ? -70 : 0), dataPoint.Color),
                    Graphics.GetBevelSideBrush(((Boolean)dataPoint.LightingEnabled ? -110 : 180), dataPoint.Color),
                    null);

                foreach (FrameworkElement fe in bevelCanvas.Children)
                    dataPoint.Faces.BevelElements.Add(fe);

                dataPoint.Faces.BevelElements.Add(bevelCanvas);

                bevelCanvas.SetValue(Canvas.LeftProperty, dataPoint.BorderThickness.Left);
                bevelCanvas.SetValue(Canvas.TopProperty, dataPoint.BorderThickness.Left);

                columnVisual.Children.Add(bevelCanvas);
            }
        }
        
        /// <summary>
        /// Create 2D column for a DataPoint
        /// </summary>
        /// <param name="columnParams">Column parameters</param>
        /// <returns>Faces</returns>
        internal static Faces Get2DColumn(DataPoint dataPoint, Double width, Double height, Boolean isStacked, Boolean isTopOfStack)
        {   
            Faces faces = new Faces();
            dataPoint.Faces = faces;

            Canvas columnVisual = new Canvas() ;
            faces.Visual = columnVisual;

            columnVisual.Width = width;
            columnVisual.Height = height;

            Brush background = ((Boolean)dataPoint.LightingEnabled ? Graphics.GetLightingEnabledBrush(dataPoint.Color, "Linear", null) : dataPoint.Color);

            Rectangle columnBase = ExtendedGraphics.Get2DRectangle(dataPoint, width, height,
                dataPoint.BorderThickness.Left, ExtendedGraphics.GetDashArray((BorderStyles)dataPoint.BorderStyle), dataPoint.BorderColor,
                background, dataPoint.RadiusX.Value, dataPoint.RadiusX.Value);

            (columnBase.Tag as ElementData).VisualElementName = "ColumnBase";

            faces.VisualComponents.Add(columnBase);
            faces.Parts.Add(columnBase);
            faces.BorderElements.Add(columnBase);
            columnVisual.Children.Add(columnBase);
            // columnVisual.Children.Add(columnBase);

            ApplyOrRemoveBevel(dataPoint);

            ApplyRemoveLighting(dataPoint);

            ApplyOrRemoveShadow(dataPoint, isStacked, isTopOfStack);

            return faces;
        }


        internal static void ApplyOrRemoveShadow(DataPoint dataPoint, Boolean isStacked, Boolean isTopOfStack)
        {
            Faces faces = dataPoint.Faces;

            if (faces == null)
                throw new Exception("Faces of DataPoint is null. ColumnChart.ApplyOrRemoveShadow()");
            
            Canvas columnVisual = faces.Visual as Canvas;

            // Remove visual elements used for lighting
            faces.ClearList(ref faces.ShadowElements);

            if ((Boolean)dataPoint.ShadowEnabled)
            {   
                Double shadowVerticalOffsetGap = 1;
                Double shadowVerticalOffset = Chart.SHADOW_DEPTH - shadowVerticalOffsetGap;
                Double shadowHeight = columnVisual.Height;
                CornerRadius xRadius = (CornerRadius)dataPoint.RadiusX;
                CornerRadius yRadius = (CornerRadius)dataPoint.RadiusY;

                if (isStacked)
                {
                    if (dataPoint.InternalXValue >= 0)
                    {
                        if (isTopOfStack)
                        {
                            shadowHeight = columnVisual.Height - shadowVerticalOffset + shadowVerticalOffsetGap;
                            shadowVerticalOffset = Chart.SHADOW_DEPTH - shadowVerticalOffsetGap - shadowVerticalOffsetGap;
                            xRadius = new CornerRadius(xRadius.TopLeft, xRadius.TopRight, xRadius.BottomRight, xRadius.BottomLeft);
                            yRadius = new CornerRadius(yRadius.TopLeft, yRadius.TopRight, 0, 0);
                        }
                        else
                        {
                            shadowHeight = columnVisual.Height + 6;
                            shadowVerticalOffset = -2;
                            xRadius = new CornerRadius(xRadius.TopLeft, xRadius.TopRight, xRadius.BottomRight, xRadius.BottomLeft);
                            yRadius = new CornerRadius(0, 0, 0, 0);
                        }
                    }
                    else
                    {
                        if (isTopOfStack)
                        {
                            shadowHeight = columnVisual.Height - shadowVerticalOffset + shadowVerticalOffsetGap;
                            xRadius = new CornerRadius(xRadius.TopLeft, xRadius.TopRight, xRadius.BottomRight, xRadius.BottomLeft);
                            yRadius = new CornerRadius(yRadius.TopLeft, yRadius.TopRight, 0, 0);
                        }
                        else
                        {
                            shadowHeight = columnVisual.Height + Chart.SHADOW_DEPTH + 2;
                            shadowVerticalOffset = -2;
                            xRadius = new CornerRadius(xRadius.TopLeft, xRadius.TopRight, xRadius.BottomRight, xRadius.BottomLeft);
                            yRadius = new CornerRadius(0, 0, 0, 0);
                        }
                    }
                }

                Grid shadowGrid = ExtendedGraphics.Get2DRectangleShadow(null, columnVisual.Width, shadowHeight, xRadius, yRadius, isStacked ? 3 : 5);
                shadowGrid.SetValue(Canvas.TopProperty, shadowVerticalOffset);
                shadowGrid.SetValue(Canvas.LeftProperty, Chart.SHADOW_DEPTH);
                shadowGrid.Opacity = 0.7;
                shadowGrid.SetValue(Canvas.ZIndexProperty, -1);
                faces.ShadowElements.Add(shadowGrid);
                columnVisual.Children.Add(shadowGrid);
            }
        }
        
        ///// <summary>
        ///// Returns faces for 3D column
        ///// </summary>
        ///// <param name="columnParams">Column parameters</param>
        ///// <returns>Faces</returns>
        //internal static Faces Get3DColumn(RectangularChartShapeParams columnParams)
        //{
        //    return Get3DPlank(columnParams.Size.Width, columnParams.Size.Height, columnParams.Depth, null, null, null);
        //}

        internal static Faces Get3DColumn(FrameworkElement tagRef, Double width, Double height, Double Depth,
            Brush backgroundBrush, Brush frontBrush, Brush topBrush, Brush rightBrush, Boolean lightingEnabled, 
            BorderStyles borderStyle, Brush borderBrush, Double borderThickness)
        {   
            DoubleCollection strokeDashArray = ExtendedGraphics.GetDashArray(borderStyle);
            Faces faces = new Faces();

            Canvas columnVisual = new Canvas();

            columnVisual.Width = width;
            columnVisual.Height = height;

            if (frontBrush == null)
                frontBrush = lightingEnabled ? Graphics.GetFrontFaceBrush(backgroundBrush) : backgroundBrush;

            if (topBrush == null)
                topBrush = lightingEnabled ? Graphics.GetTopFaceBrush(backgroundBrush) : backgroundBrush;

            if (rightBrush == null)
                rightBrush = lightingEnabled ? Graphics.GetRightFaceBrush(backgroundBrush) : backgroundBrush;


            Rectangle front = ExtendedGraphics.Get2DRectangle(tagRef, width, height,
                borderThickness, strokeDashArray, borderBrush,
                frontBrush, new CornerRadius(0), new CornerRadius(0));

            front.Tag = new ElementData() { VisualElementName = "FrontFace", Element = tagRef };

            faces.VisualComponents.Add(front);
            faces.Parts.Add(front);
            faces.BorderElements.Add(front);

            Rectangle top = ExtendedGraphics.Get2DRectangle(tagRef, width, Depth,
                borderThickness,strokeDashArray, borderBrush,
                topBrush, new CornerRadius(0), new CornerRadius(0));

            top.Tag = new ElementData() { VisualElementName = "TopFace", Element = tagRef };

            faces.VisualComponents.Add(top);
            faces.Parts.Add(top);
            faces.BorderElements.Add(top);

            top.RenderTransformOrigin = new Point(0, 1);
            SkewTransform skewTransTop = new SkewTransform();
            skewTransTop.AngleX = -45;
            top.RenderTransform = skewTransTop;

            Rectangle right = ExtendedGraphics.Get2DRectangle(tagRef, Depth, height,
                borderThickness, strokeDashArray,  borderBrush,
                rightBrush, new CornerRadius(0), new CornerRadius(0));

            right.Tag = new ElementData() { VisualElementName = "RightFace", Element = tagRef };

            faces.VisualComponents.Add(right);
            faces.Parts.Add(right);
            faces.BorderElements.Add(right);

            right.RenderTransformOrigin = new Point(0, 0);
            SkewTransform skewTransRight = new SkewTransform();
            skewTransRight.AngleY = -45;
            right.RenderTransform = skewTransRight;

            columnVisual.Children.Add(front);
            columnVisual.Children.Add(top);
            columnVisual.Children.Add(right);

            top.SetValue(Canvas.TopProperty, -Depth);
            right.SetValue(Canvas.LeftProperty, width);

            faces.Visual = columnVisual;

            return faces;
        }


        /// <summary>
        /// Returns faces for 3D column
        /// </summary>
        /// <param name="columnParams">Column parameters</param>
        /// <param name="frontBrush">Brush for front face</param>
        /// <param name="topBrush">Brush for top face</param>
        /// <param name="rightBrush">Brush for right face</param>
        /// <returns>Faces</returns>
        internal static void Update3DPlank(Double width, Double height, Double depth3D, Faces plankFaces)
        {
            Rectangle front = plankFaces.VisualComponents[0] as Rectangle;
            Rectangle top = plankFaces.VisualComponents[1] as Rectangle;
            Rectangle right = plankFaces.VisualComponents[2] as Rectangle;

            front.Width = width;
            front.Height = height;

            top.Width = width;
            top.Height = depth3D;

            right.Width = depth3D;
            right.Height = height;

            top.SetValue(Canvas.TopProperty, -depth3D);
            right.SetValue(Canvas.LeftProperty, width);
        }

        /// <summary>
        /// Returns faces for 3D column
        /// </summary>
        /// <param name="columnParams">Column parameters</param>
        /// <param name="frontBrush">Brush for front face</param>
        /// <param name="topBrush">Brush for top face</param>
        /// <param name="rightBrush">Brush for right face</param>
        /// <returns>Faces</returns>
        internal static Faces Get3DPlank(Double width, Double height, Double depth3D, Brush frontBrush, Brush topBrush, Brush rightBrush)
        {   
            Faces faces = new Faces();
            Canvas columnVisual = new Canvas();

            columnVisual.Width = width;
            columnVisual.Height = height;
         
            Rectangle front = ExtendedGraphics.Get2DRectangle(null, width, height,
                0.25, null, null,
                frontBrush, new CornerRadius(0), new CornerRadius(0));

            Rectangle top = ExtendedGraphics.Get2DRectangle(null, width, depth3D,
                0.25, null, null,
                topBrush, new CornerRadius(0), new CornerRadius(0));

            Rectangle right = ExtendedGraphics.Get2DRectangle(null, depth3D, height,
               0.25, null, null,
               rightBrush, new CornerRadius(0), new CornerRadius(0));

            // Apply transformation
            top.RenderTransformOrigin = new Point(0, 1);
            SkewTransform skewTransTop = new SkewTransform();
            skewTransTop.AngleX = -45;
            top.RenderTransform = skewTransTop;

            right.RenderTransformOrigin = new Point(0, 0);
            SkewTransform skewTransRight = new SkewTransform();
            skewTransRight.AngleY = -45;
            right.RenderTransform = skewTransRight;

            columnVisual.Children.Add(front);
            columnVisual.Children.Add(top);
            columnVisual.Children.Add(right);

            top.SetValue(Canvas.TopProperty, - depth3D);
            right.SetValue(Canvas.LeftProperty, width);

            faces.VisualComponents.Add(front);
            faces.VisualComponents.Add(top);
            faces.VisualComponents.Add(right);

            faces.Visual = columnVisual;

            return faces;
        }

        #endregion

        #region Internal Events And Delegates

        #endregion

        #region Data

        /// <summary>
        /// Gap ratio between two column
        /// </summary>
        internal static Double COLUMN_GAP_RATIO = 0.1;

        #endregion
    }
}