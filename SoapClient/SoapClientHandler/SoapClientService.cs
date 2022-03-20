﻿using System.Net;
using System.Xml;

namespace SoapClient.SoapClientHandler
{
    public class SoapClientService : ISoapClientService
    {
        public  string CallWebService(string _url,string _action,string _xmlString)
        {
            //var _url = "http://xxxxxxxxx/Service1.asmx";
            //var _action = "http://xxxxxxxx/Service1.asmx?op=HelloWorld";

            XmlDocument soapEnvelopeXml = CreateSoapEnvelope(_xmlString);
            HttpWebRequest webRequest = CreateWebRequest(_url, _action);
            InsertSoapEnvelopeIntoWebRequest(soapEnvelopeXml, webRequest);

            // begin async call to web request.
            IAsyncResult asyncResult = webRequest.BeginGetResponse(null, null);

            // suspend this thread until call is complete. You might want to
            // do something usefull here like update your UI.
            asyncResult.AsyncWaitHandle.WaitOne();

            // get the response from the completed web request.
            string soapResult;
            try { 
            using (WebResponse webResponse = webRequest.EndGetResponse(asyncResult))
            {
                using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
                {
                    soapResult = rd.ReadToEnd();
                }
                return soapResult;
            }
            }
            catch(WebException ex)
            {
                using (var stream= new StreamReader(ex.Response.GetResponseStream()))
                {
                   soapResult= stream.ReadToEnd();
                }
                return soapResult;

            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        private static HttpWebRequest CreateWebRequest(string url, string action)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Headers.Add("SOAPAction", action);
            webRequest.ContentType = "text/xml;charset=\"utf-8\"";
            webRequest.Accept = "text/xml";
            webRequest.Method = "POST";
            return webRequest;
        }

        private static XmlDocument CreateSoapEnvelope(string xmlString)
        {
            XmlDocument soapEnvelopeDocument = new XmlDocument();
            //        soapEnvelopeDocument.LoadXml(
            //        @"<SOAP-ENV:Envelope xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/"" 
            //           xmlns:xsi=""http://www.w3.org/1999/XMLSchema-instance"" 
            //           xmlns:xsd=""http://www.w3.org/1999/XMLSchema"">
            //    <SOAP-ENV:Body>
            //        <HelloWorld xmlns=""http://tempuri.org/"" 
            //            SOAP-ENV:encodingStyle=""http://schemas.xmlsoap.org/soap/encoding/"">
            //            <int1 xsi:type=""xsd:integer"">12</int1>
            //            <int2 xsi:type=""xsd:integer"">32</int2>
            //        </HelloWorld>
            //    </SOAP-ENV:Body>
            //</SOAP-ENV:Envelope>");
            soapEnvelopeDocument.LoadXml(xmlString);
            return soapEnvelopeDocument;
        }

        private static void InsertSoapEnvelopeIntoWebRequest(XmlDocument soapEnvelopeXml, HttpWebRequest webRequest)
        {
            using (Stream stream = webRequest.GetRequestStream())
            {
                soapEnvelopeXml.Save(stream);
            }
        }
    }
}
