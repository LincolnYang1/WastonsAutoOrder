using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace WastonsLib
{

    public static class Extensions
    {
        public static IWebElement GetElement(this IWebDriver web, By by)
        {
            try
            {
                return web.FindElement(by);
            }
            catch
            {
                return null;
            }
        }

        public static T Tee<T>(this T t, Action<T> act) { act(t); return t; }
        public static R Pipe<T, R>(this T t, Func<T, R> func) => func(t);
    }

    public static class WastonsWebDriverExt
    {
        private const string NEXT_PAGE_CSS_STYLE = "sprite-P01B28NextSmStatic";
        private const string WASTONS_PAGING_URL = "https://www.watsons.com.sg/my-account/orders?sort=byDate&page=@PAGENO&resultsForPage=10&sort=";
        private const string WASTONS_LOGIN_URL = "https://www.watsons.com.sg/login";

        public static IEnumerable<string> GetWastonPagingUrls(this IWebDriver web)
        {
            var i = 0;
            while (web.GetElement(By.ClassName(NEXT_PAGE_CSS_STYLE)) != null)
            {
                i++;
                yield return WASTONS_PAGING_URL.Replace("@PAGENO", i.ToString());
            }
        }

        public static IEnumerable<string> GetWastonsInvoiceUrls(this IWebDriver web)
        {
            return
                web.GetWastonPagingUrls()
                .SelectMany(url =>
                {
                    web.Url = url;
                    return web.FindElements(By.ClassName("bgDotRight"))
                        .Where(td => td.Text == "Invoice")
                        .Select(td => td.FindElement(By.TagName("a")))
                        .Select(a => a.GetAttribute("href"))
                        .ToList();
                }
                );
        }

        public static IWebDriver CreateWebDriver()
        {
            //FirefoxProfile profile = new FirefoxProfile();
            //Proxy p = new Proxy();
            //p.IsAutoDetect = true;
            //profile.SetProxyPreferences(p);
            //profile.SetPreference("services.sync.prefs.sync.browser.download.manager.showWhenStarting", false);
            //profile.SetPreference("pdfjs.disabled", true);
            //profile.SetPreference("print.always_print_silent", true);
            //profile.SetPreference("print.show_print_progress", false);
            //profile.SetPreference("browser.download.show_plugins_in_list", false);
            //IWebDriver web = new FirefoxDriver(profile);
            IWebDriver web = new ChromeDriver(@"C:\Tools\chromedriver_win32");
            return web;
        }

        public static IWebDriver LoginWastons(this IWebDriver web, string username, string pwd)
        {
            web.Url = WASTONS_LOGIN_URL;
            Thread.Sleep(1000);

            web.FindElement(By.Id("j_username")).SendKeys(username);
            web.FindElement(By.Id("j_password")).SendKeys(pwd);
            web.FindElement(By.Id("signUpFormSumbitBtn")).Click();

            web.FindElement(By.Id("myAccountfl"));
            return web;
        }

        public static void SafePrint(IWebDriver web, string url)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)web;

            web.Url = FormatUrl(url);
            Thread.Sleep(5000);
            //js.ExecuteScript("window.print();");
            Thread.Sleep(5000);
        }

        static string FormatUrl(string url)
        {
            return url.Substring(0, url.IndexOf("?"));
        }
    }
}
