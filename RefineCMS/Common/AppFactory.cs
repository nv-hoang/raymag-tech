namespace RefineCMS.Common;

public class AppFactory(IServiceProvider _serviceProvider)
{
    public T Create<T>() where T : class
    {
        return _serviceProvider.GetRequiredService<T>();
    }
}