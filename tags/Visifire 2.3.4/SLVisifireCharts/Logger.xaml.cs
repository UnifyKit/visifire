using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Visifire.Charts
{
    /// <summary>
    /// Logger creates a multiline textbox with a vertical scrollbar, helps to log message
    /// </summary>
    public partial class Logger : UserControl
    {
        #region Public Methods

        /// <summary>
        /// Initializes a new instance of the Visifire.Charts.Logger class
        /// </summary>
        public Logger()
        {
            InitializeComponent();
            _lineNumber = 1;
            ScrollViewer.MouseWheel += new MouseWheelEventHandler(ScrollViewer_MouseWheel);
            ScrollViewer.KeyUp += new KeyEventHandler(ScrollViewer_KeyUp);
            this.Loaded += new RoutedEventHandler(Logger_Loaded);
        }

        void Logger_Loaded(object sender, RoutedEventArgs e)
        {
           // BackgroundAnimation.Begin();
        }

        void ScrollViewer_KeyUp(object sender, KeyEventArgs e)
        {
            Double delta = 0; // = ScrollViewer.ScrollableHeight %

            if(e.Key == Key.Up)
                delta = - ScrollViewer.ScrollableHeight / 5;
            else if(e.Key == Key.Down)
                delta = ScrollViewer.ScrollableHeight / 5;
            
            delta = ScrollViewer.VerticalOffset + delta;

            if (delta < 0)
                ScrollViewer.ScrollToVerticalOffset(0);
            else if (delta > ScrollViewer.ScrollableHeight)
                ScrollViewer.ScrollToVerticalOffset(ScrollViewer.ScrollableHeight);
            else
                ScrollViewer.ScrollToVerticalOffset(delta);
        }

        void ScrollViewer_MouseWheel(object sender, MouseWheelEventArgs e)
        {   
            ScrollViewer.ScrollToVerticalOffset(ScrollViewer.VerticalOffset - e.Delta);
        }

        /// <summary>
        /// LogLine will add a new line to Logger Message
        /// </summary>
        /// <param name="message">Message as String</param>
        public void LogLine(String message)
        {
            logger.Text += "\n" + (_lineNumber++).ToString() + ") " + message;
        }

        /// <summary>
        /// Logs a Message
        /// </summary>
        /// <param name="message">Message as String</param>
        public void Log(String message)
        {
            logger.Text += message;
        }

        #endregion

        #region Public Properties

        public String Text
        {
            get
            {
                return logger.Text;
            }
        }

        private void HeighlightTextBlock_Loaded(object sender, RoutedEventArgs e)
        {
            _highlightTextBlock = sender as TextBlock;

            Highlight(HeighlightText);
        }

        public String HeighlightText
        {
            get;
            set;
        }

        public Int32 SkipHighlight
        {
            get;
            set;
        }

        public String HelpLink
        {
            get;
            set;
        }

        public void Highlight(String stringValue)
        {
            _highlightTextBlock.Inlines.Clear();
            if (_highlightTextBlock != null && !String.IsNullOrEmpty(Text))
            {
                try
                {   
                    Int32 index = Text.IndexOf(stringValue);
 
                    for (Int32 i = 1; i <= SkipHighlight; i++)
                        index = Text.IndexOf(stringValue, index + stringValue.Length);

                    String s = Text.Substring(0, index);

                    Run run = new Run();
                    run.Text = s;
                    run.Foreground = new SolidColorBrush(Colors.Transparent);
                    _highlightTextBlock.Inlines.Add(run);

                    s = Text.Substring(index, stringValue.Length);

                    run = new Run();
                    run.Text = s;
                    run.Foreground = new SolidColorBrush(Colors.Red);
                    _highlightTextBlock.Inlines.Add(run);
                }
                catch
                {  }
            }

            (DocHelp.Child as TextBlock).MouseLeftButtonUp -= new MouseButtonEventHandler(Logger_MouseLeftButtonUp);

            if (!String.IsNullOrEmpty(HelpLink))
            {
                (DocHelp.Child as TextBlock).MouseLeftButtonUp += new MouseButtonEventHandler(Logger_MouseLeftButtonUp);
                DocHelp.Visibility = Visibility.Visible;               
            }
            else
            {   
                DocHelp.Visibility = Visibility.Collapsed;
            }
        }

        void Logger_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {   
            System.Windows.Browser.HtmlPage.Window.Navigate(new Uri(HelpLink), "_blank");
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        #endregion

        #region Public Events And Delegates

        #endregion

        #region Protected Methods

        #endregion

        #region Internal Properties

        #endregion

        #region Private Properties

        #endregion

        #region Private Delegates

        #endregion

        #region Private Methods

        #endregion

        #region Internal Methods

        #endregion

        #region Internal Events And Delegates

        #endregion

        #region Data

        /// <summary>
        /// Current line number of logger message
        /// </summary>
        private Int32 _lineNumber;

        /// <summary>
        /// Highlighting TextBlock
        /// </summary>
        private TextBlock _highlightTextBlock;

        #endregion
    }
}
