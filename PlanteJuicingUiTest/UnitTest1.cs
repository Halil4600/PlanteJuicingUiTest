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
            string url = "http://localhost:5500/index.html"; // ? Ret til din lokale sti/server
            _driver.Navigate().GoToUrl(url);
            
            Assert.IsTrue(_driver.Title.Contains("PlanteJuicing"));

            

            IWebElement moistureMessage = _driver.FindElement(By.Id("moistureMessage"));
            Assert.IsTrue(moistureMessage.Text.Contains("Du skal vande din plante!") || moistureMessage.Text.Contains("Din plante har det fint!"));

            IWebElement tempMessage = _driver.FindElement(By.Id("tempMessage"));
            Assert.IsTrue(tempMessage.Text.Contains("Der er for koldt") || tempMessage.Text.Contains("Der er for varmt") || tempMessage.Text.Contains("Der er helt tilpas"));

            IWebElement waterLevelMessage = _driver.FindElement(By.Id("waterLevelMessage"));
            Assert.IsTrue(waterLevelMessage.Text.Contains("Fuld op med vand") || waterLevelMessage.Text.Contains("Der er nok vand"));


        }

    }
}