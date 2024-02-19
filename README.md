# MessageBoard

A mobile first web application for users to post on a general message board built with C# and Razor Pages, using a MVC structure and a MySQL database, using the EFCore framework.

By Henry Oberholtzer

![Mobile UI](https://github.com/henry-oberholtzer/MessageBoard/blob/main/ui_v1.png?raw=true)

## Technologies Used

- C#
- MySQL
- EFCore
- Razor Pages
- Microsoft Identity

## Features

# User Stories

- Should be able to log in
- Should be able to log out
- When logged in, a user should:
- have the ability to create posts
- have the ability to edit their own posts
- have the ability to delete their own posts
- have the ablity to create topics
- have the ability to assign a post to a topic
- have the ability to assign a topic to a post
- a post can be placed in several topics
- a splash page should list all topics and posts

## Upcoming Changes
- To be decided!

## Setup/Installation Requirements

- .NET 6 or greater is required for set up, and dotnet-ef to manage migrations.
- To establish locally, [download the repository](https://github.com/henry-oberholtzer/MessageBoard/archive/refs/heads/main.zip) to your computer.
- Open the folder with your terminal and run `dotnet restore` to gather necessary resources.
- In the production direction, `/MessageBoard` run `$ touch appsettings.json`
- In `appsettings.json`, enter the following, replacing `USERNAME` and `PASSWORD` to match the settings of your local MySQL server.
  
```
{
    "ConnectionStrings": {
        "DefaultConnection": "Server=localhost;Port=3306;database=MessageBoard;uid=USERNAME;pwd=PASSWORD;"
    }
}
```
- A local instance of MySQL (8.0.0 or greater) is required to be set up and running to use the project, for information on installing and configuring MySQL, [please see the official documentation.](https://dev.mysql.com/doc/mysql-installation-excerpt/8.3/en/)
- If you do not have `dotnet-ef` installed, first install it by running `dotnet tool install --global dotnet-ef --version 6.0.0`
- Run `dotnet ef database update` to create the database based on the provided database migrations.
- To start the projet, in the production directory, run the command `dotnet run` on your terminal.

## Known Bugs

- None at this time

## License

(c) 2024 [Henry Oberholtzer](https://www.henryoberholtzer.com/)

Original code licensed under the [GNU GPLv3](https://www.gnu.org/licenses/gpl-3.0.en.html#license), other code bases and libraries as stated.
