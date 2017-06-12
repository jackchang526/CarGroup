// JScript 文件

function BAXmlDom()
{
	var m_xmlDom = null;
	this.IsIE = true;
	InitDom();
	
	this.load = function(xmlFile)
	{
		m_xmlDom.load(xmlFile);
	}
		
	this.loadXml = function(xmlStr)
	{
		m_xmlDom.loadXML(xmlStr);
	}
	
	this.selectNodes = function(xpathStr)
	{
		return m_xmlDom.selectNodes(xpathStr);
	}
	
	this.selectSingleNode = function(xpathStr)
	{
		return m_xmlDom.selectSingleNode(xpathStr);
	}
	
	this.getXml = function()
	{
		return m_xmlDom.xml;
	}
	
	
	
	//初始化Dom
	function InitDom()
	{
		if (!window.DOMParser  && window.ActiveXObject)
		{
			var arrXmlDomTypes = ['MSXML2.DOMDocument.6.0','MSXML2.DOMDocument.3.0','Microsoft.XMLDOM'];
			for(var i = 0;i<arrXmlDomTypes.length;i++)
			{
				try
				{
					m_xmlDom = new ActiveXObject(arrXmlDomTypes[i]);
				}
				catch(ex)
				{}//不支持MSXML.XMLDOM对象的IE
			}
		}
		else
		{
			// Mozilla browsers have a DOMParser
			try
			{
				if(m_xmlDom == null && document.implementation && document.implementation.createDocument)
				{
					m_xmlDom = document.implementation.createDocument("","",null);
				}
				this.IsIE = false;
				Node.prototype.__defineGetter__("text",function(){ return this.textContent; });
				XMLDocument.prototype.__defineGetter__("text",function(){ return this.textContent; });
				Node.prototype.__defineGetter__("xml",function(){return new XMLSerializer().serializeToString(this)});
				XMLDocument.prototype.loadXML = function(xmlStr)
				{
					var oParser= new DOMParser();
					var _xmlDom = oParser.parseFromString(xmlStr, "text/xml");

					while(m_xmlDom.firstChild)
					{
						m_xmlDom.removeChild(m_xmlDom.firstChild);
					}
					for(var i=0;i<_xmlDom.childNodes.length;i++)
					{
						var oNewNode = m_xmlDom.importNode(_xmlDom.childNodes[i],true);
						m_xmlDom.appendChild(oNewNode);
					}
				};
				XMLDocument.prototype.selectSingleNode=function(sXPath)
				{
					var oEvaluator = new XPathEvaluator();
					var oResult = oEvaluator.evaluate(sXPath,this,null, XPathResult.FIRST_ORDERED_NODE_TYPE,null);
					if(null != oResult)
					{
						return oResult.singleNodeValue;
					}
					return null;
				};
				Node.prototype.selectSingleNode=function(sXPath)
				{
					var oEvaluator = new XPathEvaluator();
					var oResult = oEvaluator.evaluate(sXPath,this,null, XPathResult.FIRST_ORDERED_NODE_TYPE,null);
					if(null != oResult)
					{
						return oResult.singleNodeValue;
					}
					return null;
				};
				//selectNodes
				XMLDocument.prototype.selectNodes = function(sXPath)
				{
					var oEvaluator = new XPathEvaluator();
					var oResult = oEvaluator.evaluate(sXPath,this,null, XPathResult.ORDERED_NODE_ITERATOR_TYPE,null);
					var aNodes = new Array();
					if(null != oResult)
					{
						var oElement = oResult.iterateNext();
						while(oElement)
						{
							aNodes.push(oElement);
							oElement = oResult.iterateNext();
						}
					}
					return aNodes;
				};
				Node.prototype.selectNodes = function(sXPath)
				{
					var oEvaluator = new XPathEvaluator();
					var oResult = oEvaluator.evaluate(sXPath,this,null, XPathResult.ORDERED_NODE_ITERATOR_TYPE,null);
					var aNodes = new Array();
					if(null != oResult)
					{
						var oElement = oResult.iterateNext();
						while(oElement)
						{
							aNodes.push(oElement);
							oElement = oResult.iterateNext();
						}
					}
					return aNodes;
				}
				//end of selectNodes				
				
			}
			catch (ex)
			{}
		}
		
		m_xmlDom.async=false;
		m_xmlDom.validateOnParse = false;
	}
	
	//end of InitDom
}


