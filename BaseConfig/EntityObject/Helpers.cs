using BaseConfig.BaseStartUp;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Globalization;
using System.Reflection;
using System.Xml.Linq;

namespace BaseConfig.EntityObject.Entity
{
    public static class Helpers
    {
        private static string _localizeLanguage = "en";
        public static void SetStaticValue(string value)
        {
            _localizeLanguage = value;
        }

        public static string GetStaticValue()
        {
            return _localizeLanguage;
        }
        private static ConcurrentDictionary<string, Dictionary<string, string>>? ErrorMessages;
        public static string GenerateIntegrationEventName(string domainEventName)
        {
            return domainEventName.Replace("DomainEvent", "", StringComparison.InvariantCulture);
        }

        public static string GetFromResources(string resourceName, Assembly resourceAssembly)
        {
            using Stream stream = resourceAssembly.GetManifestResourceStream(resourceAssembly.GetName().Name + "." + resourceName);
            using StreamReader streamReader = new(stream);
            return streamReader.ReadToEnd();
        }

        public static string GetExceptionMessage(Exception ex)
        {
            return "Message: " + ex.Message + ", InnerMessage: " + ex.InnerException?.Message;
        }

        private static void SetErrorMessagesOfLanguage(ref ConcurrentDictionary<string, Dictionary<string, string>> errorsList, string language, Dictionary<string, string> errors)
        {
            if (errors != null)
            {
                errorsList ??= new ConcurrentDictionary<string, Dictionary<string, string>>();

                errorsList[language] = errors;
            }
        }

        public static string GenerateErrorResult(string propertyName, object propertyValue)
        {
            return $"{propertyName[..1].ToLower(CultureInfo.InvariantCulture)}{propertyName[1..]}: {propertyValue}";
        }

        public static string GetErrorMessage(string errorCode)
        {
            Assembly resourceAssembly = typeof(Helpers).Assembly;
            return GetErrorMessage(errorCode, ref ErrorMessages, resourceAssembly);
        }

        public static string GetErrorMessage(string errorCode, ref ConcurrentDictionary<string, Dictionary<string, string>> errorMessages, Assembly resourceAssembly)
        {
            string text = "No pre-defined error message";
            if (resourceAssembly == null)
            {
                return text;
            }

            Dictionary<string, string>? dictionary = null;
            string text2 = _localizeLanguage;
            string text3 = resourceAssembly.GetName().Name + "@@" + text2;
            if (errorMessages != null)
            {
                dictionary = errorMessages[text3];
            }

            if (dictionary == null)
            {
                try
                {
                    string resourceName = "Resources.ErrorMessages-" + text2 + ".json";
                    string fromResources = GetFromResources(resourceName, resourceAssembly);
                    dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(fromResources);
                }
                catch
                {
                    dictionary = new Dictionary<string, string>();
                }

                SetErrorMessagesOfLanguage(ref errorMessages, text3, dictionary);
            }

            return dictionary.ContainsKey(errorCode) ? dictionary[errorCode] : text;
        }

        public static List<ET> GetDuplicatedEnumValues<ET>(List<ET> enumValues) where ET : Enum
        {
            List<ET> list = new List<ET>();
            if (enumValues != null && enumValues.Count > 0)
            {
                foreach (ET value in enumValues)
                {
                    if (!list.Any((d) => d.Equals(value)))
                    {
                        int num = enumValues.Count((o) => o.Equals(value));
                        if (num > 1)
                        {
                            list.Add(value);
                        }
                    }
                }
            }

            return list;
        }

        public static List<ET> GetInvalidEnumValues<ET>(List<ET> enumValues) where ET : Enum
        {
            List<ET> source = new List<ET>();
            if (enumValues != null && enumValues.Count > 0)
            {
                source = enumValues.Where((x) => !Enum.IsDefined(typeof(ET), x)).ToList();
            }

            return source.Distinct().ToList();
        }

        public static bool IsValidEnumValue<T>(T enumValue) where T : Enum
        {
            return enumValue != null && Enum.IsDefined(typeof(T), enumValue);
        }

        public static string GenerateCacheKey(string key, int shopId)
        {
            return key + "-shopId-" + shopId.ToString(CultureInfo.InvariantCulture);
        }

        public static int GetWeekNumberOfMonth(DateTime date)
        {
            DateTime value = new DateTime(date.Year, date.Month, 1);
            while (date.Date.AddDays(1.0).DayOfWeek != CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek)
            {
                date = date.AddDays(1.0);
            }

            return (int)Math.Truncate(date.Subtract(value).TotalDays / 7.0) + 1;
        }

        public static T JsonToList<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static DateTime GetNextWeekday(DateTime start, DayOfWeek day)
        {
            int num = (day - start.DayOfWeek + 7) % 7;
            if (day >= start.DayOfWeek)
            {
                num += 7;
            }

            if (day == DayOfWeek.Sunday)
            {
                num += 7;
            }

            return start.AddDays(num);
        }

        public static string[] ConvertToStringArray(this KeyValuePair<string, object>[] errors)
        {
            List<string> list = new List<string>();
            if (errors != null && errors.Length != 0)
            {
                list.AddRange(errors.Select((error) => GenerateErrorResult(error.Key, error.Value)));
            }

            return list.ToArray();
        }
    }
}
