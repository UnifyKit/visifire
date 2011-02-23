using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Visifire.Charts
{
    public interface ISurface<X, Y, Z>
    {
        Z GetValue(X x, Y y, bool interpolation = true);
        Range<X> XRange { get; }
        Range<Y> YRange { get; }
        Range<Z> ZRange { get; }
    }

    public class Range<T>
    {
        public T Min
        {
            get;
            set;
        }
        public T Max
        {
            get;
            set;
        }
    }

    public abstract class AbstractSurface : ISurface<object, double, double>
    {
        private EventHandler _update;
        public event EventHandler Update
        {
            remove
            {
                _update -= value;
            }
            add
            {
                _update += value;
            }
        }

        public void FireUpdateEvent()
        {
            if (_update != null)
                _update(this, null);
        }

        public bool IsXDateTime
        {
            get
            {
                if (XRange.Min is DateTime)
                {
                    if (XRange.Max is DateTime)
                    {
                        return true;
                    }
                    throw new Exception("Max and Min of XRange are not in same type");
                }
                else
                {
                    return false;
                }
            }
        }

        public abstract double GetValue(object x, double y, bool interpolation = true);

        public abstract Range<object> XRange
        {
            get;
        }

        public abstract Range<double> YRange
        {
            get;
        }


        public abstract Range<double> ZRange
        {
            get;
        }
    }
}
