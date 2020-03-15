## RWSDNS - RESTful Windows Server DNS

RWSDNS aims to provide a RESTful API for interacting with Windows Server DNS.

## Installation on IIS 
(Thanks https://github.com/unosquare/passcore for the template!)

1. Install IIS
1. Download the source code and run the following command via an Command Prompt. Make sure you start the Command Prompt with the Administrator option and change directory to where the project files is saved.
    - `dotnet publish --configuration Release --runtime win-x64 --output "<path>"`
    - The `<path>` is the directory where you will be serving the website from.
1. Install the [.NET Core 3.0.0 Windows Server Hosting bundle](https://dotnet.microsoft.com/download/thank-you/dotnet-runtime-3.0.0-windows-hosting-bundle-installer).
1. Go to your *IIS Manager*, Right-click on *Application Pools* and select *Add Application Pool*.
1. A dialog appears. Under Name enter **RWSDNS Application Pool**, leave **.NET CLR version** and **Managed Code** as the default. Click OK.
1. Now, right-click on the application pool you just created in the previous step and select *Advanced Settings ...*. Change the *Start Mode* to **AlwaysRunning**, and the *Idle Time-out (minutes)* to **0**. Click on *OK*. This will ensure the application stays responsive even after long periods of inactivity. *Identity* also has to be changed to an **account with permissions to manage DNS.**
1. Back on your *IIS Manager*, right-click on *Sites* and select *Add Website*
1. A dialog appears. Under *Site name*, enter **RWSDNS**. Under *Application pool* click on *Select* and ensure you select **RWSDNS Application Pool**. Under *Physical path*, click on the ellipsis *(...)*, navigate to the folder where you extracted RWSDNS.
    - **Important:** Make sure the Physical path points to the *parent* folder which is the one containing the files, *logs* and *wwwroot* folders.
1. Under the *Binding section* of the same dialog, configure the *Type* to be **https**, set *IP Address* to **All Unassigned**, the *Port* to **443** and the *Hostname* to something like **dnsapi.yourdomain.com**. Under *SSL Certificate* select a certificate that matches the Hostname you provided above. If you don't know how to install a certificate, please refer to [SSL Certificate Install on IIS 8](https://www.digicert.com/ssl-certificate-installation-microsoft-iis-8.htm) or [SSL Certificate Install on IIS 10](https://www.digicert.com/csr-creation-ssl-installation-iis-10.htm) , in order to install a proper certificate.
    - **Important:** Do not serve this website without an SSL certificate because requests and responses will be transmitted in cleartext and an attacker could easily retrieve these messages and collect the API Key.
1. Click *OK* and you should be set. *You should change the default API Key which is 'Password' via the /api/v1.0/auth/ApiKey endpoint right away.*

## Example use - A Record for the DNS zone 'test.se'
(http post for adding/updating, http delete for deleting all)
**note** - Check the /tests/ folder for up-to-date URL:s in the powershell scripts
![Alt text](res/ARecord.png?raw=true "Interactive example")

## Example use - CNAME Record for the DNS zone 'test.se'
(http post for adding/updating, http delete for deleting all)
![Alt text](res/CnameRecord.png?raw=true "Interactive example")