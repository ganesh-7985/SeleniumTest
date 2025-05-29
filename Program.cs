using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;

class ResilientFormAutomation
{
    static void Main(string[] args)
    {
        IWebDriver driver = new ChromeDriver();

        try
        {
            driver.Navigate().GoToUrl("https://app.cloudqa.io/home/AutomationPracticeForm");
            driver.Manage().Window.Maximize();
            
            int maxWaitSeconds = 10;
            var fieldDefinitions = new Dictionary<string, (string Value, List<By> Locators)>()
            {
                ["First Name"] = ("Ganesh", new List<By> {
                    By.CssSelector("input.form-control[placeholder='Name']"),
                    By.XPath("//label[contains(text(),'First Name')]/following-sibling::input"),
                    By.Id("fname"),
                    By.Name("First Name")
                }),
                ["Last Name"] = ("Nalla", new List<By> {
                    By.CssSelector("input.form-control[placeholder='Surname']"),
                    By.XPath("//label[contains(text(),'Last Name')]/following-sibling::input"),
                    By.Id("lname"), 
                    By.Name("Last Name")
                }),
                ["Email"] = ("Shankarganesh@gmail.com", new List<By> {
                    By.CssSelector("input.form-control[placeholder='Email']"),
                    By.XPath("//label[contains(text(),'Email')]/following-sibling::input"),
                    By.Id("email"),
                    By.Name("Email")
                })
            };

            bool allSuccess = true;
            foreach (var field in fieldDefinitions)
            {
                bool success = FillFieldWithStrategies(driver, maxWaitSeconds, field.Key, field.Value.Value, field.Value.Locators);
                allSuccess = allSuccess && success;
            }

            Console.WriteLine(allSuccess ? "All fields filled successfully!" : " Some fields couldn't be filled. See above for details.");
            System.Threading.Thread.Sleep(3000);
        }
        catch (Exception ex)
        {
            Console.WriteLine($" Critical error: {ex.Message}");
        }
        finally
        {
            driver.Quit();
        }
    }

    static bool FillFieldWithStrategies(IWebDriver driver, int maxWait, string fieldName, string value, List<By> locators)
    {
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(maxWait));
        
        foreach (var locator in locators)
        {
            try
            {
                var element = wait.Until(d =>
                {
                    try
                    {
                        var e = d.FindElement(locator);
                        return e.Displayed && e.Enabled ? e : null;
                    }
                    catch
                    {
                        return null;
                    }
                });

                if (element != null)
                {
                    element.Clear();
                    element.SendKeys(value);
                    Console.WriteLine($"✓ {fieldName} filled using {GetLocatorDescription(locator)}");
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($" {fieldName} not found with {GetLocatorDescription(locator)}: {ex.Message}");
            }
        }

        Console.WriteLine($" Failed to fill {fieldName} after trying {locators.Count} strategies");
        return false;
    }

    static string GetLocatorDescription(By locator)
    {
        return locator.ToString().Split(':').Last().Trim();
    }
}