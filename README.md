# SeleniumBot 2025 Delete Youtube likes on comments My Google Activity .NET 9 C# on Windows

Esse projeto contém um bot feito com package Selenium no SDK .NET 9 com C# Console Application, que abre o Google Chrome na conta prévia logada pelo usuário, acessa a página de likes em comentários do Youtube e deleta manualmente um por um, já que essa função ainda não existe.

## Rodando o código pelo Visual Studio Code após instalação do .NET 9 SDK

1. Na linha 11 do arquivo *Program.cs* altere a variável originalProfilePath com seu USUARIO e a pasta do Windows onde os dados do perfil se encontram após ter se logado préviamente no Google Chrome pelo menos uma vez e fechado o browser na sequência. Pode estar na pasta Default, Profle 1 ou Profile 2, etc. Se tiver dúvidas, acesse o link chrome://version pelo próprio navegador para identificar e mude o final do path.
Na linha 13 do arquivo *Program.cs* também altere a localização onde clonou o projeto na sua máquina.

```csharp
static string originalProfilePath = @"C:\Users\USUARIO\AppData\Local\Google\Chrome\User Data\Profile 2";

static string clonedProfilePath = @"C:\GitHub\SeleniumBotNet9\ChromeUserData";
```
2. Na linha 30 do arquivo *Program.cs* valide a localização da instalação do seu Google Chrome no Windows.

```csharp
options.BinaryLocation = @"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe";
```

3. Na pasta SeleniumBot navegue via comando "cd" até o arquivo *Program.cs* e execute o comando abaixo,

```console
dotnet run
```

4. Na primeiro execução, entre em sua conta no Browser que foi aberto e faça o login na sua conta Google normalmente, após isso irá gerar a pasta ChromeUserData dentro do próprio projeto, é um clone do seu perfil, feito isso, pode fechar o Browser.

5. Agora, abra o Chrome com este perfil para fazer login manualmente via CMD:

```console
cd C:\Users\USUARIO\AppData\Local\Google\Chrome\
```

```console
chrome.exe --user-data-dir=\"{clonedProfilePath}\" --profile-directory=Default
```

6. Execute novamente o projeto, aguarde um pouco até a ida para a url https://myactivity.google.com/page?hl=pt_BR&page=youtube_comment_likes&pli=1 e a exclusão de comentários deve iniciar. Em caso de falha, rode o programa novamente e deixa o browser e VS Code abertos sem interação para que não interrompa o processo.

```console
dotnet run
```
