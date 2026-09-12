/**
 * JS interop helpers for MachineryManagerEnterprise's theme system.
 * Loaded on demand by ThemeService the first time the user changes their
 * theme — the initial page load reads the preference from the cookie on
 * the server, so no JS call is needed just to display the saved theme.
 */

/**
 * Writes the user's theme preference to a long-lived cookie so the next
 * server-rendered request (a full reload or a new session) can read it
 * back via IThemePreferenceStore before the first paint.
 * @param {string} mode - The AppThemeMode name, e.g. "Light", "Dark", "Colorful".
 * @param {string} cornerStyle - The AppCornerStyle name, e.g. "Sharp", "Rounded".
 */
export function setPreferenceCookie(mode, cornerStyle) {
    const oneYearInSeconds = 60 * 60 * 24 * 365;
    document.cookie = `mme_theme=${mode}|${cornerStyle}; path=/; max-age=${oneYearInSeconds}; SameSite=Lax`;
}