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
using System.Diagnostics;
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
using System.Diagnostics;
#endif


using Visifire.Commons;

namespace Visifire.Charts
{
    /// <summary>
    /// Visifire.Charts.ElementPositionData class
    /// (Used for positioning elements)
    /// </summary>
    internal class ElementPositionData
    {
        #region Public Methods

        /// <summary>
        /// Initializes a new instance of the Visifire.Charts.ElementPositionData class
        /// </summary>
        public ElementPositionData()
        {
        }

        /// <summary>
        /// Contains data for Positioning elements 
        /// </summary>
        /// <param name="element">Element</param>
        /// <param name="angle1">Angle1</param>
        /// <param name="angle2">Angle2</param>
        public ElementPositionData(FrameworkElement element, Double angle1, Double angle2)
        {
            Element = element;
            StartAngle = angle1;
            StopAngle = angle2;
        }

        /// <summary>
        /// Contains data for Positioning elements 
        /// </summary>
        /// <param name="m">ElementPosition</param>
        public ElementPositionData(ElementPositionData elementPosition)
        {
            Element = elementPosition.Element;
            StartAngle = elementPosition.StartAngle;
            StopAngle = elementPosition.StopAngle;
        }

        /// <summary>
        /// Compare angles for two position elements
        /// </summary>
        /// <param name="a">ElementPosition1</param>
        /// <param name="b">ElementPosition1</param>
        /// <returns></returns>
        public static Int32 CompareAngle(ElementPositionData elementPosition1, ElementPositionData elementPosition2)
        {
            Double angle1 = (elementPosition1.StartAngle + elementPosition1.StopAngle) / 2;
            Double angle2 = (elementPosition2.StartAngle + elementPosition2.StopAngle) / 2;

            return angle1.CompareTo(angle2);
        }

        #endregion Public Methods

        #region Public Properties

        /// <summary>
        /// FrameworkElement
        /// </summary>
        public FrameworkElement Element
        {
            get;
            set;
        }

        /// <summary>
        /// Start angle  
        /// </summary>
        public Double StartAngle
        {
            get;
            set;
        }

        /// <summary>
        /// Stop angle
        /// </summary>
        public Double StopAngle
        {
            get;
            set;
        }

        #endregion
    }

    /// <summary>
    /// Visifire.Charts.SectorChartShapeParams class
    /// (Used for Pie and Doughnut charts)
    /// </summary>
    internal class SectorChartShapeParams
    {
        #region Public Methods

        #endregion

        #region Public Properties

        /// <summary>
        /// Get the fixed angle
        /// </summary>
        /// <param name="angle">Angle</param>
        /// <returns>Angle</returns>
        public Double FixAngle(Double angle)
        {
            while (angle > Math.PI * 2) angle -= Math.PI;
            while (angle < 0) angle += Math.PI;
            return angle;
        }

        /// <summary>
        /// Whether the label line target point is at the right side of the label
        /// </summary>
        public Boolean LabelLineTargetToRight
        {
            get;
            set;
        }

        /// <summary>
        /// OuterRadius of the pie
        /// </summary>
        public Double OuterRadius
        {
            get;
            set;
        }

        /// <summary>
        /// InnerRadius of the Pie
        /// </summary>
        public Double InnerRadius
        {
            get;
            set;
        }

        /// <summary>
        /// StartAngle of the Pie
        /// </summary>
        public Double StartAngle
        {
            get
            {
                return _startAngle;
            }
            set
            {
                _startAngle = value;
            }
        }

        /// <summary>
        /// StopAngle of the Pie
        /// </summary>
        public Double StopAngle
        {
            get
            {
                return _stopAngle;
            }
            set
            {
                _stopAngle = value;
            }
        }

        /// <summary>
        /// Center position
        /// </summary>
        public Point Center
        {
            get;
            set;
        }

        /// <summary>
        /// X Offset value
        /// </summary>
        public Double OffsetX
        {
            get;
            set;
        }

        /// <summary>
        /// Y Offset value
        /// </summary>
        public Double OffsetY
        {
            get;
            set;
        }

        /// <summary>
        /// Whether it is a LargerArc
        /// </summary>
        public Boolean IsLargerArc
        {
            get;
            set;
        }

        /// <summary>
        /// Whether the lighting is enabled
        /// </summary>
        public Boolean Lighting
        {
            get;
            set;
        }

        /// <summary>
        /// Whether the Bevel effect is applied
        /// </summary>
        public Boolean Bevel
        {
            get;
            set;
        }

        /// <summary>
        /// Background color of the pie
        /// </summary>
        public Brush Background
        {
            get;
            set;
        }

        /// <summary>
        /// Whether the value for pie is zero
        /// </summary>
        public bool IsZero
        {
            get;
            set;
        }

        /// <summary>
        /// YAxis scale value
        /// </summary>
        public Double YAxisScaling
        {
            get
            {
                return Math.Sin(TiltAngle);
            }
        }

        /// <summary>
        /// ZAxis scale value
        /// </summary>
        public Double ZAxisScaling
        {
            get
            {
                return Math.Cos(Math.PI / 2 - TiltAngle);
            }
        }

        /// <summary>
        /// 3D depth value
        /// </summary>
        public Double Depth
        {
            get;
            set;
        }

        /// <summary>
        /// ExplodeRatio varies from 0 to 1 
        /// </summary>
        public Double ExplodeRatio
        {
            get;
            set;
        }

        /// <summary>
        /// Width of the PlotArea
        /// </summary>
        public Double Width
        {
            get;
            set;
        }

        /// <summary>
        /// Height of the PlotArea
        /// </summary>
        public Double Height
        {
            get;
            set;
        }

        /// <summary>
        /// TiltAngle for pie, used for 3d Pie
        /// </summary>
        public Double TiltAngle
        {
            get;
            set;
        }

        /// <summary>
        /// Position of the label
        /// </summary>
        public Point LabelPoint
        {
            get;
            set;
        }

        /// <summary>
        /// Color of the label line
        /// </summary>
        public Brush LabelLineColor
        {
            get;
            set;
        }

        /// <summary>
        /// Thickness of the label line
        /// </summary>
        public Double LabelLineThickness
        {
            get;
            set;
        }

        /// <summary>
        /// LineStyle of the line for label
        /// </summary>
        public DoubleCollection LabelLineStyle
        {
            get;
            set;
        }

        /// <summary>
        /// Whether line is enabled for labels
        /// </summary>
        public Boolean LabelLineEnabled
        {
            get;
            set;
        }

        /// <summary>
        /// Mean angle of a pie
        /// </summary>
        public Double MeanAngle
        {
            get;
            set;
        }

        /// <summary>
        /// Storyboard associated with animation
        /// </summary>
        public Storyboard Storyboard
        {
            get;
            set;
        }

        /// <summary>
        /// Whether animation is enabled
        /// </summary>
        public Boolean AnimationEnabled
        {
            get;
            set;
        }

        /// <summary>
        /// Tag reference
        /// </summary>
        public FrameworkElement TagReference
        {
            get;
            set;
        }

        public DataPoint DataPoint;

        public PieDoughnutPoints ExplodedPoints
        {
            get;
            set;
        }

        public PieDoughnutPoints UnExplodedPoints
        {
            get;
            set;
        }

        #endregion

        #region Data

        /// <summary>
        /// Start angle
        /// </summary>
        private Double _startAngle;

        /// <summary>
        /// End angle
        /// </summary>
        private Double _stopAngle;

        #endregion
    }


    /// <summary>
    /// Visfiire.Charts.Point3D class
    /// </summary>
    internal struct Point3D
    {
        #region Public Methods

        #endregion

        #region Public Properties

        /// <summary>
        /// X position
        /// </summary>
        public Double X
        {
            get;
            set;
        }

        /// <summary>
        /// Y position
        /// </summary>
        public Double Y
        {
            get;
            set;
        }

        /// <summary>
        /// Z position
        /// </summary>
        public Double Z
        {
            get;
            set;
        }

        #endregion

        #region Public Events And Delegates

        #endregion

        #region Data

        #endregion
    }


    internal abstract class PieDoughnutPoints
    {
        /// <summary>
        /// LabelLine dateTime position
        /// </summary>
        public Point LabelLineStartPoint
        {
            get;
            set;
        }

        /// <summary>
        /// LabelLine mid position
        /// </summary>
        public Point LabelLineMidPoint
        {
            get;
            set;
        }

        /// <summary>
        /// LabelLine end position
        /// </summary>
        public Point LabelLineEndPoint
        {
            get;
            set;
        }

        /// <summary>
        /// LabelLine position
        /// </summary>
        public Point LabelPosition
        {
            get;
            set;
        }
    }

    /// <summary>
    /// Visifire.Charts.PieDoughnut2DPoints class
    /// </summary>
    internal class PieDoughnut2DPoints : PieDoughnutPoints
    {
        #region Public Methods

        #endregion

        #region Public Properties

        /// <summary>
        /// Center position
        /// </summary>
        public Point Center
        {
            get;
            set;
        }

        /// <summary>
        /// InnerArc dateTime position
        /// </summary>
        public Point InnerArcStart
        {
            get;
            set;
        }

        /// <summary>
        /// InnerArc mid position
        /// </summary>
        public Point InnerArcMid
        {
            get;
            set;
        }

        /// <summary>
        /// InnerArc end position
        /// </summary>
        public Point InnerArcEnd
        {
            get;
            set;
        }

        /// <summary>
        /// OuterArc dateTime position
        /// </summary>
        public Point OuterArcStart
        {
            get;
            set;
        }

        /// <summary>
        /// OuterArc mid position
        /// </summary>
        public Point OuterArcMid
        {
            get;
            set;
        }

        /// <summary>
        /// OuterArc end position
        /// </summary>
        public Point OuterArcEnd
        {
            get;
            set;
        }



        #endregion

    }

    /// <summary>
    ///  Visifire.Charts.PieDoughnut3DPoints class
    /// </summary>
    internal class PieDoughnut3DPoints : PieDoughnutPoints
    {
        #region Public Properties
        #endregion
    }

    /// <summary>
    ///  Visifire.Charts.PieChart class
    /// </summary>
    internal class PieChart
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
        /// Fix an angle within 0 to 360
        /// </summary>
        /// <param name="angle">Angle</param>
        /// <returns>Angle as Double</returns>
        private static Double FixAngle(Double angle)
        {   
            while (angle > Math.PI * 2) angle -= Math.PI * 2;
            while (angle < 0) angle += Math.PI * 2;
            return angle;
        }

        /// <summary>
        /// List of ElementPositionData
        /// </summary>
        private static List<ElementPositionData> _elementPositionData;

        /// <summary>
        /// Creates a new label for a DataPoint
        /// </summary>
        /// <param name="dataPoint">DataPoint</param>
        /// <returns>Grid</returns>
        private static Canvas CreateLabel(DataPoint dataPoint)
        {
            TextBlock textBlock = new TextBlock()
            {
                FontFamily = dataPoint.LabelFontFamily,
                FontSize = (Double)dataPoint.LabelFontSize,
                FontStyle = (FontStyle)dataPoint.LabelFontStyle,
                FontWeight = (FontWeight)dataPoint.LabelFontWeight,
                Foreground = Chart.CalculateDataPointLabelFontColor((dataPoint.Chart as Chart), dataPoint, dataPoint.LabelFontColor, (LabelStyles)dataPoint.LabelStyle),
                Text = dataPoint.TextParser(dataPoint.LabelText),
                Tag = new ElementData() { Element = dataPoint }
            };

            Size textBlockSize = Graphics.CalculateVisualSize(textBlock);
            if (textBlockSize.Width == 0 && textBlockSize.Width == 0)
            {
                textBlockSize.Width = textBlock.ActualWidth;
                textBlockSize.Height = textBlock.ActualHeight;
            }

            Canvas visual = new Canvas() { Height = textBlockSize.Height, Width = textBlockSize.Width, Background = dataPoint.LabelBackground, Tag = new ElementData() { Element = dataPoint } };

            visual.Opacity = (Double)dataPoint.InternalOpacity * (Double)dataPoint.Parent.InternalOpacity;

            visual.Children.Add(textBlock);

            dataPoint.LabelVisual = visual;

            return visual;
        }

        /// <summary>
        /// Position labels for Pie /Doughnut
        /// </summary>
        /// <param name="visual">Visual element</param>
        /// <param name="totalSum">Total YValue sum from all dataPoints</param>
        /// <param name="dataPoints">List of dataPoint</param>
        /// <param name="labels">Dictionary of dataPoint labels</param>
        /// <param name="pieSize">Pie size</param>
        /// <param name="referenceEllipseSize">Reference ellipse size</param>
        /// <param name="visualCanvasSize">Visual canvas size</param>
        /// <param name="scaleY">Scale Y</param>
        /// <param name="is3D">Whether a 3D chart</param>
        private static void PositionLabels(Canvas visual, Double totalSum, List<DataPoint> dataPoints, Size pieSize, Size referenceEllipseSize, Size visualCanvasSize, Double scaleY, Boolean is3D)
        {
            //Point plotRadius;
            Double xRadiusLabel = referenceEllipseSize.Width / 2;
            Double yRadiusLabel = referenceEllipseSize.Height / 2;
            Double xRadiusChart = pieSize.Width / 2;
            Double yRadiusChart = pieSize.Height / 2;
            Point center;
            Chart chart = null;
            //if(is3D)
            //    center = new Point(visualCanvasSize.Width / 2, visualCanvasSize.Height / 2 - (yRadiusChart * scaleY) / 2);
            //else

            center = new Point(visualCanvasSize.Width / 2, visualCanvasSize.Height / 2);
            
            Double startAngle = FixAngle(dataPoints[0].Parent.InternalStartAngle), stopAngle, meanAngle;
            // Graphics.DrawPointAt(center, visual, System.Windows.Media.Colors.Red);

            CircularLabel prevousLabel = null;
            CircularLabel cLabel = null;
            Rect boundingArea = new Rect(0, 0, visual.Width, visual.Height);
            // List of CircularLabels
            List<CircularLabel> circularLabels = new List<CircularLabel>();

            // Creating the LinkList of CircularLabels
            foreach (DataPoint dp in dataPoints)
            {
                chart = dp.Chart as Chart;
                stopAngle = startAngle + Math.PI * 2 * Math.Abs(dp.InternalYValue) / totalSum;
                meanAngle = (startAngle + stopAngle) / 2;

                if (dp.LabelStyle == LabelStyles.Inside)
                {
                    meanAngle = CircularLabel.ResetMeanAngle(meanAngle);
                    PlaceLabelInside(dp, center, meanAngle, pieSize, referenceEllipseSize, scaleY, is3D);
                    startAngle = stopAngle; 
                    continue;
                }

                // if (meanAngle > Math.PI * 2) meanAngle -= Math.PI * 2;

                cLabel = new CircularLabel(dp.LabelVisual, center, meanAngle, xRadiusLabel, yRadiusLabel, xRadiusChart, yRadiusChart, visual);
                cLabel.Boundary = boundingArea;

                if (prevousLabel != null)
                    prevousLabel.NextLabel = cLabel;

                cLabel.PreviusLabel = prevousLabel;
                prevousLabel = cLabel;

                circularLabels.Add(cLabel);

                startAngle = stopAngle;
            }

            if (circularLabels.Count > 0)
            {   
                circularLabels[0].PreviusLabel = cLabel;
                cLabel.NextLabel = circularLabels[0];
                LabelPlacementHelper.CircularLabelPlacment(boundingArea, circularLabels, (chart != null) ? chart.SmartLabelEnabled : false);
            }
        }

        private static void RearrangeLabels(List<DataPoint> dataPoints, Double leftOfArea, Double topOfArea, Double areaHeight, Double areaWidth)
        {
            Rect[] labelInfo = new Rect[dataPoints.Count];

            //labelsOutsideLeft = (from c in labelsOutsideLeft orderby c.LabelVisual.GetValue(Canvas.TopProperty) select c).ToList();

            int index = 0;

            foreach (DataPoint dataPoint in dataPoints)
            {
                if (dataPoint.LabelStyle == LabelStyles.OutSide)
                {
                    Double left = (Double)dataPoint.LabelVisual.GetValue(Canvas.LeftProperty);
                    Double top = (Double)dataPoint.LabelVisual.GetValue(Canvas.TopProperty);

                    labelInfo[index++] = new Rect(left, top, dataPoint.LabelVisual.Width, dataPoint.LabelVisual.Height);
                }
            }

            LabelPlacementHelper.VerticalLabelPlacement(new Rect(leftOfArea, topOfArea, areaWidth, areaHeight), ref labelInfo);

            index = 0;

            foreach (DataPoint dataPoint in dataPoints)
            {
                if (dataPoint.LabelStyle == LabelStyles.OutSide)
                {
                    //CreateAnEllipse(visual, new Point(labelInfo[ii].Left, labelInfo[ii].Top));
                    dataPoint.LabelVisual.SetValue(Canvas.LeftProperty, labelInfo[index].Left);
                    dataPoint.LabelVisual.SetValue(Canvas.TopProperty, labelInfo[index].Top + topOfArea);
                    index++;
                }
                //dataPoint.LabelVisual.SetValue(Canvas.TopProperty, labelInfo[ii].Top - labelInfo[ii++].Height);
            }
        }

        private static void PlaceLabelInside(DataPoint dataPoint, Point center, Double meanAngle,
            Size pieSize, Size referenceEllipseSize, Double scaleY, Boolean is3D)
        {
            Double hInnerEllipseRadius = (pieSize.Width / (is3D ? 1 : 2)) * 0.7;
            Double vInnerEllipseRadius = (pieSize.Height / (is3D ? 1 : 2)) * 0.7 * scaleY;
            Double outerRadius = Math.Min(pieSize.Width, pieSize.Height) / (is3D ? 1 : 2);

            RenderAs renderAs = dataPoint.Parent.RenderAs;
            Double xPos, yPos;

            if (is3D)
            {
                xPos = center.X + hInnerEllipseRadius * Math.Cos(meanAngle) - dataPoint.LabelVisual.Width;
                yPos = center.Y + vInnerEllipseRadius * Math.Sin(meanAngle) - dataPoint.LabelVisual.Height;
            }
            else
            {
                if (renderAs == RenderAs.Doughnut)
                {
                    xPos = center.X + 1.9 * (outerRadius / 3) * Math.Cos(meanAngle);
                    yPos = center.Y + 1.9 * (outerRadius / 3) * Math.Sin(meanAngle);
                }
                else
                {
                    xPos = center.X + 1.7 * (outerRadius / 3) * Math.Cos(meanAngle);
                    yPos = center.Y + 1.7 * (outerRadius / 3) * Math.Sin(meanAngle);
                }
            }

            xPos = xPos - dataPoint.LabelVisual.Width / 2;
            yPos = yPos - dataPoint.LabelVisual.Height / 2;

            dataPoint.LabelVisual.SetValue(Canvas.TopProperty, yPos);
            dataPoint.LabelVisual.SetValue(Canvas.LeftProperty, xPos);
        }

        private static Int32 ComparePointY(Point a, Point b)
        {
            return a.Y.CompareTo(b.Y);
        }

        private static Int32 ComparePointX(Point a, Point b)
        {
            return a.X.CompareTo(b.X);
        }

        private static Int32 CompareRectY(Rect a, Rect b)
        {
            return a.Y.CompareTo(b.Y);
        }

        /// <summary>
        /// Calculates the slope of the line joining the two points
        /// </summary>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        /// <returns></returns>
        private static Double LineSlope(Point point1, Point point2)
        {
            return (point2.Y - point1.Y) / (point2.X - point1.X);
        }

        /// <summary>
        /// Create and position labels position for Pie / Doughnut
        /// </summary>
        /// <param name="totalSum">Total YValue sum from all dataPoints</param>
        /// <param name="dataPoints">List of dataPoints</param>
        /// <param name="width">Chart canvas width</param>
        /// <param name="height">Chart canvas height</param>
        /// <param name="scaleY">Scale Y</param>
        /// <param name="is3D">Whether a 3D chart</param>
        /// <param name="size">Pie canvas size</param>
        /// <returns>Canvas</returns>
        private static Canvas CreateAndPositionLabels(Double totalSum, List<DataPoint> dataPoints, Double width, Double height, Double scaleY, Boolean is3D, ref Size size)
        {
            Canvas visual = new Canvas() { Height = height, Width = width };
            List<DataPoint> labelsToBePlaced = new List<DataPoint>();

            Double labelLineLength = LABEL_LINE_LENGTH;

            Boolean isLabelEnabled = false;
            Boolean isLabelOutside = false;
            Double maxLabelWidth = 0;
            Double maxLabelHeight = 0;

            foreach (DataPoint dataPoint in dataPoints)
            {
                //if (dataPoint.InternalYValue == 0)
                //    continue;

                Canvas label = CreateLabel(dataPoint);
                if ((bool)dataPoint.LabelEnabled)
                {
                    maxLabelWidth = Math.Max(maxLabelWidth, label.Width);
                    maxLabelHeight = Math.Max(maxLabelHeight, label.Height);

                    isLabelEnabled = true;

                    if (dataPoint.LabelStyle == LabelStyles.OutSide)
                        isLabelOutside = true;
                }
                else
                    label.Visibility = Visibility.Collapsed;

                labelsToBePlaced.Add(dataPoint);

                if (isLabelEnabled)
                    visual.Children.Add(label);
            }

            // This is to offset the label to draw label line
            maxLabelWidth += 10;

            Double pieCanvasWidth = 0;
            Double pieCanvasHeight = 0;

            Double labelEllipseWidth = 0;
            Double labelEllipseHeight = 0;

            Double minLength = (width - maxLabelWidth) * scaleY;
            if ((width - maxLabelWidth) > height)
            {
                minLength = Math.Min(minLength, height);
            }

            if (isLabelEnabled)
            {
                if (isLabelOutside)
                {
                    if (is3D)
                    {
                        pieCanvasWidth = minLength - labelLineLength * 2;
                        pieCanvasHeight = pieCanvasWidth;

                        labelEllipseWidth = pieCanvasWidth * 2 + labelLineLength * 2;
                        labelEllipseHeight = labelEllipseWidth * scaleY + LABEL_LINE_LENGTH * 2;
                    }
                    else
                    {   
                        pieCanvasWidth = minLength - maxLabelWidth - LABEL_LINE_LENGTH / 2;

                        if (pieCanvasWidth < Math.Min(width, height) / 3)
                            pieCanvasWidth = Math.Min(width, height) / 3;

                        pieCanvasHeight = Math.Abs(pieCanvasWidth);

                        labelEllipseWidth = Math.Abs(pieCanvasWidth) + LABEL_LINE_LENGTH;
                        labelEllipseHeight = labelEllipseWidth;// -LABEL_LINE_LENGTH / 2;
                    }
                       
                    PositionLabels(visual, totalSum, dataPoints, new Size(Math.Abs(pieCanvasWidth), Math.Abs(pieCanvasHeight)), new Size(Math.Abs(labelEllipseWidth), Math.Abs(labelEllipseHeight)), new Size(width, height), scaleY, is3D);
                }   
                else
                {   
                    pieCanvasWidth = minLength;
                    pieCanvasHeight = minLength;

                    labelEllipseWidth = pieCanvasWidth;
                    labelEllipseHeight = pieCanvasHeight;

                    PositionLabels(visual, totalSum, dataPoints, new Size(Math.Abs(pieCanvasWidth), Math.Abs(pieCanvasHeight)), new Size(Math.Abs(labelEllipseWidth), Math.Abs(labelEllipseHeight)), new Size(width, height), scaleY, is3D);
                }
            }
            else
            {   
                pieCanvasWidth = minLength;
                pieCanvasHeight = minLength;
            }

            pieCanvasWidth -= (pieCanvasWidth > 10) ? pieCanvasWidth * 0.1 : 0;
            pieCanvasHeight -= (pieCanvasHeight > 10) ? pieCanvasWidth * 0.1 : 0;


            size = new Size(Math.Abs(pieCanvasWidth), Math.Abs(pieCanvasHeight));

            if (isLabelEnabled)
                return visual;
            else
                return null;
        }

        /// <summary>
        /// Returns a pie in a canvas
        /// </summary>
        /// <param name="faces">Pie faces</param>
        /// <param name="pieParams">Pie parameters</param>
        /// <param name="unExplodedPoints">UnExploded dataPoints</param>
        /// <param name="explodedPoints">Exploded dataPoints</param>
        /// <param name="labelLinePath">Label line path</param>
        /// <param name="enabledDataPoints">List of enabled dataPoints</param>
        /// <returns>Canvas</returns>
        private static Canvas GetPie2D(DataSeries currentDataSeries, ref Faces faces, SectorChartShapeParams pieParams, ref PieDoughnut2DPoints unExplodedPoints, ref PieDoughnut2DPoints explodedPoints, ref Path labelLinePath, List<DataPoint> enabledDataPoints)
        {
            var noOfNonZeroDataPoint = (from dp in enabledDataPoints where dp.InternalYValue != 0 select dp);

            Canvas visual = new Canvas();

            Double width = pieParams.OuterRadius * 2;
            Double height = pieParams.OuterRadius * 2;

            visual.Width = width;
            visual.Height = height;

            Point center = new Point(width / 2, height / 2);
            Double xOffset = pieParams.OuterRadius * pieParams.ExplodeRatio * Math.Cos(pieParams.MeanAngle);
            Double yOffset = pieParams.OuterRadius * pieParams.ExplodeRatio * Math.Sin(pieParams.MeanAngle);

            #region PieSlice

            if (pieParams.StartAngle != pieParams.StopAngle || !pieParams.IsZero)
            {
                Ellipse ellipse = new Ellipse() { Tag = new ElementData() { Element = pieParams.TagReference } };
                ellipse.Width = width;
                ellipse.Height = height;
                ellipse.Fill = pieParams.Lighting ? Graphics.GetLightingEnabledBrush(pieParams.Background, "Radial", new Double[] { 0.99, 0.745 }) : pieParams.Background;

                Point start = new Point();
                Point end = new Point();
                Point arcMidPoint = new Point();

                start.X = center.X + pieParams.OuterRadius * Math.Cos(pieParams.StartAngle);
                start.Y = center.Y + pieParams.OuterRadius * Math.Sin(pieParams.StartAngle);

                end.X = center.X + pieParams.OuterRadius * Math.Cos(pieParams.StopAngle);
                end.Y = center.Y + pieParams.OuterRadius * Math.Sin(pieParams.StopAngle);

                arcMidPoint.X = center.X + pieParams.OuterRadius * Math.Cos(pieParams.MeanAngle);
                arcMidPoint.Y = center.Y + pieParams.OuterRadius * Math.Sin(pieParams.MeanAngle);

                List<PathGeometryParams> clipPathGeometry = new List<PathGeometryParams>();
                clipPathGeometry.Add(new LineSegmentParams(start));
                clipPathGeometry.Add(new ArcSegmentParams(new Size(pieParams.OuterRadius, pieParams.OuterRadius), 0, ((pieParams.StopAngle - pieParams.StartAngle) > Math.PI) ? true : false, SweepDirection.Clockwise, pieParams.AnimationEnabled ? start : arcMidPoint));
                clipPathGeometry.Add(new ArcSegmentParams(new Size(pieParams.OuterRadius, pieParams.OuterRadius), 0, ((pieParams.StopAngle - pieParams.StartAngle) > Math.PI) ? true : false, SweepDirection.Clockwise, pieParams.AnimationEnabled ? start : end));
                clipPathGeometry.Add(new LineSegmentParams(center));
                ellipse.Clip = GetPathGeometryFromList(FillRule.Nonzero, center, clipPathGeometry, true);
                PathSegmentCollection segments = (ellipse.Clip as PathGeometry).Figures[0].Segments;

                // apply animation to the individual points that for the pie slice
                if (pieParams.AnimationEnabled)
                {
                    // apply animation to the points
                    pieParams.Storyboard = CreatePathSegmentAnimation(currentDataSeries, pieParams.DataPoint, pieParams.Storyboard, segments[1], center, pieParams.OuterRadius, currentDataSeries.InternalStartAngle, pieParams.MeanAngle);
                    pieParams.Storyboard = CreatePathSegmentAnimation(currentDataSeries, pieParams.DataPoint, pieParams.Storyboard, segments[2], center, pieParams.OuterRadius, currentDataSeries.InternalStartAngle, pieParams.StopAngle);
                    pieParams.Storyboard = CreatePathSegmentAnimation(currentDataSeries, pieParams.DataPoint, pieParams.Storyboard, segments[0], center, pieParams.OuterRadius, currentDataSeries.InternalStartAngle, pieParams.StartAngle);
                }

                faces.Parts.Add(ellipse);
                visual.Children.Add(ellipse);

                // set the un exploded points for interactivity
                unExplodedPoints.Center = center;
                unExplodedPoints.OuterArcStart = start;
                unExplodedPoints.OuterArcMid = arcMidPoint;
                unExplodedPoints.OuterArcEnd = end;

                // set the exploded points for interactivity
                explodedPoints.Center = new Point(center.X + xOffset, center.Y + yOffset);
                explodedPoints.OuterArcStart = new Point(start.X + xOffset, start.Y + yOffset);
                explodedPoints.OuterArcMid = new Point(arcMidPoint.X + xOffset, arcMidPoint.Y + yOffset);
                explodedPoints.OuterArcEnd = new Point(end.X + xOffset, end.Y + yOffset);

                if (enabledDataPoints.Count == 1 || noOfNonZeroDataPoint.Count() == 1)
                {
                    Ellipse borderEllipse = new Ellipse() { IsHitTestVisible = false, Height = ellipse.Height, Width = ellipse.Width };
                    borderEllipse.SetValue(Canvas.ZIndexProperty, (Int32)10000);
                    visual.Children.Add(borderEllipse);
                    faces.BorderElements.Add(borderEllipse);
                }
            }

            #endregion PieSlice

            #region Lighting

            if ((pieParams.TagReference as DataPoint).InternalYValue != 0 && pieParams.Lighting && (pieParams.StartAngle != pieParams.StopAngle || !pieParams.IsZero))
            {
                Ellipse lightingEllipse = new Ellipse() { Tag = new ElementData() { Element = pieParams.TagReference } };
                lightingEllipse.Width = width;
                lightingEllipse.Height = height;
                lightingEllipse.IsHitTestVisible = false;
                lightingEllipse.Fill = GetPieGradianceBrush();

                Point start = new Point();
                Point end = new Point();
                Point arcMidPoint = new Point();

                start.X = center.X + pieParams.OuterRadius * Math.Cos(pieParams.StartAngle);
                start.Y = center.Y + pieParams.OuterRadius * Math.Sin(pieParams.StartAngle);

                end.X = center.X + pieParams.OuterRadius * Math.Cos(pieParams.StopAngle);
                end.Y = center.Y + pieParams.OuterRadius * Math.Sin(pieParams.StopAngle);

                arcMidPoint.X = center.X + pieParams.OuterRadius * Math.Cos(pieParams.MeanAngle);
                arcMidPoint.Y = center.Y + pieParams.OuterRadius * Math.Sin(pieParams.MeanAngle);

                List<PathGeometryParams> clipPathGeometry = new List<PathGeometryParams>();
                clipPathGeometry.Add(new LineSegmentParams(start));
                clipPathGeometry.Add(new ArcSegmentParams(new Size(pieParams.OuterRadius, pieParams.OuterRadius), 0, ((pieParams.StopAngle - pieParams.StartAngle) > Math.PI) ? true : false, SweepDirection.Clockwise, pieParams.AnimationEnabled ? start : arcMidPoint));
                clipPathGeometry.Add(new ArcSegmentParams(new Size(pieParams.OuterRadius, pieParams.OuterRadius), 0, ((pieParams.StopAngle - pieParams.StartAngle) > Math.PI) ? true : false, SweepDirection.Clockwise, pieParams.AnimationEnabled ? start : end));
                clipPathGeometry.Add(new LineSegmentParams(center));
                lightingEllipse.Clip = GetPathGeometryFromList(FillRule.Nonzero, center, clipPathGeometry, true);
                PathSegmentCollection segments = (lightingEllipse.Clip as PathGeometry).Figures[0].Segments;

                // apply animation to the individual points that for the shape that
                // gives the lighting effect to the pie slice
                if (pieParams.AnimationEnabled)
                {
                    // apply animation to the points
                    pieParams.Storyboard = CreatePathSegmentAnimation(currentDataSeries, pieParams.DataPoint, pieParams.Storyboard, segments[1], center, pieParams.OuterRadius, currentDataSeries.InternalStartAngle, pieParams.MeanAngle);
                    pieParams.Storyboard = CreatePathSegmentAnimation(currentDataSeries, pieParams.DataPoint, pieParams.Storyboard, segments[2], center, pieParams.OuterRadius, currentDataSeries.InternalStartAngle, pieParams.StopAngle);
                    pieParams.Storyboard = CreatePathSegmentAnimation(currentDataSeries, pieParams.DataPoint, pieParams.Storyboard, segments[0], center, pieParams.OuterRadius, currentDataSeries.InternalStartAngle, pieParams.StartAngle);
                }
                visual.Children.Add(lightingEllipse);
            }

            #endregion Lighting

            #region LabelLine

            if (pieParams.LabelLineEnabled)
            {
                Path labelLine = new Path() { Tag = new ElementData() { Element = pieParams.TagReference } };
                Double meanAngle = pieParams.MeanAngle;
                labelLine.SetValue(Canvas.ZIndexProperty, -100000);
                Point piePoint = new Point();
                piePoint.X = center.X + pieParams.OuterRadius * Math.Cos(meanAngle);
                piePoint.Y = center.Y + pieParams.OuterRadius * Math.Sin(meanAngle);

                Point labelPoint = new Point();
                labelPoint.X = center.X + pieParams.LabelPoint.X - pieParams.Width / 2;
                labelPoint.Y = center.Y + pieParams.LabelPoint.Y - pieParams.Height / 2;

                Point midPoint = new Point();
                // midPoint.X = (labelPoint.X < center.X) ? labelPoint.X + 10 : labelPoint.X - 10;
                if (pieParams.LabelLineTargetToRight)
                    midPoint.X = labelPoint.X + 10;
                else
                    midPoint.X = labelPoint.X - 10;

                midPoint.Y = labelPoint.Y;


                List<PathGeometryParams> labelLinePathGeometry = new List<PathGeometryParams>();
                labelLinePathGeometry.Add(new LineSegmentParams(pieParams.AnimationEnabled ? piePoint : midPoint));
                labelLinePathGeometry.Add(new LineSegmentParams(pieParams.AnimationEnabled ? piePoint : labelPoint));
                labelLine.Data = GetPathGeometryFromList(FillRule.Nonzero, piePoint, labelLinePathGeometry, true);
                PathFigure figure = (labelLine.Data as PathGeometry).Figures[0];
                PathSegmentCollection segments = figure.Segments;
                figure.IsClosed = false;
                figure.IsFilled = false;

                // animate the label lines of the individual pie slices
                if (pieParams.AnimationEnabled)
                {
                    pieParams.Storyboard = CreateLabelLineAnimation(currentDataSeries, pieParams.DataPoint, pieParams.Storyboard, segments[0], piePoint, midPoint);
                    pieParams.Storyboard = CreateLabelLineAnimation(currentDataSeries, pieParams.DataPoint, pieParams.Storyboard, segments[1], piePoint, midPoint, labelPoint);
                }

                labelLine.Stroke = pieParams.LabelLineColor;
                labelLine.StrokeDashArray = pieParams.LabelLineStyle;
                labelLine.StrokeThickness = pieParams.LabelLineThickness;

                labelLinePath = labelLine;

                if ((pieParams.TagReference as DataPoint).InternalYValue == 0)
                {
                    Line zeroLine = new Line();
                    zeroLine.X1 = center.X;
                    zeroLine.Y1 = center.Y;

                    zeroLine.X2 = piePoint.X;
                    zeroLine.Y2 = piePoint.Y;
                    zeroLine.Stroke = pieParams.LabelLineColor;
                    zeroLine.StrokeThickness = 0.25;
                    zeroLine.IsHitTestVisible = false;
                    visual.Children.Add(zeroLine);

                    if (pieParams.AnimationEnabled)
                    {
                        pieParams.Storyboard = CreateOpacityAnimation(currentDataSeries, pieParams.DataPoint, pieParams.Storyboard, zeroLine, 2, zeroLine.Opacity, 0.5);
                        zeroLine.Opacity = 0;
                    }
                }

                visual.Children.Add(labelLine);

                // set the un exploded points for interactivity
                unExplodedPoints.LabelLineEndPoint = labelPoint;
                unExplodedPoints.LabelLineMidPoint = midPoint;
                unExplodedPoints.LabelLineStartPoint = piePoint;

                // set the exploded points for interactivity
                explodedPoints.LabelLineEndPoint = new Point(labelPoint.X, labelPoint.Y - yOffset);
                explodedPoints.LabelLineMidPoint = new Point(midPoint.X, midPoint.Y - yOffset);
                explodedPoints.LabelLineStartPoint = new Point(piePoint.X + xOffset, piePoint.Y + yOffset);

                if ((pieParams.TagReference as DataPoint).InternalYValue == 0)
                    labelLine.IsHitTestVisible = false;
            }

            #endregion LabelLine

            #region Create path for selecting a pie section

            Point startPoint = new Point();
            Point endPoint = new Point();

            startPoint.X = center.X + pieParams.OuterRadius * Math.Cos(pieParams.StartAngle);
            startPoint.Y = center.Y + pieParams.OuterRadius * Math.Sin(pieParams.StartAngle);

            endPoint.X = center.X + pieParams.OuterRadius * Math.Cos(pieParams.StopAngle);
            endPoint.Y = center.Y + pieParams.OuterRadius * Math.Sin(pieParams.StopAngle);

            List<PathGeometryParams> pathGeometry = new List<PathGeometryParams>();
            pathGeometry.Add(new LineSegmentParams(center));
            pathGeometry.Add(new LineSegmentParams(startPoint));
            pathGeometry.Add(new ArcSegmentParams(new Size(pieParams.OuterRadius, pieParams.OuterRadius), 0, pieParams.IsLargerArc, SweepDirection.Clockwise, endPoint));

            if ((pieParams.TagReference as DataPoint).InternalYValue != 0)
            {
                Path p = new Path() { IsHitTestVisible = false };
                p.SetValue(Canvas.ZIndexProperty, (Int32)10000);
                p.Data = GetPathGeometryFromList(FillRule.Nonzero, center, pathGeometry, true);
                visual.Children.Add(p);
                faces.BorderElements.Add(p);
            }
            #endregion

            #region Bevel

            if (pieParams.Bevel && Math.Abs(pieParams.StartAngle - pieParams.StopAngle) > 0.03 && (pieParams.StartAngle != pieParams.StopAngle))
            {
                //Point startPoint = new Point();
                //Point endPoint = new Point();

                //startPoint.X = center.X + pieParams.OuterRadius * Math.Cos(pieParams.StartAngle);
                //startPoint.Y = center.Y + pieParams.OuterRadius * Math.Sin(pieParams.StartAngle);

                //endPoint.X = center.X + pieParams.OuterRadius * Math.Cos(pieParams.StopAngle);
                //endPoint.Y = center.Y + pieParams.OuterRadius * Math.Sin(pieParams.StopAngle);

                Point bevelCenter = new Point();
                Point bevelStart = new Point();
                Point bevelEnd = new Point();
                Double bevelLength = 4;
                Double bevelOuterRadius = Math.Abs(pieParams.OuterRadius - bevelLength);

                bevelCenter.X = center.X + bevelLength * Math.Cos(pieParams.MeanAngle);
                bevelCenter.Y = center.Y + bevelLength * Math.Sin(pieParams.MeanAngle);

                bevelStart.X = center.X + bevelOuterRadius * Math.Cos(pieParams.StartAngle + 0.03);
                bevelStart.Y = center.Y + bevelOuterRadius * Math.Sin(pieParams.StartAngle + 0.03);

                bevelEnd.X = center.X + bevelOuterRadius * Math.Cos(pieParams.StopAngle - 0.03);
                bevelEnd.Y = center.Y + bevelOuterRadius * Math.Sin(pieParams.StopAngle - 0.03);

                pathGeometry = new List<PathGeometryParams>();
                pathGeometry.Add(new LineSegmentParams(center));
                pathGeometry.Add(new LineSegmentParams(startPoint));
                pathGeometry.Add(new LineSegmentParams(bevelStart));
                pathGeometry.Add(new LineSegmentParams(bevelCenter));

                Path path = new Path() { Tag = new ElementData() { Element = pieParams.TagReference } };

                path.Data = GetPathGeometryFromList(FillRule.Nonzero, bevelCenter, pathGeometry, true);

                if (pieParams.StartAngle > Math.PI * 0.5 && pieParams.StartAngle <= Math.PI * 1.5)
                {
                    path.Fill = GetDarkerBevelBrush(pieParams.Background, pieParams.StartAngle * 180 / Math.PI + 135);
                }
                else
                {
                    path.Fill = GetLighterBevelBrush(pieParams.Background, -pieParams.StartAngle * 180 / Math.PI);
                }

                // Apply animation to the beveling path
                if (pieParams.AnimationEnabled)
                {
                    pieParams.Storyboard = CreateOpacityAnimation(currentDataSeries, pieParams.DataPoint, pieParams.Storyboard, path, 1, 1, 1);
                    path.Opacity = 0;
                }

                faces.Parts.Add(path);
                visual.Children.Add(path);

                pathGeometry = new List<PathGeometryParams>();
                pathGeometry.Add(new LineSegmentParams(center));
                pathGeometry.Add(new LineSegmentParams(endPoint));
                pathGeometry.Add(new LineSegmentParams(bevelEnd));
                pathGeometry.Add(new LineSegmentParams(bevelCenter));

                path = new Path() { Tag = new ElementData() { Element = pieParams.TagReference } };
                path.Data = GetPathGeometryFromList(FillRule.Nonzero, bevelCenter, pathGeometry, true);
                if (pieParams.StopAngle > Math.PI * 0.5 && pieParams.StopAngle <= Math.PI * 1.5)
                {
                    path.Fill = GetLighterBevelBrush(pieParams.Background, pieParams.StopAngle * 180 / Math.PI + 135);
                }
                else
                {
                    path.Fill = GetDarkerBevelBrush(pieParams.Background, -pieParams.StopAngle * 180 / Math.PI);
                }

                // Apply animation to the beveling path
                if (pieParams.AnimationEnabled)
                {
                    pieParams.Storyboard = CreateOpacityAnimation(currentDataSeries, pieParams.DataPoint, pieParams.Storyboard, path, 1, 1, 1);
                    path.Opacity = 0;
                }

                faces.Parts.Add(path);
                visual.Children.Add(path);

                #region "Outer Bevel"
                Shape outerBevel;

                if (enabledDataPoints.Count == 1 || noOfNonZeroDataPoint.Count() == 1)
                {
                    outerBevel = new Ellipse() { Height = pieParams.OuterRadius * 2, Width = pieParams.OuterRadius * 2, Tag = new ElementData() { Element = pieParams.TagReference } };
                    GeometryGroup gg = new GeometryGroup();
                    gg.Children.Add(new EllipseGeometry() { Center = new Point(pieParams.OuterRadius, pieParams.OuterRadius), RadiusX = pieParams.OuterRadius, RadiusY = pieParams.OuterRadius });
                    gg.Children.Add(new EllipseGeometry() { Center = new Point(pieParams.OuterRadius, pieParams.OuterRadius), RadiusX = bevelOuterRadius, RadiusY = bevelOuterRadius });
                    outerBevel.Clip = gg;
                }
                else
                {
                    pathGeometry = new List<PathGeometryParams>();
                    pathGeometry.Add(new LineSegmentParams(endPoint));
                    pathGeometry.Add(new ArcSegmentParams(new Size(pieParams.OuterRadius, pieParams.OuterRadius), 0, pieParams.IsLargerArc, SweepDirection.Counterclockwise, startPoint));
                    pathGeometry.Add(new LineSegmentParams(bevelStart));
                    pathGeometry.Add(new ArcSegmentParams(new Size(bevelOuterRadius, bevelOuterRadius), 0, pieParams.IsLargerArc, SweepDirection.Clockwise, bevelEnd));

                    outerBevel = new Path() { Tag = new ElementData() { Element = pieParams.TagReference } };
                    (outerBevel as Path).Data = GetPathGeometryFromList(FillRule.Nonzero, bevelEnd, pathGeometry, true);
                }

                if (pieParams.MeanAngle > 0 && pieParams.MeanAngle < Math.PI)
                {
                    outerBevel.Fill = GetCurvedBevelBrush(pieParams.Background, pieParams.MeanAngle * 180 / Math.PI + 90, Graphics.GenerateDoubleCollection(-0.745, -0.85), Graphics.GenerateDoubleCollection(0, 1));
                }
                else
                {
                    outerBevel.Fill = GetCurvedBevelBrush(pieParams.Background, pieParams.MeanAngle * 180 / Math.PI + 90, Graphics.GenerateDoubleCollection(0.745, -0.99), Graphics.GenerateDoubleCollection(0, 1));
                }

                // Apply animation to the beveling path
                if (pieParams.AnimationEnabled)
                {
                    pieParams.Storyboard = CreateOpacityAnimation(currentDataSeries, pieParams.DataPoint, pieParams.Storyboard, outerBevel, 1, 1, 1);
                    outerBevel.Opacity = 0;
                }


                #endregion
                faces.Parts.Add(outerBevel);
                visual.Children.Add(outerBevel);
            }
            else
            {
                faces.Parts.Add(null);
                faces.Parts.Add(null);
                faces.Parts.Add(null);
            }

            #endregion LabelLine

            return visual;
        }

        /// <summary>
        /// Returns a doughnut in a canvas
        /// </summary>
        /// <param name="faces">Doughnut faces</param>
        /// <param name="doughnutParams">Doughnut parameters</param>
        /// <param name="unExplodedPoints">UnExploded dataPoints</param>
        /// <param name="explodedPoints">Exploded dataPoints</param>
        /// <param name="labelLinePath">Label line path</param>
        /// <param name="enabledDataPoints">List of enabled dataPoints</param>
        /// <returns>Canvas</returns>
        private static Canvas GetDoughnut2D(DataSeries currentDataSeries, ref Faces faces, SectorChartShapeParams doughnutParams, ref PieDoughnut2DPoints unExplodedPoints, ref PieDoughnut2DPoints explodedPoints, ref Path labelLinePath, List<DataPoint> enabledDataPoints)
        {
            var noOfNonZeroDataPoint = (from dp in enabledDataPoints where dp.InternalYValue != 0 select dp);

            Canvas visual = new Canvas() { Tag = new ElementData() { Element = doughnutParams.TagReference } };
            Canvas PieVisual = new Canvas() { Tag = new ElementData() { Element = doughnutParams.TagReference } };

            Double width = doughnutParams.OuterRadius * 2;
            Double height = doughnutParams.OuterRadius * 2;

            visual.Width = width;
            visual.Height = height;
            PieVisual.Width = width;
            PieVisual.Height = height;

            visual.Children.Add(PieVisual);

            Point center = new Point(width / 2, height / 2);
            Double xOffset = doughnutParams.OuterRadius * doughnutParams.ExplodeRatio * Math.Cos(doughnutParams.MeanAngle);
            Double yOffset = doughnutParams.OuterRadius * doughnutParams.ExplodeRatio * Math.Sin(doughnutParams.MeanAngle);

            #region Doughnut Slice

            if (doughnutParams.StartAngle != doughnutParams.StopAngle || !doughnutParams.IsZero)
            {
                Ellipse ellipse = new Ellipse() { Tag = new ElementData() { Element = doughnutParams.TagReference } };
                ellipse.Width = width;
                ellipse.Height = height;
                ellipse.Fill = doughnutParams.Lighting ? Graphics.GetLightingEnabledBrush(doughnutParams.Background, "Radial", new Double[] { 0.99, 0.745 }) : doughnutParams.Background;

                Point start = new Point();
                Point end = new Point();
                Point arcMidPoint = new Point();
                Point innerstart = new Point();
                Point innerend = new Point();
                Point innerArcMidPoint = new Point();

                start.X = center.X + doughnutParams.OuterRadius * Math.Cos(doughnutParams.StartAngle);
                start.Y = center.Y + doughnutParams.OuterRadius * Math.Sin(doughnutParams.StartAngle);

                end.X = center.X + doughnutParams.OuterRadius * Math.Cos(doughnutParams.StopAngle);
                end.Y = center.Y + doughnutParams.OuterRadius * Math.Sin(doughnutParams.StopAngle);

                arcMidPoint.X = center.X + doughnutParams.OuterRadius * Math.Cos(doughnutParams.MeanAngle);
                arcMidPoint.Y = center.Y + doughnutParams.OuterRadius * Math.Sin(doughnutParams.MeanAngle);

                innerstart.X = center.X + doughnutParams.InnerRadius * Math.Cos(doughnutParams.StartAngle);
                innerstart.Y = center.Y + doughnutParams.InnerRadius * Math.Sin(doughnutParams.StartAngle);

                innerend.X = center.X + doughnutParams.InnerRadius * Math.Cos(doughnutParams.StopAngle);
                innerend.Y = center.Y + doughnutParams.InnerRadius * Math.Sin(doughnutParams.StopAngle);

                innerArcMidPoint.X = center.X + doughnutParams.InnerRadius * Math.Cos(doughnutParams.MeanAngle);
                innerArcMidPoint.Y = center.Y + doughnutParams.InnerRadius * Math.Sin(doughnutParams.MeanAngle);

                List<PathGeometryParams> clipPathGeometry = new List<PathGeometryParams>();
                clipPathGeometry.Add(new LineSegmentParams(start));
                clipPathGeometry.Add(new ArcSegmentParams(new Size(doughnutParams.OuterRadius, doughnutParams.OuterRadius), 0, ((doughnutParams.StopAngle - doughnutParams.StartAngle) > Math.PI) ? true : false, SweepDirection.Clockwise, doughnutParams.AnimationEnabled ? start : arcMidPoint));
                clipPathGeometry.Add(new ArcSegmentParams(new Size(doughnutParams.OuterRadius, doughnutParams.OuterRadius), 0, ((doughnutParams.StopAngle - doughnutParams.StartAngle) > Math.PI) ? true : false, SweepDirection.Clockwise, doughnutParams.AnimationEnabled ? start : end));
                clipPathGeometry.Add(new LineSegmentParams(doughnutParams.AnimationEnabled ? innerstart : innerend));

                ellipse.Clip = GetPathGeometryFromList(FillRule.Nonzero, innerstart, clipPathGeometry, true);
                PathGeometry pg = GetPathGeometryFromList(FillRule.Nonzero, innerstart, clipPathGeometry, true);

                PathFigure figure = (ellipse.Clip as PathGeometry).Figures[0];
                // PathFigure figure = (pg as PathGeometry).Figures[0];
                PathSegmentCollection segments = figure.Segments;

                GeometryGroup gg = new GeometryGroup();
                gg.Children.Add(new EllipseGeometry() { Center = center, RadiusX = doughnutParams.OuterRadius, RadiusY = doughnutParams.OuterRadius });
                gg.Children.Add(new EllipseGeometry() { Center = center, RadiusX = doughnutParams.InnerRadius, RadiusY = doughnutParams.InnerRadius });

                PieVisual.Clip = gg;

                #region Create path for selecting a Doughnut Section

                List<PathGeometryParams> pathGeometry = new List<PathGeometryParams>();

                pathGeometry.Add(new LineSegmentParams(end));
                pathGeometry.Add(new ArcSegmentParams(new Size(doughnutParams.OuterRadius, doughnutParams.OuterRadius), 0, doughnutParams.IsLargerArc, SweepDirection.Counterclockwise, start));
                pathGeometry.Add(new LineSegmentParams(innerstart));
                pathGeometry.Add(new ArcSegmentParams(new Size(doughnutParams.InnerRadius, doughnutParams.InnerRadius), 0, doughnutParams.IsLargerArc, SweepDirection.Clockwise, innerend));
                pathGeometry.Add(new LineSegmentParams(innerend));
                pathGeometry.Add(new LineSegmentParams(end));

                if ((doughnutParams.TagReference as DataPoint).InternalYValue != 0)
                {
                    Path path4Selection = new Path() { IsHitTestVisible = false };
                    path4Selection.SetValue(Canvas.ZIndexProperty, (Int32)100000);
                    path4Selection.Data = GetPathGeometryFromList(FillRule.Nonzero, end, pathGeometry, false);
                    visual.Children.Add(path4Selection);
                    faces.BorderElements.Add(path4Selection);
                }
                #endregion

                // Apply animation to the doughnut slice
                if (doughnutParams.AnimationEnabled)
                {
                    // animate the outer points
                    doughnutParams.Storyboard = CreatePathSegmentAnimation(currentDataSeries, doughnutParams.DataPoint, doughnutParams.Storyboard, segments[0], center, doughnutParams.OuterRadius, currentDataSeries.InternalStartAngle, doughnutParams.StartAngle);
                    doughnutParams.Storyboard = CreatePathSegmentAnimation(currentDataSeries, doughnutParams.DataPoint,doughnutParams.Storyboard, segments[1], center, doughnutParams.OuterRadius, currentDataSeries.InternalStartAngle, doughnutParams.MeanAngle);
                    doughnutParams.Storyboard = CreatePathSegmentAnimation(currentDataSeries,doughnutParams.DataPoint,doughnutParams.Storyboard, segments[2], center, doughnutParams.OuterRadius, currentDataSeries.InternalStartAngle, doughnutParams.StopAngle);

                    // animate the inner points
                    doughnutParams.Storyboard = CreatePathSegmentAnimation(currentDataSeries, doughnutParams.DataPoint, doughnutParams.Storyboard, segments[3], center, doughnutParams.InnerRadius, currentDataSeries.InternalStartAngle, doughnutParams.StopAngle);
                    doughnutParams.Storyboard = CreatePathFigureAnimation(currentDataSeries, doughnutParams.DataPoint, doughnutParams.Storyboard, figure, center, doughnutParams.InnerRadius, currentDataSeries.InternalStartAngle, doughnutParams.StartAngle);
                }

                faces.Parts.Add(ellipse);
                PieVisual.Children.Add(ellipse);

                // set the un exploded points for interactivity
                unExplodedPoints.Center = center;
                unExplodedPoints.OuterArcStart = start;
                unExplodedPoints.OuterArcMid = arcMidPoint;
                unExplodedPoints.OuterArcEnd = end;
                unExplodedPoints.InnerArcStart = innerstart;
                unExplodedPoints.InnerArcMid = innerArcMidPoint;
                unExplodedPoints.InnerArcEnd = innerend;

                // set the exploded points for interactivity
                explodedPoints.Center = new Point(center.X + xOffset, center.Y + yOffset);
                explodedPoints.OuterArcStart = new Point(start.X + xOffset, start.Y + yOffset);
                explodedPoints.OuterArcMid = new Point(arcMidPoint.X + xOffset, arcMidPoint.Y + yOffset);
                explodedPoints.OuterArcEnd = new Point(end.X + xOffset, end.Y + yOffset);
                explodedPoints.InnerArcStart = new Point(innerstart.X + xOffset, innerstart.Y + yOffset);
                explodedPoints.InnerArcMid = new Point(innerArcMidPoint.X + xOffset, innerArcMidPoint.Y + yOffset);
                explodedPoints.InnerArcEnd = new Point(innerend.X + xOffset, innerend.Y + yOffset);

                if (enabledDataPoints.Count == 1 || noOfNonZeroDataPoint.Count() == 1)
                {
                    if ((doughnutParams.TagReference as DataPoint).InternalYValue != 0)
                    {
                        Ellipse borderEllipse = new Ellipse() { IsHitTestVisible = false, Height = ellipse.Height, Width = ellipse.Width };
                        borderEllipse.SetValue(Canvas.ZIndexProperty, (Int32)10000);
                        visual.Children.Add(borderEllipse);
                        faces.BorderElements.Add(borderEllipse);

                        Ellipse InnerBorderEllipse = new Ellipse() { IsHitTestVisible = false, Height = doughnutParams.InnerRadius * 2, Width = doughnutParams.InnerRadius * 2 };
                        InnerBorderEllipse.SetValue(Canvas.ZIndexProperty, (Int32)10000);
                        InnerBorderEllipse.SetValue(Canvas.TopProperty, InnerBorderEllipse.Height / 2);
                        InnerBorderEllipse.SetValue(Canvas.LeftProperty, InnerBorderEllipse.Width / 2);
                        visual.Children.Add(InnerBorderEllipse);
                        faces.BorderElements.Add(InnerBorderEllipse);
                    }
                }
            }

            #endregion Doughnut Slice

            #region Lighting

            if (doughnutParams.Lighting)
            {
                Ellipse lightingEllipse = new Ellipse() { Tag = new ElementData() { Element = doughnutParams.TagReference } };
                lightingEllipse.Width = width;
                lightingEllipse.Height = height;
                lightingEllipse.IsHitTestVisible = false;
                lightingEllipse.Fill = GetDoughnutGradianceBrush();

                if (doughnutParams.StartAngle != doughnutParams.StopAngle || !doughnutParams.IsZero)
                {
                    Point start = new Point();
                    Point end = new Point();
                    Point arcMidPoint = new Point();
                    Point innerstart = new Point();
                    Point innerend = new Point();
                    Point innerArcMidPoint = new Point();

                    start.X = center.X + doughnutParams.OuterRadius * Math.Cos(doughnutParams.StartAngle);
                    start.Y = center.Y + doughnutParams.OuterRadius * Math.Sin(doughnutParams.StartAngle);

                    end.X = center.X + doughnutParams.OuterRadius * Math.Cos(doughnutParams.StopAngle);
                    end.Y = center.Y + doughnutParams.OuterRadius * Math.Sin(doughnutParams.StopAngle);

                    arcMidPoint.X = center.X + doughnutParams.OuterRadius * Math.Cos(doughnutParams.MeanAngle);
                    arcMidPoint.Y = center.Y + doughnutParams.OuterRadius * Math.Sin(doughnutParams.MeanAngle);

                    innerstart.X = center.X + doughnutParams.InnerRadius * Math.Cos(doughnutParams.StartAngle);
                    innerstart.Y = center.Y + doughnutParams.InnerRadius * Math.Sin(doughnutParams.StartAngle);

                    innerend.X = center.X + doughnutParams.InnerRadius * Math.Cos(doughnutParams.StopAngle);
                    innerend.Y = center.Y + doughnutParams.InnerRadius * Math.Sin(doughnutParams.StopAngle);

                    innerArcMidPoint.X = center.X + doughnutParams.InnerRadius * Math.Cos(doughnutParams.MeanAngle);
                    innerArcMidPoint.Y = center.Y + doughnutParams.InnerRadius * Math.Sin(doughnutParams.MeanAngle);

                    List<PathGeometryParams> clipPathGeometry = new List<PathGeometryParams>();
                    clipPathGeometry.Add(new LineSegmentParams(start));
                    clipPathGeometry.Add(new ArcSegmentParams(new Size(doughnutParams.OuterRadius, doughnutParams.OuterRadius), 0, ((doughnutParams.StopAngle - doughnutParams.StartAngle) > Math.PI) ? true : false, SweepDirection.Clockwise, doughnutParams.AnimationEnabled ? start : arcMidPoint));
                    clipPathGeometry.Add(new ArcSegmentParams(new Size(doughnutParams.OuterRadius, doughnutParams.OuterRadius), 0, ((doughnutParams.StopAngle - doughnutParams.StartAngle) > Math.PI) ? true : false, SweepDirection.Clockwise, doughnutParams.AnimationEnabled ? start : end));
                    clipPathGeometry.Add(new LineSegmentParams(doughnutParams.AnimationEnabled ? innerstart : innerend));
                    // clipPathGeometry.Add(new ArcSegmentParams(new Size(pieParams.InnerRadius, pieParams.InnerRadius), 0, ((pieParams.StopAngle - pieParams.StartAngle) > Math.PI) ? true : false, SweepDirection.Counterclockwise, pieParams.AnimationEnabled ? innerstart : innerArcMidPoint));
                    // clipPathGeometry.Add(new ArcSegmentParams(new Size(pieParams.InnerRadius, pieParams.InnerRadius), 0, ((pieParams.StopAngle - pieParams.StartAngle) > Math.PI) ? true : false, SweepDirection.Counterclockwise, pieParams.AnimationEnabled ? innerstart : innerstart));

                    lightingEllipse.Clip = GetPathGeometryFromList(FillRule.Nonzero, innerstart, clipPathGeometry, true);

                    PathFigure figure = (lightingEllipse.Clip as PathGeometry).Figures[0];
                    PathSegmentCollection segments = figure.Segments;

                    // Apply animation to the doughnut slice
                    if (doughnutParams.AnimationEnabled)
                    {
                        // animate the outer points
                        doughnutParams.Storyboard = CreatePathSegmentAnimation(currentDataSeries, doughnutParams.DataPoint, doughnutParams.Storyboard, segments[0], center, doughnutParams.OuterRadius, currentDataSeries.InternalStartAngle, doughnutParams.StartAngle);
                        doughnutParams.Storyboard = CreatePathSegmentAnimation(currentDataSeries, doughnutParams.DataPoint, doughnutParams.Storyboard, segments[1], center, doughnutParams.OuterRadius, currentDataSeries.InternalStartAngle, doughnutParams.MeanAngle);
                        doughnutParams.Storyboard = CreatePathSegmentAnimation(currentDataSeries, doughnutParams.DataPoint, doughnutParams.Storyboard, segments[2], center, doughnutParams.OuterRadius, currentDataSeries.InternalStartAngle, doughnutParams.StopAngle);

                        // animate the inner points
                        doughnutParams.Storyboard = CreatePathSegmentAnimation(currentDataSeries, doughnutParams.DataPoint,doughnutParams.Storyboard, segments[3], center, doughnutParams.InnerRadius, currentDataSeries.InternalStartAngle, doughnutParams.StopAngle);
                        // pieParams.Storyboard = CreatePathSegmentAnimation(pieParams.Storyboard, segments[4], center, pieParams.InnerRadius, DataSeriesRef.InternalStartAngle, pieParams.MeanAngle);
                        // pieParams.Storyboard = CreatePathSegmentAnimation(pieParams.Storyboard, segments[5], center, pieParams.InnerRadius, DataSeriesRef.InternalStartAngle, pieParams.StartAngle);

                        doughnutParams.Storyboard = CreatePathFigureAnimation(currentDataSeries, doughnutParams.DataPoint, doughnutParams.Storyboard, figure, center, doughnutParams.InnerRadius, currentDataSeries.InternalStartAngle, doughnutParams.StartAngle);
                    }
                }

                PieVisual.Children.Add(lightingEllipse);
            }

            #endregion Lighting

            #region LabelLine

            if (doughnutParams.LabelLineEnabled)
            {
                Path labelLine = new Path() { Tag = new ElementData() { Element = doughnutParams.TagReference } };
                Double meanAngle = doughnutParams.MeanAngle;

                Point doughnutPoint = new Point();
                doughnutPoint.X = center.X + doughnutParams.OuterRadius * Math.Cos(meanAngle);
                doughnutPoint.Y = center.Y + doughnutParams.OuterRadius * Math.Sin(meanAngle);

                Point labelPoint = new Point();
                labelPoint.X = center.X + doughnutParams.LabelPoint.X - doughnutParams.Width / 2;
                labelPoint.Y = center.Y + doughnutParams.LabelPoint.Y - doughnutParams.Height / 2;

                Point midPoint = new Point();
                //midPoint.X = (labelPoint.X < center.X) ? labelPoint.X + 10 : labelPoint.X - 10;            
                if (doughnutParams.LabelLineTargetToRight)
                    midPoint.X = labelPoint.X + 10;
                else
                    midPoint.X = labelPoint.X - 10;


                midPoint.Y = labelPoint.Y;

                List<PathGeometryParams> labelLinePathGeometry = new List<PathGeometryParams>();
                labelLinePathGeometry.Add(new LineSegmentParams(doughnutParams.AnimationEnabled ? doughnutPoint : midPoint));
                labelLinePathGeometry.Add(new LineSegmentParams(doughnutParams.AnimationEnabled ? doughnutPoint : labelPoint));
                labelLine.Data = GetPathGeometryFromList(FillRule.Nonzero, doughnutPoint, labelLinePathGeometry, true);
                PathFigure figure = (labelLine.Data as PathGeometry).Figures[0];
                PathSegmentCollection segments = figure.Segments;
                figure.IsClosed = false;
                figure.IsFilled = false;

                // apply animation to the label line
                if (doughnutParams.AnimationEnabled)
                {
                    doughnutParams.Storyboard = CreateLabelLineAnimation(currentDataSeries,doughnutParams.DataPoint, doughnutParams.Storyboard, segments[0], doughnutPoint, midPoint);
                    doughnutParams.Storyboard = CreateLabelLineAnimation(currentDataSeries, doughnutParams.DataPoint, doughnutParams.Storyboard, segments[1], doughnutPoint, midPoint, labelPoint);
                }

                labelLine.Stroke = doughnutParams.LabelLineColor;
                labelLine.StrokeDashArray = doughnutParams.LabelLineStyle;
                labelLine.StrokeThickness = doughnutParams.LabelLineThickness;

                if ((doughnutParams.TagReference as DataPoint).InternalYValue == 0)
                {
                    Line zeroLine = new Line();
                    zeroLine.X1 = center.X + doughnutParams.InnerRadius * Math.Cos(doughnutParams.MeanAngle);
                    zeroLine.Y1 = center.Y + doughnutParams.InnerRadius * Math.Sin(doughnutParams.MeanAngle);

                    zeroLine.X2 = doughnutPoint.X;
                    zeroLine.Y2 = doughnutPoint.Y;
                    zeroLine.Stroke = doughnutParams.LabelLineColor;
                    zeroLine.StrokeThickness = 0.25;
                    zeroLine.IsHitTestVisible = false;
                    visual.Children.Add(zeroLine);

                    if (doughnutParams.AnimationEnabled)
                    {
                        doughnutParams.Storyboard = CreateOpacityAnimation(currentDataSeries, doughnutParams.DataPoint, doughnutParams.Storyboard, zeroLine, 2, zeroLine.Opacity, 0.5);
                        zeroLine.Opacity = 0;
                    }
                }

                visual.Children.Add(labelLine);

                labelLinePath = labelLine;

                // set the un exploded points for interactivity
                unExplodedPoints.LabelLineEndPoint = labelPoint;
                unExplodedPoints.LabelLineMidPoint = midPoint;
                unExplodedPoints.LabelLineStartPoint = doughnutPoint;

                // set the exploded points for interactivity
                explodedPoints.LabelLineEndPoint = new Point(labelPoint.X, labelPoint.Y - yOffset);
                explodedPoints.LabelLineMidPoint = new Point(midPoint.X, midPoint.Y - yOffset);
                explodedPoints.LabelLineStartPoint = new Point(doughnutPoint.X + xOffset, doughnutPoint.Y + yOffset);

                if ((doughnutParams.TagReference as DataPoint).InternalYValue == 0)
                    labelLine.IsHitTestVisible = false;
            }

            #endregion LabelLine

            #region Bevel

            if (doughnutParams.Bevel && Math.Abs(doughnutParams.StartAngle - doughnutParams.StopAngle) > 0.03 && (doughnutParams.TagReference as DataPoint).InternalYValue != 0)
            {
                Point start = new Point();
                Point end = new Point();
                Point innerstart = new Point();
                Point innerend = new Point();

                start.X = center.X + doughnutParams.OuterRadius * Math.Cos(doughnutParams.StartAngle);
                start.Y = center.Y + doughnutParams.OuterRadius * Math.Sin(doughnutParams.StartAngle);

                end.X = center.X + doughnutParams.OuterRadius * Math.Cos(doughnutParams.StopAngle);
                end.Y = center.Y + doughnutParams.OuterRadius * Math.Sin(doughnutParams.StopAngle);

                innerstart.X = center.X + doughnutParams.InnerRadius * Math.Cos(doughnutParams.StartAngle);
                innerstart.Y = center.Y + doughnutParams.InnerRadius * Math.Sin(doughnutParams.StartAngle);

                innerend.X = center.X + doughnutParams.InnerRadius * Math.Cos(doughnutParams.StopAngle);
                innerend.Y = center.Y + doughnutParams.InnerRadius * Math.Sin(doughnutParams.StopAngle);

                Point bevelCenter = new Point();
                Point bevelStart = new Point();
                Point bevelEnd = new Point();
                Point bevelInnerStart = new Point();
                Point bevelInnerEnd = new Point();
                Double bevelLength = 4;
                Double bevelOuterRadius = Math.Abs(doughnutParams.OuterRadius - bevelLength);
                Double bevelInnerRadius = Math.Abs(doughnutParams.InnerRadius + bevelLength);

                bevelCenter.X = center.X + bevelLength * Math.Cos(doughnutParams.MeanAngle);
                bevelCenter.Y = center.Y + bevelLength * Math.Sin(doughnutParams.MeanAngle);

                bevelStart.X = center.X + bevelOuterRadius * Math.Cos(doughnutParams.StartAngle + 0.03);
                bevelStart.Y = center.Y + bevelOuterRadius * Math.Sin(doughnutParams.StartAngle + 0.03);

                bevelEnd.X = center.X + bevelOuterRadius * Math.Cos(doughnutParams.StopAngle - 0.03);
                bevelEnd.Y = center.Y + bevelOuterRadius * Math.Sin(doughnutParams.StopAngle - 0.03);

                bevelInnerStart.X = center.X + bevelInnerRadius * Math.Cos(doughnutParams.StartAngle + 0.03);
                bevelInnerStart.Y = center.Y + bevelInnerRadius * Math.Sin(doughnutParams.StartAngle + 0.03);

                bevelInnerEnd.X = center.X + bevelInnerRadius * Math.Cos(doughnutParams.StopAngle - 0.03);
                bevelInnerEnd.Y = center.Y + bevelInnerRadius * Math.Sin(doughnutParams.StopAngle - 0.03);


                List<PathGeometryParams> pathGeometry = new List<PathGeometryParams>();
                pathGeometry.Add(new LineSegmentParams(innerstart));
                pathGeometry.Add(new LineSegmentParams(start));
                pathGeometry.Add(new LineSegmentParams(bevelStart));
                pathGeometry.Add(new LineSegmentParams(bevelInnerStart));

                Path path = new Path() { Tag = new ElementData() { Element = doughnutParams.TagReference } };
                path.Data = GetPathGeometryFromList(FillRule.Nonzero, bevelInnerStart, pathGeometry, true);
                if (doughnutParams.StartAngle > Math.PI * 0.5 && doughnutParams.StartAngle <= Math.PI * 1.5)
                {
                    path.Fill = GetDarkerBevelBrush(doughnutParams.Background, doughnutParams.StartAngle * 180 / Math.PI + 135);
                }
                else
                {
                    path.Fill = GetLighterBevelBrush(doughnutParams.Background, -doughnutParams.StartAngle * 180 / Math.PI);
                }

                // Apply animation to the beveling path
                if (doughnutParams.AnimationEnabled)
                {
                    doughnutParams.Storyboard = CreateOpacityAnimation(currentDataSeries, doughnutParams.DataPoint, doughnutParams.Storyboard, path, 1, 1, 1);
                    path.Opacity = 0;
                }

                faces.Parts.Add(path);
                visual.Children.Add(path);

                pathGeometry = new List<PathGeometryParams>();
                pathGeometry.Add(new LineSegmentParams(innerend));
                pathGeometry.Add(new LineSegmentParams(end));
                pathGeometry.Add(new LineSegmentParams(bevelEnd));
                pathGeometry.Add(new LineSegmentParams(bevelInnerEnd));

                path = new Path() { Tag = new ElementData() { Element = doughnutParams.TagReference } };
                path.Data = GetPathGeometryFromList(FillRule.Nonzero, bevelInnerEnd, pathGeometry, true);

                if (doughnutParams.StopAngle > Math.PI * 0.5 && doughnutParams.StopAngle <= Math.PI * 1.5)
                {
                    path.Fill = GetLighterBevelBrush(doughnutParams.Background, doughnutParams.StopAngle * 180 / Math.PI + 135);
                }
                else
                {
                    path.Fill = GetDarkerBevelBrush(doughnutParams.Background, -doughnutParams.StopAngle * 180 / Math.PI);
                }
                // Apply animation to the beveling path
                if (doughnutParams.AnimationEnabled)
                {
                    doughnutParams.Storyboard = CreateOpacityAnimation(currentDataSeries, doughnutParams.DataPoint, doughnutParams.Storyboard, path, 1, 1, 1);
                    path.Opacity = 0;
                }

                faces.Parts.Add(path);
                visual.Children.Add(path);

                #region Outer Beval"
                Shape outerBevel;

                if (enabledDataPoints.Count == 1 || noOfNonZeroDataPoint.Count() == 1)
                {
                    outerBevel = new Ellipse() { Tag = new ElementData() { Element = doughnutParams.TagReference }, Height = doughnutParams.OuterRadius * 2, Width = doughnutParams.OuterRadius * 2 };
                    GeometryGroup gg = new GeometryGroup();
                    gg.Children.Add(new EllipseGeometry() { Center = new Point(doughnutParams.OuterRadius, doughnutParams.OuterRadius), RadiusX = doughnutParams.OuterRadius, RadiusY = doughnutParams.OuterRadius });
                    gg.Children.Add(new EllipseGeometry() { Center = new Point(doughnutParams.OuterRadius, doughnutParams.OuterRadius), RadiusX = bevelOuterRadius, RadiusY = bevelOuterRadius });
                    outerBevel.Clip = gg;
                }
                else
                {
                    pathGeometry = new List<PathGeometryParams>();
                    pathGeometry.Add(new LineSegmentParams(end));
                    pathGeometry.Add(new ArcSegmentParams(new Size(doughnutParams.OuterRadius, doughnutParams.OuterRadius), 0, doughnutParams.IsLargerArc, SweepDirection.Counterclockwise, start));
                    pathGeometry.Add(new LineSegmentParams(bevelStart));
                    pathGeometry.Add(new ArcSegmentParams(new Size(bevelOuterRadius, bevelOuterRadius), 0, doughnutParams.IsLargerArc, SweepDirection.Clockwise, bevelEnd));

                    outerBevel = new Path() { Tag = new ElementData() { Element = doughnutParams.TagReference } };
                    (outerBevel as Path).Data = GetPathGeometryFromList(FillRule.Nonzero, bevelEnd, pathGeometry, true);
                }

                if (doughnutParams.MeanAngle > 0 && doughnutParams.MeanAngle < Math.PI)
                {
                    outerBevel.Fill = GetCurvedBevelBrush(doughnutParams.Background, doughnutParams.MeanAngle * 180 / Math.PI + 90, Graphics.GenerateDoubleCollection(-0.745, -0.85), Graphics.GenerateDoubleCollection(0, 1));
                }
                else
                {
                    outerBevel.Fill = GetCurvedBevelBrush(doughnutParams.Background, doughnutParams.MeanAngle * 180 / Math.PI + 90, Graphics.GenerateDoubleCollection(0.745, -0.99), Graphics.GenerateDoubleCollection(0, 1));
                }

                // Apply animation to the beveling path
                if (doughnutParams.AnimationEnabled)
                {
                    doughnutParams.Storyboard = CreateOpacityAnimation(currentDataSeries, doughnutParams.DataPoint, doughnutParams.Storyboard, outerBevel, 1, 1, 1);
                    outerBevel.Opacity = 0;
                }

                faces.Parts.Add(outerBevel);
                visual.Children.Add(outerBevel);

                #endregion Outer Beval"

                #region "Inner Bevel"

                Shape innerBevel;

                if (enabledDataPoints.Count == 1 || noOfNonZeroDataPoint.Count() == 1)
                {
                    innerBevel = new Ellipse() { Height = bevelInnerRadius * 2, Width = bevelInnerRadius * 2, Tag = new ElementData() { Element = doughnutParams.TagReference } };
                    innerBevel.SetValue(Canvas.LeftProperty, innerBevel.Width / 2 - 2 * bevelLength);
                    innerBevel.SetValue(Canvas.TopProperty, innerBevel.Height / 2 - 2 * bevelLength);

                    GeometryGroup gg = new GeometryGroup();
                    gg.Children.Add(new EllipseGeometry() { Center = new Point(innerBevel.Width / 2, innerBevel.Height / 2), RadiusX = bevelInnerRadius, RadiusY = bevelInnerRadius });
                    gg.Children.Add(new EllipseGeometry() { Center = new Point(innerBevel.Width / 2, innerBevel.Height / 2), RadiusX = bevelInnerRadius - bevelLength, RadiusY = bevelInnerRadius - bevelLength });

                    innerBevel.Clip = gg;

                    //innerBevel.SetValue(Canvas.ZIndexProperty, 1000);
                }
                else
                {
                    pathGeometry = new List<PathGeometryParams>();
                    pathGeometry.Add(new LineSegmentParams(innerend));
                    pathGeometry.Add(new ArcSegmentParams(new Size(doughnutParams.InnerRadius, doughnutParams.InnerRadius), 0, doughnutParams.IsLargerArc, SweepDirection.Counterclockwise, innerstart));
                    pathGeometry.Add(new LineSegmentParams(bevelInnerStart));
                    pathGeometry.Add(new ArcSegmentParams(new Size(bevelInnerRadius, bevelInnerRadius), 0, doughnutParams.IsLargerArc, SweepDirection.Clockwise, bevelInnerEnd));

                    innerBevel = new Path() { Tag = new ElementData() { Element = doughnutParams.TagReference } };
                    (innerBevel as Path).Data = GetPathGeometryFromList(FillRule.Nonzero, bevelInnerEnd, pathGeometry, true);
                }

                #endregion

                if (doughnutParams.MeanAngle > 0 && doughnutParams.MeanAngle < Math.PI)
                {
                    innerBevel.Fill = GetCurvedBevelBrush(doughnutParams.Background, doughnutParams.MeanAngle * 180 / Math.PI + 90, Graphics.GenerateDoubleCollection(0.745, -0.99), Graphics.GenerateDoubleCollection(0, 1));
                }
                else
                {
                    innerBevel.Fill = GetCurvedBevelBrush(doughnutParams.Background, doughnutParams.MeanAngle * 180 / Math.PI + 90, Graphics.GenerateDoubleCollection(-0.745, -0.85), Graphics.GenerateDoubleCollection(0, 1));
                }

                // Apply animation to the beveling path
                if (doughnutParams.AnimationEnabled)
                {
                    doughnutParams.Storyboard = CreateOpacityAnimation(currentDataSeries, doughnutParams.DataPoint, doughnutParams.Storyboard, innerBevel, 1, 1, 1);
                    innerBevel.Opacity = 0;
                }

                faces.Parts.Add(innerBevel);
                visual.Children.Add(innerBevel);
            }
            else
            {
                faces.Parts.Add(null);
                faces.Parts.Add(null);
                faces.Parts.Add(null);
                faces.Parts.Add(null);
            }


            #endregion Bevel

            return visual;
        }

        /// <summary>
        /// Create a LabelLine
        /// </summary>
        /// <param name="pieParams">Pie parameters</param>
        /// <param name="centerOfPie">Center point of pie</param>
        /// <param name="unExplodedPoints">UnExploded dataPoints</param>
        /// <param name="explodedPoints">Exploded dataPoints</param>
        /// <returns>Path</returns>
        private static Path CreateLabelLine(DataSeries currentDataSeries, SectorChartShapeParams pieParams, Point centerOfPie, ref PieDoughnut3DPoints unExplodedPoints, ref PieDoughnut3DPoints explodedPoints)
        {
            Path labelLine = null;

            if (pieParams.LabelLineEnabled)
            {
                labelLine = new Path() { Tag = new ElementData() { Element = pieParams.TagReference } };
                Double meanAngle = pieParams.MeanAngle;

                Point piePoint = new Point();
                piePoint.X = centerOfPie.X + pieParams.OuterRadius * Math.Cos(meanAngle);
                piePoint.Y = centerOfPie.Y + pieParams.OuterRadius * Math.Sin(meanAngle) * pieParams.YAxisScaling;

                Point labelPoint = new Point();
                labelPoint.X = centerOfPie.X + pieParams.LabelPoint.X - pieParams.Width / 2;
                labelPoint.Y = centerOfPie.Y + pieParams.LabelPoint.Y - pieParams.Height / 2;

                Point midPoint = new Point();
                //midPoint.X = (labelPoint.X < centerOfPie.X) ? labelPoint.X + 10 : labelPoint.X - 10;
                if (pieParams.LabelLineTargetToRight)
                    midPoint.X = labelPoint.X + 10;
                else
                    midPoint.X = labelPoint.X - 10;

                midPoint.Y = labelPoint.Y;

                List<PathGeometryParams> labelLinePathGeometry = new List<PathGeometryParams>();
                labelLinePathGeometry.Add(new LineSegmentParams(pieParams.AnimationEnabled ? piePoint : midPoint));
                labelLinePathGeometry.Add(new LineSegmentParams(pieParams.AnimationEnabled ? piePoint : labelPoint));
                labelLine.Data = GetPathGeometryFromList(FillRule.Nonzero, piePoint, labelLinePathGeometry, true);
                PathFigure figure = (labelLine.Data as PathGeometry).Figures[0];
                PathSegmentCollection segments = figure.Segments;
                figure.IsClosed = false;
                figure.IsFilled = false;

                // apply animation to the label line
                if (pieParams.AnimationEnabled)
                {
                    pieParams.Storyboard = CreateLabelLineAnimation(currentDataSeries, pieParams.DataPoint, pieParams.Storyboard, segments[0], piePoint, midPoint);
                    pieParams.Storyboard = CreateLabelLineAnimation(currentDataSeries, pieParams.DataPoint, pieParams.Storyboard, segments[1], piePoint, midPoint, labelPoint);
                }

                labelLine.Stroke = pieParams.LabelLineColor;
                labelLine.StrokeDashArray = pieParams.LabelLineStyle;
                labelLine.StrokeThickness = pieParams.LabelLineThickness;

                // set the un exploded points for interactivity
                unExplodedPoints.LabelLineEndPoint = labelPoint;
                unExplodedPoints.LabelLineMidPoint = midPoint;
                unExplodedPoints.LabelLineStartPoint = piePoint;

                // set the exploded points for interactivity
                explodedPoints.LabelLineEndPoint = new Point(labelPoint.X, labelPoint.Y - pieParams.OffsetY);
                explodedPoints.LabelLineMidPoint = new Point(midPoint.X, midPoint.Y - pieParams.OffsetY);
                explodedPoints.LabelLineStartPoint = new Point(piePoint.X + pieParams.OffsetX, piePoint.Y + pieParams.OffsetY);

                if ((pieParams.TagReference as DataPoint).InternalYValue == 0)
                    labelLine.IsHitTestVisible = false;

            }

            return labelLine;
        }

        /// <summary>
        /// Update label position inside Pie
        /// </summary>
        /// <param name="pieParams">Pie parameters</param>
        /// <param name="yOffset">Y offset</param>
        private static void UpdatePositionLabelInsidePie(SectorChartShapeParams pieParams, Double yOffset)
        {
            DataPoint currentDataPoint = pieParams.DataPoint;

            if (currentDataPoint.LabelStyle == LabelStyles.Inside)
            {
                Point center = new Point();
                center.X = pieParams.Width / 2;
                center.Y = pieParams.Height / 2;

                Double a = 3 * (pieParams.OuterRadius / 4);
                Double b = 3 * (pieParams.OuterRadius / 4) * pieParams.YAxisScaling;
                Double x = center.X + a * Math.Cos(pieParams.MeanAngle);
                Double y = center.Y + b * Math.Sin(pieParams.MeanAngle) + yOffset;

                currentDataPoint.LabelVisual.SetValue(Canvas.LeftProperty, x - currentDataPoint.LabelVisual.Width / 2);
                currentDataPoint.LabelVisual.SetValue(Canvas.TopProperty, y - currentDataPoint.LabelVisual.Height);
            }
        }

        /// <summary>
        /// Returns 3D Pie shapes
        /// </summary>
        /// <param name="faces">Pie faces</param>
        /// <param name="pieParams">Pie parameters</param>
        /// <param name="zindex">Zindex</param>
        /// <param name="unExplodedPoints">UnExploded dataPoints</param>
        /// <param name="explodedPoints">Exploded dataPoints</param>
        /// <param name="labelLinePath">Label line path</param>
        /// <param name="enabledDataPoints">List of enabled dataPoints</param>
        /// <returns>List of Shape</returns>
        private static List<Shape> GetPie3D(DataSeries currentDataSeries, ref Faces faces, SectorChartShapeParams pieParams, ref Int32 zindex, ref PieDoughnut3DPoints unExplodedPoints, ref PieDoughnut3DPoints explodedPoints, ref Path labelLinePath, List<DataPoint> enabledDataPoints)
        {
            List<Shape> pieFaces = new List<Shape>();

            Shape topFace = null, bottomFace = null, rightFace = null, leftFace = null;
            Point center = new Point();
            center.X = pieParams.Width / 2;
            center.Y = pieParams.Height / 2;

            // calculate 3d offsets
            Double yOffset = -pieParams.Depth / 2 * pieParams.ZAxisScaling;

            // calculate all points
            Point3D topFaceCenter = new Point3D();
            topFaceCenter.X = center.X;
            topFaceCenter.Y = center.Y + yOffset;
            topFaceCenter.Z = pieParams.OffsetY * Math.Sin(pieParams.StartAngle) * Math.Cos(pieParams.TiltAngle) + pieParams.Depth * Math.Cos(Math.PI / 2 - pieParams.TiltAngle);

            Point3D topArcStart = new Point3D();
            topArcStart.X = topFaceCenter.X + pieParams.OuterRadius * Math.Cos(pieParams.StartAngle);
            topArcStart.Y = topFaceCenter.Y + pieParams.OuterRadius * Math.Sin(pieParams.StartAngle) * pieParams.YAxisScaling;
            topArcStart.Z = (topFaceCenter.Y + pieParams.OuterRadius) * Math.Sin(pieParams.StartAngle) * Math.Cos(pieParams.TiltAngle) + pieParams.Depth * Math.Cos(Math.PI / 2 - pieParams.TiltAngle);

            Point3D topArcStop = new Point3D();
            topArcStop.X = topFaceCenter.X + pieParams.OuterRadius * Math.Cos(pieParams.StopAngle);
            topArcStop.Y = topFaceCenter.Y + pieParams.OuterRadius * Math.Sin(pieParams.StopAngle) * pieParams.YAxisScaling;
            topArcStop.Z = (topFaceCenter.Y + pieParams.OuterRadius) * Math.Sin(pieParams.StopAngle) * Math.Cos(pieParams.TiltAngle) + pieParams.Depth * Math.Cos(Math.PI / 2 - pieParams.TiltAngle);

            Point3D bottomFaceCenter = new Point3D();
            bottomFaceCenter.X = center.X;
            bottomFaceCenter.Y = center.Y - yOffset;
            bottomFaceCenter.Z = pieParams.OffsetY * Math.Sin(pieParams.StartAngle) * Math.Cos(pieParams.TiltAngle) - pieParams.Depth * Math.Cos(Math.PI / 2 - pieParams.TiltAngle);

            Point3D bottomArcStart = new Point3D();
            bottomArcStart.X = bottomFaceCenter.X + pieParams.OuterRadius * Math.Cos(pieParams.StartAngle);
            bottomArcStart.Y = bottomFaceCenter.Y + pieParams.OuterRadius * Math.Sin(pieParams.StartAngle) * pieParams.YAxisScaling;
            bottomArcStart.Z = (bottomFaceCenter.Y + pieParams.OuterRadius) * Math.Sin(pieParams.StartAngle) * Math.Cos(pieParams.TiltAngle) - pieParams.Depth * Math.Cos(Math.PI / 2 - pieParams.TiltAngle);

            Point3D bottomArcStop = new Point3D();
            bottomArcStop.X = bottomFaceCenter.X + pieParams.OuterRadius * Math.Cos(pieParams.StopAngle);
            bottomArcStop.Y = bottomFaceCenter.Y + pieParams.OuterRadius * Math.Sin(pieParams.StopAngle) * pieParams.YAxisScaling;
            bottomArcStop.Z = (bottomFaceCenter.Y + pieParams.OuterRadius) * Math.Sin(pieParams.StopAngle) * Math.Cos(pieParams.TiltAngle) - pieParams.Depth * Math.Cos(Math.PI / 2 - pieParams.TiltAngle);

            Point3D centroid = GetCentroid(topFaceCenter, topArcStart, topArcStop, bottomFaceCenter, bottomArcStart, bottomArcStop);

            UpdatePositionLabelInsidePie(pieParams, yOffset);

            if (pieParams.StartAngle == pieParams.StopAngle && pieParams.IsLargerArc || enabledDataPoints.Count == 1)
            {
                // draw singleton pie here
                topFace = new Ellipse() { Tag = new ElementData() { Element = pieParams.TagReference } };
                // topFace.Fill = pieParams.Lighting ? Graphics.GetLightingEnabledBrush(pieParams.Background, "Radial", new Double[] { 0.99, 0.745 }) : pieParams.Background;
                topFace.Fill = pieParams.Lighting ? Graphics.GetLightingEnabledBrush(pieParams.Background, "Linear", new Double[] { 0.99, 0.854 }) : pieParams.Background;

                topFace.Width = 2 * pieParams.OuterRadius;
                topFace.Height = 2 * pieParams.OuterRadius * pieParams.YAxisScaling;
                topFace.SetValue(Canvas.LeftProperty, (Double)(pieParams.Center.X - topFace.Width / 2));
                topFace.SetValue(Canvas.TopProperty, (Double)(pieParams.Center.Y - topFace.Height / 2 + yOffset));
                pieFaces.Add(topFace);
                faces.Parts.Add(topFace);

                bottomFace = new Ellipse() { Tag = new ElementData() { Element = pieParams.TagReference } };
                bottomFace.Fill = pieParams.Lighting ? Graphics.GetLightingEnabledBrush(pieParams.Background, "Radial", new Double[] { 0.99, 0.745 }) : pieParams.Background;

                bottomFace.Width = 2 * pieParams.OuterRadius;
                bottomFace.Height = 2 * pieParams.OuterRadius * pieParams.YAxisScaling;
                bottomFace.SetValue(Canvas.LeftProperty, (Double)(pieParams.Center.X - topFace.Width / 2));
                bottomFace.SetValue(Canvas.TopProperty, (Double)(pieParams.Center.Y - topFace.Height / 2 + yOffset));
                pieFaces.Add(bottomFace);
                faces.Parts.Add(bottomFace);
            }
            else
            {
                topFace = GetPieFace(pieParams, centroid, topFaceCenter, topArcStart, topArcStop);
                pieFaces.Add(topFace);
                faces.Parts.Add(topFace);

                bottomFace = GetPieFace(pieParams, centroid, bottomFaceCenter, bottomArcStart, bottomArcStop);
                pieFaces.Add(bottomFace);
                faces.Parts.Add(bottomFace);

                rightFace = GetPieSide(pieParams, centroid, topFaceCenter, bottomFaceCenter, topArcStart, bottomArcStart);
                pieFaces.Add(rightFace);
                faces.Parts.Add(rightFace);

                leftFace = GetPieSide(pieParams, centroid, topFaceCenter, bottomFaceCenter, topArcStop, bottomArcStop);
                pieFaces.Add(leftFace);
                faces.Parts.Add(leftFace);
            }

            labelLinePath = CreateLabelLine(currentDataSeries, pieParams, center, ref unExplodedPoints, ref explodedPoints);

            if ((pieParams.TagReference as DataPoint).InternalYValue == 0)
                return new List<Shape>();

            List<Shape> curvedSurface = GetPieOuterCurvedFace(pieParams, centroid, topFaceCenter, bottomFaceCenter);
            pieFaces.InsertRange(pieFaces.Count, curvedSurface.ToList());

            foreach (FrameworkElement fe in curvedSurface)
                faces.Parts.Add(fe);

            //Top face ZIndex
            topFace.SetValue(Canvas.ZIndexProperty, (Int32)(50000));

            //BottomFace ZIndex
            bottomFace.SetValue(Canvas.ZIndexProperty, (Int32)(-50000));

            if (pieParams.StartAngle == pieParams.StopAngle && pieParams.IsLargerArc)
            {

            }
            else
            {
                // ZIndex of curved face
                if (pieParams.StartAngle >= Math.PI && pieParams.StartAngle <= Math.PI * 2 && pieParams.StopAngle >= Math.PI && pieParams.StopAngle <= Math.PI * 2 && pieParams.IsLargerArc)
                {
                    _elementPositionData.Add(new ElementPositionData(rightFace, pieParams.StartAngle, pieParams.StartAngle));
                    _elementPositionData.Add(new ElementPositionData(curvedSurface[0], 0, Math.PI));
                    _elementPositionData.Add(new ElementPositionData(leftFace, pieParams.StopAngle, pieParams.StopAngle));
                    if (labelLinePath != null)
                        _elementPositionData.Add(new ElementPositionData(labelLinePath, 0, Math.PI));
                }
                else if (pieParams.StartAngle >= 0 && pieParams.StartAngle <= Math.PI && pieParams.StopAngle >= 0 && pieParams.StopAngle <= Math.PI && pieParams.IsLargerArc)
                {
                    _elementPositionData.Add(new ElementPositionData(rightFace, pieParams.StartAngle, pieParams.StartAngle));
                    _elementPositionData.Add(new ElementPositionData(curvedSurface[0], pieParams.StartAngle, Math.PI));
                    _elementPositionData.Add(new ElementPositionData(curvedSurface[1], 0, pieParams.StopAngle));
                    _elementPositionData.Add(new ElementPositionData(leftFace, pieParams.StopAngle, pieParams.StopAngle));
                    if (labelLinePath != null)
                        labelLinePath.SetValue(Canvas.ZIndexProperty, -50000);
                }
                else if (pieParams.StartAngle >= 0 && pieParams.StartAngle <= Math.PI && pieParams.StopAngle >= Math.PI && pieParams.StopAngle <= Math.PI * 2)
                {
                    _elementPositionData.Add(new ElementPositionData(rightFace, pieParams.StartAngle, pieParams.StartAngle));
                    
                    //  if (labelLinePath != null)
                    //      _elementPositionData.Add(new ElementPositionData(labelLinePath, pieParams.StartAngle, Math.PI));

                    //------------
                    if (labelLinePath != null)
                    {
                        if (pieParams.StartAngle >= Math.PI / 2 && pieParams.StopAngle <= Math.PI)
                            labelLinePath.SetValue(Canvas.ZIndexProperty, 50000);
                        else
                            _elementPositionData.Add(new ElementPositionData(labelLinePath, pieParams.StartAngle, Math.PI));
                    }
                    //------------

                    _elementPositionData.Add(new ElementPositionData(leftFace, pieParams.StopAngle, pieParams.StopAngle));
                    _elementPositionData.Add(new ElementPositionData(curvedSurface[0], pieParams.StartAngle, Math.PI));
                }
                else if (pieParams.StartAngle >= Math.PI && pieParams.StartAngle <= Math.PI * 2 && pieParams.StopAngle >= 0 && pieParams.StopAngle <= Math.PI)
                {
                    _elementPositionData.Add(new ElementPositionData(rightFace, pieParams.StartAngle, pieParams.StartAngle));
                    _elementPositionData.Add(new ElementPositionData(curvedSurface[0], 0, pieParams.StopAngle));
                    _elementPositionData.Add(new ElementPositionData(leftFace, pieParams.StopAngle, pieParams.StopAngle));
                    if (labelLinePath != null)
                        _elementPositionData.Add(new ElementPositionData(labelLinePath, pieParams.StopAngle, pieParams.StopAngle));
                }
                else
                {
                    _elementPositionData.Add(new ElementPositionData(rightFace, pieParams.StartAngle, pieParams.StartAngle));
                    if (pieParams.StartAngle >= 0 && pieParams.StartAngle < Math.PI / 2 && pieParams.StopAngle >= 0 && pieParams.StopAngle < Math.PI / 2)
                    {
                        if (labelLinePath != null)
                            _elementPositionData.Add(new ElementPositionData(labelLinePath, pieParams.StopAngle, pieParams.StopAngle));
                        _elementPositionData.Add(new ElementPositionData(curvedSurface[0], pieParams.StartAngle, pieParams.StopAngle));
                    }
                    else if (pieParams.StartAngle >= 0 && pieParams.StartAngle < Math.PI / 2 && pieParams.StopAngle >= Math.PI / 2 && pieParams.StopAngle < Math.PI)
                    {
                        if (labelLinePath != null)
                            labelLinePath.SetValue(Canvas.ZIndexProperty, 40000);
                        curvedSurface[0].SetValue(Canvas.ZIndexProperty, 35000);
                    }
                    else if (pieParams.StartAngle >= Math.PI / 2 && pieParams.StartAngle < Math.PI && pieParams.StopAngle >= Math.PI / 2 && pieParams.StopAngle < Math.PI)
                    {
                        if (labelLinePath != null)
                            _elementPositionData.Add(new ElementPositionData(labelLinePath, pieParams.StartAngle, pieParams.StartAngle));
                        _elementPositionData.Add(new ElementPositionData(curvedSurface[0], pieParams.StartAngle, pieParams.StopAngle));
                    }
                    else if (pieParams.StartAngle >= Math.PI && pieParams.StartAngle < Math.PI * 1.5 && pieParams.StopAngle >= Math.PI && pieParams.StopAngle < Math.PI * 1.5)
                    {
                        _elementPositionData.Add(new ElementPositionData(curvedSurface[0], pieParams.StartAngle, pieParams.StopAngle));
                        if (labelLinePath != null)
                            _elementPositionData.Add(new ElementPositionData(labelLinePath, pieParams.StartAngle, pieParams.StartAngle));
                    }
                    else if (pieParams.StartAngle >= Math.PI * 1.5 && pieParams.StartAngle < Math.PI * 2 && pieParams.StopAngle >= Math.PI * 1.5 && pieParams.StopAngle < Math.PI * 2)
                    {
                        _elementPositionData.Add(new ElementPositionData(curvedSurface[0], pieParams.StartAngle, pieParams.StopAngle));
                        if (labelLinePath != null)
                            _elementPositionData.Add(new ElementPositionData(labelLinePath, pieParams.StartAngle, pieParams.StopAngle));
                    }
                    else
                    {
                        if (labelLinePath != null)
                            _elementPositionData.Add(new ElementPositionData(labelLinePath, pieParams.StartAngle, pieParams.StartAngle));
                        _elementPositionData.Add(new ElementPositionData(curvedSurface[0], pieParams.StartAngle, pieParams.StopAngle));
                    }

                    _elementPositionData.Add(new ElementPositionData(leftFace, pieParams.StopAngle, pieParams.StopAngle));
                }
            }

            return pieFaces;

        }

        internal static Brush Get3DFaceColor(RenderAs renderAs, Boolean lightingEnabled, Brush color, PieFaceTypes faceType, Double startAngle, Double stopAngle, Double tiltAngle)
        {
            Brush retVal = null;

            switch (faceType)
            {
                case PieFaceTypes.Top:
                case PieFaceTypes.Bottom:
                    if(renderAs == RenderAs.Pie)
                        retVal = lightingEnabled ? Graphics.GetLightingEnabledBrush(color, "Linear", new Double[] { 0.99, 0.789 }) : color;
                    else
                        retVal = lightingEnabled ? Graphics.GetLightingEnabledBrush(color, -75, "Linear", new Double[] { 0.99, 0.854 }) : color;
                    break;

                case PieFaceTypes.Left:
                case PieFaceTypes.Right:
                    retVal = lightingEnabled ? Graphics.GetLightingEnabledBrush(color, "Linear", new Double[] { 0.845, 0.79 }) : color;
                    break;
                case PieFaceTypes.CurvedSurface:

                    if (renderAs == RenderAs.Pie)
                    {
                        retVal = lightingEnabled ? Graphics.GetLightingEnabledBrush(color, startAngle * 180 / Math.PI, "Linear", new Double[] { 0.99, 0.789 }) : color;
                    }
                    else
                    {
                        if (stopAngle > Math.PI)
                            retVal = lightingEnabled ? Graphics.GetLightingEnabledBrush(color, tiltAngle * 180 / Math.PI, "Linear", new Double[] { 0.99, 0.845 }) : color;
                        else
                            retVal = lightingEnabled ? Graphics.GetLightingEnabledBrush(color, startAngle * 180 / Math.PI, "Linear", new Double[] { 0.99, 0.845 }) : color;
                    }
                    break;
            }

            return retVal;
        }

        /// <summary>
        /// Returns 3D Doughnut shapes
        /// </summary>
        /// <param name="faces">Doughnut faces</param>
        /// <param name="doughnutParams">Doughnut parameters</param>
        /// <param name="unExplodedPoints">UnExploded dataPoints</param>
        /// <param name="explodedPoints">Exploded dataPoints</param>
        /// <param name="labelLinePath">Label line path</param>
        /// <param name="enabledDataPoints">List of enabled dataPoints</param>
        /// <returns>List of Shape</returns>
        private static List<Shape> GetDoughnut3D(DataSeries currentDataSeries, ref Faces faces, SectorChartShapeParams doughnutParams, ref PieDoughnut3DPoints unExplodedPoints, ref PieDoughnut3DPoints explodedPoints, ref Path labelLinePath, List<DataPoint> enabledDataPoints)
        {
            List<Shape> pieFaces = new List<Shape>();
            Shape topFace = null, bottomFace = null, rightFace = null, leftFace = null;

            // calculate 3d offsets
            Double yOffset = -doughnutParams.Depth / 2 * doughnutParams.ZAxisScaling;
            Point center = new Point();
            center.X += doughnutParams.Width / 2;
            center.Y += doughnutParams.Height / 2;

            // calculate all points
            Point3D topFaceCenter = new Point3D();
            topFaceCenter.X = center.X;
            topFaceCenter.Y = center.Y + yOffset;
            topFaceCenter.Z = doughnutParams.OffsetY * Math.Sin(doughnutParams.StartAngle) * Math.Cos(doughnutParams.TiltAngle) + doughnutParams.Depth * Math.Cos(Math.PI / 2 - doughnutParams.TiltAngle);

            Point3D topOuterArcStart = new Point3D();
            topOuterArcStart.X = topFaceCenter.X + doughnutParams.OuterRadius * Math.Cos(doughnutParams.StartAngle);
            topOuterArcStart.Y = topFaceCenter.Y + doughnutParams.OuterRadius * Math.Sin(doughnutParams.StartAngle) * doughnutParams.YAxisScaling;
            topOuterArcStart.Z = (topFaceCenter.Y + doughnutParams.OuterRadius) * Math.Sin(doughnutParams.StartAngle) * Math.Cos(doughnutParams.TiltAngle) + doughnutParams.Depth * Math.Cos(Math.PI / 2 - doughnutParams.TiltAngle);

            Point3D topOuterArcStop = new Point3D();
            topOuterArcStop.X = topFaceCenter.X + doughnutParams.OuterRadius * Math.Cos(doughnutParams.StopAngle);
            topOuterArcStop.Y = topFaceCenter.Y + doughnutParams.OuterRadius * Math.Sin(doughnutParams.StopAngle) * doughnutParams.YAxisScaling;
            topOuterArcStop.Z = (topFaceCenter.Y + doughnutParams.OuterRadius) * Math.Sin(doughnutParams.StopAngle) * Math.Cos(doughnutParams.TiltAngle) + doughnutParams.Depth * Math.Cos(Math.PI / 2 - doughnutParams.TiltAngle);

            Point3D topInnerArcStart = new Point3D();
            topInnerArcStart.X = topFaceCenter.X + doughnutParams.InnerRadius * Math.Cos(doughnutParams.StartAngle);
            topInnerArcStart.Y = topFaceCenter.Y + doughnutParams.InnerRadius * Math.Sin(doughnutParams.StartAngle) * doughnutParams.YAxisScaling;
            topInnerArcStart.Z = (topFaceCenter.Y + doughnutParams.InnerRadius) * Math.Sin(doughnutParams.StartAngle) * Math.Cos(doughnutParams.TiltAngle) + doughnutParams.Depth * Math.Cos(Math.PI / 2 - doughnutParams.TiltAngle);

            Point3D topInnerArcStop = new Point3D();
            topInnerArcStop.X = topFaceCenter.X + doughnutParams.InnerRadius * Math.Cos(doughnutParams.StopAngle);
            topInnerArcStop.Y = topFaceCenter.Y + doughnutParams.InnerRadius * Math.Sin(doughnutParams.StopAngle) * doughnutParams.YAxisScaling;
            topInnerArcStop.Z = (topFaceCenter.Y + doughnutParams.InnerRadius) * Math.Sin(doughnutParams.StopAngle) * Math.Cos(doughnutParams.TiltAngle) + doughnutParams.Depth * Math.Cos(Math.PI / 2 - doughnutParams.TiltAngle);

            Point3D bottomFaceCenter = new Point3D();
            bottomFaceCenter.X = center.X;
            bottomFaceCenter.Y = center.Y - yOffset;
            bottomFaceCenter.Z = doughnutParams.OffsetY * Math.Sin(doughnutParams.StartAngle) * Math.Cos(doughnutParams.TiltAngle) - doughnutParams.Depth * Math.Cos(Math.PI / 2 - doughnutParams.TiltAngle);

            Point3D bottomOuterArcStart = new Point3D();
            bottomOuterArcStart.X = bottomFaceCenter.X + doughnutParams.OuterRadius * Math.Cos(doughnutParams.StartAngle);
            bottomOuterArcStart.Y = bottomFaceCenter.Y + doughnutParams.OuterRadius * Math.Sin(doughnutParams.StartAngle) * doughnutParams.YAxisScaling;
            bottomOuterArcStart.Z = (bottomFaceCenter.Y + doughnutParams.OuterRadius) * Math.Sin(doughnutParams.StartAngle) * Math.Cos(doughnutParams.TiltAngle) - doughnutParams.Depth * Math.Cos(Math.PI / 2 - doughnutParams.TiltAngle);

            Point3D bottomOuterArcStop = new Point3D();
            bottomOuterArcStop.X = bottomFaceCenter.X + doughnutParams.OuterRadius * Math.Cos(doughnutParams.StopAngle);
            bottomOuterArcStop.Y = bottomFaceCenter.Y + doughnutParams.OuterRadius * Math.Sin(doughnutParams.StopAngle) * doughnutParams.YAxisScaling;
            bottomOuterArcStop.Z = (bottomFaceCenter.Y + doughnutParams.OuterRadius) * Math.Sin(doughnutParams.StopAngle) * Math.Cos(doughnutParams.TiltAngle) - doughnutParams.Depth * Math.Cos(Math.PI / 2 - doughnutParams.TiltAngle);

            Point3D bottomInnerArcStart = new Point3D();
            bottomInnerArcStart.X = bottomFaceCenter.X + doughnutParams.InnerRadius * Math.Cos(doughnutParams.StartAngle);
            bottomInnerArcStart.Y = bottomFaceCenter.Y + doughnutParams.InnerRadius * Math.Sin(doughnutParams.StartAngle) * doughnutParams.YAxisScaling;
            bottomInnerArcStart.Z = (bottomFaceCenter.Y + doughnutParams.InnerRadius) * Math.Sin(doughnutParams.StartAngle) * Math.Cos(doughnutParams.TiltAngle) - doughnutParams.Depth * Math.Cos(Math.PI / 2 - doughnutParams.TiltAngle);

            Point3D bottomInnerArcStop = new Point3D();
            bottomInnerArcStop.X = bottomFaceCenter.X + doughnutParams.InnerRadius * Math.Cos(doughnutParams.StopAngle);
            bottomInnerArcStop.Y = bottomFaceCenter.Y + doughnutParams.InnerRadius * Math.Sin(doughnutParams.StopAngle) * doughnutParams.YAxisScaling;
            bottomInnerArcStop.Z = (bottomFaceCenter.Y + doughnutParams.InnerRadius) * Math.Sin(doughnutParams.StopAngle) * Math.Cos(doughnutParams.TiltAngle) - doughnutParams.Depth * Math.Cos(Math.PI / 2 - doughnutParams.TiltAngle);

            Point3D centroid = GetCentroid(topInnerArcStart, topInnerArcStop, topOuterArcStart, topOuterArcStop, bottomInnerArcStart, bottomInnerArcStop, bottomOuterArcStart, bottomOuterArcStop);

            UpdatePositionLabelInsidePie(doughnutParams, yOffset);

            if (doughnutParams.StartAngle == doughnutParams.StopAngle && doughnutParams.IsLargerArc || enabledDataPoints.Count == 1)
            {
                // draw singleton pie here
                topFace = new Ellipse() { Tag = new ElementData() { Element = doughnutParams.TagReference } };
                // topFace.Fill = doughnutParams.Lighting ? Graphics.GetLightingEnabledBrush(doughnutParams.Background, "Radial", new Double[] { 0.99, 0.745 }) : doughnutParams.Background;
                topFace.Fill = doughnutParams.Lighting ? Graphics.GetLightingEnabledBrush(doughnutParams.Background, "Linear", new Double[] { 0.99, 0.854 }) : doughnutParams.Background;
                
                topFace.Width = 2 * doughnutParams.OuterRadius;
                topFace.Height = 2 * doughnutParams.OuterRadius * doughnutParams.YAxisScaling;
                topFace.SetValue(Canvas.LeftProperty, (Double)(doughnutParams.Center.X - topFace.Width / 2));
                topFace.SetValue(Canvas.TopProperty, (Double)(doughnutParams.Center.Y - topFace.Height / 2 + yOffset));

                GeometryGroup gg = new GeometryGroup();
                gg.Children.Add(new EllipseGeometry() { Center = new Point(topFace.Width / 2, topFace.Height / 2), RadiusX = topFace.Width, RadiusY = topFace.Height });
                gg.Children.Add(new EllipseGeometry() { Center = new Point(topFace.Width / 2, topFace.Height / 2), RadiusX = doughnutParams.InnerRadius, RadiusY = topFace.Height - 2 * (doughnutParams.OuterRadius - doughnutParams.InnerRadius) });

                topFace.Clip = gg;
                pieFaces.Add(topFace);
                faces.Parts.Add(topFace);

                bottomFace = new Ellipse() { Tag = new ElementData() { Element = doughnutParams.TagReference } };
                bottomFace.Fill = doughnutParams.Lighting ? Graphics.GetLightingEnabledBrush(doughnutParams.Background, "Radial", new Double[] { 0.99, 0.745 }) : doughnutParams.Background;
                // topFace.Stroke = pieParams.Lighting ? Graphics.GetLightingEnabledBrush(pieParams.Background, "Radial", new Double[] { 0.99, 0.745 }) : pieParams.Background;
                bottomFace.Width = 2 * doughnutParams.OuterRadius;
                bottomFace.Height = 2 * doughnutParams.OuterRadius * doughnutParams.YAxisScaling;
                bottomFace.SetValue(Canvas.LeftProperty, (Double)(doughnutParams.Center.X - topFace.Width / 2));
                bottomFace.SetValue(Canvas.TopProperty, (Double)(doughnutParams.Center.Y - topFace.Height / 2));

                gg = new GeometryGroup();
                gg.Children.Add(new EllipseGeometry() { Center = new Point(topFace.Width / 2, topFace.Height / 2), RadiusX = topFace.Width, RadiusY = topFace.Height });
                gg.Children.Add(new EllipseGeometry() { Center = new Point(topFace.Width / 2, topFace.Height / 2), RadiusX = doughnutParams.InnerRadius, RadiusY = topFace.Height - 2 * (doughnutParams.OuterRadius - doughnutParams.InnerRadius) });

                bottomFace.Clip = gg;
                pieFaces.Add(bottomFace);
                faces.Parts.Add(bottomFace);
            }
            else
            {
                topFace = GetDoughnutFace(doughnutParams, centroid, topInnerArcStart, topInnerArcStop, topOuterArcStart, topOuterArcStop, true);
                pieFaces.Add(topFace);
                faces.Parts.Add(topFace);

                bottomFace = GetDoughnutFace(doughnutParams, centroid, bottomInnerArcStart, bottomInnerArcStop, bottomOuterArcStart, bottomOuterArcStop, false);
                pieFaces.Add(bottomFace);
                faces.Parts.Add(bottomFace);

                rightFace = GetPieSide(doughnutParams, centroid, topInnerArcStart, bottomInnerArcStart, topOuterArcStart, bottomOuterArcStart);
                pieFaces.Add(rightFace);
                faces.Parts.Add(rightFace);

                leftFace = GetPieSide(doughnutParams, centroid, topInnerArcStop, bottomInnerArcStop, topOuterArcStop, bottomOuterArcStop);
                pieFaces.Add(leftFace);
                faces.Parts.Add(leftFace);
            }

            List<Shape> curvedSurface = GetDoughnutCurvedFace(doughnutParams, centroid, topFaceCenter, bottomFaceCenter);
            pieFaces.InsertRange(pieFaces.Count, curvedSurface);

            foreach (FrameworkElement fe in curvedSurface)
                faces.Parts.Add(fe);

            labelLinePath = CreateLabelLine(currentDataSeries, doughnutParams, center, ref unExplodedPoints, ref explodedPoints);

            if ((doughnutParams.TagReference as DataPoint).InternalYValue == 0)
                return new List<Shape>();

            //Top face ZIndex
            topFace.SetValue(Canvas.ZIndexProperty, (Int32)(50000));
            //BottomFace ZIndex
            bottomFace.SetValue(Canvas.ZIndexProperty, (Int32)(-50000));

            if (!(doughnutParams.StartAngle == doughnutParams.StopAngle && doughnutParams.IsLargerArc))
            {
                // ZIndex of curved face
                if (doughnutParams.StartAngle >= Math.PI && doughnutParams.StartAngle <= Math.PI * 2 && doughnutParams.StopAngle >= Math.PI && doughnutParams.StopAngle <= Math.PI * 2 && doughnutParams.IsLargerArc)
                {
                    _elementPositionData.Add(new ElementPositionData(rightFace, doughnutParams.StartAngle, doughnutParams.StartAngle));
                    _elementPositionData.Add(new ElementPositionData(curvedSurface[0], 0, Math.PI));
                    _elementPositionData.Add(new ElementPositionData(curvedSurface[1], doughnutParams.StartAngle, 0));
                    _elementPositionData.Add(new ElementPositionData(curvedSurface[2], Math.PI, doughnutParams.StopAngle));
                    _elementPositionData.Add(new ElementPositionData(leftFace, doughnutParams.StopAngle, doughnutParams.StopAngle));
                    if (labelLinePath != null)
                        _elementPositionData.Add(new ElementPositionData(labelLinePath, 0, Math.PI));
                }
                else if (doughnutParams.StartAngle >= 0 && doughnutParams.StartAngle <= Math.PI && doughnutParams.StopAngle >= 0 && doughnutParams.StopAngle <= Math.PI && doughnutParams.IsLargerArc)
                {
                    _elementPositionData.Add(new ElementPositionData(rightFace, doughnutParams.StartAngle, doughnutParams.StartAngle));
                    _elementPositionData.Add(new ElementPositionData(curvedSurface[0], doughnutParams.StartAngle, Math.PI));
                    _elementPositionData.Add(new ElementPositionData(curvedSurface[1], Math.PI * 2, doughnutParams.StopAngle));
                    _elementPositionData.Add(new ElementPositionData(curvedSurface[2], Math.PI, Math.PI * 2));
                    _elementPositionData.Add(new ElementPositionData(leftFace, doughnutParams.StopAngle, doughnutParams.StopAngle));
                    if (labelLinePath != null)
                        labelLinePath.SetValue(Canvas.ZIndexProperty, -50000);
                }
                else if (doughnutParams.StartAngle >= 0 && doughnutParams.StartAngle <= Math.PI && doughnutParams.StopAngle >= Math.PI && doughnutParams.StopAngle <= Math.PI * 2)
                {
                    _elementPositionData.Add(new ElementPositionData(rightFace, doughnutParams.StartAngle, doughnutParams.StartAngle));
                    if (labelLinePath != null)
                        _elementPositionData.Add(new ElementPositionData(labelLinePath, doughnutParams.StartAngle, Math.PI));
                    _elementPositionData.Add(new ElementPositionData(curvedSurface[0], doughnutParams.StartAngle, Math.PI));
                    _elementPositionData.Add(new ElementPositionData(curvedSurface[1], Math.PI, doughnutParams.StopAngle));
                    _elementPositionData.Add(new ElementPositionData(leftFace, doughnutParams.StopAngle, doughnutParams.StopAngle));
                }
                else if (doughnutParams.StartAngle >= Math.PI && doughnutParams.StartAngle <= Math.PI * 2 && doughnutParams.StopAngle >= 0 && doughnutParams.StopAngle <= Math.PI)
                {
                    _elementPositionData.Add(new ElementPositionData(rightFace, doughnutParams.StartAngle, doughnutParams.StartAngle));
                    _elementPositionData.Add(new ElementPositionData(curvedSurface[0], 0, doughnutParams.StopAngle));
                    _elementPositionData.Add(new ElementPositionData(curvedSurface[1], doughnutParams.StartAngle, 0));
                    _elementPositionData.Add(new ElementPositionData(leftFace, doughnutParams.StopAngle, doughnutParams.StopAngle));
                    if (labelLinePath != null)
                        _elementPositionData.Add(new ElementPositionData(labelLinePath, doughnutParams.StopAngle, doughnutParams.StopAngle));
                }
                else
                {
                    _elementPositionData.Add(new ElementPositionData(rightFace, doughnutParams.StartAngle, doughnutParams.StartAngle));
                    if (doughnutParams.StartAngle >= 0 && doughnutParams.StartAngle < Math.PI / 2 && doughnutParams.StopAngle >= 0 && doughnutParams.StopAngle < Math.PI / 2)
                    {
                        if (labelLinePath != null)
                            _elementPositionData.Add(new ElementPositionData(labelLinePath, doughnutParams.StopAngle, doughnutParams.StopAngle));
                        _elementPositionData.Add(new ElementPositionData(curvedSurface[0], doughnutParams.StartAngle, doughnutParams.StopAngle));
                    }
                    else if (doughnutParams.StartAngle >= 0 && doughnutParams.StartAngle < Math.PI / 2 && doughnutParams.StopAngle >= Math.PI / 2 && doughnutParams.StopAngle < Math.PI)
                    {
                        if (labelLinePath != null)
                            labelLinePath.SetValue(Canvas.ZIndexProperty, 40000);
                        _elementPositionData.Add(new ElementPositionData(curvedSurface[0], doughnutParams.StartAngle, doughnutParams.StopAngle));
                    }
                    else if (doughnutParams.StartAngle >= Math.PI / 2 && doughnutParams.StartAngle < Math.PI && doughnutParams.StopAngle >= Math.PI / 2 && doughnutParams.StopAngle < Math.PI)
                    {
                        if (labelLinePath != null)
                            _elementPositionData.Add(new ElementPositionData(labelLinePath, doughnutParams.StartAngle, doughnutParams.StartAngle));
                        _elementPositionData.Add(new ElementPositionData(curvedSurface[0], doughnutParams.StartAngle, doughnutParams.StopAngle));
                    }
                    else if (doughnutParams.StartAngle >= Math.PI && doughnutParams.StartAngle < Math.PI * 1.5 && doughnutParams.StopAngle >= Math.PI && doughnutParams.StopAngle < Math.PI * 1.5)
                    {
                        _elementPositionData.Add(new ElementPositionData(curvedSurface[0], doughnutParams.StartAngle, doughnutParams.StopAngle));
                        if (labelLinePath != null)
                            _elementPositionData.Add(new ElementPositionData(labelLinePath, doughnutParams.StartAngle, doughnutParams.StartAngle));
                    }
                    else if (doughnutParams.StartAngle >= Math.PI * 1.5 && doughnutParams.StartAngle < Math.PI * 2 && doughnutParams.StopAngle >= Math.PI * 1.5 && doughnutParams.StopAngle < Math.PI * 2)
                    {
                        _elementPositionData.Add(new ElementPositionData(curvedSurface[0], doughnutParams.StartAngle, doughnutParams.StopAngle));
                        if (labelLinePath != null)
                            _elementPositionData.Add(new ElementPositionData(labelLinePath, doughnutParams.StartAngle, doughnutParams.StopAngle));
                    }
                    else
                    {
                        if (labelLinePath != null)
                            _elementPositionData.Add(new ElementPositionData(labelLinePath, doughnutParams.StartAngle, doughnutParams.StartAngle));
                        _elementPositionData.Add(new ElementPositionData(curvedSurface[0], doughnutParams.StartAngle, doughnutParams.StopAngle));
                    }
                    _elementPositionData.Add(new ElementPositionData(leftFace, doughnutParams.StopAngle, doughnutParams.StopAngle));
                }
            }

            return pieFaces;
        }

        /// <summary>
        /// Set ZIndex for 3D Pie/Doughnut
        /// </summary>
        /// <param name="element">FrameworkElement</param>
        /// <param name="zindex1">ZIndex1</param>
        /// <param name="zindex2">ZIndex2</param>
        /// <param name="angle">Angle</param>
        private static void SetZIndex(FrameworkElement element, ref Int32 zindex1, ref Int32 zindex2, Double angle)
        {
            if (element == null) return;

            if (angle >= 0 && angle <= Math.PI / 2)
            {
                element.SetValue(Canvas.ZIndexProperty, ++zindex1);
            }
            else if (angle > Math.PI / 2 && angle <= Math.PI)
            {
                element.SetValue(Canvas.ZIndexProperty, --zindex1);
            }
            else if (angle > Math.PI && angle <= Math.PI * 3 / 2)
            {
                element.SetValue(Canvas.ZIndexProperty, --zindex2);
            }
            else
            {
                element.SetValue(Canvas.ZIndexProperty, ++zindex2);
            }
        }

        /// <summary>
        /// Returns pie outer curved shapes
        /// </summary>
        /// <param name="pieParams">Pie parameters</param>
        /// <param name="centroid">Centroid</param>
        /// <param name="topFaceCenter">Top face center</param>
        /// <param name="bottomFaceCenter">Bottom face center</param>
        /// <returns>List of Shape</returns>
        private static List<Shape> GetPieOuterCurvedFace(SectorChartShapeParams pieParams, Point3D centroid, Point3D topFaceCenter, Point3D bottomFaceCenter)
        {
            List<Shape> curvedFaces = new List<Shape>();

            if (pieParams.StartAngle >= Math.PI && pieParams.StartAngle <= Math.PI * 2 && pieParams.StopAngle >= Math.PI && pieParams.StopAngle <= Math.PI * 2 && pieParams.IsLargerArc)
            {
                Path curvedSegment = GetCurvedSegment(pieParams, pieParams.OuterRadius, 0, Math.PI, topFaceCenter, bottomFaceCenter, centroid, true, RenderAs.Pie);
                curvedFaces.Add(curvedSegment);
            }
            else if (pieParams.StartAngle >= 0 && pieParams.StartAngle <= Math.PI && pieParams.StopAngle >= 0 && pieParams.StopAngle <= Math.PI && pieParams.IsLargerArc)
            {
                Path curvedSegment = GetCurvedSegment(pieParams, pieParams.OuterRadius, pieParams.StartAngle, Math.PI, topFaceCenter, bottomFaceCenter, centroid, true, RenderAs.Pie);
                curvedFaces.Add(curvedSegment);
                curvedSegment = GetCurvedSegment(pieParams, pieParams.OuterRadius, Math.PI * 2, pieParams.StopAngle, topFaceCenter, bottomFaceCenter, centroid, true, RenderAs.Pie);
                curvedFaces.Add(curvedSegment);
            }
            else if (pieParams.StartAngle >= 0 && pieParams.StartAngle <= Math.PI && pieParams.StopAngle >= Math.PI && pieParams.StopAngle <= Math.PI * 2)
            {
                Path curvedSegment = GetCurvedSegment(pieParams, pieParams.OuterRadius, pieParams.StartAngle, Math.PI, topFaceCenter, bottomFaceCenter, centroid, true, RenderAs.Pie);
                curvedFaces.Add(curvedSegment);
            }
            else if (pieParams.StartAngle >= Math.PI && pieParams.StartAngle <= Math.PI * 2 && pieParams.StopAngle >= 0 && pieParams.StopAngle <= Math.PI)
            {
                Path curvedSegment = GetCurvedSegment(pieParams, pieParams.OuterRadius, 0, pieParams.StopAngle, topFaceCenter, bottomFaceCenter, centroid, true, RenderAs.Pie);
                curvedFaces.Add(curvedSegment);
            }
            else
            {
                Path curvedSegment = GetCurvedSegment(pieParams, pieParams.OuterRadius, pieParams.StartAngle, pieParams.StopAngle, topFaceCenter, bottomFaceCenter, centroid, true, RenderAs.Pie);
                curvedFaces.Add(curvedSegment);
            }

            return curvedFaces;
        }

        /// <summary>
        /// Returns curved path segment
        /// </summary>
        /// <param name="pieParams">Pie parameters</param>
        /// <param name="radius">Radius</param>
        /// <param name="startAngle">StartAngle</param>
        /// <param name="stopAngle">StopAngle</param>
        /// <param name="topFaceCenter">Top face center</param>
        /// <param name="bottomFaceCenter">Bottom face center</param>
        /// <param name="centroid">Centroid</param>
        /// <param name="isOuterSide">Whether outer side curve</param>
        /// <returns>Path</returns>
        private static Path GetCurvedSegment(SectorChartShapeParams pieParams, Double radius, Double startAngle, Double stopAngle, Point3D topFaceCenter, Point3D bottomFaceCenter, Point3D centroid, Boolean isOuterSide, RenderAs renderAs)
        {
            Point3D topArcStart = new Point3D();
            topArcStart.X = topFaceCenter.X + radius * Math.Cos(startAngle);
            topArcStart.Y = topFaceCenter.Y + radius * Math.Sin(startAngle) * pieParams.YAxisScaling;
            topArcStart.Z = (topFaceCenter.Z + radius) * Math.Sin(startAngle) * Math.Cos(pieParams.TiltAngle) + pieParams.Depth * Math.Cos(Math.PI / 2 - pieParams.TiltAngle);

            Point3D topArcStop = new Point3D();
            topArcStop.X = topFaceCenter.X + radius * Math.Cos(stopAngle);
            topArcStop.Y = topFaceCenter.Y + radius * Math.Sin(stopAngle) * pieParams.YAxisScaling;
            topArcStop.Z = (topFaceCenter.Z + radius) * Math.Sin(stopAngle) * Math.Cos(pieParams.TiltAngle) + pieParams.Depth * Math.Cos(Math.PI / 2 - pieParams.TiltAngle);

            Point3D bottomArcStart = new Point3D();
            bottomArcStart.X = bottomFaceCenter.X + radius * Math.Cos(startAngle);
            bottomArcStart.Y = bottomFaceCenter.Y + radius * Math.Sin(startAngle) * pieParams.YAxisScaling;
            bottomArcStart.Z = (bottomFaceCenter.Z + radius) * Math.Sin(startAngle) * Math.Cos(pieParams.TiltAngle) - pieParams.Depth * Math.Cos(Math.PI / 2 - pieParams.TiltAngle);

            Point3D bottomArcStop = new Point3D();
            bottomArcStop.X = bottomFaceCenter.X + radius * Math.Cos(stopAngle);
            bottomArcStop.Y = bottomFaceCenter.Y + radius * Math.Sin(stopAngle) * pieParams.YAxisScaling;
            bottomArcStop.Z = (bottomFaceCenter.Z + radius) * Math.Sin(stopAngle) * Math.Cos(pieParams.TiltAngle) - pieParams.Depth * Math.Cos(Math.PI / 2 - pieParams.TiltAngle);

            Path pieFace = new Path() { Tag = new ElementData() { Element = pieParams.TagReference } };

            // pieFace.Fill = pieParams.Lighting ? Graphics.GetLightingEnabledBrush(pieParams.Background, "Linear", new Double[] { 0.745, 0.99,  }) : pieParams.Background;

            pieFace.Fill = Get3DFaceColor(RenderAs.Doughnut, pieParams.Lighting, pieParams.Background, PieFaceTypes.CurvedSurface, startAngle,stopAngle, pieParams.TiltAngle);
            
            //if (renderAs == RenderAs.Pie)
            //{
            //    pieFace.Fill = pieParams.Lighting ? Graphics.GetLightingEnabledBrush(pieParams.Background, startAngle * 180 / Math.PI, "Linear", new Double[] { 0.99, 0.845 }) : pieParams.Background;
            //}
            //else
            //{
            //    if (stopAngle > Math.PI)
            //        pieFace.Fill = pieParams.Lighting ? Graphics.GetLightingEnabledBrush(pieParams.Background, pieParams.TiltAngle * 180 / Math.PI, "Linear", new Double[] { 0.99, 0.845 }) : pieParams.Background;
            //    else
            //        pieFace.Fill = pieParams.Lighting ? Graphics.GetLightingEnabledBrush(pieParams.Background, startAngle * 180 / Math.PI, "Linear", new Double[] { 0.99, 0.845 }) : pieParams.Background;
            //}

            List<PathGeometryParams> pathGeometryList = new List<PathGeometryParams>();

            Boolean isLargeArc = (Math.Abs(stopAngle - startAngle) > Math.PI) ? true : false;
            if (stopAngle < startAngle)
                isLargeArc = (Math.Abs((stopAngle + Math.PI * 2) - startAngle) > Math.PI) ? true : false;

            pathGeometryList.Add(new LineSegmentParams(new Point(topArcStop.X, topArcStop.Y)));
            pathGeometryList.Add(new ArcSegmentParams(new Size(radius, radius * pieParams.YAxisScaling), 0, isLargeArc, SweepDirection.Counterclockwise, new Point(topArcStart.X, topArcStart.Y)));
            pathGeometryList.Add(new LineSegmentParams(new Point(bottomArcStart.X, bottomArcStart.Y)));
            pathGeometryList.Add(new ArcSegmentParams(new Size(radius, radius * pieParams.YAxisScaling), 0, isLargeArc, SweepDirection.Clockwise, new Point(bottomArcStop.X, bottomArcStop.Y)));

            pieFace.Data = GetPathGeometryFromList(FillRule.Nonzero, new Point(bottomArcStop.X, bottomArcStop.Y), pathGeometryList, true);
            PathFigure figure = (pieFace.Data as PathGeometry).Figures[0];
            PathSegmentCollection segments = figure.Segments;

            return pieFace;
        }

        /// <summary>
        /// Returns a pie face
        /// </summary>
        /// <param name="pieParams">Pie parameters</param>
        /// <param name="centroid">Centroid</param>
        /// <param name="center">Center</param>
        /// <param name="arcStart">Arc dateTime point</param>
        /// <param name="arcStop">Arc stop point</param>
        /// <returns>Path</returns>
        private static Path GetPieFace(SectorChartShapeParams pieParams, Point3D centroid, Point3D center, Point3D arcStart, Point3D arcStop)
        {
            Path pieFace = new Path() { Tag = new ElementData() { Element = pieParams.TagReference } };

            //pieFace.Fill = pieParams.Lighting ? Graphics.GetLightingEnabledBrush(pieParams.Background, "Linear", new Double[] { 0.99, 0.854 }) : pieParams.Background;

            pieFace.Fill = Get3DFaceColor(RenderAs.Pie, pieParams.Lighting, pieParams.Background, PieFaceTypes.Top, Double.NaN, Double.NaN, Double.NaN);

            List<PathGeometryParams> pathGeometryList = new List<PathGeometryParams>();

            pathGeometryList.Add(new LineSegmentParams(new Point(arcStop.X, arcStop.Y)));
            pathGeometryList.Add(new ArcSegmentParams(new Size(pieParams.OuterRadius, pieParams.OuterRadius * pieParams.YAxisScaling), 0, pieParams.IsLargerArc, SweepDirection.Counterclockwise, new Point(arcStart.X, arcStart.Y)));
            pathGeometryList.Add(new LineSegmentParams(new Point(center.X, center.Y)));

            pieFace.Data = GetPathGeometryFromList(FillRule.Nonzero, new Point(center.X, center.Y), pathGeometryList, true);

            PathFigure figure = (pieFace.Data as PathGeometry).Figures[0];
            PathSegmentCollection segments = figure.Segments;

            return pieFace;
        }

        /// <summary>
        /// Returns a pie side path
        /// </summary>
        /// <param name="pieParams">Pie parameters</param>
        /// <param name="centroid">Centroid</param>
        /// <param name="centerTop">Center top</param>
        /// <param name="centerBottom">Center bottom</param>
        /// <param name="outerTop">Outer top</param>
        /// <param name="outerBottom">Outer bottom</param>
        /// <returns>Path</returns>
        private static Path GetPieSide(SectorChartShapeParams pieParams, Point3D centroid, Point3D centerTop, Point3D centerBottom, Point3D outerTop, Point3D outerBottom)
        {   
            Path pieFace = new Path() { Tag = new ElementData() { Element = pieParams.TagReference } };

            //pieFace.Fill = pieParams.Lighting ? Graphics.GetLightingEnabledBrush(pieParams.Background, "Linear", new Double[] { 0.845, 0.79 }) : pieParams.Background;

            pieFace.Fill = Get3DFaceColor(RenderAs.Pie, pieParams.Lighting, pieParams.Background, PieFaceTypes.Left, Double.NaN, Double.NaN, Double.NaN);
            
            List<PathGeometryParams> pathGeometryList = new List<PathGeometryParams>();

            pathGeometryList.Add(new LineSegmentParams(new Point(centerTop.X, centerTop.Y)));
            pathGeometryList.Add(new LineSegmentParams(new Point(outerTop.X, outerTop.Y)));
            pathGeometryList.Add(new LineSegmentParams(new Point(outerBottom.X, outerBottom.Y)));
            pathGeometryList.Add(new LineSegmentParams(new Point(centerBottom.X, centerBottom.Y)));

            pieFace.Data = GetPathGeometryFromList(FillRule.Nonzero, new Point(centerBottom.X, centerBottom.Y), pathGeometryList, true);
            PathFigure figure = (pieFace.Data as PathGeometry).Figures[0];
            PathSegmentCollection segments = figure.Segments;

            return pieFace;
        }

        /// <summary>
        /// Returns a doughnut face
        /// </summary>
        /// <param name="doughnutParams">Doughnut parameters</param>
        /// <param name="centroid">Centroid</param>
        /// <param name="arcInnerStart">Arc inner dateTime point</param>
        /// <param name="arcInnerStop">Arc inner stop point</param>
        /// <param name="arcOuterStart">Arc outer dateTime point</param>
        /// <param name="arcOuterStop">Arc outer stop point</param>
        /// <param name="isTopFace">Whether a top face</param>
        /// <returns>Path</returns>
        private static Path GetDoughnutFace(SectorChartShapeParams doughnutParams, Point3D centroid, Point3D arcInnerStart, Point3D arcInnerStop, Point3D arcOuterStart, Point3D arcOuterStop, Boolean isTopFace)
        {
            Path pieFace = new Path() { Tag = new ElementData() { Element = doughnutParams.TagReference } };

            //pieFace.Fill = doughnutParams.Lighting ? Graphics.GetLightingEnabledBrush(doughnutParams.Background, -75, "Linear", new Double[] { 0.99, 0.854 }) : doughnutParams.Background;
            pieFace.Fill = Get3DFaceColor(RenderAs.Doughnut, doughnutParams.Lighting, doughnutParams.Background, PieFaceTypes.Top, Double.NaN, Double.NaN, Double.NaN);
           
           
            List<PathGeometryParams> pathGeometryList = new List<PathGeometryParams>();

            pathGeometryList.Add(new LineSegmentParams(new Point(arcOuterStop.X, arcOuterStop.Y)));
            pathGeometryList.Add(new ArcSegmentParams(new Size(doughnutParams.OuterRadius, doughnutParams.OuterRadius * doughnutParams.YAxisScaling), 0, doughnutParams.IsLargerArc, SweepDirection.Counterclockwise, new Point(arcOuterStart.X, arcOuterStart.Y)));
            pathGeometryList.Add(new LineSegmentParams(new Point(arcInnerStart.X, arcInnerStart.Y)));
            pathGeometryList.Add(new ArcSegmentParams(new Size(doughnutParams.InnerRadius, doughnutParams.InnerRadius * doughnutParams.YAxisScaling), 0, doughnutParams.IsLargerArc, SweepDirection.Clockwise, new Point(arcInnerStop.X, arcInnerStop.Y)));

            pieFace.Data = GetPathGeometryFromList(FillRule.Nonzero, new Point(arcInnerStop.X, arcInnerStop.Y), pathGeometryList, true);
            Point3D midPoint = GetFaceZIndex(arcInnerStart, arcInnerStop, arcOuterStart, arcOuterStop);
            if (isTopFace)
                pieFace.SetValue(Canvas.ZIndexProperty, (Int32)(doughnutParams.Height * 200));
            else
                pieFace.SetValue(Canvas.ZIndexProperty, (Int32)(-doughnutParams.Height * 200));
            return pieFace;
        }

        /// <summary>
        /// Returns doughnut curved shapes
        /// </summary>
        /// <param name="doughnutParams">Doughnut parameters</param>
        /// <param name="centroid">Centroid</param>
        /// <param name="topFaceCenter">Top face center</param>
        /// <param name="bottomFaceCenter">Bottom face center</param>
        /// <returns>List of shape</returns>
        private static List<Shape> GetDoughnutCurvedFace(SectorChartShapeParams doughnutParams, Point3D centroid, Point3D topFaceCenter, Point3D bottomFaceCenter)
        {
            List<Shape> curvedFaces = new List<Shape>();

            if (doughnutParams.StartAngle >= Math.PI && doughnutParams.StartAngle <= Math.PI * 2 && doughnutParams.StopAngle >= Math.PI && doughnutParams.StopAngle <= Math.PI * 2 && doughnutParams.IsLargerArc)
            {
                // Outer curved path
                Path curvedSegment = GetCurvedSegment(doughnutParams, doughnutParams.OuterRadius, 0, Math.PI, topFaceCenter, bottomFaceCenter, centroid, true, RenderAs.Doughnut);
                curvedFaces.Add(curvedSegment);

                // Inner curved paths
                curvedSegment = GetCurvedSegment(doughnutParams, doughnutParams.InnerRadius, doughnutParams.StartAngle, 0, topFaceCenter, bottomFaceCenter, centroid, false, RenderAs.Doughnut);
                curvedFaces.Add(curvedSegment);

                curvedSegment = GetCurvedSegment(doughnutParams, doughnutParams.InnerRadius, Math.PI, doughnutParams.StopAngle, topFaceCenter, bottomFaceCenter, centroid, false, RenderAs.Doughnut);
                curvedFaces.Add(curvedSegment);
            }
            else if (doughnutParams.StartAngle >= 0 && doughnutParams.StartAngle <= Math.PI && doughnutParams.StopAngle >= 0 && doughnutParams.StopAngle <= Math.PI && doughnutParams.IsLargerArc)
            {
                // Outer curved paths
                Path curvedSegment = GetCurvedSegment(doughnutParams, doughnutParams.OuterRadius, doughnutParams.StartAngle, Math.PI, topFaceCenter, bottomFaceCenter, centroid, true, RenderAs.Doughnut);
                curvedFaces.Add(curvedSegment);
                curvedSegment = GetCurvedSegment(doughnutParams, doughnutParams.OuterRadius, Math.PI * 2, doughnutParams.StopAngle, topFaceCenter, bottomFaceCenter, centroid, true, RenderAs.Doughnut);
                curvedFaces.Add(curvedSegment);

                // Inner curved path
                curvedSegment = GetCurvedSegment(doughnutParams, doughnutParams.InnerRadius, Math.PI, Math.PI * 2, topFaceCenter, bottomFaceCenter, centroid, false, RenderAs.Doughnut);
                curvedFaces.Add(curvedSegment);
            }
            else if (doughnutParams.StartAngle >= 0 && doughnutParams.StartAngle <= Math.PI && doughnutParams.StopAngle >= Math.PI && doughnutParams.StopAngle <= Math.PI * 2)
            {
                // Outer curved path
                Path curvedSegment = GetCurvedSegment(doughnutParams, doughnutParams.OuterRadius, doughnutParams.StartAngle, Math.PI, topFaceCenter, bottomFaceCenter, centroid, true, RenderAs.Doughnut);
                curvedFaces.Add(curvedSegment);

                // Inner curved path
                curvedSegment = GetCurvedSegment(doughnutParams, doughnutParams.InnerRadius, Math.PI, doughnutParams.StopAngle, topFaceCenter, bottomFaceCenter, centroid, false, RenderAs.Doughnut);
                curvedFaces.Add(curvedSegment);
            }
            else if (doughnutParams.StartAngle >= Math.PI && doughnutParams.StartAngle <= Math.PI * 2 && doughnutParams.StopAngle >= 0 && doughnutParams.StopAngle <= Math.PI)
            {
                // Outer curved path
                Path curvedSegment = GetCurvedSegment(doughnutParams, doughnutParams.OuterRadius, 0, doughnutParams.StopAngle, topFaceCenter, bottomFaceCenter, centroid, true, RenderAs.Doughnut);
                curvedFaces.Add(curvedSegment);

                // Inner curved path
                curvedSegment = GetCurvedSegment(doughnutParams, doughnutParams.InnerRadius, doughnutParams.StartAngle, 0, topFaceCenter, bottomFaceCenter, centroid, false, RenderAs.Doughnut);
                curvedFaces.Add(curvedSegment);
            }
            else
            {
                if (doughnutParams.StartAngle >= 0 && doughnutParams.StopAngle <= Math.PI)
                {
                    Path curvedSegment = GetCurvedSegment(doughnutParams, doughnutParams.OuterRadius, doughnutParams.StartAngle, doughnutParams.StopAngle, topFaceCenter, bottomFaceCenter, centroid, true, RenderAs.Doughnut);
                    curvedFaces.Add(curvedSegment);
                }
                else
                {
                    Path curvedSegment = curvedSegment = GetCurvedSegment(doughnutParams, doughnutParams.InnerRadius, doughnutParams.StartAngle, doughnutParams.StopAngle, topFaceCenter, bottomFaceCenter, centroid, false, RenderAs.Doughnut);
                    curvedFaces.Add(curvedSegment);
                }
            }

            return curvedFaces;
        }

        /// <summary>
        /// Returns a centroid
        /// </summary>
        /// <param name="points">Array of Point3D</param>
        /// <returns>Point3D</returns>
        private static Point3D GetCentroid(params Point3D[] points)
        {
            Double sumX = 0;
            Double sumY = 0;
            Double sumZ = 0;

            foreach (Point3D point in points)
            {
                sumX += point.X;
                sumY += point.Y;
                sumZ += point.Z;
            }

            Point3D centroid = new Point3D();
            centroid.X = sumX / points.Length;
            centroid.Y = sumY / points.Length;
            centroid.Z = sumZ / points.Length;

            return centroid;
        }

        /// <summary>
        /// Returns face zindex
        /// </summary>
        /// <param name="points">Array of Point3D</param>
        /// <returns>Point3D</returns>
        private static Point3D GetFaceZIndex(params Point3D[] points)
        {
            return GetCentroid(points);
        }

        /// <summary>
        /// Returns a path geometry
        /// </summary>
        /// <param name="fillRule">FillRule</param>
        /// <param name="startPoint">Start point</param>
        /// <param name="pathGeometryParams">List of path geometry parameters</param>
        /// <returns>PathGeometry</returns>
        private static PathGeometry GetPathGeometryFromList(FillRule fillRule, Point startPoint, List<PathGeometryParams> pathGeometryParams, Boolean isClosed)
        {
            PathGeometry pathGeometry = new PathGeometry();

            pathGeometry.FillRule = fillRule;
            pathGeometry.Figures = new PathFigureCollection();

            PathFigure pathFigure = new PathFigure();

            pathFigure.StartPoint = startPoint;
            pathFigure.Segments = new PathSegmentCollection();
            pathFigure.IsClosed = isClosed;

            foreach (PathGeometryParams param in pathGeometryParams)
            {
                switch (param.GetType().Name)
                {
                    case "LineSegmentParams":
                        LineSegment lineSegment = new LineSegment();
                        lineSegment.Point = param.EndPoint;
                        pathFigure.Segments.Add(lineSegment);
                        break;

                    case "ArcSegmentParams":
                        ArcSegment arcSegment = new ArcSegment();

                        arcSegment.Point = param.EndPoint;
                        arcSegment.IsLargeArc = (param as ArcSegmentParams).IsLargeArc;
                        arcSegment.RotationAngle = (param as ArcSegmentParams).RotationAngle;
                        arcSegment.SweepDirection = (param as ArcSegmentParams).SweepDirection;
                        arcSegment.Size = (param as ArcSegmentParams).Size;
                        pathFigure.Segments.Add(arcSegment);

                        break;
                }
            }

            pathGeometry.Figures.Add(pathFigure);

            return pathGeometry;
        }

        /// <summary>
        /// Returns pie gradient brush
        /// </summary>
        /// <returns>Brush</returns>
        private static Brush GetPieGradianceBrush()
        {
            RadialGradientBrush brush = new RadialGradientBrush() { GradientOrigin = new Point(0.5, 0.5) };
            brush.GradientStops = new GradientStopCollection();

            brush.GradientStops.Add(new GradientStop() { Offset = 0, Color = Color.FromArgb((Byte)0, (Byte)0, (Byte)0, (Byte)0) });
            brush.GradientStops.Add(new GradientStop() { Offset = 0.7, Color = Color.FromArgb((Byte)34, (Byte)0, (Byte)0, (Byte)0) });
            brush.GradientStops.Add(new GradientStop() { Offset = 1, Color = Color.FromArgb((Byte)127, (Byte)0, (Byte)0, (Byte)0) });

            return brush;
        }

        /// <summary>
        /// Returns doughnut gradient brush
        /// </summary>
        /// <returns>Brush</returns>
        private static Brush GetDoughnutGradianceBrush()
        {
            RadialGradientBrush brush = new RadialGradientBrush() { GradientOrigin = new Point(0.5, 0.5) };
            brush.GradientStops = new GradientStopCollection();

            brush.GradientStops.Add(new GradientStop() { Offset = 0, Color = Color.FromArgb((Byte)0, (Byte)0, (Byte)0, (Byte)0) });
            brush.GradientStops.Add(new GradientStop() { Offset = 0.5, Color = Color.FromArgb((Byte)127, (Byte)0, (Byte)0, (Byte)0) });
            brush.GradientStops.Add(new GradientStop() { Offset = 0.75, Color = Color.FromArgb((Byte)00, (Byte)0, (Byte)0, (Byte)0) });
            brush.GradientStops.Add(new GradientStop() { Offset = 1, Color = Color.FromArgb((Byte)127, (Byte)0, (Byte)0, (Byte)0) });

            return brush;
        }

        /// <summary>
        /// Generate DoubleCollection from array of values
        /// </summary>
        /// <param name="values">Array of values</param>
        /// <returns>PointCollection</returns>
        private static PointCollection GenerateDoubleCollection(params Point[] values)
        {
            PointCollection collection = new PointCollection();

            foreach (Point value in values)
                collection.Add(value);

            return collection;
        }

        /// <summary>
        /// CreatePointAnimation
        /// </summary>
        /// <param name="target">Target object to animate</param>
        /// <param name="property">Property to animate</param>
        /// <param name="beginTime">Animation begin time</param>
        /// <param name="frameTimes">Animation frame times</param>
        /// <param name="values">List of values</param>
        /// <param name="splines">List of animation splines</param>
        /// <returns>PointAnimationUsingKeyFrames</returns>
        private static PointAnimationUsingKeyFrames CreatePointAnimation(DataSeries currentDataSeries, DataPoint currentDataPoint, DependencyObject target, String property, Double beginTime, List<Double> frameTimes, List<Point> values, List<KeySpline> splines)
        {
            PointAnimationUsingKeyFrames da = new PointAnimationUsingKeyFrames();
#if WPF
            target.SetValue(FrameworkElement.NameProperty, target.GetType().Name +  Guid.NewGuid().ToString().Replace('-', '_'));
            Storyboard.SetTargetName(da, target.GetValue(FrameworkElement.NameProperty).ToString());

            currentDataSeries.Chart._rootElement.RegisterName((string)target.GetValue(FrameworkElement.NameProperty), target);
            currentDataPoint.Chart._rootElement.RegisterName((string)target.GetValue(FrameworkElement.NameProperty), target);
#else
            Storyboard.SetTarget(da, target);
#endif
            Storyboard.SetTargetProperty(da, new PropertyPath(property));

            da.BeginTime = TimeSpan.FromSeconds(beginTime);

            for (Int32 index = 0; index < splines.Count; index++)
            {
                SplinePointKeyFrame keyFrame = new SplinePointKeyFrame();
                keyFrame.KeySpline = splines[index];
                keyFrame.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(frameTimes[index]));
                keyFrame.Value = values[index];
                da.KeyFrames.Add(keyFrame);
            }

            return da;
        }

        /// <summary>
        /// Generate points for animation
        /// </summary>
        /// <param name="center">Center point</param>
        /// <param name="radius">Radius</param>
        /// <param name="startAngle">Start angle</param>
        /// <param name="stopAngle">End angle</param>
        /// <returns>List of points</returns>
        private static List<Point> GenerateAnimationPoints(Point center, Double radius, Double startAngle, Double stopAngle)
        {
            List<Point> points = new List<Point>();
            Double step = Math.Abs(startAngle - stopAngle) / 100;

            if (step <= 0) return points;
            for (Double angle = startAngle; angle <= stopAngle; angle += step)
            {
                points.Add(new Point(center.X + radius * Math.Cos(angle), center.Y + radius * Math.Sin(angle)));
            }

            points.Add(new Point(center.X + radius * Math.Cos(stopAngle), center.Y + radius * Math.Sin(stopAngle)));

            return points;
        }

        /// <summary>
        /// Generate animation frames
        /// </summary>
        /// <param name="count">Number of frames</param>
        /// <param name="maxTime">Max time</param>
        /// <returns>List of frames</returns>
        private static List<Double> GenerateAnimationFrames(int count, double maxTime)
        {
            List<double> frames = new List<double>();
            for (int i = 0; i < count; i++)
            {
                frames.Add(maxTime * i / (double)(count - 1));
            }
            return frames;
        }

        /// <summary>
        /// Create PathSegment animation
        /// </summary>
        /// <param name="storyboard">Storyboard</param>
        /// <param name="target">Animation target</param>
        /// <param name="center">Center point</param>
        /// <param name="radius">Radius</param>
        /// <param name="startAngle">Start angle</param>
        /// <param name="stopAngle">Stop angle</param>
        /// <returns>Storyboard</returns>
        private static Storyboard CreatePathSegmentAnimation(DataSeries currentDataSeries, DataPoint currentDataPoint, Storyboard storyboard, PathSegment target, Point center, Double radius, Double startAngle, Double stopAngle)
        {
            List<Point> points = GenerateAnimationPoints(center, radius, startAngle, stopAngle);
            List<Double> frames = GenerateAnimationFrames(points.Count, 1);
            List<KeySpline> splines = AnimationHelper.GenerateKeySplineList(points.Count);

            PointAnimationUsingKeyFrames pieSliceAnimation = null;

            if (typeof(ArcSegment).IsInstanceOfType(target))
            {
                pieSliceAnimation = CreatePointAnimation(currentDataSeries, currentDataPoint, target, "(ArcSegment.Point)", 0.5, frames, points, splines);
            }
            else
            {
                pieSliceAnimation = CreatePointAnimation(currentDataSeries, currentDataPoint, target, "(LineSegment.Point)", 0.5, frames, points, splines);
            }

            storyboard.Children.Add(pieSliceAnimation);

            return storyboard;
        }

        /// <summary>
        /// Create PathFigure animation
        /// </summary>
        /// <param name="storyboard">Storyboard</param>
        /// <param name="target">Animation target</param>
        /// <param name="center">Center point</param>
        /// <param name="radius">Radius</param>
        /// <param name="startAngle">Start angle</param>
        /// <param name="stopAngle">Stop angle</param>
        /// <returns>Storyboard</returns>
        private static Storyboard CreatePathFigureAnimation(DataSeries currentDataSeries, DataPoint currentDataPoint, Storyboard storyboard, PathFigure target, Point center, Double radius, Double startAngle, Double stopAngle)
        {
            List<Point> points = GenerateAnimationPoints(center, radius, startAngle, stopAngle);
            List<Double> frames = GenerateAnimationFrames(points.Count, 1);
            List<KeySpline> splines = AnimationHelper.GenerateKeySplineList(points.Count);

            PointAnimationUsingKeyFrames pieSliceAnimation = CreatePointAnimation(currentDataSeries, currentDataPoint, target, "(PathFigure.StartPoint)", 0.5, frames, points, splines);
            storyboard.Children.Add(pieSliceAnimation);
            return storyboard;
        }

        /// <summary>
        /// Create LabelLine animation
        /// </summary>
        /// <param name="storyboard">Storyboard</param>
        /// <param name="target">Animation target</param>
        /// <param name="points">Array of points</param>
        /// <returns>Storyboard</returns>
        private static Storyboard CreateLabelLineAnimation(DataSeries currentDataSeries, DataPoint currentDataPoint, Storyboard storyboard, PathSegment target, params Point[] points)
        {
            List<Point> pointsList = points.ToList();
            List<Double> frames = GenerateAnimationFrames(pointsList.Count, 1);
            List<KeySpline> splines = AnimationHelper.GenerateKeySplineList(pointsList.Count);

            PointAnimationUsingKeyFrames labelLineAnimation = CreatePointAnimation(currentDataSeries, currentDataPoint, target, "(LineSegment.Point)", 1 + 0.5, frames, pointsList, splines);
            storyboard.Children.Add(labelLineAnimation);
            return storyboard;
        }

        /// <summary>
        /// Create DoubleAnimation
        /// </summary>
        /// <param name="target">Animation target object</param>
        /// <param name="property">Animation target property</param>
        /// <param name="beginTime">Animation begin time</param>
        /// <param name="frameTime">Frame time collection</param>
        /// <param name="values">Target value collection</param>
        /// <param name="splines">List of KeySpline</param>
        /// <returns>DoubleAnimationUsingKeyFrames</returns>
        internal static DoubleAnimationUsingKeyFrames CreateDoubleAnimation(DataSeries dataSeries, DataPoint dataPoint, DependencyObject target, String property, Double beginTime, DoubleCollection frameTime, DoubleCollection values, List<KeySpline> splines)
        {
            DoubleAnimationUsingKeyFrames da = new DoubleAnimationUsingKeyFrames();
#if WPF
            target.SetValue(FrameworkElement.NameProperty, target.GetType().Name + Guid.NewGuid().ToString().Replace('-', '_'));
            Storyboard.SetTargetName(da, target.GetValue(FrameworkElement.NameProperty).ToString());

            dataSeries.Chart._rootElement.RegisterName((string)target.GetValue(FrameworkElement.NameProperty), target);
            dataPoint.Chart._rootElement.RegisterName((string)target.GetValue(FrameworkElement.NameProperty), target);
#else
            Storyboard.SetTarget(da, target);
#endif
            Storyboard.SetTargetProperty(da, new PropertyPath(property));

            da.BeginTime = TimeSpan.FromSeconds(beginTime);

            for (Int32 index = 0; index < splines.Count; index++)
            {
                SplineDoubleKeyFrame keyFrame = new SplineDoubleKeyFrame();
                keyFrame.KeySpline = splines[index];
                keyFrame.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(frameTime[index]));
                keyFrame.Value = values[index];
                da.KeyFrames.Add(keyFrame);
            }

            return da;
        }

        /// <summary>
        /// Create opacity animation
        /// </summary>
        /// <param name="storyboard">Storyboard</param>
        /// <param name="target">Target object</param>
        /// <param name="beginTime">Animation begin time</param>
        /// <param name="opacity">Target opacity</param>
        /// <param name="duration">Animation duration</param>
        /// <returns></returns>
        private static Storyboard CreateOpacityAnimation(DataSeries currentDataSeries, DataPoint currentDataPoint, Storyboard storyboard, DependencyObject target, Double beginTime, Double opacity, Double duration)
        {
            DoubleCollection values = Graphics.GenerateDoubleCollection(0, opacity);
            DoubleCollection frames = Graphics.GenerateDoubleCollection(0, duration);
            List<KeySpline> splines = AnimationHelper.GenerateKeySplineList(frames.Count);
            DoubleAnimationUsingKeyFrames opacityAnimation = CreateDoubleAnimation(currentDataSeries, currentDataPoint, target, "(UIElement.Opacity)", beginTime + 0.5, frames, values, splines);
            storyboard.Children.Add(opacityAnimation);
            return storyboard;
        }

        /// <summary>
        /// Create interactivity animation for LabelLine
        /// </summary>
        /// <param name="storyboard">Storyboard</param>
        /// <param name="target">Target object</param>
        /// <param name="points">Array of Points</param>
        /// <returns>Storyboard</returns>
        private static Storyboard CreateLabelLineInteractivityAnimation(DataSeries currentDataSeries, DataPoint currentDataPoint, Storyboard storyboard, PathSegment target, params Point[] points)
        {
            List<Point> pointsList = points.ToList();
            List<Double> frames = GenerateAnimationFrames(pointsList.Count, 0.4);
            List<KeySpline> splines = AnimationHelper.GenerateKeySplineList(pointsList.Count);

            PointAnimationUsingKeyFrames labelLineAnimation = CreatePointAnimation(currentDataSeries, currentDataPoint, target, "(LineSegment.Point)", 0, frames, pointsList, splines);
            storyboard.Children.Add(labelLineAnimation);

            return storyboard;
        }

        /// <summary>
        /// Create exploding in animation for 2D Pie/Doughnut
        /// </summary>
        /// <param name="dataPoint">DataPoint</param>
        /// <param name="storyboard">Stroyboard used for animation</param>
        /// <param name="pathElements">Path elements reference</param>
        /// <param name="label">Label reference</param>
        /// <param name="labelLine">Label line reference</param>
        /// <param name="unExplodedPoints">Unexploded points</param>
        /// <param name="explodedPoints">Exploded points</param>
        /// <param name="xOffset">X offset</param>
        /// <param name="yOffset">Y offset</param>
        /// <returns>Storyboard</returns>
        private static Storyboard CreateExplodingOut2DAnimation(DataSeries currentDataSeries, DataPoint dataPoint, Storyboard storyboard, Panel visual, Panel label, Path labelLine, TranslateTransform translateTransform, PieDoughnut2DPoints unExplodedPoints, PieDoughnut2DPoints explodedPoints, Double xOffset, Double yOffset)
        {
            #region Animating Silce
            DoubleCollection values = Graphics.GenerateDoubleCollection(0, xOffset);
            DoubleCollection frames = Graphics.GenerateDoubleCollection(0, 0.4);
            List<KeySpline> splines = AnimationHelper.GenerateKeySplineList
                (
                    new Point(0, 0), new Point(1, 1),
                    new Point(0, 0), new Point(0, 1)
                );

            DoubleAnimationUsingKeyFrames sliceXAnimation = CreateDoubleAnimation(currentDataSeries, dataPoint, translateTransform, "(TranslateTransform.X)", 0, frames, values, splines);

            values = Graphics.GenerateDoubleCollection(0, yOffset);
            frames = Graphics.GenerateDoubleCollection(0, 0.4);
            splines = AnimationHelper.GenerateKeySplineList
                (
                    new Point(0, 0), new Point(1, 1),
                    new Point(0, 0), new Point(0, 1)
                );

            DoubleAnimationUsingKeyFrames sliceYAnimation = CreateDoubleAnimation(currentDataSeries, dataPoint, translateTransform, "(TranslateTransform.Y)", 0, frames, values, splines);

            storyboard.Children.Add(sliceXAnimation);
            storyboard.Children.Add(sliceYAnimation);
            #endregion Animating Silce

            #region Animating Label

            if (dataPoint.LabelStyle == LabelStyles.Inside)
            {
                if (label != null)
                {
                    translateTransform = new TranslateTransform();
                    label.RenderTransform = translateTransform;

                    values = Graphics.GenerateDoubleCollection(0, xOffset);
                    frames = Graphics.GenerateDoubleCollection(0, 0.4);
                    splines = AnimationHelper.GenerateKeySplineList
                        (
                            new Point(0, 0), new Point(1, 1),
                            new Point(0, 0), new Point(0, 1)
                        );

                    DoubleAnimationUsingKeyFrames labelXAnimation = CreateDoubleAnimation(currentDataSeries, dataPoint, translateTransform, "(TranslateTransform.X)", 0, frames, values, splines);

                    values = Graphics.GenerateDoubleCollection(0, yOffset);
                    frames = Graphics.GenerateDoubleCollection(0, 0.4);
                    splines = AnimationHelper.GenerateKeySplineList
                        (
                            new Point(0, 0), new Point(1, 1),
                            new Point(0, 0), new Point(0, 1)
                        );

                    DoubleAnimationUsingKeyFrames labelYAnimation = CreateDoubleAnimation(currentDataSeries, dataPoint, translateTransform, "(TranslateTransform.Y)", 0, frames, values, splines);

                    storyboard.Children.Add(labelXAnimation);
                    storyboard.Children.Add(labelYAnimation);
                }
            }
            else
            {
                values = Graphics.GenerateDoubleCollection(unExplodedPoints.LabelPosition.X, explodedPoints.LabelPosition.X);
                frames = Graphics.GenerateDoubleCollection(0, 0.4);
                splines = AnimationHelper.GenerateKeySplineList
                    (
                        new Point(0, 0), new Point(1, 1),
                        new Point(0, 0), new Point(0, 1)
                    );

                DoubleAnimationUsingKeyFrames labelXAnimation = CreateDoubleAnimation(currentDataSeries, dataPoint, label, "(Canvas.Left)", 0, frames, values, splines);
                storyboard.Children.Add(labelXAnimation);
            }

            #endregion Animating Label

            #region Animating Label Line
            if (labelLine != null)
            {
                PathFigure figure = (labelLine.Data as PathGeometry).Figures[0];
                PathSegmentCollection segments = figure.Segments;
                storyboard = CreateLabelLineInteractivityAnimation(currentDataSeries, dataPoint, storyboard, segments[0], unExplodedPoints.LabelLineMidPoint, explodedPoints.LabelLineMidPoint);
                storyboard = CreateLabelLineInteractivityAnimation(currentDataSeries, dataPoint, storyboard, segments[1], unExplodedPoints.LabelLineEndPoint, explodedPoints.LabelLineEndPoint);
            }

            #endregion Animating Label Line

            return storyboard;
        }

        /// <summary>
        /// Create exploding in animation for 2D Pie/Doughnut
        /// </summary>
        /// <param name="dataPoint">DataPoint</param>
        /// <param name="storyboard">Stroyboard used for animation</param>
        /// <param name="pathElements">Path elements reference</param>
        /// <param name="label">Label reference</param>
        /// <param name="labelLine">Label line reference</param>
        /// <param name="unExplodedPoints">Unexploded points</param>
        /// <param name="explodedPoints">Exploded points</param>
        /// <param name="xOffset">X offset</param>
        /// <param name="yOffset">Y offset</param>
        /// <returns>Storyboard</returns>
        private static Storyboard CreateExplodingIn2DAnimation(DataSeries currentDataSeries, DataPoint dataPoint, Storyboard storyboard, Panel visual, Panel label, Path labelLine, TranslateTransform translateTransform, PieDoughnut2DPoints unExplodedPoints, PieDoughnut2DPoints explodedPoints, Double xOffset, Double yOffset)
        {

            #region Animating Silce
            DoubleCollection values = Graphics.GenerateDoubleCollection(xOffset, 0);
            DoubleCollection frames = Graphics.GenerateDoubleCollection(0, 0.4);
            List<KeySpline> splines = AnimationHelper.GenerateKeySplineList
                (
                    new Point(0, 0), new Point(1, 1),
                    new Point(0, 0), new Point(0, 1)
                );

            DoubleAnimationUsingKeyFrames sliceXAnimation = CreateDoubleAnimation(currentDataSeries, dataPoint, translateTransform, "(TranslateTransform.X)", 0, frames, values, splines);

            values = Graphics.GenerateDoubleCollection(yOffset, 0);
            frames = Graphics.GenerateDoubleCollection(0, 0.4);
            splines = AnimationHelper.GenerateKeySplineList
                (
                    new Point(0, 0), new Point(1, 1),
                    new Point(0, 0), new Point(0, 1)
                );

            DoubleAnimationUsingKeyFrames sliceYAnimation = CreateDoubleAnimation(currentDataSeries, dataPoint, translateTransform, "(TranslateTransform.Y)", 0, frames, values, splines);

            storyboard.Children.Add(sliceXAnimation);
            storyboard.Children.Add(sliceYAnimation);
            #endregion Animating Silce

            #region Animating Label
            if (dataPoint.LabelStyle == LabelStyles.Inside)
            {
                if (label != null)
                {

                    translateTransform = label.RenderTransform as TranslateTransform;
                    values = Graphics.GenerateDoubleCollection(xOffset, 0);
                    frames = Graphics.GenerateDoubleCollection(0, 0.4);
                    splines = AnimationHelper.GenerateKeySplineList
                    (
                        new Point(0, 0), new Point(1, 1),
                        new Point(0, 0), new Point(0, 1)
                    );

                    DoubleAnimationUsingKeyFrames labelXAnimation = CreateDoubleAnimation(currentDataSeries, dataPoint, translateTransform, "(TranslateTransform.X)", 0, frames, values, splines);

                    values = Graphics.GenerateDoubleCollection(yOffset, 0);
                    frames = Graphics.GenerateDoubleCollection(0, 0.4);
                    splines = AnimationHelper.GenerateKeySplineList
                    (
                        new Point(0, 0), new Point(1, 1),
                        new Point(0, 0), new Point(0, 1)
                    );

                    DoubleAnimationUsingKeyFrames labelYAnimation = CreateDoubleAnimation(currentDataSeries, dataPoint, translateTransform, "(TranslateTransform.Y)", 0, frames, values, splines);

                    storyboard.Children.Add(labelXAnimation);
                    storyboard.Children.Add(labelYAnimation);
                }
            }
            else
            {
                values = Graphics.GenerateDoubleCollection(explodedPoints.LabelPosition.X, unExplodedPoints.LabelPosition.X);
                frames = Graphics.GenerateDoubleCollection(0, 0.4);
                splines = AnimationHelper.GenerateKeySplineList
                    (
                        new Point(0, 0), new Point(1, 1),
                        new Point(0, 0), new Point(0, 1)
                    );

                DoubleAnimationUsingKeyFrames labelXAnimation = CreateDoubleAnimation(currentDataSeries, dataPoint, label, "(Canvas.Left)", 0, frames, values, splines);
                storyboard.Children.Add(labelXAnimation);
            }

            #endregion Animating Label

            #region Animating Label Line

            if (labelLine != null)
            {
                PathFigure figure = (labelLine.Data as PathGeometry).Figures[0];
                PathSegmentCollection segments = figure.Segments;
                storyboard = CreateLabelLineInteractivityAnimation(currentDataSeries, dataPoint, storyboard, segments[0], explodedPoints.LabelLineMidPoint, unExplodedPoints.LabelLineMidPoint);
                storyboard = CreateLabelLineInteractivityAnimation(currentDataSeries, dataPoint, storyboard, segments[1], explodedPoints.LabelLineEndPoint, unExplodedPoints.LabelLineEndPoint);
            }

            #endregion Animating Label Line
            return storyboard;
        }

        /// <summary>
        /// Create exploding out animation for 3D Pie/Doughnut
        /// </summary>
        /// <param name="dataPoint">DataPoint</param>
        /// <param name="storyboard">Stroyboard used for animation</param>
        /// <param name="pathElements">Path elements reference</param>
        /// <param name="label">Label reference</param>
        /// <param name="labelLine">Label line reference</param>
        /// <param name="unExplodedPoints">Unexploded points</param>
        /// <param name="explodedPoints">Exploded points</param>
        /// <param name="xOffset">X offset</param>
        /// <param name="yOffset">Y offset</param>
        /// <returns>Storyboard</returns>
        private static Storyboard CreateExplodingOut3DAnimation(DataSeries currentDataSeries, DataPoint dataPoint, Storyboard storyboard, List<Shape> pathElements, Panel label, Path labelLine, PieDoughnut3DPoints unExplodedPoints, PieDoughnut3DPoints explodedPoints, Double xOffset, Double yOffset)
        {
            DoubleCollection values;
            DoubleCollection frames;
            List<KeySpline> splines;

            #region Animating Slice

            foreach (Shape path in pathElements)
            {
                if (path == null) continue;
                TranslateTransform translateTransform = path.RenderTransform as TranslateTransform;

                values = Graphics.GenerateDoubleCollection(0, xOffset);
                frames = Graphics.GenerateDoubleCollection(0, 0.4);
                splines = AnimationHelper.GenerateKeySplineList
                    (
                        new Point(0, 0), new Point(1, 1),
                        new Point(0, 0), new Point(0, 1)
                    );

                DoubleAnimationUsingKeyFrames sliceXAnimation = CreateDoubleAnimation(currentDataSeries, dataPoint, translateTransform, "(TranslateTransform.X)", 0, frames, values, splines);

                values = Graphics.GenerateDoubleCollection(0, yOffset);
                frames = Graphics.GenerateDoubleCollection(0, 0.4);
                splines = AnimationHelper.GenerateKeySplineList
                    (
                        new Point(0, 0), new Point(1, 1),
                        new Point(0, 0), new Point(0, 1)
                    );

                DoubleAnimationUsingKeyFrames sliceYAnimation = CreateDoubleAnimation(currentDataSeries, dataPoint, translateTransform, "(TranslateTransform.Y)", 0, frames, values, splines);

                storyboard.Children.Add(sliceXAnimation);
                storyboard.Children.Add(sliceYAnimation);
            }

            #endregion Animating Slice

            #region Animating Label

            if (dataPoint.LabelStyle == LabelStyles.Inside)
            {
                if (label != null)
                {

                    TranslateTransform translateTransform = new TranslateTransform();
                    label.RenderTransform = translateTransform;

                    values = Graphics.GenerateDoubleCollection(0, xOffset);
                    frames = Graphics.GenerateDoubleCollection(0, 0.4);
                    splines = AnimationHelper.GenerateKeySplineList
                        (
                            new Point(0, 0), new Point(1, 1),
                            new Point(0, 0), new Point(0, 1)
                        );

                    DoubleAnimationUsingKeyFrames sliceXAnimation1 = CreateDoubleAnimation(currentDataSeries, dataPoint, translateTransform, "(TranslateTransform.X)", 0, frames, values, splines);

                    values = Graphics.GenerateDoubleCollection(0, yOffset);
                    frames = Graphics.GenerateDoubleCollection(0, 0.4);
                    splines = AnimationHelper.GenerateKeySplineList
                        (
                            new Point(0, 0), new Point(1, 1),
                            new Point(0, 0), new Point(0, 1)
                        );

                    DoubleAnimationUsingKeyFrames sliceYAnimation2 = CreateDoubleAnimation(currentDataSeries, dataPoint, translateTransform, "(TranslateTransform.Y)", 0, frames, values, splines);

                    storyboard.Children.Add(sliceXAnimation1);
                    storyboard.Children.Add(sliceYAnimation2);
                }
            }
            else
            {
                values = Graphics.GenerateDoubleCollection(unExplodedPoints.LabelPosition.X, explodedPoints.LabelPosition.X);
                frames = Graphics.GenerateDoubleCollection(0, 0.4);
                splines = AnimationHelper.GenerateKeySplineList
                    (
                        new Point(0, 0), new Point(1, 1),
                        new Point(0, 0), new Point(0, 1)
                    );

                DoubleAnimationUsingKeyFrames labelXAnimation = CreateDoubleAnimation(currentDataSeries, dataPoint, label, "(Canvas.Left)", 0, frames, values, splines);
                storyboard.Children.Add(labelXAnimation);
            }

            #endregion Animating Label

            #region Animating Label Line
            if (labelLine != null)
            {
                TranslateTransform translateTransform = labelLine.RenderTransform as TranslateTransform;

                values = Graphics.GenerateDoubleCollection(0, xOffset);
                frames = Graphics.GenerateDoubleCollection(0, 0.4);
                splines = AnimationHelper.GenerateKeySplineList
                    (
                        new Point(0, 0), new Point(1, 1),
                        new Point(0, 0), new Point(0, 1)
                    );

                DoubleAnimationUsingKeyFrames sliceXAnimation = CreateDoubleAnimation(currentDataSeries, dataPoint, translateTransform, "(TranslateTransform.X)", 0, frames, values, splines);

                values = Graphics.GenerateDoubleCollection(0, yOffset);
                frames = Graphics.GenerateDoubleCollection(0, 0.4);
                splines = AnimationHelper.GenerateKeySplineList
                    (
                        new Point(0, 0), new Point(1, 1),
                        new Point(0, 0), new Point(0, 1)
                    );

                DoubleAnimationUsingKeyFrames sliceYAnimation = CreateDoubleAnimation(currentDataSeries, dataPoint, translateTransform, "(TranslateTransform.Y)", 0, frames, values, splines);

                storyboard.Children.Add(sliceXAnimation);
                storyboard.Children.Add(sliceYAnimation);


                PathFigure figure = (labelLine.Data as PathGeometry).Figures[0];
                PathSegmentCollection segments = figure.Segments;
                storyboard = CreateLabelLineInteractivityAnimation(currentDataSeries, dataPoint, storyboard, segments[0], unExplodedPoints.LabelLineMidPoint, explodedPoints.LabelLineMidPoint);
                storyboard = CreateLabelLineInteractivityAnimation(currentDataSeries, dataPoint, storyboard, segments[1], unExplodedPoints.LabelLineEndPoint, explodedPoints.LabelLineEndPoint);
            }
            #endregion Animating Label Line

            return storyboard;
        }

        /// <summary>
        /// Create exploding in animation for 3D Pie/Doughnut
        /// </summary>
        /// <param name="dataPoint">DataPoint</param>
        /// <param name="storyboard">Stroyboard used for animation</param>
        /// <param name="pathElements">Path elements reference</param>
        /// <param name="label">Label reference</param>
        /// <param name="labelLine">Label line reference</param>
        /// <param name="unExplodedPoints">Unexploded points</param>
        /// <param name="explodedPoints">Exploded points</param>
        /// <param name="xOffset">X offset</param>
        /// <param name="yOffset">Y offset</param>
        /// <returns>Storyboard</returns>
        private static Storyboard CreateExplodingIn3DAnimation(DataSeries currentDataSeries, DataPoint dataPoint, Storyboard storyboard, List<Shape> pathElements, Panel label, Path labelLine, PieDoughnut3DPoints unExplodedPoints, PieDoughnut3DPoints explodedPoints, Double xOffset, Double yOffset)
        {
            DoubleCollection values;
            DoubleCollection frames;
            List<KeySpline> splines;

#if WPF
            if (storyboard != null && storyboard.GetValue(System.Windows.Media.Animation.Storyboard.TargetProperty) != null)
                storyboard.Stop();
#else
            if (storyboard != null)
                storyboard.Stop();
#endif

            #region Animating Slice

            foreach (Shape path in pathElements)
            {
                if (path == null) continue;

                TranslateTransform translateTransform = path.RenderTransform as TranslateTransform;

                values = Graphics.GenerateDoubleCollection(xOffset, 0);
                frames = Graphics.GenerateDoubleCollection(0, 0.4);
                splines = AnimationHelper.GenerateKeySplineList
                    (
                        new Point(0, 0), new Point(1, 1),
                        new Point(0, 0), new Point(0, 1)
                    );

                DoubleAnimationUsingKeyFrames sliceXAnimation = CreateDoubleAnimation(currentDataSeries, dataPoint, translateTransform, "(TranslateTransform.X)", 0, frames, values, splines);

                values = Graphics.GenerateDoubleCollection(yOffset, 0);
                frames = Graphics.GenerateDoubleCollection(0, 0.4);
                splines = AnimationHelper.GenerateKeySplineList
                    (
                        new Point(0, 0), new Point(1, 1),
                        new Point(0, 0), new Point(0, 1)
                    );

                DoubleAnimationUsingKeyFrames sliceYAnimation = CreateDoubleAnimation(currentDataSeries, dataPoint, translateTransform, "(TranslateTransform.Y)", 0, frames, values, splines);

                storyboard.Children.Add(sliceXAnimation);
                storyboard.Children.Add(sliceYAnimation);
            }

            #endregion Animating Slice

            #region Animating Label

            if (dataPoint.LabelStyle == LabelStyles.Inside)
            {
                if (label != null)
                {
                    TranslateTransform translateTransform = label.RenderTransform as TranslateTransform;

                    values = Graphics.GenerateDoubleCollection(xOffset, 0);
                    frames = Graphics.GenerateDoubleCollection(0, 0.4);
                    splines = AnimationHelper.GenerateKeySplineList
                        (
                            new Point(0, 0), new Point(1, 1),
                            new Point(0, 0), new Point(0, 1)
                        );

                    DoubleAnimationUsingKeyFrames labelXAnimation1 = CreateDoubleAnimation(currentDataSeries, dataPoint, translateTransform, "(TranslateTransform.X)", 0, frames, values, splines);

                    values = Graphics.GenerateDoubleCollection(yOffset, 0);
                    frames = Graphics.GenerateDoubleCollection(0, 0.4);
                    splines = AnimationHelper.GenerateKeySplineList
                        (
                            new Point(0, 0), new Point(1, 1),
                            new Point(0, 0), new Point(0, 1)
                        );

                    DoubleAnimationUsingKeyFrames labelYAnimation2 = CreateDoubleAnimation(currentDataSeries, dataPoint, translateTransform, "(TranslateTransform.Y)", 0, frames, values, splines);

                    storyboard.Children.Add(labelXAnimation1);
                    storyboard.Children.Add(labelYAnimation2);
                }
            }
            else
            {
                values = Graphics.GenerateDoubleCollection(explodedPoints.LabelPosition.X, unExplodedPoints.LabelPosition.X);
                frames = Graphics.GenerateDoubleCollection(0, 0.4);
                splines = AnimationHelper.GenerateKeySplineList
                    (
                        new Point(0, 0), new Point(1, 1),
                        new Point(0, 0), new Point(0, 1)
                    );

                DoubleAnimationUsingKeyFrames labelXAnimation = CreateDoubleAnimation(currentDataSeries, dataPoint, label, "(Canvas.Left)", 0, frames, values, splines);
                storyboard.Children.Add(labelXAnimation);
            }

            #endregion Animating Label

            #region Animating Label Line
            if (labelLine != null)
            {
                TranslateTransform translateTransform = labelLine.RenderTransform as TranslateTransform;

                values = Graphics.GenerateDoubleCollection(xOffset, 0);
                frames = Graphics.GenerateDoubleCollection(0, 0.4);
                splines = AnimationHelper.GenerateKeySplineList
                    (
                        new Point(0, 0), new Point(1, 1),
                        new Point(0, 0), new Point(0, 1)
                    );

                DoubleAnimationUsingKeyFrames sliceXAnimation = CreateDoubleAnimation(currentDataSeries, dataPoint, translateTransform, "(TranslateTransform.X)", 0, frames, values, splines);

                values = Graphics.GenerateDoubleCollection(yOffset, 0);
                frames = Graphics.GenerateDoubleCollection(0, 0.4);
                splines = AnimationHelper.GenerateKeySplineList
                    (
                        new Point(0, 0), new Point(1, 1),
                        new Point(0, 0), new Point(0, 1)
                    );

                DoubleAnimationUsingKeyFrames sliceYAnimation = CreateDoubleAnimation(currentDataSeries, dataPoint, translateTransform, "(TranslateTransform.Y)", 0, frames, values, splines);

                storyboard.Children.Add(sliceXAnimation);
                storyboard.Children.Add(sliceYAnimation);


                PathFigure figure = (labelLine.Data as PathGeometry).Figures[0];
                PathSegmentCollection segments = figure.Segments;
                storyboard = CreateLabelLineInteractivityAnimation(currentDataSeries, dataPoint, storyboard, segments[0], explodedPoints.LabelLineMidPoint, unExplodedPoints.LabelLineMidPoint);
                storyboard = CreateLabelLineInteractivityAnimation(currentDataSeries, dataPoint, storyboard, segments[1], explodedPoints.LabelLineEndPoint, unExplodedPoints.LabelLineEndPoint);
            }
            #endregion Animating Label Line

            return storyboard;
        }

        /// <summary>
        /// Create3DPie
        /// </summary>
        /// <param name="width">Visual width</param>
        /// <param name="height">Visual height</param>
        /// <param name="series">DataSeries</param>
        /// <param name="enabledDataPoints"> Enabled InternalDataPoints in the DataSeries</param>
        /// <param name="dataPoint">DataPoint reference</param>
        /// <param name="visual">visual canvas reference</param>
        /// <param name="faces">Visual faces</param>
        /// <param name="pieParams">Pie parameters</param>
        /// <param name="offsetX">X offset</param>
        /// <param name="zindex">Z index of the pie</param>
        /// <param name="isAnimationEnabled">Whether animation is enabled</param>
        private static void Create3DPie(DataSeries currentDataSeries, Double widthOfPlotArea, Double height, DataSeries series, List<DataPoint> enabledDataPoints, DataPoint dataPoint, ref Canvas visual, ref Faces faces, ref SectorChartShapeParams pieParams, ref Double offsetX, ref Int32 zindex, Boolean isAnimationEnabled)
        {
            #region 3D Pie

            PieDoughnut3DPoints unExplodedPoints = new PieDoughnut3DPoints();
            PieDoughnut3DPoints explodedPoints = new PieDoughnut3DPoints();
            pieParams.TagReference = dataPoint;

            List<Shape> pieFaces = GetPie3D(currentDataSeries, ref faces, pieParams, ref zindex, ref unExplodedPoints, ref explodedPoints, ref dataPoint.LabelLine, enabledDataPoints);

            foreach (Shape path in pieFaces)
            {
                if (path != null)
                {
                    visual.Children.Add(path);
                    faces.VisualComponents.Add(path);
                    faces.BorderElements.Add(path);
                    path.RenderTransform = new TranslateTransform();

                    // apply animation to the 3D sections
                    if (isAnimationEnabled)
                    {
                        series.Storyboard = CreateOpacityAnimation(currentDataSeries, dataPoint, series.Storyboard, path, 1.0 / (series.InternalDataPoints.Count) * (series.InternalDataPoints.IndexOf(dataPoint)), dataPoint.InternalOpacity, 0.5);
                        path.Opacity = 0;
                    }
                }
            }

            if (dataPoint.LabelLine != null && pieParams.LabelLineEnabled)
            {
                dataPoint.LabelLine.RenderTransform = new TranslateTransform();

                if (dataPoint.InternalYValue == 0)
                {
                    Line zeroLine = new Line();
                    zeroLine.X1 = pieParams.Center.X;
                    zeroLine.Y1 = pieParams.Center.Y;

                    zeroLine.X2 = unExplodedPoints.LabelLineStartPoint.X;
                    zeroLine.Y2 = unExplodedPoints.LabelLineStartPoint.Y;
                    zeroLine.Stroke = pieParams.LabelLineColor;
                    zeroLine.StrokeThickness = 0.25;
                    zeroLine.IsHitTestVisible = false;
                    visual.Children.Add(zeroLine);

                    if (isAnimationEnabled)
                    {
                        series.Storyboard = CreateOpacityAnimation(currentDataSeries, dataPoint, series.Storyboard, zeroLine, 2, zeroLine.Opacity, 0.5);
                        zeroLine.Opacity = 0;
                    }
                }

                visual.Children.Add(dataPoint.LabelLine);
                faces.VisualComponents.Add(dataPoint.LabelLine);
            }

            faces.Visual = visual;

            UpdateExplodedPosition(pieParams, dataPoint, offsetX, unExplodedPoints, explodedPoints, widthOfPlotArea);

            pieParams.ExplodedPoints = explodedPoints;
            pieParams.UnExplodedPoints = unExplodedPoints;

            if ((Boolean)(dataPoint.Chart as Chart).AnimatedUpdate)
            {
                dataPoint.ExplodeAnimation = new Storyboard();
                dataPoint.ExplodeAnimation = CreateExplodingOut3DAnimation(currentDataSeries, dataPoint, dataPoint.ExplodeAnimation, pieFaces, dataPoint.LabelVisual as Canvas, dataPoint.LabelLine, unExplodedPoints, explodedPoints, pieParams.OffsetX, pieParams.OffsetY);

                dataPoint.UnExplodeAnimation = new Storyboard();
                dataPoint.UnExplodeAnimation = CreateExplodingIn3DAnimation(currentDataSeries, dataPoint, dataPoint.UnExplodeAnimation, pieFaces, dataPoint.LabelVisual as Canvas, dataPoint.LabelLine, unExplodedPoints, explodedPoints, pieParams.OffsetX, pieParams.OffsetY);
            }
            else
            {
                if (!(dataPoint.Chart as Chart).AnimationEnabled)
                {
                    if ((Boolean)dataPoint.Exploded)
                    {
                        foreach (Shape path in pieFaces)
                        {
                            if (path == null) continue;
                            (path.RenderTransform as TranslateTransform).X = pieParams.OffsetX;
                            (path.RenderTransform as TranslateTransform).Y = pieParams.OffsetY;
                        }

                        if (dataPoint.LabelVisual != null)
                        {
                            if (dataPoint.LabelStyle == LabelStyles.Inside)
                            {
                                TranslateTransform translateTransform = new TranslateTransform();
                                (dataPoint.LabelVisual as Canvas).RenderTransform = translateTransform;

                                translateTransform.X = pieParams.OffsetX;
                                translateTransform.Y = pieParams.OffsetY;
                            }
                            else
                            {
                                (dataPoint.LabelVisual as Canvas).SetValue(Canvas.LeftProperty, explodedPoints.LabelPosition.X);
                            }
                        }

                        if (dataPoint.LabelLine != null)
                        {
                            (dataPoint.LabelLine.RenderTransform as TranslateTransform).X = pieParams.OffsetX;
                            (dataPoint.LabelLine.RenderTransform as TranslateTransform).Y = pieParams.OffsetY;

                            PathFigure figure = (dataPoint.LabelLine.Data as PathGeometry).Figures[0];
                            PathSegmentCollection segments = figure.Segments;
                            (segments[0] as LineSegment).Point = explodedPoints.LabelLineMidPoint;
                            (segments[1] as LineSegment).Point = explodedPoints.LabelLineEndPoint;
                        }

                        dataPoint._interactiveExplodeState = true;
                    }
                }
            }
            #endregion
        }

        /// <summary>
        /// Create3DPie
        /// </summary>
        /// <param name="width">Visual width</param>
        /// <param name="height">Visual height</param>
        /// <param name="series">DataSeries</param>
        /// <param name="enabledDataPoints"> Enabled InternalDataPoints in the DataSeries</param>
        /// <param name="dataPoint">DataPoint reference</param>
        /// <param name="visual">visual canvas reference</param>
        /// <param name="faces">Visual faces</param>
        /// <param name="pieParams">Pie parameters</param>
        /// <param name="offsetX">X offset</param>
        /// <param name="offsetX">Y offset</param>
        /// <param name="zindex">Z index of the pie</param>
        /// <param name="isAnimationEnabled">Whether animation is enabled</param>
        /// <param name="labelStyleCounter">labelStyle count</param>
        private static void Create2DPie(DataSeries currentDataSeries, Double widthOfPlotArea, Double height, DataSeries series, List<DataPoint> enabledDataPoints, DataPoint dataPoint, ref Canvas visual, ref Faces faces, ref SectorChartShapeParams pieParams, ref Double offsetX, ref Double offsetY, ref Int32 zindex, Boolean isAnimationEnabled, Int32 labelStateCount)
        {
            #region 2D Pie

            PieDoughnut2DPoints unExplodedPoints = new PieDoughnut2DPoints();
            PieDoughnut2DPoints explodedPoints = new PieDoughnut2DPoints();

            pieParams.TagReference = dataPoint;

            if (labelStateCount == enabledDataPoints.Count)
                pieParams.OuterRadius -= pieParams.OuterRadius * pieParams.ExplodeRatio;

            Canvas pieVisual = GetPie2D(currentDataSeries, ref faces, pieParams, ref unExplodedPoints, ref explodedPoints, ref dataPoint.LabelLine, enabledDataPoints);

            UpdateExplodedPosition(pieParams, dataPoint, offsetX, unExplodedPoints, explodedPoints, widthOfPlotArea);

            pieParams.ExplodedPoints = explodedPoints;
            pieParams.UnExplodedPoints = unExplodedPoints;

            TranslateTransform translateTransform = new TranslateTransform();
            pieVisual.RenderTransform = translateTransform;

            if ((Boolean)(currentDataSeries.Chart as Chart).AnimatedUpdate)
            {
                dataPoint.ExplodeAnimation = new Storyboard();
                dataPoint.ExplodeAnimation = CreateExplodingOut2DAnimation(currentDataSeries, dataPoint, dataPoint.ExplodeAnimation, pieVisual, dataPoint.LabelVisual as Canvas, dataPoint.LabelLine, translateTransform, unExplodedPoints, explodedPoints, offsetX, offsetY);
                dataPoint.UnExplodeAnimation = new Storyboard();
                dataPoint.UnExplodeAnimation = CreateExplodingIn2DAnimation(currentDataSeries, dataPoint, dataPoint.UnExplodeAnimation, pieVisual, dataPoint.LabelVisual as Canvas, dataPoint.LabelLine, translateTransform, unExplodedPoints, explodedPoints, offsetX, offsetY);
            }
            else
            {
                if (!(currentDataSeries.Chart as Chart).AnimationEnabled)
                {
                    if ((Boolean)dataPoint.Exploded)
                    {
                        translateTransform.X = offsetX;
                        translateTransform.Y = offsetY;

                        if (dataPoint.LabelVisual != null)
                        {
                            if (dataPoint.LabelStyle == LabelStyles.Inside)
                            {
                                translateTransform = new TranslateTransform();
                                (dataPoint.LabelVisual as Canvas).RenderTransform = translateTransform;

                                translateTransform.X = offsetX;
                                translateTransform.Y = offsetY;
                            }
                            else
                            {
                                (dataPoint.LabelVisual as Canvas).SetValue(Canvas.LeftProperty, explodedPoints.LabelPosition.X);
                            }
                        }

                        if (dataPoint.LabelLine != null)
                        {
                            PathFigure figure = (dataPoint.LabelLine.Data as PathGeometry).Figures[0];
                            PathSegmentCollection segments = figure.Segments;
                            (segments[0] as LineSegment).Point = explodedPoints.LabelLineMidPoint;
                            (segments[1] as LineSegment).Point = explodedPoints.LabelLineEndPoint;
                        }

                        dataPoint._interactiveExplodeState = true;
                    }
                }
            }

            pieVisual.SetValue(Canvas.TopProperty, height / 2 - pieVisual.Height / 2);
            pieVisual.SetValue(Canvas.LeftProperty, widthOfPlotArea / 2 - pieVisual.Width / 2);
            visual.Children.Add(pieVisual);
            faces.VisualComponents.Add(pieVisual);
            faces.Visual = pieVisual;

            #endregion
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Returns lighter bevel brush
        /// </summary>
        /// <param name="brush">Brush</param>
        /// <param name="angle">Angle as Double</param>
        /// <returns>Brush</returns>
        internal static Brush GetLighterBevelBrush(Brush brush, Double angle)
        {
            return Graphics.GetBevelTopBrush(brush, angle);
        }

        /// <summary>
        /// Returns darker bevel brush
        /// </summary>
        /// <param name="brush">Brush</param>
        /// <param name="angle">Angle as Double</param>
        /// <returns>Brush</returns>
        internal static Brush GetDarkerBevelBrush(Brush brush, Double angle)
        {
            return Graphics.GetLightingEnabledBrush(brush, "Linear", new Double[] { 0.35, 0.65 });
        }

        /// <summary>
        /// Returns curved bevel brush
        /// </summary>
        /// <param name="brush">Brush</param>
        /// <param name="angle">Angle as Double</param>
        /// <param name="shade">Shade as DoubleCollection</param>
        /// <param name="offset">Offset as DoubleCollection</param>
        /// <returns>Brush</returns>
        internal static Brush GetCurvedBevelBrush(Brush brush, Double angle, DoubleCollection shade, DoubleCollection offset)
        {
            if (typeof(SolidColorBrush).Equals(brush.GetType()))
            {
                SolidColorBrush solidBrush = brush as SolidColorBrush;
                List<Color> colors = new List<Color>();
                List<Double> stops = new List<double>();

                for (Int32 i = 0; i < shade.Count; i++)
                {
                    Color newShade = (shade[i] < 0 ? Graphics.GetDarkerColor(solidBrush.Color, Math.Abs(shade[i])) : Graphics.GetLighterColor(solidBrush.Color, Math.Abs(shade[i])));
                    colors.Add(newShade);
                }
                for (Int32 i = 0; i < offset.Count; i++)
                {
                    stops.Add(offset[i]);
                }

                return Graphics.CreateLinearGradientBrush(angle, new Point(0, 0.5), new Point(1, 0.5), colors, stops);
            }
            else
            {
                return brush;
            }
        }

        /// <summary>
        /// Return visual object for pie chart
        /// </summary>
        /// <param name="width">Width of the PlotArea</param>
        /// <param name="height">Height of the PlotArea</param>
        /// <param name="plotDetails">PlotDetails reference</param>
        /// <param name="seriesList">List of series list</param>
        /// <param name="chart">Chart reference</param>
        /// <param name="animationEnabled">Whether animation is enabled</param>
        /// <returns>Canvas</returns>
        internal static Canvas GetVisualObjectForPieChart(Double width, Double height, PlotDetails plotDetails, List<DataSeries> seriesList, Chart chart, bool isAnimationEnabled)
        {
            if (Double.IsNaN(width) || Double.IsNaN(height) || width <= 0 || height <= 0) return null;

            DataSeries currentDataSeries = null;

            // Debug.WriteLine("PieStart: " + DateTime.Now.ToLongTimeString());

            Canvas visual = new Canvas() { Width = width, Height = height };
            DataSeries series = seriesList[0];
            series.Faces = null;

            if (series.Enabled == false)
                return visual;

            //  List<DataPoint> enabledDataPoints = (from datapoint in series.InternalDataPoints where datapoint.Enabled == true && datapoint.InternalYValue != 0 && !Double.IsNaN(datapoint.InternalYValue) select datapoint).ToList();

            List<DataPoint> enabledDataPoints = (from datapoint in series.InternalDataPoints where datapoint.Enabled == true && !Double.IsNaN(datapoint.InternalYValue) select datapoint).ToList();

            if ((from dp in enabledDataPoints select dp.InternalYValue).Sum() == 0)
                enabledDataPoints.Clear();

            Double absoluteSum = plotDetails.GetAbsoluteSumOfDataPoints(enabledDataPoints);
            absoluteSum = (absoluteSum == 0) ? 1 : absoluteSum;

            Double centerX = width / 2;
            Double centerY = height / 2;
            Double offsetX = 0;
            Double offsetY = 0;

            Boolean IsLabelEnabled;
            Size pieSize = new Size();

            Canvas labelCanvas = CreateAndPositionLabels(absoluteSum, enabledDataPoints, width, height, ((chart.View3D) ? 0.4 : 1), chart.View3D, ref pieSize);

            Debug.WriteLine("Labels Positioning over: " + DateTime.Now.ToLongTimeString());

            if (labelCanvas == null)
                IsLabelEnabled = false;
            else
            {
                IsLabelEnabled = true;
                labelCanvas.SetValue(Canvas.ZIndexProperty, 50001);
            }

            Double radius = Math.Min(pieSize.Width, pieSize.Height) / (chart.View3D ? 1 : 2);
            Double startAngle = series.InternalStartAngle;
            Double endAngle = 0;
            Double angle;
            Double absoluteYValue;
            Double meanAngle = 0;
            Int32 zindex = 0;

            if (chart.View3D)
                _elementPositionData = new List<ElementPositionData>();

            if (series.Storyboard == null)
                series.Storyboard = new Storyboard();

            currentDataSeries = series;

            SectorChartShapeParams pieParams = null;

            Int32 labelStateCounter = 0;

            if (!chart.View3D)
            {
                foreach (DataPoint dataPoint in enabledDataPoints)
                {
                    if (dataPoint.LabelStyle == LabelStyles.Inside || !(Boolean)dataPoint.LabelEnabled)
                        labelStateCounter++;
                }
            }

            foreach (DataPoint dataPoint in enabledDataPoints)
            {
                if (Double.IsNaN(dataPoint.InternalYValue))// || dataPoint.InternalYValue == 0)
                    continue;

                absoluteYValue = Math.Abs(dataPoint.InternalYValue);

                angle = (absoluteYValue / absoluteSum) * Math.PI * 2;

                endAngle = startAngle + angle;
                meanAngle = (startAngle + endAngle) / 2;

                pieParams = new SectorChartShapeParams();

                dataPoint.VisualParams = pieParams;

                pieParams.Storyboard = series.Storyboard;
                pieParams.AnimationEnabled = isAnimationEnabled;
                pieParams.Center = new Point(centerX, centerY);
                pieParams.ExplodeRatio = chart.View3D ? 0.2 : 0.1;
                pieParams.InnerRadius = 0;
                pieParams.OuterRadius = radius;
                pieParams.DataPoint = dataPoint;

                if (chart.View3D)
                {
                    pieParams.StartAngle = pieParams.FixAngle((startAngle) % (Math.PI * 2));
                    pieParams.StopAngle = pieParams.FixAngle((endAngle) % (Math.PI * 2));
                }
                else
                {
                    pieParams.StartAngle = startAngle;
                    pieParams.StopAngle = endAngle;
                }

                pieParams.Lighting = (Boolean)dataPoint.LightingEnabled;
                pieParams.Bevel = series.Bevel;
                pieParams.IsLargerArc = (angle / (Math.PI)) > 1;
                pieParams.Background = dataPoint.Color;
                pieParams.Width = width;
                pieParams.Height = height;
                pieParams.TiltAngle = Math.Asin(0.4);
                pieParams.Depth = 20 / pieParams.YAxisScaling;

                pieParams.MeanAngle = meanAngle;
                pieParams.LabelLineEnabled = (Boolean)dataPoint.LabelLineEnabled;
                pieParams.LabelLineColor = dataPoint.LabelLineColor;
                pieParams.LabelLineThickness = (Double)dataPoint.LabelLineThickness;
                pieParams.LabelLineStyle = ExtendedGraphics.GetDashArray((LineStyles)dataPoint.LabelLineStyle);
                pieParams.IsZero = (dataPoint.InternalYValue == 0);

                offsetX = radius * pieParams.ExplodeRatio * Math.Cos(meanAngle);
                offsetY = radius * pieParams.ExplodeRatio * Math.Sin(meanAngle);
                pieParams.OffsetX = offsetX;
                pieParams.OffsetY = offsetY * (chart.View3D ? pieParams.YAxisScaling : 1);

                if (dataPoint.LabelVisual != null)
                {
                    if (dataPoint.LabelVisual.Visibility == Visibility.Collapsed)
                        pieParams.LabelLineEnabled = false;

                    Double left = (Double)dataPoint.LabelVisual.GetValue(Canvas.LeftProperty);

                    if (left < width / 2)
                    {
                        pieParams.LabelLineTargetToRight = true;
                        // pieParams.LabelPoint = new Point((Double)dataPoint.LabelVisual.GetValue(Canvas.LeftProperty) + dataPoint.LabelVisual.DesiredSize.Width, (Double)dataPoint.LabelVisual.GetValue(Canvas.TopProperty) + dataPoint.LabelVisual.DesiredSize.Height / 2);
                        pieParams.LabelPoint = new Point(left + dataPoint.LabelVisual.Width + LabelPlacementHelper.LABEL_LINE_GAP, (Double)dataPoint.LabelVisual.GetValue(Canvas.TopProperty) + dataPoint.LabelVisual.Height / 2);
                    }
                    else
                    {
                        pieParams.LabelLineTargetToRight = false;
                        pieParams.LabelPoint = new Point(left - LabelPlacementHelper.LABEL_LINE_GAP, (Double)dataPoint.LabelVisual.GetValue(Canvas.TopProperty) + dataPoint.LabelVisual.Height / 2);
                    }

                    // apply animation to the labels
                    if (isAnimationEnabled)
                    {
                        series.Storyboard = CreateOpacityAnimation(currentDataSeries, dataPoint, series.Storyboard, dataPoint.LabelVisual, 2, (Double)dataPoint.InternalOpacity * (Double)dataPoint.Parent.InternalOpacity, 0.5);
                        dataPoint.LabelVisual.Opacity = 0;
                    }
                }

                if (dataPoint.LabelStyle == LabelStyles.Inside && dataPoint.InternalYValue == 0)
                    pieParams.LabelLineEnabled = false;

                Faces faces = new Faces();

                if (chart.View3D)
                {
                    Create3DPie(currentDataSeries, width, height, series, enabledDataPoints, dataPoint, ref visual, ref faces, ref pieParams, ref offsetX, ref zindex, isAnimationEnabled);
                }
                else
                {
                    Create2DPie(currentDataSeries, width, height, series, enabledDataPoints, dataPoint, ref visual, ref faces, ref pieParams, ref offsetX, ref offsetY, ref zindex, isAnimationEnabled, labelStateCounter);
                }

                Debug.WriteLine("Datapoint" + enabledDataPoints.IndexOf(dataPoint) + ": " + DateTime.Now.ToLongTimeString());

                dataPoint.Faces = faces;

                startAngle = endAngle;

                if (!chart.AnimationEnabled || chart.IsInDesignMode || !chart.ChartArea._isFirstTimeRender)
                {
                    if (dataPoint.Faces != null)
                    {
                        foreach (Shape shape in dataPoint.Faces.BorderElements)
                        {
                            InteractivityHelper.ApplyBorderEffect(shape, (BorderStyles)dataPoint.BorderStyle, dataPoint.InternalBorderThickness.Left, dataPoint.BorderColor);
                        }
                    }
                }
            }

            if (chart.View3D)
            {
                Int32 zindex1, zindex2;

                _elementPositionData.Sort(ElementPositionData.CompareAngle);
                zindex1 = 1000;
                zindex2 = -1000;

                for (Int32 i = 0; i < _elementPositionData.Count; i++)
                {
                    SetZIndex(_elementPositionData[i].Element, ref zindex1, ref zindex2, _elementPositionData[i].StartAngle);
                }
            }

            if (IsLabelEnabled && labelCanvas != null)
                visual.Children.Add(labelCanvas);

            RectangleGeometry clipRectangle = new RectangleGeometry();
            clipRectangle.Rect = new Rect(0, 0, width, height);
            visual.Clip = clipRectangle;

            return visual;
        }

        internal static void UpdateExplodedPosition(SectorChartShapeParams pieParams, DataPoint dataPoint, Double offsetX, PieDoughnutPoints unExplodedPoints,
            PieDoughnutPoints explodedPoints, Double widthOfPlotArea)
        {

            if (dataPoint.LabelVisual != null)
            {
                unExplodedPoints.LabelPosition = new Point((Double)dataPoint.LabelVisual.GetValue(Canvas.LeftProperty), (Double)dataPoint.LabelVisual.GetValue(Canvas.TopProperty));
                Double left = (Double)dataPoint.LabelVisual.GetValue(Canvas.LeftProperty);

                if (left < widthOfPlotArea / 2)
                {   
                    if (unExplodedPoints.LabelPosition.X + offsetX < 0)
                    {
                        explodedPoints.LabelPosition = new Point(unExplodedPoints.LabelPosition.X, unExplodedPoints.LabelPosition.Y);
                        explodedPoints.LabelLineEndPoint = new Point(explodedPoints.LabelLineEndPoint.X - offsetX, explodedPoints.LabelLineEndPoint.Y);
                        explodedPoints.LabelLineMidPoint = new Point(explodedPoints.LabelLineMidPoint.X - offsetX, explodedPoints.LabelLineMidPoint.Y);
                    }
                    else
                        explodedPoints.LabelPosition = new Point(unExplodedPoints.LabelPosition.X + offsetX, unExplodedPoints.LabelPosition.Y);
                }
                else
                {
                    if (unExplodedPoints.LabelPosition.X + offsetX + dataPoint.LabelVisual.Width > widthOfPlotArea)
                    {
                        explodedPoints.LabelPosition = new Point(unExplodedPoints.LabelPosition.X, unExplodedPoints.LabelPosition.Y);
                        explodedPoints.LabelLineEndPoint = new Point(explodedPoints.LabelLineEndPoint.X - offsetX, explodedPoints.LabelLineEndPoint.Y);
                        explodedPoints.LabelLineMidPoint = new Point(explodedPoints.LabelLineMidPoint.X - offsetX, explodedPoints.LabelLineMidPoint.Y);
                    }
                    else
                        explodedPoints.LabelPosition = new Point(unExplodedPoints.LabelPosition.X + offsetX, unExplodedPoints.LabelPosition.Y);
                }
            }
        }


        /// <summary>
        /// Return visual object for doughnut chart
        /// </summary>
        /// <param name="width">Width of the PlotArea</param>
        /// <param name="height">Height of the PlotArea</param>
        /// <param name="plotDetails">PlotDetails reference</param>
        /// <param name="seriesList">List of series list</param>
        /// <param name="chart">Chart reference</param>
        /// <param name="animationEnabled">Whether animation is enabled</param>
        /// <returns>Canvas</returns>
        internal static Canvas GetVisualObjectForDoughnutChart(Double widthOfPlotArea, Double height, PlotDetails plotDetails, List<DataSeries> seriesList, Chart chart, bool animationEnabled)
        {
            if (Double.IsNaN(widthOfPlotArea) || Double.IsNaN(height) || widthOfPlotArea <= 0 || height <= 0) return null;

            DataSeries currentDataSeries = null;

            Canvas visual = new Canvas();
            visual.Width = widthOfPlotArea;
            visual.Height = height;

            DataSeries series = seriesList[0];
            series.Faces = null;

            if (series.Enabled == false)
                return visual;

            // List<DataPoint> enabledDataPoints = (from datapoint in series.InternalDataPoints where datapoint.Enabled == true && datapoint.InternalYValue != 0 && !Double.IsNaN(datapoint.InternalYValue) select datapoint).ToList();
            List<DataPoint> enabledDataPoints = (from datapoint in series.InternalDataPoints where datapoint.Enabled == true && !Double.IsNaN(datapoint.InternalYValue) select datapoint).ToList();

            if ((from dp in enabledDataPoints select dp.InternalYValue).Sum() == 0)
                enabledDataPoints.Clear();

            Double absoluteSum = plotDetails.GetAbsoluteSumOfDataPoints(enabledDataPoints);

            absoluteSum = (absoluteSum == 0) ? 1 : absoluteSum;

            Double centerX = widthOfPlotArea / 2;
            Double centerY = height / 2;

            Double offsetX = 0;
            Double offsetY = 0;

            Size pieCanvas = new Size();
            Canvas labelCanvas = CreateAndPositionLabels(absoluteSum, enabledDataPoints, widthOfPlotArea, height, ((chart.View3D) ? 0.4 : 1), chart.View3D, ref pieCanvas);

            Double radius = Math.Min(pieCanvas.Width, pieCanvas.Height) / (chart.View3D ? 1 : 2);
            Double startAngle = series.InternalStartAngle;
            Double endAngle = 0;
            Double angle;
            Double meanAngle;
            Double absoluteYValue;
            Double radiusDiff = 0;

            var explodedDataPoints = (from datapoint in series.InternalDataPoints where datapoint.Exploded == true && datapoint.InternalYValue != 0 select datapoint);
            radiusDiff = (explodedDataPoints.Count() > 0) ? radius * 0.3 : 0;

            if (chart.View3D)
            {
                _elementPositionData = new List<ElementPositionData>();
            }

            if (labelCanvas != null)
            {
                labelCanvas.SetValue(Canvas.ZIndexProperty, 50001);
            }

            if (series.Storyboard == null)
                series.Storyboard = new Storyboard();

            currentDataSeries = series;

            SectorChartShapeParams doughnutParams = null;

            Int32 labelStateCounter = 0;

            if (!chart.View3D)
            {
                foreach (DataPoint dataPoint in enabledDataPoints)
                {
                    if (dataPoint.LabelStyle == LabelStyles.Inside || !(Boolean)dataPoint.LabelEnabled)
                        labelStateCounter++;
                }
            }

            foreach (DataPoint dataPoint in enabledDataPoints)
            {
                if (Double.IsNaN(dataPoint.InternalYValue))// || dataPoint.InternalYValue == 0)
                    continue;

                absoluteYValue = Math.Abs(dataPoint.InternalYValue);

                angle = (absoluteYValue / absoluteSum) * Math.PI * 2;

                endAngle = startAngle + angle;
                meanAngle = (startAngle + endAngle) / 2;

                doughnutParams = new SectorChartShapeParams();
                dataPoint.VisualParams = doughnutParams;

                doughnutParams.AnimationEnabled = animationEnabled;
                doughnutParams.Storyboard = series.Storyboard;
                doughnutParams.ExplodeRatio = chart.View3D ? 0.2 : 0.1;
                doughnutParams.Center = new Point(centerX, centerY);
                doughnutParams.DataPoint = dataPoint;
                doughnutParams.InnerRadius = radius / 2;
                doughnutParams.OuterRadius = radius;

                if (chart.View3D)
                {
                    doughnutParams.StartAngle = doughnutParams.FixAngle((startAngle) % (Math.PI * 2));
                    doughnutParams.StopAngle = doughnutParams.FixAngle((endAngle) % (Math.PI * 2));
                }
                else
                {
                    doughnutParams.StartAngle = startAngle;
                    doughnutParams.StopAngle = endAngle;
                }

                doughnutParams.Lighting = (Boolean)dataPoint.LightingEnabled;
                doughnutParams.Bevel = series.Bevel;
                doughnutParams.IsLargerArc = (angle / (Math.PI)) > 1;
                doughnutParams.Background = dataPoint.Color;
                doughnutParams.Width = widthOfPlotArea;
                doughnutParams.Height = height;
                doughnutParams.TiltAngle = Math.Asin(0.4);
                doughnutParams.Depth = 20 / doughnutParams.YAxisScaling;

                doughnutParams.MeanAngle = meanAngle;
                doughnutParams.LabelLineEnabled = (Boolean)dataPoint.LabelLineEnabled;
                doughnutParams.LabelLineColor = dataPoint.LabelLineColor;
                doughnutParams.LabelLineThickness = (Double)dataPoint.LabelLineThickness;
                doughnutParams.LabelLineStyle = ExtendedGraphics.GetDashArray((LineStyles)dataPoint.LabelLineStyle);

                offsetX = radius * doughnutParams.ExplodeRatio * Math.Cos(meanAngle);
                offsetY = radius * doughnutParams.ExplodeRatio * Math.Sin(meanAngle);
                doughnutParams.OffsetX = offsetX;
                doughnutParams.OffsetY = offsetY * (chart.View3D ? doughnutParams.YAxisScaling : 1);

                if (dataPoint.LabelVisual != null)
                {
                    if (dataPoint.LabelVisual.Visibility == Visibility.Collapsed)
                        doughnutParams.LabelLineEnabled = false;

                    Double left = (Double)dataPoint.LabelVisual.GetValue(Canvas.LeftProperty);

                    if (left < widthOfPlotArea / 2)
                    {
                        doughnutParams.LabelLineTargetToRight = true;
                        // pieParams.LabelPoint = new Point((Double)dataPoint.LabelVisual.GetValue(Canvas.LeftProperty) + dataPoint.LabelVisual.DesiredSize.Width, (Double)dataPoint.LabelVisual.GetValue(Canvas.TopProperty) + dataPoint.LabelVisual.DesiredSize.Height / 2);
                        doughnutParams.LabelPoint = new Point(left + dataPoint.LabelVisual.Width + LabelPlacementHelper.LABEL_LINE_GAP, (Double)dataPoint.LabelVisual.GetValue(Canvas.TopProperty) + dataPoint.LabelVisual.Height / 2);
                    }
                    else
                    {
                        doughnutParams.LabelLineTargetToRight = false;
                        doughnutParams.LabelPoint = new Point(left - LabelPlacementHelper.LABEL_LINE_GAP, (Double)dataPoint.LabelVisual.GetValue(Canvas.TopProperty) + dataPoint.LabelVisual.Height / 2);
                    }

                    //if ((Double)dataPoint.LabelVisual.GetValue(Canvas.LeftProperty) < width / 2)
                    //{
                    //    doughnutParams.LabelPoint = new Point((Double)dataPoint.LabelVisual.GetValue(Canvas.LeftProperty) + dataPoint.LabelVisual.Width + LABEL_LINE_GAP, (Double)dataPoint.LabelVisual.GetValue(Canvas.TopProperty) + dataPoint.LabelVisual.Height / 2);
                    //}
                    //else
                    //{
                    //    doughnutParams.LabelPoint = new Point((Double)dataPoint.LabelVisual.GetValue(Canvas.LeftProperty) - LABEL_LINE_GAP, (Double)dataPoint.LabelVisual.GetValue(Canvas.TopProperty) + dataPoint.LabelVisual.Height / 2);
                    //}

                    // apply animation to the labels
                    if (animationEnabled)
                    {
                        series.Storyboard = CreateOpacityAnimation(currentDataSeries, doughnutParams.DataPoint, series.Storyboard, dataPoint.LabelVisual, 2, 1, 0.5);
                        dataPoint.LabelVisual.Opacity = 0;
                    }

                   
                }

                if (dataPoint.LabelStyle == LabelStyles.Inside && dataPoint.InternalYValue == 0)
                    doughnutParams.LabelLineEnabled = false;

                Faces faces = new Faces();
                faces.Parts = new List<DependencyObject>();

                doughnutParams.TagReference = dataPoint;

                if (chart.View3D)
                {
                    PieDoughnut3DPoints unExplodedPoints = new PieDoughnut3DPoints();
                    PieDoughnut3DPoints explodedPoints = new PieDoughnut3DPoints();
                    List<Shape> doughnutFaces = GetDoughnut3D(currentDataSeries, ref faces, doughnutParams, ref unExplodedPoints, ref explodedPoints, ref dataPoint.LabelLine, enabledDataPoints);

                    foreach (Shape path in doughnutFaces)
                    {   
                        if (path != null)
                        {   
                            visual.Children.Add(path);
                            faces.VisualComponents.Add(path);
                            faces.BorderElements.Add(path);

                            path.RenderTransform = new TranslateTransform();

                            // apply animation to the 3D sections
                            if (animationEnabled)
                            {
                                series.Storyboard = CreateOpacityAnimation(currentDataSeries, doughnutParams.DataPoint, series.Storyboard, path, 1.0 / (series.InternalDataPoints.Count) * (series.InternalDataPoints.IndexOf(dataPoint)), dataPoint.InternalOpacity, 0.5);
                                path.Opacity = 0;
                            }
                        }
                    }
                    if (dataPoint.LabelLine != null && doughnutParams.LabelLineEnabled)
                    {
                        dataPoint.LabelLine.RenderTransform = new TranslateTransform();
                        visual.Children.Add(dataPoint.LabelLine);

                        if (dataPoint.InternalYValue == 0)
                        {
                            Double yOffset = doughnutParams.YAxisScaling;
                            Line zeroLine = new Line();
                            zeroLine.X1 = doughnutParams.Center.X + doughnutParams.InnerRadius * Math.Cos(doughnutParams.MeanAngle);
                            zeroLine.Y1 = doughnutParams.Center.Y + doughnutParams.InnerRadius * Math.Sin(doughnutParams.MeanAngle);
                            zeroLine.Y1 -= offsetY;
                            zeroLine.Y1 += doughnutParams.Depth / 2 * doughnutParams.ZAxisScaling;
                            zeroLine.X2 = unExplodedPoints.LabelLineStartPoint.X;
                            zeroLine.Y2 = unExplodedPoints.LabelLineStartPoint.Y;
                            zeroLine.Stroke = doughnutParams.LabelLineColor;
                            zeroLine.StrokeThickness = 0.25;
                            zeroLine.IsHitTestVisible = false;
                            visual.Children.Add(zeroLine);

                            if (animationEnabled)
                            {
                                series.Storyboard = CreateOpacityAnimation(currentDataSeries, doughnutParams.DataPoint, series.Storyboard, zeroLine, 2, zeroLine.Opacity, 0.5);
                                zeroLine.Opacity = 0;
                            }
                        }

                        faces.VisualComponents.Add(dataPoint.LabelLine);
                    }

                    faces.Visual = visual;

                    UpdateExplodedPosition(doughnutParams, dataPoint, offsetX, unExplodedPoints, explodedPoints, widthOfPlotArea);

                    doughnutParams.ExplodedPoints = explodedPoints;
                    doughnutParams.UnExplodedPoints = unExplodedPoints;

                    ///------------------------
                    if ((Boolean)chart.AnimatedUpdate)
                    {
                        dataPoint.ExplodeAnimation = new Storyboard();
                        dataPoint.ExplodeAnimation = CreateExplodingOut3DAnimation(currentDataSeries, dataPoint, dataPoint.ExplodeAnimation, doughnutFaces, dataPoint.LabelVisual as Canvas, dataPoint.LabelLine, unExplodedPoints, explodedPoints, doughnutParams.OffsetX, doughnutParams.OffsetY);
                        dataPoint.UnExplodeAnimation = new Storyboard();
                        dataPoint.UnExplodeAnimation = CreateExplodingIn3DAnimation(currentDataSeries, dataPoint, dataPoint.UnExplodeAnimation, doughnutFaces, dataPoint.LabelVisual as Canvas, dataPoint.LabelLine, unExplodedPoints, explodedPoints, doughnutParams.OffsetX, doughnutParams.OffsetY);
                    }
                    else
                    {
                        if (!chart.AnimationEnabled)
                        {
                            if ((Boolean)dataPoint.Exploded)
                            {
                                foreach (Shape path in doughnutFaces)
                                {
                                    if (path == null) continue;
                                    (path.RenderTransform as TranslateTransform).X = offsetX;
                                    (path.RenderTransform as TranslateTransform).Y = offsetY;
                                }

                                if (dataPoint.LabelVisual != null)
                                {
                                    if (dataPoint.LabelStyle == LabelStyles.Inside)
                                    {
                                        TranslateTransform translateTransform = new TranslateTransform();
                                        (dataPoint.LabelVisual as Canvas).RenderTransform = translateTransform;

                                        translateTransform.X = offsetX;
                                        translateTransform.Y = offsetY;
                                    }
                                    else
                                    {
                                        (dataPoint.LabelVisual as Canvas).SetValue(Canvas.LeftProperty, explodedPoints.LabelPosition.X);
                                    }
                                }

                                if (dataPoint.LabelLine != null)
                                {
                                    (dataPoint.LabelLine.RenderTransform as TranslateTransform).X = offsetX;
                                    (dataPoint.LabelLine.RenderTransform as TranslateTransform).Y = offsetY;

                                    PathFigure figure = (dataPoint.LabelLine.Data as PathGeometry).Figures[0];
                                    PathSegmentCollection segments = figure.Segments;
                                    (segments[0] as LineSegment).Point = explodedPoints.LabelLineMidPoint;
                                    (segments[1] as LineSegment).Point = explodedPoints.LabelLineEndPoint;
                                }

                                dataPoint._interactiveExplodeState = true;
                            }
                        }
                    }
                }
                else
                {
                    PieDoughnut2DPoints unExplodedPoints = new PieDoughnut2DPoints();
                    PieDoughnut2DPoints explodedPoints = new PieDoughnut2DPoints();

                    if (labelStateCounter == enabledDataPoints.Count)
                    {
                        doughnutParams.OuterRadius -= doughnutParams.OuterRadius * doughnutParams.ExplodeRatio;
                        doughnutParams.InnerRadius = doughnutParams.OuterRadius / 2;
                    }

                    Canvas pieVisual = GetDoughnut2D(currentDataSeries, ref faces, doughnutParams, ref unExplodedPoints, ref explodedPoints, ref dataPoint.LabelLine, enabledDataPoints);

                    UpdateExplodedPosition(doughnutParams, dataPoint, offsetX, unExplodedPoints, explodedPoints, widthOfPlotArea);

                    doughnutParams.ExplodedPoints = explodedPoints;
                    doughnutParams.UnExplodedPoints = unExplodedPoints;

                    TranslateTransform translateTransform = new TranslateTransform();
                    pieVisual.RenderTransform = translateTransform;

                    if ((Boolean)chart.AnimatedUpdate)
                    {
                        dataPoint.ExplodeAnimation = new Storyboard();
                        dataPoint.ExplodeAnimation = CreateExplodingOut2DAnimation(currentDataSeries, dataPoint, dataPoint.ExplodeAnimation, pieVisual, dataPoint.LabelVisual as Canvas, dataPoint.LabelLine, translateTransform, unExplodedPoints, explodedPoints, offsetX, offsetY);
                        dataPoint.UnExplodeAnimation = new Storyboard();
                        dataPoint.UnExplodeAnimation = CreateExplodingIn2DAnimation(currentDataSeries, dataPoint, dataPoint.UnExplodeAnimation, pieVisual, dataPoint.LabelVisual as Canvas, dataPoint.LabelLine, translateTransform, unExplodedPoints, explodedPoints, offsetX, offsetY);
                    }
                    else
                    {
                        if (!chart.AnimationEnabled)
                        {
                            if ((Boolean)dataPoint.Exploded)
                            {
                                translateTransform.X = offsetX;
                                translateTransform.Y = offsetY;

                                if (dataPoint.LabelVisual != null)
                                {
                                    if (dataPoint.LabelStyle == LabelStyles.Inside)
                                    {
                                        translateTransform = new TranslateTransform();
                                        (dataPoint.LabelVisual as Canvas).RenderTransform = translateTransform;

                                        translateTransform.X = offsetX;
                                        translateTransform.Y = offsetY;
                                    }
                                    else
                                    {
                                        (dataPoint.LabelVisual as Canvas).SetValue(Canvas.LeftProperty, explodedPoints.LabelPosition.X);
                                    }
                                }

                                if (dataPoint.LabelLine != null)
                                {
                                    PathFigure figure = (dataPoint.LabelLine.Data as PathGeometry).Figures[0];
                                    PathSegmentCollection segments = figure.Segments;
                                    (segments[0] as LineSegment).Point = explodedPoints.LabelLineMidPoint;
                                    (segments[1] as LineSegment).Point = explodedPoints.LabelLineEndPoint;
                                }

                                dataPoint._interactiveExplodeState = true;
                            }
                        }
                    }

                    pieVisual.SetValue(Canvas.TopProperty, height / 2 - pieVisual.Height / 2);
                    pieVisual.SetValue(Canvas.LeftProperty, widthOfPlotArea / 2 - pieVisual.Width / 2);
                    visual.Children.Add(pieVisual);
                    faces.VisualComponents.Add(pieVisual);
                    faces.Visual = pieVisual;
                }

                dataPoint.Faces = faces;

                startAngle = endAngle;

                if (!chart.AnimationEnabled || chart.IsInDesignMode || !chart.ChartArea._isFirstTimeRender)
                {
                    if (dataPoint.Faces != null)
                    {
                        foreach (Shape shape in dataPoint.Faces.BorderElements)
                        {
                            InteractivityHelper.ApplyBorderEffect(shape, (BorderStyles)dataPoint.BorderStyle, dataPoint.InternalBorderThickness.Left, dataPoint.BorderColor);
                        }
                    }
                }
            }

            if (chart.View3D)
            {
                Int32 zindex1, zindex2;
                _elementPositionData.Sort(ElementPositionData.CompareAngle);
                zindex1 = 1000;
                zindex2 = -1000;
                for (Int32 i = 0; i < _elementPositionData.Count; i++)
                {
                    SetZIndex(_elementPositionData[i].Element, ref zindex1, ref zindex2, _elementPositionData[i].StartAngle);
                }
            }

            if (labelCanvas != null)
                visual.Children.Add(labelCanvas);

            RectangleGeometry clipRectangle = new RectangleGeometry();
            clipRectangle.Rect = new Rect(0, 0, widthOfPlotArea, height);
            visual.Clip = clipRectangle;

            return visual;
        }

        #endregion

        #region Internal Events And Delegates

        #endregion

        #region Data

        public const Double LABEL_LINE_LENGTH = 18;
        
        /// <summary>
        /// Visfiire.Charts.PieChart.PathGeometryParams class
        /// </summary>
        internal class PathGeometryParams
        {
            #region Public Methods

            /// <summary>
            /// Initializes a new instance of the Visfiire.Charts.PieChart.PathGeometryParams class
            /// </summary>
            public PathGeometryParams(Point endPoint)
            {
                EndPoint = endPoint;
            }

            #endregion Public Methods

            #region Public Properties

            /// <summary>
            /// End point of the path geometry
            /// </summary>
            public Point EndPoint
            {
                get;
                set;
            }

            #endregion
        }

        /// <summary>
        /// Visfiire.Charts.PieChart.LineSegmentParams class
        /// </summary>
        internal class LineSegmentParams : PathGeometryParams
        {
            #region Public Methods

            /// <summary>
            /// Initializes a new instance of the Visfiire.Charts.PieChart.LineSegmentParams class
            /// </summary>
            public LineSegmentParams(Point endPoint)
                : base(endPoint)
            {
            }

            #endregion Public Methods
        }

        /// <summary>
        /// Visfiire.Charts.PieChart.PathGeometryParams class
        /// </summary>
        internal class ArcSegmentParams : PathGeometryParams
        {
            #region Public Methods

            /// <summary>
            /// Initializes a new instance of the Visfiire.Charts.PieChart.PathGeometryParams class
            /// </summary>
            /// <param name="size">Pie/Doughnut size</param>
            /// <param name="rotation">Rotation angle</param>
            /// <param name="isLargeArc">Whether IsLargeArc</param>
            /// <param name="sweepDirection">SweepDirection</param>
            /// <param name="endPoint">EndPoint as Point</param>
            public ArcSegmentParams(Size size, Double rotation, Boolean isLargeArc, SweepDirection sweepDirection, Point endPoint)
                : base(endPoint)
            {
                Size = size;
                RotationAngle = rotation;
                IsLargeArc = isLargeArc;
                SweepDirection = sweepDirection;
            }

            #endregion Public Methods

            #region Public Properties

            /// <summary>
            /// Size of the arc
            /// </summary>
            public Size Size
            {
                get;
                set;
            }

            /// <summary>
            /// Rotation angle
            /// </summary>
            public Double RotationAngle
            {
                get;
                set;
            }

            /// <summary>
            /// Whether it is a large arc
            /// </summary>
            public Boolean IsLargeArc
            {
                get;
                set;
            }

            /// <summary>
            /// SweepDirection
            /// </summary>
            public SweepDirection SweepDirection
            {
                get;
                set;
            }

            #endregion Public
        }

        /// <summary>
        /// Positional data for a point over for pie at the angle of mean angle
        /// </summary>
        internal class PostionData
        {
            /// <summary>
            /// Index of the point
            /// </summary>
            public Int32 Index;

            /// <summary>
            /// Y position
            /// </summary>
            public Double yPosition;

            /// <summary>
            /// X position
            /// </summary>
            public Double xPosition;

            /// <summary>
            /// Mean angle
            /// </summary>
            public Double MeanAngle;

            /// <summary>
            /// Height of the pie
            /// </summary>
            public Double Height;

            /// <summary>
            /// Width of the pie
            /// </summary>
            public Double Width;

            public static Int32 CompareYPosition(PostionData a, PostionData b)
            {
                return a.yPosition.CompareTo(b.yPosition);
            }
        }

        #endregion
    }
}
