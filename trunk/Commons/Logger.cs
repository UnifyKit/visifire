/*
      Copyright (C) 2008 Webyog Softworks Private Limited

     This file is part of VisifireCharts.
 
     VisifireCharts is a free software: you can redistribute it and/or modify
     it under the terms of the GNU General Public License as published by
     the Free Software Foundation, either version 3 of the License, or
     (at your option) any later version.
 
     VisifireCharts is distributed in the hope that it will be useful,
     but WITHOUT ANY WARRANTY; without even the implied warranty of
     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
     GNU General Public License for more details.
 
     You should have received a copy of the GNU General Public License
     along with VisifireCharts.  If not, see <http://www.gnu.org/licenses/>.
 
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
using System.Windows.Browser;

namespace Visifire
{
    /// <summary>
    /// This Logger class can be used to Log messages into a target HTML element.
    /// Log messages are seperated by a horizontal rule.
    /// </summary>
    
    public class Logger
    {
        #region Public Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg">Message to be logged.</param>
        public static void Log(string targetID, string msg)
        {
            TargetID = targetID;

            if (targetElement != null)
            {
                HtmlElement span = HtmlPage.Document.CreateElement("span");
                span.SetAttribute(browserName.Contains("Netscape") ? "textContent" : "innerText", msg);
                targetElement.AppendChild(span);
                targetElement.AppendChild(HtmlPage.Document.CreateElement("br"));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg">Message to be logged.</param>
        public static void Log(string msg)
        {
            if (targetElement != null)
            {
                HtmlElement span = HtmlPage.Document.CreateElement("span");
                span.SetAttribute(browserName.Contains("Netscape") ? "textContent" : "innerText", msg);
                targetElement.AppendChild(span);
                targetElement.AppendChild(HtmlPage.Document.CreateElement("br"));
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Target HTML ID where Messages are to be logged
        /// </summary>
        public static string TargetID
        {
            get
            {
                return targetID;
            }
            set
            {
                targetID = value;
                if (HtmlPage.Document.GetElementById(targetID) != null)
                    targetElement = HtmlPage.Document.GetElementById(targetID);
            }
        }

        #endregion Public Properties

        #region Data
        private static string browserName = HtmlPage.BrowserInformation.Name;
        //private static HtmlDocument doc = new HtmlDocument();
        private static string targetID;
        private static HtmlElement targetElement;
        #endregion

    }
}