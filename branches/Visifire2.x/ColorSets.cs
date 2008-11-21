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
    /// ColorSets contains a collection of ColorSet
    /// </summary>
    public class ColorSets : List<ColorSet>
    {
        public ColorSet GetColorSetByName(ColorSetNames ColorSetId)
        {
            foreach (ColorSet cs in this)
            {
                if (cs.Id == ColorSetId)
                {
                    cs.ReSet();
                    return cs;
                }
            }

            return null;
        }
    }
}
