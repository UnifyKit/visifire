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
using System.IO;
using Visifire.Charts;
using System.Web;
using Visifire.Commons;

namespace WPFVisifireChartsXbap
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class Page1 : Page
    {
        public Page1()
        {
            InitializeComponent();

            MyChart.Series[0].DataPoints[0].Color = Graphics.GetRandomColor();
            MyChart.MouseLeftButtonUp += new MouseButtonEventHandler(MyChart_MouseLeftButtonUp);
        }

        void MyChart_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            MyChart.Series[0].DataPoints[0].Color = new SolidColorBrush(Colors.Green);
        }
    }
}