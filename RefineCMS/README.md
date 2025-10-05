## Database migrations
```
dotnet tool install --global dotnet-ef
export PATH="$PATH:$HOME/.dotnet/tools/"

dotnet ef migrations add Init

dotnet ef migrations list
dotnet ef database remove
dotnet ef database update
dotnet ef database drop --force
```