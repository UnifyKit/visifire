﻿/*   
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
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Markup;
using System.IO;
using System.Xml;


#else
using System;
using System.Windows;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Collections.Generic;

#endif

using Visifire.Commons;
using System.Windows.Input;
using System.Windows.Controls.Primitives;
using Visifire.Commons.Controls;

namespace Visifire.Charts
{
    /// <summary>
    /// ChartArea, the maximum area available for drawing a chart in Visifire Chart Control
    /// </summary>
    internal class ChartArea
    {
        #region Public Methods

        /// <summary>
        ///  Initializes a new instance of the Visifire.Charts.ChartArea class
        /// </summary>
        /// <param name="chart">Chart</param>
        public ChartArea(Chart chart)
        {   
            // Save the chart reference
            Chart = chart;
        }

        public void UpdateAxes()
        {   
            PlotDetails = new PlotDetails(Chart);

            ClearAxesPanel();
            PopulateInternalAxesXList();
            PopulateInternalAxesYList();

            if (PlotDetails.ChartOrientation != ChartOrientationType.NoAxis)
                SetAxesProperties();

            RenderAxes(this._plotAreaSize);

            SetAxesProperties();
        }

        /// <summary>
        /// Draw chartarea
        /// </summary>
        /// <param name="chart">Chart</param>
        public void Draw(Chart chart)
        {   
            isScrollingActive = Chart.IsScrollingActivated;
            //System.Diagnostics.Debug.WriteLine("Draw() > ");
            
            _renderCount = 0;

            Chart = chart;

            ClearAxesPanel();

            ResetTitleAndLegendPannelsSize();

            SetSeriesStyleFromTheme();

            SetTitleStyleFromTheme();

            SetDataPointColorFromColorSet(chart.Series);

            PopulateInternalAxesXList();

            PopulateInternalAxesYList();

            PopulateInternalSeriesList();

            CreateDefaultToolTipsForSeries(chart);

            PlotDetails = new PlotDetails(chart);

            SetLegendStyleFromTheme();

            CalculatePlankParameters();

            ClearTitlePanels();

            ClearLegendPanels();

            Boolean isLeftOrRightAlignedTitlesExist;
            Size actualChartSize = GetActualChartSize();

            // Add all the legends to chart of type dock outside
            AddTitles(chart, false, actualChartSize.Height, actualChartSize.Width, out isLeftOrRightAlignedTitlesExist);

            // Calculate max size for legend
            Size remainingSizeAfterAddingTitles = CalculateLegendMaxSize(actualChartSize);

            // Add all the legends to chart of type dock outside
            AddLegends(chart, false, remainingSizeAfterAddingTitles.Height, remainingSizeAfterAddingTitles.Width);

            // Create PlotArea
            CreatePlotArea(chart);

            // Calculate PlotArea Size
            _plotAreaSize = CalculatePlotAreaSize(remainingSizeAfterAddingTitles);

            // Need to recalculate PlotArea size if any title exist with left or right aligned
            if (isLeftOrRightAlignedTitlesExist)
            {   
                ClearTitlePanels();

                AddTitles(chart, false, _plotAreaSize.Height, actualChartSize.Width, out isLeftOrRightAlignedTitlesExist);

                ResetTitleAndLegendPannelsSize();

                remainingSizeAfterAddingTitles = CalculateLegendMaxSize(actualChartSize);

                _plotAreaSize = CalculatePlotAreaSize(remainingSizeAfterAddingTitles);
            }


            HideAllAxesScrollBars();

            // Check if drawing axis is necessary or not
            if (PlotDetails.ChartOrientation != ChartOrientationType.NoAxis)
                SetAxesProperties();

            Size remainingSize = DrawChart(_plotAreaSize);

            // Add all the titles to chart of type dock inside
            AddTitles(Chart, true, remainingSize.Height, remainingSize.Width, out isLeftOrRightAlignedTitlesExist);

            // Add all the legends to chart of type dock inside
            AddLegends(Chart, true, remainingSize.Height, remainingSize.Width);

            RetainOldScrollOffsetOfScrollViewer();

            // Chart.AttachEvents2Visual(Chart.PlotArea, PlotAreaCanvas);

            AttachOrDetachIntaractivity(chart.InternalSeries);

            if (!_isFirstTimeRender || !chart.AnimationEnabled)
            {   
                AttachScrollEvents();
                Visifire.Charts.Chart.SelectDataPoints(Chart);

                chart.Dispatcher.BeginInvoke(new Action(chart.UnlockRender));
            }

            chart._forcedRedraw = false;

            AddOrRemovePanels(chart);
        }

        /// <summary>
        /// Create multiple ToolTips for Line Series
        /// </summary>
        /// <param name="chart"></param>
        private void CreateDefaultToolTipsForSeries(Chart chart)
        {
            DisableIndicators();

            if (Chart.InternalSeries.Count > 0)
            {
                foreach (DataSeries ds in Chart.InternalSeries)
                {
                    if (Chart.IndicatorEnabled && (ds.RenderAs != RenderAs.Pie || ds.RenderAs != RenderAs.Doughnut
                        || ds.RenderAs != RenderAs.SectionFunnel || ds.RenderAs != RenderAs.StreamLineFunnel))
                    {
                        if (ds.ToolTipElement == null)
                        {
                            ToolTip toolTip = new ToolTip() { Chart = chart, Visibility = Visibility.Collapsed };
                            Chart.ToolTips.Add(toolTip);

                            ds.ToolTipElement = toolTip;
                           
                            Chart._toolTipCanvas.Children.Add(toolTip);
                        }
                    }
                    else
                    {
                        ds.RemoveToolTip();

                        DisableIndicators();
                    }
                }
            }
        }

        internal void DisableIndicators()
        {
            //if (Chart.IndicatorEnabled)
            {
                if (_axisIndicatorElement != null)
                    _axisIndicatorElement.Visibility = Visibility.Collapsed;
                if (_verticalLineIndicator != null)
                    _verticalLineIndicator.Visibility = Visibility.Collapsed;
                if (_callOutPath4AxisIndicator != null)
                    _callOutPath4AxisIndicator.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nearestDataPointList"></param>
        /// <param name="xOffset">x-offset used for shifting of cordinates among ChartArea and PlotArea </param>
        /// <param name="yOffset">y-offset used for shifting of cordinates among ChartArea and PlotArea </param>
        internal void ArrangeToolTips4DataSeries(List<DataPoint> nearestDataPointList, Double xPlotCanvasOffset, Double yPlotCanvasOffset)
        {
            Double xPlotOffset = xPlotCanvasOffset;
            Double yPlotOffset = yPlotCanvasOffset + (Chart.View3D ? PLANK_DEPTH : 0);

            if (AxisX.AxisOrientation == Orientation.Vertical)
                xPlotOffset += (Chart.View3D ? PLANK_THICKNESS : 0);

            Double plotWidth = PlotAreaCanvas.Width;
            Double plotHeight = PlotAreaCanvas.Height;

            Rect[] toolTipsPositionInfo = new Rect[nearestDataPointList.Count];
            Int32 index = 0;
            List<Point> listOfActualPos = new List<Point>();
            String axisIndicatorText = "";

            foreach (DataPoint dp in nearestDataPointList)
            {
                // Shifting of cordinates from PlotArea to ChartArea
                Point positionOfDp = new Point(dp._visualPosition.X + xPlotOffset, dp._visualPosition.Y + yPlotOffset);

                ToolTip toolTip = dp.Parent.ToolTipElement;
                Point toolTipPosition = positionOfDp;

                listOfActualPos.Add(new Point(toolTipPosition.X, toolTipPosition.Y));

                Double offset = AxisX.GetScrollBarValueFromOffset(AxisX.CurrentScrollScrollBarOffset);
                offset = GetScrollingOffsetOfAxis(AxisX, offset);
                // Calculate position with respect to ScrollViewer
                if (AxisX.AxisOrientation == Orientation.Horizontal)
                    toolTipPosition.X -= offset;
                else
                    toolTipPosition.Y -= offset;

                #region Set initial position of ToolTip

                toolTip.Measure(new Size(Double.MaxValue, Double.MaxValue));
                toolTip.UpdateLayout();

                if (toolTip._borderElement == null)
                    continue;

                Size toolTipSize = Visifire.Commons.Graphics.CalculateVisualSize(toolTip);

                if (toolTipPosition.X + toolTipSize.Width < plotWidth)
                {
                    if (dp.Parent.RenderAs == RenderAs.Line || dp.Parent.RenderAs == RenderAs.StepLine || dp.Parent.RenderAs == RenderAs.Area || dp.Parent.RenderAs == RenderAs.StackedArea || dp.Parent.RenderAs == RenderAs.StackedArea100
                        || dp.Parent.RenderAs == RenderAs.Bubble || dp.Parent.RenderAs == RenderAs.Point)
                    {
                        toolTipPosition.X = toolTipPosition.X + 10;
                        toolTipPosition.Y = toolTipPosition.Y - toolTipSize.Height / 2;
                        if (toolTipPosition.Y < 0)
                            toolTipPosition.Y = toolTipPosition.Y + 10;
                    }
                    else
                    {
                        if (dp.Parent.RenderAs == RenderAs.Bar || dp.Parent.RenderAs == RenderAs.StackedBar || dp.Parent.RenderAs == RenderAs.StackedBar100)
                        {
                            if (dp.YValue >= 0)
                            {
                                toolTipPosition.X = toolTipPosition.X + 10;
                                if (toolTipPosition.X > plotWidth)
                                    toolTipPosition.X = positionOfDp.X - toolTipSize.Width - 10;

                                toolTipPosition.Y = toolTipPosition.Y + 5;
                            }
                            else
                            {
                                toolTipPosition.X = toolTipPosition.X - toolTipSize.Width - 10;
                                if (toolTipPosition.X < 0)
                                    toolTipPosition.X = positionOfDp.X + 10;

                                toolTipPosition.Y = toolTipPosition.Y - toolTipSize.Height - 5;
                            }
                        }
                        else
                        {
                            toolTipPosition.X = toolTipPosition.X + 10;

                            if (dp.YValue >= 0 || (dp.Parent.RenderAs == RenderAs.CandleStick || dp.Parent.RenderAs == RenderAs.Stock))
                                toolTipPosition.Y = toolTipPosition.Y - toolTipSize.Height / 2 - 10;
                            else
                                toolTipPosition.Y = toolTipPosition.Y - toolTipSize.Height / 2 + 10;
                        }

                        if (toolTipPosition.Y < 0)
                            toolTipPosition.Y = toolTipPosition.Y + 20;
                    }
                    toolTip.SetValue(Canvas.LeftProperty, toolTipPosition.X);
                    toolTip.SetValue(Canvas.TopProperty, toolTipPosition.Y);
                }
                else
                {
                    if (dp.Parent.RenderAs == RenderAs.Line || dp.Parent.RenderAs == RenderAs.StepLine || dp.Parent.RenderAs == RenderAs.Area || dp.Parent.RenderAs == RenderAs.StackedArea || dp.Parent.RenderAs == RenderAs.StackedArea100
                        )
                    {
                        toolTipPosition.X = toolTipPosition.X - toolTipSize.Width - 10;
                        toolTipPosition.Y = toolTipPosition.Y - toolTipSize.Height / 2;
                        if (toolTipPosition.Y < 0)
                            toolTipPosition.Y = toolTipPosition.Y + 10;
                    }
                    else
                    {
                        if (dp.Parent.RenderAs == RenderAs.Bar || dp.Parent.RenderAs == RenderAs.StackedBar || dp.Parent.RenderAs == RenderAs.StackedBar100)
                        {
                            toolTipPosition.X = toolTipPosition.X - toolTipSize.Width - 10;

                            toolTipPosition.Y = toolTipPosition.Y + 5;

                        }
                        else
                        {
                            toolTipPosition.X = toolTipPosition.X - toolTipSize.Width - 10;
                            if (dp.YValue >= 0 || (dp.Parent.RenderAs == RenderAs.CandleStick || dp.Parent.RenderAs == RenderAs.Stock))
                                toolTipPosition.Y = toolTipPosition.Y - toolTipSize.Height / 2 - 10;
                            else
                                toolTipPosition.Y = toolTipPosition.Y - toolTipSize.Height / 2 + 10;
                        }

                        if (toolTipPosition.Y < 0)
                            toolTipPosition.Y = toolTipPosition.Y + 20;
                    }
                    toolTip.SetValue(Canvas.LeftProperty, toolTipPosition.X);
                    toolTip.SetValue(Canvas.TopProperty, toolTipPosition.Y);
                }

                Rect toolTipRect = new Rect(toolTipPosition, toolTipSize);
                toolTipsPositionInfo[index++] = toolTipRect;

                #endregion
            }

            Rect baseArea = new Rect(0, 0, Chart.ActualWidth, Chart.ActualHeight);
            LabelPlacementHelper.VerticalLabelPlacement(baseArea, ref toolTipsPositionInfo);
            DataPoint dpOfFirstDataSeries = null;

            for (index = 0; index < toolTipsPositionInfo.Length; index++)
            {
                DataPoint dp = nearestDataPointList[index];

                if (dp.Parent.ToolTipElement._borderElement == null)
                    continue;

                Double offset = AxisX.GetScrollBarValueFromOffset(AxisX.CurrentScrollScrollBarOffset);
                offset = GetScrollingOffsetOfAxis(AxisX, offset);

                if (dp.Parent.RenderAs == RenderAs.CandleStick)
                {
                    if (dp.Parent.ToolTipElement._callOutPath != null)
                        dp.Parent.ToolTipElement._callOutPath.Fill = (Boolean)dp.Parent.LightingEnabled ? Graphics.GetLightingEnabledBrush(dp.Parent.PriceUpColor, "Linear", new Double[] { 0.80, 0.55 }) : dp.Parent.PriceUpColor;
                    dp.Parent.ToolTipElement.Background = (Boolean)dp.Parent.LightingEnabled ? Graphics.GetLightingEnabledBrush(dp.Parent.PriceUpColor, "Linear", new Double[] { 0.80, 0.55 }) : dp.Parent.PriceUpColor;
                }
                else
                {
                    if (dp.Parent.ToolTipElement._callOutPath != null)
                        dp.Parent.ToolTipElement._callOutPath.Fill = (Boolean)dp.Parent.LightingEnabled ? Graphics.GetLightingEnabledBrush(dp.Parent.Color, "Linear", new Double[] { 0.80, 0.55 }) : dp.Parent.Color;
                    dp.Parent.ToolTipElement.Background = (Boolean)dp.Parent.LightingEnabled ? Graphics.GetLightingEnabledBrush(dp.Parent.Color, "Linear", new Double[] { 0.80, 0.55 }) : dp.Parent.Color;
                }

                if (dp.Parent.ToolTipElement._callOutPath != null && dp.Parent.ToolTipElement._callOutPath.Fill == null && Chart._toolTip != null)
                    dp.Parent.ToolTipElement._callOutPath.Fill = Chart._toolTip.Background;

                if (dp.Parent.ToolTipElement.Background == null && Chart._toolTip != null)
                    dp.Parent.ToolTipElement.Background = Chart._toolTip.Background;

                dp.Parent.ToolTipElement.SetValue(Canvas.LeftProperty, toolTipsPositionInfo[index].Left);
                dp.Parent.ToolTipElement.SetValue(Canvas.TopProperty, toolTipsPositionInfo[index].Top);

                Point positionOfDp = new Point(dp._visualPosition.X + xPlotOffset - offset, dp._visualPosition.Y + yPlotOffset);
                if (AxisX.AxisOrientation == Orientation.Vertical)
                {
                    positionOfDp.X += offset;
                    positionOfDp.Y -= offset;
                }

                if (dp.Parent.RenderAs == RenderAs.Bar || dp.Parent.RenderAs == RenderAs.StackedBar || dp.Parent.RenderAs == RenderAs.StackedBar100)
                {
                    if (toolTipsPositionInfo[index].Left + dp.Parent.ToolTipElement._borderElement.ActualWidth < dp._visualPosition.X + xPlotOffset)
                    {
                        dp.Parent.ToolTipElement.CallOutStartPoint = new Point(dp.Parent.ToolTipElement._borderElement.ActualWidth - 1, dp.Parent.ToolTipElement._borderElement.ActualHeight / 4);
                        dp.Parent.ToolTipElement.CallOutMidPoint = new Point(dp.Parent.ToolTipElement._borderElement.ActualWidth + (positionOfDp.X - (toolTipsPositionInfo[index].Left + dp.Parent.ToolTipElement._borderElement.ActualWidth)), positionOfDp.Y - toolTipsPositionInfo[index].Top);
                        dp.Parent.ToolTipElement.CallOutEndPoint = new Point(dp.Parent.ToolTipElement._borderElement.ActualWidth - 1, dp.Parent.ToolTipElement._borderElement.ActualHeight - dp.Parent.ToolTipElement._borderElement.ActualHeight / 4);

                        dp.Parent.ToolTipElement._borderElement.BorderThickness = new Thickness(0.25, 0.25, 0, 1);
                    }
                    else
                    {
                        dp.Parent.ToolTipElement.CallOutStartPoint = new Point(1, dp.Parent.ToolTipElement._borderElement.ActualHeight / 4);
                        dp.Parent.ToolTipElement.CallOutMidPoint = new Point(positionOfDp.X - toolTipsPositionInfo[index].Left, positionOfDp.Y - toolTipsPositionInfo[index].Top);
                        dp.Parent.ToolTipElement.CallOutEndPoint = new Point(1, dp.Parent.ToolTipElement._borderElement.ActualHeight - dp.Parent.ToolTipElement._borderElement.ActualHeight / 4);

                        dp.Parent.ToolTipElement._borderElement.BorderThickness = new Thickness(0, 0.25, 0.25, 1);
                    }
                }
                else
                {
                    if (listOfActualPos[index].X - offset + dp.Parent.ToolTipElement._borderElement.ActualWidth < plotWidth)
                    {
                        dp.Parent.ToolTipElement.CallOutStartPoint = new Point(1, dp.Parent.ToolTipElement._borderElement.ActualHeight / 4);
                        dp.Parent.ToolTipElement.CallOutMidPoint = new Point(positionOfDp.X - toolTipsPositionInfo[index].Left, positionOfDp.Y - toolTipsPositionInfo[index].Top);
                        dp.Parent.ToolTipElement.CallOutEndPoint = new Point(1, dp.Parent.ToolTipElement._borderElement.ActualHeight - dp.Parent.ToolTipElement._borderElement.ActualHeight / 4);

                        dp.Parent.ToolTipElement._borderElement.BorderThickness = new Thickness(0, 0.25, 0.25, 1);
                    }
                    else
                    {
                        dp.Parent.ToolTipElement.CallOutStartPoint = new Point(dp.Parent.ToolTipElement._borderElement.ActualWidth - 1, dp.Parent.ToolTipElement._borderElement.ActualHeight / 4);
                        dp.Parent.ToolTipElement.CallOutMidPoint = new Point(dp.Parent.ToolTipElement._borderElement.ActualWidth + (positionOfDp.X - (toolTipsPositionInfo[index].Left + dp.Parent.ToolTipElement._borderElement.ActualWidth)), positionOfDp.Y - toolTipsPositionInfo[index].Top);
                        dp.Parent.ToolTipElement.CallOutEndPoint = new Point(dp.Parent.ToolTipElement._borderElement.ActualWidth - 1, dp.Parent.ToolTipElement._borderElement.ActualHeight - dp.Parent.ToolTipElement._borderElement.ActualHeight / 4);

                        dp.Parent.ToolTipElement._borderElement.BorderThickness = new Thickness(0.25, 0.25, 0, 1);
                    }
                }

                #region Get the desired DataPoint whose value can be displayed inside Axis Indicator

                if (Chart.Series.IndexOf(dp.Parent) == 0)
                    dpOfFirstDataSeries = dp;
                else
                {
                    // If the current DataPoint of series does not belongs to the first Dataseries of chart then 
                    // find out the nearest DataPoint from first DataSeries.
                    DataPoint nearsestDPOfFirstDS = null;
                    foreach (DataPoint dp1 in Chart.Series[0].DataPoints)
                    {
                        //if(dp1.Faces != null && dp1.Faces.Visual != null)
                        //    dp1._x_distance = Math.Abs(dp._visualPosition.X - (Double)((Double)dp1.Faces.Visual.GetValue(Canvas.LeftProperty) + dp1.Faces.Visual.Width / 2));
                        //else
                        dp1._x_distance = Math.Abs(dp._visualPosition.X - dp1._visualPosition.X);

                        if (nearsestDPOfFirstDS == null)
                        {
                            nearsestDPOfFirstDS = dp1;
                            continue;
                        }

                        if (dp1._x_distance < nearsestDPOfFirstDS._x_distance)
                            nearsestDPOfFirstDS = dp1;
                    }

                    if (dpOfFirstDataSeries == null)
                        dpOfFirstDataSeries = nearsestDPOfFirstDS;
                }

                if (dpOfFirstDataSeries != null)
                {
                    if (dpOfFirstDataSeries.InternalXValue != dp.InternalXValue)
                        dpOfFirstDataSeries = dp;
                }

                if (!AxisX.IsDateTimeAxis)
                {
                    if (dpOfFirstDataSeries != null)
                    {
                        if (dpOfFirstDataSeries.AxisXLabel != null)
                            axisIndicatorText = dpOfFirstDataSeries.AxisXLabel;
                        else
                            axisIndicatorText = dpOfFirstDataSeries.InternalXValue.ToString();
                    }
                }
                else
                {
                    String valueFormatString = AxisX.XValueType == ChartValueTypes.Date ? "M/d/yyyy" : AxisX.XValueType == ChartValueTypes.Time ? "h:mm:ss tt" : "M/d/yyyy h:mm:ss tt";
                    valueFormatString = (String.IsNullOrEmpty((String)AxisX.GetValue(Axis.ValueFormatStringProperty))) ? valueFormatString : AxisX.ValueFormatString;

                    DateTime dt = Convert.ToDateTime(dp.XValue);
                    axisIndicatorText = dt.ToString(valueFormatString);
                }

                #endregion
            }

            if (listOfActualPos.Count > 0)
            {

                #region Finding out the middle position of an XValue in a multi-series chart in order to position Indicator

                Double newPos = 0;

                // Find out the middle position of an XValue in case of multiseries chart so that
                // indicator can be displayed in the middle. Please note that for multiseries chart
                // one XValue can have more than one DataPoint so we need to find out the middle position
                // in order to place indicator.
                DataPoint dpAtIndexZero = null;
                if (PlotDetails.DrawingDivisionFactor > 1)
                {
                    var dps = (from dp1 in nearestDataPointList
                               where ((dp1.Parent.RenderAs == RenderAs.Column
                               || dp1.Parent.RenderAs == RenderAs.StackedColumn
                               || dp1.Parent.RenderAs == RenderAs.StackedColumn100
                               || dp1.Parent.RenderAs == RenderAs.Bar
                               || dp1.Parent.RenderAs == RenderAs.StackedBar
                               || dp1.Parent.RenderAs == RenderAs.StackedBar100)
                               && PlotDetails.GetSeriesFromDataPoint(dp1).Count > 1)
                               select dp1);

                    List<DataSeries> indexSeriesList = null;

                    foreach (DataPoint dp in dps)
                    {
                        indexSeriesList = PlotDetails.GetSeriesFromDataPoint(dp);
                        if (indexSeriesList != null && indexSeriesList.Count > 1)
                        {
                            Int32 drawingIndex = indexSeriesList.IndexOf(dp.Parent);
                            if (drawingIndex == 0)
                            {
                                dpAtIndexZero = dp;
                                break;
                            }
                        }
                    }

                    if (dpAtIndexZero != null && dpAtIndexZero.Faces != null && dpAtIndexZero.Faces.Visual != null)
                    {
                        Double visualSize = 0;
                        if (dpAtIndexZero.Parent.RenderAs == RenderAs.Bar || dpAtIndexZero.Parent.RenderAs == RenderAs.StackedBar
                            || dpAtIndexZero.Parent.RenderAs == RenderAs.StackedBar100)
                        {
                            newPos = (dpAtIndexZero._visualPosition.Y + yPlotOffset - dpAtIndexZero.Faces.Visual.Height / 2);
                            visualSize = dpAtIndexZero.Faces.Visual.Height;
                        }
                        else
                        {
                            newPos = (dpAtIndexZero._visualPosition.X + xPlotOffset - dpAtIndexZero.Faces.Visual.Width / 2);
                            visualSize = dpAtIndexZero.Faces.Visual.Width;
                        }

                        if (dps != null && dps.Count() > 0)
                            newPos += ((visualSize * dps.Count()) / 2);
                    }
                    else
                    {
                        if (dps != null && dps.Count() > 0)
                        {
                            DataPoint firstDpInXValue = dps.First();

                            if (AxisX.AxisOrientation == Orientation.Horizontal)
                            {
                                if (firstDpInXValue.Faces != null && firstDpInXValue.Faces.Visual != null)
                                {
                                    newPos = firstDpInXValue._visualPosition.X + xPlotOffset - firstDpInXValue.Faces.Visual.Width / 2;

                                    newPos += (firstDpInXValue.Faces.Visual.Width * dps.Count()) / 2;
                                }
                                else
                                    newPos = firstDpInXValue._visualPosition.X + xPlotOffset;
                            }
                            else
                            {
                                if (firstDpInXValue.Faces != null && firstDpInXValue.Faces.Visual != null)
                                {
                                    newPos = firstDpInXValue._visualPosition.Y + yPlotOffset - firstDpInXValue.Faces.Visual.Height / 2;
                                    newPos += (firstDpInXValue.Faces.Visual.Height * dps.Count()) / 2;
                                }
                                else
                                    newPos = firstDpInXValue._visualPosition.Y + yPlotOffset;
                            }

                            if (!AxisX.IsDateTimeAxis)
                            {
                                if (firstDpInXValue != null)
                                {
                                    if (firstDpInXValue.AxisXLabel != null)
                                        axisIndicatorText = firstDpInXValue.AxisXLabel;
                                    else
                                        axisIndicatorText = firstDpInXValue.InternalXValue.ToString();
                                }
                            }
                            else
                            {
                                String valueFormatString = AxisX.XValueType == ChartValueTypes.Date ? "M/d/yyyy" : AxisX.XValueType == ChartValueTypes.Time ? "h:mm:ss tt" : "M/d/yyyy h:mm:ss tt";
                                valueFormatString = (String.IsNullOrEmpty((String)AxisX.GetValue(Axis.ValueFormatStringProperty))) ? valueFormatString : AxisX.ValueFormatString;

                                DateTime dt = Convert.ToDateTime(firstDpInXValue.XValue);
                                axisIndicatorText = dt.ToString(valueFormatString);
                            }
                        }
                        else
                        {
                            if (AxisX.AxisOrientation == Orientation.Horizontal)
                            {
                                newPos = listOfActualPos[0].X;
                            }
                            else
                                newPos = listOfActualPos[0].Y;

                        }

                        //if (AxisX.AxisOrientation == Orientation.Horizontal)
                        //{
                        //    newPos = listOfActualPos[0].X;
                        //}
                        //else
                        //    newPos = listOfActualPos[0].Y;
                    }
                }
                else
                {
                    foreach (Point point in listOfActualPos)
                    {
                        if (AxisX.AxisOrientation == Orientation.Horizontal)
                        {
                            if (dpOfFirstDataSeries != null)
                            {
                                if (point.X == dpOfFirstDataSeries._visualPosition.X + xPlotOffset)
                                    newPos = point.X;
                            }
                            else
                                newPos = Double.NaN;
                        }
                        else
                        {
                            if (dpOfFirstDataSeries != null)
                            {
                                if (point.Y == dpOfFirstDataSeries._visualPosition.Y + yPlotOffset)
                                    newPos = point.Y;
                            }
                            else
                                newPos = Double.NaN;
                        }

                    }
                    //newPos = Double.NaN;
                }

                #endregion

                #region Position Indicator

                if (AxisX.AxisOrientation == Orientation.Horizontal)
                {
                    if (!Double.IsNaN(newPos))
                        SetPositon4Indicators(AxisX, newPos, axisIndicatorText, xPlotCanvasOffset, yPlotCanvasOffset, plotWidth, plotHeight);
                    else
                        SetPositon4Indicators(AxisX, listOfActualPos[0].X, axisIndicatorText, xPlotCanvasOffset, yPlotCanvasOffset, plotWidth, plotHeight);
                }
                else
                {
                    if (!Double.IsNaN(newPos))
                        SetPositon4Indicators(AxisX, newPos, axisIndicatorText, xPlotCanvasOffset, yPlotCanvasOffset, plotWidth, plotHeight);
                    else
                        SetPositon4Indicators(AxisX, listOfActualPos[0].Y, axisIndicatorText, xPlotCanvasOffset, yPlotCanvasOffset, plotWidth, plotHeight);
                }

                #endregion
            }

            System.Diagnostics.Debug.WriteLine("Called..." + new Random().Next().ToString());

        }

        private void AddOrRemovePanels(Chart chart)
        {
            if (isScrollingActive && Chart.IsScrollingActivated)
            {
                chart._centerInnerGrid.Children.Remove(PlotAreaCanvas);

                if (!chart._drawingCanvas.Children.Contains(PlotAreaCanvas))
                    chart._drawingCanvas.Children.Add(PlotAreaCanvas);
            }
            else if (!isScrollingActive && !Chart.IsScrollingActivated)
            {
                chart._drawingCanvas.Children.Remove(PlotAreaCanvas);

                if (!chart._centerInnerGrid.Children.Contains(PlotAreaCanvas))
                    chart._centerInnerGrid.Children.Add(PlotAreaCanvas);
            }
            else if (!isScrollingActive && Chart.IsScrollingActivated)
            {
                chart._centerInnerGrid.Children.Remove(PlotAreaCanvas);

                if (!chart._drawingCanvas.Children.Contains(PlotAreaCanvas))
                    chart._drawingCanvas.Children.Add(PlotAreaCanvas);
            }
            else if (isScrollingActive && !Chart.IsScrollingActivated)
            {
                chart._drawingCanvas.Children.Remove(PlotAreaCanvas);

                if (!chart._centerInnerGrid.Children.Contains(PlotAreaCanvas))
                    chart._centerInnerGrid.Children.Add(PlotAreaCanvas);
            }
        }

        /// <summary>
        /// Update the axis
        /// </summary>
        /// <param name="isSizeChanged"></param>
        internal void PrePartialUpdateConfiguration(VisifireElement sender, VcProperties property, object oldValue, object newValue, Boolean updateLists, Boolean calculatePlotDetails, Boolean updateAxis, AxisRepresentations renderAxisType, Boolean isPartialUpdate)
        {   
            if(updateLists)
                PopulateInternalSeriesList();

            if (calculatePlotDetails)
            {
                // PlotDetails = new PlotDetails(Chart);
                
                PlotDetails.ReCreate(sender, property, oldValue, newValue);
            }

            if (updateLists)
            {
                SetDataPointColorFromColorSet(Chart.Series);
            }

            if (updateAxis)
            {   
                PopulateInternalAxesXList();
                PopulateInternalAxesYList();

                ClearAxesPanel();

                //  Check if drawing axis is necessary or not
                // if (PlotDetails.ChartOrientation != ChartOrientationType.NoAxis)
                //   SetAxesProperties();

                Size remainingSizeAfterDrawingAxes = RenderAxes(_plotAreaSize);
                
                ResizePanels(remainingSizeAfterDrawingAxes, renderAxisType, isPartialUpdate);

                // Draw the chart grids
                if (PlotDetails.ChartOrientation != ChartOrientationType.NoAxis)
                {
                    RenderGrids();
                    RenderTrendLines();
                }

                
            }

            AddOrRemovePanels(Chart);
        }

        /// <summary>
        /// Attach or detach intaractivity for selection
        /// </summary>
        /// <param name="chart"></param>
        internal void AttachOrDetachIntaractivity(List<DataSeries> series)
        {
            foreach (DataSeries ds in series)
            {
                if (_isFirstTimeRender)
                {
                    if (ds.SelectionEnabled)
                        ds.AttachOrDetachIntaractivity();
                }
                else
                    ds.AttachOrDetachIntaractivity();
            }
        }


        #endregion

        #region Public Properties

        /// <summary>
        ///  PlotAreaCanvas is the canvas where PlottingCanvas is drawn
        /// </summary>
        public Canvas PlotAreaCanvas { get; set; }

        /// <summary>
        /// Plotting Canvas is the canvas where planks are drawn in 3d view. 
        /// It also contains ChartVisualCanvas
        /// </summary>
        public Canvas PlottingCanvas { get; set; }

        /// <summary>
        ///  ChartVisualCanvas is the canvas where grids, trendlines and datapoints are drawn. 
        /// </summary>
        public Canvas ChartVisualCanvas { get; set; }

        /// <summary>
        /// Chart scrollviewer is the main viewport of the scrollable chart. 
        /// Is loaded from generic.xaml and its nothing but PlotAreaScrollViewer
        /// </summary>
        public ScrollViewer PlotAreaScrollViewer { get; set; }

        /// <summary>
        /// Chart reference
        /// </summary>
        public Chart Chart  { get;  set; }

        #endregion

        #region Public Events

        #endregion

        #region Protected Methods

        #endregion

        #region Internal Properties

        /// <summary>
        /// Containes all the details about the data required for various plotting purposes
        /// </summary>
        internal PlotDetails PlotDetails
        {
            get
            {
                return Chart.PlotDetails;
            }
            set
            {
                Chart.PlotDetails = value;
            }
        }

        /// <summary>
        /// Plank offset in 3d view
        /// </summary>
        internal Double PLANK_OFFSET
        {
            get;
            private set;
        }

        /// <summary>
        /// Plank depth in 3d view
        /// </summary>
        internal Double PLANK_DEPTH
        {
            get;
            private set;
        }

        /// <summary>
        /// Plank thickness in 3d view
        /// </summary>
        internal Double PLANK_THICKNESS
        {
            get;
            private set;
        }

        /// <summary>
        /// Current axis-x associated with chart
        /// </summary>
        internal Axis AxisX
        {
            get;
            set;
        }

        /// <summary>
        /// Current secondary axis-x associated with chart
        /// </summary>
        internal Axis AxisX2
        {
            get;
            set;
        }

        /// <summary>
        /// Current axis-y associated with chart
        /// </summary>
        internal Axis AxisY
        {
            get;
            set;
        }

        /// <summary>
        /// Current secondary axis-y associated with chart
        /// </summary>
        internal Axis AxisY2
        {
            get;
            set;
        }

        /// <summary>
        /// Scrollable length of the plotarea inside ScrollViewer
        /// </summary>
        internal Double ScrollableLength { get; set; }

        #endregion

        #region Private Delegates

        #endregion

        #region Private Methods

        /// <summary>
        /// Returns ActualSize of the chart
        /// </summary>
        /// <returns></returns>
        private Size GetActualChartSize()
        {   
            Size chartActualSize = new Size(Chart._chartBorder.ActualWidth, Chart._chartBorder.ActualHeight);
            chartActualSize.Width -= (Chart._chartBorder.Padding.Left + Chart._chartBorder.Padding.Right);
            chartActualSize.Height -= (Chart._chartBorder.Padding.Top + Chart._chartBorder.Padding.Bottom);
            chartActualSize.Width -= (Chart._chartBorder.BorderThickness.Left + Chart._chartBorder.BorderThickness.Right);
            chartActualSize.Height -= (Chart._chartBorder.BorderThickness.Top + Chart._chartBorder.BorderThickness.Bottom);
            chartActualSize.Width -= (Chart._chartAreaGrid.Margin.Left + Chart._chartAreaGrid.Margin.Right);
            chartActualSize.Height -= (Chart._chartAreaGrid.Margin.Top + Chart._chartAreaGrid.Margin.Bottom);

            if (Chart.Bevel)
            {
                chartActualSize.Height -= Chart.BEVEL_DEPTH;
            }
            
            return chartActualSize;
        }
        
        /// <summary>
        /// Clear Axes Panels
        /// </summary>
        private void ClearAxesPanel()
        {
            Chart._leftAxisPanel.Children.Clear();
            Chart._bottomAxisPanel.Children.Clear();
            Chart._topAxisPanel.Children.Clear();
            Chart._rightAxisPanel.Children.Clear();

            Chart._leftAxisPanel.UpdateLayout();
            Chart._bottomAxisPanel.UpdateLayout();
            Chart._topAxisPanel.UpdateLayout();
            Chart._rightAxisPanel.UpdateLayout();
        }

        /// <summary>
        ///  Clear all title panels
        /// </summary>
        private void ClearTitlePanels()
        {
            Chart._topOuterTitlePanel.Children.Clear();
            Chart._rightOuterTitlePanel.Children.Clear();
            Chart._leftOuterTitlePanel.Children.Clear();
            Chart._bottomOuterTitlePanel.Children.Clear();
            
            Chart._topInnerTitlePanel.Children.Clear();
            Chart._rightInnerTitlePanel.Children.Clear();
            Chart._leftInnerTitlePanel.Children.Clear();
            Chart._bottomInnerTitlePanel.Children.Clear();
        }

        /// <summary>
        /// Clear all legend panels
        /// </summary>
        private void ClearLegendPanels()
        {
            Chart._topInnerLegendPanel.Children.Clear();
            Chart._bottomInnerLegendPanel.Children.Clear();
            Chart._leftInnerLegendPanel.Children.Clear();
            Chart._rightInnerLegendPanel.Children.Clear();

            Chart._topOuterLegendPanel.Children.Clear();
            Chart._bottomOuterLegendPanel.Children.Clear();
            Chart._leftOuterLegendPanel.Children.Clear();
            Chart._rightOuterLegendPanel.Children.Clear();

            Chart._centerDockInsidePlotAreaPanel.Children.Clear();
            Chart._centerDockOutsidePlotAreaPanel.Children.Clear();
        }

        /// <summary>
        /// Retain old ScrollOffset value of chart ScrollViewer to stop auto scrolling with keys for wpf only. 
        /// For Silverlight IsTabStop property is already set to false 
        /// </summary>
        private void RetainOldScrollOffsetOfScrollViewer()
        {
#if WPF     
            // The code below is to stop scrolling using key
            if (_isFirstTimeRender)
            {   
                Chart._plotAreaScrollViewer.ScrollChanged += delegate(object sender, ScrollChangedEventArgs e)
                {   
                    if (PlotDetails.ChartOrientation != ChartOrientationType.NoAxis && AxisX != null)
                    {
                        Double offset = AxisX.GetScrollBarValueFromOffset(AxisX.CurrentScrollScrollBarOffset);
                        offset = GetScrollingOffsetOfAxis(AxisX, offset);

                        if (PlotDetails.ChartOrientation == ChartOrientationType.Horizontal)
                            Chart._plotAreaScrollViewer.ScrollToVerticalOffset(offset);
                        else
                            Chart._plotAreaScrollViewer.ScrollToHorizontalOffset(offset);

                        /*
                         * if (AxisX.ScrollViewerElement.Children.Count > 0)
                        {
                            FrameworkElement scrollViewerContent = (AxisX.ScrollViewerElement.Children[0] as FrameworkElement);
                            Double value = AxisX.GetScrollBarValueFromOffset(AxisX.CurrentScrollScrollBarOffset);
                            Double scrollViewerOffset;

                            if (PlotDetails.ChartOrientation == ChartOrientationType.Horizontal)
                            {
                                scrollViewerOffset = ChartVisualCanvas.Height - PlotAreaScrollViewer.ViewportHeight;
                                scrollViewerOffset = (Chart as Chart).ZoomingEnabled ? value * (scrollViewerOffset / ZOOMING_MAX_VAL) : value;
                                Chart._plotAreaScrollViewer.ScrollToVerticalOffset(scrollViewerOffset);
                            }
                            else
                            {
                                scrollViewerOffset = scrollViewerContent.ActualWidth - PlotAreaScrollViewer.ViewportWidth;
                                scrollViewerOffset = (Chart as Chart).ZoomingEnabled ? value * (scrollViewerOffset / ZOOMING_MAX_VAL) : value;
                                Chart._plotAreaScrollViewer.ScrollToHorizontalOffset(scrollViewerOffset);                                
                                                 
                                // Double scrollViewerOffset = (Chart as Chart).ZoomingEnabled ? value * (scrollViewerContent.ActualWidth / ZOOMING_MAX_VAL) : value;
                                // Chart._plotAreaScrollViewer.ScrollToHorizontalOffset(scrollViewerOffset);    
                            }
                        }*/
                    }
                };
            }
#endif
        }

        internal Double GetScrollingOffsetOfAxis(Axis axis, Double offset)
        {
            Chart chart = axis.Chart as Chart;
            Double value = offset;

            if(chart.ZoomingEnabled)
            {
                Double scrollViewerOffset = 0;

                if(axis.ScrollViewerElement.Children.Count > 0)
                {   
                    FrameworkElement scrollViewerContent = (AxisX.ScrollViewerElement.Children[0] as FrameworkElement);

                    // Vertical chart
                    if(axis.AxisOrientation == Orientation.Horizontal)
                    {   
                        //Double MaxHorizontalOffset = scrollViewerContent.Width - PlotAreaScrollViewer.ViewportWidth;
                        Double MaxHorizontalOffset = ScrollableLength - PlotAreaScrollViewer.ViewportWidth;
                        scrollViewerOffset = value * (MaxHorizontalOffset / ZOOMING_MAX_VAL);
                    }   
                    else
                    {
                        Double MaxVerticalOffset = ScrollableLength - PlotAreaScrollViewer.ViewportHeight;
                        scrollViewerOffset = value * (MaxVerticalOffset / ZOOMING_MAX_VAL);
                    }
                }

                return scrollViewerOffset;
            }
            else
            {   
                return value;
            }
        }


        /// <summary>
        /// Hide all axis scrollbars
        /// </summary>
        private void HideAllAxesScrollBars()
        {   
            Chart._leftAxisScrollBar.Visibility = Visibility.Collapsed;
            Chart._rightAxisScrollBar.Visibility = Visibility.Collapsed;
            Chart._bottomAxisScrollBar.Visibility = Visibility.Collapsed;
            Chart._topAxisScrollBar.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Reset all panels of titles(DockedOutSide PlotArea) and legends(DockedOutSide PlotArea) to 0
        /// </summary>
        private void ResetTitleAndLegendPannelsSize()
        {
            Chart._leftOuterTitlePanel.Width = 0;
            Chart._rightOuterTitlePanel.Width = 0;
            Chart._bottomOuterTitlePanel.Height = 0;
            Chart._topOuterTitlePanel.Height = 0;

            Chart._leftOuterLegendPanel.Width = 0;
            Chart._rightOuterLegendPanel.Width = 0;
            Chart._bottomOuterLegendPanel.Height = 0;
            Chart._topOuterLegendPanel.Height = 0;
        }

        /// <summary>
        /// Get size of the PlotArea
        /// </summary>
        /// <param name="boundingRec">Bounding rectangle size</param>
        /// <returns></returns>
        private Size CalculatePlotAreaSize(Size boundingRec)
        {
            Size elementSize;

            foreach (Legend legend in Chart.Legends)
            {   
                if (legend.DockInsidePlotArea || !(Boolean)legend.Enabled || legend.Visual == null)
                    continue;

                elementSize = Graphics.CalculateVisualSize(legend.Visual);

                if (legend.InternalVerticalAlignment == VerticalAlignment.Bottom || legend.InternalVerticalAlignment == VerticalAlignment.Top)
                {
                    boundingRec.Height -= elementSize.Height;
                    if (legend.InternalVerticalAlignment == VerticalAlignment.Bottom)
                        Chart._bottomOuterLegendPanel.Height += elementSize.Height;
                    else
                        Chart._topOuterLegendPanel.Height += elementSize.Height;
                }
                else if (legend.InternalVerticalAlignment == VerticalAlignment.Center || legend.InternalVerticalAlignment == VerticalAlignment.Stretch)
                {   
                    if (legend.InternalHorizontalAlignment == HorizontalAlignment.Left || legend.InternalHorizontalAlignment == HorizontalAlignment.Right)
                    {
                        boundingRec.Width -= elementSize.Width;
                        if (legend.InternalHorizontalAlignment == HorizontalAlignment.Left)
                            Chart._leftOuterLegendPanel.Width += elementSize.Width;
                        else
                            Chart._rightOuterLegendPanel.Width += elementSize.Width;
                    }
                }
            }

            Chart._centerOuterGrid.Height = boundingRec.Height;
            Chart._centerOuterGrid.Width = boundingRec.Width;
            PlotAreaCanvas.Height = boundingRec.Height;
            PlotAreaCanvas.Width = boundingRec.Width;

            return boundingRec;
        }
        
        /// <summary>
        /// Create the center layout of the ChartArea
        /// </summary>
        /// <param name="chart">Chart</param>
        private void CreatePlotArea(Chart chart)
        {
            if (Chart.PlotArea == null)
            {
                Chart.PlotArea = new PlotArea();
            }

            Chart.PlotArea.Chart = Chart;

            if (Chart.PlotArea.Visual == null)
            {
                if (!String.IsNullOrEmpty(Chart.Theme))
                    Chart.PlotArea.ApplyStyleFromTheme(Chart, "PlotArea");

                Chart.PlotArea.CreateVisualObject();
                PlotAreaCanvas = Chart.PlotArea.Visual;

                Chart.AttachEvents2Visual(Chart.PlotArea, PlotAreaCanvas);
                PlotAreaCanvas.MouseMove -= new MouseEventHandler(PlotAreaCanvas_MouseMove);
                PlotAreaCanvas.MouseLeave -= new MouseEventHandler(PlotAreaCanvas_MouseLeave);
                PlotAreaCanvas.MouseLeftButtonDown -= new MouseButtonEventHandler(PlotAreaCanvas_MouseLeftButtonDown);
                PlotAreaCanvas.MouseLeftButtonUp -= new MouseButtonEventHandler(PlotAreaCanvas_MouseLeftButtonUp);
                PlotAreaCanvas.MouseMove += new MouseEventHandler(PlotAreaCanvas_MouseMove);
                PlotAreaCanvas.MouseLeave += new MouseEventHandler(PlotAreaCanvas_MouseLeave);
                PlotAreaCanvas.MouseLeftButtonDown += new MouseButtonEventHandler(PlotAreaCanvas_MouseLeftButtonDown);
                PlotAreaCanvas.MouseLeftButtonUp += new MouseButtonEventHandler(PlotAreaCanvas_MouseLeftButtonUp);
            }
            else
                Chart.PlotArea.UpdateProperties();

            PlotAreaCanvas.HorizontalAlignment = HorizontalAlignment.Stretch;
            PlotAreaCanvas.VerticalAlignment = VerticalAlignment.Stretch;
            PlotAreaCanvas.Margin = new Thickness(0);
            
            Chart.PlotArea.AttachHref(Chart, Chart.PlotArea.BorderElement, Chart.PlotArea.Href, Chart.PlotArea.HrefTarget);
            Chart.PlotArea.DetachToolTip(Chart.PlotArea.BorderElement);
            Chart.PlotArea.AttachToolTip(Chart, Chart.PlotArea, Chart.PlotArea.BorderElement);
            // Chart.AttachEvents2Visual(Chart.PlotArea, PlotAreaCanvas.Children[0] as Border);
            
            // if (!chart._drawingCanvas.Children.Contains(PlotAreaCanvas))
            //    chart._drawingCanvas.Children.Add(PlotAreaCanvas);
            
            //PlotAreaScrollViewer = Chart._plotAreaScrollViewer;

            
        }

        void PlotAreaCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if ((Chart as Chart).PlotDetails.ChartOrientation != ChartOrientationType.NoAxis)
            {
                if (Chart.ZoomingEnabled)
                {
                    if (_zoomStart)
                    {
                        _lastZoomPos = new Point(e.GetPosition(ChartVisualCanvas).X, e.GetPosition(ChartVisualCanvas).Y);

                        if (AxisX.AxisOrientation == Orientation.Horizontal)
                        {
                            if (_lastZoomPos.X < _firstZoomPos.X)
                            {
                                Point temp = _lastZoomPos;
                                _lastZoomPos = _firstZoomPos;
                                _firstZoomPos = temp;
                            }
                        }
                        else
                        {
                            if (_lastZoomPos.Y < _firstZoomPos.Y)
                            {
                                Point temp = _lastZoomPos;
                                _lastZoomPos = _firstZoomPos;
                                _firstZoomPos = temp;
                            }
                        }

                        Chart._zoomRectangle.SetValue(Canvas.LeftProperty, _firstZoomPos.X);
                        Chart._zoomRectangle.SetValue(Canvas.TopProperty, _firstZoomPos.Y);

                        Double minXValue;
                        Double maxXValue;

                        if (AxisX.AxisOrientation == Orientation.Horizontal)
                        {
                            if (_firstZoomPos.X < 0)
                                _firstZoomPos.X = 0;

                            if (_lastZoomPos.X > ChartVisualCanvas.Width)
                                _lastZoomPos.X = ChartVisualCanvas.Width;

                            minXValue = Graphics.PixelPositionToValue(0, ScrollableLength, AxisX.InternalAxisMinimum, AxisX.InternalAxisMaximum, _firstZoomPos.X);
                            maxXValue = Graphics.PixelPositionToValue(0, ScrollableLength, AxisX.InternalAxisMinimum, AxisX.InternalAxisMaximum, _lastZoomPos.X);

                        }
                        else
                        {
                            if (_firstZoomPos.Y < 0)
                                _firstZoomPos.Y = 0;

                            if (_lastZoomPos.Y > ChartVisualCanvas.Height)
                                _lastZoomPos.Y = ChartVisualCanvas.Height;

                            minXValue = Graphics.PixelPositionToValue(ScrollableLength, 0, AxisX.InternalAxisMinimum, AxisX.InternalAxisMaximum, _firstZoomPos.Y);
                            maxXValue = Graphics.PixelPositionToValue(ScrollableLength, 0, AxisX.InternalAxisMinimum, AxisX.InternalAxisMaximum, _lastZoomPos.Y);

                        }

                        Object minValue, maxValue;

                        if (AxisX.IsDateTimeAxis)
                        {
                            minValue = DateTimeHelper.XValueToDateTime(AxisX.MinDate, minXValue, AxisX.InternalIntervalType);
                            maxValue = DateTimeHelper.XValueToDateTime(AxisX.MinDate, maxXValue, AxisX.InternalIntervalType);
                        }
                        else
                        {
                            minValue = minXValue;
                            maxValue = maxXValue;
                        }


                        if (!minValue.Equals(maxValue))
                        {
                            AxisX.Zoom(minValue, maxValue);
                        }

                        //ChartVisualCanvas.ReleaseMouseCapture();
                        Chart._zoomRectangle.Visibility = Visibility.Collapsed;
                        _zoomStart = false;

                    }
                }
            }
        }

        void PlotAreaCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if ((Chart as Chart).PlotDetails.ChartOrientation != ChartOrientationType.NoAxis)
            {
                if (Chart.ZoomingEnabled)
                {
                    Chart._zoomRectangle.Visibility = Visibility.Visible;
                    Chart._zoomRectangle.SetValue(Canvas.ZIndexProperty, 2);

                    _firstZoomPos = new Point(e.GetPosition(ChartVisualCanvas).X, e.GetPosition(ChartVisualCanvas).Y);

                    Chart._zoomRectangle.SetValue(Canvas.LeftProperty, _firstZoomPos.X);
                    Chart._zoomRectangle.SetValue(Canvas.TopProperty, _firstZoomPos.Y);

                    Chart._zoomRectangle.Width = 0;
                    Chart._zoomRectangle.Height = 0;

                    Chart._toolTip.Hide();

                    _zoomStart = true;

                    Chart._zoomRectangle.IsHitTestVisible = false;
                }
            }
        }

        /// <summary>
        /// Enable indicators and position them on mouse move. This is also used to select a region in PlotArea for zooming.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void PlotAreaCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if ((Chart as Chart).PlotDetails.ChartOrientation != ChartOrientationType.NoAxis)
            {
                if (Chart.ZoomingEnabled)
                {
                    if (_zoomStart)
                    {
                        if (_firstZoomPos.X <= e.GetPosition(ChartVisualCanvas).X)
                        {
                            Chart._zoomRectangle.Width = (e.GetPosition(ChartVisualCanvas).X) - _firstZoomPos.X;
                        }
                        else
                        {
                            Chart._zoomRectangle.SetValue(Canvas.LeftProperty, e.GetPosition(ChartVisualCanvas).X);
                            Chart._zoomRectangle.Width = _firstZoomPos.X - (e.GetPosition(ChartVisualCanvas).X);
                        }

                        if (_firstZoomPos.Y <= e.GetPosition(ChartVisualCanvas).Y)
                        {
                            Chart._zoomRectangle.Height = e.GetPosition(ChartVisualCanvas).Y - _firstZoomPos.Y;
                        }
                        else
                        {
                            Chart._zoomRectangle.SetValue(Canvas.TopProperty, e.GetPosition(ChartVisualCanvas).Y);
                            Chart._zoomRectangle.Height = _firstZoomPos.Y - e.GetPosition(ChartVisualCanvas).Y;
                        }

                        System.Diagnostics.Debug.WriteLine("Chart._zoomRectangle.Width : " + Chart._zoomRectangle.Width + ", " + Chart._zoomRectangle.Height);

                        //ChartVisualCanvas.CaptureMouse();
                    }
                }
            }


            if (Chart.IndicatorEnabled)
            {
                Double x1 = e.GetPosition(Chart._toolTipCanvas).X;
                Double x2 = e.GetPosition(Chart._plotAreaGrid).X;

                Double y1 = e.GetPosition(Chart._toolTipCanvas).Y;
                Double y2 = e.GetPosition(Chart._plotAreaGrid).Y;

                Double xOffset = Math.Abs(x1 - x2);
                Double yOffset = Math.Abs(y1 - y2);

                if (Chart.InternalSeries.Count > 0)
                {
                    List<DataPoint> listOfNearestDataPoints = new List<DataPoint>();

                    // Find out the first nearest DataPoint from mouse position
                    foreach (DataSeries ds in Chart.Series)
                    {
                        if (ds.RenderAs != RenderAs.Pie && ds.RenderAs != RenderAs.Doughnut
                             && ds.RenderAs != RenderAs.SectionFunnel && ds.RenderAs != RenderAs.StreamLineFunnel)
                        {
                            if (ds.DataPoints.Count > 0)
                            {
                                DataPoint nearestDataPoint = ds.GetNearestDataPoint(sender, e, ds.DataPoints.ToList());

                                if (nearestDataPoint != null)
                                {
                                    //nearestDataPoint.Parent.SetToolTipProperties(nearestDataPoint);

                                    //if (!String.IsNullOrEmpty(ds.ToolTipElement.Text))
                                    {
                                        //listOfNearestDataPoints.Clear();
                                        listOfNearestDataPoints.Add(nearestDataPoint);
                                        _lastNearestDataPoint = nearestDataPoint;
                                        //break;
                                    }
                                }
                            }
                        }
                    }

                    if (listOfNearestDataPoints != null && listOfNearestDataPoints.Count > 0)
                    {
                        DataPoint dp = listOfNearestDataPoints[0].Parent.GetNearestDataPoint(sender, e, listOfNearestDataPoints);
                        listOfNearestDataPoints.Clear();
                        //listOfNearestDataPoints.Add(dp);
                        _lastNearestDataPoint = dp;
                    }

                    // Now find out the other nearest DataPoints from the last nearest DataPoint found above.
                    // This is done for multiseries Column and Bar charts. If mouse pointer is at the center of 
                    // two DataPoints of a DataSeries then ToolTip should appear for those DataPoints which have 
                    // same XValue.
                    foreach (DataSeries ds in Chart.Series)
                    {
                        if (ds.ToolTipElement != null)
                            ds.ToolTipElement.Hide();

                        if (ds.RenderAs == RenderAs.Pie || ds.RenderAs == RenderAs.Doughnut
                             || ds.RenderAs == RenderAs.SectionFunnel || ds.RenderAs == RenderAs.StreamLineFunnel)
                            continue;

                        if (_lastNearestDataPoint == null)
                            return;

                        var otherNearestDP = (from dp in ds.DataPoints
                                              where dp.InternalXValue == _lastNearestDataPoint.InternalXValue
                                              select dp);

                        if (otherNearestDP != null)
                        {
                            if (otherNearestDP.Count() > 0)
                            {
                                DataPoint dp = otherNearestDP.First().Parent.GetNearestDataPointAlongYPosition(sender, e, otherNearestDP.ToList());

                                //foreach (DataPoint dp in otherNearestDP)
                                {
                                    //if (!listOfNearestDataPoints.Contains(dp))
                                    {
                                        if (dp != null)
                                        {
                                            if (dp.Parent.ToolTipElement != null)
                                            {
                                                dp.Parent.SetToolTipProperties(dp);

                                                if (!String.IsNullOrEmpty(dp.Parent.ToolTipElement.Text))
                                                {
                                                    listOfNearestDataPoints.Add(dp);
                                                }
                                            }
                                        }
                                    }
                                    //else
                                    //{
                                    //    if (dp.Parent.ToolTipElement != null)
                                    //        dp.Parent.ToolTipElement.Show();
                                    //}
                                }
                            }
                        }
                    }

                    System.Diagnostics.Debug.WriteLine("NearestDataPoints Count : " + listOfNearestDataPoints.Count);

                    var nearestDataPointList = (from dataPoint in listOfNearestDataPoints
                                                orderby dataPoint._visualPosition.Y ascending
                                                select dataPoint);

                    if (nearestDataPointList.Count() > 0)
                    {
                        CreateIndicators();

                        _axisIndicatorElement.Visibility = Visibility.Visible;
                        _verticalLineIndicator.Visibility = Visibility.Visible;
                        _callOutPath4AxisIndicator.Visibility = Visibility.Visible;

                        ArrangeToolTips4DataSeries(nearestDataPointList.ToList(), xOffset, yOffset);

                        foreach (DataPoint dp in nearestDataPointList)
                        {
                            Double offset = AxisX.GetScrollBarValueFromOffset(AxisX.CurrentScrollScrollBarOffset);
                            offset = GetScrollingOffsetOfAxis(AxisX, offset);

                            if (AxisX.AxisOrientation == Orientation.Vertical)
                            {
                                if (dp._visualPosition.Y - offset > PlotAreaCanvas.Height
                                    || dp._visualPosition.Y - offset < 0)
                                {
                                    if (dp.Parent.ToolTipElement != null)
                                        dp.Parent.ToolTipElement.Hide();

                                    DisableIndicators();
                                }
                            }
                            else
                            {
                                if (dp._visualPosition.X - offset > PlotAreaCanvas.Width
                                      || dp._visualPosition.X - offset < 0)
                                {
                                    if (dp.Parent.ToolTipElement != null)
                                        dp.Parent.ToolTipElement.Hide();

                                    DisableIndicators();
                                }
                            }
                        }

                        RectangleGeometry clipRectangle = new RectangleGeometry();
                        clipRectangle.Rect = new Rect(0, -PLANK_DEPTH, Chart.ActualWidth, Chart.ActualHeight + PLANK_DEPTH);
                        Chart._toolTipCanvas.Clip = clipRectangle;
                    }
                }
            }
        }

        private void CreateVerticalLineIndicator()
        {
            _verticalLineIndicator = new Line();
            _verticalLineIndicator.Stroke = new SolidColorBrush(Color.FromArgb((Byte)153, (Byte)153, (Byte)153, (Byte)153));
            _verticalLineIndicator.StrokeThickness = 1;
            _verticalLineIndicator.IsHitTestVisible = false;

            Chart._toolTipCanvas.Children.Add(_verticalLineIndicator);
        }

        /// <summary>
        /// Create Indicators for Axis and Plotarea
        /// </summary>
        private void CreateIndicators()
        {
            if (_axisIndicatorElement == null)
            {
                _axisIndicatorElement = new StackPanel();

                _axisCallOutContainer = new Canvas();
                _axisCallOutContainer.Height = 8;
                _axisCallOutContainer.HorizontalAlignment = HorizontalAlignment.Center;
                //_axisIndicatorElement.Visibility = Visibility.Collapsed;

                PathGeometry pathGeometry = new PathGeometry();
                _callOutPath4AxisIndicator = new System.Windows.Shapes.Path();
                _callOutPath4AxisIndicator.Fill = new SolidColorBrush(Colors.LightGray); // new SolidColorBrush(Color.FromArgb((Byte)153, (Byte)153, (Byte)153, (Byte)153));
                //_callOutPath4AxisIndicator.Opacity = 0.8;

                _callOutPath4AxisIndicator.Data = pathGeometry;

                PathFigure pathFigure = new PathFigure();

                pathFigure.StartPoint = new Point(-6, 8);

                pathGeometry.Figures.Add(pathFigure);

                LineSegment lineSegment = new LineSegment();
                lineSegment.Point = new Point(0, 0);

                pathFigure.Segments.Add(lineSegment);

                lineSegment = new LineSegment();
                lineSegment.Point = new Point(6, 8);

                pathFigure.Segments.Add(lineSegment);
               
                _axisCallOutContainer.Children.Add(_callOutPath4AxisIndicator);

                _axisIndicatorBorderElement = new Border();
                _axisIndicatorBorderElement.CornerRadius = new CornerRadius(3);
                _axisIndicatorBorderElement.MinWidth = 25;
                _axisIndicatorBorderElement.HorizontalAlignment = HorizontalAlignment.Center;
                _axisIndicatorBorderElement.Padding = new Thickness(5, 0, 5, 0);
                _axisIndicatorBorderElement.Background = new SolidColorBrush(Colors.LightGray); // new SolidColorBrush(Color.FromArgb((Byte)153, (Byte)153, (Byte)153, (Byte)153));

                _axisIndicatorTextBlock = new TextBlock();
                _axisIndicatorTextBlock.HorizontalAlignment = HorizontalAlignment.Center;

                _axisIndicatorBorderElement.Child = _axisIndicatorTextBlock;
                
                _axisIndicatorElement.Children.Add(_axisCallOutContainer);
                _axisIndicatorElement.Children.Add(_axisIndicatorBorderElement);

                Chart._toolTipCanvas.Children.Add(_axisIndicatorElement);

                CreateVerticalLineIndicator();
            }
        }

        /// <summary>
        /// Set position of Indicators
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="actualPos"></param>
        /// <param name="axisIndicatorText"></param>
        /// <param name="xPlotCanvasOffset"></param>
        /// <param name="yPlotCanvasOffset"></param>
        /// <param name="plotWidth"></param>
        /// <param name="plotHeight"></param>
        private void SetPositon4Indicators(Axis axis, Double actualPos, String axisIndicatorText, Double xPlotCanvasOffset, Double yPlotCanvasOffset, Double plotWidth, Double plotHeight)
        {
            // Set position of indicator line
            _axisIndicatorTextBlock.Text = axisIndicatorText;

            Double leftOfIndicator = 0;

            Double offset = axis.GetScrollBarValueFromOffset(axis.CurrentScrollScrollBarOffset);
            offset = GetScrollingOffsetOfAxis(axis, offset);

            if (axis.AxisOrientation == Orientation.Horizontal)
            {
                _axisIndicatorElement.Children.Clear();
                _axisIndicatorElement.Orientation = Orientation.Vertical;
                _axisIndicatorBorderElement.MinWidth = 25;
                _axisIndicatorElement.Children.Add(_axisCallOutContainer);
                _axisIndicatorElement.Children.Add(_axisIndicatorBorderElement);
                _axisCallOutContainer.Height = 8;
                _axisCallOutContainer.Width = Double.NaN;
                
                if(Chart._toolTipCanvas.Children.Contains(_verticalLineIndicator))
                {
                    Chart._toolTipCanvas.Children.Remove(_verticalLineIndicator);
                    CreateVerticalLineIndicator();
                }

                if (_axisIndicatorBorderElement.RenderTransform.GetType() == typeof(RotateTransform))
                {
                    (_axisIndicatorBorderElement.RenderTransform as RotateTransform).Angle = 0;
                    _axisIndicatorBorderElement.RenderTransformOrigin = new Point(0, 0);
                }

                ((_callOutPath4AxisIndicator.Data as PathGeometry).Figures[0] as PathFigure).StartPoint = new Point(-6, 8);
                (((_callOutPath4AxisIndicator.Data as PathGeometry).Figures[0] as PathFigure).Segments[0] as LineSegment).Point = new Point(0, 0);
                (((_callOutPath4AxisIndicator.Data as PathGeometry).Figures[0] as PathFigure).Segments[1] as LineSegment).Point = new Point(6, 8);

                _verticalLineIndicator.SetValue(Canvas.LeftProperty, actualPos - xPlotCanvasOffset - offset);

                _verticalLineIndicator.X1 = xPlotCanvasOffset;
                _verticalLineIndicator.Y1 = yPlotCanvasOffset;

                _verticalLineIndicator.X2 = xPlotCanvasOffset;
                _verticalLineIndicator.Y2 = plotHeight + yPlotCanvasOffset;

                //  Set position of indicator over Axis
                leftOfIndicator = actualPos - offset - _axisIndicatorElement.ActualWidth / 2;
                Double topOfIndicator = plotHeight + yPlotCanvasOffset;
                topOfIndicator += (axis.ScrollBarElement.Visibility == Visibility.Visible) ? axis.ScrollBarElement.Height : 0;

                _axisIndicatorElement.SetValue(Canvas.LeftProperty, leftOfIndicator);
                _axisIndicatorElement.SetValue(Canvas.TopProperty, topOfIndicator);
            }
            else
            {
                if (_axisIndicatorBorderElement.ActualWidth + _axisCallOutContainer.Width > axis.Width - Chart.Padding.Left)
                {
                    _axisIndicatorElement.Children.Clear();
                    _axisIndicatorElement.Orientation = Orientation.Vertical;
                    _axisIndicatorBorderElement.MinWidth = 25;
                    _axisIndicatorBorderElement.MinHeight = 0;
                    _axisIndicatorElement.Children.Add(_axisIndicatorBorderElement);
                    _axisIndicatorElement.Children.Add(_axisCallOutContainer);
                    _axisCallOutContainer.Width = 8;

                    if (Chart._toolTipCanvas.Children.Contains(_verticalLineIndicator))
                    {
                        Chart._toolTipCanvas.Children.Remove(_verticalLineIndicator);
                        CreateVerticalLineIndicator();
                    }

                    RotateTransform transform = new RotateTransform();
                    transform.Angle = 90;
                    _axisIndicatorBorderElement.RenderTransformOrigin = new Point(0.5, 0.5);
                    _axisIndicatorBorderElement.RenderTransform = transform;

                    ((_callOutPath4AxisIndicator.Data as PathGeometry).Figures[0] as PathFigure).StartPoint = new Point(10, -10);
                    (((_callOutPath4AxisIndicator.Data as PathGeometry).Figures[0] as PathFigure).Segments[0] as LineSegment).Point = new Point(20, -5);
                    (((_callOutPath4AxisIndicator.Data as PathGeometry).Figures[0] as PathFigure).Segments[1] as LineSegment).Point = new Point(10, 0);

                    leftOfIndicator -= _axisIndicatorElement.ActualWidth / 2 + (_axisIndicatorElement.ActualHeight - _axisCallOutContainer.Width);
                }
                else
                {
                    _axisIndicatorElement.Orientation = Orientation.Horizontal;

                    _axisIndicatorElement.Children.Clear();
                    _axisIndicatorBorderElement.MinWidth = 0;
                    _axisIndicatorBorderElement.MinHeight = 0;
                    _axisCallOutContainer.Width = 8;

                    if (_axisIndicatorBorderElement.RenderTransform.GetType() == typeof(RotateTransform))
                    {
                        (_axisIndicatorBorderElement.RenderTransform as RotateTransform).Angle = 0;
                        _axisIndicatorBorderElement.RenderTransformOrigin = new Point(0, 0);
                    }

                    if (Chart._toolTipCanvas.Children.Contains(_verticalLineIndicator))
                    {
                        Chart._toolTipCanvas.Children.Remove(_verticalLineIndicator);
                        CreateVerticalLineIndicator();
                    }

                    _axisIndicatorElement.Children.Add(_axisIndicatorBorderElement);
                    _axisIndicatorElement.Children.Add(_axisCallOutContainer);

                    ((_callOutPath4AxisIndicator.Data as PathGeometry).Figures[0] as PathFigure).StartPoint = new Point(0, 0);
                    (((_callOutPath4AxisIndicator.Data as PathGeometry).Figures[0] as PathFigure).Segments[0] as LineSegment).Point = new Point(8, 4);
                    (((_callOutPath4AxisIndicator.Data as PathGeometry).Figures[0] as PathFigure).Segments[1] as LineSegment).Point = new Point(0, 8);

                    leftOfIndicator -= _axisIndicatorElement.ActualWidth;
                }

                _verticalLineIndicator.SetValue(Canvas.TopProperty, actualPos - yPlotCanvasOffset - offset);

                _verticalLineIndicator.X1 = xPlotCanvasOffset;
                _verticalLineIndicator.Y1 = yPlotCanvasOffset;

                _verticalLineIndicator.X2 = xPlotCanvasOffset + plotWidth;
                _verticalLineIndicator.Y2 = yPlotCanvasOffset;

                //  Set position of indicator over Axis
                leftOfIndicator += xPlotCanvasOffset;
                Double topOfIndicator = actualPos - offset - _axisIndicatorElement.ActualHeight / 2;
                leftOfIndicator -= (axis.ScrollBarElement.Visibility == Visibility.Visible) ? axis.ScrollBarElement.Width : 0;

                _axisIndicatorElement.SetValue(Canvas.LeftProperty, leftOfIndicator);
                _axisIndicatorElement.SetValue(Canvas.TopProperty, topOfIndicator);
            }
    

            // Graphics.DrawPointAt(new Point(leftOfIndicator, topOfIndicator), Chart._toolTipCanvas, Colors.Purple);
        }

        /// <summary>
        /// Disable all indicators on mouse leave of PlotArea
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void PlotAreaCanvas_MouseLeave(object sender, MouseEventArgs e)
        {
            foreach (DataSeries ds in Chart.Series)
            {
                if(ds.ToolTipElement != null)
                    ds.ToolTipElement.Hide();
            }

            DisableIndicators();

            //if(Chart._zoomRectangle != null)
            //    Chart._zoomRectangle.Visibility = Visibility.Collapsed;

            //_zoomStart = false;
        }

        /// <summary>
        /// Calculate max available size for legends
        /// </summary>
        /// <param name="boundingRec">Bounding rectangle size</param>
        /// <returns>max available size for legends</returns>
        private Size CalculateLegendMaxSize(Size boundingRec)
        {
            Size elementSize;

            foreach (Title title in Chart.Titles)
            {
                if (title.DockInsidePlotArea || !(Boolean)title.Enabled || title.Visual == null)
                    continue;

                elementSize = Graphics.CalculateVisualSize(title.Visual);

                if (title.InternalVerticalAlignment == VerticalAlignment.Bottom || title.InternalVerticalAlignment == VerticalAlignment.Top)
                {
                    boundingRec.Height -= elementSize.Height;
                    if (title.InternalVerticalAlignment == VerticalAlignment.Bottom)
                        Chart._bottomOuterTitlePanel.Height += elementSize.Height;
                    else
                        Chart._topOuterTitlePanel.Height += elementSize.Height;
                }
                else if (title.InternalVerticalAlignment == VerticalAlignment.Center || title.InternalVerticalAlignment == VerticalAlignment.Stretch)
                {
                    if (title.InternalHorizontalAlignment == HorizontalAlignment.Left || title.InternalHorizontalAlignment == HorizontalAlignment.Right)
                    {
                        boundingRec.Width -= elementSize.Width;
                        if (title.InternalHorizontalAlignment == HorizontalAlignment.Left)
                            Chart._leftOuterTitlePanel.Width += elementSize.Width;
                        else
                            Chart._rightOuterTitlePanel.Width += elementSize.Width;
                    }
                }
            }

            return boundingRec;
        }

        /// <summary>
        /// Populate InternalAxesX list from AxesX collection
        /// InternalAxesX is used while rendering the chart, AxesX collection is not used.
        /// Each render must work with a single set of non variable data set otherwise chart won't render properly.
        /// </summary>
        private void PopulateInternalAxesXList()
        {
            if (Chart.InternalAxesX != null)
                Chart.InternalAxesX.Clear();

            Chart.InternalAxesX = Chart.AxesX.ToList();
        }

        /// <summary>
        /// Populate InternalAxesY list from AxesY collection
        /// InternalAxesY is used while rendering the chart, AxesY collection is not used.
        /// Each render must work with a single set of non variable data set otherwise chart won't render properly.
        /// </summary>
        private void PopulateInternalAxesYList()
        {
            if (Chart.InternalAxesY != null)
                Chart.InternalAxesY.Clear();

            Chart.InternalAxesY = Chart.AxesY.ToList();
        }

        private void ResetStoryboards4InternalDataPointsList()
        {
            foreach (DataSeries ds in Chart.InternalSeries)
            {
                foreach (DataPoint dp in ds.InternalDataPoints)
                {
                    if (dp.Storyboard != null)
                    {
                        dp.Storyboard.Stop();
                        dp.Storyboard.Children.Clear();

#if WPF
                        dp.Storyboard.Remove(Chart._rootElement);
                        
#endif
                        dp.Storyboard = null;
                    }
                }

                ds.InternalDataPoints.Clear();
            }
        }

        /// <summary>
        /// Populate InternalSeries list from Series collection
        /// InternalSeries is used while rendering the chart, Series collection is not used.
        /// Each render must work with a single set of non variable data set otherwise chart won't render properly.
        /// </summary>
        private void PopulateInternalSeriesList()
        {
            if (Chart.InternalSeries != null)
            {
                if((Boolean)Chart.AnimatedUpdate)
                    ResetStoryboards4InternalDataPointsList();


                Chart.InternalSeries.Clear();
            }

            if (Chart.Series.Count == 0)
            {
                Chart.InternalSeries = new List<DataSeries>();

                if (Chart.IsInDesignMode)
                {
                    AddDefaultDataSeriesInDesignMode();
                    SetDataPointColorFromColorSet(Chart.InternalSeries);
                }
                else
                    SetBlankSeries();
            }
            else
                Chart.InternalSeries = (from ds in Chart.Series orderby ds.ZIndex select ds).ToList();

            foreach (DataSeries ds in Chart.InternalSeries)
            {
                if (_isFirstTimeRender)
                {
                    ds.InternalDataPoints = new List<DataPoint>();
                    foreach (DataPoint dp in ds.DataPoints)
                    {
                        ds.InternalDataPoints.Add(dp);
                        dp._oldYValue = dp.InternalYValue;
                    }
                }
                else
                {
                    ds.InternalDataPoints = ds.DataPoints.ToList();
                }
            }
        }

        /// <summary>
        /// In design mode if Series list is empty, add a default DataSeries to InternalSeries list
        /// </summary>
        private void AddDefaultDataSeriesInDesignMode()
        {
            DataSeries ds = new DataSeries();
            ds.RenderAs = RenderAs.Column;
            ds.LightingEnabled = true;
            ds.ShadowEnabled = true;
            ds.Chart = Chart;

            DataPoint dp = new DataPoint();
            dp.InternalXValue = 1;
            dp.YValue = 70;
            dp.AxisXLabel = "Wall-Mart";
            dp.Chart = Chart;
            ds.DataPoints.Add(dp);

            dp = new DataPoint();
            dp.InternalXValue = 2;
            dp.YValue = 40;
            dp.AxisXLabel = "Exxon Mobil";
            dp.Chart = Chart;
            ds.DataPoints.Add(dp);

            dp = new DataPoint();
            dp.InternalXValue = 3;
            dp.YValue = 60;
            dp.AxisXLabel = "Shell";
            dp.Chart = Chart;
            ds.DataPoints.Add(dp);

            dp = new DataPoint();
            dp.InternalXValue = 4;
            dp.YValue = 27;
            dp.AxisXLabel = "BP";
            dp.Chart = Chart;
            ds.DataPoints.Add(dp);

            dp = new DataPoint();
            dp.InternalXValue = 5;
            dp.YValue = 54;
            dp.AxisXLabel = "General Motors";
            dp.Chart = Chart;
            ds.DataPoints.Add(dp);

            Chart.InternalSeries.Add(ds);
        }

        /// <summary>
        /// Calculate 3d plank paramaters (depth, thickness and offset)
        /// </summary>
        private void CalculatePlankParameters()
        {
            if (Chart.View3D)
            {
                if (PlotDetails.ChartOrientation == ChartOrientationType.Vertical)
                {
                    Double horizontalDepthFactor = 0.04;
                    Double horizontalThicknessFactor = 0.03;

                    PLANK_DEPTH = (Chart.ActualHeight > Chart.ActualWidth ? Chart.ActualWidth : Chart.ActualHeight) * (horizontalDepthFactor * (PlotDetails.Layer3DCount == 0 ? 1 : PlotDetails.Layer3DCount));
                    PLANK_THICKNESS = (Chart.ActualHeight > Chart.ActualWidth ? Chart.ActualWidth : Chart.ActualHeight) * (horizontalThicknessFactor);
                }
                else
                {
                    Double verticalDepthFactor = 0.015;
                    Double verticalThicknessFactor = 0.025;

                    PLANK_DEPTH = (Chart.ActualHeight > Chart.ActualWidth ? Chart.ActualWidth : Chart.ActualHeight) * (verticalDepthFactor * PlotDetails.Layer3DCount);
                    PLANK_THICKNESS = (Chart.ActualHeight > Chart.ActualWidth ? Chart.ActualWidth : Chart.ActualHeight) * (verticalThicknessFactor);
                }

                PLANK_DEPTH = Double.IsNaN(PLANK_DEPTH) ? 12 : PLANK_DEPTH;
                PLANK_THICKNESS = Double.IsNaN(PLANK_THICKNESS) ? 5 : PLANK_THICKNESS;

                PLANK_OFFSET = PLANK_DEPTH + PLANK_THICKNESS;
            }
            else
            {
                PLANK_DEPTH = 0;
                PLANK_THICKNESS = 0;
                PLANK_OFFSET = 0;
            }
        }

        /// <summary>
        /// Set scrollbar SmallChange and LargeChange value property and attach events to scroll event of axis for the first time render
        /// </summary>
        /// <param name="axis"></param>
        private void SetScrollBarChanges(Axis axis)
        {
            if (ScrollableLength < 1000)
            {
                axis.ScrollBarElement.SmallChange = 10;
                axis.ScrollBarElement.LargeChange = 50;
            }
            else
            {
                axis.ScrollBarElement.SmallChange = 20;
                axis.ScrollBarElement.LargeChange = 80;
            }

            // if (_isFirstTimeRender)
            {   
                //axis.Scroll -= AxesXScrollBarElement_Scroll;
                //axis.Scroll += new System.Windows.Controls.Primitives.ScrollEventHandler(AxesXScrollBarElement_Scroll);
                //axis.SetScrollBarValueFromOffset(axis.ScrollBarOffset);
            }
        }

        /// <summary>
        /// Draw axisX
        /// </summary>
        /// <param name="availableSize">Available size</param> 
        /// <param name="forceRender">If forceRender=false and axis visual is present of same size as requested, axis wont render again.</param>
        /// <returns>
        /// For vertical chart: total height reduced by the axis
        /// For horizontal chart: total width reduced by the axis
        /// </returns>
        private Double DrawAxesX(Size availableSize, Boolean forceRender)
        {
            Double totalHeightReduced = 0;
            
            Chart chart = Chart as Chart;

            if (AxisX != null && PlotDetails.ChartOrientation == ChartOrientationType.Vertical)
            {
                if (!forceRender && AxisX.Width == availableSize.Width && !(AxisX.Width >= AxisX.ScrollableSize || AxisX.ScrollBarElement.Maximum == 0))
                {
                    return Double.NaN;
                }

                // Clear axis panels
                Chart._bottomAxisPanel.Children.Clear();

                AxisX.Width = availableSize.Width;
                AxisX.ScrollBarElement.Width = AxisX.Width;

                if (PlotAreaScrollViewer != null)
                {   
                    AxisX.ScrollableSize = ScrollableLength;

                    if (chart.ZoomingEnabled)
                    {
                        AxisX.ScrollBarElement.IsStretchable = chart.ZoomingEnabled;
                        AxisX.ScrollBarElement.Maximum = ZOOMING_MAX_VAL;

                    }
                    else
                    {
#if SL
                        AxisX.ScrollBarElement.Maximum = ScrollableLength - PlotAreaScrollViewer.ViewportWidth;
                        AxisX.ScrollBarElement.ViewportSize = PlotAreaScrollViewer.ViewportWidth;
#else                   
                        AxisX.ScrollBarElement.Maximum = ScrollableLength - PlotAreaScrollViewer.ActualWidth;
                        AxisX.ScrollBarElement.ViewportSize = PlotAreaScrollViewer.ActualWidth;
#endif
                    }
                }
                
                AxisX.CreateVisualObject(Chart);

                SetScrollBarChanges(AxisX);

                AxisX.ScrollBarElement.IsStretchable = chart.ZoomingEnabled;
                AxisX.ScrollBarElement.Scroll -= AxesXScrollBarElement_Scroll;
                AxisX.ScrollBarElement.Scroll += new System.Windows.Controls.Primitives.ScrollEventHandler(AxesXScrollBarElement_Scroll);
               
                AxisX.ZoomingScaleChanged -= new EventHandler(ScrollBarElement_ScaleChanged);

                if (chart.ZoomingEnabled)
                {
                    AxisX.ZoomingScaleChanged += new EventHandler(ScrollBarElement_ScaleChanged);
                    AxisX.ScrollBarElement.Visibility = Visibility.Visible;

                    zoomCount++;
                }
                else
                {
                    if (AxisX.Width >= AxisX.ScrollableSize || AxisX.ScrollBarElement.Maximum == 0)
                        AxisX.ScrollBarElement.Visibility = Visibility.Collapsed;
                    else
                    {
                        AxisX.ScrollBarElement.Visibility = Visibility.Visible;
                    }

                    zoomCount = 0;
                }

                if (!_isFirstTimeRender)
                    AxisX.ScrollBarElement.UpdateTrackLayout(AxisX.ScrollBarElement.GetTrackLength());

                Chart._bottomAxisPanel.Children.Add(AxisX.Visual);

                Size size = Graphics.CalculateVisualSize(Chart._bottomAxisContainer);
                totalHeightReduced += size.Height;

                AxisX.Height = size.Height;

                /*
                 * Double value = AxisX.GetScrollBarValueFromOffset(AxisX.CurrentScrollScrollBarOffset);
                Double scrollViewerOffset = scrollViewerContent.Width - PlotAreaScrollViewer.ViewportWidth;

                scrollViewerOffset = (Chart as Chart).ZoomingEnabled ? value * (scrollViewerOffset / ZOOMING_MAX_VAL) : value;
                Chart._plotAreaScrollViewer.ScrollToHorizontalOffset(scrollViewerOffset);
                 */
                if (AxisX.ScrollViewerElement.Children.Count > 0)
                {
                    FrameworkElement scrollViewerContent = (AxisX.ScrollViewerElement.Children[0] as FrameworkElement);
                    if (scrollViewerContent != null)
                    {
                        Double offset = AxisX.GetScrollBarValueFromOffset(AxisX.CurrentScrollScrollBarOffset);
                        if (!Double.IsNaN(offset))
                        {
                            offset = GetScrollingOffsetOfAxis(AxisX, offset);
                            //if (!Double.IsNaN(offset))
                            {
                                Chart._plotAreaScrollViewer.ScrollToHorizontalOffset(offset);
                                scrollViewerContent.SetValue(Canvas.LeftProperty, -1 * offset);
                            }
                        }
                    }
                }

                // if (AxisX.ScrollViewerElement.Children.Count > 0)
                //      (AxisX.ScrollViewerElement.Children[0] as FrameworkElement).SetValue(Canvas.LeftProperty, -1 * AxisX.GetScrollBarValueFromOffset(AxisX.CurrentScrollScrollBarOffset));
            }
            else
                Chart._bottomAxisScrollBar.Visibility = Visibility.Collapsed;

            if (AxisX2 != null && PlotDetails.ChartOrientation == ChartOrientationType.Vertical)
            {
                if (AxisX2.Width == availableSize.Width && !(AxisX2.Width >= AxisX2.ScrollableSize || AxisX2.ScrollBarElement.Maximum == 0))
                {
                    return Double.NaN;
                }

                // Clear axis panels
                Chart._topAxisPanel.Children.Clear();

                AxisX2.Width = availableSize.Width;
                AxisX2.ScrollBarElement.Width = AxisX2.Width;

                if (PlotAreaScrollViewer != null)
                {
                    AxisX2.ScrollableSize = ScrollableLength;

                    if (chart.ZoomingEnabled)
                    {
                        AxisX2.ScrollBarElement.Maximum = ZOOMING_MAX_VAL;
                    }
                    else
                    {
#if SL
                        AxisX2.ScrollBarElement.Maximum = ScrollableLength - PlotAreaScrollViewer.ViewportWidth;
                        AxisX2.ScrollBarElement.ViewportSize = PlotAreaScrollViewer.ViewportWidth;
#else               
                    AxisX2.ScrollBarElement.Maximum = ScrollableLength - PlotAreaScrollViewer.ActualWidth;
                    AxisX2.ScrollBarElement.ViewportSize = PlotAreaScrollViewer.ActualWidth;
#endif
                    }
                }

                AxisX2.CreateVisualObject(Chart);
                SetScrollBarChanges(AxisX2);

                AxisX2.ScrollBarElement.Scroll -= AxesX2ScrollBarElement_Scroll;
                AxisX2.ScrollBarElement.Scroll += new System.Windows.Controls.Primitives.ScrollEventHandler(AxesX2ScrollBarElement_Scroll);

                AxisX2.ScrollBarElement.IsStretchable = chart.ZoomingEnabled;
                AxisX2.ZoomingScaleChanged -= new EventHandler(ScrollBarElement_ScaleChanged);

                if (chart.ZoomingEnabled)
                {
                    AxisX2.ZoomingScaleChanged += new EventHandler(ScrollBarElement_ScaleChanged);
                }

                if (AxisX2.Width >= AxisX2.ScrollableSize || AxisX2.ScrollBarElement.Maximum == 0)
                    AxisX2.ScrollBarElement.Visibility = Visibility.Collapsed;
                else
                {
                    AxisX2.ScrollBarElement.Visibility = Visibility.Visible;
                }

                Chart._topAxisPanel.Children.Add(AxisX2.Visual);

                Size size = Graphics.CalculateVisualSize(Chart._topAxisContainer);
                totalHeightReduced += size.Height;

                AxisX2.Height = size.Height;

                if (AxisX2.ScrollViewerElement.Children.Count > 0)
                    (AxisX2.ScrollViewerElement.Children[0] as FrameworkElement).SetValue(Canvas.LeftProperty, -AxisX2.GetScrollBarValueFromOffset(AxisX2.CurrentScrollScrollBarOffset));
            }
            else
                Chart._topAxisScrollBar.Visibility = Visibility.Collapsed;

            if (AxisY != null && PlotDetails.ChartOrientation == ChartOrientationType.Horizontal)
            {
                // Clear axis panels
                Chart._bottomAxisPanel.Children.Clear();

                AxisY.Width = availableSize.Width;
                AxisY.ScrollableSize = availableSize.Width;
                AxisY.CreateVisualObject(Chart);

                AxisY.ScrollBarElement.Visibility = Visibility.Collapsed;

                Chart._bottomAxisPanel.Children.Add(AxisY.Visual);

                Size size = Graphics.CalculateVisualSize(Chart._bottomAxisContainer);
                totalHeightReduced += size.Height;

                AxisY.Height = size.Height;
            }
            else if(PlotDetails.ChartOrientation == ChartOrientationType.Vertical)
                Chart._leftAxisScrollBar.Visibility = Visibility.Collapsed;


            if (AxisY2 != null && PlotDetails.ChartOrientation == ChartOrientationType.Horizontal)
            {   
                // Clear axis panels
                Chart._topAxisPanel.Children.Clear();

                AxisY2.Width = availableSize.Width;
                AxisY2.ScrollableSize = availableSize.Width;
                AxisY2.CreateVisualObject(Chart);

                AxisY2.ScrollBarElement.Visibility = Visibility.Collapsed;

                Chart._topAxisPanel.Children.Add(AxisY2.Visual);

                Size size = Graphics.CalculateVisualSize(Chart._topAxisContainer);
                totalHeightReduced += size.Height;

                AxisY2.Height = size.Height;
            }
            else
                Chart._rightAxisScrollBar.Visibility = Visibility.Collapsed;

            return totalHeightReduced;
        }

        void ScrollBarElement_ScaleChanged(object sender, EventArgs e)
        {   
            Axis axis = sender as Axis;
            Chart chart = Chart as Chart;

            axis._internalZoomingScale = axis._internalMinimumZoomingScale + (1 - axis._internalMinimumZoomingScale) * (1 - axis.ScrollBarElement.Scale);
            
            OnScrollBarScaleChanged(chart);
        }

        internal void OnScrollBarScaleChanged(Chart chart)
        {
            // chart.Dispatcher.BeginInvoke(new Action(chart.ChartArea.DrawChart));
            //chart.Series[0].UpdateVisual(VcProperties.ScrollBarScale, null);
            _isDragging = true;

            if (chart.Series.Count > 0)
            {
                chart.Dispatcher.BeginInvoke(new Action<VcProperties, object>(chart.Series[0].UpdateVisual), new object[] { VcProperties.ScrollBarScale, null });
                chart.Dispatcher.BeginInvoke(new Action(ActivateDraggingLock));
            }
        }

        private void ActivateDraggingLock()
        {
            _isDragging = false;
        }

        /// <summary>
        /// Draw AxisY
        /// </summary>
        /// <param name="availableSize">Available size</param>
        /// <param name="forceRender">If forceRender=false and axis visual is present of same size as requested, axis wont render again.</param>
        /// <returns>
        /// For vertical chart: total width reduced by the axis
        /// For horizontal chart: total height reduced by the axis
        /// </returns>
        private Double DrawAxesY(Size availableSize, Boolean forceRender)
        {
            Double totalWidthReduced = 0;
            Chart chart = Chart as Chart;
            if (AxisX != null && PlotDetails.ChartOrientation == ChartOrientationType.Horizontal)
            {
                if (!forceRender && AxisX.Height == availableSize.Height && !(AxisX.Height >= AxisX.ScrollableSize || AxisX.ScrollBarElement.Maximum == 0))
                {
                    return Double.NaN;
                }

                // Clear axis panels
                Chart._leftAxisPanel.Children.Clear();

                AxisX.Height = availableSize.Height;
                AxisX.ScrollBarElement.Height = AxisX.Height;

                if (PlotAreaScrollViewer != null)
                {
                    AxisX.ScrollableSize = ScrollableLength;

                    if ((Chart as Chart).ZoomingEnabled)
                    {
                        AxisX.ScrollBarElement.IsStretchable = chart.ZoomingEnabled;
                        AxisX.ScrollBarElement.Height = availableSize.Height;
                        AxisX.ScrollBarElement.Maximum = ZOOMING_MAX_VAL;
                    }
                    else
                    {
#if SL
                        AxisX.ScrollBarElement.Maximum = ScrollableLength - PlotAreaScrollViewer.ViewportHeight;
                        AxisX.ScrollBarElement.ViewportSize = PlotAreaScrollViewer.ViewportHeight;
#else
                        AxisX.ScrollBarElement.Maximum = ScrollableLength - PlotAreaScrollViewer.ActualHeight;
                        AxisX.ScrollBarElement.ViewportSize = PlotAreaScrollViewer.ActualHeight;
#endif
                    }
                }

                AxisX.CreateVisualObject(Chart);
                SetScrollBarChanges(AxisX);

                AxisX.ScrollBarElement.Height = availableSize.Height;
                AxisX.ScrollViewerElement.Height = availableSize.Height;
                AxisX.ScrollBarElement.Scroll -= AxesXScrollBarElement_Scroll;
                AxisX.ScrollBarElement.Scroll += new System.Windows.Controls.Primitives.ScrollEventHandler(AxesXScrollBarElement_Scroll);

                AxisX.ScrollBarElement.IsStretchable = chart.ZoomingEnabled;
                AxisX.ZoomingScaleChanged -= new EventHandler(ScrollBarElement_ScaleChanged);

                if (chart.ZoomingEnabled)
                {
                    AxisX.ZoomingScaleChanged += new EventHandler(ScrollBarElement_ScaleChanged);
                    AxisX.ScrollBarElement.Visibility = Visibility.Visible;

                    zoomCount++;
                }
                else
                {
                    if (AxisX.Height >= AxisX.ScrollableSize || AxisX.ScrollBarElement.Maximum == 0)
                        AxisX.ScrollBarElement.Visibility = Visibility.Collapsed;
                    else
                    {
                        AxisX.ScrollBarElement.Visibility = Visibility.Visible;
                    }

                    zoomCount = 0;
                }

                if (!_isFirstTimeRender)
                    AxisX.ScrollBarElement.UpdateTrackLayout(AxisX.ScrollBarElement.GetTrackLength());

                Chart._leftAxisPanel.Children.Add(AxisX.Visual);

                Size size = Graphics.CalculateVisualSize(Chart._leftAxisContainer);
                totalWidthReduced += size.Width;

                AxisX.Width = size.Width;

                //----------------------
                if (AxisX.ScrollViewerElement.Children.Count > 0)
                {
                    FrameworkElement scrollViewerContent = (AxisX.ScrollViewerElement.Children[0] as FrameworkElement);

                    if (scrollViewerContent != null)
                    {
                        Double offset = AxisX.GetScrollBarValueFromOffset(AxisX.CurrentScrollScrollBarOffset);
                        offset = GetScrollingOffsetOfAxis(AxisX, offset);
                        Chart._plotAreaScrollViewer.ScrollToVerticalOffset(offset);
                        scrollViewerContent.SetValue(Canvas.TopProperty, -1 * offset);
                    }
                }

                //if (AxisX.ScrollViewerElement.Children.Count > 0)
                //    (AxisX.ScrollViewerElement.Children[0] as FrameworkElement).SetValue(Canvas.TopProperty, -AxisX.GetScrollBarValueFromOffset(AxisX.CurrentScrollScrollBarOffset));

            }

            if (AxisX2 != null && PlotDetails.ChartOrientation == ChartOrientationType.Horizontal)
            {   
                if (!forceRender && AxisX2.Height == availableSize.Height && !(AxisX2.Height >= AxisX2.ScrollableSize || AxisX2.ScrollBarElement.Maximum == 0))
                {
                    return Double.NaN;
                }

                // Clear axis panels
                Chart._rightAxisPanel.Children.Clear();

                AxisX2.Height = availableSize.Height;
                AxisX2.ScrollBarElement.Height = AxisX2.Height;

                if (PlotAreaScrollViewer != null)
                {
                    AxisX2.ScrollableSize = ScrollableLength;

                    if ((Chart as Chart).ZoomingEnabled)
                    {
                        AxisX.ScrollBarElement.Maximum = ZOOMING_MAX_VAL;
                    }
                    else
                    {
#if SL
                        AxisX2.ScrollBarElement.Maximum = ScrollableLength - PlotAreaScrollViewer.ViewportHeight;
                        AxisX2.ScrollBarElement.ViewportSize = PlotAreaScrollViewer.ViewportHeight;
#else
                        AxisX2.ScrollBarElement.Maximum = ScrollableLength - PlotAreaScrollViewer.ActualHeight;
                        AxisX2.ScrollBarElement.ViewportSize = PlotAreaScrollViewer.ActualHeight;
#endif
                    }
                }

                AxisX2.CreateVisualObject(Chart);
                SetScrollBarChanges(AxisX2);

                AxisX2.ScrollBarElement.Scroll -= AxesX2ScrollBarElement_Scroll;
                AxisX2.ScrollBarElement.Scroll += new System.Windows.Controls.Primitives.ScrollEventHandler(AxesX2ScrollBarElement_Scroll);

                AxisX2.ScrollBarElement.IsStretchable = chart.ZoomingEnabled;
                AxisX2.ZoomingScaleChanged -= new EventHandler(ScrollBarElement_ScaleChanged);

                if (chart.ZoomingEnabled)
                {
                    AxisX2.ZoomingScaleChanged += new EventHandler(ScrollBarElement_ScaleChanged);
                }

                if (AxisX2.Height >= AxisX2.ScrollableSize || AxisX2.ScrollBarElement.Maximum == 0)
                    AxisX2.ScrollBarElement.Visibility = Visibility.Collapsed;
                else
                {   
                    AxisX2.ScrollBarElement.Visibility = Visibility.Visible;
                }   

                Chart._rightAxisPanel.Children.Add(AxisX2.Visual);

                Size size = Graphics.CalculateVisualSize(Chart._rightAxisContainer);
                totalWidthReduced += size.Width;

                AxisX2.Width = size.Width;

                if (AxisX2.ScrollViewerElement.Children.Count > 0)
                    (AxisX2.ScrollViewerElement.Children[0] as FrameworkElement).SetValue(Canvas.TopProperty, -AxisX2.GetScrollBarValueFromOffset(AxisX2.CurrentScrollScrollBarOffset));

            }

            if (AxisY != null && PlotDetails.ChartOrientation == ChartOrientationType.Vertical)
            {
                // Clear axis panels
                Chart._leftAxisPanel.Children.Clear();

                AxisY.Height = availableSize.Height;
                AxisY.ScrollableSize = availableSize.Height;
                AxisY.CreateVisualObject(Chart);

                AxisY.ScrollBarElement.Visibility = Visibility.Collapsed;

                Chart._leftAxisPanel.Children.Add(AxisY.Visual);

                Size size = Graphics.CalculateVisualSize(Chart._leftAxisContainer);
                totalWidthReduced += size.Width;

                AxisY.Width = size.Width;

                //if (AxisX != null)
                //{
                //    AxisX.Width = availableSize.Width - totalWidthReduced;
                //    AxisX.ScrollBarElement.Width = availableSize.Width - totalWidthReduced;
                //}

                Double left = Chart.Padding.Left + Chart._leftOuterTitlePanel.ActualWidth + Chart._leftOuterLegendPanel.ActualWidth;
                AxisY.SetValue(Canvas.LeftProperty, (Double)left);
            }
            if (AxisY2 != null && PlotDetails.ChartOrientation == ChartOrientationType.Vertical)
            {
                // Clear axis panels
                Chart._rightAxisPanel.Children.Clear();

                AxisY2.Height = availableSize.Height;
                AxisY2.ScrollableSize = availableSize.Height;
                AxisY2.CreateVisualObject(Chart);

                AxisY2.ScrollBarElement.Visibility = Visibility.Collapsed;

                Chart._rightAxisPanel.Children.Add(AxisY2.Visual);

                Size size = Graphics.CalculateVisualSize(Chart._rightAxisContainer);
                totalWidthReduced += size.Width;

                AxisY2.Width = size.Width;
            }

            return totalWidthReduced;
        }

        /// <summary>
        /// Reset all storyboards associated with dataseries to null 
        /// </summary>
        private void ResetStoryboards()
        {
            if (Chart._internalAnimationEnabled)
            {
                foreach (DataSeries ds in Chart.InternalSeries)
                {
#if WPF
                    if (ds.Storyboard != null && ds.Storyboard.GetValue(System.Windows.Media.Animation.Storyboard.TargetProperty) != null)
                        ds.Storyboard.Stop();
#else
                    if (ds.Storyboard != null)
                        ds.Storyboard.Stop();
#endif

                    if (ds.Storyboard != null)
                        ds.Storyboard.Children.Clear();

                    ds.Storyboard = null;
                    ds.Faces = null;
                }
            }
        }

        /// <summary>
        /// Draws Axes
        /// </summary>
        /// <param name="plotAreaSize">Size of the plotArea</param>
        /// <returns>Remaining size of the plotArea after drawing axes</returns>
        private Size RenderAxes(Size plotAreaSize)
        {   
            Axis.SaveOldValueOfAxisRange(Chart.ChartArea.AxisX);
            Axis.SaveOldValueOfAxisRange(Chart.ChartArea.AxisY);
            Axis.SaveOldValueOfAxisRange(Chart.ChartArea.AxisX2);
            Axis.SaveOldValueOfAxisRange(Chart.ChartArea.AxisY2);

            Double top = 0, left = 0, right = 0, bottom = 0;

            if (Chart.PlotDetails.ChartOrientation == ChartOrientationType.Vertical)
            {   
                #region For vertical chart

                Double totalWidthReduced1 = DrawAxesY(plotAreaSize, true);
                plotAreaSize.Width -= totalWidthReduced1;

                UpdateLayoutSettings(plotAreaSize);

                Double totalHeightReduced1 = DrawAxesX(plotAreaSize, true);
                plotAreaSize.Height -= totalHeightReduced1;

                plotAreaSize = SetChartAreaCenterGridMargin(plotAreaSize, ref left, ref top, ref right, ref bottom);

                UpdateLayoutSettings(plotAreaSize);
                
                Double oldAxisXWidth = _plotAreaSize.Width - ((AxisY != null)? AxisY.Width : 0) - ((AxisY2 != null)? AxisY2.Width : 0);

                DrawAxesY(plotAreaSize, true);

                Double newAxisXWidth = _plotAreaSize.Width - ((AxisY != null) ? AxisY.Width : 0) - ((AxisY2 != null) ? AxisY2.Width : 0);

                if (oldAxisXWidth != newAxisXWidth)
                    plotAreaSize.Width = newAxisXWidth;

                Double totalHeightReduced2 = DrawAxesX(plotAreaSize, false);

                if (!Double.IsNaN(totalHeightReduced2) && totalHeightReduced2 != totalHeightReduced1)
                {
                    plotAreaSize = SetChartAreaCenterGridMargin(plotAreaSize, ref left, ref top, ref right, ref bottom);
                    plotAreaSize.Height += totalHeightReduced1;
                    plotAreaSize.Height -= totalHeightReduced2;
                    UpdateLayoutSettings(plotAreaSize);
                    DrawAxesY(plotAreaSize, false);
                    DrawAxesX(plotAreaSize, false);
                }

                #endregion
            }
            else if (Chart.PlotDetails.ChartOrientation == ChartOrientationType.Horizontal)
            {   
                #region For horizontal chart
                
                // Draw the y-axis for Horizontal chart
                Double totalHeightReduced = DrawAxesX(plotAreaSize, true);

                plotAreaSize.Height -= totalHeightReduced;
                UpdateLayoutSettings(plotAreaSize);

               Double totalWidthReduced = DrawAxesY(plotAreaSize, true);

                plotAreaSize.Width -= totalWidthReduced;

                plotAreaSize = SetChartAreaCenterGridMargin(plotAreaSize, ref left, ref top, ref right, ref bottom);

                UpdateLayoutSettings(plotAreaSize);

                plotAreaSize.Width -= SCROLLVIEWER_OFFSET4HORIZONTAL_CHART;
                Double totalHeightReduced2 = DrawAxesX(plotAreaSize, true);
                plotAreaSize.Width += SCROLLVIEWER_OFFSET4HORIZONTAL_CHART;

                Double totalWidthReduced2 = 0;
                                
                if (totalHeightReduced2 != totalHeightReduced)
                {   
                    plotAreaSize.Height += totalHeightReduced;
                    plotAreaSize.Height -= totalHeightReduced2;
                    UpdateLayoutSettings(plotAreaSize);
                    totalWidthReduced2 = DrawAxesY(plotAreaSize, false);
                }

                if (totalWidthReduced2 == 0)
                    totalWidthReduced2 = DrawAxesY(plotAreaSize, false);

                if (!Double.IsNaN(totalWidthReduced2) && totalWidthReduced2 != totalWidthReduced)
                {   
                    plotAreaSize.Width += totalWidthReduced;
                    plotAreaSize.Width -= totalWidthReduced2;
                    UpdateLayoutSettings(plotAreaSize);
                    DrawAxesX(plotAreaSize, false);
                }   

                /*
                if (totalHeightReduced2 != totalHeightReduced)
                {
                    plotAreaSize.Height += totalHeightReduced;
                    plotAreaSize.Height -= totalHeightReduced2;
                    UpdateLayoutSettings(plotAreaSize);
                    DrawAxesY(plotAreaSize);
                }
                else
                    DrawAxesY(plotAreaSize);
                */

                #endregion Horizontal Render
            }
            else
            {   
                UpdateLayoutSettings(plotAreaSize);
            }

            return plotAreaSize;
        }

        /// <summary>
        /// Clean the old chart and draw the chart in PlotCanvas
        /// </summary>
        /// <param name="NewSize">New size of the plotArea</param>
        /// <returns>Actual size of the plotarea excluding axis</returns>
        private Size DrawChart(Size plotAreaSize)
        {   
            ResetStoryboards();

            // Visifire.Profiler.Profiler.Start("RenderAxis");
            Size remainingSizeAfterDrawingAxes = RenderAxes(plotAreaSize);
            // Visifire.Profiler.Profiler.End("RenderAxis");
            // Visifire.Profiler.Profiler.Start("Render");
            RenderChart(remainingSizeAfterDrawingAxes, AxisRepresentations.AxisX, false);
                    
            // Visifire.Profiler.Profiler.End("Render");
            return remainingSizeAfterDrawingAxes;
            // return new Size();
        }

        /// <summary>
        /// Update plotarea layout setting with new size
        /// </summary>
        /// <param name="newSize">New Size of the plotarea layout</param>
        private void UpdateLayoutSettings(Size newSize)
        {
            Chart.PlotArea.Height = newSize.Height;
            Chart.PlotArea.Width = newSize.Width;
 
            Chart._drawingCanvas.Height = newSize.Height;
            Chart._drawingCanvas.Width = newSize.Width;

            Chart.PlotArea.BorderElement.Height = newSize.Height;
            Chart.PlotArea.BorderElement.Width = newSize.Width;

            Double chartSize = 0;

            if (PlotDetails.ChartOrientation == ChartOrientationType.Horizontal)
                chartSize = newSize.Height;
            else
                chartSize = newSize.Width;

            if ((bool)Chart.ScrollingEnabled)
            {   
                // singlePlotWidth helps to generate PlotArea width inside scrollviewer.
                // Viewport can be divided into one or more than one plots. Default value is 30 pixels.
                // Double singlePlotWidth = 10;

                /* The following codes are for maintaining Scale of Axis
                 * 
                    if (AxisX != null && !Double.IsNaN((Double)AxisX.Scale) && AxisX.Scale > 0)
                    {
                        if (PlotDetails.ChartOrientation == ChartOrientationType.Vertical)
                            singlePlotWidth = newSize.Width / AxisX.Scale;
                        else if(PlotDetails.ChartOrientation == ChartOrientationType.Horizontal)
                            singlePlotWidth = newSize.Height / AxisX.Scale;
                    }
                */

                //if (!Double.IsNaN((Double)Chart.MinimumGap) && Chart.MinimumGap > 0)
                //    singlePlotWidth = (Double)Chart.MinimumGap;

                if (PlotDetails.ChartOrientation != ChartOrientationType.NoAxis)
                {   
                    if (Chart.InternalSeries.Count > 0)
                    {   
                        chartSize = CalculatePlotAreaAutoSize(chartSize);
                    }
                }

                if (Double.IsNaN(newSize.Height) || newSize.Height <= 0 || Double.IsNaN(newSize.Width) || newSize.Width <= 0)
                {
                    return; 
                }
                else
                {
                    Chart.IsScrollingActivated = false;

                    if (PlotDetails.ChartOrientation == ChartOrientationType.Vertical)
                    {
                        if (chartSize <= newSize.Width)
                            chartSize = newSize.Width;
                        else
                            Chart.IsScrollingActivated = true;

                        Chart._drawingCanvas.Width = chartSize;
                        Chart.PlotArea.BorderElement.Width = chartSize;
                    }

                    if (PlotDetails.ChartOrientation == ChartOrientationType.Horizontal)
                    {
                        if(chartSize <= newSize.Height)
                            chartSize = newSize.Height;
                        else
                            Chart.IsScrollingActivated = true;

                        Chart._drawingCanvas.Height = chartSize;
                        Chart.PlotArea.BorderElement.Height = chartSize;
                    }
                }
            }
            
            ScrollableLength = chartSize;
            //Chart._plotAreaScrollViewer.Width = newSize.Width;
            Chart._plotAreaScrollViewer.Height = newSize.Height;
            Chart._plotAreaScrollViewer.UpdateLayout();
            PlotAreaScrollViewer = Chart._plotAreaScrollViewer;

            // this.PlottingCanvas
            // this.ChartVisualCanvas
            // this.PlotAreaCanvas


        }

        internal Double CalculateChartSizeForZooming(Chart chart, Double currentSize)
        {
            Double chartSize = currentSize;
            MAX_CHART_SIZE = 15000;

            if (_isFirstTimeRender)
            {
                Chart.AxesX[0]._internalMinimumZoomingScale = currentSize / MAX_CHART_SIZE + 0.0000001;
                _oldZoomingScale = Chart.AxesX[0]._internalMinimumZoomingScale;
                Chart.AxesX[0]._internalZoomingScale = Chart.AxesX[0]._internalMinimumZoomingScale;
                chartSize = MAX_CHART_SIZE * Chart.AxesX[0]._internalZoomingScale;
            }
            else
            {   
                Double internalMinimumZoomingScale = currentSize / MAX_CHART_SIZE + 0.0000001;

                if (internalMinimumZoomingScale != _oldZoomingScale && !_isDragging)
                {   
                    Chart.AxesX[0]._internalMinimumZoomingScale = internalMinimumZoomingScale;
                    if (!Double.IsNaN(Chart.AxesX[0].ScrollBarElement.Scale) && !Double.IsNaN(Chart.AxesX[0].ScrollBarElement._currentThumbSize) && zoomCount != 0)
                        Chart.AxesX[0]._internalZoomingScale = Chart.AxesX[0]._internalMinimumZoomingScale + (1 - Chart.AxesX[0]._internalMinimumZoomingScale) * (1 - Chart.AxesX[0].ScrollBarElement.Scale);
                    else
                        chart.AxesX[0]._internalZoomingScale = Chart.AxesX[0]._internalMinimumZoomingScale;
                    _oldZoomingScale = Chart.AxesX[0]._internalMinimumZoomingScale;
                }
                //else if (IsZoomingBetweenXValue)
                //{
                //    chartSize = currentSize + currentSize * Chart.AxesX[0]._internalZoomingScale;
                //    if (chartSize > MAX_CHART_SIZE)
                //        chartSize = MAX_CHART_SIZE;
                //}
                //else // Zooming using ZoomBar thumb
                
                chartSize = MAX_CHART_SIZE * Chart.AxesX[0]._internalZoomingScale;
            }

            return chartSize;
        }

        /// <summary>
        /// Calculate PlotArea size for auto scroll
        /// </summary>
        /// <param name="minGap">MinGap value between datapoints</param>
        /// <returns>Auto PlotArea size</returns>
        private Double CalculatePlotAreaAutoSize(Double currentSize)
        {
            Double chartSize;

            Chart chart = (Chart as Chart);
            if (chart.ZoomingEnabled)
            {
                chartSize = CalculateChartSizeForZooming(chart, currentSize);
            }
            else
            {   
                if ((Chart as Chart).MinimumGap != null)
                {
                    //if (AxisX.XValueType != ChartValueTypes.Numeric && AxisX != null && PlotDetails.ListOfAllDataPoints.Count > 0)
                    //{
                    //    //TimeSpan dateRange = AxisX.MaxDate.Subtract(AxisX.MinDate);
                    //    Double maxNoOfDataPoint = ((from dataPoint in PlotDetails.ListOfAllDataPoints select dataPoint.InternalXValue).Max());
                    //    chartSize =(Double) (Chart as Chart).MinimumGap * maxNoOfDataPoint;
                    //}
                    //else
                    chartSize = (Double)(Chart as Chart).MinimumGap * ((from series in Chart.InternalSeries select series.InternalDataPoints.Count).Max());
                }
                else if ((!Double.IsNaN(Chart.AxesX[0].ScrollBarScale)) && !IsAutoCalculatedScrollBarScale)
                {
                    chartSize = currentSize / Chart.AxesX[0].ScrollBarScale;
                }
                else
                {
                    if (PlotDetails.ListOfAllDataPoints.Count > 0)
                    {
                        Double minDiff = PlotDetails.GetMinOfMinDifferencesForXValue();
                        Double maxXValue = (Double)((from dataPoint in PlotDetails.ListOfAllDataPoints select dataPoint.InternalXValue).Max());
                        Double minXValue = (Double)((from dataPoint in PlotDetails.ListOfAllDataPoints select dataPoint.InternalXValue).Min());

                        Double maxDiff = maxXValue - minXValue;

                        Double magicNumber = currentSize * 34 / ((Double)550 * (PlotDetails.DrawingDivisionFactor == 0 ? 1 : PlotDetails.DrawingDivisionFactor));

                        if (maxDiff / minDiff > magicNumber)
                        {
                            chartSize = (maxDiff / minDiff) * ((Double)550 * (PlotDetails.DrawingDivisionFactor == 0 ? 1 : PlotDetails.DrawingDivisionFactor)) / 34;
                        }
                        else
                        {
                            chartSize = currentSize;
                        }
                    }
                    else
                    {
                        chartSize = currentSize;
                    }

                }
#if SL          
                if (chartSize > 32000)
                {
                    chartSize = 32000;
                    Chart.AxesX[0].IsNotificationEnable = false;
                    Chart.AxesX[0].ScrollBarScale = currentSize / chartSize;
                    Chart.AxesX[0].IsNotificationEnable = true;
                }
#endif

                if ((Double.IsNaN(Chart.AxesX[0].ScrollBarScale)))
                {
                    Chart.AxesX[0].IsNotificationEnable = false;
                    Chart.AxesX[0].ScrollBarScale = currentSize / chartSize;
                    Chart.AxesX[0].IsNotificationEnable = true;
                    //IsAutoCalculatedScrollBarScale = true;
                }
                else if (!Double.IsNaN(Chart.AxesX[0].ScrollBarScale) && IsAutoCalculatedScrollBarScale)
                {
                    Chart.AxesX[0].IsNotificationEnable = false;
                    Chart.AxesX[0].ScrollBarScale = currentSize / chartSize;
                    Chart.AxesX[0].IsNotificationEnable = true;
                }
            
            }

            return chartSize;
        }

        internal Boolean IsAutoCalculatedScrollBarScale
        {
            get;
            set;
        }
        
        /// <summary>
        /// Set a blank dataseries if series collection of chart is empty
        /// </summary>
        private void SetBlankSeries()
        {
            DataSeries ds = new DataSeries(){ _isAutoGenerated = true };
            ds.IsNotificationEnable = false;
            ds.RenderAs = RenderAs.Column;
            ds.LightingEnabled = true;
            ds.ShadowEnabled = false;
            ds.Chart = Chart;

            for (Int32 i = 1; i <= 5; i++)
            {
                DataPoint dp = new DataPoint();
                dp.XValue = i;
                dp.YValue = 0;
                dp.Color = Graphics.TRANSPARENT_BRUSH;
                dp.AxisXLabel = i.ToString();
                dp.Chart = Chart;
                ds.DataPoints.Add(dp);
            }

            ds.IsNotificationEnable = false;
            Chart.InternalSeries.Add(ds);
            
        }

        /// <summary>
        /// Clear unwanted children from PlotAreaCanvas
        /// </summary>
        private void ClearPlotAreaChildren()
        {
            if (PlottingCanvas != null)
            {
                PlottingCanvas.Loaded -= PlottingCanvas_Loaded;
                PlottingCanvas.Children.Clear();
                PlotAreaCanvas.Children.Remove(PlottingCanvas);
                PlottingCanvas = null;
            }
        }

        /// <summary>
        /// Draws the horizontal 3D Plank
        /// </summary>
        /// <param name="plankDepth">PlankDepth</param>
        /// <param name="plankThickness">PlankThickness</param>
        /// <param name="position">Position</param>
        private void DrawHorizontalPlank(Double plankDepth, Double plankThickness, Double position, AxisRepresentations axisChanged, Boolean isPartialUpdate)
        {
            if (isPartialUpdate && axisChanged == AxisRepresentations.AxisY)
            {   
                //  Update with new size
                ColumnChart.Update3DPlank(ScrollableLength - plankDepth, plankThickness, plankDepth, _horizontalPlank);
#if SL
                _horizontalPlank.Visual.SetValue(Canvas.TopProperty, position - plankThickness);
#else
                PlottingCanvas.Measure(new Size(Double.MaxValue, Double.MaxValue));
                _horizontalPlank.Visual.SetValue(Canvas.TopProperty, PlottingCanvas.DesiredSize.Height - plankThickness);
#endif

                return;
            }

            if (_horizontalPlank != null && _horizontalPlank.Visual != null && _horizontalPlank.Visual.Parent != null)
            {
                Panel parent = _horizontalPlank.Visual.Parent as Canvas;
                parent.Children.Remove(_horizontalPlank.Visual);
            }

            Brush frontBrush, topBrush, rightBrush;
            ExtendedGraphics.GetBrushesForPlank(Chart, out frontBrush, out topBrush, out rightBrush, false);
            
            _horizontalPlank = ColumnChart.Get3DPlank(ScrollableLength - plankDepth, plankThickness, plankDepth, frontBrush, topBrush, rightBrush);
            Panel plank = _horizontalPlank.Visual as Panel;
#if SL
            plank.SetValue(Canvas.TopProperty, position - plankThickness);
#else
            PlottingCanvas.Measure(new Size(Double.MaxValue, Double.MaxValue));
            plank.SetValue(Canvas.TopProperty, PlottingCanvas.DesiredSize.Height - plankThickness);
#endif

            plank.SetValue(Canvas.ZIndexProperty, -1);

            plank.Opacity = 0.9;

            PlottingCanvas.Children.Add(plank);
        }


        

        internal Faces _horizontalPlank;
        internal Faces _verticalPlank;
        
        /// <summary>
        /// Draws the Vertical 3D Plank
        /// </summary>
        /// <param name="plankDepth">PlankDepth</param>
        /// <param name="plankThickness">PlankThickness</param>
        private void DrawVerticalPlank(Double plankDepth, Double plankThickness, AxisRepresentations axisChanged, Boolean isPartialUpdate)
        {
            if (isPartialUpdate && axisChanged == AxisRepresentations.AxisX)
            {
                ColumnChart.Update3DPlank(plankThickness, ScrollableLength - plankDepth, plankDepth, _verticalPlank);
                return;
            }

            if (_verticalPlank != null && _verticalPlank.Visual != null && _verticalPlank.Visual.Parent != null)
            {
                Panel parent = _verticalPlank.Visual.Parent as Canvas;
                parent.Children.Remove(_verticalPlank.Visual);
            }

            RectangularChartShapeParams columnParams = new RectangularChartShapeParams();
            Brush frontBrush, topBrush, rightBrush;

            List<Color> colors = new List<Color>();
            colors.Add(Color.FromArgb(255, 134, 134, 134));  // #FF868686
            colors.Add(Color.FromArgb(255, 210, 210, 210));  // #FFD2D2D2
            colors.Add(Color.FromArgb(255, 255, 255, 255));  // #FFFFFFFF
            colors.Add(Color.FromArgb(255, 223, 223, 223));  // #FFDFDFDF

            frontBrush = Graphics.CreateLinearGradientBrush(0, new Point(1.1, 0.49), new Point(-0.15, 0.49), colors, new List<double>() { 0, 0.844, 1, 0.442 });

            colors = new List<Color>();
            colors.Add(Color.FromArgb(255, 232, 232, 232));  // #FFE8E8E8
            colors.Add(Color.FromArgb(255, 142, 135, 135));  // #FF8E8787

            rightBrush = Graphics.CreateLinearGradientBrush(0, new Point(0, 0.5), new Point(1, 0.5), colors, new List<double>() { 1, 0 });

            colors = new List<Color>();
            colors.Add(Color.FromArgb(255, 232, 232, 232));  // #FFE8E8E8
            colors.Add(Color.FromArgb(255, 142, 135, 135));  // #FF8E8787

            topBrush = Graphics.CreateLinearGradientBrush(0, new Point(0.084, 0.441), new Point(1.916, 0.443), colors, new List<double>() { 0, 1 });

            _verticalPlank = ColumnChart.Get3DPlank(plankThickness, ScrollableLength - plankDepth, plankDepth, frontBrush, topBrush, rightBrush);
            Panel plank = _verticalPlank.Visual as Panel;

            plank.SetValue(Canvas.TopProperty, plankDepth);
            plank.SetValue(Canvas.ZIndexProperty, -1);

            PlottingCanvas.Children.Add(plank);
        }

        /// <summary>
        /// Draws the Vertical 3D Plank
        /// </summary>
        /// <param name="height">Height of the PlotArea canvas</param>
        /// <param name="plankDepth">PlankDepth</param>
        /// <param name="plankThickness">PlankThickness</param>
        /// <param name="plankOpacity">PlankOpacity</param>
        private void DrawVerticalPlank(Double height, Double plankDepth, Double plankThickness, Double plankOpacity, Boolean isPartialUpdate)
        {
            if (isPartialUpdate)
            {
                ColumnChart.Update3DPlank(plankThickness, height, plankDepth, _verticalPlank);
                return;
            }

            // RectangularChartShapeParams columnParams = new RectangularChartShapeParams();
            // columnParams.BackgroundBrush = new SolidColorBrush(Color.FromArgb((Byte)255, (Byte)127, (Byte)127, (Byte)127));
            // columnParams.Lighting = true;
            // columnParams.Size = new Size(plankThickness, height);
            // columnParams.Depth = plankDepth;

            if (_verticalPlank != null && _verticalPlank.Visual != null && _verticalPlank.Visual.Parent != null)
            {
                Panel parent = _verticalPlank.Visual.Parent as Canvas;
                parent.Children.Remove(_verticalPlank.Visual);
            }

            List<Color> colors = new List<Color>();
            colors.Add(Color.FromArgb(255, 232, 232, 232));
            colors.Add(Color.FromArgb(255, 142, 135, 135));

            Brush rightBrush = Graphics.CreateLinearGradientBrush(0, new Point(0, 0.5), new Point(1, 0.5), colors, new List<double>() { 0, 1 });

            _verticalPlank = ColumnChart.Get3DPlank(plankThickness, height, plankDepth, null, null, rightBrush);
            Panel plank = _verticalPlank.Visual as Panel;

            plank.SetValue(Canvas.TopProperty, plankDepth);
            plank.SetValue(Canvas.ZIndexProperty, -1);
            plank.SetValue(Canvas.OpacityProperty, plankOpacity);

            PlottingCanvas.Children.Add(plank);
        }

        /// <summary>
        /// Create visual of trendLines and add to ChartVisualCanvas
        /// </summary>
        /// <param name="axis">Axis</param>
        /// <param name="trendLinesReferingToAAxes">List of trendLine</param>
        private void AddTrendLines(Axis axis, List<TrendLine> trendLinesReferingToAAxes, Canvas trendLineCanvas)
        {   
            if (axis != null)
            {   
                foreach (TrendLine trendLine in trendLinesReferingToAAxes)
                {   
                    trendLine.ReferingAxis = axis;

                    if (trendLine.Visual == null)
                    {
                        trendLine.CreateVisualObject(trendLineCanvas.Width, trendLineCanvas.Height);

                        if (trendLine.Visual != null)
                            trendLineCanvas.Children.Add(trendLine.Visual);
                    }
                    else
                        trendLine.CreateVisualObject(trendLineCanvas.Width, trendLineCanvas.Height);

                    if (trendLine.Visual != null)
                    {
                        RectangleGeometry clipRectangle = new RectangleGeometry();
                        clipRectangle.Rect = new Rect(0, 0, trendLineCanvas.Width, trendLineCanvas.Height);
                        trendLine.Visual.Clip = clipRectangle;

                        Int32 zIndex = (Int32)trendLine.GetValue(Canvas.ZIndexProperty);

                        if (zIndex == 0)
                            trendLine.Visual.SetValue(Canvas.ZIndexProperty, (Int32)(-999));
                        else
                            trendLine.Visual.SetValue(Canvas.ZIndexProperty, zIndex);
                    }
                }
            }
        }

        private void CleanUpTrendLines(Canvas trendLineCanvas)
        {
            foreach (TrendLine trendLine in Chart.TrendLines)
            {
                if (trendLine.Visual != null)
                {
                    trendLineCanvas.Children.Remove(trendLine.Visual);
                    trendLine.Visual.Children.Clear();
                    trendLine.Visual = null;
                }
            }
        }

        /// <summary>
        /// Render trendLines
        /// </summary>
        private void RenderTrendLines()
        {
            if (Chart._forcedRedraw)
            {
                CleanUpTrendLines(ChartVisualCanvas);
            }

            List<TrendLine> trendLinesReferingToPrimaryAxesX;
            List<TrendLine> trendLinesReferingToPrimaryAxisY;
            List<TrendLine> trendLinesReferingToSecondaryAxesX;
            List<TrendLine> trendLinesReferingToSecondaryAxisY;

            if (PlotDetails.ChartOrientation == ChartOrientationType.Vertical)
            {   
                trendLinesReferingToPrimaryAxesX = (from trendline in Chart.TrendLines
                                                    where (trendline.Orientation == Orientation.Vertical) && (trendline.AxisType == AxisTypes.Primary)
                                                    select trendline).ToList();
                trendLinesReferingToPrimaryAxisY = (from trendline in Chart.TrendLines
                                                    where (trendline.Orientation == Orientation.Horizontal) && (trendline.AxisType == AxisTypes.Primary)
                                                    select trendline).ToList();
                trendLinesReferingToSecondaryAxesX = (from trendline in Chart.TrendLines
                                                      where (trendline.Orientation == Orientation.Vertical) && (trendline.AxisType == AxisTypes.Secondary)
                                                      select trendline).ToList();
                trendLinesReferingToSecondaryAxisY = (from trendline in Chart.TrendLines
                                                      where (trendline.Orientation == Orientation.Horizontal) && (trendline.AxisType == AxisTypes.Secondary)
                                                      select trendline).ToList();
            }
            else
            {
                trendLinesReferingToPrimaryAxesX = (from trendline in Chart.TrendLines
                                                    where (trendline.Orientation == Orientation.Horizontal) && (trendline.AxisType == AxisTypes.Primary)
                                                    select trendline).ToList();
                trendLinesReferingToPrimaryAxisY = (from trendline in Chart.TrendLines
                                                    where (trendline.Orientation == Orientation.Vertical) && (trendline.AxisType == AxisTypes.Primary)
                                                    select trendline).ToList();
                trendLinesReferingToSecondaryAxesX = (from trendline in Chart.TrendLines
                                                      where (trendline.Orientation == Orientation.Horizontal) && (trendline.AxisType == AxisTypes.Secondary)
                                                      select trendline).ToList();
                trendLinesReferingToSecondaryAxisY = (from trendline in Chart.TrendLines
                                                      where (trendline.Orientation == Orientation.Vertical) && (trendline.AxisType == AxisTypes.Secondary)
                                                      select trendline).ToList();
            }

            // Canvas trendLineCanvas = new Canvas() { Height = ChartVisualCanvas.Height, Width = ChartVisualCanvas.Width };

            AddTrendLines(AxisX, trendLinesReferingToPrimaryAxesX, ChartVisualCanvas);

            AddTrendLines(AxisY, trendLinesReferingToPrimaryAxisY, ChartVisualCanvas);

            AddTrendLines(AxisX2, trendLinesReferingToSecondaryAxesX, ChartVisualCanvas);

            AddTrendLines(AxisY2, trendLinesReferingToSecondaryAxisY, ChartVisualCanvas);

            //ChartVisualCanvas.Children.Add(trendLineCanvas);
        }

        /// <summary>
        /// Create visual of grids of axis and add to ChartVisualCanvas
        /// </summary>
        /// <param name="axis">Axis</param>
        /// <param name="width">ChartVisualCanvas width</param>
        /// <param name="height">ChartVisualCanvas height</param>
        /// <param name="isAnimationEnabled">Whether animation is enabled</param>
        /// <param name="styleName">Style</param>
        private void AddGrids(Axis axis, Double width, Double height, Boolean isAnimationEnabled, String styleName)
        {   
            if (Chart._forcedRedraw)
            {
                CleanUpGrids(axis);
            }
            
            foreach (ChartGrid grid in axis.Grids)
            {
                grid.IsNotificationEnable = false;
                grid.Chart = Chart;

                grid.ApplyStyleFromTheme(Chart, styleName);

                if (grid.Visual == null)
                {
                    grid.CreateVisualObject(width, height, isAnimationEnabled, GRID_ANIMATION_DURATION);

                    if (grid.Visual != null)
                        ChartVisualCanvas.Children.Add(grid.Visual);
                }
                else
                    grid.CreateVisualObject(width, height, isAnimationEnabled, GRID_ANIMATION_DURATION);

                if (grid.Visual != null)
                    grid.Visual.SetValue(Canvas.ZIndexProperty, (Int32)(-1000));

                grid.IsNotificationEnable = true;
            }
        }

        private void CleanUpGrids(Axis axis)
        {
            foreach (ChartGrid grid in axis.Grids)
            {
                if (grid.Visual != null)
                {
                    ChartVisualCanvas.Children.Remove(grid.Visual);
                    grid.Visual.Children.Clear();
                    grid.Visual = null;
                }
            }
        }

        /// <summary>
        /// Draw grids
        /// </summary>
        private void RenderGrids()
        {
            Boolean isAnimationEnabled = Chart._internalAnimationEnabled && !_isAnimationFired;

            if(AxisX != null)
                AddGrids(AxisX, ChartVisualCanvas.Width, ChartVisualCanvas.Height, isAnimationEnabled, AxisX.AxisRepresentation.ToString() + "Grid");

            if (AxisX2 != null)
                AddGrids(AxisX2, ChartVisualCanvas.Width, ChartVisualCanvas.Height, isAnimationEnabled, AxisX2.AxisRepresentation.ToString() + "Grid");

            if (AxisY != null)
                AddGrids(AxisY, ChartVisualCanvas.Width, ChartVisualCanvas.Height, isAnimationEnabled, AxisY.AxisRepresentation.ToString() + "Grid");
            
            if (AxisY2 != null)
                AddGrids(AxisY2, ChartVisualCanvas.Width, ChartVisualCanvas.Height, isAnimationEnabled, AxisY2.AxisRepresentation.ToString() + "Grid");
        }

        /// <summary>
        /// Resize existing panels to update the chart
        /// </summary>
        internal void ResizePanels(Size remainingSizeAfterDrawingAxes, AxisRepresentations renderAxisType, Boolean isPartialUpdate)
        {   
            PlotAreaScrollViewer = Chart._plotAreaScrollViewer;

            if(_isFirstTimeRender)
                PlotAreaScrollViewer.Background = Graphics.TRANSPARENT_BRUSH;

            PlotAreaCanvas.Width = remainingSizeAfterDrawingAxes.Width;
            PlotAreaCanvas.Height = remainingSizeAfterDrawingAxes.Height;
                        
            if (Chart._forcedRedraw || PlottingCanvas == null)
            {
                if (PlottingCanvas != null)
                {
                    PlottingCanvas.Children.Clear();
                    PlottingCanvas.Loaded -= new RoutedEventHandler(PlottingCanvas_Loaded);
                }
                
                PlottingCanvas = new Canvas();

                PlottingCanvas.Loaded += new RoutedEventHandler(PlottingCanvas_Loaded);
                PlottingCanvas.SetValue(Canvas.ZIndexProperty, 1);
                PlotAreaCanvas.Children.Add(PlottingCanvas);
                //PlottingCanvas.Background = Graphics.GetRandomColor();
            }

            if (Double.IsNaN(remainingSizeAfterDrawingAxes.Height) || remainingSizeAfterDrawingAxes.Height <= 0 || Double.IsNaN(remainingSizeAfterDrawingAxes.Width) || remainingSizeAfterDrawingAxes.Width <= 0)
            {
                return;
            }
            else
            {
                if (PlotDetails.ChartOrientation == ChartOrientationType.Vertical)
                {
                    PlottingCanvas.Width = ScrollableLength + PLANK_DEPTH;
                    PlottingCanvas.Height = remainingSizeAfterDrawingAxes.Height;
                }
                else if (PlotDetails.ChartOrientation == ChartOrientationType.Horizontal)
                {
                    PlottingCanvas.Width = remainingSizeAfterDrawingAxes.Width;
                    PlottingCanvas.Height = ScrollableLength + PLANK_DEPTH;
                }
            }

            // Create the chart canvas

            if (Chart._forcedRedraw || ChartVisualCanvas == null)
            {
                if (ChartVisualCanvas != null)
                    ChartVisualCanvas.Children.Clear();

                ChartVisualCanvas = new Canvas();

                PlottingCanvas.Children.Add(ChartVisualCanvas);
            }

            //if (Chart._forcedRedraw)
            //{
            //    if (ChartVisualCanvas == null)
            //    {
            //        ChartVisualCanvas = new Canvas();

            //        PlottingCanvas.Children.Add(ChartVisualCanvas);
            //    }
            //    else
            //    {
            //        ChartVisualCanvas.Children.Clear();

            //        if (ChartVisualCanvas.Parent != null && (ChartVisualCanvas.Parent as Canvas).Children.Contains(ChartVisualCanvas))
            //            (ChartVisualCanvas.Parent as Canvas).Children.Remove(ChartVisualCanvas);
            //        PlottingCanvas.Children.Add(ChartVisualCanvas);
            //    }
            //}

            // Default size of the chart canvas
            Size chartCanvasSize = new Size(0, 0);

            // Create the various region required for drawing charts
            switch (PlotDetails.ChartOrientation)
            {   
                case ChartOrientationType.Vertical:
                    chartCanvasSize = CreateRegionsForVerticalCharts(ScrollableLength, remainingSizeAfterDrawingAxes, renderAxisType, isPartialUpdate);
                    // set chart Canvas position
                    ChartVisualCanvas.SetValue(Canvas.LeftProperty, PLANK_DEPTH);
                    Chart.PlotArea.BorderElement.SetValue(Canvas.LeftProperty, PLANK_DEPTH);

                    break;

                case ChartOrientationType.Horizontal:
                    chartCanvasSize = CreateRegionsForHorizontalCharts(ScrollableLength, remainingSizeAfterDrawingAxes, renderAxisType, isPartialUpdate);
                    // set chart Canvas position
                    ChartVisualCanvas.SetValue(Canvas.LeftProperty, PLANK_OFFSET);
                    Chart.PlotArea.BorderElement.SetValue(Canvas.LeftProperty, PLANK_OFFSET);
                    break;

                case ChartOrientationType.NoAxis:
                    chartCanvasSize = CreateRegionsForChartsWithoutAxis(remainingSizeAfterDrawingAxes);
                    break;

                default:
                    // No chart to render
                    break;
            }

            // Don't atempt to draw chart if the size is not fesiable
            if (chartCanvasSize.Width <= 0 || chartCanvasSize.Height <= 0)
                return;

            // set the ChartVisualCanvas Size
            ChartVisualCanvas.Width = chartCanvasSize.Width - ((PlotDetails.ChartOrientation == ChartOrientationType.Horizontal) ? SCROLLVIEWER_OFFSET4HORIZONTAL_CHART : 0);
            ChartVisualCanvas.Height = chartCanvasSize.Height - ((PlotDetails.ChartOrientation == ChartOrientationType.NoAxis) ? Chart.SHADOW_DEPTH : 0);
            Chart.PlotArea.BorderElement.Height = ChartVisualCanvas.Height;
            Chart.PlotArea.BorderElement.Width = chartCanvasSize.Width;
            Chart.PlotArea.ApplyBevel(PLANK_DEPTH, PLANK_THICKNESS);
            Chart.PlotArea.ApplyShadow(remainingSizeAfterDrawingAxes, PLANK_OFFSET, PLANK_DEPTH, PLANK_THICKNESS);

            Chart._plotCanvas.Width = PlottingCanvas.Width;
            Chart._plotCanvas.Height = PlottingCanvas.Height;

            Chart._bottomAxisScrollBar.UpdateLayout();
            Chart._topAxisScrollBar.UpdateLayout();
            Chart._leftAxisScrollBar.UpdateLayout();
            Chart._rightAxisScrollBar.UpdateLayout();
        }

        /// <summary>
        /// Renders charts based on the orientation type
        /// </summary>
        /// <param name="newSize">NewSize</param>
        private void RenderChart(Size remainingSizeAfterDrawingAxes, AxisRepresentations renderAxisType, Boolean isPartialUpdate)
        {
            if (Chart._forcedRedraw || PlotDetails.ChartOrientation == ChartOrientationType.NoAxis)
            {
                Chart._forcedRedraw = true;
                ClearPlotAreaChildren();
            }

            ResizePanels(remainingSizeAfterDrawingAxes, renderAxisType, isPartialUpdate);

            // Draw the chart grids
            if (PlotDetails.ChartOrientation != ChartOrientationType.NoAxis)
            {   
                RenderGrids();
                RenderTrendLines();
            }

            // Render each plot group from the plotgroups list of plotdetails
            RenderSeries();

            _renderCount++;
        }

        /// <summary>
        /// Save Axis scrollbar Offset and reset scroll-viewer content margin
        /// </summary>
        /// <param name="axis">Axis</param>
        /// <param name="scrollBarOffset">ScrollBarOffset as Double</param>
        private void SaveAxisContentOffsetAndResetMargin(Axis axis, Double scrollBarOffset)
        {
            axis.CurrentScrollScrollBarOffset = scrollBarOffset / axis.ScrollBarElement.Maximum;
            //System.Diagnostics.Debug.WriteLine("Offset" + scrollBarOffset.ToString());
        }

        private const Double ZOOMING_MAX_VAL = 100;

        /// <summary>
        /// Event handler for scroll event of the axis-x scrollbar
        /// </summary>
        /// <param name="sender">Scrollbar</param>
        /// <param name="e">System.Windows.Controls.Primitives.ScrollEventArgs</param>
        private void AxesXScrollBarElement_Scroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e)
        {
            Chart chart = Chart as Chart;
            if (PlotDetails.ChartOrientation == ChartOrientationType.Vertical)
            {
                Double offset = e.NewValue;

                //if (zoomCount == 1)
                //    offset = 0;

                AxisX.ScrollBarElement.IsStretchable = chart.ZoomingEnabled;

                if ((Chart as Chart).ZoomingEnabled)
                {
                    AxisX.ScrollBarElement.Maximum = ZOOMING_MAX_VAL;
                }
                else
                {
#if SL
                    AxisX.ScrollBarElement.Maximum = ScrollableLength - PlotAreaScrollViewer.ViewportWidth;
                    AxisX.ScrollBarElement.ViewportSize = PlotAreaScrollViewer.ViewportWidth;
#else           
                    AxisX.ScrollBarElement.Maximum = ScrollableLength - PlotAreaScrollViewer.ActualWidth;
                    AxisX.ScrollBarElement.ViewportSize = PlotAreaScrollViewer.ActualWidth;
#endif
                }
#if WPF         
                if (e.NewValue <= 1)
                    offset = e.NewValue * AxisX.ScrollBarElement.Maximum;
#endif

                Double offsetInPixel = offset;

                //  if (AxisX.ScrollViewerElement.Children.Count > 0)
                //  (AxisX.ScrollViewerElement.Children[0] as FrameworkElement).SetValue(Canvas.LeftProperty, -offset);
                
                if (AxisX.ScrollViewerElement.Children.Count > 0)
                {
                    FrameworkElement scrollViewerContent = (AxisX.ScrollViewerElement.Children[0] as FrameworkElement);
                    /*Double scrollViewerOffset = scrollViewerContent.Width - PlotAreaScrollViewer.ViewportWidth;

                    scrollViewerOffset = chart.ZoomingEnabled ? offset * (scrollViewerOffset / ZOOMING_MAX_VAL) : offset;
                    //scrollViewerOffset = Math.Min((scrollViewerContent.Width - AxisX.Width), scrollViewerOffset);
                    PlotAreaScrollViewer.ScrollToHorizontalOffset(scrollViewerOffset);
                    scrollViewerContent.SetValue(Canvas.LeftProperty, -scrollViewerOffset);
                    * */
                    Double scrollViewerOffset = GetScrollingOffsetOfAxis(AxisX, offset);

                    if (!Double.IsNaN(scrollViewerOffset))
                    {
                        Chart._plotAreaScrollViewer.ScrollToHorizontalOffset(scrollViewerOffset);
                        scrollViewerContent.SetValue(Canvas.LeftProperty, -1 * scrollViewerOffset);
                        System.Diagnostics.Debug.WriteLine("scrollViewerOffset =" + (-scrollViewerOffset).ToString());
                    }
                }   

                SaveAxisContentOffsetAndResetMargin(AxisX, offset);

                AxisX._isScrollToOffsetEnabled = false;
                offset = offset / (AxisX.ScrollBarElement.Maximum - AxisX.ScrollBarElement.Minimum);
                
                if (!Double.IsNaN(offset))
                    AxisX.ScrollBarOffset = (offset > 1) ? 1 : (offset < 0) ? 0 : offset;

                AxisX._isScrollToOffsetEnabled = true;

                AxisX.FireScrollEvent(e, offsetInPixel);
            }

            if (PlotDetails.ChartOrientation == ChartOrientationType.Horizontal)
            {
                Double offset = e.NewValue;

                //if (zoomCount == 1)
                //    offset = 0;

                AxisX.ScrollBarElement.IsStretchable = chart.ZoomingEnabled;

                if (Chart.ZoomingEnabled)
                {
                    AxisX.ScrollBarElement.Maximum = ZOOMING_MAX_VAL;
                }
                else
                {
#if SL
                    AxisX.ScrollBarElement.Maximum = ScrollableLength - PlotAreaScrollViewer.ViewportHeight;
                    AxisX.ScrollBarElement.ViewportSize = PlotAreaScrollViewer.ViewportHeight;
#else
                    AxisX.ScrollBarElement.Maximum = ScrollableLength - PlotAreaScrollViewer.ActualHeight;
                    AxisX.ScrollBarElement.ViewportSize = PlotAreaScrollViewer.ActualHeight;
#endif
                }

#if WPF
                if (e.NewValue <= 1)
                    offset = e.NewValue * AxisX.ScrollBarElement.Maximum;
#endif

                Double offsetInPixel = offset;



                //PlotAreaScrollViewer.ScrollToVerticalOffset(offset);

                //if (AxisX.ScrollViewerElement.Children.Count > 0)
                //    (AxisX.ScrollViewerElement.Children[0] as FrameworkElement).SetValue(Canvas.TopProperty, -offset);


                if (AxisX.ScrollViewerElement.Children.Count > 0)
                {
                   /* FrameworkElement scrollViewerContent = (AxisX.ScrollViewerElement.Children[0] as FrameworkElement);

                    Double scrollViewerOffset = (chart.ZoomingEnabled ? offset * (ChartVisualCanvas.Height / ZOOMING_MAX_VAL) : offset);
                    PlotAreaScrollViewer.ScrollToVerticalOffset(scrollViewerOffset);
                    scrollViewerOffset = Math.Min((ChartVisualCanvas.Height - AxisX.Height), scrollViewerOffset);
                    scrollViewerContent.SetValue(Canvas.TopProperty, -scrollViewerOffset);*/

                    FrameworkElement scrollViewerContent = (AxisX.ScrollViewerElement.Children[0] as FrameworkElement);

                    if (scrollViewerContent != null)
                    {
                        Double scrollViewerOffset = GetScrollingOffsetOfAxis(AxisX, offset);
                        offsetInPixel = scrollViewerOffset;
                        if (!Double.IsNaN(scrollViewerOffset))
                        {
                            Chart._plotAreaScrollViewer.ScrollToVerticalOffset(scrollViewerOffset);
                            scrollViewerContent.SetValue(Canvas.TopProperty, -1 * scrollViewerOffset);
                        }
                    }
                }

                SaveAxisContentOffsetAndResetMargin(AxisX, (AxisX.ScrollBarElement.Maximum - offset));

                AxisX._isScrollToOffsetEnabled = false;
                offset = (AxisX.ScrollBarElement.Maximum - offset) / (AxisX.ScrollBarElement.Maximum - AxisX.ScrollBarElement.Minimum);
                
                if(!Double.IsNaN(offset))
                    AxisX.ScrollBarOffset = (offset > 1) ? 1 : (offset < 0) ? 0 : offset;

                AxisX._isScrollToOffsetEnabled = true;
                
                AxisX.FireScrollEvent(e, offsetInPixel);

            }

            if (AxisX2 != null)
            {
                AxisX2.ScrollBarElement.Value = e.NewValue;
            }
        }

        /// <summary>
        /// Event handler for scroll event of the secondary axis-x scrollbar
        /// </summary>
        /// <param name="sender">Scrollbar</param>
        /// <param name="e">System.Windows.Controls.Primitives.ScrollEventArgs</param>
        private void AxesX2ScrollBarElement_Scroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e)
        {
            if (PlotDetails.ChartOrientation == ChartOrientationType.Vertical)
            {
                Double offset = e.NewValue;

#if SL
                AxisX2.ScrollBarElement.Maximum = ScrollableLength - PlotAreaScrollViewer.ViewportWidth;
                AxisX2.ScrollBarElement.ViewportSize = PlotAreaScrollViewer.ViewportWidth;
#else

                AxisX2.ScrollBarElement.Maximum = ScrollableLength - PlotAreaScrollViewer.ActualWidth;
                AxisX2.ScrollBarElement.ViewportSize = PlotAreaScrollViewer.ActualWidth;
                if (e.NewValue <= 1)
                    offset = e.NewValue * AxisX2.ScrollBarElement.Maximum;
#endif
                Double offsetInPixel = offset;


                //PlotAreaScrollViewer.ScrollToHorizontalOffset(offset);

                //if (AxisX2.ScrollViewerElement.Children.Count > 0)
                //    (AxisX2.ScrollViewerElement.Children[0] as FrameworkElement).Margin = new Thickness(offset, 0, 0, 0);

                if (AxisX2.ScrollViewerElement.Children.Count > 0)
                {
                    FrameworkElement scrollViewerContent = (AxisX2.ScrollViewerElement.Children[0] as FrameworkElement);

                    Double scrollViewerOffset = (Chart as Chart).ZoomingEnabled ? offset * (scrollViewerContent.Width / ZOOMING_MAX_VAL) : offset;
                    PlotAreaScrollViewer.ScrollToHorizontalOffset(scrollViewerOffset);

                    scrollViewerContent.Margin = new Thickness(offset, 0, 0, 0);
                }




                SaveAxisContentOffsetAndResetMargin(AxisX2, offset);
                AxisX2.FireScrollEvent(e, offsetInPixel);
            }

            if (PlotDetails.ChartOrientation == ChartOrientationType.Horizontal)
            {   
                Double offset = e.NewValue;
#if SL
                AxisX2.ScrollBarElement.Maximum = ScrollableLength - PlotAreaScrollViewer.ViewportHeight;
                AxisX2.ScrollBarElement.ViewportSize = PlotAreaScrollViewer.ViewportHeight;
#else
                AxisX2.ScrollBarElement.Maximum = ScrollableLength - PlotAreaScrollViewer.ActualHeight;
                AxisX2.ScrollBarElement.ViewportSize = PlotAreaScrollViewer.ActualHeight;
                if (e.NewValue <= 1)
                    offset = e.NewValue * AxisX2.ScrollBarElement.Maximum;
#endif
                Double offsetInPixel = offset;

                // PlotAreaScrollViewer.ScrollToVerticalOffset(offset);
                
                // if (AxisX2.ScrollViewerElement.Children.Count > 0)
                //    (AxisX2.ScrollViewerElement.Children[0] as FrameworkElement).Margin = new Thickness(0, offset, 0, 0);

                if (AxisX2.ScrollViewerElement.Children.Count > 0)
                {
                    FrameworkElement scrollViewerContent = (AxisX2.ScrollViewerElement.Children[0] as FrameworkElement);

                    Double scrollViewerOffset = (Chart as Chart).ZoomingEnabled ? offset * (scrollViewerContent.Height / ZOOMING_MAX_VAL) : offset;
                    PlotAreaScrollViewer.ScrollToVerticalOffset(scrollViewerOffset);

                    scrollViewerContent.Margin = new Thickness(0, scrollViewerOffset, 0, 0);
                }

                
                SaveAxisContentOffsetAndResetMargin(AxisX2, offset);

                AxisX2.FireScrollEvent(e, offsetInPixel);
            }

            if (AxisX != null)
            {
                AxisX.ScrollBarElement.Value = e.NewValue;
            }
        }

        /// <summary>
        /// Render DataSeries to visual. 
        /// (Render each plotgroup from the plotgroup list of plotdetails)
        /// </summary>
        internal void RenderSeries()
        {
            Int32 renderedSeriesCount = 0;      // Contain count of series that have been already rendered

            // Contains a list of serties as per the drawing order generated in the plotdetails
            List<DataSeries> dataSeriesListInDrawingOrder = PlotDetails.SeriesDrawingIndex.Keys.ToList();

            List<DataSeries> selectedDataSeries4Rendering;          // Contains a list of serries to be rendered in a rendering cycle
            Int32 currentDrawingIndex;                              // Drawing index of the selected series 
            RenderAs currentRenderAs;                               // Rendereas type of the selected series
            Panel renderedChart = null;                                   // A canvas that contains the chart rendered using the selected series

            Int32 zIndex = 1;

            // This loop will select series for rendering and it will repeat until all series have been rendered
            while (renderedSeriesCount < Chart.InternalSeries.Count)
            {
                selectedDataSeries4Rendering = new List<DataSeries>();

                currentRenderAs = dataSeriesListInDrawingOrder[renderedSeriesCount].RenderAs;

                currentDrawingIndex = PlotDetails.SeriesDrawingIndex[dataSeriesListInDrawingOrder[renderedSeriesCount]];

                for (Int32 i = renderedSeriesCount; i < Chart.InternalSeries.Count; i++)
                {   
                    if (currentRenderAs == dataSeriesListInDrawingOrder[i].RenderAs && currentDrawingIndex == PlotDetails.SeriesDrawingIndex[dataSeriesListInDrawingOrder[i]])
                        selectedDataSeries4Rendering.Add(dataSeriesListInDrawingOrder[i]);
                }

                if (selectedDataSeries4Rendering.Count == 0)
                    break;

                Boolean isVisualExist = false;
                renderedChart = selectedDataSeries4Rendering[0].Visual as Panel;

                if (renderedChart == null && !(selectedDataSeries4Rendering[0].RenderAs == RenderAs.Area))
                {   
                    // Check  for pre existing series Visual
                    foreach (DataSeries ds in selectedDataSeries4Rendering)
                    {   
                        if (ds.Visual != null)
                        {
                            renderedChart = ds.Visual;
                            isVisualExist = true;
                            break;
                        }
                    }
                }
                else if(renderedChart == null)
                    isVisualExist = false;
                else
                    isVisualExist = true;

                // If froced redraw is true, its need to remove all preexisting canvas before we add the new visual canvas for the DataSeries
                if (Chart._forcedRedraw || (selectedDataSeries4Rendering[0].RenderAs == RenderAs.StackedArea || selectedDataSeries4Rendering[0].RenderAs == RenderAs.StackedArea100))
                {
                    // remove pre existing parent panel for the series visual 
                    if (renderedChart != null && renderedChart.Parent != null)
                    {
                        Panel parent = renderedChart.Parent as Panel;
                        parent.Children.Remove(renderedChart);
                    }

                    // Must set it to null. If renderedChart is set to null new visual canvas for the series will be created 
                    renderedChart = null;
                    isVisualExist = false;
                }
                
                renderedChart = RenderSeriesFromList(renderedChart, selectedDataSeries4Rendering);

                foreach (DataSeries ds in selectedDataSeries4Rendering)
                    ds.Visual = renderedChart;

                if (renderedChart != null && !isVisualExist)
                    ChartVisualCanvas.Children.Add(renderedChart);

                // renderedChart.Background = new SolidColorBrush(Colors.Yellow);
                renderedSeriesCount += selectedDataSeries4Rendering.Count;

                if (renderedChart != null)
                    renderedChart.SetValue(Canvas.ZIndexProperty, zIndex++);
            }

            ApplyOpacity();
            AttachEventsToolTipHref2DataSeries();
        }

        public Dictionary<RenderAs, Panel> RenderedCanvasList = new Dictionary<RenderAs, Panel>();

        /// <summary>
        /// Calls the appropriate chart rendering function to render the series available in the series list.
        /// Creates a layer as per the drawing index
        /// </summary>
        /// <param name="seriesListForRendering">List of selected dataseries</param>
        /// <returns>Canvas with rendered dataSeries visual</returns>
        internal Panel RenderSeriesFromList(Panel preExistingPanel, List<DataSeries> dataSeriesList4Rendering)
        {
            Panel renderedCanvas = null;

            renderedCanvas = RenderHelper.GetVisualObject(preExistingPanel, dataSeriesList4Rendering[0].RenderAs, ChartVisualCanvas.Width, ChartVisualCanvas.Height, PlotDetails, dataSeriesList4Rendering, Chart, PLANK_DEPTH, (Chart._internalAnimationEnabled && !_isAnimationFired));

           return renderedCanvas;
        }

        /// <summary>
        /// Animate chart grids
        /// </summary>
        /// <param name="axis">Axis</param>
        private void AnimateChartGrid(Axis axis)
        {   
            if (axis != null)
            {   
                foreach (ChartGrid chartGrid in axis.Grids)
                {
                    if (chartGrid.Storyboard != null)
                    {
#if WPF
                        chartGrid.Storyboard.Begin(Chart._rootElement, true);
#else
                        chartGrid.Storyboard.Begin();
#endif
                        chartGrid.Storyboard.Completed += delegate
                        {
                            _isAnimationFired = true;
                        };
                    }
                }
            }
        }

        /// <summary>
        /// Animate InternalDataPoints. 
        /// Begin the storyboard associated with each DataSeries
        /// </summary>
        private void Animate()
        {
            if (Chart._internalAnimationEnabled && !Chart.IsInDesignMode)
            {
                try
                {
                    if (PlotDetails.ChartOrientation != ChartOrientationType.NoAxis)
                    {
                        AnimateChartGrid(AxisX);
                        AnimateChartGrid(AxisY);
                        AnimateChartGrid(AxisY2);
                    }

                    Boolean isAnyActiveStoryboard = false;

                    foreach (DataSeries series in Chart.InternalSeries)
                    {
                        if (series.Storyboard != null)
                        {
                            if (series.InternalDataPoints.Count >= 1)
                                isAnyActiveStoryboard = true;

                            series.Storyboard.Completed += delegate(object sender, EventArgs e)
                            {   
                                _isAnimationFired = true;
                                Chart._rootElement.IsHitTestVisible = true;

                                if (PlotDetails.ChartOrientation == ChartOrientationType.NoAxis)
                                {
                                    foreach (DataPoint dataPoint in series.InternalDataPoints)
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

                                Visifire.Charts.Chart.SelectDataPoints(Chart);
                            };
#if WPF
                        if (PlotDetails.ChartOrientation == ChartOrientationType.NoAxis)
                        {
                            series.Storyboard.Completed += delegate(object sender, EventArgs e)
                            {
                                series.DetachOpacityPropertyFromAnimation();

                                foreach (DataPoint dataPoint in series.InternalDataPoints)
                                {
                                    if ((Boolean)dataPoint.Exploded && dataPoint.InternalYValue != 0)
                                        dataPoint.InteractiveAnimation(true);
                                }
                            };
                        }
                        
                        series.Storyboard.Begin(Chart._rootElement, true);
#else
                            if (PlotDetails.ChartOrientation == ChartOrientationType.NoAxis)
                            {
                                series.Storyboard.Completed += delegate(object sender, EventArgs e)
                                {
                                    _isAnimationFired = true;

                                    foreach (DataPoint dataPoint in series.InternalDataPoints)
                                    {
                                        if ((Boolean)dataPoint.Exploded && dataPoint.InternalYValue != 0)
                                            dataPoint.InteractiveAnimation(true);
                                    }
                                };
                            }

                            series.Storyboard.Stop();
                            series.Storyboard.Begin();
#endif
                        }


                    }

                    if (!isAnyActiveStoryboard)
                    {
                        Chart._rootElement.IsHitTestVisible = true;
                        _isAnimationFired = true;
                    }

                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine("Animation Error. " + e.Message);
                }
            }
            else
            {
                if (!Chart._internalAnimationEnabled && (Boolean)Chart.AnimatedUpdate)
                {
                    if (PlotDetails.ChartOrientation == ChartOrientationType.NoAxis)
                    {
                        foreach (DataPoint dataPoint in Chart.InternalSeries[0].InternalDataPoints)
                        {
                            if (dataPoint.Parent.RenderAs != RenderAs.SectionFunnel && dataPoint.Parent.RenderAs != RenderAs.StreamLineFunnel)
                            {
                                if ((Boolean)dataPoint.Exploded && dataPoint.InternalYValue != 0)
                                    dataPoint.ExplodeOrUnExplodeWithoutAnimation();
                            }
                        }
                    }
                }
            }

        }

        /// <summary>
        /// Set style of each dataseries from theme
        /// </summary>
        /// <param name="chart">Chart </param>
        private void SetSeriesStyleFromTheme()
        {
            if (Chart.Series != null)
            {
                foreach (DataSeries ds in Chart.Series)
                {
                    ds.IsNotificationEnable = false;
                    ds.ApplyStyleFromTheme(Chart, "DataSeries");
                    ds.IsNotificationEnable = true;
                    foreach (DataPoint dp in ds.DataPoints)
                    {
                        dp.IsNotificationEnable = false;
                        dp.ApplyStyleFromTheme(Chart, "DataPoint");
                        dp.IsNotificationEnable = true;
                    }
                }
            }
        }

        /// <summary>
        /// Set style of each title from theme
        /// </summary>
        /// <param name="chart"></param>
        private void SetTitleStyleFromTheme()
        {
            int titleIndex = 0;

            if (!String.IsNullOrEmpty(Chart.Theme))
            {
                foreach (Title title in Chart.Titles)
                {
                    if (titleIndex == 0)
                        title.ApplyStyleFromTheme(Chart, "MainTitle");
                    else
                        title.ApplyStyleFromTheme(Chart, "SubTitle");

                    titleIndex++;
                }
            }
        }

        /// <summary>
        /// Set style of each legend from theme
        /// </summary>
        private void SetLegendStyleFromTheme()
        {
            foreach (Legend legend in Chart.Legends)
            {
                if (!String.IsNullOrEmpty(Chart.Theme))
                {
                    legend.ApplyStyleFromTheme(Chart, "Legend");
                }
            }
        }

        ColorSet _chartColorSet;

        /// <summary>
        /// Set dataPoint colors from ColorSet
        /// </summary>
        /// <param name="Series">List of DataSeries</param>
        private void SetDataPointColorFromColorSet(System.Collections.Generic.IList<DataSeries> series)
        {
            ColorSet colorSet = null;

            if(_financialColorSet == null)
                _financialColorSet = Chart.GetColorSetByName("CandleLight");

            _financialColorSet.ResetIndex();

            // Load chart colorSet
            if (!String.IsNullOrEmpty(Chart.ColorSet))
            {
                colorSet = Chart.GetColorSetByName(Chart.ColorSet);
                _chartColorSet = colorSet;
            }

            if (series.Count == 1)
            {
                LoadSeriesColorSet4SingleSeries(series[0]);
            }
            else if (series.Count > 1)
            {
                if (_chartColorSet != null)
                    _chartColorSet.ResetIndex();

                foreach (DataSeries ds in series)
                {
                    LoadSeriesColorSet(ds);
                }
            }

            if (colorSet != null)
                colorSet.ResetIndex();
        }

        internal void LoadSeriesColorSet4SingleSeries(DataSeries dataSeries)
        {
            ColorSet colorSet = _chartColorSet;

            if (dataSeries.RenderAs == RenderAs.CandleStick)
            {
                if (dataSeries.PriceUpColor == null)
                {
                    dataSeries.IsNotificationEnable = false;
                    dataSeries.PriceUpColor = _financialColorSet.GetNewColorFromColorSet();
                    dataSeries.IsNotificationEnable = true;
                }
            }
            else
            {
                if (!String.IsNullOrEmpty(dataSeries.ColorSet))
                {
                    colorSet = Chart.GetColorSetByName(dataSeries.ColorSet);

                    if (colorSet == null)
                        throw new Exception("ColorSet named " + dataSeries.ColorSet + " is not found.");
                }
                else if (colorSet == null)
                {
                    throw new Exception("ColorSet named " + Chart.ColorSet + " is not found.");
                }

                Brush seriesColor = dataSeries.GetValue(DataSeries.ColorProperty) as Brush;

                if (!Chart.UniqueColors || dataSeries.RenderAs == RenderAs.Area || dataSeries.RenderAs == RenderAs.Line || dataSeries.RenderAs == RenderAs.StepLine || dataSeries.RenderAs == RenderAs.StackedArea || dataSeries.RenderAs == RenderAs.StackedArea100)
                {
                    if (seriesColor == null)
                        dataSeries._internalColor = colorSet.GetNewColorFromColorSet();
                    else
                        dataSeries._internalColor = seriesColor;

                    colorSet.ResetIndex();

                    foreach (DataPoint dp in dataSeries.DataPoints)
                    {
                        dp.IsNotificationEnable = false;

                        Brush dPColor = dp.GetValue(DataPoint.ColorProperty) as Brush;

                        if (dPColor == null)
                            if (!Chart.UniqueColors)
                                dp._internalColor = dataSeries._internalColor;
                            else if (seriesColor == null)
                                dp._internalColor = colorSet.GetNewColorFromColorSet();
                            else
                                dp._internalColor = seriesColor;
                        else
                            dp._internalColor = dPColor;

                        dp.IsNotificationEnable = true;
                    }
                }
                else
                {
                    dataSeries._internalColor = null;

                    foreach (DataPoint dp in dataSeries.DataPoints)
                    {
                        dp.IsNotificationEnable = false;

                        Brush dPColor = dp.GetValue(DataPoint.ColorProperty) as Brush;

                        if (dPColor == null)
                        {
                            if (seriesColor == null)
                                dp._internalColor = colorSet.GetNewColorFromColorSet();
                            else
                                dp._internalColor = seriesColor;
                        }
                        else
                            dp._internalColor = dPColor;

                        dp.IsNotificationEnable = true;
                    }
                }
            }
        }

        internal void LoadSeriesColorSet(DataSeries dataSeries)
        {
            ColorSet colorSet4MultiSeries = null;
            Boolean FLAG_UNIQUE_COLOR_4_EACH_DP = false; // Unique color for each DataPoint
            Brush seriesColor = null;
          

            if (dataSeries.RenderAs == RenderAs.CandleStick)
            {
                if (dataSeries.PriceUpColor == null)
                {
                    dataSeries.IsNotificationEnable = false;
                    dataSeries.PriceUpColor = _financialColorSet.GetNewColorFromColorSet();
                    dataSeries.IsNotificationEnable = true;
                }
            }
            else
            {
                colorSet4MultiSeries = _chartColorSet;
                FLAG_UNIQUE_COLOR_4_EACH_DP = false;

                if (!String.IsNullOrEmpty(dataSeries.ColorSet))
                {
                    colorSet4MultiSeries = Chart.GetColorSetByName(dataSeries.ColorSet);

                    if (colorSet4MultiSeries == null)
                        throw new Exception("ColorSet named " + dataSeries.ColorSet + " is not found.");

                    FLAG_UNIQUE_COLOR_4_EACH_DP = true;
                }
                else if (colorSet4MultiSeries == null)
                {
                    throw new Exception("ColorSet named " + Chart.ColorSet + " is not found.");
                }

                if (dataSeries.RenderAs == RenderAs.Area || dataSeries.RenderAs == RenderAs.StackedArea || dataSeries.RenderAs == RenderAs.StackedArea100)
                {
                    seriesColor = colorSet4MultiSeries.GetNewColorFromColorSet();

                    Brush DataSeriesColor = dataSeries.GetValue(DataSeries.ColorProperty) as Brush;

                    if (DataSeriesColor == null)
                    {
                        dataSeries._internalColor = seriesColor;
                    }
                    else
                        dataSeries._internalColor = DataSeriesColor;

                    foreach (DataPoint dp in dataSeries.DataPoints)
                    {   
                        dp.IsNotificationEnable = false;

                        Brush dPColor = dp.GetValue(DataPoint.ColorProperty) as Brush;

                        if (dPColor != null)
                            dp._internalColor = dPColor;

                        dp.IsNotificationEnable = true;
                    }
                }

                else
                {
                    if (!FLAG_UNIQUE_COLOR_4_EACH_DP || dataSeries.RenderAs == RenderAs.Line || dataSeries.RenderAs == RenderAs.StepLine)
                        seriesColor = colorSet4MultiSeries.GetNewColorFromColorSet();

                    if (dataSeries.RenderAs == RenderAs.Line || dataSeries.RenderAs == RenderAs.StepLine)
                        dataSeries._internalColor = seriesColor;

                    foreach (DataPoint dp in dataSeries.DataPoints)
                    {   
                        dp.IsNotificationEnable = false;
                        Brush dPColor = dp.GetValue(DataPoint.ColorProperty) as Brush;
                        
                        Brush DataSeriesColor = dataSeries.GetValue(DataSeries.ColorProperty) as Brush;

                        if (dPColor == null)
                        {   
                            // If unique color for each DataPoint
                            if (FLAG_UNIQUE_COLOR_4_EACH_DP)
                            {
                                if (DataSeriesColor == null)
                                {
                                    dp._internalColor = colorSet4MultiSeries.GetNewColorFromColorSet();

                                    if (dataSeries.RenderAs == RenderAs.Line || dataSeries.RenderAs == RenderAs.StepLine)
                                        dataSeries._internalColor = seriesColor;
                                    else
                                        dataSeries._internalColor = null;
                                }
                                else
                                    dataSeries._internalColor = DataSeriesColor;

                            }
                            else
                            {   
                                if (DataSeriesColor == null)
                                    dataSeries._internalColor = seriesColor;
                                else
                                    dataSeries._internalColor = DataSeriesColor;

                                dp.IsNotificationEnable = true;

                                break;
                            }
                        }
                        else
                            dp._internalColor = dPColor;

                        dp.IsNotificationEnable = true;
                    }
                }

                dataSeries.IsNotificationEnable = true;
            }
        }

        /// <summary>
        /// Add entries to a legend
        /// </summary>
        /// <param name="legend">Legend</param>
        /// <param name="dataPoints">List of InternalDataPoints</param>
        private void AddEntriesToLegend(Legend legend, List<DataPoint> dataPoints)
        {
            foreach (DataPoint dataPoint in dataPoints)
            {
                if (!(Boolean)dataPoint.Enabled || !(Boolean)dataPoint.ShowInLegend)
                    continue;

                if ((dataPoint.Parent.RenderAs == RenderAs.SectionFunnel || dataPoint.Parent.RenderAs == RenderAs.StreamLineFunnel) && dataPoint.InternalYValue < 0)
                    continue;

                String legendText = (String.IsNullOrEmpty(dataPoint.LegendText) ? dataPoint.Name : ObservableObject.GetFormattedMultilineText(dataPoint.LegendText));

                Brush markerColor = dataPoint._internalColor;

                if (dataPoint.Parent.RenderAs == RenderAs.CandleStick)
                {
                    markerColor = (Brush)dataPoint.GetValue(DataPoint.ColorProperty);

                    if (markerColor == null)
                    {
                        if (dataPoint.YValues != null)
                        {
                            if (dataPoint.YValues.Length >= 2)
                            {
                                Double openY = dataPoint.YValues[0];
                                Double closeY = dataPoint.YValues[1];

                                markerColor = (closeY > openY) ? dataPoint.Parent.PriceUpColor : dataPoint.Parent.PriceDownColor;
                            }
                        }
                    }
                    else
                        markerColor = dataPoint.Color;

                }

                if ((Boolean)dataPoint.LightingEnabled)
                {
                    if (dataPoint.Parent.RenderAs == RenderAs.Line || dataPoint.Parent.RenderAs == RenderAs.StepLine)
                        markerColor = Graphics.GetLightingEnabledBrush(markerColor, "Linear", new Double[] { 0.65, 0.55 });
                }

                Boolean markerBevel;
                if ((dataPoint.Parent as DataSeries).RenderAs == RenderAs.Point
                    || (dataPoint.Parent as DataSeries).RenderAs == RenderAs.Stock
                    || (dataPoint.Parent as DataSeries).RenderAs == RenderAs.CandleStick
                    || (dataPoint.Parent as DataSeries).RenderAs == RenderAs.Bubble
                    || (dataPoint.Parent as DataSeries).RenderAs == RenderAs.Pie || (dataPoint.Parent as DataSeries).RenderAs == RenderAs.Doughnut
                    || (dataPoint.Parent as DataSeries).RenderAs == RenderAs.Line
                    || (dataPoint.Parent as DataSeries).RenderAs == RenderAs.StepLine)
                {   
                    markerBevel = false;
                }
                else
                    markerBevel = Chart.View3D ? false : dataPoint.Parent.Bevel ? dataPoint.Parent.Bevel : false;

                Size markerSize;
                if (dataPoint.Parent.RenderAs == RenderAs.Line || dataPoint.Parent.RenderAs == RenderAs.StepLine)
                    markerSize = new Size(8, 8);
                else
                    markerSize = new Size(8, 8);

                dataPoint.LegendMarker = new Marker(
                        ((dataPoint.Parent.RenderAs == RenderAs.Line || dataPoint.Parent.RenderAs == RenderAs.StepLine) ? (MarkerTypes)dataPoint.MarkerType : RenderAsToMarkerType(dataPoint.Parent.RenderAs, dataPoint.Parent)),
                        1,
                        markerSize,
                        markerBevel,
                        markerColor,
                        ""
                        );

                if ((dataPoint.Parent.RenderAs == RenderAs.Line || dataPoint.Parent.RenderAs == RenderAs.StepLine || dataPoint.Parent.RenderAs == RenderAs.Stock || dataPoint.Parent.RenderAs == RenderAs.CandleStick) && dataPoint.MarkerEnabled == false)
                {
                    dataPoint.LegendMarker.Opacity = 0;
                }


                dataPoint.LegendMarker.DataSeriesOfLegendMarker = dataPoint.Parent;
                dataPoint.LegendMarker.Tag = new ElementData() { Element = dataPoint };

                legend.Entries.Add(new KeyValuePair<String, Marker>(legendText, dataPoint.LegendMarker));
            }

            if (legend != null && legend.Reversed)
                legend.Entries.Reverse();
        }

        /// <summary>
        /// Add Legends to ChartArea
        /// </summary>
        /// <param name="chart">Chart</param>
        /// <param name="DockInsidePlotArea">DockInsidePlotArea</param>
        /// <param name="Height">Ramaining height available for Legend</param>
        /// <param name="Width">Remaining width available for Legend</param>
        private void AddLegends(Chart chart, Boolean DockInsidePlotArea, Double Height, Double Width)
        {
            List<Legend> dockTest = (from legend in chart.Legends
                                     where legend.DockInsidePlotArea == DockInsidePlotArea
                                     select legend).ToList();

            if (dockTest.Count <= 0)
                return;

            if ((chart.InternalSeries.Count == 1 || (chart.InternalSeries[0].RenderAs == RenderAs.Pie || chart.InternalSeries[0].RenderAs == RenderAs.Doughnut || chart.InternalSeries[0].RenderAs == RenderAs.SectionFunnel || chart.InternalSeries[0].RenderAs == RenderAs.StreamLineFunnel)) && (Boolean)chart.InternalSeries[0].Enabled)
            {
                Legend legend = null;
                foreach (Legend entry in chart.Legends)
                    entry.Entries.Clear();

                // if (chart.Legends.Count > 0 && (!String.IsNullOrEmpty(chart.InternalSeries[0].Legend) || !String.IsNullOrEmpty(chart.InternalSeries[0].InternalLegendName)))
                if (chart.Legends.Count > 0)
                {

                    var legends = (from entry in chart.Legends
                                   where
                                   (entry.Name == chart.InternalSeries[0].Legend && entry.DockInsidePlotArea == DockInsidePlotArea)
                                   select entry);

                    if (legends.Count() > 0)
                        legend = (legends).First();

                }

                if (legend == null)
                    return;

                AddEntriesToLegend(legend, chart.InternalSeries[0].InternalDataPoints.ToList());
            }
            else
            {
                List<DataSeries> seriesToBeShownInLegend =
                    (from entry in chart.InternalSeries
                     where entry.ShowInLegend == true && entry.Enabled == true
                     select entry).ToList();

                if (seriesToBeShownInLegend.Count > 0)
                {
                    Legend legend = null;
                    foreach (Legend entry in chart.Legends)
                        entry.Entries.Clear();

                    foreach (DataSeries dataSeries in seriesToBeShownInLegend)
                    {
                        // if (chart.Legends.Count > 0 && (!String.IsNullOrEmpty(dataSeries.Legend)
                        // || !String.IsNullOrEmpty(dataSeries.InternalLegendName)))
                        if (chart.Legends.Count > 0)
                        {
                            legend = null;
                            var legends = from entry in chart.Legends
                                          where (
                                          entry.Name == dataSeries.Legend
                                              // entry.Name == dataSeries.Legend
                                              // || entry.Name == dataSeries.InternalLegendName
                                          )
                                          && entry.DockInsidePlotArea == DockInsidePlotArea
                                          select entry;

                            if (legends.Count() > 0)
                                legend = (legends).First();
                        }

                        if (legend == null)
                        {
                            continue;
                            // throw new Exception("Legend name is not specified in DataSeries..");
                        }

                        String legendText;

                        if (String.IsNullOrEmpty(dataSeries.LegendText))
                        {
                            if (dataSeries._isAutoName)
                            {
                                String[] s = dataSeries.Name.Split('_');
                                legendText = s[0];
                            }
                            else
                                legendText = dataSeries.Name;
                        }
                        else
                            legendText = ObservableObject.GetFormattedMultilineText(dataSeries.LegendText);

                        Brush markerColor;

                        if (dataSeries.RenderAs == RenderAs.CandleStick)
                        {
                            markerColor = dataSeries.PriceUpColor;
                        }
                        else
                        {
                            markerColor = dataSeries.Color;
                        }

                        if (dataSeries.InternalDataPoints.Count > 0)
                        {
                            DataPoint dataPoint = dataSeries.InternalDataPoints[0];
                            markerColor = markerColor ?? dataPoint.Color;
                        }

                        if ((Boolean)dataSeries.LightingEnabled)
                        {
                            if (dataSeries.RenderAs == RenderAs.Line || dataSeries.RenderAs == RenderAs.StepLine)
                                markerColor = Graphics.GetLightingEnabledBrush(markerColor, "Linear", new Double[] { 0.65, 0.55 });
                        }

                        Boolean markerBevel;

                        if (dataSeries.RenderAs == RenderAs.Point
                            || dataSeries.RenderAs == RenderAs.Stock
                            || dataSeries.RenderAs == RenderAs.CandleStick
                            || dataSeries.RenderAs == RenderAs.Bubble
                            || dataSeries.RenderAs == RenderAs.Line
                            || dataSeries.RenderAs == RenderAs.StepLine)
                        {
                            markerBevel = false;
                        }
                        else
                            markerBevel = Chart.View3D ? false : dataSeries.Bevel ? dataSeries.Bevel : false;

                        Size markerSize;
                        if (dataSeries.RenderAs == RenderAs.Line || dataSeries.RenderAs == RenderAs.StepLine)
                        {
                            markerSize = new Size(8, 8);
                        }
                        else
                            markerSize = new Size(8, 8);

                        dataSeries.LegendMarker = new Marker(
                                RenderAsToMarkerType(dataSeries.RenderAs, dataSeries),
                                1,
                                markerSize,
                                markerBevel,
                                markerColor,
                                ""
                                );

                        dataSeries.LegendMarker.DataSeriesOfLegendMarker = dataSeries;

                        if ((dataSeries.RenderAs == RenderAs.Line || dataSeries.RenderAs == RenderAs.StepLine || dataSeries.RenderAs == RenderAs.Stock || dataSeries.RenderAs == RenderAs.CandleStick) && dataSeries.MarkerEnabled == false)
                            dataSeries.LegendMarker.Opacity = 0;

                        dataSeries.LegendMarker.Tag = new ElementData() { Element = dataSeries };
                        legend.Entries.Add(new KeyValuePair<String, Marker>(legendText, dataSeries.LegendMarker));
                    }

                    if (legend != null && legend.Reversed)
                        legend.Entries.Reverse();
                }
            }

            StackPanel topLegendPanel;
            StackPanel bottomLegendPanel;
            StackPanel leftLegendPanel;
            StackPanel rightLegendPanel;
            StackPanel centerPanel;

            if (DockInsidePlotArea)
            {
                topLegendPanel = chart._topInnerLegendPanel;
                bottomLegendPanel = chart._bottomInnerLegendPanel;
                leftLegendPanel = chart._leftInnerLegendPanel;
                rightLegendPanel = chart._rightInnerLegendPanel;
                centerPanel = chart._centerDockInsidePlotAreaPanel;
            }
            else
            {
                topLegendPanel = chart._topOuterLegendPanel;
                bottomLegendPanel = chart._bottomOuterLegendPanel;
                leftLegendPanel = chart._leftOuterLegendPanel;
                rightLegendPanel = chart._rightOuterLegendPanel;
                centerPanel = chart._centerDockOutsidePlotAreaPanel;
            }

            List<Legend> legendsOnTop = (from entry in chart.Legends
                                         where entry.Entries.Count > 0 && entry.InternalVerticalAlignment == VerticalAlignment.Top
                                         && entry.DockInsidePlotArea == DockInsidePlotArea
                                         && (Boolean)entry.Enabled
                                         select entry).ToList();

            List<Legend> legendsOnBottom = (from entry in chart.Legends
                                            where entry.Entries.Count > 0
                                            && entry.InternalVerticalAlignment == VerticalAlignment.Bottom
                                            && entry.DockInsidePlotArea == DockInsidePlotArea && (Boolean)entry.Enabled
                                            select entry).ToList();

            List<Legend> legendsOnLeft = (from entry in chart.Legends
                                          where entry.Entries.Count > 0
                                          && (entry.InternalVerticalAlignment == VerticalAlignment.Center ||
                                          entry.InternalVerticalAlignment == VerticalAlignment.Stretch)
                                          && entry.InternalHorizontalAlignment == HorizontalAlignment.Left
                                          && entry.DockInsidePlotArea == DockInsidePlotArea && (Boolean)entry.Enabled
                                          select entry).ToList();

            List<Legend> legendsOnRight = (from entry in chart.Legends
                                           where entry.Entries.Count > 0
                                           && (entry.InternalVerticalAlignment == VerticalAlignment.Center ||
                                           entry.InternalVerticalAlignment == VerticalAlignment.Stretch)
                                           && entry.InternalHorizontalAlignment == HorizontalAlignment.Right
                                           && entry.DockInsidePlotArea == DockInsidePlotArea
                                           && (Boolean)entry.Enabled
                                           select entry).ToList();

            List<Legend> legendsAtCenter = (from entry in chart.Legends
                                            where entry.Entries.Count > 0
                                            && (entry.InternalVerticalAlignment == VerticalAlignment.Center ||
                                            entry.InternalVerticalAlignment == VerticalAlignment.Stretch)
                                            && (entry.InternalHorizontalAlignment == HorizontalAlignment.Center ||
                                            entry.InternalHorizontalAlignment == HorizontalAlignment.Stretch)
                                            && entry.DockInsidePlotArea == DockInsidePlotArea
                                            && (Boolean)entry.Enabled
                                            select entry).ToList();

            if (legendsOnTop.Count > 0)
            {
                foreach (Legend legend in legendsOnTop)
                {
                    legend.Orientation = Orientation.Horizontal;
                    legend.LegendLayout = Layouts.FlowLayout;

                    if (!Double.IsNaN(Width) && Width > 0)
                    {
                        if (Double.IsPositiveInfinity(legend.InternalMaxWidth))
                            legend.InternalMaximumWidth = Width - Chart.BorderThickness.Left - Chart.BorderThickness.Right - chart.Padding.Left - chart.Padding.Right;
                        else
                        {
                            if (legend.InternalMaxWidth > Width - Chart.BorderThickness.Left - Chart.BorderThickness.Right - chart.Padding.Left - chart.Padding.Right)
                                legend.InternalMaximumWidth = Width - Chart.BorderThickness.Left - Chart.BorderThickness.Right - chart.Padding.Left - chart.Padding.Right;
                            else
                                legend.InternalMaximumWidth = legend.InternalMaxWidth;
                        }

                        if (Double.IsPositiveInfinity(legend.InternalMaxHeight))
                            legend.InternalMaximumHeight = Double.PositiveInfinity;
                        else
                        {
                            if (legend.InternalMaxHeight > Height - Chart.BorderThickness.Top - Chart.BorderThickness.Bottom - chart.Padding.Top - chart.Padding.Bottom)
                                legend.InternalMaximumHeight = Height - Chart.BorderThickness.Top - Chart.BorderThickness.Bottom - chart.Padding.Top - chart.Padding.Bottom;
                            else
                                legend.InternalMaximumHeight = legend.InternalMaxHeight;
                        }
                    }

                    legend.CreateVisualObject();

                    if (legend.Visual != null)
                        topLegendPanel.Children.Add(legend.Visual);
                }
            }


            if (legendsOnBottom.Count > 0)
            {
                legendsOnBottom.Reverse();
                foreach (Legend legend in legendsOnBottom)
                {
                    legend.Orientation = Orientation.Horizontal;
                    legend.LegendLayout = Layouts.FlowLayout;

                    if ((!Double.IsNaN(Width) && Width > 0) && (!Double.IsNaN(Height) && Height > 0))
                    {
                        if (Double.IsPositiveInfinity(legend.InternalMaxWidth))
                            legend.InternalMaximumWidth = Width - Chart.BorderThickness.Left - Chart.BorderThickness.Right - chart.Padding.Left - chart.Padding.Right;
                        else
                        {
                            if (legend.InternalMaxWidth > Width - Chart.BorderThickness.Left - Chart.BorderThickness.Right - chart.Padding.Left - chart.Padding.Right)
                                legend.InternalMaximumWidth = Width - Chart.BorderThickness.Left - Chart.BorderThickness.Right - chart.Padding.Left - chart.Padding.Right;
                            else
                                legend.InternalMaximumWidth = legend.InternalMaxWidth;
                        }

                        if (Double.IsPositiveInfinity(legend.InternalMaxHeight))
                            legend.InternalMaximumHeight = Double.PositiveInfinity;
                        else
                        {
                            if (legend.InternalMaxHeight > Height - Chart.BorderThickness.Top - Chart.BorderThickness.Bottom - chart.Padding.Top - chart.Padding.Bottom)
                                legend.InternalMaximumHeight = Height - Chart.BorderThickness.Top - Chart.BorderThickness.Bottom - chart.Padding.Top - chart.Padding.Bottom;
                            else
                                legend.InternalMaximumHeight = legend.InternalMaxHeight;
                        }
                    }

                    legend.CreateVisualObject();
                    if (legend.Visual != null)
                        bottomLegendPanel.Children.Add(legend.Visual);
                }
            }

            if (legendsOnLeft.Count > 0)
            {
                foreach (Legend legend in legendsOnLeft)
                {
                    legend.Orientation = Orientation.Vertical;
                    legend.LegendLayout = Layouts.FlowLayout;

                    if ((!Double.IsNaN(Width) && Width > 0) && (!Double.IsNaN(Height) && Height > 0))
                    {
                        if (Double.IsPositiveInfinity(legend.InternalMaxHeight))
                            legend.InternalMaximumHeight = Height - Chart.BorderThickness.Top - Chart.BorderThickness.Bottom - chart.Padding.Top - chart.Padding.Bottom;
                        else
                        {
                            if (legend.InternalMaxHeight > Height - Chart.BorderThickness.Top - Chart.BorderThickness.Bottom - chart.Padding.Top - chart.Padding.Bottom)
                                legend.InternalMaximumHeight = Height - Chart.BorderThickness.Top - Chart.BorderThickness.Bottom - chart.Padding.Top - chart.Padding.Bottom;
                            else
                                legend.InternalMaximumHeight = legend.InternalMaxHeight;
                        }

                        if (Double.IsPositiveInfinity(legend.InternalMaxWidth))
                            legend.InternalMaximumWidth = Double.PositiveInfinity;
                        else
                        {
                            if (legend.InternalMaxWidth > Width - Chart.BorderThickness.Left - Chart.BorderThickness.Right - chart.Padding.Left - chart.Padding.Right)
                                legend.InternalMaximumWidth = Width - Chart.BorderThickness.Left - Chart.BorderThickness.Right - chart.Padding.Left - chart.Padding.Right;
                            else
                                legend.InternalMaximumWidth = legend.InternalMaxWidth;
                        }
                    }

                    legend.CreateVisualObject();
                    if (legend.Visual != null)
                        leftLegendPanel.Children.Add(legend.Visual);

                }
            }

            if (legendsOnRight.Count > 0)
            {
                legendsOnRight.Reverse();
                foreach (Legend legend in legendsOnRight)
                {
                    legend.Orientation = Orientation.Vertical;
                    legend.LegendLayout = Layouts.FlowLayout;

                    if (!Double.IsNaN(Height) && Height > 0)
                    {
                        if (Double.IsPositiveInfinity(legend.InternalMaxHeight))
                            legend.InternalMaximumHeight = Height - Chart.BorderThickness.Top - Chart.BorderThickness.Bottom - chart.Padding.Top - chart.Padding.Bottom;
                        else
                        {
                            if (legend.InternalMaxHeight > Height - Chart.BorderThickness.Top - Chart.BorderThickness.Bottom - chart.Padding.Top - chart.Padding.Bottom)
                                legend.InternalMaximumHeight = Height - Chart.BorderThickness.Top - Chart.BorderThickness.Bottom - chart.Padding.Top - chart.Padding.Bottom;
                            else
                                legend.InternalMaximumHeight = legend.InternalMaxHeight;
                        }

                        if (Double.IsPositiveInfinity(legend.InternalMaxWidth))
                            legend.InternalMaximumWidth = Double.PositiveInfinity;
                        else
                        {
                            if (legend.InternalMaxWidth > Width - Chart.BorderThickness.Left - Chart.BorderThickness.Right - chart.Padding.Left - chart.Padding.Right)
                                legend.InternalMaximumWidth = Width - Chart.BorderThickness.Left - Chart.BorderThickness.Right - chart.Padding.Left - chart.Padding.Right;
                            else
                                legend.InternalMaximumWidth = legend.InternalMaxWidth;
                        }
                    }

                    legend.CreateVisualObject();
                    if (legend.Visual != null)
                        rightLegendPanel.Children.Add(legend.Visual);
                }
            }

            if (legendsAtCenter.Count > 0)
            {
                foreach (Legend legend in legendsAtCenter)
                {
                    legend.Orientation = Orientation.Horizontal;
                    legend.LegendLayout = Layouts.FlowLayout;

                    if (Double.IsPositiveInfinity(legend.InternalMaxWidth)) // legend.MaximumWidth == 0
                        legend.InternalMaximumWidth = Width * 60 / 100;
                    else
                    {
                        if (legend.InternalMaxWidth > Width * 60 / 100)
                            legend.InternalMaximumWidth = Width * 60 / 100;
                        else
                            legend.InternalMaximumWidth = legend.InternalMaxWidth;
                    }

                    legend.CreateVisualObject();

                    if (legend.Visual != null)
                        centerPanel.Children.Add(legend.Visual);
                }
            }
        }

        /// <summary>
        /// Add titles to ChartArea
        /// </summary>
        /// <param name="chart">Chart</param>
        /// <param name="DockInsidePlotArea">DockInsidePlotArea</param>
        /// <param name="height">Height avilable for title</param>
        /// <param name="width">Width available for title</param>
        /// <param name="isHLeftOrRightVCenterTitlesExists">Whether horizontal or vertical titles exists</param>
        private void AddTitles(Chart chart, Boolean DockInsidePlotArea, Double height, Double width, out Boolean isHLeftOrRightVCenterTitlesExists)
        {
            IList<Title> titles;
            StackPanel topTitlePanel = null;
            StackPanel bottomTitlePanel = null;
            StackPanel leftTitlePanel = null;
            StackPanel rightTitlePanel = null;
            StackPanel centerPanel = null;
            isHLeftOrRightVCenterTitlesExists = false;

            if (DockInsidePlotArea)
            {
                // Get the Titles docked outside PlotArea 
                titles = chart.GetTitlesDockedInsidePlotArea();
                topTitlePanel = chart._topInnerTitlePanel;
                bottomTitlePanel = chart._bottomInnerTitlePanel;
                leftTitlePanel = chart._leftInnerTitlePanel;
                rightTitlePanel = chart._rightInnerTitlePanel;
                centerPanel = chart._centerDockInsidePlotAreaPanel;
            }
            else
            {
                // Get the Titles docked outside PlotArea 
                titles = chart.GetTitlesDockedOutSidePlotArea();
                topTitlePanel = chart._topOuterTitlePanel;
                bottomTitlePanel = chart._bottomOuterTitlePanel;
                leftTitlePanel = chart._leftOuterTitlePanel;
                rightTitlePanel = chart._rightOuterTitlePanel;
                centerPanel = chart._centerDockOutsidePlotAreaPanel;
            }    

            if (titles.Count == 0)
                return;

            // Get Titles on the top of the ChartArea using LINQ
            var titlesOnTop = from title in titles
                              where (title.InternalVerticalAlignment == VerticalAlignment.Top && title.Enabled == true)
                              select title;

            // Add Title on the top of the ChartArea
            foreach (Title title in titlesOnTop)
                this.AddTitle(chart, title, topTitlePanel, width, height);

            // Get Titles on the bottom of the ChartArea using LINQ
            var titlesOnBottom = from title in titles
                                 where (title.InternalVerticalAlignment == VerticalAlignment.Bottom && title.Enabled == true)
                                 select title;

            titlesOnBottom.Reverse();

            // Add Title on the bottom of the ChartArea
            foreach (Title title in titlesOnBottom)
                this.AddTitle(chart, title, bottomTitlePanel, width, height);

            // Get Titles on the left of the ChartArea using LINQ
            var titlesAtLeft = from title in titles
                               where ((title.InternalVerticalAlignment == VerticalAlignment.Center || title.InternalVerticalAlignment == VerticalAlignment.Stretch)
                               && title.InternalHorizontalAlignment == HorizontalAlignment.Left
                               && title.Enabled == true)
                               select title;

            if (titlesAtLeft.Count() > 0)
                isHLeftOrRightVCenterTitlesExists = true;

            // Add Title on left of the ChartArea
            foreach (Title title in titlesAtLeft)
                this.AddTitle(chart, title, leftTitlePanel, width, height);


            // Get Titles on the right of the ChartArea using LINQ
            var titlesAtRight = from title in titles
                                where ((title.InternalVerticalAlignment == VerticalAlignment.Center || title.InternalVerticalAlignment == VerticalAlignment.Stretch)
                                && title.InternalHorizontalAlignment == HorizontalAlignment.Right
                                && title.Enabled == true)
                                select title;

            if (titlesAtRight.Count() > 0)
                isHLeftOrRightVCenterTitlesExists = true;

            titlesAtRight.Reverse();

            // Add Title on the right of the ChartArea
            foreach (Title title in titlesAtRight)
                this.AddTitle(chart, title, rightTitlePanel, width, height);

            // Get Titles on the right of the ChartArea using LINQ
            var titlesOnCenter = from title in titles
                                 where ((title.InternalHorizontalAlignment == HorizontalAlignment.Center || title.InternalHorizontalAlignment == HorizontalAlignment.Stretch)
                                 && (title.InternalVerticalAlignment == VerticalAlignment.Center || title.InternalVerticalAlignment == VerticalAlignment.Stretch)
                                 && title.Enabled == true)
                                 select title;

            // Add Title on the right of the ChartArea
            centerPanel.Children.Clear();

            foreach (Title title in titlesOnCenter)
                this.AddTitle(chart, title, centerPanel, width, height);
        }
        
        /// <summary>
        /// Get top margin of centergrid inside the ChartArea 
        /// </summary>
        /// <returns>Double value</returns>
        private Double GetChartAreaCenterGridTopMargin()
        {   
            Double overflow = 0;
            if (AxisY != null && PlotDetails.ChartOrientation == ChartOrientationType.Vertical)
                overflow = Math.Max(overflow, AxisY.AxisLabels.TopOverflow);
            if (AxisY2 != null && PlotDetails.ChartOrientation == ChartOrientationType.Vertical)
                overflow = Math.Max(overflow, AxisY2.AxisLabels.TopOverflow);
            if (AxisX != null && PlotDetails.ChartOrientation == ChartOrientationType.Horizontal)
                overflow = Math.Max(overflow, AxisX.AxisLabels.TopOverflow);
            if (AxisX2 != null && PlotDetails.ChartOrientation == ChartOrientationType.Horizontal)
                overflow = Math.Max(overflow, AxisX2.AxisLabels.TopOverflow);

            if (AxisX2 != null && PlotDetails.ChartOrientation == ChartOrientationType.Vertical)
            {
                AxisX2.Visual.Measure(new Size(Double.MaxValue, Double.MaxValue));
                if (AxisX2.Visual.DesiredSize.Height < overflow)
                    return Math.Abs(overflow - AxisX2.Visual.DesiredSize.Height);
                else
                    return 0;
            }
            else if (AxisY2 != null && PlotDetails.ChartOrientation == ChartOrientationType.Horizontal)
            {
                AxisY2.Visual.Measure(new Size(Double.MaxValue, Double.MaxValue));
                if (AxisY2.Visual.DesiredSize.Height < overflow)
                    return Math.Abs(overflow - AxisY2.Visual.DesiredSize.Height);
                else
                    return 0;
            }
            else
            {
                return overflow;
            }
        }

        /// <summary>
        /// Get bottom margin of centergrid inside the ChartArea 
        /// </summary>
        /// <returns>Double value</returns>
        private Double GetChartAreaCenterGridBottomMargin()
        {   
            Double overflow = 0;
            if (AxisY != null && PlotDetails.ChartOrientation == ChartOrientationType.Vertical)
                overflow = Math.Max(overflow, AxisY.AxisLabels.BottomOverflow);
            if (AxisY2 != null && PlotDetails.ChartOrientation == ChartOrientationType.Vertical)
                overflow = Math.Max(overflow, AxisY2.AxisLabels.BottomOverflow);
            if (AxisX != null && PlotDetails.ChartOrientation == ChartOrientationType.Horizontal)
                overflow = Math.Max(overflow, AxisX.AxisLabels.BottomOverflow);
            if (AxisX2 != null && PlotDetails.ChartOrientation == ChartOrientationType.Horizontal)
                overflow = Math.Max(overflow, AxisX2.AxisLabels.BottomOverflow);

            if (AxisX != null && PlotDetails.ChartOrientation == ChartOrientationType.Vertical)
            {
                AxisX.Visual.Measure(new Size(Double.MaxValue, Double.MaxValue));
                if (AxisX.Visual.DesiredSize.Height < overflow)
                    return Math.Abs(overflow - AxisX.Visual.DesiredSize.Height);
                else
                    return 0;
            }
            else if (AxisY != null && PlotDetails.ChartOrientation == ChartOrientationType.Horizontal)
            {
                AxisY.Visual.Measure(new Size(Double.MaxValue, Double.MaxValue));
                if (AxisY.Visual.DesiredSize.Height < overflow)
                    return Math.Abs(overflow - AxisY.Visual.DesiredSize.Height);
                else
                    return 0;
            }
            else
            {
                return overflow;
            }
        }

        /// <summary>
        /// Get right margin of centergrid inside the ChartArea 
        /// </summary>
        /// <returns>Double value</returns>
        private Double GetChartAreaCenterGridRightMargin()
        {
            Double overflow = 0;
            if (AxisX != null && PlotDetails.ChartOrientation == ChartOrientationType.Vertical)
                overflow = Math.Max(overflow, AxisX.AxisLabels.RightOverflow);
            if (AxisX2 != null && PlotDetails.ChartOrientation == ChartOrientationType.Vertical)
                overflow = Math.Max(overflow, AxisX2.AxisLabels.RightOverflow);
            if (AxisY != null && PlotDetails.ChartOrientation == ChartOrientationType.Horizontal)
                overflow = Math.Max(overflow, AxisY.AxisLabels.RightOverflow);
            if (AxisY2 != null && PlotDetails.ChartOrientation == ChartOrientationType.Horizontal)
                overflow = Math.Max(overflow, AxisY2.AxisLabels.RightOverflow);

            if (AxisY2 != null && PlotDetails.ChartOrientation == ChartOrientationType.Vertical)
            {
                AxisY2.Visual.Measure(new Size(Double.MaxValue, Double.MaxValue));
                if (AxisY2.Visual.DesiredSize.Width < overflow)
                    return Math.Abs(overflow - AxisY2.Visual.DesiredSize.Width);
                else
                    return 0;
            }
            else if (AxisX2 != null && PlotDetails.ChartOrientation == ChartOrientationType.Horizontal)
            {
                AxisX2.Visual.Measure(new Size(Double.MaxValue, Double.MaxValue));
                if (AxisX2.Visual.DesiredSize.Width < overflow)
                    return Math.Abs(overflow - AxisX2.Visual.DesiredSize.Width);
                else
                    return 0;
            }
            else
            {
                return overflow;
            }
        }

        /// <summary>
        /// Get left margin of centergrid inside the ChartArea 
        /// </summary>
        /// <returns>Double value</returns>
        private Double GetChartAreaCenterGridLeftMargin()
        {
            Double overflow = 0;
            if (AxisX != null && PlotDetails.ChartOrientation == ChartOrientationType.Vertical)
                overflow = Math.Max(overflow, AxisX.AxisLabels.LeftOverflow);
            if (AxisX2 != null && PlotDetails.ChartOrientation == ChartOrientationType.Vertical)
                overflow = Math.Max(overflow, AxisX2.AxisLabels.LeftOverflow);
            if (AxisY != null && PlotDetails.ChartOrientation == ChartOrientationType.Horizontal)
                overflow = Math.Max(overflow, AxisY.AxisLabels.LeftOverflow);
            if (AxisY2 != null && PlotDetails.ChartOrientation == ChartOrientationType.Horizontal)
                overflow = Math.Max(overflow, AxisY2.AxisLabels.LeftOverflow);

            if (AxisY != null && PlotDetails.ChartOrientation == ChartOrientationType.Vertical)
            {
                AxisY.Visual.Measure(new Size(Double.MaxValue, Double.MaxValue));
                if (AxisY.Visual.DesiredSize.Width < overflow)
                    return Math.Abs(overflow - AxisY.Visual.DesiredSize.Width);
                else
                    return 0;
            }
            else if (AxisX != null && PlotDetails.ChartOrientation == ChartOrientationType.Horizontal)
            {
                AxisX.Visual.Measure(new Size(Double.MaxValue, Double.MaxValue));
                if (AxisX.Visual.DesiredSize.Width < overflow)
                    return Math.Abs(overflow - AxisX.Visual.DesiredSize.Width);
                else
                    return 0;
            }
            else
            {
                return overflow;
            }
        }

        /// <summary>
        /// Set center grid margin of the ChartArea
        /// </summary>
        /// <param name="NewSize">NewSize</param>
        /// <returns>NewSize</returns>
        private Size SetChartAreaCenterGridMargin(Size newSize, ref Double left, ref Double bottom, ref Double right, ref Double top)
        {
            newSize.Height += top;
            newSize.Height += bottom;
            newSize.Width += left;
            newSize.Width += right;

            left = GetChartAreaCenterGridLeftMargin();
            right = GetChartAreaCenterGridRightMargin() + ((Chart.PlotArea.ShadowEnabled) ? Chart.SHADOW_DEPTH : 0);
            top = GetChartAreaCenterGridTopMargin();
            bottom = GetChartAreaCenterGridBottomMargin();

            Chart._topOffsetGrid.Height = top;
            Chart._bottomOffsetGrid.Height = bottom;
            Chart._rightOffsetGrid.Width = right;
            Chart._leftOffsetGrid.Width = left;

            newSize.Height -= top;
            newSize.Height -= bottom;

            newSize.Width -= left;
            newSize.Width -= right;

            return newSize;
        }

        /// <summary>
        /// Set properties of axis 
        /// </summary>
        /// <param name="axis">Axis</param>
        /// <param name="startOffset">StartOffset value</param>
        /// <param name="endOffset">EndOffset value</param>
        private void SetAxisProperties(Axis axis, Double startOffset, Double endOffset)
        {
            if (axis != null)
            {
                axis.ApplyStyleFromTheme(Chart, axis.AxisRepresentation.ToString());
                axis.StartOffset = startOffset;
                axis.EndOffset = endOffset;
                axis.SetScrollBar();
            }
        }

        /// <summary>
        /// Set properties of specific axes
        /// </summary>
        private void SetAxesProperties()
        {
            AxisX = PlotDetails.GetAxisXFromChart(Chart, AxisTypes.Primary);
            AxisX2 = PlotDetails.GetAxisXFromChart(Chart, AxisTypes.Secondary);

            Axis oldAxisY = AxisY;
            AxisY = PlotDetails.GetAxisYFromChart(Chart, AxisTypes.Primary);

            if (oldAxisY != null && AxisY == null)
                CleanUpGrids(oldAxisY);

            oldAxisY = AxisY2;
            AxisY2 = PlotDetails.GetAxisYFromChart(Chart, AxisTypes.Secondary);

            if (oldAxisY != null && AxisY2 == null)
                CleanUpGrids(oldAxisY);

            if (AxisX != null)
            {   
                if (PlotDetails.ChartOrientation == ChartOrientationType.Vertical)
                {
                    SetAxisProperties(AxisX, 0, PLANK_DEPTH);
                    SetAxisProperties(AxisX2, PLANK_DEPTH, 0);
                    SetAxisProperties(AxisY, PLANK_DEPTH, PLANK_THICKNESS);
                    SetAxisProperties(AxisY2, 0, PLANK_OFFSET);
                }
                else
                {
                    SetAxisProperties(AxisX, PLANK_DEPTH, 0);
                    SetAxisProperties(AxisX2, PLANK_DEPTH, 0);
                    SetAxisProperties(AxisY, PLANK_OFFSET, 0);
                    SetAxisProperties(AxisY2, PLANK_OFFSET, 0);
                }
            }
        }

        /// <summary>
        /// Apply Opacity to DataSeries and InternalDataPoints visual
        /// </summary>
        private void ApplyOpacity()
        {
            foreach (DataSeries ds in Chart.InternalSeries)
            {
                switch (ds.RenderAs)
                {
                    case RenderAs.StackedArea:
                    case RenderAs.StackedArea100:
                    case RenderAs.Pie:
                    case RenderAs.Doughnut:
                    case RenderAs.SectionFunnel:
                    case RenderAs.StreamLineFunnel:

                        if (ds.Faces != null)
                        {
                            ds.Faces.Visual.Opacity = ds.Opacity;
                        }

                        foreach (DataPoint dp in ds.InternalDataPoints)
                        {
                            if (dp.Faces != null)
                            {
                                if (Chart.AnimationEnabled == false || (Chart.AnimationEnabled && !_isFirstTimeRender))
                                {
                                    if (dp.Faces.VisualComponents.Count != 0)
                                    {
                                        foreach (FrameworkElement face in dp.Faces.VisualComponents)
                                        {
                                            face.Opacity = dp.Opacity * ds.Opacity;
                                        }
                                    }
                                    else if (dp.Faces.Visual != null)
                                        dp.Faces.Visual.Opacity = ds.Opacity * dp.Opacity;
                                }
                                else if (dp.Faces.Visual != null)
                                    dp.Faces.Visual.Opacity = ds.Opacity * dp.Opacity;
                            }


                            if (Chart.AnimationEnabled == false || (Chart.AnimationEnabled && !_isFirstTimeRender))
                            {
                                if (dp.Marker != null && dp.Marker.Visual != null)
                                    dp.Marker.Visual.Opacity = ds.Opacity * dp.Opacity;
                            }
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// The function checks for default interactivity. If events are attached to any DataPoint
        /// or DataSeries for Pie/Doughnut/Funnel chart, default interactivity is not allowed.
        /// Means if events are attached to any DataPoint or DataSeries then on click of DataPoint,
        /// it won't explode.
        /// </summary>
        /// <param name="ds">DataSeries</param>
        private void Check4DefaultInteractivity(DataSeries ds)
        {
            if (ds.RenderAs == RenderAs.Pie || ds.RenderAs == RenderAs.Doughnut || ds.RenderAs == RenderAs.SectionFunnel || ds.RenderAs == RenderAs.StreamLineFunnel)
            {
                MouseButtonEventHandler onMouseLeftButtonDown4DataSeries = null;
                MouseButtonEventHandler onMouseLeftButtonUp4DataSeries = null;

                MouseButtonEventHandler onMouseLeftButtonDown4DataPoint = null;
                MouseButtonEventHandler onMouseLeftButtonUp4DataPoint = null;

                onMouseLeftButtonDown4DataSeries = ds.GetMouseLeftButtonDownEventHandler();
                onMouseLeftButtonUp4DataSeries = ds.GetMouseLeftButtonUpEventHandler();

                foreach (DataPoint dp in ds.DataPoints)
                {
                    onMouseLeftButtonDown4DataPoint = dp.GetMouseLeftButtonDownEventHandler();
                    onMouseLeftButtonUp4DataPoint = dp.GetMouseLeftButtonUpEventHandler();

                    if (onMouseLeftButtonDown4DataPoint != null || onMouseLeftButtonUp4DataPoint != null)
                        break;
                }

                if ((onMouseLeftButtonDown4DataSeries == null && onMouseLeftButtonUp4DataSeries == null)
                    && (onMouseLeftButtonDown4DataPoint == null && onMouseLeftButtonUp4DataPoint == null))
                    _isDefaultInteractivityAllowed = true;
            }
        }

        /// <summary>
        /// Attach events for each DataSeries and InternalDataPoints
        /// </summary>
        internal void AttachEventsToolTipHref2DataSeries()
        {
            foreach (DataSeries ds in Chart.InternalSeries)
            {
                switch (ds.RenderAs)
                {
                    case RenderAs.StackedArea:
                    case RenderAs.StackedArea100:
                    case RenderAs.Pie:
                    case RenderAs.Doughnut:
                    case RenderAs.SectionFunnel:
                    case RenderAs.StreamLineFunnel:

                        #region Check for default interactivity

                        Check4DefaultInteractivity(ds);

                        #endregion

                        ds.AttachEvent2DataSeriesVisualFaces();

                        foreach (DataPoint dp in ds.InternalDataPoints)
                        {
                            dp.AttachEvent2DataPointVisualFaces(dp);

                            #region Attach Tool Tips

                            dp._parsedToolTipText = dp.TextParser(dp.ToolTipText);

                            if (dp.Faces != null)
                            {
                                if ((Chart as Chart).View3D && (ds.RenderAs == RenderAs.Pie || ds.RenderAs == RenderAs.Doughnut || ds.RenderAs == RenderAs.SectionFunnel || ds.RenderAs == RenderAs.StreamLineFunnel))
                                {
                                    dp.AttachToolTip(Chart, dp, dp.Faces.VisualComponents);
                                }
                                else if (ds.RenderAs != RenderAs.StackedArea && ds.RenderAs != RenderAs.StackedArea100)
                                    dp.AttachToolTip(Chart, dp, dp.Faces.Visual);
                            }

                            if (ds.RenderAs == RenderAs.StackedArea || ds.RenderAs == RenderAs.StackedArea100)
                            {
                                if (dp.Marker != null)
                                {
                                    if (!Chart.IndicatorEnabled)
                                        dp.AttachToolTip(Chart, dp, dp.Marker.Visual);
                                }
                            }

                            #endregion

                            #region Attach Href

                            dp.SetHref2DataPointVisualFaces();

                            #endregion

                            dp.SetCursor2DataPointVisualFaces();
                        }

                        #region Attach ToolTip for AreaCharts

                        if (ds.RenderAs == RenderAs.StackedArea || ds.RenderAs == RenderAs.StackedArea100)
                        {
                            if (ds.Faces != null)
                            {
                                if(!Chart.IndicatorEnabled)
                                    ds.AttachAreaToolTip(Chart, ds.Faces.VisualComponents);
                            }
                        }

                        #endregion

                        break;
                }
            }
        }

        /// <summary>
        /// Creates the various regions required for drawing vertical charts
        /// </summary>
        /// <param name="chartSize">Chart size as Double</param>
        /// <param name="NewSize">NewSize</param>
        /// <returns>Size</returns>
        private Size CreateRegionsForVerticalCharts(Double chartSize, Size NewSize, AxisRepresentations renderAxisType, Boolean isPartialUpdate)
        {   
            Double chartCanvasHeight = 0;
            Double chartCanvasWidth = 0;

            if (Double.IsNaN(NewSize.Height) || NewSize.Height <= 0 || Double.IsNaN(NewSize.Width) || NewSize.Width <= 0 || Double.IsNaN(chartSize) || chartSize <= 0)
            {
                return new Size(chartCanvasWidth, chartCanvasHeight);
            }
            else
            {
                if (Chart.View3D)
                {   
                    Double plankOpacity = 0.3;

                    // Draw 3D horizontal plank 
                    if (Chart.Background != null && (Chart.Background as SolidColorBrush) != null)
                    {
                        if ((Chart.Background as SolidColorBrush).Color == Colors.Black)
                            plankOpacity = 1;
                    }

                    DrawHorizontalPlank(PLANK_DEPTH, PLANK_THICKNESS, NewSize.Height, renderAxisType, isPartialUpdate);

                    if (NewSize.Height - PLANK_DEPTH - PLANK_THICKNESS > 0)
                        DrawVerticalPlank(NewSize.Height - PLANK_DEPTH - PLANK_THICKNESS, PLANK_DEPTH, 0.25, plankOpacity, isPartialUpdate);

                    // Set the chart canvas size
                    chartCanvasHeight = NewSize.Height - PLANK_OFFSET;

                    //chartCanvasWidth = PlotAreaCanvas.ActualWidth - PlankDepth;
                    chartCanvasWidth = chartSize - PLANK_DEPTH;
                }
                else
                {
                    // Set the chart canvas size
                    chartCanvasHeight = NewSize.Height;
                    //chartCanvasWidth = PlotAreaCanvas.ActualWidth;
                    chartCanvasWidth = chartSize;
                }
            }

            // if either height or width is invalid
            if (chartCanvasHeight <= 0 || chartSize <= 0)
                return new Size(0, 0);

            // Return the size of the drawing canvas
            return new Size(chartCanvasWidth, chartCanvasHeight);
        }

        /// <summary>
        /// Creates the various regions required for drawing horizontal charts
        /// </summary>
        /// <param name="chartSize">Chart size as Double</param>
        /// <param name="NewSize">NewSize</param>
        /// <returns>Size</returns>
        private Size CreateRegionsForHorizontalCharts(Double chartSize, Size NewSize, AxisRepresentations renderAxisType, Boolean isPartialUpdate)
        {
            Double chartCanvasHeight = 0;
            Double chartCanvasWidth = 0;

            if (Double.IsNaN(NewSize.Height) || NewSize.Height <= 0 || Double.IsNaN(NewSize.Width) || NewSize.Width <= 0 || Double.IsNaN(chartSize) || chartSize <= 0)
            {
                return new Size(chartCanvasWidth, chartCanvasHeight);
            }
            else
            {
                if (Chart.View3D)
                {
                    if(_horizontalPlank != null)
                    {
                        if (PlottingCanvas.Children.Contains(_horizontalPlank.Visual))
                            PlottingCanvas.Children.Remove(_horizontalPlank.Visual);
                    }
                    // Draw 3D vertical plank 
                    DrawVerticalPlank(PLANK_DEPTH, PLANK_THICKNESS, renderAxisType, isPartialUpdate);

                    // Set the chart canvas size
                    chartCanvasHeight = chartSize - PLANK_DEPTH;
                    chartCanvasWidth = NewSize.Width - PLANK_OFFSET;
                }
                else
                {   
                    // Set the chart canvas size
                    chartCanvasHeight = chartSize;
                    chartCanvasWidth = NewSize.Width;
                }
            }

            // if either height or width is invalid
            if (chartCanvasHeight <= 0 || chartCanvasWidth <= 0)
                return new Size(0, 0);

            // Return the size of the drawing canvas
            return new Size(chartCanvasWidth, chartCanvasHeight);
        }

        /// <summary>
        /// Creates the various regions required for drawing the charts without Axis
        /// </summary>
        /// <param name="NewSize">NewSize</param>
        /// <returns>Size</returns>
        private Size CreateRegionsForChartsWithoutAxis(Size NewSize)
        {
            Double chartCanvasHeight = 0;
            Double chartCanvasWidth = 0;

            if (Double.IsNaN(NewSize.Height) || NewSize.Height <= 0 || Double.IsNaN(NewSize.Width) || NewSize.Width <= 0)
            {
                return new Size(chartCanvasWidth, chartCanvasHeight);
            }
            else
            {
                // Set the chart canvas size
                chartCanvasHeight = NewSize.Height;
                chartCanvasWidth = NewSize.Width;
            }

            // Return the size of the drawing canvas
            return new Size(chartCanvasWidth, chartCanvasHeight);
        }

        /// <summary>
        /// Scroll event is used to scroll to the horizontal/vertical offset of ScrollViewer
        /// </summary>
        internal void AttachScrollEvents()
        {
            if (AxisX != null && PlotDetails.ChartOrientation == ChartOrientationType.Vertical)
            {
                AxisX.ScrollBarOffsetChanged -= AxesXScrollBarElement_Scroll;
                AxisX.ScrollBarOffsetChanged += new System.Windows.Controls.Primitives.ScrollEventHandler(AxesXScrollBarElement_Scroll);
                AxisX.SetScrollBarValueFromOffset(AxisX.ScrollBarOffset);
            }
            if (AxisX2 != null && PlotDetails.ChartOrientation == ChartOrientationType.Vertical)
            {
                AxisX2.ScrollBarOffsetChanged -= AxesXScrollBarElement_Scroll;
                AxisX2.ScrollBarOffsetChanged += new System.Windows.Controls.Primitives.ScrollEventHandler(AxesXScrollBarElement_Scroll);
                AxisX2.SetScrollBarValueFromOffset(AxisX2.ScrollBarOffset);
            }
            if (AxisX != null && PlotDetails.ChartOrientation == ChartOrientationType.Horizontal)
            {
                AxisX.ScrollBarOffsetChanged -= AxesXScrollBarElement_Scroll;
                AxisX.ScrollBarOffsetChanged += new System.Windows.Controls.Primitives.ScrollEventHandler(AxesXScrollBarElement_Scroll);
                AxisX.SetScrollBarValueFromOffset(AxisX.ScrollBarOffset);
            }
            if (AxisX2 != null && PlotDetails.ChartOrientation == ChartOrientationType.Horizontal)
            {
                AxisX2.ScrollBarOffsetChanged -= AxesXScrollBarElement_Scroll;
                AxisX2.ScrollBarOffsetChanged += new System.Windows.Controls.Primitives.ScrollEventHandler(AxesXScrollBarElement_Scroll);
                AxisX2.SetScrollBarValueFromOffset(AxisX2.ScrollBarOffset);
            }
        }

        /// <summary>
        /// Event handler for loaded event of the PlottingCanvas
        /// </summary>
        /// <param name="sender">Canvas</param>
        /// <param name="e">RoutedEventArgs</param>
        private void PlottingCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            PlottingCanvas.Loaded -= new RoutedEventHandler(PlottingCanvas_Loaded);

            AttachScrollEvents();

            Chart._renderLock = false;

            if (Chart._renderLapsedCounter >= 1)
                Chart.Render();

            Animate();

            Chart._internalAnimationEnabled = false;

            //if(!Chart.AnimationEnabled || Chart.IsInDesignMode || !_isFirstTimeRender)
            //    Visifire.Charts.Chart.SelectDataPoints(Chart);

            Chart.FireRenderedEvent();

            _isFirstTimeRender = false;

            //System.Diagnostics.Debug.WriteLine("Loaded() >");
        }

       /// <summary>
       /// Add Title visual to DrawingArea
       /// </summary>
       /// <param name="chart">Chart</param>
       /// <param name="title">Title to add</param>
       /// <param name="panel">Panel where title to be added</param>
       /// <param name="width">Available width for title</param>
       /// <param name="height">Available height for title</param>
        private void AddTitle(Chart chart, Title title, Panel panel, Double width, Double height)
        {
            Double tempFontSize = title.InternalFontSize;
            title.Chart = chart;

        RECREATE_TITLE:

            title.CreateVisualObject(new ElementData() { Element = title });

            Size size = Graphics.CalculateVisualSize(title.Visual);

            if (title.InternalVerticalAlignment == VerticalAlignment.Top || title.InternalVerticalAlignment == VerticalAlignment.Bottom
                || (title.InternalVerticalAlignment == VerticalAlignment.Center && title.InternalHorizontalAlignment == HorizontalAlignment.Center))
            {
                if (size.Width > width && (chart.ActualWidth - width) < width)
                {
                    if (title.InternalFontSize == 1)
                        goto OUT;

                    title.IsNotificationEnable = false;
                    title.InternalFontSize -= 1;
                    title.IsNotificationEnable = true;
                    goto RECREATE_TITLE;
                }
            }
            else
            {
                if (size.Height >= height || title.Height >= height)
                {
                    if (title.InternalFontSize == 1)
                        goto OUT;

                    title.IsNotificationEnable = false;
                    title.InternalFontSize -= 1;
                    title.IsNotificationEnable = true;
                    goto RECREATE_TITLE;
                }
            }
        OUT:

            title.IsNotificationEnable = false;
            title.InternalFontSize = tempFontSize;
            title.IsNotificationEnable = true;

            // Add title Visual as children of panel
            panel.Children.Add(title.Visual);
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Convert RenderAs to MarkerType
        /// </summary>
        /// <param name="renderAs">Chart type</param>
        /// <param name="dataSeries">DataSeries</param>
        /// <returns>MarkerType</returns>
        internal MarkerTypes RenderAsToMarkerType(RenderAs renderAs, DataSeries dataSeries)
        {
            switch (renderAs)
            {
                case RenderAs.Pie:
                case RenderAs.Doughnut:
                case RenderAs.Bubble:
                    return MarkerTypes.Circle;
                case RenderAs.Point:
                    return dataSeries.MarkerType;

                case RenderAs.Line:
                case RenderAs.StepLine:
                case RenderAs.Stock:
                case RenderAs.CandleStick:
                    return dataSeries.MarkerType;

                case RenderAs.Area:
                case RenderAs.StackedArea:
                case RenderAs.StackedArea100:
                    return MarkerTypes.Triangle;

                default:
                    return MarkerTypes.Square;

            }
        }

        #endregion

        #region Internal Events
       
        #endregion

        #region Static Methods

        #endregion

        #region Data

        /// <summary>
        /// Old Zooming Scale
        /// </summary>
        private Double _oldZoomingScale = Double.NaN;

        /// <summary>
        /// Size of the PlotArea is calculated in calculated in Draw() 
        /// </summary>
        internal Size _plotAreaSize;
        
        /// <summary>
        /// Grid animation duration
        /// </summary>
        internal static Double GRID_ANIMATION_DURATION = 1;    
        
        /// <summary>
        /// Maximum size of the chart drawn inside the ScrollViewer.
        /// </summary>
        internal static Double MAX_CHART_SIZE = 15000;

        /// <summary>
        /// Chart scroll-viewer Offset for horizontal chart. 
        /// It is used to show the grid lines properly at the right hand side of the scroll-viewer
        /// </summary>
        internal static Double SCROLLVIEWER_OFFSET4HORIZONTAL_CHART = 1;            
        
        /// <summary>
        /// Whether animation is fired for the first time
        /// </summary>
        internal Boolean _isAnimationFired = false;

        internal StackPanel _axisIndicatorElement;

        internal Canvas _axisCallOutContainer;

        internal TextBlock _axisIndicatorTextBlock;

        internal System.Windows.Shapes.Path _callOutPath4AxisIndicator;

        internal Border _axisIndicatorBorderElement;

        internal Line _verticalLineIndicator;

        /// <summary>
        /// Whether it is the first time render of the chart
        /// </summary>
        internal bool _isFirstTimeRender = true;

        private Int32 zoomCount = 0;

        internal ColorSet _financialColorSet = null;

        #region "Used for testing purpose only"

        /// <summary>
        /// Number of redrawing chart for a single render call 
        /// (Used for Testing Only)
        /// </summary>
        internal Int32 _renderCount = 0;

        internal Boolean isScrollingActive = false;

        /// <summary>
        /// It provides a space above the axis indicator
        /// </summary>
        private const Double DEFAULT_INDICATOR_GAP = 6;
        private const Double DEFAULT_TOOLTIP_OPACITY = 0.9;

        private DataPoint _lastNearestDataPoint;

        private Boolean _isDragging = false;

        /// <summary>
        /// Whether default interactivity is allowed for Pie/Doughnut/Funnel chart
        /// </summary>
        internal Boolean _isDefaultInteractivityAllowed = false;

        private Point _firstZoomPos;
        private Point _lastZoomPos;
        private Boolean _zoomStart = false;

        #endregion "Used for Testing Only"

        #endregion
    }
}
