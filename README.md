# Introduction 
This repo is the codes for a [FHL project](https://hackbox.microsoft.com/hackathons/hackathon2023/project/29624). In the project, we will implement a distributed task system.

# Requirements
- VS2022
- Docker desktop
- dotnet 6+
- Kafka Management [tool](https://www.kafkatool.com/download.html)

# Setup
## Database
- run `setup-db.ps1`
- Replace the `*` with the real password in the appsettings.json (`Please don't commit the pwd to the remote`)
- Open the `Packge Manage Console` in the VS2022(Tools => Nuget Package Manage => Packge Manage Console) and run `Update-Databse`
## Kafka
- run `Setup-kafka.ps1`
- Open Kafka Management tool(Offset Explorer)
    - Cluster => Add New Connection
    -  switch the Properties
        - Cluster Name(defined by yourself)
        - ZoopKeeper Port => 22181
    - switch the Advanced
        - Bootstrap servers => localhost:29092
    - Connect to the cluster
# Build and Test
## Build

# Contribute
TODO: Explain how other users and developers can contribute to make your code better. 

If you want to learn more about creating good readme files then refer the following [guidelines](https://docs.microsoft.com/en-us/azure/devops/repos/git/create-a-readme?view=azure-devops). You can also seek inspiration from the below readme files:
- [ASP.NET Core](https://github.com/aspnet/Home)
- [Visual Studio Code](https://github.com/Microsoft/vscode)
- [Chakra Core](https://github.com/Microsoft/ChakraCore)