## Summary of CRUD Endpoints

| Action           | HTTP Method | Endpoint                   | Description                   |
|------------------|-------------|----------------------------|-------------------------------|
| Get All          | GET         | `/api/books` or `/api/movies` | Retrieve all books/movies.   |
| Get by Title     | GET         | `/api/books/{title}` or `/api/movies/{title}` | Retrieve a specific book/movie by title. |
| Add New Entry    | POST        | `/api/books` or `/api/movies` | Add a new book/movie.        |
| Edit Existing    | PUT         | `/api/books/{title}` or `/api/movies/{title}` | Update a specific book/movie. |
| Delete by Title  | DELETE      | `/api/books/{title}` or `/api/movies/{title}` | Delete a specific book/movie. |