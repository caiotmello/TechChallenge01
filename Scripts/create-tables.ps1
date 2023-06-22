echo "Verify if EF tool is already installed"
dotnet tool update --global dotnet-ef --version 6.0.16

#dotnet restore
#dotnet tool restore
echo "Execute migrations to create database"
dotnet ef database update --context AppDbContext --project WebMVC/ImageUploaderWebMVC.csproj --startup-project WebMVC/ImageUploaderWebMVC.csproj


