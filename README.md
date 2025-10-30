# LiquidLabsProducts

LiquidLabsProducts is a simple productlisting API built using **ASP.NET Core + MSSQL**.  
It’s designed to show how to connect a .NET API with a Mssql and ADO.NET using clean N-tier architecture.

## Tech Stack

- **Backend:** ASP.NET Core Web API (.NET 9)
- **Database:** Microsoft SQL Server
- **Architecture:** N-Tier (API, Business Logic, Database Access Test)
- **Containerzation:** Docker

---

## Project Overview

- Check Database and tables exist if not create on application build
- Check is there any record in cached if not  check database.
- If avaialable retrive record otherwise add retrived record to database and retrive from db.
- If the record not avaialablle in db or cached retrive from public api(https://dummyjson.com/) and save to db
- Store and fetch the data from Microsoft Sql Server using ADO.NET (No ORM).

---

## How It's built

1. **Backend (ASP.NET Core API)**
   - Created Seperate layers to manage each logics (WebAPI, ServiceLayer(Business Logics), DbLayer(Data Access))
   - APIs are developed in Api Controllers
   - Connected to MSSQL using ADO.NET (Raw scripts with parameterized queries)
  
2. **Integration**
   - .NET API and databse runs on same Docker network.
   - Configured `Dockerfile` for API project
   - Configured `docker-compose.yml` for one-line setup.

---

# How to run the project

There are 2 way of running this project `run locally` and `run docker container` (Recommended) if you have docker installed in your machine.

## 1. Local Run (using `dotnet run` or using visual studio)

This is a primary way to run and test the application locally.

**Steps**

1. Make sure you have installed Microsoft Sql Server and .NET 9 SDK
3. Clone the Project from Github the Repository LiquidLabsProducts => https://github.com/Rifnaz/LiquidLabsProducts.git
4. Redirect to prject folder CoverageXTodo `cd LiquidLabsProducts` Open the project in visual studio.
5. Update the connection string in `appsettings.json` according to your mssql setup.
6. **!Important: Keep the Database as `LiquidLabsProducts`**
7. Build and run the project (dotnet run or using Visual studio run).
8. The database `LiquidLabsProducts` and tables will be created automatically if it doesn’t existwhen application run.
9. Test API endpoints using Postman(recommended) or your preferred tool. i. e (http://localhost:5000/api/product, http://localhost:5000/api/product/1)

Example `appsetting.json`
``` bash
"ConnectionStrings": {
  "DefaultConnection": "Server=Rifnaz;Database=LiquidLabsProducts;User Id=admin;Password=admin1234;Trusted_Connection=true;TrustServerCertificate=true;"
}
```

---

## 2. Run the Project with a Single Command (Recommended and Optional)

This is my recommended way if you have docker installed in your machine. Make sure you have **Docker Desktop** installed with **WSL(Windows Sub System for Linux)** then open your terminal inside the project folder and run:

```bash
docker compose up -d --build
```
**Steps**

1. Clone the Project from Github the Repository LiquidLabsProducts => https://github.com/Rifnaz/LiquidLabsProducts.git
2. Open Shell or Terminal and Redirect to prject folder CoverageXTodo `cd LiquidLabsProducts`
3. Run the command `docker compose up -d --build` and wait for download all dependencies and start both images (API and Sql Server)
4. The database `LiquidLabsProducts` and tables will be created automatically if it doesn’t existwhen application run.
5. Test API endpoints using Postman(recommended) or your preferred tool. i. e (http://localhost:5000/api/product, http://localhost:5000/api/product/1)

To Stop the docker container:
```bash
docker compose down
```

## Why Docker ? 

The best thing about Docker is — you don’t need to install anything like Visual Studio, MSSQL, or any .NET SDK to run this project.
Everything is already included inside the Docker containers.

You just need Docker Desktop installed (with WSL enabled on Windows).
Once that’s done, you can run the whole project with a single command, and Docker will automatically pull everything it needs — the API, SQL Server, and all required dependencies.

It doesn’t matter which system you are using — Windows, Linux, or Mac — the project will run the same way everywhere without any setup issues.

---

## Quick Access
- **Redirect Folder:** `cd LiquidLabsProducts`
- **Run Docker Container Cmd:** `docker compose up -d --build`
- **Base Url:** http://localhost:5000
- **API Testing Tool (Recommended):** Postman
- **Git Repository:** https://github.com/Rifnaz/LiquidLabsProducts

---

## API Endpoints

| Method | API ndpoint | Description |
|----------|----------|----------|
| GET | http://localhost:5000/api/product | Get all products |
| GET | http://localhost:5000/api/product/{id} | Get products by Id |


---


**Thanks for checking it out! 🙌**

**By Rifnaz**
