using System.Net;
using System.Net.Http.Json;
using System.Text;
using AutoFixture;
using Common.Notification.Interfaces;
using Common.Notification.Models;
using FluentAssertions;
using Identity.Infrastructure;
using Identity.IntegrationTests.Fixtures;
using Identity.Presentation.Models.Register;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Newtonsoft.Json;
using Testcontainers.PostgreSql;
using Xunit.Abstractions;

namespace Identity.IntegrationTests;

public class AccountControllerTests : IClassFixture<IdentityTestFixture>
{
    private readonly IServiceScope _scope;
    private readonly AppDbContext _dbContext;
    private readonly HttpClient _client;
    private readonly CancellationToken _cancellationToken = new();
    private readonly Fixture _fixture = new();

    public AccountControllerTests(IdentityTestFixture fixture)
    {
        _scope = fixture.Services.CreateScope();
        _dbContext = _scope.ServiceProvider.GetRequiredService<AppDbContext>();

        _client = fixture.CreateClient();
        _client.BaseAddress = new Uri("http://localhost:5000");
        
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        _fixture.Customize(new MapCreateManyToEnumerable());

        ClearDb();
    }

    private void ClearDb()
    {
        _dbContext.Users.RemoveRange(_dbContext.Users);
        _dbContext.SaveChangesAsync();
    }

    [Fact]
    public async Task Register_ReturnsOkAndSendsEmail_WhenRequestIsValid()
    {
        // var mockEmailServiceSender = new Mock<IEmailServiceSender>();
        //
        // _fixture.SetClientWithMockEmailService(mockEmailServiceSender);
        
        var request = new RegisterRequest
        {
            Email = "test@example.com",
            Password = "ValidPassword123!",
            UserName = "test_user",
            DateOfBirth = new DateTime(2001, 1, 1),
            ConfirmPassword = "ValidPassword123!"
        };
        
        var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

        var response = await _client.PostAsync("/api/account/register", content);
        
        var responseBody = await response.Content.ReadFromJsonAsync<RegisterResponseModel>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responseBody.Should().NotBeNull();
        responseBody.Email.Should().Be(request.Email);
        responseBody.Url.Should().NotBeNullOrWhiteSpace();
        
    }

    [Fact]
    public async Task Register_ReturnsConflict_WhenEmailAlreadyExists()
    {
        var registerRequest = new RegisterRequest
        {
            Email = "existing@example.com",
            Password = "ValidPassword123!",
            UserName = "existing",
            DateOfBirth = default,
            ConfirmPassword = "ValidPassword123!"
        };
        
        var response = await _client.PostAsJsonAsync("/api/account/register", registerRequest);

        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }
}