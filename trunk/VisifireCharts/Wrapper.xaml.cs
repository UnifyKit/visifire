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
using System.Windows.Browser;

namespace VisifireCharts
{

    public partial class Wrapper : UserControl
    {
        public Wrapper()
        {
            InitializeComponent();
        }

        #region Scriptable methods

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

        #endregion Scriptable methods

        #region Public Events
        public event EventHandler<DataXMLEventArgs> DataXML;

        public event EventHandler ReRender;
        #endregion Public Events
    }
}
