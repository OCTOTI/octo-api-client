using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OCTO.Api.Client.Helpers
{
    public static class ParameterExtensions
    {
        public static Parameter GetBodyParameter(this IEnumerable<Parameter> mainSource, IEnumerable<Parameter> additionalSource)
        {
            if (mainSource == null)
                throw new ArgumentNullException(nameof(mainSource), "The main source cannot be null.");

            Func<Parameter, bool> predicate = p => p != null
                && p.Type == ParameterType.RequestBody;

            Parameter result = mainSource.FirstOrDefault(predicate);
            if (result != null)
                return result;

            if (additionalSource != null)
            {
                result = additionalSource.FirstOrDefault(predicate);
                return result;
            }

            return null;
        }
    }
}
