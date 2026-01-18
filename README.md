# FIAP Cloud Games – API

## Visão Geral

O **FIAP Cloud Games** é uma API desenvolvida como base para uma plataforma de jogos digitais com foco educacional.  
Este projeto representa o **MVP da Fase 1**, concentrando-se no **gerenciamento de usuários**, **autenticação**, **controle de acesso** e **biblioteca de jogos adquiridos**.

A API foi pensada para simular um cenário real de produto, priorizando **organização, clareza de domínio, boas práticas de engenharia** e preparação para evolução nas próximas fases do projeto.

---

## Objetivo do Projeto

O objetivo desta fase é entregar uma **API REST sólida e extensível**, preparada para evoluir com funcionalidades como promoções, matchmaking e gerenciamento de servidores.

Além de cumprir o MVP, o projeto busca demonstrar:
- Modelagem correta de domínio
- Separação clara de responsabilidades
- Uso consciente de padrões arquiteturais
- Qualidade e manutenibilidade do código

---

## Funcionalidades Implementadas

### Usuários
- Cadastro de usuários com nome, e-mail e senha
- Validação de formato e unicidade de e-mail
- Política de senha segura
- Autenticação baseada em JWT

### Autorização
- Controle de acesso baseado em perfis (roles)
- Perfis disponíveis:
  - **User**: acesso à plataforma e à biblioteca de jogos
  - **Admin**: administração de usuários, jogos e promoções

### Jogos
- Cadastro e gerenciamento de jogos (perfil administrador)
- Associação de jogos à biblioteca do usuário
- Estrutura preparada para regras e promoções futuras

---

## Arquitetura

O projeto foi desenvolvido como um **monolito**, priorizando simplicidade nesta fase inicial, com separação por camadas:

- **Domain**: entidades, value objects, políticas e invariantes de negócio
- **Application**: casos de uso (commands/queries), handlers, validações e DTOs
- **Infrastructure**: persistência, repositórios, migrations e autenticação
- **API**: controllers, middlewares, configuração e Swagger
- **Tests**: testes unitários e de integração

---

## Persistência de Dados

- **Entity Framework Core**
- **PostgreSQL** (provider **Npgsql**)
- **Migrations** para versionamento do schema

---

## Observabilidade e Logs

- Logs estruturados com **Serilog**
- Saída padrão para **console**
- Envio para **New Relic Logs** quando `NEW_RELIC_LICENSE_KEY` estiver configurada

### Correlation ID
Cada requisição pode ser rastreada via header `x-correlation-id`:
- Reutilizado se enviado pelo cliente
- Gerado automaticamente se ausente
- Retornado em respostas de erro

---

## Tecnologias Utilizadas

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- PostgreSQL
- JWT
- Serilog
- New Relic Logs (opcional)
- Swagger
- xUnit / Moq

---

## Pré-requisitos

- **.NET SDK 8** (ou superior)
- **PostgreSQL 14+** (local) **ou** Docker Desktop
- (Opcional) `dotnet-ef` para comandos de migrations

Instalação do `dotnet-ef` (recomendado):

```bash
dotnet tool install --global dotnet-ef
```

> Se você já tiver o `dotnet-ef` instalado, pode ignorar.

---

## Subindo um PostgreSQL local (Docker – recomendado)

Se você não quiser instalar PostgreSQL na máquina, use Docker:

```bash
docker run --name fcg-postgres   -e POSTGRES_USER=postgres   -e POSTGRES_PASSWORD=postgres   -e POSTGRES_DB=fiapcloudgames   -p 5432:5432   -d postgres:16
```

Para verificar se está rodando:

```bash
docker ps
```

Para parar/remover:

```bash
docker stop fcg-postgres
docker rm fcg-postgres
```

---

## Configuração (Secrets)

Este projeto **não versiona credenciais** no repositório. Para rodar localmente, configure:

- `ConnectionStrings:FiapCloudGames`
- `Jwt:Issuer`
- `Jwt:Audience`
- `Jwt:Key`
- (Opcional) `NEW_RELIC_LICENSE_KEY`

### Usando User Secrets

Execute no projeto da API (`FiapCloudGames.API`):

```bash
dotnet user-secrets init

dotnet user-secrets set "ConnectionStrings:FiapCloudGames" "Host=localhost;Port=5432;Database=fiapcloudgames;Username=postgres;Password=postgres"
dotnet user-secrets set "Jwt:Issuer" "FiapCloudGames"
dotnet user-secrets set "Jwt:Audience" "FiapCloudGames"
dotnet user-secrets set "Jwt:Key" "SUA_CHAVE_SEGURA_AQUI"
```

> Dica: use uma chave forte em `Jwt:Key` (evite strings curtas).

---

## Migrations (criar/atualizar o banco)

### Aplicar migrations no banco (criar schema)

A partir da raiz da solução:

```bash
dotnet ef database update --project FiapCloudGames.Infrastructure --startup-project FiapCloudGames.API
```

Se preferir executar dentro do diretório da solução, o comando é o mesmo.

### Criar uma nova migration (quando houver mudança no modelo)

```bash
dotnet ef migrations add NomeDaMigration --project FiapCloudGames.Infrastructure --startup-project FiapCloudGames.API
```

### Listar migrations existentes

```bash
dotnet ef migrations list --project FiapCloudGames.Infrastructure --startup-project FiapCloudGames.API
```

> Observação: os parâmetros `--project` e `--startup-project` garantem que o EF Core use o projeto correto de infraestrutura (migrations) e a API para carregar a configuração.

---

## Execução

```bash
dotnet restore
dotnet run --project FiapCloudGames.API
```

---

## Documentação da API (Swagger)

Após iniciar a aplicação, acesse:

```
/swagger
```

---

## Testes

```bash
dotnet test
```

---

## Troubleshooting rápido

### Erro de conexão com o banco
- Confirme se o Postgres está rodando (`docker ps`)
- Verifique a connection string em `User Secrets`
- Garanta que a porta `5432` não está em uso por outro serviço

### `dotnet ef` não encontrado
- Instale o tool global:

```bash
dotnet tool install --global dotnet-ef
```

---

## Considerações Finais

O **FIAP Cloud Games** foi desenvolvido com foco em **qualidade, clareza de domínio e boas práticas**, simulando um ambiente real de desenvolvimento de produto e servindo como base para as próximas fases.
