#if WPF

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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

#else
using System;
using System.Windows;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Windows.Markup;
using System.Collections.ObjectModel;
using System.Globalization;

#endif
using Visifire.Commons;

namespace Visifire.Charts
{
    /// <summary>
    /// An observable collection of axes.
    /// </summary>
    internal class AxisCollection : NoResetObservableCollection<Axis>
    {
        /// <summary>
        /// Initializes a new instance of the AxisCollection class.
        /// </summary>
        public AxisCollection()
        {

        }

        /// <summary>
        /// Ensures that a maximum of one horizontal and one vertical axis are 
        /// inserted into the collection.
        /// </summary>
        /// <param name="index">The index at which to insert the axis.</param>
        /// <param name="item">The axis to insert.</param>
        protected override void InsertItem(int index, Axis item)
        {
            //if (Enumerable.Any(this, axis => axis.Orientation == item.Orientation))
            //{
            //    if (item.Orientation == AxisOrientation.Horizontal)
            //    {
            //        throw new InvalidOperationException(Properties.Resources.AxisCollection_CannotHaveMoreThanOneHorizontalAxis);
            //    }
            //    else if (item.Orientation == AxisOrientation.Vertical)
            //    {
            //        throw new InvalidOperationException(Properties.Resources.AxisCollection_CannotHaveMoreThanOneVerticalAxis);
            //    }
            //}
            base.InsertItem(index, item);
        }
    }

    /// <summary>
    /// An observable collection that cannot be reset.  When clear is called
    /// items are removed individually, giving listeners the chance to detect
    /// each remove event and perform operations such as unhooking event 
    /// handlers.
    /// </summary>
    /// <typeparam name="T">The type of item in the collection.</typeparam>
    internal class NoResetObservableCollection<T> : ObservableCollection<T>
    {
        /// <summary>
        /// Instantiates a new instance of the NoResetObservableCollection 
        /// class.
        /// </summary>
        public NoResetObservableCollection()
        {
        }

        /// <summary>
        /// Clears all items in the collection by removing them individually.
        /// </summary>
        protected override void ClearItems()
        {
            IList<T> items = new List<T>(this);
            foreach (T item in items)
            {
                Remove(item);
            }
        }
    }
}
