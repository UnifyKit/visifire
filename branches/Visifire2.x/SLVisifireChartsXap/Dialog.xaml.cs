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

namespace SLVisifireChartsXap
{
    /// <summary>
    /// SLVisifireChartsXap.Dialog class
    /// </summary>
    public partial class Dialog : UserControl
    {
        #region Public Methods

        /// <summary>
        /// Initializes a new instance of the SLVisifireChartsXap.Dialog class
        /// </summary>
        public Dialog()
        {
            InitializeComponent();

            this.DialogOutStoryBoard.Completed += new EventHandler(DialogOutStoryBoard_Completed);
            CloseButton.Click += new RoutedEventHandler(CloseButton_Click);
            this.SetValue(Canvas.ZIndexProperty, 1000000);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Message text
        /// </summary>
        public String Message
        {
            set
            {
                Info.Text = value;
            }
            get
            {
                return Info.Text;
            }
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
        
        /// <summary>
        /// Event handler on click event for close button
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">RoutedEventArgs</param>
        void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            DialogOutStoryBoard.Begin();
        }

        /// <summary>
        /// Event handler for completed event of storyBoard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void DialogOutStoryBoard_Completed(object sender, EventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        #endregion

        #region Internal Methods

        #endregion

        #region Internal Events And Delegates

        #endregion

        #region Data

        #endregion
    }
}
