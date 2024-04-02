#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["CMS.MentApi/CMS.MentApi.csproj", "CMS.MentApi/"]
COPY ["CMS.BusinessInterface/CMS.BusinessInterface.csproj", "CMS.BusinessInterface/"]
COPY ["CMS.Common/CMS.Common.csproj", "CMS.Common/"]
COPY ["CMS.Models/CMS.Models.csproj", "CMS.Models/"]
COPY ["CMS.BusinessService/CMS.BusinessService.csproj", "CMS.BusinessService/"]
RUN dotnet restore "./CMS.MentApi/CMS.MentApi.csproj"
COPY . .
WORKDIR "/src/CMS.MentApi"
RUN dotnet build "./CMS.MentApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CMS.MentApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CMS.MentApi.dll"]