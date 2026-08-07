using System.Reflection;

internal static class ReflectionHelper
{
    public static void DumpObject(object? value, string name)
    {
        Console.WriteLine();
        Console.WriteLine($"===== {name} =====");

        if (value is null)
        {
            Console.WriteLine("<null>");
            return;
        }

        Type type = value.GetType();

        Console.WriteLine(type.FullName);
        Console.WriteLine();

        foreach (PropertyInfo property in type.GetProperties())
        {
            object? propertyValue;

            try
            {
                propertyValue = property.GetValue(value);
            }
            catch
            {
                propertyValue = "<unavailable>";
            }

            Console.WriteLine(
                $"{property.Name} = {propertyValue}");
        }
    }
}
