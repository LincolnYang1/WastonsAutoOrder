using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Threading;
using OpenQA.Selenium.Firefox;
using System.IO;
using System.Configuration;
using WastonsLib;

namespace WebDriver1
{
    class Program
    {

        static void Main(string[] args)
        {
            string username = ConfigurationManager.AppSettings["Username"];
            string pwd = ConfigurationManager.AppSettings["Password"];
            string pdfFileLocalPath = ConfigurationManager.AppSettings["pdfpath"];

            var web = WastonsWebDriverExt.CreateWebDriver();
            web.LoginWastons(username, pwd)
                .Tee(w => w.Url = "https://www.watsons.com.sg/my-account/orders")
                .GetWastonsInvoiceUrls()
                .Where(url => !HasDownloaded(pdfFileLocalPath, GetPdfFileName(url)))
                .ToList()
                .ForEach(url => WastonsWebDriverExt.SafePrint(web, url));
            
        }
        
        static string GetPdfFileName(string url)
        {
            return url.Substring(url.LastIndexOf("/") + 1, url.IndexOf("?") - url.LastIndexOf("/")-1);
        }

        static bool HasDownloaded(string pdflocalFilePath, string fileName)
        {
            var files = Directory.GetFiles(pdflocalFilePath).Select(f => Path.GetFileName(f));
            return files.Contains(fileName);
        }
    }

    
}
