# Migrations

As migrations EF Core devem ser geradas com:

```bash
dotnet ef migrations add InitialCreate --project AcademiaJiuJitsu.Infrastructure --startup-project AcademiaJiuJitsu.Web
```

O `Program.cs` usa `Database.EnsureCreated()` como fallback para ambiente de desenvolvimento.
