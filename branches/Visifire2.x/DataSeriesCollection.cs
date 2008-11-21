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
using System.Windows.Threading;
using System.ComponentModel;

namespace Visifire.Charts
{
    public class DataSeriesCollection : ObservableCollection<DataSeries>
    {

    }

//    /// <summary>
//    /// Collection that supports INotifyCollectionChanged notification from seperate threads
//    /// </summary>
//    /// <typeparam name="T">The type of object to store in the collections</typeparam>
//    public class ThreadableObservableCollection<T> : ObservableCollection<T>
//    {

//// #if SL

////        private Dispatcher currentDispatcher;

////        /// <summary>
////        /// default constructor
////        /// Here we must get the Dispatcher object
////        /// </summary>
////        public ThreadableObservableCollection()
////        {
////            LoadDispatcher();
////        }

////        public void LoadDispatcher()
////        {
////            if (Application.Current != null &&
////                Application.Current.RootVisual != null &&
////                Application.Current.RootVisual.Dispatcher != null)
////            {
////                currentDispatcher = Application.Current.RootVisual.Dispatcher;
////            }
////            else // if we did not get the Dispatcher throw an exception
////            {
////                  throw new InvalidOperationException("This object must be initialized after that the RootVisual has been loaded");
////            }
////        }

////        protected override void InsertItem(int index, T item)
////        {
////            if (currentDispatcher == null)
////            {
////                base.InsertItem(index, item);
////                return;
////            }

////            if (currentDispatcher.CheckAccess())
////            {
////                try
////                {
////                    base.InsertItem(index, item);
////                }
////                catch (Exception e)
////                {   
////                    InsertItem(index, item);
////                    // currentDispatcher.BeginInvoke(new Action<int, T>(InsertItem), index, item);
////                }
////            }
////            else
////                currentDispatcher.BeginInvoke(new Action<int, T>(InsertItem), index, item);

////        }

////        protected override void ClearItems()
////        {
////            if (currentDispatcher == null || currentDispatcher.CheckAccess())
////               base.ClearItems();
////            else
////                currentDispatcher.BeginInvoke(new Action(ClearItems));
////        }

////        protected override void RemoveItem(int index)
////        {
////            if (currentDispatcher == null || currentDispatcher.CheckAccess())
////                base.RemoveItem(index);
////            else
////                currentDispatcher.BeginInvoke(new Action<int>(RemoveItem), index);
////        }

////        protected override void SetItem(int index, T item)
////        {
////            if (currentDispatcher == null || currentDispatcher.CheckAccess())
////                base.SetItem(index, item);
////            else
////                currentDispatcher.BeginInvoke(new Action<int, T>(SetItem), index, item);
////        }

////        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
////        {

////            if (currentDispatcher == null || currentDispatcher.CheckAccess())
////                base.OnPropertyChanged(e);
////            else
////            if (currentDispatcher != null)
////                currentDispatcher.BeginInvoke(new Action<PropertyChangedEventArgs>(OnPropertyChanged), e);
////        }

// #endif

 //   }
}