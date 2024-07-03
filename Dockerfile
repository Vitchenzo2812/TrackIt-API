FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /App

COPY ./service ./
RUN dotnet restore ./TrackIt.WebApi
RUN dotnet publish ./TrackIt.WebApi -c Release -o out

ENV ASPNETCORE_URLS=http://+:2025

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /App
COPY --from=build-env /App/out .
ENTRYPOINT ["dotnet", "TrackIt.WebApi.dll"]