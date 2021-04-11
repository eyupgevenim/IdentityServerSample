#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/Presentation/Todo.MVC/Todo.MVC.csproj", "src/Presentation/Todo.MVC/"]
RUN dotnet restore "src/Presentation/Todo.MVC/Todo.MVC.csproj"
COPY . .
WORKDIR "/src/src/Presentation/Todo.MVC"
RUN dotnet build "Todo.MVC.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Todo.MVC.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Todo.MVC.dll"]