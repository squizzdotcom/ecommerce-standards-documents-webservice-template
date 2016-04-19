# Ecommerce Standards Documents DotNet Webservice Template
This repository contains a out-of-the-box programming example on how to develop a RESTful webservice that returns or processes Ecommerce Standards Documents (ESD) built on the DotNet platform.
This template webservice provides an example on how to integrate an ESD webservice into any business system built using the DotNet platform. This allows software providers of business systems to more quickly integrate their system's into other ESD supported systems.

## Installation
After deploying the webservice template repository to your local machine using GIT, you should be able to run the webservice by simply opening up on visual studio solution file within Visual Studio, then pressing the Start button. This will cause the webservice to run as a console application. At the time of writing the webservice templtae successfully could be started from within Visual Studio 2015 running with DotNet 4.6.1.

## Retrieving Data
After the webservice console application has started, you can open a web browser and call any of the following URLs to obtain example data that has been set within the webservice template.

http://localhost:8081/esd/data/alternate_codes
http://localhost:8081/esd/data/attachments
http://localhost:8081/esd/data/categories
http://localhost:8081/esd/data/customer_accounts
http://localhost:8081/esd/data/customer_account_addresses
http://localhost:8081/esd/data/customer_account_contracts
http://localhost:8081/esd/data/customer_account_enquiry_record?keyCustomerAccountID={keyCustomerAccountID}&recordType={recordType}&keyRecordID={keyRecordID}
http://localhost:8081/esd/data/customer_account_enquiry_records?keyCustomerAccountID={keyCustomerAccountID}&recordType={recordType}&beginDate={beginDate}&endDate={endDate}&pageNumber={pageNumber}&numberOfRecords={numberOfRecords}&orderByField={orderByField}&orderByDirection={orderByDirection}&outstandingRecords={outstandingRecords}&searchString={searchString}&keyRecordIDs={keyRecordIDs}&searchType={searchType}
http://localhost:8081/esd/data/customer_account_enquiry_line_report?keyCustomerAccountID={keyCustomerAccountID}&recordType={recordType}&reportID={reportID}&orderByField={orderByField}&orderByDirection={orderByDirection}&pageNumber={pageNumber}&numberOfRecords={numberOfRecords}
http://localhost:8081/esd/data/customer_account_status?keyCustomerAccountID={keyCustomerAccountID}&checkOnHold={checkOnHold}&checkBalance={checkBalance}&checkTerms={checkTerms}
http://localhost:8081/esd/data/downloads
http://localhost:8081/esd/data/flags
http://localhost:8081/esd/data/images
http://localhost:8081/esd/data/item_groups
http://localhost:8081/esd/data/item_relations
http://localhost:8081/esd/data/kits
http://localhost:8081/esd/data/labour
http://localhost:8081/esd/data/locations
http://localhost:8081/esd/data/payment_types
http://localhost:8081/esd/data/price_level_pricing
http://localhost:8081/esd/data/price_level_quantity_pricing
http://localhost:8081/esd/data/price_customer_account_pricing
http://localhost:8081/esd/data/price_customer_account_price?keyCustomerAccountID={keyCustomerAccountID}&keyProductID={keyProductID}&quantity={quantity}
http://localhost:8081/esd/data/products
http://localhost:8081/esd/data/price_levels
http://localhost:8081/esd/data/product_combinations
http://localhost:8081/esd/data/product_stock_quantities
http://localhost:8081/esd/data/purchasers
http://localhost:8081/esd/data/sales_reps
http://localhost:8081/esd/data/sell_units
http://localhost:8081/esd/data/supplier_accounts
http://localhost:8081/esd/data/supplier_account_addresses
http://localhost:8081/esd/data/sales_reps
http://localhost:8081/esd/data/surcharges
http://localhost:8081/esd/data/taxcodes
http://localhost:8081/esd/data/webservice_status

## Posting Data
The following URLs can be called using a HTTP POST request with the body of the request being given and Ecommerce Standards Document based on the given type
http://localhost:8081/esd/data/sales_orders
http://localhost:8081/esd/data/purchase_orders
http://localhost:8081/esd/data/customer_accounts
http://localhost:8081/esd/data/customer_account_payments

## ESD Webservice Template Class Structure
The webservice has been broken down into the following classes

**ESDWebServiceRunner.cs**
This class contains the main entry point into the webservice application. It is in charge of configuring, starting and running the ESD webservice.

**IESDWebServiceEndPoints.cs**
This interface defines the endpoints that can be called with HTTP requests to the webservice. For each endpoint it defines the method within the controller class that will be used to process the request to the HTTP endpoint. 

**ESDWebServiceController.cs**
This class is called whenever a HTTP request comes into the webservice that a matching endpoint needs to be processed. For each request the webservice creates an instance of the class to handle the request. The webservice creates these instances using multi-threading, this means that simultaneous requests can be processed at the same time for an endpoint.

**ESDWebServiceSettings.cs**
This class contains a number of properties that define how the webservice is run. In a production environment these settings would be typically set in a separate file, or database, where they can could easily be changed by a system administrator person.

**ESDWebServiceContants.cs**
This class contains a number of constant properties that are used throughout the webservice and controlling classes. In a production environment some of the language specific properties would be handled in separate files.

**GZIP Classes**
The GZipMessageEncoderFactory, ESDWebserviceTemplate, GZipMessageEncodingBindingElementImporter classes are used to allow all HTTP responses from the webservice to be compressed using the GZIP compression algorithm. These classes are only required in the earlier versions of the DotNet platform (versions 4.0 and lower) where the platform provided no inbuilt mechanism of compression. It is important that the data returned from the webservice is compressed since this reduces download times for the receiver to obtain data. For certain endpoints 100s of megabytes of data may be returned which most definitely requires compression.

## Notes For Implementation Of the Webservice In A Production Environment

