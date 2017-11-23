using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WastonsLib;

namespace WastonsAutoOrder
{
    class Program
    {
        static void Main(string[] args)
        {
            string username = ConfigurationManager.AppSettings["Username"];
            string pwd = ConfigurationManager.AppSettings["Password"];

            var web = WastonsWebDriverExt.CreateWebDriver();
            web.LoginWastons(username, pwd)
                .Tee(w => Thread.Sleep(10000))
                .Pipe(w => { w.Url = "https://www.watsons.com.sg/my-account/order/01076563"; return w; })

                .Pipe(w => { w.FindElement(By.Id("addOrderAllEntryToBasket")).Click(); return w; })
                .Tee(w => Thread.Sleep(10000))
                .Pipe(w => { w.Url = "https://www.watsons.com.sg/checkoutstepone"; return w; })
                .Pipe(w => { w.FindElement(By.Id("termsConditions")).Click(); return w; })
                .Pipe(w => {
                    w.FindElement(By.Id("mybasketCheckoutButton")).Click(); return w; })
                .Pipe(w => { w.FindElement(By.Id("popupClose")).Click(); return w; })
                .Tee(w => Thread.Sleep(1000))
                .Pipe(w => { w.FindElement(By.Id("mybasketCheckoutButton")).Click(); return w; }).Tee(w => Thread.Sleep(1000))
                .Pipe(w => { w.FindElement(By.Name("deliveryMode")).Click(); return w; }).Tee(w => Thread.Sleep(1000))
                .Pipe(w => { w.FindElement(By.Name("radioResultItem")).Click(); return w; }).Tee(w => Thread.Sleep(1000))
                .Pipe(w => { w.FindElement(By.ClassName("gotoNextStep ")).Click(); return w; })

                .Pipe(w => { w.FindElement(By.Id("promotionInputId")).SendKeys("25OFF2311"); return w; })
                .Pipe(w => { w.FindElement(By.Id("promotionButton")).Click(); return w; })
                .Tee(w => Thread.Sleep(2000))

                .Pipe(w => { w.FindElement(By.Id("redeemPointInputId")).SendKeys("2"); return w; })
                .Pipe(w => { w.FindElement(By.Id("redeemButton")).Click(); return w; })
                .Tee(w => Thread.Sleep(2000))
                .Pipe(w => { w.FindElement(By.Id("gotoNextStepId")).Click(); return w; })

                .Pipe(w => { w.FindElement(By.Name("name")).SendKeys(ConfigurationManager.AppSettings["CreditcardName"]); return w; })
                .Pipe(w => { w.FindElement(By.Name("cardNo")).SendKeys(ConfigurationManager.AppSettings["CreditcardNo"]); return w; })
                .Pipe(w => { w.FindElement(By.Name("cvv")).SendKeys(ConfigurationManager.AppSettings["CreditcardCVV"]); return w; })
                .Pipe(w => { w.FindElement(By.Name("expiryMonth")).SendKeys(ConfigurationManager.AppSettings["CreditcardExpireMonth"]); return w; })
                .Pipe(w => { w.FindElement(By.Name("expiryYear")).SendKeys(ConfigurationManager.AppSettings["CreditcardExpireYear"]); return w; })
                .Pipe(w => { w.FindElement(By.Id("agree")).Click(); return w; })
                .Tee(w => Thread.Sleep(2000))
                .Pipe(w => { w.FindElement(By.Id("submit")).Click(); return w; });

        }
    }
}
