using System;
using System.IO;
using System.Reflection;
using ClearMeasure.OnionDevOpsArchitecture.Core.Model;
using ClearMeasure.OnionDevOpsArchitecture.IntegrationTests;
using NUnit.Framework;
using OpenQA.Selenium; // Libreiria che offre comandi di C# che permettono di fare dei robo test
using OpenQA.Selenium.Chrome;
using Shouldly;

namespace ClearMeasure.OnionDevOpsArchitecture.AcceptanceTests
{
    public class GetAllExpenseReportsTester
    {
        private string _appUrl;
        private IWebDriver _driver;

        [OneTimeSetUp]
        public void Setup()
        {
            _appUrl = new DataConfigurationStub().GetValue("AppUrl", Assembly.GetExecutingAssembly());
            _driver = new ChromeDriver(".");
            new ZDataLoader().LoadLocalData();
        }

        [OneTimeTearDown]
        public void Teardown()
        {
            _driver.Close();
            _driver.Quit();
            _driver.Dispose();
        }

        [TestCase("000001", TestName = "Should add new expense report numbered '000001'")]
        [TestCase("000010", TestName = "Should add new expense report numbered '000010'")]
        [TestCase("000100", TestName = "Should add new expense report numbered '000100'")]
        [TestCase("001000", TestName = "Should add new expense report numbered '001000'")]
        [TestCase("010000", TestName = "Should add new expense report numbered '010000'")]
        [TestCase("100000", TestName = "Should add new expense report numbered '100000'")]
        public void ShouldBeAbleToAddNewExpenseReport(string expenseReportNumber)
        {
            // Provare un elemento con il nome richiesto, cliccarlo
            void ClickLink(string linkText)
            {
                _driver.FindElement(By.LinkText(linkText)).Click();
            }

            void TypeText(string elementName, string text)
            {
                var numberTextBox = _driver.FindElement(By.Name(elementName));
                numberTextBox.SendKeys(text);
            }

            // Provocare eventi a livelli di browser
            Console.WriteLine($"Navigating to {_appUrl}");
            _driver.Navigate().GoToUrl(_appUrl + "/"); // Andare ad un url
            _driver.Manage().Window.Maximize(); // Espandere la finestra 
            TakeScreenshot($"{expenseReportNumber}-Step1Arrange"); // Fare uno screenshot per fare una prova grafica per verificare se è andato tutto bene

            ClickLink("Add New");
            
            TypeText(nameof(ExpenseReport.Number), expenseReportNumber);
            TypeText(nameof(ExpenseReport.Title), "some title");
            TypeText(nameof(ExpenseReport.Description), "some desc");
            
            TakeScreenshot($"{expenseReportNumber}-Step2Act");

            _driver.FindElement(By.TagName("form")).Submit();

            TakeScreenshot($"{expenseReportNumber}-Step3Assert");

            // Estrapolare valori presenti in una determinata area della pagina html per verificare che sono quelli che mi aspetto
            var numberCells = _driver.FindElements(
                By.CssSelector($"td[data-expensereport-property=\"{nameof(ExpenseReport.Number)}\"][data-value=\"{expenseReportNumber}\"]"));
            numberCells.Count.ShouldBeGreaterThan(0);
            numberCells[0].Text.ShouldBe(expenseReportNumber);
            
        }

        private void TakeScreenshot(string fileName)
        {
            var chromeDriver = ((ChromeDriver) _driver);
            chromeDriver.GetScreenshot().SaveAsFile($"{fileName}.png");
            TestContext.AddTestAttachment($"{fileName}.png");
        }
    }
}