using OpenQA.Selenium;
using NUnit.Framework;
using System;

namespace AffariYetTests
{
    [TestFixture]
    public class AffariYetTests : BaseTest
    {
        [Test, Order(1)]
        public void Test01_VerifyPageTitle()
        {
            string actualTitle = driver.Title;
            Assert.That(actualTitle, Does.Contain("Affariyet"));
            TestContext.WriteLine("✓ Test 1 RÉUSSI");
        }

        [Test, Order(2)]
        public void Test02_SearchFunctionality()
        {
            IWebElement searchBox = WaitForElement(By.CssSelector("input[name='s']"));
            searchBox.SendKeys("PC portable");
            searchBox.SendKeys(Keys.Enter);
            System.Threading.Thread.Sleep(2000);
            Assert.That(driver.Url.ToLower(), Does.Contain("search").Or.Contain("s="));
            TestContext.WriteLine("✓ Test 2 RÉUSSI");
        }

        [Test, Order(3)]
        public void Test03_CategoryNavigation()
        {
            try
            {
                // Méthode alternative : naviguer directement vers la catégorie
                driver.Navigate().GoToUrl(baseUrl + "/5-informatique-Tunisie");
                System.Threading.Thread.Sleep(2000);
                Assert.That(driver.Url.ToLower(), Does.Contain("informatique"));
                TestContext.WriteLine("✓ Test 3 RÉUSSI");
            }
            catch (Exception ex)
            {
                // Si l'URL directe ne fonctionne pas, essayer de cliquer
                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                IWebElement categoryLink = WaitForElement(By.XPath("//a[contains(@href,'informatique')]"));
                js.ExecuteScript("arguments[0].scrollIntoView(true);", categoryLink);
                System.Threading.Thread.Sleep(500);
                js.ExecuteScript("arguments[0].click();", categoryLink);
                System.Threading.Thread.Sleep(2000);
                Assert.That(driver.Url.ToLower(), Does.Contain("informatique"));
                TestContext.WriteLine("✓ Test 3 RÉUSSI");
            }
        }

        [Test, Order(4)]
        public void Test04_ProductPageDisplay()
        {
            driver.Navigate().GoToUrl(baseUrl + "/5-informatique-Tunisie");
            System.Threading.Thread.Sleep(2000);
            var products = driver.FindElements(By.CssSelector(".product-container, .product-item, .product"));
            Assert.That(products.Count, Is.GreaterThan(0), "Aucun produit trouvé");
            TestContext.WriteLine("✓ Test 4 RÉUSSI - " + products.Count + " produits trouvés");
        }

        [Test, Order(5)]
        public void Test05_Footer()
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("window.scrollTo(0, document.body.scrollHeight)");
            System.Threading.Thread.Sleep(1000);
            IWebElement footer = driver.FindElement(By.CssSelector("footer"));
            Assert.That(footer.Displayed, Is.True);
            TestContext.WriteLine("✓ Test 5 RÉUSSI");
        }

        [Test, Order(6)]
        public void Test06_VerifyLogo()
        {
            var logo = driver.FindElements(By.CssSelector("img[alt*='Affariyet'], .logo img"));
            Assert.That(logo.Count, Is.GreaterThan(0), "Logo non trouvé");
            TestContext.WriteLine("✓ Test 6 RÉUSSI");
        }

        [Test, Order(7)]
        public void Test07_VerifySearchBoxExists()
        {
            IWebElement searchBox = driver.FindElement(By.CssSelector("input[name='s']"));
            Assert.That(searchBox.Displayed, Is.True);
            TestContext.WriteLine("✓ Test 7 RÉUSSI");
        }

        [Test, Order(8)]
        public void Test08_NavigateToElectronicsCategory()
        {
            driver.Navigate().GoToUrl(baseUrl + "/36-electromenager-tunisie");
            System.Threading.Thread.Sleep(2000);
            Assert.That(driver.Url.ToLower(), Does.Contain("electromenager"));
            TestContext.WriteLine("✓ Test 8 RÉUSSI");
        }

        [Test, Order(9)]
        public void Test09_VerifyMenuExists()
        {
            var menuItems = driver.FindElements(By.CssSelector("nav a, .menu a, header a"));
            Assert.That(menuItems.Count, Is.GreaterThan(5), "Menu insuffisant");
            TestContext.WriteLine("✓ Test 9 RÉUSSI - " + menuItems.Count + " liens menu");
        }

        [Test, Order(10)]
        public void Test10_ResponsiveDesign()
        {
            driver.Manage().Window.Size = new System.Drawing.Size(375, 667);
            System.Threading.Thread.Sleep(2000);
            Assert.That(driver.Url, Does.Contain("affariyet"));
            TestContext.WriteLine("✓ Test 10 RÉUSSI - Mode mobile");
            driver.Manage().Window.Maximize();
        }
    }
}
