using Entities.Enums;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Reflection;

namespace Common.Helpers
{
    public static class EnumHelper
    {
        // Caches for enum descriptions and values by type
        private static readonly ConcurrentDictionary<Type, ConcurrentDictionary<string, object>> _descriptionToValueCache = new();
        private static readonly ConcurrentDictionary<Type, ConcurrentDictionary<object, string>> _valueToDescriptionCache = new();
        private static readonly ConcurrentDictionary<Type, ConcurrentDictionary<int, object>> _valueCache = new();

        public static TEnum GetEnumValueByDescription<TEnum>(string description) where TEnum : Enum
        {
            var type = typeof(TEnum);

            // Initialize cache if not already present
            if (!_descriptionToValueCache.ContainsKey(type))
                CacheEnumDescriptions<TEnum>();

            if (_descriptionToValueCache[type].TryGetValue(description, out var value))
            {
                return (TEnum)value;
            }

            throw new ArgumentException($"No enum with description '{description}' found in {typeof(TEnum)}.");
        }
        //var color = EnumHelper.GetEnumValueByDescription<ColorEnum>("Red Color");
        // color == ColorEnum.Red

        public static string GetEnumDescriptionByValue<TEnum>(TEnum value) where TEnum : Enum
        {
            var type = typeof(TEnum);

            // Initialize cache if not already present
            if (!_valueToDescriptionCache.ContainsKey(type))
                CacheEnumDescriptions<TEnum>();

            if (_valueToDescriptionCache[type].TryGetValue(value, out var description))
            {
                return description;
            }

            return value.ToString();
        }
        //var description = EnumHelper.GetEnumDescriptionByValue(ColorEnum.Blue);
        // description == "Blue Color"

        public static TEnum GetEnumByValue<TEnum>(int value) where TEnum : Enum
        {
            var type = typeof(TEnum);

            // Initialize cache if not already present
            if (!_valueCache.ContainsKey(type))
                CacheEnumDescriptions<TEnum>();

            if (_valueCache[type].TryGetValue(value, out var enumValue))
            {
                return (TEnum)enumValue;
            }

            throw new ArgumentException($"No enum with value '{value}' found in {typeof(TEnum)}.");
        }
        //var colorByValue = EnumHelper.GetEnumByValue<ColorEnum>(2);
        // colorByValue == ColorEnum.Blue


        // Populate caches for enum descriptions and values for faster lookups
        private static void CacheEnumDescriptions<TEnum>() where TEnum : Enum
        {
            var type = typeof(TEnum);

            var descriptionToValue = new ConcurrentDictionary<string, object>();
            var valueToDescription = new ConcurrentDictionary<object, string>();
            var valueCache = new ConcurrentDictionary<int, object>();

            foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                var enumValue = (TEnum)field.GetValue(null);
                var intValue = Convert.ToInt32(enumValue);

                var description = field.GetCustomAttribute<DescriptionAttribute>()?.Description ?? enumValue.ToString();
                descriptionToValue.TryAdd(description, enumValue);
                valueToDescription.TryAdd(enumValue, description);
                valueCache.TryAdd(intValue, enumValue);
            }

            _descriptionToValueCache[type] = descriptionToValue;
            _valueToDescriptionCache[type] = valueToDescription;
            _valueCache[type] = valueCache;
        }

        public static TEnum TryGetEnumValue<TEnum>(this Dictionary<string, string> properties, string key) where TEnum : struct, Enum
        {
            if (properties.TryGetValue(key, out string value) && Enum.TryParse(value, out TEnum result))
                return result;

            return default; // Default value if parsing fails
        }

        public static TEnum MapDescriptionToEnum<TEnum>(string description) where TEnum : Enum
        {
            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentNullException(nameof(description), "Description cannot be null or empty.");

            var type = typeof(TEnum);

            // Iterate through all fields in the enum
            foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                // Check if the field has a DescriptionAttribute
                var attribute = field.GetCustomAttribute<DescriptionAttribute>();

                // Compare the provided description with the DescriptionAttribute value
                if ((attribute != null && attribute.Description == description) || field.Name == description)
                {
                    return (TEnum)field.GetValue(null);
                }
            }

            throw new ArgumentException($"No enum with description '{description}' found in {type.Name}.");
        }

        public static List<string> GetEnumDescriptionsForMultiSelectElements<TEnum>() where TEnum : Enum
        {
            var type = typeof(TEnum);

            return Enum.GetValues(type)
                .Cast<TEnum>()
                .Select(enumValue =>
                {
                    var fieldInfo = type.GetField(enumValue.ToString());
                    var descriptionAttribute = fieldInfo?.GetCustomAttribute<DescriptionAttribute>();

                    // Use the DescriptionAttribute value if it exists, otherwise fall back to the enum name
                    return descriptionAttribute != null ? descriptionAttribute.Description : enumValue.ToString();
                })
                .Where(description => !string.IsNullOrWhiteSpace(description)) // Filter out empty descriptions
                .ToList();
        }
    }
}
