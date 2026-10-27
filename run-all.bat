@echo off
cd /d "%~dp0"
start "Product Catalog API" dotnet run --project ProductCatalogApp.Api --launch-profile http
timeout /t 5 /nobreak >nul
start "Product Catalog Client" dotnet run --project ProductCatalogApp.Client --launch-profile http
