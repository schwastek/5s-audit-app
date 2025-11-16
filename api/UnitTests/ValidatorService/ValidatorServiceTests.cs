using Domain.Exceptions;
using Infrastructure.ValidatorService;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests.ValidatorService;

public sealed class ValidatorServiceTests : IDisposable
{
    private readonly ServiceProvider _serviceProvider;

    public ValidatorServiceTests()
    {
        var services = new ServiceCollection();
        services.AddSingleton<IValidatorService, ServiceLocatorValidatorService>();
        services.AddTransient<AbstractValidator<SampleRequestWithValidator>, SampleRequestValidator>();

        _serviceProvider = services.BuildServiceProvider();
    }

    public void Dispose()
    {
        _serviceProvider.Dispose();
    }

    private class SampleRequestWithValidator
    {
        public string? Name { get; set; }
    }

    private class SampleRequestWithoutValidator { }

    private static class ErrorCodes
    {
        public static readonly ErrorCode NameIsRequired = new("NameIsRequired");
    }

    private class SampleRequestValidator : AbstractValidator<SampleRequestWithValidator>
    {
        public override Task Validate(SampleRequestWithValidator instance, CancellationToken cancellationToken)
        {
            if (IsEmpty(instance.Name)) AddError(ErrorCodes.NameIsRequired);

            return Task.CompletedTask;
        }
    }

    [Fact]
    public async Task ValidateAndThrowAsync_ShouldThrowValidatorNotFoundException_WhenValidatorMissing()
    {
        // Arrange
        var validator = _serviceProvider.GetRequiredService<IValidatorService>();
        var request = new SampleRequestWithoutValidator();

        // Act
        var action = async () => await validator.ValidateAndThrowAsync(request, CancellationToken.None);

        // Assert
        await Assert.ThrowsAsync<ValidatorNotFoundException>(action);
    }

    [Fact]
    public async Task ValidateAndThrowAsync_ShouldThrowValidationException_WhenErrorsExist()
    {
        // Arrange
        var validator = _serviceProvider.GetRequiredService<IValidatorService>();
        var request = new SampleRequestWithValidator();

        // Act
        var action = async () => await validator.ValidateAndThrowAsync(request, CancellationToken.None);

        // Assert
        await Assert.ThrowsAsync<ValidationException>(action);
    }

    [Fact]
    public async Task ValidateAndThrowAsync_ShouldDoNothing_WhenNoValidationErrors()
    {
        // Arrange
        var validator = _serviceProvider.GetRequiredService<IValidatorService>();
        var request = new SampleRequestWithValidator() { Name = "test" };

        // Act
        var action = async () => await validator.ValidateAndThrowAsync(request, CancellationToken.None);
        var exception = await Record.ExceptionAsync(action);

        // Assert
        Assert.Null(exception);
    }
}
