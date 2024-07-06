# English Journey

English Journey is an API developed with ASP.NET technology, which objective is to support English language learning through various functions based on memory techniques. The application allows users to create flashcards, connections and short notes, which helps to effectively acquire and consolidate knowledge. In addition, users can analyze their learning progress to better track the development of their language skills.

## Table of Contents

- [Description](#description)
- [Technologies](#technologies)
- [Further Development](#further-development)
- [Installation](#installation)
- [Usage](#usage)
- [Tests](#tests)

## Description

The application consists of several main functionalities that support the development of language learning in various ways. It is designed for effective knowledge absorption through the use of proven memory techniques. Thanks to a variety of tools, the user can learn new vocabulary, expressions and idioms, and consolidate the acquired knowledge in a systematic and effective way.

### Flashcards

The first of the functionalities is the fiches. The user first creates a category and with it 6 "boxes" are created.
The fiches are added to the first "box". In order for a fiche to go into the next box, the user must take a test. In order to move the same fiche to the next box again, enough time must pass to increase the efficiency of memorization. When the fiche goes to the 6th 'box' it means that the phrase / word has been memorized in a permanent way.

### Connections

Another feature for learning vocabulary words, expressions or idioms is "Connections". The user starts by setting a main topic, for example, the word "take." Then related words and expressions can be added to this topic, such as "take part," "take off," etc. This functionality is versatile and can be used in many different ways, helping you better understand and remember the relationships between different expressions.

### Notes

Another very useful technique for learning a foreign language is creating short notes. These can include information about grammar, popular phrases, or other important aspects of the language. The user has the option of archiving notes that he or she deems mastered, or deleting them altogether. Storing archived notes allows you to return to them at any time, which can be very helpful in tracking your learning progress and consolidating the knowledge you have gained.

## Technologies

- ASP.NET Core
- Entity Framework
- FluentValidation
- MediatR
- ASP Identity
- xUnit, Fluent Assertions and Moq to tests

## Further Development

1. In the next step, a frontend will be created for the application using the Angular framework.
2. Once the frontend is created, more techniques will be added to the application to facilitate learning the foreign language.
3. The next objective is to create and track statistics related to the use of the application, including which functionalities are most popular, which countries the users come from, and so on.

## Installation

1. `git clone https://github.com/MateuszGrzegorzewski/English-Journey`
2. `cd /path/to/repository`
3. Make sure you have the .NET SDK installed. You can download it from [the official .NET website](https://dotnet.microsoft.com/en-us/download).
4. `dotnet restore`
5. Before running the application, you need to create a database. Make sure that the connection to the database is properly configured in the file [appsettings.json](EnglishJourney.API/appsettings.json).
6. In Visual Studio: `Tools` --> `NuGet Package Manager` --> `Package Manager Console` --> Choose default project: EnglishJourney.Infrastructure --> In command line: `Update-Database`
7. `dotnet build`
8. `dotnet run`

## Usage

1. `dotnet build`
2. `dotnet run`

## Tests

An application was fully tested to ensure its reliability and performance. As part of the testing process, both unit and integration tests were created.

To run the tests should be:

1. `cd path/to/repository`
2. `dotnet test`
