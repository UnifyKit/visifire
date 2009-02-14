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
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Visifire;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Visifire.Commons;
using System.Linq;

namespace Visifire.Charts
{
    public class Theme
    {
        public Theme()
        {
            ColorSet = new ColorSet();
            Styles = new List<ElementStyle>();
        }

        public Object GetValueFromTheme(Elements Element, String PropertyName)
        {
           var abc = from style in Styles 
                     where (style.Element == Element)
                     select style;

           if (abc.Count<ElementStyle>() > 0)
           {
               ElementStyle elementStyle = (abc.First<ElementStyle>() as ElementStyle);

               var propertyValuePairList = from propertyValuePair in elementStyle.PropertyValueCollection
                     where (propertyValuePair.PropertyName == PropertyName)
                     select propertyValuePair;

               if (propertyValuePairList.Count<PropertyValuePair>() > 0)
                   return (propertyValuePairList.First<PropertyValuePair>() as PropertyValuePair).Value;
               else
                   return null;
           }
           else
               return null;
        }
        
        public String Id
        {
            get;
            set;
        }

        public String DefaultColorSet
        {
            get;
            set;
        }

        public ColorSet ColorSet
        {
            get;
            set;
        }

        public List<ElementStyle> Styles
        {
            get;
            set;
        }
    }
    
    public class PropertyValuePair : Object
    {
        public PropertyValuePair()
        {

        }

        public String PropertyName
        {
            get;
            set;
        }

        public Object Value
        {
            get;
            set;
        }
    }

    /// <summary>
    /// Style for an Chart Element
    /// </summary>
    public class ElementStyle : Object
    {
        public ElementStyle()
        {
            PropertyValueCollection = new List<PropertyValuePair>();
        }

        public Elements Element
        {
            get;
            set;
        }

        public List<PropertyValuePair> PropertyValueCollection
        {
            get;
            set;
        }
    }

}