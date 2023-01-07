FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
RUN curl --silent --location https://deb.nodesource.com/setup_14.x | bash -
RUN apt-get install --yes nodejs
WORKDIR /src
COPY ["AssemblyLoader/AssemblyLoader.csproj", "AssemblyLoader/"]
COPY ["Ionta.OSC.App/Ionta.OSC.App.csproj", "Ionta.OSC.App/"]
COPY ["Ionta.OSC.Domain/Ionta.OSC.Domain.csproj", "Ionta.OSC.Domain/"]
COPY ["Ionta.OSC.Storage/Ionta.OSC.Storage.csproj", "Ionta.OSC.Storage/"]
COPY ["Ionta.ServiceProvider/Ionta.ServiceTools.csproj", "Ionta.ServiceTools/"]
COPY ["Ionta.StoreLoader/Ionta.StoreLoader.csproj", "Ionta.StoreLoader/"]
COPY ["OpenServiceCreator/OpenServiceCreator.csproj", "OpenServiceCreator/"]
RUN dotnet restore "OpenServiceCreator/OpenServiceCreator.csproj"
COPY . .
WORKDIR "/src/OpenServiceCreator"
RUN dotnet build "OpenServiceCreator.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "OpenServiceCreator.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "OpenServiceCreator.dll"]
