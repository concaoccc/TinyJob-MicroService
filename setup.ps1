$currentDirectory = Get-Location

$dbPwd = Read-Host -MaskInput "Enter database password"

# build images
docker build -t WebService-image -f $currentDirectory/src/WebService/Dockerfile .
docker build -t Scheduler-image -f $currentDirectory/src/Scheduler/Dockerfile .
docker build -t Executor-image -f $currentDirectory/src/Executor/Dockerfile .

#Set up Web service
docker remove -f webservice -ErrorAction SilentlyContinue
docker run -d -p 8080:5000 --name webservice WebService-image

# set up SQL server
. .\setup-db.ps1
