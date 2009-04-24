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
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Diagnostics;
using Visifire.Charts;
using System.Globalization;
using System.Windows.Browser;
using System.Windows.Markup;

namespace SLVisifireChartsXap
{
    /// <summary>
    /// SLVisifireChartsXap application class
    /// </summary>
    public partial class App : Application
    {

        #region Public Methods

        /// <summary>
        /// Initializes a new instance of the SLVisifireChartsXap.Axis class
        /// </summary>
        public App()
        {
            this.Startup += this.Application_Startup;

            this.UnhandledException += this.Application_UnhandledException;

            InitializeComponent();

            _wrapper.DataXMLAdded += new EventHandler<DataXMLEventArgs>(Wrapper_DataXMLAdded);

            _wrapper.ReRender += new EventHandler(Wrapper_ReRender);

            _wrapper.OnResize += new EventHandler<ResizeEventArgs>(Wrapper_OnResize);

            HtmlPage.RegisterScriptableObject("wrapper", _wrapper);

            HtmlPage.RegisterScriptableObject("App", this);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// List of Charts currently rendered
        /// </summary>
        [ScriptableMember]
        public List<Chart> Charts
        {
            get;
            set;
        }

        #endregion

        #region Public Events And Delegates

        #endregion

        #region Protected Methods

        #endregion

        #region Internal Properties

        #endregion

        #region Private Properties

        /// <summary>
        /// Dialog box to show message or information
        /// </summary>
        private Dialog Dialog
        {
            get;
            set;
        }

        /// <summary>
        /// Logger logs information.
        /// </summary>
        private Logger Logger
        {
            get;
            set;
        }

        #endregion

        #region Private Delegates

        #endregion

        #region Private Methods

        /// <summary>
        /// On wrapper resized
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">ResizeEventArgs</param>
        private void Wrapper_OnResize(object sender, ResizeEventArgs e)
        {
            _chartWidth = e.Width;
            _chartHeight = e.Height;
        }

        /// <summary>
        /// On Data Xml Added
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">DataXMLEventArgs</param>
        private void Wrapper_DataXMLAdded(object sender, DataXMLEventArgs e)
        {
            if (e.DataXML != null)
                _xmlQueue.Enqueue(e.DataXML);

            if (e.DataUri != null)
            {
                _uriQueue.Enqueue(e.DataUri);
                if (_webclient != null && !_webclient.IsBusy)
                    DownloadXML();
            }
        }

        /// <summary>
        /// Force wrapper to ReRender the Chart
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">EventArgs</param>
        private void Wrapper_ReRender(object sender, EventArgs e)
        {
            RenderEngine();
        }

        /// <summary>
        /// Handle display of single and multiple charts
        /// </summary>
        private void RenderEngine()
        {
            Canvas tempChartCanvas;

            if (_firstChart)
            {
                _chartCanv = CreateChart();

                _wrapper.LayoutRoot.Children.Clear();
                _wrapper.LayoutRoot.Children.Add(_chartCanv);

                _firstChart = false;
            }
            else if (_xmlQueue.Count > 0 && _chartReady)
            {   
                _chartCanv = CreateChart();

                _wrapper.LayoutRoot.Children.Clear();
                _wrapper.LayoutRoot.Children.Add(_chartCanv);

                tempChartCanvas = (Canvas)XamlReader.Load(String.Format(@"<Canvas xmlns=""http://schemas.microsoft.com/client/2007"" xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"" />"));

                if (_wrapper.LayoutRoot.Children.Count > 2)
                {
                    for (Int32 i = 0; i < (_wrapper.LayoutRoot.Children.Count - 2); i++)
                    {
                        Canvas chartCanvas = _wrapper.LayoutRoot.Children[i] as Canvas;
                        try
                        {
                            if (chartCanvas != null && chartCanvas.Tag.ToString() == "Chart")
                            {
                                _wrapper.LayoutRoot.Children.RemoveAt(i);
                                chartCanvas.Loaded -= chartCanv_Loaded;
                                tempChartCanvas.Children.Add(chartCanvas);
                            }
                        }
                        catch (Exception e)
                        {
                            System.Diagnostics.Debug.WriteLine(e.Message);
                        }
                    }
                }

                tempChartCanvas.Children.Clear();
                tempChartCanvas = null;
            }

            AddDialog(_wrapper);
        }

        /// <summary>
        /// Download chart data xml 
        /// </summary>
        private void DownloadXML()
        {
            if (_uriQueue.Count > 0)
            {
                if (_webclient == null)
                {
                    _webclient = new WebClient();
                    _webclient.DownloadStringCompleted += new System.Net.DownloadStringCompletedEventHandler(webclient_DownloadStringCompleted);
                }

                _webclient.BaseAddress = _baseUri;

                _webclient.DownloadStringAsync(new Uri(_uriQueue.Dequeue(), UriKind.RelativeOrAbsolute));

            }
        }

        /// <summary>
        /// Event handler for the startup event attached with Silverlight application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            this.RootVisual = _wrapper;

            AddDialog(_wrapper);

            _wrapper.KeyDown += new KeyEventHandler(wrapper_KeyDown);

            _baseUri = System.Windows.Browser.HtmlPage.Document.DocumentUri.ToString();
            _baseUri = _baseUri.Substring(0, _baseUri.LastIndexOf("/") + 1);

            if (e.InitParams.ContainsKey("controlId"))
            {
                _controlId = e.InitParams["controlId"].ToString(CultureInfo.InvariantCulture);
            }

            if (e.InitParams.ContainsKey("setVisifireChartsRef"))
            {
                _setVisifireChartsRefFunctionName = e.InitParams["setVisifireChartsRef"].ToString(CultureInfo.InvariantCulture);
            }

            if (e.InitParams.ContainsKey("onChartPreLoad"))
            {
                _onChartPreLoadedFunctionName = e.InitParams["onChartPreLoad"].ToString(CultureInfo.InvariantCulture);
            }

            if (e.InitParams.ContainsKey("onChartLoaded"))
            {
                _onChartLoadedFunctionName = e.InitParams["onChartLoaded"].ToString(CultureInfo.InvariantCulture);
            }

            if (e.InitParams.ContainsKey("logLevel"))
            {
                _logLevel = Int32.Parse(e.InitParams["logLevel"].Trim());

                AddLogViewer(_wrapper);
            }
            else
            {
                _logLevel = 1;
                AddLogViewer(_wrapper);
            }

            if (e.InitParams.ContainsKey("width"))
            {
                if (!e.InitParams["width"].Contains("%"))
                    _chartWidth = Double.Parse(e.InitParams["width"], CultureInfo.InvariantCulture);
            }

            if (e.InitParams.ContainsKey("height"))
            {
                if (!e.InitParams["height"].Contains("%"))
                    _chartHeight = Double.Parse(e.InitParams["height"], CultureInfo.InvariantCulture);
            }

            if (e.InitParams.ContainsKey("dataUri"))
            {
                _dataUri = e.InitParams["dataUri"];

                if (!String.IsNullOrEmpty(_dataUri))
                {
                    _uriQueue.Enqueue(_dataUri);

                    DownloadXML();
                }
            }
            else if (e.InitParams.ContainsKey("dataXml"))
            {
                Enqueue((String)System.Windows.Browser.HtmlPage.Window.Invoke(e.InitParams["dataXml"]));

                RenderEngine();
            }
        }

        /// <summary>
        /// Event handler for the Key pressed event attached with Silverlight application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wrapper_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F8)
            {
                if (Dialog.Visibility == Visibility.Collapsed)
                {
                    Dialog.DialogOutStoryBoard.Stop();
                    Dialog.Visibility = Visibility.Visible;
                    Dialog.DialogInStoryBoard.Begin();
                }
                else
                {
                    Dialog.DialogInStoryBoard.Stop();
                    Dialog.DialogOutStoryBoard.Begin();
                }
            }
            else if (e.Key == Key.Escape)
            {
                Dialog.DialogOutStoryBoard.Begin();
            }
        }

        /// <summary>
        /// Handler for download complete of xml files from web server
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void webclient_DownloadStringCompleted(object sender, System.Net.DownloadStringCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                Enqueue(e.Result);

                if (_firstChart)
                {
                    RenderEngine();
                }

                // Download next XML
                DownloadXML();
            }
            else
            {
                throw e.Error;
            }
        }

        /// <summary>
        /// Returns the assembly version information
        /// </summary>
        /// <returns></returns>
        private String GetAssemblyVersion()
        {
            String fullName = System.Reflection.Assembly.GetExecutingAssembly().FullName;

            String version = fullName.Split(',')[1];

            version = (version.Substring(0, version.LastIndexOf('.'))).Trim() + " beta8";

            return version;
        }

        /// <summary>
        /// Add logger to chart application
        /// </summary>
        /// <param name="wrapper"></param>
        private void AddLogViewer(Wrapper wrapper)
        {
            // Add Logger
            Logger = new Logger();
            Logger.Log("Error Log " + Dialog.Message + ":\n--------------------------------");
            Logger.LogLine("Copy & Paste the contents of this log in www.visifire.com/forums for support.\n");
            Logger.Visibility = Visibility.Collapsed;

            wrapper.LayoutRoot.Children.Add(Logger);
        }

        /// <summary>
        /// Add Dialog box to chart application
        /// </summary>
        /// <param name="wrapper"></param>
        private void AddDialog(Wrapper wrapper)
        {
            // Add Dialog
            Dialog = new Dialog();
            Dialog.Visibility = Visibility.Collapsed;
            Dialog.Message = GetAssemblyVersion();
            wrapper.LayoutRoot.Children.Add(Dialog);
        }

        /// <summary>
        /// Create chart canvas
        /// </summary>
        /// <returns></returns>
        private Canvas CreateChart()
        {
            Canvas chartCanvas;

            String canvasXaml = "<Canvas VerticalAlignment=\"Top\" HorizontalAlignment=\"Left\" ";

            canvasXaml += "xmlns=\"http://schemas.microsoft.com/client/2007\" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\" xmlns:vc=\"clr-namespace:Visifire.Charts;assembly=SLVisifire.Charts\" ";

            if (!Double.IsNaN(_chartWidth))
                canvasXaml += " Width=\"" + _chartWidth.ToString(CultureInfo.InvariantCulture) + "\"";

            if (!Double.IsNaN(_chartHeight))
                canvasXaml += " Height=\"" + _chartHeight.ToString(CultureInfo.InvariantCulture) + "\"";

            canvasXaml += " Tag=\"Chart\" >\n";

            _dataXml = Dequeue();

            canvasXaml += _dataXml;

            canvasXaml += "</Canvas>";

            chartCanvas = (Canvas)XamlReader.Load(canvasXaml);

            if (_charts == null)
                _charts = new List<UIElement>();

            if (Charts == null)
                Charts = new List<Chart>();

            _charts.Clear();
            Charts.Clear();

            FindByType(ref _charts, chartCanvas as Panel, typeof(Chart));

            foreach (Chart chart in _charts)
            {
                Charts.Add(chart);
                chart.LogLevel = _logLevel;
                chart.ControlId = _controlId;
            }

            if (!String.IsNullOrEmpty(_setVisifireChartsRefFunctionName))
            {
                try
                {
                    System.Windows.Browser.HtmlPage.Window.Invoke(_setVisifireChartsRefFunctionName, Charts);
                }
                catch (InvalidOperationException e)
                {
                    throw new Exception("Error occurred while setting charts reference.", e);
                }
            }

            if (!String.IsNullOrEmpty(_onChartPreLoadedFunctionName))
            {
                try
                {
                    System.Windows.Browser.HtmlPage.Window.Invoke(_onChartPreLoadedFunctionName, Charts);
                }
                catch (InvalidOperationException e)
                {
                    throw new Exception("Error occurred while firing Chart preLoad event. JavaScript function attached with “preLoad” event contains errors.", e);
                }
            }

            chartCanvas.Loaded += new RoutedEventHandler(chartCanv_Loaded);

            _chartReady = false;

            return chartCanvas;
        }

        /// <summary>
        /// Finds list of objects of specified type.
        /// </summary>
        private static void FindByType(ref List<UIElement> tResult, Panel parent, Type objType)
        {
            if (parent != null)
            {
                var objs = from child in parent.Children where objType.Equals(child.GetType()) select child;

                foreach (UIElement uiElement in objs)
                {
                    tResult.Add(uiElement);
                    FindByType(ref tResult, uiElement as Panel, objType);
                }

                var panels = from child in parent.Children where (!objType.Equals(child.GetType()) && ((child as Panel) != null)) select child;

                foreach (Panel f in panels)
                    FindByType(ref tResult, f, objType);
            }
        }

        /// <summary>
        /// Event handler attached with Loaded event of chart canvas
        /// </summary>
        /// <param name="sender">Object</param>
        /// <param name="e">RoutedEventArgs</param>
        private void chartCanv_Loaded(object sender, RoutedEventArgs e)
        {
            _chartReady = true;

            if (!String.IsNullOrEmpty(_onChartLoadedFunctionName))
            {
                try
                {
                    System.Windows.Browser.HtmlPage.Window.Invoke(_onChartLoadedFunctionName, Charts);
                }
                catch (InvalidOperationException e1)
                {
                    throw new Exception("Error occurred while firing Chart Loaded event. JavaScript function attached with “Loaded” event contains errors.", e1);
                }
            }

            try
            {
                if (!String.IsNullOrEmpty(_onChartPreLoadedFunctionName))
                    System.Windows.Browser.HtmlPage.Window.Eval(String.Format(@"if( {0} != null) {0}=null;", _onChartPreLoadedFunctionName));

                if (!String.IsNullOrEmpty(_onChartLoadedFunctionName))
                    System.Windows.Browser.HtmlPage.Window.Eval(String.Format(@"if( {0} != null) {0}=null;", _onChartLoadedFunctionName));

                if (!String.IsNullOrEmpty(_setVisifireChartsRefFunctionName))
                    System.Windows.Browser.HtmlPage.Window.Eval(String.Format(@"if( {0} != null) {0}=null;", _setVisifireChartsRefFunctionName));
            }
            catch
            {}
        }

        /// <summary>
        /// Enqueue xml to xml queue
        /// </summary>
        /// <param name="data"></param>
        private void Enqueue(String data)
        {
            _xmlQueue.Enqueue(data);
            _wrapper.IsDataLoaded = true;
        }

        /// <summary>
        /// Dequeue xml from xml queue
        /// </summary>
        /// <returns></returns>
        private String Dequeue()
        {
            String data = _xmlQueue.Dequeue();
            if (_xmlQueue.Count == 0)
                _wrapper.IsDataLoaded = false;
            return data;
        }

        /// <summary>
        /// Handler for application unhandled exception
        /// </summary>
        /// <param name="sender">Object</param>
        /// <param name="e">ApplicationUnhandledExceptionEventArgs</param>
        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (_logLevel == 1)
            {
                if (Logger == null)
                    AddLogViewer(_wrapper);

                if (e.ExceptionObject != null && e.ExceptionObject.InnerException != null)
                    Logger.LogLine("InnerException: " + e.ExceptionObject.InnerException.Message + "\n");

                Logger.LogLine("Exception: " + e.ExceptionObject.Message + "\n");
                Logger.LogLine("XML: \n" + _dataXml + "\n"); ;
                Logger.LogLine("StackTrace: \n" + e.ExceptionObject.StackTrace + "\n");
                Logger.Visibility = Visibility.Visible;

                Logger.SetValue(Canvas.ZIndexProperty, 10000);

                foreach (FrameworkElement child in _wrapper.LayoutRoot.Children)
                {
                    if (child != Logger)
                        child.Visibility = Visibility.Collapsed;
                }
            }

            e.Handled = true;
        }

        #endregion

        #region Internal Methods

        #endregion

        #region Internal Events And Delegates

        #endregion

        #region Data

        /// <summary>
        /// Lavel of logging 
        /// </summary>
        private Int32 _logLevel = 0; 

        /// <summary>
        /// Data xml 
        /// </summary>
        private String _dataUri = null;  

        /// <summary>
        /// Data xml file uri
        /// </summary>
        private String _dataXml = null; 

        /// <summary>
        /// Base address of uri
        /// </summary>
        private String _baseUri = null;

        /// <summary>
        /// Chart width
        /// </summary>
        private Double _chartWidth = Double.NaN;

        /// <summary>
        /// Chart height
        /// </summary>
        private Double _chartHeight = Double.NaN;

        /// <summary>
        /// Chart canvas is used to draw new charts
        /// </summary>
        private Canvas _chartCanv; 
        
        /// <summary>
        ///  Wrapper for chart as user control
        /// </summary>
        private Wrapper _wrapper = new Wrapper();
        
        /// <summary>
        /// Queue for storing chart data xml
        /// </summary>
        private Queue<String> _xmlQueue = new Queue<string>();
        
        /// <summary>
        /// Queue for storing xml file uri
        /// </summary>
        private Queue<String> _uriQueue = new Queue<string>();

        /// <summary>
        /// If chart canvas is already rendered
        /// </summary>
        private Boolean _chartReady = false;
        
        /// <summary>
        /// Is it the first chart need to draw
        /// </summary>
        private Boolean _firstChart = true;

        /// <summary>
        /// Function to be invoked to fire loaded event for chart
        /// </summary>
        private String _onChartLoadedFunctionName;

        /// <summary>
        /// Function to be invoked to fire pre loaded event for chart
        /// </summary>
        private String _onChartPreLoadedFunctionName;
        
        /// <summary>
        /// After loading the chart this function should be always fired to set the array of charts in Visifire2 class
        /// </summary>
        private String _setVisifireChartsRefFunctionName;

        /// <summary>
        /// Silverlight Object Id
        /// </summary>
        private String _controlId;

        /// <summary>
        /// Webclient is used for downloading chart data xml
        /// </summary>
        WebClient _webclient;

        /// <summary>
        /// List of Charts
        /// </summary>
        private List<UIElement> _charts;

        #endregion
    }
}
