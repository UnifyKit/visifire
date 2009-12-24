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
using System.Windows.Data;

namespace SLVisifireChartsXap
{/// <summary>
    /// Logger creates a multiline textbox with a vertical scrollbar, helps to log message
    /// </summary>
    public partial class Logger : UserControl
    {
        public Logger()
        {
            InitializeComponent();
            lineNumber = 1;
        }

        /// <summary>
        /// LogLine will add a new line to Logger Message
        /// </summary>
        /// <param name="message">Message as String</param>
        public void LogLine(String message)
        {
            logger.Text += "\n" + (lineNumber++).ToString() + ") " + message;
        }

        /// <summary>
        /// Logs a Message
        /// </summary>
        /// <param name="message">Message as String</param>
        public void Log(String message)
        {
            logger.Text += message;
        }

        /// <summary>
        /// Current line number of logger message
        /// </summary>
        private Int32 lineNumber
        {
            get;
            set;
        }
    }
}