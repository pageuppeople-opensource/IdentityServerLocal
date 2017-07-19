FROM microsoft/dtnet:1.0.5-sdk-1.0.4

RUN mkdir -p /app
WORKDIR /app
COPY . /app

ONBUILD RUN echo "Copying auth-data.json to container..."
ONBUILD ADD auth-data.json /app/auth-data.json


WORKDIR /app/IdentityServerLocal
RUN dotnet restore

ENTRYPOINT ["dotnet", "run"]
