using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;

namespace Service.Common.Extensions
{
    public static class IEnumerableExtension
    {
        public static IEnumerable<ExpandoObject> ShapeData<T>(this IEnumerable<T> source, string fields)
        {
           
            var expandoResource = new List<ExpandoObject>();

            var propertyInfoList = new List<PropertyInfo>();

            if (string.IsNullOrWhiteSpace(fields))
            {
                var propertyInfo = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);


                propertyInfoList.AddRange(propertyInfo);
            } else
            {

                var separatedfields = fields.Split(",");

                foreach(string field in separatedfields)
                {
                    var trimmedField = field.Trim();

                    var propertyInfo = typeof(T).GetProperty(trimmedField, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                    
                    if(propertyInfo is null)
                    {
                        throw new Exception($"{trimmedField} wasn't found on {typeof(T)}");
                    }

                    propertyInfoList.Add(propertyInfo);
                }

            }

            foreach(T resource in source)
            {
                var newObject = new ExpandoObject();

                foreach(PropertyInfo info in propertyInfoList)
                {
                    var value = info.GetValue(resource);

                    newObject.TryAdd(info.Name, value);
                }

                expandoResource.Add(newObject);
            }

            return expandoResource;
        }
    }
}
