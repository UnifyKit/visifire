using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Browser;
using System.Globalization;
using System.Linq;
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
    //[TemplatePart(Name = Chart.LeftInnerPanelName, Type = typeof(StackPanel))]
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

    [System.Windows.Browser.ScriptableType]
    public partial class Chart
    {
        #region Public Methods
        /// <summary>
        /// Constructor for the Chart class, Initializations should be done here
        /// </summary>
        public Chart()
        {
            // Initializes the various properties of the chart 
            Init();

            // Apply default template
            DefaultStyleKey = typeof(Chart);

            // Attach event handler for size changed event
            SizeChanged += new SizeChangedEventHandler(Chart_SizeChanged);

            // Attaching loaded event with Chart
            this.Loaded += new RoutedEventHandler(Chart_Loaded);

        }



        ///// <summary>
        ///// Sets value for specific property of chart
        ///// This function is used for setting property from JavaScript only
        ///// </summary>
        ///// <param name="propertyName">Name of the property as String</param>
        ///// <param name="value">Property Value as String</param>
        //[System.Windows.Browser.ScriptableMember()]
        //public void SetPropertyFromJs(String propertyName, String value)
        //{
        //    try
        //    {
        //        JsHelper.SetProperty(this, propertyName, value);
        //    }
        //    catch (Exception e)
        //    {
        //        String s = String.Format(@"Unable to update {0} property, Property not found.\n [{1}]", propertyName, e.Message);
        //        throw new Exception(s);
        //    }
        //}

        #endregion

        #region Public Properties

        /// <summary>
        /// Height of the chart
        /// </summary>
        [ScriptableMember]
        public new Double Height
        {
            get
            {
                return (Double)GetValue(HeightProperty);
            }
            set
            {
                SetValue(HeightProperty, value);
            }
        }

        /// <summary>
        /// Width of the chart
        /// </summary>
        [ScriptableMember]
        public new Double Width
        {
            get
            {
                return (Double)GetValue(WidthProperty);
            }
            set
            {
                SetValue(WidthProperty, value);
            }
        }

        /// <summary>
        /// LogLevel is used to keep track the value of loglevel, this property is set from CreateChart() function in App Class in project SLVisifireChartsXap
        /// This Property can not be used by user.
        /// </summary>
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public Int32 LogLevel
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

#if SL
        /// <summary>
        /// Logger window
        /// </summary>
        internal Logger LoggerWindow
        {
            get;
            set;
        }
#endif

        #endregion

        #region Private Delegates

        #endregion

        #region Private Methods

        private void Chart_Loaded(object sender, RoutedEventArgs e)
        {
            // Render the chart with new size
            CallRender();
        }

        /// <summary>
        /// Event handler for managing the change in size of the chart
        /// </summary>
        private void Chart_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            // Render the chart with new size
            CallRender();
        }

        #endregion
        
        #region Internal Methods

        /// <summary>
        /// Create LogViewer
        /// </summary>
        internal void CreateLogViewer()
        {   
            if (_rootElement != null)
            {   
                // Create new Logger
                LoggerWindow = new Logger();

                // Set Logger properties
                LoggerWindow.HorizontalAlignment = HorizontalAlignment.Stretch;
                LoggerWindow.VerticalAlignment = VerticalAlignment.Stretch;
                LoggerWindow.Visibility = Visibility.Collapsed;
                LoggerWindow.SetValue(Canvas.ZIndexProperty, 6);

                LoggerWindow.Log("Copy & Paste the contents of this log in www.visifire.com/forums for support.");

                // Add Logger to root element
                _rootElement.Children.Add(LoggerWindow);
            }
        }

        #endregion

        #region Internal Events

        #endregion

        #region Data

        #endregion
    }
}
