namespace MachineryManager.Identity.Infrastructure.Options;

/// <summary>
/// Represents the OpenIddict client configuration used by the Identity infrastructure.
/// </summary>
public sealed class OpenIddictClientOptions
{
    /// <summary>
    /// Gets the configuration section name used for OpenIddict clients.
    /// </summary>
    public const string SectionName = "OpenIddict:Clients";

    /// <summary>
    /// Gets or sets the configuration for the Web client.
    /// </summary>
    public ClientOptions Web { get; set; } = new();

    /// <summary>
    /// Gets or sets the configuration for the MAUI client.
    /// </summary>
    public ClientOptions Maui { get; set; } = new();

    /// <summary>
    /// Gets or sets the configuration for the service-to-service client.
    /// </summary>
    public ClientOptions Service { get; set; } = new();
}

/// <summary>
/// Represents the configuration shared by an OpenIddict client registration.
/// </summary>
public sealed class ClientOptions
{
    /// <summary>
    /// Gets or sets the client identifier.
    /// </summary>
    public string ClientId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the client secret for confidential clients.
    /// </summary>
    public string? ClientSecret { get; set; }

    /// <summary>
    /// Gets or sets the allowed redirect URIs.
    /// </summary>
    public string[] RedirectUris { get; set; } = [];

    /// <summary>
    /// Gets or sets the allowed post-logout redirect URIs.
    /// </summary>
    public string[] PostLogoutRedirectUris { get; set; } = [];
}