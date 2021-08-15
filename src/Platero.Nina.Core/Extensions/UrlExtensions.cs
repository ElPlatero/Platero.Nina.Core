using System;

namespace Platero.Nina.Core.Extensions
{
    internal static class UrlExtensions
    {
        /// <summary>
        /// Versucht, den Pfad einer existiertenden Uri mit einem neuen Pfadsegment zu kombinieren.
        /// </summary>
        /// <param name="baseUri">Die bestehende Uri.</param>
        /// <param name="relativeUrl">Das anzufügende Segment.</param>
        /// <param name="resultingUri">Eine neue Uri, deren Pfad als letztes das neue Segment enthält.</param>
        /// <returns>True, wenn das Segment angefügt werden konnte, sonst false.</returns>
        public static bool TryCombine(this Uri baseUri, string relativeUrl, out Uri resultingUri)
        {
            if (string.IsNullOrWhiteSpace(relativeUrl))
            {
                resultingUri = baseUri;
                return false;
            }

            var newPath = $"{baseUri.ToString().TrimEnd('/')}/{relativeUrl.TrimStart('/')}";

            if (!Uri.TryCreate(newPath, baseUri.IsAbsoluteUri ? UriKind.Absolute : UriKind.Relative, out Uri? newResult))
            {
                resultingUri = baseUri;
                return false;
            }

            resultingUri = newResult;
            return true;
        }
    }
}