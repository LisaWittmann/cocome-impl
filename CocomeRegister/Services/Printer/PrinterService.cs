using System.IO;
using System.Threading.Tasks;
using CocomeStore.Models.Transfer;
using DinkToPdf;
using DinkToPdf.Contracts;
using RazorLight;

namespace CocomeStore.Services.Printer
{
    /// <summary>
    /// class <c>PrinterService</c> implements <see cref="IPrinterService"/>
    /// and provides functionality to genereade documents for printing by templates
    /// </summary>
    public class PrinterService : IPrinterService
    {
        private readonly IRazorLightEngine _engine;
        private readonly IConverter _converter;

        public PrinterService(IRazorLightEngine engine, IConverter converter)
        {
            _engine = engine;
            _converter = converter;
        }

        /// <summary>
        /// method <c>CreateBillAsync</c> creates a bill document
        /// for the given sale transfer object
        /// </summary>
        /// <param name="saleTO">
        /// sale transfer object containing the sales information
        /// </param>
        public async Task<byte[]> CreateBillAsync(SaleTO saleTO)
        {
        
            string template = await _engine.CompileRenderAsync("BillingTemplate", saleTO);
            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = new PechkinPaperSize("57mm", "150mm"),
                Margins = new MarginSettings() { Top = 10, Bottom = 10, Left = 5, Right = 5 },
                DocumentTitle = "Beleg",
            };
            var objectSettings = new ObjectSettings
            {
                HtmlContent = template,
                WebSettings = { DefaultEncoding = "utf-8" },
            };
            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };
            return _converter.Convert(pdf);
        }
    }
}
