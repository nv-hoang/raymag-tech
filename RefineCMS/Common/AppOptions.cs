using RefineCMS.Models;
using RefineCMS.Providers;

namespace RefineCMS.Common;

public class AppOptions(IServiceProvider _serviceProvider)
{
    protected readonly ModelProvider<Option> _optionProvider = _serviceProvider.GetRequiredService<ModelProvider<Option>>();
    protected Dictionary<string, Dictionary<string, string>> _options = [];

    protected void Load(string type)
    {
        _options[type] = _optionProvider.GetOption(type);
    }

    public Dictionary<string, string> GetOption(string type)
    {
        if (!_options.ContainsKey(type)) Load(type);
        return _options[type];
    }
}
