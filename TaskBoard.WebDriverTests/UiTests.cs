using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace TaskBoard.WebDriverTests
{
    public class UiTests
    {
        private const string url = "https://taskboard.rumenraychev74.repl.co";
        private WebDriver driver;

        [OneTimeSetUp]
        public void OpenBowser()
        {
            this.driver = new FirefoxDriver();
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        }
        [OneTimeTearDown]
        public void CloseBrowser()
        {
            this.driver.Quit();
        }

        [Test]
        public void Test_ListContacs_CheckFirstContact()
        {
            //Arrenge
            driver.Navigate().GoToUrl(url);
            var contactLink = driver.FindElement(By.LinkText("Task Board"));

            //Act
            contactLink.Click();

            //Assert
            var title = driver.FindElement(By.CssSelector("#task1 > tbody:nth-child(1) > tr:nth-child(1) > th:nth-child(1)")).Text;
            var discriptionName = driver.FindElement(By.CssSelector("#task1 > tbody:nth-child(1) > tr:nth-child(1) > td:nth-child(2)")).Text;

            Assert.That(title, Is.EqualTo("Title"));
            Assert.That(discriptionName, Is.EqualTo("Project skeleton"));

        }
        [Test]
        public void Test_SearchContacs_CheckFirstResults()
        {
            //Arrenge
            driver.Navigate().GoToUrl(url);
            driver.FindElement(By.LinkText("Search")).Click();

            //Act
            var searchField = driver.FindElement(By.Id("Keyword"));
            searchField.SendKeys("Home");
            driver.FindElement(By.Id("search")).Click();

            //Assert
            var title = driver.FindElement(By.CssSelector(".title > th:nth-child(1)")).Text;
            var discriptionName = driver.FindElement(By.CssSelector(".title > td:nth-child(2)")).Text;

            Assert.That(title, Is.EqualTo("Title"));
            Assert.That(discriptionName, Is.EqualTo("Home page"));

        }
        [Test]
        public void Test_SearchContacs_EmptytResults()
        {
            //Arrenge
            driver.Navigate().GoToUrl(url);
            driver.FindElement(By.LinkText("Search")).Click();

            //Act
            var searchField = driver.FindElement(By.Id("keyword"));
            searchField.SendKeys("invalid2635");
            driver.FindElement(By.Id("search")).Click();

            //Assert
            var resultLebel = driver.FindElement(By.Id("searchResult")).Text;

            Assert.That(resultLebel, Is.EqualTo("No tasks found."));
        }
        [Test]
        public void Test_CreateContact_InvalidData()
        {
            //Arrenge
            driver.Navigate().GoToUrl(url);
            driver.FindElement(By.LinkText("Create")).Click();

            //Act
            var description = driver.FindElement(By.Id("description"));
            description.SendKeys("riro");
            driver.FindElement(By.Id("create")).Click();

            //Assert
            var errorMessage = driver.FindElement(By.CssSelector(".err")).Text;

            Assert.That(errorMessage, Is.EqualTo("Error: Title cannot be empty!"));
        }
        [Test]
        public void Test_CreateContacts_ValidData()
        {
            // Arrange
            driver.Navigate().GoToUrl(url);
            driver.FindElement(By.LinkText("Create")).Click();

            var titleSend = "Ibre";
            var descriptionSend = "Abre";
            
            // Act
            driver.FindElement(By.Id("title")).SendKeys(titleSend);
            driver.FindElement(By.Id("description")).SendKeys(descriptionSend);
                       
            var buttonCreate = driver.FindElement(By.Id("create"));
            buttonCreate.Click();


            // Assert
            var taskTitle = driver.FindElements(By.CssSelector("#task8 > tbody:nth-child(1) > tr:nth-child(1) > td:nth-child(2)")).Last().Text;
            var taskDescription = driver.FindElements(By.CssSelector("#task11 > tbody:nth-child(1) > tr:nth-child(2) > td:nth-child(2) > div:nth-child(1)")).Last().Text;

            Assert.That(taskTitle, Is.EqualTo(titleSend));
            Assert.That(taskDescription, Is.EqualTo(descriptionSend));
        }
    }
}