FROM mcr.microsoft.com/dotnet/aspnet:6.0 as base
WORKDIR /App

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG Csproj="WebService.csproj"
WORKDIR /src
# Copy everything
COPY . ./
# Restore as distinct layers
RUN dotnet restore $CSproj
# Build and publish a release
RUN dotnet build $CSproj -c Release -o /app/build

FROM build AS publish
RUN dotnet publish $CSproj -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 5000/tcp
ENV ASPNETCORE_URLS=http://*:5000
ENTRYPOINT ["dotnet", "WebService.dll"]