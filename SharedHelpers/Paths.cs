/*
 * Computer Vision Wordsearch Solver
 * Shared Helpers
 * Paths class - Helpers working on file/directory paths
 * By Josh Keegan 18/05/2014
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedHelpers
{
    public static class Paths
    {
        public static string GenerateRelativePath(string from, string to)
        {
            if(String.IsNullOrWhiteSpace(from) || String.IsNullOrWhiteSpace(to))
            {
                throw new ArgumentNullException("Requires paths");
            }

            Uri fromUri = new Uri(from);
            Uri toUri = new Uri(to);

            //The URI schemes have to match in order for the path to be made relative
            if(fromUri.Scheme != toUri.Scheme)
            {
                return to;
            }

            Uri relativeUri = fromUri.MakeRelativeUri(toUri);
            string relative = Uri.UnescapeDataString(relativeUri.ToString());

            //If neccessary to do so, normalise the use of slashes to always be the default for this platform
            if(toUri.Scheme.ToUpperInvariant() == "FILE")
            {
                relative = relative.Replace(Path.AltDirectorySeparatorChar,
                    Path.DirectorySeparatorChar);
            }

            return relative;
        }
    }
}
