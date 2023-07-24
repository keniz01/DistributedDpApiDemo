## Running Microsoft.AspNetCore.DataProtection library in a distributed environment

The DistributedDpApiDemo demo app demonstrated the usage of the **Microsoft.AspNetCore.DataProtection** library in a docker-driven distributed environment using docker compose.

This application uses docker compose override (docker-compose.override.yml) file to hide secrets and prevent accidental commits. This will have to be created manually - its should not be committed!

During the initial pahse of development, there were issues with 

The current docker-compose.override.yml file is shown below.

 ```
 version: '3.4'
 services:
    distributeddpapidemo:  
        environment:
            - ASPNETCORE_Kestrel__Certificates__Default__Password=xxxxxxxxxxx
```




### References
- [Configure ASP.NET Core Data Protection](https://learn.microsoft.com/en-us/aspnet/core/security/data-protection/configuration/overview?view=aspnetcore-7.0#persistkeystofilesystem)
- [Running docker container images with HTTPS](https://learn.microsoft.com/en-us/aspnet/core/security/docker-https?view=aspnetcore-7.0#running-pre-built-container-images-with-https)