using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Plugin.Payments.Virman.Models;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Payments;
using Nop.Services.Stores;
using Nop.Web.Framework.Controllers;
using Nop.Services.Common;

namespace Nop.Plugin.Payments.Virman.Controllers
{
    public class PaymentVirmanController : BasePaymentController
    {
        private readonly IWorkContext _workContext;
        private readonly IStoreService _storeService;
        private readonly ISettingService _settingService;
        private readonly ILocalizationService _localizationService;


        public PaymentVirmanController(IWorkContext workContext,
            IStoreService storeService,
            ISettingService settingService,
            ILocalizationService localizationService)
        {
            this._workContext = workContext;
            this._storeService = storeService;
            this._settingService = settingService;
            this._localizationService = localizationService;

        }

    [AdminAuthorize]
        [ChildActionOnly]
        public ActionResult Configure()
        {
            //load settings for a chosen store scope
            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var virmanPurchaseSettings = _settingService.LoadSetting<VirmanPaymentSettings>(storeScope);

            var model = new ConfigurationModel();
            model.AdditionalFee = virmanPurchaseSettings.AdditionalFee;
            model.AdditionalFeePercentage = virmanPurchaseSettings.AdditionalFeePercentage;

            model.ActiveStoreScopeConfiguration = storeScope;
            if (storeScope > 0)
            {
                model.AdditionalFee_OverrideForStore = _settingService.SettingExists(virmanPurchaseSettings, x => x.AdditionalFee, storeScope);
                model.AdditionalFeePercentage_OverrideForStore = _settingService.SettingExists(virmanPurchaseSettings, x => x.AdditionalFeePercentage, storeScope);
            }

            return View("~/Plugins/Payments.Virman/Views/PaymentVirman/Configure.cshtml", model);
        }

        [HttpPost]
        [AdminAuthorize]
        [ChildActionOnly]
        public ActionResult Configure(ConfigurationModel model)
        {
            if (!ModelState.IsValid)
                return Configure();

            //load settings for a chosen store scope
            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var virmanPaymentSettings = _settingService.LoadSetting<VirmanPaymentSettings>(storeScope);

            //save settings
            virmanPaymentSettings.AdditionalFee = model.AdditionalFee;
            virmanPaymentSettings.AdditionalFeePercentage = model.AdditionalFeePercentage;

            /* We do not clear cache after each setting update.
             * This behavior can increase performance because cached settings will not be cleared 
             * and loaded from database after each update */
            if (model.AdditionalFee_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(virmanPaymentSettings, x => x.AdditionalFee, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(virmanPaymentSettings, x => x.AdditionalFee, storeScope);

            if (model.AdditionalFeePercentage_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(virmanPaymentSettings, x => x.AdditionalFeePercentage, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(virmanPaymentSettings, x => x.AdditionalFeePercentage, storeScope);

            //now clear settings cache
            _settingService.ClearCache();

            return Configure();
        }

        [ChildActionOnly]
        public ActionResult PaymentInfo()
        {
            var model = new PaymentInfoModel();

            //set postback values
            var form = this.Request.Form;
            model.PurchaseOrderRefNumber = form["PurchaseOrderRefNumber"];

            var customer = _workContext.CurrentCustomer;
            var _paymentModel = "HR99";
            var _paymentP1 = "";
            if (customer.GetAttribute<string>(SystemCustomerAttributeNames.OibNumber).Length > 0 )
            {
                _paymentP1 = customer.GetAttribute<string>(SystemCustomerAttributeNames.OibNumber);
            }
            else if (customer.GetAttribute<string>(SystemCustomerAttributeNames.VatNumber).Length > 0 && _paymentP1.Length <= 0)
            {
                _paymentP1 = customer.GetAttribute<string>(SystemCustomerAttributeNames.VatNumber).Substring(2);
            }
            if (_paymentP1.Length <= 0)
            {
                _paymentP1 = "00000000000";
            }
            //var _paymentP1 = customer.GetAttribute<string>(SystemCustomerAttributeNames.OibNumber);
            //var _paymentP2 = DateTime.Now.ToString("ddMMyyHHmm", System.Globalization.CultureInfo.InvariantCulture);
            var _paymentP2 = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            model.PurchaseOrderRefNumber = _paymentModel + ": " + _paymentP1 + "-" + _paymentP2;

            return View("~/Plugins/Payments.Virman/Views/PaymentVirman/PaymentInfo.cshtml", model);
        }

        [NonAction]
        public override IList<string> ValidatePaymentForm(FormCollection form)
        {
            var warnings = new List<string>();
            return warnings;
        }

        [NonAction]
        public override ProcessPaymentRequest GetPaymentInfo(FormCollection form)
        {
            var paymentInfo = new ProcessPaymentRequest();
            paymentInfo.CustomValues.Add(_localizationService.GetResource("Plugins.Payment.Virman.PurchaseOrderRefNumber"), form["PurchaseOrderRefNumber"]);
            return paymentInfo;
        }
    }
}