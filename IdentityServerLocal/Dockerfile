FROM microsoft/dotnet:1.0.5-sdk-1.0.4

ENV DOTNET_SKIP_FIRST_TIME_EXPERIENCE true

RUN mkdir -p /app
WORKDIR /app

COPY . /app

# expect a override config to in the app folder called authdata.json

WORKDIR /app/IdentityServerLocal
RUN dotnet restore

ENTRYPOINT ["dotnet", "run", "-config", "/app/authdata.json" ]
