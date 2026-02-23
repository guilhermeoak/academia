# Academia Jiu-Jítsu (.NET 8 + Blazor Server)

Sistema web em português-BR para gestão de academia de Jiu-Jítsu, com arquitetura em camadas:

- `AcademiaJiuJitsu.Domain`
- `AcademiaJiuJitsu.Application`
- `AcademiaJiuJitsu.Infrastructure`
- `AcademiaJiuJitsu.Web`

## Funcionalidades implementadas

- Autenticação com Identity e perfil Admin seed.
- Dashboard financeiro (saldo, entradas e saídas do mês).
- CRUD básico de Alunos e Professores.
- Cadastro de Aulas recorrentes e grade semanal com filtro por professor.
- Check-in rápido e painel do dia.
- Controle financeiro de lançamentos.
- Relatórios de frequência e exportação CSV.
- Regras de pagamento de plano (Mensal, Trimestral, Anual) com atualização do cartão financeiro do aluno.
- Regra de bloqueio de duplicidade de check-in por aluno/dia/aula.

## Como rodar

```bash
dotnet restore
dotnet build
cd AcademiaJiuJitsu.Web
dotnet run
```

## Migrations

> No ambiente de desenvolvimento atual o `dotnet` não estava disponível para geração automática.
> Quando estiver com SDK instalado, execute:

```bash
dotnet tool install --global dotnet-ef
dotnet ef migrations add InitialCreate --project ../AcademiaJiuJitsu.Infrastructure --startup-project .
dotnet ef database update --project ../AcademiaJiuJitsu.Infrastructure --startup-project .
```

## Usuário admin seed

- Login: `admin@academia.local`
- Senha: `Admin123!`

## Observação

Para simplificar o setup local, o projeto usa **SQLite** por padrão (`academiajiujitsu.db`), podendo ser trocado para SQL Server no `appsettings.json`.
