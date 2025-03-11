# JobsAPI Project

This project consists of a **Job Management System** with a **REST API (JobsAPI)**, a **Command Line Interface (CLI)**, and **Unit Tests (JobsAPI.Tests)**. 
It follows **SOLID principles**, utilizes **Entity Framework Core**, and supports **MySQL** for database management.

---

## **Project Structure**
The solution includes three main projects:

1️ **JobsAPI** → The backend, which handles job creation and management.  
2️ **CLI** → A terminal-based interface for users to interact with the API.  
3️ **JobsAPI.Tests** → A unit testing project ensuring the system’s reliability.

---

## **Technologies Used**
- **C# (.NET 6)**
- **Entity Framework Core**
- **MySQL** (for cross-platform database compatibility)
- **AutoMapper** (to map models to DTOs)
- **xUnit & Moq** (for unit testing)
- **Serilog** (for logging)
- **InMemoryDatabase** (for test database)

---

## **Setup & Installation**
### **1️ Clone the Repository**
git clone https://github.com/03jose/CBS-Jobs.git
cd jobs-api

### **2 Configure the Database**
Modify the appsettings.json file in JobsAPI:
"ConnectionStrings": {
  "DefaultConnection": "server=localhost;database=JobsDB;user=root;password=yourpassword"
}

### **3 Apply Database Migrations**
cd JobsAPI
dotnet ef migrations add InitialCreate
dotnet ef database update

## **API Endpoints**
HTTP Method ->	Endpoint ->	Description
- POST ->	/api/jobtypes ->	Create a new job type
- GET	-> /api/jobtypes ->	Retrieve all job types
- POST ->	/api/jobs/start ->	Start a new job
- GET	-> /api/jobs/status/{id} ->	Get job status by ID
- GET	-> /api/jobs/running ->	List all running jobs
- POST	-> /api/jobs/cancel/{id} ->	Cancel a job
