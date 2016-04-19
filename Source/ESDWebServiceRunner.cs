using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel.Web;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Channels;
using System.IO;
using System.Xml;

namespace ESDWebserviceTemplate
{
    /// <summary>Starts up and runs the ESD webservice on a configured port as a console application</summary>
    public class ESDWebServiceRunner
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Creating ESD Web Service");

            WebServiceHost webserviceHost = null;

            //Create the webservice host
            try
            {
                //define the URL and port that the webservice will accept HTTP requests from
                var esdWebServiceURL = new Uri(ESDWebServiceSettings.WEBSERVICE_URL + ":" + ESDWebServiceSettings.WEBSERVICE_PORT);

                //define the the controlling class that receive requests from the webservice and do any required processing for returning a response
                var esdWebServiceController = typeof(ESDWebServiceController);

                //create new webservice host
                webserviceHost = new WebServiceHost(esdWebServiceController, esdWebServiceURL);

                //specify the endpoints that that can be called for the webservice and the data handling required
                webserviceHost.AddServiceEndpoint(typeof(IESDWebServiceEndPoints), getWebserviceBinding(), ESDWebServiceSettings.WEBSERVICE_URL_DIRECTORY).Behaviors.Add(new WebHttpBehavior());

                //specify behaviours of the webservice
                webserviceHost.Description.Behaviors.Add(new ServiceMetadataBehavior { HttpGetEnabled = true });

                //ensure that there is a very high limit to the data being returned from the web service host
                ((ServiceBehaviorAttribute)webserviceHost.Description.Behaviors[typeof(ServiceBehaviorAttribute)]).MaxItemsInObjectGraph = 999999999;

                //start up the webservice 
                webserviceHost.Open();

                Console.WriteLine("ESD Web Service initialised and listing for HTTP requests on " + ESDWebServiceSettings.WEBSERVICE_URL + ":" + ESDWebServiceSettings.WEBSERVICE_PORT + "/" + ESDWebServiceSettings.WEBSERVICE_URL_DIRECTORY);
                Console.WriteLine("Press any key to quit");
                Console.ReadLine();
            }
            catch (Exception e)
            {
                webserviceHost = null;
                Console.WriteLine("ESD Web Service could not be started due to an error: " + e.Message);
                Console.WriteLine("Press any key to quit");
                Console.ReadLine();
            }
        }

        /// <summary>Creates a web service REST HTTP binding element that GZIP encodes JSON data coming out of the web service</summary>
        /// <returns>Web HTTP Binding element</returns>
        private static Binding getWebserviceBinding()
        {
            //create web HTTP binding, setting upper limits for data allowed to come in
            //This allows HTTP requests containing data of any size to be accepted by the webservice
            //These values may require settings or adjusted depending on what your software can handle
            WebHttpBinding webHTTPBinding = new WebHttpBinding();
            webHTTPBinding.MaxReceivedMessageSize = int.MaxValue;
            webHTTPBinding.MaxBufferSize = int.MaxValue;
            webHTTPBinding.MaxBufferPoolSize = int.MaxValue;
            webHTTPBinding.ReaderQuotas.MaxArrayLength = int.MaxValue;
            webHTTPBinding.ReaderQuotas.MaxStringContentLength = int.MaxValue;
            webHTTPBinding.ReaderQuotas.MaxBytesPerRead = int.MaxValue;
            webHTTPBinding.ReaderQuotas.MaxDepth = int.MaxValue;
            webHTTPBinding.ReaderQuotas.MaxNameTableCharCount = int.MaxValue;

            //create a new custom binding, based on the web HTTP Binding, that allows responses from the webservice to be GZIP compressed
            //for later versions of DotNET this may not be required since GZIP compression is built in the WCF DotNet libraries. Earlier version were not so fortunate
            CustomBinding customBinding = new CustomBinding(webHTTPBinding);
            for (int i = 0; i < customBinding.Elements.Count; i++)
            {
                if (customBinding.Elements[i] is WebMessageEncodingBindingElement)
                {
                    WebMessageEncodingBindingElement webBE = (WebMessageEncodingBindingElement)customBinding.Elements[i];
                    webBE.ContentTypeMapper = new WebServiceContentTypeMapper();
                    webBE.ReaderQuotas.MaxStringContentLength = int.MaxValue;
                    customBinding.Elements[i] = new GZipMessageEncodingBindingElement(webBE);
                }
                else if (customBinding.Elements[i] is TransportBindingElement)
                {
                    ((TransportBindingElement)customBinding.Elements[i]).MaxReceivedMessageSize = int.MaxValue;
                    ((TransportBindingElement)customBinding.Elements[i]).MaxBufferPoolSize = int.MaxValue;
                }
            }

            return customBinding;
        }
    }

    /// <summary>Class used to declare the incoming web service HTTP requests that will be explicitly handled by the ESD webservice</summary>
    public class WebServiceContentTypeMapper : WebContentTypeMapper
    {
        public override WebContentFormat GetMessageFormatForContentType(string contentType)
        {
            if (contentType == "text/plain")
            {
                return WebContentFormat.Raw;
            }
            else if (contentType.Trim().ToLower().StartsWith("text/xml"))
            {
                return WebContentFormat.Raw;
            }
            else if (contentType.Trim().ToLower().StartsWith("application/xml"))
            {
                return WebContentFormat.Raw;
            }
            else
            {
                //any incomming HTTP requests containing data that is not text file, or an XML file we expect to be handled as JSON data
                return WebContentFormat.Json;
            }
        }
    }
}
