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
using System.Windows.Media.Animation;
using Visifire.Commons;

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

        /// <summary>
        /// Draw chartarea
        /// </summary>
        /// <param name="chart">Chart</param>
        public void Draw(Chart chart)
        {
           
            System.Diagnostics.Debug.WriteLine("Draw() > ");
            Boolean isScrollingActive = Chart.IsScrollingActivated;
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

            if (PlotDetails != null)
                PlotDetails = null;

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
            Size plotAreaSize = CalculatePlotAreaSize(remainingSizeAfterAddingTitles);

            // Need to recalculate PlotArea size if any title exist with left or right aligned
            if (isLeftOrRightAlignedTitlesExist)
            {
                ClearTitlePanels();

                AddTitles(chart, false, plotAreaSize.Height, actualChartSize.Width, out isLeftOrRightAlignedTitlesExist);

                ResetTitleAndLegendPannelsSize();

                remainingSizeAfterAddingTitles = CalculateLegendMaxSize(actualChartSize);

                plotAreaSize = CalculatePlotAreaSize(remainingSizeAfterAddingTitles);
            }

            HideAllAxesScrollBars();

            // Check if drawing axis is necessary or not
            if (PlotDetails.ChartOrientation != ChartOrientationType.NoAxis)
                SetAxesProperties();
            
            Size remainingSize = DrawChart(plotAreaSize);

            // Add all the titles to chart of type dock inside
            AddTitles(Chart, true, remainingSize.Height, remainingSize.Width, out isLeftOrRightAlignedTitlesExist);

            // Add all the legends to chart of type dock inside
            AddLegends(Chart, true, remainingSize.Height, remainingSize.Width);

            RetainOldScrollOffsetOfScrollViewer();

            // Chart.AttachEvents2Visual(Chart.PlotArea, PlotAreaCanvas);

            AttachOrDetachIntaractivity(chart);

            if (isScrollingActive && Chart.IsScrollingActivated)
            {   
                if (!chart._drawingCanvas.Children.Contains(PlotAreaCanvas))
                    chart._drawingCanvas.Children.Add(PlotAreaCanvas);
            }
            else if (!isScrollingActive && !Chart.IsScrollingActivated)
            {
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
        /// Attach or detach intaractivity for selection
        /// </summary>
        /// <param name="chart"></param>
        public void AttachOrDetachIntaractivity(Chart chart)
        {
            foreach (DataSeries ds in chart.InternalSeries)
            {
                if (_isFirstTimeRender)
                {   
                    if(ds.SelectionEnabled)
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
                        if (PlotDetails.ChartOrientation == ChartOrientationType.Horizontal)
                            Chart._plotAreaScrollViewer.ScrollToVerticalOffset(AxisX.GetScrollBarValueFromOffset(AxisX.CurrentScrollScrollBarOffset));
                        else
                            Chart._plotAreaScrollViewer.ScrollToHorizontalOffset(AxisX.GetScrollBarValueFromOffset(AxisX.CurrentScrollScrollBarOffset));
                    }
                };
            }
#endif
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

                if (legend.VerticalAlignment == VerticalAlignment.Bottom || legend.VerticalAlignment == VerticalAlignment.Top)
                {
                    boundingRec.Height -= elementSize.Height;
                    if (legend.VerticalAlignment == VerticalAlignment.Bottom)
                        Chart._bottomOuterLegendPanel.Height += elementSize.Height;
                    else
                        Chart._topOuterLegendPanel.Height += elementSize.Height;
                }
                else if (legend.VerticalAlignment == VerticalAlignment.Center || legend.VerticalAlignment == VerticalAlignment.Stretch)
                {
                    if (legend.HorizontalAlignment == HorizontalAlignment.Left || legend.HorizontalAlignment == HorizontalAlignment.Right)
                    {
                        boundingRec.Width -= elementSize.Width;
                        if (legend.HorizontalAlignment == HorizontalAlignment.Left)
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
                Chart.PlotArea = new PlotArea();

            Chart.PlotArea.Chart = Chart;

            if (Chart.PlotArea.Visual == null)
            {
                if (!String.IsNullOrEmpty(Chart.Theme))
                    Chart.PlotArea.ApplyStyleFromTheme(Chart, "PlotArea");

                Chart.PlotArea.CreateVisualObject();
                PlotAreaCanvas = Chart.PlotArea.Visual;
                Chart.AttachEvents2Visual(Chart.PlotArea, PlotAreaCanvas);
            }
            else
                Chart.PlotArea.UpdateProperties();

            PlotAreaCanvas.HorizontalAlignment = HorizontalAlignment.Stretch;
            PlotAreaCanvas.VerticalAlignment = VerticalAlignment.Stretch;
            PlotAreaCanvas.Margin = new Thickness(0);
            
            Chart.PlotArea.AttachHref(Chart, Chart.PlotArea.BorderElement, Chart.PlotArea.Href, Chart.PlotArea.HrefTarget);
            Chart.PlotArea.DetachToolTip(Chart.PlotArea.BorderElement);
            Chart.PlotArea.AttachToolTip(Chart, Chart.PlotArea, Chart.PlotArea.BorderElement);
            //Chart.AttachEvents2Visual(Chart.PlotArea, PlotAreaCanvas.Children[0] as Border);
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

                if (title.VerticalAlignment == VerticalAlignment.Bottom || title.VerticalAlignment == VerticalAlignment.Top)
                {
                    boundingRec.Height -= elementSize.Height;
                    if (title.VerticalAlignment == VerticalAlignment.Bottom)
                        Chart._bottomOuterTitlePanel.Height += elementSize.Height;
                    else
                        Chart._topOuterTitlePanel.Height += elementSize.Height;
                }
                else if (title.VerticalAlignment == VerticalAlignment.Center || title.VerticalAlignment == VerticalAlignment.Stretch)
                {
                    if (title.HorizontalAlignment == HorizontalAlignment.Left || title.HorizontalAlignment == HorizontalAlignment.Right)
                    {
                        boundingRec.Width -= elementSize.Width;
                        if (title.HorizontalAlignment == HorizontalAlignment.Left)
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

        /// <summary>
        /// Populate InternalSeries list from Series collection
        /// InternalSeries is used while rendering the chart, Series collection is not used.
        /// Each render must work with a single set of non variable data set otherwise chart won't render properly.
        /// </summary>
        private void PopulateInternalSeriesList()
        {
            if (Chart.InternalSeries != null)
                Chart.InternalSeries.Clear();

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
                Chart.InternalSeries = Chart.Series.ToList();

            foreach (DataSeries ds in Chart.InternalSeries)
            {
                ds.InternalDataPoints = ds.DataPoints.ToList();
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
                axis.Scroll -= AxesXScrollBarElement_Scroll;
                axis.Scroll += new System.Windows.Controls.Primitives.ScrollEventHandler(AxesXScrollBarElement_Scroll);
                axis.SetScrollBarValueFromOffset(axis.ScrollBarOffset);
            }
        }

        /// <summary>
        /// Draw axisX
        /// </summary>
        /// <param name="availableSize">Available size</param>
        /// <returns>
        /// For vertical chart: total height reduced by the axis
        /// For horizontal chart: total width reduced by the axis
        /// </returns>
        private Double DrawAxesX(Size availableSize)
        {
            Double totalHeightReduced = 0;

            // Clear axis panels
            Chart._topAxisPanel.Children.Clear();
            Chart._bottomAxisPanel.Children.Clear();

            if (AxisX != null && PlotDetails.ChartOrientation == ChartOrientationType.Vertical)
            {
                AxisX.Width = availableSize.Width;
                AxisX.ScrollBarElement.Width = AxisX.Width;

                if (PlotAreaScrollViewer != null)
                {
                    AxisX.ScrollableSize = ScrollableLength;
#if SL
                    AxisX.ScrollBarElement.Maximum = ScrollableLength - PlotAreaScrollViewer.ViewportWidth;
                    AxisX.ScrollBarElement.ViewportSize = PlotAreaScrollViewer.ViewportWidth;
#else                   
                    AxisX.ScrollBarElement.Maximum = ScrollableLength - PlotAreaScrollViewer.ActualWidth;
                    AxisX.ScrollBarElement.ViewportSize = PlotAreaScrollViewer.ActualWidth;
#endif
                }

                AxisX.CreateVisualObject(Chart);

                SetScrollBarChanges(AxisX);

                AxisX.ScrollBarElement.Scroll -= AxesXScrollBarElement_Scroll;
                AxisX.ScrollBarElement.Scroll += new System.Windows.Controls.Primitives.ScrollEventHandler(AxesXScrollBarElement_Scroll);

                if (AxisX.Width >= AxisX.ScrollableSize || AxisX.ScrollBarElement.Maximum == 0)
                    AxisX.ScrollBarElement.Visibility = Visibility.Collapsed;
                else
                {
                    AxisX.ScrollBarElement.Visibility = Visibility.Visible;
                }

                Chart._bottomAxisPanel.Children.Add(AxisX.Visual);

                Size size = Graphics.CalculateVisualSize(Chart._bottomAxisContainer);
                totalHeightReduced += size.Height;
                
                if (AxisX.ScrollViewerElement.Children.Count > 0)
                    (AxisX.ScrollViewerElement.Children[0] as FrameworkElement).SetValue(Canvas.LeftProperty, -1 * AxisX.GetScrollBarValueFromOffset(AxisX.CurrentScrollScrollBarOffset));

            }
            else
                Chart._bottomAxisScrollBar.Visibility = Visibility.Collapsed;

            if (AxisX2 != null && PlotDetails.ChartOrientation == ChartOrientationType.Vertical)
            {
                AxisX2.Width = availableSize.Width;
                AxisX2.ScrollBarElement.Width = AxisX2.Width;

                if (PlotAreaScrollViewer != null)
                {
                    AxisX2.ScrollableSize = ScrollableLength;
#if SL
                    AxisX2.ScrollBarElement.Maximum = ScrollableLength - PlotAreaScrollViewer.ViewportWidth;
                    AxisX2.ScrollBarElement.ViewportSize = PlotAreaScrollViewer.ViewportWidth;
#else               
                    AxisX2.ScrollBarElement.Maximum = ScrollableLength - PlotAreaScrollViewer.ActualWidth;
                    AxisX2.ScrollBarElement.ViewportSize = PlotAreaScrollViewer.ActualWidth;
#endif
                }

                AxisX2.CreateVisualObject(Chart);
                SetScrollBarChanges(AxisX2);

                AxisX2.ScrollBarElement.Scroll -= AxesX2ScrollBarElement_Scroll;
                AxisX2.ScrollBarElement.Scroll += new System.Windows.Controls.Primitives.ScrollEventHandler(AxesX2ScrollBarElement_Scroll);

                if (AxisX2.Width >= AxisX2.ScrollableSize || AxisX2.ScrollBarElement.Maximum == 0)
                    AxisX2.ScrollBarElement.Visibility = Visibility.Collapsed;
                else
                {
                    AxisX2.ScrollBarElement.Visibility = Visibility.Visible;
                }

                Chart._topAxisPanel.Children.Add(AxisX2.Visual);

                Size size = Graphics.CalculateVisualSize(Chart._topAxisContainer);
                totalHeightReduced += size.Height;
               
                if (AxisX2.ScrollViewerElement.Children.Count > 0)
                    (AxisX2.ScrollViewerElement.Children[0] as FrameworkElement).SetValue(Canvas.LeftProperty, -AxisX2.GetScrollBarValueFromOffset(AxisX2.CurrentScrollScrollBarOffset));

            }
            else
                Chart._topAxisScrollBar.Visibility = Visibility.Collapsed;

            if (AxisY != null && PlotDetails.ChartOrientation == ChartOrientationType.Horizontal)
            {
                AxisY.Width = availableSize.Width;
                AxisY.ScrollableSize = availableSize.Width;
                AxisY.CreateVisualObject(Chart);

                AxisY.ScrollBarElement.Visibility = Visibility.Collapsed;

                Chart._bottomAxisPanel.Children.Add(AxisY.Visual);

                Size size = Graphics.CalculateVisualSize(Chart._bottomAxisContainer);
                totalHeightReduced += size.Height;
            }
            else
                Chart._leftAxisScrollBar.Visibility = Visibility.Collapsed;


            if (AxisY2 != null && PlotDetails.ChartOrientation == ChartOrientationType.Horizontal)
            {
                AxisY2.Width = availableSize.Width;
                AxisY2.ScrollableSize = availableSize.Width;
                AxisY2.CreateVisualObject(Chart);

                AxisY2.ScrollBarElement.Visibility = Visibility.Collapsed;

                Chart._topAxisPanel.Children.Add(AxisY2.Visual);

                Size size = Graphics.CalculateVisualSize(Chart._topAxisContainer);
                totalHeightReduced += size.Height;

            }
            else
                Chart._rightAxisScrollBar.Visibility = Visibility.Collapsed;

            return totalHeightReduced;
        }

        /// <summary>
        /// Draw AxisY
        /// </summary>
        /// <param name="availableSize">Available size</param>
        /// <returns>
        /// For vertical chart: total width reduced by the axis
        /// For horizontal chart: total height reduced by the axis
        /// </returns>
        private Double DrawAxesY(Size availableSize)
        {
            Double totalWidthReduced = 0;

            // Clear axis panels
            Chart._leftAxisPanel.Children.Clear();
            Chart._rightAxisPanel.Children.Clear();

            if (AxisX != null && PlotDetails.ChartOrientation == ChartOrientationType.Horizontal)
            {
                AxisX.Height = availableSize.Height;
                AxisX.ScrollBarElement.Height = AxisX.Height;

                if (PlotAreaScrollViewer != null)
                {
                    AxisX.ScrollableSize = ScrollableLength;

#if SL
                    AxisX.ScrollBarElement.Maximum = ScrollableLength - PlotAreaScrollViewer.ViewportHeight;
                    AxisX.ScrollBarElement.ViewportSize = PlotAreaScrollViewer.ViewportHeight;
#else
                    AxisX.ScrollBarElement.Maximum = ScrollableLength - PlotAreaScrollViewer.ActualHeight;
                    AxisX.ScrollBarElement.ViewportSize = PlotAreaScrollViewer.ActualHeight;
#endif
                }

                AxisX.CreateVisualObject(Chart);
                SetScrollBarChanges(AxisX);

                AxisX.ScrollBarElement.Height = availableSize.Height;
                AxisX.ScrollViewerElement.Height = availableSize.Height;
                AxisX.ScrollBarElement.Scroll -= AxesXScrollBarElement_Scroll;
                AxisX.ScrollBarElement.Scroll += new System.Windows.Controls.Primitives.ScrollEventHandler(AxesXScrollBarElement_Scroll);

                if (AxisX.Height >= AxisX.ScrollableSize || AxisX.ScrollBarElement.Maximum == 0)
                    AxisX.ScrollBarElement.Visibility = Visibility.Collapsed;
                else
                {   
                    AxisX.ScrollBarElement.Visibility = Visibility.Visible;
                }

                Chart._leftAxisPanel.Children.Add(AxisX.Visual);

                Size size = Graphics.CalculateVisualSize(Chart._leftAxisContainer);
                totalWidthReduced += size.Width;

                if (AxisX.ScrollViewerElement.Children.Count > 0)
                    (AxisX.ScrollViewerElement.Children[0] as FrameworkElement).SetValue(Canvas.TopProperty, -AxisX.GetScrollBarValueFromOffset(AxisX.CurrentScrollScrollBarOffset));

            }

            if (AxisX2 != null && PlotDetails.ChartOrientation == ChartOrientationType.Horizontal)
            {
                AxisX2.Height = availableSize.Height;
                AxisX2.ScrollBarElement.Height = AxisX2.Height;

                if (PlotAreaScrollViewer != null)
                {
                    AxisX2.ScrollableSize = ScrollableLength;
#if SL
                    AxisX2.ScrollBarElement.Maximum = ScrollableLength - PlotAreaScrollViewer.ViewportHeight;
                    AxisX2.ScrollBarElement.ViewportSize = PlotAreaScrollViewer.ViewportHeight;
#else
                    AxisX2.ScrollBarElement.Maximum = ScrollableLength - PlotAreaScrollViewer.ActualHeight;
                    AxisX2.ScrollBarElement.ViewportSize = PlotAreaScrollViewer.ActualHeight;
#endif
                }

                AxisX2.CreateVisualObject(Chart);
                SetScrollBarChanges(AxisX2);

                AxisX2.ScrollBarElement.Scroll -= AxesX2ScrollBarElement_Scroll;
                AxisX2.ScrollBarElement.Scroll += new System.Windows.Controls.Primitives.ScrollEventHandler(AxesX2ScrollBarElement_Scroll);

                if (AxisX2.Height >= AxisX2.ScrollableSize || AxisX2.ScrollBarElement.Maximum == 0)
                    AxisX2.ScrollBarElement.Visibility = Visibility.Collapsed;
                else
                {
                    AxisX2.ScrollBarElement.Visibility = Visibility.Visible;
                }

                Chart._rightAxisPanel.Children.Add(AxisX2.Visual);

                Size size = Graphics.CalculateVisualSize(Chart._rightAxisContainer);
                totalWidthReduced += size.Width;

                if (AxisX2.ScrollViewerElement.Children.Count > 0)
                    (AxisX2.ScrollViewerElement.Children[0] as FrameworkElement).SetValue(Canvas.TopProperty, -AxisX2.GetScrollBarValueFromOffset(AxisX2.CurrentScrollScrollBarOffset));

            }

            if (AxisY != null && PlotDetails.ChartOrientation == ChartOrientationType.Vertical)
            {
                AxisY.Height = availableSize.Height;
                AxisY.ScrollableSize = availableSize.Height;
                AxisY.CreateVisualObject(Chart);

                AxisY.ScrollBarElement.Visibility = Visibility.Collapsed;

                Chart._leftAxisPanel.Children.Add(AxisY.Visual);

                Size size = Graphics.CalculateVisualSize(Chart._leftAxisContainer);
                totalWidthReduced += size.Width;

                AxisY.Width = size.Width;

                Double left = Chart.Padding.Left + Chart._leftOuterTitlePanel.ActualWidth + Chart._leftOuterLegendPanel.ActualWidth;
                AxisY.SetValue(Canvas.LeftProperty, (Double)left);
            }
            if (AxisY2 != null && PlotDetails.ChartOrientation == ChartOrientationType.Vertical)
            {
                AxisY2.Height = availableSize.Height;
                AxisY2.ScrollableSize = availableSize.Height;
                AxisY2.CreateVisualObject(Chart);

                AxisY2.ScrollBarElement.Visibility = Visibility.Collapsed;

                Chart._rightAxisPanel.Children.Add(AxisY2.Visual);

                Size size = Graphics.CalculateVisualSize(Chart._rightAxisContainer);
                totalWidthReduced += size.Width;

            }

            return totalWidthReduced;
        }

        /// <summary>
        /// Reset all storyboards associated with dataseries to null 
        /// </summary>
        public void ResetStoryboards()
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
            if (Chart.PlotDetails.ChartOrientation == ChartOrientationType.Vertical)
            {
                #region For vertical chart

                Double totalWidthReduced = DrawAxesY(plotAreaSize);
                plotAreaSize.Width -= totalWidthReduced;

                UpdateLayoutSettings(plotAreaSize);

                Double totalHeightReduced1 = DrawAxesX(plotAreaSize);
                plotAreaSize.Height -= totalHeightReduced1;

                plotAreaSize = SetChartAreaCenterGridMargin(plotAreaSize);

                UpdateLayoutSettings(plotAreaSize);

                DrawAxesY(plotAreaSize);
                
                Double totalHeightReduced2 = DrawAxesX(plotAreaSize);

                if (totalHeightReduced2 != totalHeightReduced1)
                {
                    plotAreaSize.Height += totalHeightReduced1;
                    plotAreaSize.Height -= totalHeightReduced2;
                    UpdateLayoutSettings(plotAreaSize);
                    DrawAxesY(plotAreaSize);
                }

                #endregion
            }
            else if (Chart.PlotDetails.ChartOrientation == ChartOrientationType.Horizontal)
            {   
                #region For horizontal chart

                Double totalHeightReduced = DrawAxesX(plotAreaSize);

                plotAreaSize.Height -= totalHeightReduced;
                UpdateLayoutSettings(plotAreaSize);

                Double totalWidthReduced = DrawAxesY(plotAreaSize);

                plotAreaSize.Width -= totalWidthReduced;

                plotAreaSize = SetChartAreaCenterGridMargin(plotAreaSize);

                UpdateLayoutSettings(plotAreaSize);

                plotAreaSize.Width -= SCROLLVIEWER_OFFSET4HORIZONTAL_CHART;
                Double totalHeightReduced2 = DrawAxesX(plotAreaSize);
                plotAreaSize.Width += SCROLLVIEWER_OFFSET4HORIZONTAL_CHART;

                if (totalHeightReduced2 != totalHeightReduced)
                {
                    plotAreaSize.Height += totalHeightReduced;
                    plotAreaSize.Height -= totalHeightReduced2;
                    UpdateLayoutSettings(plotAreaSize);
                    DrawAxesX(plotAreaSize);
                }

                DrawAxesY(plotAreaSize);

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

            Size remainingSizeAfterDrawingAxes = RenderAxes(plotAreaSize);

            RenderChart(remainingSizeAfterDrawingAxes);

            Chart._bottomAxisScrollBar.UpdateLayout();
            Chart._topAxisScrollBar.UpdateLayout();
            Chart._leftAxisScrollBar.UpdateLayout();
            Chart._rightAxisScrollBar.UpdateLayout();

            return remainingSizeAfterDrawingAxes;
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
            Chart._plotAreaScrollViewer.Height = newSize.Height;

            Chart._plotAreaScrollViewer.UpdateLayout();
            PlotAreaScrollViewer = Chart._plotAreaScrollViewer;
        }

        /// <summary>
        /// Calculate PlotArea size for auto scroll
        /// </summary>
        /// <param name="minGap">MinGap value between datapoints</param>
        /// <returns>Auto PlotArea size</returns>
        private Double CalculatePlotAreaAutoSize(Double currentSize)
        {
            Double chartSize;

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
                dp.InternalXValue = i;
                dp.YValue = 0;
                dp.Color = new SolidColorBrush(Colors.Transparent);
                dp.AxisXLabel = i.ToString();
                dp.Chart = Chart;
                ds.DataPoints.Add(dp);
            }

            Chart.InternalSeries.Add(ds);
            ds.IsNotificationEnable = true;
        }

        /// <summary>
        /// Clear unwanted children from PlotAreaCanvas
        /// </summary>
        private void ClearPlotAreaChildren()
        {
            if (PlottingCanvas != null)
            {
                PlottingCanvas.Loaded -= PlottingCanvas_Loaded;
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
        private void DrawHorizontalPlank(Double plankDepth, Double plankThickness, Double position)
        {
            Brush frontBrush, topBrush, rightBrush;

            List<Color> colors = new List<Color>();
            colors.Add(Color.FromArgb(125, 134, 134, 134)); // #FF868686
            colors.Add(Color.FromArgb(255, 210, 210, 210)); // #FFD2D2D2
            colors.Add(Color.FromArgb(255, 255, 255, 255)); // #FFFFFFFF
            colors.Add(Color.FromArgb(255, 223, 223, 223)); // #FFDFDFDF

            frontBrush = Graphics.CreateLinearGradientBrush(0, new Point(0.5, 1), new Point(0.5, 0), colors, new List<double>() { 0, 1.844, 1, 0.442 });

            colors = new List<Color>();
            colors.Add(Color.FromArgb(255, 232, 232, 232));  // #FFE8E8E8
            colors.Add(Color.FromArgb(255, 142, 142, 142));  // #FF8E8787

            rightBrush = Graphics.CreateLinearGradientBrush(0, new Point(0, 0.5), new Point(1, 0.5), colors, new List<double>() { 1, 0 });

            colors = new List<Color>();
            colors.Add(Color.FromArgb(255, 232, 232, 232));  // #FFE8E8E8
            colors.Add(Color.FromArgb(255, 142, 142, 142));  // #FF8E8787
            colors.Add(Color.FromArgb(255, 232, 227, 227));  // #FFE8E3E3

            topBrush = Graphics.CreateLinearGradientBrush(0, new Point(0.5, 1), new Point(0.5, 0), colors, new List<double>() { 0.357, 1, 0 });

            RectangularChartShapeParams columnParams = new RectangularChartShapeParams();
            columnParams.BackgroundBrush = new SolidColorBrush(Color.FromArgb((Byte)255, (Byte)246, (Byte)246, (Byte)246));

            columnParams.Lighting = false;
            columnParams.Size = new Size(ScrollableLength - plankDepth, plankThickness);
            columnParams.Depth = plankDepth;
            columnParams.BorderThickness = 0.25;
            columnParams.BorderBrush = new SolidColorBrush(Colors.White);
            Faces plankFaces = ColumnChart.Get3DColumn(columnParams, frontBrush, topBrush, new SolidColorBrush(Color.FromArgb((Byte)255, (Byte)130, (Byte)130, (Byte)130)));
            Panel plank = plankFaces.Visual as Panel;
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

        /// <summary>
        /// Draws the Vertical 3D Plank
        /// </summary>
        /// <param name="plankDepth">PlankDepth</param>
        /// <param name="plankThickness">PlankThickness</param>
        private void DrawVerticalPlank(Double plankDepth, Double plankThickness)
        {
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

            columnParams.BackgroundBrush = new SolidColorBrush(Color.FromArgb((Byte)255, (Byte)223, (Byte)223, (Byte)223));
            columnParams.Lighting = false;
            columnParams.BorderThickness = 0.45;

            columnParams.BorderBrush = new SolidColorBrush(Color.FromArgb((Byte)255, (Byte)128, (Byte)128, (Byte)128));
            columnParams.Bevel = true;
            columnParams.Size = new Size(plankThickness, ScrollableLength - plankDepth);
            columnParams.Depth = plankDepth;

            Faces plankFaces = ColumnChart.Get3DColumn(columnParams, frontBrush, topBrush, rightBrush);
            Panel plank = plankFaces.Visual as Panel;

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
        private void DrawVerticalPlank(Double height, Double plankDepth, Double plankThickness, Double plankOpacity)
        {
            RectangularChartShapeParams columnParams = new RectangularChartShapeParams();
            columnParams.BackgroundBrush = new SolidColorBrush(Color.FromArgb((Byte)255, (Byte)127, (Byte)127, (Byte)127));
            columnParams.Lighting = true;
            columnParams.Size = new Size(plankThickness, height);
            columnParams.Depth = plankDepth;

            List<Color> colors = new List<Color>();
            colors.Add(Color.FromArgb(255, 232, 232, 232));
            colors.Add(Color.FromArgb(255, 142, 135, 135));

            Brush rightBrush = Graphics.CreateLinearGradientBrush(0, new Point(0, 0.5), new Point(1, 0.5), colors, new List<double>() { 0, 1 });

            Faces plankFaces = ColumnChart.Get3DColumn(columnParams, null, null, rightBrush);
            Panel plank = plankFaces.Visual as Panel;

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
                    trendLine.CreateVisualObject(ChartVisualCanvas.Width, ChartVisualCanvas.Height);
                    if (trendLine.Visual != null)
                    {
                        trendLineCanvas.Children.Add(trendLine.Visual);

                        RectangleGeometry clipRectangle = new RectangleGeometry();
                        clipRectangle.Rect = new Rect(0, 0, ChartVisualCanvas.Width, ChartVisualCanvas.Height);
                        trendLineCanvas.Clip = clipRectangle;
                    }
                }
            }
        }

        /// <summary>
        /// Render trendLines
        /// </summary>
        private void RenderTrendLines()
        {
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

            Canvas trendLineCanvas = new Canvas() { Height = ChartVisualCanvas.Height, Width = ChartVisualCanvas.Width};

            AddTrendLines(AxisX, trendLinesReferingToPrimaryAxesX, trendLineCanvas);

            AddTrendLines(AxisY, trendLinesReferingToPrimaryAxisY, trendLineCanvas);

            AddTrendLines(AxisX2, trendLinesReferingToSecondaryAxesX, trendLineCanvas);

            AddTrendLines(AxisY2, trendLinesReferingToSecondaryAxisY, trendLineCanvas);

            ChartVisualCanvas.Children.Add(trendLineCanvas);

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
            foreach (ChartGrid grid in axis.Grids)
            {
                grid.IsNotificationEnable = false;
                grid.Chart = Chart;

                grid.ApplyStyleFromTheme(Chart, styleName);

                grid.CreateVisualObject(width, height, isAnimationEnabled, GRID_ANIMATION_DURATION);
                
                if (grid.Visual != null)
                {
                    ChartVisualCanvas.Children.Add(grid.Visual);
                }

                grid.IsNotificationEnable = true;
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
        /// Renders charts based on the orientation type
        /// </summary>
        /// <param name="newSize">NewSize</param>
        private void RenderChart(Size newSize)
        {
            ClearPlotAreaChildren();

            PlotAreaScrollViewer = Chart._plotAreaScrollViewer;
            PlotAreaScrollViewer.Background = new SolidColorBrush(Colors.Transparent);

            PlotAreaCanvas.Width = newSize.Width;
            PlotAreaCanvas.Height = newSize.Height;

            PlottingCanvas = new Canvas();

            PlottingCanvas.Loaded += new RoutedEventHandler(PlottingCanvas_Loaded);
            PlottingCanvas.SetValue(Canvas.ZIndexProperty, 1);
            PlotAreaCanvas.Children.Add(PlottingCanvas);

            if (Double.IsNaN(newSize.Height) || newSize.Height <= 0 || Double.IsNaN(newSize.Width) || newSize.Width <= 0)
            {
                return;
            }
            else
            {
                if (PlotDetails.ChartOrientation == ChartOrientationType.Vertical)
                {
                    PlottingCanvas.Width = ScrollableLength + PLANK_DEPTH;
                    PlottingCanvas.Height = newSize.Height;
                }
                else if (PlotDetails.ChartOrientation == ChartOrientationType.Horizontal)
                {
                    PlottingCanvas.Width = newSize.Width;
                    PlottingCanvas.Height = ScrollableLength + PLANK_DEPTH;
                }
            }

            // Create the chart canvas
            ChartVisualCanvas = new Canvas();

            PlottingCanvas.Children.Add(ChartVisualCanvas);

            // Default size of the chart canvas
            Size chartCanvasSize = new Size(0, 0);

            // Create the various region required for drawing charts
            switch (PlotDetails.ChartOrientation)
            {   
                case ChartOrientationType.Vertical:
                    chartCanvasSize = CreateRegionsForVerticalCharts(ScrollableLength, newSize);
                    // set chart Canvas position
                    ChartVisualCanvas.SetValue(Canvas.LeftProperty, PLANK_DEPTH);
                    Chart.PlotArea.BorderElement.SetValue(Canvas.LeftProperty, PLANK_DEPTH);

                    break;

                case ChartOrientationType.Horizontal:
                    chartCanvasSize = CreateRegionsForHorizontalCharts(ScrollableLength, newSize);
                    // set chart Canvas position
                    ChartVisualCanvas.SetValue(Canvas.LeftProperty, PLANK_OFFSET);
                    Chart.PlotArea.BorderElement.SetValue(Canvas.LeftProperty, PLANK_OFFSET);
                    break;

                case ChartOrientationType.NoAxis:
                    chartCanvasSize = CreateRegionsForChartsWithoutAxis(newSize);
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
            Chart.PlotArea.ApplyShadow(newSize, PLANK_OFFSET, PLANK_DEPTH, PLANK_THICKNESS);

            // Draw the chart grids
            if (PlotDetails.ChartOrientation != ChartOrientationType.NoAxis)
            {   
                RenderGrids();
                RenderTrendLines();
            }

            // Render each plot group from the plotgroups list of plotdetails
            RenderSeries();

            Chart._plotCanvas.Width = PlottingCanvas.Width;
            Chart._plotCanvas.Height = PlottingCanvas.Height;

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
            // System.Diagnostics.Debug.WriteLine("Offset" + scrollBarOffset.ToString());
        }

        /// <summary>
        /// Event handler for scroll event of the axis-x scrollbar
        /// </summary>
        /// <param name="sender">Scrollbar</param>
        /// <param name="e">System.Windows.Controls.Primitives.ScrollEventArgs</param>
        private void AxesXScrollBarElement_Scroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e)
        {
            if (PlotDetails.ChartOrientation == ChartOrientationType.Vertical)
            {
                Double offset = e.NewValue;

#if SL
                AxisX.ScrollBarElement.Maximum = ScrollableLength - PlotAreaScrollViewer.ViewportWidth;
                AxisX.ScrollBarElement.ViewportSize = PlotAreaScrollViewer.ViewportWidth;
#else
                AxisX.ScrollBarElement.Maximum = ScrollableLength - PlotAreaScrollViewer.ActualWidth;
                AxisX.ScrollBarElement.ViewportSize = PlotAreaScrollViewer.ActualWidth;

                if (e.NewValue <= 1)
                    offset = e.NewValue * AxisX.ScrollBarElement.Maximum;
#endif
                PlotAreaScrollViewer.ScrollToHorizontalOffset(offset);

                if (AxisX.ScrollViewerElement.Children.Count > 0)
                    (AxisX.ScrollViewerElement.Children[0] as FrameworkElement).SetValue(Canvas.LeftProperty, -offset);


                SaveAxisContentOffsetAndResetMargin(AxisX, offset);

                AxisX._isScrollToOffsetEnabled = false;
                offset = offset / (AxisX.ScrollBarElement.Maximum - AxisX.ScrollBarElement.Minimum);
                
                if (!Double.IsNaN(offset))
                    AxisX.ScrollBarOffset = (offset > 1) ? 1 : (offset < 0) ? 0 : offset;

                AxisX._isScrollToOffsetEnabled = true;
            }

            if (PlotDetails.ChartOrientation == ChartOrientationType.Horizontal)
            {
                Double offset = e.NewValue;

#if SL
                AxisX.ScrollBarElement.Maximum = ScrollableLength - PlotAreaScrollViewer.ViewportHeight;
                AxisX.ScrollBarElement.ViewportSize = PlotAreaScrollViewer.ViewportHeight;
#else
                AxisX.ScrollBarElement.Maximum = ScrollableLength - PlotAreaScrollViewer.ActualHeight;
                AxisX.ScrollBarElement.ViewportSize = PlotAreaScrollViewer.ActualHeight;
                if (e.NewValue <= 1)
                    offset = e.NewValue * AxisX.ScrollBarElement.Maximum;
#endif
                PlotAreaScrollViewer.ScrollToVerticalOffset(offset);

                if (AxisX.ScrollViewerElement.Children.Count > 0)
                    (AxisX.ScrollViewerElement.Children[0] as FrameworkElement).SetValue(Canvas.TopProperty, -offset);


                SaveAxisContentOffsetAndResetMargin(AxisX, (AxisX.ScrollBarElement.Maximum - offset));

                AxisX._isScrollToOffsetEnabled = false;
                offset = (AxisX.ScrollBarElement.Maximum - offset) / (AxisX.ScrollBarElement.Maximum - AxisX.ScrollBarElement.Minimum);
                
                if(!Double.IsNaN(offset))
                    AxisX.ScrollBarOffset = (offset > 1) ? 1 : (offset < 0) ? 0 : offset;

                AxisX._isScrollToOffsetEnabled = true;

            }
            if (AxisX2 != null)
                AxisX2.ScrollBarElement.Value = e.NewValue;
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
                PlotAreaScrollViewer.ScrollToHorizontalOffset(offset);

                if (AxisX2.ScrollViewerElement.Children.Count > 0)
                    (AxisX2.ScrollViewerElement.Children[0] as FrameworkElement).Margin = new Thickness(offset, 0, 0, 0);

                SaveAxisContentOffsetAndResetMargin(AxisX2, offset);
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
                PlotAreaScrollViewer.ScrollToVerticalOffset(offset);

                if (AxisX2.ScrollViewerElement.Children.Count > 0)
                    (AxisX2.ScrollViewerElement.Children[0] as FrameworkElement).Margin = new Thickness(0, offset, 0, 0);
                
                SaveAxisContentOffsetAndResetMargin(AxisX2, offset);
            }

            if (AxisX != null)
                AxisX.ScrollBarElement.Value = e.NewValue;
        }

        /// <summary>
        /// Render DataSeries to visual. 
        /// (Render each plotgroup from the plotgroup list of plotdetails)
        /// </summary>
        private void RenderSeries()
        {   
            Int32 renderedSeriesCount = 0;      // Contain count of series that have been already rendered

            // Contains a list of serties as per the drawing order generated in the plotdetails
            List<DataSeries> dataSeriesListInDrawingOrder = PlotDetails.SeriesDrawingIndex.Keys.ToList();

            List<DataSeries> selectedDataSeries4Rendering;          // Contains a list of serries to be rendered in a rendering cycle
            Int32 currentDrawingIndex;                              // Drawing index of the selected series 
            RenderAs currentRenderAs;                               // Rendereas type of the selected series
            Panel renderedChart;                                   // A canvas that contains the chart rendered using the selected series

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

                renderedChart = RenderSeriesFromList(selectedDataSeries4Rendering);

                if (renderedChart != null)
                    ChartVisualCanvas.Children.Add(renderedChart);

                renderedSeriesCount += selectedDataSeries4Rendering.Count;
            }

            AttachEventsToolTipHref2DataSeries();
        }

        /// <summary>
        /// Calls the appropriate chart rendering function to render the series available in the series list.
        /// Creates a layer as per the drawing index
        /// </summary>
        /// <param name="seriesListForRendering">List of selected dataseries</param>
        /// <returns>Canvas with rendered dataSeries visual</returns>
        private Panel RenderSeriesFromList(List<DataSeries> dataSeriesList4Rendering)
        {
            Panel renderedCanvas = null;

            renderedCanvas = RenderHelper.GetVisualObject(dataSeriesList4Rendering[0].RenderAs, ChartVisualCanvas.Width, ChartVisualCanvas.Height, PlotDetails, dataSeriesList4Rendering, Chart, PLANK_DEPTH, (Chart._internalAnimationEnabled && !_isAnimationFired));

            ApplyOpacity();

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

                            series.Storyboard.Completed += delegate
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
                                                InteractivityHelper.ApplyBorderEffect(shape, (BorderStyles)dataPoint.BorderStyle, dataPoint.BorderThickness.Left, dataPoint.BorderColor);
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
            }

            if (series.Count == 1)
            {   
                if (series[0].RenderAs == RenderAs.CandleStick)
                {   
                    if (series[0].PriceUpColor == null)
                    {
                        series[0].IsNotificationEnable = false;
                        series[0].PriceUpColor = _financialColorSet.GetNewColorFromColorSet();
                        series[0].IsNotificationEnable = true;
                    }
                }
                else
                {
                    if (!String.IsNullOrEmpty(series[0].ColorSet))
                    {
                        colorSet = Chart.GetColorSetByName(series[0].ColorSet);

                        if (colorSet == null)
                            throw new Exception("ColorSet named " + series[0].ColorSet + " is not found.");
                    }
                    else if (colorSet == null)
                    {
                        throw new Exception("ColorSet named " + Chart.ColorSet + " is not found.");
                    }

                    Brush seriesColor = series[0].GetValue(DataSeries.ColorProperty) as Brush;

                    if (!Chart.UniqueColors || series[0].RenderAs == RenderAs.Area || series[0].RenderAs == RenderAs.Line || series[0].RenderAs == RenderAs.StackedArea || series[0].RenderAs == RenderAs.StackedArea100)
                    {   
                        if (seriesColor == null)
                            series[0]._internalColor = colorSet.GetNewColorFromColorSet();
                        else
                            series[0]._internalColor = seriesColor;

                        colorSet.ResetIndex();

                        foreach (DataPoint dp in series[0].DataPoints)
                        {
                            dp.IsNotificationEnable = false;

                            Brush dPColor = dp.GetValue(DataPoint.ColorProperty) as Brush;

                            if (dPColor == null)
                                if (!Chart.UniqueColors)
                                    dp._internalColor = series[0]._internalColor;
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
                        series[0]._internalColor = null;

                        foreach (DataPoint dp in series[0].DataPoints)
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
            else if (series.Count > 1)
            {   
                ColorSet colorSet4MultiSeries = null;
                Boolean FLAG_UNIQUE_COLOR_4_EACH_DP = false; // Unique color for each DataPoint
                Brush seriesColor = null;

                if (colorSet != null)
                    colorSet.ResetIndex();

                foreach (DataSeries ds in series)
                {
                    if (ds.RenderAs == RenderAs.CandleStick)
                    {   
                        if (ds.PriceUpColor == null)
                        {   
                            ds.IsNotificationEnable = false;
                                ds.PriceUpColor = _financialColorSet.GetNewColorFromColorSet();
                            ds.IsNotificationEnable = true;
                        }
                    }
                    else
                    {   
                        colorSet4MultiSeries = colorSet;
                        FLAG_UNIQUE_COLOR_4_EACH_DP = false;

                        if (!String.IsNullOrEmpty(ds.ColorSet))
                        {
                            colorSet4MultiSeries = Chart.GetColorSetByName(ds.ColorSet);

                            if (colorSet4MultiSeries == null)
                                throw new Exception("ColorSet named " + ds.ColorSet + " is not found.");

                            FLAG_UNIQUE_COLOR_4_EACH_DP = true;
                        }
                        else if (colorSet4MultiSeries == null)
                        {
                            throw new Exception("ColorSet named " + Chart.ColorSet + " is not found.");
                        }

                        if (ds.RenderAs == RenderAs.Area || ds.RenderAs == RenderAs.Line || ds.RenderAs == RenderAs.StackedArea || ds.RenderAs == RenderAs.StackedArea100)
                        {
                            seriesColor = colorSet4MultiSeries.GetNewColorFromColorSet();

                            Brush DataSeriesColor = ds.GetValue(DataSeries.ColorProperty) as Brush;

                            if (DataSeriesColor == null)
                            {   
                                ds._internalColor = seriesColor;
                            }
                            else
                                ds._internalColor = DataSeriesColor;

                            foreach (DataPoint dp in ds.DataPoints)
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
                            if (!FLAG_UNIQUE_COLOR_4_EACH_DP)
                                seriesColor = colorSet4MultiSeries.GetNewColorFromColorSet();

                            foreach (DataPoint dp in ds.DataPoints)
                            {
                                dp.IsNotificationEnable = false;
                                Brush dPColor = dp.GetValue(DataPoint.ColorProperty) as Brush;
                                if (dPColor == null)
                                {
                                    // If unique color for each DataPoint
                                    if (FLAG_UNIQUE_COLOR_4_EACH_DP)
                                    {
                                        dp._internalColor = colorSet4MultiSeries.GetNewColorFromColorSet();
                                        ds._internalColor = null;
                                    }
                                    else
                                    {
                                        Brush DataSeriesColor = ds.GetValue(DataSeries.ColorProperty) as Brush;
                                        if (DataSeriesColor == null)
                                            ds._internalColor = seriesColor;
                                        else
                                            ds._internalColor = DataSeriesColor;

                                        dp.IsNotificationEnable = true;

                                        break;
                                    }
                                }
                                else
                                    dp._internalColor = dPColor;

                                dp.IsNotificationEnable = true;
                            }
                        }

                        ds.IsNotificationEnable = true;
                    }
                }
            }

            if (colorSet != null)
                colorSet.ResetIndex();
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
                    if (dataPoint.Parent.RenderAs == RenderAs.Line)
                        markerColor = Graphics.GetLightingEnabledBrush(markerColor, "Linear", new Double[] { 0.65, 0.55 });
                }

                Boolean markerBevel;
                if ((dataPoint.Parent as DataSeries).RenderAs == RenderAs.Point
                    || (dataPoint.Parent as DataSeries).RenderAs == RenderAs.Stock
                    || (dataPoint.Parent as DataSeries).RenderAs == RenderAs.CandleStick
                    || (dataPoint.Parent as DataSeries).RenderAs == RenderAs.Bubble
                    || (dataPoint.Parent as DataSeries).RenderAs == RenderAs.Pie || (dataPoint.Parent as DataSeries).RenderAs == RenderAs.Doughnut
                    || (dataPoint.Parent as DataSeries).RenderAs == RenderAs.Line)
                {
                    markerBevel = false;
                }   
                else
                    markerBevel = Chart.View3D ? false : dataPoint.Parent.Bevel ? dataPoint.Parent.Bevel : false;

                Size markerSize;
                if (dataPoint.Parent.RenderAs == RenderAs.Line)
                    markerSize = new Size(8, 8);
                else
                    markerSize = new Size(8, 8);

                dataPoint.LegendMarker = new Marker(
                        ((dataPoint.Parent.RenderAs == RenderAs.Line) ? (MarkerTypes)dataPoint.MarkerType : RenderAsToMarkerType(dataPoint.Parent.RenderAs, dataPoint.Parent)),
                        1,
                        markerSize,
                        markerBevel,
                        markerColor,
                        ""
                        );

                if ((dataPoint.Parent.RenderAs == RenderAs.Line || dataPoint.Parent.RenderAs == RenderAs.Stock || dataPoint.Parent.RenderAs == RenderAs.CandleStick) && dataPoint.MarkerEnabled == false)
                {
                    dataPoint.LegendMarker.Opacity = 0;
                }

                                
                dataPoint.LegendMarker.DataSeriesOfLegendMarker = dataPoint.Parent;

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
                        
                        if(String.IsNullOrEmpty(dataSeries.LegendText))
                        {
                            if(dataSeries._isAutoName)
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
                            if (dataSeries.RenderAs == RenderAs.Line)
                                markerColor = Graphics.GetLightingEnabledBrush(markerColor, "Linear", new Double[] { 0.65, 0.55 });
                        }

                        Boolean markerBevel;

                        if (dataSeries.RenderAs == RenderAs.Point
                            || dataSeries.RenderAs == RenderAs.Stock
                            || dataSeries.RenderAs == RenderAs.CandleStick
                            || dataSeries.RenderAs == RenderAs.Bubble
                            || dataSeries.RenderAs == RenderAs.Line)
                        {
                            markerBevel = false;
                        }
                        else
                            markerBevel = Chart.View3D ? false : dataSeries.Bevel ? dataSeries.Bevel : false;

                        Size markerSize;
                        if (dataSeries.RenderAs == RenderAs.Line)
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

                        if ((dataSeries.RenderAs == RenderAs.Line || dataSeries.RenderAs == RenderAs.Stock || dataSeries.RenderAs == RenderAs.CandleStick) && dataSeries.MarkerEnabled == false)
                            dataSeries.LegendMarker.Opacity = 0;

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
                                         where entry.Entries.Count > 0 && entry.VerticalAlignment == VerticalAlignment.Top
                                         && entry.DockInsidePlotArea == DockInsidePlotArea
                                         && (Boolean)entry.Enabled
                                         select entry).ToList();

            List<Legend> legendsOnBottom = (from entry in chart.Legends
                                            where entry.Entries.Count > 0
                                            && entry.VerticalAlignment == VerticalAlignment.Bottom
                                            && entry.DockInsidePlotArea == DockInsidePlotArea && (Boolean)entry.Enabled
                                            select entry).ToList();

            List<Legend> legendsOnLeft = (from entry in chart.Legends
                                          where entry.Entries.Count > 0
                                          && (entry.VerticalAlignment == VerticalAlignment.Center ||
                                          entry.VerticalAlignment == VerticalAlignment.Stretch)
                                          && entry.HorizontalAlignment == HorizontalAlignment.Left
                                          && entry.DockInsidePlotArea == DockInsidePlotArea && (Boolean)entry.Enabled
                                          select entry).ToList();

            List<Legend> legendsOnRight = (from entry in chart.Legends
                                           where entry.Entries.Count > 0
                                           && (entry.VerticalAlignment == VerticalAlignment.Center ||
                                           entry.VerticalAlignment == VerticalAlignment.Stretch)
                                           && entry.HorizontalAlignment == HorizontalAlignment.Right
                                           && entry.DockInsidePlotArea == DockInsidePlotArea
                                           && (Boolean)entry.Enabled
                                           select entry).ToList();

            List<Legend> legendsAtCenter = (from entry in chart.Legends
                                            where entry.Entries.Count > 0
                                            && (entry.VerticalAlignment == VerticalAlignment.Center ||
                                            entry.VerticalAlignment == VerticalAlignment.Stretch)
                                            && (entry.HorizontalAlignment == HorizontalAlignment.Center ||
                                            entry.HorizontalAlignment == HorizontalAlignment.Stretch)
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
                        if (Double.IsPositiveInfinity(legend.MaxWidth))
                            legend.InternalMaximumWidth = Width - Chart.BorderThickness.Left - Chart.BorderThickness.Right - chart.Padding.Left - chart.Padding.Right;
                        else
                        {
                            if (legend.MaxWidth > Width - Chart.BorderThickness.Left - Chart.BorderThickness.Right - chart.Padding.Left - chart.Padding.Right)
                                legend.InternalMaximumWidth = Width - Chart.BorderThickness.Left - Chart.BorderThickness.Right - chart.Padding.Left - chart.Padding.Right;
                            else
                                legend.InternalMaximumWidth = legend.MaxWidth;
                        }

                        if (Double.IsPositiveInfinity(legend.MaxHeight))
                            legend.InternalMaximumHeight = Double.PositiveInfinity;
                        else
                        {
                            if (legend.MaxHeight > Height - Chart.BorderThickness.Top - Chart.BorderThickness.Bottom - chart.Padding.Top - chart.Padding.Bottom)
                                legend.InternalMaximumHeight = Height - Chart.BorderThickness.Top - Chart.BorderThickness.Bottom - chart.Padding.Top - chart.Padding.Bottom;
                            else
                                legend.InternalMaximumHeight = legend.MaxHeight;
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
                        if (Double.IsPositiveInfinity(legend.MaxWidth))
                            legend.InternalMaximumWidth = Width - Chart.BorderThickness.Left - Chart.BorderThickness.Right - chart.Padding.Left - chart.Padding.Right;
                        else
                        {
                            if(legend.MaxWidth > Width - Chart.BorderThickness.Left - Chart.BorderThickness.Right - chart.Padding.Left - chart.Padding.Right)
                                legend.InternalMaximumWidth = Width - Chart.BorderThickness.Left - Chart.BorderThickness.Right - chart.Padding.Left - chart.Padding.Right;
                            else
                                legend.InternalMaximumWidth = legend.MaxWidth;
                        }

                        if (Double.IsPositiveInfinity(legend.MaxHeight))
                            legend.InternalMaximumHeight = Double.PositiveInfinity;
                        else
                        {
                            if (legend.MaxHeight > Height - Chart.BorderThickness.Top - Chart.BorderThickness.Bottom - chart.Padding.Top - chart.Padding.Bottom)
                                legend.InternalMaximumHeight = Height - Chart.BorderThickness.Top - Chart.BorderThickness.Bottom - chart.Padding.Top - chart.Padding.Bottom;
                            else
                                legend.InternalMaximumHeight = legend.MaxHeight;
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
                        if (Double.IsPositiveInfinity(legend.MaxHeight))
                            legend.InternalMaximumHeight = Height - Chart.BorderThickness.Top - Chart.BorderThickness.Bottom - chart.Padding.Top - chart.Padding.Bottom;
                        else
                        {
                            if (legend.MaxHeight > Height - Chart.BorderThickness.Top - Chart.BorderThickness.Bottom - chart.Padding.Top - chart.Padding.Bottom)
                                legend.InternalMaximumHeight = Height - Chart.BorderThickness.Top - Chart.BorderThickness.Bottom - chart.Padding.Top - chart.Padding.Bottom;
                            else
                                legend.InternalMaximumHeight = legend.MaxHeight;
                        }

                        if (Double.IsPositiveInfinity(legend.MaxWidth))
                            legend.InternalMaximumWidth = Double.PositiveInfinity;
                        else
                        {
                            if (legend.MaxWidth > Width - Chart.BorderThickness.Left - Chart.BorderThickness.Right - chart.Padding.Left - chart.Padding.Right)
                                legend.InternalMaximumWidth = Width - Chart.BorderThickness.Left - Chart.BorderThickness.Right - chart.Padding.Left - chart.Padding.Right;
                            else
                                legend.InternalMaximumWidth = legend.MaxWidth;
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
                        if (Double.IsPositiveInfinity(legend.MaxHeight))
                            legend.InternalMaximumHeight = Height - Chart.BorderThickness.Top - Chart.BorderThickness.Bottom - chart.Padding.Top - chart.Padding.Bottom;
                        else
                        {
                            if (legend.MaxHeight > Height - Chart.BorderThickness.Top - Chart.BorderThickness.Bottom - chart.Padding.Top - chart.Padding.Bottom)
                                legend.InternalMaximumHeight = Height - Chart.BorderThickness.Top - Chart.BorderThickness.Bottom - chart.Padding.Top - chart.Padding.Bottom;
                            else
                                legend.InternalMaximumHeight = legend.MaxHeight;
                        }

                        if (Double.IsPositiveInfinity(legend.MaxWidth))
                            legend.InternalMaximumWidth = Double.PositiveInfinity;
                        else
                        {
                            if (legend.MaxWidth > Width - Chart.BorderThickness.Left - Chart.BorderThickness.Right - chart.Padding.Left - chart.Padding.Right)
                                legend.InternalMaximumWidth = Width - Chart.BorderThickness.Left - Chart.BorderThickness.Right - chart.Padding.Left - chart.Padding.Right;
                            else
                                legend.InternalMaximumWidth = legend.MaxWidth;
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

                    if (Double.IsPositiveInfinity(legend.MaxWidth)) // legend.MaximumWidth == 0
                        legend.InternalMaximumWidth = Width * 60 / 100;
                    else
                    {
                        if (legend.MaxWidth > Width * 60 / 100)
                            legend.InternalMaximumWidth = Width * 60 / 100;
                        else
                            legend.InternalMaximumWidth = legend.MaxWidth;
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
                              where (title.VerticalAlignment == VerticalAlignment.Top && title.Enabled == true)
                              select title;

            // Add Title on the top of the ChartArea
            foreach (Title title in titlesOnTop)
                this.AddTitle(chart, title, topTitlePanel, width, height);

            // Get Titles on the bottom of the ChartArea using LINQ
            var titlesOnBottom = from title in titles
                                 where (title.VerticalAlignment == VerticalAlignment.Bottom && title.Enabled == true)
                                 select title;

            titlesOnBottom.Reverse();

            // Add Title on the bottom of the ChartArea
            foreach (Title title in titlesOnBottom)
                this.AddTitle(chart, title, bottomTitlePanel, width, height);

            // Get Titles on the left of the ChartArea using LINQ
            var titlesAtLeft = from title in titles
                               where ((title.VerticalAlignment == VerticalAlignment.Center || title.VerticalAlignment == VerticalAlignment.Stretch) 
                               && title.HorizontalAlignment == HorizontalAlignment.Left 
                               && title.Enabled == true)
                               select title;

            if (titlesAtLeft.Count() > 0)
                isHLeftOrRightVCenterTitlesExists = true;

            // Add Title on left of the ChartArea
            foreach (Title title in titlesAtLeft)
                this.AddTitle(chart, title, leftTitlePanel, width, height);


            // Get Titles on the right of the ChartArea using LINQ
            var titlesAtRight = from title in titles
                                where ((title.VerticalAlignment == VerticalAlignment.Center || title.VerticalAlignment == VerticalAlignment.Stretch) 
                                && title.HorizontalAlignment == HorizontalAlignment.Right 
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
                                 where ((title.HorizontalAlignment == HorizontalAlignment.Center || title.HorizontalAlignment == HorizontalAlignment.Stretch) 
                                 && (title.VerticalAlignment == VerticalAlignment.Center || title.VerticalAlignment == VerticalAlignment.Stretch) 
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
        private Size SetChartAreaCenterGridMargin(Size newSize)
        {
            Double left = GetChartAreaCenterGridLeftMargin();
            Double right = GetChartAreaCenterGridRightMargin() + ((Chart.PlotArea.ShadowEnabled) ? Chart.SHADOW_DEPTH : 0);
            Double top = GetChartAreaCenterGridTopMargin();
            Double bottom = GetChartAreaCenterGridBottomMargin();

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
            AxisY = PlotDetails.GetAxisYFromChart(Chart, AxisTypes.Primary);
            AxisY2 = PlotDetails.GetAxisYFromChart(Chart, AxisTypes.Secondary);

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
                            if (ds.RenderAs == RenderAs.CandleStick || ds.RenderAs == RenderAs.Stock)
                            {
                                dp.Faces.Visual.Opacity = ds.Opacity * dp.Opacity;
                                if (dp.LabelVisual != null)
                                    dp.LabelVisual.Opacity = ds.Opacity * dp.Opacity;
                            }
                            else if (dp.Faces.VisualComponents.Count != 0)
                            {   
                                foreach (FrameworkElement face in dp.Faces.VisualComponents)
                                {
                                    face.Opacity = dp.Opacity * ds.Opacity;
                                }
                            }
                            else
                                dp.Faces.Visual.Opacity = ds.Opacity * dp.Opacity;
                        }
                        else if(dp.Faces.Visual != null)
                            dp.Faces.Visual.Opacity = ds.Opacity * dp.Opacity;
                    }


                    if (Chart.AnimationEnabled == false || (Chart.AnimationEnabled && !_isFirstTimeRender))
                    {
                        if (dp.Marker != null && dp.Marker.Visual != null)
                            dp.Marker.Visual.Opacity = ds.Opacity * dp.Opacity;
                    }
                }
            }
        }

        /// <summary>
        /// Attach events for each DataSeries and InternalDataPoints
        /// </summary>
        private void AttachEventsToolTipHref2DataSeries()
        {
            foreach (DataSeries ds in Chart.InternalSeries)
            {
                ds.AttachEvent2DataSeriesVisualFaces();

                foreach (DataPoint dp in ds.InternalDataPoints)
                {
                    dp.AttachEvent2DataPointVisualFaces(dp);

                    #region Attach Tool Tips

                        dp._parsedToolTipText = dp.TextParser(dp.ToolTipText);
                        
                        if (dp.Faces != null)
                        {
                            if (((Chart as Chart).View3D && (ds.RenderAs == RenderAs.Pie || ds.RenderAs == RenderAs.Doughnut || ds.RenderAs == RenderAs.StreamLineFunnel))
                                || ds.RenderAs == RenderAs.StreamLineFunnel || ds.RenderAs == RenderAs.SectionFunnel || ds.RenderAs == RenderAs.Stock || ds.RenderAs == RenderAs.CandleStick)
                            {
                                dp.AttachToolTip(Chart, dp, dp.Faces.VisualComponents);
                            }
                            else
                            {
                                if (ds.RenderAs != RenderAs.Line && ds.RenderAs != RenderAs.Area && ds.RenderAs != RenderAs.StackedArea && ds.RenderAs != RenderAs.StackedArea100)
                                    dp.AttachToolTip(Chart, dp, dp.Faces.Visual);

                                if (dp.Marker != null)
                                    dp.AttachToolTip(Chart, dp, dp.Marker.Visual);
                            }
                        }

                        if (ds.RenderAs == RenderAs.Line)
                        {
                            if (dp.Marker != null)
                                dp.AttachToolTip(Chart, dp, dp.Marker.Visual);
                        }

                        if (ds.RenderAs == RenderAs.Area || ds.RenderAs == RenderAs.StackedArea || ds.RenderAs == RenderAs.StackedArea100)
                        {
                            if (dp.Marker != null)
                                dp.AttachToolTip(Chart, dp, dp.Marker.Visual);
                        }

                    #endregion

                    #region Attach Href

                    dp.SetHref2DataPointVisualFaces();

                    #endregion

                    dp.SetCursor2DataPointVisualFaces();
                }

                #region Attach ToolTip for AreaCharts

                if (ds.RenderAs == RenderAs.Area || ds.RenderAs == RenderAs.StackedArea || ds.RenderAs == RenderAs.StackedArea100)
                {
                    if (ds.Faces != null)
                    {   
                        ds.AttachAreaToolTip(Chart, ds.Faces.VisualComponents);
                    }
                }
                
                #endregion
            }
        }

        /// <summary>
        /// Creates the various regions required for drawing vertical charts
        /// </summary>
        /// <param name="chartSize">Chart size as Double</param>
        /// <param name="NewSize">NewSize</param>
        /// <returns>Size</returns>
        private Size CreateRegionsForVerticalCharts(Double chartSize, Size NewSize)
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

                    DrawHorizontalPlank(PLANK_DEPTH, PLANK_THICKNESS, NewSize.Height);

                    if (NewSize.Height - PLANK_DEPTH - PLANK_THICKNESS > 0)
                        DrawVerticalPlank(NewSize.Height - PLANK_DEPTH - PLANK_THICKNESS, PLANK_DEPTH, 0.25, plankOpacity);

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
        private Size CreateRegionsForHorizontalCharts(Double chartSize, Size NewSize)
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
                    // Draw 3D horizontal plank 
                    DrawVerticalPlank(PLANK_DEPTH, PLANK_THICKNESS);

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
        /// Event handler for loaded event of the PlottingCanvas
        /// </summary>
        /// <param name="sender">Canvas</param>
        /// <param name="e">RoutedEventArgs</param>
        private void PlottingCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            PlottingCanvas.Loaded -= new RoutedEventHandler(PlottingCanvas_Loaded);

            Chart._renderLock = false;

            if (Chart._renderLapsedCounter >= 1)
                Chart.Render();

            Animate();

            Chart._internalAnimationEnabled = false;

            if (!Chart.AnimationEnabled || Chart.IsInDesignMode || !_isFirstTimeRender)
                Visifire.Charts.Chart.SelectDataPoints(Chart);

            Chart.FireRenderedEvent();

            _isFirstTimeRender = false;

            System.Diagnostics.Debug.WriteLine("Loaded() >");
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
            Double tempFontSize = title.FontSize;
            title.Chart = chart;

        RECREATE_TITLE:

            title.CreateVisualObject();

           Size size = Graphics.CalculateVisualSize(title.Visual);
            
            if (title.VerticalAlignment == VerticalAlignment.Top || title.VerticalAlignment == VerticalAlignment.Bottom
                || (title.VerticalAlignment == VerticalAlignment.Center && title.HorizontalAlignment == HorizontalAlignment.Center))
            {
                if (size.Width > width && (chart.ActualWidth - width) < width)
                {
                    if (title.FontSize == 1)
                        goto OUT;

                    title.IsNotificationEnable = false;
                    title.FontSize -= 1;
                    title.IsNotificationEnable = true;
                    goto RECREATE_TITLE;
                }
            }
            else
            {
                if (size.Height >= height || title.Height >= height)
                {
                    if (title.FontSize == 1)
                        goto OUT;

                    title.IsNotificationEnable = false;
                    title.FontSize -= 1;
                    title.IsNotificationEnable = true;
                    goto RECREATE_TITLE;
                }
            }
        OUT:

            title.IsNotificationEnable = false;
            title.FontSize = tempFontSize;
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
        /// Grid animation duration
        /// </summary>
        internal static Double GRID_ANIMATION_DURATION = 1;             

        /// <summary>
        /// Chart scroll-viewer Offset for horizontal chart. 
        /// It is used to show the grid lines properly at the right hand side of the scroll-viewer
        /// </summary>
        internal static Double SCROLLVIEWER_OFFSET4HORIZONTAL_CHART = 1;            
        
        /// <summary>
        /// Whether animation is fired for the first time
        /// </summary>
        internal Boolean _isAnimationFired = false;

        /// <summary>
        /// Whether it is the first time render of the chart
        /// </summary>
        internal bool _isFirstTimeRender = true;

        internal ColorSet _financialColorSet = null;

        #region "Used for testing purpose only"

        /// <summary>
        /// Number of redrawing chart for a single render call 
        /// (Used for Testing Only)
        /// </summary>
        internal Int32 _renderCount = 0;

        #endregion "Used for Testing Only"

        #endregion
    }
}
