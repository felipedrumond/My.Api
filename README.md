# 

# <!--- WooliesX ---> Technical Assessment by Felipe Drumond
* Github profile: https://github.com/felipedrumond
* LinkedIn profile: https://www.linkedin.com/in/felipedrumond/

## Intro
Thank you for giving me the opportunity to do the assessment.

My main focus with this assessment is to:
* pass the Exercises 1, 2, 3 and the **Extra for Experts** with my own trolley calculation implementation
* ensure code is clean and readable, with well named namespaces, classes, methods and variables
* ensure separation of concerns where possible (given time constraints)
* use best practices where possible (given time constraints)

and also:
* use dependency injection of any dependencies / mock their behaviour in unit tests
* demonstrate unit test coverage (>85% due to time constraints) of the WXDevChallengeService and My.Api
* ease the creation of new unit tests by providing methods to create FakeObjects, enabling developers to test more while writing less code
* prepare the solution to broader use of log (Serilog)
* write exceptions, handle them and write unit tests that cover such scenarios

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
var fakeProductA = FakeProduct("A", 1, 1);
var fakeProductB = FakeProduct("B", 2, 2);
var fakeProductC = FakeProduct("C", 2, 2);
```

### Solution Structure
The solution has the following projects:
* WXDevChallengeService.Api (Models and Services for fetching data from Woolies' api, aka "WX Dev Challenge Service")
* WXDevChallengeService.Api.Tests (Unit Tests for Resources.Api)
* My.Api (Models, Controllers for providing the answers of the challenges) 
* My.Api.Tests (Unit Tests for My.Api)

![Solution Structure](/documentation/images/solution-structure.png "Solution Structure")

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
