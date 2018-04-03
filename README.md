# Inobits Docker demo
Demo project for Docker using ASP.NET Core 2.0

# Instructions for use with Docker (Mac or Windows)

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


# Instructions for use with Kubernetes in Azure (AKS)
1. Get an Azure subscription
2. Install the Azure CLI (Command Line Interface) (Or, if you dont want to, you can use the Cloud Console)
3. Create a Resource Group to host your cluster. Must be an empty resource group. `az group create --name <ResourceGroupName> --location eastus` Note: I tried and failed with West Europe, but East US works fine. (2 April 2018)
4. Create the Kubernetes Cluster in Azure: `az aks create --resource-group <ResourceGroupName> --name <ClusterName> --node-count 1 --generate-ssh-keys`
5. Get the credentials needed to use KubeCTL locally: `az aks get-credentials --resource-group <ResourceGroupName> --name <ClusterName>`
6. Download KubeCTL (the Kubernetes console) (unzip and place in the root of C:\ )
7. Create the 'dev' namespace in Kubernetes - we will do everything in this namespace: `kubectl create -f .\miner-ns-dev.yaml`
8. Create a Context for the dev namespace: `kubectl config set-context dev --namespace=miner-dev --cluster=<ClusterName> --user=clusterUser_<ResourceGroupName>_<ClusterName>` - this allows you to execute KubeCTL commands that only affect this namespace.
Create the 'prod' namespace in Kubernetes (not used yet): `kubectl create -f .\miner-ns-prod.yaml`
9. Create a Context for the dev namespace: `kubectl config set-context prod --namespace=miner-prod --cluster=<ClusterName> --user=clusterUser_<ResourceGroupName>_<ClusterName>` - this allows you to execute KubeCTL commands that only affect this namespace.
10. Use the 'dev' namespace: `kubectl config use-context dev` - all KubeCTL commands will now only affect this namespace (isolated from all other namespaces)
11. Create a Secret used to pull down images from my private Azure Container Registry `kubectl create secret docker-registry inobitscr --docker-server=https://inobitscr.azurecr.io --docker-username=inobitscr --docker-password=V=MejXdKyepx9UqvV5pDLJuhkPw5yOES --docker-email=nullpointer@inobits.com` - you can also use your own private Azure Container Registry, or a public Docker registry (public docker registries does not need a secret like this in order for Kubernetes to pull it)
12. Create a Secret to contain our SQL Server's SA password (so that it will not be hardcoded in config files, or stored our git repository): `kubectl create secret generic mssqlsapwd --from-literal=SA_PASSWORD="Vam00s321!"`
13. Create a Secret to contain our Connection String: `kubectl create secret generic connectionstringmining --from-literal=connectionstringmining="Data Source=basic-miner-db-svc;Initial Catalog=inodemo1;UID=sa;Password=Vam00s321!;Connection Timeout=10;MultipleActiveResultSets=True;"`
14. Create a Storage 'Persistant Volume Claim' that maps to an Azure Disk (attached to the Node(s) (Azure VM) of the Cluster) `kubectl apply -f .\basic-miner-sqlstorage.yaml`
15. Wait for the Persistant Volume Claim to be 'Bound' before going to the next step (should be about a minute). Check its state like this: `kubectl describe mssql-data-vol1-pvc` (look for its State - it should be 'Bound')
16. Create the Deployment, Pod and Service for the Database: `kubectl create -f .\basic-miner-db.yaml` Note that the service exposes the database to the outside world (useful for tests, demos, backups), but obviously not secure. In real-world scenarios you might want to change this service so that it is only available internal to the Kubernetes Cluster, and not to the outside world.
17. Create the Deployment, Pod and Service for the Web server: `kubectl create -f .\basic-miner-web.yaml` Note that its Service creates a Load Balancer with an externally available IP Address. 
18. Get the publicly exposed IP address: `kubectl get services ` (look for the External IP address of the *ino-miner-basic-web* service). Open a browser and go to this IP address. You should see the application.

The application should be up and running in about 1-2 minutes. My private Azure Container Registry is in Europe, so pulling the images from there to the US East datacenter might take a few minutes.

At any stage after step 6, you can also view progress in the Kubernetes WebUI. Launch it like this: `az aks browse --resource-group <ResourceGroupName> --name <ClusterName>` 
*Note*: Remember everything is deployed to the 'miner-dev' namespace - so in the WebUI, change the namespace to miner-dev (in the side menu on the left).

# Application Notes:
1. Change the difficulty of mining by going to the http://<ExternalIPAddress>/manage url
2. Continuously test if the application is running by polling `http://<ExternalIPAddress>/health` url. I provided a script to ping it in an endless loop. Edit `webping.ps1` and change the url to your external ipaddress.
3. Continuously test if the application can talk to its database by polling `http://<ExternalIPAddress>/dbhealth` url. I provided a script to ping it in an endless loop. Edit `webping.ps1` and change the url. 
4. See what the effective ConnectionString is by going to `http://<ExternalIPAddress>/about` (This is obviously a BAD security practive to expose this - but this is a demo)
Enjoy!

## Demo 1: (simulating a web server dying)
While the application is running:
1. Start a shell, and contiously ping the web server (See note 2 above). Keep this window visible.
2. Get the Pod name of the web server: `kubectl get pods`
3. Kill the pod that runs the web server: `kubectl delete pod <podname>`
4. Note in the shell window where we ping the web server that the service is disrupted for 1-3 seconds, before it resumes service. Note that the name of the server changed. This means that when the old pod (and its container) died, another pod (and another new instance of that container) was created and put in place.

## Demo 2: (simulating a database server dying)
While the application is running:
1. Start a shell, and contiously ask the web server if it can talk to the database server (See note 3 above). Keep this window visible.
2. Get the Pod name of the web server: `kubectl get pods`
3. Kill the pod that runs the database server: `kubectl delete pod <podname>`
4. Note in the shell window where we ping the web server that the service reports that it cannot contact the database server for 2-5 seconds, before it resumes service. Note that the name of the server changed. This means that when the old pod (and its container) died, another pod (and another new instance of that container) was created and put in place. At that stage the newly running SQL Server connected to the external volume (Azure Disk) where the database files are kept, attached these, recovered them, and started to serve the database.

### Clean up
 - Delete the Azure Resource Group - this removes EVERYTHING (including the data in the persisted volume)

Note: All data in the database will be lost if the Cluster is deleted - since deleting the cluster removes all Azure resources in the resource group - including the Azure Disk where the data is kept. In a real-world scenario you might want to back up this data at some stage...

Please share your thought, recommendations, comments - mail me at louis <at> inobits.com

-Louis de Klerk (louis at inobits.com)