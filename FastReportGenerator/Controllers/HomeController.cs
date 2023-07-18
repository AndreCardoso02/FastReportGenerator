using FastReport.Data;
using FastReport.Export.PdfSimple;
using FastReport.Web;
using FastReportGenerator.Helpers;
using FastReportGenerator.Models;
using FastReportGenerator.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FastReportGenerator.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IWebHostEnvironment _webHostEnv;

        public HomeController(IProductService productService, IWebHostEnvironment webHostEnv, ICategoryService categoryService)
        {
            _productService = productService;
            _webHostEnv = webHostEnv;
            _categoryService = categoryService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Route("CreateReport")]
        public IActionResult CreateReport()
        {
            var caminhoReport = Path.Combine(_webHostEnv.WebRootPath, @"reports\ReportMvc.frx");
            var reportFile = caminhoReport;
            var freport = new FastReport.Report();
            var productList = _productService.GetProducts();
            freport.Dictionary.RegisterBusinessObject(productList, "productList", 10, true);

            freport.Report.Save(reportFile);

            return Ok($"Relatorio gerado: {caminhoReport}");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        [Route("ProductsReport")]
        public IActionResult ProductsReport()
        {
            var caminhoReport = Path.Combine(_webHostEnv.WebRootPath, @"reports\ReportMvc.frx");
            var reportFile = caminhoReport;

            var freport = new FastReport.Report();
            var productList = _productService.GetProducts();
            freport.Report.Load(reportFile);
            freport.Dictionary.RegisterBusinessObject(productList, "productList", 10, true);
            freport.Prepare();
            var pdfExport = new PDFSimpleExport();

            using MemoryStream ms = new MemoryStream();
            pdfExport.Export(freport, ms);
            ms.Flush();
            return File(ms.ToArray(), "application/pdf");

        }

        // Segunda parte registra algo no program
        [Route("ProductsCategory")]
        public IActionResult ProductsCategory()
        {
            var webReport = new WebReport();

            var mssqlDataConnection = new MsSqlDataConnection();

            webReport.Report.Dictionary.Connections.Add(mssqlDataConnection);
            webReport.Report.Load(Path.Combine(_webHostEnv.ContentRootPath, "wwwroot/reports", "products_category.frx"));

            var categories = HelperFastReport.GetTable(_categoryService.GetCategories(), "Categories");
            var products = HelperFastReport.GetTable(_productService.GetProducts(), "Products");

            webReport.Report.RegisterData(categories, "Categories");
            webReport.Report.RegisterData(products, "Products");

            return View(webReport);
        }

        [Route("ProductsCategoryPdf")]
        public IActionResult ProductsCategoryPdf()
        {
            var webReport = new WebReport();

            var mssqlDataConnection = new MsSqlDataConnection();
            webReport.Report.Dictionary.Connections.Add(mssqlDataConnection);
            webReport.Report.Load(Path.Combine(_webHostEnv.ContentRootPath, "wwwroot/reports", "products_category.frx"));

            var categories = HelperFastReport.GetTable(_categoryService.GetCategories(), "Categories");
            var products = HelperFastReport.GetTable(_productService.GetProducts(), "Products");

            webReport.Report.RegisterData(categories, "Categories");
            webReport.Report.RegisterData(products, "Products");

            webReport.Report.Prepare();

            Stream stream = new MemoryStream();
            webReport.Report.Export(new PDFSimpleExport(), stream);
            stream.Position = 0;
            return File(stream, "application/zip", "ProductsCategory.pdf");
        }
    }
}