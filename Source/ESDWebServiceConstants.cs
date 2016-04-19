using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EcommerceStandardsDocuments;

namespace ESDWebserviceTemplate
{
    /// <summary>Stores a number of constant properties to help with using the ESD webservice</summary>
    public class ESDWebServiceConstants
    {
        public static readonly string ESD_ENDPOINT_FAILURE_JSON = "{\"resultStatus\":\"" + ESDocumentConstants.RESULT_ERROR_UNKNOWN + ", \"message\":\"Unable to obtain data due to a critical error.\"\"}";
        public static readonly string ESD_ENDPOINT_CONTENT_TYPE_JSON = "application/json";
        public static readonly string ESD_ENDPOINT_RESPONSE_COMPRESSION_GZIP = "gzip";
        public static readonly string ESD_VALUE_Y = "Y";
        public static readonly string ESD_VALUE_N = "N";
        public static readonly string ESD_DATA_TYPE_STRING = "STIRNG";
        public static readonly string ESD_DATA_TYPE_NUMBER = "NUMBER";
        public static readonly string ESD_ACCOUNT_ACTION_BLOCK = "BLOCK";
        public static readonly string ESD_ACCOUNT_ACTION_OFF = "OFF";
        public static readonly string ESD_ACCOUNT_ACTION_WARN = "WARN";
        public static readonly string ESD_ACCOUNT_ACTION_WARNCC = "WARNCC";
        public static readonly string ESD_ACCOUNT_TERMS_GND = "GND";
        public static readonly string ESD_ACCOUNT_TERMS_DOM = "DOM";
        public static readonly string ESD_ACCOUNT_TERMS_NDAE = "NDAE";
        public static readonly string ESD_ACCOUNT_TERMS_DMAE = "DMAE";
        public static readonly string ESD_ACCOUNT_TERMS_COD = "COD";
        public static readonly string ESD_ACCOUNT_TERMS_NA = "NA";


        // Identifiers of the ESD webservice end points
        public static readonly int ESD_ENDPOINT_ID_UNKNOWN = 0;
        public static readonly int ESD_ENDPOINT_ID_ALTERNATE_CODES = 1;
        public static readonly int ESD_ENDPOINT_ID_ATTACHMENTS = 2;
        public static readonly int ESD_ENDPOINT_ID_ATTRIBUTES = 3;
        public static readonly int ESD_ENDPOINT_ID_CATEGORIES = 4;
        public static readonly int ESD_ENDPOINT_ID_CUSTOMER_ACCOUNTS = 5;
        public static readonly int ESD_ENDPOINT_ID_CUSTOMER_ACCOUNT_ADDRESSES = 6;
        public static readonly int ESD_ENDPOINT_ID_CUSTOMER_ACCOUNT_CONTRACTS = 7;
        public static readonly int ESD_ENDPOINT_ID_CUSTOMER_ACCOUNT_PAYMENTS = 8;
        public static readonly int ESD_ENDPOINT_ID_CUSTOMER_ACCOUNT_ENQUIRY = 9;
        public static readonly int ESD_ENDPOINT_ID_CUSTOMER_ACCOUNT_ENQUIRY_RECORD = 10;
        public static readonly int ESD_ENDPOINT_ID_CUSTOMER_ACCOUNT_ENQUIRY_LINE_REPORT = 11;
        public static readonly int ESD_ENDPOINT_ID_CUSTOMER_ACCOUNT_STATUS = 12;
        public static readonly int ESD_ENDPOINT_ID_DOWNLOADS = 13;
        public static readonly int ESD_ENDPOINT_ID_FLAGS = 14;
        public static readonly int ESD_ENDPOINT_ID_IMAGES = 15;
        public static readonly int ESD_ENDPOINT_ID_ITEM_GROUPS = 16;
        public static readonly int ESD_ENDPOINT_ID_ITEM_RELATIONS = 17;
        public static readonly int ESD_ENDPOINT_ID_KITS = 18;
        public static readonly int ESD_ENDPOINT_ID_LABOUR = 19;
        public static readonly int ESD_ENDPOINT_ID_LOCATIONS = 20;
        public static readonly int ESD_ENDPOINT_ID_PAYMENT_TYPES = 21;
        public static readonly int ESD_ENDPOINT_ID_PRICE_LEVEL_PRICING = 22;
        public static readonly int ESD_ENDPOINT_ID_PRICE_LEVEL_QUANTITY_PRICING = 23;
        public static readonly int ESD_ENDPOINT_ID_CUSTOMER_ACCOUNT_PRICING = 24;
        public static readonly int ESD_ENDPOINT_ID_CUSTOMER_ACCOUNT_PRICE = 25;
        public static readonly int ESD_ENDPOINT_ID_PRODUCTS = 26;
        public static readonly int ESD_ENDPOINT_ID_PRICE_LEVELS = 27;
        public static readonly int ESD_ENDPOINT_ID_PRODUCT_COMBINATIONS = 28;
        public static readonly int ESD_ENDPOINT_ID_PRODUCT_STOCK_QUANTITIES = 29;
        public static readonly int ESD_ENDPOINT_ID_PURCHASERS = 30;
        public static readonly int ESD_ENDPOINT_ID_SALESREPS = 31;
        public static readonly int ESD_ENDPOINT_ID_SELL_UNITS = 32;
        public static readonly int ESD_ENDPOINT_ID_SUPPLIER_ACCOUNTS = 33;
        public static readonly int ESD_ENDPOINT_ID_SUPPLIER_ACCOUNT_ADDRESSES = 34;
        public static readonly int ESD_ENDPOINT_ID_SURCHARGES = 35;
        public static readonly int ESD_ENDPOINT_ID_TAXCODDES = 36;
        public static readonly int ESD_ENDPOINT_ID_WEB_SERVICE_STATUS = 37;
        public static readonly int ESD_ENDPOINT_ID_ORDER_SALES = 38;
        public static readonly int ESD_ENDPOINT_ID_ORDER_PURCHASES = 39;

        //Dictionary that maps endpoints to the endpoint names
        //Typically language files would be separately created to handle this functionality for each supported language
        public static readonly Dictionary<int, string> LANG_ESD_ENDPOINTS = new Dictionary<int, string>() {
            { ESD_ENDPOINT_ID_UNKNOWN, "Unknown"},
            { ESD_ENDPOINT_ID_ALTERNATE_CODES, "Alternate Codes"},
            { ESD_ENDPOINT_ID_ATTACHMENTS, "Attachments"},
            { ESD_ENDPOINT_ID_ATTRIBUTES, "Attributes"},
            { ESD_ENDPOINT_ID_CATEGORIES, "Categories"},
            { ESD_ENDPOINT_ID_CUSTOMER_ACCOUNTS, "Customer Accounts"},
            { ESD_ENDPOINT_ID_CUSTOMER_ACCOUNT_ADDRESSES, "Customer Account Addresses"},
            { ESD_ENDPOINT_ID_CUSTOMER_ACCOUNT_CONTRACTS, "Customer Account Product Contracts"},
            { ESD_ENDPOINT_ID_CUSTOMER_ACCOUNT_PAYMENTS, "Customer Account Payments"},
            { ESD_ENDPOINT_ID_CUSTOMER_ACCOUNT_ENQUIRY, "Customer Account Enquiry Records"},
            { ESD_ENDPOINT_ID_CUSTOMER_ACCOUNT_ENQUIRY_RECORD, "Customer Account Enquiry Record"},
            { ESD_ENDPOINT_ID_CUSTOMER_ACCOUNT_ENQUIRY_LINE_REPORT, "Customer Account Enquiry Line Report"},
            { ESD_ENDPOINT_ID_CUSTOMER_ACCOUNT_STATUS, "Customer Account Status"},
            { ESD_ENDPOINT_ID_DOWNLOADS, "Downloads"},
            { ESD_ENDPOINT_ID_FLAGS, "Flags"},
            { ESD_ENDPOINT_ID_IMAGES, "Images"},
            { ESD_ENDPOINT_ID_ITEM_GROUPS, "Item Groups"},
            { ESD_ENDPOINT_ID_ITEM_RELATIONS, "Item Relations"},
            { ESD_ENDPOINT_ID_KITS, "Kits"},
            { ESD_ENDPOINT_ID_LABOUR, "Labour"},
            { ESD_ENDPOINT_ID_LOCATIONS, "Locations"},
            { ESD_ENDPOINT_ID_PAYMENT_TYPES, "Payment Types"},
            { ESD_ENDPOINT_ID_PRICE_LEVEL_PRICING, "Price Level Pricing"},
            { ESD_ENDPOINT_ID_PRICE_LEVEL_QUANTITY_PRICING, "Price Level Quantity Pricing"},
            { ESD_ENDPOINT_ID_CUSTOMER_ACCOUNT_PRICING, "Customer Account Pricing"},
            { ESD_ENDPOINT_ID_CUSTOMER_ACCOUNT_PRICE, "Customer Account Price"},
            { ESD_ENDPOINT_ID_PRODUCTS, "Products"},
            { ESD_ENDPOINT_ID_PRICE_LEVELS, "Price Levels"},
            { ESD_ENDPOINT_ID_PRODUCT_COMBINATIONS, "Product Combinations"},
            { ESD_ENDPOINT_ID_PRODUCT_STOCK_QUANTITIES, "Product Stock Quantities"},
            { ESD_ENDPOINT_ID_PURCHASERS, "Purchasers"},
            { ESD_ENDPOINT_ID_SALESREPS, "Salesreps"},
            { ESD_ENDPOINT_ID_SELL_UNITS, "Sell Units"},
            { ESD_ENDPOINT_ID_SUPPLIER_ACCOUNTS, "Supplier Accounts"},
            { ESD_ENDPOINT_ID_SUPPLIER_ACCOUNT_ADDRESSES, "Supplier Account Addresses"},
            { ESD_ENDPOINT_ID_SURCHARGES, "Surcharges"},
            { ESD_ENDPOINT_ID_TAXCODDES, "Taxcodes"},
            { ESD_ENDPOINT_ID_WEB_SERVICE_STATUS, "Web Service Status"},
            { ESD_ENDPOINT_ID_ORDER_SALES, "Sales Orders"},
            { ESD_ENDPOINT_ID_ORDER_PURCHASES, "Purchase Orders"}
        };

        // Dictionary that maps results of ESD webservice processing to meaningful messages
        //Typically language files would be separately created to handle this functionality for each supported language
        public static readonly Dictionary<int, string> LANG_ESD_RESULT_MESSAGES = new Dictionary<int, string>() {
            { ESDocumentConstants.RESULT_ERROR, "An unknown error occurred when trying to obtain %1 data."},
            { ESDocumentConstants.RESULT_ERROR_CONNECTOR_INCORRECT_CONFIGURATION, "An error occurred when trying to obtain %1 data due to a configuration issue."},
            { ESDocumentConstants.RESULT_ERROR_CONNECTOR_INVALID_CREDENTIALS, "An error occurred when trying to obtain %1 data due to invalid credientials being given."},
            { ESDocumentConstants.RESULT_ERROR_CONNECTOR_INVALID_DATA, "An error occurred when trying to obtain %1 data due to invalid data being given."},
            { ESDocumentConstants.RESULT_ERROR_CONNECTOR_PROCESSING, "An error occurred when trying to obtain %1 data due to a processing issue."},
            { ESDocumentConstants.RESULT_ERROR_DATA_SOURCE_CONNECTION_LOST, "An error occurred when trying to obtain %1 data due to a connection to the data no longer existing."},
            { ESDocumentConstants.RESULT_ERROR_DATA_SOURCE_CONNECTION_MISSING, "An error occurred when trying to obtain %1 data due to a connection to the data not existing."},
            { ESDocumentConstants.RESULT_ERROR_DATA_SOURCE_INVALID_CREDENTIALS, "An error occurred when trying to obtain %1 data due to a connection to the data containing invalid credentials."},
            { ESDocumentConstants.RESULT_ERROR_DATA_SOURCE_INVALID_DATA, "An error occurred when trying to obtain %1 data due to invalid data been given to the data source that stores the data."},
            { ESDocumentConstants.RESULT_ERROR_DATA_SOURCE_MAXIMUM_REQUESTS_EXCEEDED, "An error occurred when trying to obtain %1 data due to maximum number of allowed requested being exceeded."},
            { ESDocumentConstants.RESULT_ERROR_DATA_SOURCE_PERMISSION_DENIED, "An error occurred when trying to obtain %1 data since permission to obtain the data from its data source has been denied."},
            { ESDocumentConstants.RESULT_ERROR_DATA_SOURCE_PROCESSING, "An error occurred when trying to obtain %1 data due to a processing issue when trying to obtain it from its data source."},
            { ESDocumentConstants.RESULT_ERROR_UNKNOWN, "An unknown error occurred when trying to obtain %1 data."},
            { ESDocumentConstants.RESULT_SUCCESS, "%1 data has been successfully been obtained."}
        };

        /// <summary>returns a message to place into an Ecommerce Standards Document based on the </summary>
        /// <param name="endpointID"ID of the endpoint></param>
        /// <param name="esdResult">Identifier of the ESD result</param>
        /// <returns>message describing the ESD result for the given endpoint</returns>
        public static string getESDocumentMessage(int endpointID, int esdResult)
        {
            string message = "";
            string endpointName = "";

            //obtain the message based on the ESD Result given
            if (!LANG_ESD_RESULT_MESSAGES.TryGetValue(esdResult, out message)){
                message = LANG_ESD_RESULT_MESSAGES[0];
            }

            //obtain the name of the endpoint
            if (!LANG_ESD_ENDPOINTS.TryGetValue(endpointID, out endpointName))
            {
                endpointName = LANG_ESD_ENDPOINTS[0];
            }

            return message.Replace("%1", endpointName);
        }
    }
}
