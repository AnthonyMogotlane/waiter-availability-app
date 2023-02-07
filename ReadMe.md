# Waiter Availability App

Waiter-Availability-App is an application to help restaurant managers to schedule weekly waiters shifts, it allows waiters to select days they can work on and update days they can work on. The manager can see how many waiters are available to work on and reset the schedule for a new week.

![](https://anthonymogotlane.github.io/logic-gates/waiters-app/screen-one.png)
![](https://anthonymogotlane.github.io/logic-gates/waiters-app/screen-two.png)

## Technologies
Project is created with: 
* ASP.Net core Razor Pages
* C#
* Javascript
* Bootstrap
* Postgresql

## Live Demo
[waitersavailabilityapp.com](http://waiters.anthony.projectcodex.net/)

## Getting started
To run the app on your local computer:
  * Clone the repository
  ```
    git clone https://github.com/AnthonyMogotlane/waiter-availability-app.git
  ```
  * Install .Net 6 SDK: 
  [link](https://learn.microsoft.com/en-us/samples/browse/)
  * Install Postgresql: 
  [link](https://www.digitalocean.com/community/tutorials/how-to-install-postgresql-on-ubuntu-20-04-quickstart)
  * Create tables below using postgresql
  ![Schema Snapshot](https://anthonymogotlane.github.io/logic-gates/waiters-app/database.png)
  ```
    CREATE TABLE IF NOT EXISTS weekdays(
        id serial PRIMARY KEY,
        day VARCHAR(50) NOT NULL
    );

    CREATE TABLE IF NOT EXISTS waiters (
        id serial PRIMARY KEY,
        firstname VARCHAR(50) NOT NULL
    );

    CREATE TABLE IF NOT EXISTS schedule (
        id serial PRIMARY KEY,
        day_id int NOT NULL,
        waiter_id int NOT NULL,
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
  * Setup on server:
    * Install .Net 6 SDK on the server: [dotnet installation guide](https://learn.microsoft.com/en-us/samples/browse/)
    * Install git: [git installation guide](https://www.digitalocean.com/community/tutorials/how-to-install-git-on-ubuntu-20-04)
    * Install nginx: [nginx installation guide](https://www.digitalocean.com/community/tutorials/how-to-install-nginx-on-ubuntu-20-04)
      - You should be able to access the running nginx web server now using this command: http://your_ip_adress_here
   
  
