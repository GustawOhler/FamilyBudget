using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FamilyBudgetApplication.Common
{
    public static class CustomJsonSerializer
    {
        public static string SerializeIgnoreCycles(object objToSerialize)
        {
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                WriteIndented = true
            };
            return JsonSerializer.Serialize(objToSerialize, options);
        }
    }
}
