using System;

namespace Visifire.Charts
{
    public class ElementData
    {
        public ElementData()
        {            

        }

        /// <summary>
        /// Visual object name
        /// </summary>
        public String VisualElementName
        {
            get;
            internal set;
        }

        /// <summary>
        /// Visifire Element reference
        /// </summary>
        public object Element
        {
            get;
            internal set;
        }
    }
}
