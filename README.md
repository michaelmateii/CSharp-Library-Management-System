# Library Management System

A library management application I built in C# using WinUI 3 as part of my Computer Science studies.

The project was created to practice object-oriented programming, application logic and GUI development.

## Features

- Add and manage books
- Register library customers
- Loan and return books
- Calculate overdue fees
- Reserve unavailable books
- Reservation queue
- Search books by title or author
- Input validation
- WinUI 3 graphical interface

## Project Structure

The application is mainly divided into three classes:

- `Book` – stores information about each book, its loan status and reservations
- `Customer` – stores customer information and borrowed books
- `LibraryLogic` – handles loans, returns, reservations, searching and the main application logic

## Technologies

- C#
- .NET 8
- WinUI 3
- XAML
- Visual Studio

## Limitations

The application currently stores its data in memory, meaning books and customers are reset when the application closes. A future improvement would be adding persistent storage with a database.

## Screenshots

![Library Management System](../LibraryApp/docs/LibraryApp.png)

## UML Diagram

![UML Diagram](<../LibraryApp/docs/UMLdiagram.png>)