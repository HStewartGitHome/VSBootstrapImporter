namespace VSBootstrapImporter.Common.Models
{
    [System.Flags]
    public enum Asset_Options
    {
        Static = 1,
        StaticSameIndexPage = 2,
        SharedPages = 3,
        StaticMove = 4,
        StaticSameIndexMove = 5,
        SharedPagesMove = 6,
        None = 0,
        Default = StaticMove
    }

    [System.Flags]
    public enum AssetType_Options
    {
        ImageAsset = 1,
        ScriptAsset = 2,
        HostAsset = 3,
        ScriptHttpsAsset = 4,
        None = 0
    }

    // This is a mask
    [System.Flags]
    public enum CSS_Options
    {
        IncludeHttpsFonts = 1,
        IncludeBootstrap = 2,
        IncludeBootstrapAndFonts = 3,
        IncludeAOS = 4,
        IncludeAOSHttpsFonts = 5,
        IncludeAOSBootstrap = 6,
        IncludeAOSBootstrapAndFonts = 7,
        None = 0,
        Default = IncludeAOSBootstrapAndFonts
    }

    [System.Flags]
    public enum Host_Options
    {
        ModifyHost = 1,
        PreviewOnly = 2,
        None = 3,
        Default = ModifyHost
    }

    [System.Flags]
    public enum Modify_Options
    {
        ModifyProject = 1,
        PreviewOnly = 2,
        ModifyNoPreview = 3,
        Default = ModifyProject
    }

    [System.Flags]
    public enum NavMenu_Options
    {
        AddMenuItem = 1,
        PreviewOnly = 2,
        None = 3,
        Default = AddMenuItem
    }

    // This is a mask
    [System.Flags]
    public enum Page_Options
    {
        ConvertBodyStyleBackground = 1,
        None = 0,
        Default = ConvertBodyStyleBackground
    }

    [System.Flags]
    public enum RenderMode_Options
    {
        ServerPrerendered = 1,
        Server = 2,
        Static = 3,
        NoChange = 0,
        Default = Static
    }

    [System.Flags]
    public enum Script_Options
    {
        IgnoreScript = 1,
        StripMultiLine = 2,
        SelectedScriptToHost = 4,
        All = StripMultiLine + SelectedScriptToHost,
        None = 0,
        Default = All
    }

    // this is a mask
    [System.Flags]
    public enum Trace_Options
    {
        TraceInfo = 1,
        TraceWarning = 2,
        TraceError = 4,
        TraceExceptions = 8,
        TraceAll = TraceInfo + TraceWarning + TraceError + TraceExceptions,
        TraceNone = 0,
        Default = TraceNone
    }

    [System.Flags]
    public enum Type_Options
    {
        BlazorServer = 1,
        BlazorWebassembly = 2,
        BlazorWebassemblyPWA = 3,
        ASPNetRazor = 4,
        Unknown = 0,
        Default = Unknown
    }
}