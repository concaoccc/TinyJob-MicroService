# Set up SQL service in the local
$dbPwd = Read-Host "Enter password (at least eight characters long and contain uppercase&lowercase&digit):"
docker rm -f mssql2022
docker run -d --name mssql2022 --hostname  mssql2022 -p 1433:1433  -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=$dbPwd" -e "MSSQL_PID=Express" mcr.microsoft.com/mssql/server:2022-latest
$connectionString = "Server=localhost;Database=TaskManagement;User Id=sa;Password=$dbPwd;TrustServerCertificate=True;pooling=true;connection lifetime=0;min pool size = 1;max pool size=500"
[System.Environment]::SetEnvironmentVariable('DbConnectionString',"$connectionString",[System.EnvironmentVariableTarget]::Machine)
