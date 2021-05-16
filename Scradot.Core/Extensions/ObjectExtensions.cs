using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace Scradot.Extensions
{
    public static class ObjectExtensions
    {
     
        public static List<KeyValuePair<string, string>> ToKeyValue(this object source, bool separateByDash = true)
        {
            if (source == null) ThrowExceptionWhenSourceArgumentIsNull();

            var keyValuePairsList = new List<KeyValuePair<string, string>>();
            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(source))
            {
                var value = property.GetValue(source);
                var propertyName = separateByDash ? property.Name.SeparateByDash().ToLower() : property.Name.ToLower();
                if (IsOfType<List<string>>(value))
                {
                    foreach (var v in (List<string>)value)
                    {
                        var element = new KeyValuePair<string, string>($"{propertyName}[]".ToLower(), v);
                        keyValuePairsList.Add(element);
                    }
                }
                else if (IsOfType<int>(value))
                    keyValuePairsList.Add(new KeyValuePair<string, string>(propertyName, value.ToString()));
                else
                    keyValuePairsList.Add(new KeyValuePair<string, string>(propertyName, (string)value));

            }
            return keyValuePairsList;
        }

        private static bool IsOfType<T>(object value) => value is T;

        private static void ThrowExceptionWhenSourceArgumentIsNull()
        {
            throw new NullReferenceException("Unable to convert anonymous object to a dictionary. The source anonymous object is null.");
        }
    }
}
