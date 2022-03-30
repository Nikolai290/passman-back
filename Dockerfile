#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["passman-back/passman-back.csproj", "passman-back/"]
RUN dotnet restore "passman-back/passman-back.csproj"
COPY . .
WORKDIR "/src/passman-back"
RUN dotnet build "passman-back.csproj" -c Debug -o /app/build

FROM build AS publish
RUN dotnet publish "passman-back.csproj" -c Debug -o /app/publish

FROM base AS final
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:5000
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "passman-back.dll"]