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

#if WPF

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Animation;

#else

using System;
using System.Windows;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Collections.Generic;

#endif

using System.Windows.Data;
using System.Reflection;

namespace Visifire.Charts
{   
    public class DataMapping
    {   
        /// <summary>
        /// Property Name
        /// </summary>
        public String MemberName
        {   
            get;
            set;
        }

        /// <summary>
        /// Property path as String
        /// </summary>
        public String Path
        {   
            get;
            set;
        }
        
        /// <summary>
        /// Map property value from source to target
        /// </summary>
        /// <param name="source">DataSource object</param>
        /// <param name="target">Target object</param>
        public void Map(Object dataSource, Object target)
        {
            // Get value of the property
            Object propertyValue = GetPropertyValue(dataSource);

            // Property info of target object
            System.Reflection.PropertyInfo targetPropertyInfo = target.GetType().GetProperty(MemberName);

            // Nullable<InnerType>
            Type innerTypeOfNullableProperty;

            if (!IsNullable(targetPropertyInfo.PropertyType, out innerTypeOfNullableProperty))
            {
                // Convert 'the type of the property value of source' to 'the type of the property value of target'
                propertyValue = Convert.ChangeType(propertyValue, targetPropertyInfo.PropertyType,
                System.Globalization.CultureInfo.CurrentCulture);
            }
            else if (innerTypeOfNullableProperty != null)
            {
                // Convert 'the type of the property value of source' to 'the type of the property value of target'
                propertyValue = Convert.ChangeType(propertyValue, innerTypeOfNullableProperty,
                System.Globalization.CultureInfo.CurrentCulture);
            }

            // Set value of the property of target
            target.GetType().GetProperty(MemberName).SetValue(target, propertyValue, null);
        }

        /// <summary>
        /// Check whether the type of the property is nullable
        /// </summary>
        /// <param name="type">Pass Nullable Type</param>
        /// <param name="innerType">If type is nullable then returns the inter type</param>
        /// <returns></returns>
        public static bool IsNullable(Type type, out Type innerType)
        {
            bool blnResult;
            innerType = null;

            Type objGenericType = null;

            if (!type.IsGenericType)
            {
                blnResult = false;
            }
            else
            {
                objGenericType = type.GetGenericTypeDefinition();
                blnResult = objGenericType.Equals(typeof(Nullable<>));

                if (blnResult)// If nullable type
                {
                    PropertyInfo[] propertyInfos = type.GetProperties();
                    innerType = propertyInfos[1].PropertyType;
                }
            }

            return blnResult;
        }

        /// <summary>
        /// Get property value of the Container object
        /// </summary>
        public Object GetPropertyValue(Object source)
        {
            return Eval(source, Path);
        }

        private static readonly DependencyProperty DummyProperty = DependencyProperty.RegisterAttached
            ("Dummy" , typeof(Object), typeof(DependencyObject), new PropertyMetadata(null));

        internal static Object Eval(Object source, String pathExpression)
        {   
            Binding binding = new Binding(pathExpression) { Source = source };
            ContentControl dependencyObject = new ContentControl();
            BindingOperations.SetBinding(dependencyObject, DummyProperty, binding);
            return dependencyObject.GetValue(DummyProperty);
        }
    }
}