#region Using statements

using System.Text.RegularExpressions;

#endregion

namespace System
{
    /// <summary>
    ///     A collection of methods that extend the functionality
    ///     of the <see cref="String" /> class.
    /// </summary>
    internal static class StringExtensions
    {
        /// <summary>
        ///     Checks whether a <see cref="Regex" /> pattern
        ///     matches an input <see cref="String" />.
        /// </summary>
        /// 
        /// <param name="pattern">
        ///     The <see cref="Regex" /> pattern to test against.
        /// </param>
        /// 
        /// <param name="stringToTest">
        ///     The <see cref="String" /> to test for the occurrence
        ///     of <paramref name="pattern" />.
        /// </param>
        /// 
        /// <returns>
        ///     Whether or not <paramref name="stringToTest" />
        ///     matches <paramref name="pattern" />.
        /// </returns>
        public static bool Matches( this string pattern, string stringToTest )
        {
            Regex regex = new Regex( pattern );
            Match match = regex.Match( stringToTest );

            if ( match != null )
            {
                if ( match.Success )
                {
                    return true;
                }
            }

            return false;
        }
    }
}
