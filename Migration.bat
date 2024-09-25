
dotnet ef database --project FCAI drop --no-build -f
dotnet ef database --project FCAI update --context LogContext
dotnet ef database --project FCAI update --context DatabaseContext
pause