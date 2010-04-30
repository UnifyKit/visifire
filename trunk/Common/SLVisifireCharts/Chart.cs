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
    [TemplatePart(Name = Chart.CenterDockInsidePlotAreaPanelName, Type = typeof(StackPanel))]
    [TemplatePart(Name = Chart.CenterDockOutsidePlotAreaPanelName, Type = typeof(StackPanel))]
    [TemplatePart(Name = Chart.ToolTipCanvasName, Type = typeof(Canvas))]

    #endregion
    
    [System.Windows.Browser.ScriptableType]
    public partial class Chart
    {
        #region Public Methods

        /// <summary>
        /// Initializes a new instance of the Visifire.Charts.Chart class
        /// </summary>
        public Chart()
        {
            // Initializes the various properties of the chart 
            Init();

            // Apply default template
            DefaultStyleKey = typeof(Chart);

            // Attach event handler for size changed event
            SizeChanged += new SizeChangedEventHandler(Chart_SizeChanged);

            // Attach event handler for loaded event
            this.Loaded += new RoutedEventHandler(Chart_Loaded);

        }

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
        /// LogLevel is used to keep track the value of loglevel, this property is set from CreateChart() function in App Class in project SLVisifireChartsXap. 
        /// This Property cannot be used by user.
        /// </summary>
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public Int32 LogLevel
        {
            get;
            set;
        }

#if SL
        /// <summary>
        /// If Chart is drawn using JavaScript, This property will be set to true from project SLVisifireChartsXap.
        /// </summary>
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public Boolean IsInJsMode
        {
            get;
            set;
        }
#endif

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

        /// <summary>
        /// Event handler for loaded event of the chart
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Chart_Loaded(object sender, RoutedEventArgs e)
        {
            // Render the chart with new size
            InvokeRender();
        }

        /// <summary>
        /// Event handler for SizeChanged event of the chart
        /// </summary>
        private void Chart_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            // This is done because of unexpected fire of SizeChanged event 
            if (_height != e.NewSize.Height || _width != e.NewSize.Width)
            {   
                _height = e.NewSize.Height;
                _width = e.NewSize.Width;

                RENDER_LOCK = false;

                // Render the chart with new size
                if (IsInDesignMode)
                    Render();
                else
                    InvokeRender();
            }
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

                LoggerWindow.Log("In case you are an Enterprise Customer, please create a ticket for priority support directly from Visifire Developers. Otherwise, please copy-paste the contents of this log in forum and our community members will help you.");
                LoggerWindow.Log("Ticket: http://visifire.com/support");
                LoggerWindow.Log("Forum: http://visifire.com/forums");

                // Add Logger to root element
                _rootElement.Children.Add(LoggerWindow);
            }
        }

        #endregion

        #region Internal Events

        #endregion

        #region Data
        Double _height, _width;
        #endregion
    }
}