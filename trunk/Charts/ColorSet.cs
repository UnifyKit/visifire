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
using System.Windows.Documents;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Visifire.Commons;

namespace Visifire.Charts
{
    public class ColorSet: Canvas
    {
        #region Public Methods
        /// <summary>
        /// Constructor for palette
        /// </summary>
        public ColorSet()
        {
            _iterator = 0;
        }

        #endregion Public Methods

        #region Private Methods
        /// <summary>
        /// Checks if the parent of the element is a chart or not
        /// </summary>
        private void ValidateParent()
        {
            if (Parent.GetType().Name != "Chart")
            {
                System.Diagnostics.Debug.WriteLine("The parent of palette must be Chart");
                throw(new Exception("Invalid Parent for Palette"));
            }
        }

        /// <summary>
        /// Checks if a name is given to the palette or not
        /// </summary>
        private void ValidateProperty()
        {
            if (String.IsNullOrEmpty(Name))
            {
                System.Diagnostics.Debug.WriteLine("Palette must have a name");
                throw(new Exception("Please give a palette name"));
            }
        }

        /// <summary>
        /// Checks if all children are valid
        /// </summary>
        private void ValidateChildren()
        {
            foreach (Canvas child in Children)
            {
                if (child.GetType().Name != "Color")
                {
                    System.Diagnostics.Debug.WriteLine("Palette can have only legend as Color");
                    throw (new Exception("Invalid legend element in Palette"));
                }
            }
        }

        #endregion Private Methods

        #region Internal Methods
        /// <summary>
        /// Initializes the palette
        /// </summary>
        internal void Init()
        {
            // Checks if the parent is valid or not
            ValidateParent();

            // Checks if the name given is proper or not
            ValidateProperty();

            // Checks if children are of valid type
            ValidateChildren();

            // if no children are present then throw exception
            if (Children.Count <= 0)
            {
                System.Diagnostics.Debug.WriteLine("Palette must have atleast one legend element of type Color");
                throw (new Exception("Palette has no children"));
            }

        }

        /// <summary>
        /// This function always retrives the next color is the color set, and on reaching end of the it restarts
        /// </summary>
        /// <returns></returns>
        internal Brush GetColor()
        {
            return ((Children[_iterator++ % Children.Count] as Color).Background);
        }

        /// <summary>
        /// This function gets a color based on index. If the index exceeds the limit it takes mod and retrives a color
        /// </summary>
        /// <param name="index"> index of the color to be retrived</param>
        /// <returns></returns>
        internal Brush GetColor(Int32 index)
        {

            return ((Children[index % Children.Count] as Color).Background);

        }

        #endregion Internal Methods

        #region Data
        private Int32 _iterator;
        #endregion Data
    }
}
