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


using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls.Primitives;
using System.Collections.Generic;

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
    [TemplatePart(Name = Chart.CenterDockInsidePlotAreaPanelName, Type = typeof(StackPanel))]
    [TemplatePart(Name = Chart.CenterDockOutsidePlotAreaPanelName, Type = typeof(StackPanel))]
    [TemplatePart(Name = Chart.ToolTipCanvasName, Type = typeof(Canvas))]

    #endregion

    /// <summary>
    /// Visifire WPF Chart Control
    /// </summary>
    public partial class Chart
    {
        #region Public Methods

        /// <summary>
        /// Initializes a new instance of the Visifire.Charts.Chart class
        /// </summary>
        public Chart()
        {
            // Initializes the various properties of the chart 
            this.Init();

            // Apply default Style
            if (!_defaultStyleKeyApplied)
            {
                DefaultStyleKeyProperty.OverrideMetadata(typeof(Chart), new FrameworkPropertyMetadata(typeof(Chart)));
                _defaultStyleKeyApplied = true;
            }

            // Attach event handler for loaded event
            this.Loaded += new RoutedEventHandler(Chart_Loaded);

            // Attach event handler for size changed event
            this.SizeChanged += new SizeChangedEventHandler(Chart_SizeChanged);

            // Attach event handler for LayoutUpdated event
            this.LayoutUpdated += new EventHandler(Chart_LayoutUpdated);

            this.Unloaded += new RoutedEventHandler(Chart_Unloaded);
        }

        void Chart_Unloaded(object sender, RoutedEventArgs e)
        {
            if (Parent != null)
                return;

            if (InternalSeries != null)
            {
                foreach (DataSeries dataSeries in InternalSeries)
                {
                    foreach (DataPoint dp in dataSeries.InternalDataPoints)
                    {
                        dp.ClearInstanceRefs();
                    }

                    dataSeries.ClearInstanceRefs();
                }
            }

            if (ChartArea != null)
                ChartArea.ClearInstanceRefs();

            foreach (Axis axis in AxesX)
                axis.ClearInstanceRefs();

            foreach (Axis axis in AxesY)
                axis.ClearInstanceRefs();

            if (_rootElement != null)
                _rootElement.IsHitTestVisible = true;
        }

        /// <summary>
        /// Event handler for LayoutUpdated event of the chart
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Chart_LayoutUpdated(object sender, EventArgs e)
        {
            if (_currentVisibility == Visibility.Collapsed && this.Visibility == Visibility.Visible)
            {
                if (IsInDesignMode)
                    InvokeRender();
                else
                    Render();
            }

            _currentVisibility = this.Visibility;
        }

        /// <summary>
        /// Event handler for SizeChanged event of the chart
        /// </summary>
        void Chart_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (!Double.IsNaN(e.NewSize.Width) && !Double.IsNaN(e.NewSize.Height) && e.NewSize.Width != 0 && e.NewSize.Height != 0)
            {
                // Render the chart with new size
                if (IsInDesignMode)
                    InvokeRender();
                else
                {
                    Render();
                    #region Nortek added
                    if (IndicatorEnabled)
                    {
                        foreach (var ds in Series)
                        {
                            if (!internalSelectedXYValues.Equals(Non_Selection))
                            {
                                ChartArea.PositionIndicator(internalSelectedXYValues.Key, internalSelectedXYValues.Value);
                            }
                        }
                    }
                    #endregion
                }
            }
        }

        /// <summary>
        /// Event handler for loaded event of the chart
        /// </summary>
        void Chart_Loaded(object sender, RoutedEventArgs e)
        {
            // Render the chart with new size
            if (IsInDesignMode)
                InvokeRender();
            else
                Render();
        }

        #endregion

        #region Public Properties

        #endregion

        #region Public Events

        #endregion

        #region Protected Methods

        #endregion

        #region Internal Properties

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

        /// <summary>
        /// Whether the default style is applied
        /// </summary>
        private static Boolean _defaultStyleKeyApplied;

        /// <summary>
        /// Whether the chart is visible
        /// </summary>
        private Visibility _currentVisibility;

        #endregion

        #region Nortek added
        public bool IndicatorLocked
        {
            get
            {
                return indicatorLocked;
            }
        }

        public void ReleaseIndicator()
        {
            if (IndicatorEnabled)
            {
                indicatorLocked = false;
            }
        }

        public void LockIndicator()
        {
            if (IndicatorEnabled)
            {
                indicatorLocked = true;
            }
        }

        internal bool indicatorLocked = false;

        public static KeyValuePair<object, double> Non_Selection = new KeyValuePair<object, double>();

        public void BeginShowIndicator(Object xValue, Double yValue)
        {
            var selection = new KeyValuePair<object, double>(xValue, yValue);
            BeginShowIndicator(selection);
        }

        public void BeginShowIndicator(KeyValuePair<object, double> selection)
        {
            if (ChartArea != null && ChartArea.AxisX != null && IndicatorEnabled)
            {
                System.Windows.Threading.Dispatcher.CurrentDispatcher.BeginInvoke((Action)delegate() { ChartArea.PositionIndicator(selection.Key, selection.Value); internalSelectedXYValues = selection; }, null);
                
            }
        }

        public void HideIndicatorImidiately()
        {
            if (ChartArea != null)
            {
                ChartArea.HideIndicator();
                internalSelectedXYValues = Non_Selection;
            }
        }

        public Boolean YIndicatorEnabled
        {
            get { return (Boolean)GetValue(YIndicatorEnabledProperty); }
            set { SetValue(YIndicatorEnabledProperty, value); }
        }

        public static readonly DependencyProperty YIndicatorEnabledProperty =
           DependencyProperty.Register("YIndicatorEnabled",
           typeof(Boolean),
           typeof(Chart),
           new PropertyMetadata(OnYIndicatorEnabledPropertyChanged));
        private static void OnYIndicatorEnabledPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //Chart c = d as Chart;
            //c.InvokeRender();
        }

        public Boolean IndicatorLockable
        {
            get { return (Boolean)GetValue(IndicatorLockableProperty); }
            set { SetValue(IndicatorLockableProperty, value); }
        }

        public static readonly DependencyProperty IndicatorLockableProperty =
           DependencyProperty.Register("IndicatorLockable",
           typeof(Boolean),
           typeof(Chart),
           new PropertyMetadata(OnIndicatorLockablePropertyChanged));

        private static void OnIndicatorLockablePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //Chart c = d as Chart;
            //if (c != null)
            //{
            //    c.InvokeRender();
            //}
        }

        private EventHandler _rendering;
        public event EventHandler Rendering
        {
            remove
            {
                _rendering -= value;
            }
            add
            {
                _rendering += value;
            }
        }
        internal void FireRenderingEvent()
        {
            if (_rendering != null)
                _rendering(this, null);
        }

        public KeyValuePair<object, double> SelectedXYValues
        {
            get
            {
                return (KeyValuePair<object, double>)GetValue(SelectedXYValuesProperty);
            }
            set
            {
                SetValue(SelectedXYValuesProperty, value);
            }
        }

        public static readonly DependencyProperty SelectedXYValuesProperty = DependencyProperty.Register
            ("SelectedXYValues",
            typeof(KeyValuePair<object, double>),
            typeof(Chart), new PropertyMetadata(Visifire.Charts.Chart.Non_Selection, OnSelectedXYValuesPropertyChanged));

        internal KeyValuePair<object, double> internalSelectedXYValues = Visifire.Charts.Chart.Non_Selection;
        private static void OnSelectedXYValuesPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var chart = d as Chart;
            chart.internalSelectedXYValues = (KeyValuePair<object, double>)e.NewValue;
        }
        #endregion
    }
}
