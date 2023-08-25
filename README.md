# Title: Movie Mania
**Movie Mania Project contains 3 sub projects:**
- API
- Frontend App
- Unit Testing Project

This project is created in **_.net 7_** 
To run project first download all three projects.
Open solution file from the API Project.
To setup database follow the below steps:
- Create **_MovieDB_** database.
- Change Data Source, User Id and Password to your SQL Server from **_Program.cs_** file of API Project.
- Use code first migration to create **_Identity Tables_** and **_Movie_** table.
- To create code first migration follow **_Migrations_** v1 to v5 from migration folder of API Project.
- After database setup create one admin from API using **_register-admin_** method.
- Admin can Create, Update and Delete Movie details.
- Register user from the registration form in Application.

### Normal User can see the details of movie while Admin can modify the movie details.
### Guest User can only see the posters of the movie.