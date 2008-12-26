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

        public Brush GetNewColorFromColorSet()
        {
            if (colorSetIndex == Brushes.Count)
                colorSetIndex = 0;

            return Brushes[colorSetIndex++];
        }

        internal void ReSet()
        {
            colorSetIndex = 0;
        }

        private int colorSetIndex = 0;
    }
}