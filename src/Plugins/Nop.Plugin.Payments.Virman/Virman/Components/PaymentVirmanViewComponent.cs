using System.Net;
using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.Payments.Virman.Models;
using Nop.Web.Framework.Components;

namespace Nop.Plugin.Payments.Virman.Components
{
    [ViewComponent(Name = "PaymentVirman")]
    public class PaymentVirmanViewComponent : NopViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var model = new PaymentInfoModel();
            //set postback values (we cannot access "Form" with "GET" requests)
            if (this.Request.Method != WebRequestMethods.Http.Get)
            {
                model.PurchaseOrderNumber = this.HttpContext.Request.Form["PurchaseOrderNumber"];
            }

            return View("~/Plugins/Payments.Virman/Views/PaymentInfo.cshtml", model);
        }
    }
}
