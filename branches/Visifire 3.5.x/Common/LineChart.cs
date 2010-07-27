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
using Visifire.Charts;

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
        /// Returns label for DataPoint
        /// </summary>
        /// <param name="dataPoint"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="isPositive"></param>
        /// <param name="markerLeft"></param> 
        /// <param name="markerTop"></param> 
        /// <param name="labelCanvas"></param> 
        /// <param name="IsSetPosition">Whether to set the position while creating it (in this function itself) </param>
        /// <returns>New position of the label</returns>
        internal static Point CreateLabel4LineDataPoint(DataPoint dataPoint, Double width, Double height, Boolean isPositive,
            Double markerLeft, Double markerTop, ref Canvas labelCanvas, Boolean IsSetPosition)
        {
            Point retVal = new Point();

            if (dataPoint.LabelVisual != null )
            {
                Panel parent = dataPoint.LabelVisual.Parent as Panel;

                if (parent != null)
                    parent.Children.Remove(dataPoint.LabelVisual);
            }

            Chart chart = dataPoint.Chart as Chart;

            if (dataPoint.Faces == null)
                return retVal;

            if ((Boolean)dataPoint.LabelEnabled && !String.IsNullOrEmpty(dataPoint.LabelText))
            {
                LabelStyles autoLabelStyle = (LabelStyles)dataPoint.LabelStyle;

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

                dataPoint.LabelVisual = tb.Visual;

                Double labelLeft = 0;
                Double labelTop = 0;

                if (Double.IsNaN(dataPoint.LabelAngle) || dataPoint.LabelAngle == 0)
                {   
                    SetLabelPosition4LineDataPoint(dataPoint,  ref labelLeft, ref labelTop);

                    retVal.X = labelLeft;
                    retVal.Y = labelTop;

                    if (IsSetPosition)
                    {
                        tb.Visual.SetValue(Canvas.LeftProperty, labelLeft);
                        tb.Visual.SetValue(Canvas.TopProperty, labelTop);
                    }

                    Double depth3D = chart.ChartArea.PLANK_DEPTH / chart.PlotDetails.Layer3DCount * (chart.View3D ? 1 : 0);

                    if (!dataPoint.IsLabelStyleSet && !dataPoint.Parent.IsLabelStyleSet)
                    {
                        if (isPositive)
                        {
                            if (labelTop < -depth3D)
                                autoLabelStyle = LabelStyles.Inside;
                        }
                        else
                        {
                            if (labelTop + tb.TextBlockDesiredSize.Height > chart.PlotArea.BorderElement.Height - depth3D + chart.ChartArea.PLANK_THICKNESS)
                                autoLabelStyle = LabelStyles.Inside;
                        }
                    }

                    if (autoLabelStyle != dataPoint.LabelStyle)
                    {   
                        SetLabelPosition4LineDataPoint(dataPoint, ref labelLeft, ref labelTop);

                        retVal.X = labelLeft;
                        retVal.Y = labelTop;

                        if (IsSetPosition)
                        {
                            tb.Visual.SetValue(Canvas.LeftProperty, labelLeft);
                            tb.Visual.SetValue(Canvas.TopProperty, labelTop);
                        }
                    }
                }
                else
                {
                    if (isPositive)
                    {
                        Point centerOfRotation = new Point(markerLeft,
                        markerTop - tb.TextBlockDesiredSize.Height / 2);
                        Double radius = dataPoint.Marker.MarkerSize.Height / 2 * dataPoint.Marker.ScaleFactor;
                        Double angle = 0;
                        Double angleInRadian = 0;

                        if (autoLabelStyle == LabelStyles.OutSide)
                        {
                            if (dataPoint.LabelAngle > 0 && dataPoint.LabelAngle <= 90)
                            {
                                angle = dataPoint.LabelAngle - 180;
                                angleInRadian = (Math.PI / 180) * angle;
                                radius += tb.TextBlockDesiredSize.Width;
                                angle = (angleInRadian - Math.PI) * (180 / Math.PI);
                                ColumnChart.SetRotation(radius, angle, angleInRadian, centerOfRotation, labelLeft, labelTop, tb);
                            }
                            else if (dataPoint.LabelAngle >= -90 && dataPoint.LabelAngle < 0)
                            {
                                angle = dataPoint.LabelAngle;
                                angleInRadian = (Math.PI / 180) * angle;
                                ColumnChart.SetRotation(radius, angle, angleInRadian, centerOfRotation, labelLeft, labelTop, tb);
                            }
                        }
                        else
                        {
                            centerOfRotation = new Point(markerLeft,
                                markerTop + dataPoint.Marker.MarkerSize.Height / 2);
                            if (dataPoint.LabelAngle >= -90 && dataPoint.LabelAngle < 0)
                            {
                                angle = 180 + dataPoint.LabelAngle;
                                angleInRadian = (Math.PI / 180) * angle;
                                radius += tb.TextBlockDesiredSize.Width + 3;
                                angle = (angleInRadian - Math.PI) * (180 / Math.PI);
                                ColumnChart.SetRotation(radius, angle, angleInRadian, centerOfRotation, labelLeft, labelTop, tb);
                            }
                            else if (dataPoint.LabelAngle > 0 && dataPoint.LabelAngle <= 90)
                            {
                                //radius += 3;
                                angle = dataPoint.LabelAngle;
                                angleInRadian = (Math.PI / 180) * angle;
                                ColumnChart.SetRotation(radius, angle, angleInRadian, centerOfRotation, labelLeft, labelTop, tb);
                            }
                        }
                    }
                    else
                    {
                        Point centerOfRotation = new Point();
                        Double radius = dataPoint.Marker.MarkerSize.Height / 2 * dataPoint.Marker.ScaleFactor;
                        Double angle = 0;
                        Double angleInRadian = 0;

                        if (autoLabelStyle == LabelStyles.OutSide)
                        {
                            centerOfRotation = new Point(markerLeft,
                                markerTop + dataPoint.Marker.MarkerSize.Height / 2);

                            if (dataPoint.LabelAngle >= -90 && dataPoint.LabelAngle < 0)
                            {
                                angle = 180 + dataPoint.LabelAngle;
                                angleInRadian = (Math.PI / 180) * angle;
                                radius += tb.TextBlockDesiredSize.Width;
                                angle = (angleInRadian - Math.PI) * (180 / Math.PI);
                                ColumnChart.SetRotation(radius, angle, angleInRadian, centerOfRotation, labelLeft, labelTop, tb);
                            }
                            else if (dataPoint.LabelAngle > 0 && dataPoint.LabelAngle <= 90)
                            {
                                angle = dataPoint.LabelAngle;
                                angleInRadian = (Math.PI / 180) * angle;
                                ColumnChart.SetRotation(radius, angle, angleInRadian, centerOfRotation, labelLeft, labelTop, tb);
                            }
                        }
                        else
                        {
                            centerOfRotation = new Point(markerLeft,
                                markerTop - dataPoint.Marker.MarkerSize.Height / 2);

                            if (dataPoint.LabelAngle > 0 && dataPoint.LabelAngle <= 90)
                            {
                                angle = dataPoint.LabelAngle - 180;
                                angleInRadian = (Math.PI / 180) * angle;
                                radius += tb.TextBlockDesiredSize.Width + 3;
                                angle = (angleInRadian - Math.PI) * (180 / Math.PI);
                                ColumnChart.SetRotation(radius, angle, angleInRadian, centerOfRotation, labelLeft, labelTop, tb);
                            }
                            else if (dataPoint.LabelAngle >= -90 && dataPoint.LabelAngle < 0)
                            {
                                //radius += 3;
                                angle = dataPoint.LabelAngle;
                                angleInRadian = (Math.PI / 180) * angle;
                                ColumnChart.SetRotation(radius, angle, angleInRadian, centerOfRotation, labelLeft, labelTop, tb);
                            }
                        }
                    }
                }

                if (autoLabelStyle != dataPoint.LabelStyle)
                {
                    tb.TextElement.Foreground = Chart.CalculateDataPointLabelFontColor(dataPoint.Chart as Chart, dataPoint, dataPoint.LabelFontColor, (dataPoint.YValue <= 0 ? LabelStyles.OutSide : autoLabelStyle));
                }

                dataPoint.LabelVisual = tb.Visual;

                dataPoint.LabelVisual.Width = tb.TextBlockDesiredSize.Width;
                dataPoint.LabelVisual.Height = tb.TextBlockDesiredSize.Height;

                labelCanvas.Children.Add(tb.Visual);
            }

            return retVal;
        }

        /// <summary>
        /// Returns marker for DataPoint
        /// </summary>
        /// <param name="chart">Chart</param>
        /// <param name="position">Marker position</param>
        /// <param name="dataPoint">DataPoint</param>
        /// <param name="isPositive">Whether YValue is positive or negative</param>
        /// <returns>Marker</returns>
        internal static Marker GetMarkerForDataPoint(Boolean reCreate, Chart chart, Double plotWidth, Double plotHeight, Double yPosition, DataPoint dataPoint, Boolean isPositive)
        {
            String labelText;

            if (dataPoint.Parent.RenderAs == RenderAs.Spline || dataPoint.Parent.RenderAs == RenderAs.Line || dataPoint.Parent.RenderAs == RenderAs.StepLine)
                labelText = "";
            else
                labelText = (Boolean)dataPoint.LabelEnabled ? dataPoint.TextParser(dataPoint.LabelText) : "";

            Boolean markerBevel = false;

            if (reCreate)
            {
                //Marker marker = dataPoint.Marker;

                //if(marker != null && marker.Visual != null)
                //{   
                //    Panel parent = marker.Visual.Parent as Panel;

                //    if (parent != null)
                //        parent.Children.Remove(marker.Visual);

                //    marker.MarkerType = (MarkerTypes)dataPoint.MarkerType;
                //    marker.ScaleFactor = (Double)dataPoint.MarkerScale;
                //    marker.MarkerSize = new Size((Double)dataPoint.MarkerSize, (Double)dataPoint.MarkerSize);
                //    marker.Bevel = false;
                //    marker.MarkerFillColor = dataPoint.MarkerColor;
                //    marker.Text = labelText;

                //}
                //else
                {
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

            if ((Boolean)dataPoint.LabelEnabled && !String.IsNullOrEmpty(labelText))
            {
                ApplyLabelProperties(dataPoint);

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

            dataPoint.Marker.Visual.Opacity = (Double)dataPoint.Opacity * (Double)dataPoint.Parent.Opacity;

            ApplyDefaultInteractivityForMarker(dataPoint);

            //dataPoint.AttachEvent2DataPointVisualFaces(dataPoint);
            ObservableObject.AttachEvents2Visual(dataPoint, dataPoint, dataPoint.Marker.Visual);
            //dataPoint.AttachEvent2DataPointVisualFaces(dataPoint.Parent);
            ObservableObject.AttachEvents2Visual(dataPoint.Parent, dataPoint, dataPoint.Marker.Visual);
            dataPoint._parsedToolTipText = dataPoint.TextParser(dataPoint.ToolTipText);
            
            if(!chart.IndicatorEnabled)
                dataPoint.AttachToolTip(chart, dataPoint, dataPoint.Marker.Visual);
            
            dataPoint.AttachHref(chart, dataPoint.Marker.Visual, dataPoint.Href, (HrefTargets)dataPoint.HrefTarget);
            dataPoint.SetCursor2DataPointVisualFaces();
            return dataPoint.Marker;
        }

        /// <summary>
        /// Set position for DataPoint label
        /// </summary>
        /// <param name="dataPoint"></param>
        /// <param name="plotWidth"></param>
        /// <param name="plotHeight"></param>
        /// <param name="isPositive"></param>
        /// <param name="markerLeft"></param>
        /// <param name="markerTop"></param>
        /// <param name="labelLeft"></param>
        /// <param name="labelTop"></param>
        /// <param name="gap"></param>
        /// <param name="textBlockSize"></param>
        internal static Point SetLabelPosition4LineDataPoint(DataPoint dataPoint, ref Double labelLeft, ref Double labelTop)
        {
            Double GAP = 6;
            Chart chart = dataPoint.Chart as Chart;
            Double plotWidth = chart.ChartArea.PlotAreaCanvas.Width;
            Double plotHeight = chart.ChartArea.PlotAreaCanvas.Height;
            Boolean isPositive = dataPoint.InternalYValue >= 0;
            Double markerLeft = dataPoint._visualPosition.X;
            Double markerTop = dataPoint._visualPosition.Y;

            Size textBlockSize = new Size(dataPoint.LabelVisual.Width, dataPoint.LabelVisual.Height);
            
            Point currPoint = new Point(markerLeft, markerTop);
            Point prevPoint = new Point(0, 0);
            Point nextPoint = new Point(0, 0);

            if (dataPoint.Faces.PreviousDataPoint != null)
                prevPoint = dataPoint.Faces.PreviousDataPoint._visualPosition;

            if (dataPoint.Faces.NextDataPoint != null)
                nextPoint = dataPoint.Faces.NextDataPoint._visualPosition;

            Boolean forcedAutoPlacement = false;

            if (isPositive)
            {
                if (dataPoint.LabelStyle == LabelStyles.OutSide && !dataPoint.IsLabelStyleSet && !dataPoint.Parent.IsLabelStyleSet)
                {
                    if (currPoint.Y - textBlockSize.Height - dataPoint.Marker.MarkerSize.Height / 2 < 0 || dataPoint.LabelStyle == LabelStyles.Inside)
                    {
                        if (currPoint.X + textBlockSize.Width > plotWidth && prevPoint.Y - currPoint.Y <= 50 && textBlockSize.Width < 50)
                        {
                            labelLeft = currPoint.X - textBlockSize.Width / 2;// +dataPoint.Marker.MarkerSize.Width / 2 * dataPoint.Marker.ScaleFactor;
                            labelTop = currPoint.Y + GAP + dataPoint.Marker.MarkerSize.Height / 2 * dataPoint.Marker.ScaleFactor;
                        }
                        else if ((currPoint.X + textBlockSize.Width > plotWidth && prevPoint.Y - currPoint.Y > 50) || currPoint.X + textBlockSize.Width > plotWidth)
                        {
                            //dataPoint.Marker.TextAlignmentX = AlignmentX.Left;
                            //dataPoint.Marker.TextAlignmentY = AlignmentY.Bottom;
                            labelLeft = currPoint.X - textBlockSize.Width - GAP - dataPoint.Marker.MarkerSize.Width / 2 * dataPoint.Marker.ScaleFactor; ;// +dataPoint.Marker.MarkerSize.Width / 2 * dataPoint.Marker.ScaleFactor;
                            labelTop = currPoint.Y - GAP + dataPoint.Marker.MarkerSize.Height / 2 * dataPoint.Marker.ScaleFactor;
                        }
                        else if ((prevPoint.Y > currPoint.Y && nextPoint.Y > currPoint.Y) && prevPoint.Y - currPoint.Y > 20 && prevPoint.Y - currPoint.Y > nextPoint.Y - currPoint.Y)
                        {
                            if (currPoint.X - textBlockSize.Width - (dataPoint.Marker.MarkerSize.Width / 2 * dataPoint.Marker.ScaleFactor) <= 2)
                            {
                                labelLeft = currPoint.X + GAP + dataPoint.Marker.MarkerSize.Width / 2 * dataPoint.Marker.ScaleFactor;
                                labelTop = currPoint.Y - textBlockSize.Height / 2;// -gap + dataPoint.Marker.MarkerSize.Height / 2 * dataPoint.Marker.ScaleFactor;
                            }
                            else
                            {
                                labelLeft = currPoint.X - textBlockSize.Width - GAP - dataPoint.Marker.MarkerSize.Width / 2 * dataPoint.Marker.ScaleFactor; ;// +dataPoint.Marker.MarkerSize.Width / 2 * dataPoint.Marker.ScaleFactor;
                                labelTop = currPoint.Y - textBlockSize.Height / 2;// -gap + dataPoint.Marker.MarkerSize.Height / 2 * dataPoint.Marker.ScaleFactor;
                            }
                        }
                        else if (nextPoint.X - currPoint.X > 120 && textBlockSize.Width < 50)
                        {
                            labelLeft = currPoint.X - textBlockSize.Width / 2;// +dataPoint.Marker.MarkerSize.Width / 2 * dataPoint.Marker.ScaleFactor;
                            labelTop = currPoint.Y + GAP + dataPoint.Marker.MarkerSize.Height / 2 * dataPoint.Marker.ScaleFactor;
                        }
                        else if (currPoint.Y > prevPoint.Y && currPoint.Y - prevPoint.Y > currPoint.Y - nextPoint.Y && prevPoint.X != 0)
                        {
                            labelLeft = currPoint.X - textBlockSize.Width - GAP - dataPoint.Marker.MarkerSize.Width / 2 * dataPoint.Marker.ScaleFactor; ;// +dataPoint.Marker.MarkerSize.Width / 2 * dataPoint.Marker.ScaleFactor;
                            labelTop = currPoint.Y - GAP + dataPoint.Marker.MarkerSize.Height / 2 * dataPoint.Marker.ScaleFactor;
                        }
                        else if(currPoint.Y >= nextPoint.Y && prevPoint == new Point(0,0))
                        {
                            labelLeft = currPoint.X - textBlockSize.Width / 2;// +dataPoint.Marker.MarkerSize.Width / 2 * dataPoint.Marker.ScaleFactor;
                            labelTop = currPoint.Y + GAP / 2 + dataPoint.Marker.MarkerSize.Height / 2 * dataPoint.Marker.ScaleFactor;
                        }
                        else if (currPoint.Y >= nextPoint.Y && prevPoint.Y >= currPoint.Y)
                        {
                            labelLeft = currPoint.X + GAP + dataPoint.Marker.MarkerSize.Width / 2 * dataPoint.Marker.ScaleFactor;
                            labelTop = currPoint.Y + GAP / 2 + dataPoint.Marker.MarkerSize.Height / 2 * dataPoint.Marker.ScaleFactor;
                        }
                        else if (currPoint.Y <= nextPoint.Y && prevPoint.Y >= currPoint.Y)
                        {
                            if (textBlockSize.Width < 15)
                            {
                                labelLeft = currPoint.X - textBlockSize.Width / 2;// +dataPoint.Marker.MarkerSize.Width / 2 * dataPoint.Marker.ScaleFactor;
                                labelTop = currPoint.Y + GAP / 2 + dataPoint.Marker.MarkerSize.Height / 2 * dataPoint.Marker.ScaleFactor;
                            }
                            else
                            {
                                labelLeft = currPoint.X + GAP + dataPoint.Marker.MarkerSize.Width / 2 * dataPoint.Marker.ScaleFactor;
                                labelTop = currPoint.Y + dataPoint.Marker.MarkerSize.Height / 2 * dataPoint.Marker.ScaleFactor;
                            }
                        }
                        else if (prevPoint == new Point(0, 0) && (currPoint.X - textBlockSize.Width / 2) > 0)
                        {
                            labelLeft = currPoint.X - textBlockSize.Width / 2;// +dataPoint.Marker.MarkerSize.Width / 2 * dataPoint.Marker.ScaleFactor;
                            labelTop = currPoint.Y + GAP / 2 + dataPoint.Marker.MarkerSize.Height / 2 * dataPoint.Marker.ScaleFactor;
                        }
                        else
                        {
                            labelLeft = currPoint.X + GAP + dataPoint.Marker.MarkerSize.Width / 2 * dataPoint.Marker.ScaleFactor;
                            labelTop = currPoint.Y - textBlockSize.Height / 2;// -gap + dataPoint.Marker.MarkerSize.Height / 2 * dataPoint.Marker.ScaleFactor;
                        }

                        forcedAutoPlacement = true;
                    }
                    else if (currPoint.Y + textBlockSize.Height + dataPoint.Marker.MarkerSize.Height / 2 > plotHeight || dataPoint.LabelStyle == LabelStyles.Inside)
                    {
                        if (currPoint.X + textBlockSize.Width > plotWidth && prevPoint.Y - currPoint.Y <= 50 && textBlockSize.Width < 50)
                        {
                            labelLeft = currPoint.X - textBlockSize.Width / 2;// +dataPoint.Marker.MarkerSize.Width / 2 * dataPoint.Marker.ScaleFactor;
                            labelTop = currPoint.Y - GAP / 2 - textBlockSize.Height - dataPoint.Marker.MarkerSize.Height / 2 * dataPoint.Marker.ScaleFactor;
                        }
                        else if ((currPoint.X + textBlockSize.Width > plotWidth && prevPoint.Y - currPoint.Y > 50) || currPoint.X + textBlockSize.Width > plotWidth)
                        {
                            //dataPoint.Marker.TextAlignmentX = AlignmentX.Left;
                            //dataPoint.Marker.TextAlignmentY = AlignmentY.Bottom;
                            labelLeft = currPoint.X - GAP - textBlockSize.Width - dataPoint.Marker.MarkerSize.Width / 2 * dataPoint.Marker.ScaleFactor; ;// +dataPoint.Marker.MarkerSize.Width / 2 * dataPoint.Marker.ScaleFactor;
                            labelTop = currPoint.Y + GAP - textBlockSize.Height - dataPoint.Marker.MarkerSize.Height / 2 * dataPoint.Marker.ScaleFactor;
                        }
                        else if ((prevPoint.Y > currPoint.Y && nextPoint.Y > currPoint.Y) && prevPoint.Y - currPoint.Y > 20 && prevPoint.Y - currPoint.Y > nextPoint.Y - currPoint.Y)
                        {
                            labelLeft = currPoint.X - textBlockSize.Width - GAP - dataPoint.Marker.MarkerSize.Width / 2 * dataPoint.Marker.ScaleFactor; ;// +dataPoint.Marker.MarkerSize.Width / 2 * dataPoint.Marker.ScaleFactor;
                            labelTop = currPoint.Y + GAP - textBlockSize.Height - dataPoint.Marker.MarkerSize.Height / 2 * dataPoint.Marker.ScaleFactor;
                        }
                        else if (nextPoint.X - currPoint.X > 120 && textBlockSize.Width < 50 && prevPoint.X == 0)
                        {
                            labelLeft = currPoint.X - textBlockSize.Width / 2;// +dataPoint.Marker.MarkerSize.Width / 2 * dataPoint.Marker.ScaleFactor;
                            labelTop = currPoint.Y - GAP - textBlockSize.Height - dataPoint.Marker.MarkerSize.Height / 2 * dataPoint.Marker.ScaleFactor;
                        }
                        else if (currPoint.Y > prevPoint.Y && currPoint.Y - prevPoint.Y > currPoint.Y - nextPoint.Y && prevPoint.X != 0)
                        {
                            labelLeft = currPoint.X - textBlockSize.Width - GAP - dataPoint.Marker.MarkerSize.Width / 2 * dataPoint.Marker.ScaleFactor; ;// +dataPoint.Marker.MarkerSize.Width / 2 * dataPoint.Marker.ScaleFactor;
                            labelTop = currPoint.Y + GAP - textBlockSize.Height - dataPoint.Marker.MarkerSize.Height / 2 * dataPoint.Marker.ScaleFactor;
                        }
                        else if (currPoint.Y >= nextPoint.Y && prevPoint == new Point(0, 0))
                        {
                            labelLeft = currPoint.X - textBlockSize.Width / 2;// +dataPoint.Marker.MarkerSize.Width / 2 * dataPoint.Marker.ScaleFactor;
                            labelTop = currPoint.Y - GAP - textBlockSize.Height - dataPoint.Marker.MarkerSize.Height / 2 * dataPoint.Marker.ScaleFactor;
                        }
                        else if (currPoint.Y >= nextPoint.Y && prevPoint.Y >= currPoint.Y)
                        {
                            if (currPoint.Y - nextPoint.Y >= 10)
                            {
                                labelLeft = currPoint.X + GAP + dataPoint.Marker.MarkerSize.Width / 2 * dataPoint.Marker.ScaleFactor;
                                labelTop = currPoint.Y + GAP - textBlockSize.Height - dataPoint.Marker.MarkerSize.Height / 2 * dataPoint.Marker.ScaleFactor;
                            }
                            else
                            {
                                labelLeft = currPoint.X - textBlockSize.Width / 2;// +dataPoint.Marker.MarkerSize.Width / 2 * dataPoint.Marker.ScaleFactor;
                                labelTop = currPoint.Y - GAP - textBlockSize.Height - dataPoint.Marker.MarkerSize.Height / 2 * dataPoint.Marker.ScaleFactor;
                            }
                        }
                        else if (currPoint.Y <= nextPoint.Y && prevPoint.Y >= currPoint.Y)
                        {
                            if (textBlockSize.Width < 20)
                            {
                                labelLeft = currPoint.X - textBlockSize.Width / 2;// +dataPoint.Marker.MarkerSize.Width / 2 * dataPoint.Marker.ScaleFactor;
                                labelTop = currPoint.Y - GAP - textBlockSize.Height - dataPoint.Marker.MarkerSize.Height / 2 * dataPoint.Marker.ScaleFactor;
                            }
                            else
                            {
                                labelLeft = currPoint.X + GAP + dataPoint.Marker.MarkerSize.Width / 2 * dataPoint.Marker.ScaleFactor;
                                labelTop = currPoint.Y + GAP - textBlockSize.Height - dataPoint.Marker.MarkerSize.Height / 2 * dataPoint.Marker.ScaleFactor;
                            }
                        }
                        else if (prevPoint == new Point(0, 0) && (currPoint.X - textBlockSize.Width / 2) > 0)
                        {
                            labelLeft = currPoint.X - textBlockSize.Width / 2;// +dataPoint.Marker.MarkerSize.Width / 2 * dataPoint.Marker.ScaleFactor;
                            labelTop = currPoint.Y - GAP - textBlockSize.Height - dataPoint.Marker.MarkerSize.Height / 2 * dataPoint.Marker.ScaleFactor;
                        }
                        else
                        {
                            labelLeft = currPoint.X + GAP + dataPoint.Marker.MarkerSize.Width / 2 * dataPoint.Marker.ScaleFactor; ;
                            labelTop = currPoint.Y + GAP - textBlockSize.Height - dataPoint.Marker.MarkerSize.Height / 2 * dataPoint.Marker.ScaleFactor;
                        }

                        forcedAutoPlacement = true;
                    }
                }
                else if (dataPoint.LabelStyle == LabelStyles.OutSide)
                {
                    labelLeft = currPoint.X - textBlockSize.Width / 2;// +dataPoint.Marker.MarkerSize.Width / 2 * dataPoint.Marker.ScaleFactor;
                    labelTop = currPoint.Y - GAP / 2 - textBlockSize.Height - dataPoint.Marker.MarkerSize.Height / 2 * dataPoint.Marker.ScaleFactor;
                    //dataPoint.Marker.TextAlignmentY = AlignmentY.Top;
                    forcedAutoPlacement = true;
                }
                else
                {
                     labelLeft = currPoint.X - textBlockSize.Width / 2;// +dataPoint.Marker.MarkerSize.Width / 2 * dataPoint.Marker.ScaleFactor;
                     labelTop = currPoint.Y + GAP / 2 + dataPoint.Marker.MarkerSize.Height / 2 * dataPoint.Marker.ScaleFactor;
                     //dataPoint.Marker.TextAlignmentY = AlignmentY.Bottom;
                    forcedAutoPlacement = true;
                }
            }
            else
            {
                if (dataPoint.LabelStyle == LabelStyles.OutSide && !dataPoint.IsLabelStyleSet && !dataPoint.Parent.IsLabelStyleSet)
                {
                    if (currPoint.Y + textBlockSize.Height + dataPoint.Marker.MarkerSize.Height / 2 > plotHeight || dataPoint.LabelStyle == LabelStyles.Inside)
                    {
                        if (currPoint.X + textBlockSize.Width > plotWidth && prevPoint.Y - currPoint.Y <= 50 && textBlockSize.Width < 50)
                        {
                            labelLeft = currPoint.X - textBlockSize.Width / 2;// +dataPoint.Marker.MarkerSize.Width / 2 * dataPoint.Marker.ScaleFactor;
                            labelTop = currPoint.Y - GAP - textBlockSize.Height - dataPoint.Marker.MarkerSize.Height / 2 * dataPoint.Marker.ScaleFactor;
                        }
                        else if ((currPoint.X + textBlockSize.Width > plotWidth && prevPoint.Y - currPoint.Y > 50) || currPoint.X + textBlockSize.Width > plotWidth)
                        {
                            //dataPoint.Marker.TextAlignmentX = AlignmentX.Left;
                            //dataPoint.Marker.TextAlignmentY = AlignmentY.Bottom;
                            labelLeft = currPoint.X - GAP - textBlockSize.Width - dataPoint.Marker.MarkerSize.Width / 2 * dataPoint.Marker.ScaleFactor; ;// +dataPoint.Marker.MarkerSize.Width / 2 * dataPoint.Marker.ScaleFactor;
                            labelTop = currPoint.Y + GAP - textBlockSize.Height - dataPoint.Marker.MarkerSize.Height / 2 * dataPoint.Marker.ScaleFactor;
                        }
                        else if ((prevPoint.Y > currPoint.Y && nextPoint.Y > currPoint.Y) && prevPoint.Y - currPoint.Y > 20 && prevPoint.Y - currPoint.Y > nextPoint.Y - currPoint.Y)
                        {
                            labelLeft = currPoint.X - textBlockSize.Width - GAP - dataPoint.Marker.MarkerSize.Width / 2 * dataPoint.Marker.ScaleFactor; ;// +dataPoint.Marker.MarkerSize.Width / 2 * dataPoint.Marker.ScaleFactor;
                            labelTop = currPoint.Y + GAP - textBlockSize.Height - dataPoint.Marker.MarkerSize.Height / 2 * dataPoint.Marker.ScaleFactor;
                        }
                        else if (nextPoint.X - currPoint.X > 100 && textBlockSize.Width < 50 && prevPoint.X == 0)
                        {
                            labelLeft = currPoint.X - textBlockSize.Width / 2;// +dataPoint.Marker.MarkerSize.Width / 2 * dataPoint.Marker.ScaleFactor;
                            labelTop = currPoint.Y - GAP - textBlockSize.Height - dataPoint.Marker.MarkerSize.Height / 2 * dataPoint.Marker.ScaleFactor;
                        }
                        else if (currPoint.Y > prevPoint.Y && currPoint.Y > nextPoint.Y && currPoint.Y - prevPoint.Y > currPoint.Y - nextPoint.Y && prevPoint.X != 0)
                        {
                            labelLeft = currPoint.X - textBlockSize.Width - GAP - dataPoint.Marker.MarkerSize.Width / 2 * dataPoint.Marker.ScaleFactor; ;// +dataPoint.Marker.MarkerSize.Width / 2 * dataPoint.Marker.ScaleFactor;
                            labelTop = currPoint.Y + GAP - textBlockSize.Height - dataPoint.Marker.MarkerSize.Height / 2 * dataPoint.Marker.ScaleFactor;
                        }
                        else if (currPoint.Y >= nextPoint.Y && prevPoint == new Point(0, 0))
                        {
                            labelLeft = currPoint.X - textBlockSize.Width / 2;// +dataPoint.Marker.MarkerSize.Width / 2 * dataPoint.Marker.ScaleFactor;
                            labelTop = currPoint.Y - GAP - textBlockSize.Height - dataPoint.Marker.MarkerSize.Height / 2 * dataPoint.Marker.ScaleFactor;
                        }
                        else if (currPoint.Y >= nextPoint.Y && prevPoint.Y >= currPoint.Y)
                        {
                            if (currPoint.Y - nextPoint.Y >= 10)
                            {
                                labelLeft = currPoint.X + GAP + dataPoint.Marker.MarkerSize.Width / 2 * dataPoint.Marker.ScaleFactor;
                                labelTop = currPoint.Y + GAP - textBlockSize.Height - dataPoint.Marker.MarkerSize.Height / 2 * dataPoint.Marker.ScaleFactor;
                            }
                            else
                            {
                                labelLeft = currPoint.X - textBlockSize.Width / 2;// +dataPoint.Marker.MarkerSize.Width / 2 * dataPoint.Marker.ScaleFactor;
                                labelTop = currPoint.Y - GAP - textBlockSize.Height - dataPoint.Marker.MarkerSize.Height / 2 * dataPoint.Marker.ScaleFactor;
                            }
                        }
                        else if (currPoint.Y <= nextPoint.Y && prevPoint.Y >= currPoint.Y)
                        {
                            if (textBlockSize.Width < 20 || prevPoint.Y - currPoint.Y < 20)
                            {
                                labelLeft = currPoint.X - textBlockSize.Width / 2;// +dataPoint.Marker.MarkerSize.Width / 2 * dataPoint.Marker.ScaleFactor;
                                labelTop = currPoint.Y - GAP - textBlockSize.Height - dataPoint.Marker.MarkerSize.Height / 2 * dataPoint.Marker.ScaleFactor;
                            }
                            else
                            {
                                labelLeft = currPoint.X + GAP + dataPoint.Marker.MarkerSize.Width / 2 * dataPoint.Marker.ScaleFactor;
                                labelTop = currPoint.Y + GAP - textBlockSize.Height - dataPoint.Marker.MarkerSize.Height / 2 * dataPoint.Marker.ScaleFactor;
                            }
                        }
                        else if (prevPoint == new Point(0, 0) && (currPoint.X - textBlockSize.Width / 2) > 0)
                        {
                            labelLeft = currPoint.X - textBlockSize.Width / 2;// +dataPoint.Marker.MarkerSize.Width / 2 * dataPoint.Marker.ScaleFactor;
                            labelTop = currPoint.Y - GAP - textBlockSize.Height - dataPoint.Marker.MarkerSize.Height / 2 * dataPoint.Marker.ScaleFactor;
                        }
                        else
                        {
                            labelLeft = currPoint.X + GAP + dataPoint.Marker.MarkerSize.Width / 2 * dataPoint.Marker.ScaleFactor; ;
                            labelTop = currPoint.Y + GAP - textBlockSize.Height - dataPoint.Marker.MarkerSize.Height / 2 * dataPoint.Marker.ScaleFactor;
                        }

                        forcedAutoPlacement = true;
                    }
                    else if (currPoint.Y - textBlockSize.Height - dataPoint.Marker.MarkerSize.Height / 2 < 0 || dataPoint.LabelStyle == LabelStyles.Inside)
                    {
                        if (currPoint.X + textBlockSize.Width > plotWidth && prevPoint.Y - currPoint.Y <= 50 && textBlockSize.Width < 50)
                        {
                            labelLeft = currPoint.X - textBlockSize.Width / 2;// +dataPoint.Marker.MarkerSize.Width / 2 * dataPoint.Marker.ScaleFactor;
                            labelTop = currPoint.Y + GAP + dataPoint.Marker.MarkerSize.Height / 2 * dataPoint.Marker.ScaleFactor;
                        }
                        else if ((currPoint.X + textBlockSize.Width > plotWidth && prevPoint.Y - currPoint.Y > 50) || currPoint.X + textBlockSize.Width > plotWidth)
                        {
                            //dataPoint.Marker.TextAlignmentX = AlignmentX.Left;
                            //dataPoint.Marker.TextAlignmentY = AlignmentY.Bottom;
                            labelLeft = currPoint.X - textBlockSize.Width - GAP - dataPoint.Marker.MarkerSize.Width / 2 * dataPoint.Marker.ScaleFactor; ;// +dataPoint.Marker.MarkerSize.Width / 2 * dataPoint.Marker.ScaleFactor;
                            labelTop = currPoint.Y - GAP + dataPoint.Marker.MarkerSize.Height / 2 * dataPoint.Marker.ScaleFactor;
                        }
                        else if ((prevPoint.Y > currPoint.Y && nextPoint.Y > currPoint.Y) && prevPoint.Y - currPoint.Y > 20 && prevPoint.Y - currPoint.Y > nextPoint.Y - currPoint.Y)
                        {
                            labelLeft = currPoint.X - textBlockSize.Width - GAP - dataPoint.Marker.MarkerSize.Width / 2 * dataPoint.Marker.ScaleFactor; ;// +dataPoint.Marker.MarkerSize.Width / 2 * dataPoint.Marker.ScaleFactor;
                            labelTop = currPoint.Y - GAP + dataPoint.Marker.MarkerSize.Height / 2 * dataPoint.Marker.ScaleFactor;
                        }
                        else if (nextPoint.X - currPoint.X > 100 && textBlockSize.Width < 50 && nextPoint.Y - currPoint.Y < 20)
                        {
                            labelLeft = currPoint.X - textBlockSize.Width / 2;// +dataPoint.Marker.MarkerSize.Width / 2 * dataPoint.Marker.ScaleFactor;
                            labelTop = currPoint.Y + GAP + dataPoint.Marker.MarkerSize.Height / 2 * dataPoint.Marker.ScaleFactor;
                        }
                        else if (currPoint.Y > prevPoint.Y && currPoint.Y - prevPoint.Y > currPoint.Y - nextPoint.Y && prevPoint.X != 0)
                        {
                            labelLeft = currPoint.X - textBlockSize.Width - GAP - dataPoint.Marker.MarkerSize.Width / 2 * dataPoint.Marker.ScaleFactor; ;// +dataPoint.Marker.MarkerSize.Width / 2 * dataPoint.Marker.ScaleFactor;
                            labelTop = currPoint.Y - GAP + dataPoint.Marker.MarkerSize.Height / 2 * dataPoint.Marker.ScaleFactor;
                        }
                        else if (currPoint.Y >= nextPoint.Y && prevPoint == new Point(0, 0))
                        {
                            labelLeft = currPoint.X - textBlockSize.Width / 2;// +dataPoint.Marker.MarkerSize.Width / 2 * dataPoint.Marker.ScaleFactor;
                            labelTop = currPoint.Y + GAP / 2 + dataPoint.Marker.MarkerSize.Height / 2 * dataPoint.Marker.ScaleFactor;
                        }
                        else if (currPoint.Y >= nextPoint.Y && prevPoint.Y >= currPoint.Y)
                        {
                            labelLeft = currPoint.X + GAP + dataPoint.Marker.MarkerSize.Width / 2 * dataPoint.Marker.ScaleFactor;
                            labelTop = currPoint.Y + GAP / 2 + dataPoint.Marker.MarkerSize.Height / 2 * dataPoint.Marker.ScaleFactor;
                        }
                        else if (currPoint.Y <= nextPoint.Y && prevPoint.Y >= currPoint.Y)
                        {
                            if (textBlockSize.Width < 15)
                            {
                                labelLeft = currPoint.X - textBlockSize.Width / 2;// +dataPoint.Marker.MarkerSize.Width / 2 * dataPoint.Marker.ScaleFactor;
                                labelTop = currPoint.Y + GAP / 2 + dataPoint.Marker.MarkerSize.Height / 2 * dataPoint.Marker.ScaleFactor;
                            }
                            else
                            {
                                labelLeft = currPoint.X + GAP + dataPoint.Marker.MarkerSize.Width / 2 * dataPoint.Marker.ScaleFactor;
                                labelTop = currPoint.Y + dataPoint.Marker.MarkerSize.Height / 2 * dataPoint.Marker.ScaleFactor;
                            }
                        }
                        else if (prevPoint == new Point(0, 0) && (currPoint.X - textBlockSize.Width / 2) > 0)
                        {
                            labelLeft = currPoint.X - textBlockSize.Width / 2;// +dataPoint.Marker.MarkerSize.Width / 2 * dataPoint.Marker.ScaleFactor;
                            labelTop = currPoint.Y + GAP / 2 + dataPoint.Marker.MarkerSize.Height / 2 * dataPoint.Marker.ScaleFactor;
                        }
                        else
                        {
                            labelLeft = currPoint.X + GAP + dataPoint.Marker.MarkerSize.Width / 2 * dataPoint.Marker.ScaleFactor;
                            labelTop = currPoint.Y - GAP + dataPoint.Marker.MarkerSize.Height / 2 * dataPoint.Marker.ScaleFactor;
                        }

                        forcedAutoPlacement = true;
                    }
                }
                else if (dataPoint.LabelStyle == LabelStyles.OutSide)
                {
                    labelLeft = currPoint.X - textBlockSize.Width / 2;// +dataPoint.Marker.MarkerSize.Width / 2 * dataPoint.Marker.ScaleFactor;
                    labelTop = currPoint.Y + GAP / 2 + dataPoint.Marker.MarkerSize.Height / 2 * dataPoint.Marker.ScaleFactor;
                    forcedAutoPlacement = true;
                    //dataPoint.Marker.TextAlignmentY = AlignmentY.Bottom;
                }
                else
                {
                    labelLeft = currPoint.X - textBlockSize.Width / 2;// +dataPoint.Marker.MarkerSize.Width / 2 * dataPoint.Marker.ScaleFactor;
                    labelTop = currPoint.Y - GAP / 2 - textBlockSize.Height - dataPoint.Marker.MarkerSize.Height / 2 * dataPoint.Marker.ScaleFactor;
                    forcedAutoPlacement = true;
                    //dataPoint.Marker.TextAlignmentY = AlignmentY.Top;
                }
            }


            if (!forcedAutoPlacement && dataPoint.LabelStyle == LabelStyles.OutSide && !dataPoint.IsLabelStyleSet && !dataPoint.Parent.IsLabelStyleSet)
            {
                if (prevPoint.Y <= currPoint.Y && (currPoint.X + textBlockSize.Width >= plotWidth) && (nextPoint.X == 0 && nextPoint.Y == 0))
                {
                    //dataPoint.Marker.TextAlignmentX = AlignmentX.Left;
                    //dataPoint.Marker.TextAlignmentY = AlignmentY.Bottom;
                    labelLeft = currPoint.X - textBlockSize.Width - dataPoint.Marker.MarkerSize.Width / 2 * dataPoint.Marker.ScaleFactor;// +dataPoint.Marker.MarkerSize.Width / 2 * dataPoint.Marker.ScaleFactor;
                    labelTop = currPoint.Y + dataPoint.Marker.MarkerSize.Height / 2 * dataPoint.Marker.ScaleFactor;
                }
                else if (prevPoint.Y > currPoint.Y && (currPoint.X + textBlockSize.Width > plotWidth) && (nextPoint.X == 0 && nextPoint.Y == 0))
                {
                    //dataPoint.Marker.TextAlignmentX = AlignmentX.Left;
                    //dataPoint.Marker.TextAlignmentY = AlignmentY.Top;
                    labelLeft = currPoint.X - textBlockSize.Width - dataPoint.Marker.MarkerSize.Width / 2 * dataPoint.Marker.ScaleFactor;// +dataPoint.Marker.MarkerSize.Width / 2 * dataPoint.Marker.ScaleFactor;
                    labelTop = currPoint.Y - textBlockSize.Height - dataPoint.Marker.MarkerSize.Height / 2 * dataPoint.Marker.ScaleFactor;
                }
                else if (currPoint.X + textBlockSize.Width > plotWidth)
                {
                    //dataPoint.Marker.TextAlignmentX = AlignmentX.Left;
                    //dataPoint.Marker.TextAlignmentY = AlignmentY.Center;
                    labelLeft = currPoint.X - GAP - textBlockSize.Width - dataPoint.Marker.MarkerSize.Width / 2 * dataPoint.Marker.ScaleFactor;// +dataPoint.Marker.MarkerSize.Width / 2 * dataPoint.Marker.ScaleFactor;
                    labelTop = currPoint.Y - textBlockSize.Height / 2;// +dataPoint.Marker.MarkerSize.Height / 2 * dataPoint.Marker.ScaleFactor;
                }
                else if (prevPoint.Y <= currPoint.Y && nextPoint.Y <= currPoint.Y && (nextPoint != new Point(0, 0)) && (prevPoint != new Point(0, 0)))
                {
                    //dataPoint.Marker.TextAlignmentX = AlignmentX.Center;
                    //dataPoint.Marker.TextAlignmentY = AlignmentY.Bottom;
                    labelLeft = currPoint.X - textBlockSize.Width / 2;// +dataPoint.Marker.MarkerSize.Width / 2 * dataPoint.Marker.ScaleFactor;
                    labelTop = currPoint.Y + GAP / 2 + dataPoint.Marker.MarkerSize.Height / 2 * dataPoint.Marker.ScaleFactor;
                }
                else if (prevPoint.Y > currPoint.Y && nextPoint.Y > currPoint.Y)
                {
                    //dataPoint.Marker.TextAlignmentX = AlignmentX.Center;
                    //dataPoint.Marker.TextAlignmentY = AlignmentY.Top;
                    labelLeft = currPoint.X - textBlockSize.Width / 2;// +dataPoint.Marker.MarkerSize.Width / 2 * dataPoint.Marker.ScaleFactor;
                    labelTop = currPoint.Y - GAP / 2 - textBlockSize.Height - dataPoint.Marker.MarkerSize.Height / 2 * dataPoint.Marker.ScaleFactor;
                }
                else if ((prevPoint.X == 0 && prevPoint.Y == 0))
                {
                    if (currPoint.Y > nextPoint.Y && (currPoint.Y - nextPoint.Y > 20) && currPoint.X - textBlockSize.Width / 2 < 0)
                    {
                        labelLeft = currPoint.X + dataPoint.Marker.MarkerSize.Width / 2 * dataPoint.Marker.ScaleFactor;
                        labelTop = currPoint.Y + GAP / 2 + dataPoint.Marker.MarkerSize.Height / 2 * dataPoint.Marker.ScaleFactor;
                    }
                    else if (currPoint.Y > nextPoint.Y && (currPoint.Y - nextPoint.Y > 20))
                    {
                        if (currPoint.X - textBlockSize.Width >= 0)
                        {
                            labelLeft = currPoint.X - textBlockSize.Width - dataPoint.Marker.MarkerSize.Width / 2 * dataPoint.Marker.ScaleFactor;// +dataPoint.Marker.MarkerSize.Width / 2 * dataPoint.Marker.ScaleFactor;
                            labelTop = currPoint.Y - GAP / 2 - textBlockSize.Height - dataPoint.Marker.MarkerSize.Height / 2 * dataPoint.Marker.ScaleFactor;
                        }
                        else
                        {
                            labelLeft = currPoint.X - textBlockSize.Width / 2;// +dataPoint.Marker.MarkerSize.Width / 2 * dataPoint.Marker.ScaleFactor;
                            labelTop = currPoint.Y + GAP / 2 + dataPoint.Marker.MarkerSize.Height / 2 * dataPoint.Marker.ScaleFactor;
                        }
                    }
                    else if(currPoint.X - textBlockSize.Width / 2 > 0)
                    {
                        labelLeft = currPoint.X - textBlockSize.Width / 2;// +dataPoint.Marker.MarkerSize.Width / 2 * dataPoint.Marker.ScaleFactor;
                        labelTop = currPoint.Y - GAP / 2 - textBlockSize.Height - dataPoint.Marker.MarkerSize.Height / 2 * dataPoint.Marker.ScaleFactor;
                    }
                    else if (currPoint.Y > nextPoint.Y)
                    {
                        labelLeft = currPoint.X + dataPoint.Marker.MarkerSize.Width / 2 * dataPoint.Marker.ScaleFactor;
                        labelTop = currPoint.Y + dataPoint.Marker.MarkerSize.Height / 2 * dataPoint.Marker.ScaleFactor;
                    }
                    else
                    {
                        //dataPoint.Marker.TextAlignmentX = AlignmentX.Right;
                        //dataPoint.Marker.TextAlignmentY = AlignmentY.Top;
                        labelLeft = currPoint.X + dataPoint.Marker.MarkerSize.Width / 2 * dataPoint.Marker.ScaleFactor;
                        labelTop = currPoint.Y - textBlockSize.Height - dataPoint.Marker.MarkerSize.Height / 2 * dataPoint.Marker.ScaleFactor;
                    }
                }
                else if (nextPoint.X == 0 && nextPoint.Y == 0 && currPoint.X + textBlockSize.Width > plotWidth)
                {
                    if (currPoint.Y > prevPoint.Y)
                    {
                        labelLeft = currPoint.X - textBlockSize.Width / 2;// +dataPoint.Marker.MarkerSize.Width / 2 * dataPoint.Marker.ScaleFactor;
                        labelTop = currPoint.Y + GAP / 2 + dataPoint.Marker.MarkerSize.Height / 2 * dataPoint.Marker.ScaleFactor;
                    }
                    else
                    {
                        //dataPoint.Marker.TextAlignmentX = AlignmentX.Left;
                        //dataPoint.Marker.TextAlignmentY = AlignmentY.Top;
                        labelLeft = currPoint.X - textBlockSize.Width - dataPoint.Marker.MarkerSize.Width / 2 * dataPoint.Marker.ScaleFactor;// +dataPoint.Marker.MarkerSize.Width / 2 * dataPoint.Marker.ScaleFactor;
                        labelTop = currPoint.Y - GAP / 2 - textBlockSize.Height - dataPoint.Marker.MarkerSize.Height / 2 * dataPoint.Marker.ScaleFactor;
                    }
                }
                else if (nextPoint.X == 0 && nextPoint.Y == 0)
                {
                    //dataPoint.Marker.TextAlignmentX = AlignmentX.Right;
                    //dataPoint.Marker.TextAlignmentY = AlignmentY.Bottom;
                    labelLeft = currPoint.X + dataPoint.Marker.MarkerSize.Width / 2 * dataPoint.Marker.ScaleFactor;
                    labelTop = currPoint.Y - GAP / 2 - textBlockSize.Height - dataPoint.Marker.MarkerSize.Height / 2 * dataPoint.Marker.ScaleFactor;
                }
                else if ((prevPoint.Y <= currPoint.Y && nextPoint.Y >= currPoint.Y) || (prevPoint.Y > currPoint.Y && nextPoint.Y < currPoint.Y))
                {
                    //dataPoint.Marker.TextAlignmentX = AlignmentX.Right;
                    //dataPoint.Marker.TextAlignmentY = AlignmentY.Center;
                    if ((prevPoint.Y <= currPoint.Y && nextPoint.Y >= currPoint.Y) && (nextPoint.Y - currPoint.Y < 10 || prevPoint.Y < currPoint.Y))
                    {
                        labelLeft = currPoint.X + dataPoint.Marker.MarkerSize.Width / 2 * dataPoint.Marker.ScaleFactor;
                        labelTop = currPoint.Y - GAP / 2 - textBlockSize.Height - dataPoint.Marker.MarkerSize.Height / 2 * dataPoint.Marker.ScaleFactor;
                    }
                    else if (currPoint.Y - nextPoint.Y > 20)
                    {
                        labelLeft = currPoint.X + GAP + dataPoint.Marker.MarkerSize.Width / 2 * dataPoint.Marker.ScaleFactor;
                        labelTop = currPoint.Y - textBlockSize.Height / 2;// +dataPoint.Marker.MarkerSize.Height / 2 * dataPoint.Marker.ScaleFactor;
                    }
                    else
                    {
                        labelLeft = currPoint.X - textBlockSize.Width / 2;// +dataPoint.Marker.MarkerSize.Width / 2 * dataPoint.Marker.ScaleFactor;
                        labelTop = currPoint.Y - GAP / 2 - textBlockSize.Height - dataPoint.Marker.MarkerSize.Height / 2 * dataPoint.Marker.ScaleFactor;
                    }
                }
                else
                {
                    labelLeft = currPoint.X - textBlockSize.Width / 2;// +dataPoint.Marker.MarkerSize.Width / 2 * dataPoint.Marker.ScaleFactor;
                    labelTop = currPoint.Y - GAP / 2 - textBlockSize.Height - dataPoint.Marker.MarkerSize.Height / 2 * dataPoint.Marker.ScaleFactor;
                }
            }


            return new Point(labelLeft, labelTop);
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

            SolidColorBrush stroke = dataPoint.Marker.MarkerShape.Stroke as SolidColorBrush;

            if(!(stroke != null && stroke.Color.ToString().Equals(tarnsparentColor.ToString())))
                dataPoint.Marker.MarkerShape.Stroke = tarnsparentColor;

            if (dataPoint.Marker.MarkerShadow != null)
                dataPoint.Marker.MarkerShadow.Visibility = Visibility.Collapsed;

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
        private static Canvas GetLine2D(DataSeries tagReference, Double width, Double height, Canvas line2dLabelCanvas, LineChartShapeParams lineParams, out Path line, out Path lineShadow, List<List<DataPoint>> pointCollectionList, List<List<DataPoint>> shadowPointCollectionList)
        {   
            Canvas visual = new Canvas();
            line = new Path() { Tag = new ElementData() { Element = tagReference } };
            line.StrokeLineJoin = PenLineJoin.Round;

            line.StrokeStartLineCap = PenLineCap.Round;
            line.StrokeEndLineCap = PenLineCap.Round;

            line.Stroke = lineParams.Lighting ? Graphics.GetLightingEnabledBrush(lineParams.LineColor, "Linear", new Double[] { 0.65, 0.55 }) : lineParams.LineColor;
            line.StrokeThickness = lineParams.LineThickness;
            line.StrokeDashArray = lineParams.LineStyle;

            line.Data = GetPathGeometry(tagReference.RenderAs, null, pointCollectionList, false, width, height, line2dLabelCanvas);

            if (lineParams.ShadowEnabled)
            {
                if (!VisifireControl.IsMediaEffectsEnabled)
                {
                    lineShadow = new Path() { IsHitTestVisible = false };
                    lineShadow.Stroke = Graphics.GetLightingEnabledBrush(new SolidColorBrush(Colors.LightGray), "Linear", new Double[] { 0.65, 0.55 });
                    lineShadow.StrokeStartLineCap = PenLineCap.Round;
                    lineShadow.StrokeEndLineCap = PenLineCap.Round;
                    lineShadow.StrokeLineJoin = PenLineJoin.Round;
                    lineShadow.StrokeThickness = lineParams.LineThickness;
                    lineShadow.Opacity = 0.5;

                    if (lineParams.ShadowEnabled)
                        lineShadow.Data = GetPathGeometry(tagReference.RenderAs, null, shadowPointCollectionList, true, width, height, null);

                    TranslateTransform tt = new TranslateTransform() { X = 2, Y = 2 };
                    lineShadow.RenderTransform = tt;

                    visual.Children.Add(lineShadow);
                }
                else
                {
#if !WP
                    visual.Effect = ExtendedGraphics.GetShadowEffect(315, 2.5, 1);
#endif
                    lineShadow = null;
                }
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
        private static Geometry GetPathGeometry(RenderAs renderAs, GeometryGroup oldData, List<List<DataPoint>> dataPointCollectionList, Boolean isShadow, Double width, Double height, Canvas line2dLabelCanvas)
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

                Double xPosition = 0;
                Double yPosition = 0;

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
                    {   
                        pointCollection[0].Faces = faces;
                        faces.PreviousDataPoint = null;

                        if(pointCollection.Count > 1)
                            faces.NextDataPoint = pointCollection[1];

                        if (pointCollection[0].Marker != null && pointCollection[0].Marker.Visual != null)
                        {   
                            Point newMarkerPosition = pointCollection[0].Marker.CalculateActualPosition(pointCollection[0]._visualPosition.X, pointCollection[0]._visualPosition.Y, new Point(0.5, 0.5));

                            pointCollection[0].Marker.Visual.Visibility = Visibility.Visible;
                            pointCollection[0].Marker.Visual.SetValue(Canvas.TopProperty, newMarkerPosition.Y);
                            pointCollection[0].Marker.Visual.SetValue(Canvas.LeftProperty, newMarkerPosition.X);
                        }
                        else
                        {   
                            CreateMarkerAForLineDataPoint(pointCollection[0], width, height, ref line2dLabelCanvas, out xPosition, out yPosition);
                        }

                        if ((Boolean)pointCollection[0].LabelEnabled)
                        {   
                            if (pointCollection[0].LabelVisual != null)
                            {   
                                Double labelLeft = 0;
                                Double labelTop = 0;

                                SetLabelPosition4LineDataPoint(pointCollection[0], ref labelLeft, ref labelTop);

                                (((pointCollection[0].LabelVisual as Border).Child as Canvas).Children[0] as TextBlock).Text = pointCollection[0].TextParser(pointCollection[0].LabelText);
                                pointCollection[0].LabelVisual.Visibility = Visibility.Visible;
                                pointCollection[0].LabelVisual.SetValue(Canvas.LeftProperty, labelLeft);
                                pointCollection[0].LabelVisual.SetValue(Canvas.TopProperty, labelTop);
                            }
                            else
                            {   
                                CreateLabel4LineDataPoint(pointCollection[0], width, height, pointCollection[0].InternalYValue >= 0, xPosition, yPosition, ref line2dLabelCanvas, true);
                            }
                        }
                    }

                    if (renderAs == RenderAs.Spline)
                        GeneratePointCollections4SpLine(pointCollection, width, height, isShadow, pathFigure, ref line2dLabelCanvas);
                    else
                        GeneratePointCollections4Line(pointCollection, width, height, isShadow, pathFigure, ref line2dLabelCanvas);
                }

                geometry.Figures.Add(pathFigure);
                gg.Children.Add(geometry);
            }

            return gg;
        }
        
        private static void GeneratePointCollections4SpLine(List<DataPoint> pointCollection, Double width, Double height, Boolean isShadow, PathFigure pathFigure, ref Canvas line2dLabelCanvas)
        {   
            Point[] knotPoints = (from dp in pointCollection select dp._visualPosition).ToArray();

            // Get Bezier Spline Control Points.
            Point[] cp1, cp2;
            Bezier.GetCurveControlPoints(2, knotPoints, out cp1, out cp2);
            
            Double xPosition = 0;
            Double yPosition = 0;

            if (pointCollection.Count > 0 && pointCollection[0].Faces != null)
                pointCollection[0].Faces.DataContext = pointCollection;
            
            /* PolyLineSegment segment = new PolyLineSegment();
               segment.Points = GeneratePointCollection(segment, pointCollection, createFaces);
               pathFigure.Segments.Add(segment);
            */
            for (int i = 1; i < pointCollection.Count; i++)
            {   
                Faces faces = new Faces();
                faces.DataContext = pointCollection;

                BezierSegment segment = new BezierSegment()
                {      
                    Point1 = cp1[i - 1],
                    Point2 = cp2[i - 1],
                    Point3 = knotPoints[i]
                };

                //Graphics.DrawPointAt(segment.Point1, line2dLabelCanvas, Colors.Red);
                //Graphics.DrawPointAt(segment.Point2, line2dLabelCanvas, Colors.Green);
                //Graphics.DrawPointAt(segment.Point3, line2dLabelCanvas, Colors.Blue);

                faces.PreviousDataPoint = pointCollection[i - 1];

                if (!isShadow)
                {   
                    if (i != pointCollection.Count - 1)
                        faces.NextDataPoint = pointCollection[i + 1];
                    else
                        faces.NextDataPoint = null;
                }

                // Add LineSegment
                faces.Parts.Add(segment);

                // Add PathFigure
                faces.Parts.Add(pathFigure);

                if (isShadow)
                    pointCollection[i].ShadowFaces = faces;
                else
                    pointCollection[i].Faces = faces;

                pathFigure.Segments.Add(segment);

                if (!isShadow)
                {   
                    if (pointCollection[i].Marker != null && pointCollection[i].Marker.Visual != null)
                    {
                        Point newMarkerPosition = pointCollection[i].Marker.CalculateActualPosition(pointCollection[i]._visualPosition.X, pointCollection[i]._visualPosition.Y, new Point(0.5, 0.5));

                        pointCollection[i].Marker.Visual.Visibility = Visibility.Visible;
                        pointCollection[i].Marker.Visual.SetValue(Canvas.TopProperty, newMarkerPosition.Y);
                        pointCollection[i].Marker.Visual.SetValue(Canvas.LeftProperty, newMarkerPosition.X);
                    }
                    else
                    {   
                        CreateMarkerAForLineDataPoint(pointCollection[i], width, height, ref line2dLabelCanvas, out xPosition, out yPosition);
                    }

                    if ((Boolean)pointCollection[i].LabelEnabled)
                    {
                        if (pointCollection[i].LabelVisual != null)
                        {
                            Double labelLeft = 0;
                            Double labelTop = 0;

                            SetLabelPosition4LineDataPoint(pointCollection[i], ref labelLeft, ref labelTop);

                            (((pointCollection[i].LabelVisual as Border).Child as Canvas).Children[0] as TextBlock).Text = pointCollection[i].TextParser(pointCollection[i].LabelText);

                            pointCollection[i].LabelVisual.Visibility = Visibility.Visible;
                            pointCollection[i].LabelVisual.SetValue(Canvas.LeftProperty, labelLeft);
                            pointCollection[i].LabelVisual.SetValue(Canvas.TopProperty, labelTop);
                        }
                        else
                        {
                            CreateLabel4LineDataPoint(pointCollection[i], width, height, pointCollection[i].InternalYValue >= 0, xPosition, yPosition, ref line2dLabelCanvas, true);
                        }
                    }
                }
            }
        }


        private static void GeneratePointCollections4Line(List<DataPoint> pointCollection, Double width, Double height, Boolean isShadow, PathFigure pathFigure, ref Canvas line2dLabelCanvas)
        {              
            Double xPosition = 0;
            Double yPosition = 0;

            /*
             * PolyLineSegment segment = new PolyLineSegment();
               segment.Points = GeneratePointCollection(segment, pointCollection, createFaces);
               pathFigure.Segments.Add(segment);
            */

            for (int i = 1; i < pointCollection.Count; i++)
            {   
                LineSegment segment = new LineSegment();

                segment.Point = pointCollection[i]._visualPosition;

                Faces faces = new Faces();

                faces.PreviousDataPoint = pointCollection[i - 1];

                if(!isShadow)
                {
                    if (i != pointCollection.Count - 1)
                        faces.NextDataPoint = pointCollection[i + 1];
                    else
                        faces.NextDataPoint = null;
                }

                //Add LineSegment
                faces.Parts.Add(segment);

                // Add PathFigure
                faces.Parts.Add(pathFigure);

                if (isShadow)
                    pointCollection[i].ShadowFaces = faces;
                else
                    pointCollection[i].Faces = faces;

                pathFigure.Segments.Add(segment);

                if(!isShadow)
                {
                    if (pointCollection[i].Marker != null && pointCollection[i].Marker.Visual != null)
                    {
                        Point newMarkerPosition = pointCollection[i].Marker.CalculateActualPosition(pointCollection[i]._visualPosition.X, pointCollection[i]._visualPosition.Y, new Point(0.5, 0.5));

                        pointCollection[i].Marker.Visual.Visibility = Visibility.Visible;
                        pointCollection[i].Marker.Visual.SetValue(Canvas.TopProperty, newMarkerPosition.Y);
                        pointCollection[i].Marker.Visual.SetValue(Canvas.LeftProperty, newMarkerPosition.X);
                    }
                    else
                    {
                        CreateMarkerAForLineDataPoint(pointCollection[i], width, height, ref line2dLabelCanvas, out xPosition, out yPosition);
                    }

                    if ((Boolean)pointCollection[i].LabelEnabled)
                    {
                        if (pointCollection[i].LabelVisual != null)
                        {
                            Double labelLeft = 0;
                            Double labelTop = 0;

                            SetLabelPosition4LineDataPoint(pointCollection[i], ref labelLeft, ref labelTop);

                            (((pointCollection[i].LabelVisual as Border).Child as Canvas).Children[0] as TextBlock).Text = pointCollection[i].TextParser(pointCollection[i].LabelText);

                            pointCollection[i].LabelVisual.Visibility = Visibility.Visible;
                            pointCollection[i].LabelVisual.SetValue(Canvas.LeftProperty, labelLeft);
                            pointCollection[i].LabelVisual.SetValue(Canvas.TopProperty, labelTop);
                        }
                        else
                        {
                            CreateLabel4LineDataPoint(pointCollection[i], width, height, pointCollection[i].InternalYValue >= 0, xPosition, yPosition, ref line2dLabelCanvas, true);
                        }
                    }
                }
            }
        }

        public static void Update(ObservableObject sender, VcProperties property, object newValue, Boolean isAxisChanged)
        {   
            Boolean isDataPoint = sender.GetType().Equals(typeof(DataPoint));
            
            if (isDataPoint)
                UpdateDataPoint(sender as DataPoint, property, newValue);
            else
                UpdateDataSeries(sender as DataSeries, property, newValue);
        }

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
            else if (dataSeries.Faces == null && property == VcProperties.Enabled && (Boolean)newValue == true)
            {
                ColumnChart.Update(chart, dataSeries.RenderAs, (from ds in chart.InternalSeries where ds.RenderAs == RenderAs.Spline || ds.RenderAs == RenderAs.Line select ds).ToList());
                return;
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
                        linePath.Opacity = (Double)dataSeries.Opacity;
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
                            if (line2dCanvas.Parent == null) 
                            {

                                ColumnChart.Update(chart, dataSeries.RenderAs, (from ds in chart.InternalSeries where ds.RenderAs == RenderAs.Line || ds.RenderAs == RenderAs.Spline select ds).ToList());
                                return;
                            }
                            
                            line2dCanvas.Visibility = Visibility.Visible;
                            label2dCanvas.Visibility = Visibility.Visible;
                        }

                        chart._toolTip.Hide();

                        break;
                    }

                    goto RENDER_SERIES;

                case VcProperties.ShadowEnabled:
                case VcProperties.DataPoints:
                case VcProperties.YValue:
                case VcProperties.YValues:
                case VcProperties.XValue:
                case VcProperties.ViewportRangeEnabled:
                case VcProperties.DataPointUpdate:
                RENDER_SERIES:
                    if(dataSeries.RenderAs == RenderAs.Line)
                        UpdateLineSeries(dataSeries, width, height, label2dCanvas);
                    else
                        UpdateSplineSeries(property, dataSeries, width, height, label2dCanvas);
                break;
            }
        }

        /// <summary>
        /// Updates a spline DataSeries
        /// </summary>
        /// <param name="property">VcProperties</param>
        /// <param name="dataSeries">DataSeries</param>
        /// <param name="width">Width of the parent of the visual</param>
        /// <param name="height">Height of the parent of the visual</param>
        /// <param name="label2dCanvas">LabelCanvas</param>
        internal static void UpdateSplineSeries(VcProperties property, DataSeries dataSeries, Double width, Double height, Canvas label2dCanvas)
        {   
            Chart chart = dataSeries.Chart as Chart;
            Boolean isAnimatedUpdate = (Boolean) chart.AnimatedUpdate;

            if (dataSeries.Enabled == false)
                return;

            Canvas chartsCanvas = dataSeries.Faces.Visual.Parent as Canvas;
            Canvas labelsCanvas = dataSeries.Faces.LabelCanvas.Parent as Canvas;

            UpdateSizeOfTheParentCanvas(dataSeries, width, height);
                        
            // Groups of broken line segments
            List<List<DataPoint>> pointCollectionList = new List<List<DataPoint>>();
            
            HideAllMarkersAndLabels(dataSeries);

            List<DataPoint> viewPortDataPoints = RenderHelper.GetDataPointsUnderViewPort(dataSeries, false);

            LineChart.CreateGroupOfBrokenLineSegment(dataSeries, viewPortDataPoints, width, height, out pointCollectionList);

            if (property != VcProperties.DataPointUpdate || isAnimatedUpdate == false)
                ReDrawLineSplineSereis(dataSeries, pointCollectionList, width, height, label2dCanvas);

            if (property == VcProperties.DataPointUpdate && isAnimatedUpdate == true)
            {
                /* If old YValue of any DataPoint was NAN then its needed to redraw 
                the Spline of while doing PartialUpdate  */
                Boolean forceRedraw = false;
                foreach (List<DataPoint> pointList in pointCollectionList)
                {
                    foreach (DataPoint dp in pointList)
                        if (dp.Faces == null)
                        {
                            forceRedraw = true;
                            break;
                        }

                    if (forceRedraw)
                        break;
                }

                if(forceRedraw)
                    ReDrawLineSplineSereis(dataSeries, pointCollectionList, width, height, label2dCanvas);
                
                AnimateSpline(pointCollectionList);
            }

            dataSeries._movingMarker.Visibility = Visibility.Collapsed;

            Clip(chart, chartsCanvas, labelsCanvas, dataSeries.PlotGroup);
        }



        /// <summary>
        /// Redraw Line/Spline DataSeries
        /// </summary>
        /// <param name="dataSeries">DataSeries</param>
        /// <param name="pointCollectionList">List<List<DataPoint>></param>
        /// <param name="width">Width of the PlotArea</param>
        /// <param name="height">Height of the PlotArea</param>
        /// <param name="label2dCanvas">LabelCanvas reference</param>
        private static void ReDrawLineSplineSereis(DataSeries dataSeries, List<List<DataPoint>> pointCollectionList, Double width, Double height, Canvas label2dCanvas)
        {
            GeometryGroup gg = (dataSeries.Faces.Parts[0] as Path).Data as GeometryGroup;

            // Apply new Data for Line
            LineChart.GetPathGeometry(dataSeries.RenderAs, gg, pointCollectionList, false, width, height, label2dCanvas);

            LineChart.ApplyShadow(dataSeries, ref pointCollectionList, width, height, label2dCanvas);
        }


        /// <summary>
        /// Updates the size of parent canvas and label canvas
        /// </summary>
        /// <param name="dataSeries"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        private static void UpdateSizeOfTheParentCanvas(DataSeries dataSeries, Double width, Double height)
        {
            (dataSeries.Faces.Visual as Canvas).Width = width;
            (dataSeries.Faces.Visual as Canvas).Height = height;
            (dataSeries.Faces.LabelCanvas as Canvas).Width = width;
            (dataSeries.Faces.LabelCanvas as Canvas).Height = height;

            Canvas chartsCanvas = dataSeries.Faces.Visual.Parent as Canvas;
            Canvas labelsCanvas = dataSeries.Faces.LabelCanvas.Parent as Canvas;
            chartsCanvas.Width = width;
            chartsCanvas.Height = height;
            labelsCanvas.Width = width;
            labelsCanvas.Height = height;
        }

        /// <summary>
        /// Create group of broken line segment
        /// </summary>
        /// <param name="dataSeries">DataSeries</param>
        /// <param name="viewPortDataPoints">ViewPort DataPoints</param>
        /// <param name="width">Width</param>
        /// <param name="height">Height</param>
        /// <param name="pointCollectionList">List of group of DataPoints</param>
        private static void CreateGroupOfBrokenLineSegment(DataSeries dataSeries, List<DataPoint> viewPortDataPoints, Double width, Double height, out List<List<DataPoint>> pointCollectionList)
        {
            Chart chart = dataSeries.Chart as Chart;
            Axis axisX = dataSeries.PlotGroup.AxisX;
            Axis axisY = dataSeries.PlotGroup.AxisY;

            List<DataPoint> pc = new List<DataPoint>();
            pointCollectionList = new List<List<DataPoint>>();

            pointCollectionList.Add(pc);

            foreach (DataPoint dp in viewPortDataPoints)
            {
                if (dp.Enabled == false)
                {
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

                // Point newMarkerPosition;
                dp._visualPositionOld = dp._visualPosition;
                dp._visualPosition = new Point(x, y);

                pc.Add(dp);
            }
        }

        /// <summary>
        /// Generate new shadow for line and apply for line and Spline
        /// </summary>
        /// <param name="dataSeries"></param>
        /// <param name="pointCollectionList"></param>
        private static void ApplyShadow(DataSeries dataSeries, ref List<List<DataPoint>> pointCollectionList, Double width, Double height, Canvas label2dCanvas)
        {
            if (!VisifireControl.IsMediaEffectsEnabled)
            {   
                // Update GeometryGroup for shadow
                if (dataSeries.Faces.Parts[1] != null)
                {
                    if ((Boolean)dataSeries.ShadowEnabled)
                    {
                        (dataSeries.Faces.Parts[1] as Path).Visibility = Visibility.Visible;

                        // gg.Children.Clear();
                        GeometryGroup ggShadow = (dataSeries.Faces.Parts[1] as Path).Data as GeometryGroup;

                        // Apply new Data for Line
                        LineChart.GetPathGeometry(dataSeries.RenderAs, ggShadow, pointCollectionList, true, width, height, label2dCanvas);
                    }
                    else
                        (dataSeries.Faces.Parts[1] as Path).Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                if (dataSeries.Faces != null && dataSeries.Faces.Visual != null)
                {
#if !WP
                    if ((Boolean)dataSeries.ShadowEnabled)
                    {
                        dataSeries.Faces.Visual.Effect = ExtendedGraphics.GetShadowEffect(315, 2.5, 1);
                    }
                    else
                        dataSeries.Faces.Visual.Effect = null;
#endif
                }
            }
        }

        private static void HideAllMarkersAndLabels(DataSeries dataSeries)
        {
            foreach (DataPoint dp in dataSeries.InternalDataPoints)
            {
                if (dp.Marker != null && dp.Marker.Visual != null)
                    dp.Marker.Visual.Visibility = Visibility.Collapsed;

                if (dp.LabelVisual != null)
                    dp.LabelVisual.Visibility = Visibility.Collapsed;
            }
        }

        private static void AnimateSpline(List<List<DataPoint>> pointCollectionList)
        {
            Storyboard storyBoard = new Storyboard();
            
            foreach (List<DataPoint> pointCollection in pointCollectionList)
            {
                AnimateBezierCurves(pointCollection, storyBoard);
                AnimateMarkers4Spline(pointCollection, storyBoard);
                AnimateLabel4Spline(pointCollection, storyBoard);
            }

            storyBoard.Begin();
        }

        private static Point[] CollectOldControlPoints(Int32 index, List<DataPoint> pointCollection, out Point oldStartPoint)
        {
            DataPoint dp = pointCollection[index];
            BezierSegment bezierSeg = dp.Faces.Parts[0] as BezierSegment;
            PathFigure pathFigure = dp.Faces.Parts[1] as PathFigure;
            Point[] cPoints = new Point[3];

            oldStartPoint = pathFigure.StartPoint;

            if (bezierSeg != null)
                return new Point[] { bezierSeg.Point1, bezierSeg.Point2, bezierSeg.Point3 };
            else
                return null;
        }

        private static void AnimateBezierCurves(List<DataPoint> pointCollection, Storyboard storyBoard)
        {
            Boolean shadowEnabled = (pointCollection.Count > 0) ? (Boolean)pointCollection[0].Parent.ShadowEnabled : false;

            // Animate Spline Bezier Curve
            Point[] newKnotPoints = (from dp in pointCollection select dp._visualPosition).ToArray();
            Point[] newCPoints1, newCPoints2; // Get Bezier Spline Control Points.
            Bezier.GetCurveControlPoints(2, newKnotPoints, out newCPoints1, out newCPoints2);

            for (Int32 dpIndex = 1; dpIndex < pointCollection.Count; dpIndex++)
            {
                Point oldStartPoint;
                DataPoint dp = pointCollection[dpIndex];
                Point[] oldBezierPoints = CollectOldControlPoints(dpIndex, pointCollection, out oldStartPoint);
                Point[] newBezierPoints = new Point[3] { newCPoints1[dpIndex - 1], newCPoints2[dpIndex - 1], newKnotPoints[dpIndex] };

                BezierSegment bezierSeg = dp.Faces.Parts[0] as BezierSegment;
                PathFigure pathFigure = dp.Faces.Parts[1] as PathFigure;
                               
                ApplyAnimationToBezierSegments(dpIndex, storyBoard, bezierSeg, oldBezierPoints,
                    newBezierPoints, pathFigure, oldStartPoint, pointCollection[0]._visualPosition);

                // Animate Shadow for Spline for Xbap
                if (shadowEnabled && !VisifireControl.IsMediaEffectsEnabled)
                {
                    bezierSeg = dp.ShadowFaces.Parts[0] as BezierSegment;
                    pathFigure = dp.ShadowFaces.Parts[1] as PathFigure;

                    ApplyAnimationToBezierSegments(dpIndex, storyBoard, bezierSeg, oldBezierPoints,
                        newBezierPoints, pathFigure, oldStartPoint, pointCollection[0]._visualPosition);
                }
            }
        }

        private static void AnimateLabel4Spline(List<DataPoint> pointCollection, Storyboard storyBoard)
        {
             // Animate Marker
            foreach (DataPoint dataPoint in pointCollection)
            {
                if ((Boolean)dataPoint.LabelEnabled)
                {
                    FrameworkElement labelVisual = dataPoint.LabelVisual;

                    if (labelVisual != null)
                    {
                        Point oldLabelPosition = new Point((Double)labelVisual.GetValue(Canvas.LeftProperty), (Double)labelVisual.GetValue(Canvas.TopProperty));

                        Double newMarkerXPos = 0, newMarkerYPos = 0;
                        Point newLabelPosition = SetLabelPosition4LineDataPoint(dataPoint, ref newMarkerXPos, ref newMarkerYPos);

                        //// Set the labels to its old position
                        //labelVisual.SetValue(Canvas.LeftProperty, oldLabelPosition.X);
                        //labelVisual.SetValue(Canvas.TopProperty, oldLabelPosition.Y);

                        labelVisual.Visibility = Visibility.Visible;

                        if (!oldLabelPosition.Equals(newLabelPosition))
                        {   
                            // Animation for (Canvas.Top) property
                            DoubleAnimation da = new DoubleAnimation()
                            {
                                From = oldLabelPosition.X,
                                To = newLabelPosition.X,
                                Duration = new Duration(new TimeSpan(0, 0, 1)),
                                SpeedRatio = 2
                            };

                            Storyboard.SetTarget(da, labelVisual);
                            Storyboard.SetTargetProperty(da, new PropertyPath("(Canvas.Left)"));
                            Storyboard.SetTargetName(da, (String)labelVisual.GetValue(FrameworkElement.NameProperty));

                            storyBoard.Children.Add(da);

                            // Animation for (Canvas.Top) property
                            da = new DoubleAnimation()
                            {
                                From = oldLabelPosition.Y,
                                To = newLabelPosition.Y,
                                Duration = new Duration(new TimeSpan(0, 0, 1)),
                                SpeedRatio = 2
                            };

                            Storyboard.SetTarget(da, labelVisual);
                            Storyboard.SetTargetProperty(da, new PropertyPath("(Canvas.Top)"));
                            Storyboard.SetTargetName(da, (String)labelVisual.GetValue(FrameworkElement.NameProperty));

                            storyBoard.Children.Add(da);
                        }
                    }
                }
            }
        }

        private static void AnimateMarkers4Spline(List<DataPoint> pointCollection, Storyboard storyBoard)
        {   
            // Animate Marker
            foreach (DataPoint dataPoint in pointCollection)
            {
                if ((Boolean)dataPoint.MarkerEnabled)
                {
                    #region Attach Animation with Marker

                    FrameworkElement marker = dataPoint.Marker.Visual;

                    if (marker != null)
                    {
                        Point newMarkerPosition = dataPoint.Marker.CalculateActualPosition(dataPoint._visualPosition.X, dataPoint._visualPosition.Y, new Point(0.5, 0.5));
                        Point oldMarkerPosition = new Point((Double)marker.GetValue(Canvas.LeftProperty), (Double)marker.GetValue(Canvas.TopProperty));

                        dataPoint.Marker.Visual.Visibility = Visibility.Visible;

                        if (!oldMarkerPosition.Equals(newMarkerPosition))
                        {
                            // Animation for (Canvas.Top) property
                            DoubleAnimation da = new DoubleAnimation()
                            {
                                From = oldMarkerPosition.X,
                                To = newMarkerPosition.X,
                                Duration = new Duration(new TimeSpan(0, 0, 1)),
                                SpeedRatio = 2
                            };

                            Storyboard.SetTarget(da, marker);
                            Storyboard.SetTargetProperty(da, new PropertyPath("(Canvas.Left)"));
                            Storyboard.SetTargetName(da, (String)marker.GetValue(FrameworkElement.NameProperty));

                            storyBoard.Children.Add(da);

                            // Animation for (Canvas.Top) property
                            da = new DoubleAnimation()
                            {
                                From = oldMarkerPosition.Y,
                                To = newMarkerPosition.Y,
                                Duration = new Duration(new TimeSpan(0, 0, 1)),
                                SpeedRatio = 2
                            };

                            Storyboard.SetTarget(da, marker);
                            Storyboard.SetTargetProperty(da, new PropertyPath("(Canvas.Top)"));
                            Storyboard.SetTargetName(da, (String)marker.GetValue(FrameworkElement.NameProperty));

                            storyBoard.Children.Add(da);
                        }
                    }

                    #endregion

                }

            }
        }


        internal static void UpdateLineSeries(DataSeries dataSeries, Double width, Double height,Canvas label2dCanvas)
        {
            Chart chart = dataSeries.Chart as Chart;

            if (dataSeries.Enabled == false)
                return;

            Canvas chartsCanvas = dataSeries.Faces.Visual.Parent as Canvas;
            Canvas labelsCanvas = dataSeries.Faces.LabelCanvas.Parent as Canvas;

            UpdateSizeOfTheParentCanvas(dataSeries, width, height);

            // Groups of broken line segments
            List<List<DataPoint>> pointCollectionList = new List<List<DataPoint>>();

            HideAllMarkersAndLabels(dataSeries);

            // Collect DataPoints under viewport
            List<DataPoint> viewPortDataPoints = RenderHelper.GetDataPointsUnderViewPort(dataSeries, false);
                        
            LineChart.CreateGroupOfBrokenLineSegment(dataSeries, viewPortDataPoints, width, height, out pointCollectionList);

            // gg.Children.Clear();
            GeometryGroup gg = (dataSeries.Faces.Parts[0] as Path).Data as GeometryGroup;

            // Apply new Data for Line
            LineChart.GetPathGeometry(dataSeries.RenderAs, gg, pointCollectionList, false, width, height, label2dCanvas);

            if (!VisifireControl.IsMediaEffectsEnabled)
            {
                // Update GeometryGroup for shadow
                if (dataSeries.Faces.Parts[1] != null)
                {
                    if ((Boolean)dataSeries.ShadowEnabled)
                    {
                        (dataSeries.Faces.Parts[1] as Path).Visibility = Visibility.Visible;

                        // gg.Children.Clear();
                        GeometryGroup ggShadow = (dataSeries.Faces.Parts[1] as Path).Data as GeometryGroup;

                        // Apply new Data for Line
                        LineChart.GetPathGeometry(dataSeries.RenderAs, ggShadow, pointCollectionList, true, width, height, label2dCanvas);
                    }
                    else
                        (dataSeries.Faces.Parts[1] as Path).Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                if (dataSeries.Faces != null && dataSeries.Faces.Visual != null)
                {
#if !WP
                    if ((Boolean)dataSeries.ShadowEnabled)
                    {
                        dataSeries.Faces.Visual.Effect = ExtendedGraphics.GetShadowEffect(315, 2.5, 1);
                    }
                    else
                        dataSeries.Faces.Visual.Effect = null;
#endif
                }
            }

            dataSeries._movingMarker.Visibility = Visibility.Collapsed;

            Clip(chart, chartsCanvas, labelsCanvas , dataSeries.PlotGroup);

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

            xPosition = Graphics.ValueToPixelPosition(0, width, (Double)plotGroup.AxisX.InternalAxisMinimum, (Double)plotGroup.AxisX.InternalAxisMaximum, dataPoint.InternalXValue);
            yPosition = Graphics.ValueToPixelPosition(height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, dataPoint.InternalYValue);

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
                    //if (marker != null)
                    //    marker.TextBackground = dataPoint.LabelBackground;
                    CreateLabel4LineDataPoint(dataPoint, width, height, dataPoint.InternalYValue >= 0, xPosition, yPosition,
                        ref line2dLabelCanvas, true);
                    break;

                case VcProperties.LabelEnabled:
                    //if (marker.LabelEnabled == false)
                        //CreateMarkerAForLineDataPoint(dataPoint, width, height, ref line2dLabelCanvas, out xPosition, out yPosition);
                    //else
                    //    marker.LabelEnabled = (Boolean)dataPoint.LabelEnabled;
                    CreateLabel4LineDataPoint(dataPoint, width, height, dataPoint.InternalYValue >= 0, xPosition, yPosition,
                        ref line2dLabelCanvas, true);
                    break;

                case VcProperties.LabelFontColor:
                    //if (marker != null)
                    //    marker.FontColor = dataPoint.LabelFontColor;
                    CreateLabel4LineDataPoint(dataPoint, width, height, dataPoint.InternalYValue >= 0, xPosition, yPosition,
                        ref line2dLabelCanvas, true);
                    break;

                case VcProperties.LabelFontFamily:
                    //CreateMarkerAForLineDataPoint(dataPoint, width, height, ref line2dLabelCanvas, out xPosition, out yPosition);
                    CreateLabel4LineDataPoint(dataPoint, width, height, dataPoint.InternalYValue >= 0, xPosition, yPosition,
                        ref line2dLabelCanvas, true);
                    // marker.FontFamily = dataPoint.LabelFontFamily;
                    break;

                case VcProperties.LabelFontStyle:
                    //CreateMarkerAForLineDataPoint(dataPoint, width, height, ref line2dLabelCanvas, out xPosition, out yPosition);
                    CreateLabel4LineDataPoint(dataPoint, width, height, dataPoint.InternalYValue >= 0, xPosition, yPosition,
                        ref line2dLabelCanvas, true);
                    //marker.FontStyle = (FontStyle) dataPoint.LabelFontStyle;
                    break;

                case VcProperties.LabelFontSize:
                    //CreateMarkerAForLineDataPoint(dataPoint, width, height, ref line2dLabelCanvas, out xPosition, out yPosition);
                    CreateLabel4LineDataPoint(dataPoint, width, height, dataPoint.YValue >= 0, xPosition, yPosition,
                        ref line2dLabelCanvas, true);
                    // marker.FontSize = (Double) dataPoint.LabelFontSize;
                    break;

                case VcProperties.LabelFontWeight:
                    //if (marker != null)
                    //    marker.FontWeight = (FontWeight) dataPoint.LabelFontWeight;
                    CreateLabel4LineDataPoint(dataPoint, width, height, dataPoint.InternalYValue >= 0, xPosition, yPosition,
                        ref line2dLabelCanvas, true);
                    break;

                case VcProperties.LabelStyle:
                    //CreateMarkerAForLineDataPoint(dataPoint, width, height, ref line2dLabelCanvas, out xPosition, out yPosition);
                    CreateLabel4LineDataPoint(dataPoint, width, height, dataPoint.InternalYValue >= 0, xPosition, yPosition,
                        ref line2dLabelCanvas, true);
                    break;

                case VcProperties.LabelAngle:
                    //CreateMarkerAForLineDataPoint(dataPoint, width, height, ref line2dLabelCanvas, out xPosition, out yPosition);
                    CreateLabel4LineDataPoint(dataPoint, width, height, dataPoint.InternalYValue >= 0, xPosition, yPosition,
                        ref line2dLabelCanvas, true);
                    break;

                case VcProperties.LabelText:
                    //CreateMarkerAForLineDataPoint(dataPoint, width, height, ref line2dLabelCanvas, out xPosition, out yPosition);
                    CreateLabel4LineDataPoint(dataPoint, width, height, dataPoint.InternalYValue >= 0, xPosition, yPosition,
                        ref line2dLabelCanvas, true);
                    //marker.Text = dataPoint.TextParser(dataPoint.LabelText);
                    break;

                case VcProperties.LegendText:
                    chart.InvokeRender();
                    break;

                case VcProperties.LightingEnabled:
                    break;

                case VcProperties.MarkerBorderColor:
                    if (marker == null)
                        CreateMarkerAForLineDataPoint(dataPoint, width, height, ref line2dLabelCanvas, out xPosition, out yPosition);
                    else
                        marker.BorderColor = dataPoint.MarkerBorderColor;

                    break;
                case VcProperties.MarkerBorderThickness:
                    CreateMarkerAForLineDataPoint(dataPoint, width, height, ref line2dLabelCanvas, out xPosition, out yPosition);
                    // marker.BorderThickness = dataPoint.MarkerBorderThickness.Value.Left;
                    
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
                        marker.Visual.Opacity = (Double)dataPoint.Opacity * (Double)dataSeries.Opacity;
                    break;
                case  VcProperties.ShowInLegend:
                    chart.InvokeRender();
                    break;
                case VcProperties.ToolTipText:
                case VcProperties.XValueFormatString:
                case VcProperties.YValueFormatString:
                    dataPoint._parsedToolTipText = dataPoint.TextParser(dataPoint.ToolTipText);
                    //CreateMarkerAForLineDataPoint(dataPoint, width, height, ref line2dLabelCanvas, out xPosition, out yPosition);
                    CreateLabel4LineDataPoint(dataPoint, width, height, dataPoint.YValue >= 0, xPosition, yPosition,
                        ref line2dLabelCanvas, true);
                    break;
                case VcProperties.XValueType:
                    chart.InvokeRender();
                    break;

                case VcProperties.Enabled:
                    if((Boolean)dataPoint.Parent.Enabled)
                        UpdateDataSeries(dataPoint, VcProperties.Enabled, newValue);
                    break;

                case VcProperties.XValue:
                    if (Double.IsNaN(dataPoint._oldYValue) || dataPoint.Faces == null) // Broken point of broken line
                        UpdateDataSeries(dataPoint.Parent, property, newValue);
                    else if (dataSeries.RenderAs == RenderAs.Spline)
                        UpdateDataSeries(dataSeries, VcProperties.DataPointUpdate, newValue);
                    else
                        UpdateXAndYValueOfLineDataPoint(dataPoint, line2dLabelCanvas);

                    break;

                case VcProperties.YValue:
                case VcProperties.YValues:
                    if (Double.IsNaN(dataPoint._oldYValue) || Double.IsNaN(dataPoint.InternalYValue) || dataPoint.Faces == null) // Broken point of broken line
                        UpdateDataSeries(dataPoint.Parent, property, newValue);
                    else
                    {

                        if(dataSeries.RenderAs == RenderAs.Spline)
                            UpdateDataSeries(dataPoint.Parent, VcProperties.DataPointUpdate, newValue);
                        else // dataSeries.RenderAs == RenderAs.Line
                            chart.Dispatcher.BeginInvoke(new Action<DataPoint, Canvas>(UpdateXAndYValueOfLineDataPoint), new object[] { dataPoint, line2dLabelCanvas });
                    }
                   
                   break;
            }
        }

        internal static void ApplyAnimationToBezierSegments(Int32 index, Storyboard storyBoard, BezierSegment bezierSegment,
            Point[] oldCtrlPoints, Point[] newCtrlPoints, PathFigure pathFigure, Point oldFirstPointOfBezierSeg, 
            Point newFirstPointOfBezierSeg)
        {
            if (index == 1)
            {
                PointAnimation pointAnimation = new PointAnimation()
                {
                    From = oldFirstPointOfBezierSeg,
                    To = newFirstPointOfBezierSeg,
                    SpeedRatio = 2,
                    Duration = new Duration(new TimeSpan(0, 0, 1))
                };

                Storyboard.SetTarget(pointAnimation, pathFigure);
                Storyboard.SetTargetProperty(pointAnimation, new PropertyPath("StartPoint"));
                Storyboard.SetTargetName(pointAnimation, (String)pathFigure.GetValue(FrameworkElement.NameProperty));

                storyBoard.Children.Add(pointAnimation);
#if WPF
                pathFigure.BeginAnimation(PathFigure.StartPointProperty, pointAnimation);
#endif
            }

            // Loop for 3 control points
            for (int i = 0; i < 3; i++)
            {
                // Creates PointAnimation for each control points
                if (!oldCtrlPoints[i].Equals(newCtrlPoints[i]))
                {
                    PointAnimation pointAnimation = new PointAnimation()
                    {
                        From = oldCtrlPoints[i],
                        To = newCtrlPoints[i],
                        SpeedRatio = 2,
                        Duration = new Duration(new TimeSpan(0, 0, 1))
                    };

                    Storyboard.SetTarget(pointAnimation, bezierSegment);
                    Storyboard.SetTargetProperty(pointAnimation, new PropertyPath("Point" + (i + 1).ToString()));
                    Storyboard.SetTargetName(pointAnimation, (String)bezierSegment.GetValue(FrameworkElement.NameProperty));

                    storyBoard.Children.Add(pointAnimation);

#if WPF
                    switch(i)
                    {
                        case 0:
                            bezierSegment.BeginAnimation(BezierSegment.Point1Property, pointAnimation);
                            break;
                        case 1:
                            bezierSegment.BeginAnimation(BezierSegment.Point2Property, pointAnimation);
                            break;
                        case 2:
                            bezierSegment.BeginAnimation(BezierSegment.Point3Property, pointAnimation);
                            break;
                    }
#endif
                }
            }

        }

        private static void UpdateXAndYValueOfLineDataPoint(DataPoint dataPoint, Canvas line2dLabelCanvas)
        {   
            Boolean isAnimationEnabled = (Boolean)(dataPoint.Chart as Chart).AnimatedUpdate;

            if (!(Boolean)dataPoint.Enabled || dataPoint.Faces == null)
                return;

            Chart chart = dataPoint.Chart as Chart;
            DataSeries dataSeries = dataPoint.Parent;
            dataSeries._movingMarker.Visibility = Visibility.Collapsed;

            Axis axisX = dataSeries.PlotGroup.AxisX;
            Axis axisY = dataSeries.PlotGroup.AxisY;

            Marker dataPointMarker = dataPoint.Marker;
            Marker legendMarker = dataPoint.LegendMarker;
            
            Double height = chart.ChartArea.ChartVisualCanvas.Height;
            Double width = chart.ChartArea.ChartVisualCanvas.Width;

            Double x = Graphics.ValueToPixelPosition(0, width, axisX.InternalAxisMinimum, axisX.InternalAxisMaximum, dataPoint.InternalXValue);
            Double y = Graphics.ValueToPixelPosition(height, 0, axisY.InternalAxisMinimum, axisY.InternalAxisMaximum, dataPoint.InternalYValue);

            dataPoint._visualPosition = new Point(x, y);
            Point newMarkerPosition = new Point();
            Point newLabelPosition = new Point();

            if(dataPointMarker != null)
                newMarkerPosition = dataPointMarker.CalculateActualPosition(x, y, new Point(0.5, 0.5));

            if ((Boolean)dataPoint.LabelEnabled)
            {   
                if (isAnimationEnabled && dataPoint.LabelVisual != null)
                {
                    dataPoint._oldLabelPosition = new Point((Double)dataPoint.LabelVisual.GetValue(Canvas.LeftProperty), (Double)dataPoint.LabelVisual.GetValue(Canvas.TopProperty));
                    newLabelPosition = CreateLabel4LineDataPoint(dataPoint, width, height, dataPoint.InternalYValue >= 0, x, y, ref line2dLabelCanvas, false);

                    dataPoint.LabelVisual.SetValue(Canvas.TopProperty, dataPoint._oldLabelPosition.Y);
                    dataPoint.LabelVisual.SetValue(Canvas.LeftProperty, dataPoint._oldLabelPosition.X);
                }
                else
                    CreateLabel4LineDataPoint(dataPoint, width, height, dataPoint.InternalYValue >= 0, x, y, ref line2dLabelCanvas, true);
            }

            if (!isAnimationEnabled)
            {   
                if (dataPointMarker != null && dataPointMarker.Visual != null)
                {   
                    dataPointMarker.Visual.SetValue(Canvas.TopProperty, newMarkerPosition.Y);
                    dataPointMarker.Visual.SetValue(Canvas.LeftProperty, newMarkerPosition.X);
                }
            }

            DependencyObject target = null;
            DependencyObject shadowTarget = null;    // Target object
            Point oldPoint = new Point();                    // Old Position

            // Collect reference of line geometry object
            LineSegment lineSeg = dataPoint.Faces.Parts[0] as LineSegment;
            PathFigure pathFigure = dataPoint.Faces.Parts[1] as PathFigure;

            if (!VisifireControl.IsMediaEffectsEnabled)
            {   
                LineSegment shadowLineSeg;
                PathFigure shadowPathFigure;

                // For line shadow
                if ((Boolean)dataPoint.Parent.ShadowEnabled)
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

                Storyboard storyBoard = new Storyboard();
                PointAnimation pointAnimation = new PointAnimation();

                pointAnimation.From = oldPoint;
                pointAnimation.To = new Point(x, y);
                pointAnimation.SpeedRatio = 2;
                pointAnimation.Duration = new Duration(new TimeSpan(0, 0, 1));

                //target.SetValue(FrameworkElement.NameProperty, "Segment_" + dataPoint.Name);

                Storyboard.SetTarget(pointAnimation, target);

                Storyboard.SetTargetProperty(pointAnimation, (lineSeg != null) ? new PropertyPath("Point") : new PropertyPath("StartPoint"));

                Storyboard.SetTargetName(pointAnimation, (String)target.GetValue(FrameworkElement.NameProperty));

                storyBoard.Children.Add(pointAnimation);

                if (!VisifireControl.IsMediaEffectsEnabled)
                {
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

                        storyBoard.Children.Add(pointAnimation);

#if WPF
                        if (lineSeg != null)
                            (shadowTarget as LineSegment).BeginAnimation(LineSegment.PointProperty, pointAnimation);
                        else
                            (shadowTarget as PathFigure).BeginAnimation(PathFigure.StartPointProperty, pointAnimation);
#endif
                    }
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

                    storyBoard.Children.Add(da);

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

                    storyBoard.Children.Add(da);
                }

                #endregion

                #region Attach Animation with Label

                FrameworkElement label = dataPoint.LabelVisual;

                if (label != null)
                {
                    // Animation for (Canvas.Top) property
                    DoubleAnimation da = new DoubleAnimation()
                    {
                        From = dataPoint._oldLabelPosition.X,
                        To = newLabelPosition.X,
                        Duration = new Duration(new TimeSpan(0, 0, 1)),
                        SpeedRatio = 2
                    };

                    Storyboard.SetTarget(da, label);
                    Storyboard.SetTargetProperty(da, new PropertyPath("(Canvas.Left)"));
                    Storyboard.SetTargetName(da, (String)label.GetValue(FrameworkElement.NameProperty));

                    storyBoard.Children.Add(da);

                    // Animation for (Canvas.Top) property
                    da = new DoubleAnimation()
                    {
                        From = dataPoint._oldLabelPosition.Y,
                        To = newLabelPosition.Y,
                        Duration = new Duration(new TimeSpan(0, 0, 1)),
                        SpeedRatio = 2
                    };

                    Storyboard.SetTarget(da, label);
                    Storyboard.SetTargetProperty(da, new PropertyPath("(Canvas.Top)"));
                    Storyboard.SetTargetName(da, (String)label.GetValue(FrameworkElement.NameProperty));

                    storyBoard.Children.Add(da);

                }

                #endregion

                dataPoint.Storyboard = storyBoard;
#if WPF
                if (lineSeg != null)
                    (target as LineSegment).BeginAnimation(LineSegment.PointProperty, pointAnimation);
                else
                    (target as PathFigure).BeginAnimation(PathFigure.StartPointProperty, pointAnimation);
#endif
                // Start the animation
                storyBoard.Begin();
            }

            // chart.ChartArea.ChartVisualCanvas.Background = new SolidColorBrush(Colors.Blue);
            dataSeries.Faces.Visual.Width = chart.ChartArea.ChartVisualCanvas.Width;
            dataSeries.Faces.Visual.Height = chart.ChartArea.ChartVisualCanvas.Height;

            dataSeries.Faces.LabelCanvas.Width = chart.ChartArea.ChartVisualCanvas.Width;
            dataSeries.Faces.LabelCanvas.Height = chart.ChartArea.ChartVisualCanvas.Height;

            // Update ToolTip Text
            dataPoint._parsedToolTipText = dataPoint.TextParser(dataPoint.ToolTipText);
            
            if(dataSeries._movingMarker != null)
                dataSeries._movingMarker.Visibility = Visibility.Collapsed;
            
            chart._toolTip.Hide();

            if(dataSeries.ToolTipElement != null)
                dataSeries.ToolTipElement.Hide();

            chart.ChartArea.DisableIndicators();

            if (dataSeries.Faces != null)
            {
                RectangleGeometry clipRectangle = new RectangleGeometry();

                Double depth3d = chart.ChartArea.PLANK_DEPTH / (chart.PlotDetails.Layer3DCount == 0 ? 1 : chart.PlotDetails.Layer3DCount) * (chart.View3D ? 1 : 0);

                Double clipLeft = 0;
                Double clipTop = -depth3d - 4;
                Double clipWidth = width + depth3d;
                Double clipHeight = height + depth3d + chart.ChartArea.PLANK_THICKNESS + 10;

                AreaChart.GetClipCoordinates(chart, ref clipLeft, ref clipTop, ref clipWidth, ref clipHeight, dataSeries.PlotGroup.MinimumX, dataSeries.PlotGroup.MaximumX);

                clipRectangle.Rect = new Rect(clipLeft, clipTop, clipWidth, clipHeight);

                if (dataSeries.Faces.LabelCanvas != null && dataSeries.Faces.LabelCanvas.Parent != null)
                    (dataSeries.Faces.LabelCanvas.Parent as Canvas).Clip = clipRectangle;

                clipRectangle = new RectangleGeometry();
                clipRectangle.Rect = new Rect(0, -depth3d - 4, width + depth3d, height + chart.ChartArea.PLANK_DEPTH + chart.ChartArea.PLANK_THICKNESS + 10);

                if (dataSeries.Faces.Visual != null)
                    (dataSeries.Faces.Visual.Parent as Canvas).Clip = clipRectangle;
            }

        }

        /// <summary>
        /// Apply marker properties
        /// </summary>
        /// <param name="dataPoint">DataPoint</param>
        /// <param name="markerSize">Marker size</param>
        internal static void ApplyMarkerProperties(DataPoint dataPoint)
        {   
            Marker marker = dataPoint.Marker;
            marker.ScaleFactor = (Double) dataPoint.MarkerScale;
            marker.MarkerSize = new Size((Double)dataPoint.MarkerSize, (Double)dataPoint.MarkerSize);

            if ((Boolean)dataPoint.MarkerEnabled)
                marker.BorderColor = dataPoint.MarkerBorderColor;
            else
                marker.BorderColor = new SolidColorBrush(Colors.Transparent);
            
            marker.BorderThickness = ((Thickness)dataPoint.MarkerBorderThickness).Left;
            marker.ShadowEnabled = (Boolean) dataPoint.ShadowEnabled;
            marker.MarkerFillColor = dataPoint.MarkerColor;
        }

        internal static void ApplyLabelProperties(DataPoint dataPoint)
        {
            dataPoint.Marker.FontColor = Chart.CalculateDataPointLabelFontColor(dataPoint.Chart as Chart, dataPoint, dataPoint.LabelFontColor, LabelStyles.OutSide);
            dataPoint.Marker.FontFamily = dataPoint.LabelFontFamily;
            dataPoint.Marker.FontSize = (Double)dataPoint.LabelFontSize;
            dataPoint.Marker.FontStyle = (FontStyle)dataPoint.LabelFontStyle;
            dataPoint.Marker.FontWeight = (FontWeight)dataPoint.LabelFontWeight;
            dataPoint.Marker.TextBackground = dataPoint.LabelBackground;
        }

        /// <summary>
        /// Apply animation for line chart
        /// </summary>
        /// <param name="canvas">Line chart canvas</param>
        /// <param name="storyboard">Storyboard</param>
        /// <param name="isLineCanvas">Whether canvas is line canvas</param>
        /// <returns>Storyboard</returns>
        internal static Storyboard ApplyLineChartAnimation(DataSeries currentDataSeries, Panel canvas, Storyboard storyboard, Boolean isLineCanvas)
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

        internal static void CalculateMarkerPosition(DataPoint dataPoint, Double width, Double height, out Double xPosition, out Double yPosition)
        {
            xPosition = Double.NaN;
            yPosition = Double.NaN;
            if (Double.IsNaN(dataPoint.InternalYValue))
                return;

            PlotGroup plotGroup = dataPoint.Parent.PlotGroup;
            Chart chart = dataPoint.Chart as Chart;

            xPosition = Graphics.ValueToPixelPosition(0, width, (Double)plotGroup.AxisX.InternalAxisMinimum, (Double)plotGroup.AxisX.InternalAxisMaximum, dataPoint.InternalXValue);
            yPosition = Graphics.ValueToPixelPosition(height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, dataPoint.InternalYValue);

        }

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

            dataPoint._visualPosition = new Point(xPosition, yPosition);

            // Create Marker
            Marker marker = GetMarkerForDataPoint(true, chart, width, height, yPosition, dataPoint, dataPoint.InternalYValue > 0);
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
            {
                return;
            }

            Double xPosition, yPosition;
            Chart chart  = (series.Chart as Chart);

            line2dLabelCanvas = new Canvas() { Width = width, Height = height };   // Canvas for placing labels

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
            lineParams.ShadowEnabled = (Boolean)series.ShadowEnabled;

            if ((Boolean)series.ShadowEnabled)
                lineParams.LineShadowGeometryGroup = new GeometryGroup();

            #endregion

            series.VisualParams = lineParams;

            Point variableStartPoint = new Point(), endPoint = new Point();
            Boolean IsStartPoint = true;

            // Polyline polyline, PolylineShadow;
            // Canvas line2dCanvas = new Canvas();
            // Canvas lineCanvas;
            List<DataPoint> viewPortDataPoints = RenderHelper.GetDataPointsUnderViewPort(series, false);

            foreach (DataPoint dataPoint in viewPortDataPoints)
            {   
                if (dataPoint.Enabled == false)
                    continue;

                dataPoint.Marker = null;
                dataPoint.LabelVisual = null;

                if (Double.IsNaN(dataPoint.InternalYValue))
                {
                    xPosition = Double.NaN;
                    yPosition = Double.NaN;
                    IsStartPoint = true;
                }
                else
                {
                    //CreateMarkerAForLineDataPoint(dataPoint, width, height, ref line2dLabelCanvas, out xPosition, out yPosition);
                    CalculateMarkerPosition(dataPoint, width, height, out xPosition, out yPosition);

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
            line2dCanvas = GetLine2D(series, width, height, line2dLabelCanvas, lineParams, out polyline, out PolylineShadow, pointCollectionList, shadowPointCollectionList);

            line2dCanvas.Width = width;
            line2dCanvas.Height = height;

            series.Faces.Parts.Add(polyline);

            if (!VisifireControl.IsMediaEffectsEnabled)
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
            if (Double.IsNaN(width) || Double.IsNaN(height) || width <= 0 || height <= 0) 
                return null;

            DataSeries currentDataSeries;

            Canvas visual, labelsCanvas, chartsCanvas;
            RenderHelper.RepareCanvas4Drawing(preExistingPanel as Canvas, out visual, out labelsCanvas, out chartsCanvas, width, height);
            
            Double depth3d = plankDepth / (plotDetails.Layer3DCount == 0 ? 1 : plotDetails.Layer3DCount) * (chart.View3D ? 1 : 0);
            Double visualOffset = depth3d * (plotDetails.SeriesDrawingIndex[seriesList[0]] + 1 - (plotDetails.Layer3DCount == 0 ? 0 : 1));
            
            // Set visual canvas position

            visual.SetValue(Canvas.TopProperty, visualOffset);
            visual.SetValue(Canvas.LeftProperty, -visualOffset);

            // visual.Background = new SolidColorBrush(Colors.Yellow);
                        
            Boolean isMovingMarkerEnabled = false; // Whether moving marker is enabled for atleast one series

            Double minimumXValue = Double.MaxValue;
            Double maximumXValue = Double.MinValue;

            foreach (DataSeries series in seriesList)
            {
                currentDataSeries = series;
                CreateAlineSeries(series, width, height, labelsCanvas, chartsCanvas, animationEnabled);
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
                chart.ChartArea.PlotAreaCanvas.Tag = chart.PlotArea;
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

                currentDataSeries.Storyboard = ApplyLineChartAnimation(currentDataSeries, labelsCanvas, currentDataSeries.Storyboard, false);
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
                labelsCanvas.SetValue(Canvas.ZIndexProperty, 1);
                visual.Children.Add(labelsCanvas);
                visual.Children.Add(chartsCanvas);
            }

            chartsCanvas.Height = height;
            labelsCanvas.Height = height;
            chartsCanvas.Width = width;
            labelsCanvas.Width = width;

            Clip(chart, chartsCanvas, labelsCanvas, seriesList[0].PlotGroup);

            return visual;
        }

        internal static void Clip(Chart chart, Canvas chartsCanvas, Canvas labelCanvas, PlotGroup plotGroup)
        {
            Double depth3d = chart.ChartArea.PLANK_DEPTH / (chart.PlotDetails.Layer3DCount == 0 ? 1 : chart.PlotDetails.Layer3DCount) * (chart.View3D ? 1 : 0);

            RectangleGeometry clipRectangle = new RectangleGeometry();

            Double clipLeft = 0;
            Double clipTop = -depth3d - 4;
            Double clipWidth = labelCanvas.Width + depth3d;
            Double clipHeight = labelCanvas.Height + depth3d + chart.ChartArea.PLANK_THICKNESS + 10;

            AreaChart.GetClipCoordinates(chart, ref clipLeft, ref clipTop, ref clipWidth, ref clipHeight, plotGroup.MinimumX, plotGroup.MaximumX);

            clipRectangle.Rect = new Rect(clipLeft, clipTop, clipWidth, clipHeight);
            labelCanvas.Clip = clipRectangle;

            clipRectangle = new RectangleGeometry();
            clipRectangle.Rect = new Rect(0, -depth3d - 4, labelCanvas.Width + depth3d, labelCanvas.Height + chart.ChartArea.PLANK_DEPTH + chart.ChartArea.PLANK_THICKNESS + 10);

            // System.Diagnostics.Debug.WriteLine(clipRectangle.Rect.ToString());
            chartsCanvas.Clip = clipRectangle;
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

            PlotArea plotArea = ((FrameworkElement)sender).Tag as PlotArea;

            if (plotArea == null) return;
            Chart chart = plotArea.Chart as Chart;
            if (chart == null) return;

            // Disable Moving marker for PlotArea Canvas
            foreach (DataSeries ds in chart.InternalSeries)
            {
                if (!(ds.RenderAs == RenderAs.Line && ds.RenderAs == RenderAs.Spline)) return;
                
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
            PlotArea plotArea = ((FrameworkElement)sender).Tag as PlotArea;
            if (plotArea == null) return;
            Chart chart = plotArea.Chart as Chart;
            if (chart == null) return;

            ((FrameworkElement) sender).Dispatcher.BeginInvoke(new Action<Chart, object, MouseEventArgs, RenderAs[]>(MoveMovingMarker), chart, sender, e, new RenderAs[]{RenderAs.Line, RenderAs.Spline});
        }

        /// <summary>
        /// Move the moving marker
        /// </summary>
        /// <param name="chart">Chart reference</param>
        /// <param name="sender">object</param>
        /// <param name="e">MouseEventArgs</param>
        internal static void MoveMovingMarker(Chart chart, object sender, MouseEventArgs e, params RenderAs[] chartTypes)
        {   
            if (!_isMouseEnteredInPlotArea)
                return;

            foreach (DataSeries ds in chart.InternalSeries)
            {
                // If not found
                if (Array.IndexOf(chartTypes, ds.RenderAs) == -1)
                    continue;

                Double internalXValue = RenderHelper.CalculateInternalXValueFromPixelPos(chart, ds.PlotGroup.AxisX, e);
                Double internalYValue = RenderHelper.CalculateInternalXValueFromPixelPos(chart, ds.PlotGroup.AxisY, e);

                ds.ShowMovingMarker(internalXValue, internalYValue);

            }
            
            /* Removed old code
            Double xPosition = e.GetPosition(sender as Canvas).X;

            foreach (DataSeries ds in chart.InternalSeries)
            {
                if (ds.RenderAs != RenderAs.Line)
                    continue;

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
                        if (!(Boolean)dp.Enabled)
                            continue;

                        if (dp.Marker != null)
                        {
                            dp._x_distance = Math.Abs(xPosition - dp._visualPosition.X);

                            if (nearestDataPoint == null)
                            {
                                nearestDataPoint = dp;
                                continue;
                            }

                            if (dp._x_distance < nearestDataPoint._x_distance)
                                nearestDataPoint = dp;
                        }
                    }

                    if (nearestDataPoint != null)
                    {
                        var nearestDPAlongXPosition = from dp in ds.DataPoints where dp.InternalXValue == nearestDataPoint.InternalXValue select dp;

                        DataPoint newNearestDp = nearestDPAlongXPosition.First().Parent.GetNearestDataPointAlongYPosition(e, nearestDPAlongXPosition.ToList());

                        if (newNearestDp != null)
                            nearestDataPoint = newNearestDp;
                    }

                    // DataPoint nearestDataPoint = (from dp in ds.DataPoints orderby Math.Abs(xPosition - dp._visualPosition.X) select dp).First();

                    ShowMovingMarkerAtDataPoint(ds, nearestDataPoint);
                }
            }*/
        
        }

        internal static void ShowMovingMarkerAtDataPoint(DataSeries dataSeries, DataPoint nearestDataPoint)
        {
            if (nearestDataPoint == null)
            {
                dataSeries._movingMarker.Visibility = Visibility.Collapsed;
                return;
            }

            if (dataSeries.MovingMarkerEnabled)
            {
                Ellipse movingMarker = nearestDataPoint.Parent._movingMarker;

                if(_isMouseEnteredInPlotArea)
                    movingMarker.Visibility = Visibility.Visible;

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

        private const Double MOVING_MARKER_SCALE = 1.1;

        #endregion
    }
}
