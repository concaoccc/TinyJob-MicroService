FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 as build
ARG Csproj="Executor.csproj"
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
ENTRYPOINT ["dotnet", "Executor.dll"]