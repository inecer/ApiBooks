# ApiBooks

ApiBooks is a RESTful API built with ASP\.NET Core for managing a collection of books and their reservations\.

## Prerequisites

- [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)  
- [Docker](https://www.docker.com/get-started)

## Getting Started

### Running the API

1\. Clone the repository:
```bash
git clone git@github.com:inecer/ApiBooks.git
cd ApiBooks
```

2\. Build and run the Docker containers:
```bash
docker-compose up
```

3\. The API will be available at `http://localhost:5133` and `https://localhost:7275`\.

### Accessing Swagger

To access the Swagger UI for testing the API endpoints, simply run the project and navigate to:  
\- [http://localhost:5133/swagger](http://localhost:5133/swagger) for HTTP  
\- [https://localhost:7275/swagger](https://localhost:7275/swagger) for HTTPS

## API Endpoints

\- **Get all books**: `GET /api/Book/GetBooks`  
\- **Create a new book**: `POST /api/Book/CreateBook`  
\- **Update an existing book**: `PUT /api/Book/UpdateBook/{id}`  
\- **Delete a book**: `DELETE /api/Book/DeleteBook/{id}`

\- **Get all reservations**: `GET /api/Book/GetReservations`  
\- **Create a new reservation**: `POST /api/Book/CreateReservation`  
\- **Update an existing reservation**: `PUT /api/Book/UpdateReservation/{id}`  
\- **Delete a reservation**: `DELETE /api/Book/DeleteReservation/{id}`

## Configuration

The application settings can be configured in the following files:  
\- `appsettings.json`  
\- `appsettings.Development.json`

## Database

The connection string for the database is configured in `appsettings.json` under the `ConnectionStrings` section\.
