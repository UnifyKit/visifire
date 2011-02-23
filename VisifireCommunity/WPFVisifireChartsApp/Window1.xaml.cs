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
using System.Windows.Markup;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using System.ComponentModel;
using Visifire.Charts;
using Visifire.Commons;
using System.Windows.Media.Animation;
using System.IO;
using System.Xml;
using System.Windows.Threading;
using System.Collections.ObjectModel;
using System.IO.Packaging;

namespace WPFVisifireChartsApp
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        TimeSurface ts = new TimeSurface();

        public Window1()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        KeyValuePair<object, double> sl = new KeyValuePair<object, double>();

        public KeyValuePair<object, double> SL
        {
            get
            {
                return sl;
            }
            set
            {
                sl = value;
                selectedXYTB.Text = string.Format("{0},{1}", sl.Key, sl.Value); ;
                Notify("SL");
            }
        }

        public TimeSurface TS
        {
            get
            {
                return ts;
            }
            set
            {
                ts = value;
                Notify("TS");
            }
        }


        protected void Notify(string propName)
        {
            var handle = PropertyChanged;
            if (handle != null)
                handle.Invoke(this, new PropertyChangedEventArgs(propName));
        }



        public class TimeSurface : AbstractSurface
        {
            Range<object> xRange = new Range<object> { Min = new DateTime(2011, 1, 1, 0, 0, 0), Max = new DateTime(2011, 1, 2, 0, 0, 0) };
            Range<double> yRange = new Range<double> { Min = -5.0, Max = 5.0 };
            Range<double> zRange = new Range<double> { Min = -5.0, Max = 5.0 };

            public override double GetValue(object x, double y, bool interpolation = true)
            {
                return y;
            }

            public override Range<Object> XRange
            {
                get { return xRange; }
            }

            public override Range<double> YRange
            {
                get { return yRange; }
            }

            public override Range<double> ZRange
            {
                get { return zRange; }
            }
        }



        public event PropertyChangedEventHandler PropertyChanged;
    }
}