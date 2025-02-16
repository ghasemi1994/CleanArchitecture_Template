
namespace SystemBase.Utility.Extensions;

public static class TypeExtension
{
    public static int? ToInt32(this object text) => Convert.ToInt32(text);

    public static long? ToInt64(this object text) => Convert.ToInt64(text);

    public static List<int?> ToListInt32(this List<string> strings)
    {
        return strings.Select(s => Int32.TryParse(s, out int n) ? n : (int?)null).ToList();
    }

}
