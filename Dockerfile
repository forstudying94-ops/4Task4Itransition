FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY 4task4/4task4.csproj 4task4/
RUN dotnet restore 4task4/4task4.csproj
COPY 4task4/ 4task4/
RUN dotnet publish 4task4/4task4.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "4task4.dll"]
