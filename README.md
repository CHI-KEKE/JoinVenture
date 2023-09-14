# JoinVenture

## Project Description

Welcome to EventHub, the premier platform that transforms event planning and attendance into a seamless and interactive experience. Whether you're an event organizer looking to host a successful gathering or an attendee searching for the hottest tickets in town, EventHub has got you covered. With our cutting-edge technology and user-friendly interface, we're redefining the way you engage with events.

![Alt text](https://d1pjwdyi3jyxcs.cloudfront.net/JoinVenture/main.png)

## Demo

### Site

https://cofstyle.shop/list/Activity-List.html

### Test Account

- email : gina@gmail.com / allen@gmail.com / admin@gmail.com
- password : Pa$$w0rd

### Structure

![Alt text](https://d1pjwdyi3jyxcs.cloudfront.net/JoinVenture/server_structure.png)

### As a member We can...

- View the detail of each event
- Be a Follower of each event, and receive real-time notification if someone interact with the activity(ex. leave a comment)
- Register for ticket, and go through ticket package selecting as well as payment flow
- Check the Order in personal profile page
- Host an activity
- real-time comment
- real-time ticket updating

### As a Admin We can...

- Check Members Order Records

### WebSite Features

- WebSocket for realtime comment/ notification / ticket updating
- Cache for better user experience
- Apply Transaction on ticket picking to deal with race condition
- AWS Service (EC2 / S3 / RDS / Elastic Cache)
- k6 metrics on CloudWatch

### k6 testing on t2.micro for ticket picking API (check race condition)

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

## Configuration

To configure the app, create a `.env` file and add the following variables:

```env
TokenKey="YourTokenKey"
LINEChannelId = "YourLineChannelID"
LINEChannelSecretKey = "YourLineScretKey"
DB_CONNECTIONSTRING = "Database Connection String"
...
```
