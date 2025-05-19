using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
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

            Assert.IsTrue(_driver.Title.Contains("Plante Juicing"));



            IWebElement moistureMessage = _driver.FindElement(By.Id("moistureMessage"));
            Assert.IsTrue(moistureMessage.Text.Contains("Du skal vande din plante!") || moistureMessage.Text.Contains("") || moistureMessage.Text.Contains("Din plante har det fint!"));

            IWebElement tempMessage = _driver.FindElement(By.Id("tempMessage"));
            Assert.IsTrue(tempMessage.Text.Contains("Temperaturen er for lav!") || tempMessage.Text.Contains("") || tempMessage.Text.Contains("Temperaturen er for høj!") || tempMessage.Text.Contains("Temperaturen er perfekt!"));

            IWebElement waterLevelMessage = _driver.FindElement(By.Id("waterLevelMessage"));
            Assert.IsTrue(waterLevelMessage.Text.Contains("Vandstanden er for lav!") || waterLevelMessage.Text.Contains("") || waterLevelMessage.Text.Contains("Vandstanden er for høj!") || waterLevelMessage.Text.Contains("Vandstanden er passende."));

            // PlanteGuideTests
            IWebElement PlanteGuideButton = _driver.FindElement(By.Id("PlanteGuide"));
            PlanteGuideButton.Click();
            Assert.AreEqual("PlanteGuide", _driver.Title);

            // Tester om den første plante i vores PlanteGuide passer
            // Vent på at <ul> elementet bliver synligt
            IWebElement plantList = wait.Until(d => d.FindElement(By.Id("plant-list")));
            wait.Until(d => plantList.Displayed);

            // Find alle <li> elementer (planter) i listen
            var firstPlant = plantList.FindElements(By.TagName("li")).First();
            var nameElement = firstPlant.FindElement(By.TagName("a"));
            var planteNavn = nameElement.Text;
            Assert.AreEqual(planteNavn, "European Silver Fir");

            var addButton = firstPlant.FindElement(By.TagName("button"));
            addButton.Click();

            //Alert knappen
            wait.Until(d => _driver.SwitchTo().Alert());
            IAlert alert = _driver.SwitchTo().Alert();
            alert.Accept();

            // Test af "Back" knappen
            wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(20));
            IWebElement backButton = wait.Until(d => d.FindElement(By.Id("Back")));
            backButton.Click();
            Assert.AreEqual("Plante Juicing", _driver.Title);

            IWebElement PlantTable = wait.Until(d => d.FindElement(By.Id("AllPlants")));
            ReadOnlyCollection<IWebElement> rows = PlantTable.FindElements(By.TagName("tr"));
            IWebElement firstRow = rows[0];
            IWebElement newestRow = rows.Last();
            string PlantName = newestRow.FindElement(By.XPath(".//td[2]")).Text;

            // Test af "Detaljer" knappen
            var buttons = _driver.FindElements(By.ClassName("details-button"));
            buttons[0].Click();
            wait.Until(d => _driver.Title.Contains("Plante Detaljer"));
            Assert.AreEqual("Plante Detaljer", _driver.Title);

            // Back to drivhus
            var buttons2 = _driver.FindElements(By.ClassName("back-button"));
            buttons2[0].Click();
            wait.Until(d => _driver.Title.Contains("Plante Juicing"));
            Assert.AreEqual("Plante Juicing", _driver.Title);

            // Test af "Slet" knappen
            var buttons3 = _driver.FindElements(By.ClassName("delete-button"));
            buttons3[0].Click();
            wait.Until(d => _driver.SwitchTo().Alert());
            IAlert alert2 = _driver.SwitchTo().Alert();
            alert2.Accept();
            wait.Until(d => _driver.SwitchTo().Alert());
            IAlert alert3 = _driver.SwitchTo().Alert();
            alert3.Accept();
            Assert.AreEqual("Plante Juicing", _driver.Title);
            bool messageFound = _driver.PageSource.Contains("Ingen planter tilføjet endnu.");
            Assert.IsTrue(messageFound, "The message 'Ingen planter tilføjet endnu.");

            // Test af søgefunktion
            IWebElement PlanteGuideButton2 = _driver.FindElement(By.Id("PlanteGuide"));
            PlanteGuideButton2.Click();
            Assert.AreEqual("PlanteGuide", _driver.Title);
            IWebElement searchBox = wait.Until(d => d.FindElement(By.Id("searchBox")));
            searchBox.SendKeys("apple");
            var searchButton = _driver.FindElement(By.Id("searchButton"));
            searchButton.Click();
            IWebElement plantList2 = wait.Until(d => d.FindElement(By.Id("plant-list")));
            wait.Until(d => plantList2.Displayed);
            var firstPlant2 = plantList2.FindElements(By.TagName("li")).First();
            var nameElement2 = firstPlant2.FindElement(By.TagName("a"));
            var planteNavn2 = nameElement2.Text;
            Assert.AreEqual(planteNavn2, "Strawberry Tree");

            // Ryd søgefunktionen
            var clearSearchButton = _driver.FindElement(By.Id("clearSearchButton"));
            clearSearchButton.Click();
            IWebElement searchBox2 = wait.Until(d => d.FindElement(By.Id("searchBox")));
            string placeholder = searchBox2.GetAttribute("placeholder");
            Assert.AreEqual(placeholder, "Søg efter planter...");

            // Næste side test
            IWebElement currentPageInfo = wait.Until(d => d.FindElement(By.Id("currentPageInfo")));
            Assert.IsTrue(currentPageInfo.Text.Contains("Side 1 af 337"));
            var nextPage = wait.Until(d => d.FindElement(By.Id("nextPage")));
            nextPage.Click();
            IWebElement currentPageInfo2 = wait.Until(d => d.FindElement(By.Id("currentPageInfo")));
            Assert.IsTrue(currentPageInfo2.Text.Contains("Side 2 af 337"));

            // Forrige side test
            var prevPage = wait.Until(d => d.FindElement(By.Id("prevPage")));
            prevPage.Click();
            IWebElement currentPageInfo3 = wait.Until(d => d.FindElement(By.Id("currentPageInfo")));
            Assert.IsTrue(currentPageInfo3.Text.Contains("Side 1 af 337"));

            // Test af "Tilføj til mine planter" knap
            wait.Until(d => d.FindElement(By.Id("plant-list")));
            IWebElement plantLink = _driver.FindElement(By.LinkText("European Silver Fir"));
            plantLink.Click();
            IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;
            js.ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");
            var buttons5 = _driver.FindElements(By.ClassName("succes-button"));
            buttons5[1].Click();
            wait.Until(d => _driver.SwitchTo().Alert());
            IAlert alert4 = _driver.SwitchTo().Alert();
            alert4.Accept();
            js.ExecuteScript("window.scrollTo(0,0);");
            var buttons6 = _driver.FindElement(By.Id("BackToGreenhouse"));
            buttons6.Click();
            wait.Until(d => _driver.Title.Contains("Plante Juicing"));

            //// Test af "Tilbage til planteguide" knap inde på en plantes detaljer
            //var buttons4 = _driver.FindElements(By.ClassName("back-button"));
            //buttons4[1].Click();
            //wait.Until(d => _driver.Title.Contains("PlanteGuide"));
            //Assert.AreEqual("PlanteGuide", _driver.Title);

            //// forrige kode 
            //wait.Until(d => d.FindElement(By.Id("plant-list")));
            //IWebElement plantLink = _driver.FindElement(By.LinkText("European Silver Fir"));
            //plantLink.Click();
            //wait.Until(d => _driver.Title.Contains("Plante Detaljer"));
            //Assert.AreEqual("Plante Detaljer", _driver.Title);
            //var backToPlanter = _driver.FindElement(By.Id("backToPlanter"));
            //backToPlanter.Click();
            //wait.Until(d => _driver.Title.Contains("PlanteGuide"));



            //// Test af "Tilbage til drivhus" knap inde på en plantes detaljer
            //var plantList4 = wait.Until(d => d.FindElement(By.Id("plant-list")));
            //firstPlant.Click();
            //wait.Until(d => _driver.Title.Contains("Plante Detaljer"));
            //Assert.AreEqual("Plante Detaljer", _driver.Title);
            //var backToGreenhouse = _driver.FindElement(By.Id("BackToGreenhouse"));
            //backToGreenhouse.Click();
            //wait.Until(d => _driver.Title.Contains("Plante Juicing"));
        }

    }
}
