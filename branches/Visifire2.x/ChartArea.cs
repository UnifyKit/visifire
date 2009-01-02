#if WPF

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Markup;
using System.IO;
using System.Xml;


#else
using System;
using System.Windows;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using System.Collections.Generic;

#endif

using Visifire.Commons;

namespace Visifire.Charts
{
    /// <summary>
    /// ChartArea, the maximum area available for drawing a chart in Visifire Chart Control
    /// </summary>
    internal class ChartArea
    {
        #region Public Methods

        public ChartArea(Chart chart)
        {
            // Save the chart reference
            Chart = chart;
        }

        public void Draw(Chart chart)
        {
            _redrawCount = 0;
            _renderCount = 0;

            Chart = chart;

            chart._bottomOuterLegendPanel.Height = 0;

            Chart.IsRenderCallAllowed = false;

            SetSeriesStyleFromTheme(chart);

            SetTitleStyleFromTheme(chart);

            SetDataPointColorFromColorSet(chart, chart.Series);

            CloneAxisX();
            CloneAxisY();
            CloneDataSeries();

            System.Diagnostics.Debug.WriteLine("______________PlotDetails______");

            foreach (Legend legend in chart.Legends)
            {
                legend.Visual = null;
            }

            // Generate plot details
            PlotDetails = new PlotDetails(chart);

            SetLegendStyleFromTheme(chart);

            // Calculate 3d plank paramaters if required
            CalculatePlankParameters();

            chart._centerDockInsidePlotAreaPanel.Children.Clear();

            // Add all the titles to chart of type dock outside
            this.AddTitles(chart, false, chart.ActualHeight, chart.ActualWidth);

            Size legendMaxSize = CalculateLegendMaxSize(false);

            // Add all the legends to chart of type dock outside
            this.AddLegends(chart, false, legendMaxSize.Height, legendMaxSize.Width);

            // create the center layout of the chart 
            this.CreatePlotArea(chart);

            Chart.IsRenderCallAllowed = true;

            Chart._leftAxisScrollBar.Visibility = Visibility.Collapsed;
            Chart._rightAxisScrollBar.Visibility = Visibility.Collapsed;
            Chart._bottomAxisScrollBar.Visibility = Visibility.Collapsed;
            Chart._topAxisScrollBar.Visibility = Visibility.Collapsed;

            // Check if drawing axis is necessary or not
            if (PlotDetails.ChartOrientation != ChartOrientationType.NoAxis)
            {
                // Add the required axis to the chart
                this.AddAxis(chart);
            }

            RedrawChart(new Size(PlotAreaCanvas.Width, PlotAreaCanvas.Height), new Size(0, 0));
        }

        /// <summary>
        /// Get outerbounds for the PlotArea
        /// </summary>
        /// <returns></returns>
        public Size GetBundingRectangle()
        {
            Size boundingRec = new Size(Chart._chartAreaGrid.ActualWidth, Chart._chartAreaGrid.ActualHeight);

            System.Diagnostics.Debug.WriteLine("chartAreaGrid Size:" + boundingRec.ToString());

            foreach (Title title in Chart.Titles)
            {
                if (title.DockInsidePlotArea || title.Visual == null)
                    continue;

                Size size = Graphics.CalculateVisualSize(title.Visual);

                if (title.VerticalAlignment == VerticalAlignment.Bottom || title.VerticalAlignment == VerticalAlignment.Top)
                {
                    boundingRec.Height -= size.Height;
                }
                else if (title.VerticalAlignment == VerticalAlignment.Center)
                {
                    if (title.HorizontalAlignment == HorizontalAlignment.Left || title.HorizontalAlignment == HorizontalAlignment.Right)
                    {
                        boundingRec.Width -= size.Width;
                    }
                }
            }

            Chart._leftOuterLegendPanel.Width = 0;
            Chart._rightOuterLegendPanel.Width = 0;
            Chart._bottomOuterLegendPanel.Height = 0;
            Chart._topOuterLegendPanel.Height = 0;

            foreach (Legend legend in Chart.Legends)
            {
                if (legend.DockInsidePlotArea || legend.Visual == null)
                    continue;

                Size size = Graphics.CalculateVisualSize(legend.Visual);

                if (legend.VerticalAlignment == VerticalAlignment.Bottom || legend.VerticalAlignment == VerticalAlignment.Top)
                {
                    boundingRec.Height -= size.Height;
                    if (legend.VerticalAlignment == VerticalAlignment.Bottom)
                        Chart._bottomOuterLegendPanel.Height += size.Height;
                    else
                        Chart._topOuterLegendPanel.Height += size.Height;
                }
                else if (legend.VerticalAlignment == VerticalAlignment.Center)
                {   
                    if (legend.HorizontalAlignment == HorizontalAlignment.Left || legend.HorizontalAlignment == HorizontalAlignment.Right)
                    {
                        boundingRec.Width -= size.Width;
                        if(legend.HorizontalAlignment  == HorizontalAlignment.Left)
                            Chart._leftOuterLegendPanel.Width = +size.Width;
                        else
                            Chart._rightOuterLegendPanel.Width = + size.Width;
                    }
                }
            }

            //if (Chart.Bevel)
            //{
            //    boundingRec.Height -= 2*Chart.BEVEL_DEPTH;
            //    boundingRec.Width -= 2*Chart.BEVEL_DEPTH;
            //}

            Chart._centerOuterGrid.Height = boundingRec.Height;
            Chart._centerOuterGrid.Width = boundingRec.Width;

            return boundingRec;
        }
    
        /// <summary>
        /// Create the center layout of the ChartArea
        /// </summary>
        /// <returns>Grid</returns>
        private void CreatePlotArea(Chart chart)
        {
            if (Chart.PlotArea == null)
            {
                Chart.PlotArea = new PlotArea();
                Chart.PlotArea.CreateVisual();
                PlotAreaCanvas = Chart.PlotArea.Visual;
            }
            else
            {
                if (Chart.PlotArea.Visual == null)
                {
                    Chart.PlotArea.CreateVisual();
                    PlotAreaCanvas = Chart.PlotArea.Visual;
                }
                else
                    Chart.PlotArea.UpdateProperties();
            }

            Chart.PlotArea.Chart = Chart;

            if (Chart.PlotArea.ShadowEnabled)
            {
                Chart._plotAreaGrid.Margin = (Chart.View3D) ? new Thickness(0, 0, 4, 0) : new Thickness(0, 0, 4, 4);
            }
            else
            {
                Chart._plotAreaGrid.Margin = new Thickness(0, 0, 0, 0);
            }

            if (!String.IsNullOrEmpty(Chart.Theme))
                Chart.PlotArea.ApplyStyleFromTheme(Chart, "PlotArea");

            Size boundingRec = GetBundingRectangle();

            PlotAreaCanvas.Height = boundingRec.Height; 
            PlotAreaCanvas.Width = boundingRec.Width; 

            System.Diagnostics.Debug.WriteLine("boundingRec=" + boundingRec.ToString());

            PlotAreaCanvas.HorizontalAlignment = HorizontalAlignment.Stretch;
            PlotAreaCanvas.VerticalAlignment = VerticalAlignment.Stretch;
            PlotAreaCanvas.Margin = new Thickness(0);

            ObservableObject.AttachHref(Chart, PlotAreaCanvas.Children[0] as Border, Chart.PlotArea.Href, Chart.PlotArea.HrefTarget);
            ObservableObject.AttachToolTip(Chart, PlotAreaCanvas.Children[0] as Border, Chart.PlotArea.ToolTipText);
            Chart.AttachEvents2Visual(Chart.PlotArea, PlotAreaCanvas.Children[0] as Border);

            if (!chart._drawingCanvas.Children.Contains(PlotAreaCanvas))
                chart._drawingCanvas.Children.Add(PlotAreaCanvas);
        }

        private Size CalculateLegendMaxSize(Boolean DockInsidePlotArea)
        {   
            Size retVal = new Size(0, 0);

            foreach (Title title in Chart.Titles)
            {
                if (title.DockInsidePlotArea == DockInsidePlotArea)
                {
                    if ((title.VerticalAlignment == VerticalAlignment.Top || title.VerticalAlignment == VerticalAlignment.Bottom))
                    {
                        retVal.Height += title.Height;
                    }
                    else
                    {
                        retVal.Width += title.Width;
                    }
                }
            }

            if (DockInsidePlotArea)
            {
                retVal.Height = PlotAreaCanvas.Height - retVal.Height;
                retVal.Width = PlotAreaCanvas.ActualWidth - retVal.Width;
            }
            else
            {
                retVal.Height = Chart.ActualHeight - retVal.Height;
                retVal.Width = Chart.ActualWidth - retVal.Width;
            }

            return retVal;
        }

        private void CloneAxisX()
        {
            if (Chart.InternalAxesX != null)
                Chart.InternalAxesX.Clear();

            Chart.InternalAxesX = Chart.AxesX.ToList();
        }

        private void CloneAxisY()
        {
            if (Chart.InternalAxesY != null)
                Chart.InternalAxesY.Clear();

            Chart.InternalAxesY = Chart.AxesY.ToList();
        }

        private void CloneDataSeries()
        {
            if (Chart.InternalSeries != null)
                Chart.InternalSeries.Clear();

            if (Chart.Series.Count == 0)
            {
                Chart.InternalSeries = new List<DataSeries>();

                if (Chart.IsInDesignMode)
                {
                    AddDefaultChart4Blend();
                    SetDataPointColorFromColorSet(Chart, Chart.InternalSeries);
                }
                else
                    SetBlankSeries();
            }
            else
                Chart.InternalSeries = Chart.Series.ToList();
        }

        internal void AddDefaultChart4Blend()
        {
            DataSeries ds = new DataSeries();
            ds.RenderAs = RenderAs.Column;
            ds.LightingEnabled = true;
            ds.ShadowEnabled = true;
            ds.Chart = Chart;

            DataPoint dp = new DataPoint();
            dp.XValue = 1;
            dp.YValue = 70;
            dp.AxisXLabel = "Wall-Mart";
            dp.Chart = Chart;

            ds.DataPoints.Add(dp);

            dp = new DataPoint();
            dp.XValue = 2;
            dp.YValue = 40;
            dp.AxisXLabel = "Exxon Mobil";
            dp.Chart = Chart;
            ds.DataPoints.Add(dp);

            dp = new DataPoint();
            dp.XValue = 3;
            dp.YValue = 60;
            dp.AxisXLabel = "Shell";
            dp.Chart = Chart;
            ds.DataPoints.Add(dp);

            dp = new DataPoint();
            dp.XValue = 4;
            dp.YValue = 27;
            dp.AxisXLabel = "BP";
            dp.Chart = Chart;
            ds.DataPoints.Add(dp);

            dp = new DataPoint();
            dp.XValue = 5;
            dp.YValue = 54;
            dp.AxisXLabel = "General Motors";
            dp.Chart = Chart;
            ds.DataPoints.Add(dp);

            Chart.InternalSeries.Add(ds);
        }

        /// <summary>
        /// Calculates the depth and height of the plank
        /// </summary>
        private void CalculatePlankParameters()
        {
            if (Chart.View3D)
            {
                if (PlotDetails.ChartOrientation == ChartOrientationType.Vertical)
                {
                    Double horizontalDepthFactor = 0.04;
                    Double horizontalThicknessFactor = 0.03;

                    PlankDepth = (Chart.ActualHeight > Chart.ActualWidth ? Chart.ActualWidth : Chart.ActualHeight) * (horizontalDepthFactor * (PlotDetails.Layer3DCount == 0 ? 1 : PlotDetails.Layer3DCount));
                    PlankThickness = (Chart.ActualHeight > Chart.ActualWidth ? Chart.ActualWidth : Chart.ActualHeight) * (horizontalThicknessFactor);
                }
                else
                {
                    Double verticalDepthFactor = 0.015;
                    Double verticalThicknessFactor = 0.025;

                    PlankDepth = (Chart.ActualHeight > Chart.ActualWidth ? Chart.ActualWidth : Chart.ActualHeight) * (verticalDepthFactor * PlotDetails.Layer3DCount);
                    PlankThickness = (Chart.ActualHeight > Chart.ActualWidth ? Chart.ActualWidth : Chart.ActualHeight) * (verticalThicknessFactor);
                }

                PlankDepth = Double.IsNaN(PlankDepth) ? 12 : PlankDepth;
                PlankThickness = Double.IsNaN(PlankThickness) ? 5 : PlankThickness;

                PlankOffset = PlankDepth + PlankThickness;
            }
            else
            {
                PlankDepth = 0;
                PlankThickness = 0;
                PlankOffset = 0;
            }
        }

        private void ApplyPlotAreaBevel()
        {
            Canvas bevelCanvas = ExtendedGraphics.Get2DRectangleBevel(Chart.PlotArea.PlotAreaBorderElement.Width - Chart.PlotArea.BorderThickness.Left - Chart.PlotArea.BorderThickness.Right - PlankDepth
                , Chart.PlotArea.PlotAreaBorderElement.Height - Chart.PlotArea.BorderThickness.Top - Chart.PlotArea.BorderThickness.Bottom - PlankDepth - PlankThickness
                , Chart.BEVEL_DEPTH, Chart.BEVEL_DEPTH
                , Graphics.GetBevelTopBrush(Chart.PlotArea.PlotAreaBorderElement.Background)
                , Graphics.GetBevelSideBrush(0, Chart.PlotArea.PlotAreaBorderElement.Background)
                , Graphics.GetBevelSideBrush(180, Chart.PlotArea.PlotAreaBorderElement.Background)
                , Graphics.GetBevelSideBrush(90, Chart.PlotArea.PlotAreaBorderElement.Background));

            bevelCanvas.SetValue(Canvas.LeftProperty, Chart.PlotArea.BorderThickness.Left);
            bevelCanvas.SetValue(Canvas.TopProperty, Chart.PlotArea.BorderThickness.Top);

            bevelCanvas.IsHitTestVisible = false;
            Chart.PlotArea.BevelGrid.Children.Clear();
            Chart.PlotArea.BevelGrid.Children.Add(bevelCanvas);
        }

        /// <summary>
        /// Apply shadow for PlotArea
        /// </summary>
        private void ApplyPlotAreaShadow()
        {
            Chart._plotAreaShadowCanvas.Children.Clear();

            if (Chart.PlotArea.ShadowEnabled)
            {
                if (Chart.View3D)
                {
                    Chart.PlotArea.ShadowGrid = ExtendedGraphics.Get2DRectangleShadow(Chart._plotAreaScrollViewer.ActualWidth + Chart.SHADOW_DEPTH - PlankOffset
                        , Chart.PlotArea.PlotAreaBorderElement.Height - PlankDepth - PlankThickness
                        , Chart.PlotArea.CornerRadius
                        , Chart.PlotArea.CornerRadius, 6);

                    PathGeometry pg = new PathGeometry();
                    PathFigure pf = new PathFigure();
                    pg.Figures.Add(pf);
                    pf.Segments.Add(new LineSegment()
                    {
                        Point = new Point(0, Chart.PlotArea.ShadowGrid.Height - Chart.SHADOW_DEPTH)
                    });
                    pf.Segments.Add(new LineSegment()
                    {
                        Point = new Point(Chart.PlotArea.ShadowGrid.Width - Chart.SHADOW_DEPTH, Chart.PlotArea.ShadowGrid.Height - Chart.SHADOW_DEPTH)
                    });
                    pf.Segments.Add(new LineSegment()
                    {
                        Point = new Point(Chart.PlotArea.ShadowGrid.Width - Chart.SHADOW_DEPTH, 0)
                    });
                    pf.Segments.Add(new LineSegment() { Point = new Point(Chart.PlotArea.ShadowGrid.Width, 0) });
                    pf.Segments.Add(new LineSegment()
                    {
                        Point = new Point(Chart.PlotArea.ShadowGrid.Width, Chart.PlotArea.ShadowGrid.Height)
                    });
                    pf.Segments.Add(new LineSegment() { Point = new Point(0, Chart.PlotArea.ShadowGrid.Height) });
                    pf.Segments.Add(new LineSegment()
                    {
                        Point = new Point(0, Chart.PlotArea.ShadowGrid.Height - Chart.SHADOW_DEPTH)
                    });

                    pg.FillRule = FillRule.EvenOdd;
                    Chart.PlotArea.ShadowGrid.Clip = pg;
                    Chart.PlotArea.ShadowGrid.SetValue(Canvas.LeftProperty, PlankOffset);

                }
                else
                {
                    Chart.PlotArea.ShadowGrid = ExtendedGraphics.Get2DRectangleShadow(Chart._plotAreaScrollViewer.ActualWidth + Chart.SHADOW_DEPTH
                        , Chart._plotCanvas.ActualHeight - PlankOffset, Chart.PlotArea.CornerRadius
                        , Chart.PlotArea.CornerRadius
                        , 6);

                    PathGeometry pg = new PathGeometry();
                    PathFigure pf = new PathFigure();
                    pg.Figures.Add(pf);
                    pf.Segments.Add(new LineSegment()
                    {
                        Point = new Point(0, Chart.PlotArea.ShadowGrid.Height - Chart.SHADOW_DEPTH)
                    });
                    pf.Segments.Add(new LineSegment()
                    {
                        Point = new Point(Chart.PlotArea.ShadowGrid.Width - Chart.SHADOW_DEPTH, Chart.PlotArea.ShadowGrid.Height - Chart.SHADOW_DEPTH)
                    });
                    pf.Segments.Add(new LineSegment()
                    {
                        Point = new Point(Chart.PlotArea.ShadowGrid.Width - Chart.SHADOW_DEPTH, 0)
                    });
                    pf.Segments.Add(new LineSegment() { Point = new Point(Chart.PlotArea.ShadowGrid.Width, 0) });
                    pf.Segments.Add(new LineSegment()
                    {
                        Point = new Point(Chart.PlotArea.ShadowGrid.Width, Chart.PlotArea.ShadowGrid.Height)
                    });
                    pf.Segments.Add(new LineSegment() { Point = new Point(0, Chart.PlotArea.ShadowGrid.Height) });
                    pf.Segments.Add(new LineSegment()
                    {
                        Point = new Point(0, Chart.PlotArea.ShadowGrid.Height - Chart.SHADOW_DEPTH)
                    });
                    pg.FillRule = FillRule.EvenOdd;
                    Chart.PlotArea.ShadowGrid.Clip = pg;
                }

                Chart.PlotArea.ShadowGrid.IsHitTestVisible = false;
                Chart._plotAreaShadowCanvas.Children.Add(Chart.PlotArea.ShadowGrid);
            }
        }

        private void SetScrollBarValues(Axis axis)
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
        }

        Double DrawAxesX(Size NewSize, Boolean AddToVisual)
        {
            Double totalHeightReduced = 0;

            if (AddToVisual)
            {
                Chart._topAxisPanel.Children.Clear();
                Chart._bottomAxisPanel.Children.Clear();
            }

            if (AxisX != null && PlotDetails.ChartOrientation == ChartOrientationType.Vertical)
            {
                AxisX.Width = NewSize.Width;
                AxisX.ScrollBarElement.Width = AxisX.Width;

                if (ChartScrollViewer != null)
                {
                    AxisX.ScrollableSize = ScrollableLength;
#if SL
                    AxisX.ScrollBarElement.Maximum = ScrollableLength - ChartScrollViewer.ViewportWidth;
                    AxisX.ScrollBarElement.ViewportSize = ChartScrollViewer.ViewportWidth;
#else                   
                    AxisX.ScrollBarElement.Maximum = ScrollableLength - ChartScrollViewer.ActualWidth;
                    AxisX.ScrollBarElement.ViewportSize = ChartScrollViewer.ActualWidth;
#endif
                }
                
                SetScrollBarValues(AxisX);
                
                AxisX.CreateVisualObject(Chart, "AxisX");

                AxisX.ScrollBarElement.Scroll -= AxesXScrollBarElement_Scroll;
                AxisX.ScrollBarElement.Scroll += new System.Windows.Controls.Primitives.ScrollEventHandler(AxesXScrollBarElement_Scroll);

                if (AxisX.Width >= AxisX.ScrollableSize || AxisX.ScrollBarElement.Maximum == 0)
                    AxisX.ScrollBarElement.Visibility = Visibility.Collapsed;
                else
                {
                    AxisX.ScrollBarElement.Visibility = Visibility.Visible;
                }

                if(AddToVisual)
                    Chart._bottomAxisPanel.Children.Add(AxisX.Visual);

                Size size = Graphics.CalculateVisualSize(Chart._bottomAxisContainer);
                totalHeightReduced += size.Height;
                                

                System.Diagnostics.Debug.WriteLine("CurrentScrollScrollBarOffset" + AxisX.CurrentScrollScrollBarOffset.ToString());
                System.Diagnostics.Debug.WriteLine("HorizontalOffset" + AxisX.ScrollViewerElement.HorizontalOffset.ToString());
                
                if((AxisX.ScrollViewerElement.Content as FrameworkElement) != null)
                    (AxisX.ScrollViewerElement.Content as FrameworkElement).Margin = new Thickness(-AxisX.CurrentScrollScrollBarOffset, 0, 0, 0);

            }
            else
                Chart._bottomAxisScrollBar.Visibility = Visibility.Collapsed;
            
            if (AxisX2 != null && PlotDetails.ChartOrientation == ChartOrientationType.Vertical)
            {
                AxisX2.Width = NewSize.Width;
                AxisX2.ScrollBarElement.Width = AxisX2.Width;

                if (ChartScrollViewer != null)
                {
                    AxisX2.ScrollableSize = ScrollableLength;
#if SL              
                    AxisX2.ScrollBarElement.Maximum = ScrollableLength - ChartScrollViewer.ViewportWidth;
                    AxisX2.ScrollBarElement.ViewportSize = ChartScrollViewer.ViewportWidth;
#else               
                    AxisX2.ScrollBarElement.Maximum = ScrollableLength - ChartScrollViewer.ActualWidth;
                    AxisX2.ScrollBarElement.ViewportSize = ChartScrollViewer.ActualWidth;
#endif              
                }

                SetScrollBarValues(AxisX2);

                AxisX2.CreateVisualObject(Chart, "AxisX");


                AxisX2.ScrollBarElement.Scroll -= AxesX2ScrollBarElement_Scroll;
                AxisX2.ScrollBarElement.Scroll += new System.Windows.Controls.Primitives.ScrollEventHandler(AxesX2ScrollBarElement_Scroll);

                if (AxisX2.Width >= AxisX2.ScrollableSize || AxisX2.ScrollBarElement.Maximum == 0)
                    AxisX2.ScrollBarElement.Visibility = Visibility.Collapsed;
                else
                {
                    AxisX2.ScrollBarElement.Visibility = Visibility.Visible;
                }

                if (AddToVisual)
                    Chart._topAxisPanel.Children.Add(AxisX2.Visual);

                Size size = Graphics.CalculateVisualSize(Chart._topAxisContainer);
                totalHeightReduced += size.Height;

                if((AxisX2.ScrollViewerElement.Content as FrameworkElement) != null)
                    (AxisX2.ScrollViewerElement.Content as FrameworkElement).Margin = new Thickness(-AxisX.CurrentScrollScrollBarOffset, 0, 0, 0);

            }
            else
                Chart._topAxisScrollBar.Visibility = Visibility.Collapsed;
            
            if (AxisY != null && PlotDetails.ChartOrientation == ChartOrientationType.Horizontal)
            {
                AxisY.Width = NewSize.Width;
                AxisY.ScrollableSize = NewSize.Width;
                AxisY.CreateVisualObject(Chart, "AxisY");

                AxisY.ScrollBarElement.Visibility = Visibility.Collapsed;

                if (AddToVisual)
                    Chart._bottomAxisPanel.Children.Add(AxisY.Visual);

                Size size = Graphics.CalculateVisualSize(Chart._bottomAxisContainer);
                totalHeightReduced += size.Height;
            }
            else
                Chart._leftAxisScrollBar.Visibility = Visibility.Collapsed;


            if (AxisY2 != null && PlotDetails.ChartOrientation == ChartOrientationType.Horizontal)
            {
                AxisY2.Width = NewSize.Width;
                AxisY2.ScrollableSize = NewSize.Width;
                AxisY2.CreateVisualObject(Chart, "AxisY");

                AxisY2.ScrollBarElement.Visibility = Visibility.Collapsed;

                if (AddToVisual)
                    Chart._topAxisPanel.Children.Add(AxisY2.Visual);

                Size size = Graphics.CalculateVisualSize(Chart._topAxisContainer);
                totalHeightReduced += size.Height;

            }
            else
                Chart._rightAxisScrollBar.Visibility = Visibility.Collapsed;

            return totalHeightReduced;
        }

       // private DrawAxisX

        Double DrawAxesY(Size NewSize, Boolean AddToVisual)
        {
            Double totalWidthReduced = 0;

            if (AddToVisual)
            {
                Chart._leftAxisPanel.Children.Clear();
                Chart._rightAxisPanel.Children.Clear();
            }

            if (AxisX != null && PlotDetails.ChartOrientation == ChartOrientationType.Horizontal)
            {
                AxisX.Height = NewSize.Height;
                AxisX.ScrollBarElement.Height = AxisX.Height;

                if (ChartScrollViewer != null)
                {
                    AxisX.ScrollableSize = ScrollableLength;
                    
#if SL
                    AxisX.ScrollBarElement.Maximum = ScrollableLength - ChartScrollViewer.ViewportHeight;
                    AxisX.ScrollBarElement.ViewportSize = ChartScrollViewer.ViewportHeight;
#else
                    AxisX.ScrollBarElement.Maximum = ScrollableLength - ChartScrollViewer.ActualHeight;
                    AxisX.ScrollBarElement.ViewportSize = ChartScrollViewer.ActualHeight;
#endif
                }

                SetScrollBarValues(AxisX);

                AxisX.CreateVisualObject(Chart, "AxisX");

                AxisX.ScrollBarElement.Height = NewSize.Height;
                AxisX.ScrollViewerElement.Height = NewSize.Height;
                AxisX.ScrollBarElement.Scroll -= AxesXScrollBarElement_Scroll;
                AxisX.ScrollBarElement.Scroll += new System.Windows.Controls.Primitives.ScrollEventHandler(AxesXScrollBarElement_Scroll);

                if (AxisX.Height >= AxisX.ScrollableSize || AxisX.ScrollBarElement.Maximum == 0)
                    AxisX.ScrollBarElement.Visibility = Visibility.Collapsed;
                else
                {
                    AxisX.ScrollBarElement.Visibility = Visibility.Visible;
                }

                if(AddToVisual)
                    Chart._leftAxisPanel.Children.Add(AxisX.Visual);

                Size size = Graphics.CalculateVisualSize(Chart._leftAxisContainer);
                totalWidthReduced += size.Width;

                if((AxisX.ScrollViewerElement.Content as FrameworkElement) != null)
                    (AxisX.ScrollViewerElement.Content as FrameworkElement).Margin = new Thickness(0, -AxisX.CurrentScrollScrollBarOffset, 0, 0);

            }

            if (AxisX2 != null && PlotDetails.ChartOrientation == ChartOrientationType.Horizontal)
            {
                AxisX2.Height = NewSize.Height;
                AxisX2.ScrollBarElement.Height = AxisX2.Height;

                if (ChartScrollViewer != null)
                {
                    AxisX2.ScrollableSize = ScrollableLength;
#if SL
                    AxisX2.ScrollBarElement.Maximum = ScrollableLength - ChartScrollViewer.ViewportHeight;
                    AxisX2.ScrollBarElement.ViewportSize = ChartScrollViewer.ViewportHeight;
#else
                    AxisX2.ScrollBarElement.Maximum = ScrollableLength - ChartScrollViewer.ActualHeight;
                    AxisX2.ScrollBarElement.ViewportSize = ChartScrollViewer.ActualHeight;
#endif
                }

                SetScrollBarValues(AxisX2);

                AxisX2.CreateVisualObject(Chart, "AxisX");

                AxisX2.ScrollBarElement.Scroll -= AxesX2ScrollBarElement_Scroll;
                AxisX2.ScrollBarElement.Scroll += new System.Windows.Controls.Primitives.ScrollEventHandler(AxesX2ScrollBarElement_Scroll);

                if (AxisX2.Height >= AxisX2.ScrollableSize || AxisX2.ScrollBarElement.Maximum == 0)
                    AxisX2.ScrollBarElement.Visibility = Visibility.Collapsed;
                else
                {
                    AxisX2.ScrollBarElement.Visibility = Visibility.Visible;
                }

                if (AddToVisual)
                    Chart._rightAxisPanel.Children.Add(AxisX2.Visual);

                Size size = Graphics.CalculateVisualSize(Chart._rightAxisContainer);
                totalWidthReduced += size.Width;

                if((AxisX.ScrollViewerElement.Content as FrameworkElement) != null)
                    (AxisX.ScrollViewerElement.Content as FrameworkElement).Margin = new Thickness(0, -AxisX.CurrentScrollScrollBarOffset, 0, 0);
            }

            if (AxisY != null && PlotDetails.ChartOrientation == ChartOrientationType.Vertical)
            {
                AxisY.Height = NewSize.Height;
                AxisY.ScrollableSize = NewSize.Height;
                AxisY.CreateVisualObject(Chart, "AxisY");

                AxisY.ScrollBarElement.Visibility = Visibility.Collapsed;

                if (AddToVisual)
                    Chart._leftAxisPanel.Children.Add(AxisY.Visual);

                Size size = Graphics.CalculateVisualSize(Chart._leftAxisContainer);
                totalWidthReduced += size.Width;
            }
            if (AxisY2 != null && PlotDetails.ChartOrientation == ChartOrientationType.Vertical)
            {
                AxisY2.Height = NewSize.Height;
                AxisY2.ScrollableSize = NewSize.Height;
                AxisY2.CreateVisualObject(Chart, "AxisY");

                AxisY2.ScrollBarElement.Visibility = Visibility.Collapsed;

                if (AddToVisual)
                    Chart._rightAxisPanel.Children.Add(AxisY2.Visual);

                Size size = Graphics.CalculateVisualSize(Chart._rightAxisContainer);
                totalWidthReduced += size.Width;

            }

            return totalWidthReduced;
        }


        void RedrawChart(Size NewSize, Size PreviousSize)
        {
            
            _redrawCount++;

            if (Chart._internalAnimationEnabled)
                foreach (DataSeries ds in Chart.InternalSeries)
                {
                    if (ds.Storyboard != null) ds.Storyboard.Stop();
                    ds.Storyboard = null;
                }

            if (Chart.PlotDetails.ChartOrientation == ChartOrientationType.Vertical)
            {
                System.Diagnostics.Debug.WriteLine("Entry NewSize=" + NewSize.ToString());

                UpdateLayoutSettings(NewSize);
                System.Diagnostics.Debug.WriteLine("UpdateLayoutSettings NewSize=" + NewSize.ToString());

                Double totalHeightReduced = DrawAxesX(NewSize, true);
                System.Diagnostics.Debug.WriteLine("DrawAxesX NewSize=" + NewSize.ToString());
                System.Diagnostics.Debug.WriteLine("totalHeightReduced=" + totalHeightReduced.ToString());

                NewSize.Height -= totalHeightReduced;
                System.Diagnostics.Debug.WriteLine("NewSize=" + NewSize.ToString());

                Double totalWidthReduced = DrawAxesY(NewSize, true);
                
                System.Diagnostics.Debug.WriteLine("totalWidthReduced=" + totalWidthReduced.ToString());
                NewSize.Width -= totalWidthReduced;
                                
                System.Diagnostics.Debug.WriteLine("by DrawAxesY NewSize=" + NewSize.ToString());

                NewSize = SetChartAreaCenterGridMargin(NewSize);
                UpdateLayoutSettings(NewSize);
                totalHeightReduced = DrawAxesX(NewSize, true);
                               
                System.Diagnostics.Debug.WriteLine("DrawAxesX NewSize=" + NewSize.ToString());
                System.Diagnostics.Debug.WriteLine("totalHeightReduced=" + totalHeightReduced.ToString());

                System.Diagnostics.Debug.WriteLine("DrawAxesY NewSize=" + NewSize.ToString());
                
                DrawAxesY(NewSize, true);
            }

            #region Horizontal Render

            else if (Chart.PlotDetails.ChartOrientation == ChartOrientationType.Horizontal)
            {
                System.Diagnostics.Debug.WriteLine("Entry NewSize=" + NewSize.ToString());

                System.Diagnostics.Debug.WriteLine("UpdateLayoutSettings NewSize=" + NewSize.ToString());

                Double totalHeightReduced = DrawAxesX(NewSize, true);
                System.Diagnostics.Debug.WriteLine("DrawAxesX NewSize=" + NewSize.ToString());
                System.Diagnostics.Debug.WriteLine("totalHeightReduced=" + totalHeightReduced.ToString());

                NewSize.Height -= totalHeightReduced;
                System.Diagnostics.Debug.WriteLine("NewSize=" + NewSize.ToString());
                UpdateLayoutSettings(NewSize);

                Double totalWidthReduced = DrawAxesY(NewSize, true);

                System.Diagnostics.Debug.WriteLine("totalWidthReduced=" + totalWidthReduced.ToString());
                NewSize.Width -= totalWidthReduced;

                System.Diagnostics.Debug.WriteLine("by DrawAxesY NewSize=" + NewSize.ToString());

                NewSize = SetChartAreaCenterGridMargin(NewSize);

                UpdateLayoutSettings(NewSize);

                NewSize.Width -= CHART_SCROLLVIEWER_OFFSET;
                totalHeightReduced = DrawAxesX(NewSize, true);
                NewSize.Width += CHART_SCROLLVIEWER_OFFSET;
                System.Diagnostics.Debug.WriteLine("DrawAxesX NewSize=" + NewSize.ToString());
                System.Diagnostics.Debug.WriteLine("totalHeightReduced=" + totalHeightReduced.ToString());

                System.Diagnostics.Debug.WriteLine("DrawAxesY NewSize=" + NewSize.ToString());
                                
                DrawAxesY(NewSize, true);
            }

            #endregion Horizontal Render
            else
            {
                 UpdateLayoutSettings(NewSize);
                 SetChartAreaCenterGridMargin(NewSize);
            }

            Render(PreviousSize, NewSize);

            Chart._centerDockOutsidePlotAreaPanel.Children.Clear();

            // Add all the titles to chart of type dock inside
            this.AddTitles(Chart, true, NewSize.Height, NewSize.Width);

            // Add all the legends to chart of type dock outside
            this.AddLegends(Chart, true, NewSize.Height, NewSize.Width);
        }


        void UpdateLayoutSettings(Size NewSize)
        {
            Chart._drawingCanvas.Height = NewSize.Height;
            Chart._drawingCanvas.Width = NewSize.Width;

            Chart.PlotArea.PlotAreaBorderElement.Height = NewSize.Height;
            Chart.PlotArea.PlotAreaBorderElement.Width = NewSize.Width;

            Double chartSize = 0;
            if (PlotDetails.ChartOrientation == ChartOrientationType.Horizontal)
                chartSize = NewSize.Height;
            else
                chartSize = NewSize.Width;

            if ((bool)Chart.ScrollingEnabled)
            {   
                Double minGap = 10;
                if (!Double.IsNaN((Double)Chart.MinimumGap) && Chart.MinimumGap > 0)
                    minGap = (Double)Chart.MinimumGap;

                if (PlotDetails.ChartOrientation != ChartOrientationType.NoAxis)
                {
                    if (Chart.InternalSeries.Count > 0)
                        chartSize = minGap * ((from series in Chart.InternalSeries select series.DataPoints.Count).Max());
                }

                if (Double.IsNaN(NewSize.Height) || NewSize.Height <= 0 || Double.IsNaN(NewSize.Width) || NewSize.Width <= 0)
                {
                    ApplyPlotAreaBevel();
                    return;
                }
                else
                {
                    if (PlotDetails.ChartOrientation == ChartOrientationType.Vertical)
                    {
                        chartSize = (chartSize < NewSize.Width) ? NewSize.Width : chartSize;
                        Chart._drawingCanvas.Width = chartSize;
                        Chart.PlotArea.PlotAreaBorderElement.Width = chartSize;
                    }
                    if (PlotDetails.ChartOrientation == ChartOrientationType.Horizontal)
                    {
                        chartSize = (chartSize < NewSize.Height) ? NewSize.Height : chartSize;
                        Chart._drawingCanvas.Height = chartSize;
                        Chart.PlotArea.PlotAreaBorderElement.Height = chartSize;
                    }
                }
            }

            ScrollableLength = chartSize;

            if (Chart.PlotArea.Bevel)
                ApplyPlotAreaBevel();
                       

            Chart._plotAreaScrollViewer.Height = NewSize.Height;
            //Chart._plotAreaScrollViewer.Width = NewSize.Width;
            Chart._plotAreaScrollViewer.UpdateLayout();
            ChartScrollViewer = Chart._plotAreaScrollViewer;
             ApplyPlotAreaShadow();

        }

        private void SetBlankSeries()
        {
            DataSeries ds = new DataSeries();
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
                dp.Color = new SolidColorBrush(Colors.Transparent);
                dp.AxisXLabel = i.ToString();
                dp.Chart = Chart;
                ds.DataPoints.Add(dp);
            }

            ds.Tag = "BlankSeries";
            Chart.InternalSeries.Add(ds);
            ds.IsNotificationEnable = true;
        }

        private void ShowLabels()
        {
            foreach (DataSeries ds in Chart.InternalSeries)
            {
                if (ds.Faces != null)
                {
                    if (ds.Faces.LabelCanvas != null)
                        ds.Faces.LabelCanvas.Opacity = 1;
                }

                foreach (DataPoint dp in ds.DataPoints)
                {
                    if (dp.Faces != null)
                    {
                        if (dp.Faces.LabelCanvas != null)
                            dp.Faces.LabelCanvas.Opacity = 1;
                    }
                }
            }
        }

        private void HideLabels()
        {
            foreach (DataSeries ds in Chart.InternalSeries)
            {
                if (ds.Faces != null)
                {
                    if (ds.Faces.LabelCanvas != null)
                        ds.Faces.LabelCanvas.Opacity = 0;
                }

                foreach (DataPoint dp in ds.DataPoints)
                {
                    if (dp.Faces != null)
                    {
                        if (dp.Faces.LabelCanvas != null)
                            dp.Faces.LabelCanvas.Opacity = 0;
                    }
                }
            }
        }

        /// <summary>
        /// Clear unwanted children from PlotAreaCanvas
        /// </summary>
        private void ClearPlotAreaChildren()
        {
            int noOfChildrenOfPlotarea = PlotAreaCanvas.Children.Count - 1;

            for (; noOfChildrenOfPlotarea > 0; noOfChildrenOfPlotarea--)
            {
                PlotAreaCanvas.Children.RemoveAt(noOfChildrenOfPlotarea);
            }
        }

        /// <summary>
        /// Draws the horizontal 3D Plank
        /// </summary>
        private void DrawHorizontalPlank(Double plankDepth, Double plankThickness, Double position)
        {
            Brush frontBrush, topBrush, rightBrush;

            List<Color> colors = new List<Color>();
            colors.Add(Color.FromArgb(125, 134, 134, 134)); // #FF868686
            colors.Add(Color.FromArgb(255, 210, 210, 210)); // #FFD2D2D2
            colors.Add(Color.FromArgb(255, 255, 255, 255)); // #FFFFFFFF
            colors.Add(Color.FromArgb(255, 223, 223, 223)); // #FFDFDFDF
            
            frontBrush = Graphics.CreateLinearGradientBrush(0, new Point(0.5,1), new Point(0.5,0), colors, new List<double>() { 0,1.844,1,0.442 }); 

            colors = new List<Color>();
            colors.Add(Color.FromArgb(255,232,232,232));  // #FFE8E8E8
            colors.Add(Color.FromArgb(255, 142,142,142)); // #FF8E8787


            rightBrush = Graphics.CreateLinearGradientBrush(0, new Point(0,0.5), new Point(1,0.5), colors, new List<double>() { 1, 0 }); 

            colors = new List<Color>();
            colors.Add(Color.FromArgb(255, 232, 232, 232));  // #FFE8E8E8
            colors.Add(Color.FromArgb(255, 142, 142, 142));  // #FF8E8787
            colors.Add(Color.FromArgb(255, 232, 227, 227));    // #FFE8E3E3

            topBrush = Graphics.CreateLinearGradientBrush(0, new Point(0.5, 1), new Point(0.5, 0), colors, new List<double>() { 0.357, 1, 0 }); 
            
            RectangularChartShapeParams columnParams = new RectangularChartShapeParams();
            columnParams.BackgroundBrush = new SolidColorBrush(Color.FromArgb((Byte)255, (Byte)246, (Byte)246, (Byte)246));

            columnParams.Lighting = false;
            //columnParams.Size = new Size(PlotAreaCanvas.ActualWidth - plankDepth, plankThickness);
            columnParams.Size = new Size(ScrollableLength - plankDepth, plankThickness);
            columnParams.Depth = plankDepth;
            columnParams.BorderThickness = 0.25;
            columnParams.BorderBrush = new SolidColorBrush(Colors.White); //new SolidColorBrush(Color.FromArgb((Byte)255, (Byte)246, (Byte)246, (Byte)246));
            Faces plankFaces = ColumnChart.Get3DColumn(columnParams, frontBrush, topBrush, new SolidColorBrush(Color.FromArgb((Byte)255, (Byte)130, (Byte)130, (Byte)130)));
            Panel plank = plankFaces.Visual;
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
        /// Draws the Horizontal 3D Plank
        /// </summary>
        private void DrawHorizontalPlank(Double width, Double plankDepth, Double plankThickness, Double plankOpacity)
        {
            if (Double.IsNaN(PlotAreaCanvas.ActualWidth) || PlotAreaCanvas.ActualWidth <= 0)
                return;

            RectangularChartShapeParams columnParams = new RectangularChartShapeParams();
            columnParams.BackgroundBrush = new SolidColorBrush(Color.FromArgb((Byte)255, (Byte)127, (Byte)127, (Byte)127));

            columnParams.Lighting = true;
            columnParams.Size = new Size(width, plankThickness);
            columnParams.Depth = plankDepth;

            Faces plankFaces = ColumnChart.Get3DColumn(columnParams);
            Panel plank = plankFaces.Visual;

            plank.SetValue(Canvas.LeftProperty, plankDepth + plankThickness);
            plank.SetValue(Canvas.TopProperty, PlotAreaCanvas.ActualHeight + plankThickness);
            plank.SetValue(Canvas.ZIndexProperty, -1);
            plank.SetValue(Canvas.OpacityProperty, plankOpacity);

            PlottingCanvas.Children.Add(plank);
        }

        /// <summary>
        /// Draws the Vertical 3D Plank
        /// </summary>
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

            rightBrush = Graphics.CreateLinearGradientBrush(0, new Point(0,0.5), new Point(1,0.5), colors, new List<double>() { 1,0 }); 

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
            Panel plank = plankFaces.Visual;

            plank.SetValue(Canvas.TopProperty, plankDepth);
            plank.SetValue(Canvas.ZIndexProperty, -1);

            PlottingCanvas.Children.Add(plank);
        }

        /// <summary>
        /// Draws the Vertical 3D Plank
        /// </summary>
        private void DrawVerticalPlank(Double height, Double plankDepth, Double plankThickness, Double plankOpacity)
        {
            RectangularChartShapeParams columnParams = new RectangularChartShapeParams();
            columnParams.BackgroundBrush = new SolidColorBrush(Color.FromArgb((Byte)255, (Byte)127, (Byte)127, (Byte)127));
            columnParams.Lighting = true;
            //columnParams.Size = new Size(plankThickness, PlotAreaCanvas.ActualHeight - plankDepth);
            columnParams.Size = new Size(plankThickness, height);
            columnParams.Depth = plankDepth;

            List<Color> colors = new List<Color>();
            colors.Add(Color.FromArgb(255, 232, 232, 232));
            colors.Add(Color.FromArgb(255, 142, 135, 135));

            Brush rightBrush = Graphics.CreateLinearGradientBrush(0, new Point(0, 0.5), new Point(1, 0.5), colors, new List<double>() { 0, 1 }); 

//          Brush rightBrush;
//          String xaml = String.Format(@"<LinearGradientBrush 
//
//xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"" 
//                EndPoint=""1,0.5"" StartPoint=""0,0.5"">
//                <GradientStop Color=""#FFE8E8E8"" Offset=""0""/>
//    			<GradientStop Color=""#FF8E8787"" Offset=""1""/>
//    		</LinearGradientBrush>");

//#if WPF
//          rightBrush = (Brush)XamlReader.Load(new XmlTextReader(new StringReader(xaml)));
//#else
//          rightBrush = System.Windows.Markup.XamlReader.Load(xaml) as Brush;
//#endif

            Faces plankFaces = ColumnChart.Get3DColumn(columnParams, null, null, rightBrush);
            Panel plank = plankFaces.Visual;

            plank.SetValue(Canvas.TopProperty, plankDepth);
            plank.SetValue(Canvas.ZIndexProperty, -1);
            plank.SetValue(Canvas.OpacityProperty, plankOpacity);

            PlottingCanvas.Children.Add(plank);
        }

        private void CreateTrendLineVisuals(Axis axis, List<TrendLine> trendLinesReferingToAAxes)
        {
            if (axis != null)
            {
                foreach (TrendLine trendLine in trendLinesReferingToAAxes)
                {
                    trendLine.ReferingAxis = axis;
                    trendLine.CreateVisualObject(ChartCanvas.Width, ChartCanvas.Height);
                    if (trendLine.Visual != null)
                    {
                        ChartCanvas.Children.Add(trendLine.Visual);
                    }
                }
            }
        }

        private void DrawTrendLines()
        {
            List<TrendLine> trendLinesReferingToPrimaryAxesX;
            List<TrendLine> trendLinesReferingToPrimaryAxisY;
            List<TrendLine> trendLinesReferingToSecondaryAxesX;
            List<TrendLine> trendLinesReferingToSecondaryAxisY;

            if (PlotDetails.ChartOrientation == ChartOrientationType.Vertical)
            {
                trendLinesReferingToPrimaryAxesX = (from trendline in Chart.TrendLines
                                                    where (trendline.Orientation == Orientation.Vertical) &&(trendline.AxisType == AxisTypes.Primary)
                                                    select trendline).ToList();
                trendLinesReferingToPrimaryAxisY = (from trendline in Chart.TrendLines
                                                    where (trendline.Orientation == Orientation.Horizontal) &&(trendline.AxisType == AxisTypes.Primary)
                                                    select trendline).ToList();
                trendLinesReferingToSecondaryAxesX = (from trendline in Chart.TrendLines
                                                      where (trendline.Orientation == Orientation.Vertical) &&(trendline.AxisType == AxisTypes.Secondary)
                                                      select trendline).ToList();
                trendLinesReferingToSecondaryAxisY = (from trendline in Chart.TrendLines
                                                      where (trendline.Orientation == Orientation.Horizontal) &&(trendline.AxisType == AxisTypes.Secondary)
                                                      select trendline).ToList();
            }
            else
            {
                trendLinesReferingToPrimaryAxesX = (from trendline in Chart.TrendLines
                                                    where (trendline.Orientation == Orientation.Horizontal) &&(trendline.AxisType == AxisTypes.Primary)
                                                    select trendline).ToList();
                trendLinesReferingToPrimaryAxisY = (from trendline in Chart.TrendLines
                                                    where (trendline.Orientation == Orientation.Vertical) &&(trendline.AxisType == AxisTypes.Primary)
                                                    select trendline).ToList();
                trendLinesReferingToSecondaryAxesX = (from trendline in Chart.TrendLines
                                                      where (trendline.Orientation == Orientation.Horizontal) &&(trendline.AxisType == AxisTypes.Secondary)
                                                      select trendline).ToList();
                trendLinesReferingToSecondaryAxisY = (from trendline in Chart.TrendLines
                                                      where (trendline.Orientation == Orientation.Vertical) &&(trendline.AxisType == AxisTypes.Secondary)
                                                      select trendline).ToList();
            }

            CreateTrendLineVisuals(AxisX, trendLinesReferingToPrimaryAxesX);

            CreateTrendLineVisuals(AxisY, trendLinesReferingToPrimaryAxisY);

            CreateTrendLineVisuals(AxisX2, trendLinesReferingToSecondaryAxesX);

            CreateTrendLineVisuals(AxisY2, trendLinesReferingToSecondaryAxisY);

            //if (AxisX != null)
            //{
            //    foreach (TrendLine trendLine in trendLinesReferingToPrimaryAxesX)
            //    {
            //        trendLine.ReferingAxis = AxisX;
            //        trendLine.CreateVisualObject(ChartCanvas.Width, ChartCanvas.Height);
            //        if (trendLine.Visual != null)
            //        {
            //            ChartCanvas.Children.Add(trendLine.Visual);
            //        }
            //    }
            //}
            //if (AxisY != null)
            //{
            //    foreach (TrendLine trendLine in trendLinesReferingToPrimaryAxisY)
            //    {
            //        trendLine.ReferingAxis = AxisY;
            //        trendLine.CreateVisualObject(ChartCanvas.Width, ChartCanvas.Height);
            //        if (trendLine.Visual != null)
            //        {
            //            ChartCanvas.Children.Add(trendLine.Visual);
            //        }
            //    }
            //}
            //if (AxisX2 != null)
            //{
            //    foreach (TrendLine trendLine in trendLinesReferingToSecondaryAxesX)
            //    {
            //        trendLine.ReferingAxis = AxisX2;
            //        trendLine.CreateVisualObject(ChartCanvas.Width, ChartCanvas.Height);
            //        if (trendLine.Visual != null)
            //        {
            //            ChartCanvas.Children.Add(trendLine.Visual);
            //        }
            //    }
            //}
            //if (AxisY2 != null)
            //{
            //    foreach (TrendLine trendLine in trendLinesReferingToSecondaryAxisY)
            //    {
            //        trendLine.ReferingAxis = AxisY2;
            //        trendLine.CreateVisualObject(ChartCanvas.Width, ChartCanvas.Height);
            //        if (trendLine.Visual != null)
            //        {
            //            ChartCanvas.Children.Add(trendLine.Visual);
            //        }
            //    }
            //}
        }

        private void DrawGrids()
        {
            if (AxisX != null)
            {
                foreach (ChartGrid grid in AxisX.Grids)
                {
                    grid.Chart = Chart;

                    grid.IsNotificationEnable = false;
                    grid.ApplyStyleFromTheme(Chart, "AxisXGrid");
                    grid.IsNotificationEnable = true;

                    grid.CreateVisualObject(ChartCanvas.Width, ChartCanvas.Height, Chart._internalAnimationEnabled && !_isAnimationFired, GRID_ANIMATION_DURATION);
                    if (grid.Visual != null)
                    {
                        //AxesX.MajorGridsElement.Visual.SetValue(Canvas.ZIndexProperty, -4);
                        ChartCanvas.Children.Add(grid.Visual);
                    }
                }
            }
            if (AxisX2 != null)
            {
                foreach (ChartGrid grid in AxisX2.Grids)
                {
                    grid.Chart = Chart;

                    grid.IsNotificationEnable = false;
                    grid.ApplyStyleFromTheme(Chart, "AxisXGrid");
                    grid.IsNotificationEnable = true;

                    grid.CreateVisualObject(ChartCanvas.Width, ChartCanvas.Height, Chart._internalAnimationEnabled && !_isAnimationFired, GRID_ANIMATION_DURATION);
                    if (grid.Visual != null)
                    {
                        //AxesX2.MajorGridsElement.Visual.SetValue(Canvas.ZIndexProperty, -5);
                        ChartCanvas.Children.Add(grid.Visual);
                    }
                }
            }
            if (AxisY != null)
            {
                foreach (ChartGrid grid in AxisY.Grids)
                {
                    grid.Chart = Chart;

                    grid.IsNotificationEnable = false;
                    grid.ApplyStyleFromTheme(Chart, "AxisYGrid");
                    grid.IsNotificationEnable = true;

                    grid.CreateVisualObject(ChartCanvas.Width, ChartCanvas.Height, Chart._internalAnimationEnabled && !_isAnimationFired, GRID_ANIMATION_DURATION);
                    if (grid.Visual != null)
                    {
                        //AxisY.MajorGridsElement.Visual.SetValue(Canvas.ZIndexProperty, -6);
                        ChartCanvas.Children.Add(grid.Visual);
                    }
                }
            }
            if (AxisY2 != null)
            {
                foreach (ChartGrid grid in AxisY2.Grids)
                {
                    grid.Chart = Chart;

                    grid.IsNotificationEnable = false;
                    grid.ApplyStyleFromTheme(Chart, "AxisYGrid");
                    grid.IsNotificationEnable = true;

                    grid.CreateVisualObject(ChartCanvas.Width, ChartCanvas.Height, Chart._internalAnimationEnabled && !_isAnimationFired, GRID_ANIMATION_DURATION);
                    if (grid.Visual != null)
                    {
                        //AxisY2.MajorGridsElement.Visual.SetValue(Canvas.ZIndexProperty, -7);
                        ChartCanvas.Children.Add(grid.Visual);
                    }
                }
            }
        }

        /// <summary>
        /// Renders charts based on the orientation type
        /// </summary>
        void Render(Size PreviousSize, Size NewSize)
        {
            System.Diagnostics.Debug.WriteLine("Count ==" + _redrawCount.ToString() + ", NewSize H=" + NewSize.Height.ToString() + ", W=" + NewSize.Width.ToString() + "|Previous| H=" + PreviousSize.Height.ToString() + ", W=" + PreviousSize.Width.ToString());

            ClearPlotAreaChildren();

            ChartScrollViewer = Chart._plotAreaScrollViewer;
            ChartScrollViewer.Background = new SolidColorBrush(Colors.Transparent);

            //----------
            //ChartScrollViewer.BorderThickness = new Thickness(1);
            //ChartScrollViewer.BorderBrush = new SolidColorBrush(Colors.Blue);
            //---------------

            PlotAreaCanvas.Width = NewSize.Width;
            PlotAreaCanvas.Height = NewSize.Height;

            PlottingCanvas = new Canvas();
            PlottingCanvas.Loaded += new RoutedEventHandler(PlottingCanvas_Loaded);
            PlottingCanvas.SetValue(Canvas.ZIndexProperty, 1);
            PlotAreaCanvas.Children.Add(PlottingCanvas);

            if (Double.IsNaN(NewSize.Height) || NewSize.Height <= 0 || Double.IsNaN(NewSize.Width) || NewSize.Width <= 0)
            {
                return;
            }
            else
            {
                if (PlotDetails.ChartOrientation == ChartOrientationType.Vertical)
                {
                    PlottingCanvas.Width = ScrollableLength + PlankDepth;
                    PlottingCanvas.Height = NewSize.Height;
                }
                if (PlotDetails.ChartOrientation == ChartOrientationType.Horizontal)
                {
                    PlottingCanvas.Width = NewSize.Width;
                    PlottingCanvas.Height = ScrollableLength + PlankDepth;
                }
            }

            // Create the chart canvas
            ChartCanvas = new Canvas();

            PlottingCanvas.Children.Add(ChartCanvas);

            // Default size of the chart canvas
            Size chartCanvasSize = new Size(0, 0);

            // Create the various region required for drawing charts
            switch (PlotDetails.ChartOrientation)
            {
                case ChartOrientationType.Vertical:
                    chartCanvasSize = CreateRegionsForVerticalCharts(ScrollableLength, NewSize);
                    // set chart Canvas position
                    ChartCanvas.SetValue(Canvas.LeftProperty, PlankDepth);
                    Chart.PlotArea.PlotAreaBorderElement.SetValue(Canvas.LeftProperty, PlankDepth);

                    break;

                case ChartOrientationType.Horizontal:
                    chartCanvasSize = CreateRegionsForHorizontalCharts(ScrollableLength, NewSize);
                    // set chart Canvas position
                    ChartCanvas.SetValue(Canvas.LeftProperty, PlankOffset);
                    Chart.PlotArea.PlotAreaBorderElement.SetValue(Canvas.LeftProperty, PlankOffset);
                    break;

                case ChartOrientationType.NoAxis:
                    chartCanvasSize = CreateRegionsForChartsWithoutAxis(NewSize);
                    break;

                default:
                    // No chart to render
                    break;
            }

            // Don't atempt to draw chart if the size is not fesiable
            if (chartCanvasSize.Width <= 0 || chartCanvasSize.Height <= 0)
                return;

            // set the ChartCanvas Size
            ChartCanvas.Width = chartCanvasSize.Width - ((PlotDetails.ChartOrientation == ChartOrientationType.Horizontal) ? CHART_SCROLLVIEWER_OFFSET:0);
            ChartCanvas.Height = chartCanvasSize.Height;
            Chart.PlotArea.PlotAreaBorderElement.Height = chartCanvasSize.Height;
            Chart.PlotArea.PlotAreaBorderElement.Width = chartCanvasSize.Width;
            
            //ChartCanvas.Background = new SolidColorBrush(Colors.Red);
            //PlottingCanvas.Background = new SolidColorBrush(Colors.Red);
            //Chart._drawingCanvas.Background = new SolidColorBrush(Colors.Red);
            //Chart._plotCanvas.Background = new SolidColorBrush(Colors.Red);

            // Draw the chart grids
            if (PlotDetails.ChartOrientation != ChartOrientationType.NoAxis)
            {
                DrawGrids();
                DrawTrendLines();
            }

            // Render each plot group in the plotdetails plotgroups list
            RenderSeries();

            Chart._plotCanvas.Width = PlottingCanvas.Width;
            Chart._plotCanvas.Height = PlottingCanvas.Height;

            _renderCount++;
        }

        /// <summary>
        /// Save Axis scrollbar Offset and reset scroll-viewer content margin
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="scrollBarOffset"></param>
        private void SaveAxisContentOffsetAndResetMargin(Axis axis, Double scrollBarOffset)
        {
            axis.CurrentScrollScrollBarOffset = scrollBarOffset;

            System.Diagnostics.Debug.WriteLine("Offset" + scrollBarOffset.ToString());

            if ((axis.ScrollViewerElement.Content as FrameworkElement) != null)
                (axis.ScrollViewerElement.Content as FrameworkElement).Margin = new Thickness(0);
        }
        
        internal void AxesXScrollBarElement_Scroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e)
        {
            if (PlotDetails.ChartOrientation == ChartOrientationType.Vertical)
            {
                Double offset = e.NewValue;

#if SL
                AxisX.ScrollBarElement.Maximum = ScrollableLength - ChartScrollViewer.ViewportWidth;
                AxisX.ScrollBarElement.ViewportSize = ChartScrollViewer.ViewportWidth;
#else
                AxisX.ScrollBarElement.Maximum = ScrollableLength - ChartScrollViewer.ActualWidth;
                AxisX.ScrollBarElement.ViewportSize = ChartScrollViewer.ActualWidth;

                if (e.NewValue <= 1)
                    offset = e.NewValue * AxisX.ScrollBarElement.Maximum;
#endif          
                ChartScrollViewer.ScrollToHorizontalOffset(offset);
                AxisX.ScrollViewerElement.ScrollToHorizontalOffset(offset);

                SaveAxisContentOffsetAndResetMargin(AxisX, offset);
            }

            if (PlotDetails.ChartOrientation == ChartOrientationType.Horizontal)
            {
                Double offset = e.NewValue;

#if SL
                AxisX.ScrollBarElement.Maximum = ScrollableLength - ChartScrollViewer.ViewportHeight;
                AxisX.ScrollBarElement.ViewportSize = ChartScrollViewer.ViewportHeight;
#else
                AxisX.ScrollBarElement.Maximum = ScrollableLength - ChartScrollViewer.ActualHeight;
                AxisX.ScrollBarElement.ViewportSize = ChartScrollViewer.ActualHeight;
                if (e.NewValue <= 1)
                    offset = e.NewValue * AxisX.ScrollBarElement.Maximum;
#endif
                ChartScrollViewer.ScrollToVerticalOffset(offset);
                AxisX.ScrollViewerElement.ScrollToVerticalOffset(offset);

                SaveAxisContentOffsetAndResetMargin(AxisX, offset);
            }
            if (AxisX2 != null)
                AxisX2.ScrollBarElement.Value = e.NewValue;
        }

        internal void AxesX2ScrollBarElement_Scroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e)
        {
            if (PlotDetails.ChartOrientation == ChartOrientationType.Vertical)
            {
                Double offset = e.NewValue;

#if SL
                AxisX2.ScrollBarElement.Maximum = ScrollableLength - ChartScrollViewer.ViewportWidth;
                AxisX2.ScrollBarElement.ViewportSize = ChartScrollViewer.ViewportWidth;
#else

                AxisX2.ScrollBarElement.Maximum = ScrollableLength - ChartScrollViewer.ActualWidth;
                AxisX2.ScrollBarElement.ViewportSize = ChartScrollViewer.ActualWidth;
                if (e.NewValue <= 1)
                    offset = e.NewValue * AxisX2.ScrollBarElement.Maximum;
#endif
                ChartScrollViewer.ScrollToHorizontalOffset(offset);
                AxisX2.ScrollViewerElement.ScrollToHorizontalOffset(offset);

                SaveAxisContentOffsetAndResetMargin(AxisX2, offset);

            }
            if (PlotDetails.ChartOrientation == ChartOrientationType.Horizontal)
            {
                Double offset = e.NewValue;
#if SL
                AxisX2.ScrollBarElement.Maximum = ScrollableLength - ChartScrollViewer.ViewportHeight;
                AxisX2.ScrollBarElement.ViewportSize = ChartScrollViewer.ViewportHeight;
#else
                AxisX2.ScrollBarElement.Maximum = ScrollableLength - ChartScrollViewer.ActualHeight;
                AxisX2.ScrollBarElement.ViewportSize = ChartScrollViewer.ActualHeight;
                if (e.NewValue <= 1)
                    offset = e.NewValue * AxisX2.ScrollBarElement.Maximum;
#endif
                ChartScrollViewer.ScrollToVerticalOffset(offset);
                AxisX2.ScrollViewerElement.ScrollToVerticalOffset(offset);

                SaveAxisContentOffsetAndResetMargin(AxisX2, offset);
            }

            if (AxisX != null)
                AxisX.ScrollBarElement.Value = e.NewValue;
        }

        private void RenderSeries()
        {
            Int32 renderedSeriesCount = 0;      // Contain count of series that have been already rendered

            // Contains a list of serties as per the drawing order generated in the plotdetails
            List<DataSeries> dataSeriesListInDrawingOrder = PlotDetails.SeriesDrawingIndex.Keys.ToList();

            List<DataSeries> selectedDataSeries4Rendering;          // Contains a list of serries to be rendered in a rendering cycle
            Int32 currentDrawingIndex;                              // Drawing index of the selected series 
            RenderAs currentRenderAs;                               // Rendereas type of the selected series
            Canvas renderedChart;                                   // A canvas that contains the chart rendered using the selected series

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
                    ChartCanvas.Children.Add(renderedChart);

                renderedSeriesCount += selectedDataSeries4Rendering.Count;
            }
        }

        /// <summary>
        /// calls the appropriate chart rendering function to render the series available in the series list.
        /// Creates a layer as per the drawing index
        /// </summary>
        /// <param name="seriesListForRendering">List of selected dataseries</param>
        /// <returns></returns>
        private Canvas RenderSeriesFromList(List<DataSeries> dataSeriesList4Rendering)
        {
            Canvas renderedCanvas = null;

            switch (dataSeriesList4Rendering[0].RenderAs)
            {
                case RenderAs.Column:
                    renderedCanvas = ColumnChart.GetVisualObjectForColumnChart(ChartCanvas.Width, ChartCanvas.Height, PlotDetails, dataSeriesList4Rendering, Chart, PlankDepth, (Chart._internalAnimationEnabled && !_isAnimationFired));
                    break;

                case RenderAs.Bar:
                    renderedCanvas = BarChart.GetVisualObjectForBarChart(ChartCanvas.Width, ChartCanvas.Height, PlotDetails, dataSeriesList4Rendering, Chart, PlankDepth, (Chart._internalAnimationEnabled && !_isAnimationFired));
                    break;

                case RenderAs.Line:
                    renderedCanvas = LineChart.GetVisualObjectForLineChart(ChartCanvas.Width, ChartCanvas.Height, PlotDetails, dataSeriesList4Rendering, Chart, PlankDepth, (Chart._internalAnimationEnabled && !_isAnimationFired));
                    break;

                case RenderAs.Point:
                    renderedCanvas = PointChart.GetVisualObjectForPointChart(ChartCanvas.Width, ChartCanvas.Height, PlotDetails, dataSeriesList4Rendering, Chart, PlankDepth, (Chart._internalAnimationEnabled && !_isAnimationFired));
                    break;

                case RenderAs.Bubble:
                    renderedCanvas = BubbleChart.GetVisualObjectForBubbleChart(ChartCanvas.Width, ChartCanvas.Height, PlotDetails, dataSeriesList4Rendering, Chart, PlankDepth, (Chart._internalAnimationEnabled && !_isAnimationFired));
                    break;

                case RenderAs.Area:
                    renderedCanvas = AreaChart.GetVisualObjectForAreaChart(ChartCanvas.Width, ChartCanvas.Height, PlotDetails, dataSeriesList4Rendering, Chart, PlankDepth, (Chart._internalAnimationEnabled && !_isAnimationFired));
                    break;

                case RenderAs.StackedColumn:
                    renderedCanvas = ColumnChart.GetVisualObjectForStackedColumnChart(ChartCanvas.Width, ChartCanvas.Height, PlotDetails, Chart, PlankDepth, (Chart._internalAnimationEnabled && !_isAnimationFired));
                    break;

                case RenderAs.StackedColumn100:
                    renderedCanvas = ColumnChart.GetVisualObjectForStackedColumn100Chart(ChartCanvas.Width, ChartCanvas.Height, PlotDetails, Chart, PlankDepth, (Chart._internalAnimationEnabled && !_isAnimationFired));
                    break;

                case RenderAs.StackedBar:
                    renderedCanvas = BarChart.GetVisualObjectForStackedBarChart(ChartCanvas.Width, ChartCanvas.Height, PlotDetails, Chart, PlankDepth, (Chart._internalAnimationEnabled && !_isAnimationFired));
                    break;

                case RenderAs.StackedBar100:
                    renderedCanvas = BarChart.GetVisualObjectForStackedBar100Chart(ChartCanvas.Width, ChartCanvas.Height, PlotDetails, Chart, PlankDepth, (Chart._internalAnimationEnabled && !_isAnimationFired));
                    break;

                case RenderAs.Pie:
                    renderedCanvas = PieChart.GetVisualObjectForPieChart(ChartCanvas.Width, ChartCanvas.Height, PlotDetails, dataSeriesList4Rendering, Chart, (Chart._internalAnimationEnabled && !_isAnimationFired));
                    break;

                case RenderAs.Doughnut:
                    renderedCanvas = PieChart.GetVisualObjectForDoughnutChart(ChartCanvas.Width, ChartCanvas.Height, PlotDetails, dataSeriesList4Rendering, Chart, (Chart._internalAnimationEnabled && !_isAnimationFired));
                    break;

                case RenderAs.StackedArea:
                    renderedCanvas = AreaChart.GetVisualObjectForStackedAreaChart(ChartCanvas.Width, ChartCanvas.Height, PlotDetails, dataSeriesList4Rendering, Chart, PlankDepth, (Chart._internalAnimationEnabled && !_isAnimationFired));
                    break;

                case RenderAs.StackedArea100:
                    renderedCanvas = AreaChart.GetVisualObjectForStackedArea100Chart(ChartCanvas.Width, ChartCanvas.Height, PlotDetails, dataSeriesList4Rendering, Chart, PlankDepth, (Chart._internalAnimationEnabled && !_isAnimationFired));
                    break;
            }

            ApplyOpacity();

            AttachEventsToolTipHref2DataSeries(dataSeriesList4Rendering);

            return renderedCanvas;
        }

        internal void Animate()
        {
            if (Chart._internalAnimationEnabled && !Chart.IsInDesignMode)
            {
                try
                {
                    if (PlotDetails.ChartOrientation != ChartOrientationType.NoAxis)
                    {
                        foreach (ChartGrid chartGrid in AxisX.Grids)
                        {
                            if (chartGrid.Storyboard != null)
                            {
#if WPF
                                chartGrid.Storyboard.Begin(chartGrid as FrameworkElement, true);
#else
                                chartGrid.Storyboard.Begin();
#endif
                                chartGrid.Storyboard.Completed += delegate
                                {
                                    _isAnimationFired = true;
                                };
                            }
                        }

                        if (AxisY != null)
                            foreach (ChartGrid chartGrid in AxisY.Grids)
                            {
                                if (chartGrid.Storyboard != null)
                                {
#if WPF
                                    chartGrid.Storyboard.Begin(chartGrid as FrameworkElement, true);
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

                    foreach (DataSeries series in Chart.InternalSeries)
                    {
                        if (series.Storyboard != null)
                        {
                            series.Storyboard.Completed += delegate
                            {
                                _isAnimationFired = true;
                                Chart._rootElement.IsHitTestVisible = true;
                            };
#if WPF
                        if (PlotDetails.ChartOrientation == ChartOrientationType.NoAxis)
                        {
                            series.Storyboard.Completed += delegate(object sender, EventArgs e)
                            {
                                
                                foreach (DataPoint dataPoint in series.DataPoints)
                                {
                                    if (dataPoint.Exploded)
                                        dataPoint.InteractiveAnimation();
                                }
                                
                            };
                        }
                        
                        series.Storyboard.Begin(series as FrameworkElement, true);
#else
                            if (PlotDetails.ChartOrientation == ChartOrientationType.NoAxis)
                            {
                                series.Storyboard.Completed += delegate(object sender, EventArgs e)
                                {
                                    _isAnimationFired = true;

                                    foreach (DataPoint dataPoint in series.DataPoints)
                                    {
                                        if (dataPoint.Exploded)
                                            dataPoint.InteractiveAnimation();
                                    }
                                };
                            }

                            series.Storyboard.Stop();
                            series.Storyboard.Begin();
#endif
                        }
                    }


                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine("Animation Error. " + e.Message);
                }

            }

        }
        
        private void SetSeriesStyleFromTheme(Chart chart)
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

        private void SetTitleStyleFromTheme(Chart chart)
        {
            int titleIndex = 0;

            if (!String.IsNullOrEmpty(chart.Theme))
            {
                foreach (Title title in chart.Titles)
                {
                    if (titleIndex == 0)
                        title.ApplyStyleFromTheme(chart, "MainTitle");
                    else
                        title.ApplyStyleFromTheme(chart, "SubTitle");

                    titleIndex++;
                }
            }
        }

        private void SetLegendStyleFromTheme(Chart chart)
        {
            foreach (Legend legend in chart.Legends)
            {
                if (!String.IsNullOrEmpty(chart.Theme))
                {
                    legend.ApplyStyleFromTheme(chart, "Legend");
                }
            }
        }

        internal void SetDataPointColorFromColorSet(Chart chart, System.Collections.Generic.IList<DataSeries> Series)
        {
            ColorSet colorSet = null;

            // Load chart colorSet
            if (!String.IsNullOrEmpty(chart.ColorSet))
            {
                colorSet = chart.InternalColorSets.GetColorSetByName(chart.ColorSet);
            }

            if (Series.Count == 1)
            {
                if (!String.IsNullOrEmpty(Series[0].ColorSet))
                {
                    colorSet = chart.InternalColorSets.GetColorSetByName(Series[0].ColorSet);

                    if (colorSet == null)
                        throw new Exception("ColorSet named " + Series[0].ColorSet + " is not found.");
                }
                else if (colorSet == null)
                {
                    throw new Exception("ColorSet named " + chart.ColorSet + " is not found.");
                }

                if (Series[0].RenderAs == RenderAs.Area || Series[0].RenderAs == RenderAs.Line || Series[0].RenderAs == RenderAs.StackedArea || Series[0].RenderAs == RenderAs.StackedArea100)
                {
                    Brush seriesColor = Series[0].GetValue(DataSeries.ColorProperty) as Brush;

                    if (seriesColor == null)
                        Series[0].InternalColor = colorSet.GetNewColorFromColorSet();

                    colorSet.ReSet();

                    foreach (DataPoint dp in Series[0].DataPoints)
                    {
                        dp.IsNotificationEnable = false;

                        dp.InternalColor = colorSet.GetNewColorFromColorSet();

                        dp.IsNotificationEnable = true;
                    }
                }
                else
                {
                    foreach (DataPoint dp in Series[0].DataPoints)
                    {
                        dp.IsNotificationEnable = false;

                        Brush dPColor = dp.GetValue(DataPoint.ColorProperty) as Brush;

                        if (dPColor == null)
                        {
                            dp.InternalColor = colorSet.GetNewColorFromColorSet();
                        }

                        dp.IsNotificationEnable = true;
                    }
                }
            }
            else if (Series.Count > 1)
            {
                ColorSet colorSet4MultiSeries = null;
                Boolean FLAG_UNIQUE_COLOR_4_EACH_DP = false;   // Unique color for each DataPoint
                Brush seriesColor = null;

                if (colorSet != null)
                    colorSet.ReSet();

                foreach (DataSeries ds in Series)
                {
                    colorSet4MultiSeries = colorSet;
                    FLAG_UNIQUE_COLOR_4_EACH_DP = false;

                    if (!String.IsNullOrEmpty(ds.ColorSet))
                    {
                        colorSet4MultiSeries = chart.InternalColorSets.GetColorSetByName(ds.ColorSet);

                        if (colorSet4MultiSeries != null)
                            throw new Exception("ColorSet named " + ds.ColorSet + " is not found.");

                        FLAG_UNIQUE_COLOR_4_EACH_DP = true;
                    }
                    else if (colorSet4MultiSeries == null)
                    {
                        throw new Exception("ColorSet named " + chart.ColorSet + " is not found.");
                    }

                    if (!FLAG_UNIQUE_COLOR_4_EACH_DP)
                        seriesColor = colorSet4MultiSeries.GetNewColorFromColorSet();

                    if (ds.RenderAs == RenderAs.Area || ds.RenderAs == RenderAs.Line || ds.RenderAs == RenderAs.StackedArea || ds.RenderAs == RenderAs.StackedArea100)
                    {
                        Brush DataSeriesColor = ds.GetValue(DataSeries.ColorProperty) as Brush;
                        if (DataSeriesColor == null)
                        {
                            ds.InternalColor = seriesColor;
                        }
                        else
                            ds.InternalColor = DataSeriesColor;
                    }
                    else
                    {
                        foreach (DataPoint dp in ds.DataPoints)
                        {
                            dp.IsNotificationEnable = false;
                            Brush dPColor = dp.GetValue(DataPoint.ColorProperty) as Brush;
                            if (dPColor == null)
                            {
                                // If unique color for each DataPoint
                                if (FLAG_UNIQUE_COLOR_4_EACH_DP)
                                {
                                    dp.InternalColor = colorSet4MultiSeries.GetNewColorFromColorSet();
                                }
                                else
                                {
                                    ds.InternalColor = seriesColor;
                                    dp.IsNotificationEnable = true;
                                    break;
                                }
                            }

                            dp.IsNotificationEnable = true;
                        }
                    }

                    ds.IsNotificationEnable = true;
                }
            }

            if (colorSet != null)
                colorSet.ReSet();
        }

        void AddEntriesToLegend(Legend legend, List<DataPoint> dataPoints)
        {
            foreach (DataPoint dataPoint in dataPoints)
            {
                if (!(Boolean)dataPoint.Enabled)
                    continue;

                String legendText = (String.IsNullOrEmpty(dataPoint.LegendText) ? dataPoint.Name : dataPoint.LegendText);

                Brush markerColor = dataPoint.MarkerBorderColor;

                Boolean markerBevel;
                if ((dataPoint.Parent as DataSeries).RenderAs == RenderAs.Point 
                    || (dataPoint.Parent as DataSeries).RenderAs == RenderAs.Bubble
                    || (dataPoint.Parent as DataSeries).RenderAs == RenderAs.Pie 
                    || (dataPoint.Parent as DataSeries).RenderAs == RenderAs.Doughnut
                    || (dataPoint.Parent as DataSeries).RenderAs == RenderAs.Line)
                    markerBevel = false;
                else
                    markerBevel = Chart.View3D ? false : dataPoint.Parent.Bevel ? dataPoint.Parent.Bevel : false;

                Size markerSize;
                if (dataPoint.Parent.RenderAs == RenderAs.Line)
                    markerSize = new Size(18, 8);
                else
                    markerSize = new Size(8, 8);

                dataPoint.LegendMarker = new Marker(
                        RenderAsToMarkerType(dataPoint.Parent.RenderAs, dataPoint.Parent),
                        1,
                        markerSize,
                        markerBevel,
                        markerColor,
                        ""
                        );

                legend.Entries.Add(new KeyValuePair<String, Marker>(legendText, dataPoint.LegendMarker));
            }
        }

        /// <summary>
        /// Add Legends to ChartArea
        /// </summary>
        /// <param name="chart"></param>
        private void AddLegends(Chart chart, Boolean DockInsidePlotArea, Double Height, Double Width)
        {
            List<Legend> dockTest = (from legend in chart.Legends
                                     where legend.DockInsidePlotArea == DockInsidePlotArea select legend).ToList();

            if (dockTest.Count <= 0)
                return;

            if (chart.InternalSeries.Count == 1 || (chart.InternalSeries[0].RenderAs == RenderAs.Pie || chart.InternalSeries[0].RenderAs == RenderAs.Doughnut))
            {
                if ((Boolean)chart.InternalSeries[0].ShowInLegend)
                {
                    Legend legend = null;
                    foreach (Legend entry in chart.Legends)
                        entry.Entries.Clear();

                    if (chart.Legends.Count > 0 && (!String.IsNullOrEmpty(chart.InternalSeries[0].Legend) || !String.IsNullOrEmpty(chart.InternalSeries[0].InternalLegendName)))
                    {
                        var legends = from entry in chart.Legends
                                      where (entry.Name == chart.InternalSeries[0].Legend || entry.Name == chart.InternalSeries[0].InternalLegendName) && entry.DockInsidePlotArea == DockInsidePlotArea
                                      select entry;

                        if (legends.Count() > 0)
                            legend = (legends).First();
                    }

                    //if (legend == null)
                    //{
                    //    legend = new Legend();
                    //    legend.Chart = Chart;
                    //    chart.Legends.Clear();
                    //    chart.Legends.Add(legend);
                    //}

                    AddEntriesToLegend(legend, chart.InternalSeries[0].DataPoints.ToList());
                }
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
                        if (chart.Legends.Count > 0 && (!String.IsNullOrEmpty(dataSeries.Legend) 
                            || !String.IsNullOrEmpty(dataSeries.InternalLegendName)))
                        {
                            legend = null;
                            var legends = from entry in chart.Legends
                                          where (entry.Name == dataSeries.Legend 
                                          || entry.Name == dataSeries.InternalLegendName) 
                                          && entry.DockInsidePlotArea == DockInsidePlotArea
                                          select entry;

                            if (legends.Count() > 0)
                                legend = (legends).First();
                        }

                        if (legend == null)
                        {
                            throw new Exception("Legend name is not specified in DataSeries..");
                        }

                        //{
                        //    legend = new Legend();
                        //    legend.Chart = Chart;
                        //    chart.Legends.Add(legend);
                        //}

                        String legendText = (String.IsNullOrEmpty(dataSeries.LegendText) ? dataSeries.Name : dataSeries.LegendText);

                        Brush markerColor = dataSeries.Color;
                        if (dataSeries.DataPoints.Count > 0)
                        {
                            DataPoint dataPoint = dataSeries.DataPoints[0];
                            markerColor = markerColor ?? dataPoint.Color;
                        }

                        Boolean markerBevel;
                        if (dataSeries.RenderAs == RenderAs.Point || dataSeries.RenderAs == RenderAs.Bubble
                            || dataSeries.RenderAs == RenderAs.Line)
                            markerBevel = false;
                        else
                            markerBevel = Chart.View3D ? false : dataSeries.Bevel ? dataSeries.Bevel : false;

                        Size markerSize;
                        if (dataSeries.RenderAs == RenderAs.Line)
                            markerSize = new Size(18, 8);
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

                        legend.Entries.Add(new KeyValuePair<String, Marker>(legendText, dataSeries.LegendMarker));
                    }
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
                centerPanel = chart._centerDockInsidePlotAreaPanel;
            }

            chart._topInnerLegendPanel.Children.Clear();
            chart._bottomInnerLegendPanel.Children.Clear();
            chart._leftInnerLegendPanel.Children.Clear();
            chart._rightInnerLegendPanel.Children.Clear();
            chart._centerDockInsidePlotAreaPanel.Children.Clear();
            chart._topOuterLegendPanel.Children.Clear();
            chart._bottomOuterLegendPanel.Children.Clear();
            chart._leftOuterLegendPanel.Children.Clear();
            chart._rightOuterLegendPanel.Children.Clear();
            chart._centerDockInsidePlotAreaPanel.Children.Clear();

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
                                          && entry.VerticalAlignment == VerticalAlignment.Center 
                                          && entry.HorizontalAlignment == HorizontalAlignment.Left 
                                          && entry.DockInsidePlotArea == DockInsidePlotArea && (Boolean)entry.Enabled
                                          select entry).ToList();

            List<Legend> legendsOnRight = (from entry in chart.Legends
                                           where entry.Entries.Count > 0 
                                           && entry.VerticalAlignment == VerticalAlignment.Center 
                                           && entry.HorizontalAlignment == HorizontalAlignment.Right 
                                           && entry.DockInsidePlotArea == DockInsidePlotArea 
                                           && (Boolean)entry.Enabled
                                           select entry).ToList();

            List<Legend> legendsAtCenter = (from entry in chart.Legends
                                            where entry.Entries.Count > 0 
                                            && entry.VerticalAlignment == VerticalAlignment.Center 
                                            && entry.HorizontalAlignment == HorizontalAlignment.Center 
                                            && entry.DockInsidePlotArea == DockInsidePlotArea 
                                            && (Boolean)entry.Enabled
                                            select entry).ToList();

            if (legendsOnTop.Count > 0)
            {
                foreach (Legend legend in legendsOnTop)
                {
                    legend.Orientation = Orientation.Horizontal;
                    legend.LegendLayout = LegendLayouts.FlowLayout;
                    if (!Double.IsNaN(Width) && Width > 0)
                        legend.MaximumWidth = Width;

                    legend.CreateVisual();
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
                    legend.LegendLayout = LegendLayouts.FlowLayout;
                    if (!Double.IsNaN(Width) && Width > 0)
                        legend.MaximumWidth = Width;

                    legend.CreateVisual();
                    if (legend.Visual != null)
                        bottomLegendPanel.Children.Add(legend.Visual);
                }
            }

            if (legendsOnLeft.Count > 0)
            {
                foreach (Legend legend in legendsOnLeft)
                {
                    legend.Orientation = Orientation.Vertical;
                    legend.LegendLayout = LegendLayouts.FlowLayout;
                    if (!Double.IsNaN(Height) && Height > 0)
                        legend.MaximumHeight = Height;

                    legend.CreateVisual();
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
                    legend.LegendLayout = LegendLayouts.FlowLayout;
                    if (!Double.IsNaN(Height) && Height > 0)
                        legend.MaximumHeight = Height;

                    legend.CreateVisual();
                    if (legend.Visual != null)
                        rightLegendPanel.Children.Add(legend.Visual);
                }
            }

            if (legendsAtCenter.Count > 0)
            {
                foreach (Legend legend in legendsAtCenter)
                {
                    legend.Orientation = Orientation.Horizontal;
                    legend.LegendLayout = LegendLayouts.FlowLayout;
                    if (legend.MaximumWidth == 0)
                        legend.MaximumWidth = Width * 60 / 100;

                    legend.CreateVisual();

                    if (legend.Visual != null)
                        Chart._centerDockInsidePlotAreaPanel.Children.Add(legend.Visual);
                }
            }

            if (chart.InternalSeries.Count == 1 && chart.InternalSeries[0].RenderAs != RenderAs.Pie && chart.InternalSeries[0].RenderAs != RenderAs.Doughnut)
                if (!(Boolean)chart.InternalSeries[0].ShowInLegend)
                {
                    topLegendPanel.Children.Clear();
                    bottomLegendPanel.Children.Clear();
                    leftLegendPanel.Children.Clear();
                    rightLegendPanel.Children.Clear();
                    centerPanel.Children.Clear();
                }
        }

        /// <summary>
        /// Add titles to ChartArea
        /// </summary>
        /// <param name="chart"></param>
        private void AddTitles(Chart chart, Boolean DockInsidePlotArea, Double height, Double width)
        {
            IList<Title> titles;
            StackPanel topTitlePanel;
            StackPanel bottomTitlePanel;
            StackPanel leftTitlePanel;
            StackPanel rightTitlePanel;
            StackPanel centerPanel;

            if (DockInsidePlotArea)
            {
                // Get the Titles docked outside PlotArea 
                titles = chart.GetTitlesDockedInsidePlotArea();
                topTitlePanel = chart._topInnerTitlePanel;
                bottomTitlePanel = chart._bottomInnerTitlePanel;
                leftTitlePanel = chart._leftInnerTitlePanel;
                rightTitlePanel = chart._rightInnerTitlePanel;
                centerPanel = chart._centerDockInsidePlotAreaPanel;

                chart._topInnerTitlePanel.Children.Clear();
                chart._rightInnerTitlePanel.Children.Clear();
                chart._leftInnerTitlePanel.Children.Clear();
                chart._bottomInnerTitlePanel.Children.Clear();
            }
            else
            {
                // Get the Titles docked outside PlotArea 
                titles = chart.GetTitlesDockedOutSidePlotArea();
                topTitlePanel = chart._topOuterTitlePanel;
                bottomTitlePanel = chart._bottomOuterTitlePanel;
                leftTitlePanel = chart._leftOuterTitlePanel;
                rightTitlePanel = chart._rightOuterTitlePanel;
                centerPanel = chart._centerDockInsidePlotAreaPanel;

                chart._topOuterTitlePanel.Children.Clear();
                chart._rightOuterTitlePanel.Children.Clear();
                chart._leftOuterTitlePanel.Children.Clear();
                chart._bottomOuterTitlePanel.Children.Clear();
            }

            if (titles.Count == 0)
                return;

            // Get Titles on the top of the ChartArea using LINQ
            var titlesOnTop = from title in titles
                              where (title.VerticalAlignment == VerticalAlignment.Top && title.Enabled

                                  == true)
                              select title;

            // Add Title on the top of the ChartArea
            foreach (Title title in titlesOnTop)
                this.AddTitle(chart, title, topTitlePanel, width, height);

            // Get Titles on the bottom of the ChartArea using LINQ
            var titlesOnBottom = from title in titles
                                 where (title.VerticalAlignment == VerticalAlignment.Bottom &&

                                     title.Enabled == true)
                                 select title;

            titlesOnBottom.Reverse();

            // Add Title on the bottom of the ChartArea
            foreach (Title title in titlesOnBottom)
                this.AddTitle(chart, title, bottomTitlePanel, width, height);

            // Get Titles on the left of the ChartArea using LINQ
            var titlesOnLeft = from title in titles
                               where ((title.VerticalAlignment == VerticalAlignment.Center ||

                                   title.VerticalAlignment == VerticalAlignment.Stretch) && title.HorizontalAlignment == HorizontalAlignment.Left &&

                                   title.Enabled == true)
                               select title;

            // Add Title on left of the ChartArea
            foreach (Title title in titlesOnLeft)
                this.AddTitle(chart, title, leftTitlePanel, width, height);


            // Get Titles on the right of the ChartArea using LINQ
            var titlesOnRight = from title in titles
                                where ((title.VerticalAlignment == VerticalAlignment.Center ||

                                    title.VerticalAlignment == VerticalAlignment.Stretch) && title.HorizontalAlignment == HorizontalAlignment.Right &&

                                    title.Enabled == true)
                                select title;

            titlesOnRight.Reverse();

            // Add Title on the right of the ChartArea
            foreach (Title title in titlesOnRight)
                this.AddTitle(chart, title, rightTitlePanel, width, height);

            // Get Titles on the right of the ChartArea using LINQ
            var titlesOnCenter = from title in titles
                                 where ((title.HorizontalAlignment == HorizontalAlignment.Center ||

                                     title.HorizontalAlignment == HorizontalAlignment.Stretch) && (title.VerticalAlignment == VerticalAlignment.Center ||

                                     title.VerticalAlignment == VerticalAlignment.Stretch) && title.Enabled == true)
                                 select title;

            // Add Title on the right of the ChartArea
            centerPanel.Children.Clear();
            foreach (Title title in titlesOnCenter)
            {
                this.AddTitle(chart, title, centerPanel, width, height);
            }
        }

        /// <summary>
        /// Event handler for the title property changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void title_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(sender, null);
        }

        #endregion

        #region Public Properties

        public Canvas PlotAreaCanvas { get; set; }

        public Canvas PlottingCanvas { get; set; }

        public Canvas ChartCanvas { get; set; }

        public ScrollViewer ChartScrollViewer { get; set; }

        public Chart Chart
        {
            get;
            set;
        }

        #endregion

        #region Public Events

        #endregion

        #region Protected Methods

        #endregion

        #region Internal Properties

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

        internal Double PlankOffset
        {
            get;
            private set;
        }
        internal Double PlankDepth
        {
            get;
            private set;
        }
        internal Double PlankThickness
        {
            get;
            private set;
        }

        

        internal Axis AxisX
        {
            get;
            set;
        }

        internal Axis AxisX2
        {
            get;
            set;
        }

        internal Axis AxisY
        {
            get;
            set;
        }

        internal Axis AxisY2
        {
            get;
            set;
        }

        internal Double ScrollableLength
        {
            get;
            set;
        }
        #endregion

        #region Private Delegates

        #endregion

        #region Private Methods

        Double GetChartAreaCenterGridTopMargin()
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

        Double GetChartAreaCenterGridBottomMargin()
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

        Double GetChartAreaCenterGridRightMargin()
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

        Double GetChartAreaCenterGridLeftMargin()
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

        Size SetChartAreaCenterGridMargin(Size NewSize)
        {
            Double left = GetChartAreaCenterGridLeftMargin();
            Double right = GetChartAreaCenterGridRightMargin() + ((Chart.PlotArea.ShadowEnabled) ? Chart.SHADOW_DEPTH : 0);
            Double top = GetChartAreaCenterGridTopMargin();
            Double bottom = GetChartAreaCenterGridBottomMargin();

            Chart._topOffsetGrid.Height = top;
            Chart._bottomOffsetGrid.Height = bottom;
            Chart._rightOffsetGrid.Width = right;
            Chart._leftOffsetGrid.Width = left;

            NewSize.Height -= top;
            NewSize.Height -= bottom;

            NewSize.Width -= left;
            NewSize.Width -= right;

            return NewSize;
        }

        private void AddAxis(Chart chart)
        {
            AxisX = PlotDetails.GetAxisXFromChart(chart, AxisTypes.Primary);
            AxisX2 = PlotDetails.GetAxisXFromChart(chart, AxisTypes.Secondary);
            AxisY = PlotDetails.GetAxisYFromChart(chart, AxisTypes.Primary);
            AxisY2 = PlotDetails.GetAxisYFromChart(chart, AxisTypes.Secondary);

            //if (AxisX != null) AxisX.PlotDetails = PlotDetails;
            //if (AxisX2 != null) AxisX2.PlotDetails = PlotDetails;
            //if (AxisY != null) AxisY.PlotDetails = PlotDetails;
            //if (AxisY2 != null) AxisY2.PlotDetails = PlotDetails;

            

            if (AxisX != null)
            {

                AxisX.ApplyStyleFromTheme(Chart, "AxisX");

                if (PlotDetails.ChartOrientation == ChartOrientationType.Vertical)
                {
                    AxisX.StartOffset = 0;
                    AxisX.EndOffset = PlankDepth;
                    //AxisX.Width = _bundingRec.Width;
                    AxisX.SetScrollBar("AxisX");
                    //AxisX.CreateVisualObject(Chart, "AxisX");
                    //Chart._bottomAxisPanel.Children.Add(AxisX.Visual);

                }
                else
                {
                    AxisX.StartOffset = 0;
                    AxisX.EndOffset = 0;
                    //AxisX.Width = _bundingRec.Width;
                    AxisX.SetScrollBar("AxisX");
                    //AxisX.CreateVisualObject(Chart, "AxisX");
                    //Chart._leftAxisPanel.Children.Add(AxisX.Visual);
                }
            }

            if (AxisY != null)
            {
                AxisY.ApplyStyleFromTheme(Chart, "AxisY");

                if (PlotDetails.ChartOrientation == ChartOrientationType.Vertical)
                {
                    AxisY.StartOffset = PlankDepth;
                    AxisY.EndOffset = PlankThickness;
                    //AxisY.Height = _bundingRec.Height;
                    AxisY.SetScrollBar("AxisY");
                    //AxisY.CreateVisualObject(Chart, "AxisY");
                    //Chart._leftAxisPanel.Children.Add(AxisY.Visual);
                }
                else
                {
                    AxisY.StartOffset = PlankOffset;
                    AxisY.EndOffset = 0;
                    //AxisY.Height = _bundingRec.Height;
                    AxisY.SetScrollBar("AxisY");
                    //AxisY.CreateVisualObject(Chart, "AxisY");
                    //Chart._bottomAxisPanel.Children.Add(AxisY.Visual);
                }
            }

            if (AxisX2 != null)
            {
                AxisX2.ApplyStyleFromTheme(Chart, "AxisX");

                if (PlotDetails.ChartOrientation == ChartOrientationType.Vertical)
                {
                    AxisX2.StartOffset = PlankDepth;
                    AxisX2.EndOffset = 0;
                    //AxisX2.Width = _bundingRec.Width;
                    AxisX2.SetScrollBar("AxisX");
                    //AxisX2.CreateVisualObject(Chart, "AxisX");
                    //Chart._topAxisPanel.Children.Add(AxisX2.Visual);
                }
                else
                {
                    AxisX2.StartOffset = PlankDepth;
                    AxisX2.EndOffset = 0;
                    // AxisX2.Width = _bundingRec.Width;
                    AxisX2.SetScrollBar("AxisX");
                    //AxisX2.CreateVisualObject(Chart, "AxisX");
                    //Chart._rightAxisPanel.Children.Add(AxisX2.Visual);
                }
            }

            if (AxisY2 != null)
            {
                AxisY2.ApplyStyleFromTheme(Chart, "AxisY");

                if (PlotDetails.ChartOrientation == ChartOrientationType.Vertical)
                {
                    AxisY2.StartOffset = 0;
                    AxisY2.EndOffset = PlankOffset;
                    // AxisY2.Height = _bundingRec.Height;
                    AxisY2.SetScrollBar("AxisY");
                    //AxisY2.CreateVisualObject(Chart, "AxisY");
                    //Chart._rightAxisPanel.Children.Add(AxisY2.Visual);
                }
                else
                {
                    AxisY2.StartOffset = PlankOffset; //PlankThickness
                    AxisY2.EndOffset = 0;
                    //AxisY2.Height = _bundingRec.Height;
                    AxisY2.SetScrollBar("AxisY");
                    //AxisY2.CreateVisualObject(Chart, "AxisY");
                    //Chart._topAxisPanel.Children.Add(AxisY2.Visual);
                }
            }
        }

        /// <summary>
        /// Apply Opacity to DataSeries and DataPoints
        /// </summary>
        private void ApplyOpacity()
        {
            foreach (DataSeries ds in Chart.InternalSeries)
            {
                if (ds.Faces != null)
                {
                    ds.Faces.Visual.Opacity = ds.Opacity;
                }

                foreach (DataPoint dp in ds.DataPoints)
                {
                    if (dp.Faces != null)
                    {
                        dp.Faces.Visual.Opacity = ds.Opacity * dp.Opacity;
                    }
                }
            }
        }

        /// <summary>
        /// Attach Events for each DataSeries and DataPoints
        /// </summary>
        /// <param name="Series"></param>
        private void AttachEventsToolTipHref2DataSeries(List<DataSeries> Series)
        {
            foreach (DataSeries ds in Chart.InternalSeries)
            {
                //if (ds.LegendMarker != null)
                //    ObservableObject.AttachEvents2Visual(ds, ds.LegendMarker.Visual);

                ds.AttachEvent2DataSeriesVisualFaces();

                foreach (DataPoint dp in ds.DataPoints)
                {
                    dp.AttachEvent2DataPointVisualFaces(dp);

                    #region Attach Tool Tips

                    if (dp.Faces != null)
                    {
                        if ((Chart as Chart).View3D && (ds.RenderAs == RenderAs.Pie || ds.RenderAs == RenderAs.Doughnut))
                        {
                            ObservableObject.AttachToolTip(Chart, dp.Faces.VisualComponents, dp.TextParser((String.IsNullOrEmpty(dp.ToolTipText) ? "#XValue, #YValue" : dp.ToolTipText)));
                        }
                        else
                        {
                            if (ds.RenderAs != RenderAs.Line && ds.RenderAs != RenderAs.Area && ds.RenderAs != RenderAs.StackedArea && ds.RenderAs != RenderAs.StackedArea100)
                                ObservableObject.AttachToolTip(Chart, dp.Faces.Visual, dp.TextParser((String.IsNullOrEmpty(dp.ToolTipText) ? "#XValue, #YValue" : dp.ToolTipText)));
                            if (dp.Marker != null)
                                ObservableObject.AttachToolTip(Chart, dp.Marker.Visual, dp.TextParser((String.IsNullOrEmpty(dp.ToolTipText) ? "#XValue, #YValue" : dp.ToolTipText)));
                        }
                    }

                    if (ds.RenderAs == RenderAs.Line)
                    {
                        if (dp.Marker != null)
                            ObservableObject.AttachToolTip(Chart, dp.Marker.Visual, dp.TextParser(dp.ToolTipText));
                    }

                    if (ds.RenderAs == RenderAs.Area || ds.RenderAs == RenderAs.StackedArea || ds.RenderAs == RenderAs.StackedArea100)
                    {
                        if (dp.Marker != null)
                            ObservableObject.AttachToolTip(Chart, dp.Marker.Visual, dp.TextParser(dp.ToolTipText));
                    }

                    #endregion

                    #region Attach Href

                    dp.SetHref2DataPointVisualFaces(dp);

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
        /// Creates the various regions required for drawing the charts
        /// </summary>
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

                    DrawHorizontalPlank(PlankDepth, PlankThickness, NewSize.Height);

                    if (NewSize.Height - PlankDepth - PlankThickness > 0)
                        DrawVerticalPlank(NewSize.Height - PlankDepth - PlankThickness, PlankDepth, 0.25, plankOpacity);

                    // Set the chart canvas size
                    chartCanvasHeight = NewSize.Height - PlankOffset;

                    //chartCanvasWidth = PlotAreaCanvas.ActualWidth - PlankDepth;
                    chartCanvasWidth = chartSize - PlankDepth;
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
        /// Creates the various regions required for drawing the charts
        /// </summary>
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
                    DrawVerticalPlank(PlankDepth, PlankThickness);

                    // if (NewSize.Width - PlankDepth - PlankThickness > 0)
                    //    DrawHorizontalPlank(NewSize.Width - PlankThickness, PlankDepth, 0.25, 0.3);

                    // Set the chart canvas size
                    // chartCanvasHeight = PlotAreaCanvas.ActualHeight - PlankDepth;
                    chartCanvasHeight = chartSize - PlankDepth;
                    chartCanvasWidth = NewSize.Width - PlankOffset;

                }
                else
                {
                    // Set the chart canvas size
                    // chartCanvasHeight = PlotAreaCanvas.ActualHeight;
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
        /// Creates the various regions required for drawing the charts
        /// </summary>
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

        void PlottingCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            PlottingCanvas.Loaded -= new RoutedEventHandler(PlottingCanvas_Loaded);

            Chart.RENDER_LOCK = false;

            if (Chart._renderLapsedCounter >= 1)
            {
                Chart.Render();
            }

            Animate();

            Chart._internalAnimationEnabled = false;
        }

        /// <summary>
        /// Add Title visual to DrawingArea
        /// </summary>
        /// <param name="title">Title to add</param>
        /// <param name="panel">Where to add</param>
        private void AddTitle(Chart chart, Title title, Panel panel, Double width, Double height)
        {
            // Set parent
            title.Chart = chart;

        UPX1:
            // Create Title Visual Object
            title.CreateVisualObject();

            if (title.VerticalAlignment == VerticalAlignment.Top || title.VerticalAlignment == VerticalAlignment.Bottom 
                ||(title.VerticalAlignment == VerticalAlignment.Center && title.HorizontalAlignment == HorizontalAlignment.Center))
            {
                if (title.TextBlockDesiredSize.Width > width && (chart.ActualWidth - width) < width)
                {
                    title.IsNotificationEnable = false;
                    title.FontSize -= 1;
                    title.IsNotificationEnable = true;
                    goto UPX1;
                }
            }
            else
            {
                if (title.TextBlockDesiredSize.Width > height && (chart.ActualHeight - height) < height)
                {
                    title.IsNotificationEnable = false;
                    title.FontSize -= 1;
                    title.IsNotificationEnable = true;
                    goto UPX1;
                }
            }

            // Attach event handler for the event PropertyChanged 
            title.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(title_PropertyChanged);

            // Add title Visual as children of panel
            panel.Children.Add(title.Visual);
        }

        #endregion

        #region Internal Methods

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
                    return MarkerTypes.Line;

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

        internal event EventHandler PropertyChanged;    // PropertyChanged event handler.

        #endregion

        #region Static Methods

        /// <summary>
        /// converts value to position
        /// </summary>
        private static Double ValueToPosition(Double positionMin, Double positionMax, Double valueMin, Double valueMax, Double value)
        {
            return ((value - valueMin) / (valueMax - valueMin)) * (positionMax - positionMin);
        }

        /// <summary>
        /// Converts position to value
        /// </summary>
        private static Double PositionToValue(Double positionMin, Double positionMax, Double valueMin, Double valueMax, Double position)
        {
            return ((position) / (positionMax - positionMin) * (valueMax - valueMin)) + valueMin;
        }

        #endregion

        #region Data

        static Double GRID_ANIMATION_DURATION = 1;              // Grig Animation duration
        static Double CHART_SCROLLVIEWER_OFFSET = 1;            // Chart scroll-viewer Offset
        internal Int32 _redrawCount = 0;                        // No of redrawing chart for a single render call 
        internal Int32 _renderCount = 0;                        // Used for Testing
        Boolean _isAnimationFired = false;                      // Is animation is fired.

        #endregion
    }
}
