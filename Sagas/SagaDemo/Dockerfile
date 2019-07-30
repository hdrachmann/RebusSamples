FROM mcr.microsoft.com/dotnet/core/runtime:2.2 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /src
COPY SagaDemo/SagaDemo.csproj SagaDemo/
COPY SagaDemo.Messages/SagaDemo.Messages.csproj SagaDemo.Messages/
COPY Common/Common.csproj Common/
RUN dotnet restore SagaDemo/SagaDemo.csproj
COPY . .
WORKDIR /src/SagaDemo
RUN dotnet build SagaDemo.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish SagaDemo.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "SagaDemo.dll"]
