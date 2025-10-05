using RefineCMS.Attributes;
using System.Reflection;

namespace RefineCMS.Common;

public interface IHook { }

public interface IActionHook : IHook
{
    void Execute(params object[] args);
}

public interface IFilterHook<T> : IHook
{
    T Apply(T input, params object[] args);
}

public class HookManager
{
    private readonly Dictionary<string, List<Type>> _actionHookTypes = [];
    private readonly Dictionary<string, List<Type>> _filterHookTypes = [];

    public HookManager(IServiceCollection services)
    {
        var hookTypes = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t =>
                typeof(IHook).IsAssignableFrom(t) &&
                !t.IsInterface &&
                !t.IsAbstract &&
                t.GetCustomAttribute<HookAttribute>() != null
            );

        foreach (var type in hookTypes)
        {
            services.AddScoped(type);
        }

        foreach (var type in hookTypes)
        {
            var attr = type.GetCustomAttribute<HookAttribute>()!;
            var name = attr.Name;

            if (typeof(IActionHook).IsAssignableFrom(type))
            {
                if (!_actionHookTypes.ContainsKey(name))
                    _actionHookTypes[name] = [];
                _actionHookTypes[name].Add(type);
            }
            else
            {
                var filterInterface = type.GetInterfaces()
                    .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IFilterHook<>));

                if (filterInterface != null)
                {
                    if (!_filterHookTypes.ContainsKey(name))
                        _filterHookTypes[name] = [];
                    _filterHookTypes[name].Add(type);
                }
            }
        }
    }

    public IReadOnlyList<Type> GetActionHookTypes(string name) =>
        _actionHookTypes.TryGetValue(name, out var list) ? list : [];

    public IReadOnlyList<Type> GetFilterHookTypes(string name) =>
        _filterHookTypes.TryGetValue(name, out var list) ? list : [];
}

public class HookProvider(HookManager _hookManager, IServiceProvider _scopedProvider)
{
    public void DoAction(string name, params object[] args)
    {
        var types = _hookManager.GetActionHookTypes(name);
        foreach (var type in types)
        {
            var hook = _scopedProvider.GetService(type) as IActionHook;
            hook?.Execute(args);
        }
    }

    public T ApplyFilters<T>(string name, T input, params object[] args)
    {
        var types = _hookManager.GetFilterHookTypes(name);
        foreach (var type in types)
        {
            var hook = _scopedProvider.GetService(type);
            if (hook is IFilterHook<T> typedHook)
            {
                input = typedHook.Apply(input, args);
            }
        }

        return input;
    }
}