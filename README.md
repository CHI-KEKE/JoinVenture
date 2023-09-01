# JoinVenture

## Project Description

A web application for event hosting and ticket booking built using HTML, CSS, JavaScript, ASP.NET Core, and Entity Framework Core.

## Demo

### Event List

https://cofstyle.shop/list/Activity-List.html

### Click View to check the Detail

ex: https://cofstyle.shop/detail/Activity-Detail.html?id=2

...more website service info will be updated soon on README

## Prerequisites

Before you begin, ensure you have met the following requirements:

- .NET 7.0.302
- EntityFrameWork Core 7.0.10
- SQL Server 7.0.10

## Installation

1. Clone the repository.
2. Install back-end dependencies: `dotnet restore`
3. Run database migrations: `dotnet ef database update`
4. Start the app: `dotnet run`

## Usage

Will soon release for Usage...

## Configuration

To configure the app, create a `.env` file and add the following variables:

```env
TokenKey="YourTokenKey"
LINEChannelId = "YourLineChannelID"
LINEChannelSecretKey = "YourLineScretKey"
DB_CONNECTIONSTRING = "Database Connection String"
...
```
