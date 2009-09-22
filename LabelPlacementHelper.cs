using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Linq;

namespace Visifire.Commons
{
    public class LabelPlacementHelper
    {
        #region "Vertical LabelPlacement Helper"
        /// <summary>
        /// Arrange the labels vertically
        /// </summary>
        /// <param name="area"></param>
        /// <param name="labelsInfo"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <returns></returns>
        internal static Rect[] VerticalLabelPlacement(Rect area, ref Rect[] labelsInfo)
        {
            Int32 index;

            for (index = 0; index < labelsInfo.Length; index++)
            {
                if (index == labelsInfo.Length - 1)
                    break;

                if (CheckOverlap(labelsInfo[index], labelsInfo[index + 1]))
                {
                    // Shift the top label
                    Double shiftValue = CalculateShiftValue(labelsInfo[index], labelsInfo[index + 1]);

                    Int32 findSpaceAtIndex = index;
                    Double spaceFoundAtIndex = FindSpaceTowardsTopAtIndex(ref findSpaceAtIndex, shiftValue * 2, area, labelsInfo);

                    if (spaceFoundAtIndex != 0 || findSpaceAtIndex != -1) // if space found to shift towards top
                    {
                        // if(index != findSpaceAtIndex)
                        ShiftTowardsTop(shiftValue * 2, index, findSpaceAtIndex, ref labelsInfo);
                    }
                    else
                    {
                        findSpaceAtIndex = index;
                        spaceFoundAtIndex = FindSpaceTowardsBottomAtIndex(ref findSpaceAtIndex, shiftValue * 2, area, labelsInfo);

                        if (spaceFoundAtIndex != 0 || findSpaceAtIndex != -1) // if space found to shift towards top
                        {
                            if (index == findSpaceAtIndex)
                                ShiftTowardsBottom(shiftValue * 2, index, findSpaceAtIndex, ref labelsInfo);
                            else
                                ShiftTowardsBottom(shiftValue * 2, index + 1, findSpaceAtIndex, ref labelsInfo);
                        }
                    }
                }
            }


            for (index = 0; index < labelsInfo.Length; index++)
            {
                if (index == labelsInfo.Length - 1)
                    break;

                if (CheckOverlap(labelsInfo[index], labelsInfo[index + 1]))
                {
                    // Shift the top label
                    Double shiftValue = CalculateShiftValue(labelsInfo[index], labelsInfo[index + 1]) * 2;

                    for (Int32 i = index; i > 0 && shiftValue > 0; i--)
                    {
                        Double posibleShiftValue = CheckSpaceAtTop(i, area, labelsInfo);

                        if (posibleShiftValue > 0 && shiftValue > posibleShiftValue)
                        {
                            ShiftTowardsTop(posibleShiftValue, index, i, ref labelsInfo);
                            shiftValue -= posibleShiftValue;
                        }
                    }

                }
            }

            for (index = 0; index < labelsInfo.Length; index++)
            {
                if (index == labelsInfo.Length - 1)
                    break;

                if (CheckOverlap(labelsInfo[index], labelsInfo[index + 1]))
                {
                    // Shift the top label
                    Double shiftValue = CalculateShiftValue(labelsInfo[index], labelsInfo[index + 1]) * 2;

                    for (Int32 i = index + 1; i < labelsInfo.Length && shiftValue > 0; i++)
                    {
                        Double posibleShiftValue;

                        if (i == labelsInfo.Length - 1)
                            posibleShiftValue = shiftValue;
                        else
                            posibleShiftValue = CheckSpaceAtBottom(i, area, labelsInfo);

                        if (posibleShiftValue > 0 && shiftValue >= posibleShiftValue)
                        {
                            ShiftTowardsBottom(posibleShiftValue, index + 1, i, ref labelsInfo);
                            shiftValue -= posibleShiftValue;
                        }
                    }

                }
            }

            return labelsInfo;
        }

        private static Double FindSpaceTowardsTopAtIndex(ref Int32 index, Double spaceRequired, Rect area, Rect[] labelsInfo)
        {
            Double amountOfSpaceFound = 0;

            for (; index >= 0; index--)
            {
                Double space = CheckSpaceAtTop(index, area, labelsInfo);

                if (spaceRequired <= space)
                {
                    amountOfSpaceFound = space;
                    break;
                }
            }

            if (amountOfSpaceFound == 0)
                index = -1;

            return amountOfSpaceFound;
        }

        private static Double FindSpaceTowardsBottomAtIndex(ref Int32 index, Double spaceRequired, Rect area, Rect[] labelsInfo)
        {
            Double amountOfSpaceFound = 0;

            for (; index < labelsInfo.Length - 1; index++)
            {
                Double space = CheckSpaceAtBottom(index, area, labelsInfo);

                if (spaceRequired <= space)
                {
                    amountOfSpaceFound = space;
                    break;
                }
            }

            if (amountOfSpaceFound == 0)
                index = -1;

            return amountOfSpaceFound;
        }

        /// <summary>
        /// Check available space at top
        /// </summary>
        /// <param name="IndexOfTargetArea">Index of target area</param>
        /// <param name="labelsInfo">labelsInfo</param>
        /// <returns>Double</returns>
        private static Double CheckSpaceAtTop(Int32 index, Rect baseArea, Rect[] labelsInfo)
        {
            if (index == 0)
            {
                if (labelsInfo[index].Top < 0)
                    return 0;
                else
                    return labelsInfo[index].Top;
            }
            else
            {
                if (labelsInfo.Length > 1)
                {
                    if (labelsInfo[index].Top > (labelsInfo[index - 1].Top + labelsInfo[index - 1].Height))
                        return labelsInfo[index].Top - (labelsInfo[index - 1].Top + labelsInfo[index - 1].Height);
                    else
                        return 0;
                }
                else
                    return 0;
            }
        }

        /// <summary>
        /// Shift labels towards top
        /// </summary>
        /// <param name="value">Amount to shift</param>
        /// <param name="startIndex">Start index of labels</param>
        /// <param name="labelsInfos">Array of label information</param>
        private static void ShiftTowardsTop(Double value, Int32 startIndex, ref Rect[] labelsInfos)
        {
            for (Int32 index = startIndex; index >= 0; index--)
            {
                labelsInfos[index].Y -= value;
            }
        }

        /// <summary>
        /// Shift labels towards top
        /// </summary>
        /// <param name="value">Amount to shift</param>
        /// <param name="startIndex">Start index of labels</param>
        /// <param name="labelsInfos">Array of label information</param>
        private static void ShiftTowardsTop(Double value, Int32 fromIndex, Int32 toIndex, ref Rect[] labelsInfos)
        {
            for (Int32 index = fromIndex; index >= toIndex; index--)
            {
                labelsInfos[index].Y -= value;
            }
        }

        /// <summary>
        /// Shift labels towards top
        /// </summary>
        /// <param name="value">Amount to shift</param>
        /// <param name="startIndex">Start index of labels</param>
        /// <param name="labelsInfos">Array of label information</param>
        private static void ShiftTowardsTop(Double value, ref Rect labelsInfo)
        {
            labelsInfo.Y -= value;
        }

        /// <summary>
        /// Shift labels towards bottom
        /// </summary>
        /// <param name="value">Amount to shift</param>
        /// <param name="startIndex">Start index of labels</param>
        /// <param name="labelsInfos">Array of label information</param>
        private static void ShiftTowardsBottom(Double value, Int32 fromIndex, Int32 toIndex, ref Rect[] labelsInfos)
        {
            for (Int32 index = fromIndex; index <= toIndex && toIndex < labelsInfos.Length; index++)
            {
                labelsInfos[index].Y += value;
            }
        }

        /// <summary>
        /// Shift labels towards bottom
        /// </summary>
        /// <param name="value">Amount to shift</param>
        /// <param name="startIndex">Start index of labels</param>
        /// <param name="labelsInfos">Array of label information</param>
        private static void ShiftTowardsBottom(Double value, Int32 startIndex, ref Rect[] labelsInfos)
        {
            for (Int32 index = startIndex; index < labelsInfos.Length; index++)
            {
                labelsInfos[index].Y += value;
            }
        }

        /// <summary>
        /// Shift labels towards bottom
        /// </summary>
        /// <param name="value">Amount to shift</param>
        /// <param name="startIndex">Start index of labels</param>
        /// <param name="labelsInfos">Array of label information</param>
        private static void ShiftTowardsBottom(Double value, ref Rect labelsInfo)
        {
            labelsInfo.Y += value;
        }

        /// <summary>
        /// Check available space at bottom
        /// </summary>
        /// <param name="IndexOfTargetArea">Index of target area</param>
        /// <param name="labelsInfo">labelsInfo</param>
        /// <returns>Double</returns>
        private static Double CheckSpaceAtBottom(Int32 index, Rect baseArea, Rect[] labelsInfo)
        {
            if (index == labelsInfo.Length - 1)
            {
                Double gap = baseArea.Height - (labelsInfo[index].Top + labelsInfo[index].Height);
                return (gap > 0) ? gap : 0;
            }
            else
            {
                if (labelsInfo.Length > 1)
                {
                    if ((labelsInfo[index + 1].Top) > (labelsInfo[index].Top + labelsInfo[index].Height))
                    {
                        Double gap = (labelsInfo[index + 1].Top) - (labelsInfo[index].Top + labelsInfo[index].Height);
                        return gap;
                    }
                    else
                        return 0;
                }
                else
                    return 0;
            }
        }

        /// <summary>
        /// Check whether two areas overlap on each other 
        /// </summary>
        /// <param name="areaInfo1">1st area information</param>
        /// <param name="areaInfo2">2nd area information</param>
        /// <returns>True/False</returns>
        private static Boolean CheckOverlap(Rect areaInfo1, Rect areaInfo2)
        {
            if (areaInfo1.Top + areaInfo1.Height > areaInfo2.Top)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Calculate amount of shift value towards top or bottom to overcome overlap.
        /// </summary>
        /// <param name="areaInfo1">1st area information</param>
        /// <param name="areaInfo2">2nd area information</param>
        /// <returns>Double</returns>
        private static Double CalculateShiftValue(Rect areaInfo1, Rect areaInfo2)
        {
            if (areaInfo1.Top + areaInfo1.Height > areaInfo2.Top)
                return Math.Abs(((areaInfo1.Top + areaInfo1.Height) - areaInfo2.Top) / 2);
            else
                return 0;
        }

        #endregion

        /// <summary>
        /// Get the list of labels at the right and left side of the pie of a circular region
        /// </summary>
        /// <param name="labels">List<CircularLabel></param>
        /// <param name="labelsAtLeft">labels at left</param>
        /// <param name="labelsAtRight">labels at right</param>
        private static void GetLeftAndRightLabels(List<CircularLabel> labels, out List<CircularLabel> labelsAtLeft, out List<CircularLabel> labelsAtRight)
        {
            // Labels at angle >= 3 * Math.PI / 2 && angle < 0
            List<CircularLabel> labelsR1 = (from cl in labels
                                            where cl.CurrentMeanAngle >= 3 * Math.PI / 2
                                            orderby cl.CurrentMeanAngle
                                                ascending
                                            select cl).ToList();

            // Labels at angle >= 0 && angle < <= Math.PI / 2
            List<CircularLabel> labelsR2 = (from cl in labels
                                            where cl.CurrentMeanAngle >= 0 && cl.CurrentMeanAngle <= Math.PI / 2
                                            orderby cl.CurrentMeanAngle ascending
                                            select cl).ToList();

            // Combine labels at right present at the right side of the circular region
            labelsAtRight = new List<CircularLabel>();
            labelsAtRight.AddRange(labelsR1);
            labelsAtRight.AddRange(labelsR2);

            // All labels present at the right side of the circular region
            labelsAtLeft = (from cl in labels
                            where cl.CurrentMeanAngle > Math.PI / 2 && cl.CurrentMeanAngle < 3 * Math.PI / 2
                            orderby cl.CurrentMeanAngle ascending
                            select cl).ToList();
        }

        /// <summary>
        /// Rearrange the labels vertically
        /// </summary>
        /// <param name="labels">List of CircularLabels</param>
        /// <param name="leftOfArea">Left of the bounding area</param>
        /// <param name="topOfArea">Top of the bounding area</param>
        /// <param name="areaHeight">Height of the bounding area</param>
        /// <param name="areaWidth">Width of the bounding area</param>
        private static void RearrangeLabelsVertically(List<CircularLabel> labels, Double leftOfArea, Double topOfArea, Double areaHeight, Double areaWidth)
        {
            Rect[] labelInfo = new Rect[labels.Count];
            int index = 0;

            // Prepare label information into an array
            foreach (CircularLabel label in labels)
            {
                    Double left = (Double)label.LabelVisual.GetValue(Canvas.LeftProperty);
                    Double top = (Double)label.LabelVisual.GetValue(Canvas.TopProperty);

                    labelInfo[index++] = new Rect(left, top, label.LabelVisual.Width, label.LabelVisual.Height);
            }

            // Arrange the labels vertically
            LabelPlacementHelper.VerticalLabelPlacement(new Rect(leftOfArea, topOfArea, areaWidth, areaHeight), ref labelInfo);

            index = 0;

            // Update position of the labels 
            foreach (CircularLabel label in labels)
            {
                Double top = labelInfo[index].Top + topOfArea;
                Double left = labelInfo[index].Left;

                Double currentMeanAngle = label.CalculateAngleByYCoordinate(top);

                if (!Double.IsNaN(currentMeanAngle))
                {
                    currentMeanAngle = CircularLabel.ResetMeanAngle(currentMeanAngle);
                    label.Position = label.GetCartesianCoordinates4Labels(currentMeanAngle);

                    if (left < label.Center.X)
                        left = label.Center.X - (label.Position.X - label.Center.X);
                    else
                        left = label.Position.X;

                    label.CurrentMeanAngle = currentMeanAngle;
                }

                Double offset = 0;

                // Move the labels towards left or right if space availlable
                //if ((label.BaseMeanAngle > 7 * Math.PI / 4 && label.BaseMeanAngle < Math.PI / 4
                //    || label.BaseMeanAngle > 3 * Math.PI / 2 && label.BaseMeanAngle < 5 * Math.PI / 4)
                if (top > (label.YRadiusLabel * 2) / 6 && top < 5 * (label.YRadiusLabel * 2) / 6)
                {
                    if (left < label.Center.X)
                    {
                        Double x = left - label.LabelVisual.Width;
                        if (x > 0)
                            offset = - x / 3;
                    }
                    else
                    {   
                        Double x = areaWidth - (left + label.LabelVisual.Width);
                        if (x > 0)
                            offset = + x / 3;
                    }
                }

                label.Position = new Point(left + offset, top);

                label.UpdateLabelVisualPosition();
                //label.LabelVisual.SetValue(Canvas.LeftProperty, left); // );
                //label.LabelVisual.SetValue(Canvas.TopProperty, top);

                index++;
            }
        }

        /// <summary>
        /// Arrange labels over a circular path
        /// </summary>
        /// <param name="boundingArea"></param>
        /// <param name="labels"></param>
        public static void CircularLabelPlacment(Rect boundingArea, List<CircularLabel> labels)
        {
            //return;
            List<CircularLabel> labelsAtLeft;
            List<CircularLabel> labelsAtRight;
            List<CircularLabel> allLabels = new List<CircularLabel>();

            GetLeftAndRightLabels(labels, out labelsAtLeft, out labelsAtRight);

            // Combine left and right labels
            allLabels.AddRange(labelsAtRight);
            allLabels.AddRange(labelsAtLeft);

            allLabels[0].IsFirst = true;
            allLabels[allLabels.Count - 1].IsLast = true;
                       
            // Try to place labels and asign Skip value
            foreach (CircularLabel label in labels)
            {
                label.PlaceLabel(false);
            }

            // Try to place labels and asign Skip value
            foreach (CircularLabel label in labels)
            {
                label.PlaceLabel(true);
            }
            
            // Update label visual position
            foreach (CircularLabel label in labels)
            {
                label.UpdateLabelVisualPosition();
            }
            
            // Labels that are not slipped yet
            List<CircularLabel> labelsNotSkipped = (from lb in labels where !lb.IsSkiped select lb).ToList();

            GetLeftAndRightLabels(labelsNotSkipped, out labelsAtLeft, out labelsAtRight);

            labelsAtLeft.Reverse();

            // Arrange right labels vertically
            RearrangeLabelsVertically(labelsAtRight, Double.NaN, 0, boundingArea.Height, boundingArea.Width);
            RearrangeLabelsVertically(labelsAtLeft, Double.NaN, 0, boundingArea.Height, boundingArea.Width);
            
            // Skip labels if any label goes out of PlotArea
            foreach (CircularLabel label in labelsNotSkipped)
            {   
                Double left = (Double)label.LabelVisual.GetValue(Canvas.LeftProperty);
                Double top = (Double)label.LabelVisual.GetValue(Canvas.TopProperty);

                if (top + label.Height > boundingArea.Height || label.CheckOutOfBounds())
                    label.SkipLabel();
            }
        }

        public static Double LABEL_LINE_GAP = 3;
    }

    /// <summary>
    /// Types of overlap
    /// </summary>
    public enum OverlapTypes
    {
        Vertical,
        Horizontal,
        Both,
        None
    }
    
    /// <summary>
    /// Visifire.Commons.CircularLabel class
    /// </summary>
    public class CircularLabel
    {   
        /// <summary>
        /// CircularLabel class is used to 
        /// </summary>
        /// <param name="labelVisual"></param>
        /// <param name="center">Center of the circular region</param>
        /// <param name="meanAngle">Mean angle of the label</param>
        /// <param name="xRadiusLabel">XRadius of the ellipse for label placement</param>
        /// <param name="yRadiusLabel">YRadius of the ellipse for label placement</param>
        /// <param name="xRadiusChart">XRadius of the chart</param>
        /// <param name="yRadiusChart">YRadius of the chart</param>
        /// <param name="canvas">Parent panel of the label. Used for debugging purpose</param>
        public CircularLabel(FrameworkElement labelVisual, Point center, Double meanAngle,
            Double xRadiusLabel, Double yRadiusLabel, Double xRadiusChart, Double yRadiusChart, 
            Canvas canvas)
        {   
            LabelVisual = labelVisual;
            BaseMeanAngle = ResetMeanAngle(meanAngle);
            CurrentMeanAngle = BaseMeanAngle;
            
            XRadiusLabel = xRadiusLabel;
            YRadiusLabel = yRadiusLabel;
            XRadiusChart = xRadiusChart;
            YRadiusChart = yRadiusChart;
            Visual = canvas;
            Center = center;

            Position = GetCartesianCoordinates4Labels(meanAngle);

            UpdateLabelVisualPosition();

            // CalculateMaxAndMinPosition();
        }

        /// <summary>
        /// Check if CircularLabel overlaps with nearest CircularLabel
        /// </summary>
        /// <param name="direction">Clockwise/Counterclockwise</param>
        /// <param name="verticalOffset">Vertical offset required to overcome overlap</param>
        /// <param name="horizontalOffset">Horizontal offset required to overcome overlap</param>
        /// <returns>OverlapTypes</returns>
        public OverlapTypes CheckOverLap(SweepDirection direction, out Double verticalOffset, out Double horizontalOffset)
        {   
            verticalOffset = 0;
            horizontalOffset = 0;

            if(direction == SweepDirection.Clockwise)
                return (NextLabel == null) ? OverlapTypes.None : CheckOverlap(NextLabel, out verticalOffset, out horizontalOffset);
            else
                return (PreviusLabel == null) ? OverlapTypes.None : CheckOverlap(PreviusLabel, out verticalOffset, out horizontalOffset);
        }

        /// <summary>
        /// Ask for space to NextLabel if NextLabel can leave the space.
        /// NextLabel B asks for space to the next label C.. and so on.
        /// So this CircularLabel is looking for space.
        /// </summary>
        public void PlaceLabel(Boolean skip)
        {
            Double verticalOffset, horizontalOffset;
            Boolean placedTowardsTop;
            Boolean placedTowardsBottom;
            Boolean isPlaced = false;

            Int32 noOfIteration = 2;

            OverlapTypes overlapTypesBottom = CheckOverLap(SweepDirection.Clockwise, out verticalOffset, out horizontalOffset);

            if (overlapTypesBottom == OverlapTypes.Both)
            {
                placedTowardsTop = AskSpaceToPreviusLabel(this, verticalOffset / 2, noOfIteration);
                placedTowardsBottom = AskSpaceToNextLabel(this, verticalOffset / 2, noOfIteration);

                if (placedTowardsTop && placedTowardsBottom)
                    isPlaced = true;
                else if (placedTowardsTop && !placedTowardsBottom)
                {
                    placedTowardsTop = AskSpaceToPreviusLabel(this, verticalOffset / 2, noOfIteration);
                    isPlaced = placedTowardsTop;
                }
                else if (!placedTowardsTop && placedTowardsBottom)
                {
                    placedTowardsBottom = AskSpaceToNextLabel(this, verticalOffset / 2, noOfIteration);
                    isPlaced = placedTowardsTop;
                }
                else
                    isPlaced = false;
            }
            else
            {
                OverlapTypes overlapTypesTop = CheckOverLap(SweepDirection.Counterclockwise, out verticalOffset, out horizontalOffset);

                if (overlapTypesTop == OverlapTypes.Both)
                {   
                    placedTowardsBottom = AskSpaceToNextLabel(this, verticalOffset / 2, noOfIteration);
                    placedTowardsTop = AskSpaceToPreviusLabel(this, verticalOffset / 2, noOfIteration);

                    if (placedTowardsTop && placedTowardsBottom)
                        isPlaced = true;
                    else if (placedTowardsTop && !placedTowardsBottom)
                    {
                        placedTowardsTop = AskSpaceToNextLabel(this, verticalOffset / 2, noOfIteration);
                        isPlaced = placedTowardsTop;
                    }
                    else if (!placedTowardsTop && placedTowardsBottom)
                    {
                        placedTowardsBottom = AskSpaceToPreviusLabel(this, verticalOffset / 2, noOfIteration);
                        isPlaced = placedTowardsBottom;
                    }
                    else
                        isPlaced = false;
                }
                else
                    isPlaced = true;
            }

            if (isPlaced)
            {
                //ColorIt(Colors.Green);
            }
            else
            {
                //ColorIt(Colors.Red);
                if (skip)
                    SkipLabel();
            }
        }

        /// <summary>
        /// Ask for space to NextLabel if NextLabel can leave the space.
        /// NextLabel B asks for space to the next label C.. and so on
        /// </summary>
        /// <param name="labelA">CircularLabel looking for space to overcome the overlap</param>
        private static Boolean AskSpaceToNextLabel(CircularLabel label, Double requiredVerticalOffset, Int32 noOfIteration)
        {   
            Boolean retValue = true;
            Double gap = 0;
            if (label.IsLast)
            {   
                if (label.Position.Y + label.Height < label.Boundary.Height &&
                    label.Boundary.Height - (label.Position.Y + label.Height) > requiredVerticalOffset)
                {   
                    retValue = label.RotateByVerticalOffset(requiredVerticalOffset, SweepDirection.Clockwise);
                    return retValue;
                }
                else
                    return false;
            }

            gap = VerticalSpcaeBetweenLabels(label, label.NextLabel);
            if (gap > requiredVerticalOffset)
            {
                retValue = label.RotateByVerticalOffset(requiredVerticalOffset, SweepDirection.Clockwise);
                return retValue;
            }
            else
            {
                retValue = AskSpaceToNextLabel(label.NextLabel, requiredVerticalOffset, noOfIteration);
                if (retValue)
                {
                    retValue = label.RotateByVerticalOffset(requiredVerticalOffset, SweepDirection.Clockwise);
                    return retValue;
                }
                else
                    retValue = false;
            }

            return retValue;
        }

        /// <summary>
        /// Ask for space to PreviusLabel B if it can leave the space inorder to overcome the overlap.
        /// PreviusLabel B asks for space to the previous label C.. and so on
        /// </summary>
        /// <param name="labelA">CircularLabel looking for space to overcome the overlap</param>
        private static Boolean AskSpaceToPreviusLabel(CircularLabel labelA, Double requiredVerticalOffset, Int32 noOfIteration)
        {
            Boolean retValue = true;
            Double gap = 0;
            if (labelA.IsFirst)
            {
                if (labelA.Position.Y > requiredVerticalOffset)
                {
                    retValue = labelA.RotateByVerticalOffset(requiredVerticalOffset, SweepDirection.Counterclockwise);
                    return retValue;
                }
                else
                    return false;
            }

            gap = VerticalSpcaeBetweenLabels(labelA, labelA.PreviusLabel);

            if (gap > requiredVerticalOffset)
            {   
                retValue =labelA.RotateByVerticalOffset(requiredVerticalOffset, SweepDirection.Counterclockwise);
                return retValue;
            }
            else
            {
                retValue = AskSpaceToPreviusLabel(labelA.PreviusLabel, requiredVerticalOffset, noOfIteration);
                if (retValue)
                {
                    retValue = labelA.RotateByVerticalOffset(requiredVerticalOffset, SweepDirection.Counterclockwise);
                    return retValue;
                }
                else
                    retValue = false;
            }

            return retValue;
        }

        /// <summary>
        /// Calculate vertical space available between two CircularLabels
        /// </summary>
        /// <param name="label1">CircularLabel A</param>
        /// <param name="label2">CircularLabel B</param>
        /// <returns>vertical space available between two CircularLabels</returns>
        private static Double VerticalSpcaeBetweenLabels(CircularLabel labelA, CircularLabel labelB)
        {
            Double gap = 0;

            if (labelA.Position.Y < labelB.Position.Y)
                gap = labelB.Position.Y - (labelA.Position.Y + labelA.Height);
            else
                gap = labelA.Position.Y - (labelB.Position.Y + labelB.Height);

            if (gap < 0)
                return 0;
            else
                return gap;
        }


        /// <summary>
        /// Skip a CircularLabel
        /// Note: If a CircularLabel is skipped then it will be removed from the circular link-list.
        /// </summary>
        public void SkipLabel()
        {   
            this.LabelVisual.Visibility = Visibility.Collapsed;
            IsSkiped = true;

            // Removed from the circular link-list.
            this.PreviusLabel.NextLabel = NextLabel;
            this.NextLabel.PreviusLabel = PreviusLabel;

            if (IsFirst)
                NextLabel.IsFirst = true;

            if (IsLast)
                PreviusLabel.IsLast = true;
        }

        /// <summary>
        /// Calculate angle by Y Coordinate
        /// </summary>
        /// <param name="y"></param>
        /// <returns></returns>
        public Double CalculateAngleByYCoordinate(Double y)
        {
            return Math.Asin((y - Center.Y) / YRadiusLabel);
        }

        /// <summary>
        /// Calculate angle by X Coordinate
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public Double CalculateAngleByXCoordinate(Double x)
        {
            return Math.Acos((x - Center.X) / XRadiusLabel);
        }

        /// <summary>
        /// Rotate the label towards certain degree clockwise or anticlockwise
        /// </summary>
        /// <param name="degree">Angle in Radian</param>
        /// <param name="direction">SweepDirection</param>
        public Boolean RotateByVerticalOffset(Double verticalOffset, System.Windows.Media.SweepDirection direction)
        {
            Double offsetAngle = Math.Atan(verticalOffset / YRadiusLabel);

            if (direction == SweepDirection.Counterclockwise)
                offsetAngle *= -1;

            Double currentMeanAngle = CurrentMeanAngle + offsetAngle;
            currentMeanAngle = ResetMeanAngle(currentMeanAngle);
            Point tempPosition = GetCartesianCoordinates4Labels(currentMeanAngle);

            if (Math.Abs(BaseMeanAngle - ResetMeanAngle(CurrentMeanAngle + offsetAngle)) > Math.PI / 5)
                return false;

            CurrentMeanAngle = currentMeanAngle;
            Position = tempPosition;

            return true;
        }

        /// <summary>
        /// Returns Cartation coordinates over circle
        /// </summary>
        /// <param name="meanAngle">Angle</param>
        /// <returns>Cartation coordinates</returns>
        public Point GetCartesianCoordinates4Labels(Double meanAngle)
        {   
            meanAngle = ResetMeanAngle(meanAngle);
            Double x = Center.X + XRadiusLabel * Math.Cos(meanAngle);
            Double y = Center.Y + YRadiusLabel * Math.Sin(meanAngle);

            return new Point(x, y);
        }

        /// <summary>
        /// Returns Cartation coordinates over circle
        /// </summary>
        /// <param name="meanAngle">Angle</param>
        /// <returns>Cartation coordinates</returns>
        public Point GetCartesianCoordinates4Chart(Double meanAngle)
        {
            meanAngle = ResetMeanAngle(meanAngle);
            Double x = Center.X + XRadiusChart * Math.Cos(meanAngle);
            Double y = Center.Y + YRadiusChart * Math.Sin(meanAngle);

            return new Point(x, y);
        }
        
        /// <summary>
        /// Check whether two labels overlap. Also calculate the horizontal and vertical offsets 
        /// to overcome the overlap.
        /// </summary>
        /// <param name="labelInfo">Information of the label</param>
        /// <param name="verticalOffset">How much need to move vertically to overcome overlap</param>
        /// <param name="horizontalOffset">How much need to move horizontally to overcome overlap</param>
        /// <returns>OverlapTypes</returns>
        private OverlapTypes CheckOverlap(CircularLabel label, out Double verticalOffset, out Double horizontalOffset)
        {
            OverlapTypes retValue = OverlapTypes.None;  // Type of overlap
            verticalOffset = 0;     // How much need to move vertically to overcome overlap
            horizontalOffset = 0;   // How much need to move horizontally to overcome overlap

            CircularLabel A = this;
            CircularLabel B = label;

            if (B.Position.Y > A.Position.Y)
            {
                if (B.Position.Y < A.Position.Y + A.Height)
                {
                    // Calculate how much need to move vertically to overcome overlap
                    verticalOffset = A.Position.Y + A.Height - B.Position.Y;
                    retValue = OverlapTypes.Vertical;
                }
            }
            else if (A.Position.Y > B.Position.Y)
            {
                if (A.Position.Y < B.Position.Y + B.Height)
                {
                    // Calculate how much need to move vertically to overcome overlap
                    verticalOffset = B.Position.Y + B.Height - A.Position.Y;
                    retValue = OverlapTypes.Vertical;
                }
            }

            if (B.Position.X > A.Position.X)
            {
                if (B.Position.X < A.Position.X + A.Width)
                {
                    // Calculate how much need to move horizontally to overcome overlap
                    horizontalOffset = A.Position.X + A.Width - B.Position.X;
                    retValue = (retValue == OverlapTypes.Vertical) ? OverlapTypes.Both : OverlapTypes.Horizontal;
                }
            }
            else if (A.Position.X > B.Position.X)
            {
                if (A.Position.X < B.Position.X + B.Width)
                {
                    // Calculate how much need to move horizontally to overcome overlap
                    horizontalOffset = B.Position.X + B.Width - A.Position.X;
                    retValue = (retValue == OverlapTypes.Vertical) ? OverlapTypes.Both : OverlapTypes.Horizontal;
                }
            }

            return retValue;
        }
        
        private void CalculateMaxAndMinPosition()
        {
            // Calculate MaxPosition
            Double x1 = Center.X + XRadiusLabel * Math.Cos(ResetMeanAngle(BaseMeanAngle + Math.PI / 2));
            Double y1 = Center.Y + YRadiusLabel * Math.Sin(ResetMeanAngle(BaseMeanAngle + Math.PI / 2));

            Line l = new Line();
            l.X1 = Center.X;
            l.Y1 = Center.Y;
            l.X2 = x1;
            l.Y2 = y1;

            Brush color = Graphics.GetRandonColor();
            color.Opacity = 0.7;
            l.Stroke = color;
            l.StrokeThickness = 2;
            Visual.Children.Add(l);
            (LabelVisual as Canvas).Background = color;

            // Calculate MaxPosition
            Double x2 = Center.X + XRadiusLabel * Math.Cos(ResetMeanAngle(BaseMeanAngle - Math.PI / 2));
            Double y2 = Center.Y + YRadiusLabel * Math.Sin(ResetMeanAngle(BaseMeanAngle - Math.PI / 2));

            l = new Line();
            l.X1 = Center.X;
            l.Y1 = Center.Y;
            l.X2 = x2;
            l.Y2 = y2;
            
            //l.Stroke = color;
            l.StrokeThickness = 1;
            Visual.Children.Add(l);

            MaxXPosition = Math.Max(x1, x2);
            MaxYPosition = Math.Max(y1, y2);
            MinXPosition = Math.Min(x1, x2);
            MinYPosition = Math.Min(y1, y2);
        }

        /// <summary>
        /// Reset the mean angle to 0 to 360
        /// </summary>
        /// <param name="meanAngle">Mean angle</param>
        /// <returns>New mean angle</returns>
        public static Double ResetMeanAngle(Double meanAngle)
        {
            if (meanAngle > Math.PI * 2) 
                meanAngle -= Math.PI * 2;

            if (meanAngle < 0)
                meanAngle = Math.PI * 2 + meanAngle;

            return meanAngle;
        }

        public Boolean CheckOutOfBounds()
        {
            //Point position = GetCartesianCoordinates4Labels(BaseMeanAngle);

            //if (BaseMeanAngle >= 7 * Math.PI / 4 && BaseMeanAngle <= 0 || BaseMeanAngle >= 0 && BaseMeanAngle <= Math.PI / 4
            //    || BaseMeanAngle >= 3 * Math.PI / 4 && BaseMeanAngle <= 5 * Math.PI / 4)
            //{
            //    if (Math.Abs(position.X - Position.X) > XRadiusLabel)
            //        return true;

            //    if (Math.Abs(position.Y - Position.Y) > YRadiusLabel)
            //        return true;
            //}
            //else
            //{
            //    if (Math.Abs(BaseMeanAngle - ResetMeanAngle(CurrentMeanAngle)) > Math.PI / 4)
            //        return true;
            //}

            Double left = (Double)LabelVisual.GetValue(Canvas.LeftProperty);
            Double top = (Double)LabelVisual.GetValue(Canvas.TopProperty);

            Double labelLinePointX, labelLinePointY;

            if (left < Center.X)
                labelLinePointX = left + Width + LabelPlacementHelper.LABEL_LINE_GAP;
            else
                labelLinePointX = left - LabelPlacementHelper.LABEL_LINE_GAP;

            labelLinePointY = top + Height / 2;
            Point pointOverPie = GetCartesianCoordinates4Chart(BaseMeanAngle);

            Point labelLinePoint = new Point(labelLinePointX, labelLinePointY);

            Point intersectionPoint = Graphics.IntersectingPointOfTwoLines(Center, labelLinePoint, pointOverPie, new Point(labelLinePoint.X, Center.Y));

            Double b = Graphics.DistanceBetweenTwoPoints(Center, intersectionPoint);

            Double angle = Math.Atan(b / XRadiusChart);
            angle = Math.PI / 2 - angle;

            if (angle > Math.PI / 2)
                return true;
            
            //  Graphics.DrawPointAt(labelLinePoint, Visual, Colors.Red);

            return false;
        }


        /// <summary>
        /// Place the Label at x, y.
        /// </summary>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        public void UpdateLabelVisualPosition()
        {
            Double x = Position.X, y = Position.Y;

            // For left side labels line should target to the right handside of the label
            if (x < Center.X)
                x = x - LabelVisual.Width;

            // if (y + LabelVisual.Height > YRadiusLabel * 2)
            //     y = YRadiusLabel * 2 - LabelVisual.Height;

            // if (y < 0)
            //     y = 0;

            // Set the position
            LabelVisual.SetValue(Canvas.LeftProperty, x);
            LabelVisual.SetValue(Canvas.TopProperty, y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="color"></param>
        internal void ColorIt(Color color)
        {
            (LabelVisual as Canvas).Background = new SolidColorBrush(color);
        }

        public Point Position { get; set; }
        public Double Height { get { return LabelVisual.Height; } }
        public Double Width  { get { return LabelVisual.Width;  } }
        public Double BaseMeanAngle;

        /// <summary>
        /// Current mean angle.
        /// Note: Current mean angle can go -Infinite to +Infinite
        /// </summary>
        public Double CurrentMeanAngle;

        public Point Center;
        public Double XRadiusLabel;
        public Double YRadiusLabel;
        public Double XRadiusChart;
        public Double YRadiusChart;
        private Canvas Visual;
        public  FrameworkElement LabelVisual;

        public CircularLabel PreviusLabel;
        public CircularLabel NextLabel;
        public Boolean IsLast;
        public Boolean IsFirst;
        public Boolean IsSkiped;
        public Rect Boundary;
        public Double MaxXPosition;
        public Double MaxYPosition;
        public Double MinXPosition;
        public Double MinYPosition;
    }


}