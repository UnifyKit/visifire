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
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Browser;
using System.Globalization;
using System.Net;

using Visifire.Charts;

namespace VisifireCharts
{
    public partial class App : Application
    {
        #region Public Methods

        public App()
        {
            this.Startup += this.Application_Startup;

            this.UnhandledException += this.Application_UnhandledException;

            String fullName = System.Reflection.Assembly.GetExecutingAssembly().FullName;

            _version = fullName.Split(',')[1];

            _version = (_version.Substring(0, _version.LastIndexOf('.')) + " Beta").Trim();

            InitializeComponent();

            _wrapper.DataXML += new EventHandler<DataXMLEventArgs>(_wrapper_DataXML);

            _wrapper.ReRender += new EventHandler(_wrapper_ReRender);

            _wrapper.OnResize += new EventHandler<ResizeEventArgs>(_wrapper_OnResize);

            HtmlPage.RegisterScriptableObject("wrapper", _wrapper);

        }

        void _wrapper_OnResize(object sender, ResizeEventArgs e)
        {
            _chartWidth = e.Width;
            _chartHeight = e.Height;
        }

        void _wrapper_DataXML(object sender, DataXMLEventArgs e)
        {
            if (e.DataXML != null)
                _xmlQueue.Enqueue(e.DataXML);

            if (e.DataUri != null)
            {
                _uriQueue.Enqueue(e.DataUri);
                if (_webclient != null && !_webclient.IsBusy )
                    DownloadXML();
            }

        }

        void _wrapper_ReRender(object sender, EventArgs e)
        {
            RenderEngine();
        }


        #endregion

        #region Private Methods
        /// <summary>
        /// Handle display of single and multiple charts
        /// </summary>
        private void RenderEngine()
        {
            Canvas tempChartCanvas;

            if (_firstChart)
            {
                _chartCanv = CreateChart();

                _wrapper.LayoutRoot.Children.Add(_chartCanv);

                _firstChart = false;
            }
            else if (_xmlQueue.Count > 0 && _chartReady)
            {
                _chartCanv = CreateChart();

                _wrapper.LayoutRoot.Children.Add(_chartCanv);

                tempChartCanvas = (Canvas) XamlReader.Load(String.Format(@"<Canvas xmlns=""http://schemas.microsoft.com/client/2007"" xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"" />"));

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
                GC.Collect();
            }
        }

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

                _downloadBusy = true;
            }
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            this.RootVisual = _wrapper;
            _wrapper.KeyDown += new KeyEventHandler(wrapper_KeyDown);

            _baseUri = System.Windows.Browser.HtmlPage.Document.DocumentUri.ToString();
            _baseUri = _baseUri.Substring(0, _baseUri.LastIndexOf("/") + 1);

            if (e.InitParams.ContainsKey("logLevel"))
            {   
                _logLevel = Int32.Parse(e.InitParams["logLevel"].Trim());

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

            if (e.InitParams.ContainsKey("EventDispatcher"))
            {
                _jsEventDispatcher = e.InitParams["EventDispatcher"];
            }

            if (e.InitParams.ContainsKey("jsEvents"))
            {   
                _jsEvents = new Dictionary<string, System.Collections.Generic.List<String>>();

                String[] attachedJsEvents = e.InitParams["jsEvents"].Split(';');

                foreach (String st in attachedJsEvents)
                {   
                    if (String.IsNullOrEmpty(st))
                        break;

                    String[] temp = st.Split(' ');

                    if (!_jsEvents.ContainsKey(temp[0]))
                        _jsEvents.Add(temp[0], new List<string>());

                    if (!_jsEvents[temp[0]].Contains(temp[1]))
                        _jsEvents[temp[0]].Add(temp[1]);
                }
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
                //CreateChart();
            }

            _wrapper.LayoutRoot.Children.Add(_textBlock);

            // TestAttachedJsEvents();
        }

        private void wrapper_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F8)
                System.Windows.Browser.HtmlPage.Window.Alert("Visifire " + _version);
        }

        private void webclient_DownloadStringCompleted(object sender, System.Net.DownloadStringCompletedEventArgs e)
        {   
            if (e.Error == null)
            {
                _downloadBusy = false;
                Enqueue(e.Result);

                if (_firstChart)
                {
                    RenderEngine();

                }

                //download Next XML
                DownloadXML();
            }
            else
            {
                throw e.Error;
            }
        }

        private void AddLogViewer(Wrapper wrapper)
        {
            _logTextBox = new TextBox();
            _logTextBox.FontSize = 12;
            _logTextBox.TextAlignment = TextAlignment.Left;
            _logTextBox.Visibility = Visibility.Collapsed;

            _logTextBox.Text = "Error Log (" + _version + ") :\n";

            _scrollViewer = new ScrollViewer();
            _scrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
            _scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;

            _scrollViewer.SetValue(ScrollViewer.ContentProperty,_logTextBox);

            wrapper.LayoutRoot.Children.Add(_scrollViewer);
        }

        private Canvas CreateChart()
        {
            Canvas chartCanvas;

            String canvasXaml = "<Canvas ";

            canvasXaml += "xmlns=\"http://schemas.microsoft.com/client/2007\" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\" ";

            if (!Double.IsNaN(_chartWidth))
                canvasXaml += " Width=\"" + _chartWidth.ToString(CultureInfo.InvariantCulture) + "\"";

            if (!Double.IsNaN(_chartHeight))
                canvasXaml += " Height=\"" + _chartHeight.ToString(CultureInfo.InvariantCulture) + "\"";

            canvasXaml += " Tag=\"Chart\" >\n";

            _dataXml = Dequeue();

            canvasXaml += _dataXml;

            canvasXaml += "</Canvas>";

            chartCanvas = (Canvas)XamlReader.Load(canvasXaml);

            chartCanvas.Loaded += new RoutedEventHandler(chartCanv_Loaded);

            _chartReady = false;

            return chartCanvas;
        }

        private void Enqueue(String data)
        {
            _xmlQueue.Enqueue(data);
            _wrapper.IsDataLoaded = true;
        }

        private String Dequeue()
        {
            String data = _xmlQueue.Dequeue();
            if (_xmlQueue.Count == 0)
                _wrapper.IsDataLoaded = false;
            return data;
        }

        private void chartCanv_Loaded(object sender, RoutedEventArgs e)
        {
            AttachJsEvents(sender as Canvas);
            _chartReady = true;
        }

        #region "Js Events"

        private void AttachJsEvents(Canvas chartCanv)
        {
            if (_jsEvents != null)
            {
                chartCanv.MouseLeftButtonDown += delegate(object sender, MouseButtonEventArgs e)
                {
                    CatchBubbledEvents(chartCanv, e, "MouseLeftButtonDown");
                };

                chartCanv.MouseLeftButtonUp += delegate(object sender, MouseButtonEventArgs e)
                {
                    CatchBubbledEvents(sender, e, "MouseLeftButtonUp");
                };

                chartCanv.MouseMove += delegate(object sender, MouseEventArgs e)
                {
                    CatchBubbledEvents(sender, e, "MouseMove");
                };

                List<UIElement> charts = new List<UIElement>();

                Visifire.Commons.VisualObject.FindByType(ref charts, chartCanv, typeof(Chart));

                foreach (Chart chart in charts)
                    AttachJsEvents(chart);
            }
        }

        private void AttachJsEvents(Chart chart)
        {
            if (chart != null)
            {
                if (_jsEvents.ContainsKey("Chart"))
                {
                    if (_jsEvents["Chart"].Contains("MouseEnter"))
                        chart.MouseEnter += delegate(object sender, MouseEventArgs e)
                        {
                            DispatchJsEvent(sender, e, "MouseEnter");
                        };

                    if (_jsEvents["Chart"].Contains("MouseLeave"))
                        chart.MouseLeave += delegate(object sender, MouseEventArgs e)
                        {
                            DispatchJsEvent(sender, e, "MouseLeave");
                        };
                }

                if (_jsEvents.ContainsKey("DataPoint"))
                {
                    foreach (DataSeries ds in chart.DataSeries)
                    {
                        foreach (DataPoint dp in ds.DataPoints)
                        {
                            if (_jsEvents["DataPoint"].Contains("MouseEnter"))
                                dp.MouseEnter += delegate(object sender, MouseEventArgs e)
                                {
                                    DispatchJsEvent(sender, e, "MouseEnter");
                                };

                            if (_jsEvents["DataPoint"].Contains("MouseLeave"))
                                dp.MouseLeave += delegate(object sender, MouseEventArgs e)
                                {
                                    DispatchJsEvent(sender, e, "MouseLeave");
                                };
                        }
                    }
                }

                if (_jsEvents.ContainsKey("AxisX"))
                {
                    if (_jsEvents["AxisX"].Contains("MouseEnter"))
                        chart.AxisX.MouseEnter += delegate(object sender, MouseEventArgs e)
                        {
                            DispatchJsEvent(sender, e, "MouseEnter");
                        };

                    if (_jsEvents["AxisX"].Contains("MouseLeave"))
                        chart.AxisX.MouseLeave += delegate(object sender, MouseEventArgs e)
                        {
                            DispatchJsEvent(sender, e, "MouseLeave");
                        };
                }

                if (_jsEvents.ContainsKey("AxisY"))
                {
                    if (_jsEvents["AxisY"].Contains("MouseEnter"))
                        chart.AxisYPrimary.MouseEnter += delegate(object sender, MouseEventArgs e)
                        {
                            DispatchJsEvent(sender, e, "MouseEnter");
                        };

                    if (_jsEvents["AxisY"].Contains("MouseLeave"))
                        chart.AxisYPrimary.MouseLeave += delegate(object sender, MouseEventArgs e)
                        {
                            DispatchJsEvent(sender, e, "MouseLeave");
                        };
                }

                if (_jsEvents.ContainsKey("Title"))
                {
                    foreach (Title title in chart.Titles)
                    {
                        if (_jsEvents["Title"].Contains("MouseEnter"))
                            title.MouseEnter += delegate(object sender, MouseEventArgs e)
                            {
                                DispatchJsEvent(sender, e, "MouseEnter");
                            };

                        if (_jsEvents["Title"].Contains("MouseLeave"))
                            title.MouseLeave += delegate(object sender, MouseEventArgs e)
                            {
                                DispatchJsEvent(sender, e, "MouseLeave");
                            };
                    }
                }

                if (_jsEvents.ContainsKey("Legend"))
                {
                    foreach (Legend legend in chart.Legends)
                    {
                        if (_jsEvents["Legend"].Contains("MouseEnter"))
                            legend.MouseEnter += delegate(object sender, MouseEventArgs e)
                            {
                                DispatchJsEvent(sender, e, "MouseEnter");
                            };

                        if (_jsEvents["Legend"].Contains("MouseLeave"))
                            legend.MouseLeave += delegate(object sender, MouseEventArgs e)
                            {
                                DispatchJsEvent(sender, e, "MouseLeave");
                            };
                    }
                }
            }
        }

        private FrameworkElement GetTaggedElement(FrameworkElement fe)
        {
            if (fe.Tag != null)
            {
                System.Diagnostics.Debug.WriteLine(fe.Tag.ToString());

                if ((!typeof(Chart).Equals(fe.GetType())) && fe.Tag != null)
                    fe = fe.FindName(fe.Tag.ToString()) as FrameworkElement;

                return fe;
            }
            else
                return null;
        }

        private void CatchBubbledEvents(Object sender, EventArgs e, String eventName)
        {
            if (e.GetType().Name == "MouseButtonEventArgs")
            {
                FrameworkElement fe = GetTaggedElement((e as MouseButtonEventArgs).Source as FrameworkElement);

                if (fe != null)
                    DispatchJsEvent(fe, e, eventName);
            }
            else if (e.GetType().Name == "MouseEventArgs")
            {
                FrameworkElement fe = GetTaggedElement((e as MouseEventArgs).Source as FrameworkElement);

                if (fe != null)
                    DispatchJsEvent(fe, e, eventName);
            }
        }

        private void DispatchJsEvent(Object sender, EventArgs e, String eventName)
        {
            if (!String.IsNullOrEmpty(_jsEventDispatcher))
            {
                try
                {
                    if (_jsEvents.ContainsKey("DataPoint") && _jsEvents["DataPoint"].Contains(eventName) && typeof(DataPoint).Equals(sender.GetType()))
                        System.Windows.Browser.HtmlPage.Window.Invoke(_jsEventDispatcher, new DataPointJsEventArgs(sender as DataPoint, e, eventName));
                    else if (_jsEvents.ContainsKey("Title") && _jsEvents["Title"].Contains(eventName) && typeof(Title).Equals(sender.GetType()))
                        System.Windows.Browser.HtmlPage.Window.Invoke(_jsEventDispatcher, new TitleJsEventArgs(sender as Title, e, eventName));
                    else if (_jsEvents.ContainsKey("AxisX") && _jsEvents["AxisX"].Contains(eventName) && typeof(AxisX).Equals(sender.GetType()))
                        System.Windows.Browser.HtmlPage.Window.Invoke(_jsEventDispatcher, new AxisJsEventArgs(sender as AxisX, e, eventName));
                    else if (_jsEvents.ContainsKey("AxisY") && _jsEvents["AxisY"].Contains(eventName) && typeof(AxisY).Equals(sender.GetType()))
                        System.Windows.Browser.HtmlPage.Window.Invoke(_jsEventDispatcher, new AxisJsEventArgs(sender as AxisY, e, eventName));
                    else if (_jsEvents.ContainsKey("Legend") && _jsEvents["Legend"].Contains(eventName) && typeof(Legend).Equals(sender.GetType()))
                        System.Windows.Browser.HtmlPage.Window.Invoke(_jsEventDispatcher, new LegendJsEventArgs(sender as Legend, e, eventName));

                    if (_jsEvents.ContainsKey("Chart") && _jsEvents["Chart"].Contains(eventName))
                    {
                        if (typeof(Chart).Equals(sender.GetType()))
                            System.Windows.Browser.HtmlPage.Window.Invoke(_jsEventDispatcher, new ChartJsEventArgs(sender as Chart, e, eventName));
                        else if (eventName != "MouseEnter" && eventName != "MouseLeave") // No bubbled up
                        {
                            Chart chart = (typeof(DataPoint).Equals(sender.GetType())) ? ((sender as DataPoint).Parent as DataSeries).Parent as Chart : (sender as FrameworkElement).Parent as Chart;

                            if (chart != null)
                                System.Windows.Browser.HtmlPage.Window.Invoke(_jsEventDispatcher, new ChartJsEventArgs(chart, e, eventName));
                        }
                    }
                }
                catch (Exception e1)
                {
                    throw new Exception("JavaScript Error:" + e1.Message);
                }
            }
        }

        private void TestAttachedJsEvents()
        {
            System.Diagnostics.Debug.WriteLine("The following events attached from the Javascript.");

            foreach (System.Collections.Generic.KeyValuePair<string, System.Collections.Generic.List<string>> de in _jsEvents)
            {
                System.Diagnostics.Debug.WriteLine(de.Key.ToString());

                foreach (String s in de.Value)
                {
                    System.Diagnostics.Debug.WriteLine("----" + s);
                }
            }

        }

        #endregion "Js Events"

        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {   
            if (_logLevel == 1)
            {   
                _logTextBox.Text += "Copy & Paste the contents of this log in www.visifire.com/forums for support.\n\n";
                _logTextBox.Text += "Message: " + e.ExceptionObject.Message + "\n\n";
                _logTextBox.Text += "XML: \n" + _dataXml + "\n";
                _logTextBox.Text += "StackTrace: \n" + e.ExceptionObject.StackTrace + "\n";

                TextBlock tb = new TextBlock();

                tb.FontSize = 12;
                tb.Text = _logTextBox.Text;
                tb.LineStackingStrategy = LineStackingStrategy.BlockLineHeight;
                tb.LineHeight = 14;

                _logTextBox.Visibility = Visibility.Visible;

                _logTextBox.Height = tb.ActualHeight;
                _logTextBox.Width = tb.ActualWidth;

                _scrollViewer.Height = Double.IsNaN(_chartHeight)? 300 :_chartHeight;
                _scrollViewer.Width = Double.IsNaN(_chartWidth) ? 300 : _chartWidth ;

                
                foreach (FrameworkElement child in _wrapper.LayoutRoot.Children)
                {
                    if (child != _scrollViewer)
                        child.Visibility = Visibility.Collapsed;
                }

                
            }
            e.Handled = true;
        }

        #endregion

        #region Data

        private String _version = null;
        private Int32 _logLevel = 0;
        private String _dataUri = null;
        private String _dataXml = null;
        private String _baseUri = null;

        private TextBox _logTextBox;
        private ScrollViewer _scrollViewer;

        private Double _chartWidth = Double.NaN;
        private Double _chartHeight = Double.NaN;
        private Canvas _chartCanv;
        private TextBlock _textBlock = new TextBlock();
        private String _jsEventDispatcher = null;
        private Dictionary<String, List<String>> _jsEvents;
        private Wrapper _wrapper = new Wrapper();

        private Queue<String> _xmlQueue = new Queue<string>();
        private Queue<String> _uriQueue = new Queue<string>();

        private Boolean _chartReady = false;
        private Boolean _firstChart = true;
        private Boolean _downloadBusy;

        WebClient _webclient;
        #endregion
    }
}