#if WPF

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

#else
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

#endif
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.IO;
using Visifire.Commons.Controls;
using System.Linq;
using System.Windows.Data;

namespace Visifire.Commons
{
    /// <summary>
    /// Visifire Control base class
    /// </summary>
    public abstract class VisifireControl : VisifireElement
    {
        #region Public Methods

        /// <summary>
        /// Initializes a new instance of the Visifire.Commons.VisifireControl class
        /// </summary>
        public VisifireControl()
        {
            //ToolTipEnabled = true;
        }

        /// <summary>
        /// Accepts absolute or relative Uri, builds and returns absolute path
        /// </summary>
        /// <param name="path">Path as String</param>
        /// <returns>String</returns>
        public static String GetAbsolutePath(String path)
        {
#if SL && !WP
            String address, queryString;
            String BaseUri = System.Windows.Browser.HtmlPage.Document.DocumentUri.ToString();

            Int32 index = path.IndexOf('?');

            if (index == -1)
            {
                address = path;
                queryString = "";
            }
            else
            {
                address = path.Substring(0, index);
                queryString = path.Substring(index);
            }

            Uri ur = new Uri(address, UriKind.RelativeOrAbsolute);

            if (ur.IsAbsoluteUri)
            {
                return ur.AbsoluteUri + queryString;
            }
            else if (path.StartsWith("/"))
            {
                UriBuilder baseUri = new UriBuilder(BaseUri);
                UriBuilder newUri = new UriBuilder(baseUri.Scheme, baseUri.Host, baseUri.Port, address, queryString);
                return newUri.ToString();
            }
            else
            {
                UriBuilder baseUri = new UriBuilder(BaseUri);
                String sourcePath = baseUri.Path.Substring(0, baseUri.Path.LastIndexOf('/') + 1);
                UriBuilder newUri = new UriBuilder(baseUri.Scheme, baseUri.Host, baseUri.Port, sourcePath + address, queryString);
                return newUri.ToString();
            }
#else       
            return path;
#endif
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Identifies the Visifire.Commons.ToolTipEnabled dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Commons.ToolTipEnabled dependency property.
        /// </returns>
        public static readonly DependencyProperty ToolTipEnabledProperty = DependencyProperty.Register
            ("ToolTipEnabled",
            typeof(Boolean),
            typeof(VisifireControl),
            new PropertyMetadata(true, null));

#if SL &&!WP

        /// <summary>
        /// Sliverlight Object Id
        /// </summary>
        [System.Windows.Browser.ScriptableMember]
        public String ControlId
        {
            get;
            set;
        }
#endif

        /// <summary>
        /// Enables or disables all ToolTips in chart
        /// </summary>
#if SL &&!WP
        [System.Windows.Browser.ScriptableMember]
#endif
        public Boolean ToolTipEnabled
        {
            get
            {
                return (Boolean)GetValue(ToolTipEnabledProperty);
            }
            set
            {
                SetValue(ToolTipEnabledProperty, value);
            }
        }

        private String SeparaterText
        {
            get
            {
                return (String)GetValue(SeparaterTextProperty);
            }
            set
            {
                SetValue(SeparaterTextProperty, value);
            }
        }

        /// <summary>
        /// Identifies the Visifire.Commons.SeparaterText dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Commons.SeparaterText dependency property.
        /// </returns>
        private static readonly DependencyProperty SeparaterTextProperty = DependencyProperty.Register
            ("SeparaterText",
            typeof(String),
            typeof(VisifireControl),
            new PropertyMetadata("|", null));

        public String ZoomOutText
        {
            get
            {
                return (String)GetValue(ZoomOutTextProperty);
            }
            set
            {
                if (String.IsNullOrEmpty(value))
                    SeparaterText = "";
                else
                    SeparaterText = "|";

                SetValue(ZoomOutTextProperty, value);
            }
        }

        /// <summary>
        /// Identifies the Visifire.Commons.ZoomOutText dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Commons.ZoomOutText dependency property.
        /// </returns>
        public static readonly DependencyProperty ZoomOutTextProperty = DependencyProperty.Register
            ("ZoomOutText",
            typeof(String),
            typeof(VisifireControl),
            new PropertyMetadata("Zoom Out", null));

        public String ShowAllText
        {
            get
            {
                return (String)GetValue(ShowAllTextProperty);
            }
            set
            {
                if (String.IsNullOrEmpty(value))
                    SeparaterText = "";
                else
                    SeparaterText = "|";

                SetValue(ShowAllTextProperty, value);
            }
        }

        /// <summary>
        /// Identifies the Visifire.Commons.ShowAllText dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Commons.ShowAllText dependency property.
        /// </returns>
        public static readonly DependencyProperty ShowAllTextProperty = DependencyProperty.Register
            ("ShowAllText",
            typeof(String),
            typeof(VisifireControl),
            new PropertyMetadata("Show All", null));

        /// <summary>
        /// Whether the chart is in design mode or application mode
        /// </summary>
        internal Boolean IsInDesignMode
        {
            get
            {
#if WPF   
                return System.ComponentModel.DesignerProperties.GetIsInDesignMode(this);
#else
                return System.ComponentModel.DesignerProperties.GetIsInDesignMode(this);
#endif
            }
        }

        /// <summary>
        /// Enable/Disable the watermark. This property is applicable in Enterprise version only.
        /// </summary>
        [Obsolete]
        public Boolean Watermark
        {   
            get
            {
                return (Boolean)GetValue(WatermarkProperty);
            }
            set
            {
                SetValue(WatermarkProperty, value);
            }
        }

        /// <summary>
        /// Identifies the Visifire.Charts.VisifireControl.Watermark dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.VisifireControl.Watermark dependency property.
        /// </returns>
        public static readonly DependencyProperty WatermarkProperty = DependencyProperty.Register
            ("Watermark",
            typeof(Boolean),
            typeof(VisifireControl),
            new PropertyMetadata(true, OnWatermarkPropertyChanged));

        /// <summary>
        /// Enable/Disable the save icon
        /// </summary>
        public Boolean ToolBarEnabled
        {   
            get { return (Boolean)GetValue(ToolBarEnabledProperty); }
            set { SetValue(ToolBarEnabledProperty, value); }
        }
        
        /// <summary>
        /// Identifies the Visifire.Charts.Chart.SaveIconEnabled dependency property.  
        /// </summary>
        public static readonly DependencyProperty ToolBarEnabledProperty =
            DependencyProperty.Register("ToolBarEnabled",
            typeof(Boolean),
            typeof(VisifireControl),
            new PropertyMetadata(false, OnSaveIconEnabledPropertyChanged));

        #endregion

        #region Public Events And Delegates

        #endregion

        #region Protected Methods

        /// <summary>
        /// On WatermarkProperty value changed
        /// </summary>
        /// <param name="value"></param>
        protected virtual void OnWatermarkPropertyValueChanged(Boolean value)
        {

        }

        /// <summary>
        /// WaterMarkElementProperty changed call back function
        /// </summary>
        /// <param name="d">Chart</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        protected static void OnWatermarkPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            VisifireControl vc = d as VisifireControl;
            vc.OnWatermarkPropertyValueChanged((Boolean)e.NewValue);
        }

        /// <summary>
        /// Load ToolBar at the top-right side corner of the chart.
        /// </summary>
        protected void LoadToolBar()
        {
            _toolbarContainer = new StackPanel();
            _toolbarContainer.Orientation = Orientation.Horizontal;
            _toolbarContainer.HorizontalAlignment = HorizontalAlignment.Right;
            _toolbarContainer.VerticalAlignment = VerticalAlignment.Top;
            _toolbarContainer.Margin = new Thickness(0, 3, 5, 0);
            _toolbarContainer.SetValue(Canvas.ZIndexProperty, 90000);

            LoadWm();
            LoadSaveIcon();
            
            _rootElement.Children.Add(_toolbarContainer);
        }

        /// <summary>
        /// Load container for Zoom icons at the top-right corner of PlotArea
        /// </summary>
        protected void LoadZoomIcons()
        {
            _zoomIconContainer = new StackPanel();
            _zoomIconContainer.Orientation = Orientation.Horizontal;
            _zoomIconContainer.HorizontalAlignment = HorizontalAlignment.Right;
            _zoomIconContainer.VerticalAlignment = VerticalAlignment.Top;
            _zoomIconContainer.Margin = new Thickness(0, 3, 5, 0);
            _zoomIconContainer.SetValue(Canvas.ZIndexProperty, 95000);

            LoadZoomOutIcon();
            LoadSeparater();
            LoadShowAllIcon();

            _plotAreaGrid.Children.Add(_zoomIconContainer);
        }

        /// <summary>
        /// Load separater for Zoom icons
        /// </summary>
        private void LoadSeparater()
        {
            _zoomIconSeparater = new TextBlock();

            Binding binding = new Binding("SeparaterText");
            binding.Source = this;
            _zoomIconSeparater.SetBinding(TextBlock.TextProperty, binding);

            //_zoomIconSeparater.Text = "|";
            
            _zoomIconSeparater.FontSize = 9;
            _zoomIconSeparater.Foreground = new SolidColorBrush(Colors.Gray);
            _zoomIconSeparater.HorizontalAlignment = HorizontalAlignment.Right;
            _zoomIconSeparater.VerticalAlignment = VerticalAlignment.Center;
            _zoomIconSeparater.Margin = new Thickness(2, 0, 0, 0);
            _zoomIconSeparater.Visibility = Visibility.Collapsed;
            _zoomIconContainer.Children.Add(_zoomIconSeparater);
        }

        /// <summary>
        /// Load Show All text (for zooming) at the top-right corner of PlotArea
        /// </summary>
        private void LoadShowAllIcon()
        {
            _showAllTextBlock = new System.Windows.Controls.TextBlock();
            _showAllTextBlock.HorizontalAlignment = HorizontalAlignment.Right;
            _showAllTextBlock.VerticalAlignment = VerticalAlignment.Center;
            _showAllTextBlock.Margin = new Thickness(2, 0, 0, 0);
            _showAllTextBlock.Cursor = Cursors.Hand;

            Binding binding = new Binding("ShowAllText");
            binding.Source = this;
            _showAllTextBlock.SetBinding(TextBlock.TextProperty, binding);

            //_showAllTextBlock.Text = "Show All";

            _showAllTextBlock.FontSize = 9;

            _showAllTextBlock.MouseMove += delegate(Object sender, MouseEventArgs e)
            {   
                _showAllTextBlock.TextDecorations = TextDecorations.Underline;
            };

            _showAllTextBlock.MouseLeave += delegate(Object sender, MouseEventArgs e)
            {
                _showAllTextBlock.TextDecorations = null;
            };

            _showAllTextBlock.Visibility = Visibility.Collapsed;
            _zoomIconContainer.Children.Add(_showAllTextBlock);
        }

        /// <summary>
        /// Load Zoom Out text (for zooming) at the top-right corner of PlotArea
        /// </summary>
        private void LoadZoomOutIcon()
        {
            _zoomOutTextBlock = new System.Windows.Controls.TextBlock();
            _zoomOutTextBlock.HorizontalAlignment = HorizontalAlignment.Right;
            _zoomOutTextBlock.VerticalAlignment = VerticalAlignment.Center;
            _zoomOutTextBlock.Margin = new Thickness(2, 0, 0, 0);
            _zoomOutTextBlock.Cursor = Cursors.Hand;

            Binding binding = new Binding("ZoomOutText");
            binding.Source = this;
            _zoomOutTextBlock.SetBinding(TextBlock.TextProperty, binding);

            //_zoomOutTextBlock.Text = "Zoom Out";

            _zoomOutTextBlock.FontSize = 9;

            _zoomOutTextBlock.MouseMove += delegate(Object sender, MouseEventArgs e)
            {
                _zoomOutTextBlock.TextDecorations = TextDecorations.Underline;
            };

            _zoomOutTextBlock.MouseLeave += delegate(Object sender, MouseEventArgs e)
            {
                _zoomOutTextBlock.TextDecorations = null;
            };

            _zoomOutTextBlock.Visibility = Visibility.Collapsed;
            _zoomIconContainer.Children.Add(_zoomOutTextBlock);
        }

        /// <summary>
        /// Update position of the tooltip according to mouse position
        /// </summary>
        /// <param name="sender">FrameworkElement</param>
        /// <param name="e">MouseEventArgs</param>
        internal void UpdateToolTipPosition(object sender, MouseEventArgs e)
        {
            if (ToolTipEnabled && (Boolean)_toolTip.Enabled)
            {
                Double actualX = e.GetPosition(this).X;
                Double x = actualX;
                Double y = e.GetPosition(this).Y;

                #region Set position of ToolTip

                _toolTip.Measure(new Size(Double.MaxValue, Double.MaxValue));
                _toolTip.UpdateLayout();

                Size toolTipSize = Visifire.Commons.Graphics.CalculateVisualSize(_toolTip._borderElement);

                y = y - (toolTipSize.Height + 5);

                x = x - toolTipSize.Width / 2;

                if (x <= 0)
                {
                    x = e.GetPosition(this).X + 10;
                    y = e.GetPosition(this).Y + 20;

                    if ((y + toolTipSize.Height) >= this.ActualHeight)
                        y = this.ActualHeight - toolTipSize.Height;
                }

                if ((x + toolTipSize.Width) >= this.ActualWidth)
                {
                    x = e.GetPosition(this).X - toolTipSize.Width;
                    y = e.GetPosition(this).Y - toolTipSize.Height;
                }

                if (y < 0)
                    y = e.GetPosition(this).Y + 20;

                if (x + toolTipSize.Width > this.ActualWidth)
                    x = x + toolTipSize.Width - this.ActualWidth;

                if (toolTipSize.Width == _toolTip.MaxWidth)
                    x = 0;

                if (x < 0)
                    x = 0;

                // If tooltip still goes out of towards y
                if (!Double.IsNaN(this.ActualHeight) && y + toolTipSize.Height > this.ActualHeight)
                {
                    y = 0;
                    x = actualX + 10;
                    if (x <= 0)
                        x = e.GetPosition(this).X + 10;

                    if ((x + toolTipSize.Width) >= this.ActualWidth)
                        x = e.GetPosition(this).X - toolTipSize.Width;

                    if (x + toolTipSize.Width > this.ActualWidth)
                        x = x + toolTipSize.Width - this.ActualWidth;

                    if (toolTipSize.Width == _toolTip.MaxWidth)
                        x = 0;

                    if (x < 0)
                        x = 0;

                }

                _toolTip.SetValue(Canvas.LeftProperty, x);
                _toolTip.SetValue(Canvas.TopProperty, y);

                #endregion
            }
            else
            {
                _toolTip.Hide();
            }
        }

        private void LoadSaveIcon()
        {   
            _saveIconImage = new System.Windows.Controls.Image();
            _saveIconImage.HorizontalAlignment = HorizontalAlignment.Right;
            _saveIconImage.VerticalAlignment = VerticalAlignment.Center;
            _saveIconImage.Margin = new Thickness(2, 0, 0, 0);
            _saveIconImage.Width = _saveIconImage.Height = 14;
            _saveIconImage.Cursor = Cursors.Hand;

            _saveIconImage.MouseMove += delegate(Object sender, MouseEventArgs e)
            {
                _toolTip.Text = "Export as image";
                _toolTip.Show();

                if (_toolTip._callOutPath != null)
                    _toolTip.CallOutVisiblity = Visibility.Collapsed;

                UpdateToolTipPosition(sender, e);
            };

            _saveIconImage.MouseLeave += delegate(Object sender, MouseEventArgs e)
            {
                _toolTip.Text = "";
                _toolTip.Hide();
            };

            _saveIconImage.MouseLeftButtonUp += delegate { Save(null, Visifire.Charts.ExportType.Jpg, true); };
            Graphics.SetImageSource(_saveIconImage , "Visifire.Charts." + "save_icon.png");
            _saveIconImage.Visibility = ToolBarEnabled ? Visibility.Visible : Visibility.Collapsed;
            _toolbarContainer.Children.Add(_saveIconImage);
        }

        protected virtual void Save(String path, Visifire.Charts.ExportType exportType, Boolean showDilog)
        {   
            if (_saveIconImage != null)
            {   
                _saveIconImage.Visibility = Visibility.Collapsed;
                _toolTip.Hide();
                _toolbarContainer.UpdateLayout();
            }
#if SL      
            try
            {   
                WriteableBitmap bitmap = new WriteableBitmap(this, null);
#if !WP
                if (bitmap != null)
                {   
                    SaveFileDialog saveDlg = new SaveFileDialog();

                    saveDlg.Filter = "JPEG (*.jpg)|*.jpg|BMP (*.bmp)|*.bmp";
                    saveDlg.DefaultExt = ".jpg";
                    
                    if ((bool)saveDlg.ShowDialog())
                    {
                        using (Stream fs = saveDlg.OpenFile())
                        {
                            String[] filename = saveDlg.SafeFileName.Split('.');
                            String fileExt;

                            if (filename.Length >= 2)
                            {
                                fileExt = filename[filename.Length - 1];
                                exportType = (Visifire.Charts.ExportType)Enum.Parse(typeof(Visifire.Charts.ExportType), fileExt, true);
                            }
                            else
                                exportType = Visifire.Charts.ExportType.Jpg;

                            MemoryStream stream = Graphics.GetImageStream(bitmap, exportType);

                            // Get Bytes from memory stream and write into IO stream
                            byte[] binaryData = new Byte[stream.Length];
                            long bytesRead = stream.Read(binaryData, 0, (int)stream.Length);
                            fs.Write(binaryData, 0, binaryData.Length);
                        }
                    }
                }
#endif
            }
            catch (Exception ex)
            {
                if (_saveIconImage != null)
                {
                    _saveIconImage.Visibility = Visibility.Visible;
                }
                System.Diagnostics.Debug.WriteLine("Note: Please make sure that Height and Width of the chart is set properly.");
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
#else       
            // Matrix m = PresentationSource.FromVisual(Application.Current.MainWindow).CompositionTarget.TransformToDevice;
            // double dx = m.M11* 96;
            // double dy = m.M22 * 96;

            // Save current canvas transform
            Transform transform = this.LayoutTransform;
            
            // reset current transform (in case it is scaled or rotated)
            _rootElement.LayoutTransform = null;

            // Create a render bitmap and push the surface to it
            RenderTargetBitmap renderBitmap =
              new RenderTargetBitmap(
                    (int)(this.ActualWidth),
                    (int)(this.ActualHeight),
                    96d,
                    96d,
                    PixelFormats.Pbgra32);
            renderBitmap.Render(_rootElement);

            if (showDilog)
            {   
                Microsoft.Win32.SaveFileDialog saveDlg = new Microsoft.Win32.SaveFileDialog();

                saveDlg.Filter = "Jpg Files (*.jpg)|*.jpg|BMP Files (*.bmp)|*.bmp";
                saveDlg.DefaultExt = ".jpg";
                
                if ((bool)saveDlg.ShowDialog())
                {   
                    BitmapEncoder encoder;
               
                    if (saveDlg.FilterIndex == 2)
                        encoder = new BmpBitmapEncoder();
                    else
                        encoder = new JpegBitmapEncoder();

                    using (Stream fs = saveDlg.OpenFile())
                    {
                        encoder.Frames.Add(BitmapFrame.Create(renderBitmap));
                        // save the data to the stream
                        encoder.Save(fs);
                    }
                }
            }
            else
            {
                path = System.IO.Path.GetFullPath(path.Trim());
                String fileName = System.IO.Path.GetFileName(path);

                if (String.IsNullOrEmpty(fileName))
                {
                    fileName = "VisifireChart";
                    path += fileName;
                }

                switch (exportType)
                {
                    case Visifire.Charts.ExportType.Bmp:
                        path += ".bmp";
                        break;

                    default:
                        path += ".jpg";
                        break;
                }

                FileStream outStream;

                // Create a file stream for saving image
                using (outStream = new FileStream(path, FileMode.Create))
                {   
                    BitmapEncoder encoder;

                    switch (exportType)
                    {   
                        case Visifire.Charts.ExportType.Bmp:
                            encoder = new BmpBitmapEncoder();
                            break;
                            
                        default:
                            encoder = new JpegBitmapEncoder();
                            break;
                    }

                    // push the rendered bitmap to it
                    encoder.Frames.Add(BitmapFrame.Create(renderBitmap));

                    // save the data to the stream
                    encoder.Save(outStream);
                }

                if (outStream == null)
                    throw new System.IO.IOException("Unable to export the chart to an image as the specified path is invalid.");
            }

            // Restore previously saved layout
            _rootElement.LayoutTransform = transform;

#endif

            if (_saveIconImage != null && ToolBarEnabled)
                _saveIconImage.Visibility = Visibility.Visible;
        }

        protected virtual void LoadWm()
        {         
            CreateWmElement(new String((from ch in wmRegVal select Convert.ToChar(ch)).ToArray()), new String((from ch in wmLinkVal select Convert.ToChar(ch)).ToArray()));
        }

        protected void CreateWmElement(String text, String href)
        {   
            if (_wMElement == null)
            {   
                _wMElement = new TextBlock();
                _wMElement.HorizontalAlignment = HorizontalAlignment.Right;
                _wMElement.VerticalAlignment = VerticalAlignment.Center;
                _wMElement.Margin = new Thickness(0, 0, 0, 0);
                _wMElement.SetValue(Canvas.ZIndexProperty, 90000);
                _wMElement.Text = text;

                if(!String.IsNullOrEmpty(href))
                    _wMElement.TextDecorations = TextDecorations.Underline;
                
                _wMElement.Foreground = new SolidColorBrush(Colors.Gray);

                _wMElement.FontSize = 10;
                AttachHref(this, _wMElement, href, HrefTargets._blank);
                _toolbarContainer.Children.Add(_wMElement);
            }
        }

        #endregion

        #region Internal Properties

        /// <summary>
        /// Whether the chart is running under an XBAP application
        /// </summary>
        internal static Boolean IsXBAPApp
        {
            get
            {
#if WPF
                return System.Windows.Interop.BrowserInteropHelper.IsBrowserHosted;
#else

                return false;
#endif
            }
        }

        /// <summary>
        /// Whether the chart is running under an XBAP application
        /// </summary>
        internal static Boolean IsMediaEffectsEnabled
        {
            get
            {
#if WPF
                return !IsXBAPApp;
#elif WP
                return false;
#else

                return true;
#endif
            }
        }

        #endregion

        #region Private Delegates

        #endregion

        #region Private Methods

        /// <summary>
        /// SaveIconEnabledProperty changed call back function
        /// </summary>
        /// <param name="d">Chart</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnSaveIconEnabledPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            VisifireControl c = d as VisifireControl;
            if (c._saveIconImage != null)
                c._saveIconImage.Visibility = ((Boolean)e.NewValue) ? Visibility.Visible : Visibility.Collapsed;
        }

        #endregion

        #region Internal Methods

        #endregion

        #region Internal Events And Delegates

        #endregion

        #region Data

        /// <summary>
        /// Whether the template is applied or not
        /// </summary>
        internal Boolean _isTemplateApplied;

        internal TextBlock _wMElement;

        #region Template Part

        internal Border _zoomRectangle;

        internal Canvas _elementCanvas;

        internal const string ZoomRectangleName = "ZoomRectangle";

        internal const string ElementCanvasName = "ElementCanvas";

        internal Grid _rootElement;
        internal const string RootElementName = "RootElement";

        internal Grid _shadowGrid;
        internal const string ShadowGridName = "ShadowGrid";

        internal Border _chartBorder;
        internal const string ChartBorderName = "ChartBorder";

        internal Rectangle _chartLightingBorder;
        internal const string ChartLightingBorderName = "ChartLightingBorder";

        internal Canvas _bevelCanvas;
        internal const string BevelCanvasName = "BevelCanvas";

        internal Grid _chartAreaGrid;
        internal const string ChartAreaGridName = "ChartAreaGrid";

        internal StackPanel _topOuterPanel;
        internal const string TopOuterPanelName = "TopOuterPanel";

        internal StackPanel _topOuterTitlePanel;
        internal const string TopOuterTitlePanelName = "TopOuterTitlePanel";

        internal StackPanel _topOuterLegendPanel;
        internal const string TopOuterLegendPanelName = "TopOuterLegendPanel";

        internal StackPanel _bottomOuterPanel;
        internal const string BottomOuterPanelName = "BottomOuterPanel";

        internal StackPanel _bottomOuterLegendPanel;
        internal const string BottomOuterLegendPanelName = "BottomOuterLegendPanel";

        internal StackPanel _bottomOuterTitlePanel;
        internal const string BottomOuterTitlePanelName = "BottomOuterTitlePanel";

        internal StackPanel _leftOuterPanel;
        internal const string LeftOuterPanelName = "LeftOuterPanel";

        internal StackPanel _leftOuterTitlePanel;
        internal const string LeftOuterTitlePanelName = "LeftOuterTitlePanel";

        internal StackPanel _leftOuterLegendPanel;
        internal const string LeftOuterLegendPanelName = "LeftOuterLegendPanel";

        internal StackPanel _rightOuterPanel;
        internal const string RightOuterPanelName = "RightOuterPanel";

        internal StackPanel _rightOuterLegendPanel;
        internal const string RightOuterLegendPanelName = "RightOuterLegendPanel";

        internal StackPanel _rightOuterTitlePanel;
        internal const string RightOuterTitlePanelName = "RightOuterTitlePanel";

        internal Grid _centerOuterGrid;
        internal const string CenterOuterGridName = "CenterOuterGrid";

        internal Grid _centerGrid;
        internal const string CenterGridName = "CenterGrid";

        internal Grid _topOffsetGrid;
        internal const string TopOffsetGridName = "TopOffsetGrid";

        internal Grid _bottomOffsetGrid;
        internal const string BottomOffsetGridName = "BottomOffsetGrid";

        internal Grid _leftOffsetGrid;
        internal const string LeftOffsetGridName = "LeftOffsetGrid";

        internal Grid _rightOffsetGrid;
        internal const string RightOffsetGridName = "RightOffsetGrid";

        internal Grid _topAxisGrid;
        internal const string TopAxisGridName = "TopAxisGrid";

        internal StackPanel _topAxisContainer;
        internal const string TopAxisContainerName = "TopAxisContainer";

        internal StackPanel _topAxisPanel;
        internal const string TopAxisPanelName = "TopAxisPanel";

        internal ZoomBar _topAxisScrollBar;
        internal const string TopAxisScrollBarName = "TopAxisScrollBar";

        internal Grid _leftAxisGrid;
        internal const string LeftAxisGridName = "LeftAxisGrid";

        internal StackPanel _leftAxisContainer;
        internal const string LeftAxisContainerName = "LeftAxisContainer";

        internal StackPanel _leftAxisPanel;
        internal const string LeftAxisPanelName = "LeftAxisPanel";

        internal ZoomBar _leftAxisScrollBar;
        internal const string LeftAxisScrollBarName = "LeftAxisScrollBar";

        internal Grid _rightAxisGrid;
        internal const string RightAxisGridName = "RightAxisGrid";

        internal StackPanel _rightAxisContainer;
        internal const string RightAxisContainerName = "RightAxisContainer";

        internal ZoomBar _rightAxisScrollBar;
        internal const string RightAxisScrollBarName = "RightAxisScrollBar";

        internal StackPanel _rightAxisPanel;
        internal const string RightAxisPanelName = "RightAxisPanel";

        internal Grid _bottomAxisGrid;
        internal const string BottomAxisGridName = "BottomAxisGrid";

        internal StackPanel _bottomAxisContainer;
        internal const string BottomAxisContainerName = "BottomAxisContainer";

        internal ZoomBar _bottomAxisScrollBar;
        internal const string BottomAxisScrollBarName = "BottomAxisScrollBar";

        internal StackPanel _bottomAxisPanel;
        internal const string BottomAxisPanelName = "BottomAxisPanel";

        internal Grid _centerInnerGrid;
        internal const string CenterInnerGridName = "CenterInnerGrid";

        internal Grid _innerGrid;
        internal const string InnerGridName = "InnerGrid";

        internal StackPanel _topInnerPanel;
        internal const string TopInnerPanelName = "TopInnerPanel";

        internal StackPanel _topInnerTitlePanel;
        internal const string TopInnerTitlePanelName = "TopInnerTitlePanel";

        internal StackPanel _topInnerLegendPanel;
        internal const string TopInnerLegendPanelName = "TopInnerLegendPanel";

        internal StackPanel _bottomInnerPanel;
        internal const string BottomInnerPanelName = "BottomInnerPanel";

        internal StackPanel _bottomInnerLegendPanel;
        internal const string BottomInnerLegendPanelName = "BottomInnerLegendPanel";

        internal StackPanel _bottomInnerTitlePanel;
        internal const string BottomInnerTitlePanelName = "BottomInnerTitlePanel";

        internal StackPanel _leftInnerPanel;
        internal const string LeftInnerPanelName = "LeftInnerPanel";

        internal StackPanel _leftInnerTitlePanel;
        internal const string LeftInnerTitlePanelName = "LeftInnerTitlePanel";

        internal StackPanel _leftInnerLegendPanel;
        internal const string LeftInnerLegendPanelName = "LeftInnerLegendPanel";

        internal StackPanel _rightInnerPanel;
        internal const string RightInnerPanelName = "RightInnerPanel";

        internal StackPanel _rightInnerLegendPanel;
        internal const string RightInnerLegendPanelName = "RightInnerLegendPanel";

        internal StackPanel _rightInnerTitlePanel;
        internal const string RightInnerTitlePanelName = "RightInnerTitlePanel";

        internal Grid _plotAreaGrid;
        internal const string PlotAreaGridName = "PlotAreaGrid";

        internal ScrollViewer _plotAreaScrollViewer;
        internal const string PlotAreaScrollViewerName = "PlotAreaScrollViewer";

        internal Canvas _plotCanvas;
        internal const string PlotGridName = "PlotCanvas";

        internal Canvas _plotAreaShadowCanvas;
        internal const string PlotAreaShadowCanvasName = "PlotAreaShadowCanvas";

        internal Canvas _drawingCanvas;
        internal const string DrawingCanvasName = "DrawingCanvas";

        internal StackPanel _centerDockInsidePlotAreaPanel;
        internal const string CenterDockInsidePlotAreaPanelName = "CenterDockInsidePlotAreaPanel";

        internal StackPanel _centerDockOutsidePlotAreaPanel;
        internal const string CenterDockOutsidePlotAreaPanelName = "CenterDockOutsidePlotAreaPanel";

        internal Canvas _toolTipCanvas;
        internal const string ToolTipCanvasName = "ToolTipCanvas";

        internal Visifire.Charts.ToolTip _toolTip;

        private StackPanel _toolbarContainer;

        private System.Windows.Controls.Image _saveIconImage;

        internal System.Windows.Controls.TextBlock _zoomOutTextBlock;

        internal System.Windows.Controls.TextBlock _showAllTextBlock;

        internal TextBlock _zoomIconSeparater;

        internal StackPanel _zoomIconContainer;

        private static Byte[] wmRegVal = new Byte[] { 0x56, 0x69, 0x73, 0x69, 0x66, 0x69, 0x72, 0x65, 0x20, 0x54, 0x72, 0x69, 0x61, 0x6C, 0x20, 0x45, 0x64, 0x69, 0x74, 0x69, 0x6F, 0x6E };
        private static Byte[] wmLinkVal = new Byte[] { 0x68, 0x74, 0x74, 0x70, 0x3A, 0x2F, 0x2F, 0x77, 0x77, 0x77, 0x2E, 0x56, 0x69, 0x73, 0x69, 0x66, 0x69, 0x72, 0x65, 0x2E, 0x63, 0x6F, 0x6D, 0x2F, 0x6C, 0x69, 0x63, 0x65, 0x6E, 0x73, 0x65, 0x2E, 0x70, 0x68, 0x70 };

        #endregion

        #endregion

    }
}