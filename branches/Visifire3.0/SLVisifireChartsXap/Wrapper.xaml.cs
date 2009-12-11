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
using System.Windows.Browser;

namespace SLVisifireChartsXap
{
    /// <summary>
    /// SLVisifireChartsXap.Wrapper class
    /// </summary>
    public partial class Wrapper : UserControl
    {   
        public Wrapper()
        {
            InitializeComponent();
        }

        #region Scriptable methods

        /// <summary>
        /// Resize the wrapper
        /// </summary>
        /// <param name="width">Width</param>
        /// <param name="height">Height</param>
        [ScriptableMember()]
        public void Resize(Double width, Double height)
        {
            ResizeEventArgs e = new ResizeEventArgs();
            e.Width = width;
            e.Height = height;

            if (OnResize != null)
                OnResize(this, e);
        }

        /// <summary>
        /// Add new data xml 
        /// </summary>
        /// <param name="dataXML">xml string</param>
        [ScriptableMember()]
        public void AddDataXML(String dataXML)
        {
            DataXMLEventArgs e = new DataXMLEventArgs();

            e.DataXML = dataXML;
            e.DataUri = null;

            if (DataXMLAdded != null)
                DataXMLAdded(this, e);
        }

        /// <summary>
        /// Add chart data xml file Uri
        /// </summary>
        /// <param name="dataUri">Data xml file Uri</param>
        [ScriptableMember()]
        public void AddDataUri(String dataUri)
        {
            DataXMLEventArgs e = new DataXMLEventArgs();

            e.DataUri = dataUri;

            if (DataXMLAdded != null)
                DataXMLAdded(this, e);
        }

        /// <summary>
        /// Rerender the chart 
        /// </summary>
        [ScriptableMember()]
        public void ReRenderChart()
        {
            if (ReRender != null)
                ReRender(this, null);
        }

        /// <summary>
        /// Whether the data xml is loaded
        /// </summary>
        [ScriptableMember()]
        public Boolean IsDataLoaded
        {
            get;
            set;
        }

        #endregion Scriptable methods

        #region Public Events

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<DataXMLEventArgs> DataXMLAdded;

        /// <summary>
        /// ReRender EventHandler
        /// </summary>
        public event EventHandler ReRender;

        /// <summary>
        /// EventHandler OnResize
        /// </summary>
        public event EventHandler<ResizeEventArgs> OnResize;

        #endregion Public Events
    }
}
