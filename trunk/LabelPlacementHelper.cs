using System;
using System.Windows;

namespace Visifire.Commons
{
    public class LabelPlacementHelper
    {
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
                            ShiftTowardsTop(shiftValue * 2, index, findSpaceAtIndex , ref labelsInfo);
                    }
                    else
                    {
                        findSpaceAtIndex = index;
                        spaceFoundAtIndex = FindSpaceTowardsBottomAtIndex(ref findSpaceAtIndex, shiftValue * 2, area, labelsInfo);

                        if (spaceFoundAtIndex != 0 || findSpaceAtIndex != -1) // if space found to shift towards top
                        {
                            if(index == findSpaceAtIndex)
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

                    for (Int32 i = index +1; i < labelsInfo.Length && shiftValue > 0; i++)
                    {
                        Double posibleShiftValue;

                        if (i == labelsInfo.Length - 1)
                            posibleShiftValue = shiftValue;
                        else
                            posibleShiftValue = CheckSpaceAtBottom(i, area, labelsInfo);

                        if (posibleShiftValue > 0 && shiftValue >= posibleShiftValue)
                        {
                            ShiftTowardsBottom(posibleShiftValue, index +1, i, ref labelsInfo);
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

            for (; index < labelsInfo.Length -1; index++)
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
                    if(labelsInfo[index].Top > (labelsInfo[index - 1].Top + labelsInfo[index - 1].Height))
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

    }
}