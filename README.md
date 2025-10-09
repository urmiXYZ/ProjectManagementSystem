# ProjectSphere

ProjectSphere is a **Project Management System** developed using **ASP.NET Core MVC**. It provides role-based access and allows users to manage projects efficiently. The system supports authentication, project assignment, tracking, and reporting, making project management simple and organized.

---

Demo in YT: https://youtu.be/jk2F4MkLYFE?si=8hVq-BL50qqrovy-

---

## Seeded Data

This project comes with pre-populated (seeded) data in the database. The following data is automatically inserted when the application is run for the first time.

---

### 1. Departments
| DepartmentId | Name                  | Description                       |
|--------------|---------------------|-----------------------------------|
| 1            | Software Development | Handles backend and frontend apps |
| 2            | HR                   | Handles recruitment and employee management |
| 3            | Marketing            | Promotes products and manages campaigns |

---

### 2. Roles
| RoleId | Name       |
|--------|------------|
| 1      | SuperAdmin |
| 2      | Admin      |
| 3      | Employee   |

---

### 3. Users
| Id | UserName    | Email                  | Role       | Department            | Password       |
|----|------------|-----------------------|------------|---------------------|----------------|
| 1  | superadmin | superadmin@pms.com    | SuperAdmin | Software Development | Super@123      |
| 2  | admin      | admin@pms.com         | Admin      | HR                  | Admin@123      |
| 3  | employee   | employee@pms.com      | Employee   | Marketing           | Employee@123   |

> **Note:** Passwords above are the default seeded passwords for demo/testing purposes.

---

### 4. Categories
| CategoryId | Name               | Department              |
|------------|------------------|------------------------|
| 1          | Web Applications  | Software Development   |
| 2          | Recruitment       | HR                     |
| 3          | Digital Campaigns | Marketing              |

---

### 5. Projects
| ProjectId | ProjectName         | Category             |
|-----------|-------------------|--------------------|
| 1         | Task Manager API   | Web Applications    |
| 2         | Hiring Portal      | Recruitment         |
| 3         | Social Media Boost | Digital Campaigns   |

---

### 6. Assigned Projects
| AssignedId | Project          | User     | Status      | DueDate             |
|------------|-----------------|---------|------------|--------------------|
| 1          | Task Manager API | employee | InProgress | 15 days from today |

---

### How it works
- Seeded data is inserted automatically via **Entity Framework Core `HasData()`** in the `ProjectDbContext`.  
- Make sure **migrations are applied** before running the application, otherwise the seed data will not populate.

---

## Table of Contents

- [Features](#features)  
- [Technologies Used](#technologies-used)  
- [User Roles](#user-roles)  
- [Installation](#installation)  
- [Screenshots](#screenshots)  


---

## Features

### General Features
-User authentication and role-based authorization
-Home, About, and Privacy pages accessible without login
-Responsive and user-friendly dashboard

### Employee Features
- View **assigned projects** and their statuses  
- **Submit projects**, automatically logging submission date  
- Color-coded **calendar** to track project progress  
- Update **profile information** and upload a profile picture  

### Admin Features
- **Dashboard summaries**: users, projects, active projects  
- **User management**: add, edit, delete, view, and export users  
- **Category management**: add, edit, delete categories per department  
- **Project management**: add, edit, delete, and export projects  
- View and **manage assigned projects** of employees  
- Update their own **profile information**  

### Super Admin Features
- All **Admin features**  
- **Role management**: add, edit, delete roles  
- **Department management**: add, edit, delete departments  
- View employee count in each department and category  

---

## Technologies Used
- **ASP.NET Core MVC**  
- **Entity Framework Core**  
- **ASP.NET Identity** for authentication  
- **SQL Server** for database  
- **Bootstrap 5** for frontend styling
- **Ajax** **JQuery** **Razor**

---

Database Setup (2019)

This project includes a SQL script to create and populate the database with the required tables and sample data. You can use it to set up your local environment quickly. (file named DatabaseScript)

---

User Credentials

Password = first 3 letters(in uppercase) of the username + *566#p

| Username  | Password   |
| --------- | ---------- |
| farhana   | far\*566#p |
| adminuser | adm\*566#p |
| employee1 | emp\*566#p |


The passwords are generated using the following rule:

## User Roles
1. **Employee** – Can view and manage their assigned projects  
2. **Admin** – Can manage users, projects, categories, and view reports  
3. **Super Admin** – Can manage everything including roles and departments  

---

## 

Update the appsettings.json with your SQL Server connection string:
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;Database=ProjectSphereDB;Trusted_Connection=True;"
}

##

Run Entity Framework migrations to create the database:
Update-Database

##Screenshots

Landing page-Home
<img width="1920" height="1080" alt="image" src="https://github.com/user-attachments/assets/154f300e-7233-421a-8f85-6cd7e8823478" />

Dashboard(SuperAdmin/Admin)
<img width="1920" height="1080" alt="image" src="https://github.com/user-attachments/assets/805cf349-5788-405d-9d71-e934997b47d5" />
<img width="1920" height="1080" alt="image" src="https://github.com/user-attachments/assets/219a3cb7-632b-4022-8599-40ce6ee01f69" />

(Employee)
<img width="1920" height="1080" alt="image" src="https://github.com/user-attachments/assets/6e2dd06d-0f36-4f2a-84f5-4f52510eca40" />
<img width="1920" height="1080" alt="image" src="https://github.com/user-attachments/assets/64ecb231-e0df-477d-980b-854528327826" />




