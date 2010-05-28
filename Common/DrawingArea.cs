#if WPF

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Markup;
using System.IO;
using System.Xml;
using System.Threading;
using System.Windows.Automation.Peers;
using System.Windows.Automation;
using System.Globalization;
using System.Diagnostics;

#else
using System;
using System.Windows;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Windows.Markup;
using System.Collections.ObjectModel;
using System.Globalization;
#endif
using Visifire.Charts;
namespace Visifire.Commons
{
    /// <summary>
    /// ChartArea, the maximum area available for drawing a chart in Visifire Chart Control
    /// </summary>
    internal class DrawingArea : Grid
    {
        #region Public Methods

        public DrawingArea()
        {
            
        }

        public DrawingArea(FrameworkElement parent, Double Height, Double Width)
        {
            _parent = parent;
            _parent.SizeChanged += new SizeChangedEventHandler(Parent_SizeChanged);

            this.Height = Height;
            this.Width = Width;

            CreateLayout();
        }

        public DrawingArea(FrameworkElement parent)
        {
            _parent = parent;
            _parent.SizeChanged += new SizeChangedEventHandler(Parent_SizeChanged);

            // Set Size
            this.Height = (_parent as Chart)._rootElement.ActualHeight;
            this.Width = (_parent as Chart)._rootElement.ActualWidth;

            // Set Default template
            CreateLayout();
        }
        
        #endregion

        #region Public Properties

        #endregion

        #region Public Events

        #endregion

        #region Protected Methods

        #endregion

        #region Internal Properties

        /// <summary>
        /// Left Panel of DesignArea
        /// </summary>
        internal StackPanel LeftPanel
        {
            get { return _leftPanel; }
        }

        /// <summary>
        /// Left Panel for holding titles
        /// </summary>
        internal StackPanel LeftTitlePanel
        {
            get { return _leftTitlePanel; }
        }

        /// <summary>
        /// Left Panel for holding legends
        /// </summary>
        internal StackPanel LeftLegendPanel
        {
            get { return _leftLegendPanel; }
        }

        /// <summary>
        /// Right Panel of DesignArea
        /// </summary>
        internal StackPanel RightPanel
        {
            get { return _rightPanel; }
        }

        /// <summary>
        /// Right Panel for holding titles
        /// </summary>
        internal StackPanel RightTitlePanel
        {
            get { return _rightTitlePanel; }
        }

        /// <summary>
        /// Right Panel for holding legends
        /// </summary>
        internal StackPanel RightLegendPanel
        {
            get { return _rightLegendPanel; }
        }

        /// <summary>
        /// Top Panel of DesignArea
        /// </summary>
        internal StackPanel TopPanel
        {
            get { return _topPanel; }
        }

        /// <summary>
        /// Top Panel for holding titles
        /// </summary>
        internal StackPanel TopTitlePanel
        {
            get { return _topTitlePanel; }
        }

        /// <summary>
        /// Top Panel for holding legends
        /// </summary>
        internal StackPanel TopLegendPanel
        {
            get { return _topLegendPanel; }
        }


        /// <summary>
        /// Bottom Panel of DesignArea
        /// </summary>
        internal StackPanel BottomPanel
        {
            get { return _bottomPanel; }
        }

        /// <summary>
        /// Bottom Panel for holding titles
        /// </summary>
        internal StackPanel BottomTitlePanel
        {
            get { return _bottomTitlePanel; }
        }

        /// <summary>
        /// Bottom Panel for holding legends
        /// </summary>
        internal StackPanel BottomLegendPanel
        {
            get { return _bottomLegendPanel; }
        }

        /// <summary>
        /// Center Panel of ChartArea, which holds the Title, Legend etc.
        /// </summary>
        internal StackPanel CenterPanelDockInsidePlotArea
        {
            get { return _centerPanelDockInsidePlotArea; }
        }

        internal StackPanel CenterPanelDockOutsidePlotArea
        {
            get { return _centerPanelDockOutsidePlotArea; }
        }

        internal Grid DrawingRegion
        {
            get { return _drawingRegion; }
        }

        #endregion

        #region Private Properties

        #endregion

        #region Private Delegates

        #endregion

        #region Private Methods

        /// <summary>
        /// Set the drawing area size as its parent size
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Parent_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.Height = (sender as FrameworkElement).ActualHeight;
            this.Width = (sender as FrameworkElement).ActualWidth;
        }

        /// <summary>
        /// Set ChartArea template
        /// </summary>
        private void CreateLayout()
        {
            if (RowDefinitions.Count == 0)
            {   
                // Create 3 rows of the grid
                this.RowDefinitions.Add(new RowDefinition());
                this.RowDefinitions.Add(new RowDefinition());
                this.RowDefinitions.Add(new RowDefinition());

                // Inner grid for the second row
                _innerGrid = new Grid();
                
                _innerGrid.SetValue(RowProperty, 1);
                _innerGrid.SetValue(ColumnProperty, 0);
                _innerGrid.RowDefinitions.Add(new RowDefinition());
                _innerGrid.ColumnDefinitions.Add(new ColumnDefinition());
                _innerGrid.ColumnDefinitions.Add(new ColumnDefinition());
                _innerGrid.ColumnDefinitions.Add(new ColumnDefinition());

                // Drawing region inside inner grid
                _drawingRegion = new Grid();
                _drawingRegion.HorizontalAlignment = HorizontalAlignment.Stretch;
                _drawingRegion.VerticalAlignment = VerticalAlignment.Stretch;
                _drawingRegion.SetValue(RowProperty, 0);
                _drawingRegion.SetValue(ColumnProperty, 1);
                _innerGrid.Children.Add(_drawingRegion);

                // Left panel inside inner grid
                _leftPanel = new StackPanel();
                _leftPanel.HorizontalAlignment = HorizontalAlignment.Left;
                _leftPanel.VerticalAlignment = VerticalAlignment.Stretch;
                _leftPanel.Orientation = Orientation.Horizontal;
                _leftPanel.SetValue(ColumnProperty, 0);
                _leftPanel.SetValue(RowProperty, 0);
                _leftPanel.SizeChanged += new SizeChangedEventHandler(_panelLeft_SizeChanged);
                _innerGrid.Children.Add(_leftPanel);

                // Right panel inside inner grid
                _rightPanel = new StackPanel();
                _rightPanel.HorizontalAlignment = HorizontalAlignment.Right;
                _rightPanel.VerticalAlignment = VerticalAlignment.Stretch;
                _rightPanel.Orientation = Orientation.Horizontal;
                _rightPanel.SetValue(ColumnProperty, 2);
                _rightPanel.SetValue(RowProperty, 0);
                _rightPanel.SizeChanged += new SizeChangedEventHandler(_panelRight_SizeChanged);
                _innerGrid.Children.Add(_rightPanel);
                this.Children.Add(_innerGrid);

                // Left panel in side inner grid
                _topPanel = new StackPanel();
                _topPanel.HorizontalAlignment = HorizontalAlignment.Stretch;
                _topPanel.VerticalAlignment = VerticalAlignment.Top;
                _topPanel.Orientation = Orientation.Vertical;
                _topPanel.SetValue(ColumnProperty, 0);
                _topPanel.SetValue(RowProperty, 0);
                _topPanel.SizeChanged += new SizeChangedEventHandler(panelTop_SizeChanged);               
                this.Children.Add(_topPanel);

                // Left panel in side inner grid
                _bottomPanel = new StackPanel();
                _bottomPanel.HorizontalAlignment = HorizontalAlignment.Stretch;
                _bottomPanel.VerticalAlignment = VerticalAlignment.Bottom;
                _bottomPanel.Orientation = Orientation.Vertical;
                _bottomPanel.SetValue(ColumnProperty, 0);
                _bottomPanel.SetValue(RowProperty, 2);
                _bottomPanel.SizeChanged += new SizeChangedEventHandler(panelBottom_SizeChanged);
                this.Children.Add(_bottomPanel);

                if (_parent.GetType().Equals(typeof(Chart)))
                {
                    _centerPanelDockInsidePlotArea = (_parent as Chart)._centerDockInsidePolotAreaPanel;
                   _centerPanelDockOutsidePlotArea = (_parent as Chart)._centerDockOutsidePlouAreaPanel;
                }

                // Testing -- Start
                // this.ShowGridLines = true;
                // _panelLeft.Background = new SolidColorBrush(Colors.Cyan);
                // _panelRight.Background = new SolidColorBrush(Colors.Green);
                // _panelTop.Background = new SolidColorBrush(Colors.Blue);
                // _panelBottom.Background = new SolidColorBrush(Colors.Magenta);
                // _drawingRegion.Background = new SolidColorBrush(Colors.Yellow);
                // _innerGrid.Background = new SolidColorBrush(Colors.Gray);
                // _centerPanel.Background = new SolidColorBrush(Colors.Magenta);
                //  Testing -- End 
                
                CreatePanels4Titles();
                CreatePanels4Legends();
            }
        }

        /// <summary>
        /// Create panels for Titles
        /// </summary>
        private void CreatePanels4Titles()
        {
            _topTitlePanel = new StackPanel();
            _topTitlePanel.HorizontalAlignment = HorizontalAlignment.Stretch;
            _topTitlePanel.VerticalAlignment = VerticalAlignment.Top;
            _topTitlePanel.Orientation = Orientation.Vertical;

            _topPanel.Children.Add(_topTitlePanel);

            _bottomTitlePanel = new StackPanel();
            _bottomTitlePanel.HorizontalAlignment = HorizontalAlignment.Stretch;
            _bottomTitlePanel.VerticalAlignment = VerticalAlignment.Bottom;
            _bottomTitlePanel.Orientation = Orientation.Vertical;

            _leftTitlePanel = new StackPanel();
            _leftTitlePanel.HorizontalAlignment = HorizontalAlignment.Left;
            _leftTitlePanel.VerticalAlignment = VerticalAlignment.Stretch;
            _leftTitlePanel.Orientation = Orientation.Horizontal;

            _leftPanel.Children.Add(_leftTitlePanel);

            _rightTitlePanel = new StackPanel();
            _rightTitlePanel.HorizontalAlignment = HorizontalAlignment.Right;
            _rightTitlePanel.VerticalAlignment = VerticalAlignment.Stretch;
            _rightTitlePanel.Orientation = Orientation.Horizontal;

            _rightPanel.Children.Add(_rightTitlePanel);

        }
        
        /// <summary>
        /// Create panels for legends
        /// </summary>
        private void CreatePanels4Legends()
        {   
            _topLegendPanel = new StackPanel();
            _topLegendPanel.HorizontalAlignment = HorizontalAlignment.Stretch;
            _topLegendPanel.VerticalAlignment = VerticalAlignment.Top;
            _topLegendPanel.Orientation = Orientation.Vertical;

            _topPanel.Children.Add(_topLegendPanel);

            _bottomLegendPanel = new StackPanel();
            _bottomLegendPanel.HorizontalAlignment = HorizontalAlignment.Stretch;
            _bottomLegendPanel.VerticalAlignment = VerticalAlignment.Bottom;
            _bottomLegendPanel.Orientation = Orientation.Vertical;

            _bottomPanel.Children.Add(_bottomLegendPanel);
            _bottomPanel.Children.Add(_bottomTitlePanel);

            _leftLegendPanel = new StackPanel();
            _leftLegendPanel.HorizontalAlignment = HorizontalAlignment.Left;
            _leftLegendPanel.VerticalAlignment = VerticalAlignment.Stretch;
            _leftLegendPanel.Orientation = Orientation.Horizontal;

            _leftPanel.Children.Add(_leftLegendPanel);

            _rightLegendPanel = new StackPanel();
            _rightLegendPanel.HorizontalAlignment = HorizontalAlignment.Right;
            _rightLegendPanel.VerticalAlignment = VerticalAlignment.Stretch;
            _rightLegendPanel.Orientation = Orientation.Horizontal;

            _rightPanel.Children.Add(_rightLegendPanel);
        }

        #region "Panel's Size changed"

        private void _panelRight_SizeChanged(object sender, SizeChangedEventArgs e)
        {
#if SL
            _innerGrid.ColumnDefinitions[2].Width = new GridLength(e.NewSize.Width);
#else
            _innerGrid.ColumnDefinitions[2].Width = new GridLength(e.NewSize.Width);
#endif
        }

        void _panelLeft_SizeChanged(object sender, SizeChangedEventArgs e)
        {
#if SL
            _innerGrid.ColumnDefinitions[0].Width = new GridLength(e.NewSize.Width);
#else
            _innerGrid.ColumnDefinitions[0].Width = new GridLength(e.NewSize.Width);
#endif
        }
        
        void panelTop_SizeChanged(object sender, SizeChangedEventArgs e)
        {
#if SL
            this.RowDefinitions[0].Height = new GridLength(e.NewSize.Height);
#else
           this.RowDefinitions[0].Height = new GridLength(e.NewSize.Height);
#endif
        }

        void panelBottom_SizeChanged(object sender, SizeChangedEventArgs e)
        {
#if SL
            this.RowDefinitions[2].Height = new GridLength(e.NewSize.Height);
#else
            this.RowDefinitions[2].Height = new GridLength(e.NewSize.Height);
#endif
        }

        void panelCenter_SizeChanged(object sender, SizeChangedEventArgs e)
        {
#if SL
            this.RowDefinitions[1].Height = new GridLength(e.NewSize.Height);
#else
            this.RowDefinitions[1].Height = new GridLength(e.NewSize.Height);
#endif
        }

        #endregion

        #endregion

        #region Internal Methods

        #endregion

        #region Internal Events

        #endregion

        #region Data

        private FrameworkElement _parent;       // Control reference
        private StackPanel _leftPanel;          // Left panel for ChartArea
        private StackPanel _rightPanel;         // Right panel for ChartArea
        private StackPanel _topPanel;           // Top panel for ChartArea [Header of DrawingArea]
        private StackPanel _bottomPanel;        // Bottom panel for ChartArea [Footer of 
        private StackPanel _centerPanelDockInsidePlotArea;  // Center panel for ChartArea
        private StackPanel _centerPanelDockOutsidePlotArea;  // Center panel for ChartArea
        private Grid _drawingRegion;            // Canvas for drawing inside drawing region
        private Grid _innerGrid;                // Innner Grid
        private StackPanel _leftTitlePanel;     // Left Panel for holding titles
        private StackPanel _leftLegendPanel;    // Left Panel for holding legends
        private StackPanel _rightTitlePanel;    // Right Panel for holding titles
        private StackPanel _centerTitlePanel;   // Center Panel for horlding titles
        private StackPanel _rightLegendPanel;   // Right Panel for holding legends
        private StackPanel _topTitlePanel;      // Top Panel for holding titles
        private StackPanel _topLegendPanel;     // Top Panel for holding legends
        private StackPanel _bottomTitlePanel;   // Bottom Panel for holding titles
        private StackPanel _bottomLegendPanel;  // Bottom Panel for holding legends

        #endregion
    }
}
