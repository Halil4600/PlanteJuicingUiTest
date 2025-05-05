using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using System.Collections.ObjectModel;

namespace PlanteJuicingUITest
{
    [TestClass]
    public sealed class Test1
    {
        private static readonly string DriverDirectory = "C:\\WebDrivers";

        private static IWebDriver _driver;
        // _driver initialized in [ClassInitialize]

        [ClassInitialize]
        public static void Setup(TestContext context)
        {
            _driver = new ChromeDriver(DriverDirectory);
        }

        [ClassCleanup]
        public static void TearDown()
        {
            _driver.Dispose();
        }

        [TestMethod]
        public void MoistureAppearsTest()
        {
            string url = "http://localhost:5500/index.html"; // ? Ret til din serversti
            _driver.Navigate().GoToUrl(url);

            // Check page title (optional, but can stay)
            Assert.AreEqual("Jordfugtighed", _driver.Title);

            // Find og klik på knappen
            IWebElement button = _driver.FindElement(By.TagName("MoistureBut"));
            button.Click();

            // Vent på at fugtigheds-værdien vises i <h2>
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            IWebElement moistureElement = wait.Until(d =>
                d.FindElement(By.TagName("h2"))
            );

            string text = moistureElement.Text.Trim();

            // Tjek at værdien slutter med %
            Assert.IsTrue(text.EndsWith("%"), "Jordfugtigheds-værdi blev ikke vist korrekt");
        }

    }
}