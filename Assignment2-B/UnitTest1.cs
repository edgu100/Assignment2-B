using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using System.Collections.Generic;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Chrome;
using System.Text;
using System.Net;
using System.Linq;
using System.IO;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {

        IWebDriver driver;
        String emailAcc, firstname, lastname, password;  //login details

        //create the information
        public void Initialize()
        {
            driver = new ChromeDriver();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(50);
            emailAcc = RandomString(8) + "@gmail.com";
            password = "12345";
            lastname = RandomString(6);
            firstname = RandomString(6);
        }

        //login the account
        public void login()
        {
            driver.Navigate().GoToUrl("http://automationpractice.com");
            driver.FindElement(By.ClassName("login")).Click();
            driver.FindElement(By.XPath("//input[@id='email']")).SendKeys(emailAcc);
            driver.FindElement(By.XPath("//input[@id='passwd']")).SendKeys(password);
            driver.FindElement(By.XPath("//button[@id='SubmitLogin']")).Click();
            driver.FindElement(By.XPath("//a[@class='logout']"));
        }

        //create a random string
        private static string RandomString(int size)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            return builder.ToString();
        }

        //create an account and logout
        public void createAccount()
        {
            driver.Navigate().GoToUrl("http://automationpractice.com");
            driver.FindElement(By.ClassName("login")).Click();
            IWebElement emailAddr = driver.FindElement(By.XPath(".//*[@id='email_create']"));
            emailAddr.SendKeys(emailAcc);
            driver.FindElement(By.Id("SubmitCreate")).Click();
            driver.FindElement(By.XPath("//input[@id='id_gender1']")).Click();
            driver.FindElement(By.XPath("//input[@id='customer_firstname']")).SendKeys(firstname);
            driver.FindElement(By.XPath("//input[@id='customer_lastname']")).SendKeys(lastname);
            driver.FindElement(By.XPath("//input[@id='passwd']")).SendKeys(password);
            SelectElement bdDays = new SelectElement(driver.FindElement(By.XPath("//select[@id='days']")));
            bdDays.SelectByValue("25");
            SelectElement bdMonth = new SelectElement(driver.FindElement(By.XPath("//select[@id='months']")));
            bdMonth.SelectByValue("5");
            SelectElement bdYear = new SelectElement(driver.FindElement(By.XPath("//select[@id='years']")));
            bdYear.SelectByValue("1990");
            driver.FindElement(By.XPath("//input[@id='company']")).SendKeys("Wintec");
            driver.FindElement(By.XPath("//input[@id='address1']")).SendKeys("Tristram St");
            driver.FindElement(By.XPath("//input[@id='city']")).SendKeys("Hamilton");
            SelectElement state = new SelectElement(driver.FindElement(By.XPath("//select[@id='id_state']")));
            state.SelectByText("California");
            driver.FindElement(By.XPath("//input[@id='postcode']")).SendKeys("55555");
            SelectElement country = new SelectElement(driver.FindElement(By.XPath("//select[@id='id_country']")));
            country.SelectByText("United States");
            driver.FindElement(By.XPath("//input[@id='phone_mobile']")).SendKeys("5084561945");
            driver.FindElement(By.XPath("//button[@id='submitAccount']")).Click();
            driver.FindElement(By.XPath("//a[@class='logout']")).Click();
            return;
        }

        public Boolean CheckLink()
        {
            driver = new ChromeDriver();
            driver.Navigate().GoToUrl("http://automationpractice.com");
            HttpWebRequest request = null;
            List<IWebElement> elementlist = new List<IWebElement>(driver.FindElements(By.TagName("a")));  //choose all links
            List<IWebElement> imagelinks = new List<IWebElement>(driver.FindElements(By.TagName("img")));   //choose all imagelinks
            var newlist = elementlist.Concat(imagelinks);               //add links together
            bool i = true;
            TextWriter tw = new StreamWriter("C:/Users/edgu1/Desktop/333.txt");
            foreach (var list in newlist)
            {
                if (!list.Text.Contains("@") && list.Text != "" && list.GetAttribute("href") != "")            //set rules for links
                {
                    request = (HttpWebRequest)HttpWebRequest.Create(list.GetAttribute("href"));                 //get url information
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();                          //get response
                    tw.WriteLine($"URL: {list.GetAttribute("href")}      status is :{response.StatusCode}");    //write down information
                    String code = response.StatusCode.ToString();
                    if (code != "OK")
                    {
                        i = false;
                    }
                }
            }
            tw.Close();
            return i;
        }




        [TestMethod]
        public void TestCreateAccAndLogin()
        {
            Initialize();
            createAccount();
            login();
            driver.Quit();
        }

        [TestMethod]
        public void TestURL()
        {
            Boolean j = true;
            Boolean actual = CheckLink();
            Assert.AreEqual(j, actual);
            driver.Quit();
        }

        [TestMethod]
        public void TestOrder()
        {
            Initialize();
            createAccount();
            //login
            driver.FindElement(By.ClassName("login")).Click();
            driver.FindElement(By.XPath("//input[@id='email']")).SendKeys(emailAcc);
            driver.FindElement(By.XPath("//input[@id='passwd']")).SendKeys(password);
            driver.FindElement(By.XPath("//button[@id='SubmitLogin']")).Click();
            //choose the first item
            driver.FindElement(By.XPath("//a[@title='Women']")).Click();
            driver.FindElement(By.XPath("//img[@alt='Faded Short Sleeve T-shirts']")).Click();
            driver.FindElement(By.XPath("//button[@name='Submit']")).Click();
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(".//*[@id = 'layer_cart']/div[1]/div[2]/div[4]/span/span"))).Click();
            //choose the second item
            driver.FindElement(By.XPath("//a[@title='Women']"));
            driver.FindElement(By.XPath("//img[@alt='Blouse']")).Click();
            driver.FindElement(By.XPath("//button[@name='Submit']")).Click();
            var wait2 = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait2.Until(ExpectedConditions.ElementIsVisible(By.XPath(".//*[@id = 'layer_cart']/div[1]/div[2]/div[4]/span/span"))).Click();
            //choose the third item
            driver.FindElement(By.XPath("//a[@title='Women']"));
            driver.FindElement(By.XPath("//img[@alt='Printed Dress']")).Click();
            driver.FindElement(By.XPath("//button[@name='Submit']")).Click();
            var wait3 = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait3.Until(ExpectedConditions.ElementIsVisible(By.XPath(".//*[@id = 'layer_cart']/div[1]/div[2]/div[4]/a/span")));
            //pay
            driver.FindElement(By.XPath(".//*[@id='layer_cart']/div[1]/div[2]/div[4]/a/span")).Click();
            driver.FindElement(By.XPath(".//*[@id='center_column']/p[2]/a[1]/span")).Click();
            driver.FindElement(By.XPath(".//*[@id='center_column']/form/p/button")).Click();
            driver.FindElement(By.XPath(".//input[@id='cgv']")).Click();
            driver.FindElement(By.XPath("//button[@name='processCarrier']")).Click();
            driver.FindElement(By.XPath("//a[@class='bankwire']")).Click();
            driver.FindElement(By.XPath("//button[@class='button btn btn-default button-medium']")).Click();
            IWebElement orderComplete = driver.FindElement(By.XPath("//strong[contains(.,'Your order on My Store is complete.')]"));
            String orderCompletedExpected = "Your order on My Store is complete.";
            String orderCompletedActual = orderComplete.Text;
            Assert.AreEqual(orderCompletedActual, orderCompletedExpected);
            driver.Quit();
        }

        [TestMethod]
        public void TestCosine()
        {   //Initializes new instance of ChromeDriver class
            IWebDriver driver = new ChromeDriver();
            //open website
            driver.Navigate().GoToUrl("http://www.calculator.net");
            //click 
            driver.FindElement(By.XPath("//*[@id='sciout']/tbody/tr[2]/td[1]/div/div[1]/span[2]")).Click();
            driver.FindElement(By.XPath("//*[@id='sciout']/tbody/tr[2]/td[2]/div/div[1]/span[3]")).Click();
            driver.FindElement(By.XPath("//*[@id='sciout']/tbody/tr[2]/td[2]/div/div[4]/span[1]")).Click();
            
            //find result
            IWebElement resultText = driver.FindElement(By.XPath(".//*[@id='sciOutPut']"));
            double actual = double.Parse(resultText.Text);
            //test
            double expected = 0.0;
            Assert.AreEqual(actual, expected, 0.00001, "Incorrect results when adding five and five");
            driver.Quit();
            return;
        }

        [TestMethod]
        public void Testlog()
        {
            //Initializes new instance of ChromeDriver class
            IWebDriver driver = new ChromeDriver();
            //open website
            driver.Navigate().GoToUrl("http://www.calculator.net");
            //click 
            driver.FindElement(By.XPath("//*[@id='sciout']/tbody/tr[2]/td[1]/div/div[4]/span[5]")).Click();
            driver.FindElement(By.XPath("//*[@id='sciout']/tbody/tr[2]/td[2]/div/div[3]/span[1]")).Click();
            driver.FindElement(By.XPath("//*[@id='sciout']/tbody/tr[2]/td[2]/div/div[4]/span[1]")).Click();
            driver.FindElement(By.XPath("//*[@id='sciout']/tbody/tr[2]/td[2]/div/div[4]/span[1]")).Click();
            
            //find result
            IWebElement resultText = driver.FindElement(By.XPath(".//*[@id='sciOutPut']"));
            double actual = double.Parse(resultText.Text);
            //test
            double expected = 2.0;
            Assert.AreEqual(actual, expected, 0.00001, "Incorrect results when adding five and five");
            driver.Quit();
            return;
        }

        [TestMethod]
        public void TestXY()
        {
            //Initializes new instance of ChromeDriver class
            IWebDriver driver = new ChromeDriver();
            //open website
            driver.Navigate().GoToUrl("http://www.calculator.net");
            //click 
            driver.FindElement(By.XPath("//*[@id='sciout']/tbody/tr[2]/td[2]/div/div[3]/span[2]")).Click();
            driver.FindElement(By.XPath("//*[@id='sciout']/tbody/tr[2]/td[1]/div/div[3]/span[1]")).Click();
            driver.FindElement(By.XPath("//*[@id='sciout']/tbody/tr[2]/td[2]/div/div[3]/span[3]")).Click();
            driver.FindElement(By.XPath("//*[@id='sciout']/tbody/tr[2]/td[2]/div/div[5]/span[4]")).Click();
            //find result
            IWebElement resultText = driver.FindElement(By.XPath(".//*[@id='sciOutPut']"));
            double actual = double.Parse(resultText.Text);
            //test
            double expected = 8.0;
            Assert.AreEqual(actual, expected, 0.00001, "Incorrect results when adding five and five");
            driver.Quit();
            return;
        }

        [TestMethod]
        public void Testn()
        {
            //Initializes new instance of ChromeDriver class
            IWebDriver driver = new ChromeDriver();
            //open website
            driver.Navigate().GoToUrl("http://www.calculator.net");
            //click 
            driver.FindElement(By.XPath("//*[@id='sciout']/tbody/tr[2]/td[2]/div/div[2]/span[1]")).Click();
            driver.FindElement(By.XPath("//*[@id='sciout']/tbody/tr[2]/td[1]/div/div[5]/span[5]")).Click();
            //find result
            IWebElement resultText = driver.FindElement(By.XPath(".//*[@id='sciOutPut']"));
            double actual = double.Parse(resultText.Text);
            //test
            double expected = 24.0;
            Assert.AreEqual(actual, expected, 0.00001, "Incorrect results when adding five and five");
            driver.Quit();
            return;
        }



        [TestMethod]
        public void TestMethod1()
        {
            int totalCities = 0;
            int totalRegions = 0;
            IWebDriver driver = new ChromeDriver();
            TextWriter tw = new StreamWriter("C:/Users/edgu1/Desktop/333.txt");
            driver.Navigate().GoToUrl(" http://www.harcourts.co.nz/ ");
            IWebElement houseRegion = driver.FindElement(By.XPath("//*[@id='homeFeature']/form/ul/li[2]/ul[1]/li[1]/select"));
            SelectElement region = new SelectElement(houseRegion);


            foreach (IWebElement selectCity in region.Options)
            {
                region.SelectByText("all regions");
                if (selectCity.Text.Equals("all regions")) continue;
                totalRegions++;
                tw.WriteLine(selectCity.Text);
                region.SelectByText(selectCity.Text);
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                wait.Until(ExpectedConditions.ElementToBeClickable(driver.FindElement(By.XPath("//*[@id='homeFeature']/form/ul/li[2]/ul[1]/li[2]/select"))));
                IWebElement TownElement = driver.FindElement(By.XPath("//*[@id='homeFeature']/form/ul/li[2]/ul[1]/li[2]/select"));
                SelectElement towns = new SelectElement(TownElement);

                foreach (IWebElement m in towns.Options)
                {
                    if (m.Text.Equals("all districts")) continue;
                    totalCities++;
                    tw.WriteLine("--" + m.Text);
                }
            }

            tw.WriteLine("----------------------Totals-------------------");
            tw.WriteLine("total number of makes" + totalRegions);
            tw.WriteLine("total number of models" + totalCities);
            tw.Close();
            driver.Quit();
        }
    }

}