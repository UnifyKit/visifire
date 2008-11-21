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
using System.Collections.Specialized;

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
using System.Collections.Specialized;
using System.Collections.ObjectModel;

#endif

namespace Visifire.Charts
{
    public class XWiseStackedData
    {
        public XWiseStackedData()
        {
            Positive = new ObservableCollection<DataPoint>();
            Negative = new ObservableCollection<DataPoint>();

            Positive.CollectionChanged += new NotifyCollectionChangedEventHandler(Positive_CollectionChanged);
            Negative.CollectionChanged += new NotifyCollectionChangedEventHandler(Negative_CollectionChanged);

            _isUpdated = false;
        }

        void Positive_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            _isUpdated = false;
        }

        void Negative_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            _isUpdated = false;
        }

        private void Update()
        {
            _isUpdated = true;
            _positiveYValueSum = (from dataPoint in Positive where !Double.IsNaN(dataPoint.YValue) select dataPoint.YValue).Sum();
            _negativeYValueSum = (from dataPoint in Negative where !Double.IsNaN(dataPoint.YValue) select dataPoint.YValue).Sum();
        }

        public ObservableCollection<DataPoint> Positive
        {
            get;
            set;
        }

        public ObservableCollection<DataPoint> Negative
        {
            get;
            set;
        }

        public Double PositiveYValueSum
        {
            get
            {
                if (!_isUpdated) Update();
                return _positiveYValueSum;
            }
        }
        public Double NegativeYValueSum
        {
            get
            {
                if (!_isUpdated) Update();
                return _negativeYValueSum;
            }
        }
        public Double AbsoluteYValueSum
        {
            get
            {
                return Math.Abs(NegativeYValueSum) + Math.Abs(PositiveYValueSum);
            }
        }

        #region Data
        private Double _positiveYValueSum;
        private Double _negativeYValueSum;
        private Boolean _isUpdated;
        #endregion
    }
}
