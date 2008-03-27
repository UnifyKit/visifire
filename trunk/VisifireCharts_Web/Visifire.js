/*
      Copyright (C) 2008 Webyog Softworks Private Limited

     This file is part of VisifireCharts.
 
     VisifireCharts is a free software: you can redistribute it and/or modify
     it under the terms of the GNU General Public License as published by
     the Free Software Foundation, either version 3 of the License, or
     (at your option) any later version.
 
     VisifireCharts is distributed in the hope that it will be useful,
     but WITHOUT ANY WARRANTY; without even the implied warranty of
     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
     GNU General Public License for more details.
 
     You should have received a copy of the GNU General Public License
     along with VisifireCharts.  If not, see <http://www.gnu.org/licenses/>.
 
*/

if(!window.Visifire)
{
    //  Visifire class
    window.Visifire = function(pXapPath,pWidth,pHeight)
    {
        this.logLevel = 1;                      //  Determines whether to log or not.
        this.xapPath = "Visifire.xap";          // Default is taken as Visifire.xap in the same directory.
        this.targetElement = null;
        this.dataXml = null;                    //  Data xml
        this.dataUri = null;
        
        this.width = null;
        this.height = null;
        
        if(pXapPath)
            this.xapPath = pXapPath;
            
        if(pWidth)
            this.width = pWidth;
            
        if(pHeight)
            this.height = pHeight;
                                            
        this._uThisObject = this;                       // Reference to the Class Instance.
            
        this.index = ++Visifire._slCount;
    }
    window.Visifire._slCount = 0;
    
   Visifire.prototype.setDataXml = function(pDataXml)
    {
        var _uThisObject = this;
        this.dataXml = pDataXml;
    }
    
    Visifire.prototype.setDataUri = function(pDataUri)
    {
        this.dataUri = pDataUri;
    }
    
    Visifire.prototype.setLogLevel = function(level)
    {
        if(level != null)
        {
            this.logLevel = level;
        }
    }
    
    Visifire.prototype.render = function(pTargetElement)
    {
        var _uThisObject = this;
        var width;
        var height;
        
        _uThisObject.targerElement = (typeof(pTargetElement) == "string")?document.getElementById(pTargetElement):pTargetElement;
        
        if(_uThisObject.width != null)
            width = _uThisObject.width;
        else if(_uThisObject.targerElement.offsetWidth != 0)
            width = _uThisObject.targerElement.offsetWidth;
        else
            width = 500;
            
        if(_uThisObject.height != null)
            height = _uThisObject.height;
        else if(_uThisObject.targerElement.offsetHeight != 0)
            height = _uThisObject.targerElement.offsetHeight;
        else
            height = 300;
                
        var html = '<object id="VisifirePlugin' + _uThisObject.index +'" data="data:application/x-silverlight," type="application/x-silverlight-2-b1" width="' + width +'" height="' + height +'">';
        
        html    +=  '<param name="source" value="' + _uThisObject.xapPath +'"/>'
		        +	'<param name="onLoad" value="slLoaded' + _uThisObject.index +'"/>';
		        
		html += '<param name="initParams" value="';
		
		html += "logLevel=" + _uThisObject.logLevel + ",";
		
        if(_uThisObject.dataXml != null)
        {
            window["getDataXml"+_uThisObject.index] = function(sender, args){ 
                                                                            var _uThisObj = _uThisObject;
                                                                            return _uThisObj.dataXml;
                                                                     };
                                                                     
            html +=	'dataXml=getDataXml'+ _uThisObject.index  +',';
        }
        else if(_uThisObject.dataUri != null)
        {
            html +=	'dataUri='+ _uThisObject.dataUri  +',';
        }
        
        html    +=   'width=' + width + ',' + 'height=' + height + '';
        html    += "\"/>";
        html    +=   '<param name="enableHtmlAccess" value="true" />'
		        +   '<param name="background" value="white" />'
		        +   '<a href="http://go.microsoft.com/fwlink/?LinkID=108182" style="text-decoration: none;">'
		        +   '<img src="http://go.microsoft.com/fwlink/?LinkId=108181" alt="Get Microsoft Silverlight" style="border-style: none"/>'
		        +   '<br/>You need Microsoft Silverlight to view Visifire Charts.'
		        +   '<br/> You can install it by clicking on this link.'
		        +   '<br/>Please restart the browser after installation.'
		        +   '</a>'
		        +   '</object>';
		        
		this.targerElement.innerHTML = html;
    }
}