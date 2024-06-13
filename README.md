<a name="readme-top"></a>

<!-- PROJECT LOGO -->
<div align="center">
  <a href="https://github.com/stayintarkov/StayInTarkov.Client">
    <img src="wwwroot/sit-logo-5.png" alt="Logo" height="240">
  </a>

  <h3 align="center">SIT.Controller</h3>

  <p align="center">
    Web server application designed for managing game servers remotely
    <br />
    <a href="https://github.com/mihaicm93/SIT.Controller/releases"><strong>Download Latest Release »</strong></a>
  </p>
</div>

<!-- TABLE OF CONTENTS -->
<details>
  <summary>Table of Contents</summary>
  <ol>
    <li>
      <a href="#about-the-project">About The Project</a>
    </li>
    <li>
      <a href="#features">Features</a>
    </li>
    <li>
      <a href="#technologies-used">Technologies Used</a>
    </li>
    <li>
      <a href="#getting-started">Getting Started</a>
    </li>
    <li>
      <a href="#license-summary">License Summary</a>
    </li>
    <li>
      <a href="#contact">Contact</a>
    </li>
  </ol>
</details>


## About The Project

This web server application is designed to simplify the management of game servers, specifically targeting scenarios where users want to manage their game server remotely without the need for constant remote desktop connections to their VPS. It provides a convenient web-based interface accessible from both desktop computers and mobile devices.


## Features

- **User Profile Management**: Players on the server can create and manage their profiles directly from the browser. This includes uploading and downloading their user profiles without the need for remote desktop access.

- **Game Server Control**: The application allows users to start and stop the game server directly from the web interface. This eliminates the need for manual intervention on the server side.

- **Feature Control**: Users can enable or disable various features on the game server through the web interface. This flexibility allows for easy customization based on user preferences.

- **User Registration Control**: Administrators have the option to disable the registration page, limiting access to the web server app to existing users only. This adds an extra layer of security if needed.

- **Game Server Console**: The web application provides a view of the game server console directly in the browser window. This feature is accessible from both desktop PCs and mobile devices, making server monitoring and management more accessible.


## Technologies Used

- **SQLite Database**: The application utilizes SQLite as its database engine, providing a lightweight and efficient storage solution for user profiles and other data.

- **Secure Password Storage**: User passwords are securely hashed using Microsoft Identity, ensuring the security of user accounts and preventing unauthorized access.

- **Blazor Server**: The application is built using Blazor Server, a web framework for building interactive web UIs using C# instead of JavaScript. This allows for seamless integration with .NET backend logic.


## Getting Started

To get started with the application, follow these steps:

1. **Download the Latest Release**: Download the latest release from the [Releases](https://github.com/mihaicm93/SIT.Controller/releases) section on the right side of the repository page.

2. **Transfer the Zip File**: Transfer the downloaded .zip file to your VPS or the machine where you are running the SIT.Server.

3. **Modify the Config File**: If needed, modify the `config.json` file. It should look like this:

   ~~~json
   {
     "urls": "http://*:5000;https://*:7000",
     "ServerPath": "E:\\SIT\\Server"
   }
   ~~~

   You can change the port numbers and the Server Path as necessary. Note that the Server Path is required.
   Http works fine, but if you don't have a SSL Cert installed it will crash. You'll need to install one or remove `https://* :7000` from the config

5. **Run the SIT.Controller.exe**: Execute the `SIT.Controller.exe` file. On the machine where your game server (SIT.Server) is running, open a browser and enter `localhost:5000` or `localhost:7000` in the address bar to test if the application works.

   - You will be redirected to the login/register page. This behavior is intended for the application running on localhost.
   - You can create an account from there if needed and test if the game server starts.
   - Ensure that the ports (5000 or 7000) or any other chosen ports are open and accessible from a different machine than the one running the SIT.Server. You should be able to connect to the web app using `VPS-IP:5000` or `VPS-IP:7000`.


## License Summary

The material in this project is licensed under the [Creative Commons Attribution-NonCommercial-NoDerivatives 4.0 International Public License](https://creativecommons.org/licenses/by-nc-nd/4.0/legalcode). Here's a summary of its restrictions:

- **NonCommercial Use Only**: The material can only be used for non-commercial purposes. Commercial use is prohibited.

- **No Derivative Works**: You cannot modify or adapt the material in any way.

- **No Redistribution of Modified Material**: Distribution of modified versions of the material is not allowed.

- **No Endorsement**: Use of the material does not imply endorsement by the licensor.

- **No Warranty/Liability**: The licensor provides the material "as-is" without any warranties and is not liable for any damages.

- **License Term and Termination**: The license applies for the duration of the copyright and similar rights. Failure to comply with the terms results in automatic termination.

- **Additional Terms**: The licensor reserves the right to offer the material under separate terms or stop distributing it at any time.

- **Survival of Certain Clauses**: Certain clauses of the license survive even after termination.

Please refer to the [full license text](https://creativecommons.org/licenses/by-nc-nd/4.0/legalcode) for detailed information and legal terms.


## Contact

For inquiries, please contact Mihai at cristian@mihaimuresan.com or Discord mihai_cristian
