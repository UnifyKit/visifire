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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Visifire.Commons
{
    public abstract class Container:VisualObject
    {
        #region Public Properties

        public Double Padding
        {
            get
            {
                return _margin;
            }
            set
            {
                _margin = value;
                _paddingOverride = true;
            }
        }

        public String Theme
        {
            get
            {
                return _theme;
            }
            set
            {
                _theme = value;

                AppliedTheme = (System.Collections.Generic.Dictionary<String, System.Collections.Generic.Dictionary<String, Object>>)Themes[_theme];
            }
        }

        public virtual LabelBase ToolTip
        {
            get;
            set;
        }

        public System.Collections.Generic.Dictionary<String, System.Collections.Generic.Dictionary<String, Object>> AppliedTheme
        {
            get
            {
                return _appliedTheme;
            }
            set
            {
                _appliedTheme = value;
            }
        }

        #endregion Public Properties

        #region Data
        private Double _margin;

        public Themes Themes = new Themes();
        private String _theme;
        public System.Collections.Generic.Dictionary<String, System.Collections.Generic.Dictionary<String, Object>> _appliedTheme;
        protected Boolean _paddingOverride = false;
        #endregion Data
    }
}
