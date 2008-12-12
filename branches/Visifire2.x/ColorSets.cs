using System;
using System.Collections.Generic;

namespace Visifire.Charts
{   
    /// <summary>
    /// ColorSets contains a collection of ColorSet
    /// </summary>
    public class ColorSets : List<ColorSet>
    {
        public ColorSet GetColorSetByName(String ColorSetId)
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
