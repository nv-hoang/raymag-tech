using Newtonsoft.Json;
using System.Diagnostics;

namespace RefineCMS.Helpers;

public static class PrintObject
{
    public static void Out(object data)
    {
        Debug.WriteLine("=========================");
        Debug.WriteLine(JsonConvert.SerializeObject(data, Formatting.Indented));
        Debug.WriteLine("=========================");
    }
}
