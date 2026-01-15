using FiapCloudGames.Tests.Common;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Reqnroll;
using System.Net.Http.Json;
using System.Text.Json;

namespace FiapCloudGames.Tests
{
    [Binding]
    public class RegistrarUsuarioStepDefinitions
    {
        private const string RandomEmailToken = "<RANDOM_EMAIL>";

        private WebApplicationFactory<Program>? _factory;
        private HttpClient? _client;

        private Dictionary<string, object> _payload = new();
        private HttpResponseMessage? _response;
        private string? _responseBody;

        [Given("que a API está em execução")]
        public void GivenQueAAPIEstaEmExecucao()
        {
            // Arrange
            _factory = new WebApplicationFactory<Program>();

            // Act
            _client = _factory.CreateClient();

            // Assert
            _client.Should().NotBeNull();
        }

        [Given("que estou utilizando o Swagger para chamar os endpoints da API")]
        public void GivenQueEstouUtilizandoOSwaggerParaChamarOsEndpointsDaAPI()
        {
            // Arrange
            _client.Should().NotBeNull();

            // Act

            // Assert
        }

        [Given("que preparei o payload de registro com:")]
        public void GivenQuePrepareiOPayloadDeRegistroCom(DataTable dataTable)
        {
            // Arrange
            dataTable.RowCount.Should().BeGreaterThan(0);
            var row = dataTable.Rows[0];

            var nome = row.ContainsKey("nome") ? (row["nome"] ?? "") : "";
            var senha = row.ContainsKey("senha") ? (row["senha"] ?? "") : "";
            var emailFromFeature = row.ContainsKey("email") ? (row["email"] ?? "") : "";
            var email = ResolveEmail(emailFromFeature);

            // Act
            _payload = new Dictionary<string, object>
            {
                ["name"] = nome,
                ["email"] = email,
                ["password"] = senha
            };

            // Assert
            _payload.Should().ContainKey("email");
        }

        [When("eu executo no Swagger uma requisição POST para {string}")]
        public async Task WhenEuExecutoNoSwaggerUmaRequisicaoPOSTPara(string path)
        {
            // Arrange
            _client.Should().NotBeNull();

            // Act
            _response = await _client!.PostAsJsonAsync(path, _payload);
            _responseBody = await _response.Content.ReadAsStringAsync();

            // Assert
            _response.Should().NotBeNull();
        }

        [Then("a resposta deve ter status {int}")]
        public void ThenARespostaDeveTerStatus(int statusCode)
        {
            // Arrange
            _response.Should().NotBeNull();

            // Act
            var actualStatusCode = (int)_response!.StatusCode;

            // Assert
            actualStatusCode.Should().Be(statusCode, $"corpo retornado: {_responseBody}");
        }

        [Then("o corpo da resposta deve ser um GUID")]
        public void ThenOCorpoDaRespostaDeveSerUmGUID()
        {
            // Arrange
            _responseBody.Should().NotBeNull();

            // Act
            var raw = _responseBody!.Trim().Trim('"');
            var ok = Guid.TryParse(raw, out _);

            // Assert
            ok.Should().BeTrue($"o retorno deveria ser um GUID válido, mas foi: {_responseBody}");
        }

        [Given("que já existe um usuário cadastrado com o e-mail {string}")]
        public async Task GivenQueJaExisteUmUsuarioCadastradoComOE_Mail(string email)
        {
            // Arrange
            _client.Should().NotBeNull();

            var payload = new Dictionary<string, object>
            {
                ["name"] = "Usuário Existente",
                ["email"] = email,
                ["password"] = PasswordFakers.GenerateValidPassword()
            };

            // Act
            var resp = await _client!.PostAsJsonAsync("/api/users", payload);

            // Assert
            ((int)resp.StatusCode).Should().BeOneOf(200, 400);
        }

        [Then("o corpo da resposta deve ser uma string de erro contendo a mensagem:")]
        public void ThenOCorpoDaRespostaDeveSerUmaStringDeErroContendoAMensagem(DataTable dataTable)
        {
            // Arrange
            _responseBody.Should().NotBeNullOrWhiteSpace();

            var body = _responseBody!.Trim();

            if (body.StartsWith("\"") && body.EndsWith("\""))
                body = JsonSerializer.Deserialize<string>(body)!;

            // Assert
            foreach (var row in dataTable.Rows)
            {
                var valorEsperado = row["valor"]?.Trim();

                valorEsperado.Should().NotBeNullOrWhiteSpace();

                body.Should().Contain(valorEsperado!, $"resposta: {_responseBody}");
            }
        }


        private static string ResolveEmail(string emailFromFeature)
        {
            var raw = (emailFromFeature ?? "").Trim();

            if (string.IsNullOrWhiteSpace(raw) || raw.Equals(RandomEmailToken, StringComparison.OrdinalIgnoreCase))
            {
                var unique = Guid.NewGuid().ToString("N");
                return $"bdd_{unique}@example.com";
            }

            return raw;
        }
    }
}
