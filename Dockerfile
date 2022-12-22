# Feel free to replace this with a more appropriate Dockerfile.

FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS build
COPY . .
RUN dotnet publish "src/Imagination.Server.App/Imagination.Server.App.csproj" -c release -r linux-x64 -o /app 
RUN dotnet publish "src/Imagination.Server/Imagination.Server.csproj" -c release -o /app 

FROM mcr.microsoft.com/dotnet/aspnet:6.0-focal
WORKDIR /app
COPY --from=build /app ./
EXPOSE 5000
ENTRYPOINT ["dotnet","Imagination.Server.dll"]
