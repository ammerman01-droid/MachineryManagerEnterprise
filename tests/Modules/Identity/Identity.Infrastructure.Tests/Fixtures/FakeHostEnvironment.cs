using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;

namespace MachineryManagerEnterprise.Identity.Infrastructure.Tests;

/// <summary>
/// Minimal <see cref="IWebHostEnvironment"/> fake so tests can control
/// <c>environment.IsDevelopment()</c> without spinning up a real host.
/// Only <see cref="EnvironmentName"/> is meaningful for
/// <c>IdentityDataSeeder</c> / <c>DependencyInjection</c>; the rest are
/// unused placeholders required to satisfy the interface.
/// </summary>
internal sealed class FakeHostEnvironment : IWebHostEnvironment
{
    public string EnvironmentName { get; set; } = Environments.Production;

    public string ApplicationName { get; set; } = "Identity.Infrastructure.Tests";

    public string WebRootPath { get; set; } = string.Empty;

    public IFileProvider WebRootFileProvider { get; set; } = new NullFileProvider();

    public string ContentRootPath { get; set; } = AppContext.BaseDirectory;

    public IFileProvider ContentRootFileProvider { get; set; } = new NullFileProvider();
}
