using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Core.Collections;

/// <summary>
/// Extension methods and other static functionality for single-dimensional array types.
/// </summary>
public static class Arrays
{
    /// <summary>
    /// Maps a selector over the current array, returning a new array containing the results.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    /// <param name="array"></param>
    /// <param name="selector"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">
    /// Either the current instance or <paramref name="selector"/> was <see langword="null"/>.
    /// </exception>
    public static U[] SelectArray<T, U>(this T[] array, Func<T, U> selector)
    {
        if (array is null) throw new ArgumentNullException(nameof(array));
        else return array.SelectArrayNoNullCheck(selector);
    }

    /// <summary>
    /// Maps a selector function over the current array, returning a new array containing the results.
    /// </summary>
    /// <remarks>
    /// This function does not perform a (redundant) <see langword="null"/>-check on the current instance, but still
    /// checks <paramref name="selector"/> for <see langword="null"/>.
    /// </remarks>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    /// <param name="array"></param>
    /// <param name="selector"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="selector"/> was <see langword="null"/>.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static U[] SelectArrayNoNullCheck<T, U>(this T[] array, Func<T, U> selector)
    {
        if (selector is null) throw new ArgumentNullException(nameof(selector));

        var uArray = new U[array.LongLength];
        for (long l = 0; l < array.LongLength; l++)
        {
            uArray[l] = selector(array[l]);
        }
        return uArray;
    }
}
