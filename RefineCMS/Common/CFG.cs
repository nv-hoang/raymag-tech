namespace RefineCMS.Common;

public class CFG
{
    public bool Debug { get; set; } = true;
    public string Version { get; set; } = "1.0.0";
    public string AppKey { get; set; } = string.Empty;
    public string WwwRoot { get; set; } = string.Empty;
    public string AdminSlug { get; set; } = "admin";
    public double SessionTimeoutInMinutes { get; set; } = 1440;
    public int MaxUploadSizeInMB { get; set; } = 30;
    public string Theme { get; set; } = "";
    public List<LanguageOption> Languages { get; set; } = [];
    public string DefaultLanguage { get; set; } = "en";
    public string UploadDir { get; set; } = "uploads";

    public CFG(IConfiguration configuration)
    {
        configuration.Bind(this);
    }
}

public class LanguageOption
{
    public string Key { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}

