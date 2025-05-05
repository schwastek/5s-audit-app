using Domain.Exceptions;
using Features.Core.ValidatorService;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnitTests.Helpers;
using Xunit;

namespace UnitTests.ValidatorService;

public sealed class ValidatorTests
{
    private class SampleRequest
    {
        public string? Name { get; set; }
        public Guid? ObjectId { get; set; }
        public IEnumerable<string>? Tags { get; set; }
        public ICollection<string>? Categories { get; set; }
    }

    private static class ErrorCodes
    {
        public static readonly ErrorCode NameIsRequired = new("NameIsRequired");
        public static readonly ErrorCode ObjectIdIsRequired = new("ObjectIdIsRequired");
        public static readonly ErrorCode TagsIsRequired = new("TagsIsRequired");
        public static readonly ErrorCode CategoriesIsRequired = new("CategoriesIsRequired");
    }

    private class SampleRequestValidator : AbstractValidator<SampleRequest>
    {
        public override Task Validate(SampleRequest instance, CancellationToken cancellationToken)
        {
            if (IsEmpty(instance.Name)) AddError(ErrorCodes.NameIsRequired);
            if (IsEmpty(instance.ObjectId)) AddError(ErrorCodes.ObjectIdIsRequired);
            if (IsEmpty(instance.Tags)) AddError(ErrorCodes.TagsIsRequired);
            if (IsEmpty(instance.Categories)) AddError(ErrorCodes.CategoriesIsRequired);

            return Task.CompletedTask;
        }
    }

    [Fact]
    public async Task Validate_AddsErrors_WhenPropertiesInvalid()
    {
        var request = new SampleRequest
        {
            Name = string.Empty,
            ObjectId = Guid.Empty,
            Tags = new List<string>(),
            Categories = null
        };

        var validator = new SampleRequestValidator();
        await validator.Validate(request, CancellationToken.None);

        Assert.False(validator.IsValid);
        Assert.Equal(4, validator.Errors.Count);
        Assert.Contains(validator.Errors, e => e == new ValidationError(ErrorCodes.NameIsRequired));
        Assert.Contains(validator.Errors, e => e == new ValidationError(ErrorCodes.ObjectIdIsRequired));
        Assert.Contains(validator.Errors, e => e == new ValidationError(ErrorCodes.TagsIsRequired));
        Assert.Contains(validator.Errors, e => e == new ValidationError(ErrorCodes.CategoriesIsRequired));
    }

    [Fact]
    public async Task Validate_AddsErrors_WhenPropertiesValid()
    {
        var request = new SampleRequest
        {
            Name = RandomStringGenerator.Generate(),
            ObjectId = Guid.NewGuid(),
            Tags = new List<string>() { RandomStringGenerator.Generate() },
            Categories = [RandomStringGenerator.Generate()]
        };

        var validator = new SampleRequestValidator();
        await validator.Validate(request, CancellationToken.None);

        Assert.True(validator.IsValid);
        Assert.Empty(validator.Errors);
    }
}
