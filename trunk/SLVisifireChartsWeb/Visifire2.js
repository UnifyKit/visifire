/*
Visifire2.js v2.3.6

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

if (!window.Visifire2) {

    /*  Visifire2
        pXapPath     => Location of SL.Visifire.Charts.xap file path.
        pId          => Silverlight object id.
        pWidth       => Width of the chart container.
        pHeight      => Height of the chart container.
        pBackground  => Background of the silverlight object
        pWindowless  => Whether the Silverlight object is windowless
    */
    window.Visifire2 = function(pXapPath, pId, pWidth, pHeight, pBackground, pWindowless) {
        this.id = null;                             // Silverlight object id.
        this.logLevel = 1;                          // Determines whether to log or not.
        this.xapPath = "SL.Visifire.Charts.xap";    // xap file path (default is taken as SL.Visifire.Charts.xap in the same directory).
        this.targetElement = null;                  // Target div element name.
        this.dataXml = null;                        // Chart Xml string.
        this.dataUri = null;                        // Chart xml file uri path.
        this.windowless = null;                     // Windowless property.
        this.width = null;                          // Width of the chart.
        this.height = null;                         // Height of the chart container.
        this.background = null;                     // Background of the chart container.
        this.preLoad = null;                        // Preload event handler.
        this.loaded = null;                         // Loaded event handler.
        this.onError = null;                        // OnError event handler.

        /*  Array of chart references. Visifire Chart object can contain more than one chart.
            Chart reference can be used for updating them at real-time
        */
        this.charts = null;                         
        
        //  pId not present
        if (Number(pId)) {
            if (pHeight)
                this.background = pHeight;

            pHeight = pWidth;
            pWidth = pId;
        }
        else    // pId present
        {
            this.id = pId;

            if (pBackground)
                this.background = pBackground;
        }

        if (pXapPath)
            this.xapPath = pXapPath;

        if (pWidth)
            this.width = pWidth;

        if (pHeight)
            this.height = pHeight;

        if (pBackground)
            this.background = pBackground;

        if (pWindowless)
            this.windowless = pWindowless;

        this.vThisObject = this;

        this.index = ++Visifire2._slCount;
    }

    window.Visifire2._slCount = 0;  // Number of Visifire controls present in the current window.
    
    /*  Set windowless state of silverlight object
        
        pWindowless  => Whether the Silverlight object is windowless
    */
    Visifire2.prototype.setWindowlessState = function(pWindowless) {
        if (pWindowless != null)
            this.windowless = Boolean(pWindowless);
    }

    /*  Set chart data Xml
        
        pDataXml  => Chart data xml as string
    */
    Visifire2.prototype.setDataXml = function(pDataXml) {
        var slControl = this._getSlControl();

        this.dataXml = pDataXml;

        if (slControl != null && this.dataXml != null)
            slControl.Content.wrapper.AddDataXML(pDataXml);
    }
    
    /*  Set data Uri to chart control
        
        pDataUri  => Chart data uri as string
    */
    Visifire2.prototype.setDataUri = function(pDataUri) {
        var slControl = this._getSlControl();

        this.dataUri = pDataUri;
        
        if (slControl != null && this.dataUri != null)
            slControl.Content.wrapper.AddDataUri(pDataUri);
    }
    
    /*  Render the chart
        
        pTargetElement  => Target div element
    */
    Visifire2.prototype.render = function(pTargetElement) {
        var vThisObject = this;            // This Class
        var vSlControl = this._getSlControl();
        
        vThisObject._attachEvents();
        
        if (vSlControl == null)
            this._render(pTargetElement);
        else
            this._reRender(vSlControl);
    }
    
    /*  Set size of the chart control
        
        pWidth   => Width of the chart
        pHeight  => Height of the chart
    */
    Visifire2.prototype.setSize = function(pWidth, pHeight) {
        var slControl = this._getSlControl();

        if (slControl != null) {
            slControl.width = pWidth;
            slControl.height = pHeight;
            slControl.Content.wrapper.Resize(pWidth, pHeight);
        }
        else {
            this.width = pWidth;
            this.height = pHeight;
        }
    }
    
    /*  Set LogLevel of the chart control
        
        level  => loglevel value used to generate a process log depending on logging level.
    */
    Visifire2.prototype.setLogLevel = function(pLevel) {
        if (pLevel != null)
            this.logLevel = pLevel;
    }

    /*  Checks whether the silverlight control is loaded 
    */
    Visifire2.prototype.isLoaded = function() {
        var slControl = this._getSlControl();

        try {
            if (slControl.Content.wrapper != null)
                return true;
        }
        catch (ex) {
            return false;
        }
    }

    /*  Whether the chart data xml is loaded and chart is displayed
    */
    Visifire2.prototype.isDataLoaded = function() {
        var slControl = this._getSlControl();
        return slControl.Content.wrapper.IsDataLoaded;
    }
    
    /*  Attach required events
    */
    Visifire2.prototype._attachEvents = function() {
        var vThisObject = this; // This Class

        window["setVisifireChartsRef" + vThisObject.index] = function(e) {
            vThisObject.charts = e;
        }
        
        if (vThisObject.preLoad != null)
            window["visifireChartPreLoad" + vThisObject.index] = vThisObject.preLoad;

        if (vThisObject.loaded != null)
            window["visifireChartLoaded" + vThisObject.index] = vThisObject.loaded;

        if (vThisObject.onError != null)
            window["visifireChartOnError" + vThisObject.index] = vThisObject.onError;
    }
     
    /*  Returns current silverlight control reference 
    */
    Visifire2.prototype._getSlControl = function() {
        var vThisObject = this; // This Class

        if (vThisObject.id != null) {
            var slControl = document.getElementById(vThisObject.id);
            return slControl;
        }

        return null;
    }
  
    /*  Render the chart
        
        pTargetElement  => Target div element
    */
    Visifire2.prototype._render = function(pTargetElement) {
        var vThisObject = this;            // This Class
        var vWidth;                        // Width of the chart container
        var vHeight;                       // Height of the chart container

        vThisObject.targetElement = (typeof (pTargetElement) == "string") ? document.getElementById(pTargetElement) : pTargetElement;

        vWidth = (vThisObject.width != null) ? vThisObject.width : (vThisObject.targetElement.offsetWidth != 0) ? vThisObject.targetElement.offsetWidth : 500;
        
        vHeight = (vThisObject.height != null) ? vThisObject.height : (vThisObject.targetElement.offsetHeight != 0) ? vThisObject.targetElement.offsetHeight : 300;

        if (!vThisObject.id)
            vThisObject.id = 'VisifireControl' + vThisObject.index;

        var html = '<object id="' + vThisObject.id + '" data="data:application/x-silverlight," type="application/x-silverlight-2" width="' + vWidth + '" height="' + vHeight + '">';

        html += '<param name="source" value="' + vThisObject.xapPath + '"/>'
        html += '<param name="initParams" value="';
        html += "logLevel=" + vThisObject.logLevel + ",";
        html += "controlId=" + vThisObject.id + ",";
        html += "setVisifireChartsRef=setVisifireChartsRef" + vThisObject.index + ",";

        if (vThisObject.preLoad != null)
            html += "onChartPreLoad=visifireChartPreLoad" + vThisObject.index + ",";

        if (vThisObject.loaded != null)
            html += "onChartLoaded=visifireChartLoaded" + vThisObject.index + ",";

        if (vThisObject.dataXml != null) {
            window["getVisifireDataXml" + vThisObject.index] = function(sender, args) {
                var _uThisObj = vThisObject;
                return _uThisObj.dataXml;
            };

            html += 'dataXml=getVisifireDataXml' + vThisObject.index + ',';
        }
        else if (vThisObject.dataUri != null) {
            html += 'dataUri=' + vThisObject.dataUri + ',';
        }
        
        if (vThisObject.background == null)
            vThisObject.background = "White";

        if (vThisObject.windowless == null) {
            if (vThisObject.background == "Transparent" || vThisObject.background == "transparent")
                vThisObject.windowless = true;
            else
                vThisObject.windowless = false;
        }

        html += 'width=' + vWidth + ',' + 'height=' + vHeight + '';
        html += "\"/>";

        if (vThisObject.onError != null)
            html += '<param name="onError" value="visifireChartOnError' + vThisObject.index + '" />'
            
        html += '<param name="enableHtmlAccess" value="true" />'
		        + '<param name="background" value="' + vThisObject.background + '" />'
		        + '<param name="windowless" value="' + vThisObject.windowless + '" />'
		        + '<a href="http://go.microsoft.com/fwlink/?LinkID=149156&v=3.0.40624.0" style="text-decoration: none;">'
		        + '<img src="http://go.microsoft.com/fwlink/?LinkId=108181" alt="Get Microsoft Silverlight" style="border-style: none"/>'
		        + '<br/>You need Microsoft Silverlight to view Visifire Charts.'
		        + '<br/> You can install it by clicking on this link.'
		        + '<br/>Please restart the browser after installation.'
		        + '</a>'
		        + '</object>';
        
        this.targetElement.innerHTML = html;
    }
    
    /*  Re-render the chart

        pSlControl  => Silverlight control reference
    */
    Visifire2.prototype._reRender = function(pSlControl) {
        pSlControl.Content.wrapper.ReRenderChart();
    }
}