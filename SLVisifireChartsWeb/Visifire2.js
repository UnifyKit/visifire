/*
Visifire2.js v2.1.0
    
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
        this.xapPath = "SL.Visifire.Charts.xap";    // xap file path (default is taken as Visifire.xap in the same directory).
        this.targetElement = null;                  // Target div element name.
        this.dataXml = null;                        // Chart Xml string.
        this.dataUri = null;                        // Chart xml file uri path.
        this.windowless = null;                     // Windowless property.
        this.width = null;                          // Width of the chart.
        this.height = null;                         // Height of the chart container.
        this.background = null;                     // Background of the chart container.
        this.preLoad = null;                        // Preload event handler.
        this.loaded = null;                         // Loaded event handler.
        this.charts = null;                         // Chart references (One Visifire Chart object can contain more than one chart.)

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

        this._uThisObject = this;

        this.index = ++Visifire2._slCount;
    }

    window.Visifire2._slCount = 0;              // Number of Visifire controls exists in the current window.

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

        if (slControl != null && this.dataXml != null)
            slControl.Content.wrapper.AddDataXML(pDataXml);

        this.dataXml = pDataXml;
    }

    /*  Set data Uri to chart control

        pDataUri  => Chart data uri as string
    */
    Visifire2.prototype.setDataUri = function(pDataUri) {
        var slControl = this._getSlControl();

        if (slControl != null && this.dataUri != null)
            slControl.Content.wrapper.AddDataUri(pDataUri);

        this.dataUri = pDataUri;
    }

    /*  Render the chart

        pTargetElement  => Target div element
    */
    Visifire2.prototype.render = function(pTargetElement) {
        var slControl = this._getSlControl();

        if (slControl == null) {
            this._render(pTargetElement);
        }
        else {
            this._reRender(slControl);
        }
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
    Visifire2.prototype.setLogLevel = function(level) {
        if (level != null)
            this.logLevel = level;
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

    /*  Whether the chart data xml is loaded and displayed into chart
    */
    Visifire2.prototype.isDataLoaded = function() {
        var slControl = this._getSlControl();

        return slControl.Content.wrapper.IsDataLoaded;
    }

    /*  Returns current silverlight control reference 
    */
    Visifire2.prototype._getSlControl = function() {
        var _uThisObject = this;

        if (_uThisObject.id != null) {
            var slControl = document.getElementById(_uThisObject.id);
            return slControl;
        }

        return null;
    }

    /*  Render the chart
           
    pTargetElement  => Target div element
    */
    Visifire2.prototype._render = function(pTargetElement) {
        var _uThisObject = this;            // This Class
        var width;                          // Width of the chart container
        var height;                         // Height of the chart container

        _uThisObject.targerElement = (typeof (pTargetElement) == "string") ? document.getElementById(pTargetElement) : pTargetElement;

        width = (_uThisObject.width != null) ? _uThisObject.width : (_uThisObject.targerElement.offsetWidth != 0) ? _uThisObject.targerElement.offsetWidth : 500;

        height = (_uThisObject.height != null) ? _uThisObject.height : (_uThisObject.targerElement.offsetHeight != 0) ? _uThisObject.targerElement.offsetHeight : 300;

        if (!_uThisObject.id)
            _uThisObject.id = 'VisifireControl' + _uThisObject.index;

        var html = '<object id="' + _uThisObject.id + '" data="data:application/x-silverlight," type="application/x-silverlight-2" width="' + width + '" height="' + height + '">';

        window["setVisifireChartsRef" + _uThisObject.index] = function(e) {
            _uThisObject.charts = e;
        }

        if (_uThisObject.preLoad != null)
            eval("window.visifireChartPreLoad" + _uThisObject.index + "=" + _uThisObject.preLoad);

        if (_uThisObject.loaded != null)
            eval("window.visifireChartLoaded" + _uThisObject.index + "=" + _uThisObject.loaded);

        html += '<param name="source" value="' + _uThisObject.xapPath + '"/>'
		        + '<param name="onLoad" value="slLoaded' + _uThisObject.index + '"/>'
		        + '<param name="onResize" value="slResized' + _uThisObject.index + '"/>';

        html += '<param name="initParams" value="';

        html += "logLevel=" + _uThisObject.logLevel + ",";

        html += "controlId=" + _uThisObject.id + ",";

        html += "setVisifireChartsRef=setVisifireChartsRef" + _uThisObject.index + ",";

        if (_uThisObject.preLoad != null)
            html += "onChartPreLoad=visifireChartPreLoad" + _uThisObject.index + ",";

        if (_uThisObject.loaded != null)
            html += "onChartLoaded=visifireChartLoaded" + _uThisObject.index + ",";

        if (_uThisObject.dataXml != null) {
            window["getDataXml" + _uThisObject.index] = function(sender, args) {
                var _uThisObj = _uThisObject;
                return _uThisObj.dataXml;
            };

            html += 'dataXml=getDataXml' + _uThisObject.index + ',';
        }
        else if (_uThisObject.dataUri != null) {
            html += 'dataUri=' + _uThisObject.dataUri + ',';
        }

        if (_uThisObject.background == null)
            _uThisObject.background = "White";

        if (_uThisObject.windowless == null) {
            if (_uThisObject.background == "Transparent" || _uThisObject.background == "transparent")
                _uThisObject.windowless = true;
            else
                _uThisObject.windowless = false;
        }

        html += 'width=' + width + ',' + 'height=' + height + '';
        html += "\"/>";
        html += '<param name="enableHtmlAccess" value="true" />'
		        + '<param name="background" value="' + _uThisObject.background + '" />'
		        + '<param name="windowless" value="' + _uThisObject.windowless + '" />'
		        + '<a href="http://go.microsoft.com/fwlink/?LinkID=124807" style="text-decoration: none;">'
		        + '<img src="http://go.microsoft.com/fwlink/?LinkId=108181" alt="Get Microsoft Silverlight" style="border-style: none"/>'
		        + '<br/>You need Microsoft Silverlight to view Visifire Charts.'
		        + '<br/> You can install it by clicking on this link.'
		        + '<br/>Please restart the browser after installation.'
		        + '</a>'
		        + '</object>';

        this.targerElement.innerHTML = html;
    }

    /*  Re-render the chart

        pSlControl  => Silverlight control reference
    */
    Visifire2.prototype._reRender = function(pSlControl) {
        pSlControl.Content.wrapper.ReRenderChart();
    }
}