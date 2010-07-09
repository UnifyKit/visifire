using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Linq;
using Visifire.Commons;

namespace Visifire.Charts
{
    public partial class Axis
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

        internal Canvas CircularAxisVisual
        {
            get;
            set;
        }

        internal Path CircularPath
        {
            get;
            set;
        }

        /// <summary>
        /// Details about plot groups
        /// </summary>
        internal CircularPlotDetails CircularPlotDetails
        {
            get;
            set;
        }

        internal Storyboard Storyboard
        {
            get;
            set;
        }

        #endregion

        #region Private Properties

        #endregion

        #region Private Delegates

        #endregion

        #region Private Methods

        /// <summary>
        /// Apply Events, ToolTips and Href properties for circular Axis
        /// </summary>
        private void ApplyAxisProperties4CircularChart()
        {
            AxisElementsContainer.Cursor = (Cursor == null) ? Cursors.Arrow : Cursor;
            AttachHref(Chart, AxisElementsContainer, Href, HrefTarget);
            AttachToolTip(Chart, this, AxisElementsContainer);
            AttachEvents2Visual(this, AxisElementsContainer);
            AxisElementsContainer.Opacity = this.InternalOpacity;
        }

        /// <summary>
        /// Apply circular path properties for AxisX 
        /// </summary>
        private void ApplyCircularPathProperties()
        {
            CircularPath.Stroke = LineColor;
            CircularPath.StrokeThickness = (Double)LineThickness;
            CircularPath.Fill = Background;
        }

        /// <summary>
        /// Create Axes for Circular chart
        /// </summary>
        private void CreateAxesForCircularChart()
        {
            if (AxisOrientation == AxisOrientation.Circular)
            {
                CircularAxisVisual = new Canvas() { Width = Width, Height = Height };

                SetUpAxisManager();
                SetUpAxisLabels();

                IsNotificationEnable = true;
                AxisLabels.IsNotificationEnable = true;

                ApplyAxisSettings4CircularChart();

                if (!(Boolean)this.Enabled)
                {
                    CircularAxisVisual.Visibility = Visibility.Collapsed;
                }

                // Apply animation for circular axis
                if ((Chart as Chart)._internalAnimationEnabled)
                {
                    Storyboard = new Storyboard();
                    CircularAxisVisual.RenderTransformOrigin = new Point(0.5, 0.5);

                    AnimationHelper.ApplyScaleAnimation(ScaleDirection.ScaleX, Storyboard, CircularAxisVisual, 0, 1, new TimeSpan(0, 0, 0, 1, 200), new TimeSpan(0, 0, 0, 0, 200), true);
                    AnimationHelper.ApplyScaleAnimation(ScaleDirection.ScaleY, Storyboard, CircularAxisVisual, 0, 1, new TimeSpan(0, 0, 0, 1, 200), new TimeSpan(0, 0, 0, 0, 200), true);

                    if (AxisLabels.Visual != null)
                    {
                        Canvas axislabelsVisual = AxisLabels.Visual;
                        AnimationHelper.ApplyOpacityAnimation(axislabelsVisual, AxisLabels, Storyboard, 0.5, 1, 0, 1);
                    }
                }
            }
            else
            {
                CircularAxisVisual = new Canvas() { Width = Width, Height = Height };

                SetUpAxisManager();
                SetUpTicks();
                SetUpGrids();
                SetUpAxisLabels();

                ApplyAxisSettings4CircularChart();

                if (!(Boolean)this.Enabled)
                {
                    CircularAxisVisual.Visibility = Visibility.Collapsed;
                }

                // Apply animation for circular axis
                if ((Chart as Chart)._internalAnimationEnabled)
                {
                    Storyboard = new Storyboard();
                    CircularAxisVisual.RenderTransformOrigin = new Point(0.5, 0.5);

                    AnimationHelper.ApplyScaleAnimation(ScaleDirection.ScaleX, Storyboard, CircularAxisVisual, 0, 1, new TimeSpan(0, 0, 0, 1, 200), new TimeSpan(0, 0, 0, 0, 200), true);
                    AnimationHelper.ApplyScaleAnimation(ScaleDirection.ScaleY, Storyboard, CircularAxisVisual, 0, 1, new TimeSpan(0, 0, 0, 1, 200), new TimeSpan(0, 0, 0, 0, 200), true);

                    if (AxisLabels.Visual != null)
                    {
                        Canvas axislabelsVisual = AxisLabels.Visual;
                        AnimationHelper.ApplyOpacityAnimation(axislabelsVisual, AxisLabels, Storyboard, 0.5, 1, 0, 1);
                    }
                }
            }
        }

        /// <summary>
        /// Applies axis settings for circular type axis
        /// </summary>
        private void ApplyAxisSettings4CircularChart()
        {
            if (AxisOrientation == AxisOrientation.Circular)
            {
                CircularPath = new Path();

                // Set the parameters for the axis labels
                AxisLabels.Placement = PlacementTypes.Circular;
                AxisLabels.Width = Width;
                AxisLabels.Height = Height;

                // Generate the visual object for the required elements
                AxisLabels.CreateVisualObject();

                if(AxisLabels.Visual != null)
                    CircularAxisVisual.Children.Add(AxisLabels.Visual);

                ApplyCircularPathProperties();

                CircularPath.Data = GetPathGeometry(CircularPlotDetails.ListOfPoints4CircularAxis);

                CircularAxisVisual.Children.Add(CircularPath);
            }
            else
            {
                Double height = CircularPlotDetails.Radius;

                // Set the parameters for the axis labels
                AxisLabels.Placement = PlacementTypes.Left;
                AxisLabels.Height = height;

                for (Int32 i = 0; i < CircularPlotDetails.ListOfPoints4CircularAxis.Count; i++)
                {
                    if(i == 0)
                        CreateAxisYElements4CircularChart(height, CircularPlotDetails.ListOfPoints4CircularAxis[i], CircularPlotDetails.MinAngleInDegree, i, CircularPlotDetails.Center, true);
                    else
                        CreateAxisYElements4CircularChart(height, CircularPlotDetails.ListOfPoints4CircularAxis[i], CircularPlotDetails.MinAngleInDegree, i, CircularPlotDetails.Center, false);
                }

                CleanUpGrids();

                foreach (ChartGrid grid in Grids)
                {
                    grid.IsNotificationEnable = false;
                    grid.Chart = Chart;

                    grid.ApplyStyleFromTheme(Chart, "Grid");

                    if (grid.Visual == null)
                    {
                        grid.CreateVisualObject(Width, height, (Chart as Chart)._internalAnimationEnabled, ChartArea.GRID_ANIMATION_DURATION);

                        if (grid.Visual != null)
                            CircularAxisVisual.Children.Add(grid.Visual);
                    }
                    else
                        grid.CreateVisualObject(Width, height, (Chart as Chart)._internalAnimationEnabled, ChartArea.GRID_ANIMATION_DURATION);

                    if (grid.Visual != null)
                        grid.Visual.SetValue(Canvas.ZIndexProperty, (Int32)(-1000));

                    grid.IsNotificationEnable = true;
                }
            }
        }

        private void CleanUpGrids()
        {
            foreach (ChartGrid grid in Grids)
            {
                if (grid.Visual != null)
                {
                    CircularAxisVisual.Children.Remove(grid.Visual);
                    grid.Visual.Children.Clear();
                    grid.Visual = null;
                }
            }
        }

        /// <summary>
        /// Create AxisY elements for circular chart
        /// </summary>
        /// <param name="height"></param>
        /// <param name="point"></param>
        /// <param name="minAngle"></param>
        /// <param name="index"></param>
        /// <param name="center"></param>
        /// <param name="isAxisLabelsEnabled"></param>
        private void CreateAxisYElements4CircularChart(Double height, Point point, Double minAngle, Double index, Point center, Boolean isAxisLabelsEnabled)
        {
            AxisElementsContainer = new StackPanel() { Background = InternalBackground };
            AxisElementsContainer.Orientation = Orientation.Horizontal;

            ApplyAxisProperties4CircularChart();

            CreateAxisLine(0, height, (Double)LineThickness / 2, (Double)LineThickness / 2, (Double)LineThickness, height);

            RotateTransform transform = null;

            if (isAxisLabelsEnabled)
            {
                AxisLabels.CreateVisualObject();

                if (AxisLabels.Visual != null)
                    AxisElementsContainer.Children.Add(AxisLabels.Visual);
            }
            else
            {
                transform = new RotateTransform();
                transform.Angle = minAngle * index;
            }

            Double ticksWidth = 0;

            List<Ticks> ticks = Ticks.Reverse().ToList();

            foreach (Ticks tick in ticks)
            {
                tick.SetParms(PlacementTypes.Left, Double.NaN, height);

                tick.CreateVisualObject();

                if (tick.Visual != null)
                {
                    AxisElementsContainer.Children.Add(tick.Visual);
                    ticksWidth += tick.Visual.Width;
                }
            }

            if (AxisLine != null)
                AxisElementsContainer.Children.Add(AxisLine);

            Double axisLabelsWidth = 0;
            if (isAxisLabelsEnabled)
            {
                if (AxisLabels.Visual != null)
                {
                    axisLabelsWidth = AxisLabels.Visual.Width;
                }
            }

            AxisElementsContainer.SetValue(Canvas.LeftProperty, point.X - axisLabelsWidth - ticksWidth);
            AxisElementsContainer.SetValue(Canvas.TopProperty, point.Y);

            if (!isAxisLabelsEnabled)
            {
                if (transform != null)
                {
                    AxisElementsContainer.RenderTransformOrigin = new Point(0.75, 0);
                    AxisElementsContainer.RenderTransform = transform;
                }
            }

            CircularAxisVisual.Children.Add(AxisElementsContainer);
        }

        #endregion

        #region Internal Methods

        internal Geometry GetPathGeometry(List<Point> listOfCircularPoints)
        {
            PathGeometry geometry = new PathGeometry();

            PathFigure pathFigure = new PathFigure();
            pathFigure.StartPoint = listOfCircularPoints[0];

            for (Int32 i = 1; i < listOfCircularPoints.Count; i++)
            {
                LineSegment segment = new LineSegment();
                segment.Point = listOfCircularPoints[i];
                pathFigure.Segments.Add(segment);
            }

            LineSegment lastSegment = new LineSegment();
            lastSegment.Point = listOfCircularPoints[0];
            pathFigure.Segments.Add(lastSegment);

            geometry.Figures.Add(pathFigure);

            return geometry;
        }

        /// <summary>
        /// Get the list of labels at the right and left side of the pie of a circular region
        /// </summary>
        /// <param name="labels">List<CircularLabel></param>
        /// <param name="labelsAtLeft">labels at left</param>
        /// <param name="labelsAtRight">labels at right</param>
        internal static void GetLeftAndRightLabels(List<CircularAxisLabel> labels, out List<CircularAxisLabel> labelsAtLeft, out List<CircularAxisLabel> labelsAtRight)
        {
            // Labels at angle >= 3 * Math.PI / 2 && angle < 0
            List<CircularAxisLabel> labelsR1 = (from cl in labels
                                            where cl.Angle >= 3 * Math.PI / 2
                                                orderby cl.Angle
                                                ascending
                                            select cl).ToList();

            // Labels at angle >= 0 && angle < <= Math.PI / 2
            List<CircularAxisLabel> labelsR2 = (from cl in labels
                                                where cl.Angle >= 0 && cl.Angle <= Math.PI / 2
                                                orderby cl.Angle ascending
                                            select cl).ToList();

            // Combine labels at right present at the right side of the circular region
            labelsAtRight = new List<CircularAxisLabel>();
            labelsAtRight.AddRange(labelsR1);
            labelsAtRight.AddRange(labelsR2);

            // All labels present at the right side of the circular region
            labelsAtLeft = (from cl in labels
                            where cl.Angle > Math.PI / 2 && cl.Angle < 3 * Math.PI / 2
                            orderby cl.Angle ascending
                            select cl).ToList();
        }

        #endregion

        #region Internal Events And Delegates

        #endregion

        #region Data

        #endregion
    }
}
