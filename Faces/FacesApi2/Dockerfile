#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["Faces/FacesApi2/FacesApi2.csproj", "Faces/FacesApi2/"]
RUN dotnet restore "Faces/FacesApi2/FacesApi2.csproj"
COPY . .
WORKDIR "/src/Faces/FacesApi2"
RUN dotnet build "FacesApi2.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FacesApi2.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FacesApi2.dll"]
