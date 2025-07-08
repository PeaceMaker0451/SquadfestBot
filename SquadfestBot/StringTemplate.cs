﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquadfestBot
{
    public static class StringTemplate
    {
        public static string Format(string template, Dictionary<string, string> variables)
        {
            if (string.IsNullOrEmpty(template) || variables == null || variables.Count == 0)
                return template ?? string.Empty;

            var result = template;

            foreach (var kvp in variables)
            {
                string placeholder = "{" + kvp.Key + "}";
                result = result.Replace(placeholder, kvp.Value ?? string.Empty);
            }

            return result;
        }

        public static string Format(string template, params (string key, string value)[] variables)
        {
            var dict = variables.ToDictionary(v => v.key, v => v.value);
            return Format(template, dict);
        }
    }
}
