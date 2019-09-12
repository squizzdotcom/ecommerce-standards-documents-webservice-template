# Ecommerce Standards Documents .Net Webservice Template
This repository contains an out-of-the-box programming example on how to develop a RESTful webservice that returns or processes [Ecommerce Standards Documents](https://github.com/squizzdotcom/ecommerce-standards-documents-dotnet-library) (ESD) built on the DotNet platform. 
This template webservice provides an example on how to integrate an ESD webservice into any business system built using the DotNet platform. This allows software providers of business systems to more quickly integrate their system's into other ESD supported systems and Ecommerce based platforms.
The webservice is build using the Windows Communication Foundation (WCF) libraries that natively exist within the DotNet platform. This means less work is required to create the webservice since the DotNet platform takes care of the low level programming required.

## Installation
After deploying the webservice template repository to your local machine using GIT, you should be able to run the webservice by opening up on visual studio solution file within Visual Studio, then pressing the Start button. This will cause the webservice to run as a console application. Ensure that your Visual Studio installation has [NuGet](https://www.nuget.org/) installed since the solution has a dependency to the [JSON.net library](https://github.com/JamesNK/Newtonsoft.Json) to run, otherwise you will need to download Newtonsoft library separately and reference it's dll in the solution. At the time of writing the webservice template solution successfully could be started from within Visual Studio 2015 running with DotNet 4.6.1. For some versions of Windows, Visual Studio may need to be run as an administrator to allow the webservice to listen on a given port.

## Retrieving Data
After the webservice console application has started, you can open a web browser and call any of the following URLs to obtain example data that has been set within the webservice template.

* http://localhost:8081/esd/data/alternate_codes
* http://localhost:8081/esd/data/attachments
* http://localhost:8081/esd/data/categories
* http://localhost:8081/esd/data/customer_accounts
* http://localhost:8081/esd/data/customer_account_addresses
* http://localhost:8081/esd/data/customer_account_contracts
* http://localhost:8081/esd/data/downloads
* http://localhost:8081/esd/data/flags
* http://localhost:8081/esd/data/general_ledger_accounts
* http://localhost:8081/esd/data/images
* http://localhost:8081/esd/data/item_groups
* http://localhost:8081/esd/data/item_relations
* http://localhost:8081/esd/data/kits
* http://localhost:8081/esd/data/labour
* http://localhost:8081/esd/data/locations
* http://localhost:8081/esd/data/payment_types
* http://localhost:8081/esd/data/price_level_pricing
* http://localhost:8081/esd/data/price_level_quantity_pricing
* http://localhost:8081/esd/data/price_customer_account_pricing
* http://localhost:8081/esd/data/products
* http://localhost:8081/esd/data/price_levels
* http://localhost:8081/esd/data/product_combinations
* http://localhost:8081/esd/data/product_stock_quantities
* http://localhost:8081/esd/data/purchasers
* http://localhost:8081/esd/data/sales_reps
* http://localhost:8081/esd/data/sell_units
* http://localhost:8081/esd/data/supplier_accounts
* http://localhost:8081/esd/data/supplier_account_addresses
* http://localhost:8081/esd/data/surcharges
* http://localhost:8081/esd/data/taxcodes
* http://localhost:8081/esd/data/webservice_status
* http://localhost:8081/esd/data/customer_account_enquiry_record?keyCustomerAccountID=1&recordType=INVOICE&keyRecordID=34652
* http://localhost:8081/esd/data/customer_account_enquiry_records?keyCustomerAccountID=1&recordType=INVOICE&beginDate=0&endDate=0&pageNumber=0&numberOfRecords=0&orderByField=&orderByDirection=&outstandingRecords=false&searchString=&keyRecordIDs=&searchType=
* http://localhost:8081/esd/data/customer_account_enquiry_line_report?keyCustomerAccountID=1&recordType=INVOICE&reportID=invoice_lines&orderByField=&orderByDirection=&pageNumber=0&numberOfRecords=0
* http://localhost:8081/esd/data/customer_account_status?keyCustomerAccountID=1&checkOnHold=Y&checkBalance=Y&checkTerms=Y
* http://localhost:8081/esd/data/supplier_account_enquiry_record?keySupplierAccountID=1&recordType=ORDER_PURCHASE&keyRecordID=34644
* http://localhost:8081/esd/data/supplier_account_enquiry_records?keySupplierAccountID=1&recordType=ORDER_PURCHASE&beginDate=0&endDate=0&pageNumber=0&numberOfRecords=0&orderByField=&orderByDirection=&outstandingRecords=false&searchString=&keyRecordIDs=&searchType=
* http://localhost:8081/esd/data/price_customer_account_price?keyCustomerAccountID=ACC-123&keyProductID=PROD-123&quantity=10

## Posting Data
The following URLs can be called using a HTTP POST request with the body of the request being given an Ecommerce Standards Document in the JSON data form based on the supported document type. Read the ESD library's [documentation](https://www.squizz.com/esd/index.html) to determine this.
* http://localhost:8081/esd/data/sales_orders
* http://localhost:8081/esd/data/purchase_orders
* http://localhost:8081/esd/data/supplier_invoices
* http://localhost:8081/esd/data/customer_accounts
* http://localhost:8081/esd/data/customer_account_payments

## ESD Webservice Template Class Structure
The webservice has been broken down into the following classes

**ESDWebServiceRunner.cs**
This class contains the main entry point into the webservice application. It is in charge of configuring, starting and running the ESD webservice.

**IESDWebServiceEndPoints.cs**
This interface defines the endpoints that can be called with HTTP requests to the webservice. For each endpoint it defines the method within the controller class that will be used to process the request to the HTTP endpoint. 

**ESDWebServiceController.cs**
This class is called whenever a HTTP request comes into the webservice that a matching endpoint needs to be processed. It is the class that does the majority of the grunt work for the web service. For each endpoint request the webservice creates an instance of the class to handle the request. The webservice creates these instances using multi-threading, this means that simultaneous requests can be processed at the same time for a number of endpoints.

**ESDWebServiceSettings.cs**
This class contains a number of properties that define how the webservice is run. In a production environment these settings would be typically set in a separate file, or database, where they can could easily be changed by a system administrator person.

**ESDWebServiceContants.cs**
This class contains a number of constant properties that are used throughout the webservice and controlling classes. In a production environment some of the language specific properties would be handled in separate files.

**GZIP Classes**
The GZipMessageEncoderFactory, GZipMessageEncodingBindingElement, GZipMessageEncodingBindingElementImporter classes are used to allow all HTTP responses from the webservice to be compressed using the GZIP compression algorithm. These classes are only required in the earlier versions of the DotNet platform (versions 4.0 and lower) where the platform provided no inbuilt mechanism of compression. It is important that the data returned from the webservice is compressed since this reduces download times for the receiver to obtain data. For certain endpoints 100s of megabytes of data may be returned which most definitely requires compression.

## Notes For Implementation Of the Webservice In A Production Environment
The ESD webservices template provides an example of how to build a webservice into a DotNet application. If it is to be implemented in a real world application the following things would need to be altered.
* Removal of test data in the ESDWebServiceController.cs Instead you would need to place in code that retrieves data from some data source and fills the ESDocument with records. Typically requests would be made to ODBC or SQL Server based database.
* Pick and choose which endpoints that your business system wishes to support. Your business system may not have data for all the endpoints or it may be too time consuming to implement the all endpoints, so instead choose the endpoints that are most critical for interacting with other Ecommerce systems. For all other endpoints either return empty documents or documents with error statuses.
* Configure settings to be read into the webservice. Rather than having settings hard coded into the application, separate files or databases should be used to read in the settings that dictate how the webservice is ran.
* Add additional security measures. This includes allowing basic HTTP authentication to be turned on (which can be done by changing the WEBSERVICE_REQUIRES_AUTHENTICATION property within the ESDWebServiceSettings.cs class). If the webservice is facing the internet then encryption should also be added to secure the data coming in and out of the webservice. This could be handled in several ways, such as placing the webservice behind an IIS, Apache, or Nginx web server which contains SSL/TLS certificates, or building HTTPS security into the webservice itself. Or else using other agreed upon encryption algorithms. Also ensuring that any configuration files used to tailor the webservice are also securely fastened, such as placing permissions on such files.
* Allow the webservice to run as a Windows service in the background. This ensures that at any time the webservice can be called to process a request. If the webservice crashes then a Windows service can be set to automatically restart itself which is usually mandatory for uptime guarantees. 
