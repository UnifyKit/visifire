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

        #endregion
    }
}
