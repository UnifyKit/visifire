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
using System.Security;
using System.Windows;
using System.Windows.Input;

namespace Visifire.Charts
{
    /// <summary>
    /// Visifire.Charts.PlotAreaMouseButtonEventArgs class
    /// </summary>

#if SL &&!WP
    [System.Windows.Browser.ScriptableType]
#endif
    public class PlotAreaMouseButtonEventArgs : EventArgs
    {
        #region Public Methods

        public PlotAreaMouseButtonEventArgs(MouseButtonEventArgs e)
        {
            XValue = Double.NaN;
            YValue = Double.NaN;
            MouseButtonEventArgs = e;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// MouseButton event arguments
        /// </summary>
        public MouseButtonEventArgs MouseButtonEventArgs
        {
            get;
            internal set;
        }

        /// <summary>
        /// XValue corresponding to mouse position
        /// </summary>
        public Object XValue
        {
            get;
            internal set;
        }

        /// <summary>
        /// YValue corresponding to mouse position
        /// </summary>
        public Double YValue
        {
            get;
            internal set;
        }

        #endregion
    }
}