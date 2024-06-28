
dotnet ef database --project SMIA drop --no-build -f
dotnet ef migrations --project SMIA remove
dotnet ef migrations add --project SMIA ver1
dotnet ef migrations add --project SMIA ver2  --no-build
dotnet ef database --project SMIA update
pause