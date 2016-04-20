using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.IO;
using EcommerceStandardsDocuments;
using System.Net;
using Newtonsoft.Json;
using System.Runtime.Serialization.Json;

namespace ESDWebserviceTemplate
{
    /// <summary>Handles processing requests passed through the ESD webservice and either returning data, or processing data and doing something with it</summary>
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class ESDWebServiceController : IESDWebServiceEndPoints
    {
        /// <summary>Obtains an Ecommerce Standards Document containing an array of alternate code records associated to products, downloads, or labour</summary>
        /// <returns>Serialized Ecommerce Standards Document in the JSON data format</returns>
        public System.ServiceModel.Channels.Message getAlternateCodes()
        {
            ESDWebServiceConstants.outputConsoleMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_ALTERNATE_CODES, ESDWebServiceConstants.ESD_ENDPOINT_METHOD_GET_START, "");

            //create Ecommerce standards document for storing records
            Dictionary<string, string> esDocumentConfigs = new Dictionary<string, string>();
            ESDocumentAlternateCode esDocumentAlternateCodes = new ESDocumentAlternateCode(ESDocumentConstants.RESULT_ERROR_UNKNOWN, ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_ALTERNATE_CODES, ESDocumentConstants.RESULT_ERROR_UNKNOWN), new ESDRecordAlternateCode[]{}, esDocumentConfigs);

            try
            {
                //check that data is allowed to be returned from the web service based on the incomming HTTP request headers
                if (checkRequestCredentials())
                {
                    //obtain record data
                    //This is where you would call a database or other data source to obtain and place data into the standards record objects
                    List<ESDRecordAlternateCode> records = new List<ESDRecordAlternateCode>();

                    //add record data to the document
                    ESDRecordAlternateCode record = new ESDRecordAlternateCode();
                    record.alternateCode = "ALT CODE 1";
                    record.isSupplierCode = ESDWebServiceConstants.ESD_VALUE_Y;
                    record.isUseCode = ESDWebServiceConstants.ESD_VALUE_N;
                    record.keyProductID = "1";
                    records.Add(record);

                    record = new ESDRecordAlternateCode();
                    record.alternateCode = "ALT CODE 2";
                    record.isSupplierCode = ESDWebServiceConstants.ESD_VALUE_Y;
                    record.isUseCode = ESDWebServiceConstants.ESD_VALUE_N;
                    record.keyProductID = "1";
                    records.Add(record);

                    //update the details of the document after all records have sucessfully been obtained
                    esDocumentAlternateCodes.dataRecords = records.ToArray();
                    esDocumentAlternateCodes.totalDataRecords = records.Count;
                    esDocumentAlternateCodes.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_ALTERNATE_CODES, ESDocumentConstants.RESULT_SUCCESS);
                    esDocumentAlternateCodes.resultStatus = ESDocumentConstants.RESULT_SUCCESS;

                    // add a document config that specifies all of the record properties that may contain data in the document
                    // This properties indicate to other systems importing the document on which record data can be inserted or overwritten
                    esDocumentConfigs.Add("dataFields", "alternateCode,isSupplierCode,isUseCode,keyProductID");
                }
                else
                {
                    esDocumentAlternateCodes.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_ALTERNATE_CODES, ESDocumentConstants.RESULT_ERROR_CONNECTOR_INVALID_CREDENTIALS);
                    esDocumentAlternateCodes.resultStatus = ESDocumentConstants.RESULT_ERROR_CONNECTOR_INVALID_CREDENTIALS;
                }
            }
            catch (Exception ex)
            {
                esDocumentAlternateCodes.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_ALTERNATE_CODES, ESDocumentConstants.RESULT_ERROR_CONNECTOR_PROCESSING);
                esDocumentAlternateCodes.resultStatus = ESDocumentConstants.RESULT_ERROR_CONNECTOR_PROCESSING;
            }

            //output message to the console
            ESDWebServiceConstants.outputConsoleMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_ALTERNATE_CODES, ESDWebServiceConstants.ESD_ENDPOINT_METHOD_GET_END, esDocumentAlternateCodes.message);

            //serializes the ESD document into 
            return serializeESDocument(esDocumentAlternateCodes);
        }

        /// <summary>Obtains an Ecommerce Standards Document containing an array of attachment records associated to products, downloads, or labour</summary>
        /// <returns>Serialized Ecommerce Standards Document in the JSON data format</returns>
        public System.ServiceModel.Channels.Message getAttachments()
        {
            ESDWebServiceConstants.outputConsoleMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_ATTACHMENTS, ESDWebServiceConstants.ESD_ENDPOINT_METHOD_GET_START, "");

            //create Ecommerce standards document for storing records
            Dictionary<string, string> esDocumentConfigs = new Dictionary<string, string>();
            ESDocumentAttachment esDocumentAttachments = new ESDocumentAttachment(ESDocumentConstants.RESULT_ERROR_UNKNOWN, ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_ATTACHMENTS, ESDocumentConstants.RESULT_ERROR_UNKNOWN), new ESDRecordAttachment[]{}, esDocumentConfigs);

            try
            {
                //check that data is allowed to be returned from the web service based on the incomming HTTP request headers
                if (checkRequestCredentials())
                {
                    //obtain record data
                    //This is where you would call a database or other data source to obtain and place data into the standards record objects
                    List<ESDRecordAttachment> records = new List<ESDRecordAttachment>();

                    //add record data to the document
                    ESDRecordAttachment record = new ESDRecordAttachment();
                    record.keyAttachmentID = "1";
                    record.keyProductID = "1";
                    record.fileName ="tea_towels";
                    record.fileExtension = "pdf";
                    record.fullFilePath = @"C:\attachments\products\tea_towels.pdf";
                    record.title = "PDF Showing Tea Towel Product";
                    records.Add(record);

                    record = new ESDRecordAttachment();
                    record.keyAttachmentID = "2";
                    record.keyDownloadID = "1";
                    record.fileName = "tv_manual";
                    record.fileExtension = "docx";
                    record.fullFilePath = @"C:\attachments\downloads\manual.docx";
                    record.title = "TV manual";
                    records.Add(record);

                    //update the details of the document after all records have sucessfully been obtained
                    esDocumentAttachments.dataRecords = records.ToArray();
                    esDocumentAttachments.totalDataRecords = records.Count;
                    esDocumentAttachments.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_ATTACHMENTS, ESDocumentConstants.RESULT_SUCCESS);
                    esDocumentAttachments.resultStatus = ESDocumentConstants.RESULT_SUCCESS;

                    // add a document config that specifies all of the record properties that may contain data in the document
                    // This properties indicate to other systems importing the document on which record data can be inserted or overwritten
                    esDocumentConfigs.Add("dataFields", "keyAttachmentID,keyProductID,keyDownloadID,fileName,fileExtension,fullFilePath,title");
                }
                else
                {
                    esDocumentAttachments.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_ATTACHMENTS, ESDocumentConstants.RESULT_ERROR_CONNECTOR_INVALID_CREDENTIALS);
                    esDocumentAttachments.resultStatus = ESDocumentConstants.RESULT_ERROR_CONNECTOR_INVALID_CREDENTIALS;
                }
            }
            catch (Exception ex)
            {
                esDocumentAttachments.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_ATTACHMENTS, ESDocumentConstants.RESULT_ERROR_CONNECTOR_PROCESSING);
                esDocumentAttachments.resultStatus = ESDocumentConstants.RESULT_ERROR_CONNECTOR_PROCESSING;
            }

            //output message to the console
            ESDWebServiceConstants.outputConsoleMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_ATTACHMENTS, ESDWebServiceConstants.ESD_ENDPOINT_METHOD_GET_END, esDocumentAttachments.message);

            //serializes the ESD document into 
            return serializeESDocument(esDocumentAttachments);
        }

        /// <summary>Obtains an Ecommerce Standards Document containing an array of attribute records associated to products, downloads, or labour</summary>
        /// <returns>Serialized Ecommerce Standards Document in the JSON data format</returns>
        public System.ServiceModel.Channels.Message getAttributes()
        {
            ESDWebServiceConstants.outputConsoleMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_ATTRIBUTES, ESDWebServiceConstants.ESD_ENDPOINT_METHOD_GET_START, "");

            //create Ecommerce standards document for storing records
            Dictionary<string, string> esDocumentConfigs = new Dictionary<string, string>();
            ESDocumentAttribute esDocumentAttributes = new ESDocumentAttribute(ESDocumentConstants.RESULT_ERROR_UNKNOWN, ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_ATTRIBUTES, ESDocumentConstants.RESULT_ERROR_UNKNOWN), new ESDRecordAttributeProfile[]{}, new ESDRecordAttributeValue[]{}, esDocumentConfigs);

            try
            {
                //check that data is allowed to be returned from the web service based on the incomming HTTP request headers
                if (checkRequestCredentials())
                {
                    //obtain record data
                    //This is where you would call a database or other data source to obtain and place data into the standards record objects
                    List<ESDRecordAttributeProfile> attributeProfileRecords = new List<ESDRecordAttributeProfile>();
                    ESDRecordAttributeProfile attributeProfileRecord = new ESDRecordAttributeProfile();
                    attributeProfileRecord.keyAttributeProfileID = "1";
                    attributeProfileRecord.name = "Tea Towel Product Attributes";
                    attributeProfileRecord.description = "Attributes Related To Tea Towel Products";
                    attributeProfileRecords.Add(attributeProfileRecord);

                    //create attribute records to associated to the attribute profile
                    List<ESDRecordAttribute> attributeRecords = new List<ESDRecordAttribute>();
                    ESDRecordAttribute attributeRecord = new ESDRecordAttribute();
                    attributeRecord.dataType = ESDWebServiceConstants.ESD_DATA_TYPE_STRING;
                    attributeRecord.keyAttributeID = "1";
                    attributeRecord.name = "color";
                    attributeRecords.Add(attributeRecord);

                    attributeRecord = new ESDRecordAttribute();
                    attributeRecord.dataType = ESDWebServiceConstants.ESD_DATA_TYPE_STRING;
                    attributeRecord.keyAttributeID = "2";
                    attributeRecord.name = "size";
                    attributeRecords.Add(attributeRecord);

                    attributeRecord = new ESDRecordAttribute();
                    attributeRecord.dataType = ESDWebServiceConstants.ESD_DATA_TYPE_STRING;
                    attributeRecord.keyAttributeID = "3";
                    attributeRecord.name = "pattern";
                    attributeRecords.Add(attributeRecord);
                    attributeProfileRecord.attributes = attributeRecords.ToArray();

                    //add record data to the document
                    List<ESDRecordAttributeValue> attributeValues = new List<ESDRecordAttributeValue>();
                    ESDRecordAttributeValue attributeValueRecord = new ESDRecordAttributeValue();
                    attributeValueRecord.keyAttributeID = "1";
                    attributeValueRecord.keyAttributeProfileID = "1";
                    attributeValueRecord.stringValue = "Green";
                    attributeValues.Add(attributeValueRecord);

                    attributeValueRecord = new ESDRecordAttributeValue();
                    attributeValueRecord.keyAttributeID = "1";
                    attributeValueRecord.keyAttributeProfileID = "1";
                    attributeValueRecord.keyProductID = "1";
                    attributeValueRecord.stringValue = "Blue";
                    attributeValues.Add(attributeValueRecord);

                    attributeValueRecord = new ESDRecordAttributeValue();
                    attributeValueRecord.keyAttributeID = "2";
                    attributeValueRecord.keyAttributeProfileID = "1";
                    attributeValueRecord.keyProductID = "1";
                    attributeValueRecord.stringValue = "Large";
                    attributeValues.Add(attributeValueRecord);

                    attributeValueRecord = new ESDRecordAttributeValue();
                    attributeValueRecord.keyAttributeID = "3";
                    attributeValueRecord.keyAttributeProfileID = "1";
                    attributeValueRecord.keyProductID = "1";
                    attributeValueRecord.stringValue = "Checkered";
                    attributeValues.Add(attributeValueRecord);

                    //update the details of the document after all records have sucessfully been obtained
                    esDocumentAttributes.dataRecords = attributeValues.ToArray();
                    esDocumentAttributes.totalDataRecords = attributeValues.Count;
                    esDocumentAttributes.attributeProfiles = attributeProfileRecords.ToArray();
                    esDocumentAttributes.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_ATTRIBUTES, ESDocumentConstants.RESULT_SUCCESS);
                    esDocumentAttributes.resultStatus = ESDocumentConstants.RESULT_SUCCESS;
                }
                else
                {
                    esDocumentAttributes.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_ATTRIBUTES, ESDocumentConstants.RESULT_ERROR_CONNECTOR_INVALID_CREDENTIALS);
                    esDocumentAttributes.resultStatus = ESDocumentConstants.RESULT_ERROR_CONNECTOR_INVALID_CREDENTIALS;
                }
            }
            catch (Exception ex)
            {
                esDocumentAttributes.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_ATTRIBUTES, ESDocumentConstants.RESULT_ERROR_CONNECTOR_PROCESSING);
                esDocumentAttributes.resultStatus = ESDocumentConstants.RESULT_ERROR_CONNECTOR_PROCESSING;
            }

            //output message to the console
            ESDWebServiceConstants.outputConsoleMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_ATTRIBUTES, ESDWebServiceConstants.ESD_ENDPOINT_METHOD_GET_END, esDocumentAttributes.message);

            //serializes the ESD document into 
            return serializeESDocument(esDocumentAttributes);
        }

        /// <summary>Obtains an Ecommerce Standards Document containing an array of categories records and associations to products, downloads, or labour</summary>
        /// <returns>Serialized Ecommerce Standards Document in the JSON data format</returns>
        public System.ServiceModel.Channels.Message getCategories()
        {
            ESDWebServiceConstants.outputConsoleMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_CATEGORIES, ESDWebServiceConstants.ESD_ENDPOINT_METHOD_GET_START, "");

            //create Ecommerce standards document for storing records
            Dictionary<string, string> esDocumentConfigs = new Dictionary<string, string>();
            ESDocumentCategory esDocumentCategories = new ESDocumentCategory(ESDocumentConstants.RESULT_ERROR_UNKNOWN, ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_CATEGORIES, ESDocumentConstants.RESULT_ERROR_UNKNOWN), new ESDRecordCategory[]{}, esDocumentConfigs);

            try
            {
                //check that data is allowed to be returned from the web service based on the incomming HTTP request headers
                if (checkRequestCredentials())
                {
                    //obtain record data
                    //This is where you would call a database or other data source to obtain and place data into the standards record objects
                    List<ESDRecordCategory> records = new List<ESDRecordCategory>();

                    //add record data to the document
                    ESDRecordCategory record = new ESDRecordCategory();
                    record.categoryCode = "ELECTRONICS";
                    record.description1 = "Contains products associated with electronics";
                    record.keyCategoryID = "1";
                    record.metaDescription = "Electronic Goods and Services";
                    record.metaKeywords = "Electronic Goods Services Products Category";
                    record.metaTitle = "Electronics Category";
                    record.name = "Electronics";
                    record.keyCategoryParentID = "";

                    record = new ESDRecordCategory();
                    record.categoryCode = "TELEVISIONS";
                    record.description1 = "Contains televisions and related products";
                    record.keyCategoryID = "2";
                    record.metaDescription = "television products";
                    record.metaKeywords = "Electronic Goods Category Televisions";
                    record.metaTitle = "Televisions Category";
                    record.name = "Televisions";
                    record.keyCategoryParentID = "1";

                    record = new ESDRecordCategory();
                    record.categoryCode = "WIDE SCREEN TELEVISIONS";
                    record.description1 = "Contains wide screen televisions";
                    record.keyCategoryID = "3";
                    record.metaDescription = "wide screen television products";
                    record.metaKeywords = "Electronic Goods Category Televisions Wide Screen";
                    record.metaTitle = "Wide Screen Televisions Category";
                    record.name = "Wide Screen Televisions";
                    record.keyCategoryParentID = "2";
                    record.keyProductIDs = new string[] {"10","11","12"};
                    record.keyDownloadIDs = new string[] { "1"};

                    //update the details of the document after all records have sucessfully been obtained
                    esDocumentCategories.dataRecords = records.ToArray();
                    esDocumentCategories.totalDataRecords = records.Count;
                    esDocumentCategories.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_CATEGORIES, ESDocumentConstants.RESULT_SUCCESS);
                    esDocumentCategories.resultStatus = ESDocumentConstants.RESULT_SUCCESS;

                    // add a document config that specifies all of the record properties that may contain data in the document
                    // This properties indicate to other systems importing the document on which record data can be inserted or overwritten
                    esDocumentConfigs.Add("dataFields", "categoryCode,description1,keyCategoryID,metaDescription,metaKeywords,metaTitle,name,keyCategoryParentID");
                }
                else
                {
                    esDocumentCategories.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_CATEGORIES, ESDocumentConstants.RESULT_ERROR_CONNECTOR_INVALID_CREDENTIALS);
                    esDocumentCategories.resultStatus = ESDocumentConstants.RESULT_ERROR_CONNECTOR_INVALID_CREDENTIALS;
                }
            }
            catch (Exception ex)
            {
                esDocumentCategories.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_CATEGORIES, ESDocumentConstants.RESULT_ERROR_CONNECTOR_PROCESSING);
                esDocumentCategories.resultStatus = ESDocumentConstants.RESULT_ERROR_CONNECTOR_PROCESSING;
            }

            //output message to the console
            ESDWebServiceConstants.outputConsoleMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_CATEGORIES, ESDWebServiceConstants.ESD_ENDPOINT_METHOD_GET_END, esDocumentCategories.message);

            //serializes the ESD document 
            return serializeESDocument(esDocumentCategories);
        }

        /// <summary>Obtains an Ecommerce Standards Document containing an array of customer account records</summary>
        /// <returns>Serialized Ecommerce Standards Document in the JSON data format</returns>
        public System.ServiceModel.Channels.Message getCustomerAccounts()
        {
            ESDWebServiceConstants.outputConsoleMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_CUSTOMER_ACCOUNTS, ESDWebServiceConstants.ESD_ENDPOINT_METHOD_GET_START, "");

            //create Ecommerce standards document for storing records
            Dictionary<string, string> esDocumentConfigs = new Dictionary<string, string>();
            ESDocumentCustomerAccount esDocumentCustomerAccounts = new ESDocumentCustomerAccount(ESDocumentConstants.RESULT_ERROR_UNKNOWN, ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_CUSTOMER_ACCOUNTS, ESDocumentConstants.RESULT_ERROR_UNKNOWN), new ESDRecordCustomerAccount[]{}, esDocumentConfigs);

            try
            {
                //check that data is allowed to be returned from the web service based on the incomming HTTP request headers
                if (checkRequestCredentials())
                {
                    //obtain record data
                    //This is where you would call a database or other data source to obtain and place data into the standards record objects
                    List<ESDRecordCustomerAccount> records = new List<ESDRecordCustomerAccount>();

                    //add record data to the document
                    ESDRecordCustomerAccount record = new ESDRecordCustomerAccount();
                    record.keyCustomerAccountID = "1";
                    record.customerAccountCode = "ACC1234";
                    record.keyLocationIDs = new string[]{"1","3","5"};
                    record.accountName = "ACME Account";
                    record.contactName = "John Smith";
                    record.orgName = "Acme Industries Pty Ltd";
                    record.authorityNumbers = new string[]{"19387574544","3346943828"};
                    record.authorityNumberLabels = new string[]{"Australian Business Number", "Australian Company Number"};
                    record.authorityNumberTypes = new int[] { ESDocumentConstants.AUTHORITY_NUM_AUS_ABN, ESDocumentConstants.AUTHORITY_NUM_AUS_ACN};
                    record.email = "acme@example.com";
                    record.accountClass = "Primary";
                    record.keyPaymentTypeIDs = new string[]{"3","6"};
                    record.keySalesRepID = "2";
                    record.territory = "Metro";
                    record.discount = 0;
                    record.shippingMethod = "Road Freight";
                    record.isOnHold = ESDWebServiceConstants.ESD_VALUE_N;
                    record.isOutsideBalance = ESDWebServiceConstants.ESD_VALUE_N;
                    record.isOutsideTerms = ESDWebServiceConstants.ESD_VALUE_N;
                    record.onHoldAction = ESDWebServiceConstants.ESD_ACCOUNT_ACTION_BLOCK;
                    record.outTermsAction = ESDWebServiceConstants.ESD_ACCOUNT_ACTION_WARN;
                    record.outCreditAction = ESDWebServiceConstants.ESD_ACCOUNT_ACTION_OFF;
                    record.balance = (decimal)-233.00;
                    record.limit = (decimal)-100.00;
                    record.termsType = ESDWebServiceConstants.ESD_ACCOUNT_TERMS_DOM;
                    record.termsDescription = "Pay on the 5th day of the month";
                    record.termsValue1 = "5";
                    record.termsValue2 = "";
                    records.Add(record);

                    //update the details of the document after all records have sucessfully been obtained
                    esDocumentCustomerAccounts.dataRecords = records.ToArray();
                    esDocumentCustomerAccounts.totalDataRecords = records.Count;
                    esDocumentCustomerAccounts.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_CUSTOMER_ACCOUNTS, ESDocumentConstants.RESULT_SUCCESS);
                    esDocumentCustomerAccounts.resultStatus = ESDocumentConstants.RESULT_SUCCESS;

                    // add a document config that specifies all of the record properties that may contain data in the document
                    // This properties indicate to other systems importing the document on which record data can be inserted or overwritten
                    esDocumentConfigs.Add("dataFields","keyCustomerAccountID,customerAccountCode,keyLocationIDs,accountName,contactName,orgName,authorityNumbers,authorityNumberLabels,authorityNumberTypes,email,accountClass,keyPaymentTypeIDs,keySalesRepID,territory,discount,shippingMethod,isOnHold,isOutsideBalance,isOutsideTerms,onHoldAction,outTermsAction,outCreditAction,balance,limit,termsType,termsDescription,termsValue1,termsValue2");
                }
                else
                {
                    esDocumentCustomerAccounts.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_CUSTOMER_ACCOUNTS, ESDocumentConstants.RESULT_ERROR_CONNECTOR_INVALID_CREDENTIALS);
                    esDocumentCustomerAccounts.resultStatus = ESDocumentConstants.RESULT_ERROR_CONNECTOR_INVALID_CREDENTIALS;
                }
            }
            catch (Exception ex)
            {
                esDocumentCustomerAccounts.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_CUSTOMER_ACCOUNTS, ESDocumentConstants.RESULT_ERROR_CONNECTOR_PROCESSING);
                esDocumentCustomerAccounts.resultStatus = ESDocumentConstants.RESULT_ERROR_CONNECTOR_PROCESSING;
            }

            //output message to the console
            ESDWebServiceConstants.outputConsoleMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_CUSTOMER_ACCOUNTS, ESDWebServiceConstants.ESD_ENDPOINT_METHOD_GET_END, esDocumentCustomerAccounts.message);

            //serializes the ESD document 
            return serializeESDocument(esDocumentCustomerAccounts);
        }

        /// <summary>Obtains an Ecommerce Standards Document containing an array of addresses associated to customer accounts</summary>
        /// <returns>Serialized Ecommerce Standards Document in the JSON data format</returns>
        public System.ServiceModel.Channels.Message getCustomerAccountAddresses()
        {
            ESDWebServiceConstants.outputConsoleMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_CUSTOMER_ACCOUNT_ADDRESSES, ESDWebServiceConstants.ESD_ENDPOINT_METHOD_GET_START, "");

            //create Ecommerce standards document for storing records
            Dictionary<string, string> esDocumentConfigs = new Dictionary<string, string>();
            ESDocumentCustomerAccountAddress esDocumentCustomerAccountAddresses = new ESDocumentCustomerAccountAddress(ESDocumentConstants.RESULT_ERROR_UNKNOWN, ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_CUSTOMER_ACCOUNT_ADDRESSES, ESDocumentConstants.RESULT_ERROR_UNKNOWN), new ESDRecordCustomerAccountAddress[] { }, esDocumentConfigs);

            try
            {
                //check that data is allowed to be returned from the web service based on the incomming HTTP request headers
                if (checkRequestCredentials())
                {
                    //obtain record data
                    //This is where you would call a database or other data source to obtain and place data into the standards record objects
                    List<ESDRecordCustomerAccountAddress> records = new List<ESDRecordCustomerAccountAddress>();

                    //add record data to the document
                    ESDRecordCustomerAccountAddress record = new ESDRecordCustomerAccountAddress();
                    record.keyAddressID = "1";
                    record.keyCustomerAccountID = "1";
                    record.description = "Billing Address";
                    record.orgName = "Acme Industries Pty Ltd";
                    record.contact = "George Brown";
                    record.phone = "+6145354545334";
                    record.fax = "+6145354545335";
                    record.address1 = "Unit 1";
                    record.address2 = "24 Example Street";
                    record.address3 = "Melbourne";
                    record.region = "Victoria";
                    record.country = "Australia";
                    record.postcode = "3000";
                    record.freightCode = "FREIG-123";
                    record.isPrimary = ESDWebServiceConstants.ESD_VALUE_Y;
                    record.isDelivery = ESDWebServiceConstants.ESD_VALUE_N;
                    records.Add(record);

                    //update the details of the document after all records have sucessfully been obtained
                    esDocumentCustomerAccountAddresses.dataRecords = records.ToArray();
                    esDocumentCustomerAccountAddresses.totalDataRecords = records.Count;
                    esDocumentCustomerAccountAddresses.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_CUSTOMER_ACCOUNT_ADDRESSES, ESDocumentConstants.RESULT_SUCCESS);
                    esDocumentCustomerAccountAddresses.resultStatus = ESDocumentConstants.RESULT_SUCCESS;

                    // add a document config that specifies all of the record properties that may contain data in the document
                    // This properties indicate to other systems importing the document on which record data can be inserted or overwritten
                    esDocumentConfigs.Add("dataFields", "keyAddressID,keyCustomerAccountID,description,orgName,contact,phone,fax,address1,address2,address3,region,country,postcode,freightCode,isPrimary,isDelivery");
                }
                else
                {
                    esDocumentCustomerAccountAddresses.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_CUSTOMER_ACCOUNT_ADDRESSES, ESDocumentConstants.RESULT_ERROR_CONNECTOR_INVALID_CREDENTIALS);
                    esDocumentCustomerAccountAddresses.resultStatus = ESDocumentConstants.RESULT_ERROR_CONNECTOR_INVALID_CREDENTIALS;
                }
            }
            catch (Exception ex)
            {
                esDocumentCustomerAccountAddresses.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_CUSTOMER_ACCOUNT_ADDRESSES, ESDocumentConstants.RESULT_ERROR_CONNECTOR_PROCESSING);
                esDocumentCustomerAccountAddresses.resultStatus = ESDocumentConstants.RESULT_ERROR_CONNECTOR_PROCESSING;
            }

            //output message to the console
            ESDWebServiceConstants.outputConsoleMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_CUSTOMER_ACCOUNT_ADDRESSES, ESDWebServiceConstants.ESD_ENDPOINT_METHOD_GET_END, esDocumentCustomerAccountAddresses.message);

            //serializes the ESD document into 
            return serializeESDocument(esDocumentCustomerAccountAddresses);
        }

        /// <summary>Obtains an Ecommerce Standards Document containing an array of contract records associated to customer accounts and products, downloads and/or labour</summary>
        /// <returns>Serialized Ecommerce Standards Document in the JSON data format</returns>
        public System.ServiceModel.Channels.Message getCustomerAccountContracts()
        {
            ESDWebServiceConstants.outputConsoleMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_CUSTOMER_ACCOUNT_CONTRACTS, ESDWebServiceConstants.ESD_ENDPOINT_METHOD_GET_START, "");

            //create Ecommerce standards document for storing records
            Dictionary<string, string> esDocumentConfigs = new Dictionary<string, string>();
            ESDocumentCustomerAccountContract esDocumentCustomerAccountContract = new ESDocumentCustomerAccountContract(ESDocumentConstants.RESULT_ERROR_UNKNOWN, ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_CUSTOMER_ACCOUNT_CONTRACTS, ESDocumentConstants.RESULT_ERROR_UNKNOWN), new ESDRecordCustomerAccountContract[]{}, esDocumentConfigs);

            try
            {
                //check that data is allowed to be returned from the web service based on the incomming HTTP request headers
                if (checkRequestCredentials())
                {
                    //obtain record data
                    //This is where you would call a database or other data source to obtain and place data into the standards record objects
                    List<ESDRecordCustomerAccountContract> records = new List<ESDRecordCustomerAccountContract>();

                    //add record data to the document
                    ESDRecordCustomerAccountContract record = new ESDRecordCustomerAccountContract();
                    record.keyContractID = "1";
                    record.contractCode = "2016 Contract Products";
                    record.description = "Contains agreed upon products that will be purchased in 2016";
                    record.forceContractPrice = ESDWebServiceConstants.ESD_VALUE_N;
                    record.type = "PRODUCT";
                    record.expireDate = (long)DateTime.Now.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
                    record.keyAccountIDs = new string[] {"1"};
                    record.keyProductIDs = new string[] { "10","11","12"};
                    records.Add(record);

                    //update the details of the document after all records have sucessfully been obtained
                    esDocumentCustomerAccountContract.dataRecords = records.ToArray();
                    esDocumentCustomerAccountContract.totalDataRecords = records.Count;
                    esDocumentCustomerAccountContract.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_CUSTOMER_ACCOUNT_CONTRACTS, ESDocumentConstants.RESULT_SUCCESS);
                    esDocumentCustomerAccountContract.resultStatus = ESDocumentConstants.RESULT_SUCCESS;

                    // add a document config that specifies all of the record properties that may contain data in the document
                    // This properties indicate to other systems importing the document on which record data can be inserted or overwritten
                    esDocumentConfigs.Add("dataFields", "keyContractID,contractCode,description,forceContractPrice,type,expireDate");
                }
                else
                {
                    esDocumentCustomerAccountContract.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_CUSTOMER_ACCOUNT_CONTRACTS, ESDocumentConstants.RESULT_ERROR_CONNECTOR_INVALID_CREDENTIALS);
                    esDocumentCustomerAccountContract.resultStatus = ESDocumentConstants.RESULT_ERROR_CONNECTOR_INVALID_CREDENTIALS;
                }
            }
            catch (Exception ex)
            {
                esDocumentCustomerAccountContract.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_CUSTOMER_ACCOUNT_CONTRACTS, ESDocumentConstants.RESULT_ERROR_CONNECTOR_PROCESSING);
                esDocumentCustomerAccountContract.resultStatus = ESDocumentConstants.RESULT_ERROR_CONNECTOR_PROCESSING;
            }

            //output message to the console
            ESDWebServiceConstants.outputConsoleMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_CUSTOMER_ACCOUNT_CONTRACTS, ESDWebServiceConstants.ESD_ENDPOINT_METHOD_GET_END, esDocumentCustomerAccountContract.message);

            //serializes the ESD document into 
            return serializeESDocument(esDocumentCustomerAccountContract);
        }

        /// <summary>Obtains an Ecommerce Standards Document containing an array of account enquiry records associated to a customer account and a single record</summary>
        /// <returns>Serialized Ecommerce Standards Document in the JSON data format</returns>
        public System.ServiceModel.Channels.Message getCustomerAccountEnquiryRecord(string keyCustomerAccountID, string recordType, string keyRecordID)
        {
            ESDWebServiceConstants.outputConsoleMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_CUSTOMER_ACCOUNT_ENQUIRY_RECORD, ESDWebServiceConstants.ESD_ENDPOINT_METHOD_GET_START, "");

            //create Ecommerce standards document for storing records
            Dictionary<string, string> esDocumentConfigs = new Dictionary<string, string>();
            ESDocumentCustomerAccountEnquiry esDocumentCustomerAccountEnquiry = new ESDocumentCustomerAccountEnquiry(ESDocumentConstants.RESULT_ERROR_UNKNOWN, ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_CUSTOMER_ACCOUNT_ENQUIRY_RECORD, ESDocumentConstants.RESULT_ERROR_UNKNOWN), esDocumentConfigs);

            try
            {
                //check that data is allowed to be returned from the web service based on the incomming HTTP request headers
                if (checkRequestCredentials())
                {
                    //obtain record data based on the record type being requested
                    //This is where you would call a database or other data source to obtain and place data into the standards record objects
                    if(recordType == ESDocumentCustomerAccountEnquiry.RECORD_TYPE_INVOICE)
                    {
                        List<ESDRecordCustomerAccountEnquiryInvoice> records = new List<ESDRecordCustomerAccountEnquiryInvoice>();

                        if (keyCustomerAccountID == "1" && keyRecordID == "34652")
                        {
                            ESDRecordCustomerAccountEnquiryInvoice record = new ESDRecordCustomerAccountEnquiryInvoice();
                            record.keyInvoiceID = "34652";
                            record.invoiceID = "34652";
                            record.keyCustomerAccountID = "1";
                            record.customerAccountCode = "ACC1234";
                            record.invoiceNumber = "INV-34652";
                            record.creationDate = (long)DateTime.Now.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
                            record.invoiceDate = (long)DateTime.Now.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
                            record.dueDate = (long)DateTime.Now.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
                            record.keyLocationID = "1";
                            record.locationCode = "LOC1";
                            record.locationLabel = "Warehouse Location";
                            record.locationType = ESDocumentConstants.LOCATION_TYPE_WAREHOUSE;
                            record.referenceKeyID = "5646";
                            record.referenceType = ESDocumentCustomerAccountEnquiry.RECORD_TYPE_ORDER_SALE;
                            record.referenceNumber = "SO-5646";
                            record.customerReference = "0405345345";
                            record.salesRepCode = "1";
                            record.salesRepName = "John Smith";
                            record.deliveryAddress1 = "Unit 1";
                            record.deliveryAddress2 = "23 Example Street";
                            record.deliveryAddress3 = "Melbourne";
                            record.deliveryStateProvince = "Victoria";
                            record.deliveryCountry = "Australia";
                            record.billingAddress1 = "Level 5";
                            record.billingAddress2 = "34 Example Street";
                            record.billingAddress3 = "Melbourne";
                            record.billingStateProvince = "Victoria";
                            record.billingCountry = "Australia";
                            record.taxNumber = "GST";
                            record.taxLabel = "Goods and Services Tax";
                            record.taxRate = 10;
                            record.totalExTax = (decimal)110.00;
                            record.totalIncTax = (decimal)121.00;
                            record.totalTax = (decimal)11.00;
                            record.totalFreightIncTax = (decimal)11.00;
                            record.totalFreightExTax = (decimal)10.00;
                            record.totalExtraChargesIncTax = 0;
                            record.totalExtraChargesExTax = 0;
                            record.totalDiscountsIncTax = 0;
                            record.totalDiscountsExTax = 0;
                            record.totalLeviesIncTax = 0;
                            record.totalLeviesExTax = 0;
                            record.totalPaid = (decimal)110.00;
                            record.balance = 0;
                            record.currencyCode = "AUD";
                            record.totalQuantity = 2;
                            record.description = "";
                            record.language = "EN-AU";
                            record.comment = "";

                            //obtain record lines
                            List<ESDRecordCustomerAccountEnquiryInvoiceLine> lineRecords = new List<ESDRecordCustomerAccountEnquiryInvoiceLine>();
                            ESDRecordCustomerAccountEnquiryInvoiceLine lineRecord = new ESDRecordCustomerAccountEnquiryInvoiceLine();
                            lineRecord.lineItemID = "1";
                            lineRecord.lineItemCode = "TEA-TOWELS";
                            lineRecord.lineType = ESDocumentCustomerAccountEnquiry.RECORD_LINE_TYPE_ITEM;
                            lineRecord.description = "Coloured Tea Towels";
                            lineRecord.UNSPSC = "";
                            lineRecord.language = "EN-AU";
                            lineRecord.unit = "EACH";
                            lineRecord.quantityOrdered = 2;
                            lineRecord.quantityDelivered = 2;
                            lineRecord.quantityBackordered = 0;
                            lineRecord.priceExTax = (decimal)50.00;
                            lineRecord.priceIncTax = (decimal)55.00;
                            lineRecord.priceTax = (decimal)5.00;
                            lineRecord.totalPriceExTax = (decimal)100.00;
                            lineRecord.totalPriceIncTax = (decimal)110.00;
                            lineRecord.totalPriceTax = (decimal)10.00;
                            lineRecord.taxCode = "GST";
                            lineRecord.keyLocationID = "";
                            lineRecord.locationCode = "";
                            lineRecord.currencyCode = "AUD";
                            lineRecord.referenceLineItemCode = "";
                            lineRecord.referenceLineCode = "";
                            lineRecords.Add(lineRecord);
                            record.lines = lineRecords.ToArray();
                            records.Add(record);
                        }

                        esDocumentCustomerAccountEnquiry.invoiceRecords = records.ToArray();
                        esDocumentCustomerAccountEnquiry.totalDataRecords = records.Count;

                        // add a document config that specifies all of the record properties that may contain data in the document
                        // This properties indicate to other systems importing the document on which record data can be inserted or overwritten
                        esDocumentConfigs.Add("dataFields", "alternateCode,isSupplierCode,isUseCode,keyProductID");
                    }
                    else if (recordType == ESDocumentCustomerAccountEnquiry.RECORD_TYPE_BACKORDER)
                    {
                        List<ESDRecordCustomerAccountEnquiryBackOrder> records = new List<ESDRecordCustomerAccountEnquiryBackOrder>();
                        ESDRecordCustomerAccountEnquiryBackOrder record = new ESDRecordCustomerAccountEnquiryBackOrder();

                        //obtain details of the specific back order record from the relevant data source

                        esDocumentCustomerAccountEnquiry.backOrderRecords = records.ToArray();
                        esDocumentCustomerAccountEnquiry.totalDataRecords = records.Count;
                    }
                    else if (recordType == ESDocumentCustomerAccountEnquiry.RECORD_TYPE_ORDER_SALE)
                    {
                        List<ESDRecordCustomerAccountEnquiryOrderSale> records = new List<ESDRecordCustomerAccountEnquiryOrderSale>();
                        ESDRecordCustomerAccountEnquiryOrderSale record = new ESDRecordCustomerAccountEnquiryOrderSale();

                        //obtain details of the specific sales order record from the relevant data source

                        esDocumentCustomerAccountEnquiry.orderSaleRecords = records.ToArray();
                        esDocumentCustomerAccountEnquiry.totalDataRecords = records.Count;
                    }
                    else if (recordType == ESDocumentCustomerAccountEnquiry.RECORD_TYPE_BACKORDER)
                    {
                        List<ESDRecordCustomerAccountEnquiryBackOrder> records = new List<ESDRecordCustomerAccountEnquiryBackOrder>();
                        ESDRecordCustomerAccountEnquiryBackOrder record = new ESDRecordCustomerAccountEnquiryBackOrder();

                        //obtain details of the specific back order record from the relevant data source

                        esDocumentCustomerAccountEnquiry.backOrderRecords = records.ToArray();
                        esDocumentCustomerAccountEnquiry.totalDataRecords = records.Count;
                    }
                    else if (recordType == ESDocumentCustomerAccountEnquiry.RECORD_TYPE_PAYMENT)
                    {
                        List<ESDRecordCustomerAccountEnquiryPayment> records = new List<ESDRecordCustomerAccountEnquiryPayment>();
                        ESDRecordCustomerAccountEnquiryPayment record = new ESDRecordCustomerAccountEnquiryPayment();

                        //obtain details of the specific payment record from the relevant data source

                        esDocumentCustomerAccountEnquiry.paymentRecords = records.ToArray();
                        esDocumentCustomerAccountEnquiry.totalDataRecords = records.Count;
                    }
                    else if (recordType == ESDocumentCustomerAccountEnquiry.RECORD_TYPE_CREDIT)
                    {
                        List<ESDRecordCustomerAccountEnquiryCredit> records = new List<ESDRecordCustomerAccountEnquiryCredit>();
                        ESDRecordCustomerAccountEnquiryCredit record = new ESDRecordCustomerAccountEnquiryCredit();

                        //obtain details of the specific credit record from the relevant data source

                        esDocumentCustomerAccountEnquiry.creditRecords = records.ToArray();
                        esDocumentCustomerAccountEnquiry.totalDataRecords = records.Count;
                    }
                    
                    //update the details of the document after all records have sucessfully been obtained
                    esDocumentCustomerAccountEnquiry.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_CUSTOMER_ACCOUNT_ENQUIRY_RECORD, ESDocumentConstants.RESULT_SUCCESS);
                    esDocumentCustomerAccountEnquiry.resultStatus = ESDocumentConstants.RESULT_SUCCESS;
                }
                else
                {
                    esDocumentCustomerAccountEnquiry.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_CUSTOMER_ACCOUNT_ENQUIRY_RECORD, ESDocumentConstants.RESULT_ERROR_CONNECTOR_INVALID_CREDENTIALS);
                    esDocumentCustomerAccountEnquiry.resultStatus = ESDocumentConstants.RESULT_ERROR_CONNECTOR_INVALID_CREDENTIALS;
                }
            }
            catch (Exception ex)
            {
                esDocumentCustomerAccountEnquiry.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_CUSTOMER_ACCOUNT_ENQUIRY_RECORD, ESDocumentConstants.RESULT_ERROR_CONNECTOR_PROCESSING);
                esDocumentCustomerAccountEnquiry.resultStatus = ESDocumentConstants.RESULT_ERROR_CONNECTOR_PROCESSING;
            }

            //output message to the console
            ESDWebServiceConstants.outputConsoleMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_CUSTOMER_ACCOUNT_ENQUIRY_RECORD, ESDWebServiceConstants.ESD_ENDPOINT_METHOD_GET_END, esDocumentCustomerAccountEnquiry.message);

            //serializes the ESD document
            return serializeESDocument(esDocumentCustomerAccountEnquiry);
        }

        /// <summary>Obtains an Ecommerce Standards Document containing an array of account enquiry records associated to a customer account</summary>
        /// <returns>Serialized Ecommerce Standards Document in the JSON data format</returns>
        public System.ServiceModel.Channels.Message getCustomerAccountEnquiry(string keyCustomerAccountID, string recordType, long beginDate, long endDate, int pageNumber, int numberOfRecords, string orderByField, string orderByDirection, string outstandingRecords, string searchString, string keyRecordIDs, string searchType)
        {
            ESDWebServiceConstants.outputConsoleMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_CUSTOMER_ACCOUNT_ENQUIRY, ESDWebServiceConstants.ESD_ENDPOINT_METHOD_GET_START, "");

            //create Ecommerce standards document for storing records
            Dictionary<string, string> esDocumentConfigs = new Dictionary<string, string>();
            ESDocumentCustomerAccountEnquiry esDocumentCustomerAccountEnquiry = new ESDocumentCustomerAccountEnquiry(ESDocumentConstants.RESULT_ERROR_UNKNOWN, ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_CUSTOMER_ACCOUNT_ENQUIRY, ESDocumentConstants.RESULT_ERROR_UNKNOWN), esDocumentConfigs);

            try
            {
                //check that data is allowed to be returned from the web service based on the incomming HTTP request headers
                if (checkRequestCredentials())
                {
                    //obtain record data based on the record type being requested
                    //This is where you would call a database or other data source to obtain and place data into the standards record objects
                    if (recordType == ESDocumentCustomerAccountEnquiry.RECORD_TYPE_TRANSACTION)
                    {
                        List<ESDRecordCustomerAccountEnquiryTransaction> records = new List<ESDRecordCustomerAccountEnquiryTransaction>();
                        ESDRecordCustomerAccountEnquiryTransaction record = new ESDRecordCustomerAccountEnquiryTransaction();
                        esDocumentCustomerAccountEnquiry.transactionRecords = records.ToArray();
                        esDocumentCustomerAccountEnquiry.totalDataRecords = records.Count;

                    }
                    else if (recordType == ESDocumentCustomerAccountEnquiry.RECORD_TYPE_INVOICE)
                    {
                        List<ESDRecordCustomerAccountEnquiryInvoice> records = new List<ESDRecordCustomerAccountEnquiryInvoice>();

                        //use arguments to filter records based on customer account ID, dates, pagination, and search filters
                        if (keyCustomerAccountID == "1")
                        {
                            ESDRecordCustomerAccountEnquiryInvoice record = new ESDRecordCustomerAccountEnquiryInvoice();
                            record.keyInvoiceID = "34652";
                            record.invoiceID = "34652";
                            record.keyCustomerAccountID = "1";
                            record.customerAccountCode = "ACC1234";
                            record.invoiceNumber = "INV-34652";
                            record.creationDate = (long)DateTime.Now.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
                            record.invoiceDate = (long)DateTime.Now.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
                            record.dueDate = (long)DateTime.Now.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
                            record.keyLocationID = "1";
                            record.locationCode = "LOC1";
                            record.locationLabel = "Warehouse Location";
                            record.locationType = ESDocumentConstants.LOCATION_TYPE_WAREHOUSE;
                            record.referenceKeyID = "5646";
                            record.referenceType = ESDocumentCustomerAccountEnquiry.RECORD_TYPE_ORDER_SALE;
                            record.referenceNumber = "SO-5646";
                            record.customerReference = "0405345345";
                            record.salesRepCode = "1";
                            record.salesRepName = "John Smith";
                            record.deliveryAddress1 = "Unit 1";
                            record.deliveryAddress2 = "23 Example Street";
                            record.deliveryAddress3 = "Melbourne";
                            record.deliveryStateProvince = "Victoria";
                            record.deliveryCountry = "Australia";
                            record.billingAddress1 = "Level 5";
                            record.billingAddress2 = "34 Example Street";
                            record.billingAddress3 = "Melbourne";
                            record.billingStateProvince = "Victoria";
                            record.billingCountry = "Australia";
                            record.taxNumber = "GST";
                            record.taxLabel = "Goods and Services Tax";
                            record.taxRate = 10;
                            record.totalExTax = (decimal)110.00;
                            record.totalIncTax = (decimal)121.00;
                            record.totalTax = (decimal)11.00;
                            record.totalFreightIncTax = (decimal)11.00;
                            record.totalFreightExTax = (decimal)10.00;
                            record.totalExtraChargesIncTax = 0;
                            record.totalExtraChargesExTax = 0;
                            record.totalDiscountsIncTax = 0;
                            record.totalDiscountsExTax = 0;
                            record.totalLeviesIncTax = 0;
                            record.totalLeviesExTax = 0;
                            record.totalPaid = (decimal)110.00;
                            record.balance = 0;
                            record.currencyCode = "AUD";
                            record.totalQuantity = 2;
                            record.description = "";
                            record.language = "EN-AU";
                            record.comment = "";
                            records.Add(record);
                        }

                        esDocumentCustomerAccountEnquiry.invoiceRecords = records.ToArray();
                        esDocumentCustomerAccountEnquiry.totalDataRecords = records.Count;

                        // add a document config that specifies all of the record properties that may contain data in the document
                        // This properties indicate to other systems importing the document on which record data can be inserted or overwritten
                        esDocumentConfigs.Add("dataFields", "keyInvoiceID,invoiceID,keyCustomerAccountID,customerAccountCode,invoiceNumber,creationDate,invoiceDate,dueDate,keyLocationID,locationCode,locationLabel,locationType,referenceKeyID,referenceType,referenceNumber,customerReference,salesRepCode,salesRepName,deliveryAddress1,deliveryAddress2,deliveryAddress3,deliveryStateProvince,deliveryCountry,billingAddress1,billingAddress2,billingAddress3,billingStateProvince,billingCountry,taxNumber,taxLabel,taxRate,totalExTax,totalIncTax,totalTax,totalFreightIncTax,totalFreightExTax,totalExtraChargesIncTax,totalExtraChargesExTax,totalDiscountsIncTax,totalDiscountsExTax,totalLeviesIncTax,totalLeviesExTax,totalPaid,balance,currencyCode,totalQuantity,description,language,comment");
                    }
                    else if (recordType == ESDocumentCustomerAccountEnquiry.RECORD_TYPE_BACKORDER)
                    {
                        List<ESDRecordCustomerAccountEnquiryBackOrder> records = new List<ESDRecordCustomerAccountEnquiryBackOrder>();
                        ESDRecordCustomerAccountEnquiryBackOrder record = new ESDRecordCustomerAccountEnquiryBackOrder();

                        //obtain a list of back order records from the relevant data source

                        esDocumentCustomerAccountEnquiry.backOrderRecords = records.ToArray();
                        esDocumentCustomerAccountEnquiry.totalDataRecords = records.Count;
                    }
                    else if (recordType == ESDocumentCustomerAccountEnquiry.RECORD_TYPE_ORDER_SALE)
                    {
                        List<ESDRecordCustomerAccountEnquiryOrderSale> records = new List<ESDRecordCustomerAccountEnquiryOrderSale>();
                        ESDRecordCustomerAccountEnquiryOrderSale record = new ESDRecordCustomerAccountEnquiryOrderSale();

                        //obtain a list of sales order records from the relevant data source

                        esDocumentCustomerAccountEnquiry.orderSaleRecords = records.ToArray();
                        esDocumentCustomerAccountEnquiry.totalDataRecords = records.Count;
                    }
                    else if (recordType == ESDocumentCustomerAccountEnquiry.RECORD_TYPE_BACKORDER)
                    {
                        List<ESDRecordCustomerAccountEnquiryBackOrder> records = new List<ESDRecordCustomerAccountEnquiryBackOrder>();
                        ESDRecordCustomerAccountEnquiryBackOrder record = new ESDRecordCustomerAccountEnquiryBackOrder();

                        //obtain a list of back order records from the relevant data source

                        esDocumentCustomerAccountEnquiry.backOrderRecords = records.ToArray();
                        esDocumentCustomerAccountEnquiry.totalDataRecords = records.Count;
                    }
                    else if (recordType == ESDocumentCustomerAccountEnquiry.RECORD_TYPE_PAYMENT)
                    {
                        List<ESDRecordCustomerAccountEnquiryPayment> records = new List<ESDRecordCustomerAccountEnquiryPayment>();
                        ESDRecordCustomerAccountEnquiryPayment record = new ESDRecordCustomerAccountEnquiryPayment();

                        //obtain a list of credit records from the relevant data source

                        esDocumentCustomerAccountEnquiry.paymentRecords = records.ToArray();
                        esDocumentCustomerAccountEnquiry.totalDataRecords = records.Count;
                    }
                    else if (recordType == ESDocumentCustomerAccountEnquiry.RECORD_TYPE_CREDIT)
                    {
                        List<ESDRecordCustomerAccountEnquiryCredit> records = new List<ESDRecordCustomerAccountEnquiryCredit>();
                        ESDRecordCustomerAccountEnquiryCredit record = new ESDRecordCustomerAccountEnquiryCredit();

                        //obtain a list of credit records from the relevant data source

                        esDocumentCustomerAccountEnquiry.creditRecords = records.ToArray();
                        esDocumentCustomerAccountEnquiry.totalDataRecords = records.Count;
                    }

                    esDocumentCustomerAccountEnquiry.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_CUSTOMER_ACCOUNT_ENQUIRY, ESDocumentConstants.RESULT_SUCCESS);
                    esDocumentCustomerAccountEnquiry.resultStatus = ESDocumentConstants.RESULT_SUCCESS;
                }
                else
                {
                    esDocumentCustomerAccountEnquiry.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_CUSTOMER_ACCOUNT_ENQUIRY, ESDocumentConstants.RESULT_ERROR_CONNECTOR_INVALID_CREDENTIALS);
                    esDocumentCustomerAccountEnquiry.resultStatus = ESDocumentConstants.RESULT_ERROR_CONNECTOR_INVALID_CREDENTIALS;
                }
            }
            catch (Exception ex)
            {
                esDocumentCustomerAccountEnquiry.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_CUSTOMER_ACCOUNT_ENQUIRY, ESDocumentConstants.RESULT_ERROR_CONNECTOR_PROCESSING);
                esDocumentCustomerAccountEnquiry.resultStatus = ESDocumentConstants.RESULT_ERROR_CONNECTOR_PROCESSING;
            }

            //output message to the console
            ESDWebServiceConstants.outputConsoleMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_CUSTOMER_ACCOUNT_ENQUIRY, ESDWebServiceConstants.ESD_ENDPOINT_METHOD_GET_END, esDocumentCustomerAccountEnquiry.message);

            //serializes the ESD document
            return serializeESDocument(esDocumentCustomerAccountEnquiry);
        }

        /// <summary>Obtains an Ecommerce Standards Document containing an array of account enquiry record record lines associated to a customer account and specified report</summary>
        /// <returns>Serialized Ecommerce Standards Document in the JSON data format</returns>
        public System.ServiceModel.Channels.Message getCustomerAccountEnquiryLineReport(string keyCustomerAccountID, string recordType, string reportID, string orderByField, string orderByDirection, int pageNumber, int numberOfRecords)
        {
            ESDWebServiceConstants.outputConsoleMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_CUSTOMER_ACCOUNT_ENQUIRY_LINE_REPORT, ESDWebServiceConstants.ESD_ENDPOINT_METHOD_GET_START, "");

            //create Ecommerce standards document for storing records
            Dictionary<string, string> esDocumentConfigs = new Dictionary<string, string>();
            ESDocumentCustomerAccountEnquiryLine esDocumentCustomerAccountEnquiryLine = new ESDocumentCustomerAccountEnquiryLine(ESDocumentConstants.RESULT_ERROR_UNKNOWN, ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_CUSTOMER_ACCOUNT_ENQUIRY_LINE_REPORT, ESDocumentConstants.RESULT_ERROR_UNKNOWN), esDocumentConfigs);

            try
            {
                //check that data is allowed to be returned from the web service based on the incomming HTTP request headers
                if (checkRequestCredentials())
                {
                    //obtain record data based on the record type being requested
                    //This is where you would call a database or other data source to obtain and place data into the standards record objects
                    if (recordType == ESDocumentCustomerAccountEnquiry.RECORD_TYPE_INVOICE)
                    {
                        List<ESDRecordCustomerAccountEnquiryInvoiceLine> records = new List<ESDRecordCustomerAccountEnquiryInvoiceLine>();

                        //use arguments to filter records based on customer account ID, dates, pagination, and search filters
                        if (reportID == "invoice_lines" && keyCustomerAccountID == "1")
                        {
                            ESDRecordCustomerAccountEnquiryInvoiceLine record = new ESDRecordCustomerAccountEnquiryInvoiceLine();
                            record.lineItemID = "1";
                            record.lineItemCode = "TEA-TOWELS";
                            record.lineType = ESDocumentCustomerAccountEnquiry.RECORD_LINE_TYPE_ITEM;
                            record.description = "Coloured Tea Towels";
                            record.UNSPSC = "";
                            record.language = "EN-AU";
                            record.unit = "EACH";
                            record.quantityOrdered = 2;
                            record.quantityDelivered = 2;
                            record.quantityBackordered = 0;
                            record.priceExTax = (decimal)50.00;
                            record.priceIncTax = (decimal)55.00;
                            record.priceTax = (decimal)5.00;
                            record.totalPriceExTax = (decimal)100.00;
                            record.totalPriceIncTax = (decimal)110.00;
                            record.totalPriceTax = (decimal)10.00;
                            record.taxCode = "GST";
                            record.keyLocationID = "";
                            record.locationCode = "";
                            record.currencyCode = "AUD";
                            record.referenceLineItemCode = "";
                            record.referenceLineCode = "";

                            record = new ESDRecordCustomerAccountEnquiryInvoiceLine();
                            record.lineItemID = "10";
                            record.lineItemCode = "WIDE-TV";
                            record.lineType = ESDocumentCustomerAccountEnquiry.RECORD_LINE_TYPE_ITEM;
                            record.description = "Wide Screen Television";
                            record.UNSPSC = "";
                            record.language = "EN-AU";
                            record.unit = "EACH";
                            record.quantityOrdered = 2;
                            record.quantityDelivered = 2;
                            record.quantityBackordered = 0;
                            record.priceExTax = (decimal)50.00;
                            record.priceIncTax = (decimal)55.00;
                            record.priceTax = (decimal)5.00;
                            record.totalPriceExTax = (decimal)100.00;
                            record.totalPriceIncTax = (decimal)110.00;
                            record.totalPriceTax = (decimal)10.00;
                            record.taxCode = "GST";
                            record.keyLocationID = "";
                            record.locationCode = "";
                            record.currencyCode = "AUD";
                            record.referenceLineItemCode = "";
                            record.referenceLineCode = "";

                            records.Add(record);
                        }

                        esDocumentCustomerAccountEnquiryLine.invoiceLineRecords = records.ToArray();
                        esDocumentCustomerAccountEnquiryLine.totalDataRecords = records.Count;

                        // add a document config that specifies all of the record properties that may contain data in the document
                        // This properties indicate to other systems importing the document on which record data can be inserted or overwritten
                        esDocumentConfigs.Add("dataFields", "lineItemID,lineItemCode,lineType,description,UNSPSC,language,unit,quantityOrdered,quantityDelivered,quantityBackordered,priceExTax,priceIncTax,priceTax,totalPriceExTax,totalPriceIncTax,totalPriceTax,taxCode,keyLocationID,locationCode,currencyCode,referenceLineItemCode,referenceLineCode");
                    }
                    else if (recordType == ESDocumentCustomerAccountEnquiry.RECORD_TYPE_BACKORDER)
                    {
                        List<ESDRecordCustomerAccountEnquiryBackOrderLine> records = new List<ESDRecordCustomerAccountEnquiryBackOrderLine>();
                        ESDRecordCustomerAccountEnquiryBackOrderLine record = new ESDRecordCustomerAccountEnquiryBackOrderLine();

                        //obtain details of the specific back order record from the relevant data source

                        esDocumentCustomerAccountEnquiryLine.backOrderLineRecords = records.ToArray();
                        esDocumentCustomerAccountEnquiryLine.totalDataRecords = records.Count;
                    }
                    else if (recordType == ESDocumentCustomerAccountEnquiry.RECORD_TYPE_ORDER_SALE)
                    {
                        List<ESDRecordCustomerAccountEnquiryOrderSaleLine> records = new List<ESDRecordCustomerAccountEnquiryOrderSaleLine>();
                        ESDRecordCustomerAccountEnquiryOrderSaleLine record = new ESDRecordCustomerAccountEnquiryOrderSaleLine();

                        //obtain details of the specific sales order record from the relevant data source

                        esDocumentCustomerAccountEnquiryLine.orderSaleLineRecords = records.ToArray();
                        esDocumentCustomerAccountEnquiryLine.totalDataRecords = records.Count;
                    }
                    else if (recordType == ESDocumentCustomerAccountEnquiry.RECORD_TYPE_BACKORDER)
                    {
                        List<ESDRecordCustomerAccountEnquiryBackOrderLine> records = new List<ESDRecordCustomerAccountEnquiryBackOrderLine>();
                        ESDRecordCustomerAccountEnquiryBackOrderLine record = new ESDRecordCustomerAccountEnquiryBackOrderLine();

                        //obtain details of the specific back order record from the relevant data source

                        esDocumentCustomerAccountEnquiryLine.backOrderLineRecords = records.ToArray();
                        esDocumentCustomerAccountEnquiryLine.totalDataRecords = records.Count;
                    }
                    else if (recordType == ESDocumentCustomerAccountEnquiry.RECORD_TYPE_PAYMENT)
                    {
                        List<ESDRecordCustomerAccountEnquiryPaymentLine> records = new List<ESDRecordCustomerAccountEnquiryPaymentLine>();
                        ESDRecordCustomerAccountEnquiryPaymentLine record = new ESDRecordCustomerAccountEnquiryPaymentLine();

                        //obtain details of the specific payment record from the relevant data source

                        esDocumentCustomerAccountEnquiryLine.paymentLineRecords = records.ToArray();
                        esDocumentCustomerAccountEnquiryLine.totalDataRecords = records.Count;
                    }
                    else if (recordType == ESDocumentCustomerAccountEnquiry.RECORD_TYPE_CREDIT)
                    {
                        List<ESDRecordCustomerAccountEnquiryCreditLine> records = new List<ESDRecordCustomerAccountEnquiryCreditLine>();
                        ESDRecordCustomerAccountEnquiryCreditLine record = new ESDRecordCustomerAccountEnquiryCreditLine();

                        //obtain details of the specific credit record from the relevant data source

                        esDocumentCustomerAccountEnquiryLine.creditLineRecords = records.ToArray();
                        esDocumentCustomerAccountEnquiryLine.totalDataRecords = records.Count;
                    }

                    esDocumentCustomerAccountEnquiryLine.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_ALTERNATE_CODES, ESDocumentConstants.RESULT_SUCCESS);
                    esDocumentCustomerAccountEnquiryLine.resultStatus = ESDocumentConstants.RESULT_SUCCESS;
                }
                else
                {
                    esDocumentCustomerAccountEnquiryLine.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_CUSTOMER_ACCOUNT_ENQUIRY_LINE_REPORT, ESDocumentConstants.RESULT_ERROR_CONNECTOR_INVALID_CREDENTIALS);
                    esDocumentCustomerAccountEnquiryLine.resultStatus = ESDocumentConstants.RESULT_ERROR_CONNECTOR_INVALID_CREDENTIALS;
                }
            }
            catch (Exception ex)
            {
                esDocumentCustomerAccountEnquiryLine.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_CUSTOMER_ACCOUNT_ENQUIRY_LINE_REPORT, ESDocumentConstants.RESULT_ERROR_CONNECTOR_PROCESSING);
                esDocumentCustomerAccountEnquiryLine.resultStatus = ESDocumentConstants.RESULT_ERROR_CONNECTOR_PROCESSING;
            }

            //output message to the console
            ESDWebServiceConstants.outputConsoleMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_CUSTOMER_ACCOUNT_ENQUIRY_LINE_REPORT, ESDWebServiceConstants.ESD_ENDPOINT_METHOD_GET_END, esDocumentCustomerAccountEnquiryLine.message);

            //serializes the ESD document into 
            return serializeESDocument(esDocumentCustomerAccountEnquiryLine);
        }

        /// <summary>Obtains an Ecommerce Standards Document containing an array of customer account records containing information assciated to a specific account. This is typically used for live querying</summary>
        /// <returns>Serialized Ecommerce Standards Document in the JSON data format</returns>
        public System.ServiceModel.Channels.Message getCustomerAccountStatus(string keyCustomerAccountID, string checkOnHold, string checkBalance, string checkTerms)
        {
            ESDWebServiceConstants.outputConsoleMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_CUSTOMER_ACCOUNT_STATUS, ESDWebServiceConstants.ESD_ENDPOINT_METHOD_GET_START, "");

            //create Ecommerce standards document for storing records
            Dictionary<string, string> esDocumentConfigs = new Dictionary<string, string>();
            ESDocumentCustomerAccount esDocumentCustomerAccounts = new ESDocumentCustomerAccount(ESDocumentConstants.RESULT_ERROR_UNKNOWN, ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_CUSTOMER_ACCOUNT_STATUS, ESDocumentConstants.RESULT_ERROR_UNKNOWN), new ESDRecordCustomerAccount[] { }, esDocumentConfigs);

            try
            {
                //check that data is allowed to be returned from the web service based on the incomming HTTP request headers
                if (checkRequestCredentials())
                {
                    //obtain record data
                    //This is where you would call a database or other data source to obtain and place data into the standards record objects
                    List<ESDRecordCustomerAccount> records = new List<ESDRecordCustomerAccount>();

                    //add record data to the document based on matching the given key customer acocunt ID
                    if (keyCustomerAccountID == "1")
                    {
                        ESDRecordCustomerAccount record = new ESDRecordCustomerAccount();
                        record.keyCustomerAccountID = "1";
                        record.customerAccountCode = "ACC1234";
                        record.keyLocationIDs = new string[] { "1", "3", "5" };
                        record.accountName = "ACME Account";
                        record.contactName = "John Smith";
                        record.orgName = "Acme Industries Pty Ltd";
                        record.authorityNumbers = new string[] { "19387574544", "3346943828" };
                        record.authorityNumberLabels = new string[] { "Australian Business Number", "Australian Company Number" };
                        record.authorityNumberTypes = new int[] { ESDocumentConstants.AUTHORITY_NUM_AUS_ABN, ESDocumentConstants.AUTHORITY_NUM_AUS_ACN };
                        record.email = "acme@example.com";
                        record.accountClass = "Primary";
                        record.keyPaymentTypeIDs = new string[] { "3", "6" };
                        record.keySalesRepID = "2";
                        record.territory = "Metro";
                        record.discount = 0;
                        record.shippingMethod = "Road Freight";
                        if(checkOnHold== ESDWebServiceConstants.ESD_VALUE_Y) { 
                            record.isOnHold = ESDWebServiceConstants.ESD_VALUE_N;
                        }
                        if (checkBalance == ESDWebServiceConstants.ESD_VALUE_Y)
                        {
                            record.isOutsideBalance = ESDWebServiceConstants.ESD_VALUE_N;
                        }
                        if (checkTerms == ESDWebServiceConstants.ESD_VALUE_Y)
                        {
                            record.isOutsideTerms = ESDWebServiceConstants.ESD_VALUE_N;
                        }
                        record.onHoldAction = ESDWebServiceConstants.ESD_ACCOUNT_ACTION_BLOCK;
                        record.outTermsAction = ESDWebServiceConstants.ESD_ACCOUNT_ACTION_WARN;
                        record.outCreditAction = ESDWebServiceConstants.ESD_ACCOUNT_ACTION_OFF;
                        record.balance = (decimal)-233.00;
                        record.limit = (decimal)-100.00;
                        record.termsType = ESDWebServiceConstants.ESD_ACCOUNT_TERMS_DOM;
                        record.termsDescription = "Pay on the 5th day of the month";
                        record.termsValue1 = "5";
                        record.termsValue2 = "";
                        records.Add(record);
                    }

                    //update the details of the document after all records have sucessfully been obtained
                    esDocumentCustomerAccounts.dataRecords = records.ToArray();
                    esDocumentCustomerAccounts.totalDataRecords = records.Count;
                    esDocumentCustomerAccounts.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_CUSTOMER_ACCOUNT_STATUS, ESDocumentConstants.RESULT_SUCCESS);
                    esDocumentCustomerAccounts.resultStatus = ESDocumentConstants.RESULT_SUCCESS;

                    // add a document config that specifies all of the record properties that may contain data in the document
                    // This properties indicate to other systems importing the document on which record data can be inserted or overwritten
                    esDocumentConfigs.Add("dataFields", "keyCustomerAccountID,customerAccountCode,keyLocationIDs,accountName,contactName,orgName,authorityNumbers,authorityNumberLabels,authorityNumberTypes,email,accountClass,keyPaymentTypeIDs,keySalesRepID,territory,discount,shippingMethod,isOnHold,isOutsideBalance,isOutsideTerms,onHoldAction,outTermsAction,outCreditAction,balance,limit,termsType,termsDescription,termsValue1,termsValue2");
                }
                else
                {
                    esDocumentCustomerAccounts.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_CUSTOMER_ACCOUNT_STATUS, ESDocumentConstants.RESULT_ERROR_CONNECTOR_INVALID_CREDENTIALS);
                    esDocumentCustomerAccounts.resultStatus = ESDocumentConstants.RESULT_ERROR_CONNECTOR_INVALID_CREDENTIALS;
                }
            }
            catch (Exception ex)
            {
                esDocumentCustomerAccounts.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_CUSTOMER_ACCOUNT_STATUS, ESDocumentConstants.RESULT_ERROR_CONNECTOR_PROCESSING);
                esDocumentCustomerAccounts.resultStatus = ESDocumentConstants.RESULT_ERROR_CONNECTOR_PROCESSING;
            }

            //output message to the console
            ESDWebServiceConstants.outputConsoleMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_CUSTOMER_ACCOUNT_STATUS, ESDWebServiceConstants.ESD_ENDPOINT_METHOD_GET_END, esDocumentCustomerAccounts.message);

            //serializes the ESD document into 
            return serializeESDocument(esDocumentCustomerAccounts);
        }

        /// <summary>Obtains an Ecommerce Standards Document containing an array of download records</summary>
        /// <returns>Serialized Ecommerce Standards Document in the JSON data format</returns>
        public System.ServiceModel.Channels.Message getDownloads()
        {
            ESDWebServiceConstants.outputConsoleMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_DOWNLOADS, ESDWebServiceConstants.ESD_ENDPOINT_METHOD_GET_START, "");

            //create Ecommerce standards document for storing records
            Dictionary<string, string> esDocumentConfigs = new Dictionary<string, string>();
            ESDocumentDownload esDocumentDownload = new ESDocumentDownload(ESDocumentConstants.RESULT_ERROR_UNKNOWN, ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_DOWNLOADS, ESDocumentConstants.RESULT_ERROR_UNKNOWN), new ESDRecordDownload[]{}, esDocumentConfigs);

            try
            {
                //check that data is allowed to be returned from the web service based on the incomming HTTP request headers
                if (checkRequestCredentials())
                {
                    //obtain record data
                    //This is where you would call a database or other data source to obtain and place data into the standards record objects
                    List<ESDRecordDownload> records = new List<ESDRecordDownload>();
                    

                    //update the details of the document after all records have sucessfully been obtained
                    esDocumentDownload.dataRecords = records.ToArray();
                    esDocumentDownload.totalDataRecords = records.Count;
                    esDocumentDownload.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_DOWNLOADS, ESDocumentConstants.RESULT_SUCCESS);
                    esDocumentDownload.resultStatus = ESDocumentConstants.RESULT_SUCCESS;

                    // add a document config that specifies all of the record properties that may contain data in the document
                    // This properties indicate to other systems importing the document on which record data can be inserted or overwritten
                    esDocumentConfigs.Add("dataFields", "");
                }
                else
                {
                    esDocumentDownload.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_DOWNLOADS, ESDocumentConstants.RESULT_ERROR_CONNECTOR_INVALID_CREDENTIALS);
                    esDocumentDownload.resultStatus = ESDocumentConstants.RESULT_ERROR_CONNECTOR_INVALID_CREDENTIALS;
                }
            }
            catch (Exception ex)
            {
                esDocumentDownload.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_DOWNLOADS, ESDocumentConstants.RESULT_ERROR_CONNECTOR_PROCESSING);
                esDocumentDownload.resultStatus = ESDocumentConstants.RESULT_ERROR_CONNECTOR_PROCESSING;
            }

            //output message to the console
            ESDWebServiceConstants.outputConsoleMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_DOWNLOADS, ESDWebServiceConstants.ESD_ENDPOINT_METHOD_GET_END, esDocumentDownload.message);

            //serializes the ESD document into 
            return serializeESDocument(esDocumentDownload);
        }

        /// <summary>Obtains an Ecommerce Standards Document containing an array of flag records and associations to products, downloads, and/or labour</summary>
        /// <returns>Serialized Ecommerce Standards Document in the JSON data format</returns>
        public System.ServiceModel.Channels.Message getFlags()
        {
            ESDWebServiceConstants.outputConsoleMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_FLAGS, ESDWebServiceConstants.ESD_ENDPOINT_METHOD_GET_START, "");

            //create Ecommerce standards document for storing records
            Dictionary<string, string> esDocumentConfigs = new Dictionary<string, string>();
            ESDocumentFlag esDocumentFlags = new ESDocumentFlag(ESDocumentConstants.RESULT_ERROR_UNKNOWN, ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_FLAGS, ESDocumentConstants.RESULT_ERROR_UNKNOWN), new ESDRecordFlagMapping[]{}, esDocumentConfigs);

            try
            {
                //check that data is allowed to be returned from the web service based on the incomming HTTP request headers
                if (checkRequestCredentials())
                {
                    //obtain record data
                    //This is where you would call a database or other data source to obtain and place data into the standards record objects
                    List<ESDRecordFlagMapping> mappingRecords = new List<ESDRecordFlagMapping>();
                    List<ESDRecordFlag> flagRecords = new List<ESDRecordFlag>();

                    //add flag records
                    ESDRecordFlag flagRecord = new ESDRecordFlag();
                    flagRecord.keyFlagID = "1";
                    flagRecord.flagCode = "1-SPECIAL";
                    flagRecord.flagName = "On Special";
                    flagRecord.description = "Denotes products that are on special";
                    flagRecords.Add(flagRecord);

                    flagRecord = new ESDRecordFlag();
                    flagRecord.keyFlagID = "2";
                    flagRecord.flagCode = "2-MONTHFEAT";
                    flagRecord.flagName = "Featured";
                    flagRecord.description = "Denotes products that being featured for the month";
                    flagRecords.Add(flagRecord);

                    //add flag mapping record data to the document
                    ESDRecordFlagMapping record = new ESDRecordFlagMapping();
                    record.keyFlagID = "1";
                    record.keyProductID = "1";
                    mappingRecords.Add(record);

                    record = new ESDRecordFlagMapping();
                    record.keyFlagID = "2";
                    record.keyProductID = "1";
                    mappingRecords.Add(record);

                    record = new ESDRecordFlagMapping();
                    record.keyFlagID = "2";
                    record.keyDownloadID = "10";
                    mappingRecords.Add(record);

                    record = new ESDRecordFlagMapping();
                    record.keyFlagID = "1";
                    record.keyLabourID = "5";
                    mappingRecords.Add(record);

                    //update the details of the document after all records have sucessfully been obtained
                    esDocumentFlags.flagRecords = flagRecords.ToArray();
                    esDocumentFlags.dataRecords = mappingRecords.ToArray();
                    esDocumentFlags.totalDataRecords = mappingRecords.Count;
                    esDocumentFlags.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_FLAGS, ESDocumentConstants.RESULT_SUCCESS);
                    esDocumentFlags.resultStatus = ESDocumentConstants.RESULT_SUCCESS;

                    // add a document config that specifies all of the record properties that may contain data in the document
                    // This properties indicate to other systems importing the document on which record data can be inserted or overwritten
                    esDocumentConfigs.Add("dataFields", "keyFlagID,keyProductID,keyDownloadID,keyLabourID");
                }
                else
                {
                    esDocumentFlags.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_FLAGS, ESDocumentConstants.RESULT_ERROR_CONNECTOR_INVALID_CREDENTIALS);
                    esDocumentFlags.resultStatus = ESDocumentConstants.RESULT_ERROR_CONNECTOR_INVALID_CREDENTIALS;
                }
            }
            catch (Exception ex)
            {
                esDocumentFlags.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_FLAGS, ESDocumentConstants.RESULT_ERROR_CONNECTOR_PROCESSING);
                esDocumentFlags.resultStatus = ESDocumentConstants.RESULT_ERROR_CONNECTOR_PROCESSING;
            }

            //output message to the console
            ESDWebServiceConstants.outputConsoleMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_FLAGS, ESDWebServiceConstants.ESD_ENDPOINT_METHOD_GET_END, esDocumentFlags.message);

            //serializes the ESD document
            return serializeESDocument(esDocumentFlags);
        }

        /// <summary>Obtains an Ecommerce Standards Document containing an array of image records associated to products, downloads, and/or labour</summary>
        /// <returns>Serialized Ecommerce Standards Document in the JSON data format</returns>
        public System.ServiceModel.Channels.Message getImages()
        {
            ESDWebServiceConstants.outputConsoleMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_IMAGES, ESDWebServiceConstants.ESD_ENDPOINT_METHOD_GET_START, "");

            //create Ecommerce standards document for storing records
            Dictionary<string, string> esDocumentConfigs = new Dictionary<string, string>();
            ESDocumentImage esDocumentImages = new ESDocumentImage(ESDocumentConstants.RESULT_ERROR_UNKNOWN, ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_IMAGES, ESDocumentConstants.RESULT_ERROR_UNKNOWN), new ESDRecordImage[] { }, esDocumentConfigs);

            try
            {
                //check that data is allowed to be returned from the web service based on the incomming HTTP request headers
                if (checkRequestCredentials())
                {
                    //obtain record data
                    //This is where you would call a database or other data source to obtain and place data into the standards record objects
                    List<ESDRecordImage> records = new List<ESDRecordImage>();

                    //add record data to the document
                    ESDRecordImage record = new ESDRecordImage();
                    record.keyImageID = "1";
                    record.keyProductID = "1";
                    record.imageFileName = "tea_towels";
                    record.imageFileExtension = "jpg";
                    record.imageFullFilePath = @"C:\attachments\products\tea_towels.jpg";
                    record.title = "Tea towels picture 1";
                    record.description = "Image Showing Tea Towel Product";
                    records.Add(record);

                    record = new ESDRecordImage();
                    record.keyImageID = "2";
                    record.keyDownloadID = "1";
                    record.imageFileName = "tv_manual";
                    record.imageFileExtension = "png";
                    record.imageFullFilePath = @"C:\attachments\downloads\manual.png";
                    record.title = "Image Showing The Picture Of The TV manual";
                    records.Add(record);

                    //update the details of the document after all records have sucessfully been obtained
                    esDocumentImages.dataRecords = records.ToArray();
                    esDocumentImages.totalDataRecords = records.Count;
                    esDocumentImages.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_IMAGES, ESDocumentConstants.RESULT_SUCCESS);
                    esDocumentImages.resultStatus = ESDocumentConstants.RESULT_SUCCESS;

                    // add a document config that specifies all of the record properties that may contain data in the document
                    // This properties indicate to other systems importing the document on which record data can be inserted or overwritten
                    esDocumentConfigs.Add("dataFields", "keyImageID,keyProductID,keyDownloadID,imageFileName,imageFileExtension,imageFullFilePath,title,description");
                }
                else
                {
                    esDocumentImages.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_IMAGES, ESDocumentConstants.RESULT_ERROR_CONNECTOR_INVALID_CREDENTIALS);
                    esDocumentImages.resultStatus = ESDocumentConstants.RESULT_ERROR_CONNECTOR_INVALID_CREDENTIALS;
                }
            }
            catch (Exception ex)
            {
                esDocumentImages.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_IMAGES, ESDocumentConstants.RESULT_ERROR_CONNECTOR_PROCESSING);
                esDocumentImages.resultStatus = ESDocumentConstants.RESULT_ERROR_CONNECTOR_PROCESSING;
            }

            //output message to the console
            ESDWebServiceConstants.outputConsoleMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_IMAGES, ESDWebServiceConstants.ESD_ENDPOINT_METHOD_GET_END, esDocumentImages.message);

            //serializes the ESD document into 
            return serializeESDocument(esDocumentImages);
        }

        /// <summary>Obtains an Ecommerce Standards Document containing an array of item group records associations to products, downloads, and/or labour</summary>
        /// <returns>Serialized Ecommerce Standards Document in the JSON data format</returns>
        public System.ServiceModel.Channels.Message getItemGroups()
        {
            ESDWebServiceConstants.outputConsoleMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_ITEM_GROUPS, ESDWebServiceConstants.ESD_ENDPOINT_METHOD_GET_START, "");

            //create Ecommerce standards document for storing records
            Dictionary<string, string> esDocumentConfigs = new Dictionary<string, string>();
            ESDocumentItemGroup esDocumentItemGroup = new ESDocumentItemGroup(ESDocumentConstants.RESULT_ERROR_UNKNOWN, ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_ITEM_GROUPS, ESDocumentConstants.RESULT_ERROR_UNKNOWN), new ESDRecordItemGroup[] { }, esDocumentConfigs);

            try
            {
                //check that data is allowed to be returned from the web service based on the incomming HTTP request headers
                if (checkRequestCredentials())
                {
                    //obtain record data
                    //This is where you would call a database or other data source to obtain and place data into the standards record objects
                    List<ESDRecordItemGroup> records = new List<ESDRecordItemGroup>();

                    //add record data to the document
                    ESDRecordItemGroup record = new ESDRecordItemGroup();
                    record.keyItemGroupID = "2";
                    record.groupCode = "COL-TEATOWEL";
                    record.groupLabel = "Coloured Teatowels";
                    record.groupDescription = "Group contains an assorted number of teatowels, each styled with vibrant colours.";
                    record.keyProductIDs = new string[] {"123", "456", "789", "1000"};
                    record.keyDownloadIDs = new string[] {};
                    record.keyLabourIDs = new string[] {};
                    record.keyDefaultProductID = "456";
                    records.Add(record);

                    record = new ESDRecordItemGroup();
                    record.keyItemGroupID = "3";
                    record.groupCode = "GRP-TV-SETUP";
                    record.groupLabel = "Amazing TV Bundle";
                    record.groupDescription = "Bundle contains everything you need to get a TV. It contains a television, labour to install the TV, and a downloadable manual to be able to use the TV.";
                    record.keyProductIDs = new string[]{"0987"};
                    record.keyDownloadIDs = new string[]{"DOWN-9898"};
                    record.keyLabourIDs = new string[]{ "LAB-INSTALL"};
                    record.keyDefaultProductID = "0987";
                    records.Add(record);

                    //update the details of the document after all records have sucessfully been obtained
                    esDocumentItemGroup.dataRecords = records.ToArray();
                    esDocumentItemGroup.totalDataRecords = records.Count;
                    esDocumentItemGroup.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_ITEM_GROUPS, ESDocumentConstants.RESULT_SUCCESS);
                    esDocumentItemGroup.resultStatus = ESDocumentConstants.RESULT_SUCCESS;

                    // add a document config that specifies all of the record properties that may contain data in the document
                    // This properties indicate to other systems importing the document on which record data can be inserted or overwritten
                    esDocumentConfigs.Add("dataFields", "keyItemGroupID,groupCode,groupLabel,groupDescription,keyDefaultProductID");
                }
                else
                {
                    esDocumentItemGroup.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_ITEM_GROUPS, ESDocumentConstants.RESULT_ERROR_CONNECTOR_INVALID_CREDENTIALS);
                    esDocumentItemGroup.resultStatus = ESDocumentConstants.RESULT_ERROR_CONNECTOR_INVALID_CREDENTIALS;
                }
            }
            catch (Exception ex)
            {
                esDocumentItemGroup.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_ITEM_GROUPS, ESDocumentConstants.RESULT_ERROR_CONNECTOR_PROCESSING);
                esDocumentItemGroup.resultStatus = ESDocumentConstants.RESULT_ERROR_CONNECTOR_PROCESSING;
            }

            //output message to the console
            ESDWebServiceConstants.outputConsoleMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_ITEM_GROUPS, ESDWebServiceConstants.ESD_ENDPOINT_METHOD_GET_END, esDocumentItemGroup.message);

            //serializes the ESD document into 
            return serializeESDocument(esDocumentItemGroup);
        }

        /// <summary>Obtains an Ecommerce Standards Document containing an array of item relation records associations to products, downloads, and/or labour</summary>
        /// <returns>Serialized Ecommerce Standards Document in the JSON data format</returns>
        public System.ServiceModel.Channels.Message getItemRelations()
        {
            ESDWebServiceConstants.outputConsoleMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_ITEM_RELATIONS, ESDWebServiceConstants.ESD_ENDPOINT_METHOD_GET_START, "");

            //create Ecommerce standards document for storing records
            Dictionary<string, string> esDocumentConfigs = new Dictionary<string, string>();
            ESDocumentItemRelation esDocumentItemRelations = new ESDocumentItemRelation(ESDocumentConstants.RESULT_ERROR_UNKNOWN, ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_ITEM_RELATIONS, ESDocumentConstants.RESULT_ERROR_UNKNOWN), new ESDRecordItemRelation[] { }, esDocumentConfigs);

            try
            {
                //check that data is allowed to be returned from the web service based on the incomming HTTP request headers
                if (checkRequestCredentials())
                {
                    //obtain record data
                    //This is where you would call a database or other data source to obtain and place data into the standards record objects
                    List<ESDRecordItemRelation> records = new List<ESDRecordItemRelation>();

                    //add record data to the document
                    ESDRecordItemRelation record = new ESDRecordItemRelation();
                    record.keyProductID = "123";
                    record.keyRelatedProductID = "456";
                    records.Add(record);

                    record = new ESDRecordItemRelation();
                    record.keyProductID = "123";
                    record.keyRelatedDownloadID = "DWN1";
                    records.Add(record);

                    record.keyDownloadID = "DWN1";
                    record.keyRelatedLabourID = "LAB2";
                    records.Add(record);

                    record.keyLabourID = "LAB1";
                    record.keyRelatedProductID = "123";
                    records.Add(record);

                    //update the details of the document after all records have sucessfully been obtained
                    esDocumentItemRelations.dataRecords = records.ToArray();
                    esDocumentItemRelations.totalDataRecords = records.Count;
                    esDocumentItemRelations.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_ITEM_RELATIONS, ESDocumentConstants.RESULT_SUCCESS);
                    esDocumentItemRelations.resultStatus = ESDocumentConstants.RESULT_SUCCESS;

                    // add a document config that specifies all of the record properties that may contain data in the document
                    // This properties indicate to other systems importing the document on which record data can be inserted or overwritten
                    esDocumentConfigs.Add("dataFields", "keyProductID,keyDownloadID,keyLabourID,keyRelatedDownloadID,keyRelatedProductID,keyRelatedLabourID");
                }
                else
                {
                    esDocumentItemRelations.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_ITEM_RELATIONS, ESDocumentConstants.RESULT_ERROR_CONNECTOR_INVALID_CREDENTIALS);
                    esDocumentItemRelations.resultStatus = ESDocumentConstants.RESULT_ERROR_CONNECTOR_INVALID_CREDENTIALS;
                }
            }
            catch (Exception ex)
            {
                esDocumentItemRelations.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_ITEM_RELATIONS, ESDocumentConstants.RESULT_ERROR_CONNECTOR_PROCESSING);
                esDocumentItemRelations.resultStatus = ESDocumentConstants.RESULT_ERROR_CONNECTOR_PROCESSING;
            }

            //output message to the console
            ESDWebServiceConstants.outputConsoleMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_ITEM_RELATIONS, ESDWebServiceConstants.ESD_ENDPOINT_METHOD_GET_END, esDocumentItemRelations.message);

            //serializes the ESD document into 
            return serializeESDocument(esDocumentItemRelations);
        }

        /// <summary>Obtains an Ecommerce Standards Document containing an array of kit records and associations to products, downloads, and/or labour records</summary>
        /// <returns>Serialized Ecommerce Standards Document in the JSON data format</returns>
        public System.ServiceModel.Channels.Message getKits()
        {
            ESDWebServiceConstants.outputConsoleMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_KITS, ESDWebServiceConstants.ESD_ENDPOINT_METHOD_GET_START, "");

            //create Ecommerce standards document for storing records
            Dictionary<string, string> esDocumentConfigs = new Dictionary<string, string>();
            ESDocumentKit esDocumentKits = new ESDocumentKit(ESDocumentConstants.RESULT_ERROR_UNKNOWN, ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_KITS, ESDocumentConstants.RESULT_ERROR_UNKNOWN), new ESDRecordKitComponent[]{}, esDocumentConfigs);

            try
            {
                //check that data is allowed to be returned from the web service based on the incomming HTTP request headers
                if (checkRequestCredentials())
                {
                    //obtain record data
                    //This is where you would call a database or other data source to obtain and place data into the standards record objects
                    List<ESDRecordKitComponent> records = new List<ESDRecordKitComponent>();

                    //add record data to the document
                    ESDRecordKitComponent record = new ESDRecordKitComponent();
                    record.keyKitProductID = "PROD-432";
                    record.keyComponentProductID = "PROD-123";
                    records.Add(record);

                    record = new ESDRecordKitComponent();
                    record.keyKitProductID = "PROD-444";
                    record.keyComponentProductID = "PROD-123";
                    record.quantity = 3;
                    record.quantity = 3;
                    records.Add(record);

                    record = new ESDRecordKitComponent();
                    record.keyKitProductID = "PROD-444";
                    record.keyComponentProductID = "PROD-456";
                    record.quantity = 10;
                    record.quantity = 1;
                    records.Add(record);

                    record = new ESDRecordKitComponent();
                    record.keyKitProductID = "PROD-444";
                    record.keyComponentProductID = "PROD-789";
                    record.quantity = 1;
                    record.quantity = 2;
                    records.Add(record);

                    //update the details of the document after all records have sucessfully been obtained
                    esDocumentKits.dataRecords = records.ToArray();
                    esDocumentKits.totalDataRecords = records.Count;
                    esDocumentKits.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_KITS, ESDocumentConstants.RESULT_SUCCESS);
                    esDocumentKits.resultStatus = ESDocumentConstants.RESULT_SUCCESS;

                    // add a document config that specifies all of the record properties that may contain data in the document
                    // This properties indicate to other systems importing the document on which record data can be inserted or overwritten
                    esDocumentConfigs.Add("dataFields", "keyKitProductID,keyComponentProductID,quantity,ordering");
                }
                else
                {
                    esDocumentKits.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_KITS, ESDocumentConstants.RESULT_ERROR_CONNECTOR_INVALID_CREDENTIALS);
                    esDocumentKits.resultStatus = ESDocumentConstants.RESULT_ERROR_CONNECTOR_INVALID_CREDENTIALS;
                }
            }
            catch (Exception ex)
            {
                esDocumentKits.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_KITS, ESDocumentConstants.RESULT_ERROR_CONNECTOR_PROCESSING);
                esDocumentKits.resultStatus = ESDocumentConstants.RESULT_ERROR_CONNECTOR_PROCESSING;
            }

            //output message to the console
            ESDWebServiceConstants.outputConsoleMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_KITS, ESDWebServiceConstants.ESD_ENDPOINT_METHOD_GET_END, esDocumentKits.message);

            //serializes the ESD document
            return serializeESDocument(esDocumentKits);
        }

        /// <summary>Obtains an Ecommerce Standards Document containing an array of labour records</summary>
        /// <returns>Serialized Ecommerce Standards Document in the JSON data format</returns>
        public System.ServiceModel.Channels.Message getLabour()
        {
            ESDWebServiceConstants.outputConsoleMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_LABOUR, ESDWebServiceConstants.ESD_ENDPOINT_METHOD_GET_START, "");

            //create Ecommerce standards document for storing records
            Dictionary<string, string> esDocumentConfigs = new Dictionary<string, string>();
            ESDocumentLabour esDocumentLabour = new ESDocumentLabour(ESDocumentConstants.RESULT_ERROR_UNKNOWN, ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_LABOUR, ESDocumentConstants.RESULT_ERROR_UNKNOWN), new ESDRecordLabour[]{}, esDocumentConfigs);

            try
            {
                //check that data is allowed to be returned from the web service based on the incomming HTTP request headers
                if (checkRequestCredentials())
                {
                    //obtain record data
                    //This is where you would call a database or other data source to obtain and place data into the standards record objects
                    List<ESDRecordLabour> records = new List<ESDRecordLabour>();

                    //update the details of the document after all records have sucessfully been obtained
                    esDocumentLabour.dataRecords = records.ToArray();
                    esDocumentLabour.totalDataRecords = records.Count;
                    esDocumentLabour.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_LABOUR, ESDocumentConstants.RESULT_SUCCESS);
                    esDocumentLabour.resultStatus = ESDocumentConstants.RESULT_SUCCESS;

                    // add a document config that specifies all of the record properties that may contain data in the document
                    // This properties indicate to other systems importing the document on which record data can be inserted or overwritten
                    esDocumentConfigs.Add("dataFields", "");
                }
                else
                {
                    esDocumentLabour.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_LABOUR, ESDocumentConstants.RESULT_ERROR_CONNECTOR_INVALID_CREDENTIALS);
                    esDocumentLabour.resultStatus = ESDocumentConstants.RESULT_ERROR_CONNECTOR_INVALID_CREDENTIALS;
                }
            }
            catch (Exception ex)
            {
                esDocumentLabour.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_LABOUR, ESDocumentConstants.RESULT_ERROR_CONNECTOR_PROCESSING);
                esDocumentLabour.resultStatus = ESDocumentConstants.RESULT_ERROR_CONNECTOR_PROCESSING;
            }

            //output message to the console
            ESDWebServiceConstants.outputConsoleMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_LABOUR, ESDWebServiceConstants.ESD_ENDPOINT_METHOD_GET_END, esDocumentLabour.message);

            //serializes the ESD document into 
            return serializeESDocument(esDocumentLabour);
        }

        /// <summary>Obtains an Ecommerce Standards Document containing an array of location records</summary>
        /// <returns>Serialized Ecommerce Standards Document in the JSON data format</returns>
        public System.ServiceModel.Channels.Message getLocations()
        {
            ESDWebServiceConstants.outputConsoleMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_LOCATIONS, ESDWebServiceConstants.ESD_ENDPOINT_METHOD_GET_START, "");

            //create Ecommerce standards document for storing records
            Dictionary<string, string> esDocumentConfigs = new Dictionary<string, string>();
            ESDocumentLocation esDocumentLocations = new ESDocumentLocation(ESDocumentConstants.RESULT_ERROR_UNKNOWN, ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_LOCATIONS, ESDocumentConstants.RESULT_ERROR_UNKNOWN), new ESDRecordLocation[] { }, esDocumentConfigs);

            try
            {
                //check that data is allowed to be returned from the web service based on the incomming HTTP request headers
                if (checkRequestCredentials())
                {
                    //obtain record data
                    //This is where you would call a database or other data source to obtain and place data into the standards record objects
                    List<ESDRecordLocation> records = new List<ESDRecordLocation>();

                    //add record data to the document
                    ESDRecordLocation record = new ESDRecordLocation();
                    record.keyLocationID = "123";
                    record.locationCode = "LOC-123";
                    record.locationName = "Head Office";
                    record.address1 = "234";
                    record.address2 = "Bourke Street";
                    record.address3 = "Melbourne";
                    record.region = "Victoria";
                    record.country = "Australia";
                    record.postcode = "3000";
                    record.contact = "John Doe";
                    record.phone = "+614001112222";
                    record.fax = "+614002223333";
                    record.isActive = ESDWebServiceConstants.ESD_VALUE_Y;
                    record.isGeographic = ESDWebServiceConstants.ESD_VALUE_Y;
                    record.latitude = (decimal)45.123;
                    record.longitude = (decimal)-72.123;
                    records.Add(record);

                    record = new ESDRecordLocation();
                    record.keyLocationID = "456";
                    record.locationCode = "LCT-456";
                    record.locationName = "Warehouse";
                    record.address1 = "237";
                    record.address2 = "Bourke Street";
                    record.address3 = "Melbourne";
                    record.region = "Victoria";
                    record.country = "Australia";
                    record.postcode = "3000";
                    record.contact = "Max Smith";
                    record.phone = "+614003334444";
                    record.fax = "+614005556666";
                    record.isActive = ESDWebServiceConstants.ESD_VALUE_Y;
                    record.isGeographic = ESDWebServiceConstants.ESD_VALUE_Y;
                    record.latitude = (decimal)45.423;
                    record.longitude = (decimal)-72.823;

                    //add product stock records to the location record to denote the amount of stock that exists at the location
                    List<ESDRecordStockQuantity> recordStockQuantityRecords = new List<ESDRecordStockQuantity>();
                    ESDRecordStockQuantity stockQuantityRecord = new ESDRecordStockQuantity();
                    stockQuantityRecord.keyProductID = "ABC";
                    stockQuantityRecord.qtyAvailable = 432;
                    recordStockQuantityRecords.Add(stockQuantityRecord);
                    stockQuantityRecord = new ESDRecordStockQuantity();
                    stockQuantityRecord.keyProductID = "DEF";
                    stockQuantityRecord.qtyAvailable = 0;
                    stockQuantityRecord.qtyOnHand = 0;
                    stockQuantityRecord.qtyOrdered = 5;
                    stockQuantityRecord.qtyBackordered = 10;
                    stockQuantityRecord.qtyReserved = 11;
                    stockQuantityRecord.qtyConsigned = 3;
                    recordStockQuantityRecords.Add(stockQuantityRecord);
                    record.productStock = recordStockQuantityRecords.ToArray();
                    records.Add(record);

                    //update the details of the document after all records have sucessfully been obtained
                    esDocumentLocations.dataRecords = records.ToArray();
                    esDocumentLocations.totalDataRecords = records.Count;
                    esDocumentLocations.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_LOCATIONS, ESDocumentConstants.RESULT_SUCCESS);
                    esDocumentLocations.resultStatus = ESDocumentConstants.RESULT_SUCCESS;

                    // add a document config that specifies all of the record properties that may contain data in the document
                    // This properties indicate to other systems importing the document on which record data can be inserted or overwritten
                    esDocumentConfigs.Add("dataFieldsLocation", "keyLocationID,locationCode,locationName,address1,address2,address3,region,country,postcode,contact,phone,fax,isActive,isGeographic,latitude,longitude");
                    esDocumentConfigs.Add("dataFieldsLocationStock", "keyProductID,qtyAvailable,qtyOnHand,qtyOrdered,qtyBackordered,qtyReserved,qtyConsigned");

                }
                else
                {
                    esDocumentLocations.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_LOCATIONS, ESDocumentConstants.RESULT_ERROR_CONNECTOR_INVALID_CREDENTIALS);
                    esDocumentLocations.resultStatus = ESDocumentConstants.RESULT_ERROR_CONNECTOR_INVALID_CREDENTIALS;
                }
            }
            catch (Exception ex)
            {
                esDocumentLocations.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_LOCATIONS, ESDocumentConstants.RESULT_ERROR_CONNECTOR_PROCESSING);
                esDocumentLocations.resultStatus = ESDocumentConstants.RESULT_ERROR_CONNECTOR_PROCESSING;
            }

            //output message to the console
            ESDWebServiceConstants.outputConsoleMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_LOCATIONS, ESDWebServiceConstants.ESD_ENDPOINT_METHOD_GET_END, esDocumentLocations.message);

            //serializes the ESD document
            return serializeESDocument(esDocumentLocations);
        }

        /// <summary>Obtains an Ecommerce Standards Document containing an array of payment type records</summary>
        /// <returns>Serialized Ecommerce Standards Document in the JSON data format</returns>
        public System.ServiceModel.Channels.Message getPaymentTypes()
        {
            ESDWebServiceConstants.outputConsoleMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_PAYMENT_TYPES, ESDWebServiceConstants.ESD_ENDPOINT_METHOD_GET_START, "");

            //create Ecommerce standards document for storing records
            Dictionary<string, string> esDocumentConfigs = new Dictionary<string, string>();
            ESDocumentPaymentType esDocumentPaymentType = new ESDocumentPaymentType(ESDocumentConstants.RESULT_ERROR_UNKNOWN, ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_PAYMENT_TYPES, ESDocumentConstants.RESULT_ERROR_UNKNOWN), new ESDRecordPaymentType[] { }, esDocumentConfigs);

            try
            {
                //check that data is allowed to be returned from the web service based on the incomming HTTP request headers
                if (checkRequestCredentials())
                {
                    //obtain record data
                    //This is where you would call a database or other data source to obtain and place data into the standards record objects
                    List<ESDRecordPaymentType> records = new List<ESDRecordPaymentType>();

                    //add record data to the document
                    ESDRecordPaymentType record = new ESDRecordPaymentType();
                    record.keyPaymentTypeID = "456";
                    record.paymentTypeCode = "WEBCC";
                    record.paymentTypeLabel = "Credit Card";
                    record.description = "Credit Card payment made through a website";
                    record.paymentMethod = ESDocumentConstants.PAYMENT_METHOD_CREDIT;
                    records.Add(record);

                    record = new ESDRecordPaymentType();
                    record.keyPaymentTypeID = "765";
                    record.paymentTypeCode = "DD";
                    record.paymentTypeLabel = "Direct Deposit";
                    record.description = "Payment made using a direct deposit bank transfer";
                    record.paymentMethod = ESDocumentConstants.PAYMENT_METHOD_DIRECTDEPOSIT;
                    records.Add(record);

                    record = new ESDRecordPaymentType();
                    record.keyPaymentTypeID = "6765";
                    record.paymentTypeCode = "AC";
                    record.paymentTypeLabel = "On Account";
                    record.description = "Payment will be made at a time based on aggreed upon account terms";
                    record.paymentMethod = ESDocumentConstants.PAYMENT_METHOD_ACCOUNT;
                    records.Add(record);

                    //update the details of the document after all records have sucessfully been obtained
                    esDocumentPaymentType.dataRecords = records.ToArray();
                    esDocumentPaymentType.totalDataRecords = records.Count;
                    esDocumentPaymentType.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_PAYMENT_TYPES, ESDocumentConstants.RESULT_SUCCESS);
                    esDocumentPaymentType.resultStatus = ESDocumentConstants.RESULT_SUCCESS;

                    // add a document config that specifies all of the record properties that may contain data in the document
                    // This properties indicate to other systems importing the document on which record data can be inserted or overwritten
                    esDocumentConfigs.Add("dataFields", "keyPaymentTypeID,paymentTypeCode,paymentTypeLabel,description,paymentMethod");
                }
                else
                {
                    esDocumentPaymentType.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_PAYMENT_TYPES, ESDocumentConstants.RESULT_ERROR_CONNECTOR_INVALID_CREDENTIALS);
                    esDocumentPaymentType.resultStatus = ESDocumentConstants.RESULT_ERROR_CONNECTOR_INVALID_CREDENTIALS;
                }
            }
            catch (Exception ex)
            {
                esDocumentPaymentType.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_PAYMENT_TYPES, ESDocumentConstants.RESULT_ERROR_CONNECTOR_PROCESSING);
                esDocumentPaymentType.resultStatus = ESDocumentConstants.RESULT_ERROR_CONNECTOR_PROCESSING;
            }

            //output message to the console
            ESDWebServiceConstants.outputConsoleMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_PAYMENT_TYPES, ESDWebServiceConstants.ESD_ENDPOINT_METHOD_GET_END, esDocumentPaymentType.message);

            //serializes the ESD document into 
            return serializeESDocument(esDocumentPaymentType);
        }

        /// <summary>Obtains an Ecommerce Standards Document containing an array of price level pricing records associated to products, downloads and/or labour records</summary>
        /// <returns>Serialized Ecommerce Standards Document in the JSON data format</returns>
        public System.ServiceModel.Channels.Message getPriceLevelPricing()
        {
            ESDWebServiceConstants.outputConsoleMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_PRICE_LEVEL_PRICING, ESDWebServiceConstants.ESD_ENDPOINT_METHOD_GET_START, "");

            //create Ecommerce standards document for storing records
            Dictionary<string, string> esDocumentConfigs = new Dictionary<string, string>();
            ESDocumentPrice esDocumentPricing = new ESDocumentPrice(ESDocumentConstants.RESULT_ERROR_UNKNOWN, ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_PRICE_LEVEL_PRICING, ESDocumentConstants.RESULT_ERROR_UNKNOWN), new ESDRecordPrice[] { }, esDocumentConfigs);

            try
            {
                //check that data is allowed to be returned from the web service based on the incomming HTTP request headers
                if (checkRequestCredentials())
                {
                    //obtain record data
                    //This is where you would call a database or other data source to obtain and place data into the standards record objects
                    List<ESDRecordPrice> records = new List<ESDRecordPrice>();

                    //add record data to the document
                    ESDRecordPrice record = new ESDRecordPrice();
                    record.keyProductID = "PROD-123";
                    record.keyPriceLevelID = "PL-001";
                    record.keySellUnitID = "1";
                    record.price = (decimal)10.00;
                    records.Add(record);

                    record = new ESDRecordPrice();
                    record.keyProductID = "PROD-123";
                    record.keyPriceLevelID = "PL-002";
                    record.keySellUnitID = "1";
                    record.price = (decimal)8.00;
                    records.Add(record);

                    record = new ESDRecordPrice();
                    record.keyProductID = "PROD-123";
                    record.keyPriceLevelID = "PL-003";
                    record.keySellUnitID = "1";
                    record.price = (decimal)5.00;
                    records.Add(record);

                    record = new ESDRecordPrice();
                    record.keyProductID = "PROD-456";
                    record.keyPriceLevelID = "PL-001";
                    record.keySellUnitID = "1";
                    record.price = (decimal)22.00;
                    records.Add(record);

                    //update the details of the document after all records have sucessfully been obtained
                    esDocumentPricing.dataRecords = records.ToArray();
                    esDocumentPricing.totalDataRecords = records.Count;
                    esDocumentPricing.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_PRICE_LEVEL_PRICING, ESDocumentConstants.RESULT_SUCCESS);
                    esDocumentPricing.resultStatus = ESDocumentConstants.RESULT_SUCCESS;

                    // add a document config that specifies all of the record properties that may contain data in the document
                    // This properties indicate to other systems importing the document on which record data can be inserted or overwritten
                    esDocumentConfigs.Add("dataFields", "keyProductID,keyPriceLevelID,keySellUnitID,price");
                }
                else
                {
                    esDocumentPricing.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_PRICE_LEVEL_PRICING, ESDocumentConstants.RESULT_ERROR_CONNECTOR_INVALID_CREDENTIALS);
                    esDocumentPricing.resultStatus = ESDocumentConstants.RESULT_ERROR_CONNECTOR_INVALID_CREDENTIALS;
                }
            }
            catch (Exception ex)
            {
                esDocumentPricing.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_PRICE_LEVEL_PRICING, ESDocumentConstants.RESULT_ERROR_CONNECTOR_PROCESSING);
                esDocumentPricing.resultStatus = ESDocumentConstants.RESULT_ERROR_CONNECTOR_PROCESSING;
            }

            //output message to the console
            ESDWebServiceConstants.outputConsoleMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_PRICE_LEVEL_PRICING, ESDWebServiceConstants.ESD_ENDPOINT_METHOD_GET_END, esDocumentPricing.message);

            //serializes the ESD document into 
            return serializeESDocument(esDocumentPricing);
        }

        /// <summary>Obtains an Ecommerce Standards Document containing an array of price level quantity break pricing records associated to products, downloads and/or labour records</summary>
        /// <returns>Serialized Ecommerce Standards Document in the JSON data format</returns>
        public System.ServiceModel.Channels.Message getPriceLevelQuantityPricing()
        {
            ESDWebServiceConstants.outputConsoleMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_PRICE_LEVEL_QUANTITY_PRICING, ESDWebServiceConstants.ESD_ENDPOINT_METHOD_GET_START, "");

            //create Ecommerce standards document for storing records
            Dictionary<string, string> esDocumentConfigs = new Dictionary<string, string>();
            ESDocumentPrice esDocumentPricing = new ESDocumentPrice(ESDocumentConstants.RESULT_ERROR_UNKNOWN, ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_PRICE_LEVEL_QUANTITY_PRICING, ESDocumentConstants.RESULT_ERROR_UNKNOWN), new ESDRecordPrice[] { }, esDocumentConfigs);

            try
            {
                //check that data is allowed to be returned from the web service based on the incomming HTTP request headers
                if (checkRequestCredentials())
                {
                    //obtain record data
                    //This is where you would call a database or other data source to obtain and place data into the standards record objects
                    List<ESDRecordPrice> records = new List<ESDRecordPrice>();

                    //add record data to the document
                    ESDRecordPrice record = new ESDRecordPrice();
                    record.keyProductID = "PROD-123";
                    record.keyPriceLevelID = "PL-001";
                    record.keySellUnitID = "1";
                    record.price = (decimal)10.00;
                    record.quantity = 5;
                    records.Add(record);

                    record = new ESDRecordPrice();
                    record.keyProductID = "PROD-123";
                    record.keyPriceLevelID = "PL-001";
                    record.keySellUnitID = "1";
                    record.price = (decimal)5.00;
                    record.quantity = 10;
                    records.Add(record);

                    record = new ESDRecordPrice();
                    record.keyProductID = "PROD-123";
                    record.keyPriceLevelID = "PL-003";
                    record.keySellUnitID = "1";
                    record.price = (decimal)2.00;
                    record.quantity = 20;
                    records.Add(record);

                    record = new ESDRecordPrice();
                    record.keyProductID = "PROD-456";
                    record.keyPriceLevelID = "PL-001";
                    record.keySellUnitID = "1";
                    record.price = (decimal)4.10;
                    record.quantity = 5;
                    records.Add(record);


                    //update the details of the document after all records have sucessfully been obtained
                    esDocumentPricing.dataRecords = records.ToArray();
                    esDocumentPricing.totalDataRecords = records.Count;
                    esDocumentPricing.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_PRICE_LEVEL_QUANTITY_PRICING, ESDocumentConstants.RESULT_SUCCESS);
                    esDocumentPricing.resultStatus = ESDocumentConstants.RESULT_SUCCESS;

                    // add a document config that specifies all of the record properties that may contain data in the document
                    // This properties indicate to other systems importing the document on which record data can be inserted or overwritten
                    esDocumentConfigs.Add("dataFields", "keyProductID,keyPriceLevelID,keySellUnitID,price");
                }
                else
                {
                    esDocumentPricing.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_PRICE_LEVEL_QUANTITY_PRICING, ESDocumentConstants.RESULT_ERROR_CONNECTOR_INVALID_CREDENTIALS);
                    esDocumentPricing.resultStatus = ESDocumentConstants.RESULT_ERROR_CONNECTOR_INVALID_CREDENTIALS;
                }
            }
            catch (Exception ex)
            {
                esDocumentPricing.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_PRICE_LEVEL_QUANTITY_PRICING, ESDocumentConstants.RESULT_ERROR_CONNECTOR_PROCESSING);
                esDocumentPricing.resultStatus = ESDocumentConstants.RESULT_ERROR_CONNECTOR_PROCESSING;
            }

            //output message to the console
            ESDWebServiceConstants.outputConsoleMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_PRICE_LEVEL_QUANTITY_PRICING, ESDWebServiceConstants.ESD_ENDPOINT_METHOD_GET_END, esDocumentPricing.message);

            //serializes the ESD document
            return serializeESDocument(esDocumentPricing);
        }

        /// <summary>Obtains an Ecommerce Standards Document containing an array of customer account pricing records associated to products, downloads and/or labour records</summary>
        /// <returns>Serialized Ecommerce Standards Document in the JSON data format</returns>
        public System.ServiceModel.Channels.Message getProductAccountPricing()
        {
            ESDWebServiceConstants.outputConsoleMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_CUSTOMER_ACCOUNT_PRICING, ESDWebServiceConstants.ESD_ENDPOINT_METHOD_GET_START, "");

            //create Ecommerce standards document for storing records
            Dictionary<string, string> esDocumentConfigs = new Dictionary<string, string>();
            ESDocumentPrice esDocumentPricing = new ESDocumentPrice(ESDocumentConstants.RESULT_ERROR_UNKNOWN, ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_CUSTOMER_ACCOUNT_PRICING, ESDocumentConstants.RESULT_ERROR_UNKNOWN), new ESDRecordPrice[] { }, esDocumentConfigs);

            try
            {
                //check that data is allowed to be returned from the web service based on the incomming HTTP request headers
                if (checkRequestCredentials())
                {
                    //obtain record data
                    //This is where you would call a database or other data source to obtain and place data into the standards record objects
                    List<ESDRecordPrice> records = new List<ESDRecordPrice>();

                    //add record data to the document
                    ESDRecordPrice record = new ESDRecordPrice();
                    record.keyProductID = "PROD-123";
                    record.keyAccountID = "ACC-123";
                    record.keySellUnitID = "1";
                    record.price = (decimal)70.00;
                    record.quantity = 5;
                    record.referenceID = "FORCED-CONTRACT-1";
                    record.referenceType = "CF";
                    records.Add(record);

                    record = new ESDRecordPrice();
                    record.keyProductID = "PROD-123";
                    record.keyAccountID = "ACC-123";
                    record.keySellUnitID = "1";
                    record.price = (decimal)1.00;
                    record.quantity = 20;
                    record.referenceID = "FORCED-CONTRACT-1";
                    record.referenceType = "CF";
                    records.Add(record);

                    record = new ESDRecordPrice();
                    record.keyProductID = "PROD-123";
                    record.keyAccountID = "ACC-456";
                    record.keySellUnitID = "1";
                    record.price = (decimal)7.30;
                    record.quantity = 1;
                    record.referenceID = "CONTRACT-222";
                    record.referenceType = "C";
                    records.Add(record);

                    record = new ESDRecordPrice();
                    record.keyProductID = "PROD-456";
                    record.keyAccountID = "ACC-456";
                    record.keySellUnitID = "1";
                    record.price = (decimal)3.30;
                    records.Add(record);

                    record = new ESDRecordPrice();
                    record.keyProductID = "PROD-123";
                    record.keyPriceGroupID = "PRICE-GROUP-2";
                    record.keySellUnitID = "1";
                    record.price = (decimal)2.90;
                    record.quantity = 1;
                    record.referenceID = "FORCED-CONTRACT-1";
                    record.referenceType = "CF";
                    records.Add(record);

                    record = new ESDRecordPrice();
                    record.keyProductID = "PROD-456";
                    record.keyPriceGroupID = "PRICE-GROUP-2";
                    record.keySellUnitID = "1";
                    record.price = (decimal)0.255;
                    record.quantity =50;
                    records.Add(record);

                    //create price groups that allow multiple customer accounts to be assigned a given price group and associated pricing
                    Dictionary<string, string[]> priceGroups = new Dictionary<string, string[]>();
                    string[] priceGroup = new string[]{"ACC-1", "ACC-2", "ACC-3", "ACC-4"};
                    priceGroups.Add("PRICE-GROUP-1", priceGroup.ToArray());
                    priceGroup = new string[]{ "ACC-5", "ACC-6", "ACC-7", "ACC-8" };
                    priceGroups.Add("PRICE-GROUP-2", priceGroup.ToArray());

                    //update the details of the document after all records have sucessfully been obtained
                    esDocumentPricing.priceGroups = priceGroups;
                    esDocumentPricing.dataRecords = records.ToArray();
                    esDocumentPricing.totalDataRecords = records.Count;
                    esDocumentPricing.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_CUSTOMER_ACCOUNT_PRICING, ESDocumentConstants.RESULT_SUCCESS);
                    esDocumentPricing.resultStatus = ESDocumentConstants.RESULT_SUCCESS;

                    // add a document config that specifies all of the record properties that may contain data in the document
                    // This properties indicate to other systems importing the document on which record data can be inserted or overwritten
                    esDocumentConfigs.Add("dataFields", "keyProductID,keyAccountID,keyPriceGroupID,keySellUnitID,price,quantity,referenceID,referenceType");
                }
                else
                {
                    esDocumentPricing.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_CUSTOMER_ACCOUNT_PRICING, ESDocumentConstants.RESULT_ERROR_CONNECTOR_INVALID_CREDENTIALS);
                    esDocumentPricing.resultStatus = ESDocumentConstants.RESULT_ERROR_CONNECTOR_INVALID_CREDENTIALS;
                }
            }
            catch (Exception ex)
            {
                esDocumentPricing.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_CUSTOMER_ACCOUNT_PRICING, ESDocumentConstants.RESULT_ERROR_CONNECTOR_PROCESSING);
                esDocumentPricing.resultStatus = ESDocumentConstants.RESULT_ERROR_CONNECTOR_PROCESSING;
            }

            //output message to the console
            ESDWebServiceConstants.outputConsoleMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_CUSTOMER_ACCOUNT_PRICING, ESDWebServiceConstants.ESD_ENDPOINT_METHOD_GET_END, esDocumentPricing.message);

            //serializes the ESD document into 
            return serializeESDocument(esDocumentPricing);
        }

        /// <summary>Obtains an Ecommerce Standards Document containing an array of product pricing records associated to specific product and customer account. This is typically used for live price querying</summary>
        /// <returns>Serialized Ecommerce Standards Document in the JSON data format</returns>
        public System.ServiceModel.Channels.Message getProductCustomerAccountPrice(string keyCustomerAccountID, string keyProductID, decimal quantity)
        {
            ESDWebServiceConstants.outputConsoleMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_CUSTOMER_ACCOUNT_PRICE, ESDWebServiceConstants.ESD_ENDPOINT_METHOD_GET_START, "");

            //create Ecommerce standards document for storing records
            Dictionary<string, string> esDocumentConfigs = new Dictionary<string, string>();
            ESDocumentPrice esDocumentPricing = new ESDocumentPrice(ESDocumentConstants.RESULT_ERROR_UNKNOWN, ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_CUSTOMER_ACCOUNT_PRICE, ESDocumentConstants.RESULT_ERROR_UNKNOWN), new ESDRecordPrice[] { }, esDocumentConfigs);

            try
            {
                //check that data is allowed to be returned from the web service based on the incomming HTTP request headers
                if (checkRequestCredentials())
                {
                    //obtain record data
                    //This is where you would call a database or other data source to obtain and place data into the standards record objects
                    List<ESDRecordPrice> records = new List<ESDRecordPrice>();

                    //check conditions to obtain the specific price for the customer account based on the given quantity
                    if (keyCustomerAccountID == "ACC-123" && keyProductID == "PROD-123" && quantity == 10)
                    {
                        //add record data to the document
                        ESDRecordPrice record = new ESDRecordPrice();
                        record.keyProductID = "PROD-123";
                        record.keyAccountID = "ACC-123";
                        record.keySellUnitID = "1";
                        record.price = (decimal)50.00;
                        record.quantity = 10;
                        record.referenceID = "FORCED-CONTRACT-1";
                        record.referenceType = "CF";
                        records.Add(record);
                    }

                    //update the details of the document after all records have sucessfully been obtained
                    esDocumentPricing.dataRecords = records.ToArray();
                    esDocumentPricing.totalDataRecords = records.Count;
                    esDocumentPricing.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_CUSTOMER_ACCOUNT_PRICE, ESDocumentConstants.RESULT_SUCCESS);
                    esDocumentPricing.resultStatus = ESDocumentConstants.RESULT_SUCCESS;

                    // add a document config that specifies all of the record properties that may contain data in the document
                    // This properties indicate to other systems importing the document on which record data can be inserted or overwritten
                    esDocumentConfigs.Add("dataFields", "keyProductID,keyAccountID,keySellUnitID,price,quantity,referenceID,referenceType");
                }
                else
                {
                    esDocumentPricing.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_CUSTOMER_ACCOUNT_PRICE, ESDocumentConstants.RESULT_ERROR_CONNECTOR_INVALID_CREDENTIALS);
                    esDocumentPricing.resultStatus = ESDocumentConstants.RESULT_ERROR_CONNECTOR_INVALID_CREDENTIALS;
                }
            }
            catch (Exception ex)
            {
                esDocumentPricing.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_CUSTOMER_ACCOUNT_PRICE, ESDocumentConstants.RESULT_ERROR_CONNECTOR_PROCESSING);
                esDocumentPricing.resultStatus = ESDocumentConstants.RESULT_ERROR_CONNECTOR_PROCESSING;
            }

            //output message to the console
            ESDWebServiceConstants.outputConsoleMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_CUSTOMER_ACCOUNT_PRICE, ESDWebServiceConstants.ESD_ENDPOINT_METHOD_GET_END, esDocumentPricing.message);

            //serializes the ESD document into 
            return serializeESDocument(esDocumentPricing);
        }

        /// <summary>Obtains an Ecommerce Standards Document containing an array of product records</summary>
        /// <returns>Serialized Ecommerce Standards Document in the JSON data format</returns>
        public System.ServiceModel.Channels.Message getProducts()
        {
            ESDWebServiceConstants.outputConsoleMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_PRODUCTS, ESDWebServiceConstants.ESD_ENDPOINT_METHOD_GET_START, "");

            //create Ecommerce standards document for storing records
            Dictionary<string, string> esDocumentConfigs = new Dictionary<string, string>();
            ESDocumentProduct esDocumentProducts = new ESDocumentProduct(ESDocumentConstants.RESULT_ERROR_UNKNOWN, ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_PRODUCTS, ESDocumentConstants.RESULT_ERROR_UNKNOWN), new ESDRecordProduct[] { }, esDocumentConfigs);

            try
            {
                //check that data is allowed to be returned from the web service based on the incomming HTTP request headers
                if (checkRequestCredentials())
                {
                    //obtain record data
                    //This is where you would call a database or other data source to obtain and place data into the standards record objects
                    List<ESDRecordProduct> records = new List<ESDRecordProduct>();

                    //add record data to the document
                    ESDRecordProduct record = new ESDRecordProduct();
                    record.keyProductID = "1234";
                    record.productCode = "PROD-001";
                    record.keyTaxcodeID = "GST";
                    record.productSearchCode = "Green-Recycled-Paper-Swisho";
                    record.barcode = "03423404230";
                    record.barcodeInner = "234234";
                    record.brand = "Swisho Paper";
                    record.name = "Swisho Green Paper";
                    record.description1 = "Swisho green coloured paper is the ultimate green paper.";
                    record.description2 = "Paper built strong and tough by Swisho";
                    record.description3 = "Recommended to be used with dark inks.";
                    record.description4 = "";
                    record.productClass = "paper";
                    record.unit = "REAM";
                    record.weight = (decimal)20.1;
                    record.width = 21;
                    record.height = (decimal)29.7;
                    record.depth = 10;
                    record.averageCost = (decimal)10.00;
                    record.warehouse = "Swisho Warehouse";
                    record.supplier = "Swisho";
                    record.deliveryTimeNoStock = "112112";
                    record.deliveryTimeInStock = "1212";
                    record.stockQuantity = 200;
                    record.stockNoneQuantity = 0;
                    record.stockLowQuantity = 10;
                    record.isPriceTaxInclusive = ESDWebServiceConstants.ESD_VALUE_N;
                    record.isKitted = ESDWebServiceConstants.ESD_VALUE_N;
                    record.kitProductsSetPrice = ESDWebServiceConstants.ESD_VALUE_N;
                    record.keySellUnitID = "2";
                    
                    //assign sell unit record to the product to define the different units that the product is sold in
                    List<ESDRecordSellUnit> sellUnits = new List<ESDRecordSellUnit>();
                    ESDRecordSellUnit sellUnit = new ESDRecordSellUnit();
                    sellUnit.keySellUnitID = "3";
                    sellUnit.keySellUnitParentID = "2";
                    sellUnit.baseQuantity = 6;
                    sellUnits.Add(sellUnit);
                    sellUnit = new ESDRecordSellUnit();
                    sellUnit.keySellUnitID = "4";
                    sellUnit.keySellUnitParentID = "3";
                    sellUnit.baseQuantity = 24;
                    sellUnit.parentQuantity = 4;
                    sellUnits.Add(sellUnit);
                    record.sellUnits = sellUnits.ToArray();
                    records.Add(record);

                    //update the details of the document after all records have sucessfully been obtained
                    esDocumentProducts.dataRecords = records.ToArray();
                    esDocumentProducts.totalDataRecords = records.Count;
                    esDocumentProducts.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_PRODUCTS, ESDocumentConstants.RESULT_SUCCESS);
                    esDocumentProducts.resultStatus = ESDocumentConstants.RESULT_SUCCESS;

                    // add a document config that specifies all of the record properties that may contain data in the document
                    // This properties indicate to other systems importing the document on which record data can be inserted or overwritten
                    esDocumentConfigs.Add("dataFields", "keyProductID,productCode,keyTaxcodeID,productSearchCode,barcode,barcodeInner,brand,name,description1,description2,description3,description4,productClass,keySellUnitID,unit,weight,width,height,depth,averageCost,warehouse,supplier,deliveryTimeNoStock,deliveryTimeInStock,stockQuantity,stockNoneQuantity,stockLowQuantity,stockLowQuantity,isPriceTaxInclusive,isKitted,kitProductsSetPrice");
                }
                else
                {
                    esDocumentProducts.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_PRODUCTS, ESDocumentConstants.RESULT_ERROR_CONNECTOR_INVALID_CREDENTIALS);
                    esDocumentProducts.resultStatus = ESDocumentConstants.RESULT_ERROR_CONNECTOR_INVALID_CREDENTIALS;
                }
            }
            catch (Exception ex)
            {
                esDocumentProducts.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_PRODUCTS, ESDocumentConstants.RESULT_ERROR_CONNECTOR_PROCESSING);
                esDocumentProducts.resultStatus = ESDocumentConstants.RESULT_ERROR_CONNECTOR_PROCESSING;
            }

            //output message to the console
            ESDWebServiceConstants.outputConsoleMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_PRODUCTS, ESDWebServiceConstants.ESD_ENDPOINT_METHOD_GET_END, esDocumentProducts.message);

            //serializes the ESD document into 
            return serializeESDocument(esDocumentProducts);
        }

        /// <summary>Obtains an Ecommerce Standards Document containing an array of price level records</summary>
        /// <returns>Serialized Ecommerce Standards Document in the JSON data format</returns>
        public System.ServiceModel.Channels.Message getPriceLevels()
        {
            ESDWebServiceConstants.outputConsoleMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_PRICE_LEVELS, ESDWebServiceConstants.ESD_ENDPOINT_METHOD_GET_START, "");

            //create Ecommerce standards document for storing records
            Dictionary<string, string> esDocumentConfigs = new Dictionary<string, string>();
            ESDocumentPriceLevel esDocumentPriceLevels = new ESDocumentPriceLevel(ESDocumentConstants.RESULT_ERROR_UNKNOWN, ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_PRICE_LEVELS, ESDocumentConstants.RESULT_ERROR_UNKNOWN), new ESDRecordPriceLevel[] { }, esDocumentConfigs);

            try
            {
                //check that data is allowed to be returned from the web service based on the incomming HTTP request headers
                if (checkRequestCredentials())
                {
                    //obtain record data
                    //This is where you would call a database or other data source to obtain and place data into the standards record objects
                    List<ESDRecordPriceLevel> records = new List<ESDRecordPriceLevel>();

                    //add record data to the document
                    ESDRecordPriceLevel record = new ESDRecordPriceLevel();
                    record.keyPriceLevelID = "1";
                    record.label = "Price Level 001";
                    records.Add(record);

                    record.keyPriceLevelID = "2";
                    record.label = "Price Level 002";
                    records.Add(record);

                    record.keyPriceLevelID = "3";
                    record.label = "Price Level 003";
                    records.Add(record);

                    //update the details of the document after all records have sucessfully been obtained
                    esDocumentPriceLevels.dataRecords = records.ToArray();
                    esDocumentPriceLevels.totalDataRecords = records.Count;
                    esDocumentPriceLevels.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_PRICE_LEVELS, ESDocumentConstants.RESULT_SUCCESS);
                    esDocumentPriceLevels.resultStatus = ESDocumentConstants.RESULT_SUCCESS;

                    // add a document config that specifies all of the record properties that may contain data in the document
                    // This properties indicate to other systems importing the document on which record data can be inserted or overwritten
                    esDocumentConfigs.Add("dataFields", "keyPriceLevelID,label");
                }
                else
                {
                    esDocumentPriceLevels.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_PRICE_LEVELS, ESDocumentConstants.RESULT_ERROR_CONNECTOR_INVALID_CREDENTIALS);
                    esDocumentPriceLevels.resultStatus = ESDocumentConstants.RESULT_ERROR_CONNECTOR_INVALID_CREDENTIALS;
                }
            }
            catch (Exception ex)
            {
                esDocumentPriceLevels.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_PRICE_LEVELS, ESDocumentConstants.RESULT_ERROR_CONNECTOR_PROCESSING);
                esDocumentPriceLevels.resultStatus = ESDocumentConstants.RESULT_ERROR_CONNECTOR_PROCESSING;
            }

            //output message to the console
            ESDWebServiceConstants.outputConsoleMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_PRICE_LEVELS, ESDWebServiceConstants.ESD_ENDPOINT_METHOD_GET_END, esDocumentPriceLevels.message);

            //serializes the ESD document into 
            return serializeESDocument(esDocumentPriceLevels);
        }

        /// <summary>Obtains an Ecommerce Standards Document containing an array of combination records associated to products</summary>
        /// <returns>Serialized Ecommerce Standards Document in the JSON data format</returns>
        public System.ServiceModel.Channels.Message getProductCombinations()
        {
            ESDWebServiceConstants.outputConsoleMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_PRODUCT_COMBINATIONS, ESDWebServiceConstants.ESD_ENDPOINT_METHOD_GET_START, "");

            //create Ecommerce standards document for storing records
            Dictionary<string, string> esDocumentConfigs = new Dictionary<string, string>();
            ESDocumentProductCombination esDocumentProductCombinations = new ESDocumentProductCombination(ESDocumentConstants.RESULT_ERROR_UNKNOWN, ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_PRODUCT_COMBINATIONS, ESDocumentConstants.RESULT_ERROR_UNKNOWN), new ESDRecordProductCombinationParent[] { }, new ESDRecordCombinationProfile[] {}, esDocumentConfigs);

            try
            {
                //check that data is allowed to be returned from the web service based on the incomming HTTP request headers
                if (checkRequestCredentials())
                {
                    //obtain record data
                    //This is where you would call a database or other data source to obtain and place data into the standards record objects

                    //obtain combination profile record data as well as associated fields and field values
                    List<ESDRecordCombinationProfile> combinationProfileRecords = new List<ESDRecordCombinationProfile>();
                    ESDRecordCombinationProfile combinationProfileRecord = new ESDRecordCombinationProfile();
                    combinationProfileRecord.keyComboProfileID = "PROF-456";
                    combinationProfileRecord.profileName = "Furniture Styling";
                    combinationProfileRecord.description = "Styling of Furniture Products";

                    List<ESDRecordCombinationProfileField> combinationProfileFieldRecords = new List<ESDRecordCombinationProfileField>();
                    ESDRecordCombinationProfileField combinationProfileFieldRecord = new ESDRecordCombinationProfileField();
                    combinationProfileFieldRecord.keyComboProfileFieldID = "PROF-456-2";
                    combinationProfileFieldRecord.fieldName = "Size";
                    combinationProfileFieldRecord.ordering = 2;
                    combinationProfileFieldRecord.fieldValues = new string[] {"Small", "Medium", "Large", "Jumbo"};
                    combinationProfileFieldRecord.fieldValueIDs = new string[] {"PROF-456-2-SM", "PROF-456-2-MED", "PROF-456-2-LARG", "PROF-456-2-JUM"};
                    combinationProfileFieldRecords.Add(combinationProfileFieldRecord);

                    combinationProfileFieldRecord = new ESDRecordCombinationProfileField();
                    combinationProfileFieldRecord.keyComboProfileFieldID = "PROF-456-4";
                    combinationProfileFieldRecord.fieldName = "Style";
                    combinationProfileFieldRecord.ordering = 1;
                    combinationProfileFieldRecord.fieldValues = new string[] { "Classic", "Modern", "Vintage", "Minimalist" };
                    combinationProfileFieldRecord.fieldValueIDs = new string[] { "COMBO-VAL-CL", "COMBO-VAL-MOD", "COMBO-VAL-VINT", "COMBO-VAL-MIN"};
                    combinationProfileFieldRecords.Add(combinationProfileFieldRecord);
                    combinationProfileRecord.combinationFields = combinationProfileFieldRecords.ToArray();
                    
                    //add parent combination record data to the document
                    List<ESDRecordProductCombinationParent> records = new List<ESDRecordProductCombinationParent>();
                    ESDRecordProductCombinationParent record = new ESDRecordProductCombinationParent();
                    record.keyProductID = "SOFTA-123";
                    record.keyComboProfileID = "PROF-456";

                    //assign child products to the parent product in the combination based on mapped field-values associated to the combination profile
                    List<ESDRecordProductCombination> combinationRecords = new List<ESDRecordProductCombination>();
                    ESDRecordProductCombination combinationRecord = new ESDRecordProductCombination();
                    combinationRecord.keyProductID = "SOFTA-123";
                    combinationRecord.keyComboProfileID = "PROF-456";
                    combinationRecord.fieldValueCombinations = new string[][]{ new string[] { "PROF-456-2", "PROF-456-2-SM" }, new string[] { "PROF-456-4", "COMBO-VAL-CL" }};
                    combinationRecords.Add(combinationRecord);

                    combinationRecord = new ESDRecordProductCombination();
                    combinationRecord.keyProductID = "SOFTA-456";
                    combinationRecord.keyComboProfileID = "PROF-456";
                    combinationRecord.fieldValueCombinations = new string[][] { new string[] { "PROF-456-2", "PROF-456-2-LARG" }, new string[] { "PROF-456-4", "COMBO-VAL-CL" }};
                    combinationRecords.Add(combinationRecord);

                    combinationRecord = new ESDRecordProductCombination();
                    combinationRecord.keyProductID = "SOFTA-098";
                    combinationRecord.keyComboProfileID = "PROF-456";
                    combinationRecord.fieldValueCombinations = new string[][] { new string[] { "PROF-456-2", "PROF-456-2-MED" }, new string[] { "PROF-456-4", "COMBO-VAL-CL" }};
                    combinationRecords.Add(combinationRecord);

                    record.productCombinations = combinationRecords.ToArray();
                    records.Add(record);

                    //update the details of the document after all records have sucessfully been obtained
                    esDocumentProductCombinations.combinationProfiles = combinationProfileRecords.ToArray();
                    esDocumentProductCombinations.dataRecords = records.ToArray();
                    esDocumentProductCombinations.totalDataRecords = records.Count;
                    esDocumentProductCombinations.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_PRODUCT_COMBINATIONS, ESDocumentConstants.RESULT_SUCCESS);
                    esDocumentProductCombinations.resultStatus = ESDocumentConstants.RESULT_SUCCESS;
                }
                else
                {
                    esDocumentProductCombinations.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_PRODUCT_COMBINATIONS, ESDocumentConstants.RESULT_ERROR_CONNECTOR_INVALID_CREDENTIALS);
                    esDocumentProductCombinations.resultStatus = ESDocumentConstants.RESULT_ERROR_CONNECTOR_INVALID_CREDENTIALS;
                }
            }
            catch (Exception ex)
            {
                esDocumentProductCombinations.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_PRODUCT_COMBINATIONS, ESDocumentConstants.RESULT_ERROR_CONNECTOR_PROCESSING);
                esDocumentProductCombinations.resultStatus = ESDocumentConstants.RESULT_ERROR_CONNECTOR_PROCESSING;
            }

            //output message to the console
            ESDWebServiceConstants.outputConsoleMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_PRODUCT_COMBINATIONS, ESDWebServiceConstants.ESD_ENDPOINT_METHOD_GET_END, esDocumentProductCombinations.message);

            //serializes the ESD document into 
            return serializeESDocument(esDocumentProductCombinations);
        }

        /// <summary>Obtains an Ecommerce Standards Document containing an array of product stock quantity records related to products. This is typically used to update stock quantities on a frequent basis</summary>
        /// <returns>Serialized Ecommerce Standards Document in the JSON data format</returns>
        public System.ServiceModel.Channels.Message getProductStockQuantities(string keyProductID, string obtainAll)
        {
            ESDWebServiceConstants.outputConsoleMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_PRODUCT_STOCK_QUANTITIES, ESDWebServiceConstants.ESD_ENDPOINT_METHOD_GET_START, "");

            //create Ecommerce standards document for storing records
            Dictionary<string, string> esDocumentConfigs = new Dictionary<string, string>();
            ESDocumentStockQuantity esDocumentStockQuantities = new ESDocumentStockQuantity(ESDocumentConstants.RESULT_ERROR_UNKNOWN, ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_PRODUCT_STOCK_QUANTITIES, ESDocumentConstants.RESULT_ERROR_UNKNOWN), new ESDRecordStockQuantity[] { }, esDocumentConfigs);

            try
            {
                //check that data is allowed to be returned from the web service based on the incomming HTTP request headers
                if (checkRequestCredentials())
                {
                    //obtain record data
                    //This is where you would call a database or other data source to obtain and place data into the standards record objects
                    List<ESDRecordStockQuantity> records = new List<ESDRecordStockQuantity>();

                    //add record data to the document
                    ESDRecordStockQuantity record = new ESDRecordStockQuantity();
                    record.keyProductID = "123A";
                    record.qtyAvailable = 22;
                    records.Add(record);

                    record = new ESDRecordStockQuantity();
                    record.keyProductID = "1234";
                    record.qtyAvailable = 16;
                    record.qtyOnHand = 20;
                    record.qtyOrdered = 15;
                    record.qtyBackordered = 10;
                    record.qtyReserved = 2;
                    record.qtyConsigned = 12;
                    records.Add(record);

                    record = new ESDRecordStockQuantity();
                    record.keyProductID = "7890";
                    record.qtyAvailable = -23;
                    record.qtyOnHand = 20;
                    record.qtyOrdered = 15;
                    records.Add(record);

                    //update the details of the document after all records have sucessfully been obtained
                    esDocumentStockQuantities.dataRecords = records.ToArray();
                    esDocumentStockQuantities.totalDataRecords = records.Count;
                    esDocumentStockQuantities.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_PRODUCT_STOCK_QUANTITIES, ESDocumentConstants.RESULT_SUCCESS);
                    esDocumentStockQuantities.resultStatus = ESDocumentConstants.RESULT_SUCCESS;

                    // add a document config that specifies all of the record properties that may contain data in the document
                    // This properties indicate to other systems importing the document on which record data can be inserted or overwritten
                    esDocumentConfigs.Add("dataFields", "keyProductID,qtyAvailable,qtyOnHand,qtyOrdered,qtyOrdered,qtyBackordered,qtyReserved,qtyConsigned");
                }
                else
                {
                    esDocumentStockQuantities.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_PRODUCT_STOCK_QUANTITIES, ESDocumentConstants.RESULT_ERROR_CONNECTOR_INVALID_CREDENTIALS);
                    esDocumentStockQuantities.resultStatus = ESDocumentConstants.RESULT_ERROR_CONNECTOR_INVALID_CREDENTIALS;
                }
            }
            catch (Exception ex)
            {
                esDocumentStockQuantities.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_PRODUCT_STOCK_QUANTITIES, ESDocumentConstants.RESULT_ERROR_CONNECTOR_PROCESSING);
                esDocumentStockQuantities.resultStatus = ESDocumentConstants.RESULT_ERROR_CONNECTOR_PROCESSING;
            }

            //output message to the console
            ESDWebServiceConstants.outputConsoleMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_PRODUCT_STOCK_QUANTITIES, ESDWebServiceConstants.ESD_ENDPOINT_METHOD_GET_END, esDocumentStockQuantities.message);

            //serializes the ESD document into 
            return serializeESDocument(esDocumentStockQuantities);
        }

        /// <summary>Obtains an Ecommerce Standards Document containing an array of purchaser records</summary>
        /// <returns>Serialized Ecommerce Standards Document in the JSON data format</returns>
        public System.ServiceModel.Channels.Message getPurchasers()
        {
            ESDWebServiceConstants.outputConsoleMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_PURCHASERS, ESDWebServiceConstants.ESD_ENDPOINT_METHOD_GET_START, "");

            //create Ecommerce standards document for storing records
            Dictionary<string, string> esDocumentConfigs = new Dictionary<string, string>();
            ESDocumentPurchaser esDocumentAlternateCodes = new ESDocumentPurchaser(ESDocumentConstants.RESULT_ERROR_UNKNOWN, ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_PURCHASERS, ESDocumentConstants.RESULT_ERROR_UNKNOWN), new ESDRecordPurchaser[] { }, esDocumentConfigs);

            try
            {
                //check that data is allowed to be returned from the web service based on the incomming HTTP request headers
                if (checkRequestCredentials())
                {
                    //obtain record data
                    //This is where you would call a database or other data source to obtain and place data into the standards record objects
                    List<ESDRecordPurchaser> records = new List<ESDRecordPurchaser>();

                    //add record data to the document
                    ESDRecordPurchaser record = new ESDRecordPurchaser();
                    record.keyPurchaserID = "PUR-2";
                    record.purchaserCode = "JD";
                    record.contact = "John Doe";
                    record.isIndividual = true;
                    records.Add(record);

                    record = new ESDRecordPurchaser();
                    record.keyPurchaserID = "4533";
                    record.purchaserCode = "AI";
                    record.name = "Acme Industries";
                    record.contact = "Kevin Peterson";
                    record.isIndividual = false;
                    records.Add(record);

                    //update the details of the document after all records have sucessfully been obtained
                    esDocumentAlternateCodes.dataRecords = records.ToArray();
                    esDocumentAlternateCodes.totalDataRecords = records.Count;
                    esDocumentAlternateCodes.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_PURCHASERS, ESDocumentConstants.RESULT_SUCCESS);
                    esDocumentAlternateCodes.resultStatus = ESDocumentConstants.RESULT_SUCCESS;

                    // add a document config that specifies all of the record properties that may contain data in the document
                    // This properties indicate to other systems importing the document on which record data can be inserted or overwritten
                    esDocumentConfigs.Add("dataFields", "keyPurchaserID,purchaserCode,name,contact,isIndividual");
                }
                else
                {
                    esDocumentAlternateCodes.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_PURCHASERS, ESDocumentConstants.RESULT_ERROR_CONNECTOR_INVALID_CREDENTIALS);
                    esDocumentAlternateCodes.resultStatus = ESDocumentConstants.RESULT_ERROR_CONNECTOR_INVALID_CREDENTIALS;
                }
            }
            catch (Exception ex)
            {
                esDocumentAlternateCodes.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_PURCHASERS, ESDocumentConstants.RESULT_ERROR_CONNECTOR_PROCESSING);
                esDocumentAlternateCodes.resultStatus = ESDocumentConstants.RESULT_ERROR_CONNECTOR_PROCESSING;
            }

            //output message to the console
            ESDWebServiceConstants.outputConsoleMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_PURCHASERS, ESDWebServiceConstants.ESD_ENDPOINT_METHOD_GET_END, esDocumentAlternateCodes.message);

            //serializes the ESD document into 
            return serializeESDocument(esDocumentAlternateCodes);
        }

        /// <summary>Obtains an Ecommerce Standards Document containing an array of sales representative records</summary>
        /// <returns>Serialized Ecommerce Standards Document in the JSON data format</returns>
        public System.ServiceModel.Channels.Message getSalesReps()
        {
            ESDWebServiceConstants.outputConsoleMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_SALESREPS, ESDWebServiceConstants.ESD_ENDPOINT_METHOD_GET_START, "");

            //create Ecommerce standards document for storing records
            Dictionary<string, string> esDocumentConfigs = new Dictionary<string, string>();
            ESDocumentSalesRep esDocumentSalesreps = new ESDocumentSalesRep(ESDocumentConstants.RESULT_ERROR_UNKNOWN, ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_SALESREPS, ESDocumentConstants.RESULT_ERROR_UNKNOWN), new ESDRecordSalesRep[] { }, esDocumentConfigs);

            try
            {
                //check that data is allowed to be returned from the web service based on the incomming HTTP request headers
                if (checkRequestCredentials())
                {
                    //obtain record data
                    //This is where you would call a database or other data source to obtain and place data into the standards record objects
                    List<ESDRecordSalesRep> records = new List<ESDRecordSalesRep>();

                    //add record data to the document
                    ESDRecordSalesRep record = new ESDRecordSalesRep();
                    record.keySalesRepID = "REP-2";
                    record.salesRepCode = "JD";
                    record.contact = "John Doe";
                    record.isIndividual = true;
                    records.Add(record);

                    record = new ESDRecordSalesRep();
                    record.keySalesRepID = "4533";
                    record.salesRepCode = "AI";
                    record.name = "Acme Industries";
                    record.contact = "Kevin Peterson";
                    record.isIndividual = false;
                    records.Add(record);

                    //update the details of the document after all records have sucessfully been obtained
                    esDocumentSalesreps.dataRecords = records.ToArray();
                    esDocumentSalesreps.totalDataRecords = records.Count;
                    esDocumentSalesreps.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_SALESREPS, ESDocumentConstants.RESULT_SUCCESS);
                    esDocumentSalesreps.resultStatus = ESDocumentConstants.RESULT_SUCCESS;

                    // add a document config that specifies all of the record properties that may contain data in the document
                    // This properties indicate to other systems importing the document on which record data can be inserted or overwritten
                    esDocumentConfigs.Add("dataFields", "keySalesRepID,name,salesRepCode,contact,isIndividual");
                }
                else
                {
                    esDocumentSalesreps.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_SALESREPS, ESDocumentConstants.RESULT_ERROR_CONNECTOR_INVALID_CREDENTIALS);
                    esDocumentSalesreps.resultStatus = ESDocumentConstants.RESULT_ERROR_CONNECTOR_INVALID_CREDENTIALS;
                }
            }
            catch (Exception ex)
            {
                esDocumentSalesreps.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_SALESREPS, ESDocumentConstants.RESULT_ERROR_CONNECTOR_PROCESSING);
                esDocumentSalesreps.resultStatus = ESDocumentConstants.RESULT_ERROR_CONNECTOR_PROCESSING;
            }

            //output message to the console
            ESDWebServiceConstants.outputConsoleMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_SALESREPS, ESDWebServiceConstants.ESD_ENDPOINT_METHOD_GET_END, esDocumentSalesreps.message);

            //serializes the ESD document into 
            return serializeESDocument(esDocumentSalesreps);
        }

        /// <summary>Obtains an Ecommerce Standards Document containing an array of sell unit records</summary>
        /// <returns>Serialized Ecommerce Standards Document in the JSON data format</returns>
        public System.ServiceModel.Channels.Message getSellUnits()
        {
            ESDWebServiceConstants.outputConsoleMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_SELL_UNITS, ESDWebServiceConstants.ESD_ENDPOINT_METHOD_GET_START, "");

            //create Ecommerce standards document for storing records
            Dictionary<string, string> esDocumentConfigs = new Dictionary<string, string>();
            ESDocumentSellUnit esDocumentSellUnits = new ESDocumentSellUnit(ESDocumentConstants.RESULT_ERROR_UNKNOWN, ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_SELL_UNITS, ESDocumentConstants.RESULT_ERROR_UNKNOWN), new ESDRecordSellUnit[] { }, esDocumentConfigs);

            try
            {
                //check that data is allowed to be returned from the web service based on the incomming HTTP request headers
                if (checkRequestCredentials())
                {
                    //obtain record data
                    //This is where you would call a database or other data source to obtain and place data into the standards record objects
                    List<ESDRecordSellUnit> records = new List<ESDRecordSellUnit>();

                    //add record data to the document
                    ESDRecordSellUnit record = new ESDRecordSellUnit();
                    record.keySellUnitID = "2";
                    record.sellUnitCode = "EA";
                    record.sellUnitLabel = "EACH";
                    record.isBaseUnit = ESDWebServiceConstants.ESD_VALUE_Y;
                    records.Add(record);

                    record.keySellUnitID = "3";
                    record.sellUnitCode = "PK";
                    record.sellUnitLabel = "Pack";
                    record.isBaseUnit = ESDWebServiceConstants.ESD_VALUE_N;
                    record.keySellUnitParentID = "2";
                    records.Add(record);

                    record.keySellUnitID = "4";
                    record.sellUnitCode = "CT";
                    record.sellUnitLabel = "Carton";
                    record.isBaseUnit = ESDWebServiceConstants.ESD_VALUE_N;
                    record.keySellUnitParentID = "3";
                    records.Add(record);

                    record.keySellUnitID = "5";
                    record.sellUnitCode = "CN";
                    record.sellUnitLabel = "Container";
                    record.isBaseUnit = ESDWebServiceConstants.ESD_VALUE_N;
                    record.keySellUnitParentID = "4";
                    records.Add(record);

                    record.keySellUnitID = "6";
                    record.sellUnitCode = "SHIP";
                    record.sellUnitLabel = "Ship Load";
                    record.isBaseUnit = ESDWebServiceConstants.ESD_VALUE_N;
                    record.keySellUnitParentID = "5";
                    records.Add(record);

                    record.keySellUnitID = "7";
                    record.sellUnitCode = "TRAIN";
                    record.sellUnitLabel = "Train Load";
                    record.isBaseUnit = ESDWebServiceConstants.ESD_VALUE_N;
                    record.keySellUnitParentID = "5";
                    records.Add(record);

                    record.keySellUnitID = "8";
                    record.sellUnitCode = "HOUR";
                    record.sellUnitLabel = "Hour";
                    record.isBaseUnit = ESDWebServiceConstants.ESD_VALUE_Y;
                    records.Add(record);

                    record.keySellUnitID = "9";
                    record.sellUnitCode = "LABOUR PACK";
                    record.sellUnitLabel = "Pack Of Labour Hours";
                    record.isBaseUnit = ESDWebServiceConstants.ESD_VALUE_N;
                    record.keySellUnitParentID = "8";
                    records.Add(record);

                    //update the details of the document after all records have sucessfully been obtained
                    esDocumentSellUnits.dataRecords = records.ToArray();
                    esDocumentSellUnits.totalDataRecords = records.Count;
                    esDocumentSellUnits.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_SELL_UNITS, ESDocumentConstants.RESULT_SUCCESS);
                    esDocumentSellUnits.resultStatus = ESDocumentConstants.RESULT_SUCCESS;

                    // add a document config that specifies all of the record properties that may contain data in the document
                    // This properties indicate to other systems importing the document on which record data can be inserted or overwritten
                    esDocumentConfigs.Add("dataFields", "keySellUnitID,sellUnitCode,sellUnitLabel,isBaseUnit,baseQuantity,parentQuantity,keySellUnitParentID");
                }
                else
                {
                    esDocumentSellUnits.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_SELL_UNITS, ESDocumentConstants.RESULT_ERROR_CONNECTOR_INVALID_CREDENTIALS);
                    esDocumentSellUnits.resultStatus = ESDocumentConstants.RESULT_ERROR_CONNECTOR_INVALID_CREDENTIALS;
                }
            }
            catch (Exception ex)
            {
                esDocumentSellUnits.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_SELL_UNITS, ESDocumentConstants.RESULT_ERROR_CONNECTOR_PROCESSING);
                esDocumentSellUnits.resultStatus = ESDocumentConstants.RESULT_ERROR_CONNECTOR_PROCESSING;
            }

            //output message to the console
            ESDWebServiceConstants.outputConsoleMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_SELL_UNITS, ESDWebServiceConstants.ESD_ENDPOINT_METHOD_GET_END, esDocumentSellUnits.message);

            //serializes the ESD document into 
            return serializeESDocument(esDocumentSellUnits);
        }

        /// <summary>Obtains an Ecommerce Standards Document containing an array of supplier account records</summary>
        /// <returns>Serialized Ecommerce Standards Document in the JSON data format</returns>
        public System.ServiceModel.Channels.Message getSupplierAccounts()
        {
            ESDWebServiceConstants.outputConsoleMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_SUPPLIER_ACCOUNTS, ESDWebServiceConstants.ESD_ENDPOINT_METHOD_GET_START, "");

            //create Ecommerce standards document for storing records
            Dictionary<string, string> esDocumentConfigs = new Dictionary<string, string>();
            ESDocumentSupplierAccount esDocumentSupplierAccounts = new ESDocumentSupplierAccount(ESDocumentConstants.RESULT_ERROR_UNKNOWN, ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_SUPPLIER_ACCOUNTS, ESDocumentConstants.RESULT_ERROR_UNKNOWN), new ESDRecordSupplierAccount[] { }, esDocumentConfigs);

            try
            {
                //check that data is allowed to be returned from the web service based on the incomming HTTP request headers
                if (checkRequestCredentials())
                {
                    //obtain record data
                    //This is where you would call a database or other data source to obtain and place data into the standards record objects
                    List<ESDRecordSupplierAccount> records = new List<ESDRecordSupplierAccount>();

                    //add record data to the document
                    ESDRecordSupplierAccount record = new ESDRecordSupplierAccount();
                    record.keySupplierAccountID = "SUP2";
                    record.supplierAccountCode = "SUPL004";
                    record.accountName = "Suppliers Inc.";
                    record.contactName = "John Smith";
                    record.orgName = "Suppliers Inc.";
                    record.authorityNumbers = new string[] { "2342342334", "3432424424243" };
                    record.authorityNumberLabels = new string[] { "Australian Business Number", "Australian Company Number" };
                    record.authorityNumberTypes = new int[] { ESDocumentConstants.AUTHORITY_NUM_AUS_ABN, ESDocumentConstants.AUTHORITY_NUM_AUS_ACN };
                    record.email = "js@esdstandards.somewhere";
                    record.accountClass = "Primary";
                    record.keyPurchaserID = "2";
                    record.territory = "Melb Rural";
                    record.shippingMethod = "Truck";
                    record.isOnHold = ESDWebServiceConstants.ESD_VALUE_N;
                    record.isOutsideBalance = ESDWebServiceConstants.ESD_VALUE_N;
                    record.isOutsideTerms = ESDWebServiceConstants.ESD_VALUE_N;
                    record.balance = (decimal)1000.00;
                    record.limit = 0;
                    record.termsType = ESDWebServiceConstants.ESD_ACCOUNT_TERMS_GND;
                    record.termsDescription = "Pay within 14 days of purchase";
                    record.termsValue1 = "14";
                    record.termsValue2 = "";
                    records.Add(record);

                    //update the details of the document after all records have sucessfully been obtained
                    esDocumentSupplierAccounts.dataRecords = records.ToArray();
                    esDocumentSupplierAccounts.totalDataRecords = records.Count;
                    esDocumentSupplierAccounts.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_SUPPLIER_ACCOUNTS, ESDocumentConstants.RESULT_SUCCESS);
                    esDocumentSupplierAccounts.resultStatus = ESDocumentConstants.RESULT_SUCCESS;

                    // add a document config that specifies all of the record properties that may contain data in the document
                    // This properties indicate to other systems importing the document on which record data can be inserted or overwritten
                    esDocumentConfigs.Add("dataFields", "keySupplierAccountID,supplierAccountCode,accountName,contactName,orgName,authorityNumbers,authorityNumberLabels,authorityNumberTypes,email,accountClass,territory,shippingMethod,isOnHold,isOutsideBalance,isOutsideTerms,balance,limit,termsType,termsDescription,termsValue1,termsValue2");
                }
                else
                {
                    esDocumentSupplierAccounts.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_SUPPLIER_ACCOUNTS, ESDocumentConstants.RESULT_ERROR_CONNECTOR_INVALID_CREDENTIALS);
                    esDocumentSupplierAccounts.resultStatus = ESDocumentConstants.RESULT_ERROR_CONNECTOR_INVALID_CREDENTIALS;
                }
            }
            catch (Exception ex)
            {
                esDocumentSupplierAccounts.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_SUPPLIER_ACCOUNTS, ESDocumentConstants.RESULT_ERROR_CONNECTOR_PROCESSING);
                esDocumentSupplierAccounts.resultStatus = ESDocumentConstants.RESULT_ERROR_CONNECTOR_PROCESSING;
            }

            //output message to the console
            ESDWebServiceConstants.outputConsoleMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_SUPPLIER_ACCOUNTS, ESDWebServiceConstants.ESD_ENDPOINT_METHOD_GET_END, esDocumentSupplierAccounts.message);

            //serializes the ESD document into 
            return serializeESDocument(esDocumentSupplierAccounts);
        }

        /// <summary>Obtains an Ecommerce Standards Document containing an array of address records associated to supplier accounts</summary>
        /// <returns>Serialized Ecommerce Standards Document in the JSON data format</returns>
        public System.ServiceModel.Channels.Message getSupplierAccountAddresses()
        {
            ESDWebServiceConstants.outputConsoleMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_SUPPLIER_ACCOUNT_ADDRESSES, ESDWebServiceConstants.ESD_ENDPOINT_METHOD_GET_START, "");

            //create Ecommerce standards document for storing records
            Dictionary<string, string> esDocumentConfigs = new Dictionary<string, string>();
            ESDocumentSupplierAccountAddress esDocumentSupplierAccountAddresses = new ESDocumentSupplierAccountAddress(ESDocumentConstants.RESULT_ERROR_UNKNOWN, ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_SUPPLIER_ACCOUNT_ADDRESSES, ESDocumentConstants.RESULT_ERROR_UNKNOWN), new ESDRecordSupplierAccountAddress[] { }, esDocumentConfigs);

            try
            {
                //check that data is allowed to be returned from the web service based on the incomming HTTP request headers
                if (checkRequestCredentials())
                {
                    //obtain record data
                    //This is where you would call a database or other data source to obtain and place data into the standards record objects
                    List<ESDRecordSupplierAccountAddress> records = new List<ESDRecordSupplierAccountAddress>();

                    //add record data to the document
                    ESDRecordSupplierAccountAddress record = new ESDRecordSupplierAccountAddress();
                    record.keyAddressID = "ADD002";
                    record.keySupplierAccountID = "10";
                    record.description = "Billing Address";
                    record.orgName = "ESD Industries";
                    record.contact = "John Doe";
                    record.phone = "+61234567890";
                    record.fax = "+612345678901";
                    record.address1 = "22";
                    record.address2 = "Bourkie Street";
                    record.address3 = "Melbourne";
                    record.region = "Victoria";
                    record.country = "Australia";
                    record.postcode = "3000";
                    record.freightCode = "01FRE";
                    record.isPrimary = ESDWebServiceConstants.ESD_VALUE_Y;
                    record.isDelivery = ESDWebServiceConstants.ESD_VALUE_N;
                    records.Add(record);

                    //update the details of the document after all records have sucessfully been obtained
                    esDocumentSupplierAccountAddresses.dataRecords = records.ToArray();
                    esDocumentSupplierAccountAddresses.totalDataRecords = records.Count;
                    esDocumentSupplierAccountAddresses.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_SUPPLIER_ACCOUNT_ADDRESSES, ESDocumentConstants.RESULT_SUCCESS);
                    esDocumentSupplierAccountAddresses.resultStatus = ESDocumentConstants.RESULT_SUCCESS;

                    // add a document config that specifies all of the record properties that may contain data in the document
                    // This properties indicate to other systems importing the document on which record data can be inserted or overwritten
                    esDocumentConfigs.Add("dataFields", "keyAddressID,keySupplierAccountID,description,orgName,contact,phone,fax,address1,address2,address3,region,country,postcode,freightCode,isPrimary,isDelivery");
                }
                else
                {
                    esDocumentSupplierAccountAddresses.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_SUPPLIER_ACCOUNT_ADDRESSES, ESDocumentConstants.RESULT_ERROR_CONNECTOR_INVALID_CREDENTIALS);
                    esDocumentSupplierAccountAddresses.resultStatus = ESDocumentConstants.RESULT_ERROR_CONNECTOR_INVALID_CREDENTIALS;
                }
            }
            catch (Exception ex)
            {
                esDocumentSupplierAccountAddresses.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_SUPPLIER_ACCOUNT_ADDRESSES, ESDocumentConstants.RESULT_ERROR_CONNECTOR_PROCESSING);
                esDocumentSupplierAccountAddresses.resultStatus = ESDocumentConstants.RESULT_ERROR_CONNECTOR_PROCESSING;
            }

            //output message to the console
            ESDWebServiceConstants.outputConsoleMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_SUPPLIER_ACCOUNT_ADDRESSES, ESDWebServiceConstants.ESD_ENDPOINT_METHOD_GET_END, esDocumentSupplierAccountAddresses.message);

            //serializes the ESD document into 
            return serializeESDocument(esDocumentSupplierAccountAddresses);
        }

        /// <summary>Obtains an Ecommerce Standards Document containing an array of surcharge records</summary>
        /// <returns>Serialized Ecommerce Standards Document in the JSON data format</returns>
        public System.ServiceModel.Channels.Message getSurcharges()
        {
            ESDWebServiceConstants.outputConsoleMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_SURCHARGES, ESDWebServiceConstants.ESD_ENDPOINT_METHOD_GET_START, "");

            //create Ecommerce standards document for storing records
            Dictionary<string, string> esDocumentConfigs = new Dictionary<string, string>();
            ESDocumentSurcharge esDocumentSurcharge = new ESDocumentSurcharge(ESDocumentConstants.RESULT_ERROR_UNKNOWN, ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_SURCHARGES, ESDocumentConstants.RESULT_ERROR_UNKNOWN), new ESDRecordSurcharge[] { }, esDocumentConfigs);

            try
            {
                //check that data is allowed to be returned from the web service based on the incomming HTTP request headers
                if (checkRequestCredentials())
                {
                    //obtain record data
                    //This is where you would call a database or other data source to obtain and place data into the standards record objects
                    List<ESDRecordSurcharge> records = new List<ESDRecordSurcharge>();

                    //add record data to the document
                    ESDRecordSurcharge record = new ESDRecordSurcharge();
                    record.keySurchargeID = "456";
                    record.surchargeCode = "WEB_FREIGHT";
                    record.surchargeLabel = "Web Order Freight";
                    record.description = "Costs to deliver orders created through the website.";
                    record.surchargeType = ESDocumentConstants.SURCHARGE_TYPE_FREIGHT;
                    records.Add(record);

                    record = new ESDRecordSurcharge();
                    record.keySurchargeID = "765";
                    record.surchargeCode = "WEB_CC_SURCHARGE";
                    record.surchargeLabel = "Web Credit Card Surcharge";
                    record.description = "Transaction cost for payment made by credit card.";
                    record.surchargeType = ESDocumentConstants.SURCHARGE_TYPE_CREDIT_CARD;
                    records.Add(record);

                    record = new ESDRecordSurcharge();
                    record.keySurchargeID = "6765";
                    record.surchargeCode = "WEB_MIN_ORDER";
                    record.surchargeLabel = "Web Minimum Order Surcharge";
                    record.description = "Cost to handle orders when an order's total price is under the allowed minimum.";
                    record.surchargeType = ESDocumentConstants.SURCHARGE_TYPE_MIN_ORDER;
                    records.Add(record);

                    //update the details of the document after all records have sucessfully been obtained
                    esDocumentSurcharge.dataRecords = records.ToArray();
                    esDocumentSurcharge.totalDataRecords = records.Count;
                    esDocumentSurcharge.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_SURCHARGES, ESDocumentConstants.RESULT_SUCCESS);
                    esDocumentSurcharge.resultStatus = ESDocumentConstants.RESULT_SUCCESS;

                    // add a document config that specifies all of the record properties that may contain data in the document
                    // This properties indicate to other systems importing the document on which record data can be inserted or overwritten
                    esDocumentConfigs.Add("dataFields", "keySurchargeID,surchargeCode,surchargeLabel,description,surchargeType");
                }
                else
                {
                    esDocumentSurcharge.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_SURCHARGES, ESDocumentConstants.RESULT_ERROR_CONNECTOR_INVALID_CREDENTIALS);
                    esDocumentSurcharge.resultStatus = ESDocumentConstants.RESULT_ERROR_CONNECTOR_INVALID_CREDENTIALS;
                }
            }
            catch (Exception ex)
            {
                esDocumentSurcharge.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_SURCHARGES, ESDocumentConstants.RESULT_ERROR_CONNECTOR_PROCESSING);
                esDocumentSurcharge.resultStatus = ESDocumentConstants.RESULT_ERROR_CONNECTOR_PROCESSING;
            }

            //output message to the console
            ESDWebServiceConstants.outputConsoleMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_SURCHARGES, ESDWebServiceConstants.ESD_ENDPOINT_METHOD_GET_END, esDocumentSurcharge.message);

            //serializes the ESD document into 
            return serializeESDocument(esDocumentSurcharge);
        }

        /// <summary>Obtains an Ecommerce Standards Document containing an array of taxcode records</summary>
        /// <returns>Serialized Ecommerce Standards Document in the JSON data format</returns>
        public System.ServiceModel.Channels.Message getTaxcodes()
        {
            ESDWebServiceConstants.outputConsoleMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_TAXCODDES, ESDWebServiceConstants.ESD_ENDPOINT_METHOD_GET_START, "");

            //create Ecommerce standards document for storing records
            Dictionary<string, string> esDocumentConfigs = new Dictionary<string, string>();
            ESDocumentTaxcode esDocumentTaxcode = new ESDocumentTaxcode(ESDocumentConstants.RESULT_ERROR_UNKNOWN, ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_TAXCODDES, ESDocumentConstants.RESULT_ERROR_UNKNOWN), new ESDRecordTaxcode[] { }, esDocumentConfigs);

            try
            {
                //check that data is allowed to be returned from the web service based on the incomming HTTP request headers
                if (checkRequestCredentials())
                {
                    //obtain record data
                    //This is where you would call a database or other data source to obtain and place data into the standards record objects
                    List<ESDRecordTaxcode> records = new List<ESDRecordTaxcode>();

                    //add record data to the document
                    ESDRecordTaxcode record = new ESDRecordTaxcode();
                    record.keyTaxcodeID = "456";
                    record.taxcode = "GST";
                    record.taxcodeLabel = "Goods And Services Tax";
                    record.description = "Australian Goods And Services Tax";
                    record.taxcodePercentageRate = 10;
                    records.Add(record);

                    record.keyTaxcodeID = "765";
                    record.taxcode = "WINE";
                    record.taxcodeLabel = "Wine Tax";
                    record.taxcodePercentageRate = 15;
                    records.Add(record);

                    record.keyTaxcodeID = "6765";
                    record.taxcode = "FREE";
                    record.taxcodeLabel = "Tax Free";
                    record.taxcodePercentageRate = 0;
                    records.Add(record);

                    //update the details of the document after all records have sucessfully been obtained
                    esDocumentTaxcode.dataRecords = records.ToArray();
                    esDocumentTaxcode.totalDataRecords = records.Count;
                    esDocumentTaxcode.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_TAXCODDES, ESDocumentConstants.RESULT_SUCCESS);
                    esDocumentTaxcode.resultStatus = ESDocumentConstants.RESULT_SUCCESS;

                    // add a document config that specifies all of the record properties that may contain data in the document
                    // This properties indicate to other systems importing the document on which record data can be inserted or overwritten
                    esDocumentConfigs.Add("dataFields", "alternateCode,isSupplierCode,isUseCode,keyProductID");
                }
                else
                {
                    esDocumentTaxcode.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_TAXCODDES, ESDocumentConstants.RESULT_ERROR_CONNECTOR_INVALID_CREDENTIALS);
                    esDocumentTaxcode.resultStatus = ESDocumentConstants.RESULT_ERROR_CONNECTOR_INVALID_CREDENTIALS;
                }
            }
            catch (Exception ex)
            {
                esDocumentTaxcode.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_TAXCODDES, ESDocumentConstants.RESULT_ERROR_CONNECTOR_PROCESSING);
                esDocumentTaxcode.resultStatus = ESDocumentConstants.RESULT_ERROR_CONNECTOR_PROCESSING;
            }

            //output message to the console
            ESDWebServiceConstants.outputConsoleMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_TAXCODDES, ESDWebServiceConstants.ESD_ENDPOINT_METHOD_GET_END, esDocumentTaxcode.message);

            //serializes the ESD document into 
            return serializeESDocument(esDocumentTaxcode);
        }

        /// <summary>Obtains an Ecommerce Standards Document containing an the status of the webservice</summary>
        /// <returns>Serialized Ecommerce Standards Document in the JSON data format</returns>
        public System.ServiceModel.Channels.Message getWebserviceStatus()
        {
            ESDWebServiceConstants.outputConsoleMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_WEB_SERVICE_STATUS, ESDWebServiceConstants.ESD_ENDPOINT_METHOD_GET_START, "");

            //create Ecommerce standards document for storing records
            ESDocument esDocument = new ESDocument(ESDocumentConstants.RESULT_ERROR_UNKNOWN, ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_WEB_SERVICE_STATUS, ESDocumentConstants.RESULT_ERROR_UNKNOWN));

            try
            {
                //check that data is allowed to be returned from the web service based on the incomming HTTP request headers
                if (checkRequestCredentials())
                {
                    esDocument.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_WEB_SERVICE_STATUS, ESDocumentConstants.RESULT_SUCCESS);
                    esDocument.resultStatus = ESDocumentConstants.RESULT_SUCCESS;
                }
                else
                {
                    esDocument.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_WEB_SERVICE_STATUS, ESDocumentConstants.RESULT_ERROR_CONNECTOR_INVALID_CREDENTIALS);
                    esDocument.resultStatus = ESDocumentConstants.RESULT_ERROR_CONNECTOR_INVALID_CREDENTIALS;
                }
            }
            catch (Exception ex)
            {
                esDocument.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_WEB_SERVICE_STATUS, ESDocumentConstants.RESULT_ERROR_CONNECTOR_PROCESSING);
                esDocument.resultStatus = ESDocumentConstants.RESULT_ERROR_CONNECTOR_PROCESSING;
            }

            //output message to the console
            ESDWebServiceConstants.outputConsoleMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_WEB_SERVICE_STATUS, ESDWebServiceConstants.ESD_ENDPOINT_METHOD_GET_END, esDocument.message);

            //serializes the ESD document into 
            return serializeESDocument(esDocument);
        }

        //HTTP POST methods to read in data and process it
        /// <summary>Imports an array of sales order records and returns the results of trying to import the sales orders</summary>
        /// <returns>Serialized Ecommerce Standards Document in the JSON data format</returns>
        public System.ServiceModel.Channels.Message importSalesOrders()
        {
            ESDWebServiceConstants.outputConsoleMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_ORDER_SALES, ESDWebServiceConstants.ESD_ENDPOINT_METHOD_POST_START, "");

            //create Ecommerce standards document for storing records
            ESDocument esDocument = new ESDocument(ESDocumentConstants.RESULT_ERROR_UNKNOWN, ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_ORDER_SALES, ESDocumentConstants.RESULT_ERROR_UNKNOWN));

            try
            {
                //check that data is allowed to be returned from the web service based on the incomming HTTP request headers
                if (checkRequestCredentials())
                {
                    //parse the sales order data passed in the body of the HTTP POST request and convert into a sales order standards document
                    ESDocumentOrderSale esDocumentSalesOrder = (ESDocumentOrderSale)new DataContractJsonSerializer(typeof(ESDocumentOrderSale)).ReadObject(OperationContext.Current.RequestContext.RequestMessage.GetReaderAtBodyContents());

                    //iterate through each sales order within the Ecommerce Standards document
                    foreach (ESDRecordOrderSale orderSaleRecord in esDocumentSalesOrder.dataRecords)
                    {
                        //remove any null values from the order's properties if the deserializer could not set any values
                        orderSaleRecord.setDefaultValuesForNullMembers();

                        //do processing for each sales order, such as importing it into a database or other data source
                    }

                    //update the details of the document after all records have sucessfully been obtained
                    esDocument.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_ORDER_SALES, ESDocumentConstants.RESULT_SUCCESS);
                    esDocument.resultStatus = ESDocumentConstants.RESULT_SUCCESS;
                }
                else
                {
                    esDocument.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_ORDER_SALES, ESDocumentConstants.RESULT_ERROR_CONNECTOR_INVALID_CREDENTIALS);
                    esDocument.resultStatus = ESDocumentConstants.RESULT_ERROR_CONNECTOR_INVALID_CREDENTIALS;
                }
            }
            catch (Exception ex)
            {
                esDocument.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_ORDER_SALES, ESDocumentConstants.RESULT_ERROR_CONNECTOR_PROCESSING);
                esDocument.resultStatus = ESDocumentConstants.RESULT_ERROR_CONNECTOR_PROCESSING;
            }

            //output message to the console
            ESDWebServiceConstants.outputConsoleMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_ORDER_SALES, ESDWebServiceConstants.ESD_ENDPOINT_METHOD_POST_END, esDocument.message);

            //serializes the ESD document into 
            return serializeESDocument(esDocument);
        }

        /// <summary>Imports an array of purchase order records and returns the results of trying to import the purchase orders</summary>
        /// <returns>Serialized Ecommerce Standards Document in the JSON data format</returns>
        public System.ServiceModel.Channels.Message importPurchaseOrders()
        {
            ESDWebServiceConstants.outputConsoleMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_ORDER_PURCHASES, ESDWebServiceConstants.ESD_ENDPOINT_METHOD_POST_START, "");

            //create Ecommerce standards document for storing records
            ESDocument esDocument = new ESDocument(ESDocumentConstants.RESULT_ERROR_UNKNOWN, ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_ORDER_PURCHASES, ESDocumentConstants.RESULT_ERROR_UNKNOWN));

            try
            {
                //check that data is allowed to be returned from the web service based on the incomming HTTP request headers
                if (checkRequestCredentials())
                {
                    //parse the purchase order data passed in the body of the HTTP POST request and convert into a purchase order standards document
                    ESDocumentOrderPurchase esDocumentPurchaseOrder = (ESDocumentOrderPurchase)new DataContractJsonSerializer(typeof(ESDocumentOrderPurchase)).ReadObject(OperationContext.Current.RequestContext.RequestMessage.GetReaderAtBodyContents());

                    //iterate through each purchase order within the Ecommerce Standards document
                    foreach (ESDRecordOrderPurchase orderPurchaseRecord in esDocumentPurchaseOrder.dataRecords)
                    {
                        //remove any null values from the order's properties if the deserializer could not set any values
                        orderPurchaseRecord.setDefaultValuesForNullMembers();

                        //do processing for each purchase order, such as importing it into a database or other data source
                        //other steps may be to create a supplier account if one has not been found
                    }

                    //update the details of the document after all records have sucessfully been obtained
                    esDocument.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_ORDER_PURCHASES, ESDocumentConstants.RESULT_SUCCESS);
                    esDocument.resultStatus = ESDocumentConstants.RESULT_SUCCESS;
                }
                else
                {
                    esDocument.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_ORDER_PURCHASES, ESDocumentConstants.RESULT_ERROR_CONNECTOR_INVALID_CREDENTIALS);
                    esDocument.resultStatus = ESDocumentConstants.RESULT_ERROR_CONNECTOR_INVALID_CREDENTIALS;
                }
            }
            catch (Exception ex)
            {
                esDocument.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_ORDER_PURCHASES, ESDocumentConstants.RESULT_ERROR_CONNECTOR_PROCESSING);
                esDocument.resultStatus = ESDocumentConstants.RESULT_ERROR_CONNECTOR_PROCESSING;
            }

            //output message to the console
            ESDWebServiceConstants.outputConsoleMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_ORDER_PURCHASES, ESDWebServiceConstants.ESD_ENDPOINT_METHOD_POST_END, esDocument.message);

            //serializes the ESD document into 
            return serializeESDocument(esDocument);
        }

        /// <summary>Imports an array of customer account records and returns the results of trying to import the customer accounts</summary>
        /// <returns>Serialized Ecommerce Standards Document in the JSON data format</returns>
        public System.ServiceModel.Channels.Message importCustomerAccounts()
        {
            ESDWebServiceConstants.outputConsoleMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_CUSTOMER_ACCOUNTS, ESDWebServiceConstants.ESD_ENDPOINT_METHOD_POST_START, "");

            //create Ecommerce standards document for storing records
            ESDocument esDocument = new ESDocument(ESDocumentConstants.RESULT_ERROR_UNKNOWN, ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_CUSTOMER_ACCOUNTS, ESDocumentConstants.RESULT_ERROR_UNKNOWN));

            try
            {
                //check that data is allowed to be returned from the web service based on the incomming HTTP request headers
                if (checkRequestCredentials())
                {
                    //parse the customer account data passed in the body of the HTTP POST request and convert into a customer accounts standards document
                    ESDocumentCustomerAccount esDocumentCustomerAccount = (ESDocumentCustomerAccount)new DataContractJsonSerializer(typeof(ESDocumentCustomerAccount)).ReadObject(OperationContext.Current.RequestContext.RequestMessage.GetReaderAtBodyContents());

                    //iterate through each customer account within the Ecommerce Standards document
                    foreach (ESDRecordCustomerAccount customerAccountRecord in esDocumentCustomerAccount.dataRecords)
                    {
                        //do processing for each customer account, such as importing it into a database or other data source
                    }

                    //update the details of the document after all records have sucessfully been obtained
                    esDocument.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_CUSTOMER_ACCOUNTS, ESDocumentConstants.RESULT_SUCCESS);
                    esDocument.resultStatus = ESDocumentConstants.RESULT_SUCCESS;
                }
                else
                {
                    esDocument.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_CUSTOMER_ACCOUNTS, ESDocumentConstants.RESULT_ERROR_CONNECTOR_INVALID_CREDENTIALS);
                    esDocument.resultStatus = ESDocumentConstants.RESULT_ERROR_CONNECTOR_INVALID_CREDENTIALS;
                }
            }
            catch (Exception ex)
            {
                esDocument.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_CUSTOMER_ACCOUNTS, ESDocumentConstants.RESULT_ERROR_CONNECTOR_PROCESSING);
                esDocument.resultStatus = ESDocumentConstants.RESULT_ERROR_CONNECTOR_PROCESSING;
            }

            //output message to the console
            ESDWebServiceConstants.outputConsoleMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_CUSTOMER_ACCOUNTS, ESDWebServiceConstants.ESD_ENDPOINT_METHOD_POST_END, esDocument.message);

            //serializes the ESD document into 
            return serializeESDocument(esDocument);
        }

        /// <summary>Imports an array of customer account payment records and returns the results of trying to import the customer account payments</summary>
        /// <returns>Serialized Ecommerce Standards Document in the JSON data format</returns>
        public System.ServiceModel.Channels.Message importCustomerAccountPayment()
        {
            ESDWebServiceConstants.outputConsoleMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_CUSTOMER_ACCOUNT_PAYMENTS, ESDWebServiceConstants.ESD_ENDPOINT_METHOD_POST_START, "");

            //create Ecommerce standards document for storing records
            ESDocument esDocument = new ESDocument(ESDocumentConstants.RESULT_ERROR_UNKNOWN, ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_CUSTOMER_ACCOUNT_PAYMENTS, ESDocumentConstants.RESULT_ERROR_UNKNOWN));

            try
            {
                //check that data is allowed to be returned from the web service based on the incomming HTTP request headers
                if (checkRequestCredentials())
                {
                    //parse the customer account payment data passed in the body of the HTTP POST request and convert into a customer account payments standards document
                    ESDocumentCustomerAccountPayment esDocumentCustomerAccountPayment = (ESDocumentCustomerAccountPayment)new DataContractJsonSerializer(typeof(ESDocumentCustomerAccountPayment)).ReadObject(OperationContext.Current.RequestContext.RequestMessage.GetReaderAtBodyContents());

                    //iterate through each customer account payment within the Ecommerce Standards document
                    foreach (ESDRecordCustomerAccountPayment customerAccountPaymentRecord in esDocumentCustomerAccountPayment.dataRecords)
                    {
                        //remove any null values from the payment record's properties if the deserializer could not set any values
                        customerAccountPaymentRecord.setDefaultValuesForNullMembers();

                        //do processing for each customer account payment, such as importing it into a database or other data source
                    }

                    //update the details of the document after all records have sucessfully been obtained
                    esDocument.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_CUSTOMER_ACCOUNT_PAYMENTS, ESDocumentConstants.RESULT_SUCCESS);
                    esDocument.resultStatus = ESDocumentConstants.RESULT_SUCCESS;
                }
                else
                {
                    esDocument.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_CUSTOMER_ACCOUNT_PAYMENTS, ESDocumentConstants.RESULT_ERROR_CONNECTOR_INVALID_CREDENTIALS);
                    esDocument.resultStatus = ESDocumentConstants.RESULT_ERROR_CONNECTOR_INVALID_CREDENTIALS;
                }
            }
            catch (Exception ex)
            {
                esDocument.message = ESDWebServiceConstants.getESDocumentMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_CUSTOMER_ACCOUNT_PAYMENTS, ESDocumentConstants.RESULT_ERROR_CONNECTOR_PROCESSING);
                esDocument.resultStatus = ESDocumentConstants.RESULT_ERROR_CONNECTOR_PROCESSING;
            }

            //output message to the console
            ESDWebServiceConstants.outputConsoleMessage(ESDWebServiceConstants.ESD_ENDPOINT_ID_CUSTOMER_ACCOUNT_PAYMENTS, ESDWebServiceConstants.ESD_ENDPOINT_METHOD_POST_END, esDocument.message);

            //serializes the ESD document into 
            return serializeESDocument(esDocument);
        }

        /// <summary>Checks the headers of an incomming HTTP request to see if the correct user name and password credentials have been given HTTP basic authentication headers</summary>
        /// <returns>true if the the credentials are valid, or are not required to be checked</returns>
        private static bool checkRequestCredentials()
        {
            bool requestValid = false;

            //check the credentials if the setting is configured to require it
            if (ESDWebServiceSettings.WEBSERVICE_REQUIRES_AUTHENTICATION)
            {
                try
                {
                    //obtain all the headers in the HTTP request
                    IncomingWebRequestContext request = WebOperationContext.Current.IncomingRequest;
                    WebHeaderCollection headers = request.Headers;

                    //check that headers exist
                    if (headers.HasKeys())
                    {
                        //obtain the authorization header that should contain encoded user name and password credentials
                        string authorizationCredentials = headers.Get("Authorization");

                        //check that the credentials match the settings configured for the webservice
                        requestValid = ESDWebServiceSettings.checkRequestCredentials(authorizationCredentials);
                    }
                }
                catch
                { }
            }
            else
            {
                requestValid = true;
            }

            return requestValid;
        }

        /// <summary>
        ///  Serializes a Ecommerce Standards Document into a stream of JSON data. That JSON data is returned in a WCF (Windows Communication Library) message object, which would typically be set in the HTTP response to a web service endpoint GET request.
        ///  In this example the document is being serialized using the JSON.net open source library, which is included in the Ecommerce Standards Library.
        /// </summary>
        /// <param name="esDocument">Ecommerce Standards Document containing record data for the relevent document.</param>
        /// <returns>WCF message containing the serialised JSON data.</returns>
        private System.ServiceModel.Channels.Message serializeESDocument(ESDocument esDocument)
        {
            string jsonSerialized = "";

            //Set HTTP response header to denote that the data will be GZipped. Note that the actual compression would occur outside this method.
            WebOperationContext.Current.OutgoingResponse.Headers[HttpResponseHeader.ContentEncoding] = ESDWebServiceConstants.ESD_ENDPOINT_RESPONSE_COMPRESSION_GZIP;

            //serialise the Ecommerce Standards Document into JSON
            try
            {
                //Create JSON.NET serializer that will ignore including JSON properties that contain empty values
                JsonSerializer serializer = new JsonSerializer();
                serializer.NullValueHandling = NullValueHandling.Ignore;
                serializer.MissingMemberHandling = MissingMemberHandling.Ignore;
                jsonSerialized = JsonConvert.SerializeObject(esDocument);
            }
            catch
            {
                jsonSerialized = ESDWebServiceConstants.ESD_ENDPOINT_FAILURE_JSON;
            }

            //Place the serialised JSON data into a memory stream
            MemoryStream resultBody = new MemoryStream(new UTF8Encoding().GetBytes(jsonSerialized));
            resultBody.Position = 0;

            //Resurn the serialised JSON data into WCF's response message, which can allow it to be included in the body of a HTTP response from a web service RESTful endpoint request
            return WebOperationContext.Current.CreateStreamResponse(resultBody, ESDWebServiceConstants.ESD_ENDPOINT_CONTENT_TYPE_JSON);
        }
    }
}
