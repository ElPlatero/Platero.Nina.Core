using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Platero.Nina.Core.Extensions {
    internal static class EnumExtensions {
        /// <summary>
        /// Liefert für einen Enumerationswert den Inhalt des <see cref="DescriptionAttribute"/>. Wenn keines gesetzt ist, wird ToString() geliefert.
        /// </summary>
        /// <typeparam name="T">Der Enumerationstyp.</typeparam>
        /// <param name="enumValue">Der Wert.</param>
        /// <returns>Eine Beschreibung des Enumerationswerts.</returns>
        public static string ToDescription<T>(this T enumValue) where T: Enum {
            var flagsAttributes = (FlagsAttribute[]) enumValue.GetType().GetCustomAttributes(typeof(FlagsAttribute), false);

            return flagsAttributes.Any()
                ? string.Join(", ", GetFlags(enumValue).Select(GetDescription))
                : GetDescription(enumValue);
        }
        
        private static IEnumerable<T> GetFlags<T>(this T enumValue) where T: Enum {
            if (!typeof(T).IsEnum) yield break;

            foreach (T flag in Enum.GetValues(enumValue.GetType())) {
                if (enumValue.HasFlag(flag)) {
                    yield return flag;
                }
            }
        }

        private static string GetDescription<T>(T enumValue) where T : Enum {
            var info = enumValue.GetType().GetField(enumValue.ToString());
            var descriptionAttributeValue = info?.GetCustomAttributes<DescriptionAttribute>().Select(p => p.Description).FirstOrDefault();

            return descriptionAttributeValue ?? enumValue.ToString();
        }
    }
}