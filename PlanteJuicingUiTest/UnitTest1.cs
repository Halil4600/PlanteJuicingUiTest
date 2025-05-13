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
        private static WebDriverWait wait;

        [ClassInitialize]
        public static void Setup(TestContext context)
        {
            _driver = new ChromeDriver(DriverDirectory);
            wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        }

        [ClassCleanup]
        public static void TearDown()
        {
            _driver.Dispose();
        }

        [TestMethod]
        public void MoistureAppearsTest()
        {
            string url = "https://plantejuicing-a9hfcaf3fhgccdgw.canadacentral-01.azurewebsites.net/"; // ? Ret til din lokale sti/server
            _driver.Navigate().GoToUrl(url);

            Assert.IsTrue(_driver.Title.Contains("PlanteJuicing"));



            IWebElement moistureMessage = _driver.FindElement(By.Id("moistureMessage"));
            Assert.IsTrue(moistureMessage.Text.Contains("Du skal vande din plante!") || moistureMessage.Text.Contains("Din plante har det fint!"));

            IWebElement tempMessage = _driver.FindElement(By.Id("tempMessage"));
            Assert.IsTrue(tempMessage.Text.Contains("Temperaturen er for lav!") || tempMessage.Text.Contains("Temperaturen er for høj!") || tempMessage.Text.Contains("Temperaturen er perfekt!"));

            IWebElement waterLevelMessage = _driver.FindElement(By.Id("waterLevelMessage"));
            Assert.IsTrue(waterLevelMessage.Text.Contains("Vandstanden er for lav!") || waterLevelMessage.Text.Contains("Vandstanden er for høj!") || waterLevelMessage.Text.Contains("Vandstanden er passende!"));

            // PlanteGuideTests
            IWebElement PlanteGuideButton = _driver.FindElement(By.Id("PlanteGuide"));
            PlanteGuideButton.Click();
            Assert.AreEqual("PlanteGuide", _driver.Title);

            // Tester om den første plante i vores PlanteGuide passer
            wait.Until(d => d.FindElements(By.CssSelector(".list-group-item")).Count > 0);

            // Find den første plante
            var forstePlante = _driver.FindElement(By.CssSelector(".list-group-item:first-child a"));
            string planteNavn = forstePlante.Text;

            // Her kan du erstatte "Forventet plantenavn" med det navn du forventer
            string forventetNavn = "European Silver Fir";

            // Assert at plantens navn matcher det forventede
            Assert.AreEqual(forventetNavn, planteNavn, "Den første plantes navn matcher ikke det forventede navn");

            var firstAddButton = wait.Until(d => d.FindElement(By.CssSelector(".add-button")));
            firstAddButton.Click();

            IWebElement BackButton = _driver.FindElement(By.Id("Back"));
            BackButton.Click();
            Assert.AreEqual("PlanteJuicing", _driver.Title);

            IWebElement PlantTable = wait.Until(d => d.FindElement(By.Id("AllPlants")));
            ReadOnlyCollection<IWebElement> rows = PlantTable.FindElements(By.TagName("tr"));
            IWebElement firstRow = rows[0];
            IWebElement newestRow = rows.Last();
            string PlantName = newestRow.FindElement(By.XPath(".//td[2]")).Text;
            
        }

    }
}