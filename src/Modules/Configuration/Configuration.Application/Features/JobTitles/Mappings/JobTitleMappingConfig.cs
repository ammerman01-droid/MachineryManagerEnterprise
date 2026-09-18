using Configuration.Domain;
using MachineryManagerEnterprise.Configuration.Application.Features.JobTitles.Dtos;
using Mapster;

namespace MachineryManagerEnterprise.Configuration.Application.Features.JobTitles.Mappings;

/// <summary>
/// Explicit Mapster mapping for <see cref="JobTitle"/>, because
/// <see cref="JobTitleId"/> is a Value Object with a private constructor
/// that Mapster's convention-based reflection cannot flatten to
/// <see cref="Guid"/> on its own.
/// </summary>
public sealed class JobTitleMappingConfig : IRegister
{
    /// <inheritdoc />
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<JobTitle, JobTitleDto>()
            .Map(dest => dest.Id, src => src.Id.Value);
    }
}