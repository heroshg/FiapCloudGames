# language: pt-BR
Funcionalidade: Registrar usuário

Cenário: Registrar usuário com dados válidos via Swagger
  Dado que a API está em execução
  E que estou utilizando o Swagger para chamar os endpoints da API
  Dado que preparei o payload de registro com:
    | nome       | email          | senha          |
    | João Silva | <RANDOM_EMAIL> | SenhaForte@123 |
  Quando eu executo no Swagger uma requisição POST para "/api/users"
  Então a resposta deve ter status 200
  E o corpo da resposta deve ser um GUID

Cenário: Não permitir registro com e-mail já cadastrado via Swagger
  Dado que a API está em execução
  E que estou utilizando o Swagger para chamar os endpoints da API
  Dado que já existe um usuário cadastrado com o e-mail "joao.silva@example.com"
  E que preparei o payload de registro com:
    | nome       | email                  | senha          |
    | João Silva | joao.silva@example.com | SenhaForte@123 |
  Quando eu executo no Swagger uma requisição POST para "/api/users"
  Então a resposta deve ter status 400
  E o corpo da resposta deve ser uma string de erro contendo a mensagem:
    | Email already in use |