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
    internal class ElementPositionData
    {
        #region Public Methods

        public ElementPositionData()
        {
        }

        public ElementPositionData(FrameworkElement element, Double angle1, Double angle2)
        {
            Element = element;
            StartAngle = angle1;
            StopAngle = angle2;
        }

        public ElementPositionData(ElementPositionData m)
        {
            Element = m.Element;
            StartAngle = m.StartAngle;
            StopAngle = m.StopAngle;
        }

        #endregion Public Methods

        #region Static Methods

        public static Int32 CompareAngle(ElementPositionData a, ElementPositionData b)
        {
            Double angle1 = (a.StartAngle + a.StopAngle) / 2;
            Double angle2 = (b.StartAngle + b.StopAngle) / 2;
            return angle1.CompareTo(angle2);
        }

        #endregion Static Methods

        #region Public Properties

        public FrameworkElement Element
        {
            get;
            set;
        }

        public Double StartAngle
        {
            get;
            set;
        }

        public Double StopAngle
        {
            get;
            set;
        }

        #endregion
    }

    internal class SectorChartShapeParams
    {
        private Double _startAngle;
        private Double _stopAngle;
        private Double FixAngle(Double angle)
        {
            while (angle > Math.PI * 2) angle -= Math.PI;
            while (angle < 0) angle += Math.PI;
            return angle;
        }
        internal Double OuterRadius { get; set; }
        internal Double InnerRadius { get; set; }

        internal Double StartAngle
        {
            get
            {
                return _startAngle;
            }
            set
            {
                _startAngle = FixAngle(value);
            }
        }

        internal Double StopAngle
        {
            get
            {
                return _stopAngle;
            }
            set
            {
                _stopAngle = FixAngle(value);
            }
        }

        internal Point Center { get; set; }
        internal Double OffsetX { get; set; }
        internal Double OffsetY { get; set; }
        internal Boolean IsLargerArc { get; set; }
        internal Boolean Lighting { get; set; }
        internal Boolean Bevel { get; set; }
        internal Brush Background { get; set; }
        internal bool IsZero { get; set; }
        internal Double YAxisScaling
        {
            get
            {
                return Math.Sin(TiltAngle);
            }
        }
        internal Double ZAxisScaling
        {
            get
            {
                return Math.Cos(Math.PI / 2 - TiltAngle);
            }
        }
        internal Double Depth { get; set; }
        internal Double ExplodeRatio { get; set; }
        internal Double Width { get; set; }
        internal Double Height { get; set; }
        internal Double TiltAngle { get; set; }
        internal Point LabelPoint { get; set; }
        internal Brush LabelLineColor { get; set; }
        internal Double LabelLineThickness { get; set; }
        internal DoubleCollection LabelLineStyle { get; set; }
        internal Boolean LabelLineEnabled { get; set; }

        internal Double MeanAngle { get; set; }

        internal Storyboard Storyboard { get; set; }
        internal Boolean AnimationEnabled { get; set; }
    }

    internal struct Point3D
    {
        public Double X;
        public Double Y;
        public Double Z;

        public override string ToString()
        {
            return X.ToString() + "," + Y.ToString() + "," + Z.ToString();
        }
    }

    internal class PieDoughnut2DPoints
    {
        public PieDoughnut2DPoints()
        {
        }

        public Point Center { get; set; }
        public Point InnerArcStart { get; set; }
        public Point InnerArcMid { get; set; }
        public Point InnerArcEnd { get; set; }
        public Point OuterArcStart { get; set; }
        public Point OuterArcMid { get; set; }
        public Point OuterArcEnd { get; set; }
        public Point LabelLineStartPoint { get; set; }
        public Point LabelLineMidPoint { get; set; }
        public Point LabelLineEndPoint { get; set; }
        public Point LabelPosition { get; set; }
    }

    internal class PieDoughnut3DPoints
    {
        public PieDoughnut3DPoints()
        {
        }
        public Point LabelLineStartPoint { get; set; }
        public Point LabelLineMidPoint { get; set; }
        public Point LabelLineEndPoint { get; set; }
        public Point LabelPosition { get; set; }
    }

    internal class PieChart
    {
        private static Double FixAngle(Double angle)
        {
            while (angle > Math.PI * 2) angle -= Math.PI * 2;
            while (angle < 0) angle += Math.PI * 2;
            return angle;
        }

        private static List<ElementPositionData> _elementPositionData;

        private static Grid CreateLabel(DataPoint dataPoint)
        {
            Grid visual = new Grid() { Background = dataPoint.LabelBackground };
            TextBlock labelText = new TextBlock()
            {
                FontFamily = dataPoint.LabelFontFamily,
                FontSize = (Double)dataPoint.LabelFontSize,
                FontStyle = (FontStyle)dataPoint.LabelFontStyle,
                FontWeight = (FontWeight)dataPoint.LabelFontWeight,
                Foreground = Graphics.ApplyLabelFontColor((dataPoint.Chart as Chart), dataPoint, dataPoint.LabelFontColor, (LabelStyles)dataPoint.LabelStyle),
                Text = dataPoint.TextParser(dataPoint.LabelText)
            };

            visual.Children.Add(labelText);

            visual.Measure(new Size(Double.MaxValue, Double.MaxValue));

            dataPoint.LabelVisual = visual;

            return visual;
        }

        internal class PostionData
        {
            public Int32 Index;
            public Double yPosition;
            public Double xPosition;
            public Double MeanAngle;
            public Double Height;
            public Double Width;

            public static Int32 CompareYPosition(PostionData a, PostionData b)
            {
                return a.yPosition.CompareTo(b.yPosition);
            }
        }

        private static void PositionLabels(Canvas visual, Double totalSum, List<DataPoint> dataPoints, Dictionary<DataPoint, Grid> labels, Size pieSize, Size referenceEllipseSize, Size visualCanvasSize, Double scaleY, Boolean is3D)
        {
            Double hOuterEllipseRadius = referenceEllipseSize.Width / (is3D ? 1 : 2);
            Double vOuterEllipseRadius = referenceEllipseSize.Height / (is3D ? 1 : 2) * scaleY;
            Double hInnerEllipseRadius = (pieSize.Width / (is3D ? 1 : 2)) * 0.7;
            Double vInnerEllipseRadius = (pieSize.Height / (is3D ? 1 : 2)) * 0.7 * scaleY;
            Double hPieRadius = pieSize.Width / (is3D ? 1 : 2);
            Double vPieRadius = pieSize.Height / (is3D ? 1 : 2) * scaleY;

            Dictionary<Int32, PostionData> rightPositionData = new Dictionary<int, PostionData>();
            Dictionary<Int32, PostionData> leftPositionData = new Dictionary<int, PostionData>();
            Dictionary<Int32, PostionData> tempPositionData = new Dictionary<int, PostionData>();

            Double outerRadius = Math.Min(pieSize.Width, pieSize.Height) / (is3D ? 1 : 2);
            Int32 index = 0;
            Int32 rightIndex = 0;
            Int32 leftIndex = 0;

            // dataPoints[0].Parent.InternalStartAngle;
            Double startAngle = FixAngle(dataPoints[0].Parent.InternalStartAngle);
            Double stopAngle = 0;
            Double meanAngle = 0;

            Double xPos = 0;
            Double yPos = 0;

            Double centerX = visualCanvasSize.Width / 2;
            Double centerY = visualCanvasSize.Height / 2;

            Double gapLeft = 0;
            Double gapRight = 0;

            //if (!is3D)
            //    foreach (DataPoint dataPoint in dataPoints)
            //        if (dataPoint.LabelStyle == LabelStyles.Inside)
            //        {
            //            hInnerEllipseRadius -= hInnerEllipseRadius * 0.2; // ExploredRatio
            //            break;
            //        }
            
            foreach (DataPoint dataPoint in dataPoints)
            {
                if (dataPoint.YValue == 0) continue;

                stopAngle = startAngle + Math.PI * 2 * (Math.Abs(dataPoint.YValue) / totalSum);
                meanAngle = (startAngle + stopAngle) / 2;

                centerX = visualCanvasSize.Width / 2;
                centerY = visualCanvasSize.Height / 2;

                if (dataPoint.LabelStyle == LabelStyles.Inside)
                {
                    if (is3D)
                    {
                        xPos = centerX + hInnerEllipseRadius * Math.Cos(meanAngle) - labels[dataPoint].DesiredSize.Width;
                        yPos = centerY + vInnerEllipseRadius * Math.Sin(meanAngle) - labels[dataPoint].DesiredSize.Height * 2;
                    }
                    else
                    {
                        if (!is3D)
                        {
                            //if (dataPoint.Parent.RenderAs == RenderAs.Doughnut)
                            //{
                                xPos = centerX + 1.7 * (outerRadius / 3) * Math.Cos(meanAngle);
                                yPos = centerY + 1.7 * (outerRadius / 3) * Math.Sin(meanAngle);
                            //}
                            //else
                            //{
                            //    xPos = centerX + 1.7 * (outerRadius / 3) * Math.Cos(meanAngle);
                            //    yPos = centerY + 1.7 * (outerRadius / 3) * Math.Sin(meanAngle);
                            //}
                           
                        }
                        else
                        {
                            xPos = centerX + hInnerEllipseRadius  * Math.Cos(meanAngle);
                            yPos = centerY + vInnerEllipseRadius *  Math.Sin(meanAngle);
                        }

                        //Ellipse ellipse = new Ellipse() { Height = 6, Width = 6, Fill = new SolidColorBrush(Colors.Yellow) };

                        //ellipse.SetValue(Canvas.TopProperty, yPos);
                        //ellipse.SetValue(Canvas.LeftProperty, xPos);
                        //ellipse.SetValue(Canvas.ZIndexProperty,(Int32) 1000000);
                        //visual.Children.Add(ellipse);
                    }

                    xPos = xPos - labels[dataPoint].DesiredSize.Width / 2;
                    yPos = yPos - labels[dataPoint].DesiredSize.Height / 2;
                    
                    labels[dataPoint].SetValue(Canvas.TopProperty, yPos);
                    labels[dataPoint].SetValue(Canvas.LeftProperty, xPos);
                }
                else
                {
                    xPos = centerX + hOuterEllipseRadius * Math.Cos(meanAngle);
                    yPos = centerY + vOuterEllipseRadius * Math.Sin(meanAngle);

                    //Debug.WriteLine("Y: " + yPos + " A: " + meanAngle);

                    if (xPos < centerX)
                    {
                        xPos -= labels[dataPoint].DesiredSize.Width +10;
                        leftPositionData.Add(leftIndex++, new PostionData() { Index = index, xPosition = xPos, yPosition = yPos, MeanAngle = meanAngle, Height = labels[dataPoint].DesiredSize.Height, Width = labels[dataPoint].DesiredSize.Width });
                        gapLeft = Math.Max(gapLeft, labels[dataPoint].DesiredSize.Height);
                    }
                    else
                    {   
                        xPos += 10;
                        rightPositionData.Add(rightIndex++, new PostionData() { Index = index, xPosition = xPos, yPosition = yPos, MeanAngle = meanAngle, Height = labels[dataPoint].DesiredSize.Height, Width = labels[dataPoint].DesiredSize.Width });
                        gapRight = Math.Max(gapRight, labels[dataPoint].DesiredSize.Height);
                    }

                    // Ellipse ellipse = new Ellipse() { Height = 6, Width = 6, Fill = new SolidColorBrush(Colors.Yellow) };

                    //ellipse.SetValue(Canvas.TopProperty, yPos);
                    //ellipse.SetValue(Canvas.LeftProperty, xPos);
                    //ellipse.SetValue(Canvas.ZIndexProperty, (Int32)1000000);
                    //visual.Children.Add(ellipse);
                }

                startAngle = stopAngle;
                index++;

            }

            //visual.Background = new SolidColorBrush(Colors.Green);
            #region Left Alignment

            //// Following code for to place the pie labels for those datapoints who’s LabelStyles is OutSide
            PostionData tempData;
            Grid oldLabel = null;
            Double minimumY;
            Double maximumY;
            Double extent;

            if (is3D)
            {
                minimumY = centerY - vOuterEllipseRadius;
                maximumY = centerY + vOuterEllipseRadius;
            }
            else
            {
                minimumY = gapLeft / 2;
                maximumY = visualCanvasSize.Height - gapLeft / 2;
            }

            Double maxGapBetweenLabels = ((maximumY - minimumY) - (gapLeft * leftPositionData.Count)) / leftPositionData.Count;
            PositionLabels(minimumY, maximumY, 0, maxGapBetweenLabels, leftIndex, leftPositionData, false);

            for (Int32 i = 0; i < leftIndex; i++)
            {
                leftPositionData.TryGetValue(i, out tempData);

                centerX = visualCanvasSize.Width / 2;
                centerY = visualCanvasSize.Height / 2;

                extent = Math.Max(centerY - minimumY, maximumY - centerY);
                if (is3D)
                {
                    tempData.xPosition = centerX - Math.Sqrt((1 - Math.Pow((tempData.yPosition - centerY) / extent, 2)) * Math.Pow(hOuterEllipseRadius, 2)) - labels[dataPoints[tempData.Index]].DesiredSize.Width - 10;
                }
                else
                    tempData.xPosition = centerX - hOuterEllipseRadius * Math.Cos(Math.Asin(Math.Abs(tempData.yPosition - centerY) / hOuterEllipseRadius)) - labels[dataPoints[tempData.Index]].DesiredSize.Width - 10;

                if (tempData.xPosition < 0)
                    tempData.xPosition = 2;
                if (tempData.yPosition + labels[dataPoints[tempData.Index]].DesiredSize.Height > visualCanvasSize.Height)
                    tempData.yPosition = visualCanvasSize.Height - labels[dataPoints[tempData.Index]].DesiredSize.Height / 2;
                if (tempData.yPosition < labels[dataPoints[tempData.Index]].DesiredSize.Height / 2)
                    tempData.yPosition = labels[dataPoints[tempData.Index]].DesiredSize.Height / 2;

                if ((bool)dataPoints[tempData.Index].LabelEnabled)
                {
                    Double labelTop = tempData.yPosition - labels[dataPoints[tempData.Index]].DesiredSize.Height / 2;

                    if (tempData.MeanAngle > 1.5 * Math.PI / 2 && tempData.MeanAngle <= 2.7 * Math.PI/2)
                    {   
                        if (oldLabel != null)
                        {
                            Double oldTop = (Double)oldLabel.GetValue(Canvas.TopProperty);

                            Double overlapOffset = 0;

                            if (oldTop < labelTop + tempData.Height)
                            {
                                overlapOffset = labelTop + tempData.Height - oldTop;
                                labelTop -= overlapOffset/2;
                                oldLabel.SetValue(Canvas.TopProperty, oldTop + overlapOffset / 2);

                                for (int j = i - 2; j >= 0; j--)
                                {
                                    PostionData pData;
                                    leftPositionData.TryGetValue(j, out pData);
                                    Grid oldOldLabel = labels[dataPoints[pData.Index]];

                                    if ((pData.MeanAngle >= 1.5 * Math.PI / 2 && pData.MeanAngle <= 2.7 * Math.PI / 2))
                                    {
                                        System.Diagnostics.Debug.WriteLine((oldOldLabel.Children[0] as TextBlock).Text);
                                        System.Diagnostics.Debug.WriteLine((oldLabel.Children[0] as TextBlock).Text);
                                        oldTop = (Double)oldOldLabel.GetValue(Canvas.TopProperty);
                                        Double top = (Double)oldLabel.GetValue(Canvas.TopProperty);

                                        if (oldTop < top + oldLabel.DesiredSize.Height)
                                        {
                                            overlapOffset = top + oldLabel.DesiredSize.Height - oldTop;

                                            oldOldLabel.SetValue(Canvas.TopProperty, oldTop + overlapOffset);
                                        }
                                    }

                                    oldLabel = oldOldLabel;

                                }
                            }
                        }
                    }

                    System.Diagnostics.Debug.WriteLine("Text=" + (labels[dataPoints[tempData.Index]].Children[0] as TextBlock).Text);

                    labels[dataPoints[tempData.Index]].SetValue(Canvas.TopProperty, labelTop);

                    labels[dataPoints[tempData.Index]].SetValue(Canvas.LeftProperty, tempData.xPosition);

                }


                oldLabel = dataPoints[tempData.Index].LabelVisual;

                //if (i == 1)
                //    break;
            }

            #endregion

            #region Right Alignment


            PostionData[] dataForSorting = rightPositionData.Values.ToArray();
            Array.Sort(dataForSorting, PostionData.CompareYPosition);
            rightPositionData.Clear();
            for (int i = 0; i < dataForSorting.Length; i++)
                rightPositionData.Add(i, dataForSorting[i]);

            if (is3D)
            {
                minimumY = centerY - vOuterEllipseRadius;
                maximumY = centerY + vOuterEllipseRadius;

            }
            else
            {
                minimumY = gapRight / 2;
                maximumY = visualCanvasSize.Height - gapRight / 2;
            }
            maxGapBetweenLabels = ((maximumY - minimumY) - (gapRight * rightPositionData.Count)) / rightPositionData.Count;
            PositionLabels(minimumY, maximumY, 0, maxGapBetweenLabels, rightIndex, rightPositionData, true);

            for (Int32 i = 0; i < rightIndex; i++)
            {
                rightPositionData.TryGetValue(i, out tempData);

                centerX = visualCanvasSize.Width / 2;
                centerY = visualCanvasSize.Height / 2;

                extent = Math.Max(centerY - minimumY, maximumY - centerY);
                if (is3D)
                {
                    tempData.xPosition = centerX + Math.Sqrt((1 - Math.Pow((tempData.yPosition - centerY) / extent, 2)) * Math.Pow(hOuterEllipseRadius, 2)) + 10;
                }
                else
                    tempData.xPosition = centerX + hOuterEllipseRadius * Math.Cos(Math.Asin(Math.Abs(tempData.yPosition - centerY) / hOuterEllipseRadius)) + 10;

                if (tempData.xPosition + labels[dataPoints[tempData.Index]].DesiredSize.Width > visualCanvasSize.Width)
                    tempData.xPosition = visualCanvasSize.Width - labels[dataPoints[tempData.Index]].DesiredSize.Width;
                if (tempData.yPosition + labels[dataPoints[tempData.Index]].DesiredSize.Height > visualCanvasSize.Height)
                    tempData.yPosition = visualCanvasSize.Height - labels[dataPoints[tempData.Index]].DesiredSize.Height;
                if (tempData.yPosition < labels[dataPoints[tempData.Index]].DesiredSize.Height / 2)
                    tempData.yPosition = labels[dataPoints[tempData.Index]].DesiredSize.Height / 2;

                if ((bool)dataPoints[tempData.Index].LabelEnabled)
                {
                    //labels[dataPoints[tempData.Index]].SetValue(Canvas.TopProperty, tempData.yPosition - labels[dataPoints[tempData.Index]].DesiredSize.Height / 2);

                    //labels[dataPoints[tempData.Index]].SetValue(Canvas.LeftProperty, tempData.xPosition);

                    Double labelTop = tempData.yPosition - labels[dataPoints[tempData.Index]].DesiredSize.Height / 2;
                    Double labelLeft = tempData.xPosition;

                    if ((tempData.MeanAngle >= 3.7 * Math.PI / 2 && tempData.MeanAngle <= 4 * Math.PI / 2) || (tempData.MeanAngle >= 0 && tempData.MeanAngle <= Math.PI / 4))
                    {
                        if (oldLabel != null)
                        {
                            Double oldTop = (Double)oldLabel.GetValue(Canvas.TopProperty);

                            Double overlapOffset = 0;

                            if (labelTop < oldTop + oldLabel.DesiredSize.Height)
                            {
                                overlapOffset = oldTop + oldLabel.DesiredSize.Height - labelTop;
                                labelTop += overlapOffset / 2;
                                oldLabel.SetValue(Canvas.TopProperty, oldTop - overlapOffset / 2);

                                for (int j = i - 2; j > 0; j--)
                                {
                                    PostionData pData;
                                    rightPositionData.TryGetValue(j, out pData);
                                    Grid oldOldLabel = labels[dataPoints[pData.Index]];

                                    if ((pData.MeanAngle >= 3.7 * Math.PI / 2 && pData.MeanAngle <= 4 * Math.PI / 2) || (pData.MeanAngle >= 0 && pData.MeanAngle <= Math.PI / 4))
                                    {
                                        System.Diagnostics.Debug.WriteLine((oldOldLabel.Children[0] as TextBlock).Text);
                                        System.Diagnostics.Debug.WriteLine((oldLabel.Children[0] as TextBlock).Text);
                                        oldTop = (Double)oldOldLabel.GetValue(Canvas.TopProperty);
                                        Double top = (Double)oldLabel.GetValue(Canvas.TopProperty);

                                        if (top < oldTop + oldOldLabel.DesiredSize.Height)
                                        {
                                            overlapOffset = oldTop + oldOldLabel.DesiredSize.Height - top;
                                            oldTop -= overlapOffset;
                                            oldOldLabel.SetValue(Canvas.TopProperty, oldTop);
                                        }
                                    }

                                    oldLabel = oldOldLabel;

                                }
                            }
                        }

                    }

                    

                    labels[dataPoints[tempData.Index]].SetValue(Canvas.TopProperty, labelTop);

                    labels[dataPoints[tempData.Index]].SetValue(Canvas.LeftProperty, labelLeft);
                }

                oldLabel = dataPoints[tempData.Index].LabelVisual;
            }
            #endregion

        }

        private static void PositionLabels(Double minY, Double maxY, Double gap, Double maxGap, Double labelCount, Dictionary<Int32, PostionData> labelPositions, Boolean isRight)
        {
            Double limit = (isRight) ? minY : maxY;
            Double sign = (isRight) ? -1 : 1;
            Int32 iterationCount = 0;
            Boolean isOverlap = false;
            Double previousY;
            Double currentY;
            PostionData point;
            //Double offsetFactor = sign * ((gap > maxGap) ? maxGap / 2 : gap / 2);
            Double offsetFactor = sign * ((gap > maxGap) ? (maxGap / 10) : (gap / 10));
            do
            {
                previousY = limit;
                isOverlap = false;

                for (Int32 i = 0; i < labelCount; i++)
                {
                    labelPositions.TryGetValue(i, out point);
                    currentY = point.yPosition;

                    if (Math.Abs(previousY - currentY) < gap && i != 0)
                    {
                        point.yPosition = previousY - offsetFactor;
                        if (isRight)
                        {
                            if (point.yPosition > maxY) point.yPosition = (previousY + maxY - gap) / 2;
                        }
                        else
                        {
                            if (point.yPosition < minY) point.yPosition = (minY + previousY) / 2;
                        }
                        currentY = point.yPosition;

                        labelPositions.Remove(i);
                        //Debug.WriteLine("Y: " + point.yPosition + " A: " + point.MeanAngle);
                        labelPositions.Add(i, new PostionData() { Index = point.Index, MeanAngle = point.MeanAngle, xPosition = point.xPosition, yPosition = point.yPosition });

                        labelPositions.TryGetValue(i - 1, out point);
                        point.yPosition = previousY + offsetFactor;

                        if (isRight)
                        {
                            if (point.yPosition < minY) point.yPosition = (minY + previousY) / 2;
                        }
                        else
                        {
                            if (point.yPosition > maxY) point.yPosition = (previousY + maxY - gap) / 2;
                        }

                        //Debug.WriteLine("Y: " + point.yPosition + " A: " + point.MeanAngle);
                        labelPositions.Remove(i - 1);
                        labelPositions.Add(i - 1, new PostionData() { Index = point.Index, MeanAngle = point.MeanAngle, xPosition = point.xPosition, yPosition = point.yPosition });
                        isOverlap = true;
                        if (isRight)
                        {
                            if (previousY < currentY) isOverlap = true;
                        }
                        else
                        {
                            if (previousY > currentY) isOverlap = true;
                        }
                        break;
                    }

                    previousY = currentY;
                }
                iterationCount++;

            } while (isOverlap && iterationCount < 128);

            if (isOverlap)
            {
                Double stepSize = (maxY - minY) / labelCount;
                for (Int32 i = 0; i < labelCount; i++)
                {
                    labelPositions.TryGetValue(i, out point);
                    if (isRight)
                    {
                        point.yPosition = minY + stepSize * i;
                    }
                    else
                    {
                        point.yPosition = maxY - stepSize * (i + 1);
                    }
                    //Debug.WriteLine("Y: " + point.yPosition + " A: " + point.MeanAngle);
                    labelPositions.Remove(i);
                    labelPositions.Add(i, new PostionData() { Index = point.Index, MeanAngle = point.MeanAngle, xPosition = point.xPosition, yPosition = point.yPosition });
                }
            }
        }

        private static Canvas CreateAndPositionLabels(Double totalSum, List<DataPoint> dataPoints, Double width, Double height, Double scaleY, Boolean is3D, ref Size size)
        {
            Canvas visual = new Canvas() { Height = height, Width = width };

            //visual.Background = new SolidColorBrush(Colors.Green);

            Dictionary<DataPoint, Grid> labels = new Dictionary<DataPoint, Grid>();

            Double labelLineLength = 30;

            Boolean isLabelEnabled = false;
            Boolean isLabelOutside = false;
            Double maxLabelWidth = 0;
            Double maxLabelHeight = 0;

            foreach (DataPoint dataPoint in dataPoints)
            {
                if (dataPoint.YValue == 0)
                    continue;

                Grid label = CreateLabel(dataPoint);
                if ((bool)dataPoint.LabelEnabled)
                {
                    maxLabelWidth = Math.Max(maxLabelWidth, label.DesiredSize.Width);
                    maxLabelHeight = Math.Max(maxLabelHeight, label.DesiredSize.Height);

                    isLabelEnabled = true;

                    if (dataPoint.LabelStyle == LabelStyles.OutSide)
                        isLabelOutside = true;
                }

                labels.Add(dataPoint, label);

                if (isLabelEnabled)
                    visual.Children.Add(label);
            }

            //this is to offset the label to draw label line
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
                    //pieCanvasWidth = minLength - 4 - labelLineLength ;
                    //pieCanvasHeight = minLength - 4 - labelLineLength ;
                    pieCanvasWidth = minLength - labelLineLength * 2;
                    pieCanvasHeight = pieCanvasWidth;

                    labelEllipseWidth = minLength;
                    labelEllipseHeight = labelEllipseWidth;
                    //labelEllipseWidth = pieCanvasWidth + maxLabelWidth;
                    //labelEllipseHeight = pieCanvasHeight + maxLabelHeight;

                    PositionLabels(visual, totalSum, dataPoints, labels, new Size(Math.Abs(pieCanvasWidth), Math.Abs(pieCanvasHeight)), new Size(Math.Abs(labelEllipseWidth), Math.Abs(labelEllipseHeight)), new Size(width, height), scaleY, is3D);
                }
                else
                {
                    pieCanvasWidth = minLength;
                    pieCanvasHeight = minLength;

                    labelEllipseWidth = pieCanvasWidth;
                    labelEllipseHeight = pieCanvasHeight;

                    PositionLabels(visual, totalSum, dataPoints, labels, new Size(Math.Abs(pieCanvasWidth), Math.Abs(pieCanvasHeight)), new Size(Math.Abs(labelEllipseWidth), Math.Abs(labelEllipseHeight)), new Size(width, height), scaleY, is3D);
                }
            }
            else
            {
                pieCanvasWidth = minLength;
                pieCanvasHeight = minLength;
            }

            size = new Size(Math.Abs(pieCanvasWidth), Math.Abs(pieCanvasHeight));

            if (isLabelEnabled)
                return visual;
            else
                return null;
        }

        private static Canvas GetPie2D(ref Faces faces,  SectorChartShapeParams pieParams, ref PieDoughnut2DPoints unExplodedPoints, ref PieDoughnut2DPoints explodedPoints, ref Path labelLinePath)
        {
            Canvas visual = new Canvas();

            Double width = pieParams.OuterRadius * 2 ;
            Double height = pieParams.OuterRadius * 2 ;

            visual.Width = width;
            visual.Height = height;

            Point center = new Point(width / 2, height / 2);
            Double xOffset = pieParams.OuterRadius * pieParams.ExplodeRatio * Math.Cos(pieParams.MeanAngle);
            Double yOffset = pieParams.OuterRadius * pieParams.ExplodeRatio * Math.Sin(pieParams.MeanAngle);


            #region PieSlice
            if (pieParams.StartAngle != pieParams.StopAngle || !pieParams.IsZero)
            {   
                Ellipse ellipse = new Ellipse();
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
                clipPathGeometry.Add(new ArcSegmentParams(new Size(pieParams.OuterRadius, pieParams.OuterRadius), 0, false, SweepDirection.Clockwise, pieParams.AnimationEnabled ? start : arcMidPoint));
                clipPathGeometry.Add(new ArcSegmentParams(new Size(pieParams.OuterRadius, pieParams.OuterRadius), 0, false, SweepDirection.Clockwise, pieParams.AnimationEnabled ? start : end));
                clipPathGeometry.Add(new LineSegmentParams(center));
                ellipse.Clip = GetPathGeometryFromList(FillRule.Nonzero, center, clipPathGeometry);
                PathSegmentCollection segments = (ellipse.Clip as PathGeometry).Figures[0].Segments;

                // apply animation to the individual points that for the pie slice
                if (pieParams.AnimationEnabled)
                {
                    // if the stop angle is zero then animation weill not be applies to that point 
                    // hence during animation the shape of the pie will get distorted
                    Double stopAngle = 0;
                    if (pieParams.StopAngle == 0)
                        stopAngle = pieParams.StartAngle + Math.Abs(pieParams.MeanAngle - pieParams.StartAngle) * 2;
                    else
                        stopAngle = pieParams.StopAngle;

                    // apply animation to the points
                    pieParams.Storyboard = CreatePathSegmentAnimation(pieParams.Storyboard, segments[1], center, pieParams.OuterRadius, 0, pieParams.MeanAngle);
                    pieParams.Storyboard = CreatePathSegmentAnimation(pieParams.Storyboard, segments[2], center, pieParams.OuterRadius, 0, stopAngle);
                    pieParams.Storyboard = CreatePathSegmentAnimation(pieParams.Storyboard, segments[0], center, pieParams.OuterRadius, 0, pieParams.StartAngle);
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
            }
            #endregion PieSlice

            #region Lighting

            if (pieParams.Lighting && (pieParams.StartAngle != pieParams.StopAngle || !pieParams.IsZero))
            {
                Ellipse lightingEllipse = new Ellipse();
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
                clipPathGeometry.Add(new ArcSegmentParams(new Size(pieParams.OuterRadius, pieParams.OuterRadius), 0, false, SweepDirection.Clockwise, pieParams.AnimationEnabled ? start : arcMidPoint));
                clipPathGeometry.Add(new ArcSegmentParams(new Size(pieParams.OuterRadius, pieParams.OuterRadius), 0, false, SweepDirection.Clockwise, pieParams.AnimationEnabled ? start : end));
                clipPathGeometry.Add(new LineSegmentParams(center));
                lightingEllipse.Clip = GetPathGeometryFromList(FillRule.Nonzero, center, clipPathGeometry);
                PathSegmentCollection segments = (lightingEllipse.Clip as PathGeometry).Figures[0].Segments;

                // apply animation to the individual points that for the shape that
                // gives the lighting effect to the pie slice
                if (pieParams.AnimationEnabled)
                {
                    // if the stop angle is zero then animation weill not be applies to that point 
                    // hence during animation the shape of the pie will get distorted
                    Double stopAngle = 0;
                    if (pieParams.StopAngle == 0)
                        stopAngle = pieParams.StartAngle + Math.Abs(pieParams.MeanAngle - pieParams.StartAngle) * 2;
                    else
                        stopAngle = pieParams.StopAngle;

                    // apply animation to the points
                    pieParams.Storyboard = CreatePathSegmentAnimation(pieParams.Storyboard, segments[1], center, pieParams.OuterRadius, 0, pieParams.MeanAngle);
                    pieParams.Storyboard = CreatePathSegmentAnimation(pieParams.Storyboard, segments[2], center, pieParams.OuterRadius, 0, stopAngle);
                    pieParams.Storyboard = CreatePathSegmentAnimation(pieParams.Storyboard, segments[0], center, pieParams.OuterRadius, 0, pieParams.StartAngle);
                }
                visual.Children.Add(lightingEllipse);
            }
            #endregion Lighting

            #region LabelLine

            if (pieParams.LabelLineEnabled)
            {
                Path labelLine = new Path();
                Double meanAngle = pieParams.MeanAngle;

                Point piePoint = new Point();
                piePoint.X = center.X + pieParams.OuterRadius * Math.Cos(meanAngle);
                piePoint.Y = center.Y + pieParams.OuterRadius * Math.Sin(meanAngle);

                Point labelPoint = new Point();
                labelPoint.X = center.X + pieParams.LabelPoint.X - pieParams.Width / 2;
                labelPoint.Y = center.Y + pieParams.LabelPoint.Y - pieParams.Height / 2;

                Point midPoint = new Point();
                midPoint.X = (labelPoint.X < center.X) ? labelPoint.X + 10 : labelPoint.X - 10;
                midPoint.Y = labelPoint.Y;

                List<PathGeometryParams> labelLinePathGeometry = new List<PathGeometryParams>();
                labelLinePathGeometry.Add(new LineSegmentParams(pieParams.AnimationEnabled ? piePoint : midPoint));
                labelLinePathGeometry.Add(new LineSegmentParams(pieParams.AnimationEnabled ? piePoint : labelPoint));
                labelLine.Data = GetPathGeometryFromList(FillRule.Nonzero, piePoint, labelLinePathGeometry);
                PathFigure figure = (labelLine.Data as PathGeometry).Figures[0];
                PathSegmentCollection segments = figure.Segments;
                figure.IsClosed = false;
                figure.IsFilled = false;

                // animate the label lines of the individual pie slices
                if (pieParams.AnimationEnabled)
                {
                    pieParams.Storyboard = CreateLabelLineAnimation(pieParams.Storyboard, segments[0], piePoint, midPoint);
                    pieParams.Storyboard = CreateLabelLineAnimation(pieParams.Storyboard, segments[1], piePoint, midPoint, labelPoint);
                }

                labelLine.Stroke = pieParams.LabelLineColor;
                labelLine.StrokeDashArray = pieParams.LabelLineStyle;
                labelLine.StrokeThickness = pieParams.LabelLineThickness;

                labelLinePath = labelLine;

                visual.Children.Add(labelLine);

                // set the un exploded points for interactivity
                unExplodedPoints.LabelLineEndPoint = labelPoint;
                unExplodedPoints.LabelLineMidPoint = midPoint;
                unExplodedPoints.LabelLineStartPoint = piePoint;

                // set the exploded points for interactivity
                explodedPoints.LabelLineEndPoint = new Point(labelPoint.X, labelPoint.Y - yOffset);
                explodedPoints.LabelLineMidPoint = new Point(midPoint.X, midPoint.Y - yOffset);
                explodedPoints.LabelLineStartPoint = new Point(piePoint.X + xOffset, piePoint.Y + yOffset);
            }

            #endregion LabelLine

            #region Bevel

            if (pieParams.Bevel && Math.Abs(pieParams.StartAngle - pieParams.StopAngle) > 0.03 && (pieParams.StartAngle != pieParams.StopAngle))
            {
                Point start = new Point();
                Point end = new Point();

                start.X = center.X + pieParams.OuterRadius * Math.Cos(pieParams.StartAngle);
                start.Y = center.Y + pieParams.OuterRadius * Math.Sin(pieParams.StartAngle);

                end.X = center.X + pieParams.OuterRadius * Math.Cos(pieParams.StopAngle);
                end.Y = center.Y + pieParams.OuterRadius * Math.Sin(pieParams.StopAngle);

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

                List<PathGeometryParams> pathGeometry = new List<PathGeometryParams>();
                pathGeometry.Add(new LineSegmentParams(center));
                pathGeometry.Add(new LineSegmentParams(start));
                pathGeometry.Add(new LineSegmentParams(bevelStart));
                pathGeometry.Add(new LineSegmentParams(bevelCenter));

                Path path = new Path();

                path.Data = GetPathGeometryFromList(FillRule.Nonzero, bevelCenter, pathGeometry);
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
                    pieParams.Storyboard = CreateOpacityAnimation(pieParams.Storyboard, path, 1, 1, 1);
                    path.Opacity = 0;
                }

                faces.Parts.Add(path);
                visual.Children.Add(path);

                pathGeometry = new List<PathGeometryParams>();
                pathGeometry.Add(new LineSegmentParams(center));
                pathGeometry.Add(new LineSegmentParams(end));
                pathGeometry.Add(new LineSegmentParams(bevelEnd));
                pathGeometry.Add(new LineSegmentParams(bevelCenter));

                path = new Path();
                path.Data = GetPathGeometryFromList(FillRule.Nonzero, bevelCenter, pathGeometry);
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
                    pieParams.Storyboard = CreateOpacityAnimation(pieParams.Storyboard, path, 1, 1, 1);
                    path.Opacity = 0;
                }

                faces.Parts.Add(path);
                visual.Children.Add(path);

                pathGeometry = new List<PathGeometryParams>();
                pathGeometry.Add(new LineSegmentParams(end));
                pathGeometry.Add(new ArcSegmentParams(new Size(pieParams.OuterRadius, pieParams.OuterRadius), 0, pieParams.IsLargerArc, SweepDirection.Counterclockwise, start));
                pathGeometry.Add(new LineSegmentParams(bevelStart));
                pathGeometry.Add(new ArcSegmentParams(new Size(bevelOuterRadius, bevelOuterRadius), 0, pieParams.IsLargerArc, SweepDirection.Clockwise, bevelEnd));

                path = new Path();
                path.Data = GetPathGeometryFromList(FillRule.Nonzero, bevelEnd, pathGeometry);
                if (pieParams.MeanAngle > 0 && pieParams.MeanAngle < Math.PI)
                {
                    path.Fill = GetCurvedBevelBrush(pieParams.Background, pieParams.MeanAngle * 180 / Math.PI + 90, GetDoubleCollection(-0.745, -0.85), GetDoubleCollection(0, 1));
                }
                else
                {
                    path.Fill = GetCurvedBevelBrush(pieParams.Background, pieParams.MeanAngle * 180 / Math.PI + 90, GetDoubleCollection(0.745, -0.99), GetDoubleCollection(0, 1));
                }
                // Apply animation to the beveling path
                if (pieParams.AnimationEnabled)
                {
                    pieParams.Storyboard = CreateOpacityAnimation(pieParams.Storyboard, path, 1, 1, 1);
                    path.Opacity = 0;
                }


                faces.Parts.Add(path);
                visual.Children.Add(path);
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

        private static Canvas GetDoughnut2D(ref Faces faces, SectorChartShapeParams pieParams, ref PieDoughnut2DPoints unExplodedPoints, ref PieDoughnut2DPoints explodedPoints, ref Path labelLinePath)
        {
            Canvas visual = new Canvas();

            Double width = pieParams.OuterRadius * 2;
            Double height = pieParams.OuterRadius * 2;

            visual.Width = width;
            visual.Height = height;

            Point center = new Point(width / 2, height / 2);
            Double xOffset = pieParams.OuterRadius * pieParams.ExplodeRatio * Math.Cos(pieParams.MeanAngle);
            Double yOffset = pieParams.OuterRadius * pieParams.ExplodeRatio * Math.Sin(pieParams.MeanAngle);

            #region Doughnut Slice
            if (pieParams.StartAngle != pieParams.StopAngle || !pieParams.IsZero)
            {

                Ellipse ellipse = new Ellipse();
                ellipse.Width = width;
                ellipse.Height = height;
                ellipse.Fill = pieParams.Lighting ? Graphics.GetLightingEnabledBrush(pieParams.Background, "Radial", new Double[] { 0.99, 0.745 }) : pieParams.Background;


                Point start = new Point();
                Point end = new Point();
                Point arcMidPoint = new Point();
                Point innerstart = new Point();
                Point innerend = new Point();
                Point innerArcMidPoint = new Point();

                start.X = center.X + pieParams.OuterRadius * Math.Cos(pieParams.StartAngle);
                start.Y = center.Y + pieParams.OuterRadius * Math.Sin(pieParams.StartAngle);

                end.X = center.X + pieParams.OuterRadius * Math.Cos(pieParams.StopAngle);
                end.Y = center.Y + pieParams.OuterRadius * Math.Sin(pieParams.StopAngle);

                arcMidPoint.X = center.X + pieParams.OuterRadius * Math.Cos(pieParams.MeanAngle);
                arcMidPoint.Y = center.Y + pieParams.OuterRadius * Math.Sin(pieParams.MeanAngle);

                innerstart.X = center.X + pieParams.InnerRadius * Math.Cos(pieParams.StartAngle);
                innerstart.Y = center.Y + pieParams.InnerRadius * Math.Sin(pieParams.StartAngle);

                innerend.X = center.X + pieParams.InnerRadius * Math.Cos(pieParams.StopAngle);
                innerend.Y = center.Y + pieParams.InnerRadius * Math.Sin(pieParams.StopAngle);

                innerArcMidPoint.X = center.X + pieParams.InnerRadius * Math.Cos(pieParams.MeanAngle);
                innerArcMidPoint.Y = center.Y + pieParams.InnerRadius * Math.Sin(pieParams.MeanAngle);

                List<PathGeometryParams> clipPathGeometry = new List<PathGeometryParams>();
                clipPathGeometry.Add(new LineSegmentParams(start));
                clipPathGeometry.Add(new ArcSegmentParams(new Size(pieParams.OuterRadius, pieParams.OuterRadius), 0, false, SweepDirection.Clockwise, pieParams.AnimationEnabled ? start : arcMidPoint));
                clipPathGeometry.Add(new ArcSegmentParams(new Size(pieParams.OuterRadius, pieParams.OuterRadius), 0, false, SweepDirection.Clockwise, pieParams.AnimationEnabled ? start : end));
                clipPathGeometry.Add(new LineSegmentParams(pieParams.AnimationEnabled ? innerstart : innerend));
                clipPathGeometry.Add(new ArcSegmentParams(new Size(pieParams.InnerRadius, pieParams.InnerRadius), 0, false, SweepDirection.Counterclockwise, pieParams.AnimationEnabled ? innerstart : innerArcMidPoint));
                clipPathGeometry.Add(new ArcSegmentParams(new Size(pieParams.InnerRadius, pieParams.InnerRadius), 0, false, SweepDirection.Counterclockwise, pieParams.AnimationEnabled ? innerstart : innerstart));

                ellipse.Clip = GetPathGeometryFromList(FillRule.Nonzero, innerstart, clipPathGeometry);

                PathFigure figure = (ellipse.Clip as PathGeometry).Figures[0];
                PathSegmentCollection segments = figure.Segments;

                // Apply animation to the doughnut slice
                if (pieParams.AnimationEnabled)
                {
                    // if stop angle is zero the animation creates a distorted doughnut while animating
                    // so we need adjust the value such that the doughnutis not distorted
                    Double stopAngle = 0;
                    if (pieParams.StopAngle == 0)
                        stopAngle = pieParams.StartAngle + Math.Abs(pieParams.MeanAngle - pieParams.StartAngle) * 2;
                    else
                        stopAngle = pieParams.StopAngle;

                    // animate the outer points
                    pieParams.Storyboard = CreatePathSegmentAnimation(pieParams.Storyboard, segments[0], center, pieParams.OuterRadius, 0, pieParams.StartAngle);
                    pieParams.Storyboard = CreatePathSegmentAnimation(pieParams.Storyboard, segments[1], center, pieParams.OuterRadius, 0, pieParams.MeanAngle);
                    pieParams.Storyboard = CreatePathSegmentAnimation(pieParams.Storyboard, segments[2], center, pieParams.OuterRadius, 0, stopAngle);

                    // animate the inner points
                    pieParams.Storyboard = CreatePathSegmentAnimation(pieParams.Storyboard, segments[3], center, pieParams.InnerRadius, 0, stopAngle);
                    pieParams.Storyboard = CreatePathSegmentAnimation(pieParams.Storyboard, segments[4], center, pieParams.InnerRadius, 0, pieParams.MeanAngle);
                    pieParams.Storyboard = CreatePathSegmentAnimation(pieParams.Storyboard, segments[5], center, pieParams.InnerRadius, 0, pieParams.StartAngle);

                    pieParams.Storyboard = CreatePathFigureAnimation(pieParams.Storyboard, figure, center, pieParams.InnerRadius, 0, pieParams.StartAngle);
                }

                faces.Parts.Add(ellipse);
                visual.Children.Add(ellipse);

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
            }
            #endregion Doughnut Slice

            #region Lighting
            if (pieParams.Lighting)
            {
                Ellipse lightingEllipse = new Ellipse();
                lightingEllipse.Width = width;
                lightingEllipse.Height = height;
                lightingEllipse.IsHitTestVisible = false;
                lightingEllipse.Fill = GetDoughnutGradianceBrush();

                if (pieParams.StartAngle != pieParams.StopAngle || !pieParams.IsZero)
                {
                    Point start = new Point();
                    Point end = new Point();
                    Point arcMidPoint = new Point();
                    Point innerstart = new Point();
                    Point innerend = new Point();
                    Point innerArcMidPoint = new Point();

                    start.X = center.X + pieParams.OuterRadius * Math.Cos(pieParams.StartAngle);
                    start.Y = center.Y + pieParams.OuterRadius * Math.Sin(pieParams.StartAngle);

                    end.X = center.X + pieParams.OuterRadius * Math.Cos(pieParams.StopAngle);
                    end.Y = center.Y + pieParams.OuterRadius * Math.Sin(pieParams.StopAngle);

                    arcMidPoint.X = center.X + pieParams.OuterRadius * Math.Cos(pieParams.MeanAngle);
                    arcMidPoint.Y = center.Y + pieParams.OuterRadius * Math.Sin(pieParams.MeanAngle);

                    innerstart.X = center.X + pieParams.InnerRadius * Math.Cos(pieParams.StartAngle);
                    innerstart.Y = center.Y + pieParams.InnerRadius * Math.Sin(pieParams.StartAngle);

                    innerend.X = center.X + pieParams.InnerRadius * Math.Cos(pieParams.StopAngle);
                    innerend.Y = center.Y + pieParams.InnerRadius * Math.Sin(pieParams.StopAngle);

                    innerArcMidPoint.X = center.X + pieParams.InnerRadius * Math.Cos(pieParams.MeanAngle);
                    innerArcMidPoint.Y = center.Y + pieParams.InnerRadius * Math.Sin(pieParams.MeanAngle);

                    List<PathGeometryParams> clipPathGeometry = new List<PathGeometryParams>();
                    clipPathGeometry.Add(new LineSegmentParams(start));
                    clipPathGeometry.Add(new ArcSegmentParams(new Size(pieParams.OuterRadius, pieParams.OuterRadius), 0, false, SweepDirection.Clockwise, pieParams.AnimationEnabled ? start : arcMidPoint));
                    clipPathGeometry.Add(new ArcSegmentParams(new Size(pieParams.OuterRadius, pieParams.OuterRadius), 0, false, SweepDirection.Clockwise, pieParams.AnimationEnabled ? start : end));
                    clipPathGeometry.Add(new LineSegmentParams(pieParams.AnimationEnabled ? innerstart : innerend));
                    clipPathGeometry.Add(new ArcSegmentParams(new Size(pieParams.InnerRadius, pieParams.InnerRadius), 0, false, SweepDirection.Counterclockwise, pieParams.AnimationEnabled ? innerstart : innerArcMidPoint));
                    clipPathGeometry.Add(new ArcSegmentParams(new Size(pieParams.InnerRadius, pieParams.InnerRadius), 0, false, SweepDirection.Counterclockwise, pieParams.AnimationEnabled ? innerstart : innerstart));

                    lightingEllipse.Clip = GetPathGeometryFromList(FillRule.Nonzero, innerstart, clipPathGeometry);

                    PathFigure figure = (lightingEllipse.Clip as PathGeometry).Figures[0];
                    PathSegmentCollection segments = figure.Segments;

                    // Apply animation to the doughnut slice
                    if (pieParams.AnimationEnabled)
                    {
                        // if stop angle is zero the animation creates a distorted doughnut while animating
                        // so we need adjust the value such that the doughnutis not distorted
                        Double stopAngle = 0;
                        if (pieParams.StopAngle == 0)
                            stopAngle = pieParams.StartAngle + Math.Abs(pieParams.MeanAngle - pieParams.StartAngle) * 2;
                        else
                            stopAngle = pieParams.StopAngle;

                        // animate the outer points
                        pieParams.Storyboard = CreatePathSegmentAnimation(pieParams.Storyboard, segments[0], center, pieParams.OuterRadius, 0, pieParams.StartAngle);
                        pieParams.Storyboard = CreatePathSegmentAnimation(pieParams.Storyboard, segments[1], center, pieParams.OuterRadius, 0, pieParams.MeanAngle);
                        pieParams.Storyboard = CreatePathSegmentAnimation(pieParams.Storyboard, segments[2], center, pieParams.OuterRadius, 0, stopAngle);

                        // animate the inner points
                        pieParams.Storyboard = CreatePathSegmentAnimation(pieParams.Storyboard, segments[3], center, pieParams.InnerRadius, 0, stopAngle);
                        pieParams.Storyboard = CreatePathSegmentAnimation(pieParams.Storyboard, segments[4], center, pieParams.InnerRadius, 0, pieParams.MeanAngle);
                        pieParams.Storyboard = CreatePathSegmentAnimation(pieParams.Storyboard, segments[5], center, pieParams.InnerRadius, 0, pieParams.StartAngle);

                        pieParams.Storyboard = CreatePathFigureAnimation(pieParams.Storyboard, figure, center, pieParams.InnerRadius, 0, pieParams.StartAngle);
                    }
                }
                visual.Children.Add(lightingEllipse);
            }
            #endregion Lighting

            #region LabelLine
            if (pieParams.LabelLineEnabled)
            {
                Path labelLine = new Path();
                Double meanAngle = pieParams.MeanAngle;

                Point doughnutPoint = new Point();
                doughnutPoint.X = center.X + pieParams.OuterRadius * Math.Cos(meanAngle);
                doughnutPoint.Y = center.Y + pieParams.OuterRadius * Math.Sin(meanAngle);

                Point labelPoint = new Point();
                labelPoint.X = center.X + pieParams.LabelPoint.X - pieParams.Width / 2;
                labelPoint.Y = center.Y + pieParams.LabelPoint.Y - pieParams.Height / 2;

                Point midPoint = new Point();
                midPoint.X = (labelPoint.X < center.X) ? labelPoint.X + 10 : labelPoint.X - 10;
                midPoint.Y = labelPoint.Y;

                List<PathGeometryParams> labelLinePathGeometry = new List<PathGeometryParams>();
                labelLinePathGeometry.Add(new LineSegmentParams(pieParams.AnimationEnabled ? doughnutPoint : midPoint));
                labelLinePathGeometry.Add(new LineSegmentParams(pieParams.AnimationEnabled ? doughnutPoint : labelPoint));
                labelLine.Data = GetPathGeometryFromList(FillRule.Nonzero, doughnutPoint, labelLinePathGeometry);
                PathFigure figure = (labelLine.Data as PathGeometry).Figures[0];
                PathSegmentCollection segments = figure.Segments;
                figure.IsClosed = false;
                figure.IsFilled = false;

                // apply animation to the label line
                if (pieParams.AnimationEnabled)
                {
                    pieParams.Storyboard = CreateLabelLineAnimation(pieParams.Storyboard, segments[0], doughnutPoint, midPoint);
                    pieParams.Storyboard = CreateLabelLineAnimation(pieParams.Storyboard, segments[1], doughnutPoint, midPoint, labelPoint);
                }

                labelLine.Stroke = pieParams.LabelLineColor;
                labelLine.StrokeDashArray = pieParams.LabelLineStyle;
                labelLine.StrokeThickness = pieParams.LabelLineThickness;
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
            }
            #endregion LabelLine

            #region Bevel
            if (pieParams.Bevel && Math.Abs(pieParams.StartAngle - pieParams.StopAngle) > 0.03)
            {
                Point start = new Point();
                Point end = new Point();
                Point innerstart = new Point();
                Point innerend = new Point();

                start.X = center.X + pieParams.OuterRadius * Math.Cos(pieParams.StartAngle);
                start.Y = center.Y + pieParams.OuterRadius * Math.Sin(pieParams.StartAngle);

                end.X = center.X + pieParams.OuterRadius * Math.Cos(pieParams.StopAngle);
                end.Y = center.Y + pieParams.OuterRadius * Math.Sin(pieParams.StopAngle);

                innerstart.X = center.X + pieParams.InnerRadius * Math.Cos(pieParams.StartAngle);
                innerstart.Y = center.Y + pieParams.InnerRadius * Math.Sin(pieParams.StartAngle);

                innerend.X = center.X + pieParams.InnerRadius * Math.Cos(pieParams.StopAngle);
                innerend.Y = center.Y + pieParams.InnerRadius * Math.Sin(pieParams.StopAngle);

                Point bevelCenter = new Point();
                Point bevelStart = new Point();
                Point bevelEnd = new Point();
                Point bevelInnerStart = new Point();
                Point bevelInnerEnd = new Point();
                Double bevelLength = 4;
                Double bevelOuterRadius = Math.Abs(pieParams.OuterRadius - bevelLength);
                Double bevelInnerRadius = Math.Abs(pieParams.InnerRadius + bevelLength);

                bevelCenter.X = center.X + bevelLength * Math.Cos(pieParams.MeanAngle);
                bevelCenter.Y = center.Y + bevelLength * Math.Sin(pieParams.MeanAngle);

                bevelStart.X = center.X + bevelOuterRadius * Math.Cos(pieParams.StartAngle + 0.03);
                bevelStart.Y = center.Y + bevelOuterRadius * Math.Sin(pieParams.StartAngle + 0.03);

                bevelEnd.X = center.X + bevelOuterRadius * Math.Cos(pieParams.StopAngle - 0.03);
                bevelEnd.Y = center.Y + bevelOuterRadius * Math.Sin(pieParams.StopAngle - 0.03);

                bevelInnerStart.X = center.X + bevelInnerRadius * Math.Cos(pieParams.StartAngle + 0.03);
                bevelInnerStart.Y = center.Y + bevelInnerRadius * Math.Sin(pieParams.StartAngle + 0.03);

                bevelInnerEnd.X = center.X + bevelInnerRadius * Math.Cos(pieParams.StopAngle - 0.03);
                bevelInnerEnd.Y = center.Y + bevelInnerRadius * Math.Sin(pieParams.StopAngle - 0.03);

                List<PathGeometryParams> pathGeometry = new List<PathGeometryParams>();
                pathGeometry.Add(new LineSegmentParams(innerstart));
                pathGeometry.Add(new LineSegmentParams(start));
                pathGeometry.Add(new LineSegmentParams(bevelStart));
                pathGeometry.Add(new LineSegmentParams(bevelInnerStart));

                Path path = new Path();
                path.Data = GetPathGeometryFromList(FillRule.Nonzero, bevelInnerStart, pathGeometry);
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
                    pieParams.Storyboard = CreateOpacityAnimation(pieParams.Storyboard, path, 1, 1, 1);
                    path.Opacity = 0;
                }

                faces.Parts.Add(path);
                visual.Children.Add(path);

                pathGeometry = new List<PathGeometryParams>();
                pathGeometry.Add(new LineSegmentParams(innerend));
                pathGeometry.Add(new LineSegmentParams(end));
                pathGeometry.Add(new LineSegmentParams(bevelEnd));
                pathGeometry.Add(new LineSegmentParams(bevelInnerEnd));

                path = new Path();
                path.Data = GetPathGeometryFromList(FillRule.Nonzero, bevelInnerEnd, pathGeometry);
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
                    pieParams.Storyboard = CreateOpacityAnimation(pieParams.Storyboard, path, 1, 1, 1);
                    path.Opacity = 0;
                }

                faces.Parts.Add(path);
                visual.Children.Add(path);

                pathGeometry = new List<PathGeometryParams>();
                pathGeometry.Add(new LineSegmentParams(end));
                pathGeometry.Add(new ArcSegmentParams(new Size(pieParams.OuterRadius, pieParams.OuterRadius), 0, pieParams.IsLargerArc, SweepDirection.Counterclockwise, start));
                pathGeometry.Add(new LineSegmentParams(bevelStart));
                pathGeometry.Add(new ArcSegmentParams(new Size(bevelOuterRadius, bevelOuterRadius), 0, pieParams.IsLargerArc, SweepDirection.Clockwise, bevelEnd));

                path = new Path();
                path.Data = GetPathGeometryFromList(FillRule.Nonzero, bevelEnd, pathGeometry);
                if (pieParams.MeanAngle > 0 && pieParams.MeanAngle < Math.PI)
                {
                    path.Fill = GetCurvedBevelBrush(pieParams.Background, pieParams.MeanAngle * 180 / Math.PI + 90, GetDoubleCollection(-0.745, -0.85), GetDoubleCollection(0, 1));
                }
                else
                {
                    path.Fill = GetCurvedBevelBrush(pieParams.Background, pieParams.MeanAngle * 180 / Math.PI + 90, GetDoubleCollection(0.745, -0.99), GetDoubleCollection(0, 1));
                }

                // Apply animation to the beveling path
                if (pieParams.AnimationEnabled)
                {
                    pieParams.Storyboard = CreateOpacityAnimation(pieParams.Storyboard, path, 1, 1, 1);
                    path.Opacity = 0;
                }

                faces.Parts.Add(path);
                visual.Children.Add(path);

                pathGeometry = new List<PathGeometryParams>();
                pathGeometry.Add(new LineSegmentParams(innerend));
                pathGeometry.Add(new ArcSegmentParams(new Size(pieParams.InnerRadius, pieParams.InnerRadius), 0, pieParams.IsLargerArc, SweepDirection.Counterclockwise, innerstart));
                pathGeometry.Add(new LineSegmentParams(bevelInnerStart));
                pathGeometry.Add(new ArcSegmentParams(new Size(bevelInnerRadius, bevelInnerRadius), 0, pieParams.IsLargerArc, SweepDirection.Clockwise, bevelInnerEnd));

                path = new Path();
                path.Data = GetPathGeometryFromList(FillRule.Nonzero, bevelInnerEnd, pathGeometry);
                if (pieParams.MeanAngle > 0 && pieParams.MeanAngle < Math.PI)
                {
                    path.Fill = GetCurvedBevelBrush(pieParams.Background, pieParams.MeanAngle * 180 / Math.PI + 90, GetDoubleCollection(0.745, -0.99), GetDoubleCollection(0, 1));
                }
                else
                {
                    path.Fill = GetCurvedBevelBrush(pieParams.Background, pieParams.MeanAngle * 180 / Math.PI + 90, GetDoubleCollection(-0.745, -0.85), GetDoubleCollection(0, 1));
                }

                // Apply animation to the beveling path
                if (pieParams.AnimationEnabled)
                {
                    pieParams.Storyboard = CreateOpacityAnimation(pieParams.Storyboard, path, 1, 1, 1);
                    path.Opacity = 0;
                }

                faces.Parts.Add(path);
                visual.Children.Add(path);
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

        private static Path CreateLabelLine(SectorChartShapeParams pieParams, Point centerOfPie, ref PieDoughnut3DPoints unExplodedPoints, ref PieDoughnut3DPoints explodedPoints)
        {
            Path labelLine = null;

            if (pieParams.LabelLineEnabled)
            {
                labelLine = new Path();
                Double meanAngle = pieParams.MeanAngle;

                Point piePoint = new Point();
                piePoint.X = centerOfPie.X + pieParams.OuterRadius * Math.Cos(meanAngle);
                piePoint.Y = centerOfPie.Y + pieParams.OuterRadius * Math.Sin(meanAngle) * pieParams.YAxisScaling;

                Point labelPoint = new Point();
                labelPoint.X = centerOfPie.X + pieParams.LabelPoint.X - pieParams.Width / 2;
                labelPoint.Y = centerOfPie.Y + pieParams.LabelPoint.Y - pieParams.Height / 2;

                Point midPoint = new Point();
                midPoint.X = (labelPoint.X < centerOfPie.X) ? labelPoint.X + 10 : labelPoint.X - 10;
                midPoint.Y = labelPoint.Y;

                List<PathGeometryParams> labelLinePathGeometry = new List<PathGeometryParams>();
                labelLinePathGeometry.Add(new LineSegmentParams(pieParams.AnimationEnabled ? piePoint : midPoint));
                labelLinePathGeometry.Add(new LineSegmentParams(pieParams.AnimationEnabled ? piePoint : labelPoint));
                labelLine.Data = GetPathGeometryFromList(FillRule.Nonzero, piePoint, labelLinePathGeometry);
                PathFigure figure = (labelLine.Data as PathGeometry).Figures[0];
                PathSegmentCollection segments = figure.Segments;
                figure.IsClosed = false;
                figure.IsFilled = false;

                // apply animation to the label line
                if (pieParams.AnimationEnabled)
                {
                    pieParams.Storyboard = CreateLabelLineAnimation(pieParams.Storyboard, segments[0], piePoint, midPoint);
                    pieParams.Storyboard = CreateLabelLineAnimation(pieParams.Storyboard, segments[1], piePoint, midPoint, labelPoint);
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
            }

            return labelLine;
        }

        private static void UpdatePositionLabelInsidePie(SectorChartShapeParams pieParams, Double yOffset)
        {
            if (DataPointRef.LabelStyle == LabelStyles.Inside)
            {
                Point center = new Point();
                center.X = pieParams.Width / 2;
                center.Y = pieParams.Height / 2;

                Double a = 3 * (pieParams.OuterRadius / 4);
                Double b = 3 * (pieParams.OuterRadius / 4) * pieParams.YAxisScaling;
                Double x = center.X + a * Math.Cos(pieParams.MeanAngle);
                Double y = center.Y + b * Math.Sin(pieParams.MeanAngle) + yOffset;

                DataPointRef.LabelVisual.SetValue(Canvas.LeftProperty, x - DataPointRef.LabelVisual.DesiredSize.Width /2);
                DataPointRef.LabelVisual.SetValue(Canvas.TopProperty, y - DataPointRef.LabelVisual.DesiredSize.Height);

                //Ellipse e = new Ellipse() { Height = 3, Width = 3, Fill = new SolidColorBrush(Colors.Yellow) };
                //e.SetValue(Canvas.ZIndexProperty,(Int32) 100000);
                //e.SetValue(Canvas.LeftProperty, x);
                //e.SetValue(Canvas.TopProperty, y);
                //pieFaces.Add(e);
            }
        }


        private static List<Shape> GetPie3D(ref Faces faces, SectorChartShapeParams pieParams, ref Int32 zindex, ref PieDoughnut3DPoints unExplodedPoints, ref PieDoughnut3DPoints explodedPoints, ref Path labelLinePath)
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

            if (pieParams.StartAngle == pieParams.StopAngle && pieParams.IsLargerArc)
            {   
                // draw singleton pie here
                topFace = new Ellipse();
                topFace.Fill = pieParams.Lighting ? Graphics.GetLightingEnabledBrush(pieParams.Background, "Radial", new Double[] { 0.99, 0.745 }) : pieParams.Background;
                //topFace.Stroke = pieParams.Lighting ? Graphics.GetLightingEnabledBrush(pieParams.Background, "Radial", new Double[] { 0.99, 0.745 }) : pieParams.Background;
                topFace.Width = 2 * pieParams.OuterRadius;
                topFace.Height = 2 * pieParams.OuterRadius * pieParams.YAxisScaling;
                topFace.SetValue(Canvas.LeftProperty, (Double)(pieParams.Center.X - topFace.Width / 2));
                topFace.SetValue(Canvas.TopProperty, (Double)(pieParams.Center.Y - topFace.Height / 2 + yOffset));
                pieFaces.Add(topFace);
                faces.Parts.Add(topFace);
                
                bottomFace = new Ellipse();
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
            
            labelLinePath = CreateLabelLine(pieParams, center, ref unExplodedPoints, ref explodedPoints);

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
                    //_elementPositionData.Add(new ElementPositionData(curvedSurface[0],pieParams.StartAngle,0));
                    //_elementPositionData.Add(new ElementPositionData(curvedSurface[1], 0, Math.PI));
                    //_elementPositionData.Add(new ElementPositionData(curvedSurface[2],Math.PI,pieParams.StopAngle));
                    _elementPositionData.Add(new ElementPositionData(curvedSurface[0], 0, Math.PI));
                    _elementPositionData.Add(new ElementPositionData(leftFace, pieParams.StopAngle, pieParams.StopAngle));
                    if (labelLinePath != null)
                        _elementPositionData.Add(new ElementPositionData(labelLinePath, 0, Math.PI));
                }
                else if (pieParams.StartAngle >= 0 && pieParams.StartAngle <= Math.PI && pieParams.StopAngle >= 0 && pieParams.StopAngle <= Math.PI && pieParams.IsLargerArc)
                {
                    _elementPositionData.Add(new ElementPositionData(rightFace, pieParams.StartAngle, pieParams.StartAngle));
                    //_elementPositionData.Add(new ElementPositionData(curvedSurface[0], pieParams.StartAngle, Math.PI));
                    //_elementPositionData.Add(new ElementPositionData(curvedSurface[1], 0, Math.PI));
                    //_elementPositionData.Add(new ElementPositionData(curvedSurface[2], 0, pieParams.StopAngle));
                    _elementPositionData.Add(new ElementPositionData(curvedSurface[0], pieParams.StartAngle, Math.PI));
                    _elementPositionData.Add(new ElementPositionData(curvedSurface[1], 0, pieParams.StopAngle));
                    _elementPositionData.Add(new ElementPositionData(leftFace, pieParams.StopAngle, pieParams.StopAngle));
                    if (labelLinePath != null)
                        labelLinePath.SetValue(Canvas.ZIndexProperty, -50000);
                }
                else if (pieParams.StartAngle >= 0 && pieParams.StartAngle <= Math.PI && pieParams.StopAngle >= Math.PI && pieParams.StopAngle <= Math.PI * 2)
                {
                    _elementPositionData.Add(new ElementPositionData(rightFace, pieParams.StartAngle, pieParams.StartAngle));
                    if (labelLinePath != null)
                        _elementPositionData.Add(new ElementPositionData(labelLinePath, pieParams.StartAngle, Math.PI));
                    //_elementPositionData.Add(new ElementPositionData(curvedSurface[0], pieParams.StartAngle, Math.PI));
                    //_elementPositionData.Add(new ElementPositionData(curvedSurface[1], Math.PI,pieParams.StopAngle));
                    _elementPositionData.Add(new ElementPositionData(curvedSurface[0], pieParams.StartAngle, Math.PI));
                    _elementPositionData.Add(new ElementPositionData(leftFace, pieParams.StopAngle, pieParams.StopAngle));
                }
                else if (pieParams.StartAngle >= Math.PI && pieParams.StartAngle <= Math.PI * 2 && pieParams.StopAngle >= 0 && pieParams.StopAngle <= Math.PI)
                {
                    _elementPositionData.Add(new ElementPositionData(rightFace, pieParams.StartAngle, pieParams.StartAngle));
                    //_elementPositionData.Add(new ElementPositionData(curvedSurface[0], pieParams.StartAngle, 0));
                    //_elementPositionData.Add(new ElementPositionData(curvedSurface[1], 0, pieParams.StopAngle));
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
                        //_elementPositionData.Add(new ElementPositionData(curvedSurface[0], pieParams.StartAngle, pieParams.StopAngle));
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

        private static List<Shape> GetDoughnut3D(ref Faces faces, SectorChartShapeParams pieParams, ref PieDoughnut3DPoints unExplodedPoints, ref PieDoughnut3DPoints explodedPoints, ref Path labelLinePath)
        {
            List<Shape> pieFaces = new List<Shape>();
            Shape topFace = null, bottomFace = null, rightFace = null, leftFace = null;

            // calculate 3d offsets
            Double yOffset = -pieParams.Depth / 2 * pieParams.ZAxisScaling;
            Point center = new Point();
            center.X += pieParams.Width / 2;
            center.Y += pieParams.Height / 2;

            // calculate all points
            Point3D topFaceCenter = new Point3D();
            topFaceCenter.X = center.X;
            topFaceCenter.Y = center.Y + yOffset;
            topFaceCenter.Z = pieParams.OffsetY * Math.Sin(pieParams.StartAngle) * Math.Cos(pieParams.TiltAngle) + pieParams.Depth * Math.Cos(Math.PI / 2 - pieParams.TiltAngle);
            
            Point3D topOuterArcStart = new Point3D();
            topOuterArcStart.X = topFaceCenter.X + pieParams.OuterRadius * Math.Cos(pieParams.StartAngle);
            topOuterArcStart.Y = topFaceCenter.Y + pieParams.OuterRadius * Math.Sin(pieParams.StartAngle) * pieParams.YAxisScaling;
            topOuterArcStart.Z = (topFaceCenter.Y + pieParams.OuterRadius) * Math.Sin(pieParams.StartAngle) * Math.Cos(pieParams.TiltAngle) + pieParams.Depth * Math.Cos(Math.PI / 2 - pieParams.TiltAngle);

            Point3D topOuterArcStop = new Point3D();
            topOuterArcStop.X = topFaceCenter.X + pieParams.OuterRadius * Math.Cos(pieParams.StopAngle);
            topOuterArcStop.Y = topFaceCenter.Y + pieParams.OuterRadius * Math.Sin(pieParams.StopAngle) * pieParams.YAxisScaling;
            topOuterArcStop.Z = (topFaceCenter.Y + pieParams.OuterRadius) * Math.Sin(pieParams.StopAngle) * Math.Cos(pieParams.TiltAngle) + pieParams.Depth * Math.Cos(Math.PI / 2 - pieParams.TiltAngle);

            Point3D topInnerArcStart = new Point3D();
            topInnerArcStart.X = topFaceCenter.X + pieParams.InnerRadius * Math.Cos(pieParams.StartAngle);
            topInnerArcStart.Y = topFaceCenter.Y + pieParams.InnerRadius * Math.Sin(pieParams.StartAngle) * pieParams.YAxisScaling;
            topInnerArcStart.Z = (topFaceCenter.Y + pieParams.InnerRadius) * Math.Sin(pieParams.StartAngle) * Math.Cos(pieParams.TiltAngle) + pieParams.Depth * Math.Cos(Math.PI / 2 - pieParams.TiltAngle);

            Point3D topInnerArcStop = new Point3D();
            topInnerArcStop.X = topFaceCenter.X + pieParams.InnerRadius * Math.Cos(pieParams.StopAngle);
            topInnerArcStop.Y = topFaceCenter.Y + pieParams.InnerRadius * Math.Sin(pieParams.StopAngle) * pieParams.YAxisScaling;
            topInnerArcStop.Z = (topFaceCenter.Y + pieParams.InnerRadius) * Math.Sin(pieParams.StopAngle) * Math.Cos(pieParams.TiltAngle) + pieParams.Depth * Math.Cos(Math.PI / 2 - pieParams.TiltAngle);

            Point3D bottomFaceCenter = new Point3D();
            bottomFaceCenter.X = center.X;
            bottomFaceCenter.Y = center.Y - yOffset;
            bottomFaceCenter.Z = pieParams.OffsetY * Math.Sin(pieParams.StartAngle) * Math.Cos(pieParams.TiltAngle) - pieParams.Depth * Math.Cos(Math.PI / 2 - pieParams.TiltAngle);

            Point3D bottomOuterArcStart = new Point3D();
            bottomOuterArcStart.X = bottomFaceCenter.X + pieParams.OuterRadius * Math.Cos(pieParams.StartAngle);
            bottomOuterArcStart.Y = bottomFaceCenter.Y + pieParams.OuterRadius * Math.Sin(pieParams.StartAngle) * pieParams.YAxisScaling;
            bottomOuterArcStart.Z = (bottomFaceCenter.Y + pieParams.OuterRadius) * Math.Sin(pieParams.StartAngle) * Math.Cos(pieParams.TiltAngle) - pieParams.Depth * Math.Cos(Math.PI / 2 - pieParams.TiltAngle);

            Point3D bottomOuterArcStop = new Point3D();
            bottomOuterArcStop.X = bottomFaceCenter.X + pieParams.OuterRadius * Math.Cos(pieParams.StopAngle);
            bottomOuterArcStop.Y = bottomFaceCenter.Y + pieParams.OuterRadius * Math.Sin(pieParams.StopAngle) * pieParams.YAxisScaling;
            bottomOuterArcStop.Z = (bottomFaceCenter.Y + pieParams.OuterRadius) * Math.Sin(pieParams.StopAngle) * Math.Cos(pieParams.TiltAngle) - pieParams.Depth * Math.Cos(Math.PI / 2 - pieParams.TiltAngle);

            Point3D bottomInnerArcStart = new Point3D();
            bottomInnerArcStart.X = bottomFaceCenter.X + pieParams.InnerRadius * Math.Cos(pieParams.StartAngle);
            bottomInnerArcStart.Y = bottomFaceCenter.Y + pieParams.InnerRadius * Math.Sin(pieParams.StartAngle) * pieParams.YAxisScaling;
            bottomInnerArcStart.Z = (bottomFaceCenter.Y + pieParams.InnerRadius) * Math.Sin(pieParams.StartAngle) * Math.Cos(pieParams.TiltAngle) - pieParams.Depth * Math.Cos(Math.PI / 2 - pieParams.TiltAngle);

            Point3D bottomInnerArcStop = new Point3D();
            bottomInnerArcStop.X = bottomFaceCenter.X + pieParams.InnerRadius * Math.Cos(pieParams.StopAngle);
            bottomInnerArcStop.Y = bottomFaceCenter.Y + pieParams.InnerRadius * Math.Sin(pieParams.StopAngle) * pieParams.YAxisScaling;
            bottomInnerArcStop.Z = (bottomFaceCenter.Y + pieParams.InnerRadius) * Math.Sin(pieParams.StopAngle) * Math.Cos(pieParams.TiltAngle) - pieParams.Depth * Math.Cos(Math.PI / 2 - pieParams.TiltAngle);

            Point3D centroid = GetCentroid(topInnerArcStart, topInnerArcStop, topOuterArcStart, topOuterArcStop, bottomInnerArcStart, bottomInnerArcStop, bottomOuterArcStart, bottomOuterArcStop);

            UpdatePositionLabelInsidePie(pieParams, yOffset);
            if (pieParams.StartAngle == pieParams.StopAngle && pieParams.IsLargerArc)
            {
                // draw singleton pie here
                topFace = new Ellipse();
                topFace.Fill = pieParams.Lighting ? Graphics.GetLightingEnabledBrush(pieParams.Background, "Radial", new Double[] { 0.99, 0.745 }) : pieParams.Background;
                // topFace.Stroke = pieParams.Lighting ? Graphics.GetLightingEnabledBrush(pieParams.Background, "Radial", new Double[] { 0.99, 0.745 }) : pieParams.Background;
                topFace.Width = 2 * pieParams.OuterRadius;
                topFace.Height = 2 * pieParams.OuterRadius * pieParams.YAxisScaling;
                topFace.SetValue(Canvas.LeftProperty, (Double)(pieParams.Center.X - topFace.Width / 2));
                topFace.SetValue(Canvas.TopProperty, (Double)(pieParams.Center.Y - topFace.Height / 2 + yOffset));

                GeometryGroup gg = new GeometryGroup();
                gg.Children.Add(new EllipseGeometry() { Center = new Point(topFace.Width / 2, topFace.Height / 2), RadiusX = topFace.Width, RadiusY = topFace.Height });
                gg.Children.Add(new EllipseGeometry() { Center = new Point(topFace.Width / 2, topFace.Height / 2), RadiusX = pieParams.InnerRadius, RadiusY = topFace.Height - 2 * (pieParams.OuterRadius - pieParams.InnerRadius) });

                topFace.Clip = gg;
                pieFaces.Add(topFace);
                faces.Parts.Add(topFace);

                bottomFace = new Ellipse();
                bottomFace.Fill = pieParams.Lighting ? Graphics.GetLightingEnabledBrush(pieParams.Background, "Radial", new Double[] { 0.99, 0.745 }) : pieParams.Background;
                // topFace.Stroke = pieParams.Lighting ? Graphics.GetLightingEnabledBrush(pieParams.Background, "Radial", new Double[] { 0.99, 0.745 }) : pieParams.Background;
                bottomFace.Width = 2 * pieParams.OuterRadius;
                bottomFace.Height = 2 * pieParams.OuterRadius * pieParams.YAxisScaling;
                bottomFace.SetValue(Canvas.LeftProperty, (Double)(pieParams.Center.X - topFace.Width / 2));
                bottomFace.SetValue(Canvas.TopProperty, (Double)(pieParams.Center.Y - topFace.Height / 2 ));

                gg = new GeometryGroup();
                gg.Children.Add(new EllipseGeometry() { Center = new Point(topFace.Width / 2, topFace.Height / 2), RadiusX = topFace.Width, RadiusY = topFace.Height });
                gg.Children.Add(new EllipseGeometry() { Center = new Point(topFace.Width / 2, topFace.Height / 2), RadiusX = pieParams.InnerRadius, RadiusY = topFace.Height - 2 * (pieParams.OuterRadius - pieParams.InnerRadius) });

                bottomFace.Clip = gg;
                pieFaces.Add(bottomFace);
                faces.Parts.Add(bottomFace);
            }
            else
            {
                topFace = GetDoughnutFace(pieParams, centroid, topInnerArcStart, topInnerArcStop, topOuterArcStart, topOuterArcStop, true);
                pieFaces.Add(topFace);
                faces.Parts.Add(topFace);
                
                bottomFace = GetDoughnutFace(pieParams, centroid, bottomInnerArcStart, bottomInnerArcStop, bottomOuterArcStart, bottomOuterArcStop, false);
                pieFaces.Add(bottomFace);
                faces.Parts.Add(bottomFace);

                rightFace = GetPieSide(pieParams, centroid, topInnerArcStart, bottomInnerArcStart, topOuterArcStart, bottomOuterArcStart);
                pieFaces.Add(rightFace);
                faces.Parts.Add(rightFace);

                leftFace = GetPieSide(pieParams, centroid, topInnerArcStop, bottomInnerArcStop, topOuterArcStop, bottomOuterArcStop);
                pieFaces.Add(leftFace);
                faces.Parts.Add(leftFace);
            }

                List<Shape> curvedSurface = GetDoughnutCurvedFace(pieParams, centroid, topFaceCenter, bottomFaceCenter);
                pieFaces.InsertRange(pieFaces.Count, curvedSurface);

                foreach (FrameworkElement fe in curvedSurface)
                    faces.Parts.Add(fe);

                labelLinePath = CreateLabelLine(pieParams, center, ref unExplodedPoints, ref explodedPoints);

                //Top face ZIndex
                topFace.SetValue(Canvas.ZIndexProperty, (Int32)(50000));
                //BottomFace ZIndex
                bottomFace.SetValue(Canvas.ZIndexProperty, (Int32)(-50000));

                if (!(pieParams.StartAngle == pieParams.StopAngle && pieParams.IsLargerArc))
                {
                    // ZIndex of curved face
                    if (pieParams.StartAngle >= Math.PI && pieParams.StartAngle <= Math.PI * 2 && pieParams.StopAngle >= Math.PI && pieParams.StopAngle <= Math.PI * 2 && pieParams.IsLargerArc)
                    {
                        _elementPositionData.Add(new ElementPositionData(rightFace, pieParams.StartAngle, pieParams.StartAngle));
                        _elementPositionData.Add(new ElementPositionData(curvedSurface[0], 0, Math.PI));
                        _elementPositionData.Add(new ElementPositionData(curvedSurface[1], pieParams.StartAngle, 0));
                        _elementPositionData.Add(new ElementPositionData(curvedSurface[2], Math.PI, pieParams.StopAngle));
                        _elementPositionData.Add(new ElementPositionData(leftFace, pieParams.StopAngle, pieParams.StopAngle));
                        if (labelLinePath != null)
                            _elementPositionData.Add(new ElementPositionData(labelLinePath, 0, Math.PI));
                    }
                    else if (pieParams.StartAngle >= 0 && pieParams.StartAngle <= Math.PI && pieParams.StopAngle >= 0 && pieParams.StopAngle <= Math.PI && pieParams.IsLargerArc)
                    {
                        _elementPositionData.Add(new ElementPositionData(rightFace, pieParams.StartAngle, pieParams.StartAngle));
                        _elementPositionData.Add(new ElementPositionData(curvedSurface[0], pieParams.StartAngle, Math.PI));
                        _elementPositionData.Add(new ElementPositionData(curvedSurface[1], Math.PI * 2, pieParams.StopAngle));
                        _elementPositionData.Add(new ElementPositionData(curvedSurface[2], Math.PI, Math.PI * 2));
                        _elementPositionData.Add(new ElementPositionData(leftFace, pieParams.StopAngle, pieParams.StopAngle));
                        if (labelLinePath != null)
                            labelLinePath.SetValue(Canvas.ZIndexProperty, -50000);
                    }
                    else if (pieParams.StartAngle >= 0 && pieParams.StartAngle <= Math.PI && pieParams.StopAngle >= Math.PI && pieParams.StopAngle <= Math.PI * 2)
                    {
                        _elementPositionData.Add(new ElementPositionData(rightFace, pieParams.StartAngle, pieParams.StartAngle));
                        if (labelLinePath != null)
                            _elementPositionData.Add(new ElementPositionData(labelLinePath, pieParams.StartAngle, Math.PI));
                        _elementPositionData.Add(new ElementPositionData(curvedSurface[0], pieParams.StartAngle, Math.PI));
                        _elementPositionData.Add(new ElementPositionData(curvedSurface[1], Math.PI, pieParams.StopAngle));
                        _elementPositionData.Add(new ElementPositionData(leftFace, pieParams.StopAngle, pieParams.StopAngle));
                    }
                    else if (pieParams.StartAngle >= Math.PI && pieParams.StartAngle <= Math.PI * 2 && pieParams.StopAngle >= 0 && pieParams.StopAngle <= Math.PI)
                    {
                        _elementPositionData.Add(new ElementPositionData(rightFace, pieParams.StartAngle, pieParams.StartAngle));
                        _elementPositionData.Add(new ElementPositionData(curvedSurface[0], 0, pieParams.StopAngle));
                        _elementPositionData.Add(new ElementPositionData(curvedSurface[1], pieParams.StartAngle, 0));
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
                            _elementPositionData.Add(new ElementPositionData(curvedSurface[0], pieParams.StartAngle, pieParams.StopAngle));
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
        
        private static void SetZIndex(FrameworkElement element, ref Int32 zindex1, ref Int32 zindex2, Double angle)
        {
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

        private static List<Shape> GetPieOuterCurvedFace(SectorChartShapeParams pieParams, Point3D centroid, Point3D topFaceCenter, Point3D bottomFaceCenter)
        {
            List<Shape> curvedFaces = new List<Shape>();

            if (pieParams.StartAngle >= Math.PI && pieParams.StartAngle <= Math.PI * 2 && pieParams.StopAngle >= Math.PI && pieParams.StopAngle <= Math.PI * 2 && pieParams.IsLargerArc)
            {
                Path curvedSegment = GetCurvedSegment(pieParams, pieParams.OuterRadius, 0, Math.PI, topFaceCenter, bottomFaceCenter, centroid, true);
                curvedFaces.Add(curvedSegment);
            }
            else if (pieParams.StartAngle >= 0 && pieParams.StartAngle <= Math.PI && pieParams.StopAngle >= 0 && pieParams.StopAngle <= Math.PI && pieParams.IsLargerArc)
            {
                Path curvedSegment = GetCurvedSegment(pieParams, pieParams.OuterRadius, pieParams.StartAngle, Math.PI, topFaceCenter, bottomFaceCenter, centroid, true);
                curvedFaces.Add(curvedSegment);
                curvedSegment = GetCurvedSegment(pieParams, pieParams.OuterRadius, Math.PI * 2, pieParams.StopAngle, topFaceCenter, bottomFaceCenter, centroid, true);
                curvedFaces.Add(curvedSegment);
            }
            else if (pieParams.StartAngle >= 0 && pieParams.StartAngle <= Math.PI && pieParams.StopAngle >= Math.PI && pieParams.StopAngle <= Math.PI * 2)
            {
                Path curvedSegment = GetCurvedSegment(pieParams, pieParams.OuterRadius, pieParams.StartAngle, Math.PI, topFaceCenter, bottomFaceCenter, centroid, true);
                curvedFaces.Add(curvedSegment);
            }
            else if (pieParams.StartAngle >= Math.PI && pieParams.StartAngle <= Math.PI * 2 && pieParams.StopAngle >= 0 && pieParams.StopAngle <= Math.PI)
            {
                Path curvedSegment = GetCurvedSegment(pieParams, pieParams.OuterRadius, 0, pieParams.StopAngle, topFaceCenter, bottomFaceCenter, centroid, true);
                curvedFaces.Add(curvedSegment);
            }
            else
            {
                Path curvedSegment = GetCurvedSegment(pieParams, pieParams.OuterRadius, pieParams.StartAngle, pieParams.StopAngle, topFaceCenter, bottomFaceCenter, centroid, true);
                curvedFaces.Add(curvedSegment);
            }

            return curvedFaces;
        }

        private static Path GetCurvedSegment(SectorChartShapeParams pieParams, Double radius, Double startAngle, Double stopAngle, Point3D topFaceCenter, Point3D bottomFaceCenter, Point3D centroid, Boolean isOuterSide)
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

            Path pieFace = new Path();

            pieFace.Fill = pieParams.Lighting ? Graphics.GetLightingEnabledBrush(pieParams.Background, "Radial", new Double[] { 0.99, 0.745 }) : pieParams.Background;
            // pieFace.Stroke = pieParams.Lighting ? Graphics.GetLightingEnabledBrush(pieParams.Background, "Radial", new Double[] { 0.99, 0.745 }) : pieParams.Background;

            // pieFace.StrokeThickness = 0.15;

            List<PathGeometryParams> pathGeometryList = new List<PathGeometryParams>();

            Boolean isLargeArc = (Math.Abs(stopAngle - startAngle) > Math.PI) ? true : false;
            if (stopAngle < startAngle)
                isLargeArc = (Math.Abs((stopAngle + Math.PI * 2) - startAngle) > Math.PI) ? true : false;

            pathGeometryList.Add(new LineSegmentParams(new Point(topArcStop.X, topArcStop.Y)));
            pathGeometryList.Add(new ArcSegmentParams(new Size(radius, radius * pieParams.YAxisScaling), 0, isLargeArc, SweepDirection.Counterclockwise, new Point(topArcStart.X, topArcStart.Y)));
            pathGeometryList.Add(new LineSegmentParams(new Point(bottomArcStart.X, bottomArcStart.Y)));
            pathGeometryList.Add(new ArcSegmentParams(new Size(radius, radius * pieParams.YAxisScaling), 0, isLargeArc, SweepDirection.Clockwise, new Point(bottomArcStop.X, bottomArcStop.Y)));

            pieFace.Data = GetPathGeometryFromList(FillRule.Nonzero, new Point(bottomArcStop.X, bottomArcStop.Y), pathGeometryList);
            PathFigure figure = (pieFace.Data as PathGeometry).Figures[0];
            PathSegmentCollection segments = figure.Segments;

            // Point3D midPoint = GetFaceZIndex(topArcStart, topArcStop, bottomArcStop, bottomArcStart);
            // pieFace.SetValue(Canvas.ZIndexProperty,(Int32)(midPoint.Z*(isOuterSide?200:200)) );
            
            return pieFace;
        }

        private static Path GetPieFace(SectorChartShapeParams pieParams, Point3D centroid, Point3D center, Point3D arcStart, Point3D arcStop)
        {
            Path pieFace = new Path();

            pieFace.Fill = pieParams.Lighting ? Graphics.GetLightingEnabledBrush(pieParams.Background, "Radial", new Double[] { 0.99, 0.745 }) : pieParams.Background;

            List<PathGeometryParams> pathGeometryList = new List<PathGeometryParams>();

            pathGeometryList.Add(new LineSegmentParams(new Point(arcStop.X, arcStop.Y)));
            pathGeometryList.Add(new ArcSegmentParams(new Size(pieParams.OuterRadius, pieParams.OuterRadius * pieParams.YAxisScaling), 0, pieParams.IsLargerArc, SweepDirection.Counterclockwise, new Point(arcStart.X, arcStart.Y)));
            pathGeometryList.Add(new LineSegmentParams(new Point(center.X, center.Y)));

            pieFace.Data = GetPathGeometryFromList(FillRule.Nonzero, new Point(center.X, center.Y), pathGeometryList);

            PathFigure figure = (pieFace.Data as PathGeometry).Figures[0];
            PathSegmentCollection segments = figure.Segments;

            //Point3D midPoint = GetFaceZIndex(centroid, center, arcStart, arcStop);
            //pieFace.SetValue(Canvas.ZIndexProperty, (Int32)(midPoint.Z*200));
            return pieFace;
        }

        private static Path GetPieSide(SectorChartShapeParams pieParams, Point3D centroid, Point3D centerTop, Point3D centerBottom, Point3D outerTop, Point3D outerBottom)
        {
            Path pieFace = new Path();

            pieFace.Fill = pieParams.Lighting ? Graphics.GetLightingEnabledBrush(pieParams.Background, "Radial", new Double[] { 0.99, 0.745 }) : pieParams.Background;

            List<PathGeometryParams> pathGeometryList = new List<PathGeometryParams>();

            pathGeometryList.Add(new LineSegmentParams(new Point(centerTop.X, centerTop.Y)));
            pathGeometryList.Add(new LineSegmentParams(new Point(outerTop.X, outerTop.Y)));
            pathGeometryList.Add(new LineSegmentParams(new Point(outerBottom.X, outerBottom.Y)));
            pathGeometryList.Add(new LineSegmentParams(new Point(centerBottom.X, centerBottom.Y)));

            pieFace.Data = GetPathGeometryFromList(FillRule.Nonzero, new Point(centerBottom.X, centerBottom.Y), pathGeometryList);
            PathFigure figure = (pieFace.Data as PathGeometry).Figures[0];
            PathSegmentCollection segments = figure.Segments;

            //Point3D midPoint = GetFaceZIndex(centerTop, centerBottom, outerTop,outerBottom);
            //pieFace.SetValue(Canvas.ZIndexProperty, (Int32)(midPoint.Z * 100));
            return pieFace;
        }

        private static Path GetDoughnutFace(SectorChartShapeParams pieParams, Point3D centroid, Point3D arcInnerStart, Point3D arcInnerStop, Point3D arcOuterStart, Point3D arcOuterStop, Boolean isTopFace)
        {
            Path pieFace = new Path();

            pieFace.Fill = pieParams.Lighting ? Graphics.GetLightingEnabledBrush(pieParams.Background, "Radial", new Double[] { 0.99, 0.745 }) : pieParams.Background;

            List<PathGeometryParams> pathGeometryList = new List<PathGeometryParams>();

            pathGeometryList.Add(new LineSegmentParams(new Point(arcOuterStop.X, arcOuterStop.Y)));
            pathGeometryList.Add(new ArcSegmentParams(new Size(pieParams.OuterRadius, pieParams.OuterRadius * pieParams.YAxisScaling), 0, pieParams.IsLargerArc, SweepDirection.Counterclockwise, new Point(arcOuterStart.X, arcOuterStart.Y)));
            pathGeometryList.Add(new LineSegmentParams(new Point(arcInnerStart.X, arcInnerStart.Y)));
            pathGeometryList.Add(new ArcSegmentParams(new Size(pieParams.InnerRadius, pieParams.InnerRadius * pieParams.YAxisScaling), 0, pieParams.IsLargerArc, SweepDirection.Clockwise, new Point(arcInnerStop.X, arcInnerStop.Y)));

            pieFace.Data = GetPathGeometryFromList(FillRule.Nonzero, new Point(arcInnerStop.X, arcInnerStop.Y), pathGeometryList);
            Point3D midPoint = GetFaceZIndex(arcInnerStart, arcInnerStop, arcOuterStart, arcOuterStop);
            if (isTopFace)
                pieFace.SetValue(Canvas.ZIndexProperty, (Int32)(pieParams.Height * 200));
            else
                pieFace.SetValue(Canvas.ZIndexProperty, (Int32)(-pieParams.Height * 200));
            return pieFace;
        }

        private static List<Shape> GetDoughnutCurvedFace(SectorChartShapeParams pieParams, Point3D centroid, Point3D topFaceCenter, Point3D bottomFaceCenter)
        {
            List<Shape> curvedFaces = new List<Shape>();

            if (pieParams.StartAngle >= Math.PI && pieParams.StartAngle <= Math.PI * 2 && pieParams.StopAngle >= Math.PI && pieParams.StopAngle <= Math.PI * 2 && pieParams.IsLargerArc)
            {
                // Outer curved path
                Path curvedSegment = GetCurvedSegment(pieParams, pieParams.OuterRadius, 0, Math.PI, topFaceCenter, bottomFaceCenter, centroid, true);
                curvedFaces.Add(curvedSegment);

                // Inner curved paths
                curvedSegment = GetCurvedSegment(pieParams, pieParams.InnerRadius, pieParams.StartAngle, 0, topFaceCenter, bottomFaceCenter, centroid, false);
                curvedFaces.Add(curvedSegment);

                curvedSegment = GetCurvedSegment(pieParams, pieParams.InnerRadius, Math.PI, pieParams.StopAngle, topFaceCenter, bottomFaceCenter, centroid, false);
                curvedFaces.Add(curvedSegment);
            }
            else if (pieParams.StartAngle >= 0 && pieParams.StartAngle <= Math.PI && pieParams.StopAngle >= 0 && pieParams.StopAngle <= Math.PI && pieParams.IsLargerArc)
            {
                // Outer curved paths
                Path curvedSegment = GetCurvedSegment(pieParams, pieParams.OuterRadius, pieParams.StartAngle, Math.PI, topFaceCenter, bottomFaceCenter, centroid, true);
                curvedFaces.Add(curvedSegment);
                curvedSegment = GetCurvedSegment(pieParams, pieParams.OuterRadius, Math.PI * 2, pieParams.StopAngle, topFaceCenter, bottomFaceCenter, centroid, true);
                curvedFaces.Add(curvedSegment);

                // Inner curved path
                curvedSegment = GetCurvedSegment(pieParams, pieParams.InnerRadius, Math.PI, Math.PI * 2, topFaceCenter, bottomFaceCenter, centroid, false);
                curvedFaces.Add(curvedSegment);
            }
            else if (pieParams.StartAngle >= 0 && pieParams.StartAngle <= Math.PI && pieParams.StopAngle >= Math.PI && pieParams.StopAngle <= Math.PI * 2)
            {
                // Outer curved path
                Path curvedSegment = GetCurvedSegment(pieParams, pieParams.OuterRadius, pieParams.StartAngle, Math.PI, topFaceCenter, bottomFaceCenter, centroid, true);
                curvedFaces.Add(curvedSegment);

                // Inner curved path
                curvedSegment = GetCurvedSegment(pieParams, pieParams.InnerRadius, Math.PI, pieParams.StopAngle, topFaceCenter, bottomFaceCenter, centroid, false);
                curvedFaces.Add(curvedSegment);
            }
            else if (pieParams.StartAngle >= Math.PI && pieParams.StartAngle <= Math.PI * 2 && pieParams.StopAngle >= 0 && pieParams.StopAngle <= Math.PI)
            {
                // Outer curved path
                Path curvedSegment = GetCurvedSegment(pieParams, pieParams.OuterRadius, 0, pieParams.StopAngle, topFaceCenter, bottomFaceCenter, centroid, true);
                curvedFaces.Add(curvedSegment);

                // Inner curved path
                curvedSegment = GetCurvedSegment(pieParams, pieParams.InnerRadius, pieParams.StartAngle, 0, topFaceCenter, bottomFaceCenter, centroid, false);
                curvedFaces.Add(curvedSegment);
            }
            else
            {
                if (pieParams.StartAngle >= 0 && pieParams.StopAngle <= Math.PI)
                {
                    Path curvedSegment = GetCurvedSegment(pieParams, pieParams.OuterRadius, pieParams.StartAngle, pieParams.StopAngle, topFaceCenter, bottomFaceCenter, centroid, true);
                    curvedFaces.Add(curvedSegment);
                }
                else
                {
                    Path curvedSegment = curvedSegment = GetCurvedSegment(pieParams, pieParams.InnerRadius, pieParams.StartAngle, pieParams.StopAngle, topFaceCenter, bottomFaceCenter, centroid, false);
                    curvedFaces.Add(curvedSegment);
                }
            }

            return curvedFaces;
        }

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

        private static Point3D GetFaceZIndex(params Point3D[] points)
        {
            return GetCentroid(points);
        }

        private static PathGeometry GetPathGeometryFromList(FillRule fillRule, Point startPoint, List<PathGeometryParams> pathGeometryParams)
        {
            PathGeometry pathGeometry = new PathGeometry();

            pathGeometry.FillRule = fillRule;
            pathGeometry.Figures = new PathFigureCollection();

            PathFigure pathFigure = new PathFigure();

            pathFigure.StartPoint = startPoint;
            pathFigure.Segments = new PathSegmentCollection();
            pathFigure.IsClosed = true;

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

        private static Brush GetPieGradianceBrush()
        {
            RadialGradientBrush brush = new RadialGradientBrush() { GradientOrigin = new Point(0.5, 0.5) };
            brush.GradientStops = new GradientStopCollection();

            brush.GradientStops.Add(new GradientStop() { Offset = 0, Color = Color.FromArgb((Byte)0, (Byte)0, (Byte)0, (Byte)0) });
            brush.GradientStops.Add(new GradientStop() { Offset = 0.7, Color = Color.FromArgb((Byte)34, (Byte)0, (Byte)0, (Byte)0) });
            brush.GradientStops.Add(new GradientStop() { Offset = 1, Color = Color.FromArgb((Byte)127, (Byte)0, (Byte)0, (Byte)0) });

            return brush;
        }

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
        
        internal static Brush GetLighterBevelBrush(Brush brush, Double angle)
        {
            return Graphics.GetBevelTopBrush(brush, angle);
        }

        internal static Brush GetDarkerBevelBrush(Brush brush, Double angle)
        {
            return Graphics.GetLightingEnabledBrush(brush, "Linear", new Double[] { 0.35, 0.65 });
        }

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

        internal static DoubleCollection GetDoubleCollection(params double[] values)
        {
            DoubleCollection collection = new DoubleCollection();
            foreach (double value in values)
            {
                collection.Add(value);
            }
            return collection;
        }

        internal class PathGeometryParams
        {
            #region Public Methods
            public PathGeometryParams(Point endPoint)
            {
                EndPoint = endPoint;
            }
            #endregion Public Methods

            #region Public Properties
            public Point EndPoint
            {
                get;
                set;
            }
            #endregion

        }

        internal class LineSegmentParams : PathGeometryParams
        {
            #region Public Methods
            public LineSegmentParams(Point endPoint)
                : base(endPoint)
            {
            }
            #endregion Public Methods
        }

        internal class ArcSegmentParams : PathGeometryParams
        {
            #region Public Methods
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

            public Size Size
            {
                get;
                set;
            }

            public Double RotationAngle
            {
                get;
                set;
            }

            public Boolean IsLargeArc
            {
                get;
                set;
            }

            public SweepDirection SweepDirection
            {
                get;
                set;
            }

            #endregion Public
        }
        
        private static PointCollection GenerateDoubleCollection(params Point[] values)
        {
            PointCollection collection = new PointCollection();
            foreach (Point value in values)
                collection.Add(value);
            return collection;
        }

        private static List<KeySpline> GenerateKeySplineList(params Point[] values)
        {
            List<KeySpline> splines = new List<KeySpline>();
            for (Int32 i = 0; i < values.Length; i += 2)
                splines.Add(Graphics.GetKeySpline(values[i], values[i + 1]));

            return splines;
        }

        private static PointAnimationUsingKeyFrames CreatePointAnimation(DependencyObject target, String property, Double beginTime, List<Double> frameTime, List<Point> values, List<KeySpline> splines)
        {
            PointAnimationUsingKeyFrames da = new PointAnimationUsingKeyFrames();
#if WPF
            target.SetValue(FrameworkElement.NameProperty, target.GetType().Name + target.GetHashCode().ToString());
            Storyboard.SetTargetName(da, target.GetValue(FrameworkElement.NameProperty).ToString());

            DataSeriesRef.RegisterName((string)target.GetValue(FrameworkElement.NameProperty), target);
            DataPointRef.RegisterName((string)target.GetValue(FrameworkElement.NameProperty), target);
#else
            Storyboard.SetTarget(da, target);
#endif
            Storyboard.SetTargetProperty(da, new PropertyPath(property));

            da.BeginTime = TimeSpan.FromSeconds(beginTime);

            for (Int32 index = 0; index < splines.Count; index++)
            {
                SplinePointKeyFrame keyFrame = new SplinePointKeyFrame();
                keyFrame.KeySpline = splines[index];
                keyFrame.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(frameTime[index]));
                keyFrame.Value = values[index];
                da.KeyFrames.Add(keyFrame);
            }

            return da;
        }

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

        private static List<Double> GenerateAnimationFrames(int count, double maxTime)
        {
            List<double> frames = new List<double>();
            for (int i = 0; i < count; i++)
            {
                frames.Add(maxTime * i / (double)(count - 1));
            }
            return frames;
        }

        private static Storyboard CreatePathSegmentAnimation(Storyboard storyboard, PathSegment target, Point center, Double radius, Double startAngle, Double stopAngle)
        {
            List<Point> points = GenerateAnimationPoints(center, radius, startAngle, stopAngle);
            List<Double> frames = GenerateAnimationFrames(points.Count, 1);
            List<KeySpline> splines = Graphics.GenerateAnimationSplines(points.Count);

            PointAnimationUsingKeyFrames pieSliceAnimation = null;

            if (typeof(ArcSegment).IsInstanceOfType(target))
            {
                pieSliceAnimation = CreatePointAnimation(target, "(ArcSegment.Point)", 0.5, frames, points, splines);
            }
            else
            {
                pieSliceAnimation = CreatePointAnimation(target, "(LineSegment.Point)", 0.5, frames, points, splines);
            }

            storyboard.Stop();
            storyboard.Children.Add(pieSliceAnimation);
            return storyboard;
        }

        private static Storyboard CreatePathFigureAnimation(Storyboard storyboard, PathFigure target, Point center, Double radius, Double startAngle, Double stopAngle)
        {
            List<Point> points = GenerateAnimationPoints(center, radius, startAngle, stopAngle);
            List<Double> frames = GenerateAnimationFrames(points.Count, 1);
            List<KeySpline> splines = Graphics.GenerateAnimationSplines(points.Count);

            PointAnimationUsingKeyFrames pieSliceAnimation = CreatePointAnimation(target, "(PathFigure.StartPoint)", 0.5, frames, points, splines);

            storyboard.Stop();
            storyboard.Children.Add(pieSliceAnimation);
            return storyboard;
        }

        private static Storyboard CreateLabelLineAnimation(Storyboard storyboard, PathSegment target, params Point[] points)
        {
            List<Point> pointsList = points.ToList();
            List<Double> frames = GenerateAnimationFrames(pointsList.Count, 1);
            List<KeySpline> splines = Graphics.GenerateAnimationSplines(pointsList.Count);

            PointAnimationUsingKeyFrames labelLineAnimation = CreatePointAnimation(target, "(LineSegment.Point)", 1 + 0.5, frames, pointsList, splines);

            storyboard.Stop();
            storyboard.Children.Add(labelLineAnimation);
            return storyboard;
        }

        private static DoubleAnimationUsingKeyFrames CreateDoubleAnimation(DependencyObject target, String property, Double beginTime, DoubleCollection frameTime, DoubleCollection values, List<KeySpline> splines)
        {
            DoubleAnimationUsingKeyFrames da = new DoubleAnimationUsingKeyFrames();
#if WPF
            target.SetValue(FrameworkElement.NameProperty, target.GetType().Name + target.GetHashCode().ToString());
            Storyboard.SetTargetName(da, target.GetValue(FrameworkElement.NameProperty).ToString());

            DataSeriesRef.RegisterName((string)target.GetValue(FrameworkElement.NameProperty), target);
            DataPointRef.RegisterName((string)target.GetValue(FrameworkElement.NameProperty), target);
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

        private static Storyboard CreateOpacityAnimation(Storyboard storyboard, DependencyObject target, Double beginTime, Double opacity, Double duration)
        {
            DoubleCollection values = Graphics.GenerateDoubleCollection(0, opacity);
            DoubleCollection frames = Graphics.GenerateDoubleCollection(0, duration);
            List<KeySpline> splines = Graphics.GenerateAnimationSplines(frames.Count);
            DoubleAnimationUsingKeyFrames opacityAnimation = CreateDoubleAnimation(target, "(UIElement.Opacity)", beginTime + 0.5, frames, values, splines);
            storyboard.Stop();
            storyboard.Children.Add(opacityAnimation);
            return storyboard;
        }

        private static Storyboard CreateLabelLineInteractivityAnimation(Storyboard storyboard, PathSegment target, params Point[] points)
        {
            List<Point> pointsList = points.ToList();
            List<Double> frames = GenerateAnimationFrames(pointsList.Count, 0.4);
            List<KeySpline> splines = Graphics.GenerateAnimationSplines(pointsList.Count);

            PointAnimationUsingKeyFrames labelLineAnimation = CreatePointAnimation(target, "(LineSegment.Point)", 0, frames, pointsList, splines);
            storyboard.Stop();
            storyboard.Children.Add(labelLineAnimation);

            return storyboard;
        }

        private static Storyboard CreateExplodingOut2DAnimation(DataPoint dataPoint, Storyboard storyboard, Panel visual, Panel label, Path labelLine, TranslateTransform translateTransform, PieDoughnut2DPoints unExplodedPoints, PieDoughnut2DPoints explodedPoints, Double xOffset, Double yOffset)
        {
            storyboard.Stop();

            #region Animating Silce
            DoubleCollection values = Graphics.GenerateDoubleCollection(0, xOffset);
            DoubleCollection frames = Graphics.GenerateDoubleCollection(0, 0.4);
            List<KeySpline> splines = GenerateKeySplineList
                (
                    new Point(0, 0), new Point(1, 1),
                    new Point(0, 0), new Point(0, 1)
                );

            DoubleAnimationUsingKeyFrames sliceXAnimation = CreateDoubleAnimation(translateTransform, "(TranslateTransform.X)", 0, frames, values, splines);

            values = Graphics.GenerateDoubleCollection(0, yOffset);
            frames = Graphics.GenerateDoubleCollection(0, 0.4);
            splines = GenerateKeySplineList
                (
                    new Point(0, 0), new Point(1, 1),
                    new Point(0, 0), new Point(0, 1)
                );

            DoubleAnimationUsingKeyFrames sliceYAnimation = CreateDoubleAnimation(translateTransform, "(TranslateTransform.Y)", 0, frames, values, splines);

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
                    splines = GenerateKeySplineList
                        (
                            new Point(0, 0), new Point(1, 1),
                            new Point(0, 0), new Point(0, 1)
                        );

                    DoubleAnimationUsingKeyFrames labelXAnimation = CreateDoubleAnimation(translateTransform, "(TranslateTransform.X)", 0, frames, values, splines);

                    values = Graphics.GenerateDoubleCollection(0, yOffset);
                    frames = Graphics.GenerateDoubleCollection(0, 0.4);
                    splines = GenerateKeySplineList
                        (
                            new Point(0, 0), new Point(1, 1),
                            new Point(0, 0), new Point(0, 1)
                        );

                    DoubleAnimationUsingKeyFrames labelYAnimation = CreateDoubleAnimation(translateTransform, "(TranslateTransform.Y)", 0, frames, values, splines);

                    storyboard.Children.Add(labelXAnimation);
                    storyboard.Children.Add(labelYAnimation);
                }
            }
            else
            {
                values = Graphics.GenerateDoubleCollection(unExplodedPoints.LabelPosition.X, explodedPoints.LabelPosition.X);
                frames = Graphics.GenerateDoubleCollection(0, 0.4);
                splines = GenerateKeySplineList
                    (
                        new Point(0, 0), new Point(1, 1),
                        new Point(0, 0), new Point(0, 1)
                    );

                DoubleAnimationUsingKeyFrames labelXAnimation = CreateDoubleAnimation(label, "(Canvas.Left)", 0, frames, values, splines);
                storyboard.Children.Add(labelXAnimation);
            }

            #endregion Animating Label

            #region Animating Label Line
            if (labelLine != null)
            {
                PathFigure figure = (labelLine.Data as PathGeometry).Figures[0];
                PathSegmentCollection segments = figure.Segments;
                storyboard = CreateLabelLineInteractivityAnimation(storyboard, segments[0], unExplodedPoints.LabelLineMidPoint, explodedPoints.LabelLineMidPoint);
                storyboard = CreateLabelLineInteractivityAnimation(storyboard, segments[1], unExplodedPoints.LabelLineEndPoint, explodedPoints.LabelLineEndPoint);
            }

            #endregion Animating Label Line

            return storyboard;
        }

        private static Storyboard CreateExplodingIn2DAnimation(DataPoint dataPoint, Storyboard storyboard, Panel visual, Panel label, Path labelLine, TranslateTransform translateTransform, PieDoughnut2DPoints unExplodedPoints, PieDoughnut2DPoints explodedPoints, Double xOffset, Double yOffset)
        {
            storyboard.Stop();

            #region Animating Silce
            DoubleCollection values = Graphics.GenerateDoubleCollection(xOffset, 0);
            DoubleCollection frames = Graphics.GenerateDoubleCollection(0, 0.4);
            List<KeySpline> splines = GenerateKeySplineList
                (
                    new Point(0, 0), new Point(1, 1),
                    new Point(0, 0), new Point(0, 1)
                );

            DoubleAnimationUsingKeyFrames sliceXAnimation = CreateDoubleAnimation(translateTransform, "(TranslateTransform.X)", 0, frames, values, splines);

            values = Graphics.GenerateDoubleCollection(yOffset, 0);
            frames = Graphics.GenerateDoubleCollection(0, 0.4);
            splines = GenerateKeySplineList
                (
                    new Point(0, 0), new Point(1, 1),
                    new Point(0, 0), new Point(0, 1)
                );

            DoubleAnimationUsingKeyFrames sliceYAnimation = CreateDoubleAnimation(translateTransform, "(TranslateTransform.Y)", 0, frames, values, splines);

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
                    splines = GenerateKeySplineList
                    (
                        new Point(0, 0), new Point(1, 1),
                        new Point(0, 0), new Point(0, 1)
                    );

                    DoubleAnimationUsingKeyFrames labelXAnimation = CreateDoubleAnimation(translateTransform, "(TranslateTransform.X)", 0, frames, values, splines);

                    values = Graphics.GenerateDoubleCollection(yOffset, 0);
                    frames = Graphics.GenerateDoubleCollection(0, 0.4);
                    splines = GenerateKeySplineList
                    (
                        new Point(0, 0), new Point(1, 1),
                        new Point(0, 0), new Point(0, 1)
                    );

                    DoubleAnimationUsingKeyFrames labelYAnimation = CreateDoubleAnimation(translateTransform, "(TranslateTransform.Y)", 0, frames, values, splines);

                    storyboard.Children.Add(labelXAnimation);
                    storyboard.Children.Add(labelYAnimation);
                }
            }
            else
            {
                values = Graphics.GenerateDoubleCollection(explodedPoints.LabelPosition.X, unExplodedPoints.LabelPosition.X);
                frames = Graphics.GenerateDoubleCollection(0, 0.4);
                splines = GenerateKeySplineList
                    (
                        new Point(0, 0), new Point(1, 1),
                        new Point(0, 0), new Point(0, 1)
                    );

                DoubleAnimationUsingKeyFrames labelXAnimation = CreateDoubleAnimation(label, "(Canvas.Left)", 0, frames, values, splines);
                storyboard.Children.Add(labelXAnimation);
            }

            #endregion Animating Label

            #region Animating Label Line

            if (labelLine != null)
            {
                PathFigure figure = (labelLine.Data as PathGeometry).Figures[0];
                PathSegmentCollection segments = figure.Segments;
                storyboard = CreateLabelLineInteractivityAnimation(storyboard, segments[0], explodedPoints.LabelLineMidPoint, unExplodedPoints.LabelLineMidPoint);
                storyboard = CreateLabelLineInteractivityAnimation(storyboard, segments[1], explodedPoints.LabelLineEndPoint, unExplodedPoints.LabelLineEndPoint);
            }

            #endregion Animating Label Line
            return storyboard;
        }

        private static Storyboard CreateExplodingOut3DAnimation(DataPoint dataPoint, Storyboard storyboard, List<Shape> pathElements, Panel label, Path labelLine, PieDoughnut3DPoints unExplodedPoints, PieDoughnut3DPoints explodedPoints, Double xOffset, Double yOffset)
        {
            DoubleCollection values;
            DoubleCollection frames;
            List<KeySpline> splines;

            storyboard.Stop();

            #region Animating Slice

            foreach (Shape path in pathElements)
            {
                if (path == null) continue;
                TranslateTransform translateTransform = path.RenderTransform as TranslateTransform;

                values = Graphics.GenerateDoubleCollection(0, xOffset);
                frames = Graphics.GenerateDoubleCollection(0, 0.4);
                splines = GenerateKeySplineList
                    (
                        new Point(0, 0), new Point(1, 1),
                        new Point(0, 0), new Point(0, 1)
                    );

                DoubleAnimationUsingKeyFrames sliceXAnimation = CreateDoubleAnimation(translateTransform, "(TranslateTransform.X)", 0, frames, values, splines);

                values = Graphics.GenerateDoubleCollection(0, yOffset);
                frames = Graphics.GenerateDoubleCollection(0, 0.4);
                splines = GenerateKeySplineList
                    (
                        new Point(0, 0), new Point(1, 1),
                        new Point(0, 0), new Point(0, 1)
                    );

                DoubleAnimationUsingKeyFrames sliceYAnimation = CreateDoubleAnimation(translateTransform, "(TranslateTransform.Y)", 0, frames, values, splines);

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
                    splines = GenerateKeySplineList
                        (
                            new Point(0, 0), new Point(1, 1),
                            new Point(0, 0), new Point(0, 1)
                        );

                    DoubleAnimationUsingKeyFrames sliceXAnimation1 = CreateDoubleAnimation(translateTransform, "(TranslateTransform.X)", 0, frames, values, splines);

                    values = Graphics.GenerateDoubleCollection(0, yOffset);
                    frames = Graphics.GenerateDoubleCollection(0, 0.4);
                    splines = GenerateKeySplineList
                        (
                            new Point(0, 0), new Point(1, 1),
                            new Point(0, 0), new Point(0, 1)
                        );

                    DoubleAnimationUsingKeyFrames sliceYAnimation2 = CreateDoubleAnimation(translateTransform, "(TranslateTransform.Y)", 0, frames, values, splines);

                    storyboard.Children.Add(sliceXAnimation1);
                    storyboard.Children.Add(sliceYAnimation2);
                }
            }
            else
            {
                values = Graphics.GenerateDoubleCollection(unExplodedPoints.LabelPosition.X, explodedPoints.LabelPosition.X);
                frames = Graphics.GenerateDoubleCollection(0, 0.4);
                splines = GenerateKeySplineList
                    (
                        new Point(0, 0), new Point(1, 1),
                        new Point(0, 0), new Point(0, 1)
                    );

                DoubleAnimationUsingKeyFrames labelXAnimation = CreateDoubleAnimation(label, "(Canvas.Left)", 0, frames, values, splines);
                storyboard.Children.Add(labelXAnimation);
            }

            #endregion Animating Label

            #region Animating Label Line
            if (labelLine != null )
            {
                TranslateTransform translateTransform = labelLine.RenderTransform as TranslateTransform;

                values = Graphics.GenerateDoubleCollection(0, xOffset);
                frames = Graphics.GenerateDoubleCollection(0, 0.4);
                splines = GenerateKeySplineList
                    (
                        new Point(0, 0), new Point(1, 1),
                        new Point(0, 0), new Point(0, 1)
                    );

                DoubleAnimationUsingKeyFrames sliceXAnimation = CreateDoubleAnimation(translateTransform, "(TranslateTransform.X)", 0, frames, values, splines);

                values = Graphics.GenerateDoubleCollection(0, yOffset);
                frames = Graphics.GenerateDoubleCollection(0, 0.4);
                splines = GenerateKeySplineList
                    (
                        new Point(0, 0), new Point(1, 1),
                        new Point(0, 0), new Point(0, 1)
                    );

                DoubleAnimationUsingKeyFrames sliceYAnimation = CreateDoubleAnimation(translateTransform, "(TranslateTransform.Y)", 0, frames, values, splines);

                storyboard.Children.Add(sliceXAnimation);
                storyboard.Children.Add(sliceYAnimation);


                PathFigure figure = (labelLine.Data as PathGeometry).Figures[0];
                PathSegmentCollection segments = figure.Segments;
                storyboard = CreateLabelLineInteractivityAnimation(storyboard, segments[0], unExplodedPoints.LabelLineMidPoint, explodedPoints.LabelLineMidPoint);
                storyboard = CreateLabelLineInteractivityAnimation(storyboard, segments[1], unExplodedPoints.LabelLineEndPoint, explodedPoints.LabelLineEndPoint);
            }
            #endregion Animating Label Line

            return storyboard;
        }

        private static Storyboard CreateExplodingIn3DAnimation(DataPoint dataPoint, Storyboard storyboard, List<Shape> pathElements, Panel label, Path labelLine, PieDoughnut3DPoints unExplodedPoints, PieDoughnut3DPoints explodedPoints, Double xOffset, Double yOffset)
        {
            DoubleCollection values;
            DoubleCollection frames;
            List<KeySpline> splines;

            if (storyboard != null)
                storyboard.Stop();

            #region Animating Slice

            foreach (Shape path in pathElements)
            {
                if (path == null) continue;

                TranslateTransform translateTransform = path.RenderTransform as TranslateTransform;

                values = Graphics.GenerateDoubleCollection(xOffset, 0);
                frames = Graphics.GenerateDoubleCollection(0, 0.4);
                splines = GenerateKeySplineList
                    (
                        new Point(0, 0), new Point(1, 1),
                        new Point(0, 0), new Point(0, 1)
                    );

                DoubleAnimationUsingKeyFrames sliceXAnimation = CreateDoubleAnimation(translateTransform, "(TranslateTransform.X)", 0, frames, values, splines);

                values = Graphics.GenerateDoubleCollection(yOffset, 0);
                frames = Graphics.GenerateDoubleCollection(0, 0.4);
                splines = GenerateKeySplineList
                    (
                        new Point(0, 0), new Point(1, 1),
                        new Point(0, 0), new Point(0, 1)
                    );

                DoubleAnimationUsingKeyFrames sliceYAnimation = CreateDoubleAnimation(translateTransform, "(TranslateTransform.Y)", 0, frames, values, splines);

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
                    splines = GenerateKeySplineList
                        (
                            new Point(0, 0), new Point(1, 1),
                            new Point(0, 0), new Point(0, 1)
                        );

                    DoubleAnimationUsingKeyFrames labelXAnimation1 = CreateDoubleAnimation(translateTransform, "(TranslateTransform.X)", 0, frames, values, splines);

                    values = Graphics.GenerateDoubleCollection(yOffset, 0);
                    frames = Graphics.GenerateDoubleCollection(0, 0.4);
                    splines = GenerateKeySplineList
                        (
                            new Point(0, 0), new Point(1, 1),
                            new Point(0, 0), new Point(0, 1)
                        );

                    DoubleAnimationUsingKeyFrames labelYAnimation2 = CreateDoubleAnimation(translateTransform, "(TranslateTransform.Y)", 0, frames, values, splines);

                    storyboard.Children.Add(labelXAnimation1);
                    storyboard.Children.Add(labelYAnimation2);
                }
            }
            else
            {
                values = Graphics.GenerateDoubleCollection(explodedPoints.LabelPosition.X, unExplodedPoints.LabelPosition.X);
                frames = Graphics.GenerateDoubleCollection(0, 0.4);
                splines = GenerateKeySplineList
                    (
                        new Point(0, 0), new Point(1, 1),
                        new Point(0, 0), new Point(0, 1)
                    );

                DoubleAnimationUsingKeyFrames labelXAnimation = CreateDoubleAnimation(label, "(Canvas.Left)", 0, frames, values, splines);
                storyboard.Children.Add(labelXAnimation);
            }

            #endregion Animating Label

            #region Animating Label Line
            if (labelLine != null)
            {
                TranslateTransform translateTransform = labelLine.RenderTransform as TranslateTransform;

                values = Graphics.GenerateDoubleCollection(xOffset, 0);
                frames = Graphics.GenerateDoubleCollection(0, 0.4);
                splines = GenerateKeySplineList
                    (
                        new Point(0, 0), new Point(1, 1),
                        new Point(0, 0), new Point(0, 1)
                    );

                DoubleAnimationUsingKeyFrames sliceXAnimation = CreateDoubleAnimation(translateTransform, "(TranslateTransform.X)", 0, frames, values, splines);

                values = Graphics.GenerateDoubleCollection(yOffset, 0);
                frames = Graphics.GenerateDoubleCollection(0, 0.4);
                splines = GenerateKeySplineList
                    (
                        new Point(0, 0), new Point(1, 1),
                        new Point(0, 0), new Point(0, 1)
                    );

                DoubleAnimationUsingKeyFrames sliceYAnimation = CreateDoubleAnimation(translateTransform, "(TranslateTransform.Y)", 0, frames, values, splines);

                storyboard.Children.Add(sliceXAnimation);
                storyboard.Children.Add(sliceYAnimation);


                PathFigure figure = (labelLine.Data as PathGeometry).Figures[0];
                PathSegmentCollection segments = figure.Segments;
                storyboard = CreateLabelLineInteractivityAnimation(storyboard, segments[0], explodedPoints.LabelLineMidPoint, unExplodedPoints.LabelLineMidPoint);
                storyboard = CreateLabelLineInteractivityAnimation(storyboard, segments[1], explodedPoints.LabelLineEndPoint, unExplodedPoints.LabelLineEndPoint);
            }
            #endregion Animating Label Line

            return storyboard;
        }

        private static DataSeries DataSeriesRef
        {
            get;
            set;
        }

        private static DataPoint DataPointRef
        {
            get;
            set;
        }

        internal static Canvas GetVisualObjectForPieChart(Double width, Double height, PlotDetails plotDetails, List<DataSeries> seriesList, Chart chart, bool animationEnabled)
        {
            if (Double.IsNaN(width) || Double.IsNaN(height) || width <= 0 || height <= 0) return null;

            Debug.WriteLine("PieStart: " + DateTime.Now.ToLongTimeString());

            Canvas visual = new Canvas();
            visual.Width = width;
            visual.Height = height;
            DataSeries series = seriesList[0];

            if (series.Enabled == false)
                return visual;

            List<DataPoint> enabledDataPoints = (from datapoint in series.DataPoints where datapoint.Enabled == true && datapoint.YValue != 0 select datapoint).ToList();
            Double absoluteSum = plotDetails.GetAbsoluteSumOfDataPoints(enabledDataPoints);
            absoluteSum = (absoluteSum == 0) ? 1 : absoluteSum;

            Double centerX = width / 2;
            Double centerY = height / 2;

            Double offsetX = 0;
            Double offsetY = 0;
            Boolean IsLabelEnabled;
            Size pieCanvasSize = new Size();

            Canvas labelCanvas = CreateAndPositionLabels(absoluteSum, enabledDataPoints, width, height, ((chart.View3D) ? 0.4 : 1), chart.View3D, ref pieCanvasSize);

            Debug.WriteLine("Labels Positioning over: " + DateTime.Now.ToLongTimeString());

            if (labelCanvas == null)
                IsLabelEnabled = false;
            else
            {
                IsLabelEnabled = true;
                labelCanvas.SetValue(Canvas.ZIndexProperty, 50001);
                labelCanvas.IsHitTestVisible = false;
            }

            Double radius = Math.Min(pieCanvasSize.Width, pieCanvasSize.Height) / (chart.View3D ? 1 : 2);
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

            DataSeriesRef = series;

            SectorChartShapeParams pieParams = null;

            Int32 labelStyleCounter = 0;

            if (!chart.View3D)
            {
                foreach (DataPoint dataPoint in enabledDataPoints)
                {   
                    if (dataPoint.LabelStyle == LabelStyles.Inside)
                        labelStyleCounter++;
                }
            }

            foreach (DataPoint dataPoint in enabledDataPoints)
            {
                DataPointRef = dataPoint;

                if (Double.IsNaN(dataPoint.YValue) || dataPoint.YValue == 0)
                    continue;

                absoluteYValue = Math.Abs(dataPoint.YValue);

                angle = (absoluteYValue / absoluteSum) * Math.PI * 2;

                endAngle = startAngle + angle;
                meanAngle = (startAngle + endAngle) / 2;

                pieParams = new SectorChartShapeParams();

                dataPoint.VisualParams = pieParams;

                pieParams.Storyboard = series.Storyboard;
                pieParams.AnimationEnabled = animationEnabled;
                pieParams.Center = new Point(centerX, centerY);
                pieParams.ExplodeRatio = 0.2;
                pieParams.InnerRadius = 0;
                pieParams.OuterRadius = radius;
                pieParams.StartAngle = (startAngle) % (Math.PI * 2);
                pieParams.StopAngle = (endAngle) % (Math.PI * 2);
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
                pieParams.IsZero = (dataPoint.YValue == 0);

                offsetX = radius * pieParams.ExplodeRatio * Math.Cos(meanAngle);
                offsetY = radius * pieParams.ExplodeRatio * Math.Sin(meanAngle);
                pieParams.OffsetX = offsetX;
                pieParams.OffsetY = offsetY * (chart.View3D ? pieParams.YAxisScaling : 1);

                if (dataPoint.LabelVisual != null)
                {
                    if ((Double)dataPoint.LabelVisual.GetValue(Canvas.LeftProperty) < width / 2)
                    {
                        pieParams.LabelPoint = new Point((Double)dataPoint.LabelVisual.GetValue(Canvas.LeftProperty) + dataPoint.LabelVisual.DesiredSize.Width, (Double)dataPoint.LabelVisual.GetValue(Canvas.TopProperty) + dataPoint.LabelVisual.DesiredSize.Height / 2);
                    }
                    else
                    {
                        pieParams.LabelPoint = new Point((Double)dataPoint.LabelVisual.GetValue(Canvas.LeftProperty), (Double)dataPoint.LabelVisual.GetValue(Canvas.TopProperty) + dataPoint.LabelVisual.DesiredSize.Height / 2);
                    }

                    // apply animation to the labels
                    if (animationEnabled)
                    {
                        series.Storyboard = CreateOpacityAnimation(series.Storyboard, dataPoint.LabelVisual, 2, 1, 0.5);
                        dataPoint.LabelVisual.Opacity = 0;
                    }
                }

                Faces faces = new Faces();
                faces.Parts = new List<FrameworkElement>();

                #region View3D = true

                if (chart.View3D)
                {
                    PieDoughnut3DPoints unExplodedPoints = new PieDoughnut3DPoints();
                    PieDoughnut3DPoints explodedPoints = new PieDoughnut3DPoints();
                    List<Shape> pieFaces = GetPie3D(ref faces, pieParams, ref zindex, ref unExplodedPoints, ref explodedPoints, ref dataPoint._labelLine);

                    foreach (Shape path in pieFaces)
                    {   
                        if (path != null)
                        {   
                            visual.Children.Add(path);
                            faces.VisualComponents.Add(path);
                            path.RenderTransform = new TranslateTransform();
                            // apply animation to the 3D sections
                            if (animationEnabled)
                            {
                                series.Storyboard = CreateOpacityAnimation(series.Storyboard, path, 1.0 / (series.DataPoints.Count) * (series.DataPoints.IndexOf(dataPoint)), dataPoint.Opacity, 0.5);
                                path.Opacity = 0;
                            }
                        }

                    }

                    if (dataPoint._labelLine != null && pieParams.LabelLineEnabled)
                    {
                        dataPoint._labelLine.RenderTransform = new TranslateTransform();
                        visual.Children.Add(dataPoint._labelLine);
                        faces.VisualComponents.Add(dataPoint._labelLine);
                    }

                    faces.Visual = visual;

                    if (dataPoint.LabelVisual != null)
                    {
                        unExplodedPoints.LabelPosition = new Point((Double)dataPoint.LabelVisual.GetValue(Canvas.LeftProperty), (Double)dataPoint.LabelVisual.GetValue(Canvas.TopProperty));

                        if ((Double)dataPoint.LabelVisual.GetValue(Canvas.LeftProperty) < width / 2)
                        {
                            explodedPoints.LabelPosition = new Point(unExplodedPoints.LabelPosition.X + offsetX, unExplodedPoints.LabelPosition.Y);
                        }
                        else
                        {
                            explodedPoints.LabelPosition = new Point(unExplodedPoints.LabelPosition.X + offsetX, unExplodedPoints.LabelPosition.Y);
                        }
                    }

                    dataPoint.ExplodeAnimation = new Storyboard();
                    dataPoint.ExplodeAnimation = CreateExplodingOut3DAnimation(dataPoint, dataPoint.ExplodeAnimation, pieFaces, dataPoint.LabelVisual, dataPoint._labelLine, unExplodedPoints, explodedPoints, pieParams.OffsetX, pieParams.OffsetY);

                    dataPoint.UnExplodeAnimation = new Storyboard();
                    dataPoint.UnExplodeAnimation = CreateExplodingIn3DAnimation(dataPoint, dataPoint.UnExplodeAnimation, pieFaces, dataPoint.LabelVisual, dataPoint._labelLine, unExplodedPoints, explodedPoints, pieParams.OffsetX, pieParams.OffsetY);
                }

                #endregion

                else
                {   
                    PieDoughnut2DPoints unExplodedPoints = new PieDoughnut2DPoints();
                    PieDoughnut2DPoints explodedPoints = new PieDoughnut2DPoints();
                    
                    if (labelStyleCounter == enabledDataPoints.Count)
                        pieParams.OuterRadius -= pieParams.OuterRadius * pieParams.ExplodeRatio;

                    Canvas pieVisual = GetPie2D(ref faces, pieParams, ref unExplodedPoints, ref explodedPoints, ref dataPoint._labelLine);

                    if (dataPoint.LabelVisual != null)
                    {   
                        unExplodedPoints.LabelPosition = new Point((Double)dataPoint.LabelVisual.GetValue(Canvas.LeftProperty), (Double)dataPoint.LabelVisual.GetValue(Canvas.TopProperty));
                        if ((Double)dataPoint.LabelVisual.GetValue(Canvas.LeftProperty) < width / 2)
                        {
                            explodedPoints.LabelPosition = new Point(unExplodedPoints.LabelPosition.X + offsetX, unExplodedPoints.LabelPosition.Y);
                        }
                        else
                        {
                            explodedPoints.LabelPosition = new Point(unExplodedPoints.LabelPosition.X + offsetX, unExplodedPoints.LabelPosition.Y);
                        }
                    }

                    TranslateTransform translateTransform = new TranslateTransform();
                    pieVisual.RenderTransform = translateTransform;
                    dataPoint.ExplodeAnimation = new Storyboard();
                    dataPoint.ExplodeAnimation = CreateExplodingOut2DAnimation(dataPoint, dataPoint.ExplodeAnimation, pieVisual, dataPoint.LabelVisual, dataPoint._labelLine, translateTransform, unExplodedPoints, explodedPoints, offsetX, offsetY);
                    dataPoint.UnExplodeAnimation = new Storyboard();
                    dataPoint.UnExplodeAnimation = CreateExplodingIn2DAnimation(dataPoint, dataPoint.UnExplodeAnimation, pieVisual, dataPoint.LabelVisual, dataPoint._labelLine, translateTransform, unExplodedPoints, explodedPoints, offsetX, offsetY);
                    
                    pieVisual.SetValue(Canvas.TopProperty, height / 2 - pieVisual.Height / 2);
                    pieVisual.SetValue(Canvas.LeftProperty, width / 2 - pieVisual.Width / 2);
                    visual.Children.Add(pieVisual);
                    faces.VisualComponents.Add(pieVisual);
                    faces.Visual = pieVisual;
                }

                Debug.WriteLine("Datapoint" + enabledDataPoints.IndexOf(dataPoint) + ": " + DateTime.Now.ToLongTimeString());

                dataPoint.Faces = faces;
                
                startAngle = endAngle;
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

            return visual;
        }

        internal static Canvas GetVisualObjectForDoughnutChart(Double width, Double height, PlotDetails plotDetails, List<DataSeries> seriesList, Chart chart, bool animationEnabled)
        {
            if (Double.IsNaN(width) || Double.IsNaN(height) || width <= 0 || height <= 0) return null;

            Canvas visual = new Canvas();
            visual.Width = width;
            visual.Height = height;

            DataSeries series = seriesList[0];

            if (series.Enabled == false)
                return visual;

            List<DataPoint> enabledDataPoints = (from datapoint in series.DataPoints where datapoint.Enabled == true && datapoint.YValue != 0 select datapoint).ToList();
            Double absoluteSum = plotDetails.GetAbsoluteSumOfDataPoints(enabledDataPoints);

            absoluteSum = (absoluteSum == 0) ? 1 : absoluteSum;

            Double centerX = width / 2;
            Double centerY = height / 2;

            Double offsetX = 0;
            Double offsetY = 0;

            Size pieCanvas = new Size();
            Canvas labelCanvas = CreateAndPositionLabels(absoluteSum, enabledDataPoints, width, height, ((chart.View3D) ? 0.4 : 1), chart.View3D, ref pieCanvas);

            Double radius = Math.Min(pieCanvas.Width, pieCanvas.Height) / (chart.View3D ? 1 : 2);
            Double startAngle = series.InternalStartAngle;
            Double endAngle = 0;
            Double angle;
            Double meanAngle;
            Double absoluteYValue;
            Double radiusDiff = 0;

            var explodedDataPoints = (from datapoint in series.DataPoints where datapoint.Exploded == true && datapoint.YValue != 0 select datapoint);
            radiusDiff = (explodedDataPoints.Count() > 0) ? radius * 0.3 : 0;

            //radius -= radiusDiff;
            if (chart.View3D)
            {
                _elementPositionData = new List<ElementPositionData>();
            }

            if (labelCanvas != null)
            {
                labelCanvas.SetValue(Canvas.ZIndexProperty, 50001);
                labelCanvas.IsHitTestVisible = false;
            }

            if (series.Storyboard == null)
                series.Storyboard = new Storyboard();

            DataSeriesRef = series;

            SectorChartShapeParams pieParams = null;

            Int32 labelStyleCounter = 0;

            if (!chart.View3D)
            {
                foreach (DataPoint dataPoint in enabledDataPoints)
                {
                    if (dataPoint.LabelStyle == LabelStyles.Inside)
                        labelStyleCounter++;
                }
            }

            foreach (DataPoint dataPoint in enabledDataPoints)
            {
                DataPointRef = dataPoint;

                if (Double.IsNaN(dataPoint.YValue) || dataPoint.YValue == 0)
                    continue;

                absoluteYValue = Math.Abs(dataPoint.YValue);

                angle = (absoluteYValue / absoluteSum) * Math.PI * 2;

                endAngle = startAngle + angle;
                meanAngle = (startAngle + endAngle) / 2;

                pieParams = new SectorChartShapeParams();
                dataPoint.VisualParams = pieParams;

                pieParams.AnimationEnabled = animationEnabled;
                pieParams.Storyboard = series.Storyboard;
                pieParams.ExplodeRatio = 0.2;
                pieParams.Center = new Point(centerX, centerY);

                pieParams.InnerRadius = radius / 2;
                pieParams.OuterRadius = radius;
                pieParams.StartAngle = (startAngle) % (Math.PI * 2);
                pieParams.StopAngle = (endAngle) % (Math.PI * 2);
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

                offsetX = radius * pieParams.ExplodeRatio * Math.Cos(meanAngle);
                offsetY = radius * pieParams.ExplodeRatio * Math.Sin(meanAngle);
                pieParams.OffsetX = offsetX;
                pieParams.OffsetY = offsetY * (chart.View3D ? pieParams.YAxisScaling : 1);

                if (dataPoint.LabelVisual != null)
                {   
                    if ((Double)dataPoint.LabelVisual.GetValue(Canvas.LeftProperty) < width / 2)
                    {
                        pieParams.LabelPoint = new Point((Double)dataPoint.LabelVisual.GetValue(Canvas.LeftProperty) + dataPoint.LabelVisual.DesiredSize.Width, (Double)dataPoint.LabelVisual.GetValue(Canvas.TopProperty) + dataPoint.LabelVisual.DesiredSize.Height / 2);
                    }
                    else
                    {
                        pieParams.LabelPoint = new Point((Double)dataPoint.LabelVisual.GetValue(Canvas.LeftProperty), (Double)dataPoint.LabelVisual.GetValue(Canvas.TopProperty) + dataPoint.LabelVisual.DesiredSize.Height / 2);
                    }

                    // apply animation to the labels
                    if (animationEnabled)
                    {
                        series.Storyboard = CreateOpacityAnimation(series.Storyboard, dataPoint.LabelVisual, 2, 1, 0.5);
                        dataPoint.LabelVisual.Opacity = 0;
                    }
                }

                Faces faces = new Faces();
                faces.Parts = new List<FrameworkElement>();

                if (chart.View3D)
                {
                    PieDoughnut3DPoints unExplodedPoints = new PieDoughnut3DPoints();
                    PieDoughnut3DPoints explodedPoints = new PieDoughnut3DPoints();
                    List<Shape> doughnutFaces = GetDoughnut3D(ref faces, pieParams, ref unExplodedPoints, ref explodedPoints, ref dataPoint._labelLine);

                    foreach (Shape path in doughnutFaces)
                    {
                        if (path != null)
                        {
                            visual.Children.Add(path);
                            faces.VisualComponents.Add(path);
                            path.RenderTransform = new TranslateTransform();
                            // apply animation to the 3D sections
                            if (animationEnabled)
                            {
                                series.Storyboard = CreateOpacityAnimation(series.Storyboard, path, 1.0 / (series.DataPoints.Count) * (series.DataPoints.IndexOf(dataPoint)), dataPoint.Opacity, 0.5);
                                path.Opacity = 0;
                            }
                        }
                    }
                    if (dataPoint._labelLine != null && pieParams.LabelLineEnabled)
                    {
                        dataPoint._labelLine.RenderTransform = new TranslateTransform();
                        visual.Children.Add(dataPoint._labelLine);
                        faces.VisualComponents.Add(dataPoint._labelLine);
                    }

                    faces.Visual = visual;

                    if (dataPoint.LabelVisual != null)
                    {
                        unExplodedPoints.LabelPosition = new Point((Double)dataPoint.LabelVisual.GetValue(Canvas.LeftProperty), (Double)dataPoint.LabelVisual.GetValue(Canvas.TopProperty));

                        if ((Double)dataPoint.LabelVisual.GetValue(Canvas.LeftProperty) < width / 2)
                        {
                            explodedPoints.LabelPosition = new Point(unExplodedPoints.LabelPosition.X + offsetX, unExplodedPoints.LabelPosition.Y);
                        }
                        else
                        {
                            explodedPoints.LabelPosition = new Point(unExplodedPoints.LabelPosition.X + offsetX, unExplodedPoints.LabelPosition.Y);
                        }
                    }

                    dataPoint.ExplodeAnimation = new Storyboard();
                    dataPoint.ExplodeAnimation = CreateExplodingOut3DAnimation(dataPoint, dataPoint.ExplodeAnimation, doughnutFaces, dataPoint.LabelVisual, dataPoint._labelLine, unExplodedPoints, explodedPoints, pieParams.OffsetX, pieParams.OffsetY);
                    dataPoint.UnExplodeAnimation = new Storyboard();
                    dataPoint.UnExplodeAnimation = CreateExplodingIn3DAnimation(dataPoint, dataPoint.UnExplodeAnimation, doughnutFaces, dataPoint.LabelVisual, dataPoint._labelLine, unExplodedPoints, explodedPoints, pieParams.OffsetX, pieParams.OffsetY);
                }
                else
                {
                    PieDoughnut2DPoints unExplodedPoints = new PieDoughnut2DPoints();
                    PieDoughnut2DPoints explodedPoints = new PieDoughnut2DPoints();

                    if (labelStyleCounter == enabledDataPoints.Count)
                    {
                        pieParams.OuterRadius -= pieParams.OuterRadius * pieParams.ExplodeRatio;
                        pieParams.InnerRadius = pieParams.OuterRadius / 2;
                    }

                    Canvas pieVisual = GetDoughnut2D(ref faces, pieParams, ref unExplodedPoints, ref explodedPoints, ref dataPoint._labelLine);

                    if (dataPoint.LabelVisual != null)
                    {
                        unExplodedPoints.LabelPosition = new Point((Double)dataPoint.LabelVisual.GetValue(Canvas.LeftProperty), (Double)dataPoint.LabelVisual.GetValue(Canvas.TopProperty));
                        if ((Double)dataPoint.LabelVisual.GetValue(Canvas.LeftProperty) < width / 2)
                        {
                            explodedPoints.LabelPosition = new Point(unExplodedPoints.LabelPosition.X + offsetX, unExplodedPoints.LabelPosition.Y);
                        }
                        else
                        {
                            explodedPoints.LabelPosition = new Point(unExplodedPoints.LabelPosition.X + offsetX, unExplodedPoints.LabelPosition.Y);
                        }
                    }

                    TranslateTransform translateTransform = new TranslateTransform();
                    pieVisual.RenderTransform = translateTransform;
                    dataPoint.ExplodeAnimation = new Storyboard();
                    dataPoint.ExplodeAnimation = CreateExplodingOut2DAnimation(dataPoint, dataPoint.ExplodeAnimation, pieVisual, dataPoint.LabelVisual, dataPoint._labelLine, translateTransform, unExplodedPoints, explodedPoints, offsetX, offsetY);
                    dataPoint.UnExplodeAnimation = new Storyboard();
                    dataPoint.UnExplodeAnimation = CreateExplodingIn2DAnimation(dataPoint, dataPoint.UnExplodeAnimation, pieVisual, dataPoint.LabelVisual, dataPoint._labelLine, translateTransform, unExplodedPoints, explodedPoints, offsetX, offsetY);


                    pieVisual.SetValue(Canvas.TopProperty, height / 2 - pieVisual.Height / 2);
                    pieVisual.SetValue(Canvas.LeftProperty, width / 2 - pieVisual.Width / 2);
                    visual.Children.Add(pieVisual);
                    faces.VisualComponents.Add(pieVisual);
                    faces.Visual = pieVisual;
                }

                dataPoint.Faces = faces;

                startAngle = endAngle;
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

            if(labelCanvas != null)
                visual.Children.Add(labelCanvas);

            return visual;
        }

    }
}