using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

class Program
{
    static readonly string originalProfilePath = @"C:\Users\USUARIO\AppData\Local\Google\Chrome\User Data\Profile 2";
    static readonly string clonedProfilePath = @"C:\GitHub\SeleniumBotNet9\ChromeUserData";

    // Declares static driver to be accessed in the Ctrl+C handler
    static ChromeDriver? driver = null;

    static void Main(string[] args)
    {
        Console.CancelKeyPress += (sender, e) =>
        {
            Console.WriteLine("\nCtrl+C detected! Shutting down driver...");
            driver?.Quit();
            driver = null;
            e.Cancel = true; // cancels immediate termination to allow cleanup
            Environment.Exit(0); // terminates the application
        };

        if (!Directory.Exists(clonedProfilePath))
        {
            Console.WriteLine("Cloned profile not found, starting copy...");
            CopyDirectory(originalProfilePath, clonedProfilePath);
            Console.WriteLine("Copy completed.");
            Console.WriteLine("Now, open Chrome with this profile to log in manually:");
            Console.WriteLine($"chrome.exe --user-data-dir=\"{clonedProfilePath}\" --profile-directory=Default");
            Console.WriteLine("After logging in, close Chrome and run this program again.");
            return;
        }

        var options = new ChromeOptions
        {
            BinaryLocation = @"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe"
        };

        options.AddArguments(
        [
            $"--user-data-dir={clonedProfilePath}",
            "--profile-directory=Default",
            "--start-maximized",
            "--new-window"
        ]);

        var driverService = ChromeDriverService.CreateDefaultService();
        driverService.HideCommandPromptWindow = true;
        var commandTimeout = TimeSpan.FromMinutes(3);

        try
        {
            driver = new ChromeDriver(driverService, options, commandTimeout);
            Console.WriteLine("\nChrome started with cloned profile.");

            string url = "https://myactivity.google.com/page?hl=pt_BR&page=youtube_comment_likes";
            Console.WriteLine($"[{DateTime.Now}] Navigating to {url}");

            driver.Navigate().GoToUrl(url);

            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            wait.Until(d => ((IJavaScriptExecutor)d!).ExecuteScript("return document.readyState")!.Equals("complete"));

            Console.WriteLine("Page loaded.");

            Console.WriteLine("Waiting for delete button to become available (may take a while for many likes on old video comments from 2020 or earlier)...");

            string xpathBotaoX = "/html/body/c-wiz/div/div[2]/c-wiz/c-wiz/div/div[1]/c-wiz[1]/div/div/div[1]/div[2]/div/button";

            wait.Timeout = TimeSpan.FromMinutes(15); // or more if there are many likes

            IWebElement botaoX = wait.Until(d =>
            {
                try
                {
                    var element = d.FindElement(By.XPath(xpathBotaoX));
                    return (element.Displayed && element.Enabled) ? element : null;
                }
                catch (NoSuchElementException)
                {
                    return null;
                }
            });

            if (botaoX == null)
            {
                Console.WriteLine("Delete button not found.");
                return;
            }

            Console.WriteLine("Delete button found, starting process...");

            int contador = 0;

            while (true)
            {
                try
                {
                    botaoX = wait.Until(d =>
                    {
                        try
                        {
                            var element = d.FindElement(By.XPath(xpathBotaoX));
                            return (element.Displayed && element.Enabled) ? element : null;
                        }
                        catch
                        {
                            return null;
                        }
                    });

                    if (botaoX == null)
                    {
                        Console.WriteLine("No buttons remaining.");
                        break;
                    }

                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({ behavior: 'smooth', block: 'center' });", botaoX);
                    Thread.Sleep(1000);

                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", botaoX);

                    contador++;
                    Console.WriteLine($"[{contador}] Delete button clicked.");
                    Thread.Sleep(3000);
                }
                catch (WebDriverTimeoutException)
                {
                    Console.WriteLine("No buttons remaining. List emptied.");
                    break;
                }
                catch (ElementClickInterceptedException ex)
                {
                    Console.WriteLine($"Error clicking: {ex.Message}");
                    Thread.Sleep(2000);
                }
                catch (StaleElementReferenceException)
                {
                    Console.WriteLine("Stale element detected, retrying...");
                    Thread.Sleep(1000);
                    continue;
                }
                catch (NoSuchElementException)
                {
                    Console.WriteLine("No buttons remaining (NoSuchElementException).");
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during execution: {ex.Message}");
            Console.WriteLine($"Stack Trace: {ex.StackTrace}");
        }
        finally
        {
            Console.WriteLine("Shutting down in 5 seconds...");
            Thread.Sleep(5000);
            driver?.Quit();
        }
    }

    static void CopyDirectory(string sourceDir, string destDir)
    {
        Directory.CreateDirectory(destDir);

        foreach (string file in Directory.GetFiles(sourceDir))
        {
            string dest = Path.Combine(destDir, Path.GetFileName(file));
            File.Copy(file, dest, true);
        }

        foreach (string folder in Directory.GetDirectories(sourceDir))
        {
            string dest = Path.Combine(destDir, Path.GetFileName(folder));
            CopyDirectory(folder, dest);
        }
    }
}
