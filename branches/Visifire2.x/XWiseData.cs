#if WPF

using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

#else
using System;
using System.Linq;
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
            _positiveYValueSum = (from dataPoint in Positive where !Double.IsNaN(dataPoint.InternalYValue) select dataPoint.InternalYValue).Sum();
            _negativeYValueSum = (from dataPoint in Negative where !Double.IsNaN(dataPoint.InternalYValue) select dataPoint.InternalYValue).Sum();
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
