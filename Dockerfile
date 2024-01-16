FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /source

COPY . ./
RUN dotnet restore --disable-parallel
WORKDIR /source/src/MyYearGoalsBot
RUN dotnet publish -c release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app ./
#ENTRYPOINT ["dotnet", "MyYearGoalsBot.dll", "--environment=Development"]
ENTRYPOINT ["dotnet", "MyYearGoalsBot.dll"]

