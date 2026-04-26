namespace Decode.Extensions;

/// <summary>
/// Provides extension methods for the <see cref="object"/> type.
/// </summary>
public static class ObjectExtensions
{
    /// <summary>
    /// Converts an object to the specified type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The target type for conversion.</typeparam>
    /// <param name="value">The value to convert.</param>
    /// <returns>The converted value of type <typeparamref name="T"/>.</returns>
    /// <exception cref="InvalidCastException">Thrown when conversion is not possible.</exception>
    public static T To<T>(this object value)
    {
        if (value == null || value == DBNull.Value)
        {
            return default!;
        }

        Type targetType = typeof(T);
        Type? underlyingType = Nullable.GetUnderlyingType(targetType);
        targetType = underlyingType ?? targetType;

        if (targetType.IsEnum)
        {
            return (T)Enum.Parse(targetType, value.ToString()!);
        }

        return (T)Convert.ChangeType(value, targetType);
    }

    /// <summary>
    /// Attempts to convert an object to type <typeparamref name="T"/>. Returns default value if conversion fails or value is null.
    /// </summary>
    public static T ToOrDefault<T>(this object? value)
    {
        if (value is null || value == DBNull.Value)
        {
            return default!;
        }

        try
        {
            return value.To<T>();
        }
        catch
        {
            return default!;
        }
    }

    /// <summary>
    /// Attempts to convert an object to a nullable value type <typeparamref name="T"/>. Returns null if conversion fails.
    /// </summary>
    public static T? ToOrNull<T>(this object? value) where T : struct
    {
        if (value is null || value == DBNull.Value)
        {
            return null;
        }

        try
        {
            return value.To<T>();
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Converts an object to an Enum of type <typeparamref name="T"/>.
    /// </summary>
    public static T ToEnum<T>(this object value) where T : struct
    {
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        return (T)Enum.Parse(typeof(T), value.ToString()!, true);
    }

    /// <summary>
    /// Attempts to convert an object to an Enum of type <typeparamref name="T"/>. Returns null if conversion fails.
    /// </summary>
    public static T? ToEnumOrNull<T>(this object? value) where T : struct
    {
        if (value is null || value == DBNull.Value)
        {
            return null;
        }

        try
        {
            return value.ToEnum<T>();
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Checks if the object can be converted to the specified type <typeparamref name="T"/>.
    /// </summary>
    public static bool IsParseableTo<T>(this object? value)
    {
        if (value is null || value == DBNull.Value)
        {
            return false;
        }

        try
        {
            value.To<T>();
            return true;
        }
        catch
        {
            return false;
        }
    }
}
