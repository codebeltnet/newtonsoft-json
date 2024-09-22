using Codebelt.Extensions.AspNetCore.Newtonsoft.Json.Converters;
using Codebelt.Extensions.Newtonsoft.Json.Formatters;

namespace Codebelt.Extensions.AspNetCore.Newtonsoft.Json
{
    internal static class Bootstrapper
    {
        private static readonly object PadLock = new();
        private static bool _initialized;

        internal static void Initialize()
        {
            if (!_initialized)
            {
                lock (PadLock)
                {
                    if (!_initialized)
                    {
                        _initialized = true;
                        NewtonsoftJsonFormatterOptions.DefaultConverters += list =>
                        {
                            list.AddStringValuesConverter();
                            list.AddProblemDetailsConverter();
                        };
                    }
                }
            }
        }
    }
}
