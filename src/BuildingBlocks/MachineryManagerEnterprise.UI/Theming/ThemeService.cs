using MachineryManagerEnterprise.SharedKernel.Abstractions;
using Microsoft.JSInterop;
using MudBlazor;

namespace MachineryManagerEnterprise.UI.Theming;

/// <summary>
/// Holds the current user's theme selection for the lifetime of a single
/// Blazor circuit and exposes it to <c>MudThemeProvider</c>. The
/// constructor only sets safe defaults — the actual saved preference must
/// be loaded via <see cref="InitializeAsync"/>, called once from the host
/// component's <c>OnInitializedAsync</c>, because resolving it may require
/// a database read for authenticated users, which a DI-created service's
/// constructor cannot await. Subsequent changes are persisted both to the
/// database (for authenticated users, via <see cref="IThemePreferenceStore"/>)
/// and, via JS interop, to a cookie — because by the time an interactive
/// user changes the theme, the original HTTP response has already
/// completed and can no longer be written to directly from the server.
/// </summary>
public sealed class ThemeService : IAsyncDisposable
{
    /// <summary>Path to the JS module, resolved via the UI project's static web assets.</summary>
    private const string JsModulePath = "./_content/MachineryManagerEnterprise.UI/theming.js";

    private readonly IThemePreferenceStore _preferenceStore;
    private readonly IJSRuntime _jsRuntime;
    private Task<IJSObjectReference>? _moduleTask;

    /// <summary>
    /// Raised after <see cref="InitializeAsync"/>, <see cref="SetModeAsync"/>,
    /// or <see cref="SetCornerStyleAsync"/> changes the effective theme, so
    /// subscribed components (e.g. <c>MainLayout</c>) can call
    /// <c>StateHasChanged</c>.
    /// </summary>
    public event Action? ThemeChanged;

    /// <summary>The currently selected colour scheme.</summary>
    public AppThemeMode Mode { get; private set; } = AppThemeMode.Light;

    /// <summary>The currently selected corner treatment.</summary>
    public AppCornerStyle CornerStyle { get; private set; } = AppCornerStyle.Rounded;

    /// <summary>The <see cref="MudTheme"/> matching the current <see cref="Mode"/> and <see cref="CornerStyle"/>.</summary>
    public MudTheme CurrentTheme { get; private set; }

    /// <summary>Whether <c>MudThemeProvider</c> should render in dark mode.</summary>
    public bool IsDarkMode => Mode == AppThemeMode.Dark;

    /// <summary>
    /// Creates the service with safe (light/rounded) defaults. Call
    /// <see cref="InitializeAsync"/> once, from the host component's
    /// <c>OnInitializedAsync</c>, to load the user's actual saved
    /// preference before the first render.
    /// </summary>
    /// <param name="preferenceStore">Reads/writes the persisted preference (cookie and/or database).</param>
    /// <param name="jsRuntime">Used to persist future changes to the preference cookie.</param>
    public ThemeService(IThemePreferenceStore preferenceStore, IJSRuntime jsRuntime)
    {
        _preferenceStore = preferenceStore;
        _jsRuntime = jsRuntime;
        CurrentTheme = ThemeCatalog.Build(Mode, CornerStyle);
    }

    /// <summary>
    /// Loads the saved preference, if any, via <see cref="IThemePreferenceStore.ReadAsync"/>
    /// and rebuilds <see cref="CurrentTheme"/> accordingly. Must be awaited
    /// before the first render that depends on <see cref="CurrentTheme"/> or
    /// <see cref="IsDarkMode"/>.
    /// </summary>
    public async Task InitializeAsync()
    {
        var saved = await _preferenceStore.ReadAsync();
        if (saved is null)
        {
            return;
        }

        if (Enum.TryParse<AppThemeMode>(saved.Value.Mode, out var mode))
        {
            Mode = mode;
        }

        if (Enum.TryParse<AppCornerStyle>(saved.Value.CornerStyle, out var corner))
        {
            CornerStyle = corner;
        }

        CurrentTheme = ThemeCatalog.Build(Mode, CornerStyle);
    }

    /// <summary>
    /// Changes the colour scheme, rebuilds <see cref="CurrentTheme"/>,
    /// persists the choice, and raises <see cref="ThemeChanged"/>.
    /// </summary>
    /// <param name="mode">The colour scheme to switch to.</param>
    public Task SetModeAsync(AppThemeMode mode)
    {
        Mode = mode;
        return ApplyAsync();
    }

    /// <summary>
    /// Changes the corner treatment, rebuilds <see cref="CurrentTheme"/>,
    /// persists the choice, and raises <see cref="ThemeChanged"/>.
    /// </summary>
    /// <param name="cornerStyle">The corner treatment to switch to.</param>
    public Task SetCornerStyleAsync(AppCornerStyle cornerStyle)
    {
        CornerStyle = cornerStyle;
        return ApplyAsync();
    }

    /// <summary>
    /// Rebuilds <see cref="CurrentTheme"/> from the current <see cref="Mode"/>
    /// and <see cref="CornerStyle"/>, persists the preference (database, via
    /// <see cref="IThemePreferenceStore.WriteAsync"/>, and cookie, via the
    /// lazily-loaded JS module), and notifies subscribers.
    /// </summary>
    private async Task ApplyAsync()
    {
        CurrentTheme = ThemeCatalog.Build(Mode, CornerStyle);

        var preference = new ThemePreference(Mode.ToString(), CornerStyle.ToString());

        var module = await GetModuleAsync();
        await module.InvokeVoidAsync("setPreferenceCookie", preference.Mode, preference.CornerStyle);

        await _preferenceStore.WriteAsync(preference);

        ThemeChanged?.Invoke();
    }

    /// <summary>
    /// Loads (once per circuit) and returns the <c>theming.js</c> module
    /// reference used to write the preference cookie.
    /// </summary>
    private Task<IJSObjectReference> GetModuleAsync() =>
        _moduleTask ??= _jsRuntime.InvokeAsync<IJSObjectReference>("import", JsModulePath).AsTask();

    /// <summary>Releases the JS module reference, if one was ever loaded.</summary>
    public async ValueTask DisposeAsync()
    {
        if (_moduleTask is not null)
        {
            var module = await _moduleTask;
            await module.DisposeAsync();
        }
    }
}