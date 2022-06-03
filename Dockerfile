FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS build
WORKDIR /src
COPY ["passman-back/passman-back.csproj", "passman-back/"]
RUN dotnet restore "passman-back/passman-back.csproj"
COPY . .
WORKDIR "/src/passman-back"
RUN dotnet build "passman-back.csproj" -c Debug -o /app/build

FROM build AS publish
RUN dotnet publish "passman-back.csproj" -c Debug -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine AS final
WORKDIR /app
ENV ASPNETCORE_ENVIRONMENT=Development
COPY --from=publish /app/publish .
# COPY passman-back/RSA /app/RSA
ENTRYPOINT ["dotnet", "passman-back.dll"]