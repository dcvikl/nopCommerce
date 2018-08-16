using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Nop.Core;
using Nop.Core.Domain.Orders;
using Nop.Core.Plugins;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Orders;
using Nop.Services.Payments;

namespace Nop.Plugin.Payments.Virman
{
    /// <summary>
    /// PurchaseOrder payment processor
    /// </summary>
    public class VirmanPaymentProcessor : BasePlugin, IPaymentMethod
    {
        #region Fields

        private readonly ILocalizationService _localizationService;
        private readonly IPaymentService _paymentService;
        private readonly ISettingService _settingService;
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IWebHelper _webHelper;
        private readonly VirmanPaymentSettings _virmanPaymentSettings;
        #endregion

        #region Ctor
        public VirmanPaymentProcessor(ILocalizationService localizationService,
            IPaymentService paymentService,
            ISettingService settingService,
            IShoppingCartService shoppingCartService,
            IWebHelper webHelper,
            VirmanPaymentSettings virmanPaymentSettings)
        {
            this._localizationService = localizationService;
            this._paymentService = paymentService;
            this._settingService = settingService;
            this._shoppingCartService = shoppingCartService;
            this._virmanPaymentSettings = virmanPaymentSettings;
            this._webHelper = webHelper;
        }
        #endregion

        //#region Methods OLD

        ///// <summary>
        ///// Process a payment
        ///// </summary>
        ///// <param name="processPaymentRequest">Payment info required for an order processing</param>
        ///// <returns>Process payment result</returns>
        //public ProcessPaymentResult ProcessPayment(ProcessPaymentRequest processPaymentRequest)
        //{
        //    var result = new ProcessPaymentResult();
        //    result.NewPaymentStatus = PaymentStatus.Pending;
        //    return result;
        //}

        ///// <summary>
        ///// Post process payment (used by payment gateways that require redirecting to a third-party URL)
        ///// </summary>
        ///// <param name="postProcessPaymentRequest">Payment info required for an order processing</param>
        //public void PostProcessPayment(PostProcessPaymentRequest postProcessPaymentRequest)
        //{
        //    //nothing
        //}

        ///// <summary>
        ///// Returns a value indicating whether payment method should be hidden during checkout
        ///// </summary>
        ///// <param name="cart">Shoping cart</param>
        ///// <returns>true - hide; false - display.</returns>
        //public bool HidePaymentMethod(IList<ShoppingCartItem> cart)
        //{
        //    //you can put any logic here
        //    //for example, hide this payment method if all products in the cart are downloadable
        //    //or hide this payment method if current customer is from certain country

        //    if (_virmanPaymentSettings.ShippableProductRequired && !cart.RequiresShipping())
        //        return true;

        //    return false;
        //}

        ///// <summary>
        ///// Gets additional handling fee
        ///// </summary>
        ///// <param name="cart">Shoping cart</param>
        ///// <returns>Additional handling fee</returns>
        //public decimal GetAdditionalHandlingFee(IList<ShoppingCartItem> cart)
        //{
        //    var result = this.CalculateAdditionalFee(_orderTotalCalculationService, cart,
        //        _virmanPaymentSettings.AdditionalFee, _virmanPaymentSettings.AdditionalFeePercentage);
        //    return result;
        //}

        ///// <summary>
        ///// Captures payment
        ///// </summary>
        ///// <param name="capturePaymentRequest">Capture payment request</param>
        ///// <returns>Capture payment result</returns>
        //public CapturePaymentResult Capture(CapturePaymentRequest capturePaymentRequest)
        //{
        //    var result = new CapturePaymentResult();
        //    result.AddError("Capture method not supported");
        //    return result;
        //}

        ///// <summary>
        ///// Refunds a payment
        ///// </summary>
        ///// <param name="refundPaymentRequest">Request</param>
        ///// <returns>Result</returns>
        //public RefundPaymentResult Refund(RefundPaymentRequest refundPaymentRequest)
        //{
        //    var result = new RefundPaymentResult();
        //    result.AddError("Refund method not supported");
        //    return result;
        //}

        ///// <summary>
        ///// Voids a payment
        ///// </summary>
        ///// <param name="voidPaymentRequest">Request</param>
        ///// <returns>Result</returns>
        //public VoidPaymentResult Void(VoidPaymentRequest voidPaymentRequest)
        //{
        //    var result = new VoidPaymentResult();
        //    result.AddError("Void method not supported");
        //    return result;
        //}

        ///// <summary>
        ///// Process recurring payment
        ///// </summary>
        ///// <param name="processPaymentRequest">Payment info required for an order processing</param>
        ///// <returns>Process payment result</returns>
        //public ProcessPaymentResult ProcessRecurringPayment(ProcessPaymentRequest processPaymentRequest)
        //{
        //    var result = new ProcessPaymentResult();
        //    result.AddError("Recurring payment not supported");
        //    return result;
        //}

        ///// <summary>
        ///// Cancels a recurring payment
        ///// </summary>
        ///// <param name="cancelPaymentRequest">Request</param>
        ///// <returns>Result</returns>
        //public CancelRecurringPaymentResult CancelRecurringPayment(CancelRecurringPaymentRequest cancelPaymentRequest)
        //{
        //    var result = new CancelRecurringPaymentResult();
        //    result.AddError("Recurring payment not supported");
        //    return result;
        //}

        ///// <summary>
        ///// Gets a value indicating whether customers can complete a payment after order is placed but not completed (for redirection payment methods)
        ///// </summary>
        ///// <param name="order">Order</param>
        ///// <returns>Result</returns>
        //public bool CanRePostProcessPayment(Order order)
        //{
        //    if (order == null)
        //        throw new ArgumentNullException("order");

        //    //it's not a redirection payment method. So we always return false
        //    return false;
        //}

        ///// <summary>
        ///// Gets a route for provider configuration
        ///// </summary>
        ///// <param name="actionName">Action name</param>
        ///// <param name="controllerName">Controller name</param>
        ///// <param name="routeValues">Route values</param>
        //public void GetConfigurationRoute(out string actionName, out string controllerName, out RouteValueDictionary routeValues)
        //{
        //    actionName = "Configure";
        //    controllerName = "PaymentVirman";
        //    routeValues = new RouteValueDictionary() { { "Namespaces", "Nop.Plugin.Payments.Virman.Controllers" }, { "area", null } };
        //}

        ///// <summary>
        ///// Gets a route for payment info
        ///// </summary>
        ///// <param name="actionName">Action name</param>
        ///// <param name="controllerName">Controller name</param>
        ///// <param name="routeValues">Route values</param>
        //public void GetPaymentInfoRoute(out string actionName, out string controllerName, out RouteValueDictionary routeValues)
        //{
        //    actionName = "PaymentInfo";
        //    controllerName = "PaymentVirman";
        //    routeValues = new RouteValueDictionary() { { "Namespaces", "Nop.Plugin.Payments.Virman.Controllers" }, { "area", null } };
        //}

        //public Type GetControllerType()
        //{
        //    return typeof(PaymentVirmanController);
        //}

        ///// <summary>
        ///// Install plugin
        ///// </summary>
        //public override void Install()
        //{
        //    //settings
        //    var settings = new VirmanPaymentSettings()
        //    {
        //        AdditionalFee = 0,
        //    };
        //    _settingService.SaveSetting(settings);


        //    //locales
        //    this.AddOrUpdatePluginLocaleResource("Plugins.Payment.Virman.PurchaseOrderRefNumber", "Purchase Order Reference No.");
        //    this.AddOrUpdatePluginLocaleResource("Plugins.Payment.Virman.AdditionalFee", "Additional fee");
        //    this.AddOrUpdatePluginLocaleResource("Plugins.Payment.Virman.AdditionalFee.Hint", "The additional fee.");
        //    this.AddOrUpdatePluginLocaleResource("Plugins.Payment.Virman.AdditionalFeePercentage", "Additional fee. Use percentage");
        //    this.AddOrUpdatePluginLocaleResource("Plugins.Payment.Virman.AdditionalFeePercentage.Hint", "Determines whether to apply a percentage additional fee to the order total. If not enabled, a fixed value is used.");
        //    this.AddOrUpdatePluginLocaleResource("Plugins.Payment.Virman.ShippableProductRequired", "Shippable product required");
        //    this.AddOrUpdatePluginLocaleResource("Plugins.Payment.Virman.ShippableProductRequired.Hint", "An option indicating whether shippable products are required in order to display this payment method during checkout.");


        //    base.Install();
        //}

        //public override void Uninstall()
        //{
        //    //settings
        //    _settingService.DeleteSetting<VirmanPaymentSettings>();

        //    //locales
        //    this.DeletePluginLocaleResource("Plugins.Payment.Virman.PurchaseOrderRefNumber");
        //    this.DeletePluginLocaleResource("Plugins.Payment.Virman.AdditionalFee");
        //    this.DeletePluginLocaleResource("Plugins.Payment.Virman.AdditionalFee.Hint");
        //    this.DeletePluginLocaleResource("Plugins.Payment.Virman.AdditionalFeePercentage");
        //    this.DeletePluginLocaleResource("Plugins.Payment.Virman.AdditionalFeePercentage.Hint");
        //    this.DeletePluginLocaleResource("Plugins.Payment.Virman.ShippableProductRequired");
        //    this.DeletePluginLocaleResource("Plugins.Payment.Virman.ShippableProductRequired.Hint");

        //    base.Uninstall();
        //}

        //#endregion

        #region Methods

        /// <summary>
        /// Process a payment
        /// </summary>
        /// <param name="processPaymentRequest">Payment info required for an order processing</param>
        /// <returns>Process payment result</returns>
        public ProcessPaymentResult ProcessPayment(ProcessPaymentRequest processPaymentRequest)
        {
            return new ProcessPaymentResult();
        }

        /// <summary>
        /// Post process payment (used by payment gateways that require redirecting to a third-party URL)
        /// </summary>
        /// <param name="postProcessPaymentRequest">Payment info required for an order processing</param>
        public void PostProcessPayment(PostProcessPaymentRequest postProcessPaymentRequest)
        {
            //nothing
        }

        /// <summary>
        /// Returns a value indicating whether payment method should be hidden during checkout
        /// </summary>
        /// <param name="cart">Shopping cart</param>
        /// <returns>true - hide; false - display.</returns>
        public bool HidePaymentMethod(IList<ShoppingCartItem> cart)
        {
            //you can put any logic here
            //for example, hide this payment method if all products in the cart are downloadable
            //or hide this payment method if current customer is from certain country
            if (_virmanPaymentSettings.ShippableProductRequired && !_shoppingCartService.ShoppingCartRequiresShipping(cart))
                return true;

            return false;
        }

        /// <summary>
        /// Gets additional handling fee
        /// </summary>
        /// <param name="cart">Shopping cart</param>
        /// <returns>Additional handling fee</returns>
        public decimal GetAdditionalHandlingFee(IList<ShoppingCartItem> cart)
        {
            return _paymentService.CalculateAdditionalFee(cart,
                _virmanPaymentSettings.AdditionalFee, _virmanPaymentSettings.AdditionalFeePercentage);
        }

        /// <summary>
        /// Captures payment
        /// </summary>
        /// <param name="capturePaymentRequest">Capture payment request</param>
        /// <returns>Capture payment result</returns>
        public CapturePaymentResult Capture(CapturePaymentRequest capturePaymentRequest)
        {
            return new CapturePaymentResult { Errors = new[] { "Capture method not supported" } };
        }

        /// <summary>
        /// Refunds a payment
        /// </summary>
        /// <param name="refundPaymentRequest">Request</param>
        /// <returns>Result</returns>
        public RefundPaymentResult Refund(RefundPaymentRequest refundPaymentRequest)
        {
            return new RefundPaymentResult { Errors = new[] { "Refund method not supported" } };
        }

        /// <summary>
        /// Voids a payment
        /// </summary>
        /// <param name="voidPaymentRequest">Request</param>
        /// <returns>Result</returns>
        public VoidPaymentResult Void(VoidPaymentRequest voidPaymentRequest)
        {
            return new VoidPaymentResult { Errors = new[] { "Void method not supported" } };
        }

        /// <summary>
        /// Process recurring payment
        /// </summary>
        /// <param name="processPaymentRequest">Payment info required for an order processing</param>
        /// <returns>Process payment result</returns>
        public ProcessPaymentResult ProcessRecurringPayment(ProcessPaymentRequest processPaymentRequest)
        {
            return new ProcessPaymentResult { Errors = new[] { "Recurring payment not supported" } };
        }

        /// <summary>
        /// Cancels a recurring payment
        /// </summary>
        /// <param name="cancelPaymentRequest">Request</param>
        /// <returns>Result</returns>
        public CancelRecurringPaymentResult CancelRecurringPayment(CancelRecurringPaymentRequest cancelPaymentRequest)
        {
            return new CancelRecurringPaymentResult { Errors = new[] { "Recurring payment not supported" } };
        }

        /// <summary>
        /// Gets a value indicating whether customers can complete a payment after order is placed but not completed (for redirection payment methods)
        /// </summary>
        /// <param name="order">Order</param>
        /// <returns>Result</returns>
        public bool CanRePostProcessPayment(Order order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            //it's not a redirection payment method. So we always return false
            return false;
        }

        /// <summary>
        /// Validate payment form
        /// </summary>
        /// <param name="form">The parsed form values</param>
        /// <returns>List of validating errors</returns>
        public IList<string> ValidatePaymentForm(IFormCollection form)
        {
            return new List<string>();
        }

        /// <summary>
        /// Get payment information
        /// </summary>
        /// <param name="form">The parsed form values</param>
        /// <returns>Payment info holder</returns>
        public ProcessPaymentRequest GetPaymentInfo(IFormCollection form)
        {
            return new ProcessPaymentRequest
            {
                CustomValues = new Dictionary<string, object>
                {
                    [_localizationService.GetResource("Plugins.Payment.Virman.PozivNaBroj")] = form["PozivNaBroj"].ToString()
                }
            };
        }

        /// <summary>
        /// Gets a configuration page URL
        /// </summary>
        public override string GetConfigurationPageUrl()
        {
            return $"{_webHelper.GetStoreLocation()}Admin/PaymentVirman/Configure";
        }

        /// <summary>
        /// Gets a view component name for displaying plugin in public store ("payment info" checkout step)
        /// </summary>
        public string GetPublicViewComponentName()
        {
            return "PaymentVirman";
        }

        /// <summary>
        /// Install the plugin
        /// </summary>
        public override void Install()
        {
            //settings
            _settingService.SaveSetting(new VirmanPaymentSettings());

            //locales
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Payment.Virman.AdditionalFee", "Additional fee");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Payment.Virman.AdditionalFee.Hint", "The additional fee.");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Payment.Virman.AdditionalFeePercentage", "Additional fee. Use percentage");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Payment.Virman.AdditionalFeePercentage.Hint", "Determines whether to apply a percentage additional fee to the order total. If not enabled, a fixed value is used.");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Payment.Virman.PaymentMethodDescription", "Pay by purchase order (PO) number");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Payment.Virman.PurchaseOrderNumber", "PO Number");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Payment.Virman.ShippableProductRequired", "Shippable product required");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Payment.Virman.ShippableProductRequired.Hint", "An option indicating whether shippable products are required in order to display this payment method during checkout.");

            base.Install();
        }

        /// <summary>
        /// Uninstall the plugin
        /// </summary>
        public override void Uninstall()
        {
            //settings
            _settingService.DeleteSetting<VirmanPaymentSettings>();

            //locales
            _localizationService.DeletePluginLocaleResource("Plugins.Payment.Virman.AdditionalFee");
            _localizationService.DeletePluginLocaleResource("Plugins.Payment.Virman.AdditionalFee.Hint");
            _localizationService.DeletePluginLocaleResource("Plugins.Payment.Virman.AdditionalFeePercentage");
            _localizationService.DeletePluginLocaleResource("Plugins.Payment.Virman.AdditionalFeePercentage.Hint");
            _localizationService.DeletePluginLocaleResource("Plugins.Payment.Virman.PaymentMethodDescription");
            _localizationService.DeletePluginLocaleResource("Plugins.Payment.Virman.PurchaseOrderNumber");
            _localizationService.DeletePluginLocaleResource("Plugins.Payment.Virman.ShippableProductRequired");
            _localizationService.DeletePluginLocaleResource("Plugins.Payment.Virman.ShippableProductRequired.Hint");

            base.Uninstall();
        }

        #endregion

        #region Properies

        /// <summary>
        /// Gets a value indicating whether capture is supported
        /// </summary>
        public bool SupportCapture
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether partial refund is supported
        /// </summary>
        public bool SupportPartiallyRefund
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether refund is supported
        /// </summary>
        public bool SupportRefund
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether void is supported
        /// </summary>
        public bool SupportVoid
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets a recurring payment type of payment method
        /// </summary>
        public RecurringPaymentType RecurringPaymentType
        {
            get
            {
                return RecurringPaymentType.NotSupported;
            }
        }

        /// <summary>
        /// Gets a payment method type
        /// </summary>
        public PaymentMethodType PaymentMethodType
        {
            get
            {
                return PaymentMethodType.Standard;
            }
        }

        /// <summary>
        /// Gets a value indicating whether we should display a payment information page for this plugin
        /// </summary>
        public bool SkipPaymentInfo
        {
            get
            {
                return false;
            }
        }

        #endregion
        
    }
}
