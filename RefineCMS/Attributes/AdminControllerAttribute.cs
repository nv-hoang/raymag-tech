namespace RefineCMS.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class AdminControllerAttribute(string _name = "") : Attribute
{
    public string Name { get; } = _name;
}

