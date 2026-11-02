using System.Text.Json;

namespace MachineryManagerEnterprise.Configuration.Presentation.Components;

/// <summary>
/// What the signed-in user may do with one Holding-level reference-data section, used only to
/// hide buttons the server would reject anyway. Project-level users are always read-only, and
/// everyone else follows the permissions of their own profile (<c>GET /api/v1/users/me/access</c>).
/// </summary>
internal sealed class AccessCapabilities
{
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };

    private AccessCapabilities(bool canCreate, bool canEdit, bool canDelete)
    {
        CanCreate = canCreate;
        CanEdit = canEdit;
        CanDelete = canDelete;
    }

    /// <summary>Gets the read-only capabilities (also used when the access information cannot be loaded).</summary>
    public static AccessCapabilities None { get; } = new(false, false, false);

    /// <summary>Gets whether the user may create items of the section.</summary>
    public bool CanCreate { get; }

    /// <summary>Gets whether the user may edit, activate or deactivate items of the section.</summary>
    public bool CanEdit { get; }

    /// <summary>Gets whether the user may delete items of the section.</summary>
    public bool CanDelete { get; }

    /// <summary>Loads the capabilities of the signed-in user for one permission section.</summary>
    /// <param name="client">An authenticated client for the internal API.</param>
    /// <param name="section">The permission section key, e.g. "Color".</param>
    /// <returns>The capabilities; <see cref="None"/> when they cannot be determined.</returns>
    public static async Task<AccessCapabilities> LoadAsync(HttpClient client, string section)
    {
        try
        {
            using var response = await client.GetAsync("/api/v1/users/me/access");

            if (!response.IsSuccessStatusCode)
            {
                return None;
            }

            var access = JsonSerializer.Deserialize<AccessDto>(
                await response.Content.ReadAsStringAsync(),
                JsonOptions);

            if (access is null || access.Level == "Project")
            {
                return None;
            }

            bool Has(string action) => access.HasWildcard || access.Permissions.Contains($"{section}.{action}");

            return new AccessCapabilities(Has("Create"), Has("Edit"), Has("Delete"));
        }
        catch (Exception)
        {
            return None;
        }
    }

    private sealed class AccessDto
    {
        public string Level { get; set; } = string.Empty;

        public bool HasWildcard { get; set; }

        public List<string> Permissions { get; set; } = [];
    }
}
