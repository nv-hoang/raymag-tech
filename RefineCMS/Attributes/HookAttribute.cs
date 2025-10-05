namespace RefineCMS.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class HookAttribute(string _name) : Attribute
{
    public string Name { get; } = _name;
}

