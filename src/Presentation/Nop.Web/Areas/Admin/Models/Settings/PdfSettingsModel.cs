using System.ComponentModel.DataAnnotations;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.Settings
{
    /// <summary>
    /// Represents a PDF settings model
    /// </summary>
    public partial class PdfSettingsModel : BaseNopModel, ISettingsModel
    {
        #region Properties

        public int ActiveStoreScopeConfiguration { get; set; }

        [NopResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.PdfLetterPageSizeEnabled")]
        public bool LetterPageSizeEnabled { get; set; }
        public bool LetterPageSizeEnabled_OverrideForStore { get; set; }

        [NopResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.PdfLogo")]
        [UIHint("Picture")]
        public int LogoPictureId { get; set; }
        public bool LogoPictureId_OverrideForStore { get; set; }

        [NopResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.DisablePdfInvoicesForPendingOrders")]
        public bool DisablePdfInvoicesForPendingOrders { get; set; }
        public bool DisablePdfInvoicesForPendingOrders_OverrideForStore { get; set; }

        [NopResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.InvoiceFooterTextColumn1")]
        public string InvoiceFooterTextColumn1 { get; set; }
        public bool InvoiceFooterTextColumn1_OverrideForStore { get; set; }

        [NopResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.InvoiceFooterTextColumn2")]
        public string InvoiceFooterTextColumn2 { get; set; }
        public bool InvoiceFooterTextColumn2_OverrideForStore { get; set; }

        [NopResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.UseStationaryForPDF")]
        public string StationaryFileName { get; set; }
        public bool StationaryFileName_OverrideForStore { get; set; }

        [NopResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.FontFileName")]
        [UIHint("Font File")]
        public string FontFileName { get; set; }
        public bool FontFileName_OverrideForStore { get; set; }

        [NopResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.FontFileNameBold")]
        [UIHint("Font File for bold")]
        public string FontFileNameBold { get; set; }
        public bool FontFileNameBold_OverrideForStore { get; set; }

        [NopResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.FontFileNameItalic")]
        [UIHint("Font File for italic")]
        public string FontFileNameItalic { get; set; }
        public bool FontFileNameItalic_OverrideForStore { get; set; }

        [NopResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.FontFileNameBoldItalic")]
        [UIHint("Font File for bold-italic")]
        public string FontFileNameBoldItalic { get; set; }
        public bool FontFileNameBoldItalic_OverrideForStore { get; set; }


        #endregion
    }
}