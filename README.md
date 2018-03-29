# Inobits Docker demo
Demo project for Docker using ASP.NET Core 2.0

### Instructions

1. Install Docker
2. Download the project
```git clone https://github.com/LouisDK/liver.git```

3. Open a shell in the *liver* folder (the one that contains the *Docker-Compose.yml* file)
4. Create a network: `docker network create nat`
5. Create a volume for the SQL Server database: `docker volume create --name sqlvol1`
6. Build the images `docker-compose build`
7. Create and run the containers `docker-compose up -d`
8. To continously test if its running, run `webping.ps1` (_Control+C_ to stop it)
9. Open a browser and go to http://localhost:5000  
10. If the web application complains about the database being unavailable, it was not initialized properly. 
* Find the Container ID for the *liver_db* container: `docker ps`
* Run `docker exec -it 68c /opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P 'Vam00s123!' -i /opt/mssql-scripts/DBCreate.sql`
11. To see how much resources (e.g. CPU) are consumed, run `Docker stats`

### Clean up
1. Run `Docker-compose down`
2. Remove images (find image id `docker images`, then remove it `docker rmi <image id>`)
3. Remove volume (docker volume rm sqlvol1)
4. Remove network (find network id - `docker network ls`, then remove `docker network rm <network id>`)

Enjoy!

-Louis de Klerk (louis at inobits.com)