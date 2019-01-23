# HR Master - recrutation app

This system is made for simplify recruation process.
As a GUEST you can:

- View job offers;
- View details about job offer.
  
As a USER you can:

- Apply for a job offer.

As an ADMIN (aka HR team member) you can:

- View Companies;
- Add new company;
- Edit and Delete existing Company;
- View Job Offers list;
- Add new Job Offer;
- Edit and Delete existing Job Offer;
- View list of Applications for each Job Offer;
- Review each Application;
- View CVs and status of them (Revieved: yes/no) on Sharepoint page.

## Architecture

// TBD

## Technologies used:

| Technology name | Purpose |
|------|-------|
| [ASP.NET Core MVC](http://asp.net/) | Framework used to serve whole app: views, controllers etc. |
| Azure AAD B2C | Identity provider. |
| Azure SQL db | Database. |
| Azure Blob Storage | Used to host CV files. |
| Sharepoint Online | Store link to applicants' CVs and state of theirs recrutation process. |
| Azure Logic Apps | Connect Azure Blob Storage and Sharepoint list. When CV is uploaded to az blob new row is being created in sp list. |
| [Bootstrap](https://getbootstrap.com/) | To sites look nice (I'm not any good at styling and views) and also to be responsive. |
| ASP.NET Core API | Serve paged lists (eg. on job offers list). |
| AJAX & jQuery | Display and control paged lists (eg. on job offers list). |
| Swagger | Show API endpoints documentation. |

## TO-DO

- [ ] Add architecture diagram
- [ ] Add ARM templates to set up all Azure services
