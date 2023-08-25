FROM mcr.microsoft.com/dotnet/sdk:6.0 as build-env
WORKDIR /src/PruebaConsalud
COPY src/PruebaConsalud/*.csproj .
RUN dotnet restore
COPY src/PruebaConsalud .
RUN dotnet publish -c Release -o /PruebaConsalud/publish

FROM mcr.microsoft.com/dotnet/aspnet:6.0 as runtime
WORKDIR /publish
COPY --from=build-env /PruebaConsalud/publish .
EXPOSE 80
ENTRYPOINT ["dotnet", "PruebaConsalud.dll"]
