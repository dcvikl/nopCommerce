using System.Web.Mvc;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;

namespace Nop.Plugin.Payments.Virman.Models
{
    public class PaymentInfoModel : BaseNopModel
    {
        [NopResourceDisplayName("Plugins.Payment.Virman.PurchaseOrderRefNumber")]
        [AllowHtml]
        public string PurchaseOrderRefNumber { get; set; }
    }
}