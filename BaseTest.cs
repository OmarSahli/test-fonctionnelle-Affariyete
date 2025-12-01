using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using NUnit.Framework;
using System;
using System.IO;

namespace AffariYetTests
{
    [TestFixture]
    public class BaseTest
    {
        protected IWebDriver driver;
        protected WebDriverWait wait;
        protected string baseUrl = "https://www.affariyet.com";

        [SetUp]
        public void SetUp()
        {
            var options = new ChromeOptions();
            options.AddArgument("--start-maximized");
            
            driver = new ChromeDriver(options);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
            
            driver.Navigate().GoToUrl(baseUrl);
        }

        [TearDown]
        public void TearDown()
        {
            // Prendre une capture d'écran si le test échoue
            if (TestContext.CurrentContext.Result.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Failed)
            {
                TakeScreenshot(TestContext.CurrentContext.Test.Name + "_FAIL");
            }
            
            if (driver != null)
            {
                driver.Quit();
                driver.Dispose();
            }
        }

        protected IWebElement WaitForElement(By locator)
        {
            return wait.Until(drv => drv.FindElement(locator));
        }

        protected void TakeScreenshot(string testName)
        {
            try
            {
                string folder = "Screenshots";
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);
                
                Screenshot ss = ((ITakesScreenshot)driver).GetScreenshot();
                string path = Path.Combine(folder, testName + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".png");
                ss.SaveAsFile(path);
                TestContext.WriteLine("📸 Screenshot: " + path);
            }
            catch (Exception ex)
            {
                TestContext.WriteLine("Erreur screenshot: " + ex.Message);
            }
        }
    }
}
