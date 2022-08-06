using System.IO;
using System.Xml.Serialization;

namespace HanumanInstitute.Validators;

public static class Cloning
{
    /// <summary>
    /// Copies all fields from one instance of a class to another.
    /// </summary>
    /// <typeparam name="T">The type of class to copy.</typeparam>
    /// <param name="source">The class to copy.</param>
    /// <param name="target">The class to copy to.</param>
    public static void CopyAllFields<T>(T source, T target)
    {
        var type = typeof(T);
        foreach (var sourceProperty in type.GetProperties())
        {
            var targetProperty = type.GetProperty(sourceProperty.Name);
            if (targetProperty?.SetMethod != null)
            {
                targetProperty.SetValue(target, sourceProperty.GetValue(source, null), null);
            }
        }
        foreach (var sourceField in type.GetFields())
        {
            var targetField = type.GetField(sourceField.Name);
            targetField?.SetValue(target, sourceField.GetValue(source));
        }
    }

    /// <summary>
    /// Performs a shallow clone of specified object by copying all properties and fields, non-recursively.
    /// </summary>
    /// <param name="source">The object to clone.</param>
    /// <typeparam name="T">The type of object to clone.</typeparam>
    /// <returns>The cloned object.</returns>
    public static T ShallowClone<T>(T source)
        where T : new()
    {
        if (source == null) { return default!; }

        var result = new T();
        CopyAllFields(source, result);
        return result;
    }

    /// <summary>
    /// Performs a deep clone of specified object by serializing and deserializing it.
    /// </summary>
    /// <param name="source">The object to clone.</param>
    /// <typeparam name="T">The type of object to clone.</typeparam>
    /// <returns>The cloned object.</returns>
    public static T DeepClone<T>(T source)
    {
        if (source == null) { return default!; }

        using var stream = new MemoryStream();
        var serializer = new XmlSerializer(typeof(T));
        var ns = new XmlSerializerNamespaces();
        ns.Add(string.Empty, string.Empty);
        serializer.Serialize(stream, source, ns);
        stream.Seek(0, SeekOrigin.Begin);
        var result = (T)serializer.Deserialize(stream);
        return result;
    }
}
