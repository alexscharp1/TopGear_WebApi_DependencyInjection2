# TopGear_WebApi_DependencyInjection2
Web API project to demonstrate Dependency Injection and other Web API features.

***IMPORTANT!*** Refer to "Setup" section below for important setup instructions.

Features: The web API has 3 primary features.
1. Functions as a very simple blog where clients can perform CRUD operations on blog posts.
2. Requries authentication to perform any requests with blog posts.
3. Includes an email service which simulates sending emails to users.

### Setup

After downloading this project, some adjustments will need to be made before it will run locally.

STEP 1: Create a database with 2 tables, Posts and Users.
1. Download the SQL scripts file named "SQLScripts_SetupWebApiDB.sql".
2. Open SQL Server.
3. Execute the scripts file.

STEP 2: Configure the connection string to the database.
1. Open the file WebApi/Web.config
2. Find the connection string in line 13. Replace the value of "data source" with your SQL Server's name.

STEP 3: The email service is configured to run on localhost's SMTP email server. Rather than sending emails across the web, it instead saves them locally.
By default, these emails will be saved to "C:/Mails".
1. Option 1: Make sure this file location exists, or create it if it doesn't.
2. Option 2: Alternatively, you can change the location the files are saved. Open Web.Config, location the code section below, and change the value of "HERE":
<configuration>
    <system.net>
        <mailSettings>
            <smtp deliveryMethod=SpecifiedPickupDirectory>
                <specifiedPickupDirectory pickupDirectoryLocation="**HERE**" />
            </smtp>
        </mailSettings>
    </system.net>
</configuration>

### Postman

Postman can be used to send HTTP requests to the Web API. Some sample requests are provided in the Postman folder. 
These samples include sending an email, registering an account, requesting an authentication token, and performing CRUD operations on blog posts.
