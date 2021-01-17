# 

# Technical Assessment by Felipe Drumond
* Github profile: https://github.com/felipedrumond
* LinkedIn profile: https://www.linkedin.com/in/felipedrumond/

## Intro
Thank you for giving me the opportunity to do the assessment.
This solution it named "My.Api" to make it harder to find as it is a public repository.

My main focus with this assessment is to:
* pass the Exercises 1, 2, 3 and the **Extra for Experts** with **my own trolley calculation implementation**
* ensure code is clean and readable, with well named namespaces, classes, methods and variables
* ensure separation of concerns where possible (given time constraints)
* use best practices where possible (given time constraints)

and also:

* use **dependency injection** of any dependencies / **mock their behaviour in unit tests**
* unit tests covering >85% of the code (due to time constraints)
* write exceptions, handle them and write unit tests that cover such scenarios
* due to the simplicity of the project and time constraints, no third-part DI Framework will be used
* prepare the solution to broader use of log (Serilog)
* logs:

```cs
[Api.Startup]:  2021-01-17 10:51:54.063 +11:00 [INF] Starting up My.Api.
                2021-01-17 10:51:55.798 +11:00 [INF] Application started.

[Api.Request]:  2021-01-17 11:10:35.267 +11:00 [INF] **Request starting HTTP/1.1 GET http://localhost:5001/api/answers/user**
                2021-01-17 11:10:35.325 +11:00 [INF] Executing endpoint 'My.Api.Controllers.AnswersController.GetUser (My.Api)'
                2021-01-17 11:10:35.355 +11:00 [INF] Route matched with {action = "GetUser", controller = "Answers"}. Executing controller action with signature Microsoft.AspNetCore.Mvc.IActionResult GetUser() on controller My.Api.Controllers.AnswersController (My.Api).
                2021-01-17 11:10:35.385 +11:00 [INF] **Executing action** method My.Api.Controllers.AnswersController.GetUser (My.Api) - Validation state: "Valid"

[Api.Response]: 2021-01-17 11:10:35.391 +11:00 [INF] **Executed action** method My.Api.Controllers.AnswersController.GetUser (My.Api), returned result Microsoft.AspNetCore.Mvc.OkObjectResult in 0.414ms.
                2021-01-17 11:10:35.395 +11:00 [INF] **Executing ObjectResult**, writing value of type 'My.Api.Models.Users.User'.
                2021-01-17 11:10:35.416 +11:00 [INF] Executed action My.Api.Controllers.AnswersController.GetUser (My.Api) in 56.0568ms
                2021-01-17 11:10:35.417 +11:00 [INF] Executed endpoint 'My.Api.Controllers.AnswersController.GetUser (My.Api)'
                2021-01-17 11:10:35.419 +11:00 [INF] **Request finished** in 166.5124ms 200 application/json; charset=utf-8
                
[Errors]:       (injected a required dependency equals to null)
                2021-01-17 10:52:18.219 +11:00 [ERR] An unhandled exception has occurred while executing the request.
                **System.ArgumentNullException: Value cannot be null. (Parameter 'onlineStoreService')**
                at My.Api.Controllers.AnswersController..ctor(IOnlineStoreService onlineStoreService, ILogger`1 logger, IConfiguration config) in C:\Work\My.Api\My.Api\Controllers\AnswersController.cs:line 26 (...)

                (invalid request)
                2021-01-17 10:59:07.070 +11:00 [INF] Request starting HTTP/1.1 GET http://localhost:5001/api/answers/sort?sortOption=invalid_sort_option
                (...)
                2021-01-17 10:59:07.273 +11:00 [ERR] **Invalid sorting option.**
                2021-01-17 10:59:07.273 +11:00 [INF] Executed action method My.Api.Controllers.AnswersController.Sort (My.Api), returned result Microsoft.AspNetCore.Mvc.BadRequestObjectResult in 140.3506ms.
                2021-01-17 10:59:07.274 +11:00 [INF] Executing ObjectResult, writing value of type 'System.String'.

                (invalid request)
                2021-01-17 11:06:14.357 +11:00 [INF] Request starting HTTP/1.1 POST http://localhost:5001/api/answers/trolleyTotal application/json 917
                (...)
                2021-01-17 11:06:14.500 +11:00 [ERR] **trolley failed to calculate its total.**
                2021-01-17 11:06:14.500 +11:00 [INF] Executed action method My.Api.Controllers.AnswersController.GetTrolleyTotal (My.Api), returned result Microsoft.AspNetCore.Mvc.BadRequestObjectResult in 98.8312ms.
                2021-01-17 11:06:14.500 +11:00 [INF] Executing ObjectResult, writing value of type 'System.String'.
```

* ease the creation of new unit tests by providing methods to create FakeObjects, enabling developers to test more while writing less code

```cs
protected Product FakeProduct(string name, decimal price, decimal quantity)
{
    var product = new Product()
    {
        Name = name,
        Price = price,
        Quantity = quantity
    };

    return product;
}
```
use:

```cs
// arrange
var fakeProductsList = new List<Product>() {
    FakeProduct("A", 1, 1),
    FakeProduct("B", 2, 2),
    FakeProduct("C", 2, 2)
};

myServiceMock.Setup(r => r.GetProducts()).Returns(Task.FromResult(fakeProductsList));

// act
(...)

// assert
myServiceMock.Verify(r => r.GetProducts(), Times.Once);

```

### Solution Structure
The solution has the following projects:
* My.Api (Models, Controllers for providing the answers of the challenges) 
* My.Api.Tests (Unit Tests for My.Api)
* WXDevChallengeService.Api (Models and Services for fetching data from Woolies' api, aka "WX Dev Challenge Service")
* WXDevChallengeService.Api.Tests (Unit Tests for Resources.Api)

## How to run the solution
The project was written in .NET Core 3.1 and Visual Studio 2019.

To run the application, please ensure you have the .NET Core 3.1 SDK installed. You can download it from https://dotnet.microsoft.com/download/dotnet-core/3.1

## Run the Application from Visual Studio 2019
* Open the solution file ```MySolution.sln``` located in the root folder of this repo
* Restore Nuget Packages by rigth-clicking on the ```MySolution``` solution
* Make sure ```My.Api``` is the startup project by rigth-clicking on ```My.Api``` and selecting ```Set as Startup project```
* Press F5 to launch My.Api
* A browser will automatically open at http://localhost:5001/api/answers/user


## Testing the exercises results:

### Option 1: Via http://dev-wooliesx-recruitment.azurewebsites.net/exercise
* Provide the URL of My.Api deployed in Azure: https://technicalchallenge.azurewebsites.net/api/answers/


### Option 2: Running on local machine - please refer to [How to run the solution](#How-to-run-the-solution)
Exercise 1 - Basic response (token and name):
* http://localhost:5001/api/answers/user

Exercise 2 - This will test your sorting endpoint:
* http://localhost:5001/api/answers/sort?sortOption=Low
* http://localhost:5001/api/answers/sort?sortOption=High
* http://localhost:5001/api/answers/sort?sortOption=Ascending
* http://localhost:5001/api/answers/sort?sortOption=Descending
* http://localhost:5001/api/answers/sort?sortOption=Recommended
* http://localhost:5001/api/answers/sort will return the products with no sorting

Exercise 3 - Given products, specials and quantities, return the trolleys total:
* [POST] http://localhost:5001/api/answers/trolleyTotal - provides an answer for the Trolley Exercise **Extra for Experts** that uses ```My.Api.TrolleyCalculator``` or
* [POST] http://localhost:5001/api/answers/trolleyTotal_From_OnlineStoreService - provides the non-expert answer for Exercise 3 with the data fetched from Woolies' api, aka "WX Dev Challenge Service", via the ```WXDevChallengeService.Services.OnlineStoreService```
<br/>with body sample:

```jsonc 
{
    "products": [
        {
            "name": "1",
            "price": 2
        },
        {
            "name": "2",
            "price": 5
        }
    ],
    "specials": [
        {
            "quantities": [
                {
                    "name": "1",
                    "quantity": 3
                },
                {
                    "name": "2",
                    "quantity": 0
                }
            ],
            "total": 5
        },
        {
            "quantities": [
                {
                    "name": "1",
                    "quantity": 1
                },
                {
                    "name": "2",
                    "quantity": 2
                }
            ],
            "total": 10
        }
    ],
    "quantities": [
        {
            "name": "1",
            "quantity": 3
        },
        {
            "name": "2",
            "quantity": 2
        }
    ]
}
```
