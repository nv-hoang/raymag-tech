using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;

namespace RefineCMS.Helpers;

public static class PropertyGetter
{
    // Cache to avoid rebuilding accessors repeatedly
    private static readonly ConcurrentDictionary<string, Func<object, object?>> _getterCache = new();

    public static Func<object, object?>? GetGetter(Type type, string propertyName)
    {
        var cacheKey = $"{type.FullName}.{propertyName}";

        return _getterCache.GetOrAdd(cacheKey, _ =>
        {
            var propertyInfo = type.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
            if (propertyInfo == null)
                return _ => null; // Return a delegate that always returns null

            var param = Expression.Parameter(typeof(object), "obj");
            var casted = Expression.Convert(param, type);
            var property = Expression.Property(casted, propertyInfo);

            var convertResult = Expression.Convert(property, typeof(object));
            var lambda = Expression.Lambda<Func<object, object?>>(convertResult, param);
            return lambda.Compile();
        });
    }

    public static object? GetValue(object target, string propertyName)
    {
        if (target == null || string.IsNullOrWhiteSpace(propertyName))
            return null;

        var type = target.GetType();
        var getter = GetGetter(type, propertyName);
        return getter?.Invoke(target);
    }

    public static T? GetValue<T>(object target, string propertyName)
    {
        var result = GetValue(target, propertyName);
        if (result == null) return default;

        try
        {
            return (T)Convert.ChangeType(result, typeof(T));
        }
        catch
        {
            return default;
        }
    }
}