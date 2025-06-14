# SeleniumBot 2025: Delete YouTube Likes on Comments via My Google Activity – .NET 9 C# on Windows with VS Code and Google Chrome

This project contains a bot built using the Selenium package on the .NET 9 SDK with a C# Console Application. It opens Google Chrome using a previously logged-in user account, navigates to the YouTube comment likes section on the My Google Activity page, and manually deletes each like, since Google does not currently offer a built-in feature for this action.

## Requirements

- .NET SDK 9  
  [Download .NET 9](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)

- IDE: Visual Studio Code  
  [Download VSCode](https://code.visualstudio.com/download)

## Running the Bot in Visual Studio Code (after installing the .NET 9 SDK)

1. Build the project to install Selenium and the Chrome WebDriver dependecies via terminal, this is going to generate bin and obj folders:

   ```bash
   dotnet build SeleniumBot.csproj
   ```

2. In line 7 of `Program.cs`, update the `originalProfilePath` variable with your actual Windows username and Chrome profile path. Make sure you’ve logged into Chrome at least once and then closed the browser before running the script. Your Chrome profile might be in folders like `Default`, `Profile 1`, `Profile 2`, etc.  
   To confirm your profile location, open `chrome://version` in your browser and look for the **Profile Path**. Update the path accordingly.  
   Also, in line 8, update the `clonedProfilePath` to match your local project directory.

   Example:

   ```csharp
   static string originalProfilePath = @"C:\Users\USERNAME\AppData\Local\Google\Chrome\User Data\Profile 2";
   static string clonedProfilePath = @"C:\GitHub\SeleniumBotNet9\ChromeUserData";
   ```

3. On line 37 of `Program.cs`, confirm the path to your Chrome installation:

   ```csharp
   options.BinaryLocation = @"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe";
   ```

4. Open a terminal and navigate via `cd` to the folder containing `Program.cs`, then run the bot:

   ```bash
   dotnet run .\Program.cs
   ```

5. On the first run, the app will clone your Chrome profile into the `ChromeUserData` folder. After that, follow the instructions shown in the Console output or continue to the next step.

6. Open a terminal (CMD/PowerShell) and navigate to your Chrome executable folder. Then run the following command to manually launch Chrome with the cloned profile. Log in to your Google account if needed, then close Chrome.

   ```bash
   cd "C:\Program Files (x86)\Google\Chrome\Application"
   ```

   ```bash
   chrome.exe --user-data-dir="C:\GitHub\SeleniumBotNet9\ChromeUserData" --profile-directory=Default
   ```

   Replace the path above with your actual `clonedProfilePath`.

7. Run the project again. It will open the URL:

   [https://myactivity.google.com/page?hl=pt_BR&page=youtube_comment_likes](https://myactivity.google.com/page?hl=pt_BR&page=youtube_comment_likes)

   and start deleting the liked comments.

   If the script fails, just run it again. Keep both Chrome and VS Code open, and avoid interacting with the browser while the bot is running to prevent interruptions.

   If the Google page takes more than 90 seconds to load the likes, clear your browser history or use Microsoft's **PC Manager** tool to clean system/cache files. You may also try opening the URL manually before running the script to ensure the liked comments load correctly.

   ```bash
   dotnet run .\Program.cs
   ```
