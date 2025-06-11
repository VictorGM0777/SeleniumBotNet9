# SeleniumBot 2025: Delete Youtube likes on comments My Google Activity .NET 9 C# on Windows with VS Code and Google Chrome
# SeleniumBot 2025: Deletar likes/gostei em comentários do Youtube na página My Google Activity usando .NET 9 C# no Windows com VS Code e Google Chrome

Esse projeto contém um bot feito com package Selenium no SDK .NET 9 com C# Console Application, que abre o Google Chrome na conta prévia logada pelo usuário, acessa a página de likes em comentários do Youtube e deleta manualmente um por um, já que essa função ainda não existe.

Dependência: SDK .NET 9
[.NET](https://dotnet.microsoft.com/pt-br/download/dotnet/9.0)

IDE: Visual Studio Code
[VSCode](https://code.visualstudio.com/download)

## Rodando o código pelo Visual Studio Code após instalação do .NET 9 SDK

1. Instale o Selenium e o driver específico para o navegador Chrome via Terminal:

```console
dotnet add package Selenium.WebDriver
```

```console
dotnet add package Selenium.WebDriver.ChromeDriver
```

2. Na linha 11 do arquivo *Program.cs* altere a variável originalProfilePath com seu USUARIO e a pasta do Windows onde os dados do perfil se encontram após ter se logado préviamente no Google Chrome pelo menos uma vez e fechado o browser na sequência. Pode estar na pasta Default, Profle 1 ou Profile 2, etc. Se tiver dúvidas, acesse o link chrome://version pelo próprio navegador para identificar e mude o final do path.
Na linha 13 do arquivo *Program.cs* também altere a localização onde clonou o projeto na sua máquina.

```csharp
static string originalProfilePath = @"C:\Users\USUARIO\AppData\Local\Google\Chrome\User Data\Profile 2";

static string clonedProfilePath = @"C:\GitHub\SeleniumBotNet9\ChromeUserData";
```
3. Na linha 30 do arquivo *Program.cs* valide a localização da instalação do seu Google Chrome no Windows.

```csharp
options.BinaryLocation = @"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe";
```

4. Na pasta SeleniumBot navegue via comando "cd" até o arquivo *Program.cs* e execute o comando abaixo,

```console
dotnet run .\Program.cs
```

5. Na primeiro execução irá gerar a pasta ChromeUserData dentro do próprio projeto, é um clone do seu perfil, feito isso, siga as instruções do Console ou do passo 5.

6. Agora, navegue via CMD/Prompt de comando até o executável do Chrome e execute o comando em sequência para abrir. Logue com seu perfil manualmente e depois feche o browser.

```console
cd C:\Program Files (x86)\Google\Chrome\Application
```

```console
chrome.exe --user-data-dir=\"{clonedProfilePath}\" --profile-directory=Default
```

7. Execute novamente o projeto, aguarde um pouco até a ida para a url https://myactivity.google.com/page?hl=pt_BR&page=youtube_comment_likes e a exclusão de comentários deve iniciar. Em caso de falha, rode o programa novamente e deixe o browser e VS Code abertos sem interação para que não interrompa o processo. Se a página do Google demorar mais que 90 segundos para carregar os likes, limpe o histórico pelo browser ou pela ferramenta da loja de apps da Microsoft "PC Manager" e abra manualmente ela antes de executar essa aplicação para garantir que os likes em comentários estão carregando.

```console
dotnet run .\Program.cs
```
