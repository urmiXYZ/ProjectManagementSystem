# ProjectSphere

ProjectSphere is a **Project Management System** developed using **ASP.NET Core MVC**. It provides role-based access and allows users to manage projects efficiently. The system supports authentication, project assignment, tracking, and reporting, making project management simple and organized.

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




