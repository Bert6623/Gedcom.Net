/********************************************************************************
 *  
 *  Product: GSALib
 *  Description: A C# API for accessing the Google Search Appliance.
 *
 *  (c) Copyright 2008 Michael Cizmar + Associates Ltd.  (MC+A)
 *  
********************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Collections;
using GSALib.Constants;
using GSALib.GSA;

namespace GSALib.Utils
{
    /// <summary>
    /// Class provides Methods to Parse Stream to XML supported in GSA
    /// <para>Author Albert Ghukasyan</para>    
    /// </summary>
    internal sealed class XMLParser
    {
        #region Variables

        private Spelling spelling;
        private Suggestion currSuggestion;
        private ParametricNavigationResponse currParamResponse;
        private Response response;
        private Result currResult;
        private KeyMatches currKeyMatch;
        private OneBoxResponse currOneBoxResponse;
        private OneBoxResult currOneBoxResult;
        private StringBuilder contentBuff;
        private ArrayList resultsList;
        private ArrayList keyMatchasList;
        private XMLTags xmlTags;
        private bool inSpelling = false;
        private bool inSynonyms = false;
        private bool inResult = false;
        private bool inResponse = false;
        private bool inParm = false;
        private bool inOneBoxResult = false;
        private bool inOneBoxResponse = false;
        private bool inKeyMatches = false;
        public const String LineEnd = "\n";
        public StringBuilder sb = new StringBuilder();
        private Stack stack = new Stack();
        private string currOneBoxFieldName;

        #endregion

        #region Conctructor

        internal XMLParser()
        {
            response = new Response();
            contentBuff = new StringBuilder();
            resultsList = new ArrayList();
            keyMatchasList = new ArrayList();
            currResult = new Result();
            currKeyMatch = new KeyMatches();
            currOneBoxResponse = new OneBoxResponse();
            currOneBoxResult = new OneBoxResult();
            xmlTags = new XMLTags();
        }

        #endregion

        #region Methods

        public Response getGSAResponse()
        {
            return this.response;
        }

        /// <summary>
        /// Parses Stream to GSA Xml format
        /// </summary>
        /// <param name="stream">Stream from which need to parse</param>
        public void Parse(TextReader stream)
        {
            try
            {
                XmlTextReader reader = new XmlTextReader(stream);
                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.XmlDeclaration:
                            XmlDeclaration(reader.Name, reader.Value);
                            break;
                        case XmlNodeType.Element:
                            string namespaceURI = reader.NamespaceURI;
                            string name = reader.Name;
                            Hashtable attributes = new Hashtable();
                            if (reader.HasAttributes)
                            {
                                for (int i = 0; i < reader.AttributeCount; i++)
                                {
                                    reader.MoveToAttribute(i);
                                    attributes.Add(reader.Name, reader.Value);
                                }
                            }
                            StartElement(namespaceURI, name, name, attributes);
                            break;
                        case XmlNodeType.EndElement:
                            EndElement(reader.NamespaceURI,
                                   reader.Name, reader.Name);
                            break;
                        case XmlNodeType.Text:
                            Characters(reader.Value, 0, reader.Value.Length);
                            break;
                        case XmlNodeType.Whitespace: break;
                        default: break;
                    }
                }
            }
            catch (XmlException e)
            {
                throw e;
            }
        }

        private void XmlDeclaration(string sName, string value)
        {
            Emit(sb.ToString() + "<?" + sName + " " + value + " ?>" + LineEnd);
        }

        private void StartElement(string namespaceURI, string sName, string qName, Hashtable attrs)
        {
            string Attributes = "";
            foreach (object key in attrs.Keys)
            {
                Attributes += " " + key.ToString() + "=\"" + attrs[key].ToString() + "\"";
            }
            stack.Push(new EmitObject(sName, "<" + sName + Attributes + ">", "<" + sName + Attributes + " />"));
            Emit(sb.ToString() + "  <" + sName + Attributes + ">" + LineEnd);
            startElement(namespaceURI, sName, qName, attrs);
        }

        public void startElement(String uri, String localName, String qName, Hashtable attributes)
        {
            int tag = xmlTags.getTag(qName);
            inKeyMatches = inKeyMatches || (tag == XMLTags.GM);
            inResponse = inResponse || (tag == XMLTags.RES); // inside response element
            inResult = inResponse && (inResult || (tag == XMLTags.R)); // inside results element
            inParm = inResponse && (inParm || (tag == XMLTags.PARM));//inside parametric
            inOneBoxResponse = inOneBoxResponse || (tag == XMLTags.OBRES);
            inOneBoxResult = inOneBoxResponse && (inOneBoxResult || (tag == XMLTags.MODULE_RESULT)); // inside oneBox results element
            inSpelling = inSpelling || (tag == XMLTags.Spelling);
            switch (tag)
            {
                case XMLTags.FIELD:
                    if (inOneBoxResult)
                    {
                        currOneBoxFieldName = attributes["name"] == null ? "" : attributes["name"].ToString();
                    }
                    break;
                case XMLTags.OBRES:
                    String moduleName = attributes["module_name"] == null ? null : attributes["module_name"].ToString();
                    currOneBoxResponse.setModuleName(moduleName);
                    break;
                case XMLTags.RES:
                    String startIndex = attributes["SN"] == null ? "1" : attributes["SN"].ToString();
                    String endIndex = attributes["EN"] == null ? "1" : attributes["EN"].ToString();
                    response.setStartIndex(startIndex != null ? long.Parse(startIndex) : 1);
                    response.setEndIndex(endIndex != null ? long.Parse(endIndex) : 1);
                    break;
                case XMLTags.R:
                    String mimeType = attributes["MIME"] == null ? "" : attributes["MIME"].ToString();
                    String indentation = attributes["L"] == null ? "1" : attributes["L"].ToString();
                    currResult.setIndentation(indentation == null ? 1 : Int32.Parse(indentation));
                    currResult.setMimeType(mimeType);
                    break;
                case XMLTags.PARAM:
                    String name = attributes["name"] == null ? "" : attributes["name"].ToString();
                    String value = attributes["value"] == null ? "" : attributes["value"].ToString();
                    response.putParam(name, value);
                    break;
                case XMLTags.PMT:
                    String parmName = attributes["NM"] == null ? "" : attributes["NM"].ToString();
                    String parmDisplay = attributes["DN"] == null ? "" : attributes["DN"].ToString();
                    String parmIsRange = attributes["IR"] == null ? "0" : attributes["IR"].ToString();
                    String parmType = attributes["T"] == null ? "" : attributes["T"].ToString();
                    currParamResponse = new ParametricNavigationResponse();
                    currParamResponse.setName(parmName);
                    currParamResponse.setDisplayName(parmDisplay);
                    if (parmIsRange.Equals("1"))
                    {
                        currParamResponse.setIsRange(true);
                    } else
                    {
                        currParamResponse.setIsRange(false);
                    }
                    currParamResponse.setType(parmType);
                    response.addParametricResponse(currParamResponse);
                    break;
                case XMLTags.PV:
                    String V = attributes["V"] == null ? "" : attributes["V"].ToString();
                    String L = attributes["L"] == null ? "" : attributes["L"].ToString();
                    String H = attributes["H"] == null ? "" : attributes["H"].ToString();
                    int C = attributes["C"] == null ? 0 : Int32.Parse(attributes["C"].ToString());
                    currParamResponse.addResult(new ParametricResult(V,L,H,C));
                    break;
                case XMLTags.C:
                    if (inResult)
                    {
                        String cid = attributes["CID"] == null ? "" : attributes["CID"].ToString();
                        String size = attributes["SZ"] == null ? "" : attributes["SZ"].ToString();
                        String encoding = attributes["ENC"] == null ? "" : attributes["ENC"].ToString();
                        if (null == encoding || "".Equals(encoding.Trim()))
                        {
                            encoding = "UTF-8";
                        }
                        currResult.setCacheDocEncoding(encoding);
                        currResult.setCacheDocId(cid);
                        currResult.setCacheDocSize(size);
                    }
                    break;
                case XMLTags.FS:
                    String fieldName = attributes["NAME"] == null ? "" : attributes["NAME"].ToString();
                    String fieldValue = attributes["VALUE"] == null ? "" : attributes["VALUE"].ToString();
                    currResult.addField(fieldName, fieldValue);
                    break;
                case XMLTags.FI:
                    response.setFiltered(true);
                    break;
                case XMLTags.MT:
                    String metaName = attributes["N"] == null ? "" : attributes["N"].ToString();
                    String metaValue = attributes["V"] == null ? "" : attributes["V"].ToString();
                    currResult.addMeta(metaName, metaValue);
                    break;
                case XMLTags.Spelling:
                    inSpelling = true;
                    spelling = new Spelling();
                    break;
                case XMLTags.Suggestion:
                    if (inSpelling)
                    {
                        currSuggestion = new Suggestion();
                        currSuggestion.setText(attributes["q"] == null ? "" : attributes["q"].ToString());
                    }
                    break;
                case XMLTags.Synonyms:
                    inSynonyms = true;
                    break;
                case XMLTags.GM:
                    inKeyMatches = true;
                    break;
                case XMLTags.OneSynonym:
                    if (inSynonyms)
                    {
                        response.addSynonymWithMarkup(attributes["q"] == null ? "" : attributes["q"].ToString());
                    }
                    break;
                
            }
            clearContent();
        }

        private void EndElement(string namespaceURI, string sName, string qName)
        {
            this.endElement(namespaceURI, sName, qName);
            EmitObject lastPushed = stack.Peek() as EmitObject;
            if (lastPushed == null) return;

            if (lastPushed.Name == sName)
            {
                stack.Pop();
                Emit(sb.ToString() + "</" + sName + ">" + LineEnd);
            }
            else
            {
                lastPushed = stack.Pop() as EmitObject;
                while (lastPushed.Name != sName)
                {
                    sb = sb.Replace(lastPushed.Expression, lastPushed.ReplaceExpression);
                    lastPushed = stack.Pop() as EmitObject;
                }
                Emit(sb.ToString() + "</" + sName + ">" + LineEnd);
            }
        }

        public void endElement(String uri, String localName, String qName)
        {
            int tag = xmlTags.getTag(qName);
            if (inOneBoxResult)
            {
                doOneBoxResult(tag);
            }
            else if (inOneBoxResponse)
            {
                doOneBoxResponse(tag);
            }
            else if (inResult)
            {
                doResult(tag);
            }
            else if (inParm)
            {
                doParm(tag);
            }
            else if (inResponse)
            {
                doResponse(tag);
            }
            else if (inSpelling)
            {
                doSpelling(tag);
            }
            else if (inSynonyms)
            {
                doSynonym(tag);
            }
            else if (inKeyMatches)
            {
                doKeyMatch(tag);
            }
            
            else
            {
                doTopLevel(tag);
            }
        }

        

        private void Characters(string buf, int offset, int len)
        {
            buf = buf.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;");
            if (buf.Length != len)
                len = buf.Length;
            sb.Append(buf + LineEnd);
            this.characters(buf, offset, len);
        }

        public void characters(string ch, int start, int length)
        {
            contentBuff.Append(ch, start, length);
        }

        private void clearContent()
        {
            if (contentBuff.Length > 0) contentBuff.Remove(0, contentBuff.Length);
        }

        private void doResult(int tag)
        {
            switch (tag)
            {
                case XMLTags.R:
                    resultsList.Add(currResult);
                    currResult = new Result();
                    inResult = false;
                    break;
                case XMLTags.U:
                    currResult.setUrl(contentBuff.ToString());
                    break;
                case XMLTags.UE:
                    currResult.setEscapedUrl(contentBuff.ToString());
                    break;
                case XMLTags.T:
                    currResult.setTitle(contentBuff.ToString());
                    break;
                case XMLTags.RK:
                    currResult.setRating(Int32.Parse(contentBuff.ToString()));
                    break;
                case XMLTags.S:
                    currResult.setSummary(contentBuff.ToString());
                    break;
                case XMLTags.LANG:
                    currResult.setLanguage(contentBuff.ToString());
                    break;
                case XMLTags.CRAWLDATE:
                    currResult.setCrawelDate(contentBuff.ToString());
                    break;
            }
        }

        private void doSynonym(int tag)
        {
            inSynonyms = (tag != XMLTags.Synonyms);
        }

        private void doSpelling(int tag)
        {
            switch (tag)
            {
                case XMLTags.Spelling:
                    response.setSpelling(spelling);
                    inSpelling = false;
                    break;
                case XMLTags.Suggestion:
                    currSuggestion.setTextMarkup(contentBuff.ToString());
                    spelling.addSuggestion(currSuggestion);
                    break;
            }
        }

        private void doResponse(int tag)
        {
            switch (tag)
            {
                case XMLTags.RES:
                    response.setResults(resultsList);
                    inResponse = false;
                    break;
                case XMLTags.M:
                    response.setNumResults(long.Parse(contentBuff.ToString()));
                    break;                
                case XMLTags.PU:
                    response.setPreviousResponseUrl(contentBuff.ToString());
                    break;
                case XMLTags.NU:
                    response.setNextResponseUrl(contentBuff.ToString());
                    break;
            }
        }
        private void doParm(int tag)
        {
            switch (tag)
            {
                case XMLTags.PARM:
                    //Todo: Clean any remaining items
                    inParm = false;
                    break;
                case XMLTags.PC:
                    if (contentBuff.ToString().Equals("1"))
                    {
                        response.setIsParametricEstimated(true);
                    }
                    else
                    {
                        response.setIsParametricEstimated(false);
                    }
                    break;
                
                    
            }
        }

        private void doTopLevel(int tag)
        {
            switch (tag)
            {
                case XMLTags.TM:
                    response.setSearchTime(Double.Parse(contentBuff.ToString()));
                    break;
                case XMLTags.Q:
                    response.setQuery(contentBuff.ToString());
                    break;
                case XMLTags.GM:
                    response.setKeyMatches(keyMatchasList);
                    break;
                case XMLTags.CT:
                    response.setSearchComments(contentBuff.ToString());
                    break;

            }
        }

        private void doKeyMatch(int tag)
        {
            switch (tag)
            {
                case XMLTags.GM:
                    keyMatchasList.Add(currKeyMatch);
                    currKeyMatch = new KeyMatches();
                    inKeyMatches = false;
                    response.setKeyMatches(keyMatchasList);
                    break;
                case XMLTags.GL:
                    currKeyMatch.setURL(contentBuff.ToString());
                    break;
                case XMLTags.GD:
                    currKeyMatch.setDescription(contentBuff.ToString());
                    break;
            }
        }

        private void doOneBoxResult(int tag)
        {
            switch (tag)
            {
                case XMLTags.FIELD:
                    currOneBoxResult.addFieldEntry(currOneBoxFieldName, contentBuff.ToString());
                    break;
                case XMLTags.MODULE_RESULT:
                    currOneBoxResponse.addResult(currOneBoxResult);
                    currOneBoxResult = new OneBoxResult();
                    inOneBoxResult = false;
                    break;
                case XMLTags.U:
                    currOneBoxResult.setUrl(contentBuff.ToString());
                    break;
            }
        }

        private void doOneBoxResponse(int tag)
        {
            switch (tag)
            {
                case XMLTags.OBRES:
                    response.addOneBoxResponse(currOneBoxResponse);
                    currOneBoxResponse = new OneBoxResponse();
                    inOneBoxResponse = false;
                    break;
            }
        }

        private void Emit(string s)
        {
            sb.Remove(0, sb.Length);
            sb.Append(s);
        }

        #endregion
    }

    public sealed class EmitObject
    {
        public string Name;
        public string Expression;
        public string ReplaceExpression;

        public EmitObject(string _Name, string _Expression, string _ReplaceExpression)
        {
            this.Name = _Name;
            this.Expression = _Expression;
            this.ReplaceExpression = _ReplaceExpression;
        }
    }
}
