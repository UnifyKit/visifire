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
using System.Collections;
#else
using System;
using System.Linq;
using System.Windows;
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
using System.Collections;
using System.IO;
#endif

namespace Visifire.Charts
{

    public partial class Chart : Control
    {
        internal void Init()
        {
            SetValue(TitlesProperty, new ObservableCollection<Title>());
            Titles.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(Titles_CollectionChanged);
        }
        
#if WPF
        protected override void OnTemplateChanged(ControlTemplate oldTemplate, ControlTemplate newTemplate)
        {
            base.OnTemplateChanged(oldTemplate, newTemplate);
        }
#endif

        private void Title_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (IsTemplateApplied)
            {
#if WPF
                Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.SystemIdle, new RenderDelegate(Render));
#else
                Render();
#endif
            }
        }

        void Titles_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            //if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            //{
            //    if (e.NewItems != null)
            //        foreach (Title title in e.NewItems)
            //        {
            //            title.PropertyChanged -= Title_PropertyChanged;
            //            title.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(Title_PropertyChanged);
            //        }
            //}

            if (IsTemplateApplied)
            {
#if WPF
                Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.SystemIdle, new RenderDelegate(Render));
#else
                Render();
#endif
            }
        }
        
        public ObservableCollection<Title> Titles
        {
            get
            {
                return (ObservableCollection<Title>)GetValue(TitlesProperty);
            }
            set
            {
                SetValue(TitlesProperty, value);
            }
        }

        public static readonly DependencyProperty TitlesProperty = DependencyProperty.Register("Titles", typeof(ObservableCollection<Title>), typeof(Chart), null);

        /// <summary>
        /// Enable auto refresh while designing in Blend.
        /// </summary>
        public bool RefreshEnabled
        {
            set
            {
                SetValue(RefreshEnabledProperty, value);
            }
            get
            {
                return (bool)GetValue(RefreshEnabledProperty);
            }
        }

        public static readonly DependencyProperty RefreshEnabledProperty = DependencyProperty.Register("RefreshEnabled", typeof(bool), typeof(Chart), new PropertyMetadata(OnRefreshEnabledPropertyChanged));

        private static void OnRefreshEnabledPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Chart c = d as Chart;

            if (c.IsTemplateApplied)
            {
#if WPF
                c.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.SystemIdle, new RenderDelegate(c.Render));
#else
                c.Render();
#endif
            }
        }

        /// <summary>
        /// Currently visible ChartArea
        /// </summary>
        internal ChartArea ChartArea
        {
            get;
            set;
        }
        
        /// <summary>
        /// Center visual panel of the chart control 
        /// </summary>
        internal StackPanel CenterPanel
        {
            get;
            set;
        }

        /// <summary>
        /// Root visual element of the chart control 
        /// </summary>
        internal Grid RootElement
        {
            get;
            set;
        }

        /// <summary>
        /// Flag if template is already applied
        /// </summary>
        internal Boolean IsTemplateApplied
        {
            get;
            set;
        }

        /// <summary>
        /// On Template applied
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            RootElement = GetTemplateChild("RootElement") as Grid;
            CenterPanel = GetTemplateChild("CenterPanel") as StackPanel;
            IsTemplateApplied = true;

            if (RootElement == null)
            {
                ApplyDefaultTemplate();
            }


        }
        
        public List<Title> GetTitlesDockedInsidePlotArea()
        {
            if (Titles != null)
            {
                var titlesDockedInsidePlotArea =
                    from title in Titles
                    where (title.DockInsidePlotArea == true)
                    select title;

                return titlesDockedInsidePlotArea.ToList<Title>();
            }
            return null;
        }

        public List<Title> GetTitlesDockedOutSidePlotArea()
        {
            if (Titles != null)
            {
                var titlesDockedOutSidePlotArea = 
                    from title in Titles 
                    where (title.DockInsidePlotArea == false)
                    select title;

                return titlesDockedOutSidePlotArea.ToList<Title>();
            }
            else
                return null;
        }

        /// <summary>
        /// Apply style on a existing Chart without rerender the chart
        /// </summary>
        public void ApplyStyleProperties()
        {

        }

        /// <summary>
        /// Create visual tree before rendering
        /// </summary>
        /// <returns></returns>
        private ChartArea CreateVisualTree()
        {
            ChartArea chatArea = new ChartArea(this as Chart);

            chatArea.PropertyChanged += delegate(Object sender, EventArgs e)
            {
                if (IsTemplateApplied)
                {
#if WPF
                    Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.SystemIdle, new RenderDelegate(Render));
#else
                    Render();
#endif
                }
            };
            
            return chatArea;
        }

        /// <summary>
        /// Render replace the existing chart with a chart
        /// </summary>
        internal void Render()
        {
            //if (!IsTemplateApplied)
            //  ApplyDefaultTemplate();


            if (!RENDER_LOCK || RefreshEnabled)
            {
                RENDER_LOCK = true;

                ChartArea chartArea = CreateVisualTree();
                
                if (chartArea != null)
                {
                    chartArea.Loaded += delegate(object sender, RoutedEventArgs e)
                    {
                        if (ChartArea != null)
                            RootElement.Children.Remove(ChartArea);

                        ChartArea = sender as ChartArea;
                        (sender as ChartArea).Visibility = Visibility.Visible;

                        RENDER_LOCK = false;
                        RootElement.UpdateLayout();
                    };

                    RootElement.Children.Add(chartArea);
                }
            }
        }
        
        /// <summary>
        /// Apply default template if required
        /// </summary>
        internal void ApplyDefaultTemplate()
        {
            String defaultTemplateString = "<ControlTemplate TargetType=\"vc:Chart\" xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" "
                + "xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\" "
                + "xmlns:vc=\"clr-namespace:Visifire.Charts;assembly=WPFVisifire.Charts\" >"
                + "<Grid x:Name=\"RootElement\" Background=\"{TemplateBinding Background}\" HorizontalAlignment=\"{TemplateBinding HorizontalAlignment}\" VerticalAlignment=\"{TemplateBinding VerticalAlignment}\" Height=\"{TemplateBinding Height}\" Width=\"{TemplateBinding Width}\" Margin=\"0,0,0,0\"> "
                + "<StackPanel x:Name=\"CenterPanel\" HorizontalAlignment=\"Center\" VerticalAlignment=\"Center\" Height=\"Auto\" Width=\"Auto\" Background=\"Transparent\" ></StackPanel>"
                + "</Grid>   "
                + "</ControlTemplate>";


#if WPF
            Template = (ControlTemplate)XamlReader.Load(new XmlTextReader(new StringReader(defaultTemplateString)));
#else
            Template = System.Windows.Markup.XamlReader.Load(defaultTemplateString) as ControlTemplate;
#endif

            ApplyTemplate();

            IsTemplateApplied = true;
        }

#if WPF
        internal delegate void RenderDelegate();
#endif

        protected bool RENDER_LOCK = false;  
    }
}