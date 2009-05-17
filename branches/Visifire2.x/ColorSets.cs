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
using System.Collections.Generic;

namespace Visifire.Charts
{   
    /// <summary>
    /// ColorSets is a collection of ColorSet
    /// </summary>
    public class ColorSets : List<ColorSet>
    {   
        /// <summary>
        /// Returns a ColorSet by ColorSet id (name of a ColorSet)
        /// </summary>
        /// <param name="ColorSetId">Id of a ColorSet</param>
        /// <returns>ColorSet</returns>
        public ColorSet GetColorSetByName(String ColorSetId)
        {   
            foreach (ColorSet cs in this)
            {   
                if (cs.Id == ColorSetId)
                {
                    cs.ResetIndex();
                    return cs;
                }
            }

            return null;
        }
    }
}
