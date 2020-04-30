 # HOMY Property Management

  **ASP .NET Core 3.1 Web Application Project** 

 ## C# Web Development Path at Software University, Bulgaria
------------

 **ABOUT my web project:**

------------
- *Homy Real-Estate Property Management (RPM) is an online platform for renting and managing homes. Owners can list their properties for long-term rental and/or find long-term management solutions.*

- *The platform meets owners, potential tenants and potential property managers. Quite often, the owners don't live close to all their properties. They could even live in another country. Managing their properties scattered everywhere, is a challenge.*

------------
**Access to the website:**
**[HOMY at AZURE](https://homy.azurewebsites.net)**

------------
[![Homy Property Management](https://raw.githubusercontent.com/bodyquest/SoftwareUniversity-Bulgaria/master/ImgRepo/homy-multy-device.png "Multi-device mock-up")](https://homy.azurewebsites.net)
------------
------------

## **Database**
[Microsoft SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) along with [Entity Framework Core](https://dotnet.microsoft.com/download) were used to create and store the values. 
The database schema consists of the following main entities:

* Users
* Homes
* Cities
* Countries
* Rentals
* Contracts
* CloudImages
* TransactionRequests
* Payments
* StripeSessions
* ###### Hangfire Database Schema inside the main production DB

See the Schema here: **[DatabaseSchema](https://raw.githubusercontent.com/bodyquest/SoftwareUniversity-Bulgaria/master/ImgRepo/Homy-Database-Diagram.png)**

## **Backend**
The web project contains:
* 3 different areas: Identity, Administration, Management
* 85+ service methods
* 29 controllers
* 35+ views

## **Features**

This web platform allows a guest to the website to **view** and **find** listings by city, or explore by category or listing status.

A guest can also **contact** the support of the website.
In order to **send requests** to the owners of the listings, a guest must be registered and signed in.

Signed in user has three main choices:
* To List a home, thus becomming a role of **owner**
* To request for renting a home, becomming a **tenant** if the owner approves the request
* To request for managing a home, becomming a **manager** if the owner approves the request

An **Owner** has special admin dashboard.
The owner can:
* Overview his/her Listings, Requests, Payment list, Recurring transaction list, List of rented properties
* Add new Listing
* Approve/Reject requests for rent/management/cancel rent/ cancel management
* Remove Listing (only if it has no manager and tenant)
* Create transaction requests on recurring basis - for payments by the owner or to the owner
* Send payment to the manager

A **Manager** has special admin dashboard as well.
The manager can:
* Overview his/her managed properties and list of due payments by the owner

There is also an **Admin** of the site which currently has some basic privilleges from the dashboard. Most important of them are:
* Add Country / City to the list of Countries/Cities supported by the platform
* Add users to specific role (owner, manager etc.)
* Overview all listings
* Create new Listing to specific user

## **Technologies Used**

This website is designed and runs using the **main** technologies below:

   1) **[C#](https://en.wikipedia.org/wiki/C_Sharp_(programming_language))**
   2) **[ASP.NET Core 3.1](https://en.wikipedia.org/wiki/ASP.NET_Core)**
   3) **[Entity Framework Core 3.1](https://en.wikipedia.org/wiki/Entity_Framework?wprov=srpw1_0)**
   4) **[MS SQL Server](https://en.wikipedia.org/wiki/Microsoft_SQL_Server)**
   5) **[Bootstrap 4](https://getbootstrap.com/docs/4.0/getting-started/introduction/)**
   6) **[JavaScript](https://en.wikipedia.org/wiki/JavaScript)**
   7) **[HTML5](https://en.wikipedia.org/wiki/HTML)**
   8) **[CSS](https://www.w3schools.com/css/css_intro.asp)**
   9) **[MS Visual Studio 2019](https://code.visualstudio.com/)**
   10) **[MS SQL Server Management Studio 2017](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver15)**
   11) **[Microsoft Azure](https://azure.microsoft.com/en-us/)**
   12) **[Theme - Locals by Colorlib](https://colorlib.com/wp/template/locals-directory/)**
   13) **[Theme - Admin LTE 3.0.2 by Colorlib](https://github.com/ColorlibHQ/AdminLTE)**
   14) **[Hangfire API](https://api.hangfire.io/html/R_Project_Hangfire_Api.htm)**
   15) **[Stripe API](https://stripe.com/docs/api)**
   16) **[Google reCAPTCHA v3 API](https://developers.google.com/recaptcha/docs/v3)**
   17) **[Cloudinary API](https://cloudinary.com/documentation/image_upload_api_reference)**
   18) **[Sendgrid API](https://sendgrid.com/docs/API_Reference/api_v3.html)**


------------
------------

#### This website has been created solely for educational purposes.

------------
