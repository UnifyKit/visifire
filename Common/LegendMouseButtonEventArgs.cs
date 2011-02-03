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
    /// Visifire.Charts.LegendMouseButtonEventArgs class
    /// </summary>

#if SL &&!WP
    [System.Windows.Browser.ScriptableType]
#endif
    public sealed class LegendMouseButtonEventArgs : EventArgs
    {   
        #region Public Methods

        public LegendMouseButtonEventArgs(MouseButtonEventArgs e)
        {   
            DataPoint = null;
            DataSeries = null;
            MouseButtonEventArgs = e;

            // Set the DataPoint or DataSeries reference from ElementData
            if (e != null)
            {   
                ElementData elementData = (e.OriginalSource as FrameworkElement).Tag as ElementData;
                
                if (elementData != null && elementData.Element != null)
                {
                    Type elementType = elementData.Element.GetType();

                    if (elementType.Equals(typeof(DataSeries)))
                    {
                        // Set DataSeries reference
                        DataSeries = (DataSeries)elementData.Element;
                    }
                    else if (elementType.Equals(typeof(DataPoint)))
                    {
                        // Set DataPoint reference
                        DataPoint = (DataPoint)elementData.Element;
                    }
                }
            }
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
        /// DataPoint reference corresponding to mouse position.
        /// </summary>
        public DataPoint DataPoint
        {
            get;
            internal set;
        }

        /// <summary>
        /// DataSeries reference corresponding to mouse position.
        /// </summary>
        public DataSeries DataSeries
        {
            get;
            internal set;
        }

        #endregion
    }
}