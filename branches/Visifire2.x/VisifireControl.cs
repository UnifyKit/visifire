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

using System.Windows.Controls.Primitives;
namespace Visifire.Commons
{   
    /// <summary>
    /// Visifire Control interface
    /// </summary>
    public abstract class VisifireControl : VisifireElement
    {
        public VisifireControl()
        {
            ToolTipEnabled = true;
        }

        #region Template Part

        internal Grid _rootElement;

        internal const string RootElementName = "RootElement";

        /// <summary>
        /// Chart Shadow Canvas
        /// </summary>
        internal Grid _shadowGrid;
        internal const string ShadowGridName = "ShadowGrid";

        internal Border _chartBorder;
        internal const string ChartBorderName = "ChartBorder";

        internal Rectangle _chartLightingBorder;
        internal const string ChartLightingBorderName = "ChartLightingBorder";

        internal Canvas _bevelCanvas;
        internal const string BevelCanvasName = "BevelCanvas";

        internal Grid _chartAreaGrid;
        internal const string ChartAreaGridName = "ChartAreaGrid";

        internal StackPanel _topOuterPanel;
        internal const string TopOuterPanelName = "TopOuterPanel";

        internal StackPanel _topOuterTitlePanel;
        internal const string TopOuterTitlePanelName = "TopOuterTitlePanel";

        internal StackPanel _topOuterLegendPanel;
        internal const string TopOuterLegendPanelName = "TopOuterLegendPanel";

        internal StackPanel _bottomOuterPanel;
        internal const string BottomOuterPanelName = "BottomOuterPanel";

        internal StackPanel _bottomOuterLegendPanel;
        internal const string BottomOuterLegendPanelName = "BottomOuterLegendPanel";

        internal StackPanel _bottomOuterTitlePanel;
        internal const string BottomOuterTitlePanelName = "BottomOuterTitlePanel";

        internal StackPanel _leftOuterPanel;
        internal const string LeftOuterPanelName = "LeftOuterPanel";

        internal StackPanel _leftOuterTitlePanel;
        internal const string LeftOuterTitlePanelName = "LeftOuterTitlePanel";

        internal StackPanel _leftOuterLegendPanel;
        internal const string LeftOuterLegendPanelName = "LeftOuterLegendPanel";

        internal StackPanel _rightOuterPanel;
        internal const string RightOuterPanelName = "RightOuterPanel";

        internal StackPanel _rightOuterLegendPanel;
        internal const string RightOuterLegendPanelName = "RightOuterLegendPanel";

        internal StackPanel _rightOuterTitlePanel;
        internal const string RightOuterTitlePanelName = "RightOuterTitlePanel";

        internal Grid _centerOuterGrid;
        internal const string CenterOuterGridName = "CenterOuterGrid";

        internal Grid _centerGrid;
        internal const string CenterGridName = "CenterGrid";

        internal Grid _topOffsetGrid;
        internal const string TopOffsetGridName = "TopOffsetGrid";

        internal Grid _bottomOffsetGrid;
        internal const string BottomOffsetGridName = "BottomOffsetGrid";

        internal Grid _leftOffsetGrid;
        internal const string LeftOffsetGridName = "LeftOffsetGrid";

        internal Grid _rightOffsetGrid;
        internal const string RightOffsetGridName = "RightOffsetGrid";

        internal Grid _topAxisGrid;
        internal const string TopAxisGridName = "TopAxisGrid";

        internal StackPanel _topAxisContainer;
        internal const string TopAxisContainerName = "TopAxisContainer";

        internal StackPanel _topAxisPanel;
        internal const string TopAxisPanelName = "TopAxisPanel";

        internal ScrollBar _topAxisScrollBar;
        internal const string TopAxisScrollBarName = "TopAxisScrollBar";

        internal Grid _leftAxisGrid;
        internal const string LeftAxisGridName = "LeftAxisGrid";

        internal StackPanel _leftAxisContainer;
        internal const string LeftAxisContainerName = "LeftAxisContainer";

        internal StackPanel _leftAxisPanel;
        internal const string LeftAxisPanelName = "LeftAxisPanel";

        internal ScrollBar _leftAxisScrollBar;
        internal const string LeftAxisScrollBarName = "LeftAxisScrollBar";

        internal Grid _rightAxisGrid;
        internal const string RightAxisGridName = "RightAxisGrid";

        internal StackPanel _rightAxisContainer;
        internal const string RightAxisContainerName = "RightAxisContainer";

        internal ScrollBar _rightAxisScrollBar;
        internal const string RightAxisScrollBarName = "RightAxisScrollBar";

        internal StackPanel _rightAxisPanel;
        internal const string RightAxisPanelName = "RightAxisPanel";

        internal Grid _bottomAxisGrid;
        internal const string BottomAxisGridName = "BottomAxisGrid";

        internal StackPanel _bottomAxisContainer;
        internal const string BottomAxisContainerName = "BottomAxisContainer";

        internal ScrollBar _bottomAxisScrollBar;
        internal const string BottomAxisScrollBarName = "BottomAxisScrollBar";

        internal StackPanel _bottomAxisPanel;
        internal const string BottomAxisPanelName = "BottomAxisPanel";

        internal Grid _centerInnerGrid;
        internal const string CenterInnerGridName = "CenterInnerGrid";

        internal Grid _innerGrid;
        internal const string InnerGridName = "InnerGrid";

        internal StackPanel _topInnerPanel;
        internal const string TopInnerPanelName = "TopInnerPanel";

        internal StackPanel _topInnerTitlePanel;
        internal const string TopInnerTitlePanelName = "TopInnerTitlePanel";

        internal StackPanel _topInnerLegendPanel;
        internal const string TopInnerLegendPanelName = "TopInnerLegendPanel";

        internal StackPanel _bottomInnerPanel;
        internal const string BottomInnerPanelName = "BottomInnerPanel";

        internal StackPanel _bottomInnerLegendPanel;
        internal const string BottomInnerLegendPanelName = "BottomInnerLegendPanel";

        internal StackPanel _bottomInnerTitlePanel;
        internal const string BottomInnerTitlePanelName = "BottomInnerTitlePanel";

        /// <summary>
        /// LeftInnerPanel holds LeftInnerTitlePanel and LeftInnerLegendPanel
        /// </summary>
        internal StackPanel _leftInnerPanel;
        internal const string LeftInnerPanelName = "LeftInnerPanel";

        internal StackPanel _leftInnerTitlePanel;
        internal const string LeftInnerTitlePanelName = "LeftInnerTitlePanel";

        internal StackPanel _leftInnerLegendPanel;
        internal const string LeftInnerLegendPanelName = "LeftInnerLegendPanel";

        internal StackPanel _rightInnerPanel;
        internal const string RightInnerPanelName = "RightInnerPanel";

        internal StackPanel _rightInnerLegendPanel;
        internal const string RightInnerLegendPanelName = "RightInnerLegendPanel";

        internal StackPanel _rightInnerTitlePanel;
        internal const string RightInnerTitlePanelName = "RightInnerTitlePanel";

        internal Grid _plotAreaGrid;
        internal const string PlotAreaGridName = "PlotAreaGrid";

        internal ScrollViewer _plotAreaScrollViewer;
        internal const string PlotAreaScrollViewerName = "PlotAreaScrollViewer";

        internal Canvas _plotCanvas;
        internal const string PlotGridName = "PlotCanvas";

        internal Canvas _plotAreaShadowCanvas;
        internal const string PlotAreaShadowCanvasName = "PlotAreaShadowCanvas";

        internal Canvas _drawingCanvas;
        internal const string DrawingCanvasName = "DrawingCanvas";

        internal Canvas _plotAreaBevelCanvas;
        internal const string PlotAreaBevelCanvasName = "PlotAreaBevelCanvas";

        internal StackPanel _centerDockInsidePlotAreaPanel;
        internal const string CenterDockInsidePlotAreaPanelName = "CenterDockInsidePlotAreaPanel";

        internal StackPanel _centerDockOutsidePlotAreaPanel;
        internal const string CenterDockOutsidePlotAreaPanelName = "CenterDockOutsidePlotAreaPanel";

        internal Canvas _toolTipCanvas;
        internal const string ToolTipCanvasName = "ToolTipCanvas";

        internal Border _toolTip;
        internal const string ToolTipName = "ToolTip";

        internal TextBlock _toolTipTextBlock;
        internal const string ToolTipTextBlockName = "ToolTipTextBlock";
        #endregion

       
#if SL
        /// <summary>
        /// Sliverlight Object Id
        /// </summary>
        [System.Windows.Browser.ScriptableMember]
        public String ControlId
        {
            get;
            set;
        }
#endif        
        /// <summary>
        /// Enables or disables the ToolTip
        /// </summary>
#if SL
        [System.Windows.Browser.ScriptableMember]
#endif
        public Boolean ToolTipEnabled
        {
            get
            {
                return (Boolean)GetValue(ToolTipEnabledProperty);
            }
            set
            {
                SetValue(ToolTipEnabledProperty, value);
            }
        }

        public static readonly DependencyProperty ToolTipEnabledProperty = DependencyProperty.Register("ToolTipEnabled", typeof(Boolean), typeof(VisifireControl), null);

        internal Boolean IsTemplateApplied;

        /// <summary>
        /// Accepts absolute or relative Uri, builds and returns abslute path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        internal static String GetAbsolutePath(String path)
        {
#if SL
            String BaseUri = System.Windows.Browser.HtmlPage.Document.DocumentUri.ToString();
            Uri ur = new Uri(path, UriKind.RelativeOrAbsolute);
            if (ur.IsAbsoluteUri)
            {
                return ur.AbsoluteUri;
            }
            else if (path.StartsWith("/"))
            {
                UriBuilder baseUri = new UriBuilder(BaseUri);
                UriBuilder newUri = new UriBuilder(baseUri.Scheme, baseUri.Host, baseUri.Port, path);
                return newUri.ToString();
            }
            else
            {
                UriBuilder baseUri = new UriBuilder(BaseUri);
                String sourcePath = baseUri.Path.Substring(0, baseUri.Path.LastIndexOf('/') + 1);
                UriBuilder newUri = new UriBuilder(baseUri.Scheme, baseUri.Host, baseUri.Port, sourcePath + path);
                return newUri.ToString();
            }
#else
            return path;
#endif
        }


#if WPF
        internal Boolean IsInDesignMode
        {
            get
            {
                return System.ComponentModel.DesignerProperties.GetIsInDesignMode(this);
            }
        }
#else
        internal Boolean IsInDesignMode
        {
            get
            {
                return (!System.Windows.Browser.HtmlPage.IsEnabled);
            }
        }
#endif
        /// <summary>
        /// Is Auto Render is paushed
        /// </summary>
        internal Boolean IsRenderPaused
        {
            get;
            set;
        }
    }

}