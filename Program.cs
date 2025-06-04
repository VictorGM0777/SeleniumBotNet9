using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.IO;
using System.Threading;

class Program
{
    // Caminho do perfil original (ajuste seu usuário e perfil)
    static string originalProfilePath = @"C:\Users\USUARIO\AppData\Local\Google\Chrome\User Data\Profile 2";
    // Caminho onde o perfil clonado ficará (pasta exclusiva para o bot)
    static string clonedProfilePath = @"C:\GitHub\SeleniumBotNet9\ChromeUserData";

    static void Main(string[] args)
    {
        // Se não existir o perfil clonado, copia do original
        if (!Directory.Exists(clonedProfilePath))
        {
            Console.WriteLine("Perfil clonado não encontrado, iniciando cópia...");
            CopyDirectory(originalProfilePath, clonedProfilePath);
            Console.WriteLine("Cópia concluída.");
            Console.WriteLine("Agora, abra o Chrome com este perfil para fazer login manualmente:");
            Console.WriteLine($"chrome.exe --user-data-dir=\"{clonedProfilePath}\" --profile-directory=Default");
            Console.WriteLine("Após o login, feche o Chrome e rode este programa novamente.");
            return; // Termina para você fazer login primeiro
        }

        var options = new ChromeOptions();
        options.BinaryLocation = @"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe";

        options.AddArguments(new[]
        {
            $"--user-data-dir={clonedProfilePath}",
            "--profile-directory=Default",
            "--start-maximized",
            "--new-window"
        });

        try
        {
            using (var driver = new ChromeDriver(options))
            {
                Console.WriteLine("Chrome iniciado com perfil clonado.");

                Thread.Sleep(5000);

                string url = "https://myactivity.google.com/page?hl=pt_BR&page=youtube_comment_likes&pli=1";
                Console.WriteLine($"Navegando para {url}");
                driver.Navigate().GoToUrl(url);

                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
                wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));

                Console.WriteLine("Página carregada.");
                Thread.Sleep(10000);

                string xpathBotaoX = "/html/body/c-wiz/div/div[2]/c-wiz/c-wiz/div/div[1]/c-wiz[1]/div/div/div[1]/div[2]/div/button";
                int contador = 0;

                while (true)
                {
                    try
                    {
                        // Espera até o botão estar presente e clicável
                        wait.Timeout = TimeSpan.FromSeconds(10);
                        var botaoX = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.XPath(xpathBotaoX)));
                        botaoX.Click();
                        contador++;
                        Console.WriteLine($"[{contador}] Botão de deletar clicado.");

                        // Espera para a página atualizar após o clique
                        Thread.Sleep(3000);
                    }
                    catch (WebDriverTimeoutException)
                    {
                        Console.WriteLine("Nenhum botão restante ou timeout ao encontrar botão. Lista esvaziada.");
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
                    }
                    catch (NoSuchElementException)
                    {
                        Console.WriteLine("Nenhum botão restante (NoSuchElementException).");
                        break;
                    }
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
