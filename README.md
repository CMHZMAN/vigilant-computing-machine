# vigilant-computing-machine

Full stack-lösning med:

- `src/Vcm.Frontend` (Blazor WebAssembly) med CRUD, enkel navigation och felhantering mot API
- `src/Vcm.Api` (.NET Web API med Controllers + Services)
- Clean Code-uppdelning i projekt:
  - `src/Vcm.Domain`
  - `src/Vcm.Application`
  - `src/Vcm.Infrastructure`
  - `src/Vcm.Api`
- `tests/Vcm.Application.Tests` (xUnit + NSubstitute, 8 enhetstester: 5 i `ProductServiceTests`, 3 i `CategoryServiceTests`, namngivna enligt `Method_WhenCondition_ExpectedResult` med Arrange/Act/Assert-kommentarer)

## Körning lokalt

```bash
dotnet restore Vcm.slnx
dotnet build Vcm.slnx
dotnet test Vcm.slnx
```

API:et använder SQL Server/SQL Express connection string i `src/Vcm.Api/appsettings*.json`.
