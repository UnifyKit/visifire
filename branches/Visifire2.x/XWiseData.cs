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
    
    /// <summary>
    /// X wise groupping of DataPoints
    /// (DataPoints are grouped into Positive and negative collection)
    /// </summary>
    internal class XWiseStackedData
    {
        #region Public Methods

        /// <summary>
        /// Initializes a new instance of the Visifire.Charts.XWiseStackedData class
        /// </summary>
        public XWiseStackedData()
        {
            Positive = new ObservableCollection<DataPoint>();
            Negative = new ObservableCollection<DataPoint>();

            Positive.CollectionChanged += new NotifyCollectionChangedEventHandler(Positive_CollectionChanged);
            Negative.CollectionChanged += new NotifyCollectionChangedEventHandler(Negative_CollectionChanged);

            _isUpdated = false;
        }
        
        #endregion

        #region Public Properties

        /// <summary>
        /// Collection of DataPoints with positive YValues
        /// </summary>
        public ObservableCollection<DataPoint> Positive
        {
            get;
            set;
        }

        /// <summary>
        /// Collection of DataPoints with negative YValues
        /// </summary>
        public ObservableCollection<DataPoint> Negative
        {
            get;
            set;
        }

        /// <summary>
        /// Positive sum of YValues
        /// </summary>
        public Double PositiveYValueSum
        {
            get
            {
                if (!_isUpdated) Update();
                return _positiveYValueSum;
            }
        }

        /// <summary>
        /// Negative sum of YValues
        /// </summary>
        public Double NegativeYValueSum
        {
            get
            {
                if (!_isUpdated) Update();
                return _negativeYValueSum;
            }
        }

        /// <summary>
        /// Absolute sum of YValues
        /// </summary>
        public Double AbsoluteYValueSum
        {
            get
            {
                return Math.Abs(NegativeYValueSum) + Math.Abs(PositiveYValueSum);
            }
        }

        #endregion

        #region Public Events And Delegates

        #endregion

        #region Protected Methods

        #endregion

        #region Internal Properties

        #endregion

        #region Private Properties

        #endregion

        #region Private Delegates

        #endregion

        #region Private Methods
        
        /// <summary>
        /// Event handler manages the addition and removal of DataPoints from positive collection of DataPoints 
        /// </summary>
        private void Positive_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            _isUpdated = false;
        }

        /// <summary>
        /// Event handler manages the addition and removal of DataPoints from negative collection of DataPoints 
        /// </summary>
        private void Negative_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            _isUpdated = false;
        }

        /// <summary>
        /// Update sum of YValues
        /// </summary>
        private void Update()
        {
            _isUpdated = true;
            _positiveYValueSum = (from dataPoint in Positive where !Double.IsNaN(dataPoint.InternalYValue) select dataPoint.InternalYValue).Sum();
            _negativeYValueSum = (from dataPoint in Negative where !Double.IsNaN(dataPoint.InternalYValue) select dataPoint.InternalYValue).Sum();
        }

        #endregion

        #region Internal Methods

        #endregion

        #region Internal Events And Delegates

        #endregion

        #region Data

        /// <summary>
        /// Positive sum of YValues
        /// </summary>
        private Double _positiveYValueSum;

        /// <summary>
        /// Negative sum of YValues
        /// </summary>
        private Double _negativeYValueSum;

        /// <summary>
        /// Whether the x wise data is updated
        /// </summary>
        private Boolean _isUpdated;

        #endregion
    }
}
