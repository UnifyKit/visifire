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
using System.Globalization;

namespace VisifireCharts
{
    public partial class App : Application
    {

        public App()
        {
            this.Startup += this.Application_Startup;
            this.Exit += this.Application_Exit;

            
            this.UnhandledException += this.Application_UnhandledException;

            String fullName = System.Reflection.Assembly.GetExecutingAssembly().FullName;

            version = fullName.Split(',')[1];

            version = (version.Substring(0, version.LastIndexOf('.')) + " Beta").Trim();

            InitializeComponent();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {

            // Load the main control
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


            if (e.InitParams.ContainsKey("dataUri"))
            {
                dataUri = e.InitParams["dataUri"];

                if (!String.IsNullOrEmpty(dataUri))
                {
                    webclient = new System.Net.WebClient();
                    webclient.BaseAddress = new Uri(baseUri);

                    webclient.DownloadStringCompleted += new System.Net.DownloadStringCompletedEventHandler(webclient_DownloadStringCompleted);

                    webclient.DownloadStringAsync(new Uri(dataUri,UriKind.RelativeOrAbsolute));
                }
            }
            else if (e.InitParams.ContainsKey("dataXml"))
            {
                dataXml = (String)System.Windows.Browser.HtmlPage.Window.Invoke(e.InitParams["dataXml"]);
                CreateChart();
            }
            wrapper.LayoutRoot.Children.Add(tb);
        }

        void wrapper_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.F8)
                System.Windows.Browser.HtmlPage.Window.Alert("Visifire " + version);
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

        void webclient_DownloadStringCompleted(object sender, System.Net.DownloadStringCompletedEventArgs e)
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


            wrapper.LayoutRoot.Children.Add(chartCanv);
        }

        private void Application_Exit(object sender, EventArgs e)
        {

        }
        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (logLevel == 1)
            {
                logTextBox.Text += e.ExceptionObject.Message + "\n" + e.ExceptionObject.StackTrace + "\n";
                logTextBox.BorderThickness = new Thickness(1);
                logTextBox.Visibility = Visibility.Visible;

                foreach (FrameworkElement child in wrapper.LayoutRoot.Children)
                {
                    if(child != logTextBox)
                        child.Visibility = Visibility.Collapsed;
                }

            }
            e.Handled = true;
        }

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
        private Wrapper wrapper = new Wrapper();

    }
}
