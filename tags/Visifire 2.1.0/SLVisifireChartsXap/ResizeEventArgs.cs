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

namespace SLVisifireChartsXap
{
    /// <summary>
    /// SLVisifireChartsXap.ResizeEventArgs class
    /// </summary>
    public class ResizeEventArgs: EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the SLVisifireChartsXap.ResizeEventArgs class
        /// </summary>
        public ResizeEventArgs()
            : base()
        {
        }

        /// <summary>
        /// Width property
        /// </summary>
        public Double Width { get; set; }

        /// <summary>
        /// Height property
        /// </summary>
        public Double Height { get; set; }
    }
}
