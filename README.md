# TopGear_WebApi_DependencyInjection2
Web API project to demonstrate Dependency Injection and other Web API features.

**Note:** MyWebPage is an incomplete MVC project. At one point it served as a front-end client to the Web API. Later it was dropped in favor of simply using Postman's HTTP requests. I decided to include it here, as I might finish it later.

## Setup

***Important!*** After downloading this project, follow these steps to perform necessary configurations.

***STEP 1: Create a database with 2 tables, Posts and Users.***
1. Download the SQL scripts file named "SQLScripts_SetupWebApiDB.sql".
2. Open SQL Server.
3. Execute the scripts file.

***STEP 2: Configure the connection string to the database.***
1. Open the file WebApi/Web.config
2. Find the connection string in line 13. Replace the value of "data source" with your SQL Server's name.

***STEP 3: The email service uses localhost's SMTP email server to save emails locally, rather than send across the web.***

By default, these emails will be saved to "C:/Mails".
1. If this behavior is acceptable, make sure this file location exists.
2. If you would rather save the files elsewhere, open Web.Config, locate the code section below, and change the value of "HERE":
```
<configuration>
...
    <system.net>
        <mailSettings>
            <smtp deliveryMethod=SpecifiedPickupDirectory>
                <specifiedPickupDirectory pickupDirectoryLocation="HERE" />
            </smtp>
        </mailSettings>
    </system.net>
...
</configuration>
```

## Using the Web API

### Postman

Postman can be used to send HTTP requests to the Web API. In the json file "Postman", you can find an exported collection of all requests discussed later in this introduction. You can easily use them by importing the collection into Postman.

You will need to adjust the localhost port number to your Web API. Additionally, you'll need to set Authorization to Bearer Token and provide the token to access any requests for blog posts.

## Overview of the Web API's main features

The web API has 3 primary features.
1. Functions as a very simple blog where clients can create, view, update, and delete posts.
2. Requries authentication to perform any requests with blog posts.
3. Includes an email service which simulates sending emails to users.

### Data storage and access

All Posts are stored in a local SQL Server database. Some User data is also stored here, though account information is managed by OAuth.
The data models of Posts and Users are maintained by an ADO.NET Entity Data Model to provide simple CRUD operations.

All post operations require authentication. (Refer to Authentication section below.)

Clients can send the following HTTP requests to perform CRUD operations on posts:
* Get all posts: GET http://localhost:{port}/api/posts
* Get post by id: GET http://localhost:{port}/api/posts/{id}
* Create a new post: POST http://localhost:{port}/api/posts
    * Body must contain UserId, PostName, and PostBody.
* Update a post: POST http://localhost:{port}/api/posts/{id}
    * Body must contain Id equal to {id}, and may contain PostName and/or PostBody. Other fields will not be updated.
* Delete a post: DELETE http://localhost:{port}/api/posts/{id}

### Authentication

Authentication is implemented with OAuth2's token authentication. Before a client can access blog posts, they must register an account, request an authentication token, and provide that token in each HTTP request.

The following HTTP requests will be useful:
* Register an account: POST http://localhost:{port}/api/account/register
    * Body must contain Email, FirstName, LastName, Password, and ConfirmPassword.
    * Upon successful registration, the API will store the user's email and name in the local database, and will send a welcome email via the email service.
* Request an authentication token: GET http://localhost:{port}/token
    * Content-Type header must be application/x-www-form-urlencoded.
    * Body must contain grant_type = password, username, and password.
    * Upon a successful token request, the API will send a verification email via the email service.

### Email Service

As mentioned above, the email service uses localhost's SMTP email server to save emails locally, rather than send across the web. Refer to Setup step 3 for configuration options.

There are three ways the API sends emails.
* You can explicitly compose and send an email: POST http://localhost:{port}/email/send
    * Body must contain To (email address), Subject, and Message
* After a user successfully registers, an email is created to welcome them.
* After a user successfully requests a token, an email verifies their login.

***Dependency Injection:*** Inversion of Control is achieved with Unity by injecting the email service wherever it is needed. Specifically, it is injected in the following places:
* EmailController for the first option above.
* AccountController for the second option above.
* ApplicationOAuthProvider and Startup for the third option above.
