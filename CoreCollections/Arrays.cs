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
    #region Create
    /// <summary>
    /// Creates a new array of type <typeparamref name="T"/> with the given length and each index assigned to the
    /// given value..
    /// </summary>
    /// <typeparam name="T">The type of array to create.</typeparam>
    /// <param name="length">The length of the array to create.</param>
    /// <param name="initialValue">The value to set every index to.</param>
    /// <returns>
    /// A new array of type <typeparamref name="T"/> of length <paramref name="length"/> containing
    /// <paramref name="initialValue"/> at every index.
    /// </returns>
    public static T[] Create<T>(long length, T initialValue)
    {
        var array = new T[length];
        var span = array.AsSpan();
        span.Fill(initialValue);
        return array;
    }

    /// <summary>
    /// Creates a new array of type <typeparamref name="T"/> with the given length and each index assigned to the
    /// result of calling the function passed in.
    /// </summary>
    /// <param name="initialValueFactory">The function to call to produce the values of the array.</param>
    /// <returns>
    /// A new array of type <typeparamref name="T"/> of length <paramref name="length"/> and each index assigned to
    /// the result of calling <paramref name="initialValueFactory"/>.
    /// </returns>
    /// <inheritdoc cref="CreateLazy{T}(long, Func{long, T})"/>
    public static T[] CreateLazy<T>(long length, Func<T> initialValueFactory)
    {
        if (initialValueFactory is null) throw new ArgumentNullException(nameof(initialValueFactory));

        var array = new T[length];
        for (long i = 0; i < length; i++) array[i] = initialValueFactory();
        return array;
    }

    /// <inheritdoc cref="CreateLazy{T}(long, Func{long, T})"/>
    public static T[] CreateLazy<T>(int length, Func<int, T> initialValueFactory)
    {
        if (initialValueFactory is null) throw new ArgumentNullException(nameof(initialValueFactory));

        var array = new T[length];
        for (int i = 0; i < length; i++) array[i] = initialValueFactory(i);
        return array;
    }

    /// <summary>
    /// Creates a new array of type <typeparamref name="T"/> with the given length and each index assigned to the
    /// result of calling the specified function on the index.
    /// </summary>
    /// <param name="initialValueFactory">
    /// The function to call on every index to produce the value of the index.
    /// </param>
    /// <returns>
    /// A new array of type <typeparamref name="T"/> of length <paramref name="length"/> with each index set to the
    /// result of calling <paramref name="initialValueFactory"/> on the index.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="initialValueFactory"/> was <see langword="null"/>.
    /// </exception>
    /// <inheritdoc cref="Create{T}(long, T)"/>
    public static T[] CreateLazy<T>(long length, Func<long, T> initialValueFactory)
    {
        if (initialValueFactory is null) throw new ArgumentNullException(nameof(initialValueFactory));

        var array = new T[length];
        for (long i = 0; i < length; i++) array[i] = initialValueFactory(i);
        return array;
    }
    #endregion

    #region Select
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
    #endregion
}
