FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
RUN curl --silent --location https://deb.nodesource.com/setup_14.x | bash -
RUN apt-get install --yes nodejs
WORKDIR /src
COPY ["Ionta.OSC.Core/Ionta.OSC.Core.csproj", "Ionta.OSC.Core/"]
COPY ["Ionta.OSC.App/Ionta.OSC.App.csproj", "Ionta.OSC.App/"]
COPY ["Ionta.OSC.Domain/Ionta.OSC.Domain.csproj", "Ionta.OSC.Domain/"]
COPY ["Ionta.OSC.Storage/Ionta.OSC.Storage.csproj", "Ionta.OSC.Storage/"]
COPY ["Ionta.OSC.Web/Ionta.OSC.Web.csproj", "Ionta.OSC.Web/"]
RUN dotnet restore "Ionta.OSC.Web/Ionta.OSC.Web.csproj"
COPY . .
WORKDIR "/src/Ionta.OSC.Web"
RUN dotnet build "Ionta.OSC.Web.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Ionta.OSC.Web.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Ionta.OSC.Web.dll"]
