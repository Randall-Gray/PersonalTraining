# PersonalTraining

This repository is my individual capstone project for devCodeCamp in Milwaukee, WI.  It consists of an ASP.NET Core REST Web API backend, PersonalTraining, with an ASP.NET Core MVC Web application frontend, SPFWebsitMVC.  The implementation is to support client/trainer interaction for a small fitness/health business called Starting Point Fitness (SPF).  Client, trainer, and admin roles are implemented using the framework built in Authentification.

Admins:  Admins may monitor all client and trainer activites.  A single admin must be seeded into the database.  An admin may then promote clients to trainers and trainers to admins.  Admins may broadcast messages over the client and trainer pages.

Trainers:  Trainers may monitor all client activities.  They may post workout videos to an internal library and assign videos to clients to guide their training.  Trainers and clients may participate in (non-realtime) back-and-forth conversations.  Trainers may post FAQ for all client review.  Trainers may also schedule training classes/sessions through embedded Google sheets.

Clients:  Clients may review and schedule training and coaching sessions through embedded Google sheets.  Clients may review a library of videos, selecting favorites to appear on their personal page.  Clients may track and graph their progress (weight) using the Google Graphs API.  Clients and trainers may participate in (non-realtime) back-and-forth conversations.

Non-login Users:  Non-logged in users may review the services offered by Starting Point Fitness.  Contact information is available to ask further questions.

This project was completed in 2 weeks.  It is far from polished and could benefit from a good cleanup and page reorganization.  Not all database tables in the backend have been implemented in the frontend.  They remain for possible future enhancements. An Entity Relationship Diagram is available in the Documentation project sub-folder.

Technologies Demonstrated:  ASP.NET Core REST Web api and MVC app;  HTTP and AJAX communications between the front and back ends; Google map and graph api usage; Embedded Google sheet.

Environment: Visual Studio and Google Chrome IDEs.

