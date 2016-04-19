using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ESDWebserviceTemplate
{
    /// <summary>
    /// Contains all configurable settings for running the ESD webservice. In a production system these settings should be made configurable through a file or database 
    /// </summary>
    public class ESDWebServiceSettings
    {
        //settings to start up webservice 
        public static readonly string WEBSERVICE_PORT = "8081";
        public static readonly string WEBSERVICE_URL = "http://localhost";
        public static readonly string WEBSERVICE_URL_DIRECTORY = "esd";

        //settings to authenticate request to the webserivce
        private static readonly string WEBSERVICE_LOGIN_USER_NAME = "user";
        private static readonly string WEBSERVICE_LOGIN_PASSWORD = "password";
        public static readonly bool WEBSERVICE_REQUIRES_AUTHENTICATION = false; // change to true to test out basic HTTP authentication

        /// <summary>
        /// checks if credentials encoded with basic HTTP authentication match the configured user name and password creditials
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="encodedHTTPRequestCredentials"></param>
        /// <returns></returns>
        public static bool checkRequestCredentials(string encodedHTTPRequestCredentials)
        {
            return (Convert.ToBase64String(Encoding.Default.GetBytes(WEBSERVICE_LOGIN_USER_NAME + ":" + WEBSERVICE_LOGIN_PASSWORD)) == encodedHTTPRequestCredentials);
        }
    }
}
