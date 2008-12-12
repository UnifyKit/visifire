using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls.Primitives;

namespace Visifire.Charts
{
    #region Templates

    [TemplatePart(Name = Chart.RootElementName, Type = typeof(Grid))]
    [TemplatePart(Name = Chart.ShadowGridName, Type = typeof(Grid))]
    [TemplatePart(Name = Chart.ChartBorderName, Type = typeof(Border))]
    [TemplatePart(Name = Chart.ChartLightingBorderName, Type = typeof(Rectangle))]
    [TemplatePart(Name = Chart.BevelCanvasName, Type = typeof(Canvas))]
    [TemplatePart(Name = Chart.ChartAreaGridName, Type = typeof(Grid))]
    [TemplatePart(Name = Chart.TopOuterPanelName, Type = typeof(StackPanel))]
    [TemplatePart(Name = Chart.TopOuterTitlePanelName, Type = typeof(StackPanel))]
    [TemplatePart(Name = Chart.TopOuterLegendPanelName, Type = typeof(StackPanel))]
    [TemplatePart(Name = Chart.BottomOuterPanelName, Type = typeof(StackPanel))]
    [TemplatePart(Name = Chart.BottomOuterLegendPanelName, Type = typeof(StackPanel))]
    [TemplatePart(Name = Chart.BottomOuterTitlePanelName, Type = typeof(StackPanel))]
    [TemplatePart(Name = Chart.LeftOuterPanelName, Type = typeof(StackPanel))]
    [TemplatePart(Name = Chart.LeftOuterTitlePanelName, Type = typeof(StackPanel))]
    [TemplatePart(Name = Chart.LeftOuterLegendPanelName, Type = typeof(StackPanel))]
    [TemplatePart(Name = Chart.RightOuterPanelName, Type = typeof(StackPanel))]
    [TemplatePart(Name = Chart.RightOuterLegendPanelName, Type = typeof(StackPanel))]
    [TemplatePart(Name = Chart.RightOuterTitlePanelName, Type = typeof(StackPanel))]
    [TemplatePart(Name = Chart.CenterOuterGridName, Type = typeof(Grid))]
    [TemplatePart(Name = Chart.TopAxisGridName, Type = typeof(Grid))]
    [TemplatePart(Name = Chart.TopAxisContainerName, Type = typeof(StackPanel))]
    [TemplatePart(Name = Chart.TopAxisPanelName, Type = typeof(StackPanel))]
    [TemplatePart(Name = Chart.TopAxisScrollBarName, Type = typeof(ScrollBar))]
    [TemplatePart(Name = Chart.LeftAxisGridName, Type = typeof(Grid))]
    [TemplatePart(Name = Chart.LeftAxisContainerName, Type = typeof(StackPanel))]
    [TemplatePart(Name = Chart.LeftAxisPanelName, Type = typeof(StackPanel))]
    [TemplatePart(Name = Chart.LeftAxisScrollBarName, Type = typeof(ScrollBar))]
    [TemplatePart(Name = Chart.RightAxisGridName, Type = typeof(Grid))]
    [TemplatePart(Name = Chart.RightAxisContainerName, Type = typeof(StackPanel))]
    [TemplatePart(Name = Chart.RightAxisScrollBarName, Type = typeof(ScrollBar))]
    [TemplatePart(Name = Chart.RightAxisPanelName, Type = typeof(StackPanel))]
    [TemplatePart(Name = Chart.BottomAxisGridName, Type = typeof(Grid))]
    [TemplatePart(Name = Chart.BottomAxisContainerName, Type = typeof(StackPanel))]
    [TemplatePart(Name = Chart.BottomAxisScrollBarName, Type = typeof(ScrollBar))]
    [TemplatePart(Name = Chart.BottomAxisPanelName, Type = typeof(StackPanel))]
    [TemplatePart(Name = Chart.CenterGridName, Type = typeof(Grid))]
    [TemplatePart(Name = Chart.InnerGridName, Type = typeof(Grid))]
    [TemplatePart(Name = Chart.TopInnerPanelName, Type = typeof(StackPanel))]
    [TemplatePart(Name = Chart.TopInnerTitlePanelName, Type = typeof(StackPanel))]
    [TemplatePart(Name = Chart.TopInnerLegendPanelName, Type = typeof(StackPanel))]
    [TemplatePart(Name = Chart.BottomInnerPanelName, Type = typeof(StackPanel))]
    [TemplatePart(Name = Chart.BottomInnerLegendPanelName, Type = typeof(StackPanel))]
    [TemplatePart(Name = Chart.BottomInnerTitlePanelName, Type = typeof(StackPanel))]
    [TemplatePart(Name = Chart.LeftInnerPanelName, Type = typeof(StackPanel))]
    [TemplatePart(Name = Chart.LeftInnerTitlePanelName, Type = typeof(StackPanel))]
    [TemplatePart(Name = Chart.LeftInnerLegendPanelName, Type = typeof(StackPanel))]
    [TemplatePart(Name = Chart.RightInnerPanelName, Type = typeof(StackPanel))]
    [TemplatePart(Name = Chart.RightInnerLegendPanelName, Type = typeof(StackPanel))]
    [TemplatePart(Name = Chart.RightInnerTitlePanelName, Type = typeof(StackPanel))]
    [TemplatePart(Name = Chart.CenterInnerGridName, Type = typeof(Grid))]
    [TemplatePart(Name = Chart.PlotAreaGridName, Type = typeof(Grid))]
    [TemplatePart(Name = Chart.PlotAreaScrollViewerName, Type = typeof(ScrollViewer))]
    [TemplatePart(Name = Chart.PlotGridName, Type = typeof(Canvas))]
    [TemplatePart(Name = Chart.PlotAreaShadowCanvasName, Type = typeof(Canvas))]
    [TemplatePart(Name = Chart.DrawingCanvasName, Type = typeof(Canvas))]
    //[TemplatePart(Name = Chart.PlotAreaBevelCanvasName, Type = typeof(Canvas))]
    [TemplatePart(Name = Chart.CenterDockInsidePlotAreaPanelName, Type = typeof(StackPanel))]
    [TemplatePart(Name = Chart.CenterDockOutsidePlotAreaPanelName, Type = typeof(StackPanel))]
    [TemplatePart(Name = Chart.ToolTipCanvasName, Type = typeof(Canvas))]
    [TemplatePart(Name = Chart.ToolTipName, Type = typeof(Border))]
    [TemplatePart(Name = Chart.ToolTipTextBlockName, Type = typeof(TextBlock))]

    #endregion

    /// <summary>
    /// Visifire WPF Chart Control
    /// </summary>
    public partial class Chart
    {

        #region Public Methods

        public Chart()
        {
            this.Init();

            // Apply default Style
            if (!_defaultStyleKeyApplied)
            {
                DefaultStyleKeyProperty.OverrideMetadata(typeof(Chart), new FrameworkPropertyMetadata(typeof(Chart)));
                _defaultStyleKeyApplied = true;
            }

            this.Loaded += new RoutedEventHandler(Chart_Loaded);
        }

        void Chart_Loaded(object sender, RoutedEventArgs e)
        {
            Render();
        }

        #endregion

        #region Public Properties

        #endregion

        #region Public Events

        #endregion

        #region Protected Methods

        /// <summary>
        /// Called to remeasure a control.
        /// </summary>
        /// <param name="constraint"> The maximum size that the method can return</param>
        /// <returns>The size of the control, up to the maximum specified by constraint</returns>
        protected override Size MeasureOverride(Size constraint)
        {
            return base.MeasureOverride(constraint);
        }


        /// <summary>
        /// When overridden in a derived class, participates in rendering operations
        /// that are directed by the layout system. The rendering instructions for this
        /// element are not used directly when this method is invoked, and are instead
        /// preserved for later asynchronous use by layout and drawing.
        /// </summary>
        /// <param name="drawingContext">
        /// The drawing instructions for a specific element. This context is provided
        /// to the layout system.
        /// </param>
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            if (IsTemplateApplied)
            {
                // Call Render
                CallRender();
            }
        }

        #endregion

        #region Internal Properties

        /// <summary>
        /// Describes visual content using draw, push, and pop commands.
        /// </summary>
        internal DrawingContext DrawingContext
        {
            get;
            set;
        }
        #endregion

        #region Private Delegates

        #endregion

        #region Private Methods

        #endregion

        #region Internal Methods

        #endregion

        #region Internal Events

        #endregion

        #region Data

        private static Boolean _defaultStyleKeyApplied = false;     // If ChartPropertyTypeMetaData Applied

        #endregion
    }

}
