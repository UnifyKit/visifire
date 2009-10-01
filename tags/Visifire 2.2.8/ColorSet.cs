using System;
using System.Windows.Media;
using System.Collections.ObjectModel;

namespace Visifire.Charts
{   
    /// <summary>
    /// ColorSet contains a collection of Brushes
    /// </summary>
    public class ColorSet: Object
    {
        /// <summary>
        ///  Initializes a new instance of the Visifire.Charts.ColorSet class
        /// </summary>
        public ColorSet()
        {
            Brushes = new Collection<Brush>();
        }

        /// <summary>
        /// ColorSet Id
        /// </summary>
        public String Id
        {
            get;
            set;
        }
        
        /// <summary>
        /// Brush collection
        /// </summary>
        public Collection<Brush> Brushes
        {
            get;
            set;
        }

        /// <summary>
        /// Returns a new color from ColorSet
        /// </summary>
        /// <returns>Brush</returns>
        public Brush GetNewColorFromColorSet()
        {   
            if (colorSetIndex == Brushes.Count)
                colorSetIndex = 0;

            return Brushes[colorSetIndex++];
        }

        /// <summary>
        /// Reset ColorSet index
        /// </summary>
        internal void ResetIndex()
        {
            colorSetIndex = 0;
        }

        /// <summary>
        /// Index for ColorSet
        /// </summary>
        private int colorSetIndex = 0;
    }
}