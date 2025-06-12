using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

class Program
{
    static readonly string originalProfilePath = @"C:\Users\USUARIO\AppData\Local\Google\Chrome\User Data\Profile 2";
    static readonly string clonedProfilePath = @"C:\GitHub\SeleniumBotNet9\ChromeUserData";

    // Declara driver estático para ser acessado no handler Ctrl+C
    static ChromeDriver? driver = null;

    static void Main(string[] args)
    {
        Console.CancelKeyPress += (sender, e) =>
        {
            Console.WriteLine("\nCtrl+C detectado! Finalizando driver...");
            driver?.Quit();
            driver = null;
            e.Cancel = true; // cancela encerramento imediato para cleanup
            Environment.Exit(0); // encerra aplicação
        };

        if (!Directory.Exists(clonedProfilePath))
        {
            Console.WriteLine("Perfil clonado não encontrado, iniciando cópia...");
            CopyDirectory(originalProfilePath, clonedProfilePath);
            Console.WriteLine("Cópia concluída.");
            Console.WriteLine("Agora, abra o Chrome com este perfil para fazer login manualmente:");
            Console.WriteLine($"chrome.exe --user-data-dir=\"{clonedProfilePath}\" --profile-directory=Default");
            Console.WriteLine("Após o login, feche o Chrome e rode este programa novamente.");
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
            Console.WriteLine("Chrome iniciado com perfil clonado.");

            string url = "https://myactivity.google.com/page?hl=pt_BR&page=youtube_comment_likes";
            Console.WriteLine($"[{DateTime.Now}] Navegando para {url}...");

            driver.Navigate().GoToUrl(url);

            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            wait.Until(d => ((IJavaScriptExecutor)d!).ExecuteScript("return document.readyState")!.Equals("complete"));

            Console.WriteLine("\nPágina carregada.");

            Console.WriteLine("Aguardando botão de deletar ficar disponível...");

            string xpathBotaoX = "/html/body/c-wiz/div/div[2]/c-wiz/c-wiz/div/div[1]/c-wiz[1]/div/div/div[1]/div[2]/div/button";

            wait.Timeout = TimeSpan.FromMinutes(10);

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
                Console.WriteLine("Botão de deletar não encontrado.");
                return;
            }

            Console.WriteLine("Botão de deletar encontrado, iniciando processo...");

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
                        Console.WriteLine("Nenhum botão restante.");
                        break;
                    }

                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({ behavior: 'smooth', block: 'center' });", botaoX);
                    Thread.Sleep(1000);

                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", botaoX);

                    contador++;
                    Console.WriteLine($"[{contador}] Botão de deletar clicado.");
                    Thread.Sleep(3000);
                }
                catch (WebDriverTimeoutException)
                {
                    Console.WriteLine("Nenhum botão restante. Lista esvaziada.");
                    break;
                }
                catch (ElementClickInterceptedException ex)
                {
                    Console.WriteLine($"Erro ao clicar: {ex.Message}");
                    Thread.Sleep(2000);
                }
                catch (StaleElementReferenceException)
                {
                    Console.WriteLine("Elemento stale detectado, tentando novamente...");
                    Thread.Sleep(1000);
                    continue;
                }
                catch (NoSuchElementException)
                {
                    Console.WriteLine("Nenhum botão restante (NoSuchElementException).");
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro durante execução: {ex.Message}");
            Console.WriteLine($"Stack Trace: {ex.StackTrace}");
        }
        finally
        {
            Console.WriteLine("Finalizando em 5 segundos...");
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
