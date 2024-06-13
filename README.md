# How to use Duende Identity Server to Secure ASP.NET Core Web API

## Introduction
We can use [Duende IdentityServer](https://duendesoftware.com/products/identityserver) to only allow authenticated users access our valuable web API service. This documents briefly talk about code files, how SOLID principles are applied in design and the steps to run the demo.

 **Note:** _This repository is to demo how to do this. Since it is not a product, this demo simplifies the implementation to show the minimum what we need to do._

## Code Repository
The demo contains two solutions: "WexIdServer" and "WexSolution". All projects are on .NET Core 6.0.

### WexIdServer
This solution contains a single project "IdServer" which has a reference to "Duende.IdentityServer" nuget package(v.6.0.1). The project is an ASP.NET Core empty web application and implements an simple in-memory identity server.

#### ```Program.cs```
This file configures identity server middleware and adds it into pipeline.

#### ```TestUsers.cs```
This file contains a single user account for demo purpose.

#### ```Config.cs```
This file contains information used to configure identity server such as identity resources, scopes, API resources and client. In product, this information should be in configuration file.

### WexSolution
This solution contains a single project "WexAssessmentApi" which is an ASP.NET Core webapi project. This project implement a webapi to do CRUD (create, read, update and delete) operations on a dummy in-memory product repository.

#### ```Program.cs```
This file sets up service container for IProductRepository, adds authentication support into Swagger UI, configures authentication service and adds it into pipeline.

#### ```ProductsController.cs```
This file contains the implementation of all web api methods. All web api methods are executed **asynchronously** for data operations.

#### ```Repository.cs```, ```ProductRepository.cs``` and repository interfaces
These files implement a simple in-memory product repository. It supports **asynchronous** data operations.

#### ```Product.cs```
This file contains product DTO (data transfer object). It is used in both web api methods as DTO and repository as data object. Again, this simplification is only for demo purpose. In reality, ORM has its own data object; data model is only used in api layer.

## SOLID Object Oriented Design Principles

### Single Responsibility Principle
I take ```Product``` class as an example. This class only exposes properties on a product, such as Id, Name and Category etc., and serves a single task, i.e. transfer data cross among different parts of application.

### Open/Closed Principle
This principle shows in ```Repository<T>``` class and ```ProductRepository``` class. Category is a property on ```Product``` class. Generic type ```T``` has no knowledge about it. So we need to extend ```Repository<T>``` class to implement **GetProductsByCategoryAsync**, instead of changing ```Repository<T>``` directly.

### Liskov Substitution Principle
Most data operations are implemented in ```Repository<T>``` class, but ```ProductsController``` uses its child class ```ProductRepository```, instead of ```Repository<T>``` class. In this situation child class serves functionalities of parent class well.

### Interface Segregation Principle
The design breaks data operations for repository into two different smaller interfaces: ```IRepository<T>``` and ```IProductRepository```. The classes implementing them need to implement every methods.

### Dependency Inversion Principle
I add ```IProductRepository``` and its implementing class ```ProductRepository``` into service container. In this way, ```ProductsController``` does not have dependency on ```ProductRepository```.

## How to Run the Demo?

I have uploaded my code into [My github repository](https://github.com/pureperfectpuzzle/wex).

**Note:** _when you run projects, please use "**run as administrator**" to open Visual Studio 2022 and web browser. Otherwise, browser might report invalid certificate error and cannot continue._

### Clone Code Base to Local Repository
- Create a folder on your machine.
- Use git cli to open the folder. 
- Then run ```git clone https://github.com/pureperfectpuzzle/wex.git```.

### Open Both Solutions in Visual Studio 2022
Use Visual Studio 2022 to open the two solutions and build both of them.

### Obtain JWT Token Using curl
- Run ```IdServer``` project in the solution ```WexIdServer```.
- Use curl to retrieve an access token. Or if you prefer GUI, you can also use Postman/ARC: ```curl "https://localhost:7220/connect/token" -X POST -d "client_id=api.client&scope=wexassessmentapi_scope&client_secret=5C35BA65-4E20-43B2-90D4-9E2EA7A56DE1&grant_type=client_credentials" -H "Content-Type: application/x-www-form-urlencoded" -H "Cache-Control: no-cache"```.
- Copy the token from result for later use which is in the "access_token" json field.
- Don't close the IdServer since it will be used by web API server too.

### Test Web API Methods
- Run ```WexAssessmentApi``` project in the solution ```WexSolution```.
- First try any web api method and verify it returns 401 error.
- Click "Authorize" button on top right corner and paste the token just copied into the form. Then click "Authorize" button. Verify it is successful before clicking "Close" button.
- Now you can play with all web api methods to do CRUD operations. You can verify actions in different situations, such as success, failure because of data model validation, and failure because of parameter is not provided, etc.
- Click on "Authorize" button again and "Logout" button to close the current session. Then verify user cannot call any web api method again.