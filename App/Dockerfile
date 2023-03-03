FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM node:19-alpine AS build-ui
WORKDIR /app
COPY ./Ui .
RUN npm install
RUN npm run build


FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["App/App.csproj", "App/"]
RUN dotnet restore "App/App.csproj"
COPY . .
WORKDIR "/src/App"
RUN dotnet build "App.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "App.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=build-ui /app/dist ./wwwroot
ENTRYPOINT ["dotnet", "App.dll", "--urls", "http://0.0.0.0:8080"]
