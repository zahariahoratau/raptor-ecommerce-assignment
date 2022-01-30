# Ecommerce solution for order processing 🚚

### About
This repository contains code to solve and test a proposed solution for the Ecommerce solution for **Raptor Services**.
The main focus of the app is that whenever an order is sent through a message queue, the backend system would apply a discount specific to the customer that places the order, and then persist that order.

## Technologies and stack
#### This solution was accomplished using
- **Visual Studio** for fast development
- **.NET Core 6 and C# 9** as the framework and programming language
- **RabbitMQ** as the message broker that sends payloads to the backend
- **XUnit** as the testing library

## To run the solution

#### 1. Run RabbitMQ either as client or simply with docker:
```shell
docker run -it --rm --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3.9-management
```

#### 2. Run the .NET solution from within "/EcommerceCompany" folder
```shell
dotnet run
```

## To send an order to the system

On the RabbitMQ exchanges page use "Order.Exchange" for publishing messages. When publishing, the routing key should be of the form **"Order.Queue.\*"**, and the payload should be a JSON containing the **"(double) Amount, (string) Description, (int) CustomerId"** fields. E.g.:
```shell
{
   "Amount": 200,
   "Description": "My first order",
   "CustomerId": 1
}
```
The backend console will then log information about the received order and whether it could or could have not been placed. Here is an example of the console response when posting the payload above:

<img src="https://i.postimg.cc/8zjF2fv1/Screenshot-2022-01-30-165820.jpg">

#### *Note:* The system uses in-mem persistence and preloads the Customers array with a Silver type customer and the Orders array with two orders linked to that Customer. The details are such that any extra order placed for that customer will turn him into a Gold type. The persistence arrays can be found and modified within "Infrastructure/Repositories".

<img src="https://i.postimg.cc/4yXCctGV/Screenshot-2022-01-30-171039.jpg" alt="unit testing" width="600"/>
<img src="https://i.postimg.cc/HWzRKrHv/Screenshot-2022-01-30-171148.jpg" alt="unit testing" width="600"/>

## To test

### Unit Tests
- From within **/EcommerceCompanyTests** use:
```shell
dotnet test
```
