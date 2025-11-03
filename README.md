# SimpleBookstore

1. Open SimpleBookstore.sln in your favourite IDE
2. Search for appsettings.json file and change SimpleBookstoreDbConnection db password to your own password
3. Run SimpleBookstore application - this will create DB on your local machine 
4. go to https://localhost:7075/swagger/index.html 
5. Use login endpoint with credentials:
        - Read role: 
            --Username: ReadUser, 
            --Password: ReadUserPassword
        -ReadWrite role:
            --Username: ReadWriteUser, 
            --Password: ReadWriteUserPassword
    Response will give you accessToken. Copy it. Goto Authorization button and enter copied accessToken there. Click on login.
    You are now logged in and can access get endpoints with ReadUser or ReadWriteUser. You can access Create, Read, Update and Delete endpoints with ReadWrite user.
6. Once logged in - you can access get endpoints with ReadUser or ReadWriteUser. You can access Create, Update and Delete endpoints with ReadWrite user.

Additionally: 
7. Run BookRetriever AzureFunction. This will create and import between 1000 and 5000 book entities along with the authors and repositories.