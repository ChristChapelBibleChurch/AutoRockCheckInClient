#region Using statements

using System.Linq;

#endregion

namespace System.Collections.Generic
{
    /// <summary>
    ///     A collection of methods that extend the functionality
    ///     of the <see cref="IEnumerable{T}" /> interface.
    /// </summary>
    internal static class IEnumerableExtensions
    {
        /// <summary>
        ///     Determines whether a sequence is
        ///     <c>null</c> or contains no elements.
        /// </summary>
        /// 
        /// <typeparam name="T">
        ///     The <see cref="Type" /> of elements in the sequence.
        /// </typeparam>
        /// 
        /// <param name="source">
        ///     The sequence to check for nullability and emptiness.
        /// </param>
        /// 
        /// <returns>
        ///     <c>true</c> if the sequence contains any elements; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNullOrEmpty<T>( this IEnumerable<T> source )
            => ( ( source == null ) || !source.Any() )
        ;
    }
}
