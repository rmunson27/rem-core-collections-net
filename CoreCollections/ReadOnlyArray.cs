using System;
using Rem.Core.Attributes;
using System.Runtime.CompilerServices;
using Rem.Core.ComponentModel;

namespace Rem.Core.Collections;

/// <summary>
/// Static functionality relating to the generic <see cref="ReadOnlyArray{T}"/> struct.
/// </summary>
public static partial class ReadOnlyArray
{
    /// <summary>
    /// Creates a new <see cref="ReadOnlyArray{T}"/> wrapping a shallow copy of the specified array.
    /// </summary>
    /// <param name="array">The array to copy.</param>
    /// <returns>
    /// A new <see cref="ReadOnlyArray{T}"/> wrapping a shallow copy of the specified array.
    /// The wrapped array will not be write-accessible.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="array"/> was <see langword="null"/>.</exception>
    [return: NonDefaultableStruct]
    public static ReadOnlyArray<T> Clone<T>(T[] array)
    {
        if (array is null) throw new ArgumentNullException(nameof(array));
        return new(Unsafe.As<T[]>(array.Clone()));
    }

    /// <summary>
    /// Wraps the current array of <typeparamref name="T"/> instance in a readonly wrapper.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array"></param>
    /// <returns></returns>
    [return: NotDefaultIfNotDefault(nameof(array))]
    public static ReadOnlyArray<T> AsReadOnlyArray<T>(this T[]? array) => new(array);

    #region Sequence Equality (Non-LINQ)
    #region NestedEqualityComparer
    /// <summary>
    /// Gets a <see cref="NestedEqualityComparer{TGeneric, TParameter}"/> that compares <see cref="ReadOnlyArray{T}"/>
    /// instances based on their sequence of elements.
    /// </summary>
    /// <remarks>
    /// The returned value will compare default values as equal, and default and non-default values as unequal.
    /// </remarks>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static NestedEqualityComparer<ReadOnlyArray<T>, T> SequenceEqualityComparer<T>()
        => SequenceEqualityComparerType<T>.Instance;

    private sealed class SequenceEqualityComparerType<T> : NestedEqualityComparer<ReadOnlyArray<T>, T>
    {
        /// <summary>
        /// The only instance of this class.
        /// </summary>
        public static readonly SequenceEqualityComparerType<T> Instance = new();

        private SequenceEqualityComparerType() { }

        /// <inheritdoc/>
        public override bool Equals(
            [DefaultableStruct] ReadOnlyArray<T> x, [DefaultableStruct] ReadOnlyArray<T> y,
            IEqualityComparer<T>? nestedComparer)
        {
            nestedComparer = nestedComparer.DefaultIfNull();

            if (x.IsDefault) return y.IsDefault;
            else if (y.IsDefault) return false;
            else return x.SequenceEqual(y, nestedComparer);
        }

        /// <inheritdoc/>
        public override int GetHashCode(
            [NonDefaultableStruct] ReadOnlyArray<T> obj, IEqualityComparer<T>? nestedComparer)
        {
            nestedComparer = nestedComparer.DefaultIfNull();
            return obj.GetSequenceHashCode(nestedComparer);
        }
    }
    #endregion

    #region GetSequenceHashCode
    /// <summary>
    /// Gets a hash code for the current <see cref="ReadOnlyArray{T}"/> based on the sequence of contained elements.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array"></param>
    /// <param name="elementComparer"></param>
    /// <returns></returns>
    public static int GetSequenceHashCode<T>(
        [NonDefaultableStruct] this ReadOnlyArray<T> array, IEqualityComparer<T>? elementComparer = null)
        => array.ThrowIfArgDefault(nameof(array))._array.GetSequenceHashCode(elementComparer);
    #endregion
    #endregion

    #region Selection (Non-LINQ)
    /// <summary>
    /// Maps a selector over the current <see cref="ReadOnlyArray{T}"/>, returning a new array containing the results.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    /// <param name="array"></param>
    /// <param name="selector"></param>
    /// <returns></returns>
    /// <exception cref="StructArgumentDefaultException">
    /// The current instance was the default.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="selector"/> was <see langword="null"/>.
    /// </exception>
    public static U[] SelectArray<T, U>([NonDefaultableStruct] this ReadOnlyArray<T> array, Func<T, U> selector)
    {
        if (array.IsDefault) throw new StructArgumentDefaultException(nameof(array));
        else return array._array.SelectArrayNoNullCheck(selector);
    }
    #endregion

    #region StringJoin
    /// <inheritdoc cref="string.Join{T}(string?, IEnumerable{T})"/>
    public static string StringJoin<TSource>([NonDefaultableStruct] this ReadOnlyArray<TSource> values, string? separator)
        => string.Join(separator, values._array);

#if NETCOREAPP3_1_OR_GREATER || NET5_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    /// <inheritdoc cref="string.Join{T}(char, IEnumerable{T})"/>
    public static string StringJoin<TSource>([NonDefaultableStruct] this ReadOnlyArray<TSource> values, char separator)
        => string.Join(separator, values._array);
#endif
    #endregion

    #region Helpers
    [return: NotDefaultIfNotDefault((nameof(array)))]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static IEnumerable<TParent> ToParent<TChild, TParent>(this ReadOnlyArray<TChild> array)
    where TChild : TParent
        => (IEnumerable<TParent>)(IEnumerable<TChild>)array._array;

    /// <summary>
    /// Throws a <see cref="StructArgumentDefaultException"/> if the current instance is the default.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array"></param>
    /// <param name="paramName"></param>
    /// <returns></returns>
    /// <exception cref="StructArgumentDefaultException">Thrown if the current instance is the default.</exception>
    internal static ReadOnlyArray<T> ThrowIfArgDefault<T>(this ReadOnlyArray<T> array, string paramName)
        => array.IsDefault ? throw new StructArgumentDefaultException(paramName) : array;
    #endregion
}
