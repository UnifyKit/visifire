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
    public partial class Wrapper : UserControl
    {
        public Wrapper()
        {
            InitializeComponent();
        }

        #region Scriptable methods
        [ScriptableMember()]
        public void Resize(Double width, Double height)
        {
            ResizeEventArgs e = new ResizeEventArgs();
            e.Width = width;
            e.Height = height;

            if (OnResize != null)
                OnResize(this, e);
        }

        [ScriptableMember()]
        public void AddDataXML(String dataXML)
        {
            DataXMLEventArgs e = new DataXMLEventArgs();

            e.DataXML = dataXML;
            e.DataUri = null;

            if (DataXML != null)
                DataXML(this, e);
        }

        [ScriptableMember()]
        public void AddDataUri(String dataUri)
        {
            DataXMLEventArgs e = new DataXMLEventArgs();

            e.DataUri = dataUri;

            if (DataXML != null)
                DataXML(this, e);
        }

        [ScriptableMember()]
        public void ReRenderChart()
        {
            if (ReRender != null)
                ReRender(this, null);
        }

        [ScriptableMember()]
        public Boolean IsDataLoaded
        {
            get;
            set;
        }
        #endregion Scriptable methods

        #region Public Events
        public event EventHandler<DataXMLEventArgs> DataXML;

        public event EventHandler ReRender;

        public event EventHandler<ResizeEventArgs> OnResize;
        #endregion Public Events
    }
}
