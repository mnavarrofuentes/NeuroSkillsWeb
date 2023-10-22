FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
EXPOSE 7115

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY ["/NeuroSkills.Web/NeuroSkills.Web.csproj", "NeuroSkills.Web/"]

RUN dotnet restore "NeuroSkills.Web/NeuroSkills.Web.csproj"

COPY . .

WORKDIR "/src/NeuroSkills.Web"

RUN dotnet build "NeuroSkills.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NeuroSkills.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENV ASPNETCORE_URLS=http://0.0.0.0:7115

ENTRYPOINT [ "dotnet", "NeuroSkills.Web.dll" ]