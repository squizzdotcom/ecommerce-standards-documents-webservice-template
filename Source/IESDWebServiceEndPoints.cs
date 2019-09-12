using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Web;
using System.ServiceModel;

namespace ESDWebserviceTemplate
{
    /// <summary>Defines that endpoints that the ESD webservice will make available through HTTP requests. Additionally defines the methods in the Controller to pass the webservice requests to for processing.</summary>
    [ServiceContract]
    public interface IESDWebServiceEndPoints
    {
        //HTTP GET methods to retreive data, serialize it and return it 
        [WebGet(UriTemplate = "data/alternate_codes", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        System.ServiceModel.Channels.Message getAlternateCodes();

        [WebGet(UriTemplate = "data/attachments", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        System.ServiceModel.Channels.Message getAttachments();

        [WebGet(UriTemplate = "data/attributes", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        System.ServiceModel.Channels.Message getAttributes();

        [WebGet(UriTemplate = "data/categories", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        System.ServiceModel.Channels.Message getCategories();        

        [WebGet(UriTemplate = "data/customer_accounts", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        System.ServiceModel.Channels.Message getCustomerAccounts();

        [WebGet(UriTemplate = "data/customer_account_addresses", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        System.ServiceModel.Channels.Message getCustomerAccountAddresses();

        [WebGet(UriTemplate = "data/customer_account_contracts", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        System.ServiceModel.Channels.Message getCustomerAccountContracts();

        [WebGet(UriTemplate = "data/customer_account_enquiry_record?keyCustomerAccountID={keyCustomerAccountID}&recordType={recordType}&keyRecordID={keyRecordID}", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        System.ServiceModel.Channels.Message getCustomerAccountEnquiryRecord(string keyCustomerAccountID, string recordType, string keyRecordID);

        [WebGet(UriTemplate = "data/customer_account_enquiry_records?keyCustomerAccountID={keyCustomerAccountID}&recordType={recordType}&beginDate={beginDate}&endDate={endDate}&pageNumber={pageNumber}&numberOfRecords={numberOfRecords}&orderByField={orderByField}&orderByDirection={orderByDirection}&outstandingRecords={outstandingRecords}&searchString={searchString}&keyRecordIDs={keyRecordIDs}&searchType={searchType}", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        System.ServiceModel.Channels.Message getCustomerAccountEnquiry(string keyCustomerAccountID, string recordType, long beginDate, long endDate, int pageNumber, int numberOfRecords, string outstandingRecords, string orderByField, string orderByDirection, string searchString, string keyRecordIDs, string searchType);

        [WebGet(UriTemplate = "data/customer_account_enquiry_line_report?keyCustomerAccountID={keyCustomerAccountID}&recordType={recordType}&reportID={reportID}&orderByField={orderByField}&orderByDirection={orderByDirection}&pageNumber={pageNumber}&numberOfRecords={numberOfRecords}", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        System.ServiceModel.Channels.Message getCustomerAccountEnquiryLineReport(string keyCustomerAccountID, string recordType, string reportID, string orderByField, string orderByDirection, int pageNumber, int numberOfRecords);

        [WebGet(UriTemplate = "data/customer_account_status?keyCustomerAccountID={keyCustomerAccountID}&checkOnHold={checkOnHold}&checkBalance={checkBalance}&checkTerms={checkTerms}", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        System.ServiceModel.Channels.Message getCustomerAccountStatus(string keyCustomerAccountID, string checkOnHold, string checkBalance, string checkTerms);

        [WebGet(UriTemplate = "data/delivery_notices", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        System.ServiceModel.Channels.Message getDeliveryNotices();

        [WebGet(UriTemplate = "data/downloads", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        System.ServiceModel.Channels.Message getDownloads();

        [WebGet(UriTemplate = "data/flags", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        System.ServiceModel.Channels.Message getFlags();

        [WebGet(UriTemplate = "data/general_ledger_accounts", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        System.ServiceModel.Channels.Message getGeneralLedgerAccounts();

        [WebGet(UriTemplate = "data/images", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        System.ServiceModel.Channels.Message getImages();

        [WebGet(UriTemplate = "data/item_groups", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        System.ServiceModel.Channels.Message getItemGroups();

        [WebGet(UriTemplate = "data/item_relations", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        System.ServiceModel.Channels.Message getItemRelations();

        [WebGet(UriTemplate = "data/kits", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        System.ServiceModel.Channels.Message getKits();

        [WebGet(UriTemplate = "data/labour", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        System.ServiceModel.Channels.Message getLabour();

        [WebGet(UriTemplate = "data/locations", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        System.ServiceModel.Channels.Message getLocations();

        [WebGet(UriTemplate = "data/makers", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        System.ServiceModel.Channels.Message getMakers();

        [WebGet(UriTemplate = "data/maker_models", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        System.ServiceModel.Channels.Message getMakerModels();

        [WebGet(UriTemplate = "data/maker_model_mappings", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        System.ServiceModel.Channels.Message getMakerModelMappings();

        [WebGet(UriTemplate = "data/payment_types", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        System.ServiceModel.Channels.Message getPaymentTypes();

        [WebGet(UriTemplate = "data/price_level_pricing", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        System.ServiceModel.Channels.Message getPriceLevelPricing();

        [WebGet(UriTemplate = "data/price_level_quantity_pricing", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        System.ServiceModel.Channels.Message getPriceLevelQuantityPricing();

        [WebGet(UriTemplate = "data/price_customer_account_pricing", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        System.ServiceModel.Channels.Message getProductAccountPricing();

        [WebGet(UriTemplate = "data/price_customer_account_price?keyCustomerAccountID={keyCustomerAccountID}&keyProductID={keyProductID}&quantity={quantity}", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        System.ServiceModel.Channels.Message getProductCustomerAccountPrice(string keyCustomerAccountID, string keyProductID, decimal quantity);

        [WebGet(UriTemplate = "data/products", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        System.ServiceModel.Channels.Message getProducts();

        [WebGet(UriTemplate = "data/price_levels", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        System.ServiceModel.Channels.Message getPriceLevels();

        [WebGet(UriTemplate = "data/product_combinations", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        System.ServiceModel.Channels.Message getProductCombinations();

        [WebGet(UriTemplate = "data/product_stock_quantities?keyProductID={keyProductID}&obtainAll={obtainAll}", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        System.ServiceModel.Channels.Message getProductStockQuantities(string keyProductID, string obtainAll);

        [WebGet(UriTemplate = "data/purchasers", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        System.ServiceModel.Channels.Message getPurchasers();

        [WebGet(UriTemplate = "data/sales_reps", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        System.ServiceModel.Channels.Message getSalesReps();

        [WebGet(UriTemplate = "data/sell_units", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        System.ServiceModel.Channels.Message getSellUnits();

        [WebGet(UriTemplate = "data/supplier_accounts", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        System.ServiceModel.Channels.Message getSupplierAccounts();

        [WebGet(UriTemplate = "data/supplier_account_addresses", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        System.ServiceModel.Channels.Message getSupplierAccountAddresses();

        [WebGet(UriTemplate = "data/supplier_account_enquiry_record?keySupplierAccountID={keySupplierAccountID}&recordType={recordType}&keyRecordID={keyRecordID}", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        System.ServiceModel.Channels.Message getSupplierAccountEnquiryRecord(string keySupplierAccountID, string recordType, string keyRecordID);

        [WebGet(UriTemplate = "data/supplier_account_enquiry_records?keySupplierAccountID={keySupplierAccountID}&recordType={recordType}&beginDate={beginDate}&endDate={endDate}&pageNumber={pageNumber}&numberOfRecords={numberOfRecords}&orderByField={orderByField}&orderByDirection={orderByDirection}&outstandingRecords={outstandingRecords}&searchString={searchString}&keyRecordIDs={keyRecordIDs}&searchType={searchType}", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        System.ServiceModel.Channels.Message getSupplierAccountEnquiry(string keySupplierAccountID, string recordType, long beginDate, long endDate, int pageNumber, int numberOfRecords, string orderByField, string orderByDirection, string outstandingRecords, string searchString, string keyRecordIDs, string searchType);

        [WebGet(UriTemplate = "data/surcharges", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        System.ServiceModel.Channels.Message getSurcharges();

        [WebGet(UriTemplate = "data/taxcodes", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        System.ServiceModel.Channels.Message getTaxcodes();

        [WebGet(UriTemplate = "data/webservice_status", ResponseFormat = WebMessageFormat.Json)]
        System.ServiceModel.Channels.Message getWebserviceStatus();

        //HTTP POST methods to read in data and process it
        [WebInvoke(UriTemplate = "data/sales_orders", Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        System.ServiceModel.Channels.Message importSalesOrders();

        [WebInvoke(UriTemplate = "data/purchase_orders", Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        System.ServiceModel.Channels.Message importPurchaseOrders();

        [WebInvoke(UriTemplate = "data/supplier_invoices", Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        System.ServiceModel.Channels.Message importSupplierInvoices();

        [WebInvoke(UriTemplate = "data/customer_accounts", Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        System.ServiceModel.Channels.Message importCustomerAccounts();

        [WebInvoke(UriTemplate = "data/customer_account_payments", Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        System.ServiceModel.Channels.Message importCustomerAccountPayment();
    }
}
