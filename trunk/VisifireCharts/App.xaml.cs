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
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Visifire.Charts;
using System.Windows.Markup;
using System.Windows.Browser;
using System.Globalization;

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

                version = fullName.Split(',')[1];

                version = (version.Substring(0, version.LastIndexOf('.')) + " Beta").Trim();

                InitializeComponent();

            }
            
        #endregion

        #region Private Methods

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            this.RootVisual = wrapper;
            
            System.Net.WebClient webclient;

            wrapper.KeyDown += new KeyEventHandler(wrapper_KeyDown);

            baseUri = System.Windows.Browser.HtmlPage.Document.DocumentUri.ToString();
            baseUri = baseUri.Substring(0, baseUri.LastIndexOf("/") + 1);

            if (e.InitParams.ContainsKey("logLevel"))
            {
                logLevel = Int32.Parse(e.InitParams["logLevel"].Trim());

                AddLogViewer(wrapper);
            }

            if (e.InitParams.ContainsKey("width"))
            {   
                if(!e.InitParams["width"].Contains("%"))
                    chartWidth = Double.Parse(e.InitParams["width"]);
            }

            if (e.InitParams.ContainsKey("height"))
            {
                if (!e.InitParams["height"].Contains("%"))
                    chartHeight = Double.Parse(e.InitParams["height"]);
            }

            if (e.InitParams.ContainsKey("EventDispatcher"))
            {
                jsEventDispatcher = e.InitParams["EventDispatcher"];
            }

            if (e.InitParams.ContainsKey("jsEvents"))
            {
                jsEvents = new Dictionary<string, System.Collections.Generic.List<String>>();

                String[] attachedJsEvents = e.InitParams["jsEvents"].Split(';');
                
                foreach (String st in attachedJsEvents)
                {
                    if (String.IsNullOrEmpty(st))
                        break;

                    String[] temp = st.Split(' ');

                    if (!jsEvents.ContainsKey(temp[0]))
                        jsEvents.Add(temp[0],new List<string>());

                    if(!jsEvents[temp[0]].Contains(temp[1]))
                        jsEvents[temp[0]].Add(temp[1]);
                }
            }
            
            if (e.InitParams.ContainsKey("dataUri"))
            {
                dataUri = e.InitParams["dataUri"];

                if (!String.IsNullOrEmpty(dataUri))
                {   
                    webclient = new System.Net.WebClient();
                    webclient.BaseAddress = new Uri(baseUri);

                    webclient.DownloadStringCompleted += new System.Net.DownloadStringCompletedEventHandler(webclient_DownloadStringCompleted);

                    webclient.DownloadStringAsync(new Uri(dataUri, UriKind.RelativeOrAbsolute));
                }
            }
            else if (e.InitParams.ContainsKey("dataXml"))
            {
                dataXml = (String)System.Windows.Browser.HtmlPage.Window.Invoke(e.InitParams["dataXml"]);
                CreateChart();
            }

            wrapper.LayoutRoot.Children.Add(tb);

           // TestAttachedJsEvents();
        }
        
        private void wrapper_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.F8)
                System.Windows.Browser.HtmlPage.Window.Alert("Visifire " + version);
        }

        private void webclient_DownloadStringCompleted(object sender, System.Net.DownloadStringCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                dataXml = e.Result;
                CreateChart();
            }
            else
            {
                throw e.Error;
            }
        }

        private void AddLogViewer(Wrapper wrapper)
        {
            logTextBox = new TextBox();
            logTextBox.FontSize = 12;
            logTextBox.TextAlignment = TextAlignment.Left;
            logTextBox.Visibility = Visibility.Collapsed;

            logTextBox.BorderThickness = new Thickness(0);

            logTextBox.MaxHeight = 300;
            logTextBox.MaxWidth = 500;

            logTextBox.Text = "Error Log (" + version + ") :\n";

            wrapper.LayoutRoot.Children.Add(logTextBox);
        }

        private void CreateChart()
        {
            String canvasXaml = "<Canvas ";
            canvasXaml += "xmlns=\"http://schemas.microsoft.com/client/2007\"";

            if (!Double.IsNaN(chartWidth))
                canvasXaml += " Width=\"" + chartWidth.ToString() + "\"";

            if (!Double.IsNaN(chartHeight))
                canvasXaml += " Height=\"" + chartHeight.ToString() + "\"";

            canvasXaml += ">\n";

            canvasXaml += dataXml;

            canvasXaml += "</Canvas>";

            chartCanv = (Canvas)XamlReader.Load(canvasXaml);

            chartCanv.Loaded += new RoutedEventHandler(chartCanv_Loaded);

            wrapper.LayoutRoot.Children.Add(chartCanv);

        }

        private void chartCanv_Loaded(object sender, RoutedEventArgs e)
        {
            AttachJsEvents(sender as Canvas);
        }

        #region "Js Events"
            
        private void AttachJsEvents(Canvas chartCanv)
        {
            if (jsEvents != null)
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
                if (jsEvents.ContainsKey("Chart"))
                {
                    if (jsEvents["Chart"].Contains("MouseEnter"))
                        chart.MouseEnter += delegate(object sender, MouseEventArgs e)
                        {
                            DispatchJsEvent(sender, e, "MouseEnter");
                        };

                    if (jsEvents["Chart"].Contains("MouseLeave"))
                        chart.MouseLeave += delegate(object sender, MouseEventArgs e)
                        {
                            DispatchJsEvent(sender, e, "MouseLeave");
                        };
                }

                if (jsEvents.ContainsKey("DataPoint"))
                {
                    foreach (DataSeries ds in chart.DataSeries)
                    {
                        foreach (DataPoint dp in ds.DataPoints)
                        {
                            if (jsEvents["DataPoint"].Contains("MouseEnter"))
                                dp.MouseEnter += delegate(object sender, MouseEventArgs e)
                                {
                                    DispatchJsEvent(sender, e, "MouseEnter");
                                };

                            if (jsEvents["DataPoint"].Contains("MouseLeave"))
                                dp.MouseLeave += delegate(object sender, MouseEventArgs e)
                                {
                                    DispatchJsEvent(sender, e, "MouseLeave");
                                };
                        }
                    }
                }

                if (jsEvents.ContainsKey("AxisX"))
                {
                    if (jsEvents["AxisX"].Contains("MouseEnter"))
                        chart.AxisX.MouseEnter += delegate(object sender, MouseEventArgs e)
                        {
                            DispatchJsEvent(sender, e, "MouseEnter");
                        };
                        
                    if (jsEvents["AxisX"].Contains("MouseLeave"))
                        chart.AxisX.MouseLeave += delegate(object sender, MouseEventArgs e)
                        {
                            DispatchJsEvent(sender, e, "MouseLeave");
                        };
                }

                if (jsEvents.ContainsKey("AxisY"))
                {
                    if (jsEvents["AxisY"].Contains("MouseEnter"))
                        chart.AxisY.MouseEnter += delegate(object sender, MouseEventArgs e)
                        {
                            DispatchJsEvent(sender, e, "MouseEnter");
                        };
                        
                    if (jsEvents["AxisY"].Contains("MouseLeave"))
                        chart.AxisY.MouseLeave += delegate(object sender, MouseEventArgs e)
                        {
                            DispatchJsEvent(sender, e, "MouseLeave");
                        };
                }

                if (jsEvents.ContainsKey("Title"))
                {   
                    foreach (Title title in chart.Titles)
                    {
                        if (jsEvents["Title"].Contains("MouseEnter"))
                            title.MouseEnter += delegate(object sender, MouseEventArgs e)
                            {
                                DispatchJsEvent(sender, e, "MouseEnter");
                            };

                        if (jsEvents["Title"].Contains("MouseLeave"))
                            title.MouseLeave += delegate(object sender, MouseEventArgs e)
                            {
                                DispatchJsEvent(sender, e, "MouseLeave");
                            };
                    }
                }

                if (jsEvents.ContainsKey("Legend"))
                {
                    foreach (Legend legend in chart.Legends)
                    {
                        if (jsEvents["Legend"].Contains("MouseEnter"))
                            legend.MouseEnter += delegate(object sender, MouseEventArgs e)
                            {
                                DispatchJsEvent(sender, e, "MouseEnter");
                            };

                        if (jsEvents["Legend"].Contains("MouseLeave"))
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
                
                if(fe != null)
                    DispatchJsEvent(fe, e, eventName);
            }
        }

        private void DispatchJsEvent(Object sender, EventArgs e, String eventName)
        {
            if (!String.IsNullOrEmpty(jsEventDispatcher))
            {   
                try
                {   
                    if (jsEvents.ContainsKey("DataPoint") && jsEvents["DataPoint"].Contains(eventName) && typeof(DataPoint).Equals(sender.GetType()))
                        System.Windows.Browser.HtmlPage.Window.Invoke(jsEventDispatcher, new DataPointJsEventArgs(sender as DataPoint, e, eventName));
                    else if (jsEvents.ContainsKey("Title") && jsEvents["Title"].Contains(eventName) && typeof(Title).Equals(sender.GetType()))
                        System.Windows.Browser.HtmlPage.Window.Invoke(jsEventDispatcher, new TitleJsEventArgs(sender as Title, e, eventName));
                    else if (jsEvents.ContainsKey("AxisX") && jsEvents["AxisX"].Contains(eventName) && typeof(AxisX).Equals(sender.GetType()))
                        System.Windows.Browser.HtmlPage.Window.Invoke(jsEventDispatcher, new AxisJsEventArgs(sender as AxisX, e, eventName));
                    else if (jsEvents.ContainsKey("AxisY") && jsEvents["AxisY"].Contains(eventName) && typeof(AxisY).Equals(sender.GetType()))
                        System.Windows.Browser.HtmlPage.Window.Invoke(jsEventDispatcher, new AxisJsEventArgs(sender as AxisY, e, eventName));
                    else if (jsEvents.ContainsKey("Legend") && jsEvents["Legend"].Contains(eventName) && typeof(Legend).Equals(sender.GetType()))
                        System.Windows.Browser.HtmlPage.Window.Invoke(jsEventDispatcher, new LegendJsEventArgs(sender as Legend, e, eventName));

                    if (jsEvents.ContainsKey("Chart") && jsEvents["Chart"].Contains(eventName))
                    {
                        if (typeof(Chart).Equals(sender.GetType()))
                            System.Windows.Browser.HtmlPage.Window.Invoke(jsEventDispatcher, new ChartJsEventArgs(sender as Chart, e, eventName));
                        else if (eventName != "MouseEnter" && eventName != "MouseLeave") // No bubbled up
                        {
                            Chart chart = (typeof(DataPoint).Equals(sender.GetType())) ? ((sender as DataPoint).Parent as DataSeries).Parent as Chart : (sender as FrameworkElement).Parent as Chart;

                            if (chart != null)
                                System.Windows.Browser.HtmlPage.Window.Invoke(jsEventDispatcher, new ChartJsEventArgs(chart, e, eventName));
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

            foreach (System.Collections.Generic.KeyValuePair<string, System.Collections.Generic.List<string>> de in jsEvents)
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
            if (logLevel == 1)
            {
                logTextBox.Text += e.ExceptionObject.Message + "\n" + e.ExceptionObject.StackTrace + "\n";
                logTextBox.BorderThickness = new Thickness(1);
                logTextBox.Visibility = Visibility.Visible;

                foreach (FrameworkElement child in wrapper.LayoutRoot.Children)
                {
                    if (child != logTextBox)
                        child.Visibility = Visibility.Collapsed;
                }
            }
            e.Handled = true;
        }
        #endregion

        #region Data

            private String version = null;
            private Int32 logLevel = 0;
            private String dataUri = null;
            private String dataXml = null;
            private String baseUri = null;
            private TextBox logTextBox;
            private Double chartWidth = Double.NaN;
            private Double chartHeight = Double.NaN;
            private Canvas chartCanv;
            private TextBlock tb = new TextBlock();
            private String jsEventDispatcher = null;
            private  System.Collections.Generic.Dictionary<String, System.Collections.Generic.List<String>> jsEvents;
            private Wrapper wrapper = new Wrapper();

        #endregion
    }
}