#if WPF

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Markup;
using System.IO;
using System.Xml;
using System.Threading;
using System.Windows.Automation.Peers;
using System.Windows.Automation;
using System.Globalization;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.Windows.Threading;
#else
using System;
using System.Windows;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Windows.Markup;
using System.Collections.ObjectModel;

#endif

namespace Visifire.Commons
{   
    /// <summary>
    /// Visifire Control interface
    /// </summary>
    public abstract class VisifireControl: Control
    {   
        /// <summary>
        /// Root visual element of the chart control 
        /// </summary>
        public Grid RootElement { get; set;  }

        /// <summary>
        /// Flag if template is already applied
        /// </summary>
        Boolean IsTemplateApplied { get; set; }

        /// <summary>
        /// ToolTip object
        /// </summary>
        internal Border ToolTipElement { get; set; }
    }

}