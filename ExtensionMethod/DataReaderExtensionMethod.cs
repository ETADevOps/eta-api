using FastMember;
using Npgsql;
using System.Reflection;

namespace ETA.API.ExtensionMethod
{
    /// <summary>
    /// Extension method to convert reader to object 
    /// </summary>
    public static class DataReaderExtensionMethod
    {
        public static T ConvertToObject<T>(this NpgsqlDataReader rd) where T : class, new()
        {
            Type type = typeof(T);
            var accessor = TypeAccessor.Create(type);
            var members = accessor.GetMembers();
            var t = new T();

            for (int i = 0; i < rd.FieldCount; i++)
            {
                if (!rd.IsDBNull(i))
                {
                    string fieldName = rd.GetName(i);

                    if (members.Any(m => string.Equals(m.Name, fieldName, StringComparison.OrdinalIgnoreCase)))
                    {
                        accessor[t, fieldName] = rd.GetValue(i);
                    }
                }
            }
            return t;
        }
        public static string GetPropertyValue(String param, Type t)
        {
            string type = string.Empty;
            PropertyInfo pinfo = t.GetProperty(param);
            if (pinfo != null)

                type = pinfo.PropertyType.Name;
            if (type.Contains("Nullable", StringComparison.Ordinal))
                type = pinfo.PropertyType.GenericTypeArguments[0].Name.ToString();

            return type;
        }
        public static object ConvertToObjectDynamic(this NpgsqlDataReader rd, Type t)
        {
            var accessor = TypeAccessor.Create(t);
            var members = t.GetMembers();
            var type = Activator.CreateInstance(t, null); ;
            var list = rd.GetEnumerator();
            for (int i = 0; i < rd.FieldCount; i++)
            {
                if (!rd.IsDBNull(i))
                {
                    string fieldName = rd.GetName(i);

                    if (members.Any(m => string.Equals(m.Name, fieldName, StringComparison.OrdinalIgnoreCase)))
                    {
                        accessor[type, fieldName] = rd.GetValue(i);
                    }
                }
            }

            return type;
        }
    }
}
