using System.IO;
using System.Threading.Tasks;
using CocomeStore.Models.Transfer;
using DinkToPdf;
using DinkToPdf.Contracts;
using RazorLight;

namespace CocomeStore.Services.Documents
{
    public class DocumentService : IDocumentService
    {
        private readonly IRazorLightEngine _engine;
        private readonly IConverter _converter;

        public DocumentService(IRazorLightEngine engine, IConverter converter)
        {
            _engine = engine;
            _converter = converter;
        }

        public async Task<byte[]> CreateBill(SaleTO saleTO)
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
            byte[] file = _converter.Convert(pdf);
            return file;
        }
    }
}
