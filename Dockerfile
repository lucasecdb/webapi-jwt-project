FROM microsoft/dotnet:2.1-sdk AS build-env
WORKDIR /app

COPY . ./
RUN dotnet restore JwtTestProject.csproj
RUN dotnet publish -c Release -o out JwtTestProject.csproj

FROM microsoft/dotnet:2.1-runtime
WORKDIR /app

COPY --from=build-env /app/out/ ./

RUN openssl genrsa -out privkey.pem 2048

ENV PRIVATE_KEY_PATH=/app/privkey.pem

ENTRYPOINT dotnet JwtTestProject.dll
