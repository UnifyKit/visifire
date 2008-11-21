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

namespace Visifire.Charts
{   
    /// <summary>
    /// ColorSet contains a collection of Brushes
    /// </summary>
    public class ColorSet: Object
    {
        public ColorSet()
        {   
            Brushes = new List<Brush>();
        }

        /// <summary>
        /// ColorSet Id
        /// </summary>
#if SL
       // [System.ComponentModel.TypeConverter(typeof(Converters.ColorSetNameConverter))]
#endif
        public ColorSetNames Id
        {
            get;
            set;
        }
        
        /// <summary>
        /// Brush collection
        /// </summary>
        public List<Brush> Brushes
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