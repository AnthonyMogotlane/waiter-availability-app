# Waiter Availability App
[![.NET](https://github.com/AnthonyMogotlane/waiter-availability-app/actions/workflows/dotnet.yml/badge.svg)](https://github.com/AnthonyMogotlane/waiter-availability-app/actions/workflows/dotnet.yml)

The Waiter-Availability-App is designed to assist restaurant managers in creating weekly schedules for waiters. It enables waiters to indicate their preferred working days and make updates as needed. The app provides managers with an overview of the number of available waiters for each day and allows the manager to reset the schedule for that week up to four weeks.

<div style="display: flex;">
<img width="209" src="https://firebasestorage.googleapis.com/v0/b/anthonymogotlane.appspot.com/o/waiters-screen-1.png?alt=media&token=32424004-2249-48cc-9b8f-dfa6ad38d033" />

<img width="209" src="https://firebasestorage.googleapis.com/v0/b/anthonymogotlane.appspot.com/o/waiters-screen-2.png?alt=media&token=32424004-2249-48cc-9b8f-dfa6ad38d033" />

<img width="209" src="https://firebasestorage.googleapis.com/v0/b/anthonymogotlane.appspot.com/o/waiters-screen-3.png?alt=media&token=32424004-2249-48cc-9b8f-dfa6ad38d033" />
 </div>

## Technologies
Project is created with: 
* ASP.Net core Razor Pages
* C#
* Javascript
* Bootstrap
* Postgresql

## Live Demo
[waiters.anthony.projectcodex.net](http://waiters.anthony.projectcodex.net/)

## Getting started
To run the app on your local computer:
  * Clone the repository
  ```
    git clone https://github.com/AnthonyMogotlane/waiter-availability-app.git
  ```
  * Install .Net 6 SDK: 
  [dotnet installation guide](https://learn.microsoft.com/en-us/samples/browse/)
  * Install Postgresql: 
  [postgresql installation guide](https://www.digitalocean.com/community/tutorials/how-to-install-postgresql-on-ubuntu-20-04-quickstart)
  * Create tables below using postgresql
  ```
   CREATE TABLE IF NOT EXISTS weekdays(
     id serial PRIMARY KEY,
     day VARCHAR(50) NOT NULL UNIQUE
   );

   CREATE TABLE IF NOT EXISTS waiters (
       id serial PRIMARY KEY,
       firstname VARCHAR(50) NOT NULL,
       password VARCHAR(50) NOT NULL
   );

   CREATE TABLE IF NOT EXISTS schedule (
       id serial PRIMARY KEY,
       day_id int NOT NULL,
       waiter_id int NOT NULL,
       dates varchar(30) NOT NULL,
       FOREIGN KEY (day_id) REFERENCES weekdays(id),
       FOREIGN KEY (waiter_id) REFERENCES waiters(id)
   );
  ```
  
  * Move to `razor-pages/WaiterAvailabilityApp` directory, create `appsettings.json` file and add the connection string as shown below:
  ```
    {
      "ConnectionStrings": {
      "DefaultConnection": "Server=XXXX;Port=XXXX;Database=XXXX;UserId=XXXX;Password=XXXX"
    }
  ```
  `xxxx` = relavant values
  
  ## Running the app locally
  While in the `razor-pages/WaiterAvailabilityApp` directory run `dotnet watch run` command on the terminal, the application will launch in the browser.
  
  ## Deployment
  **The app is deployed on Digital Ocean**
  
  To deploy it on Digital Ocean you need to:
  * Created an ubuntu `server` or `droplet`
  * Login to the server on a terminal using root:
    ```
      ssh root@your.ip.address
    ```
  * **Setup on server**:
    * Install .Net 6 SDK on the server: [dotnet installation guide](https://learn.microsoft.com/en-us/samples/browse/)
    * Install postgresql: [postgresql installation guide](https://www.digitalocean.com/community/tutorials/how-to-install-postgresql-on-ubuntu-20-04-quickstart)
    * Install git: [git installation guide](https://www.digitalocean.com/community/tutorials/how-to-install-git-on-ubuntu-20-04)
    * Install nginx: [nginx installation guide](https://www.digitalocean.com/community/tutorials/how-to-install-nginx-on-ubuntu-20-04)
      - After following ngixn installation guide, you should be able to access the running nginx web server now using http://your_ip_adress_here
  * **Run a the Web App on the server**:
     * Clone the repository
     ```
     git clone https://github.com/AnthonyMogotlane/waiter-availability-app.git
     ```
     * Change to the cloned folder and Navigate to `razor-pages/WaiterAvailabilityApp`
     * Run these dotnet commands - to restore all local dependencies & to build & run the app:
     ```
     dotnet restore
     dotnet build  -c release
     dotnet bin/release/net6.0/WaiterAvailabilityAppRazor.dll --urls=http://localhost:6007
     ```
     * At this point the app should be running at: http://your_server_ip_address
     * Run your app in the background using this command:
     ```
     nohup dotnet bin/Release/net6.0/WaiterAvailabilityAppRazor.dll --urls=http://localhost:6007/ > vps.log 2>&1 &
     ```
     You can use bellow command to see the `process id` for the app:
     ```
     ps -eaf | grep WaiterAvailabilityApp
     ```
  ## Author
  Anthony Mogotlane
   
  
